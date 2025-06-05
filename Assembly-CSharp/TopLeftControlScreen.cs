using System;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200209E RID: 8350
public class TopLeftControlScreen : KScreen
{
	// Token: 0x0600B20E RID: 45582 RVA: 0x00118455 File Offset: 0x00116655
	public static void DestroyInstance()
	{
		TopLeftControlScreen.Instance = null;
	}

	// Token: 0x0600B20F RID: 45583 RVA: 0x0043BBBC File Offset: 0x00439DBC
	protected override void OnActivate()
	{
		base.OnActivate();
		TopLeftControlScreen.Instance = this;
		this.RefreshName();
		KInputManager.InputChange.AddListener(new UnityAction(this.ResetToolTip));
		this.UpdateSandboxToggleState();
		MultiToggle multiToggle = this.sandboxToggle;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(this.OnClickSandboxToggle));
		MultiToggle multiToggle2 = this.kleiItemDropButton;
		multiToggle2.onClick = (System.Action)Delegate.Combine(multiToggle2.onClick, new System.Action(this.OnClickKleiItemDropButton));
		KleiItemsStatusRefresher.AddOrGetListener(this).OnRefreshUI(new System.Action(this.RefreshKleiItemDropButton));
		this.RefreshKleiItemDropButton();
		Game.Instance.Subscribe(-1948169901, delegate(object data)
		{
			this.UpdateSandboxToggleState();
		});
		LayoutRebuilder.ForceRebuildLayoutImmediate(this.secondaryRow);
	}

	// Token: 0x0600B210 RID: 45584 RVA: 0x0011845D File Offset: 0x0011665D
	protected override void OnForcedCleanUp()
	{
		KInputManager.InputChange.RemoveListener(new UnityAction(this.ResetToolTip));
		base.OnForcedCleanUp();
	}

	// Token: 0x0600B211 RID: 45585 RVA: 0x0011847B File Offset: 0x0011667B
	public void RefreshName()
	{
		if (SaveGame.Instance != null)
		{
			this.locText.text = SaveGame.Instance.BaseName;
		}
	}

	// Token: 0x0600B212 RID: 45586 RVA: 0x0043BC8C File Offset: 0x00439E8C
	public void ResetToolTip()
	{
		if (this.CheckSandboxModeLocked())
		{
			this.sandboxToggle.GetComponent<ToolTip>().SetSimpleTooltip(GameUtil.ReplaceHotkeyString(UI.SANDBOX_TOGGLE.TOOLTIP_LOCKED, global::Action.ToggleSandboxTools));
			return;
		}
		this.sandboxToggle.GetComponent<ToolTip>().SetSimpleTooltip(GameUtil.ReplaceHotkeyString(UI.SANDBOX_TOGGLE.TOOLTIP_UNLOCKED, global::Action.ToggleSandboxTools));
	}

	// Token: 0x0600B213 RID: 45587 RVA: 0x0043BCEC File Offset: 0x00439EEC
	public void UpdateSandboxToggleState()
	{
		if (this.CheckSandboxModeLocked())
		{
			this.sandboxToggle.GetComponent<ToolTip>().SetSimpleTooltip(GameUtil.ReplaceHotkeyString(UI.SANDBOX_TOGGLE.TOOLTIP_LOCKED, global::Action.ToggleSandboxTools));
			this.sandboxToggle.ChangeState(0);
		}
		else
		{
			this.sandboxToggle.GetComponent<ToolTip>().SetSimpleTooltip(GameUtil.ReplaceHotkeyString(UI.SANDBOX_TOGGLE.TOOLTIP_UNLOCKED, global::Action.ToggleSandboxTools));
			this.sandboxToggle.ChangeState(Game.Instance.SandboxModeActive ? 2 : 1);
		}
		this.sandboxToggle.gameObject.SetActive(SaveGame.Instance.sandboxEnabled);
	}

	// Token: 0x0600B214 RID: 45588 RVA: 0x0043BD8C File Offset: 0x00439F8C
	private void OnClickSandboxToggle()
	{
		if (this.CheckSandboxModeLocked())
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative", false));
		}
		else
		{
			Game.Instance.SandboxModeActive = !Game.Instance.SandboxModeActive;
			KMonoBehaviour.PlaySound(Game.Instance.SandboxModeActive ? GlobalAssets.GetSound("SandboxTool_Toggle_On", false) : GlobalAssets.GetSound("SandboxTool_Toggle_Off", false));
		}
		this.UpdateSandboxToggleState();
	}

	// Token: 0x0600B215 RID: 45589 RVA: 0x0043BDFC File Offset: 0x00439FFC
	private void RefreshKleiItemDropButton()
	{
		if (!KleiItemDropScreen.HasItemsToShow())
		{
			this.kleiItemDropButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.ITEM_DROP_SCREEN.IN_GAME_BUTTON.TOOLTIP_ERROR_NO_ITEMS);
			this.kleiItemDropButton.ChangeState(1);
			return;
		}
		this.kleiItemDropButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.ITEM_DROP_SCREEN.IN_GAME_BUTTON.TOOLTIP_ITEMS_AVAILABLE);
		this.kleiItemDropButton.ChangeState(2);
	}

	// Token: 0x0600B216 RID: 45590 RVA: 0x0011849F File Offset: 0x0011669F
	private void OnClickKleiItemDropButton()
	{
		this.RefreshKleiItemDropButton();
		if (!KleiItemDropScreen.HasItemsToShow())
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative", false));
			return;
		}
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click", false));
		UnityEngine.Object.FindObjectOfType<KleiItemDropScreen>(true).Show(true);
	}

	// Token: 0x0600B217 RID: 45591 RVA: 0x001184DB File Offset: 0x001166DB
	private bool CheckSandboxModeLocked()
	{
		return !SaveGame.Instance.sandboxEnabled;
	}

	// Token: 0x04008C91 RID: 35985
	public static TopLeftControlScreen Instance;

	// Token: 0x04008C92 RID: 35986
	[SerializeField]
	private MultiToggle sandboxToggle;

	// Token: 0x04008C93 RID: 35987
	[SerializeField]
	private MultiToggle kleiItemDropButton;

	// Token: 0x04008C94 RID: 35988
	[SerializeField]
	private LocText locText;

	// Token: 0x04008C95 RID: 35989
	[SerializeField]
	private RectTransform secondaryRow;

	// Token: 0x0200209F RID: 8351
	private enum MultiToggleState
	{
		// Token: 0x04008C97 RID: 35991
		Disabled,
		// Token: 0x04008C98 RID: 35992
		Off,
		// Token: 0x04008C99 RID: 35993
		On
	}
}
