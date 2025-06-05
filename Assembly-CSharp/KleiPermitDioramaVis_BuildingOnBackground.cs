using System;
using Database;
using UnityEngine;

// Token: 0x02001DB2 RID: 7602
public class KleiPermitDioramaVis_BuildingOnBackground : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	// Token: 0x06009ED0 RID: 40656 RVA: 0x003DDCCC File Offset: 0x003DBECC
	public void ConfigureSetup()
	{
		this.buildingKAnimPrefab.gameObject.SetActive(false);
		this.buildingKAnimArray = new KBatchedAnimController[9];
		for (int i = 0; i < this.buildingKAnimArray.Length; i++)
		{
			this.buildingKAnimArray[i] = (KBatchedAnimController)UnityEngine.Object.Instantiate(this.buildingKAnimPrefab, this.buildingKAnimPrefab.transform.parent, false);
		}
		Vector2 anchoredPosition = this.buildingKAnimPrefab.rectTransform().anchoredPosition;
		Vector2 a = 175f * Vector2.one;
		Vector2 a2 = anchoredPosition + a * new Vector2(-1f, 0f);
		int num = 0;
		for (int j = 0; j < 3; j++)
		{
			int k = 0;
			while (k < 3)
			{
				this.buildingKAnimArray[num].rectTransform().anchoredPosition = a2 + a * new Vector2((float)j, (float)k);
				this.buildingKAnimArray[num].gameObject.SetActive(true);
				k++;
				num++;
			}
		}
	}

	// Token: 0x06009ED1 RID: 40657 RVA: 0x000CEC86 File Offset: 0x000CCE86
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x06009ED2 RID: 40658 RVA: 0x003DDDD0 File Offset: 0x003DBFD0
	public void ConfigureWith(PermitResource permit)
	{
		BuildingFacadeResource buildingPermit = (BuildingFacadeResource)permit;
		BuildingDef buildingDef = KleiPermitVisUtil.GetBuildingDef(permit);
		DebugUtil.DevAssert(buildingDef.WidthInCells == 1, "assert failed", null);
		DebugUtil.DevAssert(buildingDef.HeightInCells == 1, "assert failed", null);
		KBatchedAnimController[] array = this.buildingKAnimArray;
		for (int i = 0; i < array.Length; i++)
		{
			KleiPermitVisUtil.ConfigureToRenderBuilding(array[i], buildingPermit);
		}
	}

	// Token: 0x04007CC1 RID: 31937
	[SerializeField]
	private KBatchedAnimController buildingKAnimPrefab;

	// Token: 0x04007CC2 RID: 31938
	private KBatchedAnimController[] buildingKAnimArray;
}
