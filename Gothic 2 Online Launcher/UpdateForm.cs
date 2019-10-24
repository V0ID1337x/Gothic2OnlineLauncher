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
    public partial class UpdateForm : Form
    {
        public UpdateForm()
        {
            InitializeComponent();
        }

        private void UpdateForm_Load(object sender, EventArgs e)
        {
            string downloadLink = "";
            if(new Checker("http://api.gothic-online.com.pl/update/version/").Check(out downloadLink))
            {
                Thread.Sleep(100);
                Application.Restart();
            }
            else
            {
                updateProgress.Increment(100);
                timerUpdate.Start();
            }
        }

        private void TimerUpdate_Tick(object sender, EventArgs e)
        {
            timerUpdate.Stop();
            MessageBox.Show(this, "You already have the latest version.", "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
        }
    }
}
