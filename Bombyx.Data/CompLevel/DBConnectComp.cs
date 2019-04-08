using System;
using System.Data;
using System.Data.SqlClient;

namespace Bombyx.Data.CompLevel
{
    public class DBConnectComp
    {
        private readonly SqlConnection connection;

        public DBConnectComp()
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

        public DataTable SelectComponents(string query)
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
