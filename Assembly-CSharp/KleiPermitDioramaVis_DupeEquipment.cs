using System;
using Database;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001DB7 RID: 7607
public class KleiPermitDioramaVis_DupeEquipment : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	// Token: 0x06009EE5 RID: 40677 RVA: 0x000CEC86 File Offset: 0x000CCE86
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x06009EE6 RID: 40678 RVA: 0x0010BC63 File Offset: 0x00109E63
	public void ConfigureSetup()
	{
		this.uiMannequin.shouldShowOutfitWithDefaultItems = false;
	}

	// Token: 0x06009EE7 RID: 40679 RVA: 0x003DDF98 File Offset: 0x003DC198
	public void ConfigureWith(PermitResource permit)
	{
		ClothingItemResource clothingItemResource = permit as ClothingItemResource;
		if (clothingItemResource != null)
		{
			this.uiMannequin.SetOutfit(clothingItemResource.outfitType, new ClothingItemResource[]
			{
				clothingItemResource
			});
			this.uiMannequin.ReactToClothingItemChange(clothingItemResource.Category);
		}
		this.dioramaBGImage.sprite = KleiPermitDioramaVis.GetDioramaBackground(permit.Category);
	}

	// Token: 0x04007CCB RID: 31947
	[SerializeField]
	private UIMannequin uiMannequin;

	// Token: 0x04007CCC RID: 31948
	[Header("Diorama Backgrounds")]
	[SerializeField]
	private Image dioramaBGImage;

	// Token: 0x04007CCD RID: 31949
	[SerializeField]
	private Sprite clothingBG;

	// Token: 0x04007CCE RID: 31950
	[SerializeField]
	private Sprite atmosuitBG;
}
