using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020012BD RID: 4797
[SkipSaveFileSerialization]
[SerializationConfig(MemberSerialization.OptIn)]
public class ElementConsumer : SimComponent, ISaveLoadable, IGameObjectEffectDescriptor
{
	// Token: 0x1400001E RID: 30
	// (add) Token: 0x06006216 RID: 25110 RVA: 0x002C35EC File Offset: 0x002C17EC
	// (remove) Token: 0x06006217 RID: 25111 RVA: 0x002C3624 File Offset: 0x002C1824
	public event Action<Sim.ConsumedMassInfo> OnElementConsumed;

	// Token: 0x17000601 RID: 1537
	// (get) Token: 0x06006218 RID: 25112 RVA: 0x000E4700 File Offset: 0x000E2900
	public float AverageConsumeRate
	{
		get
		{
			return Game.Instance.accumulators.GetAverageRate(this.accumulator);
		}
	}

	// Token: 0x06006219 RID: 25113 RVA: 0x000E4717 File Offset: 0x000E2917
	public static void ClearInstanceMap()
	{
		ElementConsumer.handleInstanceMap.Clear();
	}

	// Token: 0x0600621A RID: 25114 RVA: 0x002C365C File Offset: 0x002C185C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.accumulator = Game.Instance.accumulators.Add("Element", this);
		if (this.elementToConsume == SimHashes.Void)
		{
			throw new ArgumentException("No consumable elements specified");
		}
		if (!this.ignoreActiveChanged)
		{
			base.Subscribe<ElementConsumer>(824508782, ElementConsumer.OnActiveChangedDelegate);
		}
		if (this.capacityKG != float.PositiveInfinity)
		{
			this.hasAvailableCapacity = !this.IsStorageFull();
			base.Subscribe<ElementConsumer>(-1697596308, ElementConsumer.OnStorageChangeDelegate);
		}
	}

	// Token: 0x0600621B RID: 25115 RVA: 0x000E4723 File Offset: 0x000E2923
	protected override void OnCleanUp()
	{
		Game.Instance.accumulators.Remove(this.accumulator);
		base.OnCleanUp();
	}

	// Token: 0x0600621C RID: 25116 RVA: 0x000E4741 File Offset: 0x000E2941
	protected virtual bool IsActive()
	{
		return this.operational == null || this.operational.IsActive;
	}

	// Token: 0x0600621D RID: 25117 RVA: 0x002C36E8 File Offset: 0x002C18E8
	public void EnableConsumption(bool enabled)
	{
		bool flag = this.consumptionEnabled;
		this.consumptionEnabled = enabled;
		if (!Sim.IsValidHandle(this.simHandle))
		{
			return;
		}
		if (enabled != flag)
		{
			this.UpdateSimData();
		}
	}

	// Token: 0x0600621E RID: 25118 RVA: 0x002C371C File Offset: 0x002C191C
	private bool IsStorageFull()
	{
		PrimaryElement primaryElement = this.storage.FindPrimaryElement(this.elementToConsume);
		return primaryElement != null && primaryElement.Mass >= this.capacityKG;
	}

	// Token: 0x0600621F RID: 25119 RVA: 0x000E475E File Offset: 0x000E295E
	public void RefreshConsumptionRate()
	{
		if (!Sim.IsValidHandle(this.simHandle))
		{
			return;
		}
		this.UpdateSimData();
	}

	// Token: 0x06006220 RID: 25120 RVA: 0x002C3758 File Offset: 0x002C1958
	private void UpdateSimData()
	{
		global::Debug.Assert(Sim.IsValidHandle(this.simHandle));
		int sampleCell = this.GetSampleCell();
		float num = (this.consumptionEnabled && this.hasAvailableCapacity) ? this.consumptionRate : 0f;
		SimMessages.SetElementConsumerData(this.simHandle, sampleCell, num);
		this.UpdateStatusItem();
	}

	// Token: 0x06006221 RID: 25121 RVA: 0x002C37B0 File Offset: 0x002C19B0
	public static void AddMass(Sim.ConsumedMassInfo consumed_info)
	{
		if (!Sim.IsValidHandle(consumed_info.simHandle))
		{
			return;
		}
		ElementConsumer elementConsumer;
		if (ElementConsumer.handleInstanceMap.TryGetValue(consumed_info.simHandle, out elementConsumer))
		{
			elementConsumer.AddMassInternal(consumed_info);
		}
	}

	// Token: 0x06006222 RID: 25122 RVA: 0x000E4774 File Offset: 0x000E2974
	private int GetSampleCell()
	{
		return Grid.PosToCell(base.transform.GetPosition() + this.sampleCellOffset);
	}

	// Token: 0x06006223 RID: 25123 RVA: 0x002C37E8 File Offset: 0x002C19E8
	private void AddMassInternal(Sim.ConsumedMassInfo consumed_info)
	{
		if (consumed_info.mass > 0f)
		{
			if (this.storeOnConsume)
			{
				Element element = ElementLoader.elements[(int)consumed_info.removedElemIdx];
				if (this.elementToConsume == SimHashes.Vacuum || this.elementToConsume == element.id)
				{
					if (element.IsLiquid)
					{
						this.storage.AddLiquid(element.id, consumed_info.mass, consumed_info.temperature, consumed_info.diseaseIdx, consumed_info.diseaseCount, true, true);
					}
					else if (element.IsGas)
					{
						this.storage.AddGasChunk(element.id, consumed_info.mass, consumed_info.temperature, consumed_info.diseaseIdx, consumed_info.diseaseCount, true, true);
					}
				}
			}
			else
			{
				this.consumedTemperature = GameUtil.GetFinalTemperature(consumed_info.temperature, consumed_info.mass, this.consumedTemperature, this.consumedMass);
				this.consumedMass += consumed_info.mass;
				if (this.OnElementConsumed != null)
				{
					this.OnElementConsumed(consumed_info);
				}
			}
		}
		Game.Instance.accumulators.Accumulate(this.accumulator, consumed_info.mass);
	}

	// Token: 0x17000602 RID: 1538
	// (get) Token: 0x06006224 RID: 25124 RVA: 0x002C3914 File Offset: 0x002C1B14
	public bool IsElementAvailable
	{
		get
		{
			int sampleCell = this.GetSampleCell();
			SimHashes id = Grid.Element[sampleCell].id;
			return this.elementToConsume == id && Grid.Mass[sampleCell] >= this.minimumMass;
		}
	}

	// Token: 0x06006225 RID: 25125 RVA: 0x002C3958 File Offset: 0x002C1B58
	private void UpdateStatusItem()
	{
		if (this.showInStatusPanel)
		{
			if (this.statusHandle == Guid.Empty && this.IsActive() && this.consumptionEnabled)
			{
				this.statusHandle = this.selectable.AddStatusItem(Db.Get().BuildingStatusItems.ElementConsumer, this);
				return;
			}
			if (this.statusHandle != Guid.Empty && !this.consumptionEnabled)
			{
				base.GetComponent<KSelectable>().RemoveStatusItem(this.statusHandle, false);
				this.statusHandle = Guid.Empty;
				return;
			}
		}
		else if (this.statusHandle != Guid.Empty)
		{
			base.GetComponent<KSelectable>().RemoveStatusItem(this.statusHandle, false);
			this.statusHandle = Guid.Empty;
		}
	}

	// Token: 0x06006226 RID: 25126 RVA: 0x002C3A1C File Offset: 0x002C1C1C
	private void OnStorageChange(object data)
	{
		bool flag = !this.IsStorageFull();
		if (flag != this.hasAvailableCapacity)
		{
			this.hasAvailableCapacity = flag;
			this.RefreshConsumptionRate();
		}
	}

	// Token: 0x06006227 RID: 25127 RVA: 0x000E4791 File Offset: 0x000E2991
	protected override void OnCmpEnable()
	{
		if (!base.isSpawned)
		{
			return;
		}
		if (!this.IsActive())
		{
			return;
		}
		this.UpdateStatusItem();
	}

	// Token: 0x06006228 RID: 25128 RVA: 0x000E47AB File Offset: 0x000E29AB
	protected override void OnCmpDisable()
	{
		this.UpdateStatusItem();
	}

	// Token: 0x06006229 RID: 25129 RVA: 0x002C3A4C File Offset: 0x002C1C4C
	public List<Descriptor> RequirementDescriptors()
	{
		List<Descriptor> list = new List<Descriptor>();
		if (this.isRequired && this.showDescriptor)
		{
			Element element = ElementLoader.FindElementByHash(this.elementToConsume);
			string arg = element.tag.ProperName();
			if (element.IsVacuum)
			{
				if (this.configuration == ElementConsumer.Configuration.AllGas)
				{
					arg = ELEMENTS.STATE.GAS;
				}
				else if (this.configuration == ElementConsumer.Configuration.AllLiquid)
				{
					arg = ELEMENTS.STATE.LIQUID;
				}
				else
				{
					arg = UI.BUILDINGEFFECTS.CONSUMESANYELEMENT;
				}
			}
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.REQUIRESELEMENT, arg), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.REQUIRESELEMENT, arg), Descriptor.DescriptorType.Requirement);
			list.Add(item);
		}
		return list;
	}

	// Token: 0x0600622A RID: 25130 RVA: 0x002C3B04 File Offset: 0x002C1D04
	public List<Descriptor> EffectDescriptors()
	{
		List<Descriptor> list = new List<Descriptor>();
		if (this.showDescriptor)
		{
			Element element = ElementLoader.FindElementByHash(this.elementToConsume);
			string arg = element.tag.ProperName();
			if (element.IsVacuum)
			{
				if (this.configuration == ElementConsumer.Configuration.AllGas)
				{
					arg = ELEMENTS.STATE.GAS;
				}
				else if (this.configuration == ElementConsumer.Configuration.AllLiquid)
				{
					arg = ELEMENTS.STATE.LIQUID;
				}
				else
				{
					arg = UI.BUILDINGEFFECTS.CONSUMESANYELEMENT;
				}
			}
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTCONSUMED, arg, GameUtil.GetFormattedMass(this.consumptionRate / 100f * 100f, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMED, arg, GameUtil.GetFormattedMass(this.consumptionRate / 100f * 100f, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), Descriptor.DescriptorType.Effect);
			list.Add(item);
		}
		return list;
	}

	// Token: 0x0600622B RID: 25131 RVA: 0x002C3BF0 File Offset: 0x002C1DF0
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		foreach (Descriptor item in this.RequirementDescriptors())
		{
			list.Add(item);
		}
		foreach (Descriptor item2 in this.EffectDescriptors())
		{
			list.Add(item2);
		}
		return list;
	}

	// Token: 0x0600622C RID: 25132 RVA: 0x002C3C8C File Offset: 0x002C1E8C
	private void OnActiveChanged(object data)
	{
		bool isActive = this.operational.IsActive;
		this.EnableConsumption(isActive);
	}

	// Token: 0x0600622D RID: 25133 RVA: 0x000E47B3 File Offset: 0x000E29B3
	protected override void OnSimUnregister()
	{
		global::Debug.Assert(Sim.IsValidHandle(this.simHandle));
		ElementConsumer.handleInstanceMap.Remove(this.simHandle);
		ElementConsumer.StaticUnregister(this.simHandle);
	}

	// Token: 0x0600622E RID: 25134 RVA: 0x000E47E1 File Offset: 0x000E29E1
	protected override void OnSimRegister(HandleVector<Game.ComplexCallbackInfo<int>>.Handle cb_handle)
	{
		SimMessages.AddElementConsumer(this.GetSampleCell(), this.configuration, this.elementToConsume, this.consumptionRadius, cb_handle.index);
	}

	// Token: 0x0600622F RID: 25135 RVA: 0x000E4807 File Offset: 0x000E2A07
	protected override Action<int> GetStaticUnregister()
	{
		return new Action<int>(ElementConsumer.StaticUnregister);
	}

	// Token: 0x06006230 RID: 25136 RVA: 0x000E4815 File Offset: 0x000E2A15
	private static void StaticUnregister(int sim_handle)
	{
		global::Debug.Assert(Sim.IsValidHandle(sim_handle));
		SimMessages.RemoveElementConsumer(-1, sim_handle);
	}

	// Token: 0x06006231 RID: 25137 RVA: 0x000E4829 File Offset: 0x000E2A29
	protected override void OnSimRegistered()
	{
		if (this.consumptionEnabled)
		{
			this.UpdateSimData();
		}
		ElementConsumer.handleInstanceMap[this.simHandle] = this;
	}

	// Token: 0x04004656 RID: 18006
	[HashedEnum]
	[SerializeField]
	public SimHashes elementToConsume = SimHashes.Vacuum;

	// Token: 0x04004657 RID: 18007
	[SerializeField]
	public float consumptionRate;

	// Token: 0x04004658 RID: 18008
	[SerializeField]
	public byte consumptionRadius = 1;

	// Token: 0x04004659 RID: 18009
	[SerializeField]
	public float minimumMass;

	// Token: 0x0400465A RID: 18010
	[SerializeField]
	public bool showInStatusPanel = true;

	// Token: 0x0400465B RID: 18011
	[SerializeField]
	public Vector3 sampleCellOffset;

	// Token: 0x0400465C RID: 18012
	[SerializeField]
	public float capacityKG = float.PositiveInfinity;

	// Token: 0x0400465D RID: 18013
	[SerializeField]
	public ElementConsumer.Configuration configuration;

	// Token: 0x0400465E RID: 18014
	[Serialize]
	[NonSerialized]
	public float consumedMass;

	// Token: 0x0400465F RID: 18015
	[Serialize]
	[NonSerialized]
	public float consumedTemperature;

	// Token: 0x04004660 RID: 18016
	[SerializeField]
	public bool storeOnConsume;

	// Token: 0x04004661 RID: 18017
	[MyCmpGet]
	public Storage storage;

	// Token: 0x04004662 RID: 18018
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04004663 RID: 18019
	[MyCmpGet]
	private KSelectable selectable;

	// Token: 0x04004665 RID: 18021
	private HandleVector<int>.Handle accumulator = HandleVector<int>.InvalidHandle;

	// Token: 0x04004666 RID: 18022
	public bool ignoreActiveChanged;

	// Token: 0x04004667 RID: 18023
	private Guid statusHandle;

	// Token: 0x04004668 RID: 18024
	public bool showDescriptor = true;

	// Token: 0x04004669 RID: 18025
	public bool isRequired = true;

	// Token: 0x0400466A RID: 18026
	private bool consumptionEnabled;

	// Token: 0x0400466B RID: 18027
	private bool hasAvailableCapacity = true;

	// Token: 0x0400466C RID: 18028
	private static Dictionary<int, ElementConsumer> handleInstanceMap = new Dictionary<int, ElementConsumer>();

	// Token: 0x0400466D RID: 18029
	private static readonly EventSystem.IntraObjectHandler<ElementConsumer> OnActiveChangedDelegate = new EventSystem.IntraObjectHandler<ElementConsumer>(delegate(ElementConsumer component, object data)
	{
		component.OnActiveChanged(data);
	});

	// Token: 0x0400466E RID: 18030
	private static readonly EventSystem.IntraObjectHandler<ElementConsumer> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<ElementConsumer>(delegate(ElementConsumer component, object data)
	{
		component.OnStorageChange(data);
	});

	// Token: 0x020012BE RID: 4798
	public enum Configuration
	{
		// Token: 0x04004670 RID: 18032
		Element,
		// Token: 0x04004671 RID: 18033
		AllLiquid,
		// Token: 0x04004672 RID: 18034
		AllGas
	}
}
