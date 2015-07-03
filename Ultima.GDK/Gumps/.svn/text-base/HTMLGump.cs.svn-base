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
using System.IO;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Design;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Windows.Forms.VisualStyles;
using System.Xml;

namespace Ultima.GDK.Gumps
{
	public enum TextType
	{
		HTML,
		CLILOC
	}

	public class HTMLGump : BaseGump
	{
		private TextType textType;
		private string text;
		private int cliloc;
		private bool showBackground;
		private bool showScrollbar;
		private Bitmap[] images;
		private Bitmap[] scrollBarImages;

        [Description("Gets or sets the value to show or not show the scroll bar")]
		public bool ShowScrollbar
		{
			get { return showScrollbar; }
			set
			{
				showScrollbar = value;

				if( Image != null )
				{
					Image.Dispose();
					Image = null;
				}

				Invalidate();
			}
		}

        [Description("Gets or sets the value to show or not show the background")]
		public bool ShowBackground
		{
			get { return showBackground; }
			set
			{
				showBackground = value;

				if( Image != null )
				{
					Image.Dispose();
					Image = null;
				}

				Invalidate();
			}
		}


        [Description("Gets or sets the index value of the cliloc to use for the gump")]
		public int CliLoc
		{
			get { return cliloc; }
			set
			{
				cliloc = value;

				if( TextType == TextType.CLILOC && Image != null )
				{
					Image.Dispose();
					Image = null;
				}

				Invalidate();
			}
		}

        [Description("Gets or sets the value to use a cliloc or text")]
		public TextType TextType
		{
			get { return textType; }
			set
			{
				textType = value;

				if( Image != null )
				{
					Image.Dispose();
					Image = null;
				}

				Invalidate();
			}
		}

        [Description("Gets or sets the text to show on the gump")]
		public string Text
		{
			get { return text; }
			set
			{
				text = value;

				if( TextType == TextType.HTML && Image != null )
				{
					Image.Dispose();
					Image = null;
				}

				Invalidate();
			}
		}

		public HTMLGump()
			: base()
		{
			Size = new Size(100, 100);

			showBackground = true;
			showScrollbar = true;

			images = new Bitmap[9];

			for( int i = 0; i < images.Length; i++ )
                images[i] = Ultima.Gumps.GetGump(9350 + i);

			scrollBarImages = new Bitmap[4];

			scrollBarImages[0] = Ultima.Gumps.GetGump(256);//Bar
            scrollBarImages[1] = Ultima.Gumps.GetGump(250);//Up
			scrollBarImages[2] = Ultima.Gumps.GetGump(252);//Down
            scrollBarImages[3] = Ultima.Gumps.GetGump(254);//Scroller
		}

		public override bool Resizable{	get	{ return true; } }

        public override BaseGump Clone()
        {
            HTMLGump b = new HTMLGump();

            return Clone(b);
        }

        protected override BaseGump Clone(BaseGump b)
        {
            ((HTMLGump)b).TextType = textType;
            ((HTMLGump)b).Text = text;
            ((HTMLGump)b).CliLoc = cliloc;
            ((HTMLGump)b).ShowBackground = showBackground;
            ((HTMLGump)b).ShowScrollbar = showScrollbar;
            //((HTMLGump)b).scrollBarImages = scrollBarImages;

            return base.Clone(b);
        }

		public override Bitmap GetImage()
		{
			Bitmap bmp = new Bitmap(Size.Width, Size.Height);
			Graphics g = Graphics.FromImage(bmp);
			Size = bmp.Size;
			
			int widthOffset = 0;

			if( showScrollbar )
				widthOffset = scrollBarImages[1].Width + 2;

			if( Size.Width - widthOffset < 0 )
				widthOffset = Size.Width - 1;

			Bitmap panel = new Bitmap(Size.Width - widthOffset, Size.Height);
			Graphics pg = Graphics.FromImage(panel);

			int middleWidth = ( ( panel.Width - images[0].Width ) - images[2].Width ) - widthOffset;
			int middleHeight = ( panel.Height - images[0].Height ) - images[6].Height;

			double dtileX = (double)middleWidth / (double)images[4].Width;
			double dtileY = middleHeight / (double)images[4].Height;

			int tileX = (int)dtileX;
			int tileY = (int)dtileY;

			//Render Background
			if( showBackground )
			{
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

						pg.DrawImageUnscaled(images[4], new Rectangle(posX + xOffset, posY + yOffset, maxWidth, maxHeight));
					}
				}

				dtileX = (double)middleWidth / (double)images[1].Width- widthOffset;
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
					Rectangle bottom = new Rectangle(posX + xOffset, Height - images[7].Height, maxWidth, images[7].Height);
					pg.DrawImageUnscaled(images[1], top);
					pg.DrawImageUnscaled(images[7], bottom);
				}

				//Draw Left and Right
				for( int i = 0; i < tileY; i++ )
				{
					int posY = images[0].Height;
					int yOffset = images[3].Height * i;

					int maxHeight = Math.Min(middleHeight - yOffset, images[3].Height);

					Rectangle left = new Rectangle(0, posY + yOffset, images[3].Width, maxHeight);
					Rectangle right = new Rectangle(panel.Width - images[5].Width, posY + yOffset, images[5].Width, maxHeight);

					pg.DrawImageUnscaled(images[3], left);
					pg.DrawImageUnscaled(images[5], right);
				}

