using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.UI;

// Token: 0x02001BA2 RID: 7074
public class ToolMenu : KScreen
{
	// Token: 0x060094A3 RID: 38051 RVA: 0x00105851 File Offset: 0x00103A51
	public static void DestroyInstance()
	{
		ToolMenu.Instance = null;
	}

	// Token: 0x170009B3 RID: 2483
	// (get) Token: 0x060094A4 RID: 38052 RVA: 0x00105859 File Offset: 0x00103A59
	public PriorityScreen PriorityScreen
	{
		get
		{
			return this.priorityScreen;
		}
	}

	// Token: 0x060094A5 RID: 38053 RVA: 0x00105861 File Offset: 0x00103A61
	public override float GetSortKey()
	{
		return 5f;
	}

	// Token: 0x060094A6 RID: 38054 RVA: 0x003A0654 File Offset: 0x0039E854
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		ToolMenu.Instance = this;
		Game.Instance.Subscribe(1798162660, new Action<object>(this.OnOverlayChanged));
		this.priorityScreen = Util.KInstantiateUI<PriorityScreen>(this.Prefab_priorityScreen.gameObject, base.gameObject, false);
		this.priorityScreen.InstantiateButtons(new Action<PrioritySetting>(this.OnPriorityClicked), false);
	}

	// Token: 0x060094A7 RID: 38055 RVA: 0x00105868 File Offset: 0x00103A68
	protected override void OnForcedCleanUp()
	{
		KInputManager.InputChange.RemoveListener(new UnityAction(this.OnInputChange));
		base.OnForcedCleanUp();
	}

	// Token: 0x060094A8 RID: 38056 RVA: 0x00105886 File Offset: 0x00103A86
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Game.Instance.Unsubscribe(1798162660, new Action<object>(this.OnOverlayChanged));
		Game.Instance.Unsubscribe(this.refreshScaleHandle);
	}

	// Token: 0x060094A9 RID: 38057 RVA: 0x003A06C0 File Offset: 0x0039E8C0
	private void OnOverlayChanged(object overlay_data)
	{
		HashedString y = (HashedString)overlay_data;
		if (PlayerController.Instance.ActiveTool != null && PlayerController.Instance.ActiveTool.ViewMode != OverlayModes.None.ID && PlayerController.Instance.ActiveTool.ViewMode != y)
		{
			this.ChooseCollection(null, true);
			this.ChooseTool(null);
		}
	}

	// Token: 0x060094AA RID: 38058 RVA: 0x003A0728 File Offset: 0x0039E928
	protected override void OnSpawn()
	{
		this.activateOnSpawn = true;
		base.OnSpawn();
		this.CreateSandBoxTools();
		this.CreateBasicTools();
		this.rows.Add(this.sandboxTools);
		this.rows.Add(this.basicTools);
		this.rows.ForEach(delegate(List<ToolMenu.ToolCollection> row)
		{
			this.InstantiateCollectionsUI(row);
		});
		this.rows.ForEach(delegate(List<ToolMenu.ToolCollection> row)
		{
			this.BuildRowToggles(row);
		});
		this.rows.ForEach(delegate(List<ToolMenu.ToolCollection> row)
		{
			this.BuildToolToggles(row);
		});
		this.ChooseCollection(null, true);
		this.priorityScreen.gameObject.SetActive(false);
		this.ToggleSandboxUI(null);
		KInputManager.InputChange.AddListener(new UnityAction(this.OnInputChange));
		Game.Instance.Subscribe(-1948169901, new Action<object>(this.ToggleSandboxUI));
		this.ResetToolDisplayPlane();
		this.refreshScaleHandle = Game.Instance.Subscribe(-442024484, new Action<object>(this.RefreshScale));
		this.RefreshScale(null);
	}

	// Token: 0x060094AB RID: 38059 RVA: 0x003A0838 File Offset: 0x0039EA38
	private void RefreshScale(object data = null)
	{
		int num = 14;
		int num2 = 16;
		foreach (ToolMenu.ToolCollection toolCollection in this.sandboxTools)
		{
			LocText componentInChildren = toolCollection.toggle.GetComponentInChildren<LocText>();
			if (componentInChildren != null)
			{
				componentInChildren.fontSize = (float)(ScreenResolutionMonitor.UsingGamepadUIMode() ? num2 : num);
			}
		}
		foreach (ToolMenu.ToolCollection toolCollection2 in this.basicTools)
		{
			LocText componentInChildren2 = toolCollection2.toggle.GetComponentInChildren<LocText>();
			if (componentInChildren2 != null)
			{
				componentInChildren2.fontSize = (float)(ScreenResolutionMonitor.UsingGamepadUIMode() ? num2 : num);
			}
		}
	}

	// Token: 0x060094AC RID: 38060 RVA: 0x001058B9 File Offset: 0x00103AB9
	public void OnInputChange()
	{
		this.rows.ForEach(delegate(List<ToolMenu.ToolCollection> row)
		{
			this.BuildRowToggles(row);
		});
		this.rows.ForEach(delegate(List<ToolMenu.ToolCollection> row)
		{
			this.BuildToolToggles(row);
		});
	}

	// Token: 0x060094AD RID: 38061 RVA: 0x003A0914 File Offset: 0x0039EB14
	private void ResetToolDisplayPlane()
	{
		this.toolEffectDisplayPlane = this.CreateToolDisplayPlane("Overlay", World.Instance.transform);
		this.toolEffectDisplayPlaneTexture = this.CreatePlaneTexture(out this.toolEffectDisplayBytes, Grid.WidthInCells, Grid.HeightInCells);
		this.toolEffectDisplayPlane.GetComponent<Renderer>().sharedMaterial = this.toolEffectDisplayMaterial;
		this.toolEffectDisplayPlane.GetComponent<Renderer>().sharedMaterial.mainTexture = this.toolEffectDisplayPlaneTexture;
		this.toolEffectDisplayPlane.transform.SetLocalPosition(new Vector3(Grid.WidthInMeters / 2f, Grid.HeightInMeters / 2f, -6f));
		this.RefreshToolDisplayPlaneColor();
	}

	// Token: 0x060094AE RID: 38062 RVA: 0x003A09C0 File Offset: 0x0039EBC0
	private GameObject CreateToolDisplayPlane(string layer, Transform parent)
	{
		GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
		gameObject.name = "toolEffectDisplayPlane";
		gameObject.SetLayerRecursively(LayerMask.NameToLayer(layer));
		UnityEngine.Object.Destroy(gameObject.GetComponent<Collider>());
		if (parent != null)
		{
			gameObject.transform.SetParent(parent);
		}
		gameObject.transform.SetPosition(Vector3.zero);
		gameObject.transform.localScale = new Vector3(Grid.WidthInMeters / -10f, 1f, Grid.HeightInMeters / -10f);
		gameObject.transform.eulerAngles = new Vector3(270f, 0f, 0f);
		gameObject.GetComponent<MeshRenderer>().reflectionProbeUsage = ReflectionProbeUsage.Off;
		return gameObject;
	}

	// Token: 0x060094AF RID: 38063 RVA: 0x001058E9 File Offset: 0x00103AE9
	private Texture2D CreatePlaneTexture(out byte[] textureBytes, int width, int height)
	{
		textureBytes = new byte[width * height * 4];
		return new Texture2D(width, height, TextureUtil.TextureFormatToGraphicsFormat(TextureFormat.RGBA32), TextureCreationFlags.None)
		{
			name = "toolEffectDisplayPlane",
			wrapMode = TextureWrapMode.Clamp,
			filterMode = FilterMode.Point
		};
	}

	// Token: 0x060094B0 RID: 38064 RVA: 0x0010591E File Offset: 0x00103B1E
	private void Update()
	{
		this.RefreshToolDisplayPlaneColor();
	}

	// Token: 0x060094B1 RID: 38065 RVA: 0x003A0A74 File Offset: 0x0039EC74
	private void RefreshToolDisplayPlaneColor()
	{
		if (PlayerController.Instance.ActiveTool == null || PlayerController.Instance.ActiveTool == SelectTool.Instance)
		{
			this.toolEffectDisplayPlane.SetActive(false);
			return;
		}
		PlayerController.Instance.ActiveTool.GetOverlayColorData(out this.colors);
		Array.Clear(this.toolEffectDisplayBytes, 0, this.toolEffectDisplayBytes.Length);
		if (this.colors != null)
		{
			foreach (ToolMenu.CellColorData cellColorData in this.colors)
			{
				if (Grid.IsValidCell(cellColorData.cell))
				{
					int num = cellColorData.cell * 4;
					if (num >= 0)
					{
						this.toolEffectDisplayBytes[num] = (byte)(Mathf.Min(cellColorData.color.r, 1f) * 255f);
						this.toolEffectDisplayBytes[num + 1] = (byte)(Mathf.Min(cellColorData.color.g, 1f) * 255f);
						this.toolEffectDisplayBytes[num + 2] = (byte)(Mathf.Min(cellColorData.color.b, 1f) * 255f);
						this.toolEffectDisplayBytes[num + 3] = (byte)(Mathf.Min(cellColorData.color.a, 1f) * 255f);
					}
				}
			}
		}
		if (!this.toolEffectDisplayPlane.activeSelf)
		{
			this.toolEffectDisplayPlane.SetActive(true);
		}
		this.toolEffectDisplayPlaneTexture.LoadRawTextureData(this.toolEffectDisplayBytes);
		this.toolEffectDisplayPlaneTexture.Apply();
	}

	// Token: 0x060094B2 RID: 38066 RVA: 0x003A0C1C File Offset: 0x0039EE1C
	public void ToggleSandboxUI(object data = null)
	{
		this.ClearSelection();
		PlayerController.Instance.ActivateTool(SelectTool.Instance);
		this.sandboxTools[0].toggle.transform.parent.transform.parent.gameObject.SetActive(Game.Instance.SandboxModeActive);
	}

	// Token: 0x060094B3 RID: 38067 RVA: 0x003A0C78 File Offset: 0x0039EE78
	public static ToolMenu.ToolCollection CreateToolCollection(LocString collection_name, string icon_name, global::Action hotkey, string tool_name, LocString tooltip, bool largeIcon)
	{
		ToolMenu.ToolCollection toolCollection = new ToolMenu.ToolCollection(collection_name, icon_name, "", false, global::Action.NumActions, largeIcon);
		new ToolMenu.ToolInfo(collection_name, icon_name, hotkey, tool_name, toolCollection, tooltip, null, null);
		return toolCollection;
	}

	// Token: 0x060094B4 RID: 38068 RVA: 0x003A0CBC File Offset: 0x0039EEBC
	private void CreateSandBoxTools()
	{
		this.sandboxTools.Add(ToolMenu.CreateToolCollection(UI.TOOLS.SANDBOX.BRUSH.NAME, "brush", global::Action.SandboxBrush, "SandboxBrushTool", UI.SANDBOXTOOLS.SETTINGS.BRUSH.TOOLTIP, false));
		this.sandboxTools.Add(ToolMenu.CreateToolCollection(UI.TOOLS.SANDBOX.SPRINKLE.NAME, "sprinkle", global::Action.SandboxSprinkle, "SandboxSprinkleTool", UI.SANDBOXTOOLS.SETTINGS.SPRINKLE.TOOLTIP, false));
		this.sandboxTools.Add(ToolMenu.CreateToolCollection(UI.TOOLS.SANDBOX.FLOOD.NAME, "flood", global::Action.SandboxFlood, "SandboxFloodTool", UI.SANDBOXTOOLS.SETTINGS.FLOOD.TOOLTIP, false));
		this.sandboxTools.Add(ToolMenu.CreateToolCollection(UI.TOOLS.SANDBOX.SAMPLE.NAME, "sample", global::Action.SandboxSample, "SandboxSampleTool", UI.SANDBOXTOOLS.SETTINGS.SAMPLE.TOOLTIP, false));
		this.sandboxTools.Add(ToolMenu.CreateToolCollection(UI.TOOLS.SANDBOX.HEATGUN.NAME, "temperature", global::Action.SandboxHeatGun, "SandboxHeatTool", UI.SANDBOXTOOLS.SETTINGS.HEATGUN.TOOLTIP, false));
		this.sandboxTools.Add(ToolMenu.CreateToolCollection(UI.TOOLS.SANDBOX.STRESSTOOL.NAME, "crew_state_happy", global::Action.SandboxStressTool, "SandboxStressTool", UI.SANDBOXTOOLS.SETTINGS.STRESS.TOOLTIP, false));
		this.sandboxTools.Add(ToolMenu.CreateToolCollection(UI.TOOLS.SANDBOX.SPAWNER.NAME, "spawn", global::Action.SandboxSpawnEntity, "SandboxSpawnerTool", UI.SANDBOXTOOLS.SETTINGS.SPAWNER.TOOLTIP, false));
		this.sandboxTools.Add(ToolMenu.CreateToolCollection(UI.TOOLS.SANDBOX.CLEAR_FLOOR.NAME, "clear_floor", global::Action.SandboxClearFloor, "SandboxClearFloorTool", UI.SANDBOXTOOLS.SETTINGS.CLEAR_FLOOR.TOOLTIP, false));
		this.sandboxTools.Add(ToolMenu.CreateToolCollection(UI.TOOLS.SANDBOX.DESTROY.NAME, "destroy", global::Action.SandboxDestroy, "SandboxDestroyerTool", UI.SANDBOXTOOLS.SETTINGS.DESTROY.TOOLTIP, false));
		this.sandboxTools.Add(ToolMenu.CreateToolCollection(UI.TOOLS.SANDBOX.FOW.NAME, "reveal", global::Action.SandboxReveal, "SandboxFOWTool", UI.SANDBOXTOOLS.SETTINGS.FOW.TOOLTIP, false));
		this.sandboxTools.Add(ToolMenu.CreateToolCollection(UI.TOOLS.SANDBOX.CRITTER.NAME, "critter", global::Action.SandboxCritterTool, "SandboxCritterTool", UI.SANDBOXTOOLS.SETTINGS.CRITTER.TOOLTIP, false));
		this.sandboxTools.Add(ToolMenu.CreateToolCollection(UI.TOOLS.SANDBOX.SPAWN_STORY_TRAIT.NAME, "sandbox_storytrait", global::Action.SandboxStoryTraitTool, "SandboxStoryTraitTool", UI.SANDBOXTOOLS.SETTINGS.SPAWN_STORY_TRAIT.TOOLTIP, false));
	}

	// Token: 0x060094B5 RID: 38069 RVA: 0x003A0EC4 File Offset: 0x0039F0C4
	private void CreateBasicTools()
	{
		this.basicTools.Add(ToolMenu.CreateToolCollection(UI.TOOLS.DIG.NAME, "icon_action_dig", global::Action.Dig, "DigTool", UI.TOOLTIPS.DIGBUTTON, true));
		this.basicTools.Add(ToolMenu.CreateToolCollection(UI.TOOLS.CANCEL.NAME, "icon_action_cancel", global::Action.BuildingCancel, "CancelTool", UI.TOOLTIPS.CANCELBUTTON, true));
		this.basicTools.Add(ToolMenu.CreateToolCollection(UI.TOOLS.DECONSTRUCT.NAME, "icon_action_deconstruct", global::Action.BuildingDeconstruct, "DeconstructTool", UI.TOOLTIPS.DECONSTRUCTBUTTON, true));
		this.basicTools.Add(ToolMenu.CreateToolCollection(UI.TOOLS.PRIORITIZE.NAME, "icon_action_prioritize", global::Action.Prioritize, "PrioritizeTool", UI.TOOLTIPS.PRIORITIZEBUTTON, true));
		this.basicTools.Add(ToolMenu.CreateToolCollection(UI.TOOLS.DISINFECT.NAME, "icon_action_disinfect", global::Action.Disinfect, "DisinfectTool", UI.TOOLTIPS.DISINFECTBUTTON, false));
		this.basicTools.Add(ToolMenu.CreateToolCollection(UI.TOOLS.MARKFORSTORAGE.NAME, "icon_action_store", global::Action.Clear, "ClearTool", UI.TOOLTIPS.CLEARBUTTON, false));
		this.basicTools.Add(ToolMenu.CreateToolCollection(UI.TOOLS.ATTACK.NAME, "icon_action_attack", global::Action.Attack, "AttackTool", UI.TOOLTIPS.ATTACKBUTTON, false));
		this.basicTools.Add(ToolMenu.CreateToolCollection(UI.TOOLS.MOP.NAME, "icon_action_mop", global::Action.Mop, "MopTool", UI.TOOLTIPS.MOPBUTTON, false));
		this.basicTools.Add(ToolMenu.CreateToolCollection(UI.TOOLS.CAPTURE.NAME, "icon_action_capture", global::Action.Capture, "CaptureTool", UI.TOOLTIPS.CAPTUREBUTTON, false));
		this.basicTools.Add(ToolMenu.CreateToolCollection(UI.TOOLS.HARVEST.NAME, "icon_action_harvest", global::Action.Harvest, "HarvestTool", UI.TOOLTIPS.HARVESTBUTTON, false));
		this.basicTools.Add(ToolMenu.CreateToolCollection(UI.TOOLS.EMPTY_PIPE.NAME, "icon_action_empty_pipes", global::Action.EmptyPipe, "EmptyPipeTool", UI.TOOLS.EMPTY_PIPE.TOOLTIP, false));
		this.basicTools.Add(ToolMenu.CreateToolCollection(UI.TOOLS.DISCONNECT.NAME, "icon_action_disconnect", global::Action.Disconnect, "DisconnectTool", UI.TOOLS.DISCONNECT.TOOLTIP, false));
	}

	// Token: 0x060094B6 RID: 38070 RVA: 0x003A10CC File Offset: 0x0039F2CC
	private void InstantiateCollectionsUI(IList<ToolMenu.ToolCollection> collections)
	{
		GameObject parent = Util.KInstantiateUI(this.prefabToolRow, base.gameObject, true);
		GameObject gameObject = Util.KInstantiateUI(this.largeToolSet, parent, true);
		GameObject gameObject2 = Util.KInstantiateUI(this.smallToolSet, parent, true);
		GameObject gameObject3 = Util.KInstantiateUI(this.smallToolBottomRow, gameObject2, true);
		GameObject gameObject4 = Util.KInstantiateUI(this.smallToolTopRow, gameObject2, true);
		GameObject gameObject5 = Util.KInstantiateUI(this.sandboxToolSet, parent, true);
		bool flag = true;
		int num = 0;
		for (int i = 0; i < collections.Count; i++)
		{
			GameObject parent2;
			if (collections == this.sandboxTools)
			{
				parent2 = gameObject5;
			}
			else if (collections[i].largeIcon)
			{
				parent2 = gameObject;
			}
			else
			{
				parent2 = (flag ? gameObject4 : gameObject3);
				flag = !flag;
				num++;
			}
			ToolMenu.ToolCollection tc = collections[i];
			tc.toggle = Util.KInstantiateUI((collections[i].tools.Count > 1) ? this.collectionIconPrefab : ((collections == this.sandboxTools) ? this.sandboxToolIconPrefab : (collections[i].largeIcon ? this.toolIconLargePrefab : this.toolIconPrefab)), parent2, true);
			KToggle component = tc.toggle.GetComponent<KToggle>();
			component.soundPlayer.Enabled = false;
			component.onClick += delegate()
			{
				if (this.currentlySelectedCollection == tc && tc.tools.Count >= 1)
				{
					KMonoBehaviour.PlaySound(GlobalAssets.GetSound(PlayerController.Instance.ActiveTool.GetDeactivateSound(), false));
				}
				this.ChooseCollection(tc, true);
			};
			if (tc.tools != null)
			{
				GameObject gameObject6;
				if (tc.tools.Count < this.smallCollectionMax)
				{
					gameObject6 = Util.KInstantiateUI(this.Prefab_collectionContainer, parent2, true);
					gameObject6.transform.SetSiblingIndex(gameObject6.transform.GetSiblingIndex() - 1);
					gameObject6.transform.localScale = Vector3.one;
					gameObject6.rectTransform().sizeDelta = new Vector2((float)(tc.tools.Count * 75), 50f);
					tc.MaskContainer = gameObject6.GetComponentInChildren<Mask>().gameObject;
					gameObject6.SetActive(false);
				}
				else
				{
					gameObject6 = Util.KInstantiateUI(this.Prefab_collectionContainerWindow, parent2, true);
					gameObject6.transform.localScale = Vector3.one;
					gameObject6.GetComponentInChildren<LocText>().SetText(tc.text.ToUpper());
					tc.MaskContainer = gameObject6.GetComponentInChildren<GridLayoutGroup>().gameObject;
					gameObject6.SetActive(false);
				}
				tc.UIMenuDisplay = gameObject6;
				Action<object> <>9__2;
				for (int j = 0; j < tc.tools.Count; j++)
				{
					ToolMenu.ToolInfo ti = tc.tools[j];
					GameObject gameObject7 = Util.KInstantiateUI((collections == this.sandboxTools) ? this.sandboxToolIconPrefab : (collections[i].largeIcon ? this.toolIconLargePrefab : this.toolIconPrefab), tc.MaskContainer, true);
					gameObject7.name = ti.text;
					ti.toggle = gameObject7.GetComponent<KToggle>();
					if (ti.collection.tools.Count > 1)
					{
						RectTransform rectTransform = ti.toggle.gameObject.GetComponentInChildren<SetTextStyleSetting>().rectTransform();
						if (gameObject7.name.Length > 12)
						{
							rectTransform.GetComponent<SetTextStyleSetting>().SetStyle(this.CategoryLabelTextStyle_LeftAlign);
							rectTransform.anchoredPosition = new Vector2(16f, rectTransform.anchoredPosition.y);
						}
					}
					ti.toggle.onClick += delegate()
					{
						this.ChooseTool(ti);
					};
					ExpandRevealUIContent component2 = tc.UIMenuDisplay.GetComponent<ExpandRevealUIContent>();
					Action<object> completeCallback;
					if ((completeCallback = <>9__2) == null)
					{
						completeCallback = (<>9__2 = delegate(object s)
						{
							this.SetToggleState(tc.toggle.GetComponent<KToggle>(), false);
							tc.UIMenuDisplay.SetActive(false);
						});
					}
					component2.Collapse(completeCallback);
				}
			}
		}
		if (num > 0 && num % 2 == 0)
		{
			gameObject3.GetComponent<HorizontalLayoutGroup>().padding.left = 26;
			gameObject4.GetComponent<HorizontalLayoutGroup>().padding.right = 26;
		}
		if (gameObject.transform.childCount == 0)
		{
			UnityEngine.Object.Destroy(gameObject);
		}
		if (gameObject3.transform.childCount == 0 && gameObject4.transform.childCount == 0)
		{
			UnityEngine.Object.Destroy(gameObject2);
		}
		if (gameObject5.transform.childCount == 0)
		{
			UnityEngine.Object.Destroy(gameObject5);
		}
	}

	// Token: 0x060094B7 RID: 38071 RVA: 0x003A1588 File Offset: 0x0039F788
	private void ChooseTool(ToolMenu.ToolInfo tool)
	{
		if (this.currentlySelectedTool == tool)
		{
			return;
		}
		if (this.currentlySelectedTool != tool)
		{
			this.currentlySelectedTool = tool;
			if (this.currentlySelectedTool != null && this.currentlySelectedTool.onSelectCallback != null)
			{
				this.currentlySelectedTool.onSelectCallback(this.currentlySelectedTool);
			}
		}
		if (this.currentlySelectedTool != null)
		{
			this.currentlySelectedCollection = this.currentlySelectedTool.collection;
			foreach (InterfaceTool interfaceTool in PlayerController.Instance.tools)
			{
				if (this.currentlySelectedTool.toolName == interfaceTool.name)
				{
					UISounds.PlaySound(UISounds.Sound.ClickObject);
					this.activeTool = interfaceTool;
					PlayerController.Instance.ActivateTool(interfaceTool);
					break;
				}
			}
		}
		else
		{
			PlayerController.Instance.ActivateTool(SelectTool.Instance);
		}
		this.rows.ForEach(delegate(List<ToolMenu.ToolCollection> row)
		{
			this.RefreshRowDisplay(row);
		});
	}

	// Token: 0x060094B8 RID: 38072 RVA: 0x003A166C File Offset: 0x0039F86C
	private void RefreshRowDisplay(IList<ToolMenu.ToolCollection> row)
	{
		for (int i = 0; i < row.Count; i++)
		{
			ToolMenu.ToolCollection tc = row[i];
			if (this.currentlySelectedTool != null && this.currentlySelectedTool.collection == tc)
			{
				if (!tc.UIMenuDisplay.activeSelf || tc.UIMenuDisplay.GetComponent<ExpandRevealUIContent>().Collapsing)
				{
					if (tc.tools.Count > 1)
					{
						tc.UIMenuDisplay.SetActive(true);
						if (tc.tools.Count < this.smallCollectionMax)
						{
							float speedScale = Mathf.Clamp(1f - (float)tc.tools.Count * 0.15f, 0.5f, 1f);
							tc.UIMenuDisplay.GetComponent<ExpandRevealUIContent>().speedScale = speedScale;
						}
						tc.UIMenuDisplay.GetComponent<ExpandRevealUIContent>().Expand(delegate(object s)
						{
							this.SetToggleState(tc.toggle.GetComponent<KToggle>(), true);
						});
					}
					else
					{
						this.currentlySelectedTool = tc.tools[0];
					}
				}
			}
			else if (tc.UIMenuDisplay.activeSelf && !tc.UIMenuDisplay.GetComponent<ExpandRevealUIContent>().Collapsing && tc.tools.Count > 0)
			{
				tc.UIMenuDisplay.GetComponent<ExpandRevealUIContent>().Collapse(delegate(object s)
				{
					this.SetToggleState(tc.toggle.GetComponent<KToggle>(), false);
					tc.UIMenuDisplay.SetActive(false);
				});
			}
			for (int j = 0; j < tc.tools.Count; j++)
			{
				if (tc.tools[j] == this.currentlySelectedTool)
				{
					this.SetToggleState(tc.tools[j].toggle, true);
				}
				else
				{
					this.SetToggleState(tc.tools[j].toggle, false);
				}
			}
		}
	}

	// Token: 0x060094B9 RID: 38073 RVA: 0x00105926 File Offset: 0x00103B26
	public void TurnLargeCollectionOff()
	{
		if (this.currentlySelectedCollection != null && this.currentlySelectedCollection.tools.Count > this.smallCollectionMax)
		{
			this.ChooseCollection(null, true);
		}
	}

	// Token: 0x060094BA RID: 38074 RVA: 0x003A1884 File Offset: 0x0039FA84
	private void ChooseCollection(ToolMenu.ToolCollection collection, bool autoSelectTool = true)
	{
		if (collection == this.currentlySelectedCollection)
		{
			if (collection != null && collection.tools.Count > 1)
			{
				this.currentlySelectedCollection = null;
				if (this.currentlySelectedTool != null)
				{
					this.ChooseTool(null);
				}
			}
			else if (this.currentlySelectedTool != null && this.currentlySelectedCollection.tools.Contains(this.currentlySelectedTool) && this.currentlySelectedCollection.tools.Count == 1)
			{
				this.currentlySelectedCollection = null;
				this.ChooseTool(null);
			}
		}
		else
		{
			this.currentlySelectedCollection = collection;
		}
		this.rows.ForEach(delegate(List<ToolMenu.ToolCollection> row)
		{
			this.OpenOrCloseCollectionsInRow(row, true);
		});
	}

	// Token: 0x060094BB RID: 38075 RVA: 0x003A1924 File Offset: 0x0039FB24
	private void OpenOrCloseCollectionsInRow(IList<ToolMenu.ToolCollection> row, bool autoSelectTool = true)
	{
		for (int i = 0; i < row.Count; i++)
		{
			ToolMenu.ToolCollection tc = row[i];
			if (this.currentlySelectedCollection == tc)
			{
				if ((this.currentlySelectedCollection.tools != null && this.currentlySelectedCollection.tools.Count == 1) || autoSelectTool)
				{
					this.ChooseTool(this.currentlySelectedCollection.tools[0]);
				}
			}
			else if (tc.UIMenuDisplay.activeSelf && !tc.UIMenuDisplay.GetComponent<ExpandRevealUIContent>().Collapsing)
			{
				tc.UIMenuDisplay.GetComponent<ExpandRevealUIContent>().Collapse(delegate(object s)
				{
					this.SetToggleState(tc.toggle.GetComponent<KToggle>(), false);
					tc.UIMenuDisplay.SetActive(false);
				});
			}
			this.SetToggleState(tc.toggle.GetComponent<KToggle>(), this.currentlySelectedCollection == tc);
		}
	}

	// Token: 0x060094BC RID: 38076 RVA: 0x00105950 File Offset: 0x00103B50
	private void SetToggleState(KToggle toggle, bool state)
	{
		if (state)
		{
			toggle.Select();
			toggle.isOn = true;
			return;
		}
		toggle.Deselect();
		toggle.isOn = false;
	}

	// Token: 0x060094BD RID: 38077 RVA: 0x00105970 File Offset: 0x00103B70
	public void ClearSelection()
	{
		if (this.currentlySelectedCollection != null)
		{
			this.ChooseCollection(null, true);
		}
		if (this.currentlySelectedTool != null)
		{
			this.ChooseTool(null);
		}
	}

	// Token: 0x060094BE RID: 38078 RVA: 0x003A1A20 File Offset: 0x0039FC20
	public override void OnKeyDown(KButtonEvent e)
	{
		if (!e.Consumed)
		{
			if (e.IsAction(global::Action.ToggleSandboxTools))
			{
				if (Application.isEditor)
				{
					DebugUtil.LogArgs(new object[]
					{
						"Force-enabling sandbox mode because we're in editor."
					});
					SaveGame.Instance.sandboxEnabled = true;
				}
				if (SaveGame.Instance.sandboxEnabled)
				{
					Game.Instance.SandboxModeActive = !Game.Instance.SandboxModeActive;
					KMonoBehaviour.PlaySound(Game.Instance.SandboxModeActive ? GlobalAssets.GetSound("SandboxTool_Toggle_On", false) : GlobalAssets.GetSound("SandboxTool_Toggle_Off", false));
				}
			}
			foreach (List<ToolMenu.ToolCollection> list in this.rows)
			{
				if (list != this.sandboxTools || Game.Instance.SandboxModeActive)
				{
					int i = 0;
					while (i < list.Count)
					{
						global::Action toolHotkey = list[i].hotkey;
						if (toolHotkey != global::Action.NumActions && e.IsAction(toolHotkey) && (this.currentlySelectedCollection == null || (this.currentlySelectedCollection != null && this.currentlySelectedCollection.tools.Find((ToolMenu.ToolInfo t) => GameInputMapping.CompareActionKeyCodes(t.hotkey, toolHotkey)) == null)))
						{
							if (this.currentlySelectedCollection != list[i])
							{
								this.ChooseCollection(list[i], false);
								this.ChooseTool(list[i].tools[0]);
								break;
							}
							if (this.currentlySelectedCollection.tools.Count <= 1)
							{
								break;
							}
							e.Consumed = true;
							this.ChooseCollection(null, true);
							this.ChooseTool(null);
							string sound = GlobalAssets.GetSound(PlayerController.Instance.ActiveTool.GetDeactivateSound(), false);
							if (sound != null)
							{
								KMonoBehaviour.PlaySound(sound);
								break;
							}
							break;
						}
						else
						{
							for (int j = 0; j < list[i].tools.Count; j++)
							{
								if ((this.currentlySelectedCollection == null && list[i].tools.Count == 1) || this.currentlySelectedCollection == list[i] || (this.currentlySelectedCollection != null && this.currentlySelectedCollection.tools.Count == 1 && list[i].tools.Count == 1))
								{
									global::Action hotkey = list[i].tools[j].hotkey;
									if (e.IsAction(hotkey) && e.TryConsume(hotkey))
									{
										if (list[i].tools.Count == 1 && this.currentlySelectedCollection != list[i])
										{
											this.ChooseCollection(list[i], false);
										}
										else if (this.currentlySelectedTool != list[i].tools[j])
										{
											this.ChooseTool(list[i].tools[j]);
										}
									}
									else if (GameInputMapping.CompareActionKeyCodes(e.GetAction(), hotkey))
									{
										e.Consumed = true;
									}
								}
							}
							i++;
						}
					}
				}
			}
			if ((this.currentlySelectedTool != null || this.currentlySelectedCollection != null) && !e.Consumed)
			{
				if (e.TryConsume(global::Action.Escape))
				{
					string sound2 = GlobalAssets.GetSound(PlayerController.Instance.ActiveTool.GetDeactivateSound(), false);
					if (sound2 != null)
					{
						KMonoBehaviour.PlaySound(sound2);
					}
					if (this.currentlySelectedCollection != null)
					{
						this.ChooseCollection(null, true);
					}
					if (this.currentlySelectedTool != null)
					{
						this.ChooseTool(null);
					}
					SelectTool.Instance.Activate();
				}
			}
			else if (!PlayerController.Instance.IsUsingDefaultTool() && !e.Consumed && e.TryConsume(global::Action.Escape))
			{
				SelectTool.Instance.Activate();
			}
		}
		base.OnKeyDown(e);
	}

	// Token: 0x060094BF RID: 38079 RVA: 0x003A1E08 File Offset: 0x003A0008
	public override void OnKeyUp(KButtonEvent e)
	{
		if (!e.Consumed)
		{
			if ((this.currentlySelectedTool != null || this.currentlySelectedCollection != null) && !e.Consumed)
			{
				if (PlayerController.Instance.ConsumeIfNotDragging(e, global::Action.MouseRight))
				{
					string sound = GlobalAssets.GetSound(PlayerController.Instance.ActiveTool.GetDeactivateSound(), false);
					if (sound != null)
					{
						KMonoBehaviour.PlaySound(sound);
					}
					if (this.currentlySelectedCollection != null)
					{
						this.ChooseCollection(null, true);
					}
					if (this.currentlySelectedTool != null)
					{
						this.ChooseTool(null);
					}
					SelectTool.Instance.Activate();
				}
			}
			else if (!PlayerController.Instance.IsUsingDefaultTool() && !e.Consumed && PlayerController.Instance.ConsumeIfNotDragging(e, global::Action.MouseRight))
			{
				SelectTool.Instance.Activate();
				string sound2 = GlobalAssets.GetSound(PlayerController.Instance.ActiveTool.GetDeactivateSound(), false);
				if (sound2 != null)
				{
					KMonoBehaviour.PlaySound(sound2);
				}
			}
		}
		base.OnKeyUp(e);
	}

	// Token: 0x060094C0 RID: 38080 RVA: 0x003A1EE8 File Offset: 0x003A00E8
	protected void BuildRowToggles(IList<ToolMenu.ToolCollection> row)
	{
		for (int i = 0; i < row.Count; i++)
		{
			ToolMenu.ToolCollection toolCollection = row[i];
			if (!(toolCollection.toggle == null))
			{
				GameObject toggle = toolCollection.toggle;
				Sprite sprite = Assets.GetSprite(toolCollection.icon);
				if (sprite != null)
				{
					toggle.transform.Find("FG").GetComponent<Image>().sprite = sprite;
				}
				Transform transform = toggle.transform.Find("Text");
				if (transform != null)
				{
					LocText component = transform.GetComponent<LocText>();
					if (component != null)
					{
						component.text = toolCollection.text;
					}
				}
				ToolTip component2 = toggle.GetComponent<ToolTip>();
				if (component2)
				{
					if (row[i].tools.Count == 1)
					{
						string newString = GameUtil.ReplaceHotkeyString(row[i].tools[0].tooltip, row[i].tools[0].hotkey);
						component2.ClearMultiStringTooltip();
						component2.AddMultiStringTooltip(row[i].tools[0].text, this.TooltipHeader);
						component2.AddMultiStringTooltip(newString, this.ToggleToolTipTextStyleSetting);
					}
					else
					{
						string text = row[i].tooltip;
						if (row[i].hotkey != global::Action.NumActions)
						{
							text = GameUtil.ReplaceHotkeyString(text, row[i].hotkey);
						}
						component2.ClearMultiStringTooltip();
						component2.AddMultiStringTooltip(text, this.ToggleToolTipTextStyleSetting);
					}
				}
			}
		}
	}

	// Token: 0x060094C1 RID: 38081 RVA: 0x003A2084 File Offset: 0x003A0284
	protected void BuildToolToggles(IList<ToolMenu.ToolCollection> row)
	{
		for (int i = 0; i < row.Count; i++)
		{
			ToolMenu.ToolCollection toolCollection = row[i];
			if (!(toolCollection.toggle == null))
			{
				for (int j = 0; j < toolCollection.tools.Count; j++)
				{
					GameObject gameObject = toolCollection.tools[j].toggle.gameObject;
					Sprite sprite = Assets.GetSprite(toolCollection.icon);
					if (sprite != null)
					{
						gameObject.transform.Find("FG").GetComponent<Image>().sprite = sprite;
					}
					Transform transform = gameObject.transform.Find("Text");
					if (transform != null)
					{
						LocText component = transform.GetComponent<LocText>();
						if (component != null)
						{
							component.text = toolCollection.tools[j].text;
						}
					}
					ToolTip component2 = gameObject.GetComponent<ToolTip>();
					if (component2)
					{
						string newString = (toolCollection.tools.Count > 1) ? GameUtil.ReplaceHotkeyString(toolCollection.tools[j].tooltip, toolCollection.hotkey, toolCollection.tools[j].hotkey) : GameUtil.ReplaceHotkeyString(toolCollection.tools[j].tooltip, toolCollection.tools[j].hotkey);
						component2.ClearMultiStringTooltip();
						component2.AddMultiStringTooltip(newString, this.ToggleToolTipTextStyleSetting);
					}
				}
			}
		}
	}

	// Token: 0x060094C2 RID: 38082 RVA: 0x003A2200 File Offset: 0x003A0400
	public bool HasUniqueKeyBindings()
	{
		bool result = true;
		this.boundRootActions.Clear();
		foreach (List<ToolMenu.ToolCollection> list in this.rows)
		{
			foreach (ToolMenu.ToolCollection toolCollection in list)
			{
				if (this.boundRootActions.Contains(toolCollection.hotkey))
				{
					result = false;
					break;
				}
				this.boundRootActions.Add(toolCollection.hotkey);
				this.boundSubgroupActions.Clear();
				foreach (ToolMenu.ToolInfo toolInfo in toolCollection.tools)
				{
					if (this.boundSubgroupActions.Contains(toolInfo.hotkey))
					{
						result = false;
						break;
					}
					this.boundSubgroupActions.Add(toolInfo.hotkey);
				}
			}
		}
		return result;
	}

	// Token: 0x060094C3 RID: 38083 RVA: 0x00105991 File Offset: 0x00103B91
	private void OnPriorityClicked(PrioritySetting priority)
	{
		this.priorityScreen.SetScreenPriority(priority, false);
	}

	// Token: 0x040070C6 RID: 28870
	public static ToolMenu Instance;

	// Token: 0x040070C7 RID: 28871
	public GameObject Prefab_collectionContainer;

	// Token: 0x040070C8 RID: 28872
	public GameObject Prefab_collectionContainerWindow;

	// Token: 0x040070C9 RID: 28873
	public PriorityScreen Prefab_priorityScreen;

	// Token: 0x040070CA RID: 28874
	public GameObject toolIconPrefab;

	// Token: 0x040070CB RID: 28875
	public GameObject toolIconLargePrefab;

	// Token: 0x040070CC RID: 28876
	public GameObject sandboxToolIconPrefab;

	// Token: 0x040070CD RID: 28877
	public GameObject collectionIconPrefab;

	// Token: 0x040070CE RID: 28878
	public GameObject prefabToolRow;

	// Token: 0x040070CF RID: 28879
	public GameObject largeToolSet;

	// Token: 0x040070D0 RID: 28880
	public GameObject smallToolSet;

	// Token: 0x040070D1 RID: 28881
	public GameObject smallToolBottomRow;

	// Token: 0x040070D2 RID: 28882
	public GameObject smallToolTopRow;

	// Token: 0x040070D3 RID: 28883
	public GameObject sandboxToolSet;

	// Token: 0x040070D4 RID: 28884
	private PriorityScreen priorityScreen;

	// Token: 0x040070D5 RID: 28885
	public ToolParameterMenu toolParameterMenu;

	// Token: 0x040070D6 RID: 28886
	public GameObject sandboxToolParameterMenu;

	// Token: 0x040070D7 RID: 28887
	private GameObject toolEffectDisplayPlane;

	// Token: 0x040070D8 RID: 28888
	private Texture2D toolEffectDisplayPlaneTexture;

	// Token: 0x040070D9 RID: 28889
	public Material toolEffectDisplayMaterial;

	// Token: 0x040070DA RID: 28890
	private byte[] toolEffectDisplayBytes;

	// Token: 0x040070DB RID: 28891
	private List<List<ToolMenu.ToolCollection>> rows = new List<List<ToolMenu.ToolCollection>>();

	// Token: 0x040070DC RID: 28892
	public List<ToolMenu.ToolCollection> basicTools = new List<ToolMenu.ToolCollection>();

	// Token: 0x040070DD RID: 28893
	public List<ToolMenu.ToolCollection> sandboxTools = new List<ToolMenu.ToolCollection>();

	// Token: 0x040070DE RID: 28894
	public ToolMenu.ToolCollection currentlySelectedCollection;

	// Token: 0x040070DF RID: 28895
	public ToolMenu.ToolInfo currentlySelectedTool;

	// Token: 0x040070E0 RID: 28896
	public InterfaceTool activeTool;

	// Token: 0x040070E1 RID: 28897
	private Coroutine activeOpenAnimationRoutine;

	// Token: 0x040070E2 RID: 28898
	private Coroutine activeCloseAnimationRoutine;

	// Token: 0x040070E3 RID: 28899
	private HashSet<global::Action> boundRootActions = new HashSet<global::Action>();

	// Token: 0x040070E4 RID: 28900
	private HashSet<global::Action> boundSubgroupActions = new HashSet<global::Action>();

	// Token: 0x040070E5 RID: 28901
	private UnityAction inputChangeReceiver;

	// Token: 0x040070E6 RID: 28902
	private int refreshScaleHandle = -1;

	// Token: 0x040070E7 RID: 28903
	[SerializeField]
	public TextStyleSetting ToggleToolTipTextStyleSetting;

	// Token: 0x040070E8 RID: 28904
	[SerializeField]
	public TextStyleSetting CategoryLabelTextStyle_LeftAlign;

	// Token: 0x040070E9 RID: 28905
	[SerializeField]
	private TextStyleSetting TooltipHeader;

	// Token: 0x040070EA RID: 28906
	private int smallCollectionMax = 5;

	// Token: 0x040070EB RID: 28907
	private HashSet<ToolMenu.CellColorData> colors = new HashSet<ToolMenu.CellColorData>();

	// Token: 0x02001BA3 RID: 7075
	public class ToolInfo
	{
		// Token: 0x060094CC RID: 38092 RVA: 0x003A23A0 File Offset: 0x003A05A0
		public ToolInfo(string text, string icon_name, global::Action hotkey, string ToolName, ToolMenu.ToolCollection toolCollection, string tooltip = "", Action<object> onSelectCallback = null, object toolData = null)
		{
			this.text = text;
			this.icon = icon_name;
			this.hotkey = hotkey;
			this.toolName = ToolName;
			this.collection = toolCollection;
			toolCollection.tools.Add(this);
			this.tooltip = tooltip;
			this.onSelectCallback = onSelectCallback;
			this.toolData = toolData;
		}

		// Token: 0x040070EC RID: 28908
		public string text;

		// Token: 0x040070ED RID: 28909
		public string icon;

		// Token: 0x040070EE RID: 28910
		public global::Action hotkey;

		// Token: 0x040070EF RID: 28911
		public string toolName;

		// Token: 0x040070F0 RID: 28912
		public ToolMenu.ToolCollection collection;

		// Token: 0x040070F1 RID: 28913
		public string tooltip;

		// Token: 0x040070F2 RID: 28914
		public KToggle toggle;

		// Token: 0x040070F3 RID: 28915
		public Action<object> onSelectCallback;

		// Token: 0x040070F4 RID: 28916
		public object toolData;
	}

	// Token: 0x02001BA4 RID: 7076
	public class ToolCollection
	{
		// Token: 0x060094CD RID: 38093 RVA: 0x001059CE File Offset: 0x00103BCE
		public ToolCollection(string text, string icon_name, string tooltip = "", bool useInfoMenu = false, global::Action hotkey = global::Action.NumActions, bool largeIcon = false)
		{
			this.text = text;
			this.icon = icon_name;
			this.tooltip = tooltip;
			this.useInfoMenu = useInfoMenu;
			this.hotkey = hotkey;
			this.largeIcon = largeIcon;
		}

		// Token: 0x040070F5 RID: 28917
		public string text;

		// Token: 0x040070F6 RID: 28918
		public string icon;

		// Token: 0x040070F7 RID: 28919
		public string tooltip;

		// Token: 0x040070F8 RID: 28920
		public bool useInfoMenu;

		// Token: 0x040070F9 RID: 28921
		public bool largeIcon;

		// Token: 0x040070FA RID: 28922
		public GameObject toggle;

		// Token: 0x040070FB RID: 28923
		public List<ToolMenu.ToolInfo> tools = new List<ToolMenu.ToolInfo>();

		// Token: 0x040070FC RID: 28924
		public GameObject UIMenuDisplay;

		// Token: 0x040070FD RID: 28925
		public GameObject MaskContainer;

		// Token: 0x040070FE RID: 28926
		public global::Action hotkey;
	}

	// Token: 0x02001BA5 RID: 7077
	public struct CellColorData
	{
		// Token: 0x060094CE RID: 38094 RVA: 0x00105A0E File Offset: 0x00103C0E
		public CellColorData(int cell, Color color)
		{
			this.cell = cell;
			this.color = color;
		}

		// Token: 0x040070FF RID: 28927
		public int cell;

		// Token: 0x04007100 RID: 28928
		public Color color;
	}
}
