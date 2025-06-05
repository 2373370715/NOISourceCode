using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x02000360 RID: 864
public class GeneratedBuildings
{
	// Token: 0x06000DB2 RID: 3506 RVA: 0x0017E8A4 File Offset: 0x0017CAA4
	public static void LoadGeneratedBuildings(List<Type> types)
	{
		Type typeFromHandle = typeof(IBuildingConfig);
		List<Type> list = new List<Type>();
		foreach (Type type in types)
		{
			if (typeFromHandle.IsAssignableFrom(type) && !type.IsAbstract && !type.IsInterface)
			{
				list.Add(type);
			}
		}
		foreach (Type type2 in list)
		{
			object obj = Activator.CreateInstance(type2);
			try
			{
				BuildingConfigManager.Instance.RegisterBuilding(obj as IBuildingConfig);
			}
			catch (Exception e)
			{
				DebugUtil.LogException(null, "Exception in RegisterBuilding for type " + type2.FullName + " from " + type2.Assembly.GetName().Name, e);
			}
		}
		foreach (PlanScreen.PlanInfo planInfo in BUILDINGS.PLANORDER)
		{
			List<string> list2 = new List<string>();
			foreach (KeyValuePair<string, string> keyValuePair in planInfo.buildingAndSubcategoryData)
			{
				if (Assets.GetBuildingDef(keyValuePair.Key) == null)
				{
					list2.Add(keyValuePair.Key);
				}
			}
			using (List<string>.Enumerator enumerator4 = list2.GetEnumerator())
			{
				while (enumerator4.MoveNext())
				{
					string entry = enumerator4.Current;
					planInfo.buildingAndSubcategoryData.RemoveAll((KeyValuePair<string, string> match) => match.Key == entry);
				}
			}
			List<string> list3 = new List<string>();
			using (List<string>.Enumerator enumerator4 = planInfo.data.GetEnumerator())
			{
				while (enumerator4.MoveNext())
				{
					string entry = enumerator4.Current;
					if (planInfo.buildingAndSubcategoryData.FindIndex((KeyValuePair<string, string> x) => x.Key == entry) == -1 && Assets.GetBuildingDef(entry) != null)
					{
						global::Debug.LogWarning("Mod: Building '" + entry + "' was not added properly to PlanInfo, use ModUtil.AddBuildingToPlanScreen instead.");
						list3.Add(entry);
					}
				}
			}
			foreach (string building_id in list3)
			{
				ModUtil.AddBuildingToPlanScreen(planInfo.category, building_id, "uncategorized");
			}
		}
	}

	// Token: 0x06000DB3 RID: 3507 RVA: 0x0017EC0C File Offset: 0x0017CE0C
	public static void MakeBuildingAlwaysOperational(GameObject go)
	{
		BuildingDef def = go.GetComponent<BuildingComplete>().Def;
		if (def.LogicInputPorts != null || def.LogicOutputPorts != null)
		{
			global::Debug.LogWarning("Do not call MakeBuildingAlwaysOperational directly if LogicInputPorts or LogicOutputPorts are defined. Instead set BuildingDef.AlwaysOperational = true");
		}
		GeneratedBuildings.MakeBuildingAlwaysOperationalImpl(go);
	}

	// Token: 0x06000DB4 RID: 3508 RVA: 0x000B062E File Offset: 0x000AE82E
	public static void RemoveLoopingSounds(GameObject go)
	{
		UnityEngine.Object.DestroyImmediate(go.GetComponent<LoopingSounds>());
	}

	// Token: 0x06000DB5 RID: 3509 RVA: 0x000B063B File Offset: 0x000AE83B
	public static void RemoveDefaultLogicPorts(GameObject go)
	{
		UnityEngine.Object.DestroyImmediate(go.GetComponent<LogicPorts>());
	}

	// Token: 0x06000DB6 RID: 3510 RVA: 0x000B0648 File Offset: 0x000AE848
	public static void RegisterWithOverlay(HashSet<Tag> overlay_tags, string id)
	{
		overlay_tags.Add(new Tag(id));
		overlay_tags.Add(new Tag(id + "UnderConstruction"));
	}

	// Token: 0x06000DB7 RID: 3511 RVA: 0x000B066E File Offset: 0x000AE86E
	public static void RegisterSingleLogicInputPort(GameObject go)
	{
		LogicPorts logicPorts = go.AddOrGet<LogicPorts>();
		logicPorts.inputPortInfo = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0)).ToArray();
		logicPorts.outputPortInfo = null;
	}

	// Token: 0x06000DB8 RID: 3512 RVA: 0x000B0693 File Offset: 0x000AE893
	private static void MakeBuildingAlwaysOperationalImpl(GameObject go)
	{
		UnityEngine.Object.DestroyImmediate(go.GetComponent<BuildingEnabledButton>());
		UnityEngine.Object.DestroyImmediate(go.GetComponent<Operational>());
		UnityEngine.Object.DestroyImmediate(go.GetComponent<LogicPorts>());
	}

	// Token: 0x06000DB9 RID: 3513 RVA: 0x0017EC48 File Offset: 0x0017CE48
	public static void InitializeLogicPorts(GameObject go, BuildingDef def)
	{
		if (def.AlwaysOperational)
		{
			GeneratedBuildings.MakeBuildingAlwaysOperationalImpl(go);
		}
		if (def.LogicInputPorts != null)
		{
			go.AddOrGet<LogicPorts>().inputPortInfo = def.LogicInputPorts.ToArray();
		}
		if (def.LogicOutputPorts != null)
		{
			go.AddOrGet<LogicPorts>().outputPortInfo = def.LogicOutputPorts.ToArray();
		}
	}

	// Token: 0x06000DBA RID: 3514 RVA: 0x0017ECA0 File Offset: 0x0017CEA0
	public static void InitializeHighEnergyParticlePorts(GameObject go, BuildingDef def)
	{
		if (def.UseHighEnergyParticleInputPort || def.UseHighEnergyParticleOutputPort)
		{
			HighEnergyParticlePort highEnergyParticlePort = go.AddOrGet<HighEnergyParticlePort>();
			highEnergyParticlePort.particleInputOffset = def.HighEnergyParticleInputOffset;
			highEnergyParticlePort.particleOutputOffset = def.HighEnergyParticleOutputOffset;
			highEnergyParticlePort.particleInputEnabled = def.UseHighEnergyParticleInputPort;
			highEnergyParticlePort.particleOutputEnabled = def.UseHighEnergyParticleOutputPort;
		}
	}
}
