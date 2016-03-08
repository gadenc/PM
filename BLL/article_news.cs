using System;
using System.Data;
using System.Collections.Generic;
using DTcms.Common;

namespace DTcms.BLL
{
    /// <summary>
    /// 文章模型
    /// </summary>
    public partial class article
    {
        #region  Method
        /// <summary>
        /// 修改文章副表一列数据
        /// </summary>
        public void UpdateNewsField(int id, string strValue)
        {
            dal.UpdateNewsField(id, strValue);
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Model.article_news model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Model.article_news model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Model.article_news GetNewsModel(int id)
        {
            return dal.GetNewsModel(id);
        }

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetNewsList(int Top, string strWhere, string filedOrder)
        {
            return dal.GetNewsList(Top, strWhere, filedOrder);
        }

        /// <summary>
        /// 获得查询分页数据
        /// </summary>
        public DataSet GetNewsList(int pageSize, int pageIndex, string strWhere, string filedOrder, out int recordCount)
        {
            return dal.GetNewsList(pageSize, pageIndex, strWhere, filedOrder, out recordCount);
        }

        #endregion  Method
    }
}