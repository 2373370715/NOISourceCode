using System;
using STRINGS;
using UnityEngine;

// Token: 0x020012D7 RID: 4823
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/SpawnableConduitConsumer")]
public class EntityConduitConsumer : KMonoBehaviour, IConduitConsumer
{
	// Token: 0x17000622 RID: 1570
	// (get) Token: 0x060062F6 RID: 25334 RVA: 0x000E505E File Offset: 0x000E325E
	public Storage Storage
	{
		get
		{
			return this.storage;
		}
	}

	// Token: 0x17000623 RID: 1571
	// (get) Token: 0x060062F7 RID: 25335 RVA: 0x000E5066 File Offset: 0x000E3266
	public ConduitType ConduitType
	{
		get
		{
			return this.conduitType;
		}
	}

	// Token: 0x17000624 RID: 1572
	// (get) Token: 0x060062F8 RID: 25336 RVA: 0x000E506E File Offset: 0x000E326E
	public bool IsConnected
	{
		get
		{
			return Grid.Objects[this.utilityCell, (this.conduitType == ConduitType.Gas) ? 12 : 16] != null;
		}
	}

	// Token: 0x17000625 RID: 1573
	// (get) Token: 0x060062F9 RID: 25337 RVA: 0x002C68DC File Offset: 0x002C4ADC
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

	// Token: 0x17000626 RID: 1574
	// (get) Token: 0x060062FA RID: 25338 RVA: 0x002C6918 File Offset: 0x002C4B18
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

	// Token: 0x17000627 RID: 1575
	// (get) Token: 0x060062FB RID: 25339 RVA: 0x002C6968 File Offset: 0x002C4B68
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

	// Token: 0x060062FC RID: 25340 RVA: 0x000E5095 File Offset: 0x000E3295
	public void SetConduitData(ConduitType type)
	{
		this.conduitType = type;
	}

	// Token: 0x17000628 RID: 1576
	// (get) Token: 0x060062FD RID: 25341 RVA: 0x000E5066 File Offset: 0x000E3266
	public ConduitType TypeOfConduit
	{
		get
		{
			return this.conduitType;
		}
	}

	// Token: 0x17000629 RID: 1577
	// (get) Token: 0x060062FE RID: 25342 RVA: 0x000E509E File Offset: 0x000E329E
	public bool IsAlmostEmpty
	{
		get
		{
			return !this.ignoreMinMassCheck && this.MassAvailable < this.ConsumptionRate * 30f;
		}
	}

	// Token: 0x1700062A RID: 1578
	// (get) Token: 0x060062FF RID: 25343 RVA: 0x000E50BE File Offset: 0x000E32BE
	public bool IsEmpty
	{
		get
		{
			return !this.ignoreMinMassCheck && (this.MassAvailable == 0f || this.MassAvailable < this.ConsumptionRate);
		}
	}

	// Token: 0x1700062B RID: 1579
	// (get) Token: 0x06006300 RID: 25344 RVA: 0x000E50E7 File Offset: 0x000E32E7
	public float ConsumptionRate
	{
		get
		{
			return this.consumptionRate;
		}
	}

	// Token: 0x1700062C RID: 1580
	// (get) Token: 0x06006301 RID: 25345 RVA: 0x000E50EF File Offset: 0x000E32EF
	// (set) Token: 0x06006302 RID: 25346 RVA: 0x000E5104 File Offset: 0x000E3304
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

	// Token: 0x06006303 RID: 25347 RVA: 0x002C69A4 File Offset: 0x002C4BA4
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

	// Token: 0x1700062D RID: 1581
	// (get) Token: 0x06006304 RID: 25348 RVA: 0x002C69DC File Offset: 0x002C4BDC
	public float MassAvailable
	{
		get
		{
			ConduitFlow conduitManager = this.GetConduitManager();
			int inputCell = this.GetInputCell(conduitManager.conduitType);
			return conduitManager.GetContents(inputCell).mass;
		}
	}

	// Token: 0x06006305 RID: 25349 RVA: 0x000E5118 File Offset: 0x000E3318
	private int GetInputCell(ConduitType inputConduitType)
	{
		return this.occupyArea.GetOffsetCellWithRotation(this.offset);
	}

