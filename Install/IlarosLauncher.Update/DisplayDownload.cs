using System;
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
            if (!Application.StartupPath.StartsWith(tempPath))
            {
                if (!Directory.Exists(tempPath)) Directory.CreateDirectory(tempPath);
                File.Copy(Application.ExecutablePath, tempPath + "\\IlarosLauncher.Update.exe", true);
                Process.Start(new ProcessStartInfo()
                {
                    FileName = tempPath + "\\IlarosLauncher.Update.exe",
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
                    "\"\r\nServerUrl=\"" + DownloadSetting.ServerUrl + "\"\r\n");
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
                        FileName = tempPath + "\\IlarosLauncher.UpdateClient.exe",
                        WorkingDirectory = tempPath
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
            progressBar1 = new ProgressBar();
            SuspendLayout();
            // 
            // progressBar1
            // 
            progressBar1.Dock = DockStyle.Fill;
            progressBar1.Location = new System.Drawing.Point(0, 0);
            progressBar1.Name = "progressBar1";
            progressBar1.Minimum = 0;
            progressBar1.Maximum = 100;
            progressBar1.Size = new System.Drawing.Size(275, 29);
            progressBar1.TabIndex = 0;
            // 
            // DisplayDownload
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(275, 29);
            ControlBox = false;
            Controls.Add(progressBar1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "DisplayDownload";
            Text = "Ilaros Launcher - Updater";
            StartPosition = FormStartPosition.CenterScreen;
            ResumeLayout(false);

        }

        [STAThread]
        static void Main()
        {
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
