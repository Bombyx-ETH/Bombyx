using System;
using Grasshopper.Kernel;
using System.Collections.Generic;

namespace Bombyx.Plugin.Impacts
{
    public class ImpactLayer : GH_Component
    {
        public ImpactLayer()
          : base("Layer impact",
                 "Layer impact",
                 "Calculates impacts of PE, GWP, UBP",
                 "Bombyx",
                 "Impacts")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Material", "Material\nproperties", "Material properties", GH_ParamAccess.list);
            pManager.AddNumberParameter("Thickness (meters)", "Thickness (m)", "Number value", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Layer properties", "Layer\nproperties", "Layer property order: " +
                "\n00: Density " +
                "\n01: UBP Embodied " +
                "\n02: UBP EoL " +
                "\n03: PE Total Embodied " +
                "\n04: PE Total EoL " +
                "\n05: PE Renewable Embodied " +
                "\n06: PE Renewable EoL " +
                "\n07: PE Non-Renewable Embodied " +
                "\n08: PE Non-Renewable EoL " +
                "\n09: GHG Embodied " +
                "\n10: GHG EoL " + 
                "\n11: R-value", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var material = new List<double>();
            if (!DA.GetDataList(0, material)) { return; }
            var thickness = 0d;
            if (!DA.GetData(1, ref thickness)) { return; }

            var result = new List<double>();
            var AreaDensity = 0d;

            if (material[1] == 0)
            {
                AreaDensity = thickness;
            }
            else
            {
                AreaDensity = material[0] * thickness;
            }

            var resistance = 0d;
            if (material[11] != 0 && thickness != 0)
            {
                resistance =  thickness / material[11];
            }

            result.Add(material[0]);
            result.Add(material[1] * AreaDensity);
            result.Add(material[2] * AreaDensity);
            result.Add(material[3] * AreaDensity);
            result.Add(material[4] * AreaDensity);
            result.Add(material[5] * AreaDensity);
            result.Add(material[6] * AreaDensity);
            result.Add(material[7] * AreaDensity);
            result.Add(material[8] * AreaDensity);
            result.Add(material[9] * AreaDensity);
            result.Add(material[10] * AreaDensity);
            result.Add(resistance);

            DA.SetDataList(0, result);

        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Icons._1layer;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("0e2d021d-4899-4b4b-bdd7-062f1ff3066b"); }
        }
    }
}