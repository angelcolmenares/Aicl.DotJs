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
		private string template = @"Ext.define('{0}',{{ 
    extend: '{1}',
    alias : '{2}', 
    constructor: function(config){{
    	config= config|| {{}};
    	config.store= config.store|| '{3}',
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
    }},
    
    initComponent: function() {{
        
        this.columns={4};
 
        this.dockedItems=[{{
            xtype: 'toolbar',
            items: [{{
                text:'New',
                tooltip:'add new record',
                iconCls:'add',
                disabled:true,
                action: 'new'
            }},'-',{{
                text:'Delete',
                tooltip:'delete selected record',
                iconCls:'remove',
                disabled:true,
                action: 'delete'
            }}]		
        }}]
                
        this.callParent(arguments);
    }}
}});
";
		
		
		private Type type;
		
		public List (Type type)
		{
			this.type=type;
		}
		
		public string AppName{ get; set;}
		
		public string Define { get; set;}
		
		public string Extend{ get; set;}
		
		public string Alias { get; set;}
		
		public string Store { get; set;}
			
		public string FileName { get; set;}
		
		public string OutputDirectory { get; set;}
		
		public void Write(){
			
			if(string.IsNullOrEmpty(AppName)) AppName="App";
			
			if(string.IsNullOrEmpty(Define))
				Define= string.Format("{0}.view.{1}.List",AppName,type.Name.ToLower());
						
			if(string.IsNullOrEmpty(Extend))
				Extend= Config.List;
			
			if(string.IsNullOrEmpty(Alias))
				Alias= string.Format("widget.{0}list",type.Name.ToLower());
			
			if(string.IsNullOrEmpty(Store))
				Store= string.Format(type.Name);
			
			
			
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
			
			string sCols= cols.SerializeAndFormat().
				Replace("<FormatInt/>", Config.FormatInt).
			    Replace("<FormatNumber/>", Config.FormatNumber).
				Replace("True","true").
				Replace("False","false");
			
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
				tw.Write(string.Format(template, Define, Extend, Alias, Store, sCols));
				tw.Close();
			}
			
		}
	}
}
