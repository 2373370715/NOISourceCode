using System;
using System.Collections.Generic;
using System.Reflection;

// Token: 0x0200110C RID: 4364
public class GameComps : KComponents
{
	// Token: 0x0600593F RID: 22847 RVA: 0x0029CE54 File Offset: 0x0029B054
	public GameComps()
	{
		foreach (FieldInfo fieldInfo in typeof(GameComps).GetFields())
		{
			object obj = Activator.CreateInstance(fieldInfo.FieldType);
			fieldInfo.SetValue(null, obj);
			base.Add<IComponentManager>(obj as IComponentManager);
			if (obj is IKComponentManager)
			{
				IKComponentManager inst = obj as IKComponentManager;
				GameComps.AddKComponentManager(fieldInfo.FieldType, inst);
			}
		}
	}

	// Token: 0x06005940 RID: 22848 RVA: 0x0029CEC8 File Offset: 0x0029B0C8
	public new void Clear()
	{
		FieldInfo[] fields = typeof(GameComps).GetFields();
		for (int i = 0; i < fields.Length; i++)
		{
			IComponentManager componentManager = fields[i].GetValue(null) as IComponentManager;
			if (componentManager != null)
			{
				componentManager.Clear();
			}
		}
	}

	// Token: 0x06005941 RID: 22849 RVA: 0x000DE983 File Offset: 0x000DCB83
	public static void AddKComponentManager(Type kcomponent, IKComponentManager inst)
	{
		GameComps.kcomponentManagers[kcomponent] = inst;
	}

	// Token: 0x06005942 RID: 22850 RVA: 0x000DE991 File Offset: 0x000DCB91
	public static IKComponentManager GetKComponentManager(Type kcomponent_type)
	{
		return GameComps.kcomponentManagers[kcomponent_type];
	}

	// Token: 0x04003F66 RID: 16230
	public static GravityComponents Gravities;

	// Token: 0x04003F67 RID: 16231
	public static FallerComponents Fallers;

	// Token: 0x04003F68 RID: 16232
	public static InfraredVisualizerComponents InfraredVisualizers;

	// Token: 0x04003F69 RID: 16233
	public static ElementSplitterComponents ElementSplitters;

	// Token: 0x04003F6A RID: 16234
	public static OreSizeVisualizerComponents OreSizeVisualizers;

	// Token: 0x04003F6B RID: 16235
	public static StructureTemperatureComponents StructureTemperatures;

	// Token: 0x04003F6C RID: 16236
	public static DiseaseContainers DiseaseContainers;

	// Token: 0x04003F6D RID: 16237
	public static RequiresFoundation RequiresFoundations;

	// Token: 0x04003F6E RID: 16238
	public static WhiteBoard WhiteBoards;

	// Token: 0x04003F6F RID: 16239
	private static Dictionary<Type, IKComponentManager> kcomponentManagers = new Dictionary<Type, IKComponentManager>();
}
