using RM.Busines;
using RM.Common.DotNetBean;
using RM.Common.DotNetCode;
using RM.Common.DotNetUI;
using RM.Web.App_Code;
using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace RM.Web.RMBase.SysMenu
{
	public class Button_Form : PageBase
	{
		protected HtmlForm form1;
		protected HtmlInputText Button_Name;
		protected HtmlInputText Button_Title;
		protected HtmlSelect Button_Type;
		protected HtmlInputHidden Button_Img;
		protected HtmlImage Img_Button_Img;
		protected HtmlInputText SortCode;
		protected HtmlTextArea Button_Code;
		protected HtmlTextArea Button_Remak;
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
			Hashtable ht = DataFactory.SqlDataBase().GetHashtableById("Base_Button", "Button_ID", this._key);
			if (ht.Count > 0 && ht != null)
			{
				ControlBindHelper.SetWebControls(this.Page, ht);
				if (!string.IsNullOrEmpty(ht["BUTTON_IMG"].ToString()))
				{
					this.Img_Button_Img.Src = "/Themes/Images/16/" + ht["BUTTON_IMG"].ToString();
				}
			}
		}
		protected void Save_Click(object sender, EventArgs e)
		{
			Hashtable ht = new Hashtable();
			ht = ControlBindHelper.GetWebControls(this.Page);
			if (!string.IsNullOrEmpty(this._key))
			{
				ht["ModifyDate"] = DateTime.Now;
				ht["ModifyUserId"] = RequestSession.GetSessionUser().UserId;
				ht["ModifyUserName"] = RequestSession.GetSessionUser().UserName;
			}
			else
			{
				ht["Button_ID"] = CommonHelper.GetGuid;
				ht["CreateUserId"] = RequestSession.GetSessionUser().UserId;
				ht["CreateUserName"] = RequestSession.GetSessionUser().UserName;
			}
			bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit("Base_Button", "Button_ID", this._key, ht);
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
