using System;
using System.Collections.Generic;

namespace Database
{
	// Token: 0x020021BD RID: 8637
	public class PermitResources : ResourceSet<PermitResource>
	{
		// Token: 0x0600B858 RID: 47192 RVA: 0x0046E3E8 File Offset: 0x0046C5E8
		public PermitResources(ResourceSet parent) : base("PermitResources", parent)
		{
			this.Root = new ResourceSet<Resource>("Root", null);
			this.Permits = new Dictionary<string, IEnumerable<PermitResource>>();
			this.BuildingFacades = new BuildingFacades(this.Root);
			this.Permits.Add(this.BuildingFacades.Id, this.BuildingFacades.resources);
			this.EquippableFacades = new EquippableFacades(this.Root);
			this.Permits.Add(this.EquippableFacades.Id, this.EquippableFacades.resources);
			this.ArtableStages = new ArtableStages(this.Root);
			this.Permits.Add(this.ArtableStages.Id, this.ArtableStages.resources);
			this.StickerBombs = new StickerBombs(this.Root);
			this.Permits.Add(this.StickerBombs.Id, this.StickerBombs.resources);
			this.ClothingItems = new ClothingItems(this.Root);
			this.ClothingOutfits = new ClothingOutfits(this.Root, this.ClothingItems);
			this.Permits.Add(this.ClothingItems.Id, this.ClothingItems.resources);
			this.BalloonArtistFacades = new BalloonArtistFacades(this.Root);
			this.Permits.Add(this.BalloonArtistFacades.Id, this.BalloonArtistFacades.resources);
			this.MonumentParts = new MonumentParts(this.Root);
			this.Permits.Add(this.MonumentParts.Id, this.MonumentParts.resources);
			foreach (IEnumerable<PermitResource> collection in this.Permits.Values)
			{
				this.resources.AddRange(collection);
			}
		}

		// Token: 0x0600B859 RID: 47193 RVA: 0x0011B6F3 File Offset: 0x001198F3
		public void PostProcess()
		{
			this.BuildingFacades.PostProcess();
		}

		// Token: 0x04009612 RID: 38418
		public ResourceSet Root;

		// Token: 0x04009613 RID: 38419
		public BuildingFacades BuildingFacades;

		// Token: 0x04009614 RID: 38420
		public EquippableFacades EquippableFacades;

		// Token: 0x04009615 RID: 38421
		public ArtableStages ArtableStages;

		// Token: 0x04009616 RID: 38422
		public StickerBombs StickerBombs;

		// Token: 0x04009617 RID: 38423
		public ClothingItems ClothingItems;

		// Token: 0x04009618 RID: 38424
		public ClothingOutfits ClothingOutfits;

		// Token: 0x04009619 RID: 38425
		public MonumentParts MonumentParts;

		// Token: 0x0400961A RID: 38426
		public BalloonArtistFacades BalloonArtistFacades;

		// Token: 0x0400961B RID: 38427
		public Dictionary<string, IEnumerable<PermitResource>> Permits;
	}
}
