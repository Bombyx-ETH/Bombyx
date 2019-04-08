using System;
using Grasshopper.Kernel;
using System.Collections.Generic;
using System.Linq;

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
            pManager.AddNumberParameter("Frame Properties", "Frame\nProperties", "List of frame properties", GH_ParamAccess.list);
            pManager.AddNumberParameter("Filling Properties", "Filling\nProperties", "List of filling properties", GH_ParamAccess.list);
            pManager.AddNumberParameter("Frame percentage", "Frame percentage", "Value", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Reference study period", "RSP (years)", "Manual input", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Reference service life", "RSL (years)", "Manual input", GH_ParamAccess.item);
            pManager.AddNumberParameter("U value", "U value\n(W/m2*K)", "U value", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Window/Door Properties", "Window/Door\nProperties", "Window properties order: " +
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
                "\n16: U-value", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var frame = new List<double>();
            if (!DA.GetDataList(0, frame)) { return; }
            var filling = new List<double>();
            if (!DA.GetDataList(1, filling)) { return; }
            var frameInput = 0d;
            if (!DA.GetData(2, ref frameInput)) { return; }
            var framePercent = frameInput / 100;
            var fillingPercent = 1 - framePercent;
            var RSP = 0;
            if (!DA.GetData(3, ref RSP)) { return; }
            var RSL = 0;
            if (!DA.GetData(4, ref RSL)) { return; }
            var uValue = 0d;
            if (!DA.GetData(5, ref uValue)) { return; }

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

            var window = frame.Zip(filling, (a, b) => ((a * framePercent) + (b * fillingPercent))).ToList();

            result.Add(1);
            result.Add(window[1]); //UBP13Embodied 1
            result.Add((window[1] + window[2]) * repNum); //UBP13Rep 2
            result.Add(window[2]); //UBP13EoL 3
            result.Add(window[3]); //TotalEmbodied 4
            result.Add((window[3] + window[4]) * repNum); //TotalRep 5
            result.Add(window[4]); //TotalEoL 6
            result.Add(window[5]); //RenewableEmbodied 7
            result.Add((window[5] + window[6]) * repNum); //RenewableRep 8
            result.Add(window[6]); //RenewableRep 9
            result.Add(window[7]); //NonRenewableEmbodied 10
            result.Add((window[7] + window[8]) * repNum); //NonRenewableRep 11
            result.Add(window[8]); //NonRenewableEoL 12
            result.Add(window[9]); //GHGEmbodied 13
            result.Add((window[9] + window[10]) * repNum); //GHGRep 14
            result.Add(window[10]); //GHGEoL 15
            result.Add(uValue);

            DA.SetDataList(0, result);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Icons._1window;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("37fb1032-e3e4-4853-8428-61f26c94afc0"); }
        }
    }
}