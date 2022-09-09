//#define Debug

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using saker_Winform.UserControls;
using ClassLibrary_WfmDataProcess;
using System.IO;
using System.Drawing.Drawing2D;
using saker_Winform.CommonBaseModule;
using saker_Winform.Global;
using saker_Winform.Module;
using saker_Winform.DataBase;
using System.Drawing.Imaging;
using ClassLibrary_MultiLanguage;



/*****************************************************************************************************************
                                        普源精电科技有限公司版权所有(2020-)
******************************************************************************************************************
类名:      Form_WaveMonitor
功能描述： Form_WaveMonitor类中定义了波形监控界面
作 者：    顾泽滔
版 本：    00.01.00.00
完成日期： 
修改历史： 
<作者>               <修改时间>               <版本>                    <修改描述>
*****************************************************************************************************************/

namespace saker_Winform.SubForm
{
    public partial class Form_WaveMonitor : Form
    {
        #region Fields
        //private bool isLoadWavaData = false;//表示当前是否已从数据库加载
        private string strFormNum;// 表示当前是哪个窗体
        /// <summary>
        /// 绘图区域的画板宽度
        /// </summary>
        private float boardWidth;
        //private float boardWidthAutoZone;
        /// <summary>
        /// 画板高度
        /// </summary>
        private float boardHeight;
        /// <summary>
        /// 垂直（纵向）边距（画图区域距离左右两边长度）
        /// </summary>
        private float verticalMargin;

        /// <summary>
        /// 平行（横向）边距（画图区域距离左右两边长度）
        /// </summary>
        private float horizontalMargin;

        /// <summary>
        /// 水平间距像素
        /// </summary>
        private float horizontalBetween;

        /// <summary>
        /// 垂直间距像素
        /// </summary>
        private float verticalBetween;

        /// <summary>
        /// 真实画布宽度
        /// </summary>
        private float canvasWidth;

        /// <summary>
        /// 图表区域宽度
        /// </summary>
        float chartWidth;

        /// <summary>
        /// 图表区域高度
        /// </summary>
        float charHeight;

        /// <summary>
        /// 画图区域起点
        /// </summary>
        PointF startPostion;

        /// <summary>
        /// 画图区域终点
        /// </summary>
        PointF endPostion;
        /// <summary>
        /// Y轴每个间隔值
        /// </summary>
        private float intervalValueY;

        /// <summary>
        /// x轴的点的个数
        /// </summary>
        private int xScaleCount = 1000;

        /// <summary>
        /// y轴的点的个数
        /// </summary>
        private int yScaleCount = 256;
        /// <summary>
        /// 判断chanLabel用户控件的鼠标是否按下
        /// </summary>
        private bool isChanLabelMouseDown = false;
        /// <summary>
        /// 记录chanLabel相对起始点的x偏移
        /// </summary>
        private int axChanLabel;
        /// <summary>
        /// 记录chanLabel相对起始点的y偏移
        /// </summary>
        private int ayChanLabel;
        /// <summary>
        /// 是否显示X轴坐标值
        /// </summary>
        private bool bXMarking = false;
        public bool m_bXMarking
        {
            get
            {
                return bXMarking;
            }
            set
            {
                bXMarking = value;
            }
        }

