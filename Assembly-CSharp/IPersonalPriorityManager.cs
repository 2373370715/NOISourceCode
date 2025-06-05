using System;

// Token: 0x02001E20 RID: 7712
public interface IPersonalPriorityManager
{
	// Token: 0x0600A116 RID: 41238
	int GetAssociatedSkillLevel(ChoreGroup group);

	// Token: 0x0600A117 RID: 41239
	int GetPersonalPriority(ChoreGroup group);

	// Token: 0x0600A118 RID: 41240
	void SetPersonalPriority(ChoreGroup group, int value);

	// Token: 0x0600A119 RID: 41241
	bool IsChoreGroupDisabled(ChoreGroup group);

	// Token: 0x0600A11A RID: 41242
	void ResetPersonalPriorities();
}
