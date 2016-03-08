using RM.Busines.DAL;
using RM.Busines.IDAO;
using RM.Common.DotNetJson;
using System;
using System.ComponentModel;
using System.Web.Services;

namespace RM.Web.WebService
{
    [ToolboxItem(false), WebService(Namespace = "http://tempuri.org/"), WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class RM_Interface : System.Web.Services.WebService
    {
        private RM_System_IDAO systemidao = new RM_System_Dal();
        [WebMethod(EnableSession = true, Description = "权限菜单导航（返回JSON格式），参数 UserId:用户主键")]
        public string GetMenuInfo(string UserId)
        {
            return JsonHelper.DataTableToJson(this.systemidao.GetMenuHtml(UserId), "RM");
        }
        [WebMethod(EnableSession = true, Description = "URL权限验证,拒绝，不合法的请求（返回JSON格式），参数 UserId:用户主键")]
        public string GetURLPermission(string UserId)
        {
            return JsonHelper.DataTableToJson(this.systemidao.GetPermission_URL(UserId), "RM");
        }
        [WebMethod(EnableSession = true, Description = "权限操作按钮（返回JSON格式），参数 UserId:用户主键")]
        public string GetButtonHtml(string UserId)
        {
            return JsonHelper.DataTableToJson(this.systemidao.GetButtonHtml(UserId), "RM");
        }
        [WebMethod(EnableSession = true, Description = "显示拥有所有权限（返回JSON格式），参数 UserId:用户主键")]
        public string GetHaveRightUserInfo(string UserId)
        {
            return JsonHelper.DataTableToJson(this.systemidao.GetHaveRightUserInfo(UserId), "RM");
        }
    }
}
