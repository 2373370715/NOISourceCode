using System;
using STRINGS;
using UnityEngine;

// Token: 0x020010DA RID: 4314
public class DirectlyEdiblePlant_StorageElement : KMonoBehaviour, IPlantConsumptionInstructions
{
	// Token: 0x17000525 RID: 1317
	// (get) Token: 0x06005824 RID: 22564 RVA: 0x000DDEBB File Offset: 0x000DC0BB
	public float MassGeneratedPerCycle
	{
		get
		{
			return this.rateProducedPerCycle * this.storageCapacity;
		}
	}

	// Token: 0x06005825 RID: 22565 RVA: 0x000DDECA File Offset: 0x000DC0CA
	protected override void OnPrefabInit()
	{
		this.storageCapacity = this.storage.capacityKg;
		base.OnPrefabInit();
	}

	// Token: 0x06005826 RID: 22566 RVA: 0x00296730 File Offset: 0x00294930
	public bool CanPlantBeEaten()
	{
		Tag tag = this.GetTagToConsume();
		return this.storage.GetMassAvailable(tag) / this.storage.capacityKg >= this.minimum_mass_percentageRequiredToEat;
	}

	// Token: 0x06005827 RID: 22567 RVA: 0x00296768 File Offset: 0x00294968
	public float ConsumePlant(float desiredUnitsToConsume)
	{
		if (this.storage.MassStored() <= 0f)
		{
			return 0f;
		}
		Tag tag = this.GetTagToConsume();
		float massAvailable = this.storage.GetMassAvailable(tag);
		float num = Mathf.Min(desiredUnitsToConsume, massAvailable);
		this.storage.ConsumeIgnoringDisease(tag, num);
		return num;
	}

	// Token: 0x06005828 RID: 22568 RVA: 0x000DDEE3 File Offset: 0x000DC0E3
	public float PlantProductGrowthPerCycle()
	{
		return this.MassGeneratedPerCycle;
	}

	// Token: 0x06005829 RID: 22569 RVA: 0x000DDEEB File Offset: 0x000DC0EB
	private Tag GetTagToConsume()
	{
		if (!(this.tagToConsume != Tag.Invalid))
		{
			return this.storage.items[0].GetComponent<KPrefabID>().PrefabTag;
		}
		return this.tagToConsume;
	}

	// Token: 0x0600582A RID: 22570 RVA: 0x000DDF21 File Offset: 0x000DC121
	public string GetFormattedConsumptionPerCycle(float consumer_KGWorthOfCaloriesLostPerSecond)
	{
		return string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.EDIBLE_PLANT_INTERNAL_STORAGE, GameUtil.GetFormattedMass(consumer_KGWorthOfCaloriesLostPerSecond, GameUtil.TimeSlice.PerCycle, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}"), this.tagToConsume.ProperName());
	}

	// Token: 0x0600582B RID: 22571 RVA: 0x000DDF4B File Offset: 0x000DC14B
	public CellOffset[] GetAllowedOffsets()
	{
		return this.edibleCellOffsets;
	}

	// Token: 0x0600582C RID: 22572 RVA: 0x000AA7FE File Offset: 0x000A89FE
	public Diet.Info.FoodType GetDietFoodType()
	{
		return Diet.Info.FoodType.EatPlantStorage;
	}

	// Token: 0x04003E1A RID: 15898
	public CellOffset[] edibleCellOffsets;

	// Token: 0x04003E1B RID: 15899
	public Tag tagToConsume = Tag.Invalid;

	// Token: 0x04003E1C RID: 15900
	public float rateProducedPerCycle;

	// Token: 0x04003E1D RID: 15901
	public float storageCapacity;

	// Token: 0x04003E1E RID: 15902
	[MyCmpReq]
	private Storage storage;

	// Token: 0x04003E1F RID: 15903
	[MyCmpGet]
	private KPrefabID prefabID;

	// Token: 0x04003E20 RID: 15904
	public float minimum_mass_percentageRequiredToEat = 0.25f;
}
