using System;

namespace Database
{
	// Token: 0x0200222B RID: 8747
	public class SkillPerk : Resource, IHasDlcRestrictions
	{
		// Token: 0x0600B9FC RID: 47612 RVA: 0x0011C589 File Offset: 0x0011A789
		public string[] GetRequiredDlcIds()
		{
			return this.requiredDlcIds;
		}

		// Token: 0x0600B9FD RID: 47613 RVA: 0x000AA765 File Offset: 0x000A8965
		public string[] GetForbiddenDlcIds()
		{
			return null;
		}

		// Token: 0x17000BF8 RID: 3064
		// (get) Token: 0x0600B9FE RID: 47614 RVA: 0x0011C591 File Offset: 0x0011A791
		// (set) Token: 0x0600B9FF RID: 47615 RVA: 0x0011C599 File Offset: 0x0011A799
		public Action<MinionResume> OnApply { get; protected set; }

		// Token: 0x17000BF9 RID: 3065
		// (get) Token: 0x0600BA00 RID: 47616 RVA: 0x0011C5A2 File Offset: 0x0011A7A2
		// (set) Token: 0x0600BA01 RID: 47617 RVA: 0x0011C5AA File Offset: 0x0011A7AA
		public Action<MinionResume> OnRemove { get; protected set; }

		// Token: 0x17000BFA RID: 3066
		// (get) Token: 0x0600BA02 RID: 47618 RVA: 0x0011C5B3 File Offset: 0x0011A7B3
		// (set) Token: 0x0600BA03 RID: 47619 RVA: 0x0011C5BB File Offset: 0x0011A7BB
		public Action<MinionResume> OnMinionsChanged { get; protected set; }

		// Token: 0x17000BFB RID: 3067
		// (get) Token: 0x0600BA04 RID: 47620 RVA: 0x0011C5C4 File Offset: 0x0011A7C4
		// (set) Token: 0x0600BA05 RID: 47621 RVA: 0x0011C5CC File Offset: 0x0011A7CC
		public bool affectAll { get; protected set; }

		// Token: 0x0600BA06 RID: 47622 RVA: 0x0047A330 File Offset: 0x00478530
		public static string GetDescription(string perkID)
		{
			string text = GameUtil.NamesOfBuildingsRequiringSkillPerk(perkID);
			if (text == null)
			{
				return Db.Get().SkillPerks.Get(perkID).Name;
			}
			return text;
		}

		// Token: 0x0600BA07 RID: 47623 RVA: 0x0011C5D5 File Offset: 0x0011A7D5
		public SkillPerk(string id_str, string description, Action<MinionResume> OnApply, Action<MinionResume> OnRemove, Action<MinionResume> OnMinionsChanged, bool affectAll = false) : base(id_str, description)
		{
			this.OnApply = OnApply;
			this.OnRemove = OnRemove;
			this.OnMinionsChanged = OnMinionsChanged;
			this.affectAll = affectAll;
		}

		// Token: 0x0600BA08 RID: 47624 RVA: 0x0011C5FE File Offset: 0x0011A7FE
		public SkillPerk(string id_str, string description, Action<MinionResume> OnApply, Action<MinionResume> OnRemove, Action<MinionResume> OnMinionsChanged, string[] requiredDlcIds = null, bool affectAll = false) : base(id_str, description)
		{
			this.OnApply = OnApply;
			this.OnRemove = OnRemove;
			this.OnMinionsChanged = OnMinionsChanged;
			this.affectAll = affectAll;
			this.requiredDlcIds = requiredDlcIds;
		}

		// Token: 0x040097E2 RID: 38882
		public string[] requiredDlcIds;
	}
}
