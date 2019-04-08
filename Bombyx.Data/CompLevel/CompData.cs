using Bombyx.Data.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Bombyx.Data.CompLevel
{
    public static class CompData
    {
        public static List<string> GetComponentGroupsList()
        {
            var query = "SELECT DISTINCT ComponentCode, CategoryEnglish FROM BtkComponent WHERE ComponentCode NOT IN ('B6.2', 'D1', 'D7', 'D8', 'D5.2', 'D5.4') " +
                        "UNION " +
                        "SELECT DISTINCT ComponentCode, CategoryEnglish FROM dbo.BtkWindows";
            DBConnectComp db = new DBConnectComp();
            DataTable data = db.SelectComponents(query);

            List<string> results = data.AsEnumerable().Select(x => x[0].ToString().Trim() + " - " + x[1].ToString().Trim()).ToList();

            return results;
        }

        public static List<string> GetComponents(string param)
        {
            var query = "";

            if(param == "E3.1")
            {
                query = "SELECT ComponentCode, SortCode, CategoryTextEnglish, RSL FROM BtkWindows";
            }
            else
            {
                query = "SELECT ComponentCode, SortCode, CategoryTextEnglish, RSL FROM BtkComponent";
            }
            
            DBConnectComp db = new DBConnectComp();
            DataTable data = db.SelectComponents(query);
            var results = new List<string>();

            IEnumerable<DataRow> dataRows = from row in data.AsEnumerable()
                                            where row.Field<string>(0).StartsWith(param)
                                            select row;

            foreach (DataRow item in dataRows)
            {
                results.Add(item[1].ToString().Trim() + " - " + item[2].ToString().Trim() + " - " + item[3]);
            }

            return results;
        }

        public static List<ComponentModel> GetGenComponent(string sortcode, double? thickness, bool defaultMaterial)
        {
            var query = "";

            if (defaultMaterial)
            {
                query = "SELECT mat.Id, bk.SortCode, mat.NameEnglish, mat.NameGerman, mat.NameFrench, mat.IdKbob, mat.Disposal, " +
                        "mat.IdDisposal, mat.Density, mat.DensityUnit, mat.Ubp13Embodied, mat.Ubp13EoL, mat.TotalEmbodied, " +
                        "mat.TotalEoL, mat.RenewableEmbodied, mat.RenewableEoL, mat.NonRenewableEmbodied, mat.NonRenewableEoL, " +
                        "mat.GHGEmbodied, mat.GHGEoL, bk.ThermalCond, bk.Thickness, bk.Layer " +
                        "FROM KbobMaterial mat " +
                        "LEFT JOIN BtkKbob bk " +
                        "ON mat.Id = bk.IdKbob " +
                        "WHERE bk.SortCode = '" + sortcode + "'";
            }
            else
            {
                query = "SELECT * FROM KbobMaterialGen WHERE Thickness = " + thickness + " AND SortCode = '" + sortcode + "'";
            }
            
            DBConnectComp db = new DBConnectComp();
            DataTable data = db.SelectComponents(query);
            var results = new List<ComponentModel>();
            IEnumerable<DataRow> dataRows = null;

            if (defaultMaterial)
            {
                dataRows = from row in data.AsEnumerable()
                           select row;
            }
            else
            {
                dataRows = from row in data.AsEnumerable()
                           where row.Field<decimal>(21) == (decimal)thickness
                           select row;
            }

            foreach (DataRow item in dataRows)
            {
                var tmp = new ComponentModel();

                tmp.SortCode = item[1].ToString();
                tmp.NameEnglish = item[2].ToString();
                tmp.NameGerman = item[3].ToString();
                tmp.NameFrench = item[4].ToString();
                tmp.Density = (decimal)item[8];
                tmp.UBP13Embodied = (decimal)item[10];
                tmp.UBP13EoL = (decimal)item[11];
                tmp.TotalEmbodied = (decimal)item[12];
                tmp.TotalEoL = (decimal)item[13];
                tmp.RenewableEmbodied = (decimal)item[14];
                tmp.RenewableEoL = (decimal)item[15];
                tmp.NonRenewableEmbodied = (decimal)item[16];
                tmp.NonRenewableEoL = (decimal)item[17];
                tmp.GHGEmbodied = (decimal)item[18];
                tmp.GHGEoL = (decimal)item[19];
                tmp.ThermalCond = (decimal)item[20];
                tmp.Thickness = (decimal)item[21];
                tmp.Layer = (int)item[22];               
                if ((decimal)item[20] != 0 && (decimal)item[21] != 0)
                {
                    tmp.Resistance = (decimal)item[21] / (decimal)item[20];
                }
                else
                {
                    tmp.Resistance = 0;
                }

                results.Add(tmp);
            } 

            return results;
        }

        public static List<WindowModel> GetWindow(string sortcode)
        {           
             var query = "SELECT mat.Id, bkw.SortCode, mat.NameEnglish, mat.NameGerman, mat.NameFrench, mat.IdKbob, mat.Disposal, " +
                         "mat.IdDisposal, mat.Density, mat.DensityUnit, mat.Ubp13Embodied, mat.Ubp13EoL, mat.TotalEmbodied, " +
                         "mat.TotalEoL, mat.RenewableEmbodied, mat.RenewableEoL, mat.NonRenewableEmbodied, mat.NonRenewableEoL, " +
                         "mat.GHGEmbodied, mat.GHGEoL, bkw.FramePercentage, bw.Uvalue, bw.Gvalue " +
                         "FROM KbobMaterial mat " +
                         "LEFT JOIN dbo.BtkKbobWindow bkw " +
                         "ON mat.Id = bkw.IdKbob " +
                         "LEFT JOIN dbo.BtkWindows bw " +
                         "ON bkw.SortCode = bw.SortCode " +
                         "WHERE bkw.SortCode = '" + sortcode + "'";

            DBConnectComp db = new DBConnectComp();
            DataTable data = db.SelectComponents(query);
            var results = new List<WindowModel>();
            IEnumerable<DataRow> dataRows = null;

            dataRows = from row in data.AsEnumerable()
                       select row;

            foreach (DataRow item in dataRows)
            {
                var tmp = new WindowModel();
                var percent = (decimal)item[20] / 100;

                tmp.SortCode = item[1].ToString();
                tmp.NameEnglish = item[2].ToString();
                tmp.NameGerman = item[3].ToString();
                tmp.NameFrench = item[4].ToString();
                tmp.UBP13Embodied = (decimal)item[10] * percent;
                tmp.UBP13EoL = (decimal)item[11] * percent;
                tmp.TotalEmbodied = (decimal)item[12] * percent;
                tmp.TotalEoL = (decimal)item[13] * percent;
                tmp.RenewableEmbodied = (decimal)item[14] * percent;
                tmp.RenewableEoL = (decimal)item[15] * percent;
                tmp.NonRenewableEmbodied = (decimal)item[16] * percent;
                tmp.NonRenewableEoL = (decimal)item[17] * percent;
                tmp.GHGEmbodied = (decimal)item[18] * percent;
                tmp.GHGEoL = (decimal)item[19] * percent;
                tmp.Uvalue = item[21] == null ? 0 : (decimal)item[21];
                tmp.Gvalue = item[22] == null ? 0 : (decimal)item[22];

                results.Add(tmp);
            }

            return results;
        }
    }
}
