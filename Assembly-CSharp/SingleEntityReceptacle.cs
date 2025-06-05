using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000FBC RID: 4028
[AddComponentMenu("KMonoBehaviour/Workable/SingleEntityReceptacle")]
public class SingleEntityReceptacle : Workable, IRender1000ms
{
	// Token: 0x17000484 RID: 1156
	// (get) Token: 0x0600510D RID: 20749 RVA: 0x000D9415 File Offset: 0x000D7615
	public FetchChore GetActiveRequest
	{
		get
		{
			return this.fetchChore;
		}
	}

	// Token: 0x17000485 RID: 1157
	// (get) Token: 0x0600510E RID: 20750 RVA: 0x000D941D File Offset: 0x000D761D
	// (set) Token: 0x0600510F RID: 20751 RVA: 0x000D9444 File Offset: 0x000D7644
	protected GameObject occupyingObject
	{
		get
		{
			if (this.occupyObjectRef.Get() != null)
			{
				return this.occupyObjectRef.Get().gameObject;
			}
			return null;
		}
		set
		{
			if (value == null)
			{
				this.occupyObjectRef.Set(null);
				return;
			}
			this.occupyObjectRef.Set(value.GetComponent<KSelectable>());
		}
	}

	// Token: 0x17000486 RID: 1158
	// (get) Token: 0x06005110 RID: 20752 RVA: 0x000D946D File Offset: 0x000D766D
	public GameObject Occupant
	{
		get
		{
			return this.occupyingObject;
		}
	}

	// Token: 0x17000487 RID: 1159
	// (get) Token: 0x06005111 RID: 20753 RVA: 0x000D9475 File Offset: 0x000D7675
	public IReadOnlyList<Tag> possibleDepositObjectTags
	{
		get
		{
			return this.possibleDepositTagsList;
		}
	}

	// Token: 0x06005112 RID: 20754 RVA: 0x000D947D File Offset: 0x000D767D
	public bool HasDepositTag(Tag tag)
	{
		return this.possibleDepositTagsList.Contains(tag);
	}

	// Token: 0x06005113 RID: 20755 RVA: 0x0027F0EC File Offset: 0x0027D2EC
	public bool IsValidEntity(GameObject candidate)
	{
		if (!Game.IsCorrectDlcActiveForCurrentSave(candidate.GetComponent<KPrefabID>()))
		{
			return false;
		}
		IReceptacleDirection component = candidate.GetComponent<IReceptacleDirection>();
		bool flag = this.rotatable != null || component == null || component.Direction == this.Direction;
		int num = 0;
		while (flag && num < this.additionalCriteria.Count)
		{
			flag = this.additionalCriteria[num](candidate);
			num++;
		}
		return flag;
	}

	// Token: 0x17000488 RID: 1160
	// (get) Token: 0x06005114 RID: 20756 RVA: 0x000D948B File Offset: 0x000D768B
	public SingleEntityReceptacle.ReceptacleDirection Direction
	{
		get
		{
			return this.direction;
		}
	}

