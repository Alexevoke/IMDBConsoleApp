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
            //List<Title> titles = GetFirst500Titles();

            //foreach (Title title in titles)
            //{
            //    Console.WriteLine("Tconst: " + title.Tconst + " Primarytitle: " + title.PrimaryTitle +
            //        " Originaltitle: " + title.OriginalTitle + " Startyear: " + title.StartYear +
            //        " Endyear: " + title.EndYear + " Runtime(min): " + title.RuntimeMinutes);
            //}
            Console.WriteLine("Indtast titlen på den film du vil finde: ");
            string? search = Console.ReadLine();

            Console.WriteLine("Search:");
            List<Title> titles = TitleSearch(search);
            

            foreach (Title title in titles)
            {
                Console.WriteLine("Primarytitle: " + title.PrimaryTitle);
                Console.WriteLine("Originaltitle: " + title.OriginalTitle);
                Console.WriteLine("Titletype: " + title.TitleType);
                Console.WriteLine("IsAdult: " + title.IsAdult);
                Console.WriteLine("Startyear: " + title.StartYear);
                Console.WriteLine("Endyear: " + title.EndYear);
                Console.WriteLine("Runtime(min): " + title.RuntimeMinutes);
                Console.Write("Genres: ");
                foreach (string genre in title.Genres)
                {
                    Console.Write(genre + " ");
                }
                Console.WriteLine( );
                Console.WriteLine();
            }
            Console.WriteLine(titles.Count);

        }


        private List<Title> TitleSearch(string? search)
        {
            Connection connection = new Connection();

            string queryString = "SELECT tconst, titleType, primaryTitle, originalTitle, isAdult, startYear, endYear, " +
                "runtimeMinutes FROM VW_titleBasics WHERE primaryTitle LIKE @search " +
                "ORDER BY primaryTitle";

            List<Title> titles = new List<Title>();

            if (search == null)
            {
                return titles;
            }

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
        





    }
}
