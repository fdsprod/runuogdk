using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ultima.GDK.Gumps;
using System.IO;
using System.Diagnostics;

namespace Ultima.GDK
{
    public partial class MainForm : Form
    {
        private UndoStack undoStack;
        private RedoStack redoStack;
        private HotKeyManager hotkeyManager;

        bool pasting = false;

        public Gump Gump 
        { 
            get { return designerFrame.Gump; } 
            set
            {
                if (designerFrame.Gump != null)
                {
                    UnsubscribeToGump(designerFrame.Gump);
                }

                if (designerFrame.Gump != value)
                {
                    designerFrame.Gump = value;

                    if (designerFrame.Gump != null)
                    {
                        SubscribeToGump(designerFrame.Gump);
                    }
                }
            }
        }

        public MainForm()
        {
            InitializeComponent();
            hotkeyManager = new HotKeyManager("Hotkeys.xml");
        }

        protected override void OnLoad(EventArgs e)
        {
            Location = new Point(Program.Settings.GetValue<int>("mainform_x", Location.X),
                Program.Settings.GetValue<int>("mainform_y", Location.Y));
            Size = new Size(Program.Settings.GetValue<int>("mainform_width", Width), 
                Program.Settings.GetValue<int>("mainform_height", Height));

            toolbox.AlphaButtonClick += new EventHandler<EventArgs>(toolbox_AlphaButtonClick);
            toolbox.BackgroundButtonClick += new EventHandler<EventArgs>(toolbox_BackgroundButtonClick);
            toolbox.ButtonButtonClick += new EventHandler<EventArgs>(toolbox_ButtonButtonClick);
            toolbox.CheckboxButtonClick += new EventHandler<EventArgs>(toolbox_CheckboxButtonClick);
            toolbox.HtmlButtonClick += new EventHandler<EventArgs>(toolbox_HtmlButtonClick);
            toolbox.ImageButtonClick += new EventHandler<EventArgs>(toolbox_ImageButtonClick);
            toolbox.ItemButtonClick += new EventHandler<EventArgs>(toolbox_ItemButtonClick);
            toolbox.LabelButtonClick += new EventHandler<EventArgs>(toolbox_LabelButtonClick);
            toolbox.RadioButtonClick += new EventHandler<EventArgs>(toolbox_RadioButtonClick);
            toolbox.TextEntryButtonClick += new EventHandler<EventArgs>(toolbox_TextEntryButtonClick);
            toolbox.TiledImageButtonClick += new EventHandler<EventArgs>(toolbox_TiledImageButtonClick);
            toolbox.InitializeClickHandlers();

            undoStack = new UndoStack();
            redoStack = new RedoStack();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            
            Application.Exit();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            Program.Settings.SetValue<int>("mainform_x", Location.X);
            Program.Settings.SetValue<int>("mainform_y", Location.Y);
            Program.Settings.SetValue<int>("mainform_width", Width);
            Program.Settings.SetValue<int>("mainform_height", Height);
        }

        private void SubscribeToGump(Gump gump)
        {
            gump.AllItemsDeselected += new EventHandler<EventArgs>(Gump_AllItemsDeselected);
            gump.AllItemsSelected += new EventHandler<EventArgs>(Gump_AllItemsSelected);   
            gump.Invalidated += new EventHandler<EventArgs>(Gump_Invalidated);
            gump.ItemAdded += new EventHandler<GumpCollectionEventArgs>(Gump_ItemAdded);
            gump.ItemRemoved += new EventHandler<GumpCollectionEventArgs>(Gump_ItemRemoved);
            gump.LoadCompleted += new EventHandler<EventArgs>(Gump_LoadCompleted);
            gump.LoadStarted += new EventHandler<EventArgs>(Gump_LoadStarted);
            gump.SavingCompleted += new EventHandler<EventArgs>(Gump_SavingCompleted);
            gump.SavingStarted += new EventHandler<EventArgs>(Gump_SavingStarted);
        }

        private void UnsubscribeToGump(Gump gump)
        {
            gump.AllItemsDeselected -= new EventHandler<EventArgs>(Gump_AllItemsDeselected);
            gump.AllItemsSelected -= new EventHandler<EventArgs>(Gump_AllItemsSelected);
            gump.Invalidated -= new EventHandler<EventArgs>(Gump_Invalidated);
            gump.ItemAdded -= new EventHandler<GumpCollectionEventArgs>(Gump_ItemAdded);
            gump.ItemRemoved -= new EventHandler<GumpCollectionEventArgs>(Gump_ItemRemoved);
            gump.LoadCompleted -= new EventHandler<EventArgs>(Gump_LoadCompleted);
            gump.LoadStarted -= new EventHandler<EventArgs>(Gump_LoadStarted);
            gump.SavingCompleted -= new EventHandler<EventArgs>(Gump_SavingCompleted);
            gump.SavingStarted -= new EventHandler<EventArgs>(Gump_SavingStarted);
        }

