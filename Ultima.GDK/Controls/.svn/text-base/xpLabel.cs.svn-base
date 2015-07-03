//This file was downloaded from http://www.codeproject.com/cs/miscctrl/XPEnabledLabelControl.asp
//All contents provided below belong to its respectable author(s)

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Windows.Forms;

namespace Ultima.GDK
{
	#region Custom Type/Styles
	/// <summary>
	/// Type for handling the BackColorScheme
	/// </summary>
	public enum BackColorSchemeType
	{
		/// <summary>
		/// Office 2003 Blue Color
		/// </summary>
		Office2003Blue,
		/// <summary>
		/// Office 2003 Orange
		/// </summary>
		Office2003Orange,
		/// <summary>
		/// Office 2003 Silver
		/// </summary>
		Office2003Silver,
		/// <summary>
		/// Office 2003 Green
		/// </summary>
		Office2003Green,
		/// <summary>
		/// Dark Blue Tinge
		/// </summary>
		DarkBlueTinge,
		/// <summary>
		/// OSX Light Gray
		/// </summary>
		OSXLightGray,
		/// <summary>
		/// Lime Green
		/// </summary>
		LightGreenVikings,
		/// <summary>
		/// Azure color
		/// </summary>
		Azure
	}

	/// <summary>
	/// Type for handling the text-alignment on the control
	/// </summary>
	public enum TextAlignStyle
	{
		/// <summary>
		/// Aligns Left-top
		/// </summary>
		TAlignLeftTop,
		/// <summary>
		/// Aligns Left-middle
		/// </summary>
		TAlignLeftMiddle,
		/// <summary>
		/// Aligns Left-bottom
		/// </summary>
		TAlignLeftBottom,
		/// <summary>
		/// Aligns Middle-top
		/// </summary>
		TAlignMiddleTop,
		/// <summary>
		/// Aligns Middle-middle
		/// </summary>
		TAlignMiddleMiddle,
		/// <summary>
		/// Aligns Middle-bottom
		/// </summary>
		TAlignMiddleBottom
	}
	#endregion

    /// <summary>
    /// XP enabled Label Control
    /// </summary>
	public class XpLabel : System.Windows.Forms.Control
	{
		#region Custom variables

		private Color _bkgcolo1 = Color.FromArgb(159, 191, 236);
		private Color _bkgcolo2 = Color.FromArgb(54, 102, 187);
		private bool _shadow = false;
		private BackColorSchemeType _bkgColorScheme = BackColorSchemeType.Office2003Blue;
		private TextAlignStyle _taStyle = TextAlignStyle.TAlignLeftMiddle;

		#endregion

