using System;
using UnityEngine;

// Token: 0x02000726 RID: 1830
public class RecoverFromHeatChore : Chore<RecoverFromHeatChore.Instance>
{
	// Token: 0x0600203A RID: 8250 RVA: 0x001C7890 File Offset: 0x001C5A90
	public RecoverFromHeatChore(IStateMachineTarget target) : base(Db.Get().ChoreTypes.RecoverFromHeat, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.personalNeeds, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new RecoverFromHeatChore.Instance(this, target.gameObject);
		HeatImmunityMonitor.Instance chillyBones = target.gameObject.GetSMI<HeatImmunityMonitor.Instance>();
		Func<int> data = () => chillyBones.ShelterCell;
		this.AddPrecondition(ChorePreconditions.instance.CanMoveToDynamicCell, data);
		this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
	}

	// Token: 0x02000727 RID: 1831
	public class States : GameStateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore>
	{
		// Token: 0x0600203B RID: 8251 RVA: 0x001C791C File Offset: 0x001C5B1C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.approach;
			base.Target(this.entityRecovering);
			this.root.Enter("CreateLocator", delegate(RecoverFromHeatChore.Instance smi)
			{
				smi.CreateLocator();
			}).Enter("UpdateImmunityProvider", delegate(RecoverFromHeatChore.Instance smi)
			{
				smi.UpdateImmunityProvider();
			}).Exit("DestroyLocator", delegate(RecoverFromHeatChore.Instance smi)
			{
				smi.DestroyLocator();
			}).Update("UpdateLocator", delegate(RecoverFromHeatChore.Instance smi, float dt)
			{
				smi.UpdateLocator();
			}, UpdateRate.SIM_200ms, true).Update("UpdateHeatImmunityProvider", delegate(RecoverFromHeatChore.Instance smi, float dt)
			{
				smi.UpdateImmunityProvider();
			}, UpdateRate.SIM_200ms, true);
			this.approach.InitializeStates(this.entityRecovering, this.locator, this.recover, null, null, null);
			this.recover.OnTargetLost(this.heatImmunityProvider, null).Enter("AnimOverride", delegate(RecoverFromHeatChore.Instance smi)
			{
				smi.cachedAnimName = RecoverFromHeatChore.States.GetAnimFileName(smi);
				smi.GetComponent<KAnimControllerBase>().AddAnimOverrides(Assets.GetAnim(smi.cachedAnimName), 0f);
			}).Exit(delegate(RecoverFromHeatChore.Instance smi)
			{
				if (smi.cachedAnimName != HashedString.Invalid)
				{
					smi.GetComponent<KAnimControllerBase>().RemoveAnimOverrides(Assets.GetAnim(smi.cachedAnimName));
				}
			}).DefaultState(this.recover.pre).ToggleTag(GameTags.RecoveringFromHeat);
			this.recover.pre.Face(this.heatImmunityProvider, 0f).PlayAnim(new Func<RecoverFromHeatChore.Instance, string>(RecoverFromHeatChore.States.GetPreAnimName), KAnim.PlayMode.Once).OnAnimQueueComplete(this.recover.loop);
			this.recover.loop.PlayAnim(new Func<RecoverFromHeatChore.Instance, string>(RecoverFromHeatChore.States.GetLoopAnimName), KAnim.PlayMode.Once).OnAnimQueueComplete(this.recover.pst);
			this.recover.pst.QueueAnim(new Func<RecoverFromHeatChore.Instance, string>(RecoverFromHeatChore.States.GetPstAnimName), false, null).OnAnimQueueComplete(this.complete);
			this.complete.DefaultState(this.complete.evaluate);
			this.complete.evaluate.EnterTransition(this.complete.success, new StateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.Transition.ConditionCallback(RecoverFromHeatChore.States.IsImmunityProviderStillValid)).EnterTransition(this.complete.fail, GameStateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.Not(new StateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.Transition.ConditionCallback(RecoverFromHeatChore.States.IsImmunityProviderStillValid)));
			this.complete.success.Enter(new StateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.State.Callback(RecoverFromHeatChore.States.ApplyHeatImmunityEffect)).ReturnSuccess();
			this.complete.fail.ReturnFailure();
		}

		// Token: 0x0600203C RID: 8252 RVA: 0x001C7BDC File Offset: 0x001C5DDC
		public static bool IsImmunityProviderStillValid(RecoverFromHeatChore.Instance smi)
		{
			HeatImmunityProvider.Instance lastKnownImmunityProvider = smi.lastKnownImmunityProvider;
			return lastKnownImmunityProvider != null && lastKnownImmunityProvider.CanBeUsed;
		}

		// Token: 0x0600203D RID: 8253 RVA: 0x001C7BFC File Offset: 0x001C5DFC
		public static void ApplyHeatImmunityEffect(RecoverFromHeatChore.Instance smi)
		{
			HeatImmunityProvider.Instance lastKnownImmunityProvider = smi.lastKnownImmunityProvider;
			if (lastKnownImmunityProvider != null)
			{
				lastKnownImmunityProvider.ApplyImmunityEffect(smi.gameObject, true);
			}
		}

		// Token: 0x0600203E RID: 8254 RVA: 0x001C7C20 File Offset: 0x001C5E20
		public static HashedString GetAnimFileName(RecoverFromHeatChore.Instance smi)
		{
			return RecoverFromHeatChore.States.GetAnimFromHeatImmunityProvider(smi, (HeatImmunityProvider.Instance p) => p.GetAnimFileName(smi.sm.entityRecovering.Get(smi)));
		}

