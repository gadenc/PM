using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DTcms.Model;
using DTcms.BLL;
using System.Data;
using DTcms.Common;

namespace RM.Web
{
    public partial class list : System.Web.UI.Page
    {
        protected int totalCount;
        protected int page;
        protected int pageSize;

        protected string property = string.Empty;
        protected string keywords = string.Empty;
        protected string prolistview = string.Empty;
        protected int channel_id;
        protected int category_id;
        protected int parent_id;
        DTcms.BLL.category category_bll = new DTcms.BLL.category();
        DTcms.BLL.article article_bll = new article();
        DTcms.Model.article_news model = new DTcms.Model.article_news();
        protected void Page_Load(object sender, EventArgs e)
        {
            this.pageSize = GetPageSize(4); //每页数量
            if (!IsPostBack)
                InitData();
        }

        //初始化数据
        public void InitData()
        {
            this.page = DTRequest.GetQueryInt("page", 1);
            this.channel_id = DTRequest.GetQueryInt("channel_id");
            this.category_id = DTRequest.GetQueryInt("category_id");
            parent_id = category_bll.GetParentId(category_id);
            //DataTable dt = article_bll.GetNewsList(10,"id="+category_id.ToString(),"id asc").Tables[0];
            //this.rptList1.DataSource = bll.GetNewsList(this.pageSize, this.page, _strWhere, _orderby, out this.totalCount);
            newslist.DataSource = article_bll.GetNewsList(this.pageSize, this.page, "category_id=" + category_id.ToString(), "id asc", out this.totalCount);
            newslist.DataBind();
            //分页样式
            string pageUrl = Utils.CombUrlTxt("list.aspx", "channel_id={0}&category_id={1}&keywords={2}&property={3}&page={4}",
                this.channel_id.ToString(), this.category_id.ToString(), this.keywords, this.property, "__id__");
            PageContent.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
        }

        #region 返回图文每页数量=========================
        private int GetPageSize(int _default_size)
        {
            int _pagesize;
            if (int.TryParse(Utils.GetCookie("article_page_size"), out _pagesize))
            {
                if (_pagesize > 0)
                {
                    return _pagesize;
                }
            }
            return _default_size;
        }
        #endregion
    }
}