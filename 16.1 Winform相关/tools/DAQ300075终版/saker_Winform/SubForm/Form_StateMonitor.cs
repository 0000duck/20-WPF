using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using saker_Winform.UserControls;
using saker_Winform.Module;
using saker_Winform.DataBase;
using IPret = ClassLibrary_MultiLanguage.InterpretBase;//添加引用
using ClassLibrary_MultiLanguage;

namespace saker_Winform.SubForm
{
    public partial class Form_StateMonitor : Form
    {
        #region Fields
        /// <summary>
        /// 运行状态
        /// </summary>
        private bool bRunState = false;
        public bool m_bRunState
        {
            get { return bRunState; }
            set { bRunState = value; }
        }
        /// <summary>
        /// 开始测试的时间
        /// </summary>
        private string strTimeStart = "";
        public string m_strTimeStart
        {
            get { return strTimeStart; }
            set { strTimeStart = value; }
        }
        /// <summary>
        /// 校准时间
        /// </summary>
        private string strCalTime = "";
        public string m_strCalTime
        {
            get { return strCalTime; }
            set { strCalTime = value; }
        }
        /// <summary>
        /// 已记录的条目
        /// </summary>
        private int recordNum = 0;
        public int m_recordNum
        {
            get { return recordNum; }
            set { recordNum = value; }
        }
        /// <summary>
        /// 数据丢失个数
        /// </summary>
        private int dataMissNum = 0;
        public int m_dataMissNum
        {
            get { return dataMissNum; }
            set { dataMissNum = value; }
        }
        /// <summary>
        /// 写入错误个数
        /// </summary>
        private int writeErrNum = 0;
        public int m_writeErrNum
        {
            get { return writeErrNum; }
            set { writeErrNum = value; }
        }
        //定义管理设备监控信息
        public Module_StateMonitor modStateMonitor = new Module_StateMonitor();

        //定时器
        System.Timers.Timer timUpdateRecord = new System.Timers.Timer(1000*5);

        public delegate void recordInfo_Refresh(string strRecordNum, string strDataMiss, string strWritError);//申明记录信息显示刷新的委托

        #endregion


        #region Construction
        public Form_StateMonitor()
        {
            InitializeComponent();
            heartbeatMonitorService_Init();
        }
        #endregion
        #region Events

        /// <summary>
        /// 窗体load事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_StateMonitor_Load(object sender, EventArgs e)
        {
            flowLayoutPanel_StateDev.AutoScroll = false;
            flowLayoutPanel_StateDev.FlowDirection = FlowDirection.LeftToRight;
            flowLayoutPanel_StateDev.WrapContents = true;
            flowLayoutPanel_StateDev.HorizontalScroll.Maximum = 0;
            flowLayoutPanel_StateDev.AutoScroll = true;


            //从数据库加载数据，查询最近一次的测量时间
            string createTimeQuery = " SELECT Top 1 CreateTime FROM dbo.Config_Device_All  WHERE ProjectGUID = '{0}' ORDER BY CREATETIME DESC";
            DataTable dtTime = new DataTable();
            createTimeQuery = string.Format(createTimeQuery, Module_DeviceManage.Instance.ProjectGUID);
            dtTime = DbHelperSql.QueryDt(createTimeQuery);
            if (dtTime.Rows.Count == 0)
            {
                this.label_CalTime.Text = InterpretBase.textTran("未查询到测试记录");
            }
            else
            {
                this.label_CalTime.Text = dtTime.Rows[0]["CreateTime"].ToString();
            }        
        } 
        /// <summary>
        /// 焦点到流布局
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void flowLayoutPanel_StateDev_Click(object sender, EventArgs e)
        {
            flowLayoutPanel_StateDev.Focus();
        }
        #endregion

