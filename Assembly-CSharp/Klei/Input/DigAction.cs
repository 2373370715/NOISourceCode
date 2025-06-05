using System;
using Klei.Actions;
using UnityEngine;

namespace Klei.Input
{
	// Token: 0x02003D09 RID: 15625
	[ActionType("InterfaceTool", "Dig", true)]
	public abstract class DigAction
	{
		// Token: 0x0600EFFF RID: 61439 RVA: 0x004EBA58 File Offset: 0x004E9C58
		public void Uproot(int cell)
		{
			ListPool<ScenePartitionerEntry, GameScenePartitioner>.PooledList pooledList = ListPool<ScenePartitionerEntry, GameScenePartitioner>.Allocate();
			int x_bottomLeft;
			int y_bottomLeft;
			Grid.CellToXY(cell, out x_bottomLeft, out y_bottomLeft);
			GameScenePartitioner.Instance.GatherEntries(x_bottomLeft, y_bottomLeft, 1, 1, GameScenePartitioner.Instance.plants, pooledList);
			if (pooledList.Count > 0)
			{
				this.EntityDig((pooledList[0].obj as Component).GetComponent<IDigActionEntity>());
			}
			pooledList.Recycle();
		}

		// Token: 0x0600F000 RID: 61440
		public abstract void Dig(int cell, int distFromOrigin);

		// Token: 0x0600F001 RID: 61441
		protected abstract void EntityDig(IDigActionEntity digAction);
	}
}
