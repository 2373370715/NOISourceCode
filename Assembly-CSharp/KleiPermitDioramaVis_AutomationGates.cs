using System;
using System.Collections.Generic;
using Database;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001DB0 RID: 7600
public class KleiPermitDioramaVis_AutomationGates : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	// Token: 0x06009EC8 RID: 40648 RVA: 0x000CEC86 File Offset: 0x000CCE86
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x06009EC9 RID: 40649 RVA: 0x000AA038 File Offset: 0x000A8238
	public void ConfigureSetup()
	{
	}

	// Token: 0x06009ECA RID: 40650 RVA: 0x003DDB60 File Offset: 0x003DBD60
	public void ConfigureWith(PermitResource permit)
	{
		this.itemSprite.gameObject.SetActive(false);
		BuildingFacadeResource buildingPermit = (BuildingFacadeResource)permit;
		KleiPermitVisUtil.ConfigureToRenderBuilding(this.buildingKAnim, buildingPermit);
		BuildingDef buildingDef = KleiPermitVisUtil.GetBuildingDef(permit);
		Dictionary<int, float> dictionary = new Dictionary<int, float>
		{
			{
				3,
				0.7f
			},
			{
				2,
				0.9f
			},
			{
				1,
				0.85f
			}
		};
		Dictionary<int, float> dictionary2 = new Dictionary<int, float>
		{
			{
				4,
				32f
			},
			{
				3,
				32f
			},
			{
				2,
				32f
			},
			{
				1,
				96f
			}
		};
		this.buildingKAnimPosition.SetOn(this.buildingKAnim);
		this.buildingKAnim.rectTransform().localScale = Vector3.one * dictionary[buildingDef.WidthInCells];
		this.buildingKAnim.rectTransform().anchoredPosition += new Vector2(0f, dictionary2[buildingDef.HeightInCells]);
		KleiPermitVisUtil.AnimateIn(this.buildingKAnim, default(Updater));
	}

	// Token: 0x04007CBC RID: 31932
	[SerializeField]
	private Image itemSprite;

	// Token: 0x04007CBD RID: 31933
	[SerializeField]
	private KBatchedAnimController buildingKAnim;

	// Token: 0x04007CBE RID: 31934
	private PrefabDefinedUIPosition buildingKAnimPosition = new PrefabDefinedUIPosition();
}
