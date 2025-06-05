using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Database;
using UnityEngine;

// Token: 0x02000995 RID: 2453
public class BlueprintCollection
{
	// Token: 0x06002BB6 RID: 11190 RVA: 0x000C0ECB File Offset: 0x000BF0CB
	public void AddBlueprintsFrom<T>(T provider) where T : BlueprintProvider
	{
		provider.blueprintCollection = this;
		provider.Internal_PreSetupBlueprints();
		provider.SetupBlueprints();
	}

	// Token: 0x06002BB7 RID: 11191 RVA: 0x001ED938 File Offset: 0x001EBB38
	public void AddBlueprintsFrom(BlueprintCollection collection)
	{
		this.artables.AddRange(collection.artables);
		this.buildingFacades.AddRange(collection.buildingFacades);
		this.clothingItems.AddRange(collection.clothingItems);
		this.balloonArtistFacades.AddRange(collection.balloonArtistFacades);
		this.stickerBombFacades.AddRange(collection.stickerBombFacades);
		this.equippableFacades.AddRange(collection.equippableFacades);
		this.monumentParts.AddRange(collection.monumentParts);
		this.outfits.AddRange(collection.outfits);
	}

	// Token: 0x06002BB8 RID: 11192 RVA: 0x001ED9D0 File Offset: 0x001EBBD0
	public void PostProcess()
	{
		if (Application.isPlaying)
		{
			this.artables.RemoveAll(new Predicate<ArtableInfo>(BlueprintCollection.<PostProcess>g__ShouldExcludeBlueprint|10_0));
			this.buildingFacades.RemoveAll(new Predicate<BuildingFacadeInfo>(BlueprintCollection.<PostProcess>g__ShouldExcludeBlueprint|10_0));
			this.clothingItems.RemoveAll(new Predicate<ClothingItemInfo>(BlueprintCollection.<PostProcess>g__ShouldExcludeBlueprint|10_0));
			this.balloonArtistFacades.RemoveAll(new Predicate<BalloonArtistFacadeInfo>(BlueprintCollection.<PostProcess>g__ShouldExcludeBlueprint|10_0));
			this.stickerBombFacades.RemoveAll(new Predicate<StickerBombFacadeInfo>(BlueprintCollection.<PostProcess>g__ShouldExcludeBlueprint|10_0));
			this.equippableFacades.RemoveAll(new Predicate<EquippableFacadeInfo>(BlueprintCollection.<PostProcess>g__ShouldExcludeBlueprint|10_0));
			this.monumentParts.RemoveAll(new Predicate<MonumentPartInfo>(BlueprintCollection.<PostProcess>g__ShouldExcludeBlueprint|10_0));
			this.outfits.RemoveAll(new Predicate<ClothingOutfitResource>(BlueprintCollection.<PostProcess>g__ShouldExcludeBlueprint|10_0));
		}
	}

	// Token: 0x06002BBA RID: 11194 RVA: 0x001EDB14 File Offset: 0x001EBD14
	[CompilerGenerated]
	internal static bool <PostProcess>g__ShouldExcludeBlueprint|10_0(IHasDlcRestrictions blueprintDlcInfo)
	{
		if (!DlcManager.IsCorrectDlcSubscribed(blueprintDlcInfo))
		{
			return true;
		}
		IBlueprintInfo blueprintInfo = blueprintDlcInfo as IBlueprintInfo;
		KAnimFile kanimFile;
		if (blueprintInfo != null && !Assets.TryGetAnim(blueprintInfo.animFile, out kanimFile))
		{
			DebugUtil.DevAssert(false, string.Concat(new string[]
			{
				"Couldnt find anim \"",
				blueprintInfo.animFile,
				"\" for blueprint \"",
				blueprintInfo.id,
				"\""
			}), null);
		}
		return false;
	}

	// Token: 0x04001DEF RID: 7663
	public List<ArtableInfo> artables = new List<ArtableInfo>();

	// Token: 0x04001DF0 RID: 7664
	public List<BuildingFacadeInfo> buildingFacades = new List<BuildingFacadeInfo>();

	// Token: 0x04001DF1 RID: 7665
	public List<ClothingItemInfo> clothingItems = new List<ClothingItemInfo>();

	// Token: 0x04001DF2 RID: 7666
	public List<BalloonArtistFacadeInfo> balloonArtistFacades = new List<BalloonArtistFacadeInfo>();

	// Token: 0x04001DF3 RID: 7667
	public List<StickerBombFacadeInfo> stickerBombFacades = new List<StickerBombFacadeInfo>();

	// Token: 0x04001DF4 RID: 7668
	public List<EquippableFacadeInfo> equippableFacades = new List<EquippableFacadeInfo>();

	// Token: 0x04001DF5 RID: 7669
	public List<MonumentPartInfo> monumentParts = new List<MonumentPartInfo>();

	// Token: 0x04001DF6 RID: 7670
	public List<ClothingOutfitResource> outfits = new List<ClothingOutfitResource>();
}
