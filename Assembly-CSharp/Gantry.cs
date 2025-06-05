using System;
using STRINGS;

// Token: 0x02000DC5 RID: 3525
public class Gantry : Switch
{
	// Token: 0x060044A1 RID: 17569 RVA: 0x00257040 File Offset: 0x00255240
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (Gantry.infoStatusItem == null)
		{
			Gantry.infoStatusItem = new StatusItem("GantryAutomationInfo", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			Gantry.infoStatusItem.resolveStringCallback = new Func<string, object, string>(Gantry.ResolveInfoStatusItemString);
		}
		base.GetComponent<KAnimControllerBase>().PlaySpeedMultiplier = 0.5f;
		this.smi = new Gantry.Instance(this, base.IsSwitchedOn);
		this.smi.StartSM();
		base.GetComponent<KSelectable>().ToggleStatusItem(Gantry.infoStatusItem, true, this.smi);
	}

	// Token: 0x060044A2 RID: 17570 RVA: 0x000D0C61 File Offset: 0x000CEE61
	protected override void OnCleanUp()
	{
		if (this.smi != null)
		{
			this.smi.StopSM("cleanup");
		}
		base.OnCleanUp();
	}

	// Token: 0x060044A3 RID: 17571 RVA: 0x000D0C81 File Offset: 0x000CEE81
	public void SetWalkable(bool active)
	{
		this.fakeFloorAdder.SetFloor(active);
	}

	// Token: 0x060044A4 RID: 17572 RVA: 0x000D0C8F File Offset: 0x000CEE8F
	protected override void Toggle()
	{
		base.Toggle();
		this.smi.SetSwitchState(this.switchedOn);
	}

	// Token: 0x060044A5 RID: 17573 RVA: 0x000D0CA8 File Offset: 0x000CEEA8
	protected override void OnRefreshUserMenu(object data)
	{
		if (!this.smi.IsAutomated())
		{
			base.OnRefreshUserMenu(data);
		}
	}

	// Token: 0x060044A6 RID: 17574 RVA: 0x000AA038 File Offset: 0x000A8238
	protected override void UpdateSwitchStatus()
	{
	}

	// Token: 0x060044A7 RID: 17575 RVA: 0x002570E0 File Offset: 0x002552E0
	private static string ResolveInfoStatusItemString(string format_str, object data)
	{
		Gantry.Instance instance = (Gantry.Instance)data;
		string format = instance.IsAutomated() ? BUILDING.STATUSITEMS.GANTRY.AUTOMATION_CONTROL : BUILDING.STATUSITEMS.GANTRY.MANUAL_CONTROL;
		string arg = instance.IsExtended() ? BUILDING.STATUSITEMS.GANTRY.EXTENDED : BUILDING.STATUSITEMS.GANTRY.RETRACTED;
		return string.Format(format, arg);
	}

	// Token: 0x04002F9F RID: 12191
	public static readonly HashedString PORT_ID = "Gantry";

	// Token: 0x04002FA0 RID: 12192
	[MyCmpReq]
	private Building building;

	// Token: 0x04002FA1 RID: 12193
	[MyCmpReq]
	private FakeFloorAdder fakeFloorAdder;

	// Token: 0x04002FA2 RID: 12194
	private Gantry.Instance smi;

	// Token: 0x04002FA3 RID: 12195
	private static StatusItem infoStatusItem;

	// Token: 0x02000DC6 RID: 3526
	public class States : GameStateMachine<Gantry.States, Gantry.Instance, Gantry>
	{
		// Token: 0x060044AA RID: 17578 RVA: 0x00257130 File Offset: 0x00255330
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.extended;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.retracted_pre.Enter(delegate(Gantry.Instance smi)
			{
				smi.SetActive(true);
			}).Exit(delegate(Gantry.Instance smi)
			{
				smi.SetActive(false);
			}).PlayAnim("off_pre").OnAnimQueueComplete(this.retracted);
			this.retracted.PlayAnim("off").ParamTransition<bool>(this.should_extend, this.extended_pre, GameStateMachine<Gantry.States, Gantry.Instance, Gantry, object>.IsTrue);
			this.extended_pre.Enter(delegate(Gantry.Instance smi)
			{
				smi.SetActive(true);
			}).Exit(delegate(Gantry.Instance smi)
			{
				smi.SetActive(false);
			}).PlayAnim("on_pre").OnAnimQueueComplete(this.extended);
			this.extended.Enter(delegate(Gantry.Instance smi)
			{
				smi.master.SetWalkable(true);
			}).Exit(delegate(Gantry.Instance smi)
			{
				smi.master.SetWalkable(false);
			}).PlayAnim("on").ParamTransition<bool>(this.should_extend, this.retracted_pre, GameStateMachine<Gantry.States, Gantry.Instance, Gantry, object>.IsFalse).ToggleTag(GameTags.GantryExtended);
		}

