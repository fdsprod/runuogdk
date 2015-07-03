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
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.ComponentModel;

namespace Ultima.GDK.Gumps
{
    public class BackgroundGump : BaseGump
    {
		private Bitmap[] images;

		public override int Index
		{
			get
			{
				return base.Index;
			}
			set
			{
				if( ValidImageRow(value) )
				{
					base.Index = value;

					if( images == null )
						images = new Bitmap[9];

					for( int i = 0; i < 9; i++ )
					{
						if( images[i] != null )
							images[i].Dispose();

                        images[i] = Ultima.Gumps.GetGump(value + i);											
					}
				}
				else
					MessageBox.Show("That is not a valid starting image,\n Please use the top left corner of the background as the Starting index.", "Invalid Index");
			}
		}

        [Browsable(true), Description("The width of the gump")]
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

        [Browsable(true), Description("The height of the gump")]
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

		public override Bitmap GetImage()
        {
            if (Size.Width < 1)
            {
                Size = new Size(1, Size.Height);
            }

            if (Size.Height < 1)
            {
                Size = new Size(Size.Width, 1);
            }

			Bitmap bmp = new Bitmap(Size.Width, Size.Height);
			Graphics g = Graphics.FromImage(bmp);
			Size = bmp.Size;

			int middleWidth = ( bmp.Width - images[0].Width ) - images[2].Width;
			int middleHeight = ( bmp.Height - images[0].Height ) - images[6].Height;

			double dtileX = (double)middleWidth / (double)images[4].Width;
			double dtileY = middleHeight / (double)images[4].Height;

			int tileX = (int)dtileX;
			int tileY = (int)dtileY;

			if( dtileX - tileX > 0 )
				tileX++;
			if( dtileY - tileY > 0 )
				tileY++;

			if( tileX < 1 )
				tileX = 1;

			if( tileY < 1 )
				tileY = 1;

			//Fill Center
			for( int x = 0; x < tileX; x++ )
			{
				for( int y = 0; y < tileY; y++ )
				{
					int posX = images[0].Width;
					int posY = images[0].Height;

					int xOffset = images[4].Width * x;
					int yOffset = images[4].Height * y;

					int maxWidth = Math.Min(middleWidth - xOffset, images[4].Width);
					int maxHeight = Math.Min(middleHeight - yOffset, images[4].Height);

					g.DrawImageUnscaled(images[4], new Rectangle(posX + xOffset, posY + yOffset, maxWidth, maxHeight));
				}
			}

			dtileX = (double)middleWidth / (double)images[1].Width;
			dtileY = middleHeight / (double)images[3].Height;

			tileX = (int)dtileX;
			tileY = (int)dtileY;

			if( dtileX - tileX > 0 )
				tileX++;
			if( dtileY - tileY > 0 )
				tileY++;

			if( tileX < 1 )
				tileX = 1;

			if( tileY < 1 )
				tileY = 1;

			//Draw Top and Bottom
			for( int i = 0; i < tileX; i++ )
			{
				int posX = images[0].Width;
				int xOffset = images[1].Width * i;

				int maxWidth = Math.Min(middleWidth - xOffset, images[1].Width);

				Rectangle top = new Rectangle(posX + xOffset, 0, maxWidth, images[1].Height);
				Rectangle bottom =  new Rectangle(posX + xOffset, Height - images[7].Height, maxWidth, images[7].Height);
				g.DrawImageUnscaled(images[1], top);
				g.DrawImageUnscaled(images[7], bottom);
			}

			//Draw Left and Right
			for( int i = 0; i < tileY; i++ )
			{
				int posY = images[0].Height;
				int yOffset = images[3].Height * i;

				int maxHeight = Math.Min(middleHeight - yOffset, images[3].Height);

				Rectangle left = new Rectangle(0, posY + yOffset, images[3].Width, maxHeight);
				Rectangle right = new Rectangle(bmp.Width - images[5].Width, posY + yOffset, images[5].Width, maxHeight);

				g.DrawImageUnscaled(images[3], left);
				g.DrawImageUnscaled(images[5], right);
			}			

			//Top Left Corner
			g.FillRectangle(Brushes.Black, new Rectangle(0, 0, images[0].Width, images[0].Height));
			g.DrawImageUnscaled(images[0], new Point(0, 0));
			//Top Right Corner 
			g.FillRectangle(Brushes.Black, new Rectangle(bmp.Width - images[2].Width, 0, images[2].Width, images[2].Height));
			g.DrawImageUnscaled(images[2], new Point(bmp.Width - images[2].Width, 0));
			//Bottom Left Corner
			g.FillRectangle(Brushes.Black, new Rectangle(0, bmp.Height - images[6].Height, images[6].Width, images[6].Height));
			g.DrawImageUnscaled(images[6], new Point(0, bmp.Height - images[6].Height));
			//Bottom Right Corner
			g.FillRectangle(Brushes.Black, new Rectangle(bmp.Width - images[8].Width, bmp.Height - images[8].Height, images[8].Width, images[8].Height));
			g.DrawImageUnscaled(images[8], new Point(bmp.Width - images[8].Width, bmp.Height - images[8].Height));

			g.Dispose();
			return bmp;
		}
		
		public override bool Resizable { get { return true; } }

		public override BaseGump Clone()
		{
            BackgroundGump b = new BackgroundGump();

			b.images = new Bitmap[9];

			for( int i = 0; i < images.Length; i++ )
				b.images[i] = (Bitmap)images[i].Clone();

			return base.Clone(b);
		}

        protected override BaseGump Clone(BaseGump b)
        {
            if (((BackgroundGump)b).images != null)
            {
                for (int i = 0; i < ((BackgroundGump)b).images.Length; i++)
                    ((BackgroundGump)b).images[i].Dispose();
            }

            ((BackgroundGump)b).images = new Bitmap[9];

            for (int i = 0; i < images.Length; i++)
                ((BackgroundGump)b).images[i] = (Bitmap)images[i].Clone();

            return base.Clone(b);
        }

		private bool ValidImageRow(int value)
		{
			bool valid = true;

            for (int i = value; i < value + 9; i++)
            {
                if (!Ultima.Gumps.IsValidIndex(i))
                {
                    valid = false;
                    break;
                }
            }

			return valid;
		}

		public BackgroundGump()
			: base(9200) 
		{
			Size = new Size(100, 100);
            Index = 9200;
		}

		//public void AddBackground( int x, int y, int width, int height, int gumpID )

		public override string ToString()
		{
			return String.Format("\t\tAddBackground( {0}, {1}, {2}, {3}, {4} );", Location.X, Location.Y, Width, Height, Index );
		}
    }
}
