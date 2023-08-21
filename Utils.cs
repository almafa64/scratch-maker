using Scratch;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading;
using static Scratch.Operators;
using static Scratch.Project;

namespace Scratch_Utils
{
	internal static class ID
	{
		//internal static List<string> Ids = new List<string>();
		private const int chuckSize = 50;
		private static string[] Ids = new string[chuckSize];
		private static string[] AssetIds = new string[chuckSize];
		private static int newIdIndex = 0;
		private static int newAssetIdIndex = 0;
		private static readonly System.Random rd = new System.Random();
		private static readonly System.Random asserRd = new System.Random();
		private const string ascii = "!#%()*+,-./:;=?@[]^_`{|}~ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
		private const string assetAscii = "0123456789ABCDEFabcdef";

		internal static string Make()
		{
			StringBuilder sb = new StringBuilder(20);

			string random;
			do
			{
				sb.Clear();

				for(byte i = 0; i < 20; i++)
				{
					sb.Append(ascii[rd.Next(ascii.Length)]);
				}

				random = sb.ToString();
			} while(Contains(random));

			Add(random);
			return random;
		}

		internal static string AssetMake()
		{
			StringBuilder sb = new StringBuilder(32);

			string random;
			do
			{
				sb.Clear();

				for(byte i = 0; i < 32; i++)
				{
					sb.Append(assetAscii[asserRd.Next(assetAscii.Length)]);
				}

				random = sb.ToString();
			} while(AssetContains(random));


			AssetAdd(random);
			return random;
		}

		private static void Add(string add)
		{
			Ids[newIdIndex] = add;
			if(++newIdIndex >= Ids.Length)
			{
				string[] tmp = new string[Ids.Length + chuckSize];
				Ids.CopyTo(tmp, 0);
				Ids = tmp;
			}
		}

		private static bool Contains(string check)
		{
			for(int i = 0; i < Ids.Length; i++)
			{
				if(check == Ids[i]) return true;
				if(Ids[i] == null) break;
			}
			return false;
		}

		internal static void Clear()
		{
			Ids = new string[chuckSize];
		}

		private static void AssetAdd(string add)
		{
			AssetIds[newAssetIdIndex] = add;
			if(++newAssetIdIndex >= AssetIds.Length)
			{
				string[] tmp = new string[AssetIds.Length + chuckSize];
				AssetIds.CopyTo(tmp, 0);
				AssetIds = tmp;
			}
		}

		private static bool AssetContains(string check)
		{
			for(int i = 0; i < AssetIds.Length; i++)
			{
				if(check == AssetIds[i]) return true;
				if(AssetIds[i] == null) break;
			}
			return false;
		}

		internal static void AssetClear()
		{
			AssetIds = new string[chuckSize];
		}
	}

	internal static class TypeCheck
	{
		internal static void Check(string blockName, string nameOfVar, object val, Types notAcceptedTypes)
		{
			Types tmp = Check(val);

			if(tmp == Types.None) throw new ArgumentException($"{nameOfVar} is null or other object on block \"{blockName}\", which it cannot be");

			if(notAcceptedTypes.HasFlag(tmp))
			{
				throw new ArgumentException($"{nameOfVar} is {typeof(Types).GetEnumName(tmp)} on block \"{blockName}\", which it cannot be");
			}
		}

		internal static Types Check(object o)
		{
			if(o == null) return Types.None;
			Type t = o.GetType();
			if(t == typeof(int) || t == typeof(float) || t == typeof(double) || t == typeof(uint) || t == typeof(short) || t == typeof(ushort) || t == typeof(byte) || t == typeof(sbyte) || t == typeof(long) || t == typeof(ulong) || t == typeof(decimal))
			{
				return (Convert.ToDouble(o) >= 0) ? Types.PositiveNumber : Types.Number;
			}
			else if(t == typeof(string) || t == typeof(bool)) return Types.String;
			else if(t == typeof(Var)) return Types.Variable;
			else if(t == typeof(List)) return Types.List;
			else if(t.IsEnum) return Types.Enum;
			else if(t == typeof(Sprite)) return Types.Sprite;
			else if(t == typeof(MyBlock.MyBlockVar)) return Types.MyBlockVar;

			return Types.None;
		}

		internal static void BaseCheck(object o, string blockName, int index)
		{
			switch(Check(o))
			{
				case Types.None:
					throw new ArgumentException($"Argument is null at place {index} on block {blockName}");
				case Types.Variable:
					if((o as Var).value == null) throw new ArgumentException($"Not initalized variable at place {index} on block \"{blockName}\"");
					break;
					/*case AcceptedTypes.List:
						if((o as List).vars == null) throw new ArgumentException($"Not initalized list at place {i} on block \"{blockName}\"");
						break;*/
			}
		}
	}

