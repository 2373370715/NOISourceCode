using System;
using STRINGS;
using UnityEngine;

// Token: 0x02001B3B RID: 6971
public class LoadingOverlay : KModalScreen
{
	// Token: 0x06009227 RID: 37415 RVA: 0x00103FA6 File Offset: 0x001021A6
	protected override void OnPrefabInit()
	{
		this.pause = false;
		this.fadeIn = false;
		base.OnPrefabInit();
	}

	// Token: 0x06009228 RID: 37416 RVA: 0x00103FBC File Offset: 0x001021BC
	private void Update()
	{
		if (!this.loadNextFrame && this.showLoad)
		{
			this.loadNextFrame = true;
			this.showLoad = false;
			return;
		}
		if (this.loadNextFrame)
		{
			this.loadNextFrame = false;
			this.loadCb();
		}
	}

	// Token: 0x06009229 RID: 37417 RVA: 0x00103FF7 File Offset: 0x001021F7
	public static void DestroyInstance()
	{
		LoadingOverlay.instance = null;
	}

	// Token: 0x0600922A RID: 37418 RVA: 0x0039132C File Offset: 0x0038F52C
	public static void Load(System.Action cb)
	{
		GameObject gameObject = GameObject.Find("/SceneInitializerFE/FrontEndManager");
		if (LoadingOverlay.instance == null)
		{
			LoadingOverlay.instance = Util.KInstantiateUI<LoadingOverlay>(ScreenPrefabs.Instance.loadingOverlay.gameObject, (GameScreenManager.Instance == null) ? gameObject : GameScreenManager.Instance.ssOverlayCanvas, false);
			LoadingOverlay.instance.GetComponentInChildren<LocText>().SetText(UI.FRONTEND.LOADING);
		}
		if (GameScreenManager.Instance != null)
		{
			LoadingOverlay.instance.transform.SetParent(GameScreenManager.Instance.ssOverlayCanvas.transform);
			LoadingOverlay.instance.transform.SetSiblingIndex(GameScreenManager.Instance.ssOverlayCanvas.transform.childCount - 1);
		}
		else
		{
			LoadingOverlay.instance.transform.SetParent(gameObject.transform);
			LoadingOverlay.instance.transform.SetSiblingIndex(gameObject.transform.childCount - 1);
			if (MainMenu.Instance != null)
			{
				MainMenu.Instance.StopAmbience();
			}
		}
		LoadingOverlay.instance.loadCb = cb;
		LoadingOverlay.instance.showLoad = true;
		LoadingOverlay.instance.Activate();
	}

	// Token: 0x0600922B RID: 37419 RVA: 0x00103FFF File Offset: 0x001021FF
	public static void Clear()
	{
		if (LoadingOverlay.instance != null)
		{
			LoadingOverlay.instance.Deactivate();
		}
	}

	// Token: 0x04006EB9 RID: 28345
	private bool loadNextFrame;

	// Token: 0x04006EBA RID: 28346
	private bool showLoad;

	// Token: 0x04006EBB RID: 28347
	private System.Action loadCb;

	// Token: 0x04006EBC RID: 28348
	private static LoadingOverlay instance;
}
