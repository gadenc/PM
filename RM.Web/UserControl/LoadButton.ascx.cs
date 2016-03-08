using RM.Busines.DAL;
using RM.Busines.IDAO;
using RM.Common.DotNetBean;
using RM.Common.DotNetData;
using System;
using System.Data;
using System.Text;
using System.Web.UI;

namespace RM.Web.UserControl
{
    public class LoadButton : System.Web.UI.UserControl
    {
        public StringBuilder sb_Button = new StringBuilder();
        private RM_System_IDAO systemidao = new RM_System_Dal();
        protected void Page_Load(object sender, EventArgs e)
        {
            string UserId = RequestSession.GetSessionUser().UserId.ToString();
            DataTable dt_Button = this.systemidao.GetButtonHtml(UserId);
            if (DataTableHelper.IsExistRows(dt_Button))
            {
                foreach (DataRow dr in dt_Button.Rows)
                {
                    this.sb_Button.Append(string.Concat(new string[]
					{
						"<a title=\"",
						dr["Menu_Title"].ToString(),
						"\" onclick=\"",
						dr["NavigateUrl"].ToString(),
						";\" class=\"button green\">"
					}));
                    this.sb_Button.Append("<span class=\"icon-botton\" style=\"background: url('/Themes/images/16/" + dr["Menu_Img"].ToString() + "') no-repeat scroll 0px 4px;\"></span>");
                    this.sb_Button.Append(dr["Menu_Name"].ToString());
                    this.sb_Button.Append("</a>");
                }
            }
            else
            {
                this.sb_Button.Append("<a class=\"button green\">");
                this.sb_Button.Append("无操作按钮");
                this.sb_Button.Append("</a>");
            }
        }
    }
}