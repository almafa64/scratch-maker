using Scratch;

namespace scratch_test
{
	internal class Program
	{
		static void Main(string[] args)
		{
			using(Project project = new Project("test", Extensions.Translate))
			{
				using(Sprite sprite = new Sprite("testSprite", project))
				{
					using(Column column = new Column(sprite))
					{
						column.Add(new Movement.Goto(2, 200));
						column.Add(new Movement.Goto(200, 2));
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
