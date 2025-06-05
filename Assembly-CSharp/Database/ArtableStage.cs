using System;
using STRINGS;

namespace Database
{
	// Token: 0x0200217C RID: 8572
	public class ArtableStage : PermitResource
	{
		// Token: 0x0600B683 RID: 46723 RVA: 0x00457454 File Offset: 0x00455654
		[Obsolete("Use ArtableStage with required/forbidden")]
		public ArtableStage(string id, string name, string desc, PermitRarity rarity, string animFile, string anim, int decor_value, bool cheer_on_complete, ArtableStatusItem status_item, string prefabId, string symbolName, string[] dlcIds) : base(id, name, desc, PermitCategory.Artwork, rarity, null, null)
		{
			this.id = id;
			this.animFile = animFile;
			this.anim = anim;
			this.symbolName = symbolName;
			this.decor = decor_value;
			this.cheerOnComplete = cheer_on_complete;
			this.statusItem = status_item;
			this.prefabId = prefabId;
		}

		// Token: 0x0600B684 RID: 46724 RVA: 0x004574B0 File Offset: 0x004556B0
		public ArtableStage(string id, string name, string desc, PermitRarity rarity, string animFile, string anim, int decor_value, bool cheer_on_complete, ArtableStatusItem status_item, string prefabId, string symbolName, string[] requiredDlcIds, string[] forbiddenDlcIds) : base(id, name, desc, PermitCategory.Artwork, rarity, requiredDlcIds, forbiddenDlcIds)
		{
			this.id = id;
			this.animFile = animFile;
			this.anim = anim;
			this.symbolName = symbolName;
			this.decor = decor_value;
			this.cheerOnComplete = cheer_on_complete;
			this.statusItem = status_item;
			this.prefabId = prefabId;
		}

		// Token: 0x0600B685 RID: 46725 RVA: 0x00457510 File Offset: 0x00455710
		public override PermitPresentationInfo GetPermitPresentationInfo()
		{
			PermitPresentationInfo result = default(PermitPresentationInfo);
			result.sprite = Def.GetUISpriteFromMultiObjectAnim(Assets.GetAnim(this.animFile), "ui", false, "");
			result.SetFacadeForText(UI.KLEI_INVENTORY_SCREEN.ARTABLE_ITEM_FACADE_FOR.Replace("{ConfigProperName}", Assets.GetPrefab(this.prefabId).GetProperName()).Replace("{ArtableQuality}", this.statusItem.GetName(null)));
			return result;
		}

		// Token: 0x0400906B RID: 36971
		public string id;

		// Token: 0x0400906C RID: 36972
		public string anim;

		// Token: 0x0400906D RID: 36973
		public string animFile;

		// Token: 0x0400906E RID: 36974
		public string prefabId;

		// Token: 0x0400906F RID: 36975
		public string symbolName;

		// Token: 0x04009070 RID: 36976
		public int decor;

		// Token: 0x04009071 RID: 36977
		public bool cheerOnComplete;

		// Token: 0x04009072 RID: 36978
		public ArtableStatusItem statusItem;
	}
}
