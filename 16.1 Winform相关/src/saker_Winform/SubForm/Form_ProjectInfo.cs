using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace saker_Winform.SubForm
{
    public partial class Form_ProjectInfo : Form
    {
        public Form_ProjectInfo()
        {
            InitializeComponent();
        }

        private void Form_ProjectInfo_Load(object sender, EventArgs e)
        {
            this.label_ProjectName.Text = Module.Module_DeviceManage.Instance.ProjectName;
            this.label_RemarkLoad.Text = Module.Module_DeviceManage.Instance.ProjectRemark;
        }

        private void label_Name_Click(object sender, EventArgs e)
        {

        }
    }
}
