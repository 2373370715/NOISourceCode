using System;
using Database;
using UnityEngine;

// Token: 0x02001DB5 RID: 7605
public class KleiPermitDioramaVis_BuildingPresentationStand : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	// Token: 0x06009EDC RID: 40668 RVA: 0x000CEC86 File Offset: 0x000CCE86
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x06009EDD RID: 40669 RVA: 0x000AA038 File Offset: 0x000A8238
	public void ConfigureSetup()
	{
	}

	// Token: 0x06009EDE RID: 40670 RVA: 0x003DDE98 File Offset: 0x003DC098
	public void ConfigureWith(PermitResource permit)
	{
		BuildingFacadeResource buildingPermit = (BuildingFacadeResource)permit;
		KleiPermitVisUtil.ConfigureToRenderBuilding(this.buildingKAnim, buildingPermit);
		KleiPermitVisUtil.ConfigureBuildingPosition(this.buildingKAnim.rectTransform(), this.anchorPos, KleiPermitVisUtil.GetBuildingDef(permit), this.lastAlignment);
		KleiPermitVisUtil.AnimateIn(this.buildingKAnim, default(Updater));
	}

	// Token: 0x06009EDF RID: 40671 RVA: 0x003DDEF0 File Offset: 0x003DC0F0
	public KleiPermitDioramaVis_BuildingPresentationStand WithAlignment(Alignment alignment)
	{
		this.lastAlignment = alignment;
		this.anchorPos = new Vector2(alignment.x.Remap(new ValueTuple<float, float>(0f, 1f), new ValueTuple<float, float>(-160f, 160f)), alignment.y.Remap(new ValueTuple<float, float>(0f, 1f), new ValueTuple<float, float>(-156f, 156f)));
		return this;
	}

	// Token: 0x04007CC5 RID: 31941
	[SerializeField]
	private KBatchedAnimController buildingKAnim;

	// Token: 0x04007CC6 RID: 31942
	private Alignment lastAlignment;

	// Token: 0x04007CC7 RID: 31943
	private Vector2 anchorPos;

	// Token: 0x04007CC8 RID: 31944
	public const float LEFT = -160f;

	// Token: 0x04007CC9 RID: 31945
	public const float TOP = 156f;
}
