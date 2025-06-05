using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x020012DC RID: 4828
[AddComponentMenu("KMonoBehaviour/scripts/EntityConfigManager")]
public class EntityConfigManager : KMonoBehaviour
{
	// Token: 0x06006315 RID: 25365 RVA: 0x000E515B File Offset: 0x000E335B
	public static void DestroyInstance()
	{
		EntityConfigManager.Instance = null;
	}

	// Token: 0x06006316 RID: 25366 RVA: 0x000E5163 File Offset: 0x000E3363
	protected override void OnPrefabInit()
	{
		EntityConfigManager.Instance = this;
	}

	// Token: 0x06006317 RID: 25367 RVA: 0x002C6F34 File Offset: 0x002C5134
	private static int GetSortOrder(Type type)
	{
		foreach (Attribute attribute in type.GetCustomAttributes(true))
		{
			if (attribute.GetType() == typeof(EntityConfigOrder))
			{
				return (attribute as EntityConfigOrder).sortOrder;
			}
		}
		return 0;
	}

	// Token: 0x06006318 RID: 25368 RVA: 0x002C6F84 File Offset: 0x002C5184
	public void LoadGeneratedEntities(List<Type> types)
	{
		Type typeFromHandle = typeof(IEntityConfig);
		Type typeFromHandle2 = typeof(IMultiEntityConfig);
		List<EntityConfigManager.ConfigEntry> list = new List<EntityConfigManager.ConfigEntry>();
		foreach (Type type in types)
		{
			if ((typeFromHandle.IsAssignableFrom(type) || typeFromHandle2.IsAssignableFrom(type)) && !type.IsAbstract && !type.IsInterface)
			{
				int sortOrder = EntityConfigManager.GetSortOrder(type);
				EntityConfigManager.ConfigEntry item = new EntityConfigManager.ConfigEntry
				{
					type = type,
					sortOrder = sortOrder
				};
				list.Add(item);
			}
		}
		list.Sort((EntityConfigManager.ConfigEntry x, EntityConfigManager.ConfigEntry y) => x.sortOrder.CompareTo(y.sortOrder));
		foreach (EntityConfigManager.ConfigEntry configEntry in list)
		{
			object obj = Activator.CreateInstance(configEntry.type);
			if (obj is IEntityConfig)
			{
				IEntityConfig entityConfig = obj as IEntityConfig;
				string[] array = null;
				string[] array2 = null;
				if (entityConfig.GetDlcIds() != null)
				{
					DlcManager.ConvertAvailableToRequireAndForbidden(entityConfig.GetDlcIds(), out array, out array2);
					DebugUtil.DevLogError(string.Format("{0} implements GetDlcIds, which is obsolete.", configEntry.type));
				}
				else
				{
					IHasDlcRestrictions hasDlcRestrictions = obj as IHasDlcRestrictions;
					if (hasDlcRestrictions != null)
					{
						array = hasDlcRestrictions.GetRequiredDlcIds();
						array2 = hasDlcRestrictions.GetForbiddenDlcIds();
					}
				}
				if (DlcManager.IsCorrectDlcSubscribed(array, array2))
				{
					this.RegisterEntity(entityConfig, array, array2);
				}
			}
			IMultiEntityConfig multiEntityConfig = obj as IMultiEntityConfig;
			if (multiEntityConfig != null)
			{
				DebugUtil.Assert(!(obj is IHasDlcRestrictions), "IMultiEntityConfig cannot implement IHasDlcRestrictions, wrap the individual config instead.");
				this.RegisterEntities(multiEntityConfig);
			}
		}
	}

	// Token: 0x06006319 RID: 25369 RVA: 0x002C715C File Offset: 0x002C535C
	[Conditional("UNITY_EDITOR")]
	private void ValidateEntityConfig(IEntityConfig entityConfig)
	{
		if (entityConfig == null)
		{
			throw new ArgumentNullException("entityConfig");
		}
		Type type = entityConfig.GetType();
		Type typeFromHandle = typeof(IHasDlcRestrictions);
		bool flag = type.GetMethod("GetRequiredDlcIds", Type.EmptyTypes) != null;
		bool flag2 = type.GetMethod("GetForbiddenDlcIds", Type.EmptyTypes) != null;
		bool flag3 = typeFromHandle.IsAssignableFrom(type);
		if ((flag || flag2) && !flag3)
		{
			DebugUtil.LogErrorArgs(new object[]
			{
				type.Name + " is an IEntityConfig and has GetRequiredDlcIds or GetForbiddenDlcIds but does not implement IHasDlcRestrictions."
			});
		}
	}

	// Token: 0x0600631A RID: 25370 RVA: 0x002C71E4 File Offset: 0x002C53E4
	[Conditional("UNITY_EDITOR")]
	private void ValidateMultiEntityConfig(IMultiEntityConfig entityConfig)
	{
		if (entityConfig == null)
		{
			throw new ArgumentNullException("entityConfig");
		}
		Type type = entityConfig.GetType();
		bool flag = type.GetMethod("GetRequiredDlcIds", Type.EmptyTypes) != null;
		bool flag2 = type.GetMethod("GetForbiddenDlcIds", Type.EmptyTypes) != null;
		if (flag || flag2)
		{
			DebugUtil.LogErrorArgs(new object[]
			{
				type.Name + " is an IMultiEntityConfig and you shouldn't be specifying GetRequiredDlcIds or GetForbiddenDlcIds. Wrap each config in a DLC check instead."
			});
		}
	}

	// Token: 0x0600631B RID: 25371 RVA: 0x002C7258 File Offset: 0x002C5458
	public void RegisterEntity(IEntityConfig config, string[] requiredDlcIds = null, string[] forbiddenDlcIds = null)
	{
		GameObject gameObject = config.CreatePrefab();
		if (gameObject == null)
		{
			return;
		}
		KPrefabID component = gameObject.GetComponent<KPrefabID>();
		component.requiredDlcIds = requiredDlcIds;
		component.forbiddenDlcIds = forbiddenDlcIds;
		component.prefabInitFn += config.OnPrefabInit;
		component.prefabSpawnFn += config.OnSpawn;
		Assets.AddPrefab(component);
	}

	// Token: 0x0600631C RID: 25372 RVA: 0x002C72B8 File Offset: 0x002C54B8
	public void RegisterEntities(IMultiEntityConfig config)
	{
		foreach (GameObject gameObject in config.CreatePrefabs())
		{
			KPrefabID component = gameObject.GetComponent<KPrefabID>();
			component.prefabInitFn += config.OnPrefabInit;
			component.prefabSpawnFn += config.OnSpawn;
			Assets.AddPrefab(component);
		}
	}

	// Token: 0x04004708 RID: 18184
	public static EntityConfigManager Instance;

	// Token: 0x020012DD RID: 4829
	private struct ConfigEntry
	{
		// Token: 0x04004709 RID: 18185
		public Type type;

		// Token: 0x0400470A RID: 18186
		public int sortOrder;
	}
}
