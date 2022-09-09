using ClassLibrary_MultiLanguage;
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
using System.Windows.Input;
using IPret = ClassLibrary_MultiLanguage.InterpretBase;//添加引用

namespace saker_Winform.SubForm
{
    public partial class Form_UserManager : Form
    {
        string sql = " SELECT DISTINCT A.GUID,A.UserName,A.Password,A.CreateTime,B.RoleName from [dbo].[Sys_User_Info] A LEFT JOIN[dbo].[Sys_Permission_Info] B ON  A.Role = B.ID; ";
        private Form_AddUserInfo form_AddUserInfo = new Form_AddUserInfo();
        public Form_UserManager()
        {
            InitializeComponent();
        }

        private void Form_UserManager_Load(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(Form_Main.lang);
            IPret.LoadLanguage(Form_Main.lang);
            IPret.InitLanguage(this);

            DataTable dt = DbHelperSql.QueryDt(sql);
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show(InterpretBase.textTran("未查询到数据"));
                return;
            }
            this.dataGridView1.DataSource = dt;
            this.dataGridView1[3, 0].ReadOnly = true;
            //CurrencyManager cm = (CurrencyManager)BindingContext[this.dataGridView1.DataSource];
            //cm.SuspendBinding();
            //this.dataGridView1.Rows[0].ReadOnly = true;
            //this.dataGridView1.Rows[0].Visible = false;
            //cm.ResumeBinding(); //恢复数据绑定
        }

        /// <summary>
        /// 从数据库重新Load数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconButton_Refresh_Click(object sender, EventArgs e)
        {
            DataTable dt = DbHelperSql.QueryDt(sql);
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show(InterpretBase.textTran("未查询到数据"));
                return;
            }
            this.dataGridView1.DataSource = dt;
        }

        /// <summary>
        /// 写数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_OkWrite_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 删除指定行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            var GUID = e.Row.Cells[0].Value;
            string sqlDel = "DELETE FROM [dbo].[Sys_User_Info] where GUID = '{0}'";
            sqlDel = string.Format(sqlDel, GUID);
            DbHelperSql.ExecuteSql(sqlDel);

        }

        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItem_Del_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.CurrentRow.Cells[1].Value.ToString() == "admin")
            {
                MessageBox.Show(InterpretBase.textTran("此账号为默认管理员，无法删除"));
            }
            else
            {
                var GUID = this.dataGridView1.CurrentRow.Cells[0].Value;
                string sqlDel = "DELETE FROM [dbo].[Sys_User_Info] where GUID = '{0}'";
                sqlDel = string.Format(sqlDel, GUID);
                DbHelperSql.ExecuteSql(sqlDel);
                DataTable dt = DbHelperSql.QueryDt(sql);
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show(InterpretBase.textTran("未查询到数据"));
                    return;
                }
                this.dataGridView1.DataSource = dt;
            }

        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.contextMenuStrip1.Show(MousePosition);
            }
        }

        /// <summary>
        /// 添加用户信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItem_Add_Click(object sender, EventArgs e)
        {
            Form_AddUserInfo.addInfo = true;
            form_AddUserInfo.AddInfo();
            form_AddUserInfo.ShowDialog();
            if (form_AddUserInfo.oK)
            {
                DataTable dt = DbHelperSql.QueryDt(sql);
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show(InterpretBase.textTran("未查询到数据"));
                    return;
                }
                this.dataGridView1.DataSource = dt;

            }
            Form_AddUserInfo.addInfo = false;
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItem_Modifi_Click(object sender, EventArgs e)
        {
            //if (this.dataGridView1.CurrentRow.Cells[1].Value.ToString() == "admin")
            //{
            //    MessageBox.Show(InterpretBase.textTran("此账号为默认管理员，无法修改"));
            //}
            //else
            //{
            Form_AddUserInfo.modif = true;
            Form_AddUserInfo.GUIDModif = (string)this.dataGridView1.CurrentRow.Cells[0].Value;
            string userName = (string)this.dataGridView1.CurrentRow.Cells[1].Value;
            string passWord = (string)this.dataGridView1.CurrentRow.Cells[2].Value;
            string authority = (string)this.dataGridView1.CurrentRow.Cells[4].Value;
           

            form_AddUserInfo.ModifInfo(userName, passWord, authority, false);
            form_AddUserInfo.ShowDialog();
            if (form_AddUserInfo.oK)
            {
                DataTable dt = DbHelperSql.QueryDt(sql);
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show(InterpretBase.textTran("未查询到数据"));
                    return;
                }
                this.dataGridView1.DataSource = dt;
            }
            Form_AddUserInfo.modif = false;
            //}       
        }

        /// <summary>
        /// 删除键按下，忽略掉admin所在行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && (this.dataGridView1.CurrentRow.Cells[1].Value.ToString() == "admin"))
            {
                e.Handled = true;//true表示跳过控件的默认处理
                MessageBox.Show(InterpretBase.textTran("此账号为默认管理员，无法删除"));
            }
        }
    }
}
