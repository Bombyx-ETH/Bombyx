using System;
using Grasshopper.Kernel;

namespace Bombyx.Plugin.Impacts
{
    public class ImpactWindowDoor : GH_Component
    {
        public ImpactWindowDoor()
          : base("Window / Door impact",
                 "Window / Door impact",
                 "Calculates impacts of PE, GWP, UBP",
                 "Bombyx",
                 "Impacts")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("PEnr(Emb)", "Frame PEnr(Emb kWh oil-eq)", "Value", GH_ParamAccess.item, 0d);
            pManager.AddNumberParameter("PEnr(EoL)", "Frame PEnr(EoL kWh oil-eq)", "Value", GH_ParamAccess.item, 0d);
            pManager.AddNumberParameter("GWP(Emb)", "Frame GWP(Emb kg CO\x2082-eq)", "Value", GH_ParamAccess.item, 0d);
            pManager.AddNumberParameter("GWP(EoL)", "Frame GWP(EoL kg CO\x2082-eq)", "Value", GH_ParamAccess.item, 0d);
            pManager.AddNumberParameter("UBP(Emb)", "Frame UBP(Emb)", "Value", GH_ParamAccess.item, 0d);
            pManager.AddNumberParameter("UBP(EoL)", "Frame UBP(EoL)", "Value", GH_ParamAccess.item, 0d);         
            pManager.AddNumberParameter("PEnr(Emb)", "Filling PEnr(Emb kWh oil-eq)", "Value", GH_ParamAccess.item, 0d);
            pManager.AddNumberParameter("PEnr(EoL)", "Filling PEnr(EoL kWh oil-eq)", "Value", GH_ParamAccess.item, 0d);
            pManager.AddNumberParameter("GWP(Emb)", "Filling GWP(Emb kg CO\x2082-eq)", "Value", GH_ParamAccess.item, 0d);
            pManager.AddNumberParameter("GWP(EoL)", "Filling GWP(EoL kg CO\x2082-eq)", "Value", GH_ParamAccess.item, 0d);
            pManager.AddNumberParameter("UBP(Emb)", "Filling UBP(Emb)", "Value", GH_ParamAccess.item, 0d);
            pManager.AddNumberParameter("UBP(EoL)", "Filling UBP(EoL)", "Value", GH_ParamAccess.item, 0d);
            pManager.AddNumberParameter("Frame percentage", "Frame percentage", "Value", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Reference study period", "RSP (years)", "Manual input", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Reference service life", "RSL (years)", "Manual input", GH_ParamAccess.item);

            pManager[0].Optional = true;
            pManager[1].Optional = true;
            pManager[2].Optional = true;
            pManager[3].Optional = true;
            pManager[4].Optional = true;
            pManager[5].Optional = true;
            pManager[6].Optional = true;
            pManager[7].Optional = true;
            pManager[8].Optional = true;
            pManager[9].Optional = true;
            pManager[10].Optional = true;
            pManager[11].Optional = true;
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
            var PEnrEmbFrame = 0d;
            if (!DA.GetData(0, ref PEnrEmbFrame)) { return; }
            var PEnrEoLFrame = 0d;
            if (!DA.GetData(1, ref PEnrEoLFrame)) { return; }
            var GWPEmbFrame = 0d;
            if (!DA.GetData(2, ref GWPEmbFrame)) { return; }
            var GWPEoLFrame = 0d;
            if (!DA.GetData(3, ref GWPEoLFrame)) { return; }
            var UBPEmbFrame = 0d;
            if (!DA.GetData(4, ref UBPEmbFrame)) { return; }
            var UBPEoLFrame = 0d;
            if (!DA.GetData(5, ref UBPEoLFrame)) { return; }          
            var PEnrEmbFilling = 0d;
            if (!DA.GetData(6, ref PEnrEmbFilling)) { return; }
            var PEnrEoLFilling = 0d;
            if (!DA.GetData(7, ref PEnrEoLFilling)) { return; }
            var GWPEmbFilling = 0d;
            if (!DA.GetData(8, ref GWPEmbFilling)) { return; }
            var GWPEoLFilling = 0d;
            if (!DA.GetData(9, ref GWPEoLFilling)) { return; }
            var UBPEmbFilling = 0d;
            if (!DA.GetData(10, ref UBPEmbFilling)) { return; }
            var UBPEoLFilling = 0d;
            if (!DA.GetData(11, ref UBPEoLFilling)) { return; }
            var frameInput = 0d;
            if (!DA.GetData("Frame percentage", ref frameInput)) { return; }
            var framePercent = frameInput / 100;
            var fillingPercent = 1 - framePercent;
            var RSP = 0;
            if (!DA.GetData("Reference study period", ref RSP)) { return; }
            var RSL = 0;
            if (!DA.GetData("Reference service life", ref RSL)) { return; }

            int repNum = 0;
            if (RSL != 0 && RSP != 0)
            {
                repNum = (RSP / RSL) - 1;
            }
            if(repNum < 0)
            {
                repNum = 0;
            }
            if (repNum > 0 && repNum < 1)
            {
                repNum = 1;
            }

            var sumPEemb = (PEnrEmbFrame * framePercent) + (PEnrEmbFilling * fillingPercent);
            var sumPEeol = (PEnrEoLFrame * framePercent) + (PEnrEoLFilling * fillingPercent);
            var sumGWPemb = (GWPEmbFrame * framePercent) + (GWPEmbFilling * fillingPercent);
            var sumGWPeol = (GWPEoLFrame * framePercent) + (GWPEoLFilling * fillingPercent);
            var sumUBPemb = (UBPEmbFrame * framePercent) + (UBPEmbFilling * fillingPercent);
            var sumUBPeol = (UBPEoLFrame * framePercent) + (UBPEoLFilling * fillingPercent);

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
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Icons.impactWindowDoor;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("37fb1032-e3e4-4853-8428-61f26c94afc0"); }
        }
    }
}