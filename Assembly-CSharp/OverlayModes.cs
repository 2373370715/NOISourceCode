using System;
using System.Collections.Generic;
using FMOD.Studio;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001B51 RID: 6993
public abstract class OverlayModes
{
	// Token: 0x02001B52 RID: 6994
	public class GasConduits : OverlayModes.ConduitMode
	{
		// Token: 0x060092B2 RID: 37554 RVA: 0x00104781 File Offset: 0x00102981
		public override HashedString ViewMode()
		{
			return OverlayModes.GasConduits.ID;
		}

		// Token: 0x060092B3 RID: 37555 RVA: 0x00104788 File Offset: 0x00102988
		public override string GetSoundName()
		{
			return "GasVent";
		}

		// Token: 0x060092B4 RID: 37556 RVA: 0x0010478F File Offset: 0x0010298F
		public GasConduits() : base(OverlayScreen.GasVentIDs)
		{
		}

		// Token: 0x04006F27 RID: 28455
		public static readonly HashedString ID = "GasConduit";
	}

	// Token: 0x02001B53 RID: 6995
	public class LiquidConduits : OverlayModes.ConduitMode
	{
		// Token: 0x060092B6 RID: 37558 RVA: 0x001047AD File Offset: 0x001029AD
		public override HashedString ViewMode()
		{
			return OverlayModes.LiquidConduits.ID;
		}

		// Token: 0x060092B7 RID: 37559 RVA: 0x001047B4 File Offset: 0x001029B4
		public override string GetSoundName()
		{
			return "LiquidVent";
		}

		// Token: 0x060092B8 RID: 37560 RVA: 0x001047BB File Offset: 0x001029BB
		public LiquidConduits() : base(OverlayScreen.LiquidVentIDs)
		{
		}

		// Token: 0x04006F28 RID: 28456
		public static readonly HashedString ID = "LiquidConduit";
	}

	// Token: 0x02001B54 RID: 6996
	public abstract class ConduitMode : OverlayModes.Mode
	{
		// Token: 0x060092BA RID: 37562 RVA: 0x00394430 File Offset: 0x00392630
		public ConduitMode(ICollection<Tag> ids)
		{
			this.objectTargetLayer = LayerMask.NameToLayer("MaskedOverlayBG");
			this.conduitTargetLayer = LayerMask.NameToLayer("MaskedOverlay");
			this.cameraLayerMask = LayerMask.GetMask(new string[]
			{
				"MaskedOverlay",
				"MaskedOverlayBG"
			});
			this.selectionMask = this.cameraLayerMask;
			this.targetIDs = ids;
		}

		// Token: 0x060092BB RID: 37563 RVA: 0x003944B8 File Offset: 0x003926B8
		public override void Enable()
		{
			base.RegisterSaveLoadListeners();
			this.partition = OverlayModes.Mode.PopulatePartition<SaveLoadRoot>(this.targetIDs);
			Camera.main.cullingMask |= this.cameraLayerMask;
			SelectTool.Instance.SetLayerMask(this.selectionMask);
			GridCompositor.Instance.ToggleMinor(false);
			base.Enable();
		}

		// Token: 0x060092BC RID: 37564 RVA: 0x00394514 File Offset: 0x00392714
		protected override void OnSaveLoadRootRegistered(SaveLoadRoot item)
		{
			Tag saveLoadTag = item.GetComponent<KPrefabID>().GetSaveLoadTag();
			if (this.targetIDs.Contains(saveLoadTag))
			{
				this.partition.Add(item);
			}
		}

		// Token: 0x060092BD RID: 37565 RVA: 0x00394548 File Offset: 0x00392748
		protected override void OnSaveLoadRootUnregistered(SaveLoadRoot item)
		{
			if (item == null || item.gameObject == null)
			{
				return;
			}
			if (this.layerTargets.Contains(item))
			{
				this.layerTargets.Remove(item);
			}
			this.partition.Remove(item);
		}

		// Token: 0x060092BE RID: 37566 RVA: 0x00394594 File Offset: 0x00392794
		public override void Disable()
		{
			foreach (SaveLoadRoot saveLoadRoot in this.layerTargets)
			{
				float defaultDepth = OverlayModes.Mode.GetDefaultDepth(saveLoadRoot);
				Vector3 position = saveLoadRoot.transform.GetPosition();
				position.z = defaultDepth;
				saveLoadRoot.transform.SetPosition(position);
				KBatchedAnimController[] componentsInChildren = saveLoadRoot.GetComponentsInChildren<KBatchedAnimController>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					this.TriggerResorting(componentsInChildren[i]);
				}
			}
			OverlayModes.Mode.ResetDisplayValues<SaveLoadRoot>(this.layerTargets);
			Camera.main.cullingMask &= ~this.cameraLayerMask;
			SelectTool.Instance.ClearLayerMask();
			base.UnregisterSaveLoadListeners();
			this.partition.Clear();
			this.layerTargets.Clear();
			GridCompositor.Instance.ToggleMinor(false);
			base.Disable();
		}

