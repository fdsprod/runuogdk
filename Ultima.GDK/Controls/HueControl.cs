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
using System.Windows.Forms;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Drawing;
using System.Diagnostics;

namespace Ultima.GDK
{
public class HuePickerControl : UserControl
{
    // Fields
    [AccessedThroughProperty("cboQuick")]
    private ComboBox _cboQuick;
    [AccessedThroughProperty("lstHue")]
    private ListBox _lstHue;
    [AccessedThroughProperty("StatusBar")]
    private StatusBar _StatusBar;
    [AccessedThroughProperty("ToolTip1")]
    private ToolTip _ToolTip1;
    private IContainer components;
    protected Hue mHue;

    // Events
    public event ValueChangedEventHandler ValueChanged;

    // Methods
    public HuePickerControl()
    {
        base.Load += new EventHandler(this.HuePickerControl_Load);
        this.InitializeComponent();
    }

    public HuePickerControl(Hue InitialHue) : this()
    {
        this.mHue = InitialHue;
    }

    private void cboQuick_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str = "";
        string text = this.cboQuick.Text;
        if (text == "Colors")
        {
            str = "0";
        }
        else if (text=="Skin")
        {
            str = "1001";
        }
        else if (text=="Hair")
        {
            str = "1101";
        }
        else if (text=="Interesting #1")
        {
            str = "1049";
        }
        else if (text=="Pinks")
        {
            str = "1200";
        }
        else if (text=="Elemental Weapons")
        {
            str = "1254";
        }
        else if (text=="Interesting #2")
        {
            str = "1278";
        }
        else if (text=="Blues")
        {
            str = "1300";
        }
        else if (text=="Elemental Wear")
        {
            str = "1354";
        }
        else if (text=="Greens")
        {
            str = "1400";
        }
        else if (text=="Oranges")
        {
            str = "1500";
        }
        else if (text=="Reds")
        {
            str = "1600";
        }
        else if (text=="Yellows")
        {
            str = "1700";
        }
        else if (text=="Neutrals")
        {
            str = "1800";
        }
        else if (text=="Snakes")
        {
            str = "2000";
        }
        else if (text=="Birds")
        {
            str = "2100";
        }
        else if (text=="Slimes")
        {
            str = "2200";
        }
        else if (text=="Animals")
        {
            str = "2300";
        }
        else if (text == "Metals")
        {
            str = "2400";
        }
        this.lstHue.SelectedIndex = this.lstHue.FindString(str);
        this.lstHue.Focus();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing && (this.components != null))
        {
            this.components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void HuePickerControl_Load(object sender, EventArgs e)
    {
        this.lstHue.Items.Clear();
        foreach (Hue hue in Hues.List)
        {
            if (hue.Index == this.mHue.Index)
            {
                this.lstHue.SelectedIndex = this.lstHue.Items.Add(hue);
            }
            else
            {
                this.lstHue.Items.Add(hue);
            }
        }
        this.StatusBar.Text = mHue.Index.ToString() + ": " + this.mHue.Index.ToString();
    }

    [DebuggerStepThrough]
    private void InitializeComponent()
    {
        this.components = new Container();
        this.lstHue = new ListBox();
        this.StatusBar = new StatusBar();
        this.cboQuick = new ComboBox();
        this.ToolTip1 = new ToolTip(this.components);
        this.SuspendLayout();
        this.lstHue.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
        this.lstHue.DrawMode = DrawMode.OwnerDrawFixed;
        this.lstHue.IntegralHeight = false;
        Point point = new Point(0, 0);
        this.lstHue.Location = point;
        this.lstHue.Name = "lstHue";
        Size size = new Size(0xd0, 0xf4);
        this.lstHue.Size = size;
        this.lstHue.TabIndex = 0;
        point = new Point(0, 0xf2);
        this.StatusBar.Location = point;
        this.StatusBar.Name = "StatusBar";
        size = new Size(0xd0, 0x16);
        this.StatusBar.Size = size;
        this.StatusBar.SizingGrip = false;
        this.StatusBar.TabIndex = 1;
        this.cboQuick.DropDownStyle = ComboBoxStyle.DropDownList;
        this.cboQuick.DropDownWidth = 120;
        this.cboQuick.Items.AddRange(new object[] { 
            "Colors", "Skin", "Hair", "Interesting #1", "Pinks", "Elemental Weapons", "Interesting #2", "Blues", "Elemental Wear", "Greens", "Oranges", "Reds", "Yellows", "Neutrals", "Snakes", "Birds", 
            "Slimes", "Animals", "Metals"
         });
        point = new Point(0x90, 0xf3);
        this.cboQuick.Location = point;
        this.cboQuick.Name = "cboQuick";
        size = new Size(0x40, 0x15);
        this.cboQuick.Size = size;
        this.cboQuick.TabIndex = 2;
        this.ToolTip1.SetToolTip(this.cboQuick, "Bookmarks");
        this.Controls.Add(this.cboQuick);
        this.Controls.Add(this.StatusBar);
        this.Controls.Add(this.lstHue);
        this.Name = "HuePickerControl";
        size = new Size(0xd0, 0x108);
        this.Size = size;
        this.ResumeLayout(false);
    }

    private void lstHue_DoubleClick(object sender, EventArgs e)
    {
        if (this.ValueChanged != null)
        {
            this.ValueChanged(this.mHue);
        }
    }

    private void lstHue_DrawItem(object sender, DrawItemEventArgs e)
    {
        Graphics graphics = e.Graphics;
        graphics.FillRectangle(Brushes.White, e.Bounds);
        if ((e.State & DrawItemState.Selected) > DrawItemState.None)
        {
            Rectangle rect = new Rectangle(e.Bounds.X, e.Bounds.Y, 50, this.lstHue.ItemHeight);
            graphics.FillRectangle(SystemBrushes.Highlight, rect);
        }
        else
        {
            Rectangle rectangle4 = new Rectangle(e.Bounds.X, e.Bounds.Y, 50, this.lstHue.ItemHeight);
            graphics.FillRectangle(SystemBrushes.Window, rectangle4);
        }
        float num2 = ((float) (e.Bounds.Width - 0x23)) / 32f;
        Hue hue = (Hue) this.lstHue.Items[e.Index];
        graphics.DrawString(hue.Index.ToString(), e.Font, Brushes.Black, (float) (e.Bounds.X + 3), (float) e.Bounds.Y);
        int num = 0;
        foreach (short num3 in hue.Colors)
        {
            Rectangle rectangle = new Rectangle((e.Bounds.X + 0x23) + ((int) Math.Round((double) (num * num2))), e.Bounds.Y, (int) Math.Round((double) (num2 + 1f)), e.Bounds.Height);
            graphics.FillRectangle(new SolidBrush(Utility.Convert555ToARGB(num3)), rectangle);
            num++;
        }
    }

    private void lstHue_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.mHue = (Hue) this.lstHue.SelectedItem;
        this.StatusBar.Text = this.mHue.Index.ToString() + ": " + this.mHue.Index.ToString();
    }

