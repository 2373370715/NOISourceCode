using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x0200180A RID: 6154
public class RetiredColonyData
{
	// Token: 0x06007EAA RID: 32426 RVA: 0x000AA024 File Offset: 0x000A8224
	public RetiredColonyData()
	{
	}

	// Token: 0x06007EAB RID: 32427 RVA: 0x00338AF8 File Offset: 0x00336CF8
	public RetiredColonyData(string colonyName, int cycleCount, string date, string[] achievements, MinionAssignablesProxy[] minions, BuildingComplete[] buildingCompletes, string startWorld, Dictionary<string, string> worldIdentities)
	{
		this.colonyName = colonyName;
		this.cycleCount = cycleCount;
		this.achievements = achievements;
		this.date = date;
		this.Duplicants = new RetiredColonyData.RetiredDuplicantData[(minions == null) ? 0 : minions.Length];
		int i = 0;
		while (i < this.Duplicants.Length)
		{
			this.Duplicants[i] = new RetiredColonyData.RetiredDuplicantData();
			this.Duplicants[i].name = minions[i].GetProperName();
			this.Duplicants[i].age = (int)Mathf.Floor((float)GameClock.Instance.GetCycle() - minions[i].GetArrivalTime());
			this.Duplicants[i].skillPointsGained = minions[i].GetTotalSkillpoints();
			this.Duplicants[i].accessories = new Dictionary<string, string>();
			if (minions[i].GetTargetGameObject().GetComponent<Accessorizer>() != null)
			{
				using (List<ResourceRef<Accessory>>.Enumerator enumerator = minions[i].GetTargetGameObject().GetComponent<Accessorizer>().GetAccessories().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ResourceRef<Accessory> resourceRef = enumerator.Current;
						if (resourceRef.Get() != null)
						{
							this.Duplicants[i].accessories.Add(resourceRef.Get().slot.Id, resourceRef.Get().Id);
						}
					}
					goto IL_3AF;
				}
				goto IL_14E;
			}
			goto IL_14E;
			IL_3AF:
			i++;
			continue;
			IL_14E:
			StoredMinionIdentity component = minions[i].GetTargetGameObject().GetComponent<StoredMinionIdentity>();
			this.Duplicants[i].accessories.Add(Db.Get().AccessorySlots.Eyes.Id, Db.Get().Accessories.Get(component.bodyData.eyes).Id);
			this.Duplicants[i].accessories.Add(Db.Get().AccessorySlots.Arm.Id, Db.Get().Accessories.Get(component.bodyData.arms).Id);
			this.Duplicants[i].accessories.Add(Db.Get().AccessorySlots.ArmLower.Id, Db.Get().Accessories.Get(component.bodyData.armslower).Id);
			this.Duplicants[i].accessories.Add(Db.Get().AccessorySlots.Body.Id, Db.Get().Accessories.Get(component.bodyData.body).Id);
			this.Duplicants[i].accessories.Add(Db.Get().AccessorySlots.Hair.Id, Db.Get().Accessories.Get(component.bodyData.hair).Id);
			if (component.bodyData.hat != HashedString.Invalid)
			{
				this.Duplicants[i].accessories.Add(Db.Get().AccessorySlots.Hat.Id, Db.Get().Accessories.Get(component.bodyData.hat).Id);
			}
			this.Duplicants[i].accessories.Add(Db.Get().AccessorySlots.HeadShape.Id, Db.Get().Accessories.Get(component.bodyData.headShape).Id);
			this.Duplicants[i].accessories.Add(Db.Get().AccessorySlots.Mouth.Id, Db.Get().Accessories.Get(component.bodyData.mouth).Id);
			goto IL_3AF;
		}
		Dictionary<Tag, int> dictionary = new Dictionary<Tag, int>();
		if (buildingCompletes != null)
		{
			foreach (BuildingComplete cmp in buildingCompletes)
			{
				if (!dictionary.ContainsKey(cmp.PrefabID()))
				{
					dictionary[cmp.PrefabID()] = 0;
				}
				Dictionary<Tag, int> dictionary2 = dictionary;
				Tag key = cmp.PrefabID();
				int num = dictionary2[key];
				dictionary2[key] = num + 1;
			}
		}
		this.buildings = new List<global::Tuple<string, int>>();
		foreach (KeyValuePair<Tag, int> keyValuePair in dictionary)
		{
			this.buildings.Add(new global::Tuple<string, int>(keyValuePair.Key.ToString(), keyValuePair.Value));
		}
		this.Stats = null;
		if (ReportManager.Instance != null)
		{
			global::Tuple<float, float>[] array = new global::Tuple<float, float>[ReportManager.Instance.reports.Count];
			for (int k = 0; k < array.Length; k++)
			{
				array[k] = new global::Tuple<float, float>((float)ReportManager.Instance.reports[k].day, ReportManager.Instance.reports[k].GetEntry(ReportManager.ReportType.OxygenCreated).accPositive);
			}
			global::Tuple<float, float>[] array2 = new global::Tuple<float, float>[ReportManager.Instance.reports.Count];
			for (int l = 0; l < array2.Length; l++)
			{
				array2[l] = new global::Tuple<float, float>((float)ReportManager.Instance.reports[l].day, ReportManager.Instance.reports[l].GetEntry(ReportManager.ReportType.OxygenCreated).accNegative * -1f);
			}
			global::Tuple<float, float>[] array3 = new global::Tuple<float, float>[ReportManager.Instance.reports.Count];
			for (int m = 0; m < array3.Length; m++)
			{
				array3[m] = new global::Tuple<float, float>((float)ReportManager.Instance.reports[m].day, ReportManager.Instance.reports[m].GetEntry(ReportManager.ReportType.CaloriesCreated).accPositive * 0.001f);
			}
			global::Tuple<float, float>[] array4 = new global::Tuple<float, float>[ReportManager.Instance.reports.Count];
			for (int n = 0; n < array4.Length; n++)
			{
				array4[n] = new global::Tuple<float, float>((float)ReportManager.Instance.reports[n].day, ReportManager.Instance.reports[n].GetEntry(ReportManager.ReportType.CaloriesCreated).accNegative * 0.001f * -1f);
			}
			global::Tuple<float, float>[] array5 = new global::Tuple<float, float>[ReportManager.Instance.reports.Count];
			for (int num2 = 0; num2 < array5.Length; num2++)
			{
				array5[num2] = new global::Tuple<float, float>((float)ReportManager.Instance.reports[num2].day, ReportManager.Instance.reports[num2].GetEntry(ReportManager.ReportType.EnergyCreated).accPositive * 0.001f);
			}
			global::Tuple<float, float>[] array6 = new global::Tuple<float, float>[ReportManager.Instance.reports.Count];
			for (int num3 = 0; num3 < array6.Length; num3++)
			{
				array6[num3] = new global::Tuple<float, float>((float)ReportManager.Instance.reports[num3].day, ReportManager.Instance.reports[num3].GetEntry(ReportManager.ReportType.EnergyWasted).accNegative * -1f * 0.001f);
			}
			global::Tuple<float, float>[] array7 = new global::Tuple<float, float>[ReportManager.Instance.reports.Count];
			for (int num4 = 0; num4 < array7.Length; num4++)
			{
				array7[num4] = new global::Tuple<float, float>((float)ReportManager.Instance.reports[num4].day, ReportManager.Instance.reports[num4].GetEntry(ReportManager.ReportType.WorkTime).accPositive);
			}
			global::Tuple<float, float>[] array8 = new global::Tuple<float, float>[ReportManager.Instance.reports.Count];
			for (int num5 = 0; num5 < array7.Length; num5++)
			{
				int num6 = 0;
				float num7 = 0f;
				ReportManager.ReportEntry entry = ReportManager.Instance.reports[num5].GetEntry(ReportManager.ReportType.WorkTime);
				for (int num8 = 0; num8 < entry.contextEntries.Count; num8++)
				{
					num6++;
					num7 += entry.contextEntries[num8].accPositive;
				}
				num7 /= (float)num6;
				num7 /= 600f;
				num7 *= 100f;
				array8[num5] = new global::Tuple<float, float>((float)ReportManager.Instance.reports[num5].day, num7);
			}
			global::Tuple<float, float>[] array9 = new global::Tuple<float, float>[ReportManager.Instance.reports.Count];
			for (int num9 = 0; num9 < array9.Length; num9++)
			{
				array9[num9] = new global::Tuple<float, float>((float)ReportManager.Instance.reports[num9].day, ReportManager.Instance.reports[num9].GetEntry(ReportManager.ReportType.TravelTime).accPositive);
			}
			global::Tuple<float, float>[] array10 = new global::Tuple<float, float>[ReportManager.Instance.reports.Count];
			for (int num10 = 0; num10 < array9.Length; num10++)
			{
				int num11 = 0;
				float num12 = 0f;
				ReportManager.ReportEntry entry2 = ReportManager.Instance.reports[num10].GetEntry(ReportManager.ReportType.TravelTime);
				for (int num13 = 0; num13 < entry2.contextEntries.Count; num13++)
				{
					num11++;
					num12 += entry2.contextEntries[num13].accPositive;
				}
				num12 /= (float)num11;
				num12 /= 600f;
				num12 *= 100f;
				array10[num10] = new global::Tuple<float, float>((float)ReportManager.Instance.reports[num10].day, num12);
			}
			global::Tuple<float, float>[] array11 = new global::Tuple<float, float>[ReportManager.Instance.reports.Count];
			for (int num14 = 0; num14 < array7.Length; num14++)
			{
				array11[num14] = new global::Tuple<float, float>((float)ReportManager.Instance.reports[num14].day, (float)ReportManager.Instance.reports[num14].GetEntry(ReportManager.ReportType.WorkTime).contextEntries.Count);
			}
			global::Tuple<float, float>[] array12 = new global::Tuple<float, float>[ReportManager.Instance.reports.Count];
			for (int num15 = 0; num15 < array12.Length; num15++)
			{
				int num16 = 0;
				float num17 = 0f;
				ReportManager.ReportEntry entry3 = ReportManager.Instance.reports[num15].GetEntry(ReportManager.ReportType.StressDelta);
				for (int num18 = 0; num18 < entry3.contextEntries.Count; num18++)
				{
					num16++;
					num17 += entry3.contextEntries[num18].accPositive;
				}
				array12[num15] = new global::Tuple<float, float>((float)ReportManager.Instance.reports[num15].day, num17 / (float)num16);
			}
			global::Tuple<float, float>[] array13 = new global::Tuple<float, float>[ReportManager.Instance.reports.Count];
			for (int num19 = 0; num19 < array13.Length; num19++)
			{
				int num20 = 0;
				float num21 = 0f;
				ReportManager.ReportEntry entry4 = ReportManager.Instance.reports[num19].GetEntry(ReportManager.ReportType.StressDelta);
				for (int num22 = 0; num22 < entry4.contextEntries.Count; num22++)
				{
					num20++;
					num21 += entry4.contextEntries[num22].accNegative;
				}
				num21 *= -1f;
				array13[num19] = new global::Tuple<float, float>((float)ReportManager.Instance.reports[num19].day, num21 / (float)num20);
			}
			global::Tuple<float, float>[] array14 = new global::Tuple<float, float>[ReportManager.Instance.reports.Count];
			for (int num23 = 0; num23 < array14.Length; num23++)
			{
				array14[num23] = new global::Tuple<float, float>((float)ReportManager.Instance.reports[num23].day, ReportManager.Instance.reports[num23].GetEntry(ReportManager.ReportType.DomesticatedCritters).accPositive);
			}
			global::Tuple<float, float>[] array15 = new global::Tuple<float, float>[ReportManager.Instance.reports.Count];
			for (int num24 = 0; num24 < array15.Length; num24++)
			{
				array15[num24] = new global::Tuple<float, float>((float)ReportManager.Instance.reports[num24].day, ReportManager.Instance.reports[num24].GetEntry(ReportManager.ReportType.WildCritters).accPositive);
			}
			global::Tuple<float, float>[] array16 = new global::Tuple<float, float>[ReportManager.Instance.reports.Count];
			for (int num25 = 0; num25 < array16.Length; num25++)
			{
				array16[num25] = new global::Tuple<float, float>((float)ReportManager.Instance.reports[num25].day, ReportManager.Instance.reports[num25].GetEntry(ReportManager.ReportType.RocketsInFlight).accPositive);
			}
			this.Stats = new RetiredColonyData.RetiredColonyStatistic[]
			{
				new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.OxygenProduced, array, UI.RETIRED_COLONY_INFO_SCREEN.STATS.OXYGEN_CREATED, UI.MATH_PICTURES.AXIS_LABELS.CYCLES, UI.UNITSUFFIXES.MASS.KILOGRAM),
				new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.OxygenConsumed, array2, UI.RETIRED_COLONY_INFO_SCREEN.STATS.OXYGEN_CONSUMED, UI.MATH_PICTURES.AXIS_LABELS.CYCLES, UI.UNITSUFFIXES.MASS.KILOGRAM),
				new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.CaloriesProduced, array3, UI.RETIRED_COLONY_INFO_SCREEN.STATS.CALORIES_CREATED, UI.MATH_PICTURES.AXIS_LABELS.CYCLES, UI.UNITSUFFIXES.CALORIES.KILOCALORIE),
				new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.CaloriesRemoved, array4, UI.RETIRED_COLONY_INFO_SCREEN.STATS.CALORIES_CONSUMED, UI.MATH_PICTURES.AXIS_LABELS.CYCLES, UI.UNITSUFFIXES.CALORIES.KILOCALORIE),
				new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.PowerProduced, array5, UI.RETIRED_COLONY_INFO_SCREEN.STATS.POWER_CREATED, UI.MATH_PICTURES.AXIS_LABELS.CYCLES, UI.UNITSUFFIXES.ELECTRICAL.KILOJOULE),
				new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.PowerWasted, array6, UI.RETIRED_COLONY_INFO_SCREEN.STATS.POWER_WASTED, UI.MATH_PICTURES.AXIS_LABELS.CYCLES, UI.UNITSUFFIXES.ELECTRICAL.KILOJOULE),
				new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.WorkTime, array7, UI.RETIRED_COLONY_INFO_SCREEN.STATS.WORK_TIME, UI.MATH_PICTURES.AXIS_LABELS.CYCLES, UI.UNITSUFFIXES.SECONDS),
				new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.AverageWorkTime, array8, UI.RETIRED_COLONY_INFO_SCREEN.STATS.AVERAGE_WORK_TIME, UI.MATH_PICTURES.AXIS_LABELS.CYCLES, UI.UNITSUFFIXES.PERCENT),
				new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.TravelTime, array9, UI.RETIRED_COLONY_INFO_SCREEN.STATS.TRAVEL_TIME, UI.MATH_PICTURES.AXIS_LABELS.CYCLES, UI.UNITSUFFIXES.SECONDS),
				new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.AverageTravelTime, array10, UI.RETIRED_COLONY_INFO_SCREEN.STATS.AVERAGE_TRAVEL_TIME, UI.MATH_PICTURES.AXIS_LABELS.CYCLES, UI.UNITSUFFIXES.PERCENT),
				new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.LiveDuplicants, array11, UI.RETIRED_COLONY_INFO_SCREEN.STATS.LIVE_DUPLICANTS, UI.MATH_PICTURES.AXIS_LABELS.CYCLES, UI.UNITSUFFIXES.DUPLICANTS),
				new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.RocketsInFlight, array16, UI.RETIRED_COLONY_INFO_SCREEN.STATS.ROCKET_MISSIONS, UI.MATH_PICTURES.AXIS_LABELS.CYCLES, UI.UNITSUFFIXES.ROCKET_MISSIONS),
				new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.AverageStressCreated, array12, UI.RETIRED_COLONY_INFO_SCREEN.STATS.AVERAGE_STRESS_CREATED, UI.MATH_PICTURES.AXIS_LABELS.CYCLES, UI.UNITSUFFIXES.PERCENT),
				new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.AverageStressRemoved, array13, UI.RETIRED_COLONY_INFO_SCREEN.STATS.AVERAGE_STRESS_REMOVED, UI.MATH_PICTURES.AXIS_LABELS.CYCLES, UI.UNITSUFFIXES.PERCENT),
				new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.DomesticatedCritters, array14, UI.RETIRED_COLONY_INFO_SCREEN.STATS.NUMBER_DOMESTICATED_CRITTERS, UI.MATH_PICTURES.AXIS_LABELS.CYCLES, UI.UNITSUFFIXES.CRITTERS),
				new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.WildCritters, array15, UI.RETIRED_COLONY_INFO_SCREEN.STATS.NUMBER_WILD_CRITTERS, UI.MATH_PICTURES.AXIS_LABELS.CYCLES, UI.UNITSUFFIXES.CRITTERS)
			};
			this.startWorld = startWorld;
			this.worldIdentities = worldIdentities;
		}
	}

	// Token: 0x17000807 RID: 2055
	// (get) Token: 0x06007EAC RID: 32428 RVA: 0x000F7E02 File Offset: 0x000F6002
	// (set) Token: 0x06007EAD RID: 32429 RVA: 0x000F7E0A File Offset: 0x000F600A
	public string colonyName { get; set; }

	// Token: 0x17000808 RID: 2056
	// (get) Token: 0x06007EAE RID: 32430 RVA: 0x000F7E13 File Offset: 0x000F6013
	// (set) Token: 0x06007EAF RID: 32431 RVA: 0x000F7E1B File Offset: 0x000F601B
	public int cycleCount { get; set; }

	// Token: 0x17000809 RID: 2057
	// (get) Token: 0x06007EB0 RID: 32432 RVA: 0x000F7E24 File Offset: 0x000F6024
	// (set) Token: 0x06007EB1 RID: 32433 RVA: 0x000F7E2C File Offset: 0x000F602C
	public string date { get; set; }

	// Token: 0x1700080A RID: 2058
	// (get) Token: 0x06007EB2 RID: 32434 RVA: 0x000F7E35 File Offset: 0x000F6035
	// (set) Token: 0x06007EB3 RID: 32435 RVA: 0x000F7E3D File Offset: 0x000F603D
	public string[] achievements { get; set; }

	// Token: 0x1700080B RID: 2059
	// (get) Token: 0x06007EB4 RID: 32436 RVA: 0x000F7E46 File Offset: 0x000F6046
	// (set) Token: 0x06007EB5 RID: 32437 RVA: 0x000F7E4E File Offset: 0x000F604E
	public RetiredColonyData.RetiredDuplicantData[] Duplicants { get; set; }

	// Token: 0x1700080C RID: 2060
	// (get) Token: 0x06007EB6 RID: 32438 RVA: 0x000F7E57 File Offset: 0x000F6057
	// (set) Token: 0x06007EB7 RID: 32439 RVA: 0x000F7E5F File Offset: 0x000F605F
	public List<global::Tuple<string, int>> buildings { get; set; }

	// Token: 0x1700080D RID: 2061
	// (get) Token: 0x06007EB8 RID: 32440 RVA: 0x000F7E68 File Offset: 0x000F6068
	// (set) Token: 0x06007EB9 RID: 32441 RVA: 0x000F7E70 File Offset: 0x000F6070
	public RetiredColonyData.RetiredColonyStatistic[] Stats { get; set; }

	// Token: 0x1700080E RID: 2062
	// (get) Token: 0x06007EBA RID: 32442 RVA: 0x000F7E79 File Offset: 0x000F6079
	// (set) Token: 0x06007EBB RID: 32443 RVA: 0x000F7E81 File Offset: 0x000F6081
	public Dictionary<string, string> worldIdentities { get; set; }

	// Token: 0x1700080F RID: 2063
	// (get) Token: 0x06007EBC RID: 32444 RVA: 0x000F7E8A File Offset: 0x000F608A
	// (set) Token: 0x06007EBD RID: 32445 RVA: 0x000F7E92 File Offset: 0x000F6092
	public string startWorld { get; set; }

	// Token: 0x0200180B RID: 6155
	public static class DataIDs
	{
		// Token: 0x0400603A RID: 24634
		public static string OxygenProduced = "oxygenProduced";

		// Token: 0x0400603B RID: 24635
		public static string OxygenConsumed = "oxygenConsumed";

		// Token: 0x0400603C RID: 24636
		public static string CaloriesProduced = "caloriesProduced";

		// Token: 0x0400603D RID: 24637
		public static string CaloriesRemoved = "caloriesRemoved";

		// Token: 0x0400603E RID: 24638
		public static string PowerProduced = "powerProduced";

		// Token: 0x0400603F RID: 24639
		public static string PowerWasted = "powerWasted";

		// Token: 0x04006040 RID: 24640
		public static string WorkTime = "workTime";

		// Token: 0x04006041 RID: 24641
		public static string TravelTime = "travelTime";

		// Token: 0x04006042 RID: 24642
		public static string AverageWorkTime = "averageWorkTime";

		// Token: 0x04006043 RID: 24643
		public static string AverageTravelTime = "averageTravelTime";

		// Token: 0x04006044 RID: 24644
		public static string LiveDuplicants = "liveDuplicants";

		// Token: 0x04006045 RID: 24645
		public static string AverageStressCreated = "averageStressCreated";

		// Token: 0x04006046 RID: 24646
		public static string AverageStressRemoved = "averageStressRemoved";

		// Token: 0x04006047 RID: 24647
		public static string DomesticatedCritters = "domesticatedCritters";

		// Token: 0x04006048 RID: 24648
		public static string WildCritters = "wildCritters";

		// Token: 0x04006049 RID: 24649
		public static string AverageGerms = "averageGerms";

		// Token: 0x0400604A RID: 24650
		public static string RocketsInFlight = "rocketsInFlight";
	}

	// Token: 0x0200180C RID: 6156
	public class RetiredColonyStatistic
	{
		// Token: 0x06007EBF RID: 32447 RVA: 0x000AA024 File Offset: 0x000A8224
		public RetiredColonyStatistic()
		{
		}

		// Token: 0x06007EC0 RID: 32448 RVA: 0x000F7E9B File Offset: 0x000F609B
		public RetiredColonyStatistic(string id, global::Tuple<float, float>[] data, string name, string axisNameX, string axisNameY)
		{
			this.id = id;
			this.value = data;
			this.name = name;
			this.nameX = axisNameX;
			this.nameY = axisNameY;
		}

		// Token: 0x06007EC1 RID: 32449 RVA: 0x00339B98 File Offset: 0x00337D98
		public global::Tuple<float, float> GetByMaxValue()
		{
			if (this.value.Length == 0)
			{
				return new global::Tuple<float, float>(0f, 0f);
			}
			int num = -1;
			float num2 = -1f;
			for (int i = 0; i < this.value.Length; i++)
			{
				if (this.value[i].second > num2)
				{
					num2 = this.value[i].second;
					num = i;
				}
			}
			if (num == -1)
			{
				num = 0;
			}
			return this.value[num];
		}

		// Token: 0x06007EC2 RID: 32450 RVA: 0x00339C08 File Offset: 0x00337E08
		public global::Tuple<float, float> GetByMaxKey()
		{
			if (this.value.Length == 0)
			{
				return new global::Tuple<float, float>(0f, 0f);
			}
			int num = -1;
			float num2 = -1f;
			for (int i = 0; i < this.value.Length; i++)
			{
				if (this.value[i].first > num2)
				{
					num2 = this.value[i].first;
					num = i;
				}
			}
			return this.value[num];
		}

		// Token: 0x0400604B RID: 24651
		public string id;

		// Token: 0x0400604C RID: 24652
		public global::Tuple<float, float>[] value;

		// Token: 0x0400604D RID: 24653
		public string name;

		// Token: 0x0400604E RID: 24654
		public string nameX;

		// Token: 0x0400604F RID: 24655
		public string nameY;
	}

	// Token: 0x0200180D RID: 6157
	public class RetiredDuplicantData
	{
		// Token: 0x04006050 RID: 24656
		public string name;

		// Token: 0x04006051 RID: 24657
		public int age;

		// Token: 0x04006052 RID: 24658
		public int skillPointsGained;

		// Token: 0x04006053 RID: 24659
		public Dictionary<string, string> accessories;
	}
}
