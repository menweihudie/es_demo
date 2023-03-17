using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zhaoxi.ElasticSearchCore.ES
{
    /// <summary>
    ///     ElasticsearchPage
    /// </summary>
    public class ElasticsearchPage<T> : PageEntity
    {
        public string Index { get; set; }

        public ElasticsearchPage(string index)
        {
            Index = index;
        }

        /// <summary>
        /// InitSearchRequest
        /// </summary>
        /// <returns></returns>
        public SearchRequest<T> InitSearchRequest()
        {
            return new SearchRequest<T>(Index)
            {
                From = (PageIndex - 1) * PageSize,
                Size = PageSize
            };
        }
    }
}
