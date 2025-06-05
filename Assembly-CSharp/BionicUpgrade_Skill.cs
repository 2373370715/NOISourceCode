using System;

// Token: 0x02000C93 RID: 3219
public class BionicUpgrade_Skill : GameStateMachine<BionicUpgrade_Skill, BionicUpgrade_Skill.Instance, IStateMachineTarget, BionicUpgrade_Skill.Def>
{
	// Token: 0x06003D19 RID: 15641 RVA: 0x000CBF66 File Offset: 0x000CA166
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.root;
		this.root.Enter(new StateMachine<BionicUpgrade_Skill, BionicUpgrade_Skill.Instance, IStateMachineTarget, BionicUpgrade_Skill.Def>.State.Callback(BionicUpgrade_Skill.EnableEffect)).Exit(new StateMachine<BionicUpgrade_Skill, BionicUpgrade_Skill.Instance, IStateMachineTarget, BionicUpgrade_Skill.Def>.State.Callback(BionicUpgrade_Skill.DisableEffect));
	}

	// Token: 0x06003D1A RID: 15642 RVA: 0x000CBFA0 File Offset: 0x000CA1A0
	public static void EnableEffect(BionicUpgrade_Skill.Instance smi)
	{
		smi.ApplySkill();
	}

	// Token: 0x06003D1B RID: 15643 RVA: 0x000CBFA8 File Offset: 0x000CA1A8
	public static void DisableEffect(BionicUpgrade_Skill.Instance smi)
	{
		smi.RemoveSkill();
	}

	// Token: 0x02000C94 RID: 3220
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04002A40 RID: 10816
		public string SKILL_ID;
	}

	// Token: 0x02000C95 RID: 3221
	public new class Instance : GameStateMachine<BionicUpgrade_Skill, BionicUpgrade_Skill.Instance, IStateMachineTarget, BionicUpgrade_Skill.Def>.GameInstance
	{
		// Token: 0x06003D1E RID: 15646 RVA: 0x000CBFB8 File Offset: 0x000CA1B8
		public Instance(IStateMachineTarget master, BionicUpgrade_Skill.Def def) : base(master, def)
		{
			this.resume = base.GetComponent<MinionResume>();
		}

		// Token: 0x06003D1F RID: 15647 RVA: 0x000CBFCE File Offset: 0x000CA1CE
		public void ApplySkill()
		{
			this.resume.GrantSkill(base.def.SKILL_ID);
		}

		// Token: 0x06003D20 RID: 15648 RVA: 0x000CBFE6 File Offset: 0x000CA1E6
		public void RemoveSkill()
		{
			this.resume.UngrantSkill(base.def.SKILL_ID);
		}

		// Token: 0x04002A41 RID: 10817
		private MinionResume resume;
	}
}
