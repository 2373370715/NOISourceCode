using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using ArabicSupport;
using Klei;
using KMod;
using Steamworks;
using STRINGS;
using TMPro;
using UnityEngine;

// Token: 0x020014DD RID: 5341
public static class Localization
{
	// Token: 0x17000714 RID: 1812
	// (get) Token: 0x06006EEC RID: 28396 RVA: 0x000ED280 File Offset: 0x000EB480
	public static TMP_FontAsset FontAsset
	{
		get
		{
			return Localization.sFontAsset;
		}
	}

	// Token: 0x17000715 RID: 1813
	// (get) Token: 0x06006EED RID: 28397 RVA: 0x000ED287 File Offset: 0x000EB487
	public static bool IsRightToLeft
	{
		get
		{
			return Localization.sLocale != null && Localization.sLocale.IsRightToLeft;
		}
	}

	// Token: 0x06006EEE RID: 28398 RVA: 0x002FED60 File Offset: 0x002FCF60
	private static IEnumerable<Type> CollectLocStringTreeRoots(string locstrings_namespace, Assembly assembly)
	{
		return from type in assembly.GetTypes()
		where type.IsClass && type.Namespace == locstrings_namespace && !type.IsNested
		select type;
	}

	// Token: 0x06006EEF RID: 28399 RVA: 0x002FED94 File Offset: 0x002FCF94
	private static Dictionary<string, object> MakeRuntimeLocStringTree(Type locstring_tree_root)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		foreach (FieldInfo fieldInfo in locstring_tree_root.GetFields())
		{
			if (!(fieldInfo.FieldType != typeof(LocString)))
			{
				if (!fieldInfo.IsStatic)
				{
					DebugUtil.DevLogError("LocString fields must be static, skipping. " + fieldInfo.Name);
				}
				else
				{
					LocString locString = (LocString)fieldInfo.GetValue(null);
					if (locString == null)
					{
						global::Debug.LogError("Tried to generate LocString for " + fieldInfo.Name + " but it is null so skipping");
					}
					else
					{
						dictionary[fieldInfo.Name] = locString.text;
					}
				}
			}
		}
		foreach (Type type in locstring_tree_root.GetNestedTypes())
		{
			Dictionary<string, object> dictionary2 = Localization.MakeRuntimeLocStringTree(type);
			if (dictionary2.Count > 0)
			{
				dictionary[type.Name] = dictionary2;
			}
		}
		return dictionary;
	}

	// Token: 0x06006EF0 RID: 28400 RVA: 0x002FEE7C File Offset: 0x002FD07C
	private static void WriteStringsTemplate(string path, StreamWriter writer, Dictionary<string, object> runtime_locstring_tree)
	{
		List<string> list = new List<string>(runtime_locstring_tree.Keys);
		list.Sort();
		foreach (string text in list)
		{
			string text2 = path + "." + text;
			object obj = runtime_locstring_tree[text];
			if (obj.GetType() != typeof(string))
			{
				Localization.WriteStringsTemplate(text2, writer, obj as Dictionary<string, object>);
			}
			else
			{
				string text3 = obj as string;
				text3 = text3.Replace("\\", "\\\\");
				text3 = text3.Replace("\"", "\\\"");
				text3 = text3.Replace("\n", "\\n");
				text3 = text3.Replace("’", "'");
				text3 = text3.Replace("“", "\\\"");
				text3 = text3.Replace("”", "\\\"");
				text3 = text3.Replace("…", "...");
				writer.WriteLine("#. " + text2);
				writer.WriteLine("msgctxt \"{0}\"", text2);
				writer.WriteLine("msgid \"" + text3 + "\"");
				writer.WriteLine("msgstr \"\"");
				writer.WriteLine("");
			}
		}
	}

	// Token: 0x06006EF1 RID: 28401 RVA: 0x002FEFFC File Offset: 0x002FD1FC
	public static void GenerateStringsTemplate(string locstrings_namespace, Assembly assembly, string output_filename, Dictionary<string, object> current_runtime_locstring_forest)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		foreach (Type type in Localization.CollectLocStringTreeRoots(locstrings_namespace, assembly))
		{
			Dictionary<string, object> dictionary2 = Localization.MakeRuntimeLocStringTree(type);
			if (dictionary2.Count > 0)
			{
				dictionary[type.Name] = dictionary2;
			}
		}
		if (current_runtime_locstring_forest != null)
		{
			dictionary.Concat(current_runtime_locstring_forest);
		}
		using (StreamWriter streamWriter = new StreamWriter(output_filename, false, new UTF8Encoding(false)))
		{
			streamWriter.WriteLine("msgid \"\"");
			streamWriter.WriteLine("msgstr \"\"");
			streamWriter.WriteLine("\"Application: Oxygen Not Included\"");
			streamWriter.WriteLine("\"POT Version: 2.0\"");
			streamWriter.WriteLine("");
			Localization.WriteStringsTemplate(locstrings_namespace, streamWriter, dictionary);
		}
		DebugUtil.LogArgs(new object[]
		{
			"Generated " + output_filename
		});
	}

	// Token: 0x06006EF2 RID: 28402 RVA: 0x002FF0F8 File Offset: 0x002FD2F8
	public static void GenerateStringsTemplate(Type locstring_tree_root, string output_folder)
	{
		output_folder = FileSystem.Normalize(output_folder);
		if (!FileUtil.CreateDirectory(output_folder, 5))
		{
			return;
		}
		Localization.GenerateStringsTemplate(locstring_tree_root.Namespace, Assembly.GetAssembly(locstring_tree_root), FileSystem.Normalize(Path.Combine(output_folder, string.Format("{0}_template.pot", locstring_tree_root.Namespace.ToLower()))), null);
	}

	// Token: 0x06006EF3 RID: 28403 RVA: 0x002FF14C File Offset: 0x002FD34C
	public static void Initialize()
	{
		DebugUtil.LogArgs(new object[]
		{
			"Localization.Initialize!"
		});
		bool flag = false;
		switch (Localization.GetSelectedLanguageType())
		{
		case Localization.SelectedLanguageType.None:
			Localization.sFontAsset = Localization.GetFont(Localization.GetDefaultLocale().FontName);
			break;
		case Localization.SelectedLanguageType.Preinstalled:
		{
			string currentLanguageCode = Localization.GetCurrentLanguageCode();
			if (!string.IsNullOrEmpty(currentLanguageCode))
			{
				DebugUtil.LogArgs(new object[]
				{
					"Localization Initialize... Preinstalled localization"
				});
				DebugUtil.LogArgs(new object[]
				{
					" -> ",
					currentLanguageCode
				});
				Localization.LoadPreinstalledTranslation(currentLanguageCode);
			}
			else
			{
				flag = true;
			}
			break;
		}
		case Localization.SelectedLanguageType.UGC:
			if (LanguageOptionsScreen.HasInstalledLanguage())
			{
				DebugUtil.LogArgs(new object[]
				{
					"Localization Initialize... Mod-based localization"
				});
				string savedLanguageMod = LanguageOptionsScreen.GetSavedLanguageMod();
				if (LanguageOptionsScreen.SetCurrentLanguage(savedLanguageMod))
				{
					DebugUtil.LogArgs(new object[]
					{
						" -> Loaded language from mod: " + savedLanguageMod
					});
				}
				else
				{
					DebugUtil.LogArgs(new object[]
					{
						" -> Failed to load language from mod: " + savedLanguageMod
					});
				}
			}
			else
			{
				flag = true;
			}
			break;
		}
		if (flag)
		{
			Localization.ClearLanguage();
		}
	}

	// Token: 0x06006EF4 RID: 28404 RVA: 0x002FF250 File Offset: 0x002FD450
	public static void VerifyTranslationModSubscription(GameObject context)
	{
		if (Localization.GetSelectedLanguageType() != Localization.SelectedLanguageType.UGC)
		{
			return;
		}
		if (!SteamManager.Initialized)
		{
			return;
		}
		if (LanguageOptionsScreen.HasInstalledLanguage())
		{
			return;
		}
		PublishedFileId_t publishedFileId_t = new PublishedFileId_t((ulong)KPlayerPrefs.GetInt("InstalledLanguage", (int)PublishedFileId_t.Invalid.m_PublishedFileId));
		Label rhs = new Label
		{
			distribution_platform = Label.DistributionPlatform.Steam,
			id = publishedFileId_t.ToString()
		};
		string arg = UI.FRONTEND.TRANSLATIONS_SCREEN.UNKNOWN;
		foreach (Mod mod in Global.Instance.modManager.mods)
		{
			if (mod.label.Match(rhs))
			{
				arg = mod.title;
				break;
			}
		}
		Localization.ClearLanguage();
		KScreen component = KScreenManager.AddChild(context, ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject).GetComponent<KScreen>();
		component.Activate();
		ConfirmDialogScreen component2 = component.GetComponent<ConfirmDialogScreen>();
		string title_text = UI.CONFIRMDIALOG.DIALOG_HEADER;
		string text = string.Format(UI.FRONTEND.TRANSLATIONS_SCREEN.MISSING_LANGUAGE_PACK, arg);
		string confirm_text = UI.FRONTEND.TRANSLATIONS_SCREEN.RESTART;
		component2.PopupConfirmDialog(text, new System.Action(App.instance.Restart), null, null, null, title_text, confirm_text, null, null);
	}

	// Token: 0x06006EF5 RID: 28405 RVA: 0x002FF398 File Offset: 0x002FD598
	public static void LoadPreinstalledTranslation(string code)
	{
		if (!string.IsNullOrEmpty(code) && code != Localization.DEFAULT_LANGUAGE_CODE)
		{
			string preinstalledLocalizationFilePath = Localization.GetPreinstalledLocalizationFilePath(code);
			if (Localization.LoadLocalTranslationFile(Localization.SelectedLanguageType.Preinstalled, preinstalledLocalizationFilePath))
			{
				KPlayerPrefs.SetString(Localization.SELECTED_LANGUAGE_CODE_KEY, code);
				return;
			}
		}
		else
		{
			Localization.ClearLanguage();
		}
	}

	// Token: 0x06006EF6 RID: 28406 RVA: 0x000ED29C File Offset: 0x000EB49C
	public static bool LoadLocalTranslationFile(Localization.SelectedLanguageType source, string path)
	{
		if (!File.Exists(path))
		{
			return false;
		}
		bool flag = Localization.LoadTranslationFromLines(File.ReadAllLines(path, Encoding.UTF8));
		if (flag)
		{
			KPlayerPrefs.SetString(Localization.SELECTED_LANGUAGE_TYPE_KEY, source.ToString());
			return flag;
		}
		Localization.ClearLanguage();
		return flag;
	}

	// Token: 0x06006EF7 RID: 28407 RVA: 0x002FF3DC File Offset: 0x002FD5DC
	private static bool LoadTranslationFromLines(string[] lines)
	{
		if (lines == null || lines.Length == 0)
		{
			return false;
		}
		Localization.sLocale = Localization.GetLocale(lines);
		DebugUtil.LogArgs(new object[]
		{
			" -> Locale is now ",
			Localization.sLocale.ToString()
		});
		bool flag = Localization.LoadTranslation(lines, false);
		if (flag)
		{
			Localization.currentFontName = Localization.GetFontName(lines);
			Localization.SwapToLocalizedFont(Localization.currentFontName);
		}
		return flag;
	}

	// Token: 0x06006EF8 RID: 28408 RVA: 0x002FF440 File Offset: 0x002FD640
	public static bool LoadTranslation(string[] lines, bool isTemplate = false)
	{
		bool result;
		try
		{
			Localization.OverloadStrings(Localization.ExtractTranslatedStrings(lines, isTemplate));
			result = true;
		}
		catch (Exception ex)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				ex
			});
			result = false;
		}
		return result;
	}

	// Token: 0x06006EF9 RID: 28409 RVA: 0x000ED2D8 File Offset: 0x000EB4D8
	public static Dictionary<string, string> LoadStringsFile(string path, bool isTemplate)
	{
		return Localization.ExtractTranslatedStrings(File.ReadAllLines(path, Encoding.UTF8), isTemplate);
	}

	// Token: 0x06006EFA RID: 28410 RVA: 0x002FF484 File Offset: 0x002FD684
	public static Dictionary<string, string> ExtractTranslatedStrings(string[] lines, bool isTemplate = false)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		Localization.Entry entry = default(Localization.Entry);
		string key = isTemplate ? "msgid" : "msgstr";
		for (int i = 0; i < lines.Length; i++)
		{
			string text = lines[i];
			if (text == null || text.Length == 0)
			{
				entry = default(Localization.Entry);
			}
			else
			{
				string parameter = Localization.GetParameter("msgctxt", i, lines);
				if (parameter != null)
				{
					entry.msgctxt = parameter;
				}
				parameter = Localization.GetParameter(key, i, lines);
				if (parameter != null)
				{
					entry.msgstr = parameter;
				}
			}
			if (entry.IsPopulated)
			{
				dictionary[entry.msgctxt] = entry.msgstr;
				entry = default(Localization.Entry);
			}
		}
		return dictionary;
	}

	// Token: 0x06006EFB RID: 28411 RVA: 0x002FF530 File Offset: 0x002FD730
	private static string FixupString(string result)
	{
		result = result.Replace("\\n", "\n");
		result = result.Replace("\\\"", "\"");
		result = result.Replace("<style=“", "<style=\"");
		result = result.Replace("”>", "\">");
		result = result.Replace("<color=^p", "<color=#");
		return result;
	}

	// Token: 0x06006EFC RID: 28412 RVA: 0x002FF598 File Offset: 0x002FD798
	private static string GetParameter(string key, int idx, string[] all_lines)
	{
		if (!all_lines[idx].StartsWith(key))
		{
			return null;
		}
		List<string> list = new List<string>();
		string text = all_lines[idx];
		text = text.Substring(key.Length + 1, text.Length - key.Length - 1);
		list.Add(text);
		for (int i = idx + 1; i < all_lines.Length; i++)
		{
			string text2 = all_lines[i];
			if (!text2.StartsWith("\""))
			{
				break;
			}
			list.Add(text2);
		}
		string text3 = "";
		foreach (string text4 in list)
		{
			if (text4.EndsWith("\r"))
			{
				text4 = text4.Substring(0, text4.Length - 1);
			}
			text4 = text4.Substring(1, text4.Length - 2);
			text4 = Localization.FixupString(text4);
			text3 += text4;
		}
		return text3;
	}

	// Token: 0x06006EFD RID: 28413 RVA: 0x002FF698 File Offset: 0x002FD898
	private static void AddAssembly(string locstrings_namespace, Assembly assembly)
	{
		List<Assembly> list;
		if (!Localization.translatable_assemblies.TryGetValue(locstrings_namespace, out list))
		{
			list = new List<Assembly>();
			Localization.translatable_assemblies.Add(locstrings_namespace, list);
		}
		list.Add(assembly);
	}

	// Token: 0x06006EFE RID: 28414 RVA: 0x000ED2EB File Offset: 0x000EB4EB
	public static void AddAssembly(Assembly assembly)
	{
		Localization.AddAssembly("STRINGS", assembly);
	}

	// Token: 0x06006EFF RID: 28415 RVA: 0x002FF6D0 File Offset: 0x002FD8D0
	public static void RegisterForTranslation(Type locstring_tree_root)
	{
		Assembly assembly = Assembly.GetAssembly(locstring_tree_root);
		Localization.AddAssembly(locstring_tree_root.Namespace, assembly);
		string parent_path = locstring_tree_root.Namespace + ".";
		foreach (Type type in Localization.CollectLocStringTreeRoots(locstring_tree_root.Namespace, assembly))
		{
			LocString.CreateLocStringKeys(type, parent_path);
		}
	}

	// Token: 0x06006F00 RID: 28416 RVA: 0x002FF748 File Offset: 0x002FD948
	public static void OverloadStrings(Dictionary<string, string> translated_strings)
	{
		string text = "";
		string text2 = "";
		string text3 = "";
		foreach (KeyValuePair<string, List<Assembly>> keyValuePair in Localization.translatable_assemblies)
		{
			foreach (Assembly assembly in keyValuePair.Value)
			{
				foreach (Type type in Localization.CollectLocStringTreeRoots(keyValuePair.Key, assembly))
				{
					string path = keyValuePair.Key + "." + type.Name;
					Localization.OverloadStrings(translated_strings, path, type, ref text, ref text2, ref text3);
				}
			}
		}
		if (!string.IsNullOrEmpty(text))
		{
			DebugUtil.LogArgs(new object[]
			{
				"TRANSLATION ERROR! The following have missing or mismatched parameters:\n" + text
			});
		}
		if (!string.IsNullOrEmpty(text2))
		{
			DebugUtil.LogArgs(new object[]
			{
				"TRANSLATION ERROR! The following have mismatched <link> tags:\n" + text2
			});
		}
		if (!string.IsNullOrEmpty(text3))
		{
			DebugUtil.LogArgs(new object[]
			{
				"TRANSLATION ERROR! The following do not have the same amount of <link> tags as the english string which can cause nested link errors:\n" + text3
			});
		}
	}

	// Token: 0x06006F01 RID: 28417 RVA: 0x002FF8BC File Offset: 0x002FDABC
	public static void OverloadStrings(Dictionary<string, string> translated_strings, string path, Type locstring_hierarchy, ref string parameter_errors, ref string link_errors, ref string link_count_errors)
	{
		foreach (FieldInfo fieldInfo in locstring_hierarchy.GetFields())
		{
			if (!(fieldInfo.FieldType != typeof(LocString)))
			{
				string text = path + "." + fieldInfo.Name;
				string text2 = null;
				if (translated_strings.TryGetValue(text, out text2))
				{
					LocString locString = (LocString)fieldInfo.GetValue(null);
					LocString value = new LocString(text2, text);
					if (!Localization.AreParametersPreserved(locString.text, text2))
					{
						parameter_errors = parameter_errors + "\t" + text + "\n";
					}
					else if (!Localization.HasSameOrLessLinkCountAsEnglish(locString.text, text2))
					{
						link_count_errors = link_count_errors + "\t" + text + "\n";
					}
					else if (!Localization.HasMatchingLinkTags(text2, 0))
					{
						link_errors = link_errors + "\t" + text + "\n";
					}
					else
					{
						fieldInfo.SetValue(null, value);
					}
				}
			}
		}
		foreach (Type type in locstring_hierarchy.GetNestedTypes())
		{
			string path2 = path + "." + type.Name;
			Localization.OverloadStrings(translated_strings, path2, type, ref parameter_errors, ref link_errors, ref link_count_errors);
		}
	}

	// Token: 0x06006F02 RID: 28418 RVA: 0x000ED2F8 File Offset: 0x000EB4F8
	public static string GetDefaultLocalizationFilePath()
	{
		return Path.Combine(Application.streamingAssetsPath, "strings/strings_template.pot");
	}

	// Token: 0x06006F03 RID: 28419 RVA: 0x000ED309 File Offset: 0x000EB509
	public static string GetModLocalizationFilePath()
	{
		return Path.Combine(Application.streamingAssetsPath, "strings/strings.po");
	}

	// Token: 0x06006F04 RID: 28420 RVA: 0x002FF9F8 File Offset: 0x002FDBF8
	public static string GetPreinstalledLocalizationFilePath(string code)
	{
		string path = "strings/strings_preinstalled_" + code + ".po";
		return Path.Combine(Application.streamingAssetsPath, path);
	}

	// Token: 0x06006F05 RID: 28421 RVA: 0x000ED31A File Offset: 0x000EB51A
	public static string GetPreinstalledLocalizationTitle(string code)
	{
		return Strings.Get("STRINGS.UI.FRONTEND.TRANSLATIONS_SCREEN.PREINSTALLED_LANGUAGES." + code.ToUpper());
	}

	// Token: 0x06006F06 RID: 28422 RVA: 0x002FFA24 File Offset: 0x002FDC24
	public static Texture2D GetPreinstalledLocalizationImage(string code)
	{
		string path = Path.Combine(Application.streamingAssetsPath, "strings/preinstalled_icon_" + code + ".png");
		if (File.Exists(path))
		{
			byte[] data = File.ReadAllBytes(path);
			Texture2D texture2D = new Texture2D(2, 2);
			texture2D.LoadImage(data);
			return texture2D;
		}
		return null;
	}

	// Token: 0x06006F07 RID: 28423 RVA: 0x000ED336 File Offset: 0x000EB536
	public static void SetLocale(Localization.Locale locale)
	{
		Localization.sLocale = locale;
		DebugUtil.LogArgs(new object[]
		{
			" -> Locale is now ",
			Localization.sLocale.ToString()
		});
	}

	// Token: 0x06006F08 RID: 28424 RVA: 0x000ED35E File Offset: 0x000EB55E
	public static Localization.Locale GetLocale()
	{
		return Localization.sLocale;
	}

	// Token: 0x06006F09 RID: 28425 RVA: 0x002FFA6C File Offset: 0x002FDC6C
	private static string GetFontParam(string line)
	{
		string text = null;
		if (line.StartsWith("\"Font:"))
		{
			text = line.Substring("\"Font:".Length).Trim();
			text = text.Replace("\\n", "");
			text = text.Replace("\"", "");
		}
		return text;
	}

	// Token: 0x06006F0A RID: 28426 RVA: 0x002FFAC4 File Offset: 0x002FDCC4
	public static string GetCurrentLanguageCode()
	{
		switch (Localization.GetSelectedLanguageType())
		{
		case Localization.SelectedLanguageType.None:
			return Localization.DEFAULT_LANGUAGE_CODE;
		case Localization.SelectedLanguageType.Preinstalled:
			return KPlayerPrefs.GetString(Localization.SELECTED_LANGUAGE_CODE_KEY);
		case Localization.SelectedLanguageType.UGC:
			return LanguageOptionsScreen.GetInstalledLanguageCode();
		default:
			return "";
		}
	}

	// Token: 0x06006F0B RID: 28427 RVA: 0x002FFB08 File Offset: 0x002FDD08
	public static Localization.SelectedLanguageType GetSelectedLanguageType()
	{
		return (Localization.SelectedLanguageType)Enum.Parse(typeof(Localization.SelectedLanguageType), KPlayerPrefs.GetString(Localization.SELECTED_LANGUAGE_TYPE_KEY, Localization.SelectedLanguageType.None.ToString()), true);
	}

	// Token: 0x06006F0C RID: 28428 RVA: 0x002FFB44 File Offset: 0x002FDD44
	private static string GetLanguageCode(string line)
	{
		string text = null;
		if (line.StartsWith("\"Language:"))
		{
			text = line.Substring("\"Language:".Length).Trim();
			text = text.Replace("\\n", "");
			text = text.Replace("\"", "");
		}
		return text;
	}

	// Token: 0x06006F0D RID: 28429 RVA: 0x002FFB9C File Offset: 0x002FDD9C
	private static Localization.Locale GetLocaleForCode(string code)
	{
		Localization.Locale result = null;
		foreach (Localization.Locale locale in Localization.Locales)
		{
			if (locale.MatchesCode(code))
			{
				result = locale;
				break;
			}
		}
		return result;
	}

	// Token: 0x06006F0E RID: 28430 RVA: 0x002FFBF8 File Offset: 0x002FDDF8
	public static Localization.Locale GetLocale(string[] lines)
	{
		Localization.Locale locale = null;
		string text = null;
		if (lines != null && lines.Length != 0)
		{
			foreach (string text2 in lines)
			{
				if (text2 != null && text2.Length != 0)
				{
					text = Localization.GetLanguageCode(text2);
					if (text != null)
					{
						locale = Localization.GetLocaleForCode(text);
					}
					if (text != null)
					{
						break;
					}
				}
			}
		}
		if (locale == null)
		{
			locale = Localization.GetDefaultLocale();
		}
		if (text != null && locale.Code == "")
		{
			locale.SetCode(text);
		}
		return locale;
	}

	// Token: 0x06006F0F RID: 28431 RVA: 0x000ED365 File Offset: 0x000EB565
	private static string GetFontName(string filename)
	{
		return Localization.GetFontName(File.ReadAllLines(filename, Encoding.UTF8));
	}

	// Token: 0x06006F10 RID: 28432 RVA: 0x002FFC70 File Offset: 0x002FDE70
	public static Localization.Locale GetDefaultLocale()
	{
		Localization.Locale result = null;
		foreach (Localization.Locale locale in Localization.Locales)
		{
			if (locale.Lang == Localization.Language.Unspecified)
			{
				result = new Localization.Locale(locale);
				break;
			}
		}
		return result;
	}

	// Token: 0x06006F11 RID: 28433 RVA: 0x002FFCD0 File Offset: 0x002FDED0
	public static string GetDefaultFontName()
	{
		string result = null;
		foreach (Localization.Locale locale in Localization.Locales)
		{
			if (locale.Lang == Localization.Language.Unspecified)
			{
				result = locale.FontName;
				break;
			}
		}
		return result;
	}

	// Token: 0x06006F12 RID: 28434 RVA: 0x002FFD30 File Offset: 0x002FDF30
	public static string ValidateFontName(string fontName)
	{
		foreach (Localization.Locale locale in Localization.Locales)
		{
			if (locale.MatchesFont(fontName))
			{
				return locale.FontName;
			}
		}
		return null;
	}

	// Token: 0x06006F13 RID: 28435 RVA: 0x002FFD90 File Offset: 0x002FDF90
	public static string GetFontName(string[] lines)
	{
		string text = null;
		if (lines != null)
		{
			foreach (string text2 in lines)
			{
				if (!string.IsNullOrEmpty(text2))
				{
					string fontParam = Localization.GetFontParam(text2);
					if (fontParam != null)
					{
						text = Localization.ValidateFontName(fontParam);
					}
				}
				if (text != null)
				{
					break;
				}
			}
		}
		if (text == null)
		{
			if (Localization.sLocale != null)
			{
				text = Localization.sLocale.FontName;
			}
			else
			{
				text = Localization.GetDefaultFontName();
			}
		}
		return text;
	}

	// Token: 0x06006F14 RID: 28436 RVA: 0x000ED377 File Offset: 0x000EB577
	public static void SwapToLocalizedFont()
	{
		Localization.SwapToLocalizedFont(Localization.currentFontName);
	}

	// Token: 0x06006F15 RID: 28437 RVA: 0x002FFDF4 File Offset: 0x002FDFF4
	public static bool SwapToLocalizedFont(string fontname)
	{
		if (string.IsNullOrEmpty(fontname))
		{
			return false;
		}
		Localization.sFontAsset = Localization.GetFont(fontname);
		foreach (TextStyleSetting textStyleSetting in Resources.FindObjectsOfTypeAll<TextStyleSetting>())
		{
			if (textStyleSetting != null)
			{
				textStyleSetting.sdfFont = Localization.sFontAsset;
			}
		}
		bool isRightToLeft = Localization.IsRightToLeft;
		foreach (LocText locText in Resources.FindObjectsOfTypeAll<LocText>())
		{
			if (locText != null)
			{
				locText.SwapFont(Localization.sFontAsset, isRightToLeft);
			}
		}
		return true;
	}

	// Token: 0x06006F16 RID: 28438 RVA: 0x002FFE7C File Offset: 0x002FE07C
	private static bool SetFont(Type target_type, object target, TMP_FontAsset font, bool is_right_to_left, HashSet<MemberInfo> excluded_members)
	{
		if (target_type == null || target == null || font == null)
		{
			return false;
		}
		foreach (FieldInfo fieldInfo in target_type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy))
		{
			if (!excluded_members.Contains(fieldInfo))
			{
				if (fieldInfo.FieldType == typeof(TextStyleSetting))
				{
					((TextStyleSetting)fieldInfo.GetValue(target)).sdfFont = font;
				}
				else if (fieldInfo.FieldType == typeof(LocText))
				{
					((LocText)fieldInfo.GetValue(target)).SwapFont(font, is_right_to_left);
				}
				else if (fieldInfo.FieldType == typeof(GameObject))
				{
					foreach (Component component in ((GameObject)fieldInfo.GetValue(target)).GetComponents<Component>())
					{
						Localization.SetFont(component.GetType(), component, font, is_right_to_left, excluded_members);
					}
				}
				else if (fieldInfo.MemberType == MemberTypes.Field && fieldInfo.FieldType != fieldInfo.DeclaringType)
				{
					Localization.SetFont(fieldInfo.FieldType, fieldInfo.GetValue(target), font, is_right_to_left, excluded_members);
				}
			}
		}
		return true;
	}

	// Token: 0x06006F17 RID: 28439 RVA: 0x000ED384 File Offset: 0x000EB584
	public static bool SetFont<T>(T target, TMP_FontAsset font, bool is_right_to_left, HashSet<MemberInfo> excluded_members)
	{
		return Localization.SetFont(typeof(T), target, font, is_right_to_left, excluded_members);
	}

	// Token: 0x06006F18 RID: 28440 RVA: 0x002FFFB8 File Offset: 0x002FE1B8
	public static TMP_FontAsset GetFont(string fontname)
	{
		foreach (TMP_FontAsset tmp_FontAsset in Resources.FindObjectsOfTypeAll<TMP_FontAsset>())
		{
			if (tmp_FontAsset.name == fontname)
			{
				return tmp_FontAsset;
			}
		}
		return null;
	}

	// Token: 0x06006F19 RID: 28441 RVA: 0x002FFFF0 File Offset: 0x002FE1F0
	private static bool HasSameOrLessTokenCount(string english_string, string translated_string, string token)
	{
		int num = english_string.Split(new string[]
		{
			token
		}, StringSplitOptions.None).Length;
		int num2 = translated_string.Split(new string[]
		{
			token
		}, StringSplitOptions.None).Length;
		return num >= num2;
	}

	// Token: 0x06006F1A RID: 28442 RVA: 0x000ED39E File Offset: 0x000EB59E
	private static bool HasSameOrLessLinkCountAsEnglish(string english_string, string translated_string)
	{
		return Localization.HasSameOrLessTokenCount(english_string, translated_string, "<link") && Localization.HasSameOrLessTokenCount(english_string, translated_string, "</link");
	}

	// Token: 0x06006F1B RID: 28443 RVA: 0x0030002C File Offset: 0x002FE22C
	private static bool HasMatchingLinkTags(string str, int idx = 0)
	{
		int num = str.IndexOf("<link", idx);
		int num2 = str.IndexOf("</link", idx);
		if (num == -1 && num2 == -1)
		{
			return true;
		}
		if (num == -1 && num2 != -1)
		{
			return false;
		}
		if (num != -1 && num2 == -1)
		{
			return false;
		}
		if (num2 < num)
		{
			return false;
		}
		int num3 = str.IndexOf("<link", num + 1);
		return (num < 0 || num3 == -1 || num3 >= num2) && Localization.HasMatchingLinkTags(str, num2 + 1);
	}

	// Token: 0x06006F1C RID: 28444 RVA: 0x003000A0 File Offset: 0x002FE2A0
	private static bool AreParametersPreserved(string old_string, string new_string)
	{
		MatchCollection matchCollection = Regex.Matches(old_string, "({.[^}]*?})(?!.*\\1)");
		MatchCollection matchCollection2 = Regex.Matches(new_string, "({.[^}]*?})(?!.*\\1)");
		bool result = false;
		if (matchCollection == null && matchCollection2 == null)
		{
			result = true;
		}
		else if (matchCollection != null && matchCollection2 != null && matchCollection.Count == matchCollection2.Count)
		{
			result = true;
			foreach (object obj in matchCollection)
			{
				string a = obj.ToString();
				bool flag = false;
				foreach (object obj2 in matchCollection2)
				{
					string b = obj2.ToString();
					if (a == b)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					result = false;
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x06006F1D RID: 28445 RVA: 0x000ED3BC File Offset: 0x000EB5BC
	public static bool HasDirtyWords(string str)
	{
		return Localization.FilterDirtyWords(str) != str;
	}

	// Token: 0x06006F1E RID: 28446 RVA: 0x000ED3CA File Offset: 0x000EB5CA
	public static string FilterDirtyWords(string str)
	{
		return DistributionPlatform.Inst.ApplyWordFilter(str);
	}

	// Token: 0x06006F1F RID: 28447 RVA: 0x000ED3D7 File Offset: 0x000EB5D7
	public static string GetFileDateFormat(int format_idx)
	{
		return "{" + format_idx.ToString() + ":dd / MMM / yyyy}";
	}

	// Token: 0x06006F20 RID: 28448 RVA: 0x00300198 File Offset: 0x002FE398
	public static void ClearLanguage()
	{
		DebugUtil.LogArgs(new object[]
		{
			" -> Clearing selected language! Either it didn't load correct or returning to english by menu."
		});
		Localization.sFontAsset = null;
		Localization.sLocale = null;
		KPlayerPrefs.SetString(Localization.SELECTED_LANGUAGE_TYPE_KEY, Localization.SelectedLanguageType.None.ToString());
		KPlayerPrefs.SetString(Localization.SELECTED_LANGUAGE_CODE_KEY, "");
		Localization.SwapToLocalizedFont(Localization.GetDefaultLocale().FontName);
		string defaultLocalizationFilePath = Localization.GetDefaultLocalizationFilePath();
		if (File.Exists(defaultLocalizationFilePath))
		{
			Localization.LoadTranslation(File.ReadAllLines(defaultLocalizationFilePath, Encoding.UTF8), true);
		}
		LanguageOptionsScreen.CleanUpSavedLanguageMod();
	}

	// Token: 0x06006F21 RID: 28449 RVA: 0x00300224 File Offset: 0x002FE424
	private static string ReverseText(string source)
	{
		char[] separator = new char[]
		{
			'\n'
		};
		string[] array = source.Split(separator);
		string text = "";
		int num = 0;
		foreach (string text2 in array)
		{
			num++;
			char[] array3 = new char[text2.Length];
			for (int j = 0; j < text2.Length; j++)
			{
				array3[array3.Length - 1 - j] = text2[j];
			}
			text += new string(array3);
			if (num < array.Length)
			{
				text += "\n";
			}
		}
		return text;
	}

	// Token: 0x06006F22 RID: 28450 RVA: 0x000ED3EF File Offset: 0x000EB5EF
	public static string Fixup(string text)
	{
		if (Localization.sLocale != null && text != null && text != "" && Localization.sLocale.Lang == Localization.Language.Arabic)
		{
			return Localization.ReverseText(ArabicFixer.Fix(text));
		}
		return text;
	}

	// Token: 0x04005378 RID: 21368
	private static TMP_FontAsset sFontAsset = null;

	// Token: 0x04005379 RID: 21369
	private static readonly List<Localization.Locale> Locales = new List<Localization.Locale>
	{
		new Localization.Locale(Localization.Language.Chinese, Localization.Direction.LeftToRight, "zh", "NotoSansCJKsc-Regular"),
		new Localization.Locale(Localization.Language.Japanese, Localization.Direction.LeftToRight, "ja", "NotoSansCJKjp-Regular"),
		new Localization.Locale(Localization.Language.Korean, Localization.Direction.LeftToRight, "ko", "NotoSansCJKkr-Regular"),
		new Localization.Locale(Localization.Language.Russian, Localization.Direction.LeftToRight, "ru", "RobotoCondensed-Regular"),
		new Localization.Locale(Localization.Language.Thai, Localization.Direction.LeftToRight, "th", "NotoSansThai-Regular"),
		new Localization.Locale(Localization.Language.Arabic, Localization.Direction.RightToLeft, "ar", "NotoNaskhArabic-Regular"),
		new Localization.Locale(Localization.Language.Hebrew, Localization.Direction.RightToLeft, "he", "NotoSansHebrew-Regular"),
		new Localization.Locale(Localization.Language.Unspecified, Localization.Direction.LeftToRight, "", "RobotoCondensed-Regular")
	};

	// Token: 0x0400537A RID: 21370
	private static Localization.Locale sLocale = null;

	// Token: 0x0400537B RID: 21371
	private static string currentFontName = null;

	// Token: 0x0400537C RID: 21372
	public static string DEFAULT_LANGUAGE_CODE = "en";

	// Token: 0x0400537D RID: 21373
	public static readonly List<string> PreinstalledLanguages = new List<string>
	{
		Localization.DEFAULT_LANGUAGE_CODE,
		"zh_klei",
		"ko_klei",
		"ru_klei"
	};

	// Token: 0x0400537E RID: 21374
	public static string SELECTED_LANGUAGE_TYPE_KEY = "SelectedLanguageType";

	// Token: 0x0400537F RID: 21375
	public static string SELECTED_LANGUAGE_CODE_KEY = "SelectedLanguageCode";

	// Token: 0x04005380 RID: 21376
	private static Dictionary<string, List<Assembly>> translatable_assemblies = new Dictionary<string, List<Assembly>>();

	// Token: 0x04005381 RID: 21377
	public const BindingFlags non_static_data_member_fields = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;

	// Token: 0x04005382 RID: 21378
	private const string start_link_token = "<link";

	// Token: 0x04005383 RID: 21379
	private const string end_link_token = "</link";

	// Token: 0x020014DE RID: 5342
	public enum Language
	{
		// Token: 0x04005385 RID: 21381
		Chinese,
		// Token: 0x04005386 RID: 21382
		Japanese,
		// Token: 0x04005387 RID: 21383
		Korean,
		// Token: 0x04005388 RID: 21384
		Russian,
		// Token: 0x04005389 RID: 21385
		Thai,
		// Token: 0x0400538A RID: 21386
		Arabic,
		// Token: 0x0400538B RID: 21387
		Hebrew,
		// Token: 0x0400538C RID: 21388
		Unspecified
	}

	// Token: 0x020014DF RID: 5343
	public enum Direction
	{
		// Token: 0x0400538E RID: 21390
		LeftToRight,
		// Token: 0x0400538F RID: 21391
		RightToLeft
	}

	// Token: 0x020014E0 RID: 5344
	public class Locale
	{
		// Token: 0x06006F24 RID: 28452 RVA: 0x000ED422 File Offset: 0x000EB622
		public Locale(Localization.Locale other)
		{
			this.mLanguage = other.mLanguage;
			this.mDirection = other.mDirection;
			this.mCode = other.mCode;
			this.mFontName = other.mFontName;
		}

		// Token: 0x06006F25 RID: 28453 RVA: 0x000ED45A File Offset: 0x000EB65A
		public Locale(Localization.Language language, Localization.Direction direction, string code, string fontName)
		{
			this.mLanguage = language;
			this.mDirection = direction;
			this.mCode = code.ToLower();
			this.mFontName = fontName;
		}

		// Token: 0x17000716 RID: 1814
		// (get) Token: 0x06006F26 RID: 28454 RVA: 0x000ED484 File Offset: 0x000EB684
		public Localization.Language Lang
		{
			get
			{
				return this.mLanguage;
			}
		}

		// Token: 0x06006F27 RID: 28455 RVA: 0x000ED48C File Offset: 0x000EB68C
		public void SetCode(string code)
		{
			this.mCode = code;
		}

		// Token: 0x17000717 RID: 1815
		// (get) Token: 0x06006F28 RID: 28456 RVA: 0x000ED495 File Offset: 0x000EB695
		public string Code
		{
			get
			{
				return this.mCode;
			}
		}

		// Token: 0x17000718 RID: 1816
		// (get) Token: 0x06006F29 RID: 28457 RVA: 0x000ED49D File Offset: 0x000EB69D
		public string FontName
		{
			get
			{
				return this.mFontName;
			}
		}

		// Token: 0x17000719 RID: 1817
		// (get) Token: 0x06006F2A RID: 28458 RVA: 0x000ED4A5 File Offset: 0x000EB6A5
		public bool IsRightToLeft
		{
			get
			{
				return this.mDirection == Localization.Direction.RightToLeft;
			}
		}

		// Token: 0x06006F2B RID: 28459 RVA: 0x000ED4B0 File Offset: 0x000EB6B0
		public bool MatchesCode(string language_code)
		{
			return language_code.ToLower().Contains(this.mCode);
		}

		// Token: 0x06006F2C RID: 28460 RVA: 0x000ED4C3 File Offset: 0x000EB6C3
		public bool MatchesFont(string fontname)
		{
			return fontname.ToLower() == this.mFontName.ToLower();
		}

		// Token: 0x06006F2D RID: 28461 RVA: 0x00300408 File Offset: 0x002FE608
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				this.mCode,
				":",
				this.mLanguage.ToString(),
				":",
				this.mDirection.ToString(),
				":",
				this.mFontName
			});
		}

		// Token: 0x04005390 RID: 21392
		private Localization.Language mLanguage;

		// Token: 0x04005391 RID: 21393
		private string mCode;

		// Token: 0x04005392 RID: 21394
		private string mFontName;

		// Token: 0x04005393 RID: 21395
		private Localization.Direction mDirection;
	}

	// Token: 0x020014E1 RID: 5345
	private struct Entry
	{
		// Token: 0x1700071A RID: 1818
		// (get) Token: 0x06006F2E RID: 28462 RVA: 0x000ED4DB File Offset: 0x000EB6DB
		public bool IsPopulated
		{
			get
			{
				return this.msgctxt != null && this.msgstr != null && this.msgstr.Length > 0;
			}
		}

		// Token: 0x04005394 RID: 21396
		public string msgctxt;

		// Token: 0x04005395 RID: 21397
		public string msgstr;
	}

	// Token: 0x020014E2 RID: 5346
	public enum SelectedLanguageType
	{
		// Token: 0x04005397 RID: 21399
		None,
		// Token: 0x04005398 RID: 21400
		Preinstalled,
		// Token: 0x04005399 RID: 21401
		UGC
	}
}
