using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000C7B RID: 3195
public class BionicMinionStorageExtension : KMonoBehaviour, StoredMinionIdentity.IStoredMinionExtension
{
	// Token: 0x06003CAB RID: 15531 RVA: 0x0023CE34 File Offset: 0x0023B034
	public void AddStoredMinionGameObjectRequirements(GameObject storedMinionGameObject)
	{
		Storage[] components = storedMinionGameObject.GetComponents<Storage>();
		using (List<Tag>.Enumerator enumerator = BionicMinionStorageExtension.StoragesTypesToTransfer.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Tag inventoryType = enumerator.Current;
				if (components == null || !(components.FindFirst((Storage s) => s.storageID == inventoryType) != null))
				{
					Storage storage = storedMinionGameObject.AddComponent<Storage>();
					storage.allowItemRemoval = false;
					storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
					storage.storageID = inventoryType;
				}
			}
		}
	}

	// Token: 0x06003CAC RID: 15532 RVA: 0x0023CED4 File Offset: 0x0023B0D4
	void StoredMinionIdentity.IStoredMinionExtension.PullFrom(StoredMinionIdentity source)
	{
		Storage[] components = source.GetComponents<Storage>();
		Storage[] components2 = base.GetComponents<Storage>();
		foreach (Storage storage in components)
		{
			bool test = false;
			foreach (Storage storage2 in components2)
			{
				if (storage2.storageID == storage.storageID)
				{
					storage.Transfer(storage2, false, true);
					test = true;
					break;
				}
			}
			DebugUtil.DevAssert(test, "Missmatched storages on BionicMinionStorageExtension", null);
		}
	}

	// Token: 0x06003CAD RID: 15533 RVA: 0x0023CF54 File Offset: 0x0023B154
	void StoredMinionIdentity.IStoredMinionExtension.PushTo(StoredMinionIdentity destination)
	{
		GameObject gameObject = destination.gameObject;
		this.AddStoredMinionGameObjectRequirements(gameObject);
		Storage[] components = base.GetComponents<Storage>();
		Storage[] components2 = gameObject.GetComponents<Storage>();
		foreach (Tag b in BionicMinionStorageExtension.StoragesTypesToTransfer)
		{
			Storage storage = null;
			Storage target = null;
			foreach (Storage storage2 in components)
			{
				if (storage2.storageID == b)
				{
					storage = storage2;
					break;
				}
			}
			foreach (Storage storage3 in components2)
			{
				if (storage3.storageID == b)
				{
					target = storage3;
					break;
				}
			}
			storage.Transfer(target, true, true);
		}
	}

	// Token: 0x04002A1B RID: 10779
	private static readonly List<Tag> StoragesTypesToTransfer = new List<Tag>
	{
		GameTags.StoragesIds.BionicBatteryStorage,
		GameTags.StoragesIds.BionicUpgradeStorage,
		GameTags.StoragesIds.BionicOxygenTankStorage
	};
}
