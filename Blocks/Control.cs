using Scratch_Utils;
using System;

namespace Scratch
{
	public static class Control
	{
		public class Stop : Block
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

		public class Wait : Block
		{
			public Wait(object secs) : base("Wait secs", UsagePlace.Both, secs)
			{
				args = new BlockArgs("control_wait", MakeInput("DURATION", secs, "secs"));
			}
		}

		public static class Clone
		{
			public class Delete : Block
			{
				public Delete() : base("Delete this sprite", UsagePlace.Sprite) 
				{
					args = new BlockArgs("control_delete_this_clone");
				}
			}

			public class Create : Block
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

			public class StartAs : TopBlock
			{
				public StartAs(SObject sprite, int x = 200, int y = 200) : base("control_start_as_clone", "When start as clone", sprite, x, y) 
				{
					mainBlock.usagePlace = UsagePlace.Sprite;
				}
			}
		}
	}
}