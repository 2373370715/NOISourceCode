using System;
using UnityEngine;

// Token: 0x02001805 RID: 6149
[AddComponentMenu("KMonoBehaviour/scripts/Reservoir")]
public class Reservoir : KMonoBehaviour
{
	// Token: 0x06007E90 RID: 32400 RVA: 0x00337ACC File Offset: 0x00335CCC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_fill",
			"meter_OL"
		});
		base.Subscribe<Reservoir>(-1697596308, Reservoir.OnStorageChangeDelegate);
		this.OnStorageChange(null);
	}

	// Token: 0x06007E91 RID: 32401 RVA: 0x000F7CB8 File Offset: 0x000F5EB8
	private void OnStorageChange(object data)
	{
		this.meter.SetPositionPercent(Mathf.Clamp01(this.storage.MassStored() / this.storage.capacityKg));
	}

	// Token: 0x04006025 RID: 24613
	private MeterController meter;

	// Token: 0x04006026 RID: 24614
	[MyCmpGet]
	private Storage storage;

	// Token: 0x04006027 RID: 24615
	private static readonly EventSystem.IntraObjectHandler<Reservoir> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<Reservoir>(delegate(Reservoir component, object data)
	{
		component.OnStorageChange(data);
	});
}
