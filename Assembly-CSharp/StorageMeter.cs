using System;

// Token: 0x02000FFB RID: 4091
public class StorageMeter : KMonoBehaviour
{
	// Token: 0x06005268 RID: 21096 RVA: 0x000DA191 File Offset: 0x000D8391
	public void SetInterpolateFunction(Func<float, int, float> func)
	{
		this.interpolateFunction = func;
		if (this.meter != null)
		{
			this.meter.interpolateFunction = this.interpolateFunction;
		}
	}

	// Token: 0x06005269 RID: 21097 RVA: 0x000B74E6 File Offset: 0x000B56E6
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x0600526A RID: 21098 RVA: 0x00283024 File Offset: 0x00281224
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target",
			"meter_frame",
			"meter_level"
		});
		this.meter.interpolateFunction = this.interpolateFunction;
		this.UpdateMeter(null);
		base.Subscribe(-1697596308, new Action<object>(this.UpdateMeter));
	}

	// Token: 0x0600526B RID: 21099 RVA: 0x000DA1B3 File Offset: 0x000D83B3
	private void UpdateMeter(object data)
	{
		this.meter.SetPositionPercent(this.storage.MassStored() / this.storage.Capacity());
	}

	// Token: 0x04003A2F RID: 14895
	[MyCmpGet]
	private Storage storage;

	// Token: 0x04003A30 RID: 14896
	private MeterController meter;

	// Token: 0x04003A31 RID: 14897
	private Func<float, int, float> interpolateFunction = new Func<float, int, float>(MeterController.MinMaxStepLerp);
}
