using System;

namespace Database
{
	// Token: 0x020021B3 RID: 8627
	public class OrbitalTypeCategories : ResourceSet<OrbitalData>
	{
		// Token: 0x0600B83E RID: 47166 RVA: 0x0046D9E0 File Offset: 0x0046BBE0
		public OrbitalTypeCategories(ResourceSet parent) : base("OrbitalTypeCategories", parent)
		{
			this.backgroundEarth = new OrbitalData("backgroundEarth", this, "earth_kanim", "", OrbitalData.OrbitalType.world, 1f, 0.5f, 0.95f, 10f, 10f, 1.05f, true, 0.05f, 25f, 1f);
			this.backgroundEarth.GetRenderZ = (() => Grid.GetLayerZ(Grid.SceneLayer.Background) + 0.9f);
			this.frozenOre = new OrbitalData("frozenOre", this, "starmap_frozen_ore_kanim", "", OrbitalData.OrbitalType.poi, 1f, 0.5f, 0.5f, -350f, 350f, 1f, true, 0.05f, 25f, 1f);
			this.heliumCloud = new OrbitalData("heliumCloud", this, "starmap_helium_cloud_kanim", "", OrbitalData.OrbitalType.poi, 1f, 0.5f, 0.5f, -350f, 350f, 1.05f, true, 0.05f, 25f, 1f);
			this.iceCloud = new OrbitalData("iceCloud", this, "starmap_ice_cloud_kanim", "", OrbitalData.OrbitalType.poi, 1f, 0.5f, 0.5f, -350f, 350f, 1.05f, true, 0.05f, 25f, 1f);
			this.iceRock = new OrbitalData("iceRock", this, "starmap_ice_kanim", "", OrbitalData.OrbitalType.poi, 1f, 0.5f, 0.5f, -350f, 350f, 1.05f, true, 0.05f, 25f, 1f);
			this.purpleGas = new OrbitalData("purpleGas", this, "starmap_purple_gas_kanim", "", OrbitalData.OrbitalType.poi, 1f, 0.5f, 0.5f, -350f, 350f, 1.05f, true, 0.05f, 25f, 1f);
			this.radioactiveGas = new OrbitalData("radioactiveGas", this, "starmap_radioactive_gas_kanim", "", OrbitalData.OrbitalType.poi, 1f, 0.5f, 0.5f, -350f, 350f, 1.05f, true, 0.05f, 25f, 1f);
			this.rocky = new OrbitalData("rocky", this, "starmap_rocky_kanim", "", OrbitalData.OrbitalType.poi, 1f, 0.5f, 0.5f, -350f, 350f, 1.05f, true, 0.05f, 25f, 1f);
			this.gravitas = new OrbitalData("gravitas", this, "starmap_space_junk_kanim", "", OrbitalData.OrbitalType.poi, 1f, 0.5f, 0.5f, -350f, 350f, 1.05f, true, 0.05f, 25f, 1f);
			this.orbit = new OrbitalData("orbit", this, "starmap_orbit_kanim", "", OrbitalData.OrbitalType.inOrbit, 1f, 0.25f, 0.5f, -350f, 350f, 1.05f, false, 0.05f, 4f, 1f);
			this.landed = new OrbitalData("landed", this, "starmap_landed_surface_kanim", "", OrbitalData.OrbitalType.landed, 0f, 0.5f, 0.35f, -350f, 350f, 1.05f, false, 0.05f, 4f, 1f);
		}

		// Token: 0x040095CE RID: 38350
		public OrbitalData backgroundEarth;

		// Token: 0x040095CF RID: 38351
		public OrbitalData frozenOre;

		// Token: 0x040095D0 RID: 38352
		public OrbitalData heliumCloud;

		// Token: 0x040095D1 RID: 38353
		public OrbitalData iceCloud;

		// Token: 0x040095D2 RID: 38354
		public OrbitalData iceRock;

		// Token: 0x040095D3 RID: 38355
		public OrbitalData purpleGas;

		// Token: 0x040095D4 RID: 38356
		public OrbitalData radioactiveGas;

		// Token: 0x040095D5 RID: 38357
		public OrbitalData rocky;

		// Token: 0x040095D6 RID: 38358
		public OrbitalData gravitas;

		// Token: 0x040095D7 RID: 38359
		public OrbitalData orbit;

		// Token: 0x040095D8 RID: 38360
		public OrbitalData landed;
	}
}
