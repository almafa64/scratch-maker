using Scratch_Utils;

namespace Scratch_Utils
{
	public class OperatorBlock : SpecBlock
	{
		internal OperatorBlock(string name, object num, string data) : base(name, UsagePlace.Both, num)
		{
			args = new BlockArgs("operator_mathop", MakeInput("NUM", num, "num"), MakeField("OPERATOR", data));
		}
	}
}

namespace Scratch
{
	public static class Operators
	{
		private static string MakeNumInput(Block bl, object num1, object num2)
		{
			return $"{bl.MakeInput("NUM1", num1, "num1")},{bl.MakeInput("NUM2", num2, "num2")}";
		}

		private static string MakeOperandInput(Block bl, object a, object b)
		{
			return $"{bl.MakeInput("OPERAND1", a, "a", Types.None, InputType.String)},{bl.MakeInput("OPERAND2", b, "b", Types.None, InputType.String)}";
		}

		public sealed class Add : SpecBlock
		{
			public Add(object num1, object num2) : base("Add num1 and num2", UsagePlace.Both, num1, num2) 
			{
				args = new BlockArgs("operator_add", MakeNumInput(this, num1, num2));
			}
		}

		public sealed class Subtract : SpecBlock
		{
			public Subtract(object num1, object num2) : base("Subtract num2 from num1", UsagePlace.Both, num1, num2)
			{
				args = new BlockArgs("operator_subtract", MakeNumInput(this, num1, num2));
			}
		}

		public sealed class Multiply : SpecBlock
		{
			public Multiply(object num1, object num2) : base("Multiply num1 and num2", UsagePlace.Both, num1, num2)
			{
				args = new BlockArgs("operator_multiply", MakeNumInput(this, num1, num2));
			}
		}

		public sealed class Divide : SpecBlock
		{
			public Divide(object num1, object num2) : base("Divide num2 from num1", UsagePlace.Both, num1, num2)
			{
				args = new BlockArgs("operator_divide", MakeNumInput(this, num1, num2));
			}
		}

		public sealed class Mod : SpecBlock
		{
			public Mod(object num1, object num2) : base("num1 mod num2", UsagePlace.Both, num1, num2)
			{
				args = new BlockArgs("operator_mod", MakeNumInput(this, num1, num2));
			}
		}

		public sealed class Greater : SpecBlock
		{
			public Greater(object a, object b) : base("If a greater than b", UsagePlace.Both, a, b)
			{
				args = new BlockArgs("operator_gt", MakeOperandInput(this, a, b));
				isBool = true;
			}
		}

		public sealed class Lesser : SpecBlock
		{
			public Lesser(object a, object b) : base("If a lesser than b", UsagePlace.Both, a, b)
			{
				args = new BlockArgs("operator_lt", MakeOperandInput(this, a, b));
				isBool = true;
			}
		}

		public sealed class Equal : SpecBlock
		{
			public Equal(object a, object b) : base("a equal b", UsagePlace.Both, a, b)
			{
				args = new BlockArgs("operator_equals", MakeOperandInput(this, a, b));
				isBool = true;
			}
		}

		public sealed class Not: SpecBlock
		{
			public Not(object a) : base("Not a", UsagePlace.Both, a)
			{
				args = new BlockArgs("operator_not", MakeInput("OPERAND", a, "a"));
				isBool = true;
			}
		}

		public sealed class Or : SpecBlock
		{
			public Or(object a, object b) : base("a or b", UsagePlace.Both, a, b)
			{
				args = new BlockArgs("operator_or", MakeOperandInput(this, a, b));
				isBool = true;
			}
		}

		public sealed class And : SpecBlock
		{
			public And(object a, object b) : base("a and b", UsagePlace.Both, a, b)
			{
				args = new BlockArgs("operator_and", MakeOperandInput(this, a, b));
				isBool = true;
			}
		}

		public sealed class Random : SpecBlock
		{
			public Random(object num1, object num2) : base("Pick random from num1 to num2", UsagePlace.Both, num1, num2)
			{
				args = new BlockArgs("operator_random", $"{MakeInput("FROM", num1, "num1")},{MakeInput("TO", num2, "num2")}");
			}
		}

		public sealed class Length : SpecBlock
		{
			public Length(object text) : base("Length of text", UsagePlace.Both, text)
			{
				args = new BlockArgs("operator_length", MakeInput("STRING", text, "text", Types.None, InputType.String));
			}
		}

		public sealed class Contains : SpecBlock
		{
			public Contains(object text, object check) : base("If text contains check", UsagePlace.Both, text, check)
			{
				args = new BlockArgs("operator_contains", $"{MakeInput("STRING1", text, "text", Types.None, InputType.String)},{MakeInput("STRING2", check, "check", Types.None, InputType.String)}");
				isBool = true;
			}
		}

		public sealed class Join : SpecBlock
		{
			public Join(object text1, object text2) : base("join text1 and text2", UsagePlace.Both, text1, text2)
			{
				args = new BlockArgs("operator_join", $"{MakeInput("STRING1", text1, "text1", Types.None, InputType.String)},{MakeInput("STRING2", text2, "text2", Types.None, InputType.String)}");
			}
		}

		public sealed class Letter : SpecBlock
		{
			public Letter(object character, object text) : base("If letter character of text", UsagePlace.Both, character, text)
			{
				args = new BlockArgs("operator_letter_of", $"{MakeInput("LETTER", character, "character", Types.Number | Types.String)},{MakeInput("STRING", text, "text", Types.None, InputType.String)}");
			}
		}

		public sealed class Round : SpecBlock
		{
			public Round(object num) : base("Round num", UsagePlace.Both, num)
			{
				args = new BlockArgs("operator_round", MakeInput("NUM", num, "num"));
			}
		}

		public sealed class Abs : OperatorBlock
		{
			public Abs(object num) : base("ABS num", num, "abs"){}
		}

		public sealed class Floor : OperatorBlock
		{
			public Floor(object num) : base("Floor num", num, "floor"){}
		}

		public sealed class Ceil : OperatorBlock
		{
			public Ceil(object num) : base("Ceil num", num, "ceil"){}
		}

		public sealed class Sqrt : OperatorBlock
		{
			public Sqrt(object num) : base("SquareRoot num", num, "sqrt"){}
		}

		public sealed class Sin : OperatorBlock
		{
			public Sin(object num) : base("Sin num", num, "sin"){}
		}

		public sealed class Cos : OperatorBlock
		{
			public Cos(object num) : base("Cos num", num, "cos"){}
		}

		public sealed class Tan : OperatorBlock
		{
			public Tan(object num) : base("Tan num", num, "tan"){}
		}

		public sealed class Asin : OperatorBlock
		{
			public Asin(object num) : base("Asin num", num, "asin"){}
		}

		public sealed class Acos : OperatorBlock
		{
			public Acos(object num) : base("Acos num", num, "acos"){}
		}

		public sealed class Atan : OperatorBlock
		{
			public Atan(object num) : base("Atan num", num, "atan"){}
		}

		public sealed class Ln : OperatorBlock
		{
			public Ln(object num) : base("Ln num", num, "ln"){}
		}

		public sealed class Log : OperatorBlock
		{
			public Log(object num) : base("Log num", num, "log"){}
		}

		public sealed class EPow : OperatorBlock
		{
			public EPow(object num) : base("E num", num, "e ^"){}
		}

		public sealed class TenPow : OperatorBlock
		{
			public TenPow(object num) : base("Ten num", num, "10 ^") {}
		}
	}
}