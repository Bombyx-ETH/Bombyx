using System;
using Grasshopper.Kernel;
using Bombyx.Data.CompLevel;

namespace Bombyx.Plugin.CompLevel
{
    public class CompGroups : GH_Component
    {
        public CompGroups()
          : base("Component Groups",
                 "Comp. Grps",
                 "Returns Component groups from database",
                 "Bombyx",
                 "Component Level")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Groups", "Component\ngroups", "Component groups", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            DA.SetDataList(0, CompData.GetComponentGroupsList());
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Icons._1component;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("bd3c10a3-2718-43e2-b96b-6c289bfaf6dd"); }
        }
    }
}