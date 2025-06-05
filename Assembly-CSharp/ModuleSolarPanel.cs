using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000F17 RID: 3863
[SerializationConfig(MemberSerialization.OptIn)]
public class ModuleSolarPanel : Generator
{
	// Token: 0x06004D60 RID: 19808 RVA: 0x000D6950 File Offset: 0x000D4B50
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.IsVirtual = true;
	}

	// Token: 0x06004D61 RID: 19809 RVA: 0x00273B68 File Offset: 0x00271D68
	protected override void OnSpawn()
	{
		CraftModuleInterface craftInterface = base.GetComponent<RocketModuleCluster>().CraftInterface;
		base.VirtualCircuitKey = craftInterface;
		base.OnSpawn();
		base.Subscribe<ModuleSolarPanel>(824508782, ModuleSolarPanel.OnActiveChangedDelegate);
		this.smi = new ModuleSolarPanel.StatesInstance(this);
		this.smi.StartSM();
		this.accumulator = Game.Instance.accumulators.Add("Element", this);
		BuildingDef def = base.GetComponent<BuildingComplete>().Def;
		Grid.PosToCell(this);
		this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target",
			"meter_fill",
			"meter_frame",
			"meter_OL"
		});
		this.meter.gameObject.GetComponent<KBatchedAnimTracker>().matchParentOffset = true;
	}

	// Token: 0x06004D62 RID: 19810 RVA: 0x000D695F File Offset: 0x000D4B5F
	protected override void OnCleanUp()
	{
		this.smi.StopSM("cleanup");
		Game.Instance.accumulators.Remove(this.accumulator);
		base.OnCleanUp();
	}

	// Token: 0x06004D63 RID: 19811 RVA: 0x00273C40 File Offset: 0x00271E40
	protected void OnActiveChanged(object data)
	{
		StatusItem status_item = ((Operational)data).IsActive ? Db.Get().BuildingStatusItems.Wattage : Db.Get().BuildingStatusItems.GeneratorOffline;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, this);
	}

	// Token: 0x06004D64 RID: 19812 RVA: 0x00273C98 File Offset: 0x00271E98
	private void UpdateStatusItem()
	{
		this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.Wattage, false);
		if (this.statusHandle == Guid.Empty)
		{
			this.statusHandle = this.selectable.AddStatusItem(Db.Get().BuildingStatusItems.ModuleSolarPanelWattage, this);
			return;
		}
		if (this.statusHandle != Guid.Empty)
		{
			base.GetComponent<KSelectable>().ReplaceStatusItem(this.statusHandle, Db.Get().BuildingStatusItems.ModuleSolarPanelWattage, this);
		}
	}

	// Token: 0x06004D65 RID: 19813 RVA: 0x00273D2C File Offset: 0x00271F2C
	public override void EnergySim200ms(float dt)
	{
		ushort circuitID = base.CircuitID;
		this.operational.SetFlag(Generator.wireConnectedFlag, true);
		this.operational.SetFlag(Generator.generatorConnectedFlag, true);
		if (!this.operational.IsOperational)
		{
			return;
		}
		float num = 0f;
		if (Grid.IsValidCell(Grid.PosToCell(this)) && Grid.WorldIdx[Grid.PosToCell(this)] != 255)
		{
			foreach (CellOffset offset in this.solarCellOffsets)
			{
				int num2 = Grid.LightIntensity[Grid.OffsetCell(Grid.PosToCell(this), offset)];
				num += (float)num2 * 0.00053f;
			}
		}
		else
		{
			num = 60f;
		}
		num = Mathf.Clamp(num, 0f, 60f);
		this.operational.SetActive(num > 0f, false);
		Game.Instance.accumulators.Accumulate(this.accumulator, num * dt);
		if (num > 0f)
		{
			num *= dt;
			num = Mathf.Max(num, 1f * dt);
			base.GenerateJoules(num, false);
		}
		this.meter.SetPositionPercent(Game.Instance.accumulators.GetAverageRate(this.accumulator) / 60f);
		this.UpdateStatusItem();
	}

	// Token: 0x17000441 RID: 1089
	// (get) Token: 0x06004D66 RID: 19814 RVA: 0x000D698D File Offset: 0x000D4B8D
	public float CurrentWattage
	{
		get
		{
			return Game.Instance.accumulators.GetAverageRate(this.accumulator);
		}
	}

	// Token: 0x04003659 RID: 13913
	private MeterController meter;

	// Token: 0x0400365A RID: 13914
	private HandleVector<int>.Handle accumulator = HandleVector<int>.InvalidHandle;

	// Token: 0x0400365B RID: 13915
	private ModuleSolarPanel.StatesInstance smi;

	// Token: 0x0400365C RID: 13916
	private Guid statusHandle;

	// Token: 0x0400365D RID: 13917
	private CellOffset[] solarCellOffsets = new CellOffset[]
	{
		new CellOffset(-1, 0),
		new CellOffset(0, 0),
		new CellOffset(1, 0)
	};

	// Token: 0x0400365E RID: 13918
	private static readonly EventSystem.IntraObjectHandler<ModuleSolarPanel> OnActiveChangedDelegate = new EventSystem.IntraObjectHandler<ModuleSolarPanel>(delegate(ModuleSolarPanel component, object data)
	{
		component.OnActiveChanged(data);
	});

	// Token: 0x02000F18 RID: 3864
	public class StatesInstance : GameStateMachine<ModuleSolarPanel.States, ModuleSolarPanel.StatesInstance, ModuleSolarPanel, object>.GameInstance
	{
		// Token: 0x06004D69 RID: 19817 RVA: 0x000D69C0 File Offset: 0x000D4BC0
		public StatesInstance(ModuleSolarPanel master) : base(master)
		{
		}
	}

	// Token: 0x02000F19 RID: 3865
	public class States : GameStateMachine<ModuleSolarPanel.States, ModuleSolarPanel.StatesInstance, ModuleSolarPanel>
	{
		// Token: 0x06004D6A RID: 19818 RVA: 0x000D69C9 File Offset: 0x000D4BC9
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.idle.EventTransition(GameHashes.DoLaunchRocket, this.launch, null).DoNothing();
			this.launch.EventTransition(GameHashes.RocketLanded, this.idle, null);
		}

		// Token: 0x0400365F RID: 13919
		public GameStateMachine<ModuleSolarPanel.States, ModuleSolarPanel.StatesInstance, ModuleSolarPanel, object>.State idle;

		// Token: 0x04003660 RID: 13920
		public GameStateMachine<ModuleSolarPanel.States, ModuleSolarPanel.StatesInstance, ModuleSolarPanel, object>.State launch;
	}
}
