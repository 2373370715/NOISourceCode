using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using ProcGen;
using STRINGS;
using UnityEngine;

// Token: 0x02001A4E RID: 6734
[AddComponentMenu("KMonoBehaviour/scripts/Unlocks")]
public class Unlocks : KMonoBehaviour
{
	// Token: 0x17000925 RID: 2341
	// (get) Token: 0x06008C4E RID: 35918 RVA: 0x00100498 File Offset: 0x000FE698
	private static string UnlocksFilename
	{
		get
		{
			return System.IO.Path.Combine(global::Util.RootFolder(), "unlocks.json");
		}
	}

	// Token: 0x06008C4F RID: 35919 RVA: 0x001004A9 File Offset: 0x000FE6A9
	protected override void OnPrefabInit()
	{
		this.LoadUnlocks();
	}

	// Token: 0x06008C50 RID: 35920 RVA: 0x00371194 File Offset: 0x0036F394
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.UnlockCycleCodexes();
		GameClock.Instance.Subscribe(631075836, new Action<object>(this.OnNewDay));
		base.Subscribe<Unlocks>(-1277991738, Unlocks.OnLaunchRocketDelegate);
		base.Subscribe<Unlocks>(282337316, Unlocks.OnDuplicantDiedDelegate);
		base.Subscribe<Unlocks>(-818188514, Unlocks.OnDiscoveredSpaceDelegate);
		Components.LiveMinionIdentities.OnAdd += this.OnNewDupe;
	}

	// Token: 0x06008C51 RID: 35921 RVA: 0x001004B1 File Offset: 0x000FE6B1
	public bool IsUnlocked(string unlockID)
	{
		return !string.IsNullOrEmpty(unlockID) && (DebugHandler.InstantBuildMode || this.unlocked.Contains(unlockID));
	}

	// Token: 0x06008C52 RID: 35922 RVA: 0x001004D2 File Offset: 0x000FE6D2
	public IReadOnlyList<string> GetAllUnlockedIds()
	{
		return this.unlocked;
	}

	// Token: 0x06008C53 RID: 35923 RVA: 0x001004DA File Offset: 0x000FE6DA
	public void Lock(string unlockID)
	{
		if (this.unlocked.Contains(unlockID))
		{
			this.unlocked.Remove(unlockID);
			this.SaveUnlocks();
			Game.Instance.Trigger(1594320620, unlockID);
		}
	}

	// Token: 0x06008C54 RID: 35924 RVA: 0x00371214 File Offset: 0x0036F414
	public void Unlock(string unlockID, bool shouldTryShowCodexNotification = true)
	{
		if (string.IsNullOrEmpty(unlockID))
		{
			DebugUtil.DevAssert(false, "Unlock called with null or empty string", null);
			return;
		}
		if (!this.unlocked.Contains(unlockID))
		{
			this.unlocked.Add(unlockID);
			this.SaveUnlocks();
			Game.Instance.Trigger(1594320620, unlockID);
			if (shouldTryShowCodexNotification)
			{
				MessageNotification messageNotification = this.GenerateCodexUnlockNotification(unlockID);
				if (messageNotification != null)
				{
					base.GetComponent<Notifier>().Add(messageNotification, "");
				}
			}
		}
		this.EvalMetaCategories();
	}

	// Token: 0x06008C55 RID: 35925 RVA: 0x0037128C File Offset: 0x0036F48C
	private void EvalMetaCategories()
	{
		foreach (Unlocks.MetaUnlockCategory metaUnlockCategory in this.MetaUnlockCategories)
		{
			string metaCollectionID = metaUnlockCategory.metaCollectionID;
			Unlocks.<>c__DisplayClass14_0 CS$<>8__locals1;
			CS$<>8__locals1.mesaCollectionID = metaUnlockCategory.mesaCollectionID;
			int mesaUnlockCount = metaUnlockCategory.mesaUnlockCount;
			CS$<>8__locals1.count = 0;
			CS$<>8__locals1.isCollectionReplaced = false;
			if (SaveLoader.Instance != null)
			{
				foreach (LoreCollectionOverride loreUnlock in SaveLoader.Instance.ClusterLayout.clusterUnlocks)
				{
					if (this.<EvalMetaCategories>g__EvaluateCollection|14_0(loreUnlock, ref CS$<>8__locals1))
					{
						break;
					}
				}
				foreach (string name in CustomGameSettings.Instance.GetCurrentDlcMixingIds())
				{
					DlcMixingSettings cachedDlcMixingSettings = SettingsCache.GetCachedDlcMixingSettings(name);
					if (cachedDlcMixingSettings != null)
					{
						foreach (LoreCollectionOverride loreUnlock2 in cachedDlcMixingSettings.globalLoreUnlocks)
						{
							if (this.<EvalMetaCategories>g__EvaluateCollection|14_0(loreUnlock2, ref CS$<>8__locals1))
							{
								break;
							}
						}
					}
				}
			}
			if (!CS$<>8__locals1.isCollectionReplaced)
			{
				foreach (string unlockID in this.lockCollections[CS$<>8__locals1.mesaCollectionID])
				{
					if (this.IsUnlocked(unlockID))
					{
						int count = CS$<>8__locals1.count;
						CS$<>8__locals1.count = count + 1;
					}
				}
			}
			if (CS$<>8__locals1.count >= mesaUnlockCount)
			{
				this.UnlockNext(metaCollectionID, false);
			}
		}
	}

	// Token: 0x06008C56 RID: 35926 RVA: 0x0037149C File Offset: 0x0036F69C
	private void SaveUnlocks()
	{
		if (!Directory.Exists(global::Util.RootFolder()))
		{
			Directory.CreateDirectory(global::Util.RootFolder());
		}
		string s = JsonConvert.SerializeObject(this.unlocked);
		bool flag = false;
		int num = 0;
		while (!flag && num < 5)
		{
			try
			{
				Thread.Sleep(num * 100);
				using (FileStream fileStream = File.Open(Unlocks.UnlocksFilename, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
				{
					flag = true;
					byte[] bytes = new ASCIIEncoding().GetBytes(s);
					fileStream.Write(bytes, 0, bytes.Length);
				}
			}
			catch (Exception ex)
			{
				global::Debug.LogWarningFormat("Failed to save Unlocks attempt {0}: {1}", new object[]
				{
					num + 1,
					ex.ToString()
				});
			}
			num++;
		}
	}

	// Token: 0x06008C57 RID: 35927 RVA: 0x00371564 File Offset: 0x0036F764
	public void LoadUnlocks()
	{
		this.unlocked.Clear();
		if (!File.Exists(Unlocks.UnlocksFilename))
		{
			return;
		}
		string text = "";
		bool flag = false;
		int num = 0;
		while (!flag && num < 5)
		{
			try
			{
				Thread.Sleep(num * 100);
				using (FileStream fileStream = File.Open(Unlocks.UnlocksFilename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
				{
					flag = true;
					ASCIIEncoding asciiencoding = new ASCIIEncoding();
					byte[] array = new byte[fileStream.Length];
					if ((long)fileStream.Read(array, 0, array.Length) == fileStream.Length)
					{
						text += asciiencoding.GetString(array);
					}
				}
			}
			catch (Exception ex)
			{
				global::Debug.LogWarningFormat("Failed to load Unlocks attempt {0}: {1}", new object[]
				{
					num + 1,
					ex.ToString()
				});
			}
			num++;
		}
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		try
		{
			foreach (string text2 in JsonConvert.DeserializeObject<string[]>(text))
			{
				if (!string.IsNullOrEmpty(text2) && !this.unlocked.Contains(text2))
				{
					this.unlocked.Add(text2);
				}
			}
		}
		catch (Exception ex2)
		{
			global::Debug.LogErrorFormat("Error parsing unlocks file [{0}]: {1}", new object[]
			{
				Unlocks.UnlocksFilename,
				ex2.ToString()
			});
		}
	}

	// Token: 0x06008C58 RID: 35928 RVA: 0x003716D4 File Offset: 0x0036F8D4
	private string GetNextClusterUnlock(string collectionID, out LoreCollectionOverride.OrderRule orderRule, bool randomize)
	{
		foreach (LoreCollectionOverride loreCollectionOverride in SaveLoader.Instance.ClusterLayout.clusterUnlocks)
		{
			if (!(loreCollectionOverride.id != collectionID))
			{
				if (!this.lockCollections.ContainsKey(collectionID))
				{
					DebugUtil.DevLogError("Lore collection '" + collectionID + "' is missing");
					orderRule = LoreCollectionOverride.OrderRule.Invalid;
					return null;
				}
				string[] array = this.lockCollections[loreCollectionOverride.collection];
				if (randomize)
				{
					array.Shuffle<string>();
				}
				foreach (string text in array)
				{
					if (!this.IsUnlocked(text))
					{
						orderRule = loreCollectionOverride.orderRule;
						return text;
					}
				}
				if (loreCollectionOverride.orderRule == LoreCollectionOverride.OrderRule.Replace)
				{
					orderRule = loreCollectionOverride.orderRule;
					return null;
				}
			}
		}
		orderRule = LoreCollectionOverride.OrderRule.Invalid;
		return null;
	}

	// Token: 0x06008C59 RID: 35929 RVA: 0x003717D8 File Offset: 0x0036F9D8
	private string GetNextGlobalDlcUnlock(string collectionID, out LoreCollectionOverride.OrderRule orderRule, bool randomize)
	{
		foreach (string name in CustomGameSettings.Instance.GetCurrentDlcMixingIds())
		{
			DlcMixingSettings cachedDlcMixingSettings = SettingsCache.GetCachedDlcMixingSettings(name);
			if (cachedDlcMixingSettings != null)
			{
				foreach (LoreCollectionOverride loreCollectionOverride in cachedDlcMixingSettings.globalLoreUnlocks)
				{
					if (!(loreCollectionOverride.id != collectionID))
					{
						if (!this.lockCollections.ContainsKey(collectionID))
						{
							DebugUtil.DevLogError("Lore collection '" + collectionID + "' is missing");
							orderRule = LoreCollectionOverride.OrderRule.Invalid;
							return null;
						}
						string[] array = this.lockCollections[loreCollectionOverride.collection];
						if (randomize)
						{
							array.Shuffle<string>();
						}
						foreach (string text in array)
						{
							if (!this.IsUnlocked(text))
							{
								orderRule = loreCollectionOverride.orderRule;
								return text;
							}
						}
						if (loreCollectionOverride.orderRule == LoreCollectionOverride.OrderRule.Replace)
						{
							orderRule = loreCollectionOverride.orderRule;
							return null;
						}
					}
				}
			}
		}
		orderRule = LoreCollectionOverride.OrderRule.Invalid;
		return null;
	}

	// Token: 0x06008C5A RID: 35930 RVA: 0x00371944 File Offset: 0x0036FB44
	public string UnlockNext(string collectionID, bool randomize = false)
	{
		if (SaveLoader.Instance != null)
		{
			LoreCollectionOverride.OrderRule orderRule;
			string text = this.GetNextClusterUnlock(collectionID, out orderRule, randomize);
			if (text != null && (orderRule == LoreCollectionOverride.OrderRule.Prepend || orderRule == LoreCollectionOverride.OrderRule.Replace))
			{
				this.Unlock(text, true);
				return text;
			}
			LoreCollectionOverride.OrderRule orderRule2;
			text = this.GetNextGlobalDlcUnlock(collectionID, out orderRule2, randomize);
			if (text != null && (orderRule2 == LoreCollectionOverride.OrderRule.Prepend || orderRule2 == LoreCollectionOverride.OrderRule.Replace))
			{
				this.Unlock(text, true);
				return text;
			}
			if (orderRule == LoreCollectionOverride.OrderRule.Replace || orderRule2 == LoreCollectionOverride.OrderRule.Replace)
			{
				return null;
			}
		}
		string[] array = this.lockCollections[collectionID];
		if (randomize)
		{
			array.Shuffle<string>();
		}
		foreach (string text2 in array)
		{
			if (string.IsNullOrEmpty(text2))
			{
				DebugUtil.DevAssertArgs(false, new object[]
				{
					"Found null/empty string in Unlocks collection: ",
					collectionID
				});
			}
			else if (!this.IsUnlocked(text2))
			{
				this.Unlock(text2, true);
				return text2;
			}
		}
		if (SaveLoader.Instance != null)
		{
			LoreCollectionOverride.OrderRule orderRule3;
			string text3 = this.GetNextClusterUnlock(collectionID, out orderRule3, randomize);
			if (text3 != null && orderRule3 == LoreCollectionOverride.OrderRule.Append)
			{
				this.Unlock(text3, true);
				return text3;
			}
			text3 = this.GetNextGlobalDlcUnlock(collectionID, out orderRule3, randomize);
			if (text3 != null && orderRule3 == LoreCollectionOverride.OrderRule.Append)
			{
				this.Unlock(text3, true);
				return text3;
			}
		}
		return null;
	}

	// Token: 0x06008C5B RID: 35931 RVA: 0x00371A64 File Offset: 0x0036FC64
	private MessageNotification GenerateCodexUnlockNotification(string lockID)
	{
		string entryForLock = CodexCache.GetEntryForLock(lockID);
		if (string.IsNullOrEmpty(entryForLock))
		{
			return null;
		}
		string text = null;
		if (CodexCache.FindSubEntry(lockID) != null)
		{
			text = CodexCache.FindSubEntry(lockID).title;
		}
		else if (CodexCache.FindSubEntry(entryForLock) != null)
		{
			text = CodexCache.FindSubEntry(entryForLock).title;
		}
		else if (CodexCache.FindEntry(entryForLock) != null)
		{
			text = CodexCache.FindEntry(entryForLock).title;
		}
		string text2 = UI.FormatAsLink(Strings.Get(text), entryForLock);
		if (!string.IsNullOrEmpty(text))
		{
			ContentContainer contentContainer = CodexCache.FindEntry(entryForLock).contentContainers.Find((ContentContainer match) => match.lockID == lockID);
			if (contentContainer != null)
			{
				foreach (ICodexWidget codexWidget in contentContainer.content)
				{
					CodexText codexText = codexWidget as CodexText;
					if (codexText != null)
					{
						text2 = text2 + "\n\n" + codexText.text;
					}
				}
			}
			return new MessageNotification(new CodexUnlockedMessage(lockID, text2));
		}
		return null;
	}

	// Token: 0x06008C5C RID: 35932 RVA: 0x00371B90 File Offset: 0x0036FD90
	private void UnlockCycleCodexes()
	{
		foreach (KeyValuePair<int, string> keyValuePair in this.cycleLocked)
		{
			if (GameClock.Instance.GetCycle() + 1 >= keyValuePair.Key)
			{
				this.Unlock(keyValuePair.Value, true);
			}
		}
	}

	// Token: 0x06008C5D RID: 35933 RVA: 0x0010050D File Offset: 0x000FE70D
	private void OnNewDay(object data)
	{
		this.UnlockCycleCodexes();
	}

	// Token: 0x06008C5E RID: 35934 RVA: 0x00100515 File Offset: 0x000FE715
	private void OnLaunchRocket(object data)
	{
		this.Unlock("surfacebreach", true);
		this.Unlock("firstrocketlaunch", true);
	}

	// Token: 0x06008C5F RID: 35935 RVA: 0x0010052F File Offset: 0x000FE72F
	private void OnDuplicantDied(object data)
	{
		this.Unlock("duplicantdeath", true);
		if (Components.LiveMinionIdentities.Count == 1)
		{
			this.Unlock("onedupeleft", true);
		}
	}

	// Token: 0x06008C60 RID: 35936 RVA: 0x00100556 File Offset: 0x000FE756
	private void OnNewDupe(MinionIdentity minion_identity)
	{
		if (Components.LiveMinionIdentities.Count >= Db.Get().Personalities.GetAll(true, false).Count)
		{
			this.Unlock("fulldupecolony", true);
		}
	}

	// Token: 0x06008C61 RID: 35937 RVA: 0x00100586 File Offset: 0x000FE786
	private void OnDiscoveredSpace(object data)
	{
		this.Unlock("surfacebreach", true);
	}

	// Token: 0x06008C62 RID: 35938 RVA: 0x00371C00 File Offset: 0x0036FE00
	public void Sim4000ms(float dt)
	{
		int x = int.MinValue;
		int num = int.MinValue;
		int x2 = int.MaxValue;
		int num2 = int.MaxValue;
		foreach (MinionIdentity minionIdentity in Components.MinionIdentities.Items)
		{
			if (!(minionIdentity == null))
			{
				int cell = Grid.PosToCell(minionIdentity);
				if (Grid.IsValidCell(cell))
				{
					int num3;
					int num4;
					Grid.CellToXY(cell, out num3, out num4);
					if (num4 > num)
					{
						num = num4;
						x = num3;
					}
					if (num4 < num2)
					{
						x2 = num3;
						num2 = num4;
					}
				}
			}
		}
		if (num != -2147483648)
		{
			int num5 = num;
			for (int i = 0; i < 30; i++)
			{
				num5++;
				int cell2 = Grid.XYToCell(x, num5);
				if (!Grid.IsValidCell(cell2))
				{
					break;
				}
				if (global::World.Instance.zoneRenderData.GetSubWorldZoneType(cell2) == SubWorld.ZoneType.Space)
				{
					this.Unlock("nearingsurface", true);
					break;
				}
			}
		}
		if (num2 != 2147483647)
		{
			int num6 = num2;
			for (int j = 0; j < 30; j++)
			{
				num6--;
				int num7 = Grid.XYToCell(x2, num6);
				if (!Grid.IsValidCell(num7))
				{
					break;
				}
				if (global::World.Instance.zoneRenderData.GetSubWorldZoneType(num7) == SubWorld.ZoneType.ToxicJungle && Grid.Element[num7].id == SimHashes.Magma)
				{
					this.Unlock("nearingmagma", true);
					return;
				}
			}
		}
	}

	// Token: 0x06008C65 RID: 35941 RVA: 0x00372430 File Offset: 0x00370630
	[CompilerGenerated]
	private bool <EvalMetaCategories>g__EvaluateCollection|14_0(LoreCollectionOverride loreUnlock, ref Unlocks.<>c__DisplayClass14_0 A_2)
	{
		if (loreUnlock.id == A_2.mesaCollectionID)
		{
			foreach (string unlockID in this.lockCollections[loreUnlock.collection])
			{
				if (this.IsUnlocked(unlockID))
				{
					int count = A_2.count;
					A_2.count = count + 1;
				}
			}
			if (loreUnlock.orderRule == LoreCollectionOverride.OrderRule.Replace)
			{
				A_2.isCollectionReplaced = true;
				return true;
			}
		}
		return false;
	}

	// Token: 0x040069F1 RID: 27121
	private const int FILE_IO_RETRY_ATTEMPTS = 5;

	// Token: 0x040069F2 RID: 27122
	private List<string> unlocked = new List<string>();

	// Token: 0x040069F3 RID: 27123
	private List<Unlocks.MetaUnlockCategory> MetaUnlockCategories = new List<Unlocks.MetaUnlockCategory>
	{
		new Unlocks.MetaUnlockCategory("dimensionalloreMeta", "dimensionallore", 4)
	};

	// Token: 0x040069F4 RID: 27124
	public Dictionary<string, string[]> lockCollections = new Dictionary<string, string[]>
	{
		{
			"emails",
			new string[]
			{
				"email_thermodynamiclaws",
				"email_security2",
				"email_pens2",
				"email_atomiconrecruitment",
				"email_devonsblog",
				"email_researchgiant",
				"email_thejanitor",
				"email_newemployee",
				"email_timeoffapproved",
				"email_security3",
				"email_preliminarycalculations",
				"email_hollandsdog",
				"email_temporalbowupdate",
				"email_retemporalbowupdate",
				"email_memorychip",
				"email_arthistoryrequest",
				"email_AIcontrol",
				"email_AIcontrol2",
				"email_friendlyemail",
				"email_AIcontrol3",
				"email_AIcontrol4",
				"email_engineeringcandidate",
				"email_missingnotes",
				"email_journalistrequest",
				"email_journalistrequest2"
			}
		},
		{
			"dlc2emails",
			new string[]
			{
				"email_newbaby",
				"email_cerestourism1",
				"email_cerestourism2",
				"email_voicemail",
				"email_expelled"
			}
		},
		{
			"dlc3emails",
			new string[]
			{
				"email_ulti"
			}
		},
		{
			"journals",
			new string[]
			{
				"journal_timesarrowthoughts",
				"journal_A046_1",
				"journal_B835_1",
				"journal_sunflowerseeds",
				"journal_B327_1",
				"journal_B556_1",
				"journal_employeeprocessing",
				"journal_B327_2",
				"journal_A046_2",
				"journal_elliesbirthday1",
				"journal_B835_2",
				"journal_ants",
				"journal_pipedream",
				"journal_B556_2",
				"journal_movedrats",
				"journal_B835_3",
				"journal_A046_3",
				"journal_B556_3",
				"journal_B327_3",
				"journal_B835_4",
				"journal_cleanup",
				"journal_A046_4",
				"journal_B327_4",
				"journal_revisitednumbers",
				"journal_B556_4",
				"journal_B835_5",
				"journal_elliesbirthday2",
				"journal_B111_1",
				"journal_revisitednumbers2",
				"journal_timemusings",
				"journal_evil",
				"journal_timesorder",
				"journal_inspace",
				"journal_mysteryaward",
				"journal_courier"
			}
		},
		{
			"dlc3journals",
			new string[]
			{
				"journal_potatobattery1",
				"journal_potatobattery2",
				"journal_potatobattery3"
			}
		},
		{
			"researchnotes",
			new string[]
			{
				"notes_clonedrats",
				"misc_dishbot",
				"notes_agriculture1",
				"notes_husbandry1",
				"notes_hibiscus3",
				"misc_newsecurity",
				"notes_husbandry2",
				"notes_agriculture2",
				"notes_geneticooze",
				"notes_agriculture3",
				"notes_husbandry3",
				"misc_casualfriday",
				"notes_memoryimplantation",
				"notes_husbandry4",
				"notes_agriculture4",
				"notes_neutronium",
				"misc_mailroometiquette",
				"notes_firstsuccess",
				"misc_reminder",
				"notes_neutroniumapplications",
				"notes_teleportation",
				"notes_AI",
				"misc_politerequest",
				"cryotank_warning",
				"misc_unattendedcultures"
			}
		},
		{
			"dlc2researchnotes",
			new string[]
			{
				"notes_cleanup"
			}
		},
		{
			"dlc3researchnotes",
			new string[]
			{
				"notes_talkshow",
				"notes_remoteworkstation"
			}
		},
		{
			"dimensionallore",
			new string[]
			{
				"notes_clonedrabbits",
				"notes_clonedraccoons",
				"journal_movedrabbits",
				"journal_movedraccoons",
				"journal_strawberries",
				"journal_shrimp"
			}
		},
		{
			"dimensionalloreMeta",
			new string[]
			{
				"log9"
			}
		},
		{
			"dlc2dimensionallore",
			new string[]
			{
				"notes_tragicnews",
				"notes_tragicnews2",
				"notes_tragicnews3"
			}
		},
		{
			"dlc2archivebuilding",
			new string[]
			{
				"notes_welcometoceres"
			}
		},
		{
			"dlc2geoplantinput",
			new string[]
			{
				"notes_geoinputs"
			}
		},
		{
			"dlc2geoplantcomplete",
			new string[]
			{
				"notes_earthquake"
			}
		},
		{
			"space",
			new string[]
			{
				"display_spaceprop1",
				"notice_pilot",
				"journal_inspace",
				"notes_firstcolony"
			}
		},
		{
			"storytraits",
			new string[]
			{
				"story_trait_critter_manipulator_initial",
				"story_trait_critter_manipulator_complete",
				"storytrait_crittermanipulator_workiversary",
				"story_trait_mega_brain_tank_initial",
				"story_trait_mega_brain_tank_competed",
				"story_trait_fossilhunt_initial",
				"story_trait_fossilhunt_poi1",
				"story_trait_fossilhunt_poi2",
				"story_trait_fossilhunt_poi3",
				"story_trait_fossilhunt_complete",
				"story_trait_morbrover_initial",
				"story_trait_morbrover_reveal",
				"story_trait_morbrover_reveal_lore",
				"story_trait_morbrover_complete",
				"story_trait_morbrover_complete_lore",
				"story_trait_morbrover_biobot",
				"story_trait_morbrover_locker"
			}
		}
	};

	// Token: 0x040069F5 RID: 27125
	public Dictionary<int, string> cycleLocked = new Dictionary<int, string>
	{
		{
			0,
			"log1"
		},
		{
			3,
			"log2"
		},
		{
			15,
			"log3"
		},
		{
			1000,
			"log4"
		},
		{
			1500,
			"log4b"
		},
		{
			2000,
			"log5"
		},
		{
			2500,
			"log5b"
		},
		{
			3000,
			"log6"
		},
		{
			3500,
			"log6b"
		},
		{
			4000,
			"log7"
		},
		{
			4001,
			"log8"
		}
	};

	// Token: 0x040069F6 RID: 27126
	private static readonly EventSystem.IntraObjectHandler<Unlocks> OnLaunchRocketDelegate = new EventSystem.IntraObjectHandler<Unlocks>(delegate(Unlocks component, object data)
	{
		component.OnLaunchRocket(data);
	});

	// Token: 0x040069F7 RID: 27127
	private static readonly EventSystem.IntraObjectHandler<Unlocks> OnDuplicantDiedDelegate = new EventSystem.IntraObjectHandler<Unlocks>(delegate(Unlocks component, object data)
	{
		component.OnDuplicantDied(data);
	});

	// Token: 0x040069F8 RID: 27128
	private static readonly EventSystem.IntraObjectHandler<Unlocks> OnDiscoveredSpaceDelegate = new EventSystem.IntraObjectHandler<Unlocks>(delegate(Unlocks component, object data)
	{
		component.OnDiscoveredSpace(data);
	});

	// Token: 0x02001A4F RID: 6735
	private class MetaUnlockCategory
	{
		// Token: 0x06008C66 RID: 35942 RVA: 0x00100594 File Offset: 0x000FE794
		public MetaUnlockCategory(string metaCollectionID, string mesaCollectionID, int mesaUnlockCount)
		{
			this.metaCollectionID = metaCollectionID;
			this.mesaCollectionID = mesaCollectionID;
			this.mesaUnlockCount = mesaUnlockCount;
		}

		// Token: 0x040069F9 RID: 27129
		public string metaCollectionID;

		// Token: 0x040069FA RID: 27130
		public string mesaCollectionID;

		// Token: 0x040069FB RID: 27131
		public int mesaUnlockCount;
	}
}
