using System;
using Grasshopper.Kernel;
using System.Collections.Generic;
using System.Linq;

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
            pManager.AddTextParameter("Layer properties (text)", "Layer\nproperties (text)", "Layer property (text)", GH_ParamAccess.list);
            pManager.AddNumberParameter("Layer properties (values)", "Layer properties (values)", "Layer properties (values)", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var material = new List<double>();
            if (!DA.GetDataList(0, material)) { return; }
            var thickness = 0d;
            if (!DA.GetData(1, ref thickness)) { return; }

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

            var output = new Dictionary<string, double>();

            output.Add("Density (kg/m\xB3)", (double?)material[0] ?? -1);
            output.Add("UBP13 Embodied * (density * thickness) (P/m\xB2 a)", material[1] * AreaDensity);
            output.Add("UBP13 End of Life * (density * thickness) (P/m\xB2 a)", material[2] * AreaDensity);
            output.Add("Total Embodied * (density * thickness) (kWh oil-eq)", material[3] * AreaDensity);
            output.Add("Total End of Life * (density * thickness) (kWh oil-eq)", material[4] * AreaDensity);
            output.Add("Renewable Embodied * (density * thickness) (kWh oil-eq)", material[5] * AreaDensity);
            output.Add("Renewable End of Life * (density * thickness) (kWh oil-eq)", material[6] * AreaDensity);
            output.Add("Non Renewable Embodied * (density * thickness) (kWh oil-eq)", material[7] * AreaDensity);
            output.Add("Non Renewable End of Life * (density * thickness) (kWh oil-eq)", material[8] * AreaDensity);
            output.Add("Green House Gases Embodied * (density * thickness) (kg CO\x2082-eq/m\xB2 a)", material[9] * AreaDensity);
            output.Add("Green House Gases End of Life * (density * thickness) (kg CO\x2082-eq/m\xB2 a)", material[10] * AreaDensity);
            output.Add("R value = (thickness / thermal conductivity)", (double?)resistance ?? -1);

            var outputValues = output.Values.ToList();

            DA.SetDataList(0, output);
            DA.SetDataList(1, outputValues);

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