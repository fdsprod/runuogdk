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
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Collections.Generic;

using Ultima.GDK.Gumps;

namespace Ultima.GDK
{
	public class DesignerFrame : Panel
	{
        private Gump gump;
        private UndoStack undoStack;
        private RedoStack redoStack;
        private Point mouseDownAt = Point.Empty;
        private Point mouseAt = Point.Empty;
        private Point moveTest;
        private List<BaseGump> movable;

		protected static SolidBrush SelectionBrush = new SolidBrush(Color.FromArgb(50, Color.LightSteelBlue));

        public Gump Gump
        { 
            get { return gump; }
            set
            {
                if (gump != null)
                {
                    gump.Items.ItemAdded -= new EventHandler<GumpCollectionEventArgs>(Items_ItemAdded);
                    gump.Items.ItemRemoved -= new EventHandler<GumpCollectionEventArgs>(Items_ItemRemoved);
                }

                gump = value;

                if (gump != null)
                {
                    gump.Items.ItemAdded += new EventHandler<GumpCollectionEventArgs>(Items_ItemAdded);
                    gump.Items.ItemRemoved += new EventHandler<GumpCollectionEventArgs>(Items_ItemRemoved);
                }

                OnGumpChanged(this, new EventArgs());
            }
        }

        public event EventHandler<EventArgs> GumpChanged;
        public event EventHandler<EventArgs> BaseGumpMoved;
        public event EventHandler<EventArgs> BaseGumpChanged;
        public event EventHandler<EventArgs> BeforeBaseGumpMoved;
        public event EventHandler<EventArgs> BeforeBaseGumpChanged;
        public event EventHandler<BaseGumpsSelectedEventArgs> BaseGumpsSelected;
        public event EventHandler<EventArgs> Copied;
        public event EventHandler<EventArgs> Pasted;

		public DesignerFrame()
			: base()
		{
			SetStyle(ControlStyles.AllPaintingInWmPaint | 
                ControlStyles.OptimizedDoubleBuffer | 
                ControlStyles.ResizeRedraw | 
                ControlStyles.UserMouse |
                ControlStyles.UserPaint, true);

            undoStack = new UndoStack();
            redoStack = new RedoStack();
		}

        private void Items_ItemRemoved(object sender, GumpCollectionEventArgs e)
        {
            redoStack.Clear();

            e.Item.BaseGumpChanged -= new EventHandler<EventArgs>(OnBaseGumpChanged);

            Invalidate();
        }

        private void Items_ItemAdded(object sender, GumpCollectionEventArgs e)
        {
            redoStack.Clear();

            e.Item.BaseGumpChanged += new EventHandler<EventArgs>(OnBaseGumpChanged);
            e.Item.Parent = Gump;
            
            Invalidate();
        }

        protected virtual void OnBeforeBaseGumpChanged(object sender, EventArgs e)
        {
            if (BeforeBaseGumpChanged != null)
            {
                BeforeBaseGumpChanged(sender, e);
            }
        }

        protected virtual void OnBaseGumpChanged(object sender, EventArgs e)
        {
            if (BaseGumpChanged != null)
            {
                BaseGumpChanged(sender, e);
            }
        }

        protected virtual void OnBeforeBaseGumpMoved(object sender, EventArgs e)
        {
            if (BeforeBaseGumpMoved != null)
            {
                BeforeBaseGumpMoved(sender, e);
            }
        }

