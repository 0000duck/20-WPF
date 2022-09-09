using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClassLibrary_MultiLanguage;
using saker_Winform.CommonBaseModule;
using saker_Winform.Global;
using saker_Winform.Module;
using saker_Winform.UserControls;
/*****************************************************************************************************************
                                        普源精电科技有限公司版权所有(2020-)
******************************************************************************************************************
类名:      UCChanWaveView
功能描述： UCChanWaveView类实现用户控件的相关功能
作 者：    sn02736
版 本：    00.01.00.00
修改历史： 
<作者> <修改时间> <版本> <修改描述>
 sn02736      2020.7.31    初次修改
*****************************************************************************************************************/
namespace saker_Winform.SubForm
{
    public partial class Form_HistoryComp : Form
    {
        #region Fields
        private string strFormNum;// 表示当前是哪个窗体
        /// <summary>
        /// 绘图区域的画板宽度
        /// </summary>
        private float boardWidth;
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
        private bool bXMarking = true;
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
        #region 波形放大操作相关
        private bool bEnlagerClick = false;//波形放大标志
        bool bDrawStart = false;
        Point pointStart = Point.Empty;
        Point pointContinue = Point.Empty;
        private int point_StartPos = 0;//起始点
        private int point_StopPos = 0;//终止点
        private int totalPointNum = 0;//所有点
        #endregion
        bool bNormal = false; // 是否已经点击归一化
        bool bAglin = false;  // 是否已经点击对齐
        //定义屏幕显示的总点数
        private const int pointDisplay = 2000;
        // 定义滚动显示时的等分数
        private const int drawRangeDiv = 50;
        /// <summary>
        /// 显示用数据管理
        /// </summary>
        public class Series
        {
            public string strDispID;//显示标记
            public string strChanMeasType;//通道测量类型
            public string strDevName;//设备别名
            public int chanActual;// 实际的通道编号-》机器上的实际通道
            public string strTestTim; //测试时间
            private Color lineColor = Color.Red;//当前画笔颜色
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
        public List<UCHistoryComp> listHistWaveView = new List<UCHistoryComp>();
        /*定义显示用的数据*/
        private List<Series> listHistWaveSeriesShow = new List<Series>();
        /*定义数据管理类*/
        public Module_HistoryComp modHistComp = new Module_HistoryComp();

        private DataTable dtHistMeasInfo = new DataTable("Table_MeasInfoView");//显示测量结果表 

        public delegate void waveView_Refresh();//申明波形显示的委托
        public delegate void updateChanLabelFlowPanel();

        #endregion

