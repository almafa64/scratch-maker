using Scratch_Utils;
using System;

namespace Scratch
{
	public static class Control
	{
		public sealed class Stop : Block
		{
			private static string MakeStopField(string what)
			{
				return MakeField("STOP_OPTION", what);
			}

			public enum What
			{
				All,
				This,
				OtherScripts
			}

			public Stop(What what) : base("Stop what_to_stop")
			{
				Mutator m = new Mutator
				{
					hasNext = 0
				};
				args = new BlockArgs("control_stop");
				switch(what)
				{
					case What.All:
						args.Fields = MakeStopField("all");
						break;
					case What.This:
						args.Fields = MakeStopField("this script");
						break;
					case What.OtherScripts:
						args.Fields = MakeStopField("other scripts in sprite");
						m.hasNext = 1;
						break;
				}
				args.Mutatator = m;
			}
		}

		public sealed class Wait : Block
		{
			public Wait(object secs) : base("Wait secs", UsagePlace.Both, secs)
			{
				args = new BlockArgs("control_wait", MakeInput("DURATION", secs, "secs"));
			}
		}

		public sealed class WaitUntil : Block
		{
			public WaitUntil(SpecBlock block) : base("Wait until block true", UsagePlace.Both)
			{
				args = new BlockArgs("control_wait_until", MakeInput("CONDITION", block, "block"));
			}
		}

		public static class Clone
		{
			public sealed class Delete : Block
			{
				public Delete() : base("Delete this sprite", UsagePlace.Sprite) 
				{
					args = new BlockArgs("control_delete_this_clone");
				}
			}

			public sealed class Create : Block
			{
				public enum What
				{
					Myself
				}

				private static string MakeCreateField(string data)
				{
					return MakeField("CLONE_OPTION", data);
				}

				public Create(object sprite) : base("Create clone of sprite")
				{
					args = new BlockArgs("control_create_clone_of");
					string id = args.Id;
					Block tmp = new Block(null)
					{
						args = new BlockArgs("control_create_clone_of_menu",null,null,null,id,true)
					};
					if(sprite is What w)
					{
						usagePlace = UsagePlace.Sprite;
						tmp.args.Fields = MakeCreateField("_myself_");
					}
					else if(sprite is Sprite s)
					{
						tmp.args.Fields = MakeCreateField(s.name);
					}
					else throw new ArgumentException("sprite is not What element or Sprite object");

					args.Inputs = $"\"CLONE_OPTION\":[1,\"{tmp.args.Id}\"]";
					tmp.needsNext = false;
					kids.Add(tmp);
				}
			}

			public sealed class StartAs : TopBlock
			{
				public StartAs(SObject sprite, int x = 200, int y = 200) : base("control_start_as_clone", "When start as clone", sprite, x, y) 
				{
					mainBlock.usagePlace = UsagePlace.Sprite;
				}
			}
		}

		public static class Loop
		{
			public sealed class Forever : Block
			{
				public Forever(params Block[] blocks) : base("Loop forever")
				{
					args = new BlockArgs("control_forever");
					blocks[0].autoLevel = false;
					args.Inputs = MakeInput("SUBSTACK", blocks[0], "blocks", Types.All, InputType.Number, "", false);

					for(int i = 1; i < blocks.Length; i++) 
					{
						kids.Add(blocks[i]);
					}
				}
			}

			public sealed class Repeat : Block
			{
				public Repeat(object num, params Block[] blocks) : base("Repeat for num", UsagePlace.Both, num)
				{
					args = new BlockArgs("control_repeat");
					blocks[0].autoLevel = false;
					args.Inputs = $"{MakeInput("TIMES", num, "num", Types.String, InputType.PositiveInteger)},{MakeInput("SUBSTACK", blocks[0], "blocks", Types.All, InputType.Number, "", false)}";

					for(int i = 1; i < blocks.Length; i++)
					{
						kids.Add(blocks[i]);
					}
				}
			}

			public sealed class RepeatUntil : Block
			{
				public RepeatUntil(Block block, params Block[] blocks) : base("Repeat until block false")
				{
					args = new BlockArgs("control_repeat_until");
					blocks[0].autoLevel = false;
					args.Inputs = $"{MakeInput("CONDITION", block, "num", Types.String, InputType.PositiveInteger)},{MakeInput("SUBSTACK", blocks[0], "blocks", Types.All, InputType.Number, "", false)}";

					for(int i = 1; i < blocks.Length; i++)
					{
						kids.Add(blocks[i]);
					}
				}
			}
		}

		public sealed class If : Block
		{
			public If(Block block, params Block[] blocks) : base("If block true")
			{
				args = new BlockArgs("control_if");
				blocks[0].autoLevel = false;
				args.Inputs = $"{MakeInput("CONDITION", block, "num", Types.String, InputType.PositiveInteger)},{MakeInput("SUBSTACK", blocks[0], "blocks", Types.All, InputType.Number, "", false)}";
			}
		}

		public sealed class IfElse : Block
		{
			public IfElse(Block block, Block[] blocksTrue, Block[] blocksFalse) : base("If block true else")
			{
				args = new BlockArgs("control_if_else");
				blocksTrue[0].autoLevel = blocksFalse[0].autoLevel = false;
				args.Inputs = $"{MakeInput("CONDITION", block, "num", Types.String, InputType.PositiveInteger)},{MakeInput("SUBSTACK", blocksTrue[0], "blocksTrue", Types.All, InputType.Number, "", false)},{MakeInput("SUBSTACK2", blocksFalse[0], "blocksFalse", Types.All, InputType.Number, "", false)}";
			}
		}
	}
}