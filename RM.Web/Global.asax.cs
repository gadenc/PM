using RM.Common.DotNetCode;
using RM.Common.DotNetConfig;
using System;
using System.Timers;
using System.Web;

namespace RM.Web
{
    public class Global : HttpApplication
    {
        protected LogHelper Logger = new LogHelper("Global");
        private void Application_Start(object sender, EventArgs e)
        {
            base.Application.Lock();
            base.Application["CurrentUsers"] = 0;
            base.Application.UnLock();
            Timer myTimer = new Timer(60000.0);
            myTimer.Elapsed += new ElapsedEventHandler(this.OnTimedEvent);
            myTimer.Interval = 1000.0;
            myTimer.Enabled = true;
        }
        private void Application_Error(object sender, EventArgs e)
        {
            Exception objErr = base.Server.GetLastError().GetBaseException();
            string error = objErr.Message ?? "";
            base.Server.ClearError();
            base.Application["error"] = error;
            base.Response.Redirect("~/Error/ErrorPage.aspx");
        }
        private void Session_Start(object sender, EventArgs e)
        {
            base.Application.Lock();
            base.Application["CurrentUsers"] = (int)base.Application["CurrentUsers"] + 1;
            base.Application.UnLock();
        }
        private void Session_End(object sender, EventArgs e)
        {
            base.Application.Lock();
            base.Application["CurrentUsers"] = (int)base.Application["CurrentUsers"] - 1;
            base.Application.UnLock();
        }
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            this.RestartIIS();
        }
        private void RestartIIS()
        {
            if (ConfigHelper.GetAppSettings("IsRestartIIS").Equals("true"))
            {
                if (DateTime.Now.ToString("HH:mm:ss").Equals(ConfigHelper.GetAppSettings("RestartIISTime")))
                {
                    this.Logger.WriteLog("自动重启IIS时间到了");
                }
            }
        }
    }
}
