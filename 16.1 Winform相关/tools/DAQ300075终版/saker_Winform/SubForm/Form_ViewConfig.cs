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
using ClassLibrary_MultiLanguage;
using saker_Winform.CommonBaseModule;
using saker_Winform.Module;
using saker_Winform.UserControls;

namespace saker_Winform.SubForm
{
    public partial class Form_ViewConfig : Form
    {
        public Form_ViewConfig()
        {
            InitializeComponent();
            // 开启双缓冲          
            this.dataGridView_Group1.DoubleBufferedDataGirdView(true);
            this.dataGridView_Group2.DoubleBufferedDataGirdView(true);
            this.dataGridView_Group3.DoubleBufferedDataGirdView(true);
            this.dataGridView_Group4.DoubleBufferedDataGirdView(true);
        }

        #region 字段
        public static string tab1Name;
        public static string tab2Name;
        public static string tab3Name;
        public static string tab4Name;

        private bool bIsFill = false;//是否填充

        //List<DataTable> dataTable_ViewConfigs = new List<DataTable>();
        public Dictionary<int, DataTable> dataTable_ViewConfigsShow = new Dictionary<int, DataTable>();//有效数据的字典
        public bool bViewConfig = false;//是否应用

        #endregion

        #region 自定义事件
        /*定义事件参数类*/
        public class clickViewFormEventArgs : EventArgs
        {
            public readonly string KeyToRaiseEvent;

            public clickViewFormEventArgs(string keyToRaiseEvent)
            {
                KeyToRaiseEvent = keyToRaiseEvent;
            }
        }
        /*定义委托声明*/
        public delegate void clickViewFormEventHandler(object sender, clickViewFormEventArgs e);

        //用event关键字声明事件对象
        public event clickViewFormEventHandler clickViewFormEvent;

        //事件触发方法
        protected virtual void onClickViewFormEvent(clickViewFormEventArgs e)
        {
            if (clickViewFormEvent != null)
            {
                clickViewFormEvent(this, e);
            }
        }

        //引发事件
        private void RaiseEvent(string keyToRaiseEvent)
        {
            clickViewFormEventArgs e = new clickViewFormEventArgs(keyToRaiseEvent);

            onClickViewFormEvent(e);
        }

        #endregion

        #region 事件：Load面板
        /// <summary>
        /// 事件：Load面板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_ViewConfig_Load(object sender, EventArgs e)
        {
            /* dataGridView字体设置 */
            dataGridView_Group1.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("微软雅黑", 9.5F, System.Drawing.FontStyle.Regular);//设置DataGridView字体大小
            dataGridView_Group2.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("微软雅黑", 9.5F, System.Drawing.FontStyle.Regular);//设置DataGridView字体大小
            dataGridView_Group3.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("微软雅黑", 9.5F, System.Drawing.FontStyle.Regular);//设置DataGridView字体大小
            dataGridView_Group4.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("微软雅黑", 9.5F, System.Drawing.FontStyle.Regular);//设置DataGridView字体大小
            dataGridView_Group1.DefaultCellStyle.Font = new System.Drawing.Font("微软雅黑", 9.5F, System.Drawing.FontStyle.Regular);//设置DataGridView字体大小
            dataGridView_Group2.DefaultCellStyle.Font = new System.Drawing.Font("微软雅黑", 9.5F, System.Drawing.FontStyle.Regular);//设置DataGridView字体大小
            dataGridView_Group3.DefaultCellStyle.Font = new System.Drawing.Font("微软雅黑", 9.5F, System.Drawing.FontStyle.Regular);//设置DataGridView字体大小
            dataGridView_Group4.DefaultCellStyle.Font = new System.Drawing.Font("微软雅黑", 9.5F, System.Drawing.FontStyle.Regular);//设置DataGridView字体大小

            /* Tabpage Load */
            this.tabControl_ViewConfig.TabPages[0].Text = Module_ViewConfig.Instance.TableName[0];
            this.tabControl_ViewConfig.TabPages[1].Text = Module_ViewConfig.Instance.TableName[1];
            this.tabControl_ViewConfig.TabPages[2].Text = Module_ViewConfig.Instance.TableName[2];
            this.tabControl_ViewConfig.TabPages[3].Text = Module_ViewConfig.Instance.TableName[3];

            textBox_Group1.Text = Module_ViewConfig.Instance.TableName[0];
            textBox_Group2.Text = Module_ViewConfig.Instance.TableName[1];
            textBox_Group3.Text = Module_ViewConfig.Instance.TableName[2];
            textBox_Group4.Text = Module_ViewConfig.Instance.TableName[3];

        }
        #endregion

        #region 方法：绑定数据源及背景色
        public void LoadViewConfig()
        {
            /* 从Module_ViewConfig单例中获取数据 */
            this.dataGridView_Group1.EndEdit();//结束编辑
            dataGridView_Group1.DataSource = Module_ViewConfig.Instance.GetShowConfigByGroup("1");//绑定数据源 
            dataGridView_Group2.DataSource = Module_ViewConfig.Instance.GetShowConfigByGroup("2");//绑定数据源           
            dataGridView_Group3.DataSource = Module_ViewConfig.Instance.GetShowConfigByGroup("3");//绑定数据源         
            dataGridView_Group4.DataSource = Module_ViewConfig.Instance.GetShowConfigByGroup("4");//绑定数据源   

            BindingColor(dataGridView_Group1);
            BindingColor(dataGridView_Group2);
            BindingColor(dataGridView_Group3);
            BindingColor(dataGridView_Group4);
        }
        #endregion

