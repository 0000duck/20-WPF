using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace saker_Winform.DataBase
{
    /// <summary>
    /// 数据访问抽象基础类
    ///  Copyright (C) Maticsoft
    /// </summary>
    public  class DbHelperSql
    {
        //数据库连接字符串(web.config来配置)，多数据库可使用DbHelperSQLP来实现.

   //     private static string _connectionString = "server = 172.18.8.217;database=Saker;uid=Sa;pwd=Rigol1998;";
        private static string _connectionString = ConfigurationManager.AppSettings["ConnectionString"];
        public static string ConnectionString = _connectionString;

        /// <summary>
        /// 重新设置连接串根据线程参数
        /// </summary>
        public static void ReSetConnectionByThreadPara()
        {
            if (!string.IsNullOrEmpty(_connectionString))
            {
                //string[] para = TPublicTools.DataBaseConnectionStr.Split(new[] {';'},
                //                                                         StringSplitOptions.RemoveEmptyEntries);
                //string pwd = para[para.Length - 1].Remove(0, 4);
                //string pwdEncode = DbHelperTools.Encode(pwd);
               //ConnectionString = TPublicTools.DataBaseConnectionStr.Replace(pwd, pwdEncode);
                //线程参数为空时,表示测试环境　连接串至空
                _connectionString = string.IsNullOrEmpty(_connectionString) ? "" : _connectionString;
            }
        }

        public static TransactionSwitch TransactionSwitch;

        #region 公用方法

        /// <summary>
        ///     判断是否存在某表的某个字段
        /// </summary>
        /// <param name="tableName">表名称</param>
        /// <param name="columnName">列名称</param>
        /// <returns>是否存在</returns>
        public static bool ColumnExists(string tableName, string columnName)
        {
            string sql = "select count(1) from syscolumns where [id]=object_id('" + tableName + "') and [name]='" +
                         columnName + "'";
            object res = GetSingle(sql);
            if (res == null)
            {
                return false;
            }
            return Convert.ToInt32(res) > 0;
        }

        public static int GetMaxID(string FieldName, string TableName)
        {
            string strsql = "select max(" + FieldName + ")+1 from " + TableName;
            object obj = GetSingle(strsql);
            if (obj == null)
            {
                return 1;
            }
            else
            {
                return int.Parse(obj.ToString());
            }
        }

        /// <summary>
        ///     数据是否存在
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public static bool Exists(string strSql)
        {
            object obj = GetSingle(strSql);
            int cmdresult;
            if ((Equals(obj, null)) || (Equals(obj, DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString()); //也可能=0
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        ///     表是否存在
        /// </summary>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public static bool TabExists(string TableName)
        {
            string strsql = "select count(*) from sysobjects where id = object_id(N'[" + TableName +
                            "]') and OBJECTPROPERTY(id, N'IsUserTable') = 1";
            //string strsql = "SELECT count(*) FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[" + TableName + "]') AND type in (N'U')";
            object obj = GetSingle(strsql);
            int cmdresult;
            if ((Equals(obj, null)) || (Equals(obj, DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        ///     数据是否存在
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public static bool Exists(string strSql, params SqlParameter[] cmdParms)
        {
            object obj = GetSingle(strSql, cmdParms);
            int cmdresult;
            if ((Equals(obj, null)) || (Equals(obj, DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        #endregion

        #region  执行简单SQL语句

        /// <summary>
        ///     执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(string SQLString)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                using (var cmd = new SqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (SqlException e)
                    {
                        connection.Close();
                        throw e;
                    }
                }
            }
        }

        /// <summary>
        ///     执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString"></param>
        /// <param name="Times">超时时间(毫秒)</param>
        /// <returns></returns>
        public static int ExecuteSqlByTime(string SQLString, int Times)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                using (var cmd = new SqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        cmd.CommandTimeout = Times;
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (SqlException e)
                    {
                        connection.Close();
                        throw e;
                    }
                }
            }
        }

        /// <summary>
        ///     执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">多条SQL语句</param>
        public static int ExecuteSqlTran(List<String> SQLStringList)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                var cmd = new SqlCommand();
                cmd.Connection = conn;
                SqlTransaction tx = conn.BeginTransaction();
                cmd.Transaction = tx;
                try
                {
                    int count = 0;
                    for (int n = 0; n < SQLStringList.Count; n++)
                    {
                        string strsql = SQLStringList[n];
                        if (strsql.Trim().Length > 1)
                        {
                            cmd.CommandText = strsql;
                            count += cmd.ExecuteNonQuery();
                        }
                    }
                    tx.Commit();
                    return count;
                }
                catch
                {
                    tx.Rollback();
                    return 0;
                }
            }
        }
        #region 仪器相关数据插入到数据库中
        public static void  InsertDeviceInfo(Module.Module_DeviceManage deviceManage)
        {
            //仪器命令
            string sql = "INSERT INTO [Config_Device_All] (GUID,ProjectGUID,TriggerSource,TriggerMode,HorizontalOffset,HorizontalTimebase,TriggerLevel,Memdepth,Holdoff,WaveTableName,IP,CreateTime) " +
                "VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}',GETDATE())";
            string waveTableName = "Data_Waveform_Info_" + DateTime.Now.ToString("yyyyMMdd_hhmmss");
            Module.Module_DeviceManage.Instance.WaveTableName = waveTableName;
            sql = string.Format(sql, deviceManage.GetGUID(), deviceManage.ProjectGUID,deviceManage.TriggerSource, deviceManage.TriggerMode,deviceManage.HorizontalOffset,deviceManage.HorizontalTimebase,
                deviceManage.TriggerLevel,deviceManage.MemDepth,deviceManage.HoldOff,waveTableName,deviceManage.IP);                  
            try
            {
                //本次测量信息，设备配置，和波形数据存入那张表中
                ExecuteSql(sql);
                //设备信息，批量，数据拼装
                SqlBulkCopyByDatatable("Data_Device_Info", deviceManage.GetDeviceDataTable());
                //通道信息，批量，数据拼装
                SqlBulkCopyByDatatable("Config_Channel", deviceManage.GetAllChannelConfigDataTable());
                sql = "SELECT * INTO " + waveTableName + " FROM Data_Waveform_Info";
                ExecuteSql(sql);
                
            }
            catch (SqlException ex) 
            { 
                throw (ex); 
            }                                          
        }
        /// <summary>
        /// 插入记录配置到数据库
        /// </summary>
        /// <param name="dic"></param>
        public static void InsertRecordInfo(Dictionary<string, string> dic)
        {
            try
            {
                string sql = "INSERT INTO [Config_Record] (GUID,ProjectGUID,CollectGUID,CollectCondition,AbsTime,RecordLocation,FileType,FileLocation,FileNameRule,FileNameRuleDesc,IsAddDateTime,CreateTime) " +
                  "Values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}',GETDATE())";
                sql = string.Format(sql, Guid.NewGuid().ToString(), dic["ProjectGUID"], dic["CollectGUID"],dic["CollectCondition"], dic["AbsTime"], dic["RecordLocation"],
                    dic["FileType"], dic["FileLocation"], dic["FileNameRule"], dic["FileNameRuleDesc"], dic["IsAddDateTime"]);
                ExecuteSql(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }
        public static void InsertViewConfig(Module.Module_ViewConfig viewConfig)
        {
            //仪器命令         
            try
            {
                
                //设备信息，批量，数据拼装
                SqlBulkCopyByDatatable("Config_View", viewConfig.Get4GroupTable());
                //通道信息，批量，数据拼装
               
                       
            }
            catch (SqlException ex)
            {
                throw (ex);
            }
        }
        public static void GetDeviceInfo(Module.Module_DeviceManage deviceManage)
        {
            
        }
        #endregion
        /// <summary>
        ///     执行带一个存储过程参数的的SQL语句。
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <param name="content">参数内容,比如一个字段是格式复杂的文章，有特殊符号，可以通过这个方式添加</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(string SQLString, string content)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var cmd = new SqlCommand(SQLString, connection);
                var myParameter = new SqlParameter("@content", SqlDbType.NText);
                myParameter.Value = content;
                cmd.Parameters.Add(myParameter);
                try
                {
                    connection.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows;
                }
                catch (SqlException e)
                {
                    throw e;
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }
        }

        /// <summary>
        ///     执行带一个存储过程参数的的SQL语句。
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <param name="content">参数内容,比如一个字段是格式复杂的文章，有特殊符号，可以通过这个方式添加</param>
        /// <returns>影响的记录数</returns>
        public static object ExecuteSqlGet(string SQLString, string content)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var cmd = new SqlCommand(SQLString, connection);
                var myParameter = new SqlParameter("@content", SqlDbType.NText);
                myParameter.Value = content;
                cmd.Parameters.Add(myParameter);
                try
                {
                    connection.Open();
                    object obj = cmd.ExecuteScalar();
                    if ((Equals(obj, null)) || (Equals(obj, DBNull.Value)))
                    {
                        return null;
                    }
                    else
                    {
                        return obj;
                    }
                }
                catch (SqlException e)
                {
                    throw e;
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }
        }

        public static object ExecuteSqlGet(string SQLString, SqlParameter[] myParameter)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var cmd = new SqlCommand(SQLString, connection);
                foreach (SqlParameter sqlParameter in myParameter)
                {
                    cmd.Parameters.Add(sqlParameter);
                }
                try
                {
                    connection.Open();
                    object obj = cmd.ExecuteScalar();
                    if ((Equals(obj, null)) || (Equals(obj, DBNull.Value)))
                    {
                        return null;
                    }
                    else
                    {
                        return obj;
                    }
                }
                catch (SqlException e)
                {
                    throw e;
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }
        }

        /// <summary>
        ///  向数据库里插入图像格式的字段(和上面情况类似的另一种实例)
        /// </summary>
        /// <param name="strSQL">SQL语句</param>
        /// <param name="fs">图像字节,数据库的字段类型为image的情况</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSqlInsertImg(string strSQL, byte[] fs)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var cmd = new SqlCommand(strSQL, connection);
                var myParameter = new SqlParameter("@WaveData", SqlDbType.Image);
                myParameter.Value = fs;
                cmd.Parameters.Add(myParameter);
                try
                {
                    connection.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows;
                }
                catch (SqlException e)
                {
                    throw e;
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }
        }

        /// <summary>
        ///     执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public static object GetSingle(string SQLString)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                using (var cmd = new SqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        object obj = cmd.ExecuteScalar();
                        if ((Equals(obj, null)) || (Equals(obj, DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (SqlException e)
                    {
                        connection.Close();
                        throw e;
                    }
                }
            }
        }

        public static object GetSingle(string SQLString, int Times)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                using (var cmd = new SqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        cmd.CommandTimeout = Times;
                        object obj = cmd.ExecuteScalar();
                        if ((Equals(obj, null)) || (Equals(obj, DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (SqlException e)
                    {
                        connection.Close();
                        throw e;
                    }
                }
            }
        }

        /// <summary>
        ///     执行查询语句，返回SqlDataReader ( 注意：调用该方法后，一定要对SqlDataReader进行Close )
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <returns>SqlDataReader</returns>
        public static SqlDataReader ExecuteReader(string strSQL)
        {
            var connection = new SqlConnection(ConnectionString);
            var cmd = new SqlCommand(strSQL, connection);
            try
            {
                connection.Open();
                SqlDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return myReader;
            }
            catch (SqlException e)
            {
                throw e;
            }
        }

       /// <summary>
        ///  查询系统当前时间
       /// </summary>
       /// <returns></returns>
        public static DateTime QueryCurrentDateTime()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var ds = new DataSet();
                try
                {
                    connection.Open();
                    var command = new SqlDataAdapter(" select GETDATE() ", connection);
                    command.Fill(ds, "ds");
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                return Convert.ToDateTime(ds.Tables[0].Rows[0][0]);
            }
        }

        /// <summary>
        ///  查询系统当前格式威治时间
        /// </summary>
        /// <returns></returns>
        public static DateTime QueryCurrentDateTimeGmt()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var ds = new DataSet();
                try
                {
                    connection.Open();
                    var command = new SqlDataAdapter(" select DATEADD(hh,-8,getdate()) ", connection);
                    command.Fill(ds, "ds");
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                return Convert.ToDateTime(ds.Tables[0].Rows[0][0]);
            }
        }
        /// <summary>
        ///     执行查询语句，返回DataTable
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataTable</returns>
        public static DataTable QueryDt(string SQLString)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var ds = new DataSet();
                try
                {
                    connection.Open();
                    var command = new SqlDataAdapter(SQLString, connection);
                    command.Fill(ds, "ds");
                }
                catch (SqlException ex)
                {                   
                    throw new Exception(ex.Message);
                }
                return ds.Tables.Count > 0 ? ds.Tables[0] : null;
            }
        }

        /// <summary>
        ///     执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public static DataSet Query(string SQLString)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var ds = new DataSet();
                try
                {
                    connection.Open();
                    var command = new SqlDataAdapter(SQLString, connection);
                    command.Fill(ds, "ds");
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                return ds;
            }
        }

        /// <summary>
        /// 验证表是否存在
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public static DataTable testFormIsExist(string queryString)
        {
            // 验证表是否存在
            string testFormIsExist = " SELECT COUNT(*) from dbo.sysobjects WHERE name= '{0}'";
            testFormIsExist = string.Format(testFormIsExist, queryString);
            DataTable dtWaveData = DbHelperSql.QueryDt(testFormIsExist);
            return dtWaveData;          
        }


        public static DataSet Query(string SQLString, int Times)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var ds = new DataSet();
                try
                {
                    connection.Open();
                    var command = new SqlDataAdapter(SQLString, connection);
                    command.SelectCommand.CommandTimeout = Times;
                    command.Fill(ds, "ds");
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                return ds;
            }
        }

        #endregion

        #region 加载参数

        public static SqlParameter[] LoadSqlParameter( SqlParameter[] parameters,object[] paraValue)
        {
            for (int i = 0; i < paraValue.Length; i++)
            {
                parameters[i].Value = paraValue[i];
            }
            return parameters;
        }

        #endregion

        #region 执行带参数的SQL语句

        /// <summary>
        ///     执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(string SQLString, params SqlParameter[] cmdParms)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                using (var cmd = new SqlCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                        int rows = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        return rows;
                    }
                    catch (SqlException e)
                    {
                        throw e;
                    }
                }
            }
        }


        /// <summary>
        ///     执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">SQL语句的哈希表（key为sql语句，value是该语句的SqlParameter[]）</param>
        public static void ExecuteSqlTran(Hashtable SQLStringList)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    var cmd = new SqlCommand();
                    try
                    {
                        //循环
                        foreach (DictionaryEntry myDE in SQLStringList)
                        {
                            string cmdText = myDE.Key.ToString();
                            var cmdParms = (SqlParameter[]) myDE.Value;
                            PrepareCommand(cmd, conn, trans, cmdText, cmdParms);
                            int val = cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                        }
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        #region 事务

        /// <summary>
        ///     执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">SQL语句的哈希表（key为sql语句，value是该语句的SqlParameter[]）</param>
        public static int ExecuteSqlTran(List<CommandInfo> cmdList)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    var cmd = new SqlCommand();
                    try
                    {
                        int count = 0;
                        //循环
                        foreach (CommandInfo myDE in cmdList)
                        {
                            string cmdText = myDE.CommandText;
                            var cmdParms = (SqlParameter[]) myDE.Parameters;
                            PrepareCommand(cmd, conn, trans, cmdText, cmdParms);

                            if (myDE.EffentNextType == EffentNextType.WhenHaveContine ||
                                myDE.EffentNextType == EffentNextType.WhenNoHaveContine)
                            {
                                if (myDE.CommandText.ToLower().IndexOf("count(") == -1)
                                {
                                    trans.Rollback();
                                    return 0;
                                }

                                object obj = cmd.ExecuteScalar();
                                bool isHave = false;
                                if (obj == null && obj == DBNull.Value)
                                {
                                    isHave = false;
                                }
                                isHave = Convert.ToInt32(obj) > 0;

                                if (myDE.EffentNextType == EffentNextType.WhenHaveContine && !isHave)
                                {
                                    trans.Rollback();
                                    return 0;
                                }
                                if (myDE.EffentNextType == EffentNextType.WhenNoHaveContine && isHave)
                                {
                                    trans.Rollback();
                                    return 0;
                                }
                                continue;
                            }
                            int val = cmd.ExecuteNonQuery();
                            count += val;
                            if (myDE.EffentNextType == EffentNextType.ExcuteEffectRows && val == 0)
                            {
                                trans.Rollback();
                                return 0;
                            }
                            cmd.Parameters.Clear();
                        }
                        trans.Commit();
                        return count;
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        ///     执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">SQL语句的哈希表（key为sql语句，value是该语句的SqlParameter[]）</param>
        public static void ExecuteSqlTranWithIndentity(List<CommandInfo> SQLStringList)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    var cmd = new SqlCommand();
                    try
                    {
                        int indentity = 0;
                        //循环
                        foreach (CommandInfo myDE in SQLStringList)
                        {
                            string cmdText = myDE.CommandText;
                            var cmdParms = (SqlParameter[]) myDE.Parameters;
                            foreach (SqlParameter q in cmdParms)
                            {
                                if (q.Direction == ParameterDirection.InputOutput)
                                {
                                    q.Value = indentity;
                                }
                            }
                            PrepareCommand(cmd, conn, trans, cmdText, cmdParms);
                            int val = cmd.ExecuteNonQuery();
                            foreach (SqlParameter q in cmdParms)
                            {
                                if (q.Direction == ParameterDirection.Output)
                                {
                                    indentity = Convert.ToInt32(q.Value);
                                }
                            }
                            cmd.Parameters.Clear();
                        }
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        ///// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">SQL语句的哈希表（key为sql语句，value是该语句的SqlParameter[]）</param>
        public static void ExecuteSqlTranWithIndentity(Hashtable SQLStringList)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    var cmd = new SqlCommand();
                    try
                    {
                        int indentity = 0;
                        //循环
                        foreach (DictionaryEntry myDE in SQLStringList)
                        {
                            string cmdText = myDE.Key.ToString();
                            var cmdParms = (SqlParameter[]) myDE.Value;
                            foreach (SqlParameter q in cmdParms)
                            {
                                if (q.Direction == ParameterDirection.InputOutput)
                                {
                                    q.Value = indentity;
                                }
                            }
                            PrepareCommand(cmd, conn, trans, cmdText, cmdParms);
                            int val = cmd.ExecuteNonQuery();
                            foreach (SqlParameter q in cmdParms)
                            {
                                if (q.Direction == ParameterDirection.Output)
                                {
                                    indentity = Convert.ToInt32(q.Value);
                                }
                            }
                            cmd.Parameters.Clear();
                        }
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        #endregion

        /// <summary>
        ///     执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public static object GetSingle(string SQLString, params SqlParameter[] cmdParms)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                using (var cmd = new SqlCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                        object obj = cmd.ExecuteScalar();
                        cmd.Parameters.Clear();
                        if ((Equals(obj, null)) || (Equals(obj, DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (SqlException e)
                    {
                        throw e;
                    }
                }
            }
        }

        /// <summary>
        ///     执行查询语句，返回SqlDataReader ( 注意：调用该方法后，一定要对SqlDataReader进行Close )
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <returns>SqlDataReader</returns>
        public static SqlDataReader ExecuteReader(string SQLString, params SqlParameter[] cmdParms)
        {
            var connection = new SqlConnection(ConnectionString);
            var cmd = new SqlCommand();
            try
            {
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                SqlDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return myReader;
            }
            catch (SqlException e)
            {
                throw e;
            }
            //			finally
            //			{
            //				cmd.Dispose();
            //				connection.Close();
            //			}	
        }

        /// <summary>
        ///     执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public static DataSet Query(string SQLString, params SqlParameter[] cmdParms)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var cmd = new SqlCommand();
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                using (var da = new SqlDataAdapter(cmd))
                {
                    var ds = new DataSet();
                    try
                    {
                        da.Fill(ds, "ds");
                        cmd.Parameters.Clear();
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    return ds;
                }
            }
        }


        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, string cmdText,
                                           SqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text; //cmdType;
            if (cmdParms != null)
            {
                foreach (SqlParameter parameter in cmdParms)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput ||
                         parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(parameter);
                }
            }
        }

        #endregion

        #region 存储过程操作

        /// <summary>
        ///     执行存储过程，返回SqlDataReader ( 注意：调用该方法后，一定要对SqlDataReader进行Close )
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>SqlDataReader</returns>
        public static SqlDataReader RunProcedure(string storedProcName, IDataParameter[] parameters)
        {
            var connection = new SqlConnection(ConnectionString);
            SqlDataReader returnReader;
            connection.Open();
            SqlCommand command = BuildQueryCommand(connection, storedProcName, parameters);
            command.CommandType = CommandType.StoredProcedure;
            returnReader = command.ExecuteReader(CommandBehavior.CloseConnection);
            return returnReader;
        }


        /// <summary>
        ///     执行存储过程
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <param name="tableName">DataSet结果中的表名</param>
        /// <returns>DataSet</returns>
        public static DataSet RunProcedure(string storedProcName, IDataParameter[] parameters, string tableName)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var dataSet = new DataSet();
                connection.Open();
                var sqlDA = new SqlDataAdapter();
                sqlDA.SelectCommand = BuildQueryCommand(connection, storedProcName, parameters);
                sqlDA.Fill(dataSet, tableName);
                connection.Close();
                return dataSet;
            }
        }

        public static DataSet RunProcedure(string storedProcName, IDataParameter[] parameters, string tableName,
                                           int Times)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var dataSet = new DataSet();
                connection.Open();
                var sqlDA = new SqlDataAdapter();
                sqlDA.SelectCommand = BuildQueryCommand(connection, storedProcName, parameters);
                sqlDA.SelectCommand.CommandTimeout = Times;
                sqlDA.Fill(dataSet, tableName);
                connection.Close();
                return dataSet;
            }
        }


        /// <summary>
        ///     构建 SqlCommand 对象(用来返回一个结果集，而不是一个整数值)
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>SqlCommand</returns>
        private static SqlCommand BuildQueryCommand(SqlConnection connection, string storedProcName,
                                                    IDataParameter[] parameters)
        {
            var command = new SqlCommand(storedProcName, connection);
            command.CommandType = CommandType.StoredProcedure;
            foreach (SqlParameter parameter in parameters)
            {
                if (parameter != null)
                {
                    // 检查未分配值的输出参数,将其分配以DBNull.Value.
                    if ((parameter.Direction == ParameterDirection.InputOutput ||
                         parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    command.Parameters.Add(parameter);
                }
            }

            return command;
        }

        /// <summary>
        ///     执行存储过程，返回影响的行数
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <param name="rowsAffected">影响的行数</param>
        /// <returns></returns>
        public static int RunProcedure(string storedProcName, IDataParameter[] parameters, out int rowsAffected)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                int result;
                connection.Open();
                SqlCommand command = BuildIntCommand(connection, storedProcName, parameters);
                rowsAffected = command.ExecuteNonQuery();
                result = (int) command.Parameters["ReturnValue"].Value;
                //Connection.Close();
                return result;
            }
        }

        /// <summary>
        ///     创建 SqlCommand 对象实例(用来返回一个整数值)
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>SqlCommand 对象实例</returns>
        private static SqlCommand BuildIntCommand(SqlConnection connection, string storedProcName,
                                                  IDataParameter[] parameters)
        {
            SqlCommand command = BuildQueryCommand(connection, storedProcName, parameters);
            command.Parameters.Add(new SqlParameter("ReturnValue",
                                                    SqlDbType.Int, 4, ParameterDirection.ReturnValue,
                                                    false, 0, 0, string.Empty, DataRowVersion.Default, null));
            return command;
        }

        #endregion

        #region SqlBulkCopy批量插入数据
        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="dt"></param>
        public static void SqlBulkCopyByDatatable(string tableName, DataTable dt)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(ConnectionString, SqlBulkCopyOptions.UseInternalTransaction))
                {
                    try
                    {
                        sqlBulkCopy.DestinationTableName = tableName;
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            sqlBulkCopy.ColumnMappings.Add(dt.Columns[i].ColumnName, dt.Columns[i].ColumnName);
                        }
                        sqlBulkCopy.WriteToServer(dt);
                    }
                    catch (System.Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }
        #endregion
    }
    public enum EffentNextType
    {
        /// <summary>
        ///     对其他语句无任何影响
        /// </summary>
        None,

        /// <summary>
        ///     当前语句必须为"select count(1) from .."格式，如果存在则继续执行，不存在回滚事务
        /// </summary>
        WhenHaveContine,

        /// <summary>
        ///     当前语句必须为"select count(1) from .."格式，如果不存在则继续执行，存在回滚事务
        /// </summary>
        WhenNoHaveContine,

        /// <summary>
        ///     当前语句影响到的行数必须大于0，否则回滚事务
        /// </summary>
        ExcuteEffectRows,

        /// <summary>
        ///     引发事件-当前语句必须为"select count(1) from .."格式，如果不存在则继续执行，存在回滚事务
        /// </summary>
        SolicitationEvent
    }

    public class CommandInfo
    {
        public object ShareObject = null;
        public object OriginalData = null;
        private event EventHandler _solicitationEvent;

        public event EventHandler SolicitationEvent
        {
            add { _solicitationEvent += value; }
            remove { _solicitationEvent -= value; }
        }

        public void OnSolicitationEvent()
        {
            if (_solicitationEvent != null)
            {
                _solicitationEvent(this, new EventArgs());
            }
        }

        public string CommandText;
        public DbParameter[] Parameters;
        public EffentNextType EffentNextType = EffentNextType.None;

        public CommandInfo()
        {
        }

        public CommandInfo(string sqlText, SqlParameter[] para)
        {
            CommandText = sqlText;
            Parameters = para;
        }

        public CommandInfo(string sqlText, SqlParameter[] para, EffentNextType type)
        {
            CommandText = sqlText;
            Parameters = para;
            EffentNextType = type;
        }
    }

    public class TransactionSwitch
    {
        private SqlConnection mSqlConnect;
        private SqlTransaction mSqlTransaction;
        public string mSQLConnectStr;

        public TransactionSwitch(string connectDbStr)
        {
            mSQLConnectStr = connectDbStr;
        }

        #region Transaction

        /// <summary>
        ///     StartTransaction
        /// </summary>
        /// <param name="aTransactionName"></param>
        public void StartTransaction(string aTransactionName)
        {
            try
            {
                if (mSqlConnect == null)
                    ConnectDB();
                mSqlTransaction = mSqlConnect.BeginTransaction();
            }
            catch (Exception e)
            {
                throw new Exception("StartTransaction failed!\n" + e.Message);
            }
        }

        /// <summary>
        ///     RollBackTransaction
        /// </summary>
        /// <param name="aTransactionName"></param>
        public void RollBackTransaction(string aTransactionName)
        {
            try
            {
                if (mSqlTransaction == null)
                {
                    string errorMsg = "Transaction Error;Transaction is not started:RollBackTransaction";
                   
                    throw new Exception(errorMsg);
                }
                mSqlTransaction.Rollback();
                mSqlTransaction.Dispose();
                mSqlTransaction = null;
                DisConnectDB();
            }
            catch (Exception e)
            {
                throw new Exception("RollBackTransaction failed!\n" + e.Message);
            }
        }

        /// <summary>
        ///     CommitTransaction
        /// </summary>
        /// <param name="aTransactionName"></param>
        public void CommitTransaction(string aTransactionName)
        {
            try
            {
                if (mSqlTransaction == null)
                {
                    string errorMsg = "Transaction Error;Transaction is not started:CommitTransaction";
                  
                    throw new Exception(errorMsg);
                }
                mSqlTransaction.Commit();
                mSqlTransaction.Dispose();
                mSqlTransaction = null;
                DisConnectDB();
            }
            catch (Exception e)
            {
                throw new Exception("CommitTransaction failed!\n" + e.Message);
            }
        }

        /// <summary>
        ///     Execute sql command
        /// </summary>
        /// <param name="aSql"></param>
        public void ExecuteNoneQueryCommand(string aSql)
        {
            if (!HasTransaction())
                if (!IsConnected())
                    ConnectDB();
            try
            {
                try
                {
                    if (!HasTransaction())
                    {
                        var sqlCommand = new SqlCommand(aSql, mSqlConnect);
                        sqlCommand.ExecuteNonQuery();
                    }
                    else
                    {
                        var sqlCommand = new SqlCommand(aSql, mSqlConnect, mSqlTransaction);
                        sqlCommand.ExecuteNonQuery();
                    }
                }
                catch (Exception e)
                {
                    string errorMsg = "ExecuteQueryCommand failed;\n" + e.Message + ";\n" +
                                      "\n SQL:" + aSql;

                  
                    throw new Exception(errorMsg + "\n");
                }
            }
            finally
            {
                if (!HasTransaction())
                    DisConnectDB();
            }
        }

        #region ConnectDB

        /// <summary>
        ///     Create DB Connection
        /// </summary>
        public void ConnectDB()
        {
            if (IsConnected())
                return;
            if (mSqlConnect == null)
                mSqlConnect = new SqlConnection(mSQLConnectStr);

            try
            {
                mSqlConnect.Open();
            }
            catch (Exception e)
            {
                mSqlConnect = null;
                string errorMsg = "ConnectDB failed;\n" + e.Message + "\n;";
              
                throw new Exception(errorMsg + "\n");
            }
        }

        #endregion

        /// <summary>
        ///     Execute sql command
        /// </summary>
        /// <param name="aSql"></param>
        /// <param name="cmdParms"></param>
        public void ExecuteNoneQueryCommand(string aSql, params SqlParameter[] cmdParms)
        {
            if (!HasTransaction())
                if (!IsConnected())
                    ConnectDB();
            try
            {
                var sqlCommand = new SqlCommand(aSql, mSqlConnect, mSqlTransaction);
                PrepareCommand(sqlCommand, null, aSql, cmdParms);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception e)
            {
                string errorMsg = "ExecuteQueryCommand failed;\n" + e.Message + ";\n" +
                                  "\n SQL:" + aSql;

              
                throw new Exception(errorMsg + "\n");
            }
            finally
            {
                if (!HasTransaction())
                    DisConnectDB();
            }
        }

        private static void PrepareCommand(SqlCommand cmd, SqlTransaction trans, string cmdText, SqlParameter[] cmdParms)
        {
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text; //cmdType;
            if (cmdParms != null)
            {
                foreach (SqlParameter parameter in cmdParms)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput ||
                         parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(parameter);
                }
            }
        }

        #region IsConnected

        /// <summary>
        ///     Check if DB is connected
        /// </summary>
        /// <returns>false:never connnect ; true:connected</returns>
        public bool IsConnected()
        {
            if (mSqlConnect == null)
                return false;
            else
            {
                if (mSqlConnect.State == ConnectionState.Closed)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        #endregion

        #region HasTransaction

        /// <summary>
        ///     Check if utilize DB Transaction
        /// </summary>
        /// <returns>false:never use;true:used</returns>
        public bool HasTransaction()
        {
            if (mSqlTransaction == null)
                return false;
            else
                return true;
        }

        #endregion

        #region DisConnectDB

        /// <summary>
        ///     Disconnect DB
        /// </summary>
        public void DisConnectDB()
        {
            if (mSqlConnect == null)
                return;
            mSqlConnect.Close();
            mSqlConnect = null;
        }

        #endregion

        #endregion
      

    }
}
