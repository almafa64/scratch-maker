using Scratch;
using System;
using System.Security.Cryptography.X509Certificates;
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

	public class TopBlock : Column
	{
		public TopBlock(SObject sprite, int x = 200, int y = 200) : base(sprite, x, y){}
	}

	public class Block
	{
		internal bool needsNext = true;
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

		internal static string MyBlockVarArg(string text, Block mainBlock, Block myBlock)
		{
			myBlock.args.ParentId = mainBlock.args.Id;
			return text;
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