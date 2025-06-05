using System;
using Klei.AI;

namespace Database
{
	// Token: 0x020021AC RID: 8620
	public class GameplaySeasons : ResourceSet<GameplaySeason>
	{
		// Token: 0x0600B80B RID: 47115 RVA: 0x0011B4F3 File Offset: 0x001196F3
		public GameplaySeasons(ResourceSet parent) : base("GameplaySeasons", parent)
		{
			this.VanillaSeasons();
			this.Expansion1Seasons();
			this.DLCSeasons();
			this.UnusedSeasons();
		}

		// Token: 0x0600B80C RID: 47116 RVA: 0x0046BC94 File Offset: 0x00469E94
		private void VanillaSeasons()
		{
			this.MeteorShowers = base.Add(new MeteorShowerSeason("MeteorShowers", GameplaySeason.Type.World, "", 14f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, -1f).AddEvent(Db.Get().GameplayEvents.MeteorShowerIronEvent).AddEvent(Db.Get().GameplayEvents.MeteorShowerGoldEvent).AddEvent(Db.Get().GameplayEvents.MeteorShowerCopperEvent));
		}

		// Token: 0x0600B80D RID: 47117 RVA: 0x0046BD18 File Offset: 0x00469F18
		private void Expansion1Seasons()
		{
			this.RegolithMoonMeteorShowers = base.Add(new MeteorShowerSeason("RegolithMoonMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f).AddEvent(Db.Get().GameplayEvents.MeteorShowerDustEvent).AddEvent(Db.Get().GameplayEvents.ClusterIronShower).AddEvent(Db.Get().GameplayEvents.ClusterIceShower));
			this.TemporalTearMeteorShowers = base.Add(new MeteorShowerSeason("TemporalTearMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 1f, false, 0f, false, -1, 0f, float.PositiveInfinity, 1, false, -1f).AddEvent(Db.Get().GameplayEvents.MeteorShowerFullereneEvent));
			this.GassyMooteorShowers = base.Add(new MeteorShowerSeason("GassyMooteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, false, 6000f).AddEvent(Db.Get().GameplayEvents.GassyMooteorEvent));
			this.SpacedOutStyleStartMeteorShowers = base.Add(new MeteorShowerSeason("SpacedOutStyleStartMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f));
			this.SpacedOutStyleRocketMeteorShowers = base.Add(new MeteorShowerSeason("SpacedOutStyleRocketMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f).AddEvent(Db.Get().GameplayEvents.ClusterOxyliteShower));
			this.SpacedOutStyleWarpMeteorShowers = base.Add(new MeteorShowerSeason("SpacedOutStyleWarpMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f).AddEvent(Db.Get().GameplayEvents.ClusterCopperShower).AddEvent(Db.Get().GameplayEvents.ClusterIceShower).AddEvent(Db.Get().GameplayEvents.ClusterBiologicalShower));
			this.ClassicStyleStartMeteorShowers = base.Add(new MeteorShowerSeason("ClassicStyleStartMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f).AddEvent(Db.Get().GameplayEvents.ClusterCopperShower).AddEvent(Db.Get().GameplayEvents.ClusterIceShower).AddEvent(Db.Get().GameplayEvents.ClusterBiologicalShower));
			this.ClassicStyleWarpMeteorShowers = base.Add(new MeteorShowerSeason("ClassicStyleWarpMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f).AddEvent(Db.Get().GameplayEvents.ClusterGoldShower).AddEvent(Db.Get().GameplayEvents.ClusterIronShower));
			this.TundraMoonletMeteorShowers = base.Add(new MeteorShowerSeason("TundraMoonletMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f));
			this.MarshyMoonletMeteorShowers = base.Add(new MeteorShowerSeason("MarshyMoonletMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f));
			this.NiobiumMoonletMeteorShowers = base.Add(new MeteorShowerSeason("NiobiumMoonletMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f));
			this.WaterMoonletMeteorShowers = base.Add(new MeteorShowerSeason("WaterMoonletMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f));
			this.MiniMetallicSwampyMeteorShowers = base.Add(new MeteorShowerSeason("MiniMetallicSwampyMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f).AddEvent(Db.Get().GameplayEvents.ClusterBiologicalShower).AddEvent(Db.Get().GameplayEvents.ClusterGoldShower));
			this.MiniForestFrozenMeteorShowers = base.Add(new MeteorShowerSeason("MiniForestFrozenMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f).AddEvent(Db.Get().GameplayEvents.ClusterOxyliteShower));
			this.MiniBadlandsMeteorShowers = base.Add(new MeteorShowerSeason("MiniBadlandsMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f).AddEvent(Db.Get().GameplayEvents.ClusterIceShower));
			this.MiniFlippedMeteorShowers = base.Add(new MeteorShowerSeason("MiniFlippedMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f));
			this.MiniRadioactiveOceanMeteorShowers = base.Add(new MeteorShowerSeason("MiniRadioactiveOceanMeteorShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f).AddEvent(Db.Get().GameplayEvents.ClusterUraniumShower));
		}

		// Token: 0x0600B80E RID: 47118 RVA: 0x0046C27C File Offset: 0x0046A47C
		private void DLCSeasons()
		{
			this.CeresMeteorShowers = base.Add(new MeteorShowerSeason("CeresMeteorShowers", GameplaySeason.Type.World, "DLC2_ID", 20f, false, -1f, true, -1, 10f, float.PositiveInfinity, 1, true, 6000f).AddEvent(Db.Get().GameplayEvents.ClusterIceAndTreesShower));
			this.MiniCeresStartShowers = base.Add(new MeteorShowerSeason("MiniCeresStartShowers", GameplaySeason.Type.World, "EXPANSION1_ID", 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f).AddEvent(Db.Get().GameplayEvents.ClusterOxyliteShower).AddEvent(Db.Get().GameplayEvents.ClusterSnowShower));
		}

		// Token: 0x0600B80F RID: 47119 RVA: 0x000AA038 File Offset: 0x000A8238
		private void UnusedSeasons()
		{
		}

		// Token: 0x04009556 RID: 38230
		public GameplaySeason NaturalRandomEvents;

		// Token: 0x04009557 RID: 38231
		public GameplaySeason DupeRandomEvents;

		// Token: 0x04009558 RID: 38232
		public GameplaySeason PrickleCropSeason;

		// Token: 0x04009559 RID: 38233
		public GameplaySeason BonusEvents;

		// Token: 0x0400955A RID: 38234
		public GameplaySeason MeteorShowers;

		// Token: 0x0400955B RID: 38235
		public GameplaySeason TemporalTearMeteorShowers;

		// Token: 0x0400955C RID: 38236
		public GameplaySeason SpacedOutStyleStartMeteorShowers;

		// Token: 0x0400955D RID: 38237
		public GameplaySeason SpacedOutStyleRocketMeteorShowers;

		// Token: 0x0400955E RID: 38238
		public GameplaySeason SpacedOutStyleWarpMeteorShowers;

		// Token: 0x0400955F RID: 38239
		public GameplaySeason ClassicStyleStartMeteorShowers;

		// Token: 0x04009560 RID: 38240
		public GameplaySeason ClassicStyleWarpMeteorShowers;

		// Token: 0x04009561 RID: 38241
		public GameplaySeason TundraMoonletMeteorShowers;

		// Token: 0x04009562 RID: 38242
		public GameplaySeason MarshyMoonletMeteorShowers;

		// Token: 0x04009563 RID: 38243
		public GameplaySeason NiobiumMoonletMeteorShowers;

		// Token: 0x04009564 RID: 38244
		public GameplaySeason WaterMoonletMeteorShowers;

		// Token: 0x04009565 RID: 38245
		public GameplaySeason GassyMooteorShowers;

		// Token: 0x04009566 RID: 38246
		public GameplaySeason RegolithMoonMeteorShowers;

		// Token: 0x04009567 RID: 38247
		public GameplaySeason MiniMetallicSwampyMeteorShowers;

		// Token: 0x04009568 RID: 38248
		public GameplaySeason MiniForestFrozenMeteorShowers;

		// Token: 0x04009569 RID: 38249
		public GameplaySeason MiniBadlandsMeteorShowers;

		// Token: 0x0400956A RID: 38250
		public GameplaySeason MiniFlippedMeteorShowers;

		// Token: 0x0400956B RID: 38251
		public GameplaySeason MiniRadioactiveOceanMeteorShowers;

		// Token: 0x0400956C RID: 38252
		public GameplaySeason MiniCeresStartShowers;

		// Token: 0x0400956D RID: 38253
		public GameplaySeason CeresMeteorShowers;
	}
}
