using System;
using UnityEngine;

// Token: 0x02001EC3 RID: 7875
public readonly struct OutfitBrowserScreenConfig
{
	// Token: 0x0600A536 RID: 42294 RVA: 0x003F868C File Offset: 0x003F688C
	public OutfitBrowserScreenConfig(Option<ClothingOutfitUtility.OutfitType> onlyShowOutfitType, Option<ClothingOutfitTarget> selectedTarget, Option<Personality> minionPersonality, Option<GameObject> minionInstance)
	{
		this.onlyShowOutfitType = onlyShowOutfitType;
		this.selectedTarget = selectedTarget;
		this.minionPersonality = minionPersonality;
		this.isPickingOutfitForDupe = (minionPersonality.HasValue || minionInstance.HasValue);
		this.targetMinionInstance = minionInstance;
		this.isValid = true;
		if (minionPersonality.IsSome() || this.targetMinionInstance.IsSome())
		{
			global::Debug.Assert(onlyShowOutfitType.IsSome(), "If viewing outfits for a specific duplicant personality or instance, an onlyShowOutfitType must also be given.");
		}
	}

	// Token: 0x0600A537 RID: 42295 RVA: 0x0010FA22 File Offset: 0x0010DC22
	public OutfitBrowserScreenConfig WithOutfitType(Option<ClothingOutfitUtility.OutfitType> onlyShowOutfitType)
	{
		return new OutfitBrowserScreenConfig(onlyShowOutfitType, this.selectedTarget, this.minionPersonality, this.targetMinionInstance);
	}

	// Token: 0x0600A538 RID: 42296 RVA: 0x0010FA3C File Offset: 0x0010DC3C
	public OutfitBrowserScreenConfig WithOutfit(Option<ClothingOutfitTarget> sourceTarget)
	{
		return new OutfitBrowserScreenConfig(this.onlyShowOutfitType, sourceTarget, this.minionPersonality, this.targetMinionInstance);
	}

	// Token: 0x0600A539 RID: 42297 RVA: 0x003F8700 File Offset: 0x003F6900
	public string GetMinionName()
	{
		if (this.targetMinionInstance.HasValue)
		{
			return this.targetMinionInstance.Value.GetProperName();
		}
		if (this.minionPersonality.HasValue)
		{
			return this.minionPersonality.Value.Name;
		}
		return "-";
	}

	// Token: 0x0600A53A RID: 42298 RVA: 0x0010FA56 File Offset: 0x0010DC56
	public static OutfitBrowserScreenConfig Mannequin()
	{
		return new OutfitBrowserScreenConfig(Option.None, Option.None, Option.None, Option.None);
	}

	// Token: 0x0600A53B RID: 42299 RVA: 0x0010FA85 File Offset: 0x0010DC85
	public static OutfitBrowserScreenConfig Minion(ClothingOutfitUtility.OutfitType onlyShowOutfitType, Personality personality)
	{
		return new OutfitBrowserScreenConfig(onlyShowOutfitType, Option.None, personality, Option.None);
	}

	// Token: 0x0600A53C RID: 42300 RVA: 0x003F8750 File Offset: 0x003F6950
	public static OutfitBrowserScreenConfig Minion(ClothingOutfitUtility.OutfitType onlyShowOutfitType, GameObject minionInstance)
	{
		Personality value = Db.Get().Personalities.Get(minionInstance.GetComponent<MinionIdentity>().personalityResourceId);
		return new OutfitBrowserScreenConfig(onlyShowOutfitType, ClothingOutfitTarget.FromMinion(onlyShowOutfitType, minionInstance), value, minionInstance);
	}

	// Token: 0x0600A53D RID: 42301 RVA: 0x003F879C File Offset: 0x003F699C
	public static OutfitBrowserScreenConfig Minion(ClothingOutfitUtility.OutfitType onlyShowOutfitType, MinionBrowserScreen.GridItem item)
	{
		MinionBrowserScreen.GridItem.PersonalityTarget personalityTarget = item as MinionBrowserScreen.GridItem.PersonalityTarget;
		if (personalityTarget != null)
		{
			return OutfitBrowserScreenConfig.Minion(onlyShowOutfitType, personalityTarget.personality);
		}
		MinionBrowserScreen.GridItem.MinionInstanceTarget minionInstanceTarget = item as MinionBrowserScreen.GridItem.MinionInstanceTarget;
		if (minionInstanceTarget != null)
		{
			return OutfitBrowserScreenConfig.Minion(onlyShowOutfitType, minionInstanceTarget.minionInstance);
		}
		throw new NotImplementedException();
	}

	// Token: 0x0600A53E RID: 42302 RVA: 0x0010FAAC File Offset: 0x0010DCAC
	public void ApplyAndOpenScreen()
	{
		LockerNavigator.Instance.outfitBrowserScreen.GetComponent<OutfitBrowserScreen>().Configure(this);
		LockerNavigator.Instance.PushScreen(LockerNavigator.Instance.outfitBrowserScreen, null);
	}

	// Token: 0x04008132 RID: 33074
	public readonly Option<ClothingOutfitUtility.OutfitType> onlyShowOutfitType;

	// Token: 0x04008133 RID: 33075
	public readonly Option<ClothingOutfitTarget> selectedTarget;

	// Token: 0x04008134 RID: 33076
	public readonly Option<Personality> minionPersonality;

	// Token: 0x04008135 RID: 33077
	public readonly Option<GameObject> targetMinionInstance;

	// Token: 0x04008136 RID: 33078
	public readonly bool isValid;

	// Token: 0x04008137 RID: 33079
	public readonly bool isPickingOutfitForDupe;
}
