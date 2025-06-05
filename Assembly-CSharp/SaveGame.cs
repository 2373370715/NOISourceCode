using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Klei.CustomSettings;
using KSerialization;
using Newtonsoft.Json;
using ProcGen;
using STRINGS;
using UnityEngine;

// Token: 0x02001860 RID: 6240
[SerializationConfig(KSerialization.MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/SaveGame")]
public class SaveGame : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x1700082E RID: 2094
	// (get) Token: 0x0600809B RID: 32923 RVA: 0x000F9367 File Offset: 0x000F7567
	// (set) Token: 0x0600809C RID: 32924 RVA: 0x000F936F File Offset: 0x000F756F
	public int AutoSaveCycleInterval
	{
		get
		{
			return this.autoSaveCycleInterval;
		}
		set
		{
			this.autoSaveCycleInterval = value;
		}
	}

	// Token: 0x1700082F RID: 2095
	// (get) Token: 0x0600809D RID: 32925 RVA: 0x000F9378 File Offset: 0x000F7578
	// (set) Token: 0x0600809E RID: 32926 RVA: 0x000F9380 File Offset: 0x000F7580
	public Vector2I TimelapseResolution
	{
		get
		{
			return this.timelapseResolution;
		}
		set
		{
			this.timelapseResolution = value;
		}
	}

	// Token: 0x17000830 RID: 2096
	// (get) Token: 0x0600809F RID: 32927 RVA: 0x000F9389 File Offset: 0x000F7589
	public string BaseName
	{
		get
		{
			return this.baseName;
		}
	}

	// Token: 0x060080A0 RID: 32928 RVA: 0x000F9391 File Offset: 0x000F7591
	public static void DestroyInstance()
	{
		SaveGame.Instance = null;
	}

	// Token: 0x17000831 RID: 2097
	// (get) Token: 0x060080A1 RID: 32929 RVA: 0x000F9399 File Offset: 0x000F7599
	public ColonyAchievementTracker ColonyAchievementTracker
	{
		get
		{
			if (this.colonyAchievementTracker == null)
			{
				this.colonyAchievementTracker = base.GetComponent<ColonyAchievementTracker>();
			}
			return this.colonyAchievementTracker;
		}
	}

	// Token: 0x060080A2 RID: 32930 RVA: 0x0034178C File Offset: 0x0033F98C
	protected override void OnPrefabInit()
	{
		SaveGame.Instance = this;
		new ColonyRationMonitor.Instance(this).StartSM();
		this.entombedItemManager = base.gameObject.AddComponent<EntombedItemManager>();
		this.worldGenSpawner = base.gameObject.AddComponent<WorldGenSpawner>();
		base.gameObject.AddOrGetDef<GameplaySeasonManager.Def>();
		base.gameObject.AddOrGetDef<ClusterFogOfWarManager.Def>();
	}

	// Token: 0x060080A3 RID: 32931 RVA: 0x000F93BB File Offset: 0x000F75BB
	[OnSerializing]
	private void OnSerialize()
	{
		this.speed = SpeedControlScreen.Instance.GetSpeed();
	}

	// Token: 0x060080A4 RID: 32932 RVA: 0x000F93CD File Offset: 0x000F75CD
	[OnDeserializing]
	private void OnDeserialize()
	{
		this.baseName = SaveLoader.Instance.GameInfo.baseName;
	}

	// Token: 0x060080A5 RID: 32933 RVA: 0x000F93E4 File Offset: 0x000F75E4
	public int GetSpeed()
	{
		return this.speed;
	}

	// Token: 0x060080A6 RID: 32934 RVA: 0x003417E4 File Offset: 0x0033F9E4
	public byte[] GetSaveHeader(bool isAutoSave, bool isCompressed, out SaveGame.Header header)
	{
		string originalSaveFileName = SaveLoader.GetOriginalSaveFileName(SaveLoader.GetActiveSaveFilePath());
		string s = JsonConvert.SerializeObject(new SaveGame.GameInfo(GameClock.Instance.GetCycle(), Components.LiveMinionIdentities.Count, this.baseName, isAutoSave, originalSaveFileName, SaveLoader.Instance.GameInfo.clusterId, SaveLoader.Instance.GameInfo.worldTraits, SaveLoader.Instance.GameInfo.colonyGuid, SaveLoader.Instance.GameInfo.dlcIds, this.sandboxEnabled));
		byte[] bytes = Encoding.UTF8.GetBytes(s);
		header = default(SaveGame.Header);
		header.buildVersion = 663500U;
		header.headerSize = bytes.Length;
		header.headerVersion = 1U;
		header.compression = (isCompressed ? 1 : 0);
		return bytes;
	}

	// Token: 0x060080A7 RID: 32935 RVA: 0x000F93EC File Offset: 0x000F75EC
	public static string GetSaveUniqueID(SaveGame.GameInfo info)
	{
		if (!(info.colonyGuid != Guid.Empty))
		{
			return info.baseName + "/" + info.clusterId;
		}
		return info.colonyGuid.ToString();
	}

	// Token: 0x060080A8 RID: 32936 RVA: 0x003418A8 File Offset: 0x0033FAA8
	public static global::Tuple<SaveGame.Header, SaveGame.GameInfo> GetFileInfo(string filename)
	{
		try
		{
			SaveGame.Header a;
			SaveGame.GameInfo gameInfo = SaveLoader.LoadHeader(filename, out a);
			if (gameInfo.saveMajorVersion >= 7)
			{
				return new global::Tuple<SaveGame.Header, SaveGame.GameInfo>(a, gameInfo);
			}
		}
		catch (Exception obj)
		{
			global::Debug.LogWarning("Exception while loading " + filename);
			global::Debug.LogWarning(obj);
		}
		return null;
	}

	// Token: 0x060080A9 RID: 32937 RVA: 0x00341900 File Offset: 0x0033FB00
	public static SaveGame.GameInfo GetHeader(IReader br, out SaveGame.Header header, string debugFileName)
	{
		header = default(SaveGame.Header);
		header.buildVersion = br.ReadUInt32();
		header.headerSize = br.ReadInt32();
		header.headerVersion = br.ReadUInt32();
		if (1U <= header.headerVersion)
		{
			header.compression = br.ReadInt32();
		}
		byte[] data = br.ReadBytes(header.headerSize);
		if (header.headerSize == 0 && !SaveGame.debug_SaveFileHeaderBlank_sent)
		{
			SaveGame.debug_SaveFileHeaderBlank_sent = true;
			global::Debug.LogWarning("SaveFileHeaderBlank - " + debugFileName);
		}
		SaveGame.GameInfo gameInfo = SaveGame.GetGameInfo(data);
		if (gameInfo.IsVersionOlderThan(7, 14) && gameInfo.worldTraits != null)
		{
			string[] worldTraits = gameInfo.worldTraits;
			for (int i = 0; i < worldTraits.Length; i++)
			{
				worldTraits[i] = worldTraits[i].Replace('\\', '/');
			}
		}
		if (gameInfo.IsVersionOlderThan(7, 20))
		{
			gameInfo.dlcId = "";
		}
		if (gameInfo.IsVersionOlderThan(7, 34))
		{
			gameInfo.dlcIds = new List<string>
			{
				gameInfo.dlcId
			};
		}
		return gameInfo;
	}

	// Token: 0x060080AA RID: 32938 RVA: 0x000F9429 File Offset: 0x000F7629
	public static SaveGame.GameInfo GetGameInfo(byte[] data)
	{
		return JsonConvert.DeserializeObject<SaveGame.GameInfo>(Encoding.UTF8.GetString(data));
	}

	// Token: 0x060080AB RID: 32939 RVA: 0x000F943B File Offset: 0x000F763B
	public void SetBaseName(string newBaseName)
	{
		if (string.IsNullOrEmpty(newBaseName))
		{
			global::Debug.LogWarning("Cannot give the base an empty name");
			return;
		}
		this.baseName = newBaseName;
	}

	// Token: 0x060080AC RID: 32940 RVA: 0x000F9457 File Offset: 0x000F7657
	protected override void OnSpawn()
	{
		ThreadedHttps<KleiMetrics>.Instance.SendProfileStats();
		Game.Instance.Trigger(-1917495436, null);
	}

	// Token: 0x060080AD RID: 32941 RVA: 0x003419FC File Offset: 0x0033FBFC
	public List<global::Tuple<string, TextStyleSetting>> GetColonyToolTip()
	{
		List<global::Tuple<string, TextStyleSetting>> list = new List<global::Tuple<string, TextStyleSetting>>();
		SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.ClusterLayout);
		ClusterLayout clusterLayout;
		SettingsCache.clusterLayouts.clusterCache.TryGetValue(currentQualitySetting.id, out clusterLayout);
		list.Add(new global::Tuple<string, TextStyleSetting>(this.baseName, ToolTipScreen.Instance.defaultTooltipHeaderStyle));
		if (DlcManager.IsExpansion1Active())
		{
			StringEntry entry = Strings.Get(clusterLayout.name);
			list.Add(new global::Tuple<string, TextStyleSetting>(entry, ToolTipScreen.Instance.defaultTooltipBodyStyle));
		}
		if (GameClock.Instance != null)
		{
			list.Add(new global::Tuple<string, TextStyleSetting>(" ", null));
			list.Add(new global::Tuple<string, TextStyleSetting>(string.Format(UI.ASTEROIDCLOCK.CYCLES_OLD, GameUtil.GetCurrentCycle()), ToolTipScreen.Instance.defaultTooltipHeaderStyle));
			list.Add(new global::Tuple<string, TextStyleSetting>(string.Format(UI.ASTEROIDCLOCK.TIME_PLAYED, (GameClock.Instance.GetTimePlayedInSeconds() / 3600f).ToString("0.00")), ToolTipScreen.Instance.defaultTooltipBodyStyle));
		}
		int cameraActiveCluster = CameraController.Instance.cameraActiveCluster;
		WorldContainer world = ClusterManager.Instance.GetWorld(cameraActiveCluster);
		list.Add(new global::Tuple<string, TextStyleSetting>(" ", null));
		if (DlcManager.IsExpansion1Active())
		{
			list.Add(new global::Tuple<string, TextStyleSetting>(world.GetComponent<ClusterGridEntity>().Name, ToolTipScreen.Instance.defaultTooltipHeaderStyle));
		}
		else
		{
			StringEntry entry2 = Strings.Get(clusterLayout.name);
			list.Add(new global::Tuple<string, TextStyleSetting>(entry2, ToolTipScreen.Instance.defaultTooltipHeaderStyle));
		}
		if (SaveLoader.Instance.GameInfo.worldTraits != null && SaveLoader.Instance.GameInfo.worldTraits.Length != 0)
		{
			string[] worldTraits = SaveLoader.Instance.GameInfo.worldTraits;
			for (int i = 0; i < worldTraits.Length; i++)
			{
				WorldTrait cachedWorldTrait = SettingsCache.GetCachedWorldTrait(worldTraits[i], false);
				if (cachedWorldTrait != null)
				{
					list.Add(new global::Tuple<string, TextStyleSetting>(Strings.Get(cachedWorldTrait.name), ToolTipScreen.Instance.defaultTooltipBodyStyle));
				}
				else
				{
					list.Add(new global::Tuple<string, TextStyleSetting>(WORLD_TRAITS.MISSING_TRAIT, ToolTipScreen.Instance.defaultTooltipBodyStyle));
				}
			}
		}
		else if (world.WorldTraitIds != null)
		{
			foreach (string name in world.WorldTraitIds)
			{
				WorldTrait cachedWorldTrait2 = SettingsCache.GetCachedWorldTrait(name, false);
				if (cachedWorldTrait2 != null)
				{
					list.Add(new global::Tuple<string, TextStyleSetting>(Strings.Get(cachedWorldTrait2.name), ToolTipScreen.Instance.defaultTooltipBodyStyle));
				}
				else
				{
					list.Add(new global::Tuple<string, TextStyleSetting>(WORLD_TRAITS.MISSING_TRAIT, ToolTipScreen.Instance.defaultTooltipBodyStyle));
				}
			}
			if (world.WorldTraitIds.Count == 0)
			{
				list.Add(new global::Tuple<string, TextStyleSetting>(WORLD_TRAITS.NO_TRAITS.NAME_SHORTHAND, ToolTipScreen.Instance.defaultTooltipBodyStyle));
			}
		}
		return list;
	}

	// Token: 0x040061D5 RID: 25045
	[Serialize]
	private int speed;

	// Token: 0x040061D6 RID: 25046
	[Serialize]
	public List<Tag> expandedResourceTags = new List<Tag>();

	// Token: 0x040061D7 RID: 25047
	[Serialize]
	public int minGermCountForDisinfect = 10000;

	// Token: 0x040061D8 RID: 25048
	[Serialize]
	public bool enableAutoDisinfect = true;

	// Token: 0x040061D9 RID: 25049
	[Serialize]
	public bool sandboxEnabled;

	// Token: 0x040061DA RID: 25050
	[Serialize]
	public float relativeTemperatureOverlaySliderValue = 294.15f;

	// Token: 0x040061DB RID: 25051
	[Serialize]
	private int autoSaveCycleInterval = 1;

	// Token: 0x040061DC RID: 25052
	[Serialize]
	private Vector2I timelapseResolution = new Vector2I(512, 768);

	// Token: 0x040061DD RID: 25053
	private string baseName;

	// Token: 0x040061DE RID: 25054
	public static SaveGame Instance;

	// Token: 0x040061DF RID: 25055
	private ColonyAchievementTracker colonyAchievementTracker;

	// Token: 0x040061E0 RID: 25056
	public EntombedItemManager entombedItemManager;

	// Token: 0x040061E1 RID: 25057
	public WorldGenSpawner worldGenSpawner;

	// Token: 0x040061E2 RID: 25058
	[MyCmpReq]
	public MaterialSelectorSerializer materialSelectorSerializer;

	// Token: 0x040061E3 RID: 25059
	private static bool debug_SaveFileHeaderBlank_sent;

	// Token: 0x02001861 RID: 6241
	public struct Header
	{
		// Token: 0x17000832 RID: 2098
		// (get) Token: 0x060080AF RID: 32943 RVA: 0x000F9474 File Offset: 0x000F7674
		public bool IsCompressed
		{
			get
			{
				return this.compression != 0;
			}
		}

		// Token: 0x040061E4 RID: 25060
		public uint buildVersion;

		// Token: 0x040061E5 RID: 25061
		public int headerSize;

		// Token: 0x040061E6 RID: 25062
		public uint headerVersion;

		// Token: 0x040061E7 RID: 25063
		public int compression;
	}

	// Token: 0x02001862 RID: 6242
	public struct GameInfo
	{
		// Token: 0x060080B0 RID: 32944 RVA: 0x00341D60 File Offset: 0x0033FF60
		public GameInfo(int numberOfCycles, int numberOfDuplicants, string baseName, bool isAutoSave, string originalSaveName, string clusterId, string[] worldTraits, Guid colonyGuid, List<string> dlcIds, bool sandboxEnabled = false)
		{
			this.numberOfCycles = numberOfCycles;
			this.numberOfDuplicants = numberOfDuplicants;
			this.baseName = baseName;
			this.isAutoSave = isAutoSave;
			this.originalSaveName = originalSaveName;
			this.clusterId = clusterId;
			this.worldTraits = worldTraits;
			this.colonyGuid = colonyGuid;
			this.sandboxEnabled = sandboxEnabled;
			this.dlcIds = dlcIds;
			this.dlcId = null;
			this.saveMajorVersion = 7;
			this.saveMinorVersion = 35;
		}

		// Token: 0x060080B1 RID: 32945 RVA: 0x000F947F File Offset: 0x000F767F
		public bool IsVersionOlderThan(int major, int minor)
		{
			return this.saveMajorVersion < major || (this.saveMajorVersion == major && this.saveMinorVersion < minor);
		}

		// Token: 0x060080B2 RID: 32946 RVA: 0x000F94A0 File Offset: 0x000F76A0
		public bool IsVersionExactly(int major, int minor)
		{
			return this.saveMajorVersion == major && this.saveMinorVersion == minor;
		}

		// Token: 0x060080B3 RID: 32947 RVA: 0x00341DD0 File Offset: 0x0033FFD0
		public bool IsCompatableWithCurrentDlcConfiguration(out HashSet<string> dlcIdsToEnable, out HashSet<string> dlcIdToDisable)
		{
			dlcIdsToEnable = new HashSet<string>();
			foreach (string item in this.dlcIds)
			{
				if (!DlcManager.IsContentSubscribed(item))
				{
					dlcIdsToEnable.Add(item);
				}
			}
			dlcIdToDisable = new HashSet<string>();
			if (!this.dlcIds.Contains("EXPANSION1_ID") && DlcManager.IsExpansion1Active())
			{
				dlcIdToDisable.Add("EXPANSION1_ID");
			}
			return dlcIdsToEnable.Count == 0 && dlcIdToDisable.Count == 0;
		}

		// Token: 0x040061E8 RID: 25064
		public int numberOfCycles;

		// Token: 0x040061E9 RID: 25065
		public int numberOfDuplicants;

		// Token: 0x040061EA RID: 25066
		public string baseName;

		// Token: 0x040061EB RID: 25067
		public bool isAutoSave;

		// Token: 0x040061EC RID: 25068
		public string originalSaveName;

		// Token: 0x040061ED RID: 25069
		public int saveMajorVersion;

		// Token: 0x040061EE RID: 25070
		public int saveMinorVersion;

		// Token: 0x040061EF RID: 25071
		public string clusterId;

		// Token: 0x040061F0 RID: 25072
		public string[] worldTraits;

		// Token: 0x040061F1 RID: 25073
		public bool sandboxEnabled;

		// Token: 0x040061F2 RID: 25074
		public Guid colonyGuid;

		// Token: 0x040061F3 RID: 25075
		[Obsolete("Please use dlcIds instead.")]
		public string dlcId;

		// Token: 0x040061F4 RID: 25076
		public List<string> dlcIds;
	}
}
