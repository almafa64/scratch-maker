using Scratch_Utils;
using System;

namespace Scratch
{
	public static class Movement 
	{
		public enum To
		{
			Random,
			Mouse
		}

		public class Goto : Block
		{
			public Goto(object x, object y) : base("Goto X Y", x,y)
			{
				if(TypeCheck.Check(x) == AcceptedTypes.String) throw new ArgumentException($"x is string, which is not accepted"); 
				if(TypeCheck.Check(y) == AcceptedTypes.String) throw new ArgumentException($"y is string, which is not accepted");

				args = new BlockArgs("motion_gotoxy");

				string arg1;
				if(x is MyBlock.MyBlockVar bx) arg1 = MyBlockVarArg($"\"X\":[3,\"{bx.block.args.Id}\",[4,\"0\"]]", this, bx.block);
				else if(x is Var varx) arg1 = $"\"X\":[3,[12,\"{varx.Name}\",\"{varx.Id}\"],[4,\"0\"]]";
				else arg1 = $"\"X\":[1,[4,\"{x}\"]]";

				string arg2;
				if(y is MyBlock.MyBlockVar by) arg2 = MyBlockVarArg($"\"Y\":[3,\"{by.block.args.Id}\",[4,\"0\"]]", this, by.block);
				else if(y is Var vary) arg2 = $"\"Y\":[3,[12,\"{vary.Name}\",\"{vary.Id}\"],[4,\"0\"]]";
				else arg2 = $"\"Y\":[1,[4,\"{y}\"]]";

				args.Inputs = $"{arg1},{arg2}";
			}

			public Goto(object to):base("Goto To")
			{
				if(to == null) throw new ArgumentException("to cannot be null");
				else if(TypeCheck.Check(to) != AcceptedTypes.None) throw new ArgumentException("to is not a sprite or To element");

				string arg;
				if(to is Sprite s) arg = $"\"TO\":[\"{s.name}\",null]";
				else if(to is To t) arg = (t == To.Mouse) ? "\"TO\":[\"_mouse_\",null]" : "\"TO\":[\"_random_\",null]";
				else throw new ArgumentException("to cannot be the Stage");

				args = new BlockArgs("motion_goto");
				Block tmp = new Block(null) {
					args = new BlockArgs("motion_goto_menu", null, arg, null, null, true)
				};
				tmp.args.ParentId = args.Id;
				kids.Add(tmp);

				args.Inputs = $"\"TO\":[1,\"{tmp.args.Id}\"]";
			}
		}

		public class Glide : Block
		{
			public Glide(object sec, object x, object y) : base("Goto X Y", sec, x, y)
			{
				if(TypeCheck.Check(sec) == AcceptedTypes.String) throw new ArgumentException($"sec is string, which is not accepted");
				if(TypeCheck.Check(x) == AcceptedTypes.String) throw new ArgumentException($"x is string, which is not accepted");
				if(TypeCheck.Check(y) == AcceptedTypes.String) throw new ArgumentException($"y is string, which is not accepted");

				args = new BlockArgs("motion_glidesecstoxy");

				string arg1;
				if(x is MyBlock.MyBlockVar bx) arg1 = MyBlockVarArg($"\"X\":[3,\"{bx.block.args.Id}\",[4,\"0\"]]", this, bx.block);
				else if(x is Var varx) arg1 = $"\"X\":[3,[12,\"{varx.Name}\",\"{varx.Id}\"],[4,\"0\"]]";
				else arg1 = $"\"X\":[1,[4,\"{x}\"]]";

				string arg2;
				if(y is MyBlock.MyBlockVar by) arg2 = MyBlockVarArg($"\"Y\":[3,\"{by.block.args.Id}\",[4,\"0\"]]", this, by.block);
				else if(y is Var vary) arg2 = $"\"Y\":[3,[12,\"{vary.Name}\",\"{vary.Id}\"],[4,\"0\"]]";
				else arg2 = $"\"Y\":[1,[4,\"{y}\"]]";

				string arg3;
				if(sec is MyBlock.MyBlockVar bsec) arg3 = MyBlockVarArg($"\"SECS\":[3,\"{bsec.block.args.Id}\",[4,\"0\"]]", this, bsec.block);
				else if(sec is Var varsec) arg3 = $"\"SECS\":[3,[12,\"{varsec.Name}\",\"{varsec.Id}\"],[4,\"0\"]]";
				else arg3 = $"\"SECS\":[1,[4,\"{sec}\"]]";

				args.Inputs = $"{arg3},{arg1},{arg2}";
			}

