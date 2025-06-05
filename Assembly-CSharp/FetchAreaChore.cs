using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x020006BF RID: 1727
public class FetchAreaChore : Chore<FetchAreaChore.StatesInstance>
{
	// Token: 0x170000C2 RID: 194
	// (get) Token: 0x06001E9E RID: 7838 RVA: 0x000B8ABD File Offset: 0x000B6CBD
	public bool IsFetching
	{
		get
		{
			return base.smi.pickingup;
		}
	}

	// Token: 0x170000C3 RID: 195
	// (get) Token: 0x06001E9F RID: 7839 RVA: 0x000B8ACA File Offset: 0x000B6CCA
	public bool IsDelivering
	{
		get
		{
			return base.smi.delivering;
		}
	}

	// Token: 0x170000C4 RID: 196
	// (get) Token: 0x06001EA0 RID: 7840 RVA: 0x000B8AD7 File Offset: 0x000B6CD7
	public GameObject GetFetchTarget
	{
		get
		{
			return base.smi.sm.fetchTarget.Get(base.smi);
		}
	}

	// Token: 0x06001EA1 RID: 7841 RVA: 0x001C0B14 File Offset: 0x001BED14
	public FetchAreaChore(Chore.Precondition.Context context) : base(context.chore.choreType, context.consumerState.consumer, context.consumerState.choreProvider, false, null, null, null, context.masterPriority.priority_class, context.masterPriority.priority_value, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		this.showAvailabilityInHoverText = false;
		base.smi = new FetchAreaChore.StatesInstance(this, context);
	}

	// Token: 0x06001EA2 RID: 7842 RVA: 0x000B8AF4 File Offset: 0x000B6CF4
	public override void Cleanup()
	{
		base.Cleanup();
	}

	// Token: 0x06001EA3 RID: 7843 RVA: 0x000B8AFC File Offset: 0x000B6CFC
	public override void Begin(Chore.Precondition.Context context)
	{
		base.smi.Begin(context);
		base.Begin(context);
	}

	// Token: 0x06001EA4 RID: 7844 RVA: 0x000B8B11 File Offset: 0x000B6D11
	protected override void End(string reason)
	{
		base.smi.End();
		base.End(reason);
	}

	// Token: 0x06001EA5 RID: 7845 RVA: 0x000B8B25 File Offset: 0x000B6D25
	private void OnTagsChanged(object data)
	{
		if (base.smi.sm.fetchTarget.Get(base.smi) != null)
		{
			this.Fail("Tags changed");
		}
	}

	// Token: 0x06001EA6 RID: 7846 RVA: 0x001C0B7C File Offset: 0x001BED7C
	private static bool IsPickupableStillValidForChore(Pickupable pickupable, FetchChore chore)
	{
		KPrefabID kprefabID = pickupable.KPrefabID;
		if ((chore.criteria == FetchChore.MatchCriteria.MatchID && !chore.tags.Contains(kprefabID.PrefabTag)) || (chore.criteria == FetchChore.MatchCriteria.MatchTags && !kprefabID.HasTag(chore.tagsFirst)))
		{
			global::Debug.Log(string.Format("Pickupable {0} is not valid for chore because it is not or does not contain one of these tags: {1}", pickupable, string.Join<Tag>(",", chore.tags)));
			return false;
		}
		if (chore.requiredTag.IsValid && !kprefabID.HasTag(chore.requiredTag))
		{
			global::Debug.Log(string.Format("Pickupable {0} is not valid for chore because it does not have the required tag: {1}", pickupable, chore.requiredTag));
			return false;
		}
		if (kprefabID.HasAnyTags(chore.forbiddenTags))
		{
			global::Debug.Log(string.Format("Pickupable {0} is not valid for chore because it has the forbidden tags: {1}", pickupable, string.Join<Tag>(",", chore.forbiddenTags)));
			return false;
		}
		return pickupable.isChoreAllowedToPickup(chore.choreType);
	}

	// Token: 0x06001EA7 RID: 7847 RVA: 0x001C0C58 File Offset: 0x001BEE58
	public static void GatherNearbyFetchChores(FetchChore root_chore, Chore.Precondition.Context context, int x, int y, int radius, List<Chore.Precondition.Context> succeeded_contexts, List<Chore.Precondition.Context> failed_contexts)
	{
		ListPool<ScenePartitionerEntry, FetchAreaChore>.PooledList pooledList = ListPool<ScenePartitionerEntry, FetchAreaChore>.Allocate();
		GameScenePartitioner.Instance.GatherEntries(x - radius, y - radius, radius * 2 + 1, radius * 2 + 1, GameScenePartitioner.Instance.fetchChoreLayer, pooledList);
		for (int i = 0; i < pooledList.Count; i++)
		{
			(pooledList[i].obj as FetchChore).CollectChoresFromGlobalChoreProvider(context.consumerState, succeeded_contexts, null, failed_contexts, true);
		}
		pooledList.Recycle();
	}

