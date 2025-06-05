using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000A0A RID: 2570
public class Diet
{
	// Token: 0x170001C3 RID: 451
	// (get) Token: 0x06002EAC RID: 11948 RVA: 0x000C2B3F File Offset: 0x000C0D3F
	// (set) Token: 0x06002EAD RID: 11949 RVA: 0x000C2B47 File Offset: 0x000C0D47
	public Diet.Info[] infos { get; private set; }

	// Token: 0x170001C4 RID: 452
	// (get) Token: 0x06002EAE RID: 11950 RVA: 0x000C2B50 File Offset: 0x000C0D50
	// (set) Token: 0x06002EAF RID: 11951 RVA: 0x000C2B58 File Offset: 0x000C0D58
	public Diet.Info[] noPlantInfos { get; private set; }

	// Token: 0x170001C5 RID: 453
	// (get) Token: 0x06002EB0 RID: 11952 RVA: 0x000C2B61 File Offset: 0x000C0D61
	// (set) Token: 0x06002EB1 RID: 11953 RVA: 0x000C2B69 File Offset: 0x000C0D69
	public Diet.Info[] directlyEatenPlantInfos { get; private set; }

	// Token: 0x170001C6 RID: 454
	// (get) Token: 0x06002EB2 RID: 11954 RVA: 0x000C2B72 File Offset: 0x000C0D72
	public bool CanEatAnyNonDirectlyEdiblePlant
	{
		get
		{
			return this.noPlantInfos != null && this.noPlantInfos.Length != 0;
		}
	}

	// Token: 0x170001C7 RID: 455
	// (get) Token: 0x06002EB3 RID: 11955 RVA: 0x000C2B88 File Offset: 0x000C0D88
	public bool CanEatAnyPlantDirectly
	{
		get
		{
			return this.directlyEatenPlantInfos != null && this.directlyEatenPlantInfos.Length != 0;
		}
	}

	// Token: 0x170001C8 RID: 456
	// (get) Token: 0x06002EB4 RID: 11956 RVA: 0x000C2B9E File Offset: 0x000C0D9E
	public bool AllConsumablesAreDirectlyEdiblePlants
	{
		get
		{
			return this.CanEatAnyPlantDirectly && (this.noPlantInfos == null || this.noPlantInfos.Length == 0);
		}
	}

	// Token: 0x06002EB5 RID: 11957 RVA: 0x002031B0 File Offset: 0x002013B0
	public bool IsConsumedTagAbleToBeEatenDirectly(Tag tag)
	{
		if (this.directlyEatenPlantInfos == null)
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
		return false;
	}

