using System;
using STRINGS;

namespace Database
{
	// Token: 0x020021B1 RID: 8625
	public class MonumentPartResource : PermitResource
	{
		// Token: 0x17000BE6 RID: 3046
		// (get) Token: 0x0600B836 RID: 47158 RVA: 0x0011B578 File Offset: 0x00119778
		// (set) Token: 0x0600B837 RID: 47159 RVA: 0x0011B580 File Offset: 0x00119780
		public KAnimFile AnimFile { get; private set; }

		// Token: 0x17000BE7 RID: 3047
		// (get) Token: 0x0600B838 RID: 47160 RVA: 0x0011B589 File Offset: 0x00119789
		// (set) Token: 0x0600B839 RID: 47161 RVA: 0x0011B591 File Offset: 0x00119791
		public string SymbolName { get; private set; }

		// Token: 0x17000BE8 RID: 3048
		// (get) Token: 0x0600B83A RID: 47162 RVA: 0x0011B59A File Offset: 0x0011979A
		// (set) Token: 0x0600B83B RID: 47163 RVA: 0x0011B5A2 File Offset: 0x001197A2
		public string State { get; private set; }

		// Token: 0x0600B83C RID: 47164 RVA: 0x0011B5AB File Offset: 0x001197AB
		public MonumentPartResource(string id, string name, string desc, PermitRarity rarity, string animFilename, string state, string symbolName, MonumentPartResource.Part part, string[] requiredDlcIds, string[] forbiddenDlcIds) : base(id, name, desc, PermitCategory.Artwork, rarity, requiredDlcIds, forbiddenDlcIds)
		{
			this.AnimFile = Assets.GetAnim(animFilename);
			this.SymbolName = symbolName;
			this.State = state;
			this.part = part;
		}

		// Token: 0x0600B83D RID: 47165 RVA: 0x0046D99C File Offset: 0x0046BB9C
		public override PermitPresentationInfo GetPermitPresentationInfo()
		{
			PermitPresentationInfo result = default(PermitPresentationInfo);
			result.sprite = Def.GetUISpriteFromMultiObjectAnim(this.AnimFile, "ui", false, "");
			result.SetFacadeForText(UI.KLEI_INVENTORY_SCREEN.MONUMENT_PART_FACADE_FOR);
			return result;
		}

		// Token: 0x040095C9 RID: 38345
		public MonumentPartResource.Part part;

		// Token: 0x020021B2 RID: 8626
		public enum Part
		{
			// Token: 0x040095CB RID: 38347
			Bottom,
			// Token: 0x040095CC RID: 38348
			Middle,
			// Token: 0x040095CD RID: 38349
			Top
		}
	}
}
