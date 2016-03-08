using RM.Busines.DAL;
using RM.Busines.IDAO;
using RM.Common.DotNetBean;
using RM.Common.DotNetUI;
using RM.Web.App_Code;
using RM.Web;
using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using RM.Web.UserControl;

namespace RM.Web.RMBase.SysPersonal
{
	public class HomeShortcut_List : PageBase
	{
		protected HtmlForm form1;
		protected LoadButton LoadButton1;
		protected Repeater rp_Item;
		private RM_System_IDAO sys_idao = new RM_System_Dal();
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!base.IsPostBack)
			{
				this.InitBindData();
			}
		}
		private void InitBindData()
		{
			DataTable dt = this.sys_idao.GetHomeShortcut_List(RequestSession.GetSessionUser().UserId.ToString());
			ControlBindHelper.BindRepeaterList(dt, this.rp_Item);
		}
	}
}
