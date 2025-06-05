using System;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x020011EB RID: 4587
public class MoltDropperMonitor : GameStateMachine<MoltDropperMonitor, MoltDropperMonitor.Instance, IStateMachineTarget, MoltDropperMonitor.Def>
{
	// Token: 0x06005D3F RID: 23871 RVA: 0x002AC03C File Offset: 0x002AA23C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.root.EventHandler(GameHashes.NewDay, (MoltDropperMonitor.Instance smi) => GameClock.Instance, delegate(MoltDropperMonitor.Instance smi)
		{
			smi.spawnedThisCycle = false;
		});
		this.satisfied.UpdateTransition(this.drop, (MoltDropperMonitor.Instance smi, float dt) => smi.ShouldDropElement(), UpdateRate.SIM_4000ms, false);
		this.drop.DefaultState(this.drop.dropping);
		this.drop.dropping.EnterTransition(this.drop.complete, (MoltDropperMonitor.Instance smi) => !smi.def.synchWithBehaviour).ToggleBehaviour(GameTags.Creatures.ReadyToMolt, (MoltDropperMonitor.Instance smi) => true, delegate(MoltDropperMonitor.Instance smi)
		{
			smi.GoTo(this.drop.complete);
		});
		this.drop.complete.Enter(delegate(MoltDropperMonitor.Instance smi)
		{
			smi.Drop();
		}).TriggerOnEnter(GameHashes.Molt, null).EventTransition(GameHashes.NewDay, (MoltDropperMonitor.Instance smi) => GameClock.Instance, this.satisfied, null);
	}

	// Token: 0x04004271 RID: 17009
	public StateMachine<MoltDropperMonitor, MoltDropperMonitor.Instance, IStateMachineTarget, MoltDropperMonitor.Def>.BoolParameter droppedThisCycle = new StateMachine<MoltDropperMonitor, MoltDropperMonitor.Instance, IStateMachineTarget, MoltDropperMonitor.Def>.BoolParameter(false);

	// Token: 0x04004272 RID: 17010
	public GameStateMachine<MoltDropperMonitor, MoltDropperMonitor.Instance, IStateMachineTarget, MoltDropperMonitor.Def>.State satisfied;

	// Token: 0x04004273 RID: 17011
	public MoltDropperMonitor.DropStates drop;

	// Token: 0x020011EC RID: 4588
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04004274 RID: 17012
		public bool synchWithBehaviour;

		// Token: 0x04004275 RID: 17013
		public string onGrowDropID;

		// Token: 0x04004276 RID: 17014
		public float massToDrop;

		// Token: 0x04004277 RID: 17015
		public string amountName;

		// Token: 0x04004278 RID: 17016
		public Func<MoltDropperMonitor.Instance, bool> isReadyToMolt;
	}

	// Token: 0x020011ED RID: 4589
	public class DropStates : GameStateMachine<MoltDropperMonitor, MoltDropperMonitor.Instance, IStateMachineTarget, MoltDropperMonitor.Def>.State
	{
		// Token: 0x04004279 RID: 17017
		public GameStateMachine<MoltDropperMonitor, MoltDropperMonitor.Instance, IStateMachineTarget, MoltDropperMonitor.Def>.State dropping;

		// Token: 0x0400427A RID: 17018
		public GameStateMachine<MoltDropperMonitor, MoltDropperMonitor.Instance, IStateMachineTarget, MoltDropperMonitor.Def>.State complete;
	}

	// Token: 0x020011EE RID: 4590
	public new class Instance : GameStateMachine<MoltDropperMonitor, MoltDropperMonitor.Instance, IStateMachineTarget, MoltDropperMonitor.Def>.GameInstance
	{
		// Token: 0x06005D44 RID: 23876 RVA: 0x002AC1C8 File Offset: 0x002AA3C8
		public Instance(IStateMachineTarget master, MoltDropperMonitor.Def def) : base(master, def)
		{
			if (!string.IsNullOrEmpty(def.amountName))
			{
				AmountInstance amountInstance = Db.Get().Amounts.Get(def.amountName).Lookup(base.smi.gameObject);
				amountInstance.OnMaxValueReached = (System.Action)Delegate.Combine(amountInstance.OnMaxValueReached, new System.Action(this.OnAmountMaxValueReached));
			}
		}

		// Token: 0x06005D45 RID: 23877 RVA: 0x000E1497 File Offset: 0x000DF697
		private void OnAmountMaxValueReached()
		{
			this.lastTineAmountReachedMax = GameClock.Instance.GetTime();
		}

		// Token: 0x06005D46 RID: 23878 RVA: 0x002AC230 File Offset: 0x002AA430
		protected override void OnCleanUp()
		{
			if (!string.IsNullOrEmpty(base.def.amountName))
			{
				AmountInstance amountInstance = Db.Get().Amounts.Get(base.def.amountName).Lookup(base.smi.gameObject);
				amountInstance.OnMaxValueReached = (System.Action)Delegate.Remove(amountInstance.OnMaxValueReached, new System.Action(this.OnAmountMaxValueReached));
			}
			base.OnCleanUp();
		}

		// Token: 0x06005D47 RID: 23879 RVA: 0x000E14A9 File Offset: 0x000DF6A9
		public bool ShouldDropElement()
		{
			return base.def.isReadyToMolt(this);
		}

		// Token: 0x06005D48 RID: 23880 RVA: 0x002AC2A0 File Offset: 0x002AA4A0
		public void Drop()
		{
			GameObject gameObject = Scenario.SpawnPrefab(this.GetDropSpawnLocation(), 0, 0, base.def.onGrowDropID, Grid.SceneLayer.Ore);
			gameObject.SetActive(true);
			gameObject.GetComponent<PrimaryElement>().Mass = base.def.massToDrop;
			this.spawnedThisCycle = true;
			this.timeOfLastDrop = GameClock.Instance.GetTime();
			if (!string.IsNullOrEmpty(base.def.amountName))
			{
				AmountInstance amountInstance = Db.Get().Amounts.Get(base.def.amountName).Lookup(base.smi.gameObject);
				amountInstance.value = amountInstance.GetMin();
			}
		}

		// Token: 0x06005D49 RID: 23881 RVA: 0x002AC344 File Offset: 0x002AA544
		private int GetDropSpawnLocation()
		{
			int num = Grid.PosToCell(base.gameObject);
			int num2 = Grid.CellAbove(num);
			if (Grid.IsValidCell(num2) && !Grid.Solid[num2])
			{
				return num2;
			}
			return num;
		}

		// Token: 0x0400427B RID: 17019
		[MyCmpGet]
		public KPrefabID prefabID;

		// Token: 0x0400427C RID: 17020
		[Serialize]
		public bool spawnedThisCycle;

		// Token: 0x0400427D RID: 17021
		[Serialize]
		public float timeOfLastDrop;

		// Token: 0x0400427E RID: 17022
		[Serialize]
		public float lastTineAmountReachedMax;
	}
}
