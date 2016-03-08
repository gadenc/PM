using RM.Busines;
using RM.Busines.DAL;
using RM.Busines.IDAO;
using RM.Common.DotNetBean;
using RM.Common.DotNetCode;
using RM.Common.DotNetData;
using RM.Common.DotNetUI;
using RM.Web.App_Code;
using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace RM.Web.RMBase.SysRole
{
	public class Role_Form : PageBase
	{
		public StringBuilder str_allUserInfo = new StringBuilder();
		public StringBuilder str_seleteUserInfo = new StringBuilder();
		private RM_UserInfo_IDAO user_idao = new RM_UserInfo_Dal();
		private RM_System_IDAO system_idao = new RM_System_Dal();
		private string _key;
		private string _ParentId;
		private int index_TreeNode = 0;
		protected HtmlForm form1;
		protected HtmlInputHidden User_ID_Hidden;
		protected HtmlInputText Roles_Name;
		protected HtmlSelect ParentId;
		protected HtmlInputText SortCode;
		protected HtmlTextArea Roles_Remark;
		protected LinkButton Save;
		protected void Page_Load(object sender, EventArgs e)
		{
			this._key = base.Request["key"];
			this._ParentId = base.Request["ParentId"];
			this.InitUserInfo();
			if (!base.IsPostBack)
			{
				this.InitParentId();
				if (!string.IsNullOrEmpty(this._ParentId))
				{
					this.ParentId.Value = this._ParentId;
				}
				if (!string.IsNullOrEmpty(this._key))
				{
					this.InitData();
				}
			}
		}
		private void InitData()
		{
			Hashtable ht = DataFactory.SqlDataBase().GetHashtableById("Base_Roles", "Roles_ID", this._key);
			if (ht.Count > 0 && ht != null)
			{
				ControlBindHelper.SetWebControls(this.Page, ht);
			}
		}
		private void InitParentId()
		{
			DataTable dt = this.system_idao.InitRoleParentId();
			if (!string.IsNullOrEmpty(this._key))
			{
				if (DataTableHelper.IsExistRows(dt))
				{
					for (int i = 0; i < dt.Rows.Count; i++)
					{
						if (dt.Rows[i]["Roles_ID"].ToString() == this._key)
						{
							dt.Rows.RemoveAt(i);
						}
					}
				}
			}
			ControlBindHelper.BindHtmlSelect(dt, this.ParentId, "Roles_Name", "Roles_ID", "角色信息 - 父节");
		}
		public void InitUserRole()
		{
			if (!string.IsNullOrEmpty(this._key))
			{
				DataTable dt = this.system_idao.InitUserRole(this._key);
				if (DataTableHelper.IsExistRows(dt))
				{
					foreach (DataRow drv in dt.Rows)
					{
						this.str_seleteUserInfo.Append(string.Concat(new object[]
						{
							"<tr ondblclick='$(this).remove()'><td>",
							drv["User_Name"],
							"</td><td>",
							drv["Organization_Name"],
							"</td><td  style='display:none'>",
							drv["User_ID"],
							"</td></tr>"
						}));
					}
				}
			}
		}
		public void InitUserInfo()
		{
			this.InitUserRole();
			DataTable dt_Org = this.user_idao.Load_StaffOrganizeList();
			foreach (DataRowView drv in new DataView(dt_Org)
			{
				RowFilter = "ParentId = '0'"
			})
			{
				DataTable GetNewData = DataTableHelper.GetNewDataTable(dt_Org, "ParentId = '" + drv["Organization_ID"].ToString() + "'");
				if (DataTableHelper.IsExistRows(GetNewData))
				{
					this.str_allUserInfo.Append("<li>");
					this.str_allUserInfo.Append("<div>" + drv["Organization_Name"].ToString() + "</div>");
					this.str_allUserInfo.Append(this.GetTreeNode(drv["Organization_ID"].ToString(), drv["Organization_Name"].ToString(), dt_Org, "1"));
					this.str_allUserInfo.Append("</li>");
				}
			}
		}
		public string GetTreeNode(string parentID, string parentName, DataTable dtNode, string status)
		{
			StringBuilder sb_TreeNode = new StringBuilder();
			DataTable GetNewData = new DataTable();
			DataView dv = new DataView(dtNode);
			dv.RowFilter = "ParentId = '" + parentID + "'";
			if (dv.Count > 0)
			{
				if (this.index_TreeNode == 0)
				{
					sb_TreeNode.Append("<ul>");
				}
				else
				{
					sb_TreeNode.Append("<ul style='display: none'>");
				}
				foreach (DataRowView drv in dv)
				{
					GetNewData = DataTableHelper.GetNewDataTable(dtNode, "ParentId = '" + drv["Organization_ID"].ToString() + "'");
					if (drv["isUser"].ToString() == "0")
					{
						if (DataTableHelper.IsExistRows(GetNewData))
						{
							sb_TreeNode.Append("<li>");
							sb_TreeNode.Append("<div>" + drv["Organization_Name"] + "</div>");
							sb_TreeNode.Append(this.GetTreeNode(drv["Organization_ID"].ToString(), drv["Organization_Name"].ToString(), dtNode, "2"));
							sb_TreeNode.Append("</li>");
						}
					}
					else
					{
						if (status != "1")
						{
							sb_TreeNode.Append("<li>");
							sb_TreeNode.Append(string.Concat(new object[]
							{
								"<div ondblclick=\"addUserInfo('",
								drv["Organization_Name"],
								"','",
								drv["Organization_ID"],
								"','",
								parentName,
								"')\">"
							}));
							sb_TreeNode.Append("<img src=\"/Themes/Images/user_mature.png\" width=\"16\" height=\"16\" />" + drv["Organization_Name"].ToString() + "</div>");
							sb_TreeNode.Append("</li>");
						}
					}
				}
				sb_TreeNode.Append("</ul>");
			}
			this.index_TreeNode++;
			return sb_TreeNode.ToString();
		}
		protected void Save_Click(object sender, EventArgs e)
		{
			string guid = CommonHelper.GetGuid;
			Hashtable ht = new Hashtable();
			ht = ControlBindHelper.GetWebControls(this.Page);
			ht.Remove("User_ID_Hidden");
			if (this.ParentId.Value == "")
			{
				ht["ParentId"] = "0";
			}
			if (!string.IsNullOrEmpty(this._key))
			{
				guid = this._key;
				ht["ModifyDate"] = DateTime.Now;
				ht["ModifyUserId"] = RequestSession.GetSessionUser().UserId;
				ht["ModifyUserName"] = RequestSession.GetSessionUser().UserName;
			}
			else
			{
				ht["Roles_ID"] = guid;
				ht["CreateUserId"] = RequestSession.GetSessionUser().UserId;
				ht["CreateUserName"] = RequestSession.GetSessionUser().UserName;
			}
			bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit("Base_Roles", "Roles_ID", this._key, ht);
			if (IsOk)
			{
				string str = this.User_ID_Hidden.Value;
				if (!string.IsNullOrEmpty(str))
				{
					str = this.User_ID_Hidden.Value.Substring(0, this.User_ID_Hidden.Value.Length - 1);
				}
				bool IsAllto = this.system_idao.Add_RoleAllotMember(str.Split(new char[]
				{
					','
				}), guid);
				if (IsAllto)
				{
					ShowMsgHelper.AlertMsg("操作成功！");
				}
				else
				{
					ShowMsgHelper.Alert_Error("操作失败！");
				}
			}
			else
			{
				ShowMsgHelper.Alert_Error("操作失败！");
			}
		}
	}
}
