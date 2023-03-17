using Nest;
using Zhaoxi.ElasticSearchCore.ES;

namespace Zhaoxi.ElasticSearchCore
{
    public class Person
    {
        public int Id { get; set; }

        [Text(Name = "firstName", Analyzer = "ik_max_word")]
        //[PropertySearchName("firstName")]
        public string FirstName { get; set; }

        //[PropertySearchName("lastName")]
        //[Keyword(Name = "lastName")]
        public string LastName { get; set; }

        //[PropertySearchName("age")]
        public int Age { get; set; }

        //[PropertySearchName("sex")]
        public byte Sex { get; set; }
    }

    //public class Person
    //{
    //    public int Id { get; set; }

    //    //[Text(Analyzer = "ik_max_word")]
    //    public string FirstName { get; set; }

    //    public string LastName { get; set; }

    //    public int Age { get; set; }

    //    public byte Sex { get; set; }
    //}
}
