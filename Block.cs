using Scratch;
using Scratch_Utils;
using System;
using System.Collections.Generic;
using System.Text;

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
		internal object hasNext;

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

			hasNext = null;
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
		internal Block mainBlock;
		public TopBlock(string opcode, string name, SObject sprite, int x = 200, int y = 200, bool addMain = true) : base(sprite, x, y)
		{
			mainBlock = new Block(name)
			{
				args = new BlockArgs(opcode, null, null, null, null, false, true)
			};
			if(addMain) blocks.Add(mainBlock);
		}

		public TopBlock AddComment(Comment comment)
		{
			if(mainBlock.comment != null) throw new ArgumentException($"Comment already assigned to mainBlock Block with text \"{mainBlock.comment.text}\"");
			mainBlock.comment = comment;
			comment.block = mainBlock;
			return this;
		}

		public TopBlock AddComment(string text, bool minimized = false, int height = 200, int width = 200)
		{
			if(mainBlock.comment != null) throw new ArgumentException($"Comment already assigned to mainBlock Block with text \"{mainBlock.comment.text}\"");
			return AddComment(new Comment(text, 0, 0, minimized, height, width));
		}
	}

	public class Block
	{
		internal bool needsNext = true;
		internal string name;
		internal Comment comment;
		internal BlockArgs args;
		internal List<Block> kids = new List<Block>();
		internal UsagePlace usagePlace = UsagePlace.Both;

		internal Block(string name, UsagePlace usagePlace = UsagePlace.Both, params object[] vals)
		{
			this.usagePlace = usagePlace;
			this.name = name;
			for(int i = 0; i < vals.Length; i++)
			{
				object val = vals[i];
				if(val is SpecVar) continue;
				TypeCheck.BaseCheck(val, name, i);
			}
		}

		public Block PlaceIn(Column col)
		{
			return col.Add(this);
		}

		private readonly static string[] randomTexts = { "apple", "orange", "hello", "texting", "lol", "XD", "cat", "banana", "scratch>js", "VALVe" };
		private readonly static Random rd = new Random();

		internal string MakeInput(string name, object val, string valName, AcceptedTypes notAcceptedTypes = AcceptedTypes.String, bool isString = false)
		{
			BuiltInVars(ref val);

			string def = isString ? $"10,\"{randomTexts[rd.Next(randomTexts.Length)]}\"" : "4,\"0\"";
			if(val is SpecVar sv) return VarBlockId(name, this, sv, def);
			else if(val is MyBlock.MyBlockVar bv) return VarBlockId(name, this, bv.block, def);

			TypeCheck.Check(this.name, valName, val, notAcceptedTypes);
			 if(val is Var v) return $"\"{name}\":[3,[12,\"{v.Name}\",\"{v.Id}\"],[{def}]]";
			return $"\"{name}\":[1,[{(isString ? "10" : "4")},\"{val}\"]]";
		}

		internal static string MakeField(string name, string data, bool needQuote = true)
		{
			return needQuote ? $"\"{name}\":[\"{data}\",null]" : $"\"{name}\":[{data},null]";
		}

		internal static string MakeEffectField(string data)
		{
			return MakeField("EFFECT", data);
		}

		internal static string VarBlockId(string type, Block mainBlock, Block varBlock, string def = "4,\"0\"")
		{
			varBlock.args.ParentId = mainBlock.args.Id;
			varBlock.needsNext = false;
			mainBlock.kids.Add(varBlock);
			return $"\"{type}\":[3,\"{varBlock.args.Id}\",[{def}]]";
		}

		internal static SpecVar GetVar(Dictionary<string, SpecVar> dir, Type enumType, object value)
		{
			return dir[enumType.GetEnumName(value)];
		}

		internal static SpecVar GetVar<T>(Dictionary<string, SpecVar> dir, object value)
		{
			return dir[typeof(T).GetEnumName(value)];
		}

		internal static void BuiltInVars(ref object val)
		{
			if(val is Movement.Vars vM) val = GetVar<Movement.Vars>(Movement.specVars, vM);
			else if(val is Looks.Vars vL) val = GetVar<Looks.Vars>(Looks.specVars, vL);
			else if(val is Sounds.Vars vS) val = GetVar<Sounds.Vars>(Sounds.specVars, vS);
		}

		public Block AddComment(Comment comment)
		{
			if(this.comment != null) throw new ArgumentException($"Comment already assigned to this Block with text \"{this.comment.text}\"");
			this.comment = comment;
			comment.block = this;
			return this;
		}

		public Block AddComment(string text, bool minimized = false, int height = 200, int width = 200)
		{
			if(this.comment != null) throw new ArgumentException($"Comment already assigned to this Block with text \"{this.comment.text}\"");
			return AddComment(new Comment(text, 0, 0, minimized, height, width));
		}
	}

	public class SpecVar : Block
	{
		internal SpecVar(UsagePlace usagePlace, string opcode, string name = null, string field = null, string input = null, params object[] vals) : base(name, usagePlace, vals)
		{
			args = new BlockArgs(opcode, input, field);
			this.usagePlace = usagePlace;
		}
	}

	public class SpecBlock : SpecVar
	{
		internal SpecBlock(string name, UsagePlace usagePlace = UsagePlace.Both, params object[] vals) : base(usagePlace, name, null, null, null, vals)
		{
			
		}
	}

	[Flags]
	internal enum AcceptedTypes
	{
		None = 0,
		Number = 1,
		String = 2,
		Variable = 4,
		ListElement = 8,
		Enum = 16,
		Sprite = 32,
		MyBlockVar = 64,
		PositiveNumber = 128,
	}

	internal enum UsagePlace
	{
		Sprite,
		Background,
		Both
	}
}