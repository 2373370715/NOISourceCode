using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x020010AA RID: 4266
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/ChoreGroupManager")]
public class ChoreGroupManager : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x0600569B RID: 22171 RVA: 0x000DCE69 File Offset: 0x000DB069
	public static void DestroyInstance()
	{
		ChoreGroupManager.instance = null;
	}

	// Token: 0x170004FC RID: 1276
	// (get) Token: 0x0600569C RID: 22172 RVA: 0x000DCE71 File Offset: 0x000DB071
	public List<Tag> DefaultForbiddenTagsList
	{
		get
		{
			return this.defaultForbiddenTagsList;
		}
	}

	// Token: 0x170004FD RID: 1277
	// (get) Token: 0x0600569D RID: 22173 RVA: 0x000DCE79 File Offset: 0x000DB079
	public Dictionary<Tag, int> DefaultChorePermission
	{
		get
		{
			return this.defaultChorePermissions;
		}
	}

	// Token: 0x0600569E RID: 22174 RVA: 0x00290B84 File Offset: 0x0028ED84
	protected override void OnSpawn()
	{
		base.OnSpawn();
		ChoreGroupManager.instance = this;
		this.ConvertOldVersion();
		foreach (ChoreGroup choreGroup in Db.Get().ChoreGroups.resources)
		{
			if (!this.defaultChorePermissions.ContainsKey(choreGroup.Id.ToTag()))
			{
				this.defaultChorePermissions.Add(choreGroup.Id.ToTag(), 2);
			}
		}
	}

	// Token: 0x0600569F RID: 22175 RVA: 0x00290C1C File Offset: 0x0028EE1C
	private void ConvertOldVersion()
	{
		foreach (Tag key in this.defaultForbiddenTagsList)
		{
			if (!this.defaultChorePermissions.ContainsKey(key))
			{
				this.defaultChorePermissions.Add(key, -1);
			}
			this.defaultChorePermissions[key] = 0;
		}
		this.defaultForbiddenTagsList.Clear();
	}

	// Token: 0x04003D5C RID: 15708
	public static ChoreGroupManager instance;

	// Token: 0x04003D5D RID: 15709
	[Serialize]
	private List<Tag> defaultForbiddenTagsList = new List<Tag>();

	// Token: 0x04003D5E RID: 15710
	[Serialize]
	private Dictionary<Tag, int> defaultChorePermissions = new Dictionary<Tag, int>();
}
