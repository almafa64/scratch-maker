using Scratch_Utils;
using System;
using static Scratch.Events;
using static Scratch.Looks.Next;
using static Scratch.Sounds;

namespace Scratch
{
	public static class Events
	{
		public class ClickSprite : TopBlock
		{
			public ClickSprite(SObject sObject, int x = 0, int y = 0) : base("event_whenthisspriteclicked", "When sprite clicked", sObject, x, y)
			{
				mainBlock.usagePlace = UsagePlace.Sprite;
			}
		}

		public class ClickStage : TopBlock
		{
			public ClickStage(SObject sObject, int x = 0, int y = 0) : base("event_whenstageclicked", "When stage clicked", sObject, x, y) 
			{
				mainBlock.usagePlace = UsagePlace.Background;
			}
		}

		public class ClickFlag : TopBlock
		{
			public ClickFlag(SObject sObject, int x = 0, int y = 0) : base("event_whenflagclicked", "When green flag clicked", sObject, x, y) { }
		}

		public class KeyPress : TopBlock
		{
			public enum Keys
			{
				Any,
				LeftArrow,
				RightArrow,
				UpArrow,
				DownArrow,
				a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z
			}

			public KeyPress(Keys key, SObject sprite, int x = 0, int y = 0) : base("event_whenkeypressed", "When key pressed", sprite, x, y)
			{
				string keyString;
				switch(key)
				{
					case Keys.Any: keyString = "any"; break;
					case Keys.LeftArrow: keyString = "left arrow"; break;
					case Keys.RightArrow: keyString = "right arrow"; break;
					case Keys.UpArrow: keyString = "up arrow"; break;
					case Keys.DownArrow: keyString = "down arrow"; break;
					default: keyString = typeof(Keys).GetEnumName(key); break;
				}
				mainBlock.args.Fields = Block.MakeField("KEY_OPTION", keyString);
			}
		}

		public class BackdropSwitch : TopBlock
		{
			public BackdropSwitch(Costume costume, SObject sprite, int x = 0, int y = 0) : base("event_whenbackdropswitchesto", "When backdrop switches to", sprite, x, y)
			{
				mainBlock.args.Fields = Block.MakeField("BACKDROP", costume.Name);
			}
		}

		public class Greater : TopBlock
		{
			public enum What
			{
				Timer,
				Loudness
			}
			public Greater(What what, int number, SObject sprite, int x = 0, int y = 0) : base("event_whengreaterthan", null, sprite, x, y)
			{
				string name;
				bool isTimer = what == What.Timer;

				name = isTimer ? "When timer bigger than" : "When loudness bigger than";

				mainBlock.name = name;
				mainBlock.args.Inputs = mainBlock.MakeInput("VALUE", number);
				mainBlock.args.Fields = Block.MakeField("WHENGREATERTHANMENU", isTimer?"TIMER":"LOUDNESS");
			}
		}

		public static class Broadcasts
		{
			public class Recive : TopBlock
			{
				public Recive(Broadcast broadcast, SObject sprite, int x = 0, int y = 0) : base("event_whenbroadcastreceived", "When broadcast recived", sprite, x, y)
				{
					mainBlock.args.Fields = Block.MakeField("BROADCAST_OPTION", $"\"{broadcast.Name}\",\"{broadcast.Id}\"", false);
				}
			}

			private static string MakeBroadcastInput(Broadcast br)
			{
				return $"\"BROADCAST_INPUT\":[1,[11,\"{br.Name}\",\"{br.Id}\"]]";
			}

			public class Send : Block
			{
				public Send(Broadcast broadcast) : base("Broadcast broadcast")
				{
					args = new BlockArgs("event_broadcast", MakeBroadcastInput(broadcast));
				}
			}

			public class SendAndWait : Block
			{
				public SendAndWait(Broadcast broadcast) : base("Broadcast broadcast and wait")
				{
					args = new BlockArgs("event_broadcastandwait", MakeBroadcastInput(broadcast));
				}
			}
		}
	}
}