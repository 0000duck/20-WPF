using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClassLibrary_MultiLanguage;
using saker_Winform.DataBase;
using saker_Winform.Module;
using saker_Winform.UserControls;
using IPret = ClassLibrary_MultiLanguage.InterpretBase;//添加引用

namespace saker_Winform.SubForm
{

    public partial class Form_ChooseProject : Form
    {
        public static bool dataBaseLoad = false;
       
        public Form_ChooseProject()
        {
            InitializeComponent();
        }
        private void Form_ChooseProject_Load(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(Form_Main.lang);
            IPret.LoadLanguage(Form_Main.lang);
            IPret.InitLanguage(this);

            DataTable dtProject = new DataTable();
            string projectQuery = "SELECT GUID,Name FROM [Data_Project_Info] ORDER BY CREATETIME DESC";
            dtProject = DbHelperSql.QueryDt(projectQuery);
            comboBox_Project_Name.DataSource = dtProject;
            comboBox_Project_Name.ValueMember = "GUID";
            comboBox_Project_Name.DisplayMember = "Name";
            comboBox_Project_Name.SelectedIndex = -1;

        }
        private void comboBox_Project_Name_SelectedIndexChanged(object sender, EventArgs e)
        {
            string createTimeQuery = " SELECT GUID,Convert(varchar(20),CreateTime,120) AS CreateTime FROM dbo.Config_Device_All  WHERE ProjectGUID = '{0}' ORDER BY CREATETIME DESC";
            DataTable dtTime = new DataTable();
            dtTime = DbHelperSql.QueryDt(createTimeQuery);
            comboBox_TestTime.DataSource = dtTime;
            comboBox_TestTime.ValueMember = "GUID";
            comboBox_TestTime.DisplayMember = "CreateTime";
            if (comboBox_Project_Name.SelectedValue != null)
            {
                createTimeQuery = string.Format(createTimeQuery, comboBox_Project_Name.SelectedValue.ToString());
                dtTime = DbHelperSql.QueryDt(createTimeQuery);
                comboBox_TestTime.DataSource = dtTime;
            }
            comboBox_TestTime.SelectedIndex = -1;
        }

