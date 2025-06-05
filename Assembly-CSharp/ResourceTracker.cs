using System;

// Token: 0x02000B7B RID: 2939
public class ResourceTracker : WorldTracker
{
	// Token: 0x17000267 RID: 615
	// (get) Token: 0x06003733 RID: 14131 RVA: 0x000C8565 File Offset: 0x000C6765
	// (set) Token: 0x06003734 RID: 14132 RVA: 0x000C856D File Offset: 0x000C676D
	public Tag tag { get; private set; }

	// Token: 0x06003735 RID: 14133 RVA: 0x000C8576 File Offset: 0x000C6776
	public ResourceTracker(int worldID, Tag materialCategoryTag) : base(worldID)
	{
		this.tag = materialCategoryTag;
	}

	// Token: 0x06003736 RID: 14134 RVA: 0x00223784 File Offset: 0x00221984
	public override void UpdateData()
	{
		if (ClusterManager.Instance.GetWorld(base.WorldID).worldInventory == null)
		{
			return;
		}
		base.AddPoint(ClusterManager.Instance.GetWorld(base.WorldID).worldInventory.GetAmount(this.tag, false));
	}

	// Token: 0x06003737 RID: 14135 RVA: 0x000C6E5E File Offset: 0x000C505E
	public override string FormatValueString(float value)
	{
		return GameUtil.GetFormattedMass(value, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}");
	}
}
