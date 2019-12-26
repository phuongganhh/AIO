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

namespace PacketEditor
{
    public partial class SocketEditor : Form
    {
        public SocketEditor()
        {
            InitializeComponent();
        }

        private void settingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var f = new SettingView();
            f.ShowDialog();
        }

        private void SocketEditor_Load(object sender, EventArgs e)
        {
            new Thread(() =>
            {

            }).Start();
        }
    }
}
