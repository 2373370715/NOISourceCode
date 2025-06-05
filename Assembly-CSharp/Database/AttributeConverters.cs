using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;

namespace Database
{
	// Token: 0x02002182 RID: 8578
	public class AttributeConverters : ResourceSet<AttributeConverter>
	{
		// Token: 0x0600B695 RID: 46741 RVA: 0x00457A14 File Offset: 0x00455C14
		public AttributeConverter Create(string id, string name, string description, Klei.AI.Attribute attribute, float multiplier, float base_value, IAttributeFormatter formatter, string[] requiredDlcIds = null, string[] forbiddenDlcIds = null)
		{
			AttributeConverter attributeConverter = new AttributeConverter(id, name, description, multiplier, base_value, attribute, formatter);
			if (DlcManager.IsCorrectDlcSubscribed(requiredDlcIds, forbiddenDlcIds))
			{
				base.Add(attributeConverter);
				attribute.converters.Add(attributeConverter);
			}
			return attributeConverter;
		}

		// Token: 0x0600B696 RID: 46742 RVA: 0x00457A54 File Offset: 0x00455C54
		public AttributeConverters()
		{
			ToPercentAttributeFormatter formatter = new ToPercentAttributeFormatter(1f, GameUtil.TimeSlice.None);
			StandardAttributeFormatter formatter2 = new StandardAttributeFormatter(GameUtil.UnitClass.Mass, GameUtil.TimeSlice.None);
			this.MovementSpeed = this.Create("MovementSpeed", "Movement Speed", DUPLICANTS.ATTRIBUTES.ATHLETICS.SPEEDMODIFIER, Db.Get().Attributes.Athletics, 0.1f, 0f, formatter, null, null);
			this.ConstructionSpeed = this.Create("ConstructionSpeed", "Construction Speed", DUPLICANTS.ATTRIBUTES.CONSTRUCTION.SPEEDMODIFIER, Db.Get().Attributes.Construction, 0.25f, 0f, formatter, null, null);
			this.DiggingSpeed = this.Create("DiggingSpeed", "Digging Speed", DUPLICANTS.ATTRIBUTES.DIGGING.SPEEDMODIFIER, Db.Get().Attributes.Digging, 0.25f, 0f, formatter, null, null);
			this.MachinerySpeed = this.Create("MachinerySpeed", "Machinery Speed", DUPLICANTS.ATTRIBUTES.MACHINERY.SPEEDMODIFIER, Db.Get().Attributes.Machinery, 0.1f, 0f, formatter, null, null);
			this.HarvestSpeed = this.Create("HarvestSpeed", "Harvest Speed", DUPLICANTS.ATTRIBUTES.BOTANIST.HARVEST_SPEED_MODIFIER, Db.Get().Attributes.Botanist, 0.05f, 0f, formatter, null, null);
			this.PlantTendSpeed = this.Create("PlantTendSpeed", "Plant Tend Speed", DUPLICANTS.ATTRIBUTES.BOTANIST.TINKER_MODIFIER, Db.Get().Attributes.Botanist, 0.025f, 0f, formatter, null, null);
			this.CompoundingSpeed = this.Create("CompoundingSpeed", "Compounding Speed", DUPLICANTS.ATTRIBUTES.CARING.FABRICATE_SPEEDMODIFIER, Db.Get().Attributes.Caring, 0.1f, 0f, formatter, null, null);
			this.ResearchSpeed = this.Create("ResearchSpeed", "Research Speed", DUPLICANTS.ATTRIBUTES.LEARNING.RESEARCHSPEED, Db.Get().Attributes.Learning, 0.4f, 0f, formatter, null, null);
			this.TrainingSpeed = this.Create("TrainingSpeed", "Training Speed", DUPLICANTS.ATTRIBUTES.LEARNING.SPEEDMODIFIER, Db.Get().Attributes.Learning, 0.1f, 0f, formatter, null, null);
			this.CookingSpeed = this.Create("CookingSpeed", "Cooking Speed", DUPLICANTS.ATTRIBUTES.COOKING.SPEEDMODIFIER, Db.Get().Attributes.Cooking, 0.05f, 0f, formatter, null, null);
			this.ArtSpeed = this.Create("ArtSpeed", "Art Speed", DUPLICANTS.ATTRIBUTES.ART.SPEEDMODIFIER, Db.Get().Attributes.Art, 0.1f, 0f, formatter, null, null);
			this.DoctorSpeed = this.Create("DoctorSpeed", "Doctor Speed", DUPLICANTS.ATTRIBUTES.CARING.SPEEDMODIFIER, Db.Get().Attributes.Caring, 0.2f, 0f, formatter, null, null);
			this.TidyingSpeed = this.Create("TidyingSpeed", "Tidying Speed", DUPLICANTS.ATTRIBUTES.STRENGTH.SPEEDMODIFIER, Db.Get().Attributes.Strength, 0.25f, 0f, formatter, null, null);
			this.AttackDamage = this.Create("AttackDamage", "Attack Damage", DUPLICANTS.ATTRIBUTES.DIGGING.ATTACK_MODIFIER, Db.Get().Attributes.Digging, 0.05f, 0f, formatter, null, null);
			this.PilotingSpeed = this.Create("PilotingSpeed", "Piloting Speed", DUPLICANTS.ATTRIBUTES.SPACENAVIGATION.SPEED_MODIFIER, Db.Get().Attributes.SpaceNavigation, 0.025f, 0f, formatter, DlcManager.EXPANSION1, null);
			this.ImmuneLevelBoost = this.Create("ImmuneLevelBoost", "Immune Level Boost", DUPLICANTS.ATTRIBUTES.IMMUNITY.BOOST_MODIFIER, Db.Get().Attributes.Immunity, 0.0016666667f, 0f, new ToPercentAttributeFormatter(100f, GameUtil.TimeSlice.PerCycle), null, null);
			this.ToiletSpeed = this.Create("ToiletSpeed", "Toilet Speed", "", Db.Get().Attributes.ToiletEfficiency, 1f, -1f, formatter, null, null);
			this.CarryAmountFromStrength = this.Create("CarryAmountFromStrength", "Carry Amount", DUPLICANTS.ATTRIBUTES.STRENGTH.CARRYMODIFIER, Db.Get().Attributes.Strength, 40f, 0f, formatter2, null, null);
			this.TemperatureInsulation = this.Create("TemperatureInsulation", "Temperature Insulation", DUPLICANTS.ATTRIBUTES.INSULATION.SPEEDMODIFIER, Db.Get().Attributes.Insulation, 0.1f, 0f, formatter, null, null);
			this.SeedHarvestChance = this.Create("SeedHarvestChance", "Seed Harvest Chance", DUPLICANTS.ATTRIBUTES.BOTANIST.BONUS_SEEDS, Db.Get().Attributes.Botanist, 0.033f, 0f, formatter, null, null);
			this.CapturableSpeed = this.Create("CapturableSpeed", "Capturable Speed", DUPLICANTS.ATTRIBUTES.RANCHING.CAPTURABLESPEED, Db.Get().Attributes.Ranching, 0.05f, 0f, formatter, null, null);
			this.GeotuningSpeed = this.Create("GeotuningSpeed", "Geotuning Speed", DUPLICANTS.ATTRIBUTES.LEARNING.GEOTUNER_SPEED_MODIFIER, Db.Get().Attributes.Learning, 0.05f, 0f, formatter, null, null);
			this.RanchingEffectDuration = this.Create("RanchingEffectDuration", "Ranching Effect Duration", DUPLICANTS.ATTRIBUTES.RANCHING.EFFECTMODIFIER, Db.Get().Attributes.Ranching, 0.1f, 0f, formatter, null, null);
			this.FarmedEffectDuration = this.Create("FarmedEffectDuration", "Farmer's Touch Duration", DUPLICANTS.ATTRIBUTES.BOTANIST.TINKER_EFFECT_MODIFIER, Db.Get().Attributes.Botanist, 0.1f, 0f, formatter, null, null);
			this.PowerTinkerEffectDuration = this.Create("PowerTinkerEffectDuration", "Engie's Tune-Up Effect Duration", DUPLICANTS.ATTRIBUTES.MACHINERY.TINKER_EFFECT_MODIFIER, Db.Get().Attributes.Machinery, 0.025f, 0f, formatter, null, null);
		}

