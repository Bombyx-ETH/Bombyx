using System;
using Grasshopper.Kernel;
using System.Collections.Generic;

namespace Bombyx.Plugin.Impacts
{
    public class ImpactComponent : GH_Component
    {
        public ImpactComponent()
          : base("Component impact",
                 "Component impact",
                 "Calculates impacts of PE, GWP, UBP",
                 "Bombyx",
                 "Impacts")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Layer properties", "Layer\nproperties", "List of layer properties", GH_ParamAccess.list);
            pManager.AddIntegerParameter("Reference study period", "RSP (years)", "Reference study period", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Reference service life", "RSL (years)", "Reference service life", GH_ParamAccess.item);            
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Component Properties", "Component\nproperties", "Component properties order: " +
                "\n00: unused " +
                "\n01: UBP13 Embodied " +
                "\n02: UBP13 Rep " +
                "\n03: UBP13 EoL " +
                "\n04: PE Total Embodied " +
                "\n05: PE Total Rep " +
                "\n06: PE Total EoL " +
                "\n07: PE Renewable Embodied " +
                "\n08: PE Renewable Rep " +
                "\n09: PE Renewable Rep " +
                "\n10: PE Non-Renewable Embodied " +
                "\n11: PE Non-Renewable Rep " +
                "\n12: PE Non-Renewable EoL " +
                "\n13: GHG Embodied " +
                "\n14: GHG Rep " +
                "\n15: GHG EoL " +
                "\n16: R-value", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var layer = new List<double>();
            if (!DA.GetDataList(0, layer)) { return; }
            var RSP = 0;
            if (!DA.GetData(1, ref RSP)) { return; }
            var RSL = 0;
            if (!DA.GetData(2, ref RSL)) { return; }          

            var result = new List<double>();
            double repNum = 0;
            double tmp = ((double)RSP / (double)RSL) - 1;
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
            
            result.Add(layer[0]);
            result.Add(layer[1]); //UBP13Embodied 1
            result.Add((layer[1] + layer[2]) * repNum); //UBP13Rep 2
            result.Add(layer[2]); //UBP13EoL 3
            result.Add(layer[3]); //TotalEmbodied 4
            result.Add((layer[3] + layer[4]) * repNum); //TotalRep 5
            result.Add(layer[4]); //TotalEoL 6
            result.Add(layer[5]); //RenewableEmbodied 7
            result.Add((layer[5] + layer[6]) * repNum); //RenewableRep 8
            result.Add(layer[6]); //RenewableRep 9
            result.Add(layer[7]); //NonRenewableEmbodied 10
            result.Add((layer[7] + layer[8]) * repNum); //NonRenewableRep 11
            result.Add(layer[8]); //NonRenewableEoL 12
            result.Add(layer[9]); //GHGEmbodied 13
            result.Add((layer[9] + layer[10]) * repNum); //GHGRep 14
            result.Add(layer[10]); //GHGEoL 15
            result.Add(layer[11]); //R-value

            DA.SetDataList(0, result);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Icons._2comp;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("5ebf6cc0-12b8-4cd9-92ec-4b185cd60c59"); }
        }
    }
}