	internal class Compiler
	{
		internal Compiler(Project pr)
		{
			string newPath = Path.GetFullPath(pr.name);

			if(!Directory.Exists(newPath)) Directory.CreateDirectory(newPath);
			else Directory.Delete(newPath, true);

			Directory.CreateDirectory($"{newPath}\\build");

			StringBuilder fileText = new StringBuilder(2000).Append("{\"extensions\":[");
			if(pr.extensions != Extensions.None)
			{
				int tmp = (int)pr.extensions;
				int flagMask = 1;
				while(flagMask < (int)Extensions.All)
				{
					switch((Extensions)(tmp & flagMask))
					{
						case Extensions.TextToSpeech:
							fileText.Append("\"text2speech\",");
							break;
						case Extensions.Pen:
							fileText.Append("\"pen\",");
							break;
						case Extensions.ForceAndAceleration:
							fileText.Append("\"gdxfor\",");
							break;
						case Extensions.WeDo2:
							fileText.Append("\"wedo2\",");
							break;
						case Extensions.BOOST:
							fileText.Append("\"boost\",");
							break;
						case Extensions.EV3:
							fileText.Append("\"ev3\",");
							break;
						case Extensions.Microbit:
							fileText.Append("\"microbit\",");
							break;
						case Extensions.MakeyMakey:
							fileText.Append("\"makeymakey\",");
							break;
						case Extensions.Translate:
							fileText.Append("\"translate\",");
							break;
						case Extensions.VideoSensing:
							fileText.Append("\"videoSensing\",");
							break;
						case Extensions.Music:
							fileText.Append("\"music\",");
							break;
					}
					flagMask <<= 1;
				}
				Utils.RemoveLast(fileText);
			}

			fileText.Append("],\"meta\":{\"semver\":\"3.0.0\",\"vm\":\"1.4.6\",\"agent\":\"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/109.0.0.0 Safari/537.36\"},\"monitors\":[");

			Dictionary<string, Sprite>.ValueCollection sprites = pr._sprites.Values;

			StringBuilder monitorText = new StringBuilder();
			bool didMonitor = DoMonitorStuff(monitorText, pr.background._Vars.Values, pr.background._Lists.Values);
			foreach(Sprite sprite in sprites)
			{
				bool a = DoMonitorStuff(monitorText, sprite._Vars.Values, sprite._Lists.Values);
				if(a) didMonitor = true;
			}
			if(didMonitor) Utils.RemoveLast(monitorText);

			fileText.Append(monitorText);

			fileText.Append("],\"targets\":[");
			DoSpriteStuff(fileText, false, pr.background);
			foreach(Sprite sprite in sprites)
			{
				DoSpriteStuff(fileText, true, sprite);
			}

			Utils.RemoveLast(fileText);

			fileText.Append("]}");

			File.WriteAllText($"{newPath}\\build\\project.json", fileText.ToString());

			ZipFile.CreateFromDirectory($"{newPath}\\build", $"{newPath}\\{pr.name}.sb3");

			if(pr.openFolder) Process.Start("explorer.exe", $"{newPath}");
		}