        #region 方法：绑定背景色
        /// <summary>
        /// 方法：绑定背景色
        /// </summary>
        public void BindingColor(DataGridView dataGridView)
        {
            if (dataGridView.Rows.Count > 0)
            {
                for (int i = 0; i < dataGridView.Rows.Count; i++)
                {
                    string WaveColor = "WaveColor_" + (i + 1).ToString();
                    dataGridView.Rows[i].Cells[6].Style.BackColor = Color.FromArgb(int.Parse(dataGridView.Rows[i].Cells[6].Value.ToString()));//第7列为通道颜色
                    dataGridView.Rows[i].Cells[6].Style.ForeColor = Color.FromArgb(int.Parse(dataGridView.Rows[i].Cells[6].Value.ToString()));//字体第7列为通道颜色
                }
            }
            dataGridView.Refresh();
        }
        #endregion

        #region 方法：将有效数据写进字典中      
        /// <summary>
        /// 方法：将有效数据写进字典中
        /// </summary>
        /// <param name="indexGroup"></param>
        /// <param name="dataTable"></param>
        private void AddDictionaryData(int indexGroup, DataTable dataTable)
        {
            if (dataTable != null)
            {
                if (dataTable.Rows.Count > 0)
                {
                    DataRow[] dataRows_ViewConfig = dataTable.AsEnumerable().Where(item=>item.ItemArray[3].ToString()=="True").ToArray();//选取有效的数据成DataRow[]
                    if (dataRows_ViewConfig.Count() > 0)
                    {
                        DataTable dataTableValidView = dataRows_ViewConfig.CopyToDataTable();//将DataRows转换为DataTable
                        if (!dataTable_ViewConfigsShow.ContainsKey(indexGroup))//如果键值不存在则允许Add
                        {
                            dataTable_ViewConfigsShow.Add(indexGroup, dataTableValidView);//将Group下四张DataTable传给波形监测
                        }

                    }
                }
            }

        }
        #endregion

        #region 方法：绑定自定义事件，将所有数据写进单历中，将有效数据写进字典中
        /// <summary>
        /// 方法：绑定自定义事件，将所有数据写进单历中，将有效数据写进字典中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void button_ViewConfigClick(bool bState = true)
        {
            bViewConfig = true;
            /*将勾选数据选取出来，绑定成新DataTable给波形监测界面*/
            DataTable dataGridView_Group1Source1 = dataGridView_Group1.DataSource as DataTable;//将Group1转换为DataTable
            DataTable dataGridView_Group1Source2 = dataGridView_Group2.DataSource as DataTable;//将Group2转换为DataTable
            DataTable dataGridView_Group1Source3 = dataGridView_Group3.DataSource as DataTable;//将Group3转换为DataTable
            DataTable dataGridView_Group1Source4 = dataGridView_Group4.DataSource as DataTable;//将Group4转换为DataTable
            Module.Module_ViewConfig.Instance.ViewConfig.Clear();//清除单例中的数据
            Module.Module_ViewConfig.Instance.SetShowConfig("1", dataGridView_Group1Source1);
            Module.Module_ViewConfig.Instance.SetShowConfig("2", dataGridView_Group1Source2);
            Module.Module_ViewConfig.Instance.SetShowConfig("3", dataGridView_Group1Source3);
            Module.Module_ViewConfig.Instance.SetShowConfig("4", dataGridView_Group1Source4);

            dataTable_ViewConfigsShow.Clear();//清除字典里面的key和值
            AddDictionaryData(1, dataGridView_Group1Source1);//将Group1的有效数据写进字典中
            AddDictionaryData(2, dataGridView_Group1Source2);//将Group2的有效数据写进字典中
            AddDictionaryData(3, dataGridView_Group1Source3);//将Group3的有效数据写进字典中
            AddDictionaryData(4, dataGridView_Group1Source4);//将Group4的有效数据写进字典中
            if (bState)
            {
                RaiseEvent("button_ViewConfig");//自定义事件
            }

        }
        #endregion

