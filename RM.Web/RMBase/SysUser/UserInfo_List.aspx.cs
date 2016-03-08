using RM.Busines.DAL;
using RM.Busines.IDAO;
using RM.Common.DotNetCode;
using RM.Common.DotNetUI;
using RM.Web.App_Code;
using RM.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using RM.Web.UserControl;

namespace RM.Web.RMBase.SysUser
{
	public class UserInfo_List : PageBase
	{
		protected HtmlForm form1;
		protected HtmlSelect Searchwhere;
		protected HtmlInputText txt_Search;
		protected LinkButton lbtSearch;
		protected LoadButton LoadButton1;
		protected Repeater rp_Item;
		protected PageControl PageControl1;
		private RM_UserInfo_IDAO user_idao = new RM_UserInfo_Dal();
		private string _Organization_ID;
		protected void Page_Load(object sender, EventArgs e)
		{
			this._Organization_ID = base.Request["Organization_ID"];
			this.PageControl1.pageHandler += new EventHandler(this.pager_PageChanged);
			if (!base.IsPostBack)
			{
			}
		}
		protected void pager_PageChanged(object sender, EventArgs e)
		{
			this.DataBindGrid();
		}
		private void DataBindGrid()
		{
			int count = 0;
			StringBuilder SqlWhere = new StringBuilder();
			IList<SqlParam> IList_param = new List<SqlParam>();
			if (!string.IsNullOrEmpty(this.txt_Search.Value))
			{
				SqlWhere.Append(" and U." + this.Searchwhere.Value + " like @obj ");
				IList_param.Add(new SqlParam("@obj", '%' + this.txt_Search.Value.Trim() + '%'));
			}
			if (!string.IsNullOrEmpty(this._Organization_ID))
			{
				SqlWhere.Append(" AND S.Organization_ID IN(" + this._Organization_ID + ")");
			}
			DataTable dt = this.user_idao.GetUserInfoPage(SqlWhere, IList_param, this.PageControl1.PageIndex, this.PageControl1.PageSize, ref count);
			ControlBindHelper.BindRepeaterList(dt, this.rp_Item);
			this.PageControl1.RecordCount = Convert.ToInt32(count);
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
					textDeleteMark = textDeleteMark.Replace("1", "<span style='color:Blue'>启用</span>");
					textDeleteMark = textDeleteMark.Replace("2", "<span style='color:red'>停用</span>");
					lblDeleteMark.Text = textDeleteMark;
				}
			}
		}
		protected void lbtSearch_Click(object sender, EventArgs e)
		{
			this.DataBindGrid();
		}
	}
}
