using System;
using System.IO;
using System.Reflection;

namespace Aicl.DotJs
{
	public static class Config
	{
		private static string modelDirectory = "model";
		private static string storeDirectory = "store";
		private static string viewDirectory = "view";
		
		public static string Model =  "Ext.data.Model";
		public static string Store =  "Aicl.data.Store";
		public static string List =  "Ext.grid.Panel";
		public static string Form = "Ext.form.Panel";
		public static string AppDirectory = "app";
		public static string ModelDirectory{
			get{
				return Path.Combine(AppDirectory, modelDirectory);
			}
			set{
				modelDirectory= value;
			}
		
		}
		
		public static string StoreDirectory{
			get{
				return Path.Combine(AppDirectory, storeDirectory);
			}
			set{
				storeDirectory= value;
			}
		
		}
		
		public static string ViewDirectory{
			get{
				return Path.Combine(AppDirectory, viewDirectory);
			}
			set{
				viewDirectory= value;
			}
		
		}
		
		
		public static string FormatInt{
			get
			{
				return @"function(value, metadata, record, store){
           	if(value>=0){
            	return '<div class=""x-cell-positive"">'+Aicl.Util.formatInt(value)+'</div>';
        	}else{
            	return '<div class=""x-cell-negative"">'+Aicl.Util.formatInt(value)+'</div>';
        	}
        }";
			}
		}
		
		public static string FormatNumber{
			get
			{
				return @"function(value, metadata, record, store){
           	if(value>=0){
            	return '<div class=""x-cell-positive"">'+Aicl.Util.formatNumber(value)+'</div>';
        	}else{
            	return '<div class=""x-cell-negative"">'+Aicl.Util.formatNumber(value)+'</div>';
        	}
        }";
			}
		}
		
		
		public static string ListDockedItems{
			get
			{
				return @"[{
            xtype: 'toolbar',
            items: [{
                text:'Agregar',
                tooltip:'Agregar nuevo registro',
                iconCls:'add',
                disabled:true,
                action: 'new'
            },'-',{
                text:'Borrar',
                tooltip:'borrar registro seleccionado',
                iconCls:'remove',
                disabled:true,
                action: 'delete'
            }]		
        }]";

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
		
		public static void SetValue( this PropertyInfo propertyInfo, object objectWithProperties, object value )
		{
			propertyInfo.SetValue(objectWithProperties, 
			                      propertyInfo.PropertyType==typeof(string)?
			                      string.Format("'{0}'",value):
			                      value,
			                      new object[]{});
		}
		
				
	}
}

