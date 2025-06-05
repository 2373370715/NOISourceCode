using System;
using System.Collections.Generic;

// Token: 0x020009F2 RID: 2546
public class CreatureThoughtGraph : GameStateMachine<CreatureThoughtGraph, CreatureThoughtGraph.Instance, IStateMachineTarget, CreatureThoughtGraph.Def>
{
	// Token: 0x06002E4A RID: 11850 RVA: 0x00202260 File Offset: 0x00200460
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.initialdelay;
		this.initialdelay.ScheduleGoTo(1f, this.nothoughts);
		this.nothoughts.OnSignal(this.thoughtsChanged, this.displayingthought, (CreatureThoughtGraph.Instance smi) => smi.HasThoughts()).OnSignal(this.thoughtsChangedImmediate, this.displayingthought, (CreatureThoughtGraph.Instance smi) => smi.HasThoughts());
		this.displayingthought.Enter("CreateBubble", delegate(CreatureThoughtGraph.Instance smi)
		{
			smi.CreateBubble();
		}).Exit("DestroyBubble", delegate(CreatureThoughtGraph.Instance smi)
		{
			smi.DestroyBubble();
		}).ScheduleGoTo((CreatureThoughtGraph.Instance smi) => this.thoughtDisplayTime.Get(smi), this.cooldown);
		this.cooldown.OnSignal(this.thoughtsChangedImmediate, this.displayingthought, (CreatureThoughtGraph.Instance smi) => smi.HasImmediateThought()).ScheduleGoTo(20f, this.nothoughts);
	}

	// Token: 0x04001FAC RID: 8108
	public StateMachine<CreatureThoughtGraph, CreatureThoughtGraph.Instance, IStateMachineTarget, CreatureThoughtGraph.Def>.Signal thoughtsChanged;

	// Token: 0x04001FAD RID: 8109
	public StateMachine<CreatureThoughtGraph, CreatureThoughtGraph.Instance, IStateMachineTarget, CreatureThoughtGraph.Def>.Signal thoughtsChangedImmediate;

	// Token: 0x04001FAE RID: 8110
	public StateMachine<CreatureThoughtGraph, CreatureThoughtGraph.Instance, IStateMachineTarget, CreatureThoughtGraph.Def>.FloatParameter thoughtDisplayTime;

	// Token: 0x04001FAF RID: 8111
	public GameStateMachine<CreatureThoughtGraph, CreatureThoughtGraph.Instance, IStateMachineTarget, CreatureThoughtGraph.Def>.State initialdelay;

	// Token: 0x04001FB0 RID: 8112
	public GameStateMachine<CreatureThoughtGraph, CreatureThoughtGraph.Instance, IStateMachineTarget, CreatureThoughtGraph.Def>.State nothoughts;

	// Token: 0x04001FB1 RID: 8113
	public GameStateMachine<CreatureThoughtGraph, CreatureThoughtGraph.Instance, IStateMachineTarget, CreatureThoughtGraph.Def>.State displayingthought;

	// Token: 0x04001FB2 RID: 8114
	public GameStateMachine<CreatureThoughtGraph, CreatureThoughtGraph.Instance, IStateMachineTarget, CreatureThoughtGraph.Def>.State cooldown;

	// Token: 0x020009F3 RID: 2547
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020009F4 RID: 2548
	public new class Instance : GameStateMachine<CreatureThoughtGraph, CreatureThoughtGraph.Instance, IStateMachineTarget, CreatureThoughtGraph.Def>.GameInstance
	{
		// Token: 0x06002E4E RID: 11854 RVA: 0x000C26A8 File Offset: 0x000C08A8
		public Instance(IStateMachineTarget master, CreatureThoughtGraph.Def def) : base(master, def)
		{
			NameDisplayScreen.Instance.RegisterComponent(base.gameObject, this, false);
		}

		// Token: 0x06002E4F RID: 11855 RVA: 0x000C26CF File Offset: 0x000C08CF
		protected override void OnCleanUp()
		{
			base.OnCleanUp();
		}

		// Token: 0x06002E50 RID: 11856 RVA: 0x000C26D7 File Offset: 0x000C08D7
		public bool HasThoughts()
		{
			return this.thoughts.Count > 0;
		}

		// Token: 0x06002E51 RID: 11857 RVA: 0x002023AC File Offset: 0x002005AC
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

		// Token: 0x06002E52 RID: 11858 RVA: 0x002023EC File Offset: 0x002005EC
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

		// Token: 0x06002E53 RID: 11859 RVA: 0x000C26E7 File Offset: 0x000C08E7
		public void RemoveThought(Thought thought)
		{
			if (!this.thoughts.Contains(thought))
			{
				return;
			}
			this.thoughts.Remove(thought);
			base.sm.thoughtsChanged.Trigger(base.smi);
		}

		// Token: 0x06002E54 RID: 11860 RVA: 0x000C271B File Offset: 0x000C091B
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

		// Token: 0x06002E55 RID: 11861 RVA: 0x0020244C File Offset: 0x0020064C
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

		// Token: 0x06002E56 RID: 11862 RVA: 0x000C2748 File Offset: 0x000C0948
		public void DestroyBubble()
		{
			NameDisplayScreen.Instance.SetThoughtBubbleDisplay(base.gameObject, false, null, null, null);
			NameDisplayScreen.Instance.SetThoughtBubbleConvoDisplay(base.gameObject, false, null, null, null, null);
		}

		// Token: 0x04001FB3 RID: 8115
		private List<Thought> thoughts = new List<Thought>();

		// Token: 0x04001FB4 RID: 8116
		public Thought currentThought;
	}
}
