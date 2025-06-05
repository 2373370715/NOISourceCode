using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Threading;
using FMOD.Studio;
using Klei;
using Klei.AI;
using Klei.CustomSettings;
using KSerialization;
using ProcGenGame;
using STRINGS;
using TUNING;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

// Token: 0x02001342 RID: 4930
[AddComponentMenu("KMonoBehaviour/scripts/Game")]
public class Game : KMonoBehaviour
{
	// Token: 0x060064F2 RID: 25842 RVA: 0x000E6547 File Offset: 0x000E4747
	public static bool IsOnMainThread()
	{
		return Game.MainThread == Thread.CurrentThread;
	}

	// Token: 0x060064F3 RID: 25843 RVA: 0x000E6555 File Offset: 0x000E4755
	public static bool IsQuitting()
	{
		return Game.quitting;
	}

	// Token: 0x1700064F RID: 1615
	// (get) Token: 0x060064F4 RID: 25844 RVA: 0x000E655C File Offset: 0x000E475C
	// (set) Token: 0x060064F5 RID: 25845 RVA: 0x000E6564 File Offset: 0x000E4764
	public KInputHandler inputHandler { get; set; }

	// Token: 0x17000650 RID: 1616
	// (get) Token: 0x060064F6 RID: 25846 RVA: 0x000E656D File Offset: 0x000E476D
	// (set) Token: 0x060064F7 RID: 25847 RVA: 0x000E6574 File Offset: 0x000E4774
	public static Game Instance { get; private set; }

	// Token: 0x17000651 RID: 1617
	// (get) Token: 0x060064F8 RID: 25848 RVA: 0x000E657C File Offset: 0x000E477C
	public static Camera MainCamera
	{
		get
		{
			if (Game.m_CachedCamera == null)
			{
				Game.m_CachedCamera = Camera.main;
			}
			return Game.m_CachedCamera;
		}
	}

