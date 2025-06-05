using System;
using UnityEngine;

// Token: 0x0200071F RID: 1823
public class RecoverFromColdChore : Chore<RecoverFromColdChore.Instance>
{
	// Token: 0x0600201A RID: 8218 RVA: 0x001C7390 File Offset: 0x001C5590
	public RecoverFromColdChore(IStateMachineTarget target) : base(Db.Get().ChoreTypes.RecoverWarmth, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.personalNeeds, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new RecoverFromColdChore.Instance(this, target.gameObject);
		ColdImmunityMonitor.Instance coldImmunityMonitor = target.gameObject.GetSMI<ColdImmunityMonitor.Instance>();
		Func<int> data = () => coldImmunityMonitor.WarmUpCell;
		this.AddPrecondition(ChorePreconditions.instance.CanMoveToDynamicCell, data);
		this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
	}

	// Token: 0x02000720 RID: 1824
	public class States : GameStateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore>
	{
		// Token: 0x0600201B RID: 8219 RVA: 0x001C741C File Offset: 0x001C561C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.approach;
			base.Target(this.entityRecovering);
			this.root.Enter("CreateLocator", delegate(RecoverFromColdChore.Instance smi)
			{
				smi.CreateLocator();
			}).Enter("UpdateImmunityProvider", delegate(RecoverFromColdChore.Instance smi)
			{
				smi.UpdateImmunityProvider();
			}).Exit("DestroyLocator", delegate(RecoverFromColdChore.Instance smi)
			{
				smi.DestroyLocator();
			}).Update("UpdateLocator", delegate(RecoverFromColdChore.Instance smi, float dt)
			{
				smi.UpdateLocator();
			}, UpdateRate.SIM_200ms, true).Update("UpdateColdImmunityProvider", delegate(RecoverFromColdChore.Instance smi, float dt)
			{
				smi.UpdateImmunityProvider();
			}, UpdateRate.SIM_200ms, true);
			this.approach.InitializeStates(this.entityRecovering, this.locator, this.recover, null, null, null);
			this.recover.OnTargetLost(this.coldImmunityProvider, null).ToggleAnims(new Func<RecoverFromColdChore.Instance, HashedString>(RecoverFromColdChore.States.GetAnimFileName)).DefaultState(this.recover.pre).ToggleTag(GameTags.RecoveringWarmnth);
			this.recover.pre.Face(this.coldImmunityProvider, 0f).PlayAnim(new Func<RecoverFromColdChore.Instance, string>(RecoverFromColdChore.States.GetPreAnimName), KAnim.PlayMode.Once).OnAnimQueueComplete(this.recover.loop);
			this.recover.loop.PlayAnim(new Func<RecoverFromColdChore.Instance, string>(RecoverFromColdChore.States.GetLoopAnimName), KAnim.PlayMode.Once).OnAnimQueueComplete(this.recover.pst);
			this.recover.pst.QueueAnim(new Func<RecoverFromColdChore.Instance, string>(RecoverFromColdChore.States.GetPstAnimName), false, null).OnAnimQueueComplete(this.complete);
			this.complete.DefaultState(this.complete.evaluate);
			this.complete.evaluate.EnterTransition(this.complete.success, new StateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.Transition.ConditionCallback(RecoverFromColdChore.States.IsImmunityProviderStillValid)).EnterTransition(this.complete.fail, GameStateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.Not(new StateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.Transition.ConditionCallback(RecoverFromColdChore.States.IsImmunityProviderStillValid)));
			this.complete.success.Enter(new StateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.State.Callback(RecoverFromColdChore.States.ApplyColdImmunityEffect)).ReturnSuccess();
			this.complete.fail.ReturnFailure();
		}

		// Token: 0x0600201C RID: 8220 RVA: 0x001C76A0 File Offset: 0x001C58A0
		public static bool IsImmunityProviderStillValid(RecoverFromColdChore.Instance smi)
		{
			ColdImmunityProvider.Instance lastKnownImmunityProvider = smi.lastKnownImmunityProvider;
			return lastKnownImmunityProvider != null && lastKnownImmunityProvider.CanBeUsed;
		}

		// Token: 0x0600201D RID: 8221 RVA: 0x001C76C0 File Offset: 0x001C58C0
		public static void ApplyColdImmunityEffect(RecoverFromColdChore.Instance smi)
		{
			ColdImmunityProvider.Instance lastKnownImmunityProvider = smi.lastKnownImmunityProvider;
			if (lastKnownImmunityProvider != null)
			{
				lastKnownImmunityProvider.ApplyImmunityEffect(smi.gameObject, true);
			}
		}

		// Token: 0x0600201E RID: 8222 RVA: 0x001C76E4 File Offset: 0x001C58E4
		public static HashedString GetAnimFileName(RecoverFromColdChore.Instance smi)
		{
			return RecoverFromColdChore.States.GetAnimFromColdImmunityProvider(smi, (ColdImmunityProvider.Instance p) => p.GetAnimFileName(smi.sm.entityRecovering.Get(smi)));
		}

		// Token: 0x0600201F RID: 8223 RVA: 0x000B9814 File Offset: 0x000B7A14
		public static string GetPreAnimName(RecoverFromColdChore.Instance smi)
		{
			return RecoverFromColdChore.States.GetAnimFromColdImmunityProvider(smi, (ColdImmunityProvider.Instance p) => p.PreAnimName);
		}

