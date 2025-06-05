using System;
using Klei.AI;
using UnityEngine;

// Token: 0x02001635 RID: 5685
public class SneezeMonitor : GameStateMachine<SneezeMonitor, SneezeMonitor.Instance>
{
	// Token: 0x060075A1 RID: 30113 RVA: 0x00315E1C File Offset: 0x0031401C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.idle;
		this.idle.ParamTransition<bool>(this.isSneezy, this.sneezy, (SneezeMonitor.Instance smi, bool p) => p);
		this.sneezy.ParamTransition<bool>(this.isSneezy, this.idle, (SneezeMonitor.Instance smi, bool p) => !p).ToggleReactable((SneezeMonitor.Instance smi) => smi.GetReactable());
	}

	// Token: 0x04005862 RID: 22626
	public StateMachine<SneezeMonitor, SneezeMonitor.Instance, IStateMachineTarget, object>.BoolParameter isSneezy = new StateMachine<SneezeMonitor, SneezeMonitor.Instance, IStateMachineTarget, object>.BoolParameter(false);

	// Token: 0x04005863 RID: 22627
	public GameStateMachine<SneezeMonitor, SneezeMonitor.Instance, IStateMachineTarget, object>.State idle;

	// Token: 0x04005864 RID: 22628
	public GameStateMachine<SneezeMonitor, SneezeMonitor.Instance, IStateMachineTarget, object>.State taking_medicine;

	// Token: 0x04005865 RID: 22629
	public GameStateMachine<SneezeMonitor, SneezeMonitor.Instance, IStateMachineTarget, object>.State sneezy;

	// Token: 0x04005866 RID: 22630
	public const float SINGLE_SNEEZE_TIME_MINOR = 140f;

	// Token: 0x04005867 RID: 22631
	public const float SINGLE_SNEEZE_TIME_MAJOR = 70f;

	// Token: 0x04005868 RID: 22632
	public const float SNEEZE_TIME_VARIANCE = 0.3f;

	// Token: 0x04005869 RID: 22633
	public const float SHORT_SNEEZE_THRESHOLD = 5f;

	// Token: 0x02001636 RID: 5686
	public new class Instance : GameStateMachine<SneezeMonitor, SneezeMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x060075A3 RID: 30115 RVA: 0x00315EC4 File Offset: 0x003140C4
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.sneezyness = Db.Get().Attributes.Sneezyness.Lookup(master.gameObject);
			this.OnSneezyChange();
			AttributeInstance attributeInstance = this.sneezyness;
			attributeInstance.OnDirty = (System.Action)Delegate.Combine(attributeInstance.OnDirty, new System.Action(this.OnSneezyChange));
		}

		// Token: 0x060075A4 RID: 30116 RVA: 0x000F1C76 File Offset: 0x000EFE76
		public override void StopSM(string reason)
		{
			AttributeInstance attributeInstance = this.sneezyness;
			attributeInstance.OnDirty = (System.Action)Delegate.Remove(attributeInstance.OnDirty, new System.Action(this.OnSneezyChange));
			base.StopSM(reason);
		}

		// Token: 0x060075A5 RID: 30117 RVA: 0x00315F28 File Offset: 0x00314128
		public float NextSneezeInterval()
		{
			if (this.sneezyness.GetTotalValue() <= 0f)
			{
				return 70f;
			}
			float num = (this.IsMinorSneeze() ? 140f : 70f) / this.sneezyness.GetTotalValue();
			return UnityEngine.Random.Range(num * 0.7f, num * 1.3f);
		}

		// Token: 0x060075A6 RID: 30118 RVA: 0x000F1CA6 File Offset: 0x000EFEA6
		public bool IsMinorSneeze()
		{
			return this.sneezyness.GetTotalValue() <= 5f;
		}

		// Token: 0x060075A7 RID: 30119 RVA: 0x000F1CBD File Offset: 0x000EFEBD
		private void OnSneezyChange()
		{
			base.smi.sm.isSneezy.Set(this.sneezyness.GetTotalValue() > 0f, base.smi, false);
		}

		// Token: 0x060075A8 RID: 30120 RVA: 0x00315F84 File Offset: 0x00314184
		public Reactable GetReactable()
		{
			float localCooldown = this.NextSneezeInterval();
			SelfEmoteReactable selfEmoteReactable = new SelfEmoteReactable(base.master.gameObject, "Sneeze", Db.Get().ChoreTypes.Cough, 0f, localCooldown, float.PositiveInfinity, 0f);
			string s = "sneeze";
			string s2 = "sneeze_pst";
			Emote emote = Db.Get().Emotes.Minion.Sneeze;
			if (this.IsMinorSneeze())
			{
				s = "sneeze_short";
				s2 = "sneeze_short_pst";
				emote = Db.Get().Emotes.Minion.Sneeze_Short;
			}
			selfEmoteReactable.SetEmote(emote);
			return selfEmoteReactable.RegisterEmoteStepCallbacks(s, new Action<GameObject>(this.TriggerDisurbance), null).RegisterEmoteStepCallbacks(s2, null, new Action<GameObject>(this.ResetSneeze));
		}

		// Token: 0x060075A9 RID: 30121 RVA: 0x000F1CEE File Offset: 0x000EFEEE
		private void TriggerDisurbance(GameObject go)
		{
			if (this.IsMinorSneeze())
			{
				AcousticDisturbance.Emit(go, 2);
				return;
			}
			AcousticDisturbance.Emit(go, 3);
		}

		// Token: 0x060075AA RID: 30122 RVA: 0x000F1D07 File Offset: 0x000EFF07
		private void ResetSneeze(GameObject go)
		{
			base.smi.GoTo(base.sm.idle);
		}

		// Token: 0x0400586A RID: 22634
		private AttributeInstance sneezyness;

		// Token: 0x0400586B RID: 22635
		private StatusItem statusItem;
	}
}
