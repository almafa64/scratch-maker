using Scratch;
using Scratch_Utils;
using Scratch_Utils.Dics;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace Scratch_Utils.Dics
{
	public class DataDic<TVal>
	{
		internal Dictionary<string, TVal> dic;
		internal SObject sObject;

		internal DataDic(SObject sObject, Dictionary<string, TVal> dic)
		{
			this.sObject = sObject;
			this.dic = dic;
		}
	}

	public class VarDic : DataDic<Var>
	{
		internal VarDic(SObject sObject, Dictionary<string, Var> vars) : base(sObject, vars) { }

		public Var this[string name, bool global = false, bool showInGame = false]
		{
			get
			{
				if(Var.Has(sObject, name)) return dic[name];
				else if(Var.BgHas(sObject, name)) return sObject.Project.background._Vars[name];
				else throw new ArgumentException($"No Var was found with the name \"{name}\"");
			}

			set
			{
				if(Var.Has(sObject, name) || Var.BgHas(sObject, name)) throw new ArgumentException($"Variable with the name \"{name}\" already exists");

				value.Name = name;
				value.show = showInGame;
				if(!global || sObject is Project.Background)
				{
					dic[name] = value;
					value.sObject = sObject;
				}
				else 
				{
					sObject.Project.background._Vars[name] = value;
					value.sObject = sObject.Project.background;
				}
			}
		}
	}

	public class ListDic : DataDic<List>
	{
		internal ListDic(SObject sObject, Dictionary<string, List> lists) : base(sObject, lists) { }

		public List this[string name, bool global = false, bool showInGame = false]
		{
			get
			{
				if(List.Has(sObject, name)) return dic[name];
				else if(List.BgHas(sObject, name)) return sObject.Project.background._Lists[name];
				else throw new ArgumentException($"No List was found with the name \"{name}\"");
			}

			set
			{
				if(List.Has(sObject, name) || List.BgHas(sObject, name)) throw new ArgumentException($"List with the name \"{name}\" already exists");

				value.Name = name;
				value.sObject = sObject;
				value.show = showInGame;
				if(!global || sObject is Project.Background)
				{
					dic[name] = value;
					value.sObject = sObject;
				}
				else
				{
					sObject.Project.background._Lists[name] = value;
					value.sObject = sObject.Project.background;
				}
			}
		}
	}

	public class BroadcastDic
	{
		SObject sObject;
		internal BroadcastDic(SObject sObject)
		{
			this.sObject = sObject;
		}

		public Broadcast this[string name]
		{
			get
			{
				if(Broadcast.Has(sObject, name)) return sObject.Project.background._Broadcasts[name];
				else throw new ArgumentException($"No Broadcast was found with the name \"{name}\"");
			}

			set
			{
				if(Broadcast.Has(sObject, name)) throw new ArgumentException($"Broadcast with the name \"{name}\" already exists");

				value.Name = name;
				sObject.Project.background._Broadcasts[name] = value;
			}
		}
	}

	public class SoundDic : DataDic<Sound>
	{
		internal SoundDic(SObject sObject, Dictionary<string, Sound> broadcast) : base(sObject, broadcast) { }

		public Sound this[string name]
		{
			get
			{
				if(Sound.Has(sObject, name)) return dic[name];
				else throw new ArgumentException($"No Sound was found with the name \"{name}\"");
			}

			set
			{
				if(Sound.Has(sObject, name)) throw new ArgumentException($"Sound with the name \"{name}\" already exists");

				value.Name = name;
				dic[name] = value;
			}
		}
	}

	public class CostumeDic : DataDic<Costume>
	{
		internal CostumeDic(SObject sObject, Dictionary<string, Costume> costume) : base(sObject, costume) { }

		public Costume this[string name]
		{
			get
			{
				if(Costume.Has(sObject, name)) return dic[name];
				else throw new ArgumentException($"No Costume was found with the name \"{name}\"");
			}

			set
			{
				if(value.Name != null) throw new ArgumentException("Wrong Costume class was used");
				else if(Costume.Has(sObject, name)) throw new ArgumentException($"Costume with the name \"{name}\" already exists");

				value.Name = name;
				dic[name] = value;
			}
		}
	}

	public class MyBlockDic : DataDic<MyBlock>
	{
		internal MyBlockDic(SObject sObject, Dictionary<string, MyBlock> myBlocks) : base(sObject, myBlocks) { }

		public MyBlock this[string name]
		{
			get
			{
				if(sObject._MyBlocks.ContainsKey(name)) return dic[name];
				else throw new ArgumentException($"No MyBlock was found with the name \"{name}\"");
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
				else throw new ArgumentException($"No sprite was found with the name \"{name}\"");
			}
		}
	}
}

