﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FetchAreaChore : Chore<FetchAreaChore.StatesInstance>
{
	public bool IsFetching
	{
		get
		{
			return base.smi.pickingup;
		}
	}

	public bool IsDelivering
	{
		get
		{
			return base.smi.delivering;
		}
	}

	public GameObject GetFetchTarget
	{
		get
		{
			return base.smi.sm.fetchTarget.Get(base.smi);
		}
	}

	public FetchAreaChore(Chore.Precondition.Context context) : base(context.chore.choreType, context.consumerState.consumer, context.consumerState.choreProvider, false, null, null, null, context.masterPriority.priority_class, context.masterPriority.priority_value, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		this.showAvailabilityInHoverText = false;
		base.smi = new FetchAreaChore.StatesInstance(this, context);
	}

	public override void Cleanup()
	{
		base.Cleanup();
	}

	public override void Begin(Chore.Precondition.Context context)
	{
		base.smi.Begin(context);
		base.Begin(context);
	}

	protected override void End(string reason)
	{
		base.smi.End();
		base.End(reason);
	}

	private void OnTagsChanged(object data)
	{
		if (base.smi.sm.fetchTarget.Get(base.smi) != null)
		{
			this.Fail("Tags changed");
		}
	}

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

	public class StatesInstance : GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.GameInstance
	{
		public Tag RootChore_RequiredTag
		{
			get
			{
				return this.rootChore.requiredTag;
			}
		}

		public bool RootChore_ValidateRequiredTagOnTagChange
		{
			get
			{
				return this.rootChore.validateRequiredTagOnTagChange;
			}
		}

		public StatesInstance(FetchAreaChore master, Chore.Precondition.Context context) : base(master)
		{
			this.rootContext = context;
			this.rootChore = (context.chore as FetchChore);
		}

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
			float fetch_amount_available = root_fetchable.UnreservedFetchAmount;
			max_carry_weight = Mathf.Max(root_fetchable.PrimaryElement.MassPerUnit, max_carry_weight);
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
				if (pickupable2.UnreservedFetchAmount <= 0f)
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
				float unreservedFetchAmount = pickupable2.UnreservedFetchAmount;
				potential_fetchables.Add(pickupable2);
				fetch_amount_available += unreservedFetchAmount;
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
				num7 -= pickupable.UnreservedFetchAmount;
				this.fetchables.Add(pickupable);
				num8++;
			}
			this.fetchAmountRequested = num5;
			this.reservations.Clear();
			succeeded_contexts.Recycle();
			pooledList.Recycle();
			potential_fetchables.Recycle();
		}

		public void End()
		{
			foreach (FetchAreaChore.StatesInstance.Delivery delivery in this.deliveries)
			{
				delivery.Cleanup();
			}
			this.deliveries.Clear();
		}

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
				if (x == null || x.FetchTotalAmount <= 0f)
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

		public void SetFetchTarget(Pickupable fetching)
		{
			base.sm.fetchTarget.Set(fetching, base.smi);
			if (fetching != null)
			{
				fetching.Subscribe(1122777325, new Action<object>(this.OnMarkForMove));
			}
		}

		public void DeliverFail()
		{
			if (this.deliveries.Count > 0)
			{
				this.deliveries[0].Cleanup();
				this.deliveries.RemoveAt(0);
			}
			this.GoTo(base.sm.delivering.next);
		}

		public void DeliverComplete()
		{
			Pickupable pickupable = base.sm.deliveryObject.Get<Pickupable>(base.smi);
			if (!(pickupable == null) && pickupable.FetchTotalAmount > 0f)
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

		public void FetchComplete()
		{
			this.reservations[0].Cleanup();
			this.reservations.RemoveAt(0);
			this.GoTo(base.sm.fetching.next);
		}

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
				if (!pickupable.KPrefabID.HasTag(GameTags.MarkedForMove) && (pickupable.PrimaryElement.MassPerUnit <= 1f || num >= pickupable.PrimaryElement.MassPerUnit))
				{
					float num2 = Math.Min(num, pickupable.UnreservedFetchAmount);
					num -= num2;
					FetchAreaChore.StatesInstance.Reservation item = new FetchAreaChore.StatesInstance.Reservation(consumer, pickupable, num2);
					this.reservations.Add(item);
				}
			}
		}

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

		public void UnreservePickupables()
		{
			foreach (FetchAreaChore.StatesInstance.Reservation reservation in this.reservations)
			{
				reservation.Cleanup();
			}
			this.reservations.Clear();
		}

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

		private List<FetchChore> chores = new List<FetchChore>();

		private List<Pickupable> fetchables = new List<Pickupable>();

		private List<FetchAreaChore.StatesInstance.Reservation> reservations = new List<FetchAreaChore.StatesInstance.Reservation>();

		private List<Pickupable> deliverables = new List<Pickupable>();

		public List<FetchAreaChore.StatesInstance.Delivery> deliveries = new List<FetchAreaChore.StatesInstance.Delivery>();

		private FetchChore rootChore;

		private Chore.Precondition.Context rootContext;

		private float fetchAmountRequested;

		public bool delivering;

		public bool pickingup;

		private static Tag[] s_transientDeliveryTags = new Tag[]
		{
			GameTags.Garbage,
			GameTags.Creatures.Deliverable
		};

		public struct Delivery
		{
			public Storage destination { readonly get; private set; }

			public float amount { readonly get; private set; }

			public FetchChore chore { readonly get; private set; }

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
								if (pickupable2 != null && pickupable2.FetchTotalAmount > 0f)
								{
									num -= pickupable2.FetchTotalAmount;
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

			private void OnFetchChoreCleanup(Chore chore)
			{
				if (this.onCancelled != null)
				{
					this.onCancelled(chore as FetchChore);
				}
			}

			public void Cleanup()
			{
				if (this.chore != null)
				{
					FetchChore chore = this.chore;
					chore.onCleanup = (Action<Chore>)Delegate.Remove(chore.onCleanup, this.onFetchChoreCleanup);
					this.chore.FetchAreaEnd(null, null, false);
				}
			}

			private Action<FetchChore> onCancelled;

			private Action<Chore> onFetchChoreCleanup;
		}

		public struct Reservation
		{
			public float amount { readonly get; private set; }

			public Pickupable pickupable { readonly get; private set; }

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

			public void Cleanup()
			{
				if (this.pickupable != null)
				{
					this.pickupable.Unreserve("FetchAreaChore", this.handle);
				}
			}

			private int handle;
		}
	}

	public class States : GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore>
	{
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

		private NavTactic GetNavTactic(FetchAreaChore.StatesInstance smi)
		{
			WorkerBase component = this.fetcher.Get(smi).GetComponent<WorkerBase>();
			if (component != null && component.IsFetchDrone())
			{
				return NavigationTactics.FetchDronePickup;
			}
			return NavigationTactics.ReduceTravelDistance;
		}

		public FetchAreaChore.States.FetchStates fetching;

		public FetchAreaChore.States.DeliverStates delivering;

		public StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.TargetParameter fetcher;

		public StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.TargetParameter fetchTarget;

		public StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.TargetParameter fetchResultTarget;

		public StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.FloatParameter fetchAmount;

		public StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.TargetParameter deliveryDestination;

		public StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.TargetParameter deliveryObject;

		public StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.FloatParameter deliveryAmount;

		public StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.Signal currentdeliverycancelled;

		public class FetchStates : GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State
		{
			public GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State next;

			public GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.ApproachSubState<Pickupable> movetopickupable;

			public GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State pickup;

			public GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State fetchfail;

			public GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State fetchcomplete;
		}

		public class DeliverStates : GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State
		{
			public GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State next;

			public GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.ApproachSubState<Storage> movetostorage;

			public GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State storing;

			public GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State deliverfail;

			public GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State delivercomplete;
		}
	}
}
