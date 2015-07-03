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

using Ultima;
using System.Xml;

namespace Ultima.GDK.Gumps
{
    public enum ResizeType
    {
        None = 0,
        TopLeft,
        TopRight,
        BottomRight,
        BottomLeft,
        TopCenter,
        MiddleRight,
        BottomCenter,
        MiddleLeft,
    }

    public class BaseGump : IDisposable
    {
        public static bool SuspendInvalidation = false;

        private string name;
        private Hue hue;
        private int index;
        private int z;
        private bool selected;
        private bool moving;
        private bool resizing;
        private bool redraw;
        private Point location;
        private Size size;
        private Bitmap img;
        private Point lastMouseLoc;
        private Gump parent;
        private ResizeType resizeType;

        [Description("The name of the gump. This is used to create enum values for those gumps that used them.")]
        public string Name
        {
            get { return name; }
            set {

                if (name != value)
                {
                    OnBeforeBaseGumpChanged(this, EventArgs.Empty);
                }

                name = value;

                if (name != value)
                {
                    OnBaseGumpChanged(this, EventArgs.Empty);
                }
            }
        }

        [Browsable( true ), Editor(typeof(HueEditor), typeof(UITypeEditor))]
        public Hue Hue 
        { 
            get 
            {
                if (hue == null)
                {
                    hue = new Hue(0);
                }

                return hue;
            }
            set 
            {
                if (hue != value)
                {
                    OnBeforeBaseGumpChanged(this, EventArgs.Empty);
                }

                hue = value;

                if (hue != value)
                {
                    OnBaseGumpChanged(this, EventArgs.Empty);
                }

                Invalidate();
            } 
        }

        [Browsable( false )]
        public Gump Parent
        {
            get { return parent; }
            set 
            {
                if (parent != value)
                {
                    OnBeforeBaseGumpChanged(this, EventArgs.Empty);
                }

                parent = value;

                if (parent != value)
                {
                    OnBaseGumpChanged(this, EventArgs.Empty);
                }

                Invalidate(); 
            }
        }

        [Browsable( false )]
        public Rectangle[] CornerHandles
        {
            get
            {
                //Clockwise from top left corner
                return new Rectangle[] {
                    new Rectangle( location.X - 4, location.Y - 4, 4, 4 ),
                    new Rectangle( location.X + Width + 1, location.Y - 4, 4, 4 ),
                    new Rectangle( location.X + Width + 1, location.Y + Height + 1, 4, 4 ),
                    new Rectangle( location.X - 4, location.Y + Height + 1, 4, 4 )
                };
            }
        }

        [Browsable( false )]
        public Rectangle[] SideHandles
        {
            get
            {
                //Clockwise from top middle
                return new Rectangle[] {
                    new Rectangle( ( location.X + (Width / 2) ) - 1, location.Y - 4, 4, 4 ),
                    new Rectangle( ( location.X + Width ) + 1, ( location.Y + (Height / 2 ) ) - 1, 4, 4 ),
                    new Rectangle( ( location.X + (Width / 2) ) - 1, ( location.Y + Height ) + 1, 4, 4 ),
                    new Rectangle( location.X - 4, ( location.Y + (Height / 2 ) ) - 1, 4, 4 )
                };
            }
        }

        [Browsable( false )]
        public virtual int Width
        {
            get { return size.Width; }
            set
            {
                if (size.Width != value)
                {
                    OnBeforeBaseGumpChanged(this, EventArgs.Empty);
                }

                size.Width = value;

                if (size.Width != value)
                {
                    OnBaseGumpChanged(this, EventArgs.Empty);
                }

                Invalidate();
            }
        }
        
        [Browsable( false )]
        public virtual int Height
        {
            get { return size.Height; }
            set 
            {
                if (size.Height != value)
                {
                    OnBeforeBaseGumpChanged(this, EventArgs.Empty);
                }

                size.Height = value;

                if (size.Height != value)
                {
                    OnBaseGumpChanged(this, EventArgs.Empty);
                }

                Invalidate();
            }
        }

