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

namespace IlarosLauncher.UpdaterCreator
{
    public partial class Form1 : Form
    {
        const int myversion = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            using (var wc = new System.Net.WebClient())
                try
                {
                    var t = await wc.DownloadStringTaskAsync(textBox1.Text + "?mode=version");
                    var version = int.Parse(t);
                    if (version > myversion)
                        label3.Text = "Inkompatible Version";
                    else label3.Text = "Server gefunden";
                }
                catch
                {
                    label3.Text = "Server nicht gefunden";
                }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = textBox2.Text;
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                textBox2.Text = folderBrowserDialog1.SelectedPath;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DirectoryInfo d;
            try { d = new DirectoryInfo(textBox2.Text); }
            catch
            {
                MessageBox.Show("ungültiger Zielpfad", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!d.Exists) d.Create();


        }
    }
}
