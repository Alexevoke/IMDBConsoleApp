using IMDBClassLibrary.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDBConsoleApp.Queries
{
    public static class TitleQueries
    {

        public static List<Title> TitleSearch(string? search)
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
                SqlCommand cmd = new SqlCommand(queryString, conn);
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
        private static Title ReadTitle(SqlDataReader reader)
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

        private static Genre ReadGenre(SqlDataReader reader)
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
