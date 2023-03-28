using Scratch.Properties;
using Scratch_Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

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
		internal bool show = false;
		internal int x, y, width = 0;

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
			this.md5ext = assetId + ".svg";
		}
	}
}

namespace Scratch
{
	public class Var : Container
	{
		public enum DisplayMode
		{
			Normal,
			Large,
			Slider
		}

		internal object value;
		internal double min, max;
		internal DisplayMode mode;

		internal Var(SObject sObject, string name, object value) : base(sObject, name)
		{
			if(Has(sObject, name) || BgHas(sObject, name)) throw new ArgumentException($"Variable with the name \"{name}\" already exists");

			if(value is bool b) value = Utils.Small(b);
			else if(value is Var v) value = v.value;
			else if(value is List l) value = string.Join(" ", l.vars);

			this.value = value;
			sObject._Vars[name] = this;
		}

		public Var(object value, DisplayMode mode = DisplayMode.Normal, int x = 0, int y = 0, double min = 0, double max = 100) : base(null, null)
		{
			if(value is bool b) value = Utils.Small(b);
			else if(value is Var v) value = v.value;
			else if(value is List l) value = string.Join(" ", l.vars);

			this.min = min;
			this.max = max;
			this.mode = mode;

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
		internal int height = 0;

		internal List(SObject sObject, string name, params object[] vars) : base(sObject, name)
		{
			if(Has(sObject, name) || BgHas(sObject, name)) throw new ArgumentException($"List with the name \"{name}\" already exists");

			if(vars != null)
			{
				for(int i = 0; i < vars.Length; i++)
				{
					object value = vars[i];

					if(value is bool b) vars[i] = Utils.Small(b);
					else if(value is Var v) vars[i] = v.value;
					else if(value is List l) vars[i] = string.Join(" ", l.vars);
				}
				this.vars.AddRange(vars);
			}
			sObject._Lists[name] = this;
		}

		public List(string name, params object[] vars) : base(null, name)
		{
			if(vars != null)
			{
				for(int i = 0; i < vars.Length; i++)
				{
					object value = vars[i];

					if(value is bool b) vars[i] = Utils.Small(b);
					else if(value is Var v) vars[i] = v.value;
					else if(value is List l) vars[i] = string.Join(" ", l.vars);
				}
				this.vars.AddRange(vars);
			}
		}

		public List(params object[] vars) : this(null, vars) { }

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
		public Broadcast() : base(null) {}

		internal static bool Has(SObject sObject, string name)
		{
			return sObject.Project.background._Broadcasts.ContainsKey(name);
		}
	}

	public class Comment : Ided
	{
		internal Block block = null;
		public int height;
		public bool minimized;
		public string text;
		public int width;
		public double x;
		public double y;

		public Comment(string text, double x = 200, double y = 200, bool minimized = false, int height = 200, int width = 200) : base(null)
		{
			this.text = text;
			this.minimized = minimized;
			this.height = height;
			this.width = width;
			this.x = x;
			this.y = y;
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

		public Costume(string path, float x = 0, float y = 0) : base(path, null)
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
				using(FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
				{
					fs.Seek(1, SeekOrigin.Begin);
					byte[] btype = new byte[3];
					fs.Read(btype, 0, 3);
					switch(Encoding.Default.GetString(btype))
					{
						case "PNG":
							fs.Seek(12, SeekOrigin.Current);
							byte[] bwidth = new byte[4];
							fs.Read(bwidth, 0, 4);
							Array.Reverse(bwidth);
							baseX = BitConverter.ToInt32(bwidth, 0) / 2f;
							byte[] bheight = new byte[4];
							fs.Read(bheight, 0, 4);
							Array.Reverse(bheight);
							baseY = BitConverter.ToInt32(bheight, 0) / 2f;
							break;
					}
				}
			}
			this.x = x + baseX;
			this.y = y + baseY;
		}

		internal Costume(bool isBg) : base(isBg?"backdrop":"cat")
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

		internal static bool Has(SObject sObject, string name)
		{
			return sObject._Costumes.ContainsKey(name);
		}

		internal static bool BgHas(SObject sObject, string name)
		{
			return sObject.Project.background._Costumes.ContainsKey(name);
		}
	}

	public class Sound : Accessories
	{
		internal string format = "";
		internal int rate;
		internal int sampleCount;

		[DllImport("winmm.dll", EntryPoint = "mciSendStringA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		private static extern int mciSendString(string lpstrCommand, string lpstrReturnString, int uReturnLength, int hwndCallback);

		public Sound(string path, int rate = 48000, int sampleCount = 1123) : base(path, null)
		{
			this.rate = rate;
			this.sampleCount = sampleCount;
		}

		public static string Record(string nameOfNewFile)
		{
			int iOfDot = nameOfNewFile.IndexOf('.');
			if(iOfDot != -1) nameOfNewFile = nameOfNewFile.Substring(0, iOfDot);
			string path = $"{AppDomain.CurrentDomain.BaseDirectory}{nameOfNewFile}.wav";
			mciSendString("open new Type waveaudio Alias recsound", "", 0, 0);
			mciSendString("record recsound", "", 0, 0);
			Console.WriteLine($"recording \"{nameOfNewFile}\", press Enter to stop and save ...");
			Console.ReadLine();

			mciSendString($"save recsound " + path, "", 0, 0);
			mciSendString("close recsound ", "", 0, 0);

			return path;
		}

		internal static bool Has(SObject sObject, string name)
		{
			return sObject._Sounds.ContainsKey(name);
		}

		internal static bool BgHas(SObject sObject, string name)
		{
			return sObject.Project.background._Sounds.ContainsKey(name);
		}
	}
}

namespace Scratch
{
	public struct Color
	{
		internal string hex;
		public Color(byte r, byte g, byte b)
		{
			hex = '#' + BitConverter.ToString(new byte[] { r, g, b }).Replace("-", "");
		}
		public Color(string hex)
		{
			bool hastag = hex[0] == '#';

			if(hex.Length - (hastag ? 1 : 0) != 6) throw new ArgumentException("hex length was not 6 (without the # character)");
			
			hex = hex.ToLower();
			string good = "0123456789abcdef";
			for(int i = hastag ? 1 : 0; i < hex.Length; i++)
			{
				if(good.IndexOf(hex[i]) == -1) throw new ArgumentException($"hex has no hex character \"{hex[i]}\"");
			}
			this.hex = hastag ? hex : ('#'+hex);
		}
		internal Color(string hex, int _) => this.hex = hex;
		public static Color Red		= new Color("#ff0000", 0);
		public static Color Green	= new Color("#00ff00", 0);
		public static Color Blue	= new Color("#0000ff", 0);
		public static Color Black	= new Color("#000000", 0);
		public static Color Gray	= new Color("#808080", 0);
		public static Color White	= new Color("#ffffff", 0);
		public static Color Purple	= new Color("#800080", 0);
		public static Color Yellow	= new Color("#ffff00", 0);
		public static Color Aqua	= new Color("#00ffff", 0);
		public static Color Pink	= new Color("#ff00ff", 0);
	}
}