	// Token: 0x17000652 RID: 1618
	// (get) Token: 0x060064F9 RID: 25849 RVA: 0x000E659A File Offset: 0x000E479A
	// (set) Token: 0x060064FA RID: 25850 RVA: 0x002CF4CC File Offset: 0x002CD6CC
	public bool SaveToCloudActive
	{
		get
		{
			return CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.SaveToCloud).id == "Enabled";
		}
		set
		{
			string value2 = value ? "Enabled" : "Disabled";
			CustomGameSettings.Instance.SetQualitySetting(CustomGameSettingConfigs.SaveToCloud, value2);
		}
	}

	// Token: 0x17000653 RID: 1619
	// (get) Token: 0x060064FB RID: 25851 RVA: 0x000E65BA File Offset: 0x000E47BA
	// (set) Token: 0x060064FC RID: 25852 RVA: 0x002CF4FC File Offset: 0x002CD6FC
	public bool FastWorkersModeActive
	{
		get
		{
			return CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.FastWorkersMode).id == "Enabled";
		}
		set
		{
			string value2 = value ? "Enabled" : "Disabled";
			CustomGameSettings.Instance.SetQualitySetting(CustomGameSettingConfigs.FastWorkersMode, value2);
		}
	}

	// Token: 0x17000654 RID: 1620
	// (get) Token: 0x060064FD RID: 25853 RVA: 0x000E65DA File Offset: 0x000E47DA
	// (set) Token: 0x060064FE RID: 25854 RVA: 0x002CF52C File Offset: 0x002CD72C
	public bool SandboxModeActive
	{
		get
		{
			return this.sandboxModeActive;
		}
		set
		{
			this.sandboxModeActive = value;
			base.Trigger(-1948169901, null);
			if (PlanScreen.Instance != null)
			{
				PlanScreen.Instance.Refresh();
			}
			if (BuildMenu.Instance != null)
			{
				BuildMenu.Instance.Refresh();
			}
			if (OverlayMenu.Instance != null)
			{
				OverlayMenu.Instance.Refresh();
			}
			if (ManagementMenu.Instance != null)
			{
				ManagementMenu.Instance.Refresh();
			}
		}
	}

	// Token: 0x17000655 RID: 1621
	// (get) Token: 0x060064FF RID: 25855 RVA: 0x000E65E2 File Offset: 0x000E47E2
	public bool DebugOnlyBuildingsAllowed
	{
		get
		{
			return DebugHandler.enabled && (this.SandboxModeActive || DebugHandler.InstantBuildMode);
		}
	}

	// Token: 0x17000656 RID: 1622
	// (get) Token: 0x06006500 RID: 25856 RVA: 0x000E65FC File Offset: 0x000E47FC
	// (set) Token: 0x06006501 RID: 25857 RVA: 0x000E6604 File Offset: 0x000E4804
	public StatusItemRenderer statusItemRenderer { get; private set; }

	// Token: 0x17000657 RID: 1623
	// (get) Token: 0x06006502 RID: 25858 RVA: 0x000E660D File Offset: 0x000E480D
	// (set) Token: 0x06006503 RID: 25859 RVA: 0x000E6615 File Offset: 0x000E4815
	public PrioritizableRenderer prioritizableRenderer { get; private set; }

	// Token: 0x06006504 RID: 25860 RVA: 0x002CF5A8 File Offset: 0x002CD7A8
	protected override void OnPrefabInit()
	{
		UnityEngine.Debug.unityLogger.logHandler = new LogCatcher(UnityEngine.Debug.unityLogger.logHandler);
		DebugUtil.LogArgs(new object[]
		{
			Time.realtimeSinceStartup,
			"Level Loaded....",
			SceneManager.GetActiveScene().name
		});
		Components.EntityCellVisualizers.OnAdd += this.OnAddBuildingCellVisualizer;
		Components.EntityCellVisualizers.OnRemove += this.OnRemoveBuildingCellVisualizer;
		Singleton<KBatchedAnimUpdater>.CreateInstance();
		Singleton<CellChangeMonitor>.CreateInstance();
		this.userMenu = new UserMenu();
		SimTemperatureTransfer.ClearInstanceMap();
		StructureTemperatureComponents.ClearInstanceMap();
		ElementConsumer.ClearInstanceMap();
		App.OnPreLoadScene = (System.Action)Delegate.Combine(App.OnPreLoadScene, new System.Action(this.StopBE));
		Game.Instance = this;
		this.statusItemRenderer = new StatusItemRenderer();
		this.prioritizableRenderer = new PrioritizableRenderer();
		this.LoadEventHashes();
		this.savedInfo.InitializeEmptyVariables();
		this.gasFlowPos = new Vector3(0f, 0f, Grid.GetLayerZ(Grid.SceneLayer.GasConduits) - 0.4f);
		this.liquidFlowPos = new Vector3(0f, 0f, Grid.GetLayerZ(Grid.SceneLayer.LiquidConduits) - 0.4f);
		this.solidFlowPos = new Vector3(0f, 0f, Grid.GetLayerZ(Grid.SceneLayer.SolidConduitContents) - 0.4f);
		Shader.WarmupAllShaders();
		Db.Get();
		Game.quitting = false;
		Game.PickupableLayer = LayerMask.NameToLayer("Pickupable");
		Game.BlockSelectionLayerMask = LayerMask.GetMask(new string[]
		{
			"BlockSelection"
		});
		this.world = World.Instance;
		KPrefabID.NextUniqueID = KPlayerPrefs.GetInt(Game.NextUniqueIDKey, 0);
		this.circuitManager = new CircuitManager();
		this.energySim = new EnergySim();
		this.gasConduitSystem = new UtilityNetworkManager<FlowUtilityNetwork, Vent>(Grid.WidthInCells, Grid.HeightInCells, 13);
		this.liquidConduitSystem = new UtilityNetworkManager<FlowUtilityNetwork, Vent>(Grid.WidthInCells, Grid.HeightInCells, 17);
		this.electricalConduitSystem = new UtilityNetworkManager<ElectricalUtilityNetwork, Wire>(Grid.WidthInCells, Grid.HeightInCells, 27);
		this.logicCircuitSystem = new UtilityNetworkManager<LogicCircuitNetwork, LogicWire>(Grid.WidthInCells, Grid.HeightInCells, 32);
		this.logicCircuitManager = new LogicCircuitManager(this.logicCircuitSystem);
		this.travelTubeSystem = new UtilityNetworkTubesManager(Grid.WidthInCells, Grid.HeightInCells, 35);
		this.solidConduitSystem = new UtilityNetworkManager<FlowUtilityNetwork, SolidConduit>(Grid.WidthInCells, Grid.HeightInCells, 21);
		this.conduitTemperatureManager = new ConduitTemperatureManager();
		this.conduitDiseaseManager = new ConduitDiseaseManager(this.conduitTemperatureManager);
		this.gasConduitFlow = new ConduitFlow(ConduitType.Gas, Grid.CellCount, this.gasConduitSystem, 1f, 0.25f);
		this.liquidConduitFlow = new ConduitFlow(ConduitType.Liquid, Grid.CellCount, this.liquidConduitSystem, 10f, 0.75f);
		this.solidConduitFlow = new SolidConduitFlow(Grid.CellCount, this.solidConduitSystem, 0.75f);
		this.gasFlowVisualizer = new ConduitFlowVisualizer(this.gasConduitFlow, this.gasConduitVisInfo, GlobalResources.Instance().ConduitOverlaySoundGas, Lighting.Instance.Settings.GasConduit);
		this.liquidFlowVisualizer = new ConduitFlowVisualizer(this.liquidConduitFlow, this.liquidConduitVisInfo, GlobalResources.Instance().ConduitOverlaySoundLiquid, Lighting.Instance.Settings.LiquidConduit);
		this.solidFlowVisualizer = new SolidConduitFlowVisualizer(this.solidConduitFlow, this.solidConduitVisInfo, GlobalResources.Instance().ConduitOverlaySoundSolid, Lighting.Instance.Settings.SolidConduit);
		this.accumulators = new Accumulators();
		this.plantElementAbsorbers = new PlantElementAbsorbers();
		this.activeFX = new ushort[Grid.CellCount];
		this.UnsafePrefabInit();
		Shader.SetGlobalVector("_MetalParameters", new Vector4(0f, 0f, 0f, 0f));
		Shader.SetGlobalVector("_WaterParameters", new Vector4(0f, 0f, 0f, 0f));
		this.InitializeFXSpawners();
		PathFinder.Initialize();
		new GameNavGrids(Pathfinding.Instance);
		this.screenMgr = global::Util.KInstantiate(this.screenManagerPrefab, null, null).GetComponent<GameScreenManager>();
		this.roomProber = new RoomProber();
		this.spaceScannerNetworkManager = new SpaceScannerNetworkManager();
		this.fetchManager = base.gameObject.AddComponent<FetchManager>();
		this.ediblesManager = base.gameObject.AddComponent<EdiblesManager>();
		Singleton<CellChangeMonitor>.Instance.SetGridSize(Grid.WidthInCells, Grid.HeightInCells);
		this.unlocks = base.GetComponent<Unlocks>();
		this.changelistsPlayedOn = new List<uint>();
		this.changelistsPlayedOn.Add(663500U);
		this.dateGenerated = System.DateTime.UtcNow.ToString("U", CultureInfo.InvariantCulture);
	}

	// Token: 0x06006505 RID: 25861 RVA: 0x000E661E File Offset: 0x000E481E
	public void SetGameStarted()
	{
		this.gameStarted = true;
	}

	// Token: 0x06006506 RID: 25862 RVA: 0x000E6627 File Offset: 0x000E4827
	public bool GameStarted()
	{
		return this.gameStarted;
	}

	// Token: 0x06006507 RID: 25863 RVA: 0x000E662F File Offset: 0x000E482F
	private void UnsafePrefabInit()
	{
		this.StepTheSim(0f);
	}

	// Token: 0x06006508 RID: 25864 RVA: 0x000E663D File Offset: 0x000E483D
	protected override void OnLoadLevel()
	{
		base.Unsubscribe<Game>(1798162660, Game.MarkStatusItemRendererDirtyDelegate, false);
		base.Unsubscribe<Game>(1983128072, Game.ActiveWorldChangedDelegate, false);
		base.OnLoadLevel();
	}

	// Token: 0x06006509 RID: 25865 RVA: 0x000E6667 File Offset: 0x000E4867
	private void MarkStatusItemRendererDirty(object data)
	{
		this.statusItemRenderer.MarkAllDirty();
	}

	// Token: 0x0600650A RID: 25866 RVA: 0x002CFA3C File Offset: 0x002CDC3C
	protected override void OnForcedCleanUp()
	{
		if (this.prioritizableRenderer != null)
		{
			this.prioritizableRenderer.Cleanup();
			this.prioritizableRenderer = null;
		}
		if (this.statusItemRenderer != null)
		{
			this.statusItemRenderer.Destroy();
			this.statusItemRenderer = null;
		}
		if (this.conduitTemperatureManager != null)
		{
			this.conduitTemperatureManager.Shutdown();
		}
		this.gasFlowVisualizer.FreeResources();
		this.liquidFlowVisualizer.FreeResources();
		this.solidFlowVisualizer.FreeResources();
		LightGridManager.Shutdown();
		RadiationGridManager.Shutdown();
		App.OnPreLoadScene = (System.Action)Delegate.Remove(App.OnPreLoadScene, new System.Action(this.StopBE));
		base.OnForcedCleanUp();
	}

	// Token: 0x0600650B RID: 25867 RVA: 0x002CFAE4 File Offset: 0x002CDCE4
	protected override void OnSpawn()
	{
		global::Debug.Log("-- GAME --");
		Game.BrainScheduler = base.GetComponent<BrainScheduler>();
		PropertyTextures.FogOfWarScale = 0f;
		if (CameraController.Instance != null)
		{
			CameraController.Instance.EnableFreeCamera(false);
		}
		this.LocalPlayer = this.SpawnPlayer();
		WaterCubes.Instance.Init();
		SpeedControlScreen.Instance.Pause(false, false);
		LightGridManager.Initialise();
		RadiationGridManager.Initialise();
		this.RefreshRadiationLoop();
		this.UnsafeOnSpawn();
		Time.timeScale = 0f;
		if (this.tempIntroScreenPrefab != null)
		{
			global::Util.KInstantiate(this.tempIntroScreenPrefab, null, null);
		}
		if (SaveLoader.Instance.Cluster != null)
		{
			foreach (WorldGen worldGen in SaveLoader.Instance.Cluster.worlds)
			{
				this.Reset(worldGen.data.gameSpawnData, worldGen.WorldOffset);
			}
			NewBaseScreen.SetInitialCamera();
		}
		TagManager.FillMissingProperNames();
		CameraController.Instance.OrthographicSize = 20f;
		if (SaveLoader.Instance.loadedFromSave)
		{
			this.baseAlreadyCreated = true;
			base.Trigger(-1992507039, null);
			base.Trigger(-838649377, null);
		}
		UnityEngine.Object[] array = Resources.FindObjectsOfTypeAll(typeof(MeshRenderer));
		for (int i = 0; i < array.Length; i++)
		{
			((MeshRenderer)array[i]).reflectionProbeUsage = ReflectionProbeUsage.Off;
		}
		base.Subscribe<Game>(1798162660, Game.MarkStatusItemRendererDirtyDelegate);
		base.Subscribe<Game>(1983128072, Game.ActiveWorldChangedDelegate);
		this.solidConduitFlow.Initialize();
		SimAndRenderScheduler.instance.Add(this.roomProber, false);
		SimAndRenderScheduler.instance.Add(this.spaceScannerNetworkManager, false);
		SimAndRenderScheduler.instance.Add(KComponentSpawn.instance, false);
		SimAndRenderScheduler.instance.RegisterBatchUpdate<ISim200ms, AmountInstance>(new UpdateBucketWithUpdater<ISim200ms>.BatchUpdateDelegate(AmountInstance.BatchUpdate));
		SimAndRenderScheduler.instance.RegisterBatchUpdate<ISim1000ms, SolidTransferArm>(new UpdateBucketWithUpdater<ISim1000ms>.BatchUpdateDelegate(SolidTransferArm.BatchUpdate));
		if (!SaveLoader.Instance.loadedFromSave)
		{
			SettingConfig settingConfig = CustomGameSettings.Instance.QualitySettings[CustomGameSettingConfigs.SandboxMode.id];
			SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.SandboxMode);
			SaveGame.Instance.sandboxEnabled = !settingConfig.IsDefaultLevel(currentQualitySetting.id);
		}
		this.mingleCellTracker = base.gameObject.AddComponent<MingleCellTracker>();
		if (Global.Instance != null)
		{
			Global.Instance.GetComponent<PerformanceMonitor>().Reset();
			Global.Instance.modManager.NotifyDialog(UI.FRONTEND.MOD_DIALOGS.SAVE_GAME_MODS_DIFFER.TITLE, UI.FRONTEND.MOD_DIALOGS.SAVE_GAME_MODS_DIFFER.MESSAGE, Global.Instance.globalCanvas);
		}
	}

	// Token: 0x0600650C RID: 25868 RVA: 0x000E6674 File Offset: 0x000E4874
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		SimAndRenderScheduler.instance.Remove(KComponentSpawn.instance);
		SimAndRenderScheduler.instance.RegisterBatchUpdate<ISim200ms, AmountInstance>(null);
		SimAndRenderScheduler.instance.RegisterBatchUpdate<ISim1000ms, SolidTransferArm>(null);
		this.DestroyInstances();
	}

	// Token: 0x0600650D RID: 25869 RVA: 0x000E66A7 File Offset: 0x000E48A7
	private new void OnDestroy()
	{
		base.OnDestroy();
		this.DestroyInstances();
	}

	// Token: 0x0600650E RID: 25870 RVA: 0x000E66B5 File Offset: 0x000E48B5
	private void UnsafeOnSpawn()
	{
		this.world.UpdateCellInfo(this.gameSolidInfo, this.callbackInfo, 0, null, 0, null);
	}

	// Token: 0x0600650F RID: 25871 RVA: 0x000E66D4 File Offset: 0x000E48D4
	private void RefreshRadiationLoop()
	{
		GameScheduler.Instance.Schedule("UpdateRadiation", 1f, delegate(object obj)
		{
			RadiationGridManager.Refresh();
			this.RefreshRadiationLoop();
		}, null, null);
	}

	// Token: 0x06006510 RID: 25872 RVA: 0x000E66F9 File Offset: 0x000E48F9
	public void SetMusicEnabled(bool enabled)
	{
		if (enabled)
		{
			MusicManager.instance.PlaySong("Music_FrontEnd", false);
			return;
		}
		MusicManager.instance.StopSong("Music_FrontEnd", true, STOP_MODE.ALLOWFADEOUT);
	}

	// Token: 0x06006511 RID: 25873 RVA: 0x002CFD9C File Offset: 0x002CDF9C
	private Player SpawnPlayer()
	{
		Player component = global::Util.KInstantiate(this.playerPrefab, base.gameObject, null).GetComponent<Player>();
		component.ScreenManager = this.screenMgr;
		component.ScreenManager.StartScreen(ScreenPrefabs.Instance.HudScreen.gameObject, null, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay);
		component.ScreenManager.StartScreen(ScreenPrefabs.Instance.HoverTextScreen.gameObject, null, GameScreenManager.UIRenderTarget.HoverTextScreen);
		component.ScreenManager.StartScreen(ScreenPrefabs.Instance.ToolTipScreen.gameObject, null, GameScreenManager.UIRenderTarget.HoverTextScreen);
		this.cameraController = global::Util.KInstantiate(this.cameraControllerPrefab, null, null).GetComponent<CameraController>();
		component.CameraController = this.cameraController;
		if (KInputManager.currentController != null)
		{
			KInputHandler.Add(KInputManager.currentController, this.cameraController, 1);
		}
		else
		{
			KInputHandler.Add(Global.GetInputManager().GetDefaultController(), this.cameraController, 1);
		}
		Global.GetInputManager().usedMenus.Add(this.cameraController);
		this.playerController = component.GetComponent<PlayerController>();
		if (KInputManager.currentController != null)
		{
			KInputHandler.Add(KInputManager.currentController, this.playerController, 20);
		}
		else
		{
			KInputHandler.Add(Global.GetInputManager().GetDefaultController(), this.playerController, 20);
		}
		Global.GetInputManager().usedMenus.Add(this.playerController);
		return component;
	}

	// Token: 0x06006512 RID: 25874 RVA: 0x000E6720 File Offset: 0x000E4920
	public void SetDupePassableSolid(int cell, bool passable, bool solid)
	{
		Grid.DupePassable[cell] = passable;
		this.gameSolidInfo.Add(new SolidInfo(cell, solid));
	}

	// Token: 0x06006513 RID: 25875 RVA: 0x002CFEE4 File Offset: 0x002CE0E4
	private unsafe Sim.GameDataUpdate* StepTheSim(float dt)
	{
		Sim.GameDataUpdate* result;
		using (new KProfiler.Region("StepTheSim", null))
		{
			IntPtr intPtr = IntPtr.Zero;
			using (new KProfiler.Region("WaitingForSim", null))
			{
				if (Grid.Visible == null || Grid.Visible.Length == 0)
				{
					global::Debug.LogError("Invalid Grid.Visible, what have you done?!");
					return null;
				}
				intPtr = Sim.HandleMessage(SimMessageHashes.PrepareGameData, Grid.Visible.Length, Grid.Visible);
			}
			if (intPtr == IntPtr.Zero)
			{
				result = null;
			}
			else
			{
				Sim.GameDataUpdate* ptr = (Sim.GameDataUpdate*)((void*)intPtr);
				Grid.elementIdx = ptr->elementIdx;
				Grid.temperature = ptr->temperature;
				Grid.mass = ptr->mass;
				Grid.radiation = ptr->radiation;
				Grid.properties = ptr->properties;
				Grid.strengthInfo = ptr->strengthInfo;
				Grid.insulation = ptr->insulation;
				Grid.diseaseIdx = ptr->diseaseIdx;
				Grid.diseaseCount = ptr->diseaseCount;
				Grid.AccumulatedFlowValues = ptr->accumulatedFlow;
				Grid.exposedToSunlight = (byte*)((void*)ptr->propertyTextureExposedToSunlight);
				PropertyTextures.externalFlowTex = ptr->propertyTextureFlow;
				PropertyTextures.externalLiquidTex = ptr->propertyTextureLiquid;
				PropertyTextures.externalLiquidDataTex = ptr->propertyTextureLiquidData;
				PropertyTextures.externalExposedToSunlight = ptr->propertyTextureExposedToSunlight;
				List<Element> elements = ElementLoader.elements;
				this.simData.emittedMassEntries = ptr->emittedMassEntries;
				this.simData.elementChunks = ptr->elementChunkInfos;
				this.simData.buildingTemperatures = ptr->buildingTemperatures;
				this.simData.diseaseEmittedInfos = ptr->diseaseEmittedInfos;
				this.simData.diseaseConsumedInfos = ptr->diseaseConsumedInfos;
				for (int i = 0; i < ptr->numSubstanceChangeInfo; i++)
				{
					Sim.SubstanceChangeInfo substanceChangeInfo = ptr->substanceChangeInfo[i];
					Element element = elements[(int)substanceChangeInfo.newElemIdx];
					Grid.Element[substanceChangeInfo.cellIdx] = element;
				}
				for (int j = 0; j < ptr->numSolidInfo; j++)
				{
					Sim.SolidInfo solidInfo = ptr->solidInfo[j];
					if (!this.solidChangedFilter.Contains(solidInfo.cellIdx))
					{
						this.solidInfo.Add(new SolidInfo(solidInfo.cellIdx, solidInfo.isSolid != 0));
						bool solid = solidInfo.isSolid != 0;
						Grid.SetSolid(solidInfo.cellIdx, solid, CellEventLogger.Instance.SimMessagesSolid);
					}
				}
				for (int k = 0; k < ptr->numCallbackInfo; k++)
				{
					Sim.CallbackInfo callbackInfo = ptr->callbackInfo[k];
					HandleVector<Game.CallbackInfo>.Handle handle = new HandleVector<Game.CallbackInfo>.Handle
					{
						index = callbackInfo.callbackIdx
					};
					if (!this.IsManuallyReleasedHandle(handle))
					{
						this.callbackInfo.Add(new Klei.CallbackInfo(handle));
					}
				}
				int numSpawnFallingLiquidInfo = ptr->numSpawnFallingLiquidInfo;
				for (int l = 0; l < numSpawnFallingLiquidInfo; l++)
				{
					Sim.SpawnFallingLiquidInfo spawnFallingLiquidInfo = ptr->spawnFallingLiquidInfo[l];
					FallingWater.instance.AddParticle(spawnFallingLiquidInfo.cellIdx, spawnFallingLiquidInfo.elemIdx, spawnFallingLiquidInfo.mass, spawnFallingLiquidInfo.temperature, spawnFallingLiquidInfo.diseaseIdx, spawnFallingLiquidInfo.diseaseCount, false, false, false, false);
				}
				int numDigInfo = ptr->numDigInfo;
				WorldDamage component = this.world.GetComponent<WorldDamage>();
				for (int m = 0; m < numDigInfo; m++)
				{
					Sim.SpawnOreInfo spawnOreInfo = ptr->digInfo[m];
					if (spawnOreInfo.temperature <= 0f && spawnOreInfo.mass > 0f)
					{
						global::Debug.LogError("Sim is telling us to spawn a zero temperature object. This shouldn't be possible because I have asserts in the dll about this....");
					}
					component.OnDigComplete(spawnOreInfo.cellIdx, spawnOreInfo.mass, spawnOreInfo.temperature, spawnOreInfo.elemIdx, spawnOreInfo.diseaseIdx, spawnOreInfo.diseaseCount);
				}
				int numSpawnOreInfo = ptr->numSpawnOreInfo;
				for (int n = 0; n < numSpawnOreInfo; n++)
				{
					Sim.SpawnOreInfo spawnOreInfo2 = ptr->spawnOreInfo[n];
					Vector3 position = Grid.CellToPosCCC(spawnOreInfo2.cellIdx, Grid.SceneLayer.Ore);
					Element element2 = ElementLoader.elements[(int)spawnOreInfo2.elemIdx];
					if (spawnOreInfo2.temperature <= 0f && spawnOreInfo2.mass > 0f)
					{
						global::Debug.LogError("Sim is telling us to spawn a zero temperature object. This shouldn't be possible because I have asserts in the dll about this....");
					}
					element2.substance.SpawnResource(position, spawnOreInfo2.mass, spawnOreInfo2.temperature, spawnOreInfo2.diseaseIdx, spawnOreInfo2.diseaseCount, false, false, false);
				}
				int numSpawnFXInfo = ptr->numSpawnFXInfo;
				for (int num = 0; num < numSpawnFXInfo; num++)
				{
					Sim.SpawnFXInfo spawnFXInfo = ptr->spawnFXInfo[num];
					this.SpawnFX((SpawnFXHashes)spawnFXInfo.fxHash, spawnFXInfo.cellIdx, spawnFXInfo.rotation);
				}
				UnstableGroundManager component2 = this.world.GetComponent<UnstableGroundManager>();
				int numUnstableCellInfo = ptr->numUnstableCellInfo;
				for (int num2 = 0; num2 < numUnstableCellInfo; num2++)
				{
					Sim.UnstableCellInfo unstableCellInfo = ptr->unstableCellInfo[num2];
					if (unstableCellInfo.fallingInfo == 0)
					{
						component2.Spawn(unstableCellInfo.cellIdx, ElementLoader.elements[(int)unstableCellInfo.elemIdx], unstableCellInfo.mass, unstableCellInfo.temperature, unstableCellInfo.diseaseIdx, unstableCellInfo.diseaseCount);
					}
				}
				int numWorldDamageInfo = ptr->numWorldDamageInfo;
				for (int num3 = 0; num3 < numWorldDamageInfo; num3++)
				{
					Sim.WorldDamageInfo damage_info = ptr->worldDamageInfo[num3];
					WorldDamage.Instance.ApplyDamage(damage_info);
				}
				for (int num4 = 0; num4 < ptr->numRemovedMassEntries; num4++)
				{
					ElementConsumer.AddMass(ptr->removedMassEntries[num4]);
				}
				int numMassConsumedCallbacks = ptr->numMassConsumedCallbacks;
				HandleVector<Game.ComplexCallbackInfo<Sim.MassConsumedCallback>>.Handle handle2 = default(HandleVector<Game.ComplexCallbackInfo<Sim.MassConsumedCallback>>.Handle);
				for (int num5 = 0; num5 < numMassConsumedCallbacks; num5++)
				{
					Sim.MassConsumedCallback massConsumedCallback = ptr->massConsumedCallbacks[num5];
					handle2.index = massConsumedCallback.callbackIdx;
					Game.ComplexCallbackInfo<Sim.MassConsumedCallback> complexCallbackInfo = this.massConsumedCallbackManager.Release(handle2, "massConsumedCB");
					if (complexCallbackInfo.cb != null)
					{
						complexCallbackInfo.cb(massConsumedCallback, complexCallbackInfo.callbackData);
					}
				}
				int numMassEmittedCallbacks = ptr->numMassEmittedCallbacks;
				HandleVector<Game.ComplexCallbackInfo<Sim.MassEmittedCallback>>.Handle handle3 = default(HandleVector<Game.ComplexCallbackInfo<Sim.MassEmittedCallback>>.Handle);
				for (int num6 = 0; num6 < numMassEmittedCallbacks; num6++)
				{
					Sim.MassEmittedCallback massEmittedCallback = ptr->massEmittedCallbacks[num6];
					handle3.index = massEmittedCallback.callbackIdx;
					if (this.massEmitCallbackManager.IsVersionValid(handle3))
					{
						Game.ComplexCallbackInfo<Sim.MassEmittedCallback> item = this.massEmitCallbackManager.GetItem(handle3);
						if (item.cb != null)
						{
							item.cb(massEmittedCallback, item.callbackData);
						}
					}
				}
				int numDiseaseConsumptionCallbacks = ptr->numDiseaseConsumptionCallbacks;
				HandleVector<Game.ComplexCallbackInfo<Sim.DiseaseConsumptionCallback>>.Handle handle4 = default(HandleVector<Game.ComplexCallbackInfo<Sim.DiseaseConsumptionCallback>>.Handle);
				for (int num7 = 0; num7 < numDiseaseConsumptionCallbacks; num7++)
				{
					Sim.DiseaseConsumptionCallback diseaseConsumptionCallback = ptr->diseaseConsumptionCallbacks[num7];
					handle4.index = diseaseConsumptionCallback.callbackIdx;
					if (this.diseaseConsumptionCallbackManager.IsVersionValid(handle4))
					{
						Game.ComplexCallbackInfo<Sim.DiseaseConsumptionCallback> item2 = this.diseaseConsumptionCallbackManager.GetItem(handle4);
						if (item2.cb != null)
						{
							item2.cb(diseaseConsumptionCallback, item2.callbackData);
						}
					}
				}
				int numComponentStateChangedMessages = ptr->numComponentStateChangedMessages;
				HandleVector<Game.ComplexCallbackInfo<int>>.Handle handle5 = default(HandleVector<Game.ComplexCallbackInfo<int>>.Handle);
				for (int num8 = 0; num8 < numComponentStateChangedMessages; num8++)
				{
					Sim.ComponentStateChangedMessage componentStateChangedMessage = ptr->componentStateChangedMessages[num8];
					handle5.index = componentStateChangedMessage.callbackIdx;
					if (this.simComponentCallbackManager.IsVersionValid(handle5))
					{
						Game.ComplexCallbackInfo<int> complexCallbackInfo2 = this.simComponentCallbackManager.Release(handle5, "component state changed cb");
						if (complexCallbackInfo2.cb != null)
						{
							complexCallbackInfo2.cb(componentStateChangedMessage.simHandle, complexCallbackInfo2.callbackData);
						}
					}
				}
				int numRadiationConsumedCallbacks = ptr->numRadiationConsumedCallbacks;
				HandleVector<Game.ComplexCallbackInfo<Sim.ConsumedRadiationCallback>>.Handle handle6 = default(HandleVector<Game.ComplexCallbackInfo<Sim.ConsumedRadiationCallback>>.Handle);
				for (int num9 = 0; num9 < numRadiationConsumedCallbacks; num9++)
				{
					Sim.ConsumedRadiationCallback consumedRadiationCallback = ptr->radiationConsumedCallbacks[num9];
					handle6.index = consumedRadiationCallback.callbackIdx;
					Game.ComplexCallbackInfo<Sim.ConsumedRadiationCallback> complexCallbackInfo3 = this.radiationConsumedCallbackManager.Release(handle6, "radiationConsumedCB");
					if (complexCallbackInfo3.cb != null)
					{
						complexCallbackInfo3.cb(consumedRadiationCallback, complexCallbackInfo3.callbackData);
					}
				}
				int numElementChunkMeltedInfos = ptr->numElementChunkMeltedInfos;
				for (int num10 = 0; num10 < numElementChunkMeltedInfos; num10++)
				{
					SimTemperatureTransfer.DoOreMeltTransition(ptr->elementChunkMeltedInfos[num10].handle);
				}
				int numBuildingOverheatInfos = ptr->numBuildingOverheatInfos;
				for (int num11 = 0; num11 < numBuildingOverheatInfos; num11++)
				{
					StructureTemperatureComponents.DoOverheat(ptr->buildingOverheatInfos[num11].handle);
				}
				int numBuildingNoLongerOverheatedInfos = ptr->numBuildingNoLongerOverheatedInfos;
				for (int num12 = 0; num12 < numBuildingNoLongerOverheatedInfos; num12++)
				{
					StructureTemperatureComponents.DoNoLongerOverheated(ptr->buildingNoLongerOverheatedInfos[num12].handle);
				}
				int numBuildingMeltedInfos = ptr->numBuildingMeltedInfos;
				for (int num13 = 0; num13 < numBuildingMeltedInfos; num13++)
				{
					StructureTemperatureComponents.DoStateTransition(ptr->buildingMeltedInfos[num13].handle);
				}
				int numCellMeltedInfos = ptr->numCellMeltedInfos;
				for (int num14 = 0; num14 < numCellMeltedInfos; num14++)
				{
					int gameCell = ptr->cellMeltedInfos[num14].gameCell;
					GameObject gameObject = Grid.Objects[gameCell, 9];
					if (gameObject != null)
					{
						gameObject.Trigger(675471409, null);
						global::Util.KDestroyGameObject(gameObject);
					}
				}
				if (dt > 0f)
				{
					this.conduitTemperatureManager.Sim200ms(0.2f);
					this.conduitDiseaseManager.Sim200ms(0.2f);
					this.gasConduitFlow.Sim200ms(0.2f);
					this.liquidConduitFlow.Sim200ms(0.2f);
					this.solidConduitFlow.Sim200ms(0.2f);
					this.accumulators.Sim200ms(0.2f);
					this.plantElementAbsorbers.Sim200ms(0.2f);
				}
				Sim.DebugProperties debugProperties;
				debugProperties.buildingTemperatureScale = 100f;
				debugProperties.buildingToBuildingTemperatureScale = 0.001f;
				debugProperties.biomeTemperatureLerpRate = 0.001f;
				debugProperties.isDebugEditing = ((DebugPaintElementScreen.Instance != null && DebugPaintElementScreen.Instance.gameObject.activeSelf) ? 1 : 0);
				debugProperties.pad0 = (debugProperties.pad1 = (debugProperties.pad2 = 0));
				SimMessages.SetDebugProperties(debugProperties);
				if (dt > 0f)
				{
					if (this.circuitManager != null)
					{
						this.circuitManager.Sim200msFirst(dt);
					}
					if (this.energySim != null)
					{
						this.energySim.EnergySim200ms(dt);
					}
					if (this.circuitManager != null)
					{
						this.circuitManager.Sim200msLast(dt);
					}
				}
				result = ptr;
			}
		}
		return result;
	}

	// Token: 0x06006514 RID: 25876 RVA: 0x000E6740 File Offset: 0x000E4940
	public void AddSolidChangedFilter(int cell)
	{
		this.solidChangedFilter.Add(cell);
	}

	// Token: 0x06006515 RID: 25877 RVA: 0x000E674F File Offset: 0x000E494F
	public void RemoveSolidChangedFilter(int cell)
	{
		this.solidChangedFilter.Remove(cell);
	}

	// Token: 0x06006516 RID: 25878 RVA: 0x000E675E File Offset: 0x000E495E
	public void SetIsLoading()
	{
		this.isLoading = true;
	}

	// Token: 0x06006517 RID: 25879 RVA: 0x000E6767 File Offset: 0x000E4967
	public bool IsLoading()
	{
		return this.isLoading;
	}

	// Token: 0x06006518 RID: 25880 RVA: 0x002D09E0 File Offset: 0x002CEBE0
	private void ShowDebugCellInfo()
	{
		int mouseCell = DebugHandler.GetMouseCell();
		int num = 0;
		int num2 = 0;
		Grid.CellToXY(mouseCell, out num, out num2);
		string text = string.Concat(new string[]
		{
			mouseCell.ToString(),
			" (",
			num.ToString(),
			", ",
			num2.ToString(),
			")"
		});
		DebugText.Instance.Draw(text, Grid.CellToPosCCC(mouseCell, Grid.SceneLayer.Move), Color.white);
	}

	// Token: 0x06006519 RID: 25881 RVA: 0x000E676F File Offset: 0x000E496F
	public void ForceSimStep()
	{
		DebugUtil.LogArgs(new object[]
		{
			"Force-stepping the sim"
		});
		this.simDt = 0.2f;
	}

	// Token: 0x0600651A RID: 25882 RVA: 0x002D0A5C File Offset: 0x002CEC5C
	private void Update()
	{
		if (this.isLoading)
		{
			return;
		}
		float deltaTime = Time.deltaTime;
		if (global::Debug.developerConsoleVisible)
		{
			global::Debug.developerConsoleVisible = false;
		}
		if (DebugHandler.DebugCellInfo)
		{
			this.ShowDebugCellInfo();
		}
		this.gasConduitSystem.Update();
		this.liquidConduitSystem.Update();
		this.solidConduitSystem.Update();
		this.circuitManager.RenderEveryTick(deltaTime);
		this.logicCircuitManager.RenderEveryTick(deltaTime);
		this.solidConduitFlow.RenderEveryTick(deltaTime);
		Pathfinding.Instance.RenderEveryTick();
		Singleton<CellChangeMonitor>.Instance.RenderEveryTick();
		this.SimEveryTick(deltaTime);
	}

	// Token: 0x0600651B RID: 25883 RVA: 0x002D0AF4 File Offset: 0x002CECF4
	private void SimEveryTick(float dt)
	{
		dt = Mathf.Min(dt, 0.2f);
		this.simDt += dt;
		if (this.simDt >= 0.016666668f)
		{
			do
			{
				this.simSubTick++;
				this.simSubTick %= 12;
				if (this.simSubTick == 0)
				{
					this.hasFirstSimTickRun = true;
					this.UnsafeSim200ms(0.2f);
				}
				if (this.hasFirstSimTickRun)
				{
					Singleton<StateMachineUpdater>.Instance.AdvanceOneSimSubTick();
				}
				this.simDt -= 0.016666668f;
			}
			while (this.simDt >= 0.016666668f);
			return;
		}
		this.UnsafeSim200ms(0f);
	}

	// Token: 0x0600651C RID: 25884 RVA: 0x002D0BA0 File Offset: 0x002CEDA0
	private unsafe void UnsafeSim200ms(float dt)
	{
		this.simActiveRegions.Clear();
		foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
		{
			if (worldContainer.IsDiscovered)
			{
				Game.SimActiveRegion simActiveRegion = new Game.SimActiveRegion();
				simActiveRegion.region = new Pair<Vector2I, Vector2I>(worldContainer.WorldOffset, worldContainer.WorldOffset + worldContainer.WorldSize);
				simActiveRegion.currentSunlightIntensity = worldContainer.currentSunlightIntensity;
				simActiveRegion.currentCosmicRadiationIntensity = worldContainer.currentCosmicIntensity;
				this.simActiveRegions.Add(simActiveRegion);
			}
		}
		global::Debug.Assert(this.simActiveRegions.Count > 0, "Cannot send a frame to the sim with zero active regions");
		SimMessages.NewGameFrame(dt, this.simActiveRegions);
		Sim.GameDataUpdate* ptr = this.StepTheSim(dt);
		if (ptr == null)
		{
			global::Debug.LogError("UNEXPECTED!");
			return;
		}
		if (ptr->numFramesProcessed <= 0)
		{
			return;
		}
		this.gameSolidInfo.AddRange(this.solidInfo);
		this.world.UpdateCellInfo(this.gameSolidInfo, this.callbackInfo, ptr->numSolidSubstanceChangeInfo, ptr->solidSubstanceChangeInfo, ptr->numLiquidChangeInfo, ptr->liquidChangeInfo);
		this.gameSolidInfo.Clear();
		this.solidInfo.Clear();
		this.callbackInfo.Clear();
		this.callbackManagerManuallyReleasedHandles.Clear();
		Pathfinding.Instance.UpdateNavGrids(false);
	}

	// Token: 0x0600651D RID: 25885 RVA: 0x000E678F File Offset: 0x000E498F
	private void LateUpdateComponents()
	{
		this.UpdateOverlayScreen();
	}

	// Token: 0x0600651E RID: 25886 RVA: 0x002D0D0C File Offset: 0x002CEF0C
	private void OnAddBuildingCellVisualizer(EntityCellVisualizer entity_cell_visualizer)
	{
		this.lastDrawnOverlayMode = default(HashedString);
		if (PlayerController.Instance != null)
		{
			BuildTool buildTool = PlayerController.Instance.ActiveTool as BuildTool;
			if (buildTool != null && buildTool.visualizer == entity_cell_visualizer.gameObject)
			{
				this.previewVisualizer = entity_cell_visualizer;
			}
		}
	}

	// Token: 0x0600651F RID: 25887 RVA: 0x000E6797 File Offset: 0x000E4997
	private void OnRemoveBuildingCellVisualizer(EntityCellVisualizer entity_cell_visualizer)
	{
		if (this.previewVisualizer == entity_cell_visualizer)
		{
			this.previewVisualizer = null;
		}
	}

	// Token: 0x06006520 RID: 25888 RVA: 0x002D0D68 File Offset: 0x002CEF68
	private void UpdateOverlayScreen()
	{
		if (OverlayScreen.Instance == null)
		{
			return;
		}
		HashedString mode = OverlayScreen.Instance.GetMode();
		if (this.previewVisualizer != null)
		{
			this.previewVisualizer.DrawIcons(mode);
		}
		if (mode == this.lastDrawnOverlayMode)
		{
			return;
		}
		foreach (EntityCellVisualizer entityCellVisualizer in Components.EntityCellVisualizers.Items)
		{
			entityCellVisualizer.DrawIcons(mode);
		}
		this.lastDrawnOverlayMode = mode;
	}

	// Token: 0x06006521 RID: 25889 RVA: 0x000E67AE File Offset: 0x000E49AE
	public void ForceOverlayUpdate(bool clearLastMode = false)
	{
		this.previousOverlayMode = OverlayModes.None.ID;
		if (clearLastMode)
		{
			this.lastDrawnOverlayMode = OverlayModes.None.ID;
		}
	}

	// Token: 0x06006522 RID: 25890 RVA: 0x002D0E08 File Offset: 0x002CF008
	private void LateUpdate()
	{
		if (this.OnSpawnComplete != null)
		{
			this.OnSpawnComplete();
			this.OnSpawnComplete = null;
		}
		if (Time.timeScale == 0f && !this.IsPaused)
		{
			this.IsPaused = true;
			base.Trigger(-1788536802, this.IsPaused);
		}
		else if (Time.timeScale != 0f && this.IsPaused)
		{
			this.IsPaused = false;
			base.Trigger(-1788536802, this.IsPaused);
		}
		if (Input.GetMouseButton(0))
		{
			this.VisualTunerElement = null;
			int mouseCell = DebugHandler.GetMouseCell();
			if (Grid.IsValidCell(mouseCell))
			{
				Element visualTunerElement = Grid.Element[mouseCell];
				this.VisualTunerElement = visualTunerElement;
			}
		}
		this.gasConduitSystem.Update();
		this.liquidConduitSystem.Update();
		this.solidConduitSystem.Update();
		HashedString mode = SimDebugView.Instance.GetMode();
		if (mode != this.previousOverlayMode)
		{
			this.previousOverlayMode = mode;
			if (mode == OverlayModes.LiquidConduits.ID)
			{
				this.liquidFlowVisualizer.ColourizePipeContents(true, true);
				this.gasFlowVisualizer.ColourizePipeContents(false, true);
				this.solidFlowVisualizer.ColourizePipeContents(false, true);
			}
			else if (mode == OverlayModes.GasConduits.ID)
			{
				this.liquidFlowVisualizer.ColourizePipeContents(false, true);
				this.gasFlowVisualizer.ColourizePipeContents(true, true);
				this.solidFlowVisualizer.ColourizePipeContents(false, true);
			}
			else if (mode == OverlayModes.SolidConveyor.ID)
			{
				this.liquidFlowVisualizer.ColourizePipeContents(false, true);
				this.gasFlowVisualizer.ColourizePipeContents(false, true);
				this.solidFlowVisualizer.ColourizePipeContents(true, true);
			}
			else
			{
				this.liquidFlowVisualizer.ColourizePipeContents(false, false);
				this.gasFlowVisualizer.ColourizePipeContents(false, false);
				this.solidFlowVisualizer.ColourizePipeContents(false, false);
			}
		}
		this.gasFlowVisualizer.Render(this.gasFlowPos.z, 0, this.gasConduitFlow.ContinuousLerpPercent, mode == OverlayModes.GasConduits.ID && this.gasConduitFlow.DiscreteLerpPercent != this.previousGasConduitFlowDiscreteLerpPercent);
		this.liquidFlowVisualizer.Render(this.liquidFlowPos.z, 0, this.liquidConduitFlow.ContinuousLerpPercent, mode == OverlayModes.LiquidConduits.ID && this.liquidConduitFlow.DiscreteLerpPercent != this.previousLiquidConduitFlowDiscreteLerpPercent);
		this.solidFlowVisualizer.Render(this.solidFlowPos.z, 0, this.solidConduitFlow.ContinuousLerpPercent, mode == OverlayModes.SolidConveyor.ID && this.solidConduitFlow.DiscreteLerpPercent != this.previousSolidConduitFlowDiscreteLerpPercent);
		this.previousGasConduitFlowDiscreteLerpPercent = ((mode == OverlayModes.GasConduits.ID) ? this.gasConduitFlow.DiscreteLerpPercent : -1f);
		this.previousLiquidConduitFlowDiscreteLerpPercent = ((mode == OverlayModes.LiquidConduits.ID) ? this.liquidConduitFlow.DiscreteLerpPercent : -1f);
		this.previousSolidConduitFlowDiscreteLerpPercent = ((mode == OverlayModes.SolidConveyor.ID) ? this.solidConduitFlow.DiscreteLerpPercent : -1f);
		Vector3 vector = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, Camera.main.transform.GetPosition().z));
		Vector3 vector2 = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, Camera.main.transform.GetPosition().z));
		Shader.SetGlobalVector("_WsToCs", new Vector4(vector.x / (float)Grid.WidthInCells, vector.y / (float)Grid.HeightInCells, (vector2.x - vector.x) / (float)Grid.WidthInCells, (vector2.y - vector.y) / (float)Grid.HeightInCells));
		WorldContainer activeWorld = ClusterManager.Instance.activeWorld;
		Vector2I worldOffset = activeWorld.WorldOffset;
		Vector2I worldSize = activeWorld.WorldSize;
		Vector4 value = new Vector4((vector.x - (float)worldOffset.x) / (float)worldSize.x, (vector.y - (float)worldOffset.y) / (float)worldSize.y, (vector2.x - vector.x) / (float)worldSize.x, (vector2.y - vector.y) / (float)worldSize.y);
		Shader.SetGlobalVector("_WsToCcs", value);
		if (this.drawStatusItems)
		{
			this.statusItemRenderer.RenderEveryTick();
			this.prioritizableRenderer.RenderEveryTick();
		}
		this.LateUpdateComponents();
		Singleton<StateMachineUpdater>.Instance.Render(Time.unscaledDeltaTime);
		Singleton<StateMachineUpdater>.Instance.RenderEveryTick(Time.unscaledDeltaTime);
		if (SelectTool.Instance != null && SelectTool.Instance.selected != null)
		{
			Navigator component = SelectTool.Instance.selected.GetComponent<Navigator>();
			if (component != null)
			{
				component.DrawPath();
			}
		}
		KFMOD.RenderEveryTick(Time.deltaTime);
		if (GenericGameSettings.instance.performanceCapture.waitTime != 0f)
		{
			this.UpdatePerformanceCapture();
		}
	}

	// Token: 0x06006523 RID: 25891 RVA: 0x002D12F8 File Offset: 0x002CF4F8
	private void UpdatePerformanceCapture()
	{
		if (this.IsPaused && SpeedControlScreen.Instance != null)
		{
			SpeedControlScreen.Instance.Unpause(true);
		}
		if (Time.timeSinceLevelLoad < GenericGameSettings.instance.performanceCapture.waitTime)
		{
			return;
		}
		uint num = 663500U;
		string text = System.DateTime.Now.ToShortDateString();
		string text2 = System.DateTime.Now.ToShortTimeString();
		string fileName = Path.GetFileName(GenericGameSettings.instance.performanceCapture.saveGame);
		string text3 = "Version,Date,Time,SaveGame";
		string text4 = string.Format("{0},{1},{2},{3}", new object[]
		{
			num,
			text,
			text2,
			fileName
		});
		float num2 = 0.1f;
		if (GenericGameSettings.instance.performanceCapture.gcStats)
		{
			global::Debug.Log("Begin GC profiling...");
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			GC.Collect();
			num2 = Time.realtimeSinceStartup - realtimeSinceStartup;
			global::Debug.Log("\tGC.Collect() took " + num2.ToString() + " seconds");
			MemorySnapshot memorySnapshot = new MemorySnapshot();
			string format = "{0},{1},{2},{3}";
			string path = "./memory/GCTypeMetrics.csv";
			if (!File.Exists(path))
			{
				using (StreamWriter streamWriter = new StreamWriter(path))
				{
					streamWriter.WriteLine(string.Format(format, new object[]
					{
						text3,
						"Type",
						"Instances",
						"References"
					}));
				}
			}
			using (StreamWriter streamWriter2 = new StreamWriter(path, true))
			{
				foreach (MemorySnapshot.TypeData typeData in memorySnapshot.types.Values)
				{
					streamWriter2.WriteLine(string.Format(format, new object[]
					{
						text4,
						"\"" + typeData.type.ToString() + "\"",
						typeData.instanceCount,
						typeData.refCount
					}));
				}
			}
			global::Debug.Log("...end GC profiling");
		}
		float fps = Global.Instance.GetComponent<PerformanceMonitor>().FPS;
		Directory.CreateDirectory("./memory");
		string format2 = "{0},{1},{2}";
		string path2 = "./memory/GeneralMetrics.csv";
		if (!File.Exists(path2))
		{
			using (StreamWriter streamWriter3 = new StreamWriter(path2))
			{
				streamWriter3.WriteLine(string.Format(format2, text3, "GCDuration", "FPS"));
			}
		}
		using (StreamWriter streamWriter4 = new StreamWriter(path2, true))
		{
			streamWriter4.WriteLine(string.Format(format2, text4, num2, fps));
		}
		GenericGameSettings.instance.performanceCapture.waitTime = 0f;
		App.Quit();
	}

	// Token: 0x06006524 RID: 25892 RVA: 0x002D1600 File Offset: 0x002CF800
	public void Reset(GameSpawnData gsd, Vector2I world_offset)
	{
		using (new KProfiler.Region("World.Reset", null))
		{
			if (gsd != null)
			{
				foreach (KeyValuePair<Vector2I, bool> keyValuePair in gsd.preventFoWReveal)
				{
					if (keyValuePair.Value)
					{
						Vector2I v = new Vector2I(keyValuePair.Key.X + world_offset.X, keyValuePair.Key.Y + world_offset.Y);
						Grid.PreventFogOfWarReveal[Grid.PosToCell(v)] = keyValuePair.Value;
					}
				}
			}
		}
	}

	// Token: 0x06006525 RID: 25893 RVA: 0x002D16D8 File Offset: 0x002CF8D8
	private void OnApplicationQuit()
	{
		Game.quitting = true;
		Sim.Shutdown();
		AudioMixer.Destroy();
		if (this.screenMgr != null && this.screenMgr.gameObject != null)
		{
			UnityEngine.Object.Destroy(this.screenMgr.gameObject);
		}
		Console.WriteLine("Game.OnApplicationQuit()");
	}

	// Token: 0x06006526 RID: 25894 RVA: 0x002D1730 File Offset: 0x002CF930
	private void InitializeFXSpawners()
	{
		for (int i = 0; i < this.fxSpawnData.Length; i++)
		{
			int fx_idx = i;
			this.fxSpawnData[fx_idx].fxPrefab.SetActive(false);
			ushort fx_mask = (ushort)(1 << fx_idx);
			Action<SpawnFXHashes, GameObject> destroyer = delegate(SpawnFXHashes fxid, GameObject go)
			{
				if (!Game.IsQuitting())
				{
					int num = Grid.PosToCell(go);
					ushort[] array = this.activeFX;
					int num2 = num;
					array[num2] &= ~fx_mask;
					go.GetComponent<KAnimControllerBase>().enabled = false;
					this.fxPools[(int)fxid].ReleaseInstance(go);
				}
			};
			Func<GameObject> instantiator = delegate()
			{
				GameObject gameObject = GameUtil.KInstantiate(this.fxSpawnData[fx_idx].fxPrefab, Grid.SceneLayer.Front, null, 0);
				KBatchedAnimController component = gameObject.GetComponent<KBatchedAnimController>();
				component.enabled = false;
				gameObject.SetActive(true);
				component.onDestroySelf = delegate(GameObject go)
				{
					destroyer(this.fxSpawnData[fx_idx].id, go);
				};
				return gameObject;
			};
			GameObjectPool pool = new GameObjectPool(instantiator, this.fxSpawnData[fx_idx].initialCount);
			this.fxPools[(int)this.fxSpawnData[fx_idx].id] = pool;
			this.fxSpawner[(int)this.fxSpawnData[fx_idx].id] = delegate(Vector3 pos, float rotation)
			{
				Action<object> action = delegate(object obj)
				{
					int num = Grid.PosToCell(pos);
					if ((this.activeFX[num] & fx_mask) == 0)
					{
						ushort[] array = this.activeFX;
						int num2 = num;
						array[num2] |= fx_mask;
						GameObject instance = pool.GetInstance();
						Game.SpawnPoolData spawnPoolData = this.fxSpawnData[fx_idx];
						Quaternion rotation = Quaternion.identity;
						bool flipX = false;
						string s = spawnPoolData.initialAnim;
						Game.SpawnRotationConfig rotationConfig = spawnPoolData.rotationConfig;
						if (rotationConfig != Game.SpawnRotationConfig.Normal)
						{
							if (rotationConfig == Game.SpawnRotationConfig.StringName)
							{
								int num3 = (int)(rotation / 90f);
								if (num3 < 0)
								{
									num3 += spawnPoolData.rotationData.Length;
								}
								s = spawnPoolData.rotationData[num3].animName;
								flipX = spawnPoolData.rotationData[num3].flip;
							}
						}
						else
						{
							rotation = Quaternion.Euler(0f, 0f, rotation);
						}
						pos += spawnPoolData.spawnOffset;
						Vector2 vector = UnityEngine.Random.insideUnitCircle;
						vector.x *= spawnPoolData.spawnRandomOffset.x;
						vector.y *= spawnPoolData.spawnRandomOffset.y;
						vector = rotation * vector;
						pos.x += vector.x;
						pos.y += vector.y;
						instance.transform.SetPosition(pos);
						instance.transform.rotation = rotation;
						KBatchedAnimController component = instance.GetComponent<KBatchedAnimController>();
						component.FlipX = flipX;
						component.TintColour = spawnPoolData.colour;
						component.Play(s, KAnim.PlayMode.Once, 1f, 0f);
						component.enabled = true;
					}
				};
				if (Game.Instance.IsPaused)
				{
					action(null);
					return;
				}
				GameScheduler.Instance.Schedule("SpawnFX", 0f, action, null, null);
			};
		}
	}

	// Token: 0x06006527 RID: 25895 RVA: 0x002D1830 File Offset: 0x002CFA30
	public void SpawnFX(SpawnFXHashes fx_id, int cell, float rotation)
	{
		Vector3 vector = Grid.CellToPosCBC(cell, Grid.SceneLayer.Front);
		if (CameraController.Instance.IsVisiblePos(vector))
		{
			this.fxSpawner[(int)fx_id](vector, rotation);
		}
	}

	// Token: 0x06006528 RID: 25896 RVA: 0x000E67C9 File Offset: 0x000E49C9
	public void SpawnFX(SpawnFXHashes fx_id, Vector3 pos, float rotation)
	{
		this.fxSpawner[(int)fx_id](pos, rotation);
	}

	// Token: 0x06006529 RID: 25897 RVA: 0x000E67DE File Offset: 0x000E49DE
	public static void SaveSettings(BinaryWriter writer)
	{
		Serializer.Serialize(new Game.Settings(Game.Instance), writer);
	}

	// Token: 0x0600652A RID: 25898 RVA: 0x002D1868 File Offset: 0x002CFA68
	public static void LoadSettings(Deserializer deserializer)
	{
		Game.Settings settings = new Game.Settings();
		deserializer.Deserialize(settings);
		KPlayerPrefs.SetInt(Game.NextUniqueIDKey, settings.nextUniqueID);
		KleiMetrics.SetGameID(settings.gameID);
	}

	// Token: 0x0600652B RID: 25899 RVA: 0x002D18A0 File Offset: 0x002CFAA0
	public void Save(BinaryWriter writer)
	{
		Game.GameSaveData gameSaveData = new Game.GameSaveData();
		gameSaveData.gasConduitFlow = this.gasConduitFlow;
		gameSaveData.liquidConduitFlow = this.liquidConduitFlow;
		gameSaveData.fallingWater = this.world.GetComponent<FallingWater>();
		gameSaveData.unstableGround = this.world.GetComponent<UnstableGroundManager>();
		gameSaveData.worldDetail = SaveLoader.Instance.clusterDetailSave;
		gameSaveData.debugWasUsed = this.debugWasUsed;
		gameSaveData.customGameSettings = CustomGameSettings.Instance;
		gameSaveData.storySetings = StoryManager.Instance;
		gameSaveData.spaceScannerNetworkManager = Game.Instance.spaceScannerNetworkManager;
		gameSaveData.autoPrioritizeRoles = this.autoPrioritizeRoles;
		gameSaveData.advancedPersonalPriorities = this.advancedPersonalPriorities;
		gameSaveData.savedInfo = this.savedInfo;
		global::Debug.Assert(gameSaveData.worldDetail != null, "World detail null");
		gameSaveData.dateGenerated = this.dateGenerated;
		if (!this.changelistsPlayedOn.Contains(663500U))
		{
			this.changelistsPlayedOn.Add(663500U);
		}
		gameSaveData.changelistsPlayedOn = this.changelistsPlayedOn;
		if (this.OnSave != null)
		{
			this.OnSave(gameSaveData);
		}
		Serializer.Serialize(gameSaveData, writer);
	}

	// Token: 0x0600652C RID: 25900 RVA: 0x002D19BC File Offset: 0x002CFBBC
	public void Load(Deserializer deserializer)
	{
		Game.GameSaveData gameSaveData = new Game.GameSaveData();
		gameSaveData.gasConduitFlow = this.gasConduitFlow;
		gameSaveData.liquidConduitFlow = this.liquidConduitFlow;
		gameSaveData.fallingWater = this.world.GetComponent<FallingWater>();
		gameSaveData.unstableGround = this.world.GetComponent<UnstableGroundManager>();
		gameSaveData.worldDetail = new WorldDetailSave();
		gameSaveData.customGameSettings = CustomGameSettings.Instance;
		gameSaveData.storySetings = StoryManager.Instance;
		gameSaveData.spaceScannerNetworkManager = Game.Instance.spaceScannerNetworkManager;
		deserializer.Deserialize(gameSaveData);
		this.gasConduitFlow = gameSaveData.gasConduitFlow;
		this.liquidConduitFlow = gameSaveData.liquidConduitFlow;
		this.debugWasUsed = gameSaveData.debugWasUsed;
		this.autoPrioritizeRoles = gameSaveData.autoPrioritizeRoles;
		this.advancedPersonalPriorities = gameSaveData.advancedPersonalPriorities;
		this.dateGenerated = gameSaveData.dateGenerated;
		this.changelistsPlayedOn = (gameSaveData.changelistsPlayedOn ?? new List<uint>());
		if (gameSaveData.dateGenerated.IsNullOrWhiteSpace())
		{
			this.dateGenerated = "Before U41 (Feb 2022)";
		}
		DebugUtil.LogArgs(new object[]
		{
			"SAVEINFO"
		});
		DebugUtil.LogArgs(new object[]
		{
			" - Generated: " + this.dateGenerated
		});
		DebugUtil.LogArgs(new object[]
		{
			" - Played on: " + string.Join<uint>(", ", this.changelistsPlayedOn)
		});
		DebugUtil.LogArgs(new object[]
		{
			" - Debug was used: " + Game.Instance.debugWasUsed.ToString()
		});
		this.savedInfo = gameSaveData.savedInfo;
		this.savedInfo.InitializeEmptyVariables();
		CustomGameSettings.Instance.Print();
		KCrashReporter.debugWasUsed = this.debugWasUsed;
		SaveLoader.Instance.SetWorldDetail(gameSaveData.worldDetail);
		if (this.OnLoad != null)
		{
			this.OnLoad(gameSaveData);
		}
	}

	// Token: 0x0600652D RID: 25901 RVA: 0x000E67F0 File Offset: 0x000E49F0
	public void SetAutoSaveCallbacks(Game.SavingPreCB activatePreCB, Game.SavingActiveCB activateActiveCB, Game.SavingPostCB activatePostCB)
	{
		this.activatePreCB = activatePreCB;
		this.activateActiveCB = activateActiveCB;
		this.activatePostCB = activatePostCB;
	}

	// Token: 0x0600652E RID: 25902 RVA: 0x000E6807 File Offset: 0x000E4A07
	public void StartDelayedInitialSave()
	{
		base.StartCoroutine(this.DelayedInitialSave());
	}

	// Token: 0x0600652F RID: 25903 RVA: 0x000E6816 File Offset: 0x000E4A16
	private IEnumerator DelayedInitialSave()
	{
		int num;
		for (int i = 0; i < 1; i = num)
		{
			yield return null;
			num = i + 1;
		}
		if (GenericGameSettings.instance.devAutoWorldGenActive)
		{
			foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
			{
				worldContainer.SetDiscovered(true);
			}
			SaveGame.Instance.worldGenSpawner.SpawnEverything();
			SaveGame.Instance.GetSMI<ClusterFogOfWarManager.Instance>().DEBUG_REVEAL_ENTIRE_MAP();
			if (CameraController.Instance != null)
			{
				CameraController.Instance.EnableFreeCamera(true);
			}
			for (int num2 = 0; num2 != Grid.WidthInCells * Grid.HeightInCells; num2++)
			{
				Grid.Reveal(num2, byte.MaxValue, false);
			}
			GenericGameSettings.instance.devAutoWorldGenActive = false;
		}
		SaveLoader.Instance.InitialSave();
		yield break;
	}

	// Token: 0x06006530 RID: 25904 RVA: 0x002D1B88 File Offset: 0x002CFD88
	public void StartDelayedSave(string filename, bool isAutoSave = false, bool updateSavePointer = true)
	{
		if (this.activatePreCB != null)
		{
			this.activatePreCB(delegate
			{
				this.StartCoroutine(this.DelayedSave(filename, isAutoSave, updateSavePointer));
			});
			return;
		}
		base.StartCoroutine(this.DelayedSave(filename, isAutoSave, updateSavePointer));
	}

	// Token: 0x06006531 RID: 25905 RVA: 0x000E681E File Offset: 0x000E4A1E
	private IEnumerator DelayedSave(string filename, bool isAutoSave, bool updateSavePointer)
	{
		while (PlayerController.Instance.IsDragging())
		{
			yield return null;
		}
		PlayerController.Instance.CancelDragging();
		PlayerController.Instance.AllowDragging(false);
		int num;
		for (int i = 0; i < 1; i = num)
		{
			yield return null;
			num = i + 1;
		}
		if (this.activateActiveCB != null)
		{
			this.activateActiveCB();
			for (int i = 0; i < 1; i = num)
			{
				yield return null;
				num = i + 1;
			}
		}
		SaveLoader.Instance.Save(filename, isAutoSave, updateSavePointer);
		if (this.activatePostCB != null)
		{
			this.activatePostCB();
		}
		for (int i = 0; i < 5; i = num)
		{
			yield return null;
			num = i + 1;
		}
		PlayerController.Instance.AllowDragging(true);
		yield break;
	}

	// Token: 0x06006532 RID: 25906 RVA: 0x000E6842 File Offset: 0x000E4A42
	public void StartDelayed(int tick_delay, System.Action action)
	{
		base.StartCoroutine(this.DelayedExecutor(tick_delay, action));
	}

	// Token: 0x06006533 RID: 25907 RVA: 0x000E6853 File Offset: 0x000E4A53
	private IEnumerator DelayedExecutor(int tick_delay, System.Action action)
	{
		int num;
		for (int i = 0; i < tick_delay; i = num)
		{
			yield return null;
			num = i + 1;
		}
		action();
		yield break;
	}

	// Token: 0x06006534 RID: 25908 RVA: 0x002D1BF8 File Offset: 0x002CFDF8
	private void LoadEventHashes()
	{
		foreach (object obj in Enum.GetValues(typeof(GameHashes)))
		{
			GameHashes hash = (GameHashes)obj;
			HashCache.Get().Add((int)hash, hash.ToString());
		}
		foreach (object obj2 in Enum.GetValues(typeof(UtilHashes)))
		{
			UtilHashes hash2 = (UtilHashes)obj2;
			HashCache.Get().Add((int)hash2, hash2.ToString());
		}
		foreach (object obj3 in Enum.GetValues(typeof(UIHashes)))
		{
			UIHashes hash3 = (UIHashes)obj3;
			HashCache.Get().Add((int)hash3, hash3.ToString());
		}
	}

	// Token: 0x06006535 RID: 25909 RVA: 0x002D1D34 File Offset: 0x002CFF34
	public void StopFE()
	{
		if (SteamUGCService.Instance)
		{
			SteamUGCService.Instance.enabled = false;
		}
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().FrontEndSnapshot, STOP_MODE.ALLOWFADEOUT);
		if (MusicManager.instance.SongIsPlaying("Music_FrontEnd"))
		{
			MusicManager.instance.StopSong("Music_FrontEnd", true, STOP_MODE.ALLOWFADEOUT);
		}
		MainMenu.Instance.StopMainMenuMusic();
	}

	// Token: 0x06006536 RID: 25910 RVA: 0x000E6869 File Offset: 0x000E4A69
	public void StartBE()
	{
		Resources.UnloadUnusedAssets();
		AudioMixer.instance.Reset();
		AudioMixer.instance.StartPersistentSnapshots();
		MusicManager.instance.ConfigureSongs();
		if (MusicManager.instance.ShouldPlayDynamicMusicLoadedGame())
		{
			MusicManager.instance.PlayDynamicMusic();
		}
	}

	// Token: 0x06006537 RID: 25911 RVA: 0x002D1D9C File Offset: 0x002CFF9C
	public void StopBE()
	{
		if (SteamUGCService.Instance)
		{
			SteamUGCService.Instance.enabled = true;
		}
		LoopingSoundManager loopingSoundManager = LoopingSoundManager.Get();
		if (loopingSoundManager != null)
		{
			loopingSoundManager.StopAllSounds();
		}
		MusicManager.instance.KillAllSongs(STOP_MODE.ALLOWFADEOUT);
		AudioMixer.instance.StopPersistentSnapshots();
		foreach (List<SaveLoadRoot> list in SaveLoader.Instance.saveManager.GetLists().Values)
		{
			foreach (SaveLoadRoot saveLoadRoot in list)
			{
				if (saveLoadRoot.gameObject != null)
				{
					global::Util.KDestroyGameObject(saveLoadRoot.gameObject);
				}
			}
		}
		base.GetComponent<EntombedItemVisualizer>().Clear();
		SimTemperatureTransfer.ClearInstanceMap();
		StructureTemperatureComponents.ClearInstanceMap();
		ElementConsumer.ClearInstanceMap();
		KComponentSpawn.instance.comps.Clear();
		KInputHandler.Remove(Global.GetInputManager().GetDefaultController(), this.cameraController);
		KInputHandler.Remove(Global.GetInputManager().GetDefaultController(), this.playerController);
		Sim.Shutdown();
		SimAndRenderScheduler.instance.Reset();
		Resources.UnloadUnusedAssets();
	}

	// Token: 0x06006538 RID: 25912 RVA: 0x000E68A5 File Offset: 0x000E4AA5
	public void SetStatusItemOffset(Transform transform, Vector3 offset)
	{
		this.statusItemRenderer.SetOffset(transform, offset);
	}

	// Token: 0x06006539 RID: 25913 RVA: 0x000E68B4 File Offset: 0x000E4AB4
	public void AddStatusItem(Transform transform, StatusItem status_item)
	{
		this.statusItemRenderer.Add(transform, status_item);
	}

	// Token: 0x0600653A RID: 25914 RVA: 0x000E68C3 File Offset: 0x000E4AC3
	public void RemoveStatusItem(Transform transform, StatusItem status_item)
	{
		this.statusItemRenderer.Remove(transform, status_item);
	}

	// Token: 0x17000658 RID: 1624
	// (get) Token: 0x0600653B RID: 25915 RVA: 0x000E68D2 File Offset: 0x000E4AD2
	public float LastTimeWorkStarted
	{
		get
		{
			return this.lastTimeWorkStarted;
		}
	}

	// Token: 0x0600653C RID: 25916 RVA: 0x000E68DA File Offset: 0x000E4ADA
	public void StartedWork()
	{
		this.lastTimeWorkStarted = Time.time;
	}

	// Token: 0x0600653D RID: 25917 RVA: 0x000AA038 File Offset: 0x000A8238
	private void SpawnOxygenBubbles(Vector3 position, float angle)
	{
	}

	// Token: 0x0600653E RID: 25918 RVA: 0x000E68E7 File Offset: 0x000E4AE7
	public void ManualReleaseHandle(HandleVector<Game.CallbackInfo>.Handle handle)
	{
		if (!handle.IsValid())
		{
			return;
		}
		this.callbackManagerManuallyReleasedHandles.Add(handle.index);
		this.callbackManager.Release(handle);
	}

	// Token: 0x0600653F RID: 25919 RVA: 0x000E6912 File Offset: 0x000E4B12
	private bool IsManuallyReleasedHandle(HandleVector<Game.CallbackInfo>.Handle handle)
	{
		return !this.callbackManager.IsVersionValid(handle) && this.callbackManagerManuallyReleasedHandles.Contains(handle.index);
	}

	// Token: 0x06006540 RID: 25920 RVA: 0x000E6939 File Offset: 0x000E4B39
	[ContextMenu("Print")]
	private void Print()
	{
		Console.WriteLine("This is a console writeline test");
		global::Debug.Log("This is a debug log test");
	}

	// Token: 0x06006541 RID: 25921 RVA: 0x002D1EEC File Offset: 0x002D00EC
	private void DestroyInstances()
	{
		KMonoBehaviour.lastGameObject = null;
		KMonoBehaviour.lastObj = null;
		Db.Get().ResetProblematicDbs();
		GridSettings.ClearGrid();
		StateMachineManager.ResetParameters();
		ChoreTable.Instance.ResetParameters();
		BubbleManager.DestroyInstance();
		AmbientSoundManager.Destroy();
		AutoDisinfectableManager.DestroyInstance();
		BuildMenu.DestroyInstance();
		CancelTool.DestroyInstance();
		ClearTool.DestroyInstance();
		ChoreGroupManager.DestroyInstance();
		CO2Manager.DestroyInstance();
		ConsumerManager.DestroyInstance();
		CopySettingsTool.DestroyInstance();
		global::DateTime.DestroyInstance();
		DebugBaseTemplateButton.DestroyInstance();
		DebugPaintElementScreen.DestroyInstance();
		DetailsScreen.DestroyInstance();
		DietManager.DestroyInstance();
		DebugText.DestroyInstance();
		FactionManager.DestroyInstance();
		EmptyPipeTool.DestroyInstance();
		FetchListStatusItemUpdater.DestroyInstance();
		FishOvercrowingManager.DestroyInstance();
		FallingWater.DestroyInstance();
		GridCompositor.DestroyInstance();
		Infrared.DestroyInstance();
		KPrefabIDTracker.DestroyInstance();
		ManagementMenu.DestroyInstance();
		ClusterMapScreen.DestroyInstance();
		Messenger.DestroyInstance();
		LoopingSoundManager.DestroyInstance();
		MeterScreen.DestroyInstance();
		MinionGroupProber.DestroyInstance();
		NavPathDrawer.DestroyInstance();
		MinionIdentity.DestroyStatics();
		PathFinder.DestroyStatics();
		Pathfinding.DestroyInstance();
		PrebuildTool.DestroyInstance();
		PrioritizeTool.DestroyInstance();
		SelectTool.DestroyInstance();
		PopFXManager.DestroyInstance();
		ProgressBarsConfig.DestroyInstance();
		PropertyTextures.DestroyInstance();
		WorldResourceAmountTracker<RationTracker>.DestroyInstance();
		WorldResourceAmountTracker<ElectrobankTracker>.DestroyInstance();
		ReportManager.DestroyInstance();
		Research.DestroyInstance();
		RootMenu.DestroyInstance();
		SaveLoader.DestroyInstance();
		Scenario.DestroyInstance();
		SimDebugView.DestroyInstance();
		SpriteSheetAnimManager.DestroyInstance();
		ScheduleManager.DestroyInstance();
		Sounds.DestroyInstance();
		ToolMenu.DestroyInstance();
		WorldDamage.DestroyInstance();
		WaterCubes.DestroyInstance();
		WireBuildTool.DestroyInstance();
		VisibilityTester.DestroyInstance();
		Traces.DestroyInstance();
		TopLeftControlScreen.DestroyInstance();
		UtilityBuildTool.DestroyInstance();
		ReportScreen.DestroyInstance();
		ChorePreconditions.DestroyInstance();
		SandboxBrushTool.DestroyInstance();
		SandboxHeatTool.DestroyInstance();
		SandboxStressTool.DestroyInstance();
		SandboxCritterTool.DestroyInstance();
		SandboxClearFloorTool.DestroyInstance();
		GameScreenManager.DestroyInstance();
		GameScheduler.DestroyInstance();
		NavigationReservations.DestroyInstance();
		Tutorial.DestroyInstance();
		CameraController.DestroyInstance();
		CellEventLogger.DestroyInstance();
		GameFlowManager.DestroyInstance();
		Immigration.DestroyInstance();
		BuildTool.DestroyInstance();
		DebugTool.DestroyInstance();
		DeconstructTool.DestroyInstance();
		DisconnectTool.DestroyInstance();
		DigTool.DestroyInstance();
		DisinfectTool.DestroyInstance();
		HarvestTool.DestroyInstance();
		MopTool.DestroyInstance();
		MoveToLocationTool.DestroyInstance();
		PlaceTool.DestroyInstance();
		SpacecraftManager.DestroyInstance();
		GameplayEventManager.DestroyInstance();
		BuildingInventory.DestroyInstance();
		PlantSubSpeciesCatalog.DestroyInstance();
		SandboxDestroyerTool.DestroyInstance();
		SandboxFOWTool.DestroyInstance();
		SandboxFloodTool.DestroyInstance();
		SandboxSprinkleTool.DestroyInstance();
		StampTool.DestroyInstance();
		OnDemandUpdater.DestroyInstance();
		HoverTextScreen.DestroyInstance();
		ImmigrantScreen.DestroyInstance();
		OverlayMenu.DestroyInstance();
		NameDisplayScreen.DestroyInstance();
		PlanScreen.DestroyInstance();
		ResourceCategoryScreen.DestroyInstance();
		ResourceRemainingDisplayScreen.DestroyInstance();
		SandboxToolParameterMenu.DestroyInstance();
		SpeedControlScreen.DestroyInstance();
		Vignette.DestroyInstance();
		PlayerController.DestroyInstance();
		NotificationScreen.DestroyInstance();
		NotificationScreen_TemporaryActions.DestroyInstance();
		BuildingCellVisualizerResources.DestroyInstance();
		PauseScreen.DestroyInstance();
		SaveLoadRoot.DestroyStatics();
		KTime.DestroyInstance();
		DemoTimer.DestroyInstance();
		UIScheduler.DestroyInstance();
		SaveGame.DestroyInstance();
		GameClock.DestroyInstance();
		TimeOfDay.DestroyInstance();
		DeserializeWarnings.DestroyInstance();
		UISounds.DestroyInstance();
		RenderTextureDestroyer.DestroyInstance();
		HoverTextHelper.DestroyStatics();
		LoadScreen.DestroyInstance();
		LoadingOverlay.DestroyInstance();
		SimAndRenderScheduler.DestroyInstance();
		Singleton<CellChangeMonitor>.DestroyInstance();
		Singleton<StateMachineManager>.Instance.Clear();
		Singleton<StateMachineUpdater>.Instance.Clear();
		UpdateObjectCountParameter.Clear();
		MaterialSelectionPanel.ClearStatics();
		StarmapScreen.DestroyInstance();
		ClusterNameDisplayScreen.DestroyInstance();
		ClusterManager.DestroyInstance();
		ClusterGrid.DestroyInstance();
		PathFinderQueries.Reset();
		KBatchedAnimUpdater instance = Singleton<KBatchedAnimUpdater>.Instance;
		if (instance != null)
		{
			instance.InitializeGrid();
		}
		GlobalChoreProvider.DestroyInstance();
		WorldSelector.DestroyInstance();
		ColonyDiagnosticUtility.DestroyInstance();
		DiscoveredResources.DestroyInstance();
		ClusterMapSelectTool.DestroyInstance();
		StoryManager.DestroyInstance();
		AnimEventHandlerManager.DestroyInstance();
		Game.Instance = null;
		Game.BrainScheduler = null;
		Grid.OnReveal = null;
		this.VisualTunerElement = null;
		Assets.ClearOnAddPrefab();
		KMonoBehaviour.lastGameObject = null;
		KMonoBehaviour.lastObj = null;
		(KComponentSpawn.instance.comps as GameComps).Clear();
	}

	// Token: 0x06006542 RID: 25922 RVA: 0x002D2234 File Offset: 0x002D0434
	public static bool IsDlcActiveForCurrentSave(string dlcId)
	{
		if (Game.Instance == null)
		{
			DebugUtil.DevLogError("Game.IsDlcActiveForCurrentSave can only be called when the game is running");
			return false;
		}
		return dlcId == "" || dlcId == null || SaveLoader.Instance.GameInfo.dlcIds.Contains(dlcId);
	}

	// Token: 0x06006543 RID: 25923 RVA: 0x000E694F File Offset: 0x000E4B4F
	public static bool IsCorrectDlcActiveForCurrentSave(IHasDlcRestrictions restrictions)
	{
		if (Game.Instance == null)
		{
			DebugUtil.DevLogError("Game.IsCorrectDlcActiveForCurrentSave can only be called when the game is running");
			return false;
		}
		return Game.IsAllDlcActiveForCurrentSave(restrictions.GetRequiredDlcIds()) && !Game.IsAnyDlcActiveForCurrentSave(restrictions.GetForbiddenDlcIds());
	}

	// Token: 0x06006544 RID: 25924 RVA: 0x002D2284 File Offset: 0x002D0484
	private static bool IsAllDlcActiveForCurrentSave(string[] dlcIds)
	{
		if (dlcIds == null || dlcIds.Length == 0)
		{
			return true;
		}
		foreach (string text in dlcIds)
		{
			if (!(text == "") && !Game.IsDlcActiveForCurrentSave(text))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06006545 RID: 25925 RVA: 0x002D22C8 File Offset: 0x002D04C8
	private static bool IsAnyDlcActiveForCurrentSave(string[] dlcIds)
	{
		if (dlcIds == null || dlcIds.Length == 0)
		{
			return false;
		}
		foreach (string text in dlcIds)
		{
			if (!(text == "") && Game.IsDlcActiveForCurrentSave(text))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x040048AB RID: 18603
	private static readonly Thread MainThread = Thread.CurrentThread;

	// Token: 0x040048AC RID: 18604
	private static readonly string NextUniqueIDKey = "NextUniqueID";

	// Token: 0x040048AD RID: 18605
	public static string clusterId = null;

	// Token: 0x040048AE RID: 18606
	private PlayerController playerController;

	// Token: 0x040048AF RID: 18607
	private CameraController cameraController;

	// Token: 0x040048B0 RID: 18608
	public Action<Game.GameSaveData> OnSave;

	// Token: 0x040048B1 RID: 18609
	public Action<Game.GameSaveData> OnLoad;

	// Token: 0x040048B2 RID: 18610
	public System.Action OnSpawnComplete;

	// Token: 0x040048B3 RID: 18611
	[NonSerialized]
	public bool baseAlreadyCreated;

	// Token: 0x040048B4 RID: 18612
	[NonSerialized]
	public bool autoPrioritizeRoles;

	// Token: 0x040048B5 RID: 18613
	[NonSerialized]
	public bool advancedPersonalPriorities;

	// Token: 0x040048B6 RID: 18614
	public Game.SavedInfo savedInfo;

	// Token: 0x040048B7 RID: 18615
	public static bool quitting = false;

	// Token: 0x040048B9 RID: 18617
	public AssignmentManager assignmentManager;

	// Token: 0x040048BA RID: 18618
	public GameObject playerPrefab;

	// Token: 0x040048BB RID: 18619
	public GameObject screenManagerPrefab;

	// Token: 0x040048BC RID: 18620
	public GameObject cameraControllerPrefab;

	// Token: 0x040048BE RID: 18622
	private static Camera m_CachedCamera = null;

	// Token: 0x040048BF RID: 18623
	public GameObject tempIntroScreenPrefab;

	// Token: 0x040048C0 RID: 18624
	public static int BlockSelectionLayerMask;

	// Token: 0x040048C1 RID: 18625
	public static int PickupableLayer;

	// Token: 0x040048C2 RID: 18626
	public static BrainScheduler BrainScheduler;

	// Token: 0x040048C3 RID: 18627
	public Element VisualTunerElement;

	// Token: 0x040048C4 RID: 18628
	public float currentFallbackSunlightIntensity;

	// Token: 0x040048C5 RID: 18629
	public RoomProber roomProber;

	// Token: 0x040048C6 RID: 18630
	public SpaceScannerNetworkManager spaceScannerNetworkManager;

	// Token: 0x040048C7 RID: 18631
	public FetchManager fetchManager;

	// Token: 0x040048C8 RID: 18632
	public EdiblesManager ediblesManager;

	// Token: 0x040048C9 RID: 18633
	public SpacecraftManager spacecraftManager;

	// Token: 0x040048CA RID: 18634
	public UserMenu userMenu;

	// Token: 0x040048CB RID: 18635
	public Unlocks unlocks;

	// Token: 0x040048CC RID: 18636
	public Timelapser timelapser;

	// Token: 0x040048CD RID: 18637
	private bool sandboxModeActive;

	// Token: 0x040048CE RID: 18638
	public HandleVector<Game.CallbackInfo> callbackManager = new HandleVector<Game.CallbackInfo>(256);

	// Token: 0x040048CF RID: 18639
	public List<int> callbackManagerManuallyReleasedHandles = new List<int>();

	// Token: 0x040048D0 RID: 18640
	public Game.ComplexCallbackHandleVector<int> simComponentCallbackManager = new Game.ComplexCallbackHandleVector<int>(256);

	// Token: 0x040048D1 RID: 18641
	public Game.ComplexCallbackHandleVector<Sim.MassConsumedCallback> massConsumedCallbackManager = new Game.ComplexCallbackHandleVector<Sim.MassConsumedCallback>(64);

	// Token: 0x040048D2 RID: 18642
	public Game.ComplexCallbackHandleVector<Sim.MassEmittedCallback> massEmitCallbackManager = new Game.ComplexCallbackHandleVector<Sim.MassEmittedCallback>(64);

	// Token: 0x040048D3 RID: 18643
	public Game.ComplexCallbackHandleVector<Sim.DiseaseConsumptionCallback> diseaseConsumptionCallbackManager = new Game.ComplexCallbackHandleVector<Sim.DiseaseConsumptionCallback>(64);

	// Token: 0x040048D4 RID: 18644
	public Game.ComplexCallbackHandleVector<Sim.ConsumedRadiationCallback> radiationConsumedCallbackManager = new Game.ComplexCallbackHandleVector<Sim.ConsumedRadiationCallback>(256);

	// Token: 0x040048D5 RID: 18645
	[NonSerialized]
	public Player LocalPlayer;

	// Token: 0x040048D6 RID: 18646
	[SerializeField]
	public TextAsset maleNamesFile;

	// Token: 0x040048D7 RID: 18647
	[SerializeField]
	public TextAsset femaleNamesFile;

	// Token: 0x040048D8 RID: 18648
	[NonSerialized]
	public World world;

	// Token: 0x040048D9 RID: 18649
	[NonSerialized]
	public CircuitManager circuitManager;

	// Token: 0x040048DA RID: 18650
	[NonSerialized]
	public EnergySim energySim;

	// Token: 0x040048DB RID: 18651
	[NonSerialized]
	public LogicCircuitManager logicCircuitManager;

	// Token: 0x040048DC RID: 18652
	private GameScreenManager screenMgr;

	// Token: 0x040048DD RID: 18653
	public UtilityNetworkManager<FlowUtilityNetwork, Vent> gasConduitSystem;

	// Token: 0x040048DE RID: 18654
	public UtilityNetworkManager<FlowUtilityNetwork, Vent> liquidConduitSystem;

	// Token: 0x040048DF RID: 18655
	public UtilityNetworkManager<ElectricalUtilityNetwork, Wire> electricalConduitSystem;

	// Token: 0x040048E0 RID: 18656
	public UtilityNetworkManager<LogicCircuitNetwork, LogicWire> logicCircuitSystem;

	// Token: 0x040048E1 RID: 18657
	public UtilityNetworkTubesManager travelTubeSystem;

	// Token: 0x040048E2 RID: 18658
	public UtilityNetworkManager<FlowUtilityNetwork, SolidConduit> solidConduitSystem;

	// Token: 0x040048E3 RID: 18659
	public ConduitFlow gasConduitFlow;

	// Token: 0x040048E4 RID: 18660
	public ConduitFlow liquidConduitFlow;

	// Token: 0x040048E5 RID: 18661
	public SolidConduitFlow solidConduitFlow;

	// Token: 0x040048E6 RID: 18662
	public Accumulators accumulators;

	// Token: 0x040048E7 RID: 18663
	public PlantElementAbsorbers plantElementAbsorbers;

	// Token: 0x040048E8 RID: 18664
	public Game.TemperatureOverlayModes temperatureOverlayMode;

	// Token: 0x040048E9 RID: 18665
	public bool showExpandedTemperatures;

	// Token: 0x040048EA RID: 18666
	public List<Tag> tileOverlayFilters = new List<Tag>();

	// Token: 0x040048EB RID: 18667
	public bool showGasConduitDisease;

	// Token: 0x040048EC RID: 18668
	public bool showLiquidConduitDisease;

	// Token: 0x040048ED RID: 18669
	public ConduitFlowVisualizer gasFlowVisualizer;

	// Token: 0x040048EE RID: 18670
	public ConduitFlowVisualizer liquidFlowVisualizer;

	// Token: 0x040048EF RID: 18671
	public SolidConduitFlowVisualizer solidFlowVisualizer;

	// Token: 0x040048F0 RID: 18672
	public ConduitTemperatureManager conduitTemperatureManager;

	// Token: 0x040048F1 RID: 18673
	public ConduitDiseaseManager conduitDiseaseManager;

	// Token: 0x040048F2 RID: 18674
	public MingleCellTracker mingleCellTracker;

	// Token: 0x040048F3 RID: 18675
	private int simSubTick;

	// Token: 0x040048F4 RID: 18676
	private bool hasFirstSimTickRun;

	// Token: 0x040048F5 RID: 18677
	private float simDt;

	// Token: 0x040048F6 RID: 18678
	public string dateGenerated;

	// Token: 0x040048F7 RID: 18679
	public List<uint> changelistsPlayedOn;

	// Token: 0x040048F8 RID: 18680
	[SerializeField]
	public Game.ConduitVisInfo liquidConduitVisInfo;

	// Token: 0x040048F9 RID: 18681
	[SerializeField]
	public Game.ConduitVisInfo gasConduitVisInfo;

	// Token: 0x040048FA RID: 18682
	[SerializeField]
	public Game.ConduitVisInfo solidConduitVisInfo;

	// Token: 0x040048FB RID: 18683
	[SerializeField]
	private Material liquidFlowMaterial;

	// Token: 0x040048FC RID: 18684
	[SerializeField]
	private Material gasFlowMaterial;

	// Token: 0x040048FD RID: 18685
	[SerializeField]
	private Color flowColour;

	// Token: 0x040048FE RID: 18686
	private Vector3 gasFlowPos;

	// Token: 0x040048FF RID: 18687
	private Vector3 liquidFlowPos;

	// Token: 0x04004900 RID: 18688
	private Vector3 solidFlowPos;

	// Token: 0x04004901 RID: 18689
	public bool drawStatusItems = true;

	// Token: 0x04004902 RID: 18690
	private List<SolidInfo> solidInfo = new List<SolidInfo>();

	// Token: 0x04004903 RID: 18691
	private List<Klei.CallbackInfo> callbackInfo = new List<Klei.CallbackInfo>();

	// Token: 0x04004904 RID: 18692
	private List<SolidInfo> gameSolidInfo = new List<SolidInfo>();

	// Token: 0x04004905 RID: 18693
	private bool IsPaused;

	// Token: 0x04004906 RID: 18694
	private HashSet<int> solidChangedFilter = new HashSet<int>();

	// Token: 0x04004907 RID: 18695
	private HashedString lastDrawnOverlayMode;

	// Token: 0x04004908 RID: 18696
	private EntityCellVisualizer previewVisualizer;

	// Token: 0x0400490B RID: 18699
	public SafetyConditions safetyConditions = new SafetyConditions();

	// Token: 0x0400490C RID: 18700
	public SimData simData = new SimData();

	// Token: 0x0400490D RID: 18701
	[MyCmpGet]
	private GameScenePartitioner gameScenePartitioner;

	// Token: 0x0400490E RID: 18702
	private bool gameStarted;

	// Token: 0x0400490F RID: 18703
	private static readonly EventSystem.IntraObjectHandler<Game> MarkStatusItemRendererDirtyDelegate = new EventSystem.IntraObjectHandler<Game>(delegate(Game component, object data)
	{
		component.MarkStatusItemRendererDirty(data);
	});

	// Token: 0x04004910 RID: 18704
	private static readonly EventSystem.IntraObjectHandler<Game> ActiveWorldChangedDelegate = new EventSystem.IntraObjectHandler<Game>(delegate(Game component, object data)
	{
		component.ForceOverlayUpdate(true);
	});

	// Token: 0x04004911 RID: 18705
	private ushort[] activeFX;

	// Token: 0x04004912 RID: 18706
	public bool debugWasUsed;

	// Token: 0x04004913 RID: 18707
	private bool isLoading;

	// Token: 0x04004914 RID: 18708
	private List<Game.SimActiveRegion> simActiveRegions = new List<Game.SimActiveRegion>();

	// Token: 0x04004915 RID: 18709
	private HashedString previousOverlayMode = OverlayModes.None.ID;

	// Token: 0x04004916 RID: 18710
	private float previousGasConduitFlowDiscreteLerpPercent = -1f;

	// Token: 0x04004917 RID: 18711
	private float previousLiquidConduitFlowDiscreteLerpPercent = -1f;

	// Token: 0x04004918 RID: 18712
	private float previousSolidConduitFlowDiscreteLerpPercent = -1f;

	// Token: 0x04004919 RID: 18713
	[SerializeField]
	private Game.SpawnPoolData[] fxSpawnData;

	// Token: 0x0400491A RID: 18714
	private Dictionary<int, Action<Vector3, float>> fxSpawner = new Dictionary<int, Action<Vector3, float>>();

	// Token: 0x0400491B RID: 18715
	private Dictionary<int, GameObjectPool> fxPools = new Dictionary<int, GameObjectPool>();

	// Token: 0x0400491C RID: 18716
	private Game.SavingPreCB activatePreCB;

	// Token: 0x0400491D RID: 18717
	private Game.SavingActiveCB activateActiveCB;

	// Token: 0x0400491E RID: 18718
	private Game.SavingPostCB activatePostCB;

	// Token: 0x0400491F RID: 18719
	[SerializeField]
	public Game.UIColours uiColours = new Game.UIColours();

	// Token: 0x04004920 RID: 18720
	private float lastTimeWorkStarted = float.NegativeInfinity;

	// Token: 0x02001343 RID: 4931
	[Serializable]
	public struct SavedInfo
	{
		// Token: 0x06006549 RID: 25929 RVA: 0x000E6994 File Offset: 0x000E4B94
		[OnDeserialized]
		private void OnDeserialized()
		{
			this.InitializeEmptyVariables();
		}

		// Token: 0x0600654A RID: 25930 RVA: 0x000E699C File Offset: 0x000E4B9C
		public void InitializeEmptyVariables()
		{
			if (this.creaturePoopAmount == null)
			{
				this.creaturePoopAmount = new Dictionary<Tag, float>();
			}
			if (this.powerCreatedbyGeneratorType == null)
			{
				this.powerCreatedbyGeneratorType = new Dictionary<Tag, float>();
			}
		}

		// Token: 0x04004921 RID: 18721
		public bool discoveredSurface;

		// Token: 0x04004922 RID: 18722
		public bool discoveredOilField;

		// Token: 0x04004923 RID: 18723
		public bool curedDisease;

		// Token: 0x04004924 RID: 18724
		public bool blockedCometWithBunkerDoor;

		// Token: 0x04004925 RID: 18725
		public Dictionary<Tag, float> creaturePoopAmount;

		// Token: 0x04004926 RID: 18726
		public Dictionary<Tag, float> powerCreatedbyGeneratorType;
	}

	// Token: 0x02001344 RID: 4932
	public struct CallbackInfo
	{
		// Token: 0x0600654B RID: 25931 RVA: 0x000E69C4 File Offset: 0x000E4BC4
		public CallbackInfo(System.Action cb, bool manually_release = false)
		{
			this.cb = cb;
			this.manuallyRelease = manually_release;
		}

		// Token: 0x04004927 RID: 18727
		public System.Action cb;

		// Token: 0x04004928 RID: 18728
		public bool manuallyRelease;
	}

	// Token: 0x02001345 RID: 4933
	public struct ComplexCallbackInfo<DataType>
	{
		// Token: 0x0600654C RID: 25932 RVA: 0x000E69D4 File Offset: 0x000E4BD4
		public ComplexCallbackInfo(Action<DataType, object> cb, object callback_data, string debug_info)
		{
			this.cb = cb;
			this.debugInfo = debug_info;
			this.callbackData = callback_data;
		}

		// Token: 0x04004929 RID: 18729
		public Action<DataType, object> cb;

		// Token: 0x0400492A RID: 18730
		public object callbackData;

		// Token: 0x0400492B RID: 18731
		public string debugInfo;
	}

	// Token: 0x02001346 RID: 4934
	public class ComplexCallbackHandleVector<DataType>
	{
		// Token: 0x0600654D RID: 25933 RVA: 0x000E69EB File Offset: 0x000E4BEB
		public ComplexCallbackHandleVector(int initial_size)
		{
			this.baseMgr = new HandleVector<Game.ComplexCallbackInfo<DataType>>(initial_size);
		}

		// Token: 0x0600654E RID: 25934 RVA: 0x000E6A0A File Offset: 0x000E4C0A
		public HandleVector<Game.ComplexCallbackInfo<DataType>>.Handle Add(Action<DataType, object> cb, object callback_data, string debug_info)
		{
			return this.baseMgr.Add(new Game.ComplexCallbackInfo<DataType>(cb, callback_data, debug_info));
		}

		// Token: 0x0600654F RID: 25935 RVA: 0x002D24A0 File Offset: 0x002D06A0
		public Game.ComplexCallbackInfo<DataType> GetItem(HandleVector<Game.ComplexCallbackInfo<DataType>>.Handle handle)
		{
			Game.ComplexCallbackInfo<DataType> item;
			try
			{
				item = this.baseMgr.GetItem(handle);
			}
			catch (Exception ex)
			{
				byte b;
				int key;
				this.baseMgr.UnpackHandleUnchecked(handle, out b, out key);
				string str = null;
				if (this.releaseInfo.TryGetValue(key, out str))
				{
					KCrashReporter.Assert(false, "Trying to get data for handle that was already released by " + str, null);
				}
				else
				{
					KCrashReporter.Assert(false, "Trying to get data for handle that was released ...... magically", null);
				}
				throw ex;
			}
			return item;
		}

		// Token: 0x06006550 RID: 25936 RVA: 0x002D2510 File Offset: 0x002D0710
		public Game.ComplexCallbackInfo<DataType> Release(HandleVector<Game.ComplexCallbackInfo<DataType>>.Handle handle, string release_info)
		{
			Game.ComplexCallbackInfo<DataType> result;
			try
			{
				byte b;
				int key;
				this.baseMgr.UnpackHandle(handle, out b, out key);
				this.releaseInfo[key] = release_info;
				result = this.baseMgr.Release(handle);
			}
			catch (Exception ex)
			{
				byte b;
				int key;
				this.baseMgr.UnpackHandleUnchecked(handle, out b, out key);
				string str = null;
				if (this.releaseInfo.TryGetValue(key, out str))
				{
					KCrashReporter.Assert(false, release_info + "is trying to release handle but it was already released by " + str, null);
				}
				else
				{
					KCrashReporter.Assert(false, release_info + "is trying to release a handle that was already released by some unknown thing", null);
				}
				throw ex;
			}
			return result;
		}

		// Token: 0x06006551 RID: 25937 RVA: 0x000E6A1F File Offset: 0x000E4C1F
		public void Clear()
		{
			this.baseMgr.Clear();
		}

		// Token: 0x06006552 RID: 25938 RVA: 0x000E6A2C File Offset: 0x000E4C2C
		public bool IsVersionValid(HandleVector<Game.ComplexCallbackInfo<DataType>>.Handle handle)
		{
			return this.baseMgr.IsVersionValid(handle);
		}

		// Token: 0x0400492C RID: 18732
		private HandleVector<Game.ComplexCallbackInfo<DataType>> baseMgr;

		// Token: 0x0400492D RID: 18733
		private Dictionary<int, string> releaseInfo = new Dictionary<int, string>();
	}

	// Token: 0x02001347 RID: 4935
	public enum TemperatureOverlayModes
	{
		// Token: 0x0400492F RID: 18735
		AbsoluteTemperature,
		// Token: 0x04004930 RID: 18736
		AdaptiveTemperature,
		// Token: 0x04004931 RID: 18737
		HeatFlow,
		// Token: 0x04004932 RID: 18738
		StateChange,
		// Token: 0x04004933 RID: 18739
		RelativeTemperature
	}

	// Token: 0x02001348 RID: 4936
	[Serializable]
	public class ConduitVisInfo
	{
		// Token: 0x04004934 RID: 18740
		public GameObject prefab;

		// Token: 0x04004935 RID: 18741
		[Header("Main View")]
		public Color32 tint;

		// Token: 0x04004936 RID: 18742
		public Color32 insulatedTint;

		// Token: 0x04004937 RID: 18743
		public Color32 radiantTint;

		// Token: 0x04004938 RID: 18744
		[Header("Overlay")]
		public string overlayTintName;

		// Token: 0x04004939 RID: 18745
		public string overlayInsulatedTintName;

		// Token: 0x0400493A RID: 18746
		public string overlayRadiantTintName;

		// Token: 0x0400493B RID: 18747
		public Vector2 overlayMassScaleRange = new Vector2f(1f, 1000f);

		// Token: 0x0400493C RID: 18748
		public Vector2 overlayMassScaleValues = new Vector2f(0.1f, 1f);
	}

	// Token: 0x02001349 RID: 4937
	private class WorldRegion
	{
		// Token: 0x17000659 RID: 1625
		// (get) Token: 0x06006554 RID: 25940 RVA: 0x000E6A76 File Offset: 0x000E4C76
		public Vector2I regionMin
		{
			get
			{
				return this.min;
			}
		}

		// Token: 0x1700065A RID: 1626
		// (get) Token: 0x06006555 RID: 25941 RVA: 0x000E6A7E File Offset: 0x000E4C7E
		public Vector2I regionMax
		{
			get
			{
				return this.max;
			}
		}

		// Token: 0x06006556 RID: 25942 RVA: 0x002D25A4 File Offset: 0x002D07A4
		public void UpdateGameActiveRegion(int x0, int y0, int x1, int y1)
		{
			this.min.x = Mathf.Max(0, x0);
			this.min.y = Mathf.Max(0, y0);
			this.max.x = Mathf.Max(x1, this.regionMax.x);
			this.max.y = Mathf.Max(y1, this.regionMax.y);
		}

		// Token: 0x06006557 RID: 25943 RVA: 0x000E6A86 File Offset: 0x000E4C86
		public void UpdateGameActiveRegion(Vector2I simActiveRegionMin, Vector2I simActiveRegionMax)
		{
			this.min = simActiveRegionMin;
			this.max = simActiveRegionMax;
		}

		// Token: 0x0400493D RID: 18749
		private Vector2I min;

		// Token: 0x0400493E RID: 18750
		private Vector2I max;

		// Token: 0x0400493F RID: 18751
		public bool isActive;
	}

	// Token: 0x0200134A RID: 4938
	public class SimActiveRegion
	{
		// Token: 0x06006559 RID: 25945 RVA: 0x000E6A96 File Offset: 0x000E4C96
		public SimActiveRegion()
		{
			this.region = default(Pair<Vector2I, Vector2I>);
			this.currentSunlightIntensity = (float)FIXEDTRAITS.SUNLIGHT.DEFAULT_VALUE;
			this.currentCosmicRadiationIntensity = (float)FIXEDTRAITS.COSMICRADIATION.DEFAULT_VALUE;
		}

		// Token: 0x04004940 RID: 18752
		public Pair<Vector2I, Vector2I> region;

		// Token: 0x04004941 RID: 18753
		public float currentSunlightIntensity;

		// Token: 0x04004942 RID: 18754
		public float currentCosmicRadiationIntensity;
	}

	// Token: 0x0200134B RID: 4939
	private enum SpawnRotationConfig
	{
		// Token: 0x04004944 RID: 18756
		Normal,
		// Token: 0x04004945 RID: 18757
		StringName
	}

	// Token: 0x0200134C RID: 4940
	[Serializable]
	private struct SpawnRotationData
	{
		// Token: 0x04004946 RID: 18758
		public string animName;

		// Token: 0x04004947 RID: 18759
		public bool flip;
	}

	// Token: 0x0200134D RID: 4941
	[Serializable]
	private struct SpawnPoolData
	{
		// Token: 0x04004948 RID: 18760
		[HashedEnum]
		public SpawnFXHashes id;

		// Token: 0x04004949 RID: 18761
		public int initialCount;

		// Token: 0x0400494A RID: 18762
		public Color32 colour;

		// Token: 0x0400494B RID: 18763
		public GameObject fxPrefab;

		// Token: 0x0400494C RID: 18764
		public string initialAnim;

		// Token: 0x0400494D RID: 18765
		public Vector3 spawnOffset;

		// Token: 0x0400494E RID: 18766
		public Vector2 spawnRandomOffset;

		// Token: 0x0400494F RID: 18767
		public Game.SpawnRotationConfig rotationConfig;

		// Token: 0x04004950 RID: 18768
		public Game.SpawnRotationData[] rotationData;
	}

	// Token: 0x0200134E RID: 4942
	[Serializable]
	private class Settings
	{
		// Token: 0x0600655A RID: 25946 RVA: 0x000E6AC2 File Offset: 0x000E4CC2
		public Settings(Game game)
		{
			this.nextUniqueID = KPrefabID.NextUniqueID;
			this.gameID = KleiMetrics.GameID();
		}

		// Token: 0x0600655B RID: 25947 RVA: 0x000AA024 File Offset: 0x000A8224
		public Settings()
		{
		}

		// Token: 0x04004951 RID: 18769
		public int nextUniqueID;

		// Token: 0x04004952 RID: 18770
		public int gameID;
	}

	// Token: 0x0200134F RID: 4943
	public class GameSaveData
	{
		// Token: 0x04004953 RID: 18771
		public ConduitFlow gasConduitFlow;

		// Token: 0x04004954 RID: 18772
		public ConduitFlow liquidConduitFlow;

		// Token: 0x04004955 RID: 18773
		public FallingWater fallingWater;

		// Token: 0x04004956 RID: 18774
		public UnstableGroundManager unstableGround;

		// Token: 0x04004957 RID: 18775
		public WorldDetailSave worldDetail;

		// Token: 0x04004958 RID: 18776
		public CustomGameSettings customGameSettings;

		// Token: 0x04004959 RID: 18777
		public StoryManager storySetings;

		// Token: 0x0400495A RID: 18778
		public SpaceScannerNetworkManager spaceScannerNetworkManager;

		// Token: 0x0400495B RID: 18779
		public bool debugWasUsed;

		// Token: 0x0400495C RID: 18780
		public bool autoPrioritizeRoles;

		// Token: 0x0400495D RID: 18781
		public bool advancedPersonalPriorities;

		// Token: 0x0400495E RID: 18782
		public Game.SavedInfo savedInfo;

		// Token: 0x0400495F RID: 18783
		public string dateGenerated;

		// Token: 0x04004960 RID: 18784
		public List<uint> changelistsPlayedOn;
	}

	// Token: 0x02001350 RID: 4944
	// (Invoke) Token: 0x0600655E RID: 25950
	public delegate void CansaveCB();

	// Token: 0x02001351 RID: 4945
	// (Invoke) Token: 0x06006562 RID: 25954
	public delegate void SavingPreCB(Game.CansaveCB cb);

	// Token: 0x02001352 RID: 4946
	// (Invoke) Token: 0x06006566 RID: 25958
	public delegate void SavingActiveCB();

	// Token: 0x02001353 RID: 4947
	// (Invoke) Token: 0x0600656A RID: 25962
	public delegate void SavingPostCB();

	// Token: 0x02001354 RID: 4948
	[Serializable]
	public struct LocationColours
	{
		// Token: 0x04004961 RID: 18785
		public Color unreachable;

		// Token: 0x04004962 RID: 18786
		public Color invalidLocation;

		// Token: 0x04004963 RID: 18787
		public Color validLocation;

		// Token: 0x04004964 RID: 18788
		public Color requiresRole;

		// Token: 0x04004965 RID: 18789
		public Color unreachable_requiresRole;
	}

	// Token: 0x02001355 RID: 4949
	[Serializable]
	public class UIColours
	{
		// Token: 0x1700065B RID: 1627
		// (get) Token: 0x0600656D RID: 25965 RVA: 0x000E6AE0 File Offset: 0x000E4CE0
		public Game.LocationColours Dig
		{
			get
			{
				return this.digColours;
			}
		}

		// Token: 0x1700065C RID: 1628
		// (get) Token: 0x0600656E RID: 25966 RVA: 0x000E6AE8 File Offset: 0x000E4CE8
		public Game.LocationColours Build
		{
			get
			{
				return this.buildColours;
			}
		}

		// Token: 0x04004966 RID: 18790
		[SerializeField]
		private Game.LocationColours digColours;

		// Token: 0x04004967 RID: 18791
		[SerializeField]
		private Game.LocationColours buildColours;
	}
}
