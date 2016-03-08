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

namespace RM.Web.RMBase.SysLog
{
    public class LoginList : PageBase
    {
        private RM_UserInfo_IDAO user_idao = new RM_UserInfo_Dal();
        protected HtmlForm form1;
        protected HtmlInputText txt_Search;
        protected HtmlInputText BeginBuilTime;
        protected HtmlInputText endBuilTime;
        protected LinkButton lbtSearch;
        protected Repeater rp_Item;
        protected PageControl PageControl1;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.PageControl1.pageHandler += new EventHandler(this.pager_PageChanged);
        }
        private void pager_PageChanged(object sender, EventArgs e)
        {
            this.DataBindGrid();
        }
        private void DataBindGrid()
        {
            int count = 0;
            StringBuilder SqlWhere = new StringBuilder();
            IList<SqlParam> IList_param = new List<SqlParam>();
            if (this.BeginBuilTime.Value != "" || this.endBuilTime.Value != "")
            {
                SqlWhere.Append(" and Sys_LoginLog_Time >= @BeginBuilTime");
                SqlWhere.Append(" and Sys_LoginLog_Time <= @endBuilTime");
                IList_param.Add(new SqlParam("@BeginBuilTime", CommonHelper.GetDateTime(this.BeginBuilTime.Value)));
                IList_param.Add(new SqlParam("@endBuilTime", CommonHelper.GetDateTime(this.endBuilTime.Value).AddDays(1.0)));
            }
            if (this.txt_Search.Value != "")
            {
                SqlWhere.Append(" and SYS_USER = @SYS_USER");
                IList_param.Add(new SqlParam("@SYS_USER", this.txt_Search.Value));
            }
            DataTable dt = this.user_idao.GetSysLoginLogPage(SqlWhere, IList_param, this.PageControl1.PageIndex, this.PageControl1.PageSize, ref count);
            ControlBindHelper.BindRepeaterList(dt, this.rp_Item);
            this.PageControl1.RecordCount = Convert.ToInt32(count);
        }
        protected void rp_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lbl_Sys_LoginLog = e.Item.FindControl("lbl_Sys_LoginLog_Status") as Label;
                if (lbl_Sys_LoginLog != null)
                {
                    string text = lbl_Sys_LoginLog.Text;
                    text = text.Replace("1", "<span style='color:Blue'>成功登陆</span>");
                    text = text.Replace("0", "<span style='color:red'>登陆失败</span>");
                    lbl_Sys_LoginLog.Text = text;
                }
            }
        }
        protected void lbtSearch_Click(object sender, EventArgs e)
        {
            this.DataBindGrid();
        }
    }
}
