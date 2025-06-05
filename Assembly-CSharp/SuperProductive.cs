using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020007EE RID: 2030
public class SuperProductive : GameStateMachine<SuperProductive, SuperProductive.Instance>
{
	// Token: 0x060023D7 RID: 9175 RVA: 0x001D34A4 File Offset: 0x001D16A4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.neutral;
		this.root.TagTransition(GameTags.Dead, null, false);
		this.neutral.TagTransition(GameTags.Overjoyed, this.overjoyed, false);
		this.overjoyed.TagTransition(GameTags.Overjoyed, this.neutral, true).ToggleStatusItem(Db.Get().DuplicantStatusItems.BeingProductive, null).Enter(delegate(SuperProductive.Instance smi)
		{
			if (PopFXManager.Instance != null)
			{
				PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, DUPLICANTS.TRAITS.SUPERPRODUCTIVE.NAME, smi.master.transform, new Vector3(0f, 0.5f, 0f), 1.5f, false, false);
			}
			smi.fx = new SuperProductiveFX.Instance(smi.GetComponent<KMonoBehaviour>(), new Vector3(0f, 0f, Grid.GetLayerZ(Grid.SceneLayer.FXFront)));
			smi.fx.StartSM();
		}).Exit(delegate(SuperProductive.Instance smi)
		{
			smi.fx.sm.destroyFX.Trigger(smi.fx);
		}).DefaultState(this.overjoyed.idle);
		this.overjoyed.idle.EventTransition(GameHashes.StartWork, this.overjoyed.working, null);
		this.overjoyed.working.ScheduleGoTo(0.33f, this.overjoyed.superProductive);
		this.overjoyed.superProductive.Enter(delegate(SuperProductive.Instance smi)
		{
			WorkerBase component = smi.GetComponent<WorkerBase>();
			if (component != null && component.GetState() == WorkerBase.State.Working)
			{
				Workable workable = component.GetWorkable();
				if (workable != null)
				{
					float num = workable.WorkTimeRemaining;
					if (workable.GetComponent<Diggable>() != null)
					{
						num = Diggable.GetApproximateDigTime(Grid.PosToCell(workable));
					}
					if (num > 1f && smi.ShouldSkipWork() && component.InstantlyFinish())
					{
						smi.ReactSuperProductive();
						smi.fx.sm.wasProductive.Trigger(smi.fx);
					}
				}
			}
			smi.GoTo(this.overjoyed.idle);
		});
	}

	// Token: 0x04001825 RID: 6181
	public GameStateMachine<SuperProductive, SuperProductive.Instance, IStateMachineTarget, object>.State neutral;

	// Token: 0x04001826 RID: 6182
	public SuperProductive.OverjoyedStates overjoyed;

	// Token: 0x020007EF RID: 2031
	public class OverjoyedStates : GameStateMachine<SuperProductive, SuperProductive.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04001827 RID: 6183
		public GameStateMachine<SuperProductive, SuperProductive.Instance, IStateMachineTarget, object>.State idle;

		// Token: 0x04001828 RID: 6184
		public GameStateMachine<SuperProductive, SuperProductive.Instance, IStateMachineTarget, object>.State working;

		// Token: 0x04001829 RID: 6185
		public GameStateMachine<SuperProductive, SuperProductive.Instance, IStateMachineTarget, object>.State superProductive;
	}

	// Token: 0x020007F0 RID: 2032
	public new class Instance : GameStateMachine<SuperProductive, SuperProductive.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x060023DB RID: 9179 RVA: 0x000BBC15 File Offset: 0x000B9E15
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x060023DC RID: 9180 RVA: 0x000BBC1E File Offset: 0x000B9E1E
		public bool ShouldSkipWork()
		{
			return UnityEngine.Random.Range(0f, 100f) <= TRAITS.JOY_REACTIONS.SUPER_PRODUCTIVE.INSTANT_SUCCESS_CHANCE;
		}

		// Token: 0x060023DD RID: 9181 RVA: 0x001D3670 File Offset: 0x001D1870
		public void ReactSuperProductive()
		{
			ReactionMonitor.Instance smi = base.gameObject.GetSMI<ReactionMonitor.Instance>();
			if (smi != null)
			{
				smi.AddSelfEmoteReactable(base.gameObject, "SuperProductive", Db.Get().Emotes.Minion.ProductiveCheer, true, Db.Get().ChoreTypes.EmoteHighPriority, 0f, 1f, 1f, 0f, null);
			}
		}

		// Token: 0x0400182A RID: 6186
		public SuperProductiveFX.Instance fx;
	}
}
