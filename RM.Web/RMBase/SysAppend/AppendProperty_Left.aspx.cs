using RM.Busines.DAL;
using RM.Busines.IDAO;
using RM.Common.DotNetData;
using RM.Web.App_Code;
using System;
using System.Data;
using System.Text;
using System.Web.UI.HtmlControls;
namespace RM.Web.RMBase.SysAppend
{
    public class AppendProperty_Left : PageBase
    {
        protected HtmlForm form1;
        public StringBuilder strHtml = new StringBuilder();
        private RM_System_IDAO systemidao = new RM_System_Dal();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                this.InitInfo();
            }
        }
        public void InitInfo()
        {
            DataTable dt = this.systemidao.AppendProperty_Function();
            if (DataTableHelper.IsExistRows(dt))
            {
                DataView dv = new DataView(dt);
                foreach (DataRowView drv in dv)
                {
                    this.strHtml.Append("<li>");
                    this.strHtml.Append(string.Concat(new string[]
					{
						"<div onclick=\"Property_Function('",
						drv["Property_Function"].ToString(),
						"')\">",
						drv["Property_Function"].ToString(),
						"</div>"
					}));
                    this.strHtml.Append("</li>");
                }
            }
            else
            {
                this.strHtml.Append("<li>");
                this.strHtml.Append("<div><span style='color:red;'>暂无数据</span></div>");
                this.strHtml.Append("</li>");
            }
        }
    }
}
