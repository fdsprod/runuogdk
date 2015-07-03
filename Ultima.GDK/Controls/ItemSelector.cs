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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Ultima.GDK
{
    public partial class ItemSelector : UserControl
    {
        private ArtType artType;
        private int itemWidth = 44;
        private int itemHeight = 44;
        private int columns;
        private int rows;

        [Browsable(false)]
        public int Index;
        [Browsable(true)]
        public ArtType ArtType { get { return artType; } set { artType = value; } }
        [Browsable(true)]
        public int ItemWidth { get { return itemWidth; } set { itemWidth = value; } }
        [Browsable(true)]
        public int ItemHeight { get { return itemHeight; } set { itemHeight = value; } }

        public ItemSelector()
        {
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
        }

        public int GetIndexAt(int x, int y)
        {
            return ((columns * (vScrollBar.Value - 1)) + (x + (y * columns)));
        }

        public Bitmap GetImageAt(int x, int y)
        {
            if (ArtType == ArtType.Item)
                return Art.GetStatic(GetIndexAt(x, y));
            else
                return Ultima.Gumps.GetGump(GetIndexAt(x, y));
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            MouseWheel += new MouseEventHandler(ItemSelector_MouseWheel);
        }

        private void ItemSelector_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                if (vScrollBar.Value < vScrollBar.Maximum)
                {
                    vScrollBar.Value++;
                }
            }
            else
            {
                if (vScrollBar.Value > 1)
                {
                    vScrollBar.Value--;
                }
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            columns = (Width - vScrollBar.Width) / itemWidth;
            rows = Height / itemHeight;

            int max = 0;
            
            if(this.ArtType == ArtType.Item)
                max = 0x3FFF / columns;
            else
                max = Ultima.Gumps.FileIndex.Index.Length / columns;

            if (512 % columns > 0)
                max++;

            vScrollBar.Maximum = max;
            vScrollBar.Minimum = 1;
            vScrollBar.SmallChange = 1;
            vScrollBar.LargeChange = rows;

            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);

            Bitmap bmp = new Bitmap((Width - vScrollBar.Width), Height);
            Graphics g = Graphics.FromImage(bmp);

            g.Clear(Color.White);

            for (int x = 0; x < columns + 1; x++)
            {
                g.DrawLine(Pens.Gray, new Point(x * itemWidth, 0),
                    new Point(x * itemWidth, rows * itemHeight));
            }

            for (int y = 0; y < rows + 1; y++)
            {
                g.DrawLine(Pens.Gray, new Point(0, y * itemHeight),
                    new Point(columns * itemWidth, y * itemHeight));
            }

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    Bitmap b = GetImageAt(x, y);

                    if (b != null)
                    {
                        Point loc = new Point((x * itemWidth) + 1, (y * itemHeight) + 1);
                        Size size = new Size(itemHeight - 1, itemWidth - 1);
                        Rectangle rect = new Rectangle(loc, size);

                        g.Clip = new Region(rect);

                        Point m = PointToClient(Control.MousePosition);

                        if (rect.Contains(m))
                        {
                            g.FillRectangle(Brushes.LightSteelBlue, rect);
                        }

                        if (b.Width < size.Width && b.Height < size.Height)
                        {
                            loc = new Point(loc.X + ((size.Width - b.Width) / 2),
                                loc.Y + ((size.Height - b.Height) / 2));
                        }

                        if (ArtType == ArtType.Item)
                        {
                            g.DrawImage(b, loc);
                        }
                        else
                        {
                            int width = b.Width;
                            int height = b.Height;

                            if (b.Height > itemHeight)
                            {
                                height = itemHeight;
                            }

                            if (b.Width > itemWidth)
                            {
                                width = itemWidth;
                            }

                            g.DrawImage(b,
                                new Rectangle(loc, new Size(width, height)));
                        }
                    }
                    else
                    {

                    }
                }
            }

            pe.Graphics.DrawImage(bmp, new Point(0, 0));
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            Index = GetIndexAt(e.X / itemWidth, e.Y / itemHeight);
            base.OnMouseDoubleClick(e);
        }

        private Point mouseLocation;

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            Point loc = new Point(e.X / itemWidth, e.Y / itemHeight);

            if (mouseLocation != loc)
            {
                mouseLocation = loc;
                Invalidate();
            }
        }

        private void vScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            Invalidate();
        }
    }
}
