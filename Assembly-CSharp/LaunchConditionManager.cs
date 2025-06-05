using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x0200194B RID: 6475
[AddComponentMenu("KMonoBehaviour/scripts/LaunchConditionManager")]
public class LaunchConditionManager : KMonoBehaviour, ISim4000ms, ISim1000ms
{
	// Token: 0x170008CB RID: 2251
	// (get) Token: 0x060086B6 RID: 34486 RVA: 0x000FCE54 File Offset: 0x000FB054
	// (set) Token: 0x060086B7 RID: 34487 RVA: 0x000FCE5C File Offset: 0x000FB05C
	public List<RocketModule> rocketModules { get; private set; }

	// Token: 0x060086B8 RID: 34488 RVA: 0x000FCE65 File Offset: 0x000FB065
	public void DEBUG_TraceModuleDestruction(string moduleName, string state, string stackTrace)
	{
		if (this.DEBUG_ModuleDestructions == null)
		{
			this.DEBUG_ModuleDestructions = new List<global::Tuple<string, string, string>>();
		}
		this.DEBUG_ModuleDestructions.Add(new global::Tuple<string, string, string>(moduleName, state, stackTrace));
	}

	// Token: 0x060086B9 RID: 34489 RVA: 0x0035AC8C File Offset: 0x00358E8C
	[ContextMenu("Dump Module Destructions")]
	private void DEBUG_DumpModuleDestructions()
	{
		if (this.DEBUG_ModuleDestructions == null || this.DEBUG_ModuleDestructions.Count == 0)
		{
			DebugUtil.LogArgs(new object[]
			{
				"Sorry, no logged module destructions. :("
			});
			return;
		}
		foreach (global::Tuple<string, string, string> tuple in this.DEBUG_ModuleDestructions)
		{
			DebugUtil.LogArgs(new object[]
			{
				tuple.first,
				">",
				tuple.second,
				"\n",
				tuple.third,
				"\nEND MODULE DUMP\n\n"
			});
		}
	}

