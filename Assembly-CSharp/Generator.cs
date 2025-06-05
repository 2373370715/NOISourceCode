using System;
using System.Collections.Generic;
using System.Diagnostics;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020013AC RID: 5036
[SerializationConfig(MemberSerialization.OptIn)]
[DebuggerDisplay("{name}")]
[AddComponentMenu("KMonoBehaviour/scripts/Generator")]
public class Generator : KMonoBehaviour, ISaveLoadable, IEnergyProducer, ICircuitConnected
{
	// Token: 0x17000664 RID: 1636
	// (get) Token: 0x0600672A RID: 26410 RVA: 0x000E7BE3 File Offset: 0x000E5DE3
	public int PowerDistributionOrder
	{
		get
		{
			return this.powerDistributionOrder;
		}
	}

	// Token: 0x17000665 RID: 1637
	// (get) Token: 0x0600672B RID: 26411 RVA: 0x000E7BEB File Offset: 0x000E5DEB
	public virtual float Capacity
	{
		get
		{
			return this.capacity;
		}
	}

	// Token: 0x17000666 RID: 1638
	// (get) Token: 0x0600672C RID: 26412 RVA: 0x000E7BF3 File Offset: 0x000E5DF3
	public virtual bool IsEmpty
	{
		get
		{
			return this.joulesAvailable <= 0f;
		}
	}

	// Token: 0x17000667 RID: 1639
	// (get) Token: 0x0600672D RID: 26413 RVA: 0x000E7C05 File Offset: 0x000E5E05
	public virtual float JoulesAvailable
	{
		get
		{
			return this.joulesAvailable;
		}
	}

	// Token: 0x17000668 RID: 1640
	// (get) Token: 0x0600672E RID: 26414 RVA: 0x000E7C0D File Offset: 0x000E5E0D
	public float WattageRating
	{
		get
		{
			return this.building.Def.GeneratorWattageRating * this.Efficiency;
		}
	}

	// Token: 0x17000669 RID: 1641
	// (get) Token: 0x0600672F RID: 26415 RVA: 0x000E7C26 File Offset: 0x000E5E26
	public float BaseWattageRating
	{
		get
		{
			return this.building.Def.GeneratorWattageRating;
		}
	}

	// Token: 0x1700066A RID: 1642
	// (get) Token: 0x06006730 RID: 26416 RVA: 0x000E7C38 File Offset: 0x000E5E38
	public float PercentFull
	{
		get
		{
			if (this.Capacity == 0f)
			{
				return 1f;
			}
			return this.joulesAvailable / this.Capacity;
		}
	}

	// Token: 0x1700066B RID: 1643
	// (get) Token: 0x06006731 RID: 26417 RVA: 0x000E7C5A File Offset: 0x000E5E5A
	// (set) Token: 0x06006732 RID: 26418 RVA: 0x000E7C62 File Offset: 0x000E5E62
	public int PowerCell { get; private set; }

	// Token: 0x1700066C RID: 1644
	// (get) Token: 0x06006733 RID: 26419 RVA: 0x000CD5E2 File Offset: 0x000CB7E2
	public ushort CircuitID
	{
		get
		{
			return Game.Instance.circuitManager.GetCircuitID(this);
		}
	}

	// Token: 0x1700066D RID: 1645
	// (get) Token: 0x06006734 RID: 26420 RVA: 0x000E7C6B File Offset: 0x000E5E6B
	private float Efficiency
	{
		get
		{
			return Mathf.Max(1f + this.generatorOutputAttribute.GetTotalValue() / 100f, 0f);
		}
	}

	// Token: 0x1700066E RID: 1646
	// (get) Token: 0x06006735 RID: 26421 RVA: 0x000E7C8E File Offset: 0x000E5E8E
	// (set) Token: 0x06006736 RID: 26422 RVA: 0x000E7C96 File Offset: 0x000E5E96
	public bool IsVirtual { get; protected set; }

	// Token: 0x1700066F RID: 1647
	// (get) Token: 0x06006737 RID: 26423 RVA: 0x000E7C9F File Offset: 0x000E5E9F
	// (set) Token: 0x06006738 RID: 26424 RVA: 0x000E7CA7 File Offset: 0x000E5EA7
	public object VirtualCircuitKey { get; protected set; }

