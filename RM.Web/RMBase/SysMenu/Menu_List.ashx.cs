using RM.Busines.DAL;
using RM.Busines.IDAO;
using RM.Web.App_Code;
using RM.Web;
using System;
using System.Data;
using System.Text;
using System.Web.UI.HtmlControls;
using RM.Web.UserControl;

namespace RM.Web.RMBase.SysMenu
{
	public class Menu_List1 : PageBase
	{
		protected HtmlHead Head1;
		protected HtmlForm form1;
		protected LoadButton LoadButton1;
		public StringBuilder TableTree_Menu = new StringBuilder();
		private RM_System_IDAO systemidao = new RM_System_Dal();
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!base.IsPostBack)
			{
				this.GetMenuTreeTable();
			}
		}
		public void GetMenuTreeTable()
		{
			DataTable dtMenu = this.systemidao.GetMenuList();
			DataView dv = new DataView(dtMenu);
			dv.RowFilter = " ParentId = '0'";
			int eRowIndex = 0;
			foreach (DataRowView drv in dv)
			{
				string trID = "node-" + eRowIndex.ToString();
				this.TableTree_Menu.Append("<tr id='" + trID + "'>");
				this.TableTree_Menu.Append("<td style='width: 230px;padding-left:20px;'><span class=\"folder\">" + drv["Menu_Name"].ToString() + "</span></td>");
				if (!string.IsNullOrEmpty(drv["Menu_Img"].ToString()))
				{
					this.TableTree_Menu.Append("<td style='width: 30px;text-align:center;'><img src='/Themes/images/32/" + drv["Menu_Img"].ToString() + "' style='width:16px; height:16px;vertical-align: middle;' alt=''/></td>");
				}
				else
				{
					this.TableTree_Menu.Append("<td style='width: 30px;text-align:center;'><img src='/Themes/images/32/5005_flag.png' style='width:16px; height:16px;vertical-align: middle;' alt=''/></td>");
				}
				this.TableTree_Menu.Append("<td style='width: 60px;text-align:center;'>" + this.GetMenu_Type(drv["Menu_Type"].ToString()) + "</td>");
				this.TableTree_Menu.Append("<td style='width: 60px;text-align:center;'>" + drv["Target"].ToString() + "</td>");
				this.TableTree_Menu.Append("<td style='width: 60px;text-align:center;'>" + drv["Sort"].ToString() + "</td>");
				this.TableTree_Menu.Append("<td>" + drv["NavigateUrl"].ToString() + "</td>");
				this.TableTree_Menu.Append("<td style='display:none'>" + drv["Menu_Id"].ToString() + "</td>");
				this.TableTree_Menu.Append("</tr>");
				this.TableTree_Menu.Append(this.GetTableTreeNode(drv["Menu_Id"].ToString(), dtMenu, trID));
				eRowIndex++;
			}
		}
		public string GetTableTreeNode(string parentID, DataTable dtMenu, string parentTRID)
		{
			StringBuilder sb_TreeNode = new StringBuilder();
			DataView dv = new DataView(dtMenu);
			dv.RowFilter = "ParentId = '" + parentID + "'";
			int i = 1;
			foreach (DataRowView drv in dv)
			{
				string trID = parentTRID + "-" + i.ToString();
				sb_TreeNode.Append(string.Concat(new string[]
				{
					"<tr id='",
					trID,
					"' class='child-of-",
					parentTRID,
					"'>"
				}));
				sb_TreeNode.Append("<td style='padding-left:20px;'><span class=\"folder\">" + drv["Menu_Name"].ToString() + "</span></td>");
				if (!string.IsNullOrEmpty(drv["Menu_Img"].ToString()))
				{
					sb_TreeNode.Append("<td style='width: 30px;text-align:center;'><img src='/Themes/images/32/" + drv["Menu_Img"].ToString() + "' style='width:16px; height:16px;vertical-align: middle;' alt=''/></td>");
				}
				else
				{
					sb_TreeNode.Append("<td style='width: 30px;text-align:center;'><img src='/Themes/images/32/5005_flag.png' style='width:16px; height:16px;vertical-align: middle;' alt=''/></td>");
				}
				sb_TreeNode.Append("<td style='width: 60px;text-align:center;'>" + this.GetMenu_Type(drv["Menu_Type"].ToString()) + "</td>");
				sb_TreeNode.Append("<td style='width: 60px;text-align:center;'>" + drv["Target"].ToString() + "</td>");
				sb_TreeNode.Append("<td style='width: 60px;text-align:center;'>" + drv["Sort"].ToString() + "</td>");
				sb_TreeNode.Append("<td>" + drv["NavigateUrl"].ToString() + "</td>");
				sb_TreeNode.Append("<td style='display:none'>" + drv["Menu_Id"].ToString() + "</td>");
				sb_TreeNode.Append("</tr>");
				sb_TreeNode.Append(this.GetTableTreeNode(drv["Menu_Id"].ToString(), dtMenu, trID));
				i++;
			}
			return sb_TreeNode.ToString();
		}
		public string GetMenu_Type(string Menu_Type)
		{
			string result;
			if (Menu_Type == "1")
			{
				result = "父节";
			}
			else
			{
				if (Menu_Type == "2")
				{
					result = "子节";
				}
				else
				{
					if (Menu_Type == "3")
					{
						result = "按钮";
					}
					else
					{
						result = "其他";
					}
				}
			}
			return result;
		}
	}
}
