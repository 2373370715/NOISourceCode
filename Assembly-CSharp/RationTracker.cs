using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using UnityEngine;

// Token: 0x02001786 RID: 6022
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/RationTracker")]
public class RationTracker : WorldResourceAmountTracker<RationTracker>, ISaveLoadable
{
	// Token: 0x06007BD0 RID: 31696 RVA: 0x000F5E2F File Offset: 0x000F402F
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.itemTag = GameTags.Edible;
	}

	// Token: 0x06007BD1 RID: 31697 RVA: 0x0032B998 File Offset: 0x00329B98
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (this.caloriesConsumedByFood != null && this.caloriesConsumedByFood.Count > 0)
		{
			foreach (string key in this.caloriesConsumedByFood.Keys)
			{
				float num = this.caloriesConsumedByFood[key];
				float num2 = 0f;
				if (this.amountsConsumedByID.TryGetValue(key, out num2))
				{
					this.amountsConsumedByID[key] = num2 + num;
				}
				else
				{
					this.amountsConsumedByID.Add(key, num);
				}
			}
		}
		this.caloriesConsumedByFood = null;
	}

	// Token: 0x06007BD2 RID: 31698 RVA: 0x0032BA4C File Offset: 0x00329C4C
	protected override WorldResourceAmountTracker<RationTracker>.ItemData GetItemData(Pickupable item)
	{
		Edible component = item.GetComponent<Edible>();
		return new WorldResourceAmountTracker<RationTracker>.ItemData
		{
			ID = component.FoodID,
			amountValue = component.Calories,
			units = component.Units
		};
	}

	// Token: 0x06007BD3 RID: 31699 RVA: 0x0032BA90 File Offset: 0x00329C90
	public float GetAmountConsumed()
	{
		float num = 0f;
		foreach (KeyValuePair<string, float> keyValuePair in this.amountsConsumedByID)
		{
			num += keyValuePair.Value;
		}
		return num;
	}

	// Token: 0x06007BD4 RID: 31700 RVA: 0x0032BAF0 File Offset: 0x00329CF0
	public float GetAmountConsumedForIDs(List<string> itemIDs)
	{
		float num = 0f;
		foreach (string key in itemIDs)
		{
			if (this.amountsConsumedByID.ContainsKey(key))
			{
				num += this.amountsConsumedByID[key];
			}
		}
		return num;
	}

	// Token: 0x06007BD5 RID: 31701 RVA: 0x0032BB5C File Offset: 0x00329D5C
	public float CountAmountForItemWithID(string ID, WorldInventory inventory, bool excludeUnreachable = true)
	{
		float num = 0f;
		ICollection<Pickupable> pickupables = inventory.GetPickupables(this.itemTag, false);
		if (pickupables != null)
		{
			foreach (Pickupable pickupable in pickupables)
			{
				if (!pickupable.KPrefabID.HasTag(GameTags.StoredPrivate))
				{
					WorldResourceAmountTracker<RationTracker>.ItemData itemData = this.GetItemData(pickupable);
					if (itemData.ID == ID)
					{
						num += itemData.amountValue;
					}
				}
			}
		}
		return num;
	}

	// Token: 0x04005D5B RID: 23899
	[Serialize]
	public Dictionary<string, float> caloriesConsumedByFood = new Dictionary<string, float>();
}
