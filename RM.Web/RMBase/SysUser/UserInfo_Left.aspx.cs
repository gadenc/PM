using RM.Busines.DAL;
using RM.Busines.IDAO;
using RM.Common.DotNetData;
using RM.Web.App_Code;
using System;
using System.Data;
using System.Text;
using System.Web.UI.HtmlControls;
namespace RM.Web.RMBase.SysUser
{
	public class UserInfo_Left : PageBase
	{
		public StringBuilder strHtml = new StringBuilder();
		private RM_UserInfo_IDAO user_idao = new RM_UserInfo_Dal();
		protected HtmlForm form1;
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!base.IsPostBack)
			{
				this.InitInfo();
			}
		}
		public void InitInfo()
		{
			DataTable dtOrg = this.user_idao.GetOrganizeList();
			if (DataTableHelper.IsExistRows(dtOrg))
			{
				foreach (DataRowView drv in new DataView(dtOrg)
				{
					RowFilter = "ParentId = '0'"
				})
				{
					this.strHtml.Append("<li>");
					this.strHtml.Append("<div>" + drv["Organization_Name"].ToString());
					this.strHtml.Append("<span style='display:none'>" + drv["Organization_ID"].ToString() + "</span></div>");
					this.strHtml.Append(this.GetTreeNode(drv["Organization_ID"].ToString(), dtOrg));
					this.strHtml.Append("</li>");
				}
			}
			else
			{
				this.strHtml.Append("<li>");
				this.strHtml.Append("<div><span style='color:red;'>暂无数据</span></div>");
				this.strHtml.Append("</li>");
			}
		}
		public string GetTreeNode(string parentID, DataTable dtNode)
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
					sb_TreeNode.Append("<div>" + drv["Organization_Name"]);
					sb_TreeNode.Append("<span style='display:none'>" + drv["Organization_ID"].ToString() + "</span></div>");
					sb_TreeNode.Append(this.GetTreeNode(drv["Organization_ID"].ToString(), dtNode));
					sb_TreeNode.Append("</li>");
				}
				sb_TreeNode.Append("</ul>");
			}
			return sb_TreeNode.ToString();
		}
	}
}
