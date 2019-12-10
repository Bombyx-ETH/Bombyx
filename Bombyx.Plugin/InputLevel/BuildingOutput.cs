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
            pManager.AddTextParameter("Building avg (text output)", "Building avg (text output)", "Building avg (text output)", GH_ParamAccess.item);
            pManager.AddTextParameter("Building avg (values output)", "Building avg (values output)", "Building avg (values output)", GH_ParamAccess.item);
            pManager.AddTextParameter("Building min (text output)", "Building min (text output)", "Building min (text output)", GH_ParamAccess.item);
            pManager.AddTextParameter("Building min (values output)", "Building min (values output)", "Building min (values output)", GH_ParamAccess.item);
            pManager.AddTextParameter("Building max (text output)", "Building max (text output)", "Building max (text output)", GH_ParamAccess.item);
            pManager.AddTextParameter("Building max (values output)", "Building max (values output)", "Building max (values output)", GH_ParamAccess.item);
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
                { "Average UBP13 Embodied (P/m\xB2 a)", 0 },
                { "Average UBP13 End of Life (P/m\xB2 a)", 0 },
                { "Average Total Embodied (kWh oil-eq)", 0 },
                { "Average Total End of Life (kWh oil-eq)", 0 },
                { "Average Renewable Embodied (kWh oil-eq)", 0 },
                { "Average Renewable End of Life (kWh oil-eq)", 0 },
                { "Average Non Renewable Embodied (kWh oil-eq)", 0 },
                { "Average Non Renewable End of Life (kWh oil-eq)", 0 },
                { "Average Green House Gasses Embodied (kg CO\x2082-eq/m\xB2 a)", 0 },
                { "Average Green House Gasses End of Life (kg CO\x2082-eq/m\xB2 a)", 0 },
                { "Minimum UBP13 Embodied (P/m\xB2 a)", 0 },
                { "Minimum UBP13 End of Life (P/m\xB2 a)", 0 },
                { "Minimum Total Embodied (kWh oil-eq)", 0 },
                { "Minimum Total End of Life (kWh oil-eq)", 0 },
                { "Minimum Renewable Embodied (kWh oil-eq)", 0 },
                { "Minimum Renewable End of Life (kWh oil-eq)", 0 },
                { "Minimum Non Renewable Embodied (kWh oil-eq)", 0 },
                { "Minimum Non Renewable End of Life (kWh oil-eq)", 0 },
                { "Minimum Green House Gasses Embodied (kg CO\x2082-eq/m\xB2 a)", 0 },
                { "Minimum Green House Gasses End of Life (kg CO\x2082-eq/m\xB2 a)", 0 },
                { "Maximum UBP13 Embodied (P/m\xB2 a)", 0 },
                { "Maximum UBP13 End of Life (P/m\xB2 a)", 0 },
                { "Maximum Total Embodied (kWh oil-eq)", 0 },
                { "Maximum Total End of Life (kWh oil-eq)", 0 },
                { "Maximum Renewable Embodied (kWh oil-eq)", 0 },
                { "Maximum Renewable End of Life (kWh oil-eq)", 0 },
                { "Maximum Non Renewable Embodied (kWh oil-eq)", 0 },
                { "Maximum Non Renewable End of Life (kWh oil-eq)", 0 },
                { "Maximum Green House Gasses Embodied (kg CO\x2082-eq/m\xB2 a)", 0 },
                { "Maximum Green House Gasses End of Life (kg CO\x2082-eq/m\xB2 a)", 0 }
        };

            foreach (var item in valueSets)
            {
                results["Average UBP13 Embodied (P/m\xB2 a)"] += item[0];
                results["Average UBP13 End of Life (P/m\xB2 a)"] += item[1];
                results["Average Total Embodied (kWh oil-eq)"] += item[2];
                results["Average Total End of Life (kWh oil-eq)"] += item[3];
                results["Average Renewable Embodied (kWh oil-eq)"] += item[4];
                results["Average Renewable End of Life (kWh oil-eq)"] += item[5];
                results["Average Non Renewable Embodied (kWh oil-eq)"] += item[6];
                results["Average Non Renewable End of Life (kWh oil-eq)"] += item[7];
                results["Average Green House Gasses Embodied (kg CO\x2082-eq/m\xB2 a)"] += item[8];
                results["Average Green House Gasses End of Life (kg CO\x2082-eq/m\xB2 a)"] += item[9];
                results["Minimum UBP13 Embodied (P/m\xB2 a)"] += item[10];
                results["Minimum UBP13 End of Life (P/m\xB2 a)"] += item[11];
                results["Minimum Total Embodied (kWh oil-eq)"] += item[12];
                results["Minimum Total End of Life (kWh oil-eq)"] += item[13];
                results["Minimum Renewable Embodied (kWh oil-eq)"] += item[14];
                results["Minimum Renewable End of Life (kWh oil-eq)"] += item[15];
                results["Minimum Non Renewable Embodied (kWh oil-eq)"] += item[16];
                results["Minimum Non Renewable End of Life (kWh oil-eq)"] += item[17];
                results["Minimum Green House Gasses Embodied (kg CO\x2082-eq/m\xB2 a)"] += item[18];
                results["Minimum Green House Gasses End of Life (kg CO\x2082-eq/m\xB2 a)"] += item[19];
                results["Maximum UBP13 Embodied (P/m\xB2 a)"] += item[20];
                results["Maximum UBP13 End of Life (P/m\xB2 a)"] += item[21];
                results["Maximum Total Embodied (kWh oil-eq)"] += item[22];
                results["Maximum Total End of Life (kWh oil-eq)"] += item[23];
                results["Maximum Renewable Embodied (kWh oil-eq)"] += item[24];
                results["Maximum Renewable End of Life (kWh oil-eq)"] += item[25];
                results["Maximum Non Renewable Embodied (kWh oil-eq)"] += item[26];
                results["Maximum Non Renewable End of Life (kWh oil-eq)"] += item[27];
                results["Maximum Green House Gasses Embodied (kg CO\x2082-eq/m\xB2 a)"] += item[28];
                results["Maximum Green House Gasses End of Life (kg CO\x2082-eq/m\xB2 a)"] += item[29];
            }

            var resultsAVG = results.Where(s => s.Key.Contains("Average")).ToDictionary(dict => dict.Key, dict => dict.Value);
            var resultsAVGValues = resultsAVG.Values.ToList();
            var resultsMIN = results.Where(s => s.Key.Contains("Minimum")).ToDictionary(dict => dict.Key, dict => dict.Value);
            var resultsMINValues = resultsMIN.Values.ToList();
            var resultsMAX = results.Where(s => s.Key.Contains("Maximum")).ToDictionary(dict => dict.Key, dict => dict.Value);
            var resultsMAXValues = resultsMAX.Values.ToList();

            DA.SetDataList(0, resultsAVG);
            DA.SetDataList(1, resultsAVGValues);
            DA.SetDataList(2, resultsMIN);
            DA.SetDataList(3, resultsMINValues);
            DA.SetDataList(4, resultsMAX);
            DA.SetDataList(5, resultsMAXValues);
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