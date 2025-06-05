using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02001190 RID: 4496
[AddComponentMenu("KMonoBehaviour/scripts/Crop")]
public class Crop : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x1700056F RID: 1391
	// (get) Token: 0x06005B84 RID: 23428 RVA: 0x000E00AC File Offset: 0x000DE2AC
	public string cropId
	{
		get
		{
			return this.cropVal.cropId;
		}
	}

	// Token: 0x17000570 RID: 1392
	// (get) Token: 0x06005B85 RID: 23429 RVA: 0x000E00B9 File Offset: 0x000DE2B9
	// (set) Token: 0x06005B86 RID: 23430 RVA: 0x000E00C1 File Offset: 0x000DE2C1
	public Storage PlanterStorage
	{
		get
		{
			return this.planterStorage;
		}
		set
		{
			this.planterStorage = value;
		}
	}

	// Token: 0x06005B87 RID: 23431 RVA: 0x000E00CA File Offset: 0x000DE2CA
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Components.Crops.Add(this);
		this.yield = this.GetAttributes().Add(Db.Get().PlantAttributes.YieldAmount);
	}

	// Token: 0x06005B88 RID: 23432 RVA: 0x000E00FD File Offset: 0x000DE2FD
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<Crop>(1272413801, Crop.OnHarvestDelegate);
	}

	// Token: 0x06005B89 RID: 23433 RVA: 0x000E0116 File Offset: 0x000DE316
	public void Configure(Crop.CropVal cropval)
	{
		this.cropVal = cropval;
	}

	// Token: 0x06005B8A RID: 23434 RVA: 0x000E011F File Offset: 0x000DE31F
	public bool CanGrow()
	{
		return this.cropVal.renewable;
	}

	// Token: 0x06005B8B RID: 23435 RVA: 0x002A6334 File Offset: 0x002A4534
	public void SpawnConfiguredFruit(object callbackParam)
	{
		if (this == null)
		{
			return;
		}
		Crop.CropVal cropVal = this.cropVal;
		if (!string.IsNullOrEmpty(cropVal.cropId))
		{
			this.SpawnSomeFruit(cropVal.cropId, this.yield.GetTotalValue());
			base.Trigger(-1072826864, this);
		}
	}

	// Token: 0x06005B8C RID: 23436 RVA: 0x002A6388 File Offset: 0x002A4588
	public void SpawnSomeFruit(Tag cropID, float amount)
	{
		GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(cropID), base.transform.GetPosition() + new Vector3(0f, 0.75f, 0f), Grid.SceneLayer.Ore, null, 0);
		if (gameObject != null)
		{
			MutantPlant component = base.GetComponent<MutantPlant>();
			MutantPlant component2 = gameObject.GetComponent<MutantPlant>();
			if (component != null && component.IsOriginal && component2 != null && base.GetComponent<SeedProducer>().RollForMutation())
			{
				component2.Mutate();
			}
			gameObject.SetActive(true);
			PrimaryElement component3 = gameObject.GetComponent<PrimaryElement>();
			component3.Units = amount;
			component3.Temperature = base.gameObject.GetComponent<PrimaryElement>().Temperature;
			base.Trigger(35625290, gameObject);
			Edible component4 = gameObject.GetComponent<Edible>();
			if (component4)
			{
				ReportManager.Instance.ReportValue(ReportManager.ReportType.CaloriesCreated, component4.Calories, StringFormatter.Replace(UI.ENDOFDAYREPORT.NOTES.HARVESTED, "{0}", component4.GetProperName()), UI.ENDOFDAYREPORT.NOTES.HARVESTED_CONTEXT);
				return;
			}
		}
		else
		{
			DebugUtil.LogErrorArgs(base.gameObject, new object[]
			{
				"tried to spawn an invalid crop prefab:",
				cropID
			});
		}
	}

	// Token: 0x06005B8D RID: 23437 RVA: 0x000E012C File Offset: 0x000DE32C
	protected override void OnCleanUp()
	{
		Components.Crops.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x06005B8E RID: 23438 RVA: 0x000AA038 File Offset: 0x000A8238
	private void OnHarvest(object obj)
	{
	}

	// Token: 0x06005B8F RID: 23439 RVA: 0x000CE880 File Offset: 0x000CCA80
	public List<Descriptor> RequirementDescriptors(GameObject go)
	{
		return new List<Descriptor>();
	}

	// Token: 0x06005B90 RID: 23440 RVA: 0x002A64AC File Offset: 0x002A46AC
	public List<Descriptor> InformationDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Tag tag = new Tag(this.cropVal.cropId);
		GameObject prefab = Assets.GetPrefab(tag);
		Edible component = prefab.GetComponent<Edible>();
		Klei.AI.Attribute yieldAmount = Db.Get().PlantAttributes.YieldAmount;
		float preModifiedAttributeValue = go.GetComponent<Modifiers>().GetPreModifiedAttributeValue(yieldAmount);
		if (component != null)
		{
			DebugUtil.Assert(GameTags.DisplayAsCalories.Contains(tag), "Trying to display crop info for an edible fruit which isn't displayed as calories!", tag.ToString());
			float caloriesPerUnit = component.FoodInfo.CaloriesPerUnit;
			float calories = caloriesPerUnit * preModifiedAttributeValue;
			string text = GameUtil.GetFormattedCalories(calories, GameUtil.TimeSlice.None, true);
			Descriptor item = new Descriptor(string.Format(UI.UISIDESCREENS.PLANTERSIDESCREEN.YIELD, prefab.GetProperName(), text), string.Format(UI.UISIDESCREENS.PLANTERSIDESCREEN.TOOLTIPS.YIELD, "", GameUtil.GetFormattedCalories(caloriesPerUnit, GameUtil.TimeSlice.None, true), GameUtil.GetFormattedCalories(calories, GameUtil.TimeSlice.None, true)), Descriptor.DescriptorType.Effect, false);
			list.Add(item);
		}
		else
		{
			string text;
			if (GameTags.DisplayAsUnits.Contains(tag))
			{
				text = GameUtil.GetFormattedUnits((float)this.cropVal.numProduced, GameUtil.TimeSlice.None, false, "");
			}
			else
			{
				text = GameUtil.GetFormattedMass((float)this.cropVal.numProduced, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}");
			}
			Descriptor item2 = new Descriptor(string.Format(UI.UISIDESCREENS.PLANTERSIDESCREEN.YIELD_NONFOOD, prefab.GetProperName(), text), string.Format(UI.UISIDESCREENS.PLANTERSIDESCREEN.TOOLTIPS.YIELD_NONFOOD, text), Descriptor.DescriptorType.Effect, false);
			list.Add(item2);
		}
		return list;
	}

	// Token: 0x06005B91 RID: 23441 RVA: 0x002A661C File Offset: 0x002A481C
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		foreach (Descriptor item in this.RequirementDescriptors(go))
		{
			list.Add(item);
		}
		foreach (Descriptor item2 in this.InformationDescriptors(go))
		{
			list.Add(item2);
		}
		return list;
	}

	// Token: 0x04004123 RID: 16675
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04004124 RID: 16676
	public Crop.CropVal cropVal;

	// Token: 0x04004125 RID: 16677
	private AttributeInstance yield;

	// Token: 0x04004126 RID: 16678
	public string domesticatedDesc = "";

	// Token: 0x04004127 RID: 16679
	private Storage planterStorage;

	// Token: 0x04004128 RID: 16680
	private static readonly EventSystem.IntraObjectHandler<Crop> OnHarvestDelegate = new EventSystem.IntraObjectHandler<Crop>(delegate(Crop component, object data)
	{
		component.OnHarvest(data);
	});

	// Token: 0x02001191 RID: 4497
	[Serializable]
	public struct CropVal
	{
		// Token: 0x06005B94 RID: 23444 RVA: 0x000E016E File Offset: 0x000DE36E
		public CropVal(string crop_id, float crop_duration, int num_produced = 1, bool renewable = true)
		{
			this.cropId = crop_id;
			this.cropDuration = crop_duration;
			this.numProduced = num_produced;
			this.renewable = renewable;
		}

		// Token: 0x04004129 RID: 16681
		public string cropId;

		// Token: 0x0400412A RID: 16682
		public float cropDuration;

		// Token: 0x0400412B RID: 16683
		public int numProduced;

		// Token: 0x0400412C RID: 16684
		public bool renewable;
	}
}
