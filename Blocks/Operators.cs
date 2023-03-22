using Scratch_Utils;

namespace Scratch
{
	public static class Operators
	{
		private static string MakeNumInput(Block bl, object a, object b)
		{
			return $"{bl.MakeInput("NUM1", a, "a")},{bl.MakeInput("NUM2", b, "b")}";
		}

		private static string MakeOperandInput(Block bl, object a, object b)
		{
			return $"{bl.MakeInput("OPERAND1", a, "a", AcceptedTypes.None, true)},{bl.MakeInput("OPERAND2", b, "b", AcceptedTypes.None, true)}";
		}

		private static void MakeOpBlock(Block bl, object val, string data)
		{
			bl.args = new BlockArgs("operator_mathop", bl.MakeInput("NUM", val, "val"), Block.MakeField("OPERATOR", data));
		}

		public class Add : SpecBlock
		{
			public Add(object a, object b) : base("Add a and b", UsagePlace.Both, a, b) 
			{
				args = new BlockArgs("operator_add", MakeNumInput(this, a, b));
			}
		}

		public class Subtract : SpecBlock
		{
			public Subtract(object a, object b) : base("Subtract b from a", UsagePlace.Both, a, b)
			{
				args = new BlockArgs("operator_subtract", MakeNumInput(this, a, b));
			}
		}

		public class Multiply : SpecBlock
		{
			public Multiply(object a, object b) : base("Multiply a and b", UsagePlace.Both, a, b)
			{
				args = new BlockArgs("operator_multiply", MakeNumInput(this, a, b));
			}
		}

		public class Divide : SpecBlock
		{
			public Divide(object a, object b) : base("Divide b from a", UsagePlace.Both, a, b)
			{
				args = new BlockArgs("operator_divide", MakeNumInput(this, a, b));
			}
		}

		public class Mod : SpecBlock
		{
			public Mod(object a, object b) : base("a mod b", UsagePlace.Both, a, b)
			{
				args = new BlockArgs("operator_mod", MakeNumInput(this, a, b));
			}
		}

		public class Greater : SpecBlock
		{
			public Greater(object a, object b) : base("If a greater than b", UsagePlace.Both, a, b)
			{
				args = new BlockArgs("operator_gt", MakeOperandInput(this, a, b));
				isBool = true;
			}
		}

		public class Lesser : SpecBlock
		{
			public Lesser(object a, object b) : base("If a lesser than b", UsagePlace.Both, a, b)
			{
				args = new BlockArgs("operator_lt", MakeOperandInput(this, a, b));
				isBool = true;
			}
		}

		public class Equal : SpecBlock
		{
			public Equal(object a, object b) : base("a equal b", UsagePlace.Both, a, b)
			{
				args = new BlockArgs("operator_equals", MakeOperandInput(this, a, b));
				isBool = true;
			}
		}

		public class Not: SpecBlock
		{
			public Not(object a) : base("Not a", UsagePlace.Both, a)
			{
				args = new BlockArgs("operator_not", MakeInput("OPERAND", a, "a"));
				isBool = true;
			}
		}

		public class Or : SpecBlock
		{
			public Or(object a, object b) : base("a or b", UsagePlace.Both, a, b)
			{
				args = new BlockArgs("operator_or", MakeOperandInput(this, a, b));
				isBool = true;
			}
		}

		public class And : SpecBlock
		{
			public And(object a, object b) : base("a and b", UsagePlace.Both, a, b)
			{
				args = new BlockArgs("operator_and", MakeOperandInput(this, a, b));
				isBool = true;
			}
		}

		public class Random : SpecBlock
		{
			public Random(object a, object b) : base("Pick random from a to b", UsagePlace.Both, a, b)
			{
				args = new BlockArgs("operator_random", $"{MakeInput("FROM", a, "a")},{MakeInput("TO", b, "b")}");
			}
		}

		public class Length : SpecBlock
		{
			public Length(object text) : base("Length of text", UsagePlace.Both, text)
			{
				args = new BlockArgs("operator_length", MakeInput("STRING", text, "text", AcceptedTypes.None, true));
			}
		}

		public class Contains : SpecBlock
		{
			public Contains(object text, object check) : base("If text contains check", UsagePlace.Both, text, check)
			{
				args = new BlockArgs("operator_contains", $"{MakeInput("STRING1", text, "text", AcceptedTypes.None, true)},{MakeInput("STRING2", check, "check", AcceptedTypes.None, true)}");
				isBool = true;
			}
		}

		public class Join : SpecBlock
		{
			public Join(object text1, object text2) : base("join text1 and text2", UsagePlace.Both, text1, text2)
			{
				args = new BlockArgs("operator_join", $"{MakeInput("STRING1", text1, "text1", AcceptedTypes.None, true)},{MakeInput("STRING2", text2, "text2", AcceptedTypes.None, true)}");
			}
		}

		public class Letter : SpecBlock
		{
			public Letter(object character, object text) : base("If letter character of text", UsagePlace.Both, character, text)
			{
				args = new BlockArgs("operator_letter_of", $"{MakeInput("LETTER", character, "character", AcceptedTypes.Number | AcceptedTypes.String, true)},{MakeInput("STRING", text, "text", AcceptedTypes.None, true)}");
			}
		}
		public class Round : SpecBlock
		{
			public Round(object a) : base("Round a", UsagePlace.Both, a)
			{
				args = new BlockArgs("operator_round", MakeInput("NUM", a, "a"));
			}
		}

		public class OperatorBlock : SpecBlock
		{
			internal OperatorBlock(string name, object a, string data) : base(name, UsagePlace.Both, a) 
			{
				MakeOpBlock(this, a, data);
			}
		}

		public class Abs : OperatorBlock
		{
			public Abs(object a) : base("ABS a", a, "abs"){}
		}

		public class Floor : OperatorBlock
		{
			public Floor(object a) : base("Floor a", a, "floor"){}
		}

		public class Ceil : OperatorBlock
		{
			public Ceil(object a) : base("Ceil a", a, "ceil"){}
		}

		public class Sqrt : OperatorBlock
		{
			public Sqrt(object a) : base("SquareRoot a", a, "sqrt"){}
		}

		public class Sin : OperatorBlock
		{
			public Sin(object a) : base("Sin a", a, "sin"){}
		}

		public class Cos : OperatorBlock
		{
			public Cos(object a) : base("Cos a", a, "cos"){}
		}

		public class Tan : OperatorBlock
		{
			public Tan(object a) : base("Tan a", a, "tan"){}
		}

		public class Asin : OperatorBlock
		{
			public Asin(object a) : base("Asin a", a, "asin"){}
		}

		public class Acos : OperatorBlock
		{
			public Acos(object a) : base("Acos a", a, "acos"){}
		}

		public class Atan : OperatorBlock
		{
			public Atan(object a) : base("Atan a", a, "atan"){}
		}

		public class Ln : OperatorBlock
		{
			public Ln(object a) : base("Ln a", a, "ln"){}
		}

		public class Log : OperatorBlock
		{
			public Log(object a) : base("Log a", a, "log"){}
		}

		public class EPow : OperatorBlock
		{
			public EPow(object a) : base("E a", a, "e ^"){}
		}

		public class TenPow : OperatorBlock
		{
			public TenPow(object a) : base("Ten a", a, "10 ^") {}
		}
	}
}