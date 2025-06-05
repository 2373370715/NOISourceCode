using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001944 RID: 6468
public class JettisonableCargoModule : GameStateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>
{
	// Token: 0x06008693 RID: 34451 RVA: 0x0035A434 File Offset: 0x00358634
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.grounded;
		this.root.Enter(delegate(JettisonableCargoModule.StatesInstance smi)
		{
			smi.CheckIfLoaded();
		}).EventHandler(GameHashes.OnStorageChange, delegate(JettisonableCargoModule.StatesInstance smi)
		{
			smi.CheckIfLoaded();
		});
		this.grounded.DefaultState(this.grounded.loaded).TagTransition(GameTags.RocketNotOnGround, this.not_grounded, false);
		this.grounded.loaded.PlayAnim("loaded").ParamTransition<bool>(this.hasCargo, this.grounded.empty, GameStateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.IsFalse);
		this.grounded.empty.PlayAnim("deployed").ParamTransition<bool>(this.hasCargo, this.grounded.loaded, GameStateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.IsTrue);
		this.not_grounded.DefaultState(this.not_grounded.loaded).TagTransition(GameTags.RocketNotOnGround, this.grounded, true);
		this.not_grounded.loaded.PlayAnim("loaded").ParamTransition<bool>(this.hasCargo, this.not_grounded.empty, GameStateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.IsFalse).OnSignal(this.emptyCargo, this.not_grounded.emptying);
		this.not_grounded.emptying.PlayAnim("deploying").Update(delegate(JettisonableCargoModule.StatesInstance smi, float dt)
		{
			if (smi.CheckReadyForFinalDeploy())
			{
				smi.FinalDeploy();
				smi.GoTo(smi.sm.not_grounded.empty);
			}
		}, UpdateRate.SIM_200ms, false).EventTransition(GameHashes.ClusterLocationChanged, (JettisonableCargoModule.StatesInstance smi) => Game.Instance, this.not_grounded, null).Exit(delegate(JettisonableCargoModule.StatesInstance smi)
		{
			smi.CancelPendingDeploy();
		});
		this.not_grounded.empty.PlayAnim("deployed").ParamTransition<bool>(this.hasCargo, this.not_grounded.loaded, GameStateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.IsTrue);
	}

	// Token: 0x04006605 RID: 26117
	public StateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.BoolParameter hasCargo;

	// Token: 0x04006606 RID: 26118
	public StateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.Signal emptyCargo;

	// Token: 0x04006607 RID: 26119
	public JettisonableCargoModule.GroundedStates grounded;

	// Token: 0x04006608 RID: 26120
	public JettisonableCargoModule.NotGroundedStates not_grounded;

	// Token: 0x02001945 RID: 6469
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04006609 RID: 26121
		public DefComponent<Storage> landerContainer;

		// Token: 0x0400660A RID: 26122
		public Tag landerPrefabID;

		// Token: 0x0400660B RID: 26123
		public Vector3 cargoDropOffset;

		// Token: 0x0400660C RID: 26124
		public string clusterMapFXPrefabID;
	}

	// Token: 0x02001946 RID: 6470
	public class GroundedStates : GameStateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.State
	{
		// Token: 0x0400660D RID: 26125
		public GameStateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.State loaded;

		// Token: 0x0400660E RID: 26126
		public GameStateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.State empty;
	}

	// Token: 0x02001947 RID: 6471
	public class NotGroundedStates : GameStateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.State
	{
		// Token: 0x0400660F RID: 26127
		public GameStateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.State loaded;

		// Token: 0x04006610 RID: 26128
		public GameStateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.State emptying;

		// Token: 0x04006611 RID: 26129
		public GameStateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.State empty;
	}

	// Token: 0x02001948 RID: 6472
	public class StatesInstance : GameStateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.GameInstance, IEmptyableCargo
	{
		// Token: 0x06008698 RID: 34456 RVA: 0x000FCD5D File Offset: 0x000FAF5D
		public StatesInstance(IStateMachineTarget master, JettisonableCargoModule.Def def) : base(master, def)
		{
			this.landerContainer = def.landerContainer.Get(this);
		}

		// Token: 0x06008699 RID: 34457 RVA: 0x0035A658 File Offset: 0x00358858
		private void ChooseLanderLocation()
		{
			ClusterGridEntity stableOrbitAsteroid = base.master.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>().GetStableOrbitAsteroid();
			if (stableOrbitAsteroid != null)
			{
				WorldContainer component = stableOrbitAsteroid.GetComponent<WorldContainer>();
				Placeable component2 = this.landerContainer.FindFirst(base.def.landerPrefabID).GetComponent<Placeable>();
				component2.restrictWorldId = component.id;
				component.LookAtSurface();
				ClusterManager.Instance.SetActiveWorld(component.id);
				ManagementMenu.Instance.CloseAll();
				PlaceTool.Instance.Activate(component2, new Action<Placeable, int>(this.OnLanderPlaced));
			}
		}

		// Token: 0x0600869A RID: 34458 RVA: 0x0035A6F0 File Offset: 0x003588F0
		private void OnLanderPlaced(Placeable lander, int cell)
		{
			this.landerPlaced = true;
			this.landerPlacementCell = cell;
			if (lander.GetComponent<MinionStorage>() != null)
			{
				this.OpenMoveChoreForChosenDuplicant();
			}
			ManagementMenu.Instance.ToggleClusterMap();
			base.sm.emptyCargo.Trigger(base.smi);
			ClusterMapScreen.Instance.SelectEntity(base.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<ClusterGridEntity>(), true);
		}

		// Token: 0x0600869B RID: 34459 RVA: 0x0035A75C File Offset: 0x0035895C
		private void OpenMoveChoreForChosenDuplicant()
		{
			RocketPassengerMonitor.Instance smi = this.ChosenDuplicant.GetSMI<RocketPassengerMonitor.Instance>();
			if (smi != null)
			{
				RocketModuleCluster component = base.master.GetComponent<RocketModuleCluster>();
				Clustercraft craft = component.CraftInterface.GetComponent<Clustercraft>();
				MinionStorage storage = this.landerContainer.FindFirst(base.def.landerPrefabID).GetComponent<MinionStorage>();
				this.EnableTeleport(true);
				smi.SetModuleDeployChore(this.landerPlacementCell, delegate(Chore obj)
				{
					Game.Instance.assignmentManager.RemoveFromWorld(this.ChosenDuplicant.assignableProxy.Get(), craft.ModuleInterface.GetInteriorWorld().id);
					storage.SerializeMinion(this.ChosenDuplicant.gameObject);
					this.EnableTeleport(false);
				});
			}
		}

		// Token: 0x0600869C RID: 34460 RVA: 0x0035A7E4 File Offset: 0x003589E4
		private void EnableTeleport(bool enable)
		{
			ClustercraftExteriorDoor component = base.master.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>().ModuleInterface.GetPassengerModule().GetComponent<ClustercraftExteriorDoor>();
			ClustercraftInteriorDoor interiorDoor = component.GetInteriorDoor();
			AccessControl component2 = component.GetInteriorDoor().GetComponent<AccessControl>();
			NavTeleporter component3 = base.GetComponent<NavTeleporter>();
			if (enable)
			{
				component3.SetOverrideCell(this.landerPlacementCell);
				interiorDoor.GetComponent<NavTeleporter>().SetTarget(component3);
				component3.SetTarget(interiorDoor.GetComponent<NavTeleporter>());
				using (List<MinionIdentity>.Enumerator enumerator = Components.MinionIdentities.GetWorldItems(interiorDoor.GetMyWorldId(), false).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						MinionIdentity minionIdentity = enumerator.Current;
						component2.SetPermission(minionIdentity.assignableProxy.Get(), (minionIdentity == this.ChosenDuplicant) ? AccessControl.Permission.Both : AccessControl.Permission.Neither);
					}
					return;
				}
			}
			component3.SetOverrideCell(-1);
			interiorDoor.GetComponent<NavTeleporter>().SetTarget(null);
			component3.SetTarget(null);
			component2.SetPermission(this.ChosenDuplicant.assignableProxy.Get(), AccessControl.Permission.Neither);
		}

		// Token: 0x0600869D RID: 34461 RVA: 0x0035A8FC File Offset: 0x00358AFC
		public void FinalDeploy()
		{
			this.landerPlaced = false;
			Placeable component = this.landerContainer.FindFirst(base.def.landerPrefabID).GetComponent<Placeable>();
			this.landerContainer.FindFirst(base.def.landerPrefabID);
			this.landerContainer.Drop(component.gameObject, true);
			TreeFilterable component2 = base.GetComponent<TreeFilterable>();
			TreeFilterable component3 = component.GetComponent<TreeFilterable>();
			if (component3 != null)
			{
				component3.UpdateFilters(component2.AcceptedTags);
			}
			Storage component4 = component.GetComponent<Storage>();
			if (component4 != null)
			{
				Storage[] components = base.gameObject.GetComponents<Storage>();
				for (int i = 0; i < components.Length; i++)
				{
					components[i].Transfer(component4, false, true);
				}
			}
			Vector3 position = Grid.CellToPosCBC(this.landerPlacementCell, Grid.SceneLayer.Building);
			component.transform.SetPosition(position);
			component.gameObject.SetActive(true);
			base.master.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>().gameObject.Trigger(1792516731, component);
			component.Trigger(1792516731, base.gameObject);
			GameObject gameObject = Assets.TryGetPrefab(base.smi.def.clusterMapFXPrefabID);
			if (gameObject != null)
			{
				this.clusterMapFX = GameUtil.KInstantiate(gameObject, Grid.SceneLayer.Background, null, 0);
				this.clusterMapFX.SetActive(true);
				this.clusterMapFX.GetComponent<ClusterFXEntity>().Init(component.GetMyWorldLocation(), Vector3.zero);
				component.Subscribe(1969584890, delegate(object data)
				{
					if (!this.clusterMapFX.IsNullOrDestroyed())
					{
						Util.KDestroyGameObject(this.clusterMapFX);
					}
				});
				component.Subscribe(1591811118, delegate(object data)
				{
					if (!this.clusterMapFX.IsNullOrDestroyed())
					{
						Util.KDestroyGameObject(this.clusterMapFX);
					}
				});
			}
		}

		// Token: 0x0600869E RID: 34462 RVA: 0x0035AAA4 File Offset: 0x00358CA4
		public bool CheckReadyForFinalDeploy()
		{
			MinionStorage component = this.landerContainer.FindFirst(base.def.landerPrefabID).GetComponent<MinionStorage>();
			return !(component != null) || component.GetStoredMinionInfo().Count > 0;
		}

		// Token: 0x0600869F RID: 34463 RVA: 0x000FCD79 File Offset: 0x000FAF79
		public void CancelPendingDeploy()
		{
			this.landerPlaced = false;
			if (this.ChosenDuplicant != null && this.CheckIfLoaded())
			{
				this.ChosenDuplicant.GetSMI<RocketPassengerMonitor.Instance>().CancelModuleDeployChore();
			}
		}

		// Token: 0x060086A0 RID: 34464 RVA: 0x0035AAE8 File Offset: 0x00358CE8
		public bool CheckIfLoaded()
		{
			bool flag = false;
			using (List<GameObject>.Enumerator enumerator = this.landerContainer.items.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.PrefabID() == base.def.landerPrefabID)
					{
						flag = true;
						break;
					}
				}
			}
			if (flag != base.sm.hasCargo.Get(this))
			{
				base.sm.hasCargo.Set(flag, this, false);
			}
			return flag;
		}

		// Token: 0x060086A1 RID: 34465 RVA: 0x000FCDA8 File Offset: 0x000FAFA8
		public bool IsValidDropLocation()
		{
			return base.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>().GetStableOrbitAsteroid() != null;
		}

		// Token: 0x170008C6 RID: 2246
		// (get) Token: 0x060086A2 RID: 34466 RVA: 0x000FCDC5 File Offset: 0x000FAFC5
		// (set) Token: 0x060086A3 RID: 34467 RVA: 0x000FCDCD File Offset: 0x000FAFCD
		public bool AutoDeploy { get; set; }

		// Token: 0x170008C7 RID: 2247
		// (get) Token: 0x060086A4 RID: 34468 RVA: 0x000B1628 File Offset: 0x000AF828
		public bool CanAutoDeploy
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060086A5 RID: 34469 RVA: 0x000FCDD6 File Offset: 0x000FAFD6
		public void EmptyCargo()
		{
			this.ChooseLanderLocation();
		}

		// Token: 0x060086A6 RID: 34470 RVA: 0x0035AB80 File Offset: 0x00358D80
		public bool CanEmptyCargo()
		{
			return base.sm.hasCargo.Get(base.smi) && this.IsValidDropLocation() && (!this.ChooseDuplicant || (this.ChosenDuplicant != null && !this.ChosenDuplicant.HasTag(GameTags.Dead))) && !this.landerPlaced;
		}

		// Token: 0x170008C8 RID: 2248
		// (get) Token: 0x060086A7 RID: 34471 RVA: 0x0035ABE0 File Offset: 0x00358DE0
		public bool ChooseDuplicant
		{
			get
			{
				GameObject gameObject = this.landerContainer.FindFirst(base.def.landerPrefabID);
				return !(gameObject == null) && gameObject.GetComponent<MinionStorage>() != null;
			}
		}

		// Token: 0x170008C9 RID: 2249
		// (get) Token: 0x060086A8 RID: 34472 RVA: 0x000FCDDE File Offset: 0x000FAFDE
		public bool ModuleDeployed
		{
			get
			{
				return this.landerPlaced;
			}
		}

		// Token: 0x170008CA RID: 2250
		// (get) Token: 0x060086A9 RID: 34473 RVA: 0x000FCDE6 File Offset: 0x000FAFE6
		// (set) Token: 0x060086AA RID: 34474 RVA: 0x000FCDEE File Offset: 0x000FAFEE
		public MinionIdentity ChosenDuplicant
		{
			get
			{
				return this.chosenDuplicant;
			}
			set
			{
				this.chosenDuplicant = value;
			}
		}

		// Token: 0x04006612 RID: 26130
		private Storage landerContainer;

		// Token: 0x04006613 RID: 26131
		private bool landerPlaced;

		// Token: 0x04006614 RID: 26132
		private MinionIdentity chosenDuplicant;

		// Token: 0x04006615 RID: 26133
		private int landerPlacementCell;

		// Token: 0x04006616 RID: 26134
		public GameObject clusterMapFX;
	}
}