		private static bool DoMonitorStuff(StringBuilder sb, Dictionary<string, Var>.ValueCollection vars, Dictionary<string, List>.ValueCollection lists)
		{
			void Base(Container c, Var.DisplayMode mode, string opcode)
			{
				Var v = null;
				List l = null;
				bool isVar;
				if(c is Var var)
				{
					v = var;
					isVar = true;
				}
				else
				{
					isVar = false;
					l = c as List;
				}

				sb.Append("{\"x\":");
				sb.Append(c.x);

				sb.Append(",\"y\":");
				sb.Append(c.y);

				sb.Append(",\"width\":");
				sb.Append(c.width);

				sb.Append(",\"height\":");
				if(isVar)
				{
					sb.Append(0);

					sb.Append(",\"sliderMax\":");
					sb.Append(v.max);

					sb.Append(",\"sliderMin\":");
					sb.Append(v.min);
				}
				else sb.Append(l.height);

				sb.Append(",\"visible\":");
				sb.Append(Utils.Small(c.show));

				string paramMode;

				sb.Append(",\"value\":");
				if(isVar)
				{
					Types types = TypeCheck.Check(v.value);
					sb.Append((types == Types.Number || types == Types.PositiveNumber) ? v.value : $"\"{v.value}\"");

					sb.Append(",\"isDiscrete\":");
					sb.Append(Utils.Small((Math.Floor(v.min) - Math.Ceiling(v.min)) + (Math.Floor(v.max) - Math.Ceiling(v.max)) == 0));

					sb.Append(",\"mode\":\"");
					string sW;
					switch(v.mode)
					{
						case Var.DisplayMode.Normal:	sW = "default"; break;
						case Var.DisplayMode.Large:		sW = "large"; break;
						case Var.DisplayMode.Slider:	sW = "slider"; break;
						default: throw new ArgumentException("Are you joking with me?");
					}
					sb.Append(sW);

					sb.Append("\",\"opcode\":\"data_variable\"");

					paramMode = "VARIABLE";
				}
				else
				{
					sb.Append('[');
					if(l.vars.Count != 0)
					{
						StringBuilder sbL = new StringBuilder();
						foreach(object o in l.vars)
						{
							sbL.Append('"');
							sbL.Append(o.ToString());
							sbL.Append("\",");
						}
						Utils.RemoveLast(sbL);
					}
					sb.Append(']');

					sb.Append(",\"mode\":\"");
					sb.Append("list");

					sb.Append("\",\"opcode\":\"data_listcontents\"");

					paramMode = "LIST";
				}

				sb.Append(",\"spriteName\":");
				sb.Append((c.sObject is Sprite s && s.name != null) ? $"\"{s.name}\"" : "null");

				sb.Append(",\"id\":\"");
				sb.Append(c.Id);

				sb.Append("\",\"params\":{\"");
				sb.Append(paramMode);
				sb.Append("\":\"");
				sb.Append(c.Name);

				sb.Append("\"}},");
			}

			foreach(Var v in vars)
			{
				Base(v, v.mode, "data_variable");
			}

			foreach(List l in lists)
			{
				Base(l, Var.DisplayMode.Normal, "data_listcontents");
			}

			return lists.Count + vars.Count != 0;
		}

		private static void DoSpriteStuff(StringBuilder fileText, bool isSprite, SObject sObject)
		{
			string newPath = sObject.Project.name;

			fileText.Append("{\"blocks\":{");
			if(Utils.CountBlock(sObject) != 0)
			{
				foreach(Column column in sObject.columns)
				{
					foreach(Block block in column.blocks)
					{
						if(block.comment != null) sObject._Comments.Add(block.comment);

						fileText.Append('"');
						BlockArgs tmpArgs = block.args;
						fileText.Append(tmpArgs.Id);

						fileText.Append("\":{\"fields\":{");
						if(tmpArgs.Fields != null) fileText.Append(tmpArgs.Fields);

						fileText.Append("},\"inputs\":{");
						if(tmpArgs.Inputs != null) fileText.Append(tmpArgs.Inputs);

						if(tmpArgs.Mutatator != null)
						{
							Mutator m = tmpArgs.Mutatator.Value;

							fileText.Append("},\"mutation\":{");

							if(m.hasNext != null)
							{
								if((int)m.hasNext == 0) fileText.Append("\"hasnext\":\"false");
								else fileText.Append("\"hasnext\":\"true");
							}
							else
							{
								bool hasDef = m.argumentDefaults != null;
								bool hasId = m.argumentIds != null;

								if(hasDef)
								{
									fileText.Append("\"argumentdefaults\":\"");
									fileText.Append(m.argumentDefaults);
								}

								if(hasId)
								{
									fileText.Append($"{(hasDef ? "\"," : "")}\"argumentids\":\"");
									fileText.Append(m.argumentIds);
								}

								if(m.argumentNames != null)
								{
									fileText.Append($"{((hasDef || hasId) ? "\"," : "")}\"argumentnames\":\"");
									fileText.Append(m.argumentNames);
								}

								fileText.Append("\",\"proccode\":\"");
								fileText.Append(m.proCode);

								fileText.Append("\",\"warp\":\"");
								fileText.Append(Utils.Small(m.warp));
							}

							fileText.Append("\",\"children\":[],\"tagName\":\"mutation\"");
						}

						fileText.Append("},\"next\":");
						if(tmpArgs.NextId != null)
						{
							fileText.Append('"');
							fileText.Append(tmpArgs.NextId);
							fileText.Append('"');
						}
						else fileText.Append("null");

