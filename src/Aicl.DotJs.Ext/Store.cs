using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using ServiceStack.Text;
using Aicl.DotJs.Ext;

namespace Aicl.DotJs
{
	public class Store
	{
		private Type type; 
		
		private Dictionary <string,object> config= new Dictionary<string, object>();
		
		
		public Store (Type type)
		{
			this.type=type;
		}
		
		public string Define{get; set;}
		
		public string Extend{get; set;}
		
		public string Model{get; set;}
		
		public string StoreId { get; set;}
		
		public string FileName { get; set;}
		
		public string OutputDirectory { get; set;}
		
		public void Write()
		{
		
			if(string.IsNullOrEmpty( Define ))
				Define= string.Format("store.{0}",type.Name);
						
			if(string.IsNullOrEmpty(Extend))
				Extend= Config.Store;
			
			config.Add("extend",Extend);
			
			if(string.IsNullOrEmpty( Model ))
				Model = string.Format("model.{0}",type.Name);			
			
			config.Add("model",Model);
			
			if(string.IsNullOrEmpty( StoreId ))
				StoreId= type.Name;
			
			var store = new ExtStore(StoreId);
			store.constructor= store.constructor.ToString().
				Replace("{","<<>>").Replace("}",">><<").
				Replace("[","<*>").Replace("]",">*<");
		
			Type t = typeof(ExtStore);
			
			foreach(KeyValuePair<string, object> kv in config)
			{
				PropertyInfo pi=  t.GetProperty(kv.Key);
				if(pi !=null)
				{
					pi.SetValue(store, kv.Value);
				}
			}
			
			if(string.IsNullOrEmpty(FileName))
				FileName= type.Name+".js";
			
			if(string.IsNullOrEmpty(OutputDirectory))
			{
				OutputDirectory= Path.Combine(Directory.GetCurrentDirectory(),Config.StoreDirectory);
				
				if (!Directory.Exists(OutputDirectory))
					Directory.CreateDirectory(OutputDirectory);
			}
			
			string r= store.SerializeAndFormat().
				Replace("<<>>","{").Replace(">><<","}").
				Replace("<*>","[").Replace(">*<","]");
			r= string.Format( "Ext.define('{0}',{1});",Define, r);
			
			
			using (TextWriter tw = new StreamWriter(Path.Combine(OutputDirectory, FileName)))
			{
				tw.Write(r);
				tw.Close();
			}
			
			
		}
	}
}


