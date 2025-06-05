using System;
using KSerialization;
using UnityEngine;

// Token: 0x02001064 RID: 4196
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/Valve")]
public class Valve : Workable, ISaveLoadable
{
	// Token: 0x170004E7 RID: 1255
	// (get) Token: 0x06005541 RID: 21825 RVA: 0x000DBF17 File Offset: 0x000DA117
	public float QueuedMaxFlow
	{
		get
		{
			if (this.chore == null)
			{
				return -1f;
			}
			return this.desiredFlow;
		}
	}

	// Token: 0x170004E8 RID: 1256
	// (get) Token: 0x06005542 RID: 21826 RVA: 0x000DBF2D File Offset: 0x000DA12D
	public float DesiredFlow
	{
		get
		{
			return this.desiredFlow;
		}
	}

	// Token: 0x170004E9 RID: 1257
	// (get) Token: 0x06005543 RID: 21827 RVA: 0x000DBF35 File Offset: 0x000DA135
	public float MaxFlow
	{
		get
		{
			return this.valveBase.MaxFlow;
		}
	}

	// Token: 0x06005544 RID: 21828 RVA: 0x0028C150 File Offset: 0x0028A350
	private void OnCopySettings(object data)
	{
		Valve component = ((GameObject)data).GetComponent<Valve>();
		if (component != null)
		{
			this.ChangeFlow(component.desiredFlow);
		}
	}

	// Token: 0x06005545 RID: 21829 RVA: 0x0028C180 File Offset: 0x0028A380
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
		this.synchronizeAnims = false;
		this.valveBase.CurrentFlow = this.valveBase.MaxFlow;
		this.desiredFlow = this.valveBase.MaxFlow;
		base.Subscribe<Valve>(-905833192, Valve.OnCopySettingsDelegate);
	}

	// Token: 0x06005546 RID: 21830 RVA: 0x000DBF42 File Offset: 0x000DA142
	protected override void OnSpawn()
	{
		this.ChangeFlow(this.desiredFlow);
		base.OnSpawn();
		Prioritizable.AddRef(base.gameObject);
	}

	// Token: 0x06005547 RID: 21831 RVA: 0x000DBF61 File Offset: 0x000DA161
	protected override void OnCleanUp()
	{
		Prioritizable.RemoveRef(base.gameObject);
		base.OnCleanUp();
	}

	// Token: 0x06005548 RID: 21832 RVA: 0x0028C1E0 File Offset: 0x0028A3E0
	public void ChangeFlow(float amount)
	{
		this.desiredFlow = Mathf.Clamp(amount, 0f, this.valveBase.MaxFlow);
		KSelectable component = base.GetComponent<KSelectable>();
		component.ToggleStatusItem(Db.Get().BuildingStatusItems.PumpingLiquidOrGas, this.desiredFlow >= 0f, this.valveBase.AccumulatorHandle);
		if (DebugHandler.InstantBuildMode)
		{
			this.UpdateFlow();
			return;
		}
		if (this.desiredFlow == this.valveBase.CurrentFlow)
		{
			if (this.chore != null)
			{
				this.chore.Cancel("desiredFlow == currentFlow");
				this.chore = null;
			}
			component.RemoveStatusItem(Db.Get().BuildingStatusItems.ValveRequest, false);
			component.RemoveStatusItem(Db.Get().BuildingStatusItems.PendingWork, false);
			return;
		}
		if (this.chore == null)
		{
			component.AddStatusItem(Db.Get().BuildingStatusItems.ValveRequest, this);
			component.AddStatusItem(Db.Get().BuildingStatusItems.PendingWork, this);
			this.chore = new WorkChore<Valve>(Db.Get().ChoreTypes.Toggle, this, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			return;
		}
	}

	// Token: 0x06005549 RID: 21833 RVA: 0x000DBF74 File Offset: 0x000DA174
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		this.UpdateFlow();
	}

	// Token: 0x0600554A RID: 21834 RVA: 0x0028C31C File Offset: 0x0028A51C
	public void UpdateFlow()
	{
		this.valveBase.CurrentFlow = this.desiredFlow;
		this.valveBase.UpdateAnim();
		if (this.chore != null)
		{
			this.chore.Cancel("forced complete");
		}
		this.chore = null;
		KSelectable component = base.GetComponent<KSelectable>();
		component.RemoveStatusItem(Db.Get().BuildingStatusItems.ValveRequest, false);
		component.RemoveStatusItem(Db.Get().BuildingStatusItems.PendingWork, false);
	}

	// Token: 0x04003C42 RID: 15426
	[MyCmpReq]
	private ValveBase valveBase;

	// Token: 0x04003C43 RID: 15427
	[Serialize]
	private float desiredFlow = 0.5f;

	// Token: 0x04003C44 RID: 15428
	private Chore chore;

	// Token: 0x04003C45 RID: 15429
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04003C46 RID: 15430
	private static readonly EventSystem.IntraObjectHandler<Valve> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<Valve>(delegate(Valve component, object data)
	{
		component.OnCopySettings(data);
	});
}
