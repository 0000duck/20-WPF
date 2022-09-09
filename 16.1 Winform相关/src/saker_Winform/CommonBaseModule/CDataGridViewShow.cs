using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace saker_Winform.CommonBaseModule
{
    public static class CDataGridViewShow
    {

        /// <summary>
        /// 双缓冲，解决闪烁问题
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="flag"></param>
        public static void DoubleBufferedDataGirdView(this DataGridView dgv, bool flag)
        {
            Type dgvType = dgv.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgv, flag, null);
        }

        public static List<string> dataTableCompare = new List<string>();
        /// <summary>
        /// 列宽根据单元格的内容自适应
        /// </summary>
        /// <param name="dataGridView"></param>
        public static void AutoSizeColumn(System.Windows.Forms.DataGridView dataGridView)
        {
            for (int i = 0; i < dataGridView.ColumnCount; i++)
            {
                dataGridView.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
        }

        /// <summary>
        /// 菜单全选或取消全选
        /// </summary>
        /// <param name="dataGridView"></param>
        /// <param name="choose"></param>
        /// <param name="value"></param>
        public static void AllChooseOrNot(System.Windows.Forms.DataGridView dataGridView,bool choose,int value)
        {
            if(choose&& (dataGridView.RowCount!=0))
            {
                for (int i = 0; i < dataGridView.RowCount; i++)
                {
                    dataGridView.Rows[i].Cells[value].Value = "True";//value列全开
                   
                }
            }
            else
            {
                for (int i = 0; i < dataGridView.RowCount; i++)
                {
                    dataGridView.Rows[i].Cells[value].Value = "False";//value列全开
                }
            }  
        }

        public static DataTable DataTableAllChooseOrNot(System.Windows.Forms.DataGridView dataGridView, bool choose, int value1,int value2)
        {
            if (choose && (dataGridView.RowCount != 0))
            {
                for (int i = 0; i < dataGridView.RowCount; i++)
                {
                    dataGridView.Rows[i].Cells[value1].Value = "True";//value列全开
                    dataGridView.Rows[i].Cells[value2].Value = "True";//value列全开
                }
            }
            else
            {
                for (int i = 0; i < dataGridView.RowCount; i++)
                {
                    dataGridView.Rows[i].Cells[value1].Value = "False";//value列全开
                    dataGridView.Rows[i].Cells[value2].Value = "False";//value列全开
                }
            }
            DataTable dt = dataGridView.DataSource as DataTable;
            return dt;
        }


        ///   <summary>
        ///   比较两个DataTable内容是否相等，先是比数量，数量相等就比内容
        ///   </summary>
        ///   <param   name= "dtA "> </param>
        ///   <param   name= "dtB "> </param>
        public static void CompareDataTable(DataTable dtA, DataTable dtB)
        {
            if (dtA.Rows.Count == dtB.Rows.Count)
            {
                if (CompareColumn(dtA.Columns, dtB.Columns))
                {
                    int i = 0;
                    int j = 0;
                    //比内容
                    for (i = 0; i < dtA.Rows.Count; i++)
                    {
                        for (j = 0; j < dtA.Columns.Count; j++)
                        {
                            if (!dtA.Rows[i][j].Equals(dtB.Rows[i][j]))
                            {
                                dataTableCompare.Add(i.ToString() + "," + j.ToString());
                            }
                        }
                    }                 
                }             
            }      
        }
        ///   <summary>
        ///   比较两个字段集合是否名称,数据类型一致
        ///   </summary>
        ///   <param   name= "dcA "> </param>
        ///   <param   name= "dcB "> </param>
        ///   <returns> </returns>
        public static bool CompareColumn(System.Data.DataColumnCollection dcA, System.Data.DataColumnCollection dcB)
        {
            if (dcA.Count == dcB.Count)
            {
                foreach (DataColumn dc in dcA)
                {
                    //找相同字段名称
                    if (dcB.IndexOf(dc.ColumnName) > -1)
                    {
                        //测试数据类型
                        if (dc.DataType != dcB[dcB.IndexOf(dc.ColumnName)].DataType)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }



    }
}
