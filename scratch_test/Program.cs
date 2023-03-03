using Scratch;

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
					sprite.MakeList("testList", true, new object[] { 42, 521, "yey" });
					sprite.MakeList("testList2");

					using(Column column = new Column(sprite))
					{
						column.Add(new Movement.Goto(2, 200));
						column.Add(new Movement.Goto(sprite.GetVar("testVar2"), 2));
					}

					sprite.AddCostume(new Costume("6a952345f4af816734ce38eb69bfea8a.png", "testCostume"));
				}

				using(Project.Background bg = project.background)
				{

				}
			}
		}
	}
}