						fileText.Append(",\"parent\":");
						if(tmpArgs.ParentId != null)
						{
							fileText.Append('"');
							fileText.Append(tmpArgs.ParentId);
							fileText.Append('"');
						}
						else fileText.Append("null");

						if(block.comment != null)
						{
							fileText.Append(",\"comment\":\"");
							fileText.Append(block.comment.Id);
							fileText.Append('"');
						}

						fileText.Append(",\"opcode\":\"");
						fileText.Append(tmpArgs.OpCode);
						fileText.Append('"');

						fileText.Append(",\"shadow\":");
						fileText.Append(Utils.Small(tmpArgs.Shadow));

						fileText.Append(",\"topLevel\":");
						fileText.Append(Utils.Small(tmpArgs.TopLevel));

						if(tmpArgs.TopLevel)
						{
							fileText.Append(",\"x\":");
							fileText.Append(column.x);
							fileText.Append(",\"y\":");
							fileText.Append(column.y);
						}
						fileText.Append("},");
					}
				}
				Utils.RemoveLast(fileText);
			}

			fileText.Append("},\"broadcasts\":{");
			if(!isSprite)
			{
				Background bg = sObject as Background;
				if(bg._Broadcasts.Count != 0)
				{
					foreach(KeyValuePair<string, Broadcast> map in bg._Broadcasts)
					{
						fileText.Append('"');
						fileText.Append(map.Value.Id);
						fileText.Append("\":\"");
						fileText.Append(map.Key);
						fileText.Append("\",");
					}
					Utils.RemoveLast(fileText);
				}
			}

			fileText.Append("},\"comments\":{");
			if(sObject._Comments.Count != 0)
			{
				foreach(Comment cm in sObject._Comments)
				{
					fileText.Append('"');
					fileText.Append(cm.Id);
					fileText.Append("\":{\"blockId\":");

					if(cm.block == null)
					{
						fileText.Append("null");

						fileText.Append(",\"x\":");
						fileText.Append(cm.x);

						fileText.Append(",\"y\":");
						fileText.Append(cm.y);
					}
					else
					{
						fileText.Append("\"");
						fileText.Append(cm.block.args.Id);
						fileText.Append("\"");
					}

					fileText.Append(",\"height\":");
					fileText.Append(cm.height);

					fileText.Append(",\"width\":");
					fileText.Append(cm.width);

					fileText.Append(",\"minimized\":");
					fileText.Append(Utils.Small(cm.minimized));

					fileText.Append(",\"text\":\"");
					fileText.Append(cm.text);
					fileText.Append("\"},");
				}
				Utils.RemoveLast(fileText);
			}

			bool embedFile = false;

			fileText.Append("},\"costumes\":[");
			if(sObject._Costumes.Count == 0)
			{
				if(isSprite) sObject._Costumes["cat"] = new Costume(false);
				else sObject._Costumes["bg"] = new Costume(true);
				embedFile = true;
			}
			foreach(KeyValuePair<string, Costume> map in sObject._Costumes)
			{
				Costume ct = map.Value;
				fileText.Append("{\"assetId\":\"");
				fileText.Append(ct.assetId);

				fileText.Append("\",\"dataFormat\":\"");
				fileText.Append(ct.dataFormat);

				fileText.Append("\",\"md5ext\":\"");
				fileText.Append(ct.md5ext);

				fileText.Append("\",\"name\":\"");
				fileText.Append(ct.Name);

				fileText.Append("\",\"bitmapResolution\":");
				fileText.Append(ct.bitmapResolution);

				fileText.Append(",\"rotationCenterX\":");
				fileText.Append(ct.x);

				fileText.Append(",\"rotationCenterY\":");
				fileText.Append(ct.y);

				fileText.Append("},");

				if(!embedFile) File.Copy(ct.path, $"{newPath}\\build\\{ct.md5ext}");
				else File.WriteAllBytes($"{newPath}\\build\\{ct.md5ext}", ct.bytes);
			}
			Utils.RemoveLast(fileText);

			fileText.Append("],\"sounds\":[");
			if(sObject._Sounds.Count != 0)
			{
				foreach(KeyValuePair<string, Sound> map in sObject._Sounds)
				{
					Sound sd = map.Value;
					fileText.Append("{\"assetId\":\"");
					fileText.Append(sd.assetId);

					fileText.Append("\",\"dataFormat\":\"");
					fileText.Append(sd.dataFormat);

					fileText.Append("\",\"md5ext\":\"");
					fileText.Append(sd.md5ext);

					fileText.Append("\",\"name\":\"");
					fileText.Append(sd.Name);

					fileText.Append("\",\"format\":\"");
					fileText.Append(sd.format);

					fileText.Append("\",\"rate\":");
					fileText.Append(sd.rate);

					fileText.Append(",\"sampleCount\":");
					fileText.Append(sd.sampleCount);

					fileText.Append("},");

					File.Copy(sd.path, $"{newPath}\\build\\{sd.md5ext}");
				}
				Utils.RemoveLast(fileText);
			}

			fileText.Append("],\"lists\":{");
			if(sObject._Lists.Count != 0)
			{
				foreach(KeyValuePair<string, List> map in sObject._Lists)
				{
					List ls = map.Value;
					fileText.Append('"');
					fileText.Append(ls.Id);

					fileText.Append("\":[\"");
					fileText.Append(ls.Name);

					fileText.Append("\",[");
					if(ls.vars.Count != 0)
					{
						foreach(object o in ls.vars)
						{
							fileText.Append('"');
							fileText.Append(o);
							fileText.Append("\",");
						}
						Utils.RemoveLast(fileText);
					}

					fileText.Append("]],");
				}
				Utils.RemoveLast(fileText);
			}

			fileText.Append("},\"variables\":{");
			if(sObject._Vars.Count != 0)
			{
				foreach(KeyValuePair<string, Var> map in sObject._Vars)
				{
					Var var = map.Value;
					fileText.Append('"');
					fileText.Append(var.Id);

					fileText.Append("\":[\"");
					fileText.Append(var.Name);

					fileText.Append("\",\"");
					fileText.Append(var.value);

					fileText.Append("\"],");
				}
				Utils.RemoveLast(fileText);
			}

			fileText.Append("},\"currentCostume\":");
			fileText.Append(sObject.CurrentCostume);

			fileText.Append(",\"layerOrder\":");
			fileText.Append(sObject.LayerOrder);

			if(isSprite)
			{
				Sprite tmpSprite = sObject as Sprite;

				fileText.Append(",\"isStage\":false,\"name\":\"");
				fileText.Append(tmpSprite.name);

				fileText.Append("\",\"direction\":");
				fileText.Append(tmpSprite.direction);

				fileText.Append(",\"draggable\":");
				fileText.Append(Utils.Small(tmpSprite.draggable));

				fileText.Append(",\"rotationStyle\":\"");
				switch(tmpSprite.rotationStyle)
				{
					case RotationStyle.AllAround:
						fileText.Append("all around");
						break;
					case RotationStyle.LeftRight:
						fileText.Append("left-right");
						break;
					case RotationStyle.DontRotate:
						fileText.Append("don't rotate");
						break;
				}

				fileText.Append("\",\"size\":");
				fileText.Append(tmpSprite.size);

				fileText.Append(",\"visible\":");
				fileText.Append(Utils.Small(tmpSprite.visible));

				fileText.Append(",\"x\":");
				fileText.Append(tmpSprite.x);

				fileText.Append(",\"y\":");
				fileText.Append(tmpSprite.y);
			}
			else
			{
				Background tmpBg = sObject as Background;

				fileText.Append(",\"isStage\":true,\"name\":\"Stage\",\"tempo\":");
				fileText.Append(tmpBg.tempo);

				if(tmpBg.textToSpeechLanguage != TextToSpeechLanguages.None)
				{
					fileText.Append(",\"textToSpeechLanguage\":\"");
					switch(tmpBg.textToSpeechLanguage)
					{
						case TextToSpeechLanguages.English:
							fileText.Append("en");
							break;
					}

					fileText.Append("\",\"videoState\":\"");
				}
				else fileText.Append(",\"textToSpeechLanguage\":null,\"videoState\":\"");

				fileText.Append(tmpBg.videoOn ? "on" : "off");

				fileText.Append("\",\"videoTransparency\":");
				fileText.Append(tmpBg.videoTransparency);

				fileText.Append(",\"volume\":");
				fileText.Append(tmpBg.volume);
			}

			fileText.Append("},");
		}
	}

	internal static class Utils
	{
		internal static int CountBlock(SObject sObject)
		{
			int tmp = 0;
			for(int i = 0; i < sObject.columns.Count; i++)
			{
				tmp += sObject.columns[i].blocks.Count;
			}
			return tmp;
		}

		internal static void RemoveLast(StringBuilder sb, int length = 1)
		{
			sb.Remove(sb.Length - length, length);
		}

		internal static string Small(bool b)
		{
			return b.ToString().ToLower();
		}
	}
}
