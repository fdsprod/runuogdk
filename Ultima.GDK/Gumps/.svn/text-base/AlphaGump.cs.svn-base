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

namespace Ultima.GDK.Gumps 
{
    public class AlphaGump : BaseGump
    {
		private Brush brush;

		public override bool Resizable
		{
			get
			{
				return true;
			}
		}

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

        [Browsable(true)]
        public override int Height
        {
            get
            {
                return base.Height;
            }
            set
            {
                base.Height = value;
            }
        }

        [Browsable(true)]
        public override int Width
        {
            get
            {
                return base.Width;
            }
            set
            {
                base.Width = value;
            }
        }

        [Browsable(false)]
        public new Hue Hue { get { return new Hue(0); } set { value = value; } }

        public AlphaGump() : base()
		{
			Size = new Size(100, 25);
			brush = new SolidBrush(Color.FromArgb(30, 255, 0, 0));
		}

		public override Bitmap GetImage()
		{
            if (Size.Height < 1)
            {
                Height = 1;
            }

            if (Size.Width < 1)
            {
                Width = 1;
            }

			Bitmap bmp = new Bitmap(Size.Width, Size.Height);

			Graphics g = Graphics.FromImage(bmp);
			Size = bmp.Size;

			g.FillRectangle(brush, new Rectangle( 0, 0, Width, Height ));
			g.DrawRectangle(Pens.Red, new Rectangle(0, 0, Width - 1, Height - 1));

			g.Dispose();

			return bmp;
		}

        public override BaseGump Clone()
        {
            AlphaGump b = new AlphaGump();

            return base.Clone(b);
        }

		//public void AddAlphaRegion( int x, int y, int width, int height )

		public override string ToString()
		{
			return String.Format("\t\tAddAlphaRegion( {0}, {1}, {2}, {3} );", Location.X, Location.Y, Width, Height );
		}
    }
}
