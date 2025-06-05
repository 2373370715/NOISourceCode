using System;
using System.Diagnostics;
using Klei.AI;
using UnityEngine;

namespace Klei
{
	// Token: 0x02003C4B RID: 15435
	public static class SimUtil
	{
		// Token: 0x0600EC62 RID: 60514 RVA: 0x001432D0 File Offset: 0x001414D0
		public static float CalculateEnergyFlow(float source_temp, float source_thermal_conductivity, float dest_temp, float dest_thermal_conductivity, float surface_area = 1f, float thickness = 1f)
		{
			return (source_temp - dest_temp) * Math.Min(source_thermal_conductivity, dest_thermal_conductivity) * (surface_area / thickness);
		}

		// Token: 0x0600EC63 RID: 60515 RVA: 0x004DD9C8 File Offset: 0x004DBBC8
		public static float CalculateEnergyFlow(int cell, float dest_temp, float dest_specific_heat_capacity, float dest_thermal_conductivity, float surface_area = 1f, float thickness = 1f)
		{
			if (Grid.Mass[cell] <= 0f)
			{
				return 0f;
			}
			Element element = Grid.Element[cell];
			if (element.IsVacuum)
			{
				return 0f;
			}
			float source_temp = Grid.Temperature[cell];
			float thermalConductivity = element.thermalConductivity;
			return SimUtil.CalculateEnergyFlow(source_temp, thermalConductivity, dest_temp, dest_thermal_conductivity, surface_area, thickness) * 0.001f;
		}

		// Token: 0x0600EC64 RID: 60516 RVA: 0x001432E3 File Offset: 0x001414E3
		public static float ClampEnergyTransfer(float dt, float source_temp, float source_mass, float source_specific_heat_capacity, float dest_temp, float dest_mass, float dest_specific_heat_capacity, float max_watts_transferred)
		{
			return SimUtil.ClampEnergyTransfer(dt, source_temp, source_mass * source_specific_heat_capacity, dest_temp, dest_mass * dest_specific_heat_capacity, max_watts_transferred);
		}

		// Token: 0x0600EC65 RID: 60517 RVA: 0x004DDA28 File Offset: 0x004DBC28
		public static float ClampEnergyTransfer(float dt, float source_temp, float source_heat_capacity, float dest_temp, float dest_heat_capacity, float max_watts_transferred)
		{
			float num = max_watts_transferred * dt / 1000f;
			SimUtil.CheckValidValue(num);
			float min = Math.Min(source_temp, dest_temp);
			float max = Math.Max(source_temp, dest_temp);
			float num2 = source_temp - num / source_heat_capacity;
			float value = dest_temp + num / dest_heat_capacity;
			SimUtil.CheckValidValue(num2);
			SimUtil.CheckValidValue(value);
			num2 = Mathf.Clamp(num2, min, max);
			float num3 = Mathf.Clamp(value, min, max);
			float num4 = Math.Abs(num2 - source_temp);
			float num5 = Math.Abs(num3 - dest_temp);
			float val = num4 * source_heat_capacity;
			float val2 = num5 * dest_heat_capacity;
			float num6 = (max_watts_transferred < 0f) ? -1f : 1f;
			float num7 = Math.Min(val, val2) * num6;
			SimUtil.CheckValidValue(num7);
			return num7;
		}

		// Token: 0x0600EC66 RID: 60518 RVA: 0x001432F8 File Offset: 0x001414F8
		private static float GetMassAreaScale(Element element)
		{
			if (!element.IsGas)
			{
				return 0.01f;
			}
			return 10f;
		}

		// Token: 0x0600EC67 RID: 60519 RVA: 0x0014330D File Offset: 0x0014150D
		public static float CalculateEnergyFlowCreatures(int cell, float creature_temperature, float creature_shc, float creature_thermal_conductivity, float creature_surface_area = 1f, float creature_surface_thickness = 1f)
		{
			return SimUtil.CalculateEnergyFlow(cell, creature_temperature, creature_shc, creature_thermal_conductivity, creature_surface_area, creature_surface_thickness);
		}

		// Token: 0x0600EC68 RID: 60520 RVA: 0x0014331C File Offset: 0x0014151C
		public static float EnergyFlowToTemperatureDelta(float kilojoules, float specific_heat_capacity, float mass)
		{
			if (kilojoules * specific_heat_capacity * mass == 0f)
			{
				return 0f;
			}
			return kilojoules / (specific_heat_capacity * mass);
		}

		// Token: 0x0600EC69 RID: 60521 RVA: 0x004DDAC4 File Offset: 0x004DBCC4
		public static float CalculateFinalTemperature(float mass1, float temp1, float mass2, float temp2)
		{
			float num = mass1 + mass2;
			if (num == 0f)
			{
				return 0f;
			}
			float num2 = mass1 * temp1;
			float num3 = mass2 * temp2;
			float val = (num2 + num3) / num;
			float val2;
			float val3;
			if (temp1 > temp2)
			{
				val2 = temp2;
				val3 = temp1;
			}
			else
			{
				val2 = temp1;
				val3 = temp2;
			}
			return Math.Max(val2, Math.Min(val3, val));
		}

