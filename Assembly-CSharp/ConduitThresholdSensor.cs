using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000D3E RID: 3390
[SerializationConfig(MemberSerialization.OptIn)]
public abstract class ConduitThresholdSensor : ConduitSensor
{
	// Token: 0x17000327 RID: 807
	// (get) Token: 0x0600419F RID: 16799
	public abstract float CurrentValue { get; }

	// Token: 0x060041A0 RID: 16800 RVA: 0x000CEE88 File Offset: 0x000CD088
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<ConduitThresholdSensor>(-905833192, ConduitThresholdSensor.OnCopySettingsDelegate);
	}

	// Token: 0x060041A1 RID: 16801 RVA: 0x0024CD58 File Offset: 0x0024AF58
	private void OnCopySettings(object data)
	{
		ConduitThresholdSensor component = ((GameObject)data).GetComponent<ConduitThresholdSensor>();
		if (component != null)
		{
			this.Threshold = component.Threshold;
			this.ActivateAboveThreshold = component.ActivateAboveThreshold;
		}
	}

	// Token: 0x060041A2 RID: 16802 RVA: 0x0024CD94 File Offset: 0x0024AF94
	protected override void ConduitUpdate(float dt)
	{
		if (this.GetContainedMass() <= 0f && !this.dirty)
		{
			return;
		}
		float currentValue = this.CurrentValue;
		this.dirty = false;
		if (this.activateAboveThreshold)
		{
			if ((currentValue > this.threshold && !base.IsSwitchedOn) || (currentValue <= this.threshold && base.IsSwitchedOn))
			{
				this.Toggle();
				return;
			}
		}
		else if ((currentValue > this.threshold && base.IsSwitchedOn) || (currentValue <= this.threshold && !base.IsSwitchedOn))
		{
			this.Toggle();
		}
	}

	// Token: 0x060041A3 RID: 16803 RVA: 0x0024CE20 File Offset: 0x0024B020
	private float GetContainedMass()
	{
		int cell = Grid.PosToCell(this);
		if (this.conduitType == ConduitType.Liquid || this.conduitType == ConduitType.Gas)
		{
			return Conduit.GetFlowManager(this.conduitType).GetContents(cell).mass;
		}
		SolidConduitFlow flowManager = SolidConduit.GetFlowManager();
		SolidConduitFlow.ConduitContents contents = flowManager.GetContents(cell);
		Pickupable pickupable = flowManager.GetPickupable(contents.pickupableHandle);
		if (pickupable != null)
		{
			return pickupable.PrimaryElement.Mass;
		}
		return 0f;
	}

	// Token: 0x17000328 RID: 808
	// (get) Token: 0x060041A4 RID: 16804 RVA: 0x000CEEA1 File Offset: 0x000CD0A1
	// (set) Token: 0x060041A5 RID: 16805 RVA: 0x000CEEA9 File Offset: 0x000CD0A9
	public float Threshold
	{
		get
		{
			return this.threshold;
		}
		set
		{
			this.threshold = value;
			this.dirty = true;
		}
	}

	// Token: 0x17000329 RID: 809
	// (get) Token: 0x060041A6 RID: 16806 RVA: 0x000CEEB9 File Offset: 0x000CD0B9
	// (set) Token: 0x060041A7 RID: 16807 RVA: 0x000CEEC1 File Offset: 0x000CD0C1
	public bool ActivateAboveThreshold
	{
		get
		{
			return this.activateAboveThreshold;
		}
		set
		{
			this.activateAboveThreshold = value;
			this.dirty = true;
		}
	}

	// Token: 0x04002D59 RID: 11609
	[SerializeField]
	[Serialize]
	protected float threshold;

	// Token: 0x04002D5A RID: 11610
	[SerializeField]
	[Serialize]
	protected bool activateAboveThreshold = true;

	// Token: 0x04002D5B RID: 11611
	[Serialize]
	private bool dirty = true;

	// Token: 0x04002D5C RID: 11612
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04002D5D RID: 11613
	private static readonly EventSystem.IntraObjectHandler<ConduitThresholdSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<ConduitThresholdSensor>(delegate(ConduitThresholdSensor component, object data)
	{
		component.OnCopySettings(data);
	});
}