		// Token: 0x060092BF RID: 37567 RVA: 0x00394684 File Offset: 0x00392884
		public override void Update()
		{
			Vector2I vector2I;
			Vector2I vector2I2;
			Grid.GetVisibleExtents(out vector2I, out vector2I2);
			OverlayModes.Mode.RemoveOffscreenTargets<SaveLoadRoot>(this.layerTargets, vector2I, vector2I2, delegate(SaveLoadRoot root)
			{
				if (root == null)
				{
					return;
				}
				float defaultDepth = OverlayModes.Mode.GetDefaultDepth(root);
				Vector3 position = root.transform.GetPosition();
				position.z = defaultDepth;
				root.transform.SetPosition(position);
				KBatchedAnimController[] componentsInChildren = root.GetComponentsInChildren<KBatchedAnimController>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					this.TriggerResorting(componentsInChildren[i]);
				}
			});
			foreach (object obj in this.partition.GetAllIntersecting(new Vector2((float)vector2I.x, (float)vector2I.y), new Vector2((float)vector2I2.x, (float)vector2I2.y)))
			{
				SaveLoadRoot saveLoadRoot = (SaveLoadRoot)obj;
				if (saveLoadRoot.GetComponent<Conduit>() != null)
				{
					base.AddTargetIfVisible<SaveLoadRoot>(saveLoadRoot, vector2I, vector2I2, this.layerTargets, this.conduitTargetLayer, null, null);
				}
				else
				{
					base.AddTargetIfVisible<SaveLoadRoot>(saveLoadRoot, vector2I, vector2I2, this.layerTargets, this.objectTargetLayer, delegate(SaveLoadRoot root)
					{
						Vector3 position = root.transform.GetPosition();
						float z = position.z;
						KPrefabID component3 = root.GetComponent<KPrefabID>();
						if (component3 != null)
						{
							if (component3.HasTag(GameTags.OverlayInFrontOfConduits))
							{
								z = Grid.GetLayerZ((this.ViewMode() == OverlayModes.LiquidConduits.ID) ? Grid.SceneLayer.LiquidConduits : Grid.SceneLayer.GasConduits) - 0.2f;
							}
							else if (component3.HasTag(GameTags.OverlayBehindConduits))
							{
								z = Grid.GetLayerZ((this.ViewMode() == OverlayModes.LiquidConduits.ID) ? Grid.SceneLayer.LiquidConduits : Grid.SceneLayer.GasConduits) + 0.2f;
							}
						}
						position.z = z;
						root.transform.SetPosition(position);
						KBatchedAnimController[] componentsInChildren = root.GetComponentsInChildren<KBatchedAnimController>();
						for (int i = 0; i < componentsInChildren.Length; i++)
						{
							this.TriggerResorting(componentsInChildren[i]);
						}
					}, null);
				}
			}
			GameObject gameObject = null;
			if (SelectTool.Instance != null && SelectTool.Instance.hover != null)
			{
				gameObject = SelectTool.Instance.hover.gameObject;
			}
			this.connectedNetworks.Clear();
			float num = 1f;
			if (gameObject != null)
			{
				IBridgedNetworkItem component = gameObject.GetComponent<IBridgedNetworkItem>();
				if (component != null)
				{
					int networkCell = component.GetNetworkCell();
					UtilityNetworkManager<FlowUtilityNetwork, Vent> mgr = (this.ViewMode() == OverlayModes.LiquidConduits.ID) ? Game.Instance.liquidConduitSystem : Game.Instance.gasConduitSystem;
					this.visited.Clear();
					this.FindConnectedNetworks(networkCell, mgr, this.connectedNetworks, this.visited);
					this.visited.Clear();
					num = OverlayModes.ModeUtil.GetHighlightScale();
				}
			}
			Game.ConduitVisInfo conduitVisInfo = (this.ViewMode() == OverlayModes.LiquidConduits.ID) ? Game.Instance.liquidConduitVisInfo : Game.Instance.gasConduitVisInfo;
			foreach (SaveLoadRoot saveLoadRoot2 in this.layerTargets)
			{
				if (!(saveLoadRoot2 == null) && saveLoadRoot2.GetComponent<IBridgedNetworkItem>() != null)
				{
					BuildingDef def = saveLoadRoot2.GetComponent<Building>().Def;
					Color32 colorByName;
					if (def.ThermalConductivity == 1f)
					{
						colorByName = GlobalAssets.Instance.colorSet.GetColorByName(conduitVisInfo.overlayTintName);
					}
					else if (def.ThermalConductivity < 1f)
					{
						colorByName = GlobalAssets.Instance.colorSet.GetColorByName(conduitVisInfo.overlayInsulatedTintName);
					}
					else
					{
						colorByName = GlobalAssets.Instance.colorSet.GetColorByName(conduitVisInfo.overlayRadiantTintName);
					}
					if (this.connectedNetworks.Count > 0)
					{
						IBridgedNetworkItem component2 = saveLoadRoot2.GetComponent<IBridgedNetworkItem>();
						if (component2 != null && component2.IsConnectedToNetworks(this.connectedNetworks))
						{
							colorByName.r = (byte)((float)colorByName.r * num);
							colorByName.g = (byte)((float)colorByName.g * num);
							colorByName.b = (byte)((float)colorByName.b * num);
						}
					}
					saveLoadRoot2.GetComponent<KBatchedAnimController>().TintColour = colorByName;
				}
			}
		}

		// Token: 0x060092C0 RID: 37568 RVA: 0x001047D9 File Offset: 0x001029D9
		private void TriggerResorting(KBatchedAnimController kbac)
		{
			if (kbac.enabled)
			{
				kbac.enabled = false;
				kbac.enabled = true;
			}
		}

		// Token: 0x060092C1 RID: 37569 RVA: 0x003949B8 File Offset: 0x00392BB8
		private void FindConnectedNetworks(int cell, IUtilityNetworkMgr mgr, ICollection<UtilityNetwork> networks, List<int> visited)
		{
			if (visited.Contains(cell))
			{
				return;
			}
			visited.Add(cell);
			UtilityNetwork networkForCell = mgr.GetNetworkForCell(cell);
			if (networkForCell != null)
			{
				networks.Add(networkForCell);
				UtilityConnections connections = mgr.GetConnections(cell, false);
				if ((connections & UtilityConnections.Right) != (UtilityConnections)0)
				{
					this.FindConnectedNetworks(Grid.CellRight(cell), mgr, networks, visited);
				}
				if ((connections & UtilityConnections.Left) != (UtilityConnections)0)
				{
					this.FindConnectedNetworks(Grid.CellLeft(cell), mgr, networks, visited);
				}
				if ((connections & UtilityConnections.Up) != (UtilityConnections)0)
				{
					this.FindConnectedNetworks(Grid.CellAbove(cell), mgr, networks, visited);
				}
				if ((connections & UtilityConnections.Down) != (UtilityConnections)0)
				{
					this.FindConnectedNetworks(Grid.CellBelow(cell), mgr, networks, visited);
				}
				object endpoint = mgr.GetEndpoint(cell);
				if (endpoint != null)
				{
					FlowUtilityNetwork.NetworkItem networkItem = endpoint as FlowUtilityNetwork.NetworkItem;
					if (networkItem != null)
					{
						IBridgedNetworkItem component = networkItem.GameObject.GetComponent<IBridgedNetworkItem>();
						if (component != null)
						{
							component.AddNetworks(networks);
						}
					}
				}
			}
		}

		// Token: 0x04006F29 RID: 28457
		private UniformGrid<SaveLoadRoot> partition;

		// Token: 0x04006F2A RID: 28458
		private HashSet<SaveLoadRoot> layerTargets = new HashSet<SaveLoadRoot>();

		// Token: 0x04006F2B RID: 28459
		private HashSet<UtilityNetwork> connectedNetworks = new HashSet<UtilityNetwork>();

		// Token: 0x04006F2C RID: 28460
		private List<int> visited = new List<int>();

		// Token: 0x04006F2D RID: 28461
		private ICollection<Tag> targetIDs;

		// Token: 0x04006F2E RID: 28462
		private int objectTargetLayer;

		// Token: 0x04006F2F RID: 28463
		private int conduitTargetLayer;

		// Token: 0x04006F30 RID: 28464
		private int cameraLayerMask;

		// Token: 0x04006F31 RID: 28465
		private int selectionMask;
	}

	// Token: 0x02001B55 RID: 6997
	public class Crop : OverlayModes.BasePlantMode
	{
		// Token: 0x060092C4 RID: 37572 RVA: 0x001047F1 File Offset: 0x001029F1
		public override HashedString ViewMode()
		{
			return OverlayModes.Crop.ID;
		}

		// Token: 0x060092C5 RID: 37573 RVA: 0x001047F8 File Offset: 0x001029F8
		public override string GetSoundName()
		{
			return "Harvest";
		}

		// Token: 0x060092C6 RID: 37574 RVA: 0x00394B9C File Offset: 0x00392D9C
		public Crop(Canvas ui_root, GameObject harvestable_notification_prefab)
		{
			OverlayModes.ColorHighlightCondition[] array = new OverlayModes.ColorHighlightCondition[3];
			array[0] = new OverlayModes.ColorHighlightCondition((KMonoBehaviour h) => GlobalAssets.Instance.colorSet.cropHalted, delegate(KMonoBehaviour h)
			{
				WiltCondition component = h.GetComponent<WiltCondition>();
				return component != null && component.IsWilting();
			});
			array[1] = new OverlayModes.ColorHighlightCondition((KMonoBehaviour h) => GlobalAssets.Instance.colorSet.cropGrowing, (KMonoBehaviour h) => !(h as HarvestDesignatable).CanBeHarvested());
			array[2] = new OverlayModes.ColorHighlightCondition((KMonoBehaviour h) => GlobalAssets.Instance.colorSet.cropGrown, (KMonoBehaviour h) => (h as HarvestDesignatable).CanBeHarvested());
			this.highlightConditions = array;
			base..ctor(OverlayScreen.HarvestableIDs);
			this.uiRoot = ui_root;
			this.harvestableNotificationPrefab = harvestable_notification_prefab;
		}

		// Token: 0x060092C7 RID: 37575 RVA: 0x00394CB8 File Offset: 0x00392EB8
		public override List<LegendEntry> GetCustomLegendData()
		{
			return new List<LegendEntry>
			{
				new LegendEntry(UI.OVERLAYS.CROP.FULLY_GROWN, UI.OVERLAYS.CROP.TOOLTIPS.FULLY_GROWN, GlobalAssets.Instance.colorSet.cropGrown, null, null, true),
				new LegendEntry(UI.OVERLAYS.CROP.GROWING, UI.OVERLAYS.CROP.TOOLTIPS.GROWING, GlobalAssets.Instance.colorSet.cropGrowing, null, null, true),
				new LegendEntry(UI.OVERLAYS.CROP.GROWTH_HALTED, UI.OVERLAYS.CROP.TOOLTIPS.GROWTH_HALTED, GlobalAssets.Instance.colorSet.cropHalted, null, null, true)
			};
		}

		// Token: 0x060092C8 RID: 37576 RVA: 0x00394D6C File Offset: 0x00392F6C
		public override void Update()
		{
			this.updateCropInfo.Clear();
			this.freeHarvestableNotificationIdx = 0;
			Vector2I vector2I;
			Vector2I vector2I2;
			Grid.GetVisibleExtents(out vector2I, out vector2I2);
			OverlayModes.Mode.RemoveOffscreenTargets<HarvestDesignatable>(this.layerTargets, vector2I, vector2I2, null);
			foreach (object obj in this.partition.GetAllIntersecting(new Vector2((float)vector2I.x, (float)vector2I.y), new Vector2((float)vector2I2.x, (float)vector2I2.y)))
			{
				HarvestDesignatable instance = (HarvestDesignatable)obj;
				base.AddTargetIfVisible<HarvestDesignatable>(instance, vector2I, vector2I2, this.layerTargets, this.targetLayer, null, null);
			}
			foreach (HarvestDesignatable harvestDesignatable in this.layerTargets)
			{
				Vector2I vector2I3 = Grid.PosToXY(harvestDesignatable.transform.GetPosition());
				if (vector2I <= vector2I3 && vector2I3 <= vector2I2)
				{
					this.AddCropUI(harvestDesignatable);
				}
			}
			foreach (OverlayModes.Crop.UpdateCropInfo updateCropInfo in this.updateCropInfo)
			{
				updateCropInfo.harvestableUI.GetComponent<HarvestableOverlayWidget>().Refresh(updateCropInfo.harvestable);
			}
			for (int i = this.freeHarvestableNotificationIdx; i < this.harvestableNotificationList.Count; i++)
			{
				if (this.harvestableNotificationList[i].activeSelf)
				{
					this.harvestableNotificationList[i].SetActive(false);
				}
			}
			base.UpdateHighlightTypeOverlay<HarvestDesignatable>(vector2I, vector2I2, this.layerTargets, this.targetIDs, this.highlightConditions, OverlayModes.BringToFrontLayerSetting.Constant, this.targetLayer);
			base.Update();
		}

		// Token: 0x060092C9 RID: 37577 RVA: 0x001047FF File Offset: 0x001029FF
		public override void Disable()
		{
			this.DisableHarvestableUINotifications();
			base.Disable();
		}

		// Token: 0x060092CA RID: 37578 RVA: 0x00394F5C File Offset: 0x0039315C
		private void DisableHarvestableUINotifications()
		{
			this.freeHarvestableNotificationIdx = 0;
			foreach (GameObject gameObject in this.harvestableNotificationList)
			{
				gameObject.SetActive(false);
			}
			this.updateCropInfo.Clear();
		}

		// Token: 0x060092CB RID: 37579 RVA: 0x00394FC0 File Offset: 0x003931C0
		public GameObject GetFreeCropUI()
		{
			GameObject gameObject;
			if (this.freeHarvestableNotificationIdx < this.harvestableNotificationList.Count)
			{
				gameObject = this.harvestableNotificationList[this.freeHarvestableNotificationIdx];
				if (!gameObject.gameObject.activeSelf)
				{
					gameObject.gameObject.SetActive(true);
				}
				this.freeHarvestableNotificationIdx++;
			}
			else
			{
				gameObject = global::Util.KInstantiateUI(this.harvestableNotificationPrefab.gameObject, this.uiRoot.transform.gameObject, false);
				this.harvestableNotificationList.Add(gameObject);
				this.freeHarvestableNotificationIdx++;
			}
			return gameObject;
		}

		// Token: 0x060092CC RID: 37580 RVA: 0x0039505C File Offset: 0x0039325C
		private void AddCropUI(HarvestDesignatable harvestable)
		{
			GameObject freeCropUI = this.GetFreeCropUI();
			OverlayModes.Crop.UpdateCropInfo item = new OverlayModes.Crop.UpdateCropInfo(harvestable, freeCropUI);
			Vector3 b = Grid.CellToPos(Grid.PosToCell(harvestable), 0.5f, -1.25f, 0f) + harvestable.iconOffset;
			freeCropUI.GetComponent<RectTransform>().SetPosition(Vector3.up + b);
			this.updateCropInfo.Add(item);
		}

		// Token: 0x04006F32 RID: 28466
		public static readonly HashedString ID = "Crop";

		// Token: 0x04006F33 RID: 28467
		private Canvas uiRoot;

		// Token: 0x04006F34 RID: 28468
		private List<OverlayModes.Crop.UpdateCropInfo> updateCropInfo = new List<OverlayModes.Crop.UpdateCropInfo>();

		// Token: 0x04006F35 RID: 28469
		private int freeHarvestableNotificationIdx;

		// Token: 0x04006F36 RID: 28470
		private List<GameObject> harvestableNotificationList = new List<GameObject>();

		// Token: 0x04006F37 RID: 28471
		private GameObject harvestableNotificationPrefab;

		// Token: 0x04006F38 RID: 28472
		private OverlayModes.ColorHighlightCondition[] highlightConditions;

		// Token: 0x02001B56 RID: 6998
		private struct UpdateCropInfo
		{
			// Token: 0x060092CE RID: 37582 RVA: 0x0010481E File Offset: 0x00102A1E
			public UpdateCropInfo(HarvestDesignatable harvestable, GameObject harvestableUI)
			{
				this.harvestable = harvestable;
				this.harvestableUI = harvestableUI;
			}

			// Token: 0x04006F39 RID: 28473
			public HarvestDesignatable harvestable;

			// Token: 0x04006F3A RID: 28474
			public GameObject harvestableUI;
		}
	}

	// Token: 0x02001B58 RID: 7000
	public class Harvest : OverlayModes.BasePlantMode
	{
		// Token: 0x060092D7 RID: 37591 RVA: 0x00104899 File Offset: 0x00102A99
		public override HashedString ViewMode()
		{
			return OverlayModes.Harvest.ID;
		}

		// Token: 0x060092D8 RID: 37592 RVA: 0x001047F8 File Offset: 0x001029F8
		public override string GetSoundName()
		{
			return "Harvest";
		}

		// Token: 0x060092D9 RID: 37593 RVA: 0x003950F0 File Offset: 0x003932F0
		public Harvest()
		{
			OverlayModes.ColorHighlightCondition[] array = new OverlayModes.ColorHighlightCondition[1];
			array[0] = new OverlayModes.ColorHighlightCondition((KMonoBehaviour harvestable) => new Color(0.65f, 0.65f, 0.65f, 0.65f), (KMonoBehaviour harvestable) => true);
			this.highlightConditions = array;
			base..ctor(OverlayScreen.HarvestableIDs);
		}

		// Token: 0x060092DA RID: 37594 RVA: 0x0039515C File Offset: 0x0039335C
		public override void Update()
		{
			Vector2I vector2I;
			Vector2I vector2I2;
			Grid.GetVisibleExtents(out vector2I, out vector2I2);
			OverlayModes.Mode.RemoveOffscreenTargets<HarvestDesignatable>(this.layerTargets, vector2I, vector2I2, null);
			foreach (object obj in this.partition.GetAllIntersecting(new Vector2((float)vector2I.x, (float)vector2I.y), new Vector2((float)vector2I2.x, (float)vector2I2.y)))
			{
				HarvestDesignatable instance = (HarvestDesignatable)obj;
				base.AddTargetIfVisible<HarvestDesignatable>(instance, vector2I, vector2I2, this.layerTargets, this.targetLayer, null, null);
			}
			base.UpdateHighlightTypeOverlay<HarvestDesignatable>(vector2I, vector2I2, this.layerTargets, this.targetIDs, this.highlightConditions, OverlayModes.BringToFrontLayerSetting.Constant, this.targetLayer);
			base.Update();
		}

		// Token: 0x04006F42 RID: 28482
		public static readonly HashedString ID = "HarvestWhenReady";

		// Token: 0x04006F43 RID: 28483
		private OverlayModes.ColorHighlightCondition[] highlightConditions;
	}

	// Token: 0x02001B5A RID: 7002
	public abstract class BasePlantMode : OverlayModes.Mode
	{
		// Token: 0x060092E0 RID: 37600 RVA: 0x00395234 File Offset: 0x00393434
		public BasePlantMode(ICollection<Tag> ids)
		{
			this.targetLayer = LayerMask.NameToLayer("MaskedOverlay");
			this.cameraLayerMask = LayerMask.GetMask(new string[]
			{
				"MaskedOverlay",
				"MaskedOverlayBG"
			});
			this.selectionMask = LayerMask.GetMask(new string[]
			{
				"MaskedOverlay"
			});
			this.targetIDs = ids;
		}

		// Token: 0x060092E1 RID: 37601 RVA: 0x001048D8 File Offset: 0x00102AD8
		public override void Enable()
		{
			base.RegisterSaveLoadListeners();
			this.partition = OverlayModes.Mode.PopulatePartition<HarvestDesignatable>(this.targetIDs);
			Camera.main.cullingMask |= this.cameraLayerMask;
			SelectTool.Instance.SetLayerMask(this.selectionMask);
		}

		// Token: 0x060092E2 RID: 37602 RVA: 0x003952A4 File Offset: 0x003934A4
		protected override void OnSaveLoadRootRegistered(SaveLoadRoot item)
		{
			Tag saveLoadTag = item.GetComponent<KPrefabID>().GetSaveLoadTag();
			if (!this.targetIDs.Contains(saveLoadTag))
			{
				return;
			}
			HarvestDesignatable component = item.GetComponent<HarvestDesignatable>();
			if (component == null)
			{
				return;
			}
			this.partition.Add(component);
		}

		// Token: 0x060092E3 RID: 37603 RVA: 0x003952EC File Offset: 0x003934EC
		protected override void OnSaveLoadRootUnregistered(SaveLoadRoot item)
		{
			if (item == null || item.gameObject == null)
			{
				return;
			}
			HarvestDesignatable component = item.GetComponent<HarvestDesignatable>();
			if (component == null)
			{
				return;
			}
			if (this.layerTargets.Contains(component))
			{
				this.layerTargets.Remove(component);
			}
			this.partition.Remove(component);
		}

		// Token: 0x060092E4 RID: 37604 RVA: 0x0039534C File Offset: 0x0039354C
		public override void Disable()
		{
			base.UnregisterSaveLoadListeners();
			base.DisableHighlightTypeOverlay<HarvestDesignatable>(this.layerTargets);
			Camera.main.cullingMask &= ~this.cameraLayerMask;
			this.partition.Clear();
			this.layerTargets.Clear();
			SelectTool.Instance.ClearLayerMask();
		}

		// Token: 0x04006F47 RID: 28487
		protected UniformGrid<HarvestDesignatable> partition;

		// Token: 0x04006F48 RID: 28488
		protected HashSet<HarvestDesignatable> layerTargets = new HashSet<HarvestDesignatable>();

		// Token: 0x04006F49 RID: 28489
		protected ICollection<Tag> targetIDs;

		// Token: 0x04006F4A RID: 28490
		protected int targetLayer;

		// Token: 0x04006F4B RID: 28491
		private int cameraLayerMask;

		// Token: 0x04006F4C RID: 28492
		private int selectionMask;
	}

	// Token: 0x02001B5B RID: 7003
	public class Decor : OverlayModes.Mode
	{
		// Token: 0x060092E5 RID: 37605 RVA: 0x00104918 File Offset: 0x00102B18
		public override HashedString ViewMode()
		{
			return OverlayModes.Decor.ID;
		}

		// Token: 0x060092E6 RID: 37606 RVA: 0x0010491F File Offset: 0x00102B1F
		public override string GetSoundName()
		{
			return "Decor";
		}

		// Token: 0x060092E7 RID: 37607 RVA: 0x003953A4 File Offset: 0x003935A4
		public override List<LegendEntry> GetCustomLegendData()
		{
			return new List<LegendEntry>
			{
				new LegendEntry(UI.OVERLAYS.DECOR.HIGHDECOR, UI.OVERLAYS.DECOR.TOOLTIPS.HIGHDECOR, GlobalAssets.Instance.colorSet.decorPositive, null, null, true),
				new LegendEntry(UI.OVERLAYS.DECOR.LOWDECOR, UI.OVERLAYS.DECOR.TOOLTIPS.LOWDECOR, GlobalAssets.Instance.colorSet.decorNegative, null, null, true)
			};
		}

		// Token: 0x060092E8 RID: 37608 RVA: 0x00395424 File Offset: 0x00393624
		public Decor()
		{
			OverlayModes.ColorHighlightCondition[] array = new OverlayModes.ColorHighlightCondition[1];
			array[0] = new OverlayModes.ColorHighlightCondition(delegate(KMonoBehaviour dp)
			{
				Color black = Color.black;
				Color b = Color.black;
				if (dp != null)
				{
					int cell = Grid.PosToCell(CameraController.Instance.baseCamera.ScreenToWorldPoint(KInputManager.GetMousePos()));
					float decorForCell = (dp as DecorProvider).GetDecorForCell(cell);
					if (decorForCell > 0f)
					{
						b = GlobalAssets.Instance.colorSet.decorHighlightPositive;
					}
					else if (decorForCell < 0f)
					{
						b = GlobalAssets.Instance.colorSet.decorHighlightNegative;
					}
					else if (dp.GetComponent<MonumentPart>() != null && dp.GetComponent<MonumentPart>().IsMonumentCompleted())
					{
						foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(dp.GetComponent<AttachableBuilding>()))
						{
							decorForCell = gameObject.GetComponent<DecorProvider>().GetDecorForCell(cell);
							if (decorForCell > 0f)
							{
								b = GlobalAssets.Instance.colorSet.decorHighlightPositive;
								break;
							}
							if (decorForCell < 0f)
							{
								b = GlobalAssets.Instance.colorSet.decorHighlightNegative;
								break;
							}
						}
					}
				}
				return Color.Lerp(black, b, 0.85f);
			}, (KMonoBehaviour dp) => SelectToolHoverTextCard.highlightedObjects.Contains(dp.gameObject));
			this.highlightConditions = array;
			base..ctor();
			this.targetLayer = LayerMask.NameToLayer("MaskedOverlay");
			this.cameraLayerMask = LayerMask.GetMask(new string[]
			{
				"MaskedOverlay",
				"MaskedOverlayBG"
			});
		}

		// Token: 0x060092E9 RID: 37609 RVA: 0x003954DC File Offset: 0x003936DC
		public override void Enable()
		{
			base.RegisterSaveLoadListeners();
			List<Tag> prefabTagsWithComponent = Assets.GetPrefabTagsWithComponent<DecorProvider>();
			this.targetIDs.UnionWith(prefabTagsWithComponent);
			foreach (Tag item in new Tag[]
			{
				new Tag("Tile"),
				new Tag("SnowTile"),
				new Tag("WoodTile"),
				new Tag("MeshTile"),
				new Tag("InsulationTile"),
				new Tag("GasPermeableMembrane"),
				new Tag("CarpetTile")
			})
			{
				this.targetIDs.Remove(item);
			}
			foreach (Tag item2 in OverlayScreen.GasVentIDs)
			{
				this.targetIDs.Remove(item2);
			}
			foreach (Tag item3 in OverlayScreen.LiquidVentIDs)
			{
				this.targetIDs.Remove(item3);
			}
			this.partition = OverlayModes.Mode.PopulatePartition<DecorProvider>(this.targetIDs);
			Camera.main.cullingMask |= this.cameraLayerMask;
		}

		// Token: 0x060092EA RID: 37610 RVA: 0x00395664 File Offset: 0x00393864
		public override void Update()
		{
			Vector2I vector2I;
			Vector2I vector2I2;
			Grid.GetVisibleExtents(out vector2I, out vector2I2);
			OverlayModes.Mode.RemoveOffscreenTargets<DecorProvider>(this.layerTargets, vector2I, vector2I2, null);
			this.partition.GetAllIntersecting(new Vector2((float)vector2I.x, (float)vector2I.y), new Vector2((float)vector2I2.x, (float)vector2I2.y), this.workingTargets);
			for (int i = 0; i < this.workingTargets.Count; i++)
			{
				DecorProvider instance = this.workingTargets[i];
				base.AddTargetIfVisible<DecorProvider>(instance, vector2I, vector2I2, this.layerTargets, this.targetLayer, null, null);
			}
			base.UpdateHighlightTypeOverlay<DecorProvider>(vector2I, vector2I2, this.layerTargets, this.targetIDs, this.highlightConditions, OverlayModes.BringToFrontLayerSetting.Conditional, this.targetLayer);
			this.workingTargets.Clear();
		}

		// Token: 0x060092EB RID: 37611 RVA: 0x00395728 File Offset: 0x00393928
		protected override void OnSaveLoadRootRegistered(SaveLoadRoot item)
		{
			Tag saveLoadTag = item.GetComponent<KPrefabID>().GetSaveLoadTag();
			if (this.targetIDs.Contains(saveLoadTag))
			{
				DecorProvider component = item.GetComponent<DecorProvider>();
				if (component != null)
				{
					this.partition.Add(component);
				}
			}
		}

		// Token: 0x060092EC RID: 37612 RVA: 0x0039576C File Offset: 0x0039396C
		protected override void OnSaveLoadRootUnregistered(SaveLoadRoot item)
		{
			if (item == null || item.gameObject == null)
			{
				return;
			}
			DecorProvider component = item.GetComponent<DecorProvider>();
			if (component != null)
			{
				if (this.layerTargets.Contains(component))
				{
					this.layerTargets.Remove(component);
				}
				this.partition.Remove(component);
			}
		}

		// Token: 0x060092ED RID: 37613 RVA: 0x003957C8 File Offset: 0x003939C8
		public override void Disable()
		{
			base.DisableHighlightTypeOverlay<DecorProvider>(this.layerTargets);
			Camera.main.cullingMask &= ~this.cameraLayerMask;
			base.UnregisterSaveLoadListeners();
			this.partition.Clear();
			this.layerTargets.Clear();
		}

		// Token: 0x04006F4D RID: 28493
		public static readonly HashedString ID = "Decor";

		// Token: 0x04006F4E RID: 28494
		private UniformGrid<DecorProvider> partition;

		// Token: 0x04006F4F RID: 28495
		private HashSet<DecorProvider> layerTargets = new HashSet<DecorProvider>();

		// Token: 0x04006F50 RID: 28496
		private List<DecorProvider> workingTargets = new List<DecorProvider>();

		// Token: 0x04006F51 RID: 28497
		private HashSet<Tag> targetIDs = new HashSet<Tag>();

		// Token: 0x04006F52 RID: 28498
		private int targetLayer;

		// Token: 0x04006F53 RID: 28499
		private int cameraLayerMask;

		// Token: 0x04006F54 RID: 28500
		private OverlayModes.ColorHighlightCondition[] highlightConditions;
	}

	// Token: 0x02001B5D RID: 7005
	public class Disease : OverlayModes.Mode
	{
		// Token: 0x060092F3 RID: 37619 RVA: 0x00395960 File Offset: 0x00393B60
		private static float CalculateHUE(Color32 colour)
		{
			byte b = Math.Max(colour.r, Math.Max(colour.g, colour.b));
			byte b2 = Math.Min(colour.r, Math.Min(colour.g, colour.b));
			float result = 0f;
			int num = (int)(b - b2);
			if (num == 0)
			{
				result = 0f;
			}
			else if (b == colour.r)
			{
				result = (float)(colour.g - colour.b) / (float)num % 6f;
			}
			else if (b == colour.g)
			{
				result = (float)(colour.b - colour.r) / (float)num + 2f;
			}
			else if (b == colour.b)
			{
				result = (float)(colour.r - colour.g) / (float)num + 4f;
			}
			return result;
		}

		// Token: 0x060092F4 RID: 37620 RVA: 0x00104955 File Offset: 0x00102B55
		public override HashedString ViewMode()
		{
			return OverlayModes.Disease.ID;
		}

		// Token: 0x060092F5 RID: 37621 RVA: 0x0010495C File Offset: 0x00102B5C
		public override string GetSoundName()
		{
			return "Disease";
		}

		// Token: 0x060092F6 RID: 37622 RVA: 0x00395A24 File Offset: 0x00393C24
		public Disease(Canvas diseaseUIParent, GameObject diseaseOverlayPrefab)
		{
			this.diseaseUIParent = diseaseUIParent;
			this.diseaseOverlayPrefab = diseaseOverlayPrefab;
			this.legendFilters = this.CreateDefaultFilters();
			this.cameraLayerMask = LayerMask.GetMask(new string[]
			{
				"MaskedOverlay",
				"MaskedOverlayBG"
			});
		}

		// Token: 0x060092F7 RID: 37623 RVA: 0x00395AAC File Offset: 0x00393CAC
		public override void Enable()
		{
			Infrared.Instance.SetMode(Infrared.Mode.Disease);
			CameraController.Instance.ToggleColouredOverlayView(true);
			Camera.main.cullingMask |= this.cameraLayerMask;
			base.RegisterSaveLoadListeners();
			foreach (DiseaseSourceVisualizer diseaseSourceVisualizer in Components.DiseaseSourceVisualizers.Items)
			{
				if (!(diseaseSourceVisualizer == null))
				{
					diseaseSourceVisualizer.Show(this.ViewMode());
				}
			}
		}

		// Token: 0x060092F8 RID: 37624 RVA: 0x00104963 File Offset: 0x00102B63
		public override Dictionary<string, ToolParameterMenu.ToggleState> CreateDefaultFilters()
		{
			return new Dictionary<string, ToolParameterMenu.ToggleState>
			{
				{
					ToolParameterMenu.FILTERLAYERS.ALL,
					ToolParameterMenu.ToggleState.On
				},
				{
					ToolParameterMenu.FILTERLAYERS.LIQUIDCONDUIT,
					ToolParameterMenu.ToggleState.Off
				},
				{
					ToolParameterMenu.FILTERLAYERS.GASCONDUIT,
					ToolParameterMenu.ToggleState.Off
				}
			};
		}

		// Token: 0x060092F9 RID: 37625 RVA: 0x0010498E File Offset: 0x00102B8E
		public override void OnFiltersChanged()
		{
			Game.Instance.showGasConduitDisease = base.InFilter(ToolParameterMenu.FILTERLAYERS.GASCONDUIT, this.legendFilters);
			Game.Instance.showLiquidConduitDisease = base.InFilter(ToolParameterMenu.FILTERLAYERS.LIQUIDCONDUIT, this.legendFilters);
		}

		// Token: 0x060092FA RID: 37626 RVA: 0x00395B44 File Offset: 0x00393D44
		protected override void OnSaveLoadRootRegistered(SaveLoadRoot item)
		{
			if (item == null)
			{
				return;
			}
			KBatchedAnimController component = item.GetComponent<KBatchedAnimController>();
			if (component == null)
			{
				return;
			}
			InfraredVisualizerComponents.ClearOverlayColour(component);
		}

		// Token: 0x060092FB RID: 37627 RVA: 0x000AA038 File Offset: 0x000A8238
		protected override void OnSaveLoadRootUnregistered(SaveLoadRoot item)
		{
		}

		// Token: 0x060092FC RID: 37628 RVA: 0x00395B74 File Offset: 0x00393D74
		public override void Disable()
		{
			foreach (DiseaseSourceVisualizer diseaseSourceVisualizer in Components.DiseaseSourceVisualizers.Items)
			{
				if (!(diseaseSourceVisualizer == null))
				{
					diseaseSourceVisualizer.Show(OverlayModes.None.ID);
				}
			}
			base.UnregisterSaveLoadListeners();
			Camera.main.cullingMask &= ~this.cameraLayerMask;
			foreach (KMonoBehaviour kmonoBehaviour in this.layerTargets)
			{
				if (!(kmonoBehaviour == null))
				{
					float defaultDepth = OverlayModes.Mode.GetDefaultDepth(kmonoBehaviour);
					Vector3 position = kmonoBehaviour.transform.GetPosition();
					position.z = defaultDepth;
					kmonoBehaviour.transform.SetPosition(position);
					KBatchedAnimController component = kmonoBehaviour.GetComponent<KBatchedAnimController>();
					component.enabled = false;
					component.enabled = true;
				}
			}
			CameraController.Instance.ToggleColouredOverlayView(false);
			Infrared.Instance.SetMode(Infrared.Mode.Disabled);
			Game.Instance.showGasConduitDisease = false;
			Game.Instance.showLiquidConduitDisease = false;
			this.freeDiseaseUI = 0;
			foreach (OverlayModes.Disease.UpdateDiseaseInfo updateDiseaseInfo in this.updateDiseaseInfo)
			{
				updateDiseaseInfo.ui.gameObject.SetActive(false);
			}
			this.updateDiseaseInfo.Clear();
			this.privateTargets.Clear();
			this.layerTargets.Clear();
		}

		// Token: 0x060092FD RID: 37629 RVA: 0x00395D18 File Offset: 0x00393F18
		public override List<LegendEntry> GetCustomLegendData()
		{
			List<LegendEntry> list = new List<LegendEntry>();
			List<OverlayModes.Disease.DiseaseSortInfo> list2 = new List<OverlayModes.Disease.DiseaseSortInfo>();
			foreach (Klei.AI.Disease d in Db.Get().Diseases.resources)
			{
				list2.Add(new OverlayModes.Disease.DiseaseSortInfo(d));
			}
			list2.Sort((OverlayModes.Disease.DiseaseSortInfo a, OverlayModes.Disease.DiseaseSortInfo b) => a.sortkey.CompareTo(b.sortkey));
			foreach (OverlayModes.Disease.DiseaseSortInfo diseaseSortInfo in list2)
			{
				list.Add(new LegendEntry(diseaseSortInfo.disease.Name, diseaseSortInfo.disease.overlayLegendHovertext.ToString(), GlobalAssets.Instance.colorSet.GetColorByName(diseaseSortInfo.disease.overlayColourName), null, null, true));
			}
			return list;
		}

		// Token: 0x060092FE RID: 37630 RVA: 0x00395E30 File Offset: 0x00394030
		public GameObject GetFreeDiseaseUI()
		{
			GameObject gameObject;
			if (this.freeDiseaseUI < this.diseaseUIList.Count)
			{
				gameObject = this.diseaseUIList[this.freeDiseaseUI];
				gameObject.gameObject.SetActive(true);
				this.freeDiseaseUI++;
			}
			else
			{
				gameObject = global::Util.KInstantiateUI(this.diseaseOverlayPrefab, this.diseaseUIParent.transform.gameObject, false);
				this.diseaseUIList.Add(gameObject);
				this.freeDiseaseUI++;
			}
			return gameObject;
		}

		// Token: 0x060092FF RID: 37631 RVA: 0x00395EB8 File Offset: 0x003940B8
		private void AddDiseaseUI(MinionIdentity target)
		{
			GameObject gameObject = this.GetFreeDiseaseUI();
			DiseaseOverlayWidget component = gameObject.GetComponent<DiseaseOverlayWidget>();
			AmountInstance amount_inst = target.GetComponent<Modifiers>().amounts.Get(Db.Get().Amounts.ImmuneLevel);
			OverlayModes.Disease.UpdateDiseaseInfo item = new OverlayModes.Disease.UpdateDiseaseInfo(amount_inst, component);
			KAnimControllerBase component2 = target.GetComponent<KAnimControllerBase>();
			Vector3 position = (component2 != null) ? component2.GetWorldPivot() : (target.transform.GetPosition() + Vector3.down);
			gameObject.GetComponent<RectTransform>().SetPosition(position);
			this.updateDiseaseInfo.Add(item);
		}

		// Token: 0x06009300 RID: 37632 RVA: 0x00395F44 File Offset: 0x00394144
		public override void Update()
		{
			Vector2I u;
			Vector2I v;
			Grid.GetVisibleExtents(out u, out v);
			using (new KProfiler.Region("UpdateDiseaseCarriers", null))
			{
				this.queuedAdds.Clear();
				foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.Items)
				{
					if (!(minionIdentity == null))
					{
						Vector2I vector2I = Grid.PosToXY(minionIdentity.transform.GetPosition());
						if (u <= vector2I && vector2I <= v && !this.privateTargets.Contains(minionIdentity))
						{
							this.AddDiseaseUI(minionIdentity);
							this.queuedAdds.Add(minionIdentity);
						}
					}
				}
				foreach (KMonoBehaviour item in this.queuedAdds)
				{
					this.privateTargets.Add(item);
				}
				this.queuedAdds.Clear();
			}
			foreach (OverlayModes.Disease.UpdateDiseaseInfo updateDiseaseInfo in this.updateDiseaseInfo)
			{
				updateDiseaseInfo.ui.Refresh(updateDiseaseInfo.valueSrc);
			}
			bool flag = false;
			if (Game.Instance.showLiquidConduitDisease)
			{
				using (HashSet<Tag>.Enumerator enumerator4 = OverlayScreen.LiquidVentIDs.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						Tag item2 = enumerator4.Current;
						if (!OverlayScreen.DiseaseIDs.Contains(item2))
						{
							OverlayScreen.DiseaseIDs.Add(item2);
							flag = true;
						}
					}
					goto IL_1F1;
				}
			}
			foreach (Tag item3 in OverlayScreen.LiquidVentIDs)
			{
				if (OverlayScreen.DiseaseIDs.Contains(item3))
				{
					OverlayScreen.DiseaseIDs.Remove(item3);
					flag = true;
				}
			}
			IL_1F1:
			if (Game.Instance.showGasConduitDisease)
			{
				using (HashSet<Tag>.Enumerator enumerator4 = OverlayScreen.GasVentIDs.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						Tag item4 = enumerator4.Current;
						if (!OverlayScreen.DiseaseIDs.Contains(item4))
						{
							OverlayScreen.DiseaseIDs.Add(item4);
							flag = true;
						}
					}
					goto IL_297;
				}
			}
			foreach (Tag item5 in OverlayScreen.GasVentIDs)
			{
				if (OverlayScreen.DiseaseIDs.Contains(item5))
				{
					OverlayScreen.DiseaseIDs.Remove(item5);
					flag = true;
				}
			}
			IL_297:
			if (flag)
			{
				this.SetLayerZ(-50f);
			}
		}

		// Token: 0x06009301 RID: 37633 RVA: 0x0039625C File Offset: 0x0039445C
		private void SetLayerZ(float offset_z)
		{
			Vector2I vector2I;
			Vector2I vector2I2;
			Grid.GetVisibleExtents(out vector2I, out vector2I2);
			OverlayModes.Mode.ClearOutsideViewObjects<KMonoBehaviour>(this.layerTargets, vector2I, vector2I2, OverlayScreen.DiseaseIDs, delegate(KMonoBehaviour go)
			{
				if (go != null)
				{
					float defaultDepth2 = OverlayModes.Mode.GetDefaultDepth(go);
					Vector3 position2 = go.transform.GetPosition();
					position2.z = defaultDepth2;
					go.transform.SetPosition(position2);
					KBatchedAnimController component2 = go.GetComponent<KBatchedAnimController>();
					component2.enabled = false;
					component2.enabled = true;
				}
			});
			Dictionary<Tag, List<SaveLoadRoot>> lists = SaveLoader.Instance.saveManager.GetLists();
			foreach (Tag key in OverlayScreen.DiseaseIDs)
			{
				List<SaveLoadRoot> list;
				if (lists.TryGetValue(key, out list))
				{
					foreach (KMonoBehaviour kmonoBehaviour in list)
					{
						if (!(kmonoBehaviour == null) && !this.layerTargets.Contains(kmonoBehaviour))
						{
							Vector3 position = kmonoBehaviour.transform.GetPosition();
							if (Grid.IsVisible(Grid.PosToCell(position)) && vector2I <= position && position <= vector2I2)
							{
								float defaultDepth = OverlayModes.Mode.GetDefaultDepth(kmonoBehaviour);
								position.z = defaultDepth + offset_z;
								kmonoBehaviour.transform.SetPosition(position);
								KBatchedAnimController component = kmonoBehaviour.GetComponent<KBatchedAnimController>();
								component.enabled = false;
								component.enabled = true;
								this.layerTargets.Add(kmonoBehaviour);
							}
						}
					}
				}
			}
		}

		// Token: 0x04006F58 RID: 28504
		public static readonly HashedString ID = "Disease";

		// Token: 0x04006F59 RID: 28505
		private int cameraLayerMask;

		// Token: 0x04006F5A RID: 28506
		private int freeDiseaseUI;

		// Token: 0x04006F5B RID: 28507
		private List<GameObject> diseaseUIList = new List<GameObject>();

		// Token: 0x04006F5C RID: 28508
		private List<OverlayModes.Disease.UpdateDiseaseInfo> updateDiseaseInfo = new List<OverlayModes.Disease.UpdateDiseaseInfo>();

		// Token: 0x04006F5D RID: 28509
		private HashSet<KMonoBehaviour> layerTargets = new HashSet<KMonoBehaviour>();

		// Token: 0x04006F5E RID: 28510
		private HashSet<KMonoBehaviour> privateTargets = new HashSet<KMonoBehaviour>();

		// Token: 0x04006F5F RID: 28511
		private List<KMonoBehaviour> queuedAdds = new List<KMonoBehaviour>();

		// Token: 0x04006F60 RID: 28512
		private Canvas diseaseUIParent;

		// Token: 0x04006F61 RID: 28513
		private GameObject diseaseOverlayPrefab;

		// Token: 0x02001B5E RID: 7006
		private struct DiseaseSortInfo
		{
			// Token: 0x06009303 RID: 37635 RVA: 0x001049D7 File Offset: 0x00102BD7
			public DiseaseSortInfo(Klei.AI.Disease d)
			{
				this.disease = d;
				this.sortkey = OverlayModes.Disease.CalculateHUE(GlobalAssets.Instance.colorSet.GetColorByName(d.overlayColourName));
			}

			// Token: 0x04006F62 RID: 28514
			public float sortkey;

			// Token: 0x04006F63 RID: 28515
			public Klei.AI.Disease disease;
		}

		// Token: 0x02001B5F RID: 7007
		private struct UpdateDiseaseInfo
		{
			// Token: 0x06009304 RID: 37636 RVA: 0x00104A00 File Offset: 0x00102C00
			public UpdateDiseaseInfo(AmountInstance amount_inst, DiseaseOverlayWidget ui)
			{
				this.ui = ui;
				this.valueSrc = amount_inst;
			}

			// Token: 0x04006F64 RID: 28516
			public DiseaseOverlayWidget ui;

			// Token: 0x04006F65 RID: 28517
			public AmountInstance valueSrc;
		}
	}

	// Token: 0x02001B61 RID: 7009
	public class Logic : OverlayModes.Mode
	{
		// Token: 0x06009309 RID: 37641 RVA: 0x00104A30 File Offset: 0x00102C30
		public override HashedString ViewMode()
		{
			return OverlayModes.Logic.ID;
		}

		// Token: 0x0600930A RID: 37642 RVA: 0x00104A37 File Offset: 0x00102C37
		public override string GetSoundName()
		{
			return "Logic";
		}

		// Token: 0x0600930B RID: 37643 RVA: 0x00396438 File Offset: 0x00394638
		public override List<LegendEntry> GetCustomLegendData()
		{
			return new List<LegendEntry>
			{
				new LegendEntry(UI.OVERLAYS.LOGIC.INPUT, UI.OVERLAYS.LOGIC.TOOLTIPS.INPUT, Color.white, null, Assets.GetSprite("logicInput"), true),
				new LegendEntry(UI.OVERLAYS.LOGIC.OUTPUT, UI.OVERLAYS.LOGIC.TOOLTIPS.OUTPUT, Color.white, null, Assets.GetSprite("logicOutput"), true),
				new LegendEntry(UI.OVERLAYS.LOGIC.RIBBON_INPUT, UI.OVERLAYS.LOGIC.TOOLTIPS.RIBBON_INPUT, Color.white, null, Assets.GetSprite("logic_ribbon_all_in"), true),
				new LegendEntry(UI.OVERLAYS.LOGIC.RIBBON_OUTPUT, UI.OVERLAYS.LOGIC.TOOLTIPS.RIBBON_OUTPUT, Color.white, null, Assets.GetSprite("logic_ribbon_all_out"), true),
				new LegendEntry(UI.OVERLAYS.LOGIC.RESET_UPDATE, UI.OVERLAYS.LOGIC.TOOLTIPS.RESET_UPDATE, Color.white, null, Assets.GetSprite("logicResetUpdate"), true),
				new LegendEntry(UI.OVERLAYS.LOGIC.CONTROL_INPUT, UI.OVERLAYS.LOGIC.TOOLTIPS.CONTROL_INPUT, Color.white, null, Assets.GetSprite("control_input_frame_legend"), true),
				new LegendEntry(UI.OVERLAYS.LOGIC.CIRCUIT_STATUS_HEADER, null, Color.white, null, null, false),
				new LegendEntry(UI.OVERLAYS.LOGIC.ONE, null, GlobalAssets.Instance.colorSet.logicOnText, null, null, true),
				new LegendEntry(UI.OVERLAYS.LOGIC.ZERO, null, GlobalAssets.Instance.colorSet.logicOffText, null, null, true),
				new LegendEntry(UI.OVERLAYS.LOGIC.DISCONNECTED, UI.OVERLAYS.LOGIC.TOOLTIPS.DISCONNECTED, GlobalAssets.Instance.colorSet.logicDisconnected, null, null, true)
			};
		}

		// Token: 0x0600930C RID: 37644 RVA: 0x00396638 File Offset: 0x00394838
		public Logic(LogicModeUI ui_asset)
		{
			this.conduitTargetLayer = LayerMask.NameToLayer("MaskedOverlay");
			this.objectTargetLayer = LayerMask.NameToLayer("MaskedOverlayBG");
			this.cameraLayerMask = LayerMask.GetMask(new string[]
			{
				"MaskedOverlay",
				"MaskedOverlayBG"
			});
			this.selectionMask = this.cameraLayerMask;
			this.uiAsset = ui_asset;
		}

		// Token: 0x0600930D RID: 37645 RVA: 0x0039671C File Offset: 0x0039491C
		public override void Enable()
		{
			Camera.main.cullingMask |= this.cameraLayerMask;
			SelectTool.Instance.SetLayerMask(this.selectionMask);
			base.RegisterSaveLoadListeners();
			this.gameObjPartition = OverlayModes.Mode.PopulatePartition<SaveLoadRoot>(OverlayModes.Logic.HighlightItemIDs);
			this.ioPartition = this.CreateLogicUIPartition();
			GridCompositor.Instance.ToggleMinor(true);
			LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
			logicCircuitManager.onElemAdded = (Action<ILogicUIElement>)Delegate.Combine(logicCircuitManager.onElemAdded, new Action<ILogicUIElement>(this.OnUIElemAdded));
			LogicCircuitManager logicCircuitManager2 = Game.Instance.logicCircuitManager;
			logicCircuitManager2.onElemRemoved = (Action<ILogicUIElement>)Delegate.Combine(logicCircuitManager2.onElemRemoved, new Action<ILogicUIElement>(this.OnUIElemRemoved));
			AudioMixer.instance.Start(AudioMixerSnapshots.Get().TechFilterLogicOn);
		}

		// Token: 0x0600930E RID: 37646 RVA: 0x003967E8 File Offset: 0x003949E8
		public override void Disable()
		{
			LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
			logicCircuitManager.onElemAdded = (Action<ILogicUIElement>)Delegate.Remove(logicCircuitManager.onElemAdded, new Action<ILogicUIElement>(this.OnUIElemAdded));
			LogicCircuitManager logicCircuitManager2 = Game.Instance.logicCircuitManager;
			logicCircuitManager2.onElemRemoved = (Action<ILogicUIElement>)Delegate.Remove(logicCircuitManager2.onElemRemoved, new Action<ILogicUIElement>(this.OnUIElemRemoved));
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().TechFilterLogicOn, STOP_MODE.ALLOWFADEOUT);
			foreach (SaveLoadRoot saveLoadRoot in this.gameObjTargets)
			{
				float defaultDepth = OverlayModes.Mode.GetDefaultDepth(saveLoadRoot);
				Vector3 position = saveLoadRoot.transform.GetPosition();
				position.z = defaultDepth;
				saveLoadRoot.transform.SetPosition(position);
				saveLoadRoot.GetComponent<KBatchedAnimController>().enabled = false;
				saveLoadRoot.GetComponent<KBatchedAnimController>().enabled = true;
			}
			OverlayModes.Mode.ResetDisplayValues<SaveLoadRoot>(this.gameObjTargets);
			OverlayModes.Mode.ResetDisplayValues<KBatchedAnimController>(this.wireControllers);
			OverlayModes.Mode.ResetDisplayValues<KBatchedAnimController>(this.ribbonControllers);
			this.ResetRibbonSymbolTints<KBatchedAnimController>(this.ribbonControllers);
			foreach (OverlayModes.Logic.BridgeInfo bridgeInfo in this.bridgeControllers)
			{
				if (bridgeInfo.controller != null)
				{
					OverlayModes.Mode.ResetDisplayValues(bridgeInfo.controller);
				}
			}
			foreach (OverlayModes.Logic.BridgeInfo bridgeInfo2 in this.ribbonBridgeControllers)
			{
				if (bridgeInfo2.controller != null)
				{
					this.ResetRibbonTint(bridgeInfo2.controller);
				}
			}
			Camera.main.cullingMask &= ~this.cameraLayerMask;
			SelectTool.Instance.ClearLayerMask();
			base.UnregisterSaveLoadListeners();
			foreach (OverlayModes.Logic.UIInfo uiinfo in this.uiInfo.GetDataList())
			{
				uiinfo.Release();
			}
			this.uiInfo.Clear();
			this.uiNodes.Clear();
			this.ioPartition.Clear();
			this.ioTargets.Clear();
			this.gameObjPartition.Clear();
			this.gameObjTargets.Clear();
			this.wireControllers.Clear();
			this.ribbonControllers.Clear();
			this.bridgeControllers.Clear();
			this.ribbonBridgeControllers.Clear();
			GridCompositor.Instance.ToggleMinor(false);
		}

		// Token: 0x0600930F RID: 37647 RVA: 0x00396AA4 File Offset: 0x00394CA4
		protected override void OnSaveLoadRootRegistered(SaveLoadRoot item)
		{
			Tag saveLoadTag = item.GetComponent<KPrefabID>().GetSaveLoadTag();
			if (OverlayModes.Logic.HighlightItemIDs.Contains(saveLoadTag))
			{
				this.gameObjPartition.Add(item);
			}
		}

		// Token: 0x06009310 RID: 37648 RVA: 0x00396AD8 File Offset: 0x00394CD8
		protected override void OnSaveLoadRootUnregistered(SaveLoadRoot item)
		{
			if (item == null || item.gameObject == null)
			{
				return;
			}
			if (this.gameObjTargets.Contains(item))
			{
				this.gameObjTargets.Remove(item);
			}
			this.gameObjPartition.Remove(item);
		}

		// Token: 0x06009311 RID: 37649 RVA: 0x00104A3E File Offset: 0x00102C3E
		private void OnUIElemAdded(ILogicUIElement elem)
		{
			this.ioPartition.Add(elem);
		}

		// Token: 0x06009312 RID: 37650 RVA: 0x00104A4C File Offset: 0x00102C4C
		private void OnUIElemRemoved(ILogicUIElement elem)
		{
			this.ioPartition.Remove(elem);
			if (this.ioTargets.Contains(elem))
			{
				this.ioTargets.Remove(elem);
				this.FreeUI(elem);
			}
		}

		// Token: 0x06009313 RID: 37651 RVA: 0x00396B24 File Offset: 0x00394D24
		public override void Update()
		{
			Vector2I vector2I;
			Vector2I vector2I2;
			Grid.GetVisibleExtents(out vector2I, out vector2I2);
			Tag wire_id = TagManager.Create("LogicWire");
			Tag ribbon_id = TagManager.Create("LogicRibbon");
			Tag bridge_id = TagManager.Create("LogicWireBridge");
			Tag ribbon_bridge_id = TagManager.Create("LogicRibbonBridge");
			OverlayModes.Mode.RemoveOffscreenTargets<SaveLoadRoot>(this.gameObjTargets, vector2I, vector2I2, delegate(SaveLoadRoot root)
			{
				if (root == null)
				{
					return;
				}
				KPrefabID component7 = root.GetComponent<KPrefabID>();
				if (component7 != null)
				{
					Tag prefabTag = component7.PrefabTag;
					if (prefabTag == wire_id)
					{
						this.wireControllers.Remove(root.GetComponent<KBatchedAnimController>());
						return;
					}
					if (prefabTag == ribbon_id)
					{
						this.ResetRibbonTint(root.GetComponent<KBatchedAnimController>());
						this.ribbonControllers.Remove(root.GetComponent<KBatchedAnimController>());
						return;
					}
					if (prefabTag == bridge_id)
					{
						KBatchedAnimController controller = root.GetComponent<KBatchedAnimController>();
						this.bridgeControllers.RemoveWhere((OverlayModes.Logic.BridgeInfo x) => x.controller == controller);
						return;
					}
					if (prefabTag == ribbon_bridge_id)
					{
						KBatchedAnimController controller = root.GetComponent<KBatchedAnimController>();
						this.ResetRibbonTint(controller);
						this.ribbonBridgeControllers.RemoveWhere((OverlayModes.Logic.BridgeInfo x) => x.controller == controller);
						return;
					}
					float defaultDepth = OverlayModes.Mode.GetDefaultDepth(root);
					Vector3 position = root.transform.GetPosition();
					position.z = defaultDepth;
					root.transform.SetPosition(position);
					root.GetComponent<KBatchedAnimController>().enabled = false;
					root.GetComponent<KBatchedAnimController>().enabled = true;
				}
			});
			OverlayModes.Mode.RemoveOffscreenTargets<ILogicUIElement>(this.ioTargets, this.workingIOTargets, vector2I, vector2I2, new Action<ILogicUIElement>(this.FreeUI), null);
			using (new KProfiler.Region("UpdateLogicOverlay", null))
			{
				Action<SaveLoadRoot> <>9__3;
				foreach (object obj in this.gameObjPartition.GetAllIntersecting(new Vector2((float)vector2I.x, (float)vector2I.y), new Vector2((float)vector2I2.x, (float)vector2I2.y)))
				{
					SaveLoadRoot saveLoadRoot = (SaveLoadRoot)obj;
					if (saveLoadRoot != null)
					{
						KPrefabID component = saveLoadRoot.GetComponent<KPrefabID>();
						if (component.PrefabTag == wire_id || component.PrefabTag == bridge_id || component.PrefabTag == ribbon_id || component.PrefabTag == ribbon_bridge_id)
						{
							SaveLoadRoot instance = saveLoadRoot;
							Vector2I vis_min = vector2I;
							Vector2I vis_max = vector2I2;
							ICollection<SaveLoadRoot> targets = this.gameObjTargets;
							int layer = this.conduitTargetLayer;
							Action<SaveLoadRoot> on_added;
							if ((on_added = <>9__3) == null)
							{
								on_added = (<>9__3 = delegate(SaveLoadRoot root)
								{
									if (root == null)
									{
										return;
									}
									KPrefabID component7 = root.GetComponent<KPrefabID>();
									if (OverlayModes.Logic.HighlightItemIDs.Contains(component7.PrefabTag))
									{
										if (component7.PrefabTag == wire_id)
										{
											this.wireControllers.Add(root.GetComponent<KBatchedAnimController>());
											return;
										}
										if (component7.PrefabTag == ribbon_id)
										{
											this.ribbonControllers.Add(root.GetComponent<KBatchedAnimController>());
											return;
										}
										if (component7.PrefabTag == bridge_id)
										{
											KBatchedAnimController component8 = root.GetComponent<KBatchedAnimController>();
											int networkCell2 = root.GetComponent<LogicUtilityNetworkLink>().GetNetworkCell();
											this.bridgeControllers.Add(new OverlayModes.Logic.BridgeInfo
											{
												cell = networkCell2,
												controller = component8
											});
											return;
										}
										if (component7.PrefabTag == ribbon_bridge_id)
										{
											KBatchedAnimController component9 = root.GetComponent<KBatchedAnimController>();
											int networkCell3 = root.GetComponent<LogicUtilityNetworkLink>().GetNetworkCell();
											this.ribbonBridgeControllers.Add(new OverlayModes.Logic.BridgeInfo
											{
												cell = networkCell3,
												controller = component9
											});
										}
									}
								});
							}
							base.AddTargetIfVisible<SaveLoadRoot>(instance, vis_min, vis_max, targets, layer, on_added, null);
						}
						else
						{
							base.AddTargetIfVisible<SaveLoadRoot>(saveLoadRoot, vector2I, vector2I2, this.gameObjTargets, this.objectTargetLayer, delegate(SaveLoadRoot root)
							{
								Vector3 position = root.transform.GetPosition();
								float z = position.z;
								KPrefabID component7 = root.GetComponent<KPrefabID>();
								if (component7 != null)
								{
									if (component7.HasTag(GameTags.OverlayInFrontOfConduits))
									{
										z = Grid.GetLayerZ(Grid.SceneLayer.LogicWires) - 0.2f;
									}
									else if (component7.HasTag(GameTags.OverlayBehindConduits))
									{
										z = Grid.GetLayerZ(Grid.SceneLayer.LogicWires) + 0.2f;
									}
								}
								position.z = z;
								root.transform.SetPosition(position);
								KBatchedAnimController component8 = root.GetComponent<KBatchedAnimController>();
								component8.enabled = false;
								component8.enabled = true;
							}, null);
						}
					}
				}
				foreach (object obj2 in this.ioPartition.GetAllIntersecting(new Vector2((float)vector2I.x, (float)vector2I.y), new Vector2((float)vector2I2.x, (float)vector2I2.y)))
				{
					ILogicUIElement logicUIElement = (ILogicUIElement)obj2;
					if (logicUIElement != null)
					{
						base.AddTargetIfVisible<ILogicUIElement>(logicUIElement, vector2I, vector2I2, this.ioTargets, this.objectTargetLayer, new Action<ILogicUIElement>(this.AddUI), (KMonoBehaviour kcmp) => kcmp != null && OverlayModes.Logic.HighlightItemIDs.Contains(kcmp.GetComponent<KPrefabID>().PrefabTag));
					}
				}
				this.connectedNetworks.Clear();
				float num = 1f;
				GameObject gameObject = null;
				if (SelectTool.Instance != null && SelectTool.Instance.hover != null)
				{
					gameObject = SelectTool.Instance.hover.gameObject;
				}
				if (gameObject != null)
				{
					IBridgedNetworkItem component2 = gameObject.GetComponent<IBridgedNetworkItem>();
					if (component2 != null)
					{
						int networkCell = component2.GetNetworkCell();
						this.visited.Clear();
						this.FindConnectedNetworks(networkCell, Game.Instance.logicCircuitSystem, this.connectedNetworks, this.visited);
						this.visited.Clear();
						num = OverlayModes.ModeUtil.GetHighlightScale();
					}
				}
				LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
				Color32 logicOn = GlobalAssets.Instance.colorSet.logicOn;
				Color32 logicOff = GlobalAssets.Instance.colorSet.logicOff;
				logicOff.a = (logicOn.a = 0);
				foreach (KBatchedAnimController kbatchedAnimController in this.wireControllers)
				{
					if (!(kbatchedAnimController == null))
					{
						Color32 color = logicOff;
						LogicCircuitNetwork networkForCell = logicCircuitManager.GetNetworkForCell(Grid.PosToCell(kbatchedAnimController.transform.GetPosition()));
						if (networkForCell != null)
						{
							color = (networkForCell.IsBitActive(0) ? logicOn : logicOff);
						}
						if (this.connectedNetworks.Count > 0)
						{
							IBridgedNetworkItem component3 = kbatchedAnimController.GetComponent<IBridgedNetworkItem>();
							if (component3 != null && component3.IsConnectedToNetworks(this.connectedNetworks))
							{
								color.r = (byte)((float)color.r * num);
								color.g = (byte)((float)color.g * num);
								color.b = (byte)((float)color.b * num);
							}
						}
						kbatchedAnimController.TintColour = color;
					}
				}
				foreach (KBatchedAnimController kbatchedAnimController2 in this.ribbonControllers)
				{
					if (!(kbatchedAnimController2 == null))
					{
						Color32 color2 = logicOff;
						Color32 color3 = logicOff;
						Color32 color4 = logicOff;
						Color32 color5 = logicOff;
						LogicCircuitNetwork networkForCell2 = logicCircuitManager.GetNetworkForCell(Grid.PosToCell(kbatchedAnimController2.transform.GetPosition()));
						if (networkForCell2 != null)
						{
							color2 = (networkForCell2.IsBitActive(0) ? logicOn : logicOff);
							color3 = (networkForCell2.IsBitActive(1) ? logicOn : logicOff);
							color4 = (networkForCell2.IsBitActive(2) ? logicOn : logicOff);
							color5 = (networkForCell2.IsBitActive(3) ? logicOn : logicOff);
						}
						if (this.connectedNetworks.Count > 0)
						{
							IBridgedNetworkItem component4 = kbatchedAnimController2.GetComponent<IBridgedNetworkItem>();
							if (component4 != null && component4.IsConnectedToNetworks(this.connectedNetworks))
							{
								color2.r = (byte)((float)color2.r * num);
								color2.g = (byte)((float)color2.g * num);
								color2.b = (byte)((float)color2.b * num);
								color3.r = (byte)((float)color3.r * num);
								color3.g = (byte)((float)color3.g * num);
								color3.b = (byte)((float)color3.b * num);
								color4.r = (byte)((float)color4.r * num);
								color4.g = (byte)((float)color4.g * num);
								color4.b = (byte)((float)color4.b * num);
								color5.r = (byte)((float)color5.r * num);
								color5.g = (byte)((float)color5.g * num);
								color5.b = (byte)((float)color5.b * num);
							}
						}
						kbatchedAnimController2.SetSymbolTint(OverlayModes.Logic.RIBBON_WIRE_1_SYMBOL_NAME, color2);
						kbatchedAnimController2.SetSymbolTint(OverlayModes.Logic.RIBBON_WIRE_2_SYMBOL_NAME, color3);
						kbatchedAnimController2.SetSymbolTint(OverlayModes.Logic.RIBBON_WIRE_3_SYMBOL_NAME, color4);
						kbatchedAnimController2.SetSymbolTint(OverlayModes.Logic.RIBBON_WIRE_4_SYMBOL_NAME, color5);
					}
				}
				foreach (OverlayModes.Logic.BridgeInfo bridgeInfo in this.bridgeControllers)
				{
					if (!(bridgeInfo.controller == null))
					{
						Color32 color6 = logicOff;
						LogicCircuitNetwork networkForCell3 = logicCircuitManager.GetNetworkForCell(bridgeInfo.cell);
						if (networkForCell3 != null)
						{
							color6 = (networkForCell3.IsBitActive(0) ? logicOn : logicOff);
						}
						if (this.connectedNetworks.Count > 0)
						{
							IBridgedNetworkItem component5 = bridgeInfo.controller.GetComponent<IBridgedNetworkItem>();
							if (component5 != null && component5.IsConnectedToNetworks(this.connectedNetworks))
							{
								color6.r = (byte)((float)color6.r * num);
								color6.g = (byte)((float)color6.g * num);
								color6.b = (byte)((float)color6.b * num);
							}
						}
						bridgeInfo.controller.TintColour = color6;
					}
				}
				foreach (OverlayModes.Logic.BridgeInfo bridgeInfo2 in this.ribbonBridgeControllers)
				{
					if (!(bridgeInfo2.controller == null))
					{
						Color32 color7 = logicOff;
						Color32 color8 = logicOff;
						Color32 color9 = logicOff;
						Color32 color10 = logicOff;
						LogicCircuitNetwork networkForCell4 = logicCircuitManager.GetNetworkForCell(bridgeInfo2.cell);
						if (networkForCell4 != null)
						{
							color7 = (networkForCell4.IsBitActive(0) ? logicOn : logicOff);
							color8 = (networkForCell4.IsBitActive(1) ? logicOn : logicOff);
							color9 = (networkForCell4.IsBitActive(2) ? logicOn : logicOff);
							color10 = (networkForCell4.IsBitActive(3) ? logicOn : logicOff);
						}
						if (this.connectedNetworks.Count > 0)
						{
							IBridgedNetworkItem component6 = bridgeInfo2.controller.GetComponent<IBridgedNetworkItem>();
							if (component6 != null && component6.IsConnectedToNetworks(this.connectedNetworks))
							{
								color7.r = (byte)((float)color7.r * num);
								color7.g = (byte)((float)color7.g * num);
								color7.b = (byte)((float)color7.b * num);
								color8.r = (byte)((float)color8.r * num);
								color8.g = (byte)((float)color8.g * num);
								color8.b = (byte)((float)color8.b * num);
								color9.r = (byte)((float)color9.r * num);
								color9.g = (byte)((float)color9.g * num);
								color9.b = (byte)((float)color9.b * num);
								color10.r = (byte)((float)color10.r * num);
								color10.g = (byte)((float)color10.g * num);
								color10.b = (byte)((float)color10.b * num);
							}
						}
						bridgeInfo2.controller.SetSymbolTint(OverlayModes.Logic.RIBBON_WIRE_1_SYMBOL_NAME, color7);
						bridgeInfo2.controller.SetSymbolTint(OverlayModes.Logic.RIBBON_WIRE_2_SYMBOL_NAME, color8);
						bridgeInfo2.controller.SetSymbolTint(OverlayModes.Logic.RIBBON_WIRE_3_SYMBOL_NAME, color9);
						bridgeInfo2.controller.SetSymbolTint(OverlayModes.Logic.RIBBON_WIRE_4_SYMBOL_NAME, color10);
					}
				}
			}
			this.UpdateUI();
		}

		// Token: 0x06009314 RID: 37652 RVA: 0x0039757C File Offset: 0x0039577C
		private void UpdateUI()
		{
			Color32 logicOn = GlobalAssets.Instance.colorSet.logicOn;
			Color32 logicOff = GlobalAssets.Instance.colorSet.logicOff;
			Color32 logicDisconnected = GlobalAssets.Instance.colorSet.logicDisconnected;
			logicOff.a = (logicOn.a = byte.MaxValue);
			foreach (OverlayModes.Logic.UIInfo uiinfo in this.uiInfo.GetDataList())
			{
				LogicCircuitNetwork networkForCell = Game.Instance.logicCircuitManager.GetNetworkForCell(uiinfo.cell);
				Color32 c = logicDisconnected;
				LogicControlInputUI component = uiinfo.instance.GetComponent<LogicControlInputUI>();
				if (component != null)
				{
					component.SetContent(networkForCell);
				}
				else if (uiinfo.bitDepth == 4)
				{
					LogicRibbonDisplayUI component2 = uiinfo.instance.GetComponent<LogicRibbonDisplayUI>();
					if (component2 != null)
					{
						component2.SetContent(networkForCell);
					}
				}
				else if (uiinfo.bitDepth == 1)
				{
					if (networkForCell != null)
					{
						c = (networkForCell.IsBitActive(0) ? logicOn : logicOff);
					}
					if (uiinfo.image.color != c)
					{
						uiinfo.image.color = c;
					}
				}
			}
		}

		// Token: 0x06009315 RID: 37653 RVA: 0x003976D4 File Offset: 0x003958D4
		private void AddUI(ILogicUIElement ui_elem)
		{
			if (this.uiNodes.ContainsKey(ui_elem))
			{
				return;
			}
			HandleVector<int>.Handle uiHandle = this.uiInfo.Allocate(new OverlayModes.Logic.UIInfo(ui_elem, this.uiAsset));
			this.uiNodes.Add(ui_elem, new OverlayModes.Logic.EventInfo
			{
				uiHandle = uiHandle
			});
		}

		// Token: 0x06009316 RID: 37654 RVA: 0x00397728 File Offset: 0x00395928
		private void FreeUI(ILogicUIElement item)
		{
			if (item == null)
			{
				return;
			}
			OverlayModes.Logic.EventInfo eventInfo;
			if (this.uiNodes.TryGetValue(item, out eventInfo))
			{
				this.uiInfo.GetData(eventInfo.uiHandle).Release();
				this.uiInfo.Free(eventInfo.uiHandle);
				this.uiNodes.Remove(item);
			}
		}

		// Token: 0x06009317 RID: 37655 RVA: 0x00397784 File Offset: 0x00395984
		protected UniformGrid<ILogicUIElement> CreateLogicUIPartition()
		{
			UniformGrid<ILogicUIElement> uniformGrid = new UniformGrid<ILogicUIElement>(Grid.WidthInCells, Grid.HeightInCells, 8, 8);
			foreach (ILogicUIElement logicUIElement in Game.Instance.logicCircuitManager.GetVisElements())
			{
				if (logicUIElement != null)
				{
					uniformGrid.Add(logicUIElement);
				}
			}
			return uniformGrid;
		}

		// Token: 0x06009318 RID: 37656 RVA: 0x00104A7C File Offset: 0x00102C7C
		private bool IsBitActive(int value, int bit)
		{
			return (value & 1 << bit) > 0;
		}

		// Token: 0x06009319 RID: 37657 RVA: 0x003977F8 File Offset: 0x003959F8
		private void FindConnectedNetworks(int cell, IUtilityNetworkMgr mgr, ICollection<UtilityNetwork> networks, List<int> visited)
		{
			if (visited.Contains(cell))
			{
				return;
			}
			visited.Add(cell);
			UtilityNetwork networkForCell = mgr.GetNetworkForCell(cell);
			if (networkForCell != null)
			{
				networks.Add(networkForCell);
				UtilityConnections connections = mgr.GetConnections(cell, false);
				if ((connections & UtilityConnections.Right) != (UtilityConnections)0)
				{
					this.FindConnectedNetworks(Grid.CellRight(cell), mgr, networks, visited);
				}
				if ((connections & UtilityConnections.Left) != (UtilityConnections)0)
				{
					this.FindConnectedNetworks(Grid.CellLeft(cell), mgr, networks, visited);
				}
				if ((connections & UtilityConnections.Up) != (UtilityConnections)0)
				{
					this.FindConnectedNetworks(Grid.CellAbove(cell), mgr, networks, visited);
				}
				if ((connections & UtilityConnections.Down) != (UtilityConnections)0)
				{
					this.FindConnectedNetworks(Grid.CellBelow(cell), mgr, networks, visited);
				}
			}
		}

		// Token: 0x0600931A RID: 37658 RVA: 0x00397888 File Offset: 0x00395A88
		private void ResetRibbonSymbolTints<T>(ICollection<T> targets) where T : MonoBehaviour
		{
			foreach (T t in targets)
			{
				if (!(t == null))
				{
					KBatchedAnimController component = t.GetComponent<KBatchedAnimController>();
					this.ResetRibbonTint(component);
				}
			}
		}

		// Token: 0x0600931B RID: 37659 RVA: 0x003978EC File Offset: 0x00395AEC
		private void ResetRibbonTint(KBatchedAnimController kbac)
		{
			if (kbac != null)
			{
				kbac.SetSymbolTint(OverlayModes.Logic.RIBBON_WIRE_1_SYMBOL_NAME, Color.white);
				kbac.SetSymbolTint(OverlayModes.Logic.RIBBON_WIRE_2_SYMBOL_NAME, Color.white);
				kbac.SetSymbolTint(OverlayModes.Logic.RIBBON_WIRE_3_SYMBOL_NAME, Color.white);
				kbac.SetSymbolTint(OverlayModes.Logic.RIBBON_WIRE_4_SYMBOL_NAME, Color.white);
			}
		}

		// Token: 0x04006F69 RID: 28521
		public static readonly HashedString ID = "Logic";

		// Token: 0x04006F6A RID: 28522
		public static HashSet<Tag> HighlightItemIDs = new HashSet<Tag>();

		// Token: 0x04006F6B RID: 28523
		public static KAnimHashedString RIBBON_WIRE_1_SYMBOL_NAME = "wire1";

		// Token: 0x04006F6C RID: 28524
		public static KAnimHashedString RIBBON_WIRE_2_SYMBOL_NAME = "wire2";

		// Token: 0x04006F6D RID: 28525
		public static KAnimHashedString RIBBON_WIRE_3_SYMBOL_NAME = "wire3";

		// Token: 0x04006F6E RID: 28526
		public static KAnimHashedString RIBBON_WIRE_4_SYMBOL_NAME = "wire4";

		// Token: 0x04006F6F RID: 28527
		private int conduitTargetLayer;

		// Token: 0x04006F70 RID: 28528
		private int objectTargetLayer;

		// Token: 0x04006F71 RID: 28529
		private int cameraLayerMask;

		// Token: 0x04006F72 RID: 28530
		private int selectionMask;

		// Token: 0x04006F73 RID: 28531
		private UniformGrid<ILogicUIElement> ioPartition;

		// Token: 0x04006F74 RID: 28532
		private HashSet<ILogicUIElement> ioTargets = new HashSet<ILogicUIElement>();

		// Token: 0x04006F75 RID: 28533
		private HashSet<ILogicUIElement> workingIOTargets = new HashSet<ILogicUIElement>();

		// Token: 0x04006F76 RID: 28534
		private HashSet<KBatchedAnimController> wireControllers = new HashSet<KBatchedAnimController>();

		// Token: 0x04006F77 RID: 28535
		private HashSet<KBatchedAnimController> ribbonControllers = new HashSet<KBatchedAnimController>();

		// Token: 0x04006F78 RID: 28536
		private HashSet<UtilityNetwork> connectedNetworks = new HashSet<UtilityNetwork>();

		// Token: 0x04006F79 RID: 28537
		private List<int> visited = new List<int>();

		// Token: 0x04006F7A RID: 28538
		private HashSet<OverlayModes.Logic.BridgeInfo> bridgeControllers = new HashSet<OverlayModes.Logic.BridgeInfo>();

		// Token: 0x04006F7B RID: 28539
		private HashSet<OverlayModes.Logic.BridgeInfo> ribbonBridgeControllers = new HashSet<OverlayModes.Logic.BridgeInfo>();

		// Token: 0x04006F7C RID: 28540
		private UniformGrid<SaveLoadRoot> gameObjPartition;

		// Token: 0x04006F7D RID: 28541
		private HashSet<SaveLoadRoot> gameObjTargets = new HashSet<SaveLoadRoot>();

		// Token: 0x04006F7E RID: 28542
		private LogicModeUI uiAsset;

		// Token: 0x04006F7F RID: 28543
		private Dictionary<ILogicUIElement, OverlayModes.Logic.EventInfo> uiNodes = new Dictionary<ILogicUIElement, OverlayModes.Logic.EventInfo>();

		// Token: 0x04006F80 RID: 28544
		private KCompactedVector<OverlayModes.Logic.UIInfo> uiInfo = new KCompactedVector<OverlayModes.Logic.UIInfo>(0);

		// Token: 0x02001B62 RID: 7010
		private struct BridgeInfo
		{
			// Token: 0x04006F81 RID: 28545
			public int cell;

			// Token: 0x04006F82 RID: 28546
			public KBatchedAnimController controller;
		}

		// Token: 0x02001B63 RID: 7011
		private struct EventInfo
		{
			// Token: 0x04006F83 RID: 28547
			public HandleVector<int>.Handle uiHandle;
		}

		// Token: 0x02001B64 RID: 7012
		private struct UIInfo
		{
			// Token: 0x0600931D RID: 37661 RVA: 0x003979A8 File Offset: 0x00395BA8
			public UIInfo(ILogicUIElement ui_elem, LogicModeUI ui_data)
			{
				this.cell = ui_elem.GetLogicUICell();
				GameObject original = null;
				Sprite sprite = null;
				this.bitDepth = 1;
				switch (ui_elem.GetLogicPortSpriteType())
				{
				case LogicPortSpriteType.Input:
					original = ui_data.prefab;
					sprite = ui_data.inputSprite;
					break;
				case LogicPortSpriteType.Output:
					original = ui_data.prefab;
					sprite = ui_data.outputSprite;
					break;
				case LogicPortSpriteType.ResetUpdate:
					original = ui_data.prefab;
					sprite = ui_data.resetSprite;
					break;
				case LogicPortSpriteType.ControlInput:
					original = ui_data.controlInputPrefab;
					break;
				case LogicPortSpriteType.RibbonInput:
					original = ui_data.ribbonInputPrefab;
					this.bitDepth = 4;
					break;
				case LogicPortSpriteType.RibbonOutput:
					original = ui_data.ribbonOutputPrefab;
					this.bitDepth = 4;
					break;
				}
				this.instance = global::Util.KInstantiate(original, Grid.CellToPosCCC(this.cell, Grid.SceneLayer.Front), Quaternion.identity, GameScreenManager.Instance.worldSpaceCanvas, null, true, 0);
				this.instance.SetActive(true);
				this.image = this.instance.GetComponent<Image>();
				if (this.image != null)
				{
					this.image.raycastTarget = false;
					this.image.sprite = sprite;
				}
			}

			// Token: 0x0600931E RID: 37662 RVA: 0x00104A89 File Offset: 0x00102C89
			public void Release()
			{
				global::Util.KDestroyGameObject(this.instance);
			}

			// Token: 0x04006F84 RID: 28548
			public GameObject instance;

			// Token: 0x04006F85 RID: 28549
			public Image image;

			// Token: 0x04006F86 RID: 28550
			public int cell;

			// Token: 0x04006F87 RID: 28551
			public int bitDepth;
		}
	}

	// Token: 0x02001B69 RID: 7017
	public enum BringToFrontLayerSetting
	{
		// Token: 0x04006F94 RID: 28564
		None,
		// Token: 0x04006F95 RID: 28565
		Constant,
		// Token: 0x04006F96 RID: 28566
		Conditional
	}

	// Token: 0x02001B6A RID: 7018
	public class ColorHighlightCondition
	{
		// Token: 0x0600932A RID: 37674 RVA: 0x00104AEA File Offset: 0x00102CEA
		public ColorHighlightCondition(Func<KMonoBehaviour, Color> highlight_color, Func<KMonoBehaviour, bool> highlight_condition)
		{
			this.highlight_color = highlight_color;
			this.highlight_condition = highlight_condition;
		}

		// Token: 0x04006F97 RID: 28567
		public Func<KMonoBehaviour, Color> highlight_color;

		// Token: 0x04006F98 RID: 28568
		public Func<KMonoBehaviour, bool> highlight_condition;
	}

	// Token: 0x02001B6B RID: 7019
	public class None : OverlayModes.Mode
	{
		// Token: 0x0600932B RID: 37675 RVA: 0x00104B00 File Offset: 0x00102D00
		public override HashedString ViewMode()
		{
			return OverlayModes.None.ID;
		}

		// Token: 0x0600932C RID: 37676 RVA: 0x00104B07 File Offset: 0x00102D07
		public override string GetSoundName()
		{
			return "Off";
		}

		// Token: 0x04006F99 RID: 28569
		public static readonly HashedString ID = HashedString.Invalid;
	}

	// Token: 0x02001B6C RID: 7020
	public class PathProber : OverlayModes.Mode
	{
		// Token: 0x0600932F RID: 37679 RVA: 0x00104B22 File Offset: 0x00102D22
		public override HashedString ViewMode()
		{
			return OverlayModes.PathProber.ID;
		}

		// Token: 0x06009330 RID: 37680 RVA: 0x00104B07 File Offset: 0x00102D07
		public override string GetSoundName()
		{
			return "Off";
		}

		// Token: 0x04006F9A RID: 28570
		public static readonly HashedString ID = "PathProber";
	}

	// Token: 0x02001B6D RID: 7021
	public class Oxygen : OverlayModes.Mode
	{
		// Token: 0x06009333 RID: 37683 RVA: 0x00104B3A File Offset: 0x00102D3A
		public override HashedString ViewMode()
		{
			return OverlayModes.Oxygen.ID;
		}

		// Token: 0x06009334 RID: 37684 RVA: 0x00104B41 File Offset: 0x00102D41
		public override string GetSoundName()
		{
			return "Oxygen";
		}

		// Token: 0x06009335 RID: 37685 RVA: 0x00397DD8 File Offset: 0x00395FD8
		public override void Enable()
		{
			base.Enable();
			int defaultLayerMask = SelectTool.Instance.GetDefaultLayerMask();
			int mask = LayerMask.GetMask(new string[]
			{
				"MaskedOverlay"
			});
			SelectTool.Instance.SetLayerMask(defaultLayerMask | mask);
		}

		// Token: 0x06009336 RID: 37686 RVA: 0x00104B48 File Offset: 0x00102D48
		public override void Disable()
		{
			base.Disable();
			SelectTool.Instance.ClearLayerMask();
		}

		// Token: 0x04006F9B RID: 28571
		public static readonly HashedString ID = "Oxygen";
	}

	// Token: 0x02001B6E RID: 7022
	public class Light : OverlayModes.Mode
	{
		// Token: 0x06009339 RID: 37689 RVA: 0x00104B6B File Offset: 0x00102D6B
		public override HashedString ViewMode()
		{
			return OverlayModes.Light.ID;
		}

		// Token: 0x0600933A RID: 37690 RVA: 0x00104B72 File Offset: 0x00102D72
		public override string GetSoundName()
		{
			return "Lights";
		}

		// Token: 0x04006F9C RID: 28572
		public static readonly HashedString ID = "Light";
	}

	// Token: 0x02001B6F RID: 7023
	public class Priorities : OverlayModes.Mode
	{
		// Token: 0x0600933D RID: 37693 RVA: 0x00104B8A File Offset: 0x00102D8A
		public override HashedString ViewMode()
		{
			return OverlayModes.Priorities.ID;
		}

		// Token: 0x0600933E RID: 37694 RVA: 0x00104B91 File Offset: 0x00102D91
		public override string GetSoundName()
		{
			return "Priorities";
		}

		// Token: 0x04006F9D RID: 28573
		public static readonly HashedString ID = "Priorities";
	}

	// Token: 0x02001B70 RID: 7024
	public class ThermalConductivity : OverlayModes.Mode
	{
		// Token: 0x06009341 RID: 37697 RVA: 0x00104BA9 File Offset: 0x00102DA9
		public override HashedString ViewMode()
		{
			return OverlayModes.ThermalConductivity.ID;
		}

		// Token: 0x06009342 RID: 37698 RVA: 0x00104BB0 File Offset: 0x00102DB0
		public override string GetSoundName()
		{
			return "HeatFlow";
		}

		// Token: 0x04006F9E RID: 28574
		public static readonly HashedString ID = "ThermalConductivity";
	}

	// Token: 0x02001B71 RID: 7025
	public class HeatFlow : OverlayModes.Mode
	{
		// Token: 0x06009345 RID: 37701 RVA: 0x00104BC8 File Offset: 0x00102DC8
		public override HashedString ViewMode()
		{
			return OverlayModes.HeatFlow.ID;
		}

		// Token: 0x06009346 RID: 37702 RVA: 0x00104BB0 File Offset: 0x00102DB0
		public override string GetSoundName()
		{
			return "HeatFlow";
		}

		// Token: 0x04006F9F RID: 28575
		public static readonly HashedString ID = "HeatFlow";
	}

	// Token: 0x02001B72 RID: 7026
	public class Rooms : OverlayModes.Mode
	{
		// Token: 0x06009349 RID: 37705 RVA: 0x00104BE0 File Offset: 0x00102DE0
		public override HashedString ViewMode()
		{
			return OverlayModes.Rooms.ID;
		}

		// Token: 0x0600934A RID: 37706 RVA: 0x00104BE7 File Offset: 0x00102DE7
		public override string GetSoundName()
		{
			return "Rooms";
		}

		// Token: 0x0600934B RID: 37707 RVA: 0x00397E18 File Offset: 0x00396018
		public override List<LegendEntry> GetCustomLegendData()
		{
			List<LegendEntry> list = new List<LegendEntry>();
			List<RoomType> list2 = new List<RoomType>(Db.Get().RoomTypes.resources);
			list2.Sort((RoomType a, RoomType b) => a.sortKey.CompareTo(b.sortKey));
			foreach (RoomType roomType in list2)
			{
				string text = roomType.GetCriteriaString();
				if (roomType.effects != null && roomType.effects.Length != 0)
				{
					text = text + "\n\n" + roomType.GetRoomEffectsString();
				}
				list.Add(new LegendEntry(roomType.Name + "\n" + roomType.effect, text, GlobalAssets.Instance.colorSet.GetColorByName(roomType.category.colorName), null, null, true));
			}
			return list;
		}

		// Token: 0x04006FA0 RID: 28576
		public static readonly HashedString ID = "Rooms";
	}

	// Token: 0x02001B74 RID: 7028
	public abstract class Mode
	{
		// Token: 0x06009351 RID: 37713 RVA: 0x00104C0B File Offset: 0x00102E0B
		public static void Clear()
		{
			OverlayModes.Mode.workingTargets.Clear();
		}

		// Token: 0x06009352 RID: 37714
		public abstract HashedString ViewMode();

		// Token: 0x06009353 RID: 37715 RVA: 0x000AA038 File Offset: 0x000A8238
		public virtual void Enable()
		{
		}

		// Token: 0x06009354 RID: 37716 RVA: 0x000AA038 File Offset: 0x000A8238
		public virtual void Update()
		{
		}

		// Token: 0x06009355 RID: 37717 RVA: 0x000AA038 File Offset: 0x000A8238
		public virtual void Disable()
		{
		}

		// Token: 0x06009356 RID: 37718 RVA: 0x000AA765 File Offset: 0x000A8965
		public virtual List<LegendEntry> GetCustomLegendData()
		{
			return null;
		}

		// Token: 0x06009357 RID: 37719 RVA: 0x000AA765 File Offset: 0x000A8965
		public virtual Dictionary<string, ToolParameterMenu.ToggleState> CreateDefaultFilters()
		{
			return null;
		}

		// Token: 0x06009358 RID: 37720 RVA: 0x000AA038 File Offset: 0x000A8238
		public virtual void OnFiltersChanged()
		{
		}

		// Token: 0x06009359 RID: 37721 RVA: 0x000AA038 File Offset: 0x000A8238
		public virtual void DisableOverlay()
		{
		}

		// Token: 0x0600935A RID: 37722 RVA: 0x000AA038 File Offset: 0x000A8238
		public virtual void OnRenderImage(RenderTexture src, RenderTexture dest)
		{
		}

		// Token: 0x0600935B RID: 37723
		public abstract string GetSoundName();

		// Token: 0x0600935C RID: 37724 RVA: 0x00104C17 File Offset: 0x00102E17
		protected bool InFilter(string layer, Dictionary<string, ToolParameterMenu.ToggleState> filter)
		{
			return (filter.ContainsKey(ToolParameterMenu.FILTERLAYERS.ALL) && filter[ToolParameterMenu.FILTERLAYERS.ALL] == ToolParameterMenu.ToggleState.On) || (filter.ContainsKey(layer) && filter[layer] == ToolParameterMenu.ToggleState.On);
		}

		// Token: 0x0600935D RID: 37725 RVA: 0x00104C4A File Offset: 0x00102E4A
		public void RegisterSaveLoadListeners()
		{
			SaveManager saveManager = SaveLoader.Instance.saveManager;
			saveManager.onRegister += this.OnSaveLoadRootRegistered;
			saveManager.onUnregister += this.OnSaveLoadRootUnregistered;
		}

		// Token: 0x0600935E RID: 37726 RVA: 0x00104C7B File Offset: 0x00102E7B
		public void UnregisterSaveLoadListeners()
		{
			SaveManager saveManager = SaveLoader.Instance.saveManager;
			saveManager.onRegister -= this.OnSaveLoadRootRegistered;
			saveManager.onUnregister -= this.OnSaveLoadRootUnregistered;
		}

		// Token: 0x0600935F RID: 37727 RVA: 0x000AA038 File Offset: 0x000A8238
		protected virtual void OnSaveLoadRootRegistered(SaveLoadRoot root)
		{
		}

		// Token: 0x06009360 RID: 37728 RVA: 0x000AA038 File Offset: 0x000A8238
		protected virtual void OnSaveLoadRootUnregistered(SaveLoadRoot root)
		{
		}

		// Token: 0x06009361 RID: 37729 RVA: 0x00397F30 File Offset: 0x00396130
		protected void ProcessExistingSaveLoadRoots()
		{
			foreach (KeyValuePair<Tag, List<SaveLoadRoot>> keyValuePair in SaveLoader.Instance.saveManager.GetLists())
			{
				foreach (SaveLoadRoot root in keyValuePair.Value)
				{
					this.OnSaveLoadRootRegistered(root);
				}
			}
		}

		// Token: 0x06009362 RID: 37730 RVA: 0x00397FC8 File Offset: 0x003961C8
		protected static UniformGrid<T> PopulatePartition<T>(ICollection<Tag> tags) where T : IUniformGridObject
		{
			Dictionary<Tag, List<SaveLoadRoot>> lists = SaveLoader.Instance.saveManager.GetLists();
			UniformGrid<T> uniformGrid = new UniformGrid<T>(Grid.WidthInCells, Grid.HeightInCells, 8, 8);
			foreach (Tag key in tags)
			{
				List<SaveLoadRoot> list = null;
				if (lists.TryGetValue(key, out list))
				{
					foreach (SaveLoadRoot saveLoadRoot in list)
					{
						T component = saveLoadRoot.GetComponent<T>();
						if (component != null)
						{
							uniformGrid.Add(component);
						}
					}
				}
			}
			return uniformGrid;
		}

		// Token: 0x06009363 RID: 37731 RVA: 0x0039808C File Offset: 0x0039628C
		protected static void ResetDisplayValues<T>(ICollection<T> targets) where T : MonoBehaviour
		{
			foreach (T t in targets)
			{
				if (!(t == null))
				{
					KBatchedAnimController component = t.GetComponent<KBatchedAnimController>();
					if (component != null)
					{
						OverlayModes.Mode.ResetDisplayValues(component);
					}
				}
			}
		}

		// Token: 0x06009364 RID: 37732 RVA: 0x00104CAC File Offset: 0x00102EAC
		protected static void ResetDisplayValues(KBatchedAnimController controller)
		{
			controller.SetLayer(0);
			controller.HighlightColour = Color.clear;
			controller.TintColour = Color.white;
			controller.SetLayer(controller.GetComponent<KPrefabID>().defaultLayer);
		}

		// Token: 0x06009365 RID: 37733 RVA: 0x003980F8 File Offset: 0x003962F8
		protected static void RemoveOffscreenTargets<T>(ICollection<T> targets, Vector2I min, Vector2I max, Action<T> on_removed = null) where T : KMonoBehaviour
		{
			OverlayModes.Mode.ClearOutsideViewObjects<T>(targets, min, max, null, delegate(T cmp)
			{
				if (cmp != null)
				{
					KBatchedAnimController component = cmp.GetComponent<KBatchedAnimController>();
					if (component != null)
					{
						OverlayModes.Mode.ResetDisplayValues(component);
					}
					if (on_removed != null)
					{
						on_removed(cmp);
					}
				}
			});
			OverlayModes.Mode.workingTargets.Clear();
		}

		// Token: 0x06009366 RID: 37734 RVA: 0x00398134 File Offset: 0x00396334
		protected static void ClearOutsideViewObjects<T>(ICollection<T> targets, Vector2I vis_min, Vector2I vis_max, ICollection<Tag> item_ids, Action<T> on_remove) where T : KMonoBehaviour
		{
			OverlayModes.Mode.workingTargets.Clear();
			foreach (T t in targets)
			{
				if (!(t == null))
				{
					Vector2I vector2I = Grid.PosToXY(t.transform.GetPosition());
					if (!(vis_min <= vector2I) || !(vector2I <= vis_max) || t.gameObject.GetMyWorldId() != ClusterManager.Instance.activeWorldId)
					{
						OverlayModes.Mode.workingTargets.Add(t);
					}
					else
					{
						KPrefabID component = t.GetComponent<KPrefabID>();
						if (item_ids != null && !item_ids.Contains(component.PrefabTag) && t.gameObject.GetMyWorldId() != ClusterManager.Instance.activeWorldId)
						{
							OverlayModes.Mode.workingTargets.Add(t);
						}
					}
				}
			}
			foreach (KMonoBehaviour kmonoBehaviour in OverlayModes.Mode.workingTargets)
			{
				T t2 = (T)((object)kmonoBehaviour);
				if (!(t2 == null))
				{
					if (on_remove != null)
					{
						on_remove(t2);
					}
					targets.Remove(t2);
				}
			}
			OverlayModes.Mode.workingTargets.Clear();
		}

		// Token: 0x06009367 RID: 37735 RVA: 0x003982A8 File Offset: 0x003964A8
		protected static void RemoveOffscreenTargets<T>(ICollection<T> targets, ICollection<T> working_targets, Vector2I vis_min, Vector2I vis_max, Action<T> on_removed = null, Func<T, bool> special_clear_condition = null) where T : IUniformGridObject
		{
			OverlayModes.Mode.ClearOutsideViewObjects<T>(targets, working_targets, vis_min, vis_max, delegate(T cmp)
			{
				if (cmp != null && on_removed != null)
				{
					on_removed(cmp);
				}
			});
			if (special_clear_condition != null)
			{
				working_targets.Clear();
				foreach (T t in targets)
				{
					if (special_clear_condition(t))
					{
						working_targets.Add(t);
					}
				}
				foreach (T t2 in working_targets)
				{
					if (t2 != null)
					{
						if (on_removed != null)
						{
							on_removed(t2);
						}
						targets.Remove(t2);
					}
				}
				working_targets.Clear();
			}
		}

		// Token: 0x06009368 RID: 37736 RVA: 0x00398384 File Offset: 0x00396584
		protected static void ClearOutsideViewObjects<T>(ICollection<T> targets, ICollection<T> working_targets, Vector2I vis_min, Vector2I vis_max, Action<T> on_removed = null) where T : IUniformGridObject
		{
			working_targets.Clear();
			foreach (T t in targets)
			{
				if (t != null)
				{
					Vector2 vector = t.PosMin();
					Vector2 vector2 = t.PosMin();
					if (vector2.x < (float)vis_min.x || vector2.y < (float)vis_min.y || (float)vis_max.x < vector.x || (float)vis_max.y < vector.y)
					{
						working_targets.Add(t);
					}
				}
			}
			foreach (T t2 in working_targets)
			{
				if (t2 != null)
				{
					if (on_removed != null)
					{
						on_removed(t2);
					}
					targets.Remove(t2);
				}
			}
			working_targets.Clear();
		}

		// Token: 0x06009369 RID: 37737 RVA: 0x00398488 File Offset: 0x00396688
		protected static float GetDefaultDepth(KMonoBehaviour cmp)
		{
			BuildingComplete component = cmp.GetComponent<BuildingComplete>();
			float layerZ;
			if (component != null)
			{
				layerZ = Grid.GetLayerZ(component.Def.SceneLayer);
			}
			else
			{
				layerZ = Grid.GetLayerZ(Grid.SceneLayer.Creatures);
			}
			return layerZ;
		}

		// Token: 0x0600936A RID: 37738 RVA: 0x003984C4 File Offset: 0x003966C4
		protected void UpdateHighlightTypeOverlay<T>(Vector2I min, Vector2I max, ICollection<T> targets, ICollection<Tag> item_ids, OverlayModes.ColorHighlightCondition[] highlights, OverlayModes.BringToFrontLayerSetting bringToFrontSetting, int layer) where T : KMonoBehaviour
		{
			foreach (T t in targets)
			{
				if (!(t == null))
				{
					Vector3 position = t.transform.GetPosition();
					int cell = Grid.PosToCell(position);
					if (Grid.IsValidCell(cell) && Grid.IsVisible(cell) && min <= position && position <= max)
					{
						KBatchedAnimController component = t.GetComponent<KBatchedAnimController>();
						if (!(component == null))
						{
							int layer2 = 0;
							Color32 highlightColour = Color.clear;
							if (highlights != null)
							{
								foreach (OverlayModes.ColorHighlightCondition colorHighlightCondition in highlights)
								{
									if (colorHighlightCondition.highlight_condition(t))
									{
										highlightColour = colorHighlightCondition.highlight_color(t);
										layer2 = layer;
										break;
									}
								}
							}
							if (bringToFrontSetting != OverlayModes.BringToFrontLayerSetting.Constant)
							{
								if (bringToFrontSetting == OverlayModes.BringToFrontLayerSetting.Conditional)
								{
									component.SetLayer(layer2);
								}
							}
							else
							{
								component.SetLayer(layer);
							}
							component.HighlightColour = highlightColour;
						}
					}
				}
			}
		}

		// Token: 0x0600936B RID: 37739 RVA: 0x0039861C File Offset: 0x0039681C
		protected void DisableHighlightTypeOverlay<T>(ICollection<T> targets) where T : KMonoBehaviour
		{
			Color32 highlightColour = Color.clear;
			foreach (T t in targets)
			{
				if (!(t == null))
				{
					KBatchedAnimController component = t.GetComponent<KBatchedAnimController>();
					if (component != null)
					{
						component.HighlightColour = highlightColour;
						component.SetLayer(0);
					}
				}
			}
			targets.Clear();
		}

		// Token: 0x0600936C RID: 37740 RVA: 0x003986A0 File Offset: 0x003968A0
		protected void AddTargetIfVisible<T>(T instance, Vector2I vis_min, Vector2I vis_max, ICollection<T> targets, int layer, Action<T> on_added = null, Func<KMonoBehaviour, bool> should_add = null) where T : IUniformGridObject
		{
			if (instance.Equals(null))
			{
				return;
			}
			Vector2 vector = instance.PosMin();
			Vector2 vector2 = instance.PosMax();
			if (vector2.x < (float)vis_min.x || vector2.y < (float)vis_min.y || vector.x > (float)vis_max.x || vector.y > (float)vis_max.y)
			{
				return;
			}
			if (targets.Contains(instance))
			{
				return;
			}
			bool flag = false;
			int num = (int)vector.y;
			while ((float)num <= vector2.y)
			{
				int num2 = (int)vector.x;
				while ((float)num2 <= vector2.x)
				{
					int num3 = Grid.XYToCell(num2, num);
					if ((Grid.IsValidCell(num3) && Grid.Visible[num3] > 20 && (int)Grid.WorldIdx[num3] == ClusterManager.Instance.activeWorldId) || !PropertyTextures.IsFogOfWarEnabled)
					{
						flag = true;
						break;
					}
					num2++;
				}
				num++;
			}
			if (flag)
			{
				bool flag2 = true;
				KMonoBehaviour kmonoBehaviour = instance as KMonoBehaviour;
				if (kmonoBehaviour != null && should_add != null)
				{
					flag2 = should_add(kmonoBehaviour);
				}
				if (flag2)
				{
					if (kmonoBehaviour != null)
					{
						KBatchedAnimController component = kmonoBehaviour.GetComponent<KBatchedAnimController>();
						if (component != null)
						{
							component.SetLayer(layer);
						}
					}
					targets.Add(instance);
					if (on_added != null)
					{
						on_added(instance);
					}
				}
			}
		}

		// Token: 0x04006FA3 RID: 28579
		public Dictionary<string, ToolParameterMenu.ToggleState> legendFilters;

		// Token: 0x04006FA4 RID: 28580
		private static List<KMonoBehaviour> workingTargets = new List<KMonoBehaviour>();
	}

	// Token: 0x02001B77 RID: 7031
	public class ModeUtil
	{
		// Token: 0x06009373 RID: 37747 RVA: 0x00104D10 File Offset: 0x00102F10
		public static float GetHighlightScale()
		{
			return Mathf.SmoothStep(0.5f, 1f, Mathf.Abs(Mathf.Sin(Time.unscaledTime * 4f)));
		}
	}

	// Token: 0x02001B78 RID: 7032
	public class Power : OverlayModes.Mode
	{
		// Token: 0x06009375 RID: 37749 RVA: 0x00104D36 File Offset: 0x00102F36
		public override HashedString ViewMode()
		{
			return OverlayModes.Power.ID;
		}

		// Token: 0x06009376 RID: 37750 RVA: 0x00104D3D File Offset: 0x00102F3D
		public override string GetSoundName()
		{
			return "Power";
		}

		// Token: 0x06009377 RID: 37751 RVA: 0x0039884C File Offset: 0x00396A4C
		public Power(Canvas powerLabelParent, LocText powerLabelPrefab, BatteryUI batteryUIPrefab, Vector3 powerLabelOffset, Vector3 batteryUIOffset, Vector3 batteryUITransformerOffset, Vector3 batteryUISmallTransformerOffset)
		{
			this.powerLabelParent = powerLabelParent;
			this.powerLabelPrefab = powerLabelPrefab;
			this.batteryUIPrefab = batteryUIPrefab;
			this.powerLabelOffset = powerLabelOffset;
			this.batteryUIOffset = batteryUIOffset;
			this.batteryUITransformerOffset = batteryUITransformerOffset;
			this.batteryUISmallTransformerOffset = batteryUISmallTransformerOffset;
			this.targetLayer = LayerMask.NameToLayer("MaskedOverlay");
			this.cameraLayerMask = LayerMask.GetMask(new string[]
			{
				"MaskedOverlay",
				"MaskedOverlayBG"
			});
			this.selectionMask = this.cameraLayerMask;
		}

		// Token: 0x06009378 RID: 37752 RVA: 0x00398934 File Offset: 0x00396B34
		public override void Enable()
		{
			Camera.main.cullingMask |= this.cameraLayerMask;
			SelectTool.Instance.SetLayerMask(this.selectionMask);
			base.RegisterSaveLoadListeners();
			this.partition = OverlayModes.Mode.PopulatePartition<SaveLoadRoot>(OverlayScreen.WireIDs);
			GridCompositor.Instance.ToggleMinor(true);
		}

		// Token: 0x06009379 RID: 37753 RVA: 0x0039898C File Offset: 0x00396B8C
		public override void Disable()
		{
			OverlayModes.Mode.ResetDisplayValues<SaveLoadRoot>(this.layerTargets);
			Camera.main.cullingMask &= ~this.cameraLayerMask;
			SelectTool.Instance.ClearLayerMask();
			base.UnregisterSaveLoadListeners();
			this.partition.Clear();
			this.layerTargets.Clear();
			this.privateTargets.Clear();
			this.queuedAdds.Clear();
			this.DisablePowerLabels();
			this.DisableBatteryUIs();
			GridCompositor.Instance.ToggleMinor(false);
		}

		// Token: 0x0600937A RID: 37754 RVA: 0x00398A10 File Offset: 0x00396C10
		protected override void OnSaveLoadRootRegistered(SaveLoadRoot item)
		{
			Tag saveLoadTag = item.GetComponent<KPrefabID>().GetSaveLoadTag();
			if (OverlayScreen.WireIDs.Contains(saveLoadTag))
			{
				this.partition.Add(item);
			}
		}

		// Token: 0x0600937B RID: 37755 RVA: 0x00398A44 File Offset: 0x00396C44
		protected override void OnSaveLoadRootUnregistered(SaveLoadRoot item)
		{
			if (item == null || item.gameObject == null)
			{
				return;
			}
			if (this.layerTargets.Contains(item))
			{
				this.layerTargets.Remove(item);
			}
			this.partition.Remove(item);
		}

		// Token: 0x0600937C RID: 37756 RVA: 0x00398A90 File Offset: 0x00396C90
		public override void Update()
		{
			Vector2I vector2I;
			Vector2I vector2I2;
			Grid.GetVisibleExtents(out vector2I, out vector2I2);
			OverlayModes.Mode.RemoveOffscreenTargets<SaveLoadRoot>(this.layerTargets, vector2I, vector2I2, null);
			using (new KProfiler.Region("UpdatePowerOverlay", null))
			{
				foreach (object obj in this.partition.GetAllIntersecting(new Vector2((float)vector2I.x, (float)vector2I.y), new Vector2((float)vector2I2.x, (float)vector2I2.y)))
				{
					SaveLoadRoot instance = (SaveLoadRoot)obj;
					base.AddTargetIfVisible<SaveLoadRoot>(instance, vector2I, vector2I2, this.layerTargets, this.targetLayer, null, null);
				}
				this.connectedNetworks.Clear();
				float num = 1f;
				GameObject gameObject = null;
				if (SelectTool.Instance != null && SelectTool.Instance.hover != null)
				{
					gameObject = SelectTool.Instance.hover.gameObject;
				}
				if (gameObject != null)
				{
					IBridgedNetworkItem component = gameObject.GetComponent<IBridgedNetworkItem>();
					if (component != null)
					{
						int networkCell = component.GetNetworkCell();
						this.visited.Clear();
						this.FindConnectedNetworks(networkCell, Game.Instance.electricalConduitSystem, this.connectedNetworks, this.visited);
						this.visited.Clear();
						num = OverlayModes.ModeUtil.GetHighlightScale();
					}
				}
				CircuitManager circuitManager = Game.Instance.circuitManager;
				foreach (SaveLoadRoot saveLoadRoot in this.layerTargets)
				{
					if (!(saveLoadRoot == null))
					{
						IBridgedNetworkItem component2 = saveLoadRoot.GetComponent<IBridgedNetworkItem>();
						if (component2 != null)
						{
							KAnimControllerBase component3 = (component2 as KMonoBehaviour).GetComponent<KBatchedAnimController>();
							int networkCell2 = component2.GetNetworkCell();
							UtilityNetwork networkForCell = Game.Instance.electricalConduitSystem.GetNetworkForCell(networkCell2);
							ushort num2 = (networkForCell != null) ? ((ushort)networkForCell.id) : ushort.MaxValue;
							float wattsUsedByCircuit = circuitManager.GetWattsUsedByCircuit(num2);
							float num3 = circuitManager.GetMaxSafeWattageForCircuit(num2);
							num3 += POWER.FLOAT_FUDGE_FACTOR;
							float wattsNeededWhenActive = circuitManager.GetWattsNeededWhenActive(num2);
							Color32 color;
							if (wattsUsedByCircuit <= 0f)
							{
								color = GlobalAssets.Instance.colorSet.powerCircuitUnpowered;
							}
							else if (wattsUsedByCircuit > num3)
							{
								color = GlobalAssets.Instance.colorSet.powerCircuitOverloading;
							}
							else if (wattsNeededWhenActive > num3 && num3 > 0f && wattsUsedByCircuit / num3 >= 0.75f)
							{
								color = GlobalAssets.Instance.colorSet.powerCircuitStraining;
							}
							else
							{
								color = GlobalAssets.Instance.colorSet.powerCircuitSafe;
							}
							if (this.connectedNetworks.Count > 0 && component2.IsConnectedToNetworks(this.connectedNetworks))
							{
								color.r = (byte)((float)color.r * num);
								color.g = (byte)((float)color.g * num);
								color.b = (byte)((float)color.b * num);
							}
							component3.TintColour = color;
						}
					}
				}
			}
			this.queuedAdds.Clear();
			using (new KProfiler.Region("BatteryUI", null))
			{
				foreach (Battery battery in Components.Batteries.Items)
				{
					Vector2I vector2I3 = Grid.PosToXY(battery.transform.GetPosition());
					if (vector2I <= vector2I3 && vector2I3 <= vector2I2 && battery.GetMyWorldId() == ClusterManager.Instance.activeWorldId)
					{
						SaveLoadRoot component4 = battery.GetComponent<SaveLoadRoot>();
						if (!this.privateTargets.Contains(component4))
						{
							this.AddBatteryUI(battery);
							this.queuedAdds.Add(component4);
						}
					}
				}
				foreach (Generator generator in Components.Generators.Items)
				{
					Vector2I vector2I4 = Grid.PosToXY(generator.transform.GetPosition());
					if (vector2I <= vector2I4 && vector2I4 <= vector2I2 && generator.GetMyWorldId() == ClusterManager.Instance.activeWorldId)
					{
						SaveLoadRoot component5 = generator.GetComponent<SaveLoadRoot>();
						if (!this.privateTargets.Contains(component5))
						{
							this.privateTargets.Add(component5);
							if (generator.GetComponent<PowerTransformer>() == null)
							{
								this.AddPowerLabels(generator);
							}
						}
					}
				}
				foreach (EnergyConsumer energyConsumer in Components.EnergyConsumers.Items)
				{
					Vector2I vector2I5 = Grid.PosToXY(energyConsumer.transform.GetPosition());
					if (vector2I <= vector2I5 && vector2I5 <= vector2I2 && energyConsumer.GetMyWorldId() == ClusterManager.Instance.activeWorldId)
					{
						SaveLoadRoot component6 = energyConsumer.GetComponent<SaveLoadRoot>();
						if (!this.privateTargets.Contains(component6))
						{
							this.privateTargets.Add(component6);
							this.AddPowerLabels(energyConsumer);
						}
					}
				}
			}
			foreach (SaveLoadRoot item in this.queuedAdds)
			{
				this.privateTargets.Add(item);
			}
			this.queuedAdds.Clear();
			this.UpdatePowerLabels();
		}

		// Token: 0x0600937D RID: 37757 RVA: 0x003990A8 File Offset: 0x003972A8
		private LocText GetFreePowerLabel()
		{
			LocText locText;
			if (this.freePowerLabelIdx < this.powerLabels.Count)
			{
				locText = this.powerLabels[this.freePowerLabelIdx];
				this.freePowerLabelIdx++;
			}
			else
			{
				locText = global::Util.KInstantiateUI<LocText>(this.powerLabelPrefab.gameObject, this.powerLabelParent.transform.gameObject, false);
				this.powerLabels.Add(locText);
				this.freePowerLabelIdx++;
			}
			return locText;
		}

		// Token: 0x0600937E RID: 37758 RVA: 0x0039912C File Offset: 0x0039732C
		private void UpdatePowerLabels()
		{
			foreach (OverlayModes.Power.UpdatePowerInfo updatePowerInfo in this.updatePowerInfo)
			{
				KMonoBehaviour item = updatePowerInfo.item;
				LocText powerLabel = updatePowerInfo.powerLabel;
				LocText unitLabel = updatePowerInfo.unitLabel;
				Generator generator = updatePowerInfo.generator;
				IEnergyConsumer consumer = updatePowerInfo.consumer;
				if (updatePowerInfo.item == null || updatePowerInfo.item.gameObject.GetMyWorldId() != ClusterManager.Instance.activeWorldId)
				{
					powerLabel.gameObject.SetActive(false);
				}
				else
				{
					powerLabel.gameObject.SetActive(true);
					if (generator != null && consumer == null)
					{
						int num;
						if (generator.GetComponent<ManualGenerator>() == null)
						{
							generator.GetComponent<Operational>();
							num = Mathf.Max(0, Mathf.RoundToInt(generator.WattageRating));
						}
						else
						{
							num = Mathf.Max(0, Mathf.RoundToInt(generator.WattageRating));
						}
						powerLabel.text = ((num != 0) ? ("+" + num.ToString()) : num.ToString());
						BuildingEnabledButton component = item.GetComponent<BuildingEnabledButton>();
						Color color = (component != null && !component.IsEnabled) ? GlobalAssets.Instance.colorSet.powerBuildingDisabled : GlobalAssets.Instance.colorSet.powerGenerator;
						powerLabel.color = color;
						unitLabel.color = color;
						BuildingCellVisualizer component2 = generator.GetComponent<BuildingCellVisualizer>();
						if (component2 != null)
						{
							Image powerOutputIcon = component2.GetPowerOutputIcon();
							if (powerOutputIcon != null)
							{
								powerOutputIcon.color = color;
							}
						}
					}
					if (consumer != null)
					{
						BuildingEnabledButton component3 = item.GetComponent<BuildingEnabledButton>();
						Color color2 = (component3 != null && !component3.IsEnabled) ? GlobalAssets.Instance.colorSet.powerBuildingDisabled : GlobalAssets.Instance.colorSet.powerConsumer;
						int num2 = Mathf.Max(0, Mathf.RoundToInt(consumer.WattsNeededWhenActive));
						string text = num2.ToString();
						powerLabel.text = ((num2 != 0) ? ("-" + text) : text);
						powerLabel.color = color2;
						unitLabel.color = color2;
						Image powerInputIcon = item.GetComponentInChildren<BuildingCellVisualizer>().GetPowerInputIcon();
						if (powerInputIcon != null)
						{
							powerInputIcon.color = color2;
						}
					}
				}
			}
			foreach (OverlayModes.Power.UpdateBatteryInfo updateBatteryInfo in this.updateBatteryInfo)
			{
				updateBatteryInfo.ui.SetContent(updateBatteryInfo.battery);
			}
		}

		// Token: 0x0600937F RID: 37759 RVA: 0x00399408 File Offset: 0x00397608
		private void AddPowerLabels(KMonoBehaviour item)
		{
			if (item.gameObject.GetMyWorldId() == ClusterManager.Instance.activeWorldId)
			{
				IEnergyConsumer componentInChildren = item.gameObject.GetComponentInChildren<IEnergyConsumer>();
				Generator componentInChildren2 = item.gameObject.GetComponentInChildren<Generator>();
				if (componentInChildren != null || componentInChildren2 != null)
				{
					float num = -10f;
					if (componentInChildren2 != null)
					{
						LocText freePowerLabel = this.GetFreePowerLabel();
						freePowerLabel.gameObject.SetActive(true);
						freePowerLabel.gameObject.name = item.gameObject.name + "power label";
						LocText component = freePowerLabel.transform.GetChild(0).GetComponent<LocText>();
						component.gameObject.SetActive(true);
						freePowerLabel.enabled = true;
						component.enabled = true;
						Vector3 a = Grid.CellToPos(componentInChildren2.PowerCell, 0.5f, 0f, 0f);
						freePowerLabel.rectTransform.SetPosition(a + this.powerLabelOffset + Vector3.up * (num * 0.02f));
						if (componentInChildren != null && componentInChildren.PowerCell == componentInChildren2.PowerCell)
						{
							num -= 15f;
						}
						this.SetToolTip(freePowerLabel, UI.OVERLAYS.POWER.WATTS_GENERATED);
						this.updatePowerInfo.Add(new OverlayModes.Power.UpdatePowerInfo(item, freePowerLabel, component, componentInChildren2, null));
					}
					if (componentInChildren != null && componentInChildren.GetType() != typeof(Battery))
					{
						LocText freePowerLabel2 = this.GetFreePowerLabel();
						LocText component2 = freePowerLabel2.transform.GetChild(0).GetComponent<LocText>();
						freePowerLabel2.gameObject.SetActive(true);
						component2.gameObject.SetActive(true);
						freePowerLabel2.gameObject.name = item.gameObject.name + "power label";
						freePowerLabel2.enabled = true;
						component2.enabled = true;
						Vector3 a2 = Grid.CellToPos(componentInChildren.PowerCell, 0.5f, 0f, 0f);
						freePowerLabel2.rectTransform.SetPosition(a2 + this.powerLabelOffset + Vector3.up * (num * 0.02f));
						this.SetToolTip(freePowerLabel2, UI.OVERLAYS.POWER.WATTS_CONSUMED);
						this.updatePowerInfo.Add(new OverlayModes.Power.UpdatePowerInfo(item, freePowerLabel2, component2, null, componentInChildren));
					}
				}
			}
		}

		// Token: 0x06009380 RID: 37760 RVA: 0x00399654 File Offset: 0x00397854
		private void DisablePowerLabels()
		{
			this.freePowerLabelIdx = 0;
			foreach (LocText locText in this.powerLabels)
			{
				locText.gameObject.SetActive(false);
			}
			this.updatePowerInfo.Clear();
		}

		// Token: 0x06009381 RID: 37761 RVA: 0x003996BC File Offset: 0x003978BC
		private void AddBatteryUI(Battery bat)
		{
			BatteryUI freeBatteryUI = this.GetFreeBatteryUI();
			freeBatteryUI.SetContent(bat);
			Vector3 b = Grid.CellToPos(bat.PowerCell, 0.5f, 0f, 0f);
			bool flag = bat.powerTransformer != null;
			float num = 1f;
			Rotatable component = bat.GetComponent<Rotatable>();
			if (component != null && component.GetVisualizerFlipX())
			{
				num = -1f;
			}
			Vector3 b2 = this.batteryUIOffset;
			if (flag)
			{
				b2 = ((bat.GetComponent<Building>().Def.WidthInCells == 2) ? this.batteryUISmallTransformerOffset : this.batteryUITransformerOffset);
			}
			b2.x *= num;
			freeBatteryUI.GetComponent<RectTransform>().SetPosition(Vector3.up + b + b2);
			this.updateBatteryInfo.Add(new OverlayModes.Power.UpdateBatteryInfo(bat, freeBatteryUI));
		}

		// Token: 0x06009382 RID: 37762 RVA: 0x0039978C File Offset: 0x0039798C
		private void SetToolTip(LocText label, string text)
		{
			ToolTip component = label.GetComponent<ToolTip>();
			if (component != null)
			{
				component.toolTip = text;
			}
		}

		// Token: 0x06009383 RID: 37763 RVA: 0x003997B0 File Offset: 0x003979B0
		private void DisableBatteryUIs()
		{
			this.freeBatteryUIIdx = 0;
			foreach (BatteryUI batteryUI in this.batteryUIList)
			{
				batteryUI.gameObject.SetActive(false);
			}
			this.updateBatteryInfo.Clear();
		}

		// Token: 0x06009384 RID: 37764 RVA: 0x00399818 File Offset: 0x00397A18
		private BatteryUI GetFreeBatteryUI()
		{
			BatteryUI batteryUI;
			if (this.freeBatteryUIIdx < this.batteryUIList.Count)
			{
				batteryUI = this.batteryUIList[this.freeBatteryUIIdx];
				batteryUI.gameObject.SetActive(true);
				this.freeBatteryUIIdx++;
			}
			else
			{
				batteryUI = global::Util.KInstantiateUI<BatteryUI>(this.batteryUIPrefab.gameObject, this.powerLabelParent.transform.gameObject, false);
				this.batteryUIList.Add(batteryUI);
				this.freeBatteryUIIdx++;
			}
			return batteryUI;
		}

		// Token: 0x06009385 RID: 37765 RVA: 0x003998A8 File Offset: 0x00397AA8
		private void FindConnectedNetworks(int cell, IUtilityNetworkMgr mgr, ICollection<UtilityNetwork> networks, List<int> visited)
		{
			if (visited.Contains(cell))
			{
				return;
			}
			visited.Add(cell);
			UtilityNetwork networkForCell = mgr.GetNetworkForCell(cell);
			if (networkForCell != null)
			{
				networks.Add(networkForCell);
				UtilityConnections connections = mgr.GetConnections(cell, false);
				if ((connections & UtilityConnections.Right) != (UtilityConnections)0)
				{
					this.FindConnectedNetworks(Grid.CellRight(cell), mgr, networks, visited);
				}
				if ((connections & UtilityConnections.Left) != (UtilityConnections)0)
				{
					this.FindConnectedNetworks(Grid.CellLeft(cell), mgr, networks, visited);
				}
				if ((connections & UtilityConnections.Up) != (UtilityConnections)0)
				{
					this.FindConnectedNetworks(Grid.CellAbove(cell), mgr, networks, visited);
				}
				if ((connections & UtilityConnections.Down) != (UtilityConnections)0)
				{
					this.FindConnectedNetworks(Grid.CellBelow(cell), mgr, networks, visited);
				}
			}
		}

		// Token: 0x04006FA7 RID: 28583
		public static readonly HashedString ID = "Power";

		// Token: 0x04006FA8 RID: 28584
		private int targetLayer;

		// Token: 0x04006FA9 RID: 28585
		private int cameraLayerMask;

		// Token: 0x04006FAA RID: 28586
		private int selectionMask;

		// Token: 0x04006FAB RID: 28587
		private List<OverlayModes.Power.UpdatePowerInfo> updatePowerInfo = new List<OverlayModes.Power.UpdatePowerInfo>();

		// Token: 0x04006FAC RID: 28588
		private List<OverlayModes.Power.UpdateBatteryInfo> updateBatteryInfo = new List<OverlayModes.Power.UpdateBatteryInfo>();

		// Token: 0x04006FAD RID: 28589
		private Canvas powerLabelParent;

		// Token: 0x04006FAE RID: 28590
		private LocText powerLabelPrefab;

		// Token: 0x04006FAF RID: 28591
		private Vector3 powerLabelOffset;

		// Token: 0x04006FB0 RID: 28592
		private BatteryUI batteryUIPrefab;

		// Token: 0x04006FB1 RID: 28593
		private Vector3 batteryUIOffset;

		// Token: 0x04006FB2 RID: 28594
		private Vector3 batteryUITransformerOffset;

		// Token: 0x04006FB3 RID: 28595
		private Vector3 batteryUISmallTransformerOffset;

		// Token: 0x04006FB4 RID: 28596
		private int freePowerLabelIdx;

		// Token: 0x04006FB5 RID: 28597
		private int freeBatteryUIIdx;

		// Token: 0x04006FB6 RID: 28598
		private List<LocText> powerLabels = new List<LocText>();

		// Token: 0x04006FB7 RID: 28599
		private List<BatteryUI> batteryUIList = new List<BatteryUI>();

		// Token: 0x04006FB8 RID: 28600
		private UniformGrid<SaveLoadRoot> partition;

		// Token: 0x04006FB9 RID: 28601
		private List<SaveLoadRoot> queuedAdds = new List<SaveLoadRoot>();

		// Token: 0x04006FBA RID: 28602
		private HashSet<SaveLoadRoot> layerTargets = new HashSet<SaveLoadRoot>();

		// Token: 0x04006FBB RID: 28603
		private HashSet<SaveLoadRoot> privateTargets = new HashSet<SaveLoadRoot>();

		// Token: 0x04006FBC RID: 28604
		private HashSet<UtilityNetwork> connectedNetworks = new HashSet<UtilityNetwork>();

		// Token: 0x04006FBD RID: 28605
		private List<int> visited = new List<int>();

		// Token: 0x02001B79 RID: 7033
		private struct UpdatePowerInfo
		{
			// Token: 0x06009387 RID: 37767 RVA: 0x00104D55 File Offset: 0x00102F55
			public UpdatePowerInfo(KMonoBehaviour item, LocText power_label, LocText unit_label, Generator g, IEnergyConsumer c)
			{
				this.item = item;
				this.powerLabel = power_label;
				this.unitLabel = unit_label;
				this.generator = g;
				this.consumer = c;
			}

			// Token: 0x04006FBE RID: 28606
			public KMonoBehaviour item;

			// Token: 0x04006FBF RID: 28607
			public LocText powerLabel;

			// Token: 0x04006FC0 RID: 28608
			public LocText unitLabel;

			// Token: 0x04006FC1 RID: 28609
			public Generator generator;

			// Token: 0x04006FC2 RID: 28610
			public IEnergyConsumer consumer;
		}

		// Token: 0x02001B7A RID: 7034
		private struct UpdateBatteryInfo
		{
			// Token: 0x06009388 RID: 37768 RVA: 0x00104D7C File Offset: 0x00102F7C
			public UpdateBatteryInfo(Battery battery, BatteryUI ui)
			{
				this.battery = battery;
				this.ui = ui;
			}

			// Token: 0x04006FC3 RID: 28611
			public Battery battery;

			// Token: 0x04006FC4 RID: 28612
			public BatteryUI ui;
		}
	}

	// Token: 0x02001B7B RID: 7035
	public class Radiation : OverlayModes.Mode
	{
		// Token: 0x06009389 RID: 37769 RVA: 0x00104D8C File Offset: 0x00102F8C
		public override HashedString ViewMode()
		{
			return OverlayModes.Radiation.ID;
		}

		// Token: 0x0600938A RID: 37770 RVA: 0x00104D93 File Offset: 0x00102F93
		public override string GetSoundName()
		{
			return "Radiation";
		}

		// Token: 0x0600938B RID: 37771 RVA: 0x00104D9A File Offset: 0x00102F9A
		public override void Enable()
		{
			AudioMixer.instance.Start(AudioMixerSnapshots.Get().TechFilterRadiationOn);
		}

		// Token: 0x0600938C RID: 37772 RVA: 0x00104DB1 File Offset: 0x00102FB1
		public override void Disable()
		{
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().TechFilterRadiationOn, STOP_MODE.ALLOWFADEOUT);
		}

		// Token: 0x04006FC5 RID: 28613
		public static readonly HashedString ID = "Radiation";
	}

	// Token: 0x02001B7C RID: 7036
	public class SolidConveyor : OverlayModes.Mode
	{
		// Token: 0x0600938F RID: 37775 RVA: 0x00104DDA File Offset: 0x00102FDA
		public override HashedString ViewMode()
		{
			return OverlayModes.SolidConveyor.ID;
		}

		// Token: 0x06009390 RID: 37776 RVA: 0x001047B4 File Offset: 0x001029B4
		public override string GetSoundName()
		{
			return "LiquidVent";
		}

		// Token: 0x06009391 RID: 37777 RVA: 0x00399938 File Offset: 0x00397B38
		public SolidConveyor()
		{
			this.targetLayer = LayerMask.NameToLayer("MaskedOverlay");
			this.cameraLayerMask = LayerMask.GetMask(new string[]
			{
				"MaskedOverlay",
				"MaskedOverlayBG"
			});
			this.selectionMask = this.cameraLayerMask;
		}

		// Token: 0x06009392 RID: 37778 RVA: 0x003999D0 File Offset: 0x00397BD0
		public override void Enable()
		{
			base.RegisterSaveLoadListeners();
			this.partition = OverlayModes.Mode.PopulatePartition<SaveLoadRoot>(this.targetIDs);
			Camera.main.cullingMask |= this.cameraLayerMask;
			SelectTool.Instance.SetLayerMask(this.selectionMask);
			GridCompositor.Instance.ToggleMinor(false);
			base.Enable();
		}

		// Token: 0x06009393 RID: 37779 RVA: 0x00399A2C File Offset: 0x00397C2C
		protected override void OnSaveLoadRootRegistered(SaveLoadRoot item)
		{
			Tag saveLoadTag = item.GetComponent<KPrefabID>().GetSaveLoadTag();
			if (this.targetIDs.Contains(saveLoadTag))
			{
				this.partition.Add(item);
			}
		}

		// Token: 0x06009394 RID: 37780 RVA: 0x00399A60 File Offset: 0x00397C60
		protected override void OnSaveLoadRootUnregistered(SaveLoadRoot item)
		{
			if (item == null || item.gameObject == null)
			{
				return;
			}
			if (this.layerTargets.Contains(item))
			{
				this.layerTargets.Remove(item);
			}
			this.partition.Remove(item);
		}

		// Token: 0x06009395 RID: 37781 RVA: 0x00399AAC File Offset: 0x00397CAC
		public override void Disable()
		{
			OverlayModes.Mode.ResetDisplayValues<SaveLoadRoot>(this.layerTargets);
			Camera.main.cullingMask &= ~this.cameraLayerMask;
			SelectTool.Instance.ClearLayerMask();
			base.UnregisterSaveLoadListeners();
			this.partition.Clear();
			this.layerTargets.Clear();
			GridCompositor.Instance.ToggleMinor(false);
			base.Disable();
		}

		// Token: 0x06009396 RID: 37782 RVA: 0x00399B14 File Offset: 0x00397D14
		public override void Update()
		{
			Vector2I vector2I;
			Vector2I vector2I2;
			Grid.GetVisibleExtents(out vector2I, out vector2I2);
			OverlayModes.Mode.RemoveOffscreenTargets<SaveLoadRoot>(this.layerTargets, vector2I, vector2I2, null);
			foreach (object obj in this.partition.GetAllIntersecting(new Vector2((float)vector2I.x, (float)vector2I.y), new Vector2((float)vector2I2.x, (float)vector2I2.y)))
			{
				SaveLoadRoot instance = (SaveLoadRoot)obj;
				base.AddTargetIfVisible<SaveLoadRoot>(instance, vector2I, vector2I2, this.layerTargets, this.targetLayer, null, null);
			}
			GameObject gameObject = null;
			if (SelectTool.Instance != null && SelectTool.Instance.hover != null)
			{
				gameObject = SelectTool.Instance.hover.gameObject;
			}
			this.connectedNetworks.Clear();
			float num = 1f;
			if (gameObject != null)
			{
				SolidConduit component = gameObject.GetComponent<SolidConduit>();
				if (component != null)
				{
					int cell = Grid.PosToCell(component);
					UtilityNetworkManager<FlowUtilityNetwork, SolidConduit> solidConduitSystem = Game.Instance.solidConduitSystem;
					this.visited.Clear();
					this.FindConnectedNetworks(cell, solidConduitSystem, this.connectedNetworks, this.visited);
					this.visited.Clear();
					num = OverlayModes.ModeUtil.GetHighlightScale();
				}
			}
			foreach (SaveLoadRoot saveLoadRoot in this.layerTargets)
			{
				if (!(saveLoadRoot == null))
				{
					Color32 color = this.tint_color;
					SolidConduit component2 = saveLoadRoot.GetComponent<SolidConduit>();
					if (component2 != null)
					{
						if (this.connectedNetworks.Count > 0 && this.IsConnectedToNetworks(component2, this.connectedNetworks))
						{
							color.r = (byte)((float)color.r * num);
							color.g = (byte)((float)color.g * num);
							color.b = (byte)((float)color.b * num);
						}
						saveLoadRoot.GetComponent<KBatchedAnimController>().TintColour = color;
					}
				}
			}
		}

		// Token: 0x06009397 RID: 37783 RVA: 0x00399D38 File Offset: 0x00397F38
		public bool IsConnectedToNetworks(SolidConduit conduit, ICollection<UtilityNetwork> networks)
		{
			UtilityNetwork network = conduit.GetNetwork();
			return networks.Contains(network);
		}

		// Token: 0x06009398 RID: 37784 RVA: 0x00399D54 File Offset: 0x00397F54
		private void FindConnectedNetworks(int cell, IUtilityNetworkMgr mgr, ICollection<UtilityNetwork> networks, List<int> visited)
		{
			if (visited.Contains(cell))
			{
				return;
			}
			visited.Add(cell);
			UtilityNetwork networkForCell = mgr.GetNetworkForCell(cell);
			if (networkForCell != null)
			{
				networks.Add(networkForCell);
				UtilityConnections connections = mgr.GetConnections(cell, false);
				if ((connections & UtilityConnections.Right) != (UtilityConnections)0)
				{
					this.FindConnectedNetworks(Grid.CellRight(cell), mgr, networks, visited);
				}
				if ((connections & UtilityConnections.Left) != (UtilityConnections)0)
				{
					this.FindConnectedNetworks(Grid.CellLeft(cell), mgr, networks, visited);
				}
				if ((connections & UtilityConnections.Up) != (UtilityConnections)0)
				{
					this.FindConnectedNetworks(Grid.CellAbove(cell), mgr, networks, visited);
				}
				if ((connections & UtilityConnections.Down) != (UtilityConnections)0)
				{
					this.FindConnectedNetworks(Grid.CellBelow(cell), mgr, networks, visited);
				}
				object endpoint = mgr.GetEndpoint(cell);
				if (endpoint != null)
				{
					FlowUtilityNetwork.NetworkItem networkItem = endpoint as FlowUtilityNetwork.NetworkItem;
					if (networkItem != null)
					{
						GameObject gameObject = networkItem.GameObject;
						if (gameObject != null)
						{
							IBridgedNetworkItem component = gameObject.GetComponent<IBridgedNetworkItem>();
							if (component != null)
							{
								component.AddNetworks(networks);
							}
						}
					}
				}
			}
		}

		// Token: 0x04006FC6 RID: 28614
		public static readonly HashedString ID = "SolidConveyor";

		// Token: 0x04006FC7 RID: 28615
		private UniformGrid<SaveLoadRoot> partition;

		// Token: 0x04006FC8 RID: 28616
		private HashSet<SaveLoadRoot> layerTargets = new HashSet<SaveLoadRoot>();

		// Token: 0x04006FC9 RID: 28617
		private ICollection<Tag> targetIDs = OverlayScreen.SolidConveyorIDs;

		// Token: 0x04006FCA RID: 28618
		private Color32 tint_color = new Color32(201, 201, 201, 0);

		// Token: 0x04006FCB RID: 28619
		private HashSet<UtilityNetwork> connectedNetworks = new HashSet<UtilityNetwork>();

		// Token: 0x04006FCC RID: 28620
		private List<int> visited = new List<int>();

		// Token: 0x04006FCD RID: 28621
		private int targetLayer;

		// Token: 0x04006FCE RID: 28622
		private int cameraLayerMask;

		// Token: 0x04006FCF RID: 28623
		private int selectionMask;
	}

	// Token: 0x02001B7D RID: 7037
	public class Sound : OverlayModes.Mode
	{
		// Token: 0x0600939A RID: 37786 RVA: 0x00104DF2 File Offset: 0x00102FF2
		public override HashedString ViewMode()
		{
			return OverlayModes.Sound.ID;
		}

		// Token: 0x0600939B RID: 37787 RVA: 0x00104DF9 File Offset: 0x00102FF9
		public override string GetSoundName()
		{
			return "Sound";
		}

		// Token: 0x0600939C RID: 37788 RVA: 0x00399E20 File Offset: 0x00398020
		public Sound()
		{
			OverlayModes.ColorHighlightCondition[] array = new OverlayModes.ColorHighlightCondition[1];
			array[0] = new OverlayModes.ColorHighlightCondition(delegate(KMonoBehaviour np)
			{
				Color black = Color.black;
				Color black2 = Color.black;
				float t = 0.8f;
				if (np != null)
				{
					int cell = Grid.PosToCell(CameraController.Instance.baseCamera.ScreenToWorldPoint(KInputManager.GetMousePos()));
					if ((np as NoisePolluter).GetNoiseForCell(cell) < 36f)
					{
						t = 1f;
						black2 = new Color(0.4f, 0.4f, 0.4f);
					}
				}
				return Color.Lerp(black, black2, t);
			}, delegate(KMonoBehaviour np)
			{
				List<GameObject> highlightedObjects = SelectToolHoverTextCard.highlightedObjects;
				bool result = false;
				for (int i = 0; i < highlightedObjects.Count; i++)
				{
					if (highlightedObjects[i] != null && highlightedObjects[i] == np.gameObject)
					{
						result = true;
						break;
					}
				}
				return result;
			});
			this.highlightConditions = array;
			base..ctor();
			this.targetLayer = LayerMask.NameToLayer("MaskedOverlay");
			this.cameraLayerMask = LayerMask.GetMask(new string[]
			{
				"MaskedOverlay",
				"MaskedOverlayBG"
			});
			List<Tag> prefabTagsWithComponent = Assets.GetPrefabTagsWithComponent<NoisePolluter>();
			this.targetIDs.UnionWith(prefabTagsWithComponent);
		}

		// Token: 0x0600939D RID: 37789 RVA: 0x00399EE0 File Offset: 0x003980E0
		public override void Enable()
		{
			base.RegisterSaveLoadListeners();
			List<Tag> prefabTagsWithComponent = Assets.GetPrefabTagsWithComponent<NoisePolluter>();
			this.targetIDs.UnionWith(prefabTagsWithComponent);
			this.partition = OverlayModes.Mode.PopulatePartition<NoisePolluter>(this.targetIDs);
			Camera.main.cullingMask |= this.cameraLayerMask;
		}

		// Token: 0x0600939E RID: 37790 RVA: 0x00399F30 File Offset: 0x00398130
		public override void Update()
		{
			Vector2I vector2I;
			Vector2I vector2I2;
			Grid.GetVisibleExtents(out vector2I, out vector2I2);
			OverlayModes.Mode.RemoveOffscreenTargets<NoisePolluter>(this.layerTargets, vector2I, vector2I2, null);
			foreach (object obj in this.partition.GetAllIntersecting(new Vector2((float)vector2I.x, (float)vector2I.y), new Vector2((float)vector2I2.x, (float)vector2I2.y)))
			{
				NoisePolluter instance = (NoisePolluter)obj;
				base.AddTargetIfVisible<NoisePolluter>(instance, vector2I, vector2I2, this.layerTargets, this.targetLayer, null, null);
			}
			base.UpdateHighlightTypeOverlay<NoisePolluter>(vector2I, vector2I2, this.layerTargets, this.targetIDs, this.highlightConditions, OverlayModes.BringToFrontLayerSetting.Conditional, this.targetLayer);
		}

		// Token: 0x0600939F RID: 37791 RVA: 0x0039A000 File Offset: 0x00398200
		protected override void OnSaveLoadRootRegistered(SaveLoadRoot item)
		{
			Tag saveLoadTag = item.GetComponent<KPrefabID>().GetSaveLoadTag();
			if (this.targetIDs.Contains(saveLoadTag))
			{
				NoisePolluter component = item.GetComponent<NoisePolluter>();
				this.partition.Add(component);
			}
		}

		// Token: 0x060093A0 RID: 37792 RVA: 0x0039A03C File Offset: 0x0039823C
		protected override void OnSaveLoadRootUnregistered(SaveLoadRoot item)
		{
			if (item == null || item.gameObject == null)
			{
				return;
			}
			NoisePolluter component = item.GetComponent<NoisePolluter>();
			if (this.layerTargets.Contains(component))
			{
				this.layerTargets.Remove(component);
			}
			this.partition.Remove(component);
		}

		// Token: 0x060093A1 RID: 37793 RVA: 0x0039A090 File Offset: 0x00398290
		public override void Disable()
		{
			base.DisableHighlightTypeOverlay<NoisePolluter>(this.layerTargets);
			Camera.main.cullingMask &= ~this.cameraLayerMask;
			base.UnregisterSaveLoadListeners();
			this.partition.Clear();
			this.layerTargets.Clear();
		}

		// Token: 0x04006FD0 RID: 28624
		public static readonly HashedString ID = "Sound";

		// Token: 0x04006FD1 RID: 28625
		private UniformGrid<NoisePolluter> partition;

		// Token: 0x04006FD2 RID: 28626
		private HashSet<NoisePolluter> layerTargets = new HashSet<NoisePolluter>();

		// Token: 0x04006FD3 RID: 28627
		private HashSet<Tag> targetIDs = new HashSet<Tag>();

		// Token: 0x04006FD4 RID: 28628
		private int targetLayer;

		// Token: 0x04006FD5 RID: 28629
		private int cameraLayerMask;

		// Token: 0x04006FD6 RID: 28630
		private OverlayModes.ColorHighlightCondition[] highlightConditions;
	}

	// Token: 0x02001B7F RID: 7039
	public class Suit : OverlayModes.Mode
	{
		// Token: 0x060093A7 RID: 37799 RVA: 0x00104E1D File Offset: 0x0010301D
		public override HashedString ViewMode()
		{
			return OverlayModes.Suit.ID;
		}

		// Token: 0x060093A8 RID: 37800 RVA: 0x00104E24 File Offset: 0x00103024
		public override string GetSoundName()
		{
			return "SuitRequired";
		}

		// Token: 0x060093A9 RID: 37801 RVA: 0x0039A1A8 File Offset: 0x003983A8
		public Suit(Canvas ui_parent, GameObject overlay_prefab)
		{
			this.targetLayer = LayerMask.NameToLayer("MaskedOverlay");
			this.cameraLayerMask = LayerMask.GetMask(new string[]
			{
				"MaskedOverlay",
				"MaskedOverlayBG"
			});
			this.selectionMask = this.cameraLayerMask;
			this.targetIDs = OverlayScreen.SuitIDs;
			this.uiParent = ui_parent;
			this.overlayPrefab = overlay_prefab;
		}

		// Token: 0x060093AA RID: 37802 RVA: 0x0039A228 File Offset: 0x00398428
		public override void Enable()
		{
			this.partition = new UniformGrid<SaveLoadRoot>(Grid.WidthInCells, Grid.HeightInCells, 8, 8);
			base.ProcessExistingSaveLoadRoots();
			base.RegisterSaveLoadListeners();
			Camera.main.cullingMask |= this.cameraLayerMask;
			SelectTool.Instance.SetLayerMask(this.selectionMask);
			GridCompositor.Instance.ToggleMinor(false);
			base.Enable();
		}

		// Token: 0x060093AB RID: 37803 RVA: 0x0039A290 File Offset: 0x00398490
		public override void Disable()
		{
			base.UnregisterSaveLoadListeners();
			OverlayModes.Mode.ResetDisplayValues<SaveLoadRoot>(this.layerTargets);
			Camera.main.cullingMask &= ~this.cameraLayerMask;
			SelectTool.Instance.ClearLayerMask();
			this.partition.Clear();
			this.partition = null;
			this.layerTargets.Clear();
			for (int i = 0; i < this.uiList.Count; i++)
			{
				this.uiList[i].SetActive(false);
			}
			GridCompositor.Instance.ToggleMinor(false);
			base.Disable();
		}

		// Token: 0x060093AC RID: 37804 RVA: 0x0039A328 File Offset: 0x00398528
		protected override void OnSaveLoadRootRegistered(SaveLoadRoot item)
		{
			Tag saveLoadTag = item.GetComponent<KPrefabID>().GetSaveLoadTag();
			if (this.targetIDs.Contains(saveLoadTag))
			{
				this.partition.Add(item);
			}
		}

		// Token: 0x060093AD RID: 37805 RVA: 0x0039A35C File Offset: 0x0039855C
		protected override void OnSaveLoadRootUnregistered(SaveLoadRoot item)
		{
			if (item == null || item.gameObject == null)
			{
				return;
			}
			if (this.layerTargets.Contains(item))
			{
				this.layerTargets.Remove(item);
			}
			this.partition.Remove(item);
		}

		// Token: 0x060093AE RID: 37806 RVA: 0x0039A3A8 File Offset: 0x003985A8
		private GameObject GetFreeUI()
		{
			GameObject gameObject;
			if (this.freeUiIdx >= this.uiList.Count)
			{
				gameObject = global::Util.KInstantiateUI(this.overlayPrefab, this.uiParent.transform.gameObject, false);
				this.uiList.Add(gameObject);
			}
			else
			{
				List<GameObject> list = this.uiList;
				int num = this.freeUiIdx;
				this.freeUiIdx = num + 1;
				gameObject = list[num];
			}
			if (!gameObject.activeSelf)
			{
				gameObject.SetActive(true);
			}
			return gameObject;
		}

		// Token: 0x060093AF RID: 37807 RVA: 0x0039A424 File Offset: 0x00398624
		public override void Update()
		{
			this.freeUiIdx = 0;
			Vector2I vector2I;
			Vector2I vector2I2;
			Grid.GetVisibleExtents(out vector2I, out vector2I2);
			OverlayModes.Mode.RemoveOffscreenTargets<SaveLoadRoot>(this.layerTargets, vector2I, vector2I2, null);
			foreach (object obj in this.partition.GetAllIntersecting(new Vector2((float)vector2I.x, (float)vector2I.y), new Vector2((float)vector2I2.x, (float)vector2I2.y)))
			{
				SaveLoadRoot instance = (SaveLoadRoot)obj;
				base.AddTargetIfVisible<SaveLoadRoot>(instance, vector2I, vector2I2, this.layerTargets, this.targetLayer, null, null);
			}
			foreach (SaveLoadRoot saveLoadRoot in this.layerTargets)
			{
				if (!(saveLoadRoot == null))
				{
					saveLoadRoot.GetComponent<KBatchedAnimController>().TintColour = Color.white;
					bool flag = false;
					if (saveLoadRoot.GetComponent<KPrefabID>().HasTag(GameTags.Suit))
					{
						flag = true;
					}
					else
					{
						SuitLocker component = saveLoadRoot.GetComponent<SuitLocker>();
						if (component != null)
						{
							flag = (component.GetStoredOutfit() != null);
						}
					}
					if (flag)
					{
						this.GetFreeUI().GetComponent<RectTransform>().SetPosition(saveLoadRoot.transform.GetPosition());
					}
				}
			}
			for (int i = this.freeUiIdx; i < this.uiList.Count; i++)
			{
				if (this.uiList[i].activeSelf)
				{
					this.uiList[i].SetActive(false);
				}
			}
		}

		// Token: 0x04006FDA RID: 28634
		public static readonly HashedString ID = "Suit";

		// Token: 0x04006FDB RID: 28635
		private UniformGrid<SaveLoadRoot> partition;

		// Token: 0x04006FDC RID: 28636
		private HashSet<SaveLoadRoot> layerTargets = new HashSet<SaveLoadRoot>();

		// Token: 0x04006FDD RID: 28637
		private ICollection<Tag> targetIDs;

		// Token: 0x04006FDE RID: 28638
		private List<GameObject> uiList = new List<GameObject>();

		// Token: 0x04006FDF RID: 28639
		private int freeUiIdx;

		// Token: 0x04006FE0 RID: 28640
		private int targetLayer;

		// Token: 0x04006FE1 RID: 28641
		private int cameraLayerMask;

		// Token: 0x04006FE2 RID: 28642
		private int selectionMask;

		// Token: 0x04006FE3 RID: 28643
		private Canvas uiParent;

		// Token: 0x04006FE4 RID: 28644
		private GameObject overlayPrefab;
	}

	// Token: 0x02001B80 RID: 7040
	public class Temperature : OverlayModes.Mode
	{
		// Token: 0x060093B1 RID: 37809 RVA: 0x00104E3C File Offset: 0x0010303C
		public override HashedString ViewMode()
		{
			return OverlayModes.Temperature.ID;
		}

		// Token: 0x060093B2 RID: 37810 RVA: 0x00104E43 File Offset: 0x00103043
		public override string GetSoundName()
		{
			return "Temperature";
		}

		// Token: 0x060093B3 RID: 37811 RVA: 0x0039A5E0 File Offset: 0x003987E0
		public Temperature()
		{
			this.legendFilters = this.CreateDefaultFilters();
		}

		// Token: 0x060093B4 RID: 37812 RVA: 0x00104E4A File Offset: 0x0010304A
		public override void Update()
		{
			base.Update();
			if (this.previousUserSetting != SimDebugView.Instance.user_temperatureThresholds)
			{
				this.RefreshLegendValues();
				this.previousUserSetting = SimDebugView.Instance.user_temperatureThresholds;
			}
		}

		// Token: 0x060093B5 RID: 37813 RVA: 0x00104E7F File Offset: 0x0010307F
		public override void Enable()
		{
			base.Enable();
			this.previousUserSetting = SimDebugView.Instance.user_temperatureThresholds;
			this.RefreshLegendValues();
		}

		// Token: 0x060093B6 RID: 37814 RVA: 0x0039AC78 File Offset: 0x00398E78
		public void RefreshLegendValues()
		{
			int num = SimDebugView.Instance.temperatureThresholds.Length - 1;
			for (int i = 0; i < num; i++)
			{
				this.temperatureLegend[i].colour = GlobalAssets.Instance.colorSet.GetColorByName(SimDebugView.Instance.temperatureThresholds[num - i].colorName);
				this.temperatureLegend[i].desc_arg = GameUtil.GetFormattedTemperature(SimDebugView.Instance.temperatureThresholds[num - i].value, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false);
			}
		}

		// Token: 0x060093B7 RID: 37815 RVA: 0x00104E9D File Offset: 0x0010309D
		public override Dictionary<string, ToolParameterMenu.ToggleState> CreateDefaultFilters()
		{
			return new Dictionary<string, ToolParameterMenu.ToggleState>
			{
				{
					ToolParameterMenu.FILTERLAYERS.ABSOLUTETEMPERATURE,
					ToolParameterMenu.ToggleState.On
				},
				{
					ToolParameterMenu.FILTERLAYERS.RELATIVETEMPERATURE,
					ToolParameterMenu.ToggleState.Off
				},
				{
					ToolParameterMenu.FILTERLAYERS.HEATFLOW,
					ToolParameterMenu.ToggleState.Off
				},
				{
					ToolParameterMenu.FILTERLAYERS.STATECHANGE,
					ToolParameterMenu.ToggleState.Off
				}
			};
		}

		// Token: 0x060093B8 RID: 37816 RVA: 0x00104ED4 File Offset: 0x001030D4
		public override void OnRenderImage(RenderTexture src, RenderTexture dest)
		{
			if (Game.IsQuitting())
			{
				return;
			}
			KAnimBatchManager.Instance().RenderKAnimTemperaturePostProcessingEffects();
		}

		// Token: 0x060093B9 RID: 37817 RVA: 0x0039AD10 File Offset: 0x00398F10
		public override List<LegendEntry> GetCustomLegendData()
		{
			switch (Game.Instance.temperatureOverlayMode)
			{
			case Game.TemperatureOverlayModes.AbsoluteTemperature:
				return this.temperatureLegend;
			case Game.TemperatureOverlayModes.AdaptiveTemperature:
				return this.expandedTemperatureLegend;
			case Game.TemperatureOverlayModes.HeatFlow:
				return this.heatFlowLegend;
			case Game.TemperatureOverlayModes.StateChange:
				return this.stateChangeLegend;
			case Game.TemperatureOverlayModes.RelativeTemperature:
				return new List<LegendEntry>();
			default:
				return this.temperatureLegend;
			}
		}

		// Token: 0x060093BA RID: 37818 RVA: 0x0039AD6C File Offset: 0x00398F6C
		public override void OnFiltersChanged()
		{
			if (base.InFilter(ToolParameterMenu.FILTERLAYERS.HEATFLOW, this.legendFilters))
			{
				Game.Instance.temperatureOverlayMode = Game.TemperatureOverlayModes.HeatFlow;
			}
			if (base.InFilter(ToolParameterMenu.FILTERLAYERS.ABSOLUTETEMPERATURE, this.legendFilters))
			{
				Game.Instance.temperatureOverlayMode = Game.TemperatureOverlayModes.AbsoluteTemperature;
			}
			if (base.InFilter(ToolParameterMenu.FILTERLAYERS.RELATIVETEMPERATURE, this.legendFilters))
			{
				Game.Instance.temperatureOverlayMode = Game.TemperatureOverlayModes.RelativeTemperature;
			}
			if (base.InFilter(ToolParameterMenu.FILTERLAYERS.ADAPTIVETEMPERATURE, this.legendFilters))
			{
				Game.Instance.temperatureOverlayMode = Game.TemperatureOverlayModes.AdaptiveTemperature;
			}
			if (base.InFilter(ToolParameterMenu.FILTERLAYERS.STATECHANGE, this.legendFilters))
			{
				Game.Instance.temperatureOverlayMode = Game.TemperatureOverlayModes.StateChange;
			}
			switch (Game.Instance.temperatureOverlayMode)
			{
			case Game.TemperatureOverlayModes.AbsoluteTemperature:
				Infrared.Instance.SetMode(Infrared.Mode.Infrared);
				CameraController.Instance.ToggleColouredOverlayView(true);
				return;
			case Game.TemperatureOverlayModes.AdaptiveTemperature:
				Infrared.Instance.SetMode(Infrared.Mode.Infrared);
				CameraController.Instance.ToggleColouredOverlayView(true);
				return;
			case Game.TemperatureOverlayModes.HeatFlow:
				Infrared.Instance.SetMode(Infrared.Mode.Disabled);
				CameraController.Instance.ToggleColouredOverlayView(false);
				return;
			case Game.TemperatureOverlayModes.StateChange:
				Infrared.Instance.SetMode(Infrared.Mode.Disabled);
				CameraController.Instance.ToggleColouredOverlayView(false);
				return;
			case Game.TemperatureOverlayModes.RelativeTemperature:
				Infrared.Instance.SetMode(Infrared.Mode.Infrared);
				CameraController.Instance.ToggleColouredOverlayView(true);
				return;
			default:
				return;
			}
		}

		// Token: 0x060093BB RID: 37819 RVA: 0x00104EE8 File Offset: 0x001030E8
		public override void Disable()
		{
			Infrared.Instance.SetMode(Infrared.Mode.Disabled);
			CameraController.Instance.ToggleColouredOverlayView(false);
			base.Disable();
		}

		// Token: 0x04006FE5 RID: 28645
		public static readonly HashedString ID = "Temperature";

		// Token: 0x04006FE6 RID: 28646
		private Vector2 previousUserSetting;

		// Token: 0x04006FE7 RID: 28647
		public List<LegendEntry> temperatureLegend = new List<LegendEntry>
		{
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.MAXHOT, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0.8901961f, 0.13725491f, 0.12941177f), null, null, true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.EXTREMEHOT, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0.9843137f, 0.3254902f, 0.3137255f), null, null, true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.VERYHOT, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(1f, 0.6627451f, 0.14117648f), null, null, true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.HOT, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0.9372549f, 1f, 0f), null, null, true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.TEMPERATE, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0.23137255f, 0.99607843f, 0.2901961f), null, null, true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.COLD, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0.12156863f, 0.6313726f, 1f), null, null, true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.VERYCOLD, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0.16862746f, 0.79607844f, 1f), null, null, true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.EXTREMECOLD, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0.5019608f, 0.99607843f, 0.9411765f), null, null, true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.HEATSOURCES, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.HEATSOURCES, Color.white, null, Assets.GetSprite("heat_source"), true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.HEATSINK, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.HEATSINK, Color.white, null, Assets.GetSprite("heat_sink"), true)
		};

		// Token: 0x04006FE8 RID: 28648
		public List<LegendEntry> heatFlowLegend = new List<LegendEntry>
		{
			new LegendEntry(UI.OVERLAYS.HEATFLOW.HEATING, UI.OVERLAYS.HEATFLOW.TOOLTIPS.HEATING, new Color(0.9098039f, 0.25882354f, 0.14901961f), null, null, true),
			new LegendEntry(UI.OVERLAYS.HEATFLOW.NEUTRAL, UI.OVERLAYS.HEATFLOW.TOOLTIPS.NEUTRAL, new Color(0.30980393f, 0.30980393f, 0.30980393f), null, null, true),
			new LegendEntry(UI.OVERLAYS.HEATFLOW.COOLING, UI.OVERLAYS.HEATFLOW.TOOLTIPS.COOLING, new Color(0.2509804f, 0.6313726f, 0.90588236f), null, null, true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.HEATSOURCES, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.HEATSOURCES, Color.white, null, Assets.GetSprite("heat_source"), true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.HEATSINK, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.HEATSINK, Color.white, null, Assets.GetSprite("heat_sink"), true)
		};

		// Token: 0x04006FE9 RID: 28649
		public List<LegendEntry> expandedTemperatureLegend = new List<LegendEntry>
		{
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.MAXHOT, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0.8901961f, 0.13725491f, 0.12941177f), null, null, true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.EXTREMEHOT, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0.9843137f, 0.3254902f, 0.3137255f), null, null, true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.VERYHOT, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(1f, 0.6627451f, 0.14117648f), null, null, true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.HOT, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0.9372549f, 1f, 0f), null, null, true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.TEMPERATE, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0.23137255f, 0.99607843f, 0.2901961f), null, null, true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.COLD, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0.12156863f, 0.6313726f, 1f), null, null, true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.VERYCOLD, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0.16862746f, 0.79607844f, 1f), null, null, true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.EXTREMECOLD, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0.5019608f, 0.99607843f, 0.9411765f), null, null, true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.HEATSOURCES, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.HEATSOURCES, Color.white, null, Assets.GetSprite("heat_source"), true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.HEATSINK, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.HEATSINK, Color.white, null, Assets.GetSprite("heat_sink"), true)
		};

		// Token: 0x04006FEA RID: 28650
		public List<LegendEntry> stateChangeLegend = new List<LegendEntry>
		{
			new LegendEntry(UI.OVERLAYS.STATECHANGE.HIGHPOINT, UI.OVERLAYS.STATECHANGE.TOOLTIPS.HIGHPOINT, new Color(0.8901961f, 0.13725491f, 0.12941177f), null, null, true),
			new LegendEntry(UI.OVERLAYS.STATECHANGE.STABLE, UI.OVERLAYS.STATECHANGE.TOOLTIPS.STABLE, new Color(0.23137255f, 0.99607843f, 0.2901961f), null, null, true),
			new LegendEntry(UI.OVERLAYS.STATECHANGE.LOWPOINT, UI.OVERLAYS.STATECHANGE.TOOLTIPS.LOWPOINT, new Color(0.5019608f, 0.99607843f, 0.9411765f), null, null, true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.HEATSOURCES, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.HEATSOURCES, Color.white, null, Assets.GetSprite("heat_source"), true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.HEATSINK, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.HEATSINK, Color.white, null, Assets.GetSprite("heat_sink"), true)
		};
	}

	// Token: 0x02001B81 RID: 7041
	public class TileMode : OverlayModes.Mode
	{
		// Token: 0x060093BD RID: 37821 RVA: 0x00104F17 File Offset: 0x00103117
		public override HashedString ViewMode()
		{
			return OverlayModes.TileMode.ID;
		}

		// Token: 0x060093BE RID: 37822 RVA: 0x00104E24 File Offset: 0x00103024
		public override string GetSoundName()
		{
			return "SuitRequired";
		}

		// Token: 0x060093BF RID: 37823 RVA: 0x0039AEA8 File Offset: 0x003990A8
		public TileMode()
		{
			OverlayModes.ColorHighlightCondition[] array = new OverlayModes.ColorHighlightCondition[1];
			array[0] = new OverlayModes.ColorHighlightCondition(delegate(KMonoBehaviour primary_element)
			{
				Color result = Color.black;
				if (primary_element != null)
				{
					result = (primary_element as PrimaryElement).Element.substance.uiColour;
				}
				return result;
			}, (KMonoBehaviour primary_element) => primary_element.gameObject.GetComponent<KBatchedAnimController>().IsVisible());
			this.highlightConditions = array;
			base..ctor();
			this.targetLayer = LayerMask.NameToLayer("MaskedOverlay");
			this.cameraLayerMask = LayerMask.GetMask(new string[]
			{
				"MaskedOverlay",
				"MaskedOverlayBG"
			});
			this.legendFilters = this.CreateDefaultFilters();
		}

		// Token: 0x060093C0 RID: 37824 RVA: 0x0039AF60 File Offset: 0x00399160
		public override void Enable()
		{
			base.Enable();
			List<Tag> prefabTagsWithComponent = Assets.GetPrefabTagsWithComponent<PrimaryElement>();
			this.targetIDs.UnionWith(prefabTagsWithComponent);
			Camera.main.cullingMask |= this.cameraLayerMask;
			int defaultLayerMask = SelectTool.Instance.GetDefaultLayerMask();
			int mask = LayerMask.GetMask(new string[]
			{
				"MaskedOverlay"
			});
			SelectTool.Instance.SetLayerMask(defaultLayerMask | mask);
		}

		// Token: 0x060093C1 RID: 37825 RVA: 0x0039AFC8 File Offset: 0x003991C8
		public override void Update()
		{
			Vector2I vector2I;
			Vector2I vector2I2;
			Grid.GetVisibleExtents(out vector2I, out vector2I2);
			OverlayModes.Mode.RemoveOffscreenTargets<PrimaryElement>(this.layerTargets, vector2I, vector2I2, null);
			int height = vector2I2.y - vector2I.y;
			int width = vector2I2.x - vector2I.x;
			Extents extents = new Extents(vector2I.x, vector2I.y, width, height);
			List<ScenePartitionerEntry> list = new List<ScenePartitionerEntry>();
			GameScenePartitioner.Instance.GatherEntries(extents, GameScenePartitioner.Instance.pickupablesLayer, list);
			foreach (ScenePartitionerEntry scenePartitionerEntry in list)
			{
				PrimaryElement component = ((Pickupable)scenePartitionerEntry.obj).gameObject.GetComponent<PrimaryElement>();
				if (component != null)
				{
					this.TryAddObject(component, vector2I, vector2I2);
				}
			}
			list.Clear();
			GameScenePartitioner.Instance.GatherEntries(extents, GameScenePartitioner.Instance.completeBuildings, list);
			foreach (ScenePartitionerEntry scenePartitionerEntry2 in list)
			{
				BuildingComplete buildingComplete = (BuildingComplete)scenePartitionerEntry2.obj;
				PrimaryElement component2 = buildingComplete.gameObject.GetComponent<PrimaryElement>();
				if (component2 != null && buildingComplete.gameObject.layer == 0)
				{
					this.TryAddObject(component2, vector2I, vector2I2);
				}
			}
			base.UpdateHighlightTypeOverlay<PrimaryElement>(vector2I, vector2I2, this.layerTargets, this.targetIDs, this.highlightConditions, OverlayModes.BringToFrontLayerSetting.Conditional, this.targetLayer);
		}

		// Token: 0x060093C2 RID: 37826 RVA: 0x0039B154 File Offset: 0x00399354
		private void TryAddObject(PrimaryElement pe, Vector2I min, Vector2I max)
		{
			Element element = pe.Element;
			foreach (Tag search_tag in Game.Instance.tileOverlayFilters)
			{
				if (element.HasTag(search_tag))
				{
					base.AddTargetIfVisible<PrimaryElement>(pe, min, max, this.layerTargets, this.targetLayer, null, null);
					break;
				}
			}
		}

		// Token: 0x060093C3 RID: 37827 RVA: 0x0039B1D0 File Offset: 0x003993D0
		public override void Disable()
		{
			base.Disable();
			base.DisableHighlightTypeOverlay<PrimaryElement>(this.layerTargets);
			Camera.main.cullingMask &= ~this.cameraLayerMask;
			this.layerTargets.Clear();
			SelectTool.Instance.ClearLayerMask();
		}

		// Token: 0x060093C4 RID: 37828 RVA: 0x0039B21C File Offset: 0x0039941C
		public override Dictionary<string, ToolParameterMenu.ToggleState> CreateDefaultFilters()
		{
			return new Dictionary<string, ToolParameterMenu.ToggleState>
			{
				{
					ToolParameterMenu.FILTERLAYERS.ALL,
					ToolParameterMenu.ToggleState.On
				},
				{
					ToolParameterMenu.FILTERLAYERS.METAL,
					ToolParameterMenu.ToggleState.Off
				},
				{
					ToolParameterMenu.FILTERLAYERS.BUILDABLE,
					ToolParameterMenu.ToggleState.Off
				},
				{
					ToolParameterMenu.FILTERLAYERS.FILTER,
					ToolParameterMenu.ToggleState.Off
				},
				{
					ToolParameterMenu.FILTERLAYERS.CONSUMABLEORE,
					ToolParameterMenu.ToggleState.Off
				},
				{
					ToolParameterMenu.FILTERLAYERS.ORGANICS,
					ToolParameterMenu.ToggleState.Off
				},
				{
					ToolParameterMenu.FILTERLAYERS.FARMABLE,
					ToolParameterMenu.ToggleState.Off
				},
				{
					ToolParameterMenu.FILTERLAYERS.LIQUIFIABLE,
					ToolParameterMenu.ToggleState.Off
				},
				{
					ToolParameterMenu.FILTERLAYERS.GAS,
					ToolParameterMenu.ToggleState.Off
				},
				{
					ToolParameterMenu.FILTERLAYERS.LIQUID,
					ToolParameterMenu.ToggleState.Off
				},
				{
					ToolParameterMenu.FILTERLAYERS.MISC,
					ToolParameterMenu.ToggleState.Off
				}
			};
		}

		// Token: 0x060093C5 RID: 37829 RVA: 0x0039B2B4 File Offset: 0x003994B4
		public override void OnFiltersChanged()
		{
			Game.Instance.tileOverlayFilters.Clear();
			if (base.InFilter(ToolParameterMenu.FILTERLAYERS.METAL, this.legendFilters))
			{
				Game.Instance.tileOverlayFilters.Add(GameTags.Metal);
				Game.Instance.tileOverlayFilters.Add(GameTags.RefinedMetal);
			}
			if (base.InFilter(ToolParameterMenu.FILTERLAYERS.BUILDABLE, this.legendFilters))
			{
				Game.Instance.tileOverlayFilters.Add(GameTags.BuildableRaw);
				Game.Instance.tileOverlayFilters.Add(GameTags.BuildableProcessed);
			}
			if (base.InFilter(ToolParameterMenu.FILTERLAYERS.FILTER, this.legendFilters))
			{
				Game.Instance.tileOverlayFilters.Add(GameTags.Filter);
			}
			if (base.InFilter(ToolParameterMenu.FILTERLAYERS.LIQUIFIABLE, this.legendFilters))
			{
				Game.Instance.tileOverlayFilters.Add(GameTags.Liquifiable);
			}
			if (base.InFilter(ToolParameterMenu.FILTERLAYERS.LIQUID, this.legendFilters))
			{
				Game.Instance.tileOverlayFilters.Add(GameTags.Liquid);
			}
			if (base.InFilter(ToolParameterMenu.FILTERLAYERS.CONSUMABLEORE, this.legendFilters))
			{
				Game.Instance.tileOverlayFilters.Add(GameTags.ConsumableOre);
				Game.Instance.tileOverlayFilters.Add(GameTags.Sublimating);
			}
			if (base.InFilter(ToolParameterMenu.FILTERLAYERS.ORGANICS, this.legendFilters))
			{
				Game.Instance.tileOverlayFilters.Add(GameTags.Organics);
			}
			if (base.InFilter(ToolParameterMenu.FILTERLAYERS.FARMABLE, this.legendFilters))
			{
				Game.Instance.tileOverlayFilters.Add(GameTags.Farmable);
				Game.Instance.tileOverlayFilters.Add(GameTags.Agriculture);
			}
			if (base.InFilter(ToolParameterMenu.FILTERLAYERS.GAS, this.legendFilters))
			{
				Game.Instance.tileOverlayFilters.Add(GameTags.Breathable);
				Game.Instance.tileOverlayFilters.Add(GameTags.Unbreathable);
			}
			if (base.InFilter(ToolParameterMenu.FILTERLAYERS.MISC, this.legendFilters))
			{
				Game.Instance.tileOverlayFilters.Add(GameTags.Other);
			}
			base.DisableHighlightTypeOverlay<PrimaryElement>(this.layerTargets);
			this.layerTargets.Clear();
			Game.Instance.ForceOverlayUpdate(false);
		}

		// Token: 0x04006FEB RID: 28651
		public static readonly HashedString ID = "TileMode";

		// Token: 0x04006FEC RID: 28652
		private HashSet<PrimaryElement> layerTargets = new HashSet<PrimaryElement>();

		// Token: 0x04006FED RID: 28653
		private HashSet<Tag> targetIDs = new HashSet<Tag>();

		// Token: 0x04006FEE RID: 28654
		private int targetLayer;

		// Token: 0x04006FEF RID: 28655
		private int cameraLayerMask;

		// Token: 0x04006FF0 RID: 28656
		private OverlayModes.ColorHighlightCondition[] highlightConditions;
	}
}
