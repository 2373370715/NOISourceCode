using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000373 RID: 883
public class PajamaDispenser : Workable, IDispenser
{
	// Token: 0x14000001 RID: 1
	// (add) Token: 0x06000E06 RID: 3590 RVA: 0x00181574 File Offset: 0x0017F774
	// (remove) Token: 0x06000E07 RID: 3591 RVA: 0x001815AC File Offset: 0x0017F7AC
	public event System.Action OnStopWorkEvent;

	// Token: 0x17000039 RID: 57
	// (get) Token: 0x06000E08 RID: 3592 RVA: 0x000B0882 File Offset: 0x000AEA82
	// (set) Token: 0x06000E09 RID: 3593 RVA: 0x001815E4 File Offset: 0x0017F7E4
	private WorkChore<PajamaDispenser> Chore
	{
		get
		{
			return this.chore;
		}
		set
		{
			this.chore = value;
			if (this.chore != null)
			{
				base.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.DispenseRequested, null);
				return;
			}
			base.gameObject.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.DispenseRequested, true);
		}
	}

	// Token: 0x06000E0A RID: 3594 RVA: 0x000B088A File Offset: 0x000AEA8A
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (PajamaDispenser.pajamaPrefab != null)
		{
			return;
		}
		PajamaDispenser.pajamaPrefab = Assets.GetPrefab(new Tag("SleepClinicPajamas"));
	}

	// Token: 0x06000E0B RID: 3595 RVA: 0x00181644 File Offset: 0x0017F844
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Vector3 targetPoint = this.GetTargetPoint();
		targetPoint.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingFront);
		Util.KInstantiate(PajamaDispenser.pajamaPrefab, targetPoint, Quaternion.identity, null, null, true, 0).SetActive(true);
		this.hasDispenseChore = false;
	}

	// Token: 0x06000E0C RID: 3596 RVA: 0x00181688 File Offset: 0x0017F888
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		if (this.Chore != null && this.Chore.smi.IsRunning())
		{
			this.Chore.Cancel("work interrupted");
		}
		this.Chore = null;
		if (this.hasDispenseChore)
		{
			this.FetchPajamas();
		}
		if (this.OnStopWorkEvent != null)
		{
			this.OnStopWorkEvent();
		}
	}

	// Token: 0x06000E0D RID: 3597 RVA: 0x001816F0 File Offset: 0x0017F8F0
	[ContextMenu("fetch")]
	public void FetchPajamas()
	{
		if (this.Chore != null)
		{
			return;
		}
		this.hasDispenseChore = true;
		this.Chore = new WorkChore<PajamaDispenser>(Db.Get().ChoreTypes.EquipmentFetch, this, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, false);
		this.Chore.AddPrecondition(ChorePreconditions.instance.IsNotARobot, null);
	}

	// Token: 0x06000E0E RID: 3598 RVA: 0x00181750 File Offset: 0x0017F950
	public void CancelFetch()
	{
		if (this.Chore == null)
		{
			return;
		}
		this.Chore.Cancel("User Cancelled");
		this.Chore = null;
		this.hasDispenseChore = false;
		base.gameObject.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.DispenseRequested, false);
	}

	// Token: 0x06000E0F RID: 3599 RVA: 0x000B08B4 File Offset: 0x000AEAB4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.hasDispenseChore)
		{
			this.FetchPajamas();
		}
	}

	// Token: 0x06000E10 RID: 3600 RVA: 0x000B08CA File Offset: 0x000AEACA
	public List<Tag> DispensedItems()
	{
		return PajamaDispenser.PajamaList;
	}

	// Token: 0x06000E11 RID: 3601 RVA: 0x000B08D1 File Offset: 0x000AEAD1
	public Tag SelectedItem()
	{
		return PajamaDispenser.PajamaList[0];
	}

	// Token: 0x06000E12 RID: 3602 RVA: 0x000AA038 File Offset: 0x000A8238
	public void SelectItem(Tag tag)
	{
	}

	// Token: 0x06000E13 RID: 3603 RVA: 0x000B08DE File Offset: 0x000AEADE
	public void OnOrderDispense()
	{
		this.FetchPajamas();
	}

	// Token: 0x06000E14 RID: 3604 RVA: 0x000B08E6 File Offset: 0x000AEAE6
	public void OnCancelDispense()
	{
		this.CancelFetch();
	}

	// Token: 0x06000E15 RID: 3605 RVA: 0x000B08EE File Offset: 0x000AEAEE
	public bool HasOpenChore()
	{
		return this.Chore != null;
	}

	// Token: 0x04000A6E RID: 2670
	[Serialize]
	private bool hasDispenseChore;

	// Token: 0x04000A6F RID: 2671
	private static GameObject pajamaPrefab = null;

	// Token: 0x04000A71 RID: 2673
	private WorkChore<PajamaDispenser> chore;

	// Token: 0x04000A72 RID: 2674
	private static List<Tag> PajamaList = new List<Tag>
	{
		"SleepClinicPajamas"
	};
}
