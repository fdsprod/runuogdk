namespace Ultima.GDK
{
    partial class ItemArtSelectorDialog
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
            this.itemSelector = new Ultima.GDK.ItemSelector();
            this.SuspendLayout();
            // 
            // itemSelector
            // 
            this.itemSelector.ArtType = Ultima.GDK.ArtType.Item;
            this.itemSelector.BackColor = System.Drawing.Color.White;
            this.itemSelector.Dock = System.Windows.Forms.DockStyle.Fill;
            this.itemSelector.ItemHeight = 44;
            this.itemSelector.ItemWidth = 44;
            this.itemSelector.Location = new System.Drawing.Point(0, 0);
            this.itemSelector.Name = "itemSelector";
            this.itemSelector.Size = new System.Drawing.Size(489, 416);
            this.itemSelector.TabIndex = 0;
            this.itemSelector.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.itemSelector_MouseDoubleClick);
            // 
            // ItemArtSelectorDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(489, 416);
            this.Controls.Add(this.itemSelector);
            this.Name = "ItemArtSelectorDialog";
            this.Text = "ItemArtSelectorDialog";
            this.ResumeLayout(false);

        }

        #endregion

        private ItemSelector itemSelector;
    }
}