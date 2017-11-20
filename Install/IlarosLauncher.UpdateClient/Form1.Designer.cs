namespace IlarosLauncher.UpdateClient
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.panel1 = new System.Windows.Forms.Panel();
            this.clientVersion = new System.Windows.Forms.Label();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnForward = new System.Windows.Forms.Button();
            this.btnInstall = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.tablessControl1 = new MaxLib.WinForms.TablessControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.acceptlicense = new System.Windows.Forms.CheckBox();
            this.licensebox = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.showTargetDirectoryDialog = new System.Windows.Forms.Button();
            this.targetDirectory = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.useRegistry = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.useAppdata = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.useTemp = new System.Windows.Forms.CheckBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.optStartLauncher = new System.Windows.Forms.CheckBox();
            this.optCloseWindowAfterDownload = new System.Windows.Forms.CheckBox();
            this.optCreateDesktopLink = new System.Windows.Forms.CheckBox();
            this.optDownloadNewImages = new System.Windows.Forms.CheckBox();
            this.optDownloadImagesNow = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.optDownloadUpdates = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.optSearchForUpdates = new System.Windows.Forms.CheckBox();
            this.tabInstall = new System.Windows.Forms.TabPage();
            this.stageInfo2 = new System.Windows.Forms.Label();
            this.taskList = new MaxLib.WinForms.RefreshingListBox();
            this.progressBar2 = new System.Windows.Forms.ProgressBar();
            this.stageTask2 = new System.Windows.Forms.Label();
            this.stageName2 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.stageInfo1 = new System.Windows.Forms.Label();
            this.stageTask1 = new System.Windows.Forms.Label();
            this.stageName1 = new System.Windows.Forms.Label();
            this.tabFinish = new System.Windows.Forms.TabPage();
            this.startLauncher = new System.Windows.Forms.CheckBox();
            this.label14 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.stageUpdateTimer = new System.Windows.Forms.Timer();
            this.panel1.SuspendLayout();
            this.tablessControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabInstall.SuspendLayout();
            this.tabFinish.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.clientVersion);
            this.panel1.Controls.Add(this.btnBack);
            this.panel1.Controls.Add(this.btnForward);
            this.panel1.Controls.Add(this.btnInstall);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 468);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(774, 45);
            this.panel1.TabIndex = 0;
            // 
            // clientVersion
            // 
            this.clientVersion.AutoSize = true;
            this.clientVersion.Location = new System.Drawing.Point(3, 29);
            this.clientVersion.Name = "clientVersion";
            this.clientVersion.Size = new System.Drawing.Size(115, 13);
            this.clientVersion.TabIndex = 4;
            this.clientVersion.Text = "Update Client Version: ";
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(444, 10);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(75, 23);
            this.btnBack.TabIndex = 3;
            this.btnBack.Text = "Zurück";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnForward
            // 
            this.btnForward.Location = new System.Drawing.Point(525, 10);
            this.btnForward.Name = "btnForward";
            this.btnForward.Size = new System.Drawing.Size(75, 23);
            this.btnForward.TabIndex = 2;
            this.btnForward.Text = "Weiter";
            this.btnForward.UseVisualStyleBackColor = true;
            this.btnForward.Click += new System.EventHandler(this.btnForward_Click);
            // 
            // btnInstall
            // 
            this.btnInstall.Location = new System.Drawing.Point(606, 10);
            this.btnInstall.Name = "btnInstall";
            this.btnInstall.Size = new System.Drawing.Size(75, 23);
            this.btnInstall.TabIndex = 1;
            this.btnInstall.Text = "Installieren";
            this.btnInstall.UseVisualStyleBackColor = true;
            this.btnInstall.Click += new System.EventHandler(this.btnInstall_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(687, 10);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Beenden";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // tablessControl1
            // 
            this.tablessControl1.Controls.Add(this.tabPage1);
            this.tablessControl1.Controls.Add(this.tabPage2);
            this.tablessControl1.Controls.Add(this.tabPage3);
            this.tablessControl1.Controls.Add(this.tabInstall);
            this.tablessControl1.Controls.Add(this.tabFinish);
            this.tablessControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tablessControl1.Location = new System.Drawing.Point(0, 100);
            this.tablessControl1.Name = "tablessControl1";
            this.tablessControl1.SelectedIndex = 0;
            this.tablessControl1.Size = new System.Drawing.Size(774, 368);
            this.tablessControl1.TabIndex = 1;
            this.tablessControl1.SelectedIndexChanged += new System.EventHandler(this.tablessControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.acceptlicense);
            this.tabPage1.Controls.Add(this.licensebox);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(766, 342);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Lizenz";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // acceptlicense
            // 
            this.acceptlicense.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.acceptlicense.AutoSize = true;
            this.acceptlicense.Location = new System.Drawing.Point(8, 319);
            this.acceptlicense.Name = "acceptlicense";
            this.acceptlicense.Size = new System.Drawing.Size(217, 17);
            this.acceptlicense.TabIndex = 1;
            this.acceptlicense.Text = "Ich akzeptiere die Lizenzvereinbarungen";
            this.acceptlicense.UseVisualStyleBackColor = true;
            this.acceptlicense.CheckedChanged += new System.EventHandler(this.acceptlicense_CheckedChanged);
            // 
            // licensebox
            // 
            this.licensebox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.licensebox.Location = new System.Drawing.Point(8, 6);
            this.licensebox.Multiline = true;
            this.licensebox.Name = "licensebox";
            this.licensebox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.licensebox.Size = new System.Drawing.Size(750, 307);
            this.licensebox.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.showTargetDirectoryDialog);
            this.tabPage2.Controls.Add(this.targetDirectory);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.useRegistry);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.useAppdata);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.useTemp);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(766, 342);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Pfade";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // showTargetDirectoryDialog
            // 
            this.showTargetDirectoryDialog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.showTargetDirectoryDialog.Location = new System.Drawing.Point(723, 316);
            this.showTargetDirectoryDialog.Name = "showTargetDirectoryDialog";
            this.showTargetDirectoryDialog.Size = new System.Drawing.Size(33, 20);
            this.showTargetDirectoryDialog.TabIndex = 8;
            this.showTargetDirectoryDialog.Text = "...";
            this.showTargetDirectoryDialog.UseVisualStyleBackColor = true;
            this.showTargetDirectoryDialog.Click += new System.EventHandler(this.showTargetDirectoryDialog_Click);
            // 
            // targetDirectory
            // 
            this.targetDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.targetDirectory.Location = new System.Drawing.Point(6, 316);
            this.targetDirectory.Name = "targetDirectory";
            this.targetDirectory.Size = new System.Drawing.Size(711, 20);
            this.targetDirectory.TabIndex = 7;
            this.targetDirectory.TextChanged += new System.EventHandler(this.targetDirectory_TextChanged);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 291);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(137, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Speicherort des Launchers:";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.Enabled = false;
            this.label3.Location = new System.Drawing.Point(26, 146);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(732, 37);
            this.label3.TabIndex = 5;
            this.label3.Text = "Speichert die obrigen Einstellungen in die Registry.";
            // 
            // useRegistry
            // 
            this.useRegistry.AutoSize = true;
            this.useRegistry.Checked = true;
            this.useRegistry.CheckState = System.Windows.Forms.CheckState.Checked;
            this.useRegistry.Enabled = false;
            this.useRegistry.Location = new System.Drawing.Point(8, 126);
            this.useRegistry.Name = "useRegistry";
            this.useRegistry.Size = new System.Drawing.Size(99, 17);
            this.useRegistry.TabIndex = 4;
            this.useRegistry.Text = "Registry nutzen";
            this.useRegistry.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.Location = new System.Drawing.Point(26, 86);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(732, 37);
            this.label2.TabIndex = 3;
            this.label2.Text = "Speichert Einstellungen und Hintergründe in das dafür vorgesehene Systemverzeichn" +
    "is.";
            // 
            // useAppdata
            // 
            this.useAppdata.AutoSize = true;
            this.useAppdata.Checked = true;
            this.useAppdata.CheckState = System.Windows.Forms.CheckState.Checked;
            this.useAppdata.Location = new System.Drawing.Point(8, 66);
            this.useAppdata.Name = "useAppdata";
            this.useAppdata.Size = new System.Drawing.Size(127, 17);
            this.useAppdata.TabIndex = 2;
            this.useAppdata.Text = "%APPDATA% nutzen";
            this.useAppdata.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(26, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(732, 37);
            this.label1.TabIndex = 1;
            this.label1.Text = "Speichert temporäre Daten ins dafür vorgesehene Systemverzeichnis.";
            // 
            // useTemp
            // 
            this.useTemp.AutoSize = true;
            this.useTemp.Checked = true;
            this.useTemp.CheckState = System.Windows.Forms.CheckState.Checked;
            this.useTemp.Location = new System.Drawing.Point(8, 6);
            this.useTemp.Name = "useTemp";
            this.useTemp.Size = new System.Drawing.Size(107, 17);
            this.useTemp.TabIndex = 0;
            this.useTemp.Text = "%TEMP% nutzen";
            this.useTemp.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.AutoScroll = true;
            this.tabPage3.Controls.Add(this.optStartLauncher);
            this.tabPage3.Controls.Add(this.optCloseWindowAfterDownload);
            this.tabPage3.Controls.Add(this.optCreateDesktopLink);
            this.tabPage3.Controls.Add(this.optDownloadNewImages);
            this.tabPage3.Controls.Add(this.optDownloadImagesNow);
            this.tabPage3.Controls.Add(this.label8);
            this.tabPage3.Controls.Add(this.optDownloadUpdates);
            this.tabPage3.Controls.Add(this.label5);
            this.tabPage3.Controls.Add(this.optSearchForUpdates);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(766, 342);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Einstellungen";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // optStartLauncher
            // 
            this.optStartLauncher.AutoSize = true;
            this.optStartLauncher.Checked = true;
            this.optStartLauncher.CheckState = System.Windows.Forms.CheckState.Checked;
            this.optStartLauncher.Location = new System.Drawing.Point(23, 222);
            this.optStartLauncher.Name = "optStartLauncher";
            this.optStartLauncher.Size = new System.Drawing.Size(184, 17);
            this.optStartLauncher.TabIndex = 8;
            this.optStartLauncher.Text = "Launcher nach Download starten";
            this.optStartLauncher.UseVisualStyleBackColor = true;
            this.optStartLauncher.CheckedChanged += new System.EventHandler(this.optStartLauncher_CheckedChanged);
            // 
            // optCloseWindowAfterDownload
            // 
            this.optCloseWindowAfterDownload.AutoSize = true;
            this.optCloseWindowAfterDownload.Location = new System.Drawing.Point(23, 199);
            this.optCloseWindowAfterDownload.Name = "optCloseWindowAfterDownload";
            this.optCloseWindowAfterDownload.Size = new System.Drawing.Size(272, 17);
            this.optCloseWindowAfterDownload.TabIndex = 7;
            this.optCloseWindowAfterDownload.Text = "Fenster nach Beendigung des Downloads schließen";
            this.optCloseWindowAfterDownload.UseVisualStyleBackColor = true;
            // 
            // optCreateDesktopLink
            // 
            this.optCreateDesktopLink.AutoSize = true;
            this.optCreateDesktopLink.Checked = true;
            this.optCreateDesktopLink.CheckState = System.Windows.Forms.CheckState.Checked;
            this.optCreateDesktopLink.Location = new System.Drawing.Point(23, 176);
            this.optCreateDesktopLink.Name = "optCreateDesktopLink";
            this.optCreateDesktopLink.Size = new System.Drawing.Size(167, 17);
            this.optCreateDesktopLink.TabIndex = 6;
            this.optCreateDesktopLink.Text = "Desktopverknüpfung anlegen";
            this.optCreateDesktopLink.UseVisualStyleBackColor = true;
            // 
            // optDownloadNewImages
            // 
            this.optDownloadNewImages.AutoSize = true;
            this.optDownloadNewImages.Checked = true;
            this.optDownloadNewImages.CheckState = System.Windows.Forms.CheckState.Checked;
            this.optDownloadNewImages.Location = new System.Drawing.Point(23, 75);
            this.optDownloadNewImages.Name = "optDownloadNewImages";
            this.optDownloadNewImages.Size = new System.Drawing.Size(242, 17);
            this.optDownloadNewImages.TabIndex = 5;
            this.optDownloadNewImages.Text = "neue Hintergründe automatisch herunterladen";
            this.optDownloadNewImages.UseVisualStyleBackColor = true;
            // 
            // optDownloadImagesNow
            // 
            this.optDownloadImagesNow.AutoSize = true;
            this.optDownloadImagesNow.Checked = true;
            this.optDownloadImagesNow.CheckState = System.Windows.Forms.CheckState.Checked;
            this.optDownloadImagesNow.Location = new System.Drawing.Point(23, 153);
            this.optDownloadImagesNow.Name = "optDownloadImagesNow";
            this.optDownloadImagesNow.Size = new System.Drawing.Size(263, 17);
            this.optDownloadImagesNow.TabIndex = 4;
            this.optDownloadImagesNow.Text = "alle verfügbaren Hintergründe sofort herunterladen";
            this.optDownloadImagesNow.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 125);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(235, 13);
            this.label8.TabIndex = 3;
            this.label8.Text = "Folgende Einstellungen betreffen die Installation:";
            // 
            // optDownloadUpdates
            // 
            this.optDownloadUpdates.AutoSize = true;
            this.optDownloadUpdates.Checked = true;
            this.optDownloadUpdates.CheckState = System.Windows.Forms.CheckState.Checked;
            this.optDownloadUpdates.Location = new System.Drawing.Point(23, 52);
            this.optDownloadUpdates.Name = "optDownloadUpdates";
            this.optDownloadUpdates.Size = new System.Drawing.Size(194, 17);
            this.optDownloadUpdates.TabIndex = 2;
            this.optDownloadUpdates.Text = "Updates automatisch herunterladen";
            this.optDownloadUpdates.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 3);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(275, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Folgende Einstellungen sind auch im Launcher änderbar:";
            // 
            // optSearchForUpdates
            // 
            this.optSearchForUpdates.AutoSize = true;
            this.optSearchForUpdates.Checked = true;
            this.optSearchForUpdates.CheckState = System.Windows.Forms.CheckState.Checked;
            this.optSearchForUpdates.Location = new System.Drawing.Point(23, 29);
            this.optSearchForUpdates.Name = "optSearchForUpdates";
            this.optSearchForUpdates.Size = new System.Drawing.Size(191, 17);
            this.optSearchForUpdates.TabIndex = 0;
            this.optSearchForUpdates.Text = "automatisch nach Updates suchen";
            this.optSearchForUpdates.UseVisualStyleBackColor = true;
            // 
            // tabInstall
            // 
            this.tabInstall.Controls.Add(this.stageInfo2);
            this.tabInstall.Controls.Add(this.taskList);
            this.tabInstall.Controls.Add(this.progressBar2);
            this.tabInstall.Controls.Add(this.stageTask2);
            this.tabInstall.Controls.Add(this.stageName2);
            this.tabInstall.Controls.Add(this.progressBar1);
            this.tabInstall.Controls.Add(this.stageInfo1);
            this.tabInstall.Controls.Add(this.stageTask1);
            this.tabInstall.Controls.Add(this.stageName1);
            this.tabInstall.Location = new System.Drawing.Point(4, 22);
            this.tabInstall.Name = "tabInstall";
            this.tabInstall.Padding = new System.Windows.Forms.Padding(3);
            this.tabInstall.Size = new System.Drawing.Size(766, 342);
            this.tabInstall.TabIndex = 3;
            this.tabInstall.Text = "Download";
            this.tabInstall.UseVisualStyleBackColor = true;
            // 
            // stageInfo2
            // 
            this.stageInfo2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.stageInfo2.AutoSize = true;
            this.stageInfo2.Location = new System.Drawing.Point(717, 50);
            this.stageInfo2.Name = "stageInfo2";
            this.stageInfo2.Size = new System.Drawing.Size(40, 13);
            this.stageInfo2.TabIndex = 8;
            this.stageInfo2.Text = "0 KB/s";
            this.stageInfo2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // taskList
            // 
            this.taskList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.taskList.FormattingEnabled = true;
            this.taskList.Location = new System.Drawing.Point(6, 95);
            this.taskList.Name = "taskList";
            this.taskList.Size = new System.Drawing.Size(751, 238);
            this.taskList.TabIndex = 7;
            // 
            // progressBar2
            // 
            this.progressBar2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar2.Location = new System.Drawing.Point(6, 66);
            this.progressBar2.Maximum = 10000;
            this.progressBar2.Name = "progressBar2";
            this.progressBar2.Size = new System.Drawing.Size(751, 23);
            this.progressBar2.TabIndex = 6;
            // 
            // stageTask2
            // 
            this.stageTask2.AutoSize = true;
            this.stageTask2.Location = new System.Drawing.Point(87, 50);
            this.stageTask2.Name = "stageTask2";
            this.stageTask2.Size = new System.Drawing.Size(41, 13);
            this.stageTask2.TabIndex = 5;
            this.stageTask2.Text = "Datei...";
            // 
            // stageName2
            // 
            this.stageName2.AutoSize = true;
            this.stageName2.Location = new System.Drawing.Point(8, 50);
            this.stageName2.Name = "stageName2";
            this.stageName2.Size = new System.Drawing.Size(54, 13);
            this.stageName2.TabIndex = 4;
            this.stageName2.Text = "Installiere:";
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(6, 19);
            this.progressBar1.Maximum = 10000;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(751, 23);
            this.progressBar1.TabIndex = 3;
            // 
            // stageInfo1
            // 
            this.stageInfo1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.stageInfo1.AutoSize = true;
            this.stageInfo1.Location = new System.Drawing.Point(717, 3);
            this.stageInfo1.Name = "stageInfo1";
            this.stageInfo1.Size = new System.Drawing.Size(40, 13);
            this.stageInfo1.TabIndex = 2;
            this.stageInfo1.Text = "0 KB/s";
            this.stageInfo1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // stageTask1
            // 
            this.stageTask1.AutoSize = true;
            this.stageTask1.Location = new System.Drawing.Point(87, 3);
            this.stageTask1.Name = "stageTask1";
            this.stageTask1.Size = new System.Drawing.Size(41, 13);
            this.stageTask1.TabIndex = 1;
            this.stageTask1.Text = "Datei...";
            // 
            // stageName1
            // 
            this.stageName1.AutoSize = true;
            this.stageName1.Location = new System.Drawing.Point(8, 3);
            this.stageName1.Name = "stageName1";
            this.stageName1.Size = new System.Drawing.Size(58, 13);
            this.stageName1.TabIndex = 0;
            this.stageName1.Text = "Download:";
            // 
            // tabFinish
            // 
            this.tabFinish.Controls.Add(this.startLauncher);
            this.tabFinish.Controls.Add(this.label14);
            this.tabFinish.Location = new System.Drawing.Point(4, 22);
            this.tabFinish.Name = "tabFinish";
            this.tabFinish.Padding = new System.Windows.Forms.Padding(3);
            this.tabFinish.Size = new System.Drawing.Size(766, 342);
            this.tabFinish.TabIndex = 4;
            this.tabFinish.Text = "Fertig";
            this.tabFinish.UseVisualStyleBackColor = true;
            // 
            // startLauncher
            // 
            this.startLauncher.AutoSize = true;
            this.startLauncher.Checked = true;
            this.startLauncher.CheckState = System.Windows.Forms.CheckState.Checked;
            this.startLauncher.Location = new System.Drawing.Point(28, 33);
            this.startLauncher.Name = "startLauncher";
            this.startLauncher.Size = new System.Drawing.Size(181, 17);
            this.startLauncher.TabIndex = 1;
            this.startLauncher.Text = "Launcher beim Schließen starten";
            this.startLauncher.UseVisualStyleBackColor = true;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(8, 3);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(203, 13);
            this.label14.TabIndex = 0;
            this.label14.Text = "Der Launcher wurde vollständig installiert.";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.Window;
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(774, 100);
            this.panel2.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(100, 0);
            this.label6.Name = "label6";
            this.label6.Padding = new System.Windows.Forms.Padding(15, 5, 5, 5);
            this.label6.Size = new System.Drawing.Size(674, 100);
            this.label6.TabIndex = 1;
            this.label6.Text = "Ilaros Launcher";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox1.Image = global::IlarosLauncher.UpdateClient.Properties.Resources.wow_icon_transparent;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 100);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // stageUpdateTimer
            // 
            this.stageUpdateTimer.Tick += new System.EventHandler(this.stageUpdateTimer_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(774, 513);
            this.Controls.Add(this.tablessControl1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ilaros Launcher - Update Client";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tablessControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabInstall.ResumeLayout(false);
            this.tabInstall.PerformLayout();
            this.tabFinish.ResumeLayout(false);
            this.tabFinish.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnForward;
        private System.Windows.Forms.Button btnInstall;
        private System.Windows.Forms.Button btnClose;
        private MaxLib.WinForms.TablessControl tablessControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.CheckBox acceptlicense;
        private System.Windows.Forms.TextBox licensebox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox useRegistry;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox useAppdata;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox useTemp;
        private System.Windows.Forms.Label clientVersion;
        private System.Windows.Forms.Button showTargetDirectoryDialog;
        private System.Windows.Forms.TextBox targetDirectory;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.CheckBox optStartLauncher;
        private System.Windows.Forms.CheckBox optCloseWindowAfterDownload;
        private System.Windows.Forms.CheckBox optCreateDesktopLink;
        private System.Windows.Forms.CheckBox optDownloadNewImages;
        private System.Windows.Forms.CheckBox optDownloadImagesNow;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox optDownloadUpdates;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox optSearchForUpdates;
        private System.Windows.Forms.TabPage tabInstall;
        private MaxLib.WinForms.RefreshingListBox taskList;
        private System.Windows.Forms.ProgressBar progressBar2;
        private System.Windows.Forms.Label stageTask2;
        private System.Windows.Forms.Label stageName2;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label stageInfo1;
        private System.Windows.Forms.Label stageTask1;
        private System.Windows.Forms.Label stageName1;
        private System.Windows.Forms.TabPage tabFinish;
        private System.Windows.Forms.CheckBox startLauncher;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label stageInfo2;
        private System.Windows.Forms.Timer stageUpdateTimer;
    }
}

