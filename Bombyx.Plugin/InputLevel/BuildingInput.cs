using System;
using Grasshopper.Kernel;
using System.Collections.Generic;
using Grasshopper.Kernel.Special;
using System.Drawing;

namespace Bombyx.Plugin.InputLevel
{
    public class BuildingInput : GH_Component
    {
        GH_Document GrasshopperDocument;
        IGH_Component Component;

        private string[] SIZE_VALUES = new string[] { "Small", "Mid-size", "Highrise" };
        private string[] USAGE_VALUES = new string[] { "Residential single family", "Residential multi family", "Office" };
        private string[] ENERGY_VALUES = new string[] { "Standard", "Above average", "Passive house" };
        private string[] STRUCTUAL_VALUES = new string[] { "Concrete", "Wood", "Brick", "Steel" };

        public BuildingInput()
          : base("Building Input",
                 "Building input",
                 "Select inputs for building level",
                 "Bombyx",
                 "Input level")
        {
            Message = "Connect a Button to the first \ninput parameter(Activate) and \nclick it to show inputs."; 
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("Activate (Button)", "Activate (Button)", "Activate (Button)", GH_ParamAccess.item);
            pManager[0].Optional = true;
            pManager.AddTextParameter("Building size", "Building size", "Building size", GH_ParamAccess.item);
            pManager[1].Optional = true;
            pManager.AddTextParameter("Building usage", "Building usage", "Building usage", GH_ParamAccess.item);
            pManager[2].Optional = true;
            pManager.AddTextParameter("Energy performance", "Energy performance", "Energy performance", GH_ParamAccess.item);
            pManager[3].Optional = true;
            pManager.AddTextParameter("Structural material", "Structural material", "Structural material", GH_ParamAccess.item);
            pManager[4].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Output", "Building\noutput", "Output", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Component = this;
            GrasshopperDocument = OnPingDocument();

            string size = "";
            string usage = "";
            string energy = "";
            string structural = "";        

            bool input = false;
            if (!DA.GetData(0, ref input)) { return; }

            if (input &&
                Params.Input[1].SourceCount == 0 &&
                Params.Input[2].SourceCount == 0 &&
                Params.Input[3].SourceCount == 0 &&
                Params.Input[4].SourceCount == 0)
            {
                CreateAccentList(SIZE_VALUES, "Building size", 1, 200, -20);
                CreateAccentList(USAGE_VALUES, "Building usage", 2, 275, -10);
                CreateAccentList(ENERGY_VALUES, "Energy preference", 3, 230, 0);
                CreateAccentList(STRUCTUAL_VALUES, "Structual material", 4, 202, 10);
                Message = "Component activated.";
            }

            if (!DA.GetData(1, ref size)) { return; }
            if (!DA.GetData(2, ref usage)) { return; }
            if (!DA.GetData(3, ref energy)) { return; }
            if (!DA.GetData(4, ref structural)) { return; }

            var outputList = new List<string>();
            outputList.Add(size);
            outputList.Add(usage);
            outputList.Add(energy);
            outputList.Add(structural);

            DA.SetDataList(0, outputList);
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
                return Icons._1;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("ff758a70-edf7-4814-b104-f28d1551b8db"); }
        }
    }
}