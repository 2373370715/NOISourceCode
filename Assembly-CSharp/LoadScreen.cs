using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using ProcGen;
using ProcGenGame;
using Steamworks;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001B2F RID: 6959
public class LoadScreen : KModalScreen
{
	// Token: 0x170009A5 RID: 2469
	// (get) Token: 0x060091C0 RID: 37312 RVA: 0x00103C3B File Offset: 0x00101E3B
	// (set) Token: 0x060091C1 RID: 37313 RVA: 0x00103C42 File Offset: 0x00101E42
	public static LoadScreen Instance { get; private set; }

	// Token: 0x060091C2 RID: 37314 RVA: 0x00103C4A File Offset: 0x00101E4A
	public static void DestroyInstance()
	{
		LoadScreen.Instance = null;
	}

	// Token: 0x060091C3 RID: 37315 RVA: 0x0038E5C4 File Offset: 0x0038C7C4
	protected override void OnPrefabInit()
	{
		global::Debug.Assert(LoadScreen.Instance == null);
		LoadScreen.Instance = this;
		base.OnPrefabInit();
		this.saveButtonPrefab.gameObject.SetActive(false);
		this.colonyListPool = new UIPool<HierarchyReferences>(this.saveButtonPrefab);
		if (SpeedControlScreen.Instance != null)
		{
			SpeedControlScreen.Instance.Pause(false, false);
		}
		if (this.closeButton != null)
		{
			this.closeButton.onClick += delegate()
			{
				this.Deactivate();
			};
		}
		if (this.colonyCloudButton != null)
		{
			this.colonyCloudButton.onClick += delegate()
			{
				this.ConvertAllToCloud();
			};
		}
		if (this.colonyLocalButton != null)
		{
			this.colonyLocalButton.onClick += delegate()
			{
				this.ConvertAllToLocal();
			};
		}
		if (this.colonyInfoButton != null)
		{
			this.colonyInfoButton.onClick += delegate()
			{
				this.ShowSaveInfo();
			};
		}
		if (this.loadMoreButton != null)
		{
			this.loadMoreButton.onClick += delegate()
			{
				this.displayedPageCount++;
				this.RefreshColonyList();
				this.ShowColonyList();
			};
		}
	}

	// Token: 0x060091C4 RID: 37316 RVA: 0x00103C52 File Offset: 0x00101E52
	private bool IsInMenu()
	{
		return App.GetCurrentSceneName() == "frontend";
	}

	// Token: 0x060091C5 RID: 37317 RVA: 0x00103C63 File Offset: 0x00101E63
	private bool CloudSavesVisible()
	{
		return SaveLoader.GetCloudSavesAvailable() && this.IsInMenu();
	}

	// Token: 0x060091C6 RID: 37318 RVA: 0x0038E6E4 File Offset: 0x0038C8E4
	protected override void OnActivate()
	{
		base.OnActivate();
		WorldGen.LoadSettings(false);
		this.SetCloudSaveInfoActive(this.CloudSavesVisible());
		this.displayedPageCount = 1;
		this.RefreshColonyList();
		this.ShowColonyList();
		bool cloudSavesAvailable = SaveLoader.GetCloudSavesAvailable();
		this.cloudTutorialBouncer.gameObject.SetActive(cloudSavesAvailable);
		if (cloudSavesAvailable && !this.cloudTutorialBouncer.IsBouncing())
		{
			int @int = KPlayerPrefs.GetInt("LoadScreenCloudTutorialTimes", 0);
			if (@int < 5)
			{
				this.cloudTutorialBouncer.Bounce();
				KPlayerPrefs.SetInt("LoadScreenCloudTutorialTimes", @int + 1);
				KPlayerPrefs.GetInt("LoadScreenCloudTutorialTimes", 0);
			}
			else
			{
				this.cloudTutorialBouncer.gameObject.SetActive(false);
			}
		}
		if (DistributionPlatform.Initialized && SteamUtils.IsSteamRunningOnSteamDeck())
		{
			this.colonyInfoButton.gameObject.SetActive(false);
		}
	}

	// Token: 0x060091C7 RID: 37319 RVA: 0x0038E7AC File Offset: 0x0038C9AC
	private Dictionary<string, List<LoadScreen.SaveGameFileDetails>> GetColoniesDetails(List<SaveLoader.SaveFileEntry> files)
	{
		Dictionary<string, List<LoadScreen.SaveGameFileDetails>> dictionary = new Dictionary<string, List<LoadScreen.SaveGameFileDetails>>();
		if (files.Count <= 0)
		{
			return dictionary;
		}
		for (int i = 0; i < files.Count; i++)
		{
			if (this.IsFileValid(files[i].path))
			{
				global::Tuple<SaveGame.Header, SaveGame.GameInfo> fileInfo = SaveGame.GetFileInfo(files[i].path);
				SaveGame.Header first = fileInfo.first;
				SaveGame.GameInfo second = fileInfo.second;
				System.DateTime timeStamp = files[i].timeStamp;
				long size = 0L;
				try
				{
					size = new FileInfo(files[i].path).Length;
				}
				catch (Exception ex)
				{
					global::Debug.LogWarning("Failed to get size for file: " + files[i].ToString() + "\n" + ex.ToString());
				}
				LoadScreen.SaveGameFileDetails saveGameFileDetails = new LoadScreen.SaveGameFileDetails
				{
					BaseName = second.baseName,
					FileName = files[i].path,
					FileDate = timeStamp,
					FileHeader = first,
					FileInfo = second,
					Size = size,
					UniqueID = SaveGame.GetSaveUniqueID(second)
				};
				if (!dictionary.ContainsKey(saveGameFileDetails.UniqueID))
				{
					dictionary.Add(saveGameFileDetails.UniqueID, new List<LoadScreen.SaveGameFileDetails>());
				}
				dictionary[saveGameFileDetails.UniqueID].Add(saveGameFileDetails);
			}
		}
		return dictionary;
	}

	// Token: 0x060091C8 RID: 37320 RVA: 0x0038E914 File Offset: 0x0038CB14
	private Dictionary<string, List<LoadScreen.SaveGameFileDetails>> GetColonies(bool sort)
	{
		List<SaveLoader.SaveFileEntry> allFiles = SaveLoader.GetAllFiles(sort, SaveLoader.SaveType.both);
		return this.GetColoniesDetails(allFiles);
	}

	// Token: 0x060091C9 RID: 37321 RVA: 0x0038E930 File Offset: 0x0038CB30
	private Dictionary<string, List<LoadScreen.SaveGameFileDetails>> GetLocalColonies(bool sort)
	{
		List<SaveLoader.SaveFileEntry> allFiles = SaveLoader.GetAllFiles(sort, SaveLoader.SaveType.local);
		return this.GetColoniesDetails(allFiles);
	}

	// Token: 0x060091CA RID: 37322 RVA: 0x0038E94C File Offset: 0x0038CB4C
	private Dictionary<string, List<LoadScreen.SaveGameFileDetails>> GetCloudColonies(bool sort)
	{
		List<SaveLoader.SaveFileEntry> allFiles = SaveLoader.GetAllFiles(sort, SaveLoader.SaveType.cloud);
		return this.GetColoniesDetails(allFiles);
	}

	// Token: 0x060091CB RID: 37323 RVA: 0x0038E968 File Offset: 0x0038CB68
	private bool IsFileValid(string filename)
	{
		bool result = false;
		try
		{
			SaveGame.Header header;
			result = (SaveLoader.LoadHeader(filename, out header).saveMajorVersion >= 7);
		}
		catch (Exception ex)
		{
			global::Debug.LogWarning("Corrupted save file: " + filename + "\n" + ex.ToString());
		}
		return result;
	}

