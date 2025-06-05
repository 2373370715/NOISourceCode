using System;
using System.Collections.Generic;

namespace Database
{
	// Token: 0x020021AF RID: 8623
	public class MonumentParts : ResourceSet<MonumentPartResource>
	{
		// Token: 0x0600B831 RID: 47153 RVA: 0x0046D88C File Offset: 0x0046BA8C
		public MonumentParts(ResourceSet parent) : base("MonumentParts", parent)
		{
			base.Initialize();
			foreach (MonumentPartInfo monumentPartInfo in Blueprints.Get().all.monumentParts)
			{
				this.Add(monumentPartInfo.id, monumentPartInfo.name, monumentPartInfo.desc, monumentPartInfo.rarity, monumentPartInfo.animFile, monumentPartInfo.state, monumentPartInfo.symbolName, monumentPartInfo.part, monumentPartInfo.requiredDlcIds, monumentPartInfo.forbiddenDlcIds);
			}
		}

		// Token: 0x0600B832 RID: 47154 RVA: 0x0046D938 File Offset: 0x0046BB38
		public void Add(string id, string name, string desc, PermitRarity rarity, string animFilename, string state, string symbolName, MonumentPartResource.Part part, string[] requiredDlcIds, string[] forbiddenDlcIds)
		{
			MonumentPartResource item = new MonumentPartResource(id, name, desc, rarity, animFilename, state, symbolName, part, requiredDlcIds, forbiddenDlcIds);
			this.resources.Add(item);
		}

		// Token: 0x0600B833 RID: 47155 RVA: 0x0046D968 File Offset: 0x0046BB68
		public List<MonumentPartResource> GetParts(MonumentPartResource.Part part)
		{
			return this.resources.FindAll((MonumentPartResource mpr) => mpr.part == part);
		}
	}
}
