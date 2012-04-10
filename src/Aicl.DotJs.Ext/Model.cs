using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using ServiceStack.OrmLite;
using ServiceStack.Common.Utils;
using ServiceStack.Common.Extensions;
using ServiceStack.Text;

namespace Aicl.DotJs
{
	public class Model
	{
		private Type type; 
		
		private Dictionary <string,object> config= new Dictionary<string, object>();
			
		
		public Model (Type type)
		{
			this.type=type;
		}
		
		public string AppName { get; set;}
		
		public string Define { get; set;}
		
		public string Extend { get; set;}
		
		public string Members { get; set;}
		
		public string IdProperty { get; set;}
		
		public string FileName { get; set;}
		
		public string OutputDirectory { get; set;}
		
		public void Write<T,TF>() 
			where T: new ()
			where TF: new ()
		{
			if(string.IsNullOrEmpty(AppName)) AppName="App";
			
			if(string.IsNullOrEmpty( Define ))
				Define= string.Format("{0}.model.{1}",AppName, type.Name);
						
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
					namePI.SetValue(field,pi.Name);
				
				if(typePI!=null)
				typePI.SetValue(field, Config.GetJsType(pi.PropertyType));
				
				if(convertPI!=null){
					var val = convertPI.GetValue(field, new object[]{} );
					if(val!=null)
					{
						string convert= val.ToString().Replace("{","<<>>").Replace("}",">><<");
						convertPI.SetValue(field, convert );
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
				   	pi.SetValue(objectWithProperties, v);
			}
					
				
			string r= objectWithProperties.SerializeAndFormat().Replace("<<>>","{").Replace(">><<","}");
			r= string.Format( "Ext.define('{0}',{1});",Define, r);
			
			
			if(string.IsNullOrEmpty(FileName))
				FileName= type.Name+".js";
			
			if(string.IsNullOrEmpty(OutputDirectory))
			{
				OutputDirectory= Path.Combine(Directory.GetCurrentDirectory(),Config.ModelDirectory);
				
				if (!Directory.Exists(OutputDirectory))
					Directory.CreateDirectory(OutputDirectory);
			}
			
			using (TextWriter tw = new StreamWriter(Path.Combine(OutputDirectory, FileName)))
			{
				tw.Write(r);
				tw.Close();
			}
				
		}
				
		
		
	}
}
