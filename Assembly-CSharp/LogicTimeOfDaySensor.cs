using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000EA5 RID: 3749
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicTimeOfDaySensor : Switch, ISaveLoadable, ISim200ms
{
	// Token: 0x06004ABB RID: 19131 RVA: 0x000D4CA7 File Offset: 0x000D2EA7
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicTimeOfDaySensor>(-905833192, LogicTimeOfDaySensor.OnCopySettingsDelegate);
	}

	// Token: 0x06004ABC RID: 19132 RVA: 0x0026A330 File Offset: 0x00268530
	private void OnCopySettings(object data)
	{
		LogicTimeOfDaySensor component = ((GameObject)data).GetComponent<LogicTimeOfDaySensor>();
		if (component != null)
		{
			this.startTime = component.startTime;
			this.duration = component.duration;
		}
	}

	// Token: 0x06004ABD RID: 19133 RVA: 0x000D4CC0 File Offset: 0x000D2EC0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.OnToggle += this.OnSwitchToggled;
		this.UpdateLogicCircuit();
		this.UpdateVisualState(true);
		this.wasOn = this.switchedOn;
	}

	// Token: 0x06004ABE RID: 19134 RVA: 0x0026A36C File Offset: 0x0026856C
	public void Sim200ms(float dt)
	{
		float currentCycleAsPercentage = GameClock.Instance.GetCurrentCycleAsPercentage();
		bool state = false;
		if (currentCycleAsPercentage >= this.startTime && currentCycleAsPercentage < this.startTime + this.duration)
		{
			state = true;
		}
		if (currentCycleAsPercentage < this.startTime + this.duration - 1f)
		{
			state = true;
		}
		this.SetState(state);
	}

	// Token: 0x06004ABF RID: 19135 RVA: 0x000D4CF3 File Offset: 0x000D2EF3
	private void OnSwitchToggled(bool toggled_on)
	{
		this.UpdateLogicCircuit();
		this.UpdateVisualState(false);
	}

	// Token: 0x06004AC0 RID: 19136 RVA: 0x000CEE6A File Offset: 0x000CD06A
	private void UpdateLogicCircuit()
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);
	}

	// Token: 0x06004AC1 RID: 19137 RVA: 0x0026A3C0 File Offset: 0x002685C0
	private void UpdateVisualState(bool force = false)
	{
		if (this.wasOn != this.switchedOn || force)
		{
			this.wasOn = this.switchedOn;
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			component.Play(this.switchedOn ? "on_pre" : "on_pst", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue(this.switchedOn ? "on" : "off", KAnim.PlayMode.Once, 1f, 0f);
		}
	}

	// Token: 0x06004AC2 RID: 19138 RVA: 0x0026473C File Offset: 0x0026293C
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x04003472 RID: 13426
	[SerializeField]
	[Serialize]
	public float startTime;

	// Token: 0x04003473 RID: 13427
	[SerializeField]
	[Serialize]
	public float duration = 1f;

	// Token: 0x04003474 RID: 13428
	private bool wasOn;

	// Token: 0x04003475 RID: 13429
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04003476 RID: 13430
	private static readonly EventSystem.IntraObjectHandler<LogicTimeOfDaySensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicTimeOfDaySensor>(delegate(LogicTimeOfDaySensor component, object data)
	{
		component.OnCopySettings(data);
	});
}
