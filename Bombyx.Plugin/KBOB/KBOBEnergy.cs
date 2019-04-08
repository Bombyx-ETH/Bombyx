using System;
using Grasshopper.Kernel;
using Bombyx.Data.KBOB;
using System.Linq;
using System.Collections.Generic;

namespace Bombyx.Plugin.KBOB
{
    public class KBOBEnergy : GH_Component
    {
        public KBOBEnergy()
          : base("KBOB Energy",
                 "KBOB Building energy",
                 "Returns KBOB energy from database",
                 "Bombyx",
                 "Energy")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Energy demand", "Energy\ndemand", "Energy demand", GH_ParamAccess.list);
            pManager.AddTextParameter("Selected energy carrier", "Energy\ncarrier", "Selected energy carrier", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Global warming potential", "GWP (kg CO\x2082-eq/m\xB2 a)", "Global warming potential (kg CO\x2082-eq/m\xB2 a)", GH_ParamAccess.item);         
            pManager.AddNumberParameter("Pe Total", "PE Total (kWh oil-eq/m\xB2 a)", "PE Total (kWh oil-eq/m\xB2 a)", GH_ParamAccess.item);
            pManager.AddNumberParameter("PE Renewable", "PE Renewable (kWh oil-eq/m\xB2 a)", "PE Renewable (kWh oil-eq/m\xB2 a)", GH_ParamAccess.item);
            pManager.AddNumberParameter("PE Non Renewable", "PE Non Renewable (kWh oil-eq/m\xB2 a)", "PE Non Renewable (kWh oil-eq/m\xB2 a)", GH_ParamAccess.item);
            pManager.AddNumberParameter("PE Renewable at Location", "PE Renewable at Location (kWh oil-eq/m\xB2 a)", "PE Renewable at Location (kWh oil-eq/m\xB2 a)", GH_ParamAccess.item);
            pManager.AddNumberParameter("UBP", "UBP (P/m\xB2 a)", "UBP (P/m\xB2 a)", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var demand = new List<double>();
            if (!DA.GetDataList(0, demand)) { return; }
            string input = null;
            if (!DA.GetData(1, ref input)) { return; }

            var newString = input.Split('|');
            var result = KBOBdata.GetEnergy(newString[1].Trim());

            var demandSum = demand.Sum(x => Convert.ToDecimal(x));

            DA.SetData(0, result[5] * demandSum);
            DA.SetData(1, result[1] * demandSum);
            DA.SetData(2, result[2] * demandSum);
            DA.SetData(3, result[3] * demandSum);
            DA.SetData(4, result[4] * demandSum);
            DA.SetData(5, result[0] * demandSum);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Icons._2energy;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("c6185e2e-9f2c-43a6-affb-d8cbe3c60c81"); }
        }
    }
}