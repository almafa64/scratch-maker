using Scratch;
using Scratch_Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace Scratch_Utils
{
	public class SObject
	{
		internal List<Column> columns;

		internal Dictionary<string, Broadcast> Broadcasts { get; set; }
		internal Dictionary<string, Comment> Comments { get; set; }
		internal Dictionary<string, Sound> Sounds { get; set; }
		internal Dictionary<string, Costume> Costumes { get; set; }
		internal Dictionary<string, List> Lists { get; set; }
		internal Dictionary<string, Var> Vars { get; set; }
		internal Project Project { get; set; }
		public int CurrentCostume { get; set; }
		public int LayerOrder { get; set; }

		public SObject(Project project) 
		{
			Broadcasts = new Dictionary<string, Broadcast>();
			Comments = new Dictionary<string, Comment>();
			Sounds = new Dictionary<string, Sound>();
			Costumes = new Dictionary<string, Costume>();
			Lists = new Dictionary<string, List>();
			Vars = new Dictionary<string, Var>();
			Project = project;
			CurrentCostume = 0;
			LayerOrder = 1;
			columns = new List<Column>();
		}

		public void AddCostume(params Costume[] costumes)
		{
			foreach(Costume c in costumes)
			{
				Costumes[c.Name] = c;
			}
		}

		public void AddSound(params Sound[] sounds)
		{
			foreach(Sound s in sounds)
			{
				Sounds[s.Name] = s;
			}
		}

		#region Vars

		public void MakeVariable(string name, bool global = true, object value = null)
		{
			if(value == null) value = 0;
			if(TypeCheck.Check(value) == AcceptedTypes.Variable) throw new ArgumentException("Variable value cannot be another variable");
			if(ThisHasVar(name) || BgHasVar(name)) throw new ArgumentException($"Variable with the name \"{name}\" already exists");
			
			if(!global || this is Project.Background) new Var(this, name, value);
			else new Var(Project.background, name, value);
		}

		public Var GetVar(string name)
		{
			if(ThisHasVar(name)) return Vars[name];
			else if(BgHasVar(name)) return Project.background.Vars[name];
			else throw new ArgumentException($"No variable was found with the name \"{name}\"");
		}

		private bool ThisHasVar(string name)
		{
			return Vars.ContainsKey(name);
		}

		private bool BgHasVar(string name)
		{
			return Project.background.Vars.ContainsKey(name);
		}

		#endregion Vars

		#region List

		public void MakeList(string name, bool global = true, object[] values = null)
		{
			if(values != null)
			{
				foreach(object value in values)
				{
					if(TypeCheck.Check(value) == AcceptedTypes.Variable || TypeCheck.Check(value) == AcceptedTypes.List) throw new ArgumentException("List value cannot be another variable or list");
				}
			}
			if(ThisHasList(name) || BgHasList(name)) throw new ArgumentException($"List with the name \"{name}\" already exists");

			if(!global || this is Project.Background) new List(this, name, values);
			else new List(Project.background, name, values);
		}

		public List GetList(string name)
		{
			if(ThisHasList(name)) return Lists[name];
			else if(BgHasList(name)) return Project.background.Lists[name];
			else throw new ArgumentException($"No list was found with the name \"{name}\"");
		}

		private bool ThisHasList(string name)
		{
			return Lists.ContainsKey(name);
		}

		private bool BgHasList(string name)
		{
			return Project.background.Lists.ContainsKey(name);
		}

		#endregion List
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
		internal List<Sprite> sprites = new List<Sprite>();

		public bool openFolder;
		public Extensions extensions;


		public Project(string name, Extensions extensions = Extensions.None, bool openFolder = true)
		{
			Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-GB");
			this.name = name;
			this.background = new Background(this);
			this.extensions = extensions;
			this.openFolder = openFolder;
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

		public Sprite(string name, Project project, double x = 0, double y = 0, int direction = 0, int size = 100, bool draggable = true, bool visible = true, RotationStyle rotationStyle = RotationStyle.AllAround) : base(project)
		{
			this.name = name;
			project.sprites.Add(this);
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

		public Column(SObject sprite)
		{
			this.blocks = new List<Block>();
			this.sObject = sprite;
			this.sObject.columns.Add(this);
		}

		public void Add(Block block)
		{
			if(blocks.Count - 1 >= 0)
			{
				Block prev = blocks[blocks.Count - 1];
				block.args.ParentId = prev.args.Id;
				prev.args.NextId = block.args.Id;
				block.args.TopLevel = false;
			}
			else
			{
				block.args.TopLevel = true;
			}
			blocks.Add(block);
		}

		public void Dispose() { }
	}
}
