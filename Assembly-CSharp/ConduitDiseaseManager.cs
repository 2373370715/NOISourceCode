using System;
using Klei;
using Klei.AI;
using Klei.AI.DiseaseGrowthRules;

// Token: 0x02001112 RID: 4370
public class ConduitDiseaseManager : KCompactedVector<ConduitDiseaseManager.Data>
{
	// Token: 0x06005960 RID: 22880 RVA: 0x0029D508 File Offset: 0x0029B708
	private static ElemGrowthInfo GetGrowthInfo(byte disease_idx, ushort elem_idx)
	{
		ElemGrowthInfo result;
		if (disease_idx != 255)
		{
			result = Db.Get().Diseases[(int)disease_idx].elemGrowthInfo[(int)elem_idx];
		}
		else
		{
			result = Disease.DEFAULT_GROWTH_INFO;
		}
		return result;
	}

	// Token: 0x06005961 RID: 22881 RVA: 0x000DEAE0 File Offset: 0x000DCCE0
	public ConduitDiseaseManager(ConduitTemperatureManager temperature_manager) : base(0)
	{
		this.temperatureManager = temperature_manager;
	}

	// Token: 0x06005962 RID: 22882 RVA: 0x0029D544 File Offset: 0x0029B744
	public HandleVector<int>.Handle Allocate(HandleVector<int>.Handle temperature_handle, ref ConduitFlow.ConduitContents contents)
	{
		ushort elementIndex = ElementLoader.GetElementIndex(contents.element);
		ConduitDiseaseManager.Data initial_data = new ConduitDiseaseManager.Data(temperature_handle, elementIndex, contents.mass, contents.diseaseIdx, contents.diseaseCount);
		return base.Allocate(initial_data);
	}

	// Token: 0x06005963 RID: 22883 RVA: 0x0029D580 File Offset: 0x0029B780
	public void SetData(HandleVector<int>.Handle handle, ref ConduitFlow.ConduitContents contents)
	{
		ConduitDiseaseManager.Data data = base.GetData(handle);
		data.diseaseCount = contents.diseaseCount;
		if (contents.diseaseIdx != data.diseaseIdx)
		{
			data.diseaseIdx = contents.diseaseIdx;
			ushort elementIndex = ElementLoader.GetElementIndex(contents.element);
			data.growthInfo = ConduitDiseaseManager.GetGrowthInfo(contents.diseaseIdx, elementIndex);
		}
		base.SetData(handle, data);
	}

	// Token: 0x06005964 RID: 22884 RVA: 0x0029D5E4 File Offset: 0x0029B7E4
	public void Sim200ms(float dt)
	{
		using (new KProfiler.Region("ConduitDiseaseManager.SimUpdate", null))
		{
			for (int i = 0; i < this.data.Count; i++)
			{
				ConduitDiseaseManager.Data data = this.data[i];
				if (data.diseaseIdx != 255)
				{
					float num = data.accumulatedError;
					num += data.growthInfo.CalculateDiseaseCountDelta(data.diseaseCount, data.mass, dt);
					Disease disease = Db.Get().Diseases[(int)data.diseaseIdx];
					float num2 = Disease.HalfLifeToGrowthRate(Disease.CalculateRangeHalfLife(this.temperatureManager.GetTemperature(data.temperatureHandle), ref disease.temperatureRange, ref disease.temperatureHalfLives), dt);
					num += (float)data.diseaseCount * num2 - (float)data.diseaseCount;
					int num3 = (int)num;
					data.accumulatedError = num - (float)num3;
					data.diseaseCount += num3;
					if (data.diseaseCount <= 0)
					{
						data.diseaseCount = 0;
						data.diseaseIdx = byte.MaxValue;
						data.accumulatedError = 0f;
					}
					this.data[i] = data;
				}
			}
		}
	}

	// Token: 0x06005965 RID: 22885 RVA: 0x0029D734 File Offset: 0x0029B934
	public void ModifyDiseaseCount(HandleVector<int>.Handle h, int disease_count_delta)
	{
		ConduitDiseaseManager.Data data = base.GetData(h);
		data.diseaseCount = Math.Max(0, data.diseaseCount + disease_count_delta);
		if (data.diseaseCount == 0)
		{
			data.diseaseIdx = byte.MaxValue;
		}
		base.SetData(h, data);
	}

	// Token: 0x06005966 RID: 22886 RVA: 0x0029D77C File Offset: 0x0029B97C
	public void AddDisease(HandleVector<int>.Handle h, byte disease_idx, int disease_count)
	{
		ConduitDiseaseManager.Data data = base.GetData(h);
		SimUtil.DiseaseInfo diseaseInfo = SimUtil.CalculateFinalDiseaseInfo(disease_idx, disease_count, data.diseaseIdx, data.diseaseCount);
		data.diseaseIdx = diseaseInfo.idx;
		data.diseaseCount = diseaseInfo.count;
		base.SetData(h, data);
	}

	// Token: 0x04003F93 RID: 16275
	private ConduitTemperatureManager temperatureManager;

	// Token: 0x02001113 RID: 4371
	public struct Data
	{
		// Token: 0x06005967 RID: 22887 RVA: 0x000DEAF0 File Offset: 0x000DCCF0
		public Data(HandleVector<int>.Handle temperature_handle, ushort elem_idx, float mass, byte disease_idx, int disease_count)
		{
			this.diseaseIdx = disease_idx;
			this.elemIdx = elem_idx;
			this.mass = mass;
			this.diseaseCount = disease_count;
			this.accumulatedError = 0f;
			this.temperatureHandle = temperature_handle;
			this.growthInfo = ConduitDiseaseManager.GetGrowthInfo(disease_idx, elem_idx);
		}

		// Token: 0x04003F94 RID: 16276
		public byte diseaseIdx;

		// Token: 0x04003F95 RID: 16277
		public ushort elemIdx;

		// Token: 0x04003F96 RID: 16278
		public int diseaseCount;

		// Token: 0x04003F97 RID: 16279
		public float accumulatedError;

		// Token: 0x04003F98 RID: 16280
		public float mass;

		// Token: 0x04003F99 RID: 16281
		public HandleVector<int>.Handle temperatureHandle;

		// Token: 0x04003F9A RID: 16282
		public ElemGrowthInfo growthInfo;
	}
}
