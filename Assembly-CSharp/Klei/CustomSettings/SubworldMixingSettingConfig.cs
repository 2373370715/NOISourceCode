using System;
using System.Collections.Generic;
using ProcGen;
using STRINGS;
using UnityEngine;

namespace Klei.CustomSettings
{
	// Token: 0x02003C59 RID: 15449
	public class SubworldMixingSettingConfig : MixingSettingConfig
	{
		// Token: 0x17000C41 RID: 3137
		// (get) Token: 0x0600ECC7 RID: 60615 RVA: 0x004DEE8C File Offset: 0x004DD08C
		public override string label
		{
			get
			{
				SubworldMixingSettings cachedSubworldMixingSetting = SettingsCache.GetCachedSubworldMixingSetting(base.worldgenPath);
				StringEntry entry;
				if (!Strings.TryGet(cachedSubworldMixingSetting.name, out entry))
				{
					return cachedSubworldMixingSetting.name;
				}
				return entry;
			}
		}

		// Token: 0x17000C42 RID: 3138
		// (get) Token: 0x0600ECC8 RID: 60616 RVA: 0x004DEEC4 File Offset: 0x004DD0C4
		public override string tooltip
		{
			get
			{
				SubworldMixingSettings cachedSubworldMixingSetting = SettingsCache.GetCachedSubworldMixingSetting(base.worldgenPath);
				StringEntry entry;
				if (!Strings.TryGet(cachedSubworldMixingSetting.description, out entry))
				{
					return cachedSubworldMixingSetting.description;
				}
				return entry;
			}
		}

		// Token: 0x17000C43 RID: 3139
		// (get) Token: 0x0600ECC9 RID: 60617 RVA: 0x004DEEFC File Offset: 0x004DD0FC
		public override Sprite icon
		{
			get
			{
				SubworldMixingSettings cachedSubworldMixingSetting = SettingsCache.GetCachedSubworldMixingSetting(base.worldgenPath);
				Sprite sprite = (cachedSubworldMixingSetting.icon != null) ? Assets.GetSprite(cachedSubworldMixingSetting.icon) : null;
				if (sprite == null)
				{
					sprite = Assets.GetSprite("unknown");
				}
				return sprite;
			}
		}

		// Token: 0x17000C44 RID: 3140
		// (get) Token: 0x0600ECCA RID: 60618 RVA: 0x001436CB File Offset: 0x001418CB
		public override List<string> forbiddenClusterTags
		{
			get
			{
				return SettingsCache.GetCachedSubworldMixingSetting(base.worldgenPath).forbiddenClusterTags;
			}
		}

		// Token: 0x17000C45 RID: 3141
		// (get) Token: 0x0600ECCB RID: 60619 RVA: 0x001436DD File Offset: 0x001418DD
		public override bool isModded
		{
			get
			{
				return SettingsCache.GetCachedSubworldMixingSetting(base.worldgenPath).isModded;
			}
		}

		// Token: 0x0600ECCC RID: 60620 RVA: 0x004DEF4C File Offset: 0x004DD14C
		public SubworldMixingSettingConfig(string id, string worldgenPath, string[] required_content = null, string dlcIdFrom = null, bool triggers_custom_game = true, long coordinate_range = 5L) : base(id, null, null, null, worldgenPath, coordinate_range, false, triggers_custom_game, required_content, "", false)
		{
			this.dlcIdFrom = dlcIdFrom;
			List<SettingLevel> levels = new List<SettingLevel>
			{
				new SettingLevel("Disabled", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SUBWORLD_MIXING.LEVELS.DISABLED.NAME, DlcManager.FeatureClusterSpaceEnabled() ? UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SUBWORLD_MIXING.LEVELS.DISABLED.TOOLTIP : UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SUBWORLD_MIXING.LEVELS.DISABLED.TOOLTIP_BASEGAME, 0L, null),
				new SettingLevel("TryMixing", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SUBWORLD_MIXING.LEVELS.TRY_MIXING.NAME, DlcManager.FeatureClusterSpaceEnabled() ? UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SUBWORLD_MIXING.LEVELS.TRY_MIXING.TOOLTIP : UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SUBWORLD_MIXING.LEVELS.TRY_MIXING.TOOLTIP_BASEGAME, 1L, null),
				new SettingLevel("GuranteeMixing", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SUBWORLD_MIXING.LEVELS.GUARANTEE_MIXING.NAME, DlcManager.FeatureClusterSpaceEnabled() ? UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SUBWORLD_MIXING.LEVELS.GUARANTEE_MIXING.TOOLTIP : UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SUBWORLD_MIXING.LEVELS.GUARANTEE_MIXING.TOOLTIP_BASEGAME, 2L, null)
			};
			base.StompLevels(levels, "Disabled", "Disabled");
		}

		// Token: 0x0400E8FD RID: 59645
		private const int COORDINATE_RANGE = 5;

		// Token: 0x0400E8FE RID: 59646
		public const string DisabledLevelId = "Disabled";

		// Token: 0x0400E8FF RID: 59647
		public const string TryMixingLevelId = "TryMixing";

		// Token: 0x0400E900 RID: 59648
		public const string GuaranteeMixingLevelId = "GuranteeMixing";
	}
}
