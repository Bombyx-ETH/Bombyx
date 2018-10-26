using System;
using System.Collections.Generic;
using System.Data;

namespace Bombyx.Data.KBOB
{
    public static class KBOBdata
    {
        private static DataTable GetData()
        {
            DBConnectKBOB db = new DBConnectKBOB();
            var data = db.SelectKBOBdata();
            return data;
        }

        public static List<string> GetMaterialsList(string param)
        {
            DataTable data = GetData();
            var results = new List<string>();

            IEnumerable<DataRow> query = from row in data.AsEnumerable()
                                         where row.Field<string>(4).StartsWith(param)
                                         select row;

            foreach (DataRow item in query)
            {
                results.Add(item[1].ToString().Trim() + " | " + item[4].ToString().Trim());
            }

            return results;
        }

        public static KBOBMaterialModel GetMaterial(string param)
        {
            DataTable data = GetData();
            var result = new KBOBMaterialModel();
            
            var newString = param.Split('|');
            IEnumerable<DataRow> query = from row in data.AsEnumerable()
                                         where row.Field<string>(4).Trim().Equals(newString[1].Trim())
                                         select row;
         
            foreach (DataRow item in query)
            {
                result.IdKBOB = item[4].ToString().Trim();
                result.Density = item[7] != DBNull.Value ? Convert.ToSingle(item[7]) : 1;
                result.UBPfab = Convert.ToSingle(item[9]);
                result.UBPeol = Convert.ToSingle(item[11]);
                result.PEnrfab = Convert.ToSingle(item[15]);
                result.PEnreol = Convert.ToSingle(item[16]);
                result.GWPfab = Convert.ToSingle(item[17]);
                result.GWPeol = Convert.ToSingle(item[18]);
            }

            return result;
        }
    }
}
