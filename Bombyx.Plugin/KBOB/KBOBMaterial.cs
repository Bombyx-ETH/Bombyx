using System;
using Grasshopper.Kernel;
using Bombyx.Data.KBOB;

namespace Bombyx.Plugin.KBOB
{
    public class KBOBMaterial : GH_Component
    {
        public KBOBMaterial()
          : base("KBOB Material",
                 "KBOB Material",
                 "Returns KBOB material details from database",
                 "Bombyx",
                 "KBOB Data")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Input", "KBOB ID", "Material", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("PEnr(Emb)", "PEnr(Emb kWh oil-eq)", "Value", GH_ParamAccess.item);
            pManager.AddNumberParameter("PEnr(EoL)", "PEnr(EoL kWh oil-eq)", "Value", GH_ParamAccess.item);
            pManager.AddNumberParameter("GWP(Emb)", "GWP(Emb kg CO\x2082-eq)", "Value", GH_ParamAccess.item);
            pManager.AddNumberParameter("GWP(EoL)", "GWP(EoL kg CO\x2082-eq)", "Value", GH_ParamAccess.item);
            pManager.AddNumberParameter("UBP(Emb)", "UBP(Emb)", "Value", GH_ParamAccess.item);
            pManager.AddNumberParameter("UBP(EoL)", "UBP(EoL)", "Value", GH_ParamAccess.item);
            pManager.AddNumberParameter("Density", "Density (kg/m\xB3)", "Value", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string data = null;

            if (!DA.GetData(0, ref data)) { return; }
            if (data == null) { return; }
            if (data.Length == 0) { return; }

            var material = KBOBdata.GetMaterial(data);

            DA.SetData(0, material.PEnrfab);
            DA.SetData(1, material.PEnreol);
            DA.SetData(2, material.GWPfab);
            DA.SetData(3, material.GWPeol);
            DA.SetData(4, material.UBPfab);
            DA.SetData(5, material.UBPeol);
            DA.SetData(6, material.Density);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Icons.KBOBMaterial;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("68a89807-59a3-4347-bc90-8994d9d208e5"); }
        }
    }
}