using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDBConsoleApp.IDGenerators
{
    public class TconstGenerator
    {
        private uint _id;

        public TconstGenerator()
        {
            _id = NumberGenerator();
        }

        public string GenerateTconst()
        {
            string text = "tt";
            string number = _id.ToString();
            string tconst = text + number;
            _id++;
            return tconst;
        }

        private uint NumberGenerator()
        {
            Connection connection = new Connection();

            string queryString = "SELECT MAX(tconst) FROM VW_titleBasics";

            string tconst;

            string number;

            uint id;

            using (SqlConnection conn = new SqlConnection(connection.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(queryString, conn);
                cmd.Connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                reader.Read();
                tconst = ReadMaxTconst(reader);

                

            }

            number = tconst.Substring(2);
            id = Convert.ToUInt32(number) + 1;

            return id;
        }

        private string ReadMaxTconst(SqlDataReader reader)
        {
            string tconst;

            tconst = reader.GetString(0);

            return tconst;
        }
    }
}
