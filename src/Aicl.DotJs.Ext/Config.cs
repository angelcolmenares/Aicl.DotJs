using System;
using System.IO;

namespace Aicl.DotJs
{
	public static class Config
	{
		private static string modelDirectory = "model";
		
		public static string Model =  "Ext.data.Model";
		public static string AppDirectory = "app";
		public static string ModelDirectory{
			get{
				return Path.Combine(AppDirectory, modelDirectory);
			}
			set{
				modelDirectory= value;
			}
		
		}
		
		public static string GetJsType(Type type){
			
			if(type==typeof(string))
				return "string";
			
			if(type==typeof(Int16) || type==typeof(Int32) || type==typeof(Int64) 
			   || type==typeof(Int16?) || type==typeof(Int32?) || type==typeof(Int64?) )
				return "int";
			
			if(type==typeof(float) ||  type==typeof(float?) )
				return "float";
			
			if(type==typeof(decimal) ||  type==typeof(decimal?) ||
			   type==typeof(double) ||  type==typeof(double?) )
				return "number";
			
			if(type==typeof(DateTime) ||  type==typeof(DateTime?) )
				return "date";
			
			if(type==typeof(bool) ||  type==typeof(bool?)) 
				return "boolean";
			
			if(type==typeof(Guid) ||  type==typeof(Guid?)) 
				return "string";
			
			return "auto";
			
			
			
			
		}
				
	}
}

