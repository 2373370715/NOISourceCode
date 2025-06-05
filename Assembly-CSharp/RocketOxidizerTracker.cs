using System;

// Token: 0x02000B84 RID: 2948
public class RocketOxidizerTracker : WorldTracker
{
	// Token: 0x06003750 RID: 14160 RVA: 0x000C84F8 File Offset: 0x000C66F8
	public RocketOxidizerTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x06003751 RID: 14161 RVA: 0x00223B90 File Offset: 0x00221D90
	public override void UpdateData()
	{
		Clustercraft component = ClusterManager.Instance.GetWorld(base.WorldID).GetComponent<Clustercraft>();
		base.AddPoint((component != null) ? component.ModuleInterface.OxidizerPowerRemaining : 0f);
	}

	// Token: 0x06003752 RID: 14162 RVA: 0x000C6E5E File Offset: 0x000C505E
	public override string FormatValueString(float value)
	{
		return GameUtil.GetFormattedMass(value, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}");
	}
}
