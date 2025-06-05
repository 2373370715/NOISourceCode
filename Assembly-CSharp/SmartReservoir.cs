using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020018D8 RID: 6360
[AddComponentMenu("KMonoBehaviour/scripts/SmartReservoir")]
public class SmartReservoir : KMonoBehaviour, IActivationRangeTarget, ISim200ms
{
	// Token: 0x1700085B RID: 2139
	// (get) Token: 0x06008388 RID: 33672 RVA: 0x000FB006 File Offset: 0x000F9206
	public float PercentFull
	{
		get
		{
			return this.storage.MassStored() / this.storage.Capacity();
		}
	}

	// Token: 0x06008389 RID: 33673 RVA: 0x000FB01F File Offset: 0x000F921F
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<SmartReservoir>(-801688580, SmartReservoir.OnLogicValueChangedDelegate);
		base.Subscribe<SmartReservoir>(-592767678, SmartReservoir.UpdateLogicCircuitDelegate);
	}

	// Token: 0x0600838A RID: 33674 RVA: 0x000FB049 File Offset: 0x000F9249
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<SmartReservoir>(-905833192, SmartReservoir.OnCopySettingsDelegate);
	}

	// Token: 0x0600838B RID: 33675 RVA: 0x000FB062 File Offset: 0x000F9262
	public void Sim200ms(float dt)
	{
		this.UpdateLogicCircuit(null);
	}

	// Token: 0x0600838C RID: 33676 RVA: 0x0034F6DC File Offset: 0x0034D8DC
	private void UpdateLogicCircuit(object data)
	{
		float num = this.PercentFull * 100f;
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
		bool flag = this.activated;
		this.logicPorts.SendSignal(SmartReservoir.PORT_ID, flag ? 1 : 0);
	}

	// Token: 0x0600838D RID: 33677 RVA: 0x0034F740 File Offset: 0x0034D940
	private void OnLogicValueChanged(object data)
	{
		LogicValueChanged logicValueChanged = (LogicValueChanged)data;
		if (logicValueChanged.portID == SmartReservoir.PORT_ID)
		{
			this.SetLogicMeter(LogicCircuitNetwork.IsBitActive(0, logicValueChanged.newValue));
		}
	}

	// Token: 0x0600838E RID: 33678 RVA: 0x0034F778 File Offset: 0x0034D978
	private void OnCopySettings(object data)
	{
		SmartReservoir component = ((GameObject)data).GetComponent<SmartReservoir>();
		if (component != null)
		{
			this.ActivateValue = component.ActivateValue;
			this.DeactivateValue = component.DeactivateValue;
		}
	}

	// Token: 0x0600838F RID: 33679 RVA: 0x000FB06B File Offset: 0x000F926B
	public void SetLogicMeter(bool on)
	{
		if (this.logicMeter != null)
		{
			this.logicMeter.SetPositionPercent(on ? 1f : 0f);
		}
	}

	// Token: 0x1700085C RID: 2140
	// (get) Token: 0x06008390 RID: 33680 RVA: 0x000FB08F File Offset: 0x000F928F
	// (set) Token: 0x06008391 RID: 33681 RVA: 0x000FB098 File Offset: 0x000F9298
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

	// Token: 0x1700085D RID: 2141
	// (get) Token: 0x06008392 RID: 33682 RVA: 0x000FB0A9 File Offset: 0x000F92A9
	// (set) Token: 0x06008393 RID: 33683 RVA: 0x000FB0B2 File Offset: 0x000F92B2
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

	// Token: 0x1700085E RID: 2142
	// (get) Token: 0x06008394 RID: 33684 RVA: 0x000C18F8 File Offset: 0x000BFAF8
	public float MinValue
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700085F RID: 2143
	// (get) Token: 0x06008395 RID: 33685 RVA: 0x000CD7B4 File Offset: 0x000CB9B4
	public float MaxValue
	{
		get
		{
			return 100f;
		}
	}

	// Token: 0x17000860 RID: 2144
	// (get) Token: 0x06008396 RID: 33686 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public bool UseWholeNumbers
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000861 RID: 2145
	// (get) Token: 0x06008397 RID: 33687 RVA: 0x000FB0C3 File Offset: 0x000F92C3
	public string ActivateTooltip
	{
		get
		{
			return BUILDINGS.PREFABS.SMARTRESERVOIR.DEACTIVATE_TOOLTIP;
		}
	}

	// Token: 0x17000862 RID: 2146
	// (get) Token: 0x06008398 RID: 33688 RVA: 0x000FB0CF File Offset: 0x000F92CF
	public string DeactivateTooltip
	{
		get
		{
			return BUILDINGS.PREFABS.SMARTRESERVOIR.ACTIVATE_TOOLTIP;
		}
	}

	// Token: 0x17000863 RID: 2147
	// (get) Token: 0x06008399 RID: 33689 RVA: 0x000FB0DB File Offset: 0x000F92DB
	public string ActivationRangeTitleText
	{
		get
		{
			return BUILDINGS.PREFABS.SMARTRESERVOIR.SIDESCREEN_TITLE;
		}
	}

	// Token: 0x17000864 RID: 2148
	// (get) Token: 0x0600839A RID: 33690 RVA: 0x000FB0E7 File Offset: 0x000F92E7
	public string ActivateSliderLabelText
	{
		get
		{
			return BUILDINGS.PREFABS.SMARTRESERVOIR.SIDESCREEN_DEACTIVATE;
		}
	}

	// Token: 0x17000865 RID: 2149
	// (get) Token: 0x0600839B RID: 33691 RVA: 0x000FB0F3 File Offset: 0x000F92F3
	public string DeactivateSliderLabelText
	{
		get
		{
			return BUILDINGS.PREFABS.SMARTRESERVOIR.SIDESCREEN_ACTIVATE;
		}
	}

	// Token: 0x04006430 RID: 25648
	[MyCmpGet]
	private Storage storage;

	// Token: 0x04006431 RID: 25649
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04006432 RID: 25650
	[Serialize]
	private int activateValue;

	// Token: 0x04006433 RID: 25651
	[Serialize]
	private int deactivateValue = 100;

	// Token: 0x04006434 RID: 25652
	[Serialize]
	private bool activated;

	// Token: 0x04006435 RID: 25653
	[MyCmpGet]
	private LogicPorts logicPorts;

	// Token: 0x04006436 RID: 25654
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04006437 RID: 25655
	private MeterController logicMeter;

	// Token: 0x04006438 RID: 25656
	public static readonly HashedString PORT_ID = "SmartReservoirLogicPort";

	// Token: 0x04006439 RID: 25657
	private static readonly EventSystem.IntraObjectHandler<SmartReservoir> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<SmartReservoir>(delegate(SmartReservoir component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x0400643A RID: 25658
	private static readonly EventSystem.IntraObjectHandler<SmartReservoir> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<SmartReservoir>(delegate(SmartReservoir component, object data)
	{
		component.OnLogicValueChanged(data);
	});

	// Token: 0x0400643B RID: 25659
	private static readonly EventSystem.IntraObjectHandler<SmartReservoir> UpdateLogicCircuitDelegate = new EventSystem.IntraObjectHandler<SmartReservoir>(delegate(SmartReservoir component, object data)
	{
		component.UpdateLogicCircuit(data);
	});
}
