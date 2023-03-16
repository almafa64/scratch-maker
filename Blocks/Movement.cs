using Scratch_Utils;
using System;
using System.Collections.Generic;
using static Scratch.Looks;
using static Scratch.Looks.Effect;
using static Scratch.Movement;

namespace Scratch
{
	public static class Movement 
	{
		public enum To
		{
			Random,
			Mouse
		}

		public enum Vars
		{
			X,
			Y,
			Direction
		}

		internal static Dictionary<string, SpecVar> specVars = new Dictionary<string, SpecVar>();

		public class Goto : Block
		{
			public Goto(object x, object y) : base("Goto X Y", UsagePlace.Sprite, x, y)
			{
				if(TypeCheck.Check(x) == AcceptedTypes.String) throw new ArgumentException("x is string, which is not accepted"); 
				if(TypeCheck.Check(y) == AcceptedTypes.String) throw new ArgumentException("y is string, which is not accepted");

				args = new BlockArgs("motion_gotoxy");

				args.Inputs = $"{MakeInput("X", x)},{MakeInput("Y", y)}";
			}

			public Goto(object to):base("Goto To", UsagePlace.Sprite, to)
			{
				string arg;
				if(to is Sprite s) arg = $"\"TO\":[\"{s.name}\",null]";
				else if(to is To t) arg = (t == To.Mouse) ? "\"TO\":[\"_mouse_\",null]" : "\"TO\":[\"_random_\",null]";
				else throw new ArgumentException("to is not a Sprite or To element");

				args = new BlockArgs("motion_goto");

				Block tmp = new Block(null) {
					args = new BlockArgs("motion_goto_menu", null, arg, null, null, true),
					needsNext = false
				};
				tmp.args.ParentId = args.Id;
				kids.Add(tmp);

				args.Inputs = $"\"TO\":[1,\"{tmp.args.Id}\"]";
			}
		}

		public class Glide : Block
		{
			public Glide(object sec, object x, object y) : base("Goto X Y", UsagePlace.Sprite, sec, x, y)
			{
				if(TypeCheck.Check(sec) == AcceptedTypes.String) throw new ArgumentException("sec is string, which is not accepted");
				if(TypeCheck.Check(x) == AcceptedTypes.String) throw new ArgumentException("x is string, which is not accepted");
				if(TypeCheck.Check(y) == AcceptedTypes.String) throw new ArgumentException("y is string, which is not accepted");

				args = new BlockArgs("motion_glidesecstoxy");

				args.Inputs = $"{MakeInput("SECS", sec)},{MakeInput("X", x)},{MakeInput("Y", y)}";
			}

			public Glide(object sec, object to) : base("Goto To", UsagePlace.Sprite, sec, to)
			{
				string arg;
				if(to is Sprite s) arg = $"\"TO\":[\"{s.name}\",null]";
				else if(to is To t) arg = (t == To.Mouse) ? "\"TO\":[\"_mouse_\",null]" : "\"TO\":[\"_random_\",null]";
				else throw new ArgumentException("to is not a sprite or To element");

				args = new BlockArgs("motion_glideto");

				Block tmp = new Block(null)
				{
					args = new BlockArgs("motion_glideto_menu", null, arg, null, null, true),
					needsNext = false
				};
				tmp.args.ParentId = args.Id;
				kids.Add(tmp);

				args.Inputs = $"{MakeInput("SECS", sec)},\"TO\":[1,\"{tmp.args.Id}\"]";
			}
		}

		public class Move : Block
		{
			public Move(object steps) : base("Move steps", UsagePlace.Sprite, steps)
			{
				if(TypeCheck.Check(steps) == AcceptedTypes.String) throw new ArgumentException("steps is string, which is not accepted");

				args = new BlockArgs("motion_movesteps");

				args.Inputs = MakeInput("STEPS", steps);
			}
		}

		public static class Change
		{
			public class X : Block
			{
				public X(object by) : base("Change X by", UsagePlace.Sprite, by)
				{
					if(TypeCheck.Check(by) == AcceptedTypes.String) throw new ArgumentException("by is string, which is not accepted");

					args = new BlockArgs("motion_changexby");

					args.Inputs = MakeInput("DX", by);
				}
			}

