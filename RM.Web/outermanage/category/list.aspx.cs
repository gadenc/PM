using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DTcms.Common;
using RM.Web.App_Code;
using RM.Common.DotNetUI;
namespace DTcms.Web.admin.category
{
    public partial class list : PageBase
    {
        protected int channel_id;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.channel_id = DTRequest.GetQueryInt("channel_id");
            if (channel_id == 0)
            { 
                channel_id = 1;
                
            }
            if (this.channel_id == 0)
            {
                JscriptMsg("频道参数不正确！", "back", "Error");
                return;
            }
            if (!Page.IsPostBack)
            {
                //ChkAdminLevel(channel_id, DTEnums.ActionEnum.View.ToString()); //检查权限
                //channel.SelectedIndex = channel_id;
                if (channel_id == 5 || channel_id==6)
                {
                    
                    channel.Visible = false;
                    xhw_channel.Visible = true;
                    xhw_channel.SelectedValue = channel_id.ToString();
                }
                else
                    channel.SelectedValue = channel_id.ToString();
                RptBind();
            }
        }

        //数据绑定
        private void RptBind()
        {
            BLL.category bll = new BLL.category();
            DataTable dt = bll.GetList(0, this.channel_id);
            this.rptList.DataSource = dt;
            this.rptList.DataBind();
        }

        //美化列表
        protected void rptList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                Literal LitFirst = (Literal)e.Item.FindControl("LitFirst");
                HiddenField hidLayer = (HiddenField)e.Item.FindControl("hidLayer");
                string LitStyle = "<span style=width:{0}px;text-align:right;display:inline-block;>{1}{2}</span>";
                string LitImg1 = "<img src=../images/folder_open.gif align=absmiddle />";
                string LitImg2 = "<img src=../images/t.gif align=absmiddle />";

                int classLayer = Convert.ToInt32(hidLayer.Value);
                if (classLayer == 1)
                {
                    LitFirst.Text = LitImg1;
                }
                else
                {
                    LitFirst.Text = string.Format(LitStyle, classLayer * 18, LitImg2, LitImg1);
                }
            }
        }

        //保存排序
        protected void btnSave_Click(object sender, EventArgs e)
        {
            BLL.category bll = new BLL.category();
            for (int i = 0; i < rptList.Items.Count; i++)
            {
                int id = Convert.ToInt32(((HiddenField)rptList.Items[i].FindControl("hidId")).Value);
                int sortId;
                if (!int.TryParse(((TextBox)rptList.Items[i].FindControl("txtSortId")).Text.Trim(), out sortId))
                {
                    sortId = 99;
                }
                bll.UpdateField(id, "sort_id=" + sortId.ToString());
            }
            //JscriptMsg("保存排序成功啦！", Utils.CombUrlTxt("list.aspx", "channel_id={0}", this.channel_id.ToString()), "Success");
            //Response.Redirect(Utils.CombUrlTxt("list.aspx", "channel_id={0}", this.channel_id.ToString()));
            ShowMsgHelper.ShowAndRedirect(this, "保存排序成功啦！", Utils.CombUrlTxt("list.aspx", "channel_id={0}", this.channel_id.ToString()));
        }

        //删除类别
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            //ChkAdminLevel(channel_id, DTEnums.ActionEnum.Delete.ToString()); //检查权限
            BLL.category bll = new BLL.category();
            for (int i = 0; i < rptList.Items.Count; i++)
            {
                int id = Convert.ToInt32(((HiddenField)rptList.Items[i].FindControl("hidId")).Value);
                CheckBox cb = (CheckBox)rptList.Items[i].FindControl("chkId");
                if (cb.Checked)
                {
                    bll.Delete(id);
                }
            }
            //JscriptMsg("批量删除成功啦！", Utils.CombUrlTxt("list.aspx", "channel_id={0}", this.channel_id.ToString()), "Success");
            //Response.Redirect(Utils.CombUrlTxt("list.aspx", "channel_id={0}", this.channel_id.ToString()));
            ShowMsgHelper.ShowAndRedirect(this, "批量删除成功啦！", Utils.CombUrlTxt("list.aspx", "channel_id={0}", this.channel_id.ToString()));
        }

        protected void channel_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Response.Write("<script>alert('1');</script>");
            
            Response.Redirect("list.aspx?channel_id="+channel.SelectedValue);
        }

        protected void xhw_channel_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Response.Write("<script>alert('1');</script>");

            Response.Redirect("list.aspx?channel_id=" + xhw_channel.SelectedValue);
        }

    }
}