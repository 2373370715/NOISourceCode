using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001EE7 RID: 7911
public class OverlayLegend : KScreen
{
	// Token: 0x0600A601 RID: 42497 RVA: 0x003FC5F0 File Offset: 0x003FA7F0
	[ContextMenu("Set all fonts color")]
	public void SetAllFontsColor()
	{
		foreach (OverlayLegend.OverlayInfo overlayInfo in this.overlayInfoList)
		{
			for (int i = 0; i < overlayInfo.infoUnits.Count; i++)
			{
				if (overlayInfo.infoUnits[i].fontColor == Color.clear)
				{
					overlayInfo.infoUnits[i].fontColor = Color.white;
				}
			}
		}
	}

	// Token: 0x0600A602 RID: 42498 RVA: 0x003FC688 File Offset: 0x003FA888
	[ContextMenu("Set all tooltips")]
	public void SetAllTooltips()
	{
		foreach (OverlayLegend.OverlayInfo overlayInfo in this.overlayInfoList)
		{
			string text = overlayInfo.name;
			text = text.Replace("NAME", "");
			for (int i = 0; i < overlayInfo.infoUnits.Count; i++)
			{
				string text2 = overlayInfo.infoUnits[i].description;
				text2 = text2.Replace(text, "");
				text2 = text + "TOOLTIPS." + text2;
				overlayInfo.infoUnits[i].tooltip = text2;
			}
		}
	}

	// Token: 0x0600A603 RID: 42499 RVA: 0x003FC74C File Offset: 0x003FA94C
	[ContextMenu("Set Sliced for empty icons")]
	public void SetSlicedForEmptyIcons()
	{
		foreach (OverlayLegend.OverlayInfo overlayInfo in this.overlayInfoList)
		{
			for (int i = 0; i < overlayInfo.infoUnits.Count; i++)
			{
				if (overlayInfo.infoUnits[i].icon == this.emptySprite)
				{
					overlayInfo.infoUnits[i].sliceIcon = true;
				}
			}
		}
	}

