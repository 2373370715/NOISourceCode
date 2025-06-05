using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Database;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000B59 RID: 2905
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/StoredMinionIdentity")]
public class StoredMinionIdentity : KMonoBehaviour, ISaveLoadable, IAssignableIdentity, IListableOption, IPersonalPriorityManager
{
	// Token: 0x1700025C RID: 604
	// (get) Token: 0x0600367D RID: 13949 RVA: 0x000C7E8A File Offset: 0x000C608A
	// (set) Token: 0x0600367E RID: 13950 RVA: 0x000C7E92 File Offset: 0x000C6092
	[Serialize]
	public string genderStringKey { get; set; }

	// Token: 0x1700025D RID: 605
	// (get) Token: 0x0600367F RID: 13951 RVA: 0x000C7E9B File Offset: 0x000C609B
	// (set) Token: 0x06003680 RID: 13952 RVA: 0x000C7EA3 File Offset: 0x000C60A3
	[Serialize]
	public string nameStringKey { get; set; }

	// Token: 0x1700025E RID: 606
	// (get) Token: 0x06003681 RID: 13953 RVA: 0x000C7EAC File Offset: 0x000C60AC
	// (set) Token: 0x06003682 RID: 13954 RVA: 0x000C7EB4 File Offset: 0x000C60B4
	[Serialize]
	public HashedString personalityResourceId { get; set; }