        [Browsable( false )]
        public Rectangle Bounds
        {
            get { return new Rectangle( location, size ); }
        }

        [Browsable( false )]
        public Size Size
        {
            get { return size; }
            set 
            {
                if (size != value)
                {
                    OnBeforeBaseGumpChanged(this, EventArgs.Empty);
                }

                size = value;

                if (size != value)
                {
                    OnBaseGumpChanged(this, EventArgs.Empty);
                }

                Invalidate();
            }
        }

        [Description("The location x,y at which the gump will be drawn.")]
        public Point Location
        {
            get { return location; }
            set
            {
                if (location != value)
                {
                    OnBeforeBaseGumpChanged(this, EventArgs.Empty);
                }

                location = value;

                if (location != value)
                {
                    OnBaseGumpChanged(this, EventArgs.Empty);
                }

                Invalidate();
            }
        }

        [Editor(typeof(GumpIndexEditor), typeof(UITypeEditor)), Description("The index in the gump's art file")]
        public virtual int Index
        {
            get { return index; }
            set
            {
                if (IsValidIndex(value))
                {
                    if (index != value)
                    {
                        OnBeforeBaseGumpChanged(this, EventArgs.Empty);
                    }

                    index = value;

                    if (index != value)
                    {
                        OnBaseGumpChanged(this, EventArgs.Empty);
                    }

                }
                
                if( img != null )
                {
                    img.Dispose();
                    img = null;
                }

                Invalidate();
            }
        }

        [Description("The Z position of the gump, this value is used for sorting draw order.")]
        public int Z
        {
			
            get { return z; }
            set 
            {
                if (z != value)
                {
                    OnBeforeBaseGumpChanged(this, EventArgs.Empty);
                }

                z = value;

                if (z != value)
                {
                    OnBaseGumpChanged(this, EventArgs.Empty);
                }

                Invalidate();
            }
        }

        [Browsable( false )]
        public virtual bool Selected
        {
            get { return selected; }
            set 
            {
                if (selected != value)
                {
                    OnBeforeBaseGumpChanged(this, EventArgs.Empty);
                }

                selected = value;

                if (selected != value)
                {
                    OnBaseGumpChanged(this, EventArgs.Empty);
                }

                Invalidate();
            }
        }

        [Browsable( false )]
        public virtual bool Resizable 
        { 
            get { return false; } 
        }

		[Browsable(false)]
		internal virtual Bitmap Image
		{
			get
            {
                if (img == null)
                {
                    img = this.GetImage();
                    redraw = false;
                }

                return img;
            }
			set
            {
                img = value;
                Invalidate();
            }
		}

        public event EventHandler<EventArgs> BaseGumpChanged;
        public event EventHandler<EventArgs> BeforeBaseGumpChanged;

		public BaseGump()
            : this( 0 )
        {
            Hue = new Hue(0);
        }

        public BaseGump( int index )
            : base()
        {
            Index = index;
        }

        protected virtual void OnBaseGumpChanged(object sender, EventArgs args)
        {
            if (BaseGumpChanged != null)
            {
                BaseGumpChanged(sender, args);
            }
        }

        protected virtual void OnBeforeBaseGumpChanged(object sender, EventArgs args)
        {
            if (BeforeBaseGumpChanged != null)
            {
                BeforeBaseGumpChanged(sender, args);
            }
        }

        public virtual void OnKeyDown( KeyEventArgs e )
        {
            if( Selected )
            {
                int toMove = 1;
                if( Control.ModifierKeys == Keys.Shift )
                    toMove += 9;

                switch( e.KeyCode )
                {
                    case Keys.Delete:
                        {
                            Parent.Items.Remove( this );
                            Dispose();
                            break;
                        }
                    case Keys.Up:
                        {
                            Location = new Point( Location.X, Location.Y - toMove );
                            break;
                        }
                    case Keys.Down:
                        {
                            Location = new Point( Location.X, Location.Y + toMove );
                            break;
                        }
                    case Keys.Left:
                        {
                            Location = new Point( Location.X - toMove, Location.Y );
                            break;
                        }
                    case Keys.Right:
                        {
                            Location = new Point( Location.X + toMove, Location.Y );
                            break;
                        }

                }
            }

        }

