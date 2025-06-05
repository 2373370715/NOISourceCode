using System;
using System.Collections.Generic;
using System.Diagnostics;
using STRINGS;
using UnityEngine;

// Token: 0x0200198A RID: 6538
[AddComponentMenu("KMonoBehaviour/scripts/RocketModule")]
public class RocketModule : KMonoBehaviour
{
	// Token: 0x06008819 RID: 34841 RVA: 0x00361A24 File Offset: 0x0035FC24
	public ProcessCondition AddModuleCondition(ProcessCondition.ProcessConditionType conditionType, ProcessCondition condition)
	{
		if (!this.moduleConditions.ContainsKey(conditionType))
		{
			this.moduleConditions.Add(conditionType, new List<ProcessCondition>());
		}
		if (!this.moduleConditions[conditionType].Contains(condition))
		{
			this.moduleConditions[conditionType].Add(condition);
		}
		return condition;
	}

	// Token: 0x0600881A RID: 34842 RVA: 0x00361A78 File Offset: 0x0035FC78
	public List<ProcessCondition> GetConditionSet(ProcessCondition.ProcessConditionType conditionType)
	{
		List<ProcessCondition> list = new List<ProcessCondition>();
		if (conditionType == ProcessCondition.ProcessConditionType.All)
		{
			using (Dictionary<ProcessCondition.ProcessConditionType, List<ProcessCondition>>.Enumerator enumerator = this.moduleConditions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<ProcessCondition.ProcessConditionType, List<ProcessCondition>> keyValuePair = enumerator.Current;
					list.AddRange(keyValuePair.Value);
				}
				return list;
			}
		}
		if (this.moduleConditions.ContainsKey(conditionType))
		{
			list = this.moduleConditions[conditionType];
		}
		return list;
	}

	// Token: 0x0600881B RID: 34843 RVA: 0x000FD9A9 File Offset: 0x000FBBA9
	public void SetBGKAnim(KAnimFile anim_file)
	{
		this.bgAnimFile = anim_file;
	}

	// Token: 0x0600881C RID: 34844 RVA: 0x000FD9B2 File Offset: 0x000FBBB2
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		GameUtil.SubscribeToTags<RocketModule>(this, RocketModule.OnRocketOnGroundTagDelegate, false);
		GameUtil.SubscribeToTags<RocketModule>(this, RocketModule.OnRocketNotOnGroundTagDelegate, false);
	}

	// Token: 0x0600881D RID: 34845 RVA: 0x00361AF8 File Offset: 0x0035FCF8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (!DlcManager.FeatureClusterSpaceEnabled())
		{
			this.conditionManager = this.FindLaunchConditionManager();
			Spacecraft spacecraftFromLaunchConditionManager = SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this.conditionManager);
			if (spacecraftFromLaunchConditionManager != null)
			{
				this.SetParentRocketName(spacecraftFromLaunchConditionManager.GetRocketName());
			}
			this.RegisterWithConditionManager();
		}
		KSelectable component = base.GetComponent<KSelectable>();
		if (component != null)
		{
			component.AddStatusItem(Db.Get().BuildingStatusItems.RocketName, this);
		}
		base.Subscribe<RocketModule>(1502190696, RocketModule.DEBUG_OnDestroyDelegate);
		this.FixSorting();
		AttachableBuilding component2 = base.GetComponent<AttachableBuilding>();
		component2.onAttachmentNetworkChanged = (Action<object>)Delegate.Combine(component2.onAttachmentNetworkChanged, new Action<object>(this.OnAttachmentNetworkChanged));
		if (this.bgAnimFile != null)
		{
			this.AddBGGantry();
		}
	}

	// Token: 0x0600881E RID: 34846 RVA: 0x00361BC0 File Offset: 0x0035FDC0
	public void FixSorting()
	{
		int num = 0;
		AttachableBuilding component = base.GetComponent<AttachableBuilding>();
		while (component != null)
		{
			BuildingAttachPoint attachedTo = component.GetAttachedTo();
			if (!(attachedTo != null))
			{
				break;
			}
			component = attachedTo.GetComponent<AttachableBuilding>();
			num++;
		}
		Vector3 localPosition = base.transform.GetLocalPosition();
		localPosition.z = Grid.GetLayerZ(Grid.SceneLayer.Building) - (float)num * 0.01f;
		base.transform.SetLocalPosition(localPosition);
		KBatchedAnimController component2 = base.GetComponent<KBatchedAnimController>();
		if (component2.enabled)
		{
			component2.enabled = false;
			component2.enabled = true;
		}
	}

	// Token: 0x0600881F RID: 34847 RVA: 0x000FD9D2 File Offset: 0x000FBBD2
	private void OnAttachmentNetworkChanged(object ab)
	{
		this.FixSorting();
	}

	// Token: 0x06008820 RID: 34848 RVA: 0x00361C4C File Offset: 0x0035FE4C
	private void AddBGGantry()
	{
		KAnimControllerBase component = base.GetComponent<KAnimControllerBase>();
		GameObject gameObject = new GameObject();
		gameObject.name = string.Format(this.rocket_module_bg_base_string, base.name, this.rocket_module_bg_affix);
		gameObject.SetActive(false);
		Vector3 position = component.transform.GetPosition();
		position.z = Grid.GetLayerZ(Grid.SceneLayer.InteriorWall);
		gameObject.transform.SetPosition(position);
		gameObject.transform.parent = base.transform;
		KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			this.bgAnimFile
		};
		kbatchedAnimController.initialAnim = this.rocket_module_bg_anim;
		kbatchedAnimController.fgLayer = Grid.SceneLayer.NoLayer;
		kbatchedAnimController.initialMode = KAnim.PlayMode.Paused;
		kbatchedAnimController.FlipX = component.FlipX;
		kbatchedAnimController.FlipY = component.FlipY;
		gameObject.SetActive(true);
	}

	// Token: 0x06008821 RID: 34849 RVA: 0x00361D18 File Offset: 0x0035FF18
	private void DEBUG_OnDestroy(object data)
	{
		if (this.conditionManager != null && !App.IsExiting && !KMonoBehaviour.isLoadingScene)
		{
			Spacecraft spacecraftFromLaunchConditionManager = SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this.conditionManager);
			this.conditionManager.DEBUG_TraceModuleDestruction(base.name, (spacecraftFromLaunchConditionManager == null) ? "null spacecraft" : spacecraftFromLaunchConditionManager.state.ToString(), new StackTrace(true).ToString());
		}
	}

	// Token: 0x06008822 RID: 34850 RVA: 0x00361D8C File Offset: 0x0035FF8C
	private void OnRocketOnGroundTag(object data)
	{
		this.RegisterComponents();
		Operational component = base.GetComponent<Operational>();
		if (this.operationalLandedRequired && component != null)
		{
			component.SetFlag(RocketModule.landedFlag, true);
		}
	}

	// Token: 0x06008823 RID: 34851 RVA: 0x00361DC4 File Offset: 0x0035FFC4
	private void OnRocketNotOnGroundTag(object data)
	{
		this.DeregisterComponents();
		Operational component = base.GetComponent<Operational>();
		if (this.operationalLandedRequired && component != null)
		{
			component.SetFlag(RocketModule.landedFlag, false);
		}
	}

	// Token: 0x06008824 RID: 34852 RVA: 0x00361DFC File Offset: 0x0035FFFC
	public void DeregisterComponents()
	{
		KSelectable component = base.GetComponent<KSelectable>();
		component.IsSelectable = false;
		BuildingComplete component2 = base.GetComponent<BuildingComplete>();
		if (component2 != null)
		{
			component2.UpdatePosition();
		}
		if (SelectTool.Instance.selected == component)
		{
			SelectTool.Instance.Select(null, false);
		}
		Deconstructable component3 = base.GetComponent<Deconstructable>();
		if (component3 != null)
		{
			component3.SetAllowDeconstruction(false);
		}
		HandleVector<int>.Handle handle = GameComps.StructureTemperatures.GetHandle(base.gameObject);
		if (handle.IsValid())
		{
			GameComps.StructureTemperatures.Disable(handle);
		}
		FakeFloorAdder component4 = base.GetComponent<FakeFloorAdder>();
		if (component4 != null)
		{
			component4.SetFloor(false);
		}
		AccessControl component5 = base.GetComponent<AccessControl>();
		if (component5 != null)
		{
			component5.SetRegistered(false);
		}
		foreach (ManualDeliveryKG manualDeliveryKG in base.GetComponents<ManualDeliveryKG>())
		{
			DebugUtil.DevAssert(!manualDeliveryKG.IsPaused, "RocketModule ManualDeliver chore was already paused, when this rocket lands it will re-enable it.", null);
			manualDeliveryKG.Pause(true, "Rocket heading to space");
		}
		BuildingConduitEndpoints[] components2 = base.GetComponents<BuildingConduitEndpoints>();
		for (int i = 0; i < components2.Length; i++)
		{
			components2[i].RemoveEndPoint();
		}
		ReorderableBuilding component6 = base.GetComponent<ReorderableBuilding>();
		if (component6 != null)
		{
			component6.ShowReorderArm(false);
		}
		Workable component7 = base.GetComponent<Workable>();
		if (component7 != null)
		{
			component7.RefreshReachability();
		}
		Structure component8 = base.GetComponent<Structure>();
		if (component8 != null)
		{
			component8.UpdatePosition();
		}
		WireUtilitySemiVirtualNetworkLink component9 = base.GetComponent<WireUtilitySemiVirtualNetworkLink>();
		if (component9 != null)
		{
			component9.SetLinkConnected(false);
		}
		PartialLightBlocking component10 = base.GetComponent<PartialLightBlocking>();
		if (component10 != null)
		{
			component10.ClearLightBlocking();
		}
	}

	// Token: 0x06008825 RID: 34853 RVA: 0x00361FA0 File Offset: 0x003601A0
	public void RegisterComponents()
	{
		base.GetComponent<KSelectable>().IsSelectable = true;
		BuildingComplete component = base.GetComponent<BuildingComplete>();
		if (component != null)
		{
			component.UpdatePosition();
		}
		Deconstructable component2 = base.GetComponent<Deconstructable>();
		if (component2 != null)
		{
			component2.SetAllowDeconstruction(true);
		}
		HandleVector<int>.Handle handle = GameComps.StructureTemperatures.GetHandle(base.gameObject);
		if (handle.IsValid())
		{
			GameComps.StructureTemperatures.Enable(handle);
		}
		Storage[] components = base.GetComponents<Storage>();
		for (int i = 0; i < components.Length; i++)
		{
			components[i].UpdateStoredItemCachedCells();
		}
		FakeFloorAdder component3 = base.GetComponent<FakeFloorAdder>();
		if (component3 != null)
		{
			component3.SetFloor(true);
		}
		AccessControl component4 = base.GetComponent<AccessControl>();
		if (component4 != null)
		{
			component4.SetRegistered(true);
		}
		ManualDeliveryKG[] components2 = base.GetComponents<ManualDeliveryKG>();
		for (int i = 0; i < components2.Length; i++)
		{
			components2[i].Pause(false, "Landing on world");
		}
		BuildingConduitEndpoints[] components3 = base.GetComponents<BuildingConduitEndpoints>();
		for (int i = 0; i < components3.Length; i++)
		{
			components3[i].AddEndpoint();
		}
		ReorderableBuilding component5 = base.GetComponent<ReorderableBuilding>();
		if (component5 != null)
		{
			component5.ShowReorderArm(true);
		}
		Workable component6 = base.GetComponent<Workable>();
		if (component6 != null)
		{
			component6.RefreshReachability();
		}
		Structure component7 = base.GetComponent<Structure>();
		if (component7 != null)
		{
			component7.UpdatePosition();
		}
		WireUtilitySemiVirtualNetworkLink component8 = base.GetComponent<WireUtilitySemiVirtualNetworkLink>();
		if (component8 != null)
		{
			component8.SetLinkConnected(true);
		}
		PartialLightBlocking component9 = base.GetComponent<PartialLightBlocking>();
		if (component9 != null)
		{
			component9.SetLightBlocking();
		}
	}

	// Token: 0x06008826 RID: 34854 RVA: 0x00362130 File Offset: 0x00360330
	private void ToggleComponent(Type cmpType, bool enabled)
	{
		MonoBehaviour monoBehaviour = (MonoBehaviour)base.GetComponent(cmpType);
		if (monoBehaviour != null)
		{
			monoBehaviour.enabled = enabled;
		}
	}

	// Token: 0x06008827 RID: 34855 RVA: 0x000FD9DA File Offset: 0x000FBBDA
	public void RegisterWithConditionManager()
	{
		global::Debug.Assert(!DlcManager.FeatureClusterSpaceEnabled());
		if (this.conditionManager != null)
		{
			this.conditionManager.RegisterRocketModule(this);
		}
	}

	// Token: 0x06008828 RID: 34856 RVA: 0x000FDA03 File Offset: 0x000FBC03
	protected override void OnCleanUp()
	{
		if (this.conditionManager != null)
		{
			this.conditionManager.UnregisterRocketModule(this);
		}
		base.OnCleanUp();
	}

	// Token: 0x06008829 RID: 34857 RVA: 0x0036215C File Offset: 0x0036035C
	public virtual LaunchConditionManager FindLaunchConditionManager()
	{
		if (!DlcManager.FeatureClusterSpaceEnabled())
		{
			foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(base.GetComponent<AttachableBuilding>()))
			{
				LaunchConditionManager component = gameObject.GetComponent<LaunchConditionManager>();
				if (component != null)
				{
					return component;
				}
			}
		}
		return null;
	}

	// Token: 0x0600882A RID: 34858 RVA: 0x000FDA25 File Offset: 0x000FBC25
	public void SetParentRocketName(string newName)
	{
		this.parentRocketName = newName;
		NameDisplayScreen.Instance.UpdateName(base.gameObject);
	}

	// Token: 0x0600882B RID: 34859 RVA: 0x000FDA3E File Offset: 0x000FBC3E
	public virtual string GetParentRocketName()
	{
		return this.parentRocketName;
	}

	// Token: 0x0600882C RID: 34860 RVA: 0x003621CC File Offset: 0x003603CC
	public void MoveToSpace()
	{
		Prioritizable component = base.GetComponent<Prioritizable>();
		if (component != null && component.GetMyWorld() != null)
		{
			component.GetMyWorld().RemoveTopPriorityPrioritizable(component);
		}
		int cell = Grid.PosToCell(base.transform.GetPosition());
		Building component2 = base.GetComponent<Building>();
		component2.Def.UnmarkArea(cell, component2.Orientation, component2.Def.ObjectLayer, base.gameObject);
		Vector3 position = new Vector3(-1f, -1f, 0f);
		base.gameObject.transform.SetPosition(position);
		LogicPorts component3 = base.GetComponent<LogicPorts>();
		if (component3 != null)
		{
			component3.OnMove();
		}
		base.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.Entombed, false, this);
	}

	// Token: 0x0600882D RID: 34861 RVA: 0x0036229C File Offset: 0x0036049C
	public void MoveToPad(int newCell)
	{
		base.gameObject.transform.SetPosition(Grid.CellToPos(newCell, CellAlignment.Bottom, Grid.SceneLayer.Building));
		int cell = Grid.PosToCell(base.transform.GetPosition());
		Building component = base.GetComponent<Building>();
		component.RefreshCells();
		component.Def.MarkArea(cell, component.Orientation, component.Def.ObjectLayer, base.gameObject);
		LogicPorts component2 = base.GetComponent<LogicPorts>();
		if (component2 != null)
		{
			component2.OnMove();
		}
		Prioritizable component3 = base.GetComponent<Prioritizable>();
		if (component3 != null && component3.IsTopPriority())
		{
			component3.GetMyWorld().AddTopPriorityPrioritizable(component3);
		}
	}

	// Token: 0x0400672C RID: 26412
	public LaunchConditionManager conditionManager;

	// Token: 0x0400672D RID: 26413
	public Dictionary<ProcessCondition.ProcessConditionType, List<ProcessCondition>> moduleConditions = new Dictionary<ProcessCondition.ProcessConditionType, List<ProcessCondition>>();

	// Token: 0x0400672E RID: 26414
	public static readonly Operational.Flag landedFlag = new Operational.Flag("landed", Operational.Flag.Type.Requirement);

	// Token: 0x0400672F RID: 26415
	public bool operationalLandedRequired = true;

	// Token: 0x04006730 RID: 26416
	private string rocket_module_bg_base_string = "{0}{1}";

	// Token: 0x04006731 RID: 26417
	private string rocket_module_bg_affix = "BG";

	// Token: 0x04006732 RID: 26418
	private string rocket_module_bg_anim = "on";

	// Token: 0x04006733 RID: 26419
	[SerializeField]
	private KAnimFile bgAnimFile;

	// Token: 0x04006734 RID: 26420
	protected string parentRocketName = UI.STARMAP.DEFAULT_NAME;

	// Token: 0x04006735 RID: 26421
	private static readonly EventSystem.IntraObjectHandler<RocketModule> DEBUG_OnDestroyDelegate = new EventSystem.IntraObjectHandler<RocketModule>(delegate(RocketModule component, object data)
	{
		component.DEBUG_OnDestroy(data);
	});

	// Token: 0x04006736 RID: 26422
	private static readonly EventSystem.IntraObjectHandler<RocketModule> OnRocketOnGroundTagDelegate = GameUtil.CreateHasTagHandler<RocketModule>(GameTags.RocketOnGround, delegate(RocketModule component, object data)
	{
		component.OnRocketOnGroundTag(data);
	});

	// Token: 0x04006737 RID: 26423
	private static readonly EventSystem.IntraObjectHandler<RocketModule> OnRocketNotOnGroundTagDelegate = GameUtil.CreateHasTagHandler<RocketModule>(GameTags.RocketNotOnGround, delegate(RocketModule component, object data)
	{
		component.OnRocketNotOnGroundTag(data);
	});
}
