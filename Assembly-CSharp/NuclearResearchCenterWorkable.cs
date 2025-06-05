using System;
using KSerialization;
using TUNING;
using UnityEngine;

// Token: 0x020016C0 RID: 5824
public class NuclearResearchCenterWorkable : Workable
{
	// Token: 0x06007820 RID: 30752 RVA: 0x0031D3B0 File Offset: 0x0031B5B0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Researching;
		this.attributeConverter = Db.Get().AttributeConverters.ResearchSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.ALL_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Research.Id;
		this.skillExperienceMultiplier = SKILLS.ALL_DAY_EXPERIENCE;
		this.radiationStorage = base.GetComponent<HighEnergyParticleStorage>();
		this.nrc = base.GetComponent<NuclearResearchCenter>();
		this.lightEfficiencyBonus = true;
	}

	// Token: 0x06007821 RID: 30753 RVA: 0x000D6E7F File Offset: 0x000D507F
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.SetWorkTime(float.PositiveInfinity);
	}

	// Token: 0x06007822 RID: 30754 RVA: 0x0031D43C File Offset: 0x0031B63C
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		float num = dt / this.nrc.timePerPoint;
		if (Game.Instance.FastWorkersModeActive)
		{
			num *= 2f;
		}
		this.radiationStorage.ConsumeAndGet(num * this.nrc.materialPerPoint);
		this.pointsProduced += num;
		if (this.pointsProduced >= 1f)
		{
			int num2 = Mathf.FloorToInt(this.pointsProduced);
			this.pointsProduced -= (float)num2;
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Research, Research.Instance.GetResearchType("nuclear").name, base.transform, 1.5f, false);
			Research.Instance.AddResearchPoints("nuclear", (float)num2);
		}
		TechInstance activeResearch = Research.Instance.GetActiveResearch();
		return this.radiationStorage.IsEmpty() || activeResearch == null || activeResearch.PercentageCompleteResearchType("nuclear") >= 1f;
	}

	// Token: 0x06007823 RID: 30755 RVA: 0x000F384C File Offset: 0x000F1A4C
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.ComplexFabricatorResearching, this.nrc);
	}

	// Token: 0x06007824 RID: 30756 RVA: 0x000F3876 File Offset: 0x000F1A76
	protected override void OnAbortWork(WorkerBase worker)
	{
		base.OnAbortWork(worker);
	}

	// Token: 0x06007825 RID: 30757 RVA: 0x000F387F File Offset: 0x000F1A7F
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.ComplexFabricatorResearching, this.nrc);
	}

	// Token: 0x06007826 RID: 30758 RVA: 0x0031D534 File Offset: 0x0031B734
	public override float GetPercentComplete()
	{
		if (Research.Instance.GetActiveResearch() == null)
		{
			return 0f;
		}
		float num = Research.Instance.GetActiveResearch().progressInventory.PointsByTypeID["nuclear"];
		float num2 = 0f;
		if (!Research.Instance.GetActiveResearch().tech.costsByResearchTypeID.TryGetValue("nuclear", out num2))
		{
			return 1f;
		}
		return num / num2;
	}

	// Token: 0x06007827 RID: 30759 RVA: 0x000B1628 File Offset: 0x000AF828
	public override bool InstantlyFinish(WorkerBase worker)
	{
		return false;
	}

	// Token: 0x04005A4F RID: 23119
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04005A50 RID: 23120
	[Serialize]
	private float pointsProduced;

	// Token: 0x04005A51 RID: 23121
	private NuclearResearchCenter nrc;

	// Token: 0x04005A52 RID: 23122
	private HighEnergyParticleStorage radiationStorage;
}
