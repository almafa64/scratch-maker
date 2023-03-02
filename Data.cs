using Scratch_Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using static Scratch.Project;

namespace Scratch_Utils
{
	public class BaseData
	{
		//internal Dictionary<string, T> map = new Dictionary<string, T>();
		public string Name;
		public BaseData(string name) 
		{
			Name = name;
		}
		
	}

	public class Ided : BaseData
	{
		public string Id { get; internal set; }
		public Ided(string name):base(name)
		{
			Id = ID.Make();
		}
	}

	public class Container : Ided
	{
		internal SObject sObject;
		public Container(SObject sObject, string name) : base(name)
		{
			this.sObject = sObject;
		}
	}

	public class Accessories : BaseData
	{
		internal string assetId;
		internal string dataFormat;
		internal string md5ext;
		internal string path;

		public Accessories(string path, string name):base(name)
		{
			this.path = path;
			this.assetId = ID.AssetMake();
			this.dataFormat = path.Substring(path.LastIndexOf('.') + 1);
			this.md5ext = assetId + '.' + dataFormat;
		}
	}

	public struct CommentData
	{
		public string blockId;
		public int height;
		public bool minimized;
		public string text;
		public int width;
		public double x;
		public double y;
		public CommentData(string blockId, string text, int height, int width, double x, double y, bool minimized)
		{
			this.blockId = blockId;
			this.text = text;
			this.height = height;
			this.width = width;
			this.x = x;
			this.y = y;
			this.minimized = minimized;
		}
	}
}

namespace Scratch
{
	public class Var : Container
	{
		internal object value;
		internal AcceptedTypes type;

		public Var(SObject sObject, string name, object value) : base(sObject, name)
		{
			//type check
			type = TypeCheck.Check(value);
			this.value = value;
			sObject.data.Vars.Add(this);
		}	
	}

	public class List : Container
	{
		internal List<object> vars = new List<object>();

		public List(SObject sObject, string name, params object[] vars) : base(sObject, name)
		{
			//type check
			foreach(object o in vars)
			{
				TypeCheck.Check(o);
			}
			this.vars.AddRange(vars);
			sObject.data.Lists.Add(this);
		}
	}

	public class Broadcast : Ided
	{
		public Broadcast(SObject sObject, string name) : base(name)
		{
			if(sObject is Background bg) bg.data.Broadcasts.Add(this);
			else sObject.data.Project.background.data.Broadcasts.Add(this);
		}
	}

	public class Comment : Ided
	{
		public CommentData data;

		public Comment(SObject sObject, string text, int height = 200, int width = 200, double x = 500, double y = 500, bool minimized = false) : this(sObject, new CommentData(null, text, height, width, x, y, minimized))
		{
			
		}

		internal Comment(SObject sObject, CommentData data) : base("")
		{
			this.data = data;
			sObject.data.Comments.Add(this);
		}
	}

	public class Costume : Accessories
	{
		internal byte bitmapResolution;
		public float x;
		public float y;

		internal float baseX;
		internal float baseY;

		public Costume(string path, string name, float x = 0, float y = 0) : base(path, name)
		{
			/*if(dataFormat == "png")
			{
				Bitmap a = new Bitmap(path);
				baseX = a.Width / 2;
				baseY = a.Height / 2;
			}*/
			this.x = x + baseX;
			this.y = y + baseY;
			this.bitmapResolution = (byte)((dataFormat == "svg") ? 1 : 2);
		}
	}

	public class Sound : Accessories
	{
		internal string format;
		internal int rate;
		internal int sampleCount;

		public Sound(string path, string name, string format = "", int rate = 48000, int sampleCount = 1123) : base(path, name)
		{
			this.format = format;
			this.rate = rate;
			this.sampleCount = sampleCount;
		}
	}
}
