using System;
using KSerialization;
using TUNING;
using UnityEngine;

// Token: 0x02000CD3 RID: 3283
public class ArtifactAnalysisStationWorkable : Workable
{
	// Token: 0x06003EA2 RID: 16034 RVA: 0x00243384 File Offset: 0x00241584
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.requiredSkillPerk = Db.Get().SkillPerks.CanStudyArtifact.Id;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.AnalyzingArtifact;
		this.attributeConverter = Db.Get().AttributeConverters.ArtSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Research.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_artifact_analysis_kanim")
		};
		base.SetWorkTime(150f);
		this.showProgressBar = true;
		this.lightEfficiencyBonus = true;
		Components.ArtifactAnalysisStations.Add(this);
	}

	// Token: 0x06003EA3 RID: 16035 RVA: 0x000CD219 File Offset: 0x000CB419
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.animController = base.GetComponent<KBatchedAnimController>();
		this.animController.SetSymbolVisiblity("snapTo_artifact", false);
	}

	// Token: 0x06003EA4 RID: 16036 RVA: 0x000CD243 File Offset: 0x000CB443
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.ArtifactAnalysisStations.Remove(this);
	}

	// Token: 0x06003EA5 RID: 16037 RVA: 0x000CD256 File Offset: 0x000CB456
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		this.InitialDisplayStoredArtifact();
	}

	// Token: 0x06003EA6 RID: 16038 RVA: 0x000CD265 File Offset: 0x000CB465
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		this.PositionArtifact();
		return base.OnWorkTick(worker, dt);
	}

	// Token: 0x06003EA7 RID: 16039 RVA: 0x00243450 File Offset: 0x00241650
	private void InitialDisplayStoredArtifact()
	{
		GameObject gameObject = base.GetComponent<Storage>().GetItems()[0];
		KBatchedAnimController component = gameObject.GetComponent<KBatchedAnimController>();
		if (component != null)
		{
			component.GetBatchInstanceData().ClearOverrideTransformMatrix();
		}
		gameObject.transform.SetPosition(new Vector3(base.transform.position.x, base.transform.position.y, Grid.GetLayerZ(Grid.SceneLayer.BuildingBack)));
		gameObject.SetActive(true);
		component.enabled = false;
		component.enabled = true;
		this.PositionArtifact();
		base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.ArtifactAnalysisAnalyzing, gameObject);
	}

	// Token: 0x06003EA8 RID: 16040 RVA: 0x002434FC File Offset: 0x002416FC
	private void ReleaseStoredArtifact()
	{
		Storage component = base.GetComponent<Storage>();
		GameObject gameObject = component.GetItems()[0];
		KBatchedAnimController component2 = gameObject.GetComponent<KBatchedAnimController>();
		gameObject.transform.SetPosition(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, Grid.GetLayerZ(Grid.SceneLayer.Ore)));
		component2.enabled = false;
		component2.enabled = true;
		component.Drop(gameObject, true);
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.ArtifactAnalysisAnalyzing, gameObject);
	}

	// Token: 0x06003EA9 RID: 16041 RVA: 0x00243590 File Offset: 0x00241790
	private void PositionArtifact()
	{
		GameObject gameObject = base.GetComponent<Storage>().GetItems()[0];
		bool flag;
		Vector3 position = this.animController.GetSymbolTransform("snapTo_artifact", out flag).GetColumn(3);
		position.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingBack);
		gameObject.transform.SetPosition(position);
	}

	// Token: 0x06003EAA RID: 16042 RVA: 0x000CD275 File Offset: 0x000CB475
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		this.ConsumeCharm();
		this.ReleaseStoredArtifact();
	}

	// Token: 0x06003EAB RID: 16043 RVA: 0x002435F0 File Offset: 0x002417F0
	private void ConsumeCharm()
	{
		GameObject gameObject = this.storage.FindFirst(GameTags.CharmedArtifact);
		DebugUtil.DevAssertArgs(gameObject != null, new object[]
		{
			"ArtifactAnalysisStation finished studying a charmed artifact but there is not one in its storage"
		});
		if (gameObject != null)
		{
			this.YieldPayload(gameObject.GetComponent<SpaceArtifact>());
			gameObject.GetComponent<SpaceArtifact>().RemoveCharm();
		}
		if (ArtifactSelector.Instance.RecordArtifactAnalyzed(gameObject.GetComponent<KPrefabID>().PrefabID().ToString()))
		{
			if (gameObject.HasTag(GameTags.TerrestrialArtifact))
			{
				ArtifactSelector.Instance.IncrementAnalyzedTerrestrialArtifacts();
				return;
			}
			ArtifactSelector.Instance.IncrementAnalyzedSpaceArtifacts();
		}
	}

	// Token: 0x06003EAC RID: 16044 RVA: 0x00243690 File Offset: 0x00241890
	private void YieldPayload(SpaceArtifact artifact)
	{
		if (this.nextYeildRoll == -1f)
		{
			this.nextYeildRoll = UnityEngine.Random.Range(0f, 1f);
		}
		if (this.nextYeildRoll <= artifact.GetArtifactTier().payloadDropChance)
		{
			GameUtil.KInstantiate(Assets.GetPrefab("GeneShufflerRecharge"), this.statesInstance.master.transform.position + this.finishedArtifactDropOffset, Grid.SceneLayer.Ore, null, 0).SetActive(true);
		}
		int num = Mathf.FloorToInt(artifact.GetArtifactTier().payloadDropChance * 20f);
		for (int i = 0; i < num; i++)
		{
			GameUtil.KInstantiate(Assets.GetPrefab("OrbitalResearchDatabank"), this.statesInstance.master.transform.position + this.finishedArtifactDropOffset, Grid.SceneLayer.Ore, null, 0).SetActive(true);
		}
		this.nextYeildRoll = UnityEngine.Random.Range(0f, 1f);
	}

	// Token: 0x04002B54 RID: 11092
	[MyCmpAdd]
	public Notifier notifier;

	// Token: 0x04002B55 RID: 11093
	[MyCmpReq]
	public Storage storage;

	// Token: 0x04002B56 RID: 11094
	[SerializeField]
	public Vector3 finishedArtifactDropOffset;

	// Token: 0x04002B57 RID: 11095
	private Notification notification;

	// Token: 0x04002B58 RID: 11096
	public ArtifactAnalysisStation.StatesInstance statesInstance;

	// Token: 0x04002B59 RID: 11097
	private KBatchedAnimController animController;

	// Token: 0x04002B5A RID: 11098
	[Serialize]
	private float nextYeildRoll = -1f;
}
