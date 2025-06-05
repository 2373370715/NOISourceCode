using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using Database;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02001996 RID: 6550
[SerializationConfig(MemberSerialization.OptIn)]
[DebuggerDisplay("{id}: {type} at distance {distance}")]
public class SpaceDestination
{
	// Token: 0x06008868 RID: 34920 RVA: 0x00362AB0 File Offset: 0x00360CB0
	private static global::Tuple<SimHashes, MathUtil.MinMax> GetRareElement(SimHashes id)
	{
		foreach (global::Tuple<SimHashes, MathUtil.MinMax> tuple in SpaceDestination.RARE_ELEMENTS)
		{
			if (tuple.first == id)
			{
				return tuple;
			}
		}
		return null;
	}

	// Token: 0x170008F8 RID: 2296
	// (get) Token: 0x06008869 RID: 34921 RVA: 0x000FDC82 File Offset: 0x000FBE82
	public int OneBasedDistance
	{
		get
		{
			return this.distance + 1;
		}
	}

	// Token: 0x170008F9 RID: 2297
	// (get) Token: 0x0600886A RID: 34922 RVA: 0x000FDC8C File Offset: 0x000FBE8C
	public float CurrentMass
	{
		get
		{
			return (float)this.GetDestinationType().minimumMass + this.availableMass;
		}
	}

	// Token: 0x170008FA RID: 2298
	// (get) Token: 0x0600886B RID: 34923 RVA: 0x000FDCA1 File Offset: 0x000FBEA1
	public float AvailableMass
	{
		get
		{
			return this.availableMass;
		}
	}

	// Token: 0x0600886C RID: 34924 RVA: 0x00362B0C File Offset: 0x00360D0C
	public SpaceDestination(int id, string type, int distance)
	{
		this.id = id;
		this.type = type;
		this.distance = distance;
		SpaceDestinationType destinationType = this.GetDestinationType();
		this.availableMass = (float)(destinationType.maxiumMass - destinationType.minimumMass);
		this.GenerateSurfaceElements();
		this.GenerateResearchOpportunities();
	}

