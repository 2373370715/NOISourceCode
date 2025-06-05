using System;

// Token: 0x02000A09 RID: 2569
public interface IPlantConsumptionInstructions
{
	// Token: 0x06002EA6 RID: 11942
	CellOffset[] GetAllowedOffsets();

	// Token: 0x06002EA7 RID: 11943
	float ConsumePlant(float desiredUnitsToConsume);

	// Token: 0x06002EA8 RID: 11944
	float PlantProductGrowthPerCycle();

	// Token: 0x06002EA9 RID: 11945
	bool CanPlantBeEaten();

	// Token: 0x06002EAA RID: 11946
	string GetFormattedConsumptionPerCycle(float consumer_caloriesLossPerCaloriesPerKG);

	// Token: 0x06002EAB RID: 11947
	Diet.Info.FoodType GetDietFoodType();
}
