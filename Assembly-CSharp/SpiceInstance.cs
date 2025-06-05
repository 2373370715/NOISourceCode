using System;
using Klei.AI;

// Token: 0x020005C0 RID: 1472
[Serializable]
public struct SpiceInstance
{
	// Token: 0x17000093 RID: 147
	// (get) Token: 0x06001997 RID: 6551 RVA: 0x000B5494 File Offset: 0x000B3694
	public AttributeModifier CalorieModifier
	{
		get
		{
			return SpiceGrinder.SettingOptions[this.Id].Spice.CalorieModifier;
		}
	}

	// Token: 0x17000094 RID: 148
	// (get) Token: 0x06001998 RID: 6552 RVA: 0x000B54B0 File Offset: 0x000B36B0
	public AttributeModifier FoodModifier
	{
		get
		{
			return SpiceGrinder.SettingOptions[this.Id].Spice.FoodModifier;
		}
	}

	// Token: 0x17000095 RID: 149
	// (get) Token: 0x06001999 RID: 6553 RVA: 0x000B54CC File Offset: 0x000B36CC
	public Effect StatBonus
	{
		get
		{
			return SpiceGrinder.SettingOptions[this.Id].StatBonus;
		}
	}

	// Token: 0x040010A0 RID: 4256
	public Tag Id;

	// Token: 0x040010A1 RID: 4257
	public float TotalKG;
}
