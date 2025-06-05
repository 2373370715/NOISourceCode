using System;
using System.Collections.Generic;
using System.Diagnostics;
using FoodRehydrator;
using UnityEngine;

// Token: 0x0200132B RID: 4907
[AddComponentMenu("KMonoBehaviour/scripts/FetchManager")]
public class FetchManager : KMonoBehaviour, ISim1000ms
{
	// Token: 0x06006478 RID: 25720 RVA: 0x000E605C File Offset: 0x000E425C
	private static int QuantizeRotValue(float rot_value)
	{
		return (int)(4f * rot_value);
	}

	// Token: 0x06006479 RID: 25721 RVA: 0x000AA038 File Offset: 0x000A8238
	[Conditional("ENABLE_FETCH_PROFILING")]
	private static void BeginDetailedSample(string region_name)
	{
	}

	// Token: 0x0600647A RID: 25722 RVA: 0x000AA038 File Offset: 0x000A8238
	[Conditional("ENABLE_FETCH_PROFILING")]
	private static void BeginDetailedSample(string region_name, int count)
	{
	}

	// Token: 0x0600647B RID: 25723 RVA: 0x000AA038 File Offset: 0x000A8238
	[Conditional("ENABLE_FETCH_PROFILING")]
	private static void EndDetailedSample(string region_name)
	{
	}

	// Token: 0x0600647C RID: 25724 RVA: 0x000AA038 File Offset: 0x000A8238
	[Conditional("ENABLE_FETCH_PROFILING")]
	private static void EndDetailedSample(string region_name, int count)
	{
	}

	// Token: 0x0600647D RID: 25725 RVA: 0x002CD3A8 File Offset: 0x002CB5A8
	public HandleVector<int>.Handle Add(Pickupable pickupable)
	{
		Tag tag = pickupable.KPrefabID.PrefabID();
		FetchManager.FetchablesByPrefabId fetchablesByPrefabId = null;
		if (!this.prefabIdToFetchables.TryGetValue(tag, out fetchablesByPrefabId))
		{
			fetchablesByPrefabId = new FetchManager.FetchablesByPrefabId(tag);
			this.prefabIdToFetchables[tag] = fetchablesByPrefabId;
		}
		return fetchablesByPrefabId.AddPickupable(pickupable);
	}

	// Token: 0x0600647E RID: 25726 RVA: 0x002CD3F0 File Offset: 0x002CB5F0
	public void Remove(Tag prefab_tag, HandleVector<int>.Handle fetchable_handle)
	{
		FetchManager.FetchablesByPrefabId fetchablesByPrefabId;
		if (this.prefabIdToFetchables.TryGetValue(prefab_tag, out fetchablesByPrefabId))
		{
			fetchablesByPrefabId.RemovePickupable(fetchable_handle);
		}
	}

	// Token: 0x0600647F RID: 25727 RVA: 0x002CD414 File Offset: 0x002CB614
	public void UpdateStorage(Tag prefab_tag, HandleVector<int>.Handle fetchable_handle, Storage storage)
	{
		FetchManager.FetchablesByPrefabId fetchablesByPrefabId;
		if (this.prefabIdToFetchables.TryGetValue(prefab_tag, out fetchablesByPrefabId))
		{
			fetchablesByPrefabId.UpdateStorage(fetchable_handle, storage);
		}
	}

	// Token: 0x06006480 RID: 25728 RVA: 0x000E6066 File Offset: 0x000E4266
	public void UpdateTags(Tag prefab_tag, HandleVector<int>.Handle fetchable_handle)
	{
		this.prefabIdToFetchables[prefab_tag].UpdateTags(fetchable_handle);
	}

	// Token: 0x06006481 RID: 25729 RVA: 0x002CD43C File Offset: 0x002CB63C
	public void Sim1000ms(float dt)
	{
		foreach (KeyValuePair<Tag, FetchManager.FetchablesByPrefabId> keyValuePair in this.prefabIdToFetchables)
		{
			keyValuePair.Value.Sim1000ms(dt);
		}
	}

