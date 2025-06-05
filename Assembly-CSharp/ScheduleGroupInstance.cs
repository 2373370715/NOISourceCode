using System;
using KSerialization;

// Token: 0x02000BC8 RID: 3016
[SerializationConfig(MemberSerialization.OptIn)]
public class ScheduleGroupInstance
{
	// Token: 0x1700029B RID: 667
	// (get) Token: 0x0600391D RID: 14621 RVA: 0x000C96C5 File Offset: 0x000C78C5
	// (set) Token: 0x0600391E RID: 14622 RVA: 0x000C96DC File Offset: 0x000C78DC
	public ScheduleGroup scheduleGroup
	{
		get
		{
			return Db.Get().ScheduleGroups.Get(this.scheduleGroupID);
		}
		set
		{
			this.scheduleGroupID = value.Id;
		}
	}

	// Token: 0x0600391F RID: 14623 RVA: 0x000C96EA File Offset: 0x000C78EA
	public ScheduleGroupInstance(ScheduleGroup scheduleGroup)
	{
		this.scheduleGroup = scheduleGroup;
		this.segments = scheduleGroup.defaultSegments;
	}

	// Token: 0x0400278A RID: 10122
	[Serialize]
	private string scheduleGroupID;

	// Token: 0x0400278B RID: 10123
	[Serialize]
	public int segments;
}
