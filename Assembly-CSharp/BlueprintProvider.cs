using System;
using Database;

// Token: 0x0200099E RID: 2462
public abstract class BlueprintProvider : IHasDlcRestrictions
{
	// Token: 0x06002C20 RID: 11296 RVA: 0x001EDE3C File Offset: 0x001EC03C
	protected void AddBuilding(string prefabConfigId, PermitRarity rarity, string permitId, string animFile)
	{
		this.blueprintCollection.buildingFacades.Add(new BuildingFacadeInfo(permitId, Strings.Get("STRINGS.BLUEPRINTS." + permitId.ToUpper() + ".NAME"), Strings.Get("STRINGS.BLUEPRINTS." + permitId.ToUpper() + ".DESC"), rarity, prefabConfigId, animFile, null, this.requiredDlcIds, this.forbiddenDlcIds));
	}

	// Token: 0x06002C21 RID: 11297 RVA: 0x001EDEB0 File Offset: 0x001EC0B0
	protected void AddClothing(BlueprintProvider.ClothingType clothingType, PermitRarity rarity, string permitId, string animFile)
	{
		this.blueprintCollection.clothingItems.Add(new ClothingItemInfo(permitId, Strings.Get("STRINGS.BLUEPRINTS." + permitId.ToUpper() + ".NAME"), Strings.Get("STRINGS.BLUEPRINTS." + permitId.ToUpper() + ".DESC"), (PermitCategory)clothingType, rarity, animFile, this.requiredDlcIds, this.forbiddenDlcIds));
	}

	// Token: 0x06002C22 RID: 11298 RVA: 0x001EDF24 File Offset: 0x001EC124
	protected BlueprintProvider.ArtableInfoAuthoringHelper AddArtable(BlueprintProvider.ArtableType artableType, PermitRarity rarity, string permitId, string animFile)
	{
		string text;
		switch (artableType)
		{
		case BlueprintProvider.ArtableType.Painting:
			text = "Canvas";
			break;
		case BlueprintProvider.ArtableType.PaintingTall:
			text = "CanvasTall";
			break;
		case BlueprintProvider.ArtableType.PaintingWide:
			text = "CanvasWide";
			break;
		case BlueprintProvider.ArtableType.Sculpture:
			text = "Sculpture";
			break;
		case BlueprintProvider.ArtableType.SculptureSmall:
			text = "SmallSculpture";
			break;
		case BlueprintProvider.ArtableType.SculptureIce:
			text = "IceSculpture";
			break;
		case BlueprintProvider.ArtableType.SculptureMetal:
			text = "MetalSculpture";
			break;
		case BlueprintProvider.ArtableType.SculptureMarble:
			text = "MarbleSculpture";
			break;
		case BlueprintProvider.ArtableType.SculptureWood:
			text = "WoodSculpture";
			break;
		default:
			text = null;
			break;
		}
		bool flag = true;
		if (text == null)
		{
			DebugUtil.DevAssert(false, "Failed to get buildingConfigId from " + artableType.ToString(), null);
			flag = false;
		}
		BlueprintProvider.ArtableInfoAuthoringHelper result;
		if (flag)
		{
			KAnimFile kanimFile;
			ArtableInfo artableInfo = new ArtableInfo(permitId, Strings.Get("STRINGS.BLUEPRINTS." + permitId.ToUpper() + ".NAME"), Strings.Get("STRINGS.BLUEPRINTS." + permitId.ToUpper() + ".DESC"), rarity, animFile, (!Assets.TryGetAnim(animFile, out kanimFile)) ? null : kanimFile.GetData().GetAnim(0).name, 0, false, "error", text, "", this.requiredDlcIds, this.forbiddenDlcIds);
			result = new BlueprintProvider.ArtableInfoAuthoringHelper(artableType, artableInfo);
			result.Quality(BlueprintProvider.ArtableQuality.LookingGreat);
			this.blueprintCollection.artables.Add(artableInfo);
		}
		else
		{
			result = default(BlueprintProvider.ArtableInfoAuthoringHelper);
		}
		return result;
	}

