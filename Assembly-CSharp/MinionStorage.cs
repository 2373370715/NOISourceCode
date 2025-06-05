using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x02001521 RID: 5409
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/MinionStorage")]
public class MinionStorage : KMonoBehaviour
{
	// Token: 0x06007064 RID: 28772 RVA: 0x000EE082 File Offset: 0x000EC282
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Components.MinionStorages.Add(this);
	}

	// Token: 0x06007065 RID: 28773 RVA: 0x000EE095 File Offset: 0x000EC295
	protected override void OnCleanUp()
	{
		Components.MinionStorages.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x06007066 RID: 28774 RVA: 0x00305148 File Offset: 0x00303348
	private KPrefabID CreateSerializedMinion(GameObject src_minion)
	{
		GameObject gameObject = Util.KInstantiate(SaveLoader.Instance.saveManager.GetPrefab(StoredMinionConfig.ID), Vector3.zero);
		gameObject.SetActive(true);
		MinionIdentity component = src_minion.GetComponent<MinionIdentity>();
		StoredMinionIdentity component2 = gameObject.GetComponent<StoredMinionIdentity>();
		this.CopyMinion(component, component2);
		MinionStorage.RedirectInstanceTracker(src_minion, gameObject);
		component.assignableProxy.Get().SetTarget(component2, gameObject);
		Util.KDestroyGameObject(src_minion);
		return gameObject.GetComponent<KPrefabID>();
	}

	// Token: 0x06007067 RID: 28775 RVA: 0x003051BC File Offset: 0x003033BC
	private void CopyMinion(MinionIdentity src_id, StoredMinionIdentity dest_id)
	{
		dest_id.storedName = src_id.name;
		dest_id.nameStringKey = src_id.nameStringKey;
		dest_id.personalityResourceId = src_id.personalityResourceId;
		dest_id.model = src_id.model;
		dest_id.gender = src_id.gender;
		dest_id.genderStringKey = src_id.genderStringKey;
		dest_id.arrivalTime = src_id.arrivalTime;
		dest_id.voiceIdx = src_id.voiceIdx;
		dest_id.bodyData = src_id.GetComponent<Accessorizer>().bodyData;
		Traits component = src_id.GetComponent<Traits>();
		dest_id.traitIDs = new List<string>(component.GetTraitIds());
		dest_id.assignableProxy.Set(src_id.assignableProxy.Get());
		dest_id.assignableProxy.Get().SetTarget(dest_id, dest_id.gameObject);
		Accessorizer component2 = src_id.GetComponent<Accessorizer>();
		dest_id.accessories = component2.GetAccessories();
		WearableAccessorizer component3 = src_id.GetComponent<WearableAccessorizer>();
		dest_id.customClothingItems = component3.GetCustomClothingItems();
		dest_id.wearables = component3.Wearables;
		ConsumableConsumer component4 = src_id.GetComponent<ConsumableConsumer>();
		if (component4.forbiddenTagSet != null)
		{
			dest_id.forbiddenTagSet = new HashSet<Tag>(component4.forbiddenTagSet);
		}
		MinionResume component5 = src_id.GetComponent<MinionResume>();
		dest_id.MasteryBySkillID = component5.MasteryBySkillID;
		dest_id.grantedSkillIDs = component5.GrantedSkillIDs;
		dest_id.AptitudeBySkillGroup = component5.AptitudeBySkillGroup;
		dest_id.TotalExperienceGained = component5.TotalExperienceGained;
		dest_id.currentHat = component5.CurrentHat;
		dest_id.targetHat = component5.TargetHat;
		ChoreConsumer component6 = src_id.GetComponent<ChoreConsumer>();
		dest_id.choreGroupPriorities = component6.GetChoreGroupPriorities();
		AttributeLevels component7 = src_id.GetComponent<AttributeLevels>();
		component7.OnSerializing();
		dest_id.attributeLevels = new List<AttributeLevels.LevelSaveLoad>(component7.SaveLoadLevels);
		Effects component8 = src_id.GetComponent<Effects>();
		dest_id.saveLoadEffects = component8.GetAllEffectsForSerialization();
		dest_id.saveLoadImmunities = component8.GetAllImmunitiesForSerialization();
		MinionStorage.StoreModifiers(src_id, dest_id);
		Schedulable component9 = src_id.GetComponent<Schedulable>();
		Schedule schedule = component9.GetSchedule();
		if (schedule != null)
		{
			schedule.Unassign(component9);
			Schedulable component10 = dest_id.GetComponent<Schedulable>();
			schedule.Assign(component10);
		}
		StoredMinionIdentity.IStoredMinionExtension[] components = src_id.GetComponents<StoredMinionIdentity.IStoredMinionExtension>();
		for (int i = 0; i < components.Length; i++)
		{
			components[i].PushTo(dest_id);
		}
	}

	// Token: 0x06007068 RID: 28776 RVA: 0x003053DC File Offset: 0x003035DC
	private static void StoreModifiers(MinionIdentity src_id, StoredMinionIdentity dest_id)
	{
		foreach (AttributeInstance attributeInstance in src_id.GetComponent<MinionModifiers>().attributes)
		{
			if (dest_id.minionModifiers.attributes.Get(attributeInstance.Attribute.Id) == null)
			{
				dest_id.minionModifiers.attributes.Add(attributeInstance.Attribute);
			}
			for (int i = 0; i < attributeInstance.Modifiers.Count; i++)
			{
				dest_id.minionModifiers.attributes.Get(attributeInstance.Id).Add(attributeInstance.Modifiers[i]);
			}
		}
	}

	// Token: 0x06007069 RID: 28777 RVA: 0x00305498 File Offset: 0x00303698
	private static void CopyMinion(StoredMinionIdentity src_id, MinionIdentity dest_id)
	{
		dest_id.Subscribe(1589886948, new Action<object>(MinionStorage.OnDeserializedMinionSpawned));
		dest_id.SetName(src_id.storedName);
		dest_id.nameStringKey = src_id.nameStringKey;
		dest_id.model = src_id.model;
		dest_id.personalityResourceId = src_id.personalityResourceId;
		dest_id.gender = src_id.gender;
		dest_id.genderStringKey = src_id.genderStringKey;
		dest_id.arrivalTime = src_id.arrivalTime;
		dest_id.voiceIdx = src_id.voiceIdx;
		dest_id.GetComponent<Accessorizer>().bodyData = src_id.bodyData;
		if (src_id.traitIDs != null)
		{
			dest_id.GetComponent<Traits>().SetTraitIds(src_id.traitIDs);
		}
		if (src_id.accessories != null)
		{
			dest_id.GetComponent<Accessorizer>().SetAccessories(src_id.accessories);
		}
		dest_id.GetComponent<WearableAccessorizer>().RestoreWearables(src_id.wearables, src_id.customClothingItems);
		ConsumableConsumer component = dest_id.GetComponent<ConsumableConsumer>();
		if (src_id.forbiddenTagSet != null)
		{
			component.forbiddenTagSet = new HashSet<Tag>(src_id.forbiddenTagSet);
		}
		if (src_id.MasteryBySkillID != null)
		{
			MinionResume component2 = dest_id.GetComponent<MinionResume>();
			component2.RestoreResume(src_id.MasteryBySkillID, src_id.AptitudeBySkillGroup, src_id.grantedSkillIDs, src_id.TotalExperienceGained);
			component2.SetHats(src_id.currentHat, src_id.targetHat);
		}
		if (src_id.choreGroupPriorities != null)
		{
			dest_id.GetComponent<ChoreConsumer>().SetChoreGroupPriorities(src_id.choreGroupPriorities);
		}
		AttributeLevels component3 = dest_id.GetComponent<AttributeLevels>();
		if (src_id.attributeLevels != null)
		{
			component3.SaveLoadLevels = src_id.attributeLevels.ToArray();
			component3.OnDeserialized();
		}
		Effects component4 = dest_id.GetComponent<Effects>();
		if (src_id.saveLoadImmunities != null)
		{
			foreach (Effects.SaveLoadImmunities saveLoadImmunities in src_id.saveLoadImmunities)
			{
				if (Db.Get().effects.Exists(saveLoadImmunities.effectID))
				{
					Effect effect = Db.Get().effects.Get(saveLoadImmunities.effectID);
					component4.AddImmunity(effect, saveLoadImmunities.giverID, saveLoadImmunities.saved);
				}
			}
		}
		if (src_id.saveLoadEffects != null)
		{
			foreach (Effects.SaveLoadEffect saveLoadEffect in src_id.saveLoadEffects)
			{
				if (Db.Get().effects.Exists(saveLoadEffect.id))
				{
					Effect newEffect = Db.Get().effects.Get(saveLoadEffect.id);
					EffectInstance effectInstance = component4.Add(newEffect, saveLoadEffect.saved);
					if (effectInstance != null)
					{
						effectInstance.timeRemaining = saveLoadEffect.timeRemaining;
					}
				}
			}
		}
		dest_id.GetComponent<Accessorizer>().ApplyAccessories();
		dest_id.assignableProxy = new Ref<MinionAssignablesProxy>();
		dest_id.assignableProxy.Set(src_id.assignableProxy.Get());
		dest_id.assignableProxy.Get().SetTarget(dest_id, dest_id.gameObject);
		Schedulable component5 = src_id.GetComponent<Schedulable>();
		Schedule schedule = component5.GetSchedule();
		if (schedule != null)
		{
			schedule.Unassign(component5);
			Schedulable component6 = dest_id.GetComponent<Schedulable>();
			schedule.Assign(component6);
		}
		StoredMinionIdentity.IStoredMinionExtension[] components = dest_id.GetComponents<StoredMinionIdentity.IStoredMinionExtension>();
		for (int i = 0; i < components.Length; i++)
		{
			components[i].PullFrom(src_id);
		}
	}

	// Token: 0x0600706A RID: 28778 RVA: 0x003057DC File Offset: 0x003039DC
	private static void OnDeserializedMinionSpawned(object deserializedMinionOBJ)
	{
		MinionIdentity component = ((GameObject)deserializedMinionOBJ).GetComponent<MinionIdentity>();
		Equipment equipment = component.GetEquipment();
		foreach (AssignableSlotInstance assignableSlotInstance in equipment.Slots)
		{
			Equippable equippable = assignableSlotInstance.assignable as Equippable;
			if (equippable != null)
			{
				equipment.Equip(equippable);
			}
		}
		component.Unsubscribe(1589886948, new Action<object>(MinionStorage.OnDeserializedMinionSpawned));
	}

	// Token: 0x0600706B RID: 28779 RVA: 0x0030586C File Offset: 0x00303A6C
	public static void RedirectInstanceTracker(GameObject src_minion, GameObject dest_minion)
	{
		KPrefabID component = src_minion.GetComponent<KPrefabID>();
		dest_minion.GetComponent<KPrefabID>().InstanceID = component.InstanceID;
		component.InstanceID = -1;
	}

	// Token: 0x0600706C RID: 28780 RVA: 0x00305898 File Offset: 0x00303A98
	public void SerializeMinion(GameObject minion)
	{
		this.CleanupBadReferences();
		KPrefabID kprefabID = this.CreateSerializedMinion(minion);
		MinionStorage.Info item = new MinionStorage.Info(kprefabID.GetComponent<StoredMinionIdentity>().storedName, new Ref<KPrefabID>(kprefabID));
		this.serializedMinions.Add(item);
	}

	// Token: 0x0600706D RID: 28781 RVA: 0x003058D8 File Offset: 0x00303AD8
	private void CleanupBadReferences()
	{
		for (int i = this.serializedMinions.Count - 1; i >= 0; i--)
		{
			if (this.serializedMinions[i].serializedMinion == null || this.serializedMinions[i].serializedMinion.Get() == null)
			{
				this.serializedMinions.RemoveAt(i);
			}
		}
	}

	// Token: 0x0600706E RID: 28782 RVA: 0x0030593C File Offset: 0x00303B3C
	private int GetMinionIndex(Guid id)
	{
		int result = -1;
		for (int i = 0; i < this.serializedMinions.Count; i++)
		{
			if (this.serializedMinions[i].id == id)
			{
				result = i;
				break;
			}
		}
		return result;
	}

	// Token: 0x0600706F RID: 28783 RVA: 0x00305980 File Offset: 0x00303B80
	public GameObject DeserializeMinion(Guid id, Vector3 pos)
	{
		int minionIndex = this.GetMinionIndex(id);
		if (minionIndex < 0 || minionIndex >= this.serializedMinions.Count)
		{
			return null;
		}
		KPrefabID kprefabID = this.serializedMinions[minionIndex].serializedMinion.Get();
		this.serializedMinions.RemoveAt(minionIndex);
		if (kprefabID == null)
		{
			return null;
		}
		return MinionStorage.DeserializeMinion(kprefabID.gameObject, pos);
	}

	// Token: 0x06007070 RID: 28784 RVA: 0x003059E4 File Offset: 0x00303BE4
	public static GameObject DeserializeMinion(GameObject sourceMinion, Vector3 pos)
	{
		StoredMinionIdentity component = sourceMinion.GetComponent<StoredMinionIdentity>();
		GameObject gameObject = Util.KInstantiate(SaveLoader.Instance.saveManager.GetPrefab(BaseMinionConfig.GetMinionIDForModel(component.model)), pos);
		MinionIdentity component2 = gameObject.GetComponent<MinionIdentity>();
		MinionStorage.RedirectInstanceTracker(sourceMinion, gameObject);
		gameObject.SetActive(true);
		MinionStorage.CopyMinion(component, component2);
		component.assignableProxy.Get().SetTarget(component2, gameObject);
		Util.KDestroyGameObject(sourceMinion);
		return gameObject;
	}

	// Token: 0x06007071 RID: 28785 RVA: 0x00305A54 File Offset: 0x00303C54
	public void DeleteStoredMinion(Guid id)
	{
		int minionIndex = this.GetMinionIndex(id);
		if (minionIndex < 0)
		{
			return;
		}
		if (this.serializedMinions[minionIndex].serializedMinion != null)
		{
			this.serializedMinions[minionIndex].serializedMinion.Get().GetComponent<StoredMinionIdentity>().OnHardDelete();
			Util.KDestroyGameObject(this.serializedMinions[minionIndex].serializedMinion.Get().gameObject);
		}
		this.serializedMinions.RemoveAt(minionIndex);
	}

	// Token: 0x06007072 RID: 28786 RVA: 0x000EE0A8 File Offset: 0x000EC2A8
	public List<MinionStorage.Info> GetStoredMinionInfo()
	{
		return this.serializedMinions;
	}

	// Token: 0x06007073 RID: 28787 RVA: 0x000EE0B0 File Offset: 0x000EC2B0
	public void SetStoredMinionInfo(List<MinionStorage.Info> info)
	{
		this.serializedMinions = info;
	}

	// Token: 0x04005486 RID: 21638
	[Serialize]
	private List<MinionStorage.Info> serializedMinions = new List<MinionStorage.Info>();

	// Token: 0x02001522 RID: 5410
	public struct Info
	{
		// Token: 0x06007075 RID: 28789 RVA: 0x000EE0CC File Offset: 0x000EC2CC
		public Info(string name, Ref<KPrefabID> ref_obj)
		{
			this.id = Guid.NewGuid();
			this.name = name;
			this.serializedMinion = ref_obj;
		}

		// Token: 0x06007076 RID: 28790 RVA: 0x00305AD0 File Offset: 0x00303CD0
		public static MinionStorage.Info CreateEmpty()
		{
			return new MinionStorage.Info
			{
				id = Guid.Empty,
				name = null,
				serializedMinion = null
			};
		}

		// Token: 0x04005487 RID: 21639
		public Guid id;

		// Token: 0x04005488 RID: 21640
		public string name;

		// Token: 0x04005489 RID: 21641
		public Ref<KPrefabID> serializedMinion;
	}
}
