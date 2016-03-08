using RM.Busines;
using RM.Common.DotNetBean;
using RM.Common.DotNetCode;
using RM.Common.DotNetUI;
using RM.Web.App_Code;
using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace RM.Web.RMBase.SysAppend
{
    public class AppendProperty_Function_Form : PageBase
    {
        private string _key;
        protected HtmlForm form1;
        protected HtmlInputText Property_Function;
        protected HtmlInputText Property_FunctionUrl;
        protected LinkButton Save;
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
            Hashtable ht = DataFactory.SqlDataBase

().GetHashtableById("Base_AppendProperty", "Property_ID", this._key);
            if (ht.Count > 0 && ht != null)
            {
                ControlBindHelper.SetWebControls(this.Page,

ht);
            }
        }
        protected void Save_Click(object sender, EventArgs e)
        {
            Hashtable ht = new Hashtable();
            ht = ControlBindHelper.GetWebControls(this.Page);
            ht["Property_Control_ID"] = 0;
            if (!string.IsNullOrEmpty(this._key))
            {
                ht["ModifyDate"] = DateTime.Now;
                ht["ModifyUserId"] =

RequestSession.GetSessionUser().UserId;
                ht["ModifyUserName"] =

RequestSession.GetSessionUser().UserName;
            }
            else
            {
                ht["Property_ID"] = CommonHelper.GetGuid;
                ht["CreateUserId"] =

RequestSession.GetSessionUser().UserId;
                ht["CreateUserName"] =

RequestSession.GetSessionUser().UserName;
            }
            bool IsOk = DataFactory.SqlDataBase

().Submit_AddOrEdit("Base_AppendProperty", "Property_ID", this._key, ht);
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
