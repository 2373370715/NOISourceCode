using System;
using Klei;
using KSerialization;
using UnityEngine;

// Token: 0x020017AA RID: 6058
public class RemoteWorkerSM : StateMachineComponent<RemoteWorkerSM.StatesInstance>
{
	// Token: 0x170007D1 RID: 2001
	// (get) Token: 0x06007C81 RID: 31873 RVA: 0x000F65ED File Offset: 0x000F47ED
	// (set) Token: 0x06007C82 RID: 31874 RVA: 0x000F65F5 File Offset: 0x000F47F5
	public bool Docked
	{
		get
		{
			return this.docked;
		}
		set
		{
			this.docked = value;
		}
	}

	// Token: 0x06007C83 RID: 31875 RVA: 0x000F65FE File Offset: 0x000F47FE
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x06007C84 RID: 31876 RVA: 0x0032DBF4 File Offset: 0x0032BDF4
	public void SetNextChore(Chore.Precondition.Context next)
	{
		if (this.nextChore != null)
		{
			this.nextChore.Value.chore.Reserve(null);
		}
		this.nextChore = new Chore.Precondition.Context?(next);
		next.chore.Reserve(this.driver);
	}

	// Token: 0x06007C85 RID: 31877 RVA: 0x000F6611 File Offset: 0x000F4811
	public void StartNextChore()
	{
		if (this.nextChore != null)
		{
			this.driver.SetChore(this.nextChore.Value);
			this.nextChore = null;
		}
	}

	// Token: 0x06007C86 RID: 31878 RVA: 0x000F6642 File Offset: 0x000F4842
	public bool HasChoreQueued()
	{
		return this.nextChore != null;
	}

	// Token: 0x170007D2 RID: 2002
	// (get) Token: 0x06007C87 RID: 31879 RVA: 0x000F664F File Offset: 0x000F484F
	// (set) Token: 0x06007C88 RID: 31880 RVA: 0x000F6662 File Offset: 0x000F4862
	public RemoteWorkerDock HomeDepot
	{
		get
		{
			Ref<RemoteWorkerDock> @ref = this.homeDepot;
			if (@ref == null)
			{
				return null;
			}
			return @ref.Get();
		}
		set
		{
			this.homeDepot = new Ref<RemoteWorkerDock>(value);
		}
	}

	// Token: 0x170007D3 RID: 2003
	// (get) Token: 0x06007C89 RID: 31881 RVA: 0x000F6670 File Offset: 0x000F4870
	public ChoreConsumerState ConsumerState
	{
		get
		{
			return this.consumer.consumerState;
		}
	}

	// Token: 0x170007D4 RID: 2004
	// (get) Token: 0x06007C8A RID: 31882 RVA: 0x000F667D File Offset: 0x000F487D
	// (set) Token: 0x06007C8B RID: 31883 RVA: 0x000F6685 File Offset: 0x000F4885
	public bool ActivelyControlled { get; set; }

	// Token: 0x170007D5 RID: 2005
	// (get) Token: 0x06007C8C RID: 31884 RVA: 0x000F668E File Offset: 0x000F488E
	// (set) Token: 0x06007C8D RID: 31885 RVA: 0x000F6696 File Offset: 0x000F4896
	public bool ActivelyWorking { get; set; }

	// Token: 0x170007D6 RID: 2006
	// (get) Token: 0x06007C8E RID: 31886 RVA: 0x000F669F File Offset: 0x000F489F
	// (set) Token: 0x06007C8F RID: 31887 RVA: 0x000F66A7 File Offset: 0x000F48A7
	public bool Available { get; set; }

	// Token: 0x170007D7 RID: 2007
	// (get) Token: 0x06007C90 RID: 31888 RVA: 0x000F66B0 File Offset: 0x000F48B0
	public bool RequiresMaintnence
	{
		get
		{
			return this.power.IsLowPower;
		}
	}

	// Token: 0x06007C91 RID: 31889 RVA: 0x0032DC44 File Offset: 0x0032BE44
	public void TickResources(float dt)
	{
		this.power.ApplyDeltaEnergy(-0.1f * dt);
		float num;
		SimUtil.DiseaseInfo diseaseInfo;
		float temperature;
		this.storage.ConsumeAndGetDisease(GameTags.LubricatingOil, 0.033333335f * dt, out num, out diseaseInfo, out temperature);
		if (num > 0f)
		{
			this.storage.AddElement(SimHashes.LiquidGunk, num, temperature, diseaseInfo.idx, diseaseInfo.count, true, true);
		}
	}

