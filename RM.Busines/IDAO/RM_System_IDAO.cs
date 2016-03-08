using System.Data;

namespace RM.Busines.IDAO
{
    public interface RM_System_IDAO
    {
        int DeleteData_Base(string tableName, string pkName, string[] pkVal);

        DataTable GetMenuBind();

        DataTable GetMenuList();

        DataTable GetMenuHtml(string UserId);

        DataTable GetSysMenuByButton(string Menu_Id);

        DataTable GetButtonList();

        int AllotButton(string pkVal, string ParentId);

        DataTable GetButtonHtml(string User_ID);

        DataTable GetHaveRightUserInfo(string User_ID);

        DataTable GetPermission_URL(string UserId);

        DataTable InitRoleList();

        DataTable InitRoleParentId();

        bool Add_RoleAllotMember(string[] pkVal, string Roles_ID);

        DataTable InitRoleRight(string Roles_ID);

        DataTable InitUserRole(string Roles_ID);

        bool Add_RoleAllotAuthority(string[] pkVal, string Roles_ID);

        DataTable GetRoleByMember(string user_id);

        DataTable AppendProperty_List(string Function);

        DataTable AppendProperty_Function();

        string GetPropertyInstancepk(string Property_Function, string Obj_ID);

        string AppendProperty_Html(string Function);

        string AppendProperty_HtmlLabel(string Function);

        bool Add_AppendPropertyInstance(string guid, string[] arrayitem);

        DataTable GetHomeShortcut_List(string User_ID);

        DataTable GetRecyclebin_ObjField(string object_TableName);

        int Virtualdelete(string module, string tableName, string pkName, string[] pkVal);

        int Recyclebin_Restore(string[] pkVal);

        int Recyclebin_Empty(string[] pkVal);

        bool DataRestore(string FilePath);

        bool DataBackups(string FilePath);

        void Add_Backup_Restore_Log(string Type, string File, string Size, string CreateUserName, string DB, string Memo);

        DataTable GetBackup_Restore_Log_List();

        DataTable GetSysobjects();

        DataTable GetSyscolumns(string object_id);
    }
}