namespace Ultima.GDK
{
    partial class DesignerWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.designerFrame = new Ultima.GDK.DesignerFrame();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.SuspendLayout();
            // 
            // designerFrame
            // 
            this.designerFrame.BackColor = System.Drawing.SystemColors.Control;
            this.designerFrame.Dock = System.Windows.Forms.DockStyle.Fill;
            this.designerFrame.Gump = null;
            this.designerFrame.Location = new System.Drawing.Point(0, 0);
            this.designerFrame.Name = "designerFrame";
            this.designerFrame.Size = new System.Drawing.Size(632, 454);
            this.designerFrame.TabIndex = 0;
            this.designerFrame.GumpChanged += new System.EventHandler<System.EventArgs>(this.OnGumpChanged);
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "gdk";
            this.saveFileDialog.Filter = "GDK files|*.gdk|All files|*.*";
            // 
            // DesignerWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(632, 454);
            this.Controls.Add(this.designerFrame);
            this.MaximumSize = new System.Drawing.Size(800, 600);
            this.MinimumSize = new System.Drawing.Size(100, 200);
            this.Name = "DesignerWindow";
            this.Text = "Gump Designer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DesignerForm_FormClosing);
            this.Load += new System.EventHandler(this.DesignerWindow_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private DesignerFrame designerFrame;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
    }
}