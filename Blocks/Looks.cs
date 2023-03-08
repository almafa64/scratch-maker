using Scratch_Utils;
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
			BackgroundName,
			BackgroundNumber
		}

		internal static Dictionary<string, SpecVar> specVars = new Dictionary<string, SpecVar>();
	}
}