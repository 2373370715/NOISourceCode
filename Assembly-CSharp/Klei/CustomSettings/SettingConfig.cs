using System;
using System.Collections.Generic;

namespace Klei.CustomSettings
{
	// Token: 0x02003C50 RID: 15440
	public abstract class SettingConfig : IHasDlcRestrictions
	{
		// Token: 0x0600EC81 RID: 60545 RVA: 0x004DE88C File Offset: 0x004DCA8C
		public SettingConfig(string id, string label, string tooltip, string default_level_id, string nosweat_default_level_id, long coordinate_range = -1L, bool debug_only = false, bool triggers_custom_game = true, string[] required_content = null, string missing_content_default = "", bool hide_in_ui = false)
		{
			this.id = id;
			this.label = label;
			this.tooltip = tooltip;
			this.default_level_id = default_level_id;
			this.nosweat_default_level_id = nosweat_default_level_id;
			this.coordinate_range = coordinate_range;
			this.debug_only = debug_only;
			this.triggers_custom_game = triggers_custom_game;
			this.required_content = required_content;
			this.missing_content_default = missing_content_default;
			this.hide_in_ui = hide_in_ui;
		}

		// Token: 0x17000C2A RID: 3114
		// (get) Token: 0x0600EC82 RID: 60546 RVA: 0x0014341F File Offset: 0x0014161F
		// (set) Token: 0x0600EC83 RID: 60547 RVA: 0x00143427 File Offset: 0x00141627
		public string id { get; private set; }

		// Token: 0x17000C2B RID: 3115
		// (get) Token: 0x0600EC84 RID: 60548 RVA: 0x00143430 File Offset: 0x00141630
		// (set) Token: 0x0600EC85 RID: 60549 RVA: 0x00143438 File Offset: 0x00141638
		public virtual string label { get; private set; }

		// Token: 0x17000C2C RID: 3116
		// (get) Token: 0x0600EC86 RID: 60550 RVA: 0x00143441 File Offset: 0x00141641
		// (set) Token: 0x0600EC87 RID: 60551 RVA: 0x00143449 File Offset: 0x00141649
		public virtual string tooltip { get; private set; }

		// Token: 0x17000C2D RID: 3117
		// (get) Token: 0x0600EC88 RID: 60552 RVA: 0x00143452 File Offset: 0x00141652
		// (set) Token: 0x0600EC89 RID: 60553 RVA: 0x0014345A File Offset: 0x0014165A
		public long coordinate_range { get; protected set; }

		// Token: 0x17000C2E RID: 3118
		// (get) Token: 0x0600EC8A RID: 60554 RVA: 0x00143463 File Offset: 0x00141663
		// (set) Token: 0x0600EC8B RID: 60555 RVA: 0x0014346B File Offset: 0x0014166B
		public string[] required_content { get; private set; }

		// Token: 0x17000C2F RID: 3119
		// (get) Token: 0x0600EC8C RID: 60556 RVA: 0x00143474 File Offset: 0x00141674
		// (set) Token: 0x0600EC8D RID: 60557 RVA: 0x0014347C File Offset: 0x0014167C
		public string missing_content_default { get; private set; }

		// Token: 0x17000C30 RID: 3120
		// (get) Token: 0x0600EC8E RID: 60558 RVA: 0x00143485 File Offset: 0x00141685
		// (set) Token: 0x0600EC8F RID: 60559 RVA: 0x0014348D File Offset: 0x0014168D
		public bool triggers_custom_game { get; protected set; }

		// Token: 0x17000C31 RID: 3121
		// (get) Token: 0x0600EC90 RID: 60560 RVA: 0x00143496 File Offset: 0x00141696
		// (set) Token: 0x0600EC91 RID: 60561 RVA: 0x0014349E File Offset: 0x0014169E
		public bool debug_only { get; protected set; }

		// Token: 0x17000C32 RID: 3122
		// (get) Token: 0x0600EC92 RID: 60562 RVA: 0x001434A7 File Offset: 0x001416A7
		// (set) Token: 0x0600EC93 RID: 60563 RVA: 0x001434AF File Offset: 0x001416AF
		public bool hide_in_ui { get; protected set; }

		// Token: 0x0600EC94 RID: 60564
		public abstract SettingLevel GetLevel(string level_id);

		// Token: 0x0600EC95 RID: 60565
		public abstract List<SettingLevel> GetLevels();

		// Token: 0x0600EC96 RID: 60566 RVA: 0x001434B8 File Offset: 0x001416B8
		public bool IsDefaultLevel(string level_id)
		{
			return level_id == this.default_level_id;
		}

		// Token: 0x0600EC97 RID: 60567 RVA: 0x001434C6 File Offset: 0x001416C6
		public bool ShowInUI()
		{
			return !this.deprecated && !this.hide_in_ui && (!this.debug_only || DebugHandler.enabled) && DlcManager.IsAllContentSubscribed(this.required_content);
		}

		// Token: 0x0600EC98 RID: 60568 RVA: 0x001434F9 File Offset: 0x001416F9
		public string GetDefaultLevelId()
		{
			if (!DlcManager.IsAllContentSubscribed(this.required_content) && !string.IsNullOrEmpty(this.missing_content_default))
			{
				return this.missing_content_default;
			}
			return this.default_level_id;
		}

		// Token: 0x0600EC99 RID: 60569 RVA: 0x00143522 File Offset: 0x00141722
		public string GetNoSweatDefaultLevelId()
		{
			if (!DlcManager.IsAllContentSubscribed(this.required_content) && !string.IsNullOrEmpty(this.missing_content_default))
			{
				return this.missing_content_default;
			}
			return this.nosweat_default_level_id;
		}

		// Token: 0x0600EC9A RID: 60570 RVA: 0x0014354B File Offset: 0x0014174B
		public string[] GetRequiredDlcIds()
		{
			return this.required_content;
		}

		// Token: 0x0600EC9B RID: 60571 RVA: 0x000AA765 File Offset: 0x000A8965
		public string[] GetForbiddenDlcIds()
		{
			return null;
		}

		// Token: 0x0400E8E2 RID: 59618
		protected string default_level_id;

		// Token: 0x0400E8E3 RID: 59619
		protected string nosweat_default_level_id;

		// Token: 0x0400E8EA RID: 59626
		public bool deprecated;
	}
}