			public class Y : Block
			{
				public Y(object by) : base("Change Y by", UsagePlace.Sprite, by)
				{
					if(TypeCheck.Check(by) == AcceptedTypes.String) throw new ArgumentException("by is string, which is not accepted");

					args = new BlockArgs("motion_changeyby");

					args.Inputs = MakeInput("DY", by);
				}
			}
		}

		public static class Set
		{
			public class X : Block
			{
				public X(object value) : base("Change X by", UsagePlace.Sprite, value)
				{
					if(TypeCheck.Check(value) == AcceptedTypes.String) throw new ArgumentException("value is string, which is not accepted");

					args = new BlockArgs("motion_setx");

					args.Inputs = MakeInput("X", value);
				}
			}

			public class Y : Block
			{
				public Y(object value) : base("Change Y value", UsagePlace.Sprite, value)
				{
					if(TypeCheck.Check(value) == AcceptedTypes.String) throw new ArgumentException("value is string, which is not accepted");

					args = new BlockArgs("motion_sety");

					args.Inputs = MakeInput("Y", value);
				}
			}
		}

		public static class Turn
		{
			public class Left : Block
			{
				public Left(object degrees) : base("Change X by", UsagePlace.Sprite, degrees)
				{
					if (TypeCheck.Check(degrees) == AcceptedTypes.String) throw new ArgumentException("degrees is string, which is not accepted");

					args = new BlockArgs("motion_turnleft");

					args.Inputs = MakeInput("DEGREES", degrees);
				}
			}

			public class Right : Block
			{
				public Right(object degrees) : base("Change Y by", UsagePlace.Sprite, degrees)
				{
					if (TypeCheck.Check(degrees) == AcceptedTypes.String) throw new ArgumentException("degrees is string, which is not accepted");

					args = new BlockArgs("motion_turnright");

					args.Inputs = MakeInput("DEGREES", degrees);
				}
			}
		}

		public class Point : Block
		{
			public Point(object to) : base("Point to/in direction", UsagePlace.Sprite, to)
			{
				if (to is Sprite || to is To)
				{
					string arg;
					if (to is Sprite s) arg = $"\"TOWARDS\":[\"{s.name}\",null]";
					else if (to is To t)
					{
						if (t == To.Random) throw new ArgumentException("to cannot be random in this block");
						arg = "\"TOWARDS\":[\"_mouse_\",null]";
					}
					else throw new ArgumentException("to is not a sprite or To element");

					args = new BlockArgs("motion_pointtowards");

					Block tmp = new Block(null)
					{
						args = new BlockArgs("motion_pointtowards_menu", null, arg, null, null, true),
						needsNext = false 
					};
					tmp.args.ParentId = args.Id;
					kids.Add(tmp);

					args.Inputs = $"\"TOWARDS\":[1,\"{tmp.args.Id}\"]";
				}
				else
				{
					if (TypeCheck.Check(to) == AcceptedTypes.String) throw new ArgumentException("to is string, which is not accepted");

					args = new BlockArgs("motion_pointindirection");

					args.Inputs = MakeInput("DIRECTION", to);
				}
			}
		}

		public class OnEdgeBounce : Block
		{
			public OnEdgeBounce() : base("On edge bounce", UsagePlace.Sprite)
			{
				args = new BlockArgs("motion_ifonedgebounce");
			}
		}

		public class RotationStyle : Block
		{
			public enum RotStyle
			{
				Dont,
				LeftRight,
				Around
			}

			public RotationStyle(RotStyle rs) : base("Rotation style", UsagePlace.Sprite)
			{
				string tmp = "";
				switch(rs)
				{
					case RotStyle.Dont:
						tmp = "don't rotate";
						break;
					case RotStyle.LeftRight:
						tmp = "left-right";
						break;
					case RotStyle.Around:
						tmp = "all around";
						break;
				}
				args = new BlockArgs("motion_setrotationstyle", null, $"\"STYLE\":[\"{tmp}\",null]");
			}
		}
	}	
}