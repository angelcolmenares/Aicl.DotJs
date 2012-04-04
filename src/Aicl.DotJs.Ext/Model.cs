using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using ServiceStack.OrmLite;
using ServiceStack.Common.Utils;
using ServiceStack.Common.Extensions;
using ServiceStack.Text;

namespace Aicl.DotJs.Ext
{
	public class Model
	{
		private Type type; 
		
		private Dictionary <string,object> config= new Dictionary<string, object>();
			
		
		public Model (Type type)
		{
			this.type=type;
		}
		
		public string Define { get; set;}
		
		public string Extend { get; set;}
		
		public string Members { get; set;}
		
		public string IdProperty { get; set;}
		
		public string FileName { get; set;}
		
		public string OutputDirectory { get; set;}
		
		public T Write<T,TF>() 
			where T: new ()
			where TF: new ()
		{
			if(string.IsNullOrEmpty( Define ))
				Define= type.FullName;
						
			if(string.IsNullOrEmpty(Extend))
				Extend= Config.Model;
			
			config.Add("extend",Extend);
			
			if(string.IsNullOrEmpty(IdProperty))
			{
				PropertyInfo pi= ReflectionUtils.GetPropertyInfo(type, OrmLiteConfig.IdField);
				if( pi!=null )
				{
					IdProperty = pi.Name;
				}
			}
			
			config.Add("idProperty",IdProperty);
			
			if(string.IsNullOrEmpty(FileName))
				FileName= type.Name+".js";
			
			if(string.IsNullOrEmpty(OutputDirectory))
			{
				OutputDirectory= Path.Combine(Directory.GetCurrentDirectory(),Config.ModelDirectory);
				
				if (!Directory.Exists(OutputDirectory))
					Directory.CreateDirectory(OutputDirectory);
			}	
			
			
			Type tft= typeof(TF);
			PropertyInfo namePI=null;
			PropertyInfo typePI=null;
			PropertyInfo convertPI=null;
			
			NameAttribute nameAttrib= null; 
			PropertyTypeAttribute typeAttrib= null; 
			ConvertAttribute convertAttrib= null; 
			foreach(PropertyInfo pi in  tft.GetProperties()){
				
				if(nameAttrib==null){
					nameAttrib = pi.FirstAttribute<NameAttribute>();
					if(nameAttrib!=null)
						namePI= pi;
				}
				
				if( typeAttrib==null){
					typeAttrib = pi.FirstAttribute<PropertyTypeAttribute>();
					if(typeAttrib!=null)
						typePI= pi;
				}
				if( convertAttrib==null){
					convertAttrib = pi.FirstAttribute<ConvertAttribute>();
					if(convertAttrib!=null)
						convertPI= pi;
				}
				
			}
			
			List<TF> fields = new List<TF>();
			
			var pis =type.GetProperties();
					
			foreach(PropertyInfo pi in pis)
			{
				TF field = new TF();
				
				if(namePI!=null)				
					namePI.SetValue(field,  string.Format("'{0}'",pi.Name), new object[]{});
				
				if(typePI!=null)
				typePI.SetValue(field, string.Format("'{0}'", Config.GetJsType(pi.PropertyType)), new object[]{});
				
				if(convertPI!=null){
					var val = convertPI.GetValue(field, new object[]{} );
					if(val!=null)
					{
						string convert= val.ToString().Replace("{","<<<<").Replace("}",">>>>");
						convertPI.SetValue(field, convert , new object[]{});
					}
				}
				fields.Add(field);
			}
			
			config.Add("fields", fields);
			
			T objectWithProperties= new T();
			Type t = typeof(T);
			foreach( PropertyInfo pi in t.GetProperties())
			{
				object v ;
				if(config.TryGetValue(pi.Name,out v))
				   	pi.SetValue(objectWithProperties, pi.PropertyType==typeof(string)?
					           string.Format("'{0}'",v):
					           v,
					           new object[]{});
			}
					
			
			
			
			var s = JsonSerializer.SerializeToString(objectWithProperties);
			
			Console.WriteLine(s);
			
			s = objectWithProperties.SerializeAndFormat(); //JsonSerializer.SerializeToString(objectWithProperties).SerializeAndFormat();
			
			Console.WriteLine(s);
			
			s = objectWithProperties.ToJson();
			
			Console.WriteLine(s);
			
			Console.WriteLine( s.SerializeAndFormat() );
			
			s = objectWithProperties.ToJsv();
			
			Console.WriteLine(s);
			
			Console.WriteLine( s.SerializeAndFormat().Replace("<<<<","{").Replace(">>>>","}"));
			//Console.WriteLine( s.SerializeAndFormat().Replace("<<<<","{").Replace(">>>>","}").Replace("\t\t\t\t","") );
			//Console.WriteLine( s.SerializeAndFormat().Replace("<<<<","{").Replace(">>>>","}").Replace("\t\t\t\t","") );
			//Console.WriteLine( s.SerializeAndFormat().Replace("<<<<","{").Replace(">>>>","}").Replace("\t\t","\t").Replace("\t\t\t","\t\t") );
			
			
			
			return objectWithProperties;
			
				
		}
				
		
		
	}
}
/*
 * Ext.define('AD.model.Company',{
	extend: 'Ext.data.Model',
	idProperty: 'Id',
    fields:[
    	{name: 'Id', type:'int'},
    	{name: 'Name', type:'string'},
    	{name: 'Turnover', type:'number'},
    	{name: 'Started' ,  type :'date', 
    			convert: function(v){ return  Aicl.Util.convertToDate(v)}},  				
    	{name: 'Employees' ,  type:'int'},
    	{name: 'CreatedDate' ,  type :'date', 
    	   		convert: function(v){ return  Aicl.Util.convertToDate(v)}},
    	{name: 'Guid' ,  type:'string'}
    ]
    
});

	
*/