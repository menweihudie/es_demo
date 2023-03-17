using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zhaoxi.ElasticSearchCore.ES
{
    public class PropertySearchNameAttribute : Attribute
    {
        public PropertySearchNameAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}
