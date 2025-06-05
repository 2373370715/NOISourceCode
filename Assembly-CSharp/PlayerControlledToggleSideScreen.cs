using System;
using UnityEngine;

// Token: 0x0200200E RID: 8206
public class PlayerControlledToggleSideScreen : SideScreenContent, IRenderEveryTick
{
	// Token: 0x0600AD99 RID: 44441 RVA: 0x0042353C File Offset: 0x0042173C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.toggleButton.onClick += this.ClickToggle;
		this.togglePendingStatusItem = new StatusItem("PlayerControlledToggleSideScreen", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
	}

	// Token: 0x0600AD9A RID: 44442 RVA: 0x00115422 File Offset: 0x00113622
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<IPlayerControlledToggle>() != null;
	}

	// Token: 0x0600AD9B RID: 44443 RVA: 0x00423590 File Offset: 0x00421790
	public void RenderEveryTick(float dt)
	{
		if (base.isActiveAndEnabled)
		{
			if (!this.keyDown && (Input.GetKeyDown(KeyCode.Return) & Time.unscaledTime - this.lastKeyboardShortcutTime > 0.1f))
			{
				if (SpeedControlScreen.Instance.IsPaused)
				{
					this.RequestToggle();
				}
				else
				{
					this.Toggle();
				}
				this.lastKeyboardShortcutTime = Time.unscaledTime;
				this.keyDown = true;
			}
			if (this.keyDown && Input.GetKeyUp(KeyCode.Return))
			{
				this.keyDown = false;
			}
		}
	}

	// Token: 0x0600AD9C RID: 44444 RVA: 0x0011542D File Offset: 0x0011362D
	private void ClickToggle()
	{
		if (SpeedControlScreen.Instance.IsPaused)
		{
			this.RequestToggle();
			return;
		}
		this.Toggle();
	}

	// Token: 0x0600AD9D RID: 44445 RVA: 0x00423610 File Offset: 0x00421810
	private void RequestToggle()
	{
		this.target.ToggleRequested = !this.target.ToggleRequested;
		if (this.target.ToggleRequested && SpeedControlScreen.Instance.IsPaused)
		{
			this.target.GetSelectable().SetStatusItem(Db.Get().StatusItemCategories.Main, this.togglePendingStatusItem, this);
		}
		else
		{
			this.target.GetSelectable().SetStatusItem(Db.Get().StatusItemCategories.Main, null, null);
		}
		this.UpdateVisuals(this.target.ToggleRequested ? (!this.target.ToggledOn()) : this.target.ToggledOn(), true);
	}

	// Token: 0x0600AD9E RID: 44446 RVA: 0x004236CC File Offset: 0x004218CC
	public override void SetTarget(GameObject new_target)
	{
		if (new_target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.target = new_target.GetComponent<IPlayerControlledToggle>();
		if (this.target == null)
		{
			global::Debug.LogError("The gameObject received is not an IPlayerControlledToggle");
			return;
		}
		this.UpdateVisuals(this.target.ToggleRequested ? (!this.target.ToggledOn()) : this.target.ToggledOn(), false);
		this.titleKey = this.target.SideScreenTitleKey;
	}

	// Token: 0x0600AD9F RID: 44447 RVA: 0x0042374C File Offset: 0x0042194C
	private void Toggle()
	{
		this.target.ToggledByPlayer();
		this.UpdateVisuals(this.target.ToggledOn(), true);
		this.target.ToggleRequested = false;
		this.target.GetSelectable().RemoveStatusItem(this.togglePendingStatusItem, false);
	}

	// Token: 0x0600ADA0 RID: 44448 RVA: 0x0042379C File Offset: 0x0042199C
	private void UpdateVisuals(bool state, bool smooth)
	{
		if (state != this.currentState)
		{
			if (smooth)
			{
				this.kbac.Play(state ? PlayerControlledToggleSideScreen.ON_ANIMS : PlayerControlledToggleSideScreen.OFF_ANIMS, KAnim.PlayMode.Once);
			}
			else
			{
				this.kbac.Play(state ? PlayerControlledToggleSideScreen.ON_ANIMS[1] : PlayerControlledToggleSideScreen.OFF_ANIMS[1], KAnim.PlayMode.Once, 1f, 0f);
			}
		}
		this.currentState = state;
	}

	// Token: 0x040088A3 RID: 34979
	public IPlayerControlledToggle target;

	// Token: 0x040088A4 RID: 34980
	public KButton toggleButton;

	// Token: 0x040088A5 RID: 34981
	protected static readonly HashedString[] ON_ANIMS = new HashedString[]
	{
		"on_pre",
		"on"
	};

	// Token: 0x040088A6 RID: 34982
	protected static readonly HashedString[] OFF_ANIMS = new HashedString[]
	{
		"off_pre",
		"off"
	};

	// Token: 0x040088A7 RID: 34983
	public float animScaleBase = 0.25f;

	// Token: 0x040088A8 RID: 34984
	private StatusItem togglePendingStatusItem;

	// Token: 0x040088A9 RID: 34985
	[SerializeField]
	private KBatchedAnimController kbac;

	// Token: 0x040088AA RID: 34986
	private float lastKeyboardShortcutTime;

	// Token: 0x040088AB RID: 34987
	private const float KEYBOARD_COOLDOWN = 0.1f;

	// Token: 0x040088AC RID: 34988
	private bool keyDown;

	// Token: 0x040088AD RID: 34989
	private bool currentState;
}
