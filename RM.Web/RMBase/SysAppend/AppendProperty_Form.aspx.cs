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
    public class AppendProperty_Form : PageBase
    {
        protected HtmlForm form1;
        protected HtmlInputText Property_Name;
        protected HtmlInputText Property_Control_ID;
        protected HtmlSelect Property_Control_Type;
        protected HtmlInputText Property_Control_Length;
        protected HtmlSelect Property_Control_Style;
        protected HtmlSelect Property_Control_Validator;
        protected HtmlInputText Property_Control_Maxlength;
        protected HtmlInputText Property_Colspan;
        protected HtmlInputText SortCode;
        protected HtmlTextArea Property_Event;
        protected HtmlTextArea Property_Control_DataSource;
        protected LinkButton Save;
        private string _Function;
        private string _key;
        protected void Page_Load(object sender, EventArgs e)
        {
            this._Function = base.Server.UrlDecode(base.Request["Function"]);
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
            Hashtable ht = DataFactory.SqlDataBase().GetHashtableById("Base_AppendProperty", "Property_ID", this._key);
            if (ht.Count > 0 && ht != null)
            {
                ControlBindHelper.SetWebControls(this.Page, ht);
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
                ht["Property_Function"] = this._Function;
                ht["Property_ID"] = CommonHelper.GetGuid;
                ht["CreateUserId"] = RequestSession.GetSessionUser().UserId;
                ht["CreateUserName"] = RequestSession.GetSessionUser().UserName;
            }
            bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit("Base_AppendProperty", "Property_ID", this._key, ht);
            if (IsOk)
            {
                ShowMsgHelper.ParmAlertMsg("操作成功！");
            }
            else
            {
                ShowMsgHelper.Alert_Error("操作失败！");
            }
        }
    }
}
