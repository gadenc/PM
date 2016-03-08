using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using RM.Common.DotNetCode;
using RM.Common.DotNetConfig;
using RM.Common.DotNetData;
using RM.Common.DotNetEncrypt;
using RM.DataBase;
using RM.DataBase.DataBase.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace PDA_Service.DataBase.DataBase.SqlServer
{
    public class SqlServerHelper : IDbHelper, IDisposable
    {
        protected LogHelper Logger = new LogHelper("SQLServerLog");
        private DbCommand dbCommand = null;
        protected string connectionString = "";
        private static object locker = new object();
        private SqlDatabase db = null;

        public DbCommand DbCommand
        {
            get
            {
                return this.dbCommand;
            }
            set
            {
                this.dbCommand = value;
            }
        }

        public SqlServerHelper(string connString)
        {
            this.connectionString = connString;
        }

        public SqlDatabase GetDatabase()
        {
            SqlDatabase result;
            if (this.db == null)
            {
                if (ConfigHelper.GetAppSettings("ConStringEncrypt") == "true")
                {
                    this.db = new SqlDatabase(DESEncrypt.Decrypt(this.connectionString));
                }
                else
                {
                    this.db = new SqlDatabase(this.connectionString);
                }
                result = this.db;
            }
            else
            {
                lock (SqlServerHelper.locker)
                {
                    result = this.db;
                }
            }
            return result;
        }

        protected void AddInParameter(DbCommand cmd, SqlParam[] _params)
        {
            if (_params != null)
            {
                for (int i = 0; i < _params.Length; i++)
                {
                    SqlParam _param = _params[i];
                    DbType type = DbType.AnsiString;
                    if (_param.FiledValue is DateTime)
                    {
                        type = DbType.DateTime;
                    }
                    this.GetDatabase().AddInParameter(cmd, _param.FieldName.Replace(":", "@"), type, _param.FiledValue);
                }
            }
        }

        protected void AddInParameter(DbCommand cmd, Hashtable ht)
        {
            if (ht != null)
            {
                foreach (string key in ht.Keys)
                {
                    if (key == "Msg")
                    {
                        this.GetDatabase().AddOutParameter(cmd, "@" + key, DbType.AnsiString, 1000);
                    }
                    else
                    {
                        this.GetDatabase().AddInParameter(cmd, "@" + key, DbType.AnsiString, ht[key]);
                    }
                }
            }
        }

        protected void AddMoreParameter(DbCommand cmd, Hashtable ht)
        {
            if (ht != null)
            {
                foreach (string key in ht.Keys)
                {
                    if (key.StartsWith("OUT_"))
                    {
                        string tmp = key.Remove(0, 4);
                        this.GetDatabase().AddOutParameter(cmd, "@" + tmp, DbType.AnsiString, 1000);
                    }
                    else
                    {
                        this.GetDatabase().AddInParameter(cmd, "@" + key, DbType.AnsiString, ht[key]);
                    }
                }
            }
        }

        public SqlParam[] GetParameter(Hashtable ht)
        {
            SqlParam[] _params = new SqlParam[ht.Count];
            int i = 0;
            foreach (string key in ht.Keys)
            {
                _params[i] = new SqlParam("@" + key, ht[key]);
                i++;
            }
            return _params;
        }

        public object GetObjectValue(StringBuilder sql)
        {
            return this.GetObjectValue(sql, null);
        }

        public object GetObjectValue(StringBuilder sql, SqlParam[] param)
        {
            object result;
            try
            {
                this.dbCommand = this.GetDatabase().GetSqlStringCommand(sql.ToString());
                this.AddInParameter(this.dbCommand, param);
                result = this.db.ExecuteScalar(this.dbCommand);
            }
            catch (Exception e)
            {
                this.Logger.WriteLog(string.Concat(new string[]
				{
					"-----------根据SQL返回影响行数-----------\r\n",
					sql.ToString(),
					"\r\n",
					e.Message,
					"\r\n"
				}));
                result = null;
            }
            return result;
        }

        public int ExecuteBySql(StringBuilder sql)
        {
            return this.ExecuteBySql(sql, null);
        }

        public int ExecuteBySql(StringBuilder sql, SqlParam[] param)
        {
            int num = 0;
            try
            {
                this.dbCommand = this.GetDatabase().GetSqlStringCommand(sql.ToString());
                this.AddInParameter(this.dbCommand, param);
                using (DbConnection conn = this.db.CreateConnection())
                {
                    conn.Open();
                    DbTransaction trans = conn.BeginTransaction();
                    try
                    {
                        num = this.db.ExecuteNonQuery(this.dbCommand, trans);
                        trans.Commit();
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        num = -1;
                        this.Logger.WriteLog(string.Concat(new object[]
						{
							"-----------根据SQL执行,回滚事物-----------\r\n",
							sql.ToString(),
							"\r\n",
							e.Message,
							"\r\n返回值",
							num,
							"\r\n"
						}));
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
            catch (Exception e)
            {
                this.Logger.WriteLog(string.Concat(new object[]
				{
					"-----------执行sql语句服务器连接失败-----------\r\n",
					e.Message,
					"\r\n返回值",
					num,
					"\r\n"
				}));
            }
            return num;
        }

        public int BatchExecuteBySql(StringBuilder[] sqls, object[] param)
        {
            int num = 0;
            StringBuilder sql_Log = new StringBuilder();
            int result;
            try
            {
                using (DbConnection connection = this.GetDatabase().CreateConnection())
                {
                    connection.Open();
                    DbTransaction transaction = connection.BeginTransaction();
                    try
                    {
                        for (int i = 0; i < sqls.Length; i++)
                        {
                            StringBuilder builder = sqls[i];
                            sql_Log.Append(builder + "\r\n");
                            if (builder != null)
                            {
                                SqlParam[] paramArray = (SqlParam[])param[i];
                                DbCommand sqlStringCommand = this.db.GetSqlStringCommand(builder.ToString());
                                this.AddInParameter(sqlStringCommand, paramArray);
                                num = this.db.ExecuteNonQuery(sqlStringCommand, transaction);
                            }
                        }
                        transaction.Commit();
                        connection.Close();
                        result = num;
                        return result;
                    }
                    catch (Exception exception)
                    {
                        num = -1;
                        transaction.Rollback();
                        this.Logger.WriteLog(string.Concat(new object[]
						{
							"-----------批量执行sql语句-----------\r\n",
							sql_Log.ToString(),
							"\r\n",
							exception.Message,
							"\r\n返回值",
							num,
							"\r\n"
						}));
                    }
                }
            }
            catch (Exception e)
            {
                this.Logger.WriteLog(string.Concat(new object[]
				{
					"-----------批量执行sql语句服务器连接失败-----------\r\n",
					e.Message,
					"\r\n返回值",
					num,
					"\r\n"
				}));
            }
            result = num;
            return result;
        }

        public DataTable GetDataTable(string TargetTable)
        {
            StringBuilder sql = new StringBuilder();
            DataTable result;
            try
            {
                sql.Append("SELECT * FROM " + TargetTable);
                this.dbCommand = this.GetDatabase().GetSqlStringCommand(sql.ToString());
                result = ReaderToIListHelper.DataTableToIDataReader(this.db.ExecuteReader(this.dbCommand));
            }
            catch (Exception e)
            {
                this.Logger.WriteLog(string.Concat(new string[]
				{
					"-----------获取数据集DataTable-----------\r\n",
					sql.ToString(),
					"\r\n",
					e.Message,
					"\r\n"
				}));
                result = null;
            }
            return result;
        }

        public DataTable GetDataTable(string TargetTable, string orderField, string orderType)
        {
            StringBuilder sql = new StringBuilder();
            DataTable result;
            try
            {
                sql.Append(string.Concat(new string[]
				{
					"SELECT * FROM ",
					TargetTable,
					" ORDER BY ",
					orderField,
					" ",
					orderType
				}));
                this.dbCommand = this.GetDatabase().GetSqlStringCommand(sql.ToString());
                result = ReaderToIListHelper.DataTableToIDataReader(this.db.ExecuteReader(this.dbCommand));
            }
            catch (Exception e)
            {
                this.Logger.WriteLog(string.Concat(new string[]
				{
					"-----------获取数据集DataTable-----------\r\n",
					sql.ToString(),
					"\r\n",
					e.Message,
					"\r\n"
				}));
                result = null;
            }
            return result;
        }

        public DataTable GetDataTableBySQL(StringBuilder sql)
        {
            return this.GetDataTableBySQL(sql, null);
        }

        public DataTable GetDataTableBySQL(StringBuilder sql, SqlParam[] param)
        {
            DataTable result;
            try
            {
                this.dbCommand = this.GetDatabase().GetSqlStringCommand(sql.ToString());
                this.AddInParameter(this.dbCommand, param);
                result = ReaderToIListHelper.DataTableToIDataReader(this.db.ExecuteReader(this.dbCommand));
            }
            catch (Exception e)
            {
                this.Logger.WriteLog(string.Concat(new string[]
				{
					"-----------获取数据集DataTable-----------\r\n",
					sql.ToString(),
					"\r\n",
					e.Message,
					"\r\n"
				}));
                result = null;
            }
            return result;
        }

        public DataTable GetDataTableProc(string procName, Hashtable ht)
        {
            DataTable result;
            try
            {
                this.dbCommand = this.GetDatabase().GetStoredProcCommand(procName);
                this.AddInParameter(this.dbCommand, ht);
                using (DbConnection conn = this.db.CreateConnection())
                {
                    conn.Open();
                    DbTransaction trans = conn.BeginTransaction();
                    result = ReaderToIListHelper.DataTableToIDataReader(this.db.ExecuteReader(this.dbCommand));
                }
            }
            catch (Exception e)
            {
                this.Logger.WriteLog(string.Concat(new string[]
				{
					"-----------执行一存储过程DataTable-----------\r\n",
					procName.ToString(),
					"\r\n",
					e.Message,
					"\r\n"
				}));
                result = null;
            }
            return result;
        }

        public DataTable GetDataTableProcReturn(string procName, Hashtable ht, ref Hashtable rs)
        {
            DataTable result;
            try
            {
                this.dbCommand = this.GetDatabase().GetStoredProcCommand(procName);
                this.AddMoreParameter(this.dbCommand, ht);
                DataSet ds = this.db.ExecuteDataSet(this.dbCommand);
                rs = new Hashtable();
                foreach (string key in ht.Keys)
                {
                    if (key.StartsWith("OUT_"))
                    {
                        string tmp = key.Remove(0, 4);
                        object val = this.db.GetParameterValue(this.dbCommand, "@" + tmp);
                        rs[key] = val;
                    }
                }
                result = ds.Tables[0];
            }
            catch (Exception e)
            {
                this.Logger.WriteLog(string.Concat(new object[]
				{
					"-----------执行一存储过程DataTable返回多个值-----------\r\n",
					procName.ToString(),
					rs,
					"\r\n",
					e.Message,
					"\r\n"
				}));
                result = null;
            }
            return result;
        }

        public DataSet GetDataSetBySQL(StringBuilder sql)
        {
            return this.GetDataSetBySQL(sql, null);
        }

        public DataSet GetDataSetBySQL(StringBuilder sql, SqlParam[] param)
        {
            DataSet result;
            try
            {
                this.dbCommand = this.GetDatabase().GetSqlStringCommand(sql.ToString());
                this.AddInParameter(this.dbCommand, param);
                result = this.db.ExecuteDataSet(this.dbCommand);
            }
            catch (Exception e)
            {
                this.Logger.WriteLog("-----------获取数据集DataSet-----------\n" + sql.ToString() + "\n" + e.Message);
                result = null;
            }
            return result;
        }

        public IList GetDataListBySQL<T>(StringBuilder sql)
        {
            return this.GetDataListBySQL<T>(sql, null);
        }

        public IList GetDataListBySQL<T>(StringBuilder sql, SqlParam[] param)
        {
            IList list = new List<T>();
            this.dbCommand = this.GetDatabase().GetSqlStringCommand(sql.ToString());
            this.AddInParameter(this.dbCommand, param);
            using (IDataReader dataReader = this.db.ExecuteReader(this.dbCommand))
            {
                list = ReaderToIListHelper.ReaderToList<T>(dataReader);
            }
            return list;
        }

        public int ExecuteByProc(string procName, Hashtable ht)
        {
            int num = 0;
            int result;
            try
            {
                DbCommand storedProcCommand = this.GetDatabase().GetStoredProcCommand(procName);
                this.AddInParameter(storedProcCommand, ht);
                using (DbConnection connection = this.db.CreateConnection())
                {
                    connection.Open();
                    DbTransaction transaction = connection.BeginTransaction();
                    try
                    {
                        num = this.db.ExecuteNonQuery(storedProcCommand, transaction);
                        transaction.Commit();
                    }
                    catch (Exception exception)
                    {
                        transaction.Rollback();
                        this.Logger.WriteLog(string.Concat(new string[]
						{
							"-----------执行存储过程-----------\r\n",
							procName,
							"\r\n",
							exception.Message,
							"\r\n"
						}));
                    }
                    connection.Close();
                    result = num;
                    return result;
                }
            }
            catch (Exception e)
            {
                this.Logger.WriteLog(string.Concat(new string[]
				{
					"-----------执行存储过程服务器连接失败-----------\r\n",
					procName,
					"\r\n",
					e.Message,
					"\r\n"
				}));
            }
            result = num;
            return result;
        }

        public int ExecuteByProcNotTran(string procName, Hashtable ht)
        {
            int num = 0;
            int result;
            try
            {
                DbCommand storedProcCommand = this.GetDatabase().GetStoredProcCommand(procName);
                this.AddInParameter(storedProcCommand, ht);
                using (DbConnection connection = this.db.CreateConnection())
                {
                    connection.Open();
                    try
                    {
                        num = this.db.ExecuteNonQuery(storedProcCommand);
                    }
                    catch (Exception exception)
                    {
                        this.Logger.WriteLog(string.Concat(new string[]
						{
							"-----------执行存储过程-----------\r\n",
							procName,
							"\r\n",
							exception.Message,
							"\r\n"
						}));
                    }
                    connection.Close();
                    result = num;
                    return result;
                }
            }
            catch (Exception e)
            {
                this.Logger.WriteLog(string.Concat(new string[]
				{
					"-----------执行存储过程服务器连接失败-----------\r\n",
					procName,
					"\r\n",
					e.Message,
					"\r\n"
				}));
            }
            result = num;
            return result;
        }

        public int ExecuteByProcReturn(string procName, Hashtable ht, ref Hashtable rs)
        {
            int num = 0;
            int result;
            try
            {
                DbCommand storedProcCommand = this.GetDatabase().GetStoredProcCommand(procName);
                this.AddMoreParameter(storedProcCommand, ht);
                using (DbConnection connection = this.db.CreateConnection())
                {
                    connection.Open();
                    DbTransaction transaction = connection.BeginTransaction();
                    try
                    {
                        num = this.db.ExecuteNonQuery(storedProcCommand, transaction);
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                    }
                    connection.Close();
                }
                rs = new Hashtable();
                foreach (string str in ht.Keys)
                {
                    if (str.StartsWith("OUT_"))
                    {
                        object parameterValue = this.db.GetParameterValue(storedProcCommand, "@" + str.Remove(0, 4));
                        rs[str] = parameterValue;
                    }
                }
                result = num;
                return result;
            }
            catch (Exception exception)
            {
                this.Logger.WriteLog("-----------执行存储过程返回指定消息-----------\n" + procName + "\n" + exception.Message);
            }
            result = num;
            return result;
        }

        public int ExecuteByProcReturnMsg(string procName, Hashtable ht, ref object msg)
        {
            int num = 0;
            try
            {
                DbCommand storedProcCommand = this.GetDatabase().GetStoredProcCommand(procName);
                this.AddInParameter(storedProcCommand, ht);
                using (DbConnection connection = this.db.CreateConnection())
                {
                    connection.Open();
                    DbTransaction transaction = connection.BeginTransaction();
                    try
                    {
                        num = this.db.ExecuteNonQuery(storedProcCommand, transaction);
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                    }
                    connection.Close();
                }
                msg = this.db.GetParameterValue(storedProcCommand, "@Msg");
            }
            catch (Exception exception)
            {
                this.Logger.WriteLog("-----------执行存储过程返回指定消息-----------\n" + procName + "\n" + exception.Message);
            }
            return num;
        }

        public bool Submit_AddOrEdit(string tableName, string pkName, string pkVal, Hashtable ht)
        {
            bool result;
            if (string.IsNullOrEmpty(pkVal))
            {
                result = (this.InsertByHashtable(tableName, ht) > 0);
            }
            else
            {
                result = (this.UpdateByHashtable(tableName, pkName, pkVal, ht) > 0);
            }
            return result;
        }

        public Hashtable GetHashtableById(string tableName, string pkName, string pkVal)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Select * From ").Append(tableName).Append(" Where ").Append(pkName).Append("=@ID");
            DataTable dt = this.GetDataTableBySQL(sb, new SqlParam[]
			{
				new SqlParam("@ID", pkVal)
			});
            return DataTableHelper.DataTableToHashtable(dt);
        }

        public int IsExist(string tableName, string pkName, string pkVal)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("Select Count(1) from " + tableName);
            strSql.Append(" where " + pkName + " = @" + pkName);
            SqlParam[] param = new SqlParam[]
			{
				new SqlParam("@" + pkName, pkVal)
			};
            return CommonHelper.GetInt(this.GetObjectValue(strSql, param));
        }

        public virtual int InsertByHashtable(string tableName, Hashtable ht)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" Insert Into ");
            sb.Append(tableName);
            sb.Append("(");
            StringBuilder sp = new StringBuilder();
            StringBuilder sb_prame = new StringBuilder();
            foreach (string key in ht.Keys)
            {
                sb_prame.Append("," + key);
                sp.Append(",@" + key);
            }
            sb.Append(sb_prame.ToString().Substring(1, sb_prame.ToString().Length - 1) + ") Values (");
            sb.Append(sp.ToString().Substring(1, sp.ToString().Length - 1) + ")");
            return this.ExecuteBySql(sb, this.GetParameter(ht));
        }

        public int InsertByHashtableReturnPkVal(string tableName, Hashtable ht)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" Declare @ReturnValue int Insert Into ");
            sb.Append(tableName);
            sb.Append("(");
            StringBuilder sp = new StringBuilder();
            StringBuilder sb_prame = new StringBuilder();
            foreach (string key in ht.Keys)
            {
                sb_prame.Append("," + key);
                sp.Append(",@" + key);
            }
            sb.Append(sb_prame.ToString().Substring(1, sb_prame.ToString().Length - 1) + ") Values (");
            sb.Append(sp.ToString().Substring(1, sp.ToString().Length - 1) + ") Set @ReturnValue=SCOPE_IDENTITY() Select @ReturnValue");
            object _object = this.GetObjectValue(sb, this.GetParameter(ht));
            return (_object == DBNull.Value) ? 0 : Convert.ToInt32(_object);
        }

        public int UpdateByHashtable(string tableName, string pkName, string pkVal, Hashtable ht)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" Update ");
            sb.Append(tableName);
            sb.Append(" Set ");
            bool isFirstValue = true;
            foreach (string key in ht.Keys)
            {
                if (isFirstValue)
                {
                    isFirstValue = false;
                    sb.Append(key);
                    sb.Append("=");
                    sb.Append("@" + key);
                }
                else
                {
                    sb.Append("," + key);
                    sb.Append("=");
                    sb.Append("@" + key);
                }
            }
            sb.Append(" Where ").Append(pkName).Append("=").Append("@" + pkName);
            ht[pkName] = pkVal;
            SqlParam[] _params = this.GetParameter(ht);
            return this.ExecuteBySql(sb, _params);
        }

        public int DeleteData(string tableName, string pkName, string pkVal)
        {
            StringBuilder sb = new StringBuilder(string.Concat(new string[]
			{
				"Delete From ",
				tableName,
				" Where ",
				pkName,
				" = @ID"
			}));
            return this.ExecuteBySql(sb, new SqlParam[]
			{
				new SqlParam("@ID", pkVal)
			});
        }

        public int BatchDeleteData(string tableName, string pkName, object[] pkValues)
        {
            SqlParam[] param = new SqlParam[pkValues.Length];
            int index = 0;
            string str = "@ID" + index;
            StringBuilder sql = new StringBuilder(string.Concat(new string[]
			{
				"DELETE FROM ",
				tableName,
				" WHERE ",
				pkName,
				" IN ("
			}));
            for (int i = 0; i < param.Length - 1; i++)
            {
                object obj2 = pkValues[i];
                str = "@ID" + index;
                sql.Append(str).Append(",");
                param[index] = new SqlParam(str, obj2);
                index++;
            }
            str = "@ID" + index;
            sql.Append(str);
            param[index] = new SqlParam(str, pkValues[index]);
            sql.Append(")");
            return this.ExecuteBySql(sql, param);
        }

        public DataTable GetPageList(string sql, SqlParam[] param, string orderField, string orderType, int pageIndex, int pageSize, ref int count)
        {
            StringBuilder sb = new StringBuilder();
            DataTable result;
            try
            {
                int num = (pageIndex - 1) * pageSize;
                int num2 = pageIndex * pageSize;
                sb.Append("Select * From (Select ROW_NUMBER() Over (Order By " + orderField + " " + orderType);
                sb.Append(string.Concat(new object[]
				{
					") As rowNum, * From (",
					sql,
					") As T ) As N Where rowNum > ",
					num,
					" And rowNum <= ",
					num2
				}));
                count = Convert.ToInt32(this.GetObjectValue(new StringBuilder("Select Count(1) From (" + sql + ") As t"), param));
                result = this.GetDataTableBySQL(sb, param);
            }
            catch (Exception e)
            {
                this.Logger.WriteLog(string.Concat(new string[]
				{
					"-----------数据分页（Oracle）-----------\r\n",
					sb.ToString(),
					"\r\n",
					e.Message,
					"\r\n"
				}));
                result = null;
            }
            return result;
        }

        public bool SqlBulkCopyImport(DataTable dt)
        {
            IDbHelperExpand copy = new IDbHelperExpand();
            return copy.MsSqlBulkCopyData(dt, this.connectionString);
        }

        public void Dispose()
        {
            if (this.dbCommand != null)
            {
                this.dbCommand.Dispose();
            }
        }
    }
}