			public Glide(object sec, object to) : base("Goto To", sec)
			{
				if(to == null) throw new ArgumentException("to cannot be null");
				else if(TypeCheck.Check(to) != AcceptedTypes.None) throw new ArgumentException("to is not a sprite or To element");

				string arg;
				if(to is Sprite s) arg = $"\"TO\":[\"{s.name}\",null]";
				else if(to is To t) arg = (t == To.Mouse) ? "\"TO\":[\"_mouse_\",null]" : "\"TO\":[\"_random_\",null]";
				else throw new ArgumentException("to cannot be the Stage");

				args = new BlockArgs("motion_glideto");
				Block tmp = new Block(null)
				{
					args = new BlockArgs("motion_glideto_menu", null, arg, null, null, true)
				};
				tmp.args.ParentId = args.Id;
				kids.Add(tmp);

				string arg3;
				if(sec is MyBlock.MyBlockVar bsec) arg3 = MyBlockVarArg($"\"SECS\":[3,\"{bsec.block.args.Id}\",[4,\"0\"]]", this, bsec.block);
				else if(sec is Var varsec) arg3 = $"\"SECS\":[3,[12,\"{varsec.Name}\",\"{varsec.Id}\"],[4,\"0\"]]";
				else arg3 = $"\"SECS\":[1,[4,\"{sec}\"]]";

				args.Inputs = $"{arg3},\"TO\":[1,\"{tmp.args.Id}\"]";
			}
		}

		public class Move : Block
		{
			public Move(object steps) : base("Move steps", steps)
			{
				if(TypeCheck.Check(steps) == AcceptedTypes.String) throw new ArgumentException($"steps is string, which is not accepted");

				args = new BlockArgs("motion_movesteps");

				string arg1;
				if(steps is MyBlock.MyBlockVar bx) arg1 = MyBlockVarArg($"\"STEPS\":[3,\"{bx.block.args.Id}\",[4,\"0\"]]", this, bx.block);
				else if(steps is Var varx) arg1 = $"\"STEPS\":[3,[12,\"{varx.Name}\",\"{varx.Id}\"],[4,\"0\"]]";
				else arg1 = $"\"STEPS\":[1,[4,\"{steps}\"]]";

				args.Inputs = arg1;
			}
		}

		public static class Change
		{
			public class X : Block
			{
				public X(object by) : base("Change X by", by)
				{
					if(TypeCheck.Check(by) == AcceptedTypes.String) throw new ArgumentException($"by is string, which is not accepted");

					args = new BlockArgs("motion_changexby");

					string arg1;
					if(by is MyBlock.MyBlockVar bx) arg1 = MyBlockVarArg($"\"DX\":[3,\"{bx.block.args.Id}\",[4,\"0\"]]", this, bx.block);
					else if(by is Var varx) arg1 = $"\"DX\":[3,[12,\"{varx.Name}\",\"{varx.Id}\"],[4,\"0\"]]";
					else arg1 = $"\"DX\":[1,[4,\"{by}\"]]";

					args.Inputs = arg1;
				}
			}

			public class Y : Block
			{
				public Y(object by) : base("Change Y by", by)
				{
					if(TypeCheck.Check(by) == AcceptedTypes.String) throw new ArgumentException($"by is string, which is not accepted");

					args = new BlockArgs("motion_changeyby");

					string arg1;
					if(by is MyBlock.MyBlockVar bx) arg1 = MyBlockVarArg($"\"DY\":[3,\"{bx.block.args.Id}\",[4,\"0\"]]", this, bx.block);
					else if(by is Var varx) arg1 = $"\"DY\":[3,[12,\"{varx.Name}\",\"{varx.Id}\"],[4,\"0\"]]";
					else arg1 = $"\"DY\":[1,[4,\"{by}\"]]";

					args.Inputs = arg1;
				}
			}
		}

		public static class Set
		{
			public class X : Block
			{
				public X(object value) : base("Change X by", value)
				{
					if(TypeCheck.Check(value) == AcceptedTypes.String) throw new ArgumentException($"value is string, which is not accepted");

					args = new BlockArgs("motion_setx");

					string arg1;
					if(value is MyBlock.MyBlockVar bx) arg1 = MyBlockVarArg($"\"X\":[3,\"{bx.block.args.Id}\",[4,\"0\"]]", this, bx.block);
					else if(value is Var varx) arg1 = $"\"X\":[3,[12,\"{varx.Name}\",\"{varx.Id}\"],[4,\"0\"]]";
					else arg1 = $"\"X\":[1,[4,\"{value}\"]]";

					args.Inputs = arg1;
				}
			}

