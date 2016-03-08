using RM.Common.DotNetConfig;
using RM.Web.App_Code;
using System;
using System.IO;
using System.Text;
using System.Web.UI.HtmlControls;

namespace RM.Web.RMBase.SysLog
{
    public class SysLog_Left : PageBase
    {
        public StringBuilder strHtml = new StringBuilder();
        protected HtmlForm form1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                this.InitInfo();
            }
        }
        public void InitInfo()
        {
            string LogFilePath = ConfigHelper.GetAppSettings("LogFilePath");
            DirectoryInfo dir = new DirectoryInfo(LogFilePath);
            FileSystemInfo[] fileSystemInfos = dir.GetFileSystemInfos();
            for (int i = 0; i < fileSystemInfos.Length; i++)
            {
                FileInfo fsi = (FileInfo)fileSystemInfos[i];
                if (fsi.Name != "Backup_Restore_Log.log")
                {
                    this.strHtml.Append("<li>");
                    this.strHtml.Append(string.Concat(new string[]
					{
						"<div title='",
						fsi.Name,
						"' onclick=\"FileName('",
						fsi.Name,
						"')\">",
						fsi.Name,
						"</div>"
					}));
                    this.strHtml.Append("</li>");
                }
            }
        }
    }
}
