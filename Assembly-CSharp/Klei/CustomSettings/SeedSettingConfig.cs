using System;
using System.Collections.Generic;

namespace Klei.CustomSettings
{
	// Token: 0x02003C55 RID: 15445
	public class SeedSettingConfig : SettingConfig
	{
		// Token: 0x0600ECB3 RID: 60595 RVA: 0x004DEC10 File Offset: 0x004DCE10
		public SeedSettingConfig(string id, string label, string tooltip, bool debug_only = false, bool triggers_custom_game = true) : base(id, label, tooltip, "", "", -1L, debug_only, triggers_custom_game, null, "", false)
		{
		}

		// Token: 0x0600ECB4 RID: 60596 RVA: 0x00143648 File Offset: 0x00141848
		public override SettingLevel GetLevel(string level_id)
		{
			return new SettingLevel(level_id, level_id, level_id, 0L, null);
		}

		// Token: 0x0600ECB5 RID: 60597 RVA: 0x00143655 File Offset: 0x00141855
		public override List<SettingLevel> GetLevels()
		{
			return new List<SettingLevel>();
		}
	}
}
