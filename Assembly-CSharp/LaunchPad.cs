using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using UnityEngine;

// Token: 0x0200194D RID: 6477
public class LaunchPad : KMonoBehaviour, ISim1000ms, IListableOption, IProcessConditionSet
{
	// Token: 0x170008CC RID: 2252
	// (get) Token: 0x060086C9 RID: 34505 RVA: 0x0035B2C8 File Offset: 0x003594C8
	public RocketModuleCluster LandedRocket
	{
		get
		{
			GameObject gameObject = null;
			Grid.ObjectLayers[1].TryGetValue(this.RocketBottomPosition, out gameObject);
			if (gameObject != null)
			{
				RocketModuleCluster component = gameObject.GetComponent<RocketModuleCluster>();
				Clustercraft clustercraft = (component != null && component.CraftInterface != null) ? component.CraftInterface.GetComponent<Clustercraft>() : null;
				if (clustercraft != null && (clustercraft.Status == Clustercraft.CraftStatus.Grounded || clustercraft.Status == Clustercraft.CraftStatus.Landing))
				{
					return component;
				}
			}
			return null;
		}
	}

	// Token: 0x170008CD RID: 2253
	// (get) Token: 0x060086CA RID: 34506 RVA: 0x000FCF17 File Offset: 0x000FB117
	public int RocketBottomPosition
	{
		get
		{
			return Grid.OffsetCell(Grid.PosToCell(this), this.baseModulePosition);
		}
	}