	// Token: 0x06002EB6 RID: 11958 RVA: 0x002031F4 File Offset: 0x002013F4
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
		Diet.Info[] noPlantInfos;
		if (this.infos != null)
		{
			noPlantInfos = (from i in this.infos
			where i.foodType == Diet.Info.FoodType.EatSolid
			select i).ToArray<Diet.Info>();
		}
		else
		{
			noPlantInfos = null;
		}
		this.noPlantInfos = noPlantInfos;
	}

	// Token: 0x06002EB7 RID: 11959 RVA: 0x00203284 File Offset: 0x00201484
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

	// Token: 0x06002EB8 RID: 11960 RVA: 0x00203428 File Offset: 0x00201628
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

	// Token: 0x06002EB9 RID: 11961 RVA: 0x00203604 File Offset: 0x00201804
	public Diet.Info GetDietInfo(Tag tag)
	{
		Diet.Info result = null;
		this.consumedTagToInfo.TryGetValue(tag, out result);
		return result;
	}

	// Token: 0x06002EBA RID: 11962 RVA: 0x00203624 File Offset: 0x00201824
	public void FilterDLC()
	{
		foreach (Diet.Info info in this.infos)
		{
			List<Tag> list = new List<Tag>();
			foreach (Tag tag in info.consumedTags)
			{
				if (!Game.IsCorrectDlcActiveForCurrentSave(Assets.GetPrefab(tag).GetComponent<KPrefabID>()))
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
			GameObject gameObject = (info.producedElement != Tag.Invalid) ? Assets.GetPrefab(info.producedElement) : null;
			if (gameObject != null && !Game.IsCorrectDlcActiveForCurrentSave(gameObject.GetComponent<KPrefabID>()))
			{
				info.consumedTags.Clear();
			}
		}
		this.infos = (from i in this.infos
		where i.consumedTags.Count > 0
		select i).ToArray<Diet.Info>();
		this.UpdateSecondaryInfoArrays();
	}

	// Token: 0x04001FF9 RID: 8185
	public List<KeyValuePair<Tag, float>> consumedTags;

	// Token: 0x04001FFA RID: 8186
	public List<KeyValuePair<Tag, float>> producedTags;

	// Token: 0x04001FFB RID: 8187
	private Dictionary<Tag, Diet.Info> consumedTagToInfo = new Dictionary<Tag, Diet.Info>();

	// Token: 0x02000A0B RID: 2571
	public class Info
	{
		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06002EBB RID: 11963 RVA: 0x000C2BBE File Offset: 0x000C0DBE
		// (set) Token: 0x06002EBC RID: 11964 RVA: 0x000C2BC6 File Offset: 0x000C0DC6
		public HashSet<Tag> consumedTags { get; private set; }

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x06002EBD RID: 11965 RVA: 0x000C2BCF File Offset: 0x000C0DCF
		// (set) Token: 0x06002EBE RID: 11966 RVA: 0x000C2BD7 File Offset: 0x000C0DD7
		public Tag producedElement { get; private set; }

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x06002EBF RID: 11967 RVA: 0x000C2BE0 File Offset: 0x000C0DE0
		// (set) Token: 0x06002EC0 RID: 11968 RVA: 0x000C2BE8 File Offset: 0x000C0DE8
		public float caloriesPerKg { get; private set; }

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x06002EC1 RID: 11969 RVA: 0x000C2BF1 File Offset: 0x000C0DF1
		// (set) Token: 0x06002EC2 RID: 11970 RVA: 0x000C2BF9 File Offset: 0x000C0DF9
		public float producedConversionRate { get; private set; }

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x06002EC3 RID: 11971 RVA: 0x000C2C02 File Offset: 0x000C0E02
		// (set) Token: 0x06002EC4 RID: 11972 RVA: 0x000C2C0A File Offset: 0x000C0E0A
		public byte diseaseIdx { get; private set; }

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x06002EC5 RID: 11973 RVA: 0x000C2C13 File Offset: 0x000C0E13
		// (set) Token: 0x06002EC6 RID: 11974 RVA: 0x000C2C1B File Offset: 0x000C0E1B
		public float diseasePerKgProduced { get; private set; }

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x06002EC7 RID: 11975 RVA: 0x000C2C24 File Offset: 0x000C0E24
		// (set) Token: 0x06002EC8 RID: 11976 RVA: 0x000C2C2C File Offset: 0x000C0E2C
		public bool emmitDiseaseOnCell { get; private set; }

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x06002EC9 RID: 11977 RVA: 0x000C2C35 File Offset: 0x000C0E35
		// (set) Token: 0x06002ECA RID: 11978 RVA: 0x000C2C3D File Offset: 0x000C0E3D
		public bool produceSolidTile { get; private set; }

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x06002ECB RID: 11979 RVA: 0x000C2C46 File Offset: 0x000C0E46
		// (set) Token: 0x06002ECC RID: 11980 RVA: 0x000C2C4E File Offset: 0x000C0E4E
		public Diet.Info.FoodType foodType { get; private set; }

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x06002ECD RID: 11981 RVA: 0x000C2C57 File Offset: 0x000C0E57
		// (set) Token: 0x06002ECE RID: 11982 RVA: 0x000C2C5F File Offset: 0x000C0E5F
		public string[] eatAnims { get; set; }

		// Token: 0x06002ECF RID: 11983 RVA: 0x002037B8 File Offset: 0x002019B8
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

		// Token: 0x06002ED0 RID: 11984 RVA: 0x00203868 File Offset: 0x00201A68
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

		// Token: 0x06002ED1 RID: 11985 RVA: 0x000C2C68 File Offset: 0x000C0E68
		public bool IsMatch(Tag tag)
		{
			return this.consumedTags.Contains(tag);
		}

		// Token: 0x06002ED2 RID: 11986 RVA: 0x002038F8 File Offset: 0x00201AF8
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

		// Token: 0x06002ED3 RID: 11987 RVA: 0x000C2C76 File Offset: 0x000C0E76
		public float ConvertCaloriesToConsumptionMass(float calories)
		{
			return calories / this.caloriesPerKg;
		}

		// Token: 0x06002ED4 RID: 11988 RVA: 0x000C2C80 File Offset: 0x000C0E80
		public float ConvertConsumptionMassToCalories(float mass)
		{
			return this.caloriesPerKg * mass;
		}

		// Token: 0x06002ED5 RID: 11989 RVA: 0x000C2C8A File Offset: 0x000C0E8A
		public float ConvertConsumptionMassToProducedMass(float consumed_mass)
		{
			return consumed_mass * this.producedConversionRate;
		}

		// Token: 0x06002ED6 RID: 11990 RVA: 0x000C2C94 File Offset: 0x000C0E94
		public float ConvertProducedMassToConsumptionMass(float produced_mass)
		{
			return produced_mass / this.producedConversionRate;
		}

		// Token: 0x02000A0C RID: 2572
		public enum FoodType
		{
			// Token: 0x04002007 RID: 8199
			EatSolid,
			// Token: 0x04002008 RID: 8200
			EatPlantDirectly,
			// Token: 0x04002009 RID: 8201
			EatPlantStorage
		}
	}
}
