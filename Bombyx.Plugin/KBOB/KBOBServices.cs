using System;
using Grasshopper.Kernel;
using Bombyx.Data.KBOB;

namespace Bombyx.Plugin.KBOB
{
    public class KBOBServices : GH_Component
    {
        public KBOBServices()
          : base("KBOB Building service",
                 "KBOB Building Services",
                 "Returns KBOB building services from database",
                 "Bombyx",
                 "Services")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Selected service", "Service", "Selected service from services list", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("UBP Embodied", "UBP Embodied (P/m\xB2 a)", "UBP Embodied", GH_ParamAccess.item);
            pManager.AddNumberParameter("UBP EoL", "UBP EoL (P/m\xB2 a)", "UBP EoL", GH_ParamAccess.item);
            pManager.AddNumberParameter("PE Total Embodied", "PE Total Embodied (kWh oil-eq/m\xB2 a)", "PE Total Embodied", GH_ParamAccess.item);
            pManager.AddNumberParameter("PE Total EoL", "PE Total EoL (kWh oil-eq/m\xB2 a)", "PE Total EoL (kWh oil-eq/m\xB2 a)", GH_ParamAccess.item);
            pManager.AddNumberParameter("PE Renewable Embodied", "PE Renewable Embodied (kWh oil-eq/m\xB2 a)", "PE Renewable Embodied", GH_ParamAccess.item);
            pManager.AddNumberParameter("PE Renewable EoL", "PE Renewable EoL (kWh oil-eq/m\xB2 a)", "PE Renewable EoL", GH_ParamAccess.item);
            pManager.AddNumberParameter("PE Non Renewable Embodied", "PE Non Renewable Embodied (kWh oil-eq/m\xB2 a)", "PE Non Renewable Embodied", GH_ParamAccess.item);
            pManager.AddNumberParameter("PE Non Renewable EoL", "PE Non Renewable EoL (kWh oil-eq/m\xB2 a)", "PE Non Renewable EoL", GH_ParamAccess.item);
            pManager.AddNumberParameter("GHG Embodied", "GHG Embodied (kg CO\x2082-eq/m\xB2 a)", "GHG Embodied", GH_ParamAccess.item);
            pManager.AddNumberParameter("GHG EoL", "GHG EoL (kg CO\x2082-eq/m\xB2 a)", "GHG EoL", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string input = null;
            if (!DA.GetData(0, ref input)) { return; }

            var result = KBOBdata.GetService(input);

            DA.SetData(0, result[0]);
            DA.SetData(1, result[1]);
            DA.SetData(2, result[2]);
            DA.SetData(3, result[3]);
            DA.SetData(4, result[4]);
            DA.SetData(5, result[5]);
            DA.SetData(6, result[6]);
            DA.SetData(7, result[7]);
            DA.SetData(8, result[8]);
            DA.SetData(9, result[9]);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Icons._3services;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("66bdbfbf-3c68-4431-91bf-b9505299ac60"); }
        }
    }
}