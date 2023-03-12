/*using Scratch;

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
						new Movement.Goto(200, 200).PlaceIn(column);
					}

					using(MyBlock b = new MyBlock(sprite, "test", 100, 100).AddValue("x").Build())
					{
						//b.Add(new Movement.Goto(4242, b["x"]));
						
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
}*/

using Scratch;
using System.Data.Common;

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

					using(Column column = new Column(sprite)) //Movement
					{
						column.Add(new Movement.Goto(2, 200));
						column.Add(new Movement.Glide(Movement.Vars.Direction, Movement.Vars.X, Movement.Vars.Y));
						column.Add(new Movement.Goto(2, 200));
						column.Add(new Movement.Goto(sprite.Vars["te"], sprite.Vars["fwafwa"]));
						column.Add(new Movement.Glide(2, sprite.Vars["fwafwa"], sprite.Vars["te"]));
						column.Add(new Movement.Goto(project.Sprites["test2"]));
						column.Add(new Movement.Goto(Movement.To.Mouse));
						column.Add(new Movement.Glide(2, project.Sprites["test2"]));
						column.Add(new Movement.Glide(2, Movement.To.Random));
						column.Add(new Movement.Move(42));
						column.Add(new Movement.Move(sprite.Vars["te"]));
						column.Add(new Movement.Change.X(42));
						column.Add(new Movement.Change.X(sprite.Vars["te"]));
						column.Add(new Movement.Change.Y(42));
						column.Add(new Movement.Change.Y(sprite.Vars["te"]));
						column.Add(new Movement.Set.X(42));
						column.Add(new Movement.Set.X(sprite.Vars["te"]));
						column.Add(new Movement.Set.Y(42));
						column.Add(new Movement.Set.Y(sprite.Vars["te"]));
						column.Add(new Movement.Turn.Left(42));
						column.Add(new Movement.Turn.Left(sprite.Vars["te"]));
						column.Add(new Movement.Turn.Right(42));
						column.Add(new Movement.Turn.Right(sprite.Vars["te"]));
						column.Add(new Movement.Point(43));
						column.Add(new Movement.Point(sprite.Vars["te"]));
						column.Add(new Movement.Point(project.Sprites["test2"]));
						column.Add(new Movement.Point(Movement.To.Mouse));
						column.Add(new Movement.OnEdgeBounce());
						column.Add(new Movement.RotationStyle(Movement.RotationStyle.RotStyle.Around));
						column.Add(new Movement.RotationStyle(Movement.RotationStyle.RotStyle.Dont));
						column.Add(new Movement.RotationStyle(Movement.RotationStyle.RotStyle.LeftRight));
					}

					using(Column column = new Column(sprite, 300, 0)) //Looks
					{
						column.Add(new Looks.Say("test"));
						column.Add(new Looks.Say(Looks.Vars.BackdropName));
						column.Add(new Looks.Say(Looks.Vars.BackdropNumber));
						column.Add(new Looks.Say(Looks.Vars.CostumeName));
						column.Add(new Looks.Say(Looks.Vars.CostumeNumber));
						column.Add(new Looks.Say(Looks.Vars.Size));
						column.Add(new Looks.Say(42, sprite.Vars["fwafwa"]));
						column.Add(new Looks.Think(sprite.Vars["te"]));
						column.Add(new Looks.Think(sprite.Vars["te"], sprite.Vars["fwafwa"]));
						column.Add(new Looks.Show());
						column.Add(new Looks.Hide());
						column.Add(new Looks.Size.Change(421));
						column.Add(new Looks.Size.Set(6343));
						column.Add(new Looks.Size.Change(sprite.Vars["te"]));
						column.Add(new Looks.Size.Set(sprite.Vars["fwafwa"]));
					}

					using(MyBlock b = new MyBlock(sprite, "test", -300, 0).AddValue("x").Build())
					{
						b.Add(new Movement.Goto(4242, b["x"]));
						b.Add(new Movement.Glide(b["x"], b["x"], b["x"]));
						b.Add(new Movement.Move(b["x"]));
						b.Add(new Movement.Change.X(b["x"]));
						b.Add(new Movement.Change.Y(b["x"]));
						b.Add(new Movement.Set.X(b["x"]));
						b.Add(new Movement.Set.Y(b["x"]));
						b.Add(new Movement.Turn.Left(b["x"]));
						b.Add(new Movement.Turn.Right(b["x"]));
						b.Add(new Movement.Point(b["x"]));

						b.Add(new Looks.Say(b["x"], b["x"]));
						b.Add(new Looks.Think(b["x"], b["x"]));
						b.Add(new Looks.Size.Change(b["x"]));
						b.Add(new Looks.Size.Set(b["x"]));
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