		// Token: 0x0600EC6A RID: 60522 RVA: 0x00143335 File Offset: 0x00141535
		[Conditional("STRICT_CHECKING")]
		public static void CheckValidValue(float value)
		{
			if (!float.IsNaN(value))
			{
				float.IsInfinity(value);
			}
		}

		// Token: 0x0600EC6B RID: 60523 RVA: 0x00143346 File Offset: 0x00141546
		public static SimUtil.DiseaseInfo CalculateFinalDiseaseInfo(SimUtil.DiseaseInfo a, SimUtil.DiseaseInfo b)
		{
			return SimUtil.CalculateFinalDiseaseInfo(a.idx, a.count, b.idx, b.count);
		}

		// Token: 0x0600EC6C RID: 60524 RVA: 0x004DDB10 File Offset: 0x004DBD10
		public static SimUtil.DiseaseInfo CalculateFinalDiseaseInfo(byte src1_idx, int src1_count, byte src2_idx, int src2_count)
		{
			SimUtil.DiseaseInfo diseaseInfo = default(SimUtil.DiseaseInfo);
			if (src1_idx == src2_idx)
			{
				diseaseInfo.idx = src1_idx;
				diseaseInfo.count = src1_count + src2_count;
			}
			else if (src1_idx == 255)
			{
				diseaseInfo.idx = src2_idx;
				diseaseInfo.count = src2_count;
			}
			else if (src2_idx == 255)
			{
				diseaseInfo.idx = src1_idx;
				diseaseInfo.count = src1_count;
			}
			else
			{
				Disease disease = Db.Get().Diseases[(int)src1_idx];
				Disease disease2 = Db.Get().Diseases[(int)src2_idx];
				float num = disease.strength * (float)src1_count;
				float num2 = disease2.strength * (float)src2_count;
				if (num > num2)
				{
					int num3 = (int)((float)src2_count - num / num2 * (float)src1_count);
					if (num3 < 0)
					{
						diseaseInfo.idx = src1_idx;
						diseaseInfo.count = -num3;
					}
					else
					{
						diseaseInfo.idx = src2_idx;
						diseaseInfo.count = num3;
					}
				}
				else
				{
					int num4 = (int)((float)src1_count - num2 / num * (float)src2_count);
					if (num4 < 0)
					{
						diseaseInfo.idx = src2_idx;
						diseaseInfo.count = -num4;
					}
					else
					{
						diseaseInfo.idx = src1_idx;
						diseaseInfo.count = num4;
					}
				}
			}
			if (diseaseInfo.count <= 0)
			{
				diseaseInfo.count = 0;
				diseaseInfo.idx = byte.MaxValue;
			}
			return diseaseInfo;
		}

		// Token: 0x0600EC6D RID: 60525 RVA: 0x004DDC40 File Offset: 0x004DBE40
		public static byte DiseaseCountToAlpha254(int count)
		{
			float num = Mathf.Log((float)count, 10f);
			num /= SimUtil.MAX_DISEASE_LOG_RANGE;
			num = Math.Max(0f, Math.Min(1f, num));
			num -= SimUtil.MIN_DISEASE_LOG_SUBTRACTION / SimUtil.MAX_DISEASE_LOG_RANGE;
			num = Math.Max(0f, num);
			num /= 1f - SimUtil.MIN_DISEASE_LOG_SUBTRACTION / SimUtil.MAX_DISEASE_LOG_RANGE;
			return (byte)(num * 254f);
		}

		// Token: 0x0600EC6E RID: 60526 RVA: 0x00143365 File Offset: 0x00141565
		public static float DiseaseCountToAlpha(int count)
		{
			return (float)SimUtil.DiseaseCountToAlpha254(count) / 255f;
		}

		// Token: 0x0600EC6F RID: 60527 RVA: 0x004DDCB0 File Offset: 0x004DBEB0
		public static SimUtil.DiseaseInfo GetPercentOfDisease(PrimaryElement pe, float percent)
		{
			return new SimUtil.DiseaseInfo
			{
				idx = pe.DiseaseIdx,
				count = (int)((float)pe.DiseaseCount * percent)
			};
		}

		// Token: 0x0400E8C4 RID: 59588
		private const int MAX_ALPHA_COUNT = 1000000;

		// Token: 0x0400E8C5 RID: 59589
		private static float MIN_DISEASE_LOG_SUBTRACTION = 2f;

		// Token: 0x0400E8C6 RID: 59590
		private static float MAX_DISEASE_LOG_RANGE = 6f;

		// Token: 0x02003C4C RID: 15436
		public struct DiseaseInfo
		{
			// Token: 0x0400E8C7 RID: 59591
			public byte idx;

			// Token: 0x0400E8C8 RID: 59592
			public int count;

			// Token: 0x0400E8C9 RID: 59593
			public static readonly SimUtil.DiseaseInfo Invalid = new SimUtil.DiseaseInfo
			{
				idx = byte.MaxValue,
				count = 0
			};
		}
	}
}
