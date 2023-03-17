using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zhaoxi.ElasticSearchCore
{
    public class InfoPrice
    {
        public int Id { get; set; }
        public int InfoPriceHeadId { get; set; }
        public int ProvinceId { get; set; }
        public int CityId { get; set; }
        public string MaterialName { get; set; }
        public string? SpecsName { get; set; }
        public string TaxExclusivePrice { get; set; }
        public string? TaxIncludingPrice { get; set; }
        public string? TaxRate { get; set; }
        public string? Remark { get; set; }
        public int FullYearMonth { get; set; }
        public int YearMonth { get; set; }
        public string FullDate { get; set; }
        //[Text(Name = "all", Analyzer = "ik_max_word")]
        public string All { get; set; }  //所有需要被搜索的信息
        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime? LastModifyTime { get; set; }
        public DateTime? CreateTime { get; set; }
        public string ProvinceName { get; set; }
        public string CityName { get; set; }
        public string? Unit { get; set; }
        public DateTime SyncTime { get; set; } = DateTime.Now;
    }
}
