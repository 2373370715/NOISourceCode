using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x0200162F RID: 5679
public class SlipperyMonitor : GameStateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>
{
	// Token: 0x0600758D RID: 30093 RVA: 0x00315944 File Offset: 0x00313B44
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.safe;
		this.safe.EventTransition(GameHashes.NavigationCellChanged, this.unsafeCell, new StateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.Transition.ConditionCallback(SlipperyMonitor.IsStandingOnASlipperyCell));
		this.unsafeCell.EventTransition(GameHashes.NavigationCellChanged, this.safe, GameStateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.Not(new StateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.Transition.ConditionCallback(SlipperyMonitor.IsStandingOnASlipperyCell))).DefaultState(this.unsafeCell.atRisk);
		this.unsafeCell.atRisk.EventTransition(GameHashes.EquipmentChanged, this.unsafeCell.immune, new StateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.Transition.ConditionCallback(this.IsImmuneToSlipperySurfaces)).EventTransition(GameHashes.EffectAdded, this.unsafeCell.immune, new StateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.Transition.ConditionCallback(this.IsImmuneToSlipperySurfaces)).DefaultState(this.unsafeCell.atRisk.idle);
		this.unsafeCell.atRisk.idle.EventHandlerTransition(GameHashes.NavigationCellChanged, this.unsafeCell.atRisk.slip, new Func<SlipperyMonitor.Instance, object, bool>(SlipperyMonitor.RollDTwenty));
		this.unsafeCell.atRisk.slip.ToggleReactable(new Func<SlipperyMonitor.Instance, Reactable>(this.GetReactable)).ScheduleGoTo(8f, this.unsafeCell.atRisk.idle);
		this.unsafeCell.immune.EventTransition(GameHashes.EquipmentChanged, this.unsafeCell.atRisk, GameStateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.Not(new StateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.Transition.ConditionCallback(this.IsImmuneToSlipperySurfaces))).EventTransition(GameHashes.EffectRemoved, this.unsafeCell.atRisk, GameStateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.Not(new StateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.Transition.ConditionCallback(this.IsImmuneToSlipperySurfaces)));
	}

	// Token: 0x0600758E RID: 30094 RVA: 0x000F1B92 File Offset: 0x000EFD92
	public bool IsImmuneToSlipperySurfaces(SlipperyMonitor.Instance smi)
	{
		return smi.IsImmune;
	}

	// Token: 0x0600758F RID: 30095 RVA: 0x000F1B9A File Offset: 0x000EFD9A
	public Reactable GetReactable(SlipperyMonitor.Instance smi)
	{
		return smi.CreateReactable();
	}

	// Token: 0x06007590 RID: 30096 RVA: 0x00315AEC File Offset: 0x00313CEC
	private static bool IsStandingOnASlipperyCell(SlipperyMonitor.Instance smi)
	{
		int num = Grid.PosToCell(smi);
		int num2 = Grid.OffsetCell(num, 0, -1);
		return (Grid.IsValidCell(num) && Grid.Element[num].IsSlippery) || (Grid.IsValidCell(num2) && Grid.Element[num2].IsSolid && Grid.Element[num2].IsSlippery);
	}

	// Token: 0x06007591 RID: 30097 RVA: 0x000F1BA2 File Offset: 0x000EFDA2
	private static bool RollDTwenty(SlipperyMonitor.Instance smi, object o)
	{
		return UnityEngine.Random.value <= 0.05f;
	}

	// Token: 0x04005852 RID: 22610
	public const string EFFECT_NAME = "RecentlySlippedTracker";

	// Token: 0x04005853 RID: 22611
	public const float SLIP_FAIL_TIMEOUT = 8f;

	// Token: 0x04005854 RID: 22612
	public const float PROBABILITY_OF_SLIP = 0.05f;

	// Token: 0x04005855 RID: 22613
	public const float STRESS_DAMAGE = 3f;

	// Token: 0x04005856 RID: 22614
	public GameStateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.State safe;

	// Token: 0x04005857 RID: 22615
	public SlipperyMonitor.UnsafeCellState unsafeCell;

	// Token: 0x02001630 RID: 5680
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001631 RID: 5681
	public class UnsafeCellState : GameStateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.State
	{
		// Token: 0x04005858 RID: 22616
		public SlipperyMonitor.RiskStates atRisk;

		// Token: 0x04005859 RID: 22617
		public GameStateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.State immune;
	}

	// Token: 0x02001632 RID: 5682
	public class RiskStates : GameStateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.State
	{
		// Token: 0x0400585A RID: 22618
		public GameStateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.State idle;

		// Token: 0x0400585B RID: 22619
		public GameStateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.State slip;
	}

	// Token: 0x02001633 RID: 5683
	public new class Instance : GameStateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.GameInstance
	{
		// Token: 0x1700077A RID: 1914
		// (get) Token: 0x06007596 RID: 30102 RVA: 0x000F1BC3 File Offset: 0x000EFDC3
		public bool IsImmune
		{
			get
			{
				return this.effects.HasEffect("RecentlySlippedTracker") || this.effects.HasImmunityTo(this.effect);
			}
		}

		// Token: 0x06007597 RID: 30103 RVA: 0x000F1BEA File Offset: 0x000EFDEA
		public Instance(IStateMachineTarget master, SlipperyMonitor.Def def) : base(master, def)
		{
			this.effects = base.GetComponent<Effects>();
			this.effect = Db.Get().effects.Get("RecentlySlippedTracker");
		}

		// Token: 0x06007598 RID: 30104 RVA: 0x000F1C1A File Offset: 0x000EFE1A
		public SlipperyMonitor.SlipReactable CreateReactable()
		{
			return new SlipperyMonitor.SlipReactable(this);
		}

		// Token: 0x0400585C RID: 22620
		private Effect effect;

		// Token: 0x0400585D RID: 22621
		public Effects effects;
	}

	// Token: 0x02001634 RID: 5684
	public class SlipReactable : Reactable
	{
		// Token: 0x06007599 RID: 30105 RVA: 0x00315B50 File Offset: 0x00313D50
		public SlipReactable(SlipperyMonitor.Instance _smi) : base(_smi.gameObject, "Slip", Db.Get().ChoreTypes.Slip, 1, 1, false, 0f, 0f, 8f, 0f, ObjectLayer.NumLayers)
		{
			this.smi = _smi;
		}

		// Token: 0x0600759A RID: 30106 RVA: 0x00315BA4 File Offset: 0x00313DA4
		public override bool InternalCanBegin(GameObject new_reactor, Navigator.ActiveTransition transition)
		{
			if (this.reactor != null)
			{
				return false;
			}
			if (new_reactor == null)
			{
				return false;
			}
			if (this.gameObject != new_reactor)
			{
				return false;
			}
			if (this.smi == null)
			{
				return false;
			}
			Navigator component = new_reactor.GetComponent<Navigator>();
			return !(component == null) && component.CurrentNavType != NavType.Tube && component.CurrentNavType != NavType.Ladder && component.CurrentNavType != NavType.Pole;
		}

		// Token: 0x0600759B RID: 30107 RVA: 0x00315C18 File Offset: 0x00313E18
		protected override void InternalBegin()
		{
			this.startTime = Time.time;
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, DUPLICANTS.MODIFIERS.SLIPPED.NAME, this.gameObject.transform, 1.5f, false);
			KBatchedAnimController component = this.reactor.GetComponent<KBatchedAnimController>();
			component.AddAnimOverrides(Assets.GetAnim("anim_slip_kanim"), 1f);
			component.Play("slip_pre", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue("slip_loop", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue("slip_pst", KAnim.PlayMode.Once, 1f, 0f);
			this.reactor.GetComponent<KSelectable>().AddStatusItem(Db.Get().DuplicantStatusItems.Slippering, null);
		}

		// Token: 0x0600759C RID: 30108 RVA: 0x000F1C22 File Offset: 0x000EFE22
		public override void Update(float dt)
		{
			if (Time.time - this.startTime > 4.3f)
			{
				base.Cleanup();
				this.ApplyStress();
				this.ApplyTrackerEffect();
			}
		}

		// Token: 0x0600759D RID: 30109 RVA: 0x000F1C49 File Offset: 0x000EFE49
		public void ApplyTrackerEffect()
		{
			this.smi.effects.Add("RecentlySlippedTracker", true);
		}

		// Token: 0x0600759E RID: 30110 RVA: 0x00315CF8 File Offset: 0x00313EF8
		private void ApplyStress()
		{
			this.smi.master.gameObject.GetAmounts().Get(Db.Get().Amounts.Stress.Id).ApplyDelta(3f);
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, 3f.ToString() + "% " + Db.Get().Amounts.Stress.Name, this.gameObject.transform, 1.5f, false);
			ReportManager.Instance.ReportValue(ReportManager.ReportType.StressDelta, 3f, DUPLICANTS.MODIFIERS.SLIPPED.NAME, this.gameObject.GetProperName());
		}

		// Token: 0x0600759F RID: 30111 RVA: 0x00315DB4 File Offset: 0x00313FB4
		protected override void InternalEnd()
		{
			if (this.reactor != null)
			{
				KBatchedAnimController component = this.reactor.GetComponent<KBatchedAnimController>();
				if (component != null)
				{
					this.reactor.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().DuplicantStatusItems.Slippering, false);
					component.RemoveAnimOverrides(Assets.GetAnim("anim_slip_kanim"));
				}
			}
		}

		// Token: 0x060075A0 RID: 30112 RVA: 0x000AA038 File Offset: 0x000A8238
		protected override void InternalCleanup()
		{
		}

		// Token: 0x0400585E RID: 22622
		private SlipperyMonitor.Instance smi;

		// Token: 0x0400585F RID: 22623
		private float startTime;

		// Token: 0x04005860 RID: 22624
		private const string ANIM_FILE_NAME = "anim_slip_kanim";

		// Token: 0x04005861 RID: 22625
		private const float DURATION = 4.3f;
	}
}
