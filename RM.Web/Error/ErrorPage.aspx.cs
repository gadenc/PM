using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace RM.Web.Error
{
    public partial class ErrorPage : System.Web.UI.Page
    {
        protected HtmlForm form1;
        protected Label Label1;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Label1.Text = base.Application["error"].ToString();
        }
    }
}