using SQLDMO;
using System.Data;
using System.Data.SqlClient;

namespace RM.DataBase.DataBase.Common
{
    public class SqlServerBackup
    {
        private string database;
        private string server;
        private string uid;
        private string pwd;

        public string Database
        {
            get
            {
                return this.database;
            }
            set
            {
                this.database = value;
            }
        }

        public string Server
        {
            get
            {
                return this.server;
            }
            set
            {
                this.server = value;
            }
        }

        public string Pwd
        {
            get
            {
                return this.pwd;
            }
            set
            {
                this.pwd = value;
            }
        }

        public string Uid
        {
            get
            {
                return this.uid;
            }
            set
            {
                this.uid = value;
            }
        }

        public bool DbBackup(string url)
        {
            Backup oBackup = new BackupClass();
            SQLServer oSQLServer = new SQLServerClass();
            bool result;
            try
            {
                oSQLServer.LoginSecure = false;
                oSQLServer.Connect(this.server, this.uid, this.pwd);
                oBackup.Action = SQLDMO_BACKUP_TYPE.SQLDMOBackup_Database;
                oBackup.Database = this.database;
                oBackup.Files = url;
                oBackup.BackupSetName = this.database;
                oBackup.BackupSetDescription = "数据库备份";
                oBackup.Initialize = true;
                oBackup.SQLBackup(oSQLServer);
                result = true;
            }
            catch
            {
                result = false;
            }
            finally
            {
                oSQLServer.DisConnect();
            }
            return result;
        }

        public bool DbRestore(string url)
        {
            bool result;
            if (!this.exepro())
            {
                result = false;
            }
            else
            {
                Restore oRestore = new RestoreClass();
                SQLServer oSQLServer = new SQLServerClass();
                try
                {
                    oSQLServer.LoginSecure = false;
                    oSQLServer.Connect(this.server, this.uid, this.pwd);
                    oRestore.Action = SQLDMO_RESTORE_TYPE.SQLDMORestore_Database;
                    oRestore.Database = this.database;
                    oRestore.Files = url;
                    oRestore.FileNumber = 1;
                    oRestore.ReplaceDatabase = true;
                    oRestore.SQLRestore(oSQLServer);
                    result = true;
                }
                catch
                {
                    result = false;
                }
                finally
                {
                    oSQLServer.DisConnect();
                }
            }
            return result;
        }

        private bool exepro()
        {
            SqlConnection conn = new SqlConnection(string.Concat(new string[]
			{
				"server=",
				this.server,
				";uid=",
				this.uid,
				";pwd=",
				this.pwd,
				";database=master"
			}));
            SqlCommand cmd = new SqlCommand("killspid", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@dbname", this.database);
            bool result;
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                result = true;
            }
            catch
            {
                result = false;
            }
            finally
            {
                conn.Close();
            }
            return result;
        }
    }
}