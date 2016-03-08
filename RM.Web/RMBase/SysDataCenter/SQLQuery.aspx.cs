using RM.Busines;
using RM.Common.DotNetUI;
using RM.Web.App_Code;
using System;
using System.Data;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace RM.Web.RMBase.SysDataCenter
{
    public class SQLQuery : PageBase
    {
        public string _Table_Name;
        protected HtmlForm form1;
        protected HtmlSelect Execute_Type;
        protected LinkButton ExeOuter;
        protected HtmlTextArea txtSql;
        protected GridView Grid;
        protected void Page_Load(object sender, EventArgs e)
        {
            this._Table_Name = base.Server.UrlDecode(base.Request["Table_Name"]);
            if (!base.IsPostBack)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("消息提示", Type.GetType("System.String"));
                DataRow row = dt.NewRow();
                row["消息提示"] = "未执行SQL命令!";
                dt.Rows.Add(row);
                ControlBindHelper.BindGridViewList(dt, this.Grid);
            }
        }
        protected void ExeOuter_Click(object sender, EventArgs e)
        {
            StringBuilder strSql = new StringBuilder(this.txtSql.Value);
            if (this.Execute_Type.Value == "1")
            {
                DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(strSql);
                if (dt != null)
                {
                    if (dt.Rows.Count != 0)
                    {
                        ControlBindHelper.BindGridViewList(dt, this.Grid);
                    }
                    else
                    {
                        dt = new DataTable();
                        dt.Columns.Add("消息提示", Type.GetType("System.String"));
                        DataRow row = dt.NewRow();
                        row["消息提示"] = "没有找到您要的相关数据";
                        dt.Rows.Add(row);
                        ControlBindHelper.BindGridViewList(dt, this.Grid);
                    }
                }
                else
                {
                    dt = new DataTable();
                    dt.Columns.Add("消息提示", Type.GetType("System.String"));
                    DataRow row = dt.NewRow();
                    row["消息提示"] = "执行SQL命令,有错误!";
                    dt.Rows.Add(row);
                    ControlBindHelper.BindGridViewList(dt, this.Grid);
                }
            }
            else
            {
                if (this.Execute_Type.Value == "2")
                {
                    int i = DataFactory.SqlDataBase().ExecuteBySql(strSql);
                    DataTable dt = new DataTable();
                    dt.Columns.Add("消息提示", Type.GetType("System.String"));
                    DataRow row = dt.NewRow();
                    if (i > 0)
                    {
                        row["消息提示"] = "执行成功!";
                        dt.Rows.Add(row);
                        ControlBindHelper.BindGridViewList(dt, this.Grid);
                    }
                    else
                    {
                        if (i == 0)
                        {
                            row["消息提示"] = "0 行受影响!";
                            dt.Rows.Add(row);
                            ControlBindHelper.BindGridViewList(dt, this.Grid);
                        }
                        else
                        {
                            row["消息提示"] = "执行SQL命令,有错误!";
                            dt.Rows.Add(row);
                            ControlBindHelper.BindGridViewList(dt, this.Grid);
                        }
                    }
                }
            }
        }
    }
}
