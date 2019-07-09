using System;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Special;
using System.Drawing;
using System.Collections.Generic;

namespace Bombyx.Plugin.InputLevel
{
    public class ComponentInput : GH_Component
    {
        GH_Document GrasshopperDocument;
        IGH_Component Component;

        public ComponentInput()
          : base("Component Input",
                 "Component input",
                 "Description",
                 "Bombyx",
                 "Input level")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Element input", "Element input", "Element input", GH_ParamAccess.list);
            pManager.AddTextParameter("RSP (years)", "RSP (years)", "RSP (years)", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Component output", "Component output", "Component output", GH_ParamAccess.item);
            pManager.AddTextParameter("Component min output", "Component min output", "Component min output", GH_ParamAccess.item);
            pManager.AddTextParameter("Component max output", "Component max output", "Component max output", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Component = this;
            GrasshopperDocument = OnPingDocument();

            var input = new List<string>();
            if (!DA.GetDataList(0, input)) { return; }
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
                return Icons._3;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("b6a6bcbf-9414-4ef7-a68a-919e26290afb"); }
        }
    }
}