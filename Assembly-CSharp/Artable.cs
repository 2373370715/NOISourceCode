﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Database;
using Klei.AI;
using KSerialization;
using TUNING;
using UnityEngine;

// Token: 0x020009B8 RID: 2488
[AddComponentMenu("KMonoBehaviour/Workable/Artable")]
public class Artable : Workable
{
	// Token: 0x1700019C RID: 412
	// (get) Token: 0x06002C98 RID: 11416 RVA: 0x000C1509 File Offset: 0x000BF709
	public string CurrentStage
	{
		get
		{
			return this.currentStage;
		}
	}

	// Token: 0x06002C99 RID: 11417 RVA: 0x000C1511 File Offset: 0x000BF711
	protected Artable()
	{
		this.faceTargetWhenWorking = true;
	}

	// Token: 0x06002C9A RID: 11418 RVA: 0x001F9EE8 File Offset: 0x001F80E8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Arting;
		this.attributeConverter = Db.Get().AttributeConverters.ArtSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Art.Id;
		this.skillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
		this.requiredSkillPerk = Db.Get().SkillPerks.CanArt.Id;
		base.SetWorkTime(80f);
	}

	// Token: 0x06002C9B RID: 11419 RVA: 0x001F9F7C File Offset: 0x001F817C
	protected override void OnSpawn()
	{
		base.GetComponent<KPrefabID>().PrefabID();
		if (string.IsNullOrEmpty(this.currentStage) || this.currentStage == this.defaultArtworkId)
		{
			this.SetDefault();
		}
		else
		{
			this.SetStage(this.currentStage, true);
		}
		this.shouldShowSkillPerkStatusItem = false;
		base.OnSpawn();
	}

	// Token: 0x06002C9C RID: 11420 RVA: 0x001F9FD8 File Offset: 0x001F81D8
	[OnDeserialized]
	public void OnDeserialized()
	{
		if (Db.GetArtableStages().TryGet(this.currentStage) == null && this.currentStage != this.defaultArtworkId)
		{
			string id = string.Format("{0}_{1}", base.GetComponent<KPrefabID>().PrefabID().ToString(), this.currentStage);
			if (Db.GetArtableStages().TryGet(id) == null)
			{
				global::Debug.LogWarning("Failed up to update " + this.currentStage + " to ArtableStages");
				this.currentStage = this.defaultArtworkId;
				return;
			}
			this.currentStage = id;
		}
	}

	// Token: 0x06002C9D RID: 11421 RVA: 0x001FA070 File Offset: 0x001F8270
	protected override void OnCompleteWork(WorkerBase worker)
	{
		if (string.IsNullOrEmpty(this.userChosenTargetStage))
		{
			Db db = Db.Get();
			Tag prefab_id = base.GetComponent<KPrefabID>().PrefabID();
			List<ArtableStage> prefabStages = Db.GetArtableStages().GetPrefabStages(prefab_id);
			ArtableStatusItem artist_skill = db.ArtableStatuses.LookingUgly;
			MinionResume component = worker.GetComponent<MinionResume>();
			if (component != null)
			{
				if (component.HasPerk(db.SkillPerks.CanArtGreat.Id))
				{
					artist_skill = db.ArtableStatuses.LookingGreat;
				}
				else if (component.HasPerk(db.SkillPerks.CanArtOkay.Id))
				{
					artist_skill = db.ArtableStatuses.LookingOkay;
				}
			}
			prefabStages.RemoveAll((ArtableStage stage) => stage.statusItem.StatusType > artist_skill.StatusType || stage.statusItem.StatusType == ArtableStatuses.ArtableStatusType.AwaitingArting);
			prefabStages.Sort((ArtableStage x, ArtableStage y) => y.statusItem.StatusType.CompareTo(x.statusItem.StatusType));
			ArtableStatuses.ArtableStatusType highest_type = prefabStages[0].statusItem.StatusType;
			prefabStages.RemoveAll((ArtableStage stage) => stage.statusItem.StatusType < highest_type);
			prefabStages.RemoveAll((ArtableStage stage) => !stage.IsUnlocked());
			prefabStages.Shuffle<ArtableStage>();
			this.SetStage(prefabStages[0].id, false);
			if (prefabStages[0].cheerOnComplete)
			{
				new EmoteChore(worker.GetComponent<ChoreProvider>(), db.ChoreTypes.EmoteHighPriority, db.Emotes.Minion.Cheer, 1, null);
			}
			else
			{
				new EmoteChore(worker.GetComponent<ChoreProvider>(), db.ChoreTypes.EmoteHighPriority, db.Emotes.Minion.Disappointed, 1, null);
			}
		}
		else
		{
			this.SetStage(this.userChosenTargetStage, false);
			this.userChosenTargetStage = null;
		}
		this.shouldShowSkillPerkStatusItem = false;
		this.UpdateStatusItem(null);
		Prioritizable.RemoveRef(base.gameObject);
	}

	// Token: 0x06002C9E RID: 11422 RVA: 0x001FA268 File Offset: 0x001F8468
	public void SetDefault()
	{
		this.currentStage = this.defaultArtworkId;
		base.GetComponent<KBatchedAnimController>().SwapAnims(base.GetComponent<Building>().Def.AnimFiles);
		base.GetComponent<KAnimControllerBase>().Play(this.defaultAnimName, KAnim.PlayMode.Once, 1f, 0f);
		KSelectable component = base.GetComponent<KSelectable>();
		BuildingDef def = base.GetComponent<Building>().Def;
		component.SetName(def.Name);
		component.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().ArtableStatuses.AwaitingArting, this);
		this.GetAttributes().Remove(this.artQualityDecorModifier);
		this.shouldShowSkillPerkStatusItem = false;
		this.UpdateStatusItem(null);
		if (this.currentStage == this.defaultArtworkId)
		{
			this.shouldShowSkillPerkStatusItem = true;
			Prioritizable.AddRef(base.gameObject);
			this.chore = new WorkChore<Artable>(Db.Get().ChoreTypes.Art, this, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			this.chore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, this.requiredSkillPerk);
		}
	}

	// Token: 0x06002C9F RID: 11423 RVA: 0x001FA38C File Offset: 0x001F858C
	public virtual void SetStage(string stage_id, bool skip_effect)
	{
		ArtableStage artableStage = Db.GetArtableStages().Get(stage_id);
		if (artableStage == null)
		{
			global::Debug.LogError("Missing stage: " + stage_id);
			return;
		}
		this.currentStage = artableStage.id;
		base.GetComponent<KBatchedAnimController>().SwapAnims(new KAnimFile[]
		{
			Assets.GetAnim(artableStage.animFile)
		});
		base.GetComponent<KAnimControllerBase>().Play(artableStage.anim, KAnim.PlayMode.Once, 1f, 0f);
		this.GetAttributes().Remove(this.artQualityDecorModifier);
		if (artableStage.decor != 0)
		{
			this.artQualityDecorModifier = new AttributeModifier(Db.Get().BuildingAttributes.Decor.Id, (float)artableStage.decor, "Art Quality", false, false, true);
			this.GetAttributes().Add(this.artQualityDecorModifier);
		}
		KSelectable component = base.GetComponent<KSelectable>();
		component.SetName(artableStage.Name);
		component.SetStatusItem(Db.Get().StatusItemCategories.Main, artableStage.statusItem, this);
		base.gameObject.GetComponent<BuildingComplete>().SetDescriptionFlavour(artableStage.Description);
		this.shouldShowSkillPerkStatusItem = false;
		this.UpdateStatusItem(null);
	}

	// Token: 0x06002CA0 RID: 11424 RVA: 0x000C152B File Offset: 0x000BF72B
	public void SetUserChosenTargetState(string stageID)
	{
		this.SetDefault();
		this.userChosenTargetStage = stageID;
	}

	// Token: 0x04001E87 RID: 7815
	[Serialize]
	private string currentStage;

	// Token: 0x04001E88 RID: 7816
	[Serialize]
	private string userChosenTargetStage;

	// Token: 0x04001E89 RID: 7817
	private AttributeModifier artQualityDecorModifier;

	// Token: 0x04001E8A RID: 7818
	private string defaultArtworkId = "Default";

	// Token: 0x04001E8B RID: 7819
	public string defaultAnimName;

	// Token: 0x04001E8C RID: 7820
	private WorkChore<Artable> chore;
}
