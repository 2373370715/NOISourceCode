using System;
using System.Collections.Generic;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using STRINGS;
using TMPro;
using UnityEngine;

// Token: 0x020013CA RID: 5066
public class GlobalAssets : KMonoBehaviour
{
	// Token: 0x17000676 RID: 1654
	// (get) Token: 0x06006801 RID: 26625 RVA: 0x000E874A File Offset: 0x000E694A
	// (set) Token: 0x06006802 RID: 26626 RVA: 0x000E8751 File Offset: 0x000E6951
	public static GlobalAssets Instance { get; private set; }

	// Token: 0x06006803 RID: 26627 RVA: 0x002E5270 File Offset: 0x002E3470
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		GlobalAssets.Instance = this;
		if (GlobalAssets.SoundTable.Count == 0)
		{
			Bank[] array = null;
			try
			{
				if (RuntimeManager.StudioSystem.getBankList(out array) != RESULT.OK)
				{
					array = null;
				}
			}
			catch
			{
				array = null;
			}
			if (array != null)
			{
				foreach (Bank bank in array)
				{
					EventDescription[] array3;
					RESULT eventList = bank.getEventList(out array3);
					if (eventList != RESULT.OK)
					{
						string text;
						bank.getPath(out text);
						global::Debug.LogError(string.Format("ERROR [{0}] loading FMOD events for bank [{1}]", eventList, text));
					}
					else
					{
						foreach (EventDescription eventDescription in array3)
						{
							string text;
							eventDescription.getPath(out text);
							if (text == null)
							{
								bank.getPath(out text);
								GUID guid;
								eventDescription.getID(out guid);
								global::Debug.LogError(string.Format("Got a FMOD event with a null path! {0} {1} in bank {2}", eventDescription.ToString(), guid, text));
							}
							else
							{
								string text2 = Assets.GetSimpleSoundEventName(text);
								text2 = text2.ToLowerInvariant();
								if (text2.Length > 0 && !GlobalAssets.SoundTable.ContainsKey(text2))
								{
									GlobalAssets.SoundTable[text2] = text;
									if (text.ToLower().Contains("lowpriority") || text2.Contains("lowpriority"))
									{
										GlobalAssets.LowPrioritySounds.Add(text);
									}
									else if (text.ToLower().Contains("highpriority") || text2.Contains("highpriority"))
									{
										GlobalAssets.HighPrioritySounds.Add(text);
									}
								}
							}
						}
					}
				}
			}
		}
		SetDefaults.Initialize();
		GraphicsOptionsScreen.SetColorModeFromPrefs();
		this.AddColorModeStyles();
		LocString.CreateLocStringKeys(typeof(UI), "STRINGS.");
		LocString.CreateLocStringKeys(typeof(INPUT), "STRINGS.");
		LocString.CreateLocStringKeys(typeof(GAMEPLAY_EVENTS), "STRINGS.");
		LocString.CreateLocStringKeys(typeof(ROOMS), "STRINGS.");
		LocString.CreateLocStringKeys(typeof(BUILDING.STATUSITEMS), "STRINGS.BUILDING.");
		LocString.CreateLocStringKeys(typeof(BUILDING.DETAILS), "STRINGS.BUILDING.");
		LocString.CreateLocStringKeys(typeof(SETITEMS), "STRINGS.");
		LocString.CreateLocStringKeys(typeof(COLONY_ACHIEVEMENTS), "STRINGS.");
		LocString.CreateLocStringKeys(typeof(CREATURES), "STRINGS.");
		LocString.CreateLocStringKeys(typeof(RESEARCH), "STRINGS.");
		LocString.CreateLocStringKeys(typeof(DUPLICANTS), "STRINGS.");
		LocString.CreateLocStringKeys(typeof(ITEMS), "STRINGS.");
		LocString.CreateLocStringKeys(typeof(ROBOTS), "STRINGS.");
		LocString.CreateLocStringKeys(typeof(ELEMENTS), "STRINGS.");
		LocString.CreateLocStringKeys(typeof(MISC), "STRINGS.");
		LocString.CreateLocStringKeys(typeof(VIDEOS), "STRINGS.");
		LocString.CreateLocStringKeys(typeof(NAMEGEN), "STRINGS.");
		LocString.CreateLocStringKeys(typeof(WORLDS), "STRINGS.");
		LocString.CreateLocStringKeys(typeof(CLUSTER_NAMES), "STRINGS.");
		LocString.CreateLocStringKeys(typeof(SUBWORLDS), "STRINGS.");
		LocString.CreateLocStringKeys(typeof(WORLD_TRAITS), "STRINGS.");
		LocString.CreateLocStringKeys(typeof(INPUT_BINDINGS), "STRINGS.");
		LocString.CreateLocStringKeys(typeof(LORE), "STRINGS.");
		LocString.CreateLocStringKeys(typeof(CODEX), "STRINGS.");
		LocString.CreateLocStringKeys(typeof(SUBWORLDS), "STRINGS.");
		LocString.CreateLocStringKeys(typeof(BLUEPRINTS), "STRINGS.");
	}

	// Token: 0x06006804 RID: 26628 RVA: 0x002E5640 File Offset: 0x002E3840
	private void AddColorModeStyles()
	{
		TMP_Style style = new TMP_Style("logic_on", string.Format("<color=#{0}>", ColorUtility.ToHtmlStringRGB(this.colorSet.logicOn)), "</color>");
		TMP_StyleSheet.instance.AddStyle(style);
		TMP_Style style2 = new TMP_Style("logic_off", string.Format("<color=#{0}>", ColorUtility.ToHtmlStringRGB(this.colorSet.logicOff)), "</color>");
		TMP_StyleSheet.instance.AddStyle(style2);
		TMP_StyleSheet.RefreshStyles();
	}

	// Token: 0x06006805 RID: 26629 RVA: 0x000E8759 File Offset: 0x000E6959
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		GlobalAssets.Instance = null;
	}

	// Token: 0x06006806 RID: 26630 RVA: 0x002E56C8 File Offset: 0x002E38C8
	public static string GetSound(string name, bool force_no_warning = false)
	{
		if (name == null)
		{
			return null;
		}
		name = name.ToLowerInvariant();
		string result = null;
		GlobalAssets.SoundTable.TryGetValue(name, out result);
		return result;
	}

	// Token: 0x06006807 RID: 26631 RVA: 0x000E8767 File Offset: 0x000E6967
	public static bool IsLowPriority(string path)
	{
		return GlobalAssets.LowPrioritySounds.Contains(path);
	}

	// Token: 0x06006808 RID: 26632 RVA: 0x000E8774 File Offset: 0x000E6974
	public static bool IsHighPriority(string path)
	{
		return GlobalAssets.HighPrioritySounds.Contains(path);
	}

	// Token: 0x04004E8F RID: 20111
	private static Dictionary<string, string> SoundTable = new Dictionary<string, string>();

	// Token: 0x04004E90 RID: 20112
	private static HashSet<string> LowPrioritySounds = new HashSet<string>();

	// Token: 0x04004E91 RID: 20113
	private static HashSet<string> HighPrioritySounds = new HashSet<string>();

	// Token: 0x04004E93 RID: 20115
	public ColorSet colorSet;

	// Token: 0x04004E94 RID: 20116
	public ColorSet[] colorSetOptions;
}
