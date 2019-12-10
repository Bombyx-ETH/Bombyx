using System;
using Grasshopper.Kernel;
using Bombyx.Data.KBOB;
using System.Collections.Generic;
using System.Linq;

namespace Bombyx.Plugin.KBOB
{
    public class KBOBMaterial : GH_Component
    {
        public KBOBMaterial()
          : base("KBOB Material",
                 "KBOB Material",
                 "Returns KBOB material details from database",
                 "Bombyx",
                 "Materials")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Selected material", "Material", "Selected material from materials list", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Material properties (text)", "Material\nproperties (text)", "Material properties (text)", GH_ParamAccess.list);
            pManager.AddNumberParameter("Material properties (values)", "Material\nproperties (values)", "Material properties (values)", GH_ParamAccess.list);
        }
        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string input = null;
            if (!DA.GetData(0, ref input)) { return; }

            var material = KBOBdata.GetMaterial(input);
            var output = new Dictionary<string, double>();

            output.Add("Density (kg/m\xB3)", (double?)material.Density ?? -1);
            output.Add("UBP13 Embodied (P/m\xB2 a)", (double)material.UBP13Embodied);
            output.Add("UBP13 End of Life (P/m\xB2 a)", (double)material.UBP13EoL);
            output.Add("Total Embodied (kWh oil-eq)", (double)material.TotalEmbodied);
            output.Add("Total End of Life (kWh oil-eq)", (double)material.TotalEoL);
            output.Add("Renewable Embodied (kWh oil-eq)", (double)material.RenewableEmbodied);
            output.Add("Renewable End of Life (kWh oil-eq)", (double)material.RenewableEoL);
            output.Add("Non Renewable Embodied (kWh oil-eq)", (double)material.NonRenewableEmbodied);
            output.Add("Non Renewable End of Life (kWh oil-eq)", (double)material.NonRenewableEoL);
            output.Add("Green House Gases Embodied (kg CO\x2082-eq/m\xB2 a)", (double)material.GHGEmbodied);
            output.Add("Green House Gases End of Life (kg CO\x2082-eq/m\xB2 a)", (double)material.GHGEoL);
            output.Add("Thermal Conductivity (W/m*K)", (double?)material.ThermalCond ?? -1);

            var outputValues = output.Values.ToList();

            DA.SetDataList(0, output);
            DA.SetDataList(1, outputValues);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Icons._3KBOB;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("68a89807-59a3-4347-bc90-8994d9d208e5"); }
        }
    }
}