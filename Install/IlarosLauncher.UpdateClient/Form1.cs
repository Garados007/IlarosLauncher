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
        bool validPath = false;

        public Form1()
        {
            InitializeComponent();
            targetDirectory.Text = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + "\\IlarosLauncher";
            if (File.Exists("License.txt"))
                licensebox.Text = File.ReadAllText("License.txt", Encoding.UTF8);
            else licensebox.Text = "no license file found";
            SetButtonStates(false, false, false, true);
            this.Load += Form1_Load;
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

            manager.Stages.Add(new FetchDownloadList());
            manager.Stages.Add(new DownloadFile());
            manager.Stages.Add(new SetupLocalEnvironment());

            stageUpdateTimer.Enabled = true;
            await manager.Execute();
            stageUpdateTimer.Enabled = false;
            stageUpdateTimer_Tick(sender, e);
            manager.Dispose();
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
