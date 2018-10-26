using System;
using Grasshopper.Kernel;

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
            pManager.AddIntegerParameter("Reference study period", "RSP (years)", "Manual input", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Reference service life", "RSL (years)", "Manual input", GH_ParamAccess.item);
            pManager.AddNumberParameter("PEnr(Emb)", "PEnr(Emb kWh oil-eq)", "Value", GH_ParamAccess.item);
            pManager.AddNumberParameter("PEnr(EoL)", "PEnr(EoL kWh oil-eq)", "Value", GH_ParamAccess.item);
            pManager.AddNumberParameter("GWP(Emb)", "GWP(Emb kg CO\x2082-eq)", "Value", GH_ParamAccess.item);
            pManager.AddNumberParameter("GWP(EoL)", "GWP(EoL kg CO\x2082-eq)", "Value", GH_ParamAccess.item);
            pManager.AddNumberParameter("UBP(Emb)", "UBP(Emb)", "Value", GH_ParamAccess.item);
            pManager.AddNumberParameter("UBP(EoL)", "UBP(EoL)", "Value", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("PEnr(Emb)", "PEnr(Emb kWh oil-eq)", "Value", GH_ParamAccess.item);
            pManager.AddNumberParameter("PEnr(Rep)", "PEnr(Rep kWh oil-eq)", "Value", GH_ParamAccess.item);
            pManager.AddNumberParameter("PEnr(EoL)", "PEnr(EoL kWh oil-eq)", "Value", GH_ParamAccess.item);
            pManager.AddNumberParameter("GWP(Emb)", "GWP(Emb kg CO\x2082-eq)", "Value", GH_ParamAccess.item);
            pManager.AddNumberParameter("GWP(Rep)", "GWP(Rep kg CO\x2082-eq)", "Value", GH_ParamAccess.item);
            pManager.AddNumberParameter("GWP(EoL)", "GWP(EoL kg CO\x2082-eq)", "Value", GH_ParamAccess.item);
            pManager.AddNumberParameter("UBP(Emb)", "UBP(Emb)", "Value", GH_ParamAccess.item);
            pManager.AddNumberParameter("UBP(Rep)", "UBP(Rep)", "Value", GH_ParamAccess.item);
            pManager.AddNumberParameter("UBP(EoL)", "UBP(EoL)", "Value", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get inputs
            var PEnrEmb = Params.Input[2].VolatileData.AllData(true);
            var PEnrEoL = Params.Input[3].VolatileData.AllData(true);
            var GWPEmb = Params.Input[4].VolatileData.AllData(true);
            var GWPEoL = Params.Input[5].VolatileData.AllData(true);
            var UBPEmb = Params.Input[6].VolatileData.AllData(true);
            var UBPEoL = Params.Input[7].VolatileData.AllData(true);
            var RSP = 0;
            if (!DA.GetData("Reference study period", ref RSP)) { return; }
            var RSL = 0;
            if (!DA.GetData("Reference service life", ref RSL)) { return; }

            int repNum = 0;
            if (RSL != 0 && RSP != 0)
            {
                repNum = (RSP / RSL) - 1;
            }

            var sumPEemb = 0d;
            var sumPEeol = 0d;
            var sumGWPemb = 0d;
            var sumGWPeol = 0d;
            var sumUBPemb = 0d;
            var sumUBPeol = 0d;

            foreach (var item in PEnrEmb)
            {
                var tmp = 0d;
                item.CastTo(out tmp);
                sumPEemb += tmp;
            }

            foreach (var item in PEnrEoL)
            {
                var tmp = 0d;
                item.CastTo(out tmp);
                sumPEeol += tmp;
            }

            foreach (var item in GWPEmb)
            {
                var tmp = 0d;
                item.CastTo(out tmp);
                sumGWPemb += tmp;
            }

            foreach (var item in GWPEoL)
            {
                var tmp = 0d;
                item.CastTo(out tmp);
                sumGWPeol += tmp;
            }

            foreach (var item in UBPEmb)
            {
                var tmp = 0d;
                item.CastTo(out tmp);
                sumUBPemb += tmp;
            }

            foreach (var item in UBPEoL)
            {
                var tmp = 0d;
                item.CastTo(out tmp);
                sumUBPeol += tmp;
            }

            // set outputs
            DA.SetData("PEnr(Emb)", sumPEemb);
            DA.SetData("PEnr(Rep)", (sumPEemb + sumPEeol) * repNum);
            DA.SetData("PEnr(EoL)", sumPEeol);
            DA.SetData("GWP(Emb)", sumGWPemb);
            DA.SetData("GWP(Rep)", (sumGWPemb + sumGWPeol) * repNum);
            DA.SetData("GWP(EoL)", sumGWPeol);
            DA.SetData("UBP(Emb)", sumUBPemb);
            DA.SetData("UBP(Rep)", (sumUBPemb + sumUBPeol) * repNum);
            DA.SetData("UBP(EoL)", sumUBPeol);

            for (int x = 0; x < Params.Output.Count; x++)
            {
                for (int i = 0; i < Params.Output[x].VolatileDataCount - 1; i++)
                {
                    Params.Output[x].VolatileData.get_Branch(0).RemoveAt(i);
                }
            }

            for (int j = 0; j < Params.Output.Count; j++)
            {
                if (Params.Output[j].VolatileData.get_Branch(0)[0] == null)
                {
                    Params.Output[j].VolatileData.get_Branch(0).RemoveAt(0);
                }
            }
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Icons.impactComponent;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("5ebf6cc0-12b8-4cd9-92ec-4b185cd60c59"); }
        }
    }
}