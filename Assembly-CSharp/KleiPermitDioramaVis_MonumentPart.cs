using System;
using Database;
using UnityEngine;

// Token: 0x02001DBC RID: 7612
public class KleiPermitDioramaVis_MonumentPart : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	// Token: 0x06009EFF RID: 40703 RVA: 0x000CEC86 File Offset: 0x000CCE86
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x06009F00 RID: 40704 RVA: 0x000AA038 File Offset: 0x000A8238
	public void ConfigureSetup()
	{
	}

	// Token: 0x06009F01 RID: 40705 RVA: 0x003DE25C File Offset: 0x003DC45C
	public void ConfigureWith(PermitResource permit)
	{
		MonumentPartResource monumentPermit = (MonumentPartResource)permit;
		KleiPermitVisUtil.ConfigureToRenderBuilding(this.buildingKAnim, monumentPermit);
		BuildingDef buildingDef = KleiPermitVisUtil.GetBuildingDef(permit);
		this.buildingKAnimPosition.SetOn(this.buildingKAnim);
		this.buildingKAnim.rectTransform().anchoredPosition += new Vector2(0f, -176f + (float)(buildingDef.HeightInCells * 6));
		this.buildingKAnim.rectTransform().localScale = Vector3.one * 0.55f;
		KleiPermitVisUtil.AnimateIn(this.buildingKAnim, default(Updater));
	}

	// Token: 0x04007CE4 RID: 31972
	[SerializeField]
	private KBatchedAnimController buildingKAnim;

	// Token: 0x04007CE5 RID: 31973
	private PrefabDefinedUIPosition buildingKAnimPosition = new PrefabDefinedUIPosition();
}
