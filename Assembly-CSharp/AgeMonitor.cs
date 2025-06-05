using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x0200115B RID: 4443
public class AgeMonitor : GameStateMachine<AgeMonitor, AgeMonitor.Instance, IStateMachineTarget, AgeMonitor.Def>
{
	// Token: 0x06005AB6 RID: 23222 RVA: 0x002A43DC File Offset: 0x002A25DC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.alive;
		this.alive.ToggleAttributeModifier("Aging", (AgeMonitor.Instance smi) => this.aging, null).Transition(this.time_to_die, new StateMachine<AgeMonitor, AgeMonitor.Instance, IStateMachineTarget, AgeMonitor.Def>.Transition.ConditionCallback(AgeMonitor.TimeToDie), UpdateRate.SIM_1000ms).Update(new Action<AgeMonitor.Instance, float>(AgeMonitor.UpdateOldStatusItem), UpdateRate.SIM_1000ms, false);
		this.time_to_die.Enter(new StateMachine<AgeMonitor, AgeMonitor.Instance, IStateMachineTarget, AgeMonitor.Def>.State.Callback(AgeMonitor.Die));
		this.aging = new AttributeModifier(Db.Get().Amounts.Age.deltaAttribute.Id, 0.0016666667f, CREATURES.MODIFIERS.AGE.NAME, false, false, true);
	}

	// Token: 0x06005AB7 RID: 23223 RVA: 0x000DF764 File Offset: 0x000DD964
	private static void Die(AgeMonitor.Instance smi)
	{
		smi.GetSMI<DeathMonitor.Instance>().Kill(Db.Get().Deaths.Generic);
	}

	// Token: 0x06005AB8 RID: 23224 RVA: 0x000DF780 File Offset: 0x000DD980
	private static bool TimeToDie(AgeMonitor.Instance smi)
	{
		return smi.age.value >= smi.age.GetMax();
	}

	// Token: 0x06005AB9 RID: 23225 RVA: 0x002A4488 File Offset: 0x002A2688
	private static void UpdateOldStatusItem(AgeMonitor.Instance smi, float dt)
	{
		bool show = smi.age.value > smi.age.GetMax() * 0.9f;
		smi.oldStatusGuid = smi.kselectable.ToggleStatusItem(Db.Get().CreatureStatusItems.Old, smi.oldStatusGuid, show, smi);
	}

	// Token: 0x04004094 RID: 16532
	public GameStateMachine<AgeMonitor, AgeMonitor.Instance, IStateMachineTarget, AgeMonitor.Def>.State alive;

	// Token: 0x04004095 RID: 16533
	public GameStateMachine<AgeMonitor, AgeMonitor.Instance, IStateMachineTarget, AgeMonitor.Def>.State time_to_die;

	// Token: 0x04004096 RID: 16534
	private AttributeModifier aging;

	// Token: 0x0200115C RID: 4444
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x06005ABC RID: 23228 RVA: 0x000DF7AD File Offset: 0x000DD9AD
		public override void Configure(GameObject prefab)
		{
			prefab.AddOrGet<Modifiers>().initialAmounts.Add(Db.Get().Amounts.Age.Id);
		}

		// Token: 0x04004097 RID: 16535
		public float maxAgePercentOnSpawn = 0.75f;
	}

	// Token: 0x0200115D RID: 4445
	public new class Instance : GameStateMachine<AgeMonitor, AgeMonitor.Instance, IStateMachineTarget, AgeMonitor.Def>.GameInstance
	{
		// Token: 0x06005ABE RID: 23230 RVA: 0x002A44DC File Offset: 0x002A26DC
		public Instance(IStateMachineTarget master, AgeMonitor.Def def) : base(master, def)
		{
			this.age = Db.Get().Amounts.Age.Lookup(base.gameObject);
			base.Subscribe(1119167081, delegate(object data)
			{
				this.RandomizeAge();
			});
		}

		// Token: 0x06005ABF RID: 23231 RVA: 0x002A4528 File Offset: 0x002A2728
		public void RandomizeAge()
		{
			this.age.value = UnityEngine.Random.value * this.age.GetMax() * base.def.maxAgePercentOnSpawn;
			AmountInstance amountInstance = Db.Get().Amounts.Fertility.Lookup(base.gameObject);
			if (amountInstance != null)
			{
				amountInstance.value = this.age.value / this.age.GetMax() * amountInstance.GetMax() * 1.75f;
				amountInstance.value = Mathf.Min(amountInstance.value, amountInstance.GetMax() * 0.9f);
			}
		}

		// Token: 0x1700056B RID: 1387
		// (get) Token: 0x06005AC0 RID: 23232 RVA: 0x000DF7E6 File Offset: 0x000DD9E6
		public float CyclesUntilDeath
		{
			get
			{
				return this.age.GetMax() - this.age.value;
			}
		}

		// Token: 0x04004098 RID: 16536
		public AmountInstance age;

		// Token: 0x04004099 RID: 16537
		public Guid oldStatusGuid;

		// Token: 0x0400409A RID: 16538
		[MyCmpReq]
		public KSelectable kselectable;
	}
}
