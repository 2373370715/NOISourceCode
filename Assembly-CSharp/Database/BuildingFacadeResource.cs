using System;
using System.Collections.Generic;
using UnityEngine;

namespace Database
{
	// Token: 0x0200218B RID: 8587
	public class BuildingFacadeResource : PermitResource
	{
		// Token: 0x0600B6B1 RID: 46769 RVA: 0x00459028 File Offset: 0x00457228
		[Obsolete("Please use constructor with dlcIds parameter")]
		public BuildingFacadeResource(string Id, string Name, string Description, PermitRarity Rarity, string PrefabID, string AnimFile, Dictionary<string, string> workables = null) : this(Id, Name, Description, Rarity, PrefabID, AnimFile, workables, null, null)
		{
		}

		// Token: 0x0600B6B2 RID: 46770 RVA: 0x00459048 File Offset: 0x00457248
		[Obsolete("Please use constructor with dlcIds parameter")]
		public BuildingFacadeResource(string Id, string Name, string Description, PermitRarity Rarity, string PrefabID, string AnimFile, string[] dlcIds, Dictionary<string, string> workables = null) : this(Id, Name, Description, Rarity, PrefabID, AnimFile, workables, null, null)
		{
		}

		// Token: 0x0600B6B3 RID: 46771 RVA: 0x0011AFA4 File Offset: 0x001191A4
		public BuildingFacadeResource(string Id, string Name, string Description, PermitRarity Rarity, string PrefabID, string AnimFile, Dictionary<string, string> workables = null, string[] requiredDlcIds = null, string[] forbiddenDlcIds = null) : base(Id, Name, Description, PermitCategory.Building, Rarity, requiredDlcIds, forbiddenDlcIds)
		{
			this.Id = Id;
			this.PrefabID = PrefabID;
			this.AnimFile = AnimFile;
			this.InteractFile = workables;
		}

		// Token: 0x0600B6B4 RID: 46772 RVA: 0x00459068 File Offset: 0x00457268
		public void Init()
		{
			GameObject gameObject = Assets.TryGetPrefab(this.PrefabID);
			if (gameObject == null)
			{
				return;
			}
			gameObject.AddOrGet<BuildingFacade>();
			BuildingDef def = gameObject.GetComponent<Building>().Def;
			if (def != null)
			{
				def.AddFacade(this.Id);
			}
		}

		// Token: 0x0600B6B5 RID: 46773 RVA: 0x004590B8 File Offset: 0x004572B8
		public override PermitPresentationInfo GetPermitPresentationInfo()
		{
			PermitPresentationInfo result = default(PermitPresentationInfo);
			result.sprite = Def.GetUISpriteFromMultiObjectAnim(Assets.GetAnim(this.AnimFile), "ui", false, "");
			result.SetFacadeForPrefabID(this.PrefabID);
			return result;
		}

		// Token: 0x040090DB RID: 37083
		public string PrefabID;

		// Token: 0x040090DC RID: 37084
		public string AnimFile;

		// Token: 0x040090DD RID: 37085
		public Dictionary<string, string> InteractFile;
	}
}
