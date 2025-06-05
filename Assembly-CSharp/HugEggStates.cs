using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020001B9 RID: 441
public class HugEggStates : GameStateMachine<HugEggStates, HugEggStates.Instance, IStateMachineTarget, HugEggStates.Def>
{
	// Token: 0x06000609 RID: 1545 RVA: 0x001635F8 File Offset: 0x001617F8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.moving;
		GameStateMachine<HugEggStates, HugEggStates.Instance, IStateMachineTarget, HugEggStates.Def>.State state = this.root.Enter(new StateMachine<HugEggStates, HugEggStates.Instance, IStateMachineTarget, HugEggStates.Def>.State.Callback(HugEggStates.SetTarget)).Enter(delegate(HugEggStates.Instance smi)
		{
			if (!HugEggStates.Reserve(smi))
			{
				smi.GoTo(this.behaviourcomplete);
			}
		}).Exit(new StateMachine<HugEggStates, HugEggStates.Instance, IStateMachineTarget, HugEggStates.Def>.State.Callback(HugEggStates.Unreserve));
		string name = CREATURES.STATUSITEMS.HUGEGG.NAME;
		string tooltip = CREATURES.STATUSITEMS.HUGEGG.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main).OnTargetLost(this.target, this.behaviourcomplete);
		this.moving.MoveTo(new Func<HugEggStates.Instance, int>(HugEggStates.GetClimbableCell), this.hug, this.behaviourcomplete, false);
		this.hug.DefaultState(this.hug.pre).Enter(delegate(HugEggStates.Instance smi)
		{
			smi.GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.Front);
		}).Exit(delegate(HugEggStates.Instance smi)
		{
			smi.GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.Creatures);
		});
		this.hug.pre.Face(this.target, 0.5f).Enter(delegate(HugEggStates.Instance smi)
		{
			Navigator component = smi.GetComponent<Navigator>();
			if (component.IsValidNavType(NavType.Floor))
			{
				component.SetCurrentNavType(NavType.Floor);
			}
		}).PlayAnim((HugEggStates.Instance smi) => HugEggStates.GetAnims(smi).pre, KAnim.PlayMode.Once).OnAnimQueueComplete(this.hug.loop);
		this.hug.loop.QueueAnim((HugEggStates.Instance smi) => HugEggStates.GetAnims(smi).loop, true, null).ScheduleGoTo((HugEggStates.Instance smi) => smi.def.hugTime, this.hug.pst);
		this.hug.pst.QueueAnim((HugEggStates.Instance smi) => HugEggStates.GetAnims(smi).pst, false, null).Enter(new StateMachine<HugEggStates, HugEggStates.Instance, IStateMachineTarget, HugEggStates.Def>.State.Callback(HugEggStates.ApplyEffect)).OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.BehaviourComplete((HugEggStates.Instance smi) => smi.def.behaviourTag, false);
	}

	// Token: 0x0600060A RID: 1546 RVA: 0x000ACD59 File Offset: 0x000AAF59
	private static void SetTarget(HugEggStates.Instance smi)
	{
		smi.sm.target.Set(smi.GetSMI<HugMonitor.Instance>().hugTarget.gameObject, smi, false);
	}

	// Token: 0x0600060B RID: 1547 RVA: 0x000ACD7E File Offset: 0x000AAF7E
	private static HugEggStates.AnimSet GetAnims(HugEggStates.Instance smi)
	{
		if (!(smi.sm.target.Get(smi).GetComponent<EggIncubator>() != null))
		{
			return smi.def.hugAnims;
		}
		return smi.def.incubatorHugAnims;
	}

	// Token: 0x0600060C RID: 1548 RVA: 0x00163870 File Offset: 0x00161A70
	private static bool Reserve(HugEggStates.Instance smi)
	{
		GameObject gameObject = smi.sm.target.Get(smi);
		if (gameObject != null && !gameObject.HasTag(GameTags.Creatures.ReservedByCreature))
		{
			gameObject.AddTag(GameTags.Creatures.ReservedByCreature);
			return true;
		}
		return false;
	}

	// Token: 0x0600060D RID: 1549 RVA: 0x001638B4 File Offset: 0x00161AB4
	private static void Unreserve(HugEggStates.Instance smi)
	{
		GameObject gameObject = smi.sm.target.Get(smi);
		if (gameObject != null)
		{
			gameObject.RemoveTag(GameTags.Creatures.ReservedByCreature);
		}
	}

	// Token: 0x0600060E RID: 1550 RVA: 0x000ACDB5 File Offset: 0x000AAFB5
	private static int GetClimbableCell(HugEggStates.Instance smi)
	{
		return Grid.PosToCell(smi.sm.target.Get(smi));
	}

	// Token: 0x0600060F RID: 1551 RVA: 0x001638E8 File Offset: 0x00161AE8
	private static void ApplyEffect(HugEggStates.Instance smi)
	{
		GameObject gameObject = smi.sm.target.Get(smi);
		if (gameObject != null)
		{
			EggIncubator component = gameObject.GetComponent<EggIncubator>();
			if (component != null && component.Occupant != null)
			{
				component.Occupant.GetComponent<Effects>().Add("EggHug", true);
				return;
			}
			if (gameObject.HasTag(GameTags.Egg))
			{
				gameObject.GetComponent<Effects>().Add("EggHug", true);
			}
		}
	}

	// Token: 0x04000465 RID: 1125
	public GameStateMachine<HugEggStates, HugEggStates.Instance, IStateMachineTarget, HugEggStates.Def>.ApproachSubState<EggIncubator> moving;

	// Token: 0x04000466 RID: 1126
	public HugEggStates.HugState hug;

	// Token: 0x04000467 RID: 1127
	public GameStateMachine<HugEggStates, HugEggStates.Instance, IStateMachineTarget, HugEggStates.Def>.State behaviourcomplete;

	// Token: 0x04000468 RID: 1128
	public StateMachine<HugEggStates, HugEggStates.Instance, IStateMachineTarget, HugEggStates.Def>.TargetParameter target;

	// Token: 0x020001BA RID: 442
	public class AnimSet
	{
		// Token: 0x04000469 RID: 1129
		public string pre;

		// Token: 0x0400046A RID: 1130
		public string loop;

		// Token: 0x0400046B RID: 1131
		public string pst;
	}

	// Token: 0x020001BB RID: 443
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x06000613 RID: 1555 RVA: 0x00163968 File Offset: 0x00161B68
		public Def(Tag behaviourTag)
		{
			this.behaviourTag = behaviourTag;
		}

		// Token: 0x0400046C RID: 1132
		public float hugTime = 15f;

		// Token: 0x0400046D RID: 1133
		public Tag behaviourTag;

		// Token: 0x0400046E RID: 1134
		public HugEggStates.AnimSet hugAnims = new HugEggStates.AnimSet
		{
			pre = "hug_egg_pre",
			loop = "hug_egg_loop",
			pst = "hug_egg_pst"
		};

		// Token: 0x0400046F RID: 1135
		public HugEggStates.AnimSet incubatorHugAnims = new HugEggStates.AnimSet
		{
			pre = "hug_incubator_pre",
			loop = "hug_incubator_loop",
			pst = "hug_incubator_pst"
		};
	}

	// Token: 0x020001BC RID: 444
	public new class Instance : GameStateMachine<HugEggStates, HugEggStates.Instance, IStateMachineTarget, HugEggStates.Def>.GameInstance
	{
		// Token: 0x06000614 RID: 1556 RVA: 0x000ACDEB File Offset: 0x000AAFEB
		public Instance(Chore<HugEggStates.Instance> chore, HugEggStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, def.behaviourTag);
		}
	}

	// Token: 0x020001BD RID: 445
	public class HugState : GameStateMachine<HugEggStates, HugEggStates.Instance, IStateMachineTarget, HugEggStates.Def>.State
	{
		// Token: 0x04000470 RID: 1136
		public GameStateMachine<HugEggStates, HugEggStates.Instance, IStateMachineTarget, HugEggStates.Def>.State pre;

		// Token: 0x04000471 RID: 1137
		public GameStateMachine<HugEggStates, HugEggStates.Instance, IStateMachineTarget, HugEggStates.Def>.State loop;

		// Token: 0x04000472 RID: 1138
		public GameStateMachine<HugEggStates, HugEggStates.Instance, IStateMachineTarget, HugEggStates.Def>.State pst;
	}
}
