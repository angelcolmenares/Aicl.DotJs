using System;
using System.Reflection;

using Aicl.DotJs.Ext;

namespace Test01
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine ("Hello World!");
			
			Model model = new Model(typeof(Demo));
			
			model.Write<ExtModel,ExtModelField>();
			
			Console.WriteLine ("This is The End my friend!");
		}
	}
	
	public class Demo{
		public Demo(){
		}
		
		public int Id { get; set;}
		public string Name{get; set;}
		public DateTime Date{get; set;}
	}
}