	// Token: 0x020006C0 RID: 1728
	public class StatesInstance : GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.GameInstance
	{
		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x06001EA8 RID: 7848 RVA: 0x000B8B55 File Offset: 0x000B6D55
		public Tag RootChore_RequiredTag
		{
			get
			{
				return this.rootChore.requiredTag;
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06001EA9 RID: 7849 RVA: 0x000B8B62 File Offset: 0x000B6D62
		public bool RootChore_ValidateRequiredTagOnTagChange
		{
			get
			{
				return this.rootChore.validateRequiredTagOnTagChange;
			}
		}

		// Token: 0x06001EAA RID: 7850 RVA: 0x001C0CD0 File Offset: 0x001BEED0
		public StatesInstance(FetchAreaChore master, Chore.Precondition.Context context) : base(master)
		{
			this.rootContext = context;
			this.rootChore = (context.chore as FetchChore);
		}

		// Token: 0x06001EAB RID: 7851 RVA: 0x001C0D34 File Offset: 0x001BEF34
		public void Begin(Chore.Precondition.Context context)
		{
			base.sm.fetcher.Set(context.consumerState.gameObject, base.smi, false);
			this.chores.Clear();
			this.chores.Add(this.rootChore);
			int x;
			int y;
			Grid.CellToXY(Grid.PosToCell(this.rootChore.destination.transform.GetPosition()), out x, out y);
			ListPool<Chore.Precondition.Context, FetchAreaChore>.PooledList succeeded_contexts = ListPool<Chore.Precondition.Context, FetchAreaChore>.Allocate();
			ListPool<Chore.Precondition.Context, FetchAreaChore>.PooledList pooledList = ListPool<Chore.Precondition.Context, FetchAreaChore>.Allocate();
			if (this.rootChore.allowMultifetch)
			{
				FetchAreaChore.GatherNearbyFetchChores(this.rootChore, context, x, y, 3, succeeded_contexts, pooledList);
			}
			float max_carry_weight = Mathf.Max(1f, Db.Get().Attributes.CarryAmount.Lookup(context.consumerState.consumer).GetTotalValue());
			Pickupable root_fetchable = context.data as Pickupable;
			if (root_fetchable == null)
			{
				global::Debug.Assert(succeeded_contexts.Count > 0, "succeeded_contexts was empty");
				FetchChore fetchChore = (FetchChore)succeeded_contexts[0].chore;
				global::Debug.Assert(fetchChore != null, "fetch_chore was null");
				DebugUtil.LogWarningArgs(new object[]
				{
					"Missing root_fetchable for FetchAreaChore",
					fetchChore.destination,
					fetchChore.tagsFirst
				});
				root_fetchable = fetchChore.FindFetchTarget(context.consumerState);
			}
			global::Debug.Assert(root_fetchable != null, "root_fetchable was null");
			ListPool<Pickupable, FetchAreaChore>.PooledList potential_fetchables = ListPool<Pickupable, FetchAreaChore>.Allocate();
			potential_fetchables.Add(root_fetchable);
			float fetch_amount_available = root_fetchable.UnreservedAmount;
			float minTakeAmount = root_fetchable.MinTakeAmount;
			int num = 0;
			int num2 = 0;
			Grid.CellToXY(Grid.PosToCell(root_fetchable.transform.GetPosition()), out num, out num2);
			int num3 = 9;
			num -= 3;
			num2 -= 3;
			Tag root_fetchable_tag = root_fetchable.GetComponent<KPrefabID>().PrefabTag;
			Func<object, object, bool> visitor = delegate(object obj, object _)
			{
				if (fetch_amount_available > max_carry_weight)
				{
					return false;
				}
				Pickupable pickupable2 = obj as Pickupable;
				KPrefabID kprefabID = pickupable2.KPrefabID;
				if (pickupable2 == root_fetchable)
				{
					return true;
				}
				if (kprefabID.HasTag(GameTags.StoredPrivate))
				{
					return true;
				}
				if (kprefabID.PrefabTag != root_fetchable_tag)
				{
					return true;
				}
				if (pickupable2.UnreservedAmount <= 0f)
				{
					return true;
				}
				if (this.rootChore.criteria == FetchChore.MatchCriteria.MatchID && !this.rootChore.tags.Contains(kprefabID.PrefabTag))
				{
					return true;
				}
				if (this.rootChore.criteria == FetchChore.MatchCriteria.MatchTags && !kprefabID.HasTag(this.rootChore.tagsFirst))
				{
					return true;
				}
				if (this.rootChore.requiredTag.IsValid && !kprefabID.HasTag(this.rootChore.requiredTag))
				{
					return true;
				}
				if (kprefabID.HasAnyTags(this.rootChore.forbiddenTags))
				{
					return true;
				}
				if (potential_fetchables.Contains(pickupable2))
				{
					return true;
				}
				if (!this.rootContext.consumerState.consumer.CanReach(pickupable2))
				{
					return true;
				}
				if (kprefabID.HasTag(GameTags.MarkedForMove))
				{
					return true;
				}
				if (!pickupable2.storage.IsNullOrDestroyed())
				{
					bool flag = true;
					foreach (Chore.Precondition.Context context3 in succeeded_contexts)
					{
						FetchChore fetchChore3 = context3.chore as FetchChore;
						if (!FetchManager.IsFetchablePickup(pickupable2, fetchChore3, fetchChore3.destination))
						{
							flag = false;
							break;
						}
					}
					if (!flag)
					{
						return true;
					}
				}
				float unreservedAmount = pickupable2.UnreservedAmount;
				potential_fetchables.Add(pickupable2);
				fetch_amount_available += unreservedAmount;
				return potential_fetchables.Count < 10;
			};
			GameScenePartitioner.Instance.AsyncSafeVisit<object>(num, num2, num3, num3, GameScenePartitioner.Instance.pickupablesLayer, visitor, null);
			GameScenePartitioner.Instance.AsyncSafeVisit<object>(num, num2, num3, num3, GameScenePartitioner.Instance.storedPickupablesLayer, visitor, null);
			fetch_amount_available = Mathf.Min(max_carry_weight, fetch_amount_available);
			if (minTakeAmount > 0f)
			{
				fetch_amount_available -= fetch_amount_available % minTakeAmount;
			}
			this.deliveries.Clear();
			float num4 = Mathf.Min(this.rootChore.originalAmount, fetch_amount_available);
			if (minTakeAmount > 0f)
			{
				num4 -= num4 % minTakeAmount;
			}
			this.deliveries.Add(new FetchAreaChore.StatesInstance.Delivery(this.rootContext, num4, new Action<FetchChore>(this.OnFetchChoreCancelled)));
			float num5 = num4;
			int num6 = 0;
			while (num6 < succeeded_contexts.Count && num5 < fetch_amount_available)
			{
				Chore.Precondition.Context context2 = succeeded_contexts[num6];
				FetchChore fetchChore2 = context2.chore as FetchChore;
				if (fetchChore2 != this.rootChore && fetchChore2.overrideTarget == null && fetchChore2.driver == null && fetchChore2.tagsHash == this.rootChore.tagsHash && fetchChore2.requiredTag == this.rootChore.requiredTag && fetchChore2.forbidHash == this.rootChore.forbidHash)
				{
					num4 = Mathf.Min(fetchChore2.originalAmount, fetch_amount_available - num5);
					if (minTakeAmount > 0f)
					{
						num4 -= num4 % minTakeAmount;
					}
					this.chores.Add(fetchChore2);
					this.deliveries.Add(new FetchAreaChore.StatesInstance.Delivery(context2, num4, new Action<FetchChore>(this.OnFetchChoreCancelled)));
					num5 += num4;
					if (this.deliveries.Count >= 10)
					{
						break;
					}
				}
				num6++;
			}
			num5 = Mathf.Min(num5, fetch_amount_available);
			float num7 = num5;
			this.fetchables.Clear();
			int num8 = 0;
			while (num8 < potential_fetchables.Count && num7 > 0f)
			{
				Pickupable pickupable = potential_fetchables[num8];
				num7 -= pickupable.UnreservedAmount;
				this.fetchables.Add(pickupable);
				num8++;
			}
			this.fetchAmountRequested = num5;
			this.reservations.Clear();
			succeeded_contexts.Recycle();
			pooledList.Recycle();
			potential_fetchables.Recycle();
		}

		// Token: 0x06001EAC RID: 7852 RVA: 0x001C11F8 File Offset: 0x001BF3F8
		public void End()
		{
			foreach (FetchAreaChore.StatesInstance.Delivery delivery in this.deliveries)
			{
				delivery.Cleanup();
			}
			this.deliveries.Clear();
		}

		// Token: 0x06001EAD RID: 7853 RVA: 0x001C1258 File Offset: 0x001BF458
		public void SetupDelivery()
		{
			if (this.deliveries.Count == 0)
			{
				this.StopSM("FetchAreaChoreComplete");
				return;
			}
			FetchAreaChore.StatesInstance.Delivery nextDelivery = this.deliveries[0];
			if (FetchAreaChore.StatesInstance.s_transientDeliveryTags.Contains(nextDelivery.chore.requiredTag))
			{
				nextDelivery.chore.requiredTag = Tag.Invalid;
			}
			this.deliverables.RemoveAll(delegate(Pickupable x)
			{
				if (x == null || x.TotalAmount <= 0f)
				{
					return true;
				}
				if (x.KPrefabID.HasTag(GameTags.MarkedForMove))
				{
					return true;
				}
				if (!FetchAreaChore.IsPickupableStillValidForChore(x, nextDelivery.chore))
				{
					global::Debug.LogWarning(string.Format("Removing deliverable {0} for a delivery to {1} which did not request it", x, nextDelivery.chore.destination));
					return true;
				}
				return false;
			});
			if (this.deliverables.Count == 0)
			{
				this.StopSM("FetchAreaChoreComplete");
				return;
			}
			base.sm.deliveryDestination.Set(nextDelivery.destination, base.smi);
			base.sm.deliveryObject.Set(this.deliverables[0], base.smi);
			if (!(nextDelivery.destination != null))
			{
				base.smi.GoTo(base.sm.delivering.deliverfail);
				return;
			}
			if (!this.rootContext.consumerState.hasSolidTransferArm)
			{
				this.GoTo(base.sm.delivering.movetostorage);
				return;
			}
			if (this.rootContext.consumerState.consumer.IsWithinReach(this.deliveries[0].destination))
			{
				this.GoTo(base.sm.delivering.storing);
				return;
			}
			this.GoTo(base.sm.delivering.deliverfail);
		}

		// Token: 0x06001EAE RID: 7854 RVA: 0x001C13F0 File Offset: 0x001BF5F0
		public void SetupFetch()
		{
			if (this.reservations.Count <= 0)
			{
				this.GoTo(base.sm.delivering.next);
				return;
			}
			this.SetFetchTarget(this.reservations[0].pickupable);
			base.sm.fetchResultTarget.Set(null, base.smi);
			base.sm.fetchAmount.Set(this.reservations[0].amount, base.smi, false);
			if (!(this.reservations[0].pickupable != null))
			{
				this.GoTo(base.sm.fetching.fetchfail);
				return;
			}
			if (!this.rootContext.consumerState.hasSolidTransferArm)
			{
				this.GoTo(base.sm.fetching.movetopickupable);
				return;
			}
			if (this.rootContext.consumerState.consumer.IsWithinReach(this.reservations[0].pickupable))
			{
				this.GoTo(base.sm.fetching.pickup);
				return;
			}
			this.GoTo(base.sm.fetching.fetchfail);
		}

		// Token: 0x06001EAF RID: 7855 RVA: 0x000B8B6F File Offset: 0x000B6D6F
		public void SetFetchTarget(Pickupable fetching)
		{
			base.sm.fetchTarget.Set(fetching, base.smi);
			if (fetching != null)
			{
				fetching.Subscribe(1122777325, new Action<object>(this.OnMarkForMove));
			}
		}

		// Token: 0x06001EB0 RID: 7856 RVA: 0x001C153C File Offset: 0x001BF73C
		public void DeliverFail()
		{
			if (this.deliveries.Count > 0)
			{
				this.deliveries[0].Cleanup();
				this.deliveries.RemoveAt(0);
			}
			this.GoTo(base.sm.delivering.next);
		}

		// Token: 0x06001EB1 RID: 7857 RVA: 0x001C1590 File Offset: 0x001BF790
		public void DeliverComplete()
		{
			Pickupable pickupable = base.sm.deliveryObject.Get<Pickupable>(base.smi);
			if (!(pickupable == null) && pickupable.TotalAmount > 0f)
			{
				if (this.deliveries.Count > 0)
				{
					FetchAreaChore.StatesInstance.Delivery delivery = this.deliveries[0];
					Chore chore = delivery.chore;
					delivery.Complete(this.deliverables);
					delivery.Cleanup();
					if (this.deliveries.Count > 0 && this.deliveries[0].chore == chore)
					{
						this.deliveries.RemoveAt(0);
					}
				}
				this.GoTo(base.sm.delivering.next);
				return;
			}
			if (this.deliveries.Count > 0 && this.deliveries[0].chore.amount < PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT)
			{
				FetchAreaChore.StatesInstance.Delivery delivery2 = this.deliveries[0];
				Chore chore2 = delivery2.chore;
				delivery2.Complete(this.deliverables);
				delivery2.Cleanup();
				if (this.deliveries.Count > 0 && this.deliveries[0].chore == chore2)
				{
					this.deliveries.RemoveAt(0);
				}
				this.GoTo(base.sm.delivering.next);
				return;
			}
			base.smi.GoTo(base.sm.delivering.deliverfail);
		}

		// Token: 0x06001EB2 RID: 7858 RVA: 0x001C170C File Offset: 0x001BF90C
		public void FetchFail()
		{
			if (base.smi.sm.fetchTarget.Get(base.smi) != null)
			{
				base.smi.sm.fetchTarget.Get(base.smi).Unsubscribe(1122777325, new Action<object>(this.OnMarkForMove));
			}
			this.reservations[0].Cleanup();
			this.reservations.RemoveAt(0);
			this.GoTo(base.sm.fetching.next);
		}

		// Token: 0x06001EB3 RID: 7859 RVA: 0x001C17A4 File Offset: 0x001BF9A4
		public void FetchComplete()
		{
			this.reservations[0].Cleanup();
			this.reservations.RemoveAt(0);
			this.GoTo(base.sm.fetching.next);
		}

		// Token: 0x06001EB4 RID: 7860 RVA: 0x001C17E8 File Offset: 0x001BF9E8
		public void SetupDeliverables()
		{
			foreach (GameObject gameObject in base.sm.fetcher.Get<Storage>(base.smi).items)
			{
				if (!(gameObject == null))
				{
					KPrefabID component = gameObject.GetComponent<KPrefabID>();
					if (!(component == null) && !component.HasTag(GameTags.MarkedForMove))
					{
						Pickupable component2 = component.GetComponent<Pickupable>();
						if (component2 != null)
						{
							this.deliverables.Add(component2);
						}
					}
				}
			}
		}

		// Token: 0x06001EB5 RID: 7861 RVA: 0x001C188C File Offset: 0x001BFA8C
		public void ReservePickupables()
		{
			ChoreConsumer consumer = base.sm.fetcher.Get<ChoreConsumer>(base.smi);
			float num = this.fetchAmountRequested;
			foreach (Pickupable pickupable in this.fetchables)
			{
				if (num <= 0f)
				{
					break;
				}
				if (!pickupable.KPrefabID.HasTag(GameTags.MarkedForMove))
				{
					float num2 = Math.Min(num, pickupable.UnreservedAmount);
					num -= num2;
					FetchAreaChore.StatesInstance.Reservation item = new FetchAreaChore.StatesInstance.Reservation(consumer, pickupable, num2);
					this.reservations.Add(item);
				}
			}
		}

		// Token: 0x06001EB6 RID: 7862 RVA: 0x001C1940 File Offset: 0x001BFB40
		private void OnFetchChoreCancelled(FetchChore chore)
		{
			int i = 0;
			while (i < this.deliveries.Count)
			{
				if (this.deliveries[i].chore == chore)
				{
					if (this.deliveries.Count == 1)
					{
						this.StopSM("AllDelivericesCancelled");
						return;
					}
					if (i == 0)
					{
						base.sm.currentdeliverycancelled.Trigger(this);
						return;
					}
					this.deliveries[i].Cleanup();
					this.deliveries.RemoveAt(i);
					return;
				}
				else
				{
					i++;
				}
			}
		}

		// Token: 0x06001EB7 RID: 7863 RVA: 0x001C19CC File Offset: 0x001BFBCC
		public void UnreservePickupables()
		{
			foreach (FetchAreaChore.StatesInstance.Reservation reservation in this.reservations)
			{
				reservation.Cleanup();
			}
			this.reservations.Clear();
		}

		// Token: 0x06001EB8 RID: 7864 RVA: 0x001C1A2C File Offset: 0x001BFC2C
		public bool SameDestination(FetchChore fetch)
		{
			using (List<FetchChore>.Enumerator enumerator = this.chores.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.destination == fetch.destination)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06001EB9 RID: 7865 RVA: 0x001C1A90 File Offset: 0x001BFC90
		public void OnMarkForMove(object data)
		{
			GameObject x = base.smi.sm.fetchTarget.Get(base.smi);
			GameObject gameObject = data as GameObject;
			if (x != null)
			{
				if (x == gameObject)
				{
					gameObject.Unsubscribe(1122777325, new Action<object>(this.OnMarkForMove));
					base.smi.sm.fetchTarget.Set(null, base.smi);
					return;
				}
				global::Debug.LogError("Listening for MarkForMove on the incorrect fetch target. Subscriptions did not update correctly.");
			}
		}

		// Token: 0x040013FC RID: 5116
		private List<FetchChore> chores = new List<FetchChore>();

		// Token: 0x040013FD RID: 5117
		private List<Pickupable> fetchables = new List<Pickupable>();

		// Token: 0x040013FE RID: 5118
		private List<FetchAreaChore.StatesInstance.Reservation> reservations = new List<FetchAreaChore.StatesInstance.Reservation>();

		// Token: 0x040013FF RID: 5119
		private List<Pickupable> deliverables = new List<Pickupable>();

		// Token: 0x04001400 RID: 5120
		public List<FetchAreaChore.StatesInstance.Delivery> deliveries = new List<FetchAreaChore.StatesInstance.Delivery>();

		// Token: 0x04001401 RID: 5121
		private FetchChore rootChore;

		// Token: 0x04001402 RID: 5122
		private Chore.Precondition.Context rootContext;

		// Token: 0x04001403 RID: 5123
		private float fetchAmountRequested;

		// Token: 0x04001404 RID: 5124
		public bool delivering;

		// Token: 0x04001405 RID: 5125
		public bool pickingup;

		// Token: 0x04001406 RID: 5126
		private static Tag[] s_transientDeliveryTags = new Tag[]
		{
			GameTags.Garbage,
			GameTags.Creatures.Deliverable
		};

		// Token: 0x020006C1 RID: 1729
		public struct Delivery
		{
			// Token: 0x170000C7 RID: 199
			// (get) Token: 0x06001EBB RID: 7867 RVA: 0x000B8BCE File Offset: 0x000B6DCE
			// (set) Token: 0x06001EBC RID: 7868 RVA: 0x000B8BD6 File Offset: 0x000B6DD6
			public Storage destination { readonly get; private set; }

			// Token: 0x170000C8 RID: 200
			// (get) Token: 0x06001EBD RID: 7869 RVA: 0x000B8BDF File Offset: 0x000B6DDF
			// (set) Token: 0x06001EBE RID: 7870 RVA: 0x000B8BE7 File Offset: 0x000B6DE7
			public float amount { readonly get; private set; }

			// Token: 0x170000C9 RID: 201
			// (get) Token: 0x06001EBF RID: 7871 RVA: 0x000B8BF0 File Offset: 0x000B6DF0
			// (set) Token: 0x06001EC0 RID: 7872 RVA: 0x000B8BF8 File Offset: 0x000B6DF8
			public FetchChore chore { readonly get; private set; }

			// Token: 0x06001EC1 RID: 7873 RVA: 0x001C1B10 File Offset: 0x001BFD10
			public Delivery(Chore.Precondition.Context context, float amount_to_be_fetched, Action<FetchChore> on_cancelled)
			{
				this = default(FetchAreaChore.StatesInstance.Delivery);
				this.chore = (context.chore as FetchChore);
				this.amount = this.chore.originalAmount;
				this.destination = this.chore.destination;
				this.chore.SetOverrideTarget(context.consumerState.consumer);
				this.onCancelled = on_cancelled;
				this.onFetchChoreCleanup = new Action<Chore>(this.OnFetchChoreCleanup);
				this.chore.FetchAreaBegin(context, amount_to_be_fetched);
				FetchChore chore = this.chore;
				chore.onCleanup = (Action<Chore>)Delegate.Combine(chore.onCleanup, this.onFetchChoreCleanup);
			}

			// Token: 0x06001EC2 RID: 7874 RVA: 0x001C1BC0 File Offset: 0x001BFDC0
			public void Complete(List<Pickupable> deliverables)
			{
				using (new KProfiler.Region("FAC.Delivery.Complete", null))
				{
					if (!(this.destination == null) && !this.destination.IsEndOfLife())
					{
						FetchChore chore = this.chore;
						chore.onCleanup = (Action<Chore>)Delegate.Remove(chore.onCleanup, this.onFetchChoreCleanup);
						float num = this.amount;
						Pickupable pickupable = null;
						int num2 = 0;
						while (num2 < deliverables.Count && num > 0f)
						{
							if (deliverables[num2] == null)
							{
								if (num < PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT)
								{
									this.destination.ForceStore(this.chore.tagsFirst, num);
								}
							}
							else if (!FetchAreaChore.IsPickupableStillValidForChore(deliverables[num2], this.chore))
							{
								global::Debug.LogError(string.Format("Attempting to store {0} in a {1} which did not request it", deliverables[num2], this.destination));
							}
							else
							{
								Pickupable pickupable2 = deliverables[num2].Take(num);
								if (pickupable2 != null && pickupable2.TotalAmount > 0f)
								{
									num -= pickupable2.TotalAmount;
									this.destination.Store(pickupable2.gameObject, false, false, true, false);
									pickupable = pickupable2;
									if (pickupable2 == deliverables[num2])
									{
										deliverables[num2] = null;
									}
								}
							}
							num2++;
						}
						if (this.chore.overrideTarget != null)
						{
							this.chore.FetchAreaEnd(this.chore.overrideTarget.GetComponent<ChoreDriver>(), pickupable, true);
						}
						this.chore = null;
					}
				}
			}

			// Token: 0x06001EC3 RID: 7875 RVA: 0x000B8C01 File Offset: 0x000B6E01
			private void OnFetchChoreCleanup(Chore chore)
			{
				if (this.onCancelled != null)
				{
					this.onCancelled(chore as FetchChore);
				}
			}

			// Token: 0x06001EC4 RID: 7876 RVA: 0x000B8C1C File Offset: 0x000B6E1C
			public void Cleanup()
			{
				if (this.chore != null)
				{
					FetchChore chore = this.chore;
					chore.onCleanup = (Action<Chore>)Delegate.Remove(chore.onCleanup, this.onFetchChoreCleanup);
					this.chore.FetchAreaEnd(null, null, false);
				}
			}

			// Token: 0x0400140A RID: 5130
			private Action<FetchChore> onCancelled;

			// Token: 0x0400140B RID: 5131
			private Action<Chore> onFetchChoreCleanup;
		}

		// Token: 0x020006C2 RID: 1730
		public struct Reservation
		{
			// Token: 0x170000CA RID: 202
			// (get) Token: 0x06001EC5 RID: 7877 RVA: 0x000B8C55 File Offset: 0x000B6E55
			// (set) Token: 0x06001EC6 RID: 7878 RVA: 0x000B8C5D File Offset: 0x000B6E5D
			public float amount { readonly get; private set; }

			// Token: 0x170000CB RID: 203
			// (get) Token: 0x06001EC7 RID: 7879 RVA: 0x000B8C66 File Offset: 0x000B6E66
			// (set) Token: 0x06001EC8 RID: 7880 RVA: 0x000B8C6E File Offset: 0x000B6E6E
			public Pickupable pickupable { readonly get; private set; }

			// Token: 0x06001EC9 RID: 7881 RVA: 0x001C1D78 File Offset: 0x001BFF78
			public Reservation(ChoreConsumer consumer, Pickupable pickupable, float reservation_amount)
			{
				this = default(FetchAreaChore.StatesInstance.Reservation);
				if (reservation_amount <= 0f)
				{
					global::Debug.LogError("Invalid amount: " + reservation_amount.ToString());
				}
				this.amount = reservation_amount;
				this.pickupable = pickupable;
				this.handle = pickupable.Reserve("FetchAreaChore", consumer.GetComponent<KPrefabID>().InstanceID, reservation_amount);
			}

			// Token: 0x06001ECA RID: 7882 RVA: 0x000B8C77 File Offset: 0x000B6E77
			public void Cleanup()
			{
				if (this.pickupable != null)
				{
					this.pickupable.Unreserve("FetchAreaChore", this.handle);
				}
			}

			// Token: 0x0400140E RID: 5134
			private int handle;
		}
	}

	// Token: 0x020006C5 RID: 1733
	public class States : GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore>
	{
		// Token: 0x06001ECF RID: 7887 RVA: 0x001C2044 File Offset: 0x001C0244
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.fetching;
			base.Target(this.fetcher);
			this.fetching.DefaultState(this.fetching.next).Enter("ReservePickupables", delegate(FetchAreaChore.StatesInstance smi)
			{
				smi.ReservePickupables();
			}).Exit("UnreservePickupables", delegate(FetchAreaChore.StatesInstance smi)
			{
				smi.UnreservePickupables();
			}).Enter("pickingup-on", delegate(FetchAreaChore.StatesInstance smi)
			{
				smi.pickingup = true;
			}).Exit("pickingup-off", delegate(FetchAreaChore.StatesInstance smi)
			{
				smi.pickingup = false;
			});
			this.fetching.next.Enter("SetupFetch", delegate(FetchAreaChore.StatesInstance smi)
			{
				smi.SetupFetch();
			});
			this.fetching.movetopickupable.InitializeStates(new Func<FetchAreaChore.StatesInstance, NavTactic>(this.GetNavTactic), this.fetcher, this.fetchTarget, this.fetching.pickup, this.fetching.fetchfail, null).Target(this.fetchTarget).EventHandlerTransition(GameHashes.TagsChanged, this.fetching.fetchfail, (FetchAreaChore.StatesInstance smi, object obj) => smi.RootChore_ValidateRequiredTagOnTagChange && smi.RootChore_RequiredTag.IsValid && !this.fetchTarget.Get(smi).HasTag(smi.RootChore_RequiredTag)).Target(this.fetcher);
			this.fetching.pickup.DoPickup(this.fetchTarget, this.fetchResultTarget, this.fetchAmount, this.fetching.fetchcomplete, this.fetching.fetchfail).Exit(delegate(FetchAreaChore.StatesInstance smi)
			{
				GameObject gameObject = smi.sm.fetchTarget.Get(smi);
				if (gameObject != null)
				{
					gameObject.Unsubscribe(1122777325, new Action<object>(smi.OnMarkForMove));
				}
			});
			this.fetching.fetchcomplete.Enter(delegate(FetchAreaChore.StatesInstance smi)
			{
				smi.FetchComplete();
			});
			this.fetching.fetchfail.Enter(delegate(FetchAreaChore.StatesInstance smi)
			{
				smi.FetchFail();
			});
			this.delivering.DefaultState(this.delivering.next).OnSignal(this.currentdeliverycancelled, this.delivering.deliverfail).Enter("SetupDeliverables", delegate(FetchAreaChore.StatesInstance smi)
			{
				smi.SetupDeliverables();
			}).Enter("delivering-on", delegate(FetchAreaChore.StatesInstance smi)
			{
				smi.delivering = true;
			}).Exit("delivering-off", delegate(FetchAreaChore.StatesInstance smi)
			{
				smi.delivering = false;
			});
			this.delivering.next.Enter("SetupDelivery", delegate(FetchAreaChore.StatesInstance smi)
			{
				smi.SetupDelivery();
			});
			this.delivering.movetostorage.InitializeStates(new Func<FetchAreaChore.StatesInstance, NavTactic>(this.GetNavTactic), this.fetcher, this.deliveryDestination, this.delivering.storing, this.delivering.deliverfail, null).Enter(delegate(FetchAreaChore.StatesInstance smi)
			{
				if (this.deliveryObject.Get(smi) != null && this.deliveryObject.Get(smi).GetComponent<MinionIdentity>() != null)
				{
					this.deliveryObject.Get(smi).transform.SetLocalPosition(Vector3.zero);
					KBatchedAnimTracker component = this.deliveryObject.Get(smi).GetComponent<KBatchedAnimTracker>();
					component.symbol = new HashedString("snapTo_chest");
					component.offset = new Vector3(0f, 0f, 1f);
				}
			});
			this.delivering.storing.DoDelivery(this.fetcher, this.deliveryDestination, this.delivering.delivercomplete, this.delivering.deliverfail);
			this.delivering.deliverfail.Enter(delegate(FetchAreaChore.StatesInstance smi)
			{
				smi.DeliverFail();
			});
			this.delivering.delivercomplete.Enter(delegate(FetchAreaChore.StatesInstance smi)
			{
				smi.DeliverComplete();
			});
		}

		// Token: 0x06001ED0 RID: 7888 RVA: 0x001C244C File Offset: 0x001C064C
		private NavTactic GetNavTactic(FetchAreaChore.StatesInstance smi)
		{
			WorkerBase component = this.fetcher.Get(smi).GetComponent<WorkerBase>();
			if (component != null && component.IsFetchDrone())
			{
				return NavigationTactics.FetchDronePickup;
			}
			return NavigationTactics.ReduceTravelDistance;
		}

		// Token: 0x04001417 RID: 5143
		public FetchAreaChore.States.FetchStates fetching;

		// Token: 0x04001418 RID: 5144
		public FetchAreaChore.States.DeliverStates delivering;

		// Token: 0x04001419 RID: 5145
		public StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.TargetParameter fetcher;

		// Token: 0x0400141A RID: 5146
		public StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.TargetParameter fetchTarget;

		// Token: 0x0400141B RID: 5147
		public StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.TargetParameter fetchResultTarget;

		// Token: 0x0400141C RID: 5148
		public StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.FloatParameter fetchAmount;

		// Token: 0x0400141D RID: 5149
		public StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.TargetParameter deliveryDestination;

		// Token: 0x0400141E RID: 5150
		public StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.TargetParameter deliveryObject;

		// Token: 0x0400141F RID: 5151
		public StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.FloatParameter deliveryAmount;

		// Token: 0x04001420 RID: 5152
		public StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.Signal currentdeliverycancelled;

		// Token: 0x020006C6 RID: 1734
		public class FetchStates : GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State
		{
			// Token: 0x04001421 RID: 5153
			public GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State next;

			// Token: 0x04001422 RID: 5154
			public GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.ApproachSubState<Pickupable> movetopickupable;

			// Token: 0x04001423 RID: 5155
			public GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State pickup;

			// Token: 0x04001424 RID: 5156
			public GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State fetchfail;

			// Token: 0x04001425 RID: 5157
			public GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State fetchcomplete;
		}

		// Token: 0x020006C7 RID: 1735
		public class DeliverStates : GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State
		{
			// Token: 0x04001426 RID: 5158
			public GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State next;

			// Token: 0x04001427 RID: 5159
			public GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.ApproachSubState<Storage> movetostorage;

			// Token: 0x04001428 RID: 5160
			public GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State storing;

			// Token: 0x04001429 RID: 5161
			public GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State deliverfail;

			// Token: 0x0400142A RID: 5162
			public GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State delivercomplete;
		}
	}
}
