using System;
using System.Data.SqlClient;
using System.Data;

namespace Bombyx.Data.KBOB
{
    public class DBConnectKBOB
    {
        private readonly SqlConnection connection;
        
        public DBConnectKBOB()
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

        public DataTable SelectKBOBdata()
        {
            var query = "SELECT * FROM KbobMaterial";
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
            catch (Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show(exp.Message.ToString(), "Exception in xxx component");
            }
            finally
            {
                CheckConnectionStatus();
            }

            return results;
        }
    }
}
