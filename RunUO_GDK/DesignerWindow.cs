using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ultima.GDK.Gumps;

namespace Ultima.GDK
{
    public partial class DesignerWindow : BaseWindow
    {
        private UndoStack undoStack;
        private RedoStack redoStack;
        private List<BaseGump> copyCollection;

        public Gump Gump
        {
            get { return designerFrame.Gump; }
            set
            {
                ResetStacks();

                if (designerFrame.Gump != null)
                {
                    //designerFrame.Gump
                }

                designerFrame.Gump = value;
            }
        }

        private void ResetStacks()
        {
            if (undoStack != null)
            {
                undoStack.Clear();
            }

            if (redoStack != null)
            {
                redoStack.Clear();
            }
        }

        public event EventHandler<EventArgs> GumpChanged;

        public DesignerWindow()
        {
            InitializeComponent();
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
        }

        private void DesignerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Gump.Modified)
            {
                DialogResult res = MessageBox.Show(this,
                    "This gump has been modified.\n" + 
                    "Do you wish to save the gump before closing it?",
                    "RunUO: GDK",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button1);

                if (res == DialogResult.Yes)
                {
                    if (String.IsNullOrEmpty(Gump.FileName))
                    {
                        if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
                        {
                            Gump.Save(saveFileDialog.FileName);
                        }
                        else
                        {
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        Gump.Save(Gump.FileName);
                    }
                }
                else if (res == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }

        protected virtual void OnGumpChanged(object sender, EventArgs e)
        {
            if (GumpChanged != null)
            {
                GumpChanged(sender, e);
            }
        }

        private void DesignerWindow_Load(object sender, EventArgs e)
        {
            redoStack = new RedoStack();
            undoStack = new UndoStack();
            copyCollection = new List<BaseGump>();

            designerFrame.BeforeBaseGumpChanged += new EventHandler<EventArgs>(designerFrame_BeforeBaseGumpChanged);
            designerFrame.BeforeBaseGumpMoved += new EventHandler<EventArgs>(designerFrame_BeforeBaseGumpMoved);
        }

        private void designerFrame_BeforeBaseGumpMoved(object sender, EventArgs e)
        {
            undoStack.Push(new GumpState(this.Gump.Items));
        }

        private void designerFrame_BeforeBaseGumpChanged(object sender, EventArgs e)
        {
            undoStack.Push(new GumpState(this.Gump.Items));
        }

        private void designerFrame_Copy(object sender, EventArgs e)
        {
            copyCollection = designerFrame.Gump.GetSelectedGumps();
        }

        private void designerFrame_Paste(object sender, EventArgs e)
        {
            for (int i = 0; i < copyCollection.Count; i++)
            {
                designerFrame.Gump.Items.Add(copyCollection[i].Clone());
            }
        }
    }
}