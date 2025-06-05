using System;
using Klei.AI;
using UnityEngine;

// Token: 0x02000A9B RID: 2715
public struct InfraredVisualizerData
{
	// Token: 0x06003170 RID: 12656 RVA: 0x0020CAA4 File Offset: 0x0020ACA4
	public void Update()
	{
		float num = 0f;
		if (this.temperatureAmount != null)
		{
			num = this.temperatureAmount.value;
		}
		else if (this.structureTemperature.IsValid())
		{
			num = GameComps.StructureTemperatures.GetPayload(this.structureTemperature).Temperature;
		}
		else if (this.primaryElement != null)
		{
			num = this.primaryElement.Temperature;
		}
		else if (this.temperatureVulnerable != null)
		{
			num = this.temperatureVulnerable.InternalTemperature;
		}
		else if (this.critterTemperatureMonitorInstance != null)
		{
			num = this.critterTemperatureMonitorInstance.GetTemperatureInternal();
		}
		if (num < 0f)
		{
			return;
		}
		Color32 c = SimDebugView.Instance.NormalizedTemperature(num);
		this.controller.OverlayColour = c;
	}

	// Token: 0x06003171 RID: 12657 RVA: 0x0020CB6C File Offset: 0x0020AD6C
	public InfraredVisualizerData(GameObject go)
	{
		this.controller = go.GetComponent<KBatchedAnimController>();
		if (this.controller != null)
		{
			this.temperatureAmount = Db.Get().Amounts.Temperature.Lookup(go);
			this.structureTemperature = GameComps.StructureTemperatures.GetHandle(go);
			this.primaryElement = go.GetComponent<PrimaryElement>();
			this.temperatureVulnerable = go.GetComponent<TemperatureVulnerable>();
			this.critterTemperatureMonitorInstance = go.GetSMI<CritterTemperatureMonitor.Instance>();
			return;
		}
		this.temperatureAmount = null;
		this.structureTemperature = HandleVector<int>.InvalidHandle;
		this.primaryElement = null;
		this.temperatureVulnerable = null;
		this.critterTemperatureMonitorInstance = null;
	}

	// Token: 0x040021F1 RID: 8689
	public KAnimControllerBase controller;

	// Token: 0x040021F2 RID: 8690
	public AmountInstance temperatureAmount;

	// Token: 0x040021F3 RID: 8691
	public HandleVector<int>.Handle structureTemperature;

	// Token: 0x040021F4 RID: 8692
	public PrimaryElement primaryElement;

	// Token: 0x040021F5 RID: 8693
	public TemperatureVulnerable temperatureVulnerable;

	// Token: 0x040021F6 RID: 8694
	public CritterTemperatureMonitor.Instance critterTemperatureMonitorInstance;
}
