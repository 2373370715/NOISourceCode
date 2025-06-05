using System;
using System.Collections.Generic;
using KSerialization;
using TUNING;
using UnityEngine;

// Token: 0x020010C9 RID: 4297
public class ClusterFogOfWarManager : GameStateMachine<ClusterFogOfWarManager, ClusterFogOfWarManager.Instance, IStateMachineTarget, ClusterFogOfWarManager.Def>
{
	// Token: 0x060057A1 RID: 22433 RVA: 0x00294374 File Offset: 0x00292574
	public override void InitializeStates(out StateMachine.BaseState defaultState)
	{
		defaultState = this.root;
		this.root.Enter(delegate(ClusterFogOfWarManager.Instance smi)
		{
			smi.Initialize();
		}).EventHandler(GameHashes.DiscoveredWorldsChanged, (ClusterFogOfWarManager.Instance smi) => Game.Instance, delegate(ClusterFogOfWarManager.Instance smi)
		{
			smi.UpdateRevealedCellsFromDiscoveredWorlds();
		});
	}

	// Token: 0x04003DE6 RID: 15846
	public const int AUTOMATIC_PEEK_RADIUS = 2;

	// Token: 0x020010CA RID: 4298
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020010CB RID: 4299
	public new class Instance : GameStateMachine<ClusterFogOfWarManager, ClusterFogOfWarManager.Instance, IStateMachineTarget, ClusterFogOfWarManager.Def>.GameInstance
	{
		// Token: 0x060057A4 RID: 22436 RVA: 0x000DDA12 File Offset: 0x000DBC12
		public Instance(IStateMachineTarget master, ClusterFogOfWarManager.Def def) : base(master, def)
		{
		}

		// Token: 0x060057A5 RID: 22437 RVA: 0x000DDA27 File Offset: 0x000DBC27
		public void Initialize()
		{
			this.UpdateRevealedCellsFromDiscoveredWorlds();
			this.EnsureRevealedTilesHavePeek();
		}

		// Token: 0x060057A6 RID: 22438 RVA: 0x000DDA35 File Offset: 0x000DBC35
		public ClusterRevealLevel GetCellRevealLevel(AxialI location)
		{
			if (this.GetRevealCompleteFraction(location) >= 1f)
			{
				return ClusterRevealLevel.Visible;
			}
			if (this.GetRevealCompleteFraction(location) > 0f)
			{
				return ClusterRevealLevel.Peeked;
			}
			return ClusterRevealLevel.Hidden;
		}

		// Token: 0x060057A7 RID: 22439 RVA: 0x000DDA58 File Offset: 0x000DBC58
		public void DEBUG_REVEAL_ENTIRE_MAP()
		{
			this.RevealLocation(AxialI.ZERO, 100);
		}

		// Token: 0x060057A8 RID: 22440 RVA: 0x000DDA67 File Offset: 0x000DBC67
		public bool IsLocationRevealed(AxialI location)
		{
			return this.GetRevealCompleteFraction(location) >= 1f;
		}

		// Token: 0x060057A9 RID: 22441 RVA: 0x002943FC File Offset: 0x002925FC
		private void EnsureRevealedTilesHavePeek()
		{
			foreach (KeyValuePair<AxialI, List<ClusterGridEntity>> keyValuePair in ClusterGrid.Instance.cellContents)
			{
				if (this.IsLocationRevealed(keyValuePair.Key))
				{
					this.PeekLocation(keyValuePair.Key, 2);
				}
			}
		}

		// Token: 0x060057AA RID: 22442 RVA: 0x0029446C File Offset: 0x0029266C
		public void PeekLocation(AxialI location, int radius)
		{
			foreach (AxialI key in AxialUtil.GetAllPointsWithinRadius(location, radius))
			{
				if (this.m_revealPointsByCell.ContainsKey(key))
				{
					this.m_revealPointsByCell[key] = Mathf.Max(this.m_revealPointsByCell[key], 0.01f);
				}
				else
				{
					this.m_revealPointsByCell[key] = 0.01f;
				}
			}
		}

		// Token: 0x060057AB RID: 22443 RVA: 0x002944FC File Offset: 0x002926FC
		public void RevealLocation(AxialI location, int radius = 0)
		{
			if (ClusterGrid.Instance.GetHiddenEntitiesOfLayerAtCell(location, EntityLayer.Asteroid).Count > 0 || ClusterGrid.Instance.GetVisibleEntityOfLayerAtCell(location, EntityLayer.Asteroid) != null)
			{
				radius = Mathf.Max(radius, 1);
			}
			bool flag = false;
			foreach (AxialI cell in AxialUtil.GetAllPointsWithinRadius(location, radius))
			{
				flag |= this.RevealCellIfValid(cell);
			}
			if (flag)
			{
				Game.Instance.Trigger(-1991583975, location);
			}
		}

		// Token: 0x060057AC RID: 22444 RVA: 0x002945A0 File Offset: 0x002927A0
		public void EarnRevealPointsForLocation(AxialI location, float points)
		{
			global::Debug.Assert(ClusterGrid.Instance.IsValidCell(location), string.Format("EarnRevealPointsForLocation called with invalid location: {0}", location));
			if (this.IsLocationRevealed(location))
			{
				return;
			}
			if (this.m_revealPointsByCell.ContainsKey(location))
			{
				Dictionary<AxialI, float> revealPointsByCell = this.m_revealPointsByCell;
				revealPointsByCell[location] += points;
			}
			else
			{
				this.m_revealPointsByCell[location] = points;
				Game.Instance.Trigger(-1554423969, location);
			}
			if (this.IsLocationRevealed(location))
			{
				this.RevealLocation(location, 0);
				this.PeekLocation(location, 2);
				Game.Instance.Trigger(-1991583975, location);
			}
		}

		// Token: 0x060057AD RID: 22445 RVA: 0x00294650 File Offset: 0x00292850
		public float GetRevealCompleteFraction(AxialI location)
		{
			if (!ClusterGrid.Instance.IsValidCell(location))
			{
				global::Debug.LogError(string.Format("GetRevealCompleteFraction called with invalid location: {0}, {1}", location.r, location.q));
			}
			if (DebugHandler.RevealFogOfWar)
			{
				return 1f;
			}
			float num;
			if (this.m_revealPointsByCell.TryGetValue(location, out num))
			{
				return Mathf.Min(num / ROCKETRY.CLUSTER_FOW.POINTS_TO_REVEAL, 1f);
			}
			return 0f;
		}

		// Token: 0x060057AE RID: 22446 RVA: 0x000DDA7A File Offset: 0x000DBC7A
		private bool RevealCellIfValid(AxialI cell)
		{
			if (!ClusterGrid.Instance.IsValidCell(cell))
			{
				return false;
			}
			if (this.IsLocationRevealed(cell))
			{
				return false;
			}
			this.m_revealPointsByCell[cell] = ROCKETRY.CLUSTER_FOW.POINTS_TO_REVEAL;
			this.PeekLocation(cell, 2);
			return true;
		}

		// Token: 0x060057AF RID: 22447 RVA: 0x002946C4 File Offset: 0x002928C4
		public bool GetUnrevealedLocationWithinRadius(AxialI center, int radius, out AxialI result)
		{
			for (int i = 0; i <= radius; i++)
			{
				foreach (AxialI axialI in AxialUtil.GetRing(center, i))
				{
					if (ClusterGrid.Instance.IsValidCell(axialI) && !this.IsLocationRevealed(axialI))
					{
						result = axialI;
						return true;
					}
				}
			}
			result = AxialI.ZERO;
			return false;
		}

		// Token: 0x060057B0 RID: 22448 RVA: 0x0029474C File Offset: 0x0029294C
		public void UpdateRevealedCellsFromDiscoveredWorlds()
		{
			int radius = DlcManager.IsExpansion1Active() ? 0 : 2;
			foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
			{
				if (worldContainer.IsDiscovered && !DebugHandler.RevealFogOfWar)
				{
					this.RevealLocation(worldContainer.GetComponent<ClusterGridEntity>().Location, radius);
				}
			}
		}

		// Token: 0x04003DE7 RID: 15847
		[Serialize]
		private Dictionary<AxialI, float> m_revealPointsByCell = new Dictionary<AxialI, float>();
	}
}
