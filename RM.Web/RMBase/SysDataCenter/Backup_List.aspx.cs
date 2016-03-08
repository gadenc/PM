using RM.Busines.DAL;
using RM.Busines.IDAO;
using RM.Common.DotNetBean;
using RM.Common.DotNetConfig;
using RM.Common.DotNetFile;
using RM.Common.DotNetUI;
using RM.Web;
using RM.Web.App_Code;
using RM.Web.UserControl;
using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace RM.Web.RMBase.SysDataCenter
{
    public class Backup_List : PageBase
    {
        protected HtmlForm form1;
        protected HtmlInputHidden Backup_Restore_Memo;
        protected HtmlInputHidden Backup_Restore_File;
        protected LoadButton LoadButton1;
        protected Button bntbackups;
        protected Button btnrecover;
        protected Repeater rp_Item;
        private RM_System_IDAO system_idao = new RM_System_Dal();
        private string _UserName;

        protected void Page_Load(object sender, EventArgs e)
        {
            this._UserName = RequestSession.GetSessionUser().UserName.ToString();
            this.DataBindGrid();
        }

        private void DataBindGrid()
        {
            DataTable dt = this.system_idao.GetBackup_Restore_Log_List();
            ControlBindHelper.BindRepeaterList(dt, this.rp_Item);
        }

        protected void bntbackups_Click(object sender, EventArgs e)
        {
            string filename = DateTime.Now.ToString("yyyyMMddhhmmssff") + ".bak";
            string FilePath = ConfigHelper.GetAppSettings("BackupsDataPath");
            FileHelper.CreateDirectory(FilePath);
            bool IsOk = this.system_idao.DataBackups(FilePath + "//" + filename);
            if (IsOk)
            {
                string filesize = FileHelper.GetFileSize(FilePath + "//" + filename);
                this.system_idao.Add_Backup_Restore_Log("备份", filename, filesize, this._UserName, "RM_DB", this.Backup_Restore_Memo.Value);
                ShowMsgHelper.ShowScript("showTipsMsg('数据库备份成功！','2500','4');top.main.windowload();");
            }
            else
            {
                ShowMsgHelper.Alert_Error("备份失败！");
            }
        }

        protected void btnrecover_Click(object sender, EventArgs e)
        {
            string filename = this.Backup_Restore_File.Value;
            string FilePath = ConfigHelper.GetAppSettings("BackupsDataPath");
            FileHelper.CreateDirectory(FilePath);
            bool IsOk = this.system_idao.DataRestore(FilePath + "//" + filename);
            if (IsOk)
            {
                string filesize = FileHelper.GetFileSize(FilePath + "//" + filename);
                this.system_idao.Add_Backup_Restore_Log("恢复", filename, filesize, this._UserName, "RM_DB", this.Backup_Restore_Memo.Value);
                ShowMsgHelper.ShowScript("showTipsMsg('数据库恢复成功！','2500','4');top.main.windowload();");
            }
            else
            {
                ShowMsgHelper.Alert_Error("恢复失败！");
            }
        }

        protected void rp_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lblBackup_Restore_Type = e.Item.FindControl("lblBackup_Restore_Type") as Label;
                if (lblBackup_Restore_Type != null)
                {
                    string textlblBackup_Restore_Type = lblBackup_Restore_Type.Text;
                    textlblBackup_Restore_Type = textlblBackup_Restore_Type.Replace("恢复", "<span style='color:Blue'>恢复</span>");
                    lblBackup_Restore_Type.Text = textlblBackup_Restore_Type;
                }
            }
        }
    }
}