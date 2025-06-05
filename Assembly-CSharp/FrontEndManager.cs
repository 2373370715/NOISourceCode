using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001D2F RID: 7471
[AddComponentMenu("KMonoBehaviour/scripts/FrontEndManager")]
public class FrontEndManager : KMonoBehaviour
{
	// Token: 0x06009BFD RID: 39933 RVA: 0x003CEDF8 File Offset: 0x003CCFF8
	protected override void OnPrefabInit()
	{
		FrontEndManager.<>c__DisplayClass2_0 CS$<>8__locals1 = new FrontEndManager.<>c__DisplayClass2_0();
		CS$<>8__locals1.<>4__this = this;
		base.OnPrefabInit();
		FrontEndManager.Instance = this;
		GameObject gameObject = base.gameObject;
		Util.KInstantiateUI(DlcManager.IsExpansion1Active() ? ScreenPrefabs.Instance.MainMenuForSpacedOut : ScreenPrefabs.Instance.MainMenuForVanilla, gameObject, true);
		if (!FrontEndManager.firstInit)
		{
			return;
		}
		FrontEndManager.firstInit = false;
		GameObject[] array = new GameObject[]
		{
			ScreenPrefabs.Instance.MainMenuIntroShort,
			ScreenPrefabs.Instance.MainMenuHealthyGameMessage,
			ScreenPrefabs.Instance.DLCBetaWarningScreen
		};
		for (int i = 0; i < array.Length; i++)
		{
			Util.KInstantiateUI(array[i], gameObject, true);
		}
		CS$<>8__locals1.screensPrefabsToSpawn = new GameObject[]
		{
			ScreenPrefabs.Instance.KleiItemDropScreen,
			ScreenPrefabs.Instance.LockerMenuScreen,
			ScreenPrefabs.Instance.LockerNavigator
		};
		CS$<>8__locals1.gameObjectsToDestroyOnNextCreate = new List<GameObject>();
		FrontEndManager.<>c__DisplayClass2_1 CS$<>8__locals2 = new FrontEndManager.<>c__DisplayClass2_1();
		CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
		CS$<>8__locals2.coroutineRunner = CoroutineRunner.Create();
		UnityEngine.Object.DontDestroyOnLoad(CS$<>8__locals2.coroutineRunner);
		CS$<>8__locals2.CS$<>8__locals1.<OnPrefabInit>g__CreateCanvases|0();
		Singleton<KBatchedAnimUpdater>.Instance.OnClear += CS$<>8__locals2.<OnPrefabInit>g__RecreateCanvases|1;
	}

	// Token: 0x06009BFE RID: 39934 RVA: 0x00109FC8 File Offset: 0x001081C8
	protected override void OnForcedCleanUp()
	{
		FrontEndManager.Instance = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x06009BFF RID: 39935 RVA: 0x003CEF28 File Offset: 0x003CD128
	private void LateUpdate()
	{
		if (global::Debug.developerConsoleVisible)
		{
			global::Debug.developerConsoleVisible = false;
		}
		KAnimBatchManager.Instance().UpdateActiveArea(new Vector2I(0, 0), new Vector2I(9999, 9999));
		KAnimBatchManager.Instance().UpdateDirty(Time.frameCount);
		KAnimBatchManager.Instance().Render();
	}

	// Token: 0x06009C00 RID: 39936 RVA: 0x003CEF7C File Offset: 0x003CD17C
	public GameObject MakeKleiCanvas(string gameObjectName = "Canvas")
	{
		GameObject gameObject = new GameObject(gameObjectName, new Type[]
		{
			typeof(RectTransform)
		});
		this.ConfigureAsKleiCanvas(gameObject);
		return gameObject;
	}

	// Token: 0x06009C01 RID: 39937 RVA: 0x003CEFAC File Offset: 0x003CD1AC
	public void ConfigureAsKleiCanvas(GameObject gameObject)
	{
		Canvas canvas = gameObject.AddOrGet<Canvas>();
		canvas.renderMode = RenderMode.ScreenSpaceOverlay;
		canvas.sortingOrder = 10;
		canvas.pixelPerfect = false;
		canvas.additionalShaderChannels = (AdditionalCanvasShaderChannels.TexCoord1 | AdditionalCanvasShaderChannels.Normal | AdditionalCanvasShaderChannels.Tangent);
		GraphicRaycaster graphicRaycaster = gameObject.AddOrGet<GraphicRaycaster>();
		graphicRaycaster.ignoreReversedGraphics = true;
		graphicRaycaster.blockingObjects = GraphicRaycaster.BlockingObjects.None;
		graphicRaycaster.blockingMask = -1;
		CanvasScaler canvasScaler = gameObject.AddOrGet<CanvasScaler>();
		canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
		canvasScaler.referencePixelsPerUnit = 100f;
		gameObject.AddOrGet<KCanvasScaler>();
	}

	// Token: 0x04007A09 RID: 31241
	public static FrontEndManager Instance;

	// Token: 0x04007A0A RID: 31242
	public static bool firstInit = true;
}
