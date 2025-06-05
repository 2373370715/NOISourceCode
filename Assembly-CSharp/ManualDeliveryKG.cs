using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02001504 RID: 5380
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/ManualDeliveryKG")]
public class ManualDeliveryKG : KMonoBehaviour, ISim1000ms
{
	// Token: 0x17000724 RID: 1828
	// (get) Token: 0x06006FE5 RID: 28645 RVA: 0x000EDC2E File Offset: 0x000EBE2E
	public bool IsPaused
	{
		get
		{
			return this.paused;
		}
	}

	// Token: 0x17000725 RID: 1829
	// (get) Token: 0x06006FE6 RID: 28646 RVA: 0x000EDC36 File Offset: 0x000EBE36
	public float Capacity
	{
		get
		{
			return this.capacity;
		}
	}

	// Token: 0x17000726 RID: 1830
	// (get) Token: 0x06006FE7 RID: 28647 RVA: 0x000EDC3E File Offset: 0x000EBE3E
	// (set) Token: 0x06006FE8 RID: 28648 RVA: 0x000EDC46 File Offset: 0x000EBE46
	public Tag RequestedItemTag
	{
		get
		{
			return this.requestedItemTag;
		}
		set
		{
			this.requestedItemTag = value;
			this.AbortDelivery("Requested Item Tag Changed");
		}
	}

	// Token: 0x17000727 RID: 1831
	// (get) Token: 0x06006FE9 RID: 28649 RVA: 0x000EDC5A File Offset: 0x000EBE5A
	// (set) Token: 0x06006FEA RID: 28650 RVA: 0x000EDC62 File Offset: 0x000EBE62
	public Tag[] ForbiddenTags
	{
		get
		{
			return this.forbiddenTags;
		}
		set
		{
			this.forbiddenTags = value;
			this.AbortDelivery("Forbidden Tags Changed");
		}
	}

	// Token: 0x17000728 RID: 1832
	// (get) Token: 0x06006FEB RID: 28651 RVA: 0x000EDC76 File Offset: 0x000EBE76
	public Storage DebugStorage
	{
		get
		{
			return this.storage;
		}
	}

	// Token: 0x17000729 RID: 1833
	// (get) Token: 0x06006FEC RID: 28652 RVA: 0x000EDC7E File Offset: 0x000EBE7E
	public FetchList2 DebugFetchList
	{
		get
		{
			return this.fetchList;
		}
	}

	// Token: 0x1700072A RID: 1834
	// (get) Token: 0x06006FED RID: 28653 RVA: 0x000EDC86 File Offset: 0x000EBE86
	private float MassStoredPerUnit
	{
		get
		{
			return this.storage.GetMassAvailable(this.requestedItemTag) / this.MassPerUnit;
		}
	}

	// Token: 0x06006FEE RID: 28654 RVA: 0x00302634 File Offset: 0x00300834
	protected override void OnSpawn()
	{
		base.OnSpawn();
		DebugUtil.Assert(this.choreTypeIDHash.IsValid, "ManualDeliveryKG Must have a valid chore type specified!", base.name);
		if (this.allowPause)
		{
			base.Subscribe<ManualDeliveryKG>(493375141, ManualDeliveryKG.OnRefreshUserMenuDelegate);
			base.Subscribe<ManualDeliveryKG>(-111137758, ManualDeliveryKG.OnRefreshUserMenuDelegate);
		}
		base.Subscribe<ManualDeliveryKG>(-592767678, ManualDeliveryKG.OnOperationalChangedDelegate);
		if (this.storage != null)
		{
			this.SetStorage(this.storage);
		}
		if (this.handlePrioritizable)
		{
			Prioritizable.AddRef(base.gameObject);
		}
		if (this.userPaused && this.allowPause)
		{
			this.OnPause();
		}
	}

	// Token: 0x06006FEF RID: 28655 RVA: 0x000EDCA0 File Offset: 0x000EBEA0
	protected override void OnCleanUp()
	{
		this.AbortDelivery("ManualDeliverKG destroyed");
		if (this.handlePrioritizable)
		{
			Prioritizable.RemoveRef(base.gameObject);
		}
		base.OnCleanUp();
	}

