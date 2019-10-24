namespace Gothic_2_Online_Launcher
{
    partial class Launcher
    {
        /// <summary>
        /// Wymagana zmienna projektanta.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Wyczyść wszystkie używane zasoby.
        /// </summary>
        /// <param name="disposing">prawda, jeżeli zarządzane zasoby powinny zostać zlikwidowane; Fałsz w przeciwnym wypadku.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kod generowany przez Projektanta formularzy systemu Windows

        /// <summary>
        /// Metoda wymagana do obsługi projektanta — nie należy modyfikować
        /// jej zawartości w edytorze kodu.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Launcher));
            this.panel1 = new System.Windows.Forms.Panel();
            this.updateBtn = new System.Windows.Forms.Button();
            this.verLabel = new System.Windows.Forms.Label();
            this.SidePanel = new System.Windows.Forms.Panel();
            this.Exit = new System.Windows.Forms.Button();
            this.Settings = new System.Windows.Forms.Button();
            this.FavList = new System.Windows.Forms.Button();
            this.SrvList = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.timerAni = new System.Windows.Forms.Timer(this.components);
            this.settingsControl1 = new Gothic_2_Online_Launcher.SettingsControl();
            this.favListControl1 = new Gothic_2_Online_Launcher.FavListControl();
            this.serverListControl1 = new Gothic_2_Online_Launcher.ServerListControl();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.panel1.Controls.Add(this.updateBtn);
            this.panel1.Controls.Add(this.verLabel);
            this.panel1.Controls.Add(this.SidePanel);
            this.panel1.Controls.Add(this.Exit);
            this.panel1.Controls.Add(this.Settings);
            this.panel1.Controls.Add(this.FavList);
            this.panel1.Controls.Add(this.SrvList);
            this.panel1.Controls.Add(this.pictureBox1);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Launcher_MouseDown);
            this.panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Launcher_MouseMove);
            this.panel1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Launcher_MouseUp);
            // 
            // updateBtn
            // 
            this.updateBtn.FlatAppearance.BorderSize = 0;
            resources.ApplyResources(this.updateBtn, "updateBtn");
            this.updateBtn.ForeColor = System.Drawing.Color.White;
            this.updateBtn.Image = global::Gothic_2_Online_Launcher.Properties.Resources.update_25px;
            this.updateBtn.Name = "updateBtn";
            this.updateBtn.UseVisualStyleBackColor = true;
            this.updateBtn.Click += new System.EventHandler(this.UpdateBtn_Click);
            // 
            // verLabel
            // 
            resources.ApplyResources(this.verLabel, "verLabel");
            this.verLabel.ForeColor = System.Drawing.Color.White;
            this.verLabel.Name = "verLabel";
            // 
            // SidePanel
            // 
            this.SidePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(253)))), ((int)(((byte)(255)))));
            resources.ApplyResources(this.SidePanel, "SidePanel");
            this.SidePanel.Name = "SidePanel";
            // 
            // Exit
            // 
            this.Exit.FlatAppearance.BorderSize = 0;
            resources.ApplyResources(this.Exit, "Exit");
            this.Exit.ForeColor = System.Drawing.Color.White;
            this.Exit.Image = global::Gothic_2_Online_Launcher.Properties.Resources.exit_25px;
            this.Exit.Name = "Exit";
            this.Exit.UseVisualStyleBackColor = true;
            this.Exit.Click += new System.EventHandler(this.Exit_Click);
            // 
            // Settings
            // 
            this.Settings.FlatAppearance.BorderSize = 0;
            resources.ApplyResources(this.Settings, "Settings");
            this.Settings.ForeColor = System.Drawing.Color.White;
            this.Settings.Image = global::Gothic_2_Online_Launcher.Properties.Resources.settings_25px;
            this.Settings.Name = "Settings";
            this.Settings.UseVisualStyleBackColor = true;
            this.Settings.Click += new System.EventHandler(this.Settings_Click);
            // 
            // FavList
            // 
            this.FavList.FlatAppearance.BorderSize = 0;
            resources.ApplyResources(this.FavList, "FavList");
            this.FavList.ForeColor = System.Drawing.Color.White;
            this.FavList.Image = global::Gothic_2_Online_Launcher.Properties.Resources.favorites_25px;
            this.FavList.Name = "FavList";
            this.FavList.UseVisualStyleBackColor = true;
            this.FavList.Click += new System.EventHandler(this.FavList_Click);
            // 
            // SrvList
            // 
            this.SrvList.FlatAppearance.BorderSize = 0;
            resources.ApplyResources(this.SrvList, "SrvList");
            this.SrvList.ForeColor = System.Drawing.Color.White;
            this.SrvList.Image = global::Gothic_2_Online_Launcher.Properties.Resources.serverlist_25px;
            this.SrvList.Name = "SrvList";
            this.SrvList.UseVisualStyleBackColor = true;
            this.SrvList.Click += new System.EventHandler(this.SrvList_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::Gothic_2_Online_Launcher.Properties.Resources.LOGO_v2_4_CIENIE_NotR_SMALL;
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.PictureBox1_Click);
            // 
            // timerAni
            // 
            this.timerAni.Enabled = true;
            this.timerAni.Interval = 1;
            this.timerAni.Tick += new System.EventHandler(this.TimerAni_Tick);
            // 
            // settingsControl1
            // 
            resources.ApplyResources(this.settingsControl1, "settingsControl1");
            this.settingsControl1.Name = "settingsControl1";
            this.settingsControl1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Launcher_MouseDown);
            this.settingsControl1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Launcher_MouseMove);
            this.settingsControl1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Launcher_MouseUp);
            // 
            // favListControl1
            // 
            resources.ApplyResources(this.favListControl1, "favListControl1");
            this.favListControl1.ForeColor = System.Drawing.Color.White;
            this.favListControl1.Name = "favListControl1";
            this.favListControl1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Launcher_MouseDown);
            this.favListControl1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Launcher_MouseMove);
            this.favListControl1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Launcher_MouseUp);
            // 
            // serverListControl1
            // 
            resources.ApplyResources(this.serverListControl1, "serverListControl1");
            this.serverListControl1.Name = "serverListControl1";
            this.serverListControl1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Launcher_MouseDown);
            this.serverListControl1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Launcher_MouseMove);
            this.serverListControl1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Launcher_MouseUp);
            // 
            // Launcher
            // 
            this.AllowDrop = true;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.settingsControl1);
            this.Controls.Add(this.favListControl1);
            this.Controls.Add(this.serverListControl1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Launcher";
            this.Load += new System.EventHandler(this.Launcher_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Launcher_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Launcher_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Launcher_MouseUp);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button SrvList;
        private System.Windows.Forms.Button Exit;
        private System.Windows.Forms.Button Settings;
        private System.Windows.Forms.Button FavList;
        private System.Windows.Forms.Panel SidePanel;
        private ServerListControl serverListControl1;
        private FavListControl favListControl1;
        private SettingsControl settingsControl1;
        private System.Windows.Forms.Label verLabel;
        private System.Windows.Forms.Button updateBtn;
        private System.Windows.Forms.Timer timerAni;
    }
}

