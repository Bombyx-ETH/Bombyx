using Bombyx.Data.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace Bombyx.Data.KBOB
{
    public static class KBOBdata
    {
        private static DataTable GetData(string table)
        {
            DBConnectKBOB db = new DBConnectKBOB();
            var data = db.SelectKBOBdata(table);
            return data;
        }

        public static List<string> GetMaterialsList(string param)
        {
            DataTable data = GetData("material");
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

        public static MaterialModel GetMaterial(string param)
        {
            DataTable data = GetData("material");
            var result = new MaterialModel();
            
            var newString = param.Split('|');
            IEnumerable<DataRow> query = from row in data.AsEnumerable()
                                         where row.Field<string>(4).Trim().Equals(newString[1].Trim())
                                         select row;
         
            foreach (DataRow item in query)
            {
                result.IdKbob = item[4].ToString().Trim();
                result.NameEnglish = item[1].ToString().Trim();
                result.NameGerman = item[2].ToString().Trim();
                result.NameFrench = item[3].ToString().Trim();
                result.Density = item[7] != DBNull.Value ? (decimal)item[7] : 1;
                result.UBP13Embodied = (decimal)item[9];
                result.UBP13EoL = (decimal)item[10];
                result.TotalEmbodied = (decimal)item[11];
                result.TotalEoL = (decimal)item[12];
                result.RenewableEmbodied = (decimal)item[13];
                result.RenewableEoL = (decimal)item[14];
                result.NonRenewableEmbodied = (decimal)item[15];
                result.NonRenewableEoL = (decimal)item[16];
                result.GHGEmbodied = (decimal)item[17];
                result.GHGEoL = (decimal)item[18];
            }

            return result;
        }

        public static List<string> GetEnergyList()
        {
            DataTable data = GetData("energy");
            var results = new List<string>();

            IEnumerable<DataRow> query = from row in data.AsEnumerable()
                                         select row;

            foreach (DataRow item in query)
            {
                results.Add(item[2].ToString().Trim() + " | " + item[4].ToString().Trim());
            }

            return results;
        }

        public static List<decimal> GetEnergy(string param)
        {
            DataTable data = GetData("energy");
            var results = new List<decimal>();

            IEnumerable<DataRow> query = from row in data.AsEnumerable()
                                         where row.Field<string>(4).Trim().Equals(param)
                                         select row;

            foreach (DataRow item in query)
            {
                results.Add((decimal)item[7]);
                results.Add((decimal)item[8]);
                results.Add((decimal)item[9]);
                results.Add((decimal)item[10]);
                results.Add((decimal)item[11]);
                results.Add((decimal)item[12]);
            }

            return results;
        }

        public static List<string> GetServicesList(string param)
        {
            DataTable data = GetData("services");
            var results = new List<string>();

            IEnumerable<DataRow> query = from row in data.AsEnumerable()
                                         where row.Field<string>(4).StartsWith(param)
                                         select row;

            foreach (DataRow item in query)
            {
                results.Add(item[2].ToString().Trim() + " | " + item[4].ToString().Trim());
            }

            return results;
        }

        public static List<decimal> GetService(string param)
        {
            DataTable data = GetData("services");
            var result = new List<decimal>();

            var newString = param.Split('|');
            IEnumerable<DataRow> query = from row in data.AsEnumerable()
                                         where row.Field<string>(4).Trim().Equals(newString[1].Trim())
                                         select row;

            foreach (DataRow item in query)
            {
                result.Add((decimal)item[9]);
                result.Add((decimal)item[10]);
                result.Add((decimal)item[11]);
                result.Add((decimal)item[12]);
                result.Add((decimal)item[13]);
                result.Add((decimal)item[14]);
                result.Add((decimal)item[15]);
                result.Add((decimal)item[16]);
                result.Add((decimal)item[17]);
                result.Add((decimal)item[18]);
            }

            return result;
        }
    }
}
