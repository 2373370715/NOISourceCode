using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02001A3D RID: 6717
[AddComponentMenu("KMonoBehaviour/Workable/TinkerStation")]
public class TinkerStation : Workable, IGameObjectEffectDescriptor, ISim1000ms
{
	// Token: 0x17000920 RID: 2336
	// (set) Token: 0x06008BE4 RID: 35812 RVA: 0x000C2387 File Offset: 0x000C0587
	public AttributeConverter AttributeConverter
	{
		set
		{
			this.attributeConverter = value;
		}
	}

	// Token: 0x17000921 RID: 2337
	// (set) Token: 0x06008BE5 RID: 35813 RVA: 0x000C2398 File Offset: 0x000C0598
	public float AttributeExperienceMultiplier
	{
		set
		{
			this.attributeExperienceMultiplier = value;
		}
	}

	// Token: 0x17000922 RID: 2338
	// (set) Token: 0x06008BE6 RID: 35814 RVA: 0x000C23A1 File Offset: 0x000C05A1
	public string SkillExperienceSkillGroup
	{
		set
		{
			this.skillExperienceSkillGroup = value;
		}
	}

	// Token: 0x17000923 RID: 2339
	// (set) Token: 0x06008BE7 RID: 35815 RVA: 0x000C23AA File Offset: 0x000C05AA
	public float SkillExperienceMultiplier
	{
		set
		{
			this.skillExperienceMultiplier = value;
		}
	}