        #region Methods
        /// <summary>
        /// 记录更新定时器初始化
        /// </summary>
        public void timUpdateRecord_Init()
        {
            timUpdateRecord.Enabled = true;
            timUpdateRecord.AutoReset = true;
        }
        /// <summary>
        /// 启动定时器
        /// </summary>
        public void timUpdateRecord_Start()
        {
            timUpdateRecord.Elapsed += recordListen;
            timUpdateRecord.Start();
        }
        /// <summary>
        /// 关闭定时器
        /// </summary>
        public void timUpdateRecord_Stop()
        {
            timUpdateRecord.Stop();
            timUpdateRecord.Elapsed -= recordListen;
        }
        /// <summary>
        /// 定时器事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void recordListen(object sender, System.Timers.ElapsedEventArgs e)
        { }
        /// <summary>
        /// 启动心跳监听的服务
        /// </summary>
        public void heartbeatMonitorService_Init()
        {
            //仪器在线状态监听初始化
            modStateMonitor.devHeartbeatMonitor_Init();
        }
        /// <summary>
        /// 更新刷新按钮的状态
        /// </summary>
        public void Form_StateRefreshButton(bool bState)
        {
            this.button_Refresh.Enabled = bState;
        }
        /// <summary>
        /// 更新测试时间
        /// </summary>
        /// <param name="strTime"></param>
        public void Form_StateUpdateTestTime(string strTime)
        {
            label_CalTime.Text = strTime;
        }
        /// <summary>
        /// 更新记录信息的刷新（委托）
        /// </summary>
        /// <param name="strRecordNum"></param>
        /// <param name="strDataMiss"></param>
        /// <param name="strWritError"></param>
        public void update_RecordInfo(string strRecordNum, string strDataMiss, string strWritError)
        {
            if (this.InvokeRequired)
            {
                recordInfo_Refresh s = new recordInfo_Refresh(update_RecordInfo);
                this.Invoke(s, strRecordNum, strDataMiss, strWritError);
            }
            else
            {
                label_RecordNum.Text = strRecordNum;
                label_DataMiss.Text = strDataMiss;
                label_WritError.Text = strWritError;
            }
        }
        /// <summary>
        /// 监控窗体初始化
        /// </summary>
        public void Form_StateMonitorInit()
        {
            //设备信息初始化
            modStateMonitor.devSysInfoInit();
            //用户控件状态初始化
            modStateMonitor.ucDevStatesInit(true);
            //排序
            modStateMonitor.ucDevStatesList_SortByVirtNum();
            //清空现有布局
            flowLayoutPanel_StateDev.Controls.Clear();
            for (int i = 0; i < modStateMonitor.ucDevStatesList.Count; i++)
            {
                flowLayoutPanel_StateDev.Controls.Add(modStateMonitor.ucDevStatesList[i]);
                modStateMonitor.ucDevStatesList[i].updateDevStateUI();
            }
            foreach (Control item in flowLayoutPanel_StateDev.Controls)
            {
                item.Margin = new Padding(20);
            }
        }
        /// <summary>
        /// 更新监控窗体的UI
        /// </summary>
        public void update_StateMonitorUI()
        {
            if (bRunState == true)
            {
                label_State.BackColor = System.Drawing.Color.Green;
                label_State.ForeColor = System.Drawing.Color.White;
                label_State.Text = "RUN";
            }
            else
            {
                label_State.BackColor = System.Drawing.Color.Red;
                label_State.ForeColor = System.Drawing.Color.Black;
                label_State.Text = "STOP";
            }
        }
        #endregion
        /// <summary>
        /// 刷新事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Refresh_Click(object sender, EventArgs e)
        {
            Form_StateMonitorInit();
        }
        /// <summary>
        /// 状态监测界面清空
        /// </summary>
        public void stateMonitorForm_Clear()
        {
            bRunState = false;
            recordNum = 0;
            dataMissNum = 0;
            writeErrNum = 0;
            // 停止服务
            modStateMonitor.devHeartbeatMonitor_Stop();
            //清空现有布局
            flowLayoutPanel_StateDev.Controls.Clear();
            // 清空list内容
            modStateMonitor.devSysInfoList.Clear();
            // 清空list内容
            modStateMonitor.ucDevStatesList.Clear();
        }
        /// <summary>
        /// 查看校准数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ReadDetail_Click(object sender, EventArgs e)
        {
            Form_CalibrationInfo form = new Form_CalibrationInfo();
            form.ShowDialog();                     
        }
    }
}