	// Token: 0x060086CB RID: 34507 RVA: 0x0035B340 File Offset: 0x00359540
	[OnDeserialized]
	private void OnDeserialzed()
	{
		if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 24))
		{
			Building component = base.GetComponent<Building>();
			if (component != null)
			{
				component.RunOnArea(delegate(int cell)
				{
					if (Grid.IsValidCell(cell) && Grid.Solid[cell])
					{
						SimMessages.ReplaceElement(cell, SimHashes.Vacuum, CellEventLogger.Instance.LaunchpadDesolidify, 0f, -1f, byte.MaxValue, 0, -1);
					}
				});
			}
		}
	}

	// Token: 0x060086CC RID: 34508 RVA: 0x0035B39C File Offset: 0x0035959C
	protected override void OnPrefabInit()
	{
		UserNameable component = base.GetComponent<UserNameable>();
		if (component != null)
		{
			component.SetName(GameUtil.GenerateRandomLaunchPadName());
		}
	}

	// Token: 0x060086CD RID: 34509 RVA: 0x0035B3C4 File Offset: 0x003595C4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.tower = new LaunchPad.LaunchPadTower(this, this.maxTowerHeight);
		this.OnRocketBuildingChanged(this.GetRocketBaseModule());
		this.partitionerEntry = GameScenePartitioner.Instance.Add("LaunchPad.OnSpawn", base.gameObject, Extents.OneCell(this.RocketBottomPosition), GameScenePartitioner.Instance.objectLayers[1], new Action<object>(this.OnRocketBuildingChanged));
		Components.LaunchPads.Add(this);
		this.CheckLandedRocketPassengerModuleStatus();
		int num = ConditionFlightPathIsClear.PadTopEdgeDistanceToCeilingEdge(base.gameObject);
		if (num < 35)
		{
			base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.RocketPlatformCloseToCeiling, num);
		}
	}

	// Token: 0x060086CE RID: 34510 RVA: 0x0035B478 File Offset: 0x00359678
	protected override void OnCleanUp()
	{
		Components.LaunchPads.Remove(this);
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		if (this.lastBaseAttachable != null)
		{
			AttachableBuilding attachableBuilding = this.lastBaseAttachable;
			attachableBuilding.onAttachmentNetworkChanged = (Action<object>)Delegate.Remove(attachableBuilding.onAttachmentNetworkChanged, new Action<object>(this.OnRocketLayoutChanged));
			this.lastBaseAttachable = null;
		}
		this.RebuildLaunchTowerHeightHandler.ClearScheduler();
		base.OnCleanUp();
	}

	// Token: 0x060086CF RID: 34511 RVA: 0x0035B4F0 File Offset: 0x003596F0
	private void CheckLandedRocketPassengerModuleStatus()
	{
		if (this.LandedRocket == null)
		{
			base.GetComponent<KSelectable>().RemoveStatusItem(this.landedRocketPassengerModuleStatusItem, false);
			this.landedRocketPassengerModuleStatusItem = Guid.Empty;
			return;
		}
		if (this.LandedRocket.CraftInterface.GetPassengerModule() == null)
		{
			if (this.landedRocketPassengerModuleStatusItem == Guid.Empty)
			{
				this.landedRocketPassengerModuleStatusItem = base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.LandedRocketLacksPassengerModule, null);
				return;
			}
		}
		else if (this.landedRocketPassengerModuleStatusItem != Guid.Empty)
		{
			base.GetComponent<KSelectable>().RemoveStatusItem(this.landedRocketPassengerModuleStatusItem, false);
			this.landedRocketPassengerModuleStatusItem = Guid.Empty;
		}
	}

	// Token: 0x060086D0 RID: 34512 RVA: 0x0035B5A8 File Offset: 0x003597A8
	public bool IsLogicInputConnected()
	{
		int portCell = base.GetComponent<LogicPorts>().GetPortCell(this.triggerPort);
		return Game.Instance.logicCircuitManager.GetNetworkForCell(portCell) != null;
	}

	// Token: 0x060086D1 RID: 34513 RVA: 0x0035B5DC File Offset: 0x003597DC
	public void Sim1000ms(float dt)
	{
		LogicPorts component = base.gameObject.GetComponent<LogicPorts>();
		RocketModuleCluster landedRocket = this.LandedRocket;
		if (landedRocket != null && this.IsLogicInputConnected())
		{
			if (component.GetInputValue(this.triggerPort) == 1)
			{
				if (landedRocket.CraftInterface.CheckReadyForAutomatedLaunchCommand())
				{
					landedRocket.CraftInterface.TriggerLaunch(true);
				}
				else
				{
					landedRocket.CraftInterface.CancelLaunch();
				}
			}
			else
			{
				landedRocket.CraftInterface.CancelLaunch();
			}
		}
		this.CheckLandedRocketPassengerModuleStatus();
		component.SendSignal(this.landedRocketPort, (landedRocket != null) ? 1 : 0);
		if (landedRocket != null)
		{
			component.SendSignal(this.statusPort, (landedRocket.CraftInterface.CheckReadyForAutomatedLaunch() || landedRocket.CraftInterface.HasTag(GameTags.RocketNotOnGround)) ? 1 : 0);
			return;
		}
		component.SendSignal(this.statusPort, 0);
	}

	// Token: 0x060086D2 RID: 34514 RVA: 0x0035B6B4 File Offset: 0x003598B4
	public GameObject AddBaseModule(BuildingDef moduleDefID, IList<Tag> elements)
	{
		int cell = Grid.OffsetCell(Grid.PosToCell(base.gameObject), this.baseModulePosition);
		GameObject gameObject;
		if (DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive)
		{
			gameObject = moduleDefID.Build(cell, Orientation.Neutral, null, elements, 293.15f, true, GameClock.Instance.GetTime());
		}
		else
		{
			gameObject = moduleDefID.TryPlace(null, Grid.CellToPosCBC(cell, moduleDefID.SceneLayer), Orientation.Neutral, elements, 0);
		}
		GameObject gameObject2 = Util.KInstantiate(Assets.GetPrefab("Clustercraft"), null, null);
		gameObject2.SetActive(true);
		Clustercraft component = gameObject2.GetComponent<Clustercraft>();
		component.GetComponent<CraftModuleInterface>().AddModule(gameObject.GetComponent<RocketModuleCluster>());
		component.Init(this.GetMyWorldLocation(), this);
		if (gameObject.GetComponent<BuildingUnderConstruction>() != null)
		{
			this.OnRocketBuildingChanged(gameObject);
		}
		base.Trigger(374403796, null);
		return gameObject;
	}

	// Token: 0x060086D3 RID: 34515 RVA: 0x0035B784 File Offset: 0x00359984
	private void OnRocketBuildingChanged(object data)
	{
		GameObject gameObject = (GameObject)data;
		RocketModuleCluster landedRocket = this.LandedRocket;
		global::Debug.Assert(gameObject == null || landedRocket == null || landedRocket.gameObject == gameObject, "Launch Pad had a rocket land or take off on it twice??");
		Clustercraft clustercraft = (landedRocket != null && landedRocket.CraftInterface != null) ? landedRocket.CraftInterface.GetComponent<Clustercraft>() : null;
		if (clustercraft != null)
		{
			if (clustercraft.Status == Clustercraft.CraftStatus.Landing)
			{
				base.Trigger(-887025858, landedRocket);
			}
			else if (clustercraft.Status == Clustercraft.CraftStatus.Launching)
			{
				base.Trigger(-1277991738, landedRocket);
				AttachableBuilding component = landedRocket.CraftInterface.ClusterModules[0].Get().GetComponent<AttachableBuilding>();
				component.onAttachmentNetworkChanged = (Action<object>)Delegate.Remove(component.onAttachmentNetworkChanged, new Action<object>(this.OnRocketLayoutChanged));
			}
		}
		this.OnRocketLayoutChanged(null);
	}

	// Token: 0x060086D4 RID: 34516 RVA: 0x0035B868 File Offset: 0x00359A68
	private void OnRocketLayoutChanged(object data)
	{
		if (this.lastBaseAttachable != null)
		{
			AttachableBuilding attachableBuilding = this.lastBaseAttachable;
			attachableBuilding.onAttachmentNetworkChanged = (Action<object>)Delegate.Remove(attachableBuilding.onAttachmentNetworkChanged, new Action<object>(this.OnRocketLayoutChanged));
			this.lastBaseAttachable = null;
		}
		GameObject rocketBaseModule = this.GetRocketBaseModule();
		if (rocketBaseModule != null)
		{
			this.lastBaseAttachable = rocketBaseModule.GetComponent<AttachableBuilding>();
			AttachableBuilding attachableBuilding2 = this.lastBaseAttachable;
			attachableBuilding2.onAttachmentNetworkChanged = (Action<object>)Delegate.Combine(attachableBuilding2.onAttachmentNetworkChanged, new Action<object>(this.OnRocketLayoutChanged));
		}
		this.DirtyTowerHeight();
	}

	// Token: 0x060086D5 RID: 34517 RVA: 0x000FCF2A File Offset: 0x000FB12A
	public bool HasRocket()
	{
		return this.LandedRocket != null;
	}

	// Token: 0x060086D6 RID: 34518 RVA: 0x000FCF38 File Offset: 0x000FB138
	public bool HasRocketWithCommandModule()
	{
		return this.HasRocket() && this.LandedRocket.CraftInterface.FindLaunchableRocket() != null;
	}

	// Token: 0x060086D7 RID: 34519 RVA: 0x0035B8FC File Offset: 0x00359AFC
	private GameObject GetRocketBaseModule()
	{
		GameObject gameObject = Grid.Objects[Grid.OffsetCell(Grid.PosToCell(base.gameObject), this.baseModulePosition), 1];
		if (!(gameObject != null) || !(gameObject.GetComponent<RocketModule>() != null))
		{
			return null;
		}
		return gameObject;
	}

	// Token: 0x060086D8 RID: 34520 RVA: 0x0035B948 File Offset: 0x00359B48
	public void DirtyTowerHeight()
	{
		if (!this.dirtyTowerHeight)
		{
			this.dirtyTowerHeight = true;
			if (!this.RebuildLaunchTowerHeightHandler.IsValid)
			{
				this.RebuildLaunchTowerHeightHandler = GameScheduler.Instance.ScheduleNextFrame("RebuildLaunchTowerHeight", new Action<object>(this.RebuildLaunchTowerHeight), null, null);
			}
		}
	}

	// Token: 0x060086D9 RID: 34521 RVA: 0x0035B994 File Offset: 0x00359B94
	private void RebuildLaunchTowerHeight(object obj)
	{
		RocketModuleCluster landedRocket = this.LandedRocket;
		if (landedRocket != null)
		{
			this.tower.SetTowerHeight(landedRocket.CraftInterface.MaxHeight);
		}
		this.dirtyTowerHeight = false;
		this.RebuildLaunchTowerHeightHandler.ClearScheduler();
	}

	// Token: 0x060086DA RID: 34522 RVA: 0x000C52B2 File Offset: 0x000C34B2
	public string GetProperName()
	{
		return base.gameObject.GetProperName();
	}

	// Token: 0x060086DB RID: 34523 RVA: 0x0035B9DC File Offset: 0x00359BDC
	public List<ProcessCondition> GetConditionSet(ProcessCondition.ProcessConditionType conditionType)
	{
		RocketProcessConditionDisplayTarget rocketProcessConditionDisplayTarget = null;
		RocketModuleCluster landedRocket = this.LandedRocket;
		if (landedRocket != null)
		{
			for (int i = 0; i < landedRocket.CraftInterface.ClusterModules.Count; i++)
			{
				RocketProcessConditionDisplayTarget component = landedRocket.CraftInterface.ClusterModules[i].Get().GetComponent<RocketProcessConditionDisplayTarget>();
				if (component != null)
				{
					rocketProcessConditionDisplayTarget = component;
					break;
				}
			}
		}
		if (rocketProcessConditionDisplayTarget != null)
		{
			return ((IProcessConditionSet)rocketProcessConditionDisplayTarget).GetConditionSet(conditionType);
		}
		return new List<ProcessCondition>();
	}

	// Token: 0x060086DC RID: 34524 RVA: 0x0035BA58 File Offset: 0x00359C58
	public static List<LaunchPad> GetLaunchPadsForDestination(AxialI destination)
	{
		List<LaunchPad> list = new List<LaunchPad>();
		foreach (object obj in Components.LaunchPads)
		{
			LaunchPad launchPad = (LaunchPad)obj;
			if (launchPad.GetMyWorldLocation() == destination)
			{
				list.Add(launchPad);
			}
		}
		return list;
	}

	// Token: 0x0400662A RID: 26154
	public HashedString triggerPort;

	// Token: 0x0400662B RID: 26155
	public HashedString statusPort;

	// Token: 0x0400662C RID: 26156
	public HashedString landedRocketPort;

	// Token: 0x0400662D RID: 26157
	private CellOffset baseModulePosition = new CellOffset(0, 2);

	// Token: 0x0400662E RID: 26158
	private SchedulerHandle RebuildLaunchTowerHeightHandler;

	// Token: 0x0400662F RID: 26159
	private AttachableBuilding lastBaseAttachable;

	// Token: 0x04006630 RID: 26160
	private LaunchPad.LaunchPadTower tower;

	// Token: 0x04006631 RID: 26161
	[Serialize]
	public int maxTowerHeight;

	// Token: 0x04006632 RID: 26162
	private bool dirtyTowerHeight;

	// Token: 0x04006633 RID: 26163
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04006634 RID: 26164
	private Guid landedRocketPassengerModuleStatusItem = Guid.Empty;

	// Token: 0x0200194E RID: 6478
	public class LaunchPadTower
	{
		// Token: 0x060086DE RID: 34526 RVA: 0x0035BAC8 File Offset: 0x00359CC8
		public LaunchPadTower(LaunchPad pad, int startHeight)
		{
			this.pad = pad;
			this.SetTowerHeight(startHeight);
		}

		// Token: 0x060086DF RID: 34527 RVA: 0x0035BB80 File Offset: 0x00359D80
		public void AddTowerRow()
		{
			GameObject gameObject = new GameObject("LaunchPadTowerRow");
			gameObject.SetActive(false);
			gameObject.transform.SetParent(this.pad.transform);
			gameObject.transform.SetLocalPosition(Grid.CellSizeInMeters * Vector3.up * (float)(this.towerAnimControllers.Count + this.pad.baseModulePosition.y));
			gameObject.transform.SetPosition(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, Grid.GetLayerZ(Grid.SceneLayer.Backwall)));
			KBatchedAnimController kbatchedAnimController = gameObject.AddComponent<KBatchedAnimController>();
			kbatchedAnimController.AnimFiles = new KAnimFile[]
			{
				Assets.GetAnim("rocket_launchpad_tower_kanim")
			};
			gameObject.SetActive(true);
			this.towerAnimControllers.Add(kbatchedAnimController);
			kbatchedAnimController.initialAnim = this.towerBGAnimNames[this.towerAnimControllers.Count % this.towerBGAnimNames.Length] + this.towerBGAnimSuffix_off;
			this.animLink = new KAnimLink(this.pad.GetComponent<KAnimControllerBase>(), kbatchedAnimController);
		}

		// Token: 0x060086E0 RID: 34528 RVA: 0x000AA038 File Offset: 0x000A8238
		public void RemoveTowerRow()
		{
		}

		// Token: 0x060086E1 RID: 34529 RVA: 0x0035BCA4 File Offset: 0x00359EA4
		public void SetTowerHeight(int height)
		{
			if (height < 8)
			{
				height = 0;
			}
			this.targetHeight = height;
			this.pad.maxTowerHeight = height;
			while (this.targetHeight > this.towerAnimControllers.Count)
			{
				this.AddTowerRow();
			}
			if (this.activeAnimationRoutine != null)
			{
				this.pad.StopCoroutine(this.activeAnimationRoutine);
			}
			this.activeAnimationRoutine = this.pad.StartCoroutine(this.TowerRoutine());
		}

		// Token: 0x060086E2 RID: 34530 RVA: 0x000FCF7A File Offset: 0x000FB17A
		private IEnumerator TowerRoutine()
		{
			while (this.currentHeight < this.targetHeight)
			{
				LaunchPad.LaunchPadTower.<>c__DisplayClass15_0 CS$<>8__locals1 = new LaunchPad.LaunchPadTower.<>c__DisplayClass15_0();
				CS$<>8__locals1.animComplete = false;
				this.towerAnimControllers[this.currentHeight].Queue(this.towerBGAnimNames[this.currentHeight % this.towerBGAnimNames.Length] + this.towerBGAnimSuffix_on_pre, KAnim.PlayMode.Once, 1f, 0f);
				this.towerAnimControllers[this.currentHeight].Queue(this.towerBGAnimNames[this.currentHeight % this.towerBGAnimNames.Length] + this.towerBGAnimSuffix_on, KAnim.PlayMode.Once, 1f, 0f);
				this.towerAnimControllers[this.currentHeight].onAnimComplete += delegate(HashedString arg)
				{
					CS$<>8__locals1.animComplete = true;
				};
				float delay = 0.25f;
				while (!CS$<>8__locals1.animComplete && delay > 0f)
				{
					delay -= Time.deltaTime;
					yield return 0;
				}
				this.currentHeight++;
				CS$<>8__locals1 = null;
			}
			while (this.currentHeight > this.targetHeight)
			{
				LaunchPad.LaunchPadTower.<>c__DisplayClass15_1 CS$<>8__locals2 = new LaunchPad.LaunchPadTower.<>c__DisplayClass15_1();
				this.currentHeight--;
				CS$<>8__locals2.animComplete = false;
				this.towerAnimControllers[this.currentHeight].Queue(this.towerBGAnimNames[this.currentHeight % this.towerBGAnimNames.Length] + this.towerBGAnimSuffix_off_pre, KAnim.PlayMode.Once, 1f, 0f);
				this.towerAnimControllers[this.currentHeight].Queue(this.towerBGAnimNames[this.currentHeight % this.towerBGAnimNames.Length] + this.towerBGAnimSuffix_off, KAnim.PlayMode.Once, 1f, 0f);
				this.towerAnimControllers[this.currentHeight].onAnimComplete += delegate(HashedString arg)
				{
					CS$<>8__locals2.animComplete = true;
				};
				float delay = 0.25f;
				while (!CS$<>8__locals2.animComplete && delay > 0f)
				{
					delay -= Time.deltaTime;
					yield return 0;
				}
				CS$<>8__locals2 = null;
			}
			yield return 0;
			yield break;
		}

		// Token: 0x04006635 RID: 26165
		private LaunchPad pad;

		// Token: 0x04006636 RID: 26166
		private KAnimLink animLink;

		// Token: 0x04006637 RID: 26167
		private Coroutine activeAnimationRoutine;

		// Token: 0x04006638 RID: 26168
		private string[] towerBGAnimNames = new string[]
		{
			"A1",
			"A2",
			"A3",
			"B",
			"C",
			"D",
			"E1",
			"E2",
			"F1",
			"F2"
		};

		// Token: 0x04006639 RID: 26169
		private string towerBGAnimSuffix_on = "_on";

		// Token: 0x0400663A RID: 26170
		private string towerBGAnimSuffix_on_pre = "_on_pre";

		// Token: 0x0400663B RID: 26171
		private string towerBGAnimSuffix_off_pre = "_off_pre";

		// Token: 0x0400663C RID: 26172
		private string towerBGAnimSuffix_off = "_off";

		// Token: 0x0400663D RID: 26173
		private List<KBatchedAnimController> towerAnimControllers = new List<KBatchedAnimController>();

		// Token: 0x0400663E RID: 26174
		private int targetHeight;

		// Token: 0x0400663F RID: 26175
		private int currentHeight;
	}
}
