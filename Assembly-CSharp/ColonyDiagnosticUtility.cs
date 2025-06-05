using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020009E1 RID: 2529
public class ColonyDiagnosticUtility : KMonoBehaviour, ISim1000ms
{
	// Token: 0x06002DD8 RID: 11736 RVA: 0x000C2217 File Offset: 0x000C0417
	public static void DestroyInstance()
	{
		ColonyDiagnosticUtility.Instance = null;
	}

	// Token: 0x06002DD9 RID: 11737 RVA: 0x001FFA90 File Offset: 0x001FDC90
	public ColonyDiagnostic.DiagnosticResult.Opinion GetWorldDiagnosticResult(int worldID)
	{
		ColonyDiagnostic.DiagnosticResult.Opinion opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Good;
		foreach (ColonyDiagnostic colonyDiagnostic in this.worldDiagnostics[worldID])
		{
			if (ColonyDiagnosticUtility.Instance.diagnosticDisplaySettings[worldID][colonyDiagnostic.id] != ColonyDiagnosticUtility.DisplaySetting.Never && !ColonyDiagnosticUtility.Instance.IsDiagnosticTutorialDisabled(colonyDiagnostic.id))
			{
				ColonyDiagnosticUtility.DisplaySetting displaySetting = this.diagnosticDisplaySettings[worldID][colonyDiagnostic.id];
				if (displaySetting > ColonyDiagnosticUtility.DisplaySetting.AlertOnly)
				{
					if (displaySetting != ColonyDiagnosticUtility.DisplaySetting.Never)
					{
					}
				}
				else
				{
					opinion = (ColonyDiagnostic.DiagnosticResult.Opinion)Math.Min((int)opinion, (int)colonyDiagnostic.LatestResult.opinion);
				}
			}
		}
		return opinion;
	}

	// Token: 0x06002DDA RID: 11738 RVA: 0x001FFB4C File Offset: 0x001FDD4C
	public string GetWorldDiagnosticResultStatus(int worldID)
	{
		ColonyDiagnostic colonyDiagnostic = null;
		foreach (ColonyDiagnostic colonyDiagnostic2 in this.worldDiagnostics[worldID])
		{
			if (ColonyDiagnosticUtility.Instance.diagnosticDisplaySettings[worldID][colonyDiagnostic2.id] != ColonyDiagnosticUtility.DisplaySetting.Never && !ColonyDiagnosticUtility.Instance.IsDiagnosticTutorialDisabled(colonyDiagnostic2.id))
			{
				ColonyDiagnosticUtility.DisplaySetting displaySetting = this.diagnosticDisplaySettings[worldID][colonyDiagnostic2.id];
				if (displaySetting > ColonyDiagnosticUtility.DisplaySetting.AlertOnly)
				{
					if (displaySetting != ColonyDiagnosticUtility.DisplaySetting.Never)
					{
					}
				}
				else if (colonyDiagnostic == null || colonyDiagnostic2.LatestResult.opinion < colonyDiagnostic.LatestResult.opinion)
				{
					colonyDiagnostic = colonyDiagnostic2;
				}
			}
		}
		if (colonyDiagnostic == null || colonyDiagnostic.LatestResult.opinion == ColonyDiagnostic.DiagnosticResult.Opinion.Normal)
		{
			return "";
		}
		return colonyDiagnostic.name;
	}

	// Token: 0x06002DDB RID: 11739 RVA: 0x001FFC2C File Offset: 0x001FDE2C
	public string GetWorldDiagnosticResultTooltip(int worldID)
	{
		string text = "";
		foreach (ColonyDiagnostic colonyDiagnostic in this.worldDiagnostics[worldID])
		{
			if (ColonyDiagnosticUtility.Instance.diagnosticDisplaySettings[worldID][colonyDiagnostic.id] != ColonyDiagnosticUtility.DisplaySetting.Never && !ColonyDiagnosticUtility.Instance.IsDiagnosticTutorialDisabled(colonyDiagnostic.id))
			{
				ColonyDiagnosticUtility.DisplaySetting displaySetting = this.diagnosticDisplaySettings[worldID][colonyDiagnostic.id];
				if (displaySetting > ColonyDiagnosticUtility.DisplaySetting.AlertOnly)
				{
					if (displaySetting != ColonyDiagnosticUtility.DisplaySetting.Never)
					{
					}
				}
				else if (colonyDiagnostic.LatestResult.opinion < ColonyDiagnostic.DiagnosticResult.Opinion.Normal)
				{
					text = text + "\n" + colonyDiagnostic.LatestResult.GetFormattedMessage();
				}
			}
		}
		return text;
	}

