using RM.Common.DotNetUI;
using System;
using System.Web.UI;

namespace RM.Web.UserControl
{
    public partial class SubmitCheck : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public string GetToken()
        {
            return WebHelper.GetToken();
        }
    }
}