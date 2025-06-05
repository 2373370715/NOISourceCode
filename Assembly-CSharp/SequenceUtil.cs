using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200060C RID: 1548
public static class SequenceUtil
{
	// Token: 0x1700009F RID: 159
	// (get) Token: 0x06001B5B RID: 7003 RVA: 0x000AA765 File Offset: 0x000A8965
	public static YieldInstruction WaitForNextFrame
	{
		get
		{
			return null;
		}
	}

	// Token: 0x170000A0 RID: 160
	// (get) Token: 0x06001B5C RID: 7004 RVA: 0x000B63D0 File Offset: 0x000B45D0
	public static YieldInstruction WaitForEndOfFrame
	{
		get
		{
			if (SequenceUtil.waitForEndOfFrame == null)
			{
				SequenceUtil.waitForEndOfFrame = new WaitForEndOfFrame();
			}
			return SequenceUtil.waitForEndOfFrame;
		}
	}

	// Token: 0x170000A1 RID: 161
	// (get) Token: 0x06001B5D RID: 7005 RVA: 0x000B63E8 File Offset: 0x000B45E8
	public static YieldInstruction WaitForFixedUpdate
	{
		get
		{
			if (SequenceUtil.waitForFixedUpdate == null)
			{
				SequenceUtil.waitForFixedUpdate = new WaitForFixedUpdate();
			}
			return SequenceUtil.waitForFixedUpdate;
		}
	}

	// Token: 0x06001B5E RID: 7006 RVA: 0x001B7328 File Offset: 0x001B5528
	public static YieldInstruction WaitForSeconds(float duration)
	{
		WaitForSeconds result;
		if (!SequenceUtil.scaledTimeCache.TryGetValue(duration, out result))
		{
			result = (SequenceUtil.scaledTimeCache[duration] = new WaitForSeconds(duration));
		}
		return result;
	}

	// Token: 0x06001B5F RID: 7007 RVA: 0x001B735C File Offset: 0x001B555C
	public static WaitForSecondsRealtime WaitForSecondsRealtime(float duration)
	{
		WaitForSecondsRealtime result;
		if (!SequenceUtil.reailTimeWaitCache.TryGetValue(duration, out result))
		{
			result = (SequenceUtil.reailTimeWaitCache[duration] = new WaitForSecondsRealtime(duration));
		}
		return result;
	}

	// Token: 0x0400118A RID: 4490
	private static WaitForEndOfFrame waitForEndOfFrame = null;

	// Token: 0x0400118B RID: 4491
	private static WaitForFixedUpdate waitForFixedUpdate = null;

	// Token: 0x0400118C RID: 4492
	private static Dictionary<float, WaitForSeconds> scaledTimeCache = new Dictionary<float, WaitForSeconds>();

	// Token: 0x0400118D RID: 4493
	private static Dictionary<float, WaitForSecondsRealtime> reailTimeWaitCache = new Dictionary<float, WaitForSecondsRealtime>();
}
