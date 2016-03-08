using System;

namespace DTcms.Model
{
    /// <summary>
    /// 图片相册
    /// </summary>
    [Serializable]
    public partial class article_albums
    {
        public article_albums()
        { }
        #region Model
        private int _id;
        private int _article_id;
        private string _big_img;
        private string _small_img;
        private string _remark = "";
        /// <summary>
        /// 自增ID
        /// </summary>
        public int id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 主表ID
        /// </summary>
        public int article_id
        {
            set { _article_id = value; }
            get { return _article_id; }
        }
        /// <summary>
        /// 大图
        /// </summary>
        public string big_img
        {
            set { _big_img = value; }
            get { return _big_img; }
        }
        /// <summary>
        /// 小图
        /// </summary>
        public string small_img
        {
            set { _small_img = value; }
            get { return _small_img; }
        }
        /// <summary>
        /// 描述
        /// </summary>
        public string remark
        {
            set { _remark = value; }
            get { return _remark; }
        }
        #endregion Model

    }
}