using System;
using Grasshopper.Kernel;
using System.Linq;
using Bombyx.Data.CompLevel;

namespace Bombyx.Plugin.CompLevel
{
    public class Components : GH_Component
    {
        public Components()
          : base("Components",
                 "Components",
                 "Returns Component from list",
                 "Bombyx",
                 "Component Level")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Group", "Component\ngroup", "Component group", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Components", "Components", "Components", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string input = null;
            if (!DA.GetData(0, ref input)) { return; }

            var splitInput = input.Split('-').ToList();

            DA.SetDataList(0, CompData.GetComponents(splitInput[0].Trim()));
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Icons._2component;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("0136d8d4-5084-4212-9254-bfd77aad11d9"); }
        }
    }
}