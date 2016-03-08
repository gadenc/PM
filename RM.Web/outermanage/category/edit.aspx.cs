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
    public partial class edit : PageBase
    {
        private string action = DTEnums.ActionEnum.Add.ToString(); //操作类型
        private int channel_id;
        private int id = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            string _action = DTRequest.GetQueryString("action");
            this.channel_id = DTRequest.GetQueryInt("channel_id");
            if (channel_id == 0)
            { channel_id = 1; }
            this.id = DTRequest.GetQueryInt("id");

            if (this.channel_id == 0)
            {
                JscriptMsg("频道参数不正确！", "back", "Error");
                return;
            }
            if (!string.IsNullOrEmpty(_action) && _action == DTEnums.ActionEnum.Edit.ToString())
            {
                this.action = DTEnums.ActionEnum.Edit.ToString();//修改类型
                if (this.id == 0)
                {
                    JscriptMsg("传输参数不正确！", "back", "Error");
                    return;
                }
                if (!new DTcms.BLL.category().Exists(this.id))
                {
                    JscriptMsg("类别不存在或已被删除！", "back", "Error");
                    return;
                }
            }
            if (!Page.IsPostBack)
            {
                TreeBind(this.channel_id); //绑定类别
                if (action == DTEnums.ActionEnum.Edit.ToString()) //修改
                {
                    //ChkAdminLevel(channel_id, DTEnums.ActionEnum.Edit.ToString()); //检查权限
                    ShowInfo(this.id);
                }
                else
                {
                    //ChkAdminLevel(channel_id, DTEnums.ActionEnum.Add.ToString()); //检查权限
                    if (this.id > 0)
                    {
                        this.ddlParentId.SelectedValue = this.id.ToString();
                    }
                }
            }
        }

        #region 绑定类别=================================
        private void TreeBind(int _channel_id)
        {
            BLL.category bll = new BLL.category();
            DataTable dt = bll.GetList(0, _channel_id);

            this.ddlParentId.Items.Clear();
            this.ddlParentId.Items.Add(new ListItem("无父级分类", "0"));
            foreach (DataRow dr in dt.Rows)
            {
                string Id = dr["id"].ToString();
                int ClassLayer = int.Parse(dr["class_layer"].ToString());
                string Title = dr["title"].ToString().Trim();

                if (ClassLayer == 1)
                {
                    this.ddlParentId.Items.Add(new ListItem(Title, Id));
                }
                else
                {
                    Title = "├ " + Title;
                    Title = Utils.StringOfChar(ClassLayer - 1, "　") + Title;
                    this.ddlParentId.Items.Add(new ListItem(Title, Id));
                }
            }
        }
        #endregion

        #region 赋值操作=================================
        private void ShowInfo(int _id)
        {
            BLL.category bll = new BLL.category();
            Model.category model = bll.GetModel(_id);

            ddlParentId.SelectedValue = model.parent_id.ToString();
            txtCallIndex.Text = model.call_index;
            txtTitle.Text = model.title;
            txtSortId.Text = model.sort_id.ToString();
            txtSeoTitle.Text = model.seo_title;
            txtSeoKeywords.Text = model.seo_keywords;
            txtSeoDescription.Text = model.seo_description;
            txtLinkUrl.Text = model.link_url;
            txtImgUrl.Text = model.img_url;
            txtContent.Value = model.content;
        }
        #endregion

        #region 增加操作=================================
        private bool DoAdd()
        {
            try
            {
                Model.category model = new Model.category();
                BLL.category bll = new BLL.category();

                model.channel_id = this.channel_id;
                model.call_index = txtCallIndex.Text.Trim();
                model.title = txtTitle.Text.Trim();
                model.parent_id = int.Parse(ddlParentId.SelectedValue);
                model.sort_id = int.Parse(txtSortId.Text.Trim());
                model.seo_title = txtSeoTitle.Text;
                model.seo_keywords = txtSeoKeywords.Text;
                model.seo_description = txtSeoDescription.Text;
                model.link_url = txtLinkUrl.Text.Trim();
                model.img_url = txtImgUrl.Text.Trim();
                model.content = txtContent.Value;
                if (bll.Add(model) < 1)
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
        #endregion

        #region 修改操作=================================
        private bool DoEdit(int _id)
        {
            try
            {
                BLL.category bll = new BLL.category();
                Model.category model = bll.GetModel(_id);
                int parentId = int.Parse(ddlParentId.SelectedValue);

                model.channel_id = this.channel_id;
                model.call_index = txtCallIndex.Text.Trim();
                model.title = txtTitle.Text.Trim();
                //如果选择的父ID不是自己,则更改
                if (parentId != model.id)
                {
                    model.parent_id = parentId;
                }
                model.sort_id = int.Parse(txtSortId.Text.Trim());
                model.seo_title = txtSeoTitle.Text;
                model.seo_keywords = txtSeoKeywords.Text;
                model.seo_description = txtSeoDescription.Text;
                model.link_url = txtLinkUrl.Text.Trim();
                model.img_url = txtImgUrl.Text.Trim();
                model.content = txtContent.Value;
                if (!bll.Update(model))
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
        #endregion

        //保存类别
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (action == DTEnums.ActionEnum.Edit.ToString()) //修改
            {
                if (!DoEdit(this.id))
                {
                    JscriptMsg("保存过程中发生错误啦！", "", "Error");
                    return;
                }
                //JscriptMsg("修改类别成功啦！", "list.aspx?channel_id=" + channel_id, "Success");
                //Response.Redirect("list.aspx?channel_id=" + this.channel_id);
                ShowMsgHelper.ShowAndRedirect(this, "修改类别成功啦！", "list.aspx?channel_id=" + this.channel_id);
            }
            else //添加
            {
                if (!DoAdd())
                {
                    JscriptMsg("保存过程中发生错误啦！", "", "Error");
                    return;
                }
                //JscriptMsg("添加类别成功啦！", "list.aspx?channel_id=" + channel_id, "Success");
                //Response.Redirect("list.aspx?channel_id=" + this.channel_id);
                ShowMsgHelper.ShowAndRedirect(this, "添加类别成功啦！", "list.aspx?channel_id=" + this.channel_id);
            }
        }

    }
}