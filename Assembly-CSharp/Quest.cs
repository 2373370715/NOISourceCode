using System;

// Token: 0x0200176A RID: 5994
public class Quest : Resource
{
	// Token: 0x06007B61 RID: 31585 RVA: 0x0032A54C File Offset: 0x0032874C
	public Quest(string id, QuestCriteria[] criteria) : base(id, id)
	{
		Debug.Assert(criteria.Length != 0);
		this.Criteria = criteria;
		string str = "STRINGS.CODEX.QUESTS." + id.ToUpperInvariant();
		StringEntry stringEntry;
		if (Strings.TryGet(str + ".NAME", out stringEntry))
		{
			this.Title = stringEntry.String;
		}
		if (Strings.TryGet(str + ".COMPLETE", out stringEntry))
		{
			this.CompletionText = stringEntry.String;
		}
		for (int i = 0; i < this.Criteria.Length; i++)
		{
			this.Criteria[i].PopulateStrings("STRINGS.CODEX.QUESTS.");
		}
	}

	// Token: 0x04005CFF RID: 23807
	public const string STRINGS_PREFIX = "STRINGS.CODEX.QUESTS.";

	// Token: 0x04005D00 RID: 23808
	public readonly QuestCriteria[] Criteria;

	// Token: 0x04005D01 RID: 23809
	public readonly string Title;

	// Token: 0x04005D02 RID: 23810
	public readonly string CompletionText;

	// Token: 0x0200176B RID: 5995
	public struct ItemData
	{
		// Token: 0x170007B2 RID: 1970
		// (get) Token: 0x06007B62 RID: 31586 RVA: 0x000F5A7E File Offset: 0x000F3C7E
		// (set) Token: 0x06007B63 RID: 31587 RVA: 0x000F5A88 File Offset: 0x000F3C88
		public int ValueHandle
		{
			get
			{
				return this.valueHandle - 1;
			}
			set
			{
				this.valueHandle = value + 1;
			}
		}

		// Token: 0x04005D03 RID: 23811
		public int LocalCellId;

		// Token: 0x04005D04 RID: 23812
		public float CurrentValue;

		// Token: 0x04005D05 RID: 23813
		public Tag SatisfyingItem;

		// Token: 0x04005D06 RID: 23814
		public Tag QualifyingTag;

		// Token: 0x04005D07 RID: 23815
		public HashedString CriteriaId;

		// Token: 0x04005D08 RID: 23816
		private int valueHandle;
	}

	// Token: 0x0200176C RID: 5996
	public enum State
	{
		// Token: 0x04005D0A RID: 23818
		NotStarted,
		// Token: 0x04005D0B RID: 23819
		InProgress,
		// Token: 0x04005D0C RID: 23820
		Completed
	}
}