		#region Windows generated code

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// Default constructor for Label control
		/// </summary>
		public XpLabel()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// my code
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.DoubleBuffer, true);
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			Text = this.Name;
			BackColor = Color.FromArgb(159, 191, 236);
			ChangeColorScheme();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if( components != null )
					components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// xpLabel
			// 
			this.Name = "xpLabel";

		}
		#endregion

		#endregion

		#region Extended Properties

		/// <summary>
		/// Starting color of gradient.
		/// </summary>
		[Description("Starting color of Gradient"),
		Category("Gradient Colors")]
		public Color BackColor1
		{
			get{ return _bkgcolo1; }
			set
			{ 
				if( (_bkgcolo1 != value) )
				{
					_bkgcolo1 = value; 
					Invalidate();
				}
			}
		}

		/// <summary>
		/// Ending color of gradient.
		/// </summary>
		[Description("Ending color of Gradient"),
		Category("Gradient Colors")]
		public Color BackColor2
		{
			get{ return _bkgcolo2; }
			set
			{ 
				if( (_bkgcolo2 != value) )
				{
					_bkgcolo2 = value; 
					Invalidate();
				}
			}
		}

		/// <summary>
		/// Drop shadow for the text.
		/// </summary>
		[Description("Show Shadow for the text")]
		public bool ShowShadow
		{
			get{ return _shadow; }
			set
			{
				_shadow = value;
				Invalidate();
			}
		}

		/// <summary>
		/// BackColorScheme: To use pre-defined color scheme.
		/// </summary>
		[Description("Color scheme to be used")]
		public BackColorSchemeType BackColorScheme
		{
			get{ return _bkgColorScheme; }
			set
			{
				if( _bkgColorScheme != value )
				{
					_bkgColorScheme = value;
					ChangeColorScheme();
					Invalidate();
				}
			}
		}

		/// <summary>
		/// The way to align text on the control
		/// </summary>
		[Description("Sets the alignment of the text")]
		public TextAlignStyle TextAlign
		{
			get{ return _taStyle; }
			set
			{
				if( _taStyle != value )
				{
					_taStyle = value;
					Invalidate();
				}
			}
		}

		#endregion

		/// <summary>
		/// Paints the control
		/// </summary>
		/// <param name="e">PaintEventArgs type object</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint (e);

			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
			LinearGradientBrush lgb = new LinearGradientBrush(
				new Rectangle(0, 0, Width, Height), BackColor1, BackColor2,
				90, false);
			e.Graphics.FillRectangle(lgb, 0, 0, Width, Height);
			lgb.Dispose();

			SolidBrush tb = new SolidBrush(ForeColor);
			e.Graphics.MeasureString(Text, Font, Width);
			e.Graphics.DrawString(Text, Font, tb, CalculateTextLocation());
			tb.Dispose();

			if( ShowShadow )
			{
				SolidBrush tb1 = new SolidBrush(ForeColor);
				e.Graphics.MeasureString(Text, Font, Width);
				e.Graphics.DrawString(Text, Font, tb1, CalculateTextLocation().X + 0.5f, CalculateTextLocation().Y + 0.5f);
				tb1.Dispose();
			}
		}

		/// <summary>
		/// Fires when the background-image is changed.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnBackgroundImageChanged(EventArgs e)
		{
			BackgroundImage = null;
			//base.OnBackgroundImageChanged (e);
		}

		/// <summary>
		/// Fires when the Text property is changed.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnTextChanged(EventArgs e)
		{
			base.OnTextChanged (e);
			Invalidate();
		}


		#region Helper functions

		private void ChangeColorScheme()
		{
			switch(BackColorScheme)
			{
				case BackColorSchemeType.Office2003Blue:
					BackColor = Color.FromArgb(159, 191, 236);
					BackColor1 = Color.FromArgb(159, 191, 236);
					BackColor2 = Color.FromArgb(54, 102, 187);
					break;
				case BackColorSchemeType.Office2003Orange:
					BackColor = Color.FromArgb(251, 230, 148);
					BackColor1 = Color.FromArgb(251, 230, 148);
					BackColor2 = Color.FromArgb(239, 150, 21);
					break;
				case BackColorSchemeType.DarkBlueTinge:
					BackColor = Color.FromArgb(89, 135, 214);
					BackColor1 = Color.FromArgb(89, 135, 214);
					BackColor2 = Color.FromArgb(4, 57, 148);
					break;
				case BackColorSchemeType.OSXLightGray:
					BackColor = Color.FromArgb(242, 242, 242);
					BackColor1 = Color.FromArgb(242, 242, 242);
					BackColor2 = Color.FromArgb(200, 200, 200);
					break;
				case BackColorSchemeType.LightGreenVikings:
					BackColor = Color.FromArgb(235, 245, 214);
					BackColor1 = Color.FromArgb(235, 245, 214);
					BackColor2 = Color.FromArgb(195, 224, 133);
					break;
				case BackColorSchemeType.Office2003Silver:
					BackColor = Color.FromArgb(225, 226, 236);
					BackColor1 = Color.FromArgb(225, 226, 236);
					BackColor2 = Color.FromArgb(150, 148, 178);
					break;
				case BackColorSchemeType.Office2003Green:
					BackColor = Color.FromArgb(234, 240, 207);
					BackColor1 = Color.FromArgb(234, 240, 207);
					BackColor2 = Color.FromArgb(178, 193, 140);
					break;
				case BackColorSchemeType.Azure:
					BackColor = Color.FromArgb(222, 218, 202);
					BackColor1 = Color.FromArgb(222, 218, 202);
					BackColor2 = Color.FromArgb(192, 185, 154);
					break;
			}
		}

		private PointF CalculateTextLocation()
		{
			float x = 0.0f, y = 0.0f;

			switch(TextAlign)
			{
				case TextAlignStyle.TAlignLeftTop:
					x = 2;
					y = 0;
					break;
				case TextAlignStyle.TAlignLeftBottom:
					x = 2;
					y = Height - 15;
					break;
				case TextAlignStyle.TAlignLeftMiddle:
					x = 2;
					y = (Height - 15) / 2;
					break;
				case TextAlignStyle.TAlignMiddleTop:
					x = (Width - 80) / 2;
					y = 0;
					break;
				case TextAlignStyle.TAlignMiddleMiddle:
					x = (Width - 80) / 2;
					y = (Height - 15) / 2;
					break;
				case TextAlignStyle.TAlignMiddleBottom:
					x = (Width - 80) / 2;
					y = (Height - 15);
					break;
				default:
					throw new InvalidEnumArgumentException("Invalid Argument for Text Alignment");
			}
			return new PointF(x, y);
		}

		#endregion

	}
}
