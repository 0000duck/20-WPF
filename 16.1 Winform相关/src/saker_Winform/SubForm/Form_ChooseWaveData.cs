using ClassLibrary_MultiLanguage;
using saker_Winform.DataBase;
using saker_Winform.Module;
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
    public partial class Form_ChooseWaveData : Form
    {
        public string strHistroyTime = "";
        public Form_ChooseWaveData()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
        }

        private void Form_ChooseWaveData_Load(object sender, EventArgs e)
        {
            this.txt_projectname.Text= Module_DeviceManage.Instance.ProjectName;
            string waveTableName = Module_DeviceManage.Instance.WaveTableName;

           

            string sqlQueryStartTime = " SELECT distinct Convert(varchar, StartTime, 120) AS StartTime FROM " + waveTableName + " ORDER BY StartTime DESC";

            DataTable dt = new DataTable();

            dt = DbHelperSql.QueryDt(sqlQueryStartTime);
            this.comboBox_startTime.DataSource = dt;
            this.comboBox_startTime.DisplayMember = "StartTime";
            this.comboBox_startTime.SelectedIndex = -1;
            this.ActiveControl = this.comboBox_startTime;
            this.comboBox_startTime.Focus();
           
        }

        private void button_import_Click(object sender, EventArgs e)
        {
            string waveTableName = Module_DeviceManage.Instance.WaveTableName;
            if (string.IsNullOrEmpty(comboBox_startTime.Text))
            {
                MessageBox.Show(InterpretBase.textTran("请选择采集时间"));
                comboBox_startTime.Focus();
                return;
            }
           
            strHistroyTime = comboBox_startTime.Text;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            return;
        }
    }
}
