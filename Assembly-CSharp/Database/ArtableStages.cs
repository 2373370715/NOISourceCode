using System;
using System.Collections.Generic;

namespace Database
{
	// Token: 0x0200217D RID: 8573
	public class ArtableStages : ResourceSet<ArtableStage>
	{
		// Token: 0x0600B686 RID: 46726 RVA: 0x00457590 File Offset: 0x00455790
		[Obsolete("Use ArtableStages with required/forbidden")]
		public ArtableStage Add(string id, string name, string desc, PermitRarity rarity, string animFile, string anim, int decor_value, bool cheer_on_complete, string status_id, string prefabId, string symbolname, string[] dlcIds)
		{
			DlcRestrictionsUtil.TemporaryHelperObject transientHelperObjectFromAllowList = DlcRestrictionsUtil.GetTransientHelperObjectFromAllowList(dlcIds);
			return this.Add(id, name, desc, rarity, animFile, anim, decor_value, cheer_on_complete, status_id, prefabId, symbolname, transientHelperObjectFromAllowList.GetRequiredDlcIds(), transientHelperObjectFromAllowList.GetForbiddenDlcIds());
		}

		// Token: 0x0600B687 RID: 46727 RVA: 0x004575CC File Offset: 0x004557CC
		public ArtableStage Add(string id, string name, string desc, PermitRarity rarity, string animFile, string anim, int decor_value, bool cheer_on_complete, string status_id, string prefabId, string symbolname, string[] requiredDlcIds, string[] forbiddenDlcIds)
		{
			ArtableStatusItem status_item = Db.Get().ArtableStatuses.Get(status_id);
			ArtableStage artableStage = new ArtableStage(id, name, desc, rarity, animFile, anim, decor_value, cheer_on_complete, status_item, prefabId, symbolname, requiredDlcIds, forbiddenDlcIds);
			this.resources.Add(artableStage);
			return artableStage;
		}

		// Token: 0x0600B688 RID: 46728 RVA: 0x00457614 File Offset: 0x00455814
		public ArtableStages(ResourceSet parent) : base("ArtableStages", parent)
		{
			foreach (ArtableInfo artableInfo in Blueprints.Get().all.artables)
			{
				this.Add(artableInfo.id, artableInfo.name, artableInfo.desc, artableInfo.rarity, artableInfo.animFile, artableInfo.anim, artableInfo.decor_value, artableInfo.cheer_on_complete, artableInfo.status_id, artableInfo.prefabId, artableInfo.symbolname, artableInfo.GetRequiredDlcIds(), artableInfo.GetForbiddenDlcIds());
			}
		}

		// Token: 0x0600B689 RID: 46729 RVA: 0x004576CC File Offset: 0x004558CC
		public List<ArtableStage> GetPrefabStages(Tag prefab_id)
		{
			return this.resources.FindAll((ArtableStage stage) => stage.prefabId == prefab_id);
		}

		// Token: 0x0600B68A RID: 46730 RVA: 0x0011AE89 File Offset: 0x00119089
		public ArtableStage DefaultPrefabStage(Tag prefab_id)
		{
			return this.GetPrefabStages(prefab_id).Find((ArtableStage stage) => stage.statusItem == Db.Get().ArtableStatuses.AwaitingArting);
		}
	}
}
