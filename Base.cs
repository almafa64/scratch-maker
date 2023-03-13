using Scratch;
using Scratch_Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Threading;

namespace Scratch_Utils
{
	public class VarDic
	{
		internal SObject sObject;
		internal Dictionary<string, Var> vars;

		internal VarDic(SObject sObject, Dictionary<string, Var> vars)
		{
			this.sObject = sObject;
			this.vars = vars;
		}

		public Var this[string name, bool global = false]
		{
			get
			{
				if(Var.Has(sObject, name)) return vars[name];
				else if(Var.BgHas(sObject, name)) return sObject.Project.background._Vars[name];
				else throw new ArgumentException($"No variable was found with the name \"{name}\"");
			}

			set
			{
				if(Var.Has(sObject, name) || Var.BgHas(sObject, name)) throw new ArgumentException($"Variable with the name \"{name}\" already exists");

				value.Name = name;
				value.sObject = sObject;
				if(!global || sObject is Project.Background) vars[name] = value;
				else sObject.Project.background._Vars[name] = value;
			}
		}
	}

	public class ListDic
	{
		internal SObject sObject;
		internal Dictionary<string, List> lists;

		internal ListDic(SObject sObject, Dictionary<string, List> lists)
		{
			this.sObject = sObject;
			this.lists = lists;
		}

		public List this[string name, bool global = false]
		{
			get
			{
				if(List.Has(sObject, name)) return lists[name];
				else if(List.BgHas(sObject, name)) return sObject.Project.background._Lists[name];
				else throw new ArgumentException($"No list was found with the name \"{name}\"");
			}

			set
			{
				if(List.Has(sObject, name) || List.BgHas(sObject, name)) throw new ArgumentException($"List with the name \"{name}\" already exists");

				value.Name = name;
				value.sObject = sObject;
				if(!global || sObject is Project.Background) lists[name] = value;
				else sObject.Project.background._Lists[name] = value;
			}
		}
	}

	public class MyBlockDic
	{
		internal SObject sObject;
		internal Dictionary<string, MyBlock> myBlocks;

		internal MyBlockDic(SObject sObject, Dictionary<string, MyBlock> myBlocks)
		{
			this.sObject = sObject;
			this.myBlocks = myBlocks;
		}

		public MyBlock this[string name]
		{
			get
			{
				if(sObject._MyBlocks.ContainsKey(name)) return myBlocks[name];
				else throw new ArgumentException($"No list was found with the name \"{name}\"");
			}
		}
	}

	public class SpriteDic
	{
		internal Project pr;
		internal Dictionary<string, Sprite> sprites;

		internal SpriteDic(Project pr, Dictionary<string, Sprite> sprites)
		{
			this.pr = pr;
			this.sprites = sprites;
		}

		public Sprite this[string name]
		{
			get
			{
				if(pr._sprites.ContainsKey(name)) return sprites[name];
				else throw new ArgumentException($"No list was found with the name \"{name}\"");
			}
		}
	}

	public class SObject
	{
		internal List<Column> columns;

		public ReadOnlyDictionary<string, Broadcast> Broadcasts { get; internal set; }
		public ReadOnlyDictionary<string, Comment> Comments { get; internal set; }
		public ReadOnlyDictionary<string, Sound> Sounds { get; internal set; }
		public ReadOnlyDictionary<string, Costume> Costumes { get; internal set; }

		internal Dictionary<string, Broadcast> _Broadcasts;
		internal Dictionary<string, Comment> _Comments;
		internal Dictionary<string, Sound> _Sounds;
		internal Dictionary<string, Costume> _Costumes;

		internal Dictionary<string, List> _Lists;
		internal Dictionary<string, Var> _Vars;
		public VarDic Vars { get; internal set; }
		public ListDic Lists { get; internal set; }

		internal Dictionary<string, MyBlock> _MyBlocks;
		public MyBlockDic MyBlocks { get; internal set; }

		internal Project Project;

		public int CurrentCostume { get; set; }
		public int LayerOrder { get; set; }

		public SObject(Project project) 
		{
			_Sounds = new Dictionary<string, Sound>();
			_Broadcasts = new Dictionary<string, Broadcast>();
			_Comments = new Dictionary<string, Comment>();
			_Costumes = new Dictionary<string, Costume>();

			_MyBlocks = new Dictionary<string, MyBlock>();
			MyBlocks = new MyBlockDic(this, _MyBlocks);

			_Vars = new Dictionary<string, Var>();
			Vars = new VarDic(this, _Vars);
			_Lists = new Dictionary<string, List>();
			Lists = new ListDic(this, _Lists);

			Sounds = new ReadOnlyDictionary<string, Sound>(_Sounds);
			Broadcasts = new ReadOnlyDictionary<string, Broadcast>(_Broadcasts);
			Comments = new ReadOnlyDictionary<string, Comment>(_Comments);
			Costumes = new ReadOnlyDictionary<string, Costume>(_Costumes);

			Project = project;
			CurrentCostume = 0;
			LayerOrder = 1;
			columns = new List<Column>();
		}

		public void AddCostumes(params Costume[] costumes)
		{
			foreach(Costume c in costumes)
			{
				_Costumes[c.Name] = c;
			}
		}

		public void AddSounds(params Sound[] sounds)
		{
			foreach(Sound s in sounds)
			{
				_Sounds[s.Name] = s;
			}
		}
	}
}

