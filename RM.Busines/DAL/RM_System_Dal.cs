using RM.Busines.IDAO;
using RM.Common.DotNetBean;
using RM.Common.DotNetCode;
using RM.Common.DotNetConfig;
using RM.Common.DotNetData;
using RM.Common.DotNetUI;
using RM.DataBase.DataBase.Common;
using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Text;

namespace RM.Busines.DAL
{
    public class RM_System_Dal : RM_System_IDAO
    {
        public int DeleteData_Base(string tableName, string pkName, string[] pkVal)
        {
            return DataFactory.SqlDataBase().BatchDeleteData(tableName, pkName, pkVal);
        }

        public DataTable GetMenuBind()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM Base_SysMenu WHERE DeleteMark = 1 ORDER BY SortCode ASC");
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql);
        }

        public DataTable GetMenuList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Menu_Id,Menu_Name,Menu_Img,Menu_Type,TARGET,ParentId,\r\n                            CAST(Menu_Type AS VARCHAR(10)) +'-'+CAST(SortCode AS VARCHAR(10)) AS Sort,\r\n                            NavigateUrl,CreateUserName,CreateDate,ModifyUserName,ModifyDate\r\n                            FROM Base_SysMenu WHERE DeleteMark = 1 and Menu_Type !=3 ORDER BY SortCode ASC");
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql);
        }

        public DataTable GetButtonList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM Base_Button WHERE DELETEMARK = 1 ORDER BY SORTCODE ASC");
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql);
        }

        public DataTable GetSysMenuByButton(string Menu_Id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM Base_SysMenu ");
            strSql.Append("WHERE ParentId =@Menu_Id ");
            strSql.Append("AND DELETEMARK = 1 AND Menu_Type = 3 ORDER BY SORTCODE ASC");
            SqlParam[] parm = new SqlParam[]
			{
				new SqlParam("@Menu_Id", Menu_Id)
			};
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql, parm);
        }

        public int AllotButton(string pkVal, string ParentId)
        {
            int result;
            try
            {
                DataTable dt_Button = this.GetButtonList();
                DataTable Newdt_Button = DataTableHelper.GetNewDataTable(dt_Button, "Button_ID = '" + pkVal + "'");
                string Button_Name = Newdt_Button.Rows[0]["Button_Name"].ToString();
                string Button_Title = Newdt_Button.Rows[0]["Button_Title"].ToString();
                string Button_Img = Newdt_Button.Rows[0]["Button_Img"].ToString();
                string Button_Code = Newdt_Button.Rows[0]["Button_Code"].ToString();
                Hashtable ht = new Hashtable();
                ht["Menu_Id"] = CommonHelper.GetGuid;
                ht["ParentId"] = ParentId;
                ht["Menu_Name"] = Button_Name;
                ht["Menu_Title"] = Button_Title;
                ht["Menu_Img"] = Button_Img;
                ht["Menu_Type"] = 3;
                ht["NavigateUrl"] = Button_Code;
                ht["SortCode"] = CommonHelper.GetInt(DataFactory.SqlDataBase().GetObjectValue(new StringBuilder("SELECT MAX(SortCode) FROM Base_SysMenu WHERE ParentId = '" + ParentId + "' AND DELETEMARK = 1 AND Menu_Type = 3"))) + 1;
                ht["Target"] = "Onclick";
                ht["CreateUserId"] = RequestSession.GetSessionUser().UserId;
                ht["CreateUserName"] = RequestSession.GetSessionUser().UserName;
                result = DataFactory.SqlDataBase().InsertByHashtable("Base_SysMenu", ht);
            }
            catch (Exception)
            {
                result = -1;
            }
            return result;
        }

        public DataTable GetButtonHtml(string UserId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT  M.Menu_Id,M.Menu_Name ,M.Menu_Title ,M.Menu_Img ,M.TARGET ,M.ParentId ,M.NavigateUrl ,M.SortCode,M.Menu_Type\r\n                            FROM    ( SELECT    M.Menu_Name ,M.Menu_Title ,M.Menu_Img ,M.TARGET ,M.ParentId ,M.Menu_Id ,M.NavigateUrl ,M.SortCode,M.Menu_Type ,'角色权限' AS TheirTYPE\r\n                                      FROM      Base_SysMenu M\r\n                                                LEFT JOIN Base_RoleRight R_R ON R_R.Menu_Id = M.Menu_Id\r\n                                                LEFT JOIN Base_UserRole U_R ON U_R.Roles_ID = R_R.Roles_ID\r\n                                      WHERE     U_R.User_ID = @User_ID\r\n                                                AND M.DeleteMark = 1 \r\n                                      UNION ALL\r\n                                      SELECT    M.Menu_Name ,M.Menu_Title ,M.Menu_Img ,M.TARGET ,M.ParentId ,M.Menu_Id ,M.NavigateUrl ,M.SortCode,M.Menu_Type ,'用户组权限' AS TheirTYPE\r\n                                      FROM      Base_SysMenu M\r\n                                                LEFT JOIN Base_UserGroupRight U_R ON U_R.Menu_Id = M.Menu_Id\r\n                                                LEFT JOIN Base_UserInfoUserGroup U_G ON U_G.UserGroup_ID = U_R.UserGroup_ID\r\n                                      WHERE     U_G.User_ID = @User_ID\r\n                                                AND M.DeleteMark = 1 \r\n                                      UNION ALL\r\n                                      SELECT    M.Menu_Name ,M.Menu_Title ,M.Menu_Img ,M.TARGET ,M.ParentId ,M.Menu_Id ,M.NavigateUrl ,M.SortCode,M.Menu_Type,'用户权限' AS TheirTYPE\r\n                                      FROM      Base_SysMenu M\r\n                                                LEFT JOIN Base_UserRight U_R ON U_R.Menu_Id = M.Menu_Id\r\n                                      WHERE     U_R.User_ID = @User_ID\r\n                                                AND M.DeleteMark = 1\r\n                                    ) M\r\n                            GROUP BY M.Menu_Id ,M.Menu_Name ,M.Menu_Title ,M.Menu_Img,M.TARGET ,M.ParentId ,M.NavigateUrl ,M.SortCode,M.Menu_Type ORDER BY M.SortCode");
            SqlParam[] para = new SqlParam[]
			{
				new SqlParam("@User_ID", UserId)
			};
            StringBuilder sb_html = new StringBuilder();
            string URL = RequestHelper.GetScriptName;
            DataTable dt_Menu = DataFactory.SqlDataBase().GetDataTableBySQL(strSql, para);
            return DataTableHelper.GetNewDataTable(dt_Menu, "ParentId='" + this.GetMenuByNavigateUrl(URL, dt_Menu) + "' AND Menu_Type = 3");
        }

        public string GetMenuByNavigateUrl(string NavigateUrl, DataTable dt_Menu)
        {
            string result;
            try
            {
                DataTable dt = DataTableHelper.GetNewDataTable(dt_Menu, "NavigateUrl='" + NavigateUrl + "'");
                result = dt.Rows[0]["Menu_Id"].ToString();
            }
            catch
            {
                result = "";
            }
            return result;
        }

        public DataTable GetHaveRightUserInfo(string User_ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT DISTINCT(M.Menu_Id),M.TheirTYPE FROM(\r\n                            SELECT M.Menu_Id,'角色权限' AS TheirTYPE FROM Base_SysMenu M\r\n                            LEFT JOIN  Base_RoleRight R_R ON R_R.Menu_Id = M.Menu_Id\r\n                            LEFT JOIN Base_UserRole U_R ON U_R.Roles_ID = R_R.Roles_ID\r\n                            WHERE U_R.User_ID = @User_ID UNION ALL\r\n                            SELECT M.Menu_Id,'用户组权限' AS TheirTYPE FROM Base_SysMenu M\r\n                            LEFT JOIN  Base_UserGroupRight U_R ON U_R.Menu_Id = M.Menu_Id\r\n                            LEFT JOIN Base_UserInfoUserGroup U_G ON U_G.UserGroup_ID = U_R.UserGroup_ID\r\n                            WHERE U_G.User_ID = @User_ID UNION ALL\r\n                            SELECT M.Menu_Id,'用户权限' AS TheirTYPE FROM Base_SysMenu M\r\n                            LEFT JOIN  Base_UserRight U_R ON U_R.Menu_Id = M.Menu_Id\r\n                            WHERE U_R.User_ID = @User_ID\r\n                            ) M");
            SqlParam[] para = new SqlParam[]
			{
				new SqlParam("@User_ID", User_ID)
			};
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql, para);
        }

        public DataTable GetMenuHtml(string UserId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT  M.Menu_Id,M.Menu_Name ,M.Menu_Title ,M.Menu_Img ,M.TARGET ,M.ParentId ,M.NavigateUrl ,M.SortCode\r\n                            FROM    ( SELECT    M.Menu_Name ,M.Menu_Title ,M.Menu_Img ,M.TARGET ,M.ParentId ,M.Menu_Id ,M.NavigateUrl ,M.SortCode ,'角色权限' AS TheirTYPE\r\n                                      FROM      Base_SysMenu M\r\n                                                LEFT JOIN Base_RoleRight R_R ON R_R.Menu_Id = M.Menu_Id\r\n                                                LEFT JOIN Base_UserRole U_R ON U_R.Roles_ID = R_R.Roles_ID\r\n                                      WHERE     M.TARGET = 'Iframe' AND U_R.User_ID = @User_ID\r\n                                                AND M.DeleteMark = 1 \r\n                                      UNION ALL\r\n                                      SELECT    M.Menu_Name ,M.Menu_Title ,M.Menu_Img ,M.TARGET ,M.ParentId ,M.Menu_Id ,M.NavigateUrl ,M.SortCode ,'用户组权限' AS TheirTYPE\r\n                                      FROM      Base_SysMenu M\r\n                                                LEFT JOIN Base_UserGroupRight U_R ON U_R.Menu_Id = M.Menu_Id\r\n                                                LEFT JOIN Base_UserInfoUserGroup U_G ON U_G.UserGroup_ID = U_R.UserGroup_ID\r\n                                      WHERE     M.TARGET = 'Iframe' AND U_G.User_ID = @User_ID\r\n                                                AND M.DeleteMark = 1 \r\n                                      UNION ALL\r\n                                      SELECT    M.Menu_Name ,M.Menu_Title ,M.Menu_Img ,M.TARGET ,M.ParentId ,M.Menu_Id ,M.NavigateUrl ,M.SortCode ,'用户权限' AS TheirTYPE\r\n                                      FROM      Base_SysMenu M\r\n                                                LEFT JOIN Base_UserRight U_R ON U_R.Menu_Id = M.Menu_Id\r\n                                      WHERE     M.TARGET = 'Iframe' AND U_R.User_ID = @User_ID\r\n                                                AND M.DeleteMark = 1\r\n                                    ) M\r\n                            GROUP BY M.Menu_Id ,M.Menu_Name ,M.Menu_Title ,M.Menu_Img ,M.TARGET ,M.ParentId ,M.NavigateUrl ,M.SortCode ORDER BY M.SortCode");
            SqlParam[] para = new SqlParam[]
			{
				new SqlParam("@User_ID", UserId)
			};
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql, para);
        }

        public DataTable GetPermission_URL(string UserId)
        {
            DataTable dt = new DataTable();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT  M.NavigateUrl ,\r\n                                    M.TheirTYPE ,\r\n                                    M.User_ID\r\n                            FROM    ( SELECT    M.NavigateUrl ,\r\n                                                '角色权限' AS TheirTYPE ,\r\n                                                U_R.User_ID AS User_ID\r\n                                      FROM      Base_SysMenu M\r\n                                                LEFT JOIN Base_RoleRight R_R ON R_R.Menu_Id = M.Menu_Id\r\n                                                LEFT JOIN Base_UserRole U_R ON U_R.Roles_ID = R_R.Roles_ID\r\n                                      WHERE     M.Menu_Type != 3\r\n                                      UNION ALL\r\n                                      SELECT    M.NavigateUrl ,\r\n                                                '用户组权限' AS TheirTYPE ,\r\n                                                U_G.User_ID AS User_ID\r\n                                      FROM      Base_SysMenu M\r\n                                                LEFT JOIN Base_UserGroupRight U_R ON U_R.Menu_Id = M.Menu_Id\r\n                                                LEFT JOIN Base_UserInfoUserGroup U_G ON U_G.UserGroup_ID = U_R.UserGroup_ID\r\n                                      WHERE     M.Menu_Type != 3\r\n                                      UNION ALL\r\n                                      SELECT    M.NavigateUrl ,\r\n                                                '用户权限' AS TheirTYPE ,\r\n                                                U_R.User_ID AS User_ID\r\n                                      FROM      Base_SysMenu M\r\n                                                LEFT JOIN Base_UserRight U_R ON U_R.Menu_Id = M.Menu_Id\r\n                                      WHERE     M.Menu_Type != 3\r\n                                    ) M\r\n                            WHERE   M.NavigateUrl != ''\r\n                            AND M.User_ID IS NOT NULL");
            if (CacheHelper.GetCache("KeyPermission_URL") == null)
            {
                dt = DataFactory.SqlDataBase().GetDataTableBySQL(strSql);
                CacheHelper.Insert("KeyPermission_URL", dt);
            }
            else
            {
                dt = (DataTable)CacheHelper.GetCache("KeyPermission_URL");
            }
            return DataTableHelper.GetNewDataTable(dt, "User_ID= '" + UserId + "'");
        }

        public DataTable InitRoleList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Roles_ID, ParentId, Roles_Name, Roles_Remark, SortCode, DeleteMark, CreateDate,CreateUserName, ModifyDate, ModifyUserName\r\n                            FROM Base_Roles WHERE DeleteMark != 0 ORDER BY SortCode ASC");
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql);
        }

        public DataTable InitRoleParentId()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Roles_ID,\r\n                            Roles_Name+' - '+CASE ParentId WHEN '0' THEN '父节' ELSE  '子节' END AS Roles_Name\r\n                            FROM Base_Roles WHERE DeleteMark = 1 ORDER BY SortCode ASC");
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql);
        }

        public DataTable InitUserRole(string Roles_ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT R.User_ID,U.User_Code+'|'+U.User_Name AS User_Name,O.Organization_Name FROM Base_UserRole R\r\n                            LEFT JOIN Base_UserInfo U ON U.User_ID=R.User_ID\r\n                            LEFT JOIN Base_StaffOrganize S ON S.User_ID = U.User_ID\r\n                            LEFT JOIN Base_Organization O ON O.Organization_ID = S.Organization_ID WHERE Roles_ID = @Roles_ID");
            SqlParam[] para = new SqlParam[]
			{
				new SqlParam("@Roles_ID", Roles_ID)
			};
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql, para);
        }

        public DataTable InitRoleRight(string Roles_ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Menu_Id FROM Base_RoleRight WHERE Roles_ID = @Roles_ID");
            SqlParam[] para = new SqlParam[]
			{
				new SqlParam("@Roles_ID", Roles_ID)
			};
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql, para);
        }

        public bool Add_RoleAllotMember(string[] pkVal, string Roles_ID)
        {
            bool result;
            try
            {
                StringBuilder[] sqls = new StringBuilder[pkVal.Length + 1];
                object[] objs = new object[pkVal.Length + 1];
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("Delete From Base_UserRole Where Roles_ID =@Roles_ID");
                SqlParam[] parm = new SqlParam[]
				{
					new SqlParam("@Roles_ID", Roles_ID)
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
                        sbadd.Append("Insert into Base_UserRole(");
                        sbadd.Append("UserRole_ID,User_ID,Roles_ID,CreateUserId,CreateUserName");
                        sbadd.Append(")Values(");
                        sbadd.Append("@UserRole_ID,@User_ID,@Roles_ID,@CreateUserId,@CreateUserName)");
                        SqlParam[] parmAdd = new SqlParam[]
						{
							new SqlParam("@UserRole_ID", CommonHelper.GetGuid),
							new SqlParam("@User_ID", item),
							new SqlParam("@Roles_ID", Roles_ID),
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

        public bool Add_RoleAllotAuthority(string[] pkVal, string Roles_ID)
        {
            bool result;
            try
            {
                StringBuilder[] sqls = new StringBuilder[pkVal.Length + 1];
                object[] objs = new object[pkVal.Length + 1];
                StringBuilder sbDelete = new StringBuilder();
                sbDelete.Append("Delete From Base_RoleRight Where Roles_ID =@Roles_ID");
                SqlParam[] parm = new SqlParam[]
				{
					new SqlParam("@Roles_ID", Roles_ID)
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
                        sbadd.Append("Insert into Base_RoleRight(");
                        sbadd.Append("RoleRight_ID,Roles_ID,Menu_Id,CreateUserId,CreateUserName");
                        sbadd.Append(")Values(");
                        sbadd.Append("@RoleRight_ID,@Roles_ID,@Menu_Id,@CreateUserId,@CreateUserName)");
                        SqlParam[] parmAdd = new SqlParam[]
						{
							new SqlParam("@RoleRight_ID", CommonHelper.GetGuid),
							new SqlParam("@Roles_ID", Roles_ID),
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

        public DataTable GetRoleByMember(string user_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Roles_Name,Roles_Remark FROM Base_Roles\r\n                            WHERE Roles_ID IN(SELECT Roles_ID FROM Base_UserRole WHERE User_ID = @User_ID)");
            SqlParam[] para = new SqlParam[]
			{
				new SqlParam("@User_ID", user_id)
			};
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql, para);
        }

        public DataTable AppendProperty_List(string Function)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM Base_AppendProperty WHERE DeleteMark = 1 AND Property_Control_ID !='0' AND Property_Function=@Property_Function ORDER BY SortCode ASC");
            SqlParam[] para = new SqlParam[]
			{
				new SqlParam("@Property_Function", Function)
			};
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql, para);
        }

        public DataTable AppendProperty_Function()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM Base_AppendProperty WHERE DeleteMark = 1 AND Property_Control_ID = '0'");
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql);
        }

        public string GetPropertyInstancepk(string Property_Function, string Obj_ID)
        {
            StringBuilder item_value = new StringBuilder();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT I.Property_Control_ID,I.PropertyInstance_Value FROM Base_AppendPropertyInstance I \r\n                            LEFT JOIN Base_AppendProperty A ON I.Property_Control_ID = A.Property_Control_ID");
            strSql.Append(" where I.PropertyInstance_Key = @PropertyInstance_Key AND A.Property_Function = @Property_Function");
            SqlParam[] param = new SqlParam[]
			{
				new SqlParam("@PropertyInstance_Key", Obj_ID),
				new SqlParam("@Property_Function", Property_Function)
			};
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(strSql, param);
            if (dt != null && dt.Rows.Count != 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    item_value.Append(dt.Rows[i]["Property_Control_ID"].ToString() + "|" + dt.Rows[i]["PropertyInstance_Value"].ToString() + ";");
                }
            }
            return item_value.ToString();
        }

        public string AppendProperty_Html(string Function)
        {
            StringBuilder str_Output = new StringBuilder();
            DataTable dt = this.AppendProperty_List(Function);
            if (DataTableHelper.IsExistRows(dt))
            {
                int i = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    string Property_Name = dr["Property_Name"].ToString();
                    string Control_ID = dr["Property_Control_ID"].ToString();
                    string Control_Type = dr["Property_Control_Type"].ToString();
                    string Control_Length = dr["Property_Control_Length"].ToString();
                    string Control_Style = dr["Property_Control_Style"].ToString();
                    string Control_Validator = dr["Property_Control_Validator"].ToString();
                    string Maxlength = dr["Property_Control_Maxlength"].ToString();
                    string Colspan = dr["Property_Colspan"].ToString();
                    string DataSource = dr["Property_Control_DataSource"].ToString();
                    string Event = dr["Property_Event"].ToString();
                    if (Control_Validator != "")
                    {
                        Control_Validator = string.Concat(new string[]
						{
							"datacol=\"yes\" err=\"",
							Property_Name,
							"\" checkexpession=\"",
							Control_Validator,
							"\""
						});
                    }
                    if (Colspan == "0")
                    {
                        Colspan = "";
                    }
                    else
                    {
                        Colspan = "colspan=" + Colspan;
                    }
                    if (Colspan == "")
                    {
                        if (i == 0)
                        {
                            str_Output.Append(ControlBindHelper.GetControlProperty(Control_Type, Property_Name, Control_ID, Control_Style, Control_Length, Control_Validator, i, Colspan, DataSource, Event, Maxlength));
                            i = 1;
                        }
                        else
                        {
                            if (i == 1)
                            {
                                str_Output.Append(ControlBindHelper.GetControlProperty(Control_Type, Property_Name, Control_ID, Control_Style, Control_Length, Control_Validator, i, Colspan, DataSource, Event, Maxlength));
                                i = 0;
                            }
                        }
                    }
                    else
                    {
                        str_Output.Append(ControlBindHelper.GetControlProperty(Control_Type, Property_Name, Control_ID, Control_Style, Control_Length, Control_Validator, i, Colspan, DataSource, Event, Maxlength));
                    }
                }
            }
            return str_Output.ToString();
        }

        public string AppendProperty_HtmlLabel(string Function)
        {
            StringBuilder str_Output = new StringBuilder();
            DataTable dt = this.AppendProperty_List(Function);
            if (DataTableHelper.IsExistRows(dt))
            {
                int i = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    string Property_Name = dr["Property_Name"].ToString();
                    string Control_ID = dr["Property_Control_ID"].ToString();
                    string Control_Type = "4";
                    string Control_Length = dr["Property_Control_Length"].ToString();
                    string Control_Style = dr["Property_Control_Style"].ToString();
                    string Control_Validator = dr["Property_Control_Validator"].ToString();
                    string Maxlength = dr["Property_Control_Maxlength"].ToString();
                    string Colspan = dr["Property_Colspan"].ToString();
                    string DataSource = dr["Property_Control_DataSource"].ToString();
                    string Event = dr["Property_Event"].ToString();
                    str_Output.Append(ControlBindHelper.GetControlProperty(Control_Type, Property_Name, Control_ID, Control_Style, Control_Length, Control_Validator, i, Colspan, DataSource, Event, Maxlength));
                }
            }
            return str_Output.ToString();
        }

        public bool Add_AppendPropertyInstance(string guid, string[] arrayitem)
        {
            StringBuilder[] sqls = new StringBuilder[arrayitem.Length + 1];
            object[] objs = new object[arrayitem.Length + 1];
            StringBuilder sbDelete = new StringBuilder();
            sbDelete.Append("Delete From Base_AppendPropertyInstance Where PropertyInstance_Key =@PropertyInstance_Key");
            SqlParam[] parm = new SqlParam[]
			{
				new SqlParam("@PropertyInstance_Key", guid)
			};
            sqls[0] = sbDelete;
            objs[0] = parm;
            int index = 1;
            for (int i = 0; i < arrayitem.Length; i++)
            {
                string item = arrayitem[i];
                if (item.Length > 0)
                {
                    string[] str_item = item.Split(new char[]
					{
						'|'
					});
                    StringBuilder sbadd = new StringBuilder();
                    sbadd.Append("Insert into Base_AppendPropertyInstance(");
                    sbadd.Append("PropertyInstance_ID,Property_Control_ID,PropertyInstance_Value,PropertyInstance_Key");
                    sbadd.Append(")Values(");
                    sbadd.Append("@PropertyInstance_ID,@Property_Control_ID,@PropertyInstance_Value,@PropertyInstance_Key)");
                    SqlParam[] parmAdd = new SqlParam[]
					{
						new SqlParam("@PropertyInstance_ID", CommonHelper.GetGuid),
						new SqlParam("@Property_Control_ID", str_item[0]),
						new SqlParam("@PropertyInstance_Value", str_item[1]),
						new SqlParam("@PropertyInstance_Key", guid)
					};
                    sqls[index] = sbadd;
                    objs[index] = parmAdd;
                    index++;
                }
            }
            return DataFactory.SqlDataBase().BatchExecuteBySql(sqls, objs) >= 0;
        }

        public DataTable GetHomeShortcut_List(string User_ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM Base_O_A_Setup WHERE User_ID = @User_ID");
            SqlParam[] para = new SqlParam[]
			{
				new SqlParam("@User_ID", User_ID)
			};
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql, para);
        }

        public DataTable GetRecyclebin_ObjField(string object_TableName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Field_Key=a.name,Field_Name=isnull(g.[value],'未填说明')\r\n                            FROM syscolumns a\r\n                            left join systypes b on a.xusertype=b.xusertype\r\n                            inner join sysobjects d on a.id=d.id  and d.xtype='U' and  d.name<>'dtproperties'\r\n                            left join syscomments e on a.cdefault=e.id\r\n                            left join sys.extended_properties g on a.id=g.major_id and a.colid=g.minor_id \r\n                            left join sys.extended_properties f on d.id=f.major_id and f.minor_id=0");
            strSql.Append("where d.name='" + object_TableName + "' order by a.id,a.colorder");
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql);
        }

        public int Virtualdelete(string module, string tableName, string pkName, string[] pkVal)
        {
            int num = 0;
            int result;
            try
            {
                StringBuilder[] sqls = new StringBuilder[pkVal.Length * 2];
                object[] objs = new object[pkVal.Length * 2];
                int index = 0;
                for (int i = 0; i < pkVal.Length; i++)
                {
                    string item = pkVal[i];
                    StringBuilder sbEdit = new StringBuilder();
                    sbEdit.Append(" Update ");
                    sbEdit.Append(tableName);
                    sbEdit.Append(" Set DeleteMark = 0");
                    sbEdit.Append(" Where ").Append(pkName).Append("=").Append("@ID");
                    SqlParam[] parmEdit = new SqlParam[]
					{
						new SqlParam("@ID", item)
					};
                    sqls[index] = sbEdit;
                    objs[index] = parmEdit;
                    index++;
                    StringBuilder sbadd = new StringBuilder();
                    sbadd.Append("Insert into Base_Recyclebin(");
                    sbadd.Append("Recyclebin_ID,");
                    sbadd.Append("Recyclebin_Name,");
                    sbadd.Append("Recyclebin_Database,");
                    sbadd.Append("Recyclebin_Table,CreateUserId,CreateUserName,");
                    sbadd.Append("Recyclebin_FieldKey,Recyclebin_EventField)Values(");
                    sbadd.Append("@Recyclebin_ID,");
                    sbadd.Append("@Recyclebin_Name,");
                    sbadd.Append("@Recyclebin_Database,");
                    sbadd.Append("@Recyclebin_Table,@CreateUserId,@CreateUserName,");
                    sbadd.Append("@Recyclebin_FieldKey,@Recyclebin_EventField)");
                    SqlParam[] parmAdd = new SqlParam[]
					{
						new SqlParam("@Recyclebin_ID", CommonHelper.GetGuid),
						new SqlParam("@Recyclebin_Name", module),
						new SqlParam("@Recyclebin_Database", "RM_DB"),
						new SqlParam("@Recyclebin_Table", tableName),
						new SqlParam("@Recyclebin_FieldKey", pkName),
						new SqlParam("@CreateUserId", RequestSession.GetSessionUser().UserId),
						new SqlParam("@CreateUserName", RequestSession.GetSessionUser().UserName),
						new SqlParam("@Recyclebin_EventField", item)
					};
                    sqls[index] = sbadd;
                    objs[index] = parmAdd;
                    index++;
                }
                num = DataFactory.SqlDataBase().BatchExecuteBySql(sqls, objs);
            }
            catch
            {
                result = num;
                return result;
            }
            result = num;
            return result;
        }

        public int Recyclebin_Restore(string[] pkVal)
        {
            int num = 0;
            int result;
            try
            {
                StringBuilder[] sqls = new StringBuilder[pkVal.Length * 2];
                object[] objs = new object[pkVal.Length * 2];
                int index = 0;
                for (int i = 0; i < pkVal.Length; i++)
                {
                    string item = pkVal[i];
                    Hashtable ht = DataFactory.SqlDataBase().GetHashtableById("Base_Recyclebin", "Recyclebin_ID", item);
                    if (ht.Count > 0 && ht != null)
                    {
                        string tableName = ht["RECYCLEBIN_TABLE"].ToString();
                        string pkName = ht["RECYCLEBIN_FIELDKEY"].ToString();
                        StringBuilder sbEdit = new StringBuilder();
                        sbEdit.Append(" Update ");
                        sbEdit.Append(tableName);
                        sbEdit.Append(" Set DeleteMark = 1");
                        sbEdit.Append(" Where ").Append(pkName).Append("=").Append("@ID");
                        SqlParam[] parmEdit = new SqlParam[]
						{
							new SqlParam("@ID", ht["RECYCLEBIN_EVENTFIELD"].ToString())
						};
                        sqls[index] = sbEdit;
                        objs[index] = parmEdit;
                        index++;
                        StringBuilder sbDelete = new StringBuilder();
                        sbDelete.Append("Delete From Base_Recyclebin Where Recyclebin_ID =@Recyclebin_ID");
                        SqlParam[] parmDelete = new SqlParam[]
						{
							new SqlParam("@Recyclebin_ID", item)
						};
                        sqls[index] = sbDelete;
                        objs[index] = parmDelete;
                    }
                    index++;
                }
                num = DataFactory.SqlDataBase().BatchExecuteBySql(sqls, objs);
            }
            catch
            {
                result = num;
                return result;
            }
            result = num;
            return result;
        }

        public int Recyclebin_Empty(string[] pkVal)
        {
            int num = 0;
            int result;
            try
            {
                StringBuilder[] sqls = new StringBuilder[pkVal.Length * 2];
                object[] objs = new object[pkVal.Length * 2];
                int index = 0;
                for (int i = 0; i < pkVal.Length; i++)
                {
                    string item = pkVal[i];
                    Hashtable ht = DataFactory.SqlDataBase().GetHashtableById("Base_Recyclebin", "Recyclebin_ID", item);
                    if (ht.Count > 0 && ht != null)
                    {
                        string tableName = ht["RECYCLEBIN_TABLE"].ToString();
                        string pkName = ht["RECYCLEBIN_FIELDKEY"].ToString();
                        StringBuilder sb = new StringBuilder();
                        sb.Append(" Delete From ");
                        sb.Append(tableName);
                        sb.Append(" Where ").Append(pkName).Append("=").Append("@ID");
                        SqlParam[] parm = new SqlParam[]
						{
							new SqlParam("@ID", ht["RECYCLEBIN_EVENTFIELD"].ToString())
						};
                        sqls[index] = sb;
                        objs[index] = parm;
                        index++;
                        StringBuilder sbDelete = new StringBuilder();
                        sbDelete.Append("Delete From Base_Recyclebin Where Recyclebin_ID =@Recyclebin_ID");
                        SqlParam[] parmDelete = new SqlParam[]
						{
							new SqlParam("@Recyclebin_ID", item)
						};
                        sqls[index] = sbDelete;
                        objs[index] = parmDelete;
                    }
                    index++;
                }
                num = DataFactory.SqlDataBase().BatchExecuteBySql(sqls, objs);
            }
            catch
            {
                result = num;
                return result;
            }
            result = num;
            return result;
        }

        public bool DataRestore(string FilePath)
        {
            string[] Connection = ConfigHelper.GetAppSettings("SqlServer_RM_DB").Split(new char[]
			{
				';'
			});
            return new SqlServerBackup
            {
                Server = Connection[0].Substring(7),
                Database = Connection[1].Substring(9),
                Uid = Connection[2].Substring(4),
                Pwd = Connection[3].Substring(4)
            }.DbRestore(FilePath);
        }

        public bool DataBackups(string FilePath)
        {
            bool result;
            try
            {
                string[] Connection = ConfigHelper.GetAppSettings("SqlServer_RM_DB").Split(new char[]
				{
					';'
				});
                if (new SqlServerBackup
                {
                    Server = Connection[0].Substring(7),
                    Database = Connection[1].Substring(9),
                    Uid = Connection[2].Substring(4),
                    Pwd = Connection[3].Substring(4)
                }.DbBackup(FilePath))
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

        public void Add_Backup_Restore_Log(string Type, string File, string Size, string CreateUserName, string DB, string Memo)
        {
            LogHelper Logger = new LogHelper("Backup_Restore_Log");
            Hashtable ht = new Hashtable();
            StringBuilder sb = new StringBuilder();
            sb.Append(Type + "∫");
            sb.Append(File + "∫");
            sb.Append(Size + "∫");
            sb.Append(CreateUserName + "∫");
            sb.Append(DB + "∫");
            sb.Append(Memo + "∫");
            sb.Append(DateTime.Now + "∫");
            sb.Append("∮");
            Logger.WriteLog(sb.ToString());
        }

        public DataTable GetBackup_Restore_Log_List()
        {
            LogHelper Logger = new LogHelper("Backup_Restore_Log");
            string filepath = ConfigHelper.GetAppSettings("LogFilePath") + "/Backup_Restore_Log.log";
            StreamReader sr = new StreamReader(filepath, Encoding.GetEncoding("UTF-8"));
            string[] strvalue = sr.ReadToEnd().ToString().Split(new char[]
			{
				'∮'
			});
            sr.Close();
            DataTable dt = new DataTable();
            dt.Columns.Add("Backup_Restore_Type", Type.GetType("System.String"));
            dt.Columns.Add("Backup_Restore_File", Type.GetType("System.String"));
            dt.Columns.Add("Backup_Restore_Size", Type.GetType("System.String"));
            dt.Columns.Add("CreateUserName", Type.GetType("System.String"));
            dt.Columns.Add("Backup_Restore_DB", Type.GetType("System.String"));
            dt.Columns.Add("Backup_Restore_Memo", Type.GetType("System.String"));
            dt.Columns.Add("CreateDate", Type.GetType("System.String"));
            string[] array = strvalue;
            for (int i = 0; i < array.Length; i++)
            {
                string item = array[i];
                if (item.Length > 6)
                {
                    string[] str_item = item.Split(new char[]
					{
						'∫'
					});
                    DataRow row = dt.NewRow();
                    string[] Typeitem = str_item[0].Split(new char[]
					{
						']'
					});
                    row["Backup_Restore_Type"] = Typeitem[1].Trim();
                    row["Backup_Restore_File"] = str_item[1];
                    row["Backup_Restore_Size"] = str_item[2];
                    row["CreateUserName"] = str_item[3];
                    row["Backup_Restore_DB"] = str_item[4];
                    row["Backup_Restore_Memo"] = str_item[5];
                    row["CreateDate"] = str_item[6];
                    dt.Rows.Add(row);
                }
            }
            dt.DefaultView.Sort = "CreateDate DESC";
            return dt.DefaultView.ToTable();
        }

        public DataTable GetSysobjects()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Name as TABLE_NAME from sysobjects where xtype='u' and status >=0 and Name !='sysdiagrams' ");
            return DataFactory.SqlDataBase().GetDataTableBySQL(strSql);
        }

        public DataTable GetSyscolumns(string object_id)
        {
            DataTable dt = new DataTable();
            StringBuilder strSql = new StringBuilder();
            DataTable result;
            if (!string.IsNullOrEmpty(object_id) && object_id != "未选择")
            {
                strSql.Append("SELECT\r\n                                     [列名]=a.name,\r\n                                     [数据类型]=b.name,\r\n                                     [长度]=COLUMNPROPERTY(a.id,a.name,'PRECISION'),\r\n                                     [是否为空]=case when a.isnullable=1 then '√'else '' end,\r\n                                     [默认值]=isnull(e.text,''),\r\n                                     [说明]=isnull(g.[value],'未填说明')\r\n                                     FROM syscolumns a\r\n                                     left join systypes b on a.xusertype=b.xusertype\r\n                                     inner join sysobjects d on a.id=d.id  and d.xtype='U' and  d.name<>'dtproperties'\r\n                                     left join syscomments e on a.cdefault=e.id\r\n                                     left join sys.extended_properties g on a.id=g.major_id and a.colid=g.minor_id \r\n                                     left join sys.extended_properties f on d.id=f.major_id and f.minor_id=0");
                strSql.Append("where d.name='" + object_id + "' order by a.id,a.colorder");
                result = DataFactory.SqlDataBase().GetDataTableBySQL(strSql);
            }
            else
            {
                result = dt;
            }
            return result;
        }
    }
}