		// Token: 0x06002020 RID: 8224 RVA: 0x000B983B File Offset: 0x000B7A3B
		public static string GetLoopAnimName(RecoverFromColdChore.Instance smi)
		{
			return RecoverFromColdChore.States.GetAnimFromColdImmunityProvider(smi, (ColdImmunityProvider.Instance p) => p.LoopAnimName);
		}

		// Token: 0x06002021 RID: 8225 RVA: 0x000B9862 File Offset: 0x000B7A62
		public static string GetPstAnimName(RecoverFromColdChore.Instance smi)
		{
			return RecoverFromColdChore.States.GetAnimFromColdImmunityProvider(smi, (ColdImmunityProvider.Instance p) => p.PstAnimName);
		}

		// Token: 0x06002022 RID: 8226 RVA: 0x001C771C File Offset: 0x001C591C
		public static string GetAnimFromColdImmunityProvider(RecoverFromColdChore.Instance smi, Func<ColdImmunityProvider.Instance, string> getCallback)
		{
			ColdImmunityProvider.Instance lastKnownImmunityProvider = smi.lastKnownImmunityProvider;
			if (lastKnownImmunityProvider != null)
			{
				return getCallback(lastKnownImmunityProvider);
			}
			return null;
		}

		// Token: 0x0400154A RID: 5450
		public GameStateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.ApproachSubState<IApproachable> approach;

		// Token: 0x0400154B RID: 5451
		public GameStateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.PreLoopPostState recover;

		// Token: 0x0400154C RID: 5452
		public GameStateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.State remove_suit;

		// Token: 0x0400154D RID: 5453
		public RecoverFromColdChore.States.CompleteStates complete;

		// Token: 0x0400154E RID: 5454
		public StateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.TargetParameter coldImmunityProvider;

		// Token: 0x0400154F RID: 5455
		public StateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.TargetParameter entityRecovering;

		// Token: 0x04001550 RID: 5456
		public StateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.TargetParameter locator;

		// Token: 0x02000721 RID: 1825
		public class CompleteStates : GameStateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.State
		{
			// Token: 0x04001551 RID: 5457
			public GameStateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.State evaluate;

			// Token: 0x04001552 RID: 5458
			public GameStateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.State fail;

			// Token: 0x04001553 RID: 5459
			public GameStateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.State success;
		}
	}

	// Token: 0x02000724 RID: 1828
	public class Instance : GameStateMachine<RecoverFromColdChore.States, RecoverFromColdChore.Instance, RecoverFromColdChore, object>.GameInstance
	{
		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x06002031 RID: 8241 RVA: 0x000B9900 File Offset: 0x000B7B00
		public ColdImmunityProvider.Instance lastKnownImmunityProvider
		{
			get
			{
				if (!(base.sm.coldImmunityProvider.Get(this) == null))
				{
					return base.sm.coldImmunityProvider.Get(this).GetSMI<ColdImmunityProvider.Instance>();
				}
				return null;
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06002032 RID: 8242 RVA: 0x000B9933 File Offset: 0x000B7B33
		public ColdImmunityMonitor.Instance coldImmunityMonitor
		{
			get
			{
				return base.sm.entityRecovering.Get(this).GetSMI<ColdImmunityMonitor.Instance>();
			}
		}

		// Token: 0x06002033 RID: 8243 RVA: 0x001C773C File Offset: 0x001C593C
		public Instance(RecoverFromColdChore master, GameObject entityRecovering) : base(master)
		{
			base.sm.entityRecovering.Set(entityRecovering, this, false);
			ColdImmunityMonitor.Instance coldImmunityMonitor = this.coldImmunityMonitor;
			if (coldImmunityMonitor.NearestImmunityProvider != null && !coldImmunityMonitor.NearestImmunityProvider.isMasterNull)
			{
				base.sm.coldImmunityProvider.Set(coldImmunityMonitor.NearestImmunityProvider.gameObject, this, false);
			}
		}

		// Token: 0x06002034 RID: 8244 RVA: 0x001C77A0 File Offset: 0x001C59A0
		public void CreateLocator()
		{
			GameObject value = ChoreHelpers.CreateLocator("RecoverWarmthLocator", Vector3.zero);
			base.sm.locator.Set(value, this, false);
			this.UpdateLocator();
		}

		// Token: 0x06002035 RID: 8245 RVA: 0x001C77D8 File Offset: 0x001C59D8
		public void UpdateImmunityProvider()
		{
			ColdImmunityProvider.Instance nearestImmunityProvider = this.coldImmunityMonitor.NearestImmunityProvider;
			base.sm.coldImmunityProvider.Set((nearestImmunityProvider == null || nearestImmunityProvider.isMasterNull) ? null : nearestImmunityProvider.gameObject, this, false);
		}

		// Token: 0x06002036 RID: 8246 RVA: 0x001C7818 File Offset: 0x001C5A18
		public void UpdateLocator()
		{
			int num = this.coldImmunityMonitor.WarmUpCell;
			if (num == Grid.InvalidCell)
			{
				num = Grid.PosToCell(base.sm.entityRecovering.Get<Transform>(base.smi).GetPosition());
				this.DestroyLocator();
			}
			else
			{
				Vector3 position = Grid.CellToPosCBC(num, Grid.SceneLayer.Move);
				base.sm.locator.Get<Transform>(base.smi).SetPosition(position);
			}
			this.targetCell = num;
		}

		// Token: 0x06002037 RID: 8247 RVA: 0x000B994B File Offset: 0x000B7B4B
		public void DestroyLocator()
		{
			ChoreHelpers.DestroyLocator(base.sm.locator.Get(this));
			base.sm.locator.Set(null, this);
		}

		// Token: 0x0400155E RID: 5470
		private int targetCell;
	}
}
