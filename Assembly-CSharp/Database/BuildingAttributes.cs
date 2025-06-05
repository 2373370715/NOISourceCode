using System;
using Klei.AI;

namespace Database
{
	// Token: 0x02002189 RID: 8585
	public class BuildingAttributes : ResourceSet<Klei.AI.Attribute>
	{
		// Token: 0x0600B6AD RID: 46765 RVA: 0x00458DA0 File Offset: 0x00456FA0
		public BuildingAttributes(ResourceSet parent) : base("BuildingAttributes", parent)
		{
			this.Decor = base.Add(new Klei.AI.Attribute("Decor", true, Klei.AI.Attribute.Display.General, false, 0f, null, null, null, null));
			this.DecorRadius = base.Add(new Klei.AI.Attribute("DecorRadius", true, Klei.AI.Attribute.Display.General, false, 0f, null, null, null, null));
			this.NoisePollution = base.Add(new Klei.AI.Attribute("NoisePollution", true, Klei.AI.Attribute.Display.General, false, 0f, null, null, null, null));
			this.NoisePollutionRadius = base.Add(new Klei.AI.Attribute("NoisePollutionRadius", true, Klei.AI.Attribute.Display.General, false, 0f, null, null, null, null));
			this.Hygiene = base.Add(new Klei.AI.Attribute("Hygiene", true, Klei.AI.Attribute.Display.General, false, 0f, null, null, null, null));
			this.Comfort = base.Add(new Klei.AI.Attribute("Comfort", true, Klei.AI.Attribute.Display.General, false, 0f, null, null, null, null));
			this.OverheatTemperature = base.Add(new Klei.AI.Attribute("OverheatTemperature", true, Klei.AI.Attribute.Display.General, false, 0f, null, null, null, null));
			this.OverheatTemperature.SetFormatter(new StandardAttributeFormatter(GameUtil.UnitClass.Temperature, GameUtil.TimeSlice.ModifyOnly));
			this.FatalTemperature = base.Add(new Klei.AI.Attribute("FatalTemperature", true, Klei.AI.Attribute.Display.General, false, 0f, null, null, null, null));
			this.FatalTemperature.SetFormatter(new StandardAttributeFormatter(GameUtil.UnitClass.Temperature, GameUtil.TimeSlice.ModifyOnly));
		}

		// Token: 0x040090D3 RID: 37075
		public Klei.AI.Attribute Decor;

		// Token: 0x040090D4 RID: 37076
		public Klei.AI.Attribute DecorRadius;

		// Token: 0x040090D5 RID: 37077
		public Klei.AI.Attribute NoisePollution;

		// Token: 0x040090D6 RID: 37078
		public Klei.AI.Attribute NoisePollutionRadius;

		// Token: 0x040090D7 RID: 37079
		public Klei.AI.Attribute Hygiene;

		// Token: 0x040090D8 RID: 37080
		public Klei.AI.Attribute Comfort;

		// Token: 0x040090D9 RID: 37081
		public Klei.AI.Attribute OverheatTemperature;

		// Token: 0x040090DA RID: 37082
		public Klei.AI.Attribute FatalTemperature;
	}
}
