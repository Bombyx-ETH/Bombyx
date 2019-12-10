using System;
using Grasshopper.Kernel;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel.Special;
using System.Drawing;

namespace Bombyx.Plugin.Impacts
{
    public class ImpactElement : GH_Component
    {
        GH_Document GrasshopperDocument;
        IGH_Component Component;
        bool isConnected = false;

        private string[] FUNCT_VALUES = new string[] { "External wall", "Internal wall", "Floor", "Ceiling", "Roof", "Window" };

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
            pManager.AddNumberParameter("Component properties", "Component\nproperties", "List of component properties", GH_ParamAccess.list);
            pManager.AddTextParameter("Functionality", "Functionality", "By selecting element functionality, air resistance will be added to the U value.", GH_ParamAccess.item);
            pManager[1].Optional = true;
            pManager.AddNumberParameter("Area (square meters)", "Area (m\xB2)", "Manual value", GH_ParamAccess.item);
            pManager[2].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("LCA factors (text)", "LCA factors (text)", "Element Properties (text)", GH_ParamAccess.item);
            pManager.AddNumberParameter("LCA factors (values)", "LCA factors (values)", "Element Properties (values)", GH_ParamAccess.item);
            pManager.AddNumberParameter("U value", "U value\n(W/m2*K)", "U Value", GH_ParamAccess.item);
            pManager.AddNumberParameter("UA value", "UA value\n(W/K)", "Area U Value", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Component = this;
            GrasshopperDocument = OnPingDocument();

            var component = new List<double>();
            if (!DA.GetDataList(0, component)) { return; }

            if (!isConnected)
            {
                CreateFunctList(FUNCT_VALUES, "Functionality", 1, 200, -20);
                isConnected = true;
                ExpireSolution(true);
            }

            var funct = "";
            if (isConnected)
            {
                if (!DA.GetData(1, ref funct)) { return; }
            }

            var area = 0d;
            if (!DA.GetData(2, ref area)) { return; }          

            var valueSets = component.Select((x, i) => new { Index = i, Value = x })
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

            results["UBP13 Embodied (P/m\xB2 a)"] *= area;
            results["UBP13 Replacements (P/m\xB2 a)"] *= area;
            results["UBP13 End of Life (P/m\xB2 a)"] *= area;
            results["Total Embodied (kWh oil-eq)"] *= area;
            results["Total Replacements (kWh oil-eq)"] *= area;
            results["Total End of Life (kWh oil-eq)"] *= area;
            results["Renewable Embodied (kWh oil-eq)"] *= area;
            results["Renewable Replacements (kWh oil-eq)"] *= area;
            results["Renewable End of Life (kWh oil-eq)"] *= area;
            results["Non Renewable Embodied (kWh oil-eq)"] *= area;
            results["Non Renewable Replacements (kWh oil-eq)"] *= area;
            results["Non Renewable End of Life (kWh oil-eq)"] *= area;
            results["Green House Gasses Embodied (kg CO\x2082-eq/m\xB2 a)"] *= area;
            results["Green House Gasses Replacements (kg CO\x2082-eq/m\xB2 a)"] *= area;
            results["Green House Gasses End of Life (kg CO\x2082-eq/m\xB2 a)"] *= area;
            results["U value"] = 1 / results["U value"];

            var resultValues = results.Values.ToList();

            DA.SetDataList(0, results);
            DA.SetDataList(1, resultValues);

            if (funct.Equals("Window"))
            {
                DA.SetData(2, 1 / results["U value"]);
                DA.SetData(3, (1 / results["U value"]) * area);
            }
            else if (funct.Equals("Internal wall"))
            {
                DA.SetData(2, results["U value"] + 0.17);
                DA.SetData(3, (results["U value"] + 0.17) * area);
            }
            else if(funct.Equals("External wall"))
            {
                DA.SetData(2, results["U value"] + 0.17);
                DA.SetData(3, (results["U value"] + 0.17) * area);
            }
            else if (funct.Equals("Roof"))
            {
                DA.SetData(2, results["U value"] + 0.17);
                DA.SetData(3, (results["U value"] + 0.17) * area);
            }
            else if (funct.Equals("Ceiling"))
            {
                DA.SetData(2, results["U value"] + 0.17);
                DA.SetData(3, (results["U value"] + 0.17) * area);
            }
            else if (funct.Equals("Floor"))
            {
                DA.SetData(2, results["U value"] + 0.13);
                DA.SetData(3, (results["U value"] + 0.13) * area);
            }
        }

        private void CreateFunctList(string[] values, string nick, int inputParam, int offsetX, int offsetY)
        {
            GH_DocumentIO docIO = new GH_DocumentIO();
            docIO.Document = new GH_Document();

            GH_ValueList vl = new GH_ValueList();
            vl.ListItems.Clear();

            foreach (string item in values)
            {
                GH_ValueListItem vi = new GH_ValueListItem(item, String.Format("\"{0}\"", item));
                vl.ListItems.Add(vi);
            }

            vl.NickName = nick;
            GH_Document doc = OnPingDocument();
            if (docIO.Document == null) return;

            docIO.Document.AddObject(vl, false, inputParam);
            PointF currPivot = Params.Input[inputParam].Attributes.Pivot;
            vl.Attributes.Pivot = new PointF(currPivot.X - offsetX, currPivot.Y + offsetY);

            docIO.Document.SelectAll();
            docIO.Document.ExpireSolution();
            docIO.Document.MutateAllIds();
            IEnumerable<IGH_DocumentObject> objs = docIO.Document.Objects;
            doc.MergeDocument(docIO.Document);
            Component.Params.Input[inputParam].AddSource(vl);
            doc.DeselectAll();
        }

        protected override Bitmap Icon
        {
            get
            {
                return Icons._3elem;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("da9a9a35-cf9c-45e9-a54f-a9320cc22ccf"); }
        }
    }
}