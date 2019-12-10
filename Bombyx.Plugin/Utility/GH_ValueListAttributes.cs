using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Special;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Bombyx.Plugin.Utility
{
    public class GH_ValueListAttributes : GH_Attributes<ItemSelector>
    {
        public const int ItemHeight = 22;
        private const int ArrowRadius = 6;

        public override bool AllowMessageBalloon
        {
            get
            {
                return false;
            }
        }

        public override bool HasInputGrip
        {
            get
            {
                return true;
            }
        }

        public override bool HasOutputGrip
        {
            get
            {
                return true;
            }
        }

        private RectangleF ItemBounds { get; set; }
        private RectangleF NameBounds { get; set; }

        public GH_ValueListAttributes(ItemSelector owner)
          : base(owner)
        {
        }

        public override GH_ObjectResponse RespondToMouseDown(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            if (e.Button == MouseButtons.Left)
            {
                switch (Owner.ListMode)
                {
                    case GH_ValueListMode.CheckList:
                        int num = Owner.ListItems.Count - 1;
                        for (int index = 0; index <= num; ++index)
                        {
                            if (Owner.ListItems[index].BoxLeft.Contains(e.CanvasLocation))
                            {
                                Owner.ToggleItem(index);
                                return GH_ObjectResponse.Handled;
                            }
                        }
                        break;
                    case GH_ValueListMode.DropDown:
                        GH_ValueListItem firstSelectedItem1 = Owner.FirstSelectedItem;
                        if (firstSelectedItem1 != null && firstSelectedItem1.BoxRight.Contains(e.CanvasLocation))
                        {
                            ToolStripDropDownMenu stripDropDownMenu = new ToolStripDropDownMenu();
                            GH_ValueListItem firstSelectedItem2 = Owner.FirstSelectedItem;
                            foreach (GH_ValueListItem listItem in Owner.ListItems)
                            {
                                ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(listItem.Name);
                                toolStripMenuItem.Click += new EventHandler(ValueMenuItem_Click);
                                if (listItem == firstSelectedItem2)
                                    toolStripMenuItem.Checked = true;
                                toolStripMenuItem.Tag = (object)listItem;
                                stripDropDownMenu.Items.Add((ToolStripItem)toolStripMenuItem);
                            }
                            stripDropDownMenu.Show((Control)sender, e.ControlLocation);
                            return GH_ObjectResponse.Handled;
                        }
                        break;
                    case GH_ValueListMode.Sequence:
                    case GH_ValueListMode.Cycle:
                        using (List<GH_ValueListItem>.Enumerator enumerator = Owner.ListItems.GetEnumerator())
                        {
                            while (enumerator.MoveNext())
                            {
                                GH_ValueListItem current = enumerator.Current;
                                if (current.IsVisible)
                                {
                                    if (current.BoxLeft.Contains(e.CanvasLocation))
                                    {
                                        Owner.PrevItem();
                                        return GH_ObjectResponse.Handled;
                                    }
                                    if (current.BoxRight.Contains(e.CanvasLocation))
                                    {
                                        Owner.NextItem();
                                        return GH_ObjectResponse.Handled;
                                    }
                                }
                            }
                            break;
                        }
                }
            }
            return base.RespondToMouseDown(sender, e);
        }

        private void ValueMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem toolStripMenuItem = (ToolStripMenuItem)sender;
            if (toolStripMenuItem.Checked)
                return;
            GH_ValueListItem tag = toolStripMenuItem.Tag as GH_ValueListItem;
            if (tag == null)
                return;
            Owner.SelectItem(Owner.ListItems.IndexOf(tag));
        }

        public override GH_ObjectResponse RespondToMouseDoubleClick(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            if (e.Button == MouseButtons.Left)
            {
                foreach (GH_ValueListItem listItem in Owner.ListItems)
                {
                    if (listItem.IsVisible && listItem.BoxName.Contains(e.CanvasLocation))
                        return GH_ObjectResponse.Handled;
                }
            }
            return base.RespondToMouseDoubleClick(sender, e);
        }

        protected override void Layout()
        {
            switch (Owner.ListMode)
            {
                case GH_ValueListMode.CheckList:
                    LayoutCheckList();
                    break;
                case GH_ValueListMode.DropDown:
                    LayoutDropDown();
                    break;
                default:
                    LayoutSequence();
                    break;
            }

            ItemBounds = Bounds;
            RectangleF bounds1 = Bounds;
            //RectangleF rectangleF;
            RectangleF local1;
            double x = (double)bounds1.X;
            RectangleF bounds2 = Bounds;
            double y1 = (double)bounds2.Y;
            bounds2 = Bounds;
            double height1 = (double)bounds2.Height;
            local1 = new RectangleF((float)x, (float)y1, 0.0f, (float)height1);
            NameBounds = local1;
            if (Owner.DisplayName == null)
                return;
            int num1 = GH_FontServer.StringWidth(Owner.DisplayName, GH_FontServer.Standard) + 10;
            RectangleF bounds3 = Bounds;
            RectangleF local2;
            double num2 = (double)bounds3.X - (double)num1;
            bounds2 = Bounds;
            double y2 = (double)bounds2.Y;
            double num3 = (double)num1;
            bounds2 = Bounds;
            double height2 = (double)bounds2.Height;
            local2 = new RectangleF((float)num2, (float)y2, (float)num3, (float)height2);
            NameBounds = bounds1;
            Bounds = RectangleF.Union(NameBounds, ItemBounds);
        }

        private void LayoutCheckList()
        {
            int num1 = ItemMaximumWidth() + 22;
            int num2 = 22 * Math.Max(1, Owner.ListItems.Count);
            Pivot = (PointF)GH_Convert.ToPoint(Pivot);
            //RectangleF rectangleF;
            RectangleF local;
            PointF pivot = Pivot;
            double x = (double)pivot.X;
            pivot = Pivot;
            double y = (double)pivot.Y;
            double num3 = (double)num1;
            double num4 = (double)num2;
            local = new RectangleF((float)x, (float)y, (float)num3, (float)num4);
            Bounds = local;
            int num5 = 0;
            int num6 = Owner.ListItems.Count - 1;
            for (int index = num5; index <= num6; ++index)
            {
                RectangleF bounds = new RectangleF(Bounds.X, Bounds.Y + (float)(index * 22), (float)num1, 22f);
                Owner.ListItems[index].SetCheckListBounds(bounds);
            }
        }

        private void LayoutDropDown()
        {
            int num1 = ItemMaximumWidth() + 22;
            int num2 = 22;
            Pivot = (PointF)GH_Convert.ToPoint(Pivot);
            //RectangleF rectangleF;
            RectangleF local;
            PointF pivot = Pivot;
            double x = (double)pivot.X;
            pivot = Pivot;
            double y = (double)pivot.Y;
            double num3 = (double)num1;
            double num4 = (double)num2;
            local = new RectangleF((float)x, (float)y, (float)num3, (float)num4);
            Bounds = local;
            GH_ValueListItem firstSelectedItem = Owner.FirstSelectedItem;
            foreach (GH_ValueListItem listItem in Owner.ListItems)
            {
                if (listItem == firstSelectedItem)
                    listItem.SetDropdownBounds(Bounds);
                else
                    listItem.SetEmptyBounds(Bounds);
            }
        }

        private void LayoutSequence()
        {
            int num1 = ItemMaximumWidth() + 44;
            int num2 = 22;
            Pivot = (PointF)GH_Convert.ToPoint(Pivot);
            //RectangleF rectangleF;
            RectangleF local;
            PointF pivot = Pivot;
            double x = (double)pivot.X;
            pivot = Pivot;
            double y = (double)pivot.Y;
            double num3 = (double)num1;
            double num4 = (double)num2;
            local = new RectangleF((float)x, (float)y, (float)num3, (float)num4);
            Bounds = local;
            GH_ValueListItem firstSelectedItem = Owner.FirstSelectedItem;
            foreach (GH_ValueListItem listItem in Owner.ListItems)
            {
                if (listItem == firstSelectedItem)
                    listItem.SetSequenceBounds(Bounds);
                else
                    listItem.SetEmptyBounds(Bounds);
            }
        }

        private int ItemMaximumWidth()
        {
            int val1 = 20;
            foreach (GH_ValueListItem listItem in Owner.ListItems)
            {
                int val2 = GH_FontServer.StringWidth(listItem.Name, GH_FontServer.Standard);
                val1 = Math.Max(val1, val2);
            }
            return val1 + 10;
        }

        protected override void Render(GH_Canvas canvas, Graphics graphics, GH_CanvasChannel channel)
        {
            if (channel == GH_CanvasChannel.Wires)
                this.RenderIncomingWires(canvas.Painter, (IEnumerable<IGH_Param>)Owner.Sources, Owner.WireDisplay);
            if (channel != GH_CanvasChannel.Objects)
                return;
            GH_Capsule capsule = GH_Capsule.CreateCapsule(Bounds, GH_Palette.White);
            GH_Capsule ghCapsule1 = capsule;
            PointF pointF = OutputGrip;
            double y1 = (double)pointF.Y;
            ghCapsule1.AddOutputGrip((float)y1);
            GH_Capsule ghCapsule2 = capsule;
            pointF = InputGrip;
            double y2 = (double)pointF.Y;
            ghCapsule2.AddInputGrip((float)y2);
            capsule.Render(canvas.Graphics, Selected, Owner.Locked, Owner.Hidden);
            capsule.Dispose();
            int zoomFadeLow = GH_Canvas.ZoomFadeLow;
            if (zoomFadeLow > 0)
            {
                canvas.SetSmartTextRenderingHint();
                GH_PaletteStyle impliedStyle = GH_CapsuleRenderEngine.GetImpliedStyle(GH_Palette.White, (IGH_Attributes)this);
                Color color = Color.FromArgb(zoomFadeLow, impliedStyle.Text);
                if (NameBounds.Width > 0.0)
                {
                    SolidBrush solidBrush = new SolidBrush(color);
                    graphics.DrawString(Owner.NickName, GH_FontServer.Standard, (Brush)solidBrush, NameBounds, GH_TextRenderingConstants.CenterCenter);
                    solidBrush.Dispose();
                    RectangleF nameBounds = NameBounds;
                    int int32_1 = Convert.ToInt32(nameBounds.Right);
                    nameBounds = NameBounds;
                    int int32_2 = Convert.ToInt32(nameBounds.Top);
                    nameBounds = NameBounds;
                    int int32_3 = Convert.ToInt32(nameBounds.Bottom);
                    GH_GraphicsUtil.EtchFadingVertical(graphics, int32_2, int32_3, int32_1, Convert.ToInt32(0.8 * (double)zoomFadeLow), Convert.ToInt32(0.3 * (double)zoomFadeLow));
                }
                switch (Owner.ListMode)
                {
                    case GH_ValueListMode.CheckList:
                        RenderCheckList(canvas, graphics, color);
                        break;
                    case GH_ValueListMode.DropDown:
                        RenderDropDown(canvas, graphics, color);
                        break;
                    case GH_ValueListMode.Sequence:
                    case GH_ValueListMode.Cycle:
                        RenderSequence(canvas, graphics, color);
                        break;
                }
            }
        }

        private void RenderCheckList(GH_Canvas canvas, Graphics graphics, Color color)
        {
            if (Owner.ListItems.Count == 0)
                return;

            RectangleF bounds = Bounds;
            Convert.ToInt32(bounds.Y);
            bounds = Bounds;
            Convert.ToInt32(bounds.Bottom);
            Convert.ToInt32(Owner.ListItems[0].BoxLeft.Right);

            foreach (GH_ValueListItem listItem in Owner.ListItems)
            {
                graphics.DrawString(listItem.Name, GH_FontServer.Standard, Brushes.Black, listItem.BoxName, GH_TextRenderingConstants.CenterCenter);
                if (listItem.Selected)
                {
                    RenderCheckMark(canvas, graphics, listItem.BoxLeft, color);
                }                    
            }
        }

        private void RenderDropDown(GH_Canvas canvas, Graphics graphics, Color color)
        {
            GH_ValueListItem firstSelectedItem = Owner.FirstSelectedItem;
            if (firstSelectedItem == null)
                return;
            graphics.DrawString(firstSelectedItem.Name, GH_FontServer.Standard, Brushes.Black, firstSelectedItem.BoxName, GH_TextRenderingConstants.CenterCenter);
            RenderDownArrow(canvas, graphics, firstSelectedItem.BoxRight, color);
        }

        private void RenderSequence(GH_Canvas canvas, Graphics graphics, Color color)
        {
            GH_ValueListItem firstSelectedItem = Owner.FirstSelectedItem;
            if (firstSelectedItem == null)
                return;
            graphics.DrawString(firstSelectedItem.Name, GH_FontServer.Standard, Brushes.Black, firstSelectedItem.BoxName, GH_TextRenderingConstants.CenterCenter);
            RenderLeftArrow(canvas, graphics, firstSelectedItem.BoxLeft, color);
            RenderRightArrow(canvas, graphics, firstSelectedItem.BoxRight, color);
        }

        private static void RenderDownArrow(GH_Canvas canvas, Graphics graphics, RectangleF bounds, Color color)
        {
            int int32_1 = Convert.ToInt32(bounds.X + 0.5f * bounds.Width);
            int int32_2 = Convert.ToInt32(bounds.Y + 0.5f * bounds.Height);
            PointF[] points = new PointF[3]
            {
                new PointF((float) int32_1, (float) (int32_2 + 6)),
                new PointF((float) (int32_1 + 6), (float) (int32_2 - 6)),
                new PointF((float) (int32_1 - 6), (float) (int32_2 - 6))
            };
            RenderShape(canvas, graphics, points, color);
        }

        private static void RenderLeftArrow(GH_Canvas canvas, Graphics graphics, RectangleF bounds, Color color)
        {
            int int32_1 = Convert.ToInt32(bounds.X + 0.5f * bounds.Width);
            int int32_2 = Convert.ToInt32(bounds.Y + 0.5f * bounds.Height);
            PointF[] points = new PointF[3]
            {
                new PointF((float) (int32_1 - 6), (float) int32_2),
                new PointF((float) (int32_1 + 6), (float) (int32_2 + 6)),
                new PointF((float) (int32_1 + 6), (float) (int32_2 - 6))
            };
            RenderShape(canvas, graphics, points, color);
        }

        private static void RenderRightArrow(GH_Canvas canvas, Graphics graphics, RectangleF bounds, Color color)
        {
            int int32_1 = Convert.ToInt32(bounds.X + 0.5f * bounds.Width);
            int int32_2 = Convert.ToInt32(bounds.Y + 0.5f * bounds.Height);
            PointF[] points = new PointF[3]
            {
                new PointF((float) (int32_1 + 6), (float) int32_2),
                new PointF((float) (int32_1 - 6), (float) (int32_2 - 6)),
                new PointF((float) (int32_1 - 6), (float) (int32_2 + 6))
            };
            RenderShape(canvas, graphics, points, color);
        }

        private static void RenderCheckMark(GH_Canvas canvas, Graphics graphics, RectangleF bounds, Color color)
        {
            int num = Convert.ToInt32(bounds.X + 0.5f * bounds.Width) - 2;
            int int32 = Convert.ToInt32(bounds.Y + 0.5f * bounds.Height);
            PointF[] points = new PointF[6]
            {
                new PointF((float) num, (float) int32),
                new PointF((float) num - 3.5f, (float) int32 - 3.5f),
                new PointF((float) num - 6.5f, (float) int32 - 0.5f),
                new PointF((float) num, (float) int32 + 6f),
                new PointF((float) num + 9.5f, (float) int32 - 3.5f),
                new PointF((float) num + 6.5f, (float) int32 - 6.5f)
            };
            RenderShape(canvas, graphics, points, color);
        }

        private static void RenderShape(GH_Canvas canvas, Graphics graphics, PointF[] points, Color color)
        {
            int zoomFadeMedium = GH_Canvas.ZoomFadeMedium;
            float num1 = points[0].X;
            float num2 = num1;
            float num3 = points[0].Y;
            float num4 = num3;
            int num5 = 1;
            int num6 = points.Length - 1;
            for (int index = num5; index <= num6; ++index)
            {
                num1 = Math.Min(num1, points[index].X);
                num2 = Math.Max(num2, points[index].X);
                num3 = Math.Min(num3, points[index].Y);
                num4 = Math.Max(num4, points[index].Y);
            }
            RectangleF rect = RectangleF.FromLTRB(num1, num3, num2, num4);
            rect.Inflate(1f, 1f);
            LinearGradientBrush linearGradientBrush1 = new LinearGradientBrush(rect, color, GH_GraphicsUtil.OffsetColour(color, 50), LinearGradientMode.Vertical);
            linearGradientBrush1.WrapMode = WrapMode.TileFlipXY;
            graphics.FillPolygon((Brush)linearGradientBrush1, points);
            linearGradientBrush1.Dispose();
            if (zoomFadeMedium > 0)
            {
                Color color1 = Color.FromArgb(Convert.ToInt32(0.5 * (double)zoomFadeMedium), Color.White);
                Color color2 = Color.FromArgb(0, Color.White);
                LinearGradientBrush linearGradientBrush2 = new LinearGradientBrush(rect, color1, color2, LinearGradientMode.Vertical);
                linearGradientBrush2.WrapMode = WrapMode.TileFlipXY;
                Pen pen = new Pen((Brush)linearGradientBrush2, 3f);
                pen.LineJoin = LineJoin.Round;
                pen.CompoundArray = new float[2] { 0.0f, 0.5f };
                graphics.DrawPolygon(pen, points);
                linearGradientBrush2.Dispose();
                pen.Dispose();
            }
            graphics.DrawPolygon(new Pen(color, 1f)
            {
                LineJoin = LineJoin.Round
            }, points);
        }
    }
}