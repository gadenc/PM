using RM.Busines.DAL;
using RM.Busines.IDAO;
using RM.Common.DotNetUI;
using RM.Web.App_Code;
using RM.Web;
using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using RM.Web.UserControl;

namespace RM.Web.RMBase.SysAppend
{
    public class AppendProperty_Function : PageBase
    {
        private RM_System_IDAO systemidao = new RM_System_Dal();
        protected HtmlForm form1;
        protected LoadButton LoadButton1;
        protected Repeater rp_Item;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                this.InitData();
            }
        }
        private void InitData()
        {
            DataTable dt = this.systemidao.AppendProperty_Function();
            ControlBindHelper.BindRepeaterList(dt, this.rp_Item);
        }
    }
}
