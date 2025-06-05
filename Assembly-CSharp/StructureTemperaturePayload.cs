using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001A11 RID: 6673
public struct StructureTemperaturePayload
{
	// Token: 0x17000919 RID: 2329
	// (get) Token: 0x06008AEC RID: 35564 RVA: 0x000FF62B File Offset: 0x000FD82B
	// (set) Token: 0x06008AED RID: 35565 RVA: 0x000FF633 File Offset: 0x000FD833
	public PrimaryElement primaryElement
	{
		get
		{
			return this.primaryElementBacking;
		}
		set
		{
			if (this.primaryElementBacking != value)
			{
				this.primaryElementBacking = value;
				this.overheatable = this.primaryElementBacking.GetComponent<Overheatable>();
			}
		}
	}

	// Token: 0x06008AEE RID: 35566 RVA: 0x0036BABC File Offset: 0x00369CBC
	public StructureTemperaturePayload(GameObject go)
	{
		this.simHandleCopy = -1;
		this.enabled = true;
		this.bypass = false;
		this.overrideExtents = false;
		this.overriddenExtents = default(Extents);
		this.primaryElementBacking = go.GetComponent<PrimaryElement>();
		this.overheatable = ((this.primaryElementBacking != null) ? this.primaryElementBacking.GetComponent<Overheatable>() : null);
		this.building = go.GetComponent<Building>();
		this.operational = go.GetComponent<Operational>();
		this.heatEffect = go.GetComponent<KBatchedAnimHeatPostProcessingEffect>();
		this.pendingEnergyModifications = 0f;
		this.maxTemperature = 10000f;
		this.energySourcesKW = null;
		this.isActiveStatusItemSet = false;
	}

	// Token: 0x1700091A RID: 2330
	// (get) Token: 0x06008AEF RID: 35567 RVA: 0x0036BB68 File Offset: 0x00369D68
	public float TotalEnergyProducedKW
	{
		get
		{
			if (this.energySourcesKW == null || this.energySourcesKW.Count == 0)
			{
				return 0f;
			}
			float num = 0f;
			for (int i = 0; i < this.energySourcesKW.Count; i++)
			{
				num += this.energySourcesKW[i].value;
			}
			return num;
		}
	}

	// Token: 0x06008AF0 RID: 35568 RVA: 0x000FF65B File Offset: 0x000FD85B
	public void OverrideExtents(Extents newExtents)
	{
		this.overrideExtents = true;
		this.overriddenExtents = newExtents;
	}

	// Token: 0x06008AF1 RID: 35569 RVA: 0x000FF66B File Offset: 0x000FD86B
	public Extents GetExtents()
	{
		if (!this.overrideExtents)
		{
			return this.building.GetExtents();
		}
		return this.overriddenExtents;
	}

	// Token: 0x1700091B RID: 2331
	// (get) Token: 0x06008AF2 RID: 35570 RVA: 0x000FF687 File Offset: 0x000FD887
	public float Temperature
	{
		get
		{
			return this.primaryElement.Temperature;
		}
	}

	// Token: 0x1700091C RID: 2332
	// (get) Token: 0x06008AF3 RID: 35571 RVA: 0x000FF694 File Offset: 0x000FD894
	public float ExhaustKilowatts
	{
		get
		{
			return this.building.Def.ExhaustKilowattsWhenActive;
		}
	}

	// Token: 0x1700091D RID: 2333
	// (get) Token: 0x06008AF4 RID: 35572 RVA: 0x000FF6A6 File Offset: 0x000FD8A6
	public float OperatingKilowatts
	{
		get
		{
			if (!(this.operational != null) || !this.operational.IsActive)
			{
				return 0f;
			}
			return this.building.Def.SelfHeatKilowattsWhenActive;
		}
	}

	// Token: 0x040068EE RID: 26862
	public int simHandleCopy;

	// Token: 0x040068EF RID: 26863
	public bool enabled;

	// Token: 0x040068F0 RID: 26864
	public bool bypass;

	// Token: 0x040068F1 RID: 26865
	public bool isActiveStatusItemSet;

	// Token: 0x040068F2 RID: 26866
	public bool overrideExtents;

	// Token: 0x040068F3 RID: 26867
	private PrimaryElement primaryElementBacking;

	// Token: 0x040068F4 RID: 26868
	public Overheatable overheatable;

	// Token: 0x040068F5 RID: 26869
	public Building building;

	// Token: 0x040068F6 RID: 26870
	public Operational operational;

	// Token: 0x040068F7 RID: 26871
	public KBatchedAnimHeatPostProcessingEffect heatEffect;

	// Token: 0x040068F8 RID: 26872
	public List<StructureTemperaturePayload.EnergySource> energySourcesKW;

	// Token: 0x040068F9 RID: 26873
	public float pendingEnergyModifications;

	// Token: 0x040068FA RID: 26874
	public float maxTemperature;

	// Token: 0x040068FB RID: 26875
	public Extents overriddenExtents;

	// Token: 0x02001A12 RID: 6674
	public class EnergySource
	{
		// Token: 0x06008AF5 RID: 35573 RVA: 0x000FF6D9 File Offset: 0x000FD8D9
		public EnergySource(float kj, string source)
		{
			this.source = source;
			this.kw_accumulator = new RunningAverage(float.MinValue, float.MaxValue, Mathf.RoundToInt(186f), true);
		}

		// Token: 0x1700091E RID: 2334
		// (get) Token: 0x06008AF6 RID: 35574 RVA: 0x000FF708 File Offset: 0x000FD908
		public float value
		{
			get
			{
				return this.kw_accumulator.AverageValue;
			}
		}

		// Token: 0x06008AF7 RID: 35575 RVA: 0x000FF715 File Offset: 0x000FD915
		public void Accumulate(float value)
		{
			this.kw_accumulator.AddSample(value);
		}

		// Token: 0x040068FC RID: 26876
		public string source;

		// Token: 0x040068FD RID: 26877
		public RunningAverage kw_accumulator;
	}
}
