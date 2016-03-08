using RM.Busines.DAL;
using RM.Busines.IDAO;
using RM.Common.DotNetData;
using RM.Common.DotNetUI;
using RM.Web.App_Code;
using RM.Web;
using System;
using System.Data;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using RM.Web.UserControl;

namespace RM.Web.RMBase.SysRole
{
	public class AllotAuthority_Form : PageBase
	{
		public StringBuilder StrTree_Menu = new StringBuilder();
		private RM_System_IDAO system_idao = new RM_System_Dal();
		public string _Roles_Name;
		public string _key;
		protected HtmlForm form1;
		protected HtmlInputHidden item_hidden;
		protected Button Save;
		protected LoadButton LoadButton1;
		protected void Page_Load(object sender, EventArgs e)
		{
			this._Roles_Name = base.Server.UrlDecode(base.Request["Roles_Name"]);
			this._key = base.Request["key"];
			this.GetMenuTreeTable();
		}
		public void GetMenuTreeTable()
		{
			DataTable dtList = this.system_idao.GetMenuBind();
			DataTable dtRoleRight = this.system_idao.InitRoleRight(this._key);
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
						this.GetChecked(drv["Menu_Id"].ToString(), dtRoleRight),
						"  value=\"",
						drv["Menu_Id"],
						"\" name=\"checkbox\" /></td>"
					}));
					this.StrTree_Menu.Append("<td>" + this.GetButton(drv["Menu_Id"].ToString(), dtButoon, trID, dtRoleRight) + "</td>");
					this.StrTree_Menu.Append("</tr>");
					this.StrTree_Menu.Append(this.GetTableTreeNode(drv["Menu_Id"].ToString(), dtMenu, trID, dtButoon, dtRoleRight));
					eRowIndex++;
				}
			}
		}
		public string GetTableTreeNode(string parentID, DataTable dtMenu, string parentTRID, DataTable dtButoon, DataTable dtRoleRight)
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
					this.GetChecked(drv["Menu_Id"].ToString(), dtRoleRight),
					" style='vertical-align: middle;margin-bottom:2px;' type=\"checkbox\" value=\"",
					drv["Menu_Id"],
					"\" name=\"checkbox\" /></td>"
				}));
				sb_TreeNode.Append("<td>" + this.GetButton(drv["Menu_Id"].ToString(), dtButoon, trID, dtRoleRight) + "</td>");
				sb_TreeNode.Append("</tr>");
				sb_TreeNode.Append(this.GetTableTreeNode(drv["Menu_Id"].ToString(), dtMenu, trID, dtButoon, dtRoleRight));
				i++;
			}
			return sb_TreeNode.ToString();
		}
		public string GetButton(string Menu_Id, DataTable dt, string parentTRID, DataTable dtRoleRight)
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
						this.GetChecked(drv["Menu_Id"].ToString(), dtRoleRight),
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
		protected void Save_Click(object sender, EventArgs e)
		{
			string str = this.item_hidden.Value;
			bool IsAllto = this.system_idao.Add_RoleAllotAuthority(str.Split(new char[]
			{
				','
			}), this._key);
			if (IsAllto)
			{
				ShowMsgHelper.ShowScript("back();showTipsMsg('操作成功！','2500','4');");
			}
			else
			{
				ShowMsgHelper.Alert_Error("操作失败！");
			}
		}
	}
}
