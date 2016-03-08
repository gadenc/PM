using RM.Busines.DAL;
using RM.Busines.IDAO;
using RM.Common.DotNetCode;
using RM.Web.App_Code;
using RM.Web;
using System;
using System.Data;
using System.Text;
using System.Web.UI.HtmlControls;
using RM.Web.UserControl;

namespace RM.Web.RMBase.SysUserGroup
{
	public class UserGroup_List : PageBase
	{
		public StringBuilder str_tableTree = new StringBuilder();
		private RM_UserInfo_IDAO user_idao = new RM_UserInfo_Dal();
		protected HtmlForm form1;
		protected LoadButton LoadButton1;
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!base.IsPostBack)
			{
				this.GetTreeTable();
			}
		}
		public void GetTreeTable()
		{
			DataTable dtUserGroup = this.user_idao.InitUserGroupList();
			DataView dv = new DataView(dtUserGroup);
			dv.RowFilter = "ParentId = '0'";
			int eRowIndex = 0;
			foreach (DataRowView drv in dv)
			{
				string trID = "node-" + eRowIndex.ToString();
				this.str_tableTree.Append("<tr id='" + trID + "'>");
				this.str_tableTree.Append("<td style='width: 180px;padding-left:20px;'><span class=\"folder\">" + drv["UserGroup_Name"].ToString() + "</span></td>");
				this.str_tableTree.Append("<td style='width: 60px;text-align:center;'>" + drv["UserGroup_Code"].ToString() + "</td>");
				this.str_tableTree.Append("<td style='width: 60px;text-align:center;'>" + drv["SortCode"].ToString() + "</td>");
				this.str_tableTree.Append("<td style='width: 120px;text-align:center;'>" + drv["CreateUserName"].ToString() + "</td>");
				this.str_tableTree.Append("<td style='width: 120px;text-align:center;'>" + CommonHelper.GetFormatDateTime(drv["CreateDate"], "yyyy-MM-dd HH:mm") + "</td>");
				this.str_tableTree.Append("<td style='width: 120px;text-align:center;'>" + drv["ModifyUserName"].ToString() + "</td>");
				this.str_tableTree.Append("<td style='width: 120px;text-align:center;'>" + CommonHelper.GetFormatDateTime(drv["ModifyDate"].ToString(), "yyyy-MM-dd HH:mm") + "</td>");
				this.str_tableTree.Append("<td>" + drv["UserGroup_Remark"].ToString() + "</td>");
				this.str_tableTree.Append("<td style='display:none'>" + drv["UserGroup_ID"].ToString() + "</td>");
				this.str_tableTree.Append("</tr>");
				this.str_tableTree.Append(this.GetTableTreeNode(drv["UserGroup_ID"].ToString(), dtUserGroup, trID));
				eRowIndex++;
			}
		}
		public string GetTableTreeNode(string parentID, DataTable dt, string parentTRID)
		{
			StringBuilder sb_TreeNode = new StringBuilder();
			DataView dv = new DataView(dt);
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
				sb_TreeNode.Append("<td style='padding-left:20px;'><span class=\"folder\">" + drv["UserGroup_Name"].ToString() + "</span></td>");
				sb_TreeNode.Append("<td style='width: 60px;text-align:center;'>" + drv["UserGroup_Code"].ToString() + "</td>");
				sb_TreeNode.Append("<td style='width: 60px;text-align:center;'>" + drv["SortCode"].ToString() + "</td>");
				sb_TreeNode.Append("<td style='width: 120px;text-align:center;'>" + drv["CreateUserName"].ToString() + "</td>");
				sb_TreeNode.Append("<td style='width: 120px;text-align:center;'>" + CommonHelper.GetFormatDateTime(drv["CreateDate"].ToString(), "yyyy-MM-dd HH:mm") + "</td>");
				sb_TreeNode.Append("<td style='width: 120px;text-align:center;'>" + drv["ModifyUserName"].ToString() + "</td>");
				sb_TreeNode.Append("<td style='width: 120px;text-align:center;'>" + CommonHelper.GetFormatDateTime(drv["ModifyDate"].ToString(), "yyyy-MM-dd HH:mm") + "</td>");
				sb_TreeNode.Append("<td>" + drv["UserGroup_Remark"].ToString() + "</td>");
				sb_TreeNode.Append("<td style='display:none'>" + drv["UserGroup_ID"].ToString() + "</td>");
				sb_TreeNode.Append("</tr>");
				sb_TreeNode.Append(this.GetTableTreeNode(drv["UserGroup_ID"].ToString(), dt, trID));
				i++;
			}
			return sb_TreeNode.ToString();
		}
	}
}
