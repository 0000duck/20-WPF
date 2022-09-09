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
    public partial class Form_DeletDeviceConfigInfo : Form
    {
        public Form_DeletDeviceConfigInfo()
        {
            InitializeComponent();
            this.CenterToParent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.ShowInTaskbar = false;
        }

        public static bool DeletStatus = false;
        private void button1_DeletOK_Click(object sender, EventArgs e)
        {
            DeletStatus = true;
            this.Close();
        }

        private void button1_DeletCancel_Click(object sender, EventArgs e)
        {
            DeletStatus = false;
            this.Close();
        }

        private void Form_DeletDeviceConfigInfo_Load(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(Form_Main.lang);
            IPret.LoadLanguage(Form_Main.lang);
            IPret.InitLanguage(this);
        }
    }
}
