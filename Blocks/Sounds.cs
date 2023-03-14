using Scratch_Utils;
using System.Collections.Generic;

namespace Scratch
{
	public static class Sounds
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

		public static class Sound
		{
			private static Block SoundBlock(Sound s)
			{
				return new Block(null)
				{
					args = new BlockArgs("sound_sounds_menu", null, $"\"SOUND_MENU\":[\"{s.Name}\",null]", null, null, true)
				};
			}

			public class Play : Block
			{
				public Play(Sound sound) : base("Play sound")
				{
					Block tmp = SoundBlock();

					args = new BlockArgs("sound_play", $"\"SOUND_MENU\":[1,\"{tmp.args.Id}\"]");

					tmp.args.ParentId = args.Id;
					kids.Add(tmp);
				}
			}

			public class PlayAndWait : Block
			{
				public PlayAndWait(Sound sound) : base("Play sound until done")
				{
					Block tmp = SoundBlock();

					args = new BlockArgs("sound_playuntildone", $"\"SOUND_MENU\":[1,\"{tmp.args.Id}\"]");

					tmp.args.ParentId = args.Id;
					kids.Add(tmp);
				}
			}

			public class Stop: Block
			{
				public Stop() : base("Stop sounds")
				{
					args = new BlockArgs("sound_stopallsounds");
				}
			}
		}

		public static class Volume
		{
			public class Change : Block
			{
				public Change(object by) : base("Change volume by", UsagePlace.Both, by)
				{
					if (TypeCheck.Check(by) == AcceptedTypes.String) throw new ArgumentException("by is string, which is not accepted");

					args = new BlockArgs("sound_changevolumeby", MakeInput("VOLUME", by));
				}
			}

			public class Set : Block
			{
				public Set(object to) : base("Set volume to", UsagePlace.Both, to)
				{
					if (TypeCheck.Check(to) == AcceptedTypes.String) throw new ArgumentException("to is string, which is not accepted");

					args = new BlockArgs("sound_setvolumeto", MakeInput("VOLUME", to));
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
				return (eff == Effects.Pitch) ? Block.MakeEffectField("PITCH") : Block.MakeEffectField("PAN");
			}

			public class Change : Block
			{
				public Change(Effects effect, object by) : base("Change effect by", UsagePlace.Both, by)
				{
					if (TypeCheck.Check(by) == AcceptedTypes.String) throw new ArgumentException("by is string, which is not accepted");

					args = new BlockArgs("looks_changeeffectby", MakeInput("VALUE", by), EffField(effect));
				}
			}

			public class Set : Block
			{
				public Set(Effects effect, object to) : base("Set effect to", UsagePlace.Both, to)
				{
					if (TypeCheck.Check(to) == AcceptedTypes.String) throw new ArgumentException("to is string, which is not accepted");

					args = new BlockArgs("looks_seteffectto", MakeInput("VALUE", to), EffField(effect));
				}
			}

			public class Clear : Block
			{
				public Clear() : base("Clear effects")
				{
					args = new BlockArgs("sound_cleareffects");
				}
			}
		}
	}
}