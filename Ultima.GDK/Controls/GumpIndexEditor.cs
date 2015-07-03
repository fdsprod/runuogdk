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
using System.Windows.Forms.Design;
using System.Drawing.Design;
using System.Drawing;
using System.Security;
using System.ComponentModel;
using System.Windows.Forms;

namespace Ultima.GDK
{
    public class GumpIndexEditor : UITypeEditor
    {
        protected IWindowsFormsEditorService editorService;
        protected int returnValue;

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            editorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

            if (editorService != null)
            {
                return ProcessDialog(value, editorService);
            }

            return value;
        }


        private object ProcessDialog(object value, IWindowsFormsEditorService editorService)
        {
 	        GumpArtSelectorDialog dialog = new GumpArtSelectorDialog();
            dialog.Index = Convert.ToInt32(value);

            if (editorService.ShowDialog(dialog) == DialogResult.OK)
            {
                if (!Ultima.Gumps.IsValidIndex(dialog.Index))
                {
                    MessageBox.Show("You may not select invalid images.", "RunUO: GDK");
                    dialog.Dispose();
                    return ProcessDialog(value, editorService);
                }

                returnValue = dialog.Index;

                return returnValue;
            }

            return value;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
    }
}
