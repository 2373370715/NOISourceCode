using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020017FD RID: 6141
public class TechInstance
{
	// Token: 0x06007E58 RID: 32344 RVA: 0x000F7A74 File Offset: 0x000F5C74
	public TechInstance(Tech tech)
	{
		this.tech = tech;
	}

	// Token: 0x06007E59 RID: 32345 RVA: 0x000F7A99 File Offset: 0x000F5C99
	public bool IsComplete()
	{
		return this.complete;
	}

	// Token: 0x06007E5A RID: 32346 RVA: 0x000F7AA1 File Offset: 0x000F5CA1
	public void Purchased()
	{
		if (!this.complete)
		{
			this.complete = true;
		}
	}

	// Token: 0x06007E5B RID: 32347 RVA: 0x00336CF8 File Offset: 0x00334EF8
	public void UnlockPOITech(string tech_id)
	{
		TechItem techItem = Db.Get().TechItems.Get(tech_id);
		if (techItem == null || !techItem.isPOIUnlock)
		{
			return;
		}
		if (!this.UnlockedPOITechIds.Contains(tech_id))
		{
			this.UnlockedPOITechIds.Add(tech_id);
			BuildingDef buildingDef = Assets.GetBuildingDef(techItem.Id);
			if (buildingDef != null)
			{
				Game.Instance.Trigger(-107300940, buildingDef);
			}
		}
	}

	// Token: 0x06007E5C RID: 32348 RVA: 0x00336D64 File Offset: 0x00334F64
	public float GetTotalPercentageComplete()
	{
		float num = 0f;
		int num2 = 0;
		foreach (string type in this.progressInventory.PointsByTypeID.Keys)
		{
			if (this.tech.RequiresResearchType(type))
			{
				num += this.PercentageCompleteResearchType(type);
				num2++;
			}
		}
		return num / (float)num2;
	}

	// Token: 0x06007E5D RID: 32349 RVA: 0x000F7AB2 File Offset: 0x000F5CB2
	public float PercentageCompleteResearchType(string type)
	{
		if (!this.tech.RequiresResearchType(type))
		{
			return 1f;
		}
		return Mathf.Clamp01(this.progressInventory.PointsByTypeID[type] / this.tech.costsByResearchTypeID[type]);
	}

	// Token: 0x06007E5E RID: 32350 RVA: 0x00336DE4 File Offset: 0x00334FE4
	public TechInstance.SaveData Save()
	{
		string[] array = new string[this.progressInventory.PointsByTypeID.Count];
		this.progressInventory.PointsByTypeID.Keys.CopyTo(array, 0);
		float[] array2 = new float[this.progressInventory.PointsByTypeID.Count];
		this.progressInventory.PointsByTypeID.Values.CopyTo(array2, 0);
		string[] unlockedPOIIDs = this.UnlockedPOITechIds.ToArray();
		return new TechInstance.SaveData
		{
			techId = this.tech.Id,
			complete = this.complete,
			inventoryIDs = array,
			inventoryValues = array2,
			unlockedPOIIDs = unlockedPOIIDs
		};
	}

	// Token: 0x06007E5F RID: 32351 RVA: 0x00336E98 File Offset: 0x00335098
	public void Load(TechInstance.SaveData save_data)
	{
		this.complete = save_data.complete;
		for (int i = 0; i < save_data.inventoryIDs.Length; i++)
		{
			this.progressInventory.AddResearchPoints(save_data.inventoryIDs[i], save_data.inventoryValues[i]);
		}
		if (save_data.unlockedPOIIDs != null)
		{
			this.UnlockedPOITechIds = new List<string>(save_data.unlockedPOIIDs);
		}
	}

	// Token: 0x04006004 RID: 24580
	public Tech tech;

	// Token: 0x04006005 RID: 24581
	private bool complete;

	// Token: 0x04006006 RID: 24582
	public ResearchPointInventory progressInventory = new ResearchPointInventory();

	// Token: 0x04006007 RID: 24583
	public List<string> UnlockedPOITechIds = new List<string>();

	// Token: 0x020017FE RID: 6142
	public struct SaveData
	{
		// Token: 0x04006008 RID: 24584
		public string techId;

		// Token: 0x04006009 RID: 24585
		public bool complete;

		// Token: 0x0400600A RID: 24586
		public string[] inventoryIDs;

		// Token: 0x0400600B RID: 24587
		public float[] inventoryValues;

		// Token: 0x0400600C RID: 24588
		public string[] unlockedPOIIDs;
	}
}
