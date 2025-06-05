using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000D73 RID: 3443
public class DirectVolumeHeater : KMonoBehaviour, ISim33ms, ISim200ms, ISim1000ms, ISim4000ms, IGameObjectEffectDescriptor
{
	// Token: 0x060042CD RID: 17101 RVA: 0x000CF9F2 File Offset: 0x000CDBF2
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.primaryElement = base.GetComponent<PrimaryElement>();
		this.structureTemperature = GameComps.StructureTemperatures.GetHandle(base.gameObject);
	}

	// Token: 0x060042CE RID: 17102 RVA: 0x002501D4 File Offset: 0x0024E3D4
	public void Sim33ms(float dt)
	{
		if (this.impulseFrequency == DirectVolumeHeater.TimeMode.ms33)
		{
			float num = 0f;
			num += this.AddHeatToVolume(dt);
			num += this.AddSelfHeat(dt);
			this.heatEffect.SetHeatBeingProducedValue(num);
		}
	}

	// Token: 0x060042CF RID: 17103 RVA: 0x00250210 File Offset: 0x0024E410
	public void Sim200ms(float dt)
	{
		if (this.impulseFrequency == DirectVolumeHeater.TimeMode.ms200)
		{
			float num = 0f;
			num += this.AddHeatToVolume(dt);
			num += this.AddSelfHeat(dt);
			this.heatEffect.SetHeatBeingProducedValue(num);
		}
	}

	// Token: 0x060042D0 RID: 17104 RVA: 0x0025024C File Offset: 0x0024E44C
	public void Sim1000ms(float dt)
	{
		if (this.impulseFrequency == DirectVolumeHeater.TimeMode.ms1000)
		{
			float num = 0f;
			num += this.AddHeatToVolume(dt);
			num += this.AddSelfHeat(dt);
			this.heatEffect.SetHeatBeingProducedValue(num);
		}
	}

	// Token: 0x060042D1 RID: 17105 RVA: 0x00250288 File Offset: 0x0024E488
	public void Sim4000ms(float dt)
	{
		if (this.impulseFrequency == DirectVolumeHeater.TimeMode.ms4000)
		{
			float num = 0f;
			num += this.AddHeatToVolume(dt);
			num += this.AddSelfHeat(dt);
			this.heatEffect.SetHeatBeingProducedValue(num);
		}
	}

	// Token: 0x060042D2 RID: 17106 RVA: 0x000CFA1C File Offset: 0x000CDC1C
	private float CalculateCellWeight(int dx, int dy, int maxDistance)
	{
		return 1f + (float)(maxDistance - Math.Abs(dx) - Math.Abs(dy));
	}

	// Token: 0x060042D3 RID: 17107 RVA: 0x002502C4 File Offset: 0x0024E4C4
	private bool TestLineOfSight(int offsetCell)
	{
		int cell = Grid.PosToCell(base.gameObject);
		int x;
		int y;
		Grid.CellToXY(offsetCell, out x, out y);
		int x2;
		int y2;
		Grid.CellToXY(cell, out x2, out y2);
		return Grid.FastTestLineOfSightSolid(x2, y2, x, y);
	}

	// Token: 0x060042D4 RID: 17108 RVA: 0x002502F8 File Offset: 0x0024E4F8
	private float AddSelfHeat(float dt)
	{
		if (!this.EnableEmission)
		{
			return 0f;
		}
		if (this.primaryElement.Temperature > this.maximumInternalTemperature)
		{
			return 0f;
		}
		float result = 8f;
		GameComps.StructureTemperatures.ProduceEnergy(this.structureTemperature, 8f * dt, BUILDINGS.PREFABS.STEAMTURBINE2.HEAT_SOURCE, dt);
		return result;
	}

	// Token: 0x060042D5 RID: 17109 RVA: 0x00250354 File Offset: 0x0024E554
	private float AddHeatToVolume(float dt)
	{
		if (!this.EnableEmission)
		{
			return 0f;
		}
		int num = Grid.PosToCell(base.gameObject);
		int num2 = this.width / 2;
		int num3 = this.width % 2;
		int maxDistance = num2 + this.height;
		float num4 = 0f;
		float num5 = this.DTUs * dt / 1000f;
		for (int i = -num2; i < num2 + num3; i++)
		{
			for (int j = 0; j < this.height; j++)
			{
				if (Grid.IsCellOffsetValid(num, i, j))
				{
					int num6 = Grid.OffsetCell(num, i, j);
					if (!Grid.Solid[num6] && Grid.Mass[num6] != 0f && Grid.WorldIdx[num6] == Grid.WorldIdx[num] && this.TestLineOfSight(num6) && Grid.Temperature[num6] < this.maximumExternalTemperature)
					{
						num4 += this.CalculateCellWeight(i, j, maxDistance);
					}
				}
			}
		}
		float num7 = num5;
		if (num4 > 0f)
		{
			num7 /= num4;
		}
		float num8 = 0f;
		for (int k = -num2; k < num2 + num3; k++)
		{
			for (int l = 0; l < this.height; l++)
			{
				if (Grid.IsCellOffsetValid(num, k, l))
				{
					int num9 = Grid.OffsetCell(num, k, l);
					if (!Grid.Solid[num9] && Grid.Mass[num9] != 0f && Grid.WorldIdx[num9] == Grid.WorldIdx[num] && this.TestLineOfSight(num9) && Grid.Temperature[num9] < this.maximumExternalTemperature)
					{
						float num10 = num7 * this.CalculateCellWeight(k, l, maxDistance);
						num8 += num10;
						SimMessages.ModifyEnergy(num9, num10, 10000f, SimMessages.EnergySourceID.HeatBulb);
					}
				}
			}
		}
		return num8;
	}

	// Token: 0x060042D6 RID: 17110 RVA: 0x0025053C File Offset: 0x0024E73C
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		string formattedHeatEnergy = GameUtil.GetFormattedHeatEnergy(this.DTUs, GameUtil.HeatEnergyFormatterUnit.Automatic);
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.HEATGENERATED, formattedHeatEnergy), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.HEATGENERATED, formattedHeatEnergy), Descriptor.DescriptorType.Effect);
		list.Add(item);
		return list;
	}

	// Token: 0x04002E04 RID: 11780
	[SerializeField]
	public int width = 12;

	// Token: 0x04002E05 RID: 11781
	[SerializeField]
	public int height = 4;

	// Token: 0x04002E06 RID: 11782
	[SerializeField]
	public float DTUs = 100000f;

	// Token: 0x04002E07 RID: 11783
	[SerializeField]
	public float maximumInternalTemperature = 773.15f;

	// Token: 0x04002E08 RID: 11784
	[SerializeField]
	public float maximumExternalTemperature = 340f;

	// Token: 0x04002E09 RID: 11785
	[SerializeField]
	public Operational operational;

	// Token: 0x04002E0A RID: 11786
	[MyCmpAdd]
	private KBatchedAnimHeatPostProcessingEffect heatEffect;

	// Token: 0x04002E0B RID: 11787
	public bool EnableEmission;

	// Token: 0x04002E0C RID: 11788
	private HandleVector<int>.Handle structureTemperature;

	// Token: 0x04002E0D RID: 11789
	private PrimaryElement primaryElement;

	// Token: 0x04002E0E RID: 11790
	[SerializeField]
	private DirectVolumeHeater.TimeMode impulseFrequency = DirectVolumeHeater.TimeMode.ms1000;

	// Token: 0x02000D74 RID: 3444
	private enum TimeMode
	{
		// Token: 0x04002E10 RID: 11792
		ms33,
		// Token: 0x04002E11 RID: 11793
		ms200,
		// Token: 0x04002E12 RID: 11794
		ms1000,
		// Token: 0x04002E13 RID: 11795
		ms4000
	}
}
