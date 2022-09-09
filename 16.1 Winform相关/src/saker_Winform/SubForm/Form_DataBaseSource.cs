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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using saker_Winform.Module;
using ClassLibrary_MultiLanguage;
using IPret = ClassLibrary_MultiLanguage.InterpretBase;//添加引用

namespace saker_Winform.SubForm
{
    public partial class Form_DataBaseSource : Form
    {
        #region 字段
        private bool bPaint;

        public bool m_bPaint
        {
            get { return bPaint; }
            set { bPaint = value; }
        }

        #endregion

        public Form_DataBaseSource()
        {
            InitializeComponent();  
            this.dataGridView_DataBase.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }
        //存储波形数据的list
        public List<Module_WaveInfo> WaveInfo = new List<Module_WaveInfo>();
        //存储项目的七个主要信息表
        public DataTable dtCommenPara = new DataTable();
        /// <summary>
        /// 事件：全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_AllChoose_Click(object sender, EventArgs e)
        {
            this.dataGridView_DataBase.EndEdit();//结束编辑
            CDataGridViewShow.AllChooseOrNot(dataGridView_DataBase, true, 0);
        }

        /// <summary>
        /// 事件：取消全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_CancelAll_Click(object sender, EventArgs e)
        {
            this.dataGridView_DataBase.EndEdit();//结束编辑
            CDataGridViewShow.AllChooseOrNot(dataGridView_DataBase, false, 0);
        }

