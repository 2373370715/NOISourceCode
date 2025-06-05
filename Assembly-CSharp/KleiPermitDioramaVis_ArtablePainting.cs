using System;
using Database;
using UnityEngine;

// Token: 0x02001DAD RID: 7597
public class KleiPermitDioramaVis_ArtablePainting : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	// Token: 0x06009EBC RID: 40636 RVA: 0x000CEC86 File Offset: 0x000CCE86
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x06009EBD RID: 40637 RVA: 0x0010BBF1 File Offset: 0x00109DF1
	public void ConfigureSetup()
	{
		SymbolOverrideControllerUtil.AddToPrefab(this.buildingKAnim.gameObject);
	}

	// Token: 0x06009EBE RID: 40638 RVA: 0x003DDA60 File Offset: 0x003DBC60
	public void ConfigureWith(PermitResource permit)
	{
		ArtableStage artablePermit = (ArtableStage)permit;
		KleiPermitVisUtil.ConfigureToRenderBuilding(this.buildingKAnim, artablePermit);
		BuildingDef buildingDef = KleiPermitVisUtil.GetBuildingDef(permit);
		this.buildingKAnimPosition.SetOn(this.buildingKAnim);
		this.buildingKAnim.rectTransform().anchoredPosition += new Vector2(0f, -176f * (float)buildingDef.HeightInCells / 2f + 176f);
		this.buildingKAnim.rectTransform().localScale = Vector3.one * 0.9f;
		KleiPermitVisUtil.AnimateIn(this.buildingKAnim, default(Updater));
	}

	// Token: 0x04007CB8 RID: 31928
	[SerializeField]
	private KBatchedAnimController buildingKAnim;

	// Token: 0x04007CB9 RID: 31929
	private PrefabDefinedUIPosition buildingKAnimPosition = new PrefabDefinedUIPosition();
}
