using System;
using UnityEngine;

// Token: 0x02000A04 RID: 2564
public class ClimbableTreeMonitor : GameStateMachine<ClimbableTreeMonitor, ClimbableTreeMonitor.Instance, IStateMachineTarget, ClimbableTreeMonitor.Def>
{
	// Token: 0x06002E99 RID: 11929 RVA: 0x00202FB0 File Offset: 0x002011B0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleBehaviour(GameTags.Creatures.WantsToClimbTree, (ClimbableTreeMonitor.Instance smi) => smi.UpdateHasClimbable(), delegate(ClimbableTreeMonitor.Instance smi)
		{
			smi.OnClimbComplete();
		});
	}

	// Token: 0x04001FEC RID: 8172
	private const int MAX_NAV_COST = 2147483647;

	// Token: 0x02000A05 RID: 2565
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04001FED RID: 8173
		public float searchMinInterval = 60f;

		// Token: 0x04001FEE RID: 8174
		public float searchMaxInterval = 120f;
	}

	// Token: 0x02000A06 RID: 2566
	public new class Instance : GameStateMachine<ClimbableTreeMonitor, ClimbableTreeMonitor.Instance, IStateMachineTarget, ClimbableTreeMonitor.Def>.GameInstance
	{
		// Token: 0x06002E9C RID: 11932 RVA: 0x000C2AA5 File Offset: 0x000C0CA5
		public Instance(IStateMachineTarget master, ClimbableTreeMonitor.Def def) : base(master, def)
		{
			this.RefreshSearchTime();
		}

		// Token: 0x06002E9D RID: 11933 RVA: 0x000C2AB5 File Offset: 0x000C0CB5
		private void RefreshSearchTime()
		{
			this.nextSearchTime = Time.time + Mathf.Lerp(base.def.searchMinInterval, base.def.searchMaxInterval, UnityEngine.Random.value);
		}

		// Token: 0x06002E9E RID: 11934 RVA: 0x000C2AE3 File Offset: 0x000C0CE3
		public bool UpdateHasClimbable()
		{
			if (this.climbTarget == null)
			{
				if (Time.time < this.nextSearchTime)
				{
					return false;
				}
				this.FindClimbableTree();
				this.RefreshSearchTime();
			}
			return this.climbTarget != null;
		}

		// Token: 0x06002E9F RID: 11935 RVA: 0x00203014 File Offset: 0x00201214
		private static bool FindClimbableTreeVisitor(object obj, ClimbableTreeMonitor.Instance.FindClimableTreeContext context)
		{
			KMonoBehaviour kmonoBehaviour = obj as KMonoBehaviour;
			if (kmonoBehaviour.HasTag(GameTags.Creatures.ReservedByCreature))
			{
				return true;
			}
			int cell = Grid.PosToCell(kmonoBehaviour);
			if (!context.navigator.CanReach(cell))
			{
				return true;
			}
			ForestTreeSeedMonitor component = kmonoBehaviour.GetComponent<ForestTreeSeedMonitor>();
			StorageLocker component2 = kmonoBehaviour.GetComponent<StorageLocker>();
			if (component != null)
			{
				if (!component.ExtraSeedAvailable)
				{
					return true;
				}
			}
			else
			{
				if (!(component2 != null))
				{
					return true;
				}
				Storage component3 = component2.GetComponent<Storage>();
				if (!component3.allowItemRemoval)
				{
					return true;
				}
				if (component3.IsEmpty())
				{
					return true;
				}
			}
			context.targets.Add(kmonoBehaviour);
			return true;
		}

		// Token: 0x06002EA0 RID: 11936 RVA: 0x002030A8 File Offset: 0x002012A8
		private void FindClimbableTree()
		{
			this.climbTarget = null;
			Vector3 position = base.master.transform.GetPosition();
			Extents extents = new Extents(Grid.PosToCell(position), 10);
			ClimbableTreeMonitor.Instance.FindClimableTreeContext findClimableTreeContext;
			findClimableTreeContext.navigator = base.GetComponent<Navigator>();
			findClimableTreeContext.targets = ListPool<KMonoBehaviour, ClimbableTreeMonitor>.Allocate();
			GameScenePartitioner.Instance.AsyncSafeVisit<ClimbableTreeMonitor.Instance.FindClimableTreeContext>(extents.x, extents.y, extents.width, extents.height, GameScenePartitioner.Instance.plants, new Func<object, ClimbableTreeMonitor.Instance.FindClimableTreeContext, bool>(ClimbableTreeMonitor.Instance.FindClimbableTreeVisitor), findClimableTreeContext);
			GameScenePartitioner.Instance.AsyncSafeVisit<ClimbableTreeMonitor.Instance.FindClimableTreeContext>(extents.x, extents.y, extents.width, extents.height, GameScenePartitioner.Instance.completeBuildings, new Func<object, ClimbableTreeMonitor.Instance.FindClimableTreeContext, bool>(ClimbableTreeMonitor.Instance.FindClimbableTreeVisitor), findClimableTreeContext);
			if (findClimableTreeContext.targets.Count > 0)
			{
				int index = UnityEngine.Random.Range(0, findClimableTreeContext.targets.Count);
				KMonoBehaviour kmonoBehaviour = findClimableTreeContext.targets[index];
				this.climbTarget = kmonoBehaviour.gameObject;
			}
			findClimableTreeContext.targets.Recycle();
		}

		// Token: 0x06002EA1 RID: 11937 RVA: 0x000C2B1A File Offset: 0x000C0D1A
		public void OnClimbComplete()
		{
			this.climbTarget = null;
		}

		// Token: 0x04001FEF RID: 8175
		public GameObject climbTarget;

		// Token: 0x04001FF0 RID: 8176
		public float nextSearchTime;

		// Token: 0x02000A07 RID: 2567
		private struct FindClimableTreeContext
		{
			// Token: 0x04001FF1 RID: 8177
			public Navigator navigator;

			// Token: 0x04001FF2 RID: 8178
			public ListPool<KMonoBehaviour, ClimbableTreeMonitor>.PooledList targets;
		}
	}
}
