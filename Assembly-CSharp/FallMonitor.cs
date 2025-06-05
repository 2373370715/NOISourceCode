using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A80 RID: 2688
public class FallMonitor : GameStateMachine<FallMonitor, FallMonitor.Instance>
{
	// Token: 0x060030E1 RID: 12513 RVA: 0x0020B364 File Offset: 0x00209564
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.standing;
		this.root.TagTransition(GameTags.Stored, this.instorage, false).Update("CheckLanded", delegate(FallMonitor.Instance smi, float dt)
		{
			smi.UpdateFalling();
		}, UpdateRate.SIM_33ms, true);
		this.standing.ParamTransition<bool>(this.isEntombed, this.entombed, GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.IsTrue).ParamTransition<bool>(this.isFalling, this.falling_pre, GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.IsTrue);
		this.falling_pre.Enter("StopNavigator", delegate(FallMonitor.Instance smi)
		{
			smi.GetComponent<Navigator>().Stop(false, true);
		}).Enter("AttemptInitialRecovery", delegate(FallMonitor.Instance smi)
		{
			smi.AttemptInitialRecovery();
		}).GoTo(this.falling).ToggleBrain("falling_pre");
		this.falling.ToggleBrain("falling").PlayAnim("fall_pre").QueueAnim("fall_loop", true, null).ParamTransition<bool>(this.isEntombed, this.entombed, GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.IsTrue).Transition(this.recoverladder, (FallMonitor.Instance smi) => smi.CanRecoverToLadder(), UpdateRate.SIM_33ms).Transition(this.recoverpole, (FallMonitor.Instance smi) => smi.CanRecoverToPole(), UpdateRate.SIM_33ms).ToggleGravity(this.landfloor);
		this.recoverinitialfall.ToggleBrain("recoverinitialfall").Enter("Recover", delegate(FallMonitor.Instance smi)
		{
			smi.Recover();
		}).EventTransition(GameHashes.DestinationReached, this.standing, null).EventTransition(GameHashes.NavigationFailed, this.standing, null).Exit(delegate(FallMonitor.Instance smi)
		{
			smi.RecoverEmote();
		});
		this.landfloor.Enter("Land", delegate(FallMonitor.Instance smi)
		{
			smi.LandFloor();
		}).GoTo(this.standing);
		this.recoverladder.ToggleBrain("recoverladder").PlayAnim("floor_ladder_0_0").Enter("MountLadder", delegate(FallMonitor.Instance smi)
		{
			smi.MountLadder();
		}).OnAnimQueueComplete(this.standing);
		this.recoverpole.ToggleBrain("recoverpole").PlayAnim("floor_pole_0_0").Enter("MountPole", delegate(FallMonitor.Instance smi)
		{
			smi.MountPole();
		}).OnAnimQueueComplete(this.standing);
		this.instorage.TagTransition(GameTags.Stored, this.standing, true);
		this.entombed.DefaultState(this.entombed.recovering);
		this.entombed.recovering.Enter("TryEntombedEscape", delegate(FallMonitor.Instance smi)
		{
			smi.TryEntombedEscape();
		});
		this.entombed.stuck.Enter("StopNavigator", delegate(FallMonitor.Instance smi)
		{
			smi.GetComponent<Navigator>().Stop(false, true);
		}).ToggleChore((FallMonitor.Instance smi) => new EntombedChore(smi.master, smi.entombedAnimOverride), this.standing).ParamTransition<bool>(this.isEntombed, this.standing, GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.IsFalse);
	}

	// Token: 0x0400219C RID: 8604
	public GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State standing;

	// Token: 0x0400219D RID: 8605
	public GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State falling_pre;

	// Token: 0x0400219E RID: 8606
	public GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State falling;

	// Token: 0x0400219F RID: 8607
	public FallMonitor.EntombedStates entombed;

	// Token: 0x040021A0 RID: 8608
	public GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State recoverladder;

	// Token: 0x040021A1 RID: 8609
	public GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State recoverpole;

	// Token: 0x040021A2 RID: 8610
	public GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State recoverinitialfall;

	// Token: 0x040021A3 RID: 8611
	public GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State landfloor;

	// Token: 0x040021A4 RID: 8612
	public GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State instorage;

	// Token: 0x040021A5 RID: 8613
	public StateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.BoolParameter isEntombed;

	// Token: 0x040021A6 RID: 8614
	public StateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.BoolParameter isFalling;

	// Token: 0x02000A81 RID: 2689
	public class EntombedStates : GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x040021A7 RID: 8615
		public GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State recovering;

		// Token: 0x040021A8 RID: 8616
		public GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State stuck;
	}

	// Token: 0x02000A82 RID: 2690
	public new class Instance : GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x060030E4 RID: 12516 RVA: 0x0020B730 File Offset: 0x00209930
		public Instance(IStateMachineTarget master, bool shouldPlayEmotes, string entombedAnimOverride = null) : base(master)
		{
			this.navigator = base.GetComponent<Navigator>();
			this.shouldPlayEmotes = shouldPlayEmotes;
			this.entombedAnimOverride = entombedAnimOverride;
			Pathfinding.Instance.FlushNavGridsOnLoad();
			base.Subscribe(915392638, new Action<object>(this.OnCellChanged));
			base.Subscribe(1027377649, new Action<object>(this.OnMovementStateChanged));
			base.Subscribe(387220196, new Action<object>(this.OnDestinationReached));
		}

		// Token: 0x060030E5 RID: 12517 RVA: 0x0020B830 File Offset: 0x00209A30
		private void OnDestinationReached(object data)
		{
			int item = Grid.PosToCell(base.transform.GetPosition());
			if (!this.safeCells.Contains(item))
			{
				this.safeCells.Add(item);
				if (this.safeCells.Count > this.MAX_CELLS_TRACKED)
				{
					this.safeCells.RemoveAt(0);
				}
			}
		}

		// Token: 0x060030E6 RID: 12518 RVA: 0x0020B888 File Offset: 0x00209A88
		private void OnMovementStateChanged(object data)
		{
			if ((GameHashes)data == GameHashes.ObjectMovementWakeUp)
			{
				int item = Grid.PosToCell(base.transform.GetPosition());
				if (!this.safeCells.Contains(item))
				{
					this.safeCells.Add(item);
					if (this.safeCells.Count > this.MAX_CELLS_TRACKED)
					{
						this.safeCells.RemoveAt(0);
					}
				}
			}
		}

		// Token: 0x060030E7 RID: 12519 RVA: 0x0020B8EC File Offset: 0x00209AEC
		private void OnCellChanged(object data)
		{
			int item = (int)data;
			if (!this.safeCells.Contains(item))
			{
				this.safeCells.Add(item);
				if (this.safeCells.Count > this.MAX_CELLS_TRACKED)
				{
					this.safeCells.RemoveAt(0);
				}
			}
		}

		// Token: 0x060030E8 RID: 12520 RVA: 0x0020B93C File Offset: 0x00209B3C
		public void Recover()
		{
			int cell = Grid.PosToCell(this.navigator);
			foreach (NavGrid.Transition transition in this.navigator.NavGrid.transitions)
			{
				if (transition.isEscape && this.navigator.CurrentNavType == transition.start)
				{
					int num = transition.IsValid(cell, this.navigator.NavGrid.NavTable);
					if (Grid.InvalidCell != num)
					{
						Vector2I vector2I = Grid.CellToXY(cell);
						Vector2I vector2I2 = Grid.CellToXY(num);
						this.flipRecoverEmote = (vector2I2.x < vector2I.x);
						this.navigator.BeginTransition(transition);
						return;
					}
				}
			}
		}

		// Token: 0x060030E9 RID: 12521 RVA: 0x0020B9F4 File Offset: 0x00209BF4
		public void RecoverEmote()
		{
			if (!this.shouldPlayEmotes)
			{
				return;
			}
			if (UnityEngine.Random.Range(0, 9) == 8)
			{
				new EmoteChore(base.master.GetComponent<ChoreProvider>(), Db.Get().ChoreTypes.EmoteHighPriority, Db.Get().Emotes.Minion.CloseCall_Fall, KAnim.PlayMode.Once, 1, this.flipRecoverEmote);
			}
		}

		// Token: 0x060030EA RID: 12522 RVA: 0x000C43CC File Offset: 0x000C25CC
		public void LandFloor()
		{
			this.navigator.SetCurrentNavType(NavType.Floor);
			base.GetComponent<Transform>().SetPosition(Grid.CellToPosCBC(Grid.PosToCell(base.GetComponent<Transform>().GetPosition()), Grid.SceneLayer.Move));
		}

		// Token: 0x060030EB RID: 12523 RVA: 0x0020BA54 File Offset: 0x00209C54
		public void AttemptInitialRecovery()
		{
			if (base.gameObject.HasTag(GameTags.Incapacitated))
			{
				return;
			}
			int cell = Grid.PosToCell(this.navigator);
			foreach (NavGrid.Transition transition in this.navigator.NavGrid.transitions)
			{
				if (transition.isEscape && this.navigator.CurrentNavType == transition.start)
				{
					int num = transition.IsValid(cell, this.navigator.NavGrid.NavTable);
					if (Grid.InvalidCell != num)
					{
						base.smi.GoTo(base.smi.sm.recoverinitialfall);
						return;
					}
				}
			}
		}

		// Token: 0x060030EC RID: 12524 RVA: 0x0020BB04 File Offset: 0x00209D04
		public bool CanRecoverToLadder()
		{
			int cell = Grid.PosToCell(base.master.transform.GetPosition());
			return this.navigator.NavGrid.NavTable.IsValid(cell, NavType.Ladder) && !base.gameObject.HasTag(GameTags.Incapacitated);
		}

		// Token: 0x060030ED RID: 12525 RVA: 0x000C43FD File Offset: 0x000C25FD
		public void MountLadder()
		{
			this.navigator.SetCurrentNavType(NavType.Ladder);
			base.GetComponent<Transform>().SetPosition(Grid.CellToPosCBC(Grid.PosToCell(base.GetComponent<Transform>().GetPosition()), Grid.SceneLayer.Move));
		}

		// Token: 0x060030EE RID: 12526 RVA: 0x0020BB58 File Offset: 0x00209D58
		public bool CanRecoverToPole()
		{
			int cell = Grid.PosToCell(base.master.transform.GetPosition());
			return this.navigator.NavGrid.NavTable.IsValid(cell, NavType.Pole) && !base.gameObject.HasTag(GameTags.Incapacitated);
		}

		// Token: 0x060030EF RID: 12527 RVA: 0x000C442E File Offset: 0x000C262E
		public void MountPole()
		{
			this.navigator.SetCurrentNavType(NavType.Pole);
			base.GetComponent<Transform>().SetPosition(Grid.CellToPosCBC(Grid.PosToCell(base.GetComponent<Transform>().GetPosition()), Grid.SceneLayer.Move));
		}

		// Token: 0x060030F0 RID: 12528 RVA: 0x0020BBAC File Offset: 0x00209DAC
		public void UpdateFalling()
		{
			bool value = false;
			bool flag = false;
			if (!this.navigator.IsMoving() && this.navigator.CurrentNavType != NavType.Tube)
			{
				int num = Grid.PosToCell(base.transform.GetPosition());
				int num2 = Grid.CellAbove(num);
				bool flag2 = Grid.IsValidCell(num);
				bool flag3 = Grid.IsValidCell(num2);
				bool flag4 = this.IsValidNavCell(num);
				flag4 = (flag4 && (!base.gameObject.HasTag(GameTags.Incapacitated) || (this.navigator.CurrentNavType != NavType.Ladder && this.navigator.CurrentNavType != NavType.Pole)));
				flag = ((!flag4 && flag2 && Grid.Solid[num] && !Grid.DupePassable[num]) || (flag3 && Grid.Solid[num2] && !Grid.DupePassable[num2]) || (flag2 && Grid.DupeImpassable[num]) || (flag3 && Grid.DupeImpassable[num2]));
				value = (!flag4 && !flag);
				if ((!flag2 && flag3) || (flag3 && Grid.WorldIdx[num] != Grid.WorldIdx[num2] && Grid.IsWorldValidCell(num2)))
				{
					this.TeleportInWorld(num);
				}
			}
			base.sm.isFalling.Set(value, base.smi, false);
			base.sm.isEntombed.Set(flag, base.smi, false);
		}

		// Token: 0x060030F1 RID: 12529 RVA: 0x0020BD2C File Offset: 0x00209F2C
		private void TeleportInWorld(int cell)
		{
			int num = Grid.CellAbove(cell);
			WorldContainer world = ClusterManager.Instance.GetWorld((int)Grid.WorldIdx[num]);
			if (world != null)
			{
				int safeCell = world.GetSafeCell();
				global::Debug.Log(string.Format("Teleporting {0} to {1}", this.navigator.name, safeCell));
				this.MoveToCell(safeCell, false);
				return;
			}
			global::Debug.LogError(string.Format("Unable to teleport {0} stuck on {1}", this.navigator.name, cell));
		}

		// Token: 0x060030F2 RID: 12530 RVA: 0x000C445F File Offset: 0x000C265F
		private bool IsValidNavCell(int cell)
		{
			return this.navigator.NavGrid.NavTable.IsValid(cell, this.navigator.CurrentNavType) && !Grid.DupeImpassable[cell];
		}

		// Token: 0x060030F3 RID: 12531 RVA: 0x0020BDAC File Offset: 0x00209FAC
		public void TryEntombedEscape()
		{
			int num = Grid.PosToCell(base.transform.GetPosition());
			int backCell = base.GetComponent<Facing>().GetBackCell();
			int num2 = Grid.CellAbove(backCell);
			int num3 = Grid.CellBelow(backCell);
			foreach (int num4 in new int[]
			{
				backCell,
				num2,
				num3
			})
			{
				if (this.IsValidNavCell(num4) && !Grid.HasDoor[num4])
				{
					this.MoveToCell(num4, false);
					return;
				}
			}
			int cell = Grid.PosToCell(base.transform.GetPosition());
			foreach (CellOffset offset in this.entombedEscapeOffsets)
			{
				if (Grid.IsCellOffsetValid(cell, offset))
				{
					int num5 = Grid.OffsetCell(cell, offset);
					if (this.IsValidNavCell(num5) && !Grid.HasDoor[num5])
					{
						this.MoveToCell(num5, false);
						return;
					}
				}
			}
			for (int k = this.safeCells.Count - 1; k >= 0; k--)
			{
				int num6 = this.safeCells[k];
				if (num6 != num && this.IsValidNavCell(num6) && !Grid.HasDoor[num6])
				{
					this.MoveToCell(num6, false);
					return;
				}
			}
			foreach (CellOffset offset2 in this.entombedEscapeOffsets)
			{
				if (Grid.IsCellOffsetValid(cell, offset2))
				{
					int num7 = Grid.OffsetCell(cell, offset2);
					int num8 = Grid.CellAbove(num7);
					if (Grid.IsValidCell(num8) && !Grid.Solid[num7] && !Grid.Solid[num8] && !Grid.DupeImpassable[num7] && !Grid.DupeImpassable[num8] && !Grid.HasDoor[num7] && !Grid.HasDoor[num8])
					{
						this.MoveToCell(num7, true);
						return;
					}
				}
			}
			this.GoTo(base.sm.entombed.stuck);
		}

		// Token: 0x060030F4 RID: 12532 RVA: 0x0020BFC0 File Offset: 0x0020A1C0
		private void MoveToCell(int cell, bool forceFloorNav = false)
		{
			base.transform.SetPosition(Grid.CellToPosCBC(cell, Grid.SceneLayer.Move));
			base.transform.GetComponent<Navigator>().Stop(false, true);
			if (base.gameObject.HasTag(GameTags.Incapacitated) || forceFloorNav)
			{
				base.transform.GetComponent<Navigator>().SetCurrentNavType(NavType.Floor);
			}
			this.UpdateFalling();
			if (base.sm.isEntombed.Get(base.smi))
			{
				this.GoTo(base.sm.entombed.stuck);
				return;
			}
			this.GoTo(base.sm.standing);
		}

		// Token: 0x040021A9 RID: 8617
		private CellOffset[] entombedEscapeOffsets = new CellOffset[]
		{
			new CellOffset(0, 1),
			new CellOffset(1, 0),
			new CellOffset(-1, 0),
			new CellOffset(1, 1),
			new CellOffset(-1, 1),
			new CellOffset(1, -1),
			new CellOffset(-1, -1)
		};

		// Token: 0x040021AA RID: 8618
		private Navigator navigator;

		// Token: 0x040021AB RID: 8619
		private bool shouldPlayEmotes;

		// Token: 0x040021AC RID: 8620
		public string entombedAnimOverride;

		// Token: 0x040021AD RID: 8621
		private List<int> safeCells = new List<int>();

		// Token: 0x040021AE RID: 8622
		private int MAX_CELLS_TRACKED = 3;

		// Token: 0x040021AF RID: 8623
		private bool flipRecoverEmote;
	}
}