        /// <summary>
        /// 事件：查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconButton_Search_Click(object sender, EventArgs e)
        {
            //从波表中查询数据,查询满足项目条件的tag数据
            if (string.IsNullOrEmpty(comboBox_Project_Name.Text) || string.IsNullOrEmpty(comboBox_TestTime.Text))
            {
                MessageBox.Show(InterpretBase.textTran("项目和测试时间不允许为空"));
                return;
            }
          
            string sqlWaveinfo = "SELECT DeviceGUID,ChannelGUID,IsWholeComplete,TrigTimeStamp,ChanDelayTime,Convert(varchar,STARTTIME,120) AS StartTime FROM " + label_waveTableName.Text + " WHERE STARTTime is not null ";
            if (!string.IsNullOrEmpty(comboBox_startTime.Text))
            {
                sqlWaveinfo = sqlWaveinfo + " AND Convert(varchar, STARTTIME, 120) = '" + comboBox_startTime.Text + "'";
            }
            DataTable dtWaveinfo = DbHelperSql.QueryDt(sqlWaveinfo);
            if (dtWaveinfo==null||dtWaveinfo.Rows.Count == 0)
            {
                MessageBox.Show(InterpretBase.textTran("未查询到相关采集数据"));
                return;
            }
            //查询设备信息h和通道信息
            string devics = "";
            string channels = "";
            foreach (DataRow drWaveInfo in dtWaveinfo.Rows)
            {
                if (!devics.Contains(drWaveInfo["DeviceGUID"].ToString()))
                {
                    if (devics == "")
                    {
                        devics = "'" + drWaveInfo["DeviceGUID"].ToString() + "'";
                    }
                    else
                    {
                        devics += "," + "'" + drWaveInfo["DeviceGUID"].ToString() + "'";
                    }
                }
                if (!channels.Contains(drWaveInfo["ChannelGUID"].ToString()))
                {
                    if (channels == "")
                    {
                        channels = "'" + drWaveInfo["ChannelGUID"].ToString() + "'";
                    }
                    else
                    {
                        channels += "," + "'" + drWaveInfo["ChannelGUID"].ToString() + "'";
                    }
                }

            }
            string sqlDevicesInfo = "SELECT * FROM Data_Device_Info WHERE GUID IN({0})";
            sqlDevicesInfo = string.Format(sqlDevicesInfo, devics);
            DataTable dtDevicesInfo = DbHelperSql.QueryDt(sqlDevicesInfo);
            string sqlChannelsInfo = "SELECT * FROM CONFIG_CHANNEL WHERE GUID IN({0}) ";
            string channelSelected = "";
            if(checkBox1.Checked == true)
            {
                channelSelected += "1,";
            }
            if(checkBox2.Checked == true)
            {
                channelSelected += "2,";
            }
            if (checkBox3.Checked == true)
            {
                channelSelected += "3,";
            }
            if (checkBox4.Checked == true)
            {
                channelSelected += "4,";
            }
            if(!string.IsNullOrEmpty(channelSelected))
            {
                channelSelected = channelSelected.Remove(channelSelected.Length - 1, 1);
            }
          
            if (!string.IsNullOrEmpty(channelSelected))
            {
                sqlChannelsInfo = sqlChannelsInfo + " AND ChannelID IN(" + channelSelected + ")";
            }
            if (!string.IsNullOrEmpty(comboBox_tagBegin.Text))
            {
                sqlChannelsInfo = sqlChannelsInfo + " AND Tag>='" + comboBox_tagBegin.Text + "'";
            }
            if (!string.IsNullOrEmpty(comboBox_tagEnd.Text))
            {
                sqlChannelsInfo = sqlChannelsInfo + " AND Tag <='" + comboBox_tagEnd.Text + "'";
            }
            if (!string.IsNullOrEmpty(comboBox_measure_type.Text))
            {
                sqlChannelsInfo = sqlChannelsInfo + " AND MeasureType = '" + comboBox_measure_type.Text + "'";
            }
            sqlChannelsInfo = string.Format(sqlChannelsInfo, channels);
            DataTable dtChannelsInfo = DbHelperSql.QueryDt(sqlChannelsInfo);
            if(dtChannelsInfo == null)
            {
                MessageBox.Show(InterpretBase.textTran("未查询到相关采集数据"));
                return;
            }
            DataTable dt = new DataTable();
            if (dtWaveinfo.Rows.Count == 0 || dtDevicesInfo.Rows.Count == 0 || dtChannelsInfo.Rows.Count == 0)
            {

                if (dataGridView_DataBase.DataSource == null)
                {
                    MessageBox.Show(InterpretBase.textTran("未查询到相关采集数据"));
                    return;
                }
                DataTable dtSource = dataGridView_DataBase.DataSource as DataTable;
                dt = dtSource.Clone();
                dt.Clear();
                dataGridView_DataBase.DataSource = dt;
                MessageBox.Show(InterpretBase.textTran("未查询到相关采集数据"));
                return;
            }
            dt.Columns.Add("DeviceName");
            dt.Columns.Add("SN");
            dt.Columns.Add("IP");
            dt.Columns.Add("ChannelID");
            dt.Columns.Add("Tag");
            dt.Columns.Add("ChanDelayTime");
            dt.Columns.Add("TrigTimeStamp");
            dt.Columns.Add("MeasureType");
            dt.Columns.Add("Offset");
            dt.Columns.Add("Xincrement");
            dt.Columns.Add("Xreference");
            dt.Columns.Add("Xorigin");
            dt.Columns.Add("Yincrement");
            dt.Columns.Add("Yreference");
            dt.Columns.Add("Yorigin");
            dt.Columns.Add("Memdepth");
            dt.Columns.Add("SampleRate");
            dt.Columns.Add("CreateTime");
            dt.Columns.Add("DeviceGUID");
            dt.Columns.Add("ChannelGUID");     
            dt.Columns.Add("WaveTableName");
            dt.Columns.Add("StartTime");
            foreach (DataRow drWaveInfo in dtWaveinfo.Rows)
            {
                DataRow dr = dt.NewRow();
                dr["TrigTimeStamp"] = drWaveInfo["TrigTimeStamp"];
                dr["ChanDelayTime"] = drWaveInfo["ChanDelayTime"];
                dr["WaveTableName"] = this.label_waveTableName.Text;
                dr["StartTime"] = drWaveInfo["StartTime"];
                dr["DeviceGUID"] = drWaveInfo["DeviceGUID"];
                dr["ChannelGUID"] = drWaveInfo["ChannelGUID"]; 
                DataRow drDevice = dtDevicesInfo.Select("GUID='" + drWaveInfo["DeviceGUID"].ToString()+"'").FirstOrDefault();
                dr["DeviceName"] = drDevice["Name"];
                dr["SN"] = drDevice["SerialNumber"];
                dr["IP"] = drDevice["IP"];
                dr["CreateTime"] = drDevice["CreateTime"];
                dr["SampleRate"] = drDevice["SampleRate"];
                dr["Memdepth"] = dtCommenPara.Rows[0]["MemDepth"];
                DataRow drChannel = dtChannelsInfo.Select("GUID='" + drWaveInfo["ChannelGUID"].ToString()+"'").FirstOrDefault();
                if (drChannel != null)
                {
                    dr["ChannelID"] = drChannel["ChannelID"];
                    dr["Tag"] = drChannel["Tag"];
                    dr["Xincrement"] = drChannel["Xincrement"];
                    dr["Xreference"] = drChannel["Xreference"];
                    dr["Xorigin"] = drChannel["Xorigin"];
                    dr["Yincrement"] = drChannel["Yincrement"];
                    dr["Yreference"] = drChannel["Yreference"];
                    dr["Yorigin"] = drChannel["Yorigin"];
                    dr["Offset"] = drChannel["Offset"];
                    dr["MeasureType"] = drChannel["MeasureType"];
                    dt.Rows.Add(dr);
                }
                
            }
            dt.DefaultView.Sort = "Tag ASC";
            dataGridView_DataBase.DataSource = dt;
        }

