using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000F77 RID: 3959
public class RanchStation : GameStateMachine<RanchStation, RanchStation.Instance, IStateMachineTarget, RanchStation.Def>
{
	// Token: 0x06004F7C RID: 20348 RVA: 0x0027998C File Offset: 0x00277B8C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.Operational;
		this.Unoperational.TagTransition(GameTags.Operational, this.Operational, false);
		this.Operational.TagTransition(GameTags.Operational, this.Unoperational, true).ToggleChore((RanchStation.Instance smi) => smi.CreateChore(), new Action<RanchStation.Instance, Chore>(RanchStation.SetRemoteChore), this.Unoperational, this.Unoperational).Update("FindRanachable", delegate(RanchStation.Instance smi, float dt)
		{
			smi.FindRanchable(null);
		}, UpdateRate.SIM_200ms, false);
	}

	// Token: 0x06004F7D RID: 20349 RVA: 0x000D82D0 File Offset: 0x000D64D0
	private static void SetRemoteChore(RanchStation.Instance smi, Chore chore)
	{
		smi.remoteChore.SetChore(chore);
	}

	// Token: 0x04003801 RID: 14337
	public StateMachine<RanchStation, RanchStation.Instance, IStateMachineTarget, RanchStation.Def>.BoolParameter RancherIsReady;

	// Token: 0x04003802 RID: 14338
	public GameStateMachine<RanchStation, RanchStation.Instance, IStateMachineTarget, RanchStation.Def>.State Unoperational;

	// Token: 0x04003803 RID: 14339
	public RanchStation.OperationalState Operational;

	// Token: 0x02000F78 RID: 3960
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04003804 RID: 14340
		public Func<GameObject, RanchStation.Instance, bool> IsCritterEligibleToBeRanchedCb;

		// Token: 0x04003805 RID: 14341
		public Action<GameObject, WorkerBase> OnRanchCompleteCb;

		// Token: 0x04003806 RID: 14342
		public Action<GameObject, float, Workable> OnRanchWorkTick;

		// Token: 0x04003807 RID: 14343
		public HashedString RanchedPreAnim = "idle_loop";

		// Token: 0x04003808 RID: 14344
		public HashedString RanchedLoopAnim = "idle_loop";

		// Token: 0x04003809 RID: 14345
		public HashedString RanchedPstAnim = "idle_loop";

		// Token: 0x0400380A RID: 14346
		public HashedString RanchedAbortAnim = "idle_loop";

		// Token: 0x0400380B RID: 14347
		public HashedString RancherInteractAnim = "anim_interacts_rancherstation_kanim";

		// Token: 0x0400380C RID: 14348
		public StatusItem RanchingStatusItem = Db.Get().DuplicantStatusItems.Ranching;

		// Token: 0x0400380D RID: 14349
		public StatusItem CreatureRanchingStatusItem = Db.Get().CreatureStatusItems.GettingRanched;

		// Token: 0x0400380E RID: 14350
		public float WorkTime = 12f;

		// Token: 0x0400380F RID: 14351
		public Func<RanchStation.Instance, int> GetTargetRanchCell = (RanchStation.Instance smi) => Grid.PosToCell(smi);
	}

	// Token: 0x02000F7A RID: 3962
	public class OperationalState : GameStateMachine<RanchStation, RanchStation.Instance, IStateMachineTarget, RanchStation.Def>.State
	{
	}

	// Token: 0x02000F7B RID: 3963
	public new class Instance : GameStateMachine<RanchStation, RanchStation.Instance, IStateMachineTarget, RanchStation.Def>.GameInstance
	{
		// Token: 0x17000468 RID: 1128
		// (get) Token: 0x06004F84 RID: 20356 RVA: 0x000D8302 File Offset: 0x000D6502
		public RanchedStates.Instance ActiveRanchable
		{
			get
			{
				return this.activeRanchable;
			}
		}

		// Token: 0x17000469 RID: 1129
		// (get) Token: 0x06004F85 RID: 20357 RVA: 0x000D830A File Offset: 0x000D650A
		private bool isCritterAvailableForRanching
		{
			get
			{
				return this.targetRanchables.Count > 0;
			}
		}

		// Token: 0x1700046A RID: 1130
		// (get) Token: 0x06004F86 RID: 20358 RVA: 0x000D831A File Offset: 0x000D651A
		public bool IsCritterAvailableForRanching
		{
			get
			{
				this.ValidateTargetRanchables();
				return this.isCritterAvailableForRanching;
			}
		}

		// Token: 0x1700046B RID: 1131
		// (get) Token: 0x06004F87 RID: 20359 RVA: 0x000D8328 File Offset: 0x000D6528
		public bool HasRancher
		{
			get
			{
				return this.rancher != null;
			}
		}

		// Token: 0x1700046C RID: 1132
		// (get) Token: 0x06004F88 RID: 20360 RVA: 0x000D8336 File Offset: 0x000D6536
		public bool IsRancherReady
		{
			get
			{
				return base.sm.RancherIsReady.Get(this);
			}
		}

		// Token: 0x1700046D RID: 1133
		// (get) Token: 0x06004F89 RID: 20361 RVA: 0x000D8349 File Offset: 0x000D6549
		public Extents StationExtents
		{
			get
			{
				return this.station.GetExtents();
			}
		}

		// Token: 0x06004F8A RID: 20362 RVA: 0x000D8356 File Offset: 0x000D6556
		public int GetRanchNavTarget()
		{
			return base.def.GetTargetRanchCell(this);
		}

		// Token: 0x06004F8B RID: 20363 RVA: 0x000D8369 File Offset: 0x000D6569
		public Instance(IStateMachineTarget master, RanchStation.Def def) : base(master, def)
		{
			base.gameObject.AddOrGet<RancherChore.RancherWorkable>();
			this.station = base.GetComponent<BuildingComplete>();
		}

		// Token: 0x06004F8C RID: 20364 RVA: 0x00279AF8 File Offset: 0x00277CF8
		public Chore CreateChore()
		{
			RancherChore rancherChore = new RancherChore(base.GetComponent<KPrefabID>());
			StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.TargetParameter targetParameter = rancherChore.smi.sm.rancher;
			StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.Parameter<GameObject>.Context context = targetParameter.GetContext(rancherChore.smi);
			context.onDirty = (Action<RancherChore.RancherChoreStates.Instance>)Delegate.Combine(context.onDirty, new Action<RancherChore.RancherChoreStates.Instance>(this.OnRancherChanged));
			this.rancher = targetParameter.Get<WorkerBase>(rancherChore.smi);
			return rancherChore;
		}

		// Token: 0x06004F8D RID: 20365 RVA: 0x000D8356 File Offset: 0x000D6556
		public int GetTargetRanchCell()
		{
			return base.def.GetTargetRanchCell(this);
		}

		// Token: 0x06004F8E RID: 20366 RVA: 0x00279B64 File Offset: 0x00277D64
		public override void StartSM()
		{
			base.StartSM();
			base.Subscribe(144050788, new Action<object>(this.OnRoomUpdated));
			CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(this.GetTargetRanchCell());
			if (cavityForCell != null && cavityForCell.room != null)
			{
				this.OnRoomUpdated(cavityForCell.room);
			}
		}

		// Token: 0x06004F8F RID: 20367 RVA: 0x000D8396 File Offset: 0x000D6596
		public override void StopSM(string reason)
		{
			base.StopSM(reason);
			base.Unsubscribe(144050788, new Action<object>(this.OnRoomUpdated));
		}

		// Token: 0x06004F90 RID: 20368 RVA: 0x000D83B6 File Offset: 0x000D65B6
		private void OnRoomUpdated(object data)
		{
			if (data == null)
			{
				return;
			}
			this.ranch = (data as Room);
			if (this.ranch.roomType != Db.Get().RoomTypes.CreaturePen)
			{
				this.TriggerRanchStationNoLongerAvailable();
				this.ranch = null;
			}
		}

		// Token: 0x06004F91 RID: 20369 RVA: 0x000D83F1 File Offset: 0x000D65F1
		private void OnRancherChanged(RancherChore.RancherChoreStates.Instance choreInstance)
		{
			this.rancher = choreInstance.sm.rancher.Get<WorkerBase>(choreInstance);
			this.TriggerRanchStationNoLongerAvailable();
		}

		// Token: 0x06004F92 RID: 20370 RVA: 0x000D8410 File Offset: 0x000D6610
		public bool TryGetRanched(RanchedStates.Instance ranchable)
		{
			return this.activeRanchable == null || this.activeRanchable == ranchable;
		}

		// Token: 0x06004F93 RID: 20371 RVA: 0x000D8425 File Offset: 0x000D6625
		public void MessageCreatureArrived(RanchedStates.Instance critter)
		{
			this.activeRanchable = critter;
			base.sm.RancherIsReady.Set(false, this, false);
			base.Trigger(-1357116271, null);
		}

		// Token: 0x06004F94 RID: 20372 RVA: 0x000D844E File Offset: 0x000D664E
		public void MessageRancherReady()
		{
			base.sm.RancherIsReady.Set(true, base.smi, false);
			this.MessageRanchables(GameHashes.RancherReadyAtRanchStation);
		}

		// Token: 0x06004F95 RID: 20373 RVA: 0x00279BBC File Offset: 0x00277DBC
		private bool CanRanchableBeRanchedAtRanchStation(RanchableMonitor.Instance ranchable)
		{
			bool flag = !ranchable.IsNullOrStopped();
			if (flag && ranchable.TargetRanchStation != null && ranchable.TargetRanchStation != this)
			{
				flag = (!ranchable.TargetRanchStation.IsRunning() || !ranchable.TargetRanchStation.HasRancher);
			}
			flag = (flag && base.def.IsCritterEligibleToBeRanchedCb(ranchable.gameObject, this));
			flag = (flag && ranchable.ChoreConsumer.IsChoreEqualOrAboveCurrentChorePriority<RanchedStates>());
			if (flag)
			{
				int cell = Grid.PosToCell(ranchable.transform.GetPosition());
				CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(cell);
				if (cavityForCell == null || this.ranch == null || cavityForCell != this.ranch.cavity)
				{
					flag = false;
				}
				else
				{
					int cell2 = this.GetRanchNavTarget();
					if (ranchable.HasTag(GameTags.Creatures.Flyer))
					{
						cell2 = Grid.CellAbove(cell2);
					}
					flag = (ranchable.NavComponent.GetNavigationCost(cell2) != -1);
				}
			}
			return flag;
		}

		// Token: 0x06004F96 RID: 20374 RVA: 0x00279CA8 File Offset: 0x00277EA8
		public void ValidateTargetRanchables()
		{
			if (!this.HasRancher)
			{
				return;
			}
			foreach (RanchableMonitor.Instance instance in this.targetRanchables.ToArray())
			{
				if (instance.States == null || !this.CanRanchableBeRanchedAtRanchStation(instance))
				{
					this.Abandon(instance);
				}
			}
		}

		// Token: 0x06004F97 RID: 20375 RVA: 0x00279CF4 File Offset: 0x00277EF4
		public void FindRanchable(object _ = null)
		{
			if (this.ranch == null)
			{
				return;
			}
			this.ValidateTargetRanchables();
			if (this.targetRanchables.Count == 2)
			{
				return;
			}
			List<KPrefabID> creatures = this.ranch.cavity.creatures;
			if (this.HasRancher && !this.isCritterAvailableForRanching && creatures.Count == 0)
			{
				this.TryNotifyEmptyRanch();
			}
			for (int i = 0; i < creatures.Count; i++)
			{
				KPrefabID kprefabID = creatures[i];
				if (!(kprefabID == null))
				{
					RanchableMonitor.Instance smi = kprefabID.GetSMI<RanchableMonitor.Instance>();
					if (!this.targetRanchables.Contains(smi) && this.CanRanchableBeRanchedAtRanchStation(smi) && smi != null)
					{
						smi.States.SetRanchStation(this);
						this.targetRanchables.Add(smi);
						return;
					}
				}
			}
		}

		// Token: 0x06004F98 RID: 20376 RVA: 0x000D8474 File Offset: 0x000D6674
		public Option<CavityInfo> GetCavityInfo()
		{
			if (this.ranch.IsNullOrDestroyed())
			{
				return Option.None;
			}
			return this.ranch.cavity;
		}

		// Token: 0x06004F99 RID: 20377 RVA: 0x00279DAC File Offset: 0x00277FAC
		public void RanchCreature()
		{
			if (this.activeRanchable.IsNullOrStopped())
			{
				return;
			}
			global::Debug.Assert(this.activeRanchable != null, "targetRanchable was null");
			global::Debug.Assert(this.activeRanchable.GetMaster() != null, "GetMaster was null");
			global::Debug.Assert(base.def != null, "def was null");
			global::Debug.Assert(base.def.OnRanchCompleteCb != null, "onRanchCompleteCb cb was null");
			base.def.OnRanchCompleteCb(this.activeRanchable.gameObject, this.rancher);
			this.targetRanchables.Remove(this.activeRanchable.Monitor);
			this.activeRanchable.Trigger(1827504087, null);
			this.activeRanchable = null;
			this.FindRanchable(null);
		}

		// Token: 0x06004F9A RID: 20378 RVA: 0x00279E74 File Offset: 0x00278074
		public void TriggerRanchStationNoLongerAvailable()
		{
			for (int i = this.targetRanchables.Count - 1; i >= 0; i--)
			{
				RanchableMonitor.Instance instance = this.targetRanchables[i];
				if (instance.IsNullOrStopped() || instance.States.IsNullOrStopped())
				{
					this.targetRanchables.RemoveAt(i);
				}
				else
				{
					this.targetRanchables.Remove(instance);
					instance.Trigger(1689625967, null);
				}
			}
			global::Debug.Assert(this.targetRanchables.Count == 0, "targetRanchables is not empty");
			this.activeRanchable = null;
			base.sm.RancherIsReady.Set(false, this, false);
		}

		// Token: 0x06004F9B RID: 20379 RVA: 0x00279F18 File Offset: 0x00278118
		public void MessageRanchables(GameHashes hash)
		{
			for (int i = 0; i < this.targetRanchables.Count; i++)
			{
				RanchableMonitor.Instance instance = this.targetRanchables[i];
				if (!instance.IsNullOrStopped())
				{
					Game.BrainScheduler.PrioritizeBrain(instance.GetComponent<CreatureBrain>());
					if (!instance.States.IsNullOrStopped())
					{
						instance.Trigger((int)hash, null);
					}
				}
			}
		}

		// Token: 0x06004F9C RID: 20380 RVA: 0x00279F78 File Offset: 0x00278178
		public void Abandon(RanchableMonitor.Instance critter)
		{
			if (critter == null)
			{
				global::Debug.LogWarning("Null critter trying to abandon ranch station");
				this.targetRanchables.Remove(critter);
				return;
			}
			critter.TargetRanchStation = null;
			if (this.targetRanchables.Remove(critter))
			{
				if (critter.States == null)
				{
					return;
				}
				bool flag = !this.isCritterAvailableForRanching;
				if (critter.States == this.activeRanchable)
				{
					flag = true;
					this.activeRanchable = null;
				}
				if (flag)
				{
					this.TryNotifyEmptyRanch();
				}
			}
		}

		// Token: 0x06004F9D RID: 20381 RVA: 0x000D849E File Offset: 0x000D669E
		private void TryNotifyEmptyRanch()
		{
			if (!this.HasRancher)
			{
				return;
			}
			this.rancher.Trigger(-364750427, null);
		}

		// Token: 0x06004F9E RID: 20382 RVA: 0x000D84BA File Offset: 0x000D66BA
		public bool IsCritterInQueue(RanchableMonitor.Instance critter)
		{
			return this.targetRanchables.Contains(critter);
		}

		// Token: 0x06004F9F RID: 20383 RVA: 0x000D84C8 File Offset: 0x000D66C8
		public List<RanchableMonitor.Instance> DEBUG_GetTargetRanchables()
		{
			return this.targetRanchables;
		}

		// Token: 0x04003812 RID: 14354
		[MyCmpAdd]
		public ManuallySetRemoteWorkTargetComponent remoteChore;

		// Token: 0x04003813 RID: 14355
		private const int QUEUE_SIZE = 2;

		// Token: 0x04003814 RID: 14356
		private List<RanchableMonitor.Instance> targetRanchables = new List<RanchableMonitor.Instance>();

		// Token: 0x04003815 RID: 14357
		private RanchedStates.Instance activeRanchable;

		// Token: 0x04003816 RID: 14358
		private Room ranch;

		// Token: 0x04003817 RID: 14359
		private WorkerBase rancher;

		// Token: 0x04003818 RID: 14360
		private BuildingComplete station;
	}
}
