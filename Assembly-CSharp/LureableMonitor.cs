using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000A4C RID: 2636
public class LureableMonitor : GameStateMachine<LureableMonitor, LureableMonitor.Instance, IStateMachineTarget, LureableMonitor.Def>
{
	// Token: 0x06002FA3 RID: 12195 RVA: 0x002065C4 File Offset: 0x002047C4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.cooldown;
		this.cooldown.ScheduleGoTo((LureableMonitor.Instance smi) => smi.def.cooldown, this.nolure);
		this.nolure.PreBrainUpdate(delegate(LureableMonitor.Instance smi)
		{
			smi.FindLure();
		}).ParamTransition<GameObject>(this.targetLure, this.haslure, (LureableMonitor.Instance smi, GameObject p) => p != null);
		this.haslure.ParamTransition<GameObject>(this.targetLure, this.nolure, (LureableMonitor.Instance smi, GameObject p) => p == null).PreBrainUpdate(delegate(LureableMonitor.Instance smi)
		{
			smi.FindLure();
		}).ToggleBehaviour(GameTags.Creatures.MoveToLure, (LureableMonitor.Instance smi) => smi.HasLure(), delegate(LureableMonitor.Instance smi)
		{
			smi.GoTo(this.cooldown);
		});
	}

	// Token: 0x040020CA RID: 8394
	public StateMachine<LureableMonitor, LureableMonitor.Instance, IStateMachineTarget, LureableMonitor.Def>.TargetParameter targetLure;

	// Token: 0x040020CB RID: 8395
	public GameStateMachine<LureableMonitor, LureableMonitor.Instance, IStateMachineTarget, LureableMonitor.Def>.State nolure;

	// Token: 0x040020CC RID: 8396
	public GameStateMachine<LureableMonitor, LureableMonitor.Instance, IStateMachineTarget, LureableMonitor.Def>.State haslure;

	// Token: 0x040020CD RID: 8397
	public GameStateMachine<LureableMonitor, LureableMonitor.Instance, IStateMachineTarget, LureableMonitor.Def>.State cooldown;

	// Token: 0x02000A4D RID: 2637
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x06002FA6 RID: 12198 RVA: 0x000C3658 File Offset: 0x000C1858
		public List<Descriptor> GetDescriptors(GameObject go)
		{
			return new List<Descriptor>
			{
				new Descriptor(UI.BUILDINGEFFECTS.CAPTURE_METHOD_LURE, UI.BUILDINGEFFECTS.TOOLTIPS.CAPTURE_METHOD_LURE, Descriptor.DescriptorType.Effect, false)
			};
		}

		// Token: 0x040020CE RID: 8398
		public float cooldown = 20f;

		// Token: 0x040020CF RID: 8399
		public Tag[] lures;
	}

	// Token: 0x02000A4E RID: 2638
	public new class Instance : GameStateMachine<LureableMonitor, LureableMonitor.Instance, IStateMachineTarget, LureableMonitor.Def>.GameInstance
	{
		// Token: 0x06002FA8 RID: 12200 RVA: 0x000C3693 File Offset: 0x000C1893
		public Instance(IStateMachineTarget master, LureableMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x06002FA9 RID: 12201 RVA: 0x002066F8 File Offset: 0x002048F8
		private static bool FindLureCounter(object obj, LureableMonitor.Instance.FindLureCounterContext context)
		{
			Lure.Instance instance = obj as Lure.Instance;
			if (instance == null || !instance.IsActive() || !instance.HasAnyLure(context.inst.def.lures))
			{
				return true;
			}
			int navigationCost = context.inst.navigator.GetNavigationCost(Grid.PosToCell(instance.transform.GetPosition()), instance.LurePoints);
			if (navigationCost != -1 && (context.cost == -1 || navigationCost < context.cost))
			{
				context.cost = navigationCost;
				context.result = instance.gameObject;
			}
			return true;
		}

		// Token: 0x06002FAA RID: 12202 RVA: 0x00206784 File Offset: 0x00204984
		public void FindLure()
		{
			LureableMonitor.Instance.context.inst = this;
			LureableMonitor.Instance.context.cost = -1;
			LureableMonitor.Instance.context.result = null;
			GameScenePartitioner.Instance.AsyncSafeVisit<LureableMonitor.Instance.FindLureCounterContext>(Grid.PosToCell(base.smi.transform.GetPosition()), 1, GameScenePartitioner.Instance.lure, new Func<object, LureableMonitor.Instance.FindLureCounterContext, bool>(LureableMonitor.Instance.FindLureCounter), LureableMonitor.Instance.context);
			base.sm.targetLure.Set(LureableMonitor.Instance.context.result, this, false);
		}

		// Token: 0x06002FAB RID: 12203 RVA: 0x000C369D File Offset: 0x000C189D
		public bool HasLure()
		{
			return base.sm.targetLure.Get(this) != null;
		}

		// Token: 0x06002FAC RID: 12204 RVA: 0x000C36B6 File Offset: 0x000C18B6
		public GameObject GetTargetLure()
		{
			return base.sm.targetLure.Get(this);
		}

		// Token: 0x040020D0 RID: 8400
		[MyCmpReq]
		private Navigator navigator;

		// Token: 0x040020D1 RID: 8401
		private static LureableMonitor.Instance.FindLureCounterContext context = new LureableMonitor.Instance.FindLureCounterContext();

		// Token: 0x02000A4F RID: 2639
		private class FindLureCounterContext
		{
			// Token: 0x040020D2 RID: 8402
			public LureableMonitor.Instance inst;

			// Token: 0x040020D3 RID: 8403
			public int cost;

			// Token: 0x040020D4 RID: 8404
			public GameObject result;
		}
	}
}
