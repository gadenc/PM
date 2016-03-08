using RM.Common.DotNetCode;
using System;
using System.Data;
using System.Data.SqlClient;

namespace RM.DataBase.DataBase.Common
{
    public class IDbHelperExpand
    {
        protected LogHelper Logger = new LogHelper("IDbHelperExpand");

        public bool MsSqlBulkCopyData(DataTable dt, string connectionString)
        {
            bool result;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlTransaction trans = conn.BeginTransaction();
                    SqlBulkCopy sqlbulkCopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, trans);
                    sqlbulkCopy.DestinationTableName = dt.TableName;
                    sqlbulkCopy.BulkCopyTimeout = 1000;
                    foreach (DataColumn dtColumn in dt.Columns)
                    {
                        sqlbulkCopy.ColumnMappings.Add(dtColumn.ColumnName, dtColumn.ColumnName);
                    }
                    try
                    {
                        sqlbulkCopy.WriteToServer(dt);
                        trans.Commit();
                        result = true;
                    }
                    catch
                    {
                        trans.Rollback();
                        sqlbulkCopy.Close();
                        result = false;
                    }
                    finally
                    {
                        sqlbulkCopy.Close();
                        conn.Close();
                    }
                }
            }
            catch (Exception e)
            {
                this.Logger.WriteLog("-----------利用Net SqlBulkCopyData 批量导入数据库,速度超快-----------\r\n" + e.Message + "\r\n");
                result = false;
            }
            return result;
        }
    }
}