	// Token: 0x06006306 RID: 25350 RVA: 0x002C6A0C File Offset: 0x002C4C0C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		ConduitFlow conduitManager = this.GetConduitManager();
		this.utilityCell = this.GetInputCell(conduitManager.conduitType);
		ScenePartitionerLayer layer = GameScenePartitioner.Instance.objectLayers[(this.conduitType == ConduitType.Gas) ? 12 : 16];
		this.partitionerEntry = GameScenePartitioner.Instance.Add("ConduitConsumer.OnSpawn", base.gameObject, this.utilityCell, layer, new Action<object>(this.OnConduitConnectionChanged));
		this.GetConduitManager().AddConduitUpdater(new Action<float>(this.ConduitUpdate), ConduitFlowPriority.Default);
		this.endpoint = new FlowUtilityNetwork.NetworkItem(conduitManager.conduitType, Endpoint.Sink, this.utilityCell, base.gameObject);
		if (conduitManager.conduitType == ConduitType.Solid)
		{
			Game.Instance.solidConduitSystem.AddToNetworks(this.utilityCell, this.endpoint, true);
		}
		else
		{
			Conduit.GetNetworkManager(conduitManager.conduitType).AddToNetworks(this.utilityCell, this.endpoint, true);
		}
		EntityCellVisualizer.Ports type = EntityCellVisualizer.Ports.LiquidIn;
		if (conduitManager.conduitType == ConduitType.Solid)
		{
			type = EntityCellVisualizer.Ports.SolidIn;
		}
		else if (conduitManager.conduitType == ConduitType.Gas)
		{
			type = EntityCellVisualizer.Ports.GasIn;
		}
		this.cellVisualizer.AddPort(type, this.offset);
		this.OnConduitConnectionChanged(null);
	}

	// Token: 0x06006307 RID: 25351 RVA: 0x002C6B30 File Offset: 0x002C4D30
	protected override void OnCleanUp()
	{
		if (this.endpoint.ConduitType == ConduitType.Solid)
		{
			Game.Instance.solidConduitSystem.RemoveFromNetworks(this.endpoint.Cell, this.endpoint, true);
		}
		else
		{
			Conduit.GetNetworkManager(this.endpoint.ConduitType).RemoveFromNetworks(this.endpoint.Cell, this.endpoint, true);
		}
		this.GetConduitManager().RemoveConduitUpdater(new Action<float>(this.ConduitUpdate));
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		base.OnCleanUp();
	}

	// Token: 0x06006308 RID: 25352 RVA: 0x000E512B File Offset: 0x000E332B
	private void OnConduitConnectionChanged(object data)
	{
		base.Trigger(-2094018600, this.IsConnected);
	}

	// Token: 0x06006309 RID: 25353 RVA: 0x000E5143 File Offset: 0x000E3343
	public void SetOnState(bool onState)
	{
		this.isOn = onState;
	}

	// Token: 0x0600630A RID: 25354 RVA: 0x002C6BC4 File Offset: 0x002C4DC4
	private void ConduitUpdate(float dt)
	{
		if (this.isConsuming && this.isOn)
		{
			ConduitFlow conduitManager = this.GetConduitManager();
			this.Consume(dt, conduitManager);
		}
	}

	// Token: 0x0600630B RID: 25355 RVA: 0x002C6BF0 File Offset: 0x002C4DF0
	private void Consume(float dt, ConduitFlow conduit_mgr)
	{
		this.IsSatisfied = false;
		this.consumedLastTick = false;
		this.utilityCell = this.GetInputCell(conduit_mgr.conduitType);
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
		if (flag || this.wrongElementResult == EntityConduitConsumer.WrongElementResult.Store || contents.element == SimHashes.Vacuum || this.capacityTag == GameTags.Any)
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
			if (this.wrongElementResult == EntityConduitConsumer.WrongElementResult.Dump)
			{
				int disease_count2 = (int)((float)contents.diseaseCount * (num2 / contents.mass));
				SimMessages.AddRemoveSubstance(Grid.PosToCell(base.transform.GetPosition()), contents.element, CellEventLogger.Instance.ConduitConsumerWrongElement, num2, contents.temperature, contents.diseaseIdx, disease_count2, true, -1);
			}
		}
	}

	// Token: 0x040046EC RID: 18156
	private FlowUtilityNetwork.NetworkItem endpoint;

	// Token: 0x040046ED RID: 18157
	[SerializeField]
	public ConduitType conduitType;

	// Token: 0x040046EE RID: 18158
	[SerializeField]
	public bool ignoreMinMassCheck;

	// Token: 0x040046EF RID: 18159
	[SerializeField]
	public Tag capacityTag = GameTags.Any;

	// Token: 0x040046F0 RID: 18160
	[SerializeField]
	public float capacityKG = float.PositiveInfinity;

	// Token: 0x040046F1 RID: 18161
	[SerializeField]
	public bool forceAlwaysSatisfied;

	// Token: 0x040046F2 RID: 18162
	[SerializeField]
	public bool alwaysConsume;

	// Token: 0x040046F3 RID: 18163
	[SerializeField]
	public bool keepZeroMassObject = true;

	// Token: 0x040046F4 RID: 18164
	[SerializeField]
	public bool isOn = true;

	// Token: 0x040046F5 RID: 18165
	[NonSerialized]
	public bool isConsuming = true;

	// Token: 0x040046F6 RID: 18166
	[NonSerialized]
	public bool consumedLastTick = true;

	// Token: 0x040046F7 RID: 18167
	[MyCmpReq]
	public Operational operational;

	// Token: 0x040046F8 RID: 18168
	[MyCmpReq]
	private OccupyArea occupyArea;

	// Token: 0x040046F9 RID: 18169
	[MyCmpReq]
	private EntityCellVisualizer cellVisualizer;

	// Token: 0x040046FA RID: 18170
	public Operational.State OperatingRequirement;

	// Token: 0x040046FB RID: 18171
	[MyCmpGet]
	public Storage storage;

	// Token: 0x040046FC RID: 18172
	public CellOffset offset;

	// Token: 0x040046FD RID: 18173
	private int utilityCell = -1;

	// Token: 0x040046FE RID: 18174
	public float consumptionRate = float.PositiveInfinity;

	// Token: 0x040046FF RID: 18175
	public SimHashes lastConsumedElement = SimHashes.Vacuum;

	// Token: 0x04004700 RID: 18176
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04004701 RID: 18177
	private bool satisfied;

	// Token: 0x04004702 RID: 18178
	public EntityConduitConsumer.WrongElementResult wrongElementResult;

	// Token: 0x020012D8 RID: 4824
	public enum WrongElementResult
	{
		// Token: 0x04004704 RID: 18180
		Destroy,
		// Token: 0x04004705 RID: 18181
		Dump,
		// Token: 0x04004706 RID: 18182
		Store
	}
}
