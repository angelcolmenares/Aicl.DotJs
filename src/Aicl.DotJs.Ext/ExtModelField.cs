using System;

namespace Aicl.DotJs.Ext
{
	public class ExtModelField
	{
		private object _convert;
		
		public ExtModelField ()
		{
			
		}
		[Name]
		public string name { get; set;}
		
		[PropertyType]
		public string type { get; set;}
		
		[Convert]
		public object convert { 
			get{
				if (_convert==null && !string.IsNullOrEmpty(type) && type.Contains("date"))
					_convert="function(v){return Aicl.Util.convertToDate(v);}";
				return _convert;
			}
			set{
				_convert=value;
			}
		}
			
		
	}
}

