using System;
using Database;
using UnityEngine;

// Token: 0x02001DB3 RID: 7603
public class KleiPermitDioramaVis_BuildingOnFloor : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	// Token: 0x06009ED4 RID: 40660 RVA: 0x000CEC86 File Offset: 0x000CCE86
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x06009ED5 RID: 40661 RVA: 0x000AA038 File Offset: 0x000A8238
	public void ConfigureSetup()
	{
	}

	// Token: 0x06009ED6 RID: 40662 RVA: 0x003DDE30 File Offset: 0x003DC030
	public void ConfigureWith(PermitResource permit)
	{
		BuildingFacadeResource buildingPermit = (BuildingFacadeResource)permit;
		KleiPermitVisUtil.ConfigureToRenderBuilding(this.buildingKAnim, buildingPermit);
		KleiPermitVisUtil.AnimateIn(this.buildingKAnim, default(Updater));
	}

	// Token: 0x04007CC3 RID: 31939
	[SerializeField]
	private KBatchedAnimController buildingKAnim;
}
