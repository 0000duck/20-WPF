using ClassLibrary_MultiLanguage;
using saker_Winform.CommonBaseModule;
using saker_Winform.DataBase;
using saker_Winform.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IPret = ClassLibrary_MultiLanguage.InterpretBase;//添加引用

namespace saker_Winform.SubForm
{
    public partial class Form_LogIn : Form
    {
        #region 属性
        public static bool m_adminLog;

        public static bool m_vistorLog;


        #endregion


        public Form_LogIn()
        {
            InitializeComponent();
        }


        private void Form_LogIn_Load(object sender, EventArgs e)
        {        
            this.CenterToParent();
            this.textBox_ID.Focus();//设置控件焦点
            this.ActiveControl = this.textBox_ID;
            if (this.checkBox_RecordPw.Checked)
            {
                this.textBox_ID.Text = CRegisterHelper.GetRegisterKey("sakerUserName");
                this.textBox2_PassWord.Text = CRegisterHelper.GetRegisterKey("sakerPassword");
            }
        }

        /// <summary>
        /// 主机登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_LogIn_Click(object sender, EventArgs e)
        {
            string sqlUserName = "select UserName from [dbo].[Sys_User_Info] where UserName = '{0}'";
            sqlUserName = string.Format(sqlUserName, this.textBox_ID.Text);
            DataTable dt = DbHelperSql.QueryDt(sqlUserName);
            if (dt.Rows.Count == 0)// 判断账号是否存在
            {
                MessageBox.Show(InterpretBase.textTran("该账号不存在"));
            }
            else
            {
                /* 判断输入的权限 */
                string sqlRole = "select Role from [dbo].[Sys_User_Info] where UserName = '{0}'";
                sqlRole = string.Format(sqlRole, this.textBox_ID.Text);
                DataTable dt1 = DbHelperSql.QueryDt(sqlRole);
                if (int.Parse(dt1.Rows[0][0].ToString()) == 0)// 看权限是否正确
                {
                    string sqlPassWord = "select Password from [dbo].[Sys_User_Info] where UserName = '{0}'";
                    sqlPassWord = string.Format(sqlPassWord, this.textBox_ID.Text);
                    DataTable dt2 = DbHelperSql.QueryDt(sqlPassWord);// 获取账号对应的密码

                    if (this.textBox2_PassWord.Text == dt2.Rows[0][0].ToString())
                    {
                        m_adminLog = true;
                        if (this.checkBox_RecordPw.Checked)
                        {
                            CRegisterHelper.WriteRegisterKey("sakerUserName", this.textBox_ID.Text);
                            CRegisterHelper.WriteRegisterKey("sakerPassword", this.textBox2_PassWord.Text);
                        }
                        else
                        {
                            CRegisterHelper.WriteRegisterKey("sakerUserName", "");
                            CRegisterHelper.WriteRegisterKey("sakerPassword", "");
                        }
                        this.Close();

                    }
                    else
                    {
                        MessageBox.Show(InterpretBase.textTran("您输入的密码不正确！"), InterpretBase.textTran("登录情况"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show(InterpretBase.textTran("权限为访客，请点击访客按钮"), InterpretBase.textTran("登录情况"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// 从机登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_CancelLogIn_Click(object sender, EventArgs e)
        {
            string sqlUserName = "select UserName from [dbo].[Sys_User_Info] where UserName = '{0}'";
            sqlUserName = string.Format(sqlUserName, this.textBox_ID.Text);
            DataTable dt = DbHelperSql.QueryDt(sqlUserName);
            if (dt.Rows.Count == 0)// 判断账号是否存在
            {
                MessageBox.Show(InterpretBase.textTran("该账号不存在"));
            }
            else
            {
                /* 判断输入的权限 */
                string sqlRole = "select Role from [dbo].[Sys_User_Info] where UserName = '{0}'";
                sqlRole = string.Format(sqlRole, this.textBox_ID.Text);
                DataTable dt1 = DbHelperSql.QueryDt(sqlRole);
                if (int.Parse(dt1.Rows[0][0].ToString()) == 1)// 看权限是否为访客
                {
                    string sqlPassWord = "select Password from [dbo].[Sys_User_Info] where UserName = '{0}'";
                    sqlPassWord = string.Format(sqlPassWord, this.textBox_ID.Text);
                    DataTable dt2 = DbHelperSql.QueryDt(sqlPassWord);// 获取账号对应的密码

                    if (this.textBox2_PassWord.Text == dt2.Rows[0][0].ToString())
                    {
                        m_vistorLog = true;
                        if (this.checkBox_RecordPw.Checked)
                        {
                            CRegisterHelper.WriteRegisterKey("sakerUserName", this.textBox_ID.Text);
                            CRegisterHelper.WriteRegisterKey("sakerPassword", this.textBox2_PassWord.Text);
                        }
                        else
                        {
                            CRegisterHelper.WriteRegisterKey("sakerUserName", "");
                            CRegisterHelper.WriteRegisterKey("sakerPassword", "");
                        }
                        this.Close();

                    }
                    else
                    {
                        MessageBox.Show(InterpretBase.textTran("您输入的密码不正确"), InterpretBase.textTran("登录情况"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show(InterpretBase.textTran("权限为管理员，请点击管理员按钮"), InterpretBase.textTran("登录情况"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }           
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            m_vistorLog = false;
            m_adminLog = false;
            this.Close();
        }
    }
}
