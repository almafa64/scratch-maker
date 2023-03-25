using Scratch_Utils;
using System;
using System.Collections.Generic;
using static Scratch.Events.KeyPress;
using static Scratch.Sensing.KeyPress;

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
				args = new BlockArgs("sensing_askandwait", MakeInput("QUESTION", text, "text", AcceptedTypes.None, InputType.String));
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

		public class Touching : SpecBlock
		{
			public enum What
			{
				Mouse,
				Edge
			}
			public Touching(object what) : base("Touching what", UsagePlace.Sprite, what)
			{
				string wS;
				if(what is What w)
				{
					if(w == What.Edge) wS = "_edge_";
					else wS = "_mouse_";
				}
				else if(what is Sprite s) wS = s.name;
				else throw new ArgumentException("what was not a Sprite or What element");

				args = new BlockArgs("sensing_touchingobject");
				Block tmp = new Block(null)
				{
					args = new BlockArgs("sensing_touchingobjectmenu", null, $"\"TOUCHINGOBJECTMENU\":[\"{wS}\",null]", null, args.Id, true)
				};
				kids.Add(tmp);
				tmp.needsNext = false;
				args.Inputs = $"\"TOUCHINGOBJECTMENU\":[1,\"{tmp.args.Id}\"]";
			}
		}

		public class TouchingColor : SpecBlock
		{
			public TouchingColor(Color color) : base("Touching color", UsagePlace.Sprite)
			{
				args = new BlockArgs("sensing_touchingcolor", MakeInput("COLOR", color.hex, "hex", AcceptedTypes.None, InputType.Color));
			}
		}

		public class ColorTouchingColor : SpecBlock
		{
			public ColorTouchingColor(Color a, Color b) : base("a Color Touching b Color", UsagePlace.Sprite)
			{
				args = new BlockArgs("sensing_coloristouchingcolor", $"{MakeInput("COLOR", a.hex, "a", AcceptedTypes.None, InputType.Color)},{MakeInput("COLOR2", b.hex, "b", AcceptedTypes.None, InputType.Color)}");
			}
		}

		public class Distance : SpecBlock
		{
			public enum What
			{
				Mouse,
			}
			public Distance(object what) : base("Touching what", UsagePlace.Sprite, what)
			{
				string wS;
				if(what is What)
				{
					wS = "_mouse_";
				}
				else if(what is Sprite s) wS = s.name;
				else throw new ArgumentException("what was not a Sprite or What element");

				args = new BlockArgs("sensing_distanceto");
				Block tmp = new Block(null)
				{
					args = new BlockArgs("sensing_distancetomenu", null, $"\"DISTANCETOMENU\":[\"{wS}\",null]", null, args.Id, true)
				};
				kids.Add(tmp);
				tmp.needsNext = false;
				args.Inputs = $"\"DISTANCETOMENU\":[1,\"{tmp.args.Id}\"]";
			}
		}

		public static class Mouse
		{
			public class X : SpecBlock
			{
				public X() : base("Mouse x position") 
				{
					args = new BlockArgs("sensing_mousex");
				}
			}
			
			public class Y : SpecBlock
			{
				public Y() : base("Mouse y position")
				{
					args = new BlockArgs("sensing_mousey");
				}
			}

			public class Down : SpecBlock
			{
				public Down() : base("Mouse down")
				{
					args = new BlockArgs("sensing_mousedown");
				}
			}
		}

		public class DaysSince : SpecBlock
		{
			public DaysSince() : base("Days since 2000")
			{
				args = new BlockArgs("sensing_dayssince2000");
			}
		}

		public class KeyPress : SpecBlock
		{
			public enum Key
			{
				Any,
				LeftArrow,
				RightArrow,
				UpArrow,
				DownArrow,
				Space,
				a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z
			}
			public KeyPress(object what) : base("Touching what", UsagePlace.Sprite, what)
			{
				string wS;
				if(what is Key w)
				{
					switch(w)
					{
						case Key.Any: wS = "any"; break;
						case Key.LeftArrow: wS = "left arrow"; break;
						case Key.RightArrow: wS = "right arrow"; break;
						case Key.UpArrow: wS = "up arrow"; break;
						case Key.DownArrow: wS = "down arrow"; break;
						case Key.Space: wS = "space"; break;
						default: wS = typeof(Keys).GetEnumName(w); break;
					}
				}
				else throw new ArgumentException("what was not a Sprite or What element");

				args = new BlockArgs("sensing_keypressed");
				Block tmp = new Block(null)
				{
					args = new BlockArgs("sensing_keyoptions", null, $"\"KEY_OPTION\":[\"{wS}\",null]", null, args.Id, true)
				};
				kids.Add(tmp);
				tmp.needsNext = false;
				args.Inputs = $"\"KEY_OPTION\":[1,\"{tmp.args.Id}\"]";
			}
		}
	}
}