using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;

// Token: 0x0200151A RID: 5402
[SkipSaveFileSerialization]
public class Meteorphile : StateMachineComponent<Meteorphile.StatesInstance>
{
	// Token: 0x06007043 RID: 28739 RVA: 0x003044D0 File Offset: 0x003026D0
	protected override void OnSpawn()
	{
		this.attributeModifiers = new AttributeModifier[]
		{
			new AttributeModifier("Construction", TRAITS.METEORPHILE_MODIFIER, DUPLICANTS.TRAITS.METEORPHILE.NAME, false, false, true),
			new AttributeModifier("Digging", TRAITS.METEORPHILE_MODIFIER, DUPLICANTS.TRAITS.METEORPHILE.NAME, false, false, true),
			new AttributeModifier("Machinery", TRAITS.METEORPHILE_MODIFIER, DUPLICANTS.TRAITS.METEORPHILE.NAME, false, false, true),
			new AttributeModifier("Athletics", TRAITS.METEORPHILE_MODIFIER, DUPLICANTS.TRAITS.METEORPHILE.NAME, false, false, true),
			new AttributeModifier("Learning", TRAITS.METEORPHILE_MODIFIER, DUPLICANTS.TRAITS.METEORPHILE.NAME, false, false, true),
			new AttributeModifier("Cooking", TRAITS.METEORPHILE_MODIFIER, DUPLICANTS.TRAITS.METEORPHILE.NAME, false, false, true),
			new AttributeModifier("Art", TRAITS.METEORPHILE_MODIFIER, DUPLICANTS.TRAITS.METEORPHILE.NAME, false, false, true),
			new AttributeModifier("Strength", TRAITS.METEORPHILE_MODIFIER, DUPLICANTS.TRAITS.METEORPHILE.NAME, false, false, true),
			new AttributeModifier("Caring", TRAITS.METEORPHILE_MODIFIER, DUPLICANTS.TRAITS.METEORPHILE.NAME, false, false, true),
			new AttributeModifier("Botanist", TRAITS.METEORPHILE_MODIFIER, DUPLICANTS.TRAITS.METEORPHILE.NAME, false, false, true),
			new AttributeModifier("Ranching", TRAITS.METEORPHILE_MODIFIER, DUPLICANTS.TRAITS.METEORPHILE.NAME, false, false, true)
		};
		base.smi.StartSM();
	}

	// Token: 0x06007044 RID: 28740 RVA: 0x0030464C File Offset: 0x0030284C
	public void ApplyModifiers()
	{
		Attributes attributes = base.gameObject.GetAttributes();
		for (int i = 0; i < this.attributeModifiers.Length; i++)
		{
			AttributeModifier modifier = this.attributeModifiers[i];
			attributes.Add(modifier);
		}
	}

	// Token: 0x06007045 RID: 28741 RVA: 0x00304688 File Offset: 0x00302888
	public void RemoveModifiers()
	{
		Attributes attributes = base.gameObject.GetAttributes();
		for (int i = 0; i < this.attributeModifiers.Length; i++)
		{
			AttributeModifier modifier = this.attributeModifiers[i];
			attributes.Remove(modifier);
		}
	}

	// Token: 0x0400545B RID: 21595
	[MyCmpReq]
	private KPrefabID kPrefabID;

	// Token: 0x0400545C RID: 21596
	private AttributeModifier[] attributeModifiers;

	// Token: 0x0200151B RID: 5403
	public class StatesInstance : GameStateMachine<Meteorphile.States, Meteorphile.StatesInstance, Meteorphile, object>.GameInstance
	{
		// Token: 0x06007047 RID: 28743 RVA: 0x000EDF91 File Offset: 0x000EC191
		public StatesInstance(Meteorphile master) : base(master)
		{
		}

		// Token: 0x06007048 RID: 28744 RVA: 0x003046C4 File Offset: 0x003028C4
		public bool IsMeteors()
		{
			if (GameplayEventManager.Instance == null || base.master.kPrefabID.PrefabTag == GameTags.MinionSelectPreview)
			{
				return false;
			}
			int myWorldId = this.GetMyWorldId();
			List<GameplayEventInstance> list = new List<GameplayEventInstance>();
			GameplayEventManager.Instance.GetActiveEventsOfType<MeteorShowerEvent>(myWorldId, ref list);
			for (int i = 0; i < list.Count; i++)
			{
				MeteorShowerEvent.StatesInstance statesInstance = list[i].smi as MeteorShowerEvent.StatesInstance;
				if (statesInstance != null && statesInstance.IsInsideState(statesInstance.sm.running.bombarding))
				{
					return true;
				}
			}
			return false;
		}
	}

	// Token: 0x0200151C RID: 5404
	public class States : GameStateMachine<Meteorphile.States, Meteorphile.StatesInstance, Meteorphile>
	{
		// Token: 0x06007049 RID: 28745 RVA: 0x00304758 File Offset: 0x00302958
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.root.TagTransition(GameTags.Dead, null, false);
			this.idle.Transition(this.early, (Meteorphile.StatesInstance smi) => smi.IsMeteors(), UpdateRate.SIM_200ms);
			this.early.Enter("Meteors", delegate(Meteorphile.StatesInstance smi)
			{
				smi.master.ApplyModifiers();
			}).Exit("NotMeteors", delegate(Meteorphile.StatesInstance smi)
			{
				smi.master.RemoveModifiers();
			}).ToggleStatusItem(Db.Get().DuplicantStatusItems.Meteorphile, null).ToggleExpression(Db.Get().Expressions.Happy, null).Transition(this.idle, (Meteorphile.StatesInstance smi) => !smi.IsMeteors(), UpdateRate.SIM_200ms);
		}

		// Token: 0x0400545D RID: 21597
		public GameStateMachine<Meteorphile.States, Meteorphile.StatesInstance, Meteorphile, object>.State idle;

		// Token: 0x0400545E RID: 21598
		public GameStateMachine<Meteorphile.States, Meteorphile.StatesInstance, Meteorphile, object>.State early;
	}
}
