using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

// Token: 0x020009CE RID: 2510
[AddComponentMenu("KMonoBehaviour/scripts/CameraController")]
public class CameraController : KMonoBehaviour, IInputHandler
{
	// Token: 0x170001A7 RID: 423
	// (get) Token: 0x06002D25 RID: 11557 RVA: 0x000C1B5E File Offset: 0x000BFD5E
	public string handlerName
	{
		get
		{
			return base.gameObject.name;
		}
	}

	// Token: 0x170001A8 RID: 424
	// (get) Token: 0x06002D26 RID: 11558 RVA: 0x000C1B6B File Offset: 0x000BFD6B
	// (set) Token: 0x06002D27 RID: 11559 RVA: 0x001FBE1C File Offset: 0x001FA01C
	public float OrthographicSize
	{
		get
		{
			if (!(this.baseCamera == null))
			{
				return this.baseCamera.orthographicSize;
			}
			return 0f;
		}
		set
		{
			for (int i = 0; i < this.cameras.Count; i++)
			{
				this.cameras[i].orthographicSize = value;
			}
		}
	}

	// Token: 0x170001A9 RID: 425
	// (get) Token: 0x06002D28 RID: 11560 RVA: 0x000C1B8C File Offset: 0x000BFD8C
	// (set) Token: 0x06002D29 RID: 11561 RVA: 0x000C1B94 File Offset: 0x000BFD94
	public KInputHandler inputHandler { get; set; }

	// Token: 0x170001AA RID: 426
	// (get) Token: 0x06002D2A RID: 11562 RVA: 0x000C1B9D File Offset: 0x000BFD9D
	// (set) Token: 0x06002D2B RID: 11563 RVA: 0x000C1BA5 File Offset: 0x000BFDA5
	public float targetOrthographicSize { get; private set; }

	// Token: 0x170001AB RID: 427
	// (get) Token: 0x06002D2C RID: 11564 RVA: 0x000C1BAE File Offset: 0x000BFDAE
	// (set) Token: 0x06002D2D RID: 11565 RVA: 0x000C1BB6 File Offset: 0x000BFDB6
	public bool isTargetPosSet { get; set; }

	// Token: 0x170001AC RID: 428
	// (get) Token: 0x06002D2E RID: 11566 RVA: 0x000C1BBF File Offset: 0x000BFDBF
	// (set) Token: 0x06002D2F RID: 11567 RVA: 0x000C1BC7 File Offset: 0x000BFDC7
	public Vector3 targetPos { get; private set; }

	// Token: 0x170001AD RID: 429
	// (get) Token: 0x06002D30 RID: 11568 RVA: 0x000C1BD0 File Offset: 0x000BFDD0
	// (set) Token: 0x06002D31 RID: 11569 RVA: 0x000C1BD8 File Offset: 0x000BFDD8
	public bool ignoreClusterFX { get; private set; }

	// Token: 0x06002D32 RID: 11570 RVA: 0x000C1BE1 File Offset: 0x000BFDE1
	public void ToggleClusterFX()
	{
		this.ignoreClusterFX = !this.ignoreClusterFX;
	}

	// Token: 0x06002D33 RID: 11571 RVA: 0x001FBE54 File Offset: 0x001FA054
	protected override void OnForcedCleanUp()
	{
		GameInputManager inputManager = Global.GetInputManager();
		if (inputManager == null)
		{
			return;
		}
		inputManager.usedMenus.Remove(this);
	}

	// Token: 0x170001AE RID: 430
	// (get) Token: 0x06002D34 RID: 11572 RVA: 0x000C1BF2 File Offset: 0x000BFDF2
	public int cameraActiveCluster
	{
		get
		{
			if (ClusterManager.Instance == null)
			{
				return 255;
			}
			return ClusterManager.Instance.activeWorldId;
		}
	}

	// Token: 0x06002D35 RID: 11573 RVA: 0x001FBE78 File Offset: 0x001FA078
	public void GetWorldCamera(out Vector2I worldOffset, out Vector2I worldSize)
	{
		WorldContainer worldContainer = null;
		if (ClusterManager.Instance != null)
		{
			worldContainer = ClusterManager.Instance.activeWorld;
		}
		if (!this.ignoreClusterFX && worldContainer != null)
		{
			worldOffset = worldContainer.WorldOffset;
			worldSize = worldContainer.WorldSize;
			return;
		}
		worldOffset = new Vector2I(0, 0);
		worldSize = new Vector2I(Grid.WidthInCells, Grid.HeightInCells);
	}

	// Token: 0x170001AF RID: 431
	// (get) Token: 0x06002D36 RID: 11574 RVA: 0x000C1C11 File Offset: 0x000BFE11
	// (set) Token: 0x06002D37 RID: 11575 RVA: 0x000C1C19 File Offset: 0x000BFE19
	public bool DisableUserCameraControl
	{
		get
		{
			return this.userCameraControlDisabled;
		}
		set
		{
			this.userCameraControlDisabled = value;
			if (this.userCameraControlDisabled)
			{
				this.panning = false;
				this.panLeft = false;
				this.panRight = false;
				this.panUp = false;
				this.panDown = false;
			}
		}
	}

	// Token: 0x170001B0 RID: 432
	// (get) Token: 0x06002D38 RID: 11576 RVA: 0x000C1C4D File Offset: 0x000BFE4D
	// (set) Token: 0x06002D39 RID: 11577 RVA: 0x000C1C54 File Offset: 0x000BFE54
	public static CameraController Instance { get; private set; }

	// Token: 0x06002D3A RID: 11578 RVA: 0x000C1C5C File Offset: 0x000BFE5C
	public static void DestroyInstance()
	{
		CameraController.Instance = null;
	}

	// Token: 0x06002D3B RID: 11579 RVA: 0x000C1C64 File Offset: 0x000BFE64
	public void ToggleColouredOverlayView(bool enabled)
	{
		this.mrt.ToggleColouredOverlayView(enabled);
	}

