using System;
using System.Data;
using System.Collections.Generic;
using DTcms.Model;
namespace DTcms.BLL
{
    /// <summary>
    /// 类别业务类
    /// </summary>
    public partial class category
    {
        private readonly DTcms.DAL.category dal = new DTcms.DAL.category();
        public category()
        { }
        #region  Method
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int id)
        {
            return dal.Exists(id);
        }

        /// <summary>
        /// 返回类别名称
        /// </summary>
        public string GetTitle(int id)
        {
            return dal.GetTitle(id);
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(DTcms.Model.category model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 修改一列数据
        /// </summary>
        public void UpdateField(int id, string strValue)
        {
            dal.UpdateField(id, strValue);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(DTcms.Model.category model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int id)
        {
            dal.Delete(id);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public DTcms.Model.category GetModel(int id)
        {
            return dal.GetModel(id);
        }

         /// <summary>
        /// 取得指定类别下的列表
        /// </summary>
        /// <param name="parent_id">父ID</param>
        /// <param name="channel_id">频道ID</param>
        /// <returns></returns>
        public DataTable GetChildList(int parent_id, int channel_id)
        {
            return dal.GetChildList(parent_id, channel_id);
        }

        /// <summary>
        /// 取得所有类别列表
        /// </summary>
        /// <param name="parent_id">父ID</param>
        /// <param name="channel_id">频道ID</param>
        /// <returns></returns>
        public DataTable GetList(int parent_id, int channel_id)
        {
            return dal.GetList(parent_id, channel_id);
        }

        #region 扩展方法================================
        /// <summary>
        /// 取得父节点的ID
        /// </summary>
        public int GetParentId(int id)
        {
            return dal.GetParentId(id);
        }
        /// <summary>
        /// 取得某个频道下父节点下的id
        /// </summary>
        /// <param name="parent_id"></param>
        /// <param name="channel_id"></param>
        /// <returns></returns>
        public DataTable GetTopList(int parent_id, int channel_id) 
        {
            DataTable dt = dal.GetTopList(parent_id, channel_id);
            return dt;
        }
        #endregion

        #endregion  Method
    }
}