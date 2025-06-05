using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001D0A RID: 7434
public class DetailsScreenMaterialPanel : TargetScreen
{
	// Token: 0x06009B46 RID: 39750 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public override bool IsValidForTarget(GameObject target)
	{
		return true;
	}

	// Token: 0x06009B47 RID: 39751 RVA: 0x001098C3 File Offset: 0x00107AC3
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.openChangeMaterialPanelButton.onClick += delegate()
		{
			this.OpenMaterialSelectionPanel();
			this.RefreshMaterialSelectionPanel();
			this.RefreshOrderChangeMaterialButton();
		};
	}

	// Token: 0x06009B48 RID: 39752 RVA: 0x003CBBA8 File Offset: 0x003C9DA8
	public override void SetTarget(GameObject target)
	{
		if (this.selectedTarget != null)
		{
			this.selectedTarget.Unsubscribe(this.subHandle);
		}
		this.building = null;
		base.SetTarget(target);
		if (target == null)
		{
			return;
		}
		this.materialSelectionPanel.gameObject.SetActive(false);
		this.orderChangeMaterialButton.ClearOnClick();
		this.orderChangeMaterialButton.isInteractable = false;
		this.CloseMaterialSelectionPanel();
		this.building = this.selectedTarget.GetComponent<Building>();
		bool flag = Db.Get().TechItems.IsTechItemComplete(this.building.Def.PrefabID) || DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive;
		this.openChangeMaterialPanelButton.isInteractable = (target.GetComponent<Reconstructable>() != null && target.GetComponent<Reconstructable>().AllowReconstruct && flag);
		this.openChangeMaterialPanelButton.GetComponent<ToolTip>().SetSimpleTooltip(flag ? "" : string.Format(UI.PRODUCTINFO_REQUIRESRESEARCHDESC, Db.Get().TechItems.GetTechFromItemID(this.building.Def.PrefabID).Name));
		this.Refresh(null);
		this.subHandle = target.Subscribe(954267658, new Action<object>(this.RefreshOrderChangeMaterialButton));
		Game.Instance.Subscribe(1980521255, new Action<object>(this.Refresh));
	}

	// Token: 0x06009B49 RID: 39753 RVA: 0x003CBD18 File Offset: 0x003C9F18
	private void OpenMaterialSelectionPanel()
	{
		this.openChangeMaterialPanelButton.gameObject.SetActive(false);
		this.materialSelectionPanel.gameObject.SetActive(true);
		this.RefreshMaterialSelectionPanel();
		if (this.selectedTarget != null && this.building != null)
		{
			this.materialSelectionPanel.SelectSourcesMaterials(this.building);
		}
	}

	// Token: 0x06009B4A RID: 39754 RVA: 0x001098E2 File Offset: 0x00107AE2
	private void CloseMaterialSelectionPanel()
	{
		this.currentMaterialDescriptionRow.gameObject.SetActive(true);
		this.openChangeMaterialPanelButton.gameObject.SetActive(true);
		this.materialSelectionPanel.gameObject.SetActive(false);
	}

	// Token: 0x06009B4B RID: 39755 RVA: 0x00109917 File Offset: 0x00107B17
	public override void OnDeselectTarget(GameObject target)
	{
		base.OnDeselectTarget(target);
		this.Refresh(null);
	}

	// Token: 0x06009B4C RID: 39756 RVA: 0x00109927 File Offset: 0x00107B27
	private void Refresh(object data = null)
	{
		this.RefreshCurrentMaterial();
		this.RefreshMaterialSelectionPanel();
	}

	// Token: 0x06009B4D RID: 39757 RVA: 0x003CBD7C File Offset: 0x003C9F7C
	private void RefreshCurrentMaterial()
	{
		if (this.selectedTarget == null)
		{
			return;
		}
		CellSelectionObject component = this.selectedTarget.GetComponent<CellSelectionObject>();
		Element element = (component == null) ? this.selectedTarget.GetComponent<PrimaryElement>().Element : component.element;
		global::Tuple<Sprite, Color> uisprite = Def.GetUISprite(element, "ui", false);
		this.currentMaterialIcon.sprite = uisprite.first;
		this.currentMaterialIcon.color = uisprite.second;
		if (component == null)
		{
			this.currentMaterialLabel.SetText(element.name + " x " + GameUtil.GetFormattedMass(this.selectedTarget.GetComponent<PrimaryElement>().Mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
		}
		else
		{
			this.currentMaterialLabel.SetText(element.name);
		}
		this.currentMaterialDescription.SetText(element.description);
		List<Descriptor> materialDescriptors = GameUtil.GetMaterialDescriptors(element);
		if (materialDescriptors.Count > 0)
		{
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(ELEMENTS.MATERIAL_MODIFIERS.EFFECTS_HEADER, ELEMENTS.MATERIAL_MODIFIERS.TOOLTIP.EFFECTS_HEADER, Descriptor.DescriptorType.Effect);
			materialDescriptors.Insert(0, item);
			this.descriptorPanel.gameObject.SetActive(true);
			this.descriptorPanel.SetDescriptors(materialDescriptors);
			return;
		}
		this.descriptorPanel.gameObject.SetActive(false);
	}

	// Token: 0x06009B4E RID: 39758 RVA: 0x003CBEC8 File Offset: 0x003CA0C8
	private void RefreshMaterialSelectionPanel()
	{
		if (this.selectedTarget == null)
		{
			return;
		}
		this.materialSelectionPanel.ClearSelectActions();
		if (!(this.building == null) && !(this.building is BuildingUnderConstruction))
		{
			this.materialSelectionPanel.ConfigureScreen(this.building.Def.CraftRecipe, (BuildingDef data) => true, (BuildingDef data) => "");
			this.materialSelectionPanel.AddSelectAction(new MaterialSelector.SelectMaterialActions(this.RefreshOrderChangeMaterialButton));
			Reconstructable component = this.selectedTarget.GetComponent<Reconstructable>();
			if (component != null && component.ReconstructRequested)
			{
				if (!this.materialSelectionPanel.gameObject.activeSelf)
				{
					this.OpenMaterialSelectionPanel();
					this.materialSelectionPanel.RefreshSelectors();
				}
				this.materialSelectionPanel.ForceSelectPrimaryTag(component.PrimarySelectedElementTag);
			}
		}
		this.confirmChangeRow.transform.SetAsLastSibling();
	}

	// Token: 0x06009B4F RID: 39759 RVA: 0x00109935 File Offset: 0x00107B35
	private void RefreshOrderChangeMaterialButton()
	{
		this.RefreshOrderChangeMaterialButton(null);
	}

	// Token: 0x06009B50 RID: 39760 RVA: 0x003CBFE4 File Offset: 0x003CA1E4
	private void RefreshOrderChangeMaterialButton(object data = null)
	{
		if (this.selectedTarget == null)
		{
			return;
		}
		Reconstructable reconstructable = this.selectedTarget.GetComponent<Reconstructable>();
		bool flag = this.materialSelectionPanel.CurrentSelectedElement != null;
		this.orderChangeMaterialButton.isInteractable = (flag && this.building.GetComponent<PrimaryElement>().Element.tag != this.materialSelectionPanel.CurrentSelectedElement);
		this.orderChangeMaterialButton.ClearOnClick();
		this.orderChangeMaterialButton.onClick += delegate()
		{
			reconstructable.RequestReconstruct(this.materialSelectionPanel.CurrentSelectedElement);
			this.RefreshOrderChangeMaterialButton();
		};
		this.orderChangeMaterialButton.GetComponentInChildren<LocText>().SetText(reconstructable.ReconstructRequested ? UI.USERMENUACTIONS.RECONSTRUCT.CANCEL_RECONSTRUCT : UI.USERMENUACTIONS.RECONSTRUCT.REQUEST_RECONSTRUCT);
		this.orderChangeMaterialButton.GetComponent<ToolTip>().SetSimpleTooltip(reconstructable.ReconstructRequested ? UI.USERMENUACTIONS.RECONSTRUCT.CANCEL_RECONSTRUCT_TOOLTIP : UI.USERMENUACTIONS.RECONSTRUCT.REQUEST_RECONSTRUCT_TOOLTIP);
	}

	// Token: 0x0400795C RID: 31068
	[Header("Current Material")]
	[SerializeField]
	private Image currentMaterialIcon;

	// Token: 0x0400795D RID: 31069
	[SerializeField]
	private RectTransform currentMaterialDescriptionRow;

	// Token: 0x0400795E RID: 31070
	[SerializeField]
	private LocText currentMaterialLabel;

	// Token: 0x0400795F RID: 31071
	[SerializeField]
	private LocText currentMaterialDescription;

	// Token: 0x04007960 RID: 31072
	[SerializeField]
	private DescriptorPanel descriptorPanel;

	// Token: 0x04007961 RID: 31073
	[Header("Change Material")]
	[SerializeField]
	private MaterialSelectionPanel materialSelectionPanel;

	// Token: 0x04007962 RID: 31074
	[SerializeField]
	private RectTransform confirmChangeRow;

	// Token: 0x04007963 RID: 31075
	[SerializeField]
	private KButton orderChangeMaterialButton;

	// Token: 0x04007964 RID: 31076
	[SerializeField]
	private KButton openChangeMaterialPanelButton;

	// Token: 0x04007965 RID: 31077
	private int subHandle = -1;

	// Token: 0x04007966 RID: 31078
	private Building building;
}
