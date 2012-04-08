using System;
namespace Aicl.DotJs.Ext
{
	public class ExtStore
	{
		private object _constructor;
		public ExtStore (string storeId)
		{
			_constructor= string.Format( @"function(config){{config=config||{{}};config.storeId=config.storeId||'{0}';if(arguments.length==0) this.callParent([config]);else this.callParent(arguments);}}",storeId);
		}
		
		public string extend {get; set;}
		
		public string model {get; set;}
		
		public object constructor
		{
			get { return _constructor;}
			set { _constructor=value;}
		}
		
	}
}
