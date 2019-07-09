using System;
using Grasshopper.Kernel;

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
            pManager.AddTextParameter("Element input", "Element input", "Element input", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Building output", "Building output", "Building output", GH_ParamAccess.item);
            pManager.AddTextParameter("Building avg output", "Building avg output", "Building avg output", GH_ParamAccess.item);
            pManager.AddTextParameter("Building min output", "Building min output", "Building min output", GH_ParamAccess.item);
            pManager.AddTextParameter("Building max output", "Building max output", "Building max output", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
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