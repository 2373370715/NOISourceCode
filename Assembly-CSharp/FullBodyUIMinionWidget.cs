using System;
using System.Linq;
using Database;
using UnityEngine;

// Token: 0x02001D32 RID: 7474
public class FullBodyUIMinionWidget : KMonoBehaviour
{
	// Token: 0x17000A4B RID: 2635
	// (get) Token: 0x06009C08 RID: 39944 RVA: 0x00109FDE File Offset: 0x001081DE
	// (set) Token: 0x06009C09 RID: 39945 RVA: 0x00109FE6 File Offset: 0x001081E6
	public KBatchedAnimController animController { get; private set; }

	// Token: 0x06009C0A RID: 39946 RVA: 0x00109FEF File Offset: 0x001081EF
	protected override void OnSpawn()
	{
		this.TrySpawnDisplayMinion();
	}

	// Token: 0x06009C0B RID: 39947 RVA: 0x003CF168 File Offset: 0x003CD368
	private void TrySpawnDisplayMinion()
	{
		if (this.animController == null)
		{
			this.animController = Util.KInstantiateUI(Assets.GetPrefab(new Tag("FullMinionUIPortrait")), this.duplicantAnimAnchor.gameObject, false).GetComponent<KBatchedAnimController>();
			this.animController.gameObject.SetActive(true);
			this.animController.animScale = 0.38f;
		}
	}

	// Token: 0x06009C0C RID: 39948 RVA: 0x003CF1D0 File Offset: 0x003CD3D0
	private void InitializeAnimator()
	{
		this.TrySpawnDisplayMinion();
		this.animController.Queue("idle_default", KAnim.PlayMode.Loop, 1f, 0f);
		Accessorizer component = this.animController.GetComponent<Accessorizer>();
		for (int i = component.GetAccessories().Count - 1; i >= 0; i--)
		{
			component.RemoveAccessory(component.GetAccessories()[i].Get());
		}
	}

	// Token: 0x06009C0D RID: 39949 RVA: 0x003CF240 File Offset: 0x003CD440
	public void SetDefaultPortraitAnimator()
	{
		MinionIdentity minionIdentity = (Components.MinionIdentities.Count > 0) ? Components.MinionIdentities[0] : null;
		HashedString id = (minionIdentity != null) ? minionIdentity.personalityResourceId : Db.Get().Personalities.resources.GetRandom<Personality>().Id;
		this.InitializeAnimator();
		this.animController.GetComponent<Accessorizer>().ApplyMinionPersonality(Db.Get().Personalities.Get(id));
		Accessorizer accessorizer = (minionIdentity != null) ? minionIdentity.GetComponent<Accessorizer>() : null;
		KAnim.Build.Symbol hair_symbol = null;
		KAnim.Build.Symbol hat_hair_symbol = null;
		if (accessorizer)
		{
			hair_symbol = accessorizer.GetAccessory(Db.Get().AccessorySlots.Hair).symbol;
			hat_hair_symbol = Db.Get().AccessorySlots.HatHair.Lookup("hat_" + HashCache.Get().Get(accessorizer.GetAccessory(Db.Get().AccessorySlots.Hair).symbol.hash)).symbol;
		}
		this.UpdateHatOverride(null, hair_symbol, hat_hair_symbol);
		this.UpdateClothingOverride(this.animController.GetComponent<SymbolOverrideController>(), minionIdentity, null);
	}

	// Token: 0x06009C0E RID: 39950 RVA: 0x003CF368 File Offset: 0x003CD568
	public void SetPortraitAnimator(IAssignableIdentity assignableIdentity)
	{
		if (assignableIdentity == null || assignableIdentity.IsNull())
		{
			this.SetDefaultPortraitAnimator();
			return;
		}
		this.InitializeAnimator();
		string current_hat = "";
		MinionIdentity minionIdentity;
		StoredMinionIdentity storedMinionIdentity;
		this.GetMinionIdentity(assignableIdentity, out minionIdentity, out storedMinionIdentity);
		Accessorizer accessorizer = null;
		Accessorizer component = this.animController.GetComponent<Accessorizer>();
		KAnim.Build.Symbol hair_symbol = null;
		KAnim.Build.Symbol hat_hair_symbol = null;
		if (minionIdentity != null)
		{
			accessorizer = minionIdentity.GetComponent<Accessorizer>();
			foreach (ResourceRef<Accessory> resourceRef in accessorizer.GetAccessories())
			{
				component.AddAccessory(resourceRef.Get());
			}
			current_hat = minionIdentity.GetComponent<MinionResume>().CurrentHat;
			hair_symbol = accessorizer.GetAccessory(Db.Get().AccessorySlots.Hair).symbol;
			hat_hair_symbol = Db.Get().AccessorySlots.HatHair.Lookup("hat_" + HashCache.Get().Get(accessorizer.GetAccessory(Db.Get().AccessorySlots.Hair).symbol.hash)).symbol;
		}
		else if (storedMinionIdentity != null)
		{
			foreach (ResourceRef<Accessory> resourceRef2 in storedMinionIdentity.accessories)
			{
				component.AddAccessory(resourceRef2.Get());
			}
			current_hat = storedMinionIdentity.currentHat;
			hair_symbol = storedMinionIdentity.GetAccessory(Db.Get().AccessorySlots.Hair).symbol;
			hat_hair_symbol = Db.Get().AccessorySlots.HatHair.Lookup("hat_" + HashCache.Get().Get(storedMinionIdentity.GetAccessory(Db.Get().AccessorySlots.Hair).symbol.hash)).symbol;
		}
		this.UpdateHatOverride(current_hat, hair_symbol, hat_hair_symbol);
		this.UpdateClothingOverride(this.animController.GetComponent<SymbolOverrideController>(), minionIdentity, storedMinionIdentity);
	}

