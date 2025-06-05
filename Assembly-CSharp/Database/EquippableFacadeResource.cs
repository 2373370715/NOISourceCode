using System;
using UnityEngine;

namespace Database
{
	// Token: 0x020021A7 RID: 8615
	public class EquippableFacadeResource : PermitResource
	{
		// Token: 0x17000BE3 RID: 3043
		// (get) Token: 0x0600B7F4 RID: 47092 RVA: 0x0011B449 File Offset: 0x00119649
		// (set) Token: 0x0600B7F5 RID: 47093 RVA: 0x0011B451 File Offset: 0x00119651
		public string BuildOverride { get; private set; }

		// Token: 0x17000BE4 RID: 3044
		// (get) Token: 0x0600B7F6 RID: 47094 RVA: 0x0011B45A File Offset: 0x0011965A
		// (set) Token: 0x0600B7F7 RID: 47095 RVA: 0x0011B462 File Offset: 0x00119662
		public string DefID { get; private set; }

		// Token: 0x17000BE5 RID: 3045
		// (get) Token: 0x0600B7F8 RID: 47096 RVA: 0x0011B46B File Offset: 0x0011966B
		// (set) Token: 0x0600B7F9 RID: 47097 RVA: 0x0011B473 File Offset: 0x00119673
		public KAnimFile AnimFile { get; private set; }

		// Token: 0x0600B7FA RID: 47098 RVA: 0x0011B47C File Offset: 0x0011967C
		public EquippableFacadeResource(string id, string name, string desc, PermitRarity rarity, string buildOverride, string defID, string animFile, string[] requiredDlcIds, string[] forbiddenDlcIds) : base(id, name, desc, PermitCategory.Equipment, rarity, requiredDlcIds, forbiddenDlcIds)
		{
			this.DefID = defID;
			this.BuildOverride = buildOverride;
			this.AnimFile = Assets.GetAnim(animFile);
		}

		// Token: 0x0600B7FB RID: 47099 RVA: 0x0046A2EC File Offset: 0x004684EC
		public global::Tuple<Sprite, Color> GetUISprite()
		{
			if (this.AnimFile == null)
			{
				global::Debug.LogError("Facade AnimFile is null: " + this.DefID);
			}
			Sprite uispriteFromMultiObjectAnim = Def.GetUISpriteFromMultiObjectAnim(this.AnimFile, "ui", false, "");
			return new global::Tuple<Sprite, Color>(uispriteFromMultiObjectAnim, (uispriteFromMultiObjectAnim != null) ? Color.white : Color.clear);
		}

		// Token: 0x0600B7FC RID: 47100 RVA: 0x0046A34C File Offset: 0x0046854C
		public override PermitPresentationInfo GetPermitPresentationInfo()
		{
			PermitPresentationInfo result = default(PermitPresentationInfo);
			result.sprite = this.GetUISprite().first;
			GameObject gameObject = Assets.TryGetPrefab(this.DefID);
			if (gameObject == null || !gameObject)
			{
				result.SetFacadeForPrefabID(this.DefID);
			}
			else
			{
				result.SetFacadeForPrefabName(gameObject.GetProperName());
			}
			return result;
		}
	}
}
