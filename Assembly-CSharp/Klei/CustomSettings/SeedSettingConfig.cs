﻿using System;
using System.Collections.Generic;

namespace Klei.CustomSettings
{
	public class SeedSettingConfig : SettingConfig
	{
		public SeedSettingConfig(string id, string label, string tooltip, bool debug_only = false, bool triggers_custom_game = true) : base(id, label, tooltip, "", "", -1L, debug_only, triggers_custom_game, null, "", false)
		{
		}

		public override SettingLevel GetLevel(string level_id)
		{
			return new SettingLevel(level_id, level_id, level_id, 0L, null);
		}

		public override List<SettingLevel> GetLevels()
		{
			return new List<SettingLevel>();
		}
	}
}