        private void iconButton_Search_Click(object sender, EventArgs e)
        {
            //从波表中查询数据,查询满足项目条件的tag数据
            if (string.IsNullOrEmpty(comboBox_Project_Name.Text))
            {
                MessageBox.Show(InterpretBase.textTran("项目不允许为空"), InterpretBase.textTran("提示"), MessageBoxButtons.OK);
                return;
            }
            if (string.IsNullOrEmpty(comboBox_TestTime.Text))
            {
                Module_DeviceManage.Instance.ProjectName = comboBox_Project_Name.Text;
                Module_DeviceManage.Instance.ProjectGUID = comboBox_Project_Name.SelectedValue.ToString();
                MessageBox.Show(InterpretBase.textTran("查询到项目，但是未查询到测试信息"));
                dataBaseLoad = true;
                this.Close();
                return;
            }                           
            string sql = "SELECT DISTINCT A.Name AS DeviceName,A.SerialNumber AS SN,A.IP,B.ChannelID,B.Tag,B.MeasureType,C.Memdepth,A.SampleRate,A.CreateTime,b.DeviceGUID,b.GUID AS ChannelGUID,B.ChannelDelayTime  from dbo.Data_Device_Info A " +
 " LEFT JOIN dbo.Config_Channel B ON A.GUID = B.DeviceGUID " + "LEFT JOIN dbo.Config_Device_All C ON A.CollectGUID = C.GUID " + "LEFT JOIN dbo.Data_Project_Info D ON C.ProjectGUID = D.GUID " +
 " WHERE D.Name = '{0}' AND C.GUID = '{1}' ";         
            sql = sql + " ORDER BY A.Name ,B.ChannelID ASC";
            sql = string.Format(sql, comboBox_Project_Name.Text, comboBox_TestTime.SelectedValue.ToString());
            DataTable dt = DbHelperSql.QueryDt(sql);
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show(InterpretBase.textTran("未查询到满足条件的项目信息"));
                return;
            }
            string info = InterpretBase.textTran("查询到项目") + comboBox_Project_Name.Text + InterpretBase.textTran("在") + comboBox_TestTime.Text + InterpretBase.textTran("的时间记录，确定导入?");
            if (MessageBox.Show(info, InterpretBase.textTran("提示"), MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                LoadDeviceManage(comboBox_Project_Name.SelectedValue.ToString(), comboBox_TestTime.SelectedValue.ToString());
                dataBaseLoad = true;
                LoadViewConfig();               
            }
            this.Close();
        }
        private void LoadDeviceManage(string project, string time)
        {
            //加载对应的项目
            string sqlProject = "SELECT TOP 1 * FROM [Data_Project_Info] WHERE GUID ='{0}' ";
            sqlProject = string.Format(sqlProject,project);
            DataTable dtProject = DbHelperSql.QueryDt(sqlProject);
            if (dtProject.Rows.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show(InterpretBase.textTran("还未创建过项目，请先创建项目"));
                return;
            }          
            Module_DeviceManage.Instance.ProjectName = dtProject.Rows[0]["Name"].ToString();
            Module_DeviceManage.Instance.ProjectGUID = dtProject.Rows[0]["GUID"].ToString();
            Module_DeviceManage.Instance.ProjectRemark = dtProject.Rows[0]["Remark"].ToString();
            string sql = "SELECT TOP 1 * FROM [Config_Device_ALL] WHERE ProjectGUID = '{0}' AND GUID ='{1}'";
            sql = string.Format(sql, project, time);
            DataTable dt = DbHelperSql.QueryDt(sql);
            if (dt.Rows.Count == 0)
                return;
            //赋值顶层instance的GUID和参数
            DataRow dr = dt.Rows[0];
            Module_DeviceManage.Instance.GUID = dr["GUID"].ToString();
            Module_DeviceManage.Instance.TriggerSource = dr["TriggerSource"].ToString();
            Module_DeviceManage.Instance.TriggerMode = dr["TriggerMode"].ToString();
            Module_DeviceManage.Instance.HorizontalOffset = dr["HorizontalOffset"].ToString();
            Module_DeviceManage.Instance.HorizontalTimebase = dr["HorizontalTimebase"].ToString();
            Module_DeviceManage.Instance.TriggerLevel = dr["TriggerLevel"].ToString();
            Module_DeviceManage.Instance.MemDepth = dr["Memdepth"].ToString();
            Module_DeviceManage.Instance.HoldOff = dr["HoldOff"].ToString();
            Module_DeviceManage.Instance.WaveTableName = dr["WaveTableName"].ToString();
            Module_DeviceManage.Instance.IP = dr["IP"].ToString();
           
            //添加设备赋值
            sql = "SELECT * FROM [Data_Device_Info] WHERE CollectGUID = '{0}' ORDER BY CREATETIME DESC";
            sql = string.Format(sql, Module_DeviceManage.Instance.GUID);
            DataTable dtDevice = DbHelperSql.QueryDt(sql);
            string devicesGUID = "'";
            //从数据库中查询设备的设备延时，校准的时候信息录入到系统中
            sql = " SELECT * FROM View_Data_Calibration ";
            DataTable dtDeviceDelayTime = DbHelperSql.QueryDt(sql);
            foreach (DataRow item in dtDevice.Rows)
            {
                Module_Device device = new Module_Device();
                device.DevDelayTime = item["DelayTime"].ToString();                     
                device.GUID = item["GUID"].ToString();
                device.IP = item["IP"].ToString();
                device.HashCode = IPAddress.Parse(device.IP).GetHashCode();
                device.MAC = item["MAC"].ToString();               
                device.VirtualNumber = item["VirtualNumber"].ToString();             
                device.Status = Convert.ToBoolean(item["Status"]);
                device.SN = item["SerialNumber"].ToString();
                device.ServerIP = item["ServerIP"].ToString();
                device.Model = item["Model"].ToString();
                device.Name = item["Name"].ToString();
                device.SampRate = item["SampleRate"].ToString();
                device.SoftVersion = item["SoftVersion"].ToString();
                Module_DeviceManage.Instance.Devices.Add(device);
                devicesGUID = devicesGUID + "','" + device.GUID;
                device.IsOnTesting = false;
            }
            sql = "SELECT * FROM [Config_Channel] WHERE DeviceGUID IN({0}')";
            sql = string.Format(sql, devicesGUID);
            DataTable dtChannels = new DataTable();
            dtChannels = DbHelperSql.QueryDt(sql);
            foreach (DataRow item in dtChannels.Rows)
            {
                string deviceGUID = item["DeviceGUID"].ToString();
                int channelID = Convert.ToInt32(item["ChannelID"]);
                Module_DeviceManage.Instance.GetDeviceByGUID(deviceGUID).GetChannel(channelID).BChanBWLimit = Convert.ToBoolean(item["BchanBWLimit"]);
                Module_DeviceManage.Instance.GetDeviceByGUID(deviceGUID).GetChannel(channelID).BChanImpedence = Convert.ToBoolean(item["BchanImpedance"]);
                Module_DeviceManage.Instance.GetDeviceByGUID(deviceGUID).GetChannel(channelID).BChanInv = Convert.ToBoolean(item["BchanInv"]);
           //   Module_DeviceManage.Instance.GetDeviceByGUID(deviceGUID).GetChannel(channelID).ChannelDelayTime = Convert.ToInt32(item["ChannelDelayTime"]);
                Module_DeviceManage.Instance.GetDeviceByGUID(deviceGUID).GetChannel(channelID).ChannelID = channelID;
                Module_DeviceManage.Instance.GetDeviceByGUID(deviceGUID).GetChannel(channelID).Collect = Convert.ToBoolean(item["Collect"]);
                if (Module_DeviceManage.Instance.GetDeviceByGUID(deviceGUID).IsOnTesting==false&&Convert.ToBoolean(item["Collect"]) == true)
                {
                    Module_DeviceManage.Instance.GetDeviceByGUID(deviceGUID).IsOnTesting = true;
                }
                Module_DeviceManage.Instance.GetDeviceByGUID(deviceGUID).GetChannel(channelID).Record = Convert.ToBoolean(item["Record"]);
                Module_DeviceManage.Instance.GetDeviceByGUID(deviceGUID).GetChannel(channelID).Scale = item["Scale"].ToString();
                Module_DeviceManage.Instance.GetDeviceByGUID(deviceGUID).GetChannel(channelID).Tag = item["Tag"].ToString();
                Module_DeviceManage.Instance.GetDeviceByGUID(deviceGUID).GetChannel(channelID).TagDesc = item["TagDesc"].ToString();
                Module_DeviceManage.Instance.GetDeviceByGUID(deviceGUID).GetChannel(channelID).TriggerPositon = Convert.ToInt32(item["TriggerPosition"]);
                Module_DeviceManage.Instance.GetDeviceByGUID(deviceGUID).GetChannel(channelID).Valid = Convert.ToBoolean(item["Valid"]);
                Module_DeviceManage.Instance.GetDeviceByGUID(deviceGUID).GetChannel(channelID).MeasureType = item["MeasureType"].ToString();
                Module_DeviceManage.Instance.GetDeviceByGUID(deviceGUID).GetChannel(channelID).ModifiedTime = Convert.ToDateTime(item["CreateTime"]);
                Module_DeviceManage.Instance.GetDeviceByGUID(deviceGUID).GetChannel(channelID).Offset = item["Offset"].ToString();
                Module_DeviceManage.Instance.GetDeviceByGUID(deviceGUID).GetChannel(channelID).Open = Convert.ToBoolean(item["Open"]);
                Module_DeviceManage.Instance.GetDeviceByGUID(deviceGUID).GetChannel(channelID).ProbeRatio = item["ProbeRatio"].ToString();
                Module_DeviceManage.Instance.GetDeviceByGUID(deviceGUID).GetChannel(channelID).Impedance = item["Impedance"].ToString();
                Module_DeviceManage.Instance.GetDeviceByGUID(deviceGUID).GetChannel(channelID).Coupling = item["Coupling"].ToString();
                Module_DeviceManage.Instance.GetDeviceByGUID(deviceGUID).GetChannel(channelID).GUID = item["GUID"].ToString();
                Module_DeviceManage.Instance.GetDeviceByGUID(deviceGUID).GetChannel(channelID).XIncrement = item["XIncrement"].ToString();
                Module_DeviceManage.Instance.GetDeviceByGUID(deviceGUID).GetChannel(channelID).XOrigin = item["XOrigin"].ToString();
                Module_DeviceManage.Instance.GetDeviceByGUID(deviceGUID).GetChannel(channelID).XReference = item["XReference"].ToString();
                Module_DeviceManage.Instance.GetDeviceByGUID(deviceGUID).GetChannel(channelID).YIncrement = item["YIncrement"].ToString();
                Module_DeviceManage.Instance.GetDeviceByGUID(deviceGUID).GetChannel(channelID).YOrigin = item["YOrigin"].ToString();
                Module_DeviceManage.Instance.GetDeviceByGUID(deviceGUID).GetChannel(channelID).YReference = item["YReference"].ToString();
                Module_DeviceManage.Instance.GetDeviceByGUID(deviceGUID).GetChannel(channelID).ChannelDelayTime = item["ChannelDelayTime"].ToString();
            }
           
        }
        private void LoadViewConfig()
        {
            string projectGUID = Module_DeviceManage.Instance.ProjectGUID;
            string collectGUID = Module_DeviceManage.Instance.GUID;
            string sql = "SELECT GROUPNAME,TAG,SCALEMIN,SCALEMAX,WAVECOLOR,WAVETYPE,ISSHOW,ID,No,Y,ISCHOOSE FROM CONFIG_VIEW WHERE ProjectGUID ='{0}' AND CollectGUID = '{1}' ORDER BY GROUPNAME ";
            sql = string.Format(sql, projectGUID, collectGUID);
            DataTable dt = new DataTable();
            dt = DbHelperSql.QueryDt(sql);
            if(dt.Rows.Count>0)
            {
                for (int i = 1; i <= 4; i++)
                {
                    DataTable temp = dt.Select("GroupName='" + i.ToString() + "'").CopyToDataTable();
                    Module_ViewConfig.Instance.SetShowConfig(i.ToString(), temp);
                }
            }
            else
            {
                for (int i = 1; i <= 4; i++)
                {
                    DataTable temp = Module_ViewConfig.Instance.GetNullTable();
                    Module_ViewConfig.Instance.SetShowConfig(i.ToString(), temp);
                }            
            }
            Module_ViewConfig.Instance.IsInit = true;
        }
        private void Form_ChooseProject_FormClosed(object sender, FormClosedEventArgs e)
        {
            return;
        }
    }
}
