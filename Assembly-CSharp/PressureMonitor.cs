using System;
using Klei.AI;

// Token: 0x020015FE RID: 5630
public class PressureMonitor : GameStateMachine<PressureMonitor, PressureMonitor.Instance, IStateMachineTarget, PressureMonitor.Def>
{
	// Token: 0x060074B5 RID: 29877 RVA: 0x00312F84 File Offset: 0x00311184
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.safe;
		this.safe.Transition(this.inPressure, new StateMachine<PressureMonitor, PressureMonitor.Instance, IStateMachineTarget, PressureMonitor.Def>.Transition.ConditionCallback(PressureMonitor.IsInPressureGas), UpdateRate.SIM_200ms);
		this.inPressure.Transition(this.safe, GameStateMachine<PressureMonitor, PressureMonitor.Instance, IStateMachineTarget, PressureMonitor.Def>.Not(new StateMachine<PressureMonitor, PressureMonitor.Instance, IStateMachineTarget, PressureMonitor.Def>.Transition.ConditionCallback(PressureMonitor.IsInPressureGas)), UpdateRate.SIM_200ms).DefaultState(this.inPressure.idle);
		this.inPressure.idle.EventTransition(GameHashes.EffectImmunityAdded, this.inPressure.immune, new StateMachine<PressureMonitor, PressureMonitor.Instance, IStateMachineTarget, PressureMonitor.Def>.Transition.ConditionCallback(PressureMonitor.IsImmuneToPressure)).Update(new Action<PressureMonitor.Instance, float>(PressureMonitor.HighPressureUpdate), UpdateRate.SIM_200ms, false);
		this.inPressure.immune.EventTransition(GameHashes.EffectImmunityRemoved, this.inPressure.idle, GameStateMachine<PressureMonitor, PressureMonitor.Instance, IStateMachineTarget, PressureMonitor.Def>.Not(new StateMachine<PressureMonitor, PressureMonitor.Instance, IStateMachineTarget, PressureMonitor.Def>.Transition.ConditionCallback(PressureMonitor.IsImmuneToPressure)));
	}

	// Token: 0x060074B6 RID: 29878 RVA: 0x000F1201 File Offset: 0x000EF401
	public static bool IsInPressureGas(PressureMonitor.Instance smi)
	{
		return smi.IsInHighPressure();
	}

	// Token: 0x060074B7 RID: 29879 RVA: 0x000F1209 File Offset: 0x000EF409
	public static bool IsImmuneToPressure(PressureMonitor.Instance smi)
	{
		return smi.IsImmuneToHighPressure();
	}

	// Token: 0x060074B8 RID: 29880 RVA: 0x000F1211 File Offset: 0x000EF411
	public static void RemoveOverpressureEffect(PressureMonitor.Instance smi)
	{
		smi.RemoveEffect();
	}

	// Token: 0x060074B9 RID: 29881 RVA: 0x000F1219 File Offset: 0x000EF419
	public static void HighPressureUpdate(PressureMonitor.Instance smi, float dt)
	{
		if (smi.timeinstate > 3f)
		{
			smi.AddEffect();
		}
	}

	// Token: 0x04005796 RID: 22422
	public const string OVER_PRESSURE_EFFECT_NAME = "PoppedEarDrums";

	// Token: 0x04005797 RID: 22423
	public const float TIME_IN_PRESSURE_BEFORE_EAR_POPS = 3f;

	// Token: 0x04005798 RID: 22424
	private static CellOffset[] PRESSURE_TEST_OFFSET = new CellOffset[]
	{
		new CellOffset(0, 0),
		new CellOffset(0, 1)
	};

	// Token: 0x04005799 RID: 22425
	public GameStateMachine<PressureMonitor, PressureMonitor.Instance, IStateMachineTarget, PressureMonitor.Def>.State safe;

	// Token: 0x0400579A RID: 22426
	public PressureMonitor.PressureStates inPressure;

	// Token: 0x020015FF RID: 5631
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001600 RID: 5632
	public class PressureStates : GameStateMachine<PressureMonitor, PressureMonitor.Instance, IStateMachineTarget, PressureMonitor.Def>.State
	{
		// Token: 0x0400579B RID: 22427
		public GameStateMachine<PressureMonitor, PressureMonitor.Instance, IStateMachineTarget, PressureMonitor.Def>.State idle;

		// Token: 0x0400579C RID: 22428
		public GameStateMachine<PressureMonitor, PressureMonitor.Instance, IStateMachineTarget, PressureMonitor.Def>.State immune;
	}

	// Token: 0x02001601 RID: 5633
	public new class Instance : GameStateMachine<PressureMonitor, PressureMonitor.Instance, IStateMachineTarget, PressureMonitor.Def>.GameInstance
	{
		// Token: 0x060074BE RID: 29886 RVA: 0x000F1267 File Offset: 0x000EF467
		public Instance(IStateMachineTarget master, PressureMonitor.Def def) : base(master, def)
		{
			this.effects = base.GetComponent<Effects>();
		}

		// Token: 0x060074BF RID: 29887 RVA: 0x000F127D File Offset: 0x000EF47D
		public bool IsImmuneToHighPressure()
		{
			return this.effects.HasImmunityTo(Db.Get().effects.Get("PoppedEarDrums"));
		}

		// Token: 0x060074C0 RID: 29888 RVA: 0x00313068 File Offset: 0x00311268
		public bool IsInHighPressure()
		{
			int cell = Grid.PosToCell(base.gameObject);
			for (int i = 0; i < PressureMonitor.PRESSURE_TEST_OFFSET.Length; i++)
			{
				int num = Grid.OffsetCell(cell, PressureMonitor.PRESSURE_TEST_OFFSET[i]);
				if (Grid.IsValidCell(num) && Grid.Element[num].IsGas && Grid.Mass[num] > 4f)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060074C1 RID: 29889 RVA: 0x000F129E File Offset: 0x000EF49E
		public void RemoveEffect()
		{
			this.effects.Remove("PoppedEarDrums");
		}

		// Token: 0x060074C2 RID: 29890 RVA: 0x000F12B0 File Offset: 0x000EF4B0
		public void AddEffect()
		{
			this.effects.Add("PoppedEarDrums", true);
		}

		// Token: 0x0400579D RID: 22429
		private Effects effects;
	}
}
