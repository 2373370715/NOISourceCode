using System;
using STRINGS;

namespace Klei.CustomSettings
{
	// Token: 0x02003C56 RID: 15446
	public class DlcMixingSettingConfig : ToggleSettingConfig
	{
		// Token: 0x17000C36 RID: 3126
		// (get) Token: 0x0600ECB6 RID: 60598 RVA: 0x0014365C File Offset: 0x0014185C
		// (set) Token: 0x0600ECB7 RID: 60599 RVA: 0x00143664 File Offset: 0x00141864
		public virtual string dlcIdFrom { get; protected set; }

		// Token: 0x0600ECB8 RID: 60600 RVA: 0x004DEC40 File Offset: 0x004DCE40
		public DlcMixingSettingConfig(string id, string label, string tooltip, long coordinate_range = 5L, bool triggers_custom_game = false, string[] required_content = null, string dlcIdFrom = null, string missing_content_default = "") : base(id, label, tooltip, null, null, null, "Disabled", coordinate_range, false, triggers_custom_game, required_content, missing_content_default)
		{
			this.dlcIdFrom = dlcIdFrom;
			SettingLevel off_level = new SettingLevel("Disabled", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.DLC_MIXING.LEVELS.DISABLED.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.DLC_MIXING.LEVELS.DISABLED.TOOLTIP, 0L, null);
			SettingLevel on_level = new SettingLevel("Enabled", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.DLC_MIXING.LEVELS.ENABLED.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.DLC_MIXING.LEVELS.ENABLED.TOOLTIP, 1L, null);
			base.StompLevels(off_level, on_level, "Disabled", "Disabled");
		}

		// Token: 0x0400E8F0 RID: 59632
		private const int COORDINATE_RANGE = 5;

		// Token: 0x0400E8F1 RID: 59633
		public const string DisabledLevelId = "Disabled";

		// Token: 0x0400E8F2 RID: 59634
		public const string EnabledLevelId = "Enabled";
	}
}
