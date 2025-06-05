using System;
using System.Collections.Generic;
using ProcGen;
using UnityEngine;

// Token: 0x0200109F RID: 4255
[AddComponentMenu("KMonoBehaviour/scripts/CellSelectionObject")]
public class CellSelectionObject : KMonoBehaviour
{
	// Token: 0x170004F8 RID: 1272
	// (get) Token: 0x06005666 RID: 22118 RVA: 0x000DCC4C File Offset: 0x000DAE4C
	public int SelectedCell
	{
		get
		{
			return this.selectedCell;
		}
	}

	// Token: 0x170004F9 RID: 1273
	// (get) Token: 0x06005667 RID: 22119 RVA: 0x000DCC54 File Offset: 0x000DAE54
	public float FlowRate
	{
		get
		{
			return Grid.AccumulatedFlow[this.selectedCell] / 3f;
		}
	}

	// Token: 0x06005668 RID: 22120 RVA: 0x0028FAC8 File Offset: 0x0028DCC8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.mCollider = base.GetComponent<KBoxCollider2D>();
		this.mCollider.size = new Vector2(1.1f, 1.1f);
		this.mSelectable = base.GetComponent<KSelectable>();
		this.SelectedDisplaySprite.transform.localScale = Vector3.one * 0.390625f;
		this.SelectedDisplaySprite.GetComponent<SpriteRenderer>().sprite = this.Sprite_Hover;
		base.Subscribe(Game.Instance.gameObject, 493375141, new Action<object>(this.ForceRefreshUserMenu));
		this.overlayFilterMap.Add(OverlayModes.Oxygen.ID, () => Grid.Element[this.mouseCell].IsGas);
		this.overlayFilterMap.Add(OverlayModes.GasConduits.ID, () => Grid.Element[this.mouseCell].IsGas);
		this.overlayFilterMap.Add(OverlayModes.LiquidConduits.ID, () => Grid.Element[this.mouseCell].IsLiquid);
		if (CellSelectionObject.selectionObjectA == null)
		{
			CellSelectionObject.selectionObjectA = this;
			return;
		}
		if (CellSelectionObject.selectionObjectB == null)
		{
			CellSelectionObject.selectionObjectB = this;
			return;
		}
		global::Debug.LogError("CellSelectionObjects not properly cleaned up.");
	}

	// Token: 0x06005669 RID: 22121 RVA: 0x000DCC6C File Offset: 0x000DAE6C
	protected override void OnCleanUp()
	{
		CellSelectionObject.selectionObjectA = null;
		CellSelectionObject.selectionObjectB = null;
		base.OnCleanUp();
	}

	// Token: 0x0600566A RID: 22122 RVA: 0x000DCC80 File Offset: 0x000DAE80
	public static bool IsSelectionObject(GameObject testObject)
	{
		return testObject == CellSelectionObject.selectionObjectA.gameObject || testObject == CellSelectionObject.selectionObjectB.gameObject;
	}

	// Token: 0x0600566B RID: 22123 RVA: 0x000DCCA6 File Offset: 0x000DAEA6
	private void OnApplicationFocus(bool focusStatus)
	{
		this.isAppFocused = focusStatus;
	}

	// Token: 0x0600566C RID: 22124 RVA: 0x0028FBEC File Offset: 0x0028DDEC
	private void Update()
	{
		if (!this.isAppFocused || SelectTool.Instance == null)
		{
			return;
		}
		if (Game.Instance == null || !Game.Instance.GameStarted())
		{
			return;
		}
		this.SelectedDisplaySprite.SetActive(PlayerController.Instance.IsUsingDefaultTool() && !DebugHandler.HideUI);
		if (SelectTool.Instance.selected != this.mSelectable)
		{
			this.mouseCell = Grid.PosToCell(CameraController.Instance.baseCamera.ScreenToWorldPoint(KInputManager.GetMousePos()));
			if (Grid.IsValidCell(this.mouseCell) && Grid.IsVisible(this.mouseCell))
			{
				bool flag = true;
				foreach (KeyValuePair<HashedString, Func<bool>> keyValuePair in this.overlayFilterMap)
				{
					if (keyValuePair.Value == null)
					{
						global::Debug.LogWarning("Filter value is null");
					}
					else if (OverlayScreen.Instance == null)
					{
						global::Debug.LogWarning("Overlay screen Instance is null");
					}
					else if (OverlayScreen.Instance.GetMode() == keyValuePair.Key)
					{
						flag = false;
						if (base.gameObject.layer != LayerMask.NameToLayer("MaskedOverlay"))
						{
							base.gameObject.layer = LayerMask.NameToLayer("MaskedOverlay");
						}
						if (!keyValuePair.Value())
						{
							this.SelectedDisplaySprite.SetActive(false);
							return;
						}
						break;
					}
				}
				if (flag && base.gameObject.layer != LayerMask.NameToLayer("Default"))
				{
					base.gameObject.layer = LayerMask.NameToLayer("Default");
				}
				Vector3 position = Grid.CellToPos(this.mouseCell, 0f, 0f, 0f) + this.offset;
				position.z = this.zDepth;
				base.transform.SetPosition(position);
				this.mSelectable.SetName(Grid.Element[this.mouseCell].name);
			}
			if (SelectTool.Instance.hover != this.mSelectable)
			{
				this.SelectedDisplaySprite.SetActive(false);
			}
		}
		this.updateTimer += Time.deltaTime;
		if (this.updateTimer >= 0.5f)
		{
			this.updateTimer = 0f;
			if (SelectTool.Instance.selected == this.mSelectable)
			{
				this.UpdateValues();
			}
		}
	}

	// Token: 0x0600566D RID: 22125 RVA: 0x0028FE74 File Offset: 0x0028E074
	public void UpdateValues()
	{
		if (!Grid.IsValidCell(this.selectedCell))
		{
			return;
		}
		this.Mass = Grid.Mass[this.selectedCell];
		this.element = Grid.Element[this.selectedCell];
		this.ElementName = this.element.name;
		this.state = this.element.state;
		this.tags = this.element.GetMaterialCategoryTag();
		this.temperature = Grid.Temperature[this.selectedCell];
		this.diseaseIdx = Grid.DiseaseIdx[this.selectedCell];
		this.diseaseCount = Grid.DiseaseCount[this.selectedCell];
		this.mSelectable.SetName(Grid.Element[this.selectedCell].name);
		DetailsScreen.Instance.Trigger(-1514841199, null);
		this.UpdateStatusItem();
		int num = Grid.CellAbove(this.selectedCell);
		bool flag = this.element.IsLiquid && Grid.IsValidCell(num) && (Grid.Element[num].IsGas || Grid.Element[num].IsVacuum);
		if (this.element.sublimateId != (SimHashes)0 && (this.element.IsSolid || flag))
		{
			this.mSelectable.AddStatusItem(Db.Get().MiscStatusItems.SublimationEmitting, this);
			bool flag2;
			bool flag3;
			GameUtil.IsEmissionBlocked(this.selectedCell, out flag2, out flag3);
			if (flag2)
			{
				this.mSelectable.AddStatusItem(Db.Get().MiscStatusItems.SublimationBlocked, this);
				this.mSelectable.RemoveStatusItem(Db.Get().MiscStatusItems.SublimationOverpressure, false);
			}
			else if (flag3)
			{
				this.mSelectable.AddStatusItem(Db.Get().MiscStatusItems.SublimationOverpressure, this);
				this.mSelectable.RemoveStatusItem(Db.Get().MiscStatusItems.SublimationBlocked, false);
			}
			else
			{
				this.mSelectable.RemoveStatusItem(Db.Get().MiscStatusItems.SublimationOverpressure, false);
				this.mSelectable.RemoveStatusItem(Db.Get().MiscStatusItems.SublimationBlocked, false);
			}
		}
		else
		{
			this.mSelectable.RemoveStatusItem(Db.Get().MiscStatusItems.SublimationEmitting, false);
			this.mSelectable.RemoveStatusItem(Db.Get().MiscStatusItems.SublimationBlocked, false);
			this.mSelectable.RemoveStatusItem(Db.Get().MiscStatusItems.SublimationOverpressure, false);
		}
		if (Game.Instance.GetComponent<EntombedItemVisualizer>().IsEntombedItem(this.selectedCell))
		{
			this.mSelectable.AddStatusItem(Db.Get().MiscStatusItems.BuriedItem, this);
		}
		else
		{
			this.mSelectable.RemoveStatusItem(Db.Get().MiscStatusItems.BuriedItem, true);
		}
		bool on = CellSelectionObject.IsExposedToSpace(this.selectedCell);
		this.mSelectable.ToggleStatusItem(Db.Get().MiscStatusItems.Space, on, null);
	}

	// Token: 0x0600566E RID: 22126 RVA: 0x000DCCAF File Offset: 0x000DAEAF
	public static bool IsExposedToSpace(int cell)
	{
		return Game.Instance.world.zoneRenderData.GetSubWorldZoneType(cell) == SubWorld.ZoneType.Space && Grid.Objects[cell, 2] == null;
	}

	// Token: 0x0600566F RID: 22127 RVA: 0x00290178 File Offset: 0x0028E378
	private void UpdateStatusItem()
	{
		if (this.element.id == SimHashes.Vacuum || this.element.id == SimHashes.Void)
		{
			this.mSelectable.RemoveStatusItem(Db.Get().MiscStatusItems.ElementalCategory, true);
			this.mSelectable.RemoveStatusItem(Db.Get().MiscStatusItems.ElementalTemperature, true);
			this.mSelectable.RemoveStatusItem(Db.Get().MiscStatusItems.ElementalMass, true);
			this.mSelectable.RemoveStatusItem(Db.Get().MiscStatusItems.ElementalDisease, true);
			return;
		}
		if (!this.mSelectable.HasStatusItem(Db.Get().MiscStatusItems.ElementalCategory))
		{
			Func<Element> data = () => this.element;
			this.mSelectable.AddStatusItem(Db.Get().MiscStatusItems.ElementalCategory, data);
		}
		if (!this.mSelectable.HasStatusItem(Db.Get().MiscStatusItems.ElementalTemperature))
		{
			this.mSelectable.AddStatusItem(Db.Get().MiscStatusItems.ElementalTemperature, this);
		}
		if (!this.mSelectable.HasStatusItem(Db.Get().MiscStatusItems.ElementalMass))
		{
			this.mSelectable.AddStatusItem(Db.Get().MiscStatusItems.ElementalMass, this);
		}
		if (!this.mSelectable.HasStatusItem(Db.Get().MiscStatusItems.ElementalDisease))
		{
			this.mSelectable.AddStatusItem(Db.Get().MiscStatusItems.ElementalDisease, this);
		}
	}

	// Token: 0x06005670 RID: 22128 RVA: 0x00290308 File Offset: 0x0028E508
	public void OnObjectSelected(object o)
	{
		this.SelectedDisplaySprite.GetComponent<SpriteRenderer>().sprite = this.Sprite_Hover;
		this.UpdateStatusItem();
		if (SelectTool.Instance.selected == this.mSelectable)
		{
			this.selectedCell = Grid.PosToCell(base.gameObject);
			this.UpdateValues();
			Vector3 position = Grid.CellToPos(this.selectedCell, 0f, 0f, 0f) + this.offset;
			position.z = this.zDepthSelected;
			base.transform.SetPosition(position);
			this.SelectedDisplaySprite.GetComponent<SpriteRenderer>().sprite = this.Sprite_Selected;
		}
	}

	// Token: 0x06005671 RID: 22129 RVA: 0x000DCCDD File Offset: 0x000DAEDD
	public string MassString()
	{
		return string.Format("{0:0.00}", this.Mass);
	}

	// Token: 0x06005672 RID: 22130 RVA: 0x000DCCF4 File Offset: 0x000DAEF4
	private void ForceRefreshUserMenu(object data)
	{
		Game.Instance.userMenu.Refresh(base.gameObject);
	}

	// Token: 0x04003D27 RID: 15655
	private static CellSelectionObject selectionObjectA;

	// Token: 0x04003D28 RID: 15656
	private static CellSelectionObject selectionObjectB;

	// Token: 0x04003D29 RID: 15657
	[HideInInspector]
	public CellSelectionObject alternateSelectionObject;

	// Token: 0x04003D2A RID: 15658
	private float zDepth = Grid.GetLayerZ(Grid.SceneLayer.WorldSelection) - 0.5f;

	// Token: 0x04003D2B RID: 15659
	private float zDepthSelected = Grid.GetLayerZ(Grid.SceneLayer.WorldSelection);

	// Token: 0x04003D2C RID: 15660
	private KBoxCollider2D mCollider;

	// Token: 0x04003D2D RID: 15661
	private KSelectable mSelectable;

	// Token: 0x04003D2E RID: 15662
	private Vector3 offset = new Vector3(0.5f, 0.5f, 0f);

	// Token: 0x04003D2F RID: 15663
	public GameObject SelectedDisplaySprite;

	// Token: 0x04003D30 RID: 15664
	public Sprite Sprite_Selected;

	// Token: 0x04003D31 RID: 15665
	public Sprite Sprite_Hover;

	// Token: 0x04003D32 RID: 15666
	public int mouseCell;

	// Token: 0x04003D33 RID: 15667
	private int selectedCell;

	// Token: 0x04003D34 RID: 15668
	public string ElementName;

	// Token: 0x04003D35 RID: 15669
	public Element element;

	// Token: 0x04003D36 RID: 15670
	public Element.State state;

	// Token: 0x04003D37 RID: 15671
	public float Mass;

	// Token: 0x04003D38 RID: 15672
	public float temperature;

	// Token: 0x04003D39 RID: 15673
	public Tag tags;

	// Token: 0x04003D3A RID: 15674
	public byte diseaseIdx;

	// Token: 0x04003D3B RID: 15675
	public int diseaseCount;

	// Token: 0x04003D3C RID: 15676
	private float updateTimer;

	// Token: 0x04003D3D RID: 15677
	private Dictionary<HashedString, Func<bool>> overlayFilterMap = new Dictionary<HashedString, Func<bool>>();

	// Token: 0x04003D3E RID: 15678
	private bool isAppFocused = true;
}
