using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace Updater
{
    public partial class UpdaterForm : Form
    {
        private WebClient downloadClient;
        private const string FILE_URL = "http://www.fallingdownstairs.net/runuogdk/latest_update.rar";
        private const string FILE_NAME = "latest_update.rar";
        private const string GDK_ASSEMBLY_NAME = "RunUO GDK.exe";

        public UpdaterForm()
        {
            InitializeComponent();
        }

        private void UpdaterForm_Load(object sender, EventArgs e)
        {
            string processId = Path.GetFileNameWithoutExtension(GDK_ASSEMBLY_NAME);
            Process[] processes = Process.GetProcessesByName(processId);

            while (processes.Length > 0)
            {
                Thread.Sleep(100);
                processes = Process.GetProcessesByName(processId);
            }

            string filePath = Path.Combine(Application.StartupPath, FILE_NAME);
            
            downloadClient = new WebClient();
            downloadClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(OnDownloadProgressChanged);
            downloadClient.DownloadFileCompleted += new AsyncCompletedEventHandler(OnDownloadFileCompleted);
            downloadClient.DownloadFileAsync(new Uri(FILE_URL), filePath);            
        }

        protected virtual void OnDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
            lblStatus.Text = String.Format("Downloading bytes {0}/{1}", e.BytesReceived, e.TotalBytesToReceive);
        }

        protected virtual void OnDownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(this,
                    "An error occurred while downloading the update for RunUO: GDK\n" +
                    "Please try again later or post the following error on the bug board at " +
                    "http://www.runuoforge.org/gf/project/runuogdk/\n" + e.Error.Message,
                    "RunUO: GDK - Updater");
                return;
            }
            else
            {
                progressBar.Value = 100;

                string filePath = Path.Combine(Application.StartupPath, FILE_NAME);
                Unrar unrar = new Unrar(filePath);

                unrar.ExtractionProgress += new ExtractionProgressHandler(OnExtractionProgress);
                unrar.DestinationPath = Application.StartupPath;
                unrar.Open(Unrar.OpenMode.Extract);

                while (unrar.ReadHeader())
                {
                    if (unrar.CurrentFile.FileName == "Updater.exe")
                    {
                        unrar.CurrentFile.FileName = "Updater.exe.new";
                    }
                    else if (unrar.CurrentFile.FileName == "unrar.dll")
                    {
                        unrar.CurrentFile.FileName = "unrar.dll.new";
                    }

                    while (File.Exists(unrar.CurrentFile.FileName))
                    {
                        try
                        {
                            File.Delete(unrar.CurrentFile.FileName);
                            continue;
                        }
                        catch (Exception exception)
                        {
                            DialogResult result = MessageBox.Show(string.Format("Unable to delete file \"{0}\" (Error: {1})\nMake sure all instances of RunUO: GDK are closed.\nRunUO: GDK may be unusable until the update completes successfully.", unrar.CurrentFile.FileName, exception.Message), "RunUO: GDK - Updater", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Exclamation);

                            if (result == DialogResult.Abort)
                            {
                                Application.Exit();
                                return;
                            }

                            if (result != DialogResult.Ignore)
                            {
                                continue;
                            }

                            break;
                        }
                    }

                    unrar.Extract(Path.Combine(Application.StartupPath, unrar.CurrentFile.FileName));
                }

                unrar.Close();
            }

            Process.Start(Path.Combine(Application.StartupPath, GDK_ASSEMBLY_NAME));
            Application.Exit();
        }

        protected virtual void OnExtractionProgress(object sender, ExtractionProgressEventArgs e)
        {
            progressBar.Value = (int)(e.PercentComplete);
            lblStatus.Text = String.Format("Extracting File: {0} {1}/{2}", e.FileName, e.BytesExtracted, e.FileSize);
        }
    }
}