        public virtual void OnMouseMove( MouseEventArgs e )
        {
            Point point = e.Location;
            Point p = ConvertPointToImagePoint( e.Location );
            
            if( point != lastMouseLoc )
            {
                if( e.Button == MouseButtons.Left && Selected )
                {
                    int xDelta = point.X - lastMouseLoc.X;
                    int yDelta = point.Y - lastMouseLoc.Y;

					if( moving )
						Location = new Point( location.X + xDelta, location.Y + yDelta );
					else if( resizing )
					{
						int width;
						int height;

						if( resizeType == ResizeType.BottomRight )
						{
							width = Width + xDelta;
							height = Height + yDelta;

							if( width < 1 )
								width = 1;
							if( height < 1 )
								height = 1;

							Size = new Size(width, height);
						}

						redraw = true;
					}
                }
            }

            lastMouseLoc = point;
        }

        public virtual void OnMouseDown( MouseEventArgs e )
        {
            Point p = ConvertPointToImagePoint( e.Location );

            if( img == null )
                return;
			
			resizing = Resizing(e.X, e.Y);

			if(!Utility.MouseHitSuccess )
            {
                if( resizing || 
                    ( Bounds.Contains(e.Location) && img.GetPixel(p.X, p.Y) != Utility.EmptyColor ))
                {
                    if (!Selected && (Control.ModifierKeys & Keys.Control) != 0)
                    {
                        Parent.DeselectAll();
                    }

                    Selected = true;                    
                    Utility.MouseHitSuccess = true;

                    moving = !resizing;

				    if( moving )
					    Cursor.Current = Cursors.NoMove2D;
				    else if( resizing )
				    {
					    switch( resizeType )
					    {
						    case ResizeType.BottomCenter:
						    {
							    Cursor.Current = Cursors.PanSouth;
							    break;
						    }
						    case ResizeType.BottomLeft:
						    {
							    Cursor.Current = Cursors.PanSW;
							    break;
						    }
						    case ResizeType.BottomRight:
						    {
							    Cursor.Current = Cursors.PanSE;
							    break;
						    }
						    case ResizeType.MiddleLeft:
						    {
							    Cursor.Current = Cursors.PanWest;
							    break;
						    }
						    case ResizeType.MiddleRight:
						    {
							    Cursor.Current = Cursors.PanEast;
							    break;
						    }
						    case ResizeType.TopCenter:
						    {
							    Cursor.Current = Cursors.PanNorth;
							    break;
						    }
						    case ResizeType.TopLeft:
						    {
							    Cursor.Current = Cursors.PanNW;
							    break;
						    }
						    case ResizeType.TopRight:
						    {
							    Cursor.Current = Cursors.PanNE;
							    break;
						    }
					    }
				    }
				    else
					    Cursor.Current = Cursors.Default;

                    Invalidate();
                }
            }
        }

        public virtual void OnMouseUp( MouseEventArgs e )
        {
            moving = false;
            resizing = false;
        }

        public virtual void OnPaint( PaintEventArgs e )
        {
			if( Image == null || redraw )
			{
				if( Image != null )
					Image.Dispose();

				Image = GetImage();
				redraw = false;
			}
            
			e.Graphics.DrawImageUnscaled(Image, Bounds);
            DrawSelectionBox( e );
        }

