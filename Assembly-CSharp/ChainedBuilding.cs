using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020010A1 RID: 4257
public class ChainedBuilding : GameStateMachine<ChainedBuilding, ChainedBuilding.StatesInstance, IStateMachineTarget, ChainedBuilding.Def>
{
	// Token: 0x0600567A RID: 22138 RVA: 0x0029045C File Offset: 0x0028E65C
	public override void InitializeStates(out StateMachine.BaseState defaultState)
	{
		defaultState = this.unlinked;
		StatusItem statusItem = new StatusItem("NotLinkedToHeadStatusItem", BUILDING.STATUSITEMS.NOTLINKEDTOHEAD.NAME, BUILDING.STATUSITEMS.NOTLINKEDTOHEAD.TOOLTIP, "status_item_not_linked", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, 129022, true, null);
		statusItem.resolveTooltipCallback = delegate(string tooltip, object obj)
		{
			ChainedBuilding.StatesInstance statesInstance = (ChainedBuilding.StatesInstance)obj;
			return tooltip.Replace("{headBuilding}", Strings.Get("STRINGS.BUILDINGS.PREFABS." + statesInstance.def.headBuildingTag.Name.ToUpper() + ".NAME")).Replace("{linkBuilding}", Strings.Get("STRINGS.BUILDINGS.PREFABS." + statesInstance.def.linkBuildingTag.Name.ToUpper() + ".NAME"));
		};
		this.root.OnSignal(this.doRelink, this.DEBUG_relink);
		this.unlinked.ParamTransition<bool>(this.isConnectedToHead, this.linked, GameStateMachine<ChainedBuilding, ChainedBuilding.StatesInstance, IStateMachineTarget, ChainedBuilding.Def>.IsTrue).ToggleStatusItem(statusItem, (ChainedBuilding.StatesInstance smi) => smi);
		this.linked.ParamTransition<bool>(this.isConnectedToHead, this.unlinked, GameStateMachine<ChainedBuilding, ChainedBuilding.StatesInstance, IStateMachineTarget, ChainedBuilding.Def>.IsFalse);
		this.DEBUG_relink.Enter(delegate(ChainedBuilding.StatesInstance smi)
		{
			smi.DEBUG_Relink();
		});
	}

	// Token: 0x04003D43 RID: 15683
	private GameStateMachine<ChainedBuilding, ChainedBuilding.StatesInstance, IStateMachineTarget, ChainedBuilding.Def>.State unlinked;

	// Token: 0x04003D44 RID: 15684
	private GameStateMachine<ChainedBuilding, ChainedBuilding.StatesInstance, IStateMachineTarget, ChainedBuilding.Def>.State linked;

	// Token: 0x04003D45 RID: 15685
	private GameStateMachine<ChainedBuilding, ChainedBuilding.StatesInstance, IStateMachineTarget, ChainedBuilding.Def>.State DEBUG_relink;

	// Token: 0x04003D46 RID: 15686
	private StateMachine<ChainedBuilding, ChainedBuilding.StatesInstance, IStateMachineTarget, ChainedBuilding.Def>.BoolParameter isConnectedToHead = new StateMachine<ChainedBuilding, ChainedBuilding.StatesInstance, IStateMachineTarget, ChainedBuilding.Def>.BoolParameter();

	// Token: 0x04003D47 RID: 15687
	private StateMachine<ChainedBuilding, ChainedBuilding.StatesInstance, IStateMachineTarget, ChainedBuilding.Def>.Signal doRelink;

	// Token: 0x020010A2 RID: 4258
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04003D48 RID: 15688
		public Tag headBuildingTag;

		// Token: 0x04003D49 RID: 15689
		public Tag linkBuildingTag;

