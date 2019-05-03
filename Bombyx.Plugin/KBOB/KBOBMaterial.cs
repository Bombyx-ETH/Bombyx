using System;
using Grasshopper.Kernel;
using Bombyx.Data.KBOB;
using System.Collections.Generic;

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
            pManager.AddNumberParameter("Material properties", "Material\nproperties", "Material property order: " +
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
                "\n11: Thermal conductivity", GH_ParamAccess.list); 
        }
        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string input = null;
            if (!DA.GetData(0, ref input)) { return; }

            var material = KBOBdata.GetMaterial(input);
            var result = new List<object>();

            result.Add(material.Density);
            result.Add(material.UBP13Embodied);
            result.Add(material.UBP13EoL);
            result.Add(material.TotalEmbodied);
            result.Add(material.TotalEoL);
            result.Add(material.RenewableEmbodied);
            result.Add(material.RenewableEoL);
            result.Add(material.NonRenewableEmbodied);
            result.Add(material.NonRenewableEoL);
            result.Add(material.GHGEmbodied);
            result.Add(material.GHGEoL);
            result.Add(material.ThermalCond);

            DA.SetDataList(0, result);
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