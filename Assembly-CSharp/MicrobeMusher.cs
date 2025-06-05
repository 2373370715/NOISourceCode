using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x02000EE7 RID: 3815
public class MicrobeMusher : ComplexFabricator
{
	// Token: 0x06004C69 RID: 19561 RVA: 0x000CF0A6 File Offset: 0x000CD2A6
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.choreType = Db.Get().ChoreTypes.Cook;
		this.fetchChoreTypeIdHash = Db.Get().ChoreTypes.CookFetch.IdHash;
	}

	// Token: 0x06004C6A RID: 19562 RVA: 0x00270124 File Offset: 0x0026E324
	protected override void OnSpawn()
	{
		base.OnSpawn();
		GameScheduler.Instance.Schedule("WaterFetchingTutorial", 2f, delegate(object obj)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_FetchingWater, true);
		}, null, null);
		this.workable.WorkerStatusItem = Db.Get().DuplicantStatusItems.Mushing;
		this.workable.AttributeConverter = Db.Get().AttributeConverters.CookingSpeed;
		this.workable.AttributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
		this.workable.SkillExperienceSkillGroup = Db.Get().SkillGroups.Cooking.Id;
		this.workable.SkillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
		this.workable.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Behind, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target",
			"meter_ration"
		});
		this.workable.meter.meterController.SetSymbolVisiblity(MicrobeMusher.canHash, false);
		this.workable.meter.meterController.SetSymbolVisiblity(MicrobeMusher.meterRationHash, false);
	}

	// Token: 0x06004C6B RID: 19563 RVA: 0x00270250 File Offset: 0x0026E450
	protected override List<GameObject> SpawnOrderProduct(ComplexRecipe recipe)
	{
		List<GameObject> list = base.SpawnOrderProduct(recipe);
		foreach (GameObject gameObject in list)
		{
			PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
			if (component != null)
			{
				if (gameObject.PrefabID() == "MushBar")
				{
					byte index = Db.Get().Diseases.GetIndex("FoodPoisoning");
					component.AddDisease(index, 1000, "Made of mud");
				}
				if (gameObject.GetComponent<PrimaryElement>().DiseaseCount > 0)
				{
					Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_DiseaseCooking, true);
				}
			}
		}
		return list;
	}

	// Token: 0x0400357F RID: 13695
	[SerializeField]
	public Vector3 mushbarSpawnOffset = Vector3.right;

	// Token: 0x04003580 RID: 13696
	private static readonly KAnimHashedString meterRationHash = new KAnimHashedString("meter_ration");

	// Token: 0x04003581 RID: 13697
	private static readonly KAnimHashedString canHash = new KAnimHashedString("can");
}
