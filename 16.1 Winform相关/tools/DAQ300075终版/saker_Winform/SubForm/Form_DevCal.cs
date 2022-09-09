using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using saker_Winform.Module;
using saker_Winform.CommonBaseModule;
using saker_Winform.Global;
using System.Threading;
using saker_Winform.DataBase;
using saker_Winform.UserControls;
using ClassLibrary_MultiLanguage;

namespace saker_Winform.SubForm
{
    public partial class Form_DevCal : Form
    {
        #region Fields
        private bool bChanDelayCal = true;// true=>通道延时校准;false=>设备延时校准
        private DataTable dtChanDelayCal = new DataTable("Table_ChanDelayCal");// 新建通道延时校准的配置表
        private DataTable dtDevDelayCal = new DataTable("Table_DevDelayCal");// 新建用于设备延时校准的配置表
        private DataTable drDevCalResult = new DataTable("Table_DevCalResult");// 用于存储最新的校准结果
        List<devCalInfo> devCalInfoList = new List<devCalInfo>(); // 设备连接信息链表
        private bool bCaling = false;//正在校准的状态
        private bool bSysCalCompeleted = false;// 系统校准完成标志
        //private bool bStart = false;
        //private int devCalNum;//设备校准数量
        // 类定义
        private class devCalInfo
        {
            public string devName = "";//设备名
            public string devIp = "";// 设备IP
            public string devSN = "";//设备序列号
            public double offsetDelay = 0.0;//根据线长转换出的水平偏移
            public Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);// socket 连接
            public bool bCalCompelete = false;// 校准完成
            public bool bCalSuccessed = false;// 校准成功标志
            public int devDelayTime = 0;//设备外触发延时
            public string lineLength = "0";
        }

        private delegate void dataGridViewRefresh(ref DataTable dt);//申明数据绑定的委托
        private delegate void dispProcessRefresh(string strTemp);//申明委托
        private delegate DialogResult MessageBoxShow(string msg1, string msg2);// 消息框弹出委托
        private delegate void progressBarRefresh();// 进度条刷新委托
        private delegate void buttonStateReset();// 进度条刷新委托

        #endregion
        #region Construction
        /// <summary>
        /// 构造函数
        /// </summary>
        public Form_DevCal()
        {
            InitializeComponent();
        }
        #endregion

        #region Events

        #region 自定义事件
        /*定义事件参数类*/

        public class clickDevCalFormEventArgs : EventArgs
        {
            public readonly string KeyToRaiseEvent;

            public clickDevCalFormEventArgs(string keyToRaiseEvent)
            {
                KeyToRaiseEvent = keyToRaiseEvent;
            }
        }
        /*定义委托声明*/
        public delegate void clickDevCalFormEventHandler(object sender, clickDevCalFormEventArgs e);

        //用event关键字声明事件对象
        public event clickDevCalFormEventHandler clickDevCalFormEvent;

        //事件触发方法
        protected virtual void onClickDevCalFormEvent(clickDevCalFormEventArgs e)
        {
            if (clickDevCalFormEvent != null)
            {
                clickDevCalFormEvent(this, e);
            }
        }

        //引发事件
        private void RaiseEvent(string keyToRaiseEvent)
        {
            clickDevCalFormEventArgs e = new clickDevCalFormEventArgs(keyToRaiseEvent);

            onClickDevCalFormEvent(e);
        }

        #endregion

