using System;
using System.Collections.Generic;

// Token: 0x02001774 RID: 6004
public class QuestCriteria_GreaterThan : QuestCriteria
{
	// Token: 0x06007B93 RID: 31635 RVA: 0x000F5BA2 File Offset: 0x000F3DA2
	public QuestCriteria_GreaterThan(Tag id, float[] targetValues, int requiredCount = 1, HashSet<Tag> acceptedTags = null, QuestCriteria.BehaviorFlags flags = QuestCriteria.BehaviorFlags.TrackValues) : base(id, targetValues, requiredCount, acceptedTags, flags)
	{
	}

	// Token: 0x06007B94 RID: 31636 RVA: 0x000F5BC5 File Offset: 0x000F3DC5
	protected override bool ValueSatisfies_Internal(float current, float target)
	{
		return current > target;
	}
}
