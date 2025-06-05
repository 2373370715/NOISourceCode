using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using UnityEngine;

// Token: 0x020007A2 RID: 1954
[AddComponentMenu("KMonoBehaviour/scripts/ConsumableConsumer")]
public class ConsumableConsumer : KMonoBehaviour
{
	// Token: 0x060022AA RID: 8874 RVA: 0x001D05D8 File Offset: 0x001CE7D8
	[OnDeserialized]
	[Obsolete]
	private void OnDeserialized()
	{
		if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 29))
		{
			this.forbiddenTagSet = new HashSet<Tag>(this.forbiddenTags);
			this.forbiddenTags = null;
		}
	}

	// Token: 0x060022AB RID: 8875 RVA: 0x001D0614 File Offset: 0x001CE814
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (ConsumerManager.instance != null)
		{
			this.forbiddenTagSet = new HashSet<Tag>(ConsumerManager.instance.DefaultForbiddenTagsList);
			this.SetModelDietaryRestrictions();
			return;
		}
		this.forbiddenTagSet = new HashSet<Tag>();
		this.dietaryRestrictionTagSet = new HashSet<Tag>();
	}

	// Token: 0x060022AC RID: 8876 RVA: 0x000BB014 File Offset: 0x000B9214
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.SetModelDietaryRestrictions();
	}

	// Token: 0x060022AD RID: 8877 RVA: 0x001D0668 File Offset: 0x001CE868
	private void SetModelDietaryRestrictions()
	{
		if (this.HasTag(GameTags.Minions.Models.Standard))
		{
			this.dietaryRestrictionTagSet = new HashSet<Tag>(ConsumerManager.instance.StandardDuplicantDietaryRestrictions);
			return;
		}
		if (this.HasTag(GameTags.Minions.Models.Bionic))
		{
			this.dietaryRestrictionTagSet = new HashSet<Tag>(ConsumerManager.instance.BionicDuplicantDietaryRestrictions);
		}
	}

	// Token: 0x060022AE RID: 8878 RVA: 0x001D06BC File Offset: 0x001CE8BC
	public bool IsPermitted(string consumable_id)
	{
		Tag item = new Tag(consumable_id);
		return !this.forbiddenTagSet.Contains(item) && !this.dietaryRestrictionTagSet.Contains(item);
	}

	// Token: 0x060022AF RID: 8879 RVA: 0x001D06F0 File Offset: 0x001CE8F0
	public bool IsDietRestricted(string consumable_id)
	{
		Tag item = new Tag(consumable_id);
		return this.dietaryRestrictionTagSet.Contains(item);
	}

	// Token: 0x060022B0 RID: 8880 RVA: 0x001D0714 File Offset: 0x001CE914
	public void SetPermitted(string consumable_id, bool is_allowed)
	{
		Tag item = new Tag(consumable_id);
		is_allowed = (is_allowed && !this.dietaryRestrictionTagSet.Contains(consumable_id));
		if (is_allowed)
		{
			this.forbiddenTagSet.Remove(item);
		}
		else
		{
			this.forbiddenTagSet.Add(item);
		}
		this.consumableRulesChanged.Signal();
	}

	// Token: 0x0400173E RID: 5950
	[Obsolete("Deprecated, use forbiddenTagSet")]
	[Serialize]
	[HideInInspector]
	public Tag[] forbiddenTags;

	// Token: 0x0400173F RID: 5951
	[Serialize]
	public HashSet<Tag> forbiddenTagSet;

	// Token: 0x04001740 RID: 5952
	public HashSet<Tag> dietaryRestrictionTagSet;

	// Token: 0x04001741 RID: 5953
	public System.Action consumableRulesChanged;
}
