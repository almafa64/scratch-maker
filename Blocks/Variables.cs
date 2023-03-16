using Scratch_Utils;
using System;

namespace Scratch
{
	public static class Variables
	{
		private static string MakeVarField(Var v)
		{
			if(v is MyBlock.MyBlockVar) throw new ArgumentException($"Variable {v.Name} is not user made, so it cannot modify");
			return Block.MakeField("VARIABLE", $"\"{v.Name}\",\"{v.Id}\"", false);
		}

		public class Change : Block
		{
			public Change(Var variable, object by) : base("Change size", UsagePlace.Both, by)
			{
				if (TypeCheck.Check(by) == AcceptedTypes.String) throw new ArgumentException("by is string, which is not accepted");

				args = new BlockArgs("data_changevariableby", MakeInput("VALUE", by), MakeVarField(variable));
			}
		}

		public class Set : Block
		{
			public Set(Var variable, object to) : base("Set size", UsagePlace.Both, to)
			{
				if (TypeCheck.Check(to) == AcceptedTypes.String) throw new ArgumentException("to is string, which is not accepted");

				args = new BlockArgs("data_setvariableto", MakeInput("VALUE", to), MakeVarField(variable));
			}
		}

		public class Show : Block
		{
			public Show(Var variable) : base("Show variable")
			{
				args = new BlockArgs("data_showvariable", null, MakeVarField(variable));
			}
		}

		public class Hide : Block
		{
			public Hide(Var variable) : base("Hide variable")
			{
				args = new BlockArgs("data_hidevariable", null, MakeVarField(variable));
			}
		}
	}
}