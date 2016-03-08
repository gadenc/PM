using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DTcms.Model;
using DTcms.BLL;
using System.Data;

namespace WEB
{
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitNav();
            }
        }
        //默认导航是1频道
        //导航菜单的初始化
        public void InitNav()
        {
            nav.Text = GetTopList(0, 1);
        }
        //根据传入的一级菜单id,频道id 返回相应的二级菜单html
        DTcms.BLL.category bll = new DTcms.BLL.category();
        public string GetHtml( int id,int channel_id){
            string htm="";
            //GetChildList
            DataTable dt = bll.GetChildList(id, channel_id);
            if (dt == null || dt.Rows.Count == 0)
            {
                htm = "";
            }
            else
            { 
                htm=" <div class=\"auto_height list\"><ul>";
                for (int i = 0; i < dt.Rows.Count; i++) 
                {
                    htm += "<li><a href='" + dt.Rows[i]["call_index"].ToString() + ".aspx?channel_id=" + dt.Rows[i]["channel_id"].ToString() + "&category_id=" + dt.Rows[i]["id"].ToString() + "'>" + dt.Rows[i]["title"].ToString() + "</a></li>";
                }
                htm += "</ul></div>";
            }
            return htm;
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

        //底部链接
        public string FootHtm() {
            DataTable dt = bll.GetChildList(0,1);
            string htm = "";
            for(int i = 0; i < 4; i++)
            {
                htm += "<li><dl>";
                htm += "<dt><a href='Single.aspx?category_id=" + dt.Rows[i]["id"].ToString() + "&channel_id=" + dt.Rows[i]["channel_id"].ToString() + "' target='_blank'>" + dt.Rows[i]["title"].ToString() + "</a></dt>";
                DataTable cdt = bll.GetChildList(int.Parse(dt.Rows[i]["id"].ToString()),1);
                for(int j = 0; j < 2; j++)
                {
                    htm += "<dd><a href='" + cdt.Rows[j]["call_index"].ToString() + ".aspx?category_id=" + cdt.Rows[j]["id"].ToString() + "&channel_id=" + cdt.Rows[j]["channel_id"].ToString() + "' target='_blank'>" + cdt.Rows[j]["title"].ToString() + "</a></dd>";
                }
                htm += "</dl></li>";
            }
            return htm;
        }
    }
}