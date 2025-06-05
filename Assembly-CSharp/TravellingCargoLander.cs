using System;
using KSerialization;
using UnityEngine;

// Token: 0x02001052 RID: 4178
[SerializationConfig(MemberSerialization.OptIn)]
public class TravellingCargoLander : GameStateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>
{
	// Token: 0x060054D9 RID: 21721 RVA: 0x0028A65C File Offset: 0x0028885C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.init;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.root.InitializeOperationalFlag(RocketModule.landedFlag, false).Enter(delegate(TravellingCargoLander.StatesInstance smi)
		{
			smi.CheckIfLoaded();
		}).EventHandler(GameHashes.OnStorageChange, delegate(TravellingCargoLander.StatesInstance smi)
		{
			smi.CheckIfLoaded();
		});
		this.init.ParamTransition<bool>(this.isLanding, this.landing.landing, GameStateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.IsTrue).ParamTransition<bool>(this.isLanded, this.grounded, GameStateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.IsTrue).GoTo(this.travel);
		this.travel.DefaultState(this.travel.travelling).Enter(delegate(TravellingCargoLander.StatesInstance smi)
		{
			smi.MoveToSpace();
		}).PlayAnim("idle").ToggleTag(GameTags.EntityInSpace).ToggleMainStatusItem(Db.Get().BuildingStatusItems.InFlight, (TravellingCargoLander.StatesInstance smi) => smi.GetComponent<ClusterTraveler>());
		this.travel.travelling.EventTransition(GameHashes.ClusterDestinationReached, this.travel.transferWorlds, null);
		this.travel.transferWorlds.Enter(delegate(TravellingCargoLander.StatesInstance smi)
		{
			smi.StartLand();
		}).GoTo(this.landing.landing);
		this.landing.Enter(delegate(TravellingCargoLander.StatesInstance smi)
		{
			this.isLanding.Set(true, smi, false);
		}).Exit(delegate(TravellingCargoLander.StatesInstance smi)
		{
			this.isLanding.Set(false, smi, false);
		});
		this.landing.landing.PlayAnim("landing", KAnim.PlayMode.Loop).Enter(delegate(TravellingCargoLander.StatesInstance smi)
		{
			smi.ResetAnimPosition();
		}).Update(delegate(TravellingCargoLander.StatesInstance smi, float dt)
		{
			smi.LandingUpdate(dt);
		}, UpdateRate.SIM_EVERY_TICK, false).Transition(this.landing.impact, (TravellingCargoLander.StatesInstance smi) => smi.flightAnimOffset <= 0f, UpdateRate.SIM_200ms).Enter(delegate(TravellingCargoLander.StatesInstance smi)
		{
			smi.MoveToWorld();
		});
		this.landing.impact.PlayAnim("grounded_pre").OnAnimQueueComplete(this.grounded);
		this.grounded.DefaultState(this.grounded.loaded).ToggleTag(GameTags.ClusterEntityGrounded).ToggleOperationalFlag(RocketModule.landedFlag).Enter(delegate(TravellingCargoLander.StatesInstance smi)
		{
			smi.CheckIfLoaded();
		}).Enter(delegate(TravellingCargoLander.StatesInstance smi)
		{
			this.isLanded.Set(true, smi, false);
		});
		this.grounded.loaded.PlayAnim("grounded").ParamTransition<bool>(this.hasCargo, this.grounded.empty, GameStateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.IsFalse).OnSignal(this.emptyCargo, this.grounded.emptying).Enter(delegate(TravellingCargoLander.StatesInstance smi)
		{
			smi.DoLand();
		});
		this.grounded.emptying.PlayAnim("deploying").TriggerOnEnter(GameHashes.JettisonCargo, null).OnAnimQueueComplete(this.grounded.empty);
		this.grounded.empty.PlayAnim("deployed").ParamTransition<bool>(this.hasCargo, this.grounded.loaded, GameStateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.IsTrue);
	}

	// Token: 0x04003BC9 RID: 15305
	public StateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.IntParameter destinationWorld = new StateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.IntParameter(-1);

	// Token: 0x04003BCA RID: 15306
	public StateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.BoolParameter isLanding = new StateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.BoolParameter(false);

	// Token: 0x04003BCB RID: 15307
	public StateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.BoolParameter isLanded = new StateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.BoolParameter(false);

	// Token: 0x04003BCC RID: 15308
	public StateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.BoolParameter hasCargo = new StateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.BoolParameter(false);

	// Token: 0x04003BCD RID: 15309
	public StateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.Signal emptyCargo;

	// Token: 0x04003BCE RID: 15310
	public GameStateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.State init;

	// Token: 0x04003BCF RID: 15311
	public TravellingCargoLander.TravelStates travel;

	// Token: 0x04003BD0 RID: 15312
	public TravellingCargoLander.LandingStates landing;

	// Token: 0x04003BD1 RID: 15313
	public TravellingCargoLander.GroundedStates grounded;

	// Token: 0x02001053 RID: 4179
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04003BD2 RID: 15314
		public int landerWidth = 1;

		// Token: 0x04003BD3 RID: 15315
		public float landingSpeed = 5f;

		// Token: 0x04003BD4 RID: 15316
		public bool deployOnLanding;
	}

	// Token: 0x02001054 RID: 4180
	public class TravelStates : GameStateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.State
	{
		// Token: 0x04003BD5 RID: 15317
		public GameStateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.State travelling;

		// Token: 0x04003BD6 RID: 15318
		public GameStateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.State transferWorlds;
	}

	// Token: 0x02001055 RID: 4181
	public class LandingStates : GameStateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.State
	{
		// Token: 0x04003BD7 RID: 15319
		public GameStateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.State landing;

		// Token: 0x04003BD8 RID: 15320
		public GameStateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.State impact;
	}

	// Token: 0x02001056 RID: 4182
	public class GroundedStates : GameStateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.State
	{
		// Token: 0x04003BD9 RID: 15321
		public GameStateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.State loaded;

		// Token: 0x04003BDA RID: 15322
		public GameStateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.State emptying;

		// Token: 0x04003BDB RID: 15323
		public GameStateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.State empty;
	}

	// Token: 0x02001057 RID: 4183
	public class StatesInstance : GameStateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.GameInstance
	{
		// Token: 0x060054E2 RID: 21730 RVA: 0x000DBBC2 File Offset: 0x000D9DC2
		public StatesInstance(IStateMachineTarget master, TravellingCargoLander.Def def) : base(master, def)
		{
			this.animController = base.GetComponent<KBatchedAnimController>();
		}

		// Token: 0x060054E3 RID: 21731 RVA: 0x0028AA2C File Offset: 0x00288C2C
		public void Travel(AxialI source, AxialI destination)
		{
			base.GetComponent<BallisticClusterGridEntity>().Configure(source, destination);
			int asteroidWorldIdAtLocation = ClusterUtil.GetAsteroidWorldIdAtLocation(destination);
			base.sm.destinationWorld.Set(asteroidWorldIdAtLocation, this, false);
			this.GoTo(base.sm.travel);
		}

		// Token: 0x060054E4 RID: 21732 RVA: 0x0028AA74 File Offset: 0x00288C74
		public void StartLand()
		{
			WorldContainer world = ClusterManager.Instance.GetWorld(base.sm.destinationWorld.Get(this));
			Vector3 position = Grid.CellToPosCBC(ClusterManager.Instance.GetRandomSurfaceCell(world.id, base.def.landerWidth, true), this.animController.sceneLayer);
			base.transform.SetPosition(position);
		}

		// Token: 0x060054E5 RID: 21733 RVA: 0x00279368 File Offset: 0x00277568
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

		// Token: 0x060054E6 RID: 21734 RVA: 0x0028AAD8 File Offset: 0x00288CD8
		public void MoveToSpace()
		{
			Pickupable component = base.GetComponent<Pickupable>();
			if (component != null)
			{
				component.deleteOffGrid = false;
			}
			base.gameObject.transform.SetPosition(new Vector3(-1f, -1f, Grid.GetLayerZ(this.animController.sceneLayer)));
		}

		// Token: 0x060054E7 RID: 21735 RVA: 0x0028AB2C File Offset: 0x00288D2C
		public void MoveToWorld()
		{
			Pickupable component = base.GetComponent<Pickupable>();
			if (component != null)
			{
				component.deleteOffGrid = true;
			}
		}

		// Token: 0x060054E8 RID: 21736 RVA: 0x000DBBE3 File Offset: 0x000D9DE3
		public void ResetAnimPosition()
		{
			this.animController.Offset = Vector3.up * this.flightAnimOffset;
		}

		// Token: 0x060054E9 RID: 21737 RVA: 0x000DBC00 File Offset: 0x000D9E00
		public void LandingUpdate(float dt)
		{
			this.flightAnimOffset = Mathf.Max(this.flightAnimOffset - dt * base.def.landingSpeed, 0f);
			this.ResetAnimPosition();
		}

		// Token: 0x060054EA RID: 21738 RVA: 0x0028AB50 File Offset: 0x00288D50
		public void DoLand()
		{
			this.animController.Offset = Vector3.zero;
			OccupyArea component = base.smi.GetComponent<OccupyArea>();
			if (component != null)
			{
				component.ApplyToCells = true;
			}
			if (base.def.deployOnLanding && this.CheckIfLoaded())
			{
				base.sm.emptyCargo.Trigger(this);
			}
		}

		// Token: 0x060054EB RID: 21739 RVA: 0x0028ABB0 File Offset: 0x00288DB0
		public bool CheckIfLoaded()
		{
			bool flag = false;
			MinionStorage component = base.GetComponent<MinionStorage>();
			if (component != null)
			{
				flag |= (component.GetStoredMinionInfo().Count > 0);
			}
			Storage component2 = base.GetComponent<Storage>();
			if (component2 != null && !component2.IsEmpty())
			{
				flag = true;
			}
			if (flag != base.sm.hasCargo.Get(this))
			{
				base.sm.hasCargo.Set(flag, this, false);
			}
			return flag;
		}

		// Token: 0x04003BDC RID: 15324
		[Serialize]
		public float flightAnimOffset = 50f;

		// Token: 0x04003BDD RID: 15325
		public KBatchedAnimController animController;
	}
}
