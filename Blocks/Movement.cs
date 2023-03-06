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
				if(x is MyBlock.MyBlockVar bx) arg1 = MyBlockVarArg($"\"X\":[3,\"{bx.block.args.Id}\",[4,\"36\"]]", this, bx.block);
				else if(x is Var varx) arg1 = $"\"X\":[3,[12,\"{varx.Name}\",\"{varx.Id}\"]]";
				else arg1 = $"\"X\":[1,[4,\"{x}\"]]";

				string arg2;
				if(y is MyBlock.MyBlockVar by) arg2 = MyBlockVarArg($"\"Y\":[3,\"{by.block.args.Id}\",[4,\"36\"]]", this, by.block);
				else if(y is Var vary) arg2 = $"\"Y\":[3,[12,\"{vary.Name}\",\"{vary.Id}\"]]";
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
				if(x is MyBlock.MyBlockVar bx) arg1 = MyBlockVarArg($"\"X\":[3,\"{bx.block.args.Id}\",[4,\"36\"]]", this, bx.block);
				else if(x is Var varx) arg1 = $"\"X\":[3,[12,\"{varx.Name}\",\"{varx.Id}\"]]";
				else arg1 = $"\"X\":[1,[4,\"{x}\"]]";

				string arg2;
				if(y is MyBlock.MyBlockVar by) arg2 = MyBlockVarArg($"\"Y\":[3,\"{by.block.args.Id}\",[4,\"36\"]]", this, by.block);
				else if(y is Var vary) arg2 = $"\"Y\":[3,[12,\"{vary.Name}\",\"{vary.Id}\"]]";
				else arg2 = $"\"Y\":[1,[4,\"{y}\"]]";

				string arg3;
				if(sec is MyBlock.MyBlockVar bsec) arg3 = MyBlockVarArg($"\"SECS\":[3,\"{bsec.block.args.Id}\",[4,\"36\"]]", this, bsec.block);
				else if(sec is Var varsec) arg3 = $"\"SECS\":[3,[12,\"{varsec.Name}\",\"{varsec.Id}\"]]";
				else arg3 = $"\"SECS\":[1,[4,\"{sec}\"]]";

				args.Inputs = $"{arg3},{arg1},{arg2}";
			}

			public Glide(object sec, object to) : base("Goto To", sec)
			{
				if(TypeCheck.Check(sec) == AcceptedTypes.String) throw new ArgumentException($"sec is string, which is not accepted");

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

				args.Inputs = $"\"SECS\":[1,[4,\"{sec}\"]],\"TO\":[1,\"{tmp.args.Id}\"]";
			}
		}
	}
}