	// Token: 0x06002DDC RID: 11740 RVA: 0x000C221F File Offset: 0x000C041F
	public bool IsDiagnosticTutorialDisabled(string id)
	{
		return ColonyDiagnosticUtility.Instance.diagnosticTutorialStatus.ContainsKey(id) && GameClock.Instance.GetTime() < ColonyDiagnosticUtility.Instance.diagnosticTutorialStatus[id];
	}

	// Token: 0x06002DDD RID: 11741 RVA: 0x000C2252 File Offset: 0x000C0452
	public void ClearDiagnosticTutorialSetting(string id)
	{
		if (ColonyDiagnosticUtility.Instance.diagnosticTutorialStatus.ContainsKey(id))
		{
			ColonyDiagnosticUtility.Instance.diagnosticTutorialStatus[id] = -1f;
		}
	}

	// Token: 0x06002DDE RID: 11742 RVA: 0x001FFD08 File Offset: 0x001FDF08
	public bool IsCriteriaEnabled(int worldID, string diagnosticID, string criteriaID)
	{
		Dictionary<string, List<string>> dictionary = this.diagnosticCriteriaDisabled[worldID];
		return dictionary.ContainsKey(diagnosticID) && !dictionary[diagnosticID].Contains(criteriaID);
	}

	// Token: 0x06002DDF RID: 11743 RVA: 0x001FFD40 File Offset: 0x001FDF40
	public void SetCriteriaEnabled(int worldID, string diagnosticID, string criteriaID, bool enabled)
	{
		Dictionary<string, List<string>> dictionary = this.diagnosticCriteriaDisabled[worldID];
		global::Debug.Assert(dictionary.ContainsKey(diagnosticID), string.Format("Trying to set criteria on World {0} lacks diagnostic {1} that criteria {2} relates to", worldID, diagnosticID, criteriaID));
		List<string> list = dictionary[diagnosticID];
		if (enabled && list.Contains(criteriaID))
		{
			list.Remove(criteriaID);
		}
		if (!enabled && !list.Contains(criteriaID))
		{
			list.Add(criteriaID);
		}
	}

