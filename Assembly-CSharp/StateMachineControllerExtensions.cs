using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000923 RID: 2339
public static class StateMachineControllerExtensions
{
	// Token: 0x06002909 RID: 10505 RVA: 0x000BF269 File Offset: 0x000BD469
	public static StateMachineInstanceType GetSMI<StateMachineInstanceType>(this StateMachine.Instance smi) where StateMachineInstanceType : StateMachine.Instance
	{
		return smi.gameObject.GetSMI<StateMachineInstanceType>();
	}

	// Token: 0x0600290A RID: 10506 RVA: 0x000BF276 File Offset: 0x000BD476
	public static DefType GetDef<DefType>(this Component cmp) where DefType : StateMachine.BaseDef
	{
		return cmp.gameObject.GetDef<DefType>();
	}

	// Token: 0x0600290B RID: 10507 RVA: 0x001E1540 File Offset: 0x001DF740
	public static DefType GetDef<DefType>(this GameObject go) where DefType : StateMachine.BaseDef
	{
		StateMachineController component = go.GetComponent<StateMachineController>();
		if (component == null)
		{
			return default(DefType);
		}
		return component.GetDef<DefType>();
	}

	// Token: 0x0600290C RID: 10508 RVA: 0x000BF283 File Offset: 0x000BD483
	public static StateMachineInstanceType GetSMI<StateMachineInstanceType>(this Component cmp) where StateMachineInstanceType : class
	{
		return cmp.gameObject.GetSMI<StateMachineInstanceType>();
	}

	// Token: 0x0600290D RID: 10509 RVA: 0x001E1570 File Offset: 0x001DF770
	public static StateMachineInstanceType GetSMI<StateMachineInstanceType>(this GameObject go) where StateMachineInstanceType : class
	{
		StateMachineController component = go.GetComponent<StateMachineController>();
		if (component != null)
		{
			return component.GetSMI<StateMachineInstanceType>();
		}
		return default(StateMachineInstanceType);
	}

	// Token: 0x0600290E RID: 10510 RVA: 0x000BF290 File Offset: 0x000BD490
	public static List<StateMachineInstanceType> GetAllSMI<StateMachineInstanceType>(this Component cmp) where StateMachineInstanceType : class
	{
		return cmp.gameObject.GetAllSMI<StateMachineInstanceType>();
	}

	// Token: 0x0600290F RID: 10511 RVA: 0x001E15A0 File Offset: 0x001DF7A0
	public static List<StateMachineInstanceType> GetAllSMI<StateMachineInstanceType>(this GameObject go) where StateMachineInstanceType : class
	{
		StateMachineController component = go.GetComponent<StateMachineController>();
		if (component != null)
		{
			return component.GetAllSMI<StateMachineInstanceType>();
		}
		return new List<StateMachineInstanceType>();
	}
}
