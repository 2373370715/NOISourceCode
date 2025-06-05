using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000E87 RID: 3719
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicGateBuffer : LogicGate, ISingleSliderControl, ISliderControl
{
	// Token: 0x170003A6 RID: 934
	// (get) Token: 0x0600494E RID: 18766 RVA: 0x000D4025 File Offset: 0x000D2225
	// (set) Token: 0x0600494F RID: 18767 RVA: 0x00267BF8 File Offset: 0x00265DF8
	public float DelayAmount
	{
		get
		{
			return this.delayAmount;
		}
		set
		{
			this.delayAmount = value;
			int delayAmountTicks = this.DelayAmountTicks;
			if (this.delayTicksRemaining > delayAmountTicks)
			{
				this.delayTicksRemaining = delayAmountTicks;
			}
		}
	}

	// Token: 0x170003A7 RID: 935
	// (get) Token: 0x06004950 RID: 18768 RVA: 0x000D402D File Offset: 0x000D222D
	private int DelayAmountTicks
	{
		get
		{
			return Mathf.RoundToInt(this.delayAmount / LogicCircuitManager.ClockTickInterval);
		}
	}

	// Token: 0x170003A8 RID: 936
	// (get) Token: 0x06004951 RID: 18769 RVA: 0x000D4040 File Offset: 0x000D2240
	public string SliderTitleKey
	{
		get
		{
			return "STRINGS.UI.UISIDESCREENS.LOGIC_BUFFER_SIDE_SCREEN.TITLE";
		}
	}

	// Token: 0x170003A9 RID: 937
	// (get) Token: 0x06004952 RID: 18770 RVA: 0x000D4047 File Offset: 0x000D2247
	public string SliderUnits
	{
		get
		{
			return UI.UNITSUFFIXES.SECOND;
		}
	}

	// Token: 0x06004953 RID: 18771 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public int SliderDecimalPlaces(int index)
	{
		return 1;
	}

	// Token: 0x06004954 RID: 18772 RVA: 0x000D4053 File Offset: 0x000D2253
	public float GetSliderMin(int index)
	{
		return 0.1f;
	}

	// Token: 0x06004955 RID: 18773 RVA: 0x000D405A File Offset: 0x000D225A
	public float GetSliderMax(int index)
	{
		return 200f;
	}

	// Token: 0x06004956 RID: 18774 RVA: 0x000D4061 File Offset: 0x000D2261
	public float GetSliderValue(int index)
	{
		return this.DelayAmount;
	}

	// Token: 0x06004957 RID: 18775 RVA: 0x000D4069 File Offset: 0x000D2269
	public void SetSliderValue(float value, int index)
	{
		this.DelayAmount = value;
	}

	// Token: 0x06004958 RID: 18776 RVA: 0x000D4072 File Offset: 0x000D2272
	public string GetSliderTooltipKey(int index)
	{
		return "STRINGS.UI.UISIDESCREENS.LOGIC_BUFFER_SIDE_SCREEN.TOOLTIP";
	}

	// Token: 0x06004959 RID: 18777 RVA: 0x000D4079 File Offset: 0x000D2279
	string ISliderControl.GetSliderTooltip(int index)
	{
		return string.Format(Strings.Get("STRINGS.UI.UISIDESCREENS.LOGIC_BUFFER_SIDE_SCREEN.TOOLTIP"), this.DelayAmount);
	}

	// Token: 0x0600495A RID: 18778 RVA: 0x000D409A File Offset: 0x000D229A
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicGateBuffer>(-905833192, LogicGateBuffer.OnCopySettingsDelegate);
	}

	// Token: 0x0600495B RID: 18779 RVA: 0x00267C24 File Offset: 0x00265E24
	private void OnCopySettings(object data)
	{
		LogicGateBuffer component = ((GameObject)data).GetComponent<LogicGateBuffer>();
		if (component != null)
		{
			this.DelayAmount = component.DelayAmount;
		}
	}

	// Token: 0x0600495C RID: 18780 RVA: 0x00267C54 File Offset: 0x00265E54
	protected override void OnSpawn()
	{
		base.OnSpawn();
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		this.meter = new MeterController(component, "meter_target", "meter", Meter.Offset.UserSpecified, Grid.SceneLayer.LogicGatesFront, Vector3.zero, null);
		this.meter.SetPositionPercent(1f);
	}

	// Token: 0x0600495D RID: 18781 RVA: 0x00267CA0 File Offset: 0x00265EA0
	private void Update()
	{
		float positionPercent;
		if (this.input_was_previously_positive)
		{
			positionPercent = 0f;
		}
		else if (this.delayTicksRemaining > 0)
		{
			positionPercent = (float)(this.DelayAmountTicks - this.delayTicksRemaining) / (float)this.DelayAmountTicks;
		}
		else
		{
			positionPercent = 1f;
		}
		this.meter.SetPositionPercent(positionPercent);
	}

	// Token: 0x0600495E RID: 18782 RVA: 0x000D40B3 File Offset: 0x000D22B3
	public override void LogicTick()
	{
		if (!this.input_was_previously_positive && this.delayTicksRemaining > 0)
		{
			this.delayTicksRemaining--;
			if (this.delayTicksRemaining <= 0)
			{
				this.OnDelay();
			}
		}
	}

	// Token: 0x0600495F RID: 18783 RVA: 0x00267CF8 File Offset: 0x00265EF8
	protected override int GetCustomValue(int val1, int val2)
	{
		if (val1 != 0)
		{
			this.input_was_previously_positive = true;
			this.delayTicksRemaining = 0;
			this.meter.SetPositionPercent(0f);
		}
		else if (this.delayTicksRemaining <= 0)
		{
			if (this.input_was_previously_positive)
			{
				this.delayTicksRemaining = this.DelayAmountTicks;
			}
			this.input_was_previously_positive = false;
		}
		if (val1 == 0 && this.delayTicksRemaining <= 0)
		{
			return 0;
		}
		return 1;
	}

	// Token: 0x06004960 RID: 18784 RVA: 0x00267D5C File Offset: 0x00265F5C
	private void OnDelay()
	{
		if (this.cleaningUp)
		{
			return;
		}
		this.delayTicksRemaining = 0;
		this.meter.SetPositionPercent(1f);
		if (this.outputValueOne == 0)
		{
			return;
		}
		int outputCellOne = base.OutputCellOne;
		if (!(Game.Instance.logicCircuitSystem.GetNetworkForCell(outputCellOne) is LogicCircuitNetwork))
		{
			return;
		}
		this.outputValueOne = 0;
		base.RefreshAnimation();
	}

	// Token: 0x040033C6 RID: 13254
	[Serialize]
	private bool input_was_previously_positive;

	// Token: 0x040033C7 RID: 13255
	[Serialize]
	private float delayAmount = 5f;

	// Token: 0x040033C8 RID: 13256
	[Serialize]
	private int delayTicksRemaining;

	// Token: 0x040033C9 RID: 13257
	private MeterController meter;

	// Token: 0x040033CA RID: 13258
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x040033CB RID: 13259
	private static readonly EventSystem.IntraObjectHandler<LogicGateBuffer> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicGateBuffer>(delegate(LogicGateBuffer component, object data)
	{
		component.OnCopySettings(data);
	});
}
