using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02001802 RID: 6146
[AddComponentMenu("KMonoBehaviour/Workable/ResearchCenter")]
public class ResearchCenter : Workable, IGameObjectEffectDescriptor, ISim200ms, IResearchCenter
{
	// Token: 0x06007E70 RID: 32368 RVA: 0x0033708C File Offset: 0x0033528C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Researching;
		this.attributeConverter = Db.Get().AttributeConverters.ResearchSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.ALL_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Research.Id;
		this.skillExperienceMultiplier = SKILLS.ALL_DAY_EXPERIENCE;
		ElementConverter elementConverter = this.elementConverter;
		elementConverter.onConvertMass = (Action<float>)Delegate.Combine(elementConverter.onConvertMass, new Action<float>(this.ConvertMassToResearchPoints));
	}

	// Token: 0x06007E71 RID: 32369 RVA: 0x00337120 File Offset: 0x00335320
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<ResearchCenter>(-1914338957, ResearchCenter.UpdateWorkingStateDelegate);
		base.Subscribe<ResearchCenter>(-125623018, ResearchCenter.UpdateWorkingStateDelegate);
		base.Subscribe<ResearchCenter>(187661686, ResearchCenter.UpdateWorkingStateDelegate);
		base.Subscribe<ResearchCenter>(-1697596308, ResearchCenter.CheckHasMaterialDelegate);
		Components.ResearchCenters.Add(this);
		this.UpdateWorkingState(null);
	}

	// Token: 0x06007E72 RID: 32370 RVA: 0x0033718C File Offset: 0x0033538C
	private void ConvertMassToResearchPoints(float mass_consumed)
	{
		this.remainder_mass_points += mass_consumed / this.mass_per_point - (float)Mathf.FloorToInt(mass_consumed / this.mass_per_point);
		int num = Mathf.FloorToInt(mass_consumed / this.mass_per_point);
		num += Mathf.FloorToInt(this.remainder_mass_points);
		this.remainder_mass_points -= (float)Mathf.FloorToInt(this.remainder_mass_points);
		ResearchType researchType = Research.Instance.GetResearchType(this.research_point_type_id);
		if (num > 0)
		{
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Research, researchType.name, base.transform, 1.5f, false);
			for (int i = 0; i < num; i++)
			{
				Research.Instance.AddResearchPoints(this.research_point_type_id, 1f);
			}
		}
	}

	// Token: 0x06007E73 RID: 32371 RVA: 0x00337250 File Offset: 0x00335450
	public void Sim200ms(float dt)
	{
		if (!this.operational.IsActive && this.operational.IsOperational && this.chore == null && this.HasMaterial())
		{
			this.chore = this.CreateChore();
			base.SetWorkTime(float.PositiveInfinity);
		}
	}

	// Token: 0x06007E74 RID: 32372 RVA: 0x003372A0 File Offset: 0x003354A0
	protected virtual Chore CreateChore()
	{
		return new WorkChore<ResearchCenter>(Db.Get().ChoreTypes.Research, this, null, true, null, null, null, true, null, false, true, null, true, true, true, PriorityScreen.PriorityClass.basic, 5, false, true)
		{
			preemption_cb = new Func<Chore.Precondition.Context, bool>(ResearchCenter.CanPreemptCB)
		};
	}

	// Token: 0x06007E75 RID: 32373 RVA: 0x003372E8 File Offset: 0x003354E8
	private static bool CanPreemptCB(Chore.Precondition.Context context)
	{
		WorkerBase component = context.chore.driver.GetComponent<WorkerBase>();
		float num = Db.Get().AttributeConverters.ResearchSpeed.Lookup(component).Evaluate();
		WorkerBase worker = context.consumerState.worker;
		return Db.Get().AttributeConverters.ResearchSpeed.Lookup(worker).Evaluate() > num && context.chore.gameObject.GetComponent<ResearchCenter>().GetPercentComplete() < 1f;
	}

	// Token: 0x06007E76 RID: 32374 RVA: 0x00337368 File Offset: 0x00335568
	public override float GetPercentComplete()
	{
		if (Research.Instance.GetActiveResearch() == null)
		{
			return 0f;
		}
		float num = Research.Instance.GetActiveResearch().progressInventory.PointsByTypeID[this.research_point_type_id];
		float num2 = 0f;
		if (!Research.Instance.GetActiveResearch().tech.costsByResearchTypeID.TryGetValue(this.research_point_type_id, out num2))
		{
			return 1f;
		}
		return num / num2;
	}

	// Token: 0x06007E77 RID: 32375 RVA: 0x000F7B89 File Offset: 0x000F5D89
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.ComplexFabricatorResearching, this);
		this.operational.SetActive(true, false);
	}

	// Token: 0x06007E78 RID: 32376 RVA: 0x003373DC File Offset: 0x003355DC
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		float efficiencyMultiplier = this.GetEfficiencyMultiplier(worker);
		float num = 2f + efficiencyMultiplier;
		if (Game.Instance.FastWorkersModeActive)
		{
			num *= 2f;
		}
		this.elementConverter.SetWorkSpeedMultiplier(num);
		return base.OnWorkTick(worker, dt);
	}

	// Token: 0x06007E79 RID: 32377 RVA: 0x000F7BBB File Offset: 0x000F5DBB
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		base.ShowProgressBar(false);
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.ComplexFabricatorResearching, this);
		this.operational.SetActive(false, false);
	}

	// Token: 0x06007E7A RID: 32378 RVA: 0x00337424 File Offset: 0x00335624
	protected bool ResearchComponentCompleted()
	{
		TechInstance activeResearch = Research.Instance.GetActiveResearch();
		if (activeResearch != null)
		{
			float num = 0f;
			float num2 = 0f;
			activeResearch.progressInventory.PointsByTypeID.TryGetValue(this.research_point_type_id, out num);
			activeResearch.tech.costsByResearchTypeID.TryGetValue(this.research_point_type_id, out num2);
			if (num >= num2)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06007E7B RID: 32379 RVA: 0x0031D160 File Offset: 0x0031B360
	protected bool IsAllResearchComplete()
	{
		using (List<Tech>.Enumerator enumerator = Db.Get().Techs.resources.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (!enumerator.Current.IsComplete())
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x06007E7C RID: 32380 RVA: 0x00337484 File Offset: 0x00335684
	protected virtual void UpdateWorkingState(object data)
	{
		bool flag = false;
		bool flag2 = false;
		TechInstance activeResearch = Research.Instance.GetActiveResearch();
		if (activeResearch != null)
		{
			flag = true;
			if (activeResearch.tech.costsByResearchTypeID.ContainsKey(this.research_point_type_id) && Research.Instance.Get(activeResearch.tech).progressInventory.PointsByTypeID[this.research_point_type_id] < activeResearch.tech.costsByResearchTypeID[this.research_point_type_id])
			{
				flag2 = true;
			}
		}
		if (this.operational.GetFlag(EnergyConsumer.PoweredFlag) && !this.IsAllResearchComplete())
		{
			if (flag)
			{
				base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.NoResearchSelected, false);
				if (!flag2 && !this.ResearchComponentCompleted())
				{
					base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.NoResearchSelected, false);
					base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.NoApplicableResearchSelected, null);
				}
				else
				{
					base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.NoApplicableResearchSelected, false);
				}
			}
			else
			{
				base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.NoResearchSelected, null);
				base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.NoApplicableResearchSelected, false);
			}
		}
		else
		{
			base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.NoResearchSelected, false);
			base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.NoApplicableResearchSelected, false);
		}
		this.operational.SetFlag(ResearchCenter.ResearchSelectedFlag, flag && flag2);
		if ((!flag || !flag2) && base.worker)
		{
			base.StopWork(base.worker, true);
		}
	}

	// Token: 0x06007E7D RID: 32381 RVA: 0x000F7BF9 File Offset: 0x000F5DF9
	private void ClearResearchScreen()
	{
		Game.Instance.Trigger(-1974454597, null);
	}

	// Token: 0x06007E7E RID: 32382 RVA: 0x000F7C0B File Offset: 0x000F5E0B
	public string GetResearchType()
	{
		return this.research_point_type_id;
	}

	// Token: 0x06007E7F RID: 32383 RVA: 0x000F7C13 File Offset: 0x000F5E13
	private void CheckHasMaterial(object o = null)
	{
		if (!this.HasMaterial() && this.chore != null)
		{
			this.chore.Cancel("No material remaining");
			this.chore = null;
		}
	}

	// Token: 0x06007E80 RID: 32384 RVA: 0x000F7C3C File Offset: 0x000F5E3C
	private bool HasMaterial()
	{
		return this.storage.MassStored() > 0f;
	}

	// Token: 0x06007E81 RID: 32385 RVA: 0x0033764C File Offset: 0x0033584C
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Research.Instance.Unsubscribe(-1914338957, new Action<object>(this.UpdateWorkingState));
		Research.Instance.Unsubscribe(-125623018, new Action<object>(this.UpdateWorkingState));
		base.Unsubscribe(-1852328367, new Action<object>(this.UpdateWorkingState));
		Components.ResearchCenters.Remove(this);
		this.ClearResearchScreen();
	}

	// Token: 0x06007E82 RID: 32386 RVA: 0x003376C0 File Offset: 0x003358C0
	public string GetStatusString()
	{
		string text = RESEARCH.MESSAGING.NORESEARCHSELECTED;
		if (Research.Instance.GetActiveResearch() != null)
		{
			text = "<b>" + Research.Instance.GetActiveResearch().tech.Name + "</b>";
			int num = 0;
			foreach (KeyValuePair<string, float> keyValuePair in Research.Instance.GetActiveResearch().progressInventory.PointsByTypeID)
			{
				if (Research.Instance.GetActiveResearch().tech.costsByResearchTypeID[keyValuePair.Key] != 0f)
				{
					num++;
				}
			}
			foreach (KeyValuePair<string, float> keyValuePair2 in Research.Instance.GetActiveResearch().progressInventory.PointsByTypeID)
			{
				if (Research.Instance.GetActiveResearch().tech.costsByResearchTypeID[keyValuePair2.Key] != 0f && keyValuePair2.Key == this.research_point_type_id)
				{
					text = text + "\n   - " + Research.Instance.researchTypes.GetResearchType(keyValuePair2.Key).name;
					text = string.Concat(new string[]
					{
						text,
						": ",
						keyValuePair2.Value.ToString(),
						"/",
						Research.Instance.GetActiveResearch().tech.costsByResearchTypeID[keyValuePair2.Key].ToString()
					});
				}
			}
			foreach (KeyValuePair<string, float> keyValuePair3 in Research.Instance.GetActiveResearch().progressInventory.PointsByTypeID)
			{
				if (Research.Instance.GetActiveResearch().tech.costsByResearchTypeID[keyValuePair3.Key] != 0f && !(keyValuePair3.Key == this.research_point_type_id))
				{
					if (num > 1)
					{
						text = text + "\n   - " + string.Format(RESEARCH.MESSAGING.RESEARCHTYPEALSOREQUIRED, Research.Instance.researchTypes.GetResearchType(keyValuePair3.Key).name);
					}
					else
					{
						text = text + "\n   - " + string.Format(RESEARCH.MESSAGING.RESEARCHTYPEREQUIRED, Research.Instance.researchTypes.GetResearchType(keyValuePair3.Key).name);
					}
				}
			}
		}
		return text;
	}

	// Token: 0x06007E83 RID: 32387 RVA: 0x003379A0 File Offset: 0x00335BA0
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> descriptors = base.GetDescriptors(go);
		descriptors.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.RESEARCH_MATERIALS, this.inputMaterial.ProperName(), GameUtil.GetFormattedByTag(this.inputMaterial, this.mass_per_point, GameUtil.TimeSlice.None)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.RESEARCH_MATERIALS, this.inputMaterial.ProperName(), GameUtil.GetFormattedByTag(this.inputMaterial, this.mass_per_point, GameUtil.TimeSlice.None)), Descriptor.DescriptorType.Requirement, false));
		descriptors.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.PRODUCES_RESEARCH_POINTS, Research.Instance.researchTypes.GetResearchType(this.research_point_type_id).name), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.PRODUCES_RESEARCH_POINTS, Research.Instance.researchTypes.GetResearchType(this.research_point_type_id).name), Descriptor.DescriptorType.Effect, false));
		return descriptors;
	}

	// Token: 0x06007E84 RID: 32388 RVA: 0x000B1628 File Offset: 0x000AF828
	public override bool InstantlyFinish(WorkerBase worker)
	{
		return false;
	}

	// Token: 0x04006017 RID: 24599
	private Chore chore;

	// Token: 0x04006018 RID: 24600
	[MyCmpAdd]
	protected Notifier notifier;

	// Token: 0x04006019 RID: 24601
	[MyCmpAdd]
	protected Operational operational;

	// Token: 0x0400601A RID: 24602
	[MyCmpAdd]
	protected Storage storage;

	// Token: 0x0400601B RID: 24603
	[MyCmpGet]
	private ElementConverter elementConverter;

	// Token: 0x0400601C RID: 24604
	[SerializeField]
	public string research_point_type_id;

	// Token: 0x0400601D RID: 24605
	[SerializeField]
	public Tag inputMaterial;

	// Token: 0x0400601E RID: 24606
	[SerializeField]
	public float mass_per_point;

	// Token: 0x0400601F RID: 24607
	[SerializeField]
	private float remainder_mass_points;

	// Token: 0x04006020 RID: 24608
	public static readonly Operational.Flag ResearchSelectedFlag = new Operational.Flag("researchSelected", Operational.Flag.Type.Requirement);

	// Token: 0x04006021 RID: 24609
	private static readonly EventSystem.IntraObjectHandler<ResearchCenter> UpdateWorkingStateDelegate = new EventSystem.IntraObjectHandler<ResearchCenter>(delegate(ResearchCenter component, object data)
	{
		component.UpdateWorkingState(data);
	});

	// Token: 0x04006022 RID: 24610
	private static readonly EventSystem.IntraObjectHandler<ResearchCenter> CheckHasMaterialDelegate = new EventSystem.IntraObjectHandler<ResearchCenter>(delegate(ResearchCenter component, object data)
	{
		component.CheckHasMaterial(data);
	});
}
