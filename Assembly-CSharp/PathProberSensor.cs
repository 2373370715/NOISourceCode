using System;

// Token: 0x0200084F RID: 2127
public class PathProberSensor : Sensor
{
	// Token: 0x06002584 RID: 9604 RVA: 0x000BCFD5 File Offset: 0x000BB1D5
	public PathProberSensor(Sensors sensors) : base(sensors)
	{
		this.navigator = sensors.GetComponent<Navigator>();
	}

	// Token: 0x06002585 RID: 9605 RVA: 0x000BCFEA File Offset: 0x000BB1EA
	public override void Update()
	{
		this.navigator.UpdateProbe(false);
	}

	// Token: 0x040019DF RID: 6623
	private Navigator navigator;
}
