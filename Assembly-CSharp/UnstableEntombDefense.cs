using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020002E2 RID: 738
public class UnstableEntombDefense : GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>
{
	// Token: 0x06000B58 RID: 2904 RVA: 0x00178350 File Offset: 0x00176550
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.disabled;
		this.disabled.EventTransition(GameHashes.Died, this.dead, null).ParamTransition<bool>(this.Active, this.active, GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.IsTrue);
		this.active.EventTransition(GameHashes.Died, this.dead, null).ParamTransition<bool>(this.Active, this.disabled, GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.IsFalse).DefaultState(this.active.safe);
		this.active.safe.DefaultState(this.active.safe.idle);
		this.active.safe.idle.ParamTransition<float>(this.TimeBeforeNextReaction, this.active.threatened, (UnstableEntombDefense.Instance smi, float p) => GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.IsGTZero(smi, p) && UnstableEntombDefense.IsEntombedByUnstable(smi)).EventTransition(GameHashes.EntombedChanged, this.active.safe.newThreat, new StateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.Transition.ConditionCallback(UnstableEntombDefense.IsEntombedByUnstable));
		this.active.safe.newThreat.Enter(new StateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.State.Callback(UnstableEntombDefense.ResetCooldown)).GoTo(this.active.threatened);
		this.active.threatened.EventTransition(GameHashes.Died, this.dead, null).Exit(new StateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.State.Callback(UnstableEntombDefense.ResetCooldown)).EventTransition(GameHashes.EntombedChanged, this.active.safe, GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.Not(new StateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.Transition.ConditionCallback(UnstableEntombDefense.IsEntombedByUnstable))).DefaultState(this.active.threatened.inCooldown);
		this.active.threatened.inCooldown.ParamTransition<float>(this.TimeBeforeNextReaction, this.active.threatened.react, GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.IsLTEZero).Update(new Action<UnstableEntombDefense.Instance, float>(UnstableEntombDefense.CooldownTick), UpdateRate.SIM_200ms, false);
		this.active.threatened.react.TriggerOnEnter(GameHashes.EntombDefenseReactionBegins, null).PlayAnim((UnstableEntombDefense.Instance smi) => smi.UnentombAnimName, KAnim.PlayMode.Once).OnAnimQueueComplete(this.active.threatened.complete).ScheduleGoTo(2f, this.active.threatened.complete);
		this.active.threatened.complete.TriggerOnEnter(GameHashes.EntombDefenseReact, null).Enter(new StateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.State.Callback(UnstableEntombDefense.AttemptToBreakFree)).Enter(new StateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.State.Callback(UnstableEntombDefense.ResetCooldown)).GoTo(this.active.threatened.inCooldown);
		this.dead.DoNothing();
	}

	// Token: 0x06000B59 RID: 2905 RVA: 0x000AF9BA File Offset: 0x000ADBBA
	public static void ResetCooldown(UnstableEntombDefense.Instance smi)
	{
		smi.sm.TimeBeforeNextReaction.Set(smi.def.Cooldown, smi, false);
	}

	// Token: 0x06000B5A RID: 2906 RVA: 0x000AF9DA File Offset: 0x000ADBDA
	public static bool IsEntombedByUnstable(UnstableEntombDefense.Instance smi)
	{
		return smi.IsEntombed && smi.IsInPressenceOfUnstableSolids();
	}

	// Token: 0x06000B5B RID: 2907 RVA: 0x000AF9EC File Offset: 0x000ADBEC
	public static void AttemptToBreakFree(UnstableEntombDefense.Instance smi)
	{
		smi.AttackUnstableCells();
	}

	// Token: 0x06000B5C RID: 2908 RVA: 0x00178614 File Offset: 0x00176814
	public static void CooldownTick(UnstableEntombDefense.Instance smi, float dt)
	{
		float value = smi.RemainingCooldown - dt;
		smi.sm.TimeBeforeNextReaction.Set(value, smi, false);
	}

	// Token: 0x040008E6 RID: 2278
	public UnstableEntombDefense.ActiveState active;

	// Token: 0x040008E7 RID: 2279
	public GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.State disabled;

	// Token: 0x040008E8 RID: 2280
	public GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.State dead;

	// Token: 0x040008E9 RID: 2281
	public StateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.FloatParameter TimeBeforeNextReaction;

	// Token: 0x040008EA RID: 2282
	public StateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.BoolParameter Active = new StateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.BoolParameter(true);

	// Token: 0x020002E3 RID: 739
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x06000B5E RID: 2910 RVA: 0x00178640 File Offset: 0x00176840
		public List<Descriptor> GetDescriptors(GameObject go)
		{
			List<Descriptor> list = new List<Descriptor>();
			UnstableEntombDefense.Instance smi = go.GetSMI<UnstableEntombDefense.Instance>();
			if (smi != null)
			{
				Descriptor stateDescriptor = smi.GetStateDescriptor();
				if (stateDescriptor.type == Descriptor.DescriptorType.Effect)
				{
					list.Add(stateDescriptor);
				}
			}
			return list;
		}

		// Token: 0x040008EB RID: 2283
		public float Cooldown = 5f;

		// Token: 0x040008EC RID: 2284
		public string defaultAnimName = "";
	}

	// Token: 0x020002E4 RID: 740
	public class SafeStates : GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.State
	{
		// Token: 0x040008ED RID: 2285
		public GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.State idle;

		// Token: 0x040008EE RID: 2286
		public GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.State newThreat;
	}

	// Token: 0x020002E5 RID: 741
	public class ThreatenedStates : GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.State
	{
		// Token: 0x040008EF RID: 2287
		public GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.State inCooldown;

		// Token: 0x040008F0 RID: 2288
		public GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.State react;

		// Token: 0x040008F1 RID: 2289
		public GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.State complete;
	}

	// Token: 0x020002E6 RID: 742
	public class ActiveState : GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.State
	{
		// Token: 0x040008F2 RID: 2290
		public UnstableEntombDefense.SafeStates safe;

		// Token: 0x040008F3 RID: 2291
		public UnstableEntombDefense.ThreatenedStates threatened;
	}

	// Token: 0x020002E7 RID: 743
	public new class Instance : GameStateMachine<UnstableEntombDefense, UnstableEntombDefense.Instance, IStateMachineTarget, UnstableEntombDefense.Def>.GameInstance
	{
		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000B63 RID: 2915 RVA: 0x000AFA2E File Offset: 0x000ADC2E
		public float RemainingCooldown
		{
			get
			{
				return base.sm.TimeBeforeNextReaction.Get(this);
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000B64 RID: 2916 RVA: 0x000AFA41 File Offset: 0x000ADC41
		public bool IsEntombed
		{
			get
			{
				return this.entombVulnerable.GetEntombed;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000B65 RID: 2917 RVA: 0x000AFA4E File Offset: 0x000ADC4E
		public bool IsActive
		{
			get
			{
				return base.sm.Active.Get(this);
			}
		}

		// Token: 0x06000B66 RID: 2918 RVA: 0x000AFA61 File Offset: 0x000ADC61
		public Instance(IStateMachineTarget master, UnstableEntombDefense.Def def) : base(master, def)
		{
			this.UnentombAnimName = ((this.UnentombAnimName == null) ? def.defaultAnimName : this.UnentombAnimName);
		}

		// Token: 0x06000B67 RID: 2919 RVA: 0x00178678 File Offset: 0x00176878
		public bool IsInPressenceOfUnstableSolids()
		{
			int cell = Grid.PosToCell(this);
			CellOffset[] occupiedCellsOffsets = this.occupyArea.OccupiedCellsOffsets;
			for (int i = 0; i < occupiedCellsOffsets.Length; i++)
			{
				int num = Grid.OffsetCell(cell, occupiedCellsOffsets[i]);
				if (Grid.IsValidCell(num) && Grid.Solid[num] && Grid.Element[num].IsUnstable)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000B68 RID: 2920 RVA: 0x001786DC File Offset: 0x001768DC
		public void AttackUnstableCells()
		{
			int cell = Grid.PosToCell(this);
			CellOffset[] occupiedCellsOffsets = this.occupyArea.OccupiedCellsOffsets;
			for (int i = 0; i < occupiedCellsOffsets.Length; i++)
			{
				int num = Grid.OffsetCell(cell, occupiedCellsOffsets[i]);
				if (Grid.IsValidCell(num) && Grid.Solid[num] && Grid.Element[num].IsUnstable)
				{
					SimMessages.Dig(num, -1, false);
				}
			}
		}

		// Token: 0x06000B69 RID: 2921 RVA: 0x000AFA87 File Offset: 0x000ADC87
		public void SetActive(bool active)
		{
			base.sm.Active.Set(active, this, false);
		}

		// Token: 0x06000B6A RID: 2922 RVA: 0x00178744 File Offset: 0x00176944
		public Descriptor GetStateDescriptor()
		{
			if (base.IsInsideState(base.sm.disabled))
			{
				return new Descriptor(UI.BUILDINGEFFECTS.UNSTABLEENTOMBDEFENSEOFF, UI.BUILDINGEFFECTS.TOOLTIPS.UNSTABLEENTOMBDEFENSEOFF, Descriptor.DescriptorType.Effect, false);
			}
			if (base.IsInsideState(base.sm.active.safe))
			{
				return new Descriptor(UI.BUILDINGEFFECTS.UNSTABLEENTOMBDEFENSEREADY, UI.BUILDINGEFFECTS.TOOLTIPS.UNSTABLEENTOMBDEFENSEREADY, Descriptor.DescriptorType.Effect, false);
			}
			if (base.IsInsideState(base.sm.active.threatened.inCooldown))
			{
				return new Descriptor(UI.BUILDINGEFFECTS.UNSTABLEENTOMBDEFENSETHREATENED, UI.BUILDINGEFFECTS.TOOLTIPS.UNSTABLEENTOMBDEFENSETHREATENED, Descriptor.DescriptorType.Effect, false);
			}
			if (base.IsInsideState(base.sm.active.threatened.react))
			{
				return new Descriptor(UI.BUILDINGEFFECTS.UNSTABLEENTOMBDEFENSEREACTING, UI.BUILDINGEFFECTS.TOOLTIPS.UNSTABLEENTOMBDEFENSEREACTING, Descriptor.DescriptorType.Effect, false);
			}
			return new Descriptor
			{
				type = Descriptor.DescriptorType.Detail
			};
		}

		// Token: 0x040008F4 RID: 2292
		public string UnentombAnimName;

		// Token: 0x040008F5 RID: 2293
		[MyCmpGet]
		private EntombVulnerable entombVulnerable;

		// Token: 0x040008F6 RID: 2294
		[MyCmpGet]
		private OccupyArea occupyArea;
	}
}
