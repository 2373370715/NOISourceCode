using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020006EE RID: 1774
public class MournChore : Chore<MournChore.StatesInstance>
{
	// Token: 0x06001F81 RID: 8065 RVA: 0x001C4F54 File Offset: 0x001C3154
	private static int GetStandableCell(int cell, Navigator navigator)
	{
		foreach (CellOffset offset in MournChore.ValidStandingOffsets)
		{
			if (Grid.IsCellOffsetValid(cell, offset))
			{
				int num = Grid.OffsetCell(cell, offset);
				if (!Grid.Reserved[num] && navigator.NavGrid.NavTable.IsValid(num, NavType.Floor) && navigator.GetNavigationCost(num) != -1)
				{
					return num;
				}
			}
		}
		return -1;
	}

	// Token: 0x06001F82 RID: 8066 RVA: 0x001C4FBC File Offset: 0x001C31BC
	private static KAnimFile GetAnimFileName(MournChore.StatesInstance smi)
	{
		string s = "anim_react_mourning_kanim";
		GameObject gameObject = smi.sm.mourner.Get(smi);
		if (gameObject == null)
		{
			return Assets.GetAnim(s);
		}
		MinionIdentity component = gameObject.GetComponent<MinionIdentity>();
		if (component == null)
		{
			return Assets.GetAnim(s);
		}
		if (component.model == BionicMinionConfig.MODEL)
		{
			return Assets.GetAnim("anim_bionic_react_mourning_kanim");
		}
		return Assets.GetAnim(s);
	}

	// Token: 0x06001F83 RID: 8067 RVA: 0x001C5040 File Offset: 0x001C3240
	public MournChore(IStateMachineTarget master) : base(Db.Get().ChoreTypes.Mourn, master, master.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.high, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new MournChore.StatesInstance(this);
		this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
		this.AddPrecondition(ChorePreconditions.instance.NoDeadBodies, null);
		this.AddPrecondition(MournChore.HasValidMournLocation, master);
	}

	// Token: 0x06001F84 RID: 8068 RVA: 0x001C50B0 File Offset: 0x001C32B0
	public static Grave FindGraveToMournAt()
	{
		Grave result = null;
		float num = -1f;
		foreach (object obj in Components.Graves)
		{
			Grave grave = (Grave)obj;
			if (grave.burialTime > num)
			{
				num = grave.burialTime;
				result = grave;
			}
		}
		return result;
	}

	// Token: 0x06001F85 RID: 8069 RVA: 0x001C5120 File Offset: 0x001C3320
	public override void Begin(Chore.Precondition.Context context)
	{
		if (context.consumerState.consumer == null)
		{
			global::Debug.LogError("MournChore null context.consumer");
			return;
		}
		if (base.smi == null)
		{
			global::Debug.LogError("MournChore null smi");
			return;
		}
		if (base.smi.sm == null)
		{
			global::Debug.LogError("MournChore null smi.sm");
			return;
		}
		if (MournChore.FindGraveToMournAt() == null)
		{
			global::Debug.LogError("MournChore no grave");
			return;
		}
		base.smi.sm.mourner.Set(context.consumerState.gameObject, base.smi, false);
		base.Begin(context);
	}

	// Token: 0x040014CC RID: 5324
	private static readonly CellOffset[] ValidStandingOffsets = new CellOffset[]
	{
		new CellOffset(0, 0),
		new CellOffset(-1, 0),
		new CellOffset(1, 0)
	};