        #region Constructuion
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="strTemp"></param>
        /// <param name="strName"></param>
        public Form_HistoryComp(string strTemp, string strName)
        {
            InitializeComponent();
            //规定画布的大小
            boardWidth = pictureBox_Wave.Width;
            boardHeight = pictureBox_Wave.Height;
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
        #region 订阅自定义事件
        /// <summary>
        /// 订阅事件
        /// </summary>
        /// <param name="evenSource"></param>
        public void Subscribe(UCHistoryComp evenSource)
        {

            evenSource.chanWaveViewEvent += new UCHistoryComp.chanWaveViewEventHandler(chanHistWaveView_KeyPressed);

        }
        /// <summary>
        /// 取消订阅事件
        /// </summary>
        /// <param name="evenSource"></param>
        public void UnSubscribe(UCHistoryComp evenSource)
        {

            evenSource.chanWaveViewEvent -= new UCHistoryComp.chanWaveViewEventHandler(chanHistWaveView_KeyPressed);

        }
        /// <summary>
        /// 通道图标点击响应事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void chanHistWaveView_KeyPressed(object sender, UCHistoryComp.chanWaveViewEventArgs e)
        {
            string str = e.KeyToRaiseEvent;
            if (str.Contains("CheckBox"))
            {
                string[] arr = str.Split(';');
                string strID = arr[0];//获取通道编号
                string strTime = arr[1];//获取测试时间
                if (str.Contains("False"))
                {
                    foreach (UCHistoryComp item in flowLayoutPanel_LabelItem.Controls)
                    {
                        if ((item.m_strChanID == strID)&&(item.m_strTestTim == strTime))
                        {
                            if (item.m_selectColor == ucChanLabel_Tag.m_labelColor)
                            {
                                ucChanLabel_Tag.Visible = false;
                            }
                        }
                    }
                }
                update_PictureBoxView_Screen();
            }
            else
            {
                ucChanLabel_Tag.Visible = true;
                string[] arr = str.Split(';');
                foreach (UCHistoryComp item in flowLayoutPanel_LabelItem.Controls)
                {
                    if ((item.m_strChanID == arr[0])&&(item.m_strTestTim == arr[1]))
                    {
                        item.label_ChanID_Set(true);
                        panel_Label.Controls.Clear();
                        panel_Label.Controls.Add(ucChanLabel_Tag);
                        ucChanLabel_Tag.setLabel(arr[0], item.m_selectColor);
                        int index = modHistComp.listHistDataProcess.FindIndex(ex => (ex.strTag == item.m_strChanID) && (ex.strTestTim == item.m_strTestTim));
                        if (index != -1)
                        {
                            float offset = (float)modHistComp.listHistDataProcess[index].vertViewOffset;
                            float scale = maxValueY - minValueY;
                            float ampPrePoint = scale / panel_Label.Height;
                            int pos = (int)(offset / ampPrePoint);
                            ucChanLabel_Tag.Location = new Point(ucChanLabel_Tag.Location.X, panel_Label.Height / 2 - ucChanLabel_Tag.Height / 2 - pos);
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
        /// 波形放大操作
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
        /// 归一化显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconToolStripButton_normalization_Click(object sender, EventArgs e)
        {
            if (modHistComp.listHistDataProcess.Count == 0)
            {
                return;
            }
            // 显示归一化计算
            modHistComp.normalWaveData();
            // 更新显示
            foreach (Module_HistoryComp.HistoryDataProcess item in modHistComp.listHistDataProcess)
            {
                int index = listHistWaveSeriesShow.FindIndex(member => ((member.strDispID == item.strTag) && (member.strTestTim == item.strTestTim)));
                if (index != -1)
                {
                    listHistWaveSeriesShow[index].dispData.Clear();
                    for (int i = 0; i < item.dataDispFin.Length; i++)
                    {
                        PointF temPoint = new PointF();
                        temPoint.X = 0;
                        /*加上显示时的偏移*/
                        temPoint.Y = (float)(item.dataDispFin[i] + item.vertViewOffset);
                        listHistWaveSeriesShow[index].dispData.Add(temPoint);
                    }
                }

            }
            //委托方式刷新
            update_PictureBoxView_Screen();
            bNormal = true;
            System.Windows.Forms.MessageBox.Show(InterpretBase.textTran("归一化完成！"), InterpretBase.textTran("消息"), MessageBoxButtons.OK, MessageBoxIcon.Information);

        }
        /// <summary>
        /// 半峰高对齐显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconToolStripButton_Align_Click(object sender, EventArgs e)
        {
            if (modHistComp.listHistDataProcess.Count == 0)
            {
                return;
            }
            // 初始直接绘制
            if ((point_StartPos == 0) && (point_StopPos == 0))
            {
                return;
            }
            // 计算对齐后的数据
            bool bState = modHistComp.aglinWaveData(point_StartPos, point_StopPos);
            // 对齐成功，绘制波形
            if (bState)
            {
                foreach (Module_HistoryComp.HistoryDataProcess item in modHistComp.listHistDataProcess)
                {
                    int index = listHistWaveSeriesShow.FindIndex(member => ((member.strDispID == item.strTag) && (member.strTestTim == item.strTestTim)));
                    if (index != -1)
                    {
                        listHistWaveSeriesShow[index].dispData.Clear();
                        for (int i = 0; i < item.dataDispFin.Length; i++)
                        {
                            PointF temPoint = new PointF();
                            temPoint.X = 0;
                            /*加上显示时的偏移*/
                            temPoint.Y = (float)(item.dataDispFin[i] + item.vertViewOffset);
                            listHistWaveSeriesShow[index].dispData.Add(temPoint);
                        }
                    }

                }
                if (bNormal)
                {
                    // 已经点击过归一化操作，对齐后还需要重新归一化一下
                    modHistComp.normalWaveData();
                    // 更新显示
                    foreach (Module_HistoryComp.HistoryDataProcess item in modHistComp.listHistDataProcess)
                    {
                        int index = listHistWaveSeriesShow.FindIndex(member => ((member.strDispID == item.strTag) && (member.strTestTim == item.strTestTim)));
                        if (index != -1)
                        {
                            listHistWaveSeriesShow[index].dispData.Clear();
                            for (int i = 0; i < item.dataDispFin.Length; i++)
                            {
                                PointF temPoint = new PointF();
                                temPoint.X = 0;
                                /*加上显示时的偏移*/
                                temPoint.Y = (float)(item.dataDispFin[i] + item.vertViewOffset);
                                listHistWaveSeriesShow[index].dispData.Add(temPoint);
                            }
                        }
                    }
                    bNormal = false;
                }
                //委托方式刷新
                update_PictureBoxView_Screen();
                //else
                //{
                //    //委托方式刷新
                //    update_PictureBoxView_Screen();
                //}
                System.Windows.Forms.MessageBox.Show(InterpretBase.textTran("半峰高对齐完成！"), InterpretBase.textTran("消息"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show(InterpretBase.textTran("半峰高对齐错误！"), InterpretBase.textTran("错误"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// 打开测量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconToolStripButton_Meas_Click(object sender, EventArgs e)
        {
            dataGridView_MeasInfo.ReadOnly = true;
            if (panel_Meas.Visible == true)
            {
                panel_Meas.Visible = false;
            }
            else
            {
                panel_Meas.Visible = true;
                // 测量计算
                bool bResult = modHistComp.getHalfPeakWidth();
                if (bResult)
                {
                    dtHistMeasInfo.Rows.Clear();
                    dataGridView_MeasInfo.Columns.Clear();
                    for (int i = 0; i < modHistComp.listHistDataProcess.Count; i++)
                    {
                        dtHistMeasInfo.Rows.Add(modHistComp.listHistDataProcess[i].strTag,
                            modHistComp.listHistDataProcess[i].strMeasType,
                            modHistComp.listHistDataProcess[i].strDevName,
                            "CHAN" + modHistComp.listHistDataProcess[i].chanID.ToString(),
                            modHistComp.listHistDataProcess[i].strTestTim,
                            CValue2String.time2String(modHistComp.listHistDataProcess[i].wfmHalfPeakWidth));
                    }
                    dataGridView_MeasInfo.DataSource = dtHistMeasInfo.Copy();
                }
            }

        }
        /// <summary>
        /// load事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_HistoryComp_Load(object sender, EventArgs e)
        {
            // 订阅通道标签的事件
            iconButton_DockRight.Visible = false;
            iconButton_DockLeft.Visible = true;
            panel_Meas.Visible = false;

            flowLayoutPanel_LabelItem.AutoScroll = false;
            flowLayoutPanel_LabelItem.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel_LabelItem.WrapContents = false;
            flowLayoutPanel_LabelItem.HorizontalScroll.Maximum = 0;
            flowLayoutPanel_LabelItem.AutoScrollMargin = new System.Drawing.Size(10, flowLayoutPanel_LabelItem.AutoScrollMargin.Height);
            flowLayoutPanel_LabelItem.AutoScroll = true;

            ucChanLabel_Tag.Visible = false;

            label_InterMutiple.Text = InterpretBase.textTran("内插倍数:")+"X1";

            dataGridView_MeasInfo.AllowUserToAddRows = false;//禁用最后一行空白
            dataGridView_MeasInfo.AllowUserToResizeRows = false;//禁拖动行高度
            dataGridView_MeasInfo.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;//禁止最左侧空白列宽度
            dataGridView_MeasInfo.AllowUserToDeleteRows = false;

            dtHistMeasInfo.Columns.Add(InterpretBase.textTran("通道标记"), typeof(String));
            dtHistMeasInfo.Columns.Add(InterpretBase.textTran("测量类型"), typeof(String));
            dtHistMeasInfo.Columns.Add(InterpretBase.textTran("设备别名"), typeof(String));
            dtHistMeasInfo.Columns.Add(InterpretBase.textTran("实际通道"), typeof(String));
            dtHistMeasInfo.Columns.Add(InterpretBase.textTran("采集时间"), typeof(String));
            dtHistMeasInfo.Columns.Add(InterpretBase.textTran("半峰宽[s]"), typeof(String));
        }
        /// <summary>
        /// 向左dock
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconButton_DockLeft_Click(object sender, EventArgs e)
        {
            panel_Itemlabel.Width = 99;
            this.iconButton_DockRight.Visible = true;
            this.iconButton_DockLeft.Visible = false;
            foreach (UCHistoryComp item in flowLayoutPanel_LabelItem.Controls)
            {
                item.setBriefViewMode();
            }
        }
        /// <summary>
        /// 向右dock
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconButton_DockRight_Click(object sender, EventArgs e)
        {
            panel_Itemlabel.Width = 270;
            this.iconButton_DockRight.Visible = false;
            this.iconButton_DockLeft.Visible = true;
            foreach (UCHistoryComp item in flowLayoutPanel_LabelItem.Controls)
            {
                item.setDetialViewMode();
            }
        }
        /// <summary>
        /// 顶层pictureBox绘制事件
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
        /// 双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox_DrawRect_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (bEnlagerClick)
            {
                point_StartPos = 0;//起始点
                point_StopPos = 0;//终止点
                totalPointNum = 0;//所有点
                if (modHistComp.listHistDataProcess.Count != 0)
                {
                    minValueX = modHistComp.listHistDataProcess[0].xOrigin;
                    maxValueX = minValueX + (modHistComp.listHistDataProcess[0].memDepth) * modHistComp.listHistDataProcess[0].xIncrement;
                }
                foreach (Module_HistoryComp.HistoryDataProcess item in modHistComp.listHistDataProcess)
                {
                    item.vertViewOffset = 0;
                }
                /*更新垂直偏移*/
                foreach (UCHistoryComp item in listHistWaveView)
                {
                    int index_Origl = modHistComp.listHistDataProcess.FindIndex(ex => (ex.strTag == item.m_strChanID) && (ex.strTestTim == item.m_strTestTim));

                    if (item.m_bSelect)
                    {
                        float scale = maxValueY - minValueY;
                        float ampPrePoint = scale / panel_Label.Height;
                        int pos = (int)(modHistComp.listHistDataProcess[index_Origl].vertViewOffset / ampPrePoint);
                        ucChanLabel_Tag.Location = new Point(ucChanLabel_Tag.Location.X, panel_Label.Height / 2 - ucChanLabel_Tag.Height / 2 - pos);
                    }
                }

                label_InterMutiple.Text = InterpretBase.textTran("内插倍数:")+"X1";
                foreach (Module_HistoryComp.HistoryDataProcess item in modHistComp.listHistDataProcess)
                {
                    int index = listHistWaveSeriesShow.FindIndex(member => (member.strDispID == item.strTag) && (member.strTestTim == item.strTestTim));
                    if (index != -1)
                    {
                        listHistWaveSeriesShow[index].dispData.Clear();
                        for (int i = 0; i < item.dataDispRough.Length; i++)
                        {
                            item.dataDispFin[i] = item.dataDispRough[i];
                            PointF temPoint = new PointF();
                            temPoint.X = 0;
                            /*加上显示时的偏移*/
                            temPoint.Y = (float)(item.dataDispRough[i]);
                            listHistWaveSeriesShow[index].dispData.Add(temPoint);
                        }
                    }
                }
                update_PictureBoxView_Screen();
                bNormal = false;
            }
        }
        /// <summary>
        /// 鼠标按下事件
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
        }
        /// <summary>
        /// 鼠标移动事件
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
        /// 鼠标抬起事件
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
            if (modHistComp.listHistDataProcess.Count == 0)
            {
                return;
            }
            /*计算初始点和终止点------只有内存数据支持放大*/
            if (modHistComp.listHistDataProcess[0].memDepth != 0)
            {
                if (totalPointNum == 0)
                {
                    totalPointNum = modHistComp.listHistDataProcess[0].memDepth;
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
            if (point_StopPosTemp > modHistComp.listHistDataProcess[0].memDepth)
            {
                point_StopPosTemp = modHistComp.listHistDataProcess[0].memDepth - 1;
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
                    mutipleFin = modHistComp.findFinMutiple(pointDisplay, pointNum);
                    if (mutipleFin > 200)
                    {
                        //限制在200倍内插
                        mutipleFin = 200;
                    }
                    //显示当前的内插倍数
                    label_InterMutiple.Text = InterpretBase.textTran("内插倍数:X") + mutipleFin.ToString();
                }
                else
                {
                    mutipleFin = 1;
                    //显示当前的内插倍数
                    label_InterMutiple.Text = InterpretBase.textTran("内插倍数:X") + mutipleFin.ToString();
                }
                //波形放大插值操作
                bool bResult = modHistComp.calEnlargeWaveData_UsingCDLL(point_StartPos, point_StopPos, mutipleFin);
                //如果插值的话，需要重新计算结束位置
                if (mutipleFin != 1)
                {
                    int numRange = (int)Math.Ceiling(pointDisplay  / (mutipleFin * 1.0));
                    point_StopPos = point_StartPos + numRange - 1;
                }
                //计算x轴的范围（默认所有的采样都是相同的采样率和时基）
                minValueX = modHistComp.listHistDataProcess[0].xOrigin + (point_StartPos) * modHistComp.listHistDataProcess[0].xIncrement;
                maxValueX = minValueX + (point_StopPos - point_StartPos + 1) * modHistComp.listHistDataProcess[0].xIncrement;
                foreach (Module_HistoryComp.HistoryDataProcess item in modHistComp.listHistDataProcess)
                {
                    int index = listHistWaveSeriesShow.FindIndex(member => ((member.strDispID == item.strTag) && (member.strTestTim == item.strTestTim)));
                    if (index != -1)
                    {
                        if (bResult)
                        {
                            listHistWaveSeriesShow[index].dispData.Clear();
                            for (int i = 0; i < item.dataDispFin.Length; i++)
                            {
                                PointF temPoint = new PointF();
                                temPoint.X = 0;
                                /*加上显示时的偏移*/
                                temPoint.Y = (float)(item.dataDispFin[i] + item.vertViewOffset);
                                listHistWaveSeriesShow[index].dispData.Add(temPoint);
                            }
                        }
                    }

                }
                update_PictureBoxView_Screen();
                bNormal = false;
            }
            pointStart = Point.Empty;
            pointContinue = Point.Empty;
        }
        /// <summary>
        /// 通道标签键按下
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
        /// 通道标签健移动
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
                if (ucChanLabel_Tag.Location.Y >= this.panel_Label.Height - ucChanLabel_Tag.Height)
                {
                    ucChanLabel_Tag.Location = new Point(ucChanLabel_Tag.Location.X, this.panel_Label.Height - ucChanLabel_Tag.Height);
                }
            }
        }
        /// <summary>
        /// 通道标签健弹起
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ucChanLabel_Tag_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isChanLabelMouseDown = false;
                /*更新垂直偏移*/
                foreach (UCHistoryComp item in listHistWaveView)
                {
                    if (item.m_bSelect)
                    {
                        int index_Origl = modHistComp.listHistDataProcess.FindIndex(ex => (ex.strTag == item.m_strChanID) && (ex.strTestTim == item.m_strTestTim));
                        if (index_Origl != -1)
                        {
                            float scale = maxValueY - minValueY;
                            float ampPrePoint = scale / panel_Label.Height;
                            int pos = (int)(modHistComp.listHistDataProcess[index_Origl].vertViewOffset / ampPrePoint);
                            float offsetAmp = ampPrePoint * (ucChanLabel_Tag.Location.Y - (panel_Label.Height / 2 - ucChanLabel_Tag.Height / 2 - pos));
                            int index_Show = listHistWaveSeriesShow.FindIndex(ex_show => (ex_show.strDispID == item.m_strChanID) && (ex_show.strTestTim == item.m_strTestTim));
                            if (index_Show != -1)
                            {
                                /*更新显示的偏移值*/
                                modHistComp.listHistDataProcess[index_Origl].vertViewOffset = modHistComp.listHistDataProcess[index_Origl].vertViewOffset - offsetAmp;
                                for (int i = 0; i < listHistWaveSeriesShow[index_Show].dispData.Count; i++)
                                {
                                    listHistWaveSeriesShow[index_Show].dispData[i] = new System.Drawing.PointF(listHistWaveSeriesShow[index_Show].dispData[i].X,
                                        listHistWaveSeriesShow[index_Show].dispData[i].Y - offsetAmp);
                                    
                                }
                            }
                        }
                    }
                }
                update_PictureBoxView_Screen();
            }
        }
        /// <summary>
        /// pictureBox尺寸改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox_Wave_ClientSizeChanged(object sender, EventArgs e)
        {
            InitCanvas();
            update_PictureBoxView_Screen();
        }
        /// <summary>
        /// 自动编号，与数据无关
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_MeasInfo_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
                e.RowBounds.Location.Y,
                dataGridView_MeasInfo.RowHeadersWidth - 4,
                e.RowBounds.Height);
            TextRenderer.DrawText(e.Graphics,
                  (e.RowIndex + 1).ToString(),
                   dataGridView_MeasInfo.RowHeadersDefaultCellStyle.Font,
                   rectangle,
                   dataGridView_MeasInfo.RowHeadersDefaultCellStyle.ForeColor,
                   TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
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
        /// textBox的输入限制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox_Min_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < 48 || e.KeyChar > 57) && (e.KeyChar != 8) && (e.KeyChar != 46) && (e.KeyChar != 45))
                e.Handled = true;
        }
        /// <summary>
        /// textBox的输入限制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox_Max_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < 48 || e.KeyChar > 57) && (e.KeyChar != 8) && (e.KeyChar != 46) && (e.KeyChar != 45))
                e.Handled = true;
        }
        /// <summary>
        /// 右击事件-反射对齐按键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconMenuItem_Aglin_Click(object sender, EventArgs e)
        {
            CReflection.callObjectEvent(this.iconToolStripButton_Align, "OnClick", e);
        }
        /// <summary>
        /// 右击事件-反射归一化按键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconMenuItem_Normal_Click(object sender, EventArgs e)
        {
            CReflection.callObjectEvent(this.iconToolStripButton_Normalization, "OnClick", e);
        }
        /// <summary>
        /// 加载历史对比数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static object objlock = new object();
        private void iconToolStripButton_Load_Click(object sender, EventArgs e)
        {
            Form_DataBaseSource formDataBase = new Form_DataBaseSource();
            formDataBase.HidePaintButton(true);
            formDataBase.ShowDialog();
            if(!formDataBase.m_bPaint)
            {
                formDataBase.m_bPaint = false;
                return;
            }
            if (formDataBase.WaveInfo.Count == 0)
            {
                return;
            }
            Form_Progressbar form_Progressbar = new Form_Progressbar();
            /* 开启进度条 */
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            cancelTokenSource.Token.Register(() =>
            {
                form_Progressbar.CloseProcess();//委托方法调用进度条关闭                
                DialogResult res = MessageBox.Show(InterpretBase.textTran("绘制已完成"), InterpretBase.textTran("绘制情况"), MessageBoxButtons.OK);
                if (res == DialogResult.OK)
                {
                    return;
                }
            });

            Task.Run(() =>
            {
                form_Progressbar.ProcessMarquee(InterpretBase.textTran("绘制中..."));//设置进度条显示为左右转动
                form_Progressbar.StartPosition = FormStartPosition.CenterScreen;//程序中间
                form_Progressbar.ShowDialog();
                //form_Process.Show();
            }, cancelTokenSource.Token);
            // 先清除现有项
            foreach (UCHistoryComp item in listHistWaveView)
            {
                UnSubscribe(item);
            }
            listHistWaveView.Clear();
            // 先添加左侧的标签栏
            for (int i = 0; i < formDataBase.WaveInfo.Count; i++)
            {
                Color colorTemp = CGlobalColor.lineColor[i % 15];
                UCHistoryComp ucTemp = new UCHistoryComp(colorTemp,
                    formDataBase.WaveInfo[i].Tag,
                    formDataBase.WaveInfo[i].MeasureType,
                    formDataBase.WaveInfo[i].DeviceName,
                    formDataBase.WaveInfo[i].ChannelID.ToString(),
                    formDataBase.WaveInfo[i].StartTime);
                listHistWaveView.Add(ucTemp);
            }
            update_ChanLabelFlowPanel();
            foreach (UCHistoryComp item in listHistWaveView)
            {
                item.label_ChanID_Set(false);
            }
            // 先获取所有设备延时的最小值
            modHistComp.devDelayMin = formDataBase.WaveInfo.Min(t => t.DeviceDelayTime);
            // 清除已有数据
            modHistComp.listHistDataProcess.Clear();
            // 加载波形数据
            foreach (Module_WaveInfo item in formDataBase.WaveInfo)
            {
                lock (objlock)
                {
                    modHistComp.modHistoryComp_Load(item);
                }
            }
            update_PictureBoxView_Data();
            if (panel_Meas.Visible == true)
            {
                panel_Meas.Visible = false;
                CReflection.callObjectEvent(this.iconToolStripButton_Meas, "OnClick", null);
            }
            cancelTokenSource.Cancel();
        }
        /// <summary>
        /// 当前选中波形垂直偏移返回初始位置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconSplitButton_Initial_ButtonClick(object sender, EventArgs e)
        {
            /*更新垂直偏移*/
            foreach (UCHistoryComp item in listHistWaveView)
            {
                if (item.m_bSelect)
                {
                    int index_Origl = modHistComp.listHistDataProcess.FindIndex(ex => (ex.strTag == item.m_strChanID) && (ex.strTestTim == item.m_strTestTim));
                    float scale = maxValueY - minValueY;
                    //恢复初始偏移

                    float ampPrePoint = scale / panel_Label.Height;
                    int pos = (int)(0 / ampPrePoint);
                    ucChanLabel_Tag.Location = new Point(ucChanLabel_Tag.Location.X, panel_Label.Height / 2 - ucChanLabel_Tag.Height / 2 - pos);
                    int index_Show = listHistWaveSeriesShow.FindIndex(ex_show => (ex_show.strDispID == item.m_strChanID) && (ex_show.strTestTim == item.m_strTestTim));
                    /*更新显示的偏移值*/
                    for (int i = 0; i < listHistWaveSeriesShow[index_Show].dispData.Count; i++)
                    {
                        listHistWaveSeriesShow[index_Show].dispData[i] = new System.Drawing.PointF(listHistWaveSeriesShow[index_Show].dispData[i].X,
                            (float)(listHistWaveSeriesShow[index_Show].dispData[i].Y - modHistComp.listHistDataProcess[index_Origl].vertViewOffset));
                    }
                    //更新偏移
                    modHistComp.listHistDataProcess[index_Origl].vertViewOffset = 0;
                }
            }
            update_PictureBoxView_Screen();
        }
        /// <summary>
        /// 当前选中波形垂直偏移返回初始位置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconMenuItem_Single_Click(object sender, EventArgs e)
        {
            CReflection.callObjectEvent(this.iconSplitButton_Initial, "OnClick", e);
        }
        /// <summary>
        /// 所有波形垂直偏移返回初始位置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconMenuItem_All_Click(object sender, EventArgs e)
        {
            /*更新垂直偏移*/
            foreach (UCHistoryComp item in listHistWaveView)
            {
                int index_Origl = modHistComp.listHistDataProcess.FindIndex(ex => (ex.strTag == item.m_strChanID) && (ex.strTestTim == item.m_strTestTim));

                if (item.m_bSelect)
                {
                    float scale = maxValueY - minValueY;
                    float ampPrePoint = scale / panel_Label.Height;
                    int pos = (int)(0 / ampPrePoint);
                    ucChanLabel_Tag.Location = new Point(ucChanLabel_Tag.Location.X, panel_Label.Height / 2 - ucChanLabel_Tag.Height / 2 - pos);
                }
                int index_Show = listHistWaveSeriesShow.FindIndex(ex_show => (ex_show.strDispID == item.m_strChanID) && (ex_show.strTestTim == item.m_strTestTim));

                for (int i = 0; i < listHistWaveSeriesShow[index_Show].dispData.Count; i++)
                {
                    listHistWaveSeriesShow[index_Show].dispData[i] = new System.Drawing.PointF(listHistWaveSeriesShow[index_Show].dispData[i].X,
                        (float)(listHistWaveSeriesShow[index_Show].dispData[i].Y - modHistComp.listHistDataProcess[index_Origl].vertViewOffset));
                }
                //更新偏移
                modHistComp.listHistDataProcess[index_Origl].vertViewOffset = 0;
            }
            update_PictureBoxView_Screen();
        }

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
            bYMarking = true;
            update_PictureBoxView_Screen();
        }

        private void iconMenuItem_CloseY_Click(object sender, EventArgs e)
        {
            bYMarking = false;
            update_PictureBoxView_Screen();
        }
        /// <summary>
        /// 波形左移操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconButton_Left_Click(object sender, EventArgs e)
        {
            if ((point_StartPos == 0)&&(point_StopPos == 0))
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
                    mutipleFin = modHistComp.findFinMutiple(pointDisplay, pointNum);
                    if (mutipleFin > 200)
                    {
                        //限制在200倍内插
                        mutipleFin = 200;
                    }
                    //显示当前的内插倍数
                    label_InterMutiple.Text = InterpretBase.textTran("内插倍数:X") + mutipleFin.ToString();
                }
                else
                {
                    mutipleFin = 1;
                    //显示当前的内插倍数
                    label_InterMutiple.Text = InterpretBase.textTran("内插倍数:X") + mutipleFin.ToString();
                }
                //波形放大插值操作
                bool bResult = modHistComp.calEnlargeWaveData_UsingCDLL(point_StartPos, point_StopPos, mutipleFin);
                //如果插值的话，需要重新计算结束位置
                if (mutipleFin != 1)
                {
                    int numRange = (int)Math.Ceiling(pointDisplay / (mutipleFin * 1.0));
                    point_StopPos = point_StartPos + numRange - 1;
                }
                //计算x轴的范围（默认所有的采样都是相同的采样率和时基）
                minValueX = modHistComp.listHistDataProcess[0].xOrigin + (point_StartPos) * modHistComp.listHistDataProcess[0].xIncrement;
                maxValueX = minValueX + (point_StopPos - point_StartPos + 1) * modHistComp.listHistDataProcess[0].xIncrement;
                foreach (Module_HistoryComp.HistoryDataProcess item in modHistComp.listHistDataProcess)
                {
                    int index = listHistWaveSeriesShow.FindIndex(member => ((member.strDispID == item.strTag) && (member.strTestTim == item.strTestTim)));
                    if (index != -1)
                    {
                        if (bResult)
                        {
                            listHistWaveSeriesShow[index].dispData.Clear();
                            for (int i = 0; i < item.dataDispFin.Length; i++)
                            {
                                PointF temPoint = new PointF();
                                temPoint.X = 0;
                                /*加上显示时的偏移*/
                                temPoint.Y = (float)(item.dataDispFin[i] + item.vertViewOffset);
                                listHistWaveSeriesShow[index].dispData.Add(temPoint);
                            }
                        }
                    }

                }
                update_PictureBoxView_Screen();
                bNormal = false;
            }
        }
        /// <summary>
        /// 波形右移操作
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
                    mutipleFin = modHistComp.findFinMutiple(pointDisplay, pointNum);
                    if (mutipleFin > 200)
                    {
                        //限制在200倍内插
                        mutipleFin = 200;
                    }
                    //显示当前的内插倍数
                    label_InterMutiple.Text = InterpretBase.textTran("内插倍数:X") + mutipleFin.ToString();
                }
                else
                {
                    mutipleFin = 1;
                    //显示当前的内插倍数
                    label_InterMutiple.Text = InterpretBase.textTran("内插倍数:X") + mutipleFin.ToString();
                }
                //波形放大插值操作
                bool bResult = modHistComp.calEnlargeWaveData_UsingCDLL(point_StartPos, point_StopPos, mutipleFin);
                //如果插值的话，需要重新计算结束位置
                if (mutipleFin != 1)
                {
                    int numRange = (int)Math.Ceiling(pointDisplay / (mutipleFin * 1.0));
                    point_StopPos = point_StartPos + numRange - 1;
                }
                //计算x轴的范围（默认所有的采样都是相同的采样率和时基）
                minValueX = modHistComp.listHistDataProcess[0].xOrigin + (point_StartPos) * modHistComp.listHistDataProcess[0].xIncrement;
                maxValueX = minValueX + (point_StopPos - point_StartPos + 1) * modHistComp.listHistDataProcess[0].xIncrement;
                foreach (Module_HistoryComp.HistoryDataProcess item in modHistComp.listHistDataProcess)
                {
                    int index = listHistWaveSeriesShow.FindIndex(member => ((member.strDispID == item.strTag) && (member.strTestTim == item.strTestTim)));
                    if (index != -1)
                    {
                        if (bResult)
                        {
                            listHistWaveSeriesShow[index].dispData.Clear();
                            for (int i = 0; i < item.dataDispFin.Length; i++)
                            {
                                PointF temPoint = new PointF();
                                temPoint.X = 0;
                                /*加上显示时的偏移*/
                                temPoint.Y = (float)(item.dataDispFin[i] + item.vertViewOffset);
                                listHistWaveSeriesShow[index].dispData.Add(temPoint);
                            }
                        }
                    }

                }
                update_PictureBoxView_Screen();
                bNormal = false;
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
        /// 更新波形显示数据
        /// </summary>
        public void update_PictureBoxView_Data()
        {
            listHistWaveSeriesShow.Clear();
            if (modHistComp.listHistDataProcess.Count != 0)
            {
                minValueX = modHistComp.listHistDataProcess[0].xOrigin;
                maxValueX = minValueX + (modHistComp.listHistDataProcess[0].memDepth) * modHistComp.listHistDataProcess[0].xIncrement;
            }
            else
            {
                minValueX = -2.000000E-4;
                maxValueX = minValueX + (1000000) * 4.000000E-10;
            }
            Parallel.For(0, modHistComp.listHistDataProcess.Count, item =>
            {
                Series dataTemp = new Series();
                dataTemp.strDispID = modHistComp.listHistDataProcess[item].strTag;
                dataTemp.strChanMeasType = modHistComp.listHistDataProcess[item].strMeasType;
                dataTemp.strDevName = modHistComp.listHistDataProcess[item].strDevName;
                dataTemp.strTestTim = modHistComp.listHistDataProcess[item].strTestTim;
                dataTemp.chanActual = modHistComp.listHistDataProcess[item].chanID;
                // 查找条件：通道标记&&测试时间
                int index = listHistWaveView.FindIndex(ex => (ex.m_strChanID == dataTemp.strDispID) && (ex.m_strTestTim == modHistComp.listHistDataProcess[item].strTestTim));
                if (index != -1)
                {
                    dataTemp.m_lineColor = listHistWaveView[index].m_selectColor;
                    for (int i = 0; i < modHistComp.listHistDataProcess[item].dataDispRough.Length; i++)
                    {
                        PointF temPoint = new PointF();
                        temPoint.X = 0;
                        temPoint.Y = (float)(modHistComp.listHistDataProcess[item].dataDispRough[i]);
                        dataTemp.dispData.Add(temPoint);
                    }
                    lock (objlock)
                    {
                        listHistWaveSeriesShow.Add(dataTemp);
                    }
                    
                }
            });
            point_StartPos = 0;//起始点
            point_StopPos = 0;//终止点
            totalPointNum = 0;//所有点
                           
            //委托方式刷新
            update_PictureBoxView_Screen(true);
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
                foreach (UCHistoryComp item in listHistWaveView)
                {
                    flowLayoutPanel_LabelItem.Controls.Add(item);
                    //订阅事件
                    Subscribe(item);
                }
            }

        }
        /// <summary>
        /// 更新波形绘制
        /// </summary>
        public void update_PictureBoxView_Screen()
        {
            if (this.pictureBox_Wave.InvokeRequired)
            {
                waveView_Refresh s = new waveView_Refresh(update_PictureBoxView_Screen);
                this.pictureBox_Wave.Invoke(s);
            }
            else
            {
                pictureBox_Wave_PaintWave();
            }
        }

