﻿using System;
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

[AddComponentMenu("KMonoBehaviour/scripts/Game")]
public class Game : KMonoBehaviour
{
	public static bool IsOnMainThread()
	{
		return Game.MainThread == Thread.CurrentThread;
	}

	public static bool IsQuitting()
	{
		return Game.quitting;
	}

	public KInputHandler inputHandler { get; set; }

	public static Game Instance { get; private set; }

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

	public bool DebugOnlyBuildingsAllowed
	{
		get
		{
			return DebugHandler.enabled && (this.SandboxModeActive || DebugHandler.InstantBuildMode);
		}
	}

	public StatusItemRenderer statusItemRenderer { get; private set; }

	public PrioritizableRenderer prioritizableRenderer { get; private set; }

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
		this.changelistsPlayedOn.Add(674504U);
		this.dateGenerated = System.DateTime.UtcNow.ToString("U", CultureInfo.InvariantCulture);
	}

	public void SetGameStarted()
	{
		this.gameStarted = true;
	}

	public bool GameStarted()
	{
		return this.gameStarted;
	}

	private IEnumerator SanityCheckBoundsNextFrame()
	{
		yield return null;
		using (List<WorldContainer>.Enumerator enumerator = ClusterManager.Instance.WorldContainers.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				WorldContainer worldContainer = enumerator.Current;
				if (worldContainer.IsDiscovered && !worldContainer.IsModuleInterior)
				{
					for (int i = worldContainer.WorldOffset.X; i < worldContainer.WorldOffset.X + worldContainer.WorldSize.X; i++)
					{
						for (int j = 0; j < Grid.TopBorderHeight; j++)
						{
							int num = Grid.XYToCell(i, worldContainer.WorldOffset.Y + worldContainer.WorldSize.Y - j);
							if (Grid.IsSolidCell(num) && Grid.Element[num].id != SimHashes.Unobtanium)
							{
								SimMessages.Dig(num, -1, true);
							}
						}
					}
				}
			}
			yield break;
		}
		yield break;
	}

	private void UnsafePrefabInit()
	{
		this.StepTheSim(0f);
		base.StartCoroutine(this.SanityCheckBoundsNextFrame());
	}

	protected override void OnLoadLevel()
	{
		base.Unsubscribe<Game>(1798162660, Game.MarkStatusItemRendererDirtyDelegate, false);
		base.Unsubscribe<Game>(1983128072, Game.ActiveWorldChangedDelegate, false);
		base.OnLoadLevel();
	}

	private void MarkStatusItemRendererDirty(object data)
	{
		this.statusItemRenderer.MarkAllDirty();
	}

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

	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		SimAndRenderScheduler.instance.Remove(KComponentSpawn.instance);
		SimAndRenderScheduler.instance.RegisterBatchUpdate<ISim200ms, AmountInstance>(null);
		SimAndRenderScheduler.instance.RegisterBatchUpdate<ISim1000ms, SolidTransferArm>(null);
		this.DestroyInstances();
	}

	private new void OnDestroy()
	{
		base.OnDestroy();
		this.DestroyInstances();
	}

	private void UnsafeOnSpawn()
	{
		this.world.UpdateCellInfo(this.gameSolidInfo, this.callbackInfo, 0, null, 0, null);
	}

	private void RefreshRadiationLoop()
	{
		GameScheduler.Instance.Schedule("UpdateRadiation", 1f, delegate(object obj)
		{
			RadiationGridManager.Refresh();
			this.RefreshRadiationLoop();
		}, null, null);
	}

	public void SetMusicEnabled(bool enabled)
	{
		if (enabled)
		{
			MusicManager.instance.PlaySong("Music_FrontEnd", false);
			return;
		}
		MusicManager.instance.StopSong("Music_FrontEnd", true, STOP_MODE.ALLOWFADEOUT);
	}

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

	public void SetDupePassableSolid(int cell, bool passable, bool solid)
	{
		Grid.DupePassable[cell] = passable;
		this.gameSolidInfo.Add(new SolidInfo(cell, solid));
	}

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
						bool flag = solidInfo.isSolid != 0;
						Grid.SetSolid(solidInfo.cellIdx, flag, CellEventLogger.Instance.SimMessagesSolid);
						if (flag && Grid.IsWorldValidCell(solidInfo.cellIdx))
						{
							int num = (int)Grid.WorldIdx[solidInfo.cellIdx];
							if (num >= 0 && num < ClusterManager.Instance.WorldContainers.Count)
							{
								WorldContainer worldContainer = ClusterManager.Instance.WorldContainers[num];
								int num2;
								int num3;
								Grid.CellToXY(solidInfo.cellIdx, out num2, out num3);
								if (!worldContainer.IsModuleInterior && num3 > worldContainer.WorldOffset.Y + worldContainer.WorldSize.Y - Grid.TopBorderHeight)
								{
									SimMessages.Dig(solidInfo.cellIdx, -1, true);
								}
							}
						}
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
				for (int num4 = 0; num4 < numSpawnFXInfo; num4++)
				{
					Sim.SpawnFXInfo spawnFXInfo = ptr->spawnFXInfo[num4];
					this.SpawnFX((SpawnFXHashes)spawnFXInfo.fxHash, spawnFXInfo.cellIdx, spawnFXInfo.rotation);
				}
				UnstableGroundManager component2 = this.world.GetComponent<UnstableGroundManager>();
				int numUnstableCellInfo = ptr->numUnstableCellInfo;
				for (int num5 = 0; num5 < numUnstableCellInfo; num5++)
				{
					Sim.UnstableCellInfo unstableCellInfo = ptr->unstableCellInfo[num5];
					if (unstableCellInfo.fallingInfo == 0)
					{
						component2.Spawn(unstableCellInfo.cellIdx, ElementLoader.elements[(int)unstableCellInfo.elemIdx], unstableCellInfo.mass, unstableCellInfo.temperature, unstableCellInfo.diseaseIdx, unstableCellInfo.diseaseCount);
					}
				}
				int numWorldDamageInfo = ptr->numWorldDamageInfo;
				for (int num6 = 0; num6 < numWorldDamageInfo; num6++)
				{
					Sim.WorldDamageInfo damage_info = ptr->worldDamageInfo[num6];
					WorldDamage.Instance.ApplyDamage(damage_info);
				}
				for (int num7 = 0; num7 < ptr->numRemovedMassEntries; num7++)
				{
					ElementConsumer.AddMass(ptr->removedMassEntries[num7]);
				}
				int numMassConsumedCallbacks = ptr->numMassConsumedCallbacks;
				HandleVector<Game.ComplexCallbackInfo<Sim.MassConsumedCallback>>.Handle handle2 = default(HandleVector<Game.ComplexCallbackInfo<Sim.MassConsumedCallback>>.Handle);
				for (int num8 = 0; num8 < numMassConsumedCallbacks; num8++)
				{
					Sim.MassConsumedCallback massConsumedCallback = ptr->massConsumedCallbacks[num8];
					handle2.index = massConsumedCallback.callbackIdx;
					Game.ComplexCallbackInfo<Sim.MassConsumedCallback> complexCallbackInfo = this.massConsumedCallbackManager.Release(handle2, "massConsumedCB");
					if (complexCallbackInfo.cb != null)
					{
						complexCallbackInfo.cb(massConsumedCallback, complexCallbackInfo.callbackData);
					}
				}
				int numMassEmittedCallbacks = ptr->numMassEmittedCallbacks;
				HandleVector<Game.ComplexCallbackInfo<Sim.MassEmittedCallback>>.Handle handle3 = default(HandleVector<Game.ComplexCallbackInfo<Sim.MassEmittedCallback>>.Handle);
				for (int num9 = 0; num9 < numMassEmittedCallbacks; num9++)
				{
					Sim.MassEmittedCallback massEmittedCallback = ptr->massEmittedCallbacks[num9];
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
				for (int num10 = 0; num10 < numDiseaseConsumptionCallbacks; num10++)
				{
					Sim.DiseaseConsumptionCallback diseaseConsumptionCallback = ptr->diseaseConsumptionCallbacks[num10];
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
				for (int num11 = 0; num11 < numComponentStateChangedMessages; num11++)
				{
					Sim.ComponentStateChangedMessage componentStateChangedMessage = ptr->componentStateChangedMessages[num11];
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
				for (int num12 = 0; num12 < numRadiationConsumedCallbacks; num12++)
				{
					Sim.ConsumedRadiationCallback consumedRadiationCallback = ptr->radiationConsumedCallbacks[num12];
					handle6.index = consumedRadiationCallback.callbackIdx;
					Game.ComplexCallbackInfo<Sim.ConsumedRadiationCallback> complexCallbackInfo3 = this.radiationConsumedCallbackManager.Release(handle6, "radiationConsumedCB");
					if (complexCallbackInfo3.cb != null)
					{
						complexCallbackInfo3.cb(consumedRadiationCallback, complexCallbackInfo3.callbackData);
					}
				}
				int numElementChunkMeltedInfos = ptr->numElementChunkMeltedInfos;
				for (int num13 = 0; num13 < numElementChunkMeltedInfos; num13++)
				{
					SimTemperatureTransfer.DoOreMeltTransition(ptr->elementChunkMeltedInfos[num13].handle);
				}
				int numBuildingOverheatInfos = ptr->numBuildingOverheatInfos;
				for (int num14 = 0; num14 < numBuildingOverheatInfos; num14++)
				{
					StructureTemperatureComponents.DoOverheat(ptr->buildingOverheatInfos[num14].handle);
				}
				int numBuildingNoLongerOverheatedInfos = ptr->numBuildingNoLongerOverheatedInfos;
				for (int num15 = 0; num15 < numBuildingNoLongerOverheatedInfos; num15++)
				{
					StructureTemperatureComponents.DoNoLongerOverheated(ptr->buildingNoLongerOverheatedInfos[num15].handle);
				}
				int numBuildingMeltedInfos = ptr->numBuildingMeltedInfos;
				for (int num16 = 0; num16 < numBuildingMeltedInfos; num16++)
				{
					StructureTemperatureComponents.DoStateTransition(ptr->buildingMeltedInfos[num16].handle);
				}
				int numCellMeltedInfos = ptr->numCellMeltedInfos;
				for (int num17 = 0; num17 < numCellMeltedInfos; num17++)
				{
					int gameCell = ptr->cellMeltedInfos[num17].gameCell;
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

	public void AddSolidChangedFilter(int cell)
	{
		this.solidChangedFilter.Add(cell);
	}

	public void RemoveSolidChangedFilter(int cell)
	{
		this.solidChangedFilter.Remove(cell);
	}

	public void SetIsLoading()
	{
		this.isLoading = true;
	}

	public bool IsLoading()
	{
		return this.isLoading;
	}

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

	public void ForceSimStep()
	{
		DebugUtil.LogArgs(new object[]
		{
			"Force-stepping the sim"
		});
		this.simDt = 0.2f;
	}

	private void Update()
	{
		if (this.isLoading)
		{
			return;
		}
		SuperluminalPerf.BeginEvent("Game.Update", null);
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
		SuperluminalPerf.EndEvent();
	}

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

	private void LateUpdateComponents()
	{
		this.UpdateOverlayScreen();
	}

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

	private void OnRemoveBuildingCellVisualizer(EntityCellVisualizer entity_cell_visualizer)
	{
		if (this.previewVisualizer == entity_cell_visualizer)
		{
			this.previewVisualizer = null;
		}
	}

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

	public void ForceOverlayUpdate(bool clearLastMode = false)
	{
		this.previousOverlayMode = OverlayModes.None.ID;
		if (clearLastMode)
		{
			this.lastDrawnOverlayMode = OverlayModes.None.ID;
		}
	}

	private void LateUpdate()
	{
		SuperluminalPerf.BeginEvent("Game.LateUpdate", null);
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
		Vector4 value = new Vector4((vector.x - (float)worldOffset.x) / (float)worldSize.x, (vector.y - (float)worldOffset.y) / (float)(worldSize.y - activeWorld.HiddenYOffset), (vector2.x - vector.x) / (float)worldSize.x, (vector2.y - vector.y) / (float)(worldSize.y - activeWorld.HiddenYOffset));
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
		SuperluminalPerf.EndEvent();
		if (GenericGameSettings.instance.performanceCapture.waitTime != 0f)
		{
			this.UpdatePerformanceCapture();
		}
	}

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
		uint num = 674504U;
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

	public void SpawnFX(SpawnFXHashes fx_id, int cell, float rotation)
	{
		Vector3 vector = Grid.CellToPosCBC(cell, Grid.SceneLayer.Front);
		if (CameraController.Instance.IsVisiblePos(vector))
		{
			this.fxSpawner[(int)fx_id](vector, rotation);
		}
	}

	public void SpawnFX(SpawnFXHashes fx_id, Vector3 pos, float rotation)
	{
		this.fxSpawner[(int)fx_id](pos, rotation);
	}

	public static void SaveSettings(BinaryWriter writer)
	{
		Serializer.Serialize(new Game.Settings(Game.Instance), writer);
	}

	public static void LoadSettings(Deserializer deserializer)
	{
		Game.Settings settings = new Game.Settings();
		deserializer.Deserialize(settings);
		KPlayerPrefs.SetInt(Game.NextUniqueIDKey, settings.nextUniqueID);
		KleiMetrics.SetGameID(settings.gameID);
	}

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
		if (!this.changelistsPlayedOn.Contains(674504U))
		{
			this.changelistsPlayedOn.Add(674504U);
		}
		gameSaveData.changelistsPlayedOn = this.changelistsPlayedOn;
		if (this.OnSave != null)
		{
			this.OnSave(gameSaveData);
		}
		Serializer.Serialize(gameSaveData, writer);
	}

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

	public void SetAutoSaveCallbacks(Game.SavingPreCB activatePreCB, Game.SavingActiveCB activateActiveCB, Game.SavingPostCB activatePostCB)
	{
		this.activatePreCB = activatePreCB;
		this.activateActiveCB = activateActiveCB;
		this.activatePostCB = activatePostCB;
	}

	public void StartDelayedInitialSave()
	{
		base.StartCoroutine(this.DelayedInitialSave());
	}

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

	public void StartDelayed(int tick_delay, System.Action action)
	{
		base.StartCoroutine(this.DelayedExecutor(tick_delay, action));
	}

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

	public void SetStatusItemOffset(Transform transform, Vector3 offset)
	{
		this.statusItemRenderer.SetOffset(transform, offset);
	}

	public void AddStatusItem(Transform transform, StatusItem status_item)
	{
		this.statusItemRenderer.Add(transform, status_item);
	}

	public void RemoveStatusItem(Transform transform, StatusItem status_item)
	{
		this.statusItemRenderer.Remove(transform, status_item);
	}

	public float LastTimeWorkStarted
	{
		get
		{
			return this.lastTimeWorkStarted;
		}
	}

	public void StartedWork()
	{
		this.lastTimeWorkStarted = Time.time;
	}

	private void SpawnOxygenBubbles(Vector3 position, float angle)
	{
	}

	public void ManualReleaseHandle(HandleVector<Game.CallbackInfo>.Handle handle)
	{
		if (!handle.IsValid())
		{
			return;
		}
		this.callbackManagerManuallyReleasedHandles.Add(handle.index);
		this.callbackManager.Release(handle);
	}

	private bool IsManuallyReleasedHandle(HandleVector<Game.CallbackInfo>.Handle handle)
	{
		return !this.callbackManager.IsVersionValid(handle) && this.callbackManagerManuallyReleasedHandles.Contains(handle.index);
	}

	[ContextMenu("Print")]
	private void Print()
	{
		Console.WriteLine("This is a console writeline test");
		global::Debug.Log("This is a debug log test");
	}

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

	public static bool IsDlcActiveForCurrentSave(string dlcId)
	{
		if (Game.Instance == null)
		{
			DebugUtil.DevLogError("Game.IsDlcActiveForCurrentSave can only be called when the game is running");
			return false;
		}
		return dlcId == "" || dlcId == null || SaveLoader.Instance.GameInfo.dlcIds.Contains(dlcId);
	}

	public static bool IsCorrectDlcActiveForCurrentSave(IHasDlcRestrictions restrictions)
	{
		if (Game.Instance == null)
		{
			DebugUtil.DevLogError("Game.IsCorrectDlcActiveForCurrentSave can only be called when the game is running");
			return false;
		}
		return Game.IsAllDlcActiveForCurrentSave(restrictions.GetRequiredDlcIds()) && !Game.IsAnyDlcActiveForCurrentSave(restrictions.GetForbiddenDlcIds());
	}

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

	private static readonly Thread MainThread = Thread.CurrentThread;

	private static readonly string NextUniqueIDKey = "NextUniqueID";

	public static string clusterId = null;

	private PlayerController playerController;

	private CameraController cameraController;

	public Action<Game.GameSaveData> OnSave;

	public Action<Game.GameSaveData> OnLoad;

	public System.Action OnSpawnComplete;

	[NonSerialized]
	public bool baseAlreadyCreated;

	[NonSerialized]
	public bool autoPrioritizeRoles;

	[NonSerialized]
	public bool advancedPersonalPriorities;

	public Game.SavedInfo savedInfo;

	public static bool quitting = false;

	public AssignmentManager assignmentManager;

	public GameObject playerPrefab;

	public GameObject screenManagerPrefab;

	public GameObject cameraControllerPrefab;

	private static Camera m_CachedCamera = null;

	public GameObject tempIntroScreenPrefab;

	public static int BlockSelectionLayerMask;

	public static int PickupableLayer;

	public static BrainScheduler BrainScheduler;

	public Element VisualTunerElement;

	public float currentFallbackSunlightIntensity;

	public RoomProber roomProber;

	public SpaceScannerNetworkManager spaceScannerNetworkManager;

	public FetchManager fetchManager;

	public EdiblesManager ediblesManager;

	public SpacecraftManager spacecraftManager;

	public UserMenu userMenu;

	public Unlocks unlocks;

	public Timelapser timelapser;

	private bool sandboxModeActive;

	public HandleVector<Game.CallbackInfo> callbackManager = new HandleVector<Game.CallbackInfo>(256);

	public List<int> callbackManagerManuallyReleasedHandles = new List<int>();

	public Game.ComplexCallbackHandleVector<int> simComponentCallbackManager = new Game.ComplexCallbackHandleVector<int>(256);

	public Game.ComplexCallbackHandleVector<Sim.MassConsumedCallback> massConsumedCallbackManager = new Game.ComplexCallbackHandleVector<Sim.MassConsumedCallback>(64);

	public Game.ComplexCallbackHandleVector<Sim.MassEmittedCallback> massEmitCallbackManager = new Game.ComplexCallbackHandleVector<Sim.MassEmittedCallback>(64);

	public Game.ComplexCallbackHandleVector<Sim.DiseaseConsumptionCallback> diseaseConsumptionCallbackManager = new Game.ComplexCallbackHandleVector<Sim.DiseaseConsumptionCallback>(64);

	public Game.ComplexCallbackHandleVector<Sim.ConsumedRadiationCallback> radiationConsumedCallbackManager = new Game.ComplexCallbackHandleVector<Sim.ConsumedRadiationCallback>(256);

	[NonSerialized]
	public Player LocalPlayer;

	[SerializeField]
	public TextAsset maleNamesFile;

	[SerializeField]
	public TextAsset femaleNamesFile;

	[NonSerialized]
	public World world;

	[NonSerialized]
	public CircuitManager circuitManager;

	[NonSerialized]
	public EnergySim energySim;

	[NonSerialized]
	public LogicCircuitManager logicCircuitManager;

	private GameScreenManager screenMgr;

	public UtilityNetworkManager<FlowUtilityNetwork, Vent> gasConduitSystem;

	public UtilityNetworkManager<FlowUtilityNetwork, Vent> liquidConduitSystem;

	public UtilityNetworkManager<ElectricalUtilityNetwork, Wire> electricalConduitSystem;

	public UtilityNetworkManager<LogicCircuitNetwork, LogicWire> logicCircuitSystem;

	public UtilityNetworkTubesManager travelTubeSystem;

	public UtilityNetworkManager<FlowUtilityNetwork, SolidConduit> solidConduitSystem;

	public ConduitFlow gasConduitFlow;

	public ConduitFlow liquidConduitFlow;

	public SolidConduitFlow solidConduitFlow;

	public Accumulators accumulators;

	public PlantElementAbsorbers plantElementAbsorbers;

	public Game.TemperatureOverlayModes temperatureOverlayMode;

	public bool showExpandedTemperatures;

	public List<Tag> tileOverlayFilters = new List<Tag>();

	public bool showGasConduitDisease;

	public bool showLiquidConduitDisease;

	public ConduitFlowVisualizer gasFlowVisualizer;

	public ConduitFlowVisualizer liquidFlowVisualizer;

	public SolidConduitFlowVisualizer solidFlowVisualizer;

	public ConduitTemperatureManager conduitTemperatureManager;

	public ConduitDiseaseManager conduitDiseaseManager;

	public MingleCellTracker mingleCellTracker;

	private int simSubTick;

	private bool hasFirstSimTickRun;

	private float simDt;

	public string dateGenerated;

	public List<uint> changelistsPlayedOn;

	[SerializeField]
	public Game.ConduitVisInfo liquidConduitVisInfo;

	[SerializeField]
	public Game.ConduitVisInfo gasConduitVisInfo;

	[SerializeField]
	public Game.ConduitVisInfo solidConduitVisInfo;

	[SerializeField]
	private Material liquidFlowMaterial;

	[SerializeField]
	private Material gasFlowMaterial;

	[SerializeField]
	private Color flowColour;

	private Vector3 gasFlowPos;

	private Vector3 liquidFlowPos;

	private Vector3 solidFlowPos;

	public bool drawStatusItems = true;

	private List<SolidInfo> solidInfo = new List<SolidInfo>();

	private List<Klei.CallbackInfo> callbackInfo = new List<Klei.CallbackInfo>();

	private List<SolidInfo> gameSolidInfo = new List<SolidInfo>();

	private bool IsPaused;

	private HashSet<int> solidChangedFilter = new HashSet<int>();

	private HashedString lastDrawnOverlayMode;

	private EntityCellVisualizer previewVisualizer;

	public SafetyConditions safetyConditions = new SafetyConditions();

	public SimData simData = new SimData();

	[MyCmpGet]
	private GameScenePartitioner gameScenePartitioner;

	private bool gameStarted;

	private static readonly EventSystem.IntraObjectHandler<Game> MarkStatusItemRendererDirtyDelegate = new EventSystem.IntraObjectHandler<Game>(delegate(Game component, object data)
	{
		component.MarkStatusItemRendererDirty(data);
	});

	private static readonly EventSystem.IntraObjectHandler<Game> ActiveWorldChangedDelegate = new EventSystem.IntraObjectHandler<Game>(delegate(Game component, object data)
	{
		component.ForceOverlayUpdate(true);
	});

	private ushort[] activeFX;

	public bool debugWasUsed;

	private bool isLoading;

	private List<Game.SimActiveRegion> simActiveRegions = new List<Game.SimActiveRegion>();

	private HashedString previousOverlayMode = OverlayModes.None.ID;

	private float previousGasConduitFlowDiscreteLerpPercent = -1f;

	private float previousLiquidConduitFlowDiscreteLerpPercent = -1f;

	private float previousSolidConduitFlowDiscreteLerpPercent = -1f;

	[SerializeField]
	private Game.SpawnPoolData[] fxSpawnData;

	private Dictionary<int, Action<Vector3, float>> fxSpawner = new Dictionary<int, Action<Vector3, float>>();

	private Dictionary<int, GameObjectPool> fxPools = new Dictionary<int, GameObjectPool>();

	private Game.SavingPreCB activatePreCB;

	private Game.SavingActiveCB activateActiveCB;

	private Game.SavingPostCB activatePostCB;

	[SerializeField]
	public Game.UIColours uiColours = new Game.UIColours();

	private float lastTimeWorkStarted = float.NegativeInfinity;

	[Serializable]
	public struct SavedInfo
	{
		[OnDeserialized]
		private void OnDeserialized()
		{
			this.InitializeEmptyVariables();
		}

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

		public bool discoveredSurface;

		public bool discoveredOilField;

		public bool curedDisease;

		public bool blockedCometWithBunkerDoor;

		public Dictionary<Tag, float> creaturePoopAmount;

		public Dictionary<Tag, float> powerCreatedbyGeneratorType;
	}

	public struct CallbackInfo
	{
		public CallbackInfo(System.Action cb, bool manually_release = false)
		{
			this.cb = cb;
			this.manuallyRelease = manually_release;
		}

		public System.Action cb;

		public bool manuallyRelease;
	}

	public struct ComplexCallbackInfo<DataType>
	{
		public ComplexCallbackInfo(Action<DataType, object> cb, object callback_data, string debug_info)
		{
			this.cb = cb;
			this.debugInfo = debug_info;
			this.callbackData = callback_data;
		}

		public Action<DataType, object> cb;

		public object callbackData;

		public string debugInfo;
	}

	public class ComplexCallbackHandleVector<DataType>
	{
		public ComplexCallbackHandleVector(int initial_size)
		{
			this.baseMgr = new HandleVector<Game.ComplexCallbackInfo<DataType>>(initial_size);
		}

		public HandleVector<Game.ComplexCallbackInfo<DataType>>.Handle Add(Action<DataType, object> cb, object callback_data, string debug_info)
		{
			return this.baseMgr.Add(new Game.ComplexCallbackInfo<DataType>(cb, callback_data, debug_info));
		}

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

		public void Clear()
		{
			this.baseMgr.Clear();
		}

		public bool IsVersionValid(HandleVector<Game.ComplexCallbackInfo<DataType>>.Handle handle)
		{
			return this.baseMgr.IsVersionValid(handle);
		}

		private HandleVector<Game.ComplexCallbackInfo<DataType>> baseMgr;

		private Dictionary<int, string> releaseInfo = new Dictionary<int, string>();
	}

	public enum TemperatureOverlayModes
	{
		AbsoluteTemperature,
		AdaptiveTemperature,
		HeatFlow,
		StateChange,
		RelativeTemperature
	}

	[Serializable]
	public class ConduitVisInfo
	{
		public GameObject prefab;

		[Header("Main View")]
		public Color32 tint;

		public Color32 insulatedTint;

		public Color32 radiantTint;

		[Header("Overlay")]
		public string overlayTintName;

		public string overlayInsulatedTintName;

		public string overlayRadiantTintName;

		public Vector2 overlayMassScaleRange = new Vector2f(1f, 1000f);

		public Vector2 overlayMassScaleValues = new Vector2f(0.1f, 1f);
	}

	private class WorldRegion
	{
		public Vector2I regionMin
		{
			get
			{
				return this.min;
			}
		}

		public Vector2I regionMax
		{
			get
			{
				return this.max;
			}
		}

		public void UpdateGameActiveRegion(int x0, int y0, int x1, int y1)
		{
			this.min.x = Mathf.Max(0, x0);
			this.min.y = Mathf.Max(0, y0);
			this.max.x = Mathf.Max(x1, this.regionMax.x);
			this.max.y = Mathf.Max(y1, this.regionMax.y);
		}

		public void UpdateGameActiveRegion(Vector2I simActiveRegionMin, Vector2I simActiveRegionMax)
		{
			this.min = simActiveRegionMin;
			this.max = simActiveRegionMax;
		}

		private Vector2I min;

		private Vector2I max;

		public bool isActive;
	}

	public class SimActiveRegion
	{
		public SimActiveRegion()
		{
			this.region = default(Pair<Vector2I, Vector2I>);
			this.currentSunlightIntensity = (float)FIXEDTRAITS.SUNLIGHT.DEFAULT_VALUE;
			this.currentCosmicRadiationIntensity = (float)FIXEDTRAITS.COSMICRADIATION.DEFAULT_VALUE;
		}

		public Pair<Vector2I, Vector2I> region;

		public float currentSunlightIntensity;

		public float currentCosmicRadiationIntensity;
	}

	private enum SpawnRotationConfig
	{
		Normal,
		StringName
	}

	[Serializable]
	private struct SpawnRotationData
	{
		public string animName;

		public bool flip;
	}

	[Serializable]
	private struct SpawnPoolData
	{
		[HashedEnum]
		public SpawnFXHashes id;

		public int initialCount;

		public Color32 colour;

		public GameObject fxPrefab;

		public string initialAnim;

		public Vector3 spawnOffset;

		public Vector2 spawnRandomOffset;

		public Game.SpawnRotationConfig rotationConfig;

		public Game.SpawnRotationData[] rotationData;
	}

	[Serializable]
	private class Settings
	{
		public Settings(Game game)
		{
			this.nextUniqueID = KPrefabID.NextUniqueID;
			this.gameID = KleiMetrics.GameID();
		}

		public Settings()
		{
		}

		public int nextUniqueID;

		public int gameID;
	}

	public class GameSaveData
	{
		public ConduitFlow gasConduitFlow;

		public ConduitFlow liquidConduitFlow;

		public FallingWater fallingWater;

		public UnstableGroundManager unstableGround;

		public WorldDetailSave worldDetail;

		public CustomGameSettings customGameSettings;

		public StoryManager storySetings;

		public SpaceScannerNetworkManager spaceScannerNetworkManager;

		public bool debugWasUsed;

		public bool autoPrioritizeRoles;

		public bool advancedPersonalPriorities;

		public Game.SavedInfo savedInfo;

		public string dateGenerated;

		public List<uint> changelistsPlayedOn;
	}

	public delegate void CansaveCB();

	public delegate void SavingPreCB(Game.CansaveCB cb);

	public delegate void SavingActiveCB();

	public delegate void SavingPostCB();

	[Serializable]
	public struct LocationColours
	{
		public Color unreachable;

		public Color invalidLocation;

		public Color validLocation;

		public Color requiresRole;

		public Color unreachable_requiresRole;
	}

	[Serializable]
	public class UIColours
	{
		public Game.LocationColours Dig
		{
			get
			{
				return this.digColours;
			}
		}

		public Game.LocationColours Build
		{
			get
			{
				return this.buildColours;
			}
		}

		[SerializeField]
		private Game.LocationColours digColours;

		[SerializeField]
		private Game.LocationColours buildColours;
	}
}
