﻿using System;
using Klei.AI;

namespace Database
{
	public class GameplaySeasons : ResourceSet<GameplaySeason>
	{
		public GameplaySeasons(ResourceSet parent) : base("GameplaySeasons", parent)
		{
			this.VanillaSeasons();
			this.Expansion1Seasons();
			this.DLCSeasons();
			this.UnusedSeasons();
		}

		private void VanillaSeasons()
		{
			this.MeteorShowers = base.Add(new MeteorShowerSeason("MeteorShowers", GameplaySeason.Type.World, 14f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, -1f, null, null).AddEvent(Db.Get().GameplayEvents.MeteorShowerIronEvent).AddEvent(Db.Get().GameplayEvents.MeteorShowerGoldEvent).AddEvent(Db.Get().GameplayEvents.MeteorShowerCopperEvent));
		}

		private void Expansion1Seasons()
		{
			this.RegolithMoonMeteorShowers = base.Add(new MeteorShowerSeason("RegolithMoonMeteorShowers", GameplaySeason.Type.World, 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f, DlcManager.EXPANSION1, null).AddEvent(Db.Get().GameplayEvents.MeteorShowerDustEvent).AddEvent(Db.Get().GameplayEvents.ClusterIronShower).AddEvent(Db.Get().GameplayEvents.ClusterIceShower));
			this.TemporalTearMeteorShowers = base.Add(new MeteorShowerSeason("TemporalTearMeteorShowers", GameplaySeason.Type.World, 1f, false, 0f, false, -1, 0f, float.PositiveInfinity, 1, false, -1f, DlcManager.EXPANSION1, null).AddEvent(Db.Get().GameplayEvents.MeteorShowerFullereneEvent));
			this.GassyMooteorShowers = base.Add(new MeteorShowerSeason("GassyMooteorShowers", GameplaySeason.Type.World, 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, false, 6000f, DlcManager.EXPANSION1, null).AddEvent(Db.Get().GameplayEvents.GassyMooteorEvent));
			this.SpacedOutStyleStartMeteorShowers = base.Add(new MeteorShowerSeason("SpacedOutStyleStartMeteorShowers", GameplaySeason.Type.World, 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f, DlcManager.EXPANSION1, null));
			this.SpacedOutStyleRocketMeteorShowers = base.Add(new MeteorShowerSeason("SpacedOutStyleRocketMeteorShowers", GameplaySeason.Type.World, 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f, DlcManager.EXPANSION1, null).AddEvent(Db.Get().GameplayEvents.ClusterOxyliteShower));
			this.SpacedOutStyleWarpMeteorShowers = base.Add(new MeteorShowerSeason("SpacedOutStyleWarpMeteorShowers", GameplaySeason.Type.World, 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f, DlcManager.EXPANSION1, null).AddEvent(Db.Get().GameplayEvents.ClusterCopperShower).AddEvent(Db.Get().GameplayEvents.ClusterIceShower).AddEvent(Db.Get().GameplayEvents.ClusterBiologicalShower));
			this.ClassicStyleStartMeteorShowers = base.Add(new MeteorShowerSeason("ClassicStyleStartMeteorShowers", GameplaySeason.Type.World, 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f, DlcManager.EXPANSION1, null).AddEvent(Db.Get().GameplayEvents.ClusterCopperShower).AddEvent(Db.Get().GameplayEvents.ClusterIceShower).AddEvent(Db.Get().GameplayEvents.ClusterBiologicalShower));
			this.ClassicStyleWarpMeteorShowers = base.Add(new MeteorShowerSeason("ClassicStyleWarpMeteorShowers", GameplaySeason.Type.World, 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f, DlcManager.EXPANSION1, null).AddEvent(Db.Get().GameplayEvents.ClusterGoldShower).AddEvent(Db.Get().GameplayEvents.ClusterIronShower));
			this.TundraMoonletMeteorShowers = base.Add(new MeteorShowerSeason("TundraMoonletMeteorShowers", GameplaySeason.Type.World, 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f, DlcManager.EXPANSION1, null));
			this.MarshyMoonletMeteorShowers = base.Add(new MeteorShowerSeason("MarshyMoonletMeteorShowers", GameplaySeason.Type.World, 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f, DlcManager.EXPANSION1, null));
			this.NiobiumMoonletMeteorShowers = base.Add(new MeteorShowerSeason("NiobiumMoonletMeteorShowers", GameplaySeason.Type.World, 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f, DlcManager.EXPANSION1, null));
			this.WaterMoonletMeteorShowers = base.Add(new MeteorShowerSeason("WaterMoonletMeteorShowers", GameplaySeason.Type.World, 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f, DlcManager.EXPANSION1, null));
			this.MiniMetallicSwampyMeteorShowers = base.Add(new MeteorShowerSeason("MiniMetallicSwampyMeteorShowers", GameplaySeason.Type.World, 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f, DlcManager.EXPANSION1, null).AddEvent(Db.Get().GameplayEvents.ClusterBiologicalShower).AddEvent(Db.Get().GameplayEvents.ClusterGoldShower));
			this.MiniForestFrozenMeteorShowers = base.Add(new MeteorShowerSeason("MiniForestFrozenMeteorShowers", GameplaySeason.Type.World, 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f, DlcManager.EXPANSION1, null).AddEvent(Db.Get().GameplayEvents.ClusterOxyliteShower));
			this.MiniBadlandsMeteorShowers = base.Add(new MeteorShowerSeason("MiniBadlandsMeteorShowers", GameplaySeason.Type.World, 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f, DlcManager.EXPANSION1, null).AddEvent(Db.Get().GameplayEvents.ClusterIceShower));
			this.MiniFlippedMeteorShowers = base.Add(new MeteorShowerSeason("MiniFlippedMeteorShowers", GameplaySeason.Type.World, 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f, DlcManager.EXPANSION1, null));
			this.MiniRadioactiveOceanMeteorShowers = base.Add(new MeteorShowerSeason("MiniRadioactiveOceanMeteorShowers", GameplaySeason.Type.World, 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f, DlcManager.EXPANSION1, null).AddEvent(Db.Get().GameplayEvents.ClusterUraniumShower));
		}

