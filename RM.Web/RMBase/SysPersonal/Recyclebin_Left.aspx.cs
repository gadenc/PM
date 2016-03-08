using RM.Busines;
using RM.Common.DotNetBean;
using RM.Common.DotNetCode;
using RM.Web.App_Code;
using System;
using System.Data;
using System.Text;
using System.Web.UI.HtmlControls;
namespace RM.Web.RMBase.SysPersonal
{
	public class Recyclebin_Left : PageBase
	{
		protected HtmlForm form1;
		public StringBuilder strHtml = new StringBuilder();
		protected void Page_Load(object sender, EventArgs e)
		{
			this.GetTreeNode();
		}
		public void GetTreeNode()
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append("SELECT DISTINCT(Recyclebin_Name) FROM Base_Recyclebin where CreateUserId = @CreateUserId");
			SqlParam[] para = new SqlParam[]
			{
				new SqlParam("@CreateUserId", RequestSession.GetSessionUser().UserId)
			};
			DataTable Recyclebin_dt = DataFactory.SqlDataBase().GetDataTableBySQL(strSql, para);
			DataView dv = new DataView(Recyclebin_dt);
			if (dv.Count > 0)
			{
				this.strHtml.Append("<ul>");
				foreach (DataRowView drv in dv)
				{
					this.strHtml.Append("<li>");
					this.strHtml.Append(string.Concat(new object[]
					{
						"<div onclick=\"GetRecyclebin_Name('",
						drv["Recyclebin_Name"].ToString(),
						"')\">",
						drv["Recyclebin_Name"],
						"</div>"
					}));
					this.strHtml.Append("</li>");
				}
				this.strHtml.Append("</ul>");
			}
			else
			{
				this.strHtml.Append("<ul>");
				this.strHtml.Append("<li>");
				this.strHtml.Append("<div><span style='color:red;'>暂无数据</span></div>");
				this.strHtml.Append("</li>");
				this.strHtml.Append("</ul>");
			}
		}
	}
}