	// Token: 0x06005115 RID: 20757 RVA: 0x000C1333 File Offset: 0x000BF533
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06005116 RID: 20758 RVA: 0x0027F160 File Offset: 0x0027D360
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.occupyingObject != null)
		{
			this.PositionOccupyingObject();
			this.SubscribeToOccupant();
		}
		this.UpdateStatusItem();
		if (this.occupyingObject == null && !this.requestedEntityTag.IsValid)
		{
			this.requestedEntityAdditionalFilterTag = null;
		}
		if (this.occupyingObject == null && this.requestedEntityTag.IsValid)
		{
			this.CreateOrder(this.requestedEntityTag, this.requestedEntityAdditionalFilterTag);
		}
		base.Subscribe<SingleEntityReceptacle>(-592767678, SingleEntityReceptacle.OnOperationalChangedDelegate);
	}

	// Token: 0x06005117 RID: 20759 RVA: 0x000D9493 File Offset: 0x000D7693
	public void AddDepositTag(Tag t)
	{
		this.possibleDepositTagsList.Add(t);
	}

	// Token: 0x06005118 RID: 20760 RVA: 0x000D94A1 File Offset: 0x000D76A1
	public void AddAdditionalCriteria(Func<GameObject, bool> criteria)
	{
		this.additionalCriteria.Add(criteria);
	}

	// Token: 0x06005119 RID: 20761 RVA: 0x000D94AF File Offset: 0x000D76AF
	public void SetReceptacleDirection(SingleEntityReceptacle.ReceptacleDirection d)
	{
		this.direction = d;
	}

	// Token: 0x0600511A RID: 20762 RVA: 0x000AA038 File Offset: 0x000A8238
	public virtual void SetPreview(Tag entityTag, bool solid = false)
	{
	}

	// Token: 0x0600511B RID: 20763 RVA: 0x000D94B8 File Offset: 0x000D76B8
	public virtual void CreateOrder(Tag entityTag, Tag additionalFilterTag)
	{
		this.requestedEntityTag = entityTag;
		this.requestedEntityAdditionalFilterTag = additionalFilterTag;
		this.CreateFetchChore(this.requestedEntityTag, this.requestedEntityAdditionalFilterTag);
		this.SetPreview(entityTag, true);
		this.UpdateStatusItem();
	}

	// Token: 0x0600511C RID: 20764 RVA: 0x000D94E8 File Offset: 0x000D76E8
	public void Render1000ms(float dt)
	{
		this.UpdateStatusItem();
	}

	// Token: 0x0600511D RID: 20765 RVA: 0x0027F1F8 File Offset: 0x0027D3F8
	protected virtual void UpdateStatusItem()
	{
		KSelectable component = base.GetComponent<KSelectable>();
		if (this.Occupant != null)
		{
			component.SetStatusItem(Db.Get().StatusItemCategories.EntityReceptacle, null, null);
			return;
		}
		if (this.fetchChore == null)
		{
			component.SetStatusItem(Db.Get().StatusItemCategories.EntityReceptacle, this.statusItemNeed, null);
			return;
		}
		bool flag = this.fetchChore.fetcher != null;
		WorldContainer myWorld = this.GetMyWorld();
		if (!flag && myWorld != null)
		{
			foreach (Tag tag in this.fetchChore.tags)
			{
				if (myWorld.worldInventory.GetTotalAmount(tag, true) > 0f)
				{
					if (myWorld.worldInventory.GetTotalAmount(this.requestedEntityAdditionalFilterTag, true) > 0f || this.requestedEntityAdditionalFilterTag == Tag.Invalid)
					{
						flag = true;
						break;
					}
					break;
				}
			}
		}
		if (flag)
		{
			component.SetStatusItem(Db.Get().StatusItemCategories.EntityReceptacle, this.statusItemAwaitingDelivery, null);
			return;
		}
		component.SetStatusItem(Db.Get().StatusItemCategories.EntityReceptacle, this.statusItemNoneAvailable, null);
	}

	// Token: 0x0600511E RID: 20766 RVA: 0x0027F34C File Offset: 0x0027D54C
	protected void CreateFetchChore(Tag entityTag, Tag additionalRequiredTag)
	{
		if (this.fetchChore == null && entityTag.IsValid && entityTag != GameTags.Empty)
		{
			this.fetchChore = new FetchChore(this.choreType, this.storage, 1f, new HashSet<Tag>
			{
				entityTag
			}, FetchChore.MatchCriteria.MatchID, (additionalRequiredTag.IsValid && additionalRequiredTag != GameTags.Empty) ? additionalRequiredTag : Tag.Invalid, null, null, true, new Action<Chore>(this.OnFetchComplete), delegate(Chore chore)
			{
				this.UpdateStatusItem();
			}, delegate(Chore chore)
			{
				this.UpdateStatusItem();
			}, Operational.State.Functional, 0);
			MaterialNeeds.UpdateNeed(this.requestedEntityTag, 1f, base.gameObject.GetMyWorldId());
			this.UpdateStatusItem();
		}
	}

	// Token: 0x0600511F RID: 20767 RVA: 0x000D94F0 File Offset: 0x000D76F0
	public virtual void OrderRemoveOccupant()
	{
		this.ClearOccupant();
	}

	// Token: 0x06005120 RID: 20768 RVA: 0x0027F414 File Offset: 0x0027D614
	protected virtual void ClearOccupant()
	{
		if (this.occupyingObject)
		{
			this.UnsubscribeFromOccupant();
			this.storage.DropAll(false, false, default(Vector3), true, null);
		}
		this.occupyingObject = null;
		this.UpdateActive();
		this.UpdateStatusItem();
		base.Trigger(-731304873, this.occupyingObject);
	}

	// Token: 0x06005121 RID: 20769 RVA: 0x0027F470 File Offset: 0x0027D670
	public void CancelActiveRequest()
	{
		if (this.fetchChore != null)
		{
			MaterialNeeds.UpdateNeed(this.requestedEntityTag, -1f, base.gameObject.GetMyWorldId());
			this.fetchChore.Cancel("User canceled");
			this.fetchChore = null;
		}
		this.requestedEntityTag = Tag.Invalid;
		this.requestedEntityAdditionalFilterTag = Tag.Invalid;
		this.UpdateStatusItem();
		this.SetPreview(Tag.Invalid, false);
	}

	// Token: 0x06005122 RID: 20770 RVA: 0x0027F4E0 File Offset: 0x0027D6E0
	private void OnOccupantDestroyed(object data)
	{
		this.occupyingObject = null;
		this.ClearOccupant();
		if (this.autoReplaceEntity && this.requestedEntityTag.IsValid && this.requestedEntityTag != GameTags.Empty)
		{
			this.CreateOrder(this.requestedEntityTag, this.requestedEntityAdditionalFilterTag);
		}
	}

	// Token: 0x06005123 RID: 20771 RVA: 0x000D94F8 File Offset: 0x000D76F8
	protected virtual void SubscribeToOccupant()
	{
		if (this.occupyingObject != null)
		{
			base.Subscribe(this.occupyingObject, 1969584890, new Action<object>(this.OnOccupantDestroyed));
		}
	}

	// Token: 0x06005124 RID: 20772 RVA: 0x000D9526 File Offset: 0x000D7726
	protected virtual void UnsubscribeFromOccupant()
	{
		if (this.occupyingObject != null)
		{
			base.Unsubscribe(this.occupyingObject, 1969584890, new Action<object>(this.OnOccupantDestroyed));
		}
	}

	// Token: 0x06005125 RID: 20773 RVA: 0x0027F534 File Offset: 0x0027D734
	private void OnFetchComplete(Chore chore)
	{
		if (this.fetchChore == null)
		{
			global::Debug.LogWarningFormat(base.gameObject, "{0} OnFetchComplete fetchChore null", new object[]
			{
				base.gameObject
			});
			return;
		}
		if (this.fetchChore.fetchTarget == null)
		{
			global::Debug.LogWarningFormat(base.gameObject, "{0} OnFetchComplete fetchChore.fetchTarget null", new object[]
			{
				base.gameObject
			});
			return;
		}
		this.OnDepositObject(this.fetchChore.fetchTarget.gameObject);
	}

	// Token: 0x06005126 RID: 20774 RVA: 0x000D9553 File Offset: 0x000D7753
	public void ForceDeposit(GameObject depositedObject)
	{
		if (this.occupyingObject != null)
		{
			this.ClearOccupant();
		}
		this.OnDepositObject(depositedObject);
	}

	// Token: 0x06005127 RID: 20775 RVA: 0x0027F5B4 File Offset: 0x0027D7B4
	private void OnDepositObject(GameObject depositedObject)
	{
		this.SetPreview(Tag.Invalid, false);
		MaterialNeeds.UpdateNeed(this.requestedEntityTag, -1f, base.gameObject.GetMyWorldId());
		KBatchedAnimController component = depositedObject.GetComponent<KBatchedAnimController>();
		if (component != null)
		{
			component.GetBatchInstanceData().ClearOverrideTransformMatrix();
		}
		this.occupyingObject = this.SpawnOccupyingObject(depositedObject);
		if (this.occupyingObject != null)
		{
			this.ConfigureOccupyingObject(this.occupyingObject);
			this.occupyingObject.SetActive(true);
			this.PositionOccupyingObject();
			this.SubscribeToOccupant();
		}
		else
		{
			global::Debug.LogWarning(base.gameObject.name + " EntityReceptacle did not spawn occupying entity.");
		}
		if (this.fetchChore != null)
		{
			this.fetchChore.Cancel("receptacle filled");
			this.fetchChore = null;
		}
		if (!this.autoReplaceEntity)
		{
			this.requestedEntityTag = Tag.Invalid;
			this.requestedEntityAdditionalFilterTag = Tag.Invalid;
		}
		this.UpdateActive();
		this.UpdateStatusItem();
		if (this.destroyEntityOnDeposit)
		{
			Util.KDestroyGameObject(depositedObject);
		}
		base.Trigger(-731304873, this.occupyingObject);
	}

	// Token: 0x06005128 RID: 20776 RVA: 0x000B64D6 File Offset: 0x000B46D6
	protected virtual GameObject SpawnOccupyingObject(GameObject depositedEntity)
	{
		return depositedEntity;
	}

	// Token: 0x06005129 RID: 20777 RVA: 0x000AA038 File Offset: 0x000A8238
	protected virtual void ConfigureOccupyingObject(GameObject source)
	{
	}

	// Token: 0x0600512A RID: 20778 RVA: 0x0027F6C8 File Offset: 0x0027D8C8
	protected virtual void PositionOccupyingObject()
	{
		if (this.rotatable != null)
		{
			this.occupyingObject.transform.SetPosition(base.gameObject.transform.GetPosition() + this.rotatable.GetRotatedOffset(this.occupyingObjectRelativePosition));
		}
		else
		{
			this.occupyingObject.transform.SetPosition(base.gameObject.transform.GetPosition() + this.occupyingObjectRelativePosition);
		}
		KBatchedAnimController component = this.occupyingObject.GetComponent<KBatchedAnimController>();
		component.enabled = false;
		component.enabled = true;
	}

	// Token: 0x0600512B RID: 20779 RVA: 0x0027F760 File Offset: 0x0027D960
	protected void UpdateActive()
	{
		if (this.Equals(null) || this == null || base.gameObject.Equals(null) || base.gameObject == null)
		{
			return;
		}
		if (this.operational != null)
		{
			this.operational.SetActive(this.operational.IsOperational && this.occupyingObject != null, false);
		}
	}

	// Token: 0x0600512C RID: 20780 RVA: 0x000D9570 File Offset: 0x000D7770
	protected override void OnCleanUp()
	{
		this.CancelActiveRequest();
		this.UnsubscribeFromOccupant();
		base.OnCleanUp();
	}

	// Token: 0x0600512D RID: 20781 RVA: 0x000D9584 File Offset: 0x000D7784
	private void OnOperationalChanged(object data)
	{
		this.UpdateActive();
		if (this.occupyingObject)
		{
			this.occupyingObject.Trigger(this.operational.IsOperational ? 1628751838 : 960378201, null);
		}
	}

	// Token: 0x04003916 RID: 14614
	[MyCmpGet]
	protected Operational operational;

	// Token: 0x04003917 RID: 14615
	[MyCmpReq]
	protected Storage storage;

	// Token: 0x04003918 RID: 14616
	[MyCmpGet]
	public Rotatable rotatable;

	// Token: 0x04003919 RID: 14617
	protected FetchChore fetchChore;

	// Token: 0x0400391A RID: 14618
	public ChoreType choreType = Db.Get().ChoreTypes.Fetch;

	// Token: 0x0400391B RID: 14619
	[Serialize]
	public bool autoReplaceEntity;

	// Token: 0x0400391C RID: 14620
	[Serialize]
	public Tag requestedEntityTag;

	// Token: 0x0400391D RID: 14621
	[Serialize]
	public Tag requestedEntityAdditionalFilterTag;

	// Token: 0x0400391E RID: 14622
	[Serialize]
	protected Ref<KSelectable> occupyObjectRef = new Ref<KSelectable>();

	// Token: 0x0400391F RID: 14623
	[SerializeField]
	private List<Tag> possibleDepositTagsList = new List<Tag>();

	// Token: 0x04003920 RID: 14624
	[SerializeField]
	private List<Func<GameObject, bool>> additionalCriteria = new List<Func<GameObject, bool>>();

	// Token: 0x04003921 RID: 14625
	[SerializeField]
	protected bool destroyEntityOnDeposit;

	// Token: 0x04003922 RID: 14626
	[SerializeField]
	protected SingleEntityReceptacle.ReceptacleDirection direction;

	// Token: 0x04003923 RID: 14627
	public Vector3 occupyingObjectRelativePosition = new Vector3(0f, 1f, 3f);

	// Token: 0x04003924 RID: 14628
	protected StatusItem statusItemAwaitingDelivery;

	// Token: 0x04003925 RID: 14629
	protected StatusItem statusItemNeed;

	// Token: 0x04003926 RID: 14630
	protected StatusItem statusItemNoneAvailable;

	// Token: 0x04003927 RID: 14631
	private static readonly EventSystem.IntraObjectHandler<SingleEntityReceptacle> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<SingleEntityReceptacle>(delegate(SingleEntityReceptacle component, object data)
	{
		component.OnOperationalChanged(data);
	});

	// Token: 0x02000FBD RID: 4029
	public enum ReceptacleDirection
	{
		// Token: 0x04003929 RID: 14633
		Top,
		// Token: 0x0400392A RID: 14634
		Side,
		// Token: 0x0400392B RID: 14635
		Bottom
	}
}
