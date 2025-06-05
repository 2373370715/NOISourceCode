using System;
using Klei.AI;
using UnityEngine;

namespace Database
{
	// Token: 0x020021D4 RID: 8660
	public class Spice : Resource, IHasDlcRestrictions
	{
		// Token: 0x17000BEE RID: 3054
		// (get) Token: 0x0600B89E RID: 47262 RVA: 0x0011B87D File Offset: 0x00119A7D
		// (set) Token: 0x0600B89F RID: 47263 RVA: 0x0011B885 File Offset: 0x00119A85
		public AttributeModifier StatBonus { get; private set; }

		// Token: 0x17000BEF RID: 3055
		// (get) Token: 0x0600B8A0 RID: 47264 RVA: 0x0011B88E File Offset: 0x00119A8E
		// (set) Token: 0x0600B8A1 RID: 47265 RVA: 0x0011B896 File Offset: 0x00119A96
		public AttributeModifier FoodModifier { get; private set; }

		// Token: 0x17000BF0 RID: 3056
		// (get) Token: 0x0600B8A2 RID: 47266 RVA: 0x0011B89F File Offset: 0x00119A9F
		// (set) Token: 0x0600B8A3 RID: 47267 RVA: 0x0011B8A7 File Offset: 0x00119AA7
		public AttributeModifier CalorieModifier { get; private set; }

		// Token: 0x17000BF1 RID: 3057
		// (get) Token: 0x0600B8A4 RID: 47268 RVA: 0x0011B8B0 File Offset: 0x00119AB0
		// (set) Token: 0x0600B8A5 RID: 47269 RVA: 0x0011B8B8 File Offset: 0x00119AB8
		public Color PrimaryColor { get; private set; }

		// Token: 0x17000BF2 RID: 3058
		// (get) Token: 0x0600B8A6 RID: 47270 RVA: 0x0011B8C1 File Offset: 0x00119AC1
		// (set) Token: 0x0600B8A7 RID: 47271 RVA: 0x0011B8C9 File Offset: 0x00119AC9
		public Color SecondaryColor { get; private set; }

		// Token: 0x17000BF3 RID: 3059
		// (get) Token: 0x0600B8A9 RID: 47273 RVA: 0x0011B8DB File Offset: 0x00119ADB
		// (set) Token: 0x0600B8A8 RID: 47272 RVA: 0x0011B8D2 File Offset: 0x00119AD2
		public string Image { get; private set; }

		// Token: 0x17000BF4 RID: 3060
		// (get) Token: 0x0600B8AB RID: 47275 RVA: 0x0011B8EC File Offset: 0x00119AEC
		// (set) Token: 0x0600B8AA RID: 47274 RVA: 0x0011B8E3 File Offset: 0x00119AE3
		public string[] requiredDlcIds { get; private set; }

		// Token: 0x0600B8AC RID: 47276 RVA: 0x00471E6C File Offset: 0x0047006C
		public Spice(ResourceSet parent, string id, Spice.Ingredient[] ingredients, Color primaryColor, Color secondaryColor, AttributeModifier foodMod = null, AttributeModifier statBonus = null, string imageName = "unknown", string[] dlcID = null) : base(id, parent, null)
		{
			this.requiredDlcIds = this.requiredDlcIds;
			this.StatBonus = statBonus;
			this.FoodModifier = foodMod;
			this.Ingredients = ingredients;
			this.Image = imageName;
			this.PrimaryColor = primaryColor;
			this.SecondaryColor = secondaryColor;
			for (int i = 0; i < this.Ingredients.Length; i++)
			{
				this.TotalKG += this.Ingredients[i].AmountKG;
			}
		}

		// Token: 0x0600B8AD RID: 47277 RVA: 0x0011B8F4 File Offset: 0x00119AF4
		public string[] GetRequiredDlcIds()
		{
			return this.requiredDlcIds;
		}

		// Token: 0x0600B8AE RID: 47278 RVA: 0x000AA765 File Offset: 0x000A8965
		public string[] GetForbiddenDlcIds()
		{
			return null;
		}

		// Token: 0x040096C7 RID: 38599
		public readonly Spice.Ingredient[] Ingredients;

		// Token: 0x040096C8 RID: 38600
		public readonly float TotalKG;

		// Token: 0x020021D5 RID: 8661
		public class Ingredient : IConfigurableConsumerIngredient
		{
			// Token: 0x0600B8AF RID: 47279 RVA: 0x0011B8FC File Offset: 0x00119AFC
			public float GetAmount()
			{
				return this.AmountKG;
			}

			// Token: 0x0600B8B0 RID: 47280 RVA: 0x0011B904 File Offset: 0x00119B04
			public Tag[] GetIDSets()
			{
				return this.IngredientSet;
			}

			// Token: 0x040096CB RID: 38603
			public Tag[] IngredientSet;

			// Token: 0x040096CC RID: 38604
			public float AmountKG;
		}
	}
}
