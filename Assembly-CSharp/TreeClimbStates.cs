using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200021B RID: 539
public class TreeClimbStates : GameStateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def>
{
	// Token: 0x0600074D RID: 1869 RVA: 0x00167C04 File Offset: 0x00165E04
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.moving;
		GameStateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def>.State state = this.root.Enter(new StateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def>.State.Callback(TreeClimbStates.SetTarget)).Enter(delegate(TreeClimbStates.Instance smi)
		{
			if (!TreeClimbStates.ReserveClimbable(smi))
			{
				smi.GoTo(this.behaviourcomplete);
			}
		}).Exit(new StateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def>.State.Callback(TreeClimbStates.UnreserveClimbable));
		string name = CREATURES.STATUSITEMS.RUMMAGINGSEED.NAME;
		string tooltip = CREATURES.STATUSITEMS.RUMMAGINGSEED.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		this.moving.MoveTo(new Func<TreeClimbStates.Instance, int>(TreeClimbStates.GetClimbableCell), this.climbing, this.behaviourcomplete, false);
		this.climbing.DefaultState(this.climbing.pre);
		this.climbing.pre.PlayAnim("rummage_pre").OnAnimQueueComplete(this.climbing.loop);
		this.climbing.loop.QueueAnim("rummage_loop", true, null).ScheduleGoTo(3.5f, this.climbing.pst).Update(new Action<TreeClimbStates.Instance, float>(TreeClimbStates.Rummage), UpdateRate.SIM_1000ms, false);
		this.climbing.pst.QueueAnim("rummage_pst", false, null).OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.WantsToClimbTree, false);
	}

	// Token: 0x0600074E RID: 1870 RVA: 0x000ADA22 File Offset: 0x000ABC22
	private static void SetTarget(TreeClimbStates.Instance smi)
	{
		smi.sm.target.Set(smi.GetSMI<ClimbableTreeMonitor.Instance>().climbTarget, smi, false);
	}

	// Token: 0x0600074F RID: 1871 RVA: 0x00167D6C File Offset: 0x00165F6C
	private static bool ReserveClimbable(TreeClimbStates.Instance smi)
	{
		GameObject gameObject = smi.sm.target.Get(smi);
		if (gameObject != null && !gameObject.HasTag(GameTags.Creatures.ReservedByCreature))
		{
			gameObject.AddTag(GameTags.Creatures.ReservedByCreature);
			return true;
		}
		return false;
	}

	// Token: 0x06000750 RID: 1872 RVA: 0x00167DB0 File Offset: 0x00165FB0
	private static void UnreserveClimbable(TreeClimbStates.Instance smi)
	{
		GameObject gameObject = smi.sm.target.Get(smi);
		if (gameObject != null)
		{
			gameObject.RemoveTag(GameTags.Creatures.ReservedByCreature);
		}
	}

	// Token: 0x06000751 RID: 1873 RVA: 0x00167DE4 File Offset: 0x00165FE4
	private static void Rummage(TreeClimbStates.Instance smi, float dt)
	{
		GameObject gameObject = smi.sm.target.Get(smi);
		if (gameObject != null)
		{
			ForestTreeSeedMonitor component = gameObject.GetComponent<ForestTreeSeedMonitor>();
			if (component != null)
			{
				component.ExtractExtraSeed();
				return;
			}
			Storage component2 = gameObject.GetComponent<Storage>();
			if (component2 && component2.items.Count > 0)
			{
				int index = UnityEngine.Random.Range(0, component2.items.Count - 1);
				GameObject gameObject2 = component2.items[index];
				Pickupable pickupable = gameObject2 ? gameObject2.GetComponent<Pickupable>() : null;
				if (pickupable && pickupable.UnreservedAmount > 0.01f)
				{
					smi.Toss(pickupable);
				}
			}
		}
	}

	// Token: 0x06000752 RID: 1874 RVA: 0x000ADA42 File Offset: 0x000ABC42
	private static int GetClimbableCell(TreeClimbStates.Instance smi)
	{
		return Grid.PosToCell(smi.sm.target.Get(smi));
	}

	// Token: 0x0400056C RID: 1388
	public GameStateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def>.ApproachSubState<Uprootable> moving;

	// Token: 0x0400056D RID: 1389
	public TreeClimbStates.ClimbState climbing;

	// Token: 0x0400056E RID: 1390
	public GameStateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def>.State behaviourcomplete;

	// Token: 0x0400056F RID: 1391
	public StateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def>.TargetParameter target;

	// Token: 0x0200021C RID: 540
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200021D RID: 541
	public new class Instance : GameStateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def>.GameInstance
	{
		// Token: 0x06000756 RID: 1878 RVA: 0x000ADA78 File Offset: 0x000ABC78
		public Instance(Chore<TreeClimbStates.Instance> chore, TreeClimbStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.WantsToClimbTree);
			this.storage = base.GetComponent<Storage>();
		}

		// Token: 0x06000757 RID: 1879 RVA: 0x00167E9C File Offset: 0x0016609C
		public void Toss(Pickupable pu)
		{
			Pickupable pickupable = pu.Take(Mathf.Min(1f, pu.UnreservedAmount));
			if (pickupable != null)
			{
				this.storage.Store(pickupable.gameObject, true, false, true, false);
				this.storage.Drop(pickupable.gameObject, true);
				this.Throw(pickupable.gameObject);
			}
		}

		// Token: 0x06000758 RID: 1880 RVA: 0x00167F00 File Offset: 0x00166100
		private void Throw(GameObject ore_go)
		{
			Vector3 position = base.transform.GetPosition();
			position.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
			int num = Grid.PosToCell(position);
			int num2 = Grid.CellAbove(num);
			Vector2 zero;
			if ((Grid.IsValidCell(num) && Grid.Solid[num]) || (Grid.IsValidCell(num2) && Grid.Solid[num2]))
			{
				zero = Vector2.zero;
			}
			else
			{
				position.y += 0.5f;
				zero = new Vector2(UnityEngine.Random.Range(TreeClimbStates.Instance.VEL_MIN.x, TreeClimbStates.Instance.VEL_MAX.x), UnityEngine.Random.Range(TreeClimbStates.Instance.VEL_MIN.y, TreeClimbStates.Instance.VEL_MAX.y));
			}
			ore_go.transform.SetPosition(position);
			if (GameComps.Fallers.Has(ore_go))
			{
				GameComps.Fallers.Remove(ore_go);
			}
			GameComps.Fallers.Add(ore_go, zero);
		}

		// Token: 0x04000570 RID: 1392
		private Storage storage;

		// Token: 0x04000571 RID: 1393
		private static readonly Vector2 VEL_MIN = new Vector2(-1f, 2f);

		// Token: 0x04000572 RID: 1394
		private static readonly Vector2 VEL_MAX = new Vector2(1f, 4f);
	}

	// Token: 0x0200021E RID: 542
	public class ClimbState : GameStateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def>.State
	{
		// Token: 0x04000573 RID: 1395
		public GameStateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def>.State pre;

		// Token: 0x04000574 RID: 1396
		public GameStateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def>.State loop;

		// Token: 0x04000575 RID: 1397
		public GameStateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def>.State pst;
	}
}
