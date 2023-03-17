namespace Zhaoxi.ElasticSearchCore.ES
{
    /// <summary>
    /// 分页类型
    /// </summary>
    public class PageEntity
    {
        /// <summary>
        ///     每页行数
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        ///     当前页
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        ///     总记录数
        /// </summary>
        public int Records { get; set; }

        /// <summary>
        ///     总页数
        /// </summary>
        public int Total
        {
            get
            {
                if (Records > 0)
                    return Records % PageSize == 0 ? Records / PageSize : Records / PageSize + 1;

                return 0;
            }
        }

        /// <summary>
        ///     排序列
        /// </summary>
        public string Sidx { get; set; }

        /// <summary>
        ///     排序类型
        /// </summary>
        public string Sord { get; set; }
    }
}
