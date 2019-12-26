using PacketEditor.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PacketEditor
{
    public partial class SettingView : Form
    {
        public SettingView()
        {
            InitializeComponent();
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var isCheck = (CheckedListBox)sender;
            if(isCheck.SelectedIndex == 0)
            {
                Setting.isSend = !Setting.isSend;
            }
            else
            {
                Setting.isRecv = !Setting.isRecv;
            }
        }

        private void SettingView_Load(object sender, EventArgs e)
        {
        }
    }
}