        #region 事件：颜色弹窗
        /// <summary>
        /// 事件：dataGridView_Group1更改颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_Group1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.ColumnIndex == 6) && (e.RowIndex > -1))//选择颜色列
            {
                //colorDialog_Group1.ShowDialog();
                if (colorDialog_Group1.ShowDialog() == DialogResult.OK)
                {
                    Color color = colorDialog_Group1.Color;
                    this.dataGridView_Group1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = color;
                    this.dataGridView_Group1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = color;
                    //this.dataGridView_Group1.DefaultCellStyle.SelectionBackColor = color;
                    //this.dataGridView_Group1.DefaultCellStyle.SelectionForeColor = color;
                    int WaveColor = color.ToArgb();
                    this.dataGridView_Group1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = WaveColor;
                    this.dataGridView_Group1.EndEdit();//结束编辑
                    this.dataGridView_Group1.Refresh();
                }
            }
        }
        /// <summary>
        /// 事件：dataGridView_Group2更改颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_Group2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 6 && (e.RowIndex > -1))//选择颜色列
            {
                //colorDialog_Group1.ShowDialog();
                if (colorDialog_Group1.ShowDialog() == DialogResult.OK)
                {
                    Color color = colorDialog_Group1.Color;
                    this.dataGridView_Group2.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = color;
                    this.dataGridView_Group2.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = color;
                    //this.dataGridView_Group2.DefaultCellStyle.SelectionBackColor = color;
                    //this.dataGridView_Group2.DefaultCellStyle.SelectionForeColor = color;
                    int WaveColor = color.ToArgb();
                    this.dataGridView_Group2.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = WaveColor;
                    this.dataGridView_Group2.EndEdit();//结束编辑
                    this.dataGridView_Group2.Refresh();
                }
            }
        }
        /// <summary>
        /// 事件：dataGridView_Group3更改颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_Group3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 6 && (e.RowIndex > -1))//选择颜色列
            {
                //colorDialog_Group1.ShowDialog();
                if (colorDialog_Group1.ShowDialog() == DialogResult.OK)
                {
                    Color color = colorDialog_Group1.Color;
                    this.dataGridView_Group3.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = color;
                    this.dataGridView_Group3.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = color;
                    //this.dataGridView_Group3.DefaultCellStyle.SelectionBackColor = color;
                    //this.dataGridView_Group3.DefaultCellStyle.SelectionForeColor = color;
                    int WaveColor = color.ToArgb();
                    this.dataGridView_Group3.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = WaveColor;
                    this.dataGridView_Group3.EndEdit();//结束编辑
                    this.dataGridView_Group3.Refresh();
                }
            }
        }
        /// <summary>
        /// 事件：dataGridView_Group4更改颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_Group4_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 6 && (e.RowIndex > -1))//选择颜色列
            {
                //colorDialog_Group1.ShowDialog();
                if (colorDialog_Group1.ShowDialog() == DialogResult.OK)
                {
                    Color color = colorDialog_Group1.Color;
                    this.dataGridView_Group4.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = color;//改变背景色为选择颜色
                    this.dataGridView_Group4.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = color;//改变前景色为选择颜色
                    //this.dataGridView_Group4.DefaultCellStyle.SelectionBackColor = color;
                    //this.dataGridView_Group4.DefaultCellStyle.SelectionForeColor = color;
                    int WaveColor = color.ToArgb();
                    this.dataGridView_Group4.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = WaveColor;
                    this.dataGridView_Group4.EndEdit();//结束编辑
                    this.dataGridView_Group4.Refresh();
                }
            }
        }
        #endregion

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //dataTable_ViewConfigs.Add();
        }

        private void tabControl_ViewConfig_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadViewConfig();
        }

        /// <summary>
        /// 方法：清除四张DataGridView
        /// </summary>
        public void ClearViewConfigShow()
        {
            dataGridView_Group1.DataSource = Module_ViewConfig.Instance.GetNullTable();
            dataGridView_Group2.DataSource = Module_ViewConfig.Instance.GetNullTable();
            dataGridView_Group3.DataSource = Module_ViewConfig.Instance.GetNullTable();
            dataGridView_Group4.DataSource = Module_ViewConfig.Instance.GetNullTable();
        }

        #region 事件：DTGV变更即存
        /// <summary>
        /// 值改变即存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_Group1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            this.dataGridView_Group1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            DataTable dataTable_ViewConfig1 = dataGridView_Group1.DataSource as DataTable;
            /*将数据写进单历中*/
            Module.Module_ViewConfig.Instance.ViewConfig.Remove("1");
            Module.Module_ViewConfig.Instance.SetShowConfig("1", dataTable_ViewConfig1);
        }

        private void dataGridView_Group2_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            this.dataGridView_Group2.CommitEdit(DataGridViewDataErrorContexts.Commit);
            DataTable dataTable_ViewConfig2 = dataGridView_Group2.DataSource as DataTable;
            /*将数据写进单历中*/
            Module.Module_ViewConfig.Instance.ViewConfig.Remove("2");//清除单例中的数据
            Module.Module_ViewConfig.Instance.SetShowConfig("2", dataTable_ViewConfig2);
        }

        private void dataGridView_Group3_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            this.dataGridView_Group3.CommitEdit(DataGridViewDataErrorContexts.Commit);
            DataTable dataTable_ViewConfig3 = dataGridView_Group3.DataSource as DataTable;
            /*将数据写进单历中*/
            Module.Module_ViewConfig.Instance.ViewConfig.Remove("3");//清除单例中的数据
            Module.Module_ViewConfig.Instance.SetShowConfig("3", dataTable_ViewConfig3);
        }

        private void dataGridView_Group4_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            this.dataGridView_Group4.CommitEdit(DataGridViewDataErrorContexts.Commit);
            DataTable dataTable_ViewConfig4 = dataGridView_Group4.DataSource as DataTable;
            /*将数据写进单历中*/
            Module.Module_ViewConfig.Instance.ViewConfig.Remove("4");//清除单例中的数据
            Module.Module_ViewConfig.Instance.SetShowConfig("4", dataTable_ViewConfig4);
        }
        #endregion

        #region 事件：textBox文本变更
        private void textBox_Group1_TextChanged(object sender, EventArgs e)
        {
            this.tabControl_ViewConfig.TabPages[0].Text = textBox_Group1.Text;
            Module_ViewConfig.Instance.TableName[0] = this.tabControl_ViewConfig.TabPages[0].Text;
        }

        private void textBox_Group2_TextChanged(object sender, EventArgs e)
        {
            this.tabControl_ViewConfig.TabPages[1].Text = textBox_Group2.Text;
            Module_ViewConfig.Instance.TableName[1] = this.tabControl_ViewConfig.TabPages[1].Text;
        }

        private void textBox_Group3_TextChanged(object sender, EventArgs e)
        {
            this.tabControl_ViewConfig.TabPages[2].Text = textBox_Group3.Text;
            Module_ViewConfig.Instance.TableName[2] = this.tabControl_ViewConfig.TabPages[2].Text;
        }

        private void textBox_Group4_TextChanged(object sender, EventArgs e)
        {
            this.tabControl_ViewConfig.TabPages[3].Text = textBox_Group4.Text;
            Module_ViewConfig.Instance.TableName[3] = this.tabControl_ViewConfig.TabPages[3].Text;
        }
        #endregion

        #region 方法：实现拉选复制
        private void dataGridView_Group1_MouseMove(object sender, MouseEventArgs e)
        {

            if (dataGridView_Group1.SelectedCells.Count == 1)
            {
                int rowIndex = dataGridView_Group1.SelectedCells[0].RowIndex;
                int columnIndex = dataGridView_Group1.SelectedCells[0].ColumnIndex;
                //返回单元格相对于datagridview的Rectangle
                if (columnIndex != 0 && columnIndex != 2 && columnIndex != 1)
                {
                    Rectangle r = dataGridView_Group1.GetCellDisplayRectangle(columnIndex, rowIndex, false);
                    int x = r.X + r.Width;//单元格右下角相对于datagridview的坐标x
                    int y = r.Y + r.Height;//单元格右下角相对于datagridview的坐标y
                    if (Math.Abs(e.X - x) < r.Width / 10 && Math.Abs(e.Y - y) < r.Height / 5)
                    {
                        this.Cursor = System.Windows.Forms.Cursors.Cross;
                        bIsFill = true;
                    }
                    else
                    {
                        this.Cursor = System.Windows.Forms.Cursors.Default;
                        bIsFill = false;
                    }
                }
            }
        }
        private void dataGridView_Group1_MouseUp(object sender, MouseEventArgs e)
        {
            if (bIsFill)
            {
                bIsFill = false;
                if (dataGridView_Group1.SelectedCells.Count > 1)
                {
                    int count = dataGridView_Group1.SelectedCells.Count;
                    for (int i = 0; i < (count - 1); i++)
                    {
                        if (dataGridView_Group1.SelectedCells[i].ColumnIndex != dataGridView_Group1.SelectedCells[i + 1].ColumnIndex)
                        {
                            return;
                        }
                    }
                    for (int i = 0; i < count - 1; i++)
                    {
                        dataGridView_Group1.SelectedCells[i].Value = dataGridView_Group1.SelectedCells[count - 1].Value;
                    }
                }
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;

            this.dataGridView_Group1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            DataTable dataTable_ViewConfig1 = dataGridView_Group1.DataSource as DataTable;
            /*将数据写进单历中*/
            Module.Module_ViewConfig.Instance.ViewConfig.Remove("1");
            Module.Module_ViewConfig.Instance.SetShowConfig("1", dataTable_ViewConfig1);
        }
        private void dataGridView_Group1_MouseDown(object sender, MouseEventArgs e)
        {
            if (bIsFill)
            {
                this.Cursor = System.Windows.Forms.Cursors.Cross;
            }
            if (e.Button == MouseButtons.Right)
            {
                this.contextMenuStrip1.Show(MousePosition);
            }
        }

        private void dataGridView_Group2_MouseMove(object sender, MouseEventArgs e)
        {
            if (dataGridView_Group2.SelectedCells.Count == 1)
            {
                int rowIndex = dataGridView_Group2.SelectedCells[0].RowIndex;
                int columnIndex = dataGridView_Group2.SelectedCells[0].ColumnIndex;
                if (columnIndex != 0 && columnIndex != 2 && columnIndex != 1)
                {
                    //返回单元格相对于datagridview的Rectangle
                    Rectangle r = dataGridView_Group2.GetCellDisplayRectangle(columnIndex, rowIndex, false);
                    int x = r.X + r.Width;//单元格右下角相对于datagridview的坐标x
                    int y = r.Y + r.Height;//单元格右下角相对于datagridview的坐标y
                    if (Math.Abs(e.X - x) < r.Width / 10 && Math.Abs(e.Y - y) < r.Height / 5)
                    {
                        this.Cursor = System.Windows.Forms.Cursors.Cross;
                        bIsFill = true;
                    }
                    else
                    {
                        this.Cursor = System.Windows.Forms.Cursors.Default;
                        bIsFill = false;
                    }
                }

            }
        }
        private void dataGridView_Group2_MouseUp(object sender, MouseEventArgs e)
        {
            if (bIsFill)
            {
                bIsFill = false;
                if (dataGridView_Group2.SelectedCells.Count > 1)
                {
                    int count = dataGridView_Group2.SelectedCells.Count;
                    for (int i = 0; i < (count - 1); i++)
                    {
                        if (dataGridView_Group2.SelectedCells[i].ColumnIndex != dataGridView_Group2.SelectedCells[i + 1].ColumnIndex)
                        {
                            return;
                        }
                    }
                    for (int i = 0; i < count - 1; i++)
                    {
                        dataGridView_Group2.SelectedCells[i].Value = dataGridView_Group2.SelectedCells[count - 1].Value;
                    }
                }
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.dataGridView_Group2.CommitEdit(DataGridViewDataErrorContexts.Commit);
            DataTable dataTable_ViewConfig2 = dataGridView_Group2.DataSource as DataTable;
            /*将数据写进单历中*/
            Module.Module_ViewConfig.Instance.ViewConfig.Remove("2");
            Module.Module_ViewConfig.Instance.SetShowConfig("2", dataTable_ViewConfig2);
        }
        private void dataGridView_Group2_MouseDown(object sender, MouseEventArgs e)
        {
            if (bIsFill)
            {
                this.Cursor = System.Windows.Forms.Cursors.Cross;
            }
            if (e.Button == MouseButtons.Right)
            {
                this.contextMenuStrip1.Show(MousePosition);
            }
        }

        private void dataGridView_Group3_MouseMove(object sender, MouseEventArgs e)
        {
            if (dataGridView_Group3.SelectedCells.Count == 1)
            {
                int rowIndex = dataGridView_Group3.SelectedCells[0].RowIndex;
                int columnIndex = dataGridView_Group3.SelectedCells[0].ColumnIndex;
                if (columnIndex != 0 && columnIndex != 2 && columnIndex != 1)
                {
                    //返回单元格相对于datagridview的Rectangle
                    Rectangle r = dataGridView_Group3.GetCellDisplayRectangle(columnIndex, rowIndex, false);
                    int x = r.X + r.Width;//单元格右下角相对于datagridview的坐标x
                    int y = r.Y + r.Height;//单元格右下角相对于datagridview的坐标y
                    if (Math.Abs(e.X - x) < r.Width / 10 && Math.Abs(e.Y - y) < r.Height / 5)
                    {
                        this.Cursor = System.Windows.Forms.Cursors.Cross;
                        bIsFill = true;
                    }
                    else
                    {
                        this.Cursor = System.Windows.Forms.Cursors.Default;
                        bIsFill = false;
                        this.Cursor = System.Windows.Forms.Cursors.Default;
                    }
                }                  
            }
        }

        private void dataGridView_Group3_MouseUp(object sender, MouseEventArgs e)
        {
            if (bIsFill)
            {
                bIsFill = false;
                if (dataGridView_Group3.SelectedCells.Count > 1)
                {
                    int count = dataGridView_Group3.SelectedCells.Count;
                    for (int i = 0; i < (count - 1); i++)
                    {
                        if (dataGridView_Group3.SelectedCells[i].ColumnIndex != dataGridView_Group3.SelectedCells[i + 1].ColumnIndex)
                        {
                            return;
                        }
                    }
                    for (int i = 0; i < count - 1; i++)
                    {
                        dataGridView_Group3.SelectedCells[i].Value = dataGridView_Group3.SelectedCells[count - 1].Value;
                    }
                }
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.dataGridView_Group3.CommitEdit(DataGridViewDataErrorContexts.Commit);
            DataTable dataTable_ViewConfig3 = dataGridView_Group3.DataSource as DataTable;
            /*将数据写进单历中*/
            Module.Module_ViewConfig.Instance.ViewConfig.Remove("3");//清除单例中的数据
            Module.Module_ViewConfig.Instance.SetShowConfig("3", dataTable_ViewConfig3);
        }

        private void dataGridView_Group3_MouseDown(object sender, MouseEventArgs e)
        {
            if (bIsFill)
            {
                this.Cursor = System.Windows.Forms.Cursors.Cross;
            }
            if (e.Button == MouseButtons.Right)
            {
                this.contextMenuStrip1.Show(MousePosition);
            }
        }

        private void dataGridView_Group4_MouseMove(object sender, MouseEventArgs e)
        {
            if (dataGridView_Group4.SelectedCells.Count == 1)
            {
                int rowIndex = dataGridView_Group4.SelectedCells[0].RowIndex;
                int columnIndex = dataGridView_Group4.SelectedCells[0].ColumnIndex;
                if (columnIndex != 0 && columnIndex != 2 && columnIndex != 1)
                {
                    //返回单元格相对于datagridview的Rectangle
                    Rectangle r = dataGridView_Group4.GetCellDisplayRectangle(columnIndex, rowIndex, false);
                    int x = r.X + r.Width;//单元格右下角相对于datagridview的坐标x
                    int y = r.Y + r.Height;//单元格右下角相对于datagridview的坐标y
                    if (Math.Abs(e.X - x) < r.Width / 10 && Math.Abs(e.Y - y) < r.Height / 5)
                    {
                        this.Cursor = System.Windows.Forms.Cursors.Cross;
                        bIsFill = true;
                    }
                    else
                    {
                        this.Cursor = System.Windows.Forms.Cursors.Default;
                        bIsFill = false;
                    }
                }             
            }
        }
        private void dataGridView_Group4_MouseUp(object sender, MouseEventArgs e)
        {
            if (bIsFill)
            {
                bIsFill = false;
                if (dataGridView_Group4.SelectedCells.Count > 1)
                {
                    int count = dataGridView_Group4.SelectedCells.Count;
                    for (int i = 0; i < (count - 1); i++)
                    {
                        if (dataGridView_Group4.SelectedCells[i].ColumnIndex != dataGridView_Group4.SelectedCells[i + 1].ColumnIndex)
                        {
                            return;
                        }
                    }
                    for (int i = 0; i < count - 1; i++)
                    {
                        dataGridView_Group4.SelectedCells[i].Value = dataGridView_Group4.SelectedCells[count - 1].Value;
                    }
                }
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.dataGridView_Group4.CommitEdit(DataGridViewDataErrorContexts.Commit);
            DataTable dataTable_ViewConfig4 = dataGridView_Group4.DataSource as DataTable;
            /*将数据写进单历中*/
            Module.Module_ViewConfig.Instance.ViewConfig.Remove("4");//清除单例中的数据
            Module.Module_ViewConfig.Instance.SetShowConfig("4", dataTable_ViewConfig4);
        }
        private void dataGridView_Group4_MouseDown(object sender, MouseEventArgs e)
        {
            if (bIsFill)
            {
                this.Cursor = System.Windows.Forms.Cursors.Cross;
            }
            if (e.Button == MouseButtons.Right)
            {
                this.contextMenuStrip1.Show(MousePosition);
            }
        }

        #endregion

        #region 事件：鼠标离开
        private void tabControl_ViewConfig_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void dataGridView_Group1_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void dataGridView_Group2_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void dataGridView_Group3_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void dataGridView_Group4_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }
        #endregion

        #region 菜单操作：全选或取消全选
        private void ToolStripMenuItem_Choose_Click(object sender, EventArgs e)
        {
            var str = tabControl_ViewConfig.SelectedIndex;
            switch (str)
            {
                case 0:
                    this.dataGridView_Group1.EndEdit();//结束编辑                   
                    CDataGridViewShow.AllChooseOrNot(dataGridView_Group1, true, 3);
                    this.dataGridView_Group1.CommitEdit(DataGridViewDataErrorContexts.Commit);
                    DataTable dataTable_ViewConfig1 = dataGridView_Group1.DataSource as DataTable;
                    /*将数据写进单历中*/
                    Module.Module_ViewConfig.Instance.ViewConfig.Remove("1");//清除单例中的数据
                    Module.Module_ViewConfig.Instance.SetShowConfig("1", dataTable_ViewConfig1);
                    break;
                case 1:
                    this.dataGridView_Group2.EndEdit();//结束编辑
                    CDataGridViewShow.AllChooseOrNot(dataGridView_Group2, true, 3);
                    this.dataGridView_Group2.CommitEdit(DataGridViewDataErrorContexts.Commit);
                    DataTable dataTable_ViewConfig2 = dataGridView_Group2.DataSource as DataTable;
                    /*将数据写进单历中*/
                    Module.Module_ViewConfig.Instance.ViewConfig.Remove("2");//清除单例中的数据
                    Module.Module_ViewConfig.Instance.SetShowConfig("2", dataTable_ViewConfig2);
                    break;
                case 2:
                    this.dataGridView_Group3.EndEdit();//结束编辑
                    CDataGridViewShow.AllChooseOrNot(dataGridView_Group3, true, 3);
                    this.dataGridView_Group3.CommitEdit(DataGridViewDataErrorContexts.Commit);
                    DataTable dataTable_ViewConfig3 = dataGridView_Group3.DataSource as DataTable;
                    /*将数据写进单历中*/
                    Module.Module_ViewConfig.Instance.ViewConfig.Remove("3");//清除单例中的数据
                    Module.Module_ViewConfig.Instance.SetShowConfig("3", dataTable_ViewConfig3);
                    break;
                case 3:
                    this.dataGridView_Group4.EndEdit();//结束编辑
                    CDataGridViewShow.AllChooseOrNot(dataGridView_Group4, true, 3);
                    this.dataGridView_Group4.CommitEdit(DataGridViewDataErrorContexts.Commit);
                    DataTable dataTable_ViewConfig4 = dataGridView_Group4.DataSource as DataTable;
                    /*将数据写进单历中*/
                    Module.Module_ViewConfig.Instance.ViewConfig.Remove("4");//清除单例中的数据
                    Module.Module_ViewConfig.Instance.SetShowConfig("4", dataTable_ViewConfig4);
                    break;
            }
        }

        private void ToolStripMenuItem_cancelChoose_Click(object sender, EventArgs e)
        {
            var str = tabControl_ViewConfig.SelectedIndex;
            switch (str)
            {
                case 0:
                    this.dataGridView_Group1.EndEdit();//结束编辑
                    CDataGridViewShow.AllChooseOrNot(dataGridView_Group1, false, 3);
                    this.dataGridView_Group1.CommitEdit(DataGridViewDataErrorContexts.Commit);
                    DataTable dataTable_ViewConfig1 = dataGridView_Group1.DataSource as DataTable;
                    /*将数据写进单历中*/
                    Module.Module_ViewConfig.Instance.ViewConfig.Remove("1");//清除单例中的数据
                    Module.Module_ViewConfig.Instance.SetShowConfig("1", dataTable_ViewConfig1);
                    break;
                case 1:
                    this.dataGridView_Group2.EndEdit();//结束编辑
                    CDataGridViewShow.AllChooseOrNot(dataGridView_Group2, false, 3);
                    this.dataGridView_Group2.CommitEdit(DataGridViewDataErrorContexts.Commit);
                    DataTable dataTable_ViewConfig2 = dataGridView_Group2.DataSource as DataTable;
                    /*将数据写进单历中*/
                    Module.Module_ViewConfig.Instance.ViewConfig.Remove("2");//清除单例中的数据
                    Module.Module_ViewConfig.Instance.SetShowConfig("2", dataTable_ViewConfig2);
                    break;
                case 2:
                    this.dataGridView_Group3.EndEdit();//结束编辑
                    CDataGridViewShow.AllChooseOrNot(dataGridView_Group3, false, 3);
                    this.dataGridView_Group3.CommitEdit(DataGridViewDataErrorContexts.Commit);
                    DataTable dataTable_ViewConfig3 = dataGridView_Group3.DataSource as DataTable;
                    /*将数据写进单历中*/
                    Module.Module_ViewConfig.Instance.ViewConfig.Remove("3");//清除单例中的数据
                    Module.Module_ViewConfig.Instance.SetShowConfig("3", dataTable_ViewConfig3);
                    break;
                case 3:
                    this.dataGridView_Group4.EndEdit();//结束编辑
                    CDataGridViewShow.AllChooseOrNot(dataGridView_Group4, false, 3);
                    this.dataGridView_Group4.CommitEdit(DataGridViewDataErrorContexts.Commit);
                    DataTable dataTable_ViewConfig4 = dataGridView_Group4.DataSource as DataTable;
                    /*将数据写进单历中*/
                    Module.Module_ViewConfig.Instance.ViewConfig.Remove("4");//清除单例中的数据
                    Module.Module_ViewConfig.Instance.SetShowConfig("4", dataTable_ViewConfig4);
                    break;
            }
        }
        #endregion

        #region 事件：双击更改整列
        private void dataGridView_Group1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridView_Group1.Rows.Count > 0)
            {
                var clickColumn = e.ColumnIndex;
                var clickRow = e.RowIndex;
                double newDouble;
                if (clickRow < 0)
                {
                    switch (clickColumn)
                    {
                        case 4:
                           
                            if (!double.TryParse(this.dataGridView_Group1.CurrentRow.Cells[4].Value.ToString(), out newDouble))
                            {
                                return;
                            }
                            else
                            {
                                if (double.Parse(this.dataGridView_Group1.CurrentRow.Cells[4].Value.ToString()) >= double.Parse(this.dataGridView_Group1.CurrentRow.Cells[5].Value.ToString()))
                                {                                  
                                    return;
                                }
                            }                               
                            doubleClickDataGridView(this.dataGridView_Group1, 1, 4);
                            break;
                        case 5:
                            if (!double.TryParse(this.dataGridView_Group1.CurrentRow.Cells[5].Value.ToString(), out newDouble))
                            {
                                return;
                            }
                            else
                            {
                                if (double.Parse(this.dataGridView_Group1.CurrentRow.Cells[4].Value.ToString()) >= double.Parse(this.dataGridView_Group1.CurrentRow.Cells[5].Value.ToString()))
                                {
                                    return;
                                }
                            }                  
                            doubleClickDataGridView(this.dataGridView_Group1, 1, 5);
                            break;
                    }
                }
            }
        }

        private void dataGridView_Group2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridView_Group2.Rows.Count > 0)
            {
                var clickColumn = e.ColumnIndex;
                var clickRow = e.RowIndex;
                double newDouble;
                if (clickRow < 0)
                {
                    switch (clickColumn)
                    {
                        case 4:
                            if (!double.TryParse(this.dataGridView_Group2.CurrentRow.Cells[4].Value.ToString(), out newDouble))
                            {
                                return;
                            }
                            else
                            {
                                if (double.Parse(this.dataGridView_Group2.CurrentRow.Cells[4].Value.ToString()) >= double.Parse(this.dataGridView_Group2.CurrentRow.Cells[5].Value.ToString()))
                                {
                                    return;
                                }
                            }                        
                            doubleClickDataGridView(this.dataGridView_Group2, 2, 4);
                            break;
                        case 5:
                            if (!double.TryParse(this.dataGridView_Group2.CurrentRow.Cells[5].Value.ToString(), out newDouble))
                            {
                                return;
                            }
                            else
                            {
                                if (double.Parse(this.dataGridView_Group2.CurrentRow.Cells[4].Value.ToString()) >= double.Parse(this.dataGridView_Group2.CurrentRow.Cells[5].Value.ToString()))
                                {
                                    return;
                                }
                            }
                            doubleClickDataGridView(this.dataGridView_Group2, 2, 5);
                            break;
                    }
                }

            }
        }

        private void dataGridView_Group3_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridView_Group3.Rows.Count > 0)
            {
                var clickColumn = e.ColumnIndex;
                var clickRow = e.RowIndex;
                double newDouble;
                if (clickRow < 0)
                {
                    switch (clickColumn)
                    {
                        case 4:

                            if (!double.TryParse(this.dataGridView_Group3.CurrentRow.Cells[4].Value.ToString(), out newDouble))
                            {
                                return;
                            }
                            else
                            {
                                if (double.Parse(this.dataGridView_Group3.CurrentRow.Cells[4].Value.ToString()) >= double.Parse(this.dataGridView_Group3.CurrentRow.Cells[5].Value.ToString()))
                                {
                                    return;
                                }
                            }
                            doubleClickDataGridView(this.dataGridView_Group3, 3, 4);
                            break;
                        case 5:
                            if (!double.TryParse(this.dataGridView_Group3.CurrentRow.Cells[5].Value.ToString(), out newDouble))
                            {
                                return;
                            }
                            else
                            {
                                if (double.Parse(this.dataGridView_Group3.CurrentRow.Cells[4].Value.ToString()) >= double.Parse(this.dataGridView_Group3.CurrentRow.Cells[5].Value.ToString()))
                                {
                                    return;
                                }
                            }
                            doubleClickDataGridView(this.dataGridView_Group3, 3, 5);
                            break;
                    }
                }

            }
        }

        private void dataGridView_Group4_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridView_Group4.Rows.Count > 0)
            {
                var clickColumn = e.ColumnIndex;
                var clickRow = e.RowIndex;
                double newDouble;
                if (clickRow < 0)
                {
                    switch (clickColumn)
                    {
                        case 4:

                            if (!double.TryParse(this.dataGridView_Group4.CurrentRow.Cells[4].Value.ToString(), out newDouble))
                            {
                                return;
                            }
                            else
                            {
                                if (double.Parse(this.dataGridView_Group4.CurrentRow.Cells[4].Value.ToString()) >= double.Parse(this.dataGridView_Group4.CurrentRow.Cells[5].Value.ToString()))
                                {
                                    return;
                                }
                            }                       
                            doubleClickDataGridView(this.dataGridView_Group4, 4, 4);
                            break;
                        case 5:
                            if (!double.TryParse(this.dataGridView_Group4.CurrentRow.Cells[5].Value.ToString(), out newDouble))
                            {
                                return;
                            }
                            else
                            {
                                if (double.Parse(this.dataGridView_Group4.CurrentRow.Cells[4].Value.ToString()) >= double.Parse(this.dataGridView_Group4.CurrentRow.Cells[5].Value.ToString()))
                                {
                                    return;
                                }
                            }
                            doubleClickDataGridView(this.dataGridView_Group4, 4, 5);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 方法：双击实现
        /// </summary>
        /// <param name="dataGridView"></param>
        /// <param name="j"></param>
        /// <param name="column"></param>
        public void doubleClickDataGridView(DataGridView dataGridView, int j, int column)
        {
            double newDouble;
            if (double.TryParse(dataGridView[column, 0].Value.ToString(), out newDouble))
            {
                for (int i = 0; i < dataGridView.Rows.Count; i++)
                {
                    dataGridView[column, i].Value = dataGridView[column, 0].Value;
                }
            }
            dataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
            /*将数据写进单历中*/
            Module.Module_ViewConfig.Instance.ViewConfig.Remove("j");
            Module.Module_ViewConfig.Instance.SetShowConfig("j", dataGridView.DataSource as DataTable);
        }
        #endregion

        /* 事件：上下限不能相同 */
        private void dataGridView_Group1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            //可编辑的列
            if (e.ColumnIndex == 4)
            {
                double newDouble;
                if (!double.TryParse(e.FormattedValue.ToString(), out newDouble))
                {
                    e.Cancel = true;
                    MessageBox.Show(InterpretBase.textTran("此输入不合法，请重新输入"));
                }
                else
                {
                    if (double.Parse(e.FormattedValue.ToString()) >= double.Parse(dataGridView_Group1[5, e.RowIndex].Value.ToString()))
                    {
                        MessageBox.Show(InterpretBase.textTran("上下限不能相同/最小值不能大于最大值"));
                        e.Cancel = true;
                        return;
                    }
                }             
            }
            else if (e.ColumnIndex == 5)
            {
                double newDouble;
                if (!double.TryParse(e.FormattedValue.ToString(), out newDouble))
                {
                    e.Cancel = true;
                    MessageBox.Show(InterpretBase.textTran("此输入不合法，请重新输入"));
                }
                else
                {
                    if (double.Parse(e.FormattedValue.ToString()) <= double.Parse(dataGridView_Group1[4, e.RowIndex].Value.ToString()))
                    {
                        MessageBox.Show(InterpretBase.textTran("上下限不能相同/最小值不能大于最大值"));
                        e.Cancel = true;
                        return;
                    }
                }
                
            }
        }

        private void dataGridView_Group2_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {

            //可编辑的列
            if (e.ColumnIndex == 4)
            {
                double newDouble;
                if (!double.TryParse(e.FormattedValue.ToString(), out newDouble))
                {
                    e.Cancel = true;
                    MessageBox.Show(InterpretBase.textTran("此输入不合法，请重新输入"));
                }
                else
                {
                    if (double.Parse(e.FormattedValue.ToString()) >= double.Parse(dataGridView_Group2[5, e.RowIndex].Value.ToString()))
                    {
                        MessageBox.Show(InterpretBase.textTran("上下限不能相同/最小值不能大于最大值"));
                        e.Cancel = true;
                        return;
                    }
                }             
            }
            else if (e.ColumnIndex == 5)
            {
                double newDouble;
                if (!double.TryParse(e.FormattedValue.ToString(), out newDouble))
                {
                    e.Cancel = true;
                    MessageBox.Show(InterpretBase.textTran("此输入不合法，请重新输入"));
                }
                else
                {
                    if (double.Parse(e.FormattedValue.ToString()) <= double.Parse(dataGridView_Group2[4, e.RowIndex].Value.ToString()))
                    {
                        MessageBox.Show(InterpretBase.textTran("上下限不能相同/最小值不能大于最大值"));
                        e.Cancel = true;
                        return;
                    }
                }          
            }
        }

        private void dataGridView_Group3_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {

            //可编辑的列
            if (e.ColumnIndex == 4)
            {
                double newDouble;
                if (!double.TryParse(e.FormattedValue.ToString(), out newDouble))
                {
                    e.Cancel = true;
                    MessageBox.Show(InterpretBase.textTran("此输入不合法，请重新输入"));
                }
                else
                {
                    if (double.Parse(e.FormattedValue.ToString()) >= double.Parse(dataGridView_Group3[5, e.RowIndex].Value.ToString()))
                    {
                        MessageBox.Show(InterpretBase.textTran("上下限不能相同/最小值不能大于最大值"));
                        e.Cancel = true;
                        return;
                    }
                }                
            }
            else if (e.ColumnIndex == 5)
            {
                double newDouble;
                if (!double.TryParse(e.FormattedValue.ToString(), out newDouble))
                {
                    e.Cancel = true;
                    MessageBox.Show(InterpretBase.textTran("此输入不合法，请重新输入"));
                }
                else
                {
                    if (double.Parse(e.FormattedValue.ToString()) <= double.Parse(dataGridView_Group3[4, e.RowIndex].Value.ToString()))
                    {
                        MessageBox.Show(InterpretBase.textTran("上下限不能相同/最小值不能大于最大值"));
                        e.Cancel = true;
                        return;
                    }
                }             
            }
        }

        private void dataGridView_Group4_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {

            //可编辑的列
            if (e.ColumnIndex == 4)
            {
                double newDouble;
                if (!double.TryParse(e.FormattedValue.ToString(), out newDouble))
                {
                    e.Cancel = true;
                    MessageBox.Show(InterpretBase.textTran("此输入不合法，请重新输入"));
                }
                else
                {
                    if (double.Parse(e.FormattedValue.ToString()) >= double.Parse(dataGridView_Group4[5, e.RowIndex].Value.ToString()))
                    {
                        MessageBox.Show(InterpretBase.textTran("上下限不能相同/最小值不能大于最大值"));
                        e.Cancel = true;
                        return;
                    }

                }            
            }
            else if (e.ColumnIndex == 5)
            {
                double newDouble;
                if (!double.TryParse(e.FormattedValue.ToString(), out newDouble))
                {
                    e.Cancel = true;
                    MessageBox.Show(InterpretBase.textTran("此输入不合法，请重新输入"));
                }
                else
                {
                    if (double.Parse(e.FormattedValue.ToString()) <= double.Parse(dataGridView_Group4[4, e.RowIndex].Value.ToString()))
                    {
                        MessageBox.Show(InterpretBase.textTran("上下限不能相同/最小值不能大于最大值"));
                        e.Cancel = true;
                        return;
                    }
                }
                
            }
        }
    }
}
