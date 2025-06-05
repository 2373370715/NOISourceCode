using System;
using System.Diagnostics;

// Token: 0x02000F5E RID: 3934
[DebuggerDisplay("{name}")]
public class PowerTransformer : Generator
{
	// Token: 0x06004ED3 RID: 20179 RVA: 0x000D7A5D File Offset: 0x000D5C5D
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.battery = base.GetComponent<Battery>();
		base.Subscribe<PowerTransformer>(-592767678, PowerTransformer.OnOperationalChangedDelegate);
		this.UpdateJoulesLostPerSecond();
	}

	// Token: 0x06004ED4 RID: 20180 RVA: 0x000D7A88 File Offset: 0x000D5C88
	public override void ApplyDeltaJoules(float joules_delta, bool can_over_power = false)
	{
		this.battery.ConsumeEnergy(-joules_delta);
		base.ApplyDeltaJoules(joules_delta, can_over_power);
	}

	// Token: 0x06004ED5 RID: 20181 RVA: 0x000D7A9F File Offset: 0x000D5C9F
	public override void ConsumeEnergy(float joules)
	{
		this.battery.ConsumeEnergy(joules);
		base.ConsumeEnergy(joules);
	}

	// Token: 0x06004ED6 RID: 20182 RVA: 0x000D7AB4 File Offset: 0x000D5CB4
	private void OnOperationalChanged(object data)
	{
		this.UpdateJoulesLostPerSecond();
	}

	// Token: 0x06004ED7 RID: 20183 RVA: 0x000D7ABC File Offset: 0x000D5CBC
	private void UpdateJoulesLostPerSecond()
	{
		if (this.operational.IsOperational)
		{
			this.battery.joulesLostPerSecond = 0f;
			return;
		}
		this.battery.joulesLostPerSecond = 3.3333333f;
	}

	// Token: 0x06004ED8 RID: 20184 RVA: 0x00277A3C File Offset: 0x00275C3C
	public override void EnergySim200ms(float dt)
	{
		base.EnergySim200ms(dt);
		float joulesAvailable = this.operational.IsOperational ? Math.Min(this.battery.JoulesAvailable, base.WattageRating * dt) : 0f;
		base.AssignJoulesAvailable(joulesAvailable);
		ushort circuitID = this.battery.CircuitID;
		ushort circuitID2 = base.CircuitID;
		bool flag = circuitID == circuitID2 && circuitID != ushort.MaxValue;
		if (this.mLoopDetected != flag)
		{
			this.mLoopDetected = flag;
			this.selectable.ToggleStatusItem(Db.Get().BuildingStatusItems.PowerLoopDetected, this.mLoopDetected, this);
		}
	}

	// Token: 0x04003757 RID: 14167
	private Battery battery;

	// Token: 0x04003758 RID: 14168
	private bool mLoopDetected;

	// Token: 0x04003759 RID: 14169
	private static readonly EventSystem.IntraObjectHandler<PowerTransformer> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<PowerTransformer>(delegate(PowerTransformer component, object data)
	{
		component.OnOperationalChanged(data);
	});
}
