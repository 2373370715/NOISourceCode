using System;
using TUNING;
using UnityEngine;

// Token: 0x02000DD2 RID: 3538
public class GeneticAnalysisStationWorkable : Workable
{
	// Token: 0x060044F1 RID: 17649 RVA: 0x00257B64 File Offset: 0x00255D64
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.requiredSkillPerk = Db.Get().SkillPerks.CanIdentifyMutantSeeds.Id;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.AnalyzingGenes;
		this.attributeConverter = Db.Get().AttributeConverters.ResearchSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Research.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_genetic_analysisstation_kanim")
		};
		base.SetWorkTime(150f);
		this.showProgressBar = true;
		this.lightEfficiencyBonus = true;
	}

	// Token: 0x060044F2 RID: 17650 RVA: 0x000D107E File Offset: 0x000CF27E
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.ComplexFabricatorResearching, this.storage.FindFirst(GameTags.UnidentifiedSeed));
	}

	// Token: 0x060044F3 RID: 17651 RVA: 0x000D10B2 File Offset: 0x000CF2B2
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.ComplexFabricatorResearching, false);
	}

	// Token: 0x060044F4 RID: 17652 RVA: 0x000D10D7 File Offset: 0x000CF2D7
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		this.IdentifyMutant();
	}

	// Token: 0x060044F5 RID: 17653 RVA: 0x00257C24 File Offset: 0x00255E24
	public void IdentifyMutant()
	{
		GameObject gameObject = this.storage.FindFirst(GameTags.UnidentifiedSeed);
		DebugUtil.DevAssertArgs(gameObject != null, new object[]
		{
			"AAACCCCKKK!! GeneticAnalysisStation finished studying a seed but we don't have one in storage??"
		});
		if (gameObject != null)
		{
			Pickupable component = gameObject.GetComponent<Pickupable>();
			Pickupable pickupable;
			if (component.PrimaryElement.Units > 1f)
			{
				pickupable = component.Take(1f);
			}
			else
			{
				pickupable = this.storage.Drop(gameObject, true).GetComponent<Pickupable>();
			}
			pickupable.transform.SetPosition(base.transform.GetPosition() + this.finishedSeedDropOffset);
			MutantPlant component2 = pickupable.GetComponent<MutantPlant>();
			PlantSubSpeciesCatalog.Instance.IdentifySubSpecies(component2.SubSpeciesID);
			component2.Analyze();
			SaveGame.Instance.ColonyAchievementTracker.LogAnalyzedSeed(component2.SpeciesID);
		}
	}

	// Token: 0x04002FD9 RID: 12249
	[MyCmpAdd]
	public Notifier notifier;

	// Token: 0x04002FDA RID: 12250
	[MyCmpReq]
	public Storage storage;

	// Token: 0x04002FDB RID: 12251
	[SerializeField]
	public Vector3 finishedSeedDropOffset;

	// Token: 0x04002FDC RID: 12252
	private Notification notification;

	// Token: 0x04002FDD RID: 12253
	public GeneticAnalysisStation.StatesInstance statesInstance;
}
