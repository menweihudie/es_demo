//using Nest;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Zhaoxi.ElasticSearchCore
//{
//    /// <summary>
//    /// 多条件搜索例子
//    /// </summary>
//    public class WheresQuerDemo
//    {

//        public class WhereInfo
//        {
//            public int venId { get; set; }
//            public string venName { get; set; }

//        }

//        [ElasticsearchType(IdProperty = "priceID")]
//        public class VendorPriceInfo
//        {
//            public Int64 priceID { get; set; }
//            public int oldID { get; set; }
//            public int source { get; set; }
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        public static void Search()
//        {
//            ElasticClient client = NestDemos.CreateClient();

//            var result = client.Search<VendorPriceInfo>(CreateSearchRequest(new WhereInfo()));
//        }
//        /// <summary>
//        /// searchRequest 生成
//        /// </summary>
//        /// <param name="where"></param>
//        /// <returns></returns>
//        public static Func<SearchDescriptor<VendorPriceInfo>, ISearchRequest> CreateSearchRequest(WhereInfo where)
//        {
//            //querys
//            var mustQuerys = new List<Func<QueryContainerDescriptor<VendorPriceInfo>, QueryContainer>>();
//            if (where.venId > 0)
//            {
//                mustQuerys.Add(t => t.Term(f => f.vendorID, where.venId));
//            }

//            //filters
//            var mustFilters = new List<Func<QueryContainerDescriptor<VendorPriceInfo>, QueryContainer>>();
//            if (!string.IsNullOrEmpty(where.venName))
//            {
//                mustFilters.Add(t => t.MatchPhrase(f => f.Field(fd => fd.vendorName).Query(where.venName)));
//            }

//            Func<SearchDescriptor<VendorPriceInfo>, ISearchRequest> searchRequest = r =>
//                r.Query(q =>
//                            q.Bool(b =>
//                                        b.Must(mustQuerys)

//                                        .Filter(f =>
//                                                    f.Bool(fb =>
//                                                        fb.Must(mustFilters))
//                                                )
//                                   )
//                        );

//            return searchRequest;
//        }


//    }
//}
