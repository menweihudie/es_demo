using Nest;
using System.Collections.Generic;

namespace Zhaoxi.ElasticSearchCore.ES
{
    public class SortKeyConfig
    {
        public string Key { get; set; }

        public SortOrder SortOrder { get; set; }

        public List<string> SortKeyConfigs { get; } = new List<string>
        {
            //"cid4-asc",
            //"sortId-asc",
            //"createTime-desc"
             "firstName-asc",
            "lastName-asc"
        };
    }
}
