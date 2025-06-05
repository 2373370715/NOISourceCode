using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001EFB RID: 7931
public class PlanBuildingToggle : KToggle
{
	// Token: 0x0600A683 RID: 42627 RVA: 0x003FEF5C File Offset: 0x003FD15C
	public void Config(BuildingDef def, PlanScreen planScreen, HashedString buildingCategory, bool? passesSearchFilter)
	{
		this.def = def;
		this.planScreen = planScreen;
		this.buildingCategory = buildingCategory;
		this.techItem = Db.Get().TechItems.TryGet(def.PrefabID);
		this.gameSubscriptions.Add(Game.Instance.Subscribe(-107300940, new Action<object>(this.CheckResearch)));
		this.gameSubscriptions.Add(Game.Instance.Subscribe(-1948169901, new Action<object>(this.CheckResearch)));
		this.gameSubscriptions.Add(Game.Instance.Subscribe(1557339983, new Action<object>(this.CheckResearch)));
		this.sprite = def.GetUISprite("ui", false);
		base.onClick += delegate()
		{
			PlanScreen.Instance.OnSelectBuilding(this.gameObject, def, null);
			this.RefreshDisplay();
		};
		if (BUILDINGS.PLANSUBCATEGORYSORTING.ContainsKey(def.PrefabID))
		{
			Strings.TryGet("STRINGS.UI.NEWBUILDCATEGORIES." + BUILDINGS.PLANSUBCATEGORYSORTING[def.PrefabID].ToUpper() + ".NAME", out this.subcategoryName);
		}
		else
		{
			global::Debug.LogWarning("Building " + def.PrefabID + " has not been added to plan screen subcategory organization in BuildingTuning.cs");
		}
		this.CheckResearch(null);
		this.Refresh(passesSearchFilter);
	}

	// Token: 0x0600A684 RID: 42628 RVA: 0x003FF0D0 File Offset: 0x003FD2D0
	protected override void OnDestroy()
	{
		if (Game.Instance != null)
		{
			foreach (int id in this.gameSubscriptions)
			{
				Game.Instance.Unsubscribe(id);
			}
		}
		this.gameSubscriptions.Clear();
		base.OnDestroy();
	}

	// Token: 0x0600A685 RID: 42629 RVA: 0x0011085C File Offset: 0x0010EA5C
	private void CheckResearch(object data = null)
	{
		this.researchComplete = PlanScreen.TechRequirementsMet(this.techItem);
	}

	// Token: 0x0600A686 RID: 42630 RVA: 0x003FF148 File Offset: 0x003FD348
	private bool StandardDisplayFilter()
	{
		return (this.researchComplete || DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive) && (this.planScreen.ActiveCategoryToggleInfo == null || this.buildingCategory == (HashedString)this.planScreen.ActiveCategoryToggleInfo.userData);
	}

	// Token: 0x0600A687 RID: 42631 RVA: 0x003FF1A4 File Offset: 0x003FD3A4
	public bool Refresh(bool? passesSearchFilter)
	{
		bool flag = passesSearchFilter ?? this.StandardDisplayFilter();
		bool flag2 = base.gameObject.activeSelf != flag;
		if (flag2)
		{
			base.gameObject.SetActive(flag);
		}
		if (base.gameObject.activeSelf)
		{
			this.PositionTooltip();
			this.RefreshLabel();
			this.RefreshDisplay();
		}
		return flag2;
	}

	// Token: 0x0600A688 RID: 42632 RVA: 0x003FF20C File Offset: 0x003FD40C
	public void SwitchViewMode(bool listView)
	{
		this.text.gameObject.SetActive(!listView);
		this.text_listView.gameObject.SetActive(listView);
		this.buildingIcon.gameObject.SetActive(!listView);
		this.buildingIcon_listView.gameObject.SetActive(listView);
	}

	// Token: 0x0600A689 RID: 42633 RVA: 0x003FF264 File Offset: 0x003FD464
	private void RefreshLabel()
	{
		if (this.text != null)
		{
			this.text.fontSize = (float)(ScreenResolutionMonitor.UsingGamepadUIMode() ? PlanScreen.fontSizeBigMode : PlanScreen.fontSizeStandardMode);
			this.text_listView.fontSize = (float)(ScreenResolutionMonitor.UsingGamepadUIMode() ? PlanScreen.fontSizeBigMode : PlanScreen.fontSizeStandardMode);
			this.text.text = this.def.Name;
			this.text_listView.text = this.def.Name;
		}
	}

