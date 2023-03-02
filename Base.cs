using Scratch;
using Scratch_Utils;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Scratch_Utils
{
	public struct SpriteData
	{
		internal List<Broadcast> Broadcasts { get; set; }	//to Dictionary<string, Broadcast>
		internal List<Comment> Comments { get; set; }		//to Dictionary<string, Comment>
		internal List<Sound> Sounds { get; set; }			//to Dictionary<string, Sounds>
		internal List<Costume> Costumes { get; set; }		//to Dictionary<string, Costume>
		internal List<List> Lists { get; set; }				//to Dictionary<string, List>
		internal List<Var> Vars { get; set; }				//to Dictionary<string, Var>
		internal Project Project { get; set; }
		public int CurrentCostume { get; set; }
		public int LayerOrder { get; set; }	
	}

	public class SObject// : List<Column>
	{
		internal SpriteData data;
		internal List<Column> columns;

		public SObject(Project project) 
		{
			data = new SpriteData();
			data.Broadcasts = new List<Broadcast>();
			data.Comments = new List<Comment>();
			data.Sounds = new List<Sound>();
			data.Costumes = new List<Costume>();
			data.Lists = new List<List>();
			data.Vars = new List<Var>();
			data.Project = project;
			data.CurrentCostume = 0;
			data.LayerOrder = 1;
			columns = new List<Column>();
		}

		public void AddCostume(params Costume[] costume)
		{
			data.Costumes.AddRange(costume);
		}

		public void AddSound(params Sound[] sounds)
		{
			data.Sounds.AddRange(sounds);
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
		internal List<Sprite> sprites = new List<Sprite>();

		public Extensions extensions;

		public Project(string name, Extensions extensions = Extensions.None)
		{
			this.name = name;
			this.background = new Background(this);
			this.extensions = extensions;
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
				data.LayerOrder = 0;
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
