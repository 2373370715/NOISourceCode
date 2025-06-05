using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ionic.Zlib;
using Klei;
using Klei.AI;
using Klei.CustomSettings;
using KMod;
using KSerialization;
using Newtonsoft.Json;
using ProcGen;
using ProcGenGame;
using STRINGS;
using UnityEngine;

// Token: 0x02001863 RID: 6243
[AddComponentMenu("KMonoBehaviour/scripts/SaveLoader")]
public class SaveLoader : KMonoBehaviour
{
	// Token: 0x17000833 RID: 2099
	// (get) Token: 0x060080B4 RID: 32948 RVA: 0x000F94B6 File Offset: 0x000F76B6
	// (set) Token: 0x060080B5 RID: 32949 RVA: 0x000F94BE File Offset: 0x000F76BE
	public bool loadedFromSave { get; private set; }

	// Token: 0x060080B6 RID: 32950 RVA: 0x000F94C7 File Offset: 0x000F76C7
	public static void DestroyInstance()
	{
		SaveLoader.Instance = null;
	}

	// Token: 0x17000834 RID: 2100
	// (get) Token: 0x060080B7 RID: 32951 RVA: 0x000F94CF File Offset: 0x000F76CF
	// (set) Token: 0x060080B8 RID: 32952 RVA: 0x000F94D6 File Offset: 0x000F76D6
	public static SaveLoader Instance { get; private set; }

	// Token: 0x17000835 RID: 2101
	// (get) Token: 0x060080B9 RID: 32953 RVA: 0x000F94DE File Offset: 0x000F76DE
	// (set) Token: 0x060080BA RID: 32954 RVA: 0x000F94E6 File Offset: 0x000F76E6
	public Action<Cluster> OnWorldGenComplete { get; set; }

	// Token: 0x17000836 RID: 2102
	// (get) Token: 0x060080BB RID: 32955 RVA: 0x000F94EF File Offset: 0x000F76EF
	public Cluster Cluster
	{
		get
		{
			return this.m_cluster;
		}
	}

	// Token: 0x17000837 RID: 2103
	// (get) Token: 0x060080BC RID: 32956 RVA: 0x000F94F7 File Offset: 0x000F76F7
	public ClusterLayout ClusterLayout
	{
		get
		{
			if (this.m_clusterLayout == null)
			{
				this.m_clusterLayout = CustomGameSettings.Instance.GetCurrentClusterLayout();
			}
			return this.m_clusterLayout;
		}
	}

	// Token: 0x17000838 RID: 2104
	// (get) Token: 0x060080BD RID: 32957 RVA: 0x000F9517 File Offset: 0x000F7717
	// (set) Token: 0x060080BE RID: 32958 RVA: 0x000F951F File Offset: 0x000F771F
	public SaveGame.GameInfo GameInfo { get; private set; }

	// Token: 0x060080BF RID: 32959 RVA: 0x000F9528 File Offset: 0x000F7728
	protected override void OnPrefabInit()
	{
		SaveLoader.Instance = this;
		this.saveManager = base.GetComponent<SaveManager>();
	}

	// Token: 0x060080C0 RID: 32960 RVA: 0x000AA038 File Offset: 0x000A8238
	private void MoveCorruptFile(string filename)
	{
	}

	// Token: 0x060080C1 RID: 32961 RVA: 0x00341E7C File Offset: 0x0034007C
	protected override void OnSpawn()
	{
		string activeSaveFilePath = SaveLoader.GetActiveSaveFilePath();
		if (WorldGen.CanLoad(activeSaveFilePath))
		{
			Sim.SIM_Initialize(new Sim.GAME_MessageHandler(Sim.DLL_MessageHandler));
			SimMessages.CreateSimElementsTable(ElementLoader.elements);
			SimMessages.CreateDiseaseTable(Db.Get().Diseases);
			this.loadedFromSave = true;
			this.loadedFromSave = this.Load(activeSaveFilePath);
			this.saveFileCorrupt = !this.loadedFromSave;
			if (!this.loadedFromSave)
			{
				SaveLoader.SetActiveSaveFilePath(null);
				if (this.mustRestartOnFail)
				{
					this.MoveCorruptFile(activeSaveFilePath);
					Sim.Shutdown();
					App.LoadScene("frontend");
					return;
				}
			}
		}
		if (!this.loadedFromSave)
		{
			Sim.Shutdown();
			if (!string.IsNullOrEmpty(activeSaveFilePath))
			{
				DebugUtil.LogArgs(new object[]
				{
					"Couldn't load [" + activeSaveFilePath + "]"
				});
			}
			if (this.saveFileCorrupt)
			{
				this.MoveCorruptFile(activeSaveFilePath);
			}
			if (!this.LoadFromWorldGen())
			{
				DebugUtil.LogWarningArgs(new object[]
				{
					"Couldn't start new game with current world gen, moving file"
				});
				KMonoBehaviour.isLoadingScene = true;
				this.MoveCorruptFile(WorldGen.WORLDGEN_SAVE_FILENAME);
				App.LoadScene("frontend");
			}
		}
	}

	// Token: 0x060080C2 RID: 32962 RVA: 0x00341F8C File Offset: 0x0034018C
	private static void CompressContents(BinaryWriter fileWriter, byte[] uncompressed, int length)
	{
		using (ZlibStream zlibStream = new ZlibStream(fileWriter.BaseStream, CompressionMode.Compress, Ionic.Zlib.CompressionLevel.BestSpeed))
		{
			zlibStream.Write(uncompressed, 0, length);
			zlibStream.Flush();
		}
	}

	// Token: 0x060080C3 RID: 32963 RVA: 0x00341FD4 File Offset: 0x003401D4
	private byte[] FloatToBytes(float[] floats)
	{
		byte[] array = new byte[floats.Length * 4];
		Buffer.BlockCopy(floats, 0, array, 0, array.Length);
		return array;
	}

	// Token: 0x060080C4 RID: 32964 RVA: 0x000F953C File Offset: 0x000F773C
	private static byte[] DecompressContents(byte[] compressed)
	{
		return ZlibStream.UncompressBuffer(compressed);
	}

	// Token: 0x060080C5 RID: 32965 RVA: 0x00341FFC File Offset: 0x003401FC
	private float[] BytesToFloat(byte[] bytes)
	{
		float[] array = new float[bytes.Length / 4];
		Buffer.BlockCopy(bytes, 0, array, 0, bytes.Length);
		return array;
	}

	// Token: 0x060080C6 RID: 32966 RVA: 0x00342024 File Offset: 0x00340224
	private SaveFileRoot PrepSaveFile()
	{
		SaveFileRoot saveFileRoot = new SaveFileRoot();
		saveFileRoot.WidthInCells = Grid.WidthInCells;
		saveFileRoot.HeightInCells = Grid.HeightInCells;
		saveFileRoot.streamed["GridVisible"] = Grid.Visible;
		saveFileRoot.streamed["GridSpawnable"] = Grid.Spawnable;
		saveFileRoot.streamed["GridDamage"] = this.FloatToBytes(Grid.Damage);
		Global.Instance.modManager.SendMetricsEvent();
		saveFileRoot.active_mods = new List<Label>();
		foreach (Mod mod in Global.Instance.modManager.mods)
		{
			if (mod.IsEnabledForActiveDlc())
			{
				saveFileRoot.active_mods.Add(mod.label);
			}
		}
		using (MemoryStream memoryStream = new MemoryStream())
		{
			using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
			{
				Camera.main.transform.parent.GetComponent<CameraController>().Save(binaryWriter);
			}
			saveFileRoot.streamed["Camera"] = memoryStream.ToArray();
		}
		return saveFileRoot;
	}

	// Token: 0x060080C7 RID: 32967 RVA: 0x000F9544 File Offset: 0x000F7744
	private void Save(BinaryWriter writer)
	{
		writer.WriteKleiString("world");
		Serializer.Serialize(this.PrepSaveFile(), writer);
		Game.SaveSettings(writer);
		Sim.Save(writer, 0, 0);
		this.saveManager.Save(writer);
		Game.Instance.Save(writer);
	}

