using Bombyx.Data.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Bombyx.Data.InputLevel
{
    public static class InputData
    {
        public static List<InputModel> GetElementsInputList(string query)
        {
            DBConnectInput db = new DBConnectInput();
            DataTable data = db.SelectInputs(query);

            var results = new List<InputModel>();
            IEnumerable<DataRow> dataRows = null;

            dataRows = from row in data.AsEnumerable()
                       select row;

            foreach (DataRow item in dataRows)
            {
                var tmp = new InputModel();

                tmp.UBP13Embodied = (decimal)item[1];
                tmp.UBP13EoL = (decimal)item[1];
                tmp.TotalEmbodied = (decimal)item[2];
                tmp.TotalEoL = (decimal)item[3];
                tmp.RenewableEmbodied = (decimal)item[4];
                tmp.RenewableEoL = (decimal)item[5];
                tmp.NonRenewableEmbodied = (decimal)item[6];
                tmp.NonRenewableEoL = (decimal)item[7];
                tmp.GHGEmbodied = (decimal)item[8];
                tmp.GHGEoL = (decimal)item[9];
                //if ((decimal)item[20] != 0 && (decimal)item[21] != 0)
                //{
                //    tmp.Resistance = (decimal)item[21] / (decimal)item[20];
                //}
                //else
                //{
                //    tmp.Resistance = 0;
                //}

                results.Add(tmp);
            }

            return results;
        }    
    }
}
