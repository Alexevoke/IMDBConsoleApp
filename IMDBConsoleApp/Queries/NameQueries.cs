using IMDBClassLibrary.Model;
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

            string queryString = "SELECT nconst, primaryName, birthYear, deathYear " +
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

            queryString = "SELECT nconst, primaryProfession " +
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

            queryString = "SELECT nconst, primaryTitle " +
                "FROM VW_nameBasicsKnownTitle WHERE primaryName LIKE @search";

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

                if (profession.ProfessionName == null || profession.ProfessionName == "meh")
                    continue;

                if (!persons.Exists(k => k.Nconst == profession.Nconst))
                    continue;

                persons.Find(k => k.Nconst == profession.Nconst).PrimaryProffesions.Add(profession.ProfessionName);
            }

            foreach (KnownForTitle title in titles)
            {
                if (titles.Count < 1)
                    break;

                if (title.Title == null || title.Title == "meh")
                    continue;

                if (!persons.Exists(k => k.Nconst == title.Nconst))
                    continue;

                persons.Find(k => k.Nconst == title.Nconst).KnownForTitles.Add(title.Title);
            }

            return persons;


        }


        private static Person ReadPerson(SqlDataReader reader)
        {
            Person person = new Person();

            if (!reader.IsDBNull(0))
                person.Nconst = reader.GetString(0);
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
                profession.Nconst = reader.GetString(0);
            if (!reader.IsDBNull(1))
                profession.ProfessionName = reader.GetString(1);

            return profession;

        }

        private static KnownForTitle ReadKnownForTitle(SqlDataReader reader)
        {
            KnownForTitle title = new KnownForTitle();

            if (!reader.IsDBNull(0))
                title.Nconst = reader.GetString(0);
            if (!reader.IsDBNull(1))
                title.Title = reader.GetString(1);

            return title;
        }


    }
}
