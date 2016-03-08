using RM.Busines;
using RM.Common.DotNetBean;
using RM.Common.DotNetCode;
using RM.Common.DotNetUI;
using RM.Web.App_Code;
using RM.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using RM.Web.UserControl;

namespace RM.Web.RMBase.SysPersonal
{
	public class Recyclebin_List : PageBase
	{
		private string _Recyclebin_Name;
		protected HtmlForm form1;
		protected HtmlInputText BeginBuilTime;
		protected HtmlInputText endBuilTime;
		protected LinkButton lbtSearch;
		protected LoadButton LoadButton1;
		protected Repeater rp_Item;
		protected PageControl PageControl1;
		protected void Page_Load(object sender, EventArgs e)
		{
			this._Recyclebin_Name = base.Server.UrlDecode(base.Request["Recyclebin_Name"]);
			this.PageControl1.pageHandler += new EventHandler(this.pager_PageChanged);
		}
		protected void pager_PageChanged(object sender, EventArgs e)
		{
			this.DataBindGrid();
		}
		private void DataBindGrid()
		{
			int count = 0;
			StringBuilder strSql = new StringBuilder("SELECT Recyclebin_ID,Recyclebin_EventField,Recyclebin_Name,CreateUserName,CreateDate,Recyclebin_Remark FROM Base_Recyclebin WHERE 1=1");
			IList<SqlParam> IList_param = new List<SqlParam>();
			if (this.BeginBuilTime.Value != "" || this.endBuilTime.Value != "")
			{
				strSql.Append(" and CreateDate >= @BeginBuilTime");
				strSql.Append(" and CreateDate <= @endBuilTime");
				IList_param.Add(new SqlParam("@BeginBuilTime", CommonHelper.GetDateTime(this.BeginBuilTime.Value)));
				IList_param.Add(new SqlParam("@endBuilTime", CommonHelper.GetDateTime(this.endBuilTime.Value).AddDays(1.0)));
			}
			strSql.Append(" and CreateUserId = @CreateUserId");
			IList_param.Add(new SqlParam("@CreateUserId", RequestSession.GetSessionUser().UserId));
			strSql.Append(" and Recyclebin_Name = @Recyclebin_Name");
			IList_param.Add(new SqlParam("@Recyclebin_Name", this._Recyclebin_Name));
			DataTable dt = DataFactory.SqlDataBase().GetPageList(strSql.ToString(), IList_param.ToArray<SqlParam>(), "CreateDate", "Desc", this.PageControl1.PageIndex, this.PageControl1.PageSize, ref count);
			ControlBindHelper.BindRepeaterList(dt, this.rp_Item);
			this.PageControl1.RecordCount = Convert.ToInt32(count);
		}
		protected void lbtSearch_Click(object sender, EventArgs e)
		{
			this.DataBindGrid();
		}
	}
}
