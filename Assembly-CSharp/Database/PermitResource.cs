using System;

namespace Database
{
	// Token: 0x020021BC RID: 8636
	public abstract class PermitResource : Resource, IHasDlcRestrictions
	{
		// Token: 0x0600B851 RID: 47185 RVA: 0x0046E360 File Offset: 0x0046C560
		public PermitResource(string id, string Name, string Desc, PermitCategory permitCategory, PermitRarity rarity, string[] requiredDlcIds, string[] forbiddenDlcIds) : base(id, Name)
		{
			DebugUtil.DevAssert(Name != null, "Name must be provided for permit with id \"" + id + "\" of type " + base.GetType().Name, null);
			DebugUtil.DevAssert(Desc != null, "Description must be provided for permit with id \"" + id + "\" of type " + base.GetType().Name, null);
			this.Description = Desc;
			this.Category = permitCategory;
			this.Rarity = rarity;
			this.requiredDlcIds = requiredDlcIds;
			this.forbiddenDlcIds = forbiddenDlcIds;
		}

		// Token: 0x0600B852 RID: 47186
		public abstract PermitPresentationInfo GetPermitPresentationInfo();

		// Token: 0x0600B853 RID: 47187 RVA: 0x0011B6AF File Offset: 0x001198AF
		public bool IsOwnableOnServer()
		{
			return this.Rarity != PermitRarity.Universal && this.Rarity != PermitRarity.UniversalLocked;
		}

		// Token: 0x0600B854 RID: 47188 RVA: 0x0011B6C8 File Offset: 0x001198C8
		public bool IsUnlocked()
		{
			return this.Rarity == PermitRarity.Universal || PermitItems.IsPermitUnlocked(this);
		}

		// Token: 0x0600B855 RID: 47189 RVA: 0x0011B6DB File Offset: 0x001198DB
		public string GetDlcIdFrom()
		{
			return DlcManager.GetMostSignificantDlc(this);
		}

		// Token: 0x0600B856 RID: 47190 RVA: 0x0011B6E3 File Offset: 0x001198E3
		public string[] GetRequiredDlcIds()
		{
			return this.requiredDlcIds;
		}

		// Token: 0x0600B857 RID: 47191 RVA: 0x0011B6EB File Offset: 0x001198EB
		public string[] GetForbiddenDlcIds()
		{
			return this.forbiddenDlcIds;
		}

		// Token: 0x0400960D RID: 38413
		public string Description;

		// Token: 0x0400960E RID: 38414
		public PermitCategory Category;

		// Token: 0x0400960F RID: 38415
		public PermitRarity Rarity;

		// Token: 0x04009610 RID: 38416
		public string[] requiredDlcIds;

		// Token: 0x04009611 RID: 38417
		public string[] forbiddenDlcIds;
	}
}
