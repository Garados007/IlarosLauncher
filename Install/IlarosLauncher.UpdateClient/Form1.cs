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
    }
}
