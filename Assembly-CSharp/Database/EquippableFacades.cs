using System;

namespace Database
{
	// Token: 0x020021A6 RID: 8614
	public class EquippableFacades : ResourceSet<EquippableFacadeResource>
	{
		// Token: 0x0600B7F0 RID: 47088 RVA: 0x0046A1B8 File Offset: 0x004683B8
		public EquippableFacades(ResourceSet parent) : base("EquippableFacades", parent)
		{
			base.Initialize();
			foreach (EquippableFacadeInfo equippableFacadeInfo in Blueprints.Get().all.equippableFacades)
			{
				this.Add(equippableFacadeInfo.id, equippableFacadeInfo.name, equippableFacadeInfo.desc, equippableFacadeInfo.rarity, equippableFacadeInfo.defID, equippableFacadeInfo.buildOverride, equippableFacadeInfo.animFile, equippableFacadeInfo.GetRequiredDlcIds(), equippableFacadeInfo.GetForbiddenDlcIds());
			}
		}

		// Token: 0x0600B7F1 RID: 47089 RVA: 0x0046A25C File Offset: 0x0046845C
		[Obsolete("Please use Add(...) with required forbidden")]
		public void Add(string id, string name, string desc, PermitRarity rarity, string defID, string buildOverride, string animFile)
		{
			this.Add(id, name, desc, rarity, defID, buildOverride, animFile, null, null);
		}

		// Token: 0x0600B7F2 RID: 47090 RVA: 0x0046A27C File Offset: 0x0046847C
		[Obsolete("Please use Add(...) with required forbidden")]
		public void Add(string id, string name, string desc, PermitRarity rarity, string defID, string buildOverride, string animFile, string[] dlcIds)
		{
			DlcRestrictionsUtil.TemporaryHelperObject transientHelperObjectFromAllowList = DlcRestrictionsUtil.GetTransientHelperObjectFromAllowList(dlcIds);
			EquippableFacadeResource item = new EquippableFacadeResource(id, name, desc, rarity, buildOverride, defID, animFile, transientHelperObjectFromAllowList.GetRequiredDlcIds(), transientHelperObjectFromAllowList.GetForbiddenDlcIds());
			this.resources.Add(item);
		}

		// Token: 0x0600B7F3 RID: 47091 RVA: 0x0046A2BC File Offset: 0x004684BC
		public void Add(string id, string name, string desc, PermitRarity rarity, string defID, string buildOverride, string animFile, string[] requiredDlcIds = null, string[] forbiddenDlcIds = null)
		{
			EquippableFacadeResource item = new EquippableFacadeResource(id, name, desc, rarity, buildOverride, defID, animFile, requiredDlcIds, forbiddenDlcIds);
			this.resources.Add(item);
		}
	}
}
