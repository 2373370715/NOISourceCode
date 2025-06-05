using System;
using System.Collections.Generic;

// Token: 0x02001777 RID: 6007
public class QuestCriteria_LessOrEqual : QuestCriteria
{
	// Token: 0x06007B99 RID: 31641 RVA: 0x000F5BA2 File Offset: 0x000F3DA2
	public QuestCriteria_LessOrEqual(Tag id, float[] targetValues, int requiredCount = 1, HashSet<Tag> acceptedTags = null, QuestCriteria.BehaviorFlags flags = QuestCriteria.BehaviorFlags.TrackValues) : base(id, targetValues, requiredCount, acceptedTags, flags)
	{
	}

	// Token: 0x06007B9A RID: 31642 RVA: 0x000F5BDA File Offset: 0x000F3DDA
	protected override bool ValueSatisfies_Internal(float current, float target)
	{
		return current <= target;
	}
}
