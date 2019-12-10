using System;
using Grasshopper.Kernel;
using System.Collections.Generic;
using System.Linq;

namespace Bombyx.Plugin.Impacts
{
    public class ImpactBuilding : GH_Component
    {
        public ImpactBuilding()
          : base("Building impact",
                 "Building impact",
                 "Calculates CO2 impact of the building",
                 "Bombyx",
                 "Impacts")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("LCA Factors\nElement", "LCA Factors\n(Element)", "List of LCA factors", GH_ParamAccess.list);
            pManager.AddNumberParameter("Reference study period (years)", "RSP (years)", "Reference study period (years)", GH_ParamAccess.item);
            pManager.AddNumberParameter("NFA (square meters)", "NFA (m\xB2)", "NFA (square meters)", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Global warming potential", "GWP (kg CO\x2082-eq/m\xB2 a)", "Global warming potential (kg CO\x2082-eq/m\xB2 a)", GH_ParamAccess.item);
            pManager.AddNumberParameter("PE Total", "PE Total (kWh oil-eq)", "PE Total (kWh oil-eq)", GH_ParamAccess.item);            
            pManager.AddNumberParameter("PE Renewable", "PE Renewable (kWh oil-eq)", "PE Renewable (kWh oil-eq)", GH_ParamAccess.item);
            pManager.AddNumberParameter("PE Non-Renewable", "PE Non-Renewable (kWh oil-eq)", "PE Non-Renewable (kWh oil-eq)", GH_ParamAccess.item);
            pManager.AddNumberParameter("UBP impact", "UBP (P/m\xB2 a)", "UBP (P/m\xB2 a)", GH_ParamAccess.item);
            pManager.AddTextParameter("LCA factors (text)", "LCA factors (text)", "Building Properties (text)", GH_ParamAccess.item);
            pManager.AddNumberParameter("LCA factors (values)", "LCA factors (values)", "Building Properties (values)", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var element = new List<double>();
            if (!DA.GetDataList(0, element)) { return; }
            var RSP = 0d;
            if (!DA.GetData(1, ref RSP)) { return; }
            var NFA = 0d;
            if (!DA.GetData(2, ref NFA)) { return; }

            var rspNFA = RSP * NFA;

            var valueSets = element.Select((x, i) => new { Index = i, Value = x })
                                   .GroupBy(x => x.Index / 16)
                                   .Select(x => x.Select(v => v.Value).ToList())
                                   .ToList();

            var results = new Dictionary<string, double>
            {
                { "UBP13 Embodied (P/m\xB2 a)", 0 },
                { "UBP13 Replacements (P/m\xB2 a)", 0 },
                { "UBP13 End of Life (P/m\xB2 a)", 0 },
                { "Total Embodied (kWh oil-eq)", 0 },
                { "Total Replacements (kWh oil-eq)", 0 },
                { "Total End of Life (kWh oil-eq)", 0 },
                { "Renewable Embodied (kWh oil-eq)", 0 },
                { "Renewable Replacements (kWh oil-eq)", 0 },
                { "Renewable End of Life (kWh oil-eq)", 0 },
                { "Non Renewable Embodied (kWh oil-eq)", 0 },
                { "Non Renewable Replacements (kWh oil-eq)", 0 },
                { "Non Renewable End of Life (kWh oil-eq)", 0 },
                { "Green House Gasses Embodied (kg CO\x2082-eq/m\xB2 a)", 0 },
                { "Green House Gasses Replacements (kg CO\x2082-eq/m\xB2 a)", 0 },
                { "Green House Gasses End of Life (kg CO\x2082-eq/m\xB2 a)", 0 },
                { "U value", 0 }
            };

            foreach (var item in valueSets)
            {
                results["UBP13 Embodied (P/m\xB2 a)"] += item[0];
                results["UBP13 Replacements (P/m\xB2 a)"] += item[1];
                results["UBP13 End of Life (P/m\xB2 a)"] += item[2];
                results["Total Embodied (kWh oil-eq)"] += item[3];
                results["Total Replacements (kWh oil-eq)"] += item[4];
                results["Total End of Life (kWh oil-eq)"] += item[5];
                results["Renewable Embodied (kWh oil-eq)"] += item[6];
                results["Renewable Replacements (kWh oil-eq)"] += item[7];
                results["Renewable End of Life (kWh oil-eq)"] += item[8];
                results["Non Renewable Embodied (kWh oil-eq)"] += item[9];
                results["Non Renewable Replacements (kWh oil-eq)"] += item[10];
                results["Non Renewable End of Life (kWh oil-eq)"] += item[11];
                results["Green House Gasses Embodied (kg CO\x2082-eq/m\xB2 a)"] += item[12];
                results["Green House Gasses Replacements (kg CO\x2082-eq/m\xB2 a)"] += item[13];
                results["Green House Gasses End of Life (kg CO\x2082-eq/m\xB2 a)"] += item[14];
                results["U value"] += item[15];
            }

            results["UBP13 Embodied (P/m\xB2 a)"] /= rspNFA;
            results["UBP13 Replacements (P/m\xB2 a)"] /= rspNFA;
            results["UBP13 End of Life (P/m\xB2 a)"] /= rspNFA;
            results["Total Embodied (kWh oil-eq)"] /= rspNFA;
            results["Total Replacements (kWh oil-eq)"] /= rspNFA;
            results["Total End of Life (kWh oil-eq)"] /= rspNFA;
            results["Renewable Embodied (kWh oil-eq)"] /= rspNFA;
            results["Renewable Replacements (kWh oil-eq)"] /= rspNFA;
            results["Renewable End of Life (kWh oil-eq)"] /= rspNFA;
            results["Non Renewable Embodied (kWh oil-eq)"] /= rspNFA;
            results["Non Renewable Replacements (kWh oil-eq)"] /= rspNFA;
            results["Non Renewable End of Life (kWh oil-eq)"] /= rspNFA;
            results["Green House Gasses Embodied (kg CO\x2082-eq/m\xB2 a)"] /= rspNFA;
            results["Green House Gasses Replacements (kg CO\x2082-eq/m\xB2 a)"] /= rspNFA;
            results["Green House Gasses End of Life (kg CO\x2082-eq/m\xB2 a)"] /= rspNFA;

            var gwp = Math.Round((results["Green House Gasses Embodied (kg CO\x2082-eq/m\xB2 a)"] + 
                                  results["Green House Gasses Replacements (kg CO\x2082-eq/m\xB2 a)"] + 
                                  results["Green House Gasses End of Life (kg CO\x2082-eq/m\xB2 a)"]), 4);
            var total = Math.Round((results["Total Embodied (kWh oil-eq)"] + 
                                    results["Total Replacements (kWh oil-eq)"] + 
                                    results["Total End of Life (kWh oil-eq)"]), 4);
            var ubp = Math.Round((results["UBP13 Embodied (P/m\xB2 a)"] + 
                                  results["UBP13 Replacements (P/m\xB2 a)"] + 
                                  results["UBP13 End of Life (P/m\xB2 a)"]), 4);
            var renew = Math.Round((results["Renewable Embodied (kWh oil-eq)"] + 
                                    results["Renewable Replacements (kWh oil-eq)"] + 
                                    results["Renewable End of Life (kWh oil-eq)"]), 4);
            var nonrenew = Math.Round((results["Non Renewable Embodied (kWh oil-eq)"] + 
                                       results["Non Renewable Replacements (kWh oil-eq)"] + 
                                       results["Non Renewable End of Life (kWh oil-eq)"]), 4);

            DA.SetData(0, gwp);
            DA.SetData(1, total);          
            DA.SetData(2, renew);
            DA.SetData(3, nonrenew);
            DA.SetData(4, ubp);

            var resultValues = results.Values.ToList();

            DA.SetDataList(5, results);
            DA.SetDataList(6, resultValues);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Icons._4building;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("874663b3-137a-4d57-9975-dc6e645f6839"); }
        }
    }
}