		// Token: 0x0600203F RID: 8255 RVA: 0x000B9982 File Offset: 0x000B7B82
		public static string GetPreAnimName(RecoverFromHeatChore.Instance smi)
		{
			return RecoverFromHeatChore.States.GetAnimFromHeatImmunityProvider(smi, (HeatImmunityProvider.Instance p) => p.PreAnimName);
		}

		// Token: 0x06002040 RID: 8256 RVA: 0x000B99A9 File Offset: 0x000B7BA9
		public static string GetLoopAnimName(RecoverFromHeatChore.Instance smi)
		{
			return RecoverFromHeatChore.States.GetAnimFromHeatImmunityProvider(smi, (HeatImmunityProvider.Instance p) => p.LoopAnimName);
		}

		// Token: 0x06002041 RID: 8257 RVA: 0x000B99D0 File Offset: 0x000B7BD0
		public static string GetPstAnimName(RecoverFromHeatChore.Instance smi)
		{
			return RecoverFromHeatChore.States.GetAnimFromHeatImmunityProvider(smi, (HeatImmunityProvider.Instance p) => p.PstAnimName);
		}

		// Token: 0x06002042 RID: 8258 RVA: 0x001C7C58 File Offset: 0x001C5E58
		public static string GetAnimFromHeatImmunityProvider(RecoverFromHeatChore.Instance smi, Func<HeatImmunityProvider.Instance, string> getCallback)
		{
			HeatImmunityProvider.Instance lastKnownImmunityProvider = smi.lastKnownImmunityProvider;
			if (lastKnownImmunityProvider != null)
			{
				return getCallback(lastKnownImmunityProvider);
			}
			return null;
		}

		// Token: 0x04001560 RID: 5472
		public GameStateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.ApproachSubState<IApproachable> approach;

		// Token: 0x04001561 RID: 5473
		public GameStateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.PreLoopPostState recover;

		// Token: 0x04001562 RID: 5474
		public GameStateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.State remove_suit;

		// Token: 0x04001563 RID: 5475
		public RecoverFromHeatChore.States.CompleteStates complete;

		// Token: 0x04001564 RID: 5476
		public StateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.TargetParameter heatImmunityProvider;

		// Token: 0x04001565 RID: 5477
		public StateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.TargetParameter entityRecovering;

		// Token: 0x04001566 RID: 5478
		public StateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.TargetParameter locator;

		// Token: 0x02000728 RID: 1832
		public class CompleteStates : GameStateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.State
		{
			// Token: 0x04001567 RID: 5479
			public GameStateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.State evaluate;

			// Token: 0x04001568 RID: 5480
			public GameStateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.State fail;

			// Token: 0x04001569 RID: 5481
			public GameStateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.State success;
		}
	}

	// Token: 0x0200072B RID: 1835
	public class Instance : GameStateMachine<RecoverFromHeatChore.States, RecoverFromHeatChore.Instance, RecoverFromHeatChore, object>.GameInstance
	{
		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06002053 RID: 8275 RVA: 0x000B9AC1 File Offset: 0x000B7CC1
		public HeatImmunityProvider.Instance lastKnownImmunityProvider
		{
			get
			{
				if (!(base.sm.heatImmunityProvider.Get(this) == null))
				{
					return base.sm.heatImmunityProvider.Get(this).GetSMI<HeatImmunityProvider.Instance>();
				}
				return null;
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06002054 RID: 8276 RVA: 0x000B9AF4 File Offset: 0x000B7CF4
		public HeatImmunityMonitor.Instance heatImmunityMonitor
		{
			get
			{
				return base.sm.entityRecovering.Get(this).GetSMI<HeatImmunityMonitor.Instance>();
			}
		}

		// Token: 0x06002055 RID: 8277 RVA: 0x001C7C78 File Offset: 0x001C5E78
		public Instance(RecoverFromHeatChore master, GameObject entityRecovering) : base(master)
		{
			base.sm.entityRecovering.Set(entityRecovering, this, false);
			HeatImmunityMonitor.Instance heatImmunityMonitor = this.heatImmunityMonitor;
			if (heatImmunityMonitor.NearestImmunityProvider != null && !heatImmunityMonitor.NearestImmunityProvider.isMasterNull)
			{
				base.sm.heatImmunityProvider.Set(heatImmunityMonitor.NearestImmunityProvider.gameObject, this, false);
			}
		}

		// Token: 0x06002056 RID: 8278 RVA: 0x001C7CDC File Offset: 0x001C5EDC
		public void CreateLocator()
		{
			GameObject value = ChoreHelpers.CreateLocator("RecoverWarmthLocator", Vector3.zero);
			base.sm.locator.Set(value, this, false);
			this.UpdateLocator();
		}

		// Token: 0x06002057 RID: 8279 RVA: 0x001C7D14 File Offset: 0x001C5F14
		public void UpdateImmunityProvider()
		{
			HeatImmunityProvider.Instance nearestImmunityProvider = this.heatImmunityMonitor.NearestImmunityProvider;
			base.sm.heatImmunityProvider.Set((nearestImmunityProvider == null || nearestImmunityProvider.isMasterNull) ? null : nearestImmunityProvider.gameObject, this, false);
		}

		// Token: 0x06002058 RID: 8280 RVA: 0x001C7D54 File Offset: 0x001C5F54
		public void UpdateLocator()
		{
			int num = this.heatImmunityMonitor.ShelterCell;
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

		// Token: 0x06002059 RID: 8281 RVA: 0x000B9B0C File Offset: 0x000B7D0C
		public void DestroyLocator()
		{
			ChoreHelpers.DestroyLocator(base.sm.locator.Get(this));
			base.sm.locator.Set(null, this);
		}

		// Token: 0x04001576 RID: 5494
		private int targetCell;

		// Token: 0x04001577 RID: 5495
		public HashedString cachedAnimName;
	}
}
