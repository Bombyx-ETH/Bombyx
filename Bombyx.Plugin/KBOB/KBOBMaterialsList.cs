using System;
using Grasshopper.Kernel;
using Bombyx.Data.KBOB;

namespace Bombyx.Plugin.KBOB
{
    public class KBOBMaterialsList : GH_Component
    {
        public KBOBMaterialsList()
          : base("KBOB Materials list",
                 "KBOB Materials",
                 "Returns list of KBOB materials from database",
                 "Bombyx",
                 "Materials")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Material group", "Material\ngroup", "Selected material group", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Materials", "Materials", "List of KBOB mterials from selected group", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string data = null;

            if (!DA.GetData(0, ref data)) { return; }
            if (data == null) { return; }
            if (data.Length == 0) { return; }

            var param = "";

            switch (data)
            {
                case "Preparatory works":
                    param = "00.";
                    break;
                case "Concrete":
                    param = "01.";
                    break;
                case "Brick":
                    param = "02.";
                    break;
                case "Other massive building materials":
                    param = "03.";
                    break;
                case "Mortar and plaster":
                    param = "04.";
                    break;
                case "Windows, solar shading and facade cladding":
                    param = "05.";
                    break;
                case "Metal building materials":
                    param = "06.";
                    break;
                case "Wood and wooden materials":
                    param = "07.";
                    break;
                case "Adhesives and joint sealants":
                    param = "08.";
                    break;
                case "Geomembranes and protective films":
                    param = "09.";
                    break;
                case "Thermal insulation":
                    param = "10.";
                    break;
                case "Flooring":
                    param = "11.";
                    break;
                case "Doors":
                    param = "12.";
                    break;
                case "Pipes":
                    param = "13.";
                    break;
                case "Paints, coatings":
                    param = "14.";
                    break;
                case "Plastics":
                    param = "15.";
                    break;
                case "Kitchen fixtures and furniture":
                    param = "21.";
                    break;
                default:
                    param = "01.";
                    break;
            }

            DA.SetDataList(0, KBOBdata.GetMaterialsList(param));
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Icons._2KBOB;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("3895b89f-959c-431d-b72b-107d475f83d8"); }
        }
    }
}