namespace Scratch_Utils
{
	public class SObject
	{
		internal List<Column> columns;

		internal Dictionary<string, Sound> _Sounds;
		internal Dictionary<string, Costume> _Costumes;
		internal Dictionary<string, List> _Lists;
		internal Dictionary<string, Var> _Vars;
		internal Dictionary<string, MyBlock> _MyBlocks;

		public VarDic Vars { get; internal set; }
		public ListDic Lists { get; internal set; }
		public SoundDic Sounds { get; internal set; }
		public CostumeDic Costumes { get; internal set; }
		public MyBlockDic MyBlocks { get; internal set; }

		public BroadcastDic Broadcasts { get; internal set; }

		internal List<Comment> _Comments;

		internal Project Project;

		public int CurrentCostume = 0;
		public int LayerOrder = 1;

		public SObject(Project project)
		{
			_Sounds = new Dictionary<string, Sound>();
			_Costumes = new Dictionary<string, Costume>();
			_MyBlocks = new Dictionary<string, MyBlock>();
			_Vars = new Dictionary<string, Var>();
			_Lists = new Dictionary<string, List>();

			MyBlocks = new MyBlockDic(this, _MyBlocks);
			Vars = new VarDic(this, _Vars);
			Lists = new ListDic(this, _Lists);
			Sounds = new SoundDic(this, _Sounds);
			Costumes = new CostumeDic(this, _Costumes);

			_Comments = new List<Comment>();

			Broadcasts = new BroadcastDic(this);

			Project = project;
			columns = new List<Column>();
		}

		public void AddCostumes(params Costume[] costumes)
		{
			for(int i = 0; i < costumes.Length; i++)
			{
				Costume c = costumes[i];
				if(c.Name == null) throw new ArgumentException($"Costume at index {i} doesn't has name");
				_Costumes[c.Name] = c;
			}
		}

		public void AddSounds(params Sound[] sounds)
		{
			for(int i = 0; i < sounds.Length; i++)
			{
				Sound s = sounds[i];
				if(s.Name == null) throw new ArgumentException($"Sound at index {i} doesn't has name");
				_Sounds[s.Name] = s;
			}
		}

		public void AddComments(params Comment[] comments)
		{
			foreach(Comment c in comments)
			{
				_Comments.Add(c);
			}
		}

		public void AddBroadcasts(params Broadcast[] broadcasts)
		{
			for(int i = 0; i < broadcasts.Length; i++)
			{
				Broadcast b = broadcasts[i];
				if(b.Name == null) throw new ArgumentException($"Broadcast at index {i} doesn't has name");
				Project.background._Broadcasts[b.Name] = b;
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
		}

		public class Background : SObject, IDisposable
		{
			internal int tempo = 60;
			internal int videoTransparency = 50;
			internal bool videoOn = true;
			internal int volume = 100;
			internal TextToSpeechLanguages textToSpeechLanguage = TextToSpeechLanguages.None;

			internal Dictionary<string, Broadcast> _Broadcasts = new Dictionary<string, Broadcast>();

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

		public static void Delete(Sprite sprite, Project project)
		{
			project._sprites.Remove(sprite.name);
			sprite.Project = null;
		}

		public void Delete()
		{
			Delete(this, Project);
		}

		public void Dispose() { }
	}

	public class Column : IDisposable
	{
		public SObject sObject;
		internal List<Block> blocks = new List<Block>();
		internal bool isSpriteCol;

		public int x;
		public int y;

		public Column(SObject sprite, int x = 0, int y = 0)
		{
			this.sObject = sprite;
			this.sObject.columns.Add(this);
			this.x = x;
			this.y = y;
			this.isSpriteCol = sObject is Sprite;
		}

		public Block Add(params Block[] inBlocks)
		{
			foreach(Block block in inBlocks)
			{
				//if(block is SpecBlock s) throw new ArgumentException(s.args.OpCode + " <--"); //removed until reference next/parent

				if(block.needsNext && block.autoLevel)
				{
					if(blocks.Count > 0)
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
					Add(b);
				}

				CheckBlockUsage(block);
			}
			return inBlocks.Last();
		}

		internal void CheckBlockUsage(Block b)
		{
			if(isSpriteCol) { if(b.usagePlace == UsagePlace.Background) throw new Exception($"Block \"{b.name}\" can only be used in the backdrop but it was used in sprite"); }
			else if(b.usagePlace == UsagePlace.Sprite) throw new Exception($"Block \"{b.name}\" can only be used in sprites but it was used in the backdrop");
			blocks.Add(b);
		}

		public static void Delete(Column column, SObject sObject)
		{
			column.sObject = null;
			sObject.columns.Remove(column);
		}

		public void Delete()
		{
			Delete(this, sObject);
		}

		public void Dispose() { }
	}
}
