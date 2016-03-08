using RM.Web.App_Code;
using RM.Web;
using System;
using System.IO;
using System.Text;
using System.Web.UI.HtmlControls;
using RM.Web.UserControl;

namespace RM.Web.RMBase.SysMenu
{
	public class Icons_List : PageBase
	{
		public StringBuilder strImg = new StringBuilder();
		private string _Size;
		protected HtmlForm form1;
		protected HtmlInputHidden hidden_Size;
		protected PageControl PageControl1;
		protected void Page_Load(object sender, EventArgs e)
		{
			this._Size = base.Request["Size"];
			this.PageControl1.pageHandler += new EventHandler(this.pager_PageChanged);
			if (!base.IsPostBack)
			{
				if (this._Size != null)
				{
					this.hidden_Size.Value = this._Size;
				}
			}
		}
		protected void pager_PageChanged(object sender, EventArgs e)
		{
			this.GetImg();
		}
		public void GetImg()
		{
			int PageIndex = this.PageControl1.PageIndex;
			int PageSize;
			DirectoryInfo dir;
			if (this.hidden_Size.Value == "32")
			{
				PageSize = (this.PageControl1.PageSize = 100);
				dir = new DirectoryInfo(base.Server.MapPath("/Themes/Images/32/"));
			}
			else
			{
				PageSize = (this.PageControl1.PageSize = 200);
				dir = new DirectoryInfo(base.Server.MapPath("/Themes/Images/16/"));
			}
			int rowCount = 0;
			int rowbegin = (PageIndex - 1) * PageSize;
			int rowend = PageIndex * PageSize;
			FileSystemInfo[] fileSystemInfos = dir.GetFileSystemInfos();
			for (int i = 0; i < fileSystemInfos.Length; i++)
			{
				FileInfo fsi = (FileInfo)fileSystemInfos[i];
				if (rowCount >= rowbegin && rowCount < rowend)
				{
					this.strImg.Append("<div class=\"divicons\" title='" + fsi.Name + "'>");
					this.strImg.Append(string.Concat(new string[]
					{
						"<img src=\"/Themes/Images/",
						this.hidden_Size.Value,
						"/",
						fsi.Name,
						"\" />"
					}));
					this.strImg.Append("</div>");
				}
				rowCount++;
			}
			this.PageControl1.RecordCount = Convert.ToInt32(rowCount);
		}
	}
}
