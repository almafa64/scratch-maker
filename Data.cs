using Scratch_Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
			this.path = path;
			this.assetId = ID.AssetMake();
			this.dataFormat = path.Substring(path.LastIndexOf('.') + 1);
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
			this.value = value;
			sObject.Vars[name] = this;
		}
	}

	public class List : Container
	{
		internal List<object> vars = new List<object>();

		internal List(SObject sObject, string name, params object[] vars) : base(sObject, name)
		{
			if(vars != null) this.vars.AddRange(vars);
			sObject.Lists[name] = this;
		}
	}

	public class Broadcast : Ided
	{
		public Broadcast(SObject sObject, string name) : base(name)
		{
			if(sObject is Background bg) bg.Broadcasts[name] = this;
			else sObject.Project.background.Broadcasts[name] = this;
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
			sObject.Comments[name] = this;
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
			this.bitmapResolution = (byte)((dataFormat == "svg") ? 1 : 2);
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