	// Token: 0x0600A68A RID: 42634 RVA: 0x003FF2EC File Offset: 0x003FD4EC
	private void RefreshDisplay()
	{
		PlanScreen.RequirementsState buildableState = PlanScreen.Instance.GetBuildableState(this.def);
		bool flag = buildableState == PlanScreen.RequirementsState.Complete || DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive;
		bool flag2 = base.gameObject == PlanScreen.Instance.SelectedBuildingGameObject;
		if (flag2 && flag)
		{
			this.toggle.ChangeState(1);
		}
		else if (!flag2 && flag)
		{
			this.toggle.ChangeState(0);
		}
		else if (flag2 && !flag)
		{
			this.toggle.ChangeState(3);
		}
		else if (!flag2 && !flag)
		{
			this.toggle.ChangeState(2);
		}
		this.RefreshBuildingButtonIconAndColors(flag);
		this.RefreshFG(buildableState);
	}

	// Token: 0x0600A68B RID: 42635 RVA: 0x003FF39C File Offset: 0x003FD59C
	private void PositionTooltip()
	{
		this.tooltip.overrideParentObject = (PlanScreen.Instance.ProductInfoScreen.gameObject.activeSelf ? PlanScreen.Instance.ProductInfoScreen.rectTransform() : PlanScreen.Instance.buildingGroupsRoot);
		this.tooltip.tooltipPivot = Vector2.zero;
		this.tooltip.parentPositionAnchor = new Vector2(1f, 0f);
		this.tooltip.tooltipPositionOffset = new Vector2(4f, 0f);
		this.tooltip.ClearMultiStringTooltip();
		string name = this.def.Name;
		string effect = this.def.Effect;
		this.tooltip.AddMultiStringTooltip(name, PlanScreen.Instance.buildingToolTipSettings.BuildButtonName);
		this.tooltip.AddMultiStringTooltip(effect, PlanScreen.Instance.buildingToolTipSettings.BuildButtonDescription);
	}

	// Token: 0x0600A68C RID: 42636 RVA: 0x003FF484 File Offset: 0x003FD684
	private void RefreshBuildingButtonIconAndColors(bool buttonAvailable)
	{
		if (this.sprite == null)
		{
			this.sprite = PlanScreen.Instance.defaultBuildingIconSprite;
		}
		this.buildingIcon.sprite = this.sprite;
		this.buildingIcon.SetNativeSize();
		this.buildingIcon_listView.sprite = this.sprite;
		float d = ScreenResolutionMonitor.UsingGamepadUIMode() ? 3.25f : 4f;
		this.buildingIcon.rectTransform().sizeDelta /= d;
		Material material = buttonAvailable ? PlanScreen.Instance.defaultUIMaterial : PlanScreen.Instance.desaturatedUIMaterial;
		if (this.buildingIcon.material != material)
		{
			this.buildingIcon.material = material;
			this.buildingIcon_listView.material = material;
		}
	}

	// Token: 0x0600A68D RID: 42637 RVA: 0x003FF554 File Offset: 0x003FD754
	private void RefreshFG(PlanScreen.RequirementsState requirementsState)
	{
		if (requirementsState == PlanScreen.RequirementsState.Tech)
		{
			this.fgImage.sprite = PlanScreen.Instance.Overlay_NeedTech;
			this.fgImage.gameObject.SetActive(true);
		}
		else
		{
			this.fgImage.gameObject.SetActive(false);
		}
		string tooltipForRequirementsState = PlanScreen.GetTooltipForRequirementsState(this.def, requirementsState);
		if (tooltipForRequirementsState != null)
		{
			this.tooltip.AddMultiStringTooltip("\n", PlanScreen.Instance.buildingToolTipSettings.ResearchRequirement);
			this.tooltip.AddMultiStringTooltip(tooltipForRequirementsState, PlanScreen.Instance.buildingToolTipSettings.ResearchRequirement);
		}
	}

	// Token: 0x0400825D RID: 33373
	private BuildingDef def;

	// Token: 0x0400825E RID: 33374
	private HashedString buildingCategory;

	// Token: 0x0400825F RID: 33375
	private TechItem techItem;

	// Token: 0x04008260 RID: 33376
	private List<int> gameSubscriptions = new List<int>();

	// Token: 0x04008261 RID: 33377
	private bool researchComplete;

	// Token: 0x04008262 RID: 33378
	private Sprite sprite;

	// Token: 0x04008263 RID: 33379
	[SerializeField]
	private MultiToggle toggle;

	// Token: 0x04008264 RID: 33380
	[SerializeField]
	private ToolTip tooltip;

	// Token: 0x04008265 RID: 33381
	[SerializeField]
	private LocText text;

	// Token: 0x04008266 RID: 33382
	[SerializeField]
	private LocText text_listView;

	// Token: 0x04008267 RID: 33383
	[SerializeField]
	private Image buildingIcon;

	// Token: 0x04008268 RID: 33384
	[SerializeField]
	private Image buildingIcon_listView;

	// Token: 0x04008269 RID: 33385
	[SerializeField]
	private Image fgIcon;

	// Token: 0x0400826A RID: 33386
	[SerializeField]
	private PlanScreen planScreen;

	// Token: 0x0400826B RID: 33387
	private StringEntry subcategoryName;
}
