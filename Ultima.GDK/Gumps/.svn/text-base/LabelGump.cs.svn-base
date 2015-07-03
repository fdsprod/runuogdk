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
using System.Text;
using System.Drawing;
using System.Drawing.Design;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Windows.Forms.VisualStyles;

using Ultima;

namespace Ultima.GDK.Gumps
{
	public class LabelGump : BaseGump
	{
		[Browsable(false)]
		public override int Index
		{
			get
			{
				return base.Index;
			}
			set
			{
				base.Index = value;
			}
		}

		private string label;
		private int font;


        [Description("Gets or sets the text of the gump")]
		public string Label
		{
			get { return label; }
			set
			{
				label = value;

				if( Image != null )
				{
					Image.Dispose();
					Image = null;
				}

				Invalidate();
			}
		}
        
        [Description("Gets or sets the font of the gump.\nValid entries are 1 through 3")]
		public int Font
		{
			get 
            { 
                if(font < 1)
                {
                    return 1; 
                }
                else if (font > 3)
                {
                    return 3;
                }
                else 
                {
                    return font;
                }
            }
			set
			{
				if( value < 1 || value > 3 )
					return;

				font = value;

				if( Image != null )
				{
					Image.Dispose();
					Image = null;
				}

				Invalidate();
			}
		}

		public LabelGump()
			: base()
		{
			Label = "Label";
			Font = 1;
		}

        public override BaseGump Clone()
        {
            LabelGump b = new LabelGump();

            return base.Clone(b);
        }

		public override Bitmap GetImage()
		{
			Bitmap bmp = UOFont.UnicodeFonts.GetStringImage(Font, Label);			
			Size = bmp.Size;
			return bmp;
		}

        //AddLabel( int x, int y, int hue, string text )	
        public override string ToString()
        {
            return String.Format("\t\tAddLabel({0}, {1}, {2}, \"{3}\");", Location.X, Location.Y, Hue.Index, Label);
        }
	}
}
