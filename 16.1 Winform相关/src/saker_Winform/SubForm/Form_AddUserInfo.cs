using ClassLibrary_MultiLanguage;
using saker_Winform.CommonBaseModule;
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
    public partial class Form_AddUserInfo : Form
    {
        public bool oK;
        public bool cancel;
        public static bool modif;
        public static bool addInfo;
        public static string GUIDModif;
        public Form_AddUserInfo()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 添加/修改用户信息
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Password"></param>
        /// <param name="Authority"></param>
        public void ModifInfo(string UserName, string Password, string Authority, bool Enable)
        {
            this.label_Add.Text = "修改用户信息";
            this.textBox_UserName.Text = UserName;
            this.textBox_Password.Text = Password;
            this.comboBox_Limit.Text = Authority;
            this.button_OK.Text = "修改";
            this.textBox_UserName.Enabled = Enable;
        }

        public void AddInfo()
        {
            this.textBox_UserName.Enabled = true;
            this.label_Add.Text = "添加用户信息";
            this.button_OK.Text = "确定";
        }

        /// <summary>
        /// 添加设备
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_OK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.textBox_UserName.Text) || string.IsNullOrEmpty(this.textBox_Password.Text))
            {
                MessageBox.Show(InterpretBase.textTran("存在没有填写的信息，请确认"));
            }
            else
            {
                // 添加用户信息
                if (addInfo)
                {
                    string sqlUserName = "select UserName from [dbo].[Sys_User_Info] where UserName = '{0}'";
                    sqlUserName = string.Format(sqlUserName, this.textBox_UserName.Text);
                    DataTable dt = DbHelperSql.QueryDt(sqlUserName);// 判断账号是否存在
                    if (dt.Rows.Count == 0)
                    {
                        int Role = 0;
                        if (this.comboBox_Limit.Text == "管理员")
                        {
                            Role = 0;
                        }
                        else if (this.comboBox_Limit.Text == "访客")
                        {
                            Role = 1;
                        }
                        string sqlInsert = "INSERT INTO [dbo].[Sys_User_Info](GUID,UserName,Password,Role,IsUse,CreateTime)"
                    + "VALUES('{0}','{1}','{2}','{3}','{4}',GETDATE())";
                        string GUID = Guid.NewGuid().ToString();
                        sqlInsert = string.Format(sqlInsert, GUID, this.textBox_UserName.Text, this.textBox_Password.Text, Role, 1);
                        DbHelperSql.ExecuteSql(sqlInsert);
                        oK = true;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show(InterpretBase.textTran("该账号已存在"));
                    }
                }
                /* 修改用户信息 */
                else if (modif)
                {
                    if (this.textBox_UserName.Text == "admin")
                    {
                        MessageBox.Show(InterpretBase.textTran("此账号为超级管理员，无法修改"));
                    }
                    else
                    {
                        //string sqlUserName = "select UserName from [dbo].[Sys_User_Info] where UserName = '{0}'";
                        //sqlUserName = string.Format(sqlUserName, this.textBox_UserName.Text);
                        //DataTable dt = DbHelperSql.QueryDt(sqlUserName);// 判断账号是否存在
                        //if (dt.Rows.Count == 0)
                        string sqlUpdate = "UPDATE [dbo].[Sys_User_Info] set UserName = '{1}',Password = '{2}',Role = '{3}',CreateTime = GETDATE() where GUID = '{0}'";
                        sqlUpdate = string.Format(sqlUpdate, GUIDModif, this.textBox_UserName.Text, this.textBox_Password.Text, this.comboBox_Limit.SelectedIndex);
                        DbHelperSql.ExecuteSql(sqlUpdate);
                        oK = true;
                        this.Close();
                        //else
                        //{
                        //    MessageBox.Show(InterpretBase.textTran("该账号已存在"));
                        //}                          
                    }
                }


            }
        }

        private void Form_AddUserInfo_Load(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(Form_Main.lang);
            IPret.LoadLanguage(Form_Main.lang);
            IPret.InitLanguage(this);
            this.comboBox_Limit.SelectedIndex = 1;
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            cancel = true;
            this.Close();
        }

        private void textBox_Password_KeyPress(object sender, KeyPressEventArgs e)
        {
            //CString2Value.LimitInputNorCh(e,this.textBox_Password.Text);
        }
    }
}
