using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino;
using Rhino.DocObjects;
using Rhino.Geometry;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Special;
using System.Windows.Forms;
using GH_IO.Serialization;
using System.Text;

namespace Bombyx.Plugin.Utility
{
    public class ItemSelector : GH_Param<IGH_Goo>, IGH_PreviewObject, IGH_BakeAwareObject, IGH_StateAwareObject
    {
        protected GH_Structure<IGH_Goo> collectedData;
        private GH_ValueListMode m_listMode;
        private readonly List<GH_ValueListItem> m_userItems;
        private bool m_hidden;

        public ItemSelector()
          : base((IGH_InstanceDescription)new GH_InstanceDescription("Item Selector", "", "Description", "Bombyx", "Utilities"))
        {
            collectedData = new GH_Structure<IGH_Goo>();
            m_listMode = GH_ValueListMode.DropDown;
            m_userItems = new List<GH_ValueListItem>();
            m_hidden = false;
        }

        public override void CreateAttributes()
        {
            m_attributes = (IGH_Attributes)new GH_ValueListAttributes(this);
        }

        public void ToggleItem(int index)
        {
            if (index < 0 || index >= this.m_userItems.Count)
                return;
            RecordUndoEvent("Toggle: " + m_userItems[index].Name);
            m_userItems[index].Selected = !m_userItems[index].Selected;
            ExpireSolution(true);
        }

        public List<GH_ValueListItem> ListItems
        {
            get
            {
                return m_userItems;
            }
        }

        public GH_ValueListMode ListMode
        {
            get
            {
                return m_listMode;
            }
            set
            {
                m_listMode = value;
                if (m_attributes == null)
                    return;
                m_attributes.ExpireLayout();
            }
        }

