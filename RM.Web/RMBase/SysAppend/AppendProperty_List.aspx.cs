using RM.Busines.DAL;
using RM.Busines.IDAO;
using RM.Common.DotNetUI;
using RM.Web.App_Code;
using RM.Web;
using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using RM.Web.UserControl;

namespace RM.Web.RMBase.SysAppend
{
    public class AppendProperty_List : PageBase
    {
        private RM_System_IDAO systemidao = new RM_System_Dal();
        public string _Function;
        protected HtmlForm form1;
        protected LoadButton LoadButton1;
        protected Repeater rp_Item;
        protected void Page_Load(object sender, EventArgs e)
        {
            this._Function = base.Server.UrlDecode(base.Request["Function"]);
            if (!base.IsPostBack)
            {
                this.InitData();
                if (string.IsNullOrEmpty(this._Function))
                {
                    this._Function = "未选择";
                }
            }
        }
        private void InitData()
        {
            DataTable dt = new DataTable();
            if (!string.IsNullOrEmpty(this._Function))
            {
                dt = this.systemidao.AppendProperty_List(this._Function);
            }
            ControlBindHelper.BindRepeaterList(dt, this.rp_Item);
        }
        protected void rp_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label title = e.Item.FindControl("lblProperty_Control_Type") as Label;
                if (title != null)
                {
                    string text = title.Text;
                    text = text.Replace("1", "文本框");
                    text = text.Replace("2", "下拉框");
                    text = text.Replace("3", "日期框");
                    text = text.Replace("4", "标  签");
                    title.Text = text;
                }
            }
        }
    }
}
