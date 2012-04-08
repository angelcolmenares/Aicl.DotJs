using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using Aicl.DotJs.Ext;
using ServiceStack.Text;
using ServiceStack.OrmLite;

namespace Aicl.DotJs
{
	public class List
	{
		private Type type;
		
		public List (Type type)
		{
			this.type=type;
		}
		
		public string Define { get; set;}
		
		public string Extend{ get; set;}
		
		public string Alias { get; set;}
			
		public string FileName { get; set;}
		
		public string OutputDirectory { get; set;}
		
		public void Write(){
			
			if(string.IsNullOrEmpty(Define))
				Define= string.Format("view.{0}.list",type.Name);
						
			if(string.IsNullOrEmpty(Extend))
				Extend= Config.List;
			
			if(string.IsNullOrEmpty(Alias))
				Alias= string.Format("widget.{0}list",type.Name.ToLower());
			
			List<ListColumn> cols = new List<ListColumn>();
			
			foreach(PropertyInfo pi in type.GetProperties()){
				
				if(pi.Name== OrmLiteConfig.IdField) continue;
				
				ListColumn col = new ListColumn();
				col.text=string.Format("'{0}'", pi.Name);
				col.dataIndex= string.Format("'{0}'", pi.Name);
				col.sortable= true;
				if (cols.Count==0)  col.flex= 1;
				
				if(pi.PropertyType== typeof(DateTime) || pi.PropertyType== typeof(DateTime?))
				{
					col.renderer="Ext.util.Format.dateRenderer('d.m.Y')";
				}
				else if(pi.PropertyType== typeof(Int16) || pi.PropertyType== typeof(Int16?)
				        || pi.PropertyType== typeof(Int32) || pi.PropertyType== typeof(Int32?)
				        || pi.PropertyType== typeof(Int64) || pi.PropertyType== typeof(Int64?))
				{
					col.renderer="<FormatInt/>";
				}
				else if(pi.PropertyType== typeof(decimal) || pi.PropertyType== typeof(decimal?)
				        || pi.PropertyType== typeof(double) || pi.PropertyType== typeof(double?)
				        || pi.PropertyType== typeof(float) || pi.PropertyType== typeof(float?))
				{
					col.renderer="<FormatNumber/>";
				}
				
				cols.Add(col);
			}
			
			string list = string.Format(@"Ext.define('{0}',{{ 
    extend: '{1}',
    alias : '{2}',
    
    constructor: function(config){{
    	config= config|| {{}};
    	config.store= config.store||'{3}',
        config.frame = config.frame==undefined? false:config.frame;
		config.selType = config.selType || 'rowmodel';
    	config.height = config.height||350;
    	config.width = config.width || 600;
    	config.viewConfig = config.viewConfig || {{
        	stripeRows: true
	    }};
        config.margin=config.margin|| '2 2 2 2';	
    	if (arguments.length==0 )
    		this.callParent([config]);
    	else
    		this.callParent(arguments); 
    }},", Define, Extend, Alias, type.Name);

			
			
			string initComponent= @"
	initComponent:function(){
        this.columns="+cols.SerializeAndFormat()+@";
        this.dockedItems="+Config.ListDockedItems+@";
		this.callParent(arguments);
	}
})";
			
			if(string.IsNullOrEmpty(OutputDirectory))
			{
				OutputDirectory= Path.Combine( Path.Combine(Directory.GetCurrentDirectory(),Config.ViewDirectory), type.Name.ToLower());
				
				if (!Directory.Exists(OutputDirectory))
					Directory.CreateDirectory(OutputDirectory);
			}
			
			if(string.IsNullOrEmpty(FileName))
				FileName= "List.js";
			
			
			using (TextWriter tw = new StreamWriter(Path.Combine(OutputDirectory, FileName)))
			{
				tw.Write((list+initComponent).
			                  Replace("<FormatInt/>", Config.FormatInt).
			                  Replace("<FormatNumber/>", Config.FormatNumber).
			                  Replace("True", "true"));
				tw.Close();
			}
			
			
		}
	}
}
