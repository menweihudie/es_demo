using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zhaoxi.ElasticSearchCore.ES
{
    public class ContentResponse : PageEntity
    {
        public List<TestData.Content> Contents { get; set; }

        //public List<BrandResponse> BrandResponses { get; set; }

        //public List<ClassificationResponse> ClassificationResponses { get; set; }
    }
}
