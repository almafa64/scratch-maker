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
				using(Sprite s = new Sprite("test2", project))
				{
					s.Vars["te"] = new Var(96);
					s.Vars["fwafwa", true] = new Var(69);
				}

				using(Sprite sprite = new Sprite("testSprite", project))
				{
					sprite.Vars["te"] = new Var(43);
					sprite.Lists["rerererere", true] = new List(43, "adwa", true);

					using(Column column = new Column(sprite))
					{
						column.Add(new Movement.Goto(2, 200));
						column.Add(new Movement.Goto(sprite.Vars["te"], sprite.Vars["te"]));
						column.Add(new Movement.Glide(2, sprite.Vars["te"], sprite.Vars["te"]));
						column.Add(new Movement.Goto(project.Sprites["test2"]));
						column.Add(new Movement.Goto(Movement.To.Mouse));
						column.Add(new Movement.Glide(2, project.Sprites["test2"]));
						column.Add(new Movement.Glide(2, Movement.To.Random));
					}

					using(MyBlock b = new MyBlock(sprite, "test", 100, 100).AddValue("x").Build())
					{
						b.Add(new Movement.Goto(4242, b["x"]));
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
