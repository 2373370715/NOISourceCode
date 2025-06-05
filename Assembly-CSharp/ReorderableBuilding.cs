using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000F90 RID: 3984
public class ReorderableBuilding : KMonoBehaviour
{
	// Token: 0x06005027 RID: 20519 RVA: 0x0027BD08 File Offset: 0x00279F08
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.animController = base.GetComponent<KBatchedAnimController>();
		base.Subscribe(2127324410, new Action<object>(this.OnCancel));
		GameObject gameObject = new GameObject();
		gameObject.name = "ReorderArm";
		gameObject.transform.SetParent(base.transform);
		gameObject.transform.SetLocalPosition(Vector3.up * Grid.CellSizeInMeters * ((float)base.GetComponent<Building>().Def.HeightInCells / 2f - 0.5f));
		gameObject.transform.SetPosition(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, Grid.GetLayerZ(Grid.SceneLayer.BuildingBack)));
		gameObject.SetActive(false);
		this.reorderArmController = gameObject.AddComponent<KBatchedAnimController>();
		this.reorderArmController.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("rocket_module_switching_arm_kanim")
		};
		this.reorderArmController.initialAnim = "off";
		gameObject.SetActive(true);
		int cell = Grid.PosToCell(gameObject);
		this.ShowReorderArm(Grid.IsValidCell(cell));
		RocketModuleCluster component = base.GetComponent<RocketModuleCluster>();
		if (component != null)
		{
			LaunchPad currentPad = component.CraftInterface.CurrentPad;
			if (currentPad != null)
			{
				this.m_animLink = new KAnimLink(currentPad.GetComponent<KAnimControllerBase>(), this.reorderArmController);
			}
		}
		if (this.m_animLink == null)
		{
			this.m_animLink = new KAnimLink(base.GetComponent<KAnimControllerBase>(), this.reorderArmController);
		}
	}

	// Token: 0x06005028 RID: 20520 RVA: 0x000D8A68 File Offset: 0x000D6C68
	private void OnCancel(object data)
	{
		if (base.GetComponent<BuildingUnderConstruction>() != null && !this.cancelShield && !ReorderableBuilding.toBeRemoved.Contains(this))
		{
			ReorderableBuilding.toBeRemoved.Add(this);
		}
	}

	// Token: 0x06005029 RID: 20521 RVA: 0x0027BE94 File Offset: 0x0027A094
	public GameObject AddModule(BuildingDef def, IList<Tag> buildMaterials)
	{
		if (Assets.GetPrefab(base.GetComponent<KPrefabID>().PrefabID()).GetComponent<ReorderableBuilding>().buildConditions.Find((SelectModuleCondition match) => match is TopOnly) == null)
		{
			if (def.BuildingComplete.GetComponent<ReorderableBuilding>().buildConditions.Find((SelectModuleCondition match) => match is EngineOnBottom) == null)
			{
				return this.AddModuleAbove(def, buildMaterials);
			}
		}
		return this.AddModuleBelow(def, buildMaterials);
	}

	// Token: 0x0600502A RID: 20522 RVA: 0x0027BF28 File Offset: 0x0027A128
	private GameObject AddModuleAbove(BuildingDef def, IList<Tag> buildMaterials)
	{
		BuildingAttachPoint component = base.GetComponent<BuildingAttachPoint>();
		if (component == null)
		{
			return null;
		}
		BuildingAttachPoint.HardPoint hardPoint = component.points[0];
		int cell = Grid.OffsetCell(Grid.PosToCell(base.gameObject), hardPoint.position);
		int heightInCells = def.HeightInCells;
		if (hardPoint.attachedBuilding != null)
		{
			if (!hardPoint.attachedBuilding.GetComponent<ReorderableBuilding>().CanMoveVertically(heightInCells, null))
			{
				return null;
			}
			hardPoint.attachedBuilding.GetComponent<ReorderableBuilding>().MoveVertical(heightInCells);
		}
		return this.AddModuleCommon(def, buildMaterials, cell);
	}

	// Token: 0x0600502B RID: 20523 RVA: 0x0027BFB0 File Offset: 0x0027A1B0
	private GameObject AddModuleBelow(BuildingDef def, IList<Tag> buildMaterials)
	{
		int cell = Grid.PosToCell(base.gameObject);
		int heightInCells = def.HeightInCells;
		if (!this.CanMoveVertically(heightInCells, null))
		{
			return null;
		}
		this.MoveVertical(heightInCells);
		return this.AddModuleCommon(def, buildMaterials, cell);
	}

	// Token: 0x0600502C RID: 20524 RVA: 0x0027BFEC File Offset: 0x0027A1EC
	private GameObject AddModuleCommon(BuildingDef def, IList<Tag> buildMaterials, int cell)
	{
		GameObject gameObject;
		if (DebugHandler.InstantBuildMode || (Game.Instance.SandboxModeActive && SandboxToolParameterMenu.instance.settings.InstantBuild))
		{
			gameObject = def.Build(cell, Orientation.Neutral, null, buildMaterials, 273.15f, true, GameClock.Instance.GetTime());
		}
		else
		{
			gameObject = def.TryPlace(null, Grid.CellToPosCBC(cell, def.SceneLayer), Orientation.Neutral, buildMaterials, 0);
		}
		ReorderableBuilding.RebuildNetworks();
		this.RocketSpecificPostAdd(gameObject, cell);
		return gameObject;
	}

	// Token: 0x0600502D RID: 20525 RVA: 0x0027C060 File Offset: 0x0027A260
	private void RocketSpecificPostAdd(GameObject obj, int cell)
	{
		RocketModuleCluster component = base.GetComponent<RocketModuleCluster>();
		RocketModuleCluster component2 = obj.GetComponent<RocketModuleCluster>();
		if (component != null && component2 != null)
		{
			component.CraftInterface.AddModule(component2);
		}
	}

	// Token: 0x0600502E RID: 20526 RVA: 0x0027C09C File Offset: 0x0027A29C
	public void RemoveModule()
	{
		BuildingAttachPoint component = base.GetComponent<BuildingAttachPoint>();
		AttachableBuilding attachableBuilding = null;
		if (component != null && component.points[0].attachedBuilding != null)
		{
			attachableBuilding = component.points[0].attachedBuilding;
		}
		int heightInCells = base.GetComponent<Building>().Def.HeightInCells;
		if (base.GetComponent<Deconstructable>() != null)
		{
			base.GetComponent<Deconstructable>().CompleteWork(null);
		}
		if (base.GetComponent<BuildingUnderConstruction>() != null)
		{
			this.DeleteObject();
		}
		Building component2 = base.GetComponent<Building>();
		component2.Def.UnmarkArea(Grid.PosToCell(this), component2.Orientation, component2.Def.ObjectLayer, base.gameObject);
		if (attachableBuilding != null)
		{
			ReorderableBuilding component3 = attachableBuilding.GetComponent<ReorderableBuilding>();
			if (component3 != null)
			{
				component3.MoveVertical(-heightInCells);
			}
		}
	}

	// Token: 0x0600502F RID: 20527 RVA: 0x0027C178 File Offset: 0x0027A378
	public void LateUpdate()
	{
		this.cancelShield = false;
		ReorderableBuilding.ProcessToBeRemoved();
		if (this.reorderingAnimUnderway)
		{
			float num = 10f;
			if (Mathf.Abs(this.animController.Offset.y) < Time.unscaledDeltaTime * num)
			{
				this.animController.Offset = new Vector3(this.animController.Offset.x, 0f, this.animController.Offset.z);
				this.reorderingAnimUnderway = false;
				string s = base.GetComponent<Building>().Def.WidthInCells.ToString() + "x" + base.GetComponent<Building>().Def.HeightInCells.ToString() + "_ungrab";
				if (!this.reorderArmController.HasAnimation(s))
				{
					s = "3x3_ungrab";
				}
				this.reorderArmController.Play(s, KAnim.PlayMode.Once, 1f, 0f);
				this.reorderArmController.Queue("off", KAnim.PlayMode.Once, 1f, 0f);
				this.loopingSounds.StopSound(GlobalAssets.GetSound(this.reorderSound, false));
			}
			else if (this.animController.Offset.y > 0f)
			{
				this.animController.Offset = new Vector3(this.animController.Offset.x, this.animController.Offset.y - Time.unscaledDeltaTime * num, this.animController.Offset.z);
			}
			else if (this.animController.Offset.y < 0f)
			{
				this.animController.Offset = new Vector3(this.animController.Offset.x, this.animController.Offset.y + Time.unscaledDeltaTime * num, this.animController.Offset.z);
			}
			this.reorderArmController.Offset = this.animController.Offset;
		}
	}

	// Token: 0x06005030 RID: 20528 RVA: 0x0027C380 File Offset: 0x0027A580
	private static void ProcessToBeRemoved()
	{
		if (ReorderableBuilding.toBeRemoved.Count > 0)
		{
			ReorderableBuilding.toBeRemoved.Sort(delegate(ReorderableBuilding a, ReorderableBuilding b)
			{
				if (a.transform.position.y < b.transform.position.y)
				{
					return -1;
				}
				return 1;
			});
			for (int i = 0; i < ReorderableBuilding.toBeRemoved.Count; i++)
			{
				ReorderableBuilding.toBeRemoved[i].RemoveModule();
			}
			ReorderableBuilding.toBeRemoved.Clear();
		}
	}

	// Token: 0x06005031 RID: 20529 RVA: 0x0027C3F4 File Offset: 0x0027A5F4
	public void MoveVertical(int amount)
	{
		if (amount == 0)
		{
			return;
		}
		this.cancelShield = true;
		List<GameObject> list = new List<GameObject>();
		list.Add(base.gameObject);
		AttachableBuilding.GetAttachedAbove(base.GetComponent<AttachableBuilding>(), ref list);
		if (amount > 0)
		{
			list.Reverse();
		}
		foreach (GameObject gameObject in list)
		{
			ReorderableBuilding.UnmarkBuilding(gameObject, null);
			int cell = Grid.OffsetCell(Grid.PosToCell(gameObject), 0, amount);
			gameObject.transform.SetPosition(Grid.CellToPos(cell, CellAlignment.Bottom, Grid.SceneLayer.BuildingFront));
			ReorderableBuilding.MarkBuilding(gameObject, null);
			gameObject.GetComponent<ReorderableBuilding>().ApplyAnimOffset((float)(-(float)amount));
		}
		if (amount > 0)
		{
			foreach (GameObject gameObject2 in list)
			{
				gameObject2.GetComponent<AttachableBuilding>().RegisterWithAttachPoint(true);
			}
		}
	}

	// Token: 0x06005032 RID: 20530 RVA: 0x0027C4F0 File Offset: 0x0027A6F0
	public void SwapWithAbove(bool selectOnComplete = true)
	{
		BuildingAttachPoint component = base.GetComponent<BuildingAttachPoint>();
		if (component == null || component.points[0].attachedBuilding == null)
		{
			return;
		}
		int num = Grid.PosToCell(base.gameObject);
		ReorderableBuilding.UnmarkBuilding(base.gameObject, null);
		AttachableBuilding attachedBuilding = component.points[0].attachedBuilding;
		BuildingAttachPoint component2 = attachedBuilding.GetComponent<BuildingAttachPoint>();
		AttachableBuilding aboveBuilding = (component2 != null) ? component2.points[0].attachedBuilding : null;
		ReorderableBuilding.UnmarkBuilding(attachedBuilding.gameObject, aboveBuilding);
		Building component3 = attachedBuilding.GetComponent<Building>();
		int cell = num;
		attachedBuilding.transform.SetPosition(Grid.CellToPos(cell, CellAlignment.Bottom, Grid.SceneLayer.BuildingFront));
		ReorderableBuilding.MarkBuilding(attachedBuilding.gameObject, null);
		int cell2 = Grid.OffsetCell(num, 0, component3.Def.HeightInCells);
		base.transform.SetPosition(Grid.CellToPos(cell2, CellAlignment.Bottom, Grid.SceneLayer.BuildingFront));
		ReorderableBuilding.MarkBuilding(base.gameObject, aboveBuilding);
		ReorderableBuilding.RebuildNetworks();
		this.ApplyAnimOffset((float)(-(float)component3.Def.HeightInCells));
		Building component4 = base.GetComponent<Building>();
		component3.GetComponent<ReorderableBuilding>().ApplyAnimOffset((float)component4.Def.HeightInCells);
		if (selectOnComplete)
		{
			SelectTool.Instance.Select(component4.GetComponent<KSelectable>(), false);
		}
	}

	// Token: 0x06005033 RID: 20531 RVA: 0x000D8A98 File Offset: 0x000D6C98
	protected override void OnCleanUp()
	{
		if (base.GetComponent<BuildingUnderConstruction>() == null && !this.HasTag(GameTags.RocketInSpace))
		{
			this.RemoveModule();
		}
		if (this.m_animLink != null)
		{
			this.m_animLink.Unregister();
		}
		base.OnCleanUp();
	}

	// Token: 0x06005034 RID: 20532 RVA: 0x0027C634 File Offset: 0x0027A834
	private void ApplyAnimOffset(float amount)
	{
		this.animController.Offset = new Vector3(this.animController.Offset.x, this.animController.Offset.y + amount, this.animController.Offset.z);
		this.reorderArmController.Offset = this.animController.Offset;
		string s = base.GetComponent<Building>().Def.WidthInCells.ToString() + "x" + base.GetComponent<Building>().Def.HeightInCells.ToString() + "_grab";
		if (!this.reorderArmController.HasAnimation(s))
		{
			s = "3x3_grab";
		}
		this.reorderArmController.Play(s, KAnim.PlayMode.Once, 1f, 0f);
		this.reorderArmController.onAnimComplete += this.StartReorderingAnim;
	}

	// Token: 0x06005035 RID: 20533 RVA: 0x0027C720 File Offset: 0x0027A920
	private void StartReorderingAnim(HashedString data)
	{
		this.loopingSounds.StartSound(GlobalAssets.GetSound(this.reorderSound, false));
		this.reorderingAnimUnderway = true;
		this.reorderArmController.onAnimComplete -= this.StartReorderingAnim;
		base.gameObject.Trigger(-1447108533, null);
	}

	// Token: 0x06005036 RID: 20534 RVA: 0x0027C774 File Offset: 0x0027A974
	public void SwapWithBelow(bool selectOnComplete = true)
	{
		if (base.GetComponent<AttachableBuilding>() == null || base.GetComponent<AttachableBuilding>().GetAttachedTo() == null)
		{
			return;
		}
		base.GetComponent<AttachableBuilding>().GetAttachedTo().GetComponent<ReorderableBuilding>().SwapWithAbove(!selectOnComplete);
		if (selectOnComplete)
		{
			SelectTool.Instance.Select(base.GetComponent<KSelectable>(), false);
		}
	}

	// Token: 0x06005037 RID: 20535 RVA: 0x0027C7D0 File Offset: 0x0027A9D0
	public bool CanMoveVertically(int moveAmount, GameObject ignoreBuilding = null)
	{
		if (moveAmount == 0)
		{
			return true;
		}
		BuildingAttachPoint component = base.GetComponent<BuildingAttachPoint>();
		AttachableBuilding component2 = base.GetComponent<AttachableBuilding>();
		if (moveAmount > 0)
		{
			if (component != null && component.points[0].attachedBuilding != null && component.points[0].attachedBuilding.gameObject != ignoreBuilding && component.points[0].attachedBuilding.HasTag(GameTags.RocketModule) && !component.points[0].attachedBuilding.GetComponent<ReorderableBuilding>().CanMoveVertically(moveAmount, null))
			{
				return false;
			}
		}
		else if (component2 != null)
		{
			BuildingAttachPoint attachedTo = component2.GetAttachedTo();
			if (attachedTo != null && attachedTo.gameObject != ignoreBuilding && !component2.GetAttachedTo().GetComponent<ReorderableBuilding>().CanMoveVertically(moveAmount, null))
			{
				return false;
			}
		}
		foreach (CellOffset offset in this.GetOccupiedOffsets())
		{
			if (!ReorderableBuilding.CheckCellClear(Grid.OffsetCell(Grid.OffsetCell(Grid.PosToCell(base.gameObject), offset), 0, moveAmount), base.gameObject))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06005038 RID: 20536 RVA: 0x0027C904 File Offset: 0x0027AB04
	public static bool CheckCellClear(int checkCell, GameObject ignoreObject = null)
	{
		return Grid.IsValidCell(checkCell) && Grid.IsValidBuildingCell(checkCell) && !Grid.Solid[checkCell] && Grid.WorldIdx[checkCell] != byte.MaxValue && (!(Grid.Objects[checkCell, 1] != null) || !(Grid.Objects[checkCell, 1] != ignoreObject) || !(Grid.Objects[checkCell, 1].GetComponent<ReorderableBuilding>() == null));
	}

	// Token: 0x06005039 RID: 20537 RVA: 0x0027C984 File Offset: 0x0027AB84
	public GameObject ConvertModule(BuildingDef toModule, IList<Tag> materials)
	{
		int cell = Grid.PosToCell(base.gameObject);
		int num = toModule.HeightInCells - base.GetComponent<Building>().Def.HeightInCells;
		base.gameObject.GetComponent<Building>();
		BuildingAttachPoint component = base.gameObject.GetComponent<BuildingAttachPoint>();
		GameObject gameObject = null;
		if (component != null && component.points[0].attachedBuilding != null)
		{
			gameObject = component.points[0].attachedBuilding.gameObject;
			component.points[0].attachedBuilding = null;
			Components.BuildingAttachPoints.Remove(component);
		}
		ReorderableBuilding.UnmarkBuilding(base.gameObject, null);
		if (num != 0 && gameObject != null)
		{
			gameObject.GetComponent<ReorderableBuilding>().MoveVertical(num);
		}
		string text;
		if (!DebugHandler.InstantBuildMode && !toModule.IsValidPlaceLocation(base.gameObject, cell, Orientation.Neutral, out text))
		{
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Building, text, base.transform, 1.5f, false);
			if (num != 0 && gameObject != null)
			{
				num *= -1;
				gameObject.GetComponent<ReorderableBuilding>().MoveVertical(num);
			}
			ReorderableBuilding.MarkBuilding(base.gameObject, (gameObject != null) ? gameObject.GetComponent<AttachableBuilding>() : null);
			if (component != null && gameObject != null)
			{
				component.points[0].attachedBuilding = gameObject.GetComponent<AttachableBuilding>();
				Components.BuildingAttachPoints.Add(component);
			}
			return null;
		}
		if (materials == null)
		{
			materials = toModule.DefaultElements();
		}
		GameObject gameObject2;
		if (DebugHandler.InstantBuildMode || (Game.Instance.SandboxModeActive && SandboxToolParameterMenu.instance.settings.InstantBuild))
		{
			gameObject2 = toModule.Build(cell, Orientation.Neutral, null, materials, 273.15f, true, GameClock.Instance.GetTime());
		}
		else
		{
			gameObject2 = toModule.TryPlace(base.gameObject, Grid.CellToPosCBC(cell, toModule.SceneLayer), Orientation.Neutral, materials, 0);
		}
		RocketModuleCluster component2 = base.GetComponent<RocketModuleCluster>();
		RocketModuleCluster component3 = gameObject2.GetComponent<RocketModuleCluster>();
		if (component2 != null && component3 != null)
		{
			component2.CraftInterface.AddModule(component3);
		}
		Deconstructable component4 = base.GetComponent<Deconstructable>();
		if (component4 != null)
		{
			component4.SetAllowDeconstruction(true);
			component4.ForceDestroyAndGetMaterials();
		}
		else
		{
			Util.KDestroyGameObject(base.gameObject);
		}
		return gameObject2;
	}

	// Token: 0x0600503A RID: 20538 RVA: 0x0027CBCC File Offset: 0x0027ADCC
	private CellOffset[] GetOccupiedOffsets()
	{
		OccupyArea component = base.GetComponent<OccupyArea>();
		if (component != null)
		{
			return component.OccupiedCellsOffsets;
		}
		return base.GetComponent<BuildingUnderConstruction>().Def.PlacementOffsets;
	}

	// Token: 0x0600503B RID: 20539 RVA: 0x0027CC00 File Offset: 0x0027AE00
	public bool CanChangeModule()
	{
		if (base.GetComponent<BuildingUnderConstruction>() != null)
		{
			string prefabID = base.GetComponent<BuildingUnderConstruction>().Def.PrefabID;
		}
		else
		{
			string prefabID2 = base.GetComponent<Building>().Def.PrefabID;
		}
		RocketModuleCluster component = base.GetComponent<RocketModuleCluster>();
		if (component != null)
		{
			if (component.CraftInterface != null)
			{
				if (component.CraftInterface.GetComponent<Clustercraft>().Status != Clustercraft.CraftStatus.Grounded)
				{
					return false;
				}
			}
			else if (component.conditionManager != null && SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(component.conditionManager).state != Spacecraft.MissionState.Grounded)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x0600503C RID: 20540 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public bool CanRemoveModule()
	{
		return true;
	}

	// Token: 0x0600503D RID: 20541 RVA: 0x0027CC9C File Offset: 0x0027AE9C
	public bool CanSwapUp(bool alsoCheckAboveCanSwapDown = true)
	{
		BuildingAttachPoint component = base.GetComponent<BuildingAttachPoint>();
		if (component == null)
		{
			return false;
		}
		if (base.GetComponent<AttachableBuilding>() == null || base.GetComponent<RocketEngineCluster>() != null)
		{
			return false;
		}
		AttachableBuilding attachedBuilding = component.points[0].attachedBuilding;
		return !(attachedBuilding == null) && !(attachedBuilding.GetComponent<BuildingAttachPoint>() == null) && !attachedBuilding.HasTag(GameTags.NoseRocketModule) && this.CanMoveVertically(attachedBuilding.GetComponent<Building>().Def.HeightInCells, attachedBuilding.gameObject) && (!alsoCheckAboveCanSwapDown || attachedBuilding.GetComponent<ReorderableBuilding>().CanSwapDown(false));
	}

	// Token: 0x0600503E RID: 20542 RVA: 0x0027CD48 File Offset: 0x0027AF48
	public bool CanSwapDown(bool alsoCheckBelowCanSwapUp = true)
	{
		if (base.gameObject.HasTag(GameTags.NoseRocketModule))
		{
			return false;
		}
		AttachableBuilding component = base.GetComponent<AttachableBuilding>();
		if (component == null)
		{
			return false;
		}
		BuildingAttachPoint attachedTo = component.GetAttachedTo();
		return !(attachedTo == null) && !(base.GetComponent<BuildingAttachPoint>() == null) && !(attachedTo.GetComponent<AttachableBuilding>() == null) && !(attachedTo.GetComponent<RocketEngineCluster>() != null) && this.CanMoveVertically(attachedTo.GetComponent<Building>().Def.HeightInCells * -1, attachedTo.gameObject) && (!alsoCheckBelowCanSwapUp || attachedTo.GetComponent<ReorderableBuilding>().CanSwapUp(false));
	}

	// Token: 0x0600503F RID: 20543 RVA: 0x000D8AD4 File Offset: 0x000D6CD4
	public void ShowReorderArm(bool show)
	{
		if (this.reorderArmController != null)
		{
			this.reorderArmController.gameObject.SetActive(show);
		}
	}

	// Token: 0x06005040 RID: 20544 RVA: 0x0027CDF4 File Offset: 0x0027AFF4
	private static void RebuildNetworks()
	{
		Game.Instance.logicCircuitSystem.ForceRebuildNetworks();
		Game.Instance.gasConduitSystem.ForceRebuildNetworks();
		Game.Instance.liquidConduitSystem.ForceRebuildNetworks();
		Game.Instance.electricalConduitSystem.ForceRebuildNetworks();
		Game.Instance.solidConduitSystem.ForceRebuildNetworks();
	}

	// Token: 0x06005041 RID: 20545 RVA: 0x0027CE4C File Offset: 0x0027B04C
	private static void UnmarkBuilding(GameObject go, AttachableBuilding aboveBuilding)
	{
		int cell = Grid.PosToCell(go);
		Building component = go.GetComponent<Building>();
		component.Def.UnmarkArea(cell, component.Orientation, component.Def.ObjectLayer, go);
		AttachableBuilding component2 = go.GetComponent<AttachableBuilding>();
		if (component2 != null)
		{
			component2.RegisterWithAttachPoint(false);
		}
		if (aboveBuilding != null)
		{
			aboveBuilding.RegisterWithAttachPoint(false);
		}
		RocketModule component3 = go.GetComponent<RocketModule>();
		if (component3 != null)
		{
			component3.DeregisterComponents();
		}
		RocketConduitSender[] components = go.GetComponents<RocketConduitSender>();
		if (components.Length != 0)
		{
			RocketConduitSender[] array = components;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].RemoveConduitPortFromNetwork();
			}
		}
		RocketConduitReceiver[] components2 = go.GetComponents<RocketConduitReceiver>();
		if (components2.Length != 0)
		{
			RocketConduitReceiver[] array2 = components2;
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].RemoveConduitPortFromNetwork();
			}
		}
	}

	// Token: 0x06005042 RID: 20546 RVA: 0x0027CF20 File Offset: 0x0027B120
	private static void MarkBuilding(GameObject go, AttachableBuilding aboveBuilding)
	{
		int cell = Grid.PosToCell(go);
		Building component = go.GetComponent<Building>();
		component.Def.MarkArea(cell, component.Orientation, component.Def.ObjectLayer, go);
		if (component.GetComponent<OccupyArea>() != null)
		{
			component.GetComponent<OccupyArea>().UpdateOccupiedArea();
		}
		LogicPorts component2 = component.GetComponent<LogicPorts>();
		if (component2 && go.GetComponent<BuildingComplete>() != null)
		{
			component2.OnMove();
		}
		component.GetComponent<AttachableBuilding>().RegisterWithAttachPoint(true);
		if (aboveBuilding != null)
		{
			aboveBuilding.RegisterWithAttachPoint(true);
		}
		RocketModule component3 = go.GetComponent<RocketModule>();
		if (component3 != null)
		{
			component3.RegisterComponents();
		}
		VerticalModuleTiler component4 = go.GetComponent<VerticalModuleTiler>();
		if (component4 != null)
		{
			component4.PostReorderMove();
		}
		RocketConduitSender[] components = go.GetComponents<RocketConduitSender>();
		if (components.Length != 0)
		{
			RocketConduitSender[] array = components;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].AddConduitPortToNetwork();
			}
		}
		RocketConduitReceiver[] components2 = go.GetComponents<RocketConduitReceiver>();
		if (components2.Length != 0)
		{
			RocketConduitReceiver[] array2 = components2;
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].AddConduitPortToNetwork();
			}
		}
	}

	// Token: 0x04003881 RID: 14465
	private bool cancelShield;

	// Token: 0x04003882 RID: 14466
	private bool reorderingAnimUnderway;

	// Token: 0x04003883 RID: 14467
	private KBatchedAnimController animController;

	// Token: 0x04003884 RID: 14468
	public List<SelectModuleCondition> buildConditions = new List<SelectModuleCondition>();

	// Token: 0x04003885 RID: 14469
	private KBatchedAnimController reorderArmController;

	// Token: 0x04003886 RID: 14470
	private KAnimLink m_animLink;

	// Token: 0x04003887 RID: 14471
	[MyCmpAdd]
	private LoopingSounds loopingSounds;

	// Token: 0x04003888 RID: 14472
	private string reorderSound = "RocketModuleSwitchingArm_moving_LP";

	// Token: 0x04003889 RID: 14473
	private static List<ReorderableBuilding> toBeRemoved = new List<ReorderableBuilding>();

	// Token: 0x02000F91 RID: 3985
	public enum MoveSource
	{
		// Token: 0x0400388B RID: 14475
		Push,
		// Token: 0x0400388C RID: 14476
		Pull
	}
}