	// Token: 0x06002D3C RID: 11580 RVA: 0x001FBEEC File Offset: 0x001FA0EC
	protected override void OnPrefabInit()
	{
		global::Util.Reset(base.transform);
		base.transform.SetLocalPosition(new Vector3(Grid.WidthInMeters / 2f, Grid.HeightInMeters / 2f, -100f));
		this.targetOrthographicSize = this.maxOrthographicSize;
		CameraController.Instance = this;
		this.DisableUserCameraControl = false;
		this.baseCamera = this.CopyCamera(Camera.main, "baseCamera");
		this.mrt = this.baseCamera.gameObject.AddComponent<MultipleRenderTarget>();
		this.mrt.onSetupComplete += this.OnMRTSetupComplete;
		this.baseCamera.gameObject.AddComponent<LightBufferCompositor>();
		this.baseCamera.transparencySortMode = TransparencySortMode.Orthographic;
		this.baseCamera.transform.parent = base.transform;
		global::Util.Reset(this.baseCamera.transform);
		int mask = LayerMask.GetMask(new string[]
		{
			"PlaceWithDepth",
			"Overlay"
		});
		int mask2 = LayerMask.GetMask(new string[]
		{
			"Construction"
		});
		this.baseCamera.cullingMask &= ~mask;
		this.baseCamera.cullingMask |= mask2;
		this.baseCamera.tag = "Untagged";
		this.baseCamera.gameObject.AddComponent<CameraRenderTexture>().TextureName = "_LitTex";
		this.infraredCamera = this.CopyCamera(this.baseCamera, "Infrared");
		this.infraredCamera.cullingMask = 0;
		this.infraredCamera.clearFlags = CameraClearFlags.Color;
		this.infraredCamera.depth = this.baseCamera.depth - 1f;
		this.infraredCamera.transform.parent = base.transform;
		this.infraredCamera.gameObject.AddComponent<Infrared>();
		if (SimDebugView.Instance != null)
		{
			this.simOverlayCamera = this.CopyCamera(this.baseCamera, "SimOverlayCamera");
			this.simOverlayCamera.cullingMask = LayerMask.GetMask(new string[]
			{
				"SimDebugView"
			});
			this.simOverlayCamera.clearFlags = CameraClearFlags.Color;
			this.simOverlayCamera.depth = this.baseCamera.depth + 1f;
			this.simOverlayCamera.transform.parent = base.transform;
			this.simOverlayCamera.gameObject.AddComponent<CameraRenderTexture>().TextureName = "_SimDebugViewTex";
		}
		this.overlayCamera = Camera.main;
		this.overlayCamera.name = "Overlay";
		this.overlayCamera.cullingMask = (mask | mask2);
		this.overlayCamera.clearFlags = CameraClearFlags.Nothing;
		this.overlayCamera.transform.parent = base.transform;
		this.overlayCamera.depth = this.baseCamera.depth + 3f;
		this.overlayCamera.transform.SetLocalPosition(Vector3.zero);
		this.overlayCamera.transform.localRotation = Quaternion.identity;
		this.overlayCamera.renderingPath = RenderingPath.Forward;
		this.overlayCamera.allowHDR = false;
		this.overlayCamera.tag = "Untagged";
		this.overlayCamera.gameObject.AddComponent<CameraReferenceTexture>().referenceCamera = this.baseCamera;
		ColorCorrectionLookup component = this.overlayCamera.GetComponent<ColorCorrectionLookup>();
		component.Convert(this.dayColourCube, "");
		component.Convert2(this.nightColourCube, "");
		this.cameras.Add(this.overlayCamera);
		this.lightBufferCamera = this.CopyCamera(this.overlayCamera, "Light Buffer");
		this.lightBufferCamera.clearFlags = CameraClearFlags.Color;
		this.lightBufferCamera.cullingMask = LayerMask.GetMask(new string[]
		{
			"Lights"
		});
		this.lightBufferCamera.depth = this.baseCamera.depth - 1f;
		this.lightBufferCamera.transform.parent = base.transform;
		this.lightBufferCamera.transform.SetLocalPosition(Vector3.zero);
		this.lightBufferCamera.rect = new Rect(0f, 0f, 1f, 1f);
		LightBuffer lightBuffer = this.lightBufferCamera.gameObject.AddComponent<LightBuffer>();
		lightBuffer.Material = this.LightBufferMaterial;
		lightBuffer.CircleMaterial = this.LightCircleOverlay;
		lightBuffer.ConeMaterial = this.LightConeOverlay;
		this.overlayNoDepthCamera = this.CopyCamera(this.overlayCamera, "overlayNoDepth");
		int mask3 = LayerMask.GetMask(new string[]
		{
			"Overlay",
			"Place"
		});
		this.baseCamera.cullingMask &= ~mask3;
		this.overlayNoDepthCamera.clearFlags = CameraClearFlags.Depth;
		this.overlayNoDepthCamera.cullingMask = mask3;
		this.overlayNoDepthCamera.transform.parent = base.transform;
		this.overlayNoDepthCamera.transform.SetLocalPosition(Vector3.zero);
		this.overlayNoDepthCamera.depth = this.baseCamera.depth + 4f;
		this.overlayNoDepthCamera.tag = "MainCamera";
		this.overlayNoDepthCamera.gameObject.AddComponent<NavPathDrawer>();
		this.overlayNoDepthCamera.gameObject.AddComponent<RangeVisualizerEffect>();
		this.overlayNoDepthCamera.gameObject.AddComponent<SkyVisibilityVisualizerEffect>();
		this.overlayNoDepthCamera.gameObject.AddComponent<ScannerNetworkVisualizerEffect>();
		this.overlayNoDepthCamera.gameObject.AddComponent<RocketLaunchConditionVisualizerEffect>();
		this.uiCamera = this.CopyCamera(this.overlayCamera, "uiCamera");
		this.uiCamera.clearFlags = CameraClearFlags.Depth;
		this.uiCamera.cullingMask = LayerMask.GetMask(new string[]
		{
			"UI"
		});
		this.uiCamera.transform.parent = base.transform;
		this.uiCamera.transform.SetLocalPosition(Vector3.zero);
		this.uiCamera.depth = this.baseCamera.depth + 5f;
		if (Game.Instance != null)
		{
			this.timelapseFreezeCamera = this.CopyCamera(this.uiCamera, "timelapseFreezeCamera");
			this.timelapseFreezeCamera.depth = this.uiCamera.depth + 3f;
			this.timelapseFreezeCamera.gameObject.AddComponent<FillRenderTargetEffect>();
			this.timelapseFreezeCamera.enabled = false;
			Camera camera = CameraController.CloneCamera(this.overlayCamera, "timelapseCamera");
			Timelapser timelapser = camera.gameObject.AddComponent<Timelapser>();
			camera.transparencySortMode = TransparencySortMode.Orthographic;
			camera.depth = this.baseCamera.depth + 2f;
			Game.Instance.timelapser = timelapser;
		}
		if (GameScreenManager.Instance != null)
		{
			for (int i = 0; i < this.uiCameraTargets.Count; i++)
			{
				GameScreenManager.Instance.SetCamera(this.uiCameraTargets[i], this.uiCamera);
			}
			this.infoText = GameScreenManager.Instance.screenshotModeCanvas.GetComponentInChildren<LocText>();
		}
		if (!KPlayerPrefs.HasKey("CameraSpeed"))
		{
			CameraController.SetDefaultCameraSpeed();
		}
		this.SetSpeedFromPrefs(null);
		Game.Instance.Subscribe(75424175, new Action<object>(this.SetSpeedFromPrefs));
		this.VisibleArea.Update();
	}

	// Token: 0x06002D3D RID: 11581 RVA: 0x000C1C72 File Offset: 0x000BFE72
	private void SetSpeedFromPrefs(object data = null)
	{
		this.keyPanningSpeed = Mathf.Clamp(0.1f, KPlayerPrefs.GetFloat("CameraSpeed"), 2f);
	}

