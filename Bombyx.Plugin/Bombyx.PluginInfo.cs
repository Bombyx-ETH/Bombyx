using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace Bombyx.Plugin
{
    public class PlugInInfo : GH_AssemblyInfo
  {
    public override string Name
    {
        get
        {
            return "Bombyx";
        }
    }
    public override Bitmap Icon
    {
        get
        {
            return Icons.bombyxLogo;
        }
    }
    public override string Description
    {
        get
        {
            return "Real-time Life Cycle Assessment – Bombyx project";
        }
    }
    public override Guid Id
    {
        get
        {
            return new Guid("80d45113-c1f5-4270-8b33-a1e647c6a509");
        }
    }

    public override string AuthorName
    {
        get
        {
            return "ETH Zurich";
        }
    }
    public override string AuthorContact
    {
        get
        {
            return "bombyx@ibi.baug.ethz.ch";
        }
    }
}
}
