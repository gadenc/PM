using RM.Common.DotNetConfig;
using RM.Common.DotNetUI;
using RM.Web.App_Code;
using System;
using System.IO;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace RM.Web.RMBase.SysLog
{
    public class SysLog_Content : PageBase
    {
        protected HtmlForm form1;
        protected LinkButton hlkempty;
        protected HtmlTextArea txtLog;
        public string _FileName;
        protected void Page_Load(object sender, EventArgs e)
        {
            this._FileName = base.Request["FileName"];
            if (!base.IsPostBack)
            {
                if (this._FileName != null)
                {
                    this.GetTxtValue();
                }
                else
                {
                    this._FileName = "未选择文件目录";
                }
            }
        }
        public void GetTxtValue()
        {
            string filepath = ConfigHelper.GetAppSettings("LogFilePath") + "\\" + this._FileName;
            if (File.Exists(filepath))
            {
                StreamReader sr = new StreamReader(filepath, Encoding.GetEncoding("UTF-8"));
                string txtvalue = sr.ReadToEnd().ToString();
                sr.Close();
                this.txtLog.InnerText = txtvalue;
            }
        }
        protected void hlkempty_Click1(object sender, EventArgs e)
        {
            string filepath = ConfigHelper.GetAppSettings("LogFilePath") + "\\" + this._FileName;
            FileStream fs = new FileStream(filepath, FileMode.Create, FileAccess.Write);
            fs.Close();
            ShowMsgHelper.ShowScript("Alert_Ok()");
        }
    }
}
