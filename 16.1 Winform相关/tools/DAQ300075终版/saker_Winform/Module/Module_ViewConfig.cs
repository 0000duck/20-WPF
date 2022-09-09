using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace saker_Winform.Module
{
    public class Module_ViewConfig
    {
        /// <summary>
        /// 仪器管理，单例模式，内存唯一
        /// </summary>
        private static readonly Lazy<Module_ViewConfig> lazy = new Lazy<Module_ViewConfig>(() => new Module_ViewConfig());
        public static Module_ViewConfig Instance { get { return lazy.Value; } }

        public List<string> TableName = new List<string>();

        public Dictionary<string, List<Module_Parameter>> ViewConfig = new Dictionary<string, List<Module_Parameter>>();

     

        public bool IsInit = false;
        public Module_ViewConfig()
        {
            TableName.Add("Group_01");
            TableName.Add("Group_02");
            TableName.Add("Group_03");
            TableName.Add("Group_04");
        }

        /// <summary>
        /// 绑定tableName到内存中
        /// </summary>
        /// <param name="tableName"></param>
        public void SetTableName(string tableName)
        {
            TableName.Add(tableName);
        }

        /// <summary>
        /// 取出TableName
        /// </summary>
        /// <returns></returns>
        public List<string> GetTableName()
        {
            return TableName;
        }

        public void Remove(Module_Device device)
        {
            if(Module_ViewConfig.Instance.ViewConfig.Count>0)
            {
                for (int i = 1; i <= 4; i++)
                {
                    if (Module_ViewConfig.Instance.ViewConfig["1"].Where(item => item.Tag == device.GetChannel(i).Tag).Count() > 0)
                    {
                        Module_ViewConfig.Instance.ViewConfig["1"].Remove(Module_ViewConfig.Instance.ViewConfig["1"].Where(item => item.Tag == device.GetChannel(i).Tag).FirstOrDefault());
                        Module_ViewConfig.Instance.ViewConfig["2"].Remove(Module_ViewConfig.Instance.ViewConfig["2"].Where(item => item.Tag == device.GetChannel(i).Tag).FirstOrDefault());
                        Module_ViewConfig.Instance.ViewConfig["3"].Remove(Module_ViewConfig.Instance.ViewConfig["3"].Where(item => item.Tag == device.GetChannel(i).Tag).FirstOrDefault());
                        Module_ViewConfig.Instance.ViewConfig["4"].Remove(Module_ViewConfig.Instance.ViewConfig["4"].Where(item => item.Tag == device.GetChannel(i).Tag).FirstOrDefault());
                    }
                }
            }        
        }

        public void SetShowConfig(string groupName, DataTable dt)
        {
            List<Module_Parameter> list = new List<Module_Parameter>();
            try
            {
                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        Module_Parameter parameter = new Module_Parameter();
                        parameter.ID = dr["ID"].ToString();
                        parameter.No = dr["NO"].ToString();
                        parameter.Tag = dr["Tag"].ToString();
                        if (string.IsNullOrEmpty(dr["IsShow"].ToString()))
                        {
                            dr["IsShow"] = false;
                        }
                        parameter.IsShow = Convert.ToBoolean(dr["IsShow"].ToString());
                        parameter.Y = dr["Y"].ToString();
                        parameter.ScaleMin = dr["ScaleMin"].ToString();
                        parameter.ScaleMax = dr["ScaleMax"].ToString();
                        parameter.WaveColor = Color.FromArgb(int.Parse(dr["WaveColor"].ToString()));
                        parameter.WaveType = dr["WaveType"].ToString();
                        if(string.IsNullOrEmpty(dr["IsChoose"].ToString()))
                        {
                            dr["IsChoose"] = true;
                        }                      
                        parameter.IsChoose = Convert.ToBoolean(dr["IsChoose"]);
                        list.Add(parameter);
                    }
                    Instance.ViewConfig.Add(groupName, list);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return;
        }

        public void SetShowConfiUpdate(string groupName, DataTable dt)
        {
            List<Module_Parameter> list = new List<Module_Parameter>();
            if (ViewConfig.ContainsKey(groupName))
            {
                List<Module_Parameter> listOrig = Instance.ViewConfig[groupName];
                try
                {
                    if (dt != null)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            var query = from item in listOrig
                                        where item.Tag == dr["Tag"].ToString()
                                        select item.Tag;
                            if (string.IsNullOrEmpty(query.ToString()))
                            {
                                break;
                            }
                        }
                        foreach (DataRow dr in dt.Rows)
                        {

                            Module_Parameter parameter = new Module_Parameter();
                            parameter.ID = dr["ID"].ToString();
                            parameter.No = dr["NO"].ToString();
                            parameter.Tag = dr["Tag"].ToString();
                            if (string.IsNullOrEmpty(dr["IsShow"].ToString()))
                            {
                                dr["IsShow"] = false;
                            }
                            parameter.IsShow = Convert.ToBoolean(dr["IsShow"].ToString());
                            parameter.Y = dr["Y"].ToString();
                            parameter.ScaleMin = dr["ScaleMin"].ToString();
                            parameter.ScaleMax = dr["ScaleMax"].ToString();
                            parameter.WaveColor = Color.FromArgb(int.Parse(dr["WaveColor"].ToString()));
                            parameter.WaveType = dr["WaveType"].ToString();                           
                            list.Add(parameter);
                        }
                        Instance.ViewConfig.Add(groupName, list);
                    }
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }
        }
        public void Init(DataTable dataTable_DeviceChannel)
        {
            /* 更新单例 */
            if (dataTable_DeviceChannel != null)
            {

                DataRow[] dataRows_DeviceChannel = dataTable_DeviceChannel.AsEnumerable().ToArray();
                if (dataRows_DeviceChannel.Count() > 0)//DataRows转换为DataTable
                {
                    DataTable dataTableValidChannel = dataRows_DeviceChannel.CopyToDataTable();
                    DataTable dataTable_ViewConfig1 = dataTableValidChannel.DefaultView.ToTable(false, new string[] { "Tag" });
                    dataTable_ViewConfigProp(dataTable_ViewConfig1);                  
                    SetShowConfig("1", dataTable_ViewConfig1);
                    SetShowConfig("2", dataTable_ViewConfig1);
                    SetShowConfig("3", dataTable_ViewConfig1);
                    SetShowConfig("4", dataTable_ViewConfig1);

                }
                else
                {
                    /*将数据写进单历中*/
                    Module.Module_ViewConfig.Instance.ViewConfig.Clear();//清除单例中的数据
                }
            }
            IsInit = true;
        }
        #region 私有方法：给数据源添加列(将有效的Tag从通道配置的DataTable中获取
        /// <summary>
        /// 私有方法：给数据源添加列(将有效的Tag从通道配置的DataTable中获取
        /// </summary>
        /// <param name="dataTable"></param>
        private void dataTable_ViewConfigProp(DataTable dataTable)//初始Tag有效一列或未点击应用拿到生成的全表
        {
            if (dataTable.Columns.Count == 1)//初始一列Tag
            {
                dataTable.Columns.Add("ID");
                dataTable.Columns.Add("IsShow");
                dataTable.Columns.Add("No");
                dataTable.Columns.Add("Y");
                dataTable.Columns.Add("ScaleMin");
                dataTable.Columns.Add("ScaleMax");
                dataTable.Columns.Add("WaveColor");
                dataTable.Columns.Add("WaveType");
                dataTable.Columns["Tag"].SetOrdinal(2);
                dataTable.Columns.Add("IsChoose");
                if (dataTable.Rows.Count != 0)
                {
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        dataTable.Rows[i]["ID"] = i.ToString();
                       // dataTable.Rows[i]["IsShow"] = true;
                        dataTable.Rows[i]["No"] = "W" + (i + 1).ToString();
                        dataTable.Rows[i]["Y"] = "Y" + (i + 1).ToString();
                        dataTable.Rows[i]["ScaleMin"] = "-10";
                        dataTable.Rows[i]["ScaleMax"] = "10";
                        dataTable.Rows[i]["WaveColor"] = Global.CGlobalColor.lineColor[i % (Global.CGlobalColor.lineColor.Count)].ToArgb();
                        dataTable.Rows[i]["WaveType"] = "趋势图";
                        dataTable.Rows[i]["IsChoose"] = false;
                        dataTable.Rows[i]["IsShow"] = true;
                    }
                }
            }
        }

        #endregion

            ///// <summary>
            ///// 绑定group和数据源到内存
            ///// </summary>
            ///// <param name="groupName"></param>
            ///// <param name="dt"></param>
            //public void SetShowConfig(string groupName, DataTable dt)
            //{
            //    List<Module_Parameter> list = new List<Module_Parameter>();
            //    if (ViewConfig.ContainsKey(groupName))
            //    {
            //        List<Module_Parameter> listOrig = Instance.ViewConfig[groupName];
            //        try
            //        {
            //            if (dt != null)
            //            {
            //                foreach (DataRow dr in dt.Rows)
            //                {
            //                    var query = from item in listOrig
            //                                where item.Tag == dr["Tag"].ToString()
            //                                select item.Tag;
            //                    if(!string.IsNullOrEmpty(query.ToString()))
            //                    {
            //                        break;
            //                    }
            //                }

            //                foreach (DataRow dr in dt.Rows)
            //                {


            //                    Module_Parameter parameter = new Module_Parameter();
            //                    parameter.ID = dr["ID"].ToString();
            //                    parameter.No = dr["NO"].ToString();
            //                    parameter.Tag = dr["Tag"].ToString();
            //                    if (string.IsNullOrEmpty(dr["IsShow"].ToString()))
            //                    {
            //                        dr["IsShow"] = false;
            //                    }
            //                    parameter.IsShow = Convert.ToBoolean(dr["IsShow"].ToString());
            //                    parameter.Y = dr["Y"].ToString();
            //                    parameter.ScaleMin = dr["ScaleMin"].ToString();
            //                    parameter.ScaleMax = dr["ScaleMax"].ToString();
            //                    parameter.WaveColor = Color.FromArgb(int.Parse(dr["WaveColor"].ToString()));
            //                    parameter.WaveType = dr["WaveType"].ToString();
            //                    list.Add(parameter);
            //                }
            //                Instance.ViewConfig.Add(groupName, list);
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            throw (ex);
            //        }
            //    }

            //    return;
            //}

            public DataTable GetNullTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("No");
            dt.Columns.Add("Tag");
            dt.Columns.Add("IsShow");
            dt.Columns.Add("Y");
            dt.Columns.Add("ScaleMin");
            dt.Columns.Add("ScaleMax");
            dt.Columns.Add("WaveColor");
            dt.Columns.Add("WaveType");
            dt.Columns.Add("IsChoose");
            return dt;
        }

        /// <summary>
        /// 根据对应的groupName 返回对应的 数据源
        /// </summary>
        /// <param name="groupName"></param>
        public DataTable GetShowConfigByGroup(string groupName)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("No");
            dt.Columns.Add("Tag");
            dt.Columns.Add("IsShow");          
            dt.Columns.Add("ScaleMin");
            dt.Columns.Add("ScaleMax");
            dt.Columns.Add("WaveColor");
            dt.Columns.Add("WaveType");
            dt.Columns.Add("Y");
            dt.Columns.Add("IsChoose");
            try
            {
                if (ViewConfig.ContainsKey(groupName))
                {
                    List<Module_Parameter> list = Instance.ViewConfig[groupName].Where(item=>item.IsChoose==true).OrderBy(item=>item.Tag).ToList();
                    for (int i = 0; i < list.Count; i++)
                    {
                        var item = list[i];
                        DataRow dr = dt.NewRow();
                        dr["ID"] = (i+1).ToString();
                        dr["NO"] = "W" + (i+1).ToString();
                        dr["Tag"] = item.Tag;
                        dr["IsShow"] = item.IsShow;
                        dr["ScaleMin"] = item.ScaleMin;
                        dr["ScaleMax"] = item.ScaleMax;
                        dr["WaveColor"] = item.WaveColor.ToArgb();
                        dr["Y"] = "Y"+(i+1).ToString();
                        dr["WaveType"] = item.WaveType;
                        dr["IsChoose"] = item.IsChoose;
                        dt.Rows.Add(dr);
                    }
                }
                else
                {
                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return dt;
        }
        public DataTable Get4GroupTable()
        {           
            try
            {              
                DataTable dt = new DataTable();
                dt.Columns.Add("GUID");
                dt.Columns.Add("ProjectGUID");
                dt.Columns.Add("CollectGUID");
                dt.Columns.Add("GroupName");
                dt.Columns.Add("Tag");
                dt.Columns.Add("ScaleMin");
                dt.Columns.Add("ScaleMax");
                dt.Columns.Add("WaveColor");             
                dt.Columns.Add("WaveType");
                dt.Columns.Add("IsShow");
                dt.Columns.Add("ID");
                dt.Columns.Add("No");
                dt.Columns.Add("Y");
                dt.Columns.Add("IsChoose");
                //dt.Columns.Add("StartTime");
                dt.Columns.Add("CreateTime");               
                foreach (var group in Instance.ViewConfig)
                {                                                   
                    foreach (var para in group.Value)
                    {
                        DataRow dr = dt.NewRow();
                        dr["GUID"] = Guid.NewGuid().ToString();
                        dr["ProjectGUID"] = Module_DeviceManage.Instance.ProjectGUID;
                        dr["CollectGUID"] = Module_DeviceManage.Instance.GUID;
                        dr["GroupName"] = group.Key.ToString();
                        dr["Tag"] = para.Tag;
                        dr["ScaleMin"] = para.ScaleMin.ToString();
                        dr["ScaleMax"] = para.ScaleMax.ToString();
                        dr["WaveColor"] = para.WaveColor.ToArgb().ToString();
                        dr["WaveType"] = para.WaveType.ToString();
                        dr["IsShow"] = para.IsShow.ToString();
                        //dr["StartTime"] = Module_DeviceManage.Instance.StartTime;
                        dr["CreateTime"] = DateTime.Now.ToString();
                        dr["IsChoose"] = para.IsChoose;
                        dr["ID"] = para.ID;
                        dr["No"] = para.No;
                        dr["Y"] = para.Y;
                        dt.Rows.Add(dr);
                    }                                     
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw (ex);
            }          
        }
    }
}