		// Token: 0x04003D4A RID: 15690
		public ObjectLayer objectLayer;
	}

	// Token: 0x020010A3 RID: 4259
	public class StatesInstance : GameStateMachine<ChainedBuilding, ChainedBuilding.StatesInstance, IStateMachineTarget, ChainedBuilding.Def>.GameInstance
	{
		// Token: 0x0600567D RID: 22141 RVA: 0x0029056C File Offset: 0x0028E76C
		public StatesInstance(IStateMachineTarget master, ChainedBuilding.Def def) : base(master, def)
		{
			BuildingDef def2 = master.GetComponent<Building>().Def;
			this.widthInCells = def2.WidthInCells;
			int cell = Grid.PosToCell(this);
			this.neighbourCheckCells = new List<int>
			{
				Grid.OffsetCell(cell, -(this.widthInCells - 1) / 2 - 1, 0),
				Grid.OffsetCell(cell, this.widthInCells / 2 + 1, 0)
			};
		}

		// Token: 0x0600567E RID: 22142 RVA: 0x002905DC File Offset: 0x0028E7DC
		public override void StartSM()
		{
			base.StartSM();
			bool foundHead = false;
			HashSetPool<ChainedBuilding.StatesInstance, ChainedBuilding.StatesInstance>.PooledHashSet pooledHashSet = HashSetPool<ChainedBuilding.StatesInstance, ChainedBuilding.StatesInstance>.Allocate();
			this.CollectToChain(ref pooledHashSet, ref foundHead, null);
			this.PropogateFoundHead(foundHead, pooledHashSet);
			this.PropagateChangedEvent(this, pooledHashSet);
			pooledHashSet.Recycle();
		}

		// Token: 0x0600567F RID: 22143 RVA: 0x00290618 File Offset: 0x0028E818
		public void DEBUG_Relink()
		{
			bool foundHead = false;
			HashSetPool<ChainedBuilding.StatesInstance, ChainedBuilding.StatesInstance>.PooledHashSet pooledHashSet = HashSetPool<ChainedBuilding.StatesInstance, ChainedBuilding.StatesInstance>.Allocate();
			this.CollectToChain(ref pooledHashSet, ref foundHead, null);
			this.PropogateFoundHead(foundHead, pooledHashSet);
			pooledHashSet.Recycle();
		}

		// Token: 0x06005680 RID: 22144 RVA: 0x00290648 File Offset: 0x0028E848
		protected override void OnCleanUp()
		{
			HashSetPool<ChainedBuilding.StatesInstance, ChainedBuilding.StatesInstance>.PooledHashSet pooledHashSet = HashSetPool<ChainedBuilding.StatesInstance, ChainedBuilding.StatesInstance>.Allocate();
			foreach (int cell in this.neighbourCheckCells)
			{
				bool foundHead = false;
				this.CollectNeighbourToChain(cell, ref pooledHashSet, ref foundHead, this);
				this.PropogateFoundHead(foundHead, pooledHashSet);
				this.PropagateChangedEvent(this, pooledHashSet);
				pooledHashSet.Clear();
			}
			pooledHashSet.Recycle();
			base.OnCleanUp();
		}

		// Token: 0x06005681 RID: 22145 RVA: 0x002906CC File Offset: 0x0028E8CC
		public HashSet<ChainedBuilding.StatesInstance> GetLinkedBuildings(ref HashSetPool<ChainedBuilding.StatesInstance, ChainedBuilding.StatesInstance>.PooledHashSet chain)
		{
			bool flag = false;
			this.CollectToChain(ref chain, ref flag, null);
			return chain;
		}

		// Token: 0x06005682 RID: 22146 RVA: 0x002906E8 File Offset: 0x0028E8E8
		private void PropogateFoundHead(bool foundHead, HashSet<ChainedBuilding.StatesInstance> chain)
		{
			foreach (ChainedBuilding.StatesInstance statesInstance in chain)
			{
				statesInstance.sm.isConnectedToHead.Set(foundHead, statesInstance, false);
			}
		}

		// Token: 0x06005683 RID: 22147 RVA: 0x00290744 File Offset: 0x0028E944
		private void PropagateChangedEvent(ChainedBuilding.StatesInstance changedLink, HashSet<ChainedBuilding.StatesInstance> chain)
		{
			foreach (ChainedBuilding.StatesInstance statesInstance in chain)
			{
				statesInstance.Trigger(-1009905786, changedLink);
			}
		}

		// Token: 0x06005684 RID: 22148 RVA: 0x00290798 File Offset: 0x0028E998
		private void CollectToChain(ref HashSetPool<ChainedBuilding.StatesInstance, ChainedBuilding.StatesInstance>.PooledHashSet chain, ref bool foundHead, ChainedBuilding.StatesInstance ignoredLink = null)
		{
			if (ignoredLink != null && ignoredLink == this)
			{
				return;
			}
			if (chain.Contains(this))
			{
				return;
			}
			chain.Add(this);
			if (base.HasTag(base.def.headBuildingTag))
			{
				foundHead = true;
			}
			foreach (int cell in this.neighbourCheckCells)
			{
				this.CollectNeighbourToChain(cell, ref chain, ref foundHead, null);
			}
		}

		// Token: 0x06005685 RID: 22149 RVA: 0x00290820 File Offset: 0x0028EA20
		private void CollectNeighbourToChain(int cell, ref HashSetPool<ChainedBuilding.StatesInstance, ChainedBuilding.StatesInstance>.PooledHashSet chain, ref bool foundHead, ChainedBuilding.StatesInstance ignoredLink = null)
		{
			GameObject gameObject = Grid.Objects[cell, (int)base.def.objectLayer];
			if (gameObject == null)
			{
				return;
			}
			KPrefabID component = gameObject.GetComponent<KPrefabID>();
			if (!component.HasTag(base.def.linkBuildingTag) && !component.IsPrefabID(base.def.headBuildingTag))
			{
				return;
			}
			ChainedBuilding.StatesInstance smi = gameObject.GetSMI<ChainedBuilding.StatesInstance>();
			if (smi != null)
			{
				smi.CollectToChain(ref chain, ref foundHead, ignoredLink);
			}
		}

		// Token: 0x04003D4B RID: 15691
		private int widthInCells;

		// Token: 0x04003D4C RID: 15692
		private List<int> neighbourCheckCells;
	}
}
