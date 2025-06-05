using System;
using STRINGS;
using UnityEngine;

namespace Database
{
	// Token: 0x020021B9 RID: 8633
	public struct PermitPresentationInfo
	{
		// Token: 0x17000BE9 RID: 3049
		// (get) Token: 0x0600B84A RID: 47178 RVA: 0x0011B66C File Offset: 0x0011986C
		// (set) Token: 0x0600B84B RID: 47179 RVA: 0x0011B674 File Offset: 0x00119874
		public string facadeFor { readonly get; private set; }

		// Token: 0x0600B84C RID: 47180 RVA: 0x0011B67D File Offset: 0x0011987D
		public static Sprite GetUnknownSprite()
		{
			return Assets.GetSprite("unknown");
		}

		// Token: 0x0600B84D RID: 47181 RVA: 0x0011B68E File Offset: 0x0011988E
		public void SetFacadeForPrefabName(string prefabName)
		{
			this.facadeFor = UI.KLEI_INVENTORY_SCREEN.ITEM_FACADE_FOR.Replace("{ConfigProperName}", prefabName);
		}

		// Token: 0x0600B84E RID: 47182 RVA: 0x0046E268 File Offset: 0x0046C468
		public void SetFacadeForPrefabID(string prefabId)
		{
			if (Assets.TryGetPrefab(prefabId) == null)
			{
				this.facadeFor = UI.KLEI_INVENTORY_SCREEN.ITEM_DLC_REQUIRED;
				return;
			}
			this.facadeFor = UI.KLEI_INVENTORY_SCREEN.ITEM_FACADE_FOR.Replace("{ConfigProperName}", Assets.GetPrefab(prefabId).GetProperName());
		}

		// Token: 0x0600B84F RID: 47183 RVA: 0x0011B6A6 File Offset: 0x001198A6
		public void SetFacadeForText(string text)
		{
			this.facadeFor = text;
		}

		// Token: 0x04009602 RID: 38402
		public Sprite sprite;
	}
}