        /// <summary>
        /// load事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_DevCal_Load(object sender, EventArgs e)
        {
            dataGridView_DevChoose.AllowUserToAddRows = false;// 禁用最后一行空白
            dataGridView_DevChoose.AllowUserToResizeRows = false;// 禁拖动行高度
            dataGridView_DevChoose.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;//禁止最左侧空白列宽度
            dataGridView_DevChoose.AllowUserToDeleteRows = false;

            button_CancelAll.Enabled = false;
            button_SelectAll.Enabled = false;

            dtChanDelayCal.Columns.Add("ALL", typeof(Boolean));
            dtChanDelayCal.Columns.Add("设备别名", typeof(String));
            dtChanDelayCal.Columns.Add("设备SN", typeof(String));
            dtChanDelayCal.Columns.Add("设备IP", typeof(String));
            dtChanDelayCal.Columns.Add("设备型号", typeof(String));
            dtChanDelayCal.Columns.Add("机器编号", typeof(String));
            dtChanDelayCal.Columns.Add("校准状态", typeof(String));
            dtChanDelayCal.Columns.Add("线长(m)", typeof(String));

            dtDevDelayCal.Columns.Add("ALL", typeof(Boolean));
            dtDevDelayCal.Columns.Add("设备别名", typeof(String));
            dtDevDelayCal.Columns.Add("设备SN", typeof(String));
            dtDevDelayCal.Columns.Add("设备IP", typeof(String));
            dtDevDelayCal.Columns.Add("设备型号", typeof(String));
            dtDevDelayCal.Columns.Add("机器编号", typeof(String));
            dtDevDelayCal.Columns.Add("校准状态", typeof(String));
            dtDevDelayCal.Columns.Add("线长(m)", typeof(String));

            drDevCalResult.Columns.Add("GUID", typeof(string));
            drDevCalResult.Columns.Add("Name", typeof(string));
            drDevCalResult.Columns.Add("IP", typeof(string));
            drDevCalResult.Columns.Add("SerialNumber", typeof(string));
            drDevCalResult.Columns.Add("DeviceDelayTime", typeof(int));
            drDevCalResult.Columns.Add("ChanDelayCalTime", typeof(DateTime));
            drDevCalResult.Columns.Add("DevDelayCalTime", typeof(DateTime));
            drDevCalResult.Columns.Add("LineLength", typeof(string));

            this.dataGridView_DevChoose.DataSource = dtDevDelayCal;
        }
        /// <summary>
        /// 通道延时校准切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconButton_ChanCal_Click(object sender, EventArgs e)
        {
            iconButton_ChanCal.BackColor = System.Drawing.Color.FromArgb(198, 198, 198);
            iconButton_ChanCal.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));

            iconButton_DevCal.BackColor = System.Drawing.Color.FromArgb(230, 230, 230);
            iconButton_DevCal.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));

            label_Title.Text = "单机校准";

            bChanDelayCal = true;

            dataSourceBinding(ref dtChanDelayCal);
            dataGridView_DevChoose.Columns[dataGridView_DevChoose.ColumnCount - 1].Visible = false;
        }
        /// <summary>
        /// 外触发延时校准切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconButton_DevCal_Click(object sender, EventArgs e)
        {
            iconButton_ChanCal.BackColor = System.Drawing.Color.FromArgb(230, 230, 230);
            iconButton_ChanCal.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));

            iconButton_DevCal.BackColor = System.Drawing.Color.FromArgb(198, 198, 198);
            iconButton_DevCal.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));

            label_Title.Text = "系统校准";

            bChanDelayCal = false;

            dataSourceBinding(ref dtDevDelayCal);
            dataGridView_DevChoose.Columns[dataGridView_DevChoose.ColumnCount - 1].ReadOnly = false;
            dataGridView_DevChoose.Columns[dataGridView_DevChoose.ColumnCount - 1].Visible = true;
        }
        /// <summary>
        /// datagridview行自动编号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_DevChoose_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            // 自动编号，与数据无关
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
               e.RowBounds.Location.Y,
               dataGridView_DevChoose.RowHeadersWidth - 4,
               e.RowBounds.Height);
            TextRenderer.DrawText(e.Graphics,
                  (e.RowIndex + 1).ToString(),
                   dataGridView_DevChoose.RowHeadersDefaultCellStyle.Font,
                   rectangle,
                   dataGridView_DevChoose.RowHeadersDefaultCellStyle.ForeColor,
                   TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }
        /// <summary>
        /// 加载设备信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconButton_LoadDev_Click(object sender, EventArgs e)
        {
            DataTable dt = this.dataGridView_DevChoose.DataSource as DataTable;
            dtChanDelayCal = dt.Clone();
            dtDevDelayCal = dt.Clone();         
            // 清除管理的数据表
            if (dtChanDelayCal!=null)
            {
                dtChanDelayCal.Rows.Clear();
            }
            if(dtDevDelayCal != null)
            {
                dtDevDelayCal.Rows.Clear();
            }
           
            // 构造数据表 
            foreach (var item in Module_DeviceManage.Instance.Devices)
            {
                dtChanDelayCal.Rows.Add(0, item.Name, item.SN, item.IP, item.Model, item.VirtualNumber, "", "0");
                dtDevDelayCal.Rows.Add(0, item.Name, item.SN, item.IP, item.Model, item.VirtualNumber, "","0");
            }
            if (bChanDelayCal)
            {
                dataSourceBinding(ref dtChanDelayCal);
                dataGridView_DevChoose.Columns[dataGridView_DevChoose.ColumnCount - 1].Visible = false;
            }
            else
            {
                dataSourceBinding(ref dtDevDelayCal);
                dataGridView_DevChoose.Columns[dataGridView_DevChoose.ColumnCount - 1].ReadOnly = false ;
                dataGridView_DevChoose.Columns[dataGridView_DevChoose.ColumnCount - 1].Visible = true;
            }
            //按键更新
            if (dataGridView_DevChoose.Rows.Count != 0)
            {
                button_SelectAll.Enabled = true;
                button_CancelAll.Enabled = true;
            }

            this.Refresh();

        }
        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_SelectAll_Click(object sender, EventArgs e)
        {
            if (bChanDelayCal)
            {
                DataRow[] datarow = dtChanDelayCal.Select();
                for (int i = 0; i < datarow.Length; i++)
                {
                    datarow[i]["ALL"] = true;
                }
                dataSourceBinding(ref dtChanDelayCal);
            }
            else
            {
                DataRow[] datarow = dtDevDelayCal.Select();
                for (int i = 0; i < datarow.Length; i++)
                {
                    datarow[i]["ALL"] = true;
                }
                dataSourceBinding(ref dtDevDelayCal);
                dataGridView_DevChoose.Columns[dataGridView_DevChoose.ColumnCount - 1].ReadOnly = false;
            }

        }
        /// <summary>
        /// 取消全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_CancelAll_Click(object sender, EventArgs e)
        {
            if (bChanDelayCal)
            {
                DataRow[] datarow = dtChanDelayCal.Select();
                for (int i = 0; i < datarow.Length; i++)
                {
                    datarow[i]["ALL"] = false;
                }
                dataSourceBinding(ref dtChanDelayCal);
            }
            else
            {
                DataRow[] datarow = dtDevDelayCal.Select();
                for (int i = 0; i < datarow.Length; i++)
                {
                    datarow[i]["ALL"] = false;
                }
                dataSourceBinding(ref dtDevDelayCal);
            }
        }

        #region 单元格输入合法性验证
        private void dataGridView_DevChoose_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == 7)
            {
                if (dataGridView_DevChoose.Rows[e.RowIndex].IsNewRow) { return; }
                double newInteger;
                if (!double.TryParse(e.FormattedValue.ToString(), out newInteger) || newInteger < 0)
                {
                    e.Cancel = true;
                    MessageBox.Show(InterpretBase.textTran("此输入不合法，请重新输入"));
                }
            }
        }
        #endregion
        /// <summary>
        /// 开始校准事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_CalStart_Click(object sender, EventArgs e)
        {
            //进度条的初始值
            progressBar_Cal.Value = 0;
            // 按键失效
            button_CalStart.Enabled = false;
            iconButton_ChanCal.Enabled = false;
            iconButton_DevCal.Enabled = false;
            RaiseEvent("DevCalClick");
            if (bChanDelayCal)
            {
                DialogResult dr = MessageBox.Show(InterpretBase.textTran("请确保同步机输出打开！") + "\n" + InterpretBase.textTran("被校准设备的所有输入通道均与同步机连接完好！"),
                     InterpretBase.textTran("点击确认开始校准"),
                     MessageBoxButtons.OKCancel,
                     MessageBoxIcon.Asterisk);
                if (dr == DialogResult.OK)
                {
                    // 根据显示的数据表，更新dtChanDelayCal
                    if (dtChanDelayCal != null)
                    {
                        dtChanDelayCal.Rows.Clear();
                    }
                    dtChanDelayCal = (DataTable)dataGridView_DevChoose.DataSource;
                    if (dtChanDelayCal == null)
                    {
                        buttonState_Reset();
                        MessageBox.Show(InterpretBase.textTran("未选择校准设备!"), InterpretBase.textTran("提示信息"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    DataRow[] dataRow = dtChanDelayCal.Select("ALL = 'True'");
                    if (dataRow.Length == 0)
                    {
                        buttonState_Reset();
                        MessageBox.Show(InterpretBase.textTran("未选择校准设备!"), InterpretBase.textTran("提示信息"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // 查一次仪器的在线状态
                    for (int i = 0; i < dataRow.Length; i++)
                    {
                        // 选中的加入到链表
                        if (true == (bool)dataRow[i]["ALL"])
                        {
                            bool bReturn = true;
                            for (int j = 0; j < 4; j++)
                            {
                                bReturn = CPingIP.PingIpConnect(dataRow[i]["设备IP"].ToString());
                                Thread.Sleep(50);
                                if (bReturn == true)
                                {
                                    break;
                                }
                            }
                            //bool bReturn = CPingIP.PingIpConnect(dataRow[i]["设备IP"].ToString());
                            if (false == bReturn)
                            {
                                //设备不在线
                                MessageBox.Show(dataRow[i]["设备别名"].ToString() + "/" + dataRow[i]["设备IP"].ToString() + InterpretBase.textTran("设备不在线，请检查仪器LAN口连接！"),
                                    InterpretBase.textTran("警告"),
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                                //按键状态复位
                                buttonState_Reset();
                                return;
                            }
                        }
                    }

                    processRefresh("设备开始通道延时校准");
                    bSysCalCompeleted = false;
                    //发送开始校准指令
                    chanDelayCal_Init();
                    //设置进度条的最大值
                    progressBar_Cal.Maximum = devCalInfoList.Count;
                    // 启动查询任务=》新线程
                    getDevCal_Status();
                    //启动新线程等待校准完成
                    Task task = new Task(() =>
                        {
                            while (bSysCalCompeleted == false)
                            {
                                Thread.Sleep(500);// 等待校准完成
                            }
                            bool bStorage = true;
                            for (int i = 0; i < devCalInfoList.Count; i++)
                            {
                                if (devCalInfoList[i].bCalSuccessed == false)
                                {
                                    ShowMessage(devCalInfoList[i].devSN + "/" + devCalInfoList[i].devIp + "校准失败", "校准失败！");
                                    break;
                                }
                            }
                            if (bStorage)
                            {
                                DialogResult drCalResult = ShowMessage("校准结束，是否存储数据库", "点击确认开始存储数据库");
                                if (drCalResult == System.Windows.Forms.DialogResult.OK)
                                {
                                    drDevCalResult.Rows.Clear();
                                    for (int i = 0; i < devCalInfoList.Count; i++)
                                    {
                                       // if (devCalInfoList[i].bCalSuccessed)
                                        {
                                            DataRow dataRowResult = drDevCalResult.NewRow();
                                            dataRowResult["GUID"] = Guid.NewGuid();
                                            dataRowResult["Name"] = devCalInfoList[i].devName;
                                            dataRowResult["IP"] = devCalInfoList[i].devIp;
                                            dataRowResult["SerialNumber"] = devCalInfoList[i].devSN;
                                            dataRowResult["ChanDelayCalTime"] = DateTime.Now;
                                            drDevCalResult.Rows.Add(dataRowResult);
                                        }
                                    }
                                    // 数据库相关操作
                                    DbHelperSql.SqlBulkCopyByDatatable("Data_Calibration_Device", drDevCalResult);
                                    MessageBox.Show(InterpretBase.textTran("写入数据库完成！"));
                                }
                                else
                                {
                                }
                            }
                            //关闭连接，释放资源
                            devConnect_Close();
                            //按键状态复位
                            buttonState_Reset();
                        });
                    task.Start();
                }
                else
                {
                    //按键状态复位
                    buttonState_Reset();
                }
            }
            else
            {
                DialogResult dr = MessageBox.Show(InterpretBase.textTran("请确保同步机输出打开！") + "\n" + InterpretBase.textTran("请确保每台被校准设备的CH1通道和与外触发连接完好！")
                    + "\n" + InterpretBase.textTran("机器间的时钟信号需要通过10M IN/OUT进行级联！"),
                    InterpretBase.textTran("点击确认开始校准"),
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Asterisk);
                if (dr == DialogResult.OK)
                {
                    // 根据显示的数据表，更新dtChanDelayCal
                    //dtDevDelayCal.Rows.Clear();
                    dtDevDelayCal = (DataTable)dataGridView_DevChoose.DataSource;
                    if (dtDevDelayCal == null)
                    {
                        buttonState_Reset();
                        MessageBox.Show(InterpretBase.textTran("未选择校准设备!"), InterpretBase.textTran("提示信息"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    //DataRow[] dataRow = dtDevDelayCal.Select("ALL = 'True'");
                    DataRow[] dataRow = dtDevDelayCal.Select("ALL = 'True'");
                    if (dataRow.Length == 0)
                    {
                        buttonState_Reset();
                        MessageBox.Show(InterpretBase.textTran("未选择校准设备!"), InterpretBase.textTran("提示信息"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    // 查一次仪器的在线状态
                    for (int i = 0; i < dataRow.Length; i++)
                    {
                        // 选中的加入到链表
                        if (true == (bool)dataRow[i]["ALL"])
                        {
                            bool bReturn = true;
                            for (int j = 0; j < 4; j++)
                            {
                                bReturn = CPingIP.PingIpConnect(dataRow[i]["设备IP"].ToString());
                                Thread.Sleep(50);
                                if (bReturn == true)
                                {
                                    break;
                                }
                            }
                            //bool bReturn = CPingIP.PingIpConnect(dataRow[i]["设备IP"].ToString());
                            if (false == bReturn)
                            {
                                //设备不在线
                                MessageBox.Show(dataRow[i]["设备别名"].ToString() + "/" + dataRow[i]["设备IP"].ToString() + InterpretBase.textTran("设备不在线，请检查仪器LAN口连接！"),
                                    InterpretBase.textTran("警告"),
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                                //按键状态复位
                                buttonState_Reset();
                                return;
                            }
                        }
                    }

                    processRefresh(InterpretBase.textTran("设备开始外触发延时校准"));
                    bSysCalCompeleted = false;
                    //发送开始校准指令
                    devDelayCal_Init();
                    //设置进度条的最大值
                    progressBar_Cal.Maximum = devCalInfoList.Count;
                    // 启动查询任务=》新线程
                    getDevCal_Status();
                    //启动新线程等待校准完成
                    Task task = new Task(() =>
                    {
                        while (bSysCalCompeleted == false)
                        {
                            Thread.Sleep(500);// 等待校准完成
                        }
                        bool bStorage = true;
                        for (int i = 0; i < devCalInfoList.Count; i++)
                        {
                            if (devCalInfoList[i].bCalSuccessed == false)
                            {
                                ShowMessage(devCalInfoList[i].devSN + "/" + devCalInfoList[i].devIp + InterpretBase.textTran("校准失败"), InterpretBase.textTran("校准失败！"));
                                break;
                            }
                        }

                        if (bStorage)
                        {
                            DialogResult drCalResult = ShowMessage(InterpretBase.textTran("校准结束，是否存储数据库"), InterpretBase.textTran("点击确认开始存储数据库"));
                            if (drCalResult == System.Windows.Forms.DialogResult.OK)
                            {
                                drDevCalResult.Rows.Clear();
                                for (int i = 0; i < devCalInfoList.Count; i++)
                                {
                                    if (devCalInfoList[i].bCalSuccessed)
                                    {
                                        DataRow dataRowResult = drDevCalResult.NewRow();
                                        dataRowResult["GUID"] = Guid.NewGuid();
                                        dataRowResult["Name"] = devCalInfoList[i].devName;
                                        dataRowResult["IP"] = devCalInfoList[i].devIp;
                                        dataRowResult["SerialNumber"] = devCalInfoList[i].devSN;
                                        dataRowResult["DeviceDelayTime"] = Convert.ToInt32(devCalInfoList[i].devDelayTime);
                                        dataRowResult["DevDelayCalTime"] = DateTime.Now;
                                        dataRowResult["LineLength"] = devCalInfoList[i].lineLength;
                                        drDevCalResult.Rows.Add(dataRowResult);
                                    }
                                }
                                // 数据库相关操作
                                DbHelperSql.SqlBulkCopyByDatatable("Data_Calibration_Info", drDevCalResult);
                                MessageBox.Show(InterpretBase.textTran("写入数据库完成！"));
                            }
                            else
                            {
                            }
                        }
                        //关闭连接，释放资源
                        devConnect_Close();
                        //按键状态复位
                        buttonState_Reset();
                    });
                    task.Start();
                }
                else
                {
                    //按键状态复位
                    buttonState_Reset();
                }
            }
        }


        #endregion

        #region Methods
        /// <summary>
        /// 定义弹出消息框的委托事件
        /// </summary>
        /// <param name="msgContent"></param>
        /// <param name="msgTitle"></param>
        /// <returns></returns>
        public DialogResult ShowMessage(string msgContent, string msgTitle)
        {

            DialogResult dr = (DialogResult)this.Invoke(new MessageBoxShow(MessageBoxShow_F), new object[] { msgContent, msgTitle });
            return dr;
        }
        /// <summary>
        /// 委托方法
        /// </summary>
        /// <param name="msgContent"></param>
        /// <param name="msgTitle"></param>
        /// <returns></returns>
        DialogResult MessageBoxShow_F(string msgContent, string msgTitle)
        {
            DialogResult dr = MessageBox.Show(msgContent,
                    msgTitle,
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Asterisk);
            //DialogResult dr = MessageBox.Show("校准结束，是否存储数据库",
            //        "点击确认开始存储数据库",
            //        MessageBoxButtons.OKCancel,
            //        MessageBoxIcon.Asterisk);
            //System.Windows.Forms.MessageBox.Show(msg, "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return dr;
        }
        /// <summary>
        /// 刷新数据绑定委托
        /// </summary>
        private void dataSourceBinding(ref DataTable dt)
        {
            if (dataGridView_DevChoose.InvokeRequired)
            {
                dataGridViewRefresh s = new dataGridViewRefresh(dataSourceBinding);
                dataGridView_DevChoose.Invoke(s, dt);
            }
            else
            {
                dataGridView_DevChoose.DataSource = dt.Copy();
                //列设置为只读*/
                for (int i = 1; i < dataGridView_DevChoose.ColumnCount; i++)
                {
                    dataGridView_DevChoose.Columns[i].ReadOnly = true;
                }
            }
        }
        /// <summary>
        /// 通道延时校准开始
        /// </summary>
        private void chanDelayCal_Init()
        {
            // 清除已有的连接
            foreach (devCalInfo item in devCalInfoList)
            {
                item.socket.Close();
            }
            // 清除链表
            devCalInfoList.Clear();

            DataRow[] dataRow = dtChanDelayCal.Select();
            // 更新list内容
            for (int i = 0; i < dataRow.Length; i++)
            {
                // 选中的加入到链表
                if (true == (bool)dataRow[i]["ALL"])
                {
                    devCalInfo devTemp = new devCalInfo();
                    devTemp.devName = dataRow[i]["设备别名"].ToString();
                    devTemp.devIp = dataRow[i]["设备IP"].ToString();
                    devTemp.devSN = dataRow[i]["设备SN"].ToString();
                    devTemp.socket.Connect(devTemp.devIp, 5555);
                    devCalInfoList.Add(devTemp);
                }
            }
            // 发送开始校准命令
            foreach (devCalInfo item in devCalInfoList)
            {
                //string command = CGlobalCmd.STR_CMD_SET_CHANNELS + "0\n";
                //item.socket.Send(Encoding.Default.GetBytes(command));
                //Thread.Sleep(50);
                //command = ":run\n";
                //item.socket.Send(Encoding.Default.GetBytes(command));
                //Thread.Sleep(200);
                string command = CGlobalCmd.STR_CMD_CAL_CHAN_DELAY + "\n";
                item.socket.Send(Encoding.Default.GetBytes(command));
            }
        }

        /// <summary>
        /// 设备外触发校准
        /// </summary>
        private void devDelayCal_Init()
        {
            // 清除已有的连接
            foreach (devCalInfo item in devCalInfoList)
            {
                item.socket.Close();
            }
            // 清除链表
            devCalInfoList.Clear();

            DataRow[] dataRow = dtDevDelayCal.Select();
            // 更新list内容
            for (int i = 0; i < dataRow.Length; i++)
            {
                // 选中的加入到链表
                if (true == (bool)dataRow[i]["ALL"])
                {
                    devCalInfo devTemp = new devCalInfo();
                    devTemp.devName = dataRow[i]["设备别名"].ToString();
                    devTemp.devIp = dataRow[i]["设备IP"].ToString();
                    devTemp.devSN = dataRow[i]["设备SN"].ToString();
                    //1m折算到6.5ns
                    devTemp.offsetDelay = Convert.ToDouble(dataRow[i]["线长(m)"].ToString()) * 6.5*1E-9;
                    devTemp.socket.Connect(devTemp.devIp, 5555);
                    devTemp.lineLength = dataRow[i]["线长(m)"].ToString();
                    devCalInfoList.Add(devTemp);
                }
            }
            // 发送开始校准命令
            foreach (devCalInfo item in devCalInfoList)
            {
                // 设置水平偏移
                string command = CGlobalCmd.STR_CMD_SET_HORIZONTAOFFSET + item.offsetDelay.ToString() + "\n";
                item.socket.Send(Encoding.Default.GetBytes(command));
                Thread.Sleep(50);
                //command = CGlobalCmd.STR_CMD_SET_CHANNELS + "0\n";
                //item.socket.Send(Encoding.Default.GetBytes(command));
                //Thread.Sleep(50);
                //command = ":run\n";
                //item.socket.Send(Encoding.Default.GetBytes(command));
                //Thread.Sleep(200);
                // 开始校准
                command = CGlobalCmd.STR_CMD_CAL_TRIG_DELAY + "\n";
                item.socket.Send(Encoding.Default.GetBytes(command));
            }
        }
        /// <summary>
        /// 关闭所有设备的连接
        /// </summary>
        private void devConnect_Close()
        {
            // 清除已有的连接
            foreach (devCalInfo item in devCalInfoList)
            {
                item.socket.Close();
            }
            // 清除链表
            devCalInfoList.Clear();
        }
        /// <summary>
        /// 获取校准状态
        /// </summary>
        private void getDevCal_Status()
        {
            Task task = new Task(() =>
                {
                    do
                    {
                        bCaling = false;
                        string strTextBoxView = "";
                        foreach (devCalInfo item in devCalInfoList)
                        {
                            if (item.bCalCompelete == false)
                            {
                                try
                                {
                                    bCaling = true;
                                    string command = CGlobalCmd.STR_CMD_GET_CAL_STATUS + "\n";
                                    item.socket.Send(Encoding.Default.GetBytes(command));
                                    Thread.Sleep(50);
                                    byte[] byteReadBuf = new byte[128];
                                    var retCount = item.socket.Receive(byteReadBuf);
                                    string result = Encoding.Default.GetString(byteReadBuf, 0, retCount);
                                    result = result.Remove(result.Length - 1);//移除\n字符
                                    DataRow[] datarow = null;
                                    if (bChanDelayCal)
                                    {
                                        // 通道延时校准
                                        datarow = dtChanDelayCal.Select("设备SN = '" + item.devSN + "'");
                                    }
                                    else
                                    {
                                        // 设备外触发校准
                                        datarow = dtDevDelayCal.Select("设备SN = '" + item.devSN + "'");
                                    }
                                    switch (result)
                                    {
                                        case "0":
                                        case "1":
                                            item.bCalCompelete = false;
                                            item.bCalSuccessed = false;
                                            strTextBoxView = item.devSN + "/" + item.devIp + " 校准启动中.";
                                            processRefresh(strTextBoxView);

                                            if (datarow.Length != 0)
                                            {
                                                datarow[0]["校准状态"] = "校准启动中";
                                            }
                                            break;
                                        case "2":
                                            item.bCalCompelete = false;
                                            item.bCalSuccessed = false;
                                            strTextBoxView = item.devSN + "/" + item.devIp + " 校准中...";
                                            processRefresh(strTextBoxView);
                                            if (datarow.Length != 0)
                                            {
                                                datarow[0]["校准状态"] = "校准中...";
                                            }
                                            break;
                                        case "3":
                                            // 校准完成
                                            item.bCalCompelete = true;
                                            //校准成功
                                            item.bCalSuccessed = true;
                                            strTextBoxView = item.devSN + "/" + item.devIp + " 校准完成.";
                                            // 如果是设备校准
                                            if (bChanDelayCal == false)
                                            {
                                                command = CGlobalCmd.STR_CMD_GET_DEV_CAL_RESULT + "\n";
                                                item.socket.Send(Encoding.Default.GetBytes(command));
                                                Thread.Sleep(50);
                                                retCount = item.socket.Receive(byteReadBuf);
                                                result = Encoding.Default.GetString(byteReadBuf, 0, retCount);
                                                result = result.Remove(result.Length - 1);//移除\n字符
                                                item.devDelayTime = Convert.ToInt32(result);
                                                strTextBoxView = strTextBoxView + " 外触发延时:" + item.devDelayTime.ToString() + "ps";
                                            }
                                            processRefresh(strTextBoxView);
                                            if (datarow.Length != 0)
                                            {
                                                datarow[0]["校准状态"] = "校准完成！";
                                            }
                                            progressBar_Refresh();
                                            break;
                                        case "4":
                                            // 校准完成
                                            item.bCalCompelete = true;
                                            //校准失败
                                            item.bCalSuccessed = false;
                                            strTextBoxView = item.devSN + "/" + item.devIp + " 校准失败.";
                                            processRefresh(strTextBoxView);
                                            if (datarow.Length != 0)
                                            {
                                                datarow[0]["校准状态"] = "校准失败！";
                                            }
                                            progressBar_Refresh();
                                            break;
                                        default:
                                            break;

                                    }
                                }
                                catch (Exception e)
                                {
                                    bSysCalCompeleted = true;//发生异常时，提前校准完成
                                    return;
                                }

                            }
                            else
                            {

                            }
                        }
                        if (bChanDelayCal)
                        {
                            dataSourceBinding(ref dtChanDelayCal);
                        }
                        else
                        {
                            dataSourceBinding(ref dtDevDelayCal);
                        }
                        Thread.Sleep(5000);
                    } while (bCaling);
                    bSysCalCompeleted = true;//校准完成
                }
            );
            task.Start();
        }

        /// <summary>
        /// text刷新内容委托
        /// </summary>
        /// <param name="strTemp"></param>
        private void processRefresh(string strTemp)
        {

            if (this.textBox_CalInfo.InvokeRequired)
            {
                dispProcessRefresh s = new dispProcessRefresh(processRefresh);
                this.Invoke(s, strTemp);
            }
            else
            {
                textBox_CalInfo.AppendText(strTemp + "\r\n");
            }
        }
        /// <summary>
        /// 进度条刷新
        /// </summary>
        private void progressBar_Refresh()
        {
            if (this.InvokeRequired)
            {
                progressBarRefresh s = new progressBarRefresh(progressBar_Refresh);
                this.Invoke(s);
            }
            else
            {
                progressBar_Cal.Value++;
            }
        }
        /// <summary>
        /// 案件状态复位
        /// </summary>
        private void buttonState_Reset()
        {
            RaiseEvent("DevCalNoClick");
            if (this.InvokeRequired)
            {
                buttonStateReset s = new buttonStateReset(buttonState_Reset);
                this.Invoke(s);
            }
            else
            {
                button_CalStart.Enabled = true;
                iconButton_ChanCal.Enabled = true;
                iconButton_DevCal.Enabled = true;              
            }
        }
        /// <summary>
        /// 窗体数据清空
        /// </summary>
        public void devCalForm_Clear()
        {
            // 关闭所有设备的连接
            devConnect_Close();
            // 清空所有DT
            bChanDelayCal = true;// true=>通道延时校准;false=>设备延时校准
            dtChanDelayCal.Rows.Clear();
            dtDevDelayCal.Rows.Clear();
            drDevCalResult.Rows.Clear();
            bCaling = false;//正在校准的状态
            bSysCalCompeleted = false;// 系统校准完成标志
        }
        #endregion







    }
}