	// Token: 0x06006739 RID: 26425 RVA: 0x002E0734 File Offset: 0x002DE934
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Attributes attributes = base.gameObject.GetAttributes();
		this.generatorOutputAttribute = attributes.Add(Db.Get().Attributes.GeneratorOutput);
	}

	// Token: 0x0600673A RID: 26426 RVA: 0x002E0770 File Offset: 0x002DE970
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.Generators.Add(this);
		this.cachedPrefabId = base.gameObject.PrefabID();
		base.Subscribe<Generator>(-1582839653, Generator.OnTagsChangedDelegate);
		this.OnTagsChanged(null);
		this.capacity = Generator.CalculateCapacity(this.building.Def, null);
		this.PowerCell = this.building.GetPowerOutputCell();
		this.CheckConnectionStatus();
		Game.Instance.energySim.AddGenerator(this);
	}

	// Token: 0x0600673B RID: 26427 RVA: 0x000E7CB0 File Offset: 0x000E5EB0
	private void OnTagsChanged(object data)
	{
		if (this.HasAllTags(this.connectedTags))
		{
			Game.Instance.circuitManager.Connect(this);
			return;
		}
		Game.Instance.circuitManager.Disconnect(this);
	}

	// Token: 0x0600673C RID: 26428 RVA: 0x000E7CE1 File Offset: 0x000E5EE1
	public virtual bool IsProducingPower()
	{
		return this.operational.IsActive;
	}

	// Token: 0x0600673D RID: 26429 RVA: 0x000E7CEE File Offset: 0x000E5EEE
	public virtual void EnergySim200ms(float dt)
	{
		this.CheckConnectionStatus();
	}

	// Token: 0x0600673E RID: 26430 RVA: 0x002E07F8 File Offset: 0x002DE9F8
	private void SetStatusItem(StatusItem status_item)
	{
		if (status_item != this.currentStatusItem && this.currentStatusItem != null)
		{
			this.statusItemID = this.selectable.RemoveStatusItem(this.statusItemID, false);
		}
		if (status_item != null && this.statusItemID == Guid.Empty)
		{
			this.statusItemID = this.selectable.AddStatusItem(status_item, this);
		}
		this.currentStatusItem = status_item;
	}

	// Token: 0x0600673F RID: 26431 RVA: 0x002E0860 File Offset: 0x002DEA60
	private void CheckConnectionStatus()
	{
		if (this.CircuitID == 65535)
		{
			if (this.showConnectedConsumerStatusItems)
			{
				this.SetStatusItem(Db.Get().BuildingStatusItems.NoWireConnected);
			}
			this.operational.SetFlag(Generator.generatorConnectedFlag, false);
			return;
		}
		if (!Game.Instance.circuitManager.HasConsumers(this.CircuitID) && !Game.Instance.circuitManager.HasBatteries(this.CircuitID))
		{
			if (this.showConnectedConsumerStatusItems)
			{
				this.SetStatusItem(Db.Get().BuildingStatusItems.NoPowerConsumers);
			}
			this.operational.SetFlag(Generator.generatorConnectedFlag, true);
			return;
		}
		this.SetStatusItem(null);
		this.operational.SetFlag(Generator.generatorConnectedFlag, true);
	}

	// Token: 0x06006740 RID: 26432 RVA: 0x000E7CF6 File Offset: 0x000E5EF6
	protected override void OnCleanUp()
	{
		Game.Instance.energySim.RemoveGenerator(this);
		Game.Instance.circuitManager.Disconnect(this);
		Components.Generators.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x06006741 RID: 26433 RVA: 0x000E7D29 File Offset: 0x000E5F29
	public static float CalculateCapacity(BuildingDef def, Element element)
	{
		if (element == null)
		{
			return def.GeneratorBaseCapacity;
		}
		return def.GeneratorBaseCapacity * (1f + (element.HasTag(GameTags.RefinedMetal) ? 1f : 0f));
	}

	// Token: 0x06006742 RID: 26434 RVA: 0x000E7D5B File Offset: 0x000E5F5B
	public void ResetJoules()
	{
		this.joulesAvailable = 0f;
	}

	// Token: 0x06006743 RID: 26435 RVA: 0x000E7D68 File Offset: 0x000E5F68
	public virtual void ApplyDeltaJoules(float joulesDelta, bool canOverPower = false)
	{
		this.joulesAvailable = Mathf.Clamp(this.joulesAvailable + joulesDelta, 0f, canOverPower ? float.MaxValue : this.Capacity);
	}

	// Token: 0x06006744 RID: 26436 RVA: 0x002E0920 File Offset: 0x002DEB20
	public void GenerateJoules(float joulesAvailable, bool canOverPower = false)
	{
		ReportManager.Instance.ReportValue(ReportManager.ReportType.EnergyCreated, joulesAvailable, this.selectable.GetProperName(), null);
		float num = this.joulesAvailable + joulesAvailable;
		this.joulesAvailable = Mathf.Clamp(num, 0f, canOverPower ? float.MaxValue : this.Capacity);
		if (num > joulesAvailable)
		{
			ReportManager.Instance.ReportValue(ReportManager.ReportType.EnergyWasted, this.joulesAvailable - num, StringFormatter.Replace(BUILDINGS.PREFABS.GENERATOR.OVERPRODUCTION, "{Generator}", base.gameObject.GetProperName()), null);
		}
		if (!Game.Instance.savedInfo.powerCreatedbyGeneratorType.ContainsKey(this.cachedPrefabId))
		{
			Game.Instance.savedInfo.powerCreatedbyGeneratorType.Add(this.cachedPrefabId, 0f);
		}
		Dictionary<Tag, float> powerCreatedbyGeneratorType = Game.Instance.savedInfo.powerCreatedbyGeneratorType;
		Tag key = this.cachedPrefabId;
		powerCreatedbyGeneratorType[key] += this.joulesAvailable;
	}

	// Token: 0x06006745 RID: 26437 RVA: 0x000E7D92 File Offset: 0x000E5F92
	public void AssignJoulesAvailable(float joulesAvailable)
	{
		this.joulesAvailable = joulesAvailable;
	}

	// Token: 0x06006746 RID: 26438 RVA: 0x000E7D9B File Offset: 0x000E5F9B
	public virtual void ConsumeEnergy(float joules)
	{
		this.joulesAvailable = Mathf.Max(0f, this.JoulesAvailable - joules);
	}

	// Token: 0x04004DDE RID: 19934
	protected const int SimUpdateSortKey = 1001;

	// Token: 0x04004DDF RID: 19935
	[MyCmpReq]
	protected Building building;

	// Token: 0x04004DE0 RID: 19936
	[MyCmpReq]
	protected Operational operational;

	// Token: 0x04004DE1 RID: 19937
	[MyCmpReq]
	protected KSelectable selectable;

	// Token: 0x04004DE2 RID: 19938
	[Serialize]
	private float joulesAvailable;

	// Token: 0x04004DE3 RID: 19939
	[SerializeField]
	public int powerDistributionOrder;

	// Token: 0x04004DE4 RID: 19940
	private Tag cachedPrefabId;

	// Token: 0x04004DE5 RID: 19941
	public static readonly Operational.Flag generatorConnectedFlag = new Operational.Flag("GeneratorConnected", Operational.Flag.Type.Requirement);

	// Token: 0x04004DE6 RID: 19942
	protected static readonly Operational.Flag wireConnectedFlag = new Operational.Flag("generatorWireConnected", Operational.Flag.Type.Requirement);

	// Token: 0x04004DE7 RID: 19943
	private float capacity;

	// Token: 0x04004DEB RID: 19947
	public static readonly Tag[] DEFAULT_CONNECTED_TAGS = new Tag[]
	{
		GameTags.Operational
	};

	// Token: 0x04004DEC RID: 19948
	[SerializeField]
	public Tag[] connectedTags = Generator.DEFAULT_CONNECTED_TAGS;

	// Token: 0x04004DED RID: 19949
	public bool showConnectedConsumerStatusItems = true;

	// Token: 0x04004DEE RID: 19950
	private StatusItem currentStatusItem;

	// Token: 0x04004DEF RID: 19951
	private Guid statusItemID;

	// Token: 0x04004DF0 RID: 19952
	private AttributeInstance generatorOutputAttribute;

	// Token: 0x04004DF1 RID: 19953
	private static readonly EventSystem.IntraObjectHandler<Generator> OnTagsChangedDelegate = new EventSystem.IntraObjectHandler<Generator>(delegate(Generator component, object data)
	{
		component.OnTagsChanged(data);
	});
}
