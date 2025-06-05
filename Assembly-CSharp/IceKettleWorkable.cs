using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000E45 RID: 3653
public class IceKettleWorkable : Workable
{
	// Token: 0x17000379 RID: 889
	// (get) Token: 0x06004768 RID: 18280 RVA: 0x000D2B6B File Offset: 0x000D0D6B
	// (set) Token: 0x06004769 RID: 18281 RVA: 0x000D2B73 File Offset: 0x000D0D73
	public MeterController meter { get; private set; }

	// Token: 0x0600476A RID: 18282 RVA: 0x00260464 File Offset: 0x0025E664
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target",
			"meter_arrow",
			"meter_scale"
		});
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_icemelter_kettle_kanim")
		};
		this.synchronizeAnims = true;
		base.SetOffsets(new CellOffset[]
		{
			this.workCellOffset
		});
		base.SetWorkTime(5f);
		this.resetProgressOnStop = true;
		this.showProgressBar = false;
		this.storage.onDestroyItemsDropped = new Action<List<GameObject>>(this.RestoreStoredItemsInteractions);
		this.handler = base.Subscribe(-1697596308, new Action<object>(this.OnStorageChanged));
	}

	// Token: 0x0600476B RID: 18283 RVA: 0x000D2B7C File Offset: 0x000D0D7C
	protected override void OnSpawn()
	{
		this.AdjustStoredItemsPositionsAndWorkable();
	}

	// Token: 0x0600476C RID: 18284 RVA: 0x00260540 File Offset: 0x0025E740
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		Pickupable.PickupableStartWorkInfo pickupableStartWorkInfo = (Pickupable.PickupableStartWorkInfo)worker.GetStartWorkInfo();
		this.meter.gameObject.SetActive(true);
		PrimaryElement component = pickupableStartWorkInfo.originalPickupable.GetComponent<PrimaryElement>();
		this.meter.SetSymbolTint(new KAnimHashedString("meter_fill"), component.Element.substance.colour);
		this.meter.SetSymbolTint(new KAnimHashedString("water1"), component.Element.substance.colour);
	}

	// Token: 0x0600476D RID: 18285 RVA: 0x002605C8 File Offset: 0x0025E7C8
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		float value = (this.workTime - base.WorkTimeRemaining) / this.workTime;
		this.meter.SetPositionPercent(Mathf.Clamp01(value));
		return base.OnWorkTick(worker, dt);
	}

	// Token: 0x0600476E RID: 18286 RVA: 0x00260604 File Offset: 0x0025E804
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Storage component = worker.GetComponent<Storage>();
		Pickupable.PickupableStartWorkInfo pickupableStartWorkInfo = (Pickupable.PickupableStartWorkInfo)worker.GetStartWorkInfo();
		if (pickupableStartWorkInfo.amount > 0f)
		{
			this.storage.TransferMass(component, pickupableStartWorkInfo.originalPickupable.KPrefabID.PrefabID(), pickupableStartWorkInfo.amount, false, false, false);
		}
		GameObject gameObject = component.FindFirst(pickupableStartWorkInfo.originalPickupable.KPrefabID.PrefabID());
		if (gameObject != null)
		{
			pickupableStartWorkInfo.setResultCb(gameObject);
		}
		else
		{
			pickupableStartWorkInfo.setResultCb(null);
		}
		base.OnCompleteWork(worker);
		foreach (GameObject gameObject2 in component.items)
		{
			if (gameObject2.HasTag(GameTags.Liquid))
			{
				Pickupable component2 = gameObject2.GetComponent<Pickupable>();
				this.RestorePickupableInteractions(component2);
			}
		}
	}

	// Token: 0x0600476F RID: 18287 RVA: 0x000D2B84 File Offset: 0x000D0D84
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		this.meter.gameObject.SetActive(false);
	}

	// Token: 0x06004770 RID: 18288 RVA: 0x000D2B7C File Offset: 0x000D0D7C
	private void OnStorageChanged(object obj)
	{
		this.AdjustStoredItemsPositionsAndWorkable();
	}

	// Token: 0x06004771 RID: 18289 RVA: 0x002606F8 File Offset: 0x0025E8F8
	private void AdjustStoredItemsPositionsAndWorkable()
	{
		int cell = Grid.PosToCell(this);
		Vector3 position = Grid.CellToPosCCC(Grid.OffsetCell(cell, new CellOffset(0, 0)), Grid.SceneLayer.Ore);
		foreach (GameObject gameObject in this.storage.items)
		{
			Pickupable component = gameObject.GetComponent<Pickupable>();
			component.transform.SetPosition(position);
			component.UpdateCachedCell(cell);
			this.OverridePickupableInteractions(component);
		}
	}

	// Token: 0x06004772 RID: 18290 RVA: 0x000D2B9E File Offset: 0x000D0D9E
	private void OverridePickupableInteractions(Pickupable pickupable)
	{
		pickupable.AddTag(GameTags.LiquidSource);
		pickupable.targetWorkable = this;
		pickupable.SetOffsets(new CellOffset[]
		{
			this.workCellOffset
		});
	}

	// Token: 0x06004773 RID: 18291 RVA: 0x000D2BCB File Offset: 0x000D0DCB
	private void RestorePickupableInteractions(Pickupable pickupable)
	{
		pickupable.RemoveTag(GameTags.LiquidSource);
		pickupable.targetWorkable = pickupable;
		pickupable.SetOffsetTable(OffsetGroups.InvertedStandardTable);
	}

	// Token: 0x06004774 RID: 18292 RVA: 0x00260788 File Offset: 0x0025E988
	private void RestoreStoredItemsInteractions(List<GameObject> specificItems = null)
	{
		specificItems = ((specificItems == null) ? this.storage.items : specificItems);
		foreach (GameObject gameObject in specificItems)
		{
			Pickupable component = gameObject.GetComponent<Pickupable>();
			this.RestorePickupableInteractions(component);
		}
	}

	// Token: 0x06004775 RID: 18293 RVA: 0x002607F0 File Offset: 0x0025E9F0
	protected override void OnCleanUp()
	{
		if (base.worker != null)
		{
			ChoreDriver component = base.worker.GetComponent<ChoreDriver>();
			base.worker.StopWork();
			component.StopChore();
		}
		this.RestoreStoredItemsInteractions(null);
		base.Unsubscribe(this.handler);
		base.OnCleanUp();
	}

	// Token: 0x040031FE RID: 12798
	public Storage storage;

	// Token: 0x040031FF RID: 12799
	private int handler;

	// Token: 0x04003201 RID: 12801
	public CellOffset workCellOffset = new CellOffset(0, 0);
}
