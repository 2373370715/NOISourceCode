using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000E73 RID: 3699
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicCounter : Switch, ISaveLoadable
{
	// Token: 0x06004889 RID: 18569 RVA: 0x000D3832 File Offset: 0x000D1A32
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicCounter>(-905833192, LogicCounter.OnCopySettingsDelegate);
	}

	// Token: 0x0600488A RID: 18570 RVA: 0x00264790 File Offset: 0x00262990
	private void OnCopySettings(object data)
	{
		LogicCounter component = ((GameObject)data).GetComponent<LogicCounter>();
		if (component != null)
		{
			this.maxCount = component.maxCount;
			this.resetCountAtMax = component.resetCountAtMax;
			this.advancedMode = component.advancedMode;
		}
	}

	// Token: 0x0600488B RID: 18571 RVA: 0x002647D8 File Offset: 0x002629D8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.OnToggle += this.OnSwitchToggled;
		LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
		logicCircuitManager.onLogicTick = (System.Action)Delegate.Combine(logicCircuitManager.onLogicTick, new System.Action(this.LogicTick));
		if (this.maxCount == 0)
		{
			this.maxCount = 10;
		}
		base.Subscribe<LogicCounter>(-801688580, LogicCounter.OnLogicValueChangedDelegate);
		this.UpdateLogicCircuit();
		this.UpdateVisualState(true);
		this.wasOn = this.switchedOn;
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		this.meter = new MeterController(component, "meter_target", component.FlipY ? "meter_dn" : "meter_up", Meter.Offset.UserSpecified, Grid.SceneLayer.LogicGatesFront, Vector3.zero, null);
		this.UpdateMeter();
	}

	// Token: 0x0600488C RID: 18572 RVA: 0x000D384B File Offset: 0x000D1A4B
	protected override void OnCleanUp()
	{
		LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
		logicCircuitManager.onLogicTick = (System.Action)Delegate.Remove(logicCircuitManager.onLogicTick, new System.Action(this.LogicTick));
	}

	// Token: 0x0600488D RID: 18573 RVA: 0x000D3878 File Offset: 0x000D1A78
	private void OnSwitchToggled(bool toggled_on)
	{
		this.UpdateLogicCircuit();
		this.UpdateVisualState(false);
	}

	// Token: 0x0600488E RID: 18574 RVA: 0x000D3887 File Offset: 0x000D1A87
	public void UpdateLogicCircuit()
	{
		if (this.receivedFirstSignal)
		{
			base.GetComponent<LogicPorts>().SendSignal(LogicCounter.OUTPUT_PORT_ID, this.switchedOn ? 1 : 0);
		}
	}

	// Token: 0x0600488F RID: 18575 RVA: 0x002648A0 File Offset: 0x00262AA0
	public void UpdateMeter()
	{
		float num = (float)(this.currentCount % (this.advancedMode ? this.maxCount : 10));
		this.meter.SetPositionPercent(num / 9f);
	}

	// Token: 0x06004890 RID: 18576 RVA: 0x002648DC File Offset: 0x00262ADC
	public void UpdateVisualState(bool force = false)
	{
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		if (!this.receivedFirstSignal)
		{
			component.Play("off", KAnim.PlayMode.Once, 1f, 0f);
			return;
		}
		if (this.wasOn != this.switchedOn || force)
		{
			int num = (this.switchedOn ? 4 : 0) + (this.wasResetting ? 2 : 0) + (this.wasIncrementing ? 1 : 0);
			this.wasOn = this.switchedOn;
			component.Play("on_" + num.ToString(), KAnim.PlayMode.Once, 1f, 0f);
		}
	}

	// Token: 0x06004891 RID: 18577 RVA: 0x00264984 File Offset: 0x00262B84
	public void OnLogicValueChanged(object data)
	{
		LogicValueChanged logicValueChanged = (LogicValueChanged)data;
		if (logicValueChanged.portID == LogicCounter.INPUT_PORT_ID)
		{
			int newValue = logicValueChanged.newValue;
			this.receivedFirstSignal = true;
			if (LogicCircuitNetwork.IsBitActive(0, newValue))
			{
				if (!this.wasIncrementing)
				{
					this.wasIncrementing = true;
					if (!this.wasResetting)
					{
						if (this.currentCount == this.maxCount || this.currentCount >= 10)
						{
							this.currentCount = 0;
						}
						this.currentCount++;
						this.UpdateMeter();
						this.SetCounterState();
						if (this.currentCount == this.maxCount && this.resetCountAtMax)
						{
							this.resetRequested = true;
						}
					}
				}
			}
			else
			{
				this.wasIncrementing = false;
			}
		}
		else
		{
			if (!(logicValueChanged.portID == LogicCounter.RESET_PORT_ID))
			{
				return;
			}
			int newValue2 = logicValueChanged.newValue;
			this.receivedFirstSignal = true;
			if (LogicCircuitNetwork.IsBitActive(0, newValue2))
			{
				if (!this.wasResetting)
				{
					this.wasResetting = true;
					this.ResetCounter();
				}
			}
			else
			{
				this.wasResetting = false;
			}
		}
		this.UpdateVisualState(true);
		this.UpdateLogicCircuit();
	}

	// Token: 0x06004892 RID: 18578 RVA: 0x0026473C File Offset: 0x0026293C
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x06004893 RID: 18579 RVA: 0x000D38AD File Offset: 0x000D1AAD
	public void ResetCounter()
	{
		this.resetRequested = false;
		this.currentCount = 0;
		this.SetState(false);
		if (this.advancedMode)
		{
			this.pulsingActive = false;
			this.pulseTicksRemaining = 0;
		}
		this.UpdateVisualState(true);
		this.UpdateMeter();
		this.UpdateLogicCircuit();
	}

	// Token: 0x06004894 RID: 18580 RVA: 0x00264A9C File Offset: 0x00262C9C
	public void LogicTick()
	{
		if (this.resetRequested)
		{
			this.ResetCounter();
		}
		if (this.pulsingActive)
		{
			this.pulseTicksRemaining--;
			if (this.pulseTicksRemaining <= 0)
			{
				this.pulsingActive = false;
				this.SetState(false);
				this.UpdateVisualState(false);
				this.UpdateMeter();
				this.UpdateLogicCircuit();
			}
		}
	}

	// Token: 0x06004895 RID: 18581 RVA: 0x00264AF8 File Offset: 0x00262CF8
	public void SetCounterState()
	{
		this.SetState(this.advancedMode ? (this.currentCount % this.maxCount == 0) : (this.currentCount == this.maxCount));
		if (this.advancedMode && this.currentCount % this.maxCount == 0)
		{
			this.pulsingActive = true;
			this.pulseTicksRemaining = 2;
		}
	}

	// Token: 0x040032EC RID: 13036
	[Serialize]
	public int maxCount;

	// Token: 0x040032ED RID: 13037
	[Serialize]
	public int currentCount;

	// Token: 0x040032EE RID: 13038
	[Serialize]
	public bool resetCountAtMax;

	// Token: 0x040032EF RID: 13039
	[Serialize]
	public bool advancedMode;

	// Token: 0x040032F0 RID: 13040
	private bool wasOn;

	// Token: 0x040032F1 RID: 13041
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x040032F2 RID: 13042
	private static readonly EventSystem.IntraObjectHandler<LogicCounter> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicCounter>(delegate(LogicCounter component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x040032F3 RID: 13043
	private static readonly EventSystem.IntraObjectHandler<LogicCounter> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<LogicCounter>(delegate(LogicCounter component, object data)
	{
		component.OnLogicValueChanged(data);
	});

	// Token: 0x040032F4 RID: 13044
	public static readonly HashedString INPUT_PORT_ID = new HashedString("LogicCounterInput");

	// Token: 0x040032F5 RID: 13045
	public static readonly HashedString RESET_PORT_ID = new HashedString("LogicCounterReset");

	// Token: 0x040032F6 RID: 13046
	public static readonly HashedString OUTPUT_PORT_ID = new HashedString("LogicCounterOutput");

	// Token: 0x040032F7 RID: 13047
	private bool resetRequested;

	// Token: 0x040032F8 RID: 13048
	[Serialize]
	private bool wasResetting;

	// Token: 0x040032F9 RID: 13049
	[Serialize]
	private bool wasIncrementing;

	// Token: 0x040032FA RID: 13050
	[Serialize]
	public bool receivedFirstSignal;

	// Token: 0x040032FB RID: 13051
	private bool pulsingActive;

	// Token: 0x040032FC RID: 13052
	private const int pulseLength = 1;

	// Token: 0x040032FD RID: 13053
	private int pulseTicksRemaining;

	// Token: 0x040032FE RID: 13054
	private MeterController meter;
}
