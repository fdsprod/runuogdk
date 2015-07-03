using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Ultima.GDK
{
    public partial class Toolbox : Form
    {
        public Toolbox()
        {
            InitializeComponent();
        }

        private void Toolbox_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                return;
            }
        }

        private void btn_MouseHover(object sender, EventArgs e)
        {
            Control btn = sender as Control;

            if (btn != null)
            {
                //toolTip.Show(btn.Text, this, Control.MousePosition);
            }
        }

        private void btn_MouseClick(object sender, MouseEventArgs e)
        {
            Control btn = sender as Control;

            if (btn != null)
            {
                if (clickHandlers.ContainsKey(btn))
                {
                    if (clickHandlers[btn] != null)
                    {
                        clickHandlers[btn](sender, e);
                    }
                }
            }
        }

        public event EventHandler<MouseEventArgs> AlphaButtonClick;
        public event EventHandler<MouseEventArgs> BackgroundButtonClick;
        public event EventHandler<MouseEventArgs> ButtonButtonClick;
        public event EventHandler<MouseEventArgs> CheckboxButtonClick;
        public event EventHandler<MouseEventArgs> HtmlButtonClick;
        public event EventHandler<MouseEventArgs> ImageButtonClick;
        public event EventHandler<MouseEventArgs> ItemButtonClick;
        public event EventHandler<MouseEventArgs> LabelButtonClick;
        public event EventHandler<MouseEventArgs> RadioButtonClick;
        public event EventHandler<MouseEventArgs> TextEntryButtonClick;
        public event EventHandler<MouseEventArgs> TiledImageButtonClick;

        private Dictionary<Control, EventHandler<MouseEventArgs>> clickHandlers;

        private void Toolbox_Load(object sender, EventArgs e)
        {
            clickHandlers = new Dictionary<Control, EventHandler<MouseEventArgs>>();
            clickHandlers.Add(btnAlphaField, AlphaButtonClick);
            clickHandlers.Add(btnBackground, BackgroundButtonClick);
            clickHandlers.Add(btnButton, ButtonButtonClick);
            clickHandlers.Add(btnCheckbox, CheckboxButtonClick);
            clickHandlers.Add(btnHtmlbox, HtmlButtonClick);
            clickHandlers.Add(btnImage, ImageButtonClick);
            clickHandlers.Add(btnItem, ItemButtonClick);
            clickHandlers.Add(btnLabel, LabelButtonClick);
            clickHandlers.Add(btnRadio, RadioButtonClick);
            clickHandlers.Add(btnTextEntry, TextEntryButtonClick);
            clickHandlers.Add(btnTiledImage, TiledImageButtonClick);
        }
    }
}