	// Token: 0x06006FF0 RID: 28656 RVA: 0x003026E0 File Offset: 0x003008E0
	public void SetStorage(Storage storage)
	{
		if (this.storage != null)
		{
			this.storage.Unsubscribe(this.onStorageChangeSubscription);
			this.onStorageChangeSubscription = -1;
		}
		this.AbortDelivery("storage pointer changed");
		this.storage = storage;
		if (this.storage != null && base.isSpawned)
		{
			global::Debug.Assert(this.onStorageChangeSubscription == -1);
			this.onStorageChangeSubscription = this.storage.Subscribe<ManualDeliveryKG>(-1697596308, ManualDeliveryKG.OnStorageChangedDelegate);
		}
	}

	// Token: 0x06006FF1 RID: 28657 RVA: 0x000EDCC6 File Offset: 0x000EBEC6
	public void Pause(bool pause, string reason)
	{
		if (this.paused != pause)
		{
			this.paused = pause;
			if (pause)
			{
				this.AbortDelivery(reason);
			}
		}
	}

	// Token: 0x06006FF2 RID: 28658 RVA: 0x000EDCE2 File Offset: 0x000EBEE2
	public void Sim1000ms(float dt)
	{
		this.UpdateDeliveryState();
	}

	// Token: 0x06006FF3 RID: 28659 RVA: 0x000EDCEA File Offset: 0x000EBEEA
	[ContextMenu("UpdateDeliveryState")]
	public void UpdateDeliveryState()
	{
		if (!this.requestedItemTag.IsValid)
		{
			return;
		}
		if (this.storage == null)
		{
			return;
		}
		this.UpdateFetchList();
	}

	// Token: 0x06006FF4 RID: 28660 RVA: 0x00302764 File Offset: 0x00300964
	public void RequestDelivery()
	{
		if (this.fetchList != null)
		{
			return;
		}
		float massStoredPerUnit = this.MassStoredPerUnit;
		if (massStoredPerUnit < this.capacity)
		{
			this.CreateFetchChore(massStoredPerUnit);
		}
	}

	// Token: 0x06006FF5 RID: 28661 RVA: 0x00302794 File Offset: 0x00300994
	private void CreateFetchChore(float stored_mass)
	{
		float num = this.capacity - stored_mass;
		num = Mathf.Max(PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT, num);
		if (this.RoundFetchAmountToInt)
		{
			num = (float)((int)num);
			if (num < 0.1f)
			{
				return;
			}
		}
		ChoreType byHash = Db.Get().ChoreTypes.GetByHash(this.choreTypeIDHash);
		this.fetchList = new FetchList2(this.storage, byHash);
		this.fetchList.ShowStatusItem = this.ShowStatusItem;
		this.fetchList.MinimumAmount[this.requestedItemTag] = Mathf.Max(PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT, this.MinimumMass);
		FetchList2 fetchList = this.fetchList;
		Tag tag = this.requestedItemTag;
		float amount = num;
		fetchList.Add(tag, this.forbiddenTags, amount, Operational.State.None);
		this.fetchList.Submit(new System.Action(this.OnFetchComplete), false);
	}

	// Token: 0x06006FF6 RID: 28662 RVA: 0x00302860 File Offset: 0x00300A60
	private void OnFetchComplete()
	{
		if (this.FillToCapacity && this.storage != null)
		{
			float amountAvailable = this.storage.GetAmountAvailable(this.requestedItemTag);
			if (amountAvailable < this.capacity)
			{
				this.CreateFetchChore(amountAvailable);
			}
		}
	}

	// Token: 0x06006FF7 RID: 28663 RVA: 0x003028A8 File Offset: 0x00300AA8
	private void UpdateFetchList()
	{
		if (this.paused)
		{
			return;
		}
		if (this.fetchList != null && this.fetchList.IsComplete)
		{
			this.fetchList = null;
		}
		if (!(this.operational == null) && !this.operational.MeetsRequirements(this.operationalRequirement))
		{
			if (this.fetchList != null)
			{
				this.fetchList.Cancel("Operational requirements");
				this.fetchList = null;
				return;
			}
		}
		else if (this.fetchList == null && this.MassStoredPerUnit < this.refillMass)
		{
			this.RequestDelivery();
		}
	}

	// Token: 0x06006FF8 RID: 28664 RVA: 0x000EDD0F File Offset: 0x000EBF0F
	public void AbortDelivery(string reason)
	{
		if (this.fetchList != null)
		{
			FetchList2 fetchList = this.fetchList;
			this.fetchList = null;
			fetchList.Cancel(reason);
		}
	}

