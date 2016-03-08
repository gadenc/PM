using RM.Busines;
using RM.Common.DotNetBean;
using RM.Common.DotNetCode;
using RM.Common.DotNetUI;
using RM.Web.App_Code;
using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace RM.Web.RMBase.SysPersonal
{
	public class HomeShortcut_Form : PageBase
	{
		protected HtmlForm form1;
		protected HtmlInputText Setup_IName;
		protected HtmlSelect Target;
		protected HtmlInputHidden Setup_Img;
		protected HtmlImage Img_Setup_Img;
		protected HtmlInputText SortCode;
		protected HtmlTextArea NavigateUrl;
		protected HtmlTextArea Setup_Remak;
		protected LinkButton Save;
		private string _key;
		protected void Page_Load(object sender, EventArgs e)
		{
			this._key = base.Request["key"];
			if (!base.IsPostBack)
			{
				if (!string.IsNullOrEmpty(this._key))
				{
					this.InitData();
				}
			}
		}
		private void InitData()
		{
			Hashtable ht = DataFactory.SqlDataBase().GetHashtableById("Base_O_A_Setup", "Setup_ID", this._key);
			if (ht.Count > 0 && ht != null)
			{
				ControlBindHelper.SetWebControls(this.Page, ht);
				if (!string.IsNullOrEmpty(ht["SETUP_IMG"].ToString()))
				{
					this.Img_Setup_Img.Src = "/Themes/Images/32/" + ht["SETUP_IMG"].ToString();
				}
			}
		}
		protected void Save_Click(object sender, EventArgs e)
		{
			Hashtable ht = new Hashtable();
			ht = ControlBindHelper.GetWebControls(this.Page);
			if (string.IsNullOrEmpty(this._key))
			{
				ht["Setup_ID"] = CommonHelper.GetGuid;
				ht["User_ID"] = RequestSession.GetSessionUser().UserId;
			}
			bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit("Base_O_A_Setup", "Setup_ID", this._key, ht);
			if (IsOk)
			{
				ShowMsgHelper.AlertMsg("操作成功！");
			}
			else
			{
				ShowMsgHelper.Alert_Error("操作失败！");
			}
		}
	}
}
