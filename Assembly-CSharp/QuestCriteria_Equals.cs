using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001773 RID: 6003
public class QuestCriteria_Equals : QuestCriteria
{
	// Token: 0x06007B91 RID: 31633 RVA: 0x000F5BA2 File Offset: 0x000F3DA2
	public QuestCriteria_Equals(Tag id, float[] targetValues, int requiredCount = 1, HashSet<Tag> acceptedTags = null, QuestCriteria.BehaviorFlags flags = QuestCriteria.BehaviorFlags.TrackValues) : base(id, targetValues, requiredCount, acceptedTags, flags)
	{
	}

	// Token: 0x06007B92 RID: 31634 RVA: 0x000F5BB1 File Offset: 0x000F3DB1
	protected override bool ValueSatisfies_Internal(float current, float target)
	{
		return Mathf.Abs(target - current) <= Mathf.Epsilon;
	}
}
