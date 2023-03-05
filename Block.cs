using Scratch;
using Scratch_Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Scratch_Utils
{
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
			if(defaults.Length > 0) Compiler.RemoveLast(defaults);
			if(names.Length > 0) Compiler.RemoveLast(names);
			if(procode.Length > 0) Compiler.RemoveLast(procode);
			if(ids.Length > 0) Compiler.RemoveLast(ids);

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

	public interface ITopBlock
	{

	}

	public class Block
	{
		internal string name;
		internal BlockArgs args;
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
	}

	[Flags]
	internal enum AcceptedTypes
	{
		None = 0,
		Number = 1,
		String = 2,
		Variable = 4,
		List = 8,
		BlockVar = 16,
	}
}

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

				string arg1;
				if(x is MyBlock.MyBlockVar bx)
				{
					arg1 = $"\"X\":[3,{bx.Id}[4,\"36\"]]";
				}
				else if(x is Var varx)
				{
					arg1 = $"\"X\":[3,[12,\"{varx.Name}\",\"{varx.Id}\"]]";
				}
				else
				{
					arg1 = $"\"X\":[1,[4,\"{x}\"]]";
				}

				string arg2;
				if(y is MyBlock.MyBlockVar by)
				{
					arg2 = $"\"Y\":[3,{by.Id}[4,\"36\"]]";
				}
				else if(y is Var vary)
				{
					arg2 = $"\"Y\":[3,[12,\"{vary.Name}\",\"{vary.Id}\"]]";
				}
				else
				{
					arg2 = $"\"Y\":[1,[4,\"{y}\"]]";
				}

				args = new BlockArgs("motion_gotoxy", $"{arg1},{arg2}");
			}
		}
	}

	public static class Looks
	{

	}

	public static class Sounds
	{

	}

	public static class Events
	{

	}

	public static class Control
	{

	}

	public static class Sensing
	{

	}

	public static class Operators
	{

	}

	public static class Variables
	{

	}

	public class MyBlock : Block, ITopBlock //Column?
	{
		internal Dictionary<string, MyBlockVar> parameters;
		internal Block prototype;
		internal List<Block> paramBlocks;

		public MyBlock(string name) : base($"My Block - {name}")
		{
			paramBlocks = new List<Block>();
			parameters = new Dictionary<string, MyBlockVar>();

			args = new BlockArgs("procedures_definition");

			Mutator m = new Mutator();

			prototype = new Block(null)
			{
				args = new BlockArgs("procedures_prototype", null, null, null, null, true, false, m)
			};
			parameters[name] = new MyBlockVar(name, null);
			prototype.args.ParentId = args.Id;

			args.Inputs = $"\"custom_block\":[1,\"{prototype.args.Id}\"]";

			Update();
		}

		public MyBlock AddDesc(string name, string text)
		{
			parameters[name] = new MyBlockVar(text, null);
			Update();
			return this;
		}

		internal void ArgBlocker(string name, string opcode)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("\"VALUE\":[\"");
			sb.Append(name);
			sb.Append("\",null]");
			string field = sb.ToString();

			Block argBlock = new Block(null)
			{
				args = new BlockArgs(opcode, null, field, null, prototype.args.Id, true)
			};

			paramBlocks.Add(argBlock);
		}

		public MyBlock AddBool(string name)
		{
			parameters[name] = new MyBlockVar("%b", "argument_reporter_boolean", name);

			ArgBlocker(name, "argument_reporter_boolean");

			Update();
			return this;
		}

		public MyBlock AddValue(string name)
		{
			parameters[name] = new MyBlockVar("%s", "argument_reporter_string_number", name);

			ArgBlocker(name, "argument_reporter_string_number");
			
			Update();
			return this;
		}

		public MyBlockVar this[string name]
		{
			get
			{
				MyBlockVar tmp = parameters[name];
				if(((string)tmp.value).Contains("%")) return tmp;

				throw new ArgumentException($"Varibale {name} doesn't exists in {this.name} block.");
			}
		}

		internal void Update()
		{
			Mutator m = prototype.args.Mutatator.Value;
			m.Reset();

			int notVar = 0;

			StringBuilder sb1 = new StringBuilder();
			StringBuilder sb2 = new StringBuilder();
			StringBuilder sb3 = new StringBuilder();
			StringBuilder sb4 = new StringBuilder();
			StringBuilder sb5 = new StringBuilder();

			for(int i = 0; i < parameters.Count; i++)
			{
				KeyValuePair<string, MyBlockVar> p = parameters.ElementAt(i);
				MyBlockVar par = p.Value;

				if(((string)par.value).Contains("%"))
				{
					//defaults
					sb1.Append(((string)par.value == "%b") ? @"\""false\""," : @"\""\"",");
				
					//ids
					sb2.Append(@"\""");
					sb2.Append(par.Id);
					sb2.Append(@"\"",");
				
					//names
					sb3.Append(@"\""");
					sb3.Append(p.Key);
					sb3.Append(@"\"",");

					//inputs
					sb5.Append('"');
					sb5.Append(par.Id);
					sb5.Append("\":[1,\"");
					sb5.Append(paramBlocks[i - notVar].args.Id);
					sb5.Append("\"],");
				}
				else notVar++;

				//procode
				sb4.Append((string)par.value);
				sb4.Append(' ');
			}

			if(paramBlocks.Count > 0) Compiler.RemoveLast(sb5);

			m.Close(sb1, sb2, sb3, sb4);

			prototype.args.Inputs = sb5.ToString();
			prototype.args.Mutatator = m;
		}

		public class MyBlockVar : Var
		{
			internal Block block;

			internal MyBlockVar(object value, string opcode, string name = null) : base(value)
			{
				string field = $"\"VALUE\":[\"{name}\",null]";
				block = new Block(null)
				{
					args = new BlockArgs(opcode, null, field)
				};
			}
		}
	}

	public static class TextToSpeech
	{

	}

	public static class Pen
	{

	}

	public static class ForceAndAcc
	{

	}

	public static class WeDo2
	{

	}

	public static class Boost
	{

	}

	public static class EV3
	{

	}

	public static class Microbit
	{

	}

	public static class MakeyMakey
	{

	}

	public static class Translate
	{

	}

	public static class Video
	{

	}

	public static class Music
	{

	}
}