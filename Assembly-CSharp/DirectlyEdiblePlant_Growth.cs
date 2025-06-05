using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x020010D7 RID: 4311
public class DirectlyEdiblePlant_Growth : KMonoBehaviour, IPlantConsumptionInstructions
{
	// Token: 0x06005817 RID: 22551 RVA: 0x00296594 File Offset: 0x00294794
	public bool CanPlantBeEaten()
	{
		float num = 0.25f;
		float num2 = 0f;
		AmountInstance amountInstance = Db.Get().Amounts.Maturity.Lookup(base.gameObject);
		if (amountInstance != null)
		{
			num2 = amountInstance.value / amountInstance.GetMax();
		}
		return num2 >= num;
	}

	// Token: 0x06005818 RID: 22552 RVA: 0x002965E0 File Offset: 0x002947E0
	public float ConsumePlant(float desiredUnitsToConsume)
	{
		AmountInstance amountInstance = Db.Get().Amounts.Maturity.Lookup(this.growing.gameObject);
		float growthUnitToMaturityRatio = this.GetGrowthUnitToMaturityRatio(amountInstance.GetMax(), base.GetComponent<KPrefabID>());
		float b = amountInstance.value * growthUnitToMaturityRatio;
		float num = Mathf.Min(desiredUnitsToConsume, b);
		this.growing.ConsumeGrowthUnits(num, growthUnitToMaturityRatio);
		return num;
	}

	// Token: 0x06005819 RID: 22553 RVA: 0x00296648 File Offset: 0x00294848
	public float PlantProductGrowthPerCycle()
	{
		Crop crop = base.GetComponent<Crop>();
		float num = CROPS.CROP_TYPES.Find((Crop.CropVal m) => m.cropId == crop.cropId).cropDuration / 600f;
		return 1f / num;
	}

	// Token: 0x0600581A RID: 22554 RVA: 0x00296690 File Offset: 0x00294890
	private float GetGrowthUnitToMaturityRatio(float maturityMax, KPrefabID prefab_id)
	{
		ResourceSet<Trait> traits = Db.Get().traits;
		Tag prefabTag = prefab_id.PrefabTag;
		Trait trait = traits.Get(prefabTag.ToString() + "Original");
		if (trait != null)
		{
			AttributeModifier attributeModifier = trait.SelfModifiers.Find((AttributeModifier match) => match.AttributeId == "MaturityMax");
			if (attributeModifier != null)
			{
				return attributeModifier.Value / maturityMax;
			}
		}
		return 1f;
	}

	// Token: 0x0600581B RID: 22555 RVA: 0x0029670C File Offset: 0x0029490C
	public string GetFormattedConsumptionPerCycle(float consumer_KGWorthOfCaloriesLostPerSecond)
	{
		float num = this.PlantProductGrowthPerCycle();
		return GameUtil.GetFormattedPlantGrowth(consumer_KGWorthOfCaloriesLostPerSecond * num * 100f, GameUtil.TimeSlice.PerCycle);
	}

	// Token: 0x0600581C RID: 22556 RVA: 0x000AA765 File Offset: 0x000A8965
	public CellOffset[] GetAllowedOffsets()
	{
		return null;
	}

	// Token: 0x0600581D RID: 22557 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public Diet.Info.FoodType GetDietFoodType()
	{
		return Diet.Info.FoodType.EatPlantDirectly;
	}

	// Token: 0x04003E16 RID: 15894
	[MyCmpGet]
	private Growing growing;
}
