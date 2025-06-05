using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001771 RID: 6001
public class QuestCriteria
{
	// Token: 0x170007BB RID: 1979
	// (get) Token: 0x06007B85 RID: 31621 RVA: 0x000F5B22 File Offset: 0x000F3D22
	// (set) Token: 0x06007B86 RID: 31622 RVA: 0x000F5B2A File Offset: 0x000F3D2A
	public string Text { get; private set; }

	// Token: 0x170007BC RID: 1980
	// (get) Token: 0x06007B87 RID: 31623 RVA: 0x000F5B33 File Offset: 0x000F3D33
	// (set) Token: 0x06007B88 RID: 31624 RVA: 0x000F5B3B File Offset: 0x000F3D3B
	public string Tooltip { get; private set; }

	// Token: 0x06007B89 RID: 31625 RVA: 0x0032B200 File Offset: 0x00329400
	public QuestCriteria(Tag id, float[] targetValues = null, int requiredCount = 1, HashSet<Tag> acceptedTags = null, QuestCriteria.BehaviorFlags flags = QuestCriteria.BehaviorFlags.None)
	{
		global::Debug.Assert(targetValues == null || (targetValues.Length != 0 && targetValues.Length <= 32));
		this.CriteriaId = id;
		this.EvaluationBehaviors = flags;
		this.TargetValues = targetValues;
		this.AcceptedTags = acceptedTags;
		this.RequiredCount = requiredCount;
	}

	// Token: 0x06007B8A RID: 31626 RVA: 0x0032B25C File Offset: 0x0032945C
	public bool ValueSatisfies(float value, int valueHandle)
	{
		if (float.IsNaN(value))
		{
			return false;
		}
		float target = (this.TargetValues == null) ? 0f : this.TargetValues[valueHandle];
		return this.ValueSatisfies_Internal(value, target);
	}

	// Token: 0x06007B8B RID: 31627 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	protected virtual bool ValueSatisfies_Internal(float current, float target)
	{
		return true;
	}

	// Token: 0x06007B8C RID: 31628 RVA: 0x000F5B44 File Offset: 0x000F3D44
	public bool IsSatisfied(uint satisfactionState, uint satisfactionMask)
	{
		return (satisfactionState & satisfactionMask) == satisfactionMask;
	}

	// Token: 0x06007B8D RID: 31629 RVA: 0x0032B294 File Offset: 0x00329494
	public void PopulateStrings(string prefix)
	{
		string str = this.CriteriaId.Name.ToUpperInvariant();
		StringEntry stringEntry;
		if (Strings.TryGet(prefix + "CRITERIA." + str + ".NAME", out stringEntry))
		{
			this.Text = stringEntry.String;
		}
		if (Strings.TryGet(prefix + "CRITERIA." + str + ".TOOLTIP", out stringEntry))
		{
			this.Tooltip = stringEntry.String;
		}
	}

	// Token: 0x06007B8E RID: 31630 RVA: 0x000F5B4C File Offset: 0x000F3D4C
	public uint GetSatisfactionMask()
	{
		if (this.TargetValues == null)
		{
			return 1U;
		}
		return (uint)Mathf.Pow(2f, (float)(this.TargetValues.Length - 1));
	}

	// Token: 0x06007B8F RID: 31631 RVA: 0x000F5B6E File Offset: 0x000F3D6E
	public uint GetValueMask(int valueHandle)
	{
		if (this.TargetValues == null)
		{
			return 1U;
		}
		if (!QuestCriteria.HasBehavior(this.EvaluationBehaviors, QuestCriteria.BehaviorFlags.TrackArea))
		{
			valueHandle %= this.TargetValues.Length;
		}
		return 1U << valueHandle;
	}

	// Token: 0x06007B90 RID: 31632 RVA: 0x000F5B9A File Offset: 0x000F3D9A
	public static bool HasBehavior(QuestCriteria.BehaviorFlags flags, QuestCriteria.BehaviorFlags behavior)
	{
		return (flags & behavior) == behavior;
	}

	// Token: 0x04005D1B RID: 23835
	public const int MAX_VALUES = 32;

	// Token: 0x04005D1C RID: 23836
	public const int INVALID_VALUE = -1;

	// Token: 0x04005D1D RID: 23837
	public readonly Tag CriteriaId;

	// Token: 0x04005D1E RID: 23838
	public readonly QuestCriteria.BehaviorFlags EvaluationBehaviors;

	// Token: 0x04005D1F RID: 23839
	public readonly float[] TargetValues;

	// Token: 0x04005D20 RID: 23840
	public readonly int RequiredCount = 1;

	// Token: 0x04005D21 RID: 23841
	public readonly HashSet<Tag> AcceptedTags;

	// Token: 0x02001772 RID: 6002
	public enum BehaviorFlags
	{
		// Token: 0x04005D25 RID: 23845
		None,
		// Token: 0x04005D26 RID: 23846
		TrackArea,
		// Token: 0x04005D27 RID: 23847
		AllowsRegression,
		// Token: 0x04005D28 RID: 23848
		TrackValues = 4,
		// Token: 0x04005D29 RID: 23849
		TrackItems = 8,
		// Token: 0x04005D2A RID: 23850
		UniqueItems = 24
	}
}