	// Token: 0x0600A604 RID: 42500 RVA: 0x003FC7E0 File Offset: 0x003FA9E0
	protected override void OnSpawn()
	{
		base.ConsumeMouseScroll = true;
		base.OnSpawn();
		if (OverlayLegend.Instance == null)
		{
			OverlayLegend.Instance = this;
			this.activeUnitObjs = new List<GameObject>();
			this.inactiveUnitObjs = new List<GameObject>();
			foreach (OverlayLegend.OverlayInfo overlayInfo in this.overlayInfoList)
			{
				overlayInfo.name = Strings.Get(overlayInfo.name);
				for (int i = 0; i < overlayInfo.infoUnits.Count; i++)
				{
					overlayInfo.infoUnits[i].description = Strings.Get(overlayInfo.infoUnits[i].description);
					if (!string.IsNullOrEmpty(overlayInfo.infoUnits[i].tooltip))
					{
						overlayInfo.infoUnits[i].tooltip = Strings.Get(overlayInfo.infoUnits[i].tooltip);
					}
				}
			}
			base.GetComponent<LayoutElement>().minWidth = (float)(DlcManager.FeatureClusterSpaceEnabled() ? 322 : 288);
			this.ClearLegend();
			return;
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x0600A605 RID: 42501 RVA: 0x00110238 File Offset: 0x0010E438
	protected override void OnLoadLevel()
	{
		OverlayLegend.Instance = null;
		this.activeDiagrams.Clear();
		UnityEngine.Object.Destroy(base.gameObject);
		base.OnLoadLevel();
	}

	// Token: 0x0600A606 RID: 42502 RVA: 0x003FC93C File Offset: 0x003FAB3C
	private void SetLegend(OverlayLegend.OverlayInfo overlayInfo)
	{
		if (overlayInfo == null)
		{
			this.ClearLegend();
			return;
		}
		if (!overlayInfo.isProgrammaticallyPopulated && (overlayInfo.infoUnits == null || overlayInfo.infoUnits.Count == 0))
		{
			this.ClearLegend();
			return;
		}
		this.Show(true);
		this.title.text = overlayInfo.name;
		if (overlayInfo.isProgrammaticallyPopulated)
		{
			this.PopulateGeneratedLegend(overlayInfo, false);
		}
		else
		{
			this.PopulateOverlayInfoUnits(overlayInfo, false);
			this.PopulateOverlayDiagrams(overlayInfo, false);
		}
		this.ConfigureUIHeight();
	}

	// Token: 0x0600A607 RID: 42503 RVA: 0x003FC9B8 File Offset: 0x003FABB8
	public void SetLegend(OverlayModes.Mode mode, bool refreshing = false)
	{
		if (this.currentMode != null && this.currentMode.ViewMode() == mode.ViewMode() && !refreshing)
		{
			return;
		}
		this.ClearLegend();
		OverlayLegend.OverlayInfo legend = this.overlayInfoList.Find((OverlayLegend.OverlayInfo ol) => ol.mode == mode.ViewMode());
		this.currentMode = mode;
		this.SetLegend(legend);
	}

	// Token: 0x0600A608 RID: 42504 RVA: 0x003FCA2C File Offset: 0x003FAC2C
	public GameObject GetFreeUnitObject()
	{
		if (this.inactiveUnitObjs.Count == 0)
		{
			this.inactiveUnitObjs.Add(Util.KInstantiateUI(this.unitPrefab, this.inactiveUnitsParent, false));
		}
		GameObject gameObject = this.inactiveUnitObjs[0];
		this.inactiveUnitObjs.RemoveAt(0);
		this.activeUnitObjs.Add(gameObject);
		return gameObject;
	}

	// Token: 0x0600A609 RID: 42505 RVA: 0x003FCA8C File Offset: 0x003FAC8C
	private void RemoveActiveObjects()
	{
		while (this.activeUnitObjs.Count > 0)
		{
			this.activeUnitObjs[0].transform.Find("Icon").GetComponent<Image>().enabled = false;
			this.activeUnitObjs[0].GetComponentInChildren<LocText>().enabled = false;
			this.activeUnitObjs[0].transform.SetParent(this.inactiveUnitsParent.transform);
			this.activeUnitObjs[0].SetActive(false);
			this.inactiveUnitObjs.Add(this.activeUnitObjs[0]);
			this.activeUnitObjs.RemoveAt(0);
		}
	}

	// Token: 0x0600A60A RID: 42506 RVA: 0x0011025C File Offset: 0x0010E45C
	public void ClearLegend()
	{
		this.RemoveActiveObjects();
		this.ClearFilters();
		this.ClearDiagrams();
		this.Show(false);
	}

	// Token: 0x0600A60B RID: 42507 RVA: 0x00110277 File Offset: 0x0010E477
	public void ClearFilters()
	{
		if (this.filterMenu != null)
		{
			UnityEngine.Object.Destroy(this.filterMenu.gameObject);
		}
		this.filterMenu = null;
	}

	// Token: 0x0600A60C RID: 42508 RVA: 0x003FCB44 File Offset: 0x003FAD44
	public void ClearDiagrams()
	{
		for (int i = 0; i < this.activeDiagrams.Count; i++)
		{
			if (this.activeDiagrams[i] != null)
			{
				UnityEngine.Object.Destroy(this.activeDiagrams[i]);
			}
		}
		this.activeDiagrams.Clear();
		Vector2 sizeDelta = this.diagramsParent.GetComponent<RectTransform>().sizeDelta;
		sizeDelta.y = 0f;
		this.diagramsParent.GetComponent<RectTransform>().sizeDelta = sizeDelta;
	}

	// Token: 0x0600A60D RID: 42509 RVA: 0x003FCBC8 File Offset: 0x003FADC8
	public OverlayLegend.OverlayInfo GetOverlayInfo(OverlayModes.Mode mode)
	{
		for (int i = 0; i < this.overlayInfoList.Count; i++)
		{
			if (this.overlayInfoList[i].mode == mode.ViewMode())
			{
				return this.overlayInfoList[i];
			}
		}
		return null;
	}

	// Token: 0x0600A60E RID: 42510 RVA: 0x003FCC18 File Offset: 0x003FAE18
	private void PopulateOverlayInfoUnits(OverlayLegend.OverlayInfo overlayInfo, bool isRefresh = false)
	{
		if (overlayInfo.infoUnits != null && overlayInfo.infoUnits.Count > 0)
		{
			this.activeUnitsParent.SetActive(true);
			using (List<OverlayLegend.OverlayInfoUnit>.Enumerator enumerator = overlayInfo.infoUnits.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					OverlayLegend.OverlayInfoUnit overlayInfoUnit = enumerator.Current;
					GameObject freeUnitObject = this.GetFreeUnitObject();
					if (overlayInfoUnit.icon != null)
					{
						Image component = freeUnitObject.transform.Find("Icon").GetComponent<Image>();
						component.gameObject.SetActive(true);
						component.sprite = overlayInfoUnit.icon;
						component.color = overlayInfoUnit.color;
						component.enabled = true;
						component.type = (overlayInfoUnit.sliceIcon ? Image.Type.Sliced : Image.Type.Simple);
					}
					else
					{
						freeUnitObject.transform.Find("Icon").gameObject.SetActive(false);
					}
					if (!string.IsNullOrEmpty(overlayInfoUnit.description))
					{
						LocText componentInChildren = freeUnitObject.GetComponentInChildren<LocText>();
						componentInChildren.text = string.Format(overlayInfoUnit.description, overlayInfoUnit.formatData);
						componentInChildren.color = overlayInfoUnit.fontColor;
						componentInChildren.enabled = true;
					}
					ToolTip component2 = freeUnitObject.GetComponent<ToolTip>();
					if (!string.IsNullOrEmpty(overlayInfoUnit.tooltip))
					{
						component2.toolTip = string.Format(overlayInfoUnit.tooltip, overlayInfoUnit.tooltipFormatData);
						component2.enabled = true;
					}
					else
					{
						component2.enabled = false;
					}
					freeUnitObject.SetActive(true);
					freeUnitObject.transform.SetParent(this.activeUnitsParent.transform);
				}
				return;
			}
		}
		this.activeUnitsParent.SetActive(false);
	}

	// Token: 0x0600A60F RID: 42511 RVA: 0x003FCDC4 File Offset: 0x003FAFC4
	private void PopulateOverlayDiagrams(OverlayLegend.OverlayInfo overlayInfo, bool isRefresh = false)
	{
		if (!isRefresh)
		{
			if (overlayInfo.mode == OverlayModes.Temperature.ID)
			{
				Game.TemperatureOverlayModes temperatureOverlayMode = Game.Instance.temperatureOverlayMode;
				if (temperatureOverlayMode != Game.TemperatureOverlayModes.AbsoluteTemperature)
				{
					if (temperatureOverlayMode == Game.TemperatureOverlayModes.RelativeTemperature)
					{
						this.ClearDiagrams();
						overlayInfo = this.overlayInfoList.Find((OverlayLegend.OverlayInfo match) => match.name == UI.OVERLAYS.RELATIVETEMPERATURE.NAME);
					}
				}
				else
				{
					SimDebugView.Instance.user_temperatureThresholds[0] = 0f;
					SimDebugView.Instance.user_temperatureThresholds[1] = 2073f;
				}
			}
			if (overlayInfo.diagrams != null && overlayInfo.diagrams.Count > 0)
			{
				this.diagramsParent.SetActive(true);
				using (List<GameObject>.Enumerator enumerator = overlayInfo.diagrams.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						GameObject original = enumerator.Current;
						GameObject item = Util.KInstantiateUI(original, this.diagramsParent, false);
						this.activeDiagrams.Add(item);
					}
					return;
				}
			}
			this.diagramsParent.SetActive(false);
		}
	}

