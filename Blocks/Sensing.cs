using Scratch_Utils;
using System;
using System.Collections.Generic;

namespace Scratch
{
	public static class Sensing
	{
		private static string MakeCurrentField(string data)
		{
			return Block.MakeField("CURRENTMENU", data);
		}

		public enum Vars
		{
			Answer,
			Loudness,
			Timer,
			CurrentYear,
			CurrentMont,
			CurrentDate,
			CurrentDayOfWeek,
			CurrentHour,
			CurrentMinute,
			CurrentSecond,
			Username
		}
		internal static Dictionary<string, SpecVar> specVars = new Dictionary<string, SpecVar>()
		{
			["Answer"] = new SpecVar(UsagePlace.Both, "sensing_answer", "answer varaible from ask block"),
			["Loudness"] = new SpecVar(UsagePlace.Both, "sensing_loudness", "loudness variable"),
			["Timer"] = new SpecVar(UsagePlace.Both, "sensing_timer", "timer current seconds variable"),
			["CurrentYear"] = new SpecVar(UsagePlace.Both, "sensing_current", "current year variable", MakeCurrentField("YEAR")),
			["CurrentMont"] = new SpecVar(UsagePlace.Both, "sensing_current", "current month variable", MakeCurrentField("MONTH")),
			["CurrentDate"] = new SpecVar(UsagePlace.Both, "sensing_current", "current date variable", MakeCurrentField("DATE")),
			["CurrentDayOfWeek"] = new SpecVar(UsagePlace.Both, "sensing_current", "current day of week variable", MakeCurrentField("DAYOFWEEK")),
			["CurrentHour"] = new SpecVar(UsagePlace.Both, "sensing_current", "current hour variable", MakeCurrentField("HOUR")),
			["CurrentMinute"] = new SpecVar(UsagePlace.Both, "sensing_current", "current minute variable", MakeCurrentField("MINUTE")),
			["CurrentSecond"] = new SpecVar(UsagePlace.Both, "sensing_current", "current second variable", MakeCurrentField("SECOND")),
			["Username"] = new SpecVar(UsagePlace.Both, "sensing_username", "username variable"),
		};

		public class Ask : Block
		{
			public Ask(object text) : base("Ask text and wait", UsagePlace.Both, text) 
			{
				args = new BlockArgs("sensing_askandwait", MakeInput("QUESTION", text, "text", AcceptedTypes.None, true));
			}
		}

		public class DragMode : Block
		{
			public enum Mode
			{
				Draggable,
				NotDraggable
			}

			public DragMode(Mode mode) : base("Set drag mode", UsagePlace.Sprite)
			{
				args = new BlockArgs("sensing_setdragmode", null, MakeField("DRAG_MODE", (mode==Mode.Draggable)?"draggable":"not draggable"));
			}
		}

		public class ResetTimer : Block
		{
			public ResetTimer() : base("Reset timer")
			{
				args = new BlockArgs("sensing_resettimer");
			}
		}
	}
}