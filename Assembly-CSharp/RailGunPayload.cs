using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000F6A RID: 3946
[SerializationConfig(MemberSerialization.OptIn)]
public class RailGunPayload : GameStateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>
{
	// Token: 0x06004F40 RID: 20288 RVA: 0x00278BA0 File Offset: 0x00276DA0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.grounded.idle;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.grounded.DefaultState(this.grounded.idle).Enter(delegate(RailGunPayload.StatesInstance smi)
		{
			this.onSurface.Set(true, smi, false);
		}).ToggleMainStatusItem(Db.Get().BuildingStatusItems.RailgunpayloadNeedsEmptying, null).ToggleTag(GameTags.RailGunPayloadEmptyable).ToggleTag(GameTags.ClusterEntityGrounded).EventHandler(GameHashes.DroppedAll, delegate(RailGunPayload.StatesInstance smi)
		{
			smi.OnDroppedAll();
		}).OnSignal(this.launch, this.takeoff);
		this.grounded.idle.PlayAnim("idle");
		this.grounded.crater.Enter(delegate(RailGunPayload.StatesInstance smi)
		{
			smi.animController.randomiseLoopedOffset = true;
		}).Exit(delegate(RailGunPayload.StatesInstance smi)
		{
			smi.animController.randomiseLoopedOffset = false;
		}).PlayAnim("landed", KAnim.PlayMode.Loop).EventTransition(GameHashes.OnStore, this.grounded.idle, null);
		this.takeoff.DefaultState(this.takeoff.launch).Enter(delegate(RailGunPayload.StatesInstance smi)
		{
			this.onSurface.Set(false, smi, false);
		}).PlayAnim("launching").OnSignal(this.beginTravelling, this.travel);
		this.takeoff.launch.Enter(delegate(RailGunPayload.StatesInstance smi)
		{
			smi.StartTakeoff();
		}).GoTo(this.takeoff.airborne);
		this.takeoff.airborne.Update("Launch", delegate(RailGunPayload.StatesInstance smi, float dt)
		{
			smi.UpdateLaunch(dt);
		}, UpdateRate.SIM_EVERY_TICK, false);
		this.travel.DefaultState(this.travel.travelling).Enter(delegate(RailGunPayload.StatesInstance smi)
		{
			this.onSurface.Set(false, smi, false);
		}).Enter(delegate(RailGunPayload.StatesInstance smi)
		{
			smi.MoveToSpace();
		}).PlayAnim("idle").ToggleTag(GameTags.EntityInSpace).ToggleMainStatusItem(Db.Get().BuildingStatusItems.InFlight, (RailGunPayload.StatesInstance smi) => smi.GetComponent<ClusterTraveler>());
		this.travel.travelling.EventTransition(GameHashes.ClusterDestinationReached, this.travel.transferWorlds, null);
		this.travel.transferWorlds.Exit(delegate(RailGunPayload.StatesInstance smi)
		{
			smi.StartLand();
		}).GoTo(this.landing.landing);
		this.landing.DefaultState(this.landing.landing).ParamTransition<bool>(this.onSurface, this.grounded.crater, GameStateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.IsTrue).ParamTransition<int>(this.destinationWorld, this.takeoff, (RailGunPayload.StatesInstance smi, int p) => p != -1).Enter(delegate(RailGunPayload.StatesInstance smi)
		{
			smi.MoveToWorld();
		});
		this.landing.landing.PlayAnim("falling", KAnim.PlayMode.Loop).UpdateTransition(this.landing.impact, (RailGunPayload.StatesInstance smi, float dt) => smi.UpdateLanding(dt), UpdateRate.SIM_200ms, false).ToggleGravity(this.landing.impact);
		this.landing.impact.PlayAnim("land").TriggerOnEnter(GameHashes.JettisonCargo, null).OnAnimQueueComplete(this.grounded.crater);
	}

	// Token: 0x040037BD RID: 14269
	public StateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.IntParameter destinationWorld = new StateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.IntParameter(-1);

	// Token: 0x040037BE RID: 14270
	public StateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.BoolParameter onSurface = new StateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.BoolParameter(false);

	// Token: 0x040037BF RID: 14271
	public StateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.Signal beginTravelling;

	// Token: 0x040037C0 RID: 14272
	public StateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.Signal launch;

	// Token: 0x040037C1 RID: 14273
	public RailGunPayload.TakeoffStates takeoff;

	// Token: 0x040037C2 RID: 14274
	public RailGunPayload.TravelStates travel;

	// Token: 0x040037C3 RID: 14275
	public RailGunPayload.LandingStates landing;

	// Token: 0x040037C4 RID: 14276
	public RailGunPayload.GroundedStates grounded;

	// Token: 0x02000F6B RID: 3947
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040037C5 RID: 14277
		public bool attractToBeacons;

		// Token: 0x040037C6 RID: 14278
		public string clusterAnimSymbolSwapTarget;

		// Token: 0x040037C7 RID: 14279
		public List<string> randomClusterSymbolSwaps;

		// Token: 0x040037C8 RID: 14280
		public string worldAnimSymbolSwapTarget;

		// Token: 0x040037C9 RID: 14281
		public List<string> randomWorldSymbolSwaps;
	}

	// Token: 0x02000F6C RID: 3948
	public class TakeoffStates : GameStateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.State
	{
		// Token: 0x040037CA RID: 14282
		public GameStateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.State launch;

		// Token: 0x040037CB RID: 14283
		public GameStateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.State airborne;
	}

	// Token: 0x02000F6D RID: 3949
	public class TravelStates : GameStateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.State
	{
		// Token: 0x040037CC RID: 14284
		public GameStateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.State travelling;

		// Token: 0x040037CD RID: 14285
		public GameStateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.State transferWorlds;
	}

	// Token: 0x02000F6E RID: 3950
	public class LandingStates : GameStateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.State
	{
		// Token: 0x040037CE RID: 14286
		public GameStateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.State landing;

		// Token: 0x040037CF RID: 14287
		public GameStateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.State impact;
	}

	// Token: 0x02000F6F RID: 3951
	public class GroundedStates : GameStateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.State
	{
		// Token: 0x040037D0 RID: 14288
		public GameStateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.State crater;

		// Token: 0x040037D1 RID: 14289
		public GameStateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.State idle;
	}

	// Token: 0x02000F70 RID: 3952
	public class StatesInstance : GameStateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.GameInstance
	{
		// Token: 0x06004F4A RID: 20298 RVA: 0x00278F98 File Offset: 0x00277198
		public StatesInstance(IStateMachineTarget master, RailGunPayload.Def def) : base(master, def)
		{
			this.animController = base.GetComponent<KBatchedAnimController>();
			DebugUtil.Assert(def.clusterAnimSymbolSwapTarget == null == (def.worldAnimSymbolSwapTarget == null), "Must specify both or neither symbol swap targets!");
			DebugUtil.Assert((def.randomClusterSymbolSwaps == null && def.randomWorldSymbolSwaps == null) || def.randomClusterSymbolSwaps.Count == def.randomWorldSymbolSwaps.Count, "Must specify the same number of swaps for both world and cluster!");
			if (def.clusterAnimSymbolSwapTarget != null && def.worldAnimSymbolSwapTarget != null)
			{
				if (this.randomSymbolSwapIndex == -1)
				{
					this.randomSymbolSwapIndex = UnityEngine.Random.Range(0, def.randomClusterSymbolSwaps.Count);
					global::Debug.Log(string.Format("Rolling a random symbol: {0}", this.randomSymbolSwapIndex), base.gameObject);
				}
				base.GetComponent<BallisticClusterGridEntity>().SwapSymbolFromSameAnim(def.clusterAnimSymbolSwapTarget, def.randomClusterSymbolSwaps[this.randomSymbolSwapIndex]);
				KAnim.Build.Symbol symbol = this.animController.AnimFiles[0].GetData().build.GetSymbol(def.randomWorldSymbolSwaps[this.randomSymbolSwapIndex]);
				this.animController.GetComponent<SymbolOverrideController>().AddSymbolOverride(def.worldAnimSymbolSwapTarget, symbol, 0);
			}
		}

		// Token: 0x06004F4B RID: 20299 RVA: 0x002790DC File Offset: 0x002772DC
		public void Launch(AxialI source, AxialI destination)
		{
			base.GetComponent<BallisticClusterGridEntity>().Configure(source, destination);
			int asteroidWorldIdAtLocation = ClusterUtil.GetAsteroidWorldIdAtLocation(destination);
			base.sm.destinationWorld.Set(asteroidWorldIdAtLocation, this, false);
			this.GoTo(base.sm.takeoff);
		}

		// Token: 0x06004F4C RID: 20300 RVA: 0x00279124 File Offset: 0x00277324
		public void Travel(AxialI source, AxialI destination)
		{
			base.GetComponent<BallisticClusterGridEntity>().Configure(source, destination);
			int asteroidWorldIdAtLocation = ClusterUtil.GetAsteroidWorldIdAtLocation(destination);
			base.sm.destinationWorld.Set(asteroidWorldIdAtLocation, this, false);
			this.GoTo(base.sm.travel);
		}

		// Token: 0x06004F4D RID: 20301 RVA: 0x000D637E File Offset: 0x000D457E
		public void StartTakeoff()
		{
			if (GameComps.Fallers.Has(base.gameObject))
			{
				GameComps.Fallers.Remove(base.gameObject);
			}
		}

		// Token: 0x06004F4E RID: 20302 RVA: 0x0027916C File Offset: 0x0027736C
		public void StartLand()
		{
			WorldContainer worldContainer = ClusterManager.Instance.GetWorld(base.sm.destinationWorld.Get(this));
			if (worldContainer == null)
			{
				worldContainer = ClusterManager.Instance.GetStartWorld();
			}
			int num = Grid.InvalidCell;
			if (base.def.attractToBeacons)
			{
				num = ClusterManager.Instance.GetLandingBeaconLocation(worldContainer.id);
			}
			int num4;
			if (num != Grid.InvalidCell)
			{
				int num2;
				int num3;
				Grid.CellToXY(num, out num2, out num3);
				int minInclusive = Mathf.Max(num2 - 3, (int)worldContainer.minimumBounds.x);
				int maxExclusive = Mathf.Min(num2 + 3, (int)worldContainer.maximumBounds.x);
				num4 = Mathf.RoundToInt((float)UnityEngine.Random.Range(minInclusive, maxExclusive));
			}
			else
			{
				num4 = Mathf.RoundToInt(UnityEngine.Random.Range(worldContainer.minimumBounds.x + 3f, worldContainer.maximumBounds.x - 3f));
			}
			Vector3 position = new Vector3((float)num4 + 0.5f, worldContainer.maximumBounds.y - 1f, Grid.GetLayerZ(Grid.SceneLayer.Front));
			base.transform.SetPosition(position);
			if (GameComps.Fallers.Has(base.gameObject))
			{
				GameComps.Fallers.Remove(base.gameObject);
			}
			GameComps.Fallers.Add(base.gameObject, new Vector2(0f, -10f));
			base.sm.destinationWorld.Set(-1, this, false);
		}

		// Token: 0x06004F4F RID: 20303 RVA: 0x002792D4 File Offset: 0x002774D4
		public void UpdateLaunch(float dt)
		{
			if (base.gameObject.GetMyWorld() != null)
			{
				Vector3 position = base.transform.GetPosition() + new Vector3(0f, this.takeoffVelocity * dt, 0f);
				base.transform.SetPosition(position);
				return;
			}
			base.sm.beginTravelling.Trigger(this);
			ClusterGridEntity component = base.GetComponent<ClusterGridEntity>();
			if (ClusterGrid.Instance.GetAsteroidAtCell(component.Location) != null)
			{
				base.GetComponent<ClusterTraveler>().AdvancePathOneStep();
			}
		}

		// Token: 0x06004F50 RID: 20304 RVA: 0x00279368 File Offset: 0x00277568
		public bool UpdateLanding(float dt)
		{
			if (base.gameObject.GetMyWorld() != null)
			{
				Vector3 position = base.transform.GetPosition();
				position.y -= 0.5f;
				int cell = Grid.PosToCell(position);
				if (Grid.IsWorldValidCell(cell) && Grid.IsSolidCell(cell))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004F51 RID: 20305 RVA: 0x000D80C0 File Offset: 0x000D62C0
		public void OnDroppedAll()
		{
			base.gameObject.DeleteObject();
		}

		// Token: 0x06004F52 RID: 20306 RVA: 0x000D80CD File Offset: 0x000D62CD
		public bool IsTraveling()
		{
			return base.IsInsideState(base.sm.travel.travelling);
		}

		// Token: 0x06004F53 RID: 20307 RVA: 0x002793C0 File Offset: 0x002775C0
		public void MoveToSpace()
		{
			Pickupable component = base.GetComponent<Pickupable>();
			if (component != null)
			{
				component.deleteOffGrid = false;
			}
			base.gameObject.transform.SetPosition(new Vector3(-1f, -1f, 0f));
		}

		// Token: 0x06004F54 RID: 20308 RVA: 0x0027940C File Offset: 0x0027760C
		public void MoveToWorld()
		{
			Pickupable component = base.GetComponent<Pickupable>();
			if (component != null)
			{
				component.deleteOffGrid = true;
			}
			Storage component2 = base.GetComponent<Storage>();
			if (component2 != null)
			{
				component2.SetContentsDeleteOffGrid(true);
			}
		}

		// Token: 0x040037D2 RID: 14290
		[Serialize]
		public float takeoffVelocity;

		// Token: 0x040037D3 RID: 14291
		[Serialize]
		private int randomSymbolSwapIndex = -1;

		// Token: 0x040037D4 RID: 14292
		public KBatchedAnimController animController;
	}
}
