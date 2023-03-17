using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zhaoxi.ElasticSearchCore
{
    public class RequestContentDto
    {
        /// <summary>
        ///     分类ID
        /// </summary>
        public int? CategoryId { get; set; }

        /// <summary>
        ///     当前页ID
        /// </summary>
        public int CurrentIndex { get; set; } = 1;

        /// <summary>
        ///     每页显示数量
        /// </summary>
        public int PageSize { get; set; } = 40;

        /// <summary>
        ///     是否自营
        /// </summary>
        public int IsSelfSupport { get; set; } = 0;

        /// <summary>
        ///     最小价格
        /// </summary>
        public decimal? MinPrice { get; set; }

        /// <summary>
        ///     最大价格
        /// </summary>
        public decimal? MaxPrice { get; set; }

        /// <summary>
        ///     品牌
        /// </summary>
        public string Brand { get; set; }

        /// <summary>
        ///     供应商
        /// </summary>
        public string BaseType { get; set; }

        /// <summary>
        ///     关键词
        /// </summary>
        public string SearchKey { get; set; }

        /// <summary>
        ///     排序字段
        /// </summary>
        public string SortKey { get; set; }
        
    }
}
