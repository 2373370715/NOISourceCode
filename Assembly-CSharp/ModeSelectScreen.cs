using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001E93 RID: 7827
public class ModeSelectScreen : NewGameFlowScreen
{
	// Token: 0x0600A417 RID: 42007 RVA: 0x0010EF02 File Offset: 0x0010D102
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.LoadWorldAndClusterData();
	}

	// Token: 0x0600A418 RID: 42008 RVA: 0x003F40FC File Offset: 0x003F22FC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		HierarchyReferences component = this.survivalButton.GetComponent<HierarchyReferences>();
		this.survivalButtonHeader = component.GetReference<RectTransform>("HeaderBackground").GetComponent<Image>();
		this.survivalButtonSelectionFrame = component.GetReference<RectTransform>("SelectionFrame").GetComponent<Image>();
		MultiToggle multiToggle = this.survivalButton;
		multiToggle.onEnter = (System.Action)Delegate.Combine(multiToggle.onEnter, new System.Action(this.OnHoverEnterSurvival));
		MultiToggle multiToggle2 = this.survivalButton;
		multiToggle2.onExit = (System.Action)Delegate.Combine(multiToggle2.onExit, new System.Action(this.OnHoverExitSurvival));
		MultiToggle multiToggle3 = this.survivalButton;
		multiToggle3.onClick = (System.Action)Delegate.Combine(multiToggle3.onClick, new System.Action(this.OnClickSurvival));
		HierarchyReferences component2 = this.nosweatButton.GetComponent<HierarchyReferences>();
		this.nosweatButtonHeader = component2.GetReference<RectTransform>("HeaderBackground").GetComponent<Image>();
		this.nosweatButtonSelectionFrame = component2.GetReference<RectTransform>("SelectionFrame").GetComponent<Image>();
		MultiToggle multiToggle4 = this.nosweatButton;
		multiToggle4.onEnter = (System.Action)Delegate.Combine(multiToggle4.onEnter, new System.Action(this.OnHoverEnterNosweat));
		MultiToggle multiToggle5 = this.nosweatButton;
		multiToggle5.onExit = (System.Action)Delegate.Combine(multiToggle5.onExit, new System.Action(this.OnHoverExitNosweat));
		MultiToggle multiToggle6 = this.nosweatButton;
		multiToggle6.onClick = (System.Action)Delegate.Combine(multiToggle6.onClick, new System.Action(this.OnClickNosweat));
		this.closeButton.onClick += base.NavigateBackward;
	}

	// Token: 0x0600A419 RID: 42009 RVA: 0x003F4280 File Offset: 0x003F2480
	private void OnHoverEnterSurvival()
	{
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Mouseover", false));
		this.survivalButtonSelectionFrame.SetAlpha(1f);
		this.survivalButtonHeader.color = new Color(0.7019608f, 0.3647059f, 0.53333336f, 1f);
		this.descriptionArea.text = UI.FRONTEND.MODESELECTSCREEN.SURVIVAL_DESC;
	}

	// Token: 0x0600A41A RID: 42010 RVA: 0x003F42E8 File Offset: 0x003F24E8
	private void OnHoverExitSurvival()
	{
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Mouseover", false));
		this.survivalButtonSelectionFrame.SetAlpha(0f);
		this.survivalButtonHeader.color = new Color(0.30980393f, 0.34117648f, 0.38431373f, 1f);
		this.descriptionArea.text = UI.FRONTEND.MODESELECTSCREEN.BLANK_DESC;
	}

	// Token: 0x0600A41B RID: 42011 RVA: 0x0010EF10 File Offset: 0x0010D110
	private void OnClickSurvival()
	{
		this.Deactivate();
		CustomGameSettings.Instance.SetSurvivalDefaults();
		base.NavigateForward();
	}

	// Token: 0x0600A41C RID: 42012 RVA: 0x0010EF28 File Offset: 0x0010D128
	private void LoadWorldAndClusterData()
	{
		if (ModeSelectScreen.dataLoaded)
		{
			return;
		}
		CustomGameSettings.Instance.LoadClusters();
		Global.Instance.modManager.Report(base.gameObject);
		ModeSelectScreen.dataLoaded = true;
	}

	// Token: 0x0600A41D RID: 42013 RVA: 0x003F4350 File Offset: 0x003F2550
	private void OnHoverEnterNosweat()
	{
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Mouseover", false));
		this.nosweatButtonSelectionFrame.SetAlpha(1f);
		this.nosweatButtonHeader.color = new Color(0.7019608f, 0.3647059f, 0.53333336f, 1f);
		this.descriptionArea.text = UI.FRONTEND.MODESELECTSCREEN.NOSWEAT_DESC;
	}

	// Token: 0x0600A41E RID: 42014 RVA: 0x003F43B8 File Offset: 0x003F25B8
	private void OnHoverExitNosweat()
	{
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Mouseover", false));
		this.nosweatButtonSelectionFrame.SetAlpha(0f);
		this.nosweatButtonHeader.color = new Color(0.30980393f, 0.34117648f, 0.38431373f, 1f);
		this.descriptionArea.text = UI.FRONTEND.MODESELECTSCREEN.BLANK_DESC;
	}

	// Token: 0x0600A41F RID: 42015 RVA: 0x0010EF57 File Offset: 0x0010D157
	private void OnClickNosweat()
	{
		this.Deactivate();
		CustomGameSettings.Instance.SetNosweatDefaults();
		base.NavigateForward();
	}

	// Token: 0x0400803F RID: 32831
	[SerializeField]
	private MultiToggle nosweatButton;

	// Token: 0x04008040 RID: 32832
	private Image nosweatButtonHeader;

	// Token: 0x04008041 RID: 32833
	private Image nosweatButtonSelectionFrame;

	// Token: 0x04008042 RID: 32834
	[SerializeField]
	private MultiToggle survivalButton;

	// Token: 0x04008043 RID: 32835
	private Image survivalButtonHeader;

	// Token: 0x04008044 RID: 32836
	private Image survivalButtonSelectionFrame;

	// Token: 0x04008045 RID: 32837
	[SerializeField]
	private LocText descriptionArea;

	// Token: 0x04008046 RID: 32838
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04008047 RID: 32839
	[SerializeField]
	private KBatchedAnimController nosweatAnim;

	// Token: 0x04008048 RID: 32840
	[SerializeField]
	private KBatchedAnimController survivalAnim;

	// Token: 0x04008049 RID: 32841
	private static bool dataLoaded;
}
