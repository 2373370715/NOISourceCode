using System;
using System.Collections.Generic;

// Token: 0x02000F07 RID: 3847
public class MissionControlCluster : GameStateMachine<MissionControlCluster, MissionControlCluster.Instance, IStateMachineTarget, MissionControlCluster.Def>
{
	// Token: 0x06004D11 RID: 19729 RVA: 0x00272E2C File Offset: 0x0027102C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.Inoperational;
		this.Inoperational.EventTransition(GameHashes.OperationalChanged, this.Operational, new StateMachine<MissionControlCluster, MissionControlCluster.Instance, IStateMachineTarget, MissionControlCluster.Def>.Transition.ConditionCallback(this.ValidateOperationalTransition)).EventTransition(GameHashes.UpdateRoom, this.Operational, new StateMachine<MissionControlCluster, MissionControlCluster.Instance, IStateMachineTarget, MissionControlCluster.Def>.Transition.ConditionCallback(this.ValidateOperationalTransition));
		this.Operational.EventTransition(GameHashes.OperationalChanged, this.Inoperational, new StateMachine<MissionControlCluster, MissionControlCluster.Instance, IStateMachineTarget, MissionControlCluster.Def>.Transition.ConditionCallback(this.ValidateOperationalTransition)).EventTransition(GameHashes.UpdateRoom, this.Operational.WrongRoom, GameStateMachine<MissionControlCluster, MissionControlCluster.Instance, IStateMachineTarget, MissionControlCluster.Def>.Not(new StateMachine<MissionControlCluster, MissionControlCluster.Instance, IStateMachineTarget, MissionControlCluster.Def>.Transition.ConditionCallback(this.IsInLabRoom))).Enter(new StateMachine<MissionControlCluster, MissionControlCluster.Instance, IStateMachineTarget, MissionControlCluster.Def>.State.Callback(this.OnEnterOperational)).DefaultState(this.Operational.NoRockets).Update(delegate(MissionControlCluster.Instance smi, float dt)
		{
			smi.UpdateWorkableRocketsInRange(null);
		}, UpdateRate.SIM_1000ms, false);
		this.Operational.WrongRoom.EventTransition(GameHashes.UpdateRoom, this.Operational.NoRockets, new StateMachine<MissionControlCluster, MissionControlCluster.Instance, IStateMachineTarget, MissionControlCluster.Def>.Transition.ConditionCallback(this.IsInLabRoom));
		this.Operational.NoRockets.ToggleStatusItem(Db.Get().BuildingStatusItems.NoRocketsToMissionControlClusterBoost, null).ParamTransition<bool>(this.WorkableRocketsAreInRange, this.Operational.HasRockets, (MissionControlCluster.Instance smi, bool inRange) => this.WorkableRocketsAreInRange.Get(smi));
		this.Operational.HasRockets.ParamTransition<bool>(this.WorkableRocketsAreInRange, this.Operational.NoRockets, (MissionControlCluster.Instance smi, bool inRange) => !this.WorkableRocketsAreInRange.Get(smi)).ToggleChore(new Func<MissionControlCluster.Instance, Chore>(this.CreateChore), this.Operational);
	}

	// Token: 0x06004D12 RID: 19730 RVA: 0x00272FC8 File Offset: 0x002711C8
	private Chore CreateChore(MissionControlCluster.Instance smi)
	{
		MissionControlClusterWorkable component = smi.master.gameObject.GetComponent<MissionControlClusterWorkable>();
		Chore result = new WorkChore<MissionControlClusterWorkable>(Db.Get().ChoreTypes.Research, component, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		Clustercraft randomBoostableClustercraft = smi.GetRandomBoostableClustercraft();
		component.TargetClustercraft = randomBoostableClustercraft;
		return result;
	}

	// Token: 0x06004D13 RID: 19731 RVA: 0x000D64BE File Offset: 0x000D46BE
	private void OnEnterOperational(MissionControlCluster.Instance smi)
	{
		smi.UpdateWorkableRocketsInRange(null);
		if (this.WorkableRocketsAreInRange.Get(smi))
		{
			smi.GoTo(this.Operational.HasRockets);
			return;
		}
		smi.GoTo(this.Operational.NoRockets);
	}

	// Token: 0x06004D14 RID: 19732 RVA: 0x0027301C File Offset: 0x0027121C
	private bool ValidateOperationalTransition(MissionControlCluster.Instance smi)
	{
		Operational component = smi.GetComponent<Operational>();
		bool flag = smi.IsInsideState(smi.sm.Operational);
		return component != null && flag != component.IsOperational;
	}

	// Token: 0x06004D15 RID: 19733 RVA: 0x000D64F8 File Offset: 0x000D46F8
	private bool IsInLabRoom(MissionControlCluster.Instance smi)
	{
		return smi.roomTracker.IsInCorrectRoom();
	}

	// Token: 0x04003612 RID: 13842
	public GameStateMachine<MissionControlCluster, MissionControlCluster.Instance, IStateMachineTarget, MissionControlCluster.Def>.State Inoperational;

	// Token: 0x04003613 RID: 13843
	public MissionControlCluster.OperationalState Operational;

	// Token: 0x04003614 RID: 13844
	public StateMachine<MissionControlCluster, MissionControlCluster.Instance, IStateMachineTarget, MissionControlCluster.Def>.BoolParameter WorkableRocketsAreInRange;

	// Token: 0x02000F08 RID: 3848
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000F09 RID: 3849
	public new class Instance : GameStateMachine<MissionControlCluster, MissionControlCluster.Instance, IStateMachineTarget, MissionControlCluster.Def>.GameInstance
	{
		// Token: 0x06004D1A RID: 19738 RVA: 0x000D652C File Offset: 0x000D472C
		public Instance(IStateMachineTarget master, MissionControlCluster.Def def) : base(master, def)
		{
		}

		// Token: 0x06004D1B RID: 19739 RVA: 0x000D6548 File Offset: 0x000D4748
		public override void StartSM()
		{
			base.StartSM();
			this.clusterUpdatedHandle = Game.Instance.Subscribe(-1298331547, new Action<object>(this.UpdateWorkableRocketsInRange));
		}

		// Token: 0x06004D1C RID: 19740 RVA: 0x000D6571 File Offset: 0x000D4771
		public override void StopSM(string reason)
		{
			base.StopSM(reason);
			Game.Instance.Unsubscribe(this.clusterUpdatedHandle);
		}

		// Token: 0x06004D1D RID: 19741 RVA: 0x0027305C File Offset: 0x0027125C
		public void UpdateWorkableRocketsInRange(object data)
		{
			this.boostableClustercraft.Clear();
			AxialI myWorldLocation = base.gameObject.GetMyWorldLocation();
			for (int i = 0; i < Components.Clustercrafts.Count; i++)
			{
				if (ClusterGrid.Instance.IsInRange(Components.Clustercrafts[i].Location, myWorldLocation, 2) && !this.IsOwnWorld(Components.Clustercrafts[i]) && this.CanBeBoosted(Components.Clustercrafts[i]))
				{
					bool flag = false;
					foreach (object obj in Components.MissionControlClusterWorkables)
					{
						MissionControlClusterWorkable missionControlClusterWorkable = (MissionControlClusterWorkable)obj;
						if (!(missionControlClusterWorkable.gameObject == base.gameObject) && missionControlClusterWorkable.TargetClustercraft == Components.Clustercrafts[i])
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						this.boostableClustercraft.Add(Components.Clustercrafts[i]);
					}
				}
			}
			base.sm.WorkableRocketsAreInRange.Set(this.boostableClustercraft.Count > 0, base.smi, false);
		}

		// Token: 0x06004D1E RID: 19742 RVA: 0x000D658A File Offset: 0x000D478A
		public Clustercraft GetRandomBoostableClustercraft()
		{
			return this.boostableClustercraft.GetRandom<Clustercraft>();
		}

		// Token: 0x06004D1F RID: 19743 RVA: 0x000D6597 File Offset: 0x000D4797
		private bool CanBeBoosted(Clustercraft clustercraft)
		{
			return clustercraft.controlStationBuffTimeRemaining == 0f && clustercraft.HasResourcesToMove(1, Clustercraft.CombustionResource.All) && clustercraft.IsFlightInProgress();
		}

		// Token: 0x06004D20 RID: 19744 RVA: 0x002731A4 File Offset: 0x002713A4
		private bool IsOwnWorld(Clustercraft candidateClustercraft)
		{
			int myWorldId = base.gameObject.GetMyWorldId();
			WorldContainer interiorWorld = candidateClustercraft.ModuleInterface.GetInteriorWorld();
			return !(interiorWorld == null) && myWorldId == interiorWorld.id;
		}

		// Token: 0x06004D21 RID: 19745 RVA: 0x000D65BD File Offset: 0x000D47BD
		public void ApplyEffect(Clustercraft clustercraft)
		{
			clustercraft.controlStationBuffTimeRemaining = 600f;
		}

		// Token: 0x04003615 RID: 13845
		private int clusterUpdatedHandle = -1;

		// Token: 0x04003616 RID: 13846
		private List<Clustercraft> boostableClustercraft = new List<Clustercraft>();

		// Token: 0x04003617 RID: 13847
		[MyCmpReq]
		public RoomTracker roomTracker;
	}

	// Token: 0x02000F0A RID: 3850
	public class OperationalState : GameStateMachine<MissionControlCluster, MissionControlCluster.Instance, IStateMachineTarget, MissionControlCluster.Def>.State
	{
		// Token: 0x04003618 RID: 13848
		public GameStateMachine<MissionControlCluster, MissionControlCluster.Instance, IStateMachineTarget, MissionControlCluster.Def>.State WrongRoom;

		// Token: 0x04003619 RID: 13849
		public GameStateMachine<MissionControlCluster, MissionControlCluster.Instance, IStateMachineTarget, MissionControlCluster.Def>.State NoRockets;

		// Token: 0x0400361A RID: 13850
		public GameStateMachine<MissionControlCluster, MissionControlCluster.Instance, IStateMachineTarget, MissionControlCluster.Def>.State HasRockets;
	}
}