        protected virtual void OnGumpChanged(object sender, EventArgs args)
        {
            if (Gump != null)
            {
                undoStack.Push(new GumpState(Gump.Items));
            }

            if (GumpChanged != null)
            {
                GumpChanged(sender, args);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (gump == null)
            {
                return;
            }

            Size size = Gump.GetResolution();
            Bitmap image = new Bitmap(size.Width, size.Height);
            Graphics g = Graphics.FromImage(image);

            gump.OnPaint(new PaintEventArgs(g, e.ClipRectangle));

            if (mouseDownAt != Point.Empty)
            {
                Rectangle rect = Utility.BuildRectangle(mouseDownAt, mouseAt);

                if (gump.Items.Count < 15)
                    g.FillRectangle(SelectionBrush, rect);

                g.DrawRectangle(Pens.LightSteelBlue, rect);
            }

            Point drawPoint = new Point(0, 0);

            if (Width > size.Width)
            {
                drawPoint.X = Width / 2 - size.Width / 2;
            }

            if (Height > size.Height)
            {
                drawPoint.Y = Height / 2 - size.Height / 2;
            }

            e.Graphics.DrawImage(image, new Point(0,0));//drawPoint);

            image.Dispose();
            g.Dispose();
        }

        private ResizeType resizeType;
        private bool resizing;

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			Focus();
            Parent.Focus();

            if (Gump == null)
            {
                return;
            }

            redoStack.Clear();
            undoStack.Push(new GumpState(Gump.Items));

			Utility.MouseHitSuccess = false;

			if( e.Button == MouseButtons.Left )
			{
                if ((resizing = gump.CheckResizing(e.Location, out resizeType, gump.GetSelectedGumps())))
                {
                    switch (resizeType)
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
                {
                    Cursor.Current = Cursors.Default;
                    BaseGump clicked = gump.GetFirstGumpAt(e.Location);
                    moveTest = e.Location;

                    if (clicked != null)
                    {
                        if (!clicked.Selected)
                        {
                            if ((Control.ModifierKeys & Keys.Control) == 0 && gump != null)
                            {
                                gump.DeselectAll();
                            }

                            clicked.Selected = true;

                            movable = new List<BaseGump>();
                            movable.Add(clicked);
                        }
                        else
                        {
                            if (gump != null)
                            {
                                movable = gump.GetSelectedGumps();
                            }
                        }
                    }
                    else
                    {
                        if (gump != null)
                        {
                            gump.DeselectAll();
                        }

                        mouseDownAt = e.Location;
                    }
                }

                OnBaseGumpsSelected(this, new BaseGumpsSelectedEventArgs(gump));
                Invalidate();
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
            
            Focus();

            if (Gump == null)
            {
                return;
            }

            if (e.Button == MouseButtons.Left)
            {
                redoStack.Clear();
                if (mouseDownAt != Point.Empty)
                {
                    mouseAt = e.Location;

                    Rectangle rect = Utility.BuildRectangle(mouseDownAt, mouseAt);

                    if (gump != null)
                    {
                        for (int i = 0; i < gump.Items.Count; i++)
                        {
                            gump.Items[i].Selected = gump.Items[i].Bounds.IntersectsWith(rect);
                        }                                            
                    }
                }
                else
                {
                    movable = null;
                }
            }
            
            mouseDownAt = Point.Empty;
            OnBaseGumpsSelected(this, new BaseGumpsSelectedEventArgs(gump));
            
			Invalidate();
		}

        protected virtual void OnBaseGumpsSelected(object sender, BaseGumpsSelectedEventArgs args)
        {
            if (BaseGumpsSelected != null)
            {
                BaseGumpsSelected(sender, args);
            }
        }

        protected virtual void OnBaseGumpMoved(object sender, EventArgs args)
        {
            if (BaseGumpMoved != null)
            {
                BaseGumpMoved(sender, args);
            }
        }

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
            
            if (Gump == null)
            {
                return;
            }

            if (e.Button == MouseButtons.None)
            {
                mouseDownAt = Point.Empty;
                resizing = false;
            }
            redoStack.Clear();
            int xDelta = e.Location.X - mouseAt.X;
            int yDelta = e.Location.Y - mouseAt.Y;

            if (resizing)
            {
                redoStack.Clear();
                OnBeforeBaseGumpMoved(this, EventArgs.Empty);
                BaseGump resizableGump = gump.GetSelectedGumps()[0];
                switch (resizeType)
                {
                    case ResizeType.BottomCenter:
                        {
                            resizableGump.Height += yDelta;
                            break;
                        }
                    case ResizeType.BottomLeft:
                        {
                            resizableGump.Location = new Point(resizableGump.Location.X + xDelta, resizableGump.Location.Y);
                            resizableGump.Width -= xDelta;
                            resizableGump.Height += yDelta;
                            break;
                        }
                    case ResizeType.BottomRight:
                        {
                            resizableGump.Width += xDelta;
                            resizableGump.Height += yDelta;
                            break;
                        }
                    case ResizeType.MiddleLeft:
                        {
                            resizableGump.Location = new Point(resizableGump.Location.X + xDelta, resizableGump.Location.Y);
                            resizableGump.Width -= xDelta;
                            break;
                        }
                    case ResizeType.MiddleRight:
                        {
                            resizableGump.Width += xDelta;
                            break;
                        }
                    case ResizeType.TopCenter:
                        {
                            resizableGump.Location = new Point(resizableGump.Location.X, resizableGump.Location.Y + yDelta);
                            resizableGump.Height -= yDelta;
                            break;
                        }
                    case ResizeType.TopLeft:
                        {
                            resizableGump.Location = new Point(resizableGump.Location.X + xDelta, resizableGump.Location.Y + yDelta);
                            resizableGump.Width -= xDelta;
                            resizableGump.Height -= yDelta;

                            break;
                        }
                    case ResizeType.TopRight:
                        {
                            resizableGump.Location = new Point(resizableGump.Location.X, resizableGump.Location.Y + yDelta);
                            resizableGump.Width += xDelta;
                            resizableGump.Height -= yDelta;
                            break;
                        }
                }

                resizableGump.Invalidate();
                OnBaseGumpMoved(this, EventArgs.Empty);
            }
            else if (movable != null)
            {
                redoStack.Clear();
                OnBeforeBaseGumpMoved(this, EventArgs.Empty);
                for (int i = 0; i < movable.Count; i++)
                {
                    movable[i].MoveBy(xDelta, yDelta);
                    movable[i].Invalidate();

                    OnBaseGumpMoved(this, EventArgs.Empty);
                }
            }
            else { }

            mouseAt = e.Location;

            Invalidate();
		}

        public virtual void Copy(object sender, EventArgs args)
        {
            if (Copied != null)
            {
                Copied(sender, args);
            }
        }

        public virtual void Paste(object sender, EventArgs args)
        {
            redoStack.Clear();

            if (Pasted != null)
            {
                Pasted(sender, args);
            }
        }

        public void Undo()
        {
            if (Gump != null && undoStack.Count > 0)
            {
                GumpState state = undoStack.Pop();
                redoStack.Push(state);
                Gump.Items = state.StateCollection;
            }
        }

        public void Redo()
        {
            if (Gump != null && redoStack.Count > 0)
            {
                undoStack.Push(new GumpState(Gump.Items));
                Gump.Items = redoStack.Pop().StateCollection;
            }
        }

		protected override void OnKeyDown(KeyEventArgs e)
		{
            if (Gump != null && Gump.GetSelectedGumps().Count > 0)
            {
                undoStack.Push(new GumpState(Gump.Items));
            }

			base.OnKeyDown(e);
			Invalidate();
		}
	}
}