        public void update_PictureBoxView_Screen(bool RefreshLabel)
        {
            if (this.pictureBox_Wave.InvokeRequired)
            {
                waveView_Refresh s = new waveView_Refresh(update_PictureBoxView_Screen);
                this.pictureBox_Wave.Invoke(s);
            }
            else
            {
                label_InterMutiple.Text = InterpretBase.textTran("内插倍数:X1");
                pictureBox_Wave_PaintWave();
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
                Drawing(listHistWaveSeriesShow);
                g.Dispose();
            }
            catch (Exception e)
            {
            }
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
                //g.SmoothingMode = SmoothingMode.HighQuality;  //图片柔顺模式选择
                g.SmoothingMode = SmoothingMode.AntiAlias;


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
                        graphics.SmoothingMode = SmoothingMode.AntiAlias;
                        int indexChan = listHistWaveView.FindIndex(chanItem => (chanItem.m_strChanID == listSeriesData[item].strDispID) && (chanItem.m_strTestTim == listSeriesData[item].strTestTim));
                        if (indexChan != -1)
                        {
                            if (listHistWaveView[indexChan].m_bCheck)//选中的才显示
                            {
                                xScaleCount = listSeriesData[item].dispData.Count;

                                horizontalBetween = canvasWidth / xScaleCount;
                                verticalBetween = charHeight / yScaleCount;
                                intervalValueY = (float)(maxValueY - minValueY) / yScaleCount;

                                //计算0值的坐标
                                //int tempv = (int)((Math.Abs(minValueY) - 0) / intervalValueY);//得到0到最小值的间隔距离；
                                int tempv = (int)((0 - minValueY) / intervalValueY);//得到0到最小值的间隔距离；
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
                                    //均值圆滑一下

                                    graphics.DrawCurve(new Pen(new SolidBrush(listSeriesData[item].m_lineColor), 1F), arrDataPoint);
                                    //graphics.DrawLines(new Pen(new SolidBrush(listSeriesData[item].m_lineColor), 1F), arrDataPoint);
                                }
                            }
                        }
                        graphics.Dispose();
                    });
                //清零计数
                foreach (Bitmap item in listImages)
                {
                    //item.MakeTransparent();
                    g.DrawImage(item, new Rectangle(0, 0, pictureBox_Wave.Width, pictureBox_Wave.Height));
                }
                g.Dispose();
                listImages.Clear();
            }
            pictureBox_Wave.Image = finalImage;
        }
        /// <summary>
        /// 清空显示界面
        /// </summary>
        private void histCompForm_Clear()
        {
            bXMarking = false;
            bYMarking = true;
            bDrawGradLineX = true;
            bDrawGradLineY = true;
            maxValueY = 3.0F;
            minValueY = -3.0F;
            bEnlagerClick = false;//波形放大标志
            bDrawStart = false;
            pointStart = Point.Empty;
            pointContinue = Point.Empty;
            point_StartPos = 0;//起始点
            point_StopPos = 0;//终止点
            totalPointNum = 0;//所有点
            modHistComp.listHistDataProcess.Clear();
            // 取消已有事件的注册
            foreach (UCHistoryComp item in listHistWaveView)
            {
                UnSubscribe(item);
            }
            listHistWaveView.Clear();
            listHistWaveSeriesShow.Clear();
            dtHistMeasInfo.Rows.Clear();
            // 清除显示
            flowLayoutPanel_LabelItem.Controls.Clear();
            panel_Label.Controls.Clear();
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

        private void iconButton_Start_Click(object sender, EventArgs e)
        {
            if (modHistComp.listHistDataProcess.Count == 0)
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
            if (minValueXTemp < modHistComp.listHistDataProcess[0].xOrigin)
            {
                MessageBox.Show(InterpretBase.textTran("超出范围"));
                return;
            }
            if (maxValueXTemp > modHistComp.listHistDataProcess[0].xOrigin + 1000000 * modHistComp.listHistDataProcess[0].xIncrement)
            {
                MessageBox.Show(InterpretBase.textTran("超出范围"));
                return;
            }

            point_StartPos = (int)((minValueXTemp - modHistComp.listHistDataProcess[0].xOrigin) / modHistComp.listHistDataProcess[0].xIncrement);
            point_StopPos = (int)((maxValueXTemp - modHistComp.listHistDataProcess[0].xOrigin) / modHistComp.listHistDataProcess[0].xIncrement);
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
                mutipleFin = modHistComp.findFinMutiple(pointDisplay, pointNum);
                if (mutipleFin > 200)
                {
                    //限制在200倍内插
                    mutipleFin = 200;
                }
                //显示当前的内插倍数
                label_InterMutiple.Text = "内插倍数:X" + mutipleFin.ToString();
            }
            else
            {
                mutipleFin = 1;
                //显示当前的内插倍数
                label_InterMutiple.Text = "内插倍数:X" + mutipleFin.ToString();
            }
            //波形放大插值操作
            bool bResult = modHistComp.calEnlargeWaveData_UsingCDLL(point_StartPos, point_StopPos, mutipleFin);
            //如果插值的话，需要重新计算结束位置
            if (mutipleFin != 1)
            {
                int numRange = (int)Math.Ceiling(pointDisplay / (mutipleFin * 1.0));
                point_StopPos = point_StartPos + numRange - 1;
            }
            //计算x轴的范围
            minValueX = modHistComp.listHistDataProcess[0].xOrigin + (point_StartPos) * modHistComp.listHistDataProcess[0].xIncrement;
            maxValueX = minValueX + (point_StopPos - point_StartPos + 1) * modHistComp.listHistDataProcess[0].xIncrement;
            foreach (Module_HistoryComp.HistoryDataProcess item in modHistComp.listHistDataProcess)
            {
                int index = listHistWaveSeriesShow.FindIndex(member => ((member.strDispID == item.strTag) && (member.strTestTim == item.strTestTim)));
                //int index = listWaveSeriesShow.FindIndex(member => (member.strChanID == item.strChanID));
                if (index != -1)
                {
                    if (bResult)
                    {
                        listHistWaveSeriesShow[index].dispData.Clear();
                        for (int i = 0; i < item.dataDispFin.Length; i++)
                        {
                            PointF temPoint = new PointF();
                            temPoint.X = 0;
                            /*加上显示时的偏移*/
                            //temPoint.Y = (float)(item.dataDispFin[i] - item.vertOffset + (item.vertViewOffset - item.vertOffset));
                            temPoint.Y = (float)(item.dataDispFin[i] + (item.vertViewOffset));
                            listHistWaveSeriesShow[index].dispData.Add(temPoint);
                        }
                    }
                }

            }
            update_PictureBoxView_Screen();
        }

        private void iconButton_Stop_Click(object sender, EventArgs e)
        {
            if (modHistComp.listHistDataProcess.Count == 0)
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
            if (minValueXTemp < modHistComp.listHistDataProcess[0].xOrigin)
            {
                MessageBox.Show(InterpretBase.textTran("超出范围"));
                return;
            }
            if (maxValueXTemp > modHistComp.listHistDataProcess[0].xOrigin + 1000000 * modHistComp.listHistDataProcess[0].xIncrement)
            {
                MessageBox.Show(InterpretBase.textTran("超出范围"));
                return;
            }

            point_StartPos = (int)((minValueXTemp - modHistComp.listHistDataProcess[0].xOrigin) / modHistComp.listHistDataProcess[0].xIncrement);
            point_StopPos = (int)((maxValueXTemp - modHistComp.listHistDataProcess[0].xOrigin) / modHistComp.listHistDataProcess[0].xIncrement);
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
                mutipleFin = modHistComp.findFinMutiple(pointDisplay, pointNum);
                if (mutipleFin > 200)
                {
                    //限制在200倍内插
                    mutipleFin = 200;
                }
                //显示当前的内插倍数
                label_InterMutiple.Text = "内插倍数:X" + mutipleFin.ToString();
            }
            else
            {
                mutipleFin = 1;
                //显示当前的内插倍数
                label_InterMutiple.Text = "内插倍数:X" + mutipleFin.ToString();
            }
            //波形放大插值操作
            bool bResult = modHistComp.calEnlargeWaveData_UsingCDLL(point_StartPos, point_StopPos, mutipleFin);
            //如果插值的话，需要重新计算结束位置
            if (mutipleFin != 1)
            {
                int numRange = (int)Math.Ceiling(pointDisplay / (mutipleFin * 1.0));
                point_StopPos = point_StartPos + numRange - 1;
            }
            //计算x轴的范围
            minValueX = modHistComp.listHistDataProcess[0].xOrigin + (point_StartPos) * modHistComp.listHistDataProcess[0].xIncrement;
            maxValueX = minValueX + (point_StopPos - point_StartPos + 1) * modHistComp.listHistDataProcess[0].xIncrement;
            foreach (Module_HistoryComp.HistoryDataProcess item in modHistComp.listHistDataProcess)
            {
                int index = listHistWaveSeriesShow.FindIndex(member => ((member.strDispID == item.strTag) && (member.strTestTim == item.strTestTim)));
                //int index = listWaveSeriesShow.FindIndex(member => (member.strChanID == item.strChanID));
                if (index != -1)
                {
                    if (bResult)
                    {
                        listHistWaveSeriesShow[index].dispData.Clear();
                        for (int i = 0; i < item.dataDispFin.Length; i++)
                        {
                            PointF temPoint = new PointF();
                            temPoint.X = 0;
                            /*加上显示时的偏移*/
                            //temPoint.Y = (float)(item.dataDispFin[i] - item.vertOffset + (item.vertViewOffset - item.vertOffset));
                            temPoint.Y = (float)(item.dataDispFin[i] + (item.vertViewOffset));
                            listHistWaveSeriesShow[index].dispData.Add(temPoint);
                        }
                    }
                }

            }
            update_PictureBoxView_Screen();
        }
        /// <summary>
        /// 正脉宽测量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItem_PlusePos_Click(object sender, EventArgs e)
        {
            dataGridView_MeasInfo.ReadOnly = true;
            if (panel_Meas.Visible == true)
            {
                panel_Meas.Visible = false;
            }
            else
            {
                panel_Meas.Visible = true;
                // 测量计算
                bool bResult = modHistComp.getHalfPeakWidth();
                if (bResult)
                {
                    dtHistMeasInfo.Rows.Clear();
                    dataGridView_MeasInfo.Columns.Clear();
                    for (int i = 0; i < modHistComp.listHistDataProcess.Count; i++)
                    {
                        dtHistMeasInfo.Rows.Add(modHistComp.listHistDataProcess[i].strTag,
                            modHistComp.listHistDataProcess[i].strMeasType,
                            modHistComp.listHistDataProcess[i].strDevName,
                            "CHAN" + modHistComp.listHistDataProcess[i].chanID.ToString(),
                            modHistComp.listHistDataProcess[i].strTestTim,
                            CValue2String.time2String(modHistComp.listHistDataProcess[i].wfmHalfPeakWidth));
                    }
                    dataGridView_MeasInfo.DataSource = dtHistMeasInfo.Copy();
                }
            }
        }
        /// <summary>
        /// 负脉宽测量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItem_PluseNeg_Click(object sender, EventArgs e)
        {
            dataGridView_MeasInfo.ReadOnly = true;
            if (panel_Meas.Visible == true)
            {
                panel_Meas.Visible = false;
            }
            else
            {
                panel_Meas.Visible = true;
                // 测量计算
                bool bResult = modHistComp.getHalfPeakWidth_Neg();
                if (bResult)
                {
                    dtHistMeasInfo.Rows.Clear();
                    dataGridView_MeasInfo.Columns.Clear();
                    for (int i = 0; i < modHistComp.listHistDataProcess.Count; i++)
                    {
                        dtHistMeasInfo.Rows.Add(modHistComp.listHistDataProcess[i].strTag,
                            modHistComp.listHistDataProcess[i].strMeasType,
                            modHistComp.listHistDataProcess[i].strDevName,
                            "CHAN" + modHistComp.listHistDataProcess[i].chanID.ToString(),
                            modHistComp.listHistDataProcess[i].strTestTim,
                            CValue2String.time2String(modHistComp.listHistDataProcess[i].wfmHalfPeakWidth));
                    }
                    dataGridView_MeasInfo.DataSource = dtHistMeasInfo.Copy();
                }
            }
        }
    }
}
