using System;
using System.Collections.Generic;
using Database;
using UnityEngine;

// Token: 0x020020A7 RID: 8359
public class UIMinion : KMonoBehaviour, UIMinionOrMannequin.ITarget
{
	// Token: 0x17000B67 RID: 2919
	// (get) Token: 0x0600B23C RID: 45628 RVA: 0x001185FD File Offset: 0x001167FD
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

	// Token: 0x17000B68 RID: 2920
	// (get) Token: 0x0600B23D RID: 45629 RVA: 0x00118619 File Offset: 0x00116819
	// (set) Token: 0x0600B23E RID: 45630 RVA: 0x00118621 File Offset: 0x00116821
	public Option<Personality> Personality { get; private set; }

	// Token: 0x0600B23F RID: 45631 RVA: 0x0011862A File Offset: 0x0011682A
	protected override void OnSpawn()
	{
		this.TrySpawn();
	}

	// Token: 0x0600B240 RID: 45632 RVA: 0x0043CC64 File Offset: 0x0043AE64
	public void TrySpawn()
	{
		if (this.animController == null)
		{
			this.animController = Util.KInstantiateUI(Assets.GetPrefab(MinionUIPortrait.ID), base.gameObject, false).GetComponent<KBatchedAnimController>();
			this.animController.gameObject.SetActive(true);
			this.animController.animScale = 0.38f;
			this.animController.Play("idle_default", KAnim.PlayMode.Loop, 1f, 0f);
			BaseMinionConfig.ConfigureSymbols(this.animController.gameObject, true);
			this.spawn = this.animController.gameObject;
		}
	}

	// Token: 0x0600B241 RID: 45633 RVA: 0x00118632 File Offset: 0x00116832
	public void SetMinion(Personality personality)
	{
		this.SpawnedAvatar.GetComponent<Accessorizer>().ApplyMinionPersonality(personality);
		this.Personality = personality;
		base.gameObject.AddOrGet<MinionVoiceProviderMB>().voice = MinionVoice.ByPersonality(personality);
	}

	// Token: 0x0600B242 RID: 45634 RVA: 0x0043CD0C File Offset: 0x0043AF0C
	public void SetOutfit(ClothingOutfitUtility.OutfitType outfitType, IEnumerable<ClothingItemResource> outfit)
	{
		outfit = UIMinionOrMannequinITargetExtensions.GetOutfitWithDefaultItems(outfitType, outfit);
		WearableAccessorizer component = this.SpawnedAvatar.GetComponent<WearableAccessorizer>();
		component.ClearClothingItems(null);
		component.ApplyClothingItems(outfitType, outfit);
	}

	// Token: 0x0600B243 RID: 45635 RVA: 0x0043CD44 File Offset: 0x0043AF44
	public MinionVoice GetMinionVoice()
	{
		return MinionVoice.ByObject(this.SpawnedAvatar).UnwrapOr(MinionVoice.Random(), null);
	}

	// Token: 0x0600B244 RID: 45636 RVA: 0x0043CD6C File Offset: 0x0043AF6C
	public void React(UIMinionOrMannequinReactSource source)
	{
		if (source != UIMinionOrMannequinReactSource.OnPersonalityChanged && this.lastReactSource == source)
		{
			KAnim.Anim currentAnim = this.animController.GetCurrentAnim();
			if (currentAnim != null && currentAnim.name != "idle_default")
			{
				return;
			}
		}
		switch (source)
		{
		case UIMinionOrMannequinReactSource.OnPersonalityChanged:
			this.animController.Play("react", KAnim.PlayMode.Once, 1f, 0f);
			goto IL_195;
		case UIMinionOrMannequinReactSource.OnWholeOutfitChanged:
		case UIMinionOrMannequinReactSource.OnBottomChanged:
			this.animController.Play("react_bottoms", KAnim.PlayMode.Once, 1f, 0f);
			goto IL_195;
		case UIMinionOrMannequinReactSource.OnHatChanged:
			this.animController.Play("react_glasses", KAnim.PlayMode.Once, 1f, 0f);
			goto IL_195;
		case UIMinionOrMannequinReactSource.OnTopChanged:
			this.animController.Play("react_tops", KAnim.PlayMode.Once, 1f, 0f);
			goto IL_195;
		case UIMinionOrMannequinReactSource.OnGlovesChanged:
			this.animController.Play("react_gloves", KAnim.PlayMode.Once, 1f, 0f);
			goto IL_195;
		case UIMinionOrMannequinReactSource.OnShoesChanged:
			this.animController.Play("react_shoes", KAnim.PlayMode.Once, 1f, 0f);
			goto IL_195;
		}
		this.animController.Play("cheer_pre", KAnim.PlayMode.Once, 1f, 0f);
		this.animController.Queue("cheer_loop", KAnim.PlayMode.Once, 1f, 0f);
		this.animController.Queue("cheer_pst", KAnim.PlayMode.Once, 1f, 0f);
		IL_195:
		this.animController.Queue("idle_default", KAnim.PlayMode.Loop, 1f, 0f);
		this.lastReactSource = source;
	}

	// Token: 0x04008CBE RID: 36030
	public const float ANIM_SCALE = 0.38f;

	// Token: 0x04008CBF RID: 36031
	private KBatchedAnimController animController;

	// Token: 0x04008CC0 RID: 36032
	private GameObject spawn;

	// Token: 0x04008CC2 RID: 36034
	private UIMinionOrMannequinReactSource lastReactSource;
}
