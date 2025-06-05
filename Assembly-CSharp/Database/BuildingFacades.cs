using System;
using System.Collections.Generic;

namespace Database
{
	// Token: 0x0200218A RID: 8586
	public class BuildingFacades : ResourceSet<BuildingFacadeResource>
	{
		// Token: 0x0600B6AE RID: 46766 RVA: 0x00458EF0 File Offset: 0x004570F0
		public BuildingFacades(ResourceSet parent) : base("BuildingFacades", parent)
		{
			base.Initialize();
			foreach (BuildingFacadeInfo buildingFacadeInfo in Blueprints.Get().all.buildingFacades)
			{
				this.Add(buildingFacadeInfo.id, buildingFacadeInfo.name, buildingFacadeInfo.desc, buildingFacadeInfo.rarity, buildingFacadeInfo.prefabId, buildingFacadeInfo.animFile, buildingFacadeInfo.workables, buildingFacadeInfo.GetRequiredDlcIds(), buildingFacadeInfo.GetForbiddenDlcIds());
			}
		}

		// Token: 0x0600B6AF RID: 46767 RVA: 0x00458FA0 File Offset: 0x004571A0
		public void Add(string id, LocString Name, LocString Desc, PermitRarity rarity, string prefabId, string animFile, Dictionary<string, string> workables = null, string[] requiredDlcIds = null, string[] forbiddenDlcIds = null)
		{
			BuildingFacadeResource item = new BuildingFacadeResource(id, Name, Desc, rarity, prefabId, animFile, workables, requiredDlcIds, forbiddenDlcIds);
			this.resources.Add(item);
		}

		// Token: 0x0600B6B0 RID: 46768 RVA: 0x00458FD8 File Offset: 0x004571D8
		public void PostProcess()
		{
			foreach (BuildingFacadeResource buildingFacadeResource in this.resources)
			{
				buildingFacadeResource.Init();
			}
		}
	}
}