	// Token: 0x040014CD RID: 5325
	private static readonly Chore.Precondition HasValidMournLocation = new Chore.Precondition
	{
		id = "HasPlaceToStand",
		description = DUPLICANTS.CHORES.PRECONDITIONS.HAS_PLACE_TO_STAND,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			Navigator component = ((IStateMachineTarget)data).GetComponent<Navigator>();
			bool result = false;
			Grave grave = MournChore.FindGraveToMournAt();
			if (grave != null && Grid.IsValidCell(MournChore.GetStandableCell(Grid.PosToCell(grave), component)))
			{
				result = true;
			}
			return result;
		}
	};

	// Token: 0x020006EF RID: 1775
	public class StatesInstance : GameStateMachine<MournChore.States, MournChore.StatesInstance, MournChore, object>.GameInstance
	{
		// Token: 0x06001F87 RID: 8071 RVA: 0x000B9302 File Offset: 0x000B7502
		public StatesInstance(MournChore master) : base(master)
		{
		}

		// Token: 0x06001F88 RID: 8072 RVA: 0x001C5244 File Offset: 0x001C3444
		public void CreateLocator()
		{
			int cell = Grid.PosToCell(MournChore.FindGraveToMournAt().transform.GetPosition());
			Navigator component = base.master.GetComponent<Navigator>();
			int standableCell = MournChore.GetStandableCell(cell, component);
			if (standableCell < 0)
			{
				base.smi.GoTo(null);
				return;
			}
			Grid.Reserved[standableCell] = true;
			Vector3 pos = Grid.CellToPosCBC(standableCell, Grid.SceneLayer.Move);
			GameObject value = ChoreHelpers.CreateLocator("MournLocator", pos);
			base.smi.sm.locator.Set(value, base.smi, false);
			this.locatorCell = standableCell;
			base.smi.GoTo(base.sm.moveto);
		}

		// Token: 0x06001F89 RID: 8073 RVA: 0x001C52E8 File Offset: 0x001C34E8
		public void DestroyLocator()
		{
			if (this.locatorCell >= 0)
			{
				Grid.Reserved[this.locatorCell] = false;
				ChoreHelpers.DestroyLocator(base.sm.locator.Get(this));
				base.sm.locator.Set(null, this);
				this.locatorCell = -1;
			}
		}

		// Token: 0x040014CE RID: 5326
		private int locatorCell = -1;
	}

	// Token: 0x020006F0 RID: 1776
	public class States : GameStateMachine<MournChore.States, MournChore.StatesInstance, MournChore>
	{
		// Token: 0x06001F8A RID: 8074 RVA: 0x001C5340 File Offset: 0x001C3540
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.findOffset;
			base.Target(this.mourner);
			this.root.ToggleAnims(new Func<MournChore.StatesInstance, KAnimFile>(MournChore.GetAnimFileName)).Exit("DestroyLocator", delegate(MournChore.StatesInstance smi)
			{
				smi.DestroyLocator();
			});
			this.findOffset.Enter("CreateLocator", delegate(MournChore.StatesInstance smi)
			{
				smi.CreateLocator();
			});
			this.moveto.InitializeStates(this.mourner, this.locator, this.mourn, null, null, null);
			this.mourn.PlayAnims((MournChore.StatesInstance smi) => MournChore.States.WORK_ANIMS, KAnim.PlayMode.Loop).ScheduleGoTo(10f, this.completed);
			this.completed.PlayAnim("working_pst").OnAnimQueueComplete(null).Exit(delegate(MournChore.StatesInstance smi)
			{
				this.mourner.Get<Effects>(smi).Remove(Db.Get().effects.Get("Mourning"));
			});
		}

		// Token: 0x040014CF RID: 5327
		public StateMachine<MournChore.States, MournChore.StatesInstance, MournChore, object>.TargetParameter mourner;

		// Token: 0x040014D0 RID: 5328
		public StateMachine<MournChore.States, MournChore.StatesInstance, MournChore, object>.TargetParameter locator;

		// Token: 0x040014D1 RID: 5329
		public GameStateMachine<MournChore.States, MournChore.StatesInstance, MournChore, object>.State findOffset;

		// Token: 0x040014D2 RID: 5330
		public GameStateMachine<MournChore.States, MournChore.StatesInstance, MournChore, object>.ApproachSubState<IApproachable> moveto;

		// Token: 0x040014D3 RID: 5331
		public GameStateMachine<MournChore.States, MournChore.StatesInstance, MournChore, object>.State mourn;

		// Token: 0x040014D4 RID: 5332
		public GameStateMachine<MournChore.States, MournChore.StatesInstance, MournChore, object>.State completed;

		// Token: 0x040014D5 RID: 5333
		private static readonly HashedString[] WORK_ANIMS = new HashedString[]
		{
			"working_pre",
			"working_loop"
		};
	}
}
