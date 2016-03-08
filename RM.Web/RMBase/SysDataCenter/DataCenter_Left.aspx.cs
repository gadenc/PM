using RM.Busines.DAL;
using RM.Busines.IDAO;
using RM.Web.App_Code;
using System;
using System.Data;
using System.Text;
using System.Web.UI.HtmlControls;

namespace RM.Web.RMBase.SysDataCenter
{
    public class DataCenter_Left : PageBase
    {
        protected HtmlForm form1;
        public StringBuilder treeItem_Table = new StringBuilder();
        private RM_System_IDAO systemidao = new RM_System_Dal();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                this.GetTreeTable();
            }
        }
        public void GetTreeTable()
        {
            DataTable dt = this.systemidao.GetSysobjects();
            foreach (DataRow drv in dt.Rows)
            {
                this.treeItem_Table.Append("<li>");
                this.treeItem_Table.Append(string.Concat(new object[]
				{
					"<div onclick=\"GetTable('",
					drv["TABLE_NAME"],
					"')\"><img src=\"/Themes/Images/20130502112716785_easyicon_net_16.png\" width=\"16\" height=\"16\" />",
					drv["TABLE_NAME"],
					"</div>"
				}));
                this.treeItem_Table.Append("</li>");
            }
        }
    }
}