	// Token: 0x0600A610 RID: 42512 RVA: 0x003FCEE0 File Offset: 0x003FB0E0
	private void PopulateGeneratedLegend(OverlayLegend.OverlayInfo info, bool isRefresh = false)
	{
		if (isRefresh)
		{
			this.RemoveActiveObjects();
			this.ClearDiagrams();
		}
		if (info.infoUnits != null && info.infoUnits.Count > 0)
		{
			this.PopulateOverlayInfoUnits(info, isRefresh);
		}
		this.PopulateOverlayDiagrams(info, false);
		List<LegendEntry> customLegendData = this.currentMode.GetCustomLegendData();
		if (customLegendData != null)
		{
			this.activeUnitsParent.SetActive(true);
			using (List<LegendEntry>.Enumerator enumerator = customLegendData.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					LegendEntry legendEntry = enumerator.Current;
					GameObject freeUnitObject = this.GetFreeUnitObject();
					Image component = freeUnitObject.transform.Find("Icon").GetComponent<Image>();
					component.gameObject.SetActive(legendEntry.displaySprite);
					component.sprite = legendEntry.sprite;
					component.color = legendEntry.colour;
					component.enabled = true;
					component.type = Image.Type.Simple;
					LocText componentInChildren = freeUnitObject.GetComponentInChildren<LocText>();
					componentInChildren.text = legendEntry.name;
					componentInChildren.color = Color.white;
					componentInChildren.enabled = true;
					ToolTip component2 = freeUnitObject.GetComponent<ToolTip>();
					component2.enabled = (legendEntry.desc != null || legendEntry.desc_arg != null);
					component2.toolTip = ((legendEntry.desc_arg == null) ? legendEntry.desc : string.Format(legendEntry.desc, legendEntry.desc_arg));
					freeUnitObject.SetActive(true);
					freeUnitObject.transform.SetParent(this.activeUnitsParent.transform);
				}
				goto IL_165;
			}
		}
		this.activeUnitsParent.SetActive(false);
		IL_165:
		if (!isRefresh && this.currentMode.legendFilters != null)
		{
			GameObject gameObject = Util.KInstantiateUI(this.toolParameterMenuPrefab, this.diagramsParent.transform.parent.gameObject, false);
			gameObject.transform.SetAsFirstSibling();
			this.filterMenu = gameObject.GetComponent<ToolParameterMenu>();
			this.filterMenu.PopulateMenu(this.currentMode.legendFilters);
			this.filterMenu.onParametersChanged += this.OnFiltersChanged;
			this.OnFiltersChanged();
		}
		this.ConfigureUIHeight();
	}

	// Token: 0x0600A611 RID: 42513 RVA: 0x0011029E File Offset: 0x0010E49E
	private void OnFiltersChanged()
	{
		this.currentMode.OnFiltersChanged();
		this.PopulateGeneratedLegend(this.GetOverlayInfo(this.currentMode), true);
		Game.Instance.ForceOverlayUpdate(false);
	}

	// Token: 0x0600A612 RID: 42514 RVA: 0x001102C9 File Offset: 0x0010E4C9
	private void DisableOverlay()
	{
		this.filterMenu.onParametersChanged -= this.OnFiltersChanged;
		this.filterMenu.ClearMenu();
		this.filterMenu.gameObject.SetActive(false);
		this.filterMenu = null;
	}

	// Token: 0x0600A613 RID: 42515 RVA: 0x003FD0E4 File Offset: 0x003FB2E4
	private void ConfigureUIHeight()
	{
		this.scrollRectLayout.enabled = false;
		this.scrollRectLayout.GetComponent<VerticalLayoutGroup>().enabled = true;
		LayoutRebuilder.ForceRebuildLayoutImmediate(base.gameObject.rectTransform());
		this.scrollRectLayout.preferredWidth = this.scrollRectLayout.rectTransform().sizeDelta.x;
		float y = this.scrollRectLayout.rectTransform().sizeDelta.y;
		this.scrollRectLayout.preferredHeight = Mathf.Min(y, 512f);
		this.scrollRectLayout.GetComponent<VerticalLayoutGroup>().enabled = false;
		this.scrollRectLayout.enabled = true;
		LayoutRebuilder.ForceRebuildLayoutImmediate(base.gameObject.rectTransform());
	}

	// Token: 0x040081F6 RID: 33270
	public static OverlayLegend Instance;

	// Token: 0x040081F7 RID: 33271
	[SerializeField]
	private LocText title;

	// Token: 0x040081F8 RID: 33272
	[SerializeField]
	private Sprite emptySprite;

	// Token: 0x040081F9 RID: 33273
	[SerializeField]
	private List<OverlayLegend.OverlayInfo> overlayInfoList;

	// Token: 0x040081FA RID: 33274
	[SerializeField]
	private GameObject unitPrefab;

	// Token: 0x040081FB RID: 33275
	[SerializeField]
	private GameObject activeUnitsParent;

	// Token: 0x040081FC RID: 33276
	[SerializeField]
	private GameObject diagramsParent;

	// Token: 0x040081FD RID: 33277
	[SerializeField]
	private GameObject inactiveUnitsParent;

	// Token: 0x040081FE RID: 33278
	[SerializeField]
	private GameObject toolParameterMenuPrefab;

	// Token: 0x040081FF RID: 33279
	[SerializeField]
	private LayoutElement scrollRectLayout;

	// Token: 0x04008200 RID: 33280
	private ToolParameterMenu filterMenu;

	// Token: 0x04008201 RID: 33281
	private OverlayModes.Mode currentMode;

	// Token: 0x04008202 RID: 33282
	private List<GameObject> inactiveUnitObjs;

	// Token: 0x04008203 RID: 33283
	private List<GameObject> activeUnitObjs;

	// Token: 0x04008204 RID: 33284
	private List<GameObject> activeDiagrams = new List<GameObject>();

	// Token: 0x02001EE8 RID: 7912
	[Serializable]
	public class OverlayInfoUnit
	{
		// Token: 0x0600A615 RID: 42517 RVA: 0x00110318 File Offset: 0x0010E518
		public OverlayInfoUnit(Sprite icon, string description, Color color, Color fontColor, object formatData = null, bool sliceIcon = false)
		{
			this.icon = icon;
			this.description = description;
			this.color = color;
			this.fontColor = fontColor;
			this.formatData = formatData;
			this.sliceIcon = sliceIcon;
		}

		// Token: 0x04008205 RID: 33285
		public Sprite icon;

		// Token: 0x04008206 RID: 33286
		public string description;

		// Token: 0x04008207 RID: 33287
		public string tooltip;

		// Token: 0x04008208 RID: 33288
		public Color color;

		// Token: 0x04008209 RID: 33289
		public Color fontColor;

		// Token: 0x0400820A RID: 33290
		public object formatData;

		// Token: 0x0400820B RID: 33291
		public object tooltipFormatData;

		// Token: 0x0400820C RID: 33292
		public bool sliceIcon;
	}

	// Token: 0x02001EE9 RID: 7913
	[Serializable]
	public class OverlayInfo
	{
		// Token: 0x0400820D RID: 33293
		public string name;

		// Token: 0x0400820E RID: 33294
		public HashedString mode;

		// Token: 0x0400820F RID: 33295
		public List<OverlayLegend.OverlayInfoUnit> infoUnits;

		// Token: 0x04008210 RID: 33296
		public List<GameObject> diagrams;

		// Token: 0x04008211 RID: 33297
		public bool isProgrammaticallyPopulated;
	}
}
