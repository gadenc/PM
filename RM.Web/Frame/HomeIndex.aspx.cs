using RM.Busines.DAL;
using RM.Busines.IDAO;
using RM.Common.DotNetBean;
using RM.Common.DotNetData;
using RM.Common.DotNetUI;
using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web;
using RM.Web.App_Code;

namespace RM.Web.Frame
{
    public partial class HomeIndex : PageBase
    {
        public StringBuilder sbHomeShortcouHtml = new StringBuilder();
        public StringBuilder Login_InfoHtml = new StringBuilder();
        public string _UserName;
        private RM_System_IDAO sys_idao = new RM_System_Dal();
        private RM_UserInfo_IDAO user_idao = new RM_UserInfo_Dal();
        protected HtmlForm form1;
        protected void Page_Load(object sender, EventArgs e)
        {
            ShowMsgHelper.ShowScript("top.TopLoading('注意：为了确保体验站点的完整性，您的部分操作将被限制执行!', 8000);");
            this._UserName = RequestSession.GetSessionUser().UserName.ToString();
            this.InitShortcouData();
            this.BindLogin_Info();
        }
        private void InitShortcouData()
        {
            DataTable dt = this.sys_idao.GetHomeShortcut_List(RequestSession.GetSessionUser().UserId.ToString());
            if (DataTableHelper.IsExistRows(dt))
            {
                int rowSum = dt.Rows.Count;
                if (rowSum > 0)
                {
                    for (int i = 0; i < rowSum; i++)
                    {
                        this.sbHomeShortcouHtml.Append(string.Concat(new object[]
						{
							"<div onclick=\"ClickShortcut('",
							dt.Rows[i]["NavigateUrl"],
							"','",
							dt.Rows[i]["Setup_IName"],
							"','",
							dt.Rows[i]["Target"],
							"')\" class=\"shortcuticons\">"
						}));
                        this.sbHomeShortcouHtml.Append("<img src=\"/Themes/Images/32/" + dt.Rows[i]["Setup_Img"] + "\" alt=\"\" /><br />");
                        this.sbHomeShortcouHtml.Append(dt.Rows[i]["Setup_IName"] + "</div>");
                    }
                }
            }
        }
        public void BindLogin_Info()
        {
            int count = 0;
            DataTable dt = this.user_idao.GetLogin_Info(ref count);
            this.Login_InfoHtml.Append("本月登录总数：" + count + " 次 <br />");
            this.Login_InfoHtml.Append("本次登录IP：" + dt.Rows[0]["SYS_LOGINLOG_IP"].ToString() + "<br />");
            this.Login_InfoHtml.Append("本次登录时间：" + dt.Rows[0]["SYS_LOGINLOG_TIME"].ToString() + "<br />");
            if (dt.Rows.Count != 1)
            {
                this.Login_InfoHtml.Append("上次登录IP：" + dt.Rows[1]["SYS_LOGINLOG_IP"].ToString() + "<br />");
                this.Login_InfoHtml.Append("上次登录时间：" + dt.Rows[1]["SYS_LOGINLOG_TIME"].ToString() + "<br />");
            }
            else
            {
                this.Login_InfoHtml.Append("上次登录IP：127.0.0.1 <br />");
                this.Login_InfoHtml.Append("上次登录时间：1900-01-01 00:00:00<br />");
            }
        }
    }
}
