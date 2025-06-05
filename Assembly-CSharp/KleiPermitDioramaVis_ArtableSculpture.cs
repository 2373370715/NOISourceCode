using System;
using Database;
using UnityEngine;

// Token: 0x02001DAE RID: 7598
public class KleiPermitDioramaVis_ArtableSculpture : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	// Token: 0x06009EC0 RID: 40640 RVA: 0x000CEC86 File Offset: 0x000CCE86
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x06009EC1 RID: 40641 RVA: 0x0010BC17 File Offset: 0x00109E17
	public void ConfigureSetup()
	{
		SymbolOverrideControllerUtil.AddToPrefab(this.buildingKAnim.gameObject);
	}

	// Token: 0x06009EC2 RID: 40642 RVA: 0x003DDB0C File Offset: 0x003DBD0C
	public void ConfigureWith(PermitResource permit)
	{
		ArtableStage artablePermit = (ArtableStage)permit;
		KleiPermitVisUtil.ConfigureToRenderBuilding(this.buildingKAnim, artablePermit);
		KleiPermitVisUtil.AnimateIn(this.buildingKAnim, default(Updater));
	}

	// Token: 0x04007CBA RID: 31930
	[SerializeField]
	private KBatchedAnimController buildingKAnim;
}
