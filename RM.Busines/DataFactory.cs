using PDA_Service.DataBase.DataBase.SqlServer;
using RM.Common.DotNetConfig;
using RM.DataBase;

namespace RM.Busines
{
    public class DataFactory
    {
        public static IDbHelper SqlDataBase()
        {
            return new SqlServerHelper(ConfigHelper.GetAppSettings("SqlServer_RM_DB"));
        }
    }
}