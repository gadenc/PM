using System;
namespace DTcms.Model
{
    /// <summary>
    /// 下载附件
    /// </summary>
    [Serializable]
    public partial class download_attach
    {
        public download_attach()
        { }
        #region Model
        private int _id;
        private int _article_id = 0;
        private string _title;
        private string _file_path = "";
        private string _file_ext = "";
        private int _file_size = 0;
        private int _down_num = 0;
        private int _point = 0;
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
        /// 附件标题
        /// </summary>
        public string title
        {
            set { _title = value; }
            get { return _title; }
        }
        /// <summary>
        /// 文件路径
        /// </summary>
        public string file_path
        {
            set { _file_path = value; }
            get { return _file_path; }
        }
        /// <summary>
        /// 文件扩展名
        /// </summary>
        public string file_ext
        {
            set { _file_ext = value; }
            get { return _file_ext; }
        }
        /// <summary>
        /// 文件大小
        /// </summary>
        public int file_size
        {
            set { _file_size = value; }
            get { return _file_size; }
        }
        /// <summary>
        /// 下载次数
        /// </summary>
        public int down_num
        {
            set { _down_num = value; }
            get { return _down_num; }
        }
        /// <summary>
        /// 下载所需积分
        /// </summary>
        public int point
        {
            set { _point = value; }
            get { return _point; }
        }
        #endregion Model

    }
}