	// Token: 0x06002D3E RID: 11582 RVA: 0x001FC624 File Offset: 0x001FA824
	public int GetCursorCell()
	{
		Vector3 rhs = Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos());
		Vector3 vector = Vector3.Max(ClusterManager.Instance.activeWorld.minimumBounds, rhs);
		vector = Vector3.Min(ClusterManager.Instance.activeWorld.maximumBounds, vector);
		return Grid.PosToCell(vector);
	}

	// Token: 0x06002D3F RID: 11583 RVA: 0x000C1C93 File Offset: 0x000BFE93
	public static Camera CloneCamera(Camera camera, string name)
	{
		Camera camera2 = new GameObject
		{
			name = name
		}.AddComponent<Camera>();
		camera2.CopyFrom(camera);
		return camera2;
	}

	// Token: 0x06002D40 RID: 11584 RVA: 0x001FC680 File Offset: 0x001FA880
	private Camera CopyCamera(Camera camera, string name)
	{
		Camera camera2 = CameraController.CloneCamera(camera, name);
		this.cameras.Add(camera2);
		return camera2;
	}

	// Token: 0x06002D41 RID: 11585 RVA: 0x000C1CAD File Offset: 0x000BFEAD
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.Restore();
	}

	// Token: 0x06002D42 RID: 11586 RVA: 0x000C1CBB File Offset: 0x000BFEBB
	public static void SetDefaultCameraSpeed()
	{
		KPlayerPrefs.SetFloat("CameraSpeed", 1f);
	}

	// Token: 0x170001B1 RID: 433
	// (get) Token: 0x06002D43 RID: 11587 RVA: 0x000C1CCC File Offset: 0x000BFECC
	// (set) Token: 0x06002D44 RID: 11588 RVA: 0x000C1CD4 File Offset: 0x000BFED4
	public Coroutine activeFadeRoutine { get; private set; }

	// Token: 0x06002D45 RID: 11589 RVA: 0x000C1CDD File Offset: 0x000BFEDD
	public void FadeOut(float targetPercentage = 1f, float speed = 1f, System.Action callback = null)
	{
		if (this.activeFadeRoutine != null)
		{
			base.StopCoroutine(this.activeFadeRoutine);
		}
		this.activeFadeRoutine = base.StartCoroutine(this.FadeWithBlack(true, 0f, targetPercentage, speed, null));
	}

	// Token: 0x06002D46 RID: 11590 RVA: 0x000C1D0E File Offset: 0x000BFF0E
	public void FadeIn(float targetPercentage = 0f, float speed = 1f, System.Action callback = null)
	{
		if (this.activeFadeRoutine != null)
		{
			base.StopCoroutine(this.activeFadeRoutine);
		}
		this.activeFadeRoutine = base.StartCoroutine(this.FadeWithBlack(true, 1f, targetPercentage, speed, callback));
	}

	// Token: 0x06002D47 RID: 11591 RVA: 0x001FC6A4 File Offset: 0x001FA8A4
	public void ActiveWorldStarWipe(int id, System.Action callback = null)
	{
		this.ActiveWorldStarWipe(id, false, default(Vector3), 10f, callback);
	}

	// Token: 0x06002D48 RID: 11592 RVA: 0x000C1D3F File Offset: 0x000BFF3F
	public void ActiveWorldStarWipe(int id, Vector3 position, float forceOrthgraphicSize = 10f, System.Action callback = null)
	{
		this.ActiveWorldStarWipe(id, true, position, forceOrthgraphicSize, callback);
	}

	// Token: 0x06002D49 RID: 11593 RVA: 0x001FC6C8 File Offset: 0x001FA8C8
	private void ActiveWorldStarWipe(int id, bool useForcePosition, Vector3 forcePosition, float forceOrthgraphicSize, System.Action callback)
	{
		if (this.activeFadeRoutine != null)
		{
			base.StopCoroutine(this.activeFadeRoutine);
		}
		if (ClusterManager.Instance.activeWorldId != id)
		{
			if (DetailsScreen.Instance != null)
			{
				DetailsScreen.Instance.DeselectAndClose();
			}
			this.activeFadeRoutine = base.StartCoroutine(this.SwapToWorldFade(id, useForcePosition, forcePosition, forceOrthgraphicSize, callback));
			return;
		}
		ManagementMenu.Instance.CloseAll();
		if (useForcePosition)
		{
			CameraController.Instance.SetTargetPos(forcePosition, 8f, true);
			if (callback != null)
			{
				callback();
			}
		}
	}

	// Token: 0x06002D4A RID: 11594 RVA: 0x000C1D4D File Offset: 0x000BFF4D
	private IEnumerator SwapToWorldFade(int worldId, bool useForcePosition, Vector3 forcePosition, float forceOrthgraphicSize, System.Action newWorldCallback)
	{
		AudioMixer.instance.Start(AudioMixerSnapshots.Get().ActiveBaseChangeSnapshot);
		ClusterManager.Instance.UpdateWorldReverbSnapshot(worldId);
		yield return base.StartCoroutine(this.FadeWithBlack(false, 0f, 1f, 3f, null));
		ClusterManager.Instance.SetActiveWorld(worldId);
		if (useForcePosition)
		{
			CameraController.Instance.SetTargetPos(forcePosition, forceOrthgraphicSize, false);
			CameraController.Instance.SetPosition(forcePosition);
		}
		if (newWorldCallback != null)
		{
			newWorldCallback();
		}
		ManagementMenu.Instance.CloseAll();
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().ActiveBaseChangeSnapshot, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		yield return base.StartCoroutine(this.FadeWithBlack(false, 1f, 0f, 3f, null));
		yield break;
	}

	// Token: 0x06002D4B RID: 11595 RVA: 0x000C1D81 File Offset: 0x000BFF81
	public void SetWorldInteractive(bool state)
	{
		GameScreenManager.Instance.fadePlaneFront.raycastTarget = !state;
	}

	// Token: 0x06002D4C RID: 11596 RVA: 0x000C1D96 File Offset: 0x000BFF96
	private IEnumerator FadeWithBlack(bool fadeUI, float startBlackPercent, float targetBlackPercent, float speed = 1f, System.Action callback = null)
	{
		Image fadePlane = fadeUI ? GameScreenManager.Instance.fadePlaneFront : GameScreenManager.Instance.fadePlaneBack;
		float percent = 0f;
		while (percent < 1f)
		{
			percent += Time.unscaledDeltaTime * speed;
			float a = MathUtil.ReRange(percent, 0f, 1f, startBlackPercent, targetBlackPercent);
			fadePlane.color = new Color(0f, 0f, 0f, a);
			yield return SequenceUtil.WaitForNextFrame;
		}
		fadePlane.color = new Color(0f, 0f, 0f, targetBlackPercent);
		if (callback != null)
		{
			callback();
		}
		this.activeFadeRoutine = null;
		yield return SequenceUtil.WaitForNextFrame;
		yield break;
	}

	// Token: 0x06002D4D RID: 11597 RVA: 0x000C1DCA File Offset: 0x000BFFCA
	public void EnableFreeCamera(bool enable)
	{
		this.FreeCameraEnabled = enable;
		this.SetInfoText("Screenshot Mode (ESC to exit)");
	}

	// Token: 0x06002D4E RID: 11598 RVA: 0x001FC750 File Offset: 0x001FA950
	private static bool WithinInputField()
	{
		UnityEngine.EventSystems.EventSystem current = UnityEngine.EventSystems.EventSystem.current;
		if (current == null)
		{
			return false;
		}
		bool result = false;
		if (current.currentSelectedGameObject != null && (current.currentSelectedGameObject.GetComponent<KInputTextField>() != null || current.currentSelectedGameObject.GetComponent<InputField>() != null))
		{
			result = true;
		}
		return result;
	}

	// Token: 0x170001B2 RID: 434
	// (get) Token: 0x06002D4F RID: 11599 RVA: 0x001FC7A8 File Offset: 0x001FA9A8
	public static bool IsMouseOverGameWindow
	{
		get
		{
			return 0f <= Input.mousePosition.x && 0f <= Input.mousePosition.y && (float)Screen.width >= Input.mousePosition.x && (float)Screen.height >= Input.mousePosition.y;
		}
	}

	// Token: 0x06002D50 RID: 11600 RVA: 0x001FC800 File Offset: 0x001FAA00
	private void SetInfoText(string text)
	{
		this.infoText.text = text;
		Color color = this.infoText.color;
		color.a = 0.5f;
		this.infoText.color = color;
	}

	// Token: 0x06002D51 RID: 11601 RVA: 0x001FC840 File Offset: 0x001FAA40
	public void OnKeyDown(KButtonEvent e)
	{
		if (e.Consumed)
		{
			return;
		}
		if (this.DisableUserCameraControl)
		{
			return;
		}
		if (CameraController.WithinInputField())
		{
			return;
		}
		if (SaveGame.Instance != null && SaveGame.Instance.GetComponent<UserNavigation>().Handle(e))
		{
			return;
		}
		if (!this.ChangeWorldInput(e))
		{
			if (e.TryConsume(global::Action.TogglePause))
			{
				SpeedControlScreen.Instance.TogglePause(false);
			}
			else if (e.TryConsume(global::Action.ZoomIn) && CameraController.IsMouseOverGameWindow)
			{
				float a = this.targetOrthographicSize * (1f / this.zoomFactor);
				this.targetOrthographicSize = Mathf.Max(a, this.minOrthographicSize);
				this.overrideZoomSpeed = 0f;
				this.isTargetPosSet = false;
			}
			else if (e.TryConsume(global::Action.ZoomOut) && CameraController.IsMouseOverGameWindow)
			{
				float a2 = this.targetOrthographicSize * this.zoomFactor;
				this.targetOrthographicSize = Mathf.Min(a2, this.FreeCameraEnabled ? TuningData<CameraController.Tuning>.Get().maxOrthographicSizeDebug : this.maxOrthographicSize);
				this.overrideZoomSpeed = 0f;
				this.isTargetPosSet = false;
			}
			else if (e.TryConsume(global::Action.MouseMiddle) || e.IsAction(global::Action.MouseRight))
			{
				this.panning = true;
				this.overrideZoomSpeed = 0f;
				this.isTargetPosSet = false;
			}
			else if (this.FreeCameraEnabled && e.TryConsume(global::Action.CinemaCamEnable))
			{
				this.cinemaCamEnabled = !this.cinemaCamEnabled;
				DebugUtil.LogArgs(new object[]
				{
					"Cinema Cam Enabled ",
					this.cinemaCamEnabled
				});
				this.SetInfoText(this.cinemaCamEnabled ? "Cinema Cam Enabled" : "Cinema Cam Disabled");
			}
			else if (this.FreeCameraEnabled && this.cinemaCamEnabled)
			{
				if (e.TryConsume(global::Action.CinemaToggleLock))
				{
					this.cinemaToggleLock = !this.cinemaToggleLock;
					DebugUtil.LogArgs(new object[]
					{
						"Cinema Toggle Lock ",
						this.cinemaToggleLock
					});
					this.SetInfoText(this.cinemaToggleLock ? "Cinema Input Lock ON" : "Cinema Input Lock OFF");
				}
				else if (e.TryConsume(global::Action.CinemaToggleEasing))
				{
					this.cinemaToggleEasing = !this.cinemaToggleEasing;
					DebugUtil.LogArgs(new object[]
					{
						"Cinema Toggle Easing ",
						this.cinemaToggleEasing
					});
					this.SetInfoText(this.cinemaToggleEasing ? "Cinema Easing ON" : "Cinema Easing OFF");
				}
				else if (e.TryConsume(global::Action.CinemaUnpauseOnMove))
				{
					this.cinemaUnpauseNextMove = !this.cinemaUnpauseNextMove;
					DebugUtil.LogArgs(new object[]
					{
						"Cinema Unpause Next Move ",
						this.cinemaUnpauseNextMove
					});
					this.SetInfoText(this.cinemaUnpauseNextMove ? "Cinema Unpause Next Move ON" : "Cinema Unpause Next Move OFF");
				}
				else if (e.TryConsume(global::Action.CinemaPanLeft))
				{
					this.cinemaPanLeft = (!this.cinemaToggleLock || !this.cinemaPanLeft);
					this.cinemaPanRight = false;
					this.CheckMoveUnpause();
				}
				else if (e.TryConsume(global::Action.CinemaPanRight))
				{
					this.cinemaPanRight = (!this.cinemaToggleLock || !this.cinemaPanRight);
					this.cinemaPanLeft = false;
					this.CheckMoveUnpause();
				}
				else if (e.TryConsume(global::Action.CinemaPanUp))
				{
					this.cinemaPanUp = (!this.cinemaToggleLock || !this.cinemaPanUp);
					this.cinemaPanDown = false;
					this.CheckMoveUnpause();
				}
				else if (e.TryConsume(global::Action.CinemaPanDown))
				{
					this.cinemaPanDown = (!this.cinemaToggleLock || !this.cinemaPanDown);
					this.cinemaPanUp = false;
					this.CheckMoveUnpause();
				}
				else if (e.TryConsume(global::Action.CinemaZoomIn))
				{
					this.cinemaZoomIn = (!this.cinemaToggleLock || !this.cinemaZoomIn);
					this.cinemaZoomOut = false;
					this.CheckMoveUnpause();
				}
				else if (e.TryConsume(global::Action.CinemaZoomOut))
				{
					this.cinemaZoomOut = (!this.cinemaToggleLock || !this.cinemaZoomOut);
					this.cinemaZoomIn = false;
					this.CheckMoveUnpause();
				}
				else if (e.TryConsume(global::Action.CinemaZoomSpeedPlus))
				{
					this.cinemaZoomSpeed++;
					DebugUtil.LogArgs(new object[]
					{
						"Cinema Zoom Speed ",
						this.cinemaZoomSpeed
					});
					this.SetInfoText("Cinema Zoom Speed: " + this.cinemaZoomSpeed.ToString());
				}
				else if (e.TryConsume(global::Action.CinemaZoomSpeedMinus))
				{
					this.cinemaZoomSpeed--;
					DebugUtil.LogArgs(new object[]
					{
						"Cinema Zoom Speed ",
						this.cinemaZoomSpeed
					});
					this.SetInfoText("Cinema Zoom Speed: " + this.cinemaZoomSpeed.ToString());
				}
			}
			else if (e.TryConsume(global::Action.PanLeft))
			{
				this.panLeft = true;
			}
			else if (e.TryConsume(global::Action.PanRight))
			{
				this.panRight = true;
			}
			else if (e.TryConsume(global::Action.PanUp))
			{
				this.panUp = true;
			}
			else if (e.TryConsume(global::Action.PanDown))
			{
				this.panDown = true;
			}
		}
		if (!e.Consumed && OverlayMenu.Instance != null)
		{
			OverlayMenu.Instance.OnKeyDown(e);
		}
	}

	// Token: 0x06002D52 RID: 11602 RVA: 0x001FCD98 File Offset: 0x001FAF98
	public bool ChangeWorldInput(KButtonEvent e)
	{
		if (e.Consumed)
		{
			return true;
		}
		int num = -1;
		if (e.TryConsume(global::Action.SwitchActiveWorld1))
		{
			num = 0;
		}
		else if (e.TryConsume(global::Action.SwitchActiveWorld2))
		{
			num = 1;
		}
		else if (e.TryConsume(global::Action.SwitchActiveWorld3))
		{
			num = 2;
		}
		else if (e.TryConsume(global::Action.SwitchActiveWorld4))
		{
			num = 3;
		}
		else if (e.TryConsume(global::Action.SwitchActiveWorld5))
		{
			num = 4;
		}
		else if (e.TryConsume(global::Action.SwitchActiveWorld6))
		{
			num = 5;
		}
		else if (e.TryConsume(global::Action.SwitchActiveWorld7))
		{
			num = 6;
		}
		else if (e.TryConsume(global::Action.SwitchActiveWorld8))
		{
			num = 7;
		}
		else if (e.TryConsume(global::Action.SwitchActiveWorld9))
		{
			num = 8;
		}
		else if (e.TryConsume(global::Action.SwitchActiveWorld10))
		{
			num = 9;
		}
		if (num != -1)
		{
			List<int> discoveredAsteroidIDsSorted = ClusterManager.Instance.GetDiscoveredAsteroidIDsSorted();
			if (num < discoveredAsteroidIDsSorted.Count && num >= 0)
			{
				num = discoveredAsteroidIDsSorted[num];
				WorldContainer world = ClusterManager.Instance.GetWorld(num);
				if (world != null && world.IsDiscovered && ClusterManager.Instance.activeWorldId != world.id)
				{
					ManagementMenu.Instance.CloseClusterMap();
					this.ActiveWorldStarWipe(world.id, null);
				}
			}
			return true;
		}
		return false;
	}

	// Token: 0x06002D53 RID: 11603 RVA: 0x001FCED0 File Offset: 0x001FB0D0
	public void OnKeyUp(KButtonEvent e)
	{
		if (this.DisableUserCameraControl)
		{
			return;
		}
		if (CameraController.WithinInputField())
		{
			return;
		}
		if (e.TryConsume(global::Action.MouseMiddle) || e.IsAction(global::Action.MouseRight))
		{
			this.panning = false;
			return;
		}
		if (this.FreeCameraEnabled && this.cinemaCamEnabled)
		{
			if (e.TryConsume(global::Action.CinemaPanLeft))
			{
				this.cinemaPanLeft = (this.cinemaToggleLock && this.cinemaPanLeft);
				return;
			}
			if (e.TryConsume(global::Action.CinemaPanRight))
			{
				this.cinemaPanRight = (this.cinemaToggleLock && this.cinemaPanRight);
				return;
			}
			if (e.TryConsume(global::Action.CinemaPanUp))
			{
				this.cinemaPanUp = (this.cinemaToggleLock && this.cinemaPanUp);
				return;
			}
			if (e.TryConsume(global::Action.CinemaPanDown))
			{
				this.cinemaPanDown = (this.cinemaToggleLock && this.cinemaPanDown);
				return;
			}
			if (e.TryConsume(global::Action.CinemaZoomIn))
			{
				this.cinemaZoomIn = (this.cinemaToggleLock && this.cinemaZoomIn);
				return;
			}
			if (e.TryConsume(global::Action.CinemaZoomOut))
			{
				this.cinemaZoomOut = (this.cinemaToggleLock && this.cinemaZoomOut);
				return;
			}
		}
		else
		{
			if (e.TryConsume(global::Action.CameraHome))
			{
				this.CameraGoHome(2f, true);
				return;
			}
			if (e.TryConsume(global::Action.PanLeft))
			{
				this.panLeft = false;
				return;
			}
			if (e.TryConsume(global::Action.PanRight))
			{
				this.panRight = false;
				return;
			}
			if (e.TryConsume(global::Action.PanUp))
			{
				this.panUp = false;
				return;
			}
			if (e.TryConsume(global::Action.PanDown))
			{
				this.panDown = false;
			}
		}
	}

	// Token: 0x06002D54 RID: 11604 RVA: 0x000C1DDE File Offset: 0x000BFFDE
	public void ForcePanningState(bool state)
	{
		this.panning = false;
	}

	// Token: 0x06002D55 RID: 11605 RVA: 0x001FD06C File Offset: 0x001FB26C
	public void CameraGoHome(float speed = 2f, bool showCameraReturnButton = false)
	{
		GameObject activeTelepad = GameUtil.GetActiveTelepad();
		if (activeTelepad != null && ClusterUtil.ActiveWorldHasPrinter())
		{
			GameUtil.FocusCamera(new Vector3(activeTelepad.transform.GetPosition().x, activeTelepad.transform.GetPosition().y + 1f, base.transform.GetPosition().z), speed, true, showCameraReturnButton);
			this.SetOverrideZoomSpeed(speed);
		}
	}

	// Token: 0x06002D56 RID: 11606 RVA: 0x000C1DE7 File Offset: 0x000BFFE7
	public void CameraGoTo(Vector3 pos, float speed = 2f, bool playSound = true)
	{
		pos.z = base.transform.GetPosition().z;
		this.SetTargetPos(pos, 10f, playSound);
		this.SetOverrideZoomSpeed(speed);
	}

	// Token: 0x06002D57 RID: 11607 RVA: 0x001FD0DC File Offset: 0x001FB2DC
	public void SnapTo(Vector3 pos)
	{
		this.ClearFollowTarget();
		pos.z = -100f;
		this.targetPos = Vector3.zero;
		this.isTargetPosSet = false;
		base.transform.SetPosition(pos);
		this.keyPanDelta = Vector3.zero;
		this.OrthographicSize = this.targetOrthographicSize;
	}

	// Token: 0x06002D58 RID: 11608 RVA: 0x000C1E14 File Offset: 0x000C0014
	public void SnapTo(Vector3 pos, float orthographicSize)
	{
		this.targetOrthographicSize = orthographicSize;
		this.SnapTo(pos);
	}

	// Token: 0x06002D59 RID: 11609 RVA: 0x000C1E24 File Offset: 0x000C0024
	public void SetOverrideZoomSpeed(float tempZoomSpeed)
	{
		this.overrideZoomSpeed = tempZoomSpeed;
	}

	// Token: 0x06002D5A RID: 11610 RVA: 0x001FD134 File Offset: 0x001FB334
	public void SetTargetPos(Vector3 pos, float orthographic_size, bool playSound)
	{
		int num = Grid.PosToCell(pos);
		if (!Grid.IsValidCell(num) || Grid.WorldIdx[num] == 255 || ClusterManager.Instance.GetWorld((int)Grid.WorldIdx[num]) == null)
		{
			return;
		}
		this.ClearFollowTarget();
		if (playSound && !this.isTargetPosSet)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Click_Notification", false));
		}
		pos.z = -100f;
		if ((int)Grid.WorldIdx[num] != ClusterManager.Instance.activeWorldId)
		{
			this.targetOrthographicSize = 20f;
			this.ActiveWorldStarWipe((int)Grid.WorldIdx[num], pos, 10f, delegate()
			{
				this.targetPos = pos;
				this.isTargetPosSet = true;
				this.OrthographicSize = orthographic_size + 5f;
				this.targetOrthographicSize = orthographic_size;
			});
		}
		else
		{
			this.targetPos = pos;
			this.isTargetPosSet = true;
			this.targetOrthographicSize = orthographic_size;
		}
		PlayerController.Instance.CancelDragging();
		this.CheckMoveUnpause();
	}

	// Token: 0x06002D5B RID: 11611 RVA: 0x001FD23C File Offset: 0x001FB43C
	public void SetTargetPosForWorldChange(Vector3 pos, float orthographic_size, bool playSound)
	{
		int num = Grid.PosToCell(pos);
		if (!Grid.IsValidCell(num) || Grid.WorldIdx[num] == 255 || ClusterManager.Instance.GetWorld((int)Grid.WorldIdx[num]) == null)
		{
			return;
		}
		this.ClearFollowTarget();
		if (playSound && !this.isTargetPosSet)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Click_Notification", false));
		}
		pos.z = -100f;
		this.targetPos = pos;
		this.isTargetPosSet = true;
		this.targetOrthographicSize = orthographic_size;
		PlayerController.Instance.CancelDragging();
		this.CheckMoveUnpause();
		this.SetPosition(pos);
		this.OrthographicSize = orthographic_size;
	}

	// Token: 0x06002D5C RID: 11612 RVA: 0x000C1E2D File Offset: 0x000C002D
	public void SetMaxOrthographicSize(float size)
	{
		this.maxOrthographicSize = size;
	}

	// Token: 0x06002D5D RID: 11613 RVA: 0x000C1E36 File Offset: 0x000C0036
	public void SetPosition(Vector3 pos)
	{
		base.transform.SetPosition(pos);
	}

	// Token: 0x06002D5E RID: 11614 RVA: 0x000C1E45 File Offset: 0x000C0045
	public IEnumerator DoCinematicZoom(float targetOrthographicSize)
	{
		this.cinemaCamEnabled = true;
		this.FreeCameraEnabled = true;
		this.targetOrthographicSize = targetOrthographicSize;
		while (targetOrthographicSize - this.OrthographicSize >= 0.001f)
		{
			yield return SequenceUtil.WaitForEndOfFrame;
		}
		this.OrthographicSize = targetOrthographicSize;
		this.FreeCameraEnabled = false;
		this.cinemaCamEnabled = false;
		yield break;
	}

	// Token: 0x06002D5F RID: 11615 RVA: 0x001FD2E0 File Offset: 0x001FB4E0
	private Vector3 PointUnderCursor(Vector3 mousePos, Camera cam)
	{
		Ray ray = cam.ScreenPointToRay(mousePos);
		Vector3 direction = ray.direction;
		Vector3 b = direction * Mathf.Abs(cam.transform.GetPosition().z / direction.z);
		return ray.origin + b;
	}

	// Token: 0x06002D60 RID: 11616 RVA: 0x001FD330 File Offset: 0x001FB530
	private void CinemaCamUpdate()
	{
		float unscaledDeltaTime = Time.unscaledDeltaTime;
		Camera main = Camera.main;
		Vector3 localPosition = base.transform.GetLocalPosition();
		float num = Mathf.Pow((float)this.cinemaZoomSpeed, 3f);
		if (this.cinemaZoomIn)
		{
			this.overrideZoomSpeed = -num / TuningData<CameraController.Tuning>.Get().cinemaZoomFactor;
			this.isTargetPosSet = false;
		}
		else if (this.cinemaZoomOut)
		{
			this.overrideZoomSpeed = num / TuningData<CameraController.Tuning>.Get().cinemaZoomFactor;
			this.isTargetPosSet = false;
		}
		else
		{
			this.overrideZoomSpeed = 0f;
		}
		if (this.cinemaToggleEasing)
		{
			this.cinemaZoomVelocity += (this.overrideZoomSpeed - this.cinemaZoomVelocity) * this.cinemaEasing;
		}
		else
		{
			this.cinemaZoomVelocity = this.overrideZoomSpeed;
		}
		if (this.cinemaZoomVelocity != 0f)
		{
			this.OrthographicSize = main.orthographicSize + this.cinemaZoomVelocity * unscaledDeltaTime * (main.orthographicSize / 20f);
			this.targetOrthographicSize = main.orthographicSize;
		}
		float num2 = num / TuningData<CameraController.Tuning>.Get().cinemaZoomToFactor;
		float num3 = this.keyPanningSpeed / 20f * main.orthographicSize;
		float num4 = num3 * (num / TuningData<CameraController.Tuning>.Get().cinemaPanToFactor);
		if (!this.isTargetPosSet && this.targetOrthographicSize != main.orthographicSize)
		{
			float t = Mathf.Min(num2 * unscaledDeltaTime, 0.1f);
			this.OrthographicSize = Mathf.Lerp(main.orthographicSize, this.targetOrthographicSize, t);
		}
		Vector3 b = Vector3.zero;
		if (this.isTargetPosSet)
		{
			float num5 = this.cinemaEasing * TuningData<CameraController.Tuning>.Get().targetZoomEasingFactor;
			float num6 = this.cinemaEasing * TuningData<CameraController.Tuning>.Get().targetPanEasingFactor;
			float num7 = this.targetOrthographicSize - main.orthographicSize;
			Vector3 vector = this.targetPos - localPosition;
			float num8;
			float num9;
			if (!this.cinemaToggleEasing)
			{
				num8 = num2 * unscaledDeltaTime;
				num9 = num4 * unscaledDeltaTime;
			}
			else
			{
				DebugUtil.LogArgs(new object[]
				{
					"Min zoom of:",
					num2 * unscaledDeltaTime,
					Mathf.Abs(num7) * num5 * unscaledDeltaTime
				});
				num8 = Mathf.Min(num2 * unscaledDeltaTime, Mathf.Abs(num7) * num5 * unscaledDeltaTime);
				DebugUtil.LogArgs(new object[]
				{
					"Min pan of:",
					num4 * unscaledDeltaTime,
					vector.magnitude * num6 * unscaledDeltaTime
				});
				num9 = Mathf.Min(num4 * unscaledDeltaTime, vector.magnitude * num6 * unscaledDeltaTime);
			}
			float num10;
			if (Mathf.Abs(num7) < num8)
			{
				num10 = num7;
			}
			else
			{
				num10 = Mathf.Sign(num7) * num8;
			}
			if (vector.magnitude < num9)
			{
				b = vector;
			}
			else
			{
				b = vector.normalized * num9;
			}
			if (Mathf.Abs(num10) < 0.001f && b.magnitude < 0.001f)
			{
				this.isTargetPosSet = false;
				num10 = num7;
				b = vector;
			}
			this.OrthographicSize = main.orthographicSize + num10 * (main.orthographicSize / 20f);
		}
		if (!PlayerController.Instance.CanDrag())
		{
			this.panning = false;
		}
		Vector3 b2 = Vector3.zero;
		if (this.panning)
		{
			b2 = -PlayerController.Instance.GetWorldDragDelta();
			this.isTargetPosSet = false;
			if (b2.magnitude > 0f)
			{
				this.ClearFollowTarget();
			}
			this.keyPanDelta = Vector3.zero;
		}
		else
		{
			float num11 = num / TuningData<CameraController.Tuning>.Get().cinemaPanFactor;
			Vector3 zero = Vector3.zero;
			if (this.cinemaPanLeft)
			{
				this.ClearFollowTarget();
				zero.x = -num3 * num11;
				this.isTargetPosSet = false;
			}
			if (this.cinemaPanRight)
			{
				this.ClearFollowTarget();
				zero.x = num3 * num11;
				this.isTargetPosSet = false;
			}
			if (this.cinemaPanUp)
			{
				this.ClearFollowTarget();
				zero.y = num3 * num11;
				this.isTargetPosSet = false;
			}
			if (this.cinemaPanDown)
			{
				this.ClearFollowTarget();
				zero.y = -num3 * num11;
				this.isTargetPosSet = false;
			}
			if (this.cinemaToggleEasing)
			{
				this.keyPanDelta += (zero - this.keyPanDelta) * this.cinemaEasing;
			}
			else
			{
				this.keyPanDelta = zero;
			}
		}
		Vector3 vector2 = localPosition + b + b2 + this.keyPanDelta * unscaledDeltaTime;
		if (this.followTarget != null)
		{
			vector2.x = this.followTargetPos.x;
			vector2.y = this.followTargetPos.y;
		}
		vector2.z = -100f;
		if ((double)(vector2 - base.transform.GetLocalPosition()).magnitude > 0.001)
		{
			base.transform.SetLocalPosition(vector2);
		}
	}

	// Token: 0x06002D61 RID: 11617 RVA: 0x001FD7F8 File Offset: 0x001FB9F8
	private void NormalCamUpdate()
	{
		float unscaledDeltaTime = Time.unscaledDeltaTime;
		Camera main = Camera.main;
		this.smoothDt = this.smoothDt * 2f / 3f + unscaledDeltaTime / 3f;
		float num = (this.overrideZoomSpeed != 0f) ? this.overrideZoomSpeed : this.zoomSpeed;
		Vector3 localPosition = base.transform.GetLocalPosition();
		Vector3 vector = (this.overrideZoomSpeed != 0f) ? new Vector3((float)Screen.width / 2f, (float)Screen.height / 2f, 0f) : KInputManager.GetMousePos();
		Vector3 position = this.PointUnderCursor(vector, main);
		Vector3 position2 = main.ScreenToViewportPoint(vector);
		float num2 = this.keyPanningSpeed / 20f * main.orthographicSize;
		num2 *= Mathf.Min(unscaledDeltaTime / 0.016666666f, 10f);
		float t = num * Mathf.Min(this.smoothDt, 0.3f);
		this.OrthographicSize = Mathf.Lerp(main.orthographicSize, this.targetOrthographicSize, t);
		base.transform.SetLocalPosition(localPosition);
		Vector3 vector2 = main.WorldToViewportPoint(position);
		position2.z = vector2.z;
		Vector3 b = main.ViewportToWorldPoint(vector2) - main.ViewportToWorldPoint(position2);
		if (this.isTargetPosSet)
		{
			b = Vector3.Lerp(localPosition, this.targetPos, num * this.smoothDt) - localPosition;
			if (b.magnitude < 0.001f)
			{
				this.isTargetPosSet = false;
				b = this.targetPos - localPosition;
			}
		}
		if (!PlayerController.Instance.CanDrag())
		{
			this.panning = false;
		}
		Vector3 b2 = Vector3.zero;
		if (this.panning)
		{
			b2 = -PlayerController.Instance.GetWorldDragDelta();
			this.isTargetPosSet = false;
		}
		Vector3 vector3 = localPosition + b + b2;
		if (this.panning)
		{
			if (b2.magnitude > 0f)
			{
				this.ClearFollowTarget();
			}
			this.keyPanDelta = Vector3.zero;
		}
		else if (!this.DisableUserCameraControl)
		{
			if (this.panLeft)
			{
				this.ClearFollowTarget();
				this.keyPanDelta.x = this.keyPanDelta.x - num2;
				this.isTargetPosSet = false;
				this.overrideZoomSpeed = 0f;
			}
			if (this.panRight)
			{
				this.ClearFollowTarget();
				this.keyPanDelta.x = this.keyPanDelta.x + num2;
				this.isTargetPosSet = false;
				this.overrideZoomSpeed = 0f;
			}
			if (this.panUp)
			{
				this.ClearFollowTarget();
				this.keyPanDelta.y = this.keyPanDelta.y + num2;
				this.isTargetPosSet = false;
				this.overrideZoomSpeed = 0f;
			}
			if (this.panDown)
			{
				this.ClearFollowTarget();
				this.keyPanDelta.y = this.keyPanDelta.y - num2;
				this.isTargetPosSet = false;
				this.overrideZoomSpeed = 0f;
			}
			if (KInputManager.currentControllerIsGamepad)
			{
				Vector2 vector4 = num2 * KInputManager.steamInputInterpreter.GetSteamCameraMovement();
				if (Mathf.Abs(vector4.x) > Mathf.Epsilon || Mathf.Abs(vector4.y) > Mathf.Epsilon)
				{
					this.ClearFollowTarget();
					this.isTargetPosSet = false;
					this.overrideZoomSpeed = 0f;
				}
				this.keyPanDelta += new Vector3(vector4.x, vector4.y, 0f);
			}
			Vector3 vector5 = new Vector3(Mathf.Lerp(0f, this.keyPanDelta.x, this.smoothDt * this.keyPanningEasing), Mathf.Lerp(0f, this.keyPanDelta.y, this.smoothDt * this.keyPanningEasing), 0f);
			this.keyPanDelta -= vector5;
			vector3.x += vector5.x;
			vector3.y += vector5.y;
		}
		if (this.followTarget != null)
		{
			vector3.x = this.followTargetPos.x;
			vector3.y = this.followTargetPos.y;
		}
		vector3.z = -100f;
		if ((double)(vector3 - base.transform.GetLocalPosition()).magnitude > 0.001)
		{
			base.transform.SetLocalPosition(vector3);
		}
	}

	// Token: 0x06002D62 RID: 11618 RVA: 0x001FDC44 File Offset: 0x001FBE44
	private void Update()
	{
		if (Game.Instance == null || !Game.Instance.timelapser.CapturingTimelapseScreenshot)
		{
			if (this.FreeCameraEnabled && this.cinemaCamEnabled)
			{
				this.CinemaCamUpdate();
			}
			else
			{
				this.NormalCamUpdate();
			}
		}
		if (this.infoText != null && this.infoText.color.a > 0f)
		{
			Color color = this.infoText.color;
			color.a = Mathf.Max(0f, this.infoText.color.a - Time.unscaledDeltaTime * 0.5f);
			this.infoText.color = color;
		}
		this.ConstrainToWorld();
		Vector3 vector = this.PointUnderCursor(KInputManager.GetMousePos(), Camera.main);
		Shader.SetGlobalVector("_WorldCameraPos", new Vector4(base.transform.GetPosition().x, base.transform.GetPosition().y, base.transform.GetPosition().z, Camera.main.orthographicSize));
		Shader.SetGlobalVector("_WorldCursorPos", new Vector4(vector.x, vector.y, 0f, 0f));
		this.VisibleArea.Update();
		this.soundCuller = SoundCuller.CreateCuller();
	}

	// Token: 0x06002D63 RID: 11619 RVA: 0x001FDD94 File Offset: 0x001FBF94
	private Vector3 GetFollowPos()
	{
		if (this.followTarget != null)
		{
			Vector3 result = this.followTarget.transform.GetPosition();
			KAnimControllerBase component = this.followTarget.GetComponent<KAnimControllerBase>();
			if (component != null)
			{
				result = component.GetWorldPivot();
			}
			return result;
		}
		return Vector3.zero;
	}

	// Token: 0x06002D64 RID: 11620 RVA: 0x001FDDE4 File Offset: 0x001FBFE4
	public static float GetHighestVisibleCell_Height(byte worldID = 255)
	{
		Vector2 zero = Vector2.zero;
		Vector2 vector = new Vector2(Grid.WidthInMeters, Grid.HeightInMeters);
		Camera main = Camera.main;
		float orthographicSize = main.orthographicSize;
		main.orthographicSize = 20f;
		Ray ray = main.ViewportPointToRay(Vector3.one - Vector3.one * 0.33f);
		Vector3 vector2 = CameraController.Instance.transform.GetPosition() - ray.origin;
		main.orthographicSize = orthographicSize;
		if (ClusterManager.Instance != null)
		{
			WorldContainer worldContainer = (worldID == byte.MaxValue) ? ClusterManager.Instance.activeWorld : ClusterManager.Instance.GetWorld((int)worldID);
			worldContainer.minimumBounds * Grid.CellSizeInMeters;
			vector = worldContainer.maximumBounds * Grid.CellSizeInMeters;
			new Vector2((float)worldContainer.Width, (float)worldContainer.Height) * Grid.CellSizeInMeters;
		}
		return vector.y * 1.1f + 20f + vector2.y;
	}

	// Token: 0x06002D65 RID: 11621 RVA: 0x001FDEEC File Offset: 0x001FC0EC
	private void ConstrainToWorld()
	{
		if (Game.Instance != null && Game.Instance.IsLoading())
		{
			return;
		}
		if (this.FreeCameraEnabled)
		{
			return;
		}
		Camera main = Camera.main;
		Ray ray = main.ViewportPointToRay(Vector3.zero + Vector3.one * 0.33f);
		Ray ray2 = main.ViewportPointToRay(Vector3.one - Vector3.one * 0.33f);
		float distance = Mathf.Abs(ray.origin.z / ray.direction.z);
		float distance2 = Mathf.Abs(ray2.origin.z / ray2.direction.z);
		Vector3 point = ray.GetPoint(distance);
		Vector3 point2 = ray2.GetPoint(distance2);
		Vector2 vector = Vector2.zero;
		Vector2 vector2 = new Vector2(Grid.WidthInMeters, Grid.HeightInMeters);
		Vector2 vector3 = vector2;
		if (ClusterManager.Instance != null)
		{
			WorldContainer activeWorld = ClusterManager.Instance.activeWorld;
			vector = activeWorld.minimumBounds * Grid.CellSizeInMeters;
			vector2 = activeWorld.maximumBounds * Grid.CellSizeInMeters;
			vector3 = new Vector2((float)activeWorld.Width, (float)activeWorld.Height) * Grid.CellSizeInMeters;
		}
		if (point2.x - point.x > vector3.x || point2.y - point.y > vector3.y)
		{
			return;
		}
		Vector3 b = base.transform.GetPosition() - ray.origin;
		Vector3 vector4 = point;
		vector4.x = Mathf.Max(vector.x, vector4.x);
		vector4.y = Mathf.Max(vector.y * Grid.CellSizeInMeters, vector4.y);
		ray.origin = vector4;
		ray.direction = -ray.direction;
		vector4 = ray.GetPoint(distance);
		base.transform.SetPosition(vector4 + b);
		b = base.transform.GetPosition() - ray2.origin;
		vector4 = point2;
		vector4.x = Mathf.Min(vector2.x, vector4.x);
		vector4.y = Mathf.Min(vector2.y * 1.1f, vector4.y);
		ray2.origin = vector4;
		ray2.direction = -ray2.direction;
		vector4 = ray2.GetPoint(distance2);
		Vector3 position = vector4 + b;
		position.z = -100f;
		base.transform.SetPosition(position);
	}

	// Token: 0x06002D66 RID: 11622 RVA: 0x001FE18C File Offset: 0x001FC38C
	public void Save(BinaryWriter writer)
	{
		writer.Write(base.transform.GetPosition());
		writer.Write(base.transform.localScale);
		writer.Write(base.transform.rotation);
		writer.Write(this.targetOrthographicSize);
		CameraSaveData.position = base.transform.GetPosition();
		CameraSaveData.localScale = base.transform.localScale;
		CameraSaveData.rotation = base.transform.rotation;
	}

	// Token: 0x06002D67 RID: 11623 RVA: 0x001FE208 File Offset: 0x001FC408
	private void Restore()
	{
		if (CameraSaveData.valid)
		{
			int cell = Grid.PosToCell(CameraSaveData.position);
			if (Grid.IsValidCell(cell) && !Grid.IsVisible(cell))
			{
				global::Debug.LogWarning("Resetting Camera Position... camera was saved in an undiscovered area of the map.");
				this.CameraGoHome(2f, false);
				return;
			}
			base.transform.SetPosition(CameraSaveData.position);
			base.transform.localScale = CameraSaveData.localScale;
			base.transform.rotation = CameraSaveData.rotation;
			this.targetOrthographicSize = Mathf.Clamp(CameraSaveData.orthographicsSize, this.minOrthographicSize, this.FreeCameraEnabled ? TuningData<CameraController.Tuning>.Get().maxOrthographicSizeDebug : this.maxOrthographicSize);
			this.SnapTo(base.transform.GetPosition());
		}
	}

	// Token: 0x06002D68 RID: 11624 RVA: 0x000C1E5B File Offset: 0x000C005B
	private void OnMRTSetupComplete(Camera cam)
	{
		this.cameras.Add(cam);
	}

	// Token: 0x06002D69 RID: 11625 RVA: 0x000C1E69 File Offset: 0x000C0069
	public bool IsAudibleSound(Vector2 pos)
	{
		return this.soundCuller.IsAudible(pos);
	}

	// Token: 0x06002D6A RID: 11626 RVA: 0x001FE2C4 File Offset: 0x001FC4C4
	public bool IsAudibleSound(Vector3 pos, EventReference event_ref)
	{
		string eventReferencePath = KFMOD.GetEventReferencePath(event_ref);
		return this.soundCuller.IsAudible(pos, eventReferencePath);
	}

	// Token: 0x06002D6B RID: 11627 RVA: 0x000C1E77 File Offset: 0x000C0077
	public bool IsAudibleSound(Vector3 pos, HashedString sound_path)
	{
		return this.soundCuller.IsAudible(pos, sound_path);
	}

	// Token: 0x06002D6C RID: 11628 RVA: 0x000C1E8B File Offset: 0x000C008B
	public Vector3 GetVerticallyScaledPosition(Vector3 pos, bool objectIsSelectedAndVisible = false)
	{
		return this.soundCuller.GetVerticallyScaledPosition(pos, objectIsSelectedAndVisible);
	}

	// Token: 0x06002D6D RID: 11629 RVA: 0x001FE2F0 File Offset: 0x001FC4F0
	public bool IsVisiblePos(Vector3 pos)
	{
		return this.VisibleArea.CurrentArea.Contains(pos);
	}

	// Token: 0x06002D6E RID: 11630 RVA: 0x001FE314 File Offset: 0x001FC514
	public bool IsVisiblePosExtended(Vector3 pos)
	{
		return this.VisibleArea.CurrentAreaExtended.Contains(pos);
	}

	// Token: 0x06002D6F RID: 11631 RVA: 0x000C1C5C File Offset: 0x000BFE5C
	protected override void OnCleanUp()
	{
		CameraController.Instance = null;
	}

	// Token: 0x06002D70 RID: 11632 RVA: 0x001FE338 File Offset: 0x001FC538
	public void SetFollowTarget(Transform follow_target)
	{
		this.ClearFollowTarget();
		if (follow_target == null)
		{
			return;
		}
		this.followTarget = follow_target;
		this.OrthographicSize = 6f;
		this.targetOrthographicSize = 6f;
		Vector3 followPos = this.GetFollowPos();
		this.followTargetPos = new Vector3(followPos.x, followPos.y, base.transform.GetPosition().z);
		base.transform.SetPosition(this.followTargetPos);
		this.followTarget.GetComponent<KMonoBehaviour>().Trigger(-1506069671, null);
	}

	// Token: 0x06002D71 RID: 11633 RVA: 0x000C1E9A File Offset: 0x000C009A
	public void ClearFollowTarget()
	{
		if (this.followTarget == null)
		{
			return;
		}
		this.followTarget.GetComponent<KMonoBehaviour>().Trigger(-485480405, null);
		this.followTarget = null;
	}

	// Token: 0x06002D72 RID: 11634 RVA: 0x001FE3C8 File Offset: 0x001FC5C8
	public void UpdateFollowTarget()
	{
		if (this.followTarget != null)
		{
			Vector3 followPos = this.GetFollowPos();
			Vector2 a = new Vector2(base.transform.GetLocalPosition().x, base.transform.GetLocalPosition().y);
			byte b = Grid.WorldIdx[Grid.PosToCell(followPos)];
			if (ClusterManager.Instance.activeWorldId != (int)b)
			{
				Transform transform = this.followTarget;
				this.SetFollowTarget(null);
				ClusterManager.Instance.SetActiveWorld((int)b);
				this.SetFollowTarget(transform);
				return;
			}
			Vector2 vector = Vector2.Lerp(a, followPos, Time.unscaledDeltaTime * 25f);
			this.followTargetPos = new Vector3(vector.x, vector.y, base.transform.GetLocalPosition().z);
		}
	}

	// Token: 0x06002D73 RID: 11635 RVA: 0x001FE494 File Offset: 0x001FC694
	public void RenderForTimelapser(ref RenderTexture tex)
	{
		this.RenderCameraForTimelapse(this.baseCamera, ref tex, this.timelapseCameraCullingMask, -1f);
		CameraClearFlags clearFlags = this.overlayCamera.clearFlags;
		this.overlayCamera.clearFlags = CameraClearFlags.Nothing;
		this.RenderCameraForTimelapse(this.overlayCamera, ref tex, this.timelapseOverlayCameraCullingMask, -1f);
		this.overlayCamera.clearFlags = clearFlags;
	}

	// Token: 0x06002D74 RID: 11636 RVA: 0x001FE4F8 File Offset: 0x001FC6F8
	private void RenderCameraForTimelapse(Camera cam, ref RenderTexture tex, LayerMask mask, float overrideAspect = -1f)
	{
		int cullingMask = cam.cullingMask;
		RenderTexture targetTexture = cam.targetTexture;
		cam.targetTexture = tex;
		cam.aspect = (float)tex.width / (float)tex.height;
		if (overrideAspect != -1f)
		{
			cam.aspect = overrideAspect;
		}
		if (mask != -1)
		{
			cam.cullingMask = mask;
		}
		cam.Render();
		cam.ResetAspect();
		cam.cullingMask = cullingMask;
		cam.targetTexture = targetTexture;
	}

	// Token: 0x06002D75 RID: 11637 RVA: 0x000C1EC8 File Offset: 0x000C00C8
	private void CheckMoveUnpause()
	{
		if (this.cinemaCamEnabled && this.cinemaUnpauseNextMove)
		{
			this.cinemaUnpauseNextMove = !this.cinemaUnpauseNextMove;
			if (SpeedControlScreen.Instance.IsPaused)
			{
				SpeedControlScreen.Instance.Unpause(false);
			}
		}
	}

	// Token: 0x04001EE8 RID: 7912
	public const float DEFAULT_MAX_ORTHO_SIZE = 20f;

	// Token: 0x04001EE9 RID: 7913
	public const float MAX_Y_SCALE = 1.1f;

	// Token: 0x04001EEA RID: 7914
	public LocText infoText;

	// Token: 0x04001EEB RID: 7915
	private const float FIXED_Z = -100f;

	// Token: 0x04001EED RID: 7917
	public bool FreeCameraEnabled;

	// Token: 0x04001EEE RID: 7918
	public float zoomSpeed;

	// Token: 0x04001EEF RID: 7919
	public float minOrthographicSize;

	// Token: 0x04001EF0 RID: 7920
	public float zoomFactor;

	// Token: 0x04001EF1 RID: 7921
	public float keyPanningSpeed;

	// Token: 0x04001EF2 RID: 7922
	public float keyPanningEasing;

	// Token: 0x04001EF3 RID: 7923
	public Texture2D dayColourCube;

	// Token: 0x04001EF4 RID: 7924
	public Texture2D nightColourCube;

	// Token: 0x04001EF5 RID: 7925
	public Material LightBufferMaterial;

	// Token: 0x04001EF6 RID: 7926
	public Material LightCircleOverlay;

	// Token: 0x04001EF7 RID: 7927
	public Material LightConeOverlay;

	// Token: 0x04001EF8 RID: 7928
	public Transform followTarget;

	// Token: 0x04001EF9 RID: 7929
	public Vector3 followTargetPos;

	// Token: 0x04001EFA RID: 7930
	public GridVisibleArea VisibleArea = new GridVisibleArea(8);

	// Token: 0x04001EFC RID: 7932
	private float maxOrthographicSize = 20f;

	// Token: 0x04001EFD RID: 7933
	private float overrideZoomSpeed;

	// Token: 0x04001EFE RID: 7934
	private bool panning;

	// Token: 0x04001EFF RID: 7935
	private const float MaxEdgePaddingPercent = 0.33f;

	// Token: 0x04001F00 RID: 7936
	private Vector3 keyPanDelta;

	// Token: 0x04001F03 RID: 7939
	[SerializeField]
	private LayerMask timelapseCameraCullingMask;

	// Token: 0x04001F04 RID: 7940
	[SerializeField]
	private LayerMask timelapseOverlayCameraCullingMask;

	// Token: 0x04001F06 RID: 7942
	private bool userCameraControlDisabled;

	// Token: 0x04001F07 RID: 7943
	private bool panLeft;

	// Token: 0x04001F08 RID: 7944
	private bool panRight;

	// Token: 0x04001F09 RID: 7945
	private bool panUp;

	// Token: 0x04001F0A RID: 7946
	private bool panDown;

	// Token: 0x04001F0C RID: 7948
	[NonSerialized]
	public Camera baseCamera;

	// Token: 0x04001F0D RID: 7949
	[NonSerialized]
	public Camera overlayCamera;

	// Token: 0x04001F0E RID: 7950
	[NonSerialized]
	public Camera overlayNoDepthCamera;

	// Token: 0x04001F0F RID: 7951
	[NonSerialized]
	public Camera uiCamera;

	// Token: 0x04001F10 RID: 7952
	[NonSerialized]
	public Camera lightBufferCamera;

	// Token: 0x04001F11 RID: 7953
	[NonSerialized]
	public Camera simOverlayCamera;

	// Token: 0x04001F12 RID: 7954
	[NonSerialized]
	public Camera infraredCamera;

	// Token: 0x04001F13 RID: 7955
	[NonSerialized]
	public Camera timelapseFreezeCamera;

	// Token: 0x04001F14 RID: 7956
	[SerializeField]
	private List<GameScreenManager.UIRenderTarget> uiCameraTargets;

	// Token: 0x04001F15 RID: 7957
	public List<Camera> cameras = new List<Camera>();

	// Token: 0x04001F16 RID: 7958
	private MultipleRenderTarget mrt;

	// Token: 0x04001F17 RID: 7959
	public SoundCuller soundCuller;

	// Token: 0x04001F18 RID: 7960
	private bool cinemaCamEnabled;

	// Token: 0x04001F19 RID: 7961
	private bool cinemaToggleLock;

	// Token: 0x04001F1A RID: 7962
	private bool cinemaToggleEasing;

	// Token: 0x04001F1B RID: 7963
	private bool cinemaUnpauseNextMove;

	// Token: 0x04001F1C RID: 7964
	private bool cinemaPanLeft;

	// Token: 0x04001F1D RID: 7965
	private bool cinemaPanRight;

	// Token: 0x04001F1E RID: 7966
	private bool cinemaPanUp;

	// Token: 0x04001F1F RID: 7967
	private bool cinemaPanDown;

	// Token: 0x04001F20 RID: 7968
	private bool cinemaZoomIn;

	// Token: 0x04001F21 RID: 7969
	private bool cinemaZoomOut;

	// Token: 0x04001F22 RID: 7970
	private int cinemaZoomSpeed = 10;

	// Token: 0x04001F23 RID: 7971
	private float cinemaEasing = 0.05f;

	// Token: 0x04001F24 RID: 7972
	private float cinemaZoomVelocity;

	// Token: 0x04001F26 RID: 7974
	private float smoothDt;

	// Token: 0x020009CF RID: 2511
	public class Tuning : TuningData<CameraController.Tuning>
	{
		// Token: 0x04001F27 RID: 7975
		public float maxOrthographicSizeDebug;

		// Token: 0x04001F28 RID: 7976
		public float cinemaZoomFactor = 100f;

		// Token: 0x04001F29 RID: 7977
		public float cinemaPanFactor = 50f;

		// Token: 0x04001F2A RID: 7978
		public float cinemaZoomToFactor = 100f;

		// Token: 0x04001F2B RID: 7979
		public float cinemaPanToFactor = 50f;

		// Token: 0x04001F2C RID: 7980
		public float targetZoomEasingFactor = 400f;

		// Token: 0x04001F2D RID: 7981
		public float targetPanEasingFactor = 100f;
	}
}
