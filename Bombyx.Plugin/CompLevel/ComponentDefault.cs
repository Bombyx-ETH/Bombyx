using System;
using Grasshopper.Kernel;
using System.Linq;
using Bombyx.Data.CompLevel;
using System.Collections.Generic;

namespace Bombyx.Plugin.CompLevel
{
    public class ComponentDefault : GH_Component
    {
        public ComponentDefault()
          : base("Default Component",
                 "Default Comp.",
                 "Returns default component from DB",
                 "Bombyx",
                 "Component Level")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Component", "Component", "Selected component", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Reference study period", "RSP (years)", "Manual input of RSP (years)", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("LCA factors", "LCA factors", "Sum of LCA factors of all materials in the component", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string input = null;
            if (!DA.GetData(0, ref input)) { return; }
            var RSP = 0;
            if (!DA.GetData(1, ref RSP)) { return; }

            var splitInput = input.Split('-').ToList();
            var results = CompData.GetGenComponent(splitInput[0].Trim(), null, true);
            var RSL = int.Parse(splitInput.Last());

            decimal repNum = 0;
            decimal tmp = ((decimal)RSP / (decimal)RSL) - 1;
            if (RSL != 0 && RSP != 0)
            {
                repNum = Math.Ceiling(tmp);
            }
            if (repNum < 0)
            {
                repNum = 0;
            }
            if (repNum == 0)
            {
                repNum = 0;
            }

            var sumResults = from row in results
                             group row by row.SortCode.Trim() into rowSum
                             select new
                             {
                                 SortCodeGrp = rowSum.Key,
                                 UBP13EmbodiedSum = rowSum.Sum(x => x.UBP13Embodied),
                                 UBP13RepSum = rowSum.Sum(x => (x.UBP13Embodied + x.UBP13EoL) * repNum),
                                 UBP13EoLSum = rowSum.Sum(x => x.UBP13EoL),
                                 TotalEmbodiedSum = rowSum.Sum(x => x.TotalEmbodied),
                                 TotalRepSum = rowSum.Sum(x => (x.TotalEmbodied + x.TotalEoL) * repNum),
                                 TotalEoLSum = rowSum.Sum(x => x.TotalEoL),
                                 RenewableEmbodiedSum = rowSum.Sum(x => x.RenewableEmbodied),
                                 RenewableRepSum = rowSum.Sum(x => (x.RenewableEmbodied + x.RenewableEoL) * repNum),
                                 RenewableEoLSum = rowSum.Sum(x => x.RenewableEoL),
                                 NonRenewableEmbodiedSum = rowSum.Sum(x => x.NonRenewableEmbodied),
                                 NonRenewableRepSum = rowSum.Sum(x => (x.NonRenewableEmbodied + x.NonRenewableEoL) * repNum),
                                 NonRenewableEoLSum = rowSum.Sum(x => x.NonRenewableEoL),
                                 GHGEmbodiedSum = rowSum.Sum(x => x.GHGEmbodied),
                                 GHGRepSum = rowSum.Sum(x => (x.GHGEmbodied + x.GHGEoL) * repNum),
                                 GHGEoLEoLSum = rowSum.Sum(x => x.GHGEoL),
                                 ResistanceSum = rowSum.Sum(x => x.Resistance)
                             };

            var result = new List<double>();

            foreach (var item in sumResults)
            {
                result.Add(1);
                result.Add((double)item.UBP13EmbodiedSum); //UBP13Embodied 1
                result.Add((double)item.UBP13RepSum); //UBP13Rep 2
                result.Add((double)item.UBP13EoLSum); //UBP13EoL 3
                result.Add((double)item.TotalEmbodiedSum); //TotalEmbodied 4
                result.Add((double)item.TotalRepSum); //TotalRep 5
                result.Add((double)item.TotalEoLSum); //TotalEoL 6
                result.Add((double)item.RenewableEmbodiedSum); //RenewableEmbodied 7
                result.Add((double)item.RenewableRepSum); //RenewableRep 8
                result.Add((double)item.RenewableEoLSum); //RenewableRep 9
                result.Add((double)item.NonRenewableEmbodiedSum); //NonRenewableEmbodied 10
                result.Add((double)item.NonRenewableRepSum); //NonRenewableRep 11
                result.Add((double)item.NonRenewableEoLSum); //NonRenewableEoL 12
                result.Add((double)item.GHGEmbodiedSum); //GHGEmbodied 13
                result.Add((double)item.GHGRepSum); //GHGRep 14
                result.Add((double)item.GHGEoLEoLSum); //GHGEoL 15
                result.Add((double)item.ResistanceSum);
            }

            DA.SetDataList(0, result);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Icons._3Dcomponent;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("f6529833-8648-4cb8-8764-f6b02535d5ed"); }
        }
    }
}