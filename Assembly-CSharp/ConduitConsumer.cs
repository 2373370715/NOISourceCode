using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200110F RID: 4367
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/ConduitConsumer")]
public class ConduitConsumer : KMonoBehaviour, IConduitConsumer
{
	// Token: 0x1700054D RID: 1357
	// (get) Token: 0x06005946 RID: 22854 RVA: 0x000DE9AA File Offset: 0x000DCBAA
	public Storage Storage
	{
		get
		{
			return this.storage;
		}
	}

	// Token: 0x1700054E RID: 1358
	// (get) Token: 0x06005947 RID: 22855 RVA: 0x000DE9B2 File Offset: 0x000DCBB2
	public ConduitType ConduitType
	{
		get
		{
			return this.conduitType;
		}
	}

	// Token: 0x1700054F RID: 1359
	// (get) Token: 0x06005948 RID: 22856 RVA: 0x000DE9BA File Offset: 0x000DCBBA
	public bool IsConnected
	{
		get
		{
			return Grid.Objects[this.utilityCell, (this.conduitType == ConduitType.Gas) ? 12 : 16] != null && this.m_buildingComplete != null;
		}
	}

	// Token: 0x17000550 RID: 1360
	// (get) Token: 0x06005949 RID: 22857 RVA: 0x0029CF0C File Offset: 0x0029B10C
	public bool CanConsume
	{
		get
		{
			bool result = false;
			if (this.IsConnected)
			{
				result = (this.GetConduitManager().GetContents(this.utilityCell).mass > 0f);
			}
			return result;
		}
	}

	// Token: 0x17000551 RID: 1361
	// (get) Token: 0x0600594A RID: 22858 RVA: 0x0029CF48 File Offset: 0x0029B148
	public float stored_mass
	{
		get
		{
			if (this.storage == null)
			{
				return 0f;
			}
			if (!(this.capacityTag != GameTags.Any))
			{
				return this.storage.MassStored();
			}
			return this.storage.GetMassAvailable(this.capacityTag);
		}
	}

	// Token: 0x17000552 RID: 1362
	// (get) Token: 0x0600594B RID: 22859 RVA: 0x0029CF98 File Offset: 0x0029B198
	public float space_remaining_kg
	{
		get
		{
			float num = this.capacityKG - this.stored_mass;
			if (!(this.storage == null))
			{
				return Mathf.Min(this.storage.RemainingCapacity(), num);
			}
			return num;
		}
	}

	// Token: 0x0600594C RID: 22860 RVA: 0x000DE9F1 File Offset: 0x000DCBF1
	public void SetConduitData(ConduitType type)
	{
		this.conduitType = type;
	}

	// Token: 0x17000553 RID: 1363
	// (get) Token: 0x0600594D RID: 22861 RVA: 0x000DE9B2 File Offset: 0x000DCBB2
	public ConduitType TypeOfConduit
	{
		get
		{
			return this.conduitType;
		}
	}

	// Token: 0x17000554 RID: 1364
	// (get) Token: 0x0600594E RID: 22862 RVA: 0x000DE9FA File Offset: 0x000DCBFA
	public bool IsAlmostEmpty
	{
		get
		{
			return !this.ignoreMinMassCheck && this.MassAvailable < this.ConsumptionRate * 30f;
		}
	}

	// Token: 0x17000555 RID: 1365
	// (get) Token: 0x0600594F RID: 22863 RVA: 0x000DEA1A File Offset: 0x000DCC1A
	public bool IsEmpty
	{
		get
		{
			return !this.ignoreMinMassCheck && (this.MassAvailable == 0f || this.MassAvailable < this.ConsumptionRate);
		}
	}

	// Token: 0x17000556 RID: 1366
	// (get) Token: 0x06005950 RID: 22864 RVA: 0x000DEA43 File Offset: 0x000DCC43
	public float ConsumptionRate
	{
		get
		{
			return this.consumptionRate;
		}
	}

	// Token: 0x17000557 RID: 1367
	// (get) Token: 0x06005951 RID: 22865 RVA: 0x000DEA4B File Offset: 0x000DCC4B
	// (set) Token: 0x06005952 RID: 22866 RVA: 0x000DEA60 File Offset: 0x000DCC60
	public bool IsSatisfied
	{
		get
		{
			return this.satisfied || !this.isConsuming;
		}
		set
		{
			this.satisfied = (value || this.forceAlwaysSatisfied);
		}
	}

	// Token: 0x06005953 RID: 22867 RVA: 0x0029CFD4 File Offset: 0x0029B1D4
	private ConduitFlow GetConduitManager()
	{
		ConduitType conduitType = this.conduitType;
		if (conduitType == ConduitType.Gas)
		{
			return Game.Instance.gasConduitFlow;
		}
		if (conduitType != ConduitType.Liquid)
		{
			return null;
		}
		return Game.Instance.liquidConduitFlow;
	}