        private void Form_DataBaseSource_Load(object sender, EventArgs e)
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
            comboBox_Channel.SelectedIndex = 0;
        }

        private void comboBox_Project_Name_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_Project_Name.SelectedValue != null)
            {
                string createTimeQuery = " SELECT GUID,Convert(varchar,CreateTime,120) AS CreateTime FROM dbo.Config_Device_All  WHERE ProjectGUID = '{0}' ORDER BY CREATETIME DESC";
                DataTable dtTime = new DataTable();
                dtTime = DbHelperSql.QueryDt(createTimeQuery);
                comboBox_TestTime.DataSource = dtTime;
                comboBox_TestTime.ValueMember = "GUID";
                comboBox_TestTime.DisplayMember = "CreateTime";
                createTimeQuery = string.Format(createTimeQuery, comboBox_Project_Name.SelectedValue.ToString());
                dtTime = DbHelperSql.QueryDt(createTimeQuery);
                comboBox_TestTime.DataSource = dtTime;
            }
            comboBox_TestTime.SelectedIndex = -1;
        }

        private void comboBox_TestTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_TestTime.SelectedValue != null)
            {

                string wtName = " SELECT * FROM dbo.Config_Device_All WHERE GUID = '{0}' ";
                wtName = string.Format(wtName, comboBox_TestTime.SelectedValue.ToString());            
                dtCommenPara = DbHelperSql.QueryDt(wtName);
                string waveTableName = dtCommenPara.Rows[0]["WaveTableName"].ToString();
                label_waveTableName.Text = waveTableName;
               
                string startTimeQuery = " SELECT distinct Convert(varchar, StartTime, 120) AS StartTime FROM " + waveTableName + " ORDER BY StartTime DESC";
                DataTable dtTime = new DataTable();
                dtTime = DbHelperSql.QueryDt(startTimeQuery);
                comboBox_startTime.DisplayMember = "StartTime";
                comboBox_startTime.DataSource = dtTime;
                comboBox_startTime.SelectedIndex = -1;

            }
        }
        private void button_SaveToLocal_Click(object sender, EventArgs e)
        {

            DataTable dt = new DataTable();
            dt = dataGridView_seleted.DataSource as DataTable;
            if (dt == null || dt.Rows.Count == 0)
            {
                MessageBox.Show(InterpretBase.textTran("请先选择数据添加绘制表中"));
                WaveInfo.Clear();
                return;
            }
            WaveInfo = GetWaveInfos(dt);
            List<Module_WaveInfo> listToSave = new List<Module_WaveInfo>();
            listToSave = WaveInfo;
            string path = AppDomain.CurrentDomain.BaseDirectory + "WaveData";
            this.ucProgressBar1.Value = 0;
            this.ucProgressBar1.Maximum = listToSave.Count;
            for (int i=0;i<listToSave.Count;i++)
            {                                           
                CFileOperate.WriteDataToLocal(path,WaveInfo[i]);
                this.ucProgressBar1.Value = this.ucProgressBar1.Value % 100 + 1;
                this.ucProgressBar1.Refresh();                            
            }
            MessageBox.Show(InterpretBase.textTran("文件已经保存在")+InterpretBase.textTran("目录下")+path);
            Debug.WriteLine(listToSave.Count);
        }

        private void button_Add_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            DataTable dtSource = dataGridView_seleted.DataSource as DataTable;
            if (dtSource == null)
            {
                dtSource = dataGridView_DataBase.DataSource as DataTable;
                if (dtSource == null)
                {
                    return;
                }
                dt = dtSource.Clone();
                dt.Clear();
            }
            else
            {
                dt = dtSource;
            }
            for (int i = 0; i < dataGridView_DataBase.RowCount; i++)
            {
                if (dataGridView_DataBase.Rows[i].Cells[0].EditedFormattedValue.ToString() == "True")
                {
                    DataRow dr = (dataGridView_DataBase.Rows[i].DataBoundItem as DataRowView).Row;
                    string conndition = "Tag = '" + dr["Tag"].ToString() + "' AND StartTime = '" + dr["StartTime"].ToString() + "'";
                    if (dt.Select(conndition).Length == 0)
                    {
                        dt.Rows.Add(dr.ItemArray);
                    }                    
                }
            }
            this.dataGridView_seleted.DataSource =  dt;
        }

        private void dataGridView_seleted_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            SolidBrush b = new SolidBrush(this.dataGridView_seleted.RowHeadersDefaultCellStyle.ForeColor);
            e.Graphics.DrawString((e.RowIndex + 1).ToString(System.Globalization.CultureInfo.CurrentUICulture), this.dataGridView_seleted.DefaultCellStyle.Font, b, e.RowBounds.Location.X + 20, e.RowBounds.Location.Y + 4);
        }

        private void dataGridView_DataBase_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            SolidBrush b = new SolidBrush(this.dataGridView_seleted.RowHeadersDefaultCellStyle.ForeColor);
            e.Graphics.DrawString((e.RowIndex + 1).ToString(System.Globalization.CultureInfo.CurrentUICulture), this.dataGridView_seleted.DefaultCellStyle.Font, b, e.RowBounds.Location.X + 20, e.RowBounds.Location.Y + 4);
        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            for (int i = this.dataGridView_seleted.SelectedRows.Count; i > 0; i--)
            {              
                dataGridView_seleted.Rows.RemoveAt(dataGridView_seleted.SelectedRows[i - 1].Index);
            }
        }
        /// <summary>
        /// 调用绘制方法，进行绘图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Paint_Click(object sender, EventArgs e)
        {          
            //拼装好波形数据，在绘制列表中的全部结算
            DataTable dt = new DataTable();
            dt = dataGridView_seleted.DataSource as DataTable;
            if (dt == null || dt.Rows.Count == 0)
            {
                MessageBox.Show(InterpretBase.textTran("请先选择数据添加绘制表中"));
                WaveInfo.Clear();
                return;
            }         
            WaveInfo = GetWaveInfos(dt);
            m_bPaint = true;
            this.Close();
            //MessageBox.Show("数据已加载完成，等待绘图......");
            //WaveInfo.Clear();
            //Thread.Sleep(1000);
            //MessageBox.Show("绘制完成，清除本次加载");
        }
        /// <summary>
        /// 根据输入的dt查找数据库中对应的波形数据及其参数
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public List<Module_WaveInfo> GetWaveInfos(DataTable dt)
        {
            List<Module_WaveInfo> list = new List<Module_WaveInfo>(); 
            if (dt == null || dt.Rows.Count == 0)
            {
                MessageBox.Show(InterpretBase.textTran("请先选择数据添加绘制表中"));             
                return list;
            }        
            string channelGUID = "";
            string deviceGUID = "";
            string waveTableName = "";
            string startTime = "";
            string createTime = "";          
            DataRow drCommenPara = dtCommenPara.Rows[0];
            //每一行都是一次数据
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                waveTableName = dt.Rows[i]["WaveTableName"].ToString();
                channelGUID = dt.Rows[i]["ChannelGUID"].ToString();
                deviceGUID = dt.Rows[i]["DeviceGUID"].ToString();
                startTime = dt.Rows[i]["StartTime"].ToString();
                createTime = dt.Rows[i]["CreateTime"].ToString();
                //从波形数据表中，查询满足条件的数据
                string getWaveData = "SELECT * FROM " + waveTableName + "  WHERE ChannelGUID = '{0}' AND  Convert(varchar, StartTime, 120) = '{1}'";
                string getWavePara = "SELECT * FROM Data_Device_Info WHERE GUID = '{0}'";             
                getWaveData = string.Format(getWaveData, channelGUID, startTime);
                getWavePara = string.Format(getWavePara, deviceGUID);                            
                //波形数据
                DataTable dtWave = DbHelperSql.QueryDt(getWaveData);
                //设备信息
                DataTable dtPara = DbHelperSql.QueryDt(getWavePara);
              
                DataRow drWaveData = dtWave.Rows[0];
                DataRow drDevicePara = dtPara.Rows[0];                
                Module_WaveInfo module_WaveInfo = new Module_WaveInfo();
                module_WaveInfo.MeasureType = dt.Rows[i]["MeasureType"].ToString();
                module_WaveInfo.Tag = dt.Rows[i]["Tag"].ToString();
                module_WaveInfo.Offset = dt.Rows[i]["Offset"].ToString();
                module_WaveInfo.SN = drDevicePara["SerialNumber"].ToString();
                module_WaveInfo.StartTime = drWaveData["StartTime"].ToString();
                module_WaveInfo.ChannelID = Convert.ToInt32(dt.Rows[i]["ChannelID"]);
                module_WaveInfo.DeviceName = drDevicePara["Name"].ToString();
                module_WaveInfo.XIncrement = dt.Rows[i]["XIncrement"].ToString();
                module_WaveInfo.XOrigin = dt.Rows[i]["XOrigin"].ToString();
                module_WaveInfo.XReference = dt.Rows[i]["XReference"].ToString();
                module_WaveInfo.YIncrement = dt.Rows[i]["YIncrement"].ToString();
                module_WaveInfo.YOrigin = dt.Rows[i]["YOrigin"].ToString();
                module_WaveInfo.YReference = dt.Rows[i]["YReference"].ToString();           
                module_WaveInfo.ChannelDelayTime = Convert.ToInt32(dt.Rows[i]["ChanDelayTime"]);
                module_WaveInfo.DeviceDelayTime = Convert.ToInt32(drDevicePara["DelayTime"]);
                module_WaveInfo.TrigTimeStamp = dt.Rows[i]["TrigTimeStamp"].ToString();
                module_WaveInfo.Data = (byte[])drWaveData["Data"];
                module_WaveInfo.IP = drDevicePara["IP"].ToString();
                module_WaveInfo.WaveTabelName = waveTableName;
                module_WaveInfo.MemDepth = drCommenPara["MemDepth"].ToString();
                module_WaveInfo.ProjectName = comboBox_Project_Name.Text;
                module_WaveInfo.SampRate = drDevicePara["SampleRate"].ToString();
                module_WaveInfo.RecordTime = createTime;
                module_WaveInfo.ChannelMode = Convert.ToInt32(drDevicePara["ChannelModel"]);
                module_WaveInfo.No = i + 1;
                list.Add(module_WaveInfo);
            }
            return list;
        }

        private void button_Clear_Click(object sender, EventArgs e)
        {
            //拼装好波形数据，在绘制列表中的全部结算
            DataTable dt = new DataTable();
            dt = dataGridView_seleted.DataSource as DataTable;
            if (dt == null || dt.Rows.Count == 0)
            {               
                return;
            }
            dt.Clear();
            dataGridView_seleted.DataSource = dt;
            this.ucProgressBar1.Value = 0;
            this.ucProgressBar1.Refresh();
        }
        public void HidePaintButton(bool hide)
        {
            this.button_Paint.Visible = hide;
        }
    }
}
