using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000E91 RID: 3729
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicMassSensor : Switch, ISaveLoadable, IThresholdSwitch
{
	// Token: 0x060049CE RID: 18894 RVA: 0x000D44F9 File Offset: 0x000D26F9
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicMassSensor>(-905833192, LogicMassSensor.OnCopySettingsDelegate);
	}

	// Token: 0x060049CF RID: 18895 RVA: 0x002688D4 File Offset: 0x00266AD4
	private void OnCopySettings(object data)
	{
		LogicMassSensor component = ((GameObject)data).GetComponent<LogicMassSensor>();
		if (component != null)
		{
			this.Threshold = component.Threshold;
			this.ActivateAboveThreshold = component.ActivateAboveThreshold;
		}
	}

	// Token: 0x060049D0 RID: 18896 RVA: 0x00268910 File Offset: 0x00266B10
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.UpdateVisualState(true);
		int cell = Grid.CellAbove(this.NaturalBuildingCell());
		this.solidChangedEntry = GameScenePartitioner.Instance.Add("LogicMassSensor.SolidChanged", base.gameObject, cell, GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnSolidChanged));
		this.pickupablesChangedEntry = GameScenePartitioner.Instance.Add("LogicMassSensor.PickupablesChanged", base.gameObject, cell, GameScenePartitioner.Instance.pickupablesChangedLayer, new Action<object>(this.OnPickupablesChanged));
		this.floorSwitchActivatorChangedEntry = GameScenePartitioner.Instance.Add("LogicMassSensor.SwitchActivatorChanged", base.gameObject, cell, GameScenePartitioner.Instance.floorSwitchActivatorChangedLayer, new Action<object>(this.OnActivatorsChanged));
		base.OnToggle += this.SwitchToggled;
	}

	// Token: 0x060049D1 RID: 18897 RVA: 0x000D4512 File Offset: 0x000D2712
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.solidChangedEntry);
		GameScenePartitioner.Instance.Free(ref this.pickupablesChangedEntry);
		GameScenePartitioner.Instance.Free(ref this.floorSwitchActivatorChangedEntry);
		base.OnCleanUp();
	}

	// Token: 0x060049D2 RID: 18898 RVA: 0x002689E0 File Offset: 0x00266BE0
	private void Update()
	{
		this.toggleCooldown = Mathf.Max(0f, this.toggleCooldown - Time.deltaTime);
		if (this.toggleCooldown == 0f)
		{
			float currentValue = this.CurrentValue;
			if ((this.activateAboveThreshold ? (currentValue > this.threshold) : (currentValue < this.threshold)) != base.IsSwitchedOn)
			{
				this.Toggle();
				this.toggleCooldown = 0.15f;
			}
			this.UpdateVisualState(false);
		}
	}

	// Token: 0x060049D3 RID: 18899 RVA: 0x00268A5C File Offset: 0x00266C5C
	private void OnSolidChanged(object data)
	{
		int i = Grid.CellAbove(this.NaturalBuildingCell());
		if (Grid.Solid[i])
		{
			this.massSolid = Grid.Mass[i];
			return;
		}
		this.massSolid = 0f;
	}

	// Token: 0x060049D4 RID: 18900 RVA: 0x00268AA0 File Offset: 0x00266CA0
	private void OnPickupablesChanged(object data)
	{
		float num = 0f;
		int cell = Grid.CellAbove(this.NaturalBuildingCell());
		ListPool<ScenePartitionerEntry, LogicMassSensor>.PooledList pooledList = ListPool<ScenePartitionerEntry, LogicMassSensor>.Allocate();
		GameScenePartitioner.Instance.GatherEntries(Grid.CellToXY(cell).x, Grid.CellToXY(cell).y, 1, 1, GameScenePartitioner.Instance.pickupablesLayer, pooledList);
		for (int i = 0; i < pooledList.Count; i++)
		{
			Pickupable pickupable = pooledList[i].obj as Pickupable;
			if (!(pickupable == null) && !pickupable.wasAbsorbed)
			{
				KPrefabID kprefabID = pickupable.KPrefabID;
				if (!kprefabID.HasTag(GameTags.Creature) || (kprefabID.HasTag(GameTags.Creatures.Walker) || kprefabID.HasTag(GameTags.Creatures.Hoverer) || kprefabID.HasTag(GameTags.Creatures.Flopping)))
				{
					num += pickupable.PrimaryElement.Mass;
				}
			}
		}
		pooledList.Recycle();
		this.massPickupables = num;
	}

	// Token: 0x060049D5 RID: 18901 RVA: 0x00268B8C File Offset: 0x00266D8C
	private void OnActivatorsChanged(object data)
	{
		float num = 0f;
		int cell = Grid.CellAbove(this.NaturalBuildingCell());
		ListPool<ScenePartitionerEntry, LogicMassSensor>.PooledList pooledList = ListPool<ScenePartitionerEntry, LogicMassSensor>.Allocate();
		GameScenePartitioner.Instance.GatherEntries(Grid.CellToXY(cell).x, Grid.CellToXY(cell).y, 1, 1, GameScenePartitioner.Instance.floorSwitchActivatorLayer, pooledList);
		for (int i = 0; i < pooledList.Count; i++)
		{
			FloorSwitchActivator floorSwitchActivator = pooledList[i].obj as FloorSwitchActivator;
			if (!(floorSwitchActivator == null))
			{
				num += floorSwitchActivator.PrimaryElement.Mass;
			}
		}
		pooledList.Recycle();
		this.massActivators = num;
	}

	// Token: 0x170003C6 RID: 966
	// (get) Token: 0x060049D6 RID: 18902 RVA: 0x000D454A File Offset: 0x000D274A
	public LocString Title
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.TITLE;
		}
	}

	// Token: 0x170003C7 RID: 967
	// (get) Token: 0x060049D7 RID: 18903 RVA: 0x000D4551 File Offset: 0x000D2751
	// (set) Token: 0x060049D8 RID: 18904 RVA: 0x000D4559 File Offset: 0x000D2759
	public float Threshold
	{
		get
		{
			return this.threshold;
		}
		set
		{
			this.threshold = value;
		}
	}

	// Token: 0x170003C8 RID: 968
	// (get) Token: 0x060049D9 RID: 18905 RVA: 0x000D4562 File Offset: 0x000D2762
	// (set) Token: 0x060049DA RID: 18906 RVA: 0x000D456A File Offset: 0x000D276A
	public bool ActivateAboveThreshold
	{
		get
		{
			return this.activateAboveThreshold;
		}
		set
		{
			this.activateAboveThreshold = value;
		}
	}

	// Token: 0x170003C9 RID: 969
	// (get) Token: 0x060049DB RID: 18907 RVA: 0x000D4573 File Offset: 0x000D2773
	public float CurrentValue
	{
		get
		{
			return this.massSolid + this.massPickupables + this.massActivators;
		}
	}

	// Token: 0x170003CA RID: 970
	// (get) Token: 0x060049DC RID: 18908 RVA: 0x000D4589 File Offset: 0x000D2789
	public float RangeMin
	{
		get
		{
			return this.rangeMin;
		}
	}

	// Token: 0x170003CB RID: 971
	// (get) Token: 0x060049DD RID: 18909 RVA: 0x000D4591 File Offset: 0x000D2791
	public float RangeMax
	{
		get
		{
			return this.rangeMax;
		}
	}

	// Token: 0x060049DE RID: 18910 RVA: 0x000D4589 File Offset: 0x000D2789
	public float GetRangeMinInputField()
	{
		return this.rangeMin;
	}

	// Token: 0x060049DF RID: 18911 RVA: 0x000D4591 File Offset: 0x000D2791
	public float GetRangeMaxInputField()
	{
		return this.rangeMax;
	}

	// Token: 0x170003CC RID: 972
	// (get) Token: 0x060049E0 RID: 18912 RVA: 0x000D4599 File Offset: 0x000D2799
	public LocString ThresholdValueName
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.PRESSURE;
		}
	}

	// Token: 0x170003CD RID: 973
	// (get) Token: 0x060049E1 RID: 18913 RVA: 0x000D45A0 File Offset: 0x000D27A0
	public string AboveToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.PRESSURE_TOOLTIP_ABOVE;
		}
	}

	// Token: 0x170003CE RID: 974
	// (get) Token: 0x060049E2 RID: 18914 RVA: 0x000D45AC File Offset: 0x000D27AC
	public string BelowToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.PRESSURE_TOOLTIP_BELOW;
		}
	}

	// Token: 0x060049E3 RID: 18915 RVA: 0x00268C28 File Offset: 0x00266E28
	public string Format(float value, bool units)
	{
		GameUtil.MetricMassFormat massFormat = GameUtil.MetricMassFormat.Kilogram;
		return GameUtil.GetFormattedMass(value, GameUtil.TimeSlice.None, massFormat, units, "{0:0.#}");
	}

	// Token: 0x060049E4 RID: 18916 RVA: 0x000D45B8 File Offset: 0x000D27B8
	public float ProcessedSliderValue(float input)
	{
		input = Mathf.Round(input);
		return input;
	}

	// Token: 0x060049E5 RID: 18917 RVA: 0x000B64D6 File Offset: 0x000B46D6
	public float ProcessedInputValue(float input)
	{
		return input;
	}

	// Token: 0x060049E6 RID: 18918 RVA: 0x000CDA3B File Offset: 0x000CBC3B
	public LocString ThresholdValueUnits()
	{
		return GameUtil.GetCurrentMassUnit(false);
	}

	// Token: 0x170003CF RID: 975
	// (get) Token: 0x060049E7 RID: 18919 RVA: 0x000B1628 File Offset: 0x000AF828
	public ThresholdScreenLayoutType LayoutType
	{
		get
		{
			return ThresholdScreenLayoutType.SliderBar;
		}
	}

	// Token: 0x170003D0 RID: 976
	// (get) Token: 0x060049E8 RID: 18920 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public int IncrementScale
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x170003D1 RID: 977
	// (get) Token: 0x060049E9 RID: 18921 RVA: 0x000D45C3 File Offset: 0x000D27C3
	public NonLinearSlider.Range[] GetRanges
	{
		get
		{
			return NonLinearSlider.GetDefaultRange(this.RangeMax);
		}
	}

	// Token: 0x060049EA RID: 18922 RVA: 0x000D45D0 File Offset: 0x000D27D0
	private void SwitchToggled(bool toggled_on)
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, toggled_on ? 1 : 0);
	}

	// Token: 0x060049EB RID: 18923 RVA: 0x00268C48 File Offset: 0x00266E48
	private void UpdateVisualState(bool force = false)
	{
		bool flag = this.CurrentValue > this.threshold;
		if (flag != this.was_pressed || this.was_on != base.IsSwitchedOn || force)
		{
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			if (flag)
			{
				if (force)
				{
					component.Play(base.IsSwitchedOn ? "on_down" : "off_down", KAnim.PlayMode.Once, 1f, 0f);
				}
				else
				{
					component.Play(base.IsSwitchedOn ? "on_down_pre" : "off_down_pre", KAnim.PlayMode.Once, 1f, 0f);
					component.Queue(base.IsSwitchedOn ? "on_down" : "off_down", KAnim.PlayMode.Once, 1f, 0f);
				}
			}
			else if (force)
			{
				component.Play(base.IsSwitchedOn ? "on_up" : "off_up", KAnim.PlayMode.Once, 1f, 0f);
			}
			else
			{
				component.Play(base.IsSwitchedOn ? "on_up_pre" : "off_up_pre", KAnim.PlayMode.Once, 1f, 0f);
				component.Queue(base.IsSwitchedOn ? "on_up" : "off_up", KAnim.PlayMode.Once, 1f, 0f);
			}
			this.was_pressed = flag;
			this.was_on = base.IsSwitchedOn;
		}
	}

	// Token: 0x060049EC RID: 18924 RVA: 0x0026473C File Offset: 0x0026293C
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x040033FC RID: 13308
	[SerializeField]
	[Serialize]
	private float threshold;

	// Token: 0x040033FD RID: 13309
	[SerializeField]
	[Serialize]
	private bool activateAboveThreshold = true;

	// Token: 0x040033FE RID: 13310
	[MyCmpGet]
	private LogicPorts logicPorts;

	// Token: 0x040033FF RID: 13311
	private bool was_pressed;

	// Token: 0x04003400 RID: 13312
	private bool was_on;

	// Token: 0x04003401 RID: 13313
	public float rangeMin;

	// Token: 0x04003402 RID: 13314
	public float rangeMax = 1f;

	// Token: 0x04003403 RID: 13315
	[Serialize]
	private float massSolid;

	// Token: 0x04003404 RID: 13316
	[Serialize]
	private float massPickupables;

	// Token: 0x04003405 RID: 13317
	[Serialize]
	private float massActivators;

	// Token: 0x04003406 RID: 13318
	private const float MIN_TOGGLE_TIME = 0.15f;

	// Token: 0x04003407 RID: 13319
	private float toggleCooldown = 0.15f;

	// Token: 0x04003408 RID: 13320
	private HandleVector<int>.Handle solidChangedEntry;

	// Token: 0x04003409 RID: 13321
	private HandleVector<int>.Handle pickupablesChangedEntry;

	// Token: 0x0400340A RID: 13322
	private HandleVector<int>.Handle floorSwitchActivatorChangedEntry;

	// Token: 0x0400340B RID: 13323
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x0400340C RID: 13324
	private static readonly EventSystem.IntraObjectHandler<LogicMassSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicMassSensor>(delegate(LogicMassSensor component, object data)
	{
		component.OnCopySettings(data);
	});
}
