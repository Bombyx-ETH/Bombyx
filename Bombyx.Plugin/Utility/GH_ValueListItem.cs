using Grasshopper.Kernel.Expressions;
using Grasshopper.Kernel.Types;
using System;
using System.ComponentModel;
using System.Drawing;

namespace Bombyx.Plugin.Utility
{
    public class GH_ValueListItem
    {
        private const int BoxWidth = 22;
        private IGH_Goo m_value;

        public bool Selected { get; set; }
        public string Name { get; set; }
        public string Expression { get; set; }
        public RectangleF BoxName { get; set; }
        public RectangleF BoxLeft { get; set; }
        public RectangleF BoxRight { get; set; }

        [Browsable(false)]
        public IGH_Goo Value
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Expression))
                    return (IGH_Goo)null;
                if (m_value == null)
                {
                    try
                    {
                        GH_Variant ghVariant = new GH_ExpressionParser().Evaluate(GH_ExpressionSyntaxWriter.RewriteAll(this.Expression));
                        if (ghVariant != null)
                            m_value = ghVariant.ToGoo();
                    }
                    catch (Exception)
                    {
                        //ProjectData.SetProjectError(ex);
                        //ProjectData.ClearProjectError();
                    }
                }
                return m_value;
            }
        }

        public bool IsVisible
        {
            get
            {
                return BoxName.Height > 0.0;
            }
        }

        public GH_ValueListItem()
        {
            Name = string.Empty;
            Expression = string.Empty;
        }

        public GH_ValueListItem(string name, string expression)
        {
            Name = name;
            Expression = expression;
        }

        public GH_ValueListItem(string name, string value, IGH_Goo gooIn)
        {
            Name = name;
            Expression = value;
            m_value = gooIn;
        }

        public void ExpireValue()
        {
            m_value = (IGH_Goo)null;
        }

        internal void SetCheckListBounds(RectangleF bounds)
        {
            RectangleF rectangleF = new RectangleF(bounds.X, bounds.Y, 22f, bounds.Height);
            BoxLeft = rectangleF;
            rectangleF = new RectangleF(bounds.X + 22f, bounds.Y, bounds.Width - 22f, bounds.Height);
            BoxName = rectangleF;
            rectangleF = new RectangleF(bounds.Right, bounds.Y, 0.0f, bounds.Height);
            BoxRight = rectangleF;
        }

        internal void SetDropdownBounds(RectangleF bounds)
        {
            RectangleF rectangleF = new RectangleF(bounds.X, bounds.Y, 0.0f, bounds.Height);
            BoxLeft = rectangleF;
            rectangleF = new RectangleF(bounds.X, bounds.Y, bounds.Width - 22f, bounds.Height);
            BoxName = rectangleF;
            rectangleF = new RectangleF(bounds.Right - 22f, bounds.Y, 22f, bounds.Height);
            BoxRight = rectangleF;
        }

        internal void SetSequenceBounds(RectangleF bounds)
        {
            RectangleF rectangleF = new RectangleF(bounds.X, bounds.Y, 22f, bounds.Height);
            BoxLeft = rectangleF;
            rectangleF = new RectangleF(bounds.X + 22f, bounds.Y, bounds.Width - 44f, bounds.Height);
            BoxName = rectangleF;
            rectangleF = new RectangleF(bounds.Right - 22f, bounds.Y, 22f, bounds.Height);
            BoxRight = rectangleF;
        }

        internal void SetEmptyBounds(RectangleF bounds)
        {
            RectangleF rectangleF = new RectangleF(bounds.X, bounds.Y, 0.0f, 0.0f);
            BoxLeft = rectangleF;
            rectangleF = new RectangleF(bounds.X, bounds.Y, 0.0f, 0.0f);
            BoxName = rectangleF;
            rectangleF = new RectangleF(bounds.X, bounds.Y, 0.0f, 0.0f);
            BoxRight = rectangleF;
        }
    }
}