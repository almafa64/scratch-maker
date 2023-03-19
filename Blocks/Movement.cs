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

		public class Goto : Block
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
					args = new BlockArgs("motion_glideto_menu", null, arg, null, null, true),
					needsNext = false
				};
				tmp.args.ParentId = args.Id;
				kids.Add(tmp);

				args.Inputs = $"{MakeInput("SECS", sec, "sec")},\"TO\":[1,\"{tmp.args.Id}\"]";
			}
		}

		public class Move : Block
		{
			public Move(object steps) : base("Move steps", UsagePlace.Sprite, steps)
			{
				args = new BlockArgs("motion_movesteps", MakeInput("STEPS", steps, "steps"));
			}
		}

		public static class Change
		{
			public class X : Block
			{
				public X(object by) : base("Change X by", UsagePlace.Sprite, by)
				{
					args = new BlockArgs("motion_changexby", MakeInput("DX", by, "by"));
				}
			}

			public class Y : Block
			{
				public Y(object by) : base("Change Y by", UsagePlace.Sprite, by)
				{
					args = new BlockArgs("motion_changeyby", MakeInput("DY", by, "by"));
				}
			}
		}

		public static class Set
		{
			public class X : Block
			{
				public X(object value) : base("Change X by", UsagePlace.Sprite, value)
				{
					args = new BlockArgs("motion_setx", MakeInput("X", value, "value"));
				}
			}

			public class Y : Block
			{
				public Y(object value) : base("Change Y value", UsagePlace.Sprite, value)
				{
					args = new BlockArgs("motion_sety", MakeInput("Y", value, "value"));
				}
			}
		}

		public static class Turn
		{
			public class Left : Block
			{
				public Left(object degrees) : base("Change X by", UsagePlace.Sprite, degrees)
				{
					args = new BlockArgs("motion_turnleft", MakeInput("DEGREES", degrees, "degrees"));
				}
			}

			public class Right : Block
			{
				public Right(object degrees) : base("Change Y by", UsagePlace.Sprite, degrees)
				{
					args = new BlockArgs("motion_turnright", MakeInput("DEGREES", degrees, "degrees"));
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
					args = new BlockArgs("motion_pointindirection", MakeInput("DIRECTION", to, "to"));
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