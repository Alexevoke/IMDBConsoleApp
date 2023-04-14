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
    public static class TitleQueries
    {

        public static List<Title> TitleSearch(string? search)
        {
            List<Title> titles = new List<Title>();

            if (String.IsNullOrEmpty(search))
                return titles;

            Connection connection = new Connection();

            string queryString = "SELECT titleBasics_ID, titleType_ID, primaryTitle, originalTitle, " +
                "isAdult, startYear, endYear, runtimeMinutes FROM VW_titleBasics " +
                "WHERE primaryTitle LIKE @search " +
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

            queryString = "SELECT titleBasics_ID, titleGenres_ID FROM VW_titleBasicsGenres " +
                "WHERE primaryTitle LIKE @search";

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

                if (!titles.Exists(k => k.Id == genre.Id))
                    continue;

                titles.Find(k => k.Id == genre.Id).Genres.Add(genre.Genretype);
            }

            return titles;
        }

        public static void AddTitle(Title title)
        {
            Connection connection = new Connection();

            string queryString = "INSERT INTO VW_titleBasics (titleType_ID, primaryTitle, " +
                "originalTitle, isAdult, startYear, endYear, runtimeMinutes) " +
                "VALUES (@titleType_ID, @primaryTitle, @originalTitle, @isAdult, @startYear, " +
                "@endYear, @runtimeMinutes)";

            using (SqlConnection conn = new SqlConnection(connection.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(queryString, conn);
                cmd.Parameters.AddWithValue("@primaryTitle", title.PrimaryTitle);
                cmd.Parameters.AddWithValue("@originalTitle", title.OriginalTitle);
                cmd.Parameters.AddWithValue("@isAdult", title.IsAdult);
                cmd.Parameters.AddWithValue("@startYear", title.StartYear);
                cmd.Parameters.AddWithValue("@endYear", title.EndYear);
                cmd.Parameters.AddWithValue("@runtimeMinutes", title.RuntimeMinutes);
                cmd.Parameters.AddWithValue("@titleType_ID", (int)title.TitleType + 1);
                cmd.Connection.Open();

                int rows = cmd.ExecuteNonQuery();

                if (rows != 1)
                {
                    throw new ArgumentException("Title has not been added");
                }
            }
            
            TitleIDGenerator generator = new TitleIDGenerator();
            title.Id = generator.Id;

            queryString = "INSERT INTO VW_titleBasicsGenres (titleBasics_ID, titleGenres_ID) " +
                "VALUES (@titleBasics_ID, @titleGenres_ID)";

            foreach (GenreType genre in title.Genres)
            {
                using (SqlConnection conn = new SqlConnection(connection.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand(queryString, conn);
                    cmd.Parameters.AddWithValue("@titleBasics_ID", title.Id);
                    cmd.Parameters.AddWithValue("@titleGenres_ID", (int)genre + 1);
                    cmd.Connection.Open();

                    int rows = cmd.ExecuteNonQuery();

                    if (rows != 1)
                    {
                        throw new ArgumentException("Title has not been added");
                    }
                }
            }

        }

        public static void DeleteTitle(int id)
        {
            Connection connection = new Connection();

            string queryString = "DELETE FROM VW_titleBasics_titleGenres " +
                "WHERE titleBasics_ID = @id";

            using (SqlConnection conn = new SqlConnection(connection.ConnectionString)) 
            {
                SqlCommand cmd = new SqlCommand(queryString, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Connection.Open();

                int rows = cmd.ExecuteNonQuery();
                if (rows != 1)
                {
                    throw new ArgumentException("Title has not been deleted");
                }
            }

            queryString = "DELETE FROM VW_titleBasics " +
                "WHERE titleBasics_ID = @id";

            using (SqlConnection conn = new SqlConnection(connection.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(queryString, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Connection.Open();

                int rows = cmd.ExecuteNonQuery();
                if (rows != 1)
                {
                    throw new ArgumentException("Title has not been deleted");
                }
            }
        }

        private static Title ReadTitle(SqlDataReader reader)
        {
            Title title = new Title();

            if (!reader.IsDBNull(0))
                title.Id = reader.GetInt32(0);
            if (!reader.IsDBNull(1))
                title.TitleType = (TitleType)reader.GetInt32(1) - 1;
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
                genre.Id = reader.GetInt32(0);
            if (!reader.IsDBNull(1))
                genre.Genretype = (GenreType)reader.GetInt32(1) - 1;

            return genre;
        }

    }
}
