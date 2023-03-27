using Scratch_Utils;
using System;
using System.Collections.Generic;

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

		internal static Dictionary<string, SpecVar> specVars = new Dictionary<string, SpecVar>()
		{
			["Direction"] = new SpecVar(UsagePlace.Sprite, "motion_direction", "direction variable"),
			["X"] = new SpecVar(UsagePlace.Sprite, "motion_xposition", "x variable"),
			["Y"] = new SpecVar(UsagePlace.Sprite, "motion_yposition", "y variable")
		};

		public sealed class Goto : Block
		{
			public Goto(object x, object y) : base("Goto X Y", UsagePlace.Sprite, x, y)
			{
				args = new BlockArgs("motion_gotoxy", $"{MakeInput("X", x, "x")},{MakeInput("Y", y, "y")}");
			}

			public Goto(object to):base("Goto To", UsagePlace.Sprite, to)
			{
				string arg;
				if(to is Sprite s) arg = $"\"TO\":[\"{s.name}\",null]";
				else if(to is To t) arg = (t == To.Mouse) ? "\"TO\":[\"_mouse_\",null]" : "\"TO\":[\"_random_\",null]";
				else throw new ArgumentException("to is not a Sprite or To element");

				args = new BlockArgs("motion_goto");

				Block tmp = new Block(null) {
					args = new BlockArgs("motion_goto_menu", null, arg, null, args.Id, true),
					needsNext = false
				};
				kids.Add(tmp);

				args.Inputs = $"\"TO\":[1,\"{tmp.args.Id}\"]";
			}
		}

		public sealed class Glide : Block
		{
			public Glide(object sec, object x, object y) : base("Goto X Y", UsagePlace.Sprite, sec, x, y)
			{
				args = new BlockArgs("motion_glidesecstoxy", $"{MakeInput("SECS", sec, "sec")},{MakeInput("X", x, "x")},{MakeInput("Y", y, "y")}");
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
					args = new BlockArgs("motion_glideto_menu", null, arg, null, args.Id, true),
					needsNext = false
				};
				kids.Add(tmp);

				args.Inputs = $"{MakeInput("SECS", sec, "sec")},\"TO\":[1,\"{tmp.args.Id}\"]";
			}
		}

		public sealed class Move : Block
		{
			public Move(object steps) : base("Move steps", UsagePlace.Sprite, steps)
			{
				args = new BlockArgs("motion_movesteps", MakeInput("STEPS", steps, "steps"));
			}
		}

		public static class Change
		{
			public sealed class X : Block
			{
				public X(object by) : base("Change X by", UsagePlace.Sprite, by)
				{
					args = new BlockArgs("motion_changexby", MakeInput("DX", by, "by"));
				}
			}

			public sealed class Y : Block
			{
				public Y(object by) : base("Change Y by", UsagePlace.Sprite, by)
				{
					args = new BlockArgs("motion_changeyby", MakeInput("DY", by, "by"));
				}
			}
		}

		public static class Set
		{
			public sealed class X : Block
			{
				public X(object value) : base("Change X by", UsagePlace.Sprite, value)
				{
					args = new BlockArgs("motion_setx", MakeInput("X", value, "value"));
				}
			}

			public sealed class Y : Block
			{
				public Y(object value) : base("Change Y value", UsagePlace.Sprite, value)
				{
					args = new BlockArgs("motion_sety", MakeInput("Y", value, "value"));
				}
			}
		}

		public static class Turn
		{
			public sealed class Left : Block
			{
				public Left(object degrees) : base("Change X by", UsagePlace.Sprite, degrees)
				{
					args = new BlockArgs("motion_turnleft", MakeInput("DEGREES", degrees, "degrees"));
				}
			}

			public sealed class Right : Block
			{
				public Right(object degrees) : base("Change Y by", UsagePlace.Sprite, degrees)
				{
					args = new BlockArgs("motion_turnright", MakeInput("DEGREES", degrees, "degrees"));
				}
			}
		}

		public sealed class Point : Block
		{
			public Point(object to) : base("Point to/in direction", UsagePlace.Sprite, to)
			{
				Sprite s = to as Sprite;
				To? t = to as To?;

				if (s != null || t.HasValue)
				{
					string arg;
					if(s != null) arg = $"\"TOWARDS\":[\"{s.name}\",null]";
					else if(t == To.Mouse) arg = "\"TOWARDS\":[\"_mouse_\",null]";
					else throw new ArgumentException("to cannot be To.Random");

					args = new BlockArgs("motion_pointtowards");

					Block tmp = new Block(null)
					{
						args = new BlockArgs("motion_pointtowards_menu", null, arg, null, args.Id, true),
						needsNext = false 
					};
					kids.Add(tmp);
					args.Inputs = $"\"TOWARDS\":[1,\"{tmp.args.Id}\"]";
				}
				else args = new BlockArgs("motion_pointindirection", MakeInput("DIRECTION", to, "to"));
			}
		}

		public sealed class OnEdgeBounce : Block
		{
			public OnEdgeBounce() : base("On edge bounce", UsagePlace.Sprite)
			{
				args = new BlockArgs("motion_ifonedgebounce");
			}
		}

		public sealed class RotationStyle : Block
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