using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using ServerManager;
using System.IO;

namespace Gothic_2_Online_Launcher
{
    public partial class FavListControl : UserControl
    {
        private List<string> serverAddresses = new List<string>();
        private NetworkList networkList;
        public FavListControl()
        {
            InitializeComponent();
        }

        private void FavListControl_Load(object sender, EventArgs e)
        {
            if(File.Exists("fav.list"))
            {
                serverAddresses = File.ReadAllLines("fav.list").ToList();
                RefreshServers();
            }
        }

        private void ConnectBtn_Click(object sender, EventArgs e)
        {
            if(listView1.SelectedItems.Count != 0)
            {
                var address = listView1.SelectedItems[0].SubItems[1].Text;
                var ver = listView1.SelectedItems[0].SubItems[2].Text.Split('.');

                JoinServer(Int32.Parse(ver[0]), Int32.Parse(ver[1]), Int32.Parse(ver[2]), address); 
            }
        }

        private void ListView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView1.SelectedItems.Count != 0)
            {
                var address = listView1.SelectedItems[0].SubItems[1].Text;
                var ver = listView1.SelectedItems[0].SubItems[2].Text.Split('.');

                JoinServer(Int32.Parse(ver[0]), Int32.Parse(ver[1]), Int32.Parse(ver[2]), address);
            }
        }

        private void RemoveBtn_Click(object sender, EventArgs e)
        {
            RemoveBtn.Enabled = false;
            Remove();
            RemoveBtn.Enabled = true;
        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            AddBtn.Enabled = false;
            Add();
            AddBtn.Enabled = true;
        }

        private void RefreshBtn_Click(object sender, EventArgs e)
        {
            RefreshServers();
        }

        private void RefreshServers()
        {
            RefreshBtn.Enabled = false;
            networkList = new NetworkList();
            if (networkList.InitSocket())
            {
                networkList.Start();
                // Sprawdzamy dla pewnosci ;)
                var regex = new Regex(@"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}:\d{1,5}$");
                foreach (var item in serverAddresses)
                    if (regex.IsMatch(item))
                        networkList.ServerData(item);

                Application.UseWaitCursor = true;
                timerRefresh.Start();
            }
            else RefreshBtn.Enabled = true;
            
        }
        public void AddToFav(string address)
        {
            var regex = new Regex(@"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}:\d{1,5}$");
            if (regex.IsMatch(address))
            {
                if (serverAddresses.Contains(address) == false)
                {
                    serverAddresses.Add(address);
                    RefreshServers();
                    File.WriteAllLines("fav.list", serverAddresses.ToArray());
                }
            }
        }
        private void Add()
        {
            var dialog = new FavListAdd();
            if(dialog.ShowDialog(this) == DialogResult.OK)
            {
                var address = dialog.addr.Text;
                var regex = new Regex(@"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}:\d{1,5}$");
                if (regex.IsMatch(address))
                {
                    if (serverAddresses.Contains(address) == false)
                    {
                        serverAddresses.Add(address);
                        RefreshServers();
                        File.WriteAllLines("fav.list", serverAddresses.ToArray());
                    }
                }
                else MessageBox.Show(this, "Enter valid IP address!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            dialog.Dispose();
        }

        private void Remove()
        {
            if(listView1.SelectedItems.Count != 0)
            {
                var result = MessageBox.Show(this, "Do you really want to remove this server?", "Ru sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    var item = listView1.SelectedItems[0];
                    serverAddresses.Remove(item.SubItems[1].Text);

                    RefreshServers();
                    if (serverAddresses.Count > 0)
                        File.WriteAllLines("fav.list", serverAddresses.ToArray());
                    else File.Delete("fav.list");
                }
            }
        }

        private void TimerRefresh_Tick(object sender, EventArgs e)
        {
            timerRefresh.Stop();
            networkList.Stop();
            listView1.Items.Clear();
            foreach(var item in networkList.ServerList)
            {
                string[] row = { item.Value.hostname, item.Value.address.ToString(), item.Value.version.ToString(), $"{item.Value.players}/{item.Value.playersMax}", item.Value.ping.ToString() };
                listView1.Items.Add(new ListViewItem(row));
            }
            foreach(var item in networkList.FailedList)
            {
                string[] row = { item.Value.hostname, item.Value.address.ToString(), item.Value.version.ToString(), $"{item.Value.players}/{item.Value.playersMax}", item.Value.ping.ToString() };
                listView1.Items.Add(new ListViewItem(row));
            }
            Application.UseWaitCursor = false;
            Cursor.Current = Cursors.Default;
            RefreshBtn.Enabled = true;
        }

        private void JoinServer(int major, int minor, int patch, string address)
        {
            ConnectBtn.Enabled = false;
            Config.SetAddress(address);
            try
            {
                Launcher.proxy.Run(major, minor, patch);
            }
            catch(Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Cannot connect to server!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ConnectBtn.Enabled = true;
            }
        }
    }
}
