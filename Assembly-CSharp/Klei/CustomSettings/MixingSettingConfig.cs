using System;
using System.Collections.Generic;
using UnityEngine;

namespace Klei.CustomSettings
{
	// Token: 0x02003C57 RID: 15447
	public class MixingSettingConfig : ListSettingConfig
	{
		// Token: 0x17000C37 RID: 3127
		// (get) Token: 0x0600ECB9 RID: 60601 RVA: 0x0014366D File Offset: 0x0014186D
		// (set) Token: 0x0600ECBA RID: 60602 RVA: 0x00143675 File Offset: 0x00141875
		public string worldgenPath { get; private set; }

		// Token: 0x17000C38 RID: 3128
		// (get) Token: 0x0600ECBB RID: 60603 RVA: 0x0014367E File Offset: 0x0014187E
		public virtual Sprite icon { get; }

		// Token: 0x17000C39 RID: 3129
		// (get) Token: 0x0600ECBC RID: 60604 RVA: 0x00143686 File Offset: 0x00141886
		public virtual List<string> forbiddenClusterTags { get; }

		// Token: 0x17000C3A RID: 3130
		// (get) Token: 0x0600ECBD RID: 60605 RVA: 0x0014368E File Offset: 0x0014188E
		// (set) Token: 0x0600ECBE RID: 60606 RVA: 0x00143696 File Offset: 0x00141896
		public virtual string dlcIdFrom { get; protected set; }

		// Token: 0x17000C3B RID: 3131
		// (get) Token: 0x0600ECBF RID: 60607 RVA: 0x0014369F File Offset: 0x0014189F
		public virtual bool isModded { get; }

		// Token: 0x0600ECC0 RID: 60608 RVA: 0x004DECC8 File Offset: 0x004DCEC8
		protected MixingSettingConfig(string id, List<SettingLevel> levels, string default_level_id, string nosweat_default_level_id, string worldgenPath, long coordinate_range = -1L, bool debug_only = false, bool triggers_custom_game = false, string[] required_content = null, string missing_content_default = "", bool hide_in_ui = false) : base(id, "", "", levels, default_level_id, nosweat_default_level_id, coordinate_range, debug_only, triggers_custom_game, required_content, missing_content_default, hide_in_ui)
		{
			this.worldgenPath = worldgenPath;
		}
	}
}
