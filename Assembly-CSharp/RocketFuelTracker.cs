using System;

// Token: 0x02000B83 RID: 2947
public class RocketFuelTracker : WorldTracker
{
	// Token: 0x0600374D RID: 14157 RVA: 0x000C84F8 File Offset: 0x000C66F8
	public RocketFuelTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x0600374E RID: 14158 RVA: 0x00223B4C File Offset: 0x00221D4C
	public override void UpdateData()
	{
		Clustercraft component = ClusterManager.Instance.GetWorld(base.WorldID).GetComponent<Clustercraft>();
		base.AddPoint((component != null) ? component.ModuleInterface.FuelRemaining : 0f);
	}

	// Token: 0x0600374F RID: 14159 RVA: 0x000C6E5E File Offset: 0x000C505E
	public override string FormatValueString(float value)
	{
		return GameUtil.GetFormattedMass(value, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}");
	}
}
