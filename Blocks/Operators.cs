using Scratch_Utils;
using System;

namespace Scratch
{
	public static class Operators
	{
		private static string MakeNumInput(Block b, object data1, object data2)
		{
			return $"{b.MakeInput("NUM1", data1)},{b.MakeInput("NUM2", data2)}";
		}

		public class Add : SpecBlock
		{
			public Add(object a, object b) : base("Add a and b", UsagePlace.Both, a, b) 
			{
				if(TypeCheck.Check(a) == AcceptedTypes.String) throw new ArgumentException("a is string, which is not accepted");
				else if(TypeCheck.Check(b) == AcceptedTypes.String) throw new ArgumentException("b is string, which is not accepted");

				args = new BlockArgs("operator_add", MakeNumInput(this, a, b));
			}
		}

		public class Subtract : SpecBlock
		{
			public Subtract(object a, object b) : base("Subtract b from a", UsagePlace.Both, a, b)
			{
				if(TypeCheck.Check(a) == AcceptedTypes.String) throw new ArgumentException("a is string, which is not accepted");
				else if(TypeCheck.Check(b) == AcceptedTypes.String) throw new ArgumentException("b is string, which is not accepted");

				args = new BlockArgs("operator_subtract", MakeNumInput(this, a, b));
			}
		}

		public class Multiply : SpecBlock
		{
			public Multiply(object a, object b) : base("Multiply a and b", UsagePlace.Both, a, b)
			{
				if(TypeCheck.Check(a) == AcceptedTypes.String) throw new ArgumentException("a is string, which is not accepted");
				else if(TypeCheck.Check(b) == AcceptedTypes.String) throw new ArgumentException("b is string, which is not accepted");

				args = new BlockArgs("operator_multiply", MakeNumInput(this, a, b));
			}
		}

		public class Divide : SpecBlock
		{
			public Divide(object a, object b) : base("Divide b from a", UsagePlace.Both, a, b)
			{
				if(TypeCheck.Check(a) == AcceptedTypes.String) throw new ArgumentException("a is string, which is not accepted");
				else if(TypeCheck.Check(b) == AcceptedTypes.String) throw new ArgumentException("b is string, which is not accepted");

				args = new BlockArgs("operator_divide", MakeNumInput(this, a, b));
			}
		}

		public class Greater : SpecBlock
		{
			public Greater(object a, object b) : base("If a greater than b", UsagePlace.Both, a, b)
			{

			}
		}

		public class Lesser : SpecBlock
		{
			public Lesser(object a, object b) : base("If a lesser than b", UsagePlace.Both, a, b)
			{

			}
		}

		public class Mod : SpecBlock
		{
			public Mod(object a, object b) : base("a mod b", UsagePlace.Both, a, b)
			{

			}
		}

		public class Not: SpecBlock
		{
			public Not(object a) : base("Not a", UsagePlace.Both, a)
			{

			}
		}

		public class Or : SpecBlock
		{
			public Or(object a, object b) : base("a or b", UsagePlace.Both, a, b)
			{

			}
		}

		public class And : SpecBlock
		{
			public And(object a, object b) : base("a and b", UsagePlace.Both, a, b)
			{

			}
		}

		public class Equal : SpecBlock
		{
			public Equal(object a, object b) : base("a equal b", UsagePlace.Both, a, b)
			{

			}
		}

		public class Round : SpecBlock
		{
			public Round(object a) : base("Round a", UsagePlace.Both, a)
			{

			}
		}

		public class Random : SpecBlock
		{
			public Random(object a, object b) : base("Pick random from a to b", UsagePlace.Both, a, b)
			{

			}
		}

		public class Length : SpecBlock
		{
			public Length(object text) : base("Length of text", UsagePlace.Both, text)
			{

			}
		}

		public class Contains : SpecBlock
		{
			public Contains(object text1, object text2) : base("If text1 contains text2", UsagePlace.Both, text1, text2)
			{

			}
		}

		public class Join : SpecBlock
		{
			public Join(object text1, object text2) : base("join text1 and text2", UsagePlace.Both, text1, text2)
			{

			}
		}

		public class Letter : SpecBlock
		{
			public Letter(object character, object text) : base("If letter character of text", UsagePlace.Both, character, text)
			{

			}
		}

		public class Abs : SpecBlock
		{
			public Abs(object a) : base("ABS a", UsagePlace.Both, a)
			{

			}
		}

		public class Floor : SpecBlock
		{
			public Floor(object a) : base("Floor a", UsagePlace.Both, a)
			{

			}
		}

		public class Ceil : SpecBlock
		{
			public Ceil(object a) : base("Ceil a", UsagePlace.Both, a)
			{

			}
		}

		public class SquareRoot : SpecBlock
		{
			public SquareRoot(object a) : base("SquareRoot a", UsagePlace.Both, a)
			{

			}
		}

		public class Sin : SpecBlock
		{
			public Sin(object a) : base("Sin a", UsagePlace.Both, a)
			{

			}
		}

		public class Cos : SpecBlock
		{
			public Cos(object a) : base("Cos a", UsagePlace.Both, a)
			{

			}
		}

		public class Tan : SpecBlock
		{
			public Tan(object a) : base("Tan a", UsagePlace.Both, a)
			{

			}
		}

		public class Asin : SpecBlock
		{
			public Asin(object a) : base("Asin a", UsagePlace.Both, a)
			{

			}
		}

		public class Acos : SpecBlock
		{
			public Acos(object a) : base("Acos a", UsagePlace.Both, a)
			{

			}
		}

		public class Atan : SpecBlock
		{
			public Atan(object a) : base("Atan a", UsagePlace.Both, a)
			{

			}
		}

		public class Ln : SpecBlock
		{
			public Ln(object a) : base("Ln a", UsagePlace.Both, a)
			{

			}
		}

		public class Log : SpecBlock
		{
			public Log(object a) : base("Log a", UsagePlace.Both, a)
			{

			}
		}

		public class E : SpecBlock
		{
			public E(object a) : base("E a", UsagePlace.Both, a)
			{

			}
		}

		public class Ten : SpecBlock
		{
			public Ten(object a) : base("Ten a", UsagePlace.Both, a)
			{

			}
		}
	}
}