	// Token: 0x06008BE8 RID: 35816 RVA: 0x0036FC44 File Offset: 0x0036DE44
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.attributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Technicals.Id;
		this.skillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
		if (this.useFilteredStorage)
		{
			ChoreType byHash = Db.Get().ChoreTypes.GetByHash(this.fetchChoreType);
			this.filteredStorage = new FilteredStorage(this, null, null, false, byHash);
		}
		base.Subscribe<TinkerStation>(-592767678, TinkerStation.OnOperationalChangedDelegate);
	}

	// Token: 0x06008BE9 RID: 35817 RVA: 0x0010004C File Offset: 0x000FE24C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.useFilteredStorage && this.filteredStorage != null)
		{
			this.filteredStorage.FilterChanged();
		}
	}

	// Token: 0x06008BEA RID: 35818 RVA: 0x0010006F File Offset: 0x000FE26F
	protected override void OnCleanUp()
	{
		if (this.filteredStorage != null)
		{
			this.filteredStorage.CleanUp();
		}
		base.OnCleanUp();
	}

	// Token: 0x06008BEB RID: 35819 RVA: 0x0036FCDC File Offset: 0x0036DEDC
	private bool CorrectRolePrecondition(MinionIdentity worker)
	{
		MinionResume component = worker.GetComponent<MinionResume>();
		return component != null && component.HasPerk(this.requiredSkillPerk);
	}

	// Token: 0x06008BEC RID: 35820 RVA: 0x0036FD0C File Offset: 0x0036DF0C
	private void OnOperationalChanged(object data)
	{
		RoomTracker component = base.GetComponent<RoomTracker>();
		if (component != null && component.room != null)
		{
			component.room.RetriggerBuildings();
		}
	}

	// Token: 0x06008BED RID: 35821 RVA: 0x0010008A File Offset: 0x000FE28A
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		if (!this.operational.IsOperational)
		{
			return;
		}
		base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.ComplexFabricatorProducing, this);
		this.operational.SetActive(true, false);
	}

	// Token: 0x06008BEE RID: 35822 RVA: 0x001000CA File Offset: 0x000FE2CA
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		base.ShowProgressBar(false);
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.ComplexFabricatorProducing, this);
		this.operational.SetActive(false, false);
	}

	// Token: 0x06008BEF RID: 35823 RVA: 0x0036FD3C File Offset: 0x0036DF3C
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		PrimaryElement primaryElement = this.storage.FindFirstWithMass(this.inputMaterial, this.massPerTinker);
		if (primaryElement != null)
		{
			SimHashes elementID = primaryElement.ElementID;
			this.storage.ConsumeIgnoringDisease(elementID.CreateTag(), this.massPerTinker);
			GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(this.outputPrefab), base.transform.GetPosition() + Vector3.up, Grid.SceneLayer.Ore, null, 0);
			PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
			component.SetElement(elementID, true);
			component.Temperature = this.outputTemperature;
			gameObject.SetActive(true);
		}
		this.chore = null;
	}

	// Token: 0x06008BF0 RID: 35824 RVA: 0x00100108 File Offset: 0x000FE308
	public void Sim1000ms(float dt)
	{
		this.UpdateChore();
	}

	// Token: 0x06008BF1 RID: 35825 RVA: 0x0036FDE0 File Offset: 0x0036DFE0
	private void UpdateChore()
	{
		if (this.operational.IsOperational && (this.ToolsRequested() || this.alwaysTinker) && this.HasMaterial())
		{
			if (this.chore == null)
			{
				this.chore = new WorkChore<TinkerStation>(Db.Get().ChoreTypes.GetByHash(this.choreType), this, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
				this.chore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, this.requiredSkillPerk);
				base.SetWorkTime(this.toolProductionTime);
				return;
			}
		}
		else if (this.chore != null)
		{
			this.chore.Cancel("Can't tinker");
			this.chore = null;
		}
	}

	// Token: 0x06008BF2 RID: 35826 RVA: 0x00100110 File Offset: 0x000FE310
	private bool HasMaterial()
	{
		return this.storage.MassStored() > 0f;
	}

	// Token: 0x06008BF3 RID: 35827 RVA: 0x0036FE94 File Offset: 0x0036E094
	private bool ToolsRequested()
	{
		return MaterialNeeds.GetAmount(this.outputPrefab, base.gameObject.GetMyWorldId(), false) > 0f && this.GetMyWorld().worldInventory.GetAmount(this.outputPrefab, true) <= 0f;
	}

	// Token: 0x06008BF4 RID: 35828 RVA: 0x0036FEE4 File Offset: 0x0036E0E4
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		string arg = this.inputMaterial.ProperName();
		List<Descriptor> descriptors = base.GetDescriptors(go);
		descriptors.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTCONSUMEDPERUSE, arg, GameUtil.GetFormattedMass(this.massPerTinker, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMEDPERUSE, arg, GameUtil.GetFormattedMass(this.massPerTinker, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), Descriptor.DescriptorType.Requirement, false));
		descriptors.AddRange(GameUtil.GetAllDescriptors(Assets.GetPrefab(this.outputPrefab), false));
		List<Tinkerable> list = new List<Tinkerable>();
		foreach (GameObject gameObject in Assets.GetPrefabsWithComponent<Tinkerable>())
		{
			Tinkerable component = gameObject.GetComponent<Tinkerable>();
			if (component.tinkerMaterialTag == this.outputPrefab)
			{
				list.Add(component);
			}
		}
		if (list.Count > 0)
		{
			Effect effect = Db.Get().effects.Get(list[0].addedEffect);
			descriptors.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.ADDED_EFFECT, effect.Name), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ADDED_EFFECT, effect.Name, Effect.CreateTooltip(effect, true, "\n    • ", true)), Descriptor.DescriptorType.Effect, false));
			descriptors.Add(new Descriptor(this.EffectTitle, this.EffectTooltip, Descriptor.DescriptorType.Effect, false));
			foreach (Tinkerable cmp in list)
			{
				Descriptor item = new Descriptor(string.Format(this.EffectItemString, cmp.GetProperName()), string.Format(this.EffectItemTooltip, cmp.GetProperName()), Descriptor.DescriptorType.Effect, false);
				item.IncreaseIndent();
				descriptors.Add(item);
			}
		}
		return descriptors;
	}

	// Token: 0x06008BF5 RID: 35829 RVA: 0x00100124 File Offset: 0x000FE324
	public static TinkerStation AddTinkerStation(GameObject go, string required_room_type)
	{
		TinkerStation result = go.AddOrGet<TinkerStation>();
		go.AddOrGet<RoomTracker>().requiredRoomType = required_room_type;
		return result;
	}

	// Token: 0x0400699B RID: 27035
	public HashedString choreType;

	// Token: 0x0400699C RID: 27036
	public HashedString fetchChoreType;

	// Token: 0x0400699D RID: 27037
	private Chore chore;

	// Token: 0x0400699E RID: 27038
	[MyCmpAdd]
	private Operational operational;

	// Token: 0x0400699F RID: 27039
	[MyCmpAdd]
	private Storage storage;

	// Token: 0x040069A0 RID: 27040
	public bool useFilteredStorage;

	// Token: 0x040069A1 RID: 27041
	protected FilteredStorage filteredStorage;

	// Token: 0x040069A2 RID: 27042
	public float toolProductionTime = 160f;

	// Token: 0x040069A3 RID: 27043
	public bool alwaysTinker;

	// Token: 0x040069A4 RID: 27044
	public float massPerTinker;

	// Token: 0x040069A5 RID: 27045
	public Tag inputMaterial;

	// Token: 0x040069A6 RID: 27046
	public Tag outputPrefab;

	// Token: 0x040069A7 RID: 27047
	public float outputTemperature;

	// Token: 0x040069A8 RID: 27048
	public string EffectTitle = UI.BUILDINGEFFECTS.IMPROVED_BUILDINGS;

	// Token: 0x040069A9 RID: 27049
	public string EffectTooltip = UI.BUILDINGEFFECTS.TOOLTIPS.IMPROVED_BUILDINGS;

	// Token: 0x040069AA RID: 27050
	public string EffectItemString = UI.BUILDINGEFFECTS.IMPROVED_BUILDINGS_ITEM;

	// Token: 0x040069AB RID: 27051
	public string EffectItemTooltip = UI.BUILDINGEFFECTS.TOOLTIPS.IMPROVED_BUILDINGS_ITEM;

	// Token: 0x040069AC RID: 27052
	private static readonly EventSystem.IntraObjectHandler<TinkerStation> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<TinkerStation>(delegate(TinkerStation component, object data)
	{
		component.OnOperationalChanged(data);
	});
}
