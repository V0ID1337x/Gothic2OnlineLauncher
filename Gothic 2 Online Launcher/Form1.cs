using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UpdateChecker;

namespace Gothic_2_Online_Launcher
{
    public partial class Launcher : Form
    {
        private bool mouseDown;
        private Point lastLocation;
        public static G2O_Proxy.Proxy proxy;
        // For "Update" button
        private int lastHeight;
        private int lastTop;
        private SidePanelAnimation sidePanelAnimation = null;
        public Launcher()
        {
            InitializeComponent();
            // Adding server to fav from serverListControl
            serverListControl1.favListReference = favListControl1;
        }

        private void Launcher_Load(object sender, EventArgs e)
        {
            serverListControl1.BringToFront();
            sidePanelAnimation = new SidePanelAnimation(SidePanel);
            Opacity = .97;
            try
            {
                proxy = new G2O_Proxy.Proxy();
                var v = proxy.version;
                verLabel.Text = $"ver. {v.Major}.{v.Minor}.{v.Patch}.{v.Build}";
                string downloadLink = "";
                if (new Checker("http://api.gothic-online.com.pl/update/version/").Check(out downloadLink))
                    MessageBox.Show(this, "Newer version is available!\nClick Update button to download!", "Update found.", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show(this, "Put launcher in G2O_Proxy.dll directory. Application will be closed.", "Error");
                Application.Exit();
            }
        }
        // Otwarcie strony internetowej
        private void PictureBox1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://gothic-online.com.pl");
        }

        private void SrvList_Click(object sender, EventArgs e)
        {
            serverListControl1.BringToFront();

            //SidePanel.Height = SrvList.Height;
            //SidePanel.Top = SrvList.Top;
            sidePanelAnimation.MoveTo(SrvList.Top, SrvList.Height);
        }

        private void FavList_Click(object sender, EventArgs e)
        {
            favListControl1.BringToFront();

            //SidePanel.Height = FavList.Height;
            //SidePanel.Top = FavList.Top;
            sidePanelAnimation.MoveTo(FavList.Top, FavList.Height);
        }

        private void Settings_Click(object sender, EventArgs e)
        {
            settingsControl1.BringToFront();

            //SidePanel.Height = Settings.Height;
            //SidePanel.Top = Settings.Top;
            sidePanelAnimation.MoveTo(Settings.Top, Settings.Height);
        }

        private void UpdateBtn_Click(object sender, EventArgs e)
        {
            lastHeight = SidePanel.Height;
            lastTop = SidePanel.Top;
            SidePanel.Height = updateBtn.Height;
            SidePanel.Top = updateBtn.Top;
            sidePanelAnimation.MoveTo(updateBtn.Top, updateBtn.Height);
            var dialog = new UpdateForm();
            if(dialog.ShowDialog(this) == DialogResult.OK)
            {

            }
            dialog.Dispose();
            //SidePanel.Height = lastHeight;
            //SidePanel.Top = lastTop;
            sidePanelAnimation.MoveTo(lastTop, lastHeight);
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            SidePanel.Height = Exit.Height;
            SidePanel.Top = Exit.Top;
            sidePanelAnimation.MoveTo(Exit.Top, Exit.Height);
            Thread.Sleep(100);
            Application.Exit();
        }

        private void Launcher_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void Launcher_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);

                this.Update();
            }
        }

        private void Launcher_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void TimerAni_Tick(object sender, EventArgs e)
        {
            if(sidePanelAnimation != null)
                sidePanelAnimation.Transform();
        }
    }

    class SidePanelAnimation
    {
        private Panel sidePanel;
        private int finalTop;
        private int finalHeight;
        public SidePanelAnimation(Panel SidePanel)
        {
            sidePanel = SidePanel;

            finalTop = sidePanel.Top;
            finalHeight = sidePanel.Height;
        }

        public void Transform()
        {
            if (sidePanel.Top != finalTop)
            {
                if (sidePanel.Top < finalTop)
                    sidePanel.Top += 4;
                else if (sidePanel.Top > finalTop) sidePanel.Top -= 4;
            }

            if(sidePanel.Height != finalHeight)
            {
                if (sidePanel.Height < finalHeight)
                    sidePanel.Height += 4;
                else if (sidePanel.Height > finalHeight) sidePanel.Height -= 4;
            }
        }
        
        public void MoveTo(int top, int height)
        {
            finalTop = top; finalHeight = height;
        }
    }
}