	// Token: 0x060086BA RID: 34490 RVA: 0x000FCE8D File Offset: 0x000FB08D
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.rocketModules = new List<RocketModule>();
	}

	// Token: 0x060086BB RID: 34491 RVA: 0x000FCEA0 File Offset: 0x000FB0A0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.launchable = base.GetComponent<ILaunchableRocket>();
		this.FindModules();
		base.GetComponent<AttachableBuilding>().onAttachmentNetworkChanged = delegate(object data)
		{
			this.FindModules();
		};
	}

	// Token: 0x060086BC RID: 34492 RVA: 0x000C4795 File Offset: 0x000C2995
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x060086BD RID: 34493 RVA: 0x0035AD40 File Offset: 0x00358F40
	public void Sim1000ms(float dt)
	{
		Spacecraft spacecraftFromLaunchConditionManager = SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this);
		if (spacecraftFromLaunchConditionManager == null)
		{
			return;
		}
		global::Debug.Assert(!DlcManager.FeatureClusterSpaceEnabled());
		SpaceDestination spacecraftDestination = SpacecraftManager.instance.GetSpacecraftDestination(spacecraftFromLaunchConditionManager.id);
		if (base.gameObject.GetComponent<LogicPorts>().GetInputValue(this.triggerPort) == 1 && spacecraftDestination != null && spacecraftDestination.id != -1)
		{
			this.Launch(spacecraftDestination);
		}
	}

	// Token: 0x060086BE RID: 34494 RVA: 0x0035ADA8 File Offset: 0x00358FA8
	public void FindModules()
	{
		foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(base.GetComponent<AttachableBuilding>()))
		{
			RocketModule component = gameObject.GetComponent<RocketModule>();
			if (component != null && component.conditionManager == null)
			{
				component.conditionManager = this;
				component.RegisterWithConditionManager();
			}
		}
	}

	// Token: 0x060086BF RID: 34495 RVA: 0x000FCED1 File Offset: 0x000FB0D1
	public void RegisterRocketModule(RocketModule module)
	{
		if (!this.rocketModules.Contains(module))
		{
			this.rocketModules.Add(module);
		}
	}

	// Token: 0x060086C0 RID: 34496 RVA: 0x000FCEED File Offset: 0x000FB0ED
	public void UnregisterRocketModule(RocketModule module)
	{
		this.rocketModules.Remove(module);
	}

	// Token: 0x060086C1 RID: 34497 RVA: 0x0035AE24 File Offset: 0x00359024
	public List<ProcessCondition> GetLaunchConditionList()
	{
		List<ProcessCondition> list = new List<ProcessCondition>();
		foreach (RocketModule rocketModule in this.rocketModules)
		{
			foreach (ProcessCondition item in rocketModule.GetConditionSet(ProcessCondition.ProcessConditionType.RocketPrep))
			{
				list.Add(item);
			}
			foreach (ProcessCondition item2 in rocketModule.GetConditionSet(ProcessCondition.ProcessConditionType.RocketStorage))
			{
				list.Add(item2);
			}
		}
		return list;
	}

	// Token: 0x060086C2 RID: 34498 RVA: 0x0035AF04 File Offset: 0x00359104
	public void Launch(SpaceDestination destination)
	{
		if (destination == null)
		{
			global::Debug.LogError("Null destination passed to launch");
		}
		if (SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this).state != Spacecraft.MissionState.Grounded)
		{
			return;
		}
		if (DebugHandler.InstantBuildMode || (this.CheckReadyToLaunch() && this.CheckAbleToFly()))
		{
			this.launchable.LaunchableGameObject.Trigger(705820818, null);
			SpacecraftManager.instance.SetSpacecraftDestination(this, destination);
			SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this).BeginMission(destination);
		}
	}

	// Token: 0x060086C3 RID: 34499 RVA: 0x0035AF7C File Offset: 0x0035917C
	public bool CheckReadyToLaunch()
	{
		foreach (RocketModule rocketModule in this.rocketModules)
		{
			using (List<ProcessCondition>.Enumerator enumerator2 = rocketModule.GetConditionSet(ProcessCondition.ProcessConditionType.RocketPrep).GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.Current.EvaluateCondition() == ProcessCondition.Status.Failure)
					{
						return false;
					}
				}
			}
			using (List<ProcessCondition>.Enumerator enumerator2 = rocketModule.GetConditionSet(ProcessCondition.ProcessConditionType.RocketStorage).GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.Current.EvaluateCondition() == ProcessCondition.Status.Failure)
					{
						return false;
					}
				}
			}
		}
		return true;
	}

	// Token: 0x060086C4 RID: 34500 RVA: 0x0035B05C File Offset: 0x0035925C
	public bool CheckAbleToFly()
	{
		foreach (RocketModule rocketModule in this.rocketModules)
		{
			using (List<ProcessCondition>.Enumerator enumerator2 = rocketModule.GetConditionSet(ProcessCondition.ProcessConditionType.RocketFlight).GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.Current.EvaluateCondition() == ProcessCondition.Status.Failure)
					{
						return false;
					}
				}
			}
		}
		return true;
	}

	// Token: 0x060086C5 RID: 34501 RVA: 0x0035B0F0 File Offset: 0x003592F0
	private void ClearFlightStatuses()
	{
		KSelectable component = base.GetComponent<KSelectable>();
		foreach (KeyValuePair<ProcessCondition, Guid> keyValuePair in this.conditionStatuses)
		{
			component.RemoveStatusItem(keyValuePair.Value, false);
		}
		this.conditionStatuses.Clear();
	}

	// Token: 0x060086C6 RID: 34502 RVA: 0x0035B160 File Offset: 0x00359360
	public void Sim4000ms(float dt)
	{
		bool flag = this.CheckReadyToLaunch();
		LogicPorts component = base.gameObject.GetComponent<LogicPorts>();
		if (flag)
		{
			Spacecraft spacecraftFromLaunchConditionManager = SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this);
			if (spacecraftFromLaunchConditionManager.state == Spacecraft.MissionState.Grounded || spacecraftFromLaunchConditionManager.state == Spacecraft.MissionState.Launching)
			{
				component.SendSignal(this.statusPort, 1);
			}
			else
			{
				component.SendSignal(this.statusPort, 0);
			}
			KSelectable component2 = base.GetComponent<KSelectable>();
			using (List<RocketModule>.Enumerator enumerator = this.rocketModules.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					RocketModule rocketModule = enumerator.Current;
					foreach (ProcessCondition processCondition in rocketModule.GetConditionSet(ProcessCondition.ProcessConditionType.RocketFlight))
					{
						if (processCondition.EvaluateCondition() == ProcessCondition.Status.Failure)
						{
							if (!this.conditionStatuses.ContainsKey(processCondition))
							{
								StatusItem statusItem = processCondition.GetStatusItem(ProcessCondition.Status.Failure);
								this.conditionStatuses[processCondition] = component2.AddStatusItem(statusItem, processCondition);
							}
						}
						else if (this.conditionStatuses.ContainsKey(processCondition))
						{
							component2.RemoveStatusItem(this.conditionStatuses[processCondition], false);
							this.conditionStatuses.Remove(processCondition);
						}
					}
				}
				return;
			}
		}
		this.ClearFlightStatuses();
		component.SendSignal(this.statusPort, 0);
	}

	// Token: 0x04006621 RID: 26145
	public HashedString triggerPort;

	// Token: 0x04006622 RID: 26146
	public HashedString statusPort;

	// Token: 0x04006624 RID: 26148
	private ILaunchableRocket launchable;

	// Token: 0x04006625 RID: 26149
	[Serialize]
	private List<global::Tuple<string, string, string>> DEBUG_ModuleDestructions;

	// Token: 0x04006626 RID: 26150
	private Dictionary<ProcessCondition, Guid> conditionStatuses = new Dictionary<ProcessCondition, Guid>();

	// Token: 0x0200194C RID: 6476
	public enum ConditionType
	{
		// Token: 0x04006628 RID: 26152
		Launch,
		// Token: 0x04006629 RID: 26153
		Flight
	}
}
