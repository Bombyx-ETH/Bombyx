using System;
using Grasshopper.Kernel;

namespace Bombyx.Plugin.InputLevel
{
    public class ElementOutput : GH_Component
    {
        public ElementOutput()
          : base("Element Output",
                 "Element output",
                 "Description",
                 "Bombyx",
                 "Input level")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Component input", "Component input", "Component input", GH_ParamAccess.list);
            pManager.AddTextParameter("Area (m2)", "Area (m2)", "Area (m2)", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Element output", "Element output", "Element output", GH_ParamAccess.item);
            pManager.AddTextParameter("Element avg output", "Element avg output", "Element avg output", GH_ParamAccess.item);
            pManager.AddTextParameter("Element min output", "Element min output", "Element min output", GH_ParamAccess.item);
            pManager.AddTextParameter("Element max output", "Element max output", "Element max output", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Icons._4;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("b4d4dd5f-8f61-4de4-8ae0-b41e33d85f90"); }
        }
    }
}