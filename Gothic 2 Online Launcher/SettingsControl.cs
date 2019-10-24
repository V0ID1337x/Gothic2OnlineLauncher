using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gothic_2_Online_Launcher
{
    public partial class SettingsControl : UserControl
    {
        public SettingsControl()
        {
            InitializeComponent();
        }

        private void SettingsControl_Load(object sender, EventArgs e)
        {
            textBox1.Text = Config.GetNickname();
        }

        private void ApplyBtn_Click(object sender, EventArgs e)
        {
            Config.SetNickname(textBox1.Text);
        }
    }
}
