using System;
using System.Collections.Generic;
using System.Linq;
using Database;
using UnityEngine;

// Token: 0x020020A5 RID: 8357
public class UIMannequin : KMonoBehaviour, UIMinionOrMannequin.ITarget
{
	// Token: 0x17000B65 RID: 2917
	// (get) Token: 0x0600B22D RID: 45613 RVA: 0x00118559 File Offset: 0x00116759
	public GameObject SpawnedAvatar
	{
		get
		{
			if (this.spawn == null)
			{
				this.TrySpawn();
			}
			return this.spawn;
		}
	}

	// Token: 0x17000B66 RID: 2918
	// (get) Token: 0x0600B22E RID: 45614 RVA: 0x0043C820 File Offset: 0x0043AA20
	public Option<Personality> Personality
	{
		get
		{
			return default(Option<Personality>);
		}
	}

	// Token: 0x0600B22F RID: 45615 RVA: 0x00118575 File Offset: 0x00116775
	protected override void OnSpawn()
	{
		this.TrySpawn();
	}

	// Token: 0x0600B230 RID: 45616 RVA: 0x0043C838 File Offset: 0x0043AA38
	public void TrySpawn()
	{
		if (this.animController == null)
		{
			this.animController = Util.KInstantiateUI(Assets.GetPrefab(MannequinUIPortrait.ID), base.gameObject, false).GetComponent<KBatchedAnimController>();
			this.animController.LoadAnims();
			this.animController.gameObject.SetActive(true);
			this.animController.animScale = 0.38f;
			this.animController.Play("idle", KAnim.PlayMode.Paused, 1f, 0f);
			this.spawn = this.animController.gameObject;
			BaseMinionConfig.ConfigureSymbols(this.spawn, false);
			base.gameObject.AddOrGet<MinionVoiceProviderMB>().voice = Option.None;
		}
	}

	// Token: 0x0600B231 RID: 45617 RVA: 0x0043C900 File Offset: 0x0043AB00
	public void SetOutfit(ClothingOutfitUtility.OutfitType outfitType, IEnumerable<ClothingItemResource> outfit)
	{
		bool flag = outfit.Count<ClothingItemResource>() == 0;
		if (this.shouldShowOutfitWithDefaultItems)
		{
			outfit = UIMinionOrMannequinITargetExtensions.GetOutfitWithDefaultItems(outfitType, outfit);
		}
		this.SpawnedAvatar.GetComponent<SymbolOverrideController>().RemoveAllSymbolOverrides(0);
		BaseMinionConfig.ConfigureSymbols(this.SpawnedAvatar, false);
		Accessorizer component = this.SpawnedAvatar.GetComponent<Accessorizer>();
		WearableAccessorizer component2 = this.SpawnedAvatar.GetComponent<WearableAccessorizer>();
		component.ApplyMinionPersonality(this.personalityToUseForDefaultClothing.UnwrapOr(Db.Get().Personalities.Get("ABE"), null));
		component2.ClearClothingItems(null);
		component2.ApplyClothingItems(outfitType, outfit);
		List<KAnimHashedString> list = new List<KAnimHashedString>(32);
		if (this.shouldShowOutfitWithDefaultItems && outfitType == ClothingOutfitUtility.OutfitType.Clothing)
		{
			list.Add("foot");
			list.Add("hand_paint");
			if (flag)
			{
				list.Add("belt");
			}
			if (!(from item in outfit
			select item.Category).Contains(PermitCategory.DupeTops))
			{
				list.Add("torso");
				list.Add("neck");
				list.Add("arm_lower");
				list.Add("arm_lower_sleeve");
				list.Add("arm_sleeve");
				list.Add("cuff");
			}
			if (!(from item in outfit
			select item.Category).Contains(PermitCategory.DupeGloves))
			{
				list.Add("arm_lower_sleeve");
				list.Add("cuff");
			}
			if (!(from item in outfit
			select item.Category).Contains(PermitCategory.DupeBottoms))
			{
				list.Add("leg");
				list.Add("pelvis");
			}
		}
		KAnimHashedString[] source = outfit.SelectMany((ClothingItemResource item) => from s in item.AnimFile.GetData().build.symbols
		select s.hash).Concat(list).ToArray<KAnimHashedString>();
		foreach (KAnim.Build.Symbol symbol in this.animController.AnimFiles[0].GetData().build.symbols)
		{
			if (symbol.hash == "mannequin_arm" || symbol.hash == "mannequin_body" || symbol.hash == "mannequin_headshape" || symbol.hash == "mannequin_leg")
			{
				this.animController.SetSymbolVisiblity(symbol.hash, true);
			}
			else
			{
				this.animController.SetSymbolVisiblity(symbol.hash, source.Contains(symbol.hash));
			}
		}
	}

	// Token: 0x0600B232 RID: 45618 RVA: 0x0043CC10 File Offset: 0x0043AE10
	private static ClothingItemResource GetItemForCategory(PermitCategory category, IEnumerable<ClothingItemResource> outfit)
	{
		foreach (ClothingItemResource clothingItemResource in outfit)
		{
			if (clothingItemResource.Category == category)
			{
				return clothingItemResource;
			}
		}
		return null;
	}

	// Token: 0x0600B233 RID: 45619 RVA: 0x0011857D File Offset: 0x0011677D
	public void React(UIMinionOrMannequinReactSource source)
	{
		this.animController.Play("idle", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x04008CB3 RID: 36019
	public const float ANIM_SCALE = 0.38f;

	// Token: 0x04008CB4 RID: 36020
	private KBatchedAnimController animController;

	// Token: 0x04008CB5 RID: 36021
	private GameObject spawn;

	// Token: 0x04008CB6 RID: 36022
	public bool shouldShowOutfitWithDefaultItems = true;

	// Token: 0x04008CB7 RID: 36023
	public Option<Personality> personalityToUseForDefaultClothing;
}
