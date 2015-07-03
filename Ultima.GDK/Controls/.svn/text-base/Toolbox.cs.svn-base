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
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Ultima.GDK.Controls
{
    public partial class Toolbox : UserControl
    {
        public event EventHandler<EventArgs> AlphaButtonClick;
        public event EventHandler<EventArgs> BackgroundButtonClick;
        public event EventHandler<EventArgs> ButtonButtonClick;
        public event EventHandler<EventArgs> CheckboxButtonClick;
        public event EventHandler<EventArgs> HtmlButtonClick;
        public event EventHandler<EventArgs> ImageButtonClick;
        public event EventHandler<EventArgs> ItemButtonClick;
        public event EventHandler<EventArgs> LabelButtonClick;
        public event EventHandler<EventArgs> RadioButtonClick;
        public event EventHandler<EventArgs> TextEntryButtonClick;
        public event EventHandler<EventArgs> TiledImageButtonClick;

        private Dictionary<Control, EventHandler<EventArgs>> clickHandlers;

        public Toolbox()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        public void InitializeClickHandlers()
        {
            clickHandlers = new Dictionary<Control, EventHandler<EventArgs>>();
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

        private void btn_Click(object sender, EventArgs e)
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

        private void btn_MouseHover(object sender, EventArgs e)
        {
            toolTip.Show((string)((Button)sender).Tag, this, Control.MousePosition.X, Control.MousePosition.Y);
        }

    }
}