	// Token: 0x17000558 RID: 1368
	// (get) Token: 0x06005954 RID: 22868 RVA: 0x0029D00C File Offset: 0x0029B20C
	public float MassAvailable
	{
		get
		{
			ConduitFlow conduitManager = this.GetConduitManager();
			int inputCell = this.GetInputCell(conduitManager.conduitType);
			return conduitManager.GetContents(inputCell).mass;
		}
	}

	// Token: 0x06005955 RID: 22869 RVA: 0x0029D03C File Offset: 0x0029B23C
	protected virtual int GetInputCell(ConduitType inputConduitType)
	{
		if (this.useSecondaryInput)
		{
			ISecondaryInput[] components = base.GetComponents<ISecondaryInput>();
			foreach (ISecondaryInput secondaryInput in components)
			{
				if (secondaryInput.HasSecondaryConduitType(inputConduitType))
				{
					return Grid.OffsetCell(this.building.NaturalBuildingCell(), secondaryInput.GetSecondaryConduitOffset(inputConduitType));
				}
			}
			global::Debug.LogWarning("No secondaryInput of type was found");
			return Grid.OffsetCell(this.building.NaturalBuildingCell(), components[0].GetSecondaryConduitOffset(inputConduitType));
		}
		return this.building.GetUtilityInputCell();
	}