	// Token: 0x060091CC RID: 37324 RVA: 0x0038E9BC File Offset: 0x0038CBBC
	private void CheckCloudLocalOverlap()
	{
		if (!SaveLoader.GetCloudSavesAvailable())
		{
			return;
		}
		string cloudSavePrefix = SaveLoader.GetCloudSavePrefix();
		if (cloudSavePrefix == null)
		{
			return;
		}
		foreach (KeyValuePair<string, List<LoadScreen.SaveGameFileDetails>> keyValuePair in this.GetColonies(false))
		{
			bool flag = false;
			List<LoadScreen.SaveGameFileDetails> list = new List<LoadScreen.SaveGameFileDetails>();
			foreach (LoadScreen.SaveGameFileDetails saveGameFileDetails in keyValuePair.Value)
			{
				if (SaveLoader.IsSaveCloud(saveGameFileDetails.FileName))
				{
					flag = true;
				}
				else
				{
					list.Add(saveGameFileDetails);
				}
			}
			if (flag && list.Count != 0)
			{
				string baseName = list[0].BaseName;
				string path = System.IO.Path.Combine(SaveLoader.GetSavePrefix(), baseName);
				string text = System.IO.Path.Combine(cloudSavePrefix, baseName);
				if (!Directory.Exists(text))
				{
					Directory.CreateDirectory(text);
				}
				global::Debug.Log("Saves / Found overlapped cloud/local saves for colony '" + baseName + "', moving to cloud...");
				foreach (LoadScreen.SaveGameFileDetails saveGameFileDetails2 in list)
				{
					string fileName = saveGameFileDetails2.FileName;
					string source = System.IO.Path.ChangeExtension(fileName, "png");
					string path2 = text;
					if (SaveLoader.IsSaveAuto(fileName))
					{
						string text2 = System.IO.Path.Combine(path2, "auto_save");
						if (!Directory.Exists(text2))
						{
							Directory.CreateDirectory(text2);
						}
						path2 = text2;
					}
					string text3 = System.IO.Path.Combine(path2, System.IO.Path.GetFileName(fileName));
					global::Tuple<bool, bool> tuple;
					if (this.FileMatch(fileName, text3, out tuple))
					{
						global::Debug.Log("Saves / file match found for `" + fileName + "`...");
						this.MigrateFile(fileName, text3, false);
						string dest = System.IO.Path.ChangeExtension(text3, "png");
						this.MigrateFile(source, dest, true);
					}
					else
					{
						global::Debug.Log("Saves / no file match found for `" + fileName + "`... move as copy");
						string nextUsableSavePath = SaveLoader.GetNextUsableSavePath(text3);
						this.MigrateFile(fileName, nextUsableSavePath, false);
						string dest2 = System.IO.Path.ChangeExtension(nextUsableSavePath, "png");
						this.MigrateFile(source, dest2, true);
					}
				}
				this.RemoveEmptyFolder(path);
			}
		}
	}

	// Token: 0x060091CD RID: 37325 RVA: 0x00103C74 File Offset: 0x00101E74
	private void DeleteFileAndEmptyFolder(string file)
	{
		if (File.Exists(file))
		{
			File.Delete(file);
		}
		this.RemoveEmptyFolder(System.IO.Path.GetDirectoryName(file));
	}

	// Token: 0x060091CE RID: 37326 RVA: 0x0038EC34 File Offset: 0x0038CE34
	private void RemoveEmptyFolder(string path)
	{
		if (!Directory.Exists(path))
		{
			return;
		}
		if (!File.GetAttributes(path).HasFlag(FileAttributes.Directory))
		{
			return;
		}
		if (Directory.EnumerateFileSystemEntries(path).Any<string>())
		{
			return;
		}
		try
		{
			Directory.Delete(path);
		}
		catch (Exception obj)
		{
			global::Debug.LogWarning("Failed to remove empty directory `" + path + "`...");
			global::Debug.LogWarning(obj);
		}
	}

	// Token: 0x060091CF RID: 37327 RVA: 0x0038ECA8 File Offset: 0x0038CEA8
	private void RefreshColonyList()
	{
		if (this.colonyListPool != null)
		{
			this.colonyListPool.ClearAll();
		}
		this.CheckCloudLocalOverlap();
		Dictionary<string, List<LoadScreen.SaveGameFileDetails>> colonies = this.GetColonies(true);
		if (colonies.Count > 0)
		{
			int num = 0;
			foreach (KeyValuePair<string, List<LoadScreen.SaveGameFileDetails>> keyValuePair in colonies)
			{
				if (num >= this.displayedPageCount * 20)
				{
					break;
				}
				this.AddColonyToList(keyValuePair.Value);
				num++;
			}
			this.loadMoreButton.gameObject.SetActive(colonies.Count != num);
			this.loadMoreButton.gameObject.transform.SetAsLastSibling();
		}
	}

	// Token: 0x060091D0 RID: 37328 RVA: 0x0038ED6C File Offset: 0x0038CF6C
	private string GetFileHash(string path)
	{
		string result;
		using (MD5 md = MD5.Create())
		{
			using (FileStream fileStream = File.OpenRead(path))
			{
				result = BitConverter.ToString(md.ComputeHash(fileStream)).Replace("-", "").ToLowerInvariant();
			}
		}
		return result;
	}

	// Token: 0x060091D1 RID: 37329 RVA: 0x0038EDDC File Offset: 0x0038CFDC
	private bool FileMatch(string file, string other_file, out global::Tuple<bool, bool> matches)
	{
		matches = new global::Tuple<bool, bool>(false, false);
		if (!File.Exists(file))
		{
			return false;
		}
		if (!File.Exists(other_file))
		{
			return false;
		}
		bool flag = false;
		bool flag2 = false;
		try
		{
			string fileHash = this.GetFileHash(file);
			string fileHash2 = this.GetFileHash(other_file);
			FileInfo fileInfo = new FileInfo(file);
			FileInfo fileInfo2 = new FileInfo(other_file);
			flag = (fileInfo.Length == fileInfo2.Length);
			flag2 = (fileHash == fileHash2);
		}
		catch (Exception obj)
		{
			global::Debug.LogWarning(string.Concat(new string[]
			{
				"FileMatch / file match failed for `",
				file,
				"` vs `",
				other_file,
				"`!"
			}));
			global::Debug.LogWarning(obj);
			return false;
		}
		matches.first = flag;
		matches.second = flag2;
		return flag && flag2;
	}

	// Token: 0x060091D2 RID: 37330 RVA: 0x0038EEA4 File Offset: 0x0038D0A4
	private bool MigrateFile(string source, string dest, bool ignoreMissing = false)
	{
		global::Debug.Log(string.Concat(new string[]
		{
			"Migration / moving `",
			source,
			"` to `",
			dest,
			"` ..."
		}));
		if (dest == source)
		{
			global::Debug.Log(string.Concat(new string[]
			{
				"Migration / ignored `",
				source,
				"` to `",
				dest,
				"` ... same location"
			}));
			return true;
		}
		global::Tuple<bool, bool> tuple;
		if (this.FileMatch(source, dest, out tuple))
		{
			global::Debug.Log("Migration / dest and source are identical size + hash ... removing original");
			try
			{
				this.DeleteFileAndEmptyFolder(source);
			}
			catch (Exception ex)
			{
				global::Debug.LogWarning("Migration / removing original failed for `" + source + "`!");
				global::Debug.LogWarning(ex);
				throw ex;
			}
			return true;
		}
		try
		{
			global::Debug.Log("Migration / copying...");
			File.Copy(source, dest, false);
		}
		catch (FileNotFoundException obj) when (ignoreMissing)
		{
			global::Debug.Log("Migration / File `" + source + "` wasn't found but we're ignoring that.");
			return true;
		}
		catch (Exception ex2)
		{
			global::Debug.LogWarning("Migration / copy failed for `" + source + "`! Leaving it alone");
			global::Debug.LogWarning(ex2);
			global::Debug.LogWarning("failed to convert colony: " + ex2.ToString());
			throw ex2;
		}
		global::Debug.Log("Migration / copy ok ...");
		global::Tuple<bool, bool> tuple2;
		if (!this.FileMatch(source, dest, out tuple2))
		{
			global::Debug.LogWarning("Migration / failed to match dest file for `" + source + "`!");
			global::Debug.LogWarning(string.Format("Migration / did hash match? {0} did size match? {1}", tuple2.second, tuple2.first));
			throw new Exception("Hash/Size didn't match for source and destination");
		}
		global::Debug.Log("Migration / hash validation ok ... removing original");
		try
		{
			this.DeleteFileAndEmptyFolder(source);
		}
		catch (Exception ex3)
		{
			global::Debug.LogWarning("Migration / removing original failed for `" + source + "`!");
			global::Debug.LogWarning(ex3);
			throw ex3;
		}
		global::Debug.Log("Migration / moved ok for `" + source + "`!");
		return true;
	}

	// Token: 0x060091D3 RID: 37331 RVA: 0x0038F0A8 File Offset: 0x0038D2A8
	private bool MigrateSave(string dest_root, string file, bool is_auto_save, out string saveError)
	{
		saveError = null;
		global::Tuple<SaveGame.Header, SaveGame.GameInfo> fileInfo = SaveGame.GetFileInfo(file);
		SaveGame.Header first = fileInfo.first;
		string path = fileInfo.second.baseName.TrimEnd(' ');
		string fileName = System.IO.Path.GetFileName(file);
		string text = System.IO.Path.Combine(dest_root, path);
		if (!Directory.Exists(text))
		{
			text = Directory.CreateDirectory(text).FullName;
		}
		string path2 = text;
		if (is_auto_save)
		{
			string text2 = System.IO.Path.Combine(text, "auto_save");
			if (!Directory.Exists(text2))
			{
				Directory.CreateDirectory(text2);
			}
			path2 = text2;
		}
		string text3 = System.IO.Path.Combine(path2, fileName);
		string source = System.IO.Path.ChangeExtension(file, "png");
		string dest = System.IO.Path.ChangeExtension(text3, "png");
		try
		{
			this.MigrateFile(file, text3, false);
			this.MigrateFile(source, dest, true);
		}
		catch (Exception ex)
		{
			saveError = ex.Message;
			return false;
		}
		return true;
	}