	// Token: 0x06006482 RID: 25730 RVA: 0x002CD498 File Offset: 0x002CB698
	public void UpdatePickups(PathProber path_prober, WorkerBase worker)
	{
		Navigator component = worker.GetComponent<Navigator>();
		this.updateOffsetTables.Reset(null);
		this.updatePickupsWorkItems.Reset(null);
		foreach (KeyValuePair<Tag, FetchManager.FetchablesByPrefabId> keyValuePair in this.prefabIdToFetchables)
		{
			FetchManager.FetchablesByPrefabId value = keyValuePair.Value;
			this.updateOffsetTables.Add(new FetchManager.UpdateOffsetTables(value));
			this.updatePickupsWorkItems.Add(new FetchManager.UpdatePickupWorkItem
			{
				fetchablesByPrefabId = value,
				pathProber = path_prober,
				navigator = component,
				worker = worker.GetComponent<KPrefabID>().InstanceID
			});
		}
		GlobalJobManager.Run(this.updateOffsetTables);
		for (int i = 0; i < this.updateOffsetTables.Count; i++)
		{
			this.updateOffsetTables.GetWorkItem(i).Finish();
		}
		OffsetTracker.isExecutingWithinJob = true;
		GlobalJobManager.Run(this.updatePickupsWorkItems);
		OffsetTracker.isExecutingWithinJob = false;
		this.pickups.Clear();
		foreach (KeyValuePair<Tag, FetchManager.FetchablesByPrefabId> keyValuePair2 in this.prefabIdToFetchables)
		{
			this.pickups.AddRange(keyValuePair2.Value.finalPickups);
		}
		this.pickups.Sort(FetchManager.PickupComparerNoPriority.CompareInst);
	}

