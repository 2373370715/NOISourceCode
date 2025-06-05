using System;
using Klei.AI;
using STRINGS;
using TUNING;

// Token: 0x02001297 RID: 4759
[SkipSaveFileSerialization]
public class EarlyBird : StateMachineComponent<EarlyBird.StatesInstance>
{
	// Token: 0x06006133 RID: 24883 RVA: 0x002BEC3C File Offset: 0x002BCE3C
	protected override void OnSpawn()
	{
		this.attributeModifiers = new AttributeModifier[]
		{
			new AttributeModifier("Construction", TRAITS.EARLYBIRD_MODIFIER, DUPLICANTS.TRAITS.EARLYBIRD.NAME, false, false, true),
			new AttributeModifier("Digging", TRAITS.EARLYBIRD_MODIFIER, DUPLICANTS.TRAITS.EARLYBIRD.NAME, false, false, true),
			new AttributeModifier("Machinery", TRAITS.EARLYBIRD_MODIFIER, DUPLICANTS.TRAITS.EARLYBIRD.NAME, false, false, true),
			new AttributeModifier("Athletics", TRAITS.EARLYBIRD_MODIFIER, DUPLICANTS.TRAITS.EARLYBIRD.NAME, false, false, true),
			new AttributeModifier("Learning", TRAITS.EARLYBIRD_MODIFIER, DUPLICANTS.TRAITS.EARLYBIRD.NAME, false, false, true),
			new AttributeModifier("Cooking", TRAITS.EARLYBIRD_MODIFIER, DUPLICANTS.TRAITS.EARLYBIRD.NAME, false, false, true),
			new AttributeModifier("Art", TRAITS.EARLYBIRD_MODIFIER, DUPLICANTS.TRAITS.EARLYBIRD.NAME, false, false, true),
			new AttributeModifier("Strength", TRAITS.EARLYBIRD_MODIFIER, DUPLICANTS.TRAITS.EARLYBIRD.NAME, false, false, true),
			new AttributeModifier("Caring", TRAITS.EARLYBIRD_MODIFIER, DUPLICANTS.TRAITS.EARLYBIRD.NAME, false, false, true),
			new AttributeModifier("Botanist", TRAITS.EARLYBIRD_MODIFIER, DUPLICANTS.TRAITS.EARLYBIRD.NAME, false, false, true),
			new AttributeModifier("Ranching", TRAITS.EARLYBIRD_MODIFIER, DUPLICANTS.TRAITS.EARLYBIRD.NAME, false, false, true)
		};
		base.smi.StartSM();
	}

	// Token: 0x06006134 RID: 24884 RVA: 0x002BEDB8 File Offset: 0x002BCFB8
	public void ApplyModifiers()
	{
		Attributes attributes = base.gameObject.GetAttributes();
		for (int i = 0; i < this.attributeModifiers.Length; i++)
		{
			AttributeModifier modifier = this.attributeModifiers[i];
			attributes.Add(modifier);
		}
	}

	// Token: 0x06006135 RID: 24885 RVA: 0x002BEDF4 File Offset: 0x002BCFF4
	public void RemoveModifiers()
	{
		Attributes attributes = base.gameObject.GetAttributes();
		for (int i = 0; i < this.attributeModifiers.Length; i++)
		{
			AttributeModifier modifier = this.attributeModifiers[i];
			attributes.Remove(modifier);
		}
	}

	// Token: 0x04004576 RID: 17782
	[MyCmpReq]
	private KPrefabID kPrefabID;

	// Token: 0x04004577 RID: 17783
	private AttributeModifier[] attributeModifiers;

	// Token: 0x02001298 RID: 4760
	public class StatesInstance : GameStateMachine<EarlyBird.States, EarlyBird.StatesInstance, EarlyBird, object>.GameInstance
	{
		// Token: 0x06006137 RID: 24887 RVA: 0x000E3C1F File Offset: 0x000E1E1F
		public StatesInstance(EarlyBird master) : base(master)
		{
		}

		// Token: 0x06006138 RID: 24888 RVA: 0x002BEE30 File Offset: 0x002BD030
		public bool IsMorning()
		{
			return !(ScheduleManager.Instance == null) && !(base.master.kPrefabID.PrefabTag == GameTags.MinionSelectPreview) && Math.Min((int)(GameClock.Instance.GetCurrentCycleAsPercentage() * 24f), 23) < TRAITS.EARLYBIRD_SCHEDULEBLOCK;
		}
	}

	// Token: 0x02001299 RID: 4761
	public class States : GameStateMachine<EarlyBird.States, EarlyBird.StatesInstance, EarlyBird>
	{
		// Token: 0x06006139 RID: 24889 RVA: 0x002BEE88 File Offset: 0x002BD088
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.root.TagTransition(GameTags.Dead, null, false);
			this.idle.Transition(this.early, (EarlyBird.StatesInstance smi) => smi.IsMorning(), UpdateRate.SIM_200ms);
			this.early.Enter("Morning", delegate(EarlyBird.StatesInstance smi)
			{
				smi.master.ApplyModifiers();
			}).Exit("NotMorning", delegate(EarlyBird.StatesInstance smi)
			{
				smi.master.RemoveModifiers();
			}).ToggleStatusItem(Db.Get().DuplicantStatusItems.EarlyMorning, null).ToggleExpression(Db.Get().Expressions.Happy, null).Transition(this.idle, (EarlyBird.StatesInstance smi) => !smi.IsMorning(), UpdateRate.SIM_200ms);
		}

		// Token: 0x04004578 RID: 17784
		public GameStateMachine<EarlyBird.States, EarlyBird.StatesInstance, EarlyBird, object>.State idle;

		// Token: 0x04004579 RID: 17785
		public GameStateMachine<EarlyBird.States, EarlyBird.StatesInstance, EarlyBird, object>.State early;
	}
}
