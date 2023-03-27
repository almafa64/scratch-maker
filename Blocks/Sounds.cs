using Scratch_Utils;
using System.Collections.Generic;

namespace Scratch
{
	public static class Sounds
	{
		private static string MakeEffectField(string data)
		{
			return Block.MakeField("EFFECT", data);
		}

		public enum Vars
		{
			Volume
		}

		internal static Dictionary<string, SpecVar> specVars = new Dictionary<string, SpecVar>()
		{
			["Volume"] = new SpecVar(UsagePlace.Both, "sound_volume", "current volume of sprite")
		};

		private static Block SoundBlock(Sound s)
		{
			return new Block(null)
			{
				args = new BlockArgs("sound_sounds_menu", null, $"\"SOUND_MENU\":[\"{s.Name}\",null]", null, null, true),
				needsNext = false
			};
		}

		public sealed class Play : Block
		{
			public Play(Sound sound) : base("Play sound")
			{
				Block tmp = SoundBlock(sound);

				args = new BlockArgs("sound_play", $"\"SOUND_MENU\":[1,\"{tmp.args.Id}\"]");

				tmp.args.ParentId = args.Id;
				kids.Add(tmp);
			}
		}

		public sealed class PlayAndWait : Block
		{
			public PlayAndWait(Sound sound) : base("Play sound until done")
			{
				Block tmp = SoundBlock(sound);

				args = new BlockArgs("sound_playuntildone", $"\"SOUND_MENU\":[1,\"{tmp.args.Id}\"]");

				tmp.args.ParentId = args.Id;
				kids.Add(tmp);
			}
		}

		public sealed class Stop : Block
		{
			public Stop() : base("Stop sounds")
			{
				args = new BlockArgs("sound_stopallsounds");
			}
		}

		public static class Volume
		{
			public sealed class Change : Block
			{
				public Change(object by) : base("Change volume by", UsagePlace.Both, by)
				{
					args = new BlockArgs("sound_changevolumeby", MakeInput("VOLUME", by, "by"));
				}
			}

			public sealed class Set : Block
			{
				public Set(object to) : base("Set volume to", UsagePlace.Both, to)
				{
					args = new BlockArgs("sound_setvolumeto", MakeInput("VOLUME", to, "to"));
				}
			}
		}

		public static class Effect
		{
			public enum Effects
			{
				Pitch,
				Pan,
			}

			private static string EffField(Effects eff)
			{
				return (eff == Effects.Pitch) ? MakeEffectField("PITCH") : MakeEffectField("PAN");
			}

			public sealed class Change : Block
			{
				public Change(Effects effect, object by) : base("Change effect by", UsagePlace.Both, by)
				{
					args = new BlockArgs("sound_changeeffectby", MakeInput("VALUE", by, "by"), EffField(effect));
				}
			}

			public sealed class Set : Block
			{
				public Set(Effects effect, object to) : base("Set effect to", UsagePlace.Both, to)
				{
					args = new BlockArgs("sound_seteffectto", MakeInput("VALUE", to, "to"), EffField(effect));
				}
			}

			public sealed class Clear : Block
			{
				public Clear() : base("Clear effects")
				{
					args = new BlockArgs("sound_cleareffects");
				}
			}
		}
	}
}