using System;
using System.IO;
using Aicl.DotJs.Ext;
using ServiceStack.Text;
using ServiceStack.OrmLite;

namespace Aicl.DotJs
{
	public class Controller
	{
		private string template=@"Ext.define('{0}',{{
	extend: '{1}',
    stores: [
        '{2}' 
    ],  
    models: [
    	'{2}'
    ],
    views: [
    	'{3}.List',
    	'{3}.Form'
    ],
    refs:[
    	{{ref: '{3}List',    	 selector: '{3}list' }},
    	{{ref: '{3}DeleteButton', selector: '{3}list button[action=delete]' 	}},
    	{{ref: '{3}NewButton',    selector: '{3}list button[action=new]' 	}},
    	{{ref: '{3}Form',    	 selector: '{3}form' }}, 
    	{{ref: '{3}SaveButton', 	 selector: '{3}form button[action=save]' }}
    ],

    init: function(application) {{
    	    	
        this.control({{
            '{3}list': {{ 
                selectionchange: function( sm,  selections,  eOpts){{
                	if (selections.length) {{
                		this.get{2}Form().getForm().loadRecord(selections[0]);
                		this.get{2}DeleteButton().setDisabled(false);
        				this.get{2}SaveButton().setText('Update');	          				
        			}}
        			else{{
        				this.reset{2}Form();
        			}}
                }}
            }},
            
            '{3}list button[action=delete]': {{
                click: function(button, event, options){{
                	var grid = this.get{2}List();
                	var record = grid.getSelectionModel().getSelection()[0];
        			grid.getStore().remove(record);
                }}
            }},
            
            '{3}list button[action=new]': {{
            	click:function(button, event, options){{
            		this.reset{2}Form();
            	}}
            }},
            
            '{3}form button[action=save]':{{
            	click:function(button, event, options){{
            		var record = this.get{2}Form().getForm().getFieldValues(true);
            		var store =this.get{2}Store();
        			if (record.Id ){{
           				var sr = store.getById(parseInt( record.Id) );
						sr.beginEdit();
						for( var r in record){{
							sr.set(r, record[r])
						}}
						sr.endEdit(); 
        			}}
        			else{{
          				var nr = Ext.ModelManager.create(record, this.get{2}Model().getName() );
						store.add(nr);
						this.get{2}List().getSelectionModel().doSingleSelect(nr,false);
        			}}
            	}}
            }}
            
        }});
    }},
    
    onLaunch: function(application){{
    	this.get{2}Store().load();
    	this.get{2}NewButton().setDisabled(false);
    	this.get{2}Form().setFocus();
    }},
    
    reset{2}Form: function(){{
        this.get{2}DeleteButton().setDisabled(true);
        this.get{2}Form().getForm().reset();            
        this.get{2}SaveButton().setText('Add');
	}}
	
}});
";
		private Type type;
		
		public Controller(Type type)
		{
			this.type=type;
		}
		
		public string AppName{ get; set;}
		
		public string Define { get; set;}
		
		public string Extend{ get; set;}
			
		public string FileName { get; set;}
		
		public string OutputDirectory { get; set;}
		
		public void Write(){
			
			if(string.IsNullOrEmpty(AppName)) AppName="App";
			
			if(string.IsNullOrEmpty(Define))
				Define= string.Format("{0}.controller.{1}",AppName,type.Name);
						
			if(string.IsNullOrEmpty(Extend))
				Extend= Config.Controller;
			
			
			if(string.IsNullOrEmpty(OutputDirectory))
			{
				OutputDirectory= Path.Combine(Directory.GetCurrentDirectory(),Config.ControllerDirectory);
				
				if (!Directory.Exists(OutputDirectory))
					Directory.CreateDirectory(OutputDirectory);
			}
			
			if(string.IsNullOrEmpty(FileName))
				FileName= type.Name+ ".js";
			
			
			using (TextWriter tw = new StreamWriter(Path.Combine(OutputDirectory, FileName)))
			{
				tw.Write(string.Format(template, Define, Extend, type.Name, type.Name.ToLower()));
				tw.Close();
			}
			
			
		}		
		
	}
}

