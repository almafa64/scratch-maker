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

		public class StartSound : Block
		{
			public StartSound(Sound sound) : base("Start playing sound")
			{
				//args = new BlockArgs("");
			}
		}

		public class StartSoundWait : Block
		{
			public StartSoundWait(Sound sound) : base("Start playing sound until done")
			{
				//args = new BlockArgs("");
			}
		}

		public class StopSounds : Block
		{
			public StopSounds() : base("Stops all sounds")
			{
				args = new BlockArgs("sound_stopallsounds");
			}
		}

		public static class Volume
		{
			public class Change : Block
			{
				public Change(object by) : base("Changes volume by", UsagePlace.Both, by)
				{
					//args = new BlockArgs("", MakeInput("", by));
				}
			}

			public class Set : Block
			{
				public Set(object to) : base("Sets volume to", UsagePlace.Both, to)
				{
					//args = new BlockArgs("", MakeInput("", to));
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

			public class Change : Block
			{
				public Change(Effects effect, object by) : base("Changes effect by", UsagePlace.Both, by)
				{
					//args = new BlockArgs("", MakeInput("", by));
				}
			}

			public class Set : Block
			{
				public Set(Effects effect, object to) : base("Sets effect to", UsagePlace.Both, to)
				{
					//args = new BlockArgs("", MakeInput("", to));
				}
			}

			public class ClearEffects : Block
			{
				public ClearEffects() : base("Clears all sound effects")
				{
					args = new BlockArgs("sound_cleareffects");
				}
			}
		}
	}
}