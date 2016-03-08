using RM.Common.DotNetCode;
using System.Collections;
using System.Data;
using System.Text;

namespace RM.DataBase
{
    public interface IDbHelper
    {
        object GetObjectValue(StringBuilder sql);

        object GetObjectValue(StringBuilder sql, SqlParam[] param);

        int ExecuteBySql(StringBuilder sql);

        int ExecuteBySql(StringBuilder sql, SqlParam[] param);

        int BatchExecuteBySql(StringBuilder[] sql, object[] param);

        DataTable GetDataTable(string TargetTable);

        DataTable GetDataTable(string TargetTable, string orderField, string orderType);

        DataTable GetDataTableBySQL(StringBuilder sql);

        DataTable GetDataTableBySQL(StringBuilder sql, SqlParam[] param);

        DataTable GetDataTableProc(string procName, Hashtable ht);

        DataTable GetDataTableProcReturn(string procName, Hashtable ht, ref Hashtable rs);

        DataSet GetDataSetBySQL(StringBuilder sql);

        DataSet GetDataSetBySQL(StringBuilder sql, SqlParam[] param);

        IList GetDataListBySQL<T>(StringBuilder sql);

        IList GetDataListBySQL<T>(StringBuilder sql, SqlParam[] param);

        int ExecuteByProc(string procName, Hashtable ht);

        int ExecuteByProcNotTran(string procName, Hashtable ht);

        int ExecuteByProcReturnMsg(string procName, Hashtable ht, ref object rs);

        int ExecuteByProcReturn(string procName, Hashtable intputHt, ref Hashtable outputHt);

        bool Submit_AddOrEdit(string tableName, string pkName, string pkVal, Hashtable ht);

        Hashtable GetHashtableById(string tableName, string pkName, string pkVal);

        int IsExist(string tableName, string pkName, string pkVal);

        int InsertByHashtable(string tableName, Hashtable ht);

        int InsertByHashtableReturnPkVal(string tableName, Hashtable ht);

        int UpdateByHashtable(string tableName, string pkName, string pkVal, Hashtable ht);

        int DeleteData(string tableName, string pkName, string pkVal);

        int BatchDeleteData(string tableName, string pkName, object[] pkValues);

        DataTable GetPageList(string sql, SqlParam[] param, string orderField, string orderType, int pageIndex, int pageSize, ref int count);

        bool SqlBulkCopyImport(DataTable dt);
    }
}