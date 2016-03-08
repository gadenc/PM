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
    public partial class Detail : System.Web.UI.Page
    {
        public string title;
        public string content;
        protected int channel_id;
        protected int category_id;
        protected int parent_id;
        DTcms.BLL.category bll = new DTcms.BLL.category();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitNav();
                InitData();
            }
        }

        public void InitData() {
            int id = DTRequest.GetQueryInt("id");
            DTcms.BLL.article article = new article();
            DTcms.Model.article_news model = article.GetNewsModel(id);
            title = model.title;
            content = model.content;
        }

        //默认导航是1频道
        //导航菜单的初始化
        public void InitNav()
        {
            nav.Text = GetTopList(0, 1);
        }
        //根据父节点id返回相应字节点菜单
        public string GetTopList(int parent_id, int channel_id)
        {
            string htm = "";
            DataTable dt = bll.GetTopList(parent_id, channel_id);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                htm += "<li class=\"navli\"><a class=\"a\" href=\"Single.aspx?category_id=" + dt.Rows[i]["id"].ToString() + "&channel_id=" + dt.Rows[i]["channel_id"].ToString() + "\"><font style=\"font-size: 14px\">" + dt.Rows[i]["title"] + "</font></a>";
                htm += "<div class=\"stair auto_height\"  style=\" display:none;\">";
                htm += "<div class=\"auto_height list\">";
                htm += GetHtml(int.Parse(dt.Rows[i]["id"].ToString()), int.Parse(dt.Rows[i]["channel_id"].ToString()));
                htm += "</div></div></li>";
            }
            return htm;
        }

        //根据传入的一级菜单id,频道id 返回相应的二级菜单html
        public string GetHtml(int id, int channel_id)
        {
            string htm = "";
            //GetChildList
            DataTable dt = bll.GetChildList(id, channel_id);
            if (dt == null || dt.Rows.Count == 0)
            {
                htm = "";
            }
            else
            {
                htm = " <div class=\"auto_height list\"><ul>";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    htm += "<li><a href='" + dt.Rows[i]["call_index"].ToString() + ".aspx?channel_id=" + dt.Rows[i]["channel_id"].ToString() + "&category_id=" + dt.Rows[i]["id"].ToString() + "'>" + dt.Rows[i]["title"].ToString() + "</a></li>";
                }
                htm += "</ul></div>";
            }
            return htm;
        }
    }
}