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

		private static string MakeEffectField(string data)
		{
			return Block.MakeField("EFFECT", data);
		}

		internal static Dictionary<string, SpecVar> specVars = new Dictionary<string, SpecVar>() 
		{
			["BackdropNumber"] = new SpecVar(UsagePlace.Both, "looks_backdropnumbername", "background number variable", "\"NUMBER_NAME\":[\"number\",null]"),
			["BackdropName"] = new SpecVar(UsagePlace.Both, "looks_backdropnumbername", "background name variable", "\"NUMBER_NAME\":[\"name\",null]"),
			["CostumeNumber"] = new SpecVar(UsagePlace.Sprite, "looks_costumenumbername", "costume number variable", "\"NUMBER_NAME\":[\"number\",null]"),
			["CostumeName"] = new SpecVar(UsagePlace.Sprite, "looks_costumenumbername", "costume name variable", "\"NUMBER_NAME\":[\"name\",null]"),
			["Size"] = new SpecVar(UsagePlace.Sprite, "looks_size", "size variable")
		};

		public sealed class Say : Block
		{
			public Say(object text, object sec) : base("Say text for seconds", UsagePlace.Sprite, text, sec)
			{
				if(TypeCheck.Check(sec) == Types.String) throw new ArgumentException("sec is string, which is not accepted");

				args = new BlockArgs("looks_sayforsecs", $"{MakeInput("MESSAGE", text, "text", Types.None, InputType.String)},{MakeInput("SECS", sec, "sec")}");
			}

			public Say(object text) : base("Say text", UsagePlace.Sprite, text)
			{
				args = new BlockArgs("looks_say", MakeInput("MESSAGE", text, "text", Types.None, InputType.String));
			}
		}

		public sealed class Think : Block
		{
			public Think(object text, object sec) : base("Think text for seconds", UsagePlace.Sprite, text, sec)
			{
				args = new BlockArgs("looks_thinkforsecs", $"{MakeInput("MESSAGE", text, "text", Types.None, InputType.String)},{MakeInput("SECS", sec, "sec")}");
			}

			public Think(object text) : base("Think text", UsagePlace.Sprite, text)
			{
				args = new BlockArgs("looks_think", MakeInput("MESSAGE", text, "text", Types.None, InputType.String));
			}
		}

		public static class Switch
		{
			public sealed class Costumes : Block
			{
				public Costumes(Costume costume) : base("Switch costume", UsagePlace.Sprite)
				{
					Block tmp = new Block(null)
					{
						args = new BlockArgs("looks_costume", null, $"\"COSTUME\":[\"{costume.Name}\",null]", null, null, true),
						needsNext = false
					};

					args = new BlockArgs("looks_switchcostumeto", $"\"COSTUME\":[1,\"{tmp.args.Id}\"]");

					tmp.args.ParentId = args.Id;
					kids.Add(tmp);
				}
			}

			public enum Which
			{
				NextBackdrop,
				PreviousBackdrop,
				RandomBackdrop
			}

			private static Block BackBlock(object costume)
			{
				string cosName = "";
				if(costume is Costume cos) cosName = cos.Name;
				else if(costume is Which w)
				{
					switch(w)
					{
						case Which.NextBackdrop:
							cosName = "next backdrop";
							break;
						case Which.PreviousBackdrop:
							cosName = "previous backdrop";
							break;
						case Which.RandomBackdrop:
							cosName = "random backdrop";
							break;
					}
				}
				else throw new ArgumentException("costume was not a Costume object or a Which element");

				return new Block(null)
				{
					args = new BlockArgs("looks_backdrops", null, $"\"BACKDROP\":[\"{cosName}\",null]", null, null, true),
					needsNext = false
				};
			}

			public sealed class Backdrop : Block
			{
				public Backdrop(object costume) : base($"Switch backdrop", UsagePlace.Both)
				{
					Block tmp = BackBlock(costume);

					args = new BlockArgs("looks_switchbackdropto", $"\"BACKDROP\":[1,\"{tmp.args.Id}\"]");

					tmp.args.ParentId = args.Id;
					kids.Add(tmp);
				}
			}

			public sealed class BackdropWait : Block
			{
				public BackdropWait(object costume) : base($"Switch backdrop and wait", UsagePlace.Background)
				{
					Block tmp = BackBlock(costume);

					args = new BlockArgs("looks_switchbackdroptoandwait", $"\"BACKDROP\":[1,\"{tmp.args.Id}\"]");

					tmp.args.ParentId = args.Id;
					kids.Add(tmp);
				}
			}
		}

		public static class Next
		{
			public sealed class Costume : Block
			{
				public Costume() : base("next Costume", UsagePlace.Sprite)
				{
					args = new BlockArgs("looks_nextcostume");

				}
			}

			public sealed class Backdrop : Block
			{
				public Backdrop() : base("next Backdrop")
				{
					args = new BlockArgs("looks_nextbackdrop");
				}
			}
		}

		public static class Size
		{
			public sealed class Set : Block
			{
				public Set(object to) : base("Set size", UsagePlace.Sprite, to)
				{
					args = new BlockArgs("looks_setsizeto", MakeInput("SIZE", to, "to"));
				}
			}

			public sealed class Change : Block
			{
				public Change(object by) : base("Change size", UsagePlace.Sprite, by)
				{
					args = new BlockArgs("looks_changesizeby", MakeInput("CHANGE", by, "by"));
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
					case Effects.Color:		 return MakeEffectField("COLOR");
					case Effects.Fisheye:	 return MakeEffectField("FISHEYE");
					case Effects.Whirl:		 return MakeEffectField("WHIRL");
					case Effects.Pixelate:	 return MakeEffectField("PIXELATE");
					case Effects.Mosaic:	 return MakeEffectField("MOSAIC");
					case Effects.Brightness: return MakeEffectField("BRIGHTNESS");
					case Effects.Ghost:		 return MakeEffectField("GHOST");
					default: return null;
				}
			}

			public sealed class Change : Block
			{
				public Change(Effects effect, object by) : base("Change effect", UsagePlace.Both, by)
				{
					args = new BlockArgs("looks_changeeffectby", MakeInput("CHANGE", by, "by"), EffField(effect));
				}
			}

			public sealed class Set : Block
			{
				public Set(Effects effect, object to) : base("Set effect", UsagePlace.Both, to)
				{
					args = new BlockArgs("looks_seteffectto", MakeInput("VALUE", to, "to"), EffField(effect));
				}
			}

			public sealed class Clear : Block
			{
				public Clear() : base("Clear effects", UsagePlace.Both)
				{
					args = new BlockArgs("looks_cleargraphiceffects");
				}
			}
		}

		public sealed class Show : Block
		{
			public Show() : base("Show", UsagePlace.Sprite)
			{
				args = new BlockArgs("looks_show");
			}
		}

		public sealed class Hide : Block
		{
			public Hide() : base("Hide", UsagePlace.Sprite)
			{
				args = new BlockArgs("looks_hide");
			}
		}

		public static class Layer
		{
			public sealed class GoTo : Block
			{
				public enum To
				{
					Front,
					Back
				}
				public GoTo(To to) : base("Go to layer", UsagePlace.Sprite)
				{
					args = new BlockArgs("looks_gotofrontback", null, (to == To.Back)?MakeField("FRONT_BACK", "back"):MakeField("FRONT_BACK", "front"));
				}
			}

			public sealed class Go : Block
			{
				public enum WhereTo
				{
					Forward,
					Backward
				}
				public Go(WhereTo whereTo, object times) : base("Go layer", UsagePlace.Sprite, times)
				{
					args = new BlockArgs("looks_goforwardbackwardlayers", MakeInput("NUM", times, "times"), (whereTo == WhereTo.Forward)?MakeField("FORWARD_BACKWARD", "forward"):MakeField("FORWARD_BACKWARD", "backward"));
				}
			}
		}
	}
}