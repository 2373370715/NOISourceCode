using System;
using STRINGS;

namespace Klei.CustomSettings
{
	// Token: 0x02003C5A RID: 15450
	public static class CustomMixingSettingsConfigs
	{
		// Token: 0x0400E901 RID: 59649
		public static SettingConfig DLC2Mixing = new DlcMixingSettingConfig("DLC2_ID", UI.DLC2.NAME, UI.DLC2.MIXING_TOOLTIP, 5L, false, DlcManager.DLC2, "DLC2_ID", "");

		// Token: 0x0400E902 RID: 59650
		public static SettingConfig DLC3Mixing = new DlcMixingSettingConfig("DLC3_ID", UI.DLC3.NAME, UI.DLC3.MIXING_TOOLTIP, 5L, false, DlcManager.DLC3, "DLC3_ID", "");

		// Token: 0x0400E903 RID: 59651
		public static SettingConfig CeresAsteroidMixing = new WorldMixingSettingConfig("CeresAsteroidMixing", "dlc2::worldMixing/CeresMixingSettings", DlcManager.DLC2, "DLC2_ID", true, 5L);

		// Token: 0x0400E904 RID: 59652
		public static SettingConfig IceCavesMixing = new SubworldMixingSettingConfig("IceCavesMixing", "dlc2::subworldMixing/IceCavesMixingSettings", DlcManager.DLC2, "DLC2_ID", true, 5L);

		// Token: 0x0400E905 RID: 59653
		public static SettingConfig CarrotQuarryMixing = new SubworldMixingSettingConfig("CarrotQuarryMixing", "dlc2::subworldMixing/CarrotQuarryMixingSettings", DlcManager.DLC2, "DLC2_ID", true, 5L);

		// Token: 0x0400E906 RID: 59654
		public static SettingConfig SugarWoodsMixing = new SubworldMixingSettingConfig("SugarWoodsMixing", "dlc2::subworldMixing/SugarWoodsMixingSettings", DlcManager.DLC2, "DLC2_ID", true, 5L);
	}
}