	// Token: 0x06003683 RID: 13955 RVA: 0x0022129C File Offset: 0x0021F49C
	[OnDeserialized]
	[Obsolete]
	private void OnDeserializedMethod()
	{
		if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 7))
		{
			int num = 0;
			foreach (KeyValuePair<string, bool> keyValuePair in this.MasteryByRoleID)
			{
				if (keyValuePair.Value && keyValuePair.Key != "NoRole")
				{
					num++;
				}
			}
			this.TotalExperienceGained = MinionResume.CalculatePreviousExperienceBar(num);
			foreach (KeyValuePair<HashedString, float> keyValuePair2 in this.AptitudeByRoleGroup)
			{
				this.AptitudeBySkillGroup[keyValuePair2.Key] = keyValuePair2.Value;
			}
		}
		if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 29))
		{
			this.forbiddenTagSet = new HashSet<Tag>(this.forbiddenTags);
			this.forbiddenTags = null;
		}
		if (!this.model.IsValid)
		{
			this.model = MinionConfig.MODEL;
		}
		if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 30))
		{
			this.bodyData = Accessorizer.UpdateAccessorySlots(this.nameStringKey, ref this.accessories);
		}
		if (this.clothingItems.Count > 0)
		{
			this.customClothingItems[ClothingOutfitUtility.OutfitType.Clothing] = new List<ResourceRef<ClothingItemResource>>(this.clothingItems);
			this.clothingItems.Clear();
		}
		List<ResourceRef<Accessory>> list = this.accessories.FindAll((ResourceRef<Accessory> acc) => acc.Get() == null);
		if (list.Count > 0)
		{
			List<ClothingItemResource> list2 = new List<ClothingItemResource>();
			foreach (ResourceRef<Accessory> resourceRef in list)
			{
				ClothingItemResource clothingItemResource = Db.Get().Permits.ClothingItems.TryResolveAccessoryResource(resourceRef.Guid);
				if (clothingItemResource != null && !list2.Contains(clothingItemResource))
				{
					list2.Add(clothingItemResource);
					this.customClothingItems[ClothingOutfitUtility.OutfitType.Clothing].Add(new ResourceRef<ClothingItemResource>(clothingItemResource));
				}
			}
			this.bodyData = Accessorizer.UpdateAccessorySlots(this.nameStringKey, ref this.accessories);
		}
		this.OnDeserializeModifiers();
	}

	// Token: 0x06003684 RID: 13956 RVA: 0x0022150C File Offset: 0x0021F70C
	public bool HasPerk(SkillPerk perk)
	{
		foreach (KeyValuePair<string, bool> keyValuePair in this.MasteryBySkillID)
		{
			if (keyValuePair.Value && Db.Get().Skills.Get(keyValuePair.Key).perks.Contains(perk))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06003685 RID: 13957 RVA: 0x000C7EBD File Offset: 0x000C60BD
	public bool HasMasteredSkill(string skillId)
	{
		return this.MasteryBySkillID.ContainsKey(skillId) && this.MasteryBySkillID[skillId];
	}

	// Token: 0x06003686 RID: 13958 RVA: 0x000C7EDB File Offset: 0x000C60DB
	protected override void OnPrefabInit()
	{
		this.assignableProxy = new Ref<MinionAssignablesProxy>();
		this.minionModifiers = base.GetComponent<MinionModifiers>();
		this.savedAttributeValues = new Dictionary<string, float>();
	}

	// Token: 0x06003687 RID: 13959 RVA: 0x0022158C File Offset: 0x0021F78C
	[OnSerializing]
	private void OnSerialize()
	{
		this.savedAttributeValues.Clear();
		foreach (AttributeInstance attributeInstance in this.minionModifiers.attributes)
		{
			this.savedAttributeValues.Add(attributeInstance.Attribute.Id, attributeInstance.GetTotalValue());
		}
	}

	// Token: 0x06003688 RID: 13960 RVA: 0x00221600 File Offset: 0x0021F800
	protected override void OnSpawn()
	{
		string[] attributes = MinionConfig.GetAttributes();
		string[] amounts = MinionConfig.GetAmounts();
		AttributeModifier[] traits = MinionConfig.GetTraits();
		if (this.model == BionicMinionConfig.MODEL)
		{
			attributes = BionicMinionConfig.GetAttributes();
			amounts = BionicMinionConfig.GetAmounts();
			traits = BionicMinionConfig.GetTraits();
		}
		BaseMinionConfig.AddMinionAttributes(this.minionModifiers, attributes);
		BaseMinionConfig.AddMinionAmounts(this.minionModifiers, amounts);
		BaseMinionConfig.AddMinionTraits(BaseMinionConfig.GetMinionNameForModel(this.model), BaseMinionConfig.GetMinionBaseTraitIDForModel(this.model), this.minionModifiers, traits);
		this.ValidateProxy();
		this.CleanupLimboMinions();
	}

	// Token: 0x06003689 RID: 13961 RVA: 0x000C7EFF File Offset: 0x000C60FF
	public void OnHardDelete()
	{
		if (this.assignableProxy.Get() != null)
		{
			Util.KDestroyGameObject(this.assignableProxy.Get().gameObject);
		}
		ScheduleManager.Instance.OnStoredDupeDestroyed(this);
		Components.StoredMinionIdentities.Remove(this);
	}

	// Token: 0x0600368A RID: 13962 RVA: 0x0022168C File Offset: 0x0021F88C
	private void OnDeserializeModifiers()
	{
		foreach (KeyValuePair<string, float> keyValuePair in this.savedAttributeValues)
		{
			Klei.AI.Attribute attribute = Db.Get().Attributes.TryGet(keyValuePair.Key);
			if (attribute == null)
			{
				attribute = Db.Get().BuildingAttributes.TryGet(keyValuePair.Key);
			}
			if (attribute != null)
			{
				if (this.minionModifiers.attributes.Get(attribute.Id) != null)
				{
					this.minionModifiers.attributes.Get(attribute.Id).Modifiers.Clear();
					this.minionModifiers.attributes.Get(attribute.Id).ClearModifiers();
				}
				else
				{
					this.minionModifiers.attributes.Add(attribute);
				}
				this.minionModifiers.attributes.Add(new AttributeModifier(attribute.Id, keyValuePair.Value, () => DUPLICANTS.ATTRIBUTES.STORED_VALUE, false, false));
			}
		}
	}

	// Token: 0x0600368B RID: 13963 RVA: 0x000C7F3F File Offset: 0x000C613F
	public void ValidateProxy()
	{
		this.assignableProxy = MinionAssignablesProxy.InitAssignableProxy(this.assignableProxy, this);
	}

	// Token: 0x0600368C RID: 13964 RVA: 0x002217C0 File Offset: 0x0021F9C0
	public string[] GetClothingItemIds(ClothingOutfitUtility.OutfitType outfitType)
	{
		if (this.customClothingItems.ContainsKey(outfitType))
		{
			string[] array = new string[this.customClothingItems[outfitType].Count];
			for (int i = 0; i < this.customClothingItems[outfitType].Count; i++)
			{
				array[i] = this.customClothingItems[outfitType][i].Get().Id;
			}
			return array;
		}
		return null;
	}

	// Token: 0x0600368D RID: 13965 RVA: 0x00221830 File Offset: 0x0021FA30
	private void CleanupLimboMinions()
	{
		KPrefabID component = base.GetComponent<KPrefabID>();
		bool flag = false;
		if (component.InstanceID == -1)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"Stored minion with an invalid kpid! Attempting to recover...",
				this.storedName
			});
			flag = true;
			if (KPrefabIDTracker.Get().GetInstance(component.InstanceID) != null)
			{
				KPrefabIDTracker.Get().Unregister(component);
			}
			component.InstanceID = KPrefabID.GetUniqueID();
			KPrefabIDTracker.Get().Register(component);
			DebugUtil.LogWarningArgs(new object[]
			{
				"Restored as:",
				component.InstanceID
			});
		}
		if (component.conflicted)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"Minion with a conflicted kpid! Attempting to recover... ",
				component.InstanceID,
				this.storedName
			});
			if (KPrefabIDTracker.Get().GetInstance(component.InstanceID) != null)
			{
				KPrefabIDTracker.Get().Unregister(component);
			}
			component.InstanceID = KPrefabID.GetUniqueID();
			KPrefabIDTracker.Get().Register(component);
			DebugUtil.LogWarningArgs(new object[]
			{
				"Restored as:",
				component.InstanceID
			});
		}
		this.assignableProxy.Get().SetTarget(this, base.gameObject);
		bool flag2 = false;
		foreach (MinionStorage minionStorage in Components.MinionStorages.Items)
		{
			List<MinionStorage.Info> storedMinionInfo = minionStorage.GetStoredMinionInfo();
			for (int i = 0; i < storedMinionInfo.Count; i++)
			{
				MinionStorage.Info info = storedMinionInfo[i];
				if (flag && info.serializedMinion != null && info.serializedMinion.GetId() == -1 && info.name == this.storedName)
				{
					DebugUtil.LogWarningArgs(new object[]
					{
						"Found a minion storage with an invalid ref, rebinding.",
						component.InstanceID,
						this.storedName,
						minionStorage.gameObject.name
					});
					info = new MinionStorage.Info(this.storedName, new Ref<KPrefabID>(component));
					storedMinionInfo[i] = info;
					minionStorage.GetComponent<Assignable>().Assign(this);
					flag2 = true;
					break;
				}
				if (info.serializedMinion != null && info.serializedMinion.Get() == component)
				{
					flag2 = true;
					break;
				}
			}
			if (flag2)
			{
				break;
			}
		}
		if (!flag2)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"Found a stored minion that wasn't in any minion storage. Respawning them at the portal.",
				component.InstanceID,
				this.storedName
			});
			GameObject activeTelepad = GameUtil.GetActiveTelepad();
			if (activeTelepad != null)
			{
				MinionStorage.DeserializeMinion(component.gameObject, activeTelepad.transform.GetPosition());
			}
		}
	}

	// Token: 0x0600368E RID: 13966 RVA: 0x000C7F53 File Offset: 0x000C6153
	public string GetProperName()
	{
		return this.storedName;
	}

	// Token: 0x0600368F RID: 13967 RVA: 0x000C7F5B File Offset: 0x000C615B
	public List<Ownables> GetOwners()
	{
		return this.assignableProxy.Get().ownables;
	}

	// Token: 0x06003690 RID: 13968 RVA: 0x000C7F6D File Offset: 0x000C616D
	public Ownables GetSoleOwner()
	{
		return this.assignableProxy.Get().GetComponent<Ownables>();
	}

	// Token: 0x06003691 RID: 13969 RVA: 0x000C7F7F File Offset: 0x000C617F
	public bool HasOwner(Assignables owner)
	{
		return this.GetOwners().Contains(owner as Ownables);
	}

	// Token: 0x06003692 RID: 13970 RVA: 0x000C7F92 File Offset: 0x000C6192
	public int NumOwners()
	{
		return this.GetOwners().Count;
	}

	// Token: 0x06003693 RID: 13971 RVA: 0x00221B10 File Offset: 0x0021FD10
	public Accessory GetAccessory(AccessorySlot slot)
	{
		for (int i = 0; i < this.accessories.Count; i++)
		{
			if (this.accessories[i].Get() != null && this.accessories[i].Get().slot == slot)
			{
				return this.accessories[i].Get();
			}
		}
		return null;
	}

	// Token: 0x06003694 RID: 13972 RVA: 0x000C530F File Offset: 0x000C350F
	public bool IsNull()
	{
		return this == null;
	}

	// Token: 0x06003695 RID: 13973 RVA: 0x00221B74 File Offset: 0x0021FD74
	public string GetStorageReason()
	{
		KPrefabID component = base.GetComponent<KPrefabID>();
		foreach (MinionStorage minionStorage in Components.MinionStorages.Items)
		{
			using (List<MinionStorage.Info>.Enumerator enumerator2 = minionStorage.GetStoredMinionInfo().GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.Current.serializedMinion.Get() == component)
					{
						return minionStorage.GetProperName();
					}
				}
			}
		}
		return "";
	}

	// Token: 0x06003696 RID: 13974 RVA: 0x000C7F9F File Offset: 0x000C619F
	public bool IsPermittedToConsume(string consumable)
	{
		return !this.forbiddenTagSet.Contains(consumable);
	}

	// Token: 0x06003697 RID: 13975 RVA: 0x00221C2C File Offset: 0x0021FE2C
	public bool IsChoreGroupDisabled(ChoreGroup chore_group)
	{
		foreach (string id in this.traitIDs)
		{
			if (Db.Get().traits.Exists(id))
			{
				Trait trait = Db.Get().traits.Get(id);
				if (trait.disabledChoreGroups != null)
				{
					ChoreGroup[] disabledChoreGroups = trait.disabledChoreGroups;
					for (int i = 0; i < disabledChoreGroups.Length; i++)
					{
						if (disabledChoreGroups[i].IdHash == chore_group.IdHash)
						{
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	// Token: 0x06003698 RID: 13976 RVA: 0x00221CDC File Offset: 0x0021FEDC
	public int GetPersonalPriority(ChoreGroup chore_group)
	{
		ChoreConsumer.PriorityInfo priorityInfo;
		if (this.choreGroupPriorities.TryGetValue(chore_group.IdHash, out priorityInfo))
		{
			return priorityInfo.priority;
		}
		return 0;
	}

	// Token: 0x06003699 RID: 13977 RVA: 0x000B1628 File Offset: 0x000AF828
	public int GetAssociatedSkillLevel(ChoreGroup group)
	{
		return 0;
	}

	// Token: 0x0600369A RID: 13978 RVA: 0x000AA038 File Offset: 0x000A8238
	public void SetPersonalPriority(ChoreGroup group, int value)
	{
	}

	// Token: 0x0600369B RID: 13979 RVA: 0x000AA038 File Offset: 0x000A8238
	public void ResetPersonalPriorities()
	{
	}

	// Token: 0x040025A8 RID: 9640
	[Serialize]
	public string storedName;

	// Token: 0x040025A9 RID: 9641
	[Serialize]
	public Tag model;

	// Token: 0x040025AA RID: 9642
	[Serialize]
	public string gender;

	// Token: 0x040025AE RID: 9646
	[Serialize]
	[ReadOnly]
	public float arrivalTime;

	// Token: 0x040025AF RID: 9647
	[Serialize]
	public int voiceIdx;

	// Token: 0x040025B0 RID: 9648
	[Serialize]
	public KCompBuilder.BodyData bodyData;

	// Token: 0x040025B1 RID: 9649
	[Serialize]
	public List<Ref<KPrefabID>> assignedItems;

	// Token: 0x040025B2 RID: 9650
	[Serialize]
	public List<Ref<KPrefabID>> equippedItems;

	// Token: 0x040025B3 RID: 9651
	[Serialize]
	public List<string> traitIDs;

	// Token: 0x040025B4 RID: 9652
	[Serialize]
	public List<ResourceRef<Accessory>> accessories;

	// Token: 0x040025B5 RID: 9653
	[Obsolete("Deprecated, use customClothingItems")]
	[Serialize]
	public List<ResourceRef<ClothingItemResource>> clothingItems = new List<ResourceRef<ClothingItemResource>>();

	// Token: 0x040025B6 RID: 9654
	[Serialize]
	public Dictionary<ClothingOutfitUtility.OutfitType, List<ResourceRef<ClothingItemResource>>> customClothingItems = new Dictionary<ClothingOutfitUtility.OutfitType, List<ResourceRef<ClothingItemResource>>>();

	// Token: 0x040025B7 RID: 9655
	[Serialize]
	public Dictionary<WearableAccessorizer.WearableType, WearableAccessorizer.Wearable> wearables = new Dictionary<WearableAccessorizer.WearableType, WearableAccessorizer.Wearable>();

	// Token: 0x040025B8 RID: 9656
	[Obsolete("Deprecated, use forbiddenTagSet")]
	[Serialize]
	public List<Tag> forbiddenTags;

	// Token: 0x040025B9 RID: 9657
	[Serialize]
	public HashSet<Tag> forbiddenTagSet;

	// Token: 0x040025BA RID: 9658
	[Serialize]
	public Ref<MinionAssignablesProxy> assignableProxy;

	// Token: 0x040025BB RID: 9659
	[Serialize]
	public List<Effects.SaveLoadEffect> saveLoadEffects;

	// Token: 0x040025BC RID: 9660
	[Serialize]
	public List<Effects.SaveLoadImmunities> saveLoadImmunities;

	// Token: 0x040025BD RID: 9661
	[Serialize]
	public Dictionary<string, bool> MasteryByRoleID = new Dictionary<string, bool>();

	// Token: 0x040025BE RID: 9662
	[Serialize]
	public Dictionary<string, bool> MasteryBySkillID = new Dictionary<string, bool>();

	// Token: 0x040025BF RID: 9663
	[Serialize]
	public List<string> grantedSkillIDs = new List<string>();

	// Token: 0x040025C0 RID: 9664
	[Serialize]
	public Dictionary<HashedString, float> AptitudeByRoleGroup = new Dictionary<HashedString, float>();

	// Token: 0x040025C1 RID: 9665
	[Serialize]
	public Dictionary<HashedString, float> AptitudeBySkillGroup = new Dictionary<HashedString, float>();

	// Token: 0x040025C2 RID: 9666
	[Serialize]
	public float TotalExperienceGained;

	// Token: 0x040025C3 RID: 9667
	[Serialize]
	public string currentHat;

	// Token: 0x040025C4 RID: 9668
	[Serialize]
	public string targetHat;

	// Token: 0x040025C5 RID: 9669
	[Serialize]
	public Dictionary<HashedString, ChoreConsumer.PriorityInfo> choreGroupPriorities = new Dictionary<HashedString, ChoreConsumer.PriorityInfo>();

	// Token: 0x040025C6 RID: 9670
	[Serialize]
	public List<AttributeLevels.LevelSaveLoad> attributeLevels;

	// Token: 0x040025C7 RID: 9671
	[Serialize]
	public Dictionary<string, float> savedAttributeValues;

	// Token: 0x040025C8 RID: 9672
	public MinionModifiers minionModifiers;

	// Token: 0x02000B5A RID: 2906
	public interface IStoredMinionExtension
	{
		// Token: 0x0600369D RID: 13981
		void PushTo(StoredMinionIdentity destination);

		// Token: 0x0600369E RID: 13982
		void PullFrom(StoredMinionIdentity source);

		// Token: 0x0600369F RID: 13983
		void AddStoredMinionGameObjectRequirements(GameObject storedMinionGameObject);
	}
}
