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
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Design;

namespace Ultima.GDK.Gumps
{
	public class ItemGump : BaseGump
    {
        [Editor(typeof(ItemIndexEditor), typeof(UITypeEditor)), Description("The index in the gump's art file")]
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

        public ItemGump()
            : base()
        {
            Index = 0;
        }

        public override bool IsValidIndex(int index)
        {
            return (index < 65535 && Ultima.Art.FileIndex.Index[index].length > 0);
        }

        public override Bitmap GetImage()
        {
            Bitmap image = (Bitmap)Ultima.Art.GetStatic(Index).Clone();
            Size = image.Size;

            return image;
        }

        public override BaseGump Clone()
        {
            ItemGump b = new ItemGump();

            return base.Clone(b);
        }

		//public void AddItem( int x, int y, int itemID )
		//public void AddItem( int x, int y, int itemID, int hue )
        public override string ToString()
        {
            return String.Format("\t\tAddItem({0}, {1}, {2}, {3});", Location.X, Location.Y, Index, Hue);
        }
	}
}