			public class Y : Block
			{
				public Y(object value) : base("Change Y value", value)
				{
					if(TypeCheck.Check(value) == AcceptedTypes.String) throw new ArgumentException($"value is string, which is not accepted");

					args = new BlockArgs("motion_sety");

					string arg1;
					if(value is MyBlock.MyBlockVar bx) arg1 = MyBlockVarArg($"\"Y\":[3,\"{bx.block.args.Id}\",[4,\"0\"]]", this, bx.block);
					else if(value is Var varx) arg1 = $"\"Y\":[3,[12,\"{varx.Name}\",\"{varx.Id}\"],[4,\"0\"]]";
					else arg1 = $"\"Y\":[1,[4,\"{value}\"]]";

					args.Inputs = arg1;
				}
			}
		}

		public static class Turn
		{
			public class Left : Block
			{
				public Left(object degrees) : base("Change X by", degrees)
				{
					if (TypeCheck.Check(degrees) == AcceptedTypes.String) throw new ArgumentException($"degrees is string, which is not accepted");

					args = new BlockArgs("motion_turnleft");

					string arg1;
					if (degrees is MyBlock.MyBlockVar bx) arg1 = MyBlockVarArg($"\"DEGREES\":[3,\"{bx.block.args.Id}\",[4,\"0\"]]", this, bx.block);
					else if (degrees is Var varx) arg1 = $"\"DEGREES\":[3,[12,\"{varx.Name}\",\"{varx.Id}\"],[4,\"0\"]]";
					else arg1 = $"\"DEGREES\":[1,[4,\"{degrees}\"]]";

					args.Inputs = arg1;
				}
			}

			public class Right : Block
			{
				public Right(object degrees) : base("Change Y by", degrees)
				{
					if (TypeCheck.Check(degrees) == AcceptedTypes.String) throw new ArgumentException($"degrees is string, which is not accepted");

					args = new BlockArgs("motion_turnright");

					string arg1;
					if (degrees is MyBlock.MyBlockVar bx) arg1 = MyBlockVarArg($"\"DEGREES\":[3,\"{bx.block.args.Id}\",[4,\"0\"]]", this, bx.block);
					else if (degrees is Var varx) arg1 = $"\"DEGREES\":[3,[12,\"{varx.Name}\",\"{varx.Id}\"],[4,\"0\"]]";
					else arg1 = $"\"DEGREES\":[1,[4,\"{degrees}\"]]";

					args.Inputs = arg1;
				}
			}
		}

		public class Point : Block
		{
			public Point(object to) : base("Point to/in direction")
			{
				if (to is Sprite || to is To)
				{
					if (TypeCheck.Check(to) != AcceptedTypes.None) throw new ArgumentException("to is not a sprite or To element");

					string arg;
					if (to is Sprite s) arg = $"\"TOWARDS\":[\"{s.name}\",null]";
					else if (to is To t)
					{
						if (t == To.Random) throw new ArgumentException("to cannot be random in this block");
						arg = "\"TOWARDS\":[\"_mouse_\",null]";
					}
					else throw new ArgumentException("to cannot be the Stage");

					args = new BlockArgs("motion_pointtowards");
					Block tmp = new Block(null)
					{
						args = new BlockArgs("motion_pointtowards_menu", null, arg, null, null, true)
					};
					tmp.args.ParentId = args.Id;
					kids.Add(tmp);

					args.Inputs = $"\"TOWARDS\":[1,\"{tmp.args.Id}\"]";
				}
				else
				{
					if (TypeCheck.Check(to) == AcceptedTypes.String) throw new ArgumentException($"to is string, which is not accepted");

					args = new BlockArgs("motion_pointindirection");

					string arg1;
					if (to is MyBlock.MyBlockVar bx) arg1 = MyBlockVarArg($"\"DIRECTION\":[3,\"{bx.block.args.Id}\",[4,\"0\"]]", this, bx.block);
					else if (to is Var varx) arg1 = $"\"DIRECTION\":[3,[12,\"{varx.Name}\",\"{varx.Id}\"],[4,\"0\"]]";
					else arg1 = $"\"DIRECTION\":[1,[4,\"{to}\"]]";

					args.Inputs = arg1;
				}
			}
		}
	}	
}