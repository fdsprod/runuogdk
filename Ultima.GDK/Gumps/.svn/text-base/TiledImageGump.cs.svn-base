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
	public class TiledImageGump : BaseGump
	{
		public override bool Resizable { get { return true;	} }

		public TiledImageGump()
			: base()
		{
            Size = new Size(100, 100);
            Index = 1;
        }

        public override BaseGump Clone()
        {
            TiledImageGump b = new TiledImageGump();

            return base.Clone(b);
        }

		public override System.Drawing.Bitmap GetImage()
		{
			Bitmap bmp = new Bitmap(Width, Height);
			Graphics g = Graphics.FromImage(bmp);

			using( TextureBrush brush = new TextureBrush(Ultima.Gumps.GetGump(Index)) )
			{
				g.FillRectangle(brush, new Rectangle(0, 0, Width, Height));
				brush.Dispose();
			}

			g.Dispose();
			return bmp;
		}

		//public void AddImageTiled( int x, int y, int width, int height, int gumpID )
        public override string ToString()
        {
            return String.Format("\t\tAddImageTiled({0}, {1}, {2}, {3}, {4});", Location.X, Location.Y, Width, Height, Index);
        }		
	}
}