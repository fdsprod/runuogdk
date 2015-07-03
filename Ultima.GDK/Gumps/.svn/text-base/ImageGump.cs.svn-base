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

namespace Ultima.GDK.Gumps
{
    public class ImageGump : BaseGump
    {
        public ImageGump() : this( 0 ) { }
        public ImageGump( int index ) : base( index ) { }

        public override BaseGump Clone()
        {
            ImageGump b = new ImageGump();

            return base.Clone(b);
        }
		//public void AddImage( int x, int y, int gumpID )
		//public void AddImage( int x, int y, int gumpID, int hue )	

		public override string ToString()
		{
			return string.Format("\t\tAddImage( {0}, {1}, {2}{3} );", Location.X, Location.Y, Index, Hue.Index == 0 ? "" : ", " + Hue.ToString());
		}
    }
}
