using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000E8B RID: 3723
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicHEPSensor : Switch, ISaveLoadable, IThresholdSwitch, ISimEveryTick
{
	// Token: 0x0600497E RID: 18814 RVA: 0x000D420F File Offset: 0x000D240F
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicHEPSensor>(-905833192, LogicHEPSensor.OnCopySettingsDelegate);
	}

	// Token: 0x0600497F RID: 18815 RVA: 0x00267F88 File Offset: 0x00266188
	private void OnCopySettings(object data)
	{
		LogicHEPSensor component = ((GameObject)data).GetComponent<LogicHEPSensor>();
		if (component != null)
		{
			this.Threshold = component.Threshold;
			this.ActivateAboveThreshold = component.ActivateAboveThreshold;
		}
	}

	// Token: 0x06004980 RID: 18816 RVA: 0x00267FC4 File Offset: 0x002661C4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.OnToggle += this.OnSwitchToggled;
		this.UpdateLogicCircuit();
		this.UpdateVisualState(true);
		this.wasOn = this.switchedOn;
		LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
		logicCircuitManager.onLogicTick = (System.Action)Delegate.Combine(logicCircuitManager.onLogicTick, new System.Action(this.LogicTick));
	}

	// Token: 0x06004981 RID: 18817 RVA: 0x000D4228 File Offset: 0x000D2428
	protected override void OnCleanUp()
	{
		LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
		logicCircuitManager.onLogicTick = (System.Action)Delegate.Remove(logicCircuitManager.onLogicTick, new System.Action(this.LogicTick));
		base.OnCleanUp();
	}

	// Token: 0x06004982 RID: 18818 RVA: 0x00268030 File Offset: 0x00266230
	public void SimEveryTick(float dt)
	{
		if (this.waitForLogicTick)
		{
			return;
		}
		Vector2I vector2I = Grid.CellToXY(Grid.PosToCell(this));
		ListPool<ScenePartitionerEntry, LogicHEPSensor>.PooledList pooledList = ListPool<ScenePartitionerEntry, LogicHEPSensor>.Allocate();
		GameScenePartitioner.Instance.GatherEntries(vector2I.x, vector2I.y, 1, 1, GameScenePartitioner.Instance.collisionLayer, pooledList);
		float num = 0f;
		foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList)
		{
			HighEnergyParticle component = (scenePartitionerEntry.obj as KCollider2D).gameObject.GetComponent<HighEnergyParticle>();
			if (!(component == null) && component.isCollideable)
			{
				num += component.payload;
			}
		}
		pooledList.Recycle();
		this.foundPayload = num;
		bool flag = (this.activateOnHigherThan && num > this.thresholdPayload) || (!this.activateOnHigherThan && num < this.thresholdPayload);
		if (flag != this.switchedOn)
		{
			this.waitForLogicTick = true;
		}
		this.SetState(flag);
	}

	// Token: 0x06004983 RID: 18819 RVA: 0x000D425B File Offset: 0x000D245B
	private void LogicTick()
	{
		this.waitForLogicTick = false;
	}

	// Token: 0x06004984 RID: 18820 RVA: 0x000D4264 File Offset: 0x000D2464
	private void OnSwitchToggled(bool toggled_on)
	{
		this.UpdateLogicCircuit();
		this.UpdateVisualState(false);
	}

	// Token: 0x06004985 RID: 18821 RVA: 0x000CEE6A File Offset: 0x000CD06A
	private void UpdateLogicCircuit()
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);
	}

	// Token: 0x06004986 RID: 18822 RVA: 0x0026813C File Offset: 0x0026633C
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

	// Token: 0x06004987 RID: 18823 RVA: 0x0026473C File Offset: 0x0026293C
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x170003AE RID: 942
	// (get) Token: 0x06004988 RID: 18824 RVA: 0x000D4273 File Offset: 0x000D2473
	// (set) Token: 0x06004989 RID: 18825 RVA: 0x000D427B File Offset: 0x000D247B
	public float Threshold
	{
		get
		{
			return this.thresholdPayload;
		}
		set
		{
			this.thresholdPayload = value;
			this.dirty = true;
		}
	}

	// Token: 0x170003AF RID: 943
	// (get) Token: 0x0600498A RID: 18826 RVA: 0x000D428B File Offset: 0x000D248B
	// (set) Token: 0x0600498B RID: 18827 RVA: 0x000D4293 File Offset: 0x000D2493
	public bool ActivateAboveThreshold
	{
		get
		{
			return this.activateOnHigherThan;
		}
		set
		{
			this.activateOnHigherThan = value;
			this.dirty = true;
		}
	}

	// Token: 0x170003B0 RID: 944
	// (get) Token: 0x0600498C RID: 18828 RVA: 0x000D42A3 File Offset: 0x000D24A3
	public float CurrentValue
	{
		get
		{
			return this.foundPayload;
		}
	}

	// Token: 0x170003B1 RID: 945
	// (get) Token: 0x0600498D RID: 18829 RVA: 0x000D42AB File Offset: 0x000D24AB
	public float RangeMin
	{
		get
		{
			return this.minPayload;
		}
	}

	// Token: 0x170003B2 RID: 946
	// (get) Token: 0x0600498E RID: 18830 RVA: 0x000D42B3 File Offset: 0x000D24B3
	public float RangeMax
	{
		get
		{
			return this.maxPayload;
		}
	}

	// Token: 0x0600498F RID: 18831 RVA: 0x000D42AB File Offset: 0x000D24AB
	public float GetRangeMinInputField()
	{
		return this.minPayload;
	}

	// Token: 0x06004990 RID: 18832 RVA: 0x000D42B3 File Offset: 0x000D24B3
	public float GetRangeMaxInputField()
	{
		return this.maxPayload;
	}

	// Token: 0x170003B3 RID: 947
	// (get) Token: 0x06004991 RID: 18833 RVA: 0x000D42BB File Offset: 0x000D24BB
	public LocString Title
	{
		get
		{
			return UI.UISIDESCREENS.HEPSWITCHSIDESCREEN.TITLE;
		}
	}

	// Token: 0x170003B4 RID: 948
	// (get) Token: 0x06004992 RID: 18834 RVA: 0x000D42C2 File Offset: 0x000D24C2
	public LocString ThresholdValueName
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.HEPS;
		}
	}

	// Token: 0x170003B5 RID: 949
	// (get) Token: 0x06004993 RID: 18835 RVA: 0x000D42C9 File Offset: 0x000D24C9
	public string AboveToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.HEPS_TOOLTIP_ABOVE;
		}
	}

	// Token: 0x170003B6 RID: 950
	// (get) Token: 0x06004994 RID: 18836 RVA: 0x000D42D5 File Offset: 0x000D24D5
	public string BelowToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.HEPS_TOOLTIP_BELOW;
		}
	}

	// Token: 0x06004995 RID: 18837 RVA: 0x000D42E1 File Offset: 0x000D24E1
	public string Format(float value, bool units)
	{
		return GameUtil.GetFormattedHighEnergyParticles(value, GameUtil.TimeSlice.None, units);
	}

	// Token: 0x06004996 RID: 18838 RVA: 0x000CEFDB File Offset: 0x000CD1DB
	public float ProcessedSliderValue(float input)
	{
		return Mathf.Round(input);
	}

	// Token: 0x06004997 RID: 18839 RVA: 0x000B64D6 File Offset: 0x000B46D6
	public float ProcessedInputValue(float input)
	{
		return input;
	}

	// Token: 0x06004998 RID: 18840 RVA: 0x000D42EB File Offset: 0x000D24EB
	public LocString ThresholdValueUnits()
	{
		return UI.UNITSUFFIXES.HIGHENERGYPARTICLES.PARTRICLES;
	}

	// Token: 0x170003B7 RID: 951
	// (get) Token: 0x06004999 RID: 18841 RVA: 0x000B1628 File Offset: 0x000AF828
	public ThresholdScreenLayoutType LayoutType
	{
		get
		{
			return ThresholdScreenLayoutType.SliderBar;
		}
	}

	// Token: 0x170003B8 RID: 952
	// (get) Token: 0x0600499A RID: 18842 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public int IncrementScale
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x170003B9 RID: 953
	// (get) Token: 0x0600499B RID: 18843 RVA: 0x002681C4 File Offset: 0x002663C4
	public NonLinearSlider.Range[] GetRanges
	{
		get
		{
			return new NonLinearSlider.Range[]
			{
				new NonLinearSlider.Range(30f, 50f),
				new NonLinearSlider.Range(30f, 200f),
				new NonLinearSlider.Range(40f, 500f)
			};
		}
	}

	// Token: 0x040033D4 RID: 13268
	[Serialize]
	public float thresholdPayload;

	// Token: 0x040033D5 RID: 13269
	[Serialize]
	public bool activateOnHigherThan;

	// Token: 0x040033D6 RID: 13270
	[Serialize]
	public bool dirty = true;

	// Token: 0x040033D7 RID: 13271
	private readonly float minPayload;

	// Token: 0x040033D8 RID: 13272
	private readonly float maxPayload = 500f;

	// Token: 0x040033D9 RID: 13273
	private float foundPayload;

	// Token: 0x040033DA RID: 13274
	private bool waitForLogicTick;

	// Token: 0x040033DB RID: 13275
	private bool wasOn;

	// Token: 0x040033DC RID: 13276
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x040033DD RID: 13277
	private static readonly EventSystem.IntraObjectHandler<LogicHEPSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicHEPSensor>(delegate(LogicHEPSensor component, object data)
	{
		component.OnCopySettings(data);
	});
}
