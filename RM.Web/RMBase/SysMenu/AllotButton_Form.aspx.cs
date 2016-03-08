using RM.Busines.DAL;
using RM.Busines.IDAO;
using RM.Common.DotNetData;
using RM.Web.App_Code;
using System;
using System.Data;
using System.Text;
using System.Web.UI.HtmlControls;
namespace RM.Web.RMBase.SysMenu
{
	public class AllotButton_Form : PageBase
	{
		protected HtmlForm form1;
		protected HtmlInputHidden Button_ID_Hidden;
		private RM_System_IDAO systemidao = new RM_System_Dal();
		public StringBuilder ButtonList = new StringBuilder();
		public StringBuilder selectedButtonList = new StringBuilder();
		public string _ParentId;
		protected void Page_Load(object sender, EventArgs e)
		{
			this._ParentId = base.Request["key"];
			this.InitButtonList();
		}
		public void InitButtonList()
		{
			DataTable dt = this.systemidao.GetButtonList();
			if (DataTableHelper.IsExistRows(dt))
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					this.ButtonList.Append(string.Concat(new string[]
					{
						"<div id=",
						dt.Rows[i]["Button_ID"].ToString(),
						" onclick='selectedButton(this)' title='",
						dt.Rows[i]["Button_Name"].ToString(),
						"' class=\"shortcuticons\"><img src=\"/Themes/Images/16/",
						dt.Rows[i]["Button_Img"].ToString(),
						"\" alt=\"\" /><br />",
						dt.Rows[i]["Button_Name"].ToString(),
						"</div>"
					}));
				}
			}
			DataTable dt_selectedButton = this.systemidao.GetSysMenuByButton(this._ParentId);
			if (DataTableHelper.IsExistRows(dt_selectedButton))
			{
				for (int i = 0; i < dt_selectedButton.Rows.Count; i++)
				{
					this.selectedButtonList.Append(string.Concat(new object[]
					{
						"<div onclick='selectedButton(this)' ondblclick=\"removeButton('",
						dt_selectedButton.Rows[i]["Menu_Id"],
						"')\" title='",
						dt_selectedButton.Rows[i]["Menu_Title"].ToString(),
						"' class=\"shortcuticons\"><img src=\"/Themes/Images/16/",
						dt_selectedButton.Rows[i]["Menu_Img"].ToString(),
						"\" alt=\"\" /><br />",
						dt_selectedButton.Rows[i]["Menu_Name"].ToString(),
						"</div>"
					}));
				}
			}
		}
	}
}
