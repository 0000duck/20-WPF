using saker_Winform.DataBase;
using saker_Winform.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IPret = ClassLibrary_MultiLanguage.InterpretBase;//添加引用

namespace saker_Winform.SubForm
{
    public partial class Form_CalibrationInfo : Form
    {
        public Form_CalibrationInfo()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        private void Form_CalibrationInfo_Load(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(Form_Main.lang);
            IPret.LoadLanguage(Form_Main.lang);
            IPret.InitLanguage(this);
            string query = "SELECT * FROM View_Data_Calibration";
            DataTable dt = DbHelperSql.QueryDt(query);
            dataGridView_Calibration.DataSource = dt;
            dataGridView_Calibration.RowHeadersVisible = false;
        }

        private void Form_CalibrationInfo_FormClosed(object sender, FormClosedEventArgs e)
        {
            return;
        }
    }
}
