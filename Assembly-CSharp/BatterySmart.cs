using System;
using System.Diagnostics;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000CDE RID: 3294
[SerializationConfig(MemberSerialization.OptIn)]
[DebuggerDisplay("{name}")]
public class BatterySmart : Battery, IActivationRangeTarget
{
	// Token: 0x06003F07 RID: 16135 RVA: 0x000CD6DE File Offset: 0x000CB8DE
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<BatterySmart>(-905833192, BatterySmart.OnCopySettingsDelegate);
	}

	// Token: 0x06003F08 RID: 16136 RVA: 0x002446A8 File Offset: 0x002428A8
	private void OnCopySettings(object data)
	{
		BatterySmart component = ((GameObject)data).GetComponent<BatterySmart>();
		if (component != null)
		{
			this.ActivateValue = component.ActivateValue;
			this.DeactivateValue = component.DeactivateValue;
		}
	}

	// Token: 0x06003F09 RID: 16137 RVA: 0x000CD6F7 File Offset: 0x000CB8F7
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.CreateLogicMeter();
		base.Subscribe<BatterySmart>(-801688580, BatterySmart.OnLogicValueChangedDelegate);
		base.Subscribe<BatterySmart>(-592767678, BatterySmart.UpdateLogicCircuitDelegate);
	}

	// Token: 0x06003F0A RID: 16138 RVA: 0x000CD727 File Offset: 0x000CB927
	private void CreateLogicMeter()
	{
		this.logicMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "logicmeter_target", "logicmeter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
	}

	// Token: 0x06003F0B RID: 16139 RVA: 0x000CD74C File Offset: 0x000CB94C
	public override void EnergySim200ms(float dt)
	{
		base.EnergySim200ms(dt);
		this.UpdateLogicCircuit(null);
	}

	// Token: 0x06003F0C RID: 16140 RVA: 0x002446E4 File Offset: 0x002428E4
	private void UpdateLogicCircuit(object data)
	{
		float num = (float)Mathf.RoundToInt(base.PercentFull * 100f);
		if (this.activated)
		{
			if (num >= (float)this.deactivateValue)
			{
				this.activated = false;
			}
		}
		else if (num <= (float)this.activateValue)
		{
			this.activated = true;
		}
		bool isOperational = this.operational.IsOperational;
		bool flag = this.activated && isOperational;
		this.logicPorts.SendSignal(BatterySmart.PORT_ID, flag ? 1 : 0);
	}

	// Token: 0x06003F0D RID: 16141 RVA: 0x0024475C File Offset: 0x0024295C
	private void OnLogicValueChanged(object data)
	{
		LogicValueChanged logicValueChanged = (LogicValueChanged)data;
		if (logicValueChanged.portID == BatterySmart.PORT_ID)
		{
			this.SetLogicMeter(LogicCircuitNetwork.IsBitActive(0, logicValueChanged.newValue));
		}
	}

	// Token: 0x06003F0E RID: 16142 RVA: 0x000CD75C File Offset: 0x000CB95C
	public void SetLogicMeter(bool on)
	{
		if (this.logicMeter != null)
		{
			this.logicMeter.SetPositionPercent(on ? 1f : 0f);
		}
	}

	// Token: 0x170002F2 RID: 754
	// (get) Token: 0x06003F0F RID: 16143 RVA: 0x000CD780 File Offset: 0x000CB980
	// (set) Token: 0x06003F10 RID: 16144 RVA: 0x000CD789 File Offset: 0x000CB989
	public float ActivateValue
	{
		get
		{
			return (float)this.deactivateValue;
		}
		set
		{
			this.deactivateValue = (int)value;
			this.UpdateLogicCircuit(null);
		}
	}

	// Token: 0x170002F3 RID: 755
	// (get) Token: 0x06003F11 RID: 16145 RVA: 0x000CD79A File Offset: 0x000CB99A
	// (set) Token: 0x06003F12 RID: 16146 RVA: 0x000CD7A3 File Offset: 0x000CB9A3
	public float DeactivateValue
	{
		get
		{
			return (float)this.activateValue;
		}
		set
		{
			this.activateValue = (int)value;
			this.UpdateLogicCircuit(null);
		}
	}

	// Token: 0x170002F4 RID: 756
	// (get) Token: 0x06003F13 RID: 16147 RVA: 0x000C18F8 File Offset: 0x000BFAF8
	public float MinValue
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170002F5 RID: 757
	// (get) Token: 0x06003F14 RID: 16148 RVA: 0x000CD7B4 File Offset: 0x000CB9B4
	public float MaxValue
	{
		get
		{
			return 100f;
		}
	}

	// Token: 0x170002F6 RID: 758
	// (get) Token: 0x06003F15 RID: 16149 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public bool UseWholeNumbers
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170002F7 RID: 759
	// (get) Token: 0x06003F16 RID: 16150 RVA: 0x000CD7BB File Offset: 0x000CB9BB
	public string ActivateTooltip
	{
		get
		{
			return BUILDINGS.PREFABS.BATTERYSMART.DEACTIVATE_TOOLTIP;
		}
	}

	// Token: 0x170002F8 RID: 760
	// (get) Token: 0x06003F17 RID: 16151 RVA: 0x000CD7C7 File Offset: 0x000CB9C7
	public string DeactivateTooltip
	{
		get
		{
			return BUILDINGS.PREFABS.BATTERYSMART.ACTIVATE_TOOLTIP;
		}
	}

	// Token: 0x170002F9 RID: 761
	// (get) Token: 0x06003F18 RID: 16152 RVA: 0x000CD7D3 File Offset: 0x000CB9D3
	public string ActivationRangeTitleText
	{
		get
		{
			return BUILDINGS.PREFABS.BATTERYSMART.SIDESCREEN_TITLE;
		}
	}

	// Token: 0x170002FA RID: 762
	// (get) Token: 0x06003F19 RID: 16153 RVA: 0x000CD7DF File Offset: 0x000CB9DF
	public string ActivateSliderLabelText
	{
		get
		{
			return BUILDINGS.PREFABS.BATTERYSMART.SIDESCREEN_DEACTIVATE;
		}
	}

	// Token: 0x170002FB RID: 763
	// (get) Token: 0x06003F1A RID: 16154 RVA: 0x000CD7EB File Offset: 0x000CB9EB
	public string DeactivateSliderLabelText
	{
		get
		{
			return BUILDINGS.PREFABS.BATTERYSMART.SIDESCREEN_ACTIVATE;
		}
	}

	// Token: 0x04002BA0 RID: 11168
	public static readonly HashedString PORT_ID = "BatterySmartLogicPort";

	// Token: 0x04002BA1 RID: 11169
	[Serialize]
	private int activateValue;

	// Token: 0x04002BA2 RID: 11170
	[Serialize]
	private int deactivateValue = 100;

	// Token: 0x04002BA3 RID: 11171
	[Serialize]
	private bool activated;

	// Token: 0x04002BA4 RID: 11172
	[MyCmpGet]
	private LogicPorts logicPorts;

	// Token: 0x04002BA5 RID: 11173
	private MeterController logicMeter;

	// Token: 0x04002BA6 RID: 11174
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04002BA7 RID: 11175
	private static readonly EventSystem.IntraObjectHandler<BatterySmart> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<BatterySmart>(delegate(BatterySmart component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x04002BA8 RID: 11176
	private static readonly EventSystem.IntraObjectHandler<BatterySmart> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<BatterySmart>(delegate(BatterySmart component, object data)
	{
		component.OnLogicValueChanged(data);
	});

	// Token: 0x04002BA9 RID: 11177
	private static readonly EventSystem.IntraObjectHandler<BatterySmart> UpdateLogicCircuitDelegate = new EventSystem.IntraObjectHandler<BatterySmart>(delegate(BatterySmart component, object data)
	{
		component.UpdateLogicCircuit(data);
	});
}
