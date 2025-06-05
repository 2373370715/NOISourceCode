using System;
using Klei.AI;
using STRINGS;
using TUNING;

// Token: 0x020016AF RID: 5807
[SkipSaveFileSerialization]
public class NightOwl : StateMachineComponent<NightOwl.StatesInstance>
{
	// Token: 0x060077B8 RID: 30648 RVA: 0x0031C468 File Offset: 0x0031A668
	protected override void OnSpawn()
	{
		this.attributeModifiers = new AttributeModifier[]
		{
			new AttributeModifier("Construction", TRAITS.NIGHTOWL_MODIFIER, DUPLICANTS.TRAITS.NIGHTOWL.NAME, false, false, true),
			new AttributeModifier("Digging", TRAITS.NIGHTOWL_MODIFIER, DUPLICANTS.TRAITS.NIGHTOWL.NAME, false, false, true),
			new AttributeModifier("Machinery", TRAITS.NIGHTOWL_MODIFIER, DUPLICANTS.TRAITS.NIGHTOWL.NAME, false, false, true),
			new AttributeModifier("Athletics", TRAITS.NIGHTOWL_MODIFIER, DUPLICANTS.TRAITS.NIGHTOWL.NAME, false, false, true),
			new AttributeModifier("Learning", TRAITS.NIGHTOWL_MODIFIER, DUPLICANTS.TRAITS.NIGHTOWL.NAME, false, false, true),
			new AttributeModifier("Cooking", TRAITS.NIGHTOWL_MODIFIER, DUPLICANTS.TRAITS.NIGHTOWL.NAME, false, false, true),
			new AttributeModifier("Art", TRAITS.NIGHTOWL_MODIFIER, DUPLICANTS.TRAITS.NIGHTOWL.NAME, false, false, true),
			new AttributeModifier("Strength", TRAITS.NIGHTOWL_MODIFIER, DUPLICANTS.TRAITS.NIGHTOWL.NAME, false, false, true),
			new AttributeModifier("Caring", TRAITS.NIGHTOWL_MODIFIER, DUPLICANTS.TRAITS.NIGHTOWL.NAME, false, false, true),
			new AttributeModifier("Botanist", TRAITS.NIGHTOWL_MODIFIER, DUPLICANTS.TRAITS.NIGHTOWL.NAME, false, false, true),
			new AttributeModifier("Ranching", TRAITS.NIGHTOWL_MODIFIER, DUPLICANTS.TRAITS.NIGHTOWL.NAME, false, false, true)
		};
		base.smi.StartSM();
	}

	// Token: 0x060077B9 RID: 30649 RVA: 0x0031C5E4 File Offset: 0x0031A7E4
	public void ApplyModifiers()
	{
		Attributes attributes = base.gameObject.GetAttributes();
		for (int i = 0; i < this.attributeModifiers.Length; i++)
		{
			AttributeModifier modifier = this.attributeModifiers[i];
			attributes.Add(modifier);
		}
	}

	// Token: 0x060077BA RID: 30650 RVA: 0x0031C620 File Offset: 0x0031A820
	public void RemoveModifiers()
	{
		Attributes attributes = base.gameObject.GetAttributes();
		for (int i = 0; i < this.attributeModifiers.Length; i++)
		{
			AttributeModifier modifier = this.attributeModifiers[i];
			attributes.Remove(modifier);
		}
	}

	// Token: 0x04005A14 RID: 23060
	[MyCmpReq]
	private KPrefabID kPrefabID;

	// Token: 0x04005A15 RID: 23061
	private AttributeModifier[] attributeModifiers;

	// Token: 0x020016B0 RID: 5808
	public class StatesInstance : GameStateMachine<NightOwl.States, NightOwl.StatesInstance, NightOwl, object>.GameInstance
	{
		// Token: 0x060077BC RID: 30652 RVA: 0x000F33FE File Offset: 0x000F15FE
		public StatesInstance(NightOwl master) : base(master)
		{
		}

		// Token: 0x060077BD RID: 30653 RVA: 0x000F3407 File Offset: 0x000F1607
		public bool IsNight()
		{
			return !(GameClock.Instance == null) && !(base.master.kPrefabID.PrefabTag == GameTags.MinionSelectPreview) && GameClock.Instance.IsNighttime();
		}
	}

	// Token: 0x020016B1 RID: 5809
	public class States : GameStateMachine<NightOwl.States, NightOwl.StatesInstance, NightOwl>
	{
		// Token: 0x060077BE RID: 30654 RVA: 0x0031C65C File Offset: 0x0031A85C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.root.TagTransition(GameTags.Dead, null, false);
			this.idle.Transition(this.early, (NightOwl.StatesInstance smi) => smi.IsNight(), UpdateRate.SIM_200ms);
			this.early.Enter("Night", delegate(NightOwl.StatesInstance smi)
			{
				smi.master.ApplyModifiers();
			}).Exit("NotNight", delegate(NightOwl.StatesInstance smi)
			{
				smi.master.RemoveModifiers();
			}).ToggleStatusItem(Db.Get().DuplicantStatusItems.NightTime, null).ToggleExpression(Db.Get().Expressions.Happy, null).Transition(this.idle, (NightOwl.StatesInstance smi) => !smi.IsNight(), UpdateRate.SIM_200ms);
		}

		// Token: 0x04005A16 RID: 23062
		public GameStateMachine<NightOwl.States, NightOwl.StatesInstance, NightOwl, object>.State idle;

		// Token: 0x04005A17 RID: 23063
		public GameStateMachine<NightOwl.States, NightOwl.StatesInstance, NightOwl, object>.State early;
	}
}
