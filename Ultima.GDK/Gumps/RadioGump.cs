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
	public class RadioGump : BaseGump
	{
		private int checkedID;
		private int normalID;

		private CheckState checkState;

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

		private int value;


        [Description("Gets or sets the value of the button to be used in the OnResponse method of the gump")]
		public int Value { get { return value; } set { this.value = value; } }
        
        [Description("Gets or sets the value that determines if the gump is checked or not")]
		public CheckState CheckState
		{
			get { return checkState; }
			set
			{
				checkState = value;


				if( Image != null )
				{
					Image.Dispose();
					Image = null;
				}

				Invalidate();
			}
		}

        [Description("Gets or sets the value of the art index for the gump when not checked")]
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


        [Description("Gets or sets the value of the art index for the gump when checked")]
		public virtual int CheckedID
		{
			get { return checkedID; }
			set
			{
				if( IsValidIndex(value) )
					checkedID = value;

				if( Image != null )
				{
					Image.Dispose();
					Image = null;
				}

				Invalidate();
			}
		}

		public RadioGump(int checkedID, int normalID)
			: base() 
		{
			this.checkedID = checkedID;
            this.normalID = normalID;
		}

		public RadioGump() : this(208, 209) 
		{

		}

		public override Bitmap GetImage()
		{
			if( checkState == CheckState.Unchecked )
				Index = normalID;
			else
				Index = checkedID;

			return base.GetImage();
		}

        public override BaseGump Clone()
        {
            RadioGump b = new RadioGump();
            b.CheckedID = checkedID;
            b.NormalID = normalID;
            b.CheckState = checkState;
            b.Value = value;

            return base.Clone(b);
        }

        protected override BaseGump Clone(BaseGump b)
        {
            ((RadioGump)b).CheckedID = checkedID;
            ((RadioGump)b).NormalID = normalID;
            ((RadioGump)b).CheckState = checkState;
            ((RadioGump)b).Value = value;

            return base.Clone(b);
        }

		//public void AddRadio( int x, int y, int inactiveID, int activeID, bool initialState, int switchID )
        public override string ToString()
        {
            return String.Format("\t\tAddRadio({0}, {1}, {2}, {3}, {4}, {5});",
                Location.X, Location.Y, normalID, checkedID,
                checkState == CheckState.Checked ? "true" : "false",
                "(int)ButtonTypes." + Name);
        }	
        
        public override void Serialize(XmlWriter writer)
        {
            base.Serialize(writer);

            writer.WriteAttributeString("checkedId", checkedID.ToString());
            writer.WriteAttributeString("normalId", normalID.ToString());
            writer.WriteAttributeString("checkState", checkState.ToString());
            writer.WriteAttributeString("value", value.ToString());
        }

        public override void Deserialize(XmlReader reader)
        {
            base.Deserialize(reader);

            normalID = XmlConvert.ToInt32(reader.GetAttribute("checkedId"));
            checkedID = XmlConvert.ToInt32(reader.GetAttribute("normalId"));
            checkState = (CheckState)Enum.Parse(typeof(CheckState), reader.GetAttribute("checkState"));
            value = XmlConvert.ToInt32(reader.GetAttribute("value"));
        }
    }
}
