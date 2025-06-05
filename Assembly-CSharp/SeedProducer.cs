using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02001216 RID: 4630
[AddComponentMenu("KMonoBehaviour/scripts/SeedProducer")]
public class SeedProducer : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x06005DE6 RID: 24038 RVA: 0x000E1B40 File Offset: 0x000DFD40
	public void Configure(string SeedID, SeedProducer.ProductionType productionType, int newSeedsProduced = 1)
	{
		this.seedInfo.seedId = SeedID;
		this.seedInfo.productionType = productionType;
		this.seedInfo.newSeedsProduced = newSeedsProduced;
	}

	// Token: 0x06005DE7 RID: 24039 RVA: 0x000E1B66 File Offset: 0x000DFD66
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<SeedProducer>(-216549700, SeedProducer.DropSeedDelegate);
		base.Subscribe<SeedProducer>(1623392196, SeedProducer.DropSeedDelegate);
		base.Subscribe<SeedProducer>(-1072826864, SeedProducer.CropPickedDelegate);
	}

	// Token: 0x06005DE8 RID: 24040 RVA: 0x002AE3F0 File Offset: 0x002AC5F0
	private GameObject ProduceSeed(string seedId, int units = 1, bool canMutate = true)
	{
		if (seedId != null && units > 0)
		{
			Vector3 position = base.gameObject.transform.GetPosition() + new Vector3(0f, 0.5f, 0f);
			GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(new Tag(seedId)), position, Grid.SceneLayer.Ore, null, 0);
			MutantPlant component = base.GetComponent<MutantPlant>();
			if (component != null)
			{
				MutantPlant component2 = gameObject.GetComponent<MutantPlant>();
				bool flag = false;
				if (canMutate && component2 != null && component2.IsOriginal)
				{
					flag = this.RollForMutation();
				}
				if (flag)
				{
					component2.Mutate();
				}
				else
				{
					component.CopyMutationsTo(component2);
				}
			}
			PrimaryElement component3 = base.gameObject.GetComponent<PrimaryElement>();
			PrimaryElement component4 = gameObject.GetComponent<PrimaryElement>();
			component4.Temperature = component3.Temperature;
			component4.Units = (float)units;
			base.Trigger(472291861, gameObject);
			gameObject.SetActive(true);
			string text = gameObject.GetProperName();
			if (component != null)
			{
				text = component.GetSubSpeciesInfo().GetNameWithMutations(text, component.IsIdentified, false);
			}
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, text, gameObject.transform, 1.5f, false);
			return gameObject;
		}
		return null;
	}

	// Token: 0x06005DE9 RID: 24041 RVA: 0x002AE524 File Offset: 0x002AC724
	public void DropSeed(object data = null)
	{
		if (this.droppedSeedAlready)
		{
			return;
		}
		if (this.seedInfo.newSeedsProduced <= 0)
		{
			return;
		}
		GameObject gameObject = this.ProduceSeed(this.seedInfo.seedId, this.seedInfo.newSeedsProduced, false);
		Uprootable component = base.GetComponent<Uprootable>();
		if (component != null && component.worker != null)
		{
			gameObject.Trigger(580035959, component.worker);
		}
		base.Trigger(-1736624145, gameObject);
		this.droppedSeedAlready = true;
	}

	// Token: 0x06005DEA RID: 24042 RVA: 0x000E1BA1 File Offset: 0x000DFDA1
	public void CropDepleted(object data)
	{
		this.DropSeed(null);
	}

	// Token: 0x06005DEB RID: 24043 RVA: 0x002AE5AC File Offset: 0x002AC7AC
	public void CropPicked(object data)
	{
		if (this.seedInfo.productionType == SeedProducer.ProductionType.Harvest)
		{
			WorkerBase completed_by = base.GetComponent<Harvestable>().completed_by;
			float num = 0.1f;
			if (completed_by != null)
			{
				num += completed_by.GetComponent<AttributeConverters>().Get(Db.Get().AttributeConverters.SeedHarvestChance).Evaluate();
			}
			int num2 = (UnityEngine.Random.Range(0f, 1f) <= num) ? 1 : 0;
			if (num2 > 0)
			{
				this.ProduceSeed(this.seedInfo.seedId, num2, true).Trigger(580035959, completed_by);
			}
		}
	}

	// Token: 0x06005DEC RID: 24044 RVA: 0x002AE640 File Offset: 0x002AC840
	public bool RollForMutation()
	{
		AttributeInstance attributeInstance = Db.Get().PlantAttributes.MaxRadiationThreshold.Lookup(this);
		int num = Grid.PosToCell(base.gameObject);
		float num2 = Mathf.Clamp(Grid.IsValidCell(num) ? Grid.Radiation[num] : 0f, 0f, attributeInstance.GetTotalValue()) / attributeInstance.GetTotalValue() * 0.8f;
		return UnityEngine.Random.value < num2;
	}

	// Token: 0x06005DED RID: 24045 RVA: 0x002AE6B0 File Offset: 0x002AC8B0
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Assets.GetPrefab(new Tag(this.seedInfo.seedId)) != null;
		switch (this.seedInfo.productionType)
		{
		case SeedProducer.ProductionType.Hidden:
		case SeedProducer.ProductionType.DigOnly:
		case SeedProducer.ProductionType.Crop:
			return null;
		case SeedProducer.ProductionType.Harvest:
			list.Add(new Descriptor(UI.GAMEOBJECTEFFECTS.SEED_PRODUCTION_HARVEST, UI.GAMEOBJECTEFFECTS.TOOLTIPS.SEED_PRODUCTION_HARVEST, Descriptor.DescriptorType.Lifecycle, true));
			list.Add(new Descriptor(string.Format(UI.UISIDESCREENS.PLANTERSIDESCREEN.BONUS_SEEDS, GameUtil.GetFormattedPercent(10f, GameUtil.TimeSlice.None)), string.Format(UI.UISIDESCREENS.PLANTERSIDESCREEN.TOOLTIPS.BONUS_SEEDS, GameUtil.GetFormattedPercent(10f, GameUtil.TimeSlice.None)), Descriptor.DescriptorType.Effect, false));
			break;
		case SeedProducer.ProductionType.Fruit:
			list.Add(new Descriptor(UI.GAMEOBJECTEFFECTS.SEED_PRODUCTION_FRUIT, UI.GAMEOBJECTEFFECTS.TOOLTIPS.SEED_PRODUCTION_DIG_ONLY, Descriptor.DescriptorType.Lifecycle, true));
			break;
		case SeedProducer.ProductionType.Sterile:
			list.Add(new Descriptor(UI.GAMEOBJECTEFFECTS.MUTANT_STERILE, UI.GAMEOBJECTEFFECTS.TOOLTIPS.MUTANT_STERILE, Descriptor.DescriptorType.Effect, false));
			break;
		default:
			DebugUtil.Assert(false, "Seed producer type descriptor not specified");
			return null;
		}
		return list;
	}

	// Token: 0x04004300 RID: 17152
	public SeedProducer.SeedInfo seedInfo;

	// Token: 0x04004301 RID: 17153
	private bool droppedSeedAlready;

	// Token: 0x04004302 RID: 17154
	private static readonly EventSystem.IntraObjectHandler<SeedProducer> DropSeedDelegate = new EventSystem.IntraObjectHandler<SeedProducer>(delegate(SeedProducer component, object data)
	{
		component.DropSeed(data);
	});

	// Token: 0x04004303 RID: 17155
	private static readonly EventSystem.IntraObjectHandler<SeedProducer> CropPickedDelegate = new EventSystem.IntraObjectHandler<SeedProducer>(delegate(SeedProducer component, object data)
	{
		component.CropPicked(data);
	});

	// Token: 0x02001217 RID: 4631
	[Serializable]
	public struct SeedInfo
	{
		// Token: 0x04004304 RID: 17156
		public string seedId;

		// Token: 0x04004305 RID: 17157
		public SeedProducer.ProductionType productionType;

		// Token: 0x04004306 RID: 17158
		public int newSeedsProduced;
	}

	// Token: 0x02001218 RID: 4632
	public enum ProductionType
	{
		// Token: 0x04004308 RID: 17160
		Hidden,
		// Token: 0x04004309 RID: 17161
		DigOnly,
		// Token: 0x0400430A RID: 17162
		Harvest,
		// Token: 0x0400430B RID: 17163
		Fruit,
		// Token: 0x0400430C RID: 17164
		Sterile,
		// Token: 0x0400430D RID: 17165
		Crop
	}
}
