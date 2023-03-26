using Scratch;
using Scratch_Utils;

namespace Scratch_Utils
{
	static class ListUtils
	{
		internal static string MakeListField(List l)
		{
			return Block.MakeField("LIST", $"\"{l.Name}\",\"{l.Id}\"", false);
		}
	}

	public class ListBlock : Block
	{
		internal List l;
		internal string MakeListField()
		{
			return ListUtils.MakeListField(l);
		}

		internal ListBlock(List l, string name, UsagePlace usagePlace = UsagePlace.Both, params object[] vals) : base(name, usagePlace, vals)
		{
			this.l = l;
		}
	}

	public class SpecList : SpecBlock
	{
		internal List l;
		internal string MakeListField()
		{
			return ListUtils.MakeListField(l);
		}

		internal SpecList(List l, string name, UsagePlace usagePlace = UsagePlace.Both, params object[] vals) : base(name, usagePlace, vals)
		{
			this.l = l;
		}
	}
}

namespace Scratch
{
	public static class Lists
	{
		public class Add : ListBlock
		{
			public Add(object data, List list) : base(list, "Add data", UsagePlace.Both, data)
			{
				args = new BlockArgs("data_addtolist", MakeInput("ITEM", data, "data", Types.None, InputType.String), MakeListField());
			}
		}

		public class Delete : ListBlock
		{
			public class All : ListBlock
			{
				public All(List list) : base(list, "Delete all")
				{
					args = new BlockArgs("data_deletealloflist", null, MakeListField());
				}
			}

			public Delete(object at, List list) : base(list, "Delete at")
			{
				args = new BlockArgs("data_deleteoflist", MakeInput("INDEX", at, "at", Invert(Types.PositiveNumber | Types.Variable | Types.List), InputType.Integer), MakeListField());
			}
		}

		public class Insert : ListBlock
		{
			public Insert(object data, object at, List list) : base(list, "Insert data at", UsagePlace.Both, data, at)
			{
				args = new BlockArgs("data_insertatlist", $"{MakeInput("ITEM", data, "data", Types.None, InputType.String)},{MakeInput("INDEX", at, "at", Invert(Types.PositiveNumber | Types.Variable | Types.List), InputType.Integer)}", MakeListField());
			}
		}

		public class Replace : ListBlock
		{
			public Replace(object data, object at, List list) : base(list, "Replace at with data", UsagePlace.Both, data, at)
			{
				args = new BlockArgs("data_replaceitemoflist", $"{MakeInput("ITEM", data, "data", Types.None, InputType.String)},{MakeInput("INDEX", at, "at", Invert(Types.PositiveNumber | Types.Variable | Types.List), InputType.Integer)}", MakeListField());
			}
		}

		public class Show : ListBlock
		{
			public Show(List list) : base(list, "")
			{
				args = new BlockArgs("data_showlist", null, MakeListField());
			}
		}

		public class Hide : ListBlock
		{
			public Hide(List list) : base(list, "")
			{
				args = new BlockArgs("data_hidelist", null, MakeListField());
			}
		}

		public class Item : SpecList
		{
			public Item(object at, List list) : base(list, "", UsagePlace.Both, at)
			{
				args = new BlockArgs("data_itemoflist", MakeInput("INDEX", at, "at", Invert(Types.PositiveNumber | Types.Variable | Types.List), InputType.Integer), MakeListField());
			}
		}

		public class ItemIndex : SpecList
		{
			public ItemIndex(object data, List list) : base(list, "")
			{
				args = new BlockArgs("data_itemnumoflist", MakeInput("ITEM", data, "data", Types.None, InputType.String), MakeListField());
			}
		}

		public class Length : SpecList
		{
			public Length(List list) : base(list, "")
			{
				args = new BlockArgs("data_lengthoflist", null, MakeListField());
			}
		}

		public class Contains : SpecList
		{
			public Contains(object data, List list) : base(list, "", UsagePlace.Both, data)
			{
				args = new BlockArgs("data_listcontainsitem", MakeInput("ITEM", data, "data", Types.None, InputType.String), MakeListField());
				isBool = true;
			}
		}
	}
}