		private void DLCSeasons()
		{
			this.CeresMeteorShowers = base.Add(new MeteorShowerSeason("CeresMeteorShowers", GameplaySeason.Type.World, 20f, false, -1f, true, -1, 10f, float.PositiveInfinity, 1, true, 6000f, DlcManager.DLC2, null).AddEvent(Db.Get().GameplayEvents.ClusterIceAndTreesShower));
			this.MiniCeresStartShowers = base.Add(new MeteorShowerSeason("MiniCeresStartShowers", GameplaySeason.Type.World, 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f, DlcManager.EXPANSION1.Append(DlcManager.DLC2), null).AddEvent(Db.Get().GameplayEvents.ClusterOxyliteShower).AddEvent(Db.Get().GameplayEvents.ClusterSnowShower));
			this.LargeImpactor = base.Add(new GameplaySeason("LargeImpactor", GameplaySeason.Type.World, 1f, false, -1f, true, 1, 0f, float.PositiveInfinity, 1, DlcManager.DLC4, null).AddEvent(Db.Get().GameplayEvents.LargeImpactor));
			this.PrehistoricMeteorShowers = base.Add(new MeteorShowerSeason("PrehistoricMeteorShowers", GameplaySeason.Type.World, 50f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f, DlcManager.DLC4, null).AddEvent(Db.Get().GameplayEvents.ClusterCopperShower).AddEvent(Db.Get().GameplayEvents.ClusterIronShower).AddEvent(Db.Get().GameplayEvents.ClusterGoldShower));
		}

		private void UnusedSeasons()
		{
		}

		public GameplaySeason NaturalRandomEvents;

		public GameplaySeason DupeRandomEvents;

		public GameplaySeason PrickleCropSeason;

		public GameplaySeason BonusEvents;

		public GameplaySeason MeteorShowers;

		public GameplaySeason TemporalTearMeteorShowers;

		public GameplaySeason SpacedOutStyleStartMeteorShowers;

		public GameplaySeason SpacedOutStyleRocketMeteorShowers;

		public GameplaySeason SpacedOutStyleWarpMeteorShowers;

		public GameplaySeason ClassicStyleStartMeteorShowers;

		public GameplaySeason ClassicStyleWarpMeteorShowers;

		public GameplaySeason TundraMoonletMeteorShowers;

		public GameplaySeason MarshyMoonletMeteorShowers;

		public GameplaySeason NiobiumMoonletMeteorShowers;

		public GameplaySeason WaterMoonletMeteorShowers;

		public GameplaySeason GassyMooteorShowers;

		public GameplaySeason RegolithMoonMeteorShowers;

		public GameplaySeason MiniMetallicSwampyMeteorShowers;

		public GameplaySeason MiniForestFrozenMeteorShowers;

		public GameplaySeason MiniBadlandsMeteorShowers;

		public GameplaySeason MiniFlippedMeteorShowers;

		public GameplaySeason MiniRadioactiveOceanMeteorShowers;

		public GameplaySeason MiniCeresStartShowers;

		public GameplaySeason CeresMeteorShowers;

		public GameplaySeason LargeImpactor;

		public GameplaySeason PrehistoricMeteorShowers;
	}
}
