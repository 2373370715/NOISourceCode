using System;
using System.Collections.Generic;
using Klei;
using UnityEngine;

// Token: 0x020013BD RID: 5053
[AddComponentMenu("KMonoBehaviour/scripts/GeyserConfigurator")]
public class GeyserConfigurator : KMonoBehaviour
{
	// Token: 0x060067A2 RID: 26530 RVA: 0x002E22EC File Offset: 0x002E04EC
	public static GeyserConfigurator.GeyserType FindType(HashedString typeId)
	{
		GeyserConfigurator.GeyserType geyserType = null;
		if (typeId != HashedString.Invalid)
		{
			geyserType = GeyserConfigurator.geyserTypes.Find((GeyserConfigurator.GeyserType t) => t.id == typeId);
		}
		if (geyserType == null)
		{
			global::Debug.LogError(string.Format("Tried finding a geyser with id {0} but it doesn't exist!", typeId.ToString()));
		}
		return geyserType;
	}

	// Token: 0x060067A3 RID: 26531 RVA: 0x000E829A File Offset: 0x000E649A
	public GeyserConfigurator.GeyserInstanceConfiguration MakeConfiguration()
	{
		return this.CreateRandomInstance(this.presetType, this.presetMin, this.presetMax);
	}

	// Token: 0x060067A4 RID: 26532 RVA: 0x002E2358 File Offset: 0x002E0558
	private GeyserConfigurator.GeyserInstanceConfiguration CreateRandomInstance(HashedString typeId, float min, float max)
	{
		KRandom randomSource = new KRandom(SaveLoader.Instance.clusterDetailSave.globalWorldSeed + (int)base.transform.GetPosition().x + (int)base.transform.GetPosition().y);
		return new GeyserConfigurator.GeyserInstanceConfiguration
		{
			typeId = typeId,
			rateRoll = this.Roll(randomSource, min, max),
			iterationLengthRoll = this.Roll(randomSource, 0f, 1f),
			iterationPercentRoll = this.Roll(randomSource, min, max),
			yearLengthRoll = this.Roll(randomSource, 0f, 1f),
			yearPercentRoll = this.Roll(randomSource, min, max)
		};
	}

	// Token: 0x060067A5 RID: 26533 RVA: 0x000E82B4 File Offset: 0x000E64B4
	private float Roll(KRandom randomSource, float min, float max)
	{
		return (float)(randomSource.NextDouble() * (double)(max - min)) + min;
	}

	// Token: 0x04004E45 RID: 20037
	private static List<GeyserConfigurator.GeyserType> geyserTypes;

	// Token: 0x04004E46 RID: 20038
	public HashedString presetType;

	// Token: 0x04004E47 RID: 20039
	public float presetMin;

	// Token: 0x04004E48 RID: 20040
	public float presetMax = 1f;

	// Token: 0x020013BE RID: 5054
	public enum GeyserShape
	{
		// Token: 0x04004E4A RID: 20042
		Gas,
		// Token: 0x04004E4B RID: 20043
		Liquid,
		// Token: 0x04004E4C RID: 20044
		Molten
	}

	// Token: 0x020013BF RID: 5055
	public class GeyserType : IHasDlcRestrictions
	{
		// Token: 0x060067A7 RID: 26535 RVA: 0x000E82D7 File Offset: 0x000E64D7
		public string[] GetRequiredDlcIds()
		{
			return this.requiredDlcIds;
		}

		// Token: 0x060067A8 RID: 26536 RVA: 0x000E82DF File Offset: 0x000E64DF
		public string[] GetForbiddenDlcIds()
		{
			return this.forbiddenDlcIds;
		}

		// Token: 0x060067A9 RID: 26537 RVA: 0x002E2408 File Offset: 0x002E0608
		public GeyserType(string id, SimHashes element, GeyserConfigurator.GeyserShape shape, float temperature, float minRatePerCycle, float maxRatePerCycle, float maxPressure, string[] requiredDlcIds, string[] forbiddenDlcIds = null, float minIterationLength = 60f, float maxIterationLength = 1140f, float minIterationPercent = 0.1f, float maxIterationPercent = 0.9f, float minYearLength = 15000f, float maxYearLength = 135000f, float minYearPercent = 0.4f, float maxYearPercent = 0.8f, float geyserTemperature = 372.15f)
		{
			this.id = id;
			this.idHash = id;
			this.element = element;
			this.shape = shape;
			this.temperature = temperature;
			this.minRatePerCycle = minRatePerCycle;
			this.maxRatePerCycle = maxRatePerCycle;
			this.maxPressure = maxPressure;
			this.minIterationLength = minIterationLength;
			this.maxIterationLength = maxIterationLength;
			this.minIterationPercent = minIterationPercent;
			this.maxIterationPercent = maxIterationPercent;
			this.minYearLength = minYearLength;
			this.maxYearLength = maxYearLength;
			this.minYearPercent = minYearPercent;
			this.maxYearPercent = maxYearPercent;
			this.requiredDlcIds = requiredDlcIds;
			this.forbiddenDlcIds = forbiddenDlcIds;
			this.geyserTemperature = geyserTemperature;
			if (GeyserConfigurator.geyserTypes == null)
			{
				GeyserConfigurator.geyserTypes = new List<GeyserConfigurator.GeyserType>();
			}
			GeyserConfigurator.geyserTypes.Add(this);
		}

