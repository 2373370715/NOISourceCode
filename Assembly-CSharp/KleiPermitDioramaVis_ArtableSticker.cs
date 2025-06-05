using System;
using Database;
using UnityEngine;

// Token: 0x02001DAF RID: 7599
public class KleiPermitDioramaVis_ArtableSticker : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	// Token: 0x06009EC4 RID: 40644 RVA: 0x000CEC86 File Offset: 0x000CCE86
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x06009EC5 RID: 40645 RVA: 0x0010BC2A File Offset: 0x00109E2A
	public void ConfigureSetup()
	{
		SymbolOverrideControllerUtil.AddToPrefab(this.buildingKAnim.gameObject);
	}

	// Token: 0x06009EC6 RID: 40646 RVA: 0x003DDB40 File Offset: 0x003DBD40
	public void ConfigureWith(PermitResource permit)
	{
		DbStickerBomb artablePermit = (DbStickerBomb)permit;
		KleiPermitVisUtil.ConfigureToRenderBuilding(this.buildingKAnim, artablePermit);
	}

	// Token: 0x04007CBB RID: 31931
	[SerializeField]
	private KBatchedAnimController buildingKAnim;
}
