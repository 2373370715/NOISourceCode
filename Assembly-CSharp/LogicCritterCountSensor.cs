using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000E75 RID: 3701
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicCritterCountSensor : Switch, ISaveLoadable, IThresholdSwitch, ISim200ms
{
	// Token: 0x0600489C RID: 18588 RVA: 0x000D390B File Offset: 0x000D1B0B
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.selectable = base.GetComponent<KSelectable>();
		base.Subscribe<LogicCritterCountSensor>(-905833192, LogicCritterCountSensor.OnCopySettingsDelegate);
	}

	// Token: 0x0600489D RID: 18589 RVA: 0x00264BC8 File Offset: 0x00262DC8
	private void OnCopySettings(object data)
	{
		LogicCritterCountSensor component = ((GameObject)data).GetComponent<LogicCritterCountSensor>();
		if (component != null)
		{
			this.countThreshold = component.countThreshold;
			this.activateOnGreaterThan = component.activateOnGreaterThan;
			this.countCritters = component.countCritters;
			this.countEggs = component.countEggs;
		}
	}

	// Token: 0x0600489E RID: 18590 RVA: 0x000D3930 File Offset: 0x000D1B30
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.OnToggle += this.OnSwitchToggled;
		this.UpdateLogicCircuit();
		this.UpdateVisualState(true);
		this.wasOn = this.switchedOn;
	}

	// Token: 0x0600489F RID: 18591 RVA: 0x00264C1C File Offset: 0x00262E1C
	public void Sim200ms(float dt)
	{
		Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(base.gameObject);
		if (roomOfGameObject != null)
		{
			this.currentCount = 0;
			if (this.countCritters)
			{
				this.currentCount += roomOfGameObject.cavity.creatures.Count;
			}
			if (this.countEggs)
			{
				this.currentCount += roomOfGameObject.cavity.eggs.Count;
			}
			bool state = this.activateOnGreaterThan ? (this.currentCount > this.countThreshold) : (this.currentCount < this.countThreshold);
			this.SetState(state);
			if (this.selectable.HasStatusItem(Db.Get().BuildingStatusItems.NotInAnyRoom))
			{
				this.selectable.RemoveStatusItem(this.roomStatusGUID, false);
				return;
			}
		}
		else
		{
			if (!this.selectable.HasStatusItem(Db.Get().BuildingStatusItems.NotInAnyRoom))
			{
				this.roomStatusGUID = this.selectable.AddStatusItem(Db.Get().BuildingStatusItems.NotInAnyRoom, null);
			}
			this.SetState(false);
		}
	}

	// Token: 0x060048A0 RID: 18592 RVA: 0x000D3963 File Offset: 0x000D1B63
	private void OnSwitchToggled(bool toggled_on)
	{
		this.UpdateLogicCircuit();
		this.UpdateVisualState(false);
	}

	// Token: 0x060048A1 RID: 18593 RVA: 0x000CEE6A File Offset: 0x000CD06A
	private void UpdateLogicCircuit()
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);
	}

	// Token: 0x060048A2 RID: 18594 RVA: 0x00264D38 File Offset: 0x00262F38
	private void UpdateVisualState(bool force = false)
	{
		if (this.wasOn != this.switchedOn || force)
		{
			this.wasOn = this.switchedOn;
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			component.Play(this.switchedOn ? "on_pre" : "on_pst", KAnim.PlayMode.Once, 1f, 0f);
			if (this.switchedOn)
			{
				component.Queue("on", KAnim.PlayMode.Loop, 1f, 0f);
				return;
			}
			component.Queue("off", KAnim.PlayMode.Once, 1f, 0f);
		}
	}

	// Token: 0x060048A3 RID: 18595 RVA: 0x0026473C File Offset: 0x0026293C
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x17000380 RID: 896
	// (get) Token: 0x060048A4 RID: 18596 RVA: 0x000D3972 File Offset: 0x000D1B72
	// (set) Token: 0x060048A5 RID: 18597 RVA: 0x000D397B File Offset: 0x000D1B7B
	public float Threshold
	{
		get
		{
			return (float)this.countThreshold;
		}
		set
		{
			this.countThreshold = (int)value;
		}
	}

	// Token: 0x17000381 RID: 897
	// (get) Token: 0x060048A6 RID: 18598 RVA: 0x000D3985 File Offset: 0x000D1B85
	// (set) Token: 0x060048A7 RID: 18599 RVA: 0x000D398D File Offset: 0x000D1B8D
	public bool ActivateAboveThreshold
	{
		get
		{
			return this.activateOnGreaterThan;
		}
		set
		{
			this.activateOnGreaterThan = value;
		}
	}

	// Token: 0x17000382 RID: 898
	// (get) Token: 0x060048A8 RID: 18600 RVA: 0x000D3996 File Offset: 0x000D1B96
	public float CurrentValue
	{
		get
		{
			return (float)this.currentCount;
		}
	}

	// Token: 0x17000383 RID: 899
	// (get) Token: 0x060048A9 RID: 18601 RVA: 0x000C18F8 File Offset: 0x000BFAF8
	public float RangeMin
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000384 RID: 900
	// (get) Token: 0x060048AA RID: 18602 RVA: 0x000D399F File Offset: 0x000D1B9F
	public float RangeMax
	{
		get
		{
			return 64f;
		}
	}

	// Token: 0x060048AB RID: 18603 RVA: 0x000D39A6 File Offset: 0x000D1BA6
	public float GetRangeMinInputField()
	{
		return this.RangeMin;
	}

	// Token: 0x060048AC RID: 18604 RVA: 0x000D39AE File Offset: 0x000D1BAE
	public float GetRangeMaxInputField()
	{
		return this.RangeMax;
	}

	// Token: 0x17000385 RID: 901
	// (get) Token: 0x060048AD RID: 18605 RVA: 0x000D39B6 File Offset: 0x000D1BB6
	public LocString Title
	{
		get
		{
			return UI.UISIDESCREENS.CRITTER_COUNT_SIDE_SCREEN.TITLE;
		}
	}

	// Token: 0x17000386 RID: 902
	// (get) Token: 0x060048AE RID: 18606 RVA: 0x000D39BD File Offset: 0x000D1BBD
	public LocString ThresholdValueName
	{
		get
		{
			return UI.UISIDESCREENS.CRITTER_COUNT_SIDE_SCREEN.VALUE_NAME;
		}
	}

	// Token: 0x17000387 RID: 903
	// (get) Token: 0x060048AF RID: 18607 RVA: 0x000D39C4 File Offset: 0x000D1BC4
	public string AboveToolTip
	{
		get
		{
			return UI.UISIDESCREENS.CRITTER_COUNT_SIDE_SCREEN.TOOLTIP_ABOVE;
		}
	}

	// Token: 0x17000388 RID: 904
	// (get) Token: 0x060048B0 RID: 18608 RVA: 0x000D39D0 File Offset: 0x000D1BD0
	public string BelowToolTip
	{
		get
		{
			return UI.UISIDESCREENS.CRITTER_COUNT_SIDE_SCREEN.TOOLTIP_BELOW;
		}
	}

	// Token: 0x060048B1 RID: 18609 RVA: 0x000C6C93 File Offset: 0x000C4E93
	public string Format(float value, bool units)
	{
		return value.ToString();
	}

	// Token: 0x060048B2 RID: 18610 RVA: 0x000CEFDB File Offset: 0x000CD1DB
	public float ProcessedSliderValue(float input)
	{
		return Mathf.Round(input);
	}

	// Token: 0x060048B3 RID: 18611 RVA: 0x000CEFDB File Offset: 0x000CD1DB
	public float ProcessedInputValue(float input)
	{
		return Mathf.Round(input);
	}

	// Token: 0x060048B4 RID: 18612 RVA: 0x000D39DC File Offset: 0x000D1BDC
	public LocString ThresholdValueUnits()
	{
		return "";
	}

	// Token: 0x17000389 RID: 905
	// (get) Token: 0x060048B5 RID: 18613 RVA: 0x000B1628 File Offset: 0x000AF828
	public ThresholdScreenLayoutType LayoutType
	{
		get
		{
			return ThresholdScreenLayoutType.SliderBar;
		}
	}

	// Token: 0x1700038A RID: 906
	// (get) Token: 0x060048B6 RID: 18614 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public int IncrementScale
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x1700038B RID: 907
	// (get) Token: 0x060048B7 RID: 18615 RVA: 0x000D39E8 File Offset: 0x000D1BE8
	public NonLinearSlider.Range[] GetRanges
	{
		get
		{
			return NonLinearSlider.GetDefaultRange(this.RangeMax);
		}
	}

	// Token: 0x04003300 RID: 13056
	private bool wasOn;

	// Token: 0x04003301 RID: 13057
	[Serialize]
	public bool countEggs = true;

	// Token: 0x04003302 RID: 13058
	[Serialize]
	public bool countCritters = true;

	// Token: 0x04003303 RID: 13059
	[Serialize]
	public int countThreshold;

	// Token: 0x04003304 RID: 13060
	[Serialize]
	public bool activateOnGreaterThan = true;

	// Token: 0x04003305 RID: 13061
	[Serialize]
	public int currentCount;

	// Token: 0x04003306 RID: 13062
	private KSelectable selectable;

	// Token: 0x04003307 RID: 13063
	private Guid roomStatusGUID;

	// Token: 0x04003308 RID: 13064
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04003309 RID: 13065
	private static readonly EventSystem.IntraObjectHandler<LogicCritterCountSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicCritterCountSensor>(delegate(LogicCritterCountSensor component, object data)
	{
		component.OnCopySettings(data);
	});
}
