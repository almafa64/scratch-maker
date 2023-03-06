using Scratch.Properties;
using Scratch_Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static Scratch.Project;

namespace Scratch_Utils
{
	public class BaseData
	{
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
			if(!File.Exists(path)) throw new ArgumentException($"File {path} doesn't exists");
			this.path = path;
			this.assetId = ID.AssetMake();
			this.dataFormat = path.Substring(path.LastIndexOf('.') + 1);
			this.md5ext = assetId + '.' + dataFormat;
		}

		internal Accessories(string name):base(name)
		{
			this.assetId = ID.AssetMake();
			this.dataFormat = "svg";
			this.md5ext = assetId + '.' + dataFormat;
		}
	}
}

namespace Scratch
{
	public class Var : Container
	{
		internal object value;

		internal Var(SObject sObject, string name, object value) : base(sObject, name)
		{
			if(Has(sObject, name) || BgHas(sObject, name)) throw new ArgumentException($"Variable with the name \"{name}\" already exists");
			
			if(value == null) value = 0;
			else if(TypeCheck.Check(value) == AcceptedTypes.Variable || TypeCheck.Check(value) == AcceptedTypes.List) throw new ArgumentException("Variable value cannot be another variable or list");
			
			this.value = value;
			sObject._Vars[name] = this;
		}

		public Var(object value) : base(null, null)
		{
			if(value == null) value = 0;
			else if(TypeCheck.Check(value) == AcceptedTypes.Variable || TypeCheck.Check(value) == AcceptedTypes.List) throw new ArgumentException("Variable value cannot be another variable or list");
			
			this.value = value;
		}

		internal static bool Has(SObject sObject, string name)
		{
			return sObject._Vars.ContainsKey(name);
		}

		internal static bool BgHas(SObject sObject, string name)
		{
			return sObject.Project.background._Vars.ContainsKey(name);
		}
	}

	public class List : Container
	{
		internal List<object> vars = new List<object>();

		internal List(SObject sObject, string name, params object[] vars) : base(sObject, name)
		{
			if(Has(sObject, name) || BgHas(sObject, name)) throw new ArgumentException($"List with the name \"{name}\" already exists");

			if(vars != null)
			{
				foreach(object value in vars)
				{
					if(TypeCheck.Check(value) == AcceptedTypes.Variable || TypeCheck.Check(value) == AcceptedTypes.List) throw new ArgumentException("List value cannot be another variable or list");
				}
				this.vars.AddRange(vars);
			}
			sObject._Lists[name] = this;
		}

		public List(params object[] vars) : base(null, null)
		{
			if(vars != null)
			{
				foreach(object value in vars)
				{
					if(TypeCheck.Check(value) == AcceptedTypes.Variable || TypeCheck.Check(value) == AcceptedTypes.List) throw new ArgumentException("List value cannot be another variable or list");
				}
				this.vars.AddRange(vars);
			}
		}

		internal static bool Has(SObject sObject, string name)
		{
			return sObject._Lists.ContainsKey(name);
		}

		internal static bool BgHas(SObject sObject, string name)
		{
			return sObject.Project.background._Lists.ContainsKey(name);
		}
	}

	public class Broadcast : Ided
	{
		public Broadcast(SObject sObject, string name) : base(name)
		{
			if(sObject is Background bg) bg._Broadcasts[name] = this;
			else sObject.Project.background._Broadcasts[name] = this;
		}
	}

	public class Comment : Ided
	{
		internal string name;
		internal string blockId = null;
		public int height;
		public bool minimized;
		public string text;
		public int width;
		public double x;
		public double y;

		public Comment(SObject sObject, string name, string text = "", bool minimized = false, int height = 200, int width = 200, double x = 200, double y = 200) : base("")
		{
			this.text = text;
			this.name = name;
			this.minimized = minimized;
			this.height = height;
			this.width = width;
			this.x = x;
			this.y = y;
			sObject._Comments[name] = this;
		}
	}

	public class Costume : Accessories
	{
		internal byte bitmapResolution;
		internal float x;
		internal float y;

		internal float baseX;
		internal float baseY;
		internal byte[] bytes;

		public Costume(string path, string name, float x = 0, float y = 0) : base(path, name)
		{
			bitmapResolution = (byte)((dataFormat == "svg") ? 1 : 2);
			if(bitmapResolution == 1)
			{
				using(FileStream fs = new FileStream(path, FileMode.Open))
				{
					const int length = 180;
					byte[] arr = new byte[length];
					fs.Read(arr, 0, length);
					StringBuilder sb = new StringBuilder();
					for(int i = 0; i < arr.Length; i++)
					{
						sb.Append((char)arr[i]);
					}
					string a = sb.ToString();
					int widthIS = a.IndexOf("width") + "width=\"".Length;
					int widthIE = a.IndexOf('"', widthIS) - widthIS;
					int heightIS = a.IndexOf("height") + "height=\"".Length;
					int heightIE = a.IndexOf('"', heightIS) - heightIS;
					baseX = Convert.ToSingle(a.Substring(widthIS, widthIE)) / 2f;
					baseY = Convert.ToSingle(a.Substring(heightIS, heightIE)) / 2f;
				}
			}
			else
			{
				/*using(Bitmap a = new Bitmap(path))
				{
					baseX = a.Width / 2;
					baseY = a.Height / 2;
				}*/
			}
			this.x = x + baseX;
			this.y = y + baseY;
		}

		internal Costume(bool isBg) : base("0")
		{
			bitmapResolution = 1;
			if(isBg)
			{
				baseX = 0;
				baseY = 0;
				bytes = Resources.bg;
			}
			else
			{
				baseX = 47.58949f;
				baseY = 49.920395f;
				bytes = Resources.cat;
			}
			this.x = baseX;
			this.y = baseY;
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
