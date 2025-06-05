using System;
using UnityEngine;

// Token: 0x02001897 RID: 6295
[AddComponentMenu("KMonoBehaviour/scripts/Schedulable")]
public class Schedulable : KMonoBehaviour
{
	// Token: 0x06008209 RID: 33289 RVA: 0x000FA0F8 File Offset: 0x000F82F8
	public Schedule GetSchedule()
	{
		return ScheduleManager.Instance.GetSchedule(this);
	}

	// Token: 0x0600820A RID: 33290 RVA: 0x00348FC0 File Offset: 0x003471C0
	public bool IsAllowed(ScheduleBlockType schedule_block_type)
	{
		WorldContainer myWorld = base.gameObject.GetMyWorld();
		if (myWorld == null)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				string.Format("Trying to schedule {0} but {1} is not on a valid world. Grid cell: {2}", schedule_block_type.Id, base.gameObject.name, Grid.PosToCell(base.gameObject.GetComponent<KPrefabID>()))
			});
			return false;
		}
		return myWorld.AlertManager.IsRedAlert() || ScheduleManager.Instance.IsAllowed(this, schedule_block_type);
	}

	// Token: 0x0600820B RID: 33291 RVA: 0x000FA105 File Offset: 0x000F8305
	public void OnScheduleChanged(Schedule schedule)
	{
		base.Trigger(467134493, schedule);
	}

	// Token: 0x0600820C RID: 33292 RVA: 0x000FA113 File Offset: 0x000F8313
	public void OnScheduleBlocksTick(Schedule schedule)
	{
		base.Trigger(1714332666, schedule);
	}

	// Token: 0x0600820D RID: 33293 RVA: 0x000FA121 File Offset: 0x000F8321
	public void OnScheduleBlocksChanged(Schedule schedule)
	{
		base.Trigger(-894023145, schedule);
	}
}
