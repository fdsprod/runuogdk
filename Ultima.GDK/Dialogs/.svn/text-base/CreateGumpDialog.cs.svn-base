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
    public partial class CreateGumpDialog : Form
    {
        private Resolution resolution;

        public Resolution Resolution { get { return resolution; } set { resolution = value; } }
        public string Path { get { return txbPath.Text; } set { txbPath.Text = value; } }
        public string GumpName { get { return txbGumpName.Text; } set { txbPath.Text = value; } }

        public CreateGumpDialog()
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            
            cboGumpSize.SelectedIndex = 1;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txbPath.Text))
            {
                MessageBox.Show("You must specify a path to save the project to.", "RunUO - GDK");
                return;
            }

            if (string.IsNullOrEmpty(txbGumpName.Text))
            {
                MessageBox.Show("You must specify a project name.", "RunUO - GDK");
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnGetPath_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog(this) == DialogResult.OK)
            {
                txbPath.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void cboGumpSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cboGumpSize.Text)
            {
                case "640x480": resolution = Resolution.Res640x480; break;
                case "800x600": resolution = Resolution.Res800x600; break;
                case "1024x768": resolution = Resolution.Res1024x768; break;
            }
        }
    }
}