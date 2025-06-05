using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200109C RID: 4252
[AddComponentMenu("KMonoBehaviour/Workable/Carvable")]
public class Carvable : Workable, IDigActionEntity
{
	// Token: 0x170004F7 RID: 1271
	// (get) Token: 0x0600564E RID: 22094 RVA: 0x000DCB99 File Offset: 0x000DAD99
	public bool IsMarkedForCarve
	{
		get
		{
			return this.isMarkedForCarve;
		}
	}

	// Token: 0x0600564F RID: 22095 RVA: 0x0028F5E8 File Offset: 0x0028D7E8
	protected Carvable()
	{
		this.buttonLabel = UI.USERMENUACTIONS.CARVE.NAME;
		this.buttonTooltip = UI.USERMENUACTIONS.CARVE.TOOLTIP;
		this.cancelButtonLabel = UI.USERMENUACTIONS.CANCELCARVE.NAME;
		this.cancelButtonTooltip = UI.USERMENUACTIONS.CANCELCARVE.TOOLTIP;
	}

	// Token: 0x06005650 RID: 22096 RVA: 0x0028F644 File Offset: 0x0028D844
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.pendingStatusItem = new StatusItem("PendingCarve", "MISC", "status_item_pending_carve", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
		this.workerStatusItem = new StatusItem("Carving", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
		this.workerStatusItem.resolveStringCallback = delegate(string str, object data)
		{
			Workable workable = (Workable)data;
			if (workable != null && workable.GetComponent<KSelectable>() != null)
			{
				str = str.Replace("{Target}", workable.GetComponent<KSelectable>().GetName());
			}
			return str;
		};
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_sculpture_kanim")
		};
		this.synchronizeAnims = false;
	}

