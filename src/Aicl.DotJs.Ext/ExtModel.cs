using System;
using System.Collections.Generic;

namespace Aicl.DotJs.Ext
{
	public class ExtModel
	{
		public ExtModel ()
		{
			fields= new List<ExtModelField>();
		}
		
		public string extend {get; set;}
		
		public string idProperty{ get; set;}
		
		public List<ExtModelField> fields { get; set;}
	}
}