	// Token: 0x06002DE0 RID: 11744 RVA: 0x000C227B File Offset: 0x000C047B
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		ColonyDiagnosticUtility.Instance = this;
	}

	// Token: 0x06002DE1 RID: 11745 RVA: 0x001FFDA8 File Offset: 0x001FDFA8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 33))
		{
			string key = "IdleDiagnostic";
			foreach (int num in this.diagnosticDisplaySettings.Keys)
			{
				WorldContainer world = ClusterManager.Instance.GetWorld(num);
				if (this.diagnosticDisplaySettings[num].ContainsKey(key) && this.diagnosticDisplaySettings[num][key] != ColonyDiagnosticUtility.DisplaySetting.Always)
				{
					this.diagnosticDisplaySettings[num][key] = (world.IsModuleInterior ? ColonyDiagnosticUtility.DisplaySetting.Never : ColonyDiagnosticUtility.DisplaySetting.AlertOnly);
				}
			}
		}
		foreach (int worldID in ClusterManager.Instance.GetWorldIDsSorted())
		{
			this.AddWorld(worldID);
		}
		ClusterManager.Instance.Subscribe(-1280433810, new Action<object>(this.Refresh));
		ClusterManager.Instance.Subscribe(-1078710002, new Action<object>(this.RemoveWorld));
	}

	// Token: 0x06002DE2 RID: 11746 RVA: 0x001FFEF4 File Offset: 0x001FE0F4
	private void Refresh(object data)
	{
		int worldID = (int)data;
		this.AddWorld(worldID);
	}

	// Token: 0x06002DE3 RID: 11747 RVA: 0x001FFF10 File Offset: 0x001FE110
	private void RemoveWorld(object data)
	{
		int key = (int)data;
		if (this.diagnosticDisplaySettings.Remove(key))
		{
			List<ColonyDiagnostic> list;
			if (this.worldDiagnostics.TryGetValue(key, out list))
			{
				foreach (ColonyDiagnostic colonyDiagnostic in list)
				{
					colonyDiagnostic.OnCleanUp();
				}
			}
			this.worldDiagnostics.Remove(key);
		}
	}

	// Token: 0x06002DE4 RID: 11748 RVA: 0x001FFF90 File Offset: 0x001FE190
	public ColonyDiagnostic GetDiagnostic(string id, int worldID)
	{
		return this.worldDiagnostics[worldID].Find((ColonyDiagnostic match) => match.id == id);
	}

	// Token: 0x06002DE5 RID: 11749 RVA: 0x000C2289 File Offset: 0x000C0489
	public T GetDiagnostic<T>(int worldID) where T : ColonyDiagnostic
	{
		return (T)((object)this.worldDiagnostics[worldID].Find((ColonyDiagnostic match) => match is T));
	}

	// Token: 0x06002DE6 RID: 11750 RVA: 0x001FFFC8 File Offset: 0x001FE1C8
	public string GetDiagnosticName(string id)
	{
		foreach (KeyValuePair<int, List<ColonyDiagnostic>> keyValuePair in this.worldDiagnostics)
		{
			foreach (ColonyDiagnostic colonyDiagnostic in keyValuePair.Value)
			{
				if (colonyDiagnostic.id == id)
				{
					return colonyDiagnostic.name;
				}
			}
		}
		global::Debug.LogWarning("Cannot locate name of diagnostic " + id + " because no worlds have a diagnostic with that id ");
		return "";
	}

	// Token: 0x06002DE7 RID: 11751 RVA: 0x00200088 File Offset: 0x001FE288
	public ChoreGroupDiagnostic GetChoreGroupDiagnostic(int worldID, ChoreGroup choreGroup)
	{
		return (ChoreGroupDiagnostic)this.worldDiagnostics[worldID].Find((ColonyDiagnostic match) => match is ChoreGroupDiagnostic && ((ChoreGroupDiagnostic)match).choreGroup == choreGroup);
	}

	// Token: 0x06002DE8 RID: 11752 RVA: 0x002000C4 File Offset: 0x001FE2C4
	public WorkTimeDiagnostic GetWorkTimeDiagnostic(int worldID, ChoreGroup choreGroup)
	{
		return (WorkTimeDiagnostic)this.worldDiagnostics[worldID].Find((ColonyDiagnostic match) => match is WorkTimeDiagnostic && ((WorkTimeDiagnostic)match).choreGroup == choreGroup);
	}

	// Token: 0x06002DE9 RID: 11753 RVA: 0x000C22C0 File Offset: 0x000C04C0
	private void TryAddDiagnosticToWorldCollection(ref List<ColonyDiagnostic> newWorldDiagnostics, ColonyDiagnostic newDiagnostic)
	{
		if (!Game.IsCorrectDlcActiveForCurrentSave(newDiagnostic))
		{
			newDiagnostic.OnCleanUp();
			return;
		}
		newWorldDiagnostics.Add(newDiagnostic);
	}

	// Token: 0x06002DEA RID: 11754 RVA: 0x00200100 File Offset: 0x001FE300
	public void AddWorld(int worldID)
	{
		bool flag = false;
		if (!this.diagnosticDisplaySettings.ContainsKey(worldID))
		{
			this.diagnosticDisplaySettings.Add(worldID, new Dictionary<string, ColonyDiagnosticUtility.DisplaySetting>());
			flag = true;
		}
		if (!this.diagnosticCriteriaDisabled.ContainsKey(worldID))
		{
			this.diagnosticCriteriaDisabled.Add(worldID, new Dictionary<string, List<string>>());
		}
		List<ColonyDiagnostic> list = new List<ColonyDiagnostic>();
		this.TryAddDiagnosticToWorldCollection(ref list, new IdleDiagnostic(worldID));
		this.TryAddDiagnosticToWorldCollection(ref list, new BreathabilityDiagnostic(worldID));
		this.TryAddDiagnosticToWorldCollection(ref list, new FoodDiagnostic(worldID));
		this.TryAddDiagnosticToWorldCollection(ref list, new StressDiagnostic(worldID));
		this.TryAddDiagnosticToWorldCollection(ref list, new RadiationDiagnostic(worldID));
		this.TryAddDiagnosticToWorldCollection(ref list, new ReactorDiagnostic(worldID));
		this.TryAddDiagnosticToWorldCollection(ref list, new SelfChargingElectrobankDiagnostic(worldID));
		this.TryAddDiagnosticToWorldCollection(ref list, new BionicBatteryDiagnostic(worldID));
		if (ClusterManager.Instance.GetWorld(worldID).IsModuleInterior)
		{
			this.TryAddDiagnosticToWorldCollection(ref list, new FloatingRocketDiagnostic(worldID));
			this.TryAddDiagnosticToWorldCollection(ref list, new RocketFuelDiagnostic(worldID));
			this.TryAddDiagnosticToWorldCollection(ref list, new RocketOxidizerDiagnostic(worldID));
		}
		else
		{
			this.TryAddDiagnosticToWorldCollection(ref list, new BedDiagnostic(worldID));
			this.TryAddDiagnosticToWorldCollection(ref list, new ToiletDiagnostic(worldID));
			this.TryAddDiagnosticToWorldCollection(ref list, new PowerUseDiagnostic(worldID));
			this.TryAddDiagnosticToWorldCollection(ref list, new BatteryDiagnostic(worldID));
			this.TryAddDiagnosticToWorldCollection(ref list, new TrappedDuplicantDiagnostic(worldID));
			this.TryAddDiagnosticToWorldCollection(ref list, new FarmDiagnostic(worldID));
			this.TryAddDiagnosticToWorldCollection(ref list, new EntombedDiagnostic(worldID));
			this.TryAddDiagnosticToWorldCollection(ref list, new RocketsInOrbitDiagnostic(worldID));
			this.TryAddDiagnosticToWorldCollection(ref list, new MeteorDiagnostic(worldID));
		}
		this.worldDiagnostics.Add(worldID, list);
		foreach (ColonyDiagnostic colonyDiagnostic in list)
		{
			if (!this.diagnosticDisplaySettings[worldID].ContainsKey(colonyDiagnostic.id))
			{
				this.diagnosticDisplaySettings[worldID].Add(colonyDiagnostic.id, ColonyDiagnosticUtility.DisplaySetting.AlertOnly);
			}
			if (!this.diagnosticCriteriaDisabled[worldID].ContainsKey(colonyDiagnostic.id))
			{
				this.diagnosticCriteriaDisabled[worldID].Add(colonyDiagnostic.id, new List<string>());
			}
		}
		if (flag)
		{
			this.diagnosticDisplaySettings[worldID]["BreathabilityDiagnostic"] = ColonyDiagnosticUtility.DisplaySetting.Always;
			this.diagnosticDisplaySettings[worldID]["FoodDiagnostic"] = ColonyDiagnosticUtility.DisplaySetting.Always;
			this.diagnosticDisplaySettings[worldID]["StressDiagnostic"] = ColonyDiagnosticUtility.DisplaySetting.Always;
			if (ClusterManager.Instance.GetWorld(worldID).IsModuleInterior)
			{
				this.diagnosticDisplaySettings[worldID]["FloatingRocketDiagnostic"] = ColonyDiagnosticUtility.DisplaySetting.Always;
				this.diagnosticDisplaySettings[worldID]["RocketFuelDiagnostic"] = ColonyDiagnosticUtility.DisplaySetting.Always;
				this.diagnosticDisplaySettings[worldID]["RocketOxidizerDiagnostic"] = ColonyDiagnosticUtility.DisplaySetting.Always;
				this.diagnosticDisplaySettings[worldID]["IdleDiagnostic"] = ColonyDiagnosticUtility.DisplaySetting.Never;
				return;
			}
			this.diagnosticDisplaySettings[worldID]["IdleDiagnostic"] = ColonyDiagnosticUtility.DisplaySetting.AlertOnly;
		}
	}

	// Token: 0x06002DEB RID: 11755 RVA: 0x000C22D9 File Offset: 0x000C04D9
	public void Sim1000ms(float dt)
	{
		if (ColonyDiagnosticUtility.IgnoreFirstUpdate)
		{
			ColonyDiagnosticUtility.IgnoreFirstUpdate = false;
		}
	}

	// Token: 0x06002DEC RID: 11756 RVA: 0x00200400 File Offset: 0x001FE600
	public static bool PastNewBuildingGracePeriod(Transform building)
	{
		BuildingComplete component = building.GetComponent<BuildingComplete>();
		return !(component != null) || GameClock.Instance.GetTime() - component.creationTime >= 600f;
	}

	// Token: 0x06002DED RID: 11757 RVA: 0x00200438 File Offset: 0x001FE638
	public static bool IgnoreRocketsWithNoCrewRequested(int worldID, out ColonyDiagnostic.DiagnosticResult result)
	{
		WorldContainer world = ClusterManager.Instance.GetWorld(worldID);
		string message = world.IsModuleInterior ? UI.COLONY_DIAGNOSTICS.NO_MINIONS_ROCKET : UI.COLONY_DIAGNOSTICS.NO_MINIONS_PLANETOID;
		result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, message, null);
		if (world.IsModuleInterior)
		{
			for (int i = 0; i < Components.Clustercrafts.Count; i++)
			{
				WorldContainer interiorWorld = Components.Clustercrafts[i].ModuleInterface.GetInteriorWorld();
				if (!(interiorWorld == null) && interiorWorld.id == worldID)
				{
					PassengerRocketModule passengerModule = Components.Clustercrafts[i].ModuleInterface.GetPassengerModule();
					if (passengerModule != null && !passengerModule.ShouldCrewGetIn())
					{
						result = default(ColonyDiagnostic.DiagnosticResult);
						result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
						result.Message = UI.COLONY_DIAGNOSTICS.NO_MINIONS_REQUESTED;
						return true;
					}
				}
			}
		}
		return false;
	}

	// Token: 0x04001F64 RID: 8036
	public static ColonyDiagnosticUtility Instance;

	// Token: 0x04001F65 RID: 8037
	private Dictionary<int, List<ColonyDiagnostic>> worldDiagnostics = new Dictionary<int, List<ColonyDiagnostic>>();

	// Token: 0x04001F66 RID: 8038
	[Serialize]
	public Dictionary<int, Dictionary<string, ColonyDiagnosticUtility.DisplaySetting>> diagnosticDisplaySettings = new Dictionary<int, Dictionary<string, ColonyDiagnosticUtility.DisplaySetting>>();

	// Token: 0x04001F67 RID: 8039
	[Serialize]
	public Dictionary<int, Dictionary<string, List<string>>> diagnosticCriteriaDisabled = new Dictionary<int, Dictionary<string, List<string>>>();

	// Token: 0x04001F68 RID: 8040
	[Serialize]
	private Dictionary<string, float> diagnosticTutorialStatus = new Dictionary<string, float>
	{
		{
			"ToiletDiagnostic",
			450f
		},
		{
			"BedDiagnostic",
			900f
		},
		{
			"BreathabilityDiagnostic",
			1800f
		},
		{
			"FoodDiagnostic",
			3000f
		},
		{
			"FarmDiagnostic",
			6000f
		},
		{
			"StressDiagnostic",
			9000f
		},
		{
			"PowerUseDiagnostic",
			12000f
		},
		{
			"BatteryDiagnostic",
			12000f
		},
		{
			"IdleDiagnostic",
			600f
		}
	};

	// Token: 0x04001F69 RID: 8041
	public static bool IgnoreFirstUpdate = true;

	// Token: 0x04001F6A RID: 8042
	public static ColonyDiagnostic.DiagnosticResult NoDataResult = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.NO_DATA, null);

	// Token: 0x020009E2 RID: 2530
	public enum DisplaySetting
	{
		// Token: 0x04001F6C RID: 8044
		Always,
		// Token: 0x04001F6D RID: 8045
		AlertOnly,
		// Token: 0x04001F6E RID: 8046
		Never,
		// Token: 0x04001F6F RID: 8047
		LENGTH
	}
}
