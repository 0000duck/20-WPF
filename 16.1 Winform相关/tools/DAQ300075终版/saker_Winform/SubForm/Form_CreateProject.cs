using System;
using System.Windows.Forms;

using System.Xml;
using System.Xml.Linq;
using System.Linq;
using saker_Winform.CommonBaseModule;
using saker_Winform.DataBase;
using saker_Winform.UserControls;
using ClassLibrary_MultiLanguage;
using IPret = ClassLibrary_MultiLanguage.InterpretBase;//添加引用

namespace saker_Winform.SubForm
{
    public partial class Form_CreateProject : Form
    {
        public bool createFile;
        public bool openFile;
        public bool CreateStatus = false;//打开文件
        public bool CreatePrj = false;//创建工程
        public static string xmlPath;//xml文件路径
        public static string xmlAllPath;//xml全路径（含拓展名）
        public static string Note;//项目注释
        public static string projectName;//项目名
        public Form_CreateProject()
        {
            InitializeComponent();
            this.CenterToParent();
                   
           /* this.FormBorderStyle = FormBorderStyle.None;
            this.ShowInTaskbar = false;*/
        }

        public void SetProp()
        {
            if (createFile)
            {
                CreateStatus = false;
                this.textBox_ProjectName.Text = "Project_";
                this.textBox_ProjectName.ReadOnly = false;
                this.textBox_FileLoaction.ReadOnly = true;
            }
            else if (openFile)
            {
                CreateStatus = false;
                this.textBox_ProjectName.Text = "";
                this.textBox_ProjectName.ReadOnly = true;
                this.textBox_ProjectName.Enabled = false;
                this.textBox_FileLoaction.ReadOnly = true;
            }
        }

        private void button_CreateOK_Click(object sender, EventArgs e)
        {
            if (createFile)//创建工程
            {
                if (string.IsNullOrEmpty(textBox_ProjectName.Text) || string.IsNullOrEmpty(textBox_FileLoaction.Text))
                {
                    MessageBox.Show(InterpretBase.textTran("您有信息未填写"), InterpretBase.textTran("提示信息"));
                }
                else
                {                    
                    if (false == System.IO.Directory.Exists(this.textBox_FileLoaction.Text + "\\" + textBox_ProjectName.Text))
                    {
                        System.IO.Directory.CreateDirectory(this.textBox_FileLoaction.Text + "\\" + textBox_ProjectName.Text);
                        xmlPath = this.textBox_FileLoaction.Text + "\\" + textBox_ProjectName.Text;
                        Note = textBox_Note.Text;
                        projectName = textBox_ProjectName.Text;
                        //写入数据到数据库库中
                        string projectGuid = Guid.NewGuid().ToString();
                        string sql = "INSERT INTO [Data_Project_Info](GUID,Name,Location,Remark,CreateTime) " +
                                     "VALUES('{0}','{1}','{2}','{3}',GETDATE())";
                        sql = string.Format(sql, projectGuid, projectName, xmlPath, Note);
                        DbHelperSql.ExecuteSql(sql);
                        Module.Module_DeviceManage.Instance.ProjectGUID = projectGuid;
                        Module.Module_DeviceManage.Instance.ProjectRemark = this.textBox_Note.Text;
                    }
                    else
                    {
                        DialogResult result = MessageBox.Show(InterpretBase.textTran("该工程信息已存在,请重新输入工程名"), InterpretBase.textTran("提示信息"), MessageBoxButtons.OK);
                        if (result == DialogResult.OK)
                        {
                            this.textBox_ProjectName.Text = "Project_";
                            this.textBox_ProjectName.Focus();//设置控件焦点
                            this.ActiveControl = this.textBox_ProjectName;
                            return;
                        }
                    }
                    createFile = false;
                    this.Close();
                    CreatePrj = true;
                }               
            }
            else if (openFile)//打开本地XML
            {
                if (string.IsNullOrEmpty(textBox_ProjectName.Text) || string.IsNullOrEmpty(textBox_FileLoaction.Text))
                {
                    MessageBox.Show(InterpretBase.textTran("请选择文件存储位置"), InterpretBase.textTran("提示信息"));
                }
                else
                {
                    CreateStatus = true;
                    Note = textBox_Note.Text;
                    openFile = false;
                    this.Close();
                }            
            }
        }
        private void button_Cancel_Click(object sender, EventArgs e)
        {
            CreateStatus = false;
            this.Close();
        }

        private void Form_CreateProject_Load(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(Form_Main.lang);
            IPret.LoadLanguage(Form_Main.lang);
            IPret.InitLanguage(this);

            this.textBox_ProjectName.Focus();//设置控件焦点
            this.ActiveControl = this.textBox_ProjectName;
        }

        /// <summary>
        /// 浏览按钮被点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Find_Click(object sender, EventArgs e)
        {
            if(createFile)//创建工程
            {
                folderBrowserDialog.ShowDialog();
                this.textBox_FileLoaction.Text = folderBrowserDialog.SelectedPath;               
            }
            else if (openFile)//打开工程
            {
                openFileDialog.ShowDialog(); 
                if(!string.IsNullOrEmpty(openFileDialog.FileName)&& (openFileDialog.FileName!=""))
                {
                    this.textBox_FileLoaction.Text = System.IO.Path.GetDirectoryName(openFileDialog.FileName);//获取选取文件的路径                
                    this.textBox_ProjectName.Text = System.IO.Path.GetFileNameWithoutExtension(openFileDialog.FileName).Substring(15);//获取所选取文件的文件名                  
                    xmlPath = System.IO.Path.GetDirectoryName(openFileDialog.FileName);
                    projectName = textBox_ProjectName.Text;
                    xmlAllPath = openFileDialog.FileName;
                    CXmlHelper xmlHelper = new CXmlHelper();
                    if (string.IsNullOrEmpty(xmlAllPath) && string.IsNullOrEmpty(projectName))
                    {
                        this.textBox_Note.Text = xmlHelper.GetXmlNodeByXpath(xmlAllPath, "//" + projectName).Attributes["Note"].Value;//选取Project元素Note属性值                   
                    }
                }                         
            }
                
        }
    }
}
