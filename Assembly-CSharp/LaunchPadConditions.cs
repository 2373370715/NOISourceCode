using System;
using System.Collections.Generic;

// Token: 0x02001953 RID: 6483
public class LaunchPadConditions : KMonoBehaviour, IProcessConditionSet
{
	// Token: 0x060086F0 RID: 34544 RVA: 0x000FCFFB File Offset: 0x000FB1FB
	public List<ProcessCondition> GetConditionSet(ProcessCondition.ProcessConditionType conditionType)
	{
		if (conditionType != ProcessCondition.ProcessConditionType.RocketStorage)
		{
			return null;
		}
		return this.conditions;
	}

	// Token: 0x060086F1 RID: 34545 RVA: 0x000FD009 File Offset: 0x000FB209
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.conditions = new List<ProcessCondition>();
		this.conditions.Add(new TransferCargoCompleteCondition(base.gameObject));
	}

	// Token: 0x0400664A RID: 26186
	private List<ProcessCondition> conditions;
}
