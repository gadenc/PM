using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using DBUtility;
using DTcms.Common;

namespace DTcms.DAL
{
    /// <summary>
    /// 下载附件
    /// </summary>
    public partial class download_attach
    {
        public download_attach()
        { }
        #region  Method
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from dt_download_attach");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;

            return DbHelperSQL2.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 修改一列数据
        /// </summary>
        public void UpdateField(int id, string strValue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update dt_download_attach set " + strValue);
            strSql.Append(" where id=" + id);
            DbHelperSQL2.ExecuteSql(strSql.ToString());
        }

        /// <summary>
        /// 查找不存在的文件并删除已删除的附件及数据
        /// </summary>
        public void DeleteList(SqlConnection conn, SqlTransaction trans, List<Model.download_attach> models, int article_id)
        {
            StringBuilder idList = new StringBuilder();
            if (models != null)
            {
                foreach (Model.download_attach modelt in models)
                {
                    if (modelt.id > 0)
                    {
                        idList.Append(modelt.id + ",");
                    }
                }
            }
            string id_list = Utils.DelLastChar(idList.ToString(), ",");
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id,file_path from dt_download_attach where article_id=" + article_id);
            if (!string.IsNullOrEmpty(id_list))
            {
                strSql.Append(" and id not in(" + id_list + ")");
            }
            DataSet ds = DbHelperSQL2.Query(conn, trans, strSql.ToString());
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int rows = DbHelperSQL2.ExecuteSql(conn, trans, "delete from dt_download_attach where id=" + dr["id"].ToString()); //删除数据库
                if (rows > 0)
                {
                    Utils.DeleteFile(dr["file_path"].ToString()); //删除文件
                }
            }
        }

        /// <summary>
        /// 删除附件文件
        /// </summary>
        public void DeleteFile(List<Model.download_attach> models)
        {
            if (models != null)
            {
                foreach (Model.download_attach modelt in models)
                {
                    Utils.DeleteFile(modelt.file_path);
                }
            }
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Model.download_attach GetModel(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 id,article_id,title,file_path,file_ext,file_size,down_num,point from dt_download_attach ");
            strSql.Append(" where id=@id");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;

            Model.download_attach model = new Model.download_attach();
            DataSet ds = DbHelperSQL2.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["id"] != null && ds.Tables[0].Rows[0]["id"].ToString() != "")
                {
                    model.id = int.Parse(ds.Tables[0].Rows[0]["id"].ToString());
                }
                if (ds.Tables[0].Rows[0]["article_id"] != null && ds.Tables[0].Rows[0]["article_id"].ToString() != "")
                {
                    model.article_id = int.Parse(ds.Tables[0].Rows[0]["article_id"].ToString());
                }
                if (ds.Tables[0].Rows[0]["title"] != null && ds.Tables[0].Rows[0]["title"].ToString() != "")
                {
                    model.title = ds.Tables[0].Rows[0]["title"].ToString();
                }
                if (ds.Tables[0].Rows[0]["file_path"] != null && ds.Tables[0].Rows[0]["file_path"].ToString() != "")
                {
                    model.file_path = ds.Tables[0].Rows[0]["file_path"].ToString();
                }
                if (ds.Tables[0].Rows[0]["file_ext"] != null && ds.Tables[0].Rows[0]["file_ext"].ToString() != "")
                {
                    model.file_ext = ds.Tables[0].Rows[0]["file_ext"].ToString();
                }
                if (ds.Tables[0].Rows[0]["file_size"] != null && ds.Tables[0].Rows[0]["file_size"].ToString() != "")
                {
                    model.file_size = int.Parse(ds.Tables[0].Rows[0]["file_size"].ToString());
                }
                if (ds.Tables[0].Rows[0]["down_num"] != null && ds.Tables[0].Rows[0]["down_num"].ToString() != "")
                {
                    model.down_num = int.Parse(ds.Tables[0].Rows[0]["down_num"].ToString());
                }
                if (ds.Tables[0].Rows[0]["point"] != null && ds.Tables[0].Rows[0]["point"].ToString() != "")
                {
                    model.point = int.Parse(ds.Tables[0].Rows[0]["point"].ToString());
                }
                return model;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Model.download_attach> GetList(int article_id)
        {
            List<Model.download_attach> modelList = new List<Model.download_attach>();

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id,article_id,title,file_path,file_ext,file_size,down_num,point ");
            strSql.Append(" FROM dt_download_attach ");
            strSql.Append(" where article_id=" + article_id);
            DataTable dt = DbHelperSQL2.Query(strSql.ToString()).Tables[0];

            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                Model.download_attach model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new Model.download_attach();
                    if (dt.Rows[n]["id"] != null && dt.Rows[n]["id"].ToString() != "")
                    {
                        model.id = int.Parse(dt.Rows[n]["id"].ToString());
                    }
                    if (dt.Rows[n]["article_id"] != null && dt.Rows[n]["article_id"].ToString() != "")
                    {
                        model.article_id = int.Parse(dt.Rows[n]["article_id"].ToString());
                    }
                    if (dt.Rows[n]["title"] != null && dt.Rows[n]["title"].ToString() != "")
                    {
                        model.title = dt.Rows[n]["title"].ToString();
                    }
                    if (dt.Rows[n]["file_path"] != null && dt.Rows[n]["file_path"].ToString() != "")
                    {
                        model.file_path = dt.Rows[n]["file_path"].ToString();
                    }
                    if (dt.Rows[n]["file_ext"] != null && dt.Rows[n]["file_ext"].ToString() != "")
                    {
                        model.file_ext = dt.Rows[n]["file_ext"].ToString();
                    }
                    if (dt.Rows[n]["file_size"] != null && dt.Rows[n]["file_size"].ToString() != "")
                    {
                        model.file_size = int.Parse(dt.Rows[n]["file_size"].ToString());
                    }
                    if (dt.Rows[n]["down_num"] != null && dt.Rows[n]["down_num"].ToString() != "")
                    {
                        model.down_num = int.Parse(dt.Rows[n]["down_num"].ToString());
                    }
                    if (dt.Rows[n]["point"] != null && dt.Rows[n]["point"].ToString() != "")
                    {
                        model.point = int.Parse(dt.Rows[n]["point"].ToString());
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        #endregion  Method
    }
}