		// Token: 0x0600B697 RID: 46743 RVA: 0x00458060 File Offset: 0x00456260
		public List<AttributeConverter> GetConvertersForAttribute(Klei.AI.Attribute attrib)
		{
			List<AttributeConverter> list = new List<AttributeConverter>();
			foreach (AttributeConverter attributeConverter in this.resources)
			{
				if (attributeConverter.attribute == attrib)
				{
					list.Add(attributeConverter);
				}
			}
			return list;
		}

		// Token: 0x0400907F RID: 36991
		public AttributeConverter MovementSpeed;

		// Token: 0x04009080 RID: 36992
		public AttributeConverter ConstructionSpeed;

		// Token: 0x04009081 RID: 36993
		public AttributeConverter DiggingSpeed;

		// Token: 0x04009082 RID: 36994
		public AttributeConverter MachinerySpeed;

		// Token: 0x04009083 RID: 36995
		public AttributeConverter HarvestSpeed;

		// Token: 0x04009084 RID: 36996
		public AttributeConverter PlantTendSpeed;

		// Token: 0x04009085 RID: 36997
		public AttributeConverter CompoundingSpeed;

		// Token: 0x04009086 RID: 36998
		public AttributeConverter ResearchSpeed;

		// Token: 0x04009087 RID: 36999
		public AttributeConverter TrainingSpeed;

		// Token: 0x04009088 RID: 37000
		public AttributeConverter CookingSpeed;

		// Token: 0x04009089 RID: 37001
		public AttributeConverter ArtSpeed;

		// Token: 0x0400908A RID: 37002
		public AttributeConverter DoctorSpeed;

		// Token: 0x0400908B RID: 37003
		public AttributeConverter TidyingSpeed;

		// Token: 0x0400908C RID: 37004
		public AttributeConverter AttackDamage;

		// Token: 0x0400908D RID: 37005
		public AttributeConverter PilotingSpeed;

		// Token: 0x0400908E RID: 37006
		public AttributeConverter ImmuneLevelBoost;

		// Token: 0x0400908F RID: 37007
		public AttributeConverter ToiletSpeed;

		// Token: 0x04009090 RID: 37008
		public AttributeConverter CarryAmountFromStrength;

		// Token: 0x04009091 RID: 37009
		public AttributeConverter TemperatureInsulation;

		// Token: 0x04009092 RID: 37010
		public AttributeConverter SeedHarvestChance;

		// Token: 0x04009093 RID: 37011
		public AttributeConverter RanchingEffectDuration;

		// Token: 0x04009094 RID: 37012
		public AttributeConverter FarmedEffectDuration;

		// Token: 0x04009095 RID: 37013
		public AttributeConverter PowerTinkerEffectDuration;

		// Token: 0x04009096 RID: 37014
		public AttributeConverter CapturableSpeed;

		// Token: 0x04009097 RID: 37015
		public AttributeConverter GeotuningSpeed;
	}
}
