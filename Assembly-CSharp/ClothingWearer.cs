using System;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020010C7 RID: 4295
[AddComponentMenu("KMonoBehaviour/scripts/ClothingWearer")]
public class ClothingWearer : KMonoBehaviour
{
	// Token: 0x06005795 RID: 22421 RVA: 0x00293FCC File Offset: 0x002921CC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.decorProvider = base.GetComponent<DecorProvider>();
		if (this.decorModifier == null)
		{
			this.decorModifier = new AttributeModifier("Decor", 0f, DUPLICANTS.MODIFIERS.CLOTHING.NAME, false, false, false);
		}
		if (this.conductivityModifier == null)
		{
			AttributeInstance attributeInstance = base.gameObject.GetAttributes().Get("ThermalConductivityBarrier");
			this.conductivityModifier = new AttributeModifier("ThermalConductivityBarrier", ClothingWearer.ClothingInfo.BASIC_CLOTHING.conductivityMod, DUPLICANTS.MODIFIERS.CLOTHING.NAME, false, false, false);
			attributeInstance.Add(this.conductivityModifier);
		}
	}

	// Token: 0x06005796 RID: 22422 RVA: 0x00294064 File Offset: 0x00292264
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.decorProvider.decor.Add(this.decorModifier);
		this.decorProvider.decorRadius.Add(new AttributeModifier(Db.Get().BuildingAttributes.DecorRadius.Id, 3f, null, false, false, true));
		Traits component = base.GetComponent<Traits>();
		string format = UI.OVERLAYS.DECOR.CLOTHING;
		if (component != null)
		{
			if (component.HasTrait("DecorUp"))
			{
				format = UI.OVERLAYS.DECOR.CLOTHING_TRAIT_DECORUP;
			}
			else if (component.HasTrait("DecorDown"))
			{
				format = UI.OVERLAYS.DECOR.CLOTHING_TRAIT_DECORDOWN;
			}
		}
		this.decorProvider.overrideName = string.Format(format, base.gameObject.GetProperName());
		if (this.currentClothing == null)
		{
			this.ChangeToDefaultClothes();
		}
		else
		{
			this.ChangeClothes(this.currentClothing);
		}
		this.spawnApplyClothesHandle = GameScheduler.Instance.Schedule("ApplySpawnClothes", 2f, delegate(object obj)
		{
			base.GetComponent<CreatureSimTemperatureTransfer>().RefreshRegistration();
		}, null, null);
	}

	// Token: 0x06005797 RID: 22423 RVA: 0x000DD985 File Offset: 0x000DBB85
	protected override void OnCleanUp()
	{
		this.spawnApplyClothesHandle.ClearScheduler();
		base.OnCleanUp();
	}

	// Token: 0x06005798 RID: 22424 RVA: 0x0029416C File Offset: 0x0029236C
	public void ChangeClothes(ClothingWearer.ClothingInfo clothingInfo)
	{
		this.decorProvider.baseRadius = 3f;
		this.currentClothing = clothingInfo;
		this.conductivityModifier.Description = clothingInfo.name;
		this.conductivityModifier.SetValue(this.currentClothing.conductivityMod);
		this.decorModifier.SetValue((float)this.currentClothing.decorMod);
	}

	// Token: 0x06005799 RID: 22425 RVA: 0x000DD998 File Offset: 0x000DBB98
	public void ChangeToDefaultClothes()
	{
		this.ChangeClothes(new ClothingWearer.ClothingInfo(ClothingWearer.ClothingInfo.BASIC_CLOTHING.name, ClothingWearer.ClothingInfo.BASIC_CLOTHING.decorMod, ClothingWearer.ClothingInfo.BASIC_CLOTHING.conductivityMod, ClothingWearer.ClothingInfo.BASIC_CLOTHING.homeostasisEfficiencyMultiplier));
	}

	// Token: 0x04003DD7 RID: 15831
	private DecorProvider decorProvider;

	// Token: 0x04003DD8 RID: 15832
	private SchedulerHandle spawnApplyClothesHandle;

	// Token: 0x04003DD9 RID: 15833
	private AttributeModifier decorModifier;

	// Token: 0x04003DDA RID: 15834
	private AttributeModifier conductivityModifier;

	// Token: 0x04003DDB RID: 15835
	[Serialize]
	public ClothingWearer.ClothingInfo currentClothing;

	// Token: 0x020010C8 RID: 4296
	public class ClothingInfo
	{
		// Token: 0x0600579C RID: 22428 RVA: 0x000DD9DA File Offset: 0x000DBBDA
		public ClothingInfo(string _name, int _decor, float _temperature, float _homeostasisEfficiencyMultiplier)
		{
			this.name = _name;
			this.decorMod = _decor;
			this.conductivityMod = _temperature;
			this.homeostasisEfficiencyMultiplier = _homeostasisEfficiencyMultiplier;
		}

		// Token: 0x0600579D RID: 22429 RVA: 0x002941D0 File Offset: 0x002923D0
		public static void OnEquipVest(Equippable eq, ClothingWearer.ClothingInfo clothingInfo)
		{
			if (eq == null || eq.assignee == null)
			{
				return;
			}
			Ownables soleOwner = eq.assignee.GetSoleOwner();
			if (soleOwner == null)
			{
				return;
			}
			ClothingWearer component = (soleOwner.GetComponent<MinionAssignablesProxy>().target as KMonoBehaviour).GetComponent<ClothingWearer>();
			if (component != null)
			{
				component.ChangeClothes(clothingInfo);
				return;
			}
			global::Debug.LogWarning("Clothing item cannot be equipped to assignee because they lack ClothingWearer component");
		}

		// Token: 0x0600579E RID: 22430 RVA: 0x00294238 File Offset: 0x00292438
		public static void OnUnequipVest(Equippable eq)
		{
			if (eq != null && eq.assignee != null)
			{
				Ownables soleOwner = eq.assignee.GetSoleOwner();
				if (soleOwner == null)
				{
					return;
				}
				MinionAssignablesProxy component = soleOwner.GetComponent<MinionAssignablesProxy>();
				if (component == null)
				{
					return;
				}
				GameObject targetGameObject = component.GetTargetGameObject();
				if (targetGameObject == null)
				{
					return;
				}
				ClothingWearer component2 = targetGameObject.GetComponent<ClothingWearer>();
				if (component2 == null)
				{
					return;
				}
				component2.ChangeToDefaultClothes();
			}
		}

		// Token: 0x0600579F RID: 22431 RVA: 0x00154BA8 File Offset: 0x00152DA8
		public static void SetupVest(GameObject go)
		{
			go.GetComponent<KPrefabID>().AddTag(GameTags.Clothes, false);
			Equippable equippable = go.GetComponent<Equippable>();
			if (equippable == null)
			{
				equippable = go.AddComponent<Equippable>();
			}
			equippable.SetQuality(global::QualityLevel.Poor);
			go.GetComponent<KBatchedAnimController>().sceneLayer = Grid.SceneLayer.BuildingBack;
		}

		// Token: 0x04003DDC RID: 15836
		[Serialize]
		public string name = "";

		// Token: 0x04003DDD RID: 15837
		[Serialize]
		public int decorMod;

		// Token: 0x04003DDE RID: 15838
		[Serialize]
		public float conductivityMod;

		// Token: 0x04003DDF RID: 15839
		[Serialize]
		public float homeostasisEfficiencyMultiplier;

		// Token: 0x04003DE0 RID: 15840
		public static readonly ClothingWearer.ClothingInfo BASIC_CLOTHING = new ClothingWearer.ClothingInfo(EQUIPMENT.PREFABS.COOL_VEST.GENERICNAME, -5, 0.0025f, -1.25f);

		// Token: 0x04003DE1 RID: 15841
		public static readonly ClothingWearer.ClothingInfo WARM_CLOTHING = new ClothingWearer.ClothingInfo(EQUIPMENT.PREFABS.WARM_VEST.NAME, 0, 0.008f, -1.25f);

		// Token: 0x04003DE2 RID: 15842
		public static readonly ClothingWearer.ClothingInfo COOL_CLOTHING = new ClothingWearer.ClothingInfo(EQUIPMENT.PREFABS.COOL_VEST.NAME, -10, 0.0005f, 0f);

		// Token: 0x04003DE3 RID: 15843
		public static readonly ClothingWearer.ClothingInfo FANCY_CLOTHING = new ClothingWearer.ClothingInfo(EQUIPMENT.PREFABS.FUNKY_VEST.NAME, 30, 0.0025f, -1.25f);

		// Token: 0x04003DE4 RID: 15844
		public static readonly ClothingWearer.ClothingInfo CUSTOM_CLOTHING = new ClothingWearer.ClothingInfo(EQUIPMENT.PREFABS.CUSTOMCLOTHING.NAME, 40, 0.0025f, -1.25f);

		// Token: 0x04003DE5 RID: 15845
		public static readonly ClothingWearer.ClothingInfo SLEEP_CLINIC_PAJAMAS = new ClothingWearer.ClothingInfo(EQUIPMENT.PREFABS.CUSTOMCLOTHING.NAME, 40, 0.0025f, -1.25f);
	}
}