		// Token: 0x060067AA RID: 26538 RVA: 0x002E24DC File Offset: 0x002E06DC
		[Obsolete]
		public GeyserType(string id, SimHashes element, GeyserConfigurator.GeyserShape shape, float temperature, float minRatePerCycle, float maxRatePerCycle, float maxPressure, float minIterationLength = 60f, float maxIterationLength = 1140f, float minIterationPercent = 0.1f, float maxIterationPercent = 0.9f, float minYearLength = 15000f, float maxYearLength = 135000f, float minYearPercent = 0.4f, float maxYearPercent = 0.8f, float geyserTemperature = 372.15f, string DlcID = "")
		{
			this.id = id;
			this.idHash = id;
			this.element = element;
			this.shape = shape;
			this.temperature = temperature;
			this.minRatePerCycle = minRatePerCycle;
			this.maxRatePerCycle = maxRatePerCycle;
			this.maxPressure = maxPressure;
			this.minIterationLength = minIterationLength;
			this.maxIterationLength = maxIterationLength;
			this.minIterationPercent = minIterationPercent;
			this.maxIterationPercent = maxIterationPercent;
			this.minYearLength = minYearLength;
			this.maxYearLength = maxYearLength;
			this.minYearPercent = minYearPercent;
			this.maxYearPercent = maxYearPercent;
			this.requiredDlcIds = new string[]
			{
				DlcID
			};
			this.geyserTemperature = geyserTemperature;
			if (GeyserConfigurator.geyserTypes == null)
			{
				GeyserConfigurator.geyserTypes = new List<GeyserConfigurator.GeyserType>();
			}
			GeyserConfigurator.geyserTypes.Add(this);
		}

		// Token: 0x060067AB RID: 26539 RVA: 0x000E82E7 File Offset: 0x000E64E7
		public GeyserConfigurator.GeyserType AddDisease(SimUtil.DiseaseInfo diseaseInfo)
		{
			this.diseaseInfo = diseaseInfo;
			return this;
		}

		// Token: 0x060067AC RID: 26540 RVA: 0x002E25B0 File Offset: 0x002E07B0
		public GeyserType()
		{
			this.id = "Blank";
			this.element = SimHashes.Void;
			this.temperature = 0f;
			this.minRatePerCycle = 0f;
			this.maxRatePerCycle = 0f;
			this.maxPressure = 0f;
			this.minIterationLength = 0f;
			this.maxIterationLength = 0f;
			this.minIterationPercent = 0f;
			this.maxIterationPercent = 0f;
			this.minYearLength = 0f;
			this.maxYearLength = 0f;
			this.minYearPercent = 0f;
			this.maxYearPercent = 0f;
			this.geyserTemperature = 0f;
		}

		// Token: 0x04004E4D RID: 20045
		public string id;

		// Token: 0x04004E4E RID: 20046
		public HashedString idHash;

		// Token: 0x04004E4F RID: 20047
		public SimHashes element;

		// Token: 0x04004E50 RID: 20048
		public GeyserConfigurator.GeyserShape shape;

		// Token: 0x04004E51 RID: 20049
		public float temperature;

		// Token: 0x04004E52 RID: 20050
		public float minRatePerCycle;

		// Token: 0x04004E53 RID: 20051
		public float maxRatePerCycle;

		// Token: 0x04004E54 RID: 20052
		public float maxPressure;

		// Token: 0x04004E55 RID: 20053
		public SimUtil.DiseaseInfo diseaseInfo = SimUtil.DiseaseInfo.Invalid;

		// Token: 0x04004E56 RID: 20054
		public float minIterationLength;

		// Token: 0x04004E57 RID: 20055
		public float maxIterationLength;

		// Token: 0x04004E58 RID: 20056
		public float minIterationPercent;

		// Token: 0x04004E59 RID: 20057
		public float maxIterationPercent;

		// Token: 0x04004E5A RID: 20058
		public float minYearLength;

