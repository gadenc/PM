using RM.Busines;
using RM.Busines.DAL;
using RM.Busines.IDAO;
using RM.Common.DotNetBean;
using RM.Common.DotNetCode;
using RM.Common.DotNetData;
using RM.Common.DotNetEncrypt;
using RM.Common.DotNetUI;
using RM.Web.App_Code;
using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace RM.Web.RMBase.SysUser
{
	public class UserInfo_Form : PageBase
	{
		public StringBuilder str_OutputHtml = new StringBuilder();
		public StringBuilder strOrgHtml = new StringBuilder();
		public StringBuilder strRoleHtml = new StringBuilder();
		public StringBuilder strUserGroupHtml = new StringBuilder();
		public StringBuilder strUserRightHtml = new StringBuilder();
		private RM_System_IDAO systemidao = new RM_System_Dal();
		private RM_UserInfo_IDAO user_idao = new RM_UserInfo_Dal();
		private string _key;
		private string Property_Function = "用户附加信息";
		protected HtmlForm form1;
		protected HtmlInputHidden checkbox_value;
		protected HtmlInputHidden AppendProperty_value;
		protected HtmlInputText User_Code;
		protected HtmlInputText User_Name;
		protected HtmlInputText User_Account;
		protected HtmlInputText User_Pwd;
		protected HtmlSelect User_Sex;
		protected HtmlInputText Email;
		protected HtmlInputText CreateUserName;
		protected HtmlInputText CreateDate;
		protected HtmlInputText ModifyUserName;
		protected HtmlInputText ModifyDate;
		protected new HtmlInputText Title;
		protected HtmlTextArea User_Remark;
		protected LinkButton Save;
		protected void Page_Load(object sender, EventArgs e)
		{
			this._key = base.Request["key"];
			this.str_OutputHtml.Append(this.systemidao.AppendProperty_Html(this.Property_Function));
			this.CreateUserName.Value = RequestSession.GetSessionUser().UserName.ToString();
			this.CreateDate.Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
			this.InitInfoOrg();
			this.InitInfoRole();
			this.InitUserGroup();
			this.InitUserRight();
			if (!base.IsPostBack)
			{
				if (!string.IsNullOrEmpty(this._key))
				{
					this.InitData();
				}
			}
		}
		private void InitData()
		{
			Hashtable ht = DataFactory.SqlDataBase().GetHashtableById("Base_UserInfo", "User_ID", this._key);
			if (ht.Count > 0 && ht != null)
			{
				ControlBindHelper.SetWebControls(this.Page, ht);
				this.User_Pwd.Value = "*************";
				this.AppendProperty_value.Value = this.systemidao.GetPropertyInstancepk(this.Property_Function, this._key);
			}
		}
		public void InitInfoOrg()
		{
			DataTable dtOrg = this.user_idao.GetOrganizeList();
			DataTable dtStaffOrganize = this.user_idao.InitStaffOrganize(this._key);
			if (DataTableHelper.IsExistRows(dtOrg))
			{
				foreach (DataRowView drv in new DataView(dtOrg)
				{
					RowFilter = "ParentId = '0'"
				})
				{
					this.strOrgHtml.Append("<li>");
					this.strOrgHtml.Append(string.Concat(new object[]
					{
						"<div><input style='vertical-align: middle;margin-bottom:2px;' type=\"checkbox\" ",
						this.GetChecked("Organization_ID", drv["Organization_ID"].ToString(), dtStaffOrganize),
						" value=\"",
						drv["Organization_ID"],
						"|所属部门\" name=\"checkbox\" />"
					}));
					this.strOrgHtml.Append(drv["Organization_Name"].ToString() + "</div>");
					this.strOrgHtml.Append(this.GetTreeNodeOrg(drv["Organization_ID"].ToString(), dtOrg, dtStaffOrganize));
					this.strOrgHtml.Append("</li>");
				}
			}
			else
			{
				this.strOrgHtml.Append("<li>");
				this.strOrgHtml.Append("<div><span style='color:red;'>暂无数据</span></div>");
				this.strOrgHtml.Append("</li>");
			}
		}
		public string GetTreeNodeOrg(string parentID, DataTable dtNode, DataTable dtStaffOrganize)
		{
			StringBuilder sb_TreeNode = new StringBuilder();
			DataView dv = new DataView(dtNode);
			dv.RowFilter = "ParentId = '" + parentID + "'";
			if (dv.Count > 0)
			{
				sb_TreeNode.Append("<ul>");
				foreach (DataRowView drv in dv)
				{
					sb_TreeNode.Append("<li>");
					sb_TreeNode.Append("<div class='treeview-file'>");
					sb_TreeNode.Append(string.Concat(new object[]
					{
						"<input style='vertical-align: middle;margin-bottom:2px;' type=\"checkbox\" ",
						this.GetChecked("Organization_ID", drv["Organization_ID"].ToString(), dtStaffOrganize),
						" value=\"",
						drv["Organization_ID"],
						"|所属部门\" name=\"checkbox\" />"
					}));
					sb_TreeNode.Append(drv["Organization_Name"].ToString() + "</div>");
					sb_TreeNode.Append(this.GetTreeNodeOrg(drv["Organization_ID"].ToString(), dtNode, dtStaffOrganize));
					sb_TreeNode.Append("</li>");
				}
				sb_TreeNode.Append("</ul>");
			}
			return sb_TreeNode.ToString();
		}
		public void InitInfoRole()
		{
			DataTable dtRole = this.systemidao.InitRoleList();
			DataTable dtUserRole = this.user_idao.InitUserRole(this._key);
			if (DataTableHelper.IsExistRows(dtRole))
			{
				foreach (DataRowView drv in new DataView(dtRole)
				{
					RowFilter = "ParentId = '0'"
				})
				{
					this.strRoleHtml.Append("<li>");
					this.strRoleHtml.Append(string.Concat(new object[]
					{
						"<div><input style='vertical-align: middle;margin-bottom:2px;' type=\"checkbox\" ",
						this.GetChecked("Roles_ID", drv["Roles_ID"].ToString(), dtUserRole),
						" value=\"",
						drv["Roles_ID"],
						"|所属角色\" name=\"checkbox\" />"
					}));
					this.strRoleHtml.Append(drv["Roles_Name"].ToString() + "</div>");
					this.strRoleHtml.Append(this.GetTreeNodeRole(drv["Roles_ID"].ToString(), dtRole, dtUserRole));
					this.strRoleHtml.Append("</li>");
				}
			}
			else
			{
				this.strRoleHtml.Append("<li>");
				this.strRoleHtml.Append("<div><span style='color:red;'>暂无数据</span></div>");
				this.strRoleHtml.Append("</li>");
			}
		}
		public string GetTreeNodeRole(string parentID, DataTable dtNode, DataTable dtUserRole)
		{
			StringBuilder sb_TreeNode = new StringBuilder();
			DataView dv = new DataView(dtNode);
			dv.RowFilter = "ParentId = '" + parentID + "'";
			if (dv.Count > 0)
			{
				sb_TreeNode.Append("<ul>");
				foreach (DataRowView drv in dv)
				{
					sb_TreeNode.Append("<li>");
					sb_TreeNode.Append("<div class='treeview-file'>");
					sb_TreeNode.Append(string.Concat(new object[]
					{
						"<input style='vertical-align: middle;margin-bottom:2px;' type=\"checkbox\" ",
						this.GetChecked("Roles_ID", drv["Roles_ID"].ToString(), dtUserRole),
						" value=\"",
						drv["Roles_ID"],
						"|所属角色\" name=\"checkbox\" />"
					}));
					sb_TreeNode.Append(drv["Roles_Name"].ToString() + "</div>");
					sb_TreeNode.Append(this.GetTreeNodeRole(drv["Roles_ID"].ToString(), dtNode, dtUserRole));
					sb_TreeNode.Append("</li>");
				}
				sb_TreeNode.Append("</ul>");
			}
			return sb_TreeNode.ToString();
		}
		public void InitUserGroup()
		{
			DataTable dtUserGroupList = this.user_idao.InitUserGroupList();
			DataTable dtUserInfoUserGroup = this.user_idao.InitUserInfoUserGroup(this._key);
			if (DataTableHelper.IsExistRows(dtUserGroupList))
			{
				foreach (DataRowView drv in new DataView(dtUserGroupList)
				{
					RowFilter = "ParentId = '0'"
				})
				{
					this.strUserGroupHtml.Append("<li>");
					this.strUserGroupHtml.Append(string.Concat(new object[]
					{
						"<div><input style='vertical-align: middle;margin-bottom:2px;' type=\"checkbox\" ",
						this.GetChecked("UserGroup_ID", drv["UserGroup_ID"].ToString(), dtUserInfoUserGroup),
						" value=\"",
						drv["UserGroup_ID"],
						"|用户工作组\" name=\"checkbox\" />"
					}));
					this.strUserGroupHtml.Append(drv["UserGroup_Name"].ToString() + "</div>");
					this.strUserGroupHtml.Append(this.GetTreeNodeUserGroup(drv["UserGroup_ID"].ToString(), dtUserGroupList, dtUserInfoUserGroup));
					this.strUserGroupHtml.Append("</li>");
				}
			}
			else
			{
				this.strUserGroupHtml.Append("<li>");
				this.strUserGroupHtml.Append("<div><span style='color:red;'>暂无数据</span></div>");
				this.strUserGroupHtml.Append("</li>");
			}
		}
		public string GetTreeNodeUserGroup(string parentID, DataTable dtNode, DataTable dtUserInfoUserGroup)
		{
			StringBuilder sb_TreeNode = new StringBuilder();
			DataView dv = new DataView(dtNode);
			dv.RowFilter = "ParentId = '" + parentID + "'";
			if (dv.Count > 0)
			{
				sb_TreeNode.Append("<ul>");
				foreach (DataRowView drv in dv)
				{
					sb_TreeNode.Append("<li>");
					sb_TreeNode.Append("<div class='treeview-file'>");
					sb_TreeNode.Append(string.Concat(new object[]
					{
						"<input style='vertical-align: middle;margin-bottom:2px;' type=\"checkbox\" ",
						this.GetChecked("UserGroup_ID", drv["UserGroup_ID"].ToString(), dtUserInfoUserGroup),
						" value=\"",
						drv["UserGroup_ID"],
						"|用户工作组\" name=\"checkbox\" />"
					}));
					sb_TreeNode.Append(drv["UserGroup_Name"].ToString() + "</div>");
					sb_TreeNode.Append(this.GetTreeNodeUserGroup(drv["UserGroup_ID"].ToString(), dtNode, dtUserInfoUserGroup));
					sb_TreeNode.Append("</li>");
				}
				sb_TreeNode.Append("</ul>");
			}
			return sb_TreeNode.ToString();
		}
		public void InitUserRight()
		{
			DataTable dtList = this.systemidao.GetMenuBind();
			DataTable dtUserRight = this.user_idao.InitUserRight(this._key);
			if (DataTableHelper.IsExistRows(dtList))
			{
				DataTable dtButoon = DataTableHelper.GetNewDataTable(dtList, "Menu_Type = '3'");
				DataTable dtMenu = DataTableHelper.GetNewDataTable(dtList, "Menu_Type < '3'");
				DataView dv = new DataView(dtMenu);
				dv.RowFilter = " ParentId = '0'";
				int eRowIndex = 0;
				foreach (DataRowView drv in dv)
				{
					string trID = "node-" + eRowIndex.ToString();
					this.strUserRightHtml.Append("<tr id='" + trID + "'>");
					this.strUserRightHtml.Append("<td style='width: 200px;padding-left:20px;'><span class=\"folder\">" + drv["Menu_Name"] + "</span></td>");
					if (!string.IsNullOrEmpty(drv["Menu_Img"].ToString()))
					{
						this.strUserRightHtml.Append("<td style='width: 30px;text-align:center;'><img src='/Themes/images/32/" + drv["Menu_Img"] + "' style='width:16px; height:16px;vertical-align: middle;' alt=''/></td>");
					}
					else
					{
						this.strUserRightHtml.Append("<td style='width: 30px;text-align:center;'><img src='/Themes/images/32/5005_flag.png' style='width:16px; height:16px;vertical-align: middle;' alt=''/></td>");
					}
					this.strUserRightHtml.Append(string.Concat(new object[]
					{
						"<td style=\"width: 23px; text-align: left;\"><input id='ckb",
						trID,
						"' onclick=\"ckbValueObj(this.id)\" style='vertical-align: middle;margin-bottom:2px;' type=\"checkbox\" ",
						this.GetChecked("Menu_Id", drv["Menu_Id"].ToString(), dtUserRight),
						"  value=\"",
						drv["Menu_Id"],
						"|用户权限\" name=\"checkbox\" /></td>"
					}));
					this.strUserRightHtml.Append("<td>" + this.GetButton(drv["Menu_Id"].ToString(), dtButoon, trID, dtUserRight) + "</td>");
					this.strUserRightHtml.Append("</tr>");
					this.strUserRightHtml.Append(this.GetTreeNodeUserRight(drv["Menu_Id"].ToString(), dtMenu, trID, dtButoon, dtUserRight));
					eRowIndex++;
				}
			}
		}
		public string GetTreeNodeUserRight(string parentID, DataTable dtMenu, string parentTRID, DataTable dtButoon, DataTable dtUserRight)
		{
			StringBuilder sb_TreeNode = new StringBuilder();
			DataView dv = new DataView(dtMenu);
			dv.RowFilter = "ParentId = '" + parentID + "'";
			int i = 1;
			foreach (DataRowView drv in dv)
			{
				string trID = parentTRID + "-" + i.ToString();
				sb_TreeNode.Append(string.Concat(new string[]
				{
					"<tr id='",
					trID,
					"' class='child-of-",
					parentTRID,
					"'>"
				}));
				sb_TreeNode.Append("<td style='padding-left:20px;'><span class=\"folder\">" + drv["Menu_Name"].ToString() + "</span></td>");
				if (!string.IsNullOrEmpty(drv["Menu_Img"].ToString()))
				{
					sb_TreeNode.Append("<td style='width: 30px;text-align:center;'><img src='/Themes/images/32/" + drv["Menu_Img"].ToString() + "' style='width:16px; height:16px;vertical-align: middle;' alt=''/></td>");
				}
				else
				{
					sb_TreeNode.Append("<td style='width: 30px;text-align:center;'><img src='/Themes/images/32/5005_flag.png' style='width:16px; height:16px;vertical-align: middle;' alt=''/></td>");
				}
				sb_TreeNode.Append(string.Concat(new object[]
				{
					"<td style=\"width: 23px; text-align: left;\"><input id='ckb",
					trID,
					"' onclick=\"ckbValueObj(this.id)\" ",
					this.GetChecked("Menu_Id", drv["Menu_Id"].ToString(), dtUserRight),
					" style='vertical-align: middle;margin-bottom:2px;' type=\"checkbox\" value=\"",
					drv["Menu_Id"],
					"|用户权限\" name=\"checkbox\" /></td>"
				}));
				sb_TreeNode.Append("<td>" + this.GetButton(drv["Menu_Id"].ToString(), dtButoon, trID, dtUserRight) + "</td>");
				sb_TreeNode.Append("</tr>");
				sb_TreeNode.Append(this.GetTreeNodeUserRight(drv["Menu_Id"].ToString(), dtMenu, trID, dtButoon, dtUserRight));
				i++;
			}
			return sb_TreeNode.ToString();
		}
		public string GetButton(string Menu_Id, DataTable dt, string parentTRID, DataTable dtUserRight)
		{
			StringBuilder ButtonHtml = new StringBuilder();
			DataTable dt_Button = DataTableHelper.GetNewDataTable(dt, "ParentId = '" + Menu_Id + "'");
			string result;
			if (DataTableHelper.IsExistRows(dt_Button))
			{
				int i = 1;
				foreach (DataRow drv in dt_Button.Rows)
				{
					string trID = parentTRID + "--" + i.ToString();
					ButtonHtml.Append(string.Concat(new object[]
					{
						"<lable><input id='ckb",
						trID,
						"' onclick=\"ckbValueObj(this.id)\" ",
						this.GetChecked("Menu_Id", drv["Menu_Id"].ToString(), dtUserRight),
						" style='vertical-align: middle;margin-bottom:2px;' type=\"checkbox\" value=\"",
						drv["Menu_Id"],
						"|用户权限\" name=\"checkbox\" />"
					}));
					ButtonHtml.Append(drv["Menu_Name"].ToString() + "</lable>&nbsp;&nbsp;&nbsp;&nbsp;");
					i++;
				}
				result = ButtonHtml.ToString();
			}
			else
			{
				result = ButtonHtml.ToString();
			}
			return result;
		}
		public string GetChecked(string pkName, string Obj_Val, DataTable dt)
		{
			StringBuilder strSql = new StringBuilder();
			dt = DataTableHelper.GetNewDataTable(dt, pkName + " = '" + Obj_Val + "'");
			string result;
			if (DataTableHelper.IsExistRows(dt))
			{
				result = "checked=\"checked\"";
			}
			else
			{
				result = "";
			}
			return result;
		}
		protected void Save_Click(object sender, EventArgs e)
		{
			string guid = CommonHelper.GetGuid;
			Hashtable ht = new Hashtable();
			ht["User_Code"] = this.User_Code.Value;
			ht["User_Name"] = this.User_Name.Value;
			ht["User_Account"] = this.User_Account.Value;
			ht["User_Pwd"] = Md5Helper.MD5(this.User_Pwd.Value, 32);
			ht["User_Sex"] = this.User_Sex.Value;
			ht["Email"] = this.Email.Value;
			ht["Title"] = this.Title.Value;
			ht["User_Remark"] = this.User_Remark.Value;
			if (!string.IsNullOrEmpty(this._key))
			{
				guid = this._key;
				ht["ModifyDate"] = DateTime.Now;
				ht.Remove("User_Pwd");
				ht["ModifyUserId"] = RequestSession.GetSessionUser().UserId;
				ht["ModifyUserName"] = RequestSession.GetSessionUser().UserName;
			}
			else
			{
				ht["User_ID"] = guid;
				ht["CreateUserId"] = RequestSession.GetSessionUser().UserId;
				ht["CreateUserName"] = RequestSession.GetSessionUser().UserName;
			}
			bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit("Base_UserInfo", "User_ID", this._key, ht);
			if (IsOk)
			{
				IsOk = this.systemidao.Add_AppendPropertyInstance(guid, this.AppendProperty_value.Value.Split(new char[]
				{
					';'
				}));
				if (IsOk)
				{
					IsOk = this.add_ItemForm(this.checkbox_value.Value.Split(new char[]
					{
						','
					}), guid);
				}
			}
			if (IsOk)
			{
				ShowMsgHelper.ParmAlertMsg("操作成功！");
			}
			else
			{
				ShowMsgHelper.Alert_Error("操作失败！");
			}
		}
		public bool add_ItemForm(string[] item_value, string user_id)
		{
			bool result;
			try
			{
				StringBuilder[] sqls = new StringBuilder[item_value.Length + 4];
				object[] objs = new object[item_value.Length + 4];
				StringBuilder sbDelete_org = new StringBuilder();
				sbDelete_org.Append("Delete From Base_StaffOrganize Where User_ID =@User_ID");
				SqlParam[] parm_org = new SqlParam[]
				{
					new SqlParam("@User_ID", user_id)
				};
				sqls[0] = sbDelete_org;
				objs[0] = parm_org;
				StringBuilder sbDelete_Role = new StringBuilder();
				sbDelete_Role.Append("Delete From Base_UserRole Where User_ID =@User_ID");
				SqlParam[] parm_Role = new SqlParam[]
				{
					new SqlParam("@User_ID", user_id)
				};
				sqls[1] = sbDelete_Role;
				objs[1] = parm_Role;
				StringBuilder sbDelete_UserGroup = new StringBuilder();
				sbDelete_UserGroup.Append("Delete From Base_UserInfoUserGroup Where User_ID =@User_ID");
				SqlParam[] parm_UserGroup = new SqlParam[]
				{
					new SqlParam("@User_ID", user_id)
				};
				sqls[2] = sbDelete_UserGroup;
				objs[2] = parm_UserGroup;
				StringBuilder sbDelete_Right = new StringBuilder();
				sbDelete_Right.Append("Delete From Base_UserRight Where User_ID =@User_ID");
				SqlParam[] parm_Right = new SqlParam[]
				{
					new SqlParam("@User_ID", user_id)
				};
				sqls[3] = sbDelete_Right;
				objs[3] = parm_Right;
				int index = 4;
				for (int i = 0; i < item_value.Length; i++)
				{
					string item = item_value[i];
					if (item.Length > 0)
					{
						string[] str_item = item.Split(new char[]
						{
							'|'
						});
						string key = str_item[0];
						string type = str_item[1];
						if (type == "所属部门")
						{
							StringBuilder sbadd = new StringBuilder();
							sbadd.Append("Insert into Base_StaffOrganize(");
							sbadd.Append("StaffOrganize_Id,Organization_ID,User_ID,CreateUserId,CreateUserName");
							sbadd.Append(")Values(");
							sbadd.Append("@StaffOrganize_Id,@Organization_ID,@User_ID,@CreateUserId,@CreateUserName)");
							SqlParam[] parmAdd = new SqlParam[]
							{
								new SqlParam("@StaffOrganize_Id", CommonHelper.GetGuid),
								new SqlParam("@Organization_ID", key),
								new SqlParam("@User_ID", user_id),
								new SqlParam("@CreateUserId", RequestSession.GetSessionUser().UserId),
								new SqlParam("@CreateUserName", RequestSession.GetSessionUser().UserName)
							};
							sqls[index] = sbadd;
							objs[index] = parmAdd;
						}
						else
						{
							if (type == "所属角色")
							{
								StringBuilder sbadd = new StringBuilder();
								sbadd.Append("Insert into Base_UserRole(");
								sbadd.Append("UserRole_ID,User_ID,Roles_ID,CreateUserId,CreateUserName");
								sbadd.Append(")Values(");
								sbadd.Append("@UserRole_ID,@User_ID,@Roles_ID,@CreateUserId,@CreateUserName)");
								SqlParam[] parmAdd = new SqlParam[]
								{
									new SqlParam("@UserRole_ID", CommonHelper.GetGuid),
									new SqlParam("@User_ID", user_id),
									new SqlParam("@Roles_ID", key),
									new SqlParam("@CreateUserId", RequestSession.GetSessionUser().UserId),
									new SqlParam("@CreateUserName", RequestSession.GetSessionUser().UserName)
								};
								sqls[index] = sbadd;
								objs[index] = parmAdd;
							}
							else
							{
								if (type == "用户工作组")
								{
									StringBuilder sbadd = new StringBuilder();
									sbadd.Append("Insert into Base_UserInfoUserGroup(");
									sbadd.Append("UserInfoUserGroup_ID,User_ID,UserGroup_ID,CreateUserId,CreateUserName");
									sbadd.Append(")Values(");
									sbadd.Append("@UserInfoUserGroup_ID,@User_ID,@UserGroup_ID,@CreateUserId,@CreateUserName)");
									SqlParam[] parmAdd = new SqlParam[]
									{
										new SqlParam("@UserInfoUserGroup_ID", CommonHelper.GetGuid),
										new SqlParam("@User_ID", user_id),
										new SqlParam("@UserGroup_ID", key),
										new SqlParam("@CreateUserId", RequestSession.GetSessionUser().UserId),
										new SqlParam("@CreateUserName", RequestSession.GetSessionUser().UserName)
									};
									sqls[index] = sbadd;
									objs[index] = parmAdd;
								}
								else
								{
									if (type == "用户权限")
									{
										StringBuilder sbadd = new StringBuilder();
										sbadd.Append("Insert into Base_UserRight(");
										sbadd.Append("UserRight_ID,User_ID,Menu_Id,CreateUserId,CreateUserName");
										sbadd.Append(")Values(");
										sbadd.Append("@UserRight_ID,@User_ID,@Menu_Id,@CreateUserId,@CreateUserName)");
										SqlParam[] parmAdd = new SqlParam[]
										{
											new SqlParam("@UserRight_ID", CommonHelper.GetGuid),
											new SqlParam("@User_ID", user_id),
											new SqlParam("@Menu_Id", key),
											new SqlParam("@CreateUserId", RequestSession.GetSessionUser().UserId),
											new SqlParam("@CreateUserName", RequestSession.GetSessionUser().UserName)
										};
										sqls[index] = sbadd;
										objs[index] = parmAdd;
									}
								}
							}
						}
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
