using System;
namespace Aicl.DotJs.Ext
{
	public class FormItem
	{
		public FormItem ()
		{
		}
		
		public string xtype{get;set;}
		
		public bool? allowDecimals {get; set;}
		
		public string name {get;set;}
		
		public string fieldLabel {get;set;}
				
		public bool? allowBlank {get;set;}
		
		public string format { get; set;}
		
		public int? maxLength { get; set;}
		
		public bool? enforceMaxLength { get; set;}
		
		public string vtype { get; set;}
		
	}
}

/*
 
this.items = [{
            xtype:'hidden',
            name: 'Id'
        },{
            fieldLabel: 'Company Name',
            name: 'Name',
            allowBlank:false
        },{
        	xtype:'numberfield',
        	fieldLabel: 'Turnover',
            name: 'Turnover',
            allowBlank:false
        },{
            fieldLabel: 'Started Date',
            name: 'Started',
			format:	'd.m.Y',
			xtype     : 'datefield'
        },{
            xtype:'numberfield',
        	fieldLabel: 'No. Employees',
            name: 'Employees',
            allowBlank:false
        },{
            fieldLabel: 'Created Date',
            name: 'CreatedDate',
			format:	'd.m.Y',
			xtype     : 'datefield'
        }];
 */