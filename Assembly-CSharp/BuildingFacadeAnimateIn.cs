using System;
using UnityEngine;

// Token: 0x02000CF3 RID: 3315
public class BuildingFacadeAnimateIn : MonoBehaviour
{
	// Token: 0x06003FA1 RID: 16289 RVA: 0x00246300 File Offset: 0x00244500
	private void Awake()
	{
		this.placeAnimController.TintColour = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 1);
		this.colorAnimController.TintColour = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 1);
		this.updater = Updater.Series(new Updater[]
		{
			KleiPermitBuildingAnimateIn.MakeAnimInUpdater(this.sourceAnimController, this.placeAnimController, this.colorAnimController),
			Updater.Do(delegate()
			{
				UnityEngine.Object.Destroy(base.gameObject);
			})
		});
	}

	// Token: 0x06003FA2 RID: 16290 RVA: 0x000CDCA7 File Offset: 0x000CBEA7
	private void Update()
	{
		if (this.sourceAnimController.IsNullOrDestroyed())
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		BuildingFacadeAnimateIn.SetVisibilityOn(this.sourceAnimController, false);
		this.updater.Internal_Update(Time.unscaledDeltaTime);
	}

	// Token: 0x06003FA3 RID: 16291 RVA: 0x00246394 File Offset: 0x00244594
	private void OnDisable()
	{
		if (!this.sourceAnimController.IsNullOrDestroyed())
		{
			BuildingFacadeAnimateIn.SetVisibilityOn(this.sourceAnimController, true);
		}
		UnityEngine.Object.Destroy(this.placeAnimController.gameObject);
		UnityEngine.Object.Destroy(this.colorAnimController.gameObject);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06003FA4 RID: 16292 RVA: 0x002463E8 File Offset: 0x002445E8
	public static BuildingFacadeAnimateIn MakeFor(KBatchedAnimController sourceAnimController)
	{
		BuildingFacadeAnimateIn.SetVisibilityOn(sourceAnimController, false);
		KBatchedAnimController kbatchedAnimController = BuildingFacadeAnimateIn.SpawnAnimFrom(sourceAnimController);
		kbatchedAnimController.gameObject.name = "BuildingFacadeAnimateIn.placeAnimController";
		kbatchedAnimController.initialAnim = "place";
		KBatchedAnimController kbatchedAnimController2 = BuildingFacadeAnimateIn.SpawnAnimFrom(sourceAnimController);
		kbatchedAnimController2.gameObject.name = "BuildingFacadeAnimateIn.colorAnimController";
		kbatchedAnimController2.initialAnim = ((sourceAnimController.CurrentAnim != null) ? sourceAnimController.CurrentAnim.name : sourceAnimController.AnimFiles[0].GetData().GetAnim(0).name);
		GameObject gameObject = new GameObject("BuildingFacadeAnimateIn");
		gameObject.SetActive(false);
		gameObject.transform.SetParent(sourceAnimController.transform.parent, false);
		BuildingFacadeAnimateIn buildingFacadeAnimateIn = gameObject.AddComponent<BuildingFacadeAnimateIn>();
		buildingFacadeAnimateIn.sourceAnimController = sourceAnimController;
		buildingFacadeAnimateIn.placeAnimController = kbatchedAnimController;
		buildingFacadeAnimateIn.colorAnimController = kbatchedAnimController2;
		kbatchedAnimController.gameObject.SetActive(true);
		kbatchedAnimController2.gameObject.SetActive(true);
		gameObject.SetActive(true);
		return buildingFacadeAnimateIn;
	}

	// Token: 0x06003FA5 RID: 16293 RVA: 0x002464CC File Offset: 0x002446CC
	private static void SetVisibilityOn(KBatchedAnimController animController, bool isVisible)
	{
		animController.SetVisiblity(isVisible);
		foreach (KBatchedAnimController kbatchedAnimController in animController.GetComponentsInChildren<KBatchedAnimController>(true))
		{
			if (kbatchedAnimController.batchGroupID == animController.batchGroupID)
			{
				kbatchedAnimController.SetVisiblity(isVisible);
			}
		}
	}

	// Token: 0x06003FA6 RID: 16294 RVA: 0x00246514 File Offset: 0x00244714
	private static KBatchedAnimController SpawnAnimFrom(KBatchedAnimController sourceAnimController)
	{
		GameObject gameObject = new GameObject();
		gameObject.SetActive(false);
		gameObject.transform.SetParent(sourceAnimController.transform.parent, false);
		gameObject.transform.localPosition = sourceAnimController.transform.localPosition;
		gameObject.transform.localRotation = sourceAnimController.transform.localRotation;
		gameObject.transform.localScale = sourceAnimController.transform.localScale;
		gameObject.layer = sourceAnimController.gameObject.layer;
		KBatchedAnimController kbatchedAnimController = gameObject.AddComponent<KBatchedAnimController>();
		kbatchedAnimController.materialType = sourceAnimController.materialType;
		kbatchedAnimController.initialMode = sourceAnimController.initialMode;
		kbatchedAnimController.AnimFiles = sourceAnimController.AnimFiles;
		kbatchedAnimController.Offset = sourceAnimController.Offset;
		kbatchedAnimController.animWidth = sourceAnimController.animWidth;
		kbatchedAnimController.animHeight = sourceAnimController.animHeight;
		kbatchedAnimController.animScale = sourceAnimController.animScale;
		kbatchedAnimController.sceneLayer = sourceAnimController.sceneLayer;
		kbatchedAnimController.fgLayer = sourceAnimController.fgLayer;
		kbatchedAnimController.FlipX = sourceAnimController.FlipX;
		kbatchedAnimController.FlipY = sourceAnimController.FlipY;
		kbatchedAnimController.Rotation = sourceAnimController.Rotation;
		kbatchedAnimController.Pivot = sourceAnimController.Pivot;
		return kbatchedAnimController;
	}

	// Token: 0x04002BF1 RID: 11249
	private KBatchedAnimController sourceAnimController;

	// Token: 0x04002BF2 RID: 11250
	private KBatchedAnimController placeAnimController;

	// Token: 0x04002BF3 RID: 11251
	private KBatchedAnimController colorAnimController;

	// Token: 0x04002BF4 RID: 11252
	private Updater updater;
}
