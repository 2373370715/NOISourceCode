using System;
using Klei;
using Klei.AI;
using KSerialization;
using TUNING;
using UnityEngine;

// Token: 0x020012E7 RID: 4839
[SerializationConfig(MemberSerialization.OptIn)]
public class Equipment : Assignables
{
	// Token: 0x1700062F RID: 1583
	// (get) Token: 0x06006344 RID: 25412 RVA: 0x000E52D0 File Offset: 0x000E34D0
	// (set) Token: 0x06006345 RID: 25413 RVA: 0x000E52D8 File Offset: 0x000E34D8
	public bool destroyed { get; private set; }

	// Token: 0x06006346 RID: 25414 RVA: 0x002C7D18 File Offset: 0x002C5F18
	public GameObject GetTargetGameObject()
	{
		MinionAssignablesProxy minionAssignablesProxy = (MinionAssignablesProxy)base.GetAssignableIdentity();
		if (minionAssignablesProxy)
		{
			return minionAssignablesProxy.GetTargetGameObject();
		}
		return null;
	}

	// Token: 0x06006347 RID: 25415 RVA: 0x000E52E1 File Offset: 0x000E34E1
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Components.Equipment.Add(this);
	}

	// Token: 0x06006348 RID: 25416 RVA: 0x000E52F4 File Offset: 0x000E34F4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<Equipment>(1502190696, Equipment.SetDestroyedTrueDelegate);
		base.Subscribe<Equipment>(1969584890, Equipment.SetDestroyedTrueDelegate);
	}

	// Token: 0x06006349 RID: 25417 RVA: 0x000E531E File Offset: 0x000E351E
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		this.refreshHandle.ClearScheduler();
		Components.Equipment.Remove(this);
	}

	// Token: 0x0600634A RID: 25418 RVA: 0x002C7D44 File Offset: 0x002C5F44
	public void Equip(Equippable equippable)
	{
		GameObject targetGameObject = this.GetTargetGameObject();
		bool flag = targetGameObject.GetComponent<KBatchedAnimController>() == null;
		if (!flag)
		{
			PrimaryElement component = equippable.GetComponent<PrimaryElement>();
			SimUtil.DiseaseInfo invalid = SimUtil.DiseaseInfo.Invalid;
			invalid.idx = component.DiseaseIdx;
			invalid.count = (int)((float)component.DiseaseCount * 0.33f);
			PrimaryElement component2 = targetGameObject.GetComponent<PrimaryElement>();
			SimUtil.DiseaseInfo invalid2 = SimUtil.DiseaseInfo.Invalid;
			invalid2.idx = component2.DiseaseIdx;
			invalid2.count = (int)((float)component2.DiseaseCount * 0.33f);
			component2.ModifyDiseaseCount(-invalid2.count, "Equipment.Equip");
			component.ModifyDiseaseCount(-invalid.count, "Equipment.Equip");
			if (invalid2.count > 0)
			{
				component.AddDisease(invalid2.idx, invalid2.count, "Equipment.Equip");
			}
			if (invalid.count > 0)
			{
				component2.AddDisease(invalid.idx, invalid.count, "Equipment.Equip");
			}
		}
		AssignableSlotInstance slot = base.GetSlot(equippable.slot);
		slot.Assign(equippable);
		global::Debug.Assert(targetGameObject, "GetTargetGameObject returned null in Equip");
		targetGameObject.Trigger(-448952673, equippable.GetComponent<KPrefabID>());
		equippable.Trigger(-1617557748, this);
		Attributes attributes = targetGameObject.GetAttributes();
		if (attributes != null)
		{
			foreach (AttributeModifier modifier in equippable.def.AttributeModifiers)
			{
				attributes.Add(modifier);
			}
		}
		SnapOn component3 = targetGameObject.GetComponent<SnapOn>();
		if (component3 != null)
		{
			component3.AttachSnapOnByName(equippable.def.SnapOn);
			if (equippable.def.SnapOn1 != null)
			{
				component3.AttachSnapOnByName(equippable.def.SnapOn1);
			}
		}
		if (equippable.transform.parent)
		{
			Storage component4 = equippable.transform.parent.GetComponent<Storage>();
			if (component4)
			{
				component4.Drop(equippable.gameObject, true);
			}
		}
		equippable.transform.parent = slot.gameObject.transform;
		equippable.transform.SetLocalPosition(Vector3.zero);
		this.SetEquippableStoredModifiers(equippable, true);
		equippable.OnEquip(slot);
		if (this.refreshHandle.TimeRemaining > 0f)
		{
			global::Debug.LogWarning(targetGameObject.GetProperName() + " is already in the process of changing equipment (equip)");
			this.refreshHandle.ClearScheduler();
		}
		CreatureSimTemperatureTransfer transferer = targetGameObject.GetComponent<CreatureSimTemperatureTransfer>();
		if (!flag)
		{
			this.refreshHandle = GameScheduler.Instance.Schedule("ChangeEquipment", 2f, delegate(object obj)
			{
				if (transferer != null)
				{
					transferer.RefreshRegistration();
				}
			}, null, null);
		}
		Game.Instance.Trigger(-2146166042, null);
	}

	// Token: 0x0600634B RID: 25419 RVA: 0x002C8014 File Offset: 0x002C6214
	public void Unequip(Equippable equippable)
	{
		AssignableSlotInstance slot = base.GetSlot(equippable.slot);
		slot.Unassign(true);
		GameObject targetGameObject = this.GetTargetGameObject();
		MinionResume minionResume = (targetGameObject != null) ? targetGameObject.GetComponent<MinionResume>() : null;
		Durability component = equippable.GetComponent<Durability>();
		if (component && minionResume && !slot.IsUnassigning() && minionResume.HasPerk(Db.Get().SkillPerks.ExosuitDurability.Id))
		{
			float num = (GameClock.Instance.GetTimeInCycles() - component.TimeEquipped) * EQUIPMENT.SUITS.SUIT_DURABILITY_SKILL_BONUS;
			component.TimeEquipped += num;
		}
		equippable.Trigger(-170173755, this);
		if (!targetGameObject)
		{
			return;
		}
		targetGameObject.Trigger(-1285462312, equippable.GetComponent<KPrefabID>());
		KBatchedAnimController component2 = targetGameObject.GetComponent<KBatchedAnimController>();
		if (!this.destroyed)
		{
			Attributes attributes = targetGameObject.GetAttributes();
			if (attributes != null)
			{
				foreach (AttributeModifier modifier in equippable.def.AttributeModifiers)
				{
					attributes.Remove(modifier);
				}
			}
			if (!equippable.def.IsBody)
			{
				SnapOn component3 = targetGameObject.GetComponent<SnapOn>();
				if (equippable.def.SnapOn != null)
				{
					component3.DetachSnapOnByName(equippable.def.SnapOn);
				}
				if (equippable.def.SnapOn1 != null)
				{
					component3.DetachSnapOnByName(equippable.def.SnapOn1);
				}
			}
			if (equippable.transform.parent)
			{
				Storage component4 = equippable.transform.parent.GetComponent<Storage>();
				if (component4)
				{
					component4.Drop(equippable.gameObject, true);
				}
			}
			this.SetEquippableStoredModifiers(equippable, false);
			equippable.transform.parent = null;
			equippable.transform.SetPosition(targetGameObject.transform.GetPosition() + Vector3.up / 2f);
			KBatchedAnimController component5 = equippable.GetComponent<KBatchedAnimController>();
			if (component5)
			{
				component5.SetSceneLayer(Grid.SceneLayer.Ore);
			}
			if (!(component2 == null))
			{
				if (this.refreshHandle.TimeRemaining > 0f)
				{
					this.refreshHandle.ClearScheduler();
				}
				Equipment instance = this;
				this.refreshHandle = GameScheduler.Instance.Schedule("ChangeEquipment", 1f, delegate(object obj)
				{
					GameObject gameObject = (instance != null) ? instance.GetTargetGameObject() : null;
					if (gameObject)
					{
						CreatureSimTemperatureTransfer component8 = gameObject.GetComponent<CreatureSimTemperatureTransfer>();
						if (component8 != null)
						{
							component8.RefreshRegistration();
						}
					}
				}, null, null);
			}
			if (!slot.IsUnassigning())
			{
				PrimaryElement component6 = equippable.GetComponent<PrimaryElement>();
				PrimaryElement component7 = targetGameObject.GetComponent<PrimaryElement>();
				if (component6 != null && component7 != null)
				{
					SimUtil.DiseaseInfo invalid = SimUtil.DiseaseInfo.Invalid;
					invalid.idx = component6.DiseaseIdx;
					invalid.count = (int)((float)component6.DiseaseCount * 0.33f);
					SimUtil.DiseaseInfo invalid2 = SimUtil.DiseaseInfo.Invalid;
					invalid2.idx = component7.DiseaseIdx;
					invalid2.count = (int)((float)component7.DiseaseCount * 0.33f);
					component7.ModifyDiseaseCount(-invalid2.count, "Equipment.Unequip");
					component6.ModifyDiseaseCount(-invalid.count, "Equipment.Unequip");
					if (invalid2.count > 0)
					{
						component6.AddDisease(invalid2.idx, invalid2.count, "Equipment.Unequip");
					}
					if (invalid.count > 0)
					{
						component7.AddDisease(invalid.idx, invalid.count, "Equipment.Unequip");
					}
					if (component != null && component.IsWornOut())
					{
						component.ConvertToWornObject();
					}
				}
			}
		}
		Game.Instance.Trigger(-2146166042, null);
	}

	// Token: 0x0600634C RID: 25420 RVA: 0x000E533C File Offset: 0x000E353C
	public bool IsEquipped(Equippable equippable)
	{
		return equippable.assignee is Equipment && (Equipment)equippable.assignee == this && equippable.isEquipped;
	}

	// Token: 0x0600634D RID: 25421 RVA: 0x002C83B4 File Offset: 0x002C65B4
	public bool IsSlotOccupied(AssignableSlot slot)
	{
		EquipmentSlotInstance equipmentSlotInstance = base.GetSlot(slot) as EquipmentSlotInstance;
		return equipmentSlotInstance.IsAssigned() && (equipmentSlotInstance.assignable as Equippable).isEquipped;
	}

	// Token: 0x0600634E RID: 25422 RVA: 0x002C83E8 File Offset: 0x002C65E8
	public void UnequipAll()
	{
		foreach (AssignableSlotInstance assignableSlotInstance in this.slots)
		{
			if (assignableSlotInstance.assignable != null)
			{
				assignableSlotInstance.assignable.Unassign();
			}
		}
	}

	// Token: 0x0600634F RID: 25423 RVA: 0x000E5366 File Offset: 0x000E3566
	private void SetEquippableStoredModifiers(Equippable equippable, bool isStoring)
	{
		GameObject gameObject = equippable.gameObject;
		Storage.MakeItemTemperatureInsulated(gameObject, isStoring, false);
		Storage.MakeItemInvisible(gameObject, isStoring, false);
	}

	// Token: 0x0400472B RID: 18219
	private SchedulerHandle refreshHandle;

	// Token: 0x0400472D RID: 18221
	private static readonly EventSystem.IntraObjectHandler<Equipment> SetDestroyedTrueDelegate = new EventSystem.IntraObjectHandler<Equipment>(delegate(Equipment component, object data)
	{
		component.destroyed = true;
	});
}
