using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000E89 RID: 3721
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicGateFilter : LogicGate, ISingleSliderControl, ISliderControl
{
	// Token: 0x170003AA RID: 938
	// (get) Token: 0x06004966 RID: 18790 RVA: 0x000D4127 File Offset: 0x000D2327
	// (set) Token: 0x06004967 RID: 18791 RVA: 0x00267DC0 File Offset: 0x00265FC0
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

	// Token: 0x170003AB RID: 939
	// (get) Token: 0x06004968 RID: 18792 RVA: 0x000D412F File Offset: 0x000D232F
	private int DelayAmountTicks
	{
		get
		{
			return Mathf.RoundToInt(this.delayAmount / LogicCircuitManager.ClockTickInterval);
		}
	}

	// Token: 0x170003AC RID: 940
	// (get) Token: 0x06004969 RID: 18793 RVA: 0x000D4142 File Offset: 0x000D2342
	public string SliderTitleKey
	{
		get
		{
			return "STRINGS.UI.UISIDESCREENS.LOGIC_FILTER_SIDE_SCREEN.TITLE";
		}
	}

	// Token: 0x170003AD RID: 941
	// (get) Token: 0x0600496A RID: 18794 RVA: 0x000D4047 File Offset: 0x000D2247
	public string SliderUnits
	{
		get
		{
			return UI.UNITSUFFIXES.SECOND;
		}
	}

	// Token: 0x0600496B RID: 18795 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public int SliderDecimalPlaces(int index)
	{
		return 1;
	}

	// Token: 0x0600496C RID: 18796 RVA: 0x000D4053 File Offset: 0x000D2253
	public float GetSliderMin(int index)
	{
		return 0.1f;
	}

	// Token: 0x0600496D RID: 18797 RVA: 0x000D405A File Offset: 0x000D225A
	public float GetSliderMax(int index)
	{
		return 200f;
	}

	// Token: 0x0600496E RID: 18798 RVA: 0x000D4149 File Offset: 0x000D2349
	public float GetSliderValue(int index)
	{
		return this.DelayAmount;
	}

	// Token: 0x0600496F RID: 18799 RVA: 0x000D4151 File Offset: 0x000D2351
	public void SetSliderValue(float value, int index)
	{
		this.DelayAmount = value;
	}

	// Token: 0x06004970 RID: 18800 RVA: 0x000D415A File Offset: 0x000D235A
	public string GetSliderTooltipKey(int index)
	{
		return "STRINGS.UI.UISIDESCREENS.LOGIC_FILTER_SIDE_SCREEN.TOOLTIP";
	}

	// Token: 0x06004971 RID: 18801 RVA: 0x000D4161 File Offset: 0x000D2361
	string ISliderControl.GetSliderTooltip(int index)
	{
		return string.Format(Strings.Get("STRINGS.UI.UISIDESCREENS.LOGIC_FILTER_SIDE_SCREEN.TOOLTIP"), this.DelayAmount);
	}

	// Token: 0x06004972 RID: 18802 RVA: 0x000D4182 File Offset: 0x000D2382
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicGateFilter>(-905833192, LogicGateFilter.OnCopySettingsDelegate);
	}

	// Token: 0x06004973 RID: 18803 RVA: 0x00267DEC File Offset: 0x00265FEC
	private void OnCopySettings(object data)
	{
		LogicGateFilter component = ((GameObject)data).GetComponent<LogicGateFilter>();
		if (component != null)
		{
			this.DelayAmount = component.DelayAmount;
		}
	}

	// Token: 0x06004974 RID: 18804 RVA: 0x00267E1C File Offset: 0x0026601C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		this.meter = new MeterController(component, "meter_target", "meter", Meter.Offset.UserSpecified, Grid.SceneLayer.LogicGatesFront, Vector3.zero, null);
		this.meter.SetPositionPercent(0f);
	}

	// Token: 0x06004975 RID: 18805 RVA: 0x00267E68 File Offset: 0x00266068
	private void Update()
	{
		float positionPercent;
		if (this.input_was_previously_negative)
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

	// Token: 0x06004976 RID: 18806 RVA: 0x000D419B File Offset: 0x000D239B
	public override void LogicTick()
	{
		if (!this.input_was_previously_negative && this.delayTicksRemaining > 0)
		{
			this.delayTicksRemaining--;
			if (this.delayTicksRemaining <= 0)
			{
				this.OnDelay();
			}
		}
	}

	// Token: 0x06004977 RID: 18807 RVA: 0x00267EC0 File Offset: 0x002660C0
	protected override int GetCustomValue(int val1, int val2)
	{
		if (val1 == 0)
		{
			this.input_was_previously_negative = true;
			this.delayTicksRemaining = 0;
			this.meter.SetPositionPercent(1f);
		}
		else if (this.delayTicksRemaining <= 0)
		{
			if (this.input_was_previously_negative)
			{
				this.delayTicksRemaining = this.DelayAmountTicks;
			}
			this.input_was_previously_negative = false;
		}
		if (val1 != 0 && this.delayTicksRemaining <= 0)
		{
			return 1;
		}
		return 0;
	}

	// Token: 0x06004978 RID: 18808 RVA: 0x00267F24 File Offset: 0x00266124
	private void OnDelay()
	{
		if (this.cleaningUp)
		{
			return;
		}
		this.delayTicksRemaining = 0;
		this.meter.SetPositionPercent(0f);
		if (this.outputValueOne == 1)
		{
			return;
		}
		int outputCellOne = base.OutputCellOne;
		if (!(Game.Instance.logicCircuitSystem.GetNetworkForCell(outputCellOne) is LogicCircuitNetwork))
		{
			return;
		}
		this.outputValueOne = 1;
		base.RefreshAnimation();
	}

	// Token: 0x040033CD RID: 13261
	[Serialize]
	private bool input_was_previously_negative;

	// Token: 0x040033CE RID: 13262
	[Serialize]
	private float delayAmount = 5f;

	// Token: 0x040033CF RID: 13263
	[Serialize]
	private int delayTicksRemaining;

	// Token: 0x040033D0 RID: 13264
	private MeterController meter;

	// Token: 0x040033D1 RID: 13265
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x040033D2 RID: 13266
	private static readonly EventSystem.IntraObjectHandler<LogicGateFilter> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicGateFilter>(delegate(LogicGateFilter component, object data)
	{
		component.OnCopySettings(data);
	});
}