				//Top Left Corner
				pg.FillRectangle(Brushes.Black, new Rectangle(0, 0, images[0].Width, images[0].Height));
				pg.DrawImageUnscaled(images[0], new Point(0, 0));
				//Top Right Corner 
				pg.FillRectangle(Brushes.Black, new Rectangle(panel.Width - images[2].Width, 0, images[2].Width, images[2].Height));
				pg.DrawImageUnscaled(images[2], new Point(panel.Width - images[2].Width, 0));
				//Bottom Left Corner
				pg.FillRectangle(Brushes.Black, new Rectangle(0, panel.Height - images[6].Height, images[6].Width, images[6].Height));
				pg.DrawImageUnscaled(images[6], new Point(0, panel.Height - images[6].Height));
				//Bottom Right Corner
				pg.FillRectangle(Brushes.Black, new Rectangle(panel.Width - images[8].Width, panel.Height - images[8].Height, images[8].Width, images[8].Height));
				pg.DrawImageUnscaled(images[8], new Point(panel.Width - images[8].Width, panel.Height - images[8].Height));
			}
			else
			{
				//Fill Bounds for visability.
				using( SolidBrush brush = new SolidBrush(Color.FromArgb(50, 255, 240, 0)) )
				{
					pg.FillRectangle(brush, new Rectangle(0, 0, panel.Width, panel.Height));
					brush.Dispose();
				}

				pg.DrawRectangle(Pens.Yellow, new Rectangle(0, 0, panel.Width - 1, panel.Height - 1));
			} 
			
			//string text = string.Empty;

			if( textType == TextType.CLILOC )
                this.text = Utility.GetCliloc(cliloc);
			else
                this.text = text;

			if(!string.IsNullOrEmpty(text))
				pg.DrawString(text, Utility.Font, Brushes.Blue, new RectangleF(new PointF(5, 5), new SizeF(bmp.Width - ( showScrollbar ? scrollBarImages[1].Width : 0 ), bmp.Height - 5)));

			g.DrawImageUnscaled(panel, new Point(0, 0));
			panel.Dispose();
			pg.Dispose();

			//Render Scrollbar
			if( showScrollbar )
			{
				Rectangle rect = new Rectangle(bmp.Width - scrollBarImages[0].Width, 0, scrollBarImages[0].Width, bmp.Height);

   			    g.DrawImage(scrollBarImages[0], rect);
				g.DrawImageUnscaled(scrollBarImages[1], new Point(bmp.Width - scrollBarImages[1].Width, 0));
				g.DrawImageUnscaled(scrollBarImages[2], new Point(bmp.Width - scrollBarImages[2].Width, bmp.Height - scrollBarImages[2].Height));
				g.DrawImageUnscaled(scrollBarImages[3], new Point(bmp.Width - scrollBarImages[3].Width - 1, scrollBarImages[1].Height));
			}			
			
			g.Dispose();
			return bmp;
		}

        public override void Dispose()
        {
            base.Dispose();

            for (int i = 0; i < images.Length; i++)
            {
                images[i].Dispose();
            }
        }

        public override void Serialize(XmlWriter writer)
        {
            base.Serialize(writer);

            writer.WriteAttributeString("textType", ((int)textType).ToString());
            writer.WriteAttributeString("text", text);
            writer.WriteAttributeString("cliloc", cliloc.ToString());
            writer.WriteAttributeString("showBackground", showBackground.ToString());
            writer.WriteAttributeString("showScrollbar", showScrollbar.ToString());
        }

        public override void Deserialize(XmlReader reader)
        {
            base.Deserialize(reader);

            textType = (TextType)XmlConvert.ToInt32(reader.GetAttribute("textType"));
            text = reader.GetAttribute("text");
            cliloc = XmlConvert.ToInt32(reader.GetAttribute("cliloc"));
            showBackground = XmlConvert.ToBoolean(reader.GetAttribute("showBackground"));
            showScrollbar = XmlConvert.ToBoolean(reader.GetAttribute("showScrollbar"));
		}

		//public void AddHtml( int x, int y, int width, int height, string text, bool background, bool scrollbar )
		//public void AddHtmlLocalized( int x, int y, int width, int height, int number, bool background, bool scrollbar )
		public override string ToString()
		{
			if( textType == TextType.CLILOC )
                return String.Format("\t\tAddHtmlLocalized({0}, {1}, {2}, {3}, {4}, {5}, {6});", 
                    Location.X, Location.Y, Height, Width, cliloc, showBackground.ToString().ToLower(), 
                    showScrollbar.ToString().ToLower());
			else
                return String.Format("\t\tAddHtml({0}, {1}, {2}, {3}, {4}, {5}, {6});",
                    Location.X, Location.Y, Height, Width, "\"" + text + "\"", 
                    showBackground.ToString().ToLower(), showScrollbar.ToString().ToLower());

		}
	}
}