	// Token: 0x0600886D RID: 34925 RVA: 0x00362B88 File Offset: 0x00360D88
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 9))
		{
			SpaceDestinationType destinationType = this.GetDestinationType();
			this.availableMass = (float)(destinationType.maxiumMass - destinationType.minimumMass);
		}
	}

	// Token: 0x0600886E RID: 34926 RVA: 0x000FDCA9 File Offset: 0x000FBEA9
	public SpaceDestinationType GetDestinationType()
	{
		return Db.Get().SpaceDestinationTypes.Get(this.type);
	}

	// Token: 0x0600886F RID: 34927 RVA: 0x00362BC8 File Offset: 0x00360DC8
	public SpaceDestination.ResearchOpportunity TryCompleteResearchOpportunity()
	{
		foreach (SpaceDestination.ResearchOpportunity researchOpportunity in this.researchOpportunities)
		{
			if (researchOpportunity.TryComplete(this))
			{
				return researchOpportunity;
			}
		}
		return null;
	}

	// Token: 0x06008870 RID: 34928 RVA: 0x00362C24 File Offset: 0x00360E24
	public void GenerateSurfaceElements()
	{
		foreach (KeyValuePair<SimHashes, MathUtil.MinMax> keyValuePair in this.GetDestinationType().elementTable)
		{
			this.recoverableElements.Add(keyValuePair.Key, UnityEngine.Random.value);
		}
	}

	// Token: 0x06008871 RID: 34929 RVA: 0x000FDCC0 File Offset: 0x000FBEC0
	public SpacecraftManager.DestinationAnalysisState AnalysisState()
	{
		return SpacecraftManager.instance.GetDestinationAnalysisState(this);
	}

	// Token: 0x06008872 RID: 34930 RVA: 0x00362C8C File Offset: 0x00360E8C
	public void GenerateResearchOpportunities()
	{
		this.researchOpportunities.Add(new SpaceDestination.ResearchOpportunity(UI.STARMAP.DESTINATIONSTUDY.UPPERATMO, ROCKETRY.DESTINATION_RESEARCH.BASIC));
		this.researchOpportunities.Add(new SpaceDestination.ResearchOpportunity(UI.STARMAP.DESTINATIONSTUDY.LOWERATMO, ROCKETRY.DESTINATION_RESEARCH.BASIC));
		this.researchOpportunities.Add(new SpaceDestination.ResearchOpportunity(UI.STARMAP.DESTINATIONSTUDY.MAGNETICFIELD, ROCKETRY.DESTINATION_RESEARCH.BASIC));
		this.researchOpportunities.Add(new SpaceDestination.ResearchOpportunity(UI.STARMAP.DESTINATIONSTUDY.SURFACE, ROCKETRY.DESTINATION_RESEARCH.BASIC));
		this.researchOpportunities.Add(new SpaceDestination.ResearchOpportunity(UI.STARMAP.DESTINATIONSTUDY.SUBSURFACE, ROCKETRY.DESTINATION_RESEARCH.BASIC));
		float num = 0f;
		foreach (global::Tuple<float, int> tuple in SpaceDestination.RARE_ELEMENT_CHANCES)
		{
			num += tuple.first;
		}
		float num2 = UnityEngine.Random.value * num;
		int num3 = 0;
		foreach (global::Tuple<float, int> tuple2 in SpaceDestination.RARE_ELEMENT_CHANCES)
		{
			num2 -= tuple2.first;
			if (num2 <= 0f)
			{
				num3 = tuple2.second;
			}
		}
		for (int i = 0; i < num3; i++)
		{
			this.researchOpportunities[UnityEngine.Random.Range(0, this.researchOpportunities.Count)].discoveredRareResource = SpaceDestination.RARE_ELEMENTS[UnityEngine.Random.Range(0, SpaceDestination.RARE_ELEMENTS.Count)].first;
		}
		if (UnityEngine.Random.value < 0.33f)
		{
			int index = UnityEngine.Random.Range(0, this.researchOpportunities.Count);
			this.researchOpportunities[index].discoveredRareItem = SpaceDestination.RARE_ITEMS[UnityEngine.Random.Range(0, SpaceDestination.RARE_ITEMS.Count)].first;
		}
	}

	// Token: 0x06008873 RID: 34931 RVA: 0x00362E84 File Offset: 0x00361084
	public float GetResourceValue(SimHashes resource, float roll)
	{
		if (this.GetDestinationType().elementTable.ContainsKey(resource))
		{
			return this.GetDestinationType().elementTable[resource].Lerp(roll);
		}
		if (SpaceDestinationTypes.extendedElementTable.ContainsKey(resource))
		{
			return SpaceDestinationTypes.extendedElementTable[resource].Lerp(roll);
		}
		return 0f;
	}

	// Token: 0x06008874 RID: 34932 RVA: 0x00362EE8 File Offset: 0x003610E8
	public Dictionary<SimHashes, float> GetMissionResourceResult(float totalCargoSpace, float reservedMass, bool solids = true, bool liquids = true, bool gasses = true)
	{
		Dictionary<SimHashes, float> dictionary = new Dictionary<SimHashes, float>();
		float num = 0f;
		foreach (KeyValuePair<SimHashes, float> keyValuePair in this.recoverableElements)
		{
			if ((ElementLoader.FindElementByHash(keyValuePair.Key).IsSolid && solids) || (ElementLoader.FindElementByHash(keyValuePair.Key).IsLiquid && liquids) || (ElementLoader.FindElementByHash(keyValuePair.Key).IsGas && gasses))
			{
				num += this.GetResourceValue(keyValuePair.Key, keyValuePair.Value);
			}
		}
		float num2 = Mathf.Min(this.CurrentMass + reservedMass - (float)this.GetDestinationType().minimumMass, totalCargoSpace);
		foreach (KeyValuePair<SimHashes, float> keyValuePair2 in this.recoverableElements)
		{
			if ((ElementLoader.FindElementByHash(keyValuePair2.Key).IsSolid && solids) || (ElementLoader.FindElementByHash(keyValuePair2.Key).IsLiquid && liquids) || (ElementLoader.FindElementByHash(keyValuePair2.Key).IsGas && gasses))
			{
				float value = num2 * (this.GetResourceValue(keyValuePair2.Key, keyValuePair2.Value) / num);
				dictionary.Add(keyValuePair2.Key, value);
			}
		}
		return dictionary;
	}

	// Token: 0x06008875 RID: 34933 RVA: 0x0036305C File Offset: 0x0036125C
	public Dictionary<Tag, int> GetRecoverableEntities()
	{
		Dictionary<Tag, int> dictionary = new Dictionary<Tag, int>();
		Dictionary<string, int> recoverableEntities = this.GetDestinationType().recoverableEntities;
		if (recoverableEntities != null)
		{
			foreach (KeyValuePair<string, int> keyValuePair in recoverableEntities)
			{
				dictionary.Add(keyValuePair.Key, keyValuePair.Value);
			}
		}
		return dictionary;
	}

	// Token: 0x06008876 RID: 34934 RVA: 0x000FDCCD File Offset: 0x000FBECD
	public Dictionary<Tag, int> GetMissionEntityResult()
	{
		return this.GetRecoverableEntities();
	}

	// Token: 0x06008877 RID: 34935 RVA: 0x003630D4 File Offset: 0x003612D4
	public float ReserveResources(CargoBay bay)
	{
		float num = 0f;
		if (bay != null)
		{
			Storage component = bay.GetComponent<Storage>();
			foreach (KeyValuePair<SimHashes, float> keyValuePair in this.recoverableElements)
			{
				if (this.HasElementType(bay.storageType))
				{
					num += component.capacityKg;
					this.availableMass = Mathf.Max(0f, this.availableMass - component.capacityKg);
					break;
				}
			}
		}
		return num;
	}

	// Token: 0x06008878 RID: 34936 RVA: 0x00363170 File Offset: 0x00361370
	public bool HasElementType(CargoBay.CargoType type)
	{
		foreach (KeyValuePair<SimHashes, float> keyValuePair in this.recoverableElements)
		{
			if ((ElementLoader.FindElementByHash(keyValuePair.Key).IsSolid && type == CargoBay.CargoType.Solids) || (ElementLoader.FindElementByHash(keyValuePair.Key).IsLiquid && type == CargoBay.CargoType.Liquids) || (ElementLoader.FindElementByHash(keyValuePair.Key).IsGas && type == CargoBay.CargoType.Gasses))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06008879 RID: 34937 RVA: 0x00363208 File Offset: 0x00361408
	public void Replenish(float dt)
	{
		SpaceDestinationType destinationType = this.GetDestinationType();
		if (this.CurrentMass < (float)destinationType.maxiumMass)
		{
			this.availableMass += destinationType.replishmentPerSim1000ms;
		}
	}

	// Token: 0x0600887A RID: 34938 RVA: 0x00363240 File Offset: 0x00361440
	public float GetAvailableResourcesPercentage(CargoBay.CargoType cargoType)
	{
		float num = 0f;
		float totalMass = this.GetTotalMass();
		foreach (KeyValuePair<SimHashes, float> keyValuePair in this.recoverableElements)
		{
			if ((ElementLoader.FindElementByHash(keyValuePair.Key).IsSolid && cargoType == CargoBay.CargoType.Solids) || (ElementLoader.FindElementByHash(keyValuePair.Key).IsLiquid && cargoType == CargoBay.CargoType.Liquids) || (ElementLoader.FindElementByHash(keyValuePair.Key).IsGas && cargoType == CargoBay.CargoType.Gasses))
			{
				num += this.GetResourceValue(keyValuePair.Key, keyValuePair.Value) / totalMass;
			}
		}
		return num;
	}

	// Token: 0x0600887B RID: 34939 RVA: 0x003632F8 File Offset: 0x003614F8
	public float GetTotalMass()
	{
		float num = 0f;
		foreach (KeyValuePair<SimHashes, float> keyValuePair in this.recoverableElements)
		{
			num += this.GetResourceValue(keyValuePair.Key, keyValuePair.Value);
		}
		return num;
	}

	// Token: 0x04006754 RID: 26452
	private const int MASS_TO_RECOVER_AMOUNT = 1000;

	// Token: 0x04006755 RID: 26453
	private static List<global::Tuple<float, int>> RARE_ELEMENT_CHANCES = new List<global::Tuple<float, int>>
	{
		new global::Tuple<float, int>(1f, 0),
		new global::Tuple<float, int>(0.33f, 1),
		new global::Tuple<float, int>(0.03f, 2)
	};

	// Token: 0x04006756 RID: 26454
	private static readonly List<global::Tuple<SimHashes, MathUtil.MinMax>> RARE_ELEMENTS = new List<global::Tuple<SimHashes, MathUtil.MinMax>>
	{
		new global::Tuple<SimHashes, MathUtil.MinMax>(SimHashes.Katairite, new MathUtil.MinMax(1f, 10f)),
		new global::Tuple<SimHashes, MathUtil.MinMax>(SimHashes.Niobium, new MathUtil.MinMax(1f, 10f)),
		new global::Tuple<SimHashes, MathUtil.MinMax>(SimHashes.Fullerene, new MathUtil.MinMax(1f, 10f)),
		new global::Tuple<SimHashes, MathUtil.MinMax>(SimHashes.Isoresin, new MathUtil.MinMax(1f, 10f))
	};

	// Token: 0x04006757 RID: 26455
	private const float RARE_ITEM_CHANCE = 0.33f;

	// Token: 0x04006758 RID: 26456
	private static readonly List<global::Tuple<string, MathUtil.MinMax>> RARE_ITEMS = new List<global::Tuple<string, MathUtil.MinMax>>
	{
		new global::Tuple<string, MathUtil.MinMax>("GeneShufflerRecharge", new MathUtil.MinMax(1f, 2f))
	};

	// Token: 0x04006759 RID: 26457
	[Serialize]
	public int id;

	// Token: 0x0400675A RID: 26458
	[Serialize]
	public string type;

	// Token: 0x0400675B RID: 26459
	public bool startAnalyzed;

	// Token: 0x0400675C RID: 26460
	[Serialize]
	public int distance;

	// Token: 0x0400675D RID: 26461
	[Serialize]
	public float activePeriod = 20f;

	// Token: 0x0400675E RID: 26462
	[Serialize]
	public float inactivePeriod = 10f;

	// Token: 0x0400675F RID: 26463
	[Serialize]
	public float startingOrbitPercentage;

	// Token: 0x04006760 RID: 26464
	[Serialize]
	public Dictionary<SimHashes, float> recoverableElements = new Dictionary<SimHashes, float>();

	// Token: 0x04006761 RID: 26465
	[Serialize]
	public List<SpaceDestination.ResearchOpportunity> researchOpportunities = new List<SpaceDestination.ResearchOpportunity>();

	// Token: 0x04006762 RID: 26466
	[Serialize]
	private float availableMass;

	// Token: 0x02001997 RID: 6551
	[SerializationConfig(MemberSerialization.OptIn)]
	public class ResearchOpportunity
	{
		// Token: 0x0600887D RID: 34941 RVA: 0x000FDCD5 File Offset: 0x000FBED5
		[OnDeserialized]
		private void OnDeserialized()
		{
			if (this.discoveredRareResource == (SimHashes)0)
			{
				this.discoveredRareResource = SimHashes.Void;
			}
			if (this.dataValue > 50)
			{
				this.dataValue = 50;
			}
		}

		// Token: 0x0600887E RID: 34942 RVA: 0x000FDCFC File Offset: 0x000FBEFC
		public ResearchOpportunity(string description, int pointValue)
		{
			this.description = description;
			this.dataValue = pointValue;
		}

		// Token: 0x0600887F RID: 34943 RVA: 0x00363460 File Offset: 0x00361660
		public bool TryComplete(SpaceDestination destination)
		{
			if (!this.completed)
			{
				this.completed = true;
				if (this.discoveredRareResource != SimHashes.Void && !destination.recoverableElements.ContainsKey(this.discoveredRareResource))
				{
					destination.recoverableElements.Add(this.discoveredRareResource, UnityEngine.Random.value);
				}
				return true;
			}
			return false;
		}

		// Token: 0x04006763 RID: 26467
		[Serialize]
		public string description;

		// Token: 0x04006764 RID: 26468
		[Serialize]
		public int dataValue;

		// Token: 0x04006765 RID: 26469
		[Serialize]
		public bool completed;

		// Token: 0x04006766 RID: 26470
		[Serialize]
		public SimHashes discoveredRareResource = SimHashes.Void;

		// Token: 0x04006767 RID: 26471
		[Serialize]
		public string discoveredRareItem;
	}
}
