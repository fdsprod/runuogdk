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
    public enum ButtonType
    {
        Page,
        Reply
    }

    public enum ButtonState
    {
        Normal,
        Pressed
    }

    public class ButtonGump : BaseGump
    {
        private int pressedID;
        private int normalID;
        private int page;
        private int value;
        private ButtonState buttonState;
        private ButtonType buttonType;

        [Description("The value of the button, this is used to determine if a button was pressed in the OnResponse method")]
        public int Value { get { return value; } set { this.value = value; } }

        [Description("The state of the button")]
		public ButtonState ButtonState
		{
			get { return buttonState; }
			set
			{
				buttonState = value;


				if( Image != null )
				{
					Image.Dispose();
					Image = null;
				}

				Invalidate();
			}
		}

        [Description("The type of button.\n\"Page\" is used to move from page to page and does not trigger OnResponse.\n \"Button\" Triggers OnResponse")]
		public ButtonType ButtonType { get { return buttonType; } set { buttonType = value; } }
		
        [Editor(typeof(GumpIndexEditor), typeof(UITypeEditor)), Description("The art index of the gump when it is not being pressed")]
		public virtual int NormalID
		{
			get { return normalID; }
			set
			{
				if( IsValidIndex(value) )
					normalID = value;

				if( Image != null )
				{
					Image.Dispose();
					Image = null;
				}

				Invalidate();
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

        public int Page
        {
            get
            {
                return page;
            }
            set
            {
                page = value;
            }
        }

        [Editor(typeof(GumpIndexEditor), typeof(UITypeEditor)), Description("The art index of the gump when it is being pressed")]
		public virtual int PressedID
		{
			get { return pressedID; }
			set
			{
				if( IsValidIndex(value) )
					pressedID = value;

				if( Image != null )
				{
					Image.Dispose();
					Image = null;
				}

				Invalidate();
			}
		}

        public ButtonGump()
            : base()
        {
			normalID = 247;
			pressedID = 248;
        }

		public override Bitmap GetImage()
		{
			if( buttonState == ButtonState.Normal )
				Index = normalID;
			else
				Index = pressedID;

			return base.GetImage();
        }

        public override BaseGump Clone()
        {
            ButtonGump b = new ButtonGump();
            b.ButtonState = buttonState;
            b.buttonType = buttonType;
            b.PressedID = pressedID;
            b.NormalID = normalID;
            b.Value = value;

            return base.Clone(b);
        }

        protected override BaseGump Clone(BaseGump b)
        {
            ((ButtonGump)b).ButtonState = buttonState;
            ((ButtonGump)b).buttonType = buttonType;
            ((ButtonGump)b).PressedID = pressedID;
            ((ButtonGump)b).NormalID = normalID;
            ((ButtonGump)b).Value = value;

            return base.Clone(b);
        }

        public override void Serialize(XmlWriter writer)
        {
            base.Serialize(writer);

            writer.WriteAttributeString("normalId", normalID.ToString());
            writer.WriteAttributeString("pressedId", pressedID.ToString());
            writer.WriteAttributeString("buttonType", buttonType.ToString());
            writer.WriteAttributeString("buttonState", buttonState.ToString());
            writer.WriteAttributeString("value", value.ToString());
        }

        public override void Deserialize(XmlReader reader)
        {
            base.Deserialize(reader);

            normalID = XmlConvert.ToInt32(reader.GetAttribute("normalId"));
            pressedID = XmlConvert.ToInt32(reader.GetAttribute("pressedId"));
            buttonType = (ButtonType)Enum.Parse(typeof(ButtonType), reader.GetAttribute("buttonType"));
            buttonState = (ButtonState)Enum.Parse(typeof(ButtonState), reader.GetAttribute("buttonState"));
            value = XmlConvert.ToInt32(reader.GetAttribute("value"));        
        }

		//public void AddButton( int x, int y, int normalID, int pressedID, int buttonID, GumpButtonType type, int param )
		public override string ToString()
		{
			return String.Format("\t\tAddButton( {0}, {1}, {2}, {3}, {4}, {5}{6});", Location.X, Location.Y, normalID, pressedID, "(int)ButtonTypes." + Name, "ButtonType." + buttonType, buttonType == ButtonType.Page ? ", " + page.ToString() : "" );
		}
    }
}
