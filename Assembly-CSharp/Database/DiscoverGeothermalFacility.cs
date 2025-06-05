using System;
using STRINGS;

namespace Database
{
	// Token: 0x020021F7 RID: 8695
	public class DiscoverGeothermalFacility : VictoryColonyAchievementRequirement
	{
		// Token: 0x0600B92D RID: 47405 RVA: 0x0011BE2E File Offset: 0x0011A02E
		public override string Description()
		{
			return this.GetProgress(this.Success());
		}

		// Token: 0x0600B92E RID: 47406 RVA: 0x0011BE7A File Offset: 0x0011A07A
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.REQUIREMENTS.DISCOVER_GEOTHERMAL_FACILITY_DESCRIPTION;
		}

		// Token: 0x0600B92F RID: 47407 RVA: 0x0011BE86 File Offset: 0x0011A086
		public override string Name()
		{
			return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.REQUIREMENTS.DISCOVER_GEOTHERMAL_FACILITY_TITLE;
		}

		// Token: 0x0600B930 RID: 47408 RVA: 0x0011BE92 File Offset: 0x0011A092
		public override bool Success()
		{
			return GeothermalPlantComponent.GeothermalFacilityDiscovered();
		}
	}
}
