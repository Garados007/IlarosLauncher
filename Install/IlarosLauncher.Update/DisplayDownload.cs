using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;
using System.IO.Compression;
using System.Diagnostics;

namespace IlarosLauncher.Update
{
    public class DisplayDownload : Form
    {
        private ProgressBar progressBar1;
#if DEBUG
        string tempPath = Debugger.IsAttached ? Environment.CurrentDirectory : "%TEMP%\\IlarosLauncher";
#else
        string tempPath = "%TEMP%\\IlarosLauncher";
#endif

        public DisplayDownload()
        {
            getTempPath();
            tempPath = Environment.ExpandEnvironmentVariables(tempPath);
            var execPath = tempPath + "\\Path";
            if (!Application.StartupPath.StartsWith(execPath))
            {
                if (!Directory.Exists(execPath)) Directory.CreateDirectory(execPath);
                File.Copy(Application.ExecutablePath, execPath + "\\IlarosLauncher.Update.exe", true);
                Process.Start(new ProcessStartInfo()
                {
                    FileName = execPath + "\\IlarosLauncher.Update.exe",
                    WorkingDirectory = tempPath,
                });
                Close();
                return;
            }
            InitializeComponent();
            Load += DisplayDownload_Load;
        }

        void getTempPath()
        {
            var key = Registry.CurrentUser.OpenSubKey("Software");
            if (key != null ) key = key.OpenSubKey("IlarosLauncher");
            if (key == null) return;
            var path = key.GetValue("TempPath", tempPath);
            tempPath = path.ToString();
        }

        private void DisplayDownload_Load(object sender, EventArgs e)
        {
            try
            {
                File.WriteAllText(tempPath + "\\config.ini", "ServerType=\"" + DownloadSetting.ServerType +
                    "\"\r\nServerUrl=\"" + DownloadSetting.ServerUrl + "\"\r\nImgSourceType=\"" +
                    DownloadSetting.ImgSourceType + "\"\r\nImgCountLink=\"" + DownloadSetting.ImgCountLink +
                    "\"\r\nImgFileLink=" + DownloadSetting.ImgFileLink + "\"\r\n");
                if (!File.Exists(tempPath + "\\config.ini"))
                    MessageBox.Show("Datei wurde noch nicht erstellt...");
                var wc = new System.Net.WebClient();
                wc.DownloadProgressChanged += (s, ev) =>
                {
                    progressBar1.Value = ev.ProgressPercentage;
                };
                wc.DownloadFileCompleted += (s, ev) =>
                {
                    using (var archive = ZipFile.OpenRead(tempPath + "\\installer.zip"))
                        archive.ExtractToDirectory(tempPath, true);
                    File.Delete(tempPath + "\\installer.zip");
                    Process.Start(new ProcessStartInfo()
                    {
                        Arguments = mainArgs,
                        FileName = tempPath + "\\IlarosLauncher.UpdateClient.exe",
                        WorkingDirectory = tempPath,
                        Verb = "runas"
                    });
                    Close();
                };
                wc.DownloadFileAsync(new Uri(DownloadSetting.ServerUrl + "?mode=installer"),
                    tempPath + "\\installer.zip");
            }
            catch
            {
                MessageBox.Show("Der Installer konnte nicht heruntergeladen werden. Bitte stellen Sie eine Verbindung mit dem Internet " +
                    "wieder her und starten diesen Installer erneut.", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DisplayDownload));
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBar1.Location = new System.Drawing.Point(0, 0);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(275, 29);
            this.progressBar1.TabIndex = 0;
            // 
            // DisplayDownload
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(275, 29);
            this.ControlBox = false;
            this.Controls.Add(this.progressBar1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DisplayDownload";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ilaros Launcher - Updater";
            this.ResumeLayout(false);

        }

        static string mainArgs;

        [STAThread]
        public static void Main(string[] args)
        {
            mainArgs = string.Join(" ", args.ToList().ConvertAll((a) => "\"" + a + "\""));
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new DisplayDownload());
        }
    }

    public static class ZipArchiveExtensions
    {
        public static void ExtractToDirectory(this ZipArchive archive, string destinationDirectoryName, bool overwrite)
        {
            if (!overwrite)
            {
                archive.ExtractToDirectory(destinationDirectoryName);
                return;
            }
            foreach (ZipArchiveEntry file in archive.Entries)
            {
                string completeFileName = Path.Combine(destinationDirectoryName, file.FullName);
                if (file.Name == "")
                {// Assuming Empty for Directory
                    Directory.CreateDirectory(Path.GetDirectoryName(completeFileName));
                    continue;
                }
                file.ExtractToFile(completeFileName, true);
            }
        }
    }
}
