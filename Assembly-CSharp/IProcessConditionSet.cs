using System;
using System.Collections.Generic;

// Token: 0x02001FBA RID: 8122
public interface IProcessConditionSet
{
	// Token: 0x0600ABAD RID: 43949
	List<ProcessCondition> GetConditionSet(ProcessCondition.ProcessConditionType conditionType);
}
