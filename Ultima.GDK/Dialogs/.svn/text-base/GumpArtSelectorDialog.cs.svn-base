/***************************************************************************
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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Ultima.GDK
{
    public partial class GumpArtSelectorDialog : Form
    {
        private int index = 0;

        public int Index 
        {
            get
            {
                return index;
            }
            set
            {
                index = value;
            }
        }

        public GumpArtSelectorDialog()
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            object[] dummyData = new object[0xFFFF];

            for (int i = 0; i < dummyData.Length; i++)
            {
                dummyData[i] = -1;
            }

            listBox.Items.AddRange(dummyData);

            pictureBox.BackgroundImage = Ultima.Gumps.GetGump(index);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (pictureBox.BackgroundImage == null)
            {
                MessageBox.Show(this, "You may not select invalid images.", "RunUO: GDK");
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void listBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            Brush fontBrush = Brushes.White;

            if (Ultima.Gumps.IsValidIndex(e.Index))
            {
                Bitmap bmp = Ultima.Gumps.GetGump(e.Index);

                int width = bmp.Width;
                int height = bmp.Height;

                if (width > 100)
                {
                    width = 100;
                }

                if (height > 54)
                {
                    height = 54;
                }

                e.Graphics.DrawImage(bmp,
                    new Rectangle(e.Bounds.X + 3, e.Bounds.Y + 3, width, height));
            }
            else
            {
                fontBrush = Brushes.Red;                
            }

            e.Graphics.DrawString(e.Index.ToString(), Font, fontBrush, 
                new PointF((float)105,
                e.Bounds.Y + ((e.Bounds.Height / 2) - 
                (e.Graphics.MeasureString(e.Index.ToString(), Font).Height / 2))));
        }

        private void listBox_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = 60;
        }

        private void listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox.SelectedIndex == -1)
            {
                return;
            }

            if (Ultima.Gumps.IsValidIndex(listBox.SelectedIndex))
            {
                pictureBox.BackgroundImage = Ultima.Gumps.GetGump(listBox.SelectedIndex); 
            }
            else
            {
                pictureBox.BackgroundImage = null;
            }

            index = listBox.SelectedIndex;
        }

        private void GumpArtSelectorDialog_Load(object sender, EventArgs e)
        {

        }
    }
}