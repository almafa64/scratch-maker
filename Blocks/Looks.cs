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
			public class Costumes : Block
			{
				public Costumes(Costume costume) : base("Switch costume")
				{
					Block tmp = new Block(null)
					{
						args = new BlockArgs("looks_costume", null, $"\"COSTUME\":[\"{costume.Name}\",null]")
					};

					args = new BlockArgs("looks_switchcostumeto", $"\"COSTUME\":[1,{tmp.args.Id}]");
					usagePlace = UsagePlace.Sprite;

					tmp.args.ParentId = args.Id;
					kids.Add(tmp);
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
					args = new BlockArgs("looks_nextcostume");
					usagePlace = UsagePlace.Sprite;

				}
			}

			public class Backdrop : Block
			{
				public Backdrop() : base("next Backdrop")
				{
					args = new BlockArgs("looks_nextbackdrop");
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
				Color,
				Fisheye,
				Whirl,
				Pixelate,
				Mosaic,
				Brightness,
				Ghost
			}

			private static string EffField(Effects eff)
            {
				switch (eff)
				{
					case Effects.Color: return Block.MakeField("EFFECT", "COLOR");
					case Effects.Fisheye: return Block.MakeField("EFFECT", "FISHEYE");
					case Effects.Whirl: return Block.MakeField("EFFECT", "WHIRL");
					case Effects.Pixelate: return Block.MakeField("EFFECT", "PIXELATE");
					case Effects.Mosaic: return Block.MakeField("EFFECT", "MOSAIC");
					case Effects.Brightness: return Block.MakeField("EFFECT", "BRIGHTNESS");
					case Effects.Ghost: return Block.MakeField("EFFECT", "GHOST");
					default: return null;
				}
			}

			public class Change : Block
			{
				public Change(Effects effect, object by) : base("Change effect", by)
				{
					if (TypeCheck.Check(by) == AcceptedTypes.String) throw new ArgumentException($"by is string, which is not accepted");

					args = new BlockArgs("looks_changeeffectby", MakeInput("CHANGE", by), EffField(effect));
				}
			}

			public class Set : Block
			{
				public Set(Effects effect, object to) : base("Set effect", to)
				{
					if (TypeCheck.Check(to) == AcceptedTypes.String) throw new ArgumentException($"to is string, which is not accepted");

					args = new BlockArgs("looks_seteffectto", MakeInput("VALUE", to), EffField(effect));
				}
			}

			public class Clear : Block
			{
				public Clear() : base("Clear effects")
				{
					args = new BlockArgs("looks_cleargraphiceffects");
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
					args = new BlockArgs("looks_gotofrontback");
					usagePlace = UsagePlace.Sprite;

					if(to == To.Back) args.Fields = MakeField("FRONT_BACK", "back");
					else args.Fields = MakeField("FRONT_BACK", "front");
				}
			}

			public class Go : Block
			{
				public enum WhereTo
				{
					Forward,
					Backward
				}
				public Go(WhereTo whereTo, object times) : base("Go layer", times)
				{
					if (TypeCheck.Check(times) == AcceptedTypes.String) throw new ArgumentException($"times is string, which is not accepted");

					args = new BlockArgs("looks_goforwardbackwardlayers");
					usagePlace = UsagePlace.Sprite;

					if (whereTo == WhereTo.Forward) args.Fields = MakeField("FORWARD_BACKWARD", "forward");
					else args.Fields = MakeField("FORWARD_BACKWARD", "backward");

					args.Inputs = MakeInput("NUM", times);
				}
			}
		}
	}
}