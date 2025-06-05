using System;
using System.Collections.Generic;
using System.Reflection;

// Token: 0x020008E7 RID: 2279
public class MySmi : MyAttributeManager<StateMachine.Instance>
{
	// Token: 0x060027E4 RID: 10212 RVA: 0x001DF6B0 File Offset: 0x001DD8B0
	public static void Init()
	{
		MyAttributes.Register(new MySmi(new Dictionary<Type, MethodInfo>
		{
			{
				typeof(MySmiGet),
				typeof(MySmi).GetMethod("FindSmi")
			},
			{
				typeof(MySmiReq),
				typeof(MySmi).GetMethod("RequireSmi")
			}
		}));
	}

	// Token: 0x060027E5 RID: 10213 RVA: 0x000BE666 File Offset: 0x000BC866
	public MySmi(Dictionary<Type, MethodInfo> attributeMap) : base(attributeMap, null)
	{
	}

	// Token: 0x060027E6 RID: 10214 RVA: 0x001DF714 File Offset: 0x001DD914
	public static StateMachine.Instance FindSmi<T>(KMonoBehaviour c, bool isStart) where T : StateMachine.Instance
	{
		StateMachineController component = c.GetComponent<StateMachineController>();
		if (component != null)
		{
			return component.GetSMI<T>();
		}
		return null;
	}

	// Token: 0x060027E7 RID: 10215 RVA: 0x001DF740 File Offset: 0x001DD940
	public static StateMachine.Instance RequireSmi<T>(KMonoBehaviour c, bool isStart) where T : StateMachine.Instance
	{
		if (isStart)
		{
			StateMachine.Instance instance = MySmi.FindSmi<T>(c, isStart);
			Debug.Assert(instance != null, string.Format("{0} '{1}' requires a StateMachineInstance of type {2}!", c.GetType().ToString(), c.name, typeof(T)));
			return instance;
		}
		return MySmi.FindSmi<T>(c, isStart);
	}
}
