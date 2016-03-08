using RM.Busines;
using RM.Common.DotNetBean;
using RM.Common.DotNetEncrypt;
using RM.Web.App_Code;
using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace RM.Web.RMBase.SysUser
{
	public class UpdateUserPwd : PageBase
	{
		protected HtmlForm form1;
		protected HtmlInputText txtUserName;
		protected HtmlInputPassword txtPastUserPwd;
		protected HtmlInputPassword txtUserPwd;
		protected HtmlInputPassword User_NewPwd;
		protected HtmlInputText txtCode;
		protected HtmlGenericControl errorMsg;
		protected LinkButton Save;
		public string _PasPwd;
		protected void Page_Load(object sender, EventArgs e)
		{
			this.txtUserName.Value = RequestSession.GetSessionUser().UserAccount.ToString();
			this._PasPwd = RequestSession.GetSessionUser().UserPwd.ToString();
		}
		protected void Save_Click(object sender, EventArgs e)
		{
			string s = this.Session["dt_session_code"].ToString().ToLower();
			if (this.txtCode.Value.ToLower() != this.Session["dt_session_code"].ToString().ToLower())
			{
				this.txtCode.Focus();
				this.errorMsg.InnerHtml = "验证码输入不正确！";
			}
			else
			{
				int i = 0;
				if (this.txtUserName.Value != "Administrator")
				{
					Hashtable ht_User = new Hashtable();
					ht_User["User_Pwd"] = Md5Helper.MD5(this.txtUserPwd.Value, 32);
					i = DataFactory.SqlDataBase().UpdateByHashtable("Base_UserInfo", "User_ID", RequestSession.GetSessionUser().UserId.ToString(), ht_User);
				}
				if (i > 0)
				{
					this.Session.Abandon();
					this.Session.Clear();
					base.Response.Write("<script>alert('登陆修改成功,请重新登陆');top.location.href='/Index.htm'</script>");
				}
				else
				{
					this.errorMsg.InnerHtml = "修改登录密码失败";
				}
			}
		}
	}
}
