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

        public DataTable SelectKBOBdata(string table)
        {
            var query = "";

            switch (table)
            {
                case "material":
                    query = "SELECT * FROM KbobMaterial";
                    break;
                case "energy":
                    query = "SELECT * FROM KbobEnergy WHERE IdKbob IN('43.001','43.002','43.006','43.007','43.008','43.009','44.001','44.002','44.003')";
                    break;
                case "services":
                    query = "SELECT * FROM KbobServices";
                    break;
            }
            
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