		// Token: 0x04002FA4 RID: 12196
		public GameStateMachine<Gantry.States, Gantry.Instance, Gantry, object>.State retracted_pre;

		// Token: 0x04002FA5 RID: 12197
		public GameStateMachine<Gantry.States, Gantry.Instance, Gantry, object>.State retracted;

		// Token: 0x04002FA6 RID: 12198
		public GameStateMachine<Gantry.States, Gantry.Instance, Gantry, object>.State extended_pre;

		// Token: 0x04002FA7 RID: 12199
		public GameStateMachine<Gantry.States, Gantry.Instance, Gantry, object>.State extended;

		// Token: 0x04002FA8 RID: 12200
		public StateMachine<Gantry.States, Gantry.Instance, Gantry, object>.BoolParameter should_extend;
	}

	// Token: 0x02000DC8 RID: 3528
	public class Instance : GameStateMachine<Gantry.States, Gantry.Instance, Gantry, object>.GameInstance
	{
		// Token: 0x060044B4 RID: 17588 RVA: 0x002572B4 File Offset: 0x002554B4
		public Instance(Gantry master, bool manual_start_state) : base(master)
		{
			this.manual_on = manual_start_state;
			this.operational = base.GetComponent<Operational>();
			this.logic = base.GetComponent<LogicPorts>();
			base.Subscribe(-592767678, new Action<object>(this.OnOperationalChanged));
			base.Subscribe(-801688580, new Action<object>(this.OnLogicValueChanged));
			base.smi.sm.should_extend.Set(true, base.smi, false);
		}

		// Token: 0x060044B5 RID: 17589 RVA: 0x000D0D11 File Offset: 0x000CEF11
		public bool IsAutomated()
		{
			return this.logic.IsPortConnected(Gantry.PORT_ID);
		}

		// Token: 0x060044B6 RID: 17590 RVA: 0x000D0D23 File Offset: 0x000CEF23
		public bool IsExtended()
		{
			if (!this.IsAutomated())
			{
				return this.manual_on;
			}
			return this.logic_on;
		}

		// Token: 0x060044B7 RID: 17591 RVA: 0x000D0D3A File Offset: 0x000CEF3A
		public void SetSwitchState(bool on)
		{
			this.manual_on = on;
			this.UpdateShouldExtend();
		}

		// Token: 0x060044B8 RID: 17592 RVA: 0x000D0D49 File Offset: 0x000CEF49
		public void SetActive(bool active)
		{
			this.operational.SetActive(this.operational.IsOperational && active, false);
		}

		// Token: 0x060044B9 RID: 17593 RVA: 0x000D0D64 File Offset: 0x000CEF64
		private void OnOperationalChanged(object data)
		{
			this.UpdateShouldExtend();
		}

		// Token: 0x060044BA RID: 17594 RVA: 0x0025733C File Offset: 0x0025553C
		private void OnLogicValueChanged(object data)
		{
			LogicValueChanged logicValueChanged = (LogicValueChanged)data;
			if (logicValueChanged.portID != Gantry.PORT_ID)
			{
				return;
			}
			this.logic_on = LogicCircuitNetwork.IsBitActive(0, logicValueChanged.newValue);
			this.UpdateShouldExtend();
		}

		// Token: 0x060044BB RID: 17595 RVA: 0x0025737C File Offset: 0x0025557C
		private void UpdateShouldExtend()
		{
			if (!this.operational.IsOperational)
			{
				return;
			}
			if (this.IsAutomated())
			{
				base.smi.sm.should_extend.Set(this.logic_on, base.smi, false);
				return;
			}
			base.smi.sm.should_extend.Set(this.manual_on, base.smi, false);
		}

		// Token: 0x04002FB0 RID: 12208
		private Operational operational;

		// Token: 0x04002FB1 RID: 12209
		public LogicPorts logic;

		// Token: 0x04002FB2 RID: 12210
		public bool logic_on = true;

		// Token: 0x04002FB3 RID: 12211
		private bool manual_on;
	}
}
