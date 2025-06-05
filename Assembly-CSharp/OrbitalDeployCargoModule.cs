using System;
using KSerialization;
using UnityEngine;

// Token: 0x02001965 RID: 6501
public class OrbitalDeployCargoModule : GameStateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>
{
	// Token: 0x06008752 RID: 34642 RVA: 0x0035E288 File Offset: 0x0035C488
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.grounded;
		this.root.Enter(delegate(OrbitalDeployCargoModule.StatesInstance smi)
		{
			smi.CheckIfLoaded();
		}).EventHandler(GameHashes.OnStorageChange, delegate(OrbitalDeployCargoModule.StatesInstance smi)
		{
			smi.CheckIfLoaded();
		}).EventHandler(GameHashes.ClusterDestinationReached, delegate(OrbitalDeployCargoModule.StatesInstance smi)
		{
			if (smi.AutoDeploy && smi.IsValidDropLocation())
			{
				smi.DeployCargoPods();
			}
		});
		this.grounded.DefaultState(this.grounded.loaded).TagTransition(GameTags.RocketNotOnGround, this.not_grounded, false);
		this.grounded.loading.PlayAnim((OrbitalDeployCargoModule.StatesInstance smi) => smi.GetLoadingAnimName(), KAnim.PlayMode.Once).ParamTransition<bool>(this.hasCargo, this.grounded.empty, GameStateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>.IsFalse).OnAnimQueueComplete(this.grounded.loaded);
		this.grounded.loaded.ParamTransition<bool>(this.hasCargo, this.grounded.empty, GameStateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>.IsFalse).EventTransition(GameHashes.OnStorageChange, this.grounded.loading, (OrbitalDeployCargoModule.StatesInstance smi) => smi.NeedsVisualUpdate());
		this.grounded.empty.Enter(delegate(OrbitalDeployCargoModule.StatesInstance smi)
		{
			this.numVisualCapsules.Set(0, smi, false);
		}).PlayAnim("deployed").ParamTransition<bool>(this.hasCargo, this.grounded.loaded, GameStateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>.IsTrue);
		this.not_grounded.DefaultState(this.not_grounded.loaded).TagTransition(GameTags.RocketNotOnGround, this.grounded, true);
		this.not_grounded.loaded.PlayAnim("loaded").ParamTransition<bool>(this.hasCargo, this.not_grounded.empty, GameStateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>.IsFalse).OnSignal(this.emptyCargo, this.not_grounded.emptying);
		this.not_grounded.emptying.PlayAnim("deploying").GoTo(this.not_grounded.empty);
		this.not_grounded.empty.PlayAnim("deployed").ParamTransition<bool>(this.hasCargo, this.not_grounded.loaded, GameStateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>.IsTrue);
	}

	// Token: 0x04006691 RID: 26257
	public StateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>.BoolParameter hasCargo;

	// Token: 0x04006692 RID: 26258
	public StateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>.Signal emptyCargo;

	// Token: 0x04006693 RID: 26259
	public OrbitalDeployCargoModule.GroundedStates grounded;

	// Token: 0x04006694 RID: 26260
	public OrbitalDeployCargoModule.NotGroundedStates not_grounded;

	// Token: 0x04006695 RID: 26261
	public StateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>.IntParameter numVisualCapsules;

	// Token: 0x02001966 RID: 6502
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04006696 RID: 26262
		public float numCapsules;
	}

	// Token: 0x02001967 RID: 6503
	public class GroundedStates : GameStateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>.State
	{
		// Token: 0x04006697 RID: 26263
		public GameStateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>.State loading;

		// Token: 0x04006698 RID: 26264
		public GameStateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>.State loaded;

		// Token: 0x04006699 RID: 26265
		public GameStateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>.State empty;
	}

	// Token: 0x02001968 RID: 6504
	public class NotGroundedStates : GameStateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>.State
	{
		// Token: 0x0400669A RID: 26266
		public GameStateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>.State loaded;

		// Token: 0x0400669B RID: 26267
		public GameStateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>.State emptying;

		// Token: 0x0400669C RID: 26268
		public GameStateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>.State empty;
	}

	// Token: 0x02001969 RID: 6505
	public class StatesInstance : GameStateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>.GameInstance, IEmptyableCargo
	{
		// Token: 0x06008758 RID: 34648 RVA: 0x0035E504 File Offset: 0x0035C704
		public StatesInstance(IStateMachineTarget master, OrbitalDeployCargoModule.Def def) : base(master, def)
		{
			this.storage = base.GetComponent<Storage>();
			base.GetComponent<RocketModule>().AddModuleCondition(ProcessCondition.ProcessConditionType.RocketStorage, new LoadingCompleteCondition(this.storage));
			base.gameObject.Subscribe(-1683615038, new Action<object>(this.SetupMeter));
		}

		// Token: 0x06008759 RID: 34649 RVA: 0x000FD363 File Offset: 0x000FB563
		private void SetupMeter(object obj)
		{
			KBatchedAnimTracker componentInChildren = base.gameObject.GetComponentInChildren<KBatchedAnimTracker>();
			componentInChildren.forceAlwaysAlive = true;
			componentInChildren.matchParentOffset = true;
		}

		// Token: 0x0600875A RID: 34650 RVA: 0x000FD37D File Offset: 0x000FB57D
		protected override void OnCleanUp()
		{
			base.gameObject.Unsubscribe(-1683615038, new Action<object>(this.SetupMeter));
			base.OnCleanUp();
		}

		// Token: 0x0600875B RID: 34651 RVA: 0x0035E55C File Offset: 0x0035C75C
		public bool NeedsVisualUpdate()
		{
			int num = base.sm.numVisualCapsules.Get(this);
			int num2 = Mathf.FloorToInt(this.storage.MassStored() / 200f);
			if (num < num2)
			{
				base.sm.numVisualCapsules.Delta(1, this);
				return true;
			}
			return false;
		}

		// Token: 0x0600875C RID: 34652 RVA: 0x0035E5AC File Offset: 0x0035C7AC
		public string GetLoadingAnimName()
		{
			int num = base.sm.numVisualCapsules.Get(this);
			int num2 = Mathf.RoundToInt(this.storage.capacityKg / 200f);
			if (num == num2)
			{
				return "loading6_full";
			}
			if (num == num2 - 1)
			{
				return "loading5";
			}
			if (num == num2 - 2)
			{
				return "loading4";
			}
			if (num == num2 - 3 || num > 2)
			{
				return "loading3_repeat";
			}
			if (num == 2)
			{
				return "loading2";
			}
			if (num == 1)
			{
				return "loading1";
			}
			return "deployed";
		}

		// Token: 0x0600875D RID: 34653 RVA: 0x0035E630 File Offset: 0x0035C830
		public void DeployCargoPods()
		{
			Clustercraft component = base.master.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>();
			ClusterGridEntity orbitAsteroid = component.GetOrbitAsteroid();
			if (orbitAsteroid != null)
			{
				WorldContainer component2 = orbitAsteroid.GetComponent<WorldContainer>();
				int id = component2.id;
				Vector3 position = new Vector3(component2.minimumBounds.x + 1f, component2.maximumBounds.y, Grid.GetLayerZ(Grid.SceneLayer.Front));
				while (this.storage.MassStored() > 0f)
				{
					GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("RailGunPayload"), position);
					gameObject.GetComponent<Pickupable>().deleteOffGrid = false;
					float num = 0f;
					while (num < 200f && this.storage.MassStored() > 0f)
					{
						num += this.storage.Transfer(gameObject.GetComponent<Storage>(), GameTags.Stored, 200f - num, false, true);
					}
					gameObject.SetActive(true);
					gameObject.GetSMI<RailGunPayload.StatesInstance>().Travel(component.Location, component2.GetMyWorldLocation());
				}
			}
			this.CheckIfLoaded();
		}

		// Token: 0x0600875E RID: 34654 RVA: 0x0035E750 File Offset: 0x0035C950
		public bool CheckIfLoaded()
		{
			bool flag = this.storage.MassStored() > 0f;
			if (flag != base.sm.hasCargo.Get(this))
			{
				base.sm.hasCargo.Set(flag, this, false);
			}
			return flag;
		}

		// Token: 0x0600875F RID: 34655 RVA: 0x000FD3A1 File Offset: 0x000FB5A1
		public bool IsValidDropLocation()
		{
			return base.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>().GetOrbitAsteroid() != null;
		}

		// Token: 0x170008DF RID: 2271
		// (get) Token: 0x06008760 RID: 34656 RVA: 0x000FD3BE File Offset: 0x000FB5BE
		// (set) Token: 0x06008761 RID: 34657 RVA: 0x000FD3C6 File Offset: 0x000FB5C6
		public bool AutoDeploy
		{
			get
			{
				return this.autoDeploy;
			}
			set
			{
				this.autoDeploy = value;
			}
		}

		// Token: 0x170008E0 RID: 2272
		// (get) Token: 0x06008762 RID: 34658 RVA: 0x000AA7E7 File Offset: 0x000A89E7
		public bool CanAutoDeploy
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06008763 RID: 34659 RVA: 0x000FD3CF File Offset: 0x000FB5CF
		public void EmptyCargo()
		{
			this.DeployCargoPods();
		}

		// Token: 0x06008764 RID: 34660 RVA: 0x000FD3D7 File Offset: 0x000FB5D7
		public bool CanEmptyCargo()
		{
			return base.sm.hasCargo.Get(base.smi) && this.IsValidDropLocation();
		}

		// Token: 0x170008E1 RID: 2273
		// (get) Token: 0x06008765 RID: 34661 RVA: 0x000B1628 File Offset: 0x000AF828
		public bool ChooseDuplicant
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008E2 RID: 2274
		// (get) Token: 0x06008766 RID: 34662 RVA: 0x000AA765 File Offset: 0x000A8965
		// (set) Token: 0x06008767 RID: 34663 RVA: 0x000AA038 File Offset: 0x000A8238
		public MinionIdentity ChosenDuplicant
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x170008E3 RID: 2275
		// (get) Token: 0x06008768 RID: 34664 RVA: 0x000B1628 File Offset: 0x000AF828
		public bool ModuleDeployed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0400669D RID: 26269
		private Storage storage;

		// Token: 0x0400669E RID: 26270
		[Serialize]
		private bool autoDeploy;
	}
}
