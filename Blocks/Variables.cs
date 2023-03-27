using Scratch_Utils;
using System;

namespace Scratch
{
	public static class Variables
	{
		private static string MakeVarField(Var v)
		{
			if(v is MyBlock.MyBlockVar) throw new ArgumentException($"Variable {v.Name} is not user made, so it cannot be modify");
			return Block.MakeField("VARIABLE", $"\"{v.Name}\",\"{v.Id}\"", false);
		}

		public sealed class Change : Block
		{
			public Change(Var variable, object by) : base("Change size", UsagePlace.Both, by)
			{
				args = new BlockArgs("data_changevariableby", MakeInput("VALUE", by, "by"), MakeVarField(variable));
			}
		}

		public sealed class Set : Block
		{
			public Set(Var variable, object to) : base("Set size", UsagePlace.Both, to)
			{
				args = new BlockArgs("data_setvariableto", MakeInput("VALUE", to, "to"), MakeVarField(variable));
			}
		}

		public sealed class Show : Block
		{
			public Show(Var variable) : base("Show variable")
			{
				args = new BlockArgs("data_showvariable", null, MakeVarField(variable));
			}
		}

		public sealed class Hide : Block
		{
			public Hide(Var variable) : base("Hide variable")
			{
				args = new BlockArgs("data_hidevariable", null, MakeVarField(variable));
			}
		}
	}
}