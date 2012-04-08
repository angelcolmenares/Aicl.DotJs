using System;
namespace Aicl.DotJs.Ext
{
	public class ListColumn
	{
		public ListColumn ()
		{
		}
		
		public string text{get;set;}
		
		public string  dataIndex {get; set;}
		
		public int? flex {get; set;}
		
		public bool sortable  {get; set;}
		
		public object renderer { get; set;}
		
	}
}
