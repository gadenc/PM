using RM.Common.DotNetBean;
using RM.Common.DotNetUI;
using RM.Web.App_Code;
using RM.Web;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using RM.Web.UserControl;

namespace RM.Web.RMBase.SysPersonal
{
	public class Individuation_Set : PageBase
	{
		protected HtmlForm form1;
		protected HtmlSelect Language_Type;
		protected HtmlSelect WebUI_Type;
		protected HtmlSelect Menu_Type;
		protected HtmlSelect PageIndex;
		protected LinkButton Save;
		protected SubmitCheck SubmitCheck1;
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!base.IsPostBack)
			{
				this.Language_Type.Value = CookieHelper.GetCookie("Language_Type");
				this.WebUI_Type.Value = CookieHelper.GetCookie("WebUI_Type");
				this.Menu_Type.Value = CookieHelper.GetCookie("Menu_Type");
				this.PageIndex.Value = CookieHelper.GetCookie("PageIndex");
			}
		}
		protected void Save_Click(object sender, EventArgs e)
		{
			try
			{
				if (WebHelper.SubmitCheckForm())
				{
					CookieHelper.WriteCookie("Language_Type", this.Language_Type.Value, 30);
					CookieHelper.WriteCookie("WebUI_Type", this.WebUI_Type.Value, 30);
					CookieHelper.WriteCookie("Menu_Type", this.Menu_Type.Value, 30);
					CookieHelper.WriteCookie("PageIndex", this.PageIndex.Value, 30);
					ShowMsgHelper.ShowScript("MainSwitch()");
				}
			}
			catch
			{
				ShowMsgHelper.Alert_Error("设置失败！");
			}
		}
	}
}
