using RM.Busines.DAL;
using RM.Busines.IDAO;
using RM.Common.DotNetBean;
using RM.Common.DotNetData;
using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
namespace RM.Web.Frame
{
    public partial class MainTree : Page
    {
        protected HtmlForm form1;
        public StringBuilder strHtml = new StringBuilder();
        private RM_System_IDAO systemidao = new RM_System_Dal();
        protected void Page_Load(object sender, EventArgs e)
        {
            this.InitInfo();
        }
        public void InitInfo()
        {
            string UserId = RequestSession.GetSessionUser().UserId.ToString();
            DataTable dt = this.systemidao.GetMenuHtml(UserId);
            if (DataTableHelper.IsExistRows(dt))
            {
                foreach (DataRowView drv in new DataView(dt)
                {
                    RowFilter = "ParentId = '0'"
                })
                {
                    this.strHtml.Append("<li>");
                    this.strHtml.Append("<div>" + drv["Menu_Name"] + "</div>");
                    this.strHtml.Append(this.GetTreeNode(drv["Menu_Id"].ToString(), dt));
                    this.strHtml.Append("</li>");
                }
            }
        }
        public string GetTreeNode(string parentID, DataTable dtNode)
        {
            StringBuilder sb_TreeNode = new StringBuilder();
            DataView dv = new DataView(dtNode);
            dv.RowFilter = "ParentId = '" + parentID + "'";
            if (dv.Count > 0)
            {
                sb_TreeNode.Append("<ul>");
                foreach (DataRowView drv in dv)
                {
                    sb_TreeNode.Append("<li>");
                    DataTable IsJudge = DataTableHelper.GetNewDataTable(dtNode, "ParentId = '" + drv["Menu_Id"].ToString() + "'");
                    if (DataTableHelper.IsExistRows(IsJudge))
                    {
                        sb_TreeNode.Append("<div>" + drv["Menu_Name"] + "</div>");
                    }
                    else
                    {
                        sb_TreeNode.Append(string.Concat(new object[]
						{
							"<div title=\"",
							drv["Menu_Title"],
							"\" onclick=\"NavMenu('",
							drv["NavigateUrl"],
							"','",
							drv["Menu_Name"],
							"')\"><img src=\"/Themes/Images/32/",
							drv["Menu_Img"],
							"\" width=\"16\" height=\"16\" />",
							drv["Menu_Name"],
							"</div>"
						}));
                    }
                    sb_TreeNode.Append(this.GetTreeNode(drv["Menu_Id"].ToString(), dtNode));
                    sb_TreeNode.Append("</li>");
                }
                sb_TreeNode.Append("</ul>");
            }
            return sb_TreeNode.ToString();
        }
    }
}
