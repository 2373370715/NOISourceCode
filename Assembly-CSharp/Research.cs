using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Database;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020017F0 RID: 6128
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Research")]
public class Research : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x06007E0C RID: 32268 RVA: 0x000F781B File Offset: 0x000F5A1B
	public static void DestroyInstance()
	{
		Research.Instance = null;
	}

	// Token: 0x06007E0D RID: 32269 RVA: 0x003356CC File Offset: 0x003338CC
	public TechInstance GetTechInstance(string techID)
	{
		return this.techs.Find((TechInstance match) => match.tech.Id == techID);
	}

	// Token: 0x06007E0E RID: 32270 RVA: 0x000F7823 File Offset: 0x000F5A23
	public bool IsBeingResearched(Tech tech)
	{
		return this.activeResearch != null && tech != null && this.activeResearch.tech == tech;
	}

	// Token: 0x06007E0F RID: 32271 RVA: 0x000F7840 File Offset: 0x000F5A40
	protected override void OnPrefabInit()
	{
		Research.Instance = this;
		this.researchTypes = new ResearchTypes();
	}

	// Token: 0x06007E10 RID: 32272 RVA: 0x00335700 File Offset: 0x00333900
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.globalPointInventory == null)
		{
			this.globalPointInventory = new ResearchPointInventory();
		}
		this.skillsUpdateHandle = Game.Instance.Subscribe(-1523247426, new Action<object>(this.OnRolesUpdated));
		this.OnRolesUpdated(null);
		Components.ResearchCenters.OnAdd += new Action<IResearchCenter>(this.CheckResearchBuildings);
		Components.ResearchCenters.OnRemove += new Action<IResearchCenter>(this.CheckResearchBuildings);
		foreach (KPrefabID kprefabID in Assets.Prefabs)
		{
			IResearchCenter component = kprefabID.GetComponent<IResearchCenter>();
			if (component != null)
			{
				this.researchCenterPrefabs.Add(component);
			}
		}
	}

	// Token: 0x06007E11 RID: 32273 RVA: 0x000F7853 File Offset: 0x000F5A53
	public ResearchType GetResearchType(string id)
	{
		return this.researchTypes.GetResearchType(id);
	}

	// Token: 0x06007E12 RID: 32274 RVA: 0x000F7861 File Offset: 0x000F5A61
	public TechInstance GetActiveResearch()
	{
		return this.activeResearch;
	}

	// Token: 0x06007E13 RID: 32275 RVA: 0x000F7869 File Offset: 0x000F5A69
	public TechInstance GetTargetResearch()
	{
		if (this.queuedTech != null && this.queuedTech.Count > 0)
		{
			return this.queuedTech[this.queuedTech.Count - 1];
		}
		return null;
	}

	// Token: 0x06007E14 RID: 32276 RVA: 0x003357CC File Offset: 0x003339CC
	public TechInstance Get(Tech tech)
	{
		foreach (TechInstance techInstance in this.techs)
		{
			if (techInstance.tech == tech)
			{
				return techInstance;
			}
		}
		return null;
	}

	// Token: 0x06007E15 RID: 32277 RVA: 0x00335828 File Offset: 0x00333A28
	public TechInstance GetOrAdd(Tech tech)
	{
		TechInstance techInstance = this.techs.Find((TechInstance tc) => tc.tech == tech);
		if (techInstance != null)
		{
			return techInstance;
		}
		TechInstance techInstance2 = new TechInstance(tech);
		this.techs.Add(techInstance2);
		return techInstance2;
	}

	// Token: 0x06007E16 RID: 32278 RVA: 0x00335878 File Offset: 0x00333A78
	public void GetNextTech()
	{
		if (this.queuedTech.Count > 0)
		{
			this.queuedTech.RemoveAt(0);
		}
		if (this.queuedTech.Count > 0)
		{
			this.SetActiveResearch(this.queuedTech[this.queuedTech.Count - 1].tech, false);
			return;
		}
		this.SetActiveResearch(null, false);
	}

	// Token: 0x06007E17 RID: 32279 RVA: 0x003358DC File Offset: 0x00333ADC
	private void AddTechToQueue(Tech tech)
	{
		TechInstance orAdd = this.GetOrAdd(tech);
		if (!orAdd.IsComplete() && !this.queuedTech.Contains(orAdd))
		{
			this.queuedTech.Add(orAdd);
		}
		orAdd.tech.requiredTech.ForEach(delegate(Tech _tech)
		{
			this.AddTechToQueue(_tech);
		});
	}

	// Token: 0x06007E18 RID: 32280 RVA: 0x00335930 File Offset: 0x00333B30
	public void CancelResearch(Tech tech, bool clickedEntry = true)
	{
		Research.<>c__DisplayClass26_0 CS$<>8__locals1 = new Research.<>c__DisplayClass26_0();
		CS$<>8__locals1.tech = tech;
		CS$<>8__locals1.ti = this.queuedTech.Find((TechInstance qt) => qt.tech == CS$<>8__locals1.tech);
		if (CS$<>8__locals1.ti == null)
		{
			return;
		}
		this.SetActiveResearch(null, false);
		int i;
		int j;
		for (i = CS$<>8__locals1.ti.tech.unlockedTech.Count - 1; i >= 0; i = j - 1)
		{
			if (this.queuedTech.Find((TechInstance qt) => qt.tech == CS$<>8__locals1.ti.tech.unlockedTech[i]) != null)
			{
				this.CancelResearch(CS$<>8__locals1.ti.tech.unlockedTech[i], false);
			}
			j = i;
		}
		this.queuedTech.Remove(CS$<>8__locals1.ti);
		if (clickedEntry)
		{
			this.NotifyResearchCenters(GameHashes.ActiveResearchChanged, this.queuedTech);
		}
	}

	// Token: 0x06007E19 RID: 32281 RVA: 0x00335A28 File Offset: 0x00333C28
	private void NotifyResearchCenters(GameHashes hash, object data)
	{
		foreach (object obj in Components.ResearchCenters)
		{
			((KMonoBehaviour)obj).Trigger(-1914338957, data);
		}
		base.Trigger((int)hash, data);
	}

	// Token: 0x06007E1A RID: 32282 RVA: 0x00335A8C File Offset: 0x00333C8C
	public void SetActiveResearch(Tech tech, bool clearQueue = false)
	{
		if (clearQueue)
		{
			this.queuedTech.Clear();
		}
		this.activeResearch = null;
		if (tech != null)
		{
			if (this.queuedTech.Count == 0)
			{
				this.AddTechToQueue(tech);
			}
			if (this.queuedTech.Count > 0)
			{
				this.queuedTech.Sort((TechInstance x, TechInstance y) => x.tech.tier.CompareTo(y.tech.tier));
				this.activeResearch = this.queuedTech[0];
			}
		}
		else
		{
			this.queuedTech.Clear();
		}
		this.NotifyResearchCenters(GameHashes.ActiveResearchChanged, this.queuedTech);
		this.CheckBuyResearch();
		this.CheckResearchBuildings(null);
		this.UpdateResearcherRoleNotification();
	}

	// Token: 0x06007E1B RID: 32283 RVA: 0x00335B40 File Offset: 0x00333D40
	private void UpdateResearcherRoleNotification()
	{
		if (this.NoResearcherRoleNotification != null)
		{
			this.notifier.Remove(this.NoResearcherRoleNotification);
			this.NoResearcherRoleNotification = null;
		}
		if (this.activeResearch != null)
		{
			Skill skill = null;
			if (this.activeResearch.tech.costsByResearchTypeID.ContainsKey("advanced") && this.activeResearch.tech.costsByResearchTypeID["advanced"] > 0f && !MinionResume.AnyMinionHasPerk(Db.Get().SkillPerks.AllowAdvancedResearch.Id, -1))
			{
				skill = Db.Get().Skills.GetSkillsWithPerk(Db.Get().SkillPerks.AllowAdvancedResearch)[0];
			}
			else if (this.activeResearch.tech.costsByResearchTypeID.ContainsKey("space") && this.activeResearch.tech.costsByResearchTypeID["space"] > 0f && !MinionResume.AnyMinionHasPerk(Db.Get().SkillPerks.AllowInterstellarResearch.Id, -1))
			{
				skill = Db.Get().Skills.GetSkillsWithPerk(Db.Get().SkillPerks.AllowInterstellarResearch)[0];
			}
			else if (this.activeResearch.tech.costsByResearchTypeID.ContainsKey("nuclear") && this.activeResearch.tech.costsByResearchTypeID["nuclear"] > 0f && !MinionResume.AnyMinionHasPerk(Db.Get().SkillPerks.AllowNuclearResearch.Id, -1))
			{
				skill = Db.Get().Skills.GetSkillsWithPerk(Db.Get().SkillPerks.AllowNuclearResearch)[0];
			}
			else if (this.activeResearch.tech.costsByResearchTypeID.ContainsKey("orbital") && this.activeResearch.tech.costsByResearchTypeID["orbital"] > 0f && !MinionResume.AnyMinionHasPerk(Db.Get().SkillPerks.AllowOrbitalResearch.Id, -1))
			{
				skill = Db.Get().Skills.GetSkillsWithPerk(Db.Get().SkillPerks.AllowOrbitalResearch)[0];
			}
			if (skill != null)
			{
				this.NoResearcherRoleNotification = new Notification(RESEARCH.MESSAGING.NO_RESEARCHER_SKILL, NotificationType.Bad, new Func<List<Notification>, object, string>(this.NoResearcherRoleTooltip), skill, false, 12f, null, null, null, true, false, false);
				this.notifier.Add(this.NoResearcherRoleNotification, "");
			}
		}
	}

	// Token: 0x06007E1C RID: 32284 RVA: 0x00335DC8 File Offset: 0x00333FC8
	private string NoResearcherRoleTooltip(List<Notification> list, object data)
	{
		Skill skill = (Skill)data;
		return RESEARCH.MESSAGING.NO_RESEARCHER_SKILL_TOOLTIP.Replace("{ResearchType}", skill.Name);
	}

	// Token: 0x06007E1D RID: 32285 RVA: 0x00335DF4 File Offset: 0x00333FF4
	public void AddResearchPoints(string researchTypeID, float points)
	{
		if (!this.UseGlobalPointInventory && this.activeResearch == null)
		{
			global::Debug.LogWarning("No active research to add research points to. Global research inventory is disabled.");
			return;
		}
		(this.UseGlobalPointInventory ? this.globalPointInventory : this.activeResearch.progressInventory).AddResearchPoints(researchTypeID, points);
		this.CheckBuyResearch();
		this.NotifyResearchCenters(GameHashes.ResearchPointsChanged, null);
	}

	// Token: 0x06007E1E RID: 32286 RVA: 0x00335E50 File Offset: 0x00334050
	private void CheckBuyResearch()
	{
		if (this.activeResearch != null)
		{
			ResearchPointInventory researchPointInventory = this.UseGlobalPointInventory ? this.globalPointInventory : this.activeResearch.progressInventory;
			if (this.activeResearch.tech.CanAfford(researchPointInventory))
			{
				foreach (KeyValuePair<string, float> keyValuePair in this.activeResearch.tech.costsByResearchTypeID)
				{
					researchPointInventory.RemoveResearchPoints(keyValuePair.Key, keyValuePair.Value);
				}
				this.activeResearch.Purchased();
				Game.Instance.Trigger(-107300940, this.activeResearch.tech);
				this.GetNextTech();
			}
		}
	}

	// Token: 0x06007E1F RID: 32287 RVA: 0x00335F20 File Offset: 0x00334120
	protected override void OnCleanUp()
	{
		if (Game.Instance != null && this.skillsUpdateHandle != -1)
		{
			Game.Instance.Unsubscribe(this.skillsUpdateHandle);
		}
		Components.ResearchCenters.OnAdd -= new Action<IResearchCenter>(this.CheckResearchBuildings);
		Components.ResearchCenters.OnRemove -= new Action<IResearchCenter>(this.CheckResearchBuildings);
		base.OnCleanUp();
	}

	// Token: 0x06007E20 RID: 32288 RVA: 0x00335F88 File Offset: 0x00334188
	public void CompleteQueue()
	{
		while (this.queuedTech.Count > 0)
		{
			foreach (KeyValuePair<string, float> keyValuePair in this.activeResearch.tech.costsByResearchTypeID)
			{
				this.AddResearchPoints(keyValuePair.Key, keyValuePair.Value);
			}
		}
	}

	// Token: 0x06007E21 RID: 32289 RVA: 0x000F789B File Offset: 0x000F5A9B
	public List<TechInstance> GetResearchQueue()
	{
		return new List<TechInstance>(this.queuedTech);
	}

	// Token: 0x06007E22 RID: 32290 RVA: 0x00336004 File Offset: 0x00334204
	[OnSerializing]
	internal void OnSerializing()
	{
		this.saveData = default(Research.SaveData);
		if (this.activeResearch != null)
		{
			this.saveData.activeResearchId = this.activeResearch.tech.Id;
		}
		else
		{
			this.saveData.activeResearchId = "";
		}
		if (this.queuedTech != null && this.queuedTech.Count > 0)
		{
			this.saveData.targetResearchId = this.queuedTech[this.queuedTech.Count - 1].tech.Id;
		}
		else
		{
			this.saveData.targetResearchId = "";
		}
		this.saveData.techs = new TechInstance.SaveData[this.techs.Count];
		for (int i = 0; i < this.techs.Count; i++)
		{
			this.saveData.techs[i] = this.techs[i].Save();
		}
	}

	// Token: 0x06007E23 RID: 32291 RVA: 0x003360FC File Offset: 0x003342FC
	[OnDeserialized]
	internal void OnDeserialized()
	{
		if (this.saveData.techs != null)
		{
			foreach (TechInstance.SaveData saveData in this.saveData.techs)
			{
				Tech tech = Db.Get().Techs.TryGet(saveData.techId);
				if (tech != null)
				{
					this.GetOrAdd(tech).Load(saveData);
				}
			}
		}
		foreach (TechInstance techInstance in this.techs)
		{
			if (this.saveData.targetResearchId == techInstance.tech.Id)
			{
				this.SetActiveResearch(techInstance.tech, false);
				break;
			}
		}
	}

	// Token: 0x06007E24 RID: 32292 RVA: 0x000F78A8 File Offset: 0x000F5AA8
	private void OnRolesUpdated(object data)
	{
		this.UpdateResearcherRoleNotification();
	}

	// Token: 0x06007E25 RID: 32293 RVA: 0x003361D0 File Offset: 0x003343D0
	public string GetMissingResearchBuildingName()
	{
		foreach (KeyValuePair<string, float> keyValuePair in this.activeResearch.tech.costsByResearchTypeID)
		{
			bool flag = true;
			if (keyValuePair.Value > 0f)
			{
				flag = false;
				using (List<IResearchCenter>.Enumerator enumerator2 = Components.ResearchCenters.Items.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.GetResearchType() == keyValuePair.Key)
						{
							flag = true;
							break;
						}
					}
				}
			}
			if (!flag)
			{
				foreach (IResearchCenter researchCenter in this.researchCenterPrefabs)
				{
					if (researchCenter.GetResearchType() == keyValuePair.Key)
					{
						return ((KMonoBehaviour)researchCenter).GetProperName();
					}
				}
				return null;
			}
		}
		return null;
	}

	// Token: 0x06007E26 RID: 32294 RVA: 0x00336300 File Offset: 0x00334500
	private void CheckResearchBuildings(object data)
	{
		if (this.activeResearch == null)
		{
			this.notifier.Remove(this.MissingResearchStation);
			return;
		}
		if (string.IsNullOrEmpty(this.GetMissingResearchBuildingName()))
		{
			this.notifier.Remove(this.MissingResearchStation);
			return;
		}
		this.notifier.Add(this.MissingResearchStation, "");
	}

	// Token: 0x04005FD3 RID: 24531
	public static Research Instance;

	// Token: 0x04005FD4 RID: 24532
	[MyCmpAdd]
	private Notifier notifier;

	// Token: 0x04005FD5 RID: 24533
	private List<TechInstance> techs = new List<TechInstance>();

	// Token: 0x04005FD6 RID: 24534
	private List<TechInstance> queuedTech = new List<TechInstance>();

	// Token: 0x04005FD7 RID: 24535
	private TechInstance activeResearch;

	// Token: 0x04005FD8 RID: 24536
	private Notification NoResearcherRoleNotification;

	// Token: 0x04005FD9 RID: 24537
	private Notification MissingResearchStation = new Notification(RESEARCH.MESSAGING.MISSING_RESEARCH_STATION, NotificationType.Bad, (List<Notification> list, object data) => RESEARCH.MESSAGING.MISSING_RESEARCH_STATION_TOOLTIP.ToString().Replace("{0}", Research.Instance.GetMissingResearchBuildingName()), null, false, 11f, null, null, null, true, false, false);

	// Token: 0x04005FDA RID: 24538
	private List<IResearchCenter> researchCenterPrefabs = new List<IResearchCenter>();

	// Token: 0x04005FDB RID: 24539
	protected int skillsUpdateHandle = -1;

	// Token: 0x04005FDC RID: 24540
	public ResearchTypes researchTypes;

	// Token: 0x04005FDD RID: 24541
	public bool UseGlobalPointInventory;

	// Token: 0x04005FDE RID: 24542
	[Serialize]
	public ResearchPointInventory globalPointInventory;

	// Token: 0x04005FDF RID: 24543
	[Serialize]
	private Research.SaveData saveData;

	// Token: 0x020017F1 RID: 6129
	private struct SaveData
	{
		// Token: 0x04005FE0 RID: 24544
		public string activeResearchId;

		// Token: 0x04005FE1 RID: 24545
		public string targetResearchId;

		// Token: 0x04005FE2 RID: 24546
		public TechInstance.SaveData[] techs;
	}
}
