using System;
using Grasshopper.Kernel;
using System.Collections.Generic;

namespace Bombyx.Plugin.KBOB
{
    public class KBOBServicesGroups : GH_Component
    {
        public KBOBServicesGroups()
          : base("KBOB Building services groups",
                 "KBOB Serv. grps",
                 "Returns KBOB list of building services from database",
                 "Bombyx",
                 "Services")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Service groups", "Service\ngroups", "List of service groups", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var groups = new List<string>();

            groups.Add("Heating systems");
            groups.Add("Ventilation systems");
            groups.Add("Sanitary");
            groups.Add("Electrical systems");

            DA.SetDataList(0, groups);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Icons._1services;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("299981a2-62e3-4067-b770-4b031d947e0f"); }
        }
    }
}