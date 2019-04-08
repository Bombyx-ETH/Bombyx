using System;
using Grasshopper.Kernel;
using System.Collections.Generic;

namespace Bombyx.Plugin.Impacts
{
    public class ImpactElement : GH_Component
    {
        public ImpactElement()
          : base("Element impact",
                 "Element impact",
                 "Calculates impacts of PE, GWP, UBP",
                 "Bombyx",
                 "Impacts")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Component Properties", "Component\nProperties", "List of component properties", GH_ParamAccess.list);
            pManager.AddNumberParameter("Area (square meters)", "Area (m\xB2)", "Manual value", GH_ParamAccess.item);
            pManager.AddTextParameter("Functionality", "Functionality", "Functionalities:\n'ext wall'\n'int wall'\n'window'\n'floor'\n'roof'", GH_ParamAccess.item);
            pManager[0].DataMapping = GH_DataMapping.Flatten;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Element Properties", "LCA Factors\nElement", "LCA factors order: " +
                "\n00:UBP13 Embodied " +
                "\n01:UBP13 Rep " +
                "\n02:UBP13 EoL " +
                "\n03:PE Total Embodied " +
                "\n04:PE Total Rep " +
                "\n05:PE Total EoL " +
                "\n06:PE Renewable Embodied " +
                "\n07:PE Renewable Rep " +
                "\n08:PE Renewable EoL " +
                "\n09:PE Non-Renewable Embodied " +
                "\n10:PE Non-Renewable Rep " +
                "\n11:PE Non-Renewable EoL " +
                "\n12:GHG Embodied " +
                "\n13:GHG Rep " +
                "\n14:GHG EoL", GH_ParamAccess.list);
            pManager.AddNumberParameter("U value", "U value\n(W/m2*K)", "U Value", GH_ParamAccess.item);
            pManager.AddNumberParameter("UA value", "UA value\n(W/K)", "Area U Value", GH_ParamAccess.item);
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
            var component = new List<double>();
            if (!DA.GetDataList(0, component)) { return; }
            var area = 0d;
            if (!DA.GetData(1, ref area)) { return; }
            var funct = "";
            if (!DA.GetData(2, ref funct)) { return; }

            var inputs = SplitList(component, 17);
            var result = new List<double>();

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
            var UvalueSum = 0d;

            foreach (var item in inputs)
            {
                UBP13EmbodiedSum = UBP13EmbodiedSum + item[1];
                UBP13RepSum = UBP13RepSum + item[2];
                UBP13EoLSum = UBP13EoLSum + item[3];
                TotalEmbodiedSum = TotalEmbodiedSum + item[4];
                TotalRepSum = TotalRepSum + item[5];
                TotalEoLSum = TotalEoLSum + item[6];
                RenewableEmbodiedSum = RenewableEmbodiedSum +item[7];
                RenewableRepSum = RenewableRepSum + item[8];
                RenewableEoLSum = RenewableEoLSum + item[9];
                NonRenewableEmbodiedSum = NonRenewableEmbodiedSum + item[10];
                NonRenewableRepSum = NonRenewableRepSum + item[11];
                NonRenewableEoLSum = NonRenewableEoLSum + item[12];
                GHGEmbodiedSum = GHGEmbodiedSum + item[13];
                GHGRepSum = GHGRepSum + item[14];
                GHGEoLEoLSum = GHGEoLEoLSum + item[15];
                UvalueSum = UvalueSum + item[16];
            }

            result.Add(UBP13EmbodiedSum * area);
            result.Add(UBP13RepSum * area);
            result.Add(UBP13EoLSum * area);
            result.Add(TotalEmbodiedSum * area);
            result.Add(TotalRepSum * area);
            result.Add(TotalEoLSum * area);
            result.Add(RenewableEmbodiedSum * area);
            result.Add(RenewableRepSum * area);
            result.Add(RenewableEoLSum * area);
            result.Add(NonRenewableEmbodiedSum * area);
            result.Add(NonRenewableRepSum * area);
            result.Add(NonRenewableEoLSum * area);
            result.Add(GHGEmbodiedSum * area);
            result.Add(GHGRepSum * area);
            result.Add(GHGEoLEoLSum * area);

            DA.SetDataList(0, result);
            if(funct.Equals("window"))
            {
                DA.SetData(1, UvalueSum);
                DA.SetData(2, UvalueSum * area);
            }
            else if (funct.Equals("int wall"))
            {
                DA.SetData(1, 1 / (UvalueSum + 0.17));
                DA.SetData(2, (1 / (UvalueSum + 0.17)) * area);
            }
            else if(funct.Equals("ext wall"))
            {
                DA.SetData(1, 1 / (UvalueSum + 0.17));
                DA.SetData(2, (1 / (UvalueSum + 0.17)) * area);
            }
            else if (funct.Equals("roof"))
            {
                DA.SetData(1, 1 / (UvalueSum + 0.17));
                DA.SetData(2, (1 / (UvalueSum + 0.17)) * area);
            }
            else if (funct.Equals("ceiling"))
            {
                DA.SetData(1, 1 / (UvalueSum + 0.17));
                DA.SetData(2, (1 / (UvalueSum + 0.17)) * area);
            }
            else if (funct.Equals("floor"))
            {
                DA.SetData(1, 1 / (UvalueSum + 0.13));
                DA.SetData(2, (1 / (UvalueSum + 0.13)) * area);
            }
            else
            {
                DA.SetData(1, UvalueSum);
                DA.SetData(2, UvalueSum * area);
            }
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Icons._3elem;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("da9a9a35-cf9c-45e9-a54f-a9320cc22ccf"); }
        }
    }
}