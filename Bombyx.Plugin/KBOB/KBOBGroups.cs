using System;
using Grasshopper.Kernel;
using System.Collections.Generic;

namespace Bombyx.Plugin.KBOB
{
    public class KBOBGroups : GH_Component
    {
        public KBOBGroups()
          : base("KBOB Groups",
                 "KBOB Groups",
                 "Returns material groups",
                 "Bombyx",
                 "KBOB Data")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Material Groups", "Material Groups", "List", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var groups = new List<string>();
            string[] range = {
                "Concrete",
                "Brick",
                "Other massive building materials",
                "Mortar and plaster",
                "Windows, solar shading and facade cladding",
                "Metal building materials",
                "Wood and wooden materials",
                "Adhesives and joint sealants",
                "Geomembranes and protective films",
                "Thermal insulation",
                "Flooring",
                "Doors",
                "Pipes",
                "Paints, coatings",
                "Plastics",
                "Preparatory works",
                "Kitchen fixtures and furniture"
            };
            groups.AddRange(range);

            DA.SetDataList(0, groups);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Icons.KBOBGroups;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("1780a9a7-6b53-47ed-984f-687d0bc0f255"); }
        }
    }
}