using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Ultima.GDK.Gumps;
using System.Reflection;

namespace Ultima.GDK
{
    public partial class SplashScreen : Form
    {
        System.Timers.Timer t = new System.Timers.Timer();
        MainForm form;// = new MainForm();

        public SplashScreen()
        {
            InitializeComponent();
        }

        private void SplashScreenLoad(object sender, EventArgs e)
        {
            lblVersion.Text += String.Format(" Beta {0}", Assembly.GetEntryAssembly().GetName().Version.Minor.ToString());
            t.Interval = 1500;
            t.Elapsed += new System.Timers.ElapsedEventHandler(t_Elapsed);
            t.Start();
        }

        private void t_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            t.Stop();

            Invoke((MethodInvoker)delegate
            {
                form = new MainForm();
                Hide();
                form.Show();
            });
        }        
    }
}