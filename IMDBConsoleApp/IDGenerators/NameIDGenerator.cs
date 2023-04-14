using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDBConsoleApp.IDGenerators
{
    public class NameIDGenerator
    {
        public NameIDGenerator()
        {
            Id = IDGenerator();
        }

        public int Id { get; private set; }

        private int IDGenerator()
        {
            Connection connection = new Connection();

            string queryString = "SELECT MAX(nameBasics_ID) FROM VW_nameBasics";

            int id;

            using (SqlConnection conn = new SqlConnection(connection.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(queryString, conn);
                cmd.Connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                reader.Read();
                id = ReadMaxID(reader);
            }

            return id;
        }

        private int ReadMaxID(SqlDataReader reader)
        {
            int id;

            id = reader.GetInt32(0);

            return id;
        }
    }
}
