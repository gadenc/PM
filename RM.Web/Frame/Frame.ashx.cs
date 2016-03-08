using RM.Busines.DAL;
using RM.Busines.IDAO;
using RM.Common.DotNetBean;
using RM.Common.DotNetCode;
using RM.Common.DotNetJson;
using RM.Common.DotNetUI;
using System;
using System.Data;
using System.Web;
using System.Web.SessionState;

namespace RM.Web.Frame
{
    /// <summary>
    /// Frame 的摘要说明
    /// </summary>
    public class Frame : IHttpHandler, IRequiresSessionState
    {
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Buffer = true;
            context.Response.ExpiresAbsolute = DateTime.Now.AddDays(-1.0);
            context.Response.AddHeader("pragma", "no-cache");
            context.Response.AddHeader("cache-control", "");
            context.Response.CacheControl = "no-cache";
            string Action = context.Request["action"];
            string user_Account = context.Request["user_Account"];
            string userPwd = context.Request["userPwd"];
            string code = context.Request["code"];
            RM_UserInfo_IDAO user_idao = new RM_UserInfo_Dal();
            RM_System_IDAO sys_idao = new RM_System_Dal();
            IPScanerHelper objScan = new IPScanerHelper();
            string text = Action;
            if (text != null)
            {
                if (text == "Menu")
                {
                    string UserId = RequestSession.GetSessionUser().UserId.ToString();
                    string strMenus = JsonHelper.DataTableToJson(sys_idao.GetMenuHtml(UserId), "MENU");
                    context.Response.Write(strMenus);
                    context.Response.End();
                }
                else if (text == "login")
                {
                    if (code.ToLower() != context.Session["dt_session_code"].ToString().ToLower())
                    {
                        context.Response.Write("1");
                        context.Response.End();
                    }
                    DataTable dtlogin = user_idao.UserLogin(user_Account.Trim(), userPwd.Trim());
                    if (dtlogin != null)
                    {
                        objScan.DataPath = context.Server.MapPath("/Themes/IPScaner/QQWry.Dat");
                        objScan.IP = RequestHelper.GetIP();
                        string OWNER_address = objScan.IPLocation();
                        if (dtlogin.Rows.Count != 0)
                        {
                            user_idao.SysLoginLog(user_Account, "1", OWNER_address);
                            if (dtlogin.Rows[0]["DeleteMark"].ToString() == "1")
                            {
                                if (this.Islogin(context, user_Account))
                                {
                                    RequestSession.AddSessionUser(new SessionUser
                                    {
                                        UserId = dtlogin.Rows[0]["User_ID"].ToString(),
                                        UserAccount = dtlogin.Rows[0]["User_Account"].ToString(),
                                        UserName = dtlogin.Rows[0]["User_Name"].ToString() + "(" + dtlogin.Rows[0]["User_Account"].ToString() + ")",
                                        UserPwd = dtlogin.Rows[0]["User_Pwd"].ToString()
                                    });
                                    context.Response.Write("3");
                                    context.Response.End();
                                }
                                else
                                {
                                    context.Response.Write("6");
                                    context.Response.End();
                                }
                            }
                            else
                            {
                                user_idao.SysLoginLog(user_Account, "2", OWNER_address);
                                context.Response.Write("2");
                                context.Response.End();
                            }
                        }
                        else
                        {
                            user_idao.SysLoginLog(user_Account, "0", OWNER_address);
                            context.Response.Write("4");
                            context.Response.End();
                        }
                    }
                    else
                    {
                        context.Response.Write("5");
                        context.Response.End();
                    }
                }
            }
        }

        public bool Islogin(HttpContext context, string User_Account)
        {
            return true;
        }
    }
}