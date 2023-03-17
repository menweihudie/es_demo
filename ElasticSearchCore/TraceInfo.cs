using System;
using System.Collections.Generic;
using System.Text;

namespace Zhaoxi.ElasticSearchCore
{
	public class TraceInfo
	{

		#region MyRegion
		//TestData testData = new TestData();
		//{
		//	while (1 == 1)
		//	{
		//		Console.WriteLine("请输入发生的消息内容");
		//		string text = Console.ReadLine();
		//		testData.Trace(text);
		//	}
		//	//testData.IndexMany();
		//} 
		#endregion
		/// <summary>
		/// 唯一的请求的标识
		/// </summary>
		public string RpcID { get; set; }

		public DateTime Time { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string Content { get; set; }



		public string Message { get; set; }



	}
}
