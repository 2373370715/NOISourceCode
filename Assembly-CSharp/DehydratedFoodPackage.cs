using System;
using System.Linq;
using FoodRehydrator;
using KSerialization;
using UnityEngine;

// Token: 0x02001252 RID: 4690
public class DehydratedFoodPackage : Workable, IApproachable
{
	// Token: 0x170005C6 RID: 1478
	// (get) Token: 0x06005FD5 RID: 24533 RVA: 0x002B815C File Offset: 0x002B635C
	// (set) Token: 0x06005FD6 RID: 24534 RVA: 0x000AA038 File Offset: 0x000A8238
	public GameObject Rehydrator
	{
		get
		{
			Storage storage = base.gameObject.GetComponent<Pickupable>().storage;
			if (storage != null)
			{
				return storage.gameObject;
			}
			return null;
		}
		private set
		{
		}
	}

	// Token: 0x06005FD7 RID: 24535 RVA: 0x000E2F9F File Offset: 0x000E119F
	public override BuildingFacade GetBuildingFacade()
	{
		if (!(this.Rehydrator != null))
		{
			return null;
		}
		return this.Rehydrator.GetComponent<BuildingFacade>();
	}

	// Token: 0x06005FD8 RID: 24536 RVA: 0x000E2FBC File Offset: 0x000E11BC
	public override KAnimControllerBase GetAnimController()
	{
		if (!(this.Rehydrator != null))
		{
			return null;
		}
		return this.Rehydrator.GetComponent<KAnimControllerBase>();
	}

	// Token: 0x06005FD9 RID: 24537 RVA: 0x002B818C File Offset: 0x002B638C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.SetOffsets(new CellOffset[]
		{
			default(CellOffset),
			new CellOffset(0, -1)
		});
		if (this.storage.items.Count < 1)
		{
			this.storage.ConsumeAllIgnoringDisease(this.FoodTag);
			int cell = Grid.PosToCell(this);
			GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(this.FoodTag), Grid.CellToPosCBC(cell, Grid.SceneLayer.Creatures), Grid.SceneLayer.Creatures, null, 0);
			gameObject.SetActive(true);
			gameObject.GetComponent<Edible>().Calories = 1000000f;
			this.storage.Store(gameObject, false, false, true, false);
		}
		base.Subscribe(-1697596308, new Action<object>(this.StorageChangeHandler));
		this.DehydrateItem(this.storage.items.ElementAtOrDefault(0));
	}

	// Token: 0x06005FDA RID: 24538 RVA: 0x002B8258 File Offset: 0x002B6458
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		if (this.Rehydrator != null)
		{
			DehydratedManager component = this.Rehydrator.GetComponent<DehydratedManager>();
			if (component != null)
			{
				component.SetFabricatedFoodSymbol(this.FoodTag);
			}
			this.Rehydrator.GetComponent<AccessabilityManager>().SetActiveWorkable(this);
		}
	}

	// Token: 0x06005FDB RID: 24539 RVA: 0x002B82AC File Offset: 0x002B64AC
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		if (this.storage.items.Count != 1)
		{
			DebugUtil.DevAssert(false, "OnCompleteWork invalid contents of package", null);
			return;
		}
		GameObject gameObject = this.storage.items[0];
		this.storage.Transfer(worker.GetComponent<Storage>(), false, false);
		DebugUtil.DevAssert(this.Rehydrator != null, "OnCompleteWork but no rehydrator", null);
		DehydratedManager component = this.Rehydrator.GetComponent<DehydratedManager>();
		this.Rehydrator.GetComponent<AccessabilityManager>().SetActiveWorkable(null);
		component.ConsumeResourcesForRehydration(base.gameObject, gameObject);
		DehydratedFoodPackage.RehydrateStartWorkItem rehydrateStartWorkItem = (DehydratedFoodPackage.RehydrateStartWorkItem)worker.GetStartWorkInfo();
		if (rehydrateStartWorkItem != null && rehydrateStartWorkItem.setResultCb != null && gameObject != null)
		{
			rehydrateStartWorkItem.setResultCb(gameObject);
		}
	}

	// Token: 0x06005FDC RID: 24540 RVA: 0x000E2FD9 File Offset: 0x000E11D9
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		if (this.Rehydrator != null)
		{
			this.Rehydrator.GetComponent<AccessabilityManager>().SetActiveWorkable(null);
		}
	}

	// Token: 0x06005FDD RID: 24541 RVA: 0x000E3001 File Offset: 0x000E1201
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x06005FDE RID: 24542 RVA: 0x002B8370 File Offset: 0x002B6570
	private void StorageChangeHandler(object obj)
	{
		GameObject item = (GameObject)obj;
		DebugUtil.DevAssert(!this.storage.items.Contains(item), "Attempting to add item to a dehydrated food package which is not allowed", null);
		this.RehydrateItem(item);
	}

	// Token: 0x06005FDF RID: 24543 RVA: 0x002B83AC File Offset: 0x002B65AC
	public void DehydrateItem(GameObject item)
	{
		DebugUtil.DevAssert(item != null, "Attempting to dehydrate contents of an empty packet", null);
		if (this.storage.items.Count != 1 || item == null)
		{
			DebugUtil.DevAssert(false, "DehydrateItem called, incorrect content", null);
			return;
		}
		item.AddTag(GameTags.Dehydrated);
	}

	// Token: 0x06005FE0 RID: 24544 RVA: 0x002B8400 File Offset: 0x002B6600
	public void RehydrateItem(GameObject item)
	{
		if (this.storage.items.Count != 0)
		{
			DebugUtil.DevAssert(false, "RehydrateItem called, incorrect storage content", null);
			return;
		}
		item.RemoveTag(GameTags.Dehydrated);
		item.AddTag(GameTags.Rehydrated);
		item.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.RehydratedFood, null);
	}

	// Token: 0x06005FE1 RID: 24545 RVA: 0x002B8464 File Offset: 0x002B6664
	private void Swap<Type>(ref Type a, ref Type b)
	{
		Type type = a;
		a = b;
		b = type;
	}

	// Token: 0x040044B4 RID: 17588
	[Serialize]
	public Tag FoodTag;

	// Token: 0x040044B5 RID: 17589
	[MyCmpReq]
	private Storage storage;

	// Token: 0x02001253 RID: 4691
	public class RehydrateStartWorkItem : WorkerBase.StartWorkInfo
	{
		// Token: 0x06005FE3 RID: 24547 RVA: 0x000E3009 File Offset: 0x000E1209
		public RehydrateStartWorkItem(DehydratedFoodPackage pkg, Action<GameObject> setResultCB) : base(pkg)
		{
			this.package = pkg;
			this.setResultCb = setResultCB;
		}

		// Token: 0x040044B6 RID: 17590
		public DehydratedFoodPackage package;

		// Token: 0x040044B7 RID: 17591
		public Action<GameObject> setResultCb;
	}
}
