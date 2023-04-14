using IMDBClassLibrary.Model;
using IMDBConsoleApp.IDGenerators;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDBConsoleApp.Queries
{
    public static class NameQueries
    {

        public static List<Person> PersonSearch(string? search)
        {
            List<Person> persons = new List<Person>();

            if (String.IsNullOrEmpty(search))
                return persons;

            Connection connection = new Connection();

            string queryString = "SELECT nameBasics_ID, primaryName, birthYear, deathYear " +
                "FROM VW_nameBasics WHERE primaryName LIKE @search " +
                "ORDER BY primaryName";

            using (SqlConnection conn = new SqlConnection(connection.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(queryString, conn);
                cmd.Parameters.AddWithValue("@search", "%" + search + "%");
                cmd.Connection.Open();


                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Person p = ReadPerson(reader);
                    persons.Add(p);
                }
            }

            List<Profession> professions = new List<Profession>();

            queryString = "SELECT nameBasics_ID, nameprimaryProfession_ID " +
                "FROM VW_nameBasicsProfession WHERE primaryName LIKE @search";

            using (SqlConnection conn = new SqlConnection(connection.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(queryString, conn);
                cmd.Parameters.AddWithValue("@search", "%" + search + "%");
                cmd.Connection.Open();


                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Profession p = ReadProfession(reader);
                    professions.Add(p);
                }
            }

            List<KnownForTitle> titles = new List<KnownForTitle>();

            queryString = "SELECT nameBasics_ID, primaryTitle " +
                "FROM VW_nameBasicsknownForTitles WHERE primaryName LIKE @search";

            using (SqlConnection conn = new SqlConnection(connection.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(queryString, conn);
                cmd.Parameters.AddWithValue("@search", "%" + search + "%");
                cmd.Connection.Open();


                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    KnownForTitle t = ReadKnownForTitle(reader);
                    titles.Add(t);
                }
            }

            foreach (Profession profession in professions)
            {
                if (professions.Count < 1)
                    break;

                if (!persons.Exists(k => k.Id == profession.Id))
                    continue;

                persons.Find(k => k.Id == profession.Id).PrimaryProfessions.Add(profession.ProfessionName);
            }

            foreach (KnownForTitle title in titles)
            {
                if (titles.Count < 1)
                    break;

                if (title.Title == null || title.Title == "meh")
                    continue;

                if (!persons.Exists(k => k.Id == title.Id))
                    continue;

                persons.Find(k => k.Id == title.Id).KnownForTitles.Add(title.Title);
            }

            return persons;


        }

        public static void AddPerson(Person person)
        {
            Connection connection = new Connection();

            string queryString = "INSERT INTO VW_nameBasics (primaryName, birthYear, deathYear) " +
                "VALUES (@primaryName, @birthYear, @deathYear)";

            using (SqlConnection conn = new SqlConnection(connection.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(queryString, conn);
                cmd.Parameters.AddWithValue("@primaryName", person.PrimaryName);
                cmd.Parameters.AddWithValue("@birthYear", person.BirthYear);
                cmd.Parameters.AddWithValue("@deathYear", person.DeathYear);
                cmd.Connection.Open();

                int rows = cmd.ExecuteNonQuery();

                if (rows != 1)
                {
                    throw new ArgumentException("Person has not been added");
                }
            }
            
            NameIDGenerator nameIDGenerator = new NameIDGenerator();
            person.Id = nameIDGenerator.Id;

            queryString = "INSERT INTO VW_nameBasicsProfession (nameBasics_ID, namePrimaryProfession_ID) " +
                "VALUES (@nameBasics_ID, @namePrimaryProfession_ID)";

            foreach (Professions profession in person.PrimaryProfessions)
            {
                using (SqlConnection conn = new SqlConnection(connection.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand(queryString, conn);
                    cmd.Parameters.AddWithValue("@nameBasics_ID", person.Id);
                    cmd.Parameters.AddWithValue("@namePrimaryProfession_ID", (int)profession + 1);
                    cmd.Connection.Open();

                    int rows = cmd.ExecuteNonQuery();

                    if (rows != 1)
                    {
                        throw new ArgumentException("Person has not been added");
                    }
                }
            }
        }


        private static Person ReadPerson(SqlDataReader reader)
        {
            Person person = new Person();

            if (!reader.IsDBNull(0))
                person.Id = reader.GetInt32(0);
            if (!reader.IsDBNull(1))
                person.PrimaryName = reader.GetString(1);
            if (!reader.IsDBNull(2))
                person.BirthYear = reader.GetInt32(2);
            if (!reader.IsDBNull(3))
                person.DeathYear = reader.GetInt32(3);

            return person;
        }

        private static Profession ReadProfession(SqlDataReader reader)
        {
            Profession profession = new Profession();

            if (!reader.IsDBNull(0))
                profession.Id = reader.GetInt32(0);
            if (!reader.IsDBNull(1))
                profession.ProfessionName = (Professions)reader.GetInt32(1) - 1;

            return profession;

        }

        private static KnownForTitle ReadKnownForTitle(SqlDataReader reader)
        {
            KnownForTitle title = new KnownForTitle();

            if (!reader.IsDBNull(0))
                title.Id = reader.GetInt32(0);
            if (!reader.IsDBNull(1))
                title.Title = reader.GetString(1);

            return title;
        }


    }
}