        public string DisplayName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(NickName) || NickName.Equals("List", StringComparison.OrdinalIgnoreCase))
                    return (string)null;
                return NickName;
            }
        }

        public void SelectItem(int index)
        {
            if (index < 0 || index >= m_userItems.Count)
                return;
            bool flag = false;
            int num1 = 0;
            int num2 = m_userItems.Count - 1;
            for (int index1 = num1; index1 <= num2; ++index1)
            {
                if (index1 == index)
                {
                    if (!m_userItems[index1].Selected)
                    {
                        flag = true;
                        break;
                    }
                }
                else if (m_userItems[index1].Selected)
                {
                    flag = true;
                    break;
                }
            }
            if (!flag)
                return;
            RecordUndoEvent("Select: " + m_userItems[index].Name);
            int num3 = 0;
            int num4 = m_userItems.Count - 1;
            for (int index1 = num3; index1 <= num4; ++index1)
                m_userItems[index1].Selected = index1 == index;
            ExpireSolution(true);
        }

        public GH_ValueListItem FirstSelectedItem
        {
            get
            {
                if (m_userItems.Count == 0)
                    return (GH_ValueListItem)null;
                foreach (GH_ValueListItem userItem in m_userItems)
                {
                    if (userItem.Selected)
                        return userItem;
                }
                return m_userItems[0];
            }
        }

        public void NextItem()
        {
            if (ListMode == GH_ValueListMode.CheckList || ListMode == GH_ValueListMode.DropDown || m_userItems.Count < 2)
                return;
            int num1 = 0;
            int num2 = 0;
            int num3 = m_userItems.Count - 1;
            for (int index = num2; index <= num3; ++index)
            {
                if (m_userItems[index].Selected)
                {
                    num1 = index;
                    break;
                }
            }
            int num4 = 0;
            int num5 = m_userItems.Count - 1;
            for (int index = num4; index <= num5; ++index)
                m_userItems[index].Selected = false;
            int index1 = num1 + 1;
            if (index1 == m_userItems.Count)
            {
                switch (ListMode)
                {
                    case GH_ValueListMode.Sequence:
                        index1 = m_userItems.Count - 1;
                        break;
                    case GH_ValueListMode.Cycle:
                        index1 = 0;
                        break;
                }
            }
            m_userItems[index1].Selected = true;
            ExpireSolution(true);
        }

        public void PrevItem()
        {
            if (ListMode == GH_ValueListMode.CheckList || ListMode == GH_ValueListMode.DropDown || m_userItems.Count < 2)
                return;
            int num1 = 0;
            int num2 = 0;
            int num3 = m_userItems.Count - 1;
            for (int index = num2; index <= num3; ++index)
            {
                if (m_userItems[index].Selected)
                {
                    num1 = index;
                    break;
                }
            }
            int num4 = 0;
            int num5 = m_userItems.Count - 1;
            for (int index = num4; index <= num5; ++index)
                m_userItems[index].Selected = false;
            int index1 = num1 - 1;
            if (index1 == -1)
            {
                switch (ListMode)
                {
                    case GH_ValueListMode.Sequence:
                        index1 = 0;
                        break;
                    case GH_ValueListMode.Cycle:
                        index1 = m_userItems.Count - 1;
                        break;
                }
            }
            m_userItems[index1].Selected = true;
            ExpireSolution(true);
        }

        public List<GH_ValueListItem> SelectedItems
        {
            get
            {
                List<GH_ValueListItem> ghValueListItemList = new List<GH_ValueListItem>();
                if (m_userItems.Count == 0)
                    return ghValueListItemList;
                if (ListMode == GH_ValueListMode.CheckList)
                {
                    foreach (GH_ValueListItem userItem in m_userItems)
                    {
                        if (userItem.Selected)
                            ghValueListItemList.Add(userItem);
                    }
                    return ghValueListItemList;
                }
                foreach (GH_ValueListItem userItem in m_userItems)
                {
                    if (userItem.Selected)
                    {
                        ghValueListItemList.Add(userItem);
                        return ghValueListItemList;
                    }
                }
                m_userItems[0].Selected = true;
                ghValueListItemList.Add(m_userItems[0]);
                return ghValueListItemList;
            }
        }

        public override void AppendAdditionalMenuItems(ToolStripDropDown menu)
        {
            Menu_AppendItem((ToolStrip)menu, "Check List", new EventHandler(Menu_CheckListClicked), true, ListMode == GH_ValueListMode.CheckList);
            Menu_AppendItem((ToolStrip)menu, "Dropdown List", new EventHandler(Menu_DropdownClicked), true, ListMode == GH_ValueListMode.DropDown);
            Menu_AppendItem((ToolStrip)menu, "Value Sequence", new EventHandler(Menu_SequenceClicked), true, ListMode == GH_ValueListMode.Sequence);
            Menu_AppendItem((ToolStrip)menu, "Value Cycle", new EventHandler(Menu_CycleClicked), true, ListMode == GH_ValueListMode.Cycle);
        }

        private void Menu_EditorClicked(object sender, EventArgs e)
        {
        }

        private void Menu_CheckListClicked(object sender, EventArgs e)
        {
            if (ListMode == GH_ValueListMode.CheckList)
                return;
            RecordUndoEvent("Check List");
            ListMode = GH_ValueListMode.CheckList;
            ExpireSolution(true);
        }

        private void Menu_DropdownClicked(object sender, EventArgs e)
        {
            if (ListMode == GH_ValueListMode.DropDown)
                return;
            RecordUndoEvent("Dropdown List");
            ListMode = GH_ValueListMode.DropDown;
            ExpireSolution(true);
        }

        private void Menu_SequenceClicked(object sender, EventArgs e)
        {
            if (ListMode == GH_ValueListMode.Sequence)
                return;
            RecordUndoEvent("Value Sequence");
            ListMode = GH_ValueListMode.Sequence;
            ExpireSolution(true);
        }

        private void Menu_CycleClicked(object sender, EventArgs e)
        {
            if (ListMode == GH_ValueListMode.Cycle)
                return;
            RecordUndoEvent("Value Cycle");
            ListMode = GH_ValueListMode.Cycle;
            ExpireSolution(true);
        }

        protected override IGH_Goo InstantiateT()
        {
            return (IGH_Goo)new GH_ObjectWrapper();
        }

        protected override void CollectVolatileData_FromSources()
        {
            base.CollectVolatileData_FromSources();
            collectedData.Clear();
            collectedData = m_data.Duplicate();
            List<GH_ValueListItem> ghValueListItemList = new List<GH_ValueListItem>((IEnumerable<GH_ValueListItem>)m_userItems);
            m_userItems.Clear();
            List<IGH_Goo> ghGooList = new List<IGH_Goo>((IEnumerable<IGH_Goo>)collectedData);
            for (int index = 0; index < ghGooList.Count; ++index)
            {
                IGH_Goo gooIn = ghGooList[index];
                m_userItems.Add(new GH_ValueListItem(gooIn.ToString(), "\"" + gooIn.ToString() + "\"", gooIn));
                if (index < ghValueListItemList.Count)
                    m_userItems[index].Selected = ghValueListItemList[index].Selected;
            }
            CollectVolatileData_Custom();
        }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.septenary;
            }
        }

        protected override void CollectVolatileData_Custom()
        {
            m_data.Clear();
            foreach (GH_ValueListItem selectedItem in SelectedItems)
                m_data.Append(selectedItem.Value, new GH_Path(0));
        }

        public void DrawViewportWires(IGH_PreviewArgs args)
        {
            Preview_DrawWires(args);
        }

        public void DrawViewportMeshes(IGH_PreviewArgs args)
        {
            Preview_DrawMeshes(args);
        }

        public void BakeGeometry(RhinoDoc doc, List<Guid> obj_ids)
        {
            throw new NotImplementedException();
        }

        public void BakeGeometry(RhinoDoc doc, ObjectAttributes att, List<Guid> obj_ids)
        {
            if (att == null)
                att = doc.CreateDefaultAttributes();
            foreach (object obj in m_data)
            {
                Guid guid;
                if (obj != null && obj is IGH_BakeAwareData && ((IGH_BakeAwareData)obj).BakeGeometry((RhinoDoc)doc, (ObjectAttributes)att, out guid))
                    obj_ids.Add(guid);
            }
        }

        public string SaveState()
        {
            StringBuilder stringBuilder = new StringBuilder(m_userItems.Count);
            foreach (GH_ValueListItem userItem in m_userItems)
            {
                if (userItem.Selected)
                    stringBuilder.Append('Y');
                else
                    stringBuilder.Append('N');
            }
            return stringBuilder.ToString();
        }

        public void LoadState(string state)
        {
            foreach (GH_ValueListItem userItem in m_userItems)
                userItem.Selected = false;
            int result;
            if (int.TryParse(state, out result))
            {
                if (result < 0 || result >= m_userItems.Count)
                    return;
                m_userItems[result].Selected = true;
            }
            else
            {
                int num1 = 0;
                int num2 = Math.Min(state.Length, m_userItems.Count) - 1;
                for (int index = num1; index <= num2; ++index)
                    m_userItems[index].Selected = state[index].Equals('Y');
            }
        }

        public override bool Write(GH_IWriter writer)
        {
            writer.SetInt32("ListMode", (int)ListMode);
            writer.SetInt32("ListCount", m_userItems.Count);
            int num1 = 0;
            int num2 = m_userItems.Count - 1;
            for (int chunk_index = num1; chunk_index <= num2; ++chunk_index)
            {
                GH_IWriter chunk = writer.CreateChunk("ListItem", chunk_index);
                chunk.SetString("Name", m_userItems[chunk_index].Name);
                chunk.SetString("Expression", m_userItems[chunk_index].Expression);
                chunk.SetBoolean("Selected", m_userItems[chunk_index].Selected);
            }
            return base.Write(writer);
        }

        public override bool Read(GH_IReader reader)
        {
            int num1 = 1;
            reader.TryGetInt32("UIMode", ref num1);
            reader.TryGetInt32("ListMode", ref num1);
            ListMode = (GH_ValueListMode)num1;
            int int32 = reader.GetInt32("ListCount");
            int num2 = 0;
            reader.TryGetInt32("CacheCount", ref num2);
            m_userItems.Clear();
            int num3 = 0;
            int num4 = int32 - 1;
            for (int index = num3; index <= num4; ++index)
            {
                GH_IReader chunk = reader.FindChunk("ListItem", index);
                if (chunk == null)
                {
                    reader.AddMessage("Missing chunk for List Value: " + index.ToString(), GH_Message_Type.error);
                }
                else
                {
                    string name = chunk.GetString("Name");
                    string expression = chunk.GetString("Expression");
                    bool flag = false;
                    chunk.TryGetBoolean("Selected", ref flag);
                    m_userItems.Add(new GH_ValueListItem(name, expression)
                    {
                        Selected = flag
                    });
                }
            }
            return base.Read(reader);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Icons.ItemSelector;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("9469e30f-6308-47df-8bf8-57590701ab8b"); }
        }

        public bool Hidden
        {
            get
            {
                return m_hidden;
            }
            set
            {
                m_hidden = value;
            }
        }

        public bool IsPreviewCapable
        {
            get
            {
                return true;
            }
        }

        public BoundingBox ClippingBox
        {
            get
            {
                return (BoundingBox)Preview_ComputeClippingBox();
            }
        }

        public bool IsBakeCapable
        {
            get
            {
                return !m_data.IsEmpty;
            }
        }
    }
}