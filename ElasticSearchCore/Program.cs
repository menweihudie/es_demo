using Elasticsearch.Net;
using ElasticSearchCore;
using Nest;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Zhaoxi.ElasticSearchCore.ES;
using static Zhaoxi.ElasticSearchCore.TestData;

namespace Zhaoxi.ElasticSearchCore
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //Install-Package NEST 

                {

                    // Test.Show();
                    //Console.ReadKey();

                    #region Deviceinfo
                    var settings = new ConnectionSettings(new Uri("http://192.10.223.64:9200"))
                    .DefaultIndex("infoprice");
                    var client = new ElasticClient(settings);

                    //var response0 = new SearchResponse<object>();

                    var ss = client.SearchTemplate<InfoPrice>(m => m.AllIndices());

                    //List<Deviceinfo> dirverinfos = new List<Deviceinfo>();
                    //for (int i = 0; i < 50; i++)
                    //{
                    //    dirverinfos.Add(new Deviceinfo()
                    //    {
                    //        Type = 1,
                    //        Status = 1,
                    //        ActionTime = DateTime.UtcNow
                    //    }); ;

                    //}
                    //for (int i = 0; i < 50; i++)
                    //{
                    //    dirverinfos.Add(new Deviceinfo()
                    //    {
                    //        Type = 2,
                    //        Status = 1,
                    //        ActionTime = DateTime.UtcNow
                    //    }); ;

                    //}
                    //client.IndexMany<Deviceinfo>(dirverinfos);

                    //var dt = SendByWebRequest.GetDataBySql("select * from device2 where type=2");
                    //Console.WriteLine(dt.Rows.Count);
                    #endregion

                    //var strList = SendByWebRequest.Post("select * from device2 where address like '%蜀%'");
                    var clayindex2 = SendByWebRequest.Post("select * from infoprice where all= '砌筑水泥 M32.5GB3183' and provinceId=340000 and cityId=340100");

                    var clayindex3 = SendByWebRequest.Post("select * from infoprice where provinceId=340000 and cityId=340100");

                    var clayindex = SendByWebRequest.Post("select * from infoprice where materialName = matchPhraseQuery('圆钢','300')");

                    #region clayindex --text  
                    // 创建的map在准备数据里面

                    var list1 = SendByWebRequest.GetDataBySql("select * from clayindex where  address like '%蜀%' ");
                    //Console.WriteLine(list1.Rows.Count);
                    //var clayindex = SendByWebRequest.Post("select * from clayindex where address like '%蜀%'");
                    //Console.WriteLine(clayindex);

                    //到这边，基本操作，sql，sdl== 推荐大家是sql ===
                    #endregion

                    //var list1 = SendByWebRequest.GetDataBySql("select * from spu where");

                    //               TestData testData = new TestData();
                    ////testData.IndexMany();
                    ////Console.WriteLine("ok");
                    //testData.Search();
                    //Console.WriteLine("ok");

                    //TestData test = new TestData();
                    //test.Search();

                    //var dto = new RequestContentDto { SearchKey = "李张",Brand= "海尔,美的" };
                    //var list = Test.SearchAB(1, 100, dto);
                }

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }


        }
    }

    public static class Test
    {
        public static List<Person> SearchAB(int pageIndex, int pageSize, RequestContentDto requestContentDto)
        {
            var elasticsearchPage = new ElasticsearchPage<Person>("person")
            {
                PageIndex = pageIndex,
                PageSize = pageSize
            };

            #region terms 分组

            var terms = new List<IFieldTerms>();
            var classificationGroupBy = "searchKey_classification";
            var brandGroupBy = "searchKey_brand";

            #endregion

            var searchRequest = elasticsearchPage.InitSearchRequest();
            var predicateList = new List<IPredicate>();
            ////分类ID
            //if (requestContentDto.CategoryId != null)
            //    predicateList.Add(Predicates.Field<Person>(x => x.ClassificationCode, ExpressOperator.Like,
            //        requestContentDto.CategoryId));
            //else
            //    terms.Add(Predicates.FieldTerms<Person>(x => x.ClassificationGroupBy, classificationGroupBy, 200));

            //品牌
            //if (string.IsNullOrWhiteSpace(requestContentDto.Brand))
            //    terms.Add(Predicates.FieldTerms<Person>(x => x.BrandGroupBy, brandGroupBy, 200));
            //供应商名称
            //if (!string.IsNullOrWhiteSpace(requestContentDto.BaseType))
            //    predicateList.Add(Predicates.Field<Person>(x => x.BaseType, ExpressOperator.Like,
            //        requestContentDto.BaseType));
            ////是否自营
            if (requestContentDto.IsSelfSupport == 1)
                predicateList.Add(Predicates.Field<Person>(x => x.Sex, ExpressOperator.Eq,
                    requestContentDto.IsSelfSupport));

            //关键词
            if (!string.IsNullOrWhiteSpace(requestContentDto.SearchKey))
                predicateList.Add(Predicates.Field<Person>(x => x.FirstName, ExpressOperator.Like,
                    requestContentDto.SearchKey));

            //if (!string.IsNullOrWhiteSpace(requestContentDto.Brand))
            //{
            //    var array = requestContentDto.Brand.Split(',').ToList();
            //    if (array.Any())
            //    {
            //        predicateList
            //            .AddRange(array.Select
            //                (item => Predicates.Field<Person>(x => x.LastName, ExpressOperator.Like, item)));
            //    }
            //}

            //规整排序
            var sortConfig = SortOrderRule(requestContentDto.SortKey);
            //var sorts = new List<ISort>
            //{
            //    Predicates.Sort<Person>(sortConfig.Key, sortConfig.SortOrder)
            //};

            var predicate = Predicates.Group(GroupOperator.And, predicateList.ToArray());
            ////构建或查询
            var predicateListOr = new List<IPredicate>();
            if (!string.IsNullOrWhiteSpace(requestContentDto.Brand))
            {
                var array = requestContentDto.Brand.Split(',').ToList();
                if (array.Any())
                {
                    predicateListOr
                        .AddRange(array.Select
                            (item => Predicates.Field<Person>(x => x.LastName, ExpressOperator.Eq, item)));
                }
            }

            var predicateOr = Predicates.Group(GroupOperator.Or, predicateListOr.ToArray());

            var predicatecCombination = new List<IPredicate> { predicate, predicateOr };
            var pgCombination = Predicates.Group(GroupOperator.And, predicatecCombination.ToArray());

            searchRequest.InitQueryContainer(pgCombination)
            /*.InitSort(sorts)*/;
            //.InitHighlight(requestContentDto.HighlightConfigEntity)
            //.InitGroupBy(terms);

            var settings = new ConnectionSettings(new Uri(Url.url))
             .DefaultIndex("person");
            var client = new ElasticClient(settings);

           

            var response =
                client.Search<Person>(searchRequest).Documents;

            return response.ToList();
            //#region terms 分组赋值

            //var classificationResponses = requestContentDto.CategoryId != null
            //    ? null
            //    : data.Aggregations.Terms(classificationGroupBy).Buckets
            //        .Select(x => new ClassificationResponse
            //        {
            //            Key = x.Key.ToString(),
            //            DocCount = x.DocCount
            //        }).ToList();

            //var brandResponses = !string.IsNullOrWhiteSpace(requestContentDto.Brand)
            //    ? null
            //    : data.Aggregations.Terms(brandGroupBy).Buckets
            //        .Select(x => new BrandResponse
            //        {
            //            Key = x.Key.ToString(),
            //            DocCount = x.DocCount
            //        }).ToList();

            //#endregion

            //初始化

            //#region 高亮

            //var titlePropertySearchName = (PropertySearchNameAttribute)
            //    LoadAttributeHelper.LoadAttributeByType<Content, PropertySearchNameAttribute>(x => x.Title);

            //var list = data.Hits.Select(c => new Content
            //{
            //    Key = c.Source.Key,
            //    //Title = (string)c.Highlights.Highlight(c.Source.Title, titlePropertySearchName.Name),
            //    Title = c.Source.Title,
            //    ImgUrl = c.Source.ImgUrl,
            //    BaseType = c.Source.BaseType,
            //    BelongMemberName = c.Source.BelongMemberName,
            //    Brand = c.Source.Brand,
            //    Code = c.Source.Code,
            //    BrandFirstLetters = c.Source.BrandFirstLetters,
            //    ClassificationName = c.Source.ClassificationName,
            //    ResourceStatus = c.Source.ResourceStatus,
            //    BrandGroupBy = c.Source.BrandGroupBy,
            //    ClassificationGroupBy = c.Source.ClassificationGroupBy,
            //    ClassificationCode = c.Source.ClassificationCode,
            //    IsSelfSupport = c.Source.IsSelfSupport,
            //    UnitPrice = c.Source.UnitPrice
            //}).ToList();

            //#endregion

            //var contentResponse = new ContentResponse
            //{
            //    Records = (int)data.Total,
            //    PageIndex = elasticsearchPage.PageIndex,
            //    PageSize = elasticsearchPage.PageSize,
            //    Contents = list,
            //    BrandResponses = brandResponses,
            //    ClassificationResponses = classificationResponses
            //};
            //return contentResponse;
        }

        /// <summary>
        ///     排序规则验证
        /// </summary>
        /// <param name="sortKey"></param>
        private static SortKeyConfig SortOrderRule(string sortKey)
        {
            //初始化对象
            var sortKeyConfig = new SortKeyConfig { Key = "_score", SortOrder = SortOrder.Descending };
            //设置默认值
            if (string.IsNullOrWhiteSpace(sortKey) || !sortKeyConfig.SortKeyConfigs.Contains(sortKey))
                return sortKeyConfig;

            //转换为小写
            sortKey = sortKey.ToLower();

            var orderArray = BreakUpOptions(sortKey, '-');
            if (orderArray == null || orderArray.Length != 2)
                return sortKeyConfig;

            var key = orderArray.FirstOrDefault();
            var sortOrder = orderArray.LastOrDefault();

            //赋值
            sortKeyConfig.Key = key;
            sortKeyConfig.SortOrder = sortOrder == "desc" ? SortOrder.Descending : SortOrder.Ascending;
            return sortKeyConfig;
        }

        /// <summary>
        ///     拆分数组
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string[] BreakUpOptions(this string str, char key)
        {
            var strArray = str.Split(new[] { key }, StringSplitOptions.RemoveEmptyEntries);
            return strArray;
        }
    }


    public class SearchProductDto 
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public string KeyWord { get; set; }
        public long Cid2 { get; set; }

        public long Cid3 { get; set; }

        public List<string> Brands { get; set; }

        public List<string> Props { get; set; }

    }
}
