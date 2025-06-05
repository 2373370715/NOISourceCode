using System;
using Database;
using UnityEngine;

// Token: 0x02001DB6 RID: 7606
public class KleiPermitDioramaVis_BuildingRocket : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	// Token: 0x06009EE1 RID: 40673 RVA: 0x000CEC86 File Offset: 0x000CCE86
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x06009EE2 RID: 40674 RVA: 0x000AA038 File Offset: 0x000A8238
	public void ConfigureSetup()
	{
	}

	// Token: 0x06009EE3 RID: 40675 RVA: 0x003DDF64 File Offset: 0x003DC164
	public void ConfigureWith(PermitResource permit)
	{
		BuildingFacadeResource buildingPermit = (BuildingFacadeResource)permit;
		KleiPermitVisUtil.ConfigureToRenderBuilding(this.buildingKAnim, buildingPermit);
		KleiPermitVisUtil.AnimateIn(this.buildingKAnim, default(Updater));
	}

	// Token: 0x04007CCA RID: 31946
	[SerializeField]
	private KBatchedAnimController buildingKAnim;
}
