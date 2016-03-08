using RM.Busines;
using RM.Busines.DAL;
using RM.Busines.IDAO;
using RM.Common.DotNetBean;
using RM.Common.DotNetData;
using RM.Common.DotNetUI;
using RM.Web.App_Code;
using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace RM.Web.RMBase.SysPersonal
{
	public class CurrentUserManager : PageBase
	{
		protected HtmlForm form1;
		protected HtmlInputHidden AppendProperty_value;
		protected Label User_Code;
		protected Label User_Name;
		protected Label User_Account;
		protected Label User_Sex;
		protected Label Email;
		protected Label CreateDate;
		protected new Label Title;
		protected Label DeleteMark;
		protected new Label Theme;
		protected Label User_Remark;
		protected Repeater rp_Item;
		public StringBuilder BasicUserInfoHtml = new StringBuilder();
		public StringBuilder AppendHtml = new StringBuilder();
		public StringBuilder StrTree_Menu = new StringBuilder();
		private RM_UserInfo_IDAO user_idao = new RM_UserInfo_Dal();
		private RM_System_IDAO systemidao = new RM_System_Dal();
		private string Property_Function = "用户附加信息";
		public string _UserName;
		public string _key;
		protected void Page_Load(object sender, EventArgs e)
		{
			this._key = RequestSession.GetSessionUser().UserId.ToString();
			this._UserName = RequestSession.GetSessionUser().UserName.ToString();
			this.InitBasicUserInfo();
			this.AppendHtml.Append(this.systemidao.AppendProperty_HtmlLabel(this.Property_Function));
			ControlBindHelper.BindRepeaterList(this.systemidao.GetRoleByMember(this._key), this.rp_Item);
			this.GetMenuTreeTable();
		}
		public void InitBasicUserInfo()
		{
			Hashtable ht = DataFactory.SqlDataBase().GetHashtableById("Base_UserInfo", "User_ID", this._key);
			if (ht.Count > 0 && ht != null)
			{
				ControlBindHelper.SetWebControls(this.Page, ht);
				if (ht["USER_SEX"].ToString() == "1")
				{
					this.User_Sex.Text = "男士";
				}
				else
				{
					this.User_Sex.Text = "女士";
				}
				if (ht["DELETEMARK"].ToString() == "2")
				{
					this.DeleteMark.Text = "正常";
				}
				else
				{
					if (ht["DELETEMARK"].ToString() == "3")
					{
						this.DeleteMark.Text = "停用";
					}
					else
					{
						if (ht["DELETEMARK"].ToString() == "1")
						{
							this.DeleteMark.Text = "未授权";
						}
					}
				}
				this.AppendProperty_value.Value = this.systemidao.GetPropertyInstancepk(this.Property_Function, this._key);
			}
		}
		public void GetMenuTreeTable()
		{
			DataTable dtList = this.systemidao.GetMenuBind();
			DataTable dtRight = this.systemidao.GetHaveRightUserInfo(this._key);
			if (DataTableHelper.IsExistRows(dtList))
			{
				DataTable dtButoon = DataTableHelper.GetNewDataTable(dtList, "Menu_Type = '3'");
				DataTable dtMenu = DataTableHelper.GetNewDataTable(dtList, "Menu_Type < '3'");
				DataView dv = new DataView(dtMenu);
				dv.RowFilter = " ParentId = '0'";
				int eRowIndex = 0;
				foreach (DataRowView drv in dv)
				{
					string trID = "node-" + eRowIndex.ToString();
					this.StrTree_Menu.Append("<tr id='" + trID + "'>");
					this.StrTree_Menu.Append("<td style='width: 200px;padding-left:20px;'><span class=\"folder\">" + drv["Menu_Name"] + "</span></td>");
					if (!string.IsNullOrEmpty(drv["Menu_Img"].ToString()))
					{
						this.StrTree_Menu.Append("<td style='width: 30px;text-align:center;'><img src='/Themes/images/32/" + drv["Menu_Img"] + "' style='width:16px; height:16px;vertical-align: middle;' alt=''/></td>");
					}
					else
					{
						this.StrTree_Menu.Append("<td style='width: 30px;text-align:center;'><img src='/Themes/images/32/5005_flag.png' style='width:16px; height:16px;vertical-align: middle;' alt=''/></td>");
					}
					this.StrTree_Menu.Append(string.Concat(new object[]
					{
						"<td style=\"width: 23px; text-align: left;\"><input id='ckb",
						trID,
						"' onclick=\"ckbValueObj(this.id)\" style='vertical-align: middle;margin-bottom:2px;' type=\"checkbox\" ",
						this.GetChecked(drv["Menu_Id"].ToString(), dtRight),
						"  value=\"",
						drv["Menu_Id"],
						"\" name=\"checkbox\" /></td>"
					}));
					this.StrTree_Menu.Append("<td>" + this.GetButton(drv["Menu_Id"].ToString(), dtButoon, trID, dtRight) + "</td>");
					this.StrTree_Menu.Append("</tr>");
					this.StrTree_Menu.Append(this.GetTableTreeNode(drv["Menu_Id"].ToString(), dtMenu, trID, dtButoon, dtRight));
					eRowIndex++;
				}
			}
		}
		public string GetTableTreeNode(string parentID, DataTable dtMenu, string parentTRID, DataTable dtButoon, DataTable dtRight)
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
				sb_TreeNode.Append(string.Concat(new object[]
				{
					"<td style=\"width: 23px; text-align: left;\"><input id='ckb",
					trID,
					"' onclick=\"ckbValueObj(this.id)\" ",
					this.GetChecked(drv["Menu_Id"].ToString(), dtRight),
					" style='vertical-align: middle;margin-bottom:2px;' type=\"checkbox\" value=\"",
					drv["Menu_Id"],
					"\" name=\"checkbox\" /></td>"
				}));
				sb_TreeNode.Append("<td>" + this.GetButton(drv["Menu_Id"].ToString(), dtButoon, trID, dtRight) + "</td>");
				sb_TreeNode.Append("</tr>");
				sb_TreeNode.Append(this.GetTableTreeNode(drv["Menu_Id"].ToString(), dtMenu, trID, dtButoon, dtRight));
				i++;
			}
			return sb_TreeNode.ToString();
		}
		public string GetButton(string Menu_Id, DataTable dt, string parentTRID, DataTable dtRight)
		{
			StringBuilder ButtonHtml = new StringBuilder();
			DataTable dt_Button = DataTableHelper.GetNewDataTable(dt, "ParentId = '" + Menu_Id + "'");
			string result;
			if (DataTableHelper.IsExistRows(dt_Button))
			{
				int i = 1;
				foreach (DataRow drv in dt_Button.Rows)
				{
					string trID = parentTRID + "--" + i.ToString();
					ButtonHtml.Append(string.Concat(new object[]
					{
						"<lable><input id='ckb",
						trID,
						"' onclick=\"ckbValueObj(this.id)\" ",
						this.GetChecked(drv["Menu_Id"].ToString(), dtRight),
						" style='vertical-align: middle;margin-bottom:2px;' type=\"checkbox\" value=\"",
						drv["Menu_Id"],
						"\" name=\"checkbox\" />"
					}));
					ButtonHtml.Append(drv["Menu_Name"].ToString() + "</lable>&nbsp;&nbsp;&nbsp;&nbsp;");
					i++;
				}
				result = ButtonHtml.ToString();
			}
			else
			{
				result = ButtonHtml.ToString();
			}
			return result;
		}
		public string GetChecked(string Menu_Id, DataTable dt)
		{
			StringBuilder strSql = new StringBuilder();
			dt = DataTableHelper.GetNewDataTable(dt, "Menu_Id = '" + Menu_Id + "'");
			string result;
			if (DataTableHelper.IsExistRows(dt))
			{
				result = "checked=\"checked\"";
			}
			else
			{
				result = "";
			}
			return result;
		}
	}
}
