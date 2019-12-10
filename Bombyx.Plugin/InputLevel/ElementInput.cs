using System;
using Grasshopper.Kernel;
using System.Collections.Generic;
using Grasshopper.Kernel.Special;
using System.Drawing;
using Bombyx.Data.InputLevel;
using System.Linq;

namespace Bombyx.Plugin.InputLevel
{
    public class ElementInput : GH_Component
    {
        GH_Document GrasshopperDocument;
        IGH_Component Component;
        int counter = 0;

        private string[] FUNCTIONALITY = new string[] { "Exterior wall above ground",
                                                        "Exterior wall under ground",
                                                        "Interior wall",
                                                        "Partition wall",
                                                        "Column",
                                                        "Ceiling",
                                                        "Foundation",
                                                        "Pitched roof",
                                                        "Flat roof",
                                                        "Balcony",
                                                        "Windows" };

        private string[] CONSTRUCTION_TYPE = new string[] { "Homogenous (concrete)", "Inhomogenous (concrete)",
                                                            "Massive (wood)", "Frame (wood)",
                                                            "With insulation (brick)", "Without insulation (brick)"};
        
        private string[] STRUCTUAL_VALUES = new string[] { "Concrete", "Wood", "Brick", "Steel" };

        public ElementInput()
          : base("Element Input",
                 "Element input",
                 "Description",
                 "Bombyx",
                 "Input level")
        {
            Message = "Connect Building input\nto the first input parameter.";
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Building input", "Building input", "Building input", GH_ParamAccess.list);
            pManager[0].Optional = true;
            pManager.AddTextParameter("Building functionality", "Building functionality", "Building functionality", GH_ParamAccess.item);
            pManager[1].Optional = true;
            pManager.AddTextParameter("Construction type", "Construction type", "Construction type", GH_ParamAccess.item);
            pManager[2].Optional = true;
            //pManager.AddTextParameter("Structural material (testing)", "Structural material (testing)", "Structural material (testing)", GH_ParamAccess.item);
            //pManager[3].Optional = true;
            pManager.AddIntegerParameter("Area (m\xB2)", "Area (m\xB2)", "Area (m\xB2)", GH_ParamAccess.item);
            pManager[3].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Element output\n(debug)", "Element output\n(debug)", "Element output\n(debug)", GH_ParamAccess.list);
            pManager.AddTextParameter("Element output\n(avg, min, max)", "Element output\n(avg, min, max)", "Element output\n(avg, min, max)", GH_ParamAccess.list);
            pManager.AddTextParameter("Element output\n(detail)", "Element output\n(detail)", "Element output\n(detail)", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Component = this;
            GrasshopperDocument = OnPingDocument();

            string functionality = "";
            string constType = "";
            //string structural = "";
            int area = 0;
            string[] sqlLoop = new string[] { "AVG", "MIN", "MAX" };

            var input = new List<string>();
            var output = new Dictionary<string, decimal>();

            var isConnected = false;
            if (Params.Input[0].SourceCount == 1)
            {
                isConnected = true;
                Message = "Component activated.";
                if (!DA.GetDataList(0, input)) { return; }
                counter++;                
            }

            if (counter == 1)
            {
                CreateAccentList(FUNCTIONALITY, "Functionality", 1, 280, -20);
                CreateAccentList(CONSTRUCTION_TYPE, "Construction type", 2, 275, -10);
                //CreateAccentList(STRUCTUAL_VALUES, "Structual material", 3, 195, 0);

                counter++;
            }

            if (isConnected)
            {
                var element = "";

                DA.GetData(1, ref functionality);
                DA.GetData(2, ref constType);
                //.GetData(3, ref structural);

                switch (functionality)
                {
                    case "Exterior wall above ground":
                        element = "('C2.1', 'E2', 'G3')";
                        break;
                    case "Exterior wall under ground":
                        element = "('C2.1', 'E1')";
                        break;
                    case "Interior wall":
                        element = "('C2.2', 'G3')";
                        break;
                    case "Partition wall":
                        element = "('G1', 'G3')";
                        break;
                    case "Column":
                        element = "('C3')";
                        break;
                    case "Ceiling":
                        element = "('C4.1', 'G4', 'G2')";
                        break;
                    case "Foundation":
                        element = "('C1', 'G2')";
                        break;
                    case "Pitched roof":
                        element = "('C4.4', 'F1', 'G4')";
                        break;
                    case "Flat roof":
                        element = "('C4.4', 'F1', 'G4')";
                        break;
                    case "Balcony":
                        element = "('C4.3')";
                        break;
                    case "Windows":
                        element = "window";
                        break;
                }

                DA.GetData(3, ref area);

                foreach (var query in sqlLoop)
                {
                    var sqlOutput = "";
                    if (!element.Equals("window"))
                    {
                        sqlOutput = "SELECT SUM(x.Ubp13Embodied) AS Ubp13Embodied, SUM(x.Ubp13EoL) AS Ubp13EoL, " +
                                    "SUM(x.TotalEmbodied) AS TotalEmbodied, SUM(x.TotalEoL) AS TotalEoL, " +
                                    "SUM(x.RenewableEmbodied)AS RenewableEmbodied, SUM(x.RenewableEoL) AS RenewableEoL, " +
                                    "SUM(x.NonRenewableEmbodied) AS NonRenewableEmbodied, SUM(x.NonRenewableEoL) AS NonRenewableEoL, " +
                                    "SUM(x.GHGEmbodied) AS GHGEmbodied, SUM(x.GHGEoL) AS GHGEoL ";

                        sqlOutput += "FROM (SELECT bc.ComponentCode, " + query + "(kmg.Ubp13Embodied) AS Ubp13Embodied, " +
                                     "" + query + "(kmg.Ubp13EoL) AS Ubp13EoL, " + query + "(kmg.TotalEmbodied) AS TotalEmbodied, " +
                                     "" + query + "(kmg.TotalEoL) AS TotalEoL, " + query + "(kmg.RenewableEmbodied)AS RenewableEmbodied, " +
                                     "" + query + "(kmg.RenewableEoL) AS RenewableEoL, " + query + "(kmg.NonRenewableEmbodied) AS NonRenewableEmbodied, " +
                                     "" + query + "(kmg.NonRenewableEoL) AS NonRenewableEoL, " + query + "(kmg.GHGEmbodied) AS GHGEmbodied, " +
                                     "" + query + "(kmg.GHGEoL) AS GHGEoL " +
                                     "FROM dbo.BtkComponent bc LEFT JOIN dbo.KbobMaterialGen kmg ON bc.SortCode = kmg.SortCode " +
                                     "WHERE bc.ComponentCode IN " + element + " AND ";

                        switch (input[0])
                        {
                            case "Small":
                                sqlOutput += "bc.BuildingSizeSmall = 1 ";
                                break;
                            case "Mid-size":
                                sqlOutput += "bc.BuildingSizeMid = 1 ";
                                break;
                            case "Highrise":
                                sqlOutput += "bc.BuildingSizeHighrise = 1 ";
                                break;
                        }

                        switch (input[1])
                        {
                            case "Residential single family":
                                sqlOutput += "AND bc.BuildingUsageSF = 1 ";
                                break;
                            case "Residential multi family":
                                sqlOutput += "AND bc.BuildingUsageMF = 1 ";
                                break;
                            case "Office":
                                sqlOutput += "AND bc.BuildingUsageOffice = 1 ";
                                break;
                        }

                        switch (input[2])
                        {
                            case "Standard":
                                sqlOutput += "AND bc.BuildingEnergyStandard = 1 ";
                                break;
                            case "Above average":
                                sqlOutput += "AND bc.BuildingEnergyAboveAvg = 1 ";
                                break;
                            case "Passive house":
                                sqlOutput += "AND bc.BuildingEnergyPassivehouse = 1 ";
                                break;
                        }

                        switch (input[3])
                        {
                            case "Concrete":
                                sqlOutput += "AND bc.StructMaterialConcrete = 1 ";
                                break;
                            case "Wood":
                                sqlOutput += "AND bc.StructMaterialWood = 1 ";
                                break;
                            case "Brick":
                                sqlOutput += "AND bc.StructMaterialBrick = 1 ";
                                break;
                            case "Steel":
                                sqlOutput += "AND bc.StructMaterialSteel = 1 ";
                                break;
                        }

                        sqlOutput += "GROUP BY bc.ComponentCode) x";
                    }
                    else
                    {
                        sqlOutput = "SELECT " + query + "(km.Ubp13Embodied) AS Ubp13Embodied, " + query + "(km.Ubp13EoL) AS Ubp13EoL, " +
                                    "" + query + "(km.TotalEmbodied) AS TotalEmbodied, " + query + "(km.TotalEoL) AS TotalEoL, " +
                                    "" + query + "(km.RenewableEmbodied)AS RenewableEmbodied, " + query + "(km.RenewableEoL) AS RenewableEoL, " +
                                    "" + query + "(km.NonRenewableEmbodied) AS NonRenewableEmbodied, " + query + "(km.NonRenewableEoL) AS NonRenewableEoL, " +
                                    "" + query + "(km.GHGEmbodied) AS GHGEmbodied, " + query + "(km.GHGEoL) AS GHGEoL " +
                                    "FROM dbo.BtkWindows bw " +
                                    "LEFT JOIN dbo.BtkKbobWindow bkw " +
                                    "ON bw.SortCode = bkw.SortCode " +
                                    "LEFT JOIN dbo.KbobMaterial km " +
                                    "ON bkw.IdKbob = km.Id";
                    }                   

                    foreach (var item in InputData.GetElementsInputList(sqlOutput))
                    {
                        output.Add(query + " UBP13 Embodied (P/m\xB2 a)", item.UBP13Embodied * area);
                        output.Add(query + " UBP13 End of Life (P/m\xB2 a)", item.UBP13EoL * area);
                        output.Add(query + " Total Embodied (kWh oil-eq)", item.TotalEmbodied * area);
                        output.Add(query + " Total End of Life (kWh oil-eq)", item.TotalEoL * area);
                        output.Add(query + " Renewable Embodied (kWh oil-eq)", item.RenewableEmbodied * area);
                        output.Add(query + " Renewable End of Life (kWh oil-eq)", item.RenewableEoL * area);
                        output.Add(query + " Non Renewable Embodied (kWh oil-eq)", item.NonRenewableEmbodied * area);
                        output.Add(query + " Non Renewable End of Life (kWh oil-eq)", item.NonRenewableEoL * area);
                        output.Add(query + " Green House Gases Embodied (kg CO\x2082-eq/m\xB2 a)", item.GHGEmbodied * area);
                        output.Add(query + " Green House Gases End of Life (kg CO\x2082-eq/m\xB2 a)", item.GHGEoL * area);
                    }
                }   
            }

            var outputValues = output.Values.ToList();

            DA.SetDataList(0, output);
            DA.SetDataList(1, outputValues);
        }

        private void CreateAccentList(string[] values, string nick, int inputParam, int offsetX, int offsetY)
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
                return Icons._2;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("a852c291-c278-40af-b9cc-2c783126e86e"); }
        }
    }
}