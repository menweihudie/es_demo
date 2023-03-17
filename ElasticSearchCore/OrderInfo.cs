using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zhaoxi.ElasticSearchCore
{
	
	class OrderInfo
	{
		public string Orderid { get; set; }

		public string Name { get; set; }

		public string Address { get; set; }

		public DateTime ActionTime { get; set; }

		public string Status { get; set; }
	}
}
