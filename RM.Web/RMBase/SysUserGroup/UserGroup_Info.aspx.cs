using RM.Busines.DAL;
using RM.Busines.IDAO;
using RM.Common.DotNetData;
using RM.Common.DotNetUI;
using RM.Web.App_Code;
using System;
using System.Data;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace RM.Web.RMBase.SysUserGroup
{
	public class UserGroup_Info : PageBase
	{
		protected HtmlForm form1;
		protected HtmlInputHidden item_hidden;
		protected Repeater rp_Item;
		public StringBuilder StrTree_Menu = new StringBuilder();
		private RM_System_IDAO system_idao = new RM_System_Dal();
		private RM_UserInfo_IDAO user_idao = new RM_UserInfo_Dal();
		public string _UserGroup_ID;
		public string _UserGroup_Name;
		protected void Page_Load(object sender, EventArgs e)
		{
			this._UserGroup_ID = base.Request["UserGroup_ID"];
			this._UserGroup_Name = base.Server.UrlDecode(base.Request["UserGroup_Name"]);
			if (!base.IsPostBack)
			{
				this.GetMenuTreeTable();
				this.InitUserRole();
			}
		}
		public void InitUserRole()
		{
			if (!string.IsNullOrEmpty(this._UserGroup_ID))
			{
				DataTable dt = this.user_idao.Load_UserInfoUserGroupList(this._UserGroup_ID);
				ControlBindHelper.BindRepeaterList(dt, this.rp_Item);
			}
		}
		public void GetMenuTreeTable()
		{
			DataTable dtList = this.system_idao.GetMenuBind();
			DataTable dtUserGroupRight = this.user_idao.InitUserGroupRight(this._UserGroup_ID);
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
					this.StrTree_Menu.Append("<tr id='" + trID + "'>");
					this.StrTree_Menu.Append("<td style='width: 200px;padding-left:20px;'><span class=\"folder\">" + drv["Menu_Name"] + "</span></td>");
					if (!string.IsNullOrEmpty(drv["Menu_Img"].ToString()))
					{
						this.StrTree_Menu.Append("<td style='width: 30px;text-align:center;'><img src='/Themes/images/32/" + drv["Menu_Img"] + "' style='width:16px; height:16px;vertical-align: middle;' alt=''/></td>");
					}
					else
					{
						this.StrTree_Menu.Append("<td style='width: 30px;text-align:center;'><img src='/Themes/images/32/5005_flag.png' style='width:16px; height:16px;vertical-align: middle;' alt=''/></td>");
					}
					this.StrTree_Menu.Append(string.Concat(new object[]
					{
						"<td style=\"width: 23px; text-align: left;\"><input id='ckb",
						trID,
						"' onclick=\"ckbValueObj(this.id)\" style='vertical-align: middle;margin-bottom:2px;' type=\"checkbox\" ",
						this.GetChecked(drv["Menu_Id"].ToString(), dtUserGroupRight),
						"  value=\"",
						drv["Menu_Id"],
						"\" name=\"checkbox\" /></td>"
					}));
					this.StrTree_Menu.Append("<td>" + this.GetButton(drv["Menu_Id"].ToString(), dtButoon, trID, dtUserGroupRight) + "</td>");
					this.StrTree_Menu.Append("</tr>");
					this.StrTree_Menu.Append(this.GetTableTreeNode(drv["Menu_Id"].ToString(), dtMenu, trID, dtButoon, dtUserGroupRight));
					eRowIndex++;
				}
			}
		}
		public string GetTableTreeNode(string parentID, DataTable dtMenu, string parentTRID, DataTable dtButoon, DataTable dtUserGroupRight)
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
					this.GetChecked(drv["Menu_Id"].ToString(), dtUserGroupRight),
					" style='vertical-align: middle;margin-bottom:2px;' type=\"checkbox\" value=\"",
					drv["Menu_Id"],
					"\" name=\"checkbox\" /></td>"
				}));
				sb_TreeNode.Append("<td>" + this.GetButton(drv["Menu_Id"].ToString(), dtButoon, trID, dtUserGroupRight) + "</td>");
				sb_TreeNode.Append("</tr>");
				sb_TreeNode.Append(this.GetTableTreeNode(drv["Menu_Id"].ToString(), dtMenu, trID, dtButoon, dtUserGroupRight));
				i++;
			}
			return sb_TreeNode.ToString();
		}
		public string GetButton(string Menu_Id, DataTable dt, string parentTRID, DataTable dtUserGroupRight)
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
						this.GetChecked(drv["Menu_Id"].ToString(), dtUserGroupRight),
						" style='vertical-align: middle;margin-bottom:2px;' type=\"checkbox\" value=\"",
						drv["Menu_Id"],
						"\" name=\"checkbox\" />"
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
		public string GetChecked(string Menu_Id, DataTable dt)
		{
			StringBuilder strSql = new StringBuilder();
			dt = DataTableHelper.GetNewDataTable(dt, "Menu_Id = '" + Menu_Id + "'");
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
		protected void rp_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				Label lblUser_Sex = e.Item.FindControl("lblUser_Sex") as Label;
				Label lblDeleteMark = e.Item.FindControl("lblDeleteMark") as Label;
				if (lblUser_Sex != null)
				{
					string text = lblUser_Sex.Text;
					text = text.Replace("1", "男士");
					text = text.Replace("0", "女士");
					lblUser_Sex.Text = text;
					string textDeleteMark = lblDeleteMark.Text;
					textDeleteMark = textDeleteMark.Replace("1", "<span style='color:Blue'>未授权</span>");
					textDeleteMark = textDeleteMark.Replace("2", "正常");
					textDeleteMark = textDeleteMark.Replace("3", "<span style='color:red'>停用</span>");
					lblDeleteMark.Text = textDeleteMark;
				}
			}
		}
	}
}
