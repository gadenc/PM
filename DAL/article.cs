using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Data.SqlClient;
using DTcms.Common;
using DBUtility;

namespace DTcms.DAL
{
    /// <summary>
    /// 文章模型
    /// </summary>
    public partial class article
    {
        public article()
        { }
        #region  Method
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from dt_article");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;

            return DbHelperSQL2.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 返回信息标题
        /// </summary>
        public string GetTitle(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 title from dt_article");
            strSql.Append(" where id=" + id);
            string title = Convert.ToString(DbHelperSQL2.GetSingle(strSql.ToString()));
            if (string.IsNullOrEmpty(title))
            {
                return "";
            }
            return title;
        }

        /// <summary>
        /// 修改一列数据
        /// </summary>
        public void UpdateField(int id, string strValue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update dt_article set " + strValue);
            strSql.Append(" where id=" + id);
            DbHelperSQL2.ExecuteSql(strSql.ToString());
        }

        /// <summary>
        /// 删除一条数据，及子表所有相关数据
        /// </summary>
        public bool Delete(int id)
        {
            //取得相册MODEL
            List<Model.article_albums> albumsList = new article_albums().GetList(id);
            //取得附件MODEL
           // List<Model.download_attach> attachList = new download_attach().GetList(id);

            List<CommandInfo> sqllist = new List<CommandInfo>();
            //删除文章模型数据
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from dt_article_news ");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;
            CommandInfo cmd = new CommandInfo(strSql.ToString(), parameters);
            sqllist.Add(cmd);

            ////删除下载模型数据
            //StringBuilder strSql2 = new StringBuilder();
            //strSql2.Append("delete from dt_article_download ");
            //strSql2.Append(" where id=@id ");
            //SqlParameter[] parameters2 = {
            //        new SqlParameter("@id", SqlDbType.Int,4)};
            //parameters2[0].Value = id;
            //cmd = new CommandInfo(strSql2.ToString(), parameters2);
            //sqllist.Add(cmd);

            ////删除商品模型数据
            //StringBuilder strSql3 = new StringBuilder();
            //strSql3.Append("delete from dt_article_goods ");
            //strSql3.Append(" where id=@id ");
            //SqlParameter[] parameters3 = {
            //        new SqlParameter("@id", SqlDbType.Int,4)};
            //parameters3[0].Value = id;
            //cmd = new CommandInfo(strSql3.ToString(), parameters3);
            //sqllist.Add(cmd);

            ////删除内容模型数据
            //StringBuilder strSql4 = new StringBuilder();
            //strSql4.Append("delete from dt_article_content ");
            //strSql4.Append(" where id=@id ");
            //SqlParameter[] parameters4 = {
            //        new SqlParameter("@id", SqlDbType.Int,4)};
            //parameters4[0].Value = id;
            //cmd = new CommandInfo(strSql4.ToString(), parameters4);
            //sqllist.Add(cmd);

            ////删除顶和踩
            //StringBuilder strSql5 = new StringBuilder();
            //strSql5.Append("delete from dt_article_diggs ");
            //strSql5.Append(" where id=@id ");
            //SqlParameter[] parameters5 = {
            //        new SqlParameter("@id", SqlDbType.Int,4)};
            //parameters5[0].Value = id;
            //cmd = new CommandInfo(strSql5.ToString(), parameters5);
            //sqllist.Add(cmd);

            ////删除商品价格
            //StringBuilder strSql6 = new StringBuilder();
            //strSql6.Append("delete from dt_goods_group_price ");
            //strSql6.Append(" where article_id=@article_id ");
            //SqlParameter[] parameters6 = {
            //        new SqlParameter("@article_id", SqlDbType.Int,4)};
            //parameters6[0].Value = id;
            //cmd = new CommandInfo(strSql6.ToString(), parameters6);
            //sqllist.Add(cmd);

            ////删除下载的附件
            //StringBuilder strSql7 = new StringBuilder();
            //strSql7.Append("delete from dt_download_attach ");
            //strSql7.Append(" where article_id=@article_id ");
            //SqlParameter[] parameters7 = {
            //        new SqlParameter("@article_id", SqlDbType.Int,4)};
            //parameters7[0].Value = id;
            //cmd = new CommandInfo(strSql7.ToString(), parameters7);
            //sqllist.Add(cmd);

            //删除图片相册
            StringBuilder strSql8 = new StringBuilder();
            strSql8.Append("delete from dt_article_albums ");
            strSql8.Append(" where article_id=@article_id ");
            SqlParameter[] parameters8 = {
					new SqlParameter("@article_id", SqlDbType.Int,4)};
            parameters8[0].Value = id;
            cmd = new CommandInfo(strSql8.ToString(), parameters8);
            sqllist.Add(cmd);

            ////删除扩展属性
            //StringBuilder strSql9 = new StringBuilder();
            //strSql9.Append("delete from dt_attribute_value ");
            //strSql9.Append(" where article_id=@article_id ");
            //SqlParameter[] parameters9 = {
            //        new SqlParameter("@article_id", SqlDbType.Int,4)};
            //parameters9[0].Value = id;
            //cmd = new CommandInfo(strSql9.ToString(), parameters9);
            //sqllist.Add(cmd);

            ////删除评论
            //StringBuilder strSql10 = new StringBuilder();
            //strSql10.Append("delete from dt_article_comment ");
            //strSql10.Append(" where article_id=@article_id ");
            //SqlParameter[] parameters10 = {
            //        new SqlParameter("@article_id", SqlDbType.Int,4)};
            //parameters10[0].Value = id;
            //cmd = new CommandInfo(strSql10.ToString(), parameters10);
            //sqllist.Add(cmd);

            //删除主表信息
            StringBuilder strSql11 = new StringBuilder();
            strSql11.Append("delete from dt_article ");
            strSql11.Append(" where id=@id ");
            SqlParameter[] parameters11 = {
					new SqlParameter("@id", SqlDbType.Int,4)};
            parameters11[0].Value = id;
            cmd = new CommandInfo(strSql11.ToString(), parameters11);
            sqllist.Add(cmd);

            int rowsAffected = DbHelperSQL2.ExecuteSqlTran(sqllist);
            if (rowsAffected > 0)
            {
                new article_albums().DeleteFile(albumsList); //删除图片
                //new download_attach().DeleteFile(attachList); //删除附件
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion  Method
    }
}