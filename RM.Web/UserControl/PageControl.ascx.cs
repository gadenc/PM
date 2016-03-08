using RM.Common.DotNetBean;
using RM.Common.DotNetUI;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RM.Web.UserControl
{
    public class PageControl : System.Web.UI.UserControl
    {
        protected Label lblRecordCount;
        protected Label default_pgStartRecord;
        protected Label default_pgEndRecord;
        protected LinkButton hlkFirst;
        protected LinkButton hlkPrev;
        protected Label lblCurrentPageIndex;
        protected Label lblPageCount;
        protected LinkButton hlkNext;
        protected LinkButton hlkLast;
        protected DropDownList ddlpageList;
        public event EventHandler pageHandler;
        public int PageSize
        {
            get
            {
                int result;
                if (CookieHelper.GetCookie("PageIndex") == "")
                {
                    result = int.Parse(this.ddlpageList.Text);
                }
                else
                {
                    this.ddlpageList.Text = CookieHelper.GetCookie("PageIndex");
                    result = int.Parse(this.ddlpageList.Text);
                }
                return result;
            }
            set
            {
                if (!this.IsListItem(value.ToString()))
                {
                    this.ddlpageList.Items.Add(value.ToString());
                }
                this.ddlpageList.Text = value.ToString();
            }
        }
        public int PageIndex
        {
            get
            {
                return int.Parse(this.lblCurrentPageIndex.Text);
            }
            set
            {
                this.lblCurrentPageIndex.Text = value.ToString();
            }
        }
        public int PagestartNumber
        {
            get
            {
                return (this.PageIndex - 1) * this.PageSize + 1;
            }
        }
        public int PageendNumber
        {
            get
            {
                return this.PageIndex * this.PageSize;
            }
        }
        public int RecordCount
        {
            get;
            set;
        }
        public int TotaPage
        {
            get
            {
                return (this.RecordCount % this.PageSize == 0) ? (this.RecordCount / this.PageSize) : (this.RecordCount / this.PageSize + 1);
            }
        }
        public bool IsListItem(string obj_value)
        {
            bool isok = false;
            foreach (ListItem item in this.ddlpageList.Items)
            {
                if (item.Value.Trim() == obj_value.ToString())
                {
                    isok = true;
                    break;
                }
            }
            return isok;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                this.pager_PageChanged(sender, e);
            }
        }
        public void pager_PageChanged(object sender, EventArgs e)
        {
            this.pageHandler(sender, e);
            this.PageChecking();
        }
        protected void hlkFirst_Click(object sender, EventArgs e)
        {
            this.PageIndex = 1;
            this.pager_PageChanged(sender, e);
            this.PageChecking();
        }
        protected void hlkPrev_Click(object sender, EventArgs e)
        {
            this.PageIndex--;
            this.pager_PageChanged(sender, e);
            this.PageChecking();
        }
        protected void hlkNext_Click(object sender, EventArgs e)
        {
            this.PageIndex++;
            this.pager_PageChanged(sender, e);
            this.PageChecking();
        }
        protected void hlkLast_Click(object sender, EventArgs e)
        {
            this.PageIndex = int.Parse(this.lblPageCount.Text);
            this.pager_PageChanged(sender, e);
            this.PageChecking();
        }
        public void PageChecking()
        {
            this.lblRecordCount.Text = this.RecordCount.ToString();
            this.lblCurrentPageIndex.Text = this.PageIndex.ToString();
            this.lblPageCount.Text = this.TotaPage.ToString();
            this.default_pgStartRecord.Text = this.PagestartNumber.ToString();
            this.default_pgEndRecord.Text = this.PageendNumber.ToString();
            if (this.TotaPage == 0 || this.TotaPage == 1)
            {
                this.hlkFirst.Enabled = false;
                this.hlkPrev.Enabled = false;
                this.hlkNext.Enabled = false;
                this.hlkLast.Enabled = false;
                ShowMsgHelper.ShowScript("Script(1)");
            }
            else
            {
                if (this.PageIndex == 1)
                {
                    this.hlkFirst.Enabled = false;
                    this.hlkPrev.Enabled = false;
                    this.hlkNext.Enabled = true;
                    this.hlkLast.Enabled = true;
                    ShowMsgHelper.ShowScript("Script(2)");
                }
                else
                {
                    if (this.PageIndex == this.TotaPage)
                    {
                        this.hlkFirst.Enabled = true;
                        this.hlkPrev.Enabled = true;
                        this.hlkNext.Enabled = false;
                        this.hlkLast.Enabled = false;
                        ShowMsgHelper.ShowScript("Script(3)");
                    }
                    else
                    {
                        this.hlkFirst.Enabled = true;
                        this.hlkPrev.Enabled = true;
                        this.hlkNext.Enabled = true;
                        this.hlkLast.Enabled = true;
                        ShowMsgHelper.ShowScript("Script(4)");
                    }
                }
            }
        }
        protected void ddlpageList_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.PageSize = int.Parse(this.ddlpageList.Text);
            this.pager_PageChanged(sender, e);
        }
    }
}