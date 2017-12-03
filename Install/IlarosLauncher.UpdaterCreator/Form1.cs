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
using System.CodeDom.Compiler;
using Microsoft.CSharp;

namespace IlarosLauncher.UpdaterCreator
{
    public partial class Form1 : Form
    {
        const int myversion = 1;
        public Form1()
        {
            InitializeComponent();
            textBox2.Text = Environment.CurrentDirectory;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
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

            if (!File.Exists("Content\\DisplayDownload.cs"))
            {
                MessageBox.Show("DisplayDownload.cs nicht gefunden!", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!Directory.Exists("Temp")) Directory.CreateDirectory("Temp");

            var sb = new StringBuilder();
            sb.Append(@"namespace IlarosLauncher.Update
{
    public class DownloadSetting
    {
        public const string ServerType = """);
            switch (comboBox1.SelectedIndex)
            {
                case 0: sb.Append("ApachePHPV1"); break;
            }
            sb.Append(@""";

        public const string ServerUrl = @""");
            sb.Append(textBox1.Text);
            sb.Append(@""";

        public const string ImgSourceType = """);
            switch (comboBox2.SelectedIndex)
            {
                case 0: sb.Append("Direct"); break;
                case 1: sb.Append("ExtSourceOverCount"); break;
            }
            sb.Append(@""";

        public const string ImgCountLink = @""");
            sb.Append(textBox3.Text);
            sb.Append(@""";

        public const string ImgFileLink = @""");
            sb.Append(textBox4.Text);
            sb.Append(@""";
    }
}");
            File.WriteAllText("Content\\DownloadSettings.cs", sb.ToString());

            using (var mcp = new CSharpCodeProvider())
            {
                var cp = new CompilerParameters
                {
                    TempFiles = new TempFileCollection(Environment.CurrentDirectory + "\\Temp", false),
                    IncludeDebugInformation = false,
                    GenerateExecutable = true,
                    CompilerOptions = "/target:winexe /win32icon:Content\\wow-icon-transparent.ico",
                    OutputAssembly = new FileInfo(textBox2.Text + "\\IlarosLauncher.Update.exe").FullName,
                };
                cp.EmbeddedResources.Add("Content\\IlarosLauncher.Update.DisplayDownload.resources");
                cp.ReferencedAssemblies.Add(typeof(Form).Assembly.Location);
                cp.ReferencedAssemblies.Add(typeof(Point).Assembly.Location);
                cp.ReferencedAssemblies.Add(typeof(System.Deployment.Application.ApplicationDeployment).Assembly.Location);
                cp.ReferencedAssemblies.Add(typeof(Component).Assembly.Location);
                cp.ReferencedAssemblies.Add(typeof(System.IO.Compression.ZipFile).Assembly.Location);
                cp.ReferencedAssemblies.Add(typeof(System.IO.Compression.ZipArchive).Assembly.Location);

                var result = mcp.CompileAssemblyFromFile(cp, "Content\\DisplayDownload.cs", "Content\\DownloadSettings.cs");
                if (result.Errors.HasErrors)
                {
                    sb.Clear();
                    foreach (var err in result.Errors)
                    {
                        sb.Append(err);
                        sb.AppendLine();
                    }
                    MessageBox.Show("Updater konnte nicht erstellt werden.\n\nFehlermeldungen:" + sb.ToString(),
                        Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                MessageBox.Show("Updater wurde erfolgreich erstellt.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox3.Enabled = button4.Enabled = textBox4.Enabled = comboBox2.SelectedIndex == 1;
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            using (var wc = new System.Net.WebClient())
                try
                {
                    var t = await wc.DownloadStringTaskAsync(textBox3.Text);
                    int c;
                    if (int.TryParse(t, out c))
                        label5.Text = "Server gefunden";
                    else label5.Text = "Ungültige Rückgabe";
                }
                catch
                {
                    label5.Text = "Server konnte nicht gefunden werden";
                }
        }
    }
}
