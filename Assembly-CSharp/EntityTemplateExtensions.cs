using System;
using UnityEngine;

// Token: 0x020000B5 RID: 181
public static class EntityTemplateExtensions
{
	// Token: 0x06000308 RID: 776 RVA: 0x0015440C File Offset: 0x0015260C
	public static DefType AddOrGetDef<DefType>(this GameObject go) where DefType : StateMachine.BaseDef
	{
		StateMachineController stateMachineController = go.AddOrGet<StateMachineController>();
		DefType defType = stateMachineController.GetDef<DefType>();
		if (defType == null)
		{
			defType = Activator.CreateInstance<DefType>();
			stateMachineController.AddDef(defType);
			defType.Configure(go);
		}
		return defType;
	}

	// Token: 0x06000309 RID: 777 RVA: 0x00154450 File Offset: 0x00152650
	public static ComponentType AddOrGet<ComponentType>(this GameObject go) where ComponentType : Component
	{
		ComponentType componentType = go.GetComponent<ComponentType>();
		if (componentType == null)
		{
			componentType = go.AddComponent<ComponentType>();
		}
		return componentType;
	}
}
