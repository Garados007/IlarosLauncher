using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IlarosLauncher
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            webBrowser1.DocumentText = Properties.Resources.preload;
            Load += Form1_Load;
            FormClosing += Form1_FormClosing;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Server.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            AsyncLoad();
        }

        void AsyncLoad()
        {
            new Task(() =>
            {
                Server.Start();
            }).Start();
        }
    }
}
