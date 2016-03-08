using RM.Common.DotNetBean;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace RM.Web.RMBase.SysDataCenter
{
    public class Backup_Confirm : Page
    {
        public string _UserPwd;
        protected HtmlForm form1;
        protected HtmlInputPassword txtUserPwd;
        protected HtmlTextArea Remak;
        protected void Page_Load(object sender, EventArgs e)
        {
            this._UserPwd = RequestSession.GetSessionUser().UserPwd.ToString();
        }
    }
}
