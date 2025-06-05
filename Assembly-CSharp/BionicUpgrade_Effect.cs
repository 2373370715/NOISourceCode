using System;
using Klei.AI;

// Token: 0x02000C80 RID: 3200
public class BionicUpgrade_Effect : GameStateMachine<BionicUpgrade_Effect, BionicUpgrade_Effect.Instance, IStateMachineTarget, BionicUpgrade_Effect.Def>
{
	// Token: 0x06003CCC RID: 15564 RVA: 0x000CBBDE File Offset: 0x000C9DDE
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.root;
		this.root.Enter(new StateMachine<BionicUpgrade_Effect, BionicUpgrade_Effect.Instance, IStateMachineTarget, BionicUpgrade_Effect.Def>.State.Callback(BionicUpgrade_Effect.EnableEffect)).Exit(new StateMachine<BionicUpgrade_Effect, BionicUpgrade_Effect.Instance, IStateMachineTarget, BionicUpgrade_Effect.Def>.State.Callback(BionicUpgrade_Effect.DisableEffect));
	}

	// Token: 0x06003CCD RID: 15565 RVA: 0x000CBC18 File Offset: 0x000C9E18
	public static void EnableEffect(BionicUpgrade_Effect.Instance smi)
	{
		smi.ApplyEffect();
	}

	// Token: 0x06003CCE RID: 15566 RVA: 0x000CBC20 File Offset: 0x000C9E20
	public static void DisableEffect(BionicUpgrade_Effect.Instance smi)
	{
		smi.RemoveEffect();
	}

	// Token: 0x02000C81 RID: 3201
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04002A23 RID: 10787
		public string EFFECT_NAME;
	}

	// Token: 0x02000C82 RID: 3202
	public new class Instance : GameStateMachine<BionicUpgrade_Effect, BionicUpgrade_Effect.Instance, IStateMachineTarget, BionicUpgrade_Effect.Def>.GameInstance
	{
		// Token: 0x06003CD1 RID: 15569 RVA: 0x000CBC30 File Offset: 0x000C9E30
		public Instance(IStateMachineTarget master, BionicUpgrade_Effect.Def def) : base(master, def)
		{
			this.effects = base.GetComponent<Effects>();
		}

		// Token: 0x06003CD2 RID: 15570 RVA: 0x0023D56C File Offset: 0x0023B76C
		public void ApplyEffect()
		{
			Effect newEffect = Db.Get().effects.Get(base.def.EFFECT_NAME);
			this.effects.Add(newEffect, false);
		}

		// Token: 0x06003CD3 RID: 15571 RVA: 0x0023D5A4 File Offset: 0x0023B7A4
		public void RemoveEffect()
		{
			Effect effect = Db.Get().effects.Get(base.def.EFFECT_NAME);
			this.effects.Remove(effect);
		}

		// Token: 0x04002A24 RID: 10788
		private Effects effects;
	}
}
