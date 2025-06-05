using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001D86 RID: 7558
public class KModalScreen : KScreen
{
	// Token: 0x06009DCA RID: 40394 RVA: 0x0010B1C9 File Offset: 0x001093C9
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.backgroundRectTransform = KModalScreen.MakeScreenModal(this);
	}

	// Token: 0x06009DCB RID: 40395 RVA: 0x003D8DF0 File Offset: 0x003D6FF0
	public static RectTransform MakeScreenModal(KScreen screen)
	{
		screen.ConsumeMouseScroll = true;
		screen.activateOnSpawn = true;
		GameObject gameObject = new GameObject("background");
		gameObject.AddComponent<LayoutElement>().ignoreLayout = true;
		gameObject.AddComponent<CanvasRenderer>();
		Image image = gameObject.AddComponent<Image>();
		image.color = new Color32(0, 0, 0, 160);
		image.raycastTarget = true;
		RectTransform component = gameObject.GetComponent<RectTransform>();
		component.SetParent(screen.transform);
		KModalScreen.ResizeBackground(component);
		return component;
	}

	// Token: 0x06009DCC RID: 40396 RVA: 0x003D8E64 File Offset: 0x003D7064
	public static void ResizeBackground(RectTransform rectTransform)
	{
		rectTransform.SetAsFirstSibling();
		rectTransform.SetLocalPosition(Vector3.zero);
		rectTransform.localScale = Vector3.one;
		rectTransform.anchorMin = new Vector2(0f, 0f);
		rectTransform.anchorMax = new Vector2(1f, 1f);
		rectTransform.sizeDelta = new Vector2(0f, 0f);
	}

	// Token: 0x06009DCD RID: 40397 RVA: 0x003D8ED0 File Offset: 0x003D70D0
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		if (CameraController.Instance != null)
		{
			CameraController.Instance.DisableUserCameraControl = true;
		}
		if (ScreenResize.Instance != null)
		{
			ScreenResize instance = ScreenResize.Instance;
			instance.OnResize = (System.Action)Delegate.Combine(instance.OnResize, new System.Action(this.OnResize));
		}
	}

	// Token: 0x06009DCE RID: 40398 RVA: 0x003D8F30 File Offset: 0x003D7130
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		if (CameraController.Instance != null)
		{
			CameraController.Instance.DisableUserCameraControl = false;
		}
		base.Trigger(476357528, null);
		if (ScreenResize.Instance != null)
		{
			ScreenResize instance = ScreenResize.Instance;
			instance.OnResize = (System.Action)Delegate.Remove(instance.OnResize, new System.Action(this.OnResize));
		}
	}

	// Token: 0x06009DCF RID: 40399 RVA: 0x0010B1DD File Offset: 0x001093DD
	private void OnResize()
	{
		KModalScreen.ResizeBackground(this.backgroundRectTransform);
	}

	// Token: 0x06009DD0 RID: 40400 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public override bool IsModal()
	{
		return true;
	}

	// Token: 0x06009DD1 RID: 40401 RVA: 0x000CD7B4 File Offset: 0x000CB9B4
	public override float GetSortKey()
	{
		return 100f;
	}

	// Token: 0x06009DD2 RID: 40402 RVA: 0x0010B1EA File Offset: 0x001093EA
	protected override void OnActivate()
	{
		this.OnShow(true);
	}

	// Token: 0x06009DD3 RID: 40403 RVA: 0x0010B1F3 File Offset: 0x001093F3
	protected override void OnDeactivate()
	{
		this.OnShow(false);
	}

	// Token: 0x06009DD4 RID: 40404 RVA: 0x003D8F9C File Offset: 0x003D719C
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (this.pause && SpeedControlScreen.Instance != null)
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
	}

	// Token: 0x06009DD5 RID: 40405 RVA: 0x003D8FFC File Offset: 0x003D71FC
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.Consumed)
		{
			return;
		}
		if (Game.Instance != null && (e.TryConsume(global::Action.TogglePause) || e.TryConsume(global::Action.CycleSpeed)))
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative", false));
		}
		if (!e.Consumed && (e.TryConsume(global::Action.Escape) || (e.TryConsume(global::Action.MouseRight) && this.canBackoutWithRightClick)))
		{
			this.Deactivate();
		}
		base.OnKeyDown(e);
		e.Consumed = true;
	}

	// Token: 0x06009DD6 RID: 40406 RVA: 0x0010B168 File Offset: 0x00109368
	public override void OnKeyUp(KButtonEvent e)
	{
		base.OnKeyUp(e);
		e.Consumed = true;
	}

	// Token: 0x04007BF2 RID: 31730
	private bool shown;

	// Token: 0x04007BF3 RID: 31731
	public bool pause = true;

	// Token: 0x04007BF4 RID: 31732
	[Tooltip("Only used for main menu")]
	public bool canBackoutWithRightClick;

	// Token: 0x04007BF5 RID: 31733
	private RectTransform backgroundRectTransform;
}