	// Token: 0x06002C23 RID: 11299 RVA: 0x001EE088 File Offset: 0x001EC288
	protected void AddJoyResponse(BlueprintProvider.JoyResponseType joyResponseType, PermitRarity rarity, string permitId, string animFile)
	{
		if (joyResponseType == BlueprintProvider.JoyResponseType.BallonSet)
		{
			this.blueprintCollection.balloonArtistFacades.Add(new BalloonArtistFacadeInfo(permitId, Strings.Get("STRINGS.BLUEPRINTS." + permitId.ToUpper() + ".NAME"), Strings.Get("STRINGS.BLUEPRINTS." + permitId.ToUpper() + ".DESC"), rarity, animFile, BalloonArtistFacadeType.ThreeSet, this.requiredDlcIds, this.forbiddenDlcIds));
			return;
		}
		throw new NotImplementedException("Missing case for " + joyResponseType.ToString());
	}

	// Token: 0x06002C24 RID: 11300 RVA: 0x001EE11C File Offset: 0x001EC31C
	protected void AddOutfit(BlueprintProvider.OutfitType outfitType, string outfitId, string[] permitIdList)
	{
		this.blueprintCollection.outfits.Add(new ClothingOutfitResource(outfitId, permitIdList, Strings.Get("STRINGS.BLUEPRINTS." + outfitId.ToUpper() + ".NAME"), (ClothingOutfitUtility.OutfitType)outfitType, this.requiredDlcIds, this.forbiddenDlcIds));
	}

	// Token: 0x06002C25 RID: 11301 RVA: 0x001EE16C File Offset: 0x001EC36C
	protected void AddMonumentPart(BlueprintProvider.MonumentPart part, PermitRarity rarity, string permitId, string animFile)
	{
		string symbolName = "";
		switch (part)
		{
		case BlueprintProvider.MonumentPart.Bottom:
			symbolName = "base";
			break;
		case BlueprintProvider.MonumentPart.Middle:
			symbolName = "mid";
			break;
		case BlueprintProvider.MonumentPart.Top:
			symbolName = "top";
			break;
		}
		this.blueprintCollection.monumentParts.Add(new MonumentPartInfo(permitId, Strings.Get("STRINGS.BLUEPRINTS." + permitId.ToUpper() + ".NAME"), Strings.Get("STRINGS.BLUEPRINTS." + permitId.ToUpper() + ".DESC"), rarity, animFile, permitId.Replace("permit_", ""), symbolName, (MonumentPartResource.Part)part, this.requiredDlcIds, this.forbiddenDlcIds));
	}

	// Token: 0x06002C26 RID: 11302 RVA: 0x000C11B2 File Offset: 0x000BF3B2
	public virtual string[] GetRequiredDlcIds()
	{
		return this.requiredDlcIds;
	}

	// Token: 0x06002C27 RID: 11303 RVA: 0x000C11BA File Offset: 0x000BF3BA
	public virtual string[] GetForbiddenDlcIds()
	{
		return this.forbiddenDlcIds;
	}

	// Token: 0x06002C28 RID: 11304
	public abstract void SetupBlueprints();

	// Token: 0x06002C29 RID: 11305 RVA: 0x000C11C2 File Offset: 0x000BF3C2
	public void Internal_PreSetupBlueprints()
	{
		this.requiredDlcIds = this.GetRequiredDlcIds();
		this.forbiddenDlcIds = this.GetForbiddenDlcIds();
	}

	// Token: 0x04001E39 RID: 7737
	public BlueprintCollection blueprintCollection;

	// Token: 0x04001E3A RID: 7738
	private string[] requiredDlcIds;

	// Token: 0x04001E3B RID: 7739
	private string[] forbiddenDlcIds;

	// Token: 0x0200099F RID: 2463
	public enum ArtableType
	{
		// Token: 0x04001E3D RID: 7741
		Painting,
		// Token: 0x04001E3E RID: 7742
		PaintingTall,
		// Token: 0x04001E3F RID: 7743
		PaintingWide,
		// Token: 0x04001E40 RID: 7744
		Sculpture,
		// Token: 0x04001E41 RID: 7745
		SculptureSmall,
		// Token: 0x04001E42 RID: 7746
		SculptureIce,
		// Token: 0x04001E43 RID: 7747
		SculptureMetal,
		// Token: 0x04001E44 RID: 7748
		SculptureMarble,
		// Token: 0x04001E45 RID: 7749
		SculptureWood
	}

