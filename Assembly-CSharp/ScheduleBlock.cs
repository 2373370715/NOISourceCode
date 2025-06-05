using System;
using System.Collections.Generic;
using KSerialization;

// Token: 0x02001898 RID: 6296
[Serializable]
public class ScheduleBlock
{
	// Token: 0x17000849 RID: 2121
	// (get) Token: 0x0600820F RID: 33295 RVA: 0x000FA12F File Offset: 0x000F832F
	public List<ScheduleBlockType> allowed_types
	{
		get
		{
			Debug.Assert(!string.IsNullOrEmpty(this._groupId));
			return Db.Get().ScheduleGroups.Get(this._groupId).allowedTypes;
		}
	}

	// Token: 0x1700084A RID: 2122
	// (get) Token: 0x06008211 RID: 33297 RVA: 0x000FA167 File Offset: 0x000F8367
	// (set) Token: 0x06008210 RID: 33296 RVA: 0x000FA15E File Offset: 0x000F835E
	public string GroupId
	{
		get
		{
			return this._groupId;
		}
		set
		{
			this._groupId = value;
		}
	}

	// Token: 0x06008212 RID: 33298 RVA: 0x000FA16F File Offset: 0x000F836F
	public ScheduleBlock(string name, string groupId)
	{
		this.name = name;
		this._groupId = groupId;
	}

	// Token: 0x06008213 RID: 33299 RVA: 0x00349040 File Offset: 0x00347240
	public bool IsAllowed(ScheduleBlockType type)
	{
		if (this.allowed_types != null)
		{
			foreach (ScheduleBlockType scheduleBlockType in this.allowed_types)
			{
				if (type.IdHash == scheduleBlockType.IdHash)
				{
					return true;
				}
			}
			return false;
		}
		return false;
	}

	// Token: 0x040062F8 RID: 25336
	[Serialize]
	public string name;

	// Token: 0x040062F9 RID: 25337
	[Serialize]
	private string _groupId;
}
