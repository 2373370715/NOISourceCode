using System;
using UnityEngine;

// Token: 0x02001D85 RID: 7557
public class KModalButtonMenu : KButtonMenu
{
	// Token: 0x06009DBC RID: 40380 RVA: 0x0010B104 File Offset: 0x00109304
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.modalBackground = KModalScreen.MakeScreenModal(this);
	}

	// Token: 0x06009DBD RID: 40381 RVA: 0x0010B118 File Offset: 0x00109318
	protected override void OnCmpEnable()
	{
		KModalScreen.ResizeBackground(this.modalBackground);
		ScreenResize instance = ScreenResize.Instance;
		instance.OnResize = (System.Action)Delegate.Combine(instance.OnResize, new System.Action(this.OnResize));
	}

	// Token: 0x06009DBE RID: 40382 RVA: 0x003D8CE0 File Offset: 0x003D6EE0
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		if (this.childDialog == null)
		{
			base.Trigger(476357528, null);
		}
		ScreenResize instance = ScreenResize.Instance;
		instance.OnResize = (System.Action)Delegate.Remove(instance.OnResize, new System.Action(this.OnResize));
	}

	// Token: 0x06009DBF RID: 40383 RVA: 0x0010B14B File Offset: 0x0010934B
	private void OnResize()
	{
		KModalScreen.ResizeBackground(this.modalBackground);
	}

	// Token: 0x06009DC0 RID: 40384 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public override bool IsModal()
	{
		return true;
	}

	// Token: 0x06009DC1 RID: 40385 RVA: 0x000CD7B4 File Offset: 0x000CB9B4
	public override float GetSortKey()
	{
		return 100f;
	}

	// Token: 0x06009DC2 RID: 40386 RVA: 0x003D8D34 File Offset: 0x003D6F34
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (SpeedControlScreen.Instance != null)
		{
			if (show && !this.shown)
			{
				SpeedControlScreen.Instance.Pause(false, false);
			}
			else if (!show && this.shown)
			{
				SpeedControlScreen.Instance.Unpause(false);
			}
			this.shown = show;
		}
		if (CameraController.Instance != null)
		{
			CameraController.Instance.DisableUserCameraControl = show;
		}
	}

	// Token: 0x06009DC3 RID: 40387 RVA: 0x0010B158 File Offset: 0x00109358
	public override void OnKeyDown(KButtonEvent e)
	{
		base.OnKeyDown(e);
		e.Consumed = true;
	}

	// Token: 0x06009DC4 RID: 40388 RVA: 0x0010B168 File Offset: 0x00109368
	public override void OnKeyUp(KButtonEvent e)
	{
		base.OnKeyUp(e);
		e.Consumed = true;
	}

	// Token: 0x06009DC5 RID: 40389 RVA: 0x000AA038 File Offset: 0x000A8238
	public void SetBackgroundActive(bool active)
	{
	}

	// Token: 0x06009DC6 RID: 40390 RVA: 0x003D8DA4 File Offset: 0x003D6FA4
	protected GameObject ActivateChildScreen(GameObject screenPrefab)
	{
		GameObject gameObject = Util.KInstantiateUI(screenPrefab, base.transform.parent.gameObject, false);
		this.childDialog = gameObject;
		gameObject.Subscribe(476357528, new Action<object>(this.Unhide));
		this.Hide();
		return gameObject;
	}

	// Token: 0x06009DC7 RID: 40391 RVA: 0x0010B178 File Offset: 0x00109378
	private void Hide()
	{
		this.panelRoot.rectTransform().localScale = Vector3.zero;
	}

	// Token: 0x06009DC8 RID: 40392 RVA: 0x0010B18F File Offset: 0x0010938F
	private void Unhide(object data = null)
	{
		this.panelRoot.rectTransform().localScale = Vector3.one;
		this.childDialog.Unsubscribe(476357528, new Action<object>(this.Unhide));
		this.childDialog = null;
	}

	// Token: 0x04007BEE RID: 31726
	private bool shown;

	// Token: 0x04007BEF RID: 31727
	[SerializeField]
	private GameObject panelRoot;

	// Token: 0x04007BF0 RID: 31728
	private GameObject childDialog;

	// Token: 0x04007BF1 RID: 31729
	private RectTransform modalBackground;
}
