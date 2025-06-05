using System;
using UnityEngine;

// Token: 0x02000A22 RID: 2594
public class CreatureFallMonitor : GameStateMachine<CreatureFallMonitor, CreatureFallMonitor.Instance, IStateMachineTarget, CreatureFallMonitor.Def>
{
	// Token: 0x06002F25 RID: 12069 RVA: 0x000C3030 File Offset: 0x000C1230
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.grounded;
		this.grounded.ToggleBehaviour(GameTags.Creatures.Falling, (CreatureFallMonitor.Instance smi) => smi.ShouldFall(), null);
	}

	// Token: 0x04002051 RID: 8273
	public static float FLOOR_DISTANCE = -0.065f;

	// Token: 0x04002052 RID: 8274
	public GameStateMachine<CreatureFallMonitor, CreatureFallMonitor.Instance, IStateMachineTarget, CreatureFallMonitor.Def>.State grounded;

	// Token: 0x04002053 RID: 8275
	public GameStateMachine<CreatureFallMonitor, CreatureFallMonitor.Instance, IStateMachineTarget, CreatureFallMonitor.Def>.State falling;

	// Token: 0x02000A23 RID: 2595
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04002054 RID: 8276
		public bool canSwim;

		// Token: 0x04002055 RID: 8277
		public bool checkHead = true;
	}

	// Token: 0x02000A24 RID: 2596
	public new class Instance : GameStateMachine<CreatureFallMonitor, CreatureFallMonitor.Instance, IStateMachineTarget, CreatureFallMonitor.Def>.GameInstance
	{
		// Token: 0x06002F29 RID: 12073 RVA: 0x000C308E File Offset: 0x000C128E
		public Instance(IStateMachineTarget master, CreatureFallMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x06002F2A RID: 12074 RVA: 0x00204EBC File Offset: 0x002030BC
		public void SnapToGround()
		{
			Vector3 position = base.smi.transform.GetPosition();
			Vector3 position2 = Grid.CellToPosCBC(Grid.PosToCell(position), Grid.SceneLayer.Creatures);
			position2.x = position.x;
			base.smi.transform.SetPosition(position2);
			if (this.navigator.IsValidNavType(NavType.Floor))
			{
				this.navigator.SetCurrentNavType(NavType.Floor);
				return;
			}
			if (this.navigator.IsValidNavType(NavType.Hover))
			{
				this.navigator.SetCurrentNavType(NavType.Hover);
			}
		}

		// Token: 0x06002F2B RID: 12075 RVA: 0x00204F3C File Offset: 0x0020313C
		public bool ShouldFall()
		{
			if (this.kprefabId.HasTag(GameTags.Stored))
			{
				return false;
			}
			Vector3 position = base.smi.transform.GetPosition();
			int num = Grid.PosToCell(position);
			if (Grid.IsValidCell(num) && Grid.Solid[num])
			{
				return false;
			}
			if (this.navigator.IsMoving())
			{
				return false;
			}
			if (this.CanSwimAtCurrentLocation())
			{
				return false;
			}
			if (this.navigator.CurrentNavType != NavType.Swim)
			{
				if (this.navigator.NavGrid.NavTable.IsValid(num, this.navigator.CurrentNavType))
				{
					return false;
				}
				if (this.navigator.CurrentNavType == NavType.Ceiling)
				{
					return true;
				}
				if (this.navigator.CurrentNavType == NavType.LeftWall)
				{
					return true;
				}
				if (this.navigator.CurrentNavType == NavType.RightWall)
				{
					return true;
				}
			}
			Vector3 vector = position;
			vector.y += CreatureFallMonitor.FLOOR_DISTANCE;
			int num2 = Grid.PosToCell(vector);
			return !Grid.IsValidCell(num2) || !Grid.Solid[num2];
		}

		// Token: 0x06002F2C RID: 12076 RVA: 0x00205044 File Offset: 0x00203244
		public bool CanSwimAtCurrentLocation()
		{
			if (base.def.canSwim)
			{
				Vector3 position = base.transform.GetPosition();
				float num = 1f;
				if (!base.def.checkHead)
				{
					num = 0.5f;
				}
				position.y += this.collider.size.y * num;
				if (Grid.IsSubstantialLiquid(Grid.PosToCell(position), 0.35f))
				{
					if (!GameComps.Gravities.Has(base.gameObject))
					{
						return true;
					}
					if (GameComps.Gravities.GetData(GameComps.Gravities.GetHandle(base.gameObject)).velocity.magnitude < 2f)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x04002056 RID: 8278
		public string anim = "fall";

		// Token: 0x04002057 RID: 8279
		[MyCmpReq]
		private KPrefabID kprefabId;

		// Token: 0x04002058 RID: 8280
		[MyCmpReq]
		private Navigator navigator;

		// Token: 0x04002059 RID: 8281
		[MyCmpReq]
		private KBoxCollider2D collider;
	}
}