	// Token: 0x06007C92 RID: 31890 RVA: 0x000F66BD File Offset: 0x000F48BD
	public GameObject FindStation()
	{
		if (Components.ComplexFabricators.Count == 0)
		{
			return null;
		}
		return Components.ComplexFabricators[0].gameObject;
	}

	// Token: 0x06007C93 RID: 31891 RVA: 0x000F66DD File Offset: 0x000F48DD
	public bool HasHomeDepot()
	{
		return !this.HomeDepot.IsNullOrDestroyed();
	}

	// Token: 0x04005DBE RID: 23998
	[MyCmpAdd]
	private RemoteWorkerCapacitor power;

	// Token: 0x04005DBF RID: 23999
	[MyCmpAdd]
	private RemoteWorkerGunkMonitor gunk;

	// Token: 0x04005DC0 RID: 24000
	[MyCmpAdd]
	private RemoteWorkerOilMonitor oil;

	// Token: 0x04005DC1 RID: 24001
	[MyCmpAdd]
	private ChoreDriver driver;

	// Token: 0x04005DC2 RID: 24002
	[MyCmpGet]
	private ChoreConsumer consumer;

	// Token: 0x04005DC3 RID: 24003
	[MyCmpGet]
	private Storage storage;

	// Token: 0x04005DC4 RID: 24004
	public bool playNewWorker;

	// Token: 0x04005DC5 RID: 24005
	[Serialize]
	private bool docked = true;

	// Token: 0x04005DC6 RID: 24006
	private Chore.Precondition.Context? nextChore;

	// Token: 0x04005DC7 RID: 24007
	private const string LostAnim_pre = "sos_pre";

	// Token: 0x04005DC8 RID: 24008
	private const string LostAnim_loop = "sos_loop";

	// Token: 0x04005DC9 RID: 24009
	private const string LostAnim_pst = "sos_pst";

	// Token: 0x04005DCA RID: 24010
	private const string DeathAnim = "explode";

	// Token: 0x04005DCB RID: 24011
	[Serialize]
	private Ref<RemoteWorkerDock> homeDepot;

	// Token: 0x020017AB RID: 6059
	public class StatesInstance : GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.GameInstance
	{
		// Token: 0x06007C95 RID: 31893 RVA: 0x000F66FC File Offset: 0x000F48FC
		public StatesInstance(RemoteWorkerSM master) : base(master)
		{
			base.sm.homedock.Set(base.smi.master.HomeDepot, base.smi);
		}
	}

