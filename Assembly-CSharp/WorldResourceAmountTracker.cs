using System;
using System.Collections.Generic;
using KSerialization;

// Token: 0x02001AB1 RID: 6833
[SerializationConfig(MemberSerialization.OptIn)]
public abstract class WorldResourceAmountTracker<T> : KMonoBehaviour where T : KMonoBehaviour
{
	// Token: 0x06008EF6 RID: 36598 RVA: 0x00101D19 File Offset: 0x000FFF19
	public static void DestroyInstance()
	{
		WorldResourceAmountTracker<T>.instance = default(T);
	}

	// Token: 0x06008EF7 RID: 36599 RVA: 0x00101D26 File Offset: 0x000FFF26
	public static T Get()
	{
		return WorldResourceAmountTracker<T>.instance;
	}

	// Token: 0x06008EF8 RID: 36600 RVA: 0x0037C74C File Offset: 0x0037A94C
	protected override void OnPrefabInit()
	{
		Debug.Assert(WorldResourceAmountTracker<T>.instance == null, "Error, WorldResourceAmountTracker of type T has already been initialize and another instance is attempting to initialize. this isn't allowed because T is meant to be a singleton, ensure only one instance exist. existing instance GameObject: " + ((WorldResourceAmountTracker<T>.instance == null) ? "" : WorldResourceAmountTracker<T>.instance.gameObject.name) + ". Error triggered by instance of T in GameObject: " + base.gameObject.name);
		WorldResourceAmountTracker<T>.instance = (this as T);
		this.itemTag = GameTags.Edible;
	}

	// Token: 0x06008EF9 RID: 36601 RVA: 0x00101D2D File Offset: 0x000FFF2D
	protected override void OnSpawn()
	{
		base.Subscribe(631075836, new Action<object>(this.OnNewDay));
	}

	// Token: 0x06008EFA RID: 36602 RVA: 0x00101D47 File Offset: 0x000FFF47
	private void OnNewDay(object data)
	{
		this.previousFrame = this.currentFrame;
		this.currentFrame = default(WorldResourceAmountTracker<T>.Frame);
	}

	// Token: 0x06008EFB RID: 36603
	protected abstract WorldResourceAmountTracker<T>.ItemData GetItemData(Pickupable item);

	// Token: 0x06008EFC RID: 36604 RVA: 0x0037C7D0 File Offset: 0x0037A9D0
	public float CountAmount(Dictionary<string, float> unitCountByID, WorldInventory inventory, bool excludeUnreachable = true)
	{
		float num;
		return this.CountAmount(unitCountByID, out num, inventory, excludeUnreachable);
	}

	// Token: 0x06008EFD RID: 36605 RVA: 0x0037C7E8 File Offset: 0x0037A9E8
	public float CountAmount(Dictionary<string, float> unitCountByID, out float totalUnitsFound, WorldInventory inventory, bool excludeUnreachable)
	{
		float num = 0f;
		totalUnitsFound = 0f;
		ICollection<Pickupable> pickupables = inventory.GetPickupables(this.itemTag, false);
		if (pickupables != null)
		{
			foreach (Pickupable pickupable in pickupables)
			{
				if (!pickupable.KPrefabID.HasTag(GameTags.StoredPrivate))
				{
					if (this.ignoredTags != null)
					{
						bool flag = false;
						foreach (Tag tag in this.ignoredTags)
						{
							if (pickupable.KPrefabID.HasTag(tag))
							{
								flag = true;
								break;
							}
						}
						if (flag)
						{
							continue;
						}
					}
					WorldResourceAmountTracker<T>.ItemData itemData = this.GetItemData(pickupable);
					num += itemData.amountValue;
					if (unitCountByID != null)
					{
						if (!unitCountByID.ContainsKey(itemData.ID))
						{
							unitCountByID[itemData.ID] = 0f;
						}
						string id = itemData.ID;
						unitCountByID[id] += itemData.units;
					}
					totalUnitsFound += itemData.units;
				}
			}
		}
		return num;
	}

	// Token: 0x06008EFE RID: 36606 RVA: 0x00101D61 File Offset: 0x000FFF61
	public void RegisterAmountProduced(float val)
	{
		this.currentFrame.amountProduced = this.currentFrame.amountProduced + val;
	}

	// Token: 0x06008EFF RID: 36607 RVA: 0x0037C918 File Offset: 0x0037AB18
	public void RegisterAmountConsumed(string ID, float valueConsumed)
	{
		this.currentFrame.amountConsumed = this.currentFrame.amountConsumed + valueConsumed;
		if (!this.amountsConsumedByID.ContainsKey(ID))
		{
			this.amountsConsumedByID.Add(ID, valueConsumed);
			return;
		}
		Dictionary<string, float> dictionary = this.amountsConsumedByID;
		dictionary[ID] += valueConsumed;
	}

	// Token: 0x04006BB5 RID: 27573
	private static T instance;

	// Token: 0x04006BB6 RID: 27574
	[Serialize]
	public WorldResourceAmountTracker<T>.Frame currentFrame;

	// Token: 0x04006BB7 RID: 27575
	[Serialize]
	public WorldResourceAmountTracker<T>.Frame previousFrame;

	// Token: 0x04006BB8 RID: 27576
	[Serialize]
	public Dictionary<string, float> amountsConsumedByID = new Dictionary<string, float>();

	// Token: 0x04006BB9 RID: 27577
	protected Tag itemTag;

	// Token: 0x04006BBA RID: 27578
	protected Tag[] ignoredTags;

	// Token: 0x02001AB2 RID: 6834
	protected struct ItemData
	{
		// Token: 0x04006BBB RID: 27579
		public string ID;

		// Token: 0x04006BBC RID: 27580
		public float amountValue;

		// Token: 0x04006BBD RID: 27581
		public float units;
	}

	// Token: 0x02001AB3 RID: 6835
	public struct Frame
	{
		// Token: 0x04006BBE RID: 27582
		public float amountProduced;

		// Token: 0x04006BBF RID: 27583
		public float amountConsumed;
	}
}
