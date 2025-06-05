using System;
using Database;
using UnityEngine;

// Token: 0x02001DB4 RID: 7604
public class KleiPermitDioramaVis_BuildingOnFloorBig : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	// Token: 0x06009ED8 RID: 40664 RVA: 0x000CEC86 File Offset: 0x000CCE86
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x06009ED9 RID: 40665 RVA: 0x000AA038 File Offset: 0x000A8238
	public void ConfigureSetup()
	{
	}

	// Token: 0x06009EDA RID: 40666 RVA: 0x003DDE64 File Offset: 0x003DC064
	public void ConfigureWith(PermitResource permit)
	{
		BuildingFacadeResource buildingPermit = (BuildingFacadeResource)permit;
		KleiPermitVisUtil.ConfigureToRenderBuilding(this.buildingKAnim, buildingPermit);
		KleiPermitVisUtil.AnimateIn(this.buildingKAnim, default(Updater));
	}

	// Token: 0x04007CC4 RID: 31940
	[SerializeField]
	private KBatchedAnimController buildingKAnim;
}
