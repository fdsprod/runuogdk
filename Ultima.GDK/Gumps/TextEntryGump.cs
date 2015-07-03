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
using System.Xml;

namespace Ultima.GDK.Gumps
{
    public class TextEntryGump : BaseGump
    {
		private Brush brush;
        private int entryId;
        private string text;

        [Description("Gets or sets the initial text of the gump")]
        public string Text { get { return text; } set { text = value; } }

        [Description("Gets or sets the id to be used in the OnResponse method")]
        public int EntryId { get { return entryId; } set { entryId = value; } }

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

        public TextEntryGump() : base()
		{
			Size = new Size(100, 25);
			brush = new SolidBrush(Color.FromArgb(30, 0, 0, 255));
		}

        public override BaseGump Clone()
        {
            TextEntryGump b = new TextEntryGump();

            return base.Clone(b);
        }

		public override Bitmap GetImage()
		{
			Bitmap bmp = new Bitmap(Size.Width, Size.Height);
			Graphics g = Graphics.FromImage(bmp);
			Size = bmp.Size;

			g.FillRectangle(brush, new Rectangle( 0, 0, Width, Height ));
			g.DrawRectangle(Pens.Blue, new Rectangle(0, 0, Width - 1, Height - 1));

			g.Dispose();
			return bmp;
        }

        public override void Deserialize(XmlReader reader)
        {
            base.Deserialize(reader);

            entryId = XmlConvert.ToInt32(reader.GetAttribute("entryId"));
            text = reader.GetAttribute("text");
        }

        public override void Serialize(XmlWriter writer)
        {
            base.Serialize(writer);

            writer.WriteAttributeString("entryId", entryId.ToString());
            writer.WriteAttributeString("text", text);
        }

		//public void AddTextEntry( int x, int y, int width, int height, int hue, int entryID, string initialText )
		//public void AddTextEntry( int x, int y, int width, int height, int hue, int entryID, string initialText, int size )
        public override string ToString()
        {
            return String.Format("\t\tAddTextEntry({0}, {1}, {2}, {3}, {4}, {5}, {6});", Location.X, Location.Y, Width, Height, 0, "(int)ButtonTypes." + Name, Text);
        }
    }
}
 