    // Properties
    internal virtual ComboBox cboQuick
    {
        get
        {
            return this._cboQuick;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (this._cboQuick != null)
            {
                this._cboQuick.SelectedIndexChanged -= new EventHandler(this.cboQuick_SelectedIndexChanged);
            }
            this._cboQuick = value;
            if (this._cboQuick != null)
            {
                this._cboQuick.SelectedIndexChanged += new EventHandler(this.cboQuick_SelectedIndexChanged);
            }
        }
    }

    public Hue Hue
    {
        get
        {
            return this.mHue;
        }
        set
        {
            this.mHue = value;
        }
    }

    internal virtual ListBox lstHue
    {
        get
        {
            return this._lstHue;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (this._lstHue != null)
            {
                this._lstHue.DoubleClick -= new EventHandler(this.lstHue_DoubleClick);
                this._lstHue.SelectedIndexChanged -= new EventHandler(this.lstHue_SelectedIndexChanged);
                this._lstHue.DrawItem -= new DrawItemEventHandler(this.lstHue_DrawItem);
            }
            this._lstHue = value;
            if (this._lstHue != null)
            {
                this._lstHue.DoubleClick += new EventHandler(this.lstHue_DoubleClick);
                this._lstHue.SelectedIndexChanged += new EventHandler(this.lstHue_SelectedIndexChanged);
                this._lstHue.DrawItem += new DrawItemEventHandler(this.lstHue_DrawItem);
            }
        }
    }

    internal virtual StatusBar StatusBar
    {
        get
        {
            return this._StatusBar;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (this._StatusBar != null)
            {
            }
            this._StatusBar = value;
            if (this._StatusBar != null)
            {
            }
        }
    }

    internal virtual ToolTip ToolTip1
    {
        get
        {
            return this._ToolTip1;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (this._ToolTip1 != null)
            {
            }
            this._ToolTip1 = value;
            if (this._ToolTip1 != null)
            {
            }
        }
    }

    public delegate void ValueChangedEventHandler(Hue Hue);
}

}