		// Token: 0x04004E5B RID: 20059
		public float maxYearLength;

		// Token: 0x04004E5C RID: 20060
		public float minYearPercent;

		// Token: 0x04004E5D RID: 20061
		public float maxYearPercent;

		// Token: 0x04004E5E RID: 20062
		public float geyserTemperature;

		// Token: 0x04004E5F RID: 20063
		[Obsolete]
		public string DlcID;

		// Token: 0x04004E60 RID: 20064
		public string[] requiredDlcIds;

		// Token: 0x04004E61 RID: 20065
		public string[] forbiddenDlcIds;

		// Token: 0x04004E62 RID: 20066
		public const string BLANK_ID = "Blank";

		// Token: 0x04004E63 RID: 20067
		public const SimHashes BLANK_ELEMENT = SimHashes.Void;
	}

	// Token: 0x020013C0 RID: 5056
	[Serializable]
	public class GeyserInstanceConfiguration
	{
		// Token: 0x060067AD RID: 26541 RVA: 0x000E82F1 File Offset: 0x000E64F1
		public Geyser.GeyserModification GetModifier()
		{
			return this.modifier;
		}

		// Token: 0x060067AE RID: 26542 RVA: 0x002E2674 File Offset: 0x002E0874
		public void Init(bool reinit = false)
		{
			if (this.didInit && !reinit)
			{
				return;
			}
			this.didInit = true;
			this.scaledRate = this.Resample(this.rateRoll, this.geyserType.minRatePerCycle, this.geyserType.maxRatePerCycle);
			this.scaledIterationLength = this.Resample(this.iterationLengthRoll, this.geyserType.minIterationLength, this.geyserType.maxIterationLength);
			this.scaledIterationPercent = this.Resample(this.iterationPercentRoll, this.geyserType.minIterationPercent, this.geyserType.maxIterationPercent);
			this.scaledYearLength = this.Resample(this.yearLengthRoll, this.geyserType.minYearLength, this.geyserType.maxYearLength);
			this.scaledYearPercent = this.Resample(this.yearPercentRoll, this.geyserType.minYearPercent, this.geyserType.maxYearPercent);
		}

		// Token: 0x060067AF RID: 26543 RVA: 0x000E82F9 File Offset: 0x000E64F9
		public void SetModifier(Geyser.GeyserModification modifier)
		{
			this.modifier = modifier;
		}

		// Token: 0x17000671 RID: 1649
		// (get) Token: 0x060067B0 RID: 26544 RVA: 0x000E8302 File Offset: 0x000E6502
		public GeyserConfigurator.GeyserType geyserType
		{
			get
			{
				return GeyserConfigurator.FindType(this.typeId);
			}
		}

		// Token: 0x060067B1 RID: 26545 RVA: 0x002E275C File Offset: 0x002E095C
		private float GetModifiedValue(float geyserVariable, float modifier, Geyser.ModificationMethod method)
		{
			float num = geyserVariable;
			if (method != Geyser.ModificationMethod.Values)
			{
				if (method == Geyser.ModificationMethod.Percentages)
				{
					num += geyserVariable * modifier;
				}
			}
			else
			{
				num += modifier;
			}
			return num;
		}

		// Token: 0x060067B2 RID: 26546 RVA: 0x000E830F File Offset: 0x000E650F
		public float GetMaxPressure()
		{
			return this.GetModifiedValue(this.geyserType.maxPressure, this.modifier.maxPressureModifier, Geyser.maxPressureModificationMethod);
		}

		// Token: 0x060067B3 RID: 26547 RVA: 0x000E8332 File Offset: 0x000E6532
		public float GetIterationLength()
		{
			this.Init(false);
			return this.GetModifiedValue(this.scaledIterationLength, this.modifier.iterationDurationModifier, Geyser.IterationDurationModificationMethod);
		}

		// Token: 0x060067B4 RID: 26548 RVA: 0x000E8357 File Offset: 0x000E6557
		public float GetIterationPercent()
		{
			this.Init(false);
			return Mathf.Clamp(this.GetModifiedValue(this.scaledIterationPercent, this.modifier.iterationPercentageModifier, Geyser.IterationPercentageModificationMethod), 0f, 1f);
		}

		// Token: 0x060067B5 RID: 26549 RVA: 0x000E838B File Offset: 0x000E658B
		public float GetOnDuration()
		{
			return this.GetIterationLength() * this.GetIterationPercent();
		}

		// Token: 0x060067B6 RID: 26550 RVA: 0x000E839A File Offset: 0x000E659A
		public float GetOffDuration()
		{
			return this.GetIterationLength() * (1f - this.GetIterationPercent());
		}

