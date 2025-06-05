using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x020010DB RID: 4315
public class DirectlyEdiblePlant_TreeBranches : KMonoBehaviour, IPlantConsumptionInstructions
{
	// Token: 0x0600582E RID: 22574 RVA: 0x000DDF71 File Offset: 0x000DC171
	protected override void OnSpawn()
	{
		this.trunk = base.gameObject.GetSMI<PlantBranchGrower.Instance>();
		base.OnSpawn();
	}

	// Token: 0x0600582F RID: 22575 RVA: 0x000DDF8A File Offset: 0x000DC18A
	public bool CanPlantBeEaten()
	{
		return this.GetMaxBranchMaturity() >= this.MinimumEdibleMaturity;
	}

	// Token: 0x06005830 RID: 22576 RVA: 0x002967B8 File Offset: 0x002949B8
	public float ConsumePlant(float desiredUnitsToConsume)
	{
		float maxBranchMaturity = this.GetMaxBranchMaturity();
		float num = Mathf.Min(desiredUnitsToConsume, maxBranchMaturity);
		GameObject mostMatureBranch = this.GetMostMatureBranch();
		if (!mostMatureBranch)
		{
			return 0f;
		}
		Growing component = mostMatureBranch.GetComponent<Growing>();
		if (component)
		{
			Harvestable component2 = mostMatureBranch.GetComponent<Harvestable>();
			if (component2 != null)
			{
				component2.Trigger(2127324410, true);
			}
			component.ConsumeMass(num);
			return num;
		}
		mostMatureBranch.GetAmounts().Get(Db.Get().Amounts.Maturity.Id).ApplyDelta(-desiredUnitsToConsume);
		base.gameObject.Trigger(-1793167409, null);
		mostMatureBranch.Trigger(-1793167409, null);
		return desiredUnitsToConsume;
	}

	// Token: 0x06005831 RID: 22577 RVA: 0x00296870 File Offset: 0x00294A70
	public float PlantProductGrowthPerCycle()
	{
		Crop component = base.GetComponent<Crop>();
		string cropID = component.cropId;
		if (this.overrideCropID != null)
		{
			cropID = this.overrideCropID;
		}
		float num = CROPS.CROP_TYPES.Find((Crop.CropVal m) => m.cropId == cropID).cropDuration / 600f;
		return 1f / num;
	}

	// Token: 0x06005832 RID: 22578 RVA: 0x002968D4 File Offset: 0x00294AD4
	public float GetMaxBranchMaturity()
	{
		float max_maturity = 0f;
		GameObject max_branch = null;
		this.trunk.ActionPerBranch(delegate(GameObject branch)
		{
			if (branch != null)
			{
				AmountInstance amountInstance = Db.Get().Amounts.Maturity.Lookup(branch);
				if (amountInstance != null)
				{
					float num = amountInstance.value / amountInstance.GetMax();
					if (num > max_maturity)
					{
						max_maturity = num;
						max_branch = branch;
					}
				}
			}
		});
		return max_maturity;
	}

	// Token: 0x06005833 RID: 22579 RVA: 0x00296918 File Offset: 0x00294B18
	private GameObject GetMostMatureBranch()
	{
		float max_maturity = 0f;
		GameObject max_branch = null;
		this.trunk.ActionPerBranch(delegate(GameObject branch)
		{
			if (branch != null)
			{
				AmountInstance amountInstance = Db.Get().Amounts.Maturity.Lookup(branch);
				if (amountInstance != null)
				{
					float num = amountInstance.value / amountInstance.GetMax();
					if (num > max_maturity)
					{
						max_maturity = num;
						max_branch = branch;
					}
				}
			}
		});
		return max_branch;
	}

	// Token: 0x06005834 RID: 22580 RVA: 0x0029695C File Offset: 0x00294B5C
	public string GetFormattedConsumptionPerCycle(float consumer_KGWorthOfCaloriesLostPerSecond)
	{
		float num = this.PlantProductGrowthPerCycle();
		return GameUtil.GetFormattedPlantGrowth(consumer_KGWorthOfCaloriesLostPerSecond * num * 100f, GameUtil.TimeSlice.PerCycle);
	}

	// Token: 0x06005835 RID: 22581 RVA: 0x000AA765 File Offset: 0x000A8965
	public CellOffset[] GetAllowedOffsets()
	{
		return null;
	}

	// Token: 0x06005836 RID: 22582 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public Diet.Info.FoodType GetDietFoodType()
	{
		return Diet.Info.FoodType.EatPlantDirectly;
	}

	// Token: 0x04003E21 RID: 15905
	private PlantBranchGrower.Instance trunk;

	// Token: 0x04003E22 RID: 15906
	public float MinimumEdibleMaturity = 0.25f;

	// Token: 0x04003E23 RID: 15907
	public string overrideCropID;
}
