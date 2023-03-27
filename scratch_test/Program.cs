using Scratch;

namespace scratch_test
{
	internal class Program
	{
		static void Main(string[] args)
		{
			using(Project project = new Project("test", Extensions.Translate, false))
			{
				using(Sprite sprite = new Sprite("test3", project))
				{
					sprite.Delete();
				}

				using(Project.Background bg = project.background)
				{
					bg.Costumes["testBackdrop"] = new Costume("blackness.png");

					using(Sprite s = new Sprite("test2", project, 43, 24, 180, 50))
					{
						s.Vars["te"] = new Var(96);
						s.Vars["fwafwa", true] = new Var(69);
						s.Broadcasts["test"] = new Broadcast();

						using(Events.ClickSprite c = new Events.ClickSprite(s, 0)) { }
						using(Events.ClickFlag c = new Events.ClickFlag(s, 0, 100)) { }
						using(Events.KeyPress c = new Events.KeyPress(Events.KeyPress.Keys.UpArrow, s, 0, 200)) { }
						using(Events.BackdropSwitch c = new Events.BackdropSwitch(bg.Costumes["testBackdrop"], s, 0, 300)) { }
						using(Events.Greater c = new Events.Greater(Events.Greater.What.Loudness, 20, s, 0, 400)) { }
						using(Events.Broadcasts.Recive c = new Events.Broadcasts.Recive(s.Broadcasts["test"], s, 0, 500)) { }
					}

					using(Events.ClickStage c = new Events.ClickStage(bg, 400)) { }
					using(Column column = new Column(bg, 0, 0))
					{
						column.Add(new Looks.Effect.Set(Looks.Effect.Effects.Ghost, bg.Vars["fwafwa"]));
						column.Add(new Looks.Switch.BackdropWait(Looks.Switch.Which.RandomBackdrop));
						column.Add(new Looks.Switch.BackdropWait(Looks.Switch.Which.NextBackdrop));
						column.Add(new Looks.Switch.BackdropWait(Looks.Switch.Which.PreviousBackdrop));
						column.Add(new Looks.Switch.BackdropWait(bg.Costumes["testBackdrop"]));
					}
				}

				using(Sprite sprite = new Sprite("testSprite", project))
				{
					sprite.Costumes["testCostume"] = new Costume("6a952345f4af816734ce38eb69bfea8a.png");
					sprite.Sounds["catting"] = new Sound("83c36d806dc92327b9e7049a565c6bff.wav"); //make recording

					sprite.AddComments(new Comment("testing", 200, 200, true));

					sprite.Vars["te", false, true, 53, 155, 55] = new Var(43);
					sprite.Vars["text", true] = new Var("aebc");
					sprite.Lists["re", true] = new List(43, "adwa", true);
					sprite.Lists["re2", true, true] = new List(43, "adwa", true);

					using(MyBlock b = new MyBlock(sprite, "test", -300, 0).AddValue("x").AddBool("y").AddDesc("lol", "lol"))
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
						b.Add(new Looks.Layer.Go(Looks.Layer.Go.WhereTo.Forward, b["x"]));
						b.Add(new Looks.Effect.Set(Looks.Effect.Effects.Color, b["x"]));
						b.Add(new Looks.Effect.Change(Looks.Effect.Effects.Whirl, b["x"]));

						b.Add(new Sounds.Volume.Set(b["x"]));
						b.Add(new Sounds.Volume.Change(b["x"]));
						b.Add(new Sounds.Effect.Change(Sounds.Effect.Effects.Pan, b["x"]));
						b.Add(new Sounds.Effect.Set(Sounds.Effect.Effects.Pitch, b["x"]));

						b.Add(new Control.Wait(b["x"]));

						b.Add(new Movement.Goto(b["x"], new Operators.And(new Sensing.Mouse.Down(), b["y"])));

						b.Add(new Variables.Change(sprite.Vars["te"], b["x"]));
					}
					using(Column column = new Column(sprite, -300, -200)) //MyBlock call
					{
						column.Add(new MyBlock.Call(sprite.MyBlocks["test"], 43, new Operators.Contains("apple", "a")));
						column.Add(new MyBlock.Call(sprite.MyBlocks["test"], sprite.Vars["te"], new Operators.Contains("banana", "b")));
					}

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

					using (Column column = new Column(sprite, 300, 0)) //Looks
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
						column.Add(new Looks.Next.Backdrop());
						column.Add(new Looks.Next.Costume());
						column.Add(new Looks.Layer.GoTo(Looks.Layer.GoTo.To.Back));
						column.Add(new Looks.Layer.GoTo(Looks.Layer.GoTo.To.Front));
						column.Add(new Looks.Layer.Go(Looks.Layer.Go.WhereTo.Backward, 43));
						column.Add(new Looks.Layer.Go(Looks.Layer.Go.WhereTo.Forward, sprite.Vars["te"]));
						column.Add(new Looks.Effect.Clear());
						column.Add(new Looks.Effect.Set(Looks.Effect.Effects.Brightness, 242));
						column.Add(new Looks.Effect.Set(Looks.Effect.Effects.Ghost, sprite.Vars["te"]));
						column.Add(new Looks.Effect.Change(Looks.Effect.Effects.Fisheye, 242));
						column.Add(new Looks.Effect.Change(Looks.Effect.Effects.Pixelate, sprite.Vars["te"]));
						column.Add(new Looks.Switch.Costumes(sprite.Costumes["testCostume"]));
						column.Add(new Looks.Switch.Backdrop(Looks.Switch.Which.RandomBackdrop));
						column.Add(new Looks.Switch.Backdrop(Looks.Switch.Which.NextBackdrop));
						column.Add(new Looks.Switch.Backdrop(Looks.Switch.Which.PreviousBackdrop));
						column.Add(new Looks.Switch.Backdrop(sprite.Costumes["testCostume"]));
					}

					using (Column column = new Column(sprite, 600, 0)) //Sounds
					{
						column.Add(new Sounds.Stop());
						column.Add(new Sounds.Effect.Clear());
						column.Add(new Sounds.Volume.Change(Sounds.Vars.Volume));
						column.Add(new Sounds.Volume.Change(43));
						column.Add(new Sounds.Volume.Change(sprite.Vars["te"]));
						column.Add(new Sounds.Volume.Set(76));
						column.Add(new Sounds.Volume.Set(sprite.Vars["te"]));
						column.Add(new Sounds.Play(sprite.Sounds["catting"]));
						column.Add(new Sounds.PlayAndWait(sprite.Sounds["catting"]));
						column.Add(new Sounds.Effect.Change(Sounds.Effect.Effects.Pan, 43));
						column.Add(new Sounds.Effect.Change(Sounds.Effect.Effects.Pitch, sprite.Vars["te"]));
						column.Add(new Sounds.Effect.Set(Sounds.Effect.Effects.Pitch, 76));
						column.Add(new Sounds.Effect.Set(Sounds.Effect.Effects.Pan, sprite.Vars["te"]));
					}

					using(Column column = new Column(sprite, 900, 0)) //Events
					{
						column.Add(new Events.Broadcasts.Send(sprite.Broadcasts["test"]));
						column.Add(new Events.Broadcasts.SendAndWait(sprite.Broadcasts["test"]));
					}

					using(Column column = new Column(sprite, 1200, 0)) //Control
					{
						column.Add(new Control.Loop.Forever(new Control.Wait(20), new Control.Wait(sprite.Vars["te"]), new Control.WaitUntil(new Sensing.Mouse.Down()), new Control.Stop(Control.Stop.What.All)));
					}
					using(Column column = new Column(sprite, 1200, 300)) //Control2
					{
						column.Add(new Control.Loop.Repeat(20, new Control.Wait(20), new Control.Wait(sprite.Vars["te"]), new Control.WaitUntil(new Sensing.Mouse.Down()), new Control.Stop(Control.Stop.What.This)));
						column.Add(new Control.Loop.Repeat(sprite.Vars["te"], new Control.Wait(20), new Control.Wait(sprite.Vars["te"]), new Control.WaitUntil(new Sensing.Mouse.Down()), new Control.Stop(Control.Stop.What.This)));
						column.Add(new Control.Loop.RepeatUntil(new Sensing.Mouse.Down(), new Control.Wait(20), new Control.Wait(sprite.Vars["te"]), new Control.WaitUntil(new Sensing.Mouse.Down()), new Control.Stop(Control.Stop.What.This)));
					}
					using(Control.Clone.StartAs column = new Control.Clone.StartAs(sprite, 1200, 1200)) //Control3
					{
						column.Add(new Control.Clone.Create(Control.Clone.Create.What.Myself));
						column.Add(new Control.Clone.Create(project.Sprites["test2"]));
						column.Add(new Control.Stop(Control.Stop.What.OtherScripts));
						column.Add(new Control.Clone.Delete());
					}
					using(Column column = new Column(sprite, 1200, 1500)) //Control4
					{
						column.Add(new Control.If(new Sensing.Mouse.Down(), new Control.Clone.Create(Control.Clone.Create.What.Myself), new Control.Clone.Create(project.Sprites["test2"]), new Control.Stop(Control.Stop.What.OtherScripts)));
						Block[] blocksTrue = {new Control.Wait(sprite.Vars["te"]), new Control.Stop(Control.Stop.What.All)};
						Block[] blocksFalse = {new Control.Wait(53), new Control.Stop(Control.Stop.What.This)};
						column.Add(new Control.IfElse(new Sensing.Mouse.Down(), blocksTrue, blocksFalse));
					}

					using(Column column = new Column(sprite, 1500, 0)) //Sensing
					{
						column.Add(new Sensing.Ask("test?"));
						column.Add(new Sensing.Ask(Sensing.Vars.Answer));
						column.Add(new Sensing.DragMode(Sensing.DragMode.Mode.Draggable));
						column.Add(new Sensing.DragMode(Sensing.DragMode.Mode.NotDraggable));
						column.Add(new Sensing.ResetTimer());
					}
					using(Column column = new Column(sprite, 1500, 500)) //Sensing2
					{
						column.Add(new Operators.And(new Operators.Or(new Operators.And(new Operators.Or(new Sensing.Touching(Sensing.Touching.What.Mouse), new Operators.And(new Sensing.Mouse.Down(), new Sensing.KeyPress(Sensing.KeyPress.Keys.Any))), new Sensing.Touching(Sensing.Touching.What.Edge)), new Sensing.Touching(project.Sprites["test2"])), new Operators.Or(new Sensing.TouchingColor(Color.Red), new Sensing.ColorTouchingColor(new Color("abcdef"), new Color("#abcdef")))));
					}
					using(Column column = new Column(sprite, 1500, 600)) //Sensing3
					{
						column.Add(new Operators.Subtract(new Operators.Multiply(new Operators.Divide(new Sensing.Mouse.X(), new Sensing.DaysSince()), new Sensing.Mouse.Y()), new Operators.Add(new Sensing.Distance(Sensing.Distance.What.Mouse), new Sensing.Distance(project.Sprites["test2"]))));
					}
					using(Column column = new Column(sprite, 1500, 700)) //Sensing4
					{
						column.Add(new Operators.Add(new Operators.Add(new Operators.Add(new Operators.Add(new Operators.Add(new Operators.Add(new Operators.Add(new Operators.Add(new Sensing.Of(Sensing.Of.Data.Volume, project.Sprites["test2"]), new Sensing.Of(Sensing.Of.Data.BackdropName, project.background)), new Sensing.Of(Sensing.Of.Data.BackdropNum, project.background)), new Sensing.Of(Sensing.Of.Data.Size, project.Sprites["test2"])), new Sensing.Of(Sensing.Of.Data.CostumeName, project.Sprites["test2"])), new Sensing.Of(Sensing.Of.Data.CostumeNum, project.Sprites["test2"])), new Sensing.Of(Sensing.Of.Data.Direction, project.Sprites["test2"])), new Sensing.Of(Sensing.Of.Data.PositionY, project.Sprites["test2"])), new Operators.Add(new Sensing.Of(Sensing.Of.Data.PositionX, project.Sprites["test2"]), new Operators.Add(new Sensing.Of(project.Sprites["test2"].Vars["te"]), new Sensing.Of(sprite.Vars["fwafwa"])))));
					}

					using(Column column = new Column(sprite, 1800, 0)) //Operators
					{
						column.Add(new Operators.Round(new Operators.Add(new Operators.Subtract(42, new Operators.Divide(sprite.Vars["te"], 4)), new Operators.Multiply(sprite.Vars["te"], 69))));
					}
					using(Column column = new Column(sprite, 1800, 100)) //Operators2
					{
						column.Add(new Operators.Not(new Operators.And(new Operators.Or(new Operators.Greater(53, sprite.Vars["te"]), new Operators.Lesser(new Operators.Greater(25, sprite.Vars["te"]), sprite.Vars["te"])), new Operators.Not(new Operators.Equal(53, sprite.Vars["te"])))));
					}
					using(Column column = new Column(sprite, 1800, 200)) //Operators3
					{
						column.Add(new Operators.Contains(sprite.Vars["text"], new Operators.Letter(new Operators.Random(sprite.Vars["te"], new Operators.Length(new Operators.Join(sprite.Vars["text"], " 535"))), "test")));
					}
					using(Column column = new Column(sprite, 1800, 300)) //Operators4
					{
						column.Add(new Operators.Abs(new Operators.Floor(new Operators.Ceil(new Operators.Sqrt(new Operators.Sin(new Operators.Cos(new Operators.Tan(new Operators.Asin(new Operators.Acos(new Operators.Tan(new Operators.Ln(new Operators.Log(new Operators.EPow(new Operators.TenPow(2)))))))))))))));
					}

					using (Column column = new Column(sprite, 2100, 0)) //Variables
					{

						column.Add(new Variables.Change(sprite.Vars["te"], 64)).AddComment("trying to test");
						column.Add(new Variables.Change(sprite.Vars["fwafwa"], sprite.Vars["te"]));
						column.Add(new Variables.Set(sprite.Vars["te"], 64));
						column.Add(new Variables.Set(sprite.Vars["fwafwa"], sprite.Vars["te"]));
						column.Add(new Variables.Show(sprite.Vars["te"]));
						column.Add(new Variables.Hide(sprite.Vars["fwafwa"]));
					}

					using(Column column = new Column(sprite, 2400, 0)) //Lists
					{
						column.Add(new Lists.Add(42, sprite.Lists["re"]));
						column.Add(new Lists.Add(sprite.Lists["re"], sprite.Lists["re"]));
						column.Add(new Lists.Add(sprite.Vars["te"], sprite.Lists["re"]));
						column.Add(new Lists.Delete(42, sprite.Lists["re"]));
						column.Add(new Lists.Delete(sprite.Vars["te"], sprite.Lists["re"]));
						column.Add(new Lists.Delete.All(sprite.Lists["re"]));
						column.Add(new Lists.Insert(42, sprite.Vars["te"], sprite.Lists["re"]));
						column.Add(new Lists.Insert(sprite.Vars["te"], 42, sprite.Lists["re"]));
						column.Add(new Lists.Replace(42, sprite.Vars["te"], sprite.Lists["re"]));
						column.Add(new Lists.Replace(sprite.Vars["te"], 43, sprite.Lists["re"]));
						column.Add(new Lists.Show(sprite.Lists["re"]));
						column.Add(new Lists.Hide(sprite.Lists["re"]));
					}
					using(Column column = new Column(sprite, 2400, 600)) //Lists2
					{
						column.Add(new Operators.Add(new Operators.Add(new Lists.ItemIndex(24, sprite.Lists["re"]), new Lists.Item(sprite.Vars["te"], sprite.Lists["re"])), new Lists.Length(sprite.Lists["re"])));
					}
					using(Column column = new Column(sprite, 2400, 700)) //Lists3
					{
						column.Add(new Lists.Contains("a", sprite.Lists["re"]));
					}

					using(Column column = new Column(sprite))
					{
						column.Add(new Movement.Goto(24, 53));
						column.Delete();
					}
				}
			}
		}
	}
}