        public virtual void DrawSelectionBox( PaintEventArgs e )
        {
            if( Selected )
            {
                Pen pen = Pens.WhiteSmoke;
                Brush brush = Brushes.WhiteSmoke;

                Rectangle rect = Bounds;

                rect.Inflate(2, 2);
				e.Graphics.DrawRectangle(Pens.Gainsboro, rect);

				rect.Inflate(1, 1);
                e.Graphics.DrawRectangle( pen, rect );

                if( Resizable )
                    for( int i = 0; i < 4; i++ )
                    {
                        e.Graphics.FillRectangle( brush, CornerHandles[i] );
                        e.Graphics.FillRectangle( brush, SideHandles[i] );
                    }
            }
        }

        public virtual bool IsValidIndex( int index )
        {
            return ( index < 65535 && Ultima.Gumps.FileIndex.Index[index].length > 0 );
        }

        public Point ConvertPointToImagePoint( Point point )
        {
            return new Point( point.X - location.X, point.Y - location.Y );
        }

        private bool Resizing( int x, int y )
        {
            if( !Resizable )
                return false;
            else
            {
                for( int i = 0; i < 4; i++ )
                {
                    if( CornerHandles[i].Contains( new Point( x, y ) ) )
                    {
                        resizeType = ( ResizeType )i + 1;
                        return true;
                    }
                    if( SideHandles[i].Contains( new Point( x, y ) ) )
                    {
                        resizeType = ( ResizeType )( i + 5 );
                        return true;
                    }
                }
            }

            return false;
        }

        public virtual Bitmap GetImage()
        {
            Bitmap image;

            if (hue == null)
            {
                hue = new Hue(0);
            }

            if( hue.Index == 0 )
                image = Ultima.Gumps.GetGump(index);
            else
                image = Ultima.Gumps.GetGump(index, hue, false);

            Size = image.Size;

            return image;
        }

        public virtual void Invalidate()
        {
            if (SuspendInvalidation)
            {
                return;
            }

            redraw = true;

            if( parent != null )
                parent.Invalidate();
        }

        public virtual void Dispose()
        {
            if( img != null )
                img.Dispose();         
        }

        public virtual BaseGump Clone()
        {
            BaseGump b = new BaseGump();
            b.Selected = Selected;
			b.Image = (Bitmap)img.Clone();
			b.Index = index;
			b.Location = location;
			b.Hue = hue;
			b.Parent = parent;
			b.Size = size;

            return b;
		}

        protected virtual BaseGump Clone(BaseGump b)
        {
            b.Image = (Bitmap)Image.Clone();
            b.Index = index;
            b.Location = location;
            b.Hue = hue;
            b.Parent = parent;
            b.Size = size;

            return b;
        }

        public string GetDesignerString()
        {
            string name = String.Format("<{0}>", Name);
            string type = Path.GetExtension(GetType().ToString());
            type = type.Substring(1, type.Length - 1);

            return String.Format("{0} {1}", type, name);
        }

        public void MoveBy(int xDelta, int yDelta)
        {
            location.X += xDelta;
            location.Y += yDelta;
        }

        public virtual void Deserialize(XmlReader reader)
        {
            hue =  Hues.GetHue(XmlConvert.ToInt32(reader.GetAttribute("hue")));
            index = XmlConvert.ToInt32(reader.GetAttribute("index"));
            z = XmlConvert.ToInt32(reader.GetAttribute("z"));
            location = new Point(XmlConvert.ToInt32(reader.GetAttribute("x")), XmlConvert.ToInt32(reader.GetAttribute("y")));
            size = new Size(XmlConvert.ToInt32(reader.GetAttribute("width")), XmlConvert.ToInt32(reader.GetAttribute("height")));
        }

        public virtual void Serialize(XmlWriter writer)
        {
            writer.WriteAttributeString("type", GetType().ToString());
            writer.WriteAttributeString("hue", hue.Index.ToString());
            writer.WriteAttributeString("index", index.ToString());
            writer.WriteAttributeString("z", z.ToString());
            writer.WriteAttributeString("x", location.X.ToString());
            writer.WriteAttributeString("y", location.Y.ToString());
            writer.WriteAttributeString("width", Width.ToString());
            writer.WriteAttributeString("height", Height.ToString());
        }
    }
}
