﻿using System;
using UnityEngine;

namespace Database
{
	public class EquippableFacadeResource : PermitResource
	{
		public string BuildOverride { get; private set; }

		public string DefID { get; private set; }

		public KAnimFile AnimFile { get; private set; }

		public EquippableFacadeResource(string id, string name, string desc, PermitRarity rarity, string buildOverride, string defID, string animFile, string[] requiredDlcIds, string[] forbiddenDlcIds) : base(id, name, desc, PermitCategory.Equipment, rarity, requiredDlcIds, forbiddenDlcIds)
		{
			this.DefID = defID;
			this.BuildOverride = buildOverride;
			this.AnimFile = Assets.GetAnim(animFile);
		}

		public global::Tuple<Sprite, Color> GetUISprite()
		{
			if (this.AnimFile == null)
			{
				global::Debug.LogError("Facade AnimFile is null: " + this.DefID);
			}
			Sprite uispriteFromMultiObjectAnim = Def.GetUISpriteFromMultiObjectAnim(this.AnimFile, "ui", false, "");
			return new global::Tuple<Sprite, Color>(uispriteFromMultiObjectAnim, (uispriteFromMultiObjectAnim != null) ? Color.white : Color.clear);
		}

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
