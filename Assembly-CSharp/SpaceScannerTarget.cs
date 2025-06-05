using System;

// Token: 0x020019EC RID: 6636
public readonly struct SpaceScannerTarget
{
	// Token: 0x06008A57 RID: 35415 RVA: 0x000FEF57 File Offset: 0x000FD157
	private SpaceScannerTarget(string id)
	{
		this.id = id;
	}

	// Token: 0x06008A58 RID: 35416 RVA: 0x000FEF60 File Offset: 0x000FD160
	public static SpaceScannerTarget MeteorShower()
	{
		return new SpaceScannerTarget("meteor_shower");
	}

	// Token: 0x06008A59 RID: 35417 RVA: 0x000FEF6C File Offset: 0x000FD16C
	public static SpaceScannerTarget BallisticObject()
	{
		return new SpaceScannerTarget("ballistic_object");
	}

	// Token: 0x06008A5A RID: 35418 RVA: 0x000FEF78 File Offset: 0x000FD178
	public static SpaceScannerTarget RocketBaseGame(LaunchConditionManager rocket)
	{
		return new SpaceScannerTarget(string.Format("rocket_base_game::{0}", rocket.GetComponent<KPrefabID>().InstanceID));
	}

	// Token: 0x06008A5B RID: 35419 RVA: 0x000FEF99 File Offset: 0x000FD199
	public static SpaceScannerTarget RocketDlc1(Clustercraft rocket)
	{
		return new SpaceScannerTarget(string.Format("rocket_dlc1::{0}", rocket.GetComponent<KPrefabID>().InstanceID));
	}

	// Token: 0x04006863 RID: 26723
	public readonly string id;
}