	// Token: 0x06006FF9 RID: 28665 RVA: 0x000EDCE2 File Offset: 0x000EBEE2
	protected void OnStorageChanged(object data)
	{
		this.UpdateDeliveryState();
	}

	// Token: 0x06006FFA RID: 28666 RVA: 0x000EDD2C File Offset: 0x000EBF2C
	private void OnPause()
	{
		this.userPaused = true;
		this.Pause(true, "Forbid manual delivery");
	}

	// Token: 0x06006FFB RID: 28667 RVA: 0x000EDD41 File Offset: 0x000EBF41
	private void OnResume()
	{
		this.userPaused = false;
		this.Pause(false, "Allow manual delivery");
	}

	// Token: 0x06006FFC RID: 28668 RVA: 0x00302938 File Offset: 0x00300B38
	private void OnRefreshUserMenu(object data)
	{
		if (!this.allowPause)
		{
			return;
		}
		KIconButtonMenu.ButtonInfo button = (!this.paused) ? new KIconButtonMenu.ButtonInfo("action_move_to_storage", UI.USERMENUACTIONS.MANUAL_DELIVERY.NAME, new System.Action(this.OnPause), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.MANUAL_DELIVERY.TOOLTIP, true) : new KIconButtonMenu.ButtonInfo("action_move_to_storage", UI.USERMENUACTIONS.MANUAL_DELIVERY.NAME_OFF, new System.Action(this.OnResume), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.MANUAL_DELIVERY.TOOLTIP_OFF, true);
		Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
	}

	// Token: 0x06006FFD RID: 28669 RVA: 0x000EDCE2 File Offset: 0x000EBEE2
	private void OnOperationalChanged(object data)
	{
		this.UpdateDeliveryState();
	}

	// Token: 0x04005403 RID: 21507
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04005404 RID: 21508
	[SerializeField]
	private Storage storage;

	// Token: 0x04005405 RID: 21509
	[SerializeField]
	public Tag requestedItemTag;

	// Token: 0x04005406 RID: 21510
	private Tag[] forbiddenTags;

	// Token: 0x04005407 RID: 21511
	[SerializeField]
	public float capacity = 100f;

	// Token: 0x04005408 RID: 21512
	[SerializeField]
	public float refillMass = 10f;

	// Token: 0x04005409 RID: 21513
	[SerializeField]
	public float MinimumMass = 10f;

	// Token: 0x0400540A RID: 21514
	[SerializeField]
	public bool RoundFetchAmountToInt;

	// Token: 0x0400540B RID: 21515
	[SerializeField]
	public float MassPerUnit = 1f;

	// Token: 0x0400540C RID: 21516
	[SerializeField]
	public bool FillToCapacity;

	// Token: 0x0400540D RID: 21517
	[SerializeField]
	public Operational.State operationalRequirement;

	// Token: 0x0400540E RID: 21518
	[SerializeField]
	public bool allowPause;

	// Token: 0x0400540F RID: 21519
	[SerializeField]
	private bool paused;

	// Token: 0x04005410 RID: 21520
	[SerializeField]
	public HashedString choreTypeIDHash;

	// Token: 0x04005411 RID: 21521
	[Serialize]
	private bool userPaused;

	// Token: 0x04005412 RID: 21522
	public bool handlePrioritizable = true;

	// Token: 0x04005413 RID: 21523
	public bool ShowStatusItem = true;

	// Token: 0x04005414 RID: 21524
	private FetchList2 fetchList;

	// Token: 0x04005415 RID: 21525
	private int onStorageChangeSubscription = -1;

	// Token: 0x04005416 RID: 21526
	private static readonly EventSystem.IntraObjectHandler<ManualDeliveryKG> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<ManualDeliveryKG>(delegate(ManualDeliveryKG component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x04005417 RID: 21527
	private static readonly EventSystem.IntraObjectHandler<ManualDeliveryKG> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<ManualDeliveryKG>(delegate(ManualDeliveryKG component, object data)
	{
		component.OnOperationalChanged(data);
	});

	// Token: 0x04005418 RID: 21528
	private static readonly EventSystem.IntraObjectHandler<ManualDeliveryKG> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<ManualDeliveryKG>(delegate(ManualDeliveryKG component, object data)
	{
		component.OnStorageChanged(data);
	});
}
