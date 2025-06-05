using System;
using System.Collections.Generic;
using UnityEngine;

namespace Klei.CustomSettings
{
	// Token: 0x02003C51 RID: 15441
	public class ListSettingConfig : SettingConfig
	{
		// Token: 0x17000C33 RID: 3123
		// (get) Token: 0x0600EC9C RID: 60572 RVA: 0x00143553 File Offset: 0x00141753
		// (set) Token: 0x0600EC9D RID: 60573 RVA: 0x0014355B File Offset: 0x0014175B
		public List<SettingLevel> levels { get; private set; }

		// Token: 0x0600EC9E RID: 60574 RVA: 0x004DE8F4 File Offset: 0x004DCAF4
		public ListSettingConfig(string id, string label, string tooltip, List<SettingLevel> levels, string default_level_id, string nosweat_default_level_id, long coordinate_range = -1L, bool debug_only = false, bool triggers_custom_game = true, string[] required_content = null, string missing_content_default = "", bool hide_in_ui = false) : base(id, label, tooltip, default_level_id, nosweat_default_level_id, coordinate_range, debug_only, triggers_custom_game, required_content, missing_content_default, hide_in_ui)
		{
			this.levels = levels;
		}

		// Token: 0x0600EC9F RID: 60575 RVA: 0x00143564 File Offset: 0x00141764
		public void StompLevels(List<SettingLevel> levels, string default_level_id, string nosweat_default_level_id)
		{
			this.levels = levels;
			this.default_level_id = default_level_id;
			this.nosweat_default_level_id = nosweat_default_level_id;
		}

		// Token: 0x0600ECA0 RID: 60576 RVA: 0x004DE924 File Offset: 0x004DCB24
		public override SettingLevel GetLevel(string level_id)
		{
			for (int i = 0; i < this.levels.Count; i++)
			{
				if (this.levels[i].id == level_id)
				{
					return this.levels[i];
				}
			}
			for (int j = 0; j < this.levels.Count; j++)
			{
				if (this.levels[j].id == this.default_level_id)
				{
					return this.levels[j];
				}
			}
			global::Debug.LogError("Unable to find setting level for setting:" + base.id + " level: " + level_id);
			return null;
		}

		// Token: 0x0600ECA1 RID: 60577 RVA: 0x0014357B File Offset: 0x0014177B
		public override List<SettingLevel> GetLevels()
		{
			return this.levels;
		}

		// Token: 0x0600ECA2 RID: 60578 RVA: 0x004DE9CC File Offset: 0x004DCBCC
		public string CycleSettingLevelID(string current_id, int direction)
		{
			string result = "";
			if (current_id == "")
			{
				current_id = this.levels[0].id;
			}
			for (int i = 0; i < this.levels.Count; i++)
			{
				if (this.levels[i].id == current_id)
				{
					int index = Mathf.Clamp(i + direction, 0, this.levels.Count - 1);
					result = this.levels[index].id;
					break;
				}
			}
			return result;
		}

		// Token: 0x0600ECA3 RID: 60579 RVA: 0x004DEA5C File Offset: 0x004DCC5C
		public bool IsFirstLevel(string level_id)
		{
			return this.levels.FindIndex((SettingLevel l) => l.id == level_id) == 0;
		}

		// Token: 0x0600ECA4 RID: 60580 RVA: 0x004DEA90 File Offset: 0x004DCC90
		public bool IsLastLevel(string level_id)
		{
			return this.levels.FindIndex((SettingLevel l) => l.id == level_id) == this.levels.Count - 1;
		}
	}
}
