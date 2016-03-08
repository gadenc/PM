using RM.Busines;
using RM.Busines.DAL;
using RM.Busines.IDAO;
using RM.Common.DotNetData;
using RM.Web.App_Code;
using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Web.UI.HtmlControls;
namespace RM.Web.RMBase.SysPersonal
{
	public class Recyclebin_Info : PageBase
	{
		protected HtmlForm form1;
		public StringBuilder str_OutputHtml = new StringBuilder();
		private RM_System_IDAO systemidao = new RM_System_Dal();
		public string _Recyclebin_Name;
		public string _key;
		public string _ItemValue;
		protected void Page_Load(object sender, EventArgs e)
		{
			this._key = base.Request["key"];
			this.LoadInfoHtml();
		}
		public void LoadInfoHtml()
		{
			Hashtable ht = DataFactory.SqlDataBase().GetHashtableById("Base_Recyclebin", "Recyclebin_ID", this._key);
			if (ht.Count > 0 && ht != null)
			{
				this._Recyclebin_Name = ht["RECYCLEBIN_NAME"].ToString();
				DataTable dt = this.systemidao.GetRecyclebin_ObjField(ht["RECYCLEBIN_TABLE"].ToString());
				if (DataTableHelper.IsExistRows(dt))
				{
					int rowSum = dt.Rows.Count;
					for (int i = 0; i < rowSum; i++)
					{
						this.str_OutputHtml.Append("<tr>");
						this.str_OutputHtml.Append("<th>" + dt.Rows[i]["Field_Name"] + "</th>");
						this.str_OutputHtml.Append("<td>");
						this.str_OutputHtml.Append("<lable id=\"" + dt.Rows[i]["Field_Key"].ToString().ToUpper() + "\"/>");
						this.str_OutputHtml.Append("</td>");
						this.str_OutputHtml.Append("</tr>");
					}
				}
				Hashtable Obj_ht = DataFactory.SqlDataBase().GetHashtableById(ht["RECYCLEBIN_TABLE"].ToString(), ht["RECYCLEBIN_FIELDKEY"].ToString(), ht["RECYCLEBIN_EVENTFIELD"].ToString());
				foreach (string key in Obj_ht.Keys)
				{
					object itemValue = this._ItemValue;
					this._ItemValue = string.Concat(new object[]
					{
						itemValue,
						key,
						"∫",
						Obj_ht[key],
						"∮"
					});
				}
			}
		}
	}
}
