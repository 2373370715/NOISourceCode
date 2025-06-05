using System;
using Klei.AI;
using KSerialization;
using TUNING;
using UnityEngine;

// Token: 0x020015E2 RID: 5602
public class JoyBehaviourMonitor : GameStateMachine<JoyBehaviourMonitor, JoyBehaviourMonitor.Instance>
{
	// Token: 0x06007438 RID: 29752 RVA: 0x00311DF0 File Offset: 0x0030FFF0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.neutral;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.root.TagTransition(GameTags.Dead, null, false);
		this.neutral.EventHandler(GameHashes.TagsChanged, delegate(JoyBehaviourMonitor.Instance smi, object data)
		{
			TagChangedEventData tagChangedEventData = (TagChangedEventData)data;
			if (!tagChangedEventData.added)
			{
				return;
			}
			if (tagChangedEventData.tag == GameTags.PleasantConversation && UnityEngine.Random.Range(0f, 100f) <= 1f)
			{
				smi.GoToOverjoyed();
			}
			smi.GetComponent<KPrefabID>().RemoveTag(GameTags.PleasantConversation);
		}).EventHandler(GameHashes.ScheduleBlocksTick, delegate(JoyBehaviourMonitor.Instance smi)
		{
			if (smi.ShouldBeOverjoyed())
			{
				smi.GoToOverjoyed();
			}
		});
		this.overjoyed.Transition(this.neutral, (JoyBehaviourMonitor.Instance smi) => GameClock.Instance.GetTime() >= smi.transitionTime, UpdateRate.SIM_200ms).ToggleExpression((JoyBehaviourMonitor.Instance smi) => smi.happyExpression).ToggleAnims((JoyBehaviourMonitor.Instance smi) => smi.happyLocoAnim).ToggleAnims((JoyBehaviourMonitor.Instance smi) => smi.happyLocoWalkAnim).ToggleTag(GameTags.Overjoyed).Exit(delegate(JoyBehaviourMonitor.Instance smi)
		{
			smi.GetComponent<KPrefabID>().RemoveTag(GameTags.PleasantConversation);
		}).OnSignal(this.exitEarly, this.neutral);
	}

	// Token: 0x04005740 RID: 22336
	public StateMachine<JoyBehaviourMonitor, JoyBehaviourMonitor.Instance, IStateMachineTarget, object>.Signal exitEarly;

	// Token: 0x04005741 RID: 22337
	public GameStateMachine<JoyBehaviourMonitor, JoyBehaviourMonitor.Instance, IStateMachineTarget, object>.State neutral;

	// Token: 0x04005742 RID: 22338
	public GameStateMachine<JoyBehaviourMonitor, JoyBehaviourMonitor.Instance, IStateMachineTarget, object>.State overjoyed;

	// Token: 0x020015E3 RID: 5603
	public new class Instance : GameStateMachine<JoyBehaviourMonitor, JoyBehaviourMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600743A RID: 29754 RVA: 0x00311F58 File Offset: 0x00310158
		public Instance(IStateMachineTarget master, string happy_loco_anim, string happy_loco_walk_anim, Expression happy_expression) : base(master)
		{
			this.happyLocoAnim = happy_loco_anim;
			this.happyLocoWalkAnim = happy_loco_walk_anim;
			this.happyExpression = happy_expression;
			Attributes attributes = base.gameObject.GetAttributes();
			this.expectationAttribute = attributes.Add(Db.Get().Attributes.QualityOfLifeExpectation);
			this.qolAttribute = Db.Get().Attributes.QualityOfLife.Lookup(base.gameObject);
		}

		// Token: 0x0600743B RID: 29755 RVA: 0x00311FE0 File Offset: 0x003101E0
		public bool ShouldBeOverjoyed()
		{
			float totalValue = this.qolAttribute.GetTotalValue();
			float totalValue2 = this.expectationAttribute.GetTotalValue();
			float num = totalValue - totalValue2;
			if (num >= TRAITS.JOY_REACTIONS.MIN_MORALE_EXCESS)
			{
				float num2 = MathUtil.ReRange(num, TRAITS.JOY_REACTIONS.MIN_MORALE_EXCESS, TRAITS.JOY_REACTIONS.MAX_MORALE_EXCESS, TRAITS.JOY_REACTIONS.MIN_REACTION_CHANCE, TRAITS.JOY_REACTIONS.MAX_REACTION_CHANCE);
				return UnityEngine.Random.Range(0f, 100f) <= num2;
			}
			return false;
		}

		// Token: 0x0600743C RID: 29756 RVA: 0x000F0AE9 File Offset: 0x000EECE9
		public void GoToOverjoyed()
		{
			base.smi.transitionTime = GameClock.Instance.GetTime() + TRAITS.JOY_REACTIONS.JOY_REACTION_DURATION;
			base.smi.GoTo(base.smi.sm.overjoyed);
		}

		// Token: 0x04005743 RID: 22339
		public string happyLocoAnim = "";

		// Token: 0x04005744 RID: 22340
		public string happyLocoWalkAnim = "";

		// Token: 0x04005745 RID: 22341
		public Expression happyExpression;

		// Token: 0x04005746 RID: 22342
		[Serialize]
		public float transitionTime;

		// Token: 0x04005747 RID: 22343
		private AttributeInstance expectationAttribute;

		// Token: 0x04005748 RID: 22344
		private AttributeInstance qolAttribute;
	}
}
