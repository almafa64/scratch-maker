using Scratch;
using Scratch_Utils;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using static Scratch.Movement;

namespace Scratch_Utils
{
	internal enum InputType // from https://en.scratch-wiki.info/wiki/Scratch_File_Format#Blocks
	{
		Number = 4,				//value
		PositiveNumber = 5,		//null
		PositiveInteger = 6,	//null
		Integer = 7,			//null
		Angle = 8,				//null
		Color = 9,				//hex color (#abcdef)
		String = 10,			//value
		Broadcast = 11,			//name, ID
		Var = 12,				//null, null, x, y
		List = 13				//null
	}

	internal enum ShadowType
	{
		/* from https://github.com/LLK/scratch-vm/blob/e5950c3/src/serialization/sb3.js#L39-L41
		const INPUT_SAME_BLOCK_SHADOW = 1; // unobscured shadow
		const INPUT_BLOCK_NO_SHADOW = 2; // no shadow
		const INPUT_DIFF_BLOCK_SHADOW = 3; // obscured shadow
		*/
		Shadow = 1,
		NoShadow = 2,
		ObscureShadow = 3
	}

	public struct Mutator
	{
		internal string argumentDefaults;
		internal string argumentIds;
		internal string argumentNames;
		internal string proCode;
		
		public bool warp;

		internal void Reset()
		{
			argumentDefaults = "[";
			argumentIds = "[";
			argumentNames = "[";
			proCode = null;
			warp = false;
		}

		internal void Close(StringBuilder defaults, StringBuilder ids, StringBuilder names, StringBuilder procode)
		{
			if(defaults.Length > 0) Utils.RemoveLast(defaults);
			if(names.Length > 0) Utils.RemoveLast(names);
			if(procode.Length > 0) Utils.RemoveLast(procode);
			if(ids.Length > 0) Utils.RemoveLast(ids);

			argumentDefaults += $"{defaults}]";
			argumentIds += $"{ids}]";
			argumentNames += $"{names}]";
			proCode = procode.ToString();
		}
	}

	public struct BlockArgs
	{
		internal string Id;
		internal string NextId;
		internal string ParentId;
		internal string OpCode;
		internal string Inputs;
		internal string Fields;
		internal Mutator? Mutatator;
		internal bool Shadow;
		internal bool TopLevel;

		internal BlockArgs(string opCode, string inputs = null, string fields = null, string nextId = null, string parentId = null, bool shadow = false, bool topLevel = false, Mutator? mutatator = null)
		{
			Id = ID.Make();
			NextId = nextId;
			ParentId = parentId;
			OpCode = opCode;
			Shadow = shadow;
			TopLevel = topLevel;
			Inputs = inputs;
			Fields = fields;
			Mutatator = mutatator;
		}
	}

	public class TopBlock : Column
	{
		public TopBlock(SObject sprite, int x = 200, int y = 200) : base(sprite, x, y){}
	}

	public class Block
	{
		internal bool needsNext = true;
		internal string name;
		internal BlockArgs args;
		internal List<Block> kids = new List<Block>();
		internal UsagePlace usagePlace = UsagePlace.Both;

		internal Block(string name, params object[] vals)
		{
			this.name = name;
			for(int i = 0; i < vals.Length; i++)
			{
				object o = vals[i];
				switch(TypeCheck.Check(o))
				{
					case AcceptedTypes.None:
						throw new ArgumentException($"Argument is null at place {i} on block {name}");
					case AcceptedTypes.Variable:
						if((o as Var).value == null) throw new ArgumentException($"Not initalized variable at place {i} on block {name}");
						break;
					case AcceptedTypes.List:
						if((o as List).vars == null) throw new ArgumentException($"Not initalized list at place {i} on block {name}");
						break;
				}
			}
		}

		public void PlaceIn(Column col)
		{
			col.Add(this);
		}

		internal static string VarBlockId(string type, Block mainBlock, Block varBlock)
		{
			varBlock.args.ParentId = mainBlock.args.Id;
			mainBlock.kids.Add(varBlock);
			return $"\"{type}\":[3,\"{varBlock.args.Id}\",[4,\"0\"]]";
		}

		internal static SpecVar GetVar(Dictionary<string, SpecVar> dir, Type enumType, object value)
		{
			return dir[Enum.GetName(enumType, value)];
		}

		internal static void BuiltInVars(ref object val)
		{
			if(val is Movement.Vars vM) val = GetVar(Movement.specVars, typeof(Movement.Vars), vM);
			else if(val is Looks.Vars vL) val = GetVar(Looks.specVars, typeof(Looks.Vars), vL);
		}
	}

	internal class SpecVar : Block
	{
		internal SpecVar(UsagePlace usagePlace, string opcode, string name = null, string field = null, string input = null, params object[] vals) : base(name, vals)
		{
			args = new BlockArgs(opcode, input, field);
			this.usagePlace = usagePlace;
		}
	}

	/*internal class SpecBlock : SpecVar
	{
		internal Block main;
		internal SpecBlock(UsagePlace usagePlace, string name, params object[] vals) : base(usagePlace, name, null, null, null, vals)
		{
			
		}
	}*/

	[Flags]
	internal enum AcceptedTypes
	{
		None = 0,
		Number = 1,
		String = 2,
		Variable = 4,
		List = 8,
		Enum = 16,
		Sprite = 32,
		MyBlockVar = 64,
	}

	internal enum UsagePlace
	{
		Sprite,
		Background,
		Both
	}
}