	// Token: 0x060080C8 RID: 32968 RVA: 0x00342180 File Offset: 0x00340380
	private bool Load(IReader reader)
	{
		global::Debug.Assert(reader.ReadKleiString() == "world");
		Deserializer deserializer = new Deserializer(reader);
		SaveFileRoot saveFileRoot = new SaveFileRoot();
		deserializer.Deserialize(saveFileRoot);
		if ((this.GameInfo.saveMajorVersion == 7 || this.GameInfo.saveMinorVersion < 8) && saveFileRoot.requiredMods != null)
		{
			saveFileRoot.active_mods = new List<Label>();
			foreach (ModInfo modInfo in saveFileRoot.requiredMods)
			{
				saveFileRoot.active_mods.Add(new Label
				{
					id = modInfo.assetID,
					version = (long)modInfo.lastModifiedTime,
					distribution_platform = Label.DistributionPlatform.Steam,
					title = modInfo.description
				});
			}
			saveFileRoot.requiredMods.Clear();
		}
		KMod.Manager modManager = Global.Instance.modManager;
		modManager.Load(Content.LayerableFiles);
		if (!modManager.MatchFootprint(saveFileRoot.active_mods, Content.LayerableFiles | Content.Strings | Content.DLL | Content.Translation | Content.Animation))
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"Mod footprint of save file doesn't match current mod configuration"
			});
		}
		string text = string.Format("Mod Footprint ({0}):", saveFileRoot.active_mods.Count);
		foreach (Label label in saveFileRoot.active_mods)
		{
			text = text + "\n  - " + label.title;
		}
		global::Debug.Log(text);
		this.LogActiveMods();
		Global.Instance.modManager.SendMetricsEvent();
		WorldGen.LoadSettings(false);
		CustomGameSettings.Instance.LoadClusters();
		if (this.GameInfo.clusterId == null)
		{
			SaveGame.GameInfo gameInfo = this.GameInfo;
			if (!string.IsNullOrEmpty(saveFileRoot.clusterID))
			{
				gameInfo.clusterId = saveFileRoot.clusterID;
			}
			else
			{
				try
				{
					gameInfo.clusterId = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.ClusterLayout).id;
				}
				catch
				{
					gameInfo.clusterId = WorldGenSettings.ClusterDefaultName;
					CustomGameSettings.Instance.SetQualitySetting(CustomGameSettingConfigs.ClusterLayout, gameInfo.clusterId);
				}
			}
			this.GameInfo = gameInfo;
		}
		Game.clusterId = this.GameInfo.clusterId;
		Game.LoadSettings(deserializer);
		GridSettings.Reset(saveFileRoot.WidthInCells, saveFileRoot.HeightInCells);
		if (Application.isPlaying)
		{
			Singleton<KBatchedAnimUpdater>.Instance.InitializeGrid();
		}
		Sim.SIM_Initialize(new Sim.GAME_MessageHandler(Sim.DLL_MessageHandler));
		SimMessages.CreateSimElementsTable(ElementLoader.elements);
		Sim.AllocateCells(saveFileRoot.WidthInCells, saveFileRoot.HeightInCells, false);
		SimMessages.CreateDiseaseTable(Db.Get().Diseases);
		Sim.HandleMessage(SimMessageHashes.ClearUnoccupiedCells, 0, null);
		IReader reader2;
		if (saveFileRoot.streamed.ContainsKey("Sim"))
		{
			reader2 = new FastReader(saveFileRoot.streamed["Sim"]);
		}
		else
		{
			reader2 = reader;
		}
		if (Sim.LoadWorld(reader2) != 0)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"\n--- Error loading save ---\nSimDLL found bad data\n"
			});
			Sim.Shutdown();
			return false;
		}
		Sim.Start();
		SceneInitializer.Instance.PostLoadPrefabs();
		this.mustRestartOnFail = true;
		if (!this.saveManager.Load(reader))
		{
			Sim.Shutdown();
			DebugUtil.LogWarningArgs(new object[]
			{
				"\n--- Error loading save ---\n"
			});
			SaveLoader.SetActiveSaveFilePath(null);
			return false;
		}
		Grid.Visible = saveFileRoot.streamed["GridVisible"];
		if (saveFileRoot.streamed.ContainsKey("GridSpawnable"))
		{
			Grid.Spawnable = saveFileRoot.streamed["GridSpawnable"];
		}
		Grid.Damage = this.BytesToFloat(saveFileRoot.streamed["GridDamage"]);
		Game.Instance.Load(deserializer);
		CameraSaveData.Load(new FastReader(saveFileRoot.streamed["Camera"]));
		ClusterManager.Instance.InitializeWorldGrid();
		SimMessages.DefineWorldOffsets((from container in ClusterManager.Instance.WorldContainers
		select new SimMessages.WorldOffsetData
		{
			worldOffsetX = container.WorldOffset.x,
			worldOffsetY = container.WorldOffset.y,
			worldSizeX = container.WorldSize.x,
			worldSizeY = container.WorldSize.y
		}).ToList<SimMessages.WorldOffsetData>());
		return true;
	}

	// Token: 0x060080C9 RID: 32969 RVA: 0x003425A4 File Offset: 0x003407A4
	private void LogActiveMods()
	{
		string text = string.Format("Active Mods ({0}):", Global.Instance.modManager.mods.Count((Mod x) => x.IsEnabledForActiveDlc()));
		foreach (Mod mod in Global.Instance.modManager.mods)
		{
			if (mod.IsEnabledForActiveDlc())
			{
				text = text + "\n  - " + mod.title;
			}
		}
		global::Debug.Log(text);
	}

	// Token: 0x060080CA RID: 32970 RVA: 0x000F9582 File Offset: 0x000F7782
	public static string GetSavePrefix()
	{
		return System.IO.Path.Combine(global::Util.RootFolder(), string.Format("{0}{1}", "save_files", System.IO.Path.DirectorySeparatorChar));
	}

	// Token: 0x060080CB RID: 32971 RVA: 0x0034265C File Offset: 0x0034085C
	public static string GetCloudSavePrefix()
	{
		string text = System.IO.Path.Combine(global::Util.RootFolder(), string.Format("{0}{1}", "cloud_save_files", System.IO.Path.DirectorySeparatorChar));
		string userID = SaveLoader.GetUserID();
		if (string.IsNullOrEmpty(userID))
		{
			return null;
		}
		text = System.IO.Path.Combine(text, userID);
		if (!System.IO.Directory.Exists(text))
		{
			System.IO.Directory.CreateDirectory(text);
		}
		return text;
	}

	// Token: 0x060080CC RID: 32972 RVA: 0x003426B8 File Offset: 0x003408B8
	public static string GetSavePrefixAndCreateFolder()
	{
		string savePrefix = SaveLoader.GetSavePrefix();
		if (!System.IO.Directory.Exists(savePrefix))
		{
			System.IO.Directory.CreateDirectory(savePrefix);
		}
		return savePrefix;
	}

	// Token: 0x060080CD RID: 32973 RVA: 0x003426DC File Offset: 0x003408DC
	public static string GetUserID()
	{
		DistributionPlatform.User localUser = DistributionPlatform.Inst.LocalUser;
		if (localUser == null)
		{
			return null;
		}
		return localUser.Id.ToString();
	}

	// Token: 0x060080CE RID: 32974 RVA: 0x00342704 File Offset: 0x00340904
	public static string GetNextUsableSavePath(string filename)
	{
		int num = 0;
		string arg = System.IO.Path.ChangeExtension(filename, null);
		while (File.Exists(filename))
		{
			filename = SaveScreen.GetValidSaveFilename(string.Format("{0} ({1})", arg, num));
			num++;
		}
		return filename;
	}

	// Token: 0x060080CF RID: 32975 RVA: 0x000F95A7 File Offset: 0x000F77A7
	public static string GetOriginalSaveFileName(string filename)
	{
		if (!filename.Contains("/") && !filename.Contains("\\"))
		{
			return filename;
		}
		filename.Replace('\\', '/');
		return System.IO.Path.GetFileName(filename);
	}

	// Token: 0x060080D0 RID: 32976 RVA: 0x000F95D6 File Offset: 0x000F77D6
	public static bool IsSaveAuto(string filename)
	{
		filename = filename.Replace('\\', '/');
		return filename.Contains("/auto_save/");
	}

	// Token: 0x060080D1 RID: 32977 RVA: 0x000F95EF File Offset: 0x000F77EF
	public static bool IsSaveLocal(string filename)
	{
		filename = filename.Replace('\\', '/');
		return filename.Contains("/save_files/");
	}

	// Token: 0x060080D2 RID: 32978 RVA: 0x000F9608 File Offset: 0x000F7808
	public static bool IsSaveCloud(string filename)
	{
		filename = filename.Replace('\\', '/');
		return filename.Contains("/cloud_save_files/");
	}

	// Token: 0x060080D3 RID: 32979 RVA: 0x00342744 File Offset: 0x00340944
	public static string GetAutoSavePrefix()
	{
		string text = System.IO.Path.Combine(SaveLoader.GetSavePrefixAndCreateFolder(), string.Format("{0}{1}", "auto_save", System.IO.Path.DirectorySeparatorChar));
		if (!System.IO.Directory.Exists(text))
		{
			System.IO.Directory.CreateDirectory(text);
		}
		return text;
	}

	// Token: 0x060080D4 RID: 32980 RVA: 0x000F9621 File Offset: 0x000F7821
	public static void SetActiveSaveFilePath(string path)
	{
		KPlayerPrefs.SetString("SaveFilenameKey/", path);
	}

	// Token: 0x060080D5 RID: 32981 RVA: 0x000F962E File Offset: 0x000F782E
	public static string GetActiveSaveFilePath()
	{
		return KPlayerPrefs.GetString("SaveFilenameKey/");
	}

	// Token: 0x060080D6 RID: 32982 RVA: 0x00342788 File Offset: 0x00340988
	public static string GetActiveAutoSavePath()
	{
		string activeSaveFilePath = SaveLoader.GetActiveSaveFilePath();
		if (activeSaveFilePath == null)
		{
			return SaveLoader.GetAutoSavePrefix();
		}
		return System.IO.Path.Combine(System.IO.Path.GetDirectoryName(activeSaveFilePath), "auto_save");
	}

	// Token: 0x060080D7 RID: 32983 RVA: 0x000F963A File Offset: 0x000F783A
	public static string GetAutosaveFilePath()
	{
		return SaveLoader.GetAutoSavePrefix() + "AutoSave Cycle 1.sav";
	}

	// Token: 0x060080D8 RID: 32984 RVA: 0x003427B4 File Offset: 0x003409B4
	public static string GetActiveSaveColonyFolder()
	{
		string text = SaveLoader.GetActiveSaveFolder();
		if (text == null)
		{
			text = System.IO.Path.Combine(SaveLoader.GetSavePrefix(), SaveLoader.Instance.GameInfo.baseName);
		}
		return text;
	}

	// Token: 0x060080D9 RID: 32985 RVA: 0x003427E8 File Offset: 0x003409E8
	public static string GetActiveSaveFolder()
	{
		string activeSaveFilePath = SaveLoader.GetActiveSaveFilePath();
		if (!string.IsNullOrEmpty(activeSaveFilePath))
		{
			return System.IO.Path.GetDirectoryName(activeSaveFilePath);
		}
		return null;
	}

	// Token: 0x060080DA RID: 32986 RVA: 0x0034280C File Offset: 0x00340A0C
	public static List<SaveLoader.SaveFileEntry> GetSaveFiles(string save_dir, bool sort, SearchOption search = SearchOption.AllDirectories)
	{
		List<SaveLoader.SaveFileEntry> list = new List<SaveLoader.SaveFileEntry>();
		if (string.IsNullOrEmpty(save_dir))
		{
			return list;
		}
		try
		{
			if (!System.IO.Directory.Exists(save_dir))
			{
				System.IO.Directory.CreateDirectory(save_dir);
			}
			foreach (string text in System.IO.Directory.GetFiles(save_dir, "*.sav", search))
			{
				try
				{
					System.DateTime lastWriteTimeUtc = File.GetLastWriteTimeUtc(text);
					SaveLoader.SaveFileEntry item = new SaveLoader.SaveFileEntry
					{
						path = text,
						timeStamp = lastWriteTimeUtc
					};
					list.Add(item);
				}
				catch (Exception ex)
				{
					global::Debug.LogWarning("Problem reading file: " + text + "\n" + ex.ToString());
				}
			}
			if (sort)
			{
				list.Sort((SaveLoader.SaveFileEntry x, SaveLoader.SaveFileEntry y) => y.timeStamp.CompareTo(x.timeStamp));
			}
		}
		catch (Exception ex2)
		{
			string text2 = null;
			if (ex2 is UnauthorizedAccessException)
			{
				text2 = string.Format(UI.FRONTEND.SUPPORTWARNINGS.SAVE_DIRECTORY_READ_ONLY, save_dir);
			}
			else if (ex2 is IOException)
			{
				text2 = string.Format(UI.FRONTEND.SUPPORTWARNINGS.SAVE_DIRECTORY_INSUFFICIENT_SPACE, save_dir);
			}
			if (text2 == null)
			{
				throw ex2;
			}
			GameObject parent = (FrontEndManager.Instance == null) ? GameScreenManager.Instance.ssOverlayCanvas : FrontEndManager.Instance.gameObject;
			global::Util.KInstantiateUI(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, parent, true).GetComponent<ConfirmDialogScreen>().PopupConfirmDialog(text2, null, null, null, null, null, null, null, null);
		}
		return list;
	}

	// Token: 0x060080DB RID: 32987 RVA: 0x0034298C File Offset: 0x00340B8C
	public static List<SaveLoader.SaveFileEntry> GetAllFiles(bool sort, SaveLoader.SaveType type = SaveLoader.SaveType.both)
	{
		switch (type)
		{
		case SaveLoader.SaveType.local:
			return SaveLoader.GetSaveFiles(SaveLoader.GetSavePrefixAndCreateFolder(), sort, SearchOption.AllDirectories);
		case SaveLoader.SaveType.cloud:
			return SaveLoader.GetSaveFiles(SaveLoader.GetCloudSavePrefix(), sort, SearchOption.AllDirectories);
		case SaveLoader.SaveType.both:
		{
			List<SaveLoader.SaveFileEntry> saveFiles = SaveLoader.GetSaveFiles(SaveLoader.GetSavePrefixAndCreateFolder(), false, SearchOption.AllDirectories);
			List<SaveLoader.SaveFileEntry> saveFiles2 = SaveLoader.GetSaveFiles(SaveLoader.GetCloudSavePrefix(), false, SearchOption.AllDirectories);
			saveFiles.AddRange(saveFiles2);
			if (sort)
			{
				saveFiles.Sort((SaveLoader.SaveFileEntry x, SaveLoader.SaveFileEntry y) => y.timeStamp.CompareTo(x.timeStamp));
			}
			return saveFiles;
		}
		default:
			return new List<SaveLoader.SaveFileEntry>();
		}
	}

	// Token: 0x060080DC RID: 32988 RVA: 0x000F964B File Offset: 0x000F784B
	public static List<SaveLoader.SaveFileEntry> GetAllColonyFiles(bool sort, SearchOption search = SearchOption.TopDirectoryOnly)
	{
		return SaveLoader.GetSaveFiles(SaveLoader.GetActiveSaveColonyFolder(), sort, search);
	}

	// Token: 0x060080DD RID: 32989 RVA: 0x000F9659 File Offset: 0x000F7859
	public static bool GetCloudSavesDefault()
	{
		return !(SaveLoader.GetCloudSavesDefaultPref() == "Disabled");
	}

	// Token: 0x060080DE RID: 32990 RVA: 0x00342A18 File Offset: 0x00340C18
	public static string GetCloudSavesDefaultPref()
	{
		string text = KPlayerPrefs.GetString("SavesDefaultToCloud", "Enabled");
		if (text != "Enabled" && text != "Disabled")
		{
			text = "Enabled";
		}
		return text;
	}

	// Token: 0x060080DF RID: 32991 RVA: 0x000F966F File Offset: 0x000F786F
	public static void SetCloudSavesDefault(bool value)
	{
		SaveLoader.SetCloudSavesDefaultPref(value ? "Enabled" : "Disabled");
	}

	// Token: 0x060080E0 RID: 32992 RVA: 0x000F9685 File Offset: 0x000F7885
	public static void SetCloudSavesDefaultPref(string pref)
	{
		if (pref != "Enabled" && pref != "Disabled")
		{
			global::Debug.LogWarning("Ignoring cloud saves default pref `" + pref + "` as it's not valid, expected `Enabled` or `Disabled`");
			return;
		}
		KPlayerPrefs.SetString("SavesDefaultToCloud", pref);
	}

	// Token: 0x060080E1 RID: 32993 RVA: 0x000F96C2 File Offset: 0x000F78C2
	public static bool GetCloudSavesAvailable()
	{
		return !string.IsNullOrEmpty(SaveLoader.GetUserID()) && SaveLoader.GetCloudSavePrefix() != null;
	}

	// Token: 0x060080E2 RID: 32994 RVA: 0x00342A58 File Offset: 0x00340C58
	public static string GetLatestSaveForCurrentDLC()
	{
		List<SaveLoader.SaveFileEntry> allFiles = SaveLoader.GetAllFiles(true, SaveLoader.SaveType.both);
		for (int i = 0; i < allFiles.Count; i++)
		{
			global::Tuple<SaveGame.Header, SaveGame.GameInfo> fileInfo = SaveGame.GetFileInfo(allFiles[i].path);
			if (fileInfo != null)
			{
				SaveGame.Header first = fileInfo.first;
				SaveGame.GameInfo second = fileInfo.second;
				HashSet<string> hashSet;
				HashSet<string> hashSet2;
				if (second.saveMajorVersion >= 7 && second.IsCompatableWithCurrentDlcConfiguration(out hashSet, out hashSet2))
				{
					return allFiles[i].path;
				}
			}
		}
		return null;
	}

	// Token: 0x060080E3 RID: 32995 RVA: 0x00342ACC File Offset: 0x00340CCC
	public void InitialSave()
	{
		string text = SaveLoader.GetActiveSaveFilePath();
		if (string.IsNullOrEmpty(text))
		{
			text = SaveLoader.GetAutosaveFilePath();
		}
		else if (!text.Contains(".sav"))
		{
			text += ".sav";
		}
		this.LogActiveMods();
		this.Save(text, false, true);
	}

	// Token: 0x060080E4 RID: 32996 RVA: 0x00342B18 File Offset: 0x00340D18
	public string Save(string filename, bool isAutoSave = false, bool updateSavePointer = true)
	{
		KSerialization.Manager.Clear();
		string directoryName = System.IO.Path.GetDirectoryName(filename);
		try
		{
			if (directoryName != null && !System.IO.Directory.Exists(directoryName))
			{
				System.IO.Directory.CreateDirectory(directoryName);
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogWarning("Problem creating save folder for " + filename + "!\n" + ex.ToString());
		}
		this.ReportSaveMetrics(isAutoSave);
		RetireColonyUtility.SaveColonySummaryData();
		if (isAutoSave && !GenericGameSettings.instance.keepAllAutosaves)
		{
			List<SaveLoader.SaveFileEntry> saveFiles = SaveLoader.GetSaveFiles(SaveLoader.GetActiveAutoSavePath(), true, SearchOption.AllDirectories);
			List<string> list = new List<string>();
			foreach (SaveLoader.SaveFileEntry saveFileEntry in saveFiles)
			{
				global::Tuple<SaveGame.Header, SaveGame.GameInfo> fileInfo = SaveGame.GetFileInfo(saveFileEntry.path);
				if (fileInfo != null && SaveGame.GetSaveUniqueID(fileInfo.second) == SaveLoader.Instance.GameInfo.colonyGuid.ToString())
				{
					list.Add(saveFileEntry.path);
				}
			}
			for (int i = list.Count - 1; i >= 9; i--)
			{
				string text = list[i];
				try
				{
					global::Debug.Log("Deleting old autosave: " + text);
					File.Delete(text);
				}
				catch (Exception ex2)
				{
					global::Debug.LogWarning("Problem deleting autosave: " + text + "\n" + ex2.ToString());
				}
				string text2 = System.IO.Path.ChangeExtension(text, ".png");
				try
				{
					if (File.Exists(text2))
					{
						File.Delete(text2);
					}
				}
				catch (Exception ex3)
				{
					global::Debug.LogWarning("Problem deleting autosave screenshot: " + text2 + "\n" + ex3.ToString());
				}
			}
		}
		using (MemoryStream memoryStream = new MemoryStream((int)((float)this.lastUncompressedSize * 1.1f)))
		{
			using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
			{
				this.Save(binaryWriter);
				this.lastUncompressedSize = (int)memoryStream.Length;
				try
				{
					using (BinaryWriter binaryWriter2 = new BinaryWriter(File.Open(filename, FileMode.Create)))
					{
						SaveGame.Header header;
						byte[] saveHeader = SaveGame.Instance.GetSaveHeader(isAutoSave, this.compressSaveData, out header);
						binaryWriter2.Write(header.buildVersion);
						binaryWriter2.Write(header.headerSize);
						binaryWriter2.Write(header.headerVersion);
						binaryWriter2.Write(header.compression);
						binaryWriter2.Write(saveHeader);
						KSerialization.Manager.SerializeDirectory(binaryWriter2);
						if (this.compressSaveData)
						{
							SaveLoader.CompressContents(binaryWriter2, memoryStream.GetBuffer(), (int)memoryStream.Length);
						}
						else
						{
							binaryWriter2.Write(memoryStream.ToArray());
						}
						KCrashReporter.MOST_RECENT_SAVEFILE = filename;
						Stats.Print();
					}
				}
				catch (Exception ex4)
				{
					if (ex4 is UnauthorizedAccessException)
					{
						DebugUtil.LogArgs(new object[]
						{
							"UnauthorizedAccessException for " + filename
						});
						((ConfirmDialogScreen)GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay)).PopupConfirmDialog(string.Format(UI.CRASHSCREEN.SAVEFAILED, "Unauthorized Access Exception"), null, null, null, null, null, null, null, null);
						return SaveLoader.GetActiveSaveFilePath();
					}
					if (ex4 is IOException)
					{
						DebugUtil.LogArgs(new object[]
						{
							"IOException (probably out of disk space) for " + filename
						});
						((ConfirmDialogScreen)GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay)).PopupConfirmDialog(string.Format(UI.CRASHSCREEN.SAVEFAILED, "IOException. You may not have enough free space!"), null, null, null, null, null, null, null, null);
						return SaveLoader.GetActiveSaveFilePath();
					}
					throw ex4;
				}
			}
		}
		if (updateSavePointer)
		{
			SaveLoader.SetActiveSaveFilePath(filename);
		}
		Game.Instance.timelapser.SaveColonyPreview(filename);
		DebugUtil.LogArgs(new object[]
		{
			"Saved to",
			"[" + filename + "]"
		});
		GC.Collect();
		return filename;
	}

	// Token: 0x060080E5 RID: 32997 RVA: 0x00342FC0 File Offset: 0x003411C0
	public static SaveGame.GameInfo LoadHeader(string filename, out SaveGame.Header header)
	{
		byte[] array = new byte[512];
		SaveGame.GameInfo header2;
		using (FileStream fileStream = File.OpenRead(filename))
		{
			fileStream.Read(array, 0, 512);
			header2 = SaveGame.GetHeader(new FastReader(array), out header, filename);
		}
		return header2;
	}

	// Token: 0x060080E6 RID: 32998 RVA: 0x00343018 File Offset: 0x00341218
	public bool Load(string filename)
	{
		SaveLoader.SetActiveSaveFilePath(filename);
		try
		{
			KSerialization.Manager.Clear();
			byte[] array = File.ReadAllBytes(filename);
			IReader reader = new FastReader(array);
			SaveGame.Header header;
			this.GameInfo = SaveGame.GetHeader(reader, out header, filename);
			ThreadedHttps<KleiMetrics>.Instance.SetExpansionsActive(this.GameInfo.dlcIds);
			DebugUtil.LogArgs(new object[]
			{
				string.Format("Loading save file: {4}\n headerVersion:{0}, buildVersion:{1}, headerSize:{2}, IsCompressed:{3}", new object[]
				{
					header.headerVersion,
					header.buildVersion,
					header.headerSize,
					header.IsCompressed,
					filename
				})
			});
			DebugUtil.LogArgs(new object[]
			{
				string.Format("GameInfo loaded from save header:\n  numberOfCycles:{0},\n  numberOfDuplicants:{1},\n  baseName:{2},\n  isAutoSave:{3},\n  originalSaveName:{4},\n  clusterId:{5},\n  worldTraits:{6},\n  colonyGuid:{7},\n  saveVersion:{8}.{9}", new object[]
				{
					this.GameInfo.numberOfCycles,
					this.GameInfo.numberOfDuplicants,
					this.GameInfo.baseName,
					this.GameInfo.isAutoSave,
					this.GameInfo.originalSaveName,
					this.GameInfo.clusterId,
					(this.GameInfo.worldTraits != null && this.GameInfo.worldTraits.Length != 0) ? string.Join(", ", this.GameInfo.worldTraits) : "<i>none</i>",
					this.GameInfo.colonyGuid,
					this.GameInfo.saveMajorVersion,
					this.GameInfo.saveMinorVersion
				})
			});
			string originalSaveName = this.GameInfo.originalSaveName;
			if (originalSaveName.Contains("/") || originalSaveName.Contains("\\"))
			{
				string originalSaveFileName = SaveLoader.GetOriginalSaveFileName(originalSaveName);
				SaveGame.GameInfo gameInfo = this.GameInfo;
				gameInfo.originalSaveName = originalSaveFileName;
				this.GameInfo = gameInfo;
				global::Debug.Log(string.Concat(new string[]
				{
					"Migration / Save originalSaveName updated from: `",
					originalSaveName,
					"` => `",
					this.GameInfo.originalSaveName,
					"`"
				}));
			}
			if (this.GameInfo.saveMajorVersion == 7 && this.GameInfo.saveMinorVersion < 4)
			{
				Helper.SetTypeInfoMask((SerializationTypeInfo)191);
			}
			KSerialization.Manager.DeserializeDirectory(reader);
			if (header.IsCompressed)
			{
				int num = array.Length - reader.Position;
				byte[] array2 = new byte[num];
				Array.Copy(array, reader.Position, array2, 0, num);
				byte[] array3 = SaveLoader.DecompressContents(array2);
				this.lastUncompressedSize = array3.Length;
				IReader reader2 = new FastReader(array3);
				this.Load(reader2);
			}
			else
			{
				this.lastUncompressedSize = array.Length;
				this.Load(reader);
			}
			KCrashReporter.MOST_RECENT_SAVEFILE = filename;
			if (this.GameInfo.isAutoSave && !string.IsNullOrEmpty(this.GameInfo.originalSaveName))
			{
				string originalSaveFileName2 = SaveLoader.GetOriginalSaveFileName(this.GameInfo.originalSaveName);
				string text;
				if (SaveLoader.IsSaveCloud(filename))
				{
					string cloudSavePrefix = SaveLoader.GetCloudSavePrefix();
					if (cloudSavePrefix != null)
					{
						text = System.IO.Path.Combine(cloudSavePrefix, this.GameInfo.baseName, originalSaveFileName2);
					}
					else
					{
						text = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(filename).Replace("auto_save", ""), this.GameInfo.baseName, originalSaveFileName2);
					}
				}
				else
				{
					text = System.IO.Path.Combine(SaveLoader.GetSavePrefix(), this.GameInfo.baseName, originalSaveFileName2);
				}
				if (text != null)
				{
					SaveLoader.SetActiveSaveFilePath(text);
				}
			}
		}
		catch (Exception ex)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"\n--- Error loading save ---\n" + ex.Message + "\n" + ex.StackTrace
			});
			Sim.Shutdown();
			SaveLoader.SetActiveSaveFilePath(null);
			return false;
		}
		Stats.Print();
		DebugUtil.LogArgs(new object[]
		{
			"Loaded",
			"[" + filename + "]"
		});
		DebugUtil.LogArgs(new object[]
		{
			"World Seeds",
			string.Concat(new string[]
			{
				"[",
				this.clusterDetailSave.globalWorldSeed.ToString(),
				"/",
				this.clusterDetailSave.globalWorldLayoutSeed.ToString(),
				"/",
				this.clusterDetailSave.globalTerrainSeed.ToString(),
				"/",
				this.clusterDetailSave.globalNoiseSeed.ToString(),
				"]"
			})
		});
		GC.Collect();
		return true;
	}

	// Token: 0x060080E7 RID: 32999 RVA: 0x003434AC File Offset: 0x003416AC
	public bool LoadFromWorldGen()
	{
		DebugUtil.LogArgs(new object[]
		{
			"Attempting to start a new game with current world gen"
		});
		WorldGen.LoadSettings(false);
		FastReader reader = new FastReader(File.ReadAllBytes(WorldGen.WORLDGEN_SAVE_FILENAME));
		this.m_cluster = Cluster.Load(reader);
		ListPool<SimSaveFileStructure, SaveLoader>.PooledList pooledList = ListPool<SimSaveFileStructure, SaveLoader>.Allocate();
		this.m_cluster.LoadClusterSim(pooledList, reader);
		SaveGame.GameInfo gameInfo = this.GameInfo;
		gameInfo.clusterId = this.m_cluster.Id;
		gameInfo.colonyGuid = Guid.NewGuid();
		ClusterLayout currentClusterLayout = CustomGameSettings.Instance.GetCurrentClusterLayout();
		gameInfo.dlcIds = new List<string>(currentClusterLayout.requiredDlcIds);
		foreach (string item in CustomGameSettings.Instance.GetCurrentDlcMixingIds())
		{
			if (!gameInfo.dlcIds.Contains(item))
			{
				gameInfo.dlcIds.Add(item);
			}
		}
		this.GameInfo = gameInfo;
		ThreadedHttps<KleiMetrics>.Instance.SetExpansionsActive(this.GameInfo.dlcIds);
		if (pooledList.Count != this.m_cluster.worlds.Count)
		{
			global::Debug.LogError("Attempt failed. Failed to load all worlds.");
			pooledList.Recycle();
			return false;
		}
		GridSettings.Reset(this.m_cluster.size.x, this.m_cluster.size.y);
		if (Application.isPlaying)
		{
			Singleton<KBatchedAnimUpdater>.Instance.InitializeGrid();
		}
		this.clusterDetailSave = new WorldDetailSave();
		foreach (SimSaveFileStructure simSaveFileStructure in pooledList)
		{
			this.clusterDetailSave.globalNoiseSeed = simSaveFileStructure.worldDetail.globalNoiseSeed;
			this.clusterDetailSave.globalTerrainSeed = simSaveFileStructure.worldDetail.globalTerrainSeed;
			this.clusterDetailSave.globalWorldLayoutSeed = simSaveFileStructure.worldDetail.globalWorldLayoutSeed;
			this.clusterDetailSave.globalWorldSeed = simSaveFileStructure.worldDetail.globalWorldSeed;
			Vector2 b = Grid.CellToPos2D(Grid.PosToCell(new Vector2I(simSaveFileStructure.x, simSaveFileStructure.y)));
			foreach (WorldDetailSave.OverworldCell overworldCell in simSaveFileStructure.worldDetail.overworldCells)
			{
				for (int num = 0; num != overworldCell.poly.Vertices.Count; num++)
				{
					List<Vector2> vertices = overworldCell.poly.Vertices;
					int index = num;
					vertices[index] += b;
				}
				overworldCell.poly.RefreshBounds();
			}
			this.clusterDetailSave.overworldCells.AddRange(simSaveFileStructure.worldDetail.overworldCells);
		}
		Sim.SIM_Initialize(new Sim.GAME_MessageHandler(Sim.DLL_MessageHandler));
		SimMessages.CreateSimElementsTable(ElementLoader.elements);
		Sim.AllocateCells(this.m_cluster.size.x, this.m_cluster.size.y, false);
		SimMessages.DefineWorldOffsets((from world in this.m_cluster.worlds
		select new SimMessages.WorldOffsetData
		{
			worldOffsetX = world.WorldOffset.x,
			worldOffsetY = world.WorldOffset.y,
			worldSizeX = world.WorldSize.x,
			worldSizeY = world.WorldSize.y
		}).ToList<SimMessages.WorldOffsetData>());
		SimMessages.CreateDiseaseTable(Db.Get().Diseases);
		Sim.HandleMessage(SimMessageHashes.ClearUnoccupiedCells, 0, null);
		try
		{
			foreach (SimSaveFileStructure simSaveFileStructure2 in pooledList)
			{
				FastReader reader2 = new FastReader(simSaveFileStructure2.Sim);
				if (Sim.Load(reader2) != 0)
				{
					DebugUtil.LogWarningArgs(new object[]
					{
						"\n--- Error loading save ---\nSimDLL found bad data\n"
					});
					Sim.Shutdown();
					pooledList.Recycle();
					return false;
				}
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogWarning("--- Error loading Sim FROM NEW WORLDGEN ---" + ex.Message + "\n" + ex.StackTrace);
			Sim.Shutdown();
			pooledList.Recycle();
			return false;
		}
		global::Debug.Log("Attempt success");
		Sim.Start();
		SceneInitializer.Instance.PostLoadPrefabs();
		SceneInitializer.Instance.NewSaveGamePrefab();
		this.cachedGSD = this.m_cluster.currentWorld.SpawnData;
		this.OnWorldGenComplete.Signal(this.m_cluster);
		OniMetrics.LogEvent(OniMetrics.Event.NewSave, "NewGame", true);
		StoryManager.Instance.InitialSaveSetup();
		ThreadedHttps<KleiMetrics>.Instance.IncrementGameCount();
		OniMetrics.SendEvent(OniMetrics.Event.NewSave, "New Save");
		pooledList.Recycle();
		return true;
	}

	// Token: 0x17000839 RID: 2105
	// (get) Token: 0x060080E8 RID: 33000 RVA: 0x000F96DC File Offset: 0x000F78DC
	// (set) Token: 0x060080E9 RID: 33001 RVA: 0x000F96E4 File Offset: 0x000F78E4
	public GameSpawnData cachedGSD { get; private set; }

	// Token: 0x1700083A RID: 2106
	// (get) Token: 0x060080EA RID: 33002 RVA: 0x000F96ED File Offset: 0x000F78ED
	// (set) Token: 0x060080EB RID: 33003 RVA: 0x000F96F5 File Offset: 0x000F78F5
	public WorldDetailSave clusterDetailSave { get; private set; }

	// Token: 0x060080EC RID: 33004 RVA: 0x000F96FE File Offset: 0x000F78FE
	public void SetWorldDetail(WorldDetailSave worldDetail)
	{
		this.clusterDetailSave = worldDetail;
	}

	// Token: 0x060080ED RID: 33005 RVA: 0x003439B0 File Offset: 0x00341BB0
	private void ReportSaveMetrics(bool is_auto_save)
	{
		if (ThreadedHttps<KleiMetrics>.Instance == null || !ThreadedHttps<KleiMetrics>.Instance.enabled || this.saveManager == null)
		{
			return;
		}
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary[GameClock.NewCycleKey] = GameClock.Instance.GetCycle() + 1;
		dictionary["IsAutoSave"] = is_auto_save;
		dictionary["SavedPrefabs"] = this.GetSavedPrefabMetrics();
		dictionary["ResourcesAccessible"] = this.GetWorldInventoryMetrics();
		dictionary["MinionMetrics"] = this.GetMinionMetrics();
		dictionary["WorldMetrics"] = this.GetWorldMetrics();
		if (is_auto_save)
		{
			dictionary["DailyReport"] = this.GetDailyReportMetrics();
			dictionary["PerformanceMeasurements"] = this.GetPerformanceMeasurements();
			dictionary["AverageFrameTime"] = this.GetFrameTime();
		}
		dictionary["CustomGameSettings"] = CustomGameSettings.Instance.GetSettingsForMetrics();
		dictionary["CustomMixingSettings"] = CustomGameSettings.Instance.GetSettingsForMixingMetrics();
		ThreadedHttps<KleiMetrics>.Instance.SendEvent(dictionary, "ReportSaveMetrics");
	}

	// Token: 0x060080EE RID: 33006 RVA: 0x00343ACC File Offset: 0x00341CCC
	private List<SaveLoader.MinionMetricsData> GetMinionMetrics()
	{
		List<SaveLoader.MinionMetricsData> list = new List<SaveLoader.MinionMetricsData>();
		foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.Items)
		{
			if (!(minionIdentity == null))
			{
				Amounts amounts = minionIdentity.gameObject.GetComponent<Modifiers>().amounts;
				List<SaveLoader.MinionAttrFloatData> list2 = new List<SaveLoader.MinionAttrFloatData>(amounts.Count);
				foreach (AmountInstance amountInstance in amounts)
				{
					float value = amountInstance.value;
					if (!float.IsNaN(value) && !float.IsInfinity(value))
					{
						list2.Add(new SaveLoader.MinionAttrFloatData
						{
							Name = amountInstance.modifier.Id,
							Value = amountInstance.value
						});
					}
				}
				MinionResume component = minionIdentity.gameObject.GetComponent<MinionResume>();
				float totalExperienceGained = component.TotalExperienceGained;
				List<string> list3 = new List<string>();
				foreach (KeyValuePair<string, bool> keyValuePair in component.MasteryBySkillID)
				{
					if (keyValuePair.Value)
					{
						list3.Add(keyValuePair.Key);
					}
				}
				list.Add(new SaveLoader.MinionMetricsData
				{
					Name = minionIdentity.name,
					Modifiers = list2,
					TotalExperienceGained = totalExperienceGained,
					Skills = list3
				});
			}
		}
		return list;
	}

	// Token: 0x060080EF RID: 33007 RVA: 0x00343C9C File Offset: 0x00341E9C
	private List<SaveLoader.SavedPrefabMetricsData> GetSavedPrefabMetrics()
	{
		Dictionary<Tag, List<SaveLoadRoot>> lists = this.saveManager.GetLists();
		List<SaveLoader.SavedPrefabMetricsData> list = new List<SaveLoader.SavedPrefabMetricsData>(lists.Count);
		foreach (KeyValuePair<Tag, List<SaveLoadRoot>> keyValuePair in lists)
		{
			Tag key = keyValuePair.Key;
			List<SaveLoadRoot> value = keyValuePair.Value;
			if (value.Count > 0)
			{
				list.Add(new SaveLoader.SavedPrefabMetricsData
				{
					PrefabName = key.ToString(),
					Count = value.Count
				});
			}
		}
		return list;
	}

	// Token: 0x060080F0 RID: 33008 RVA: 0x00343D48 File Offset: 0x00341F48
	private List<SaveLoader.WorldInventoryMetricsData> GetWorldInventoryMetrics()
	{
		Dictionary<Tag, float> allWorldsAccessibleAmounts = ClusterManager.Instance.GetAllWorldsAccessibleAmounts();
		List<SaveLoader.WorldInventoryMetricsData> list = new List<SaveLoader.WorldInventoryMetricsData>(allWorldsAccessibleAmounts.Count);
		foreach (KeyValuePair<Tag, float> keyValuePair in allWorldsAccessibleAmounts)
		{
			float value = keyValuePair.Value;
			if (!float.IsInfinity(value) && !float.IsNaN(value))
			{
				list.Add(new SaveLoader.WorldInventoryMetricsData
				{
					Name = keyValuePair.Key.ToString(),
					Amount = value
				});
			}
		}
		return list;
	}

	// Token: 0x060080F1 RID: 33009 RVA: 0x00343DF4 File Offset: 0x00341FF4
	private List<SaveLoader.DailyReportMetricsData> GetDailyReportMetrics()
	{
		List<SaveLoader.DailyReportMetricsData> list = new List<SaveLoader.DailyReportMetricsData>();
		int cycle = GameClock.Instance.GetCycle();
		ReportManager.DailyReport dailyReport = ReportManager.Instance.FindReport(cycle);
		if (dailyReport != null)
		{
			foreach (ReportManager.ReportEntry reportEntry in dailyReport.reportEntries)
			{
				SaveLoader.DailyReportMetricsData item = default(SaveLoader.DailyReportMetricsData);
				item.Name = reportEntry.reportType.ToString();
				if (!float.IsInfinity(reportEntry.Net) && !float.IsNaN(reportEntry.Net))
				{
					item.Net = new float?(reportEntry.Net);
				}
				if (SaveLoader.force_infinity)
				{
					item.Net = null;
				}
				if (!float.IsInfinity(reportEntry.Positive) && !float.IsNaN(reportEntry.Positive))
				{
					item.Positive = new float?(reportEntry.Positive);
				}
				if (!float.IsInfinity(reportEntry.Negative) && !float.IsNaN(reportEntry.Negative))
				{
					item.Negative = new float?(reportEntry.Negative);
				}
				list.Add(item);
			}
			list.Add(new SaveLoader.DailyReportMetricsData
			{
				Name = "MinionCount",
				Net = new float?((float)Components.LiveMinionIdentities.Count),
				Positive = new float?(0f),
				Negative = new float?(0f)
			});
		}
		return list;
	}

	// Token: 0x060080F2 RID: 33010 RVA: 0x00343F8C File Offset: 0x0034218C
	private List<SaveLoader.PerformanceMeasurement> GetPerformanceMeasurements()
	{
		List<SaveLoader.PerformanceMeasurement> list = new List<SaveLoader.PerformanceMeasurement>();
		if (Global.Instance != null)
		{
			PerformanceMonitor component = Global.Instance.GetComponent<PerformanceMonitor>();
			list.Add(new SaveLoader.PerformanceMeasurement
			{
				name = "FramesAbove30",
				value = component.NumFramesAbove30
			});
			list.Add(new SaveLoader.PerformanceMeasurement
			{
				name = "FramesBelow30",
				value = component.NumFramesBelow30
			});
			component.Reset();
		}
		return list;
	}

	// Token: 0x060080F3 RID: 33011 RVA: 0x00344014 File Offset: 0x00342214
	private float GetFrameTime()
	{
		PerformanceMonitor component = Global.Instance.GetComponent<PerformanceMonitor>();
		DebugUtil.LogArgs(new object[]
		{
			"Average frame time:",
			1f / component.FPS
		});
		return 1f / component.FPS;
	}

	// Token: 0x060080F4 RID: 33012 RVA: 0x00344060 File Offset: 0x00342260
	private List<SaveLoader.WorldMetricsData> GetWorldMetrics()
	{
		List<SaveLoader.WorldMetricsData> list = new List<SaveLoader.WorldMetricsData>();
		if (Global.Instance != null)
		{
			foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
			{
				if (!worldContainer.IsModuleInterior)
				{
					float discoveryTimestamp = worldContainer.IsDiscovered ? worldContainer.DiscoveryTimestamp : -1f;
					float dupeVisitedTimestamp = worldContainer.IsDupeVisited ? worldContainer.DupeVisitedTimestamp : -1f;
					list.Add(new SaveLoader.WorldMetricsData
					{
						Name = worldContainer.worldName,
						DiscoveryTimestamp = discoveryTimestamp,
						DupeVisitedTimestamp = dupeVisitedTimestamp
					});
				}
			}
		}
		return list;
	}

	// Token: 0x060080F5 RID: 33013 RVA: 0x000F9707 File Offset: 0x000F7907
	[Obsolete("Use Game.IsDlcActiveForCurrentSave instead")]
	public bool IsDLCActiveForCurrentSave(string dlcid)
	{
		return DlcManager.IsContentSubscribed(dlcid) && (dlcid == "" || dlcid == "" || this.GameInfo.dlcIds.Contains(dlcid));
	}

	// Token: 0x060080F6 RID: 33014 RVA: 0x0034412C File Offset: 0x0034232C
	[Obsolete("Use Game methods instead")]
	public bool IsDlcListActiveForCurrentSave(string[] dlcIds)
	{
		if (dlcIds == null || dlcIds.Length == 0)
		{
			return true;
		}
		foreach (string text in dlcIds)
		{
			if (text == "")
			{
				return true;
			}
			if (Game.IsDlcActiveForCurrentSave(text))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060080F7 RID: 33015 RVA: 0x00344170 File Offset: 0x00342370
	[Obsolete("Use Game methods instead")]
	public bool IsAllDlcActiveForCurrentSave(string[] dlcIds)
	{
		if (dlcIds == null || dlcIds.Length == 0)
		{
			return true;
		}
		foreach (string text in dlcIds)
		{
			if (!(text == "") && !Game.IsDlcActiveForCurrentSave(text))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060080F8 RID: 33016 RVA: 0x003441B4 File Offset: 0x003423B4
	[Obsolete("Use Game methods instead")]
	public bool IsAnyDlcActiveForCurrentSave(string[] dlcIds)
	{
		if (dlcIds == null || dlcIds.Length == 0)
		{
			return false;
		}
		foreach (string text in dlcIds)
		{
			if (!(text == "") && Game.IsDlcActiveForCurrentSave(text))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060080F9 RID: 33017 RVA: 0x000F9740 File Offset: 0x000F7940
	[Obsolete("Use Game's version")]
	public bool IsCorrectDlcActiveForCurrentSave(string[] required, string[] forbidden)
	{
		return this.IsAllDlcActiveForCurrentSave(required) && !this.IsAnyDlcActiveForCurrentSave(forbidden);
	}

	// Token: 0x060080FA RID: 33018 RVA: 0x003441F8 File Offset: 0x003423F8
	public string GetSaveLoadContentLetters()
	{
		if (this.GameInfo.dlcIds.Count <= 0)
		{
			return "V";
		}
		string text = "";
		foreach (string dlcId in this.GameInfo.dlcIds)
		{
			text += DlcManager.GetContentLetter(dlcId);
		}
		return text;
	}

	// Token: 0x060080FB RID: 33019 RVA: 0x00344278 File Offset: 0x00342478
	public void UpgradeActiveSaveDLCInfo(string dlcId, bool trigger_load = false)
	{
		string activeSaveFolder = SaveLoader.GetActiveSaveFolder();
		string path = SaveGame.Instance.BaseName + UI.FRONTEND.OPTIONS_SCREEN.TOGGLE_SANDBOX_SCREEN.BACKUP_SAVE_GAME_APPEND + ".sav";
		string filename = System.IO.Path.Combine(activeSaveFolder, path);
		this.Save(filename, false, false);
		if (!this.GameInfo.dlcIds.Contains(dlcId))
		{
			this.GameInfo.dlcIds.Add(dlcId);
		}
		string current_save = SaveLoader.GetActiveSaveFilePath();
		this.Save(SaveLoader.GetActiveSaveFilePath(), false, false);
		if (trigger_load)
		{
			LoadingOverlay.Load(delegate
			{
				LoadScreen.DoLoad(current_save);
			});
		}
	}

	// Token: 0x040061F5 RID: 25077
	[MyCmpGet]
	private GridSettings gridSettings;

	// Token: 0x040061F7 RID: 25079
	private bool saveFileCorrupt;

	// Token: 0x040061F8 RID: 25080
	private bool compressSaveData = true;

	// Token: 0x040061F9 RID: 25081
	private int lastUncompressedSize;

	// Token: 0x040061FA RID: 25082
	public bool saveAsText;

	// Token: 0x040061FB RID: 25083
	public const string MAINMENU_LEVELNAME = "launchscene";

	// Token: 0x040061FC RID: 25084
	public const string FRONTEND_LEVELNAME = "frontend";

	// Token: 0x040061FD RID: 25085
	public const string BACKEND_LEVELNAME = "backend";

	// Token: 0x040061FE RID: 25086
	public const string SAVE_EXTENSION = ".sav";

	// Token: 0x040061FF RID: 25087
	public const string AUTOSAVE_FOLDER = "auto_save";

	// Token: 0x04006200 RID: 25088
	public const string CLOUDSAVE_FOLDER = "cloud_save_files";

	// Token: 0x04006201 RID: 25089
	public const string SAVE_FOLDER = "save_files";

	// Token: 0x04006202 RID: 25090
	public const int MAX_AUTOSAVE_FILES = 10;

	// Token: 0x04006204 RID: 25092
	[NonSerialized]
	public SaveManager saveManager;

	// Token: 0x04006206 RID: 25094
	private Cluster m_cluster;

	// Token: 0x04006207 RID: 25095
	private ClusterLayout m_clusterLayout;

	// Token: 0x04006209 RID: 25097
	private const string CorruptFileSuffix = "_";

	// Token: 0x0400620A RID: 25098
	private const float SAVE_BUFFER_HEAD_ROOM = 0.1f;

	// Token: 0x0400620B RID: 25099
	private bool mustRestartOnFail;

	// Token: 0x0400620E RID: 25102
	public const string METRIC_SAVED_PREFAB_KEY = "SavedPrefabs";

	// Token: 0x0400620F RID: 25103
	public const string METRIC_IS_AUTO_SAVE_KEY = "IsAutoSave";

	// Token: 0x04006210 RID: 25104
	public const string METRIC_WAS_DEBUG_EVER_USED = "WasDebugEverUsed";

	// Token: 0x04006211 RID: 25105
	public const string METRIC_IS_SANDBOX_ENABLED = "IsSandboxEnabled";

	// Token: 0x04006212 RID: 25106
	public const string METRIC_RESOURCES_ACCESSIBLE_KEY = "ResourcesAccessible";

	// Token: 0x04006213 RID: 25107
	public const string METRIC_DAILY_REPORT_KEY = "DailyReport";

	// Token: 0x04006214 RID: 25108
	public const string METRIC_WORLD_METRICS_KEY = "WorldMetrics";

	// Token: 0x04006215 RID: 25109
	public const string METRIC_MINION_METRICS_KEY = "MinionMetrics";

	// Token: 0x04006216 RID: 25110
	public const string METRIC_CUSTOM_GAME_SETTINGS = "CustomGameSettings";

	// Token: 0x04006217 RID: 25111
	public const string METRIC_CUSTOM_MIXING_SETTINGS = "CustomMixingSettings";

	// Token: 0x04006218 RID: 25112
	public const string METRIC_PERFORMANCE_MEASUREMENTS = "PerformanceMeasurements";

	// Token: 0x04006219 RID: 25113
	public const string METRIC_FRAME_TIME = "AverageFrameTime";

	// Token: 0x0400621A RID: 25114
	private static bool force_infinity;

	// Token: 0x02001864 RID: 6244
	public class FlowUtilityNetworkInstance
	{
		// Token: 0x0400621B RID: 25115
		public int id = -1;

		// Token: 0x0400621C RID: 25116
		public SimHashes containedElement = SimHashes.Vacuum;

		// Token: 0x0400621D RID: 25117
		public float containedMass;

		// Token: 0x0400621E RID: 25118
		public float containedTemperature;
	}

	// Token: 0x02001865 RID: 6245
	[SerializationConfig(KSerialization.MemberSerialization.OptOut)]
	public class FlowUtilityNetworkSaver : ISaveLoadable
	{
		// Token: 0x060080FE RID: 33022 RVA: 0x000F9780 File Offset: 0x000F7980
		public FlowUtilityNetworkSaver()
		{
			this.gas = new List<SaveLoader.FlowUtilityNetworkInstance>();
			this.liquid = new List<SaveLoader.FlowUtilityNetworkInstance>();
		}

		// Token: 0x0400621F RID: 25119
		public List<SaveLoader.FlowUtilityNetworkInstance> gas;

		// Token: 0x04006220 RID: 25120
		public List<SaveLoader.FlowUtilityNetworkInstance> liquid;
	}

	// Token: 0x02001866 RID: 6246
	public struct SaveFileEntry
	{
		// Token: 0x04006221 RID: 25121
		public string path;

		// Token: 0x04006222 RID: 25122
		public System.DateTime timeStamp;
	}

	// Token: 0x02001867 RID: 6247
	public enum SaveType
	{
		// Token: 0x04006224 RID: 25124
		local,
		// Token: 0x04006225 RID: 25125
		cloud,
		// Token: 0x04006226 RID: 25126
		both
	}

	// Token: 0x02001868 RID: 6248
	private struct MinionAttrFloatData
	{
		// Token: 0x04006227 RID: 25127
		public string Name;

		// Token: 0x04006228 RID: 25128
		public float Value;
	}

	// Token: 0x02001869 RID: 6249
	private struct MinionMetricsData
	{
		// Token: 0x04006229 RID: 25129
		public string Name;

		// Token: 0x0400622A RID: 25130
		public List<SaveLoader.MinionAttrFloatData> Modifiers;

		// Token: 0x0400622B RID: 25131
		public float TotalExperienceGained;

		// Token: 0x0400622C RID: 25132
		public List<string> Skills;
	}

	// Token: 0x0200186A RID: 6250
	private struct SavedPrefabMetricsData
	{
		// Token: 0x0400622D RID: 25133
		public string PrefabName;

		// Token: 0x0400622E RID: 25134
		public int Count;
	}

	// Token: 0x0200186B RID: 6251
	private struct WorldInventoryMetricsData
	{
		// Token: 0x0400622F RID: 25135
		public string Name;

		// Token: 0x04006230 RID: 25136
		public float Amount;
	}

	// Token: 0x0200186C RID: 6252
	private struct DailyReportMetricsData
	{
		// Token: 0x04006231 RID: 25137
		public string Name;

		// Token: 0x04006232 RID: 25138
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public float? Net;

		// Token: 0x04006233 RID: 25139
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public float? Positive;

		// Token: 0x04006234 RID: 25140
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public float? Negative;
	}

	// Token: 0x0200186D RID: 6253
	private struct PerformanceMeasurement
	{
		// Token: 0x04006235 RID: 25141
		public string name;

		// Token: 0x04006236 RID: 25142
		public float value;
	}

	// Token: 0x0200186E RID: 6254
	private struct WorldMetricsData
	{
		// Token: 0x04006237 RID: 25143
		public string Name;

		// Token: 0x04006238 RID: 25144
		public float DiscoveryTimestamp;

		// Token: 0x04006239 RID: 25145
		public float DupeVisitedTimestamp;
	}
}
