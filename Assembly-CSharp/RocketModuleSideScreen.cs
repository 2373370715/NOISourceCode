using System;
using System.Collections;
using FMOD.Studio;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02002021 RID: 8225
public class RocketModuleSideScreen : SideScreenContent
{
	// Token: 0x0600AE1D RID: 44573 RVA: 0x001159FC File Offset: 0x00113BFC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		RocketModuleSideScreen.instance = this;
	}

	// Token: 0x0600AE1E RID: 44574 RVA: 0x00115A0A File Offset: 0x00113C0A
	protected override void OnForcedCleanUp()
	{
		RocketModuleSideScreen.instance = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x0600AE1F RID: 44575 RVA: 0x000E4472 File Offset: 0x000E2672
	public override int GetSideScreenSortOrder()
	{
		return 500;
	}

	// Token: 0x0600AE20 RID: 44576 RVA: 0x004256FC File Offset: 0x004238FC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.addNewModuleButton.onClick += delegate()
		{
			Vector2 vector = Vector2.zero;
			if (SelectModuleSideScreen.Instance != null)
			{
				vector = SelectModuleSideScreen.Instance.mainContents.GetComponent<KScrollRect>().content.rectTransform().anchoredPosition;
			}
			this.ClickAddNew(vector.y, null);
		};
		this.removeModuleButton.onClick += this.ClickRemove;
		this.moveModuleUpButton.onClick += this.ClickSwapUp;
		this.moveModuleDownButton.onClick += this.ClickSwapDown;
		this.changeModuleButton.onClick += delegate()
		{
			Vector2 vector = Vector2.zero;
			if (SelectModuleSideScreen.Instance != null)
			{
				vector = SelectModuleSideScreen.Instance.mainContents.GetComponent<KScrollRect>().content.rectTransform().anchoredPosition;
			}
			this.ClickChangeModule(vector.y);
		};
		this.viewInteriorButton.onClick += this.ClickViewInterior;
		this.moduleNameLabel.textStyleSetting = this.nameSetting;
		this.moduleDescriptionLabel.textStyleSetting = this.descriptionSetting;
		this.moduleNameLabel.ApplySettings();
		this.moduleDescriptionLabel.ApplySettings();
	}

	// Token: 0x0600AE21 RID: 44577 RVA: 0x00115A18 File Offset: 0x00113C18
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		DetailsScreen.Instance.ClearSecondarySideScreen();
	}

	// Token: 0x0600AE22 RID: 44578 RVA: 0x00115A2A File Offset: 0x00113C2A
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x0600AE23 RID: 44579 RVA: 0x00115A32 File Offset: 0x00113C32
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<ReorderableBuilding>() != null;
	}

	// Token: 0x0600AE24 RID: 44580 RVA: 0x004257D4 File Offset: 0x004239D4
	public override void SetTarget(GameObject new_target)
	{
		if (new_target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.reorderable = new_target.GetComponent<ReorderableBuilding>();
		this.moduleIcon.sprite = Def.GetUISprite(this.reorderable.gameObject, "ui", false).first;
		this.moduleNameLabel.SetText(this.reorderable.GetProperName());
		this.moduleDescriptionLabel.SetText(this.reorderable.GetComponent<Building>().Desc);
		this.UpdateButtonStates();
	}

	// Token: 0x0600AE25 RID: 44581 RVA: 0x00425860 File Offset: 0x00423A60
	public void UpdateButtonStates()
	{
		this.changeModuleButton.isInteractable = this.reorderable.CanChangeModule();
		this.changeModuleButton.GetComponent<ToolTip>().SetSimpleTooltip(this.changeModuleButton.isInteractable ? UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONCHANGEMODULE.DESC.text : UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONCHANGEMODULE.INVALID.text);
		this.addNewModuleButton.isInteractable = true;
		this.addNewModuleButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.ADDMODULE.DESC.text);
		this.removeModuleButton.isInteractable = this.reorderable.CanRemoveModule();
		this.removeModuleButton.GetComponent<ToolTip>().SetSimpleTooltip(this.removeModuleButton.isInteractable ? UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONREMOVEMODULE.DESC.text : UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONREMOVEMODULE.INVALID.text);
		this.moveModuleDownButton.isInteractable = this.reorderable.CanSwapDown(true);
		this.moveModuleDownButton.GetComponent<ToolTip>().SetSimpleTooltip(this.moveModuleDownButton.isInteractable ? UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONSWAPMODULEDOWN.DESC.text : UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONSWAPMODULEDOWN.INVALID.text);
		this.moveModuleUpButton.isInteractable = this.reorderable.CanSwapUp(true);
		this.moveModuleUpButton.GetComponent<ToolTip>().SetSimpleTooltip(this.moveModuleUpButton.isInteractable ? UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONSWAPMODULEUP.DESC.text : UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONSWAPMODULEUP.INVALID.text);
		ClustercraftExteriorDoor component = this.reorderable.GetComponent<ClustercraftExteriorDoor>();
		if (!(component != null) || !component.HasTargetWorld())
		{
			this.viewInteriorButton.isInteractable = false;
			this.viewInteriorButton.GetComponentInChildren<LocText>().SetText(UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONVIEWINTERIOR.LABEL);
			this.viewInteriorButton.GetComponent<ToolTip>().SetSimpleTooltip(this.viewInteriorButton.isInteractable ? UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONVIEWINTERIOR.DESC.text : UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONVIEWINTERIOR.INVALID.text);
			return;
		}
		if (ClusterManager.Instance.activeWorld == component.GetTargetWorld())
		{
			this.changeModuleButton.isInteractable = false;
			this.addNewModuleButton.isInteractable = false;
			this.removeModuleButton.isInteractable = false;
			this.moveModuleDownButton.isInteractable = false;
			this.moveModuleUpButton.isInteractable = false;
			this.viewInteriorButton.isInteractable = (component.GetMyWorldId() != 255);
			this.viewInteriorButton.GetComponentInChildren<LocText>().SetText(UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONVIEWEXTERIOR.LABEL);
			this.viewInteriorButton.GetComponent<ToolTip>().SetSimpleTooltip(this.viewInteriorButton.isInteractable ? UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONVIEWEXTERIOR.DESC.text : UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONVIEWEXTERIOR.INVALID.text);
			return;
		}
		this.viewInteriorButton.isInteractable = (this.reorderable.GetComponent<PassengerRocketModule>() != null);
		this.viewInteriorButton.GetComponentInChildren<LocText>().SetText(UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONVIEWINTERIOR.LABEL);
		this.viewInteriorButton.GetComponent<ToolTip>().SetSimpleTooltip(this.viewInteriorButton.isInteractable ? UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONVIEWINTERIOR.DESC.text : UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONVIEWINTERIOR.INVALID.text);
	}

	// Token: 0x0600AE26 RID: 44582 RVA: 0x00425B60 File Offset: 0x00423D60
	public void ClickAddNew(float scrollViewPosition, BuildingDef autoSelectDef = null)
	{
		SelectModuleSideScreen selectModuleSideScreen = (SelectModuleSideScreen)DetailsScreen.Instance.SetSecondarySideScreen(this.changeModuleSideScreen, UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.CHANGEMODULEPANEL);
		selectModuleSideScreen.addingNewModule = true;
		selectModuleSideScreen.SetTarget(this.reorderable.gameObject);
		if (autoSelectDef != null)
		{
			selectModuleSideScreen.SelectModule(autoSelectDef);
		}
		this.ScrollToTargetPoint(scrollViewPosition);
	}

	// Token: 0x0600AE27 RID: 44583 RVA: 0x00425BBC File Offset: 0x00423DBC
	private void ScrollToTargetPoint(float scrollViewPosition)
	{
		if (SelectModuleSideScreen.Instance != null)
		{
			SelectModuleSideScreen.Instance.mainContents.GetComponent<KScrollRect>().content.anchoredPosition = new Vector2(0f, scrollViewPosition);
			if (base.gameObject.activeInHierarchy)
			{
				base.StartCoroutine(this.DelayedScrollToTargetPoint(scrollViewPosition));
			}
		}
	}

	// Token: 0x0600AE28 RID: 44584 RVA: 0x00115A40 File Offset: 0x00113C40
	private IEnumerator DelayedScrollToTargetPoint(float scrollViewPosition)
	{
		if (SelectModuleSideScreen.Instance != null)
		{
			yield return SequenceUtil.WaitForEndOfFrame;
			SelectModuleSideScreen.Instance.mainContents.GetComponent<KScrollRect>().content.anchoredPosition = new Vector2(0f, scrollViewPosition);
		}
		yield break;
	}

	// Token: 0x0600AE29 RID: 44585 RVA: 0x00115A4F File Offset: 0x00113C4F
	private void ClickRemove()
	{
		this.reorderable.Trigger(-790448070, null);
		this.UpdateButtonStates();
	}

	// Token: 0x0600AE2A RID: 44586 RVA: 0x00115A68 File Offset: 0x00113C68
	private void ClickSwapUp()
	{
		this.reorderable.SwapWithAbove(true);
		this.UpdateButtonStates();
	}

	// Token: 0x0600AE2B RID: 44587 RVA: 0x00115A7C File Offset: 0x00113C7C
	private void ClickSwapDown()
	{
		this.reorderable.SwapWithBelow(true);
		this.UpdateButtonStates();
	}

	// Token: 0x0600AE2C RID: 44588 RVA: 0x00115A90 File Offset: 0x00113C90
	private void ClickChangeModule(float scrollViewPosition)
	{
		SelectModuleSideScreen selectModuleSideScreen = (SelectModuleSideScreen)DetailsScreen.Instance.SetSecondarySideScreen(this.changeModuleSideScreen, UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.CHANGEMODULEPANEL);
		selectModuleSideScreen.addingNewModule = false;
		selectModuleSideScreen.SetTarget(this.reorderable.gameObject);
		this.ScrollToTargetPoint(scrollViewPosition);
	}

	// Token: 0x0600AE2D RID: 44589 RVA: 0x00425C18 File Offset: 0x00423E18
	private void ClickViewInterior()
	{
		ClustercraftExteriorDoor component = this.reorderable.GetComponent<ClustercraftExteriorDoor>();
		PassengerRocketModule component2 = this.reorderable.GetComponent<PassengerRocketModule>();
		WorldContainer targetWorld = component.GetTargetWorld();
		WorldContainer myWorld = component.GetMyWorld();
		if (ClusterManager.Instance.activeWorld == targetWorld)
		{
			if (myWorld.id != 255)
			{
				AudioMixer.instance.Stop(component2.interiorReverbSnapshot, STOP_MODE.ALLOWFADEOUT);
				AudioMixer.instance.PauseSpaceVisibleSnapshot(false);
				ClusterManager.Instance.SetActiveWorld(myWorld.id);
			}
		}
		else
		{
			AudioMixer.instance.Start(component2.interiorReverbSnapshot);
			AudioMixer.instance.PauseSpaceVisibleSnapshot(true);
			ClusterManager.Instance.SetActiveWorld(targetWorld.id);
		}
		DetailsScreen.Instance.ClearSecondarySideScreen();
		this.UpdateButtonStates();
	}

	// Token: 0x0400890A RID: 35082
	public static RocketModuleSideScreen instance;

	// Token: 0x0400890B RID: 35083
	private ReorderableBuilding reorderable;

	// Token: 0x0400890C RID: 35084
	public KScreen changeModuleSideScreen;

	// Token: 0x0400890D RID: 35085
	public Image moduleIcon;

	// Token: 0x0400890E RID: 35086
	[Header("Buttons")]
	public KButton addNewModuleButton;

	// Token: 0x0400890F RID: 35087
	public KButton removeModuleButton;

	// Token: 0x04008910 RID: 35088
	public KButton changeModuleButton;

	// Token: 0x04008911 RID: 35089
	public KButton moveModuleUpButton;

	// Token: 0x04008912 RID: 35090
	public KButton moveModuleDownButton;

	// Token: 0x04008913 RID: 35091
	public KButton viewInteriorButton;

	// Token: 0x04008914 RID: 35092
	[Header("Labels")]
	public LocText moduleNameLabel;

	// Token: 0x04008915 RID: 35093
	public LocText moduleDescriptionLabel;

	// Token: 0x04008916 RID: 35094
	public TextStyleSetting nameSetting;

	// Token: 0x04008917 RID: 35095
	public TextStyleSetting descriptionSetting;
}
