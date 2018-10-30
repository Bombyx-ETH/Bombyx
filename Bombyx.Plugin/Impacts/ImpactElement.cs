using System;
using Grasshopper.Kernel;

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
            pManager.AddNumberParameter("PEnr(Emb)", "PEnr(Emb kWh oil-eq)", "Value", GH_ParamAccess.item, 0d);
            pManager.AddNumberParameter("PEnr(Rep)", "PEnr(Rep kWh oil-eq)", "Value", GH_ParamAccess.item, 0d);
            pManager.AddNumberParameter("PEnr(EoL)", "PEnr(EoL kWh oil-eq)", "Value", GH_ParamAccess.item, 0d);
            pManager.AddNumberParameter("GWP(Emb)", "GWP(Emb kg CO\x2082-eq)", "Value", GH_ParamAccess.item, 0d);
            pManager.AddNumberParameter("GWP(Rep)", "GWP(Rep kg CO\x2082-eq)", "Value", GH_ParamAccess.item, 0d);
            pManager.AddNumberParameter("GWP(EoL)", "GWP(EoL kg CO\x2082-eq)", "Value", GH_ParamAccess.item, 0d);
            pManager.AddNumberParameter("UBP(Emb)", "UBP(Emb)", "Value", GH_ParamAccess.item, 0d);            
            pManager.AddNumberParameter("UBP(Rep)", "UBP(Rep)", "Value", GH_ParamAccess.item, 0d);
            pManager.AddNumberParameter("UBP(EoL)", "UBP(EoL)", "Value", GH_ParamAccess.item, 0d);
            pManager.AddNumberParameter("Area (square meters)", "Area (m\xB2)", "Manual value", GH_ParamAccess.item);

            pManager[0].Optional = true;
            pManager[1].Optional = true;
            pManager[2].Optional = true;
            pManager[3].Optional = true;
            pManager[4].Optional = true;
            pManager[5].Optional = true;
            pManager[6].Optional = true;
            pManager[7].Optional = true;
            pManager[8].Optional = true;
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
            var PEnrEmb = Params.Input[0].VolatileData.AllData(true);
            var PEnrRep = Params.Input[1].VolatileData.AllData(true);
            var PEnrEoL = Params.Input[2].VolatileData.AllData(true);
            var GWPEmb = Params.Input[3].VolatileData.AllData(true);
            var GWPRep = Params.Input[4].VolatileData.AllData(true);
            var GWPEoL = Params.Input[5].VolatileData.AllData(true);
            var UBPEmb = Params.Input[6].VolatileData.AllData(true);
            var UBPRep = Params.Input[7].VolatileData.AllData(true);
            var UBPEoL = Params.Input[8].VolatileData.AllData(true);
            var area = 0d;
            if (!DA.GetData("Area", ref area)) { return; }

            var sumPEemb = 0d;
            var sumPErep = 0d;
            var sumPEeol = 0d;
            var sumGWPemb = 0d;
            var sumGWPrep = 0d;
            var sumGWPeol = 0d;
            var sumUBPemb = 0d;
            var sumUBPrep = 0d;
            var sumUBPeol = 0d;

            foreach (var item in PEnrEmb)
            {
                var tmp = 0d;
                item.CastTo(out tmp);
                sumPEemb += tmp;
            }

            foreach (var item in PEnrRep)
            {
                var tmp = 0d;
                item.CastTo(out tmp);
                sumPErep += tmp;
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

            foreach (var item in GWPRep)
            {
                var tmp = 0d;
                item.CastTo(out tmp);
                sumGWPrep += tmp;
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

            foreach (var item in UBPRep)
            {
                var tmp = 0d;
                item.CastTo(out tmp);
                sumUBPrep += tmp;
            }

            foreach (var item in UBPEoL)
            {
                var tmp = 0d;
                item.CastTo(out tmp);
                sumUBPeol += tmp;
            }

            // set outputs
            DA.SetData("PEnr(Emb)", sumPEemb * area);
            DA.SetData("PEnr(Rep)", sumPErep * area);
            DA.SetData("PEnr(EoL)", sumPEeol * area);
            DA.SetData("GWP(Emb)", sumGWPemb * area);
            DA.SetData("GWP(Rep)", sumGWPrep * area);
            DA.SetData("GWP(EoL)", sumGWPeol * area);
            DA.SetData("UBP(Emb)", sumUBPemb * area);
            DA.SetData("UBP(Rep)", sumUBPrep * area);
            DA.SetData("UBP(EoL)", sumUBPeol * area);

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
                return Icons.impactElement;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("da9a9a35-cf9c-45e9-a54f-a9320cc22ccf"); }
        }
    }
}