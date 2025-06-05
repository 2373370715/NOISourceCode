using System;
using Klei.AI;

namespace Database
{
	// Token: 0x020021C5 RID: 8645
	public class PlantAttributes : ResourceSet<Klei.AI.Attribute>
	{
		// Token: 0x0600B86F RID: 47215 RVA: 0x0046E8F8 File Offset: 0x0046CAF8
		public PlantAttributes(ResourceSet parent) : base("PlantAttributes", parent)
		{
			this.WiltTempRangeMod = base.Add(new Klei.AI.Attribute("WiltTempRangeMod", false, Klei.AI.Attribute.Display.Normal, false, 1f, null, null, null, null));
			this.WiltTempRangeMod.SetFormatter(new PercentAttributeFormatter());
			this.YieldAmount = base.Add(new Klei.AI.Attribute("YieldAmount", false, Klei.AI.Attribute.Display.Normal, false, 0f, null, null, null, null));
			this.YieldAmount.SetFormatter(new PercentAttributeFormatter());
			this.HarvestTime = base.Add(new Klei.AI.Attribute("HarvestTime", false, Klei.AI.Attribute.Display.Normal, false, 0f, null, null, null, null));
			this.HarvestTime.SetFormatter(new StandardAttributeFormatter(GameUtil.UnitClass.Time, GameUtil.TimeSlice.None));
			this.DecorBonus = base.Add(new Klei.AI.Attribute("DecorBonus", false, Klei.AI.Attribute.Display.Normal, false, 0f, null, null, null, null));
			this.DecorBonus.SetFormatter(new StandardAttributeFormatter(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.None));
			this.MinLightLux = base.Add(new Klei.AI.Attribute("MinLightLux", false, Klei.AI.Attribute.Display.Normal, false, 0f, null, null, null, null));
			this.MinLightLux.SetFormatter(new StandardAttributeFormatter(GameUtil.UnitClass.Lux, GameUtil.TimeSlice.None));
			this.FertilizerUsageMod = base.Add(new Klei.AI.Attribute("FertilizerUsageMod", false, Klei.AI.Attribute.Display.Normal, false, 1f, null, null, null, null));
			this.FertilizerUsageMod.SetFormatter(new PercentAttributeFormatter());
			this.MinRadiationThreshold = base.Add(new Klei.AI.Attribute("MinRadiationThreshold", false, Klei.AI.Attribute.Display.Normal, false, 0f, null, null, null, null));
			this.MinRadiationThreshold.SetFormatter(new RadsPerCycleAttributeFormatter());
			this.MaxRadiationThreshold = base.Add(new Klei.AI.Attribute("MaxRadiationThreshold", false, Klei.AI.Attribute.Display.Normal, false, 0f, null, null, null, null));
			this.MaxRadiationThreshold.SetFormatter(new RadsPerCycleAttributeFormatter());
		}

		// Token: 0x0400963B RID: 38459
		public Klei.AI.Attribute WiltTempRangeMod;

		// Token: 0x0400963C RID: 38460
		public Klei.AI.Attribute YieldAmount;

		// Token: 0x0400963D RID: 38461
		public Klei.AI.Attribute HarvestTime;

		// Token: 0x0400963E RID: 38462
		public Klei.AI.Attribute DecorBonus;

		// Token: 0x0400963F RID: 38463
		public Klei.AI.Attribute MinLightLux;

		// Token: 0x04009640 RID: 38464
		public Klei.AI.Attribute FertilizerUsageMod;

		// Token: 0x04009641 RID: 38465
		public Klei.AI.Attribute MinRadiationThreshold;

		// Token: 0x04009642 RID: 38466
		public Klei.AI.Attribute MaxRadiationThreshold;
	}
}
