using System;
using KSerialization;

// Token: 0x02000F51 RID: 3921
[SerializationConfig(MemberSerialization.OptIn)]
public class PlantAirConditioner : AirConditioner
{
	// Token: 0x06004E8A RID: 20106 RVA: 0x000D766B File Offset: 0x000D586B
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<PlantAirConditioner>(-1396791468, PlantAirConditioner.OnFertilizedDelegate);
		base.Subscribe<PlantAirConditioner>(-1073674739, PlantAirConditioner.OnUnfertilizedDelegate);
	}

	// Token: 0x06004E8B RID: 20107 RVA: 0x000D7695 File Offset: 0x000D5895
	private void OnFertilized(object data)
	{
		this.operational.SetFlag(PlantAirConditioner.fertilizedFlag, true);
	}

	// Token: 0x06004E8C RID: 20108 RVA: 0x000D76A8 File Offset: 0x000D58A8
	private void OnUnfertilized(object data)
	{
		this.operational.SetFlag(PlantAirConditioner.fertilizedFlag, false);
	}

	// Token: 0x04003729 RID: 14121
	private static readonly Operational.Flag fertilizedFlag = new Operational.Flag("fertilized", Operational.Flag.Type.Requirement);

	// Token: 0x0400372A RID: 14122
	private static readonly EventSystem.IntraObjectHandler<PlantAirConditioner> OnFertilizedDelegate = new EventSystem.IntraObjectHandler<PlantAirConditioner>(delegate(PlantAirConditioner component, object data)
	{
		component.OnFertilized(data);
	});

	// Token: 0x0400372B RID: 14123
	private static readonly EventSystem.IntraObjectHandler<PlantAirConditioner> OnUnfertilizedDelegate = new EventSystem.IntraObjectHandler<PlantAirConditioner>(delegate(PlantAirConditioner component, object data)
	{
		component.OnUnfertilized(data);
	});
}
