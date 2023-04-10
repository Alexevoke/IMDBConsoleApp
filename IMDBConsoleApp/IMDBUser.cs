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
    public class AIDS
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
                Console.WriteLine("IsAdult: " + title.IsAdult);
                Console.WriteLine("Startyear: " + title.StartYear);
                Console.WriteLine("Endyear: " + title.EndYear);
                Console.WriteLine("Runtime(min): " + title.RuntimeMinutes);
                Console.WriteLine();
            }


        }

        //private List<Title> GetFirst500Titles()
        //{
        //    Connection connection = new Connection();

        //    string queryString = "SELECT TOP (500) tconst, primaryTitle, originalTitle, " +
        //        "startYear, endYear, runtimeMinutes FROM titleBasics";

        //    List<Title> titles = new List<Title>();

        //    using (SqlConnection conn = new SqlConnection(connection.ConnectionString))
        //    {
        //        SqlCommand cmd = new SqlCommand(queryString, conn);
        //        cmd.Connection.Open();

        //        SqlDataReader reader = cmd.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            Title t = ReadTitle(reader);
        //            titles.Add(t);
        //        }
        //    }
        //    return titles;
        //}

        private List<Title> TitleSearch(string? search)
        {
            Connection connection = new Connection();

            string queryString = "SELECT tconst, primaryTitle, originalTitle, isAdult, startYear, endYear, " +
                "runtimeMinutes FROM titleBasics WHERE primaryTitle LIKE @search " +
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

            return titles;
        }

        private Title ReadTitle(SqlDataReader reader)
        {
            Title title = new Title();

            if (!reader.IsDBNull(0))
                title.Tconst = reader.GetString(0);
            if (!reader.IsDBNull(1))
                title.PrimaryTitle = reader.GetString(1);
            if (!reader.IsDBNull(2))
                title.OriginalTitle = reader.GetString(2);
            if (!reader.IsDBNull(3))
                title.IsAdult = reader.GetBoolean(3);
            if (!reader.IsDBNull(4))
                title.StartYear = reader.GetInt32(4);
            if (!reader.IsDBNull(5))
                title.EndYear = reader.GetInt32(5);
            if (!reader.IsDBNull(6))
                title.RuntimeMinutes = reader.GetInt32(6);

            return title;
        }
        





    }
}
