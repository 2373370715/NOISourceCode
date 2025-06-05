using System;

// Token: 0x02001684 RID: 5764
public class ModuleGenerator : Generator
{
	// Token: 0x06007719 RID: 30489 RVA: 0x000F2C6B File Offset: 0x000F0E6B
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.connectedTags = new Tag[0];
		base.IsVirtual = true;
	}

	// Token: 0x0600771A RID: 30490 RVA: 0x0031A28C File Offset: 0x0031848C
	protected override void OnSpawn()
	{
		CraftModuleInterface craftInterface = base.GetComponent<RocketModuleCluster>().CraftInterface;
		base.VirtualCircuitKey = craftInterface;
		this.clustercraft = craftInterface.GetComponent<Clustercraft>();
		Game.Instance.electricalConduitSystem.AddToVirtualNetworks(base.VirtualCircuitKey, this, true);
		base.OnSpawn();
	}

	// Token: 0x0600771B RID: 30491 RVA: 0x000F2C86 File Offset: 0x000F0E86
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Game.Instance.electricalConduitSystem.RemoveFromVirtualNetworks(base.VirtualCircuitKey, this, true);
	}

	// Token: 0x0600771C RID: 30492 RVA: 0x000F2CA5 File Offset: 0x000F0EA5
	public override bool IsProducingPower()
	{
		return this.clustercraft.IsFlightInProgress();
	}

	// Token: 0x0600771D RID: 30493 RVA: 0x0031A2D8 File Offset: 0x003184D8
	public override void EnergySim200ms(float dt)
	{
		base.EnergySim200ms(dt);
		if (this.IsProducingPower())
		{
			base.GenerateJoules(base.WattageRating * dt, false);
			if (this.poweringStatusItemHandle == Guid.Empty)
			{
				this.poweringStatusItemHandle = this.selectable.ReplaceStatusItem(this.notPoweringStatusItemHandle, Db.Get().BuildingStatusItems.ModuleGeneratorPowered, this);
				this.notPoweringStatusItemHandle = Guid.Empty;
				return;
			}
		}
		else if (this.notPoweringStatusItemHandle == Guid.Empty)
		{
			this.notPoweringStatusItemHandle = this.selectable.ReplaceStatusItem(this.poweringStatusItemHandle, Db.Get().BuildingStatusItems.ModuleGeneratorNotPowered, this);
			this.poweringStatusItemHandle = Guid.Empty;
		}
	}

	// Token: 0x040059A9 RID: 22953
	private Clustercraft clustercraft;

	// Token: 0x040059AA RID: 22954
	private Guid poweringStatusItemHandle;

	// Token: 0x040059AB RID: 22955
	private Guid notPoweringStatusItemHandle;
}