	// Token: 0x06005651 RID: 22097 RVA: 0x0028F6F8 File Offset: 0x0028D8F8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.SetWorkTime(10f);
		base.Subscribe<Carvable>(2127324410, Carvable.OnCancelDelegate);
		base.Subscribe<Carvable>(493375141, Carvable.OnRefreshUserMenuDelegate);
		this.faceTargetWhenWorking = true;
		Prioritizable.AddRef(base.gameObject);
		Extents extents = new Extents(Grid.PosToCell(base.gameObject), base.gameObject.GetComponent<OccupyArea>().OccupiedCellsOffsets);
		this.partitionerEntry = GameScenePartitioner.Instance.Add(base.gameObject.name, base.gameObject.GetComponent<KPrefabID>(), extents, GameScenePartitioner.Instance.plants, null);
		if (this.isMarkedForCarve)
		{
			this.MarkForCarve(true);
		}
	}

	// Token: 0x06005652 RID: 22098 RVA: 0x0028F7B0 File Offset: 0x0028D9B0
	public void Carve()
	{
		this.isMarkedForCarve = false;
		this.chore = null;
		base.GetComponent<KSelectable>().RemoveStatusItem(this.pendingStatusItem, false);
		base.GetComponent<KSelectable>().RemoveStatusItem(this.workerStatusItem, false);
		Game.Instance.userMenu.Refresh(base.gameObject);
		this.ProducePickupable(this.dropItemPrefabId);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06005653 RID: 22099 RVA: 0x0028F820 File Offset: 0x0028DA20
	public void MarkForCarve(bool instantOnDebug = true)
	{
		if (DebugHandler.InstantBuildMode && instantOnDebug)
		{
			this.Carve();
			return;
		}
		if (this.chore == null)
		{
			this.isMarkedForCarve = true;
			this.chore = new WorkChore<Carvable>(Db.Get().ChoreTypes.Dig, this, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			this.chore.AddPrecondition(ChorePreconditions.instance.IsNotARobot, null);
			base.GetComponent<KSelectable>().AddStatusItem(this.pendingStatusItem, this);
		}
	}

	// Token: 0x06005654 RID: 22100 RVA: 0x000DCBA1 File Offset: 0x000DADA1
	protected override void OnCompleteWork(WorkerBase worker)
	{
		this.Carve();
	}

	// Token: 0x06005655 RID: 22101 RVA: 0x0028F8A4 File Offset: 0x0028DAA4
	private void OnCancel(object data)
	{
		if (this.chore != null)
		{
			this.chore.Cancel("Cancel uproot");
			this.chore = null;
			base.GetComponent<KSelectable>().RemoveStatusItem(this.pendingStatusItem, false);
		}
		this.isMarkedForCarve = false;
		Game.Instance.userMenu.Refresh(base.gameObject);
	}

	// Token: 0x06005656 RID: 22102 RVA: 0x000DCBA9 File Offset: 0x000DADA9
	private void OnClickCarve()
	{
		this.MarkForCarve(true);
	}

	// Token: 0x06005657 RID: 22103 RVA: 0x000DCBB2 File Offset: 0x000DADB2
	protected void OnClickCancelCarve()
	{
		this.OnCancel(null);
	}

	// Token: 0x06005658 RID: 22104 RVA: 0x0028F900 File Offset: 0x0028DB00
	private void OnRefreshUserMenu(object data)
	{
		if (!this.showUserMenuButtons)
		{
			return;
		}
		KIconButtonMenu.ButtonInfo button = (this.chore != null) ? new KIconButtonMenu.ButtonInfo("action_carve", this.cancelButtonLabel, new System.Action(this.OnClickCancelCarve), global::Action.NumActions, null, null, null, this.cancelButtonTooltip, true) : new KIconButtonMenu.ButtonInfo("action_carve", this.buttonLabel, new System.Action(this.OnClickCarve), global::Action.NumActions, null, null, null, this.buttonTooltip, true);
		Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
	}

	// Token: 0x06005659 RID: 22105 RVA: 0x000DCBBB File Offset: 0x000DADBB
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
	}

	// Token: 0x0600565A RID: 22106 RVA: 0x000DCBD3 File Offset: 0x000DADD3
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		base.GetComponent<KSelectable>().RemoveStatusItem(this.pendingStatusItem, false);
	}

	// Token: 0x0600565B RID: 22107 RVA: 0x0028F994 File Offset: 0x0028DB94
	private GameObject ProducePickupable(string pickupablePrefabId)
	{
		if (pickupablePrefabId != null)
		{
			Vector3 position = base.gameObject.transform.GetPosition() + new Vector3(0f, 0.5f, 0f);
			GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(new Tag(pickupablePrefabId)), position, Grid.SceneLayer.Ore, null, 0);
			PrimaryElement component = base.gameObject.GetComponent<PrimaryElement>();
			gameObject.GetComponent<PrimaryElement>().Temperature = component.Temperature;
			gameObject.SetActive(true);
			string properName = gameObject.GetProperName();
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, properName, gameObject.transform, 1.5f, false);
			return gameObject;
		}
		return null;
	}

	// Token: 0x0600565C RID: 22108 RVA: 0x000DCBA1 File Offset: 0x000DADA1
	public void Dig()
	{
		this.Carve();
	}

	// Token: 0x0600565D RID: 22109 RVA: 0x000DCBEF File Offset: 0x000DADEF
	public void MarkForDig(bool instantOnDebug = true)
	{
		this.MarkForCarve(instantOnDebug);
	}

	// Token: 0x04003D18 RID: 15640
	[Serialize]
	protected bool isMarkedForCarve;

	// Token: 0x04003D19 RID: 15641
	protected Chore chore;

	// Token: 0x04003D1A RID: 15642
	private string buttonLabel;

	// Token: 0x04003D1B RID: 15643
	private string buttonTooltip;

	// Token: 0x04003D1C RID: 15644
	private string cancelButtonLabel;

	// Token: 0x04003D1D RID: 15645
	private string cancelButtonTooltip;

	// Token: 0x04003D1E RID: 15646
	private StatusItem pendingStatusItem;

	// Token: 0x04003D1F RID: 15647
	public bool showUserMenuButtons = true;

	// Token: 0x04003D20 RID: 15648
	public string dropItemPrefabId;

	// Token: 0x04003D21 RID: 15649
	public HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04003D22 RID: 15650
	private static readonly EventSystem.IntraObjectHandler<Carvable> OnCancelDelegate = new EventSystem.IntraObjectHandler<Carvable>(delegate(Carvable component, object data)
	{
		component.OnCancel(data);
	});

	// Token: 0x04003D23 RID: 15651
	private static readonly EventSystem.IntraObjectHandler<Carvable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Carvable>(delegate(Carvable component, object data)
	{
		component.OnRefreshUserMenu(data);
	});
}