        /// <summary>
        /// 是否显示Y轴坐标值
        /// </summary>
        private bool bYMarking = true;
        public bool m_bYMarking
        {
            get
            {
                return bYMarking;
            }
            set
            {
                bYMarking = value;
            }
        }
        /// <summary>
        /// 是否绘制x轴的刻度线
        /// </summary>
        private bool bDrawGradLineX = true;
        public bool m_bDrawGradLineX
        {
            get
            {
                return bDrawGradLineX;
            }
            set
            {
                bDrawGradLineX = value;
            }
        }
        /// <summary>
        /// 是否绘制y轴的刻度线
        /// </summary>
        private bool bDrawGradLineY = true;
        public bool m_bDrawGradLineY
        {
            get
            {
                return bDrawGradLineY;
            }
            set
            {
                bDrawGradLineY = value;
            }
        }
        /// <summary>
        /// 波形显示y轴范围的最大值，单位Vpp
        /// </summary>
        private float maxValueY = 10.0F;
        public float m_maxValueY
        {
            get
            {
                return maxValueY;
            }
            set
            {
                if (value <= minValueY)
                {
                    MessageBox.Show(InterpretBase.textTran("最大值不能低于最小值！"), InterpretBase.textTran("警告"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    maxValueY = value;
                }
            }
        }
        //autozone下不同模式的显示范围
        private int autZonLittleScale = 32;
        private int autZonNormalScale = 16;
        private int autZonLargeScale = 8;
        private int curPosition = 0;
        private bool bLittleScale = false;
        private bool bNormalScale = true;
        private bool bLargeScale = false;
        private int autZonRange = 0;
        private static object objlock = new object();
        /// <summary>
        /// 波形显示y轴范围的最小值，单位Vpp,归一化下的幅度范围
        /// </summary>
        private float minValueY = -10.0F;
        public float m_minValueY
        {
            get
            {
                return minValueY;
            }
            set
            {
                if (value >= maxValueY)
                {
                    MessageBox.Show(InterpretBase.textTran("最小值不能超过最大值！"), InterpretBase.textTran("警告"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    minValueY = value;
                }
            }

        }
        /// <summary>
        /// 波形显示x轴范围的最大值，单位s
        /// </summary>
        private double maxValueX = 0.00001;
        public double m_maxValueX
        {
            get
            {
                return maxValueX;
            }
            set
            {
                if (value <= minValueX)
                {
                    MessageBox.Show(InterpretBase.textTran("最大值不能低于最小值！"), InterpretBase.textTran("警告"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    maxValueX = value;
                }
            }
        }
        /// <summary>
        /// 波形显示x轴范围的最小值，单位s
        /// </summary>
        private double minValueX = -0.00001;
        public double m_minValueX
        {
            get
            {
                return minValueX;
            }
            set
            {
                if (value >= maxValueX)
                {
                    MessageBox.Show(InterpretBase.textTran("最大值不能低于最小值！"), InterpretBase.textTran("警告"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    minValueX = value;
                }
            }
        }
        //自动区域显示
        private bool bAutoZone = true;
        //半峰高对齐区域显示
        private bool bEditZone = false;
        #region 波形放大操作相关
        private bool bEnlagerClick = false;//波形放大标志
        bool bDrawStart = false;
        Point pointStart = Point.Empty;
        Point pointContinue = Point.Empty;
        private int point_StartPos = 0;//起始点
        private int point_StopPos = 0;//终止点
        private int totalPointNum = 0;//所有点
        //定义屏幕显示的总点数
        private const int pointDisplay = 2000;
        // 定义滚动显示时的等分数
        private const int drawRangeDiv = 50;

        #endregion

        public delegate void waveView_Refresh(bool bnormal);//申明波形显示的委托
        public delegate void updateChanLabelFlowPanel();
        public delegate void toolStripWaveViewEnable(bool bState);
        public delegate void dataGridViewRefresh(ref DataTable dt);//申明数据绑定的委托
        public delegate void updateChannelDataGridView();//刷新表格
        public delegate void updateLabel(string st);//刷新label
        /// <summary>
        /// 显示用数据管理
        /// </summary>
        public class Series
        {
            public string strDispID;//显示标记
            public string strChanID;//通道标签ID
            private Color lineColor = Color.Red;//当前画笔颜色
            public double wfmFirstPeak;//波形第一个峰值
            public double wfmHalfPeakWidth;//波形的半峰宽宽度
            public bool bMeasure;//测量结果计算标志
            public float scaleMax;//scale上限
            public float scaleMin;//scale下限
            public Color m_lineColor
            {
                get
                {
                    return lineColor;
                }
                set
                {
                    lineColor = value;
                }
            }
            public List<PointF> dispData = new List<PointF>();//波形显示数据
        }

        /*界面左侧通道标签链表*/
        public List<UCChanWaveView> listChanWaveView = new List<UCChanWaveView>();
        /*定义显示用的数据*/
        private List<Series> listWaveSeriesShow = new List<Series>();
        /*定义数据管理类*/
        public Module_WaveMonitor modWavMonitor = new Module_WaveMonitor();

        private DataTable dtMeasInfo = new DataTable("Table_MeasInfoView");//显示测量结果表 
        private DataTable dtChanInfo = new DataTable("Table_ChanInfoView");//显示测量结果表 
        #endregion

        #region Construction

        public Form_WaveMonitor(string strTemp, string strName)
        {
            InitializeComponent();
            //toolStrip_WaveView.Enabled = false;
            //规定画布的大小
            boardWidth = this.pictureBox_Wave.Width;
            boardHeight = (float)(pictureBox_Wave.Height / autZonNormalScale * 1.0);
            horizontalMargin = 0;
            verticalMargin = 0;
            chartWidth = boardWidth - 2 * horizontalMargin;//画图区域宽度
            charHeight = boardHeight - 2 * verticalMargin; //画图区域高度,
            canvasWidth = chartWidth;//可以设置实际画布为画图区域的80%
            strFormNum = strTemp;
            this.Text = strName;
            // 两个pictureBox叠加，顶层透明
            pictureBox_DrawRect.Parent = pictureBox_Wave;
        }

        #endregion

        #region Events
        #region 自定义事件
        /*定义事件参数类*/
        public class formWaveEventArgs : EventArgs
        {
            public readonly string KeyToRaiseEvent;
            public formWaveEventArgs(string keyToRaiseEvent)
            {
                KeyToRaiseEvent = keyToRaiseEvent;
            }
        }
        //定义delegate委托申明
        public delegate void formWaveEventHandler(object sender, formWaveEventArgs e);
        //用event 关键字声明事件对象
        public event formWaveEventHandler formWaveEvent;
        //事件触发方法
        protected virtual void onFormWaveEvent(formWaveEventArgs e)
        {
            if (formWaveEvent != null)
                formWaveEvent(this, e);
        }
        //引发事件
        private void RaiseEvent(string keyToRaiseEvent)
        {
            formWaveEventArgs e = new formWaveEventArgs(keyToRaiseEvent);
            onFormWaveEvent(e);
        }

        #endregion
        #region 订阅自定义事件
        /// <summary>
        /// 订阅事件
        /// </summary>
        /// <param name="evenSource"></param>
        public void Subscribe(UCChanWaveView evenSource)
        {

            evenSource.chanWaveViewEvent += new UCChanWaveView.chanWaveViewEventHandler(chanWaveView_KeyPressed);

        }
        /// <summary>
        /// 取消订阅事件
        /// </summary>
        /// <param name="evenSource"></param>
        public void UnSubscribe(UCChanWaveView evenSource)
        {

            evenSource.chanWaveViewEvent -= new UCChanWaveView.chanWaveViewEventHandler(chanWaveView_KeyPressed);

        }
        /// <summary>
        /// 通道图标点击响应事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void chanWaveView_KeyPressed(object sender, UCChanWaveView.chanWaveViewEventArgs e)
        {
            string str = e.KeyToRaiseEvent;
            if (str.Contains("CheckBox"))
            {
                string[] arr = str.Split(':');
                string strID = arr[0];//获取通道编号
                if (str.Contains("False"))
                {
                    foreach (UCChanWaveView item in flowLayoutPanel_LabelItem.Controls)
                    {
                        if (item.m_strChanID == strID)
                        {
                            if (item.m_selectColor == ucChanLabel_Tag.m_labelColor)
                            {
                                ucChanLabel_Tag.Visible = false;
                            }
                        }
                    }
                }
                else
                {
                    if (bAutoZone)
                    {
                        foreach (UCChanWaveView item in flowLayoutPanel_LabelItem.Controls)
                        {
                            if (item.m_strChanID == strID)
                            {
                                item.label_ChanID_Set(true);
                            }
                        }
                    }
                }
                //this.pictureBox_Wave.Refresh();
                update_PictureBoxView_Screen();
            }
            else if (str.Contains("DoubleClick"))
            {
                //图标双击事件
                string[] arr = str.Split(':');
                string strID = arr[0];//获取通道编号
                int index = modWavMonitor.listOscDataProcess.FindIndex(item => (item.strChanID == strID));
                int indexShow = listWaveSeriesShow.FindIndex(item => (item.strChanID == strID));
                if ((index != -1) && (indexShow != -1))
                {
                    Form_ChanScaleSet formChanScaleSet = new Form_ChanScaleSet(strID,
                        bAutoZone,
                        modWavMonitor.listOscDataProcess[index].vertViewOffset,
                        listWaveSeriesShow[indexShow].scaleMax,
                        listWaveSeriesShow[indexShow].scaleMin);
                    formChanScaleSet.ShowDialog();
                    if (formChanScaleSet.m_bOk)
                    {
                        double offsetTemp = formChanScaleSet.m_offSet;
                        double scaleMaxTemp = formChanScaleSet.m_scaleMax;
                        double scaleMinTemp = formChanScaleSet.m_scaleMin;
                        if (bAutoZone)
                        {
                            if (formChanScaleSet.m_bCheckAll)
                            {
                                foreach (Series item in listWaveSeriesShow)
                                {
                                    item.scaleMax = (float)scaleMaxTemp;
                                    item.scaleMin = (float)scaleMinTemp;
                                }
                            }
                            else
                            {
                                listWaveSeriesShow[indexShow].scaleMax = (float)scaleMaxTemp;
                                listWaveSeriesShow[indexShow].scaleMin = (float)scaleMinTemp;
                            }

                        }
                        else
                        {

                            //更新垂直标签
                            int indexChanView = listChanWaveView.FindIndex(item => (item.m_strChanID == strID));
                            listChanWaveView[indexChanView].updateChanOffset(offsetTemp);
                            for (int i = 0; i < listWaveSeriesShow[indexShow].dispData.Count; i++)
                            {
                                listWaveSeriesShow[indexShow].dispData[i] = new System.Drawing.PointF(listWaveSeriesShow[indexShow].dispData[i].X,
                                    listWaveSeriesShow[indexShow].dispData[i].Y -
                                    (float)modWavMonitor.listOscDataProcess[index].vertViewOffset +
                                    (float)(offsetTemp));
                            }
                            modWavMonitor.listOscDataProcess[index].vertViewOffset = offsetTemp;
                            float offset = (float)modWavMonitor.listOscDataProcess[index].vertViewOffset;
                            float scale = maxValueY - minValueY;
                            float ampPrePoint = scale / panel_WaveLabel.Height;
                            int pos = (int)(offset / ampPrePoint);
                            ucChanLabel_Tag.Location = new Point(ucChanLabel_Tag.Location.X, panel_WaveLabel.Height / 2 - ucChanLabel_Tag.Height / 2 - pos);
                        }


                        update_PictureBoxView_Screen();
                    }
                }

            }
            else
            {
                //autozone模式下不响应label点击事件
                if (bAutoZone)
                {
                    return;
                }
                ucChanLabel_Tag.Visible = true;
                foreach (UCChanWaveView item in flowLayoutPanel_LabelItem.Controls)
                {
                    if (item.m_strChanID == str)
                    {
                        item.label_ChanID_Set(true);
                        panel_WaveLabel.Controls.Clear();
                        panel_WaveLabel.Controls.Add(ucChanLabel_Tag);
                        ucChanLabel_Tag.setLabel(str, item.m_selectColor);
                        int index = modWavMonitor.listOscDataProcess.FindIndex(ex => (ex.strChanID == item.m_strChanID));
                        if (index != -1)
                        {
                            float offset = (float)modWavMonitor.listOscDataProcess[index].vertViewOffset;
                            float scale = maxValueY - minValueY;
                            float ampPrePoint = scale / panel_WaveLabel.Height;
                            int pos = (int)(offset / ampPrePoint);
                            ucChanLabel_Tag.Location = new Point(ucChanLabel_Tag.Location.X, panel_WaveLabel.Height / 2 - ucChanLabel_Tag.Height / 2 - pos);
                        }
                    }
                    else
                    {
                        item.label_ChanID_Set(false);
                    }
                }
            }
        }
        #endregion
        /// <summary>
        /// load事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_WaveMonitor_Load(object sender, EventArgs e)
        {
            //toolStrip_WaveView.Enabled = false;

            //不显示幅度的上下限控制
            panel_Min.Visible = false;
            panel_Max.Visible = false;

            //显示左右移动按钮
            iconButton_Left.Visible = false;
            iconButton_Right.Visible = false;

            panel_InfoAndMeas.Visible = false;
            iconButton_DockRight.Visible = false;

            flowLayoutPanel_LabelItem.AutoScroll = false;
            flowLayoutPanel_LabelItem.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel_LabelItem.WrapContents = false;
            flowLayoutPanel_LabelItem.HorizontalScroll.Maximum = 0;
            flowLayoutPanel_LabelItem.AutoScrollMargin = new System.Drawing.Size(10, flowLayoutPanel_LabelItem.AutoScrollMargin.Height);
            flowLayoutPanel_LabelItem.AutoScroll = true;
            ///*业务逻辑模块初始化*/
            //bool bReturn = modWavMonitor.modWaveMonitor_Init();
            //if (bReturn != true)
            //{
            //    MessageBox.Show("初始化插值系数表失败！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //}
            ucChanLabel_Tag.Visible = false;

            label_InterMutiple.Text = "X1";

            dataGridView_ChanInfo.AllowUserToAddRows = false;//禁用最后一行空白
            dataGridView_ChanInfo.AllowUserToResizeRows = false;//禁拖动行高度
            dataGridView_ChanInfo.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;//禁止最左侧空白列宽度
            dataGridView_ChanInfo.AllowUserToDeleteRows = false;


            dtMeasInfo.Columns.Add(InterpretBase.textTran("通道标记"), typeof(String));
            dtMeasInfo.Columns.Add(InterpretBase.textTran("第一个峰值[v]"), typeof(String));
            dtMeasInfo.Columns.Add(InterpretBase.textTran("半峰宽[s]"), typeof(String));

            dtChanInfo.Columns.Add(InterpretBase.textTran("通道标记"), typeof(String));
            dtChanInfo.Columns.Add(InterpretBase.textTran("采样率"), typeof(String));
            dtChanInfo.Columns.Add(InterpretBase.textTran("点数"), typeof(String));
            dtChanInfo.Columns.Add(InterpretBase.textTran("垂直档位"), typeof(String));
            dtChanInfo.Columns.Add(InterpretBase.textTran("垂直偏移"), typeof(String));
            //dtChanInfo.Columns.Add("探头比", typeof(String));
            //dtChanInfo.Columns.Add("耦合", typeof(String));
            dtChanInfo.Columns.Add(InterpretBase.textTran("阻抗"), typeof(String));
            dtChanInfo.Columns.Add(InterpretBase.textTran("反向"), typeof(String));


            dataGridView_ChanInfo.ReadOnly = true;
            // 两个pictureBox叠加，顶层透明
            //pictureBox_DrawRect.Parent = pictureBox_Wave;
            //pictureBox_Wave.Controls.Add(pictureBox_DrawRect);

#if Debug
            //CSincInter temp = new CSincInter();
            //temp.CSincInter_LoadAll();
            ///*生成正弦测试数据*/
            //int[] sineResult = new int[100];
            //int[] inData = new int[1000];
            //for (int j = 0; j < 100; j++)
            //{
            //    sineResult[j] = (int)(Math.Sin(Math.PI * 2 / 100.0 * j) * 255);
            //}
            ///*初始化原始波形点*/
            //for (int i = 0; i < 1000; i++)
            //{
            //    inData[i] = sineResult[(11 + i * 25) % 100];
            //}
            ///*输出的点数= */
            //double[] outData = new double[10*1000];
            //outData = temp.sincConvolut_Cal(CSincInter.euInterMutipule.X10, ref inData);
            ///*csv文件操作-插值后波形保存*/
            //string path_OrigWave = System.IO.Directory.GetCurrentDirectory();
            //path_OrigWave += "\\" + "OrigWave.csv";
            //FileStream csvFs_Orignal = new FileStream(path_OrigWave, FileMode.Create);
            //StreamWriter csvSw = new StreamWriter(csvFs_Orignal, System.Text.Encoding.Default);
            //for (int i = 0; i < inData.Length; i++)
            //{
            //    csvSw.WriteLine(inData[i]);
            //}
            //csvSw.Flush();
            ////关闭流
            //csvSw.Close();
            //csvFs_Orignal.Close();

            //path_OrigWave = System.IO.Directory.GetCurrentDirectory();
            //path_OrigWave += "\\" + "WaveAfterInter.csv";
            //csvFs_Orignal = new FileStream(path_OrigWave, FileMode.Create);
            //csvSw = new StreamWriter(csvFs_Orignal, System.Text.Encoding.Default);
            //for (int i = 0; i < outData.Length; i++)
            //{
            //    csvSw.WriteLine(outData[i]);
            //}
            //csvSw.Flush();
            ////关闭流
            //csvSw.Close();
            //csvFs_Orignal.Close();
#endif
        }

        public void refreshLabel(string sampleRate)
        {
            if (this.InvokeRequired)
            {

                updateLabel s = new updateLabel(refreshLabel);
                this.pictureBox_Wave.Invoke(s, sampleRate);
            }
            else
            {
                if (string.IsNullOrEmpty(sampleRate))
                {
                    this.label_acqValue.Text = "2.5GSa/s";
                    return;
                }
                else
                {
                    this.label_acqValue.Text = CValue2String.acq2String(float.Parse(sampleRate));
                }
            }

        }
        /// <summary>
        /// 边框缩回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconButton_DockLeft_Click(object sender, EventArgs e)
        {
            /*左侧显示框宽度控制*/
            iconButton_DockLeft.Visible = false;
            iconButton_DockRight.Visible = true;
            panel_LabelItem.Width = 83;
            foreach (UCChanWaveView item in listChanWaveView)
            {
                item.setBriefViewMode();
            }
        }
        /// <summary>
        /// 边框弹出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconButton_DockRight_Click(object sender, EventArgs e)
        {
            /*左侧显示框宽度控制*/
            iconButton_DockRight.Visible = false;
            iconButton_DockLeft.Visible = true;
            panel_LabelItem.Width = 165;
            foreach (UCChanWaveView item in listChanWaveView)
            {
                item.setDetialViewMode();
            }
        }
        /// <summary>
        /// 显示信息表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconToolStripButton_Table_Click(object sender, EventArgs e)
        {
            if (panel_InfoAndMeas.Visible)
            {
                panel_InfoAndMeas.Visible = false;
            }
            else
            {
                panel_InfoAndMeas.Visible = true;
                // this.tabControl_Info.TabPages.Clear();
                // this.tabControl_Info.TabPages.Remove(tabControl_Info.TabPages[1]);
                dtChanInfo.Rows.Clear();
                // 显示数据表相关内容
                for (int i = 0; i < listChanWaveView.Count; i++)
                {
                    DataRow dataRowChanInfo = dtChanInfo.NewRow();
                    dataRowChanInfo[InterpretBase.textTran("通道标记")] = listChanWaveView[i].m_strChanID;
                    int index = modWavMonitor.listOscDataProcess.FindIndex(item => (item.strChanID == listChanWaveView[i].m_strChanID));
                    if (index != -1)
                    {
                        dataRowChanInfo[InterpretBase.textTran("采样率")] = CValue2String.acq2String((float)modWavMonitor.listOscDataProcess[index].sampRate);
                        dataRowChanInfo[InterpretBase.textTran("点数")] = CValue2String.meDepth2String((float)modWavMonitor.listOscDataProcess[index].memDepth);
                        dataRowChanInfo[InterpretBase.textTran("垂直档位")] = CValue2String.scal2String(modWavMonitor.listOscDataProcess[index].vertScale);
                        dataRowChanInfo[InterpretBase.textTran("垂直偏移")] = CValue2String.voltage2String(modWavMonitor.listOscDataProcess[index].vertOffset);
                        //dataRowChanInfo["探头比"] = CValue2String.scal2String(modWavMonitor.listOscDataProcess[index].);
                        //dataRowChanInfo["耦合"] = CValue2String.voltage2String(modWavMonitor.listOscDataProcess[index].vertOffset);
                        if (modWavMonitor.listOscDataProcess[index].bChanImpedence)
                        {
                            dataRowChanInfo[InterpretBase.textTran("阻抗")] = "1M";
                        }
                        else
                        {
                            dataRowChanInfo[InterpretBase.textTran("阻抗")] = "50Ω";
                        }
                        if (modWavMonitor.listOscDataProcess[index].bChanInv)
                        {
                            dataRowChanInfo[InterpretBase.textTran("反向")] = "true";
                        }
                        else
                        {
                            dataRowChanInfo[InterpretBase.textTran("反向")] = "false";
                        }
                    }
                    dtChanInfo.Rows.Add(dataRowChanInfo);
                }
                dataSourceBindingChanInfo(ref dtChanInfo);
            }

        }
        /// <summary>
        /// 绘图区域的主动绘制事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox_Wave_PaintWave()
        {
            try
            {
                if ((pictureBox_Wave.Width <= 0) || (pictureBox_Wave.Height <= 0))
                {
                    return;
                }
                // 先生成一个新的位图
                Bitmap bmp = new Bitmap(pictureBox_Wave.Width, pictureBox_Wave.Height);
                Graphics g = Graphics.FromImage(bmp);
                g.Clear(System.Drawing.Color.Black);
                if ((toolStrip_WaveView.Enabled))
                {
                    //autozone显示需要区别开
                    if (bAutoZone)
                    {
                        //重新规定画布的大小
                        //boardWidth = this.pictureBox_Wave.Width;
                        if (bNormalScale)
                        {
                            boardHeight = (pictureBox_Wave.Height / (float)autZonNormalScale);
                        }
                        else if (bLittleScale)
                        {
                            boardHeight = (pictureBox_Wave.Height / (float)autZonLittleScale);
                        }
                        else if (bLargeScale)
                        {
                            boardHeight = (pictureBox_Wave.Height / (float)autZonLargeScale);
                        }
                        /*先把所有波形的垂直偏移回到初始位置*/
                        foreach (UCChanWaveView item in listChanWaveView)
                        {
                            int index_Origl = modWavMonitor.listOscDataProcess.FindIndex(ex => (ex.strChanID == item.m_strChanID));
                            if (index_Origl != -1)
                            {
                                int index_Show = listWaveSeriesShow.FindIndex(ex_show => (ex_show.strChanID == item.m_strChanID));
                                if (index_Show != -1)
                                {
                                    /*更新显示的偏移值*/
                                    item.updateChanOffset(modWavMonitor.listOscDataProcess[index_Origl].vertOffset);
                                    for (int i = 0; i < listWaveSeriesShow[index_Show].dispData.Count; i++)
                                    {
                                        listWaveSeriesShow[index_Show].dispData[i] = new System.Drawing.PointF(listWaveSeriesShow[index_Show].dispData[i].X,
                                            (float)(listWaveSeriesShow[index_Show].dispData[i].Y - modWavMonitor.listOscDataProcess[index_Origl].vertViewOffset + modWavMonitor.listOscDataProcess[index_Origl].vertOffset));
                                    }
                                    //更新偏移
                                    modWavMonitor.listOscDataProcess[index_Origl].vertViewOffset = modWavMonitor.listOscDataProcess[index_Origl].vertOffset;
                                }
                            }
                        }
                        //重新根据需要绘制的波形数重新排列所有需要显示的波形
                        int wavNum = listChanWaveView.Count;
                        //boardWidth = (float)(pictureBox_Wave.Width / 50.0 * (autZonRange + 1));
                        boardWidth = (float)(pictureBox_Wave.Width / (drawRangeDiv * 1.0) * (autZonRange + 1));
                        horizontalMargin = 0;
                        verticalMargin = 0;
                        chartWidth = boardWidth - 2 * horizontalMargin;//画图区域宽度
                        charHeight = boardHeight - 2 * verticalMargin; //画图区域高度,
                        canvasWidth = chartWidth;//可以设置实际画布为画图区域的80%
                        if (bNormalScale)
                        {
                            for (int i = curPosition * autZonNormalScale; i < wavNum; i++)
                            {
                                if (i - curPosition * autZonNormalScale < autZonNormalScale)
                                {
                                    startPostion = new PointF(horizontalMargin, verticalMargin + boardHeight * (i - curPosition * autZonNormalScale));
                                    endPostion = new PointF(boardWidth - horizontalMargin, boardHeight * (i - curPosition * autZonNormalScale + 1) - verticalMargin);
                                    int index = listWaveSeriesShow.FindIndex(item => (item.strChanID == listChanWaveView[i].m_strChanID));
                                    if (index != -1)
                                    {
                                        Drawing(ref g, listWaveSeriesShow[index], (pointDisplay / drawRangeDiv) * (autZonRange + 1), true);
                                    }
                                    else
                                    {
                                        Series dataTemp = new Series();
                                        dataTemp.strDispID = "";
                                        dataTemp.strChanID = listChanWaveView[i].m_strChanID;
                                        dataTemp.m_lineColor = listChanWaveView[i].m_selectColor;
                                        dataTemp.scaleMax = (float)listChanWaveView[i].m_vertScalMax;
                                        dataTemp.scaleMin = (float)listChanWaveView[i].m_vertScalMin;
                                        Drawing(ref g, dataTemp, false);
                                    }

                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        else if (bLittleScale)
                        {
                            for (int i = curPosition * autZonLittleScale; i < wavNum; i++)
                            {
                                if (i - curPosition * autZonLittleScale < autZonLittleScale)
                                {
                                    startPostion = new PointF(horizontalMargin, verticalMargin + boardHeight * (i - curPosition * autZonLittleScale));
                                    endPostion = new PointF(boardWidth - horizontalMargin, boardHeight * (i - curPosition * autZonLittleScale + 1) - verticalMargin);
                                    //Drawing(e.Graphics, listWaveSeriesShow[i]);
                                    int index = listWaveSeriesShow.FindIndex(item => (item.strChanID == listChanWaveView[i].m_strChanID));
                                    if (index != -1)
                                    {
                                        Drawing(ref g, listWaveSeriesShow[index], (pointDisplay / drawRangeDiv) * (autZonRange + 1), true);
                                    }
                                    else
                                    {
                                        Series dataTemp = new Series();
                                        dataTemp.strDispID = "";
                                        dataTemp.strChanID = listChanWaveView[i].m_strChanID;
                                        dataTemp.m_lineColor = listChanWaveView[i].m_selectColor;
                                        dataTemp.scaleMax = (float)listChanWaveView[i].m_vertScalMax;
                                        dataTemp.scaleMin = (float)listChanWaveView[i].m_vertScalMin;
                                        Drawing(ref g, dataTemp, false);
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        else if (bLargeScale)
                        {
                            for (int i = curPosition * autZonLargeScale; i < wavNum; i++)
                            {
                                if (i - curPosition * autZonLargeScale < autZonLargeScale)
                                {
                                    startPostion = new PointF(horizontalMargin, verticalMargin + boardHeight * (i - curPosition * autZonLargeScale));
                                    endPostion = new PointF(boardWidth - horizontalMargin, boardHeight * (i - curPosition * autZonLargeScale + 1) - verticalMargin);
                                    //Drawing(e.Graphics, listWaveSeriesShow[i]);
                                    int index = listWaveSeriesShow.FindIndex(item => (item.strChanID == listChanWaveView[i].m_strChanID));
                                    if (index != -1)
                                    {
                                        Drawing(ref g, listWaveSeriesShow[index], (pointDisplay / drawRangeDiv) * (autZonRange + 1), true);
                                    }
                                    else
                                    {
                                        Series dataTemp = new Series();
                                        dataTemp.strDispID = "";
                                        dataTemp.strChanID = listChanWaveView[i].m_strChanID;
                                        dataTemp.m_lineColor = listChanWaveView[i].m_selectColor;
                                        dataTemp.scaleMax = (float)listChanWaveView[i].m_vertScalMax;
                                        dataTemp.scaleMin = (float)listChanWaveView[i].m_vertScalMin;
                                        Drawing(ref g, dataTemp, false);
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        pictureBox_Wave.Image = bmp;
                    }
                    else//非autozone模式
                    {
                        //string startTime = DateTime.Now.ToString("ss fff");
                        //System.Diagnostics.Debug.WriteLine("开始时间：" + startTime);
                        Drawing(listWaveSeriesShow);
                        //string stopTime = DateTime.Now.ToString("ss fff");
                        //System.Diagnostics.Debug.WriteLine("结束时间：" + stopTime);
                    }
                }
                g.Dispose();
            }
            catch (Exception e)
            {

            }

        }
        /// <summary>
        /// 波形显示窗体大小变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox_Wave_SizeChanged(object sender, EventArgs e)
        {
            if (bAutoZone)
            {
                //重新规定画布的大小
                boardWidth = this.pictureBox_Wave.Width;
                if (bNormalScale)
                {
                    boardHeight = (pictureBox_Wave.Height / (float)autZonNormalScale);
                }
                else if (bLittleScale)
                {
                    boardHeight = (pictureBox_Wave.Height / (float)autZonLittleScale);
                }
                else if (bLargeScale)
                {
                    boardHeight = (pictureBox_Wave.Height / (float)autZonLargeScale);
                }
                horizontalMargin = 0;
                verticalMargin = 0;
                chartWidth = boardWidth - 2 * horizontalMargin;//画图区域宽度
                charHeight = boardHeight - 2 * verticalMargin; //画图区域高度,
                canvasWidth = chartWidth;//可以设置实际画布为画图区域的80%
            }
            else
            {
                InitCanvas();
                foreach (UCChanWaveView item in listChanWaveView)
                {
                    int index = modWavMonitor.listOscDataProcess.FindIndex(ex => (ex.strChanID == item.m_strChanID));
                    if (index != -1)
                    {
                        //item.updateChanOffset(modWavMonitor.listOscDataProcess[index].vertViewOffset);
                        if (item.m_bSelect)
                        {
                            float offset = (float)modWavMonitor.listOscDataProcess[index].vertViewOffset;
                            float scale = maxValueY - minValueY;
                            float ampPrePoint = scale / panel_WaveLabel.Height;
                            int pos = (int)(offset / ampPrePoint);
                            ucChanLabel_Tag.Location = new Point(ucChanLabel_Tag.Location.X, panel_WaveLabel.Height / 2 - ucChanLabel_Tag.Height / 2 - pos);
                        }

                    }
                }
            }

            if (bAutoZone)
            {
                //先清除panel中已有的标签
                panel_WaveLabel.Controls.Clear();
                //重新根据需要绘制的波形数重新排列所有需要显示的波形
                for (int i = 0; i < listChanWaveView.Count; i++)
                {
                    /*新建一个标签用于标示当前通道*/
                    UCChanLabel ucChanLabTemp = new UCChanLabel();
                    ucChanLabTemp.setLabel(listChanWaveView[i].m_strChanID, listChanWaveView[i].m_selectColor);
                    panel_WaveLabel.Controls.Add(ucChanLabTemp);
                    //ucChanLabTemp.Location = new Point(ucChanLabel_Tag.Location.X, panel_WaveLabel.Height / (listChanWaveView.Count * 2) - ucChanLabTemp.Height / 2 + i * (panel_WaveLabel.Height / listChanWaveView.Count));
                    if (bNormalScale)
                    {
                        ucChanLabTemp.Location = new Point(ucChanLabel_Tag.Location.X,
                    (int)(panel_WaveLabel.Height / (autZonNormalScale * 2.0) - ucChanLabTemp.Height / 2.0 + i * (panel_WaveLabel.Height / (autZonNormalScale * 1.0))));
                    }
                    else if (bLittleScale)
                    {
                        ucChanLabTemp.Location = new Point(ucChanLabel_Tag.Location.X,
                    (int)(panel_WaveLabel.Height / (autZonLittleScale * 2.0) - ucChanLabTemp.Height / 2.0 + i * (panel_WaveLabel.Height / (autZonLittleScale * 1.0))));
                    }
                    else if (bLargeScale)
                    {
                        ucChanLabTemp.Location = new Point(ucChanLabel_Tag.Location.X,
                    (int)(panel_WaveLabel.Height / (autZonLargeScale * 2.0) - ucChanLabTemp.Height / 2.0 + i * (panel_WaveLabel.Height / (autZonLargeScale * 1.0))));
                    }

                }
            }
            //pictureBox_Wave.Refresh();
            update_PictureBoxView_Screen();
        }

        /// <summary>
        /// 用户控件鼠标按下事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ucChanLabel_Tag_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isChanLabelMouseDown = true;
                axChanLabel = e.X;
                ayChanLabel = e.Y;
            }
        }
        /// <summary>
        /// 用户控件鼠标抬起事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ucChanLabel_Tag_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isChanLabelMouseDown = false;
                /*更新垂直偏移*/
                foreach (UCChanWaveView item in listChanWaveView)
                {
                    if (item.m_bSelect)
                    {
                        int index_Origl = modWavMonitor.listOscDataProcess.FindIndex(ex => (ex.strChanID == item.m_strChanID));
                        float scale = maxValueY - minValueY;
                        float ampPrePoint = scale / panel_WaveLabel.Height;
                        int pos = (int)(modWavMonitor.listOscDataProcess[index_Origl].vertViewOffset / ampPrePoint);
                        float offsetAmp = ampPrePoint * (ucChanLabel_Tag.Location.Y - (panel_WaveLabel.Height / 2 - ucChanLabel_Tag.Height / 2 - pos));
                        int index_Show = listWaveSeriesShow.FindIndex(ex_show => (ex_show.strChanID == item.m_strChanID));
                        if (index_Show != -1)
                        {
                            /*更新显示的偏移值*/
                            modWavMonitor.listOscDataProcess[index_Origl].vertViewOffset = modWavMonitor.listOscDataProcess[index_Origl].vertViewOffset - offsetAmp;
                            item.updateChanOffset(modWavMonitor.listOscDataProcess[index_Origl].vertViewOffset);
                            for (int i = 0; i < listWaveSeriesShow[index_Show].dispData.Count; i++)
                            {
                                listWaveSeriesShow[index_Show].dispData[i] = new System.Drawing.PointF(listWaveSeriesShow[index_Show].dispData[i].X,
                                    listWaveSeriesShow[index_Show].dispData[i].Y - offsetAmp);
                            }
                        }

                    }
                }
                //this.pictureBox_Wave.Refresh();
                update_PictureBoxView_Screen();
            }
        }
        /// <summary>
        /// 用户控件鼠标移动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ucChanLabel_Tag_MouseMove(object sender, MouseEventArgs e)
        {
            if (isChanLabelMouseDown)
            {
                ucChanLabel_Tag.Location = new Point(ucChanLabel_Tag.Location.X, ucChanLabel_Tag.Location.Y + (e.Y - ayChanLabel));
                if (ucChanLabel_Tag.Location.Y <= 0)
                {
                    ucChanLabel_Tag.Location = new Point(ucChanLabel_Tag.Location.X, 0);
                }
                if (ucChanLabel_Tag.Location.Y >= this.panel_WaveLabel.Height - ucChanLabel_Tag.Height)
                {
                    ucChanLabel_Tag.Location = new Point(ucChanLabel_Tag.Location.X, this.panel_WaveLabel.Height - ucChanLabel_Tag.Height);
                }
            }
        }

        /// <summary>
        /// 加载历史数据事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconToolStripButton_Load_Click(object sender, EventArgs e)
        {
            Form_ChooseWaveData form_ChooseWaveData = new Form_ChooseWaveData();
            form_ChooseWaveData.ShowDialog();
            if (string.IsNullOrEmpty(form_ChooseWaveData.strHistroyTime))
            {
                return;
            }
            string sql = "SELECT DeviceGUID,ChannelGUID,Data FROM " + Module_DeviceManage.Instance.WaveTableName + " Where Convert(varchar, StartTime,120) = '{0}'";
            sql = string.Format(sql, form_ChooseWaveData.strHistroyTime);
            DataTable dt = new DataTable();
            dt = DbHelperSql.QueryDt(sql);
            foreach (DataRow dr in dt.Rows)
            {
                string guid = dr["DeviceGUID"].ToString();
                string channelGuid = dr["ChannelGUID"].ToString();
                Module_DeviceManage.Instance.GetDeviceByGUID(guid).GetChannelByGUID(channelGuid).SetData((byte[])dr["Data"], 0, ((byte[])dr["Data"]).Length);
            }
            // 发送自定义事件
            RaiseEvent(strFormNum);

#if Debug
            setOscPara("172.18.8.252");
            setOscPara("172.18.8.253");

            listChanWaveView.Clear();
            Thread receive = new Thread(new ThreadStart(ReceiveNews));
            receive.IsBackground = true;
            receive.Start();
            Task task = new Task(() =>
            {
                while (ChatSession.bComplete != 2)
                {
                    Thread.Sleep(500);
                }
                modWavMonitor.devDelayMin = ChatSession.findMinDevDelay();
                /*加载接收到的数据*/
                foreach (ChatSession.OscilloscopeDataMemory item in ChatSession.m_listOscDataMemory)
                {
                    modWavMonitor.modWaveMonitor_Load(item);
                }

                foreach (Module_WaveMonitor.OscilloscopeDataProcess item in modWavMonitor.listOscDataProcess)
                {
                    Color ctemp = Color.Red;
                    if (item.strChanID == "Tag001")
                    {
                        ctemp = Color.Red;
                    }
                    else if (item.strChanID == "Tag002")
                    {
                        ctemp = Color.Yellow;
                    }
                    else if (item.strChanID == "Tag003")
                    {
                        ctemp = Color.Blue;
                    }
                    else if (item.strChanID == "Tag004")
                    {
                        ctemp = Color.Green;
                    }
                    else if (item.strChanID == "Tag005")
                    {
                        ctemp = Color.DarkRed;
                    }
                    else if (item.strChanID == "Tag006")
                    {
                        ctemp = Color.YellowGreen;
                    }
                    else if (item.strChanID == "Tag007")
                    {
                        ctemp = Color.DarkBlue;
                    }
                    else if (item.strChanID == "Tag008")
                    {
                        ctemp = Color.LightGreen;
                    }
                    UCChanWaveView uctemp = new UCChanWaveView(ctemp,
                        item.strChanID,
                        CValue2String.scal2String(item.vertScale),
                        CValue2String.voltage2String(item.vertOffset),
                        item.bChanInv,
                        item.bChanBWLimit,
                        item.bChanImpedence);
                    uctemp.label_ChanID_Set(true);
                    listChanWaveView.Add(uctemp);
                    //订阅自定义事件
                    Subscribe(uctemp);
                }
                update_PictureBoxView_Data();
            }
            );
            task.Start();
#endif
        }
        /// <summary>
        /// 测量按键响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton_Meas_Click(object sender, EventArgs e)
        {
            dtMeasInfo.Rows.Clear();
            foreach (UCChanWaveView item in listChanWaveView)
            {
                //计算半峰宽
                int index = listWaveSeriesShow.FindIndex(ex => (ex.strChanID == item.m_strChanID));
                if (listWaveSeriesShow[index].bMeasure == false)
                {
                    double widthTemp = modWavMonitor.getHafPeakWidthResult(item.m_strChanID);
                    double firstFeak = modWavMonitor.getFirstPeakValue(item.m_strChanID);
                    listWaveSeriesShow[index].wfmFirstPeak = firstFeak;
                    listWaveSeriesShow[index].wfmHalfPeakWidth = widthTemp;
                    listWaveSeriesShow[index].bMeasure = true;
                }
                dtMeasInfo.Rows.Add(item.m_strChanID, CValue2String.voltage2String(listWaveSeriesShow[index].wfmFirstPeak), CValue2String.time2String(listWaveSeriesShow[index].wfmHalfPeakWidth));
            }
        }
        /// <summary>
        /// 放大按键响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconToolStripButton_Enlager_Click(object sender, EventArgs e)
        {
            if (bEnlagerClick == false)
            {
                bEnlagerClick = true;
                iconToolStripButton_Enlager.IconColor = Color.Red;
                this.pictureBox_Wave.Cursor = Cursors.Hand;
            }
            else
            {
                bEnlagerClick = false;
                iconToolStripButton_Enlager.IconColor = Color.White;
                this.pictureBox_Wave.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// 全区域显示模式-所有波形归一化，在统一的垂直范围下进行显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton_FullZone_Click(object sender, EventArgs e)
        {
            // 图标不显示
            iconButton_Up.Visible = false;
            iconButton_Down.Visible = false;
            //显示左右移动按钮
            iconButton_Left.Visible = true;
            iconButton_Right.Visible = true;
            //显示幅度的上下限控制
            panel_Min.Visible = true;
            panel_Max.Visible = true;
            //xy轴坐标轴显示
            bYMarking = true;
            bXMarking = true;
            //显示x和y的轴线
            bDrawGradLineX = true;
            bDrawGradLineY = true;
            InitCanvas();
            bAutoZone = false;
            bEditZone = false;
            panel_WaveLabel.Controls.Clear();
            panel_WaveLabel.Controls.Add(ucChanLabel_Tag);
            ucChanLabel_Tag.Visible = false;
            foreach (UCChanWaveView item in listChanWaveView)
            {
                item.label_ChanID_Set(false);
            }
            //this.pictureBox_Wave.Refresh();
            update_PictureBoxView_Screen();
        }
        /// <summary>
        /// 自动区域显示模式-所有波形自动排列，默认是普通图标显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton_AutoZone_ButtonClick(object sender, EventArgs e)
        {
            bAutoZone = true;
            bEditZone = false;

            bLittleScale = false;
            bNormalScale = true;
            bLargeScale = false;
            //隐藏左右移动按钮
            iconButton_Left.Visible = false;
            iconButton_Right.Visible = false;
            //先不显示上下移动的图标
            iconButton_Up.Visible = false;
            iconButton_Down.Visible = false;
            //不显示幅度的上下限控制
            panel_Min.Visible = false;
            panel_Max.Visible = false;
            //xy轴坐标轴不显示
            bYMarking = true;
            bXMarking = false;
            //显示x和y的轴线
            bDrawGradLineX = true;
            bDrawGradLineY = true;

            /*原有的通道标签不显示*/
            ucChanLabel_Tag.Visible = false;
            //获取显示波形的个数
            int wavNum = listChanWaveView.Count;
            if (wavNum > autZonNormalScale)
            {
                iconButton_Down.Visible = true;
            }
            //位置信息清零
            curPosition = 0;
            //先清除panel中已有的标签
            panel_WaveLabel.Controls.Clear();
            //重新根据需要绘制的波形数重新排列所有需要显示的波形
            for (int i = 0; i < autZonNormalScale; i++)
            //for (int i = 0; i < listChanWaveView.Count; i++)
            {
                if (i >= wavNum)
                {
                    break;
                }
                //所有的标签都是选中状态
                listChanWaveView[i].label_ChanID_Set(true);
                /*新建一个标签用于标示当前通道*/
                UCChanLabel ucChanLabTemp = new UCChanLabel();
                ucChanLabTemp.setLabel(listChanWaveView[i].m_strChanID, listChanWaveView[i].m_selectColor);
                panel_WaveLabel.Controls.Add(ucChanLabTemp);
                //ucChanLabTemp.Location = new Point(ucChanLabel_Tag.Location.X, 
                //panel_WaveLabel.Height / (listChanWaveView.Count * 2) - ucChanLabTemp.Height / 2 + i * (panel_WaveLabel.Height / listChanWaveView.Count));
                ucChanLabTemp.Location = new Point(ucChanLabel_Tag.Location.X,
                    (int)(panel_WaveLabel.Height / (autZonNormalScale * 2.0) - ucChanLabTemp.Height / 2.0 + i * (panel_WaveLabel.Height / (autZonNormalScale * 1.0))));
            }
            update_PictureBoxView_Screen();
        }
        /// <summary>
        /// 设置各个模式的显示范围
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconMenuItem_SettingViewScale_Click(object sender, EventArgs e)
        {
            Form_SettingWaveMonitorViewScale form_SetViewScale = new Form_SettingWaveMonitorViewScale();
            form_SetViewScale.RefreshTextBox(autZonLittleScale, autZonNormalScale, autZonLargeScale);
            form_SetViewScale.ShowDialog();
            if (form_SetViewScale.bOkClick != true)
            {
                form_SetViewScale.Dispose();
                return;
            }
            autZonLittleScale = form_SetViewScale.m_littleNum;
            autZonNormalScale = form_SetViewScale.m_normalNum;
            autZonLargeScale = form_SetViewScale.m_largeNum;
        }
        /// <summary>
        /// 小图标显示模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconMenuItem_Little_Click(object sender, EventArgs e)
        {
            bLittleScale = true;
            bNormalScale = false;
            bLargeScale = false;
            //先不显示上下移动的图标
            iconButton_Up.Visible = false;
            iconButton_Down.Visible = false;
            //显示左右移动按钮
            iconButton_Left.Visible = false;
            iconButton_Right.Visible = false;
            //不显示幅度的上下限控制
            panel_Min.Visible = false;
            panel_Max.Visible = false;
            //xy轴坐标轴不显示
            bYMarking = false;
            bXMarking = false;
            //显示x和y的轴线
            bDrawGradLineX = false;
            bDrawGradLineY = false;

            /*原有的通道标签不显示*/
            ucChanLabel_Tag.Visible = false;
            //获取显示波形的个数
            int wavNum = listChanWaveView.Count;
            if (wavNum > autZonLittleScale)
            {
                iconButton_Down.Visible = true;
            }
            //位置信息清零
            curPosition = 0;
            //先清除panel中已有的标签
            panel_WaveLabel.Controls.Clear();
            //重新根据需要绘制的波形数重新排列所有需要显示的波形
            for (int i = 0; i < autZonLittleScale; i++)
            //for (int i = 0; i < listChanWaveView.Count; i++)
            {
                if (i >= wavNum)
                {
                    break;
                }
                //所有的标签都是选中状态
                listChanWaveView[i].label_ChanID_Set(true);
                /*新建一个标签用于标示当前通道*/
                UCChanLabel ucChanLabTemp = new UCChanLabel();
                ucChanLabTemp.setLabel(listChanWaveView[i].m_strChanID, listChanWaveView[i].m_selectColor);
                panel_WaveLabel.Controls.Add(ucChanLabTemp);
                //ucChanLabTemp.Location = new Point(ucChanLabel_Tag.Location.X, 
                //panel_WaveLabel.Height / (listChanWaveView.Count * 2) - ucChanLabTemp.Height / 2 + i * (panel_WaveLabel.Height / listChanWaveView.Count));
                ucChanLabTemp.Location = new Point(ucChanLabel_Tag.Location.X,
                    (int)(panel_WaveLabel.Height / (autZonLittleScale * 2.0) - ucChanLabTemp.Height / 2.0 + i * (panel_WaveLabel.Height / (autZonLittleScale * 1.0))));
            }
            bAutoZone = true;
            bEditZone = false;
            update_PictureBoxView_Screen();
        }
        /// <summary>
        /// 正常图标显示模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconMenuItem_Normal_Click(object sender, EventArgs e)
        {
            bLittleScale = false;
            bNormalScale = true;
            bLargeScale = false;
            //先不显示上下移动的图标
            iconButton_Up.Visible = false;
            iconButton_Down.Visible = false;
            //显示左右移动按钮
            iconButton_Left.Visible = false;
            iconButton_Right.Visible = false;
            //不显示幅度的上下限控制
            panel_Min.Visible = false;
            panel_Max.Visible = false;
            //xy轴坐标轴不显示
            bYMarking = true;
            bXMarking = false;
            //显示x和y的轴线
            bDrawGradLineX = true;
            bDrawGradLineY = true;

            /*原有的通道标签不显示*/
            ucChanLabel_Tag.Visible = false;
            //获取显示波形的个数
            int wavNum = listChanWaveView.Count;
            if (wavNum > autZonNormalScale)
            {
                iconButton_Down.Visible = true;
            }
            //位置信息清零
            curPosition = 0;
            //先清除panel中已有的标签
            panel_WaveLabel.Controls.Clear();
            //重新根据需要绘制的波形数重新排列所有需要显示的波形
            for (int i = 0; i < autZonNormalScale; i++)
            //for (int i = 0; i < listChanWaveView.Count; i++)
            {
                if (i >= wavNum)
                {
                    break;
                }
                //所有的标签都是选中状态
                listChanWaveView[i].label_ChanID_Set(true);
                /*新建一个标签用于标示当前通道*/
                UCChanLabel ucChanLabTemp = new UCChanLabel();
                ucChanLabTemp.setLabel(listChanWaveView[i].m_strChanID, listChanWaveView[i].m_selectColor);
                panel_WaveLabel.Controls.Add(ucChanLabTemp);
                //ucChanLabTemp.Location = new Point(ucChanLabel_Tag.Location.X, 
                //panel_WaveLabel.Height / (listChanWaveView.Count * 2) - ucChanLabTemp.Height / 2 + i * (panel_WaveLabel.Height / listChanWaveView.Count));
                ucChanLabTemp.Location = new Point(ucChanLabel_Tag.Location.X,
                    (int)(panel_WaveLabel.Height / (autZonNormalScale * 2.0) - ucChanLabTemp.Height / 2.0 + i * (panel_WaveLabel.Height / (autZonNormalScale * 1.0))));
            }
            bAutoZone = true;
            bEditZone = false;
            update_PictureBoxView_Screen();
        }
        /// <summary>
        /// 大图标显示模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconMenuItem_Big_Click(object sender, EventArgs e)
        {
            bLittleScale = false;
            bNormalScale = false;
            bLargeScale = true;
            //先不显示上下移动的图标
            iconButton_Up.Visible = false;
            iconButton_Down.Visible = false;
            //显示左右移动按钮
            iconButton_Left.Visible = false;
            iconButton_Right.Visible = false;
            //不显示幅度的上下限控制
            panel_Min.Visible = false;
            panel_Max.Visible = false;
            //xy轴坐标轴不显示
            bYMarking = true;
            bXMarking = false;
            //显示x和y的轴线
            bDrawGradLineX = true;
            bDrawGradLineY = true;

            /*原有的通道标签不显示*/
            ucChanLabel_Tag.Visible = false;
            //获取显示波形的个数
            int wavNum = listChanWaveView.Count;
            if (wavNum > autZonLargeScale)
            {
                iconButton_Down.Visible = true;
            }
            //位置信息清零
            curPosition = 0;
            //先清除panel中已有的标签
            panel_WaveLabel.Controls.Clear();
            //重新根据需要绘制的波形数重新排列所有需要显示的波形
            for (int i = 0; i < autZonLargeScale; i++)
            //for (int i = 0; i < listChanWaveView.Count; i++)
            {
                if (i >= wavNum)
                {
                    break;
                }
                //所有的标签都是选中状态
                listChanWaveView[i].label_ChanID_Set(true);
                /*新建一个标签用于标示当前通道*/
                UCChanLabel ucChanLabTemp = new UCChanLabel();
                ucChanLabTemp.setLabel(listChanWaveView[i].m_strChanID, listChanWaveView[i].m_selectColor);
                panel_WaveLabel.Controls.Add(ucChanLabTemp);
                //ucChanLabTemp.Location = new Point(ucChanLabel_Tag.Location.X, 
                //panel_WaveLabel.Height / (listChanWaveView.Count * 2) - ucChanLabTemp.Height / 2 + i * (panel_WaveLabel.Height / listChanWaveView.Count));
                ucChanLabTemp.Location = new Point(ucChanLabel_Tag.Location.X,
                    (int)(panel_WaveLabel.Height / (autZonLargeScale * 2.0) - ucChanLabTemp.Height / 2.0 + i * (panel_WaveLabel.Height / (autZonLargeScale * 1.0))));
            }
            bAutoZone = true;
            bEditZone = false;
            update_PictureBoxView_Screen();
        }
        /// <summary>
        /// 向上移动操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconButton_Up_Click(object sender, EventArgs e)
        {
            int num = listChanWaveView.Count;
            curPosition--;
            if (curPosition == 0)
            {
                iconButton_Down.Visible = true;
                iconButton_Up.Visible = false;
            }
            else
            {
                iconButton_Down.Visible = true;
                iconButton_Up.Visible = true;
            }
            //先清除panel中已有的标签
            panel_WaveLabel.Controls.Clear();
            if (bNormalScale)
            {
                //重新根据需要绘制的波形数重新排列所有需要显示的波形
                for (int i = curPosition * autZonNormalScale; i < listChanWaveView.Count; i++)
                {
                    if (i - curPosition * autZonNormalScale >= autZonNormalScale)
                    {
                        break;
                    }
                    /*新建一个标签用于标示当前通道*/
                    UCChanLabel ucChanLabTemp = new UCChanLabel();
                    ucChanLabTemp.setLabel(listChanWaveView[i].m_strChanID, listChanWaveView[i].m_selectColor);
                    panel_WaveLabel.Controls.Add(ucChanLabTemp);
                    ucChanLabTemp.Location = new Point(ucChanLabel_Tag.Location.X,
                        (int)(panel_WaveLabel.Height / (autZonNormalScale * 2.0) - ucChanLabTemp.Height / 2.0 + (i - curPosition * autZonNormalScale) * (panel_WaveLabel.Height / (autZonNormalScale * 1.0))));
                }
            }
            else if (bLittleScale)
            {
                //重新根据需要绘制的波形数重新排列所有需要显示的波形
                for (int i = curPosition * autZonLittleScale; i < listChanWaveView.Count; i++)
                {
                    if (i - curPosition * autZonLittleScale >= autZonLittleScale)
                    {
                        break;
                    }
                    /*新建一个标签用于标示当前通道*/
                    UCChanLabel ucChanLabTemp = new UCChanLabel();
                    ucChanLabTemp.setLabel(listChanWaveView[i].m_strChanID, listChanWaveView[i].m_selectColor);
                    panel_WaveLabel.Controls.Add(ucChanLabTemp);
                    ucChanLabTemp.Location = new Point(ucChanLabel_Tag.Location.X,
                        (int)(panel_WaveLabel.Height / (autZonLittleScale * 2.0) - ucChanLabTemp.Height / 2.0 + (i - curPosition * autZonLittleScale) * (panel_WaveLabel.Height / (autZonLittleScale * 1.0))));
                }
            }
            else if (bLargeScale)
            {
                //重新根据需要绘制的波形数重新排列所有需要显示的波形
                for (int i = curPosition * autZonLargeScale; i < listChanWaveView.Count; i++)
                {
                    if (i - curPosition * autZonLargeScale >= autZonLargeScale)
                    {
                        break;
                    }
                    /*新建一个标签用于标示当前通道*/
                    UCChanLabel ucChanLabTemp = new UCChanLabel();
                    ucChanLabTemp.setLabel(listChanWaveView[i].m_strChanID, listChanWaveView[i].m_selectColor);
                    panel_WaveLabel.Controls.Add(ucChanLabTemp);
                    ucChanLabTemp.Location = new Point(ucChanLabel_Tag.Location.X,
                        (int)(panel_WaveLabel.Height / (autZonLargeScale * 2.0) - ucChanLabTemp.Height / 2.0 + (i - curPosition * autZonLargeScale) * (panel_WaveLabel.Height / (autZonLargeScale * 1.0))));
                }
            }
            update_PictureBoxView_Screen();
        }
        /// <summary>
        /// 向下移动操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconButton_Down_Click(object sender, EventArgs e)
        {
            int num = listChanWaveView.Count;
            curPosition++;
            if (bNormalScale)
            {
                int levelTotal = (int)Math.Ceiling((double)(num / (autZonNormalScale * 1.0)));
                if (curPosition == levelTotal - 1)
                {
                    iconButton_Down.Visible = false;
                    iconButton_Up.Visible = true;
                }
                else
                {
                    iconButton_Down.Visible = true;
                    iconButton_Up.Visible = true;
                }
                //先清除panel中已有的标签
                panel_WaveLabel.Controls.Clear();
                //重新根据需要绘制的波形数重新排列所有需要显示的波形
                for (int i = curPosition * autZonNormalScale; i < listChanWaveView.Count; i++)
                {
                    if (i - curPosition * autZonNormalScale >= autZonNormalScale)
                    {
                        break;
                    }
                    /*新建一个标签用于标示当前通道*/
                    UCChanLabel ucChanLabTemp = new UCChanLabel();
                    ucChanLabTemp.setLabel(listChanWaveView[i].m_strChanID, listChanWaveView[i].m_selectColor);
                    panel_WaveLabel.Controls.Add(ucChanLabTemp);
                    ucChanLabTemp.Location = new Point(ucChanLabel_Tag.Location.X,
                        (int)(panel_WaveLabel.Height / (autZonNormalScale * 2.0) - ucChanLabTemp.Height / 2.0 + (i - curPosition * autZonNormalScale) * (panel_WaveLabel.Height / (autZonNormalScale * 1.0))));
                }
            }
            else if (bLittleScale)
            {
                int levelTotal = (int)Math.Ceiling((double)(num / (autZonLittleScale * 1.0)));
                if (curPosition == levelTotal - 1)
                {
                    iconButton_Down.Visible = false;
                    iconButton_Up.Visible = true;
                }
                else
                {
                    iconButton_Down.Visible = true;
                    iconButton_Up.Visible = true;
                }
                //先清除panel中已有的标签
                panel_WaveLabel.Controls.Clear();
                //重新根据需要绘制的波形数重新排列所有需要显示的波形
                for (int i = curPosition * autZonLittleScale; i < listChanWaveView.Count; i++)
                {
                    if (i - curPosition * autZonLittleScale >= autZonLittleScale)
                    {
                        break;
                    }
                    /*新建一个标签用于标示当前通道*/
                    UCChanLabel ucChanLabTemp = new UCChanLabel();
                    ucChanLabTemp.setLabel(listChanWaveView[i].m_strChanID, listChanWaveView[i].m_selectColor);
                    panel_WaveLabel.Controls.Add(ucChanLabTemp);
                    ucChanLabTemp.Location = new Point(ucChanLabel_Tag.Location.X,
                        (int)(panel_WaveLabel.Height / (autZonLittleScale * 2.0) - ucChanLabTemp.Height / 2.0 + (i - curPosition * autZonLittleScale) * (panel_WaveLabel.Height / (autZonLittleScale * 1.0))));
                }
            }
            else if (bLargeScale)
            {
                int levelTotal = (int)Math.Ceiling((double)(num / (autZonLargeScale * 1.0)));
                if (curPosition == levelTotal - 1)
                {
                    iconButton_Down.Visible = false;
                    iconButton_Up.Visible = true;
                }
                else
                {
                    iconButton_Down.Visible = true;
                    iconButton_Up.Visible = true;
                }
                //先清除panel中已有的标签
                panel_WaveLabel.Controls.Clear();
                //重新根据需要绘制的波形数重新排列所有需要显示的波形
                for (int i = curPosition * autZonLargeScale; i < listChanWaveView.Count; i++)
                {
                    if (i - curPosition * autZonLargeScale >= autZonLargeScale)
                    {
                        break;
                    }
                    /*新建一个标签用于标示当前通道*/
                    UCChanLabel ucChanLabTemp = new UCChanLabel();
                    ucChanLabTemp.setLabel(listChanWaveView[i].m_strChanID, listChanWaveView[i].m_selectColor);
                    panel_WaveLabel.Controls.Add(ucChanLabTemp);
                    ucChanLabTemp.Location = new Point(ucChanLabel_Tag.Location.X,
                        (int)(panel_WaveLabel.Height / (autZonLargeScale * 2.0) - ucChanLabTemp.Height / 2.0 + (i - curPosition * autZonLargeScale) * (panel_WaveLabel.Height / (autZonLargeScale * 1.0))));
                }
            }
            update_PictureBoxView_Screen();
        }
        /// <summary>
        /// 编辑对齐区域显示模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton_EditZone_Click(object sender, EventArgs e)
        {
            //显示幅度的上下限控制
            panel_Min.Visible = true;
            panel_Max.Visible = true;
            //y轴坐标轴显示
            bYMarking = true;
            bXMarking = true;
            //显示x和y的轴线
            bDrawGradLineX = true;
            bDrawGradLineY = true;
            InitCanvas();
            bAutoZone = false;
            bEditZone = true;
            panel_WaveLabel.Controls.Clear();
            panel_WaveLabel.Controls.Add(ucChanLabel_Tag);
            ucChanLabel_Tag.Visible = false;
            foreach (UCChanWaveView item in listChanWaveView)
            {
                item.label_ChanID_Set(false);
                int index = listWaveSeriesShow.FindIndex(ex => (ex.strChanID == item.m_strChanID));
                if (listWaveSeriesShow[index].bMeasure == false)//如果没有计算先计算峰高
                {
                    double widthTemp = modWavMonitor.getHafPeakWidthResult(item.m_strChanID);
                    double firstFeak = modWavMonitor.getFirstPeakValue(item.m_strChanID);
                    listWaveSeriesShow[index].wfmFirstPeak = firstFeak;
                    listWaveSeriesShow[index].wfmHalfPeakWidth = widthTemp;
                    listWaveSeriesShow[index].bMeasure = true;
                }
                //半峰高对齐到0显示
                int indexOrig = modWavMonitor.listOscDataProcess.FindIndex(ex => (ex.strChanID == item.m_strChanID));
                double offset = listWaveSeriesShow[index].wfmFirstPeak / 2.0 - modWavMonitor.listOscDataProcess[indexOrig].wfmFirstTopValue;

                item.updateChanOffset(offset);
                for (int i = 0; i < listWaveSeriesShow[index].dispData.Count; i++)
                {
                    listWaveSeriesShow[index].dispData[i] = new System.Drawing.PointF(listWaveSeriesShow[index].dispData[i].X,
                        (float)(listWaveSeriesShow[index].dispData[i].Y - modWavMonitor.listOscDataProcess[indexOrig].vertViewOffset + offset));
                }
                modWavMonitor.listOscDataProcess[indexOrig].vertViewOffset = offset;
            }

            //this.pictureBox_Wave.Refresh();
            update_PictureBoxView_Screen();
        }
        /// <summary>
        /// 自动编号，与数据无关
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_ChanInfo_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
               e.RowBounds.Location.Y,
               dataGridView_ChanInfo.RowHeadersWidth - 4,
               e.RowBounds.Height);
            TextRenderer.DrawText(e.Graphics,
                  (e.RowIndex + 1).ToString(),
                   dataGridView_ChanInfo.RowHeadersDefaultCellStyle.Font,
                   rectangle,
                   dataGridView_ChanInfo.RowHeadersDefaultCellStyle.ForeColor,
                   TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }
        /// <summary>
        /// 自动编号，与数据无关
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_MeasInfo_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            //Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
            //    e.RowBounds.Location.Y,
            //    dataGridView_MeasInfo.RowHeadersWidth - 4,
            //    e.RowBounds.Height);
            //TextRenderer.DrawText(e.Graphics,
            //      (e.RowIndex + 1).ToString(),
            //       dataGridView_MeasInfo.RowHeadersDefaultCellStyle.Font,
            //       rectangle,
            //       dataGridView_MeasInfo.RowHeadersDefaultCellStyle.ForeColor,
            //       TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }
        /// <summary>
        /// 当前选中波形垂直偏移返回初始位置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconSplitButton_Initial_ButtonClick(object sender, EventArgs e)
        {
            /*更新垂直偏移*/
            foreach (UCChanWaveView item in listChanWaveView)
            {
                if (item.m_bSelect)
                {
                    int index_Origl = modWavMonitor.listOscDataProcess.FindIndex(ex => (ex.strChanID == item.m_strChanID));
                    float scale = maxValueY - minValueY;
                    //恢复初始偏移

                    float ampPrePoint = scale / panel_WaveLabel.Height;
                    int pos = (int)(modWavMonitor.listOscDataProcess[index_Origl].vertOffset / ampPrePoint);
                    ucChanLabel_Tag.Location = new Point(ucChanLabel_Tag.Location.X, panel_WaveLabel.Height / 2 - ucChanLabel_Tag.Height / 2 - pos);
                    int index_Show = listWaveSeriesShow.FindIndex(ex_show => (ex_show.strChanID == item.m_strChanID));
                    /*更新显示的偏移值*/
                    item.updateChanOffset(modWavMonitor.listOscDataProcess[index_Origl].vertOffset);
                    for (int i = 0; i < listWaveSeriesShow[index_Show].dispData.Count; i++)
                    {
                        listWaveSeriesShow[index_Show].dispData[i] = new System.Drawing.PointF(listWaveSeriesShow[index_Show].dispData[i].X,
                            (float)(listWaveSeriesShow[index_Show].dispData[i].Y - modWavMonitor.listOscDataProcess[index_Origl].vertViewOffset + modWavMonitor.listOscDataProcess[index_Origl].vertOffset));
                    }
                    //更新偏移
                    modWavMonitor.listOscDataProcess[index_Origl].vertViewOffset = modWavMonitor.listOscDataProcess[index_Origl].vertOffset;
                }
            }
            //this.pictureBox_Wave.Refresh();
            update_PictureBoxView_Screen();
        }
        /// <summary>
        /// 当前选中波形垂直偏移返回初始位置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconMenuItem_Single_Click(object sender, EventArgs e)
        {
            /*更新垂直偏移*/
            foreach (UCChanWaveView item in listChanWaveView)
            {
                if (item.m_bSelect)
                {
                    int index_Origl = modWavMonitor.listOscDataProcess.FindIndex(ex => (ex.strChanID == item.m_strChanID));
                    float scale = maxValueY - minValueY;

                    float ampPrePoint = scale / panel_WaveLabel.Height;
                    int pos = (int)(modWavMonitor.listOscDataProcess[index_Origl].vertOffset / ampPrePoint);
                    ucChanLabel_Tag.Location = new Point(ucChanLabel_Tag.Location.X, panel_WaveLabel.Height / 2 - ucChanLabel_Tag.Height / 2 - pos);
                    int index_Show = listWaveSeriesShow.FindIndex(ex_show => (ex_show.strChanID == item.m_strChanID));
                    /*更新显示的偏移值*/
                    item.updateChanOffset(modWavMonitor.listOscDataProcess[index_Origl].vertOffset);
                    for (int i = 0; i < listWaveSeriesShow[index_Show].dispData.Count; i++)
                    {
                        listWaveSeriesShow[index_Show].dispData[i] = new System.Drawing.PointF(listWaveSeriesShow[index_Show].dispData[i].X,
                            (float)(listWaveSeriesShow[index_Show].dispData[i].Y - modWavMonitor.listOscDataProcess[index_Origl].vertViewOffset + modWavMonitor.listOscDataProcess[index_Origl].vertOffset));
                    }
                    //更新偏移
                    modWavMonitor.listOscDataProcess[index_Origl].vertViewOffset = modWavMonitor.listOscDataProcess[index_Origl].vertOffset;
                }
            }
            //this.pictureBox_Wave.Refresh();
            update_PictureBoxView_Screen();
        }
        /// <summary>
        /// 所有波形垂直偏移返回初始位置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconMenuItem_All_Click(object sender, EventArgs e)
        {
            /*更新垂直偏移*/
            foreach (UCChanWaveView item in listChanWaveView)
            {
                int index_Origl = modWavMonitor.listOscDataProcess.FindIndex(ex => (ex.strChanID == item.m_strChanID));

                if (item.m_bSelect)
                {
                    float scale = maxValueY - minValueY;
                    float ampPrePoint = scale / panel_WaveLabel.Height;
                    int pos = (int)(modWavMonitor.listOscDataProcess[index_Origl].vertOffset / ampPrePoint);
                    ucChanLabel_Tag.Location = new Point(ucChanLabel_Tag.Location.X, panel_WaveLabel.Height / 2 - ucChanLabel_Tag.Height / 2 - pos);
                }
                int index_Show = listWaveSeriesShow.FindIndex(ex_show => (ex_show.strChanID == item.m_strChanID));
                /*更新显示的偏移值*/
                item.updateChanOffset(modWavMonitor.listOscDataProcess[index_Origl].vertOffset);
                for (int i = 0; i < listWaveSeriesShow[index_Show].dispData.Count; i++)
                {
                    listWaveSeriesShow[index_Show].dispData[i] = new System.Drawing.PointF(listWaveSeriesShow[index_Show].dispData[i].X,
                        (float)(listWaveSeriesShow[index_Show].dispData[i].Y - modWavMonitor.listOscDataProcess[index_Origl].vertViewOffset + modWavMonitor.listOscDataProcess[index_Origl].vertOffset));
                }
                //更新偏移
                modWavMonitor.listOscDataProcess[index_Origl].vertViewOffset = modWavMonitor.listOscDataProcess[index_Origl].vertOffset;
            }
            //this.pictureBox_Wave.Refresh();
            update_PictureBoxView_Screen();
        }
        /// <summary>
        /// 设置显示的最小幅度值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconButton_Min_Click(object sender, EventArgs e)
        {
            double scale_Min = 0;
            string strScaleMin = textBox_Min.Text;
            try
            {
                scale_Min = Convert.ToDouble(strScaleMin);
                if (scale_Min > maxValueY)
                {
                    MessageBox.Show(InterpretBase.textTran("幅度范围的最小值必须<最大值！"));
                    return;
                }
                else
                {
                    minValueY = (float)scale_Min;
                    //this.pictureBox_Wave.Refresh();
                    update_PictureBoxView_Screen();
                }
            }
            catch
            {
                MessageBox.Show(InterpretBase.textTran("最小值输入格式有误！"));
            }
        }
        /// <summary>
        /// 设置显示的最大幅度值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconButton_Max_Click(object sender, EventArgs e)
        {
            double scale_Max = 0;
            string strScaleMax = textBox_Max.Text;
            try
            {
                scale_Max = Convert.ToDouble(strScaleMax);
                if (scale_Max < minValueY)
                {
                    MessageBox.Show(InterpretBase.textTran("幅度范围的最大值必须>最小值！"));
                    return;
                }
                else
                {
                    maxValueY = (float)scale_Max;
                    //this.pictureBox_Wave.Refresh();
                    update_PictureBoxView_Screen();
                }
            }
            catch
            {
                MessageBox.Show(InterpretBase.textTran("最大值输入格式有误！"));
            }
        }
        /// <summary>
        /// textbox输入限制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox_Min_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < 48 || e.KeyChar > 57) && (e.KeyChar != 8) && (e.KeyChar != 46) && (e.KeyChar != 45))
                e.Handled = true;
        }
        /// <summary>
        /// textbox输入限制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox_Max_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < 48 || e.KeyChar > 57) && (e.KeyChar != 8) && (e.KeyChar != 46) && (e.KeyChar != 45))
                e.Handled = true;
        }
        /// <summary>
        /// 波形左移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconButton_Left_Click(object sender, EventArgs e)
        {
            if ((point_StartPos == 0) && (point_StopPos == 0))
            {
                return;
            }
            if (bEnlagerClick)
            {
                int pointsPerMov = (point_StopPos - point_StartPos + 1) / 10;
                if (point_StopPos + pointsPerMov > 1000000)
                {
                    return;
                }
                point_StartPos = point_StartPos + pointsPerMov;
                point_StopPos = point_StopPos + pointsPerMov;

                int pointNum = point_StopPos - point_StartPos + 1;
                int mutipleFin;
                if (pointNum < pointDisplay)
                {
                    mutipleFin = modWavMonitor.findFinMutiple(pointDisplay, pointNum);
                    if (mutipleFin > 200)
                    {
                        //限制在200倍内插
                        mutipleFin = 200;
                    }
                    //显示当前的内插倍数
                    label_InterMutiple.Text = "X" + mutipleFin.ToString();
                }
                else
                {
                    mutipleFin = 1;
                    //显示当前的内插倍数
                    label_InterMutiple.Text = "X" + mutipleFin.ToString();
                }
                //波形放大插值操作
                bool bResult = modWavMonitor.calEnlargeWaveData_UsingCDLL(point_StartPos, point_StopPos, mutipleFin);
                //如果插值的话，需要重新计算结束位置
                if (mutipleFin != 1)
                {
                    int numRange = (int)Math.Ceiling(pointDisplay / (mutipleFin * 1.0));
                    point_StopPos = point_StartPos + numRange - 1;
                }
                //计算x轴的范围
                minValueX = modWavMonitor.listOscDataProcess[0].xOrigin + (point_StartPos) * modWavMonitor.listOscDataProcess[0].xIncrement;
                maxValueX = minValueX + (point_StopPos - point_StartPos + 1) * modWavMonitor.listOscDataProcess[0].xIncrement;
                foreach (Module_WaveMonitor.OscilloscopeDataProcess item in modWavMonitor.listOscDataProcess)
                {
                    int index = listWaveSeriesShow.FindIndex(member => (member.strChanID == item.strChanID));
                    if (index != -1)
                    {
                        if (bResult)
                        {
                            listWaveSeriesShow[index].dispData.Clear();
                            for (int i = 0; i < item.dataDispFin.Length; i++)
                            {
                                PointF temPoint = new PointF();
                                temPoint.X = 0;
                                /*加上显示时的偏移*/
                                //temPoint.Y = (float)(item.dataDispFin[i] - item.vertOffset + (item.vertViewOffset - item.vertOffset));
                                temPoint.Y = (float)(item.dataDispFin[i] + (item.vertViewOffset - item.vertOffset));
                                listWaveSeriesShow[index].dispData.Add(temPoint);
                            }
                        }
                    }

                }
                update_PictureBoxView_Screen();
            }
        }
        /// <summary>
        /// 波形右移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconButton_Right_Click(object sender, EventArgs e)
        {
            if ((point_StartPos == 0) && (point_StopPos == 0))
            {
                return;
            }
            if (bEnlagerClick)
            {
                int pointsPerMov = (point_StopPos - point_StartPos + 1) / 10;
                if (point_StartPos - pointsPerMov <= 0)
                {
                    return;
                }
                point_StartPos = point_StartPos - pointsPerMov;
                point_StopPos = point_StopPos - pointsPerMov;

                int pointNum = point_StopPos - point_StartPos + 1;
                int mutipleFin;
                if (pointNum < pointDisplay)
                {
                    mutipleFin = modWavMonitor.findFinMutiple(pointDisplay, pointNum);
                    if (mutipleFin > 200)
                    {
                        //限制在200倍内插
                        mutipleFin = 200;
                    }
                    //显示当前的内插倍数
                    label_InterMutiple.Text = "X" + mutipleFin.ToString();
                }
                else
                {
                    mutipleFin = 1;
                    //显示当前的内插倍数
                    label_InterMutiple.Text = "X" + mutipleFin.ToString();
                }
                //波形放大插值操作
                bool bResult = modWavMonitor.calEnlargeWaveData_UsingCDLL(point_StartPos, point_StopPos, mutipleFin);
                //如果插值的话，需要重新计算结束位置
                if (mutipleFin != 1)
                {
                    int numRange = (int)Math.Ceiling(pointDisplay / (mutipleFin * 1.0));
                    point_StopPos = point_StartPos + numRange - 1;
                }
                //计算x轴的范围
                minValueX = modWavMonitor.listOscDataProcess[0].xOrigin + (point_StartPos) * modWavMonitor.listOscDataProcess[0].xIncrement;
                maxValueX = minValueX + (point_StopPos - point_StartPos + 1) * modWavMonitor.listOscDataProcess[0].xIncrement;
                foreach (Module_WaveMonitor.OscilloscopeDataProcess item in modWavMonitor.listOscDataProcess)
                {
                    int index = listWaveSeriesShow.FindIndex(member => (member.strChanID == item.strChanID));
                    if (index != -1)
                    {
                        if (bResult)
                        {
                            listWaveSeriesShow[index].dispData.Clear();
                            for (int i = 0; i < item.dataDispFin.Length; i++)
                            {
                                PointF temPoint = new PointF();
                                temPoint.X = 0;
                                /*加上显示时的偏移*/
                                //temPoint.Y = (float)(item.dataDispFin[i] - item.vertOffset + (item.vertViewOffset - item.vertOffset));
                                temPoint.Y = (float)(item.dataDispFin[i] + (item.vertViewOffset - item.vertOffset));
                                listWaveSeriesShow[index].dispData.Add(temPoint);
                            }
                        }
                    }

                }
                update_PictureBoxView_Screen();
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// 初始化实际的画布尺寸信息
        /// </summary>
        private void InitCanvas()
        {
            boardWidth = this.pictureBox_Wave.Width;
            boardHeight = this.pictureBox_Wave.Height;
            horizontalMargin = 0;
            verticalMargin = 0;
            chartWidth = boardWidth - 2 * horizontalMargin;//画图区域宽度
            charHeight = boardHeight - 2 * verticalMargin; //画图区域高度,
            canvasWidth = chartWidth;//可以设置实际画布为画图区域的80%
            startPostion = new PointF(horizontalMargin, verticalMargin);
            endPostion = new PointF(boardWidth - horizontalMargin, boardHeight - verticalMargin);
        }

        /// <summary>
        /// 波形绘制处理
        /// </summary>
        /// <param name="g"></param>
        /// <param name="listSeriesData"></param>
        private void Drawing(List<Series> listSeriesData)
        {
            Bitmap finalImage = new Bitmap(pictureBox_Wave.Width, pictureBox_Wave.Height);

            using (Graphics g = Graphics.FromImage(finalImage))
            {
                g.SmoothingMode = SmoothingMode.HighQuality;  //图片柔顺模式选择
                Pen p = new Pen(Color.Gray, 1);//定义了一个灰色,宽度为1的画笔
                p.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid; //实线
                Rectangle rect = new Rectangle(pictureBox_Wave.ClientRectangle.X, pictureBox_Wave.ClientRectangle.Y,
                                                 pictureBox_Wave.ClientRectangle.X + pictureBox_Wave.ClientRectangle.Width,
                                                 pictureBox_Wave.ClientRectangle.Y + pictureBox_Wave.ClientRectangle.Height);
                g.DrawRectangle(p, rect);//绘制矩形框

                p.DashStyle = System.Drawing.Drawing2D.DashStyle.Custom; //虚线
                p.DashPattern = new float[] { 2, 2 };
                //计算垂直范围
                float vertScale = maxValueY - minValueY;
                float vertPerGitter = vertScale / 8.0f;
                StringFormat strFmt = new System.Drawing.StringFormat();
                strFmt.Alignment = StringAlignment.Center; //文本水平居中
                strFmt.LineAlignment = StringAlignment.Center; //文本垂直居中
                Font axisFont = new Font("微软雅黑", 9, FontStyle.Bold);
                //先绘制顶部位置的纵坐标标示
                string strVertScale = CValue2String.voltage2String(maxValueY);
                SizeF sf = g.MeasureString(strVertScale, axisFont);
                RectangleF rf = new RectangleF(0, 0, sf.Width, sf.Height);
                //绘制Y轴
                if (bYMarking)
                {
                    g.DrawString(strVertScale, axisFont, new SolidBrush(Color.White), rf, strFmt);
                }
                //再绘制底部位置的纵坐标标示
                strVertScale = CValue2String.voltage2String(minValueY);
                sf = g.MeasureString(strVertScale, axisFont);
                rf = new RectangleF(0, this.pictureBox_Wave.Height - sf.Height, sf.Width, sf.Height);
                if (bYMarking)
                {
                    g.DrawString(strVertScale, axisFont, new SolidBrush(Color.White), rf, strFmt);
                }
                //绘制x轴刻度线
                for (int i = 1; i <= 7; i++)
                {
                    float div = (float)pictureBox_Wave.ClientRectangle.Height / (7 + 1);
                    strVertScale = CValue2String.voltage2String(maxValueY - vertPerGitter * i);
                    sf = g.MeasureString(strVertScale, axisFont);
                    rf = new RectangleF(0, div * i - sf.Height / 2, sf.Width, sf.Height);
                    //绘制纵坐标标示
                    if (bYMarking)
                    {
                        g.DrawString(strVertScale, axisFont, new SolidBrush(Color.White), rf, strFmt);
                    }
                    if (i == 4)
                    {
                        p.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid; //实线
                        if (bDrawGradLineY)
                        {
                            g.DrawLine(p, 0, div * i, pictureBox_Wave.ClientRectangle.Width, div * i);
                        }
                        p.DashStyle = System.Drawing.Drawing2D.DashStyle.Custom; //虚线
                        p.DashPattern = new float[] { 2, 2 };
                    }
                    else
                    {
                        if (bDrawGradLineY)
                        {
                            g.DrawLine(p, 0, div * i, pictureBox_Wave.ClientRectangle.Width, div * i);
                        }
                    }
                }
                //绘制最右侧水平坐标标示
                string strHorizScale = CValue2String.time2String(maxValueX);
                sf = g.MeasureString(strHorizScale, axisFont);
                rf = new RectangleF(this.pictureBox_Wave.Width - sf.Width, 0, sf.Width, sf.Height);
                //绘制最右侧水平坐标标示
                if (bXMarking)
                {
                    g.DrawString(strHorizScale, axisFont, new SolidBrush(Color.Orange), rf, strFmt);
                }
                //绘制y轴刻度线条
                for (int i = 1; i <= 9; i++)
                {
                    float div = (float)pictureBox_Wave.ClientRectangle.Width / (9 + 1);
                    double timePerGitter = (maxValueX - minValueX) / 10.0;
                    strHorizScale = CValue2String.time2String(minValueX + timePerGitter * i);
                    sf = g.MeasureString(strHorizScale, axisFont);
                    rf = new RectangleF(div * i - sf.Width / 2, 0, sf.Width, sf.Height);
                    /*绘制其余的水平坐标标示*/
                    if (bXMarking)
                    {
                        g.DrawString(strHorizScale, axisFont, new SolidBrush(Color.Orange), rf, strFmt);
                    }
                    if (i == 5)
                    {
                        p.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid; //实线
                        if (bDrawGradLineX)
                        {
                            g.DrawLine(p, div * i, 0, div * i, pictureBox_Wave.ClientRectangle.Height);
                        }
                        p.DashStyle = System.Drawing.Drawing2D.DashStyle.Custom; //虚线
                        p.DashPattern = new float[] { 2, 2 };
                    }
                    else
                    {
                        if (bDrawGradLineX)
                        {
                            g.DrawLine(p, div * i, 0, div * i, pictureBox_Wave.ClientRectangle.Height);
                        }
                    }
                }
                // 使用多个图层实现,定义图层链表
                List<Bitmap> listImages = new List<Bitmap>();
                for (int i = 0; i < listSeriesData.Count; i++)
                {
                    Bitmap image = new Bitmap(pictureBox_Wave.Width, pictureBox_Wave.Height);
                    listImages.Add(image);
                }
                //并发的绘制多个图层
                Parallel.For(0, listSeriesData.Count,
                    item =>
                    {
                        Graphics graphics = Graphics.FromImage(listImages[item]);
                        int indexChan = listChanWaveView.FindIndex(chanItem => (chanItem.m_strChanID == listSeriesData[item].strChanID));
                        if (indexChan != -1)
                        {
                            if (listChanWaveView[indexChan].m_bCheck)//选中的才显示
                            {
                                xScaleCount = listSeriesData[item].dispData.Count;

                                horizontalBetween = canvasWidth / xScaleCount;
                                verticalBetween = charHeight / yScaleCount;
                                intervalValueY = (float)(maxValueY - minValueY) / yScaleCount;

                                //计算0值的坐标
                                //int tempv = (int)((Math.Abs(minValueY) - 0) / intervalValueY);//得到0到最小值的间隔距离；
                                int tempv = (int)((0- minValueY) / intervalValueY);
                                float zeroY = endPostion.Y - tempv * verticalBetween;//值为0点Y抽坐标；
                                if (listSeriesData[item].dispData.Count > 1)
                                {
                                    int dataIndex = 0;
                                    PointF[] arrDataPoint = new PointF[listSeriesData[item].dispData.Count];
                                    int index = 0;
                                    foreach (PointF pf in listSeriesData[item].dispData)
                                    {
                                        PointF temp = new PointF();
                                        temp.X = startPostion.X + horizontalBetween * index;
                                        temp.Y = zeroY - verticalBetween * pf.Y / intervalValueY;
                                        arrDataPoint[dataIndex++] = temp;
                                        index++;
                                    }
                                    /*重新构造波形点，用于显示,两点之间不能直接连线，需要横线连接*/
                                    //List<PointF> listPonitf = new List<PointF>();

                                    //for (int i = 0; i < arrDataPoint.Length; i++)
                                    //{
                                    //    if (i != arrDataPoint.Length - 1)
                                    //    {
                                    //        PointF temp1 = arrDataPoint[i];
                                    //        PointF temp2 = arrDataPoint[i + 1];
                                    //        listPonitf.Add(temp1);
                                    //        if (temp1.Y != temp2.Y)
                                    //        {
                                    //            PointF temp = new PointF();
                                    //            temp.X = temp2.X;
                                    //            temp.Y = temp1.Y;
                                    //            listPonitf.Add(temp);
                                    //        }
                                    //    }
                                    //    else
                                    //    {
                                    //        listPonitf.Add(arrDataPoint[i]);
                                    //    }
                                    //}
                                    //PointF[] arrDataPoint_New = new PointF[listPonitf.Count];
                                    //for (int i = 0; i < listPonitf.Count; i++)
                                    //{
                                    //    arrDataPoint_New[i] = listPonitf[i];
                                    //}
                                    //graphics.DrawLines(new Pen(new SolidBrush(listSeriesData[item].m_lineColor), 1F), arrDataPoint_New);
                                    graphics.DrawLines(new Pen(new SolidBrush(listSeriesData[item].m_lineColor), 1F), arrDataPoint);
                                    //if (listSeriesData[item].strChanID == "Tag001")
                                    //{
                                    //    listImages[item].Save("E:\\WaveData\\test\\current001" + ".bmp", ImageFormat.Bmp);
                                    //}

                                }
                            }
                        }
                        graphics.Dispose();
                    });
                //清零计数
                foreach (Bitmap item in listImages)
                {
                    //item.MakeTransparent();
                    //item.Save("E:\\WaveData\\test\\current" + ".bmp", ImageFormat.Bmp);
                    g.DrawImage(item, new Rectangle(0, 0, pictureBox_Wave.Width, pictureBox_Wave.Height));
                }
                g.Dispose();
                listImages.Clear();
            }
            pictureBox_Wave.Image = finalImage;
        }
        /// <summary>
        /// 绘制指定的单条波形,用于autozone显示
        /// </summary>
        /// <param name="g"></param>
        /// <param name="listSeriesData"></param>
        private void Drawing(ref Graphics g, Series seriesData, bool bDraw = true)
        {
            g.SmoothingMode = SmoothingMode.HighQuality;  //图片柔顺模式选择
            //g.InterpolationMode = InterpolationMode.HighQualityBicubic;//高质量
            //g.CompositingQuality = CompositingQuality.HighQuality;//再加一点
            Pen p = new Pen(Color.Gray, 1);//定义了一个灰色,宽度为1的画笔
            p.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid; //实线
            Rectangle rect = new Rectangle(pictureBox_Wave.ClientRectangle.X, pictureBox_Wave.ClientRectangle.Y,
                                             pictureBox_Wave.ClientRectangle.X + pictureBox_Wave.ClientRectangle.Width,
                                             pictureBox_Wave.ClientRectangle.Y + pictureBox_Wave.ClientRectangle.Height);
            g.DrawRectangle(p, rect);//绘制图形框的矩形外框

            p.DashStyle = System.Drawing.Drawing2D.DashStyle.Custom; //虚线
            p.DashPattern = new float[] { 2, 2 };
            //显示的轴线个数，奇数个轴线
            int xAxisNum = 3;
            if (bLargeScale)
            {
                xAxisNum = 5;
            }
            if (xAxisNum % 2 == 0)
            {
                xAxisNum = xAxisNum + 1;
            }
            //计算垂直范围
            float vertScale = seriesData.scaleMax - seriesData.scaleMin;
            float vertPerGitter = (float)vertScale / (xAxisNum + 1);
            StringFormat strFmt = new System.Drawing.StringFormat();
            strFmt.Alignment = StringAlignment.Center; //文本水平居中
            strFmt.LineAlignment = StringAlignment.Center; //文本垂直居中
            Font axisFont = new Font("微软雅黑", 6, FontStyle.Bold);
            //先绘制顶部位置的纵坐标标示
            string strVertScale = CValue2String.voltage2String(seriesData.scaleMax);
            SizeF sf = g.MeasureString(strVertScale, axisFont);
            RectangleF rf = new RectangleF(0, 0 + startPostion.Y, sf.Width, sf.Height);
            //绘制Y轴
            if (bYMarking)
            {
                g.DrawString(strVertScale, axisFont, new SolidBrush(seriesData.m_lineColor), rf, strFmt);
            }
            //再绘制底部位置的纵坐标标示

            strVertScale = CValue2String.voltage2String(seriesData.scaleMin);
            sf = g.MeasureString(strVertScale, axisFont);

            //规定单条绘制区域的高度
            float areaHeight = endPostion.Y - startPostion.Y;
            rf = new RectangleF(0, startPostion.Y + areaHeight - sf.Height, sf.Width, sf.Height);
            if (bYMarking)
            {
                g.DrawString(strVertScale, axisFont, new SolidBrush(seriesData.m_lineColor), rf, strFmt);
            }

            //绘制x方向刻度线
            for (int i = 1; i <= xAxisNum; i++)
            {
                float div = (float)areaHeight / (xAxisNum + 1);
                strVertScale = CValue2String.voltage2String(seriesData.scaleMax - vertPerGitter * i);
                sf = g.MeasureString(strVertScale, axisFont);
                rf = new RectangleF(0, startPostion.Y + div * i - sf.Height / 2, sf.Width, sf.Height);
                //绘制纵坐标标示
                if (bYMarking)
                {
                    g.DrawString(strVertScale, axisFont, new SolidBrush(seriesData.m_lineColor), rf, strFmt);
                }
                if (i == (xAxisNum + 1) / 2)
                {
                    p.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid; //实线
                    if (bDrawGradLineY)
                    {
                        g.DrawLine(p, 0, div * i + startPostion.Y, pictureBox_Wave.ClientRectangle.Width, div * i + startPostion.Y);
                    }
                    p.DashStyle = System.Drawing.Drawing2D.DashStyle.Custom; //虚线
                    p.DashPattern = new float[] { 2, 2 };
                }
                else
                {
                    if (bDrawGradLineY)
                    {
                        g.DrawLine(p, 0, div * i + startPostion.Y, pictureBox_Wave.ClientRectangle.Width, div * i + startPostion.Y);
                    }
                }
            }
            //绘制最后一行的实线
            p.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid; //实线
            if (bDrawGradLineY)
            {
                g.DrawLine(p, 0, endPostion.Y, endPostion.X, endPostion.Y);
            }
            p.DashStyle = System.Drawing.Drawing2D.DashStyle.Custom; //虚线
            p.DashPattern = new float[] { 2, 2 };


            //绘制最右侧水平坐标标示
            string strHorizScale = CValue2String.time2String(maxValueX);
            sf = g.MeasureString(strHorizScale, axisFont);
            rf = new RectangleF(this.pictureBox_Wave.Width - sf.Width, 0, sf.Width, sf.Height);
            //绘制最右侧水平坐标标示
            if (bXMarking)
            {
                g.DrawString(strHorizScale, axisFont, new SolidBrush(Color.Orange), rf, strFmt);
            }
            //绘制y方向的刻度线条
            for (int i = 1; i <= 9; i++)
            {
                float div = (float)pictureBox_Wave.ClientRectangle.Width / (9 + 1);
                double timePerGitter = (maxValueX - minValueX) / 10.0;
                strHorizScale = CValue2String.time2String(minValueX + timePerGitter * i);
                sf = g.MeasureString(strHorizScale, axisFont);
                rf = new RectangleF(div * i - sf.Width / 2, 0, sf.Width, sf.Height);
                /*绘制其余的水平坐标标示*/
                if (bXMarking)
                {
                    g.DrawString(strHorizScale, axisFont, new SolidBrush(Color.Orange), rf, strFmt);
                }
                if (i == 5)
                {
                    p.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid; //实线
                    if (bDrawGradLineX)
                    {
                        g.DrawLine(p, div * i, 0 + startPostion.Y, div * i, areaHeight + startPostion.Y);
                    }
                    p.DashStyle = System.Drawing.Drawing2D.DashStyle.Custom; //虚线
                    p.DashPattern = new float[] { 2, 2 };
                }
                else
                {
                    if (bDrawGradLineX)
                    {
                        g.DrawLine(p, div * i, 0 + startPostion.Y, div * i, areaHeight + startPostion.Y);
                    }
                }
            }
            //是否绘制图形
            if (bDraw)
            {
                string strChanID = seriesData.strChanID;
                int indexChan = listChanWaveView.FindIndex(chanItem => (chanItem.m_strChanID == strChanID));
                if (indexChan != -1)
                {
                    if (listChanWaveView[indexChan].m_bCheck)//选中的才显示
                    {
                        xScaleCount = seriesData.dispData.Count;

                        horizontalBetween = canvasWidth / xScaleCount;
                        verticalBetween = charHeight / yScaleCount;
                        //intervalValueY = (float)(maxValueY - minValueY) / yScaleCount;
                        intervalValueY = (float)(seriesData.scaleMax - seriesData.scaleMin) / yScaleCount;

                        //计算0值的坐标
                        //int tempv = (int)((Math.Abs(seriesData.scaleMin) - 0) / intervalValueY);//得到0到最小值的间隔距离；
                        int tempv = (int)((0 - seriesData.scaleMin) / intervalValueY);
                        float zeroY = endPostion.Y - tempv * verticalBetween;//值为0点Y抽坐标；
                        if (seriesData.dispData.Count > 1)
                        {
                            int dataIndex = 0;
                            PointF[] arrDataPoint = new PointF[seriesData.dispData.Count];
                            int index = 0;
                            foreach (PointF pf in seriesData.dispData)
                            {
                                PointF temp = new PointF();
                                temp.X = startPostion.X + horizontalBetween * index;
                                temp.Y = zeroY - verticalBetween * pf.Y / intervalValueY;
                                arrDataPoint[dataIndex++] = temp;
                                index++;
                            }
                            /*重新构造波形点，用于显示,两点之间不能直接连线，需要横线连接*/
                            //List<PointF> listPonitf = new List<PointF>();

                            //for (int i = 0; i < arrDataPoint.Length; i++)
                            //{
                            //    if (i != arrDataPoint.Length - 1)
                            //    {
                            //        PointF temp1 = arrDataPoint[i];
                            //        PointF temp2 = arrDataPoint[i + 1];
                            //        listPonitf.Add(temp1);
                            //        if (temp1.Y != temp2.Y)
                            //        {
                            //            PointF temp = new PointF();
                            //            temp.X = temp2.X;
                            //            temp.Y = temp1.Y;
                            //            listPonitf.Add(temp);
                            //        }
                            //    }
                            //    else
                            //    {
                            //        listPonitf.Add(arrDataPoint[i]);
                            //    }
                            //}
                            //PointF[] arrDataPoint_New = new PointF[listPonitf.Count];
                            //for (int i = 0; i < listPonitf.Count; i++)
                            //{
                            //    arrDataPoint_New[i] = listPonitf[i];
                            //}
                            //g.DrawLines(new Pen(new SolidBrush(seriesData.m_lineColor), 1F), arrDataPoint_New);
                            g.DrawLines(new Pen(new SolidBrush(seriesData.m_lineColor), 1F), arrDataPoint);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 绘制指定的单条波形,用于autozone显示
        /// </summary>
        /// <param name="g"></param>
        /// <param name="listSeriesData"></param>
        private void Drawing(ref Graphics g, Series seriesData, int offset, bool bDraw = true)
        {
            g.SmoothingMode = SmoothingMode.HighQuality;  //图片柔顺模式选择
            //g.InterpolationMode = InterpolationMode.HighQualityBicubic;//高质量
            //g.CompositingQuality = CompositingQuality.HighQuality;//再加一点
            Pen p = new Pen(Color.Gray, 1);//定义了一个灰色,宽度为1的画笔
            p.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid; //实线
            Rectangle rect = new Rectangle(pictureBox_Wave.ClientRectangle.X, pictureBox_Wave.ClientRectangle.Y,
                                             pictureBox_Wave.ClientRectangle.X + pictureBox_Wave.ClientRectangle.Width,
                                             pictureBox_Wave.ClientRectangle.Y + pictureBox_Wave.ClientRectangle.Height);
            g.DrawRectangle(p, rect);//绘制图形框的矩形外框

            p.DashStyle = System.Drawing.Drawing2D.DashStyle.Custom; //虚线
            p.DashPattern = new float[] { 2, 2 };
            //显示的轴线个数，奇数个轴线
            int xAxisNum = 3;
            if (bLargeScale)
            {
                xAxisNum = 5;
            }
            if (xAxisNum % 2 == 0)
            {
                xAxisNum = xAxisNum + 1;
            }
            //计算垂直范围
            float vertScale = seriesData.scaleMax - seriesData.scaleMin;
            float vertPerGitter = (float)vertScale / (xAxisNum + 1);
            StringFormat strFmt = new System.Drawing.StringFormat();
            strFmt.Alignment = StringAlignment.Center; //文本水平居中
            strFmt.LineAlignment = StringAlignment.Center; //文本垂直居中
            Font axisFont = new Font("微软雅黑", 6, FontStyle.Bold);
            //先绘制顶部位置的纵坐标标示
            string strVertScale = CValue2String.voltage2String(seriesData.scaleMax);
            SizeF sf = g.MeasureString(strVertScale, axisFont);
            RectangleF rf = new RectangleF(0, 0 + startPostion.Y, sf.Width, sf.Height);
            //绘制Y轴
            if (bYMarking)
            {
                g.DrawString(strVertScale, axisFont, new SolidBrush(seriesData.m_lineColor), rf, strFmt);
            }
            //再绘制底部位置的纵坐标标示

            strVertScale = CValue2String.voltage2String(seriesData.scaleMin);
            sf = g.MeasureString(strVertScale, axisFont);

            //规定单条绘制区域的高度
            float areaHeight = endPostion.Y - startPostion.Y;
            rf = new RectangleF(0, startPostion.Y + areaHeight - sf.Height, sf.Width, sf.Height);
            if (bYMarking)
            {
                g.DrawString(strVertScale, axisFont, new SolidBrush(seriesData.m_lineColor), rf, strFmt);
            }

            //绘制x方向刻度线
            for (int i = 1; i <= xAxisNum; i++)
            {
                float div = (float)areaHeight / (xAxisNum + 1);
                strVertScale = CValue2String.voltage2String(seriesData.scaleMax - vertPerGitter * i);
                sf = g.MeasureString(strVertScale, axisFont);
                rf = new RectangleF(0, startPostion.Y + div * i - sf.Height / 2, sf.Width, sf.Height);
                //绘制纵坐标标示
                if (bYMarking)
                {
                    g.DrawString(strVertScale, axisFont, new SolidBrush(seriesData.m_lineColor), rf, strFmt);
                }
                if (i == (xAxisNum + 1) / 2)
                {
                    p.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid; //实线
                    if (bDrawGradLineY)
                    {
                        g.DrawLine(p, 0, div * i + startPostion.Y, pictureBox_Wave.ClientRectangle.Width, div * i + startPostion.Y);
                    }
                    p.DashStyle = System.Drawing.Drawing2D.DashStyle.Custom; //虚线
                    p.DashPattern = new float[] { 2, 2 };
                }
                else
                {
                    if (bDrawGradLineY)
                    {
                        g.DrawLine(p, 0, div * i + startPostion.Y, pictureBox_Wave.ClientRectangle.Width, div * i + startPostion.Y);
                    }
                }
            }
            //绘制最后一行的实线
            p.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid; //实线
            if (bDrawGradLineY)
            {
                g.DrawLine(p, 0, endPostion.Y, endPostion.X, endPostion.Y);
            }
            p.DashStyle = System.Drawing.Drawing2D.DashStyle.Custom; //虚线
            p.DashPattern = new float[] { 2, 2 };


            //绘制最右侧水平坐标标示
            string strHorizScale = CValue2String.time2String(maxValueX);
            sf = g.MeasureString(strHorizScale, axisFont);
            rf = new RectangleF(this.pictureBox_Wave.Width - sf.Width, 0, sf.Width, sf.Height);
            //绘制最右侧水平坐标标示
            if (bXMarking)
            {
                g.DrawString(strHorizScale, axisFont, new SolidBrush(Color.Orange), rf, strFmt);
            }
            //绘制y方向的刻度线条
            for (int i = 1; i <= 9; i++)
            {
                float div = (float)pictureBox_Wave.ClientRectangle.Width / (9 + 1);
                double timePerGitter = (maxValueX - minValueX) / 10.0;
                strHorizScale = CValue2String.time2String(minValueX + timePerGitter * i);
                sf = g.MeasureString(strHorizScale, axisFont);
                rf = new RectangleF(div * i - sf.Width / 2, 0, sf.Width, sf.Height);
                /*绘制其余的水平坐标标示*/
                if (bXMarking)
                {
                    g.DrawString(strHorizScale, axisFont, new SolidBrush(Color.Orange), rf, strFmt);
                }
                if (i == 5)
                {
                    p.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid; //实线
                    if (bDrawGradLineX)
                    {
                        g.DrawLine(p, div * i, 0 + startPostion.Y, div * i, areaHeight + startPostion.Y);
                    }
                    p.DashStyle = System.Drawing.Drawing2D.DashStyle.Custom; //虚线
                    p.DashPattern = new float[] { 2, 2 };
                }
                else
                {
                    if (bDrawGradLineX)
                    {
                        g.DrawLine(p, div * i, 0 + startPostion.Y, div * i, areaHeight + startPostion.Y);
                    }
                }
            }
            //是否绘制图形
            if (bDraw)
            {
                string strChanID = seriesData.strChanID;
                int indexChan = listChanWaveView.FindIndex(chanItem => (chanItem.m_strChanID == strChanID));
                if (indexChan != -1)
                {
                    if (listChanWaveView[indexChan].m_bCheck)//选中的才显示
                    {
                        xScaleCount = offset;// seriesData.dispData.Count;

                        horizontalBetween = canvasWidth / xScaleCount;
                        verticalBetween = charHeight / yScaleCount;
                        //intervalValueY = (float)(maxValueY - minValueY) / yScaleCount;
                        intervalValueY = (float)(seriesData.scaleMax - seriesData.scaleMin) / yScaleCount;

                        //计算0值的坐标
                        //int tempv = (int)((Math.Abs(seriesData.scaleMin) - 0) / intervalValueY);//得到0到最小值的间隔距离；
                        int tempv = (int)((0 - seriesData.scaleMin) / intervalValueY);
                        float zeroY = endPostion.Y - tempv * verticalBetween;//值为0点Y抽坐标；
                        if (offset > 1)
                        {
                            int dataIndex = 0;
                            PointF[] arrDataPoint = new PointF[offset];
                            int index = 0;
                            for (int i = 0; i < offset; i++)
                            {
                                PointF temp = new PointF();
                                temp.X = startPostion.X + horizontalBetween * index;
                                temp.Y = zeroY - verticalBetween * seriesData.dispData[i].Y / intervalValueY;
                                arrDataPoint[dataIndex++] = temp;
                                index++;
                            }
                            /*重新构造波形点，用于显示,两点之间不能直接连线，需要横线连接*/
                            //List<PointF> listPonitf = new List<PointF>();

                            //for (int i = 0; i < arrDataPoint.Length; i++)
                            //{
                            //    if (i != arrDataPoint.Length - 1)
                            //    {
                            //        PointF temp1 = arrDataPoint[i];
                            //        PointF temp2 = arrDataPoint[i + 1];
                            //        listPonitf.Add(temp1);
                            //        if (temp1.Y != temp2.Y)
                            //        {
                            //            PointF temp = new PointF();
                            //            temp.X = temp2.X;
                            //            temp.Y = temp1.Y;
                            //            listPonitf.Add(temp);
                            //        }
                            //    }
                            //    else
                            //    {
                            //        listPonitf.Add(arrDataPoint[i]);
                            //    }
                            //}
                            //PointF[] arrDataPoint_New = new PointF[listPonitf.Count];
                            //for (int i = 0; i < listPonitf.Count; i++)
                            //{
                            //    arrDataPoint_New[i] = listPonitf[i];
                            //}
                            //g.DrawLines(new Pen(new SolidBrush(seriesData.m_lineColor), 1F), arrDataPoint_New);
                            g.DrawLines(new Pen(new SolidBrush(seriesData.m_lineColor), 1F), arrDataPoint);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 更新波形显示数据
        /// </summary>
        public void update_PictureBoxView_Data()
        {
            listWaveSeriesShow.Clear();

            if (modWavMonitor.listOscDataProcess.Count != 0)
            {
                minValueX = modWavMonitor.listOscDataProcess[0].xOrigin;
                maxValueX = minValueX + (modWavMonitor.listOscDataProcess[0].memDepth) * modWavMonitor.listOscDataProcess[0].xIncrement;
            }
            else
            {
                minValueX = -2.000000E-4;
                maxValueX = minValueX + (1000000) * 4.000000E-10;
            }
            Parallel.For(0, modWavMonitor.listOscDataProcess.Count, item =>
                {
                    Series dataTemp = new Series();
                    dataTemp.strDispID = "";
                    dataTemp.strChanID = modWavMonitor.listOscDataProcess[item].strChanID;
                    int index = listChanWaveView.FindIndex(ex => (ex.m_strChanID == modWavMonitor.listOscDataProcess[item].strChanID));
                    if (index != -1)
                    {
                        dataTemp.m_lineColor = listChanWaveView[index].m_selectColor;
                        for (int i = 0; i < modWavMonitor.listOscDataProcess[item].dataDispRough.Length; i++)
                        {
                            PointF temPoint = new PointF();
                            temPoint.X = 0;
                            /*加上原始的偏移*/
                            //temPoint.Y = (float)(item.dataDispRough[i] + item.vertOffset);
                            temPoint.Y = (float)(modWavMonitor.listOscDataProcess[item].dataDispRough[i]);
                            dataTemp.dispData.Add(temPoint);
                        }
                        //垂直显示范围
                        dataTemp.scaleMax = (float)listChanWaveView[index].m_vertScalMax;
                        dataTemp.scaleMin = (float)listChanWaveView[index].m_vertScalMin;
                        lock (objlock)
                        {
                            listWaveSeriesShow.Add(dataTemp);
                        }

                    }
                });

           
            //根据起始位置和终止位置重新计算
            double minValueXTemp = 0.0;
            double maxValueXTemp = 0.0;
            try
            {
                minValueXTemp = Convert.ToDouble(textBox_MinX.Text);
                maxValueXTemp = Convert.ToDouble(textBox_MaxX.Text);
            }
            catch
            {
                minValueXTemp = minValueX;
                maxValueXTemp = maxValueX;
                point_StartPos = 0;//起始点
                point_StopPos = modWavMonitor.listOscDataProcess[0].memDepth - 1;//终止点
            }
            if (minValueXTemp > maxValueXTemp)
            {
                point_StartPos = 0;//起始点
                point_StopPos = modWavMonitor.listOscDataProcess[0].memDepth - 1;//终止点
            }
            if (minValueXTemp < modWavMonitor.listOscDataProcess[0].xOrigin)
            {
                point_StartPos = 0;//起始点
            }
            if (maxValueXTemp > modWavMonitor.listOscDataProcess[0].xOrigin + 1000000 * modWavMonitor.listOscDataProcess[0].xIncrement)
            {
                point_StopPos = modWavMonitor.listOscDataProcess[0].memDepth - 1;//终止点
            }
            //if (bEnlagerClick)
            {
                point_StartPos = (int)((minValueXTemp - modWavMonitor.listOscDataProcess[0].xOrigin) / modWavMonitor.listOscDataProcess[0].xIncrement);
                point_StopPos = (int)((maxValueXTemp - modWavMonitor.listOscDataProcess[0].xOrigin) / modWavMonitor.listOscDataProcess[0].xIncrement);
                // 压缩时，至少有2k点，初始位置对齐到1000的整数倍
                if (point_StopPos - point_StartPos + 1 >= pointDisplay * 2)
                {
                    int start = (int)Math.Floor(point_StartPos / (pointDisplay * 1.0));
                    int stop = (int)Math.Ceiling(point_StopPos / (pointDisplay * 1.0));
                    point_StopPos = stop * pointDisplay - 1;
                    point_StartPos = start * pointDisplay;
                }
                // 大于1K，小于2K，从左侧取2K点
                else if ((point_StopPos - point_StartPos + 1 >= pointDisplay) && (point_StopPos - point_StartPos + 1 < pointDisplay * 2))
                {
                    int start = (int)Math.Floor(point_StartPos / (pointDisplay * 1.0));
                    point_StartPos = start * pointDisplay;
                    //point_StartPos = point_StartPosTemp;
                    point_StopPos = point_StartPos + pointDisplay * 2 - 1;
                }
                int pointNum = point_StopPos - point_StartPos + 1;
                if (point_StopPos <= point_StartPos + 9)
                {
                    return;
                }
                int mutipleFin;
                if (pointNum < pointDisplay)
                {
                    mutipleFin = modWavMonitor.findFinMutiple(pointDisplay, pointNum);
                    if (mutipleFin > 200)
                    {
                        //限制在200倍内插
                        mutipleFin = 200;
                    }
                    //显示当前的内插倍数
                    //label_InterMutiple.Text = "X" + mutipleFin.ToString();
                }
                else
                {
                    mutipleFin = 1;
                    //显示当前的内插倍数
                    //label_InterMutiple.Text = "X" + mutipleFin.ToString();
                }
                //波形放大插值操作
                bool bResult = modWavMonitor.calEnlargeWaveData_UsingCDLL(point_StartPos, point_StopPos, mutipleFin);
                //如果插值的话，需要重新计算结束位置
                if (mutipleFin != 1)
                {
                    int numRange = (int)Math.Ceiling(pointDisplay / (mutipleFin * 1.0));
                    point_StopPos = point_StartPos + numRange - 1;
                }
                //计算x轴的范围
                minValueX = modWavMonitor.listOscDataProcess[0].xOrigin + (point_StartPos) * modWavMonitor.listOscDataProcess[0].xIncrement;
                maxValueX = minValueX + (point_StopPos - point_StartPos + 1) * modWavMonitor.listOscDataProcess[0].xIncrement;
                foreach (Module_WaveMonitor.OscilloscopeDataProcess item in modWavMonitor.listOscDataProcess)
                {
                    int index = listWaveSeriesShow.FindIndex(member => (member.strChanID == item.strChanID));
                    if (index != -1)
                    {
                        if (bResult)
                        {
                            listWaveSeriesShow[index].dispData.Clear();
                            for (int i = 0; i < item.dataDispFin.Length; i++)
                            {
                                PointF temPoint = new PointF();
                                temPoint.X = 0;
                                /*加上显示时的偏移*/
                                //temPoint.Y = (float)(item.dataDispFin[i] - item.vertOffset + (item.vertViewOffset - item.vertOffset));
                                temPoint.Y = (float)(item.dataDispFin[i] + (item.vertViewOffset - item.vertOffset));
                                listWaveSeriesShow[index].dispData.Add(temPoint);
                            }
                        }
                    }
                }
                }

                //委托方式刷新
                if (bAutoZone)
            {
                update_PictureBoxView_Screen(true);
            }
            else
            {
                update_PictureBoxView_Screen();
            }

            //刷新表格
            updateDataGridViewChannel();

        }
        /// <summary>
        /// 更新波形显示数据
        /// </summary>
        public void update_PictureBoxView_Data_Roll()
        {
            if (modWavMonitor.listOscDataProcess.Count != 0)
            {
                minValueX = modWavMonitor.listOscDataProcess[0].xOrigin;
                maxValueX = minValueX + (modWavMonitor.listOscDataProcess[0].memDepth) * modWavMonitor.listOscDataProcess[0].xIncrement;
            }
            else
            {
                minValueX = -2.000000E-4;
                maxValueX = minValueX + (1000000) * 4.000000E-10;
            }

            if (bAutoZone)
            {
                // 如果list为0 则标示是第一次收数据
                if (listWaveSeriesShow.Count == 0)
                {
                    Parallel.For(0, modWavMonitor.listOscDataProcess.Count, item =>
                    {
                        Series dataTemp = new Series();
                        dataTemp.strDispID = "";
                        dataTemp.strChanID = modWavMonitor.listOscDataProcess[item].strChanID;
                        int index = listChanWaveView.FindIndex(ex => (ex.m_strChanID == modWavMonitor.listOscDataProcess[item].strChanID));
                        if (index != -1)
                        {
                            dataTemp.m_lineColor = listChanWaveView[index].m_selectColor;
                            for (int i = 0; i < modWavMonitor.listOscDataProcess[item].dataDispRough.Length; i++)
                            {
                                PointF temPoint = new PointF();
                                temPoint.X = 0;
                                /*加上原始的偏移*/
                                //temPoint.Y = (float)(item.dataDispRough[i] + item.vertOffset);
                                temPoint.Y = (float)(modWavMonitor.listOscDataProcess[item].dataDispRough[i] - modWavMonitor.listOscDataProcess[item].vertOffset);
                                dataTemp.dispData.Add(temPoint);
                            }
                            //垂直显示范围
                            dataTemp.scaleMax = (float)listChanWaveView[index].m_vertScalMax;
                            dataTemp.scaleMin = (float)listChanWaveView[index].m_vertScalMin;
                            lock (objlock)
                            {
                                listWaveSeriesShow.Add(dataTemp);
                            }
                        }
                    });
                    update_PictureBoxView_Screen();
                }
                else
                {
                    // 标示已经接收完一次数据
                    for (int i = 0; i < drawRangeDiv; i++)
                    {
                        Parallel.For(0, listWaveSeriesShow.Count, item =>
                            {
                                int index = modWavMonitor.listOscDataProcess.FindIndex(ex => ex.strChanID == listWaveSeriesShow[item].strChanID);
                                if (index != -1)
                                {
                                    // 向左移动count/50个点
                                    for (int j = listWaveSeriesShow[item].dispData.Count / drawRangeDiv; j < listWaveSeriesShow[item].dispData.Count; j++)
                                    {
                                        listWaveSeriesShow[item].dispData[j - listWaveSeriesShow[item].dispData.Count / drawRangeDiv] = listWaveSeriesShow[item].dispData[j];
                                    }
                                    // 计算新数据开始拷贝的长度
                                    int newDataLength = i * (modWavMonitor.listOscDataProcess[index].dataDispRough.Length / drawRangeDiv);
                                    // 将新收的数据填充到最右侧
                                    for (int j = listWaveSeriesShow[item].dispData.Count - listWaveSeriesShow[item].dispData.Count / drawRangeDiv; j < listWaveSeriesShow[item].dispData.Count; j++)
                                    {
                                        PointF temPoint = new PointF();
                                        temPoint.X = 0;
                                        /*加上原始的偏移*/
                                        temPoint.Y = (float)(modWavMonitor.listOscDataProcess[index].dataDispRough[newDataLength++]
                                            - modWavMonitor.listOscDataProcess[index].vertOffset);
                                        listWaveSeriesShow[item].dispData[j] = temPoint;
                                    }
                                }
                            });
                        update_PictureBoxView_Screen();
                    }
                }
            }
            else
            {
                listWaveSeriesShow.Clear();
                Parallel.For(0, modWavMonitor.listOscDataProcess.Count, item =>
                {
                    Series dataTemp = new Series();
                    dataTemp.strDispID = "";
                    dataTemp.strChanID = modWavMonitor.listOscDataProcess[item].strChanID;
                    int index = listChanWaveView.FindIndex(ex => (ex.m_strChanID == modWavMonitor.listOscDataProcess[item].strChanID));
                    if (index != -1)
                    {
                        dataTemp.m_lineColor = listChanWaveView[index].m_selectColor;
                        for (int i = 0; i < modWavMonitor.listOscDataProcess[item].dataDispRough.Length; i++)
                        {
                            PointF temPoint = new PointF();
                            temPoint.X = 0;
                            /*加上原始的偏移*/
                            //temPoint.Y = (float)(item.dataDispRough[i] + item.vertOffset);
                            temPoint.Y = (float)(modWavMonitor.listOscDataProcess[item].dataDispRough[i] - modWavMonitor.listOscDataProcess[item].vertOffset);
                            dataTemp.dispData.Add(temPoint);
                        }
                        //垂直显示范围
                        dataTemp.scaleMax = (float)listChanWaveView[index].m_vertScalMax;
                        dataTemp.scaleMin = (float)listChanWaveView[index].m_vertScalMin;
                        listWaveSeriesShow.Add(dataTemp);
                    }
                });
                //foreach (Module_WaveMonitor.OscilloscopeDataProcess item in modWavMonitor.listOscDataProcess)
                //{
                //    Series dataTemp = new Series();
                //    dataTemp.strDispID = "";
                //    dataTemp.strChanID = item.strChanID;
                //    int index = listChanWaveView.FindIndex(ex => (ex.m_strChanID == item.strChanID));
                //    if (index != -1)
                //    {
                //        dataTemp.m_lineColor = listChanWaveView[index].m_selectColor;
                //        for (int i = 0; i < item.dataDispRough.Length; i++)
                //        {
                //            PointF temPoint = new PointF();
                //            temPoint.X = 0;
                //            /*加上原始的偏移*/
                //            //temPoint.Y = (float)(item.dataDispRough[i] + item.vertOffset);
                //            temPoint.Y = (float)(item.dataDispRough[i] + item.vertViewOffset);
                //            dataTemp.dispData.Add(temPoint);
                //        }
                //        //垂直显示范围
                //        dataTemp.scaleMax = (float)listChanWaveView[index].m_vertScalMax;
                //        dataTemp.scaleMin = (float)listChanWaveView[index].m_vertScalMin;
                //        listWaveSeriesShow.Add(dataTemp);
                //    }
                //}
                update_PictureBoxView_Screen();
            }

        }

        public void updateDataGridViewChannel()
        {
            if (this.pictureBox_Wave.InvokeRequired)
            {
                updateChannelDataGridView s = new updateChannelDataGridView(updateDataGridViewChannel);
                this.pictureBox_Wave.Invoke(s);
            }
            else
            {
                if (panel_InfoAndMeas.Visible == true)
                {
                    panel_InfoAndMeas.Visible = false;
                    CReflection.callObjectEvent(iconToolStripButton_Table, "OnClick", null);
                }

            }
        }

        /// <summary>
        /// 更新波形显示-屏幕刷新（委托）
        /// </summary>
        public void update_PictureBoxView_Screen(bool bNormal = false)
        {
            if (this.pictureBox_Wave.InvokeRequired)
            {
                waveView_Refresh s = new waveView_Refresh(update_PictureBoxView_Screen);
                this.pictureBox_Wave.Invoke(s, bNormal);
            }
            else
            {
                //panel_InfoAndMeas.Visible = false;
                if ((point_StartPos == 0) && (point_StopPos == 0))
                {
                    label_InterMutiple.Text = "X1";
                }
                if (bNormal == false)
                {
                    autZonRange = drawRangeDiv - 1;
                    pictureBox_Wave_PaintWave();
                }
                else
                {
                    for (int i = 0; i < drawRangeDiv; i++)
                    {
                        autZonRange = i;
                        pictureBox_Wave_PaintWave();
                        // 这里需要刷新一下，不然新的图层刷新不出来
                        pictureBox_Wave.Refresh();
                    }
                }
            }
        }
        /// <summary>
        /// 刷新数据绑定委托-显示信息
        /// </summary>
        private void dataSourceBindingChanInfo(ref DataTable dt)
        {
            if (dataGridView_ChanInfo.InvokeRequired)
            {
                dataGridViewRefresh s = new dataGridViewRefresh(dataSourceBindingChanInfo);
                dataGridView_ChanInfo.Invoke(s, dt);
            }
            else
            {
                dataGridView_ChanInfo.DataSource = dt.Copy();
            }
        }
        /// <summary>
        /// 刷新数据绑定委托-显示测量
        /// </summary>
        private void dataSourceBindingMeasInfo(ref DataTable dt)
        {
            //if (dataGridView_MeasInfo.InvokeRequired)
            //{
            //    dataGridViewRefresh s = new dataGridViewRefresh(dataSourceBindingMeasInfo);
            //    dataGridView_MeasInfo.Invoke(s, dt);
            //}
            //else
            //{
            //    dataGridView_MeasInfo.DataSource = dt.Copy();
            //}
        }
        /// <summary>
        /// 更新通道标签显示
        /// </summary>
        public void update_ChanLabelFlowPanel()
        {
            if (this.InvokeRequired)
            {
                updateChanLabelFlowPanel s = new updateChanLabelFlowPanel(update_ChanLabelFlowPanel);
                this.Invoke(s);
            }
            else
            {
                flowLayoutPanel_LabelItem.Controls.Clear();
                foreach (UCChanWaveView item in listChanWaveView)
                {
                    flowLayoutPanel_LabelItem.Controls.Add(item);
                    //订阅事件
                    Subscribe(item);
                }
                panel_WaveLabel.Controls.Clear();
                //先清除panel中已有的标签
                //重新根据需要绘制的波形数重新排列所有需要显示的波形
                if (bAutoZone)
                {
                    for (int i = 0; i < listChanWaveView.Count; i++)
                    {
                        //所有的标签都是选中状态
                        listChanWaveView[i].label_ChanID_Set(true);
                        /*新建一个波形标记用于标示当前通道*/
                        if (i < autZonNormalScale)
                        {
                            UCChanLabel ucChanLabTemp = new UCChanLabel();
                            ucChanLabTemp.setLabel(listChanWaveView[i].m_strChanID, listChanWaveView[i].m_selectColor);
                            //ucChanLabTemp.Location = new Point(ucChanLabel_Tag.Location.X, panel_WaveLabel.Height / (listChanWaveView.Count * 2) - ucChanLabTemp.Height / 2 + i * (panel_WaveLabel.Height / listChanWaveView.Count));
                            ucChanLabTemp.Location = new Point(ucChanLabel_Tag.Location.X,
                            (int)(panel_WaveLabel.Height / (autZonNormalScale * 2.0) - ucChanLabTemp.Height / 2.0 + i * (panel_WaveLabel.Height / autZonNormalScale * 1.0)));
                            panel_WaveLabel.Controls.Add(ucChanLabTemp);
                            panel_WaveLabel.Refresh();
                        }

                    }
                }
            }

        }
        /// <summary>
        /// 使能工具栏
        /// </summary>
        /// <param name="bEnable"></param>
        public void toolStripWaveView_Enable(bool bEnable)
        {
            if (this.InvokeRequired)
            {
                toolStripWaveViewEnable s = new toolStripWaveViewEnable(toolStripWaveView_Enable);
                this.Invoke(s, bEnable);
            }
            else
            {
                if (bEnable)
                {
                    toolStrip_WaveView.Enabled = true;
                }
                else
                {
                    toolStrip_WaveView.Enabled = false;
                    panel_Min.Visible = false;
                    panel_Max.Visible = false;
                    iconButton_Left.Visible = false;
                    iconButton_Right.Visible = false;
                    iconToolStripButton_Enlager.IconColor = Color.White;
                    this.pictureBox_Wave.Cursor = Cursors.Default;
                }
            }

        }
        /// <summary>
        /// 顶层透明pictureBox双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox_DrawRect_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (bEnlagerClick)
            {
                point_StartPos = 0;//起始点
                point_StopPos = modWavMonitor.listOscDataProcess[0].memDepth - 1;//终止点
                totalPointNum = point_StopPos - point_StartPos + 1;//所有点

                if (modWavMonitor.listOscDataProcess.Count != 0)
                {
                    minValueX = modWavMonitor.listOscDataProcess[0].xOrigin;
                    maxValueX = minValueX + (modWavMonitor.listOscDataProcess[0].memDepth) * modWavMonitor.listOscDataProcess[0].xIncrement;
                }
                foreach (Module_WaveMonitor.OscilloscopeDataProcess item in modWavMonitor.listOscDataProcess)
                {
                    item.vertViewOffset = item.vertOffset;
                }

                foreach (UCChanWaveView item in listChanWaveView)
                {
                    int index = modWavMonitor.listOscDataProcess.FindIndex(ex => (ex.strChanID == item.m_strChanID));
                    if (index != -1)
                    {
                        item.updateChanOffset(modWavMonitor.listOscDataProcess[index].vertViewOffset);
                        if (item.m_bSelect)
                        {
                            float offset = (float)modWavMonitor.listOscDataProcess[index].vertViewOffset;
                            float scale = maxValueY - minValueY;
                            float ampPrePoint = scale / panel_WaveLabel.Height;
                            int pos = (int)(offset / ampPrePoint);
                            ucChanLabel_Tag.Location = new Point(ucChanLabel_Tag.Location.X, panel_WaveLabel.Height / 2 - ucChanLabel_Tag.Height / 2 - pos);
                        }

                    }
                }
                label_InterMutiple.Text = "X1";
                foreach (Module_WaveMonitor.OscilloscopeDataProcess item in modWavMonitor.listOscDataProcess)
                {
                    int index = listWaveSeriesShow.FindIndex(member => (member.strChanID == item.strChanID));
                    if (index != -1)
                    {
                        listWaveSeriesShow[index].dispData.Clear();
                        for (int i = 0; i < item.dataDispRough.Length; i++)
                        {
                            PointF temPoint = new PointF();
                            temPoint.X = 0;
                            temPoint.Y = (float)(item.dataDispRough[i]);
                            listWaveSeriesShow[index].dispData.Add(temPoint);
                        }
                    }
                }
                //update_PictureBoxView_Data();
                update_PictureBoxView_Screen();
            }
        }
        /// <summary>
        /// 顶层透明pictureBox鼠标按下事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox_DrawRect_MouseDown(object sender, MouseEventArgs e)
        {
            if (bEnlagerClick)
            {
                bDrawStart = true;
                pointStart = e.Location;
            }
            if (e.Button == MouseButtons.Right)
            {
                this.contextMenuStrip_Right.Show(MousePosition);
            }
        }
        /// <summary>
        /// 顶层透明pictureBox鼠标移动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox_DrawRect_MouseMove(object sender, MouseEventArgs e)
        {
            if (bDrawStart)
            {
                pointContinue = e.Location;
                pictureBox_DrawRect.Invalidate();
            }
        }
        /// <summary>
        /// 顶层透明pictureBox鼠标抬起事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox_DrawRect_MouseUp(object sender, MouseEventArgs e)
        {
            bDrawStart = false;
            pictureBox_DrawRect.Invalidate();
            /*重新构建显示的数据*/
            int width = pictureBox_Wave.Width;
            int height = pictureBox_Wave.Height;
            int w = pointContinue.X - pointStart.X;
            if ((w <= 1) || (width < w))
            {
                pointStart = Point.Empty;
                pointContinue = Point.Empty;
                return;
            }
            int h = pointContinue.Y - pointStart.Y;
            if ((height < h) || (h <= 0))
            {
                pointStart = Point.Empty;
                pointContinue = Point.Empty;
                return;
            }
            if (modWavMonitor.listOscDataProcess.Count == 0)
            {
                return;
            }
            /*计算初始点和终止点------只有内存数据支持放大*/
            if (modWavMonitor.listOscDataProcess[0].memDepth != 0)
            {
                if (totalPointNum == 0)
                {
                    totalPointNum = modWavMonitor.listOscDataProcess[0].memDepth;
                }
                else
                {
                    totalPointNum = point_StopPos - point_StartPos + 1;
                }
            }
            else
            {
                pointStart = Point.Empty;
                pointContinue = Point.Empty;
                return;
            }
            // 计算初始位置
            int point_StopPosTemp = point_StartPos + (int)Math.Ceiling(totalPointNum * (pointContinue.X / (width * 1.0)));
            if (point_StopPosTemp > modWavMonitor.listOscDataProcess[0].memDepth)
            {
                point_StopPosTemp = modWavMonitor.listOscDataProcess[0].memDepth - 1;
            }
            int point_StartPosTemp = point_StartPos + (int)Math.Floor(totalPointNum * (pointStart.X / (width * 1.0)));

            if (point_StopPosTemp - point_StartPosTemp <= (pointDisplay / 200 - 1))
            {
                return;
            }
            else
            {
                // 压缩时，至少有2k点，初始位置对齐到1000的整数倍
                if (point_StopPosTemp - point_StartPosTemp + 1 >= pointDisplay * 2)
                {
                    int start = (int)Math.Floor(point_StartPosTemp / (pointDisplay * 1.0));
                    int stop = (int)Math.Ceiling(point_StopPosTemp / (pointDisplay * 1.0));
                    point_StopPos = stop * pointDisplay - 1;
                    point_StartPos = start * pointDisplay;
                }
                // 大于1K，小于2K，从左侧取2K点
                else if ((point_StopPosTemp - point_StartPosTemp + 1 >= pointDisplay) && (point_StopPosTemp - point_StartPosTemp + 1 < pointDisplay * 2))
                {
                    int start = (int)Math.Floor(point_StartPosTemp / (pointDisplay * 1.0));
                    point_StartPos = start * pointDisplay;
                    //point_StartPos = point_StartPosTemp;
                    point_StopPos = point_StartPos + pointDisplay * 2 - 1;
                }
                else
                {
                    // 小于1k的点，先插值，后截取
                    point_StopPos = point_StopPosTemp - 1;
                    point_StartPos = point_StartPosTemp;
                }
            }

            if (bEnlagerClick)
            {
                int pointNum = point_StopPos - point_StartPos + 1;
                int mutipleFin;
                if (pointNum < pointDisplay)
                {
                    mutipleFin = modWavMonitor.findFinMutiple(pointDisplay, pointNum);
                    if (mutipleFin > 200)
                    {
                        //限制在200倍内插
                        mutipleFin = 200;
                    }
                    //显示当前的内插倍数
                    label_InterMutiple.Text = "X" + mutipleFin.ToString();
                }
                else
                {
                    mutipleFin = 1;
                    //显示当前的内插倍数
                    label_InterMutiple.Text = "X" + mutipleFin.ToString();
                }
                //波形放大插值操作
                bool bResult = modWavMonitor.calEnlargeWaveData_UsingCDLL(point_StartPos, point_StopPos, mutipleFin);
                //如果插值的话，需要重新计算结束位置
                if (mutipleFin != 1)
                {
                    int numRange = (int)Math.Ceiling(pointDisplay / (mutipleFin * 1.0));
                    point_StopPos = point_StartPos + numRange - 1;
                }
                //计算x轴的范围
                minValueX = modWavMonitor.listOscDataProcess[0].xOrigin + (point_StartPos) * modWavMonitor.listOscDataProcess[0].xIncrement;
                maxValueX = minValueX + (point_StopPos - point_StartPos + 1) * modWavMonitor.listOscDataProcess[0].xIncrement;
                foreach (Module_WaveMonitor.OscilloscopeDataProcess item in modWavMonitor.listOscDataProcess)
                {
                    int index = listWaveSeriesShow.FindIndex(member => (member.strChanID == item.strChanID));
                    if (index != -1)
                    {
                        if (bResult)
                        {
                            listWaveSeriesShow[index].dispData.Clear();
                            for (int i = 0; i < item.dataDispFin.Length; i++)
                            {
                                PointF temPoint = new PointF();
                                temPoint.X = 0;
                                /*加上显示时的偏移*/
                                //temPoint.Y = (float)(item.dataDispFin[i] - item.vertOffset + (item.vertViewOffset - item.vertOffset));
                                temPoint.Y = (float)(item.dataDispFin[i] + (item.vertViewOffset - item.vertOffset));
                                listWaveSeriesShow[index].dispData.Add(temPoint);
                            }
                        }
                    }

                }
                update_PictureBoxView_Screen();
            }
            pointStart = Point.Empty;
            pointContinue = Point.Empty;
        }
        /// <summary>
        /// pictureBox_DrawRect 绘图事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox_DrawRect_Paint(object sender, PaintEventArgs e)
        {
            /*绘制矩形*/
            if (bDrawStart & bEnlagerClick)
            {

                Pen pen = new Pen(Color.White, 1f);
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

                //实时的画矩形
                int w = pointContinue.X - pointStart.X;
                int h = pointContinue.Y - pointStart.Y;
                Rectangle rect = new Rectangle(pointStart, new Size(w, h));
                e.Graphics.DrawRectangle(pen, rect);
                pen.Dispose();
            }
        }
        /// <summary>
        /// 清空界面显示
        /// </summary>
        public void waveMonitorForm_Clear()
        {
            bXMarking = false;
            bYMarking = true;
            bDrawGradLineX = true;
            bDrawGradLineY = true;
            maxValueY = 10.0F;
            minValueY = -10.0F;
            autZonLittleScale = 32;
            autZonNormalScale = 16;
            autZonLargeScale = 8;
            curPosition = 0;
            bLittleScale = false;
            bNormalScale = true;
            bLargeScale = false;
            //自动区域显示
            bAutoZone = true;
            //半峰高对齐区域显示
            bEditZone = false;
            bEnlagerClick = false;//波形放大标志
            bDrawStart = false;
            pointStart = Point.Empty;
            pointContinue = Point.Empty;
            point_StartPos = 0;//起始点
            point_StopPos = 0;//终止点
            totalPointNum = 0;//所有点
            modWavMonitor.listOscDataProcess.Clear();
            // 取消已有事件的注册
            foreach (UCChanWaveView item in listChanWaveView)
            {
                UnSubscribe(item);
            }
            listChanWaveView.Clear();
            listWaveSeriesShow.Clear();
            dtMeasInfo.Rows.Clear();
            dtChanInfo.Rows.Clear();
            // 清除显示
            flowLayoutPanel_LabelItem.Controls.Clear();
            panel_WaveLabel.Controls.Clear();
            if ((pictureBox_Wave.Width <= 0) || (pictureBox_Wave.Height <= 0))
            {
                return;
            }
            Bitmap bmp = new Bitmap(pictureBox_Wave.Width, pictureBox_Wave.Height);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(System.Drawing.Color.Black);
            pictureBox_Wave.Image = bmp;
        }

        #endregion
        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_WaveMonitor_FormClosing(object sender, FormClosingEventArgs e)
        {
            //this.Hide();       
            //e.Cancel = true;
        }

        #region 打开关闭横纵轴显示
        private void iconMenuItem_OpenX_Click(object sender, EventArgs e)
        {
            m_bXMarking = true;
            update_PictureBoxView_Screen();
        }

        private void iconMenuItem_CloseX_Click(object sender, EventArgs e)
        {
            m_bXMarking = false;
            update_PictureBoxView_Screen();
        }

        private void iconMenuItem_OpenY_Click(object sender, EventArgs e)
        {
            m_bYMarking = true;
            update_PictureBoxView_Screen();
        }

        private void iconMenuItem_CloseY_Click(object sender, EventArgs e)
        {
            m_bYMarking = false;
            update_PictureBoxView_Screen();
        }
        /// <summary>
        /// 起始时间设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconButton_Start_Click(object sender, EventArgs e)
        {
            if (modWavMonitor.listOscDataProcess.Count == 0)
            {
                return;
            }
            try
            {
                Convert.ToDouble(textBox_MinX.Text);
                Convert.ToDouble(textBox_MaxX.Text);
            }
            catch
            {
                return;
            }
            double minValueXTemp = Convert.ToDouble(textBox_MinX.Text);
            double maxValueXTemp = Convert.ToDouble(textBox_MaxX.Text);
            if (minValueXTemp > maxValueXTemp)
            {
                return; 
            }
            if (minValueXTemp < modWavMonitor.listOscDataProcess[0].xOrigin)
            {
                MessageBox.Show(InterpretBase.textTran("超出范围"));
                return;
            }
            if (maxValueXTemp > modWavMonitor.listOscDataProcess[0].xOrigin + 1000000 * modWavMonitor.listOscDataProcess[0].xIncrement)
            {
                MessageBox.Show(InterpretBase.textTran("超出范围"));
                return;
            }

            point_StartPos = (int)((minValueXTemp - modWavMonitor.listOscDataProcess[0].xOrigin) / modWavMonitor.listOscDataProcess[0].xIncrement);
            point_StopPos = (int)((maxValueXTemp - modWavMonitor.listOscDataProcess[0].xOrigin) / modWavMonitor.listOscDataProcess[0].xIncrement);
            // 压缩时，至少有2k点，初始位置对齐到1000的整数倍
            if (point_StopPos - point_StartPos + 1 >= pointDisplay * 2)
            {
                int start = (int)Math.Floor(point_StartPos / (pointDisplay * 1.0));
                int stop = (int)Math.Ceiling(point_StopPos / (pointDisplay * 1.0));
                point_StopPos = stop * pointDisplay - 1;
                point_StartPos = start * pointDisplay;
            }
            // 大于1K，小于2K，从左侧取2K点
            else if ((point_StopPos - point_StartPos + 1 >= pointDisplay) && (point_StopPos - point_StartPos + 1 < pointDisplay * 2))
            {
                int start = (int)Math.Floor(point_StartPos / (pointDisplay * 1.0));
                point_StartPos = start * pointDisplay;
                //point_StartPos = point_StartPosTemp;
                point_StopPos = point_StartPos + pointDisplay * 2 - 1;
            }


            int pointNum = point_StopPos - point_StartPos + 1;
            if (point_StopPos <= point_StartPos + 9)
            {
                return;
            }
            int mutipleFin;
            if (pointNum < pointDisplay)
            {
                mutipleFin = modWavMonitor.findFinMutiple(pointDisplay, pointNum);
                if (mutipleFin > 200)
                {
                    //限制在200倍内插
                    mutipleFin = 200;
                }
                //显示当前的内插倍数
                label_InterMutiple.Text = "X" + mutipleFin.ToString();
            }
            else
            {
                mutipleFin = 1;
                //显示当前的内插倍数
                label_InterMutiple.Text = "X" + mutipleFin.ToString();
            }
            //波形放大插值操作
            bool bResult = modWavMonitor.calEnlargeWaveData_UsingCDLL(point_StartPos, point_StopPos, mutipleFin);
            //如果插值的话，需要重新计算结束位置
            if (mutipleFin != 1)
            {
                int numRange = (int)Math.Ceiling(pointDisplay / (mutipleFin * 1.0));
                point_StopPos = point_StartPos + numRange - 1;
            }
            //计算x轴的范围
            minValueX = modWavMonitor.listOscDataProcess[0].xOrigin + (point_StartPos) * modWavMonitor.listOscDataProcess[0].xIncrement;
            maxValueX = minValueX + (point_StopPos - point_StartPos + 1) * modWavMonitor.listOscDataProcess[0].xIncrement;
            foreach (Module_WaveMonitor.OscilloscopeDataProcess item in modWavMonitor.listOscDataProcess)
            {
                int index = listWaveSeriesShow.FindIndex(member => (member.strChanID == item.strChanID));
                if (index != -1)
                {
                    if (bResult)
                    {
                        listWaveSeriesShow[index].dispData.Clear();
                        for (int i = 0; i < item.dataDispFin.Length; i++)
                        {
                            PointF temPoint = new PointF();
                            temPoint.X = 0;
                            /*加上显示时的偏移*/
                            //temPoint.Y = (float)(item.dataDispFin[i] - item.vertOffset + (item.vertViewOffset - item.vertOffset));
                            temPoint.Y = (float)(item.dataDispFin[i] + (item.vertViewOffset - item.vertOffset));
                            listWaveSeriesShow[index].dispData.Add(temPoint);
                        }
                    }
                }

            }
            update_PictureBoxView_Screen();
        }
        /// <summary>
        /// 终止时间设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconButton_Stop_Click(object sender, EventArgs e)
        {
            if (modWavMonitor.listOscDataProcess.Count == 0)
            {
                return;
            }
            try
            {
                Convert.ToDouble(textBox_MinX.Text);
                Convert.ToDouble(textBox_MaxX.Text);
            }
            catch
            {
                return;
            }
            double minValueXTemp = Convert.ToDouble(textBox_MinX.Text);
            double maxValueXTemp = Convert.ToDouble(textBox_MaxX.Text);
            if (minValueXTemp > maxValueXTemp)
            {
                return;
            }
            if (minValueXTemp < modWavMonitor.listOscDataProcess[0].xOrigin)
            {
                MessageBox.Show(InterpretBase.textTran("超出范围"));
                return;
            }
            if (maxValueXTemp > modWavMonitor.listOscDataProcess[0].xOrigin + 1000000 * modWavMonitor.listOscDataProcess[0].xIncrement)
            {
                MessageBox.Show(InterpretBase.textTran("超出范围"));
                return;
            }
            //if (bEnlagerClick)
            {
                point_StartPos = (int)((minValueXTemp - modWavMonitor.listOscDataProcess[0].xOrigin) / modWavMonitor.listOscDataProcess[0].xIncrement);
                point_StopPos = (int)((maxValueXTemp - modWavMonitor.listOscDataProcess[0].xOrigin) / modWavMonitor.listOscDataProcess[0].xIncrement);
                // 压缩时，至少有2k点，初始位置对齐到1000的整数倍
                if (point_StopPos - point_StartPos + 1 >= pointDisplay * 2)
                {
                    int start = (int)Math.Floor(point_StartPos / (pointDisplay * 1.0));
                    int stop = (int)Math.Ceiling(point_StopPos / (pointDisplay * 1.0));
                    point_StopPos = stop * pointDisplay - 1;
                    point_StartPos = start * pointDisplay;
                }
                // 大于1K，小于2K，从左侧取2K点
                else if ((point_StopPos - point_StartPos + 1 >= pointDisplay) && (point_StopPos - point_StartPos + 1 < pointDisplay * 2))
                {
                    int start = (int)Math.Floor(point_StartPos / (pointDisplay * 1.0));
                    point_StartPos = start * pointDisplay;
                    //point_StartPos = point_StartPosTemp;
                    point_StopPos = point_StartPos + pointDisplay * 2 - 1;
                }
                int pointNum = point_StopPos - point_StartPos + 1;
                if (point_StopPos <= point_StartPos + 9)
                {
                    return;
                }
                int mutipleFin;
                if (pointNum < pointDisplay)
                {
                    mutipleFin = modWavMonitor.findFinMutiple(pointDisplay, pointNum);
                    if (mutipleFin > 200)
                    {
                        //限制在200倍内插
                        mutipleFin = 200;
                    }
                    //显示当前的内插倍数
                    label_InterMutiple.Text = "X" + mutipleFin.ToString();
                }
                else
                {
                    mutipleFin = 1;
                    //显示当前的内插倍数
                    label_InterMutiple.Text = "X" + mutipleFin.ToString();
                }
                //波形放大插值操作
                bool bResult = modWavMonitor.calEnlargeWaveData_UsingCDLL(point_StartPos, point_StopPos, mutipleFin);
                //如果插值的话，需要重新计算结束位置
                if (mutipleFin != 1)
                {
                    int numRange = (int)Math.Ceiling(pointDisplay / (mutipleFin * 1.0));
                    point_StopPos = point_StartPos + numRange - 1;
                }
                //计算x轴的范围
                minValueX = modWavMonitor.listOscDataProcess[0].xOrigin + (point_StartPos) * modWavMonitor.listOscDataProcess[0].xIncrement;
                maxValueX = minValueX + (point_StopPos - point_StartPos + 1) * modWavMonitor.listOscDataProcess[0].xIncrement;
                foreach (Module_WaveMonitor.OscilloscopeDataProcess item in modWavMonitor.listOscDataProcess)
                {
                    int index = listWaveSeriesShow.FindIndex(member => (member.strChanID == item.strChanID));
                    if (index != -1)
                    {
                        if (bResult)
                        {
                            listWaveSeriesShow[index].dispData.Clear();
                            for (int i = 0; i < item.dataDispFin.Length; i++)
                            {
                                PointF temPoint = new PointF();
                                temPoint.X = 0;
                                /*加上显示时的偏移*/
                                //temPoint.Y = (float)(item.dataDispFin[i] - item.vertOffset + (item.vertViewOffset - item.vertOffset));
                                temPoint.Y = (float)(item.dataDispFin[i] + (item.vertViewOffset - item.vertOffset));
                                listWaveSeriesShow[index].dispData.Add(temPoint);
                            }
                        }
                    }

                }
                update_PictureBoxView_Screen();
            }
        }
        #endregion

#if Debug

        /// <summary>
        /// 配置示波器
        /// </summary>
        /// <param name="strIP"></param>
        private void setOscPara(string strIP)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(strIP, 5555);
            string command = ":ULTRalab:SERVer 172.18.10.106\n";
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 200);
            socket.Send(Encoding.Default.GetBytes(command));
            Thread.Sleep(50);
            command = ":ULTRalab:SERVer?\n";
            socket.Send(Encoding.Default.GetBytes(command));
            byte[] byteReadBuf = new byte[128];
            var retCount = socket.Receive(byteReadBuf);
            string result = Encoding.Default.GetString(byteReadBuf, 0, retCount);
            command = ":WAVeform:CHANsend 15\n";
            socket.Send(Encoding.Default.GetBytes(command));
            Thread.Sleep(50);
            command = ":WAVeform:CHANsend?\n";
            socket.Send(Encoding.Default.GetBytes(command));
            byte[] byteReadBuf1 = new byte[128];
            var retCount1 = socket.Receive(byteReadBuf);
            string result1 = Encoding.Default.GetString(byteReadBuf, 0, retCount1);
        }
        /// <summary>
        /// 定义作为服务器端接受信息套接字
        /// </summary>
        public Socket socketReceive = null;
        /// <summary>
        /// 定义接受信息的IP地址和端口号
        /// </summary>
        public IPEndPoint ipReceive = null;
        /// <summary>
        /// 接收数据
        /// </summary>
        private void ReceiveNews()
        {
            string[] IP_Address = new string[10];
            int i = 0;
            //获取本机所有IP的值
            IPAddress[] ips = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
            //遍历获得的IP集以得到IPV4地址
            foreach (IPAddress ip in ips)
            {
                //筛选出IPV4地址
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    IP_Address[i] = ip.ToString();
                    i++;
                }
            }
            try
            {
                //初始化接受套接字：寻址方案，以字符流方式和Tcp通信
                socketReceive = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                //获取本机IP地址并设置接受信息的端口
                ipReceive = new IPEndPoint(IPAddress.Parse(IP_Address[0]), 11111);

                //将本机IP地址和接受端口绑定到接受套接字
                socketReceive.Bind(ipReceive);

                //监听端口，并设置监听缓存大小
                socketReceive.Listen(4096);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }

            //连续接受客户端发送过来的信息
            while (true)
            {
                //定义一个chat套接字用来接受信息
                Socket chat = socketReceive.Accept();

                //定义一个处理信息的对象
                ChatSession cs = new ChatSession(chat);

                //定义一个新的线程用来接受其他主机发送的信息
                Thread newThread = new Thread(new ThreadStart(cs.StartChat));
                //设置线程状态为单线程单元/在主线程中运行（如果不设置，无法弹出保存对话框）
                newThread.SetApartmentState(ApartmentState.STA);
                //启动新的线程
                newThread.Start();
            }
        }

        public class ChatSession
        {
            private static string fileName = string.Empty;
            private Socket chat;
            //private string pack;
            IPEndPoint ep;
            string strIP;
            private SpinLock spin = new SpinLock();//自旋锁
            public static int bComplete = 0;
            public byte[] byteReadBuffer = new byte[1000 * 1000 * 4 + 8 * 4 + 10];
            public class OscilloscopeDataMemory
            {
                public string strChanID = "";

                public string strPreAmple = "";

                public byte[] waveData = new byte[1000 * 1000 + 8];

                public int chanDelayTime = 0;//单位为ps-通道精细延时

                public int devDelayTime = 0;

                public int triggerTime = 0;//单位为ps
               
            }
            public static List<OscilloscopeDataMemory> m_listOscDataMemory = new List<OscilloscopeDataMemory>();
            /// <summary>
            /// 初始化构造方法
            /// </summary>
            /// <param name="chat">套接字</param>
            /// <param name="form">主窗体</param>
            public ChatSession(Socket chat)
            {
                this.chat = chat;

            }
            /// <summary>
            /// 找通道延迟的最小值
            /// </summary>
            /// <returns></returns>
            public static int findMinDevDelay()
            {
                int result = m_listOscDataMemory[0].devDelayTime;
                foreach (OscilloscopeDataMemory item in m_listOscDataMemory)
                {
                    if (result > item.devDelayTime)
                    {
                        result = item.devDelayTime;
                    }
                }
                return result;
            }
            /// <summary>
            /// 对信息进行处理
            /// </summary>
            public void StartChat()
            {
                //获取远程主机的IP地址和端口号
                ep = (IPEndPoint)chat.RemoteEndPoint;
                strIP = ep.ToString().Substring(0, ep.ToString().IndexOf(':'));

                //设置
                byte[] buff = new byte[4096];
                int len;
                int num = 0;
                while ((len = chat.Receive(buff)) != 0)
                {
                    Buffer.BlockCopy(buff, 0, byteReadBuffer, num, len);
                    num += len;
                }

                Debug.WriteLine(strIP + "已完成" + num.ToString() + "\r\n");
                /*停止仪器发送数据*/
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(strIP, 5555);
                string command = ":WAVeform:CHANsend 0\n";
                socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 200);
                socket.Send(Encoding.Default.GetBytes(command));
                command = ":WAVeform:PREamble?\n";
                socket.Send(Encoding.Default.GetBytes(command));
                byte[] byteReadBuf = new byte[128];
                var retCount = socket.Receive(byteReadBuf);
                string result1 = Encoding.Default.GetString(byteReadBuf, 0, retCount);
                result1 = result1.Remove(result1.Length - 1);
                command = ":CALibration:EXTDelay?\n";
                socket.Send(Encoding.Default.GetBytes(command));
                retCount = socket.Receive(byteReadBuf);
                string result2 = Encoding.Default.GetString(byteReadBuf, 0, retCount);
                result2 = result2.Remove(result2.Length - 1);
                socket.Close();

                /*提取通道精细延时*/
                byte[] timeInfo = new byte[10];
                Buffer.BlockCopy(byteReadBuffer, 1000 * 1000 * 4 + 8 * 4, timeInfo, 0, 10);
                bool lockTaken = false;
                if (strIP == "172.18.8.252")
                {
                    /*提取波形数据*/
                    for (int i = 0; i < 4; i++)
                    {
                        OscilloscopeDataMemory memoryDataTemp = new OscilloscopeDataMemory();
                        memoryDataTemp.strChanID = "Tag00" + (i + 1).ToString();
                        memoryDataTemp.strPreAmple = result1;
                        memoryDataTemp.chanDelayTime = timeInfo[i * 2] << 8 | timeInfo[i * 2 + 1];
                        memoryDataTemp.devDelayTime = Convert.ToInt32(result2);
                        memoryDataTemp.triggerTime = timeInfo[8] << 8 | timeInfo[9];
                        Buffer.BlockCopy(byteReadBuffer, 0 + i * (1000 * 1000 + 8), memoryDataTemp.waveData, 0, 1000 * 1000 + 8);
                        try
                        {
                            spin.Enter(ref lockTaken);
                            m_listOscDataMemory.Add(memoryDataTemp);
                            spin.Exit();
                            lockTaken = false;
                        }
                        finally
                        {
                            if (lockTaken)
                            {
                                spin.Exit();
                            }
                        }

                    }
                }
                else if (strIP == "172.18.8.253")
                {
                    /*提取波形数据-多存一组*/
                    for (int i = 0; i < 4; i++)
                    {
                        OscilloscopeDataMemory memoryDataTemp = new OscilloscopeDataMemory();
                        memoryDataTemp.strChanID = "Tag00" + (i + 5).ToString();
                        memoryDataTemp.strPreAmple = result1;
                        memoryDataTemp.chanDelayTime = timeInfo[i * 2] << 8 | timeInfo[i * 2 + 1];
                        memoryDataTemp.devDelayTime = Convert.ToInt32(result2);
                        memoryDataTemp.triggerTime = timeInfo[8] << 8 | timeInfo[9];
                        Buffer.BlockCopy(byteReadBuffer, 0 + i * (1000 * 1000 + 8), memoryDataTemp.waveData, 0, 1000 * 1000 + 8);
                        try
                        {
                            spin.Enter(ref lockTaken);
                            m_listOscDataMemory.Add(memoryDataTemp);
                            spin.Exit();
                            lockTaken = false;
                        }
                        finally
                        {
                            if (lockTaken)
                            {
                                spin.Exit();
                            }
                        }
                    }
                }
                chat.Close();

                bComplete++;
            }
        }











#endif

    }
}
