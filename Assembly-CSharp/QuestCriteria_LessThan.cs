using System;
using System.Collections.Generic;

// Token: 0x02001775 RID: 6005
public class QuestCriteria_LessThan : QuestCriteria
{
	// Token: 0x06007B95 RID: 31637 RVA: 0x000F5BA2 File Offset: 0x000F3DA2
	public QuestCriteria_LessThan(Tag id, float[] targetValues, int requiredCount = 1, HashSet<Tag> acceptedTags = null, QuestCriteria.BehaviorFlags flags = QuestCriteria.BehaviorFlags.TrackValues) : base(id, targetValues, requiredCount, acceptedTags, flags)
	{
	}

	// Token: 0x06007B96 RID: 31638 RVA: 0x000F5BCB File Offset: 0x000F3DCB
	protected override bool ValueSatisfies_Internal(float current, float target)
	{
		return current < target;
	}
}
