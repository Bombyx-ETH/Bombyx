using System;
using Grasshopper.Kernel;
using Bombyx.Data.KBOB;

namespace Bombyx.Plugin.KBOB
{
    public class KBOBEnergyList : GH_Component
    {
        public KBOBEnergyList()
          : base("KBOB Energy list",
                 "KBOB Energy",
                 "Returns KBOB energy list from database",
                 "Bombyx",
                 "Energy")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Energy list", "Energy\nList", "Energy list", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            DA.SetDataList(0, KBOBdata.GetEnergyList());
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Icons._1energy;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("5c4f0470-63a9-480a-85fa-0e35faf53dac"); }
        }
    }
}