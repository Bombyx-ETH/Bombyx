using System;
using Grasshopper.Kernel;
using System.Drawing;
using System.Collections.Generic;
using Bombyx.Data.InputLevel;

namespace Bombyx.Plugin.InputLevel
{
    public class ComponentInput : GH_Component
    {
        public ComponentInput()
          : base("Component Input",
                 "Component input",
                 "Description",
                 "Bombyx",
                 "Input level")
        {
            Message = "Connect Element output\nto the first input parameter.";
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Element input", "Element input", "Element input", GH_ParamAccess.list);
            pManager[0].Optional = true;
            pManager.AddIntegerParameter("RSP (years)", "RSP (years)", "RSP (years)", GH_ParamAccess.item);
            pManager[1].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Component avg output", "Component avg output", "Component avg output", GH_ParamAccess.list);
            pManager.AddTextParameter("Component min output", "Component min output", "Component min output", GH_ParamAccess.list);
            pManager.AddTextParameter("Component max output", "Component max output", "Component max output", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var input = new List<string>();

            var isConnected = false;
            if (Params.Input[0].SourceCount == 1)
            {
                isConnected = true;
                if (!DA.GetDataList(0, input)) { return; }
            }

            if (isConnected)
            {
                Message = "Component connected.\nProvide RSP in years.";
            }

            var rsp = 0;
            if (!DA.GetData(1, ref rsp)) { return; }

            if (isConnected && Params.Input[1].SourceCount == 1)
            {
                Message = "Component connected.";
                //var sortCode = input.Split('|');
                //var x = "";

            }

            var sqlOutput = "SELECT AVG(kmg.Ubp13Embodied) AS Ubp13Embodied, AVG(kmg.Ubp13EoL) AS Ubp13EoL, " +
                            "AVG(kmg.TotalEmbodied) AS TotalEmbodied, AVG(kmg.TotalEoL) AS TotalEoL, " +
                            "AVG(kmg.RenewableEmbodied)AS RenewableEmbodied, AVG(kmg.RenewableEoL) AS RenewableEoL, " +
                            "AVG(kmg.NonRenewableEmbodied) AS NonRenewableEmbodied, AVG(kmg.NonRenewableEoL) AS NonRenewableEoL, " +
                            "AVG(kmg.GHGEmbodied) AS GHGEmbodied, AVG(kmg.GHGEoL) AS GHGEoL FROM dbo.BtkComponent bc " +
                            "LEFT JOIN dbo.KbobMaterialGen kmg ON bc.SortCode = kmg.SortCode WHERE bc.ComponentCode IN " + input[4] + " AND ";


            switch (input[0])
            {
                case "Small":
                    sqlOutput += "bc.BuildingSizeSmall = 1 ";
                    break;
                case "Mid-size":
                    sqlOutput += "bc.BuildingSizeMid = 1 ";
                    break;
                case "Highrise":
                    sqlOutput += "bc.BuildingSizeHighrise = 1 ";
                    break;
            }

            switch (input[1])
            {
                case "Residential single family":
                    sqlOutput += "AND bc.BuildingUsageSF = 1 ";
                    break;
                case "Residential multi family":
                    sqlOutput += "AND bc.BuildingUsageMF = 1 ";
                    break;
                case "Office":
                    sqlOutput += "AND bc.BuildingUsageOffice = 1 ";
                    break;
            }

            switch (input[2])
            {
                case "Standard":
                    sqlOutput += "AND bc.BuildingEnergyStandard = 1 ";
                    break;
                case "Above average":
                    sqlOutput += "AND bc.BuildingEnergyAboveAvg = 1 ";
                    break;
                case "Passive house":
                    sqlOutput += "AND bc.BuildingEnergyPassivehouse = 1 ";
                    break;
            }

            switch (input[3])
            {
                case "Concrete":
                    sqlOutput += "AND bc.StructMaterialConcrete = 1";
                    break;
                case "Wood":
                    sqlOutput += "AND bc.StructMaterialWood = 1";
                    break;
                case "Brick":
                    sqlOutput += "AND bc.StructMaterialBrick = 1";
                    break;
                case "Steel":
                    sqlOutput += "AND bc.StructMaterialSteel = 1 ";
                    break;
            }

            var output = new Dictionary<string, decimal>();

            foreach (var item in InputData.GetElementsInputList(sqlOutput))
            {
                output.Add("UBP13Embodied", item.UBP13Embodied);
                output.Add("UBP13EoL", item.UBP13EoL);
                output.Add("TotalEmbodied", item.TotalEmbodied);
                output.Add("TotalEoL", item.TotalEoL);
                output.Add("RenewableEmbodied", item.RenewableEmbodied);
                output.Add("RenewableEoL", item.RenewableEoL);
                output.Add("NonRenewableEmbodied", item.NonRenewableEmbodied);
                output.Add("NonRenewableEoL", item.NonRenewableEoL);
                output.Add("GHGEmbodied", item.GHGEmbodied);
                output.Add("GHGEoL", item.GHGEoL);
            }

            DA.SetDataList(0, output);
        }

        protected override Bitmap Icon
        {
            get
            {
                return Icons._3;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("b6a6bcbf-9414-4ef7-a68a-919e26290afb"); }
        }
    }
}