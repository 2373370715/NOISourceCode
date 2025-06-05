using System;
using Database;
using UnityEngine;

// Token: 0x02001DB1 RID: 7601
public class KleiPermitDioramaVis_BuildingHangingHook : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	// Token: 0x06009ECC RID: 40652 RVA: 0x000CEC86 File Offset: 0x000CCE86
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x06009ECD RID: 40653 RVA: 0x000AA038 File Offset: 0x000A8238
	public void ConfigureSetup()
	{
	}

	// Token: 0x06009ECE RID: 40654 RVA: 0x003DDC78 File Offset: 0x003DBE78
	public void ConfigureWith(PermitResource permit)
	{
		KleiPermitVisUtil.ConfigureToRenderBuilding(this.buildingKAnim, (BuildingFacadeResource)permit);
		KleiPermitVisUtil.ConfigureBuildingPosition(this.buildingKAnim.rectTransform(), this.buildingKAnimPosition, KleiPermitVisUtil.GetBuildingDef(permit), Alignment.Top());
		KleiPermitVisUtil.AnimateIn(this.buildingKAnim, default(Updater));
	}

	// Token: 0x04007CBF RID: 31935
	[SerializeField]
	private KBatchedAnimController buildingKAnim;

	// Token: 0x04007CC0 RID: 31936
	private PrefabDefinedUIPosition buildingKAnimPosition = new PrefabDefinedUIPosition();
}
