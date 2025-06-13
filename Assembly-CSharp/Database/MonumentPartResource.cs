using System;
using STRINGS;

namespace Database
{
	public class MonumentPartResource : PermitResource
	{
		public KAnimFile AnimFile { get; private set; }

		public string SymbolName { get; private set; }

		public string State { get; private set; }

		public MonumentPartResource(string id, string name, string desc, PermitRarity rarity, string animFilename, string state, string symbolName, MonumentPartResource.Part part, string[] requiredDlcIds, string[] forbiddenDlcIds) : base(id, name, desc, PermitCategory.Artwork, rarity, requiredDlcIds, forbiddenDlcIds)
		{
			this.AnimFile = Assets.GetAnim(animFilename);
			this.SymbolName = symbolName;
			this.State = state;
			this.part = part;
		}

		public override PermitPresentationInfo GetPermitPresentationInfo()
		{
			PermitPresentationInfo result = default(PermitPresentationInfo);
			result.sprite = Def.GetUISpriteFromMultiObjectAnim(this.AnimFile, "ui", false, "");
			result.SetFacadeForText(UI.KLEI_INVENTORY_SCREEN.MONUMENT_PART_FACADE_FOR);
			return result;
		}

		public MonumentPartResource.Part part;

		public enum Part
		{
			Bottom,
			Middle,
			Top
		}
	}
}
