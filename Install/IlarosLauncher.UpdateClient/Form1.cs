using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using IlarosLauncher.UpdateClient.Update;

namespace IlarosLauncher.UpdateClient
{
    public partial class Form1 : Form
    {
        bool validPath = false, installFinished = false, skipedOptions = false;

        public Form1()
        {
            InitializeComponent();
            targetDirectory.Text = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + "\\IlarosLauncher";
            if (File.Exists("License.txt"))
                licensebox.Text = File.ReadAllText("License.txt", Encoding.UTF8);
            else licensebox.Text = "no license file found";
            SetButtonStates(false, false, false, true);
            this.Load += Form1_Load;
            this.FormClosing += Form1_FormClosing;
            clientVersion.Text = "Update Client Version: " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (installFinished && startLauncher.Checked)
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = DownloadSettings.Current.LauncherPath + "\\IlarosLauncher.exe",
                    Verb = "",
                    WorkingDirectory = DownloadSettings.Current.LauncherPath
                });
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!DownloadSettings.LoadFromIni())
            {
                MessageBox.Show("Die Einstellungskonfigurationen konnten nicht gefunden werden. Bitte starten Sie den Installer erneut!",
                    Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }
            if (!DownloadSettings.CanConnect())
            {
                MessageBox.Show("Es konnte keine Verbindung mit dem Updateserver hergestellt werden. Dies kann an folgenden Problemen liegen:\n" +
                    "\n1. Sie haben aktuell keine Internetverbindung\n2. Der Updateserver ist offline\n3. Diese Updatesoftware ist veraltet.",
                    Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }
            HandleStartFlags();
            if (!Program.StartupParams.Options.ContainsKey("/disable-skip") && (skipedOptions = CanSkip()))
            {
                btnInstall.PerformClick();
            }
        }

        void HandleStartFlags()
        {
            var p = Program.StartupParams.Options;
            if (p.ContainsKey("/path")) targetDirectory.Text = p["/path"];
            if (p.ContainsKey("/use-temp")) useTemp.Checked = true;
            if (p.ContainsKey("/app-data")) useAppdata.Checked = true;
            if (p.ContainsKey("/search-updates")) optSearchForUpdates.Checked = true;
            if (p.ContainsKey("/download-updates")) optDownloadUpdates.Checked = true;
            if (p.ContainsKey("/download-backgrounds")) optDownloadNewImages.Checked = true;
            if (p.ContainsKey("/desktop-link")) optCreateDesktopLink.Checked = true;
            if (p.ContainsKey("/download-backgrounds-now")) optDownloadImagesNow.Checked = true;
            if (p.ContainsKey("/close-window")) optCloseWindowAfterDownload.Checked = true;
            if (p.ContainsKey("/start-launcher")) optStartLauncher.Checked = true;
            if (p.ContainsKey("/!use-temp")) useTemp.Checked = false;
            if (p.ContainsKey("/!app-data")) useAppdata.Checked = false;
            if (p.ContainsKey("/!search-updates")) optSearchForUpdates.Checked = false;
            if (p.ContainsKey("/!download-updates")) optDownloadUpdates.Checked = false;
            if (p.ContainsKey("/!download-backgrounds")) optDownloadNewImages.Checked = false;
            if (p.ContainsKey("/!desktop-link")) optCreateDesktopLink.Checked = false;
            if (p.ContainsKey("/!download-backgrounds-now")) optDownloadImagesNow.Checked = false;
            if (p.ContainsKey("/!close-window")) optCloseWindowAfterDownload.Checked = false;
            if (p.ContainsKey("/!start-launcher")) optStartLauncher.Checked = false;
        }

        bool CanSkip()
        {
            var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software", false);
            key = key?.OpenSubKey("IlarosLauncher", false);
            if (key == null) return false;
            var val = key.GetValue("LicenseAccepted");
            if (val == null) return false;
            if (val.ToString() == "1")
            {
                acceptlicense.Checked = true;
                var path = key.GetValue("LauncherPath").ToString();
                var db = key.GetValue("Install.DownloadBackgrounds").ToString() != "0";
                var ua = key.GetValue("Install.UseAppData").ToString() != "0";
                var ut = key.GetValue("Install.UseTemp").ToString() != "0";
                useTemp.Checked = ut;
                useAppdata.Checked = ua;
                targetDirectory.Text = path;
                optDownloadImagesNow.Checked = db;
                optCreateDesktopLink.Checked = false;
                return true;
            }
            else return false;
        }

        void SetButtonStates(bool back, bool forward, bool install, bool close)
        {
            btnBack.Enabled = back;
            btnForward.Enabled = forward;
            btnInstall.Enabled = install;
            btnClose.Enabled = close;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (tablessControl1.SelectedIndex > 0)
                tablessControl1.SelectedIndex--;
        }

        private void btnForward_Click(object sender, EventArgs e)
        {
            if (tablessControl1.SelectedIndex < tablessControl1.TabCount - 1)
                tablessControl1.SelectedIndex++;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void tablessControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tablessControl1.SelectedIndex)
            {
                case 0:
                    SetButtonStates(false, acceptlicense.Checked, acceptlicense.Checked && validPath, true);
                    break;
                case 1:
                    SetButtonStates(true, true, validPath, true);
                    break;
                case 2:
                    SetButtonStates(true, false, validPath, true);
                    break;
                case 3:
                    SetButtonStates(false, false, false, true);
                    break;
                case 4:
                    SetButtonStates(false, false, false, true);
                    break;
            }
        }

        private void acceptlicense_CheckedChanged(object sender, EventArgs e)
        {
            SetButtonStates(false, acceptlicense.Checked, acceptlicense.Checked && validPath, true);
        }

        private void targetDirectory_TextChanged(object sender, EventArgs e)
        {
            try
            {
                new FileInfo(targetDirectory.Text);
                validPath = true;
            }
            catch
            {
                validPath = false;
            }
            SetButtonStates(true, true, validPath, true);
        }

        private void showTargetDirectoryDialog_Click(object sender, EventArgs e)
        {
            if (validPath) folderBrowserDialog1.SelectedPath = targetDirectory.Text;
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                var d = new DirectoryInfo(folderBrowserDialog1.SelectedPath);
                if (d.Name != "IlarosLauncher") targetDirectory.Text = d.FullName + "\\IlarosLauncher";
                else targetDirectory.Text = d.FullName;
            }
        }

        private void optStartLauncher_CheckedChanged(object sender, EventArgs e)
        {
            startLauncher.Checked = optStartLauncher.Checked;
        }

        UpdateStage stage1, stage2;

        void updateStageInfo(UpdateStage stage, Label stageTask, Label stageInfo, ProgressBar progress)
        {
            if (stage != null)
            {
                stageTask.Text = stage.TaskName;
                progress.Value = (int)(10000 * stage.TaskProgress);
                var r = stageInfo.Right;
                stageInfo.Text = stage.TaskInfo;
                stageInfo.Left = r - stageInfo.Width;
            }
            else
            {
                stageTask.Text = null;
                progress.Value = 0;
                var r = stageInfo.Right;
                stageInfo.Text = null;
                stageInfo.Left = r - stageInfo.Width;
            }
        }

        private void stageUpdateTimer_Tick(object sender, EventArgs e)
        {
            updateStageInfo(stage1, stageTask1, stageInfo1, progressBar1);
            updateStageInfo(stage2, stageTask2, stageInfo2, progressBar2);
        }

        void Invoke(Action action)
        {
            base.Invoke(action);
        }

        private void useTemp_CheckedChanged(object sender, EventArgs e)
        {
            label1.Text = useTemp.Checked ? "Speichert temporäre Daten ins dafür vorgesehene Systemverzeichnis." :
                "Temporäre Daten werden in dem Installationsordner gespeichert. ACHTUNG: Es muss Schreibzugriff bestehen.";

        }

        private void useAppdata_CheckedChanged(object sender, EventArgs e)
        {
            label2.Text = useAppdata.Checked ? "Speichert Einstellungen und Hintergründe in das dafür vorgesehene Systemverzeichnis." :
                "Einstellungen und Hintergründe werden in den Installationsordner gespeichert. ACHTUNG: Es muss Schreibzugriff bestehen.";
        }

        private void useRegistry_CheckedChanged(object sender, EventArgs e)
        {
            label3.Text = useRegistry.Checked ? "Speichert die obrigen Einstellungen in die Registry." :
                "NICHT UNTERSTÜTZT !!!";
        }

        private async void btnInstall_Click(object sender, EventArgs e)
        {
            tablessControl1.SelectedTab = tabInstall;
            DownloadSettings.Current = new DownloadSettings()
            {
                CreateDesktopLink = optCreateDesktopLink.Checked,
                DownloadBackgrounds = optDownloadImagesNow.Checked,
                LauncherPath = targetDirectory.Text,
                LSDownloadBackgrounds = optDownloadNewImages.Checked,
                LSDownloadUpdates = optDownloadUpdates.Checked,
                LSSearchUpdates = optSearchForUpdates.Checked,
                UseAppData = useAppdata.Checked,
                UseRegistry = useRegistry.Checked,
                UseTemp = useTemp.Checked,
                SkipedOptions = skipedOptions,
            };
            var manager = new UpdateManager(2);
            manager.StartExecution += (s, stage) =>
            {
                if (stage1 == null)
                {
                    stage1 = stage;
                    Invoke(() => stageName1.Text = stage.GlobalTaskVerb);
                }
                else
                {
                    stage2 = stage;
                    Invoke(() => stageName2.Text = stage.GlobalTaskVerb);
                }
                stage.NewTaskAdded += Stage_NewTaskAdded;
            };
            manager.EndExecution += (s, stage) =>
            {
                if (stage1 == stage)
                {
                    stage1 = null;
                    Invoke(() => stageName1.Text = null);
                }
                else
                {
                    stage2 = null;
                    Invoke(() => stageName2.Text = null);
                }
                Invoke(() => stage.NewTaskAdded -= Stage_NewTaskAdded);
            };

            manager.Stages.AddRange(new UpdateStage[] { 
                new FetchDownloadList(),
                new DownloadFile(),
                new SetupLocalEnvironment(),
                new CreateCompactFile(),
                new CloseWaiter(),
                new ManageFiles(),
                new FileSearcher(),
                new SetRegistry(),
                new DesktopLink(),
                new BackgroundSearcher(),
                new BackgroundDownloader(),
            });

            stageUpdateTimer.Enabled = true;
            await manager.Execute();
            stageUpdateTimer.Enabled = false;
            stageUpdateTimer_Tick(sender, e);
            manager.Dispose();
            installFinished = true;
            if (optCloseWindowAfterDownload.Checked) Close();
#if DEBUG
            if (!System.Diagnostics.Debugger.IsAttached) tablessControl1.SelectedTab = tabFinish;
            else SetButtonStates(false, true, false, true);
#else
            tablessControl1.SelectedTab = tabFinish;
#endif
        }

        Dictionary<UpdateTask, int> indexTable = new Dictionary<UpdateTask, int>();

        private void Stage_NewTaskAdded(UpdateTask task)
        {
            lock (indexTable)
            {
                Invoke(() =>
                {
                    taskList.Items.Add(task);
                    taskList.TopIndex = taskList.Items.Count - 1;
                    indexTable.Add(task, indexTable.Count);
                    task.ValueChanged += (sender, e) => Invoke(() => taskList.RefreshItem(indexTable[sender as UpdateTask]));
                });
            }
        }
    }
}
