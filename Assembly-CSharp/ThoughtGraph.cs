using System;
using System.Collections.Generic;

// Token: 0x02000B62 RID: 2914
public class ThoughtGraph : GameStateMachine<ThoughtGraph, ThoughtGraph.Instance>
{
	// Token: 0x060036D6 RID: 14038 RVA: 0x002223B4 File Offset: 0x002205B4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.initialdelay;
		this.initialdelay.ScheduleGoTo(1f, this.nothoughts);
		this.nothoughts.OnSignal(this.thoughtsChanged, this.displayingthought, (ThoughtGraph.Instance smi) => smi.HasThoughts()).OnSignal(this.thoughtsChangedImmediate, this.displayingthought, (ThoughtGraph.Instance smi) => smi.HasThoughts());
		this.displayingthought.DefaultState(this.displayingthought.pre).Enter("CreateBubble", delegate(ThoughtGraph.Instance smi)
		{
			smi.CreateBubble();
		}).Exit("DestroyBubble", delegate(ThoughtGraph.Instance smi)
		{
			smi.DestroyBubble();
		}).ScheduleGoTo((ThoughtGraph.Instance smi) => this.thoughtDisplayTime.Get(smi), this.cooldown);
		this.displayingthought.pre.ScheduleGoTo((ThoughtGraph.Instance smi) => TuningData<ThoughtGraph.Tuning>.Get().preLengthInSeconds, this.displayingthought.talking);
		this.displayingthought.talking.Enter(new StateMachine<ThoughtGraph, ThoughtGraph.Instance, IStateMachineTarget, object>.State.Callback(ThoughtGraph.BeginTalking));
		this.cooldown.OnSignal(this.thoughtsChangedImmediate, this.displayingthought, (ThoughtGraph.Instance smi) => smi.HasImmediateThought()).ScheduleGoTo(20f, this.nothoughts);
	}

	// Token: 0x060036D7 RID: 14039 RVA: 0x000C81C4 File Offset: 0x000C63C4
	private static void BeginTalking(ThoughtGraph.Instance smi)
	{
		if (smi.currentThought == null)
		{
			return;
		}
		if (SpeechMonitor.IsAllowedToPlaySpeech(smi.gameObject))
		{
			smi.GetSMI<SpeechMonitor.Instance>().PlaySpeech(smi.currentThought.speechPrefix, smi.currentThought.sound);
		}
	}

	// Token: 0x040025E8 RID: 9704
	public StateMachine<ThoughtGraph, ThoughtGraph.Instance, IStateMachineTarget, object>.Signal thoughtsChanged;

	// Token: 0x040025E9 RID: 9705
	public StateMachine<ThoughtGraph, ThoughtGraph.Instance, IStateMachineTarget, object>.Signal thoughtsChangedImmediate;

	// Token: 0x040025EA RID: 9706
	public StateMachine<ThoughtGraph, ThoughtGraph.Instance, IStateMachineTarget, object>.FloatParameter thoughtDisplayTime;

	// Token: 0x040025EB RID: 9707
	public GameStateMachine<ThoughtGraph, ThoughtGraph.Instance, IStateMachineTarget, object>.State initialdelay;

	// Token: 0x040025EC RID: 9708
	public GameStateMachine<ThoughtGraph, ThoughtGraph.Instance, IStateMachineTarget, object>.State nothoughts;

	// Token: 0x040025ED RID: 9709
	public ThoughtGraph.DisplayingThoughtState displayingthought;

	// Token: 0x040025EE RID: 9710
	public GameStateMachine<ThoughtGraph, ThoughtGraph.Instance, IStateMachineTarget, object>.State cooldown;

	// Token: 0x02000B63 RID: 2915
	public class Tuning : TuningData<ThoughtGraph.Tuning>
	{
		// Token: 0x040025EF RID: 9711
		public float preLengthInSeconds;
	}

	// Token: 0x02000B64 RID: 2916
	public class DisplayingThoughtState : GameStateMachine<ThoughtGraph, ThoughtGraph.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x040025F0 RID: 9712
		public GameStateMachine<ThoughtGraph, ThoughtGraph.Instance, IStateMachineTarget, object>.State pre;

		// Token: 0x040025F1 RID: 9713
		public GameStateMachine<ThoughtGraph, ThoughtGraph.Instance, IStateMachineTarget, object>.State talking;
	}

	// Token: 0x02000B65 RID: 2917
	public new class Instance : GameStateMachine<ThoughtGraph, ThoughtGraph.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x060036DC RID: 14044 RVA: 0x000C8223 File Offset: 0x000C6423
		public Instance(IStateMachineTarget master) : base(master)
		{
			NameDisplayScreen.Instance.RegisterComponent(base.gameObject, this, false);
		}

		// Token: 0x060036DD RID: 14045 RVA: 0x000C8249 File Offset: 0x000C6449
		public bool HasThoughts()
		{
			return this.thoughts.Count > 0;
		}

		// Token: 0x060036DE RID: 14046 RVA: 0x00222568 File Offset: 0x00220768
		public bool HasImmediateThought()
		{
			bool result = false;
			for (int i = 0; i < this.thoughts.Count; i++)
			{
				if (this.thoughts[i].showImmediately)
				{
					result = true;
					break;
				}
			}
			return result;
		}

		// Token: 0x060036DF RID: 14047 RVA: 0x002225A8 File Offset: 0x002207A8
		public void AddThought(Thought thought)
		{
			if (this.thoughts.Contains(thought))
			{
				return;
			}
			this.thoughts.Add(thought);
			if (thought.showImmediately)
			{
				base.sm.thoughtsChangedImmediate.Trigger(base.smi);
				return;
			}
			base.sm.thoughtsChanged.Trigger(base.smi);
		}

		// Token: 0x060036E0 RID: 14048 RVA: 0x000C8259 File Offset: 0x000C6459
		public void RemoveThought(Thought thought)
		{
			if (!this.thoughts.Contains(thought))
			{
				return;
			}
			this.thoughts.Remove(thought);
			base.sm.thoughtsChanged.Trigger(base.smi);
		}

		// Token: 0x060036E1 RID: 14049 RVA: 0x000C271B File Offset: 0x000C091B
		private int SortThoughts(Thought a, Thought b)
		{
			if (a.showImmediately == b.showImmediately)
			{
				return b.priority.CompareTo(a.priority);
			}
			if (!a.showImmediately)
			{
				return 1;
			}
			return -1;
		}

		// Token: 0x060036E2 RID: 14050 RVA: 0x00222608 File Offset: 0x00220808
		public void CreateBubble()
		{
			if (this.thoughts.Count == 0)
			{
				return;
			}
			this.thoughts.Sort(new Comparison<Thought>(this.SortThoughts));
			Thought thought = this.thoughts[0];
			if (thought.modeSprite != null)
			{
				NameDisplayScreen.Instance.SetThoughtBubbleConvoDisplay(base.gameObject, true, thought.hoverText, thought.bubbleSprite, thought.sprite, thought.modeSprite);
			}
			else
			{
				NameDisplayScreen.Instance.SetThoughtBubbleDisplay(base.gameObject, true, thought.hoverText, thought.bubbleSprite, thought.sprite);
			}
			base.sm.thoughtDisplayTime.Set(thought.showTime, this, false);
			this.currentThought = thought;
			if (thought.showImmediately)
			{
				this.thoughts.RemoveAt(0);
			}
		}

		// Token: 0x060036E3 RID: 14051 RVA: 0x000C2748 File Offset: 0x000C0948
		public void DestroyBubble()
		{
			NameDisplayScreen.Instance.SetThoughtBubbleDisplay(base.gameObject, false, null, null, null);
			NameDisplayScreen.Instance.SetThoughtBubbleConvoDisplay(base.gameObject, false, null, null, null, null);
		}

		// Token: 0x040025F2 RID: 9714
		private List<Thought> thoughts = new List<Thought>();

		// Token: 0x040025F3 RID: 9715
		public Thought currentThought;
	}
}
