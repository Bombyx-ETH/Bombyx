using Grasshopper.Kernel;

namespace Bombyx.Plugin
{
    public class BombyxIcon : GH_AssemblyPriority
    {
        public override GH_LoadingInstruction PriorityLoad()
        {
            Grasshopper.Instances.ComponentServer.AddCategoryIcon("Bombyx", Icons.bombyxLogo);
            Grasshopper.Instances.ComponentServer.AddCategorySymbolName("Bombyx", 'B');
            return GH_LoadingInstruction.Proceed;
        }
    }
}
