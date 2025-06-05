using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000EA7 RID: 3751
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicTimerSensor : Switch, ISaveLoadable, ISim33ms
{
	// Token: 0x06004AC8 RID: 19144 RVA: 0x000D4D46 File Offset: 0x000D2F46
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicTimerSensor>(-905833192, LogicTimerSensor.OnCopySettingsDelegate);
	}

	// Token: 0x06004AC9 RID: 19145 RVA: 0x0026A448 File Offset: 0x00268648
	private void OnCopySettings(object data)
	{
		LogicTimerSensor component = ((GameObject)data).GetComponent<LogicTimerSensor>();
		if (component != null)
		{
			this.onDuration = component.onDuration;
			this.offDuration = component.offDuration;
			this.timeElapsedInCurrentState = component.timeElapsedInCurrentState;
			this.displayCyclesMode = component.displayCyclesMode;
		}
	}

	// Token: 0x06004ACA RID: 19146 RVA: 0x000D4D5F File Offset: 0x000D2F5F
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.OnToggle += this.OnSwitchToggled;
		this.UpdateLogicCircuit();
		this.UpdateVisualState(true);
		this.wasOn = this.switchedOn;
	}

	// Token: 0x06004ACB RID: 19147 RVA: 0x0026A49C File Offset: 0x0026869C
	public void Sim33ms(float dt)
	{
		if (this.onDuration == 0f && this.offDuration == 0f)
		{
			return;
		}
		this.timeElapsedInCurrentState += dt;
		bool flag = base.IsSwitchedOn;
		if (flag)
		{
			if (this.timeElapsedInCurrentState >= this.onDuration)
			{
				flag = false;
				this.timeElapsedInCurrentState -= this.onDuration;
			}
		}
		else if (this.timeElapsedInCurrentState >= this.offDuration)
		{
			flag = true;
			this.timeElapsedInCurrentState -= this.offDuration;
		}
		this.SetState(flag);
	}

	// Token: 0x06004ACC RID: 19148 RVA: 0x000D4D92 File Offset: 0x000D2F92
	private void OnSwitchToggled(bool toggled_on)
	{
		this.UpdateLogicCircuit();
		this.UpdateVisualState(false);
	}

	// Token: 0x06004ACD RID: 19149 RVA: 0x000CEE6A File Offset: 0x000CD06A
	private void UpdateLogicCircuit()
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);
	}

	// Token: 0x06004ACE RID: 19150 RVA: 0x0026A52C File Offset: 0x0026872C
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

	// Token: 0x06004ACF RID: 19151 RVA: 0x0026473C File Offset: 0x0026293C
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x06004AD0 RID: 19152 RVA: 0x000D4DA1 File Offset: 0x000D2FA1
	public void ResetTimer()
	{
		this.SetState(true);
		this.OnSwitchToggled(true);
		this.timeElapsedInCurrentState = 0f;
	}

	// Token: 0x04003478 RID: 13432
	[Serialize]
	public float onDuration = 10f;

	// Token: 0x04003479 RID: 13433
	[Serialize]
	public float offDuration = 10f;

	// Token: 0x0400347A RID: 13434
	[Serialize]
	public bool displayCyclesMode;

	// Token: 0x0400347B RID: 13435
	private bool wasOn;

	// Token: 0x0400347C RID: 13436
	[SerializeField]
	[Serialize]
	public float timeElapsedInCurrentState;

	// Token: 0x0400347D RID: 13437
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x0400347E RID: 13438
	private static readonly EventSystem.IntraObjectHandler<LogicTimerSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicTimerSensor>(delegate(LogicTimerSensor component, object data)
	{
		component.OnCopySettings(data);
	});
}
