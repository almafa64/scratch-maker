using Scratch;
using System;
using System.Threading;

namespace scratch_test
{
	internal class Program
	{
		static void Main(string[] args)
		{
			using(Project project = new Project("test", Extensions.Translate, false))
			{
				using(Sprite sprite = new Sprite("testSprite", project))
				{
					sprite.MakeVariable("testVar1", false, 42);
					sprite.MakeVariable("testVar2", false);
					sprite.MakeList("testList", false, new object[] { 42, 521, "yey" });
					sprite.MakeList("testList2");

					sprite.Vars["te"] = new Var(43);
					sprite.Lists["rerererere", true] = new List(43, "adwa", true);

					using(Column column = new Column(sprite))
					{
						column.Add(new Movement.Goto(2, 200));
						column.Add(new Movement.Goto(sprite.Vars["testVar2"], sprite.Vars["testVar1"]));
					}

					using(Column c = new Column(sprite, 100, 100))
					{
						MyBlock b = new MyBlock("test").AddValue("x");
						c.Add(b);
						c.Add(new Movement.Goto(4242, b["x"]));
					}

					using(Column column = new Column(sprite))
					{
						column.Add(new Movement.Goto(sprite.Vars["te"], 53));
					}

					sprite.AddCostumes(new Costume("6a952345f4af816734ce38eb69bfea8a.png", "testCostume"));
					sprite.AddSounds(new Sound("83c36d806dc92327b9e7049a565c6bff.wav", "catting"));
				}

				using(Project.Background bg = project.background)
				{

				}
			}
		}
	}
}
