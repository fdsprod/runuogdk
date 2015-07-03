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
using System.Text;
using System.Drawing.Design;
using System.ComponentModel;
using System.Windows.Forms.Design;
using System.Drawing;

namespace Ultima.GDK
{
    public class HueEditor : UITypeEditor
    {
        protected IWindowsFormsEditorService editorService;
        protected Hue returnValue;

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (value == null)
            {
                value = Hues.GetHue(0);
            }

            if (value.GetType() == typeof(Hue))
            {
                editorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

                if (editorService == null)
                {
                    return value;
                }

                HuePickerControl control = new HuePickerControl((Hue)value);
                control.ValueChanged += new HuePickerControl.ValueChangedEventHandler(this.ValueSelected);
                editorService.DropDownControl(control);

                if (returnValue != null)
                {
                    control.Dispose();
                    return returnValue;
                }

                control.Dispose();
            }

            return value;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override void PaintValue(PaintValueEventArgs e)
        {
            Graphics graphics = e.Graphics;
            graphics.FillRectangle(Brushes.White, e.Bounds);
            
            float width = ((float)(e.Bounds.Width - 3)) / 32f;
            Hue hue = (Hue)e.Value;
            int i = 0;

            foreach (short color in hue.Colors)
            {
                Rectangle rect = new Rectangle((int)Math.Round((double)(e.Bounds.X + (i * width))), e.Bounds.Y, ((int)Math.Round((double)width)) + 1, e.Bounds.Height);
                graphics.FillRectangle(new SolidBrush(Utility.Convert555ToARGB(color)), rect);

                i++;
            }
        }

        protected void ValueSelected(Hue Hue)
        {
            editorService.CloseDropDown();
            returnValue = Hue;
        }
    }
}
