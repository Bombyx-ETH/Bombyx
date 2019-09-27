using System;
using Grasshopper.Kernel;
using System.Collections.Generic;
using System.Linq;

namespace Bombyx.Plugin.InputLevel
{
    public class BuildingOutput : GH_Component
    {
        public BuildingOutput()
          : base("Building Output",
                 "Building output",
                 "Description",
                 "Bombyx",
                 "Input level")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Element output\n(avg, min, max)", "Element output\n(avg, min, max)", "Element output\n(avg, min, max)", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Building avg output", "Building avg output", "Building avg output", GH_ParamAccess.item);
            pManager.AddTextParameter("Building min output", "Building min output", "Building min output", GH_ParamAccess.item);
            pManager.AddTextParameter("Building max output", "Building max output", "Building max output", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var input = new List<double>();
            if (!DA.GetDataList(0, input)) { return; }

            var valueSets = input.Select((x, i) => new { Index = i, Value = x })
                                 .GroupBy(x => x.Index / 30)
                                 .Select(x => x.Select(v => v.Value).ToList())
                                 .ToList();

            var results = new Dictionary<string, double>
            {
                { "Average UBP13 Embodied", 0 },
                { "Average UBP13 End of Life", 0 },
                { "Average Total Embodied", 0 },
                { "Average Total End of Life", 0 },
                { "Average Renewable Embodied", 0 },
                { "Average Renewable End of Life", 0 },
                { "Average Non Renewable Embodied", 0 },
                { "Average Non Renewable End of Life", 0 },
                { "Average Green House Gasses Embodied", 0 },
                { "Average Green House Gasses End of Life", 0 },
                { "Minumum UBP13 Embodied", 0 },
                { "Minumum UBP13 End of Life", 0 },
                { "Minumum Total Embodied", 0 },
                { "Minumum Total End of Life", 0 },
                { "Minumum Renewable Embodied", 0 },
                { "Minumum Renewable End of Life", 0 },
                { "Minumum Non Renewable Embodied", 0 },
                { "Minumum Non Renewable End of Life", 0 },
                { "Minumum Green House Gasses Embodied", 0 },
                { "Minumum Green House Gasses End of Life", 0 },
                { "Maximum UBP13 Embodied", 0 },
                { "Maximum UBP13 End of Life", 0 },
                { "Maximum Total Embodied", 0 },
                { "Maximum Total End of Life", 0 },
                { "Maximum Renewable Embodied", 0 },
                { "Maximum Renewable End of Life", 0 },
                { "Maximum Non Renewable Embodied", 0 },
                { "Maximum Non Renewable End of Life", 0 },
                { "Maximum Green House Gasses Embodied", 0 },
                { "Maximum Green House Gasses End of Life", 0 }
        };

            foreach (var item in valueSets)
            {
                results["Average UBP13 Embodied"] += item[0];
                results["Average UBP13 End of Life"] += item[1];
                results["Average Total Embodied"] += item[2];
                results["Average Total End of Life"] += item[3];
                results["Average Renewable Embodied"] += item[4];
                results["Average Renewable End of Life"] += item[5];
                results["Average Non Renewable Embodied"] += item[6];
                results["Average Non Renewable End of Life"] += item[7];
                results["Average Green House Gasses Embodied"] += item[8];
                results["Average Green House Gasses End of Life"] += item[9];
                results["Minumum UBP13 Embodied"] += item[10];
                results["Minumum UBP13 End of Life"] += item[11];
                results["Minumum Total Embodied"] += item[12];
                results["Minumum Total End of Life"] += item[13];
                results["Minumum Renewable Embodied"] += item[14];
                results["Minumum Renewable End of Life"] += item[15];
                results["Minumum Non Renewable Embodied"] += item[16];
                results["Minumum Non Renewable End of Life"] += item[17];
                results["Minumum Green House Gasses Embodied"] += item[18];
                results["Minumum Green House Gasses End of Life"] += item[19];
                results["Maximum UBP13 Embodied"] += item[20];
                results["Maximum UBP13 End of Life"] += item[21];
                results["Maximum Total Embodied"] += item[22];
                results["Maximum Total End of Life"] += item[23];
                results["Maximum Renewable Embodied"] += item[24];
                results["Maximum Renewable End of Life"] += item[25];
                results["Maximum Non Renewable Embodied"] += item[26];
                results["Maximum Non Renewable End of Life"] += item[27];
                results["Maximum Green House Gasses Embodied"] += item[28];
                results["Maximum Green House Gasses End of Life"] += item[29];
            }

            var resultsAVG = results.Where(s => s.Key.Contains("Average")).ToDictionary(dict => dict.Key, dict => dict.Value);
            var resultsMIN = results.Where(s => s.Key.Contains("Minumum")).ToDictionary(dict => dict.Key, dict => dict.Value);
            var resultsMAX = results.Where(s => s.Key.Contains("Maximum")).ToDictionary(dict => dict.Key, dict => dict.Value);

            DA.SetDataList(0, resultsAVG);
            DA.SetDataList(1, resultsMIN);
            DA.SetDataList(2, resultsMAX);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Icons._5;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("c1fcb5a5-3d29-4901-8ec6-0c6f4edb0ef1"); }
        }
    }
}