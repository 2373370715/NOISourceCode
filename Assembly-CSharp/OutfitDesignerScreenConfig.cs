using System;
using UnityEngine;

// Token: 0x02001ED4 RID: 7892
public readonly struct OutfitDesignerScreenConfig
{
	// Token: 0x0600A590 RID: 42384 RVA: 0x003FA4D0 File Offset: 0x003F86D0
	public OutfitDesignerScreenConfig(ClothingOutfitTarget sourceTarget, Option<Personality> minionPersonality, Option<GameObject> targetMinionInstance, Action<ClothingOutfitTarget> onWriteToOutfitTargetFn = null)
	{
		this.sourceTarget = sourceTarget;
		this.outfitTemplate = (sourceTarget.IsTemplateOutfit() ? Option.Some<ClothingOutfitTarget>(sourceTarget) : Option.None);
		this.minionPersonality = minionPersonality;
		this.targetMinionInstance = targetMinionInstance;
		this.onWriteToOutfitTargetFn = onWriteToOutfitTargetFn;
		this.isValid = true;
		ClothingOutfitTarget.MinionInstance minionInstance;
		if (sourceTarget.Is<ClothingOutfitTarget.MinionInstance>(out minionInstance))
		{
			global::Debug.Assert(targetMinionInstance.HasValue && targetMinionInstance == minionInstance.minionInstance);
		}
	}

	// Token: 0x0600A591 RID: 42385 RVA: 0x0010FD87 File Offset: 0x0010DF87
	public OutfitDesignerScreenConfig WithOutfit(ClothingOutfitTarget sourceTarget)
	{
		return new OutfitDesignerScreenConfig(sourceTarget, this.minionPersonality, this.targetMinionInstance, this.onWriteToOutfitTargetFn);
	}

	// Token: 0x0600A592 RID: 42386 RVA: 0x0010FDA1 File Offset: 0x0010DFA1
	public OutfitDesignerScreenConfig OnWriteToOutfitTarget(Action<ClothingOutfitTarget> onWriteToOutfitTargetFn)
	{
		return new OutfitDesignerScreenConfig(this.sourceTarget, this.minionPersonality, this.targetMinionInstance, onWriteToOutfitTargetFn);
	}

	// Token: 0x0600A593 RID: 42387 RVA: 0x0010FDBB File Offset: 0x0010DFBB
	public static OutfitDesignerScreenConfig Mannequin(ClothingOutfitTarget outfit)
	{
		return new OutfitDesignerScreenConfig(outfit, Option.None, Option.None, null);
	}

	// Token: 0x0600A594 RID: 42388 RVA: 0x0010FDD8 File Offset: 0x0010DFD8
	public static OutfitDesignerScreenConfig Minion(ClothingOutfitTarget outfit, Personality personality)
	{
		return new OutfitDesignerScreenConfig(outfit, personality, Option.None, null);
	}

	// Token: 0x0600A595 RID: 42389 RVA: 0x003FA54C File Offset: 0x003F874C
	public static OutfitDesignerScreenConfig Minion(ClothingOutfitTarget outfit, GameObject targetMinionInstance)
	{
		Personality value = Db.Get().Personalities.Get(targetMinionInstance.GetComponent<MinionIdentity>().personalityResourceId);
		ClothingOutfitTarget.MinionInstance minionInstance;
		global::Debug.Assert(outfit.Is<ClothingOutfitTarget.MinionInstance>(out minionInstance));
		global::Debug.Assert(minionInstance.minionInstance == targetMinionInstance);
		return new OutfitDesignerScreenConfig(outfit, value, targetMinionInstance, null);
	}

	// Token: 0x0600A596 RID: 42390 RVA: 0x003FA5A8 File Offset: 0x003F87A8
	public static OutfitDesignerScreenConfig Minion(ClothingOutfitTarget outfit, MinionBrowserScreen.GridItem item)
	{
		MinionBrowserScreen.GridItem.PersonalityTarget personalityTarget = item as MinionBrowserScreen.GridItem.PersonalityTarget;
		if (personalityTarget != null)
		{
			return OutfitDesignerScreenConfig.Minion(outfit, personalityTarget.personality);
		}
		MinionBrowserScreen.GridItem.MinionInstanceTarget minionInstanceTarget = item as MinionBrowserScreen.GridItem.MinionInstanceTarget;
		if (minionInstanceTarget != null)
		{
			return OutfitDesignerScreenConfig.Minion(outfit, minionInstanceTarget.minionInstance);
		}
		throw new NotImplementedException();
	}

	// Token: 0x0600A597 RID: 42391 RVA: 0x0010FDF1 File Offset: 0x0010DFF1
	public void ApplyAndOpenScreen()
	{
		LockerNavigator.Instance.outfitDesignerScreen.GetComponent<OutfitDesignerScreen>().Configure(this);
		LockerNavigator.Instance.PushScreen(LockerNavigator.Instance.outfitDesignerScreen, null);
	}

	// Token: 0x04008194 RID: 33172
	public readonly ClothingOutfitTarget sourceTarget;

	// Token: 0x04008195 RID: 33173
	public readonly Option<ClothingOutfitTarget> outfitTemplate;

	// Token: 0x04008196 RID: 33174
	public readonly Option<Personality> minionPersonality;

	// Token: 0x04008197 RID: 33175
	public readonly Option<GameObject> targetMinionInstance;

	// Token: 0x04008198 RID: 33176
	public readonly Action<ClothingOutfitTarget> onWriteToOutfitTargetFn;

	// Token: 0x04008199 RID: 33177
	public readonly bool isValid;
}
