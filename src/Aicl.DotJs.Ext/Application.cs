using System;
using System.IO;
using ServiceStack.Text;

namespace Aicl.DotJs.Ext
{
	public class Application
	{
		private string template=@"Ext.Loader.setConfig({{enabled: true}});
Ext.Loader.setPath('{0}', '../app');
Ext.require(['Ext.tip.*']);
Ext.QuickTips.init();
    
Ext.application({{
name: '{0}',
appFolder: '../app',

launch: function(){{
    Ext.create('Ext.form.Panel',{{
  	width:950,
    id:'panelModule',
    //baseCls:'x-plain',
    frame: true,
    renderTo: 'module',
    layout: {{
        type: 'table',
        columns: 2
    }},
    items:[
     	{{xtype:'{1}list'}},
       	{{
			xtype:'panel',
			height:352,
			width:340,
			baseCls:'x-plain',
			layout: {{
    		type: 'vbox'       
    		}},
			items:[
				{{ xtype:'{1}form'}}
			]	
		}}
    ]
    }});
}},
    
controllers: ['{2}']
    
}});";
		
		private string html=@"<html>
<head>
    <meta http-equiv=""Content-Type"" content=""text/html; charset=iso-8859-1"" />
    <title>{0}</title>
    <link rel=""stylesheet"" type=""text/css"" href=""../../extjs/resources/css/{1}""/>
    <link rel=""stylesheet"" type=""text/css"" href=""../../resources/util.css""/>
	<script type=""text/javascript"" src=""../../extjs/bootstrap.js""></script>
    <script type=""text/javascript"" src=""../../resources/util.js""></script>
	<script type=""text/javascript"" src=""app.js""></script>
</head>
<!-- <body style=""text-align: center;""> !-->
<body>
	<div id=""module""/>	
</body>
</html>";
		
		private Type type;
		
		public string AppName { get; set;}
		
		public string FileName { get; set;}
		
		public string OutputDirectory { get; set;}
		
		public string Theme {get; set;}
		
		public Application(Type type)
		{
			this.type=type;
		}
		
		public void Write()
		{
			if(string.IsNullOrEmpty(AppName)) AppName="App";
			
			if(string.IsNullOrEmpty(OutputDirectory))
			{
				OutputDirectory= Path.Combine(Directory.GetCurrentDirectory(), type.Name.ToLower());
				
				if (!Directory.Exists(OutputDirectory))
					Directory.CreateDirectory(OutputDirectory);
			}
			
			if(string.IsNullOrEmpty(Theme)) Theme="ext-all-slate.css";
			
			if(string.IsNullOrEmpty(FileName))
				FileName= "app.js";
				
			using (TextWriter tw = new StreamWriter(Path.Combine(OutputDirectory, FileName)))
			{
				tw.Write(string.Format(template, AppName, type.Name.ToLower(), type.Name));
				tw.Close();
			}
			
			using (TextWriter hw = new StreamWriter(Path.Combine(OutputDirectory, "index.html")))
			{
				hw.Write(string.Format(html,type.Name, Theme));
				hw.Close();
			}
			
		}
		
	}
}

