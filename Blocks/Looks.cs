using Scratch_Utils;
using System;
using System.Collections.Generic;

namespace Scratch
{
	public static class Looks
	{
		public enum Vars
		{
			Size,
			CostumeName,
			CostumeNumber,
			BackdropName,
			BackdropNumber
		}

		internal static Dictionary<string, SpecVar> specVars = new Dictionary<string, SpecVar>();

		public class Say : Block
		{
			public Say(object text, object sec) : base("Say text for seconds", text, sec)
			{
				if(TypeCheck.Check(sec) == AcceptedTypes.String) throw new ArgumentException($"sec is string, which is not accepted");

				args = new BlockArgs("looks_sayforsecs");
				usagePlace = UsagePlace.Sprite;

				args.Inputs = $"{MakeInput("MESSAGE", text, true)},{MakeInput("SECS", sec)}";
			}

			public Say(object text) : base("Say text", text)
			{
				args = new BlockArgs("looks_say");
				usagePlace = UsagePlace.Sprite;

				args.Inputs = MakeInput("MESSAGE", text, true);
			}
		}

		public class Think : Block
		{
			public Think(object text, object sec) : base("Think text for seconds", text, sec)
			{
				if(TypeCheck.Check(sec) == AcceptedTypes.String) throw new ArgumentException($"sec is string, which is not accepted");

				args = new BlockArgs("looks_thinkforsecs");
				usagePlace = UsagePlace.Sprite;

				args.Inputs = $"{MakeInput("MESSAGE", text, true)},{MakeInput("SECS", sec)}";
			}

			public Think(object text) : base("Think text", text)
			{
				args = new BlockArgs("looks_think");
				usagePlace = UsagePlace.Sprite;

				args.Inputs = MakeInput("MESSAGE", text, true);
			}
		}

		public static class Switch
		{
			public class Costume : Block
			{
				public Costume(Costume costume) : base("Switch costume")
				{
					//args = new BlockArgs("motion_gotoxy");
					usagePlace = UsagePlace.Sprite;

				}
			}

			public class Backdrop : Block
			{
				public Backdrop(Costume costume) : base($"Switch backdrop")
				{
					//args = new BlockArgs("motion_gotoxy");
				}
			}

			public class BackdropWrait : Block
			{
				public BackdropWrait(Costume costume) : base($"Switch backdrop and wait")
				{
					//args = new BlockArgs("motion_gotoxy");
					usagePlace = UsagePlace.Background;
				}
			}
		}

		public static class Next
		{
			public class Costume : Block
			{
				public Costume() : base("next Costume")
				{
					//args = new BlockArgs("motion_gotoxy");
					usagePlace = UsagePlace.Sprite;

				}
			}

			public class Backdrop : Block
			{
				public Backdrop() : base("next Backdrop")
				{
					//args = new BlockArgs("motion_gotoxy");
				}
			}
		}

		public static class Size
		{
			public class Set : Block
			{
				public Set(object to) : base("Set size", to)
				{
					args = new BlockArgs("looks_setsizeto");
					usagePlace = UsagePlace.Sprite;

					args.Inputs = MakeInput("SIZE", to);
				}
			}

			public class Change : Block
			{
				public Change(object by) : base("Change size", by)
				{
					args = new BlockArgs("looks_changesizeby");
					usagePlace = UsagePlace.Sprite;

					args.Inputs = MakeInput("CHANGE", by);
				}
			}
		}

		public static class Effect
		{
			public enum Effects
			{

			}

			public class Change : Block
			{
				public Change(Effects effect) : base($"Change effect")
				{
					//args = new BlockArgs("motion_gotoxy");
				}
			}

			public class Set : Block
			{
				public Set(Effects effect) : base($"Set effect")
				{
					//args = new BlockArgs("motion_gotoxy");
				}
			}

			public class Clear : Block
			{
				public Clear() : base($"Clear effects")
				{
					//args = new BlockArgs("motion_gotoxy");
				}
			}
		}

		public class Show : Block
		{
			public Show() : base("Show")
			{
				args = new BlockArgs("looks_show");
				usagePlace = UsagePlace.Sprite;
			}
		}

		public class Hide : Block
		{
			public Hide() : base("Hide")
			{
				args = new BlockArgs("looks_hide");
				usagePlace = UsagePlace.Sprite;
			}
		}

		public static class Layer
		{
			public class GoTo : Block
			{
				public enum To
				{
					Front,
					Back
				}
				public GoTo(To to) : base("Go to layer")
				{
					//args = new BlockArgs("motion_gotoxy");
					usagePlace = UsagePlace.Sprite;
				}
			}

			public class Go : Block
			{
				public enum WhereTo
				{
					Forward,
					Backward
				}
				public Go(WhereTo whereTo) : base("Go layer")
				{
					//args = new BlockArgs("motion_gotoxy");
					usagePlace = UsagePlace.Sprite;
				}
			}
		}
	}
}