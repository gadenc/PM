using RM.Busines.IDAO;
using RM.Common.DotNetBean;
using RM.Common.DotNetCode;
using RM.Common.DotNetEncrypt;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace RM.Busines.DAL
{
    public class RM_UserInfo_Dal : RM_UserInfo_IDAO
    {
        public DataTable Load_StaffOrganizeList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Organization_ID,Organization_Name,ParentId,'0' AS isUser FROM Base_Organization UNION ALL SELECT U.User_ID AS Organization_ID ,U.User_Code+'|'+U.User_Name AS User_Name,S.Organization_ID,'1' AS isUser FROM Base_UserInfo U RIGHT JOIN Base_StaffOrganize S ON U.User_ID = S.User_ID");
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql);
        }

        public DataTable GetOrganizeList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM Base_Organization WHERE DeleteMark = 1 ORDER BY SortCode ASC");
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql);
        }

        public DataTable GetUserInfoPage(StringBuilder SqlWhere, IList<SqlParam> IList_param, int pageIndex, int pageSize, ref int count)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT U.User_ID,U.User_Code,U.User_Name,U.User_Account,U.User_Sex,U.Title,U.DeleteMark,U.User_Remark,U.CreateDate from Base_UserInfo U LEFT JOIN Base_StaffOrganize S ON U.User_ID = S.User_ID where U.DeleteMark !=0");
            strSql.Append(SqlWhere);
            strSql.Append("GROUP BY U.User_ID,U.User_Code,U.User_Name,U.User_Account,U.User_Sex,U.Title,U.DeleteMark,U.User_Remark,U.CreateDate");
            return DataFactory.SqlDataBase().GetPageList(strSql.ToString(), IList_param.ToArray<SqlParam>(), "CreateDate", "Desc", pageIndex, pageSize, ref count);
        }

        public DataTable UserLogin(string name, string pwd)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("Select User_ID,User_Account,User_Pwd,User_Name,DeleteMark from Base_UserInfo where ");
            strSql.Append("User_Account=@User_Account ");
            strSql.Append("and User_Pwd=@User_Pwd ");
            strSql.Append("and DeleteMark!=0");
            SqlParam[] para = new SqlParam[]
			{
				new SqlParam("@User_Account", name),
				new SqlParam("@User_Pwd", Md5Helper.MD5(pwd, 32))
			};
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql, para);
        }

        public DataTable GetUserInfoInfo(StringBuilder SqlWhere, IList<SqlParam> IList_param)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * from Base_UserInfo where DeleteMark !=0");
            strSql.Append(SqlWhere);
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql, IList_param.ToArray<SqlParam>());
        }

        public DataTable InitUserRight(string User_ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Menu_Id FROM Base_UserRight WHERE User_ID = @User_ID");
            SqlParam[] para = new SqlParam[]
			{
				new SqlParam("@User_ID", User_ID)
			};
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql, para);
        }

        public DataTable InitUserInfoUserGroup(string User_ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT UserGroup_ID FROM Base_UserInfoUserGroup WHERE User_ID = @User_ID");
            SqlParam[] para = new SqlParam[]
			{
				new SqlParam("@User_ID", User_ID)
			};
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql, para);
        }

        public DataTable InitUserRole(string User_ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Roles_ID FROM Base_UserRole WHERE User_ID = @User_ID");
            SqlParam[] para = new SqlParam[]
			{
				new SqlParam("@User_ID", User_ID)
			};
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql, para);
        }

        public DataTable InitStaffOrganize(string User_ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Organization_ID FROM Base_StaffOrganize WHERE User_ID = @User_ID");
            SqlParam[] para = new SqlParam[]
			{
				new SqlParam("@User_ID", User_ID)
			};
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql, para);
        }

        public void SysLoginLog(string SYS_USER_ACCOUNT, string SYS_LOGINLOG_STATUS, string OWNER_address)
        {
            Hashtable ht = new Hashtable();
            ht["SYS_LOGINLOG_ID"] = CommonHelper.GetGuid;
            ht["User_Account"] = SYS_USER_ACCOUNT;
            ht["SYS_LOGINLOG_IP"] = RequestHelper.GetIP();
            ht["OWNER_address"] = OWNER_address;
            ht["SYS_LOGINLOG_STATUS"] = SYS_LOGINLOG_STATUS;
            DataFactory.SqlDataBase().InsertByHashtable("Base_SysLoginlog", ht);
        }

        public DataTable GetSysLoginLogPage(StringBuilder SqlWhere, IList<SqlParam> IList_param, int pageIndex, int pageSize, ref int count)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * from Base_SysLoginlog where 1=1");
            strSql.Append(SqlWhere);
            return DataFactory.SqlDataBase().GetPageList(strSql.ToString(), IList_param.ToArray<SqlParam>(), "SYS_LOGINLOG_TIME", "Desc", pageIndex, pageSize, ref count);
        }

        public DataTable GetLogin_Info(ref int count)
        {
            DateTime now = DateTime.Now;
            DateTime d = new DateTime(now.Year, now.Month, 1);
            DateTime d2 = d.AddMonths(1).AddDays(-1.0);
            string UserAccount = RequestSession.GetSessionUser().UserAccount.ToString();
            StringBuilder strSql = new StringBuilder();
            StringBuilder strSqlCount = new StringBuilder();
            strSql.Append("Select top 2 SYS_LOGINLOG_IP,Sys_LoginLog_Time from Base_SysLoginlog where User_Account = @User_Account");
            strSql.Append(" and Sys_LoginLog_Time >= @BeginBuilTime");
            strSql.Append(" and Sys_LoginLog_Time <= @endBuilTime ORDER BY Sys_LoginLog_Time DESC ");
            strSqlCount.Append("Select count(1) from Base_SysLoginlog where User_Account = @User_Account");
            strSqlCount.Append(" and Sys_LoginLog_Time >= @BeginBuilTime");
            strSqlCount.Append(" and Sys_LoginLog_Time <= @endBuilTime");
            SqlParam[] para = new SqlParam[]
			{
				new SqlParam("@User_Account", UserAccount),
				new SqlParam("@BeginBuilTime", d),
				new SqlParam("@endBuilTime", d2)
			};
            count = Convert.ToInt32(DataFactory.SqlDataBase().GetObjectValue(strSqlCount, para));
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql, para);
        }

        public DataTable InitUserGroupList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * from Base_UserGroup WHERE DeleteMark = 1 ORDER BY SortCode ASC");
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql);
        }

        public DataTable InitUserGroupParentId()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT UserGroup_ID,\r\n                            UserGroup_Name+' - '+CASE ParentId WHEN '0' THEN '父节' ELSE  '子节' END AS UserGroup_Name\r\n                            FROM Base_UserGroup WHERE DeleteMark = 1 ORDER BY SortCode ASC");
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql);
        }

        public DataTable Load_UserInfoUserGroupList(string UserGroup_ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT UserInfoUserGroup_ID,U.User_Name+'|'+U.User_Code AS User_Name,U.User_Account,U.User_Sex,U.Title,U.DeleteMark,U.User_Remark\r\n                            FROM Base_UserInfo U RIGHT JOIN Base_UserInfoUserGroup G ON G.User_ID = U.User_ID \r\n                            WHERE G.UserGroup_ID = @UserGroup_ID");
            SqlParam[] para = new SqlParam[]
			{
				new SqlParam("@UserGroup_ID", UserGroup_ID)
			};
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql, para);
        }

        public DataTable InitUserGroupRight(string UserGroup_ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Menu_Id FROM Base_UserGroupRight WHERE UserGroup_ID = @UserGroup_ID");
            SqlParam[] para = new SqlParam[]
			{
				new SqlParam("@UserGroup_ID", UserGroup_ID)
			};
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql, para);
        }

        public bool AddUserGroupMenber(string[] User_ID, string UserGroup_ID)
        {
            bool result;
            try
            {
                StringBuilder[] sqls = new StringBuilder[User_ID.Length];
                object[] objs = new object[User_ID.Length];
                int index = 0;
                for (int i = 0; i < User_ID.Length; i++)
                {
                    string item = User_ID[i];
                    if (item.Length > 0)
                    {
                        StringBuilder sbadd = new StringBuilder();
                        sbadd.Append("Insert into Base_UserInfoUserGroup(");
                        sbadd.Append("UserInfoUserGroup_ID,User_ID,UserGroup_ID,CreateUserId,CreateUserName");
                        sbadd.Append(")Values(");
                        sbadd.Append("@UserInfoUserGroup_ID,@User_ID,@UserGroup_ID,@CreateUserId,@CreateUserName)");
                        SqlParam[] parmAdd = new SqlParam[]
						{
							new SqlParam("@UserInfoUserGroup_ID", CommonHelper.GetGuid),
							new SqlParam("@User_ID", item),
							new SqlParam("@UserGroup_ID", UserGroup_ID),
							new SqlParam("@CreateUserId", RequestSession.GetSessionUser().UserId),
							new SqlParam("@CreateUserName", RequestSession.GetSessionUser().UserName)
						};
                        sqls[index] = sbadd;
                        objs[index] = parmAdd;
                        index++;
                    }
                }
                result = (DataFactory.SqlDataBase().BatchExecuteBySql(sqls, objs) >= 0);
            }
            catch
            {
                result = false;
            }
            return result;
        }

        public bool Add_UserGroupAllotAuthority(string[] pkVal, string UserGroup_ID)
        {
            bool result;
            try
            {
                StringBuilder[] sqls = new StringBuilder[pkVal.Length + 1];
                object[] objs = new object[pkVal.Length + 1];
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("Delete From Base_UserGroupRight Where UserGroup_ID =@UserGroup_ID");
                SqlParam[] parm = new SqlParam[]
				{
					new SqlParam("@UserGroup_ID", UserGroup_ID)
				};
                sqls[0] = sbDelete;
                objs[0] = parm;
                int index = 1;
                for (int i = 0; i < pkVal.Length; i++)
                {
                    string item = pkVal[i];
                    if (item.Length > 0)
                    {
                        StringBuilder sbadd = new StringBuilder();
                        sbadd.Append("Insert into Base_UserGroupRight(");
                        sbadd.Append("UserGroupRight_ID,UserGroup_ID,Menu_Id,CreateUserId,CreateUserName");
                        sbadd.Append(")Values(");
                        sbadd.Append("@UserGroupRight_ID,@UserGroup_ID,@Menu_Id,@CreateUserId,@CreateUserName)");
                        SqlParam[] parmAdd = new SqlParam[]
						{
							new SqlParam("@UserGroupRight_ID", CommonHelper.GetGuid),
							new SqlParam("@UserGroup_ID", UserGroup_ID),
							new SqlParam("@Menu_Id", item),
							new SqlParam("@CreateUserId", RequestSession.GetSessionUser().UserId),
							new SqlParam("@CreateUserName", RequestSession.GetSessionUser().UserName)
						};
                        sqls[index] = sbadd;
                        objs[index] = parmAdd;
                        index++;
                    }
                }
                result = (DataFactory.SqlDataBase().BatchExecuteBySql(sqls, objs) >= 0);
            }
            catch
            {
                result = false;
            }
            return result;
        }
    }
}