		// Token: 0x060067B7 RID: 26551 RVA: 0x000E83AF File Offset: 0x000E65AF
		public float GetMassPerCycle()
		{
			this.Init(false);
			return this.GetModifiedValue(this.scaledRate, this.modifier.massPerCycleModifier, Geyser.massModificationMethod);
		}

		// Token: 0x060067B8 RID: 26552 RVA: 0x002E2780 File Offset: 0x002E0980
		public float GetEmitRate()
		{
			float num = 600f / this.GetIterationLength();
			return this.GetMassPerCycle() / num / this.GetOnDuration();
		}

		// Token: 0x060067B9 RID: 26553 RVA: 0x000E83D4 File Offset: 0x000E65D4
		public float GetYearLength()
		{
			this.Init(false);
			return this.GetModifiedValue(this.scaledYearLength, this.modifier.yearDurationModifier, Geyser.yearDurationModificationMethod);
		}

		// Token: 0x060067BA RID: 26554 RVA: 0x000E83F9 File Offset: 0x000E65F9
		public float GetYearPercent()
		{
			this.Init(false);
			return Mathf.Clamp(this.GetModifiedValue(this.scaledYearPercent, this.modifier.yearPercentageModifier, Geyser.yearPercentageModificationMethod), 0f, 1f);
		}

		// Token: 0x060067BB RID: 26555 RVA: 0x000E842D File Offset: 0x000E662D
		public float GetYearOnDuration()
		{
			return this.GetYearLength() * this.GetYearPercent();
		}

		// Token: 0x060067BC RID: 26556 RVA: 0x000E843C File Offset: 0x000E663C
		public float GetYearOffDuration()
		{
			return this.GetYearLength() * (1f - this.GetYearPercent());
		}

		// Token: 0x060067BD RID: 26557 RVA: 0x000E8451 File Offset: 0x000E6651
		public SimHashes GetElement()
		{
			if (!this.modifier.modifyElement || this.modifier.newElement == (SimHashes)0)
			{
				return this.geyserType.element;
			}
			return this.modifier.newElement;
		}

		// Token: 0x060067BE RID: 26558 RVA: 0x000E8484 File Offset: 0x000E6684
		public float GetTemperature()
		{
			return this.GetModifiedValue(this.geyserType.temperature, this.modifier.temperatureModifier, Geyser.temperatureModificationMethod);
		}

		// Token: 0x060067BF RID: 26559 RVA: 0x000E84A7 File Offset: 0x000E66A7
		public byte GetDiseaseIdx()
		{
			return this.geyserType.diseaseInfo.idx;
		}

		// Token: 0x060067C0 RID: 26560 RVA: 0x000E84B9 File Offset: 0x000E66B9
		public int GetDiseaseCount()
		{
			return this.geyserType.diseaseInfo.count;
		}

		// Token: 0x060067C1 RID: 26561 RVA: 0x002E27AC File Offset: 0x002E09AC
		public float GetAverageEmission()
		{
			float num = this.GetEmitRate() * this.GetOnDuration();
			return this.GetYearOnDuration() / this.GetIterationLength() * num / this.GetYearLength();
		}

		// Token: 0x060067C2 RID: 26562 RVA: 0x002E27E0 File Offset: 0x002E09E0
		private float Resample(float t, float min, float max)
		{
			float num = 6f;
			float num2 = 0.002472623f;
			float num3 = t * (1f - num2 * 2f) + num2;
			return (-Mathf.Log(1f / num3 - 1f) + num) / (num * 2f) * (max - min) + min;
		}

		// Token: 0x04004E64 RID: 20068
		public HashedString typeId;

		// Token: 0x04004E65 RID: 20069
		public float rateRoll;

		// Token: 0x04004E66 RID: 20070
		public float iterationLengthRoll;

		// Token: 0x04004E67 RID: 20071
		public float iterationPercentRoll;

		// Token: 0x04004E68 RID: 20072
		public float yearLengthRoll;

		// Token: 0x04004E69 RID: 20073
		public float yearPercentRoll;

		// Token: 0x04004E6A RID: 20074
		public float scaledRate;

		// Token: 0x04004E6B RID: 20075
		public float scaledIterationLength;

		// Token: 0x04004E6C RID: 20076
		public float scaledIterationPercent;

		// Token: 0x04004E6D RID: 20077
		public float scaledYearLength;

		// Token: 0x04004E6E RID: 20078
		public float scaledYearPercent;

		// Token: 0x04004E6F RID: 20079
		private bool didInit;

		// Token: 0x04004E70 RID: 20080
		private Geyser.GeyserModification modifier;
	}
}
