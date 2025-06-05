using System;
using KSerialization;
using UnityEngine;

// Token: 0x020012B5 RID: 4789
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/ElectrobankTracker")]
public class ElectrobankTracker : WorldResourceAmountTracker<ElectrobankTracker>, ISaveLoadable
{
	// Token: 0x060061EF RID: 25071 RVA: 0x000E44BC File Offset: 0x000E26BC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.ignoredTags = new Tag[GameTags.BionicIncompatibleBatteries.Count];
		GameTags.BionicIncompatibleBatteries.CopyTo(this.ignoredTags, 0);
		this.itemTag = GameTags.ChargedPortableBattery;
	}

	// Token: 0x060061F0 RID: 25072 RVA: 0x002C3094 File Offset: 0x002C1294
	protected override WorldResourceAmountTracker<ElectrobankTracker>.ItemData GetItemData(Pickupable item)
	{
		Electrobank component = item.GetComponent<Electrobank>();
		return new WorldResourceAmountTracker<ElectrobankTracker>.ItemData
		{
			ID = component.ID,
			amountValue = component.Charge * item.PrimaryElement.Units,
			units = item.PrimaryElement.Units
		};
	}
}