        private void ChangedDesignerFocus()
        {
            propertyGrid.SelectedObjects = Gump.GetSelectedGumps().ToArray();
        }

        private void AddGump(BaseGump baseGump)
        {   
            if (Gump != null)
            {
                undoStack.Push(new GumpState(Gump.Items));
                Gump.Items.Add(baseGump);
            }
        }

        private void Gump_SavingStarted(object sender, EventArgs e)
        {
            statusLabel.Text = "Status: Saving...";
        }

        private void Gump_SavingCompleted(object sender, EventArgs e)
        {
            statusLabel.Text = "Status: Idle";
        }

        private void Gump_LoadStarted(object sender, EventArgs e)
        {
            statusLabel.Text = "Status: Loading...";
        }

        private void Gump_LoadCompleted(object sender, EventArgs e)
        {
            statusLabel.Text = "Status: Idle";
        }

        private void Gump_ItemRemoved(object sender, GumpCollectionEventArgs e)
        {
            RefreshPropertyGrid();

            for( int a = 0; a < cboGumpItems.Items.Count; a++ )
            {
                if (e.Item.GetDesignerString() == cboGumpItems.Items[a].ToString())
                {
                    cboGumpItems.Items.RemoveAt(a);
                    break;
                }
            }
        }

        private void RefreshPropertyGrid()
        {
            if (Gump != null)
            {
                List<BaseGump> selected = Gump.GetSelectedGumps();

                if(selected.Count > 0)
                {
                    propertyGrid.SelectedObjects = selected.ToArray(); 
                }
                else
                {
                    propertyGrid.SelectedObjects = new object[] { designerFrame.Gump };
                }
            }
           
        }

        private void Gump_ItemAdded(object sender, GumpCollectionEventArgs e)
        {
            int index = cboGumpItems.Items.Add(e.Item.GetDesignerString());
            cboGumpItems.SelectedIndex = index;

            RefreshPropertyGrid();
        }

        private void Gump_Invalidated(object sender, EventArgs e)
        {
            //propertyGrid.SelectedObjects = Gump.GetSelectedGumps().ToArray();
        }

        private void designerWindow_GumpChanged(object sender, EventArgs e)
        {
            RefreshPropertyGrid();
        }

        private void Gump_AllItemsSelected(object sender, EventArgs e)
        {
            RefreshPropertyGrid();
        }

        private void Gump_AllItemsDeselected(object sender, EventArgs e)
        {
            RefreshPropertyGrid();
        }

        private void newToolStripButton1_Click(object sender, EventArgs e)
        {
            CreateGumpDialog cgd = new CreateGumpDialog();

            string myGumpsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "My Gumps");

            if (!Directory.Exists(myGumpsPath))
            {
                Directory.CreateDirectory(myGumpsPath);
            }

            cgd.Path = Program.Settings.GetValue<string>("last_path", myGumpsPath);

            if (cgd.ShowDialog(this) == DialogResult.OK)
            {

                Program.Settings.SetValue<string>("last_path", cgd.Path);
                if (this.Gump != null && this.Gump.Modified)
                {
                    DialogResult res = MessageBox.Show(this, "Do you wish to save changes to your current gump?", "RunUO: GDK", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);

                    if (res == DialogResult.Cancel)
                    {
                        return;
                    }
                    else if (res == DialogResult.Yes)
                    {
                        this.Gump.Save(Gump.FileName);
                    }
                }

                Gump g = new Gump();
                g.Resolution = cgd.Resolution;
                g.Name = cgd.GumpName;
                //g.FileName = Path.Combine(cgd.Path, g.Name);
                
                Gump = g;
                
                Invalidate(true);
            }
        }

        private void openToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            DialogResult res = openFileDialog.ShowDialog();

