using System;
using UnityEngine;

// Token: 0x02001816 RID: 6166
public class FetchDrone : KMonoBehaviour
{
	// Token: 0x06007EF7 RID: 32503 RVA: 0x0033A52C File Offset: 0x0033872C
	protected override void OnSpawn()
	{
		ChoreGroup[] array = new ChoreGroup[]
		{
			Db.Get().ChoreGroups.Build,
			Db.Get().ChoreGroups.Basekeeping,
			Db.Get().ChoreGroups.Cook,
			Db.Get().ChoreGroups.Art,
			Db.Get().ChoreGroups.Dig,
			Db.Get().ChoreGroups.Research,
			Db.Get().ChoreGroups.Farming,
			Db.Get().ChoreGroups.Ranching,
			Db.Get().ChoreGroups.MachineOperating,
			Db.Get().ChoreGroups.MedicalAid,
			Db.Get().ChoreGroups.Combat,
			Db.Get().ChoreGroups.LifeSupport,
			Db.Get().ChoreGroups.Recreation,
			Db.Get().ChoreGroups.Toggle,
			Db.Get().ChoreGroups.Rocketry
		};
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] != null)
			{
				this.choreConsumer.SetPermittedByUser(array[i], false);
			}
		}
		foreach (Storage storage in base.GetComponents<Storage>())
		{
			if (storage.storageID != GameTags.ChargedPortableBattery)
			{
				this.pickupableStorage = storage;
				break;
			}
		}
		this.animController = base.GetComponent<KBatchedAnimController>();
		this.pickupableStorage.Subscribe(-1697596308, new Action<object>(this.OnStorageChanged));
		base.Subscribe(-1582839653, new Action<object>(this.OnTagsChanged));
	}

	// Token: 0x06007EF8 RID: 32504 RVA: 0x000F8128 File Offset: 0x000F6328
	protected override void OnCleanUp()
	{
		base.Unsubscribe(-1697596308);
		base.Unsubscribe(-1582839653);
		base.OnCleanUp();
	}

	// Token: 0x06007EF9 RID: 32505 RVA: 0x0033A6F0 File Offset: 0x003388F0
	private void OnTagsChanged(object data)
	{
		TagChangedEventData tagChangedEventData = (TagChangedEventData)data;
		if (tagChangedEventData.added && tagChangedEventData.tag == GameTags.Creatures.Die)
		{
			Brain component = base.GetComponent<Brain>();
			if (component != null && !component.IsRunning())
			{
				component.Resume("death");
			}
		}
	}

	// Token: 0x06007EFA RID: 32506 RVA: 0x0033A744 File Offset: 0x00338944
	private void OnStorageChanged(object data)
	{
		GameObject gameObject = (GameObject)data;
		this.RemoveTracker(gameObject);
		this.ShowPickupSymbol(gameObject);
	}

	// Token: 0x06007EFB RID: 32507 RVA: 0x0033A768 File Offset: 0x00338968
	private void ShowPickupSymbol(GameObject pickupable)
	{
		bool flag = this.pickupableStorage.items.Contains(pickupable);
		if (flag)
		{
			this.AddAnimTracker(pickupable);
		}
		this.animController.SetSymbolVisiblity(FetchDrone.BOTTOM, !flag);
		this.animController.SetSymbolVisiblity(FetchDrone.BOTTOM_CARRY, flag);
	}

	// Token: 0x06007EFC RID: 32508 RVA: 0x0033A7C0 File Offset: 0x003389C0
	private void AddAnimTracker(GameObject go)
	{
		KAnimControllerBase component = go.GetComponent<KAnimControllerBase>();
		if (component == null)
		{
			return;
		}
		if (component.AnimFiles != null && component.AnimFiles.Length != 0 && component.AnimFiles[0] != null && component.GetComponent<Pickupable>().trackOnPickup)
		{
			KBatchedAnimTracker kbatchedAnimTracker = go.GetComponent<KBatchedAnimTracker>();
			if (kbatchedAnimTracker != null && kbatchedAnimTracker.controller == this.animController)
			{
				return;
			}
			kbatchedAnimTracker = go.AddComponent<KBatchedAnimTracker>();
			kbatchedAnimTracker.useTargetPoint = false;
			kbatchedAnimTracker.fadeOut = false;
			kbatchedAnimTracker.symbol = ((go.GetComponent<Brain>() != null) ? new HashedString("snapTo_pivot") : new HashedString("snapTo_thing"));
			kbatchedAnimTracker.forceAlwaysVisible = true;
		}
	}

	// Token: 0x06007EFD RID: 32509 RVA: 0x0033A87C File Offset: 0x00338A7C
	private void RemoveTracker(GameObject go)
	{
		KBatchedAnimTracker kbatchedAnimTracker = (go != null) ? go.GetComponent<KBatchedAnimTracker>() : null;
		if (kbatchedAnimTracker != null && kbatchedAnimTracker.controller == this.animController)
		{
			UnityEngine.Object.Destroy(kbatchedAnimTracker);
		}
	}

	// Token: 0x0400607D RID: 24701
	private static string BOTTOM = "bottom";

	// Token: 0x0400607E RID: 24702
	private static string BOTTOM_CARRY = "bottom_carry";

	// Token: 0x0400607F RID: 24703
	private KBatchedAnimController animController;

	// Token: 0x04006080 RID: 24704
	private Storage pickupableStorage;

	// Token: 0x04006081 RID: 24705
	[MyCmpAdd]
	private ChoreConsumer choreConsumer;
}