	// Token: 0x020017AC RID: 6060
	public class States : GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM>
	{
		// Token: 0x06007C96 RID: 31894 RVA: 0x0032DCAC File Offset: 0x0032BEAC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.uncontrolled;
			this.controlled.Enter(delegate(RemoteWorkerSM.StatesInstance smi)
			{
				smi.master.Available = false;
			}).EnterTransition(this.controlled.exit_dock, new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.IsInsideDock)).EnterTransition(this.controlled.working, GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Not(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.IsInsideDock))).Transition(this.uncontrolled, GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Not(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.HasRemoteOperator)), UpdateRate.SIM_200ms).Transition(this.incapacitated.lost, GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Not(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.CanReachDepot)), UpdateRate.SIM_200ms).Transition(this.incapacitated.die, GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Not(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.HasHomeDepot)), UpdateRate.SIM_200ms).Update(new Action<RemoteWorkerSM.StatesInstance, float>(RemoteWorkerSM.States.TickResources), UpdateRate.SIM_200ms, false);
			this.controlled.exit_dock.ToggleWork<RemoteWorkerDock.ExitableDock>(this.homedock, this.controlled.working, this.controlled.working, (RemoteWorkerSM.StatesInstance _) => true);
			this.controlled.working.Enter(delegate(RemoteWorkerSM.StatesInstance smi)
			{
				smi.master.ActivelyWorking = true;
			}).Exit(delegate(RemoteWorkerSM.StatesInstance smi)
			{
				smi.master.ActivelyWorking = false;
			}).DefaultState(this.controlled.working.find_work);
			this.controlled.working.find_work.Enter(delegate(RemoteWorkerSM.StatesInstance smi)
			{
				if (RemoteWorkerSM.States.HasChore(smi))
				{
					smi.GoTo(this.controlled.working.do_work);
					return;
				}
				RemoteWorkerSM.States.SetNextChore(smi);
				smi.GoTo(RemoteWorkerSM.States.HasChore(smi) ? this.controlled.working.do_work : this.controlled.no_work);
			});
			this.controlled.working.do_work.Exit(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State.Callback(RemoteWorkerSM.States.ClearChore)).Transition(this.controlled.working.find_work, GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Not(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.HasChore)), UpdateRate.SIM_200ms);
			this.controlled.no_work.Transition(this.controlled.working.do_work, new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.HasChore), UpdateRate.SIM_200ms).Transition(this.controlled.working.find_work, new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.HasChoreQueued), UpdateRate.SIM_200ms);
			this.uncontrolled.EnterTransition(this.uncontrolled.working.new_worker, new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.IsNewWorker)).EnterTransition(this.uncontrolled.idle, GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.And(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.IsInsideDock), GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Not(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.IsNewWorker)))).EnterTransition(this.uncontrolled.approach_dock, GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.And(GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Not(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.IsInsideDock)), GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Not(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.IsNewWorker)))).Transition(this.controlled.working.find_work, new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.HasRemoteOperator), UpdateRate.SIM_200ms).Transition(this.incapacitated.lost, GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Not(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.CanReachDepot)), UpdateRate.SIM_200ms).Transition(this.incapacitated.die, GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Not(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.HasHomeDepot)), UpdateRate.SIM_200ms);
			this.uncontrolled.approach_dock.Enter(delegate(RemoteWorkerSM.StatesInstance smi)
			{
				smi.master.Available = true;
			}).MoveTo<IApproachable>(this.homedock, this.uncontrolled.working.enter, this.incapacitated.lost, null, null);
			this.uncontrolled.working.Enter(delegate(RemoteWorkerSM.StatesInstance smi)
			{
				smi.master.Available = false;
			});
			this.uncontrolled.working.new_worker.Enter(delegate(RemoteWorkerSM.StatesInstance smi)
			{
				smi.master.playNewWorker = false;
			}).ToggleWork<RemoteWorkerDock.NewWorker>(this.homedock, this.uncontrolled.working.recharge, this.uncontrolled.working.recharge, (RemoteWorkerSM.StatesInstance _) => true);
			this.uncontrolled.working.enter.ToggleWork<RemoteWorkerDock.EnterableDock>(this.homedock, this.uncontrolled.working.recharge, this.uncontrolled.idle, (RemoteWorkerSM.StatesInstance _) => true);
			this.uncontrolled.working.recharge.ToggleWork<RemoteWorkerDock.WorkerRecharger>(this.homedock, this.uncontrolled.working.recharge_pst, this.uncontrolled.idle, (RemoteWorkerSM.StatesInstance _) => true);
			this.uncontrolled.working.recharge_pst.OnAnimQueueComplete(this.uncontrolled.working.drain_gunk).ScheduleGoTo(1f, this.uncontrolled.working.drain_gunk);
			this.uncontrolled.working.drain_gunk.ToggleWork<RemoteWorkerDock.WorkerGunkRemover>(this.homedock, this.uncontrolled.working.drain_gunk_pst, this.uncontrolled.idle, (RemoteWorkerSM.StatesInstance _) => true);
			this.uncontrolled.working.drain_gunk_pst.OnAnimQueueComplete(this.uncontrolled.working.fill_oil).ScheduleGoTo(1f, this.uncontrolled.working.fill_oil);
			this.uncontrolled.working.fill_oil.ToggleWork<RemoteWorkerDock.WorkerOilRefiller>(this.homedock, this.uncontrolled.working.fill_oil_pst, this.uncontrolled.idle, (RemoteWorkerSM.StatesInstance _) => true);
			this.uncontrolled.working.fill_oil_pst.OnAnimQueueComplete(this.uncontrolled.idle).ScheduleGoTo(1f, this.uncontrolled.idle);
			this.uncontrolled.idle.Enter(delegate(RemoteWorkerSM.StatesInstance smi)
			{
				smi.master.Available = true;
			}).PlayAnim(RemoteWorkerConfig.IDLE_IN_DOCK_ANIM, KAnim.PlayMode.Loop).Transition(this.uncontrolled.working.recharge, GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.And(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.RequiresMaintnence), new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.DockIsOperational)), UpdateRate.SIM_1000ms);
			this.incapacitated.lost.Enter(delegate(RemoteWorkerSM.StatesInstance smi)
			{
				smi.Play("sos_pre", KAnim.PlayMode.Once);
				smi.Queue("sos_loop", KAnim.PlayMode.Loop);
				RemoteWorkerSM.States.ClearChore(smi);
			}).ToggleStatusItem(Db.Get().DuplicantStatusItems.UnreachableDock, null).Transition(this.incapacitated.lost_recovery, new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.CanReachDepot), UpdateRate.SIM_200ms).Transition(this.incapacitated.die, GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Not(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.HasHomeDepot)), UpdateRate.SIM_200ms);
			this.incapacitated.lost_recovery.PlayAnim("sos_pst").OnAnimQueueComplete(this.controlled);
			this.incapacitated.die.Enter(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State.Callback(RemoteWorkerSM.States.ClearChore)).PlayAnim("explode").OnAnimQueueComplete(this.incapacitated.explode).ToggleStatusItem(Db.Get().DuplicantStatusItems.NoHomeDock, null);
			this.incapacitated.explode.Enter(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State.Callback(RemoteWorkerSM.States.Explode));
		}

		// Token: 0x06007C97 RID: 31895 RVA: 0x000F672B File Offset: 0x000F492B
		public static bool IsNewWorker(RemoteWorkerSM.StatesInstance smi)
		{
			return smi.master.playNewWorker;
		}

		// Token: 0x06007C98 RID: 31896 RVA: 0x000F6738 File Offset: 0x000F4938
		public static void SetNextChore(RemoteWorkerSM.StatesInstance smi)
		{
			smi.master.StartNextChore();
		}

		// Token: 0x06007C99 RID: 31897 RVA: 0x000F6745 File Offset: 0x000F4945
		public static void ClearChore(RemoteWorkerSM.StatesInstance smi)
		{
			smi.master.driver.StopChore();
		}

		// Token: 0x06007C9A RID: 31898 RVA: 0x000F6757 File Offset: 0x000F4957
		public static bool HasChore(RemoteWorkerSM.StatesInstance smi)
		{
			return smi.master.driver.HasChore();
		}

		// Token: 0x06007C9B RID: 31899 RVA: 0x000F6769 File Offset: 0x000F4969
		public static bool HasChoreQueued(RemoteWorkerSM.StatesInstance smi)
		{
			return smi.master.HasChoreQueued();
		}

		// Token: 0x06007C9C RID: 31900 RVA: 0x0032E498 File Offset: 0x0032C698
		public static bool CanReachDepot(RemoteWorkerSM.StatesInstance smi)
		{
			int depotCell = RemoteWorkerSM.States.GetDepotCell(smi);
			return depotCell != Grid.InvalidCell && smi.master.GetComponent<Navigator>().CanReach(depotCell);
		}

		// Token: 0x06007C9D RID: 31901 RVA: 0x0032E4C8 File Offset: 0x0032C6C8
		public static int GetDepotCell(RemoteWorkerSM.StatesInstance smi)
		{
			RemoteWorkerDock homeDepot = smi.master.HomeDepot;
			if (homeDepot == null)
			{
				return Grid.InvalidCell;
			}
			return Grid.PosToCell(homeDepot);
		}

		// Token: 0x06007C9E RID: 31902 RVA: 0x000F6776 File Offset: 0x000F4976
		public static bool HasRemoteOperator(RemoteWorkerSM.StatesInstance smi)
		{
			return smi.master.ActivelyControlled;
		}

		// Token: 0x06007C9F RID: 31903 RVA: 0x000F6783 File Offset: 0x000F4983
		public static bool RequiresMaintnence(RemoteWorkerSM.StatesInstance smi)
		{
			return smi.master.RequiresMaintnence;
		}

		// Token: 0x06007CA0 RID: 31904 RVA: 0x000F6790 File Offset: 0x000F4990
		public static bool DockIsOperational(RemoteWorkerSM.StatesInstance smi)
		{
			return smi.master.HomeDepot != null && smi.master.HomeDepot.IsOperational;
		}

		// Token: 0x06007CA1 RID: 31905 RVA: 0x000F67B7 File Offset: 0x000F49B7
		public static bool HasHomeDepot(RemoteWorkerSM.StatesInstance smi)
		{
			return RemoteWorkerSM.States.GetDepotCell(smi) != Grid.InvalidCell;
		}

		// Token: 0x06007CA2 RID: 31906 RVA: 0x000F67C9 File Offset: 0x000F49C9
		public static void StopWork(RemoteWorkerSM.StatesInstance smi)
		{
			if (smi.master.driver.HasChore())
			{
				smi.master.driver.StopChore();
			}
		}

		// Token: 0x06007CA3 RID: 31907 RVA: 0x000F67ED File Offset: 0x000F49ED
		public static bool IsInsideDock(RemoteWorkerSM.StatesInstance smi)
		{
			return smi.master.Docked;
		}

		// Token: 0x06007CA4 RID: 31908 RVA: 0x0032E4F8 File Offset: 0x0032C6F8
		public static void Explode(RemoteWorkerSM.StatesInstance smi)
		{
			Game.Instance.SpawnFX(SpawnFXHashes.MeteorImpactDust, smi.master.transform.position, 0f);
			PrimaryElement component = smi.master.GetComponent<PrimaryElement>();
			component.Element.substance.SpawnResource(Grid.CellToPosCCC(Grid.PosToCell(smi.master.gameObject), Grid.SceneLayer.Ore), 42f, component.Temperature, component.DiseaseIdx, component.DiseaseCount, false, false, false);
			Util.KDestroyGameObject(smi.master.gameObject);
		}

		// Token: 0x06007CA5 RID: 31909 RVA: 0x000F67FA File Offset: 0x000F49FA
		public static void TickResources(RemoteWorkerSM.StatesInstance smi, float dt)
		{
			if (dt > 0f)
			{
				smi.master.TickResources(dt);
			}
		}

		// Token: 0x04005DCF RID: 24015
		public RemoteWorkerSM.States.ControlledStates controlled;

		// Token: 0x04005DD0 RID: 24016
		public RemoteWorkerSM.States.UncontrolledStates uncontrolled;

		// Token: 0x04005DD1 RID: 24017
		public RemoteWorkerSM.States.IncapacitatedStates incapacitated;

		// Token: 0x04005DD2 RID: 24018
		public StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.TargetParameter homedock;

		// Token: 0x020017AD RID: 6061
		public class ControlledStates : GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State
		{
			// Token: 0x04005DD3 RID: 24019
			public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State exit_dock;

			// Token: 0x04005DD4 RID: 24020
			public RemoteWorkerSM.States.ControlledStates.WorkingStates working;

			// Token: 0x04005DD5 RID: 24021
			public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State no_work;

			// Token: 0x020017AE RID: 6062
			public class WorkingStates : GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State
			{
				// Token: 0x04005DD6 RID: 24022
				public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State find_work;

				// Token: 0x04005DD7 RID: 24023
				public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State do_work;
			}
		}

		// Token: 0x020017AF RID: 6063
		public class UncontrolledStates : GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State
		{
			// Token: 0x04005DD8 RID: 24024
			public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State approach_dock;

			// Token: 0x04005DD9 RID: 24025
			public RemoteWorkerSM.States.UncontrolledStates.WorkingDockStates working;

			// Token: 0x04005DDA RID: 24026
			public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State idle;

			// Token: 0x020017B0 RID: 6064
			public class WorkingDockStates : GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State
			{
				// Token: 0x04005DDB RID: 24027
				public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State new_worker;

				// Token: 0x04005DDC RID: 24028
				public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State enter;

				// Token: 0x04005DDD RID: 24029
				public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State recharge;

				// Token: 0x04005DDE RID: 24030
				public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State recharge_pst;

				// Token: 0x04005DDF RID: 24031
				public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State drain_gunk;

				// Token: 0x04005DE0 RID: 24032
				public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State drain_gunk_pst;

				// Token: 0x04005DE1 RID: 24033
				public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State fill_oil;

				// Token: 0x04005DE2 RID: 24034
				public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State fill_oil_pst;
			}
		}

		// Token: 0x020017B1 RID: 6065
		public class IncapacitatedStates : GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State
		{
			// Token: 0x04005DE3 RID: 24035
			public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State lost;

			// Token: 0x04005DE4 RID: 24036
			public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State lost_recovery;

			// Token: 0x04005DE5 RID: 24037
			public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State die;

			// Token: 0x04005DE6 RID: 24038
			public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State explode;
		}
	}
}
