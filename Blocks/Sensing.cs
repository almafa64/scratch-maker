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
				args = new BlockArgs("sensing_askandwait", MakeInput("QUESTION", text, "text", Types.None, InputType.String));
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
					args = new BlockArgs("sensing_touchingobjectmenu", null, $"\"TOUCHINGOBJECTMENU\":[\"{wS}\",null]", null, args.Id, true),
					needsNext = false
				};
				kids.Add(tmp);
				args.Inputs = $"\"TOUCHINGOBJECTMENU\":[1,\"{tmp.args.Id}\"]";

				isBool = true;
			}
		}

		public class TouchingColor : SpecBlock
		{
			public TouchingColor(Color color) : base("Touching color", UsagePlace.Sprite)
			{
				args = new BlockArgs("sensing_touchingcolor", MakeInput("COLOR", color.hex, "hex", Types.None, InputType.Color));
				isBool = true;
			}
		}

		public class ColorTouchingColor : SpecBlock
		{
			public ColorTouchingColor(Color a, Color b) : base("a Color Touching b Color", UsagePlace.Sprite)
			{
				args = new BlockArgs("sensing_coloristouchingcolor", $"{MakeInput("COLOR", a.hex, "a", Types.None, InputType.Color)},{MakeInput("COLOR2", b.hex, "b", Types.None, InputType.Color)}");
				isBool = true;
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
					args = new BlockArgs("sensing_distancetomenu", null, $"\"DISTANCETOMENU\":[\"{wS}\",null]", null, args.Id, true),
					needsNext = false
				};
				kids.Add(tmp);
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
					isBool = true;
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
			public enum Keys
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
				if(what is Keys w)
				{
					switch(w)
					{
						case Keys.Any: wS = "any"; break;
						case Keys.LeftArrow: wS = "left arrow"; break;
						case Keys.RightArrow: wS = "right arrow"; break;
						case Keys.UpArrow: wS = "up arrow"; break;
						case Keys.DownArrow: wS = "down arrow"; break;
						case Keys.Space: wS = "space"; break;
						default: wS = typeof(Keys).GetEnumName(w); break;
					}
				}
				else throw new ArgumentException("what was not a Sprite or What element");

				args = new BlockArgs("sensing_keypressed");
				Block tmp = new Block(null)
				{
					args = new BlockArgs("sensing_keyoptions", null, $"\"KEY_OPTION\":[\"{wS}\",null]", null, args.Id, true),
					needsNext = false
				};
				kids.Add(tmp);
				args.Inputs = $"\"KEY_OPTION\":[1,\"{tmp.args.Id}\"]";

				isBool = true;
			}
		}

		public class Of : SpecBlock
		{
			public enum Data
			{
				//Sprite
				PositionX,
				PositionY,
				Direction,
				CostumeNum,
				CostumeName,
				Size,
				//Back
				BackdropNum,
				BackdropName,
				//Both
				Volume,
			}

			public Of(object data, SObject sObject = null) : base("data of sObject", UsagePlace.Both, data)
			{
				string prop, obj;

				if(data is Var v)
				{
					prop = v.Name;
					obj = (v.sObject is Sprite s) ? s.name : "_stage_";
				}
				else
				{
					bool isSprite = false;
					if(sObject is Sprite s)
					{
						isSprite = true;
						obj = s.name;
					}
					else if(sObject is Project.Background) obj = "_stage_";
					else throw new ArgumentException("sObject is not Sprite or Background");

					if(data is Data d)
					{
						if(d == Data.Volume) prop = "volume";
						else if(isSprite)
						{
							switch(d)
							{
								case Data.PositionX: prop = "x position"; break;
								case Data.PositionY: prop = "y position"; break;
								case Data.Direction: prop = "direction"; break;
								case Data.CostumeNum: prop = "costume #"; break;
								case Data.CostumeName: prop = "costume name"; break;
								case Data.Size: prop = "size"; break;
								case Data.Volume: prop = "volume"; break;
								default: throw new ArgumentException($"cannot use \"{typeof(Data).GetEnumName(d)}\" elelemnt on Sprite");
							}
						}
						else
						{
							switch(d)
							{
								case Data.BackdropName: prop = "backdrop name"; break;
								case Data.BackdropNum: prop = "backdrop #"; break;
								default: throw new ArgumentException($"cannot use \"{typeof(Data).GetEnumName(d)}\" elelemnt on the Background");
							}
						}
					}
					else throw new ArgumentException("data is not Data elelemnt or Var");
				}

				args = new BlockArgs("sensing_of", null, $"\"PROPERTY\":[\"{prop}\",null]");
				Block tmp = new Block(null)
				{
					args = new BlockArgs("sensing_of_object_menu", null, $"\"OBJECT\":[\"{obj}\",null]", null, args.Id, true),
					needsNext = false
				};
				kids.Add(tmp);
				args.Inputs = $"\"OBJECT\":[1,\"{tmp.args.Id}\"]";
			}
		}
	}
}