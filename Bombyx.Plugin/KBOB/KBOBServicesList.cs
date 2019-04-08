using System;
using Grasshopper.Kernel;
using Bombyx.Data.KBOB;

namespace Bombyx.Plugin.KBOB
{
    public class KBOBServicesList : GH_Component
    {
        public KBOBServicesList()
          : base("KBOB Building services list",
                 "KBOB Services",
                 "Returns KBOB list of building services from database",
                 "Bombyx",
                 "Services")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Service group", "Service\ngroup", "Selected service group", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Services list", "Services\nlist", "List of services from selected group", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string data = null;
            if (!DA.GetData(0, ref data)) { return; }

            var param = "";

            switch (data)
            {
                case "Heating systems":
                    param = "31.";
                    break;
                case "Ventilation systems":
                    param = "32.";
                    break;
                case "Sanitary":
                    param = "33.";
                    break;
                case "Electrical systems":
                    param = "34.";
                    break;
            }

            DA.SetDataList(0, KBOBdata.GetServicesList(param));
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Icons._2services;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("955fbe24-258b-46f1-b9c9-fdac41d74a8f"); }
        }
    }
}