using RM.Busines;
using RM.Common.DotNetBean;
using RM.Common.DotNetCode;
using RM.Common.DotNetData;
using RM.Common.DotNetUI;
using RM.Web.App_Code;
using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace RM.Web.RMBase.SysMenu
{
	public class Menu_Form : PageBase
	{
		private string _key;
		private string _ParentId;
		protected HtmlForm form1;
		protected HtmlInputText Menu_Name;
		protected HtmlInputText Menu_Title;
		protected HtmlSelect ParentId;
		protected HtmlInputHidden Menu_Img;
		protected HtmlImage Img_Menu_Img;
		protected HtmlSelect Target;
		protected HtmlInputText SortCode;
		protected HtmlTextArea NavigateUrl;
		protected LinkButton Save;
		protected void Page_Load(object sender, EventArgs e)
		{
			this._key = base.Request["key"];
			this._ParentId = base.Request["ParentId"];
			if (!base.IsPostBack)
			{
				this.InitParentId();
				if (!string.IsNullOrEmpty(this._ParentId))
				{
					this.ParentId.Value = this._ParentId;
				}
				if (!string.IsNullOrEmpty(this._key))
				{
					this.InitData();
				}
			}
		}
		private void InitData()
		{
			Hashtable ht = DataFactory.SqlDataBase().GetHashtableById("Base_SysMenu", "Menu_Id", this._key);
			if (ht.Count > 0 && ht != null)
			{
				ControlBindHelper.SetWebControls(this.Page, ht);
				if (!string.IsNullOrEmpty(ht["MENU_IMG"].ToString()))
				{
					this.Img_Menu_Img.Src = "/Themes/Images/32/" + ht["MENU_IMG"].ToString();
				}
			}
		}
		private void InitParentId()
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append("SELECT Menu_Id,\r\n                            Menu_Name+' - '+CASE Menu_Type WHEN '1' THEN '父节' WHEN '2' THEN '子节' END AS Menu_Name\r\n                            FROM Base_SysMenu WHERE DeleteMark = 1 AND Menu_Type != 3 ORDER BY SortCode ASC");
			DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(strSql);
			if (!string.IsNullOrEmpty(this._key))
			{
				if (DataTableHelper.IsExistRows(dt))
				{
					for (int i = 0; i < dt.Rows.Count; i++)
					{
						if (dt.Rows[i]["Menu_Id"].ToString() == this._key)
						{
							dt.Rows.RemoveAt(i);
						}
					}
				}
			}
			ControlBindHelper.BindHtmlSelect(dt, this.ParentId, "Menu_Name", "Menu_Id", "模块菜单 - 父节");
		}
		protected void Save_Click(object sender, EventArgs e)
		{
			Hashtable ht = new Hashtable();
			ht = ControlBindHelper.GetWebControls(this.Page);
			if (this.ParentId.Value == "")
			{
				ht["ParentId"] = "0";
				ht["Menu_Type"] = 1;
			}
			else
			{
				ht["Menu_Type"] = 2;
			}
			if (!string.IsNullOrEmpty(this._key))
			{
				ht["ModifyDate"] = DateTime.Now;
				ht["ModifyUserId"] = RequestSession.GetSessionUser().UserId;
				ht["ModifyUserName"] = RequestSession.GetSessionUser().UserName;
			}
			else
			{
				ht["Menu_Id"] = CommonHelper.GetGuid;
				ht["CreateUserId"] = RequestSession.GetSessionUser().UserId;
				ht["CreateUserName"] = RequestSession.GetSessionUser().UserName;
			}
			bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit("Base_SysMenu", "Menu_Id", this._key, ht);
			if (IsOk)
			{
				CacheHelper.RemoveAllCache();
				ShowMsgHelper.AlertMsg("操作成功！");
			}
			else
			{
				ShowMsgHelper.Alert_Error("操作失败！");
			}
		}
	}
}
