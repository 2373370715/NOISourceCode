using System;
using UnityEngine;

// Token: 0x020018DA RID: 6362
[SkipSaveFileSerialization]
public class Snorer : StateMachineComponent<Snorer.StatesInstance>
{
	// Token: 0x060083A3 RID: 33699 RVA: 0x000FB136 File Offset: 0x000F9336
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x0400643D RID: 25661
	private static readonly HashedString HeadHash = "snapTo_mouth";

	// Token: 0x020018DB RID: 6363
	public class StatesInstance : GameStateMachine<Snorer.States, Snorer.StatesInstance, Snorer, object>.GameInstance
	{
		// Token: 0x060083A6 RID: 33702 RVA: 0x000FB15C File Offset: 0x000F935C
		public StatesInstance(Snorer master) : base(master)
		{
		}

		// Token: 0x060083A7 RID: 33703 RVA: 0x0034F820 File Offset: 0x0034DA20
		public bool IsSleeping()
		{
			StaminaMonitor.Instance smi = base.master.GetSMI<StaminaMonitor.Instance>();
			return smi != null && smi.IsSleeping();
		}

		// Token: 0x060083A8 RID: 33704 RVA: 0x000FB165 File Offset: 0x000F9365
		public void StartSmallSnore()
		{
			this.snoreHandle = GameScheduler.Instance.Schedule("snorelines", 2f, new Action<object>(this.StartSmallSnoreInternal), null, null);
		}

		// Token: 0x060083A9 RID: 33705 RVA: 0x0034F844 File Offset: 0x0034DA44
		private void StartSmallSnoreInternal(object data)
		{
			this.snoreHandle.ClearScheduler();
			bool flag;
			Matrix4x4 symbolTransform = base.smi.master.GetComponent<KBatchedAnimController>().GetSymbolTransform(Snorer.HeadHash, out flag);
			if (flag)
			{
				Vector3 position = symbolTransform.GetColumn(3);
				position.z = Grid.GetLayerZ(Grid.SceneLayer.FXFront);
				this.snoreEffect = FXHelpers.CreateEffect("snore_fx_kanim", position, null, false, Grid.SceneLayer.Front, false);
				this.snoreEffect.destroyOnAnimComplete = true;
				this.snoreEffect.Play("snore", KAnim.PlayMode.Loop, 1f, 0f);
			}
		}

		// Token: 0x060083AA RID: 33706 RVA: 0x000FB18F File Offset: 0x000F938F
		public void StopSmallSnore()
		{
			this.snoreHandle.ClearScheduler();
			if (this.snoreEffect != null)
			{
				this.snoreEffect.PlayMode = KAnim.PlayMode.Once;
			}
			this.snoreEffect = null;
		}

		// Token: 0x060083AB RID: 33707 RVA: 0x000FB1BD File Offset: 0x000F93BD
		public void StartSnoreBGEffect()
		{
			AcousticDisturbance.Emit(base.smi.master.gameObject, 3);
		}

		// Token: 0x060083AC RID: 33708 RVA: 0x000AA038 File Offset: 0x000A8238
		public void StopSnoreBGEffect()
		{
		}

		// Token: 0x0400643E RID: 25662
		private SchedulerHandle snoreHandle;

		// Token: 0x0400643F RID: 25663
		private KBatchedAnimController snoreEffect;

		// Token: 0x04006440 RID: 25664
		private KBatchedAnimController snoreBGEffect;

		// Token: 0x04006441 RID: 25665
		private const float BGEmissionRadius = 3f;
	}

	// Token: 0x020018DC RID: 6364
	public class States : GameStateMachine<Snorer.States, Snorer.StatesInstance, Snorer>
	{
		// Token: 0x060083AD RID: 33709 RVA: 0x0034F8DC File Offset: 0x0034DADC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.root.TagTransition(GameTags.Dead, null, false);
			this.idle.Transition(this.sleeping, (Snorer.StatesInstance smi) => smi.IsSleeping(), UpdateRate.SIM_200ms);
			this.sleeping.DefaultState(this.sleeping.quiet).Enter(delegate(Snorer.StatesInstance smi)
			{
				smi.StartSmallSnore();
			}).Exit(delegate(Snorer.StatesInstance smi)
			{
				smi.StopSmallSnore();
			}).Transition(this.idle, (Snorer.StatesInstance smi) => !smi.master.GetSMI<StaminaMonitor.Instance>().IsSleeping(), UpdateRate.SIM_200ms);
			this.sleeping.quiet.Enter("ScheduleNextSnore", delegate(Snorer.StatesInstance smi)
			{
				smi.ScheduleGoTo(this.GetNewInterval(), this.sleeping.snoring);
			});
			this.sleeping.snoring.Enter(delegate(Snorer.StatesInstance smi)
			{
				smi.StartSnoreBGEffect();
			}).ToggleExpression(Db.Get().Expressions.Relief, null).ScheduleGoTo(3f, this.sleeping.quiet).Exit(delegate(Snorer.StatesInstance smi)
			{
				smi.StopSnoreBGEffect();
			});
		}

		// Token: 0x060083AE RID: 33710 RVA: 0x000FB1D5 File Offset: 0x000F93D5
		private float GetNewInterval()
		{
			return Mathf.Min(Mathf.Max(Util.GaussianRandom(5f, 1f), 3f), 10f);
		}

		// Token: 0x04006442 RID: 25666
		public GameStateMachine<Snorer.States, Snorer.StatesInstance, Snorer, object>.State idle;

		// Token: 0x04006443 RID: 25667
		public Snorer.States.SleepStates sleeping;

		// Token: 0x020018DD RID: 6365
		public class SleepStates : GameStateMachine<Snorer.States, Snorer.StatesInstance, Snorer, object>.State
		{
			// Token: 0x04006444 RID: 25668
			public GameStateMachine<Snorer.States, Snorer.StatesInstance, Snorer, object>.State quiet;

			// Token: 0x04006445 RID: 25669
			public GameStateMachine<Snorer.States, Snorer.StatesInstance, Snorer, object>.State snoring;
		}
	}
}
