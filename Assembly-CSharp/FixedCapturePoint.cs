using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000A87 RID: 2695
public class FixedCapturePoint : GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>
{
	// Token: 0x0600310E RID: 12558 RVA: 0x0020C180 File Offset: 0x0020A380
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.operational;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.unoperational.TagTransition(GameTags.Operational, this.operational, false);
		this.operational.DefaultState(this.operational.manual).TagTransition(GameTags.Operational, this.unoperational, true);
		this.operational.manual.ParamTransition<bool>(this.automated, this.operational.automated, GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.IsTrue);
		this.operational.automated.ParamTransition<bool>(this.automated, this.operational.manual, GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.IsFalse).ToggleChore((FixedCapturePoint.Instance smi) => smi.CreateChore(), this.unoperational, this.unoperational).Update("FindFixedCapturable", delegate(FixedCapturePoint.Instance smi, float dt)
		{
			smi.FindFixedCapturable();
		}, UpdateRate.SIM_1000ms, false);
	}

	// Token: 0x040021C5 RID: 8645
	public static readonly Operational.Flag enabledFlag = new Operational.Flag("enabled", Operational.Flag.Type.Requirement);

	// Token: 0x040021C6 RID: 8646
	private StateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.BoolParameter automated;

	// Token: 0x040021C7 RID: 8647
	public GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.State unoperational;

	// Token: 0x040021C8 RID: 8648
	public FixedCapturePoint.OperationalState operational;

	// Token: 0x02000A88 RID: 2696
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040021C9 RID: 8649
		public Func<FixedCapturePoint.Instance, FixedCapturableMonitor.Instance, bool> isAmountStoredOverCapacity;

		// Token: 0x040021CA RID: 8650
		public Func<FixedCapturePoint.Instance, int> getTargetCapturePoint = delegate(FixedCapturePoint.Instance smi)
		{
			int num = Grid.PosToCell(smi);
			Navigator navigator = smi.targetCapturable.Navigator;
			if (Grid.IsValidCell(num - 1) && navigator.CanReach(num - 1))
			{
				return num - 1;
			}
			if (Grid.IsValidCell(num + 1) && navigator.CanReach(num + 1))
			{
				return num + 1;
			}
			return num;
		};

		// Token: 0x040021CB RID: 8651
		public bool allowBabies;
	}

	// Token: 0x02000A8A RID: 2698
	public class OperationalState : GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.State
	{
		// Token: 0x040021CE RID: 8654
		public GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.State manual;

		// Token: 0x040021CF RID: 8655
		public GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.State automated;
	}

	// Token: 0x02000A8B RID: 2699
	[SerializationConfig(MemberSerialization.OptIn)]
	public new class Instance : GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.GameInstance
	{
		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06003116 RID: 12566 RVA: 0x000C4596 File Offset: 0x000C2796
		// (set) Token: 0x06003117 RID: 12567 RVA: 0x000C459E File Offset: 0x000C279E
		public FixedCapturableMonitor.Instance targetCapturable { get; private set; }

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x06003118 RID: 12568 RVA: 0x000C45A7 File Offset: 0x000C27A7
		// (set) Token: 0x06003119 RID: 12569 RVA: 0x000C45AF File Offset: 0x000C27AF
		public bool shouldCreatureGoGetCaptured { get; private set; }

		// Token: 0x0600311A RID: 12570 RVA: 0x0020C2DC File Offset: 0x0020A4DC
		public Instance(IStateMachineTarget master, FixedCapturePoint.Def def) : base(master, def)
		{
			base.Subscribe(-905833192, new Action<object>(this.OnCopySettings));
			this.captureCell = Grid.PosToCell(base.transform.GetPosition());
			this.critterCapactiy = base.GetComponent<BaggableCritterCapacityTracker>();
			this.operationComp = base.GetComponent<Operational>();
			this.logicPorts = base.GetComponent<LogicPorts>();
			if (this.logicPorts != null)
			{
				base.Subscribe(-801688580, new Action<object>(this.OnLogicEvent));
				this.operationComp.SetFlag(FixedCapturePoint.enabledFlag, !this.logicPorts.IsPortConnected("CritterPickUpInput") || this.logicPorts.GetInputValue("CritterPickUpInput") > 0);
				return;
			}
			this.operationComp.SetFlag(FixedCapturePoint.enabledFlag, true);
		}

		// Token: 0x0600311B RID: 12571 RVA: 0x0020C3BC File Offset: 0x0020A5BC
		private void OnLogicEvent(object data)
		{
			LogicValueChanged logicValueChanged = (LogicValueChanged)data;
			if (logicValueChanged.portID == "CritterPickUpInput" && this.logicPorts.IsPortConnected("CritterPickUpInput"))
			{
				this.operationComp.SetFlag(FixedCapturePoint.enabledFlag, logicValueChanged.newValue > 0);
			}
		}

		// Token: 0x0600311C RID: 12572 RVA: 0x000C45B8 File Offset: 0x000C27B8
		public override void StartSM()
		{
			base.StartSM();
			if (base.GetComponent<FixedCapturePoint.AutoWrangleCapture>() == null)
			{
				base.sm.automated.Set(true, this, false);
			}
		}

		// Token: 0x0600311D RID: 12573 RVA: 0x0020C418 File Offset: 0x0020A618
		private void OnCopySettings(object data)
		{
			GameObject gameObject = (GameObject)data;
			if (gameObject == null)
			{
				return;
			}
			FixedCapturePoint.Instance smi = gameObject.GetSMI<FixedCapturePoint.Instance>();
			if (smi == null)
			{
				return;
			}
			base.sm.automated.Set(base.sm.automated.Get(smi), this, false);
		}

		// Token: 0x0600311E RID: 12574 RVA: 0x000C45E2 File Offset: 0x000C27E2
		public bool GetAutomated()
		{
			return base.sm.automated.Get(this);
		}

		// Token: 0x0600311F RID: 12575 RVA: 0x000C45F5 File Offset: 0x000C27F5
		public void SetAutomated(bool automate)
		{
			base.sm.automated.Set(automate, this, false);
		}

		// Token: 0x06003120 RID: 12576 RVA: 0x000C460B File Offset: 0x000C280B
		public Chore CreateChore()
		{
			this.FindFixedCapturable();
			return new FixedCaptureChore(base.GetComponent<KPrefabID>());
		}

		// Token: 0x06003121 RID: 12577 RVA: 0x0020C468 File Offset: 0x0020A668
		public bool IsCreatureAvailableForFixedCapture()
		{
			if (!this.targetCapturable.IsNullOrStopped())
			{
				CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(this.captureCell);
				return FixedCapturePoint.Instance.CanCapturableBeCapturedAtCapturePoint(this.targetCapturable, this, cavityForCell, this.captureCell);
			}
			return false;
		}

		// Token: 0x06003122 RID: 12578 RVA: 0x000C461E File Offset: 0x000C281E
		public void SetRancherIsAvailableForCapturing()
		{
			this.shouldCreatureGoGetCaptured = true;
		}

		// Token: 0x06003123 RID: 12579 RVA: 0x000C4627 File Offset: 0x000C2827
		public void ClearRancherIsAvailableForCapturing()
		{
			this.shouldCreatureGoGetCaptured = false;
		}

		// Token: 0x06003124 RID: 12580 RVA: 0x0020C4B0 File Offset: 0x0020A6B0
		private static bool CanCapturableBeCapturedAtCapturePoint(FixedCapturableMonitor.Instance capturable, FixedCapturePoint.Instance capture_point, CavityInfo capture_cavity_info, int capture_cell)
		{
			if (!capturable.IsRunning())
			{
				return false;
			}
			if (capturable.targetCapturePoint != capture_point && !capturable.targetCapturePoint.IsNullOrStopped())
			{
				return false;
			}
			int cell = Grid.PosToCell(capturable.transform.GetPosition());
			CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(cell);
			return cavityForCell != null && cavityForCell == capture_cavity_info && !capturable.HasTag(GameTags.Creatures.Bagged) && (!capturable.isBaby || capture_point.def.allowBabies) && capturable.ChoreConsumer.IsChoreEqualOrAboveCurrentChorePriority<FixedCaptureStates>() && capturable.Navigator.GetNavigationCost(capture_cell) != -1 && capture_point.def.isAmountStoredOverCapacity(capture_point, capturable);
		}

		// Token: 0x06003125 RID: 12581 RVA: 0x0020C564 File Offset: 0x0020A764
		public void FindFixedCapturable()
		{
			int num = Grid.PosToCell(base.transform.GetPosition());
			CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(num);
			if (cavityForCell == null)
			{
				this.ResetCapturePoint();
				return;
			}
			if (!this.targetCapturable.IsNullOrStopped() && !FixedCapturePoint.Instance.CanCapturableBeCapturedAtCapturePoint(this.targetCapturable, this, cavityForCell, num))
			{
				this.ResetCapturePoint();
			}
			if (this.targetCapturable.IsNullOrStopped())
			{
				foreach (object obj in Components.FixedCapturableMonitors)
				{
					FixedCapturableMonitor.Instance instance = (FixedCapturableMonitor.Instance)obj;
					if (FixedCapturePoint.Instance.CanCapturableBeCapturedAtCapturePoint(instance, this, cavityForCell, num))
					{
						this.targetCapturable = instance;
						if (!this.targetCapturable.IsNullOrStopped())
						{
							this.targetCapturable.targetCapturePoint = this;
							break;
						}
						break;
					}
				}
			}
		}

		// Token: 0x06003126 RID: 12582 RVA: 0x000C4630 File Offset: 0x000C2830
		public void ResetCapturePoint()
		{
			base.Trigger(643180843, null);
			if (!this.targetCapturable.IsNullOrStopped())
			{
				this.targetCapturable.targetCapturePoint = null;
				this.targetCapturable.Trigger(1034952693, null);
				this.targetCapturable = null;
			}
		}

		// Token: 0x040021D2 RID: 8658
		public BaggableCritterCapacityTracker critterCapactiy;

		// Token: 0x040021D3 RID: 8659
		private int captureCell;

		// Token: 0x040021D4 RID: 8660
		private Operational operationComp;

		// Token: 0x040021D5 RID: 8661
		private LogicPorts logicPorts;
	}

	// Token: 0x02000A8C RID: 2700
	public class AutoWrangleCapture : KMonoBehaviour, ICheckboxControl
	{
		// Token: 0x06003127 RID: 12583 RVA: 0x000C466F File Offset: 0x000C286F
		protected override void OnSpawn()
		{
			base.OnSpawn();
			this.fcp = this.GetSMI<FixedCapturePoint.Instance>();
		}

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x06003128 RID: 12584 RVA: 0x000C4683 File Offset: 0x000C2883
		string ICheckboxControl.CheckboxTitleKey
		{
			get
			{
				return UI.UISIDESCREENS.CAPTURE_POINT_SIDE_SCREEN.TITLE.key.String;
			}
		}

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x06003129 RID: 12585 RVA: 0x000C4694 File Offset: 0x000C2894
		string ICheckboxControl.CheckboxLabel
		{
			get
			{
				return UI.UISIDESCREENS.CAPTURE_POINT_SIDE_SCREEN.AUTOWRANGLE;
			}
		}

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x0600312A RID: 12586 RVA: 0x000C46A0 File Offset: 0x000C28A0
		string ICheckboxControl.CheckboxTooltip
		{
			get
			{
				return UI.UISIDESCREENS.CAPTURE_POINT_SIDE_SCREEN.AUTOWRANGLE_TOOLTIP;
			}
		}

		// Token: 0x0600312B RID: 12587 RVA: 0x000C46AC File Offset: 0x000C28AC
		bool ICheckboxControl.GetCheckboxValue()
		{
			return this.fcp.GetAutomated();
		}

		// Token: 0x0600312C RID: 12588 RVA: 0x000C46B9 File Offset: 0x000C28B9
		void ICheckboxControl.SetCheckboxValue(bool value)
		{
			this.fcp.SetAutomated(value);
		}

		// Token: 0x040021D6 RID: 8662
		private FixedCapturePoint.Instance fcp;
	}
}
