using System;
using System.Collections.Generic;
using CodeBlocks;
using Scratch;

namespace CodeBlocks
{
	public class Block
	{
		public string id;
		public Block()
		{
			id = "random(nem fogom megcisnálni ebben a tesztben, de a rendesben már benne van)";
		}
	}
	
	public static class Movement
	{
		public class Goto : Block
		{
			public double x, y;
			public Goto(double x, double y):base()
			{
				this.x = x;
				this.y = y;
			}
		}
	}
}

namespace Scratch
{
	public class Project : IDisposable
	{
		string name = ""; 
		public Project(string name)
		{
			this.name = name;
		}
		
		public void Dispose() {}
	}
	
	public class Sprite : IDisposable
	{
		string name = "";
		Project project = null;
		public Sprite(string name, Project project)
		{
			this.name = name;
			this.project = project;
		}
		
		public void Dispose() {}
	}
	
	public class Column : List<Block>, IDisposable
	{
		Sprite sprite = null;
		public Column(Sprite sprite)
		{
			this.sprite = sprite;
		}
		
		public void Dispose() {}
	}
}

public class HelloWorld
{
	public static void Main(string[] args)
	{
		using(Project project1 = new Project("projectName"))
		{
			using(Sprite sprite1 = new Sprite("spriteName",project1))
			{
				using(Column column1 = new Column(sprite1))
				{
					double x = 2;
					double y = 2;
					column1.Add(new Movement.Goto(x, y));
				}
			}
		}
	}
}