	// Token: 0x020009A0 RID: 2464
	public enum ArtableQuality
	{
		// Token: 0x04001E47 RID: 7751
		LookingGreat,
		// Token: 0x04001E48 RID: 7752
		LookingOkay,
		// Token: 0x04001E49 RID: 7753
		LookingUgly
	}

	// Token: 0x020009A1 RID: 2465
	public enum ClothingType
	{
		// Token: 0x04001E4B RID: 7755
		DupeTops = 1,
		// Token: 0x04001E4C RID: 7756
		DupeBottoms,
		// Token: 0x04001E4D RID: 7757
		DupeGloves,
		// Token: 0x04001E4E RID: 7758
		DupeShoes,
		// Token: 0x04001E4F RID: 7759
		DupeHats,
		// Token: 0x04001E50 RID: 7760
		DupeAccessories,
		// Token: 0x04001E51 RID: 7761
		AtmoSuitHelmet,
		// Token: 0x04001E52 RID: 7762
		AtmoSuitBody,
		// Token: 0x04001E53 RID: 7763
		AtmoSuitGloves,
		// Token: 0x04001E54 RID: 7764
		AtmoSuitBelt,
		// Token: 0x04001E55 RID: 7765
		AtmoSuitShoes
	}

	// Token: 0x020009A2 RID: 2466
	public enum OutfitType
	{
		// Token: 0x04001E57 RID: 7767
		Clothing,
		// Token: 0x04001E58 RID: 7768
		AtmoSuit = 2
	}

	// Token: 0x020009A3 RID: 2467
	public enum JoyResponseType
	{
		// Token: 0x04001E5A RID: 7770
		BallonSet
	}

	// Token: 0x020009A4 RID: 2468
	public enum MonumentPart
	{
		// Token: 0x04001E5C RID: 7772
		Bottom,
		// Token: 0x04001E5D RID: 7773
		Top = 2,
		// Token: 0x04001E5E RID: 7774
		Middle = 1
	}

	// Token: 0x020009A5 RID: 2469
	protected readonly ref struct ArtableInfoAuthoringHelper
	{
		// Token: 0x06002C2B RID: 11307 RVA: 0x000C11DC File Offset: 0x000BF3DC
		public ArtableInfoAuthoringHelper(BlueprintProvider.ArtableType artableType, ArtableInfo artableInfo)
		{
			this.artableType = artableType;
			this.artableInfo = artableInfo;
		}

		// Token: 0x06002C2C RID: 11308 RVA: 0x001EE220 File Offset: 0x001EC420
		public void Quality(BlueprintProvider.ArtableQuality artableQuality)
		{
			if (this.artableInfo == null)
			{
				return;
			}
			int num;
			int num2;
			int num3;
			if (this.artableType == BlueprintProvider.ArtableType.SculptureWood)
			{
				num = 4;
				num2 = 8;
				num3 = 12;
			}
			else
			{
				num = 5;
				num2 = 10;
				num3 = 15;
			}
			int decor_value;
			bool cheer_on_complete;
			string status_id;
			switch (artableQuality)
			{
			case BlueprintProvider.ArtableQuality.LookingGreat:
				decor_value = num3;
				cheer_on_complete = true;
				status_id = "LookingGreat";
				break;
			case BlueprintProvider.ArtableQuality.LookingOkay:
				decor_value = num2;
				cheer_on_complete = false;
				status_id = "LookingOkay";
				break;
			case BlueprintProvider.ArtableQuality.LookingUgly:
				decor_value = num;
				cheer_on_complete = false;
				status_id = "LookingUgly";
				break;
			default:
				throw new ArgumentException();
			}
			this.artableInfo.decor_value = decor_value;
			this.artableInfo.cheer_on_complete = cheer_on_complete;
			this.artableInfo.status_id = status_id;
		}

		// Token: 0x04001E5F RID: 7775
		private readonly BlueprintProvider.ArtableType artableType;

		// Token: 0x04001E60 RID: 7776
		private readonly ArtableInfo artableInfo;
	}
}
