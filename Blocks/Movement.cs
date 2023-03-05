using Scratch_Utils;
using System;

namespace Scratch
{
	public static class Movement 
	{
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
		}
	}
}