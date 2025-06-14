﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Diet
{
	public Diet.Info[] infos { get; private set; }

	public Diet.Info[] solidEdiblesInfo { get; private set; }

	public Diet.Info[] directlyEatenPlantInfos { get; private set; }

	public Diet.Info[] preyInfos { get; private set; }

	public bool CanEatAnySolid
	{
		get
		{
			return this.solidEdiblesInfo != null && this.solidEdiblesInfo.Length != 0;
		}
	}

	public bool CanEatAnyPlantDirectly
	{
		get
		{
			return this.directlyEatenPlantInfos != null && this.directlyEatenPlantInfos.Length != 0;
		}
	}

	public bool CanEatPreyCritter
	{
		get
		{
			return this.preyInfos != null && this.preyInfos.Length != 0;
		}
	}

	public bool IsConsumedTagAbleToBeEatenDirectly(Tag tag)
	{
		if (this.directlyEatenPlantInfos == null && this.preyInfos == null)
		{
			return false;
		}
		for (int i = 0; i < this.directlyEatenPlantInfos.Length; i++)
		{
			if (this.directlyEatenPlantInfos[i].consumedTags.Contains(tag))
			{
				return true;
			}
		}
		for (int j = 0; j < this.preyInfos.Length; j++)
		{
			if (this.preyInfos[j].consumedTags.Contains(tag))
			{
				return true;
			}
		}
		return false;
	}

	private void UpdateSecondaryInfoArrays()
	{
		Diet.Info[] directlyEatenPlantInfos;
		if (this.infos != null)
		{
			directlyEatenPlantInfos = (from i in this.infos
			where i.foodType == Diet.Info.FoodType.EatPlantDirectly || i.foodType == Diet.Info.FoodType.EatPlantStorage
			select i).ToArray<Diet.Info>();
		}
		else
		{
			directlyEatenPlantInfos = null;
		}
		this.directlyEatenPlantInfos = directlyEatenPlantInfos;
		Diet.Info[] solidEdiblesInfo;
		if (this.infos != null)
		{
			solidEdiblesInfo = (from i in this.infos
			where i.foodType == Diet.Info.FoodType.EatSolid
			select i).ToArray<Diet.Info>();
		}
		else
		{
			solidEdiblesInfo = null;
		}
		this.solidEdiblesInfo = solidEdiblesInfo;
		Diet.Info[] preyInfos;
		if (this.infos != null)
		{
			preyInfos = (from i in this.infos
			where i.foodType == Diet.Info.FoodType.EatPrey || i.foodType == Diet.Info.FoodType.EatButcheredPrey
			select i).ToArray<Diet.Info>();
		}
		else
		{
			preyInfos = null;
		}
		this.preyInfos = preyInfos;
	}

	public Diet(params Diet.Info[] infos)
	{
		this.infos = infos;
		this.consumedTags = new List<KeyValuePair<Tag, float>>();
		this.producedTags = new List<KeyValuePair<Tag, float>>();
		for (int i = 0; i < infos.Length; i++)
		{
			Diet.Info info = infos[i];
			using (HashSet<Tag>.Enumerator enumerator = info.consumedTags.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Tag tag = enumerator.Current;
					if (-1 == this.consumedTags.FindIndex((KeyValuePair<Tag, float> e) => e.Key == tag))
					{
						this.consumedTags.Add(new KeyValuePair<Tag, float>(tag, info.caloriesPerKg));
					}
					if (this.consumedTagToInfo.ContainsKey(tag))
					{
						string str = "Duplicate diet entry: ";
						Tag tag2 = tag;
						global::Debug.LogError(str + tag2.ToString());
					}
					this.consumedTagToInfo[tag] = info;
				}
			}
			if (info.producedElement != Tag.Invalid && -1 == this.producedTags.FindIndex((KeyValuePair<Tag, float> e) => e.Key == info.producedElement))
			{
				this.producedTags.Add(new KeyValuePair<Tag, float>(info.producedElement, info.producedConversionRate));
			}
		}
		this.UpdateSecondaryInfoArrays();
	}

	public Diet(Diet diet)
	{
		this.infos = new Diet.Info[diet.infos.Length];
		for (int i = 0; i < diet.infos.Length; i++)
		{
			this.infos[i] = new Diet.Info(diet.infos[i]);
		}
		this.consumedTags = new List<KeyValuePair<Tag, float>>();
		this.producedTags = new List<KeyValuePair<Tag, float>>();
		Diet.Info[] infos = this.infos;
		for (int j = 0; j < infos.Length; j++)
		{
			Diet.Info info = infos[j];
			using (HashSet<Tag>.Enumerator enumerator = info.consumedTags.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Tag tag = enumerator.Current;
					if (-1 == this.consumedTags.FindIndex((KeyValuePair<Tag, float> e) => e.Key == tag))
					{
						this.consumedTags.Add(new KeyValuePair<Tag, float>(tag, info.caloriesPerKg));
					}
					if (this.consumedTagToInfo.ContainsKey(tag))
					{
						string str = "Duplicate diet entry: ";
						Tag tag2 = tag;
						global::Debug.LogError(str + tag2.ToString());
					}
					this.consumedTagToInfo[tag] = info;
				}
			}
			if (info.producedElement != Tag.Invalid && -1 == this.producedTags.FindIndex((KeyValuePair<Tag, float> e) => e.Key == info.producedElement))
			{
				this.producedTags.Add(new KeyValuePair<Tag, float>(info.producedElement, info.producedConversionRate));
			}
		}
		this.UpdateSecondaryInfoArrays();
	}

	public Diet.Info GetDietInfo(Tag tag)
	{
		Diet.Info result = null;
		this.consumedTagToInfo.TryGetValue(tag, out result);
		return result;
	}

	public float AvailableCaloriesInPrey(Tag tag)
	{
		Diet.Info dietInfo = this.GetDietInfo(tag);
		if (dietInfo == null)
		{
			return 0f;
		}
		GameObject prefab = Assets.GetPrefab(tag);
		if (dietInfo.foodType == Diet.Info.FoodType.EatPrey)
		{
			return prefab.GetComponent<PrimaryElement>().Mass * dietInfo.caloriesPerKg;
		}
		Butcherable component = prefab.GetComponent<Butcherable>();
		float num = 0f;
		if (component == null)
		{
			return 0f;
		}
		foreach (KeyValuePair<string, float> keyValuePair in component.drops)
		{
			Diet.Info dietInfo2 = this.GetDietInfo(new Tag(keyValuePair.Key));
			if (dietInfo2 != null)
			{
				num += keyValuePair.Value * dietInfo2.caloriesPerKg;
			}
		}
		return num;
	}

	public void FilterDLC()
	{
		foreach (Diet.Info info in this.infos)
		{
			List<Tag> list = new List<Tag>();
			foreach (Tag tag in info.consumedTags)
			{
				GameObject prefab = Assets.GetPrefab(tag);
				if (prefab == null || !Game.IsCorrectDlcActiveForCurrentSave(prefab.GetComponent<KPrefabID>()))
				{
					list.Add(tag);
				}
			}
			using (List<Tag>.Enumerator enumerator2 = list.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					Tag invalid_tag = enumerator2.Current;
					info.consumedTags.Remove(invalid_tag);
					this.consumedTags.RemoveAll((KeyValuePair<Tag, float> t) => t.Key == invalid_tag);
					this.consumedTagToInfo.Remove(invalid_tag);
				}
			}
			if (info.producedElement != Tag.Invalid)
			{
				GameObject prefab2 = Assets.GetPrefab(info.producedElement);
				if (prefab2 == null || !Game.IsCorrectDlcActiveForCurrentSave(prefab2.GetComponent<KPrefabID>()))
				{
					info.consumedTags.Clear();
				}
			}
		}
		this.infos = (from i in this.infos
		where i.consumedTags.Count > 0
		select i).ToArray<Diet.Info>();
		this.UpdateSecondaryInfoArrays();
	}

	public List<KeyValuePair<Tag, float>> consumedTags;

	public List<KeyValuePair<Tag, float>> producedTags;

	private Dictionary<Tag, Diet.Info> consumedTagToInfo = new Dictionary<Tag, Diet.Info>();

	public class Info
	{
		public HashSet<Tag> consumedTags { get; private set; }

		public Tag producedElement { get; private set; }

		public float caloriesPerKg { get; private set; }

		public float producedConversionRate { get; private set; }

		public byte diseaseIdx { get; private set; }

		public float diseasePerKgProduced { get; private set; }

		public bool emmitDiseaseOnCell { get; private set; }

		public bool produceSolidTile { get; private set; }

		public Diet.Info.FoodType foodType { get; private set; }

		public string[] eatAnims { get; set; }

		public Info(HashSet<Tag> consumed_tags, Tag produced_element, float calories_per_kg, float produced_conversion_rate = 1f, string disease_id = null, float disease_per_kg_produced = 0f, bool produce_solid_tile = false, Diet.Info.FoodType food_type = Diet.Info.FoodType.EatSolid, bool emmit_disease_on_cell = false, string[] eat_anims = null)
		{
			this.consumedTags = consumed_tags;
			this.producedElement = produced_element;
			this.caloriesPerKg = calories_per_kg;
			this.producedConversionRate = produced_conversion_rate;
			if (!string.IsNullOrEmpty(disease_id))
			{
				this.diseaseIdx = Db.Get().Diseases.GetIndex(disease_id);
			}
			else
			{
				this.diseaseIdx = byte.MaxValue;
			}
			this.diseasePerKgProduced = disease_per_kg_produced;
			this.emmitDiseaseOnCell = emmit_disease_on_cell;
			this.produceSolidTile = produce_solid_tile;
			this.foodType = food_type;
			if (eat_anims == null)
			{
				eat_anims = new string[]
				{
					"eat_pre",
					"eat_loop",
					"eat_pst"
				};
			}
			this.eatAnims = eat_anims;
		}

		public Info(Diet.Info info)
		{
			this.consumedTags = new HashSet<Tag>(info.consumedTags);
			this.producedElement = info.producedElement;
			this.caloriesPerKg = info.caloriesPerKg;
			this.producedConversionRate = info.producedConversionRate;
			this.diseaseIdx = info.diseaseIdx;
			this.diseasePerKgProduced = info.diseasePerKgProduced;
			this.emmitDiseaseOnCell = info.emmitDiseaseOnCell;
			this.produceSolidTile = info.produceSolidTile;
			this.foodType = info.foodType;
			this.eatAnims = info.eatAnims;
		}

		public bool IsMatch(Tag tag)
		{
			return this.consumedTags.Contains(tag);
		}

		public bool IsMatch(HashSet<Tag> tags)
		{
			if (tags.Count < this.consumedTags.Count)
			{
				foreach (Tag item in tags)
				{
					if (this.consumedTags.Contains(item))
					{
						return true;
					}
				}
				return false;
			}
			foreach (Tag item2 in this.consumedTags)
			{
				if (tags.Contains(item2))
				{
					return true;
				}
			}
			return false;
		}

		public float ConvertCaloriesToConsumptionMass(float calories)
		{
			return calories / this.caloriesPerKg;
		}

		public float ConvertConsumptionMassToCalories(float mass)
		{
			return this.caloriesPerKg * mass;
		}

		public float ConvertConsumptionMassToProducedMass(float consumed_mass)
		{
			return consumed_mass * this.producedConversionRate;
		}

		public float ConvertProducedMassToConsumptionMass(float produced_mass)
		{
			return produced_mass / this.producedConversionRate;
		}

		public enum FoodType
		{
			EatSolid,
			EatPlantDirectly,
			EatPlantStorage,
			EatPrey,
			EatButcheredPrey
		}
	}
}
