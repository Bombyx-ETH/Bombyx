using System;
using Grasshopper.Kernel;
using System.Collections.Generic;
using Grasshopper.Kernel.Special;
using System.Drawing;

namespace Bombyx.Plugin.InputLevel
{
    public class ElementInput : GH_Component
    {
        GH_Document GrasshopperDocument;
        IGH_Component Component;

        private string[] FUNCTIONALITY = new string[] { "Exterior wall above ground",
                                                        "Exterior wall under ground",
                                                        "Interior wall",
                                                        "Partition wall",
                                                        "Column",
                                                        "Ceiling",
                                                        "Slab",
                                                        "Pitched roof",
                                                        "Flat roof",
                                                        "Balcony",
                                                        "Windows" };

        private string[] CONSTRUCTION_TYPE = new string[] { "Homogenous (massive)", "Inhomogenous (frame)" };
        private string[] STRUCTUAL_VALUES = new string[] { "Concrete", "Wood", "Brick", "Steel" };

        public ElementInput()
          : base("Element Input",
                 "Element input",
                 "Description",
                 "Bombyx",
                 "Input level")
        {
            Message = "Connect Building input component\nto the first input parameter.";
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Building input", "Building input", "Building input", GH_ParamAccess.list);
            pManager[0].Optional = true;
            pManager.AddTextParameter("Building functionality", "Building functionality", "Building functionality", GH_ParamAccess.item);
            pManager[1].Optional = true;
            pManager.AddTextParameter("Construction type", "Construction type", "Construction type", GH_ParamAccess.item);
            pManager[2].Optional = true;
            pManager.AddTextParameter("Structural material", "Structural material", "Structural material", GH_ParamAccess.item);
            pManager[3].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Element output", "Element output", "Element output", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Component = this;
            GrasshopperDocument = OnPingDocument();

            string functionality = "";
            string constType = "";
            string structural = "";
            var input = new List<string>();

            var isConnected = false;
            if (Params.Input[0].SourceCount == 1)
            {
                isConnected = true;
                if (!DA.GetDataList(0, input)) { return; }
            }
            
            if (isConnected &&
                Params.Input[1].SourceCount == 0 &&
                Params.Input[2].SourceCount == 0 &&
                Params.Input[3].SourceCount == 0)
            {
                CreateAccentList(FUNCTIONALITY, "Functionality", 1, 280, 0);
                CreateAccentList(CONSTRUCTION_TYPE, "Construction type", 2, 265, 10);
                CreateAccentList(STRUCTUAL_VALUES, "Structual material", 3, 195, 20);
                Message = "Component connected.";

                DA.GetData(1, ref functionality);
                DA.GetData(2, ref constType);
                DA.GetData(3, ref structural);
            }

            var outputList = new List<string>();
            outputList.Add(functionality);
            outputList.Add(constType);
            outputList.Add(structural);

            var sqlOutput = "SELECT * FROM dbo.BtkComponent bc WHERE ";

            if(isConnected)
            {
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
                        sqlOutput += "AND bc.StructMaterialConcrete = 1";
                        break;
                    case "Wood":
                        sqlOutput += "AND bc.StructMaterialWood = 1";
                        break;
                    case "Brick":
                        sqlOutput += "AND bc.StructMaterialBrick = 1";
                        break;
                    case "Steel":
                        sqlOutput += "AND bc.StructMaterialSteel = 1 ";
                        break;
                }
            }         

            DA.SetData(0, sqlOutput);
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