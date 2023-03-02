using Scratch_Utils;
using System;

namespace Scratch_Utils
{
	public struct BlockArgs
	{
		internal string Id;
		internal string NextId;
		internal string ParentId;
		internal string OpCode;
		internal string Inputs;
		internal string Fields;
		internal bool Shadow;
		internal bool TopLevel;
		public double X;
		public double Y;

		internal BlockArgs(string opCode, string inputs = null, string fields = null, string nextId = null, string parentId = null, bool topLevel = false, double x = 500, double y = 500, bool shadow = false)
		{
			Id = ID.Make();
			NextId = nextId;
			ParentId = parentId;
			OpCode = opCode;
			Shadow = shadow;
			TopLevel = topLevel;
			X = x;
			Y = y;
			Inputs = inputs;
			Fields = fields;
		}
	}

	public class Block
	{
		public BlockArgs args;
	}

	[Flags]
	internal enum AcceptedTypes
	{
		None = 0,
		Number = 1,
		String = 2,
		Variable = 4
	}
}

namespace Scratch
{
	public static class Movement 
	{
		public class Goto : Block
		{
			public Goto(object x, object y)
			{
				if(TypeCheck.Check(x) == AcceptedTypes.String) throw new ArgumentException($"x was string, which is not accepted"); 
				if(TypeCheck.Check(y) == AcceptedTypes.String) throw new ArgumentException($"y was string, which is not accepted"); 
				args = new BlockArgs("motion_gotoxy", $"\"X\":[1,[4,\"{x}\"}}]],\"Y\":[1,[4,\"{y}\"}}]]");
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

	public static class MyBlock
	{
		//hard
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