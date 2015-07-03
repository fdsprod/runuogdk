using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using GdkSettings = Ultima.GDK.XmlSettings;
using System.Threading;
using System.Net;
using System.Text;

namespace Ultima.GDK
{
    public static class Program
    {
        private static ArgumentProcessor argsProc;
        private static string baseDirectory;
        private static string exePath;
        private static Assembly assembly;
        private static Process process;
        private static GdkSettings settings;

        public static GdkSettings Settings
        {
            get { return settings; }
        }

        public static string BaseDirectory
        {
            get
            {
                if (baseDirectory == null)
                {
                    try
                    {
                        baseDirectory = ExePath;

                        if (baseDirectory.Length > 0)
                            baseDirectory = Path.GetDirectoryName(baseDirectory);
                    }
                    catch
                    {
                        baseDirectory = "";
                    }
                }

                return baseDirectory;
            }
        }

        public static string ExePath
        {
            get
            {
                if (exePath == null)
                    exePath = assembly.Location;

                return exePath;
            }
        }

        public static ArgumentProcessor ArgumentProcessor 
        {
            get
            { 
                return argsProc; 
            } 
        }

        public static string SettingsPath
        {
            get { return Path.Combine(BaseDirectory, "config.xml"); }
        }

        [STAThread]
        static void Main(string[] args)
        {
            argsProc = ArgumentProcessor.Compile(args);
            process = Process.GetCurrentProcess();
            assembly = Assembly.GetEntryAssembly();

            string mutexName = "RunUO: GDK";

            using (Mutex instanceMutex = new Mutex(false, mutexName))
            {
                if (instanceMutex.WaitOne(1, true) == false)
                {
                    MessageBox.Show("You may only run 1 instance of RunUO: GDK at a time.", "RunUO: GDK");
                    return;
                }

                try
                {
                    WebClient client = new WebClient();
                    byte[] buffer = client.DownloadData(new Uri("http://www.fallingdownstairs.net/runuogdk/version.php"));

                    string version = Encoding.ASCII.GetString(buffer);
                    Version latest = new Version(version);
                    Version current = System.Reflection.Assembly.GetEntryAssembly().GetName().Version;

                    if (current.CompareTo(latest) == -1 && 
                        MessageBox.Show("A newer version of RunUO: GDK is available!\nDo you wish to update?", 
                        "RunUO: GDK", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(Path.Combine(BaseDirectory, "Updater.exe"));
                        Application.Exit();
                    }
                }
                catch(Exception e)
                {

                }

                string updateNew = Path.Combine(BaseDirectory, "Updater.exe.new");
                if(File.Exists(updateNew))
                {
                    try
                    {
                        File.Move(updateNew, Path.GetFileNameWithoutExtension(updateNew));
                    }
                    catch { }
                }
                                
                string unrarNew = Path.Combine(BaseDirectory, "unrar.dll.new");
                if(File.Exists(unrarNew))
                {
                    try
                    {
                        File.Move(unrarNew, Path.GetFileNameWithoutExtension(unrarNew));
                    }
                    catch { }
                }
                
                if (File.Exists(SettingsPath))
                {
                    settings = GdkSettings.Load(SettingsPath);
                }
                else
                {
                    settings = new GdkSettings();
                    settings.Save(SettingsPath);
                }

                uint id = Settings.GetValue<uint>("user_id", (uint)(Utility.Rand.Next() + Utility.Rand.Next()));
                
                Application.ApplicationExit += new EventHandler(Application_ApplicationExit);
                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
        
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new SplashScreen());                
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Utility.LogException(e.ExceptionObject as Exception);
        }

        private static void Application_ApplicationExit(object sender, EventArgs e)
        {
            Settings.Save(SettingsPath);
        }
    }
}