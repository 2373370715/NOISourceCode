using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02001F25 RID: 7973
public class ResearchScreen : KModalScreen
{
	// Token: 0x0600A7BF RID: 42943 RVA: 0x00111544 File Offset: 0x0010F744
	public bool IsBeingResearched(Tech tech)
	{
		return Research.Instance.IsBeingResearched(tech);
	}

	// Token: 0x0600A7C0 RID: 42944 RVA: 0x00107159 File Offset: 0x00105359
	public override float GetSortKey()
	{
		if (base.isEditing)
		{
			return 50f;
		}
		return 20f;
	}

	// Token: 0x0600A7C1 RID: 42945 RVA: 0x00406B24 File Offset: 0x00404D24
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.ConsumeMouseScroll = true;
		Transform transform = base.transform;
		while (this.m_Raycaster == null)
		{
			this.m_Raycaster = transform.GetComponent<GraphicRaycaster>();
			if (this.m_Raycaster == null)
			{
				transform = transform.parent;
			}
		}
	}

	// Token: 0x0600A7C2 RID: 42946 RVA: 0x00111551 File Offset: 0x0010F751
	private void ZoomOut()
	{
		this.targetZoom = Mathf.Clamp(this.targetZoom - this.zoomAmountPerButton, this.minZoom, this.maxZoom);
		this.zoomCenterLock = true;
	}

	// Token: 0x0600A7C3 RID: 42947 RVA: 0x0011157E File Offset: 0x0010F77E
	private void ZoomIn()
	{
		this.targetZoom = Mathf.Clamp(this.targetZoom + this.zoomAmountPerButton, this.minZoom, this.maxZoom);
		this.zoomCenterLock = true;
	}

	// Token: 0x0600A7C4 RID: 42948 RVA: 0x00406B78 File Offset: 0x00404D78
	public void ZoomToTech(string techID, bool highlight = false)
	{
		Vector2 a = this.entryMap[Db.Get().Techs.Get(techID)].rectTransform().GetLocalPosition() + new Vector2(-this.foreground.rectTransform().rect.size.x / 2f, this.foreground.rectTransform().rect.size.y / 2f);
		this.forceTargetPosition = -a;
		this.zoomingToTarget = true;
		this.targetZoom = this.maxZoom;
		if (highlight)
		{
			this.sideBar.SetSearch(Db.Get().Techs.Get(techID).Name);
		}
	}

	// Token: 0x0600A7C5 RID: 42949 RVA: 0x00406C44 File Offset: 0x00404E44
	private void Update()
	{
		if (!base.canvas.enabled)
		{
			return;
		}
		RectTransform component = this.scrollContent.GetComponent<RectTransform>();
		if (this.isDragging && !KInputManager.isFocused)
		{
			this.AbortDragging();
		}
		Vector2 anchoredPosition = component.anchoredPosition;
		float t = Mathf.Min(this.effectiveZoomSpeed * Time.unscaledDeltaTime, 0.9f);
		this.currentZoom = Mathf.Lerp(this.currentZoom, this.targetZoom, t);
		Vector2 b = Vector2.zero;
		Vector2 v = KInputManager.GetMousePos();
		Vector2 b2 = this.zoomCenterLock ? (component.InverseTransformPoint(new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2))) * this.currentZoom) : (component.InverseTransformPoint(v) * this.currentZoom);
		component.localScale = new Vector3(this.currentZoom, this.currentZoom, 1f);
		b = (this.zoomCenterLock ? (component.InverseTransformPoint(new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2))) * this.currentZoom) : (component.InverseTransformPoint(v) * this.currentZoom)) - b2;
		float d = this.keyboardScrollSpeed;
		if (this.panUp)
		{
			this.keyPanDelta -= Vector2.up * Time.unscaledDeltaTime * d;
		}
		else if (this.panDown)
		{
			this.keyPanDelta += Vector2.up * Time.unscaledDeltaTime * d;
		}
		if (this.panLeft)
		{
			this.keyPanDelta += Vector2.right * Time.unscaledDeltaTime * d;
		}
		else if (this.panRight)
		{
			this.keyPanDelta -= Vector2.right * Time.unscaledDeltaTime * d;
		}
		if (KInputManager.currentControllerIsGamepad)
		{
			Vector2 a = KInputManager.steamInputInterpreter.GetSteamCameraMovement();
			a *= -1f;
			this.keyPanDelta = a * Time.unscaledDeltaTime * d * 2f;
		}
		Vector2 b3 = new Vector2(Mathf.Lerp(0f, this.keyPanDelta.x, Time.unscaledDeltaTime * this.keyPanEasing), Mathf.Lerp(0f, this.keyPanDelta.y, Time.unscaledDeltaTime * this.keyPanEasing));
		this.keyPanDelta -= b3;
		Vector2 vector = Vector2.zero;
		if (this.isDragging)
		{
			Vector2 b4 = KInputManager.GetMousePos() - this.dragLastPosition;
			vector += b4;
			this.dragLastPosition = KInputManager.GetMousePos();
			this.dragInteria = Vector2.ClampMagnitude(this.dragInteria + b4, 400f);
		}
		this.dragInteria *= Mathf.Max(0f, 1f - Time.unscaledDeltaTime * 4f);
		Vector2 vector2 = anchoredPosition + b + this.keyPanDelta + vector;
		if (!this.isDragging)
		{
			Vector2 size = base.GetComponent<RectTransform>().rect.size;
			Vector2 vector3 = new Vector2((-component.rect.size.x / 2f - 250f) * this.currentZoom, -250f * this.currentZoom);
			Vector2 vector4 = new Vector2(250f * this.currentZoom, (component.rect.size.y + 250f) * this.currentZoom - size.y);
			Vector2 a2 = new Vector2(Mathf.Clamp(vector2.x, vector3.x, vector4.x), Mathf.Clamp(vector2.y, vector3.y, vector4.y));
			this.forceTargetPosition = new Vector2(Mathf.Clamp(this.forceTargetPosition.x, vector3.x, vector4.x), Mathf.Clamp(this.forceTargetPosition.y, vector3.y, vector4.y));
			Vector2 vector5 = a2 + this.dragInteria - vector2;
			if (!this.panLeft && !this.panRight && !this.panUp && !this.panDown)
			{
				vector2 += vector5 * this.edgeClampFactor * Time.unscaledDeltaTime;
			}
			else
			{
				vector2 += vector5;
				if (vector5.x < 0f)
				{
					this.keyPanDelta.x = Mathf.Min(0f, this.keyPanDelta.x);
				}
				if (vector5.x > 0f)
				{
					this.keyPanDelta.x = Mathf.Max(0f, this.keyPanDelta.x);
				}
				if (vector5.y < 0f)
				{
					this.keyPanDelta.y = Mathf.Min(0f, this.keyPanDelta.y);
				}
				if (vector5.y > 0f)
				{
					this.keyPanDelta.y = Mathf.Max(0f, this.keyPanDelta.y);
				}
			}
		}
		if (this.zoomingToTarget)
		{
			vector2 = Vector2.Lerp(vector2, this.forceTargetPosition, Time.unscaledDeltaTime * 4f);
			if (Vector3.Distance(vector2, this.forceTargetPosition) < 1f || this.isDragging || this.panLeft || this.panRight || this.panUp || this.panDown)
			{
				this.zoomingToTarget = false;
			}
		}
		component.anchoredPosition = vector2;
	}

	// Token: 0x0600A7C6 RID: 42950 RVA: 0x00407240 File Offset: 0x00405440
	protected override void OnSpawn()
	{
		base.Subscribe(Research.Instance.gameObject, -1914338957, new Action<object>(this.OnActiveResearchChanged));
		base.Subscribe(Game.Instance.gameObject, -107300940, new Action<object>(this.OnResearchComplete));
		base.Subscribe(Game.Instance.gameObject, -1974454597, delegate(object o)
		{
			this.Show(false);
		});
		this.pointDisplayMap = new Dictionary<string, LocText>();
		foreach (ResearchType researchType in Research.Instance.researchTypes.Types)
		{
			this.pointDisplayMap[researchType.id] = Util.KInstantiateUI(this.pointDisplayCountPrefab, this.pointDisplayContainer, true).GetComponentInChildren<LocText>();
			this.pointDisplayMap[researchType.id].text = Research.Instance.globalPointInventory.PointsByTypeID[researchType.id].ToString();
			this.pointDisplayMap[researchType.id].transform.parent.GetComponent<ToolTip>().SetSimpleTooltip(researchType.description);
			this.pointDisplayMap[researchType.id].transform.parent.GetComponentInChildren<Image>().sprite = researchType.sprite;
		}
		this.pointDisplayContainer.transform.parent.gameObject.SetActive(Research.Instance.UseGlobalPointInventory);
		this.entryMap = new Dictionary<Tech, ResearchEntry>();
		List<Tech> resources = Db.Get().Techs.resources;
		resources.Sort((Tech x, Tech y) => y.center.y.CompareTo(x.center.y));
		List<TechTreeTitle> resources2 = Db.Get().TechTreeTitles.resources;
		resources2.Sort((TechTreeTitle x, TechTreeTitle y) => y.center.y.CompareTo(x.center.y));
		float x3 = 0f;
		float y3 = 125f;
		Vector2 b = new Vector2(x3, y3);
		for (int i = 0; i < resources2.Count; i++)
		{
			ResearchTreeTitle researchTreeTitle = Util.KInstantiateUI<ResearchTreeTitle>(this.researchTreeTitlePrefab.gameObject, this.treeTitles, false);
			TechTreeTitle techTreeTitle = resources2[i];
			researchTreeTitle.name = techTreeTitle.Name + " Title";
			Vector3 vector = techTreeTitle.center + b;
			researchTreeTitle.transform.rectTransform().anchoredPosition = vector;
			float num = techTreeTitle.height;
			if (i + 1 < resources2.Count)
			{
				TechTreeTitle techTreeTitle2 = resources2[i + 1];
				Vector3 vector2 = techTreeTitle2.center + b;
				num += vector.y - (vector2.y + techTreeTitle2.height);
			}
			else
			{
				num += 600f;
			}
			researchTreeTitle.transform.rectTransform().sizeDelta = new Vector2(techTreeTitle.width, num);
			researchTreeTitle.SetLabel(techTreeTitle.Name);
			researchTreeTitle.SetColor(i);
		}
		List<Vector2> list = new List<Vector2>();
		float x2 = 0f;
		float y2 = 0f;
		Vector2 b2 = new Vector2(x2, y2);
		for (int j = 0; j < resources.Count; j++)
		{
			ResearchEntry researchEntry = Util.KInstantiateUI<ResearchEntry>(this.entryPrefab.gameObject, this.scrollContent, false);
			Tech tech = resources[j];
			researchEntry.name = tech.Name + " Panel";
			Vector3 v = tech.center + b2;
			researchEntry.transform.rectTransform().anchoredPosition = v;
			researchEntry.transform.rectTransform().sizeDelta = new Vector2(tech.width, tech.height);
			this.entryMap.Add(tech, researchEntry);
			if (tech.edges.Count > 0)
			{
				for (int k = 0; k < tech.edges.Count; k++)
				{
					ResourceTreeNode.Edge edge = tech.edges[k];
					if (edge.path == null)
					{
						list.AddRange(edge.SrcTarget);
					}
					else
					{
						ResourceTreeNode.Edge.EdgeType edgeType = edge.edgeType;
						if (edgeType <= ResourceTreeNode.Edge.EdgeType.QuadCurveEdge || edgeType - ResourceTreeNode.Edge.EdgeType.BezierEdge <= 1)
						{
							list.Add(edge.SrcTarget[0]);
							list.Add(edge.path[0]);
							for (int l = 1; l < edge.path.Count; l++)
							{
								list.Add(edge.path[l - 1]);
								list.Add(edge.path[l]);
							}
							list.Add(edge.path[edge.path.Count - 1]);
							list.Add(edge.SrcTarget[1]);
						}
						else
						{
							list.AddRange(edge.path);
						}
					}
				}
			}
		}
		for (int m = 0; m < list.Count; m++)
		{
			list[m] = new Vector2(list[m].x, list[m].y + this.foreground.transform.rectTransform().rect.height);
		}
		foreach (KeyValuePair<Tech, ResearchEntry> keyValuePair in this.entryMap)
		{
			keyValuePair.Value.SetTech(keyValuePair.Key);
		}
		this.CloseButton.soundPlayer.Enabled = false;
		this.CloseButton.onClick += delegate()
		{
			ManagementMenu.Instance.CloseAll();
		};
		base.StartCoroutine(this.WaitAndSetActiveResearch());
		base.OnSpawn();
		this.scrollContent.GetComponent<RectTransform>().anchoredPosition = new Vector2(250f, -250f);
		this.zoomOutButton.onClick += delegate()
		{
			this.ZoomOut();
		};
		this.zoomInButton.onClick += delegate()
		{
			this.ZoomIn();
		};
		base.gameObject.SetActive(true);
		this.Show(false);
	}

	// Token: 0x0600A7C7 RID: 42951 RVA: 0x001115AB File Offset: 0x0010F7AB
	public override void OnBeginDrag(PointerEventData eventData)
	{
		base.OnBeginDrag(eventData);
		this.isDragging = true;
	}

	// Token: 0x0600A7C8 RID: 42952 RVA: 0x001115BB File Offset: 0x0010F7BB
	public override void OnEndDrag(PointerEventData eventData)
	{
		base.OnEndDrag(eventData);
		this.AbortDragging();
	}

	// Token: 0x0600A7C9 RID: 42953 RVA: 0x001115CA File Offset: 0x0010F7CA
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		base.Unsubscribe(Game.Instance.gameObject, -1974454597, delegate(object o)
		{
			this.Deactivate();
		});
	}

	// Token: 0x0600A7CA RID: 42954 RVA: 0x001115F3 File Offset: 0x0010F7F3
	private IEnumerator WaitAndSetActiveResearch()
	{
		yield return SequenceUtil.WaitForEndOfFrame;
		TechInstance targetResearch = Research.Instance.GetTargetResearch();
		if (targetResearch != null)
		{
			this.SetActiveResearch(targetResearch.tech);
		}
		yield break;
	}

	// Token: 0x0600A7CB RID: 42955 RVA: 0x00111602 File Offset: 0x0010F802
	public Vector3 GetEntryPosition(Tech tech)
	{
		if (!this.entryMap.ContainsKey(tech))
		{
			global::Debug.LogError("The Tech provided was not present in the dictionary");
			return Vector3.zero;
		}
		return this.entryMap[tech].transform.GetPosition();
	}

	// Token: 0x0600A7CC RID: 42956 RVA: 0x00111638 File Offset: 0x0010F838
	public ResearchEntry GetEntry(Tech tech)
	{
		if (this.entryMap == null)
		{
			return null;
		}
		if (!this.entryMap.ContainsKey(tech))
		{
			global::Debug.LogError("The Tech provided was not present in the dictionary");
			return null;
		}
		return this.entryMap[tech];
	}

	// Token: 0x0600A7CD RID: 42957 RVA: 0x004078E8 File Offset: 0x00405AE8
	public void SetEntryPercentage(Tech tech, float percent)
	{
		ResearchEntry entry = this.GetEntry(tech);
		if (entry != null)
		{
			entry.SetPercentage(percent);
		}
	}

	// Token: 0x0600A7CE RID: 42958 RVA: 0x00407910 File Offset: 0x00405B10
	public void TurnEverythingOff()
	{
		foreach (KeyValuePair<Tech, ResearchEntry> keyValuePair in this.entryMap)
		{
			keyValuePair.Value.SetEverythingOff();
		}
	}

	// Token: 0x0600A7CF RID: 42959 RVA: 0x00407968 File Offset: 0x00405B68
	public void TurnEverythingOn()
	{
		foreach (KeyValuePair<Tech, ResearchEntry> keyValuePair in this.entryMap)
		{
			keyValuePair.Value.SetEverythingOn();
		}
	}

	// Token: 0x0600A7D0 RID: 42960 RVA: 0x004079C0 File Offset: 0x00405BC0
	private void SelectAllEntries(Tech tech, bool isSelected)
	{
		ResearchEntry entry = this.GetEntry(tech);
		if (entry != null)
		{
			entry.QueueStateChanged(isSelected);
		}
		foreach (Tech tech2 in tech.requiredTech)
		{
			this.SelectAllEntries(tech2, isSelected);
		}
	}

	// Token: 0x0600A7D1 RID: 42961 RVA: 0x00407A2C File Offset: 0x00405C2C
	private void OnResearchComplete(object data)
	{
		if (data is Tech)
		{
			Tech tech = (Tech)data;
			ResearchEntry entry = this.GetEntry(tech);
			if (entry != null)
			{
				entry.ResearchCompleted(true);
			}
			this.UpdateProgressBars();
			this.UpdatePointDisplay();
		}
	}

	// Token: 0x0600A7D2 RID: 42962 RVA: 0x00407A6C File Offset: 0x00405C6C
	private void UpdatePointDisplay()
	{
		foreach (ResearchType researchType in Research.Instance.researchTypes.Types)
		{
			this.pointDisplayMap[researchType.id].text = string.Format("{0}: {1}", Research.Instance.researchTypes.GetResearchType(researchType.id).name, Research.Instance.globalPointInventory.PointsByTypeID[researchType.id].ToString());
		}
	}

	// Token: 0x0600A7D3 RID: 42963 RVA: 0x00407B20 File Offset: 0x00405D20
	private void OnActiveResearchChanged(object data)
	{
		List<TechInstance> list = (List<TechInstance>)data;
		foreach (TechInstance techInstance in list)
		{
			ResearchEntry entry = this.GetEntry(techInstance.tech);
			if (entry != null)
			{
				entry.QueueStateChanged(true);
			}
		}
		this.UpdateProgressBars();
		this.UpdatePointDisplay();
		if (list.Count > 0)
		{
			this.currentResearch = list[list.Count - 1].tech;
		}
	}

	// Token: 0x0600A7D4 RID: 42964 RVA: 0x00407BBC File Offset: 0x00405DBC
	private void UpdateProgressBars()
	{
		foreach (KeyValuePair<Tech, ResearchEntry> keyValuePair in this.entryMap)
		{
			keyValuePair.Value.UpdateProgressBars();
		}
	}

	// Token: 0x0600A7D5 RID: 42965 RVA: 0x00407C14 File Offset: 0x00405E14
	public void CancelResearch()
	{
		List<TechInstance> researchQueue = Research.Instance.GetResearchQueue();
		foreach (TechInstance techInstance in researchQueue)
		{
			ResearchEntry entry = this.GetEntry(techInstance.tech);
			if (entry != null)
			{
				entry.QueueStateChanged(false);
			}
		}
		researchQueue.Clear();
	}

	// Token: 0x0600A7D6 RID: 42966 RVA: 0x0011166A File Offset: 0x0010F86A
	private void SetActiveResearch(Tech newResearch)
	{
		if (newResearch != this.currentResearch && this.currentResearch != null)
		{
			this.SelectAllEntries(this.currentResearch, false);
		}
		this.currentResearch = newResearch;
		if (this.currentResearch != null)
		{
			this.SelectAllEntries(this.currentResearch, true);
		}
	}

	// Token: 0x0600A7D7 RID: 42967 RVA: 0x00407C8C File Offset: 0x00405E8C
	public override void Show(bool show = true)
	{
		this.mouseOver = false;
		this.scrollContentChildFitter.enabled = show;
		foreach (Canvas canvas in base.GetComponentsInChildren<Canvas>(true))
		{
			if (canvas.enabled != show)
			{
				canvas.enabled = show;
			}
		}
		CanvasGroup component = base.GetComponent<CanvasGroup>();
		if (component != null)
		{
			component.interactable = show;
			component.blocksRaycasts = show;
			component.ignoreParentGroups = true;
		}
		this.OnShow(show);
	}

	// Token: 0x0600A7D8 RID: 42968 RVA: 0x00407D04 File Offset: 0x00405F04
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (show)
		{
			this.sideBar.ResetFilter();
		}
		if (show)
		{
			CameraController.Instance.DisableUserCameraControl = true;
			if (DetailsScreen.Instance != null)
			{
				DetailsScreen.Instance.gameObject.SetActive(false);
			}
		}
		else
		{
			CameraController.Instance.DisableUserCameraControl = false;
			if (SelectTool.Instance.selected != null && !DetailsScreen.Instance.gameObject.activeSelf)
			{
				DetailsScreen.Instance.gameObject.SetActive(true);
				DetailsScreen.Instance.Refresh(SelectTool.Instance.selected.gameObject);
			}
		}
		this.UpdateProgressBars();
		this.UpdatePointDisplay();
	}

	// Token: 0x0600A7D9 RID: 42969 RVA: 0x001116A6 File Offset: 0x0010F8A6
	private void AbortDragging()
	{
		this.isDragging = false;
		this.draggingJustEnded = true;
	}

	// Token: 0x0600A7DA RID: 42970 RVA: 0x001116B6 File Offset: 0x0010F8B6
	private void LateUpdate()
	{
		this.draggingJustEnded = false;
	}

	// Token: 0x0600A7DB RID: 42971 RVA: 0x00407DB8 File Offset: 0x00405FB8
	public override void OnKeyUp(KButtonEvent e)
	{
		if (!base.canvas.enabled)
		{
			return;
		}
		if (!e.Consumed)
		{
			if (e.IsAction(global::Action.MouseRight) && !this.isDragging && !this.draggingJustEnded)
			{
				ManagementMenu.Instance.CloseAll();
			}
			if (e.IsAction(global::Action.MouseRight) || e.IsAction(global::Action.MouseLeft) || e.IsAction(global::Action.MouseMiddle))
			{
				this.AbortDragging();
			}
			if (this.panUp && e.TryConsume(global::Action.PanUp))
			{
				this.panUp = false;
				return;
			}
			if (this.panDown && e.TryConsume(global::Action.PanDown))
			{
				this.panDown = false;
				return;
			}
			if (this.panRight && e.TryConsume(global::Action.PanRight))
			{
				this.panRight = false;
				return;
			}
			if (this.panLeft && e.TryConsume(global::Action.PanLeft))
			{
				this.panLeft = false;
				return;
			}
		}
		base.OnKeyUp(e);
	}

	// Token: 0x0600A7DC RID: 42972 RVA: 0x00407EA0 File Offset: 0x004060A0
	public override void OnKeyDown(KButtonEvent e)
	{
		if (!base.canvas.enabled)
		{
			return;
		}
		if (!e.Consumed)
		{
			if (e.TryConsume(global::Action.MouseRight))
			{
				this.dragStartPosition = KInputManager.GetMousePos();
				this.dragLastPosition = KInputManager.GetMousePos();
				return;
			}
			if (e.TryConsume(global::Action.MouseLeft))
			{
				this.dragStartPosition = KInputManager.GetMousePos();
				this.dragLastPosition = KInputManager.GetMousePos();
				return;
			}
			if (KInputManager.GetMousePos().x > this.sideBar.rectTransform().sizeDelta.x && CameraController.IsMouseOverGameWindow)
			{
				if (e.TryConsume(global::Action.ZoomIn))
				{
					this.targetZoom = Mathf.Clamp(this.targetZoom + this.zoomAmountPerScroll, this.minZoom, this.maxZoom);
					this.zoomCenterLock = false;
					return;
				}
				if (e.TryConsume(global::Action.ZoomOut))
				{
					this.targetZoom = Mathf.Clamp(this.targetZoom - this.zoomAmountPerScroll, this.minZoom, this.maxZoom);
					this.zoomCenterLock = false;
					return;
				}
			}
			if (e.TryConsume(global::Action.Escape))
			{
				ManagementMenu.Instance.CloseAll();
				return;
			}
			if (e.TryConsume(global::Action.PanLeft))
			{
				this.panLeft = true;
				return;
			}
			if (e.TryConsume(global::Action.PanRight))
			{
				this.panRight = true;
				return;
			}
			if (e.TryConsume(global::Action.PanUp))
			{
				this.panUp = true;
				return;
			}
			if (e.TryConsume(global::Action.PanDown))
			{
				this.panDown = true;
				return;
			}
		}
		base.OnKeyDown(e);
	}

	// Token: 0x040083C5 RID: 33733
	private const float SCROLL_BUFFER = 250f;

	// Token: 0x040083C6 RID: 33734
	[SerializeField]
	private Image BG;

	// Token: 0x040083C7 RID: 33735
	public ResearchEntry entryPrefab;

	// Token: 0x040083C8 RID: 33736
	public ResearchTreeTitle researchTreeTitlePrefab;

	// Token: 0x040083C9 RID: 33737
	public GameObject foreground;

	// Token: 0x040083CA RID: 33738
	public GameObject scrollContent;

	// Token: 0x040083CB RID: 33739
	public GameObject treeTitles;

	// Token: 0x040083CC RID: 33740
	public GameObject pointDisplayCountPrefab;

	// Token: 0x040083CD RID: 33741
	public GameObject pointDisplayContainer;

	// Token: 0x040083CE RID: 33742
	private Dictionary<string, LocText> pointDisplayMap;

	// Token: 0x040083CF RID: 33743
	private Dictionary<Tech, ResearchEntry> entryMap;

	// Token: 0x040083D0 RID: 33744
	[SerializeField]
	private KButton zoomOutButton;

	// Token: 0x040083D1 RID: 33745
	[SerializeField]
	private KButton zoomInButton;

	// Token: 0x040083D2 RID: 33746
	[SerializeField]
	private ResearchScreenSideBar sideBar;

	// Token: 0x040083D3 RID: 33747
	private Tech currentResearch;

	// Token: 0x040083D4 RID: 33748
	public KButton CloseButton;

	// Token: 0x040083D5 RID: 33749
	private GraphicRaycaster m_Raycaster;

	// Token: 0x040083D6 RID: 33750
	private PointerEventData m_PointerEventData;

	// Token: 0x040083D7 RID: 33751
	private Vector3 currentScrollPosition;

	// Token: 0x040083D8 RID: 33752
	private bool panUp;

	// Token: 0x040083D9 RID: 33753
	private bool panDown;

	// Token: 0x040083DA RID: 33754
	private bool panLeft;

	// Token: 0x040083DB RID: 33755
	private bool panRight;

	// Token: 0x040083DC RID: 33756
	[SerializeField]
	private KChildFitter scrollContentChildFitter;

	// Token: 0x040083DD RID: 33757
	private bool isDragging;

	// Token: 0x040083DE RID: 33758
	private Vector3 dragStartPosition;

	// Token: 0x040083DF RID: 33759
	private Vector3 dragLastPosition;

	// Token: 0x040083E0 RID: 33760
	private Vector2 dragInteria;

	// Token: 0x040083E1 RID: 33761
	private Vector2 forceTargetPosition;

	// Token: 0x040083E2 RID: 33762
	private bool zoomingToTarget;

	// Token: 0x040083E3 RID: 33763
	private bool draggingJustEnded;

	// Token: 0x040083E4 RID: 33764
	private float targetZoom = 1f;

	// Token: 0x040083E5 RID: 33765
	private float currentZoom = 1f;

	// Token: 0x040083E6 RID: 33766
	private bool zoomCenterLock;

	// Token: 0x040083E7 RID: 33767
	private Vector2 keyPanDelta = Vector3.zero;

	// Token: 0x040083E8 RID: 33768
	[SerializeField]
	private float effectiveZoomSpeed = 5f;

	// Token: 0x040083E9 RID: 33769
	[SerializeField]
	private float zoomAmountPerScroll = 0.05f;

	// Token: 0x040083EA RID: 33770
	[SerializeField]
	private float zoomAmountPerButton = 0.5f;

	// Token: 0x040083EB RID: 33771
	[SerializeField]
	private float minZoom = 0.15f;

	// Token: 0x040083EC RID: 33772
	[SerializeField]
	private float maxZoom = 1f;

	// Token: 0x040083ED RID: 33773
	[SerializeField]
	private float keyboardScrollSpeed = 200f;

	// Token: 0x040083EE RID: 33774
	[SerializeField]
	private float keyPanEasing = 1f;

	// Token: 0x040083EF RID: 33775
	[SerializeField]
	private float edgeClampFactor = 0.5f;

	// Token: 0x02001F26 RID: 7974
	public enum ResearchState
	{
		// Token: 0x040083F1 RID: 33777
		Available,
		// Token: 0x040083F2 RID: 33778
		ActiveResearch,
		// Token: 0x040083F3 RID: 33779
		ResearchComplete,
		// Token: 0x040083F4 RID: 33780
		MissingPrerequisites,
		// Token: 0x040083F5 RID: 33781
		StateCount
	}
}
