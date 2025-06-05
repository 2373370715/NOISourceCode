using System;
using System.Collections.Generic;
using Database;
using Klei;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200102C RID: 4140
[AddComponentMenu("KMonoBehaviour/Workable/Telescope")]
public class Telescope : Workable, OxygenBreather.IGasProvider, IGameObjectEffectDescriptor, ISim200ms, BuildingStatusItems.ISkyVisInfo
{
	// Token: 0x060053B5 RID: 21429 RVA: 0x000DAF43 File Offset: 0x000D9143
	float BuildingStatusItems.ISkyVisInfo.GetPercentVisible01()
	{
		return this.percentClear;
	}

	// Token: 0x060053B6 RID: 21430 RVA: 0x002875A4 File Offset: 0x002857A4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.attributeConverter = Db.Get().AttributeConverters.ResearchSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.ALL_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Research.Id;
		this.skillExperienceMultiplier = SKILLS.ALL_DAY_EXPERIENCE;
	}

	// Token: 0x060053B7 RID: 21431 RVA: 0x002875FC File Offset: 0x002857FC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		SpacecraftManager.instance.Subscribe(532901469, new Action<object>(this.UpdateWorkingState));
		Components.Telescopes.Add(this);
		this.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(this.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(this.OnWorkableEvent));
		this.operational = base.GetComponent<Operational>();
		this.storage = base.GetComponent<Storage>();
		this.UpdateWorkingState(null);
	}

	// Token: 0x060053B8 RID: 21432 RVA: 0x000DAF4B File Offset: 0x000D914B
	protected override void OnCleanUp()
	{
		Components.Telescopes.Remove(this);
		SpacecraftManager.instance.Unsubscribe(532901469, new Action<object>(this.UpdateWorkingState));
		base.OnCleanUp();
	}

	// Token: 0x060053B9 RID: 21433 RVA: 0x00287678 File Offset: 0x00285878
	public void Sim200ms(float dt)
	{
		base.GetComponent<Building>().GetExtents();
		ValueTuple<bool, float> visibilityOf = TelescopeConfig.SKY_VISIBILITY_INFO.GetVisibilityOf(base.gameObject);
		bool item = visibilityOf.Item1;
		float item2 = visibilityOf.Item2;
		this.percentClear = item2;
		KSelectable component = base.GetComponent<KSelectable>();
		component.ToggleStatusItem(Db.Get().BuildingStatusItems.SkyVisNone, !item, this);
		component.ToggleStatusItem(Db.Get().BuildingStatusItems.SkyVisLimited, item && item2 < 1f, this);
		Operational component2 = base.GetComponent<Operational>();
		component2.SetFlag(Telescope.visibleSkyFlag, item);
		if (!component2.IsActive && component2.IsOperational && this.chore == null)
		{
			this.chore = this.CreateChore();
			base.SetWorkTime(float.PositiveInfinity);
		}
	}

	// Token: 0x060053BA RID: 21434 RVA: 0x0028773C File Offset: 0x0028593C
	private void OnWorkableEvent(Workable workable, Workable.WorkableEvent ev)
	{
		WorkerBase worker = base.worker;
		if (worker == null)
		{
			return;
		}
		OxygenBreather component = worker.GetComponent<OxygenBreather>();
		KPrefabID component2 = worker.GetComponent<KPrefabID>();
		KSelectable component3 = base.GetComponent<KSelectable>();
		if (ev == Workable.WorkableEvent.WorkStarted)
		{
			base.ShowProgressBar(true);
			this.progressBar.SetUpdateFunc(delegate
			{
				if (SpacecraftManager.instance.HasAnalysisTarget())
				{
					return SpacecraftManager.instance.GetDestinationAnalysisScore(SpacecraftManager.instance.GetStarmapAnalysisDestinationID()) / (float)ROCKETRY.DESTINATION_ANALYSIS.COMPLETE;
				}
				return 0f;
			});
			if (component != null)
			{
				component.AddGasProvider(this);
			}
			worker.GetComponent<CreatureSimTemperatureTransfer>().enabled = false;
			component2.AddTag(GameTags.Shaded, false);
			component3.AddStatusItem(Db.Get().BuildingStatusItems.TelescopeWorking, this);
			return;
		}
		if (ev != Workable.WorkableEvent.WorkStopped)
		{
			return;
		}
		if (component != null)
		{
			component.RemoveGasProvider(this);
		}
		worker.GetComponent<CreatureSimTemperatureTransfer>().enabled = true;
		base.ShowProgressBar(false);
		component2.RemoveTag(GameTags.Shaded);
		component3.AddStatusItem(Db.Get().BuildingStatusItems.TelescopeWorking, this);
	}

	// Token: 0x060053BB RID: 21435 RVA: 0x000DAF79 File Offset: 0x000D9179
	public override float GetEfficiencyMultiplier(WorkerBase worker)
	{
		return base.GetEfficiencyMultiplier(worker) * Mathf.Clamp01(this.percentClear);
	}

	// Token: 0x060053BC RID: 21436 RVA: 0x00287830 File Offset: 0x00285A30
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		if (SpacecraftManager.instance.HasAnalysisTarget())
		{
			int starmapAnalysisDestinationID = SpacecraftManager.instance.GetStarmapAnalysisDestinationID();
			SpaceDestination destination = SpacecraftManager.instance.GetDestination(starmapAnalysisDestinationID);
			float num = 1f / (float)destination.OneBasedDistance;
			float num2 = (float)ROCKETRY.DESTINATION_ANALYSIS.DISCOVERED;
			float default_CYCLES_PER_DISCOVERY = ROCKETRY.DESTINATION_ANALYSIS.DEFAULT_CYCLES_PER_DISCOVERY;
			float num3 = num2 / default_CYCLES_PER_DISCOVERY / 600f;
			float points = dt * num * num3;
			SpacecraftManager.instance.EarnDestinationAnalysisPoints(starmapAnalysisDestinationID, points);
		}
		return base.OnWorkTick(worker, dt);
	}

	// Token: 0x060053BD RID: 21437 RVA: 0x00248884 File Offset: 0x00246A84
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> descriptors = base.GetDescriptors(go);
		Element element = ElementLoader.FindElementByHash(SimHashes.Oxygen);
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(element.tag.ProperName(), string.Format(STRINGS.BUILDINGS.PREFABS.TELESCOPE.REQUIREMENT_TOOLTIP, element.tag.ProperName()), Descriptor.DescriptorType.Requirement);
		descriptors.Add(item);
		return descriptors;
	}

	// Token: 0x060053BE RID: 21438 RVA: 0x002878A4 File Offset: 0x00285AA4
	protected Chore CreateChore()
	{
		WorkChore<Telescope> workChore = new WorkChore<Telescope>(Db.Get().ChoreTypes.Research, this, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		workChore.AddPrecondition(Telescope.ContainsOxygen, null);
		return workChore;
	}

	// Token: 0x060053BF RID: 21439 RVA: 0x002878E4 File Offset: 0x00285AE4
	protected void UpdateWorkingState(object data)
	{
		bool flag = false;
		if (SpacecraftManager.instance.HasAnalysisTarget() && SpacecraftManager.instance.GetDestinationAnalysisState(SpacecraftManager.instance.GetDestination(SpacecraftManager.instance.GetStarmapAnalysisDestinationID())) != SpacecraftManager.DestinationAnalysisState.Complete)
		{
			flag = true;
		}
		KSelectable component = base.GetComponent<KSelectable>();
		bool on = !flag && !SpacecraftManager.instance.AreAllDestinationsAnalyzed();
		component.ToggleStatusItem(Db.Get().BuildingStatusItems.NoApplicableAnalysisSelected, on, null);
		this.operational.SetFlag(Telescope.flag, flag);
		if (!flag && base.worker)
		{
			base.StopWork(base.worker, true);
		}
	}

	// Token: 0x060053C0 RID: 21440 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSetOxygenBreather(OxygenBreather oxygen_breather)
	{
	}

	// Token: 0x060053C1 RID: 21441 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnClearOxygenBreather(OxygenBreather oxygen_breather)
	{
	}

	// Token: 0x060053C2 RID: 21442 RVA: 0x000B1628 File Offset: 0x000AF828
	public bool ShouldEmitCO2()
	{
		return false;
	}

	// Token: 0x060053C3 RID: 21443 RVA: 0x000B1628 File Offset: 0x000AF828
	public bool ShouldStoreCO2()
	{
		return false;
	}

	// Token: 0x060053C4 RID: 21444 RVA: 0x00287984 File Offset: 0x00285B84
	public bool ConsumeGas(OxygenBreather oxygen_breather, float amount)
	{
		if (this.storage.items.Count <= 0)
		{
			return false;
		}
		GameObject gameObject = this.storage.items[0];
		if (gameObject == null)
		{
			return false;
		}
		float mass = gameObject.GetComponent<PrimaryElement>().Mass;
		float num = 0f;
		float temperature = 0f;
		SimHashes elementConsumed = SimHashes.Vacuum;
		SimUtil.DiseaseInfo diseaseInfo;
		this.storage.ConsumeAndGetDisease(GameTags.Breathable, amount, out num, out diseaseInfo, out temperature, out elementConsumed);
		bool result = num >= amount;
		OxygenBreather.BreathableGasConsumed(oxygen_breather, elementConsumed, num, temperature, diseaseInfo.idx, diseaseInfo.count);
		return result;
	}

	// Token: 0x060053C5 RID: 21445 RVA: 0x00287A18 File Offset: 0x00285C18
	public bool IsLowOxygen()
	{
		if (this.storage.items.Count <= 0)
		{
			return true;
		}
		PrimaryElement primaryElement = this.storage.FindFirstWithMass(GameTags.Breathable, 0f);
		return primaryElement == null || primaryElement.Mass == 0f;
	}

	// Token: 0x060053C6 RID: 21446 RVA: 0x00287A68 File Offset: 0x00285C68
	public bool HasOxygen()
	{
		if (this.storage.items.Count <= 0)
		{
			return true;
		}
		PrimaryElement primaryElement = this.storage.FindFirstWithMass(GameTags.Breathable, 0f);
		return primaryElement != null && primaryElement.Mass > 0f;
	}

	// Token: 0x060053C7 RID: 21447 RVA: 0x000B1628 File Offset: 0x000AF828
	public bool IsBlocked()
	{
		return false;
	}

	// Token: 0x04003B0B RID: 15115
	private Operational operational;

	// Token: 0x04003B0C RID: 15116
	private float percentClear;

	// Token: 0x04003B0D RID: 15117
	private static readonly Operational.Flag visibleSkyFlag = new Operational.Flag("VisibleSky", Operational.Flag.Type.Requirement);

	// Token: 0x04003B0E RID: 15118
	private Storage storage;

	// Token: 0x04003B0F RID: 15119
	public static readonly Chore.Precondition ContainsOxygen = new Chore.Precondition
	{
		id = "ContainsOxygen",
		sortOrder = 1,
		description = DUPLICANTS.CHORES.PRECONDITIONS.CONTAINS_OXYGEN,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return context.chore.target.GetComponent<Storage>().FindFirstWithMass(GameTags.Oxygen, 0f) != null;
		}
	};

	// Token: 0x04003B10 RID: 15120
	private Chore chore;

	// Token: 0x04003B11 RID: 15121
	private static readonly Operational.Flag flag = new Operational.Flag("ValidTarget", Operational.Flag.Type.Requirement);
}
