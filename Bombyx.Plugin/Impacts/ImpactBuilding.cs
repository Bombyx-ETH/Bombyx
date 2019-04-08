using System;
using Grasshopper.Kernel;
using System.Collections.Generic;

namespace Bombyx.Plugin.Impacts
{
    public class ImpactBuilding : GH_Component
    {
        public ImpactBuilding()
          : base("Building impact",
                 "Building impact",
                 "Calculates CO2 impact of the building",
                 "Bombyx",
                 "Impacts")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("LCA Factors\nElement", "LCA Factors\n(Element)", "List of LCA factors", GH_ParamAccess.list);
            pManager.AddNumberParameter("Reference study period (years)", "RSP (years)", "Reference study period (years)", GH_ParamAccess.item);
            pManager.AddNumberParameter("NFA (square meters)", "NFA (m\xB2)", "NFA (square meters)", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Global warming potential", "GWP (kg CO\x2082-eq/m\xB2 a)", "Global warming potential (kg CO\x2082-eq/m\xB2 a)", GH_ParamAccess.item);
            pManager.AddNumberParameter("PE Total", "PE Total (kWh oil-eq)", "PE Total (kWh oil-eq)", GH_ParamAccess.item);            
            pManager.AddNumberParameter("PE Renewable", "PE Renewable (kWh oil-eq)", "PE Renewable (kWh oil-eq)", GH_ParamAccess.item);
            pManager.AddNumberParameter("PE Non-Renewable", "PE Non-Renewable (kWh oil-eq)", "PE Non-Renewable (kWh oil-eq)", GH_ParamAccess.item);
            pManager.AddNumberParameter("UBP impact", "UBP (P/m\xB2 a)", "UBP (P/m\xB2 a)", GH_ParamAccess.item);
        }

        public static IEnumerable<List<T>> SplitList<T>(List<T> locations, int nSize)
        {
            for (int i = 0; i < locations.Count; i += nSize)
            {
                yield return locations.GetRange(i, Math.Min(nSize, locations.Count - i));
            }
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var element = new List<double>();
            if (!DA.GetDataList(0, element)) { return; }
            var RSP = 0d;
            if (!DA.GetData(1, ref RSP)) { return; }
            var NFA = 0d;
            if (!DA.GetData(2, ref NFA)) { return; }

            var inputs = SplitList(element, 15);
            var result = new List<double>();
            var rspNFA = RSP * NFA;

            var UBP13EmbodiedSum = 0d;
            var UBP13RepSum = 0d;
            var UBP13EoLSum = 0d;
            var TotalEmbodiedSum = 0d;
            var TotalRepSum = 0d;
            var TotalEoLSum = 0d;
            var RenewableEmbodiedSum = 0d;
            var RenewableRepSum = 0d;
            var RenewableEoLSum = 0d;
            var NonRenewableEmbodiedSum = 0d;
            var NonRenewableRepSum = 0d;
            var NonRenewableEoLSum = 0d;
            var GHGEmbodiedSum = 0d;
            var GHGRepSum = 0d;
            var GHGEoLEoLSum = 0d;

            foreach (var item in inputs)
            {
                UBP13EmbodiedSum = UBP13EmbodiedSum + item[0];
                UBP13RepSum = UBP13RepSum + item[1];
                UBP13EoLSum = UBP13EoLSum + item[2];
                TotalEmbodiedSum = TotalEmbodiedSum + item[3];
                TotalRepSum = TotalRepSum + item[4];
                TotalEoLSum = TotalEoLSum + item[5];
                RenewableEmbodiedSum = RenewableEmbodiedSum + item[6];
                RenewableRepSum = RenewableRepSum + item[7];
                RenewableEoLSum = RenewableEoLSum + item[8];
                NonRenewableEmbodiedSum = NonRenewableEmbodiedSum + item[9];
                NonRenewableRepSum = NonRenewableRepSum + item[10];
                NonRenewableEoLSum = NonRenewableEoLSum + item[11];
                GHGEmbodiedSum = GHGEmbodiedSum + item[12];
                GHGRepSum = GHGRepSum + item[13];
                GHGEoLEoLSum = GHGEoLEoLSum + item[14];
            }

            result.Add(UBP13EmbodiedSum / rspNFA);
            result.Add(UBP13RepSum / rspNFA);
            result.Add(UBP13EoLSum / rspNFA);
            result.Add(TotalEmbodiedSum / rspNFA);
            result.Add(TotalRepSum / rspNFA);
            result.Add(TotalEoLSum / rspNFA);
            result.Add(RenewableEmbodiedSum / rspNFA);
            result.Add(RenewableRepSum / rspNFA);
            result.Add(RenewableEoLSum / rspNFA);
            result.Add(NonRenewableEmbodiedSum / rspNFA);
            result.Add(NonRenewableRepSum / rspNFA);
            result.Add(NonRenewableEoLSum / rspNFA);
            result.Add(GHGEmbodiedSum / rspNFA);
            result.Add(GHGRepSum / rspNFA);
            result.Add(GHGEoLEoLSum / rspNFA);

            var gwp = Math.Round((result[12] + result[13] + result[14]), 4);
            var total = Math.Round((result[3] + result[4] + result[5]), 4);
            var ubp = Math.Round((result[0] + result[1] + result[2]), 4);
            var renew = Math.Round((result[6] + result[7] + result[8]), 4);
            var nonrenew = Math.Round((result[9] + result[10] + result[11]), 4);

            DA.SetData(0, gwp);
            DA.SetData(1, total);          
            DA.SetData(2, renew);
            DA.SetData(3, nonrenew);
            DA.SetData(4, ubp);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Icons._4building;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("874663b3-137a-4d57-9975-dc6e645f6839"); }
        }
    }
}