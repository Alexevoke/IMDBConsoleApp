using IMDBClassLibrary.Model;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;

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
            Console.WriteLine("Search:");
            List<Title> titles = TitleSearch("Chinese");

            foreach (Title title in titles)
            {
                Console.WriteLine("Primarytitle: " + title.PrimaryTitle +
                    " Originaltitle: " + title.OriginalTitle + " Startyear: " + title.StartYear +
                    " Endyear: " + title.EndYear + " Runtime(min): " + title.RuntimeMinutes);
            }


        }

        private List<Title> GetFirst500Titles()
        {

            string queryString = "SELECT TOP (500) tconst, primaryTitle, originalTitle, " +
                "startYear, endYear, runtimeMinutes FROM titleBasics";

            List<Title> titles = new List<Title>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(queryString, conn);
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

        private List<Title> TitleSearch(string search)
        {

            string queryString = "SELECT TOP(500) tconst, primaryTitle, originalTitle, startYear, endYear, " +
                "runtimeMinutes FROM titleBasics WHERE primaryTitle LIKE '%" + "@search" + "%'";

            List<Title> titles = new List<Title>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(queryString,conn);
                cmd.Parameters.AddWithValue("@search", search);
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
                title.StartYear = reader.GetInt32(3);
            if (!reader.IsDBNull(4))
                title.EndYear = reader.GetInt32(4);
            if (!reader.IsDBNull(5))
                title.RuntimeMinutes = reader.GetInt32(5);

            return title;
        }
        





    }
}
