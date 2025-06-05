using System;
using System.Collections.Generic;

// Token: 0x02000F02 RID: 3842
public class MissionControl : GameStateMachine<MissionControl, MissionControl.Instance, IStateMachineTarget, MissionControl.Def>
{
	// Token: 0x06004CFF RID: 19711 RVA: 0x00272AEC File Offset: 0x00270CEC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.Inoperational;
		this.Inoperational.EventTransition(GameHashes.OperationalChanged, this.Operational, new StateMachine<MissionControl, MissionControl.Instance, IStateMachineTarget, MissionControl.Def>.Transition.ConditionCallback(this.ValidateOperationalTransition)).EventTransition(GameHashes.UpdateRoom, this.Operational, new StateMachine<MissionControl, MissionControl.Instance, IStateMachineTarget, MissionControl.Def>.Transition.ConditionCallback(this.ValidateOperationalTransition));
		this.Operational.EventTransition(GameHashes.OperationalChanged, this.Inoperational, new StateMachine<MissionControl, MissionControl.Instance, IStateMachineTarget, MissionControl.Def>.Transition.ConditionCallback(this.ValidateOperationalTransition)).EventTransition(GameHashes.UpdateRoom, this.Operational.WrongRoom, GameStateMachine<MissionControl, MissionControl.Instance, IStateMachineTarget, MissionControl.Def>.Not(new StateMachine<MissionControl, MissionControl.Instance, IStateMachineTarget, MissionControl.Def>.Transition.ConditionCallback(this.IsInLabRoom))).Enter(new StateMachine<MissionControl, MissionControl.Instance, IStateMachineTarget, MissionControl.Def>.State.Callback(this.OnEnterOperational)).DefaultState(this.Operational.NoRockets).Update(delegate(MissionControl.Instance smi, float dt)
		{
			smi.UpdateWorkableRockets(null);
		}, UpdateRate.SIM_1000ms, false);
		this.Operational.WrongRoom.EventTransition(GameHashes.UpdateRoom, this.Operational.NoRockets, new StateMachine<MissionControl, MissionControl.Instance, IStateMachineTarget, MissionControl.Def>.Transition.ConditionCallback(this.IsInLabRoom));
		this.Operational.NoRockets.ToggleStatusItem(Db.Get().BuildingStatusItems.NoRocketsToMissionControlBoost, null).ParamTransition<bool>(this.WorkableRocketsAreInRange, this.Operational.HasRockets, (MissionControl.Instance smi, bool inRange) => this.WorkableRocketsAreInRange.Get(smi));
		this.Operational.HasRockets.ParamTransition<bool>(this.WorkableRocketsAreInRange, this.Operational.NoRockets, (MissionControl.Instance smi, bool inRange) => !this.WorkableRocketsAreInRange.Get(smi)).ToggleChore(new Func<MissionControl.Instance, Chore>(this.CreateChore), this.Operational);
	}

	// Token: 0x06004D00 RID: 19712 RVA: 0x00272C88 File Offset: 0x00270E88
	private Chore CreateChore(MissionControl.Instance smi)
	{
		MissionControlWorkable component = smi.master.gameObject.GetComponent<MissionControlWorkable>();
		Chore result = new WorkChore<MissionControlWorkable>(Db.Get().ChoreTypes.Research, component, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		Spacecraft randomBoostableSpacecraft = smi.GetRandomBoostableSpacecraft();
		component.TargetSpacecraft = randomBoostableSpacecraft;
		return result;
	}

	// Token: 0x06004D01 RID: 19713 RVA: 0x000D63E7 File Offset: 0x000D45E7
	private void OnEnterOperational(MissionControl.Instance smi)
	{
		smi.UpdateWorkableRockets(null);
		if (this.WorkableRocketsAreInRange.Get(smi))
		{
			smi.GoTo(this.Operational.HasRockets);
			return;
		}
		smi.GoTo(this.Operational.NoRockets);
	}

	// Token: 0x06004D02 RID: 19714 RVA: 0x00272CDC File Offset: 0x00270EDC
	private bool ValidateOperationalTransition(MissionControl.Instance smi)
	{
		Operational component = smi.GetComponent<Operational>();
		bool flag = smi.IsInsideState(smi.sm.Operational);
		return component != null && flag != component.IsOperational;
	}

	// Token: 0x06004D03 RID: 19715 RVA: 0x000D6421 File Offset: 0x000D4621
	private bool IsInLabRoom(MissionControl.Instance smi)
	{
		return smi.roomTracker.IsInCorrectRoom();
	}

	// Token: 0x04003608 RID: 13832
	public GameStateMachine<MissionControl, MissionControl.Instance, IStateMachineTarget, MissionControl.Def>.State Inoperational;

	// Token: 0x04003609 RID: 13833
	public MissionControl.OperationalState Operational;

	// Token: 0x0400360A RID: 13834
	public StateMachine<MissionControl, MissionControl.Instance, IStateMachineTarget, MissionControl.Def>.BoolParameter WorkableRocketsAreInRange;

	// Token: 0x02000F03 RID: 3843
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000F04 RID: 3844
	public new class Instance : GameStateMachine<MissionControl, MissionControl.Instance, IStateMachineTarget, MissionControl.Def>.GameInstance
	{
		// Token: 0x06004D08 RID: 19720 RVA: 0x000D6455 File Offset: 0x000D4655
		public Instance(IStateMachineTarget master, MissionControl.Def def) : base(master, def)
		{
		}

		// Token: 0x06004D09 RID: 19721 RVA: 0x00272D1C File Offset: 0x00270F1C
		public void UpdateWorkableRockets(object data)
		{
			this.boostableSpacecraft.Clear();
			for (int i = 0; i < SpacecraftManager.instance.GetSpacecraft().Count; i++)
			{
				if (this.CanBeBoosted(SpacecraftManager.instance.GetSpacecraft()[i]))
				{
					bool flag = false;
					foreach (object obj in Components.MissionControlWorkables)
					{
						MissionControlWorkable missionControlWorkable = (MissionControlWorkable)obj;
						if (!(missionControlWorkable.gameObject == base.gameObject) && missionControlWorkable.TargetSpacecraft == SpacecraftManager.instance.GetSpacecraft()[i])
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						this.boostableSpacecraft.Add(SpacecraftManager.instance.GetSpacecraft()[i]);
					}
				}
			}
			base.sm.WorkableRocketsAreInRange.Set(this.boostableSpacecraft.Count > 0, base.smi, false);
		}

		// Token: 0x06004D0A RID: 19722 RVA: 0x000D646A File Offset: 0x000D466A
		public Spacecraft GetRandomBoostableSpacecraft()
		{
			return this.boostableSpacecraft.GetRandom<Spacecraft>();
		}

		// Token: 0x06004D0B RID: 19723 RVA: 0x000D6477 File Offset: 0x000D4677
		private bool CanBeBoosted(Spacecraft spacecraft)
		{
			return spacecraft.controlStationBuffTimeRemaining == 0f && spacecraft.state == Spacecraft.MissionState.Underway;
		}

		// Token: 0x06004D0C RID: 19724 RVA: 0x000D6494 File Offset: 0x000D4694
		public void ApplyEffect(Spacecraft spacecraft)
		{
			spacecraft.controlStationBuffTimeRemaining = 600f;
		}

		// Token: 0x0400360B RID: 13835
		private List<Spacecraft> boostableSpacecraft = new List<Spacecraft>();

		// Token: 0x0400360C RID: 13836
		[MyCmpReq]
		public RoomTracker roomTracker;
	}

	// Token: 0x02000F05 RID: 3845
	public class OperationalState : GameStateMachine<MissionControl, MissionControl.Instance, IStateMachineTarget, MissionControl.Def>.State
	{
		// Token: 0x0400360D RID: 13837
		public GameStateMachine<MissionControl, MissionControl.Instance, IStateMachineTarget, MissionControl.Def>.State WrongRoom;

		// Token: 0x0400360E RID: 13838
		public GameStateMachine<MissionControl, MissionControl.Instance, IStateMachineTarget, MissionControl.Def>.State NoRockets;

		// Token: 0x0400360F RID: 13839
		public GameStateMachine<MissionControl, MissionControl.Instance, IStateMachineTarget, MissionControl.Def>.State HasRockets;
	}
}
