using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ServerManager;

namespace Gothic_2_Online_Launcher
{
    public partial class ServerListControl : UserControl
    {
        public FavListControl favListReference { get; set; }
        private NetworkList networkList;
        public ServerListControl()
        {
            InitializeComponent();
        }

        private void RefreshBtn_Click(object sender, EventArgs e)
        {
            RefreshServers();
        }

        private void RefreshServers()
        {
            RefreshBtn.Enabled = false;
            var masterList = new MasterList("http://185.5.97.181:8000/master/public_servers");
            if (masterList.ListGot)
            {
                networkList = new NetworkList();
                if (networkList.InitSocket())
                {
                    Application.UseWaitCursor = true;
                    networkList.Start();
                    foreach (var server in masterList.Servers)
                        networkList.ServerData(server);
                    timerRefresh.Start();
                }
                else RefreshBtn.Enabled = true;
            }
            else RefreshBtn.Enabled = true;
        }

        private void ConnectBtn_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 0)
            {
                ConnectBtn.Enabled = false;

                Config.SetAddress(listView1.SelectedItems[0].SubItems[1].Text);

                try
                {
                    var ver = listView1.SelectedItems[0].SubItems[2].Text.Split('.');
                    Launcher.proxy.Run(Int32.Parse(ver[0]), Int32.Parse(ver[1]), Int32.Parse(ver[2]));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message, "Cannot connect to server!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    ConnectBtn.Enabled = true;
                }
            }
        }

        private void ListView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView1.SelectedItems.Count != 0)
            {
                ConnectBtn.Enabled = false;

                Config.SetAddress(listView1.SelectedItems[0].SubItems[1].Text);

                try
                {
                    var ver = listView1.SelectedItems[0].SubItems[2].Text.Split('.');
                    Launcher.proxy.Run(Int32.Parse(ver[0]), Int32.Parse(ver[1]), Int32.Parse(ver[2]));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message, "Cannot connect to server!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    ConnectBtn.Enabled = true;
                }
            }
        }

        private void TimerRefresh_Tick(object sender, EventArgs e)
        {
            networkList.Stop();
            listView1.Items.Clear();
            timerRefresh.Stop();
            foreach (var item in networkList.ServerList)
            {
                string[] row = { item.Value.hostname, item.Value.address.ToString(), item.Value.version.ToString(), $"{item.Value.players}/{item.Value.playersMax}", item.Value.ping.ToString() };
                listView1.Items.Add(new ListViewItem(row));
            }
            Application.UseWaitCursor = false;
            Cursor.Current = Cursors.Default;
            RefreshBtn.Enabled = true;
        }

        private void ServerListControl_Load(object sender, EventArgs e)
        {
            RefreshServers();
        }

        private void ListView1_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                if (listView1.FocusedItem.Bounds.Contains(e.Location))
                    rightClick.Show(Cursor.Position);
            }
        }
        private void RightClick_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            // Póki jest jeden item to tylko dodanie do ulubionych tu robie
            var address = listView1.SelectedItems[0].SubItems[1].Text;

            favListReference.AddToFav(address);
        }
    }
}
