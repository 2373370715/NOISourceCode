using System;

// Token: 0x020006BB RID: 1723
public class EquipChore : Chore<EquipChore.StatesInstance>
{
	// Token: 0x06001E98 RID: 7832 RVA: 0x001C0928 File Offset: 0x001BEB28
	public EquipChore(IStateMachineTarget equippable) : base(Db.Get().ChoreTypes.Equip, equippable, null, false, null, null, null, PriorityScreen.PriorityClass.personalNeeds, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new EquipChore.StatesInstance(this);
		base.smi.sm.equippable_source.Set(equippable.gameObject, base.smi, false);
		base.smi.sm.requested_units.Set(1f, base.smi, false);
		this.showAvailabilityInHoverText = false;
		Prioritizable.AddRef(equippable.gameObject);
		Game.Instance.Trigger(1980521255, equippable.gameObject);
		this.AddPrecondition(ChorePreconditions.instance.IsAssignedtoMe, equippable.GetComponent<Assignable>());
		this.AddPrecondition(ChorePreconditions.instance.CanPickup, equippable.GetComponent<Pickupable>());
	}

	// Token: 0x06001E99 RID: 7833 RVA: 0x001C09FC File Offset: 0x001BEBFC
	public override void Begin(Chore.Precondition.Context context)
	{
		if (context.consumerState.consumer == null)
		{
			Debug.LogError("EquipChore null context.consumer");
			return;
		}
		if (base.smi == null)
		{
			Debug.LogError("EquipChore null smi");
			return;
		}
		if (base.smi.sm == null)
		{
			Debug.LogError("EquipChore null smi.sm");
			return;
		}
		if (base.smi.sm.equippable_source == null)
		{
			Debug.LogError("EquipChore null smi.sm.equippable_source");
			return;
		}
		base.smi.sm.equipper.Set(context.consumerState.gameObject, base.smi, false);
		base.Begin(context);
	}

	// Token: 0x020006BC RID: 1724
	public class StatesInstance : GameStateMachine<EquipChore.States, EquipChore.StatesInstance, EquipChore, object>.GameInstance
	{
		// Token: 0x06001E9A RID: 7834 RVA: 0x000B8AA4 File Offset: 0x000B6CA4
		public StatesInstance(EquipChore master) : base(master)
		{
		}
	}

	// Token: 0x020006BD RID: 1725
	public class States : GameStateMachine<EquipChore.States, EquipChore.StatesInstance, EquipChore>
	{
		// Token: 0x06001E9B RID: 7835 RVA: 0x001C0AA0 File Offset: 0x001BECA0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.fetch;
			base.Target(this.equipper);
			this.root.DoNothing();
			this.fetch.InitializeStates(this.equipper, this.equippable_source, this.equippable_result, this.requested_units, this.actual_units, this.equip, null);
			this.equip.ToggleWork<EquippableWorkable>(this.equippable_result, null, null, null);
		}

		// Token: 0x040013F3 RID: 5107
		public StateMachine<EquipChore.States, EquipChore.StatesInstance, EquipChore, object>.TargetParameter equipper;

		// Token: 0x040013F4 RID: 5108
		public StateMachine<EquipChore.States, EquipChore.StatesInstance, EquipChore, object>.TargetParameter equippable_source;

		// Token: 0x040013F5 RID: 5109
		public StateMachine<EquipChore.States, EquipChore.StatesInstance, EquipChore, object>.TargetParameter equippable_result;

		// Token: 0x040013F6 RID: 5110
		public StateMachine<EquipChore.States, EquipChore.StatesInstance, EquipChore, object>.FloatParameter requested_units;

		// Token: 0x040013F7 RID: 5111
		public StateMachine<EquipChore.States, EquipChore.StatesInstance, EquipChore, object>.FloatParameter actual_units;

		// Token: 0x040013F8 RID: 5112
		public GameStateMachine<EquipChore.States, EquipChore.StatesInstance, EquipChore, object>.FetchSubState fetch;

		// Token: 0x040013F9 RID: 5113
		public EquipChore.States.Equip equip;

		// Token: 0x020006BE RID: 1726
		public class Equip : GameStateMachine<EquipChore.States, EquipChore.StatesInstance, EquipChore, object>.State
		{
			// Token: 0x040013FA RID: 5114
			public GameStateMachine<EquipChore.States, EquipChore.StatesInstance, EquipChore, object>.State pre;

			// Token: 0x040013FB RID: 5115
			public GameStateMachine<EquipChore.States, EquipChore.StatesInstance, EquipChore, object>.State pst;
		}
	}
}