            if (res == DialogResult.OK)
            {
                string file = openFileDialog.FileName;

                if (this.Gump != null && this.Gump.Modified)
                {
                    DialogResult res2 = MessageBox.Show(this, "Do you wish to save changes to your current gump?", "RunUO: GDK", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);

                    if (res2 == DialogResult.Cancel)
                    {
                        return;
                    }
                    else if (res2 == DialogResult.Yes)
                    {
                        this.Gump.Save(Gump.FileName);
                    }
                }

                Gump g = new Gump();
                bool success = g.Load(file);

                if (!success)
                {
                    MessageBox.Show(this, "The file you were trying to load was either not a gdk file or is corrupt.", "RunUO: GDK");
                    return;
                }

                Gump = g;

                Invalidate();
            }
        }

        private void saveToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (Gump != null)
            {
                if (string.IsNullOrEmpty(Gump.FileName))
                {
                    saveFileDialog.Filter = "RunUO: GDK files(*.gdk)|*.gdk";
                    DialogResult res = saveFileDialog.ShowDialog(this);

                    if (res == DialogResult.OK)
                    {
                        Gump.Save(saveFileDialog.FileName);
                    }
                }
                else
                {
                    Gump.Save(Gump.FileName);
                }
            }
        }

        public void Save()
        {
            saveAsToolStripMenuItem2_Click(null, null);
        }

        private void saveAsToolStripMenuItem2_Click(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void undoToolStripMenuItem2_Click(object sender, EventArgs e)
        {

        }

        private void redoToolStripMenuItem2_Click(object sender, EventArgs e)
        {

        }

        private void cutToolStripMenuItem2_Click(object sender, EventArgs e)
        {

        }

        private void copyToolStripMenuItem2_Click(object sender, EventArgs e)
        {

        }

        private void pasteToolStripMenuItem2_Click(object sender, EventArgs e)
        {

        }

        private void selectAllToolStripMenuItem2_Click(object sender, EventArgs e)
        {

        }

        private void optionsToolStripMenuItem2_Click(object sender, EventArgs e)
        {

        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolbox_TiledImageButtonClick(object sender, EventArgs e)
        {
            AddGump(new TiledImageGump());            
        }        

        private void toolbox_TextEntryButtonClick(object sender, EventArgs e)
        {
            AddGump(new TextEntryGump());
        }

        private void toolbox_RadioButtonClick(object sender, EventArgs e)
        {
            AddGump(new RadioGump());
        }

        private void toolbox_LabelButtonClick(object sender, EventArgs e)
        {
            AddGump(new LabelGump());
        }

        private void toolbox_ItemButtonClick(object sender, EventArgs e)
        {
            AddGump(new ItemGump());
        }

        private void toolbox_ImageButtonClick(object sender, EventArgs e)
        {
            AddGump(new ImageGump());
        }

        private void toolbox_HtmlButtonClick(object sender, EventArgs e)
        {
            AddGump(new HTMLGump());
        }

        private void toolbox_CheckboxButtonClick(object sender, EventArgs e)
        {
            AddGump(new CheckboxGump());
        }

        private void toolbox_ButtonButtonClick(object sender, EventArgs e)
        {
            AddGump(new ButtonGump());
        }

        private void toolbox_BackgroundButtonClick(object sender, EventArgs e)
        {
            AddGump(new BackgroundGump());
        }

        private void toolbox_AlphaButtonClick(object sender, EventArgs e)
        {
            AddGump(new AlphaGump());
        }

        private void donationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_xclick&business=jeff%40runuo%2ecom&buyer_credit_promo_code=&buyer_credit_product_category=&buyer_credit_shipping_method=&buyer_credit_user_address_change=&no_shipping=0&no_note=1&tax=0&currency_code=USD&lc=US&bn=PP%2dDonationsBF&charset=UTF%2d8");
        }

        private void aboutToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            AboutBox dialog = new AboutBox();
            dialog.ShowDialog(this);
        }

        private void supportToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Process.Start("http://www.runuoforge.org/gf/project/runuogdk/");
        }

        private void designerFrame_MouseUp(object sender, MouseEventArgs e)
        {
            RefreshPropertyGrid();
        }

        private void propertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            this.propertyGrid.Cursor = System.Windows.Forms.Cursors.Arrow;
            
            designerFrame.Invalidate();
        }

        private List<BaseGump> copyCache;

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            KeyData keyData = new KeyData();
            keyData.Alt = e.Alt;
            keyData.Control = e.Control;
            keyData.Shift = e.Shift;
            keyData.Key = e.KeyData;

            hotkeyManager.OnKeyDown(this, new HotKeyEventArgs(Gump, designerFrame, keyData));
            /*

            //Copy
            if (e.Control && (e.KeyCode & Keys.C) == Keys.C)
            {
                List<BaseGump> selected = designerFrame.Gump.GetSelectedGumps();
                copyCache = new List<BaseGump>(selected.Count);
                for (int i = 0; i < selected.Count; i++)
                {
                    copyCache.Add(selected[i]);
                }
            }

            //Paste
            if (e.Control && (e.KeyCode & Keys.V) == Keys.V)
            {
                if (copyCache != null && copyCache.Count > 0)
                {
                    Gump.DeselectAll();

                    copyCache.Sort(new Comparison<BaseGump>(CompareZ));

                    for (int i = copyCache.Count - 1; i >= 0; i--)
                    {
                        BaseGump bg = copyCache[i].Clone();
                        bg.Selected = true;

                        Gump.Items.Add(bg);
                    }
                }
            }
            
            //Cut

            //Select All
            if (e.Control && e.KeyCode == Keys.A)
            {
                Gump.SelectAll();
            }*/
        }

        private int CompareZ(BaseGump a, BaseGump b)
        {
            return b.Z.CompareTo(a.Z);
        }

        private void cntxZlevel_Opening(object sender, CancelEventArgs e)
        {
            if (Gump == null || Gump.GetSelectedGumps().Count == 0)
            {
                e.Cancel = true;
                return;
            }
        }

        private void moveForwardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BaseGump.SuspendInvalidation = true;
            Gump.MoveForward(Gump.GetSelectedGumps());
            BaseGump.SuspendInvalidation = false;
            Gump.Invalidate();
        }

        private void moveBackwardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BaseGump.SuspendInvalidation = true;
            Gump.MoveBackward(Gump.GetSelectedGumps());
            BaseGump.SuspendInvalidation = false;
            Gump.Invalidate();
        }

        private void moveToBackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BaseGump.SuspendInvalidation = true;
            Gump.MoveToBack(Gump.GetSelectedGumps());
            BaseGump.SuspendInvalidation = false;
            Gump.Invalidate();
        }

        private void moveToFrontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BaseGump.SuspendInvalidation = true;
            Gump.MoveToFront(Gump.GetSelectedGumps());
            BaseGump.SuspendInvalidation = false;
            Gump.Invalidate();
        }

        private void designerFrame_BeforeBaseGumpChanged(object sender, EventArgs e)
        {
            undoStack.Push(new GumpState(Gump.Items));
        }

        private void designerFrame_BeforeBaseGumpMoved(object sender, EventArgs e)
        {
            //undoStack.Push(new GumpState(Gump.Items));
        }

        private void exportToScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Gump == null)
            {
                return;
            }

            DialogResult res = exportGumpFileDialog.ShowDialog(this);

            if (res == DialogResult.OK)
            {
                string file = exportGumpFileDialog.FileName;

                StreamWriter writer = new StreamWriter(file);
                writer.Write(Gump.ToString());
                writer.Close();
            }
        }

        private void saveAsToolStripMenuItem2_Click_1(object sender, EventArgs e)
        {
            if( Gump != null )
            {
                saveFileDialog.Filter = "RunUO: GDK files(*.gdk)|*.gdk";
                DialogResult res = saveFileDialog.ShowDialog(this);

                if (res == DialogResult.OK)
                {
                    Gump.Save(saveFileDialog.FileName);
                }
            }
        }

        private void cboGumpItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Gump != null && !pasting)
            {
                for (int i = 0; i < Gump.Items.Count; i++)
                {
                    if (cboGumpItems.SelectedItem.ToString() == Gump.Items[i].GetDesignerString())
                    {
                        Gump.DeselectAll();
                        Gump.Items[i].Selected = true;

                        break;
                    }
                }
            }

            Invalidate();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void designerFrame_Copied(object sender, EventArgs e)
        {
            if(Gump != null)
            {
                List<BaseGump> selected = Gump.GetSelectedGumps();
                copyCache = new List<BaseGump>(selected.Count);
                for (int i = 0; i < selected.Count; i++)
                {
                    copyCache.Add(selected[i]);
                }
            }

        }

        private void designerFrame_Pasted(object sender, EventArgs e)
        {
            if(Gump != null)
            {
                if (copyCache != null && copyCache.Count > 0)
                {
                    pasting = true;
                    Gump.DeselectAll();

                    copyCache.Sort(new Comparison<BaseGump>(CompareZ));

                    for (int i = copyCache.Count - 1; i >= 0; i--)
                    {
                        BaseGump bg = copyCache[i].Clone();
                        bg.Selected = true;

                        Gump.Items.Add(bg);
                    }

                    pasting = false;
                }
            }
        }

        private void designerFrame_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}