using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FMOD.Studio;
using STRINGS;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02001C72 RID: 7282
public class ClusterMapScreen : KScreen
{
	// Token: 0x06009761 RID: 38753 RVA: 0x00107133 File Offset: 0x00105333
	public static void DestroyInstance()
	{
		ClusterMapScreen.Instance = null;
	}

	// Token: 0x06009762 RID: 38754 RVA: 0x0010713B File Offset: 0x0010533B
	public ClusterMapVisualizer GetEntityVisAnim(ClusterGridEntity entity)
	{
		if (this.m_gridEntityAnims.ContainsKey(entity))
		{
			return this.m_gridEntityAnims[entity];
		}
		return null;
	}

	// Token: 0x06009763 RID: 38755 RVA: 0x00107159 File Offset: 0x00105359
	public override float GetSortKey()
	{
		if (base.isEditing)
		{
			return 50f;
		}
		return 20f;
	}

	// Token: 0x06009764 RID: 38756 RVA: 0x0010716E File Offset: 0x0010536E
	public float CurrentZoomPercentage()
	{
		return (this.m_currentZoomScale - 50f) / 100f;
	}

	// Token: 0x06009765 RID: 38757 RVA: 0x00107182 File Offset: 0x00105382
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.m_selectMarker = global::Util.KInstantiateUI<SelectMarker>(this.selectMarkerPrefab, base.gameObject, false);
		this.m_selectMarker.gameObject.SetActive(false);
		ClusterMapScreen.Instance = this;
	}

	// Token: 0x06009766 RID: 38758 RVA: 0x003B2FA8 File Offset: 0x003B11A8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		global::Debug.Assert(this.cellVisPrefab.rectTransform().sizeDelta == new Vector2(2f, 2f), "The radius of the cellVisPrefab hex must be 1");
		global::Debug.Assert(this.terrainVisPrefab.rectTransform().sizeDelta == new Vector2(2f, 2f), "The radius of the terrainVisPrefab hex must be 1");
		global::Debug.Assert(this.mobileVisPrefab.rectTransform().sizeDelta == new Vector2(2f, 2f), "The radius of the mobileVisPrefab hex must be 1");
		global::Debug.Assert(this.staticVisPrefab.rectTransform().sizeDelta == new Vector2(2f, 2f), "The radius of the staticVisPrefab hex must be 1");
		int num;
		int num2;
		int num3;
		int num4;
		this.GenerateGridVis(out num, out num2, out num3, out num4);
		this.Show(false);
		this.mapScrollRect.content.sizeDelta = new Vector2((float)(num2 * 4), (float)(num4 * 4));
		this.mapScrollRect.content.localScale = new Vector3(this.m_currentZoomScale, this.m_currentZoomScale, 1f);
		this.m_onDestinationChangedDelegate = new Action<object>(this.OnDestinationChanged);
		this.m_onSelectObjectDelegate = new Action<object>(this.OnSelectObject);
		base.Subscribe(1980521255, new Action<object>(this.UpdateVis));
	}

	// Token: 0x06009767 RID: 38759 RVA: 0x003B3108 File Offset: 0x003B1308
	protected void MoveToNISPosition()
	{
		if (!this.movingToTargetNISPosition)
		{
			return;
		}
		Vector3 b = new Vector3(-this.targetNISPosition.x * this.mapScrollRect.content.localScale.x, -this.targetNISPosition.y * this.mapScrollRect.content.localScale.y, this.targetNISPosition.z);
		this.m_targetZoomScale = Mathf.Lerp(this.m_targetZoomScale, this.targetNISZoom, Time.unscaledDeltaTime * 2f);
		this.mapScrollRect.content.SetLocalPosition(Vector3.Lerp(this.mapScrollRect.content.GetLocalPosition(), b, Time.unscaledDeltaTime * 2.5f));
		float num = Vector3.Distance(this.mapScrollRect.content.GetLocalPosition(), b);
		if (num < 100f)
		{
			ClusterMapHex component = this.m_cellVisByLocation[this.selectOnMoveNISComplete].GetComponent<ClusterMapHex>();
			if (this.m_selectedHex != component)
			{
				this.SelectHex(component);
			}
			if (num < 10f)
			{
				this.movingToTargetNISPosition = false;
			}
		}
	}

	// Token: 0x06009768 RID: 38760 RVA: 0x001071B9 File Offset: 0x001053B9
	public void SetTargetFocusPosition(AxialI targetPosition, float delayBeforeMove = 0.5f)
	{
		if (this.activeMoveToTargetRoutine != null)
		{
			base.StopCoroutine(this.activeMoveToTargetRoutine);
		}
		this.activeMoveToTargetRoutine = base.StartCoroutine(this.MoveToTargetRoutine(targetPosition, delayBeforeMove));
	}

	// Token: 0x06009769 RID: 38761 RVA: 0x001071E3 File Offset: 0x001053E3
	private IEnumerator MoveToTargetRoutine(AxialI targetPosition, float delayBeforeMove)
	{
		delayBeforeMove = Mathf.Max(delayBeforeMove, 0f);
		yield return SequenceUtil.WaitForSecondsRealtime(delayBeforeMove);
		this.targetNISPosition = AxialUtil.AxialToWorld((float)targetPosition.r, (float)targetPosition.q);
		this.targetNISZoom = 150f;
		this.movingToTargetNISPosition = true;
		this.selectOnMoveNISComplete = targetPosition;
		yield break;
	}

	// Token: 0x0600976A RID: 38762 RVA: 0x003B3224 File Offset: 0x003B1424
	public override void OnKeyDown(KButtonEvent e)
	{
		if (!e.Consumed && (e.IsAction(global::Action.ZoomIn) || e.IsAction(global::Action.ZoomOut)) && CameraController.IsMouseOverGameWindow)
		{
			List<RaycastResult> list = new List<RaycastResult>();
			PointerEventData pointerEventData = new PointerEventData(UnityEngine.EventSystems.EventSystem.current);
			pointerEventData.position = KInputManager.GetMousePos();
			UnityEngine.EventSystems.EventSystem current = UnityEngine.EventSystems.EventSystem.current;
			if (current != null)
			{
				current.RaycastAll(pointerEventData, list);
				bool flag = false;
				foreach (RaycastResult raycastResult in list)
				{
					if (!raycastResult.gameObject.transform.IsChildOf(base.transform))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					float num;
					if (KInputManager.currentControllerIsGamepad)
					{
						num = 25f;
						num *= (float)(e.IsAction(global::Action.ZoomIn) ? 1 : -1);
					}
					else
					{
						num = Input.mouseScrollDelta.y * 25f;
					}
					this.m_targetZoomScale = Mathf.Clamp(this.m_targetZoomScale + num, 50f, 150f);
					e.TryConsume(global::Action.ZoomIn);
					if (!e.Consumed)
					{
						e.TryConsume(global::Action.ZoomOut);
					}
				}
			}
		}
		CameraController.Instance.ChangeWorldInput(e);
		base.OnKeyDown(e);
	}

	// Token: 0x0600976B RID: 38763 RVA: 0x00107200 File Offset: 0x00105400
	public bool TryHandleCancel()
	{
		if (this.m_mode == ClusterMapScreen.Mode.SelectDestination && !this.m_closeOnSelect)
		{
			this.SetMode(ClusterMapScreen.Mode.Default);
			return true;
		}
		return false;
	}

	// Token: 0x0600976C RID: 38764 RVA: 0x003B337C File Offset: 0x003B157C
	public void ShowInSelectDestinationMode(ClusterDestinationSelector destination_selector)
	{
		this.m_destinationSelector = destination_selector;
		if (!base.gameObject.activeSelf)
		{
			ManagementMenu.Instance.ToggleClusterMap();
			this.m_closeOnSelect = true;
		}
		ClusterGridEntity component = destination_selector.GetComponent<ClusterGridEntity>();
		this.SetSelectedEntity(component, false);
		if (this.m_selectedEntity != null)
		{
			this.m_selectedHex = this.m_cellVisByLocation[this.m_selectedEntity.Location].GetComponent<ClusterMapHex>();
		}
		else
		{
			AxialI myWorldLocation = destination_selector.GetMyWorldLocation();
			ClusterMapHex component2 = this.m_cellVisByLocation[myWorldLocation].GetComponent<ClusterMapHex>();
			this.m_selectedHex = component2;
		}
		this.SetMode(ClusterMapScreen.Mode.SelectDestination);
	}

	// Token: 0x0600976D RID: 38765 RVA: 0x0010721D File Offset: 0x0010541D
	private void SetMode(ClusterMapScreen.Mode mode)
	{
		this.m_mode = mode;
		if (this.m_mode == ClusterMapScreen.Mode.Default)
		{
			this.m_destinationSelector = null;
		}
		this.UpdateVis(null);
	}

	// Token: 0x0600976E RID: 38766 RVA: 0x0010723C File Offset: 0x0010543C
	public ClusterMapScreen.Mode GetMode()
	{
		return this.m_mode;
	}

	// Token: 0x0600976F RID: 38767 RVA: 0x003B3418 File Offset: 0x003B1618
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (show)
		{
			this.MoveToNISPosition();
			this.UpdateVis(null);
			if (this.m_mode == ClusterMapScreen.Mode.Default)
			{
				this.TrySelectDefault();
			}
			Game.Instance.Subscribe(-1991583975, new Action<object>(this.OnFogOfWarRevealed));
			Game.Instance.Subscribe(-1554423969, new Action<object>(this.OnNewTelescopeTarget));
			Game.Instance.Subscribe(-1298331547, new Action<object>(this.OnClusterLocationChanged));
			ClusterMapSelectTool.Instance.Activate();
			this.SetShowingNonClusterMapHud(false);
			CameraController.Instance.DisableUserCameraControl = true;
			AudioMixer.instance.Start(AudioMixerSnapshots.Get().MENUStarmapNotPausedSnapshot);
			MusicManager.instance.PlaySong("Music_Starmap", false);
			this.UpdateTearStatus();
			return;
		}
		Game.Instance.Unsubscribe(-1554423969, new Action<object>(this.OnNewTelescopeTarget));
		Game.Instance.Unsubscribe(-1991583975, new Action<object>(this.OnFogOfWarRevealed));
		Game.Instance.Unsubscribe(-1298331547, new Action<object>(this.OnClusterLocationChanged));
		this.m_mode = ClusterMapScreen.Mode.Default;
		this.m_closeOnSelect = false;
		this.m_destinationSelector = null;
		SelectTool.Instance.Activate();
		this.SetShowingNonClusterMapHud(true);
		CameraController.Instance.DisableUserCameraControl = false;
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().MENUStarmapNotPausedSnapshot, STOP_MODE.ALLOWFADEOUT);
		if (MusicManager.instance.SongIsPlaying("Music_Starmap"))
		{
			MusicManager.instance.StopSong("Music_Starmap", true, STOP_MODE.ALLOWFADEOUT);
		}
	}

	// Token: 0x06009770 RID: 38768 RVA: 0x00107244 File Offset: 0x00105444
	private void SetShowingNonClusterMapHud(bool show)
	{
		PlanScreen.Instance.gameObject.SetActive(show);
		ToolMenu.Instance.gameObject.SetActive(show);
		OverlayScreen.Instance.gameObject.SetActive(show);
	}

	// Token: 0x06009771 RID: 38769 RVA: 0x003B35A4 File Offset: 0x003B17A4
	private void SetSelectedEntity(ClusterGridEntity entity, bool frameDelay = false)
	{
		if (this.m_selectedEntity != null)
		{
			this.m_selectedEntity.Unsubscribe(543433792, this.m_onDestinationChangedDelegate);
			this.m_selectedEntity.Unsubscribe(-1503271301, this.m_onSelectObjectDelegate);
		}
		this.m_selectedEntity = entity;
		if (this.m_selectedEntity != null)
		{
			this.m_selectedEntity.Subscribe(543433792, this.m_onDestinationChangedDelegate);
			this.m_selectedEntity.Subscribe(-1503271301, this.m_onSelectObjectDelegate);
		}
		KSelectable new_selected = (this.m_selectedEntity != null) ? this.m_selectedEntity.GetComponent<KSelectable>() : null;
		if (frameDelay)
		{
			ClusterMapSelectTool.Instance.SelectNextFrame(new_selected, false);
			return;
		}
		ClusterMapSelectTool.Instance.Select(new_selected, false);
	}

	// Token: 0x06009772 RID: 38770 RVA: 0x00107276 File Offset: 0x00105476
	private void OnDestinationChanged(object data)
	{
		this.UpdateVis(null);
	}

	// Token: 0x06009773 RID: 38771 RVA: 0x003B3668 File Offset: 0x003B1868
	private void OnSelectObject(object data)
	{
		if (this.m_selectedEntity == null)
		{
			return;
		}
		KSelectable component = this.m_selectedEntity.GetComponent<KSelectable>();
		if (component == null || component.IsSelected)
		{
			return;
		}
		this.SetSelectedEntity(null, false);
		if (this.m_mode == ClusterMapScreen.Mode.SelectDestination)
		{
			if (this.m_closeOnSelect)
			{
				ManagementMenu.Instance.CloseAll();
			}
			else
			{
				this.SetMode(ClusterMapScreen.Mode.Default);
			}
		}
		this.UpdateVis(null);
	}

	// Token: 0x06009774 RID: 38772 RVA: 0x00107276 File Offset: 0x00105476
	private void OnFogOfWarRevealed(object data = null)
	{
		this.UpdateVis(null);
	}

	// Token: 0x06009775 RID: 38773 RVA: 0x00107276 File Offset: 0x00105476
	private void OnNewTelescopeTarget(object data = null)
	{
		this.UpdateVis(null);
	}

	// Token: 0x06009776 RID: 38774 RVA: 0x0010727F File Offset: 0x0010547F
	private void Update()
	{
		if (KInputManager.currentControllerIsGamepad)
		{
			this.mapScrollRect.AnalogUpdate(KInputManager.steamInputInterpreter.GetSteamCameraMovement() * this.scrollSpeed);
		}
	}

	// Token: 0x06009777 RID: 38775 RVA: 0x003B36D8 File Offset: 0x003B18D8
	private void TrySelectDefault()
	{
		if (this.m_selectedHex != null && this.m_selectedEntity != null)
		{
			this.UpdateVis(null);
			return;
		}
		WorldContainer activeWorld = ClusterManager.Instance.activeWorld;
		if (activeWorld == null)
		{
			return;
		}
		ClusterGridEntity component = activeWorld.GetComponent<ClusterGridEntity>();
		if (component == null)
		{
			return;
		}
		this.SelectEntity(component, false);
	}

	// Token: 0x06009778 RID: 38776 RVA: 0x003B3738 File Offset: 0x003B1938
	private void GenerateGridVis(out int minR, out int maxR, out int minQ, out int maxQ)
	{
		minR = int.MaxValue;
		maxR = int.MinValue;
		minQ = int.MaxValue;
		maxQ = int.MinValue;
		foreach (KeyValuePair<AxialI, List<ClusterGridEntity>> keyValuePair in ClusterGrid.Instance.cellContents)
		{
			ClusterMapVisualizer clusterMapVisualizer = UnityEngine.Object.Instantiate<ClusterMapVisualizer>(this.cellVisPrefab, Vector3.zero, Quaternion.identity, this.cellVisContainer.transform);
			clusterMapVisualizer.rectTransform().SetLocalPosition(keyValuePair.Key.ToWorld());
			clusterMapVisualizer.gameObject.SetActive(true);
			ClusterMapHex component = clusterMapVisualizer.GetComponent<ClusterMapHex>();
			component.SetLocation(keyValuePair.Key);
			this.m_cellVisByLocation.Add(keyValuePair.Key, clusterMapVisualizer);
			minR = Mathf.Min(minR, component.location.R);
			maxR = Mathf.Max(maxR, component.location.R);
			minQ = Mathf.Min(minQ, component.location.Q);
			maxQ = Mathf.Max(maxQ, component.location.Q);
		}
		this.SetupVisGameObjects();
		this.UpdateVis(null);
	}

	// Token: 0x06009779 RID: 38777 RVA: 0x003B388C File Offset: 0x003B1A8C
	public Transform GetGridEntityNameTarget(ClusterGridEntity entity)
	{
		ClusterMapVisualizer clusterMapVisualizer;
		if (this.m_currentZoomScale >= 115f && this.m_gridEntityVis.TryGetValue(entity, out clusterMapVisualizer))
		{
			return clusterMapVisualizer.nameTarget;
		}
		return null;
	}

	// Token: 0x0600977A RID: 38778 RVA: 0x003B38C0 File Offset: 0x003B1AC0
	public override void ScreenUpdate(bool topLevel)
	{
		float t = Mathf.Min(4f * Time.unscaledDeltaTime, 0.9f);
		this.m_currentZoomScale = Mathf.Lerp(this.m_currentZoomScale, this.m_targetZoomScale, t);
		Vector2 v = KInputManager.GetMousePos();
		Vector3 b = this.mapScrollRect.content.InverseTransformPoint(v);
		this.mapScrollRect.content.localScale = new Vector3(this.m_currentZoomScale, this.m_currentZoomScale, 1f);
		Vector3 a = this.mapScrollRect.content.InverseTransformPoint(v);
		this.mapScrollRect.content.localPosition += (a - b) * this.m_currentZoomScale;
		this.MoveToNISPosition();
		this.FloatyAsteroidAnimation();
	}

	// Token: 0x0600977B RID: 38779 RVA: 0x003B3994 File Offset: 0x003B1B94
	private void FloatyAsteroidAnimation()
	{
		float num = 0f;
		foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
		{
			AsteroidGridEntity component = worldContainer.GetComponent<AsteroidGridEntity>();
			if (component != null && this.m_gridEntityVis.ContainsKey(component) && ClusterMapScreen.GetRevealLevel(component) == ClusterRevealLevel.Visible)
			{
				KAnimControllerBase firstAnimController = this.m_gridEntityVis[component].GetFirstAnimController();
				float y = this.floatCycleOffset + this.floatCycleScale * Mathf.Sin(this.floatCycleSpeed * (num + GameClock.Instance.GetTime()));
				firstAnimController.Offset = new Vector2(0f, y);
			}
			num += 1f;
		}
	}

	// Token: 0x0600977C RID: 38780 RVA: 0x003B3A6C File Offset: 0x003B1C6C
	private void SetupVisGameObjects()
	{
		foreach (KeyValuePair<AxialI, List<ClusterGridEntity>> keyValuePair in ClusterGrid.Instance.cellContents)
		{
			foreach (ClusterGridEntity clusterGridEntity in keyValuePair.Value)
			{
				ClusterGrid.Instance.GetCellRevealLevel(keyValuePair.Key);
				ClusterRevealLevel isVisibleInFOW = clusterGridEntity.IsVisibleInFOW;
				ClusterRevealLevel revealLevel = ClusterMapScreen.GetRevealLevel(clusterGridEntity);
				if (clusterGridEntity.IsVisible && revealLevel != ClusterRevealLevel.Hidden && !this.m_gridEntityVis.ContainsKey(clusterGridEntity))
				{
					ClusterMapVisualizer original = null;
					GameObject gameObject = null;
					switch (clusterGridEntity.Layer)
					{
					case EntityLayer.Asteroid:
						original = this.terrainVisPrefab;
						gameObject = this.terrainVisContainer;
						break;
					case EntityLayer.Craft:
						original = this.mobileVisPrefab;
						gameObject = this.mobileVisContainer;
						break;
					case EntityLayer.POI:
						original = this.staticVisPrefab;
						gameObject = this.POIVisContainer;
						break;
					case EntityLayer.Telescope:
						original = this.staticVisPrefab;
						gameObject = this.telescopeVisContainer;
						break;
					case EntityLayer.Payload:
						original = this.mobileVisPrefab;
						gameObject = this.mobileVisContainer;
						break;
					case EntityLayer.FX:
						original = this.staticVisPrefab;
						gameObject = this.FXVisContainer;
						break;
					}
					ClusterNameDisplayScreen.Instance.AddNewEntry(clusterGridEntity);
					ClusterMapVisualizer clusterMapVisualizer = UnityEngine.Object.Instantiate<ClusterMapVisualizer>(original, gameObject.transform);
					clusterMapVisualizer.Init(clusterGridEntity, this.pathDrawer);
					clusterMapVisualizer.gameObject.SetActive(true);
					this.m_gridEntityAnims.Add(clusterGridEntity, clusterMapVisualizer);
					this.m_gridEntityVis.Add(clusterGridEntity, clusterMapVisualizer);
					clusterGridEntity.positionDirty = false;
					clusterGridEntity.Subscribe(1502190696, new Action<object>(this.RemoveDeletedEntities));
				}
			}
		}
		this.RemoveDeletedEntities(null);
		foreach (KeyValuePair<ClusterGridEntity, ClusterMapVisualizer> keyValuePair2 in this.m_gridEntityVis)
		{
			ClusterGridEntity key = keyValuePair2.Key;
			if (key.Layer == EntityLayer.Asteroid)
			{
				int id = key.GetComponent<WorldContainer>().id;
				keyValuePair2.Value.alertVignette.worldID = id;
			}
		}
	}

	// Token: 0x0600977D RID: 38781 RVA: 0x003B3CF0 File Offset: 0x003B1EF0
	private void RemoveDeletedEntities(object obj = null)
	{
		foreach (ClusterGridEntity key in (from x in this.m_gridEntityVis.Keys
		where x == null || x.gameObject == (GameObject)obj
		select x).ToList<ClusterGridEntity>())
		{
			global::Util.KDestroyGameObject(this.m_gridEntityVis[key]);
			this.m_gridEntityVis.Remove(key);
			this.m_gridEntityAnims.Remove(key);
		}
	}

	// Token: 0x0600977E RID: 38782 RVA: 0x00107276 File Offset: 0x00105476
	private void OnClusterLocationChanged(object data)
	{
		this.UpdateVis(null);
	}

	// Token: 0x0600977F RID: 38783 RVA: 0x003B3D90 File Offset: 0x003B1F90
	public static ClusterRevealLevel GetRevealLevel(ClusterGridEntity entity)
	{
		ClusterRevealLevel cellRevealLevel = ClusterGrid.Instance.GetCellRevealLevel(entity.Location);
		ClusterRevealLevel isVisibleInFOW = entity.IsVisibleInFOW;
		if (cellRevealLevel == ClusterRevealLevel.Visible || isVisibleInFOW == ClusterRevealLevel.Visible)
		{
			return ClusterRevealLevel.Visible;
		}
		if (cellRevealLevel == ClusterRevealLevel.Peeked && isVisibleInFOW == ClusterRevealLevel.Peeked)
		{
			return ClusterRevealLevel.Peeked;
		}
		return ClusterRevealLevel.Hidden;
	}

	// Token: 0x06009780 RID: 38784 RVA: 0x003B3DCC File Offset: 0x003B1FCC
	private void UpdateVis(object data = null)
	{
		this.SetupVisGameObjects();
		this.UpdatePaths();
		foreach (KeyValuePair<ClusterGridEntity, ClusterMapVisualizer> keyValuePair in this.m_gridEntityAnims)
		{
			ClusterRevealLevel revealLevel = ClusterMapScreen.GetRevealLevel(keyValuePair.Key);
			keyValuePair.Value.Show(revealLevel);
			bool selected = this.m_selectedEntity == keyValuePair.Key;
			keyValuePair.Value.Select(selected);
			if (keyValuePair.Key.positionDirty)
			{
				Vector3 position = ClusterGrid.Instance.GetPosition(keyValuePair.Key);
				keyValuePair.Value.rectTransform().SetLocalPosition(position);
				keyValuePair.Key.positionDirty = false;
			}
		}
		if (this.m_selectedEntity != null && this.m_gridEntityVis.ContainsKey(this.m_selectedEntity))
		{
			ClusterMapVisualizer clusterMapVisualizer = this.m_gridEntityVis[this.m_selectedEntity];
			this.m_selectMarker.SetTargetTransform(clusterMapVisualizer.transform);
			this.m_selectMarker.gameObject.SetActive(true);
			clusterMapVisualizer.transform.SetAsLastSibling();
		}
		else
		{
			this.m_selectMarker.gameObject.SetActive(false);
		}
		foreach (KeyValuePair<AxialI, ClusterMapVisualizer> keyValuePair2 in this.m_cellVisByLocation)
		{
			ClusterMapHex component = keyValuePair2.Value.GetComponent<ClusterMapHex>();
			AxialI key = keyValuePair2.Key;
			component.SetRevealed(ClusterGrid.Instance.GetCellRevealLevel(key));
		}
		this.UpdateHexToggleStates();
		this.FloatyAsteroidAnimation();
	}

	// Token: 0x06009781 RID: 38785 RVA: 0x001072A8 File Offset: 0x001054A8
	private void OnEntityDestroyed(object obj)
	{
		this.RemoveDeletedEntities(null);
	}

	// Token: 0x06009782 RID: 38786 RVA: 0x003B3F8C File Offset: 0x003B218C
	private void UpdateHexToggleStates()
	{
		bool flag = this.m_hoveredHex != null && ClusterGrid.Instance.GetVisibleEntityOfLayerAtCell(this.m_hoveredHex.location, EntityLayer.Asteroid);
		foreach (KeyValuePair<AxialI, ClusterMapVisualizer> keyValuePair in this.m_cellVisByLocation)
		{
			ClusterMapHex component = keyValuePair.Value.GetComponent<ClusterMapHex>();
			AxialI key = keyValuePair.Key;
			ClusterMapHex.ToggleState state;
			if (this.m_selectedHex != null && this.m_selectedHex.location == key)
			{
				state = ClusterMapHex.ToggleState.Selected;
			}
			else if (flag && this.m_hoveredHex.location.IsAdjacent(key))
			{
				state = ClusterMapHex.ToggleState.OrbitHighlight;
			}
			else
			{
				state = ClusterMapHex.ToggleState.Unselected;
			}
			component.UpdateToggleState(state);
		}
	}

	// Token: 0x06009783 RID: 38787 RVA: 0x003B4064 File Offset: 0x003B2264
	public void SelectEntity(ClusterGridEntity entity, bool frameDelay = false)
	{
		if (entity != null)
		{
			this.SetSelectedEntity(entity, frameDelay);
			ClusterMapHex component = this.m_cellVisByLocation[entity.Location].GetComponent<ClusterMapHex>();
			this.m_selectedHex = component;
		}
		this.UpdateVis(null);
	}

	// Token: 0x06009784 RID: 38788 RVA: 0x003B40A8 File Offset: 0x003B22A8
	public void SelectHex(ClusterMapHex newSelectionHex)
	{
		if (this.m_mode == ClusterMapScreen.Mode.Default)
		{
			List<ClusterGridEntity> visibleEntitiesAtCell = ClusterGrid.Instance.GetVisibleEntitiesAtCell(newSelectionHex.location);
			for (int i = visibleEntitiesAtCell.Count - 1; i >= 0; i--)
			{
				KSelectable component = visibleEntitiesAtCell[i].GetComponent<KSelectable>();
				if (component == null || !component.IsSelectable)
				{
					visibleEntitiesAtCell.RemoveAt(i);
				}
			}
			if (visibleEntitiesAtCell.Count == 0)
			{
				this.SetSelectedEntity(null, false);
			}
			else
			{
				int num = visibleEntitiesAtCell.IndexOf(this.m_selectedEntity);
				int index = 0;
				if (num >= 0)
				{
					index = (num + 1) % visibleEntitiesAtCell.Count;
				}
				this.SetSelectedEntity(visibleEntitiesAtCell[index], false);
			}
			this.m_selectedHex = newSelectionHex;
		}
		else if (this.m_mode == ClusterMapScreen.Mode.SelectDestination)
		{
			global::Debug.Assert(this.m_destinationSelector != null, "Selected a hex in SelectDestination mode with no ClusterDestinationSelector");
			if (ClusterGrid.Instance.GetPath(this.m_selectedHex.location, newSelectionHex.location, this.m_destinationSelector) != null)
			{
				this.m_destinationSelector.SetDestination(newSelectionHex.location);
				if (this.m_closeOnSelect)
				{
					ManagementMenu.Instance.CloseAll();
				}
				else
				{
					this.SetMode(ClusterMapScreen.Mode.Default);
				}
			}
		}
		this.UpdateVis(null);
	}

	// Token: 0x06009785 RID: 38789 RVA: 0x001072B1 File Offset: 0x001054B1
	public bool HasCurrentHover()
	{
		return this.m_hoveredHex != null;
	}

	// Token: 0x06009786 RID: 38790 RVA: 0x001072BF File Offset: 0x001054BF
	public AxialI GetCurrentHoverLocation()
	{
		return this.m_hoveredHex.location;
	}

	// Token: 0x06009787 RID: 38791 RVA: 0x001072CC File Offset: 0x001054CC
	public void OnHoverHex(ClusterMapHex newHoverHex)
	{
		this.m_hoveredHex = newHoverHex;
		if (this.m_mode == ClusterMapScreen.Mode.SelectDestination)
		{
			this.UpdateVis(null);
		}
		this.UpdateHexToggleStates();
	}

	// Token: 0x06009788 RID: 38792 RVA: 0x001072EB File Offset: 0x001054EB
	public void OnUnhoverHex(ClusterMapHex unhoveredHex)
	{
		if (this.m_hoveredHex == unhoveredHex)
		{
			this.m_hoveredHex = null;
			this.UpdateHexToggleStates();
		}
	}

	// Token: 0x06009789 RID: 38793 RVA: 0x00107308 File Offset: 0x00105508
	public void SetLocationHighlight(AxialI location, bool highlight)
	{
		this.m_cellVisByLocation[location].GetComponent<ClusterMapHex>().ChangeState(highlight ? 1 : 0);
	}

	// Token: 0x0600978A RID: 38794 RVA: 0x003B41C8 File Offset: 0x003B23C8
	private void UpdatePaths()
	{
		ClusterDestinationSelector clusterDestinationSelector = (this.m_selectedEntity != null) ? this.m_selectedEntity.GetComponent<ClusterDestinationSelector>() : null;
		if (this.m_mode != ClusterMapScreen.Mode.SelectDestination || !(this.m_hoveredHex != null))
		{
			if (this.m_previewMapPath != null)
			{
				global::Util.KDestroyGameObject(this.m_previewMapPath);
				this.m_previewMapPath = null;
			}
			return;
		}
		global::Debug.Assert(this.m_destinationSelector != null, "In SelectDestination mode without a destination selector");
		AxialI myWorldLocation = this.m_destinationSelector.GetMyWorldLocation();
		string text;
		List<AxialI> path = ClusterGrid.Instance.GetPath(myWorldLocation, this.m_hoveredHex.location, this.m_destinationSelector, out text, false);
		if (path != null)
		{
			if (this.m_previewMapPath == null)
			{
				this.m_previewMapPath = this.pathDrawer.AddPath();
			}
			ClusterMapVisualizer clusterMapVisualizer = this.m_gridEntityVis[this.GetSelectorGridEntity(this.m_destinationSelector)];
			this.m_previewMapPath.SetPoints(ClusterMapPathDrawer.GetDrawPathList(clusterMapVisualizer.transform.localPosition, path));
			this.m_previewMapPath.SetColor(this.rocketPreviewPathColor);
		}
		else if (this.m_previewMapPath != null)
		{
			global::Util.KDestroyGameObject(this.m_previewMapPath);
			this.m_previewMapPath = null;
		}
		int num = (path != null) ? path.Count : -1;
		if (this.m_selectedEntity != null)
		{
			int rangeInTiles = this.m_selectedEntity.GetComponent<IClusterRange>().GetRangeInTiles();
			if (num > rangeInTiles && string.IsNullOrEmpty(text))
			{
				text = string.Format(UI.CLUSTERMAP.TOOLTIP_INVALID_DESTINATION_OUT_OF_RANGE, rangeInTiles);
			}
			bool repeat = clusterDestinationSelector.GetComponent<RocketClusterDestinationSelector>().Repeat;
			this.m_hoveredHex.SetDestinationStatus(text, num, rangeInTiles, repeat);
			return;
		}
		this.m_hoveredHex.SetDestinationStatus(text);
	}

	// Token: 0x0600978B RID: 38795 RVA: 0x003B4384 File Offset: 0x003B2584
	private ClusterGridEntity GetSelectorGridEntity(ClusterDestinationSelector selector)
	{
		ClusterGridEntity component = selector.GetComponent<ClusterGridEntity>();
		if (component != null && ClusterGrid.Instance.IsVisible(component))
		{
			return component;
		}
		ClusterGridEntity visibleEntityOfLayerAtCell = ClusterGrid.Instance.GetVisibleEntityOfLayerAtCell(selector.GetMyWorldLocation(), EntityLayer.Asteroid);
		global::Debug.Assert(component != null || visibleEntityOfLayerAtCell != null, string.Format("{0} has no grid entity and isn't located at a visible asteroid at {1}", selector, selector.GetMyWorldLocation()));
		if (visibleEntityOfLayerAtCell)
		{
			return visibleEntityOfLayerAtCell;
		}
		return component;
	}

	// Token: 0x0600978C RID: 38796 RVA: 0x003B43FC File Offset: 0x003B25FC
	private void UpdateTearStatus()
	{
		ClusterPOIManager clusterPOIManager = null;
		if (ClusterManager.Instance != null)
		{
			clusterPOIManager = ClusterManager.Instance.GetComponent<ClusterPOIManager>();
		}
		if (clusterPOIManager != null)
		{
			TemporalTear temporalTear = clusterPOIManager.GetTemporalTear();
			if (temporalTear != null)
			{
				temporalTear.UpdateStatus();
			}
		}
	}

	// Token: 0x040075DD RID: 30173
	public static ClusterMapScreen Instance;

	// Token: 0x040075DE RID: 30174
	public GameObject cellVisContainer;

	// Token: 0x040075DF RID: 30175
	public GameObject terrainVisContainer;

	// Token: 0x040075E0 RID: 30176
	public GameObject mobileVisContainer;

	// Token: 0x040075E1 RID: 30177
	public GameObject telescopeVisContainer;

	// Token: 0x040075E2 RID: 30178
	public GameObject POIVisContainer;

	// Token: 0x040075E3 RID: 30179
	public GameObject FXVisContainer;

	// Token: 0x040075E4 RID: 30180
	public ClusterMapVisualizer cellVisPrefab;

	// Token: 0x040075E5 RID: 30181
	public ClusterMapVisualizer terrainVisPrefab;

	// Token: 0x040075E6 RID: 30182
	public ClusterMapVisualizer mobileVisPrefab;

	// Token: 0x040075E7 RID: 30183
	public ClusterMapVisualizer staticVisPrefab;

	// Token: 0x040075E8 RID: 30184
	public Color rocketPathColor;

	// Token: 0x040075E9 RID: 30185
	public Color rocketSelectedPathColor;

	// Token: 0x040075EA RID: 30186
	public Color rocketPreviewPathColor;

	// Token: 0x040075EB RID: 30187
	private ClusterMapHex m_selectedHex;

	// Token: 0x040075EC RID: 30188
	private ClusterMapHex m_hoveredHex;

	// Token: 0x040075ED RID: 30189
	private ClusterGridEntity m_selectedEntity;

	// Token: 0x040075EE RID: 30190
	public KButton closeButton;

	// Token: 0x040075EF RID: 30191
	private const float ZOOM_SCALE_MIN = 50f;

	// Token: 0x040075F0 RID: 30192
	private const float ZOOM_SCALE_MAX = 150f;

	// Token: 0x040075F1 RID: 30193
	private const float ZOOM_SCALE_INCREMENT = 25f;

	// Token: 0x040075F2 RID: 30194
	private const float ZOOM_SCALE_SPEED = 4f;

	// Token: 0x040075F3 RID: 30195
	private const float ZOOM_NAME_THRESHOLD = 115f;

	// Token: 0x040075F4 RID: 30196
	private float m_currentZoomScale = 75f;

	// Token: 0x040075F5 RID: 30197
	private float m_targetZoomScale = 75f;

	// Token: 0x040075F6 RID: 30198
	private ClusterMapPath m_previewMapPath;

	// Token: 0x040075F7 RID: 30199
	private Dictionary<ClusterGridEntity, ClusterMapVisualizer> m_gridEntityVis = new Dictionary<ClusterGridEntity, ClusterMapVisualizer>();

	// Token: 0x040075F8 RID: 30200
	private Dictionary<ClusterGridEntity, ClusterMapVisualizer> m_gridEntityAnims = new Dictionary<ClusterGridEntity, ClusterMapVisualizer>();

	// Token: 0x040075F9 RID: 30201
	private Dictionary<AxialI, ClusterMapVisualizer> m_cellVisByLocation = new Dictionary<AxialI, ClusterMapVisualizer>();

	// Token: 0x040075FA RID: 30202
	private Action<object> m_onDestinationChangedDelegate;

	// Token: 0x040075FB RID: 30203
	private Action<object> m_onSelectObjectDelegate;

	// Token: 0x040075FC RID: 30204
	[SerializeField]
	private KScrollRect mapScrollRect;

	// Token: 0x040075FD RID: 30205
	[SerializeField]
	private float scrollSpeed = 15f;

	// Token: 0x040075FE RID: 30206
	public GameObject selectMarkerPrefab;

	// Token: 0x040075FF RID: 30207
	public ClusterMapPathDrawer pathDrawer;

	// Token: 0x04007600 RID: 30208
	private SelectMarker m_selectMarker;

	// Token: 0x04007601 RID: 30209
	private bool movingToTargetNISPosition;

	// Token: 0x04007602 RID: 30210
	private Vector3 targetNISPosition;

	// Token: 0x04007603 RID: 30211
	private float targetNISZoom;

	// Token: 0x04007604 RID: 30212
	private AxialI selectOnMoveNISComplete;

	// Token: 0x04007605 RID: 30213
	private ClusterMapScreen.Mode m_mode;

	// Token: 0x04007606 RID: 30214
	private ClusterDestinationSelector m_destinationSelector;

	// Token: 0x04007607 RID: 30215
	private bool m_closeOnSelect;

	// Token: 0x04007608 RID: 30216
	private Coroutine activeMoveToTargetRoutine;

	// Token: 0x04007609 RID: 30217
	public float floatCycleScale = 4f;

	// Token: 0x0400760A RID: 30218
	public float floatCycleOffset = 0.75f;

	// Token: 0x0400760B RID: 30219
	public float floatCycleSpeed = 0.75f;

	// Token: 0x02001C73 RID: 7283
	public enum Mode
	{
		// Token: 0x0400760D RID: 30221
		Default,
		// Token: 0x0400760E RID: 30222
		SelectDestination
	}
}
