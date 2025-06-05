using System;
using UnityEngine;

// Token: 0x02001DA8 RID: 7592
public class KleiPermitBuildingAnimateIn : MonoBehaviour
{
	// Token: 0x06009EA2 RID: 40610 RVA: 0x003DCFC8 File Offset: 0x003DB1C8
	private void Awake()
	{
		this.placeAnimController.TintColour = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 1);
		this.colorAnimController.TintColour = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 1);
		this.updater = Updater.Parallel(new Updater[]
		{
			KleiPermitBuildingAnimateIn.MakeAnimInUpdater(this.sourceAnimController, this.placeAnimController, this.colorAnimController),
			this.extraUpdater
		});
	}

	// Token: 0x06009EA3 RID: 40611 RVA: 0x0010BB34 File Offset: 0x00109D34
	private void Update()
	{
		this.sourceAnimController.gameObject.SetActive(false);
		this.updater.Internal_Update(Time.unscaledDeltaTime);
	}

	// Token: 0x06009EA4 RID: 40612 RVA: 0x0010BB58 File Offset: 0x00109D58
	private void OnDisable()
	{
		this.sourceAnimController.gameObject.SetActive(true);
		UnityEngine.Object.Destroy(this.placeAnimController.gameObject);
		UnityEngine.Object.Destroy(this.colorAnimController.gameObject);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06009EA5 RID: 40613 RVA: 0x003DD054 File Offset: 0x003DB254
	public static KleiPermitBuildingAnimateIn MakeFor(KBatchedAnimController sourceAnimController, Updater extraUpdater = default(Updater))
	{
		sourceAnimController.gameObject.SetActive(false);
		KBatchedAnimController kbatchedAnimController = UnityEngine.Object.Instantiate<KBatchedAnimController>(sourceAnimController, sourceAnimController.transform.parent, false);
		kbatchedAnimController.gameObject.name = "KleiPermitBuildingAnimateIn.placeAnimController";
		kbatchedAnimController.initialAnim = "place";
		KBatchedAnimController kbatchedAnimController2 = UnityEngine.Object.Instantiate<KBatchedAnimController>(sourceAnimController, sourceAnimController.transform.parent, false);
		kbatchedAnimController2.gameObject.name = "KleiPermitBuildingAnimateIn.colorAnimController";
		KAnimFileData data = sourceAnimController.AnimFiles[0].GetData();
		KAnim.Anim anim = data.GetAnim("idle");
		if (anim == null)
		{
			anim = data.GetAnim("off");
			if (anim == null)
			{
				anim = data.GetAnim(0);
			}
		}
		kbatchedAnimController2.initialAnim = anim.name;
		GameObject gameObject = new GameObject("KleiPermitBuildingAnimateIn");
		gameObject.SetActive(false);
		gameObject.transform.SetParent(sourceAnimController.transform.parent, false);
		KleiPermitBuildingAnimateIn kleiPermitBuildingAnimateIn = gameObject.AddComponent<KleiPermitBuildingAnimateIn>();
		kleiPermitBuildingAnimateIn.sourceAnimController = sourceAnimController;
		kleiPermitBuildingAnimateIn.placeAnimController = kbatchedAnimController;
		kleiPermitBuildingAnimateIn.colorAnimController = kbatchedAnimController2;
		kleiPermitBuildingAnimateIn.extraUpdater = ((extraUpdater.fn == null) ? Updater.None() : extraUpdater);
		kbatchedAnimController.gameObject.SetActive(true);
		kbatchedAnimController2.gameObject.SetActive(true);
		gameObject.SetActive(true);
		return kleiPermitBuildingAnimateIn;
	}

	// Token: 0x06009EA6 RID: 40614 RVA: 0x003DD17C File Offset: 0x003DB37C
	public static Updater MakeAnimInUpdater(KBatchedAnimController sourceAnimController, KBatchedAnimController placeAnimController, KBatchedAnimController colorAnimController)
	{
		return Updater.Parallel(new Updater[]
		{
			Updater.Series(new Updater[]
			{
				Updater.Ease(delegate(float alpha)
				{
					placeAnimController.TintColour = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)Mathf.Clamp(alpha, 1f, 255f));
				}, 1f, 255f, 0.1f, Easing.CubicOut, -1f),
				Updater.Ease(delegate(float alpha)
				{
					placeAnimController.TintColour = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)Mathf.Clamp(255f - alpha, 1f, 255f));
					colorAnimController.TintColour = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)Mathf.Clamp(alpha, 1f, 255f));
				}, 1f, 255f, 0.3f, Easing.CubicIn, -1f)
			}),
			Updater.Series(new Updater[]
			{
				Updater.Ease(delegate(float scale)
				{
					scale = sourceAnimController.transform.localScale.x * scale;
					placeAnimController.transform.localScale = Vector3.one * scale;
					colorAnimController.transform.localScale = Vector3.one * scale;
				}, 1f, 1.012f, 0.2f, Easing.CubicOut, -1f),
				Updater.Ease(delegate(float scale)
				{
					scale = sourceAnimController.transform.localScale.x * scale;
					placeAnimController.transform.localScale = Vector3.one * scale;
					colorAnimController.transform.localScale = Vector3.one * scale;
				}, 1.012f, 1f, 0.1f, Easing.CubicIn, -1f)
			})
		});
	}

	// Token: 0x04007C96 RID: 31894
	private KBatchedAnimController sourceAnimController;

	// Token: 0x04007C97 RID: 31895
	private KBatchedAnimController placeAnimController;

	// Token: 0x04007C98 RID: 31896
	private KBatchedAnimController colorAnimController;

	// Token: 0x04007C99 RID: 31897
	private Updater updater;

	// Token: 0x04007C9A RID: 31898
	private Updater extraUpdater;
}
