using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200010A RID: 266
public class AttackStates : GameStateMachine<AttackStates, AttackStates.Instance, IStateMachineTarget, AttackStates.Def>
{
	// Token: 0x06000418 RID: 1048 RVA: 0x0015DF70 File Offset: 0x0015C170
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.waitBeforeAttack;
		this.root.Enter("SetTarget", delegate(AttackStates.Instance smi)
		{
			this.target.Set(smi.GetSMI<ThreatMonitor.Instance>().MainThreat, smi, false);
			this.cellOffsets = smi.def.cellOffsets;
		});
		this.waitBeforeAttack.ScheduleGoTo((AttackStates.Instance smi) => UnityEngine.Random.Range(0f, 4f), this.approach);
		GameStateMachine<AttackStates, AttackStates.Instance, IStateMachineTarget, AttackStates.Def>.State state = this.approach.InitializeStates(this.masterTarget, this.target, this.attack, null, this.cellOffsets, null);
		string name = CREATURES.STATUSITEMS.ATTACK_APPROACH.NAME;
		string tooltip = CREATURES.STATUSITEMS.ATTACK_APPROACH.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		GameStateMachine<AttackStates, AttackStates.Instance, IStateMachineTarget, AttackStates.Def>.State state2 = this.attack.DefaultState(this.attack.pre);
		string name2 = CREATURES.STATUSITEMS.ATTACK.NAME;
		string tooltip2 = CREATURES.STATUSITEMS.ATTACK.TOOLTIP;
		string icon2 = "";
		StatusItem.IconType icon_type2 = StatusItem.IconType.Info;
		NotificationType notification_type2 = NotificationType.Neutral;
		bool allow_multiples2 = false;
		main = Db.Get().StatusItemCategories.Main;
		state2.ToggleStatusItem(name2, tooltip2, icon2, icon_type2, notification_type2, allow_multiples2, default(HashedString), 129022, null, null, main);
		this.attack.pre.PlayAnim((AttackStates.Instance smi) => smi.def.preAnim, KAnim.PlayMode.Once).Exit(delegate(AttackStates.Instance smi)
		{
			smi.GetComponent<Weapon>().AttackTarget(this.target.Get(smi));
		}).OnAnimQueueComplete(this.attack.pst);
		this.attack.pst.PlayAnim((AttackStates.Instance smi) => smi.def.pstAnim, KAnim.PlayMode.Once).OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.Attack, false);
	}

	// Token: 0x040002EA RID: 746
	public StateMachine<AttackStates, AttackStates.Instance, IStateMachineTarget, AttackStates.Def>.TargetParameter target;

	// Token: 0x040002EB RID: 747
	public GameStateMachine<AttackStates, AttackStates.Instance, IStateMachineTarget, AttackStates.Def>.ApproachSubState<AttackableBase> approach;

	// Token: 0x040002EC RID: 748
	public CellOffset[] cellOffsets;

	// Token: 0x040002ED RID: 749
	public GameStateMachine<AttackStates, AttackStates.Instance, IStateMachineTarget, AttackStates.Def>.State waitBeforeAttack;

	// Token: 0x040002EE RID: 750
	public AttackStates.AttackingStates attack;

	// Token: 0x040002EF RID: 751
	public GameStateMachine<AttackStates, AttackStates.Instance, IStateMachineTarget, AttackStates.Def>.State behaviourcomplete;

	// Token: 0x0200010B RID: 267
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0600041C RID: 1052 RVA: 0x0015E134 File Offset: 0x0015C334
		public Def(string pre_anim = "eat_pre", string pst_anim = "eat_pst", CellOffset[] cell_offsets = null)
		{
			this.preAnim = pre_anim;
			this.pstAnim = pst_anim;
			if (cell_offsets != null)
			{
				this.cellOffsets = cell_offsets;
			}
		}

		// Token: 0x040002F0 RID: 752
		public string preAnim;

		// Token: 0x040002F1 RID: 753
		public string pstAnim;

		// Token: 0x040002F2 RID: 754
		public CellOffset[] cellOffsets = new CellOffset[]
		{
			new CellOffset(0, 0),
			new CellOffset(1, 0),
			new CellOffset(-1, 0),
			new CellOffset(1, 1),
			new CellOffset(-1, 1)
		};
	}

	// Token: 0x0200010C RID: 268
	public class AttackingStates : GameStateMachine<AttackStates, AttackStates.Instance, IStateMachineTarget, AttackStates.Def>.State
	{
		// Token: 0x040002F3 RID: 755
		public GameStateMachine<AttackStates, AttackStates.Instance, IStateMachineTarget, AttackStates.Def>.State pre;

		// Token: 0x040002F4 RID: 756
		public GameStateMachine<AttackStates, AttackStates.Instance, IStateMachineTarget, AttackStates.Def>.State pst;
	}

	// Token: 0x0200010D RID: 269
	public new class Instance : GameStateMachine<AttackStates, AttackStates.Instance, IStateMachineTarget, AttackStates.Def>.GameInstance
	{
		// Token: 0x0600041E RID: 1054 RVA: 0x000AB6A0 File Offset: 0x000A98A0
		public Instance(Chore<AttackStates.Instance> chore, AttackStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Attack);
		}
	}
}
