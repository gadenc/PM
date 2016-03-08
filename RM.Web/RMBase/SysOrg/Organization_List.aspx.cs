using RM.Busines.DAL;
using RM.Busines.IDAO;
using RM.Web.App_Code;
using RM.Web;
using System;
using System.Data;
using System.Text;
using System.Web.UI.HtmlControls;
using RM.Web.UserControl;

namespace RM.Web.RMBase.SysOrg
{
	public class Organization_List : PageBase
	{
		protected HtmlForm form1;
		protected LoadButton LoadButton1;
		public StringBuilder str_tableTree = new StringBuilder();
		private RM_UserInfo_IDAO user_idao = new RM_UserInfo_Dal();
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!base.IsPostBack)
			{
				this.GetTreeTable();
			}
		}
		public void GetTreeTable()
		{
			DataTable dtOrg = this.user_idao.GetOrganizeList();
			DataView dv = new DataView(dtOrg);
			dv.RowFilter = " ParentId = '0'";
			int eRowIndex = 0;
			foreach (DataRowView drv in dv)
			{
				string trID = "node-" + eRowIndex.ToString();
				this.str_tableTree.Append("<tr id='" + trID + "'>");
				this.str_tableTree.Append("<td style='width: 120px;padding-left:20px;'><span class=\"folder\">" + drv["Organization_Name"].ToString() + "</span></td>");
				this.str_tableTree.Append("<td style='width: 50px;text-align: center;'>" + drv["Organization_Code"].ToString() + "</td>");
				this.str_tableTree.Append("<td style='width: 100px;text-align: center;'>" + drv["Organization_Manager"].ToString() + "</td>");
				this.str_tableTree.Append("<td style='width: 100px;text-align: center;'>" + drv["Organization_InnerPhone"].ToString() + "</td>");
				this.str_tableTree.Append("<td style='width: 100px;text-align: center;'>" + drv["Organization_OuterPhone"].ToString() + "</td>");
				this.str_tableTree.Append("<td style='width: 100px;text-align: center;'>" + drv["Organization_Fax"].ToString() + "</td>");
				this.str_tableTree.Append("<td style='width: 50px;text-align: center;'>" + drv["Organization_Zipcode"].ToString() + "</td>");
				this.str_tableTree.Append("<td style='width: 50px;text-align: center;'>" + drv["SortCode"].ToString() + "</td>");
				this.str_tableTree.Append("<td>" + drv["Organization_Address"].ToString() + "</td>");
				this.str_tableTree.Append("<td style='display:none'>" + drv["Organization_ID"].ToString() + "</td>");
				this.str_tableTree.Append("</tr>");
				this.str_tableTree.Append(this.GetTableTreeNode(drv["Organization_ID"].ToString(), dtOrg, trID));
				eRowIndex++;
			}
		}
		public string GetTableTreeNode(string parentID, DataTable dtMenu, string parentTRID)
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
				sb_TreeNode.Append("<td style='padding-left:20px;'><span class=\"folder\">" + drv["Organization_Name"].ToString() + "</span></td>");
				sb_TreeNode.Append("<td style='width: 50px;text-align: center;'>" + drv["Organization_Code"].ToString() + "</td>");
				sb_TreeNode.Append("<td style='width: 100px;text-align: center;'>" + drv["Organization_Manager"].ToString() + "</td>");
				sb_TreeNode.Append("<td style='width: 100px;text-align: center;'>" + drv["Organization_InnerPhone"].ToString() + "</td>");
				sb_TreeNode.Append("<td style='width: 100px;text-align: center;'>" + drv["Organization_OuterPhone"].ToString() + "</td>");
				sb_TreeNode.Append("<td style='width: 100px;text-align: center;'>" + drv["Organization_Fax"].ToString() + "</td>");
				sb_TreeNode.Append("<td style='width: 50px;text-align: center;'>" + drv["Organization_Zipcode"].ToString() + "</td>");
				sb_TreeNode.Append("<td style='width: 50px;text-align: center;'>" + drv["SortCode"].ToString() + "</td>");
				sb_TreeNode.Append("<td>" + drv["Organization_Address"].ToString() + "</td>");
				sb_TreeNode.Append("<td style='display:none'>" + drv["Organization_ID"].ToString() + "</td>");
				sb_TreeNode.Append("</tr>");
				sb_TreeNode.Append(this.GetTableTreeNode(drv["Organization_ID"].ToString(), dtMenu, trID));
				i++;
			}
			return sb_TreeNode.ToString();
		}
	}
}
