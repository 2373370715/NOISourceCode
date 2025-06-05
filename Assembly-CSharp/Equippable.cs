using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x020012F2 RID: 4850
[SerializationConfig(MemberSerialization.OptIn)]
public class Equippable : Assignable, ISaveLoadable, IGameObjectEffectDescriptor, IQuality
{
	// Token: 0x0600636C RID: 25452 RVA: 0x000E5509 File Offset: 0x000E3709
	public global::QualityLevel GetQuality()
	{
		return this.quality;
	}

	// Token: 0x0600636D RID: 25453 RVA: 0x000E5511 File Offset: 0x000E3711
	public void SetQuality(global::QualityLevel level)
	{
		this.quality = level;
	}

	// Token: 0x17000636 RID: 1590
	// (get) Token: 0x0600636E RID: 25454 RVA: 0x000E551A File Offset: 0x000E371A
	// (set) Token: 0x0600636F RID: 25455 RVA: 0x000E5527 File Offset: 0x000E3727
	public EquipmentDef def
	{
		get
		{
			return this.defHandle.Get<EquipmentDef>();
		}
		set
		{
			this.defHandle.Set<EquipmentDef>(value);
		}
	}

	// Token: 0x06006370 RID: 25456 RVA: 0x002C8778 File Offset: 0x002C6978
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (this.def.AdditionalTags != null)
		{
			foreach (Tag tag in this.def.AdditionalTags)
			{
				base.GetComponent<KPrefabID>().AddTag(tag, false);
			}
		}
	}

	// Token: 0x06006371 RID: 25457 RVA: 0x002C87C8 File Offset: 0x002C69C8
	protected override void OnSpawn()
	{
		Components.AssignableItems.Add(this);
		if (this.isEquipped)
		{
			if (this.assignee != null && this.assignee is MinionIdentity)
			{
				this.assignee = (this.assignee as MinionIdentity).assignableProxy.Get();
				this.assignee_identityRef.Set(this.assignee as KMonoBehaviour);
			}
			if (this.assignee == null && this.assignee_identityRef.Get() != null)
			{
				this.assignee = this.assignee_identityRef.Get().GetComponent<IAssignableIdentity>();
			}
			if (this.assignee != null)
			{
				Equipment component = this.assignee.GetSoleOwner().GetComponent<Equipment>();
				bool flag = true;
				UnityEngine.Object component2 = component.GetComponent<MinionAssignablesProxy>();
				GameObject gameObject = null;
				if (component2 != null)
				{
					gameObject = component.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
					if (gameObject != null)
					{
						flag = gameObject.GetComponent<KPrefabID>().isSpawned;
					}
				}
				if (flag)
				{
					this.EquipToAssignable();
				}
				else
				{
					gameObject.Subscribe(1589886948, new Action<object>(this.OnAsigneeSpawnedAndReadyForEquip));
				}
			}
			else
			{
				global::Debug.LogWarning("Equippable trying to be equipped to missing prefab");
				this.isEquipped = false;
			}
		}
		base.Subscribe<Equippable>(1969584890, Equippable.SetDestroyedTrueDelegate);
	}

	// Token: 0x06006372 RID: 25458 RVA: 0x000E5535 File Offset: 0x000E3735
	private void EquipToAssignable()
	{
		if (this.assignee != null)
		{
			this.assignee.GetSoleOwner().GetComponent<Equipment>().Equip(this);
		}
	}

	// Token: 0x06006373 RID: 25459 RVA: 0x000E5555 File Offset: 0x000E3755
	private void OnAsigneeSpawnedAndReadyForEquip(object o)
	{
		GameObject go = (GameObject)o;
		this.EquipToAssignable();
		go.Unsubscribe(1589886948, new Action<object>(this.OnAsigneeSpawnedAndReadyForEquip));
	}

	// Token: 0x06006374 RID: 25460 RVA: 0x002C88F8 File Offset: 0x002C6AF8
	public KAnimFile GetBuildOverride()
	{
		EquippableFacade component = base.GetComponent<EquippableFacade>();
		if (component == null || component.BuildOverride == null)
		{
			return this.def.BuildOverride;
		}
		return Assets.GetAnim(component.BuildOverride);
	}

	// Token: 0x06006375 RID: 25461 RVA: 0x002C893C File Offset: 0x002C6B3C
	public override void Assign(IAssignableIdentity new_assignee)
	{
		if (new_assignee == this.assignee)
		{
			return;
		}
		if (base.slot != null && new_assignee is MinionIdentity)
		{
			new_assignee = (new_assignee as MinionIdentity).assignableProxy.Get();
		}
		if (base.slot != null && new_assignee is StoredMinionIdentity)
		{
			new_assignee = (new_assignee as StoredMinionIdentity).assignableProxy.Get();
		}
		if (new_assignee is MinionAssignablesProxy)
		{
			AssignableSlotInstance slot = new_assignee.GetSoleOwner().GetComponent<Equipment>().GetSlot(base.slot);
			if (slot != null)
			{
				Assignable assignable = slot.assignable;
				if (assignable != null)
				{
					assignable.Unassign();
				}
			}
		}
		base.Assign(new_assignee);
	}

	// Token: 0x06006376 RID: 25462 RVA: 0x002C89D8 File Offset: 0x002C6BD8
	public override void Unassign()
	{
		if (this.isEquipped)
		{
			((this.assignee is MinionIdentity) ? ((MinionIdentity)this.assignee).assignableProxy.Get().GetComponent<Equipment>() : ((KMonoBehaviour)this.assignee).GetComponent<Equipment>()).Unequip(this);
			this.OnUnequip();
		}
		base.Unassign();
	}

	// Token: 0x06006377 RID: 25463 RVA: 0x002C8A38 File Offset: 0x002C6C38
	public void OnEquip(AssignableSlotInstance slot)
	{
		this.isEquipped = true;
		if (SelectTool.Instance.selected == this.selectable)
		{
			SelectTool.Instance.Select(null, false);
		}
		base.GetComponent<KBatchedAnimController>().enabled = false;
		base.GetComponent<KSelectable>().IsSelectable = false;
		string name = base.GetComponent<KPrefabID>().PrefabTag.Name;
		GameObject targetGameObject = slot.gameObject.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
		Effects component = targetGameObject.GetComponent<Effects>();
		if (component != null)
		{
			foreach (Effect effect in this.def.EffectImmunites)
			{
				component.AddImmunity(effect, name, true);
			}
		}
		if (this.def.OnEquipCallBack != null)
		{
			this.def.OnEquipCallBack(this);
		}
		base.GetComponent<KPrefabID>().AddTag(GameTags.Equipped, false);
		targetGameObject.Trigger(-210173199, this);
	}

	// Token: 0x06006378 RID: 25464 RVA: 0x002C8B44 File Offset: 0x002C6D44
	public void OnUnequip()
	{
		this.isEquipped = false;
		if (this.destroyed)
		{
			return;
		}
		base.GetComponent<KPrefabID>().RemoveTag(GameTags.Equipped);
		base.GetComponent<KBatchedAnimController>().enabled = true;
		base.GetComponent<KSelectable>().IsSelectable = true;
		string name = base.GetComponent<KPrefabID>().PrefabTag.Name;
		if (this.assignee != null)
		{
			Ownables soleOwner = this.assignee.GetSoleOwner();
			if (soleOwner)
			{
				GameObject targetGameObject = soleOwner.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
				if (targetGameObject)
				{
					Effects component = targetGameObject.GetComponent<Effects>();
					if (component != null)
					{
						foreach (Effect effect in this.def.EffectImmunites)
						{
							component.RemoveImmunity(effect, name);
						}
					}
				}
			}
		}
		if (this.def.OnUnequipCallBack != null)
		{
			this.def.OnUnequipCallBack(this);
		}
		if (this.assignee != null)
		{
			Ownables soleOwner2 = this.assignee.GetSoleOwner();
			if (soleOwner2)
			{
				GameObject targetGameObject2 = soleOwner2.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
				if (targetGameObject2)
				{
					targetGameObject2.Trigger(-1841406856, this);
				}
			}
		}
	}

	// Token: 0x06006379 RID: 25465 RVA: 0x002C8C8C File Offset: 0x002C6E8C
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		if (this.def != null)
		{
			List<Descriptor> equipmentEffects = GameUtil.GetEquipmentEffects(this.def);
			if (this.def.additionalDescriptors != null)
			{
				foreach (Descriptor item in this.def.additionalDescriptors)
				{
					equipmentEffects.Add(item);
				}
			}
			return equipmentEffects;
		}
		return new List<Descriptor>();
	}

	// Token: 0x0400474E RID: 18254
	private global::QualityLevel quality;

	// Token: 0x0400474F RID: 18255
	[MyCmpAdd]
	private EquippableWorkable equippableWorkable;

	// Token: 0x04004750 RID: 18256
	[MyCmpAdd]
	private EquippableFacade facade;

	// Token: 0x04004751 RID: 18257
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04004752 RID: 18258
	public DefHandle defHandle;

	// Token: 0x04004753 RID: 18259
	[Serialize]
	public bool isEquipped;

	// Token: 0x04004754 RID: 18260
	private bool destroyed;

	// Token: 0x04004755 RID: 18261
	[Serialize]
	public bool unequippable = true;

	// Token: 0x04004756 RID: 18262
	[Serialize]
	public bool hideInCodex;

	// Token: 0x04004757 RID: 18263
	private static readonly EventSystem.IntraObjectHandler<Equippable> SetDestroyedTrueDelegate = new EventSystem.IntraObjectHandler<Equippable>(delegate(Equippable component, object data)
	{
		component.destroyed = true;
	});
}
