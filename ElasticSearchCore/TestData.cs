using Nest;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Zhaoxi.ElasticSearchCore
{
    public class TestData
    {
        #region 集群连接方式
        //var uris = new[]
        //        {
        //   	new Uri("http://localhost:9200"),
        //  new Uri("http://localhost:9201"),
        //  new Uri("http://localhost:9202"),
        //        };
        //var connectionPool = new SniffingConnectionPool(uris);
        //var settings = new ConnectionSettings(connectionPool)
        //	.DefaultIndex("people"); 
        //var client = new ElasticClient(settings);
        #endregion




        public void Search()
        {
            var settings = new ConnectionSettings(new Uri(Url.url))
              .DefaultIndex("person");
            var client = new ElasticClient(settings);

            #region InitData

            List<Person> list = new();
            int i = 0;

            Person person1 = new Person();
            person1.Id = i;
            person1.Age = 15 + i;
            person1.FirstName = "张三";
            person1.Sex = Convert.ToByte(person1.Age % 2);
            person1.LastName = "海尔";
            list.Add(person1);
            i++;

            Person person2 = new Person();
            person2.Id = i;
            person2.Age = 22;
            person2.FirstName = "李四";
            person2.Sex = 1;
            person2.LastName = "美的";
            list.Add(person2);
            i++;

            Person person3 = new Person();
            person3.Id = i;
            person3.Age = 15 + i;
            person3.FirstName = "李四";
            person3.Sex = 1;
            person3.LastName = "美的";
            list.Add(person3);
            i++;

            Person person4 = new Person();
            person2.Id = i;
            person2.Age = 22;
            person2.FirstName = "李四";
            person2.Sex = 1;
            person2.LastName = "海尔";
            list.Add(person2);
            i++;

            Person person5 = new Person();
            person5.Id = i;
            person5.Age = 15 + i;
            person5.FirstName = "李四";
            person5.Sex = Convert.ToByte(person5.Age % 2);
            person5.LastName = "沃尔沃";
            list.Add(person5);
            i++;

            Person person6 = new Person();
            person6.Id = i;
            person6.Age = 15 + i;
            person6.FirstName = "李四";
            person6.Sex = Convert.ToByte(person6.Age % 2);
            person6.LastName = "海尔";
            list.Add(person6);
            i++;

            Person person7 = new Person();
            person7.Id = i;
            person7.Age = 15 + i;
            person7.FirstName = "李四";
            person7.Sex = Convert.ToByte(person7.Age % 2);
            person7.LastName = "美的2";
            list.Add(person7);
            i++;

            Person person8 = new Person();
            person8.Id = i;
            person8.Age = 15 + i;
            person8.FirstName = "李四";
            person8.Sex = Convert.ToByte(person8.Age % 2);
            person8.LastName = "今天";
            list.Add(person8);
            i++;

            Person person9 = new Person();
            person9.Id = i;
            person9.Age = 15 + i;
            person9.FirstName = "李四";
            person9.Sex = Convert.ToByte(person9.Age % 2);
            person9.LastName = "沃尔沃";
            list.Add(person9);
            i++;


            #endregion


            client.IndexMany<Person>(list);
            // 数据刷盘延迟--默认1s 
            //        var searchResponse = client.Search<Person>(s => s
            //            .From(0)
            //.Size(10)
            //    .Query(q => q
            //                 .Match(m => m
            //                    .Field(f => f.FirstName)
            //                    .Query("张三")
            //                 )
            //            )
            //        );
            //        var people = searchResponse.Documents;
            //        Console.WriteLine("查询结果");
            //        foreach (var item in people)
            //        {
            //            Console.WriteLine($"id:{item.Id},firstname:{item.FirstName},lastname:{item.LastName}");
            //        }


            //Console.WriteLine("**********");
            //// select * from tabel where name="1" and age>1

            ////{ "query" : { "bool" : { "must": [{ "match_all" : { } }]} },"from" : 0,"size" : 1}
            ////{"query" : {"bool" : {"must" : [{"match" : {"name" : {"query" : "1", "type" : "phrase"}}},{"range" : {"age" : {"gt" : "1"}}}]}},"from" : 0,"size" : 1}

            //var ss = client.Search<Person>(s => s.Query(
            //      m => m.Bool(
            //          m => m.Must(
            //              x => x.Match(m => m.Field(f => f.FirstName).Query("张")),
            //              mm => mm.Range(xx => xx.Field(f => f.Age).GreaterThan(20))
            //                     )
            //               )

            //      )
            //).Documents;


            //var searchResponse = client.Search<Person>(s => s
            //	.From(0)
            //	.Size(10)
            //	.Query(q => q
            //		 .Match(m => m
            //			.Field(f => f.FirstName)
            //			.Query("Martijn1")
            //		 )
            //	)
            //);

            var b = new string[] { "海尔", "美的" };
            var where = new WhereInfo() { venId = 1, venName = "张李" };
            var highlightConfig = new HighlightConfig<Content>
            {
                Tag = "i",
                HighlightConfigExpression = new List<Expression<Func<Content, object>>>
                {
                    x=>x.Title
                }
            };

            var search = CreateSearchRequest(where);

            var rs = client.Search<Person>(search).Documents;

            Console.WriteLine("查询结果");
            foreach (var item in rs)
            {
                Console.WriteLine($"id:{item.Id},firstname:{item.FirstName},lastname:{item.LastName}");
            }
        }

        /// <summary>
        /// 多条件搜索例子
        /// </summary>

        public class HighlightConfig<T>
        {
            public string Tag { get; set; }

            public List<Expression<Func<T, object>>> HighlightConfigExpression { get; set; }
        }

        public class WhereInfo
        {
            public int venId { get; set; }
            public string venName { get; set; }
            public string[] brands { get; set; }

        }
        /// <summary>
        /// searchRequest 生成
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public static Func<SearchDescriptor<Person>, ISearchRequest> CreateSearchRequest(WhereInfo where)
        {
            //querys
            var mustQuerys = new List<Func<QueryContainerDescriptor<Person>, QueryContainer>>();
            if (where.venId > 0)
            {
                mustQuerys.Add(t => t.Term(f => f.Sex, where.venId));
            }

            //mustQuerys.Add(t => t.Terms(m => m.Field(fd => fd.LastName).Terms(new[] { "美的", "海尔" })));
            //filters
            var mustFilters = new List<Func<QueryContainerDescriptor<Person>, QueryContainer>>();
            if (!string.IsNullOrEmpty(where.venName))
            {
                mustQuerys.Add(t => t.Match(f => f.Field(fd => fd.FirstName).Query(where.venName)));
            }

            //should 条件
            var shouldQuerys = new List<Func<QueryContainerDescriptor<Person>, QueryContainer>>();
            if (where.brands != null && where.brands.Length > 0)
            {
                foreach (var item in where.brands)
                {
                    shouldQuerys.Add(mt => mt.MatchPhrase(t => t.Field(f => f.LastName).Query(item)));
                    //shouldQuerys.Add(mt => mt.Field(fd => fd.LastName).Terms(new[] { "美的", "海尔" }));
                }
                //shouldQuerys.Add(t => t.Terms(m => m.Field(fd => fd.LastName).Terms(new[] { "美的", "海尔" })));

            }


            Func<SearchDescriptor<Person>, ISearchRequest> searchRequest = r =>
            r.Query(q =>
                        q.Bool(b =>
                                    b.Must(mustQuerys)
                                    .Filter(f => f.Bool(fb => fb.Must(mustFilters)))

                               )
                    );//排序

            
            var settings = new ConnectionSettings(new Uri(Url.url))
             .DefaultIndex("person");
            var client = new ElasticClient(settings);
            

            var result2 = client.Search<Person>(s => s
                         .Index("person")
                         .Query(q => q.Bool(b => b.Must(mustQuerys))
                          && 
                         q.Bool(b => b.Should(shouldQuerys)))
                         .Size(100)
                         .From(0)
                     //.Sort(s => s.Ascending(x => x.LastName))
                     ).Documents;

            var result3 = client.Search<Person>(s => s
                      .Index("person")
                      .Query(q => q.Bool(b => b.Must(mustQuerys))
                      && (
                      q.MatchPhrase(t => t
                             .Field(f => f.LastName).Query("美的"))
                              ||
                 q.MatchPhrase(t => t
                             .Field(f => f.LastName).Query("海尔"))
                      ))
                      .Size(100)
                      .From(0)
                  //.Sort(s => s.Ascending(x => x.LastName))
                  ).Documents;


            return searchRequest;
        }
        public void SearchA()
        {
            var settings = new ConnectionSettings(new Uri(Url.url))
               .DefaultIndex("contentTest");
            var client = new ElasticClient(settings);

            var list = InitData();
            try
            {
                client.IndexMany<Content>(list);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


        }
        private static List<Content> InitData()
        {
            return new List<Content>
            {
                new Content
                {
                    Id = 1,
                    Key="123852",
                    Title="康尔 KingCamp 碳纤维登山杖 碳素手杖 户外徒步 轻 外锁KA4665",
                    ImgUrl= "https://m.360buyimg.com/babel/jfs/t7699/154/4235664957/46882/ab720071/5a082a42N0f424779.jpg",
                    Code="2495868",
                    UnitPrice= 199.0M,
                    BaseType= "JdBulkindustrial",
                    ClassificationName= "登山攀岩",
                    ClassificationCode= ",1,1862,1917,1930,",
                    BrandFirstLetters= "K",
                    Brand= "康尔（KingCamp）",
                    BelongMemberName= "成都京东世纪贸易有限公司",
                    IsSelfSupport= 0,
                    ResourceStatus= 1,
                    BrandGroupBy= "康尔（KingCamp）&K",
                    ClassificationGroupBy= "登山攀岩&,1,1862,1917,1930,"
                },
                new Content
                {
                    Id=2,
                   Key="123754",
                   Title="赛乐（Zealwood）银离子吸湿排汗防脚臭跑步骑行功能袜TREK LT系列17017 Z084M 墨绿色一双装",
                   ImgUrl="https://m.360buyimg.com/babel/jfs/t12136/106/885881193/94278/adda7271/5a164d21Ncde2321a.jpg",
                   Code="5899056",
                   UnitPrice=49.0M,
                   BaseType="JdBulkindustrial",
                   ClassificationName="户外袜",
                   ClassificationCode=",1,1862,1896,1899,",
                   BrandFirstLetters="Z",
                   Brand="ZEAL WOOD",
                   BelongMemberName="成都京东世纪贸易有限公司",
                   IsSelfSupport=0,
                   ResourceStatus=1,
                   BrandGroupBy="ZEAL WOOD&Z",
                   ClassificationGroupBy="户外袜&,1,1862,1896,1899,"
                },
                new Content
                {
                    Id=3,
                   Key= "123753",
                   Title= "赛乐（Zealwood）椰碳纤维登山徒步吸湿排汗功能袜ACTIVE系列17012 Z081M 黑蓝色一双装",
                   ImgUrl= "https://m.360buyimg.com/babel/jfs/t12802/305/908277128/85492/a064e452/5a164b94Nfd9daf87.jpg",
                   Code= "5815969",
                   UnitPrice= 39.0M,
                   BaseType= "JdBulkindustrial",
                   ClassificationName= "户外袜",
                   ClassificationCode= ",1,1862,1896,1899,",
                   BrandFirstLetters= "Z",
                   Brand= "ZEAL WOOD",
                   BelongMemberName= "成都京东世纪贸易有限公司",
                   IsSelfSupport= 0,
                   ResourceStatus= 1,
                   BrandGroupBy= "ZEAL WOOD&Z",
                   ClassificationGroupBy= "户外袜&,1,1862,1896,1899,"
                },
                new Content
                {
                    Id=4,
                    Key="123795",
                    Title="红色营地 户外铝合金登山杖 便携徒步健走杖 拐杖 直把 红色",
                    ImgUrl="https://m.360buyimg.com/babel/jfs/t3631/98/575860737/76998/8e120f1e/580da7b5N156b74e4.jpg",
                    Code="1084302",
                    UnitPrice=29.0M,
                    BaseType="JdBulkindustrial",
                    ClassificationName="登山攀岩",
                    ClassificationCode=",1,1862,1917,1930,",
                    BrandFirstLetters="G",
                    Brand="红色营地",
                    BelongMemberName="成都京东世纪贸易有限公司",
                    IsSelfSupport=0,
                    ResourceStatus=1,
                    BrandGroupBy="红色营地&G",
                    ClassificationGroupBy="登山攀岩&,1,1862,1917,1930,"
                },
                new Content
                {
                    Id=5,
                   Key="123797",
                   Title="红色营地 户外铝合金登山杖 便携徒步健走杖 拐杖 T型 红色",
                   ImgUrl="https://m.360buyimg.com/babel/jfs/t20161/327/430422201/70069/d06bc66d/5b0e090cN7e077a21.jpg",
                   Code="1084306",
                   UnitPrice=33.0M,
                   BaseType="JdBulkindustrial",
                   ClassificationName="登山攀岩",
                   ClassificationCode=",1,1862,1917,1930,",
                   BrandFirstLetters="G",
                   Brand="红色营地",
                   BelongMemberName="成都京东世纪贸易有限公司",
                   IsSelfSupport=0,
                   ResourceStatus=1,
                   BrandGroupBy="红色营地&G",
                   ClassificationGroupBy="登山攀岩&,1,1862,1917,1930,"
                }
            };
        }

        /// ik分词结果对象
        /// </summary>
        public class ik
        {
            public List<tokens> tokens { get; set; }
        }
        public class tokens
        {
            public string token { get; set; }
            public int start_offset { get; set; }
            public int end_offset { get; set; }
            public string type { get; set; }
            public int position { get; set; }
        }


        public class Content
        {
            public int Id { get; set; }
            /// <summary>
            /// 键值
            /// </summary>
            /// Text 代表字符串类型  Name 为名称   Analyzer 分词方式（ik_max_word采用Ik）
            /// Norms 无需筛选字段配置为不参与评分 Similarity 相似性算法 （eg:LMDirichlet）
            /// DocValues 设置是否对字段进行排序或聚合  ignore_malformed 忽略格式异常 Coerce 强制格式化
            /// 更多参考连接 https://www.elastic.co/guide/en/elasticsearch/reference/current/mapping-params.html
            //[Text(Name = "key", Analyzer = "ik_max_word")]
            public string Key { get; set; }

            /// <summary>
            /// 标题
            /// </summary>
            /// , Analyzer = "ik_max_word"
            //[Text(Name = "title", Analyzer = "ik_max_word")]
            //[PropertySearchName("title")]
            public string Title { get; set; }

            /// <summary>
            /// 图片连接
            /// </summary>
            //[Text(Name = "img_url", Norms = false)]
            //[PropertySearchName("img_url")]
            public string ImgUrl { get; set; }

            /// <summary>
            /// 资源代码
            /// </summary>
            //[Text(Name = "code", Analyzer = "ik_max_word")]
            //[PropertySearchName("code")]
            public string Code { get; set; }

            /// <summary>
            /// 单价
            /// </summary>
            //[Number(Name = "unit_price", DocValues = true, IgnoreMalformed = true, Coerce = true)]
            //[PropertySearchName("unit_price")]
            public decimal UnitPrice { get; set; }

            /// <summary>
            /// 基础类型
            /// </summary>
            //[Text(Name = "base_type", Analyzer = "ik_max_word")]
            //[PropertySearchName("base_type")]
            public string BaseType { get; set; }

            /// <summary>
            /// 分类名称
            /// </summary>
            //[Text(Name = "classification_name", Analyzer = "ik_max_word")]
            //[PropertySearchName("classification_name")]
            public string ClassificationName { get; set; }

            /// <summary>
            /// 分类代码
            /// </summary>
            //[Text(Name = "classification_code", Analyzer = "ik_max_word")]
            //[PropertySearchName("classification_code")]
            public string ClassificationCode { get; set; }

            /// <summary>
            /// 品牌首字母
            /// </summary>
            //[Text(Name = "brand_first_letters")]
            //[PropertySearchName("brand_first_letters")]
            public string BrandFirstLetters { get; set; }

            /// <summary>
            /// 品牌
            /// </summary>
            //[Text(Name = "brand", Analyzer = "ik_max_word")]
            //[PropertySearchName("brand")]
            public string Brand { get; set; }

            /// <summary>
            /// 属于用户
            /// </summary>
            //[Text(Name = "belong_member_name", Analyzer = "ik_max_word")]
            //[PropertySearchName("belong_member_name")]
            public string BelongMemberName { get; set; }

            /// <summary>
            /// 是否自营
            /// </summary>
            //[Text(Name = "is_self_support")]
            //[PropertySearchName("is_self_support")]
            public int IsSelfSupport { get; set; }

            /// <summary>
            /// 资源状态
            /// </summary>
            //[Text(Name = "resource_status")]
            //[PropertySearchName("resource_status")]
            public int ResourceStatus { get; set; }

            /// <summary>
            /// brand_group_by
            /// </summary>
            //[Keyword(Name = "brand_group_by")]
            //[PropertySearchName("brand_group_by")]
            public string BrandGroupBy { get; set; }

            /// <summary>
            /// ClassificationGroupBy
            /// </summary>
            //[Keyword(Name = "classification_group_by")]
            //[PropertySearchName("classification_group_by")]
            public string ClassificationGroupBy { get; set; }
        }
        public class PropertySearchNameAttribute : Attribute
        {
            public PropertySearchNameAttribute(string name)
            {
                Name = name;
            }

            public string Name { get; set; }
        }
    }
}
