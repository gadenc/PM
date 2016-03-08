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

namespace RM.Web.UserControl
{
    public partial class NewsList : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DTcms.BLL.article article = new article();
            if (!IsPostBack)
            {
                if (ucl_news.Text.Trim() != "")
                {
                    DataTable dt = article.GetNewsList(5, "category_id="+ucl_news.Text.Trim(), "id asc").Tables[0];
                    string htm = "" ;
                    for (int i = 1; i < dt.Rows.Count; i++) {
                        htm += "<li><a href='Detail.aspx?id="+dt.Rows[i]["id"].ToString()+"' target='_blank'>"+dt.Rows[i]["title"].ToString()+"</a></li>";
                    }
                    ucl_news.Text = htm;
                }
                else
                    ucl_news.Text = "";
            }
        }
    }
}