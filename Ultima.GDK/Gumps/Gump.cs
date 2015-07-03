/*************************************************************************
 *
 * $Author: Jeff Boulanger
 * 
 * This work is protected by the Creative Commons Attribution-Noncommercial-No 
 * Derivative Works 3.0 Unported License.  All information regarding this 
 * license can be found at http://creativecommons.org/licenses/by-nc-nd/3.0/
 *
 ***************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using Ultima.GDK.Gumps;
using System.Windows.Forms;
using System.Drawing;
using System.Xml;
using System.IO;
using System.Reflection;
using System.ComponentModel;

namespace Ultima.GDK
{
    public class Gump
    {
        public const int GumpVersion = 1;

        private bool resizable;
        private bool dragable;
        private bool closable;
        private bool isOpen;
        private bool modified;
        private bool suspendSorting = false;
        private int x;
        private int y;
        private string name;
        private string filename;
        private BaseGumpCollection items;
        private Resolution resolution;

        public Resolution Resolution { get { return resolution; } set { resolution = value; } }        
        public bool Resizable { get { return resizable; } set { resizable = value; } }
        public bool Dragable { get { return dragable; } set { dragable = value; } }
        public bool Closable { get { return closable; } set { closable = value; } }
        [Browsable(false)]
        public bool IsOpen { get { return isOpen; } set { isOpen = value; } }
        [Browsable(false)]
        public bool Modified { get { return modified; } set { modified = value; } }
        public int X { get { return x; } set { x = value; } }
        public int Y { get { return y; } set { y = value; } }
        public string Name { get { return name; } set { name = value; } }
        [Browsable(false)]
        public string FileName { get { return filename; } set { filename = value; } }
        [Browsable(false)]
        public BaseGumpCollection Items { get { return items; } set { items = value; } }

        public event EventHandler<EventArgs> Invalidated;
        public event EventHandler<EventArgs> AllItemsSelected;
        public event EventHandler<EventArgs> AllItemsDeselected;
        public event EventHandler<GumpCollectionEventArgs> ItemAdded;
        public event EventHandler<GumpCollectionEventArgs> ItemRemoved;
        public event EventHandler<EventArgs> SavingStarted;
        public event EventHandler<EventArgs> SavingCompleted;
        public event EventHandler<EventArgs> LoadStarted;
        public event EventHandler<EventArgs> LoadCompleted;
        public event EventHandler<EventArgs> BaseGumpMoved;
        public event EventHandler<EventArgs> BaseGumpChanged;
        public event EventHandler<EventArgs> BeforeBaseGumpMoved;
        public event EventHandler<EventArgs> BeforeBaseGumpChanged;

        public Gump() : this(0, 0) { }

        public Gump(int x, int y)
        {
            items = new BaseGumpCollection();

            items.ItemAdded += new EventHandler<GumpCollectionEventArgs>(OnItemAdded);
            items.ItemRemoved += new EventHandler<GumpCollectionEventArgs>(OnItemRemoved);
        }

        protected virtual void OnItemRemoved(object sender, GumpCollectionEventArgs e)
        {
            Modified = true;
            Invalidate();

            if (ItemRemoved != null)
            {
                ItemRemoved(sender, e);
            }
        }

        protected virtual void OnItemAdded(object sender, GumpCollectionEventArgs e)
        {
            Modified = true;
            e.Item.Parent = this;
            e.Item.Z = items.Count;
            Invalidate();

            if (string.IsNullOrEmpty(e.Item.Name))
            {
                Type t = e.Item.GetType();
                int count = FindGumpsByType(t).Count;

                e.Item.Name = String.Format("{0}{1}", Path.GetExtension(t.ToString()).TrimStart('.'), count);
            }

            if (ItemAdded != null)
            {
                ItemAdded(sender, e);
            }
        }

        private int CompareZ(BaseGump a, BaseGump b)
        {
            return b.Z.CompareTo(a.Z);
        }

        public List<BaseGump> FindGumpsByType<T>()
        {
            List<BaseGump> gumps = new List<BaseGump>();

            for (int i = 0; i < items.Count; i++)
            {
                BaseGump g = items[i];
                if (g.GetType() == typeof(T))
                {
                    gumps.Add(items[i]);
                }
            }

            return gumps;
        }

        public List<BaseGump> FindGumpsByType( Type type )
        {
            List<BaseGump> gumps = new List<BaseGump>();

            for (int i = 0; i < items.Count; i++)
            {
                BaseGump g = items[i];
                if (g.GetType() == type)
                {
                    gumps.Add(items[i]);
                }
            }

            return gumps;
        }

        public BaseGump FindFirstGumpsByType<T>()
        {
            for (int i = 0; i < items.Count; i++)
            {
                BaseGump g = items[i];
                if (g.GetType() == typeof(T))
                {
                    return g;
                }
            }

            return null;
        }

        public List<BaseGump> GetSelectedGumps()
        {
            List<BaseGump> selected = new List<BaseGump>();

            for (int i = 0; i < items.Count; i++)
                if (items[i].Selected)
                {
                    selected.Add(items[i]);
                }

            return selected;
        }

        public void SelectAll()
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i].Selected = true;
            }

            OnSelectAll(this, EventArgs.Empty);
        }

        protected virtual void OnSelectAll(object sender, EventArgs e)
        {
            if (AllItemsSelected != null)
            {
                AllItemsSelected(sender, e);
            }
        }

        protected virtual void OnDeselectAll(object sender, EventArgs e)
        {
            if (AllItemsDeselected != null)
            {
                AllItemsDeselected(sender, e);
            }
        }

        public void DeselectAll()
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i].Selected = false;
            }
        }

        public void InvertSelected()
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i].Selected = !items[i].Selected;
            }
        }

        public virtual Size GetResolution()
        {
            switch(resolution)
            {
                case Resolution.Res640x480:
                    return new Size(640, 480);
                case Resolution.Res800x600:
                    return new Size(800, 600);
                case Resolution.Res1024x768:
                    return new Size(1024, 768);
            }

            return new Size(800, 600);
        }

        public virtual Bitmap Render()
        {
            Size size = GetResolution();
            Bitmap b = new Bitmap(size.Width, size.Height);
            Graphics g = Graphics.FromImage(b);

            PaintEventArgs args = new PaintEventArgs(g, new Rectangle(0, 0, size.Width, size.Height));

            OnPaint(args);

            g.Dispose();
            return b;
        }

        internal virtual void OnPaint(PaintEventArgs args)
        {
            Size size = GetResolution();
            args.Graphics.FillRectangle(Brushes.Black, new Rectangle(0, 0, size.Width, size.Height));

            for (int i = items.Count - 1; i >= 0; i--)
            {
                items[i].OnPaint(args);
            }
        }

        public void Invalidate()
        {
            if (!suspendSorting)
            {
                Sort();
            }

            if (Invalidated != null)
            {
                Invalidated(this, EventArgs.Empty);
            }
        }

        public void Sort()
        {
            items.items.Sort(new Comparison<BaseGump>(CompareZ));
        }

        private void SwapZ(BaseGump item1, BaseGump item2)
        {
            int z = item1.Z;
            item1.Z = item2.Z;
            item2.Z = z;
        }

        public void MoveForward(List<BaseGump> items)
        {
            suspendSorting = true;

            items.Sort(CompareZ);
            int selectedIdex = 0;

            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].Z - 1 == items[selectedIdex].Z && Items[i] != items[selectedIdex])
                {
                    SwapZ(items[selectedIdex], Items[i]);
                    selectedIdex++;

                    if (selectedIdex >= items.Count)
                    {
                        break;
                    }
                }
            }

            suspendSorting = false;
            Invalidate();
        }

        public void DeleteSelected()
        {
            List<BaseGump> selected = GetSelectedGumps();

            for (int i = 0; i < selected.Count; i++)
            {
                Items.Remove(selected[i]);
                selected[i].Dispose();
            }

            Invalidate();
        }

        public void MoveBackward(List<BaseGump> items)
        {
            suspendSorting = true;

            items.Sort(CompareZ);
            int selectedIdex = 0;

            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].Z + 1 == items[selectedIdex].Z && Items[i] != items[selectedIdex])
                {
                    SwapZ(items[selectedIdex], Items[i]);
                    selectedIdex++;

                    if (selectedIdex >= items.Count)
                    {
                        break;
                    }
                }
            }

            suspendSorting = false;
            Invalidate();
        }

        public void MoveToFront(List<BaseGump> items)
        {
            suspendSorting = true;

            suspendSorting = false;
            Invalidate();
        }

        public void MoveToBack(List<BaseGump> items)
        {
            suspendSorting = true;

            suspendSorting = false;
            Invalidate();
        }

        public BaseGump GetFirstGumpAt(Point point)
        {
            for (int i = 0; i < items.Count; i++)
            {   
                Point imgPoint = Items[i].ConvertPointToImagePoint(point);
                if (Items[i].Bounds.Contains(point) && 
                    Items[i].Image.GetPixel(imgPoint.X, imgPoint.Y) != Utility.EmptyColor)
                {
                    return Items[i];
                }
            }

            return null;
        }

        public override string ToString()
        {
            Sort();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine("using Server;");
            sb.AppendLine("using Server.Items;");
            sb.AppendLine("using Server.Network;");
            sb.AppendLine("using Server.Commands;\n");

            sb.AppendLine("namespace Server.Gumps");
            sb.AppendLine("{");
                sb.AppendFormat("\tpublic class {0}Gump : Gump\n", Name);
                sb.AppendLine("\t{");

                    sb.AppendFormat("\t\tpublic {0}Gump() : base({1}, {2})\n", Name, X, Y);
		            sb.AppendLine("\t\t{");
                        sb.AppendFormat("\t\t\tClosable = {0};\n", Closable.ToString().ToLower());
                        sb.AppendFormat("\t\t\tDragable = {0};\n", Dragable.ToString().ToLower());
                        sb.AppendFormat("\t\t\tResizable = {0};\n", Resizable.ToString().ToLower());

                        for (int i = Items.Count - 1; i >= 0; i--)// Items.Count; i++)
                        {
                            sb.AppendLine("\t" + Items[i].ToString());
                        }

		            sb.AppendLine("\t\t}\n");

                    List<BaseGump> enumTypes = new List<BaseGump>();
                    enumTypes.AddRange(FindGumpsByType<ButtonGump>());
                    enumTypes.AddRange(FindGumpsByType<CheckboxGump>());
                    enumTypes.AddRange(FindGumpsByType<RadioGump>());
                    enumTypes.AddRange(FindGumpsByType<TextEntryGump>());

                    if (enumTypes.Count > 0)
                    {
                        sb.AppendLine("\t\tpublic enum ButtonTypes");
                        sb.AppendLine("\t\t{");
                            for (int i = 0; i < enumTypes.Count; i++)
                            {
                                int value = -1;

                                if (enumTypes[i].GetType() == typeof(ButtonGump))
                                {
                                    value = ((ButtonGump)enumTypes[i]).Value;
                                }
                                if (enumTypes[i].GetType() == typeof(CheckboxGump))
                                {
                                    value = ((CheckboxGump)enumTypes[i]).Value;
                                }
                                if (enumTypes[i].GetType() == typeof(RadioGump))
                                {
                                    value = ((RadioGump)enumTypes[i]).Value;
                                }
                                if (enumTypes[i].GetType() == typeof(TextEntryGump))
                                {
                                    value = ((TextEntryGump)enumTypes[i]).EntryId;
                                }

                                sb.AppendFormat("\t\t\t{0} = {1}{2}\n", 
                                    string.IsNullOrEmpty(enumTypes[i].Name) ? enumTypes[i].GetType().ToString() + i.ToString() : enumTypes[i].Name, 
                                                value, (i + 1 == Items.Count) ? "" : ",");
                            }

                        sb.AppendLine("\t\t}\n");
                    }

                    sb.AppendLine("\t\tpublic override void OnResponse( NetState sender, RelayInfo info )");
		            sb.AppendLine("\t\t{");
			        sb.AppendLine("\t\t\tMobile from = sender.Mobile;");
                    sb.AppendLine("\t\t}");
                sb.AppendLine("\t}");
            sb.AppendLine("}");

            return sb.ToString();
        }

        public bool CheckResizing(Point point, out ResizeType resizeType, List<BaseGump> gumps)
        {
            resizeType = ResizeType.None;

            if (gumps == null || gumps.Count != 1)
            {
                return false;
            }

            if (!gumps[0].Resizable)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    if (gumps[0].CornerHandles[i].Contains(point))
                    {
                        resizeType = (ResizeType)i + 1;
                        return true;
                    }
                    if (gumps[0].SideHandles[i].Contains(point))
                    {
                        resizeType = (ResizeType)(i + 5);
                        return true;
                    }
                }
            }

            return false;
        }

        public void Save(string path)
        {
            filename = path;

            if (Path.GetExtension(path) != ".gdk")
            {
                path += ".gdk";
            }

            if(!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.CloseOutput = true;
            settings.ConformanceLevel = ConformanceLevel.Document;
            settings.Encoding = Encoding.Unicode;
            settings.Indent = true;
            settings.IndentChars = "\t";
            settings.NewLineChars = "\n";
            settings.NewLineHandling = NewLineHandling.Entitize;
            settings.NewLineOnAttributes = false;
            settings.OmitXmlDeclaration = true;

            XmlWriter writer = XmlWriter.Create(path, settings);

            Save(writer);
        }

        public void Save(XmlWriter writer)
        {
            OnSaveStarted(this, EventArgs.Empty);

            writer.WriteStartElement("Gump");
            writer.WriteAttributeString("version", GumpVersion.ToString());
            writer.WriteAttributeString("resizable", resizable.ToString());
            writer.WriteAttributeString("dragable", dragable.ToString());
            writer.WriteAttributeString("closable", closable.ToString());
            writer.WriteAttributeString("x", x.ToString());
            writer.WriteAttributeString("y", y.ToString());
            writer.WriteAttributeString("name", name);

            foreach (BaseGump bg in Items)
            {
                writer.WriteStartElement("BaseGump");
                bg.Serialize(writer);
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
            writer.Close();

            OnSaveCompleted(this, EventArgs.Empty);
        }
        
        protected virtual void OnSaveStarted(object sender, EventArgs e)
        {
            if (SavingStarted != null)
            {
                SavingStarted(sender, e);
            }
        }

        protected virtual void OnSaveCompleted(object sender, EventArgs e)
        {
            if (SavingCompleted != null)
            {
                SavingCompleted(sender, e);
            }
        }

        protected virtual void OnLoadStarted(object sender, EventArgs e)
        {
            if (LoadStarted != null)
            {
                LoadStarted(sender, e);
            }
        }

        protected virtual void OnLoadCompleted(object sender, EventArgs e)
        {
            if (LoadCompleted != null)
            {
                LoadCompleted(sender, e);
            }
        }

        protected virtual void OnBeforeBaseGumpChanged(object sender, EventArgs e)
        {
            if (BeforeBaseGumpChanged != null)
            {
                BeforeBaseGumpChanged(sender, e);
            }
        }

        protected virtual void OnBaseGumpChanged(object sender, EventArgs e)
        {
            if (BaseGumpChanged != null)
            {
                BaseGumpChanged(sender, e);
            }
        }

        protected virtual void OnBeforeBaseGumpMoved(object sender, EventArgs e)
        {
            if (BeforeBaseGumpMoved != null)
            {
                BeforeBaseGumpMoved(sender, e);
            }
        }

        public void MoveSelected(int x, int y)
        {
            if (x != 0 || y != 0)
            {
                List<BaseGump> toMove = GetSelectedGumps();

                if (toMove != null)
                {
                    if (toMove.Count > 0)
                    {
                        OnBeforeBaseGumpMoved(this, EventArgs.Empty);

                        for (int i = 0; i < toMove.Count; i++)
                        {
                            toMove[i].MoveBy(x, y);
                        }
                    }
                }
            }
        }

        public bool Load(string path)
        {
            XmlDocument doc = new XmlDocument();
            FileName = path;

            try
            {
                doc.Load(path);
            }
            catch
            {
                return false;
            }

            return Load(doc);
        }

        public bool Load(XmlDocument doc)
        {
            try
            {
                OnLoadStarted(this, EventArgs.Empty);

                XmlElement root = doc.DocumentElement;
                XmlNodeList baseGumps = root.GetElementsByTagName("BaseGump");

                for (int i = baseGumps.Count - 1; i >= 0; i--)
                {
                    Type type = Type.GetType(baseGumps[i].Attributes["type"].Value);
                    BaseGump gump = (BaseGump)Activator.CreateInstance(type);

                    string xml = baseGumps[i].OuterXml;
                    char[] chars = xml.ToCharArray();
                    byte[] xmlData = Utility.ConvertCharToByteArray(chars);
                    XmlReader reader = XmlReader.Create(new MemoryStream(xmlData, 0, xmlData.Length));
                    reader.Read();
                    gump.Deserialize(reader);
                    reader.Close();

                    Items.Add(gump);
                }

                OnLoadCompleted(this, EventArgs.Empty);
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