	// Token: 0x06006483 RID: 25731 RVA: 0x002CD61C File Offset: 0x002CB81C
	public static bool IsFetchablePickup(Pickupable pickup, FetchChore chore, Storage destination)
	{
		KPrefabID kprefabID = pickup.KPrefabID;
		Storage storage = pickup.storage;
		if (pickup.UnreservedAmount <= 0f)
		{
			return false;
		}
		if (kprefabID == null)
		{
			return false;
		}
		if (!pickup.isChoreAllowedToPickup(chore.choreType))
		{
			return false;
		}
		if (chore.criteria == FetchChore.MatchCriteria.MatchID && !chore.tags.Contains(kprefabID.PrefabTag))
		{
			return false;
		}
		if (chore.criteria == FetchChore.MatchCriteria.MatchTags && !kprefabID.HasTag(chore.tagsFirst))
		{
			return false;
		}
		if (chore.requiredTag.IsValid && !kprefabID.HasTag(chore.requiredTag))
		{
			return false;
		}
		if (kprefabID.HasAnyTags(chore.forbiddenTags))
		{
			return false;
		}
		if (kprefabID.HasTag(GameTags.MarkedForMove))
		{
			return false;
		}
		if (storage != null)
		{
			if (!storage.ignoreSourcePriority && destination.ShouldOnlyTransferFromLowerPriority && destination.masterPriority <= storage.masterPriority)
			{
				return false;
			}
			if (destination.storageNetworkID != -1 && destination.storageNetworkID == storage.storageNetworkID)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06006484 RID: 25732 RVA: 0x002CD71C File Offset: 0x002CB91C
	public static Pickupable FindFetchTarget(List<Pickupable> pickupables, Storage destination, FetchChore chore)
	{
		foreach (Pickupable pickupable in pickupables)
		{
			if (FetchManager.IsFetchablePickup(pickupable, chore, destination))
			{
				return pickupable;
			}
		}
		return null;
	}

	// Token: 0x06006485 RID: 25733 RVA: 0x002CD774 File Offset: 0x002CB974
	public Pickupable FindFetchTarget(Storage destination, FetchChore chore)
	{
		foreach (FetchManager.Pickup pickup in this.pickups)
		{
			if (FetchManager.IsFetchablePickup(pickup.pickupable, chore, destination))
			{
				return pickup.pickupable;
			}
		}
		return null;
	}

	// Token: 0x06006486 RID: 25734 RVA: 0x000E607A File Offset: 0x000E427A
	public static bool IsFetchablePickup_Exclude(KPrefabID pickup_id, Storage source, float pickup_unreserved_amount, HashSet<Tag> exclude_tags, Tag required_tag, Storage destination)
	{
		return FetchManager.IsFetchablePickup_Exclude(pickup_id, source, pickup_unreserved_amount, exclude_tags, new Tag[]
		{
			required_tag
		}, destination);
	}

	// Token: 0x06006487 RID: 25735 RVA: 0x002CD7DC File Offset: 0x002CB9DC
	public static bool IsFetchablePickup_Exclude(KPrefabID pickup_id, Storage source, float pickup_unreserved_amount, HashSet<Tag> exclude_tags, Tag[] required_tags, Storage destination)
	{
		if (pickup_unreserved_amount <= 0f)
		{
			return false;
		}
		if (pickup_id == null)
		{
			return false;
		}
		if (exclude_tags.Contains(pickup_id.PrefabTag))
		{
			return false;
		}
		if (!pickup_id.HasAllTags(required_tags))
		{
			return false;
		}
		if (source != null)
		{
			if (!source.ignoreSourcePriority && destination.ShouldOnlyTransferFromLowerPriority && destination.masterPriority <= source.masterPriority)
			{
				return false;
			}
			if (destination.storageNetworkID != -1 && destination.storageNetworkID == source.storageNetworkID)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06006488 RID: 25736 RVA: 0x000E6096 File Offset: 0x000E4296
	public Pickupable FindEdibleFetchTarget(Storage destination, HashSet<Tag> exclude_tags, Tag required_tag)
	{
		return this.FindEdibleFetchTarget(destination, exclude_tags, new Tag[]
		{
			required_tag
		});
	}

	// Token: 0x06006489 RID: 25737 RVA: 0x002CD868 File Offset: 0x002CBA68
	public Pickupable FindEdibleFetchTarget(Storage destination, HashSet<Tag> exclude_tags, Tag[] required_tags)
	{
		FetchManager.Pickup pickup = new FetchManager.Pickup
		{
			PathCost = ushort.MaxValue,
			foodQuality = int.MinValue
		};
		int num = int.MaxValue;
		foreach (FetchManager.Pickup pickup2 in this.pickups)
		{
			Pickupable pickupable = pickup2.pickupable;
			if (FetchManager.IsFetchablePickup_Exclude(pickupable.KPrefabID, pickupable.storage, pickupable.UnreservedAmount, exclude_tags, required_tags, destination))
			{
				int num2 = (int)pickup2.PathCost + (5 - pickup2.foodQuality) * 50;
				if (num2 < num)
				{
					pickup = pickup2;
					num = num2;
				}
			}
		}
		Navigator component = destination.GetComponent<Navigator>();
		if (component != null)
		{
			foreach (object obj in Components.FoodRehydrators)
			{
				GameObject gameObject = (GameObject)obj;
				int cell = Grid.PosToCell(gameObject);
				int cost = component.PathProber.GetCost(cell);
				if (cost != -1 && num > cost + 50 + 5)
				{
					AccessabilityManager accessabilityManager = (gameObject != null) ? gameObject.GetComponent<AccessabilityManager>() : null;
					if (accessabilityManager != null && accessabilityManager.CanAccess(destination.gameObject))
					{
						foreach (GameObject gameObject2 in gameObject.GetComponent<Storage>().items)
						{
							Storage storage = (gameObject2 != null) ? gameObject2.GetComponent<Storage>() : null;
							if (storage != null && !storage.IsEmpty())
							{
								Edible component2 = storage.items[0].GetComponent<Edible>();
								Pickupable component3 = component2.GetComponent<Pickupable>();
								if (FetchManager.IsFetchablePickup_Exclude(component3.KPrefabID, component3.storage, component3.UnreservedAmount, exclude_tags, required_tags, destination))
								{
									int num3 = cost + (5 - component2.FoodInfo.Quality + 1) * 50 + 5;
									if (num3 < num)
									{
										pickup.pickupable = component3;
										pickup.foodQuality = component2.FoodInfo.Quality;
										pickup.tagBitsHash = component2.PrefabID().GetHashCode();
										num = num3;
									}
								}
							}
						}
					}
				}
			}
		}
		return pickup.pickupable;
	}

	// Token: 0x04004851 RID: 18513
	private List<FetchManager.Pickup> pickups = new List<FetchManager.Pickup>();

	// Token: 0x04004852 RID: 18514
	public Dictionary<Tag, FetchManager.FetchablesByPrefabId> prefabIdToFetchables = new Dictionary<Tag, FetchManager.FetchablesByPrefabId>();

	// Token: 0x04004853 RID: 18515
	private WorkItemCollection<FetchManager.UpdateOffsetTables, object> updateOffsetTables = new WorkItemCollection<FetchManager.UpdateOffsetTables, object>();

	// Token: 0x04004854 RID: 18516
	private WorkItemCollection<FetchManager.UpdatePickupWorkItem, object> updatePickupsWorkItems = new WorkItemCollection<FetchManager.UpdatePickupWorkItem, object>();

	// Token: 0x0200132C RID: 4908
	public struct Fetchable
	{
		// Token: 0x04004855 RID: 18517
		public Pickupable pickupable;

		// Token: 0x04004856 RID: 18518
		public int tagBitsHash;

		// Token: 0x04004857 RID: 18519
		public int masterPriority;

		// Token: 0x04004858 RID: 18520
		public int freshness;

		// Token: 0x04004859 RID: 18521
		public int foodQuality;
	}

	// Token: 0x0200132D RID: 4909
	[DebuggerDisplay("{pickupable.name}")]
	public struct Pickup
	{
		// Token: 0x0400485A RID: 18522
		public Pickupable pickupable;

		// Token: 0x0400485B RID: 18523
		public int tagBitsHash;

		// Token: 0x0400485C RID: 18524
		public ushort PathCost;

		// Token: 0x0400485D RID: 18525
		public int masterPriority;

		// Token: 0x0400485E RID: 18526
		public int freshness;

		// Token: 0x0400485F RID: 18527
		public int foodQuality;
	}

	// Token: 0x0200132E RID: 4910
	private static class PickupComparerIncludingPriority
	{
		// Token: 0x0600648B RID: 25739 RVA: 0x002CDB24 File Offset: 0x002CBD24
		private static int Compare(FetchManager.Pickup a, FetchManager.Pickup b)
		{
			int num = a.tagBitsHash.CompareTo(b.tagBitsHash);
			if (num != 0)
			{
				return num;
			}
			num = b.masterPriority.CompareTo(a.masterPriority);
			if (num != 0)
			{
				return num;
			}
			num = a.PathCost.CompareTo(b.PathCost);
			if (num != 0)
			{
				return num;
			}
			num = b.foodQuality.CompareTo(a.foodQuality);
			if (num != 0)
			{
				return num;
			}
			return b.freshness.CompareTo(a.freshness);
		}

		// Token: 0x04004860 RID: 18528
		public static Comparison<FetchManager.Pickup> CompareInst = new Comparison<FetchManager.Pickup>(FetchManager.PickupComparerIncludingPriority.Compare);
	}

	// Token: 0x0200132F RID: 4911
	private static class PickupComparerNoPriority
	{
		// Token: 0x0600648D RID: 25741 RVA: 0x002CDBA8 File Offset: 0x002CBDA8
		public static int Compare(FetchManager.Pickup a, FetchManager.Pickup b)
		{
			int num = a.PathCost.CompareTo(b.PathCost);
			if (num != 0)
			{
				return num;
			}
			num = b.foodQuality.CompareTo(a.foodQuality);
			if (num != 0)
			{
				return num;
			}
			return b.freshness.CompareTo(a.freshness);
		}

		// Token: 0x04004861 RID: 18529
		public static Comparison<FetchManager.Pickup> CompareInst = new Comparison<FetchManager.Pickup>(FetchManager.PickupComparerNoPriority.Compare);
	}

	// Token: 0x02001330 RID: 4912
	public class FetchablesByPrefabId
	{
		// Token: 0x17000644 RID: 1604
		// (get) Token: 0x0600648F RID: 25743 RVA: 0x000E6108 File Offset: 0x000E4308
		// (set) Token: 0x06006490 RID: 25744 RVA: 0x000E6110 File Offset: 0x000E4310
		public Tag prefabId { get; private set; }

		// Token: 0x06006491 RID: 25745 RVA: 0x002CDBFC File Offset: 0x002CBDFC
		public FetchablesByPrefabId(Tag prefab_id)
		{
			this.prefabId = prefab_id;
			this.fetchables = new KCompactedVector<FetchManager.Fetchable>(0);
			this.rotUpdaters = new Dictionary<HandleVector<int>.Handle, Rottable.Instance>();
			this.finalPickups = new List<FetchManager.Pickup>();
		}

		// Token: 0x06006492 RID: 25746 RVA: 0x002CDC5C File Offset: 0x002CBE5C
		public HandleVector<int>.Handle AddPickupable(Pickupable pickupable)
		{
			int foodQuality = 5;
			Edible component = pickupable.GetComponent<Edible>();
			if (component != null)
			{
				foodQuality = component.GetQuality();
			}
			int masterPriority = 0;
			if (pickupable.storage != null)
			{
				Prioritizable prioritizable = pickupable.storage.prioritizable;
				if (prioritizable != null)
				{
					masterPriority = prioritizable.GetMasterPriority().priority_value;
				}
			}
			Rottable.Instance smi = pickupable.GetSMI<Rottable.Instance>();
			int freshness = 0;
			if (!smi.IsNullOrStopped())
			{
				freshness = FetchManager.QuantizeRotValue(smi.RotValue);
			}
			KPrefabID kprefabID = pickupable.KPrefabID;
			HandleVector<int>.Handle handle = this.fetchables.Allocate(new FetchManager.Fetchable
			{
				pickupable = pickupable,
				foodQuality = foodQuality,
				freshness = freshness,
				masterPriority = masterPriority,
				tagBitsHash = kprefabID.GetTagsHash()
			});
			if (!smi.IsNullOrStopped())
			{
				this.rotUpdaters[handle] = smi;
			}
			return handle;
		}

		// Token: 0x06006493 RID: 25747 RVA: 0x000E6119 File Offset: 0x000E4319
		public void RemovePickupable(HandleVector<int>.Handle fetchable_handle)
		{
			this.fetchables.Free(fetchable_handle);
			this.rotUpdaters.Remove(fetchable_handle);
		}

		// Token: 0x06006494 RID: 25748 RVA: 0x002CDD40 File Offset: 0x002CBF40
		public void UpdatePickups(PathProber path_prober, Navigator worker_navigator, int worker)
		{
			this.GatherPickupablesWhichCanBePickedUp(worker);
			this.GatherReachablePickups(worker_navigator);
			this.finalPickups.Sort(FetchManager.PickupComparerIncludingPriority.CompareInst);
			if (this.finalPickups.Count > 0)
			{
				FetchManager.Pickup pickup = this.finalPickups[0];
				int num = pickup.tagBitsHash;
				int num2 = this.finalPickups.Count;
				int num3 = 0;
				for (int i = 1; i < this.finalPickups.Count; i++)
				{
					bool flag = false;
					FetchManager.Pickup pickup2 = this.finalPickups[i];
					int tagBitsHash = pickup2.tagBitsHash;
					if (pickup.masterPriority == pickup2.masterPriority && tagBitsHash == num)
					{
						flag = true;
					}
					if (flag)
					{
						num2--;
					}
					else
					{
						num3++;
						pickup = pickup2;
						num = tagBitsHash;
						if (i > num3)
						{
							this.finalPickups[num3] = pickup2;
						}
					}
				}
				this.finalPickups.RemoveRange(num2, this.finalPickups.Count - num2);
			}
		}

		// Token: 0x06006495 RID: 25749 RVA: 0x002CDE2C File Offset: 0x002CC02C
		private void GatherPickupablesWhichCanBePickedUp(int worker)
		{
			this.pickupsWhichCanBePickedUp.Clear();
			foreach (FetchManager.Fetchable fetchable in this.fetchables.GetDataList())
			{
				Pickupable pickupable = fetchable.pickupable;
				if (pickupable.CouldBePickedUpByMinion(worker))
				{
					this.pickupsWhichCanBePickedUp.Add(new FetchManager.Pickup
					{
						pickupable = pickupable,
						tagBitsHash = fetchable.tagBitsHash,
						PathCost = ushort.MaxValue,
						masterPriority = fetchable.masterPriority,
						freshness = fetchable.freshness,
						foodQuality = fetchable.foodQuality
					});
				}
			}
		}

		// Token: 0x06006496 RID: 25750 RVA: 0x002CDEF4 File Offset: 0x002CC0F4
		public void UpdateOffsetTables()
		{
			foreach (FetchManager.Fetchable fetchable in this.fetchables.GetDataList())
			{
				fetchable.pickupable.GetOffsets(fetchable.pickupable.cachedCell);
			}
		}

		// Token: 0x06006497 RID: 25751 RVA: 0x002CDF5C File Offset: 0x002CC15C
		private void GatherReachablePickups(Navigator navigator)
		{
			this.cellCosts.Clear();
			this.finalPickups.Clear();
			foreach (FetchManager.Pickup pickup in this.pickupsWhichCanBePickedUp)
			{
				Pickupable pickupable = pickup.pickupable;
				int num = -1;
				if (!this.cellCosts.TryGetValue(pickupable.cachedCell, out num))
				{
					num = pickupable.GetNavigationCost(navigator, pickupable.cachedCell);
					this.cellCosts[pickupable.cachedCell] = num;
				}
				if (num != -1)
				{
					this.finalPickups.Add(new FetchManager.Pickup
					{
						pickupable = pickupable,
						tagBitsHash = pickup.tagBitsHash,
						PathCost = (ushort)num,
						masterPriority = pickup.masterPriority,
						freshness = pickup.freshness,
						foodQuality = pickup.foodQuality
					});
				}
			}
		}

		// Token: 0x06006498 RID: 25752 RVA: 0x002CE060 File Offset: 0x002CC260
		public void UpdateStorage(HandleVector<int>.Handle fetchable_handle, Storage storage)
		{
			FetchManager.Fetchable data = this.fetchables.GetData(fetchable_handle);
			int masterPriority = 0;
			Pickupable pickupable = data.pickupable;
			if (pickupable.storage != null)
			{
				Prioritizable prioritizable = pickupable.storage.prioritizable;
				if (prioritizable != null)
				{
					masterPriority = prioritizable.GetMasterPriority().priority_value;
				}
			}
			data.masterPriority = masterPriority;
			this.fetchables.SetData(fetchable_handle, data);
		}

		// Token: 0x06006499 RID: 25753 RVA: 0x002CE0CC File Offset: 0x002CC2CC
		public void UpdateTags(HandleVector<int>.Handle fetchable_handle)
		{
			FetchManager.Fetchable data = this.fetchables.GetData(fetchable_handle);
			data.tagBitsHash = data.pickupable.KPrefabID.GetTagsHash();
			this.fetchables.SetData(fetchable_handle, data);
		}

		// Token: 0x0600649A RID: 25754 RVA: 0x002CE10C File Offset: 0x002CC30C
		public void Sim1000ms(float dt)
		{
			foreach (KeyValuePair<HandleVector<int>.Handle, Rottable.Instance> keyValuePair in this.rotUpdaters)
			{
				HandleVector<int>.Handle key = keyValuePair.Key;
				Rottable.Instance value = keyValuePair.Value;
				FetchManager.Fetchable data = this.fetchables.GetData(key);
				data.freshness = FetchManager.QuantizeRotValue(value.RotValue);
				this.fetchables.SetData(key, data);
			}
		}

		// Token: 0x04004862 RID: 18530
		public KCompactedVector<FetchManager.Fetchable> fetchables;

		// Token: 0x04004863 RID: 18531
		public List<FetchManager.Pickup> finalPickups = new List<FetchManager.Pickup>();

		// Token: 0x04004864 RID: 18532
		private Dictionary<HandleVector<int>.Handle, Rottable.Instance> rotUpdaters;

		// Token: 0x04004865 RID: 18533
		private List<FetchManager.Pickup> pickupsWhichCanBePickedUp = new List<FetchManager.Pickup>();

		// Token: 0x04004866 RID: 18534
		private Dictionary<int, int> cellCosts = new Dictionary<int, int>();
	}

	// Token: 0x02001331 RID: 4913
	private struct UpdateOffsetTables : IWorkItem<object>
	{
		// Token: 0x0600649B RID: 25755 RVA: 0x000E6135 File Offset: 0x000E4335
		public UpdateOffsetTables(FetchManager.FetchablesByPrefabId fetchables)
		{
			this.data = fetchables;
			this.failed = ListPool<Pickupable, FetchManager.UpdateOffsetTables>.Allocate();
		}

		// Token: 0x0600649C RID: 25756 RVA: 0x002CE198 File Offset: 0x002CC398
		public void Run(object _, int threadIndex)
		{
			if (Game.IsOnMainThread())
			{
				this.data.UpdateOffsetTables();
				return;
			}
			foreach (FetchManager.Fetchable fetchable in this.data.fetchables.GetDataList())
			{
				if (!fetchable.pickupable.ValidateOffsets(fetchable.pickupable.cachedCell))
				{
					this.failed.Add(fetchable.pickupable);
				}
			}
		}

		// Token: 0x0600649D RID: 25757 RVA: 0x002CE22C File Offset: 0x002CC42C
		public void Finish()
		{
			foreach (Pickupable pickupable in this.failed)
			{
				pickupable.GetOffsets(pickupable.cachedCell);
			}
			this.failed.Recycle();
		}

		// Token: 0x04004868 RID: 18536
		public FetchManager.FetchablesByPrefabId data;

		// Token: 0x04004869 RID: 18537
		private ListPool<Pickupable, FetchManager.UpdateOffsetTables>.PooledList failed;
	}

	// Token: 0x02001332 RID: 4914
	private struct UpdatePickupWorkItem : IWorkItem<object>
	{
		// Token: 0x0600649E RID: 25758 RVA: 0x000E6149 File Offset: 0x000E4349
		public void Run(object shared_data, int threadIndex)
		{
			this.fetchablesByPrefabId.UpdatePickups(this.pathProber, this.navigator, this.worker);
		}

		// Token: 0x0400486A RID: 18538
		public FetchManager.FetchablesByPrefabId fetchablesByPrefabId;

		// Token: 0x0400486B RID: 18539
		public PathProber pathProber;

		// Token: 0x0400486C RID: 18540
		public Navigator navigator;

		// Token: 0x0400486D RID: 18541
		public int worker;
	}
}
