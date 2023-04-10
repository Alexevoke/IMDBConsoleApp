using IMDBClassLibrary.Model;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;
using System.Threading.Channels;

namespace IMDBConsoleApp
{
    public class IMDBUser
    {

        

        public void Start()
        {
            //Console.WriteLine("Indtast titlen på den film du vil finde: ");
            //string? search = Console.ReadLine();

            //Console.WriteLine("Search:");
            //List<Title> titles = TitleSearch(search);


            //foreach (Title title in titles)
            //{
            //    Console.WriteLine("Primarytitle: " + title.PrimaryTitle);
            //    Console.WriteLine("Originaltitle: " + title.OriginalTitle);
            //    Console.WriteLine("Titletype: " + title.TitleType);
            //    Console.WriteLine("IsAdult: " + title.IsAdult);
            //    Console.WriteLine("Startyear: " + title.StartYear);
            //    Console.WriteLine("Endyear: " + title.EndYear);
            //    Console.WriteLine("Runtime(min): " + title.RuntimeMinutes);
            //    Console.Write("Genres: ");
            //    foreach (string genre in title.Genres)
            //    {
            //        Console.Write(genre + " | ");
            //    }
            //    Console.WriteLine();
            //    Console.WriteLine();
            //}
            //Console.WriteLine(titles.Count);

            Console.WriteLine("Indtast navnet på den person du vil finde: ");
            string? search = Console.ReadLine();

            Console.WriteLine("Search:");
            List<Person> persons = PersonSearch(search);

            foreach (Person person in persons)
            {
                Console.WriteLine("Name: " + person.PrimaryName);
                Console.WriteLine("Birthyear: " + person.BirthYear);
                Console.WriteLine("Deathyear: " + person.DeathYear);
                Console.Write("Professions: ");
                foreach (string profession in person.PrimaryProffesions)
                {
                    Console.Write(profession + " | ");
                }
                Console.WriteLine();
                Console.Write("Known for titles: ");
                foreach (string title in person.KnownForTitles)
                {
                    Console.Write(title + " | ");
                }
                Console.WriteLine();
                Console.WriteLine();

            }
            Console.WriteLine(persons.Count);

        }


        private List<Title> TitleSearch(string? search)
        {
            List<Title> titles = new List<Title>();

            if (String.IsNullOrEmpty(search))
                return titles;

            Connection connection = new Connection();

            string queryString = "SELECT tconst, titleType, primaryTitle, originalTitle, isAdult, startYear, endYear, " +
                "runtimeMinutes FROM VW_titleBasics WHERE primaryTitle LIKE @search " +
                "ORDER BY primaryTitle";          

            using (SqlConnection conn = new SqlConnection(connection.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(queryString,conn);
                cmd.Parameters.AddWithValue("@search", "%" + search + "%");
                cmd.Connection.Open();
                

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Title t = ReadTitle(reader);
                    titles.Add(t);
                }
            }

            List<Genre> titlegenre = new List<Genre>();

            queryString = "SELECT tconst, titleGenres FROM WV_titleBasicsGenre WHERE primaryTitle LIKE @search";

            using (SqlConnection conn = new SqlConnection(connection.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(queryString, conn);
                cmd.Parameters.AddWithValue("@search", "%" + search + "%");
                cmd.Connection.Open();


                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Genre g = ReadGenre(reader);
                    titlegenre.Add(g);
                }
            }

            foreach (Genre genre in titlegenre)
            {
                if (titlegenre.Count < 1)
                    break;

                if (genre.Genretype == null || genre.Genretype == "meh")
                    continue;

                if (!titles.Exists(k => k.Tconst == genre.Tconst))
                    continue;

                titles.Find(k => k.Tconst == genre.Tconst).Genres.Add(genre.Genretype);
            }

            return titles;
        }

        private List<Person> PersonSearch(string? search)
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

        private Title ReadTitle(SqlDataReader reader)
        {
            Title title = new Title();

            if (!reader.IsDBNull(0))
                title.Tconst = reader.GetString(0);
            if (!reader.IsDBNull(1))
                title.TitleType = reader.GetString(1);
            if (!reader.IsDBNull(2))
                title.PrimaryTitle = reader.GetString(2);
            if (!reader.IsDBNull(3))
                title.OriginalTitle = reader.GetString(3);
            if (!reader.IsDBNull(4))
                title.IsAdult = reader.GetBoolean(4);
            if (!reader.IsDBNull(5))
                title.StartYear = reader.GetInt32(5);
            if (!reader.IsDBNull(6))
                title.EndYear = reader.GetInt32(6);
            if (!reader.IsDBNull(7))
                title.RuntimeMinutes = reader.GetInt32(7);

            return title;
        }

        private Genre ReadGenre(SqlDataReader reader)
        {
            Genre genre = new Genre();

            if (!reader.IsDBNull(0))
                genre.Tconst = reader.GetString(0);
            if (!reader.IsDBNull(1))
                genre.Genretype = reader.GetString(1);

            return genre;
        }
        
        private Person ReadPerson(SqlDataReader reader)
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

        private Profession ReadProfession(SqlDataReader reader)
        {
            Profession profession = new Profession();

            if (!reader.IsDBNull(0))
                profession.Nconst = reader.GetString(0);
            if (!reader.IsDBNull(1))
                profession.ProfessionName = reader.GetString(1);

            return profession;

        }

        private KnownForTitle ReadKnownForTitle(SqlDataReader reader)
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
