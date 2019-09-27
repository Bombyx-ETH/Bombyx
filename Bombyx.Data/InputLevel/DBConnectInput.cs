using System;
using System.Data;
using System.Data.SqlClient;

namespace Bombyx.Data.InputLevel
{
    public class DBConnectInput
    {
        private readonly SqlConnection connection;

        public DBConnectInput()
        {
            connection = new SqlConnection(Config.connectAzure);
        }

        private void CheckConnectionStatus()
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }

        public DataTable SelectInputs(string query)
        {
            var results = new DataTable();

            CheckConnectionStatus();

            try
            {
                var cmd = new SqlCommand(query, connection);
                connection.Open();
                var da = new SqlDataAdapter(cmd);
                da.Fill(results);

                connection.Close();
            }
            catch (Exception)
            {
            }
            finally
            {
                CheckConnectionStatus();
            }

            return results;
        }
    }
}
