using System;

namespace Aicl.DotJs
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple=false)]
	public class NameAttribute:Attribute
	{
		public NameAttribute ()
		{
		}	
			
	}
	
	[AttributeUsage(AttributeTargets.Property, AllowMultiple=false)]
	public class PropertyTypeAttribute:Attribute
	{
		public PropertyTypeAttribute ()
		{
		}	
			
	}
	
	[AttributeUsage(AttributeTargets.Property, AllowMultiple=false)]
	public class ConvertAttribute:Attribute
	{
		public ConvertAttribute ()
		{
		}	
			
	}
	
}

