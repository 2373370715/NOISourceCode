using System;
using STRINGS;
using UnityEngine;

// Token: 0x02001A27 RID: 6695
[AddComponentMenu("KMonoBehaviour/scripts/SuitEquipper")]
public class SuitEquipper : KMonoBehaviour
{
	// Token: 0x06008B6D RID: 35693 RVA: 0x000FFC7D File Offset: 0x000FDE7D
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<SuitEquipper>(493375141, SuitEquipper.OnRefreshUserMenuDelegate);
	}

	// Token: 0x06008B6E RID: 35694 RVA: 0x0036DB60 File Offset: 0x0036BD60
	private void OnRefreshUserMenu(object data)
	{
		foreach (AssignableSlotInstance assignableSlotInstance in base.GetComponent<MinionIdentity>().GetEquipment().Slots)
		{
			EquipmentSlotInstance equipmentSlotInstance = (EquipmentSlotInstance)assignableSlotInstance;
			Equippable equippable = equipmentSlotInstance.assignable as Equippable;
			if (equippable && equippable.unequippable)
			{
				string text = string.Format(UI.USERMENUACTIONS.UNEQUIP.NAME, equippable.def.GenericName);
				Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("iconDown", text, delegate()
				{
					equippable.Unassign();
				}, global::Action.NumActions, null, null, null, "", true), 2f);
			}
		}
	}

	// Token: 0x06008B6F RID: 35695 RVA: 0x0036DC54 File Offset: 0x0036BE54
	public Equippable IsWearingAirtightSuit()
	{
		Equippable result = null;
		foreach (AssignableSlotInstance assignableSlotInstance in base.GetComponent<MinionIdentity>().GetEquipment().Slots)
		{
			Equippable equippable = ((EquipmentSlotInstance)assignableSlotInstance).assignable as Equippable;
			if (equippable && equippable.GetComponent<KPrefabID>().HasTag(GameTags.AirtightSuit) && equippable.isEquipped)
			{
				result = equippable;
				break;
			}
		}
		return result;
	}

	// Token: 0x04006946 RID: 26950
	private static readonly EventSystem.IntraObjectHandler<SuitEquipper> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<SuitEquipper>(delegate(SuitEquipper component, object data)
	{
		component.OnRefreshUserMenu(data);
	});
}
