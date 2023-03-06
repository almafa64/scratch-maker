using Scratch_Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scratch
{
	public class MyBlock : TopBlock
	{
		internal Dictionary<string, MyBlockVar> parameters;
		internal Block prototype;
		internal Block def;
		internal List<Block> paramBlocks;

		internal bool made = false;

		public MyBlock(SObject sObject, string name, int x = 200, int y = 200) : base(sObject, x, y)
		{
			sObject._MyBlocks[name] = this;

			def = new Block(name)
			{
				args = new BlockArgs("procedures_definition", null, null, null, null, false, true)
			};
			paramBlocks = new List<Block>();
            parameters = new Dictionary<string, MyBlockVar>
            {
                [name] = new MyBlockVar(name, null)
            };

            prototype = new Block(null)
			{
				args = new BlockArgs("procedures_prototype", null, null, null, def.args.Id, true, false, new Mutator())
			};

			def.args.Inputs = $"\"custom_block\":[1,\"{prototype.args.Id}\"]";

			blocks.Add(prototype);

			Update();
		}

		public MyBlock Build()
		{
			made = true;
			blocks.Add(def);
			return this;
		}

		public MyBlock AddDesc(string name, string text)
		{
			if(made) throw new Exception($"Block \"{def.name}\" is builded, so no more variable can be added!");
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

			blocks.Add(argBlock);
			paramBlocks.Add(argBlock);
		}

		public MyBlock AddBool(string name)
		{
			if(made) throw new Exception($"Block \"{def.name}\" is builded, so no more variable can be added!");
			parameters[name] = new MyBlockVar("%b", "argument_reporter_boolean", name);

			ArgBlocker(name, "argument_reporter_boolean");

			Update();
			return this;
		}

		public MyBlock AddValue(string name)
		{
			if(made) throw new Exception($"Block \"{def.name}\" is builded, so no more variable can be added!");
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
				if(((string)tmp.value).Contains("%"))
				{
					MyBlockVar newVar = new MyBlockVar(tmp.value, tmp.block.args.OpCode, tmp.Name);
					blocks.Add(newVar.block);
					return newVar;
				}

				throw new ArgumentException($"Varibale {name} doesn't exists in {def.name} block.");
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

		public new void Add(Block block)
		{
			if(!made) throw new Exception($"Block \"{def.name}\" is not builded!");
			base.Add(block);
		}

		public class MyBlockVar : Var
		{
			internal Block block;

			internal MyBlockVar(object value, string opcode, string name = null) : base(value)
			{
				Name = name;
				string field = $"\"VALUE\":[\"{name}\",null]";
				block = new Block(null)
				{
					args = new BlockArgs(opcode, null, field),
					needsNext = false,
				};
			}
		}
	}
}