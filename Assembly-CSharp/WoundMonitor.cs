using System;

// Token: 0x02001673 RID: 5747
public class WoundMonitor : GameStateMachine<WoundMonitor, WoundMonitor.Instance>
{
	// Token: 0x060076CF RID: 30415 RVA: 0x00319568 File Offset: 0x00317768
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.healthy;
		this.root.ToggleAnims("anim_hits_kanim", 0f).EventHandler(GameHashes.HealthChanged, delegate(WoundMonitor.Instance smi, object data)
		{
			smi.OnHealthChanged(data);
		});
		this.healthy.EventTransition(GameHashes.HealthChanged, this.wounded, (WoundMonitor.Instance smi) => smi.health.State > Health.HealthState.Perfect);
		this.wounded.ToggleUrge(Db.Get().Urges.Heal).Enter(delegate(WoundMonitor.Instance smi)
		{
			switch (smi.health.State)
			{
			case Health.HealthState.Scuffed:
				smi.GoTo(this.wounded.light);
				return;
			case Health.HealthState.Injured:
				smi.GoTo(this.wounded.medium);
				return;
			case Health.HealthState.Critical:
				smi.GoTo(this.wounded.heavy);
				return;
			default:
				return;
			}
		}).EventHandler(GameHashes.HealthChanged, delegate(WoundMonitor.Instance smi)
		{
			smi.GoToProperHeathState();
		});
		this.wounded.medium.ToggleAnims("anim_loco_wounded_kanim", 1f);
		this.wounded.heavy.ToggleAnims("anim_loco_wounded_kanim", 3f).Update("LookForAvailableClinic", delegate(WoundMonitor.Instance smi, float dt)
		{
			smi.FindAvailableMedicalBed();
		}, UpdateRate.SIM_1000ms, false);
	}

	// Token: 0x0400595A RID: 22874
	public GameStateMachine<WoundMonitor, WoundMonitor.Instance, IStateMachineTarget, object>.State healthy;

	// Token: 0x0400595B RID: 22875
	public WoundMonitor.Wounded wounded;

	// Token: 0x02001674 RID: 5748
	public class Wounded : GameStateMachine<WoundMonitor, WoundMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x0400595C RID: 22876
		public GameStateMachine<WoundMonitor, WoundMonitor.Instance, IStateMachineTarget, object>.State light;

		// Token: 0x0400595D RID: 22877
		public GameStateMachine<WoundMonitor, WoundMonitor.Instance, IStateMachineTarget, object>.State medium;

		// Token: 0x0400595E RID: 22878
		public GameStateMachine<WoundMonitor, WoundMonitor.Instance, IStateMachineTarget, object>.State heavy;
	}

	// Token: 0x02001675 RID: 5749
	public new class Instance : GameStateMachine<WoundMonitor, WoundMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x060076D3 RID: 30419 RVA: 0x000F2A59 File Offset: 0x000F0C59
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.health = master.GetComponent<Health>();
			this.worker = master.GetComponent<WorkerBase>();
		}

		// Token: 0x060076D4 RID: 30420 RVA: 0x0031970C File Offset: 0x0031790C
		public void OnHealthChanged(object data)
		{
			float num = (float)data;
			if (this.health.hitPoints != 0f && num < 0f)
			{
				this.PlayHitAnimation();
			}
		}

		// Token: 0x060076D5 RID: 30421 RVA: 0x00319740 File Offset: 0x00317940
		private void PlayHitAnimation()
		{
			string text = null;
			KBatchedAnimController kbatchedAnimController = base.smi.Get<KBatchedAnimController>();
			if (kbatchedAnimController.CurrentAnim != null)
			{
				text = kbatchedAnimController.CurrentAnim.name;
			}
			KAnim.PlayMode playMode = kbatchedAnimController.PlayMode;
			if (text != null)
			{
				if (text.Contains("hit"))
				{
					return;
				}
				if (text.Contains("2_0"))
				{
					return;
				}
				if (text.Contains("2_1"))
				{
					return;
				}
				if (text.Contains("2_-1"))
				{
					return;
				}
				if (text.Contains("2_-2"))
				{
					return;
				}
				if (text.Contains("1_-1"))
				{
					return;
				}
				if (text.Contains("1_-2"))
				{
					return;
				}
				if (text.Contains("1_1"))
				{
					return;
				}
				if (text.Contains("1_2"))
				{
					return;
				}
				if (text.Contains("breathe_"))
				{
					return;
				}
				if (text.Contains("death_"))
				{
					return;
				}
				if (text.Contains("impact"))
				{
					return;
				}
			}
			string s = "hit";
			AttackChore.StatesInstance smi = base.gameObject.GetSMI<AttackChore.StatesInstance>();
			if (smi != null && smi.GetCurrentState() == smi.sm.attack)
			{
				s = smi.master.GetHitAnim();
			}
			if (this.worker.GetComponent<Navigator>().CurrentNavType == NavType.Ladder)
			{
				s = "hit_ladder";
			}
			else if (this.worker.GetComponent<Navigator>().CurrentNavType == NavType.Pole)
			{
				s = "hit_pole";
			}
			kbatchedAnimController.Play(s, KAnim.PlayMode.Once, 1f, 0f);
			if (text != null)
			{
				kbatchedAnimController.Queue(text, playMode, 1f, 0f);
			}
		}

		// Token: 0x060076D6 RID: 30422 RVA: 0x003198C4 File Offset: 0x00317AC4
		public void PlayKnockedOverImpactAnimation()
		{
			string text = null;
			KBatchedAnimController kbatchedAnimController = base.smi.Get<KBatchedAnimController>();
			if (kbatchedAnimController.CurrentAnim != null)
			{
				text = kbatchedAnimController.CurrentAnim.name;
			}
			KAnim.PlayMode playMode = kbatchedAnimController.PlayMode;
			if (text != null)
			{
				if (text.Contains("impact"))
				{
					return;
				}
				if (text.Contains("2_0"))
				{
					return;
				}
				if (text.Contains("2_1"))
				{
					return;
				}
				if (text.Contains("2_-1"))
				{
					return;
				}
				if (text.Contains("2_-2"))
				{
					return;
				}
				if (text.Contains("1_-1"))
				{
					return;
				}
				if (text.Contains("1_-2"))
				{
					return;
				}
				if (text.Contains("1_1"))
				{
					return;
				}
				if (text.Contains("1_2"))
				{
					return;
				}
				if (text.Contains("breathe_"))
				{
					return;
				}
				if (text.Contains("death_"))
				{
					return;
				}
			}
			string s = "impact";
			kbatchedAnimController.Play(s, KAnim.PlayMode.Once, 1f, 0f);
			if (text != null)
			{
				kbatchedAnimController.Queue(text, playMode, 1f, 0f);
			}
		}

		// Token: 0x060076D7 RID: 30423 RVA: 0x003199D4 File Offset: 0x00317BD4
		public void GoToProperHeathState()
		{
			switch (base.smi.health.State)
			{
			case Health.HealthState.Perfect:
				base.smi.GoTo(base.sm.healthy);
				return;
			case Health.HealthState.Alright:
				break;
			case Health.HealthState.Scuffed:
				base.smi.GoTo(base.sm.wounded.light);
				break;
			case Health.HealthState.Injured:
				base.smi.GoTo(base.sm.wounded.medium);
				return;
			case Health.HealthState.Critical:
				base.smi.GoTo(base.sm.wounded.heavy);
				return;
			default:
				return;
			}
		}

		// Token: 0x060076D8 RID: 30424 RVA: 0x000F2A7A File Offset: 0x000F0C7A
		public bool ShouldExitInfirmary()
		{
			return this.health.State == Health.HealthState.Perfect;
		}

		// Token: 0x060076D9 RID: 30425 RVA: 0x00319A78 File Offset: 0x00317C78
		public void FindAvailableMedicalBed()
		{
			AssignableSlot clinic = Db.Get().AssignableSlots.Clinic;
			Ownables soleOwner = base.gameObject.GetComponent<MinionIdentity>().GetSoleOwner();
			if (soleOwner.GetSlot(clinic).assignable == null)
			{
				soleOwner.AutoAssignSlot(clinic);
			}
		}

		// Token: 0x0400595F RID: 22879
		public Health health;

		// Token: 0x04005960 RID: 22880
		private WorkerBase worker;
	}
}
