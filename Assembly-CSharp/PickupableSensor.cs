using System;

// Token: 0x02000850 RID: 2128
public class PickupableSensor : Sensor
{
	// Token: 0x06002586 RID: 9606 RVA: 0x000BCFF8 File Offset: 0x000BB1F8
	public PickupableSensor(Sensors sensors) : base(sensors)
	{
		this.worker = base.GetComponent<WorkerBase>();
		this.pathProber = base.GetComponent<PathProber>();
	}

	// Token: 0x06002587 RID: 9607 RVA: 0x000BD019 File Offset: 0x000BB219
	public override void Update()
	{
		GlobalChoreProvider.Instance.UpdateFetches(this.pathProber);
		Game.Instance.fetchManager.UpdatePickups(this.pathProber, this.worker);
	}

	// Token: 0x040019E0 RID: 6624
	private PathProber pathProber;

	// Token: 0x040019E1 RID: 6625
	private WorkerBase worker;
}