	// Token: 0x06009C0F RID: 39951 RVA: 0x003CF578 File Offset: 0x003CD778
	private void UpdateHatOverride(string current_hat, KAnim.Build.Symbol hair_symbol, KAnim.Build.Symbol hat_hair_symbol)
	{
		AccessorySlot hat = Db.Get().AccessorySlots.Hat;
		this.animController.SetSymbolVisiblity(hat.targetSymbolId, !string.IsNullOrEmpty(current_hat));
		this.animController.SetSymbolVisiblity(Db.Get().AccessorySlots.Hair.targetSymbolId, string.IsNullOrEmpty(current_hat));
		this.animController.SetSymbolVisiblity(Db.Get().AccessorySlots.HatHair.targetSymbolId, !string.IsNullOrEmpty(current_hat));
		SymbolOverrideController component = this.animController.GetComponent<SymbolOverrideController>();
		if (hair_symbol != null)
		{
			component.AddSymbolOverride("snapto_hair_always", hair_symbol, 1);
		}
		if (hat_hair_symbol != null)
		{
			component.AddSymbolOverride(Db.Get().AccessorySlots.HatHair.targetSymbolId, hat_hair_symbol, 1);
		}
	}

	// Token: 0x06009C10 RID: 39952 RVA: 0x003CF650 File Offset: 0x003CD850
	private void UpdateClothingOverride(SymbolOverrideController symbolOverrideController, MinionIdentity identity, StoredMinionIdentity storedMinionIdentity)
	{
		string[] array = null;
		if (identity != null)
		{
			array = identity.GetComponent<WearableAccessorizer>().GetClothingItemsIds(ClothingOutfitUtility.OutfitType.Clothing);
		}
		else if (storedMinionIdentity != null)
		{
			array = storedMinionIdentity.GetClothingItemIds(ClothingOutfitUtility.OutfitType.Clothing);
		}
		if (array != null)
		{
			this.animController.GetComponent<WearableAccessorizer>().ApplyClothingItems(ClothingOutfitUtility.OutfitType.Clothing, from i in array
			select Db.Get().Permits.ClothingItems.Get(i));
		}
	}

	// Token: 0x06009C11 RID: 39953 RVA: 0x00109FF7 File Offset: 0x001081F7
	public void UpdateEquipment(Equippable equippable, KAnimFile animFile)
	{
		this.animController.GetComponent<WearableAccessorizer>().ApplyEquipment(equippable, animFile);
	}

	// Token: 0x06009C12 RID: 39954 RVA: 0x0010A00B File Offset: 0x0010820B
	public void RemoveEquipment(Equippable equippable)
	{
		this.animController.GetComponent<WearableAccessorizer>().RemoveEquipment(equippable);
	}

	// Token: 0x06009C13 RID: 39955 RVA: 0x0010A01E File Offset: 0x0010821E
	private void GetMinionIdentity(IAssignableIdentity assignableIdentity, out MinionIdentity minionIdentity, out StoredMinionIdentity storedMinionIdentity)
	{
		if (assignableIdentity is MinionAssignablesProxy)
		{
			minionIdentity = ((MinionAssignablesProxy)assignableIdentity).GetTargetGameObject().GetComponent<MinionIdentity>();
			storedMinionIdentity = ((MinionAssignablesProxy)assignableIdentity).GetTargetGameObject().GetComponent<StoredMinionIdentity>();
			return;
		}
		minionIdentity = (assignableIdentity as MinionIdentity);
		storedMinionIdentity = (assignableIdentity as StoredMinionIdentity);
	}

	// Token: 0x04007A10 RID: 31248
	[SerializeField]
	private GameObject duplicantAnimAnchor;

	// Token: 0x04007A12 RID: 31250
	public const float UI_MINION_PORTRAIT_ANIM_SCALE = 0.38f;
}