namespace Scratch
{
	[Flags]
	public enum Extensions
	{
		None = 0,
		Music = 1,
		Pen = 2,
		VideoSensing = 4,
		TextToSpeech = 8,
		Translate = 16,
		MakeyMakey = 32,
		Microbit = 64,
		EV3 = 128,
		BOOST = 256,
		WeDo2 = 512,
		ForceAndAceleration = 1024
	}

	public enum TextToSpeechLanguages
	{
		None,
		English,
	}

	public enum RotationStyle
	{
		LeftRight,
		DontRotate,
		AllAround
	}

	public class Project : IDisposable
	{
		public string name;
		public Background background;
		internal Dictionary<string, Sprite> _sprites = new Dictionary<string, Sprite>();
		public SpriteDic Sprites;

		public bool openFolder;
		public Extensions extensions;


		public Project(string name, Extensions extensions = Extensions.None, bool openFolder = true)
		{
			Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-GB");
			this.name = name;
			this.background = new Background(this);
			this.extensions = extensions;
			this.openFolder = openFolder;
			Sprites = new SpriteDic(this, _sprites);

			if(Movement.specVars.ContainsKey("dir")) return;

			#region MovementVars
			Movement.specVars["Direction"] = new SpecVar(UsagePlace.Sprite, "motion_direction", "direction variable");
			Movement.specVars["X"] = new SpecVar(UsagePlace.Sprite, "motion_xposition", "x variable");
			Movement.specVars["Y"] = new SpecVar(UsagePlace.Sprite, "motion_yposition", "y variable");
			#endregion

			#region LooksVars
			Looks.specVars["BackdropNumber"] = new SpecVar(UsagePlace.Both, "looks_backdropnumbername", "background number variable", "\"NUMBER_NAME\":[\"number\",null]");
			Looks.specVars["BackdropName"] = new SpecVar(UsagePlace.Both, "looks_backdropnumbername", "background name variable", "\"NUMBER_NAME\":[\"name\",null]");
			Looks.specVars["CostumeNumber"] = new SpecVar(UsagePlace.Sprite, "looks_costumenumbername", "costume number variable", "\"NUMBER_NAME\":[\"number\",null]");
			Looks.specVars["CostumeName"] = new SpecVar(UsagePlace.Sprite, "looks_costumenumbername", "costume name variable", "\"NUMBER_NAME\":[\"name\",null]");
			Looks.specVars["Size"] = new SpecVar(UsagePlace.Sprite, "looks_size", "size variable");
			#endregion
		}

		public class Background : SObject, IDisposable
		{
			internal int tempo = 60;
			internal int videoTransparency = 50;
			internal bool videoOn = true;
			internal int volume = 100;
			internal TextToSpeechLanguages textToSpeechLanguage = TextToSpeechLanguages.None;

			internal Background(Project project) : base(project)
			{
				LayerOrder = 0;
			}

			public void Dispose(){}
		}

		public void Dispose() 
		{
			ID.Clear();
			ID.AssetClear();
			new Compiler(this);
		}
	}

	public class Sprite : SObject, IDisposable
	{
		public string name;

		internal int direction;
		internal bool draggable;
		internal RotationStyle rotationStyle;
		internal int size;
		internal bool visible;
		internal double x;
		internal double y;

		public Sprite(string name, Project project, double x = 0, double y = 0, int direction = 90, int size = 100, bool draggable = true, bool visible = true, RotationStyle rotationStyle = RotationStyle.AllAround) : base(project)
		{
			this.name = name;
			project._sprites[name] = this;
			this.x = x;
			this.y = y;
			this.direction = direction;
			this.size = size;
			this.visible = visible;
			this.rotationStyle = rotationStyle;
			this.draggable = draggable;
		}

		public void Dispose() { }
	}

	public class Column : IDisposable
	{
		public SObject sObject;
		internal List<Block> blocks;
		internal bool isSpriteCol;

		public int x;
		public int y;

		public Column(SObject sprite, int x = 0, int y = 0)
		{
			this.blocks = new List<Block>();
			this.sObject = sprite;
			this.sObject.columns.Add(this);
			this.x = x;
			this.y = y;
			this.isSpriteCol = sObject is Sprite;
		}

		public void Add(Block block)
		{
			if(block.needsNext)
			{
				if(blocks.Count - 1 >= 0)
				{
					Block prev;
					for(int i = 0; ; i++)
					{
						prev = blocks[blocks.Count - (i + 1)];
						if(prev.needsNext) break;
					}

					block.args.ParentId = prev.args.Id;
					prev.args.NextId = block.args.Id;
					block.args.TopLevel = false;
				}
				else block.args.TopLevel = true;
			}
			foreach(Block b in block.kids)
			{
				CheckBlockUsage(b);
			}
			CheckBlockUsage(block);
		}

		internal void CheckBlockUsage(Block b)
		{
			if(isSpriteCol) { if(b.usagePlace == UsagePlace.Background) throw new Exception($"Block \"{b.name}\" can only be used in the backdrop but it was used in sprite"); }
			else if(b.usagePlace == UsagePlace.Sprite) throw new Exception($"Block \"{b.name}\" can only be used in sprites but it was used in the backdrop");
			blocks.Add(b);
		}

		public void Dispose() { }
	}
}
