using System;
using System.Collections.Generic;
using ProcGen;
using STRINGS;
using UnityEngine;

namespace Klei.CustomSettings
{
	// Token: 0x02003C58 RID: 15448
	public class WorldMixingSettingConfig : MixingSettingConfig
	{
		// Token: 0x17000C3C RID: 3132
		// (get) Token: 0x0600ECC1 RID: 60609 RVA: 0x004DED00 File Offset: 0x004DCF00
		public override string label
		{
			get
			{
				WorldMixingSettings cachedWorldMixingSetting = SettingsCache.GetCachedWorldMixingSetting(base.worldgenPath);
				StringEntry entry;
				if (!Strings.TryGet(cachedWorldMixingSetting.name, out entry))
				{
					return cachedWorldMixingSetting.name;
				}
				return entry;
			}
		}

		// Token: 0x17000C3D RID: 3133
		// (get) Token: 0x0600ECC2 RID: 60610 RVA: 0x004DED38 File Offset: 0x004DCF38
		public override string tooltip
		{
			get
			{
				WorldMixingSettings cachedWorldMixingSetting = SettingsCache.GetCachedWorldMixingSetting(base.worldgenPath);
				StringEntry entry;
				if (!Strings.TryGet(cachedWorldMixingSetting.description, out entry))
				{
					return cachedWorldMixingSetting.description;
				}
				return entry;
			}
		}

		// Token: 0x17000C3E RID: 3134
		// (get) Token: 0x0600ECC3 RID: 60611 RVA: 0x004DED70 File Offset: 0x004DCF70
		public override Sprite icon
		{
			get
			{
				WorldMixingSettings cachedWorldMixingSetting = SettingsCache.GetCachedWorldMixingSetting(base.worldgenPath);
				Sprite sprite = (cachedWorldMixingSetting.icon != null) ? ColonyDestinationAsteroidBeltData.GetUISprite(cachedWorldMixingSetting.icon) : null;
				if (sprite == null)
				{
					sprite = Assets.GetSprite(cachedWorldMixingSetting.icon);
				}
				if (sprite == null)
				{
					sprite = Assets.GetSprite("unknown");
				}
				return sprite;
			}
		}

		// Token: 0x17000C3F RID: 3135
		// (get) Token: 0x0600ECC4 RID: 60612 RVA: 0x001436A7 File Offset: 0x001418A7
		public override List<string> forbiddenClusterTags
		{
			get
			{
				return SettingsCache.GetCachedWorldMixingSetting(base.worldgenPath).forbiddenClusterTags;
			}
		}

		// Token: 0x17000C40 RID: 3136
		// (get) Token: 0x0600ECC5 RID: 60613 RVA: 0x001436B9 File Offset: 0x001418B9
		public override bool isModded
		{
			get
			{
				return SettingsCache.GetCachedWorldMixingSetting(base.worldgenPath).isModded;
			}
		}

		// Token: 0x0600ECC6 RID: 60614 RVA: 0x004DEDD4 File Offset: 0x004DCFD4
		public WorldMixingSettingConfig(string id, string worldgenPath, string[] required_content = null, string dlcIdFrom = null, bool triggers_custom_game = true, long coordinate_range = 5L) : base(id, null, null, null, worldgenPath, coordinate_range, false, triggers_custom_game, required_content, "", false)
		{
			this.dlcIdFrom = dlcIdFrom;
			List<SettingLevel> levels = new List<SettingLevel>
			{
				new SettingLevel("Disabled", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.WORLD_MIXING.LEVELS.DISABLED.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.WORLD_MIXING.LEVELS.DISABLED.TOOLTIP, 0L, null),
				new SettingLevel("TryMixing", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.WORLD_MIXING.LEVELS.TRY_MIXING.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.WORLD_MIXING.LEVELS.TRY_MIXING.TOOLTIP, 1L, null),
				new SettingLevel("GuranteeMixing", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.WORLD_MIXING.LEVELS.GUARANTEE_MIXING.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.WORLD_MIXING.LEVELS.GUARANTEE_MIXING.TOOLTIP, 2L, null)
			};
			base.StompLevels(levels, "Disabled", "Disabled");
		}

		// Token: 0x0400E8F9 RID: 59641
		private const int COORDINATE_RANGE = 5;

		// Token: 0x0400E8FA RID: 59642
		public const string DisabledLevelId = "Disabled";

		// Token: 0x0400E8FB RID: 59643
		public const string TryMixingLevelId = "TryMixing";

		// Token: 0x0400E8FC RID: 59644
		public const string GuaranteeMixingLevelId = "GuranteeMixing";
	}
}
