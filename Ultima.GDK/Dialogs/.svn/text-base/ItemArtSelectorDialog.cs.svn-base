using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Ultima.GDK
{
    public partial class ItemArtSelectorDialog : Form
    {
        public int Index = 0;

        public ItemArtSelectorDialog()
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
        }

        private void itemSelector_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Index = itemSelector.Index;
            if (Ultima.Art.FileIndex.Index[Index].length <= 0)
            {
                MessageBox.Show(this, "You may not select invalid images.", "RunUO: GDK");
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}