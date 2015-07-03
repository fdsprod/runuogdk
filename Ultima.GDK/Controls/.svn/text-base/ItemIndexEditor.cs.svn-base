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
    public class ItemIndexEditor : UITypeEditor
    {
        protected IWindowsFormsEditorService editorService;
        protected int returnValue;

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            editorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

            if (editorService != null)
            {
                ItemArtSelectorDialog dialog = new ItemArtSelectorDialog();

                dialog.Index = Convert.ToInt32(value);

                if (editorService.ShowDialog(dialog) == DialogResult.OK)
                {                
                    returnValue = dialog.Index;
                    dialog.Dispose();

                    return returnValue;
                }

                dialog.Dispose();
            }
            
            return value;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
    }
}
