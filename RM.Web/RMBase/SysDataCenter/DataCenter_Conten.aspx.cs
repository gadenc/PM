using RM.Busines.DAL;
using RM.Busines.IDAO;
using RM.Common.DotNetUI;
using RM.Web.App_Code;
using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace RM.Web.RMBase.SysDataCenter
{
    public class DataCenter_Conten : PageBase
    {
        protected HtmlForm form1;
        protected Repeater rp_Item;
        private RM_System_IDAO systemidao = new RM_System_Dal();
        public string _Table_Name;
        protected void Page_Load(object sender, EventArgs e)
        {
            this._Table_Name = base.Server.UrlDecode(base.Request["Table_Name"]);
            if (!base.IsPostBack)
            {
                if (this._Table_Name == null)
                {
                    this._Table_Name = "未选择";
                }
                this.GridBind();
            }
        }
        public void GridBind()
        {
            DataTable dt = this.systemidao.GetSyscolumns(this._Table_Name);
            ControlBindHelper.BindRepeaterList(dt, this.rp_Item);
        }
    }
}