	// Token: 0x06005956 RID: 22870 RVA: 0x0029D0BC File Offset: 0x0029B2BC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		GameScheduler.Instance.Schedule("PlumbingTutorial", 2f, delegate(object obj)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Plumbing, true);
		}, null, null);
		ConduitFlow conduitManager = this.GetConduitManager();
		this.utilityCell = this.GetInputCell(conduitManager.conduitType);
		ScenePartitionerLayer layer = GameScenePartitioner.Instance.objectLayers[(this.conduitType == ConduitType.Gas) ? 12 : 16];
		this.partitionerEntry = GameScenePartitioner.Instance.Add("ConduitConsumer.OnSpawn", base.gameObject, this.utilityCell, layer, new Action<object>(this.OnConduitConnectionChanged));
		this.GetConduitManager().AddConduitUpdater(new Action<float>(this.ConduitUpdate), ConduitFlowPriority.Default);
		this.OnConduitConnectionChanged(null);
	}

	// Token: 0x06005957 RID: 22871 RVA: 0x000DEA74 File Offset: 0x000DCC74
	protected override void OnCleanUp()
	{
		this.GetConduitManager().RemoveConduitUpdater(new Action<float>(this.ConduitUpdate));
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		base.OnCleanUp();
	}

	// Token: 0x06005958 RID: 22872 RVA: 0x000DEAA3 File Offset: 0x000DCCA3
	private void OnConduitConnectionChanged(object data)
	{
		base.Trigger(-2094018600, this.IsConnected);
	}

	// Token: 0x06005959 RID: 22873 RVA: 0x000DEABB File Offset: 0x000DCCBB
	public void SetOnState(bool onState)
	{
		this.isOn = onState;
	}

	// Token: 0x0600595A RID: 22874 RVA: 0x0029D188 File Offset: 0x0029B388
	private void ConduitUpdate(float dt)
	{
		if (this.isConsuming && this.isOn)
		{
			ConduitFlow conduitManager = this.GetConduitManager();
			this.Consume(dt, conduitManager);
		}
	}

	// Token: 0x0600595B RID: 22875 RVA: 0x0029D1B4 File Offset: 0x0029B3B4
	private void Consume(float dt, ConduitFlow conduit_mgr)
	{
		this.IsSatisfied = false;
		this.consumedLastTick = false;
		if (this.building.Def.CanMove)
		{
			this.utilityCell = this.GetInputCell(conduit_mgr.conduitType);
		}
		if (!this.IsConnected)
		{
			return;
		}
		ConduitFlow.ConduitContents contents = conduit_mgr.GetContents(this.utilityCell);
		if (contents.mass <= 0f)
		{
			return;
		}
		this.IsSatisfied = true;
		if (!this.alwaysConsume && !this.operational.MeetsRequirements(this.OperatingRequirement))
		{
			return;
		}
		float num = this.ConsumptionRate * dt;
		num = Mathf.Min(num, this.space_remaining_kg);
		Element element = ElementLoader.FindElementByHash(contents.element);
		if (contents.element != this.lastConsumedElement)
		{
			DiscoveredResources.Instance.Discover(element.tag, element.materialCategory);
		}
		float num2 = 0f;
		if (num > 0f)
		{
			ConduitFlow.ConduitContents conduitContents = conduit_mgr.RemoveElement(this.utilityCell, num);
			num2 = conduitContents.mass;
			this.lastConsumedElement = conduitContents.element;
		}
		bool flag = element.HasTag(this.capacityTag);
		if (num2 > 0f && this.capacityTag != GameTags.Any && !flag)
		{
			base.Trigger(-794517298, new BuildingHP.DamageSourceInfo
			{
				damage = 1,
				source = BUILDINGS.DAMAGESOURCES.BAD_INPUT_ELEMENT,
				popString = UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.WRONG_ELEMENT
			});
		}
		if (flag || this.wrongElementResult == ConduitConsumer.WrongElementResult.Store || contents.element == SimHashes.Vacuum || this.capacityTag == GameTags.Any)
		{
			if (num2 > 0f)
			{
				this.consumedLastTick = true;
				int disease_count = (int)((float)contents.diseaseCount * (num2 / contents.mass));
				Element element2 = ElementLoader.FindElementByHash(contents.element);
				ConduitType conduitType = this.conduitType;
				if (conduitType != ConduitType.Gas)
				{
					if (conduitType == ConduitType.Liquid)
					{
						if (element2.IsLiquid)
						{
							this.storage.AddLiquid(contents.element, num2, contents.temperature, contents.diseaseIdx, disease_count, this.keepZeroMassObject, false);
							return;
						}
						global::Debug.LogWarning("Liquid conduit consumer consuming non liquid: " + element2.id.ToString());
						return;
					}
				}
				else
				{
					if (element2.IsGas)
					{
						this.storage.AddGasChunk(contents.element, num2, contents.temperature, contents.diseaseIdx, disease_count, this.keepZeroMassObject, false);
						return;
					}
					global::Debug.LogWarning("Gas conduit consumer consuming non gas: " + element2.id.ToString());
					return;
				}
			}
		}
		else if (num2 > 0f)
		{
			this.consumedLastTick = true;
			if (this.wrongElementResult == ConduitConsumer.WrongElementResult.Dump)
			{
				int disease_count2 = (int)((float)contents.diseaseCount * (num2 / contents.mass));
				SimMessages.AddRemoveSubstance(Grid.PosToCell(base.transform.GetPosition()), contents.element, CellEventLogger.Instance.ConduitConsumerWrongElement, num2, contents.temperature, contents.diseaseIdx, disease_count2, true, -1);
			}
		}
	}

	// Token: 0x04003F76 RID: 16246
	[SerializeField]
	public ConduitType conduitType;

	// Token: 0x04003F77 RID: 16247
	[SerializeField]
	public bool ignoreMinMassCheck;

	// Token: 0x04003F78 RID: 16248
	[SerializeField]
	public Tag capacityTag = GameTags.Any;

	// Token: 0x04003F79 RID: 16249
	[SerializeField]
	public float capacityKG = float.PositiveInfinity;

	// Token: 0x04003F7A RID: 16250
	[SerializeField]
	public bool forceAlwaysSatisfied;

	// Token: 0x04003F7B RID: 16251
	[SerializeField]
	public bool alwaysConsume;

	// Token: 0x04003F7C RID: 16252
	[SerializeField]
	public bool keepZeroMassObject = true;

	// Token: 0x04003F7D RID: 16253
	[SerializeField]
	public bool useSecondaryInput;

	// Token: 0x04003F7E RID: 16254
	[SerializeField]
	public bool isOn = true;

	// Token: 0x04003F7F RID: 16255
	[NonSerialized]
	public bool isConsuming = true;

	// Token: 0x04003F80 RID: 16256
	[NonSerialized]
	public bool consumedLastTick = true;

	// Token: 0x04003F81 RID: 16257
	[MyCmpReq]
	public Operational operational;

	// Token: 0x04003F82 RID: 16258
	[MyCmpReq]
	protected Building building;

	// Token: 0x04003F83 RID: 16259
	public Operational.State OperatingRequirement;

	// Token: 0x04003F84 RID: 16260
	public ISecondaryInput targetSecondaryInput;

	// Token: 0x04003F85 RID: 16261
	[MyCmpGet]
	public Storage storage;

	// Token: 0x04003F86 RID: 16262
	[MyCmpGet]
	private BuildingComplete m_buildingComplete;

	// Token: 0x04003F87 RID: 16263
	private int utilityCell = -1;

	// Token: 0x04003F88 RID: 16264
	public float consumptionRate = float.PositiveInfinity;

	// Token: 0x04003F89 RID: 16265
	public SimHashes lastConsumedElement = SimHashes.Vacuum;

	// Token: 0x04003F8A RID: 16266
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04003F8B RID: 16267
	private bool satisfied;

	// Token: 0x04003F8C RID: 16268
	public ConduitConsumer.WrongElementResult wrongElementResult;

	// Token: 0x02001110 RID: 4368
	public enum WrongElementResult
	{
		// Token: 0x04003F8E RID: 16270
		Destroy,
		// Token: 0x04003F8F RID: 16271
		Dump,
		// Token: 0x04003F90 RID: 16272
		Store
	}
}
