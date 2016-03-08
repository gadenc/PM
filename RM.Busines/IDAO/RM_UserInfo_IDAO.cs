using RM.Common.DotNetCode;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace RM.Busines.IDAO
{
    public interface RM_UserInfo_IDAO
    {
        DataTable UserLogin(string name, string pwd);

        DataTable GetUserInfoPage(StringBuilder SqlWhere, IList<SqlParam> IList_param, int pageIndex, int pageSize, ref int count);

        DataTable GetUserInfoInfo(StringBuilder SqlWhere, IList<SqlParam> IList_param);

        DataTable InitUserRight(string User_ID);

        DataTable InitUserInfoUserGroup(string User_ID);

        DataTable InitUserRole(string User_ID);

        DataTable InitStaffOrganize(string User_ID);

        void SysLoginLog(string SYS_USER_ACCOUNT, string SYS_LOGINLOG_STATUS, string OWNER_address);

        DataTable GetSysLoginLogPage(StringBuilder SqlWhere, IList<SqlParam> IList_param, int pageIndex, int pageSize, ref int count);

        DataTable GetLogin_Info(ref int count);

        DataTable Load_StaffOrganizeList();

        DataTable GetOrganizeList();

        DataTable InitUserGroupList();

        DataTable InitUserGroupParentId();

        DataTable Load_UserInfoUserGroupList(string UserGroup_ID);

        DataTable InitUserGroupRight(string UserGroup_ID);

        bool AddUserGroupMenber(string[] User_ID, string UserGroup_ID);

        bool Add_UserGroupAllotAuthority(string[] pkVal, string UserGroup_ID);
    }
}