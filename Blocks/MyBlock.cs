using Scratch_Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Scratch
{
	public sealed class MyBlock : TopBlock
	{
		public sealed class Call : Block
		{
			public Call(MyBlock myBlock, params object[] parameters) : base("Call myBlock")
			{
				if(parameters.Length != myBlock.paramBlocks.Count) throw new ArgumentException($"parameters length ({parameters.Length}) does not match with myBlock parameters length ({myBlock.paramBlocks.Count})");

				Mutator mMy = myBlock.prototype.args.Mutatator.Value;

				Mutator m = new Mutator
				{
					argumentIds = mMy.argumentIds,
					proCode = mMy.proCode,
					warp = mMy.warp,
				};
				StringBuilder sb = new StringBuilder();

				string[] arr = myBlock.prototype.args.Inputs.Split('"');
				for(int i = 1, j = 0; i < arr.Length; i+=4, j++)
				{
					sb.Append(MakeInput(arr[i], parameters[j], $"parameter at index {j}") + ",");
				}

				Utils.RemoveLast(sb);

				args = new BlockArgs("procedures_call", sb.ToString(), null, null, null, false, false, m);
			}
		}

		internal Dictionary<string, MyBlockVar> parameters;
		internal Block prototype;
		internal List<Block> paramBlocks = new List<Block>();

		internal bool made = false;

		public MyBlock(SObject sObject, string name, int x = 200, int y = 200) : base("procedures_definition", "MyBlock", sObject, x, y, false)
		{
			sObject._MyBlocks[name] = this;

			parameters = new Dictionary<string, MyBlockVar>
			{
				[name] = new MyBlockVar(name, null)
			};

			prototype = new Block(null)
			{
				args = new BlockArgs("procedures_prototype", null, null, null, mainBlock.args.Id, true, false, new Mutator()),
				needsNext = false
			};

			mainBlock.args.Inputs = $"\"custom_block\":[1,\"{prototype.args.Id}\"]";

			blocks.Add(prototype);
			blocks.Add(mainBlock);

			Update();
		}

		public MyBlock AddDesc(string name, string text)
		{
			MyBlockVar tmp = new MyBlockVar(text, null);
			tmp.block.needsNext = false;
			parameters[name] = tmp;

			Update();
			return this;
		}

		private void ArgBlocker(string name, string opcode)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("\"VALUE\":[\"");
			sb.Append(name);
			sb.Append("\",null]");
			string field = sb.ToString();

			Block argBlock = new Block(null)
			{
				args = new BlockArgs(opcode, null, field, null, prototype.args.Id, true),
				needsNext = false,
			};

			blocks.Add(argBlock);
			paramBlocks.Add(argBlock);

			Update();
		}

		public MyBlock AddBool(string name)
		{
			MyBlockVar tmp = new MyBlockVar("%b", "argument_reporter_boolean", name);
			tmp.block.needsNext = false;
			parameters[name] = tmp;

			ArgBlocker(name, "argument_reporter_boolean");

			return this;
		}

		public MyBlock AddValue(string name)
		{
			MyBlockVar tmp = new MyBlockVar("%s", "argument_reporter_string_number", name);
			tmp.block.needsNext = false;
			parameters[name] = tmp;

			ArgBlocker(name, "argument_reporter_string_number");

			return this;
		}

		public MyBlockVar this[string name]
		{
			get
			{
				MyBlockVar tmp = parameters[name];
				if(((string)tmp.value).Contains("%")) return new MyBlockVar(tmp.value, tmp.block.args.OpCode, tmp.Name);
				else throw new ArgumentException($"Varibale {name} doesn't exists in {mainBlock.name} block.");
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

			if(paramBlocks.Count > 0) Utils.RemoveLast(sb5);

			m.Close(sb1, sb2, sb3, sb4);

			prototype.args.Inputs = sb5.ToString();
			prototype.args.Mutatator = m;
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