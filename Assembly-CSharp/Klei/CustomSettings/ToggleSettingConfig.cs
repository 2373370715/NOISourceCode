using System;
using System.Collections.Generic;

namespace Klei.CustomSettings
{
	// Token: 0x02003C54 RID: 15444
	public class ToggleSettingConfig : SettingConfig
	{
		// Token: 0x17000C34 RID: 3124
		// (get) Token: 0x0600ECA9 RID: 60585 RVA: 0x001435A9 File Offset: 0x001417A9
		// (set) Token: 0x0600ECAA RID: 60586 RVA: 0x001435B1 File Offset: 0x001417B1
		public SettingLevel on_level { get; private set; }

		// Token: 0x17000C35 RID: 3125
		// (get) Token: 0x0600ECAB RID: 60587 RVA: 0x001435BA File Offset: 0x001417BA
		// (set) Token: 0x0600ECAC RID: 60588 RVA: 0x001435C2 File Offset: 0x001417C2
		public SettingLevel off_level { get; private set; }

		// Token: 0x0600ECAD RID: 60589 RVA: 0x004DEAD0 File Offset: 0x004DCCD0
		public ToggleSettingConfig(string id, string label, string tooltip, SettingLevel off_level, SettingLevel on_level, string default_level_id, string nosweat_default_level_id, long coordinate_range = -1L, bool debug_only = false, bool triggers_custom_game = true, string[] required_content = null, string missing_content_default = "") : base(id, label, tooltip, default_level_id, nosweat_default_level_id, coordinate_range, debug_only, triggers_custom_game, required_content, missing_content_default, false)
		{
			this.off_level = off_level;
			this.on_level = on_level;
		}

		// Token: 0x0600ECAE RID: 60590 RVA: 0x001435CB File Offset: 0x001417CB
		public void StompLevels(SettingLevel off_level, SettingLevel on_level, string default_level_id, string nosweat_default_level_id)
		{
			this.off_level = off_level;
			this.on_level = on_level;
			this.default_level_id = default_level_id;
			this.nosweat_default_level_id = nosweat_default_level_id;
		}

		// Token: 0x0600ECAF RID: 60591 RVA: 0x004DEB08 File Offset: 0x004DCD08
		public override SettingLevel GetLevel(string level_id)
		{
			if (this.on_level.id == level_id)
			{
				return this.on_level;
			}
			if (this.off_level.id == level_id)
			{
				return this.off_level;
			}
			if (this.default_level_id == this.on_level.id)
			{
				Debug.LogWarning(string.Concat(new string[]
				{
					"Unable to find level for setting:",
					base.id,
					"(",
					level_id,
					") Using default level."
				}));
				return this.on_level;
			}
			if (this.default_level_id == this.off_level.id)
			{
				Debug.LogWarning(string.Concat(new string[]
				{
					"Unable to find level for setting:",
					base.id,
					"(",
					level_id,
					") Using default level."
				}));
				return this.off_level;
			}
			Debug.LogError("Unable to find setting level for setting:" + base.id + " level: " + level_id);
			return null;
		}

		// Token: 0x0600ECB0 RID: 60592 RVA: 0x001435EA File Offset: 0x001417EA
		public override List<SettingLevel> GetLevels()
		{
			return new List<SettingLevel>
			{
				this.off_level,
				this.on_level
			};
		}

		// Token: 0x0600ECB1 RID: 60593 RVA: 0x00143609 File Offset: 0x00141809
		public string ToggleSettingLevelID(string current_id)
		{
			if (this.on_level.id == current_id)
			{
				return this.off_level.id;
			}
			return this.on_level.id;
		}

		// Token: 0x0600ECB2 RID: 60594 RVA: 0x00143635 File Offset: 0x00141835
		public bool IsOnLevel(string level_id)
		{
			return level_id == this.on_level.id;
		}
	}
}
