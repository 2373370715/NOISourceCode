using System;
using System.Collections.Generic;

// Token: 0x02001776 RID: 6006
public class QuestCriteria_GreaterOrEqual : QuestCriteria
{
	// Token: 0x06007B97 RID: 31639 RVA: 0x000F5BA2 File Offset: 0x000F3DA2
	public QuestCriteria_GreaterOrEqual(Tag id, float[] targetValues, int requiredCount = 1, HashSet<Tag> acceptedTags = null, QuestCriteria.BehaviorFlags flags = QuestCriteria.BehaviorFlags.TrackValues) : base(id, targetValues, requiredCount, acceptedTags, flags)
	{
	}

	// Token: 0x06007B98 RID: 31640 RVA: 0x000F5BD1 File Offset: 0x000F3DD1
	protected override bool ValueSatisfies_Internal(float current, float target)
	{
		return current >= target;
	}
}
