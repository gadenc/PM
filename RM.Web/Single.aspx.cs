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
    public partial class Single : System.Web.UI.Page
    {
        public string content="";
        protected int channel_id;
        protected int category_id;
        protected int parent_id;
        DTcms.BLL.category category_bll = new DTcms.BLL.category();
        DTcms.BLL.article article_bll = new article();
        DTcms.Model.article_news model=new DTcms.Model.article_news();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitData();
            }
        }

        //页面数据初始化
        public void InitData()
        {
            this.channel_id = DTRequest.GetQueryInt("channel_id");
            this.category_id = DTRequest.GetQueryInt("category_id");
            parent_id = category_bll.GetParentId(category_id);
            if (parent_id == 0)
            {
                //parentid==0 说明是从头部导航链接过来的
                //判断这个类别是否有子类别，有就显示这个子类别中第一个类别链接的内容
                //没有就跳转到这个类别下的内容
                DataTable dt = category_bll.GetChildList(category_id,channel_id);
                if (dt != null && dt.Rows.Count > 0)
                {

                    Response.Redirect(dt.Rows[0]["call_index"].ToString() + ".aspx?channel_id=" + dt.Rows[0]["channel_id"].ToString() + "&category_id=" + dt.Rows[0]["id"].ToString());
                }
                else
                {
                    //Response.Redirect("Single.aspx?channel_id=" + channel_id.ToString() + "&category_id=" + category_id.ToString());
                    DataTable dat = article_bll.GetNewsList(1, "category_id=" + category_id, "id asc").Tables[0];
                    if (dat.Rows.Count == 0 || dt == null)
                        content = "";
                    else
                    {
                        model = article_bll.GetNewsModel(int.Parse(dat.Rows[0]["id"].ToString()));
                        content = model.content.ToString();
                    }
                }

            }
            else
            {
                DataTable  dt = article_bll.GetNewsList(1, "category_id="+category_id,"id asc").Tables[0];
                if (dt.Rows.Count == 0 || dt == null)
                    content = "";
                else
                {
                    model = article_bll.GetNewsModel(int.Parse(dt.Rows[0]["id"].ToString()));
                    content = model.content.ToString();
                }
            }
            
        }
    }
}