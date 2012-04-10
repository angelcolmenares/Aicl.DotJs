using System;
using System.Reflection;

using Aicl.DotJs;
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
			
			Store store = new Store(typeof(Demo));
			store.Write();
			
			List list = new List(typeof(Demo));
			list.Write();
			
			Form form = new Form(typeof(Demo));
			form.Write();
			
			Controller controller = new Controller(typeof(Demo));
			controller.Write();
			
			Application app = new Application(typeof(Demo));
			app.Write();
			
			Console.WriteLine ("This is The End my friend!");
		}
	}
	
	public class Demo{
		public Demo(){
		}
		
		public int Id { get; set;}
		public string Name{get; set;}
		public DateTime? Date{get; set;}
		public int SomeInteger{get; set;}
		public decimal SomeDecimal{get; set;}
		public bool SomeBool{get; set;}
		
	}
}