	// Token: 0x060091D4 RID: 37332 RVA: 0x0038F184 File Offset: 0x0038D384
	private ValueTuple<int, int, ulong> GetSavesSizeAndCounts(List<LoadScreen.SaveGameFileDetails> list)
	{
		ulong num = 0UL;
		int num2 = 0;
		int num3 = 0;
		for (int i = 0; i < list.Count; i++)
		{
			LoadScreen.SaveGameFileDetails saveGameFileDetails = list[i];
			num += (ulong)saveGameFileDetails.Size;
			if (saveGameFileDetails.FileInfo.isAutoSave)
			{
				num3++;
			}
			else
			{
				num2++;
			}
		}
		return new ValueTuple<int, int, ulong>(num2, num3, num);
	}

	// Token: 0x060091D5 RID: 37333 RVA: 0x0038F1DC File Offset: 0x0038D3DC
	private int CountValidSaves(string path, SearchOption searchType = SearchOption.AllDirectories)
	{
		int num = 0;
		List<SaveLoader.SaveFileEntry> saveFiles = SaveLoader.GetSaveFiles(path, false, searchType);
		for (int i = 0; i < saveFiles.Count; i++)
		{
			if (this.IsFileValid(saveFiles[i].path))
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x060091D6 RID: 37334 RVA: 0x0038F220 File Offset: 0x0038D420
	private ValueTuple<int, int> GetMigrationSaveCounts()
	{
		int item = this.CountValidSaves(SaveLoader.GetSavePrefixAndCreateFolder(), SearchOption.TopDirectoryOnly);
		int item2 = this.CountValidSaves(SaveLoader.GetAutoSavePrefix(), SearchOption.AllDirectories);
		return new ValueTuple<int, int>(item, item2);
	}

	// Token: 0x060091D7 RID: 37335 RVA: 0x0038F24C File Offset: 0x0038D44C
	private ValueTuple<int, int> MigrateSaves(out string errorColony, out string errorMessage)
	{
		errorColony = null;
		errorMessage = null;
		int num = 0;
		string savePrefixAndCreateFolder = SaveLoader.GetSavePrefixAndCreateFolder();
		List<SaveLoader.SaveFileEntry> saveFiles = SaveLoader.GetSaveFiles(savePrefixAndCreateFolder, false, SearchOption.TopDirectoryOnly);
		for (int i = 0; i < saveFiles.Count; i++)
		{
			SaveLoader.SaveFileEntry saveFileEntry = saveFiles[i];
			if (this.IsFileValid(saveFileEntry.path))
			{
				string text;
				if (this.MigrateSave(savePrefixAndCreateFolder, saveFileEntry.path, false, out text))
				{
					num++;
				}
				else if (errorColony == null)
				{
					errorColony = saveFileEntry.path;
					errorMessage = text;
				}
			}
		}
		int num2 = 0;
		List<SaveLoader.SaveFileEntry> saveFiles2 = SaveLoader.GetSaveFiles(SaveLoader.GetAutoSavePrefix(), false, SearchOption.AllDirectories);
		for (int j = 0; j < saveFiles2.Count; j++)
		{
			SaveLoader.SaveFileEntry saveFileEntry2 = saveFiles2[j];
			if (this.IsFileValid(saveFileEntry2.path))
			{
				string text2;
				if (this.MigrateSave(savePrefixAndCreateFolder, saveFileEntry2.path, true, out text2))
				{
					num2++;
				}
				else if (errorColony == null)
				{
					errorColony = saveFileEntry2.path;
					errorMessage = text2;
				}
			}
		}
		return new ValueTuple<int, int>(num, num2);
	}

	// Token: 0x060091D8 RID: 37336 RVA: 0x0038F33C File Offset: 0x0038D53C
	public void ShowMigrationIfNecessary(bool fromMainMenu)
	{
		ValueTuple<int, int> migrationSaveCounts = this.GetMigrationSaveCounts();
		int saveCount = migrationSaveCounts.Item1;
		int autoCount = migrationSaveCounts.Item2;
		if (saveCount == 0 && autoCount == 0)
		{
			if (fromMainMenu)
			{
				this.Deactivate();
			}
			return;
		}
		base.Activate();
		this.migrationPanelRefs.gameObject.SetActive(true);
		KButton migrateButton = this.migrationPanelRefs.GetReference<RectTransform>("MigrateSaves").GetComponent<KButton>();
		KButton continueButton = this.migrationPanelRefs.GetReference<RectTransform>("Continue").GetComponent<KButton>();
		KButton moreInfoButton = this.migrationPanelRefs.GetReference<RectTransform>("MoreInfo").GetComponent<KButton>();
		KButton component = this.migrationPanelRefs.GetReference<RectTransform>("OpenSaves").GetComponent<KButton>();
		LocText statsText = this.migrationPanelRefs.GetReference<RectTransform>("CountText").GetComponent<LocText>();
		LocText infoText = this.migrationPanelRefs.GetReference<RectTransform>("InfoText").GetComponent<LocText>();
		migrateButton.gameObject.SetActive(true);
		continueButton.gameObject.SetActive(false);
		moreInfoButton.gameObject.SetActive(false);
		statsText.text = string.Format(UI.FRONTEND.LOADSCREEN.MIGRATE_COUNT, saveCount, autoCount);
		component.ClearOnClick();
		component.onClick += delegate()
		{
			App.OpenWebURL(SaveLoader.GetSavePrefixAndCreateFolder());
		};
		migrateButton.ClearOnClick();
		migrateButton.onClick += delegate()
		{
			migrateButton.gameObject.SetActive(false);
			string text;
			string newValue;
			ValueTuple<int, int> valueTuple = this.MigrateSaves(out text, out newValue);
			int item = valueTuple.Item1;
			int item2 = valueTuple.Item2;
			bool flag = text == null;
			string format = flag ? UI.FRONTEND.LOADSCREEN.MIGRATE_RESULT.text : UI.FRONTEND.LOADSCREEN.MIGRATE_RESULT_FAILURES.Replace("{ErrorColony}", text).Replace("{ErrorMessage}", newValue);
			statsText.text = string.Format(format, new object[]
			{
				item,
				saveCount,
				item2,
				autoCount
			});
			infoText.gameObject.SetActive(false);
			if (flag)
			{
				continueButton.gameObject.SetActive(true);
			}
			else
			{
				moreInfoButton.gameObject.SetActive(true);
			}
			MainMenu.Instance.RefreshResumeButton(false);
			this.RefreshColonyList();
		};
		continueButton.ClearOnClick();
		continueButton.onClick += delegate()
		{
			this.migrationPanelRefs.gameObject.SetActive(false);
			this.cloudTutorialBouncer.Bounce();
		};
		moreInfoButton.ClearOnClick();
		Action<InfoDialogScreen> <>9__4;
		Action<InfoDialogScreen> <>9__6;
		moreInfoButton.onClick += delegate()
		{
			if (DistributionPlatform.Initialized && SteamUtils.IsSteamRunningOnSteamDeck())
			{
				InfoDialogScreen infoDialogScreen = global::Util.KInstantiateUI<InfoDialogScreen>(ScreenPrefabs.Instance.InfoDialogScreen.gameObject, this.gameObject, false).SetHeader(UI.FRONTEND.LOADSCREEN.MIGRATE_RESULT_FAILURES_MORE_INFO_TITLE).AddPlainText(UI.FRONTEND.LOADSCREEN.MIGRATE_RESULT_FAILURES_MORE_INFO_PRE).AddLineItem(UI.FRONTEND.LOADSCREEN.MIGRATE_RESULT_FAILURES_MORE_INFO_ITEM1, "").AddLineItem(UI.FRONTEND.LOADSCREEN.MIGRATE_RESULT_FAILURES_MORE_INFO_ITEM2, "").AddLineItem(UI.FRONTEND.LOADSCREEN.MIGRATE_RESULT_FAILURES_MORE_INFO_ITEM3, "").AddPlainText(UI.FRONTEND.LOADSCREEN.MIGRATE_RESULT_FAILURES_MORE_INFO_POST);
				string text = UI.CONFIRMDIALOG.OK;
				Action<InfoDialogScreen> action;
				if ((action = <>9__4) == null)
				{
					action = (<>9__4 = delegate(InfoDialogScreen d)
					{
						this.migrationPanelRefs.gameObject.SetActive(false);
						this.cloudTutorialBouncer.Bounce();
						d.Deactivate();
					});
				}
				infoDialogScreen.AddOption(text, action, true).Activate();
				return;
			}
			InfoDialogScreen infoDialogScreen2 = global::Util.KInstantiateUI<InfoDialogScreen>(ScreenPrefabs.Instance.InfoDialogScreen.gameObject, this.gameObject, false).SetHeader(UI.FRONTEND.LOADSCREEN.MIGRATE_RESULT_FAILURES_MORE_INFO_TITLE).AddPlainText(UI.FRONTEND.LOADSCREEN.MIGRATE_RESULT_FAILURES_MORE_INFO_PRE).AddLineItem(UI.FRONTEND.LOADSCREEN.MIGRATE_RESULT_FAILURES_MORE_INFO_ITEM1, "").AddLineItem(UI.FRONTEND.LOADSCREEN.MIGRATE_RESULT_FAILURES_MORE_INFO_ITEM2, "").AddLineItem(UI.FRONTEND.LOADSCREEN.MIGRATE_RESULT_FAILURES_MORE_INFO_ITEM3, "").AddPlainText(UI.FRONTEND.LOADSCREEN.MIGRATE_RESULT_FAILURES_MORE_INFO_POST).AddOption(UI.FRONTEND.LOADSCREEN.MIGRATE_FAILURES_FORUM_BUTTON, delegate(InfoDialogScreen d)
			{
				App.OpenWebURL("https://forums.kleientertainment.com/klei-bug-tracker/oni/");
			}, false);
			string text2 = UI.CONFIRMDIALOG.OK;
			Action<InfoDialogScreen> action2;
			if ((action2 = <>9__6) == null)
			{
				action2 = (<>9__6 = delegate(InfoDialogScreen d)
				{
					this.migrationPanelRefs.gameObject.SetActive(false);
					this.cloudTutorialBouncer.Bounce();
					d.Deactivate();
				});
			}
			infoDialogScreen2.AddOption(text2, action2, true).Activate();
		};
	}

	// Token: 0x060091D9 RID: 37337 RVA: 0x00103C90 File Offset: 0x00101E90
	private void SetCloudSaveInfoActive(bool active)
	{
		this.colonyCloudButton.gameObject.SetActive(active);
		this.colonyLocalButton.gameObject.SetActive(active);
	}

	// Token: 0x060091DA RID: 37338 RVA: 0x0038F538 File Offset: 0x0038D738
	private bool ConvertToLocalOrCloud(string fromRoot, string destRoot, string colonyName)
	{
		string text = System.IO.Path.Combine(fromRoot, colonyName);
		string text2 = System.IO.Path.Combine(destRoot, colonyName);
		global::Debug.Log(string.Concat(new string[]
		{
			"Convert / Colony '",
			colonyName,
			"' from `",
			text,
			"` => `",
			text2,
			"`"
		}));
		try
		{
			Directory.Move(text, text2);
			return true;
		}
		catch (Exception ex)
		{
			global::Debug.LogWarning("failed to convert colony: " + ex.ToString());
			string message = UI.FRONTEND.LOADSCREEN.CONVERT_ERROR.Replace("{Colony}", colonyName).Replace("{Error}", ex.Message);
			this.ShowConvertError(message);
		}
		return false;
	}

	// Token: 0x060091DB RID: 37339 RVA: 0x0038F5F4 File Offset: 0x0038D7F4
	private bool ConvertColonyToCloud(string colonyName)
	{
		string savePrefix = SaveLoader.GetSavePrefix();
		string cloudSavePrefix = SaveLoader.GetCloudSavePrefix();
		if (cloudSavePrefix == null)
		{
			global::Debug.LogWarning("Failed to move colony to cloud, no cloud save prefix found (usually a userID is missing, not logged in?)");
			return false;
		}
		return this.ConvertToLocalOrCloud(savePrefix, cloudSavePrefix, colonyName);
	}

	// Token: 0x060091DC RID: 37340 RVA: 0x0038F628 File Offset: 0x0038D828
	private bool ConvertColonyToLocal(string colonyName)
	{
		string savePrefix = SaveLoader.GetSavePrefix();
		string cloudSavePrefix = SaveLoader.GetCloudSavePrefix();
		if (cloudSavePrefix == null)
		{
			global::Debug.LogWarning("Failed to move colony from cloud, no cloud save prefix found (usually a userID is missing, not logged in?)");
			return false;
		}
		return this.ConvertToLocalOrCloud(cloudSavePrefix, savePrefix, colonyName);
	}

	// Token: 0x060091DD RID: 37341 RVA: 0x0038F65C File Offset: 0x0038D85C
	private void DoConvertAllToLocal()
	{
		Dictionary<string, List<LoadScreen.SaveGameFileDetails>> cloudColonies = this.GetCloudColonies(false);
		if (cloudColonies.Count == 0)
		{
			return;
		}
		bool flag = true;
		foreach (KeyValuePair<string, List<LoadScreen.SaveGameFileDetails>> keyValuePair in cloudColonies)
		{
			flag &= this.ConvertColonyToLocal(keyValuePair.Value[0].BaseName);
		}
		if (flag)
		{
			string replacement = UI.PLATFORMS.STEAM;
			this.ShowSimpleDialog(UI.FRONTEND.LOADSCREEN.CONVERT_TO_LOCAL, UI.FRONTEND.LOADSCREEN.CONVERT_ALL_TO_LOCAL_SUCCESS.Replace("{Client}", replacement));
		}
		this.RefreshColonyList();
		MainMenu.Instance.RefreshResumeButton(false);
		SaveLoader.SetCloudSavesDefault(false);
	}

	// Token: 0x060091DE RID: 37342 RVA: 0x0038F718 File Offset: 0x0038D918
	private void DoConvertAllToCloud()
	{
		Dictionary<string, List<LoadScreen.SaveGameFileDetails>> localColonies = this.GetLocalColonies(false);
		if (localColonies.Count == 0)
		{
			return;
		}
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, List<LoadScreen.SaveGameFileDetails>> keyValuePair in localColonies)
		{
			string baseName = keyValuePair.Value[0].BaseName;
			if (!list.Contains(baseName))
			{
				list.Add(baseName);
			}
		}
		bool flag = true;
		foreach (string colonyName in list)
		{
			flag &= this.ConvertColonyToCloud(colonyName);
		}
		if (flag)
		{
			string replacement = UI.PLATFORMS.STEAM;
			this.ShowSimpleDialog(UI.FRONTEND.LOADSCREEN.CONVERT_TO_CLOUD, UI.FRONTEND.LOADSCREEN.CONVERT_ALL_TO_CLOUD_SUCCESS.Replace("{Client}", replacement));
		}
		this.RefreshColonyList();
		MainMenu.Instance.RefreshResumeButton(false);
		SaveLoader.SetCloudSavesDefault(true);
	}

	// Token: 0x060091DF RID: 37343 RVA: 0x0038F82C File Offset: 0x0038DA2C
	private void ConvertAllToCloud()
	{
		string message = string.Format("{0}\n{1}\n", UI.FRONTEND.LOADSCREEN.CONVERT_TO_CLOUD_DETAILS, UI.FRONTEND.LOADSCREEN.CONVERT_ALL_WARNING);
		KPlayerPrefs.SetInt("LoadScreenCloudTutorialTimes", 5);
		this.ConfirmCloudSaveMigrations(message, UI.FRONTEND.LOADSCREEN.CONVERT_TO_CLOUD, UI.FRONTEND.LOADSCREEN.CONVERT_ALL_COLONIES, UI.FRONTEND.LOADSCREEN.OPEN_SAVE_FOLDER, delegate
		{
			this.DoConvertAllToCloud();
		}, delegate
		{
			App.OpenWebURL(SaveLoader.GetSavePrefix());
		}, this.localToCloudSprite);
	}

	// Token: 0x060091E0 RID: 37344 RVA: 0x0038F8B0 File Offset: 0x0038DAB0
	private void ConvertAllToLocal()
	{
		string message = string.Format("{0}\n{1}\n", UI.FRONTEND.LOADSCREEN.CONVERT_TO_LOCAL_DETAILS, UI.FRONTEND.LOADSCREEN.CONVERT_ALL_WARNING);
		KPlayerPrefs.SetInt("LoadScreenCloudTutorialTimes", 5);
		this.ConfirmCloudSaveMigrations(message, UI.FRONTEND.LOADSCREEN.CONVERT_TO_LOCAL, UI.FRONTEND.LOADSCREEN.CONVERT_ALL_COLONIES, UI.FRONTEND.LOADSCREEN.OPEN_SAVE_FOLDER, delegate
		{
			this.DoConvertAllToLocal();
		}, delegate
		{
			App.OpenWebURL(SaveLoader.GetCloudSavePrefix());
		}, this.cloudToLocalSprite);
	}

	// Token: 0x060091E1 RID: 37345 RVA: 0x0038F934 File Offset: 0x0038DB34
	private void ShowSaveInfo()
	{
		if (this.infoScreen == null)
		{
			this.infoScreen = global::Util.KInstantiateUI<InfoDialogScreen>(ScreenPrefabs.Instance.InfoDialogScreen.gameObject, base.gameObject, false).SetHeader(UI.FRONTEND.LOADSCREEN.SAVE_INFO_DIALOG_TITLE).AddSprite(this.infoSprite).AddPlainText(UI.FRONTEND.LOADSCREEN.SAVE_INFO_DIALOG_TEXT).AddOption(UI.FRONTEND.LOADSCREEN.OPEN_SAVE_FOLDER, delegate(InfoDialogScreen d)
			{
				App.OpenWebURL(SaveLoader.GetSavePrefix());
			}, true).AddDefaultCancel();
			string cloudRoot = SaveLoader.GetCloudSavePrefix();
			if (cloudRoot != null && this.CloudSavesVisible())
			{
				this.infoScreen.AddOption(UI.FRONTEND.LOADSCREEN.OPEN_CLOUDSAVE_FOLDER, delegate(InfoDialogScreen d)
				{
					App.OpenWebURL(cloudRoot);
				}, true);
			}
			this.infoScreen.gameObject.SetActive(true);
		}
	}

	// Token: 0x060091E2 RID: 37346 RVA: 0x00103CB4 File Offset: 0x00101EB4
	protected override void OnDeactivate()
	{
		if (SpeedControlScreen.Instance != null)
		{
			SpeedControlScreen.Instance.Unpause(false);
		}
		this.selectedSave = null;
		base.OnDeactivate();
	}

	// Token: 0x060091E3 RID: 37347 RVA: 0x00103CDB File Offset: 0x00101EDB
	private void ShowColonyList()
	{
		this.colonyListRoot.SetActive(true);
		this.colonyViewRoot.SetActive(false);
		this.currentColony = null;
		this.selectedSave = null;
	}

	// Token: 0x060091E4 RID: 37348 RVA: 0x0038FA28 File Offset: 0x0038DC28
	private bool CheckSaveVersion(LoadScreen.SaveGameFileDetails save, LocText display)
	{
		if (LoadScreen.IsSaveFileFromUnsupportedFutureBuild(save.FileHeader, save.FileInfo))
		{
			if (display != null)
			{
				display.text = string.Format(UI.FRONTEND.LOADSCREEN.SAVE_TOO_NEW, new object[]
				{
					save.FileName,
					save.FileHeader.buildVersion,
					save.FileInfo.saveMinorVersion,
					663500U,
					35
				});
			}
			return false;
		}
		if (save.FileInfo.saveMajorVersion < 7)
		{
			if (display != null)
			{
				display.text = string.Format(UI.FRONTEND.LOADSCREEN.UNSUPPORTED_SAVE_VERSION, new object[]
				{
					save.FileName,
					save.FileInfo.saveMajorVersion,
					save.FileInfo.saveMinorVersion,
					7,
					35
				});
			}
			return false;
		}
		return true;
	}

	// Token: 0x060091E5 RID: 37349 RVA: 0x0038FB2C File Offset: 0x0038DD2C
	private bool CheckSaveDLCsCompatable(LoadScreen.SaveGameFileDetails save)
	{
		HashSet<string> hashSet;
		HashSet<string> hashSet2;
		return save.FileInfo.IsCompatableWithCurrentDlcConfiguration(out hashSet, out hashSet2);
	}

	// Token: 0x060091E6 RID: 37350 RVA: 0x0038FB4C File Offset: 0x0038DD4C
	private string GetSaveDLCIncompatabilityTooltip(LoadScreen.SaveGameFileDetails save)
	{
		string text = "";
		HashSet<string> hashSet;
		HashSet<string> hashSet2;
		if (save.FileInfo.IsCompatableWithCurrentDlcConfiguration(out hashSet, out hashSet2))
		{
			text = null;
		}
		else
		{
			text = UI.FRONTEND.LOADSCREEN.TOOLTIP_SAVE_INCOMPATABLE_DLC_CONFIGURATION;
			foreach (string dlcId in hashSet)
			{
				text = text + "\n" + string.Format(UI.FRONTEND.LOADSCREEN.TOOLTIP_SAVE_INCOMPATABLE_DLC_CONFIGURATION_ASK_TO_ENABLE, DlcManager.GetDlcTitle(dlcId));
			}
			foreach (string dlcId2 in hashSet2)
			{
				text = text + "\n" + string.Format(UI.FRONTEND.LOADSCREEN.TOOLTIP_SAVE_INCOMPATABLE_DLC_CONFIGURATION_ASK_TO_DISABLE, DlcManager.GetDlcTitle(dlcId2));
			}
		}
		return text;
	}

	// Token: 0x060091E7 RID: 37351 RVA: 0x0038FC3C File Offset: 0x0038DE3C
	private void ShowColonySave(LoadScreen.SaveGameFileDetails save)
	{
		HierarchyReferences component = this.colonyViewRoot.GetComponent<HierarchyReferences>();
		component.GetReference<RectTransform>("Title").GetComponent<LocText>().text = save.BaseName;
		component.GetReference<RectTransform>("Date").GetComponent<LocText>().text = string.Format("{0:H:mm:ss} - " + Localization.GetFileDateFormat(0), save.FileDate.ToLocalTime());
		string text = save.FileInfo.clusterId;
		if (text != null && !SettingsCache.clusterLayouts.clusterCache.ContainsKey(text))
		{
			string text2 = SettingsCache.GetScope("EXPANSION1_ID") + text;
			if (SettingsCache.clusterLayouts.clusterCache.ContainsKey(text2))
			{
				text = text2;
			}
			else
			{
				DebugUtil.LogWarningArgs(new object[]
				{
					"Failed to find cluster " + text + " including the scoped path, setting to default cluster name."
				});
				global::Debug.Log("ClusterCache: " + string.Join(",", SettingsCache.clusterLayouts.clusterCache.Keys));
				text = WorldGenSettings.ClusterDefaultName;
			}
		}
		ProcGen.World world = (text != null) ? SettingsCache.clusterLayouts.GetWorldData(text, 0) : null;
		string arg = (world != null) ? Strings.Get(world.name) : " - ";
		component.GetReference<LocText>("InfoWorld").text = string.Format(UI.FRONTEND.LOADSCREEN.COLONY_INFO_FMT, UI.FRONTEND.LOADSCREEN.WORLD_NAME, arg);
		component.GetReference<LocText>("InfoCycles").text = string.Format(UI.FRONTEND.LOADSCREEN.COLONY_INFO_FMT, UI.FRONTEND.LOADSCREEN.CYCLES_SURVIVED, save.FileInfo.numberOfCycles);
		component.GetReference<LocText>("InfoDupes").text = string.Format(UI.FRONTEND.LOADSCREEN.COLONY_INFO_FMT, UI.FRONTEND.LOADSCREEN.DUPLICANTS_ALIVE, save.FileInfo.numberOfDuplicants);
		TMP_Text component2 = component.GetReference<RectTransform>("FileSize").GetComponent<LocText>();
		string formattedBytes = GameUtil.GetFormattedBytes((ulong)save.Size);
		component2.text = string.Format(UI.FRONTEND.LOADSCREEN.COLONY_FILE_SIZE, formattedBytes);
		component.GetReference<RectTransform>("Filename").GetComponent<LocText>().text = string.Format(UI.FRONTEND.LOADSCREEN.COLONY_FILE_NAME, System.IO.Path.GetFileName(save.FileName));
		LocText component3 = component.GetReference<RectTransform>("AutoInfo").GetComponent<LocText>();
		component3.gameObject.SetActive(!this.CheckSaveVersion(save, component3));
		Image component4 = component.GetReference<RectTransform>("Preview").GetComponent<Image>();
		this.SetPreview(save.FileName, save.BaseName, component4, false);
		KButton component5 = component.GetReference<RectTransform>("DeleteButton").GetComponent<KButton>();
		component5.ClearOnClick();
		System.Action <>9__1;
		component5.onClick += delegate()
		{
			LoadScreen <>4__this = this;
			System.Action onDelete;
			if ((onDelete = <>9__1) == null)
			{
				onDelete = (<>9__1 = delegate()
				{
					int num = this.currentColony.IndexOf(save);
					this.currentColony.Remove(save);
					this.ShowColony(this.currentColony, num - 1);
				});
			}
			<>4__this.Delete(onDelete);
		};
	}

	// Token: 0x060091E8 RID: 37352 RVA: 0x0038FF24 File Offset: 0x0038E124
	private void ShowColony(List<LoadScreen.SaveGameFileDetails> saves, int selectIndex = -1)
	{
		if (saves.Count <= 0)
		{
			this.RefreshColonyList();
			this.ShowColonyList();
			return;
		}
		this.currentColony = saves;
		this.colonyListRoot.SetActive(false);
		this.colonyViewRoot.SetActive(true);
		string baseName = saves[0].BaseName;
		HierarchyReferences component = this.colonyViewRoot.GetComponent<HierarchyReferences>();
		KButton component2 = component.GetReference<RectTransform>("Back").GetComponent<KButton>();
		component2.ClearOnClick();
		component2.onClick += delegate()
		{
			this.ShowColonyList();
		};
		component.GetReference<RectTransform>("ColonyTitle").GetComponent<LocText>().text = string.Format(UI.FRONTEND.LOADSCREEN.COLONY_TITLE, baseName);
		GameObject gameObject = component.GetReference<RectTransform>("Content").gameObject;
		RectTransform reference = component.GetReference<RectTransform>("SaveTemplate");
		for (int i = 0; i < gameObject.transform.childCount; i++)
		{
			GameObject gameObject2 = gameObject.transform.GetChild(i).gameObject;
			if (gameObject2 != null && gameObject2.name.Contains("Clone"))
			{
				UnityEngine.Object.Destroy(gameObject2);
			}
		}
		if (selectIndex < 0)
		{
			selectIndex = 0;
		}
		if (selectIndex > saves.Count - 1)
		{
			selectIndex = saves.Count - 1;
		}
		for (int j = 0; j < saves.Count; j++)
		{
			LoadScreen.SaveGameFileDetails save = saves[j];
			RectTransform rectTransform = UnityEngine.Object.Instantiate<RectTransform>(reference, gameObject.transform);
			HierarchyReferences component3 = rectTransform.GetComponent<HierarchyReferences>();
			rectTransform.gameObject.SetActive(true);
			component3.GetReference<RectTransform>("AutoLabel").gameObject.SetActive(save.FileInfo.isAutoSave);
			component3.GetReference<RectTransform>("SaveText").GetComponent<LocText>().text = System.IO.Path.GetFileNameWithoutExtension(save.FileName);
			component3.GetReference<RectTransform>("DateText").GetComponent<LocText>().text = string.Format("{0:H:mm:ss} - " + Localization.GetFileDateFormat(0), save.FileDate.ToLocalTime());
			component3.GetReference<RectTransform>("NewestLabel").gameObject.SetActive(j == 0);
			RectTransform reference2 = component3.GetReference<RectTransform>("DLCIconPrefab");
			foreach (string dlcId in save.FileInfo.dlcIds)
			{
				GameObject gameObject3 = global::Util.KInstantiateUI(reference2.gameObject, reference2.transform.parent.gameObject, true);
				gameObject3.GetComponent<Image>().sprite = Assets.GetSprite(DlcManager.GetDlcSmallLogo(dlcId));
				gameObject3.GetComponent<ToolTip>().SetSimpleTooltip(string.Format(UI.FRONTEND.LOADSCREEN.TOOLTIP_SAVE_USES_DLC, DlcManager.GetDlcTitle(dlcId)));
			}
			object obj = this.CheckSaveVersion(save, null) && this.CheckSaveDLCsCompatable(save);
			KButton button = rectTransform.GetComponent<KButton>();
			button.ClearOnClick();
			button.onClick += delegate()
			{
				this.UpdateSelected(button, save.FileName, save.FileInfo.dlcIds);
				this.ShowColonySave(save);
			};
			object obj2 = obj;
			if (obj2 != null)
			{
				button.onDoubleClick += delegate()
				{
					this.UpdateSelected(button, save.FileName, save.FileInfo.dlcIds);
					this.Load();
				};
			}
			KButton component4 = component3.GetReference<RectTransform>("LoadButton").GetComponent<KButton>();
			component4.ClearOnClick();
			if (obj2 == null)
			{
				component4.isInteractable = false;
				component4.GetComponent<ImageToggleState>().SetState(ImageToggleState.State.Disabled);
				component4.GetComponent<ToolTip>().SetSimpleTooltip(this.GetSaveDLCIncompatabilityTooltip(save));
			}
			else
			{
				component4.onClick += delegate()
				{
					this.UpdateSelected(button, save.FileName, save.FileInfo.dlcIds);
					this.Load();
				};
			}
			if (j == selectIndex)
			{
				this.UpdateSelected(button, save.FileName, save.FileInfo.dlcIds);
				this.ShowColonySave(save);
			}
		}
	}

	// Token: 0x060091E9 RID: 37353 RVA: 0x00390318 File Offset: 0x0038E518
	private void AddColonyToList(List<LoadScreen.SaveGameFileDetails> saves)
	{
		if (saves.Count == 0)
		{
			return;
		}
		HierarchyReferences freeElement = this.colonyListPool.GetFreeElement(this.saveButtonRoot, true);
		saves.Sort((LoadScreen.SaveGameFileDetails x, LoadScreen.SaveGameFileDetails y) => y.FileDate.CompareTo(x.FileDate));
		LoadScreen.SaveGameFileDetails saveGameFileDetails = saves[0];
		string colonyName = saveGameFileDetails.BaseName;
		ValueTuple<int, int, ulong> savesSizeAndCounts = this.GetSavesSizeAndCounts(saves);
		int item = savesSizeAndCounts.Item1;
		int item2 = savesSizeAndCounts.Item2;
		string formattedBytes = GameUtil.GetFormattedBytes(savesSizeAndCounts.Item3);
		freeElement.GetReference<RectTransform>("HeaderTitle").GetComponent<LocText>().text = colonyName;
		freeElement.GetReference<RectTransform>("HeaderDate").GetComponent<LocText>().text = string.Format("{0:H:mm:ss} - " + Localization.GetFileDateFormat(0), saveGameFileDetails.FileDate.ToLocalTime());
		freeElement.GetReference<RectTransform>("SaveTitle").GetComponent<LocText>().text = string.Format(UI.FRONTEND.LOADSCREEN.SAVE_INFO, item, item2, formattedBytes);
		Image component = freeElement.GetReference<RectTransform>("Preview").GetComponent<Image>();
		this.SetPreview(saveGameFileDetails.FileName, colonyName, component, true);
		List<ValueTuple<Sprite, string>> list = new List<ValueTuple<Sprite, string>>();
		if (saveGameFileDetails.FileInfo.dlcIds.Contains("EXPANSION1_ID"))
		{
			list.Add(new ValueTuple<Sprite, string>(Assets.GetSprite(DlcManager.GetDlcSmallLogo("EXPANSION1_ID")), string.Format(UI.FRONTEND.LOADSCREEN.TOOLTIP_SAVE_USES_DLC, DlcManager.GetDlcTitle("EXPANSION1_ID"))));
		}
		foreach (string text in saveGameFileDetails.FileInfo.dlcIds)
		{
			if (DlcManager.IsDlcId(text) && !(text == "EXPANSION1_ID"))
			{
				list.Add(new ValueTuple<Sprite, string>(Assets.GetSprite(DlcManager.GetDlcSmallLogo(text)), string.Format(UI.FRONTEND.LOADSCREEN.TOOLTIP_SAVE_USES_DLC, DlcManager.GetDlcTitle(text))));
			}
		}
		GameObject gameObject = freeElement.transform.Find("Header").Find("DlcIcons").Find("Prefab_DlcIcon").gameObject;
		gameObject.SetActive(false);
		for (int i = 0; i < gameObject.transform.parent.childCount; i++)
		{
			GameObject gameObject2 = gameObject.transform.parent.GetChild(i).gameObject;
			if (gameObject2 != gameObject)
			{
				UnityEngine.Object.Destroy(gameObject2);
			}
		}
		foreach (ValueTuple<Sprite, string> valueTuple in list)
		{
			GameObject gameObject3 = global::Util.KInstantiateUI(gameObject, gameObject.transform.parent.gameObject, true);
			Image component2 = gameObject3.GetComponent<Image>();
			ToolTip component3 = gameObject3.GetComponent<ToolTip>();
			component2.sprite = valueTuple.Item1;
			component3.SetSimpleTooltip(valueTuple.Item2);
		}
		Component reference = freeElement.GetReference<RectTransform>("LocationIcons");
		bool flag = this.CloudSavesVisible();
		reference.gameObject.SetActive(flag);
		if (flag)
		{
			LocText locationText = freeElement.GetReference<RectTransform>("LocationText").GetComponent<LocText>();
			bool isLocal = SaveLoader.IsSaveLocal(saveGameFileDetails.FileName);
			locationText.text = (isLocal ? UI.FRONTEND.LOADSCREEN.LOCAL_SAVE : UI.FRONTEND.LOADSCREEN.CLOUD_SAVE);
			KButton cloudButton = freeElement.GetReference<RectTransform>("CloudButton").GetComponent<KButton>();
			KButton localButton = freeElement.GetReference<RectTransform>("LocalButton").GetComponent<KButton>();
			cloudButton.gameObject.SetActive(!isLocal);
			cloudButton.ClearOnClick();
			System.Action <>9__4;
			cloudButton.onClick += delegate()
			{
				string text2 = string.Format("{0}\n", UI.FRONTEND.LOADSCREEN.CONVERT_TO_LOCAL_DETAILS);
				LoadScreen <>4__this = this;
				string message = text2;
				string title = UI.FRONTEND.LOADSCREEN.CONVERT_TO_LOCAL;
				string confirmText = UI.FRONTEND.LOADSCREEN.CONVERT_COLONY;
				string backupText = null;
				System.Action commitAction;
				if ((commitAction = <>9__4) == null)
				{
					commitAction = (<>9__4 = delegate()
					{
						cloudButton.gameObject.SetActive(false);
						isLocal = true;
						locationText.text = (isLocal ? UI.FRONTEND.LOADSCREEN.LOCAL_SAVE : UI.FRONTEND.LOADSCREEN.CLOUD_SAVE);
						this.ConvertColonyToLocal(colonyName);
						this.RefreshColonyList();
						MainMenu.Instance.RefreshResumeButton(false);
					});
				}
				<>4__this.ConfirmCloudSaveMigrations(message, title, confirmText, backupText, commitAction, null, this.cloudToLocalSprite);
			};
			localButton.gameObject.SetActive(isLocal);
			localButton.ClearOnClick();
			System.Action <>9__5;
			localButton.onClick += delegate()
			{
				string text2 = string.Format("{0}\n", UI.FRONTEND.LOADSCREEN.CONVERT_TO_CLOUD_DETAILS);
				LoadScreen <>4__this = this;
				string message = text2;
				string title = UI.FRONTEND.LOADSCREEN.CONVERT_TO_CLOUD;
				string confirmText = UI.FRONTEND.LOADSCREEN.CONVERT_COLONY;
				string backupText = null;
				System.Action commitAction;
				if ((commitAction = <>9__5) == null)
				{
					commitAction = (<>9__5 = delegate()
					{
						localButton.gameObject.SetActive(false);
						isLocal = false;
						locationText.text = (isLocal ? UI.FRONTEND.LOADSCREEN.LOCAL_SAVE : UI.FRONTEND.LOADSCREEN.CLOUD_SAVE);
						this.ConvertColonyToCloud(colonyName);
						this.RefreshColonyList();
						MainMenu.Instance.RefreshResumeButton(false);
					});
				}
				<>4__this.ConfirmCloudSaveMigrations(message, title, confirmText, backupText, commitAction, null, this.localToCloudSprite);
			};
		}
		this.GetSaveDLCIncompatabilityTooltip(saveGameFileDetails);
		freeElement.GetReference<RectTransform>("Button").GetComponent<KButton>().onClick += delegate()
		{
			this.ShowColony(saves, -1);
		};
		freeElement.transform.SetAsLastSibling();
	}

	// Token: 0x060091EA RID: 37354 RVA: 0x003907B8 File Offset: 0x0038E9B8
	private void SetPreview(string filename, string basename, Image preview, bool fallbackToTimelapse = false)
	{
		preview.color = Color.black;
		preview.gameObject.SetActive(false);
		try
		{
			Sprite sprite = RetireColonyUtility.LoadColonyPreview(filename, basename, fallbackToTimelapse);
			if (!(sprite == null))
			{
				Rect rect = preview.rectTransform.parent.rectTransform().rect;
				preview.sprite = sprite;
				preview.color = (sprite ? Color.white : Color.black);
				float num = sprite.bounds.size.x / sprite.bounds.size.y;
				if ((double)num >= 1.77777777777778)
				{
					preview.rectTransform.sizeDelta = new Vector2(rect.height * num, rect.height);
				}
				else
				{
					preview.rectTransform.sizeDelta = new Vector2(rect.width, rect.width / num);
				}
				preview.gameObject.SetActive(true);
			}
		}
		catch (Exception obj)
		{
			global::Debug.Log(obj);
		}
	}

	// Token: 0x060091EB RID: 37355 RVA: 0x00103D03 File Offset: 0x00101F03
	public static void ForceStopGame()
	{
		ThreadedHttps<KleiMetrics>.Instance.ClearGameFields();
		ThreadedHttps<KleiMetrics>.Instance.SendProfileStats();
		Game.Instance.SetIsLoading();
		Grid.CellCount = 0;
		Sim.Shutdown();
	}

	// Token: 0x060091EC RID: 37356 RVA: 0x00103D2F File Offset: 0x00101F2F
	private static bool IsSaveFileFromUnsupportedFutureBuild(SaveGame.Header header, SaveGame.GameInfo gameInfo)
	{
		return gameInfo.saveMajorVersion > 7 || (gameInfo.saveMajorVersion == 7 && gameInfo.saveMinorVersion > 35) || header.buildVersion > 663500U;
	}

	// Token: 0x060091ED RID: 37357 RVA: 0x003908C8 File Offset: 0x0038EAC8
	private void UpdateSelected(KButton button, string filename, List<string> dlcIds)
	{
		if (this.selectedSave != null && this.selectedSave.button != null)
		{
			this.selectedSave.button.GetComponent<ImageToggleState>().SetState(ImageToggleState.State.Inactive);
		}
		if (this.selectedSave == null)
		{
			this.selectedSave = new LoadScreen.SelectedSave();
		}
		this.selectedSave.button = button;
		this.selectedSave.filename = filename;
		this.selectedSave.dlcIds = dlcIds;
		if (this.selectedSave.button != null)
		{
			this.selectedSave.button.GetComponent<ImageToggleState>().SetState(ImageToggleState.State.Active);
		}
	}

	// Token: 0x060091EE RID: 37358 RVA: 0x00390968 File Offset: 0x0038EB68
	private void Load()
	{
		if (!DlcManager.IsAllContentSubscribed(this.selectedSave.dlcIds))
		{
			string message = this.selectedSave.dlcIds.Contains("") ? UI.FRONTEND.LOADSCREEN.VANILLA_RESTART : UI.FRONTEND.LOADSCREEN.EXPANSION1_RESTART;
			this.ConfirmDoAction(message, delegate
			{
				KPlayerPrefs.SetString("AutoResumeSaveFile", this.selectedSave.filename);
				DlcManager.ToggleDLC("EXPANSION1_ID");
			});
			return;
		}
		LoadingOverlay.Load(new System.Action(this.DoLoad));
	}

	// Token: 0x060091EF RID: 37359 RVA: 0x00103D5C File Offset: 0x00101F5C
	private void DoLoad()
	{
		if (this.selectedSave == null)
		{
			return;
		}
		LoadScreen.DoLoad(this.selectedSave.filename);
		this.Deactivate();
	}

	// Token: 0x060091F0 RID: 37360 RVA: 0x003909D8 File Offset: 0x0038EBD8
	public static void DoLoad(string filename)
	{
		KCrashReporter.MOST_RECENT_SAVEFILE = filename;
		bool flag = true;
		SaveGame.Header header;
		SaveGame.GameInfo gameInfo = SaveLoader.LoadHeader(filename, out header);
		string arg = null;
		string arg2 = null;
		if (header.buildVersion > 663500U)
		{
			arg = header.buildVersion.ToString();
			arg2 = 663500U.ToString();
		}
		else if (gameInfo.saveMajorVersion < 7)
		{
			arg = string.Format("v{0}.{1}", gameInfo.saveMajorVersion, gameInfo.saveMinorVersion);
			arg2 = string.Format("v{0}.{1}", 7, 35);
		}
		if (!flag)
		{
			GameObject parent = (FrontEndManager.Instance == null) ? GameScreenManager.Instance.ssOverlayCanvas : FrontEndManager.Instance.gameObject;
			global::Util.KInstantiateUI(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, parent, true).GetComponent<ConfirmDialogScreen>().PopupConfirmDialog(string.Format(UI.CRASHSCREEN.LOADFAILED, "Version Mismatch", arg, arg2), null, null, null, null, null, null, null, null);
			return;
		}
		if (Game.Instance != null)
		{
			LoadScreen.ForceStopGame();
		}
		SaveLoader.SetActiveSaveFilePath(filename);
		Time.timeScale = 0f;
		App.LoadScene("backend");
	}

	// Token: 0x060091F1 RID: 37361 RVA: 0x00103D7D File Offset: 0x00101F7D
	private void MoreInfo()
	{
		App.OpenWebURL("http://support.kleientertainment.com/customer/portal/articles/2776550");
	}

	// Token: 0x060091F2 RID: 37362 RVA: 0x00390AFC File Offset: 0x0038ECFC
	private void Delete(System.Action onDelete)
	{
		if (this.selectedSave == null || string.IsNullOrEmpty(this.selectedSave.filename))
		{
			global::Debug.LogError("The path provided is not valid and cannot be deleted.");
			return;
		}
		this.ConfirmDoAction(string.Format(UI.FRONTEND.LOADSCREEN.CONFIRMDELETE, System.IO.Path.GetFileName(this.selectedSave.filename)), delegate
		{
			try
			{
				this.DeleteFileAndEmptyFolder(this.selectedSave.filename);
				string file = System.IO.Path.ChangeExtension(this.selectedSave.filename, "png");
				this.DeleteFileAndEmptyFolder(file);
				if (onDelete != null)
				{
					onDelete();
				}
				MainMenu.Instance.RefreshResumeButton(false);
			}
			catch (SystemException ex)
			{
				global::Debug.LogError(ex.ToString());
			}
		});
	}

	// Token: 0x060091F3 RID: 37363 RVA: 0x00103D89 File Offset: 0x00101F89
	private void ShowSimpleDialog(string title, string message)
	{
		global::Util.KInstantiateUI<InfoDialogScreen>(ScreenPrefabs.Instance.InfoDialogScreen.gameObject, base.gameObject, false).SetHeader(title).AddPlainText(message).AddDefaultOK(false).Activate();
	}

	// Token: 0x060091F4 RID: 37364 RVA: 0x00390B74 File Offset: 0x0038ED74
	private void ConfirmCloudSaveMigrations(string message, string title, string confirmText, string backupText, System.Action commitAction, System.Action backupAction, Sprite sprite)
	{
		global::Util.KInstantiateUI<InfoDialogScreen>(ScreenPrefabs.Instance.InfoDialogScreen.gameObject, base.gameObject, false).SetHeader(title).AddSprite(sprite).AddPlainText(message).AddDefaultCancel().AddOption(confirmText, delegate(InfoDialogScreen d)
		{
			d.Deactivate();
			commitAction();
		}, true).Activate();
	}

	// Token: 0x060091F5 RID: 37365 RVA: 0x00390BDC File Offset: 0x0038EDDC
	private void ShowConvertError(string message)
	{
		if (this.errorInfoScreen == null)
		{
			if (DistributionPlatform.Initialized && SteamUtils.IsSteamRunningOnSteamDeck())
			{
				this.errorInfoScreen = global::Util.KInstantiateUI<InfoDialogScreen>(ScreenPrefabs.Instance.InfoDialogScreen.gameObject, base.gameObject, false).SetHeader(UI.FRONTEND.LOADSCREEN.CONVERT_ERROR_TITLE).AddSprite(this.errorSprite).AddPlainText(message).AddDefaultOK(false);
				this.errorInfoScreen.Activate();
				return;
			}
			this.errorInfoScreen = global::Util.KInstantiateUI<InfoDialogScreen>(ScreenPrefabs.Instance.InfoDialogScreen.gameObject, base.gameObject, false).SetHeader(UI.FRONTEND.LOADSCREEN.CONVERT_ERROR_TITLE).AddSprite(this.errorSprite).AddPlainText(message).AddOption(UI.FRONTEND.LOADSCREEN.MIGRATE_FAILURES_FORUM_BUTTON, delegate(InfoDialogScreen d)
			{
				App.OpenWebURL("https://forums.kleientertainment.com/klei-bug-tracker/oni/");
			}, false).AddDefaultOK(false);
			this.errorInfoScreen.Activate();
		}
	}

	// Token: 0x060091F6 RID: 37366 RVA: 0x00390CDC File Offset: 0x0038EEDC
	private void ConfirmDoAction(string message, System.Action action)
	{
		if (this.confirmScreen == null)
		{
			this.confirmScreen = global::Util.KInstantiateUI<ConfirmDialogScreen>(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, base.gameObject, false);
			this.confirmScreen.PopupConfirmDialog(message, action, delegate
			{
			}, null, null, null, null, null, null);
			this.confirmScreen.gameObject.SetActive(true);
		}
	}

	// Token: 0x060091F7 RID: 37367 RVA: 0x00103DBD File Offset: 0x00101FBD
	public override void OnKeyDown(KButtonEvent e)
	{
		if (this.currentColony != null && e.TryConsume(global::Action.Escape))
		{
			this.ShowColonyList();
		}
		base.OnKeyDown(e);
	}

	// Token: 0x04006E6C RID: 28268
	private const int MAX_CLOUD_TUTORIALS = 5;

	// Token: 0x04006E6D RID: 28269
	private const string CLOUD_TUTORIAL_KEY = "LoadScreenCloudTutorialTimes";

	// Token: 0x04006E6E RID: 28270
	private const int ITEMS_PER_PAGE = 20;

	// Token: 0x04006E6F RID: 28271
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04006E70 RID: 28272
	[SerializeField]
	private GameObject saveButtonRoot;

	// Token: 0x04006E71 RID: 28273
	[SerializeField]
	private GameObject colonyListRoot;

	// Token: 0x04006E72 RID: 28274
	[SerializeField]
	private GameObject colonyViewRoot;

	// Token: 0x04006E73 RID: 28275
	[SerializeField]
	private HierarchyReferences migrationPanelRefs;

	// Token: 0x04006E74 RID: 28276
	[SerializeField]
	private HierarchyReferences saveButtonPrefab;

	// Token: 0x04006E75 RID: 28277
	[SerializeField]
	private KButton loadMoreButton;

	// Token: 0x04006E76 RID: 28278
	[Space]
	[SerializeField]
	private KButton colonyCloudButton;

	// Token: 0x04006E77 RID: 28279
	[SerializeField]
	private KButton colonyLocalButton;

	// Token: 0x04006E78 RID: 28280
	[SerializeField]
	private KButton colonyInfoButton;

	// Token: 0x04006E79 RID: 28281
	[SerializeField]
	private Sprite localToCloudSprite;

	// Token: 0x04006E7A RID: 28282
	[SerializeField]
	private Sprite cloudToLocalSprite;

	// Token: 0x04006E7B RID: 28283
	[SerializeField]
	private Sprite errorSprite;

	// Token: 0x04006E7C RID: 28284
	[SerializeField]
	private Sprite infoSprite;

	// Token: 0x04006E7D RID: 28285
	[SerializeField]
	private Bouncer cloudTutorialBouncer;

	// Token: 0x04006E7E RID: 28286
	public bool requireConfirmation = true;

	// Token: 0x04006E7F RID: 28287
	private LoadScreen.SelectedSave selectedSave;

	// Token: 0x04006E80 RID: 28288
	private List<LoadScreen.SaveGameFileDetails> currentColony;

	// Token: 0x04006E81 RID: 28289
	private UIPool<HierarchyReferences> colonyListPool;

	// Token: 0x04006E82 RID: 28290
	private ConfirmDialogScreen confirmScreen;

	// Token: 0x04006E83 RID: 28291
	private InfoDialogScreen infoScreen;

	// Token: 0x04006E84 RID: 28292
	private InfoDialogScreen errorInfoScreen;

	// Token: 0x04006E85 RID: 28293
	private ConfirmDialogScreen errorScreen;

	// Token: 0x04006E86 RID: 28294
	private InspectSaveScreen inspectScreenInstance;

	// Token: 0x04006E87 RID: 28295
	private int displayedPageCount = 1;

	// Token: 0x02001B30 RID: 6960
	private struct SaveGameFileDetails
	{
		// Token: 0x04006E88 RID: 28296
		public string BaseName;

		// Token: 0x04006E89 RID: 28297
		public string FileName;

		// Token: 0x04006E8A RID: 28298
		public string UniqueID;

		// Token: 0x04006E8B RID: 28299
		public System.DateTime FileDate;

		// Token: 0x04006E8C RID: 28300
		public SaveGame.Header FileHeader;

		// Token: 0x04006E8D RID: 28301
		public SaveGame.GameInfo FileInfo;

		// Token: 0x04006E8E RID: 28302
		public long Size;
	}

	// Token: 0x02001B31 RID: 6961
	private class SelectedSave
	{
		// Token: 0x04006E8F RID: 28303
		public string filename;

		// Token: 0x04006E90 RID: 28304
		public List<string> dlcIds;

		// Token: 0x04006E91 RID: 28305
		public KButton button;
	}
}
