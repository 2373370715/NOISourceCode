using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200195F RID: 6495
[SerializationConfig(MemberSerialization.OptIn)]
public class LaunchableRocketCluster : StateMachineComponent<LaunchableRocketCluster.StatesInstance>, ILaunchableRocket
{
	// Token: 0x170008D8 RID: 2264
	// (get) Token: 0x06008720 RID: 34592 RVA: 0x000FD15A File Offset: 0x000FB35A
	public IList<Ref<RocketModuleCluster>> parts
	{
		get
		{
			return base.GetComponent<RocketModuleCluster>().CraftInterface.ClusterModules;
		}
	}

	// Token: 0x170008D9 RID: 2265
	// (get) Token: 0x06008721 RID: 34593 RVA: 0x000FD16C File Offset: 0x000FB36C
	// (set) Token: 0x06008722 RID: 34594 RVA: 0x000FD174 File Offset: 0x000FB374
	public bool isLanding { get; private set; }

	// Token: 0x170008DA RID: 2266
	// (get) Token: 0x06008723 RID: 34595 RVA: 0x000FD17D File Offset: 0x000FB37D
	// (set) Token: 0x06008724 RID: 34596 RVA: 0x000FD185 File Offset: 0x000FB385
	public float rocketSpeed { get; private set; }

	// Token: 0x170008DB RID: 2267
	// (get) Token: 0x06008725 RID: 34597 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public LaunchableRocketRegisterType registerType
	{
		get
		{
			return LaunchableRocketRegisterType.Clustercraft;
		}
	}

	// Token: 0x170008DC RID: 2268
	// (get) Token: 0x06008726 RID: 34598 RVA: 0x000CEC86 File Offset: 0x000CCE86
	public GameObject LaunchableGameObject
	{
		get
		{
			return base.gameObject;
		}
	}

	// Token: 0x06008727 RID: 34599 RVA: 0x000FD18E File Offset: 0x000FB38E
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x06008728 RID: 34600 RVA: 0x0035D1E0 File Offset: 0x0035B3E0
	public List<GameObject> GetEngines()
	{
		List<GameObject> list = new List<GameObject>();
		foreach (Ref<RocketModuleCluster> @ref in this.parts)
		{
			if (@ref.Get().GetComponent<RocketEngineCluster>())
			{
				list.Add(@ref.Get().gameObject);
			}
		}
		return list;
	}

	// Token: 0x06008729 RID: 34601 RVA: 0x0035D250 File Offset: 0x0035B450
	private int GetRocketHeight()
	{
		int num = 0;
		foreach (Ref<RocketModuleCluster> @ref in this.parts)
		{
			num += @ref.Get().GetComponent<Building>().Def.HeightInCells;
		}
		return num;
	}

	// Token: 0x0600872A RID: 34602 RVA: 0x0035D2B4 File Offset: 0x0035B4B4
	private float InitialFlightAnimOffsetForLanding()
	{
		int num = Grid.PosToCell(base.gameObject);
		return ClusterManager.Instance.GetWorld((int)Grid.WorldIdx[num]).maximumBounds.y - base.gameObject.transform.GetPosition().y + (float)this.GetRocketHeight() + 100f;
	}

	// Token: 0x0400666F RID: 26223
	[Serialize]
	private int takeOffLocation;

	// Token: 0x04006672 RID: 26226
	private GameObject soundSpeakerObject;

	// Token: 0x02001960 RID: 6496
	public class StatesInstance : GameStateMachine<LaunchableRocketCluster.States, LaunchableRocketCluster.StatesInstance, LaunchableRocketCluster, object>.GameInstance
	{
		// Token: 0x170008DD RID: 2269
		// (get) Token: 0x0600872C RID: 34604 RVA: 0x000FD1A9 File Offset: 0x000FB3A9
		private float heightLaunchSpeedRatio
		{
			get
			{
				return Mathf.Pow((float)base.master.GetRocketHeight(), TuningData<LaunchableRocketCluster.StatesInstance.Tuning>.Get().heightSpeedPower) * TuningData<LaunchableRocketCluster.StatesInstance.Tuning>.Get().heightSpeedFactor;
			}
		}

		// Token: 0x170008DE RID: 2270
		// (get) Token: 0x0600872D RID: 34605 RVA: 0x000FD1D1 File Offset: 0x000FB3D1
		// (set) Token: 0x0600872E RID: 34606 RVA: 0x000FD1E4 File Offset: 0x000FB3E4
		public float DistanceAboveGround
		{
			get
			{
				return base.sm.distanceAboveGround.Get(this);
			}
			set
			{
				base.sm.distanceAboveGround.Set(value, this, false);
			}
		}

		// Token: 0x0600872F RID: 34607 RVA: 0x000FD1FA File Offset: 0x000FB3FA
		public StatesInstance(LaunchableRocketCluster master) : base(master)
		{
			this.takeoffAccelPowerInv = 1f / TuningData<LaunchableRocketCluster.StatesInstance.Tuning>.Get().takeoffAccelPower;
		}

		// Token: 0x06008730 RID: 34608 RVA: 0x000FD219 File Offset: 0x000FB419
		public void SetMissionState(Spacecraft.MissionState state)
		{
			global::Debug.Assert(!DlcManager.FeatureClusterSpaceEnabled());
			SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(base.master.GetComponent<LaunchConditionManager>()).SetState(state);
		}

		// Token: 0x06008731 RID: 34609 RVA: 0x000FD243 File Offset: 0x000FB443
		public Spacecraft.MissionState GetMissionState()
		{
			global::Debug.Assert(!DlcManager.FeatureClusterSpaceEnabled());
			return SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(base.master.GetComponent<LaunchConditionManager>()).state;
		}

		// Token: 0x06008732 RID: 34610 RVA: 0x000FD26C File Offset: 0x000FB46C
		public bool IsGrounded()
		{
			return base.smi.master.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>().Status == Clustercraft.CraftStatus.Grounded;
		}

		// Token: 0x06008733 RID: 34611 RVA: 0x0035D30C File Offset: 0x0035B50C
		public bool IsNotSpaceBound()
		{
			Clustercraft component = base.smi.master.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>();
			return component.Status == Clustercraft.CraftStatus.Grounded || component.Status == Clustercraft.CraftStatus.Landing;
		}

		// Token: 0x06008734 RID: 34612 RVA: 0x0035D348 File Offset: 0x0035B548
		public bool IsNotGroundBound()
		{
			Clustercraft component = base.smi.master.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>();
			return component.Status == Clustercraft.CraftStatus.Launching || component.Status == Clustercraft.CraftStatus.InFlight;
		}

		// Token: 0x06008735 RID: 34613 RVA: 0x0035D384 File Offset: 0x0035B584
		public void SetupLaunch()
		{
			base.master.isLanding = false;
			base.master.rocketSpeed = 0f;
			base.sm.warmupTimeRemaining.Set(5f, this, false);
			base.sm.distanceAboveGround.Set(0f, this, false);
			if (base.master.soundSpeakerObject == null)
			{
				base.master.soundSpeakerObject = new GameObject("rocketSpeaker");
				base.master.soundSpeakerObject.transform.SetParent(base.master.gameObject.transform);
			}
			foreach (Ref<RocketModuleCluster> @ref in base.master.parts)
			{
				if (@ref != null)
				{
					base.master.takeOffLocation = Grid.PosToCell(base.master.gameObject);
					@ref.Get().Trigger(-1277991738, base.master.gameObject);
				}
			}
			CraftModuleInterface craftInterface = base.master.GetComponent<RocketModuleCluster>().CraftInterface;
			if (craftInterface != null)
			{
				craftInterface.Trigger(-1277991738, base.master.gameObject);
				WorldContainer component = craftInterface.GetComponent<WorldContainer>();
				if (component != null)
				{
					List<MinionIdentity> worldItems = Components.MinionIdentities.GetWorldItems(component.id, false);
					MinionMigrationEventArgs minionMigrationEventArgs = new MinionMigrationEventArgs
					{
						prevWorldId = component.id,
						targetWorldId = component.id
					};
					foreach (MinionIdentity minionId in worldItems)
					{
						minionMigrationEventArgs.minionId = minionId;
						Game.Instance.Trigger(586301400, minionMigrationEventArgs);
					}
				}
			}
			Game.Instance.Trigger(-1277991738, base.gameObject);
			this.constantVelocityPhase_maxSpeed = 0f;
		}

		// Token: 0x06008736 RID: 34614 RVA: 0x0035D588 File Offset: 0x0035B788
		public void LaunchLoop(float dt)
		{
			base.master.isLanding = false;
			if (this.constantVelocityPhase_maxSpeed == 0f)
			{
				float num = Mathf.Pow((Mathf.Pow(TuningData<LaunchableRocketCluster.StatesInstance.Tuning>.Get().maxAccelerationDistance, this.takeoffAccelPowerInv) * this.heightLaunchSpeedRatio - 0.033f) / this.heightLaunchSpeedRatio, TuningData<LaunchableRocketCluster.StatesInstance.Tuning>.Get().takeoffAccelPower);
				this.constantVelocityPhase_maxSpeed = (TuningData<LaunchableRocketCluster.StatesInstance.Tuning>.Get().maxAccelerationDistance - num) / 0.033f;
			}
			if (base.sm.warmupTimeRemaining.Get(this) > 0f)
			{
				base.sm.warmupTimeRemaining.Delta(-dt, this);
			}
			else if (this.DistanceAboveGround < TuningData<LaunchableRocketCluster.StatesInstance.Tuning>.Get().maxAccelerationDistance)
			{
				float num2 = Mathf.Pow(this.DistanceAboveGround, this.takeoffAccelPowerInv) * this.heightLaunchSpeedRatio;
				num2 += dt;
				this.DistanceAboveGround = Mathf.Pow(num2 / this.heightLaunchSpeedRatio, TuningData<LaunchableRocketCluster.StatesInstance.Tuning>.Get().takeoffAccelPower);
				float num3 = Mathf.Pow((num2 - 0.033f) / this.heightLaunchSpeedRatio, TuningData<LaunchableRocketCluster.StatesInstance.Tuning>.Get().takeoffAccelPower);
				base.master.rocketSpeed = (this.DistanceAboveGround - num3) / 0.033f;
			}
			else
			{
				base.master.rocketSpeed = this.constantVelocityPhase_maxSpeed;
				this.DistanceAboveGround += base.master.rocketSpeed * dt;
			}
			this.UpdateSoundSpeakerObject();
			if (this.UpdatePartsAnimPositionsAndDamage(true) == 0)
			{
				base.smi.GoTo(base.sm.not_grounded.space);
			}
		}

		// Token: 0x06008737 RID: 34615 RVA: 0x0035D70C File Offset: 0x0035B90C
		public void FinalizeLaunch()
		{
			base.master.rocketSpeed = 0f;
			this.DistanceAboveGround = base.sm.distanceToSpace.Get(base.smi);
			foreach (Ref<RocketModuleCluster> @ref in base.master.parts)
			{
				if (@ref != null && !(@ref.Get() == null))
				{
					RocketModuleCluster rocketModuleCluster = @ref.Get();
					rocketModuleCluster.GetComponent<KBatchedAnimController>().Offset = Vector3.up * this.DistanceAboveGround;
					rocketModuleCluster.GetComponent<KBatchedAnimController>().enabled = false;
					rocketModuleCluster.GetComponent<RocketModule>().MoveToSpace();
				}
			}
			base.smi.master.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>().SetCraftStatus(Clustercraft.CraftStatus.InFlight);
		}

		// Token: 0x06008738 RID: 34616 RVA: 0x0035D7EC File Offset: 0x0035B9EC
		public void SetupLanding()
		{
			float distanceAboveGround = base.master.InitialFlightAnimOffsetForLanding();
			this.DistanceAboveGround = distanceAboveGround;
			base.sm.warmupTimeRemaining.Set(2f, this, false);
			base.master.isLanding = true;
			base.master.rocketSpeed = 0f;
			this.constantVelocityPhase_maxSpeed = 0f;
		}

		// Token: 0x06008739 RID: 34617 RVA: 0x0035D84C File Offset: 0x0035BA4C
		public void LandingLoop(float dt)
		{
			base.master.isLanding = true;
			if (this.constantVelocityPhase_maxSpeed == 0f)
			{
				float num = Mathf.Pow((Mathf.Pow(TuningData<LaunchableRocketCluster.StatesInstance.Tuning>.Get().maxAccelerationDistance, this.takeoffAccelPowerInv) * this.heightLaunchSpeedRatio - 0.033f) / this.heightLaunchSpeedRatio, TuningData<LaunchableRocketCluster.StatesInstance.Tuning>.Get().takeoffAccelPower);
				this.constantVelocityPhase_maxSpeed = (num - TuningData<LaunchableRocketCluster.StatesInstance.Tuning>.Get().maxAccelerationDistance) / 0.033f;
			}
			if (this.DistanceAboveGround > TuningData<LaunchableRocketCluster.StatesInstance.Tuning>.Get().maxAccelerationDistance)
			{
				base.master.rocketSpeed = this.constantVelocityPhase_maxSpeed;
				this.DistanceAboveGround += base.master.rocketSpeed * dt;
			}
			else if (this.DistanceAboveGround > 0.0025f)
			{
				float num2 = Mathf.Pow(this.DistanceAboveGround, this.takeoffAccelPowerInv) * this.heightLaunchSpeedRatio;
				num2 -= dt;
				this.DistanceAboveGround = Mathf.Pow(num2 / this.heightLaunchSpeedRatio, TuningData<LaunchableRocketCluster.StatesInstance.Tuning>.Get().takeoffAccelPower);
				float num3 = Mathf.Pow((num2 - 0.033f) / this.heightLaunchSpeedRatio, TuningData<LaunchableRocketCluster.StatesInstance.Tuning>.Get().takeoffAccelPower);
				base.master.rocketSpeed = (this.DistanceAboveGround - num3) / 0.033f;
			}
			else if (base.sm.warmupTimeRemaining.Get(this) > 0f)
			{
				base.sm.warmupTimeRemaining.Delta(-dt, this);
				this.DistanceAboveGround = 0f;
			}
			this.UpdateSoundSpeakerObject();
			this.UpdatePartsAnimPositionsAndDamage(true);
		}

		// Token: 0x0600873A RID: 34618 RVA: 0x0035D9CC File Offset: 0x0035BBCC
		public void FinalizeLanding()
		{
			base.GetComponent<KSelectable>().IsSelectable = true;
			base.master.rocketSpeed = 0f;
			this.DistanceAboveGround = 0f;
			foreach (Ref<RocketModuleCluster> @ref in base.smi.master.parts)
			{
				if (@ref != null && !(@ref.Get() == null))
				{
					@ref.Get().GetComponent<KBatchedAnimController>().Offset = Vector3.zero;
				}
			}
			base.smi.master.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>().SetCraftStatus(Clustercraft.CraftStatus.Grounded);
		}

		// Token: 0x0600873B RID: 34619 RVA: 0x0035DA8C File Offset: 0x0035BC8C
		private void UpdateSoundSpeakerObject()
		{
			if (base.master.soundSpeakerObject == null)
			{
				base.master.soundSpeakerObject = new GameObject("rocketSpeaker");
				base.master.soundSpeakerObject.transform.SetParent(base.gameObject.transform);
			}
			base.master.soundSpeakerObject.transform.SetLocalPosition(this.DistanceAboveGround * Vector3.up);
		}

		// Token: 0x0600873C RID: 34620 RVA: 0x0035DB08 File Offset: 0x0035BD08
		public int UpdatePartsAnimPositionsAndDamage(bool doDamage = true)
		{
			int myWorldId = base.gameObject.GetMyWorldId();
			if (myWorldId == -1)
			{
				return 0;
			}
			LaunchPad currentPad = base.smi.master.GetComponent<RocketModuleCluster>().CraftInterface.CurrentPad;
			if (currentPad != null)
			{
				myWorldId = currentPad.GetMyWorldId();
			}
			int num = 0;
			foreach (Ref<RocketModuleCluster> @ref in base.master.parts)
			{
				if (@ref != null)
				{
					RocketModuleCluster rocketModuleCluster = @ref.Get();
					KBatchedAnimController component = rocketModuleCluster.GetComponent<KBatchedAnimController>();
					component.Offset = Vector3.up * this.DistanceAboveGround;
					Vector3 positionIncludingOffset = component.PositionIncludingOffset;
					int num2 = Grid.PosToCell(component.transform.GetPosition());
					bool flag = Grid.IsValidCell(num2);
					bool flag2 = flag && (int)Grid.WorldIdx[num2] == myWorldId;
					if (component.enabled != flag2)
					{
						component.enabled = flag2;
					}
					if (doDamage && flag)
					{
						num++;
						LaunchableRocketCluster.States.DoWorldDamage(rocketModuleCluster.gameObject, positionIncludingOffset, myWorldId);
					}
				}
			}
			return num;
		}

		// Token: 0x04006673 RID: 26227
		private float takeoffAccelPowerInv;

		// Token: 0x04006674 RID: 26228
		private float constantVelocityPhase_maxSpeed;

		// Token: 0x02001961 RID: 6497
		public class Tuning : TuningData<LaunchableRocketCluster.StatesInstance.Tuning>
		{
			// Token: 0x04006675 RID: 26229
			public float takeoffAccelPower = 4f;

			// Token: 0x04006676 RID: 26230
			public float maxAccelerationDistance = 25f;

			// Token: 0x04006677 RID: 26231
			public float warmupTime = 5f;

			// Token: 0x04006678 RID: 26232
			public float heightSpeedPower = 0.5f;

			// Token: 0x04006679 RID: 26233
			public float heightSpeedFactor = 4f;

			// Token: 0x0400667A RID: 26234
			public int maxAccelHeight = 40;
		}
	}

	// Token: 0x02001962 RID: 6498
	public class States : GameStateMachine<LaunchableRocketCluster.States, LaunchableRocketCluster.StatesInstance, LaunchableRocketCluster>
	{
		// Token: 0x0600873E RID: 34622 RVA: 0x0035DC84 File Offset: 0x0035BE84
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.grounded;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.grounded.EventTransition(GameHashes.DoLaunchRocket, this.not_grounded.launch_setup, null).EnterTransition(this.not_grounded.launch_loop, (LaunchableRocketCluster.StatesInstance smi) => smi.IsNotGroundBound()).Enter(delegate(LaunchableRocketCluster.StatesInstance smi)
			{
				smi.FinalizeLanding();
			});
			this.not_grounded.launch_setup.Enter(delegate(LaunchableRocketCluster.StatesInstance smi)
			{
				smi.SetupLaunch();
				this.distanceToSpace.Set((float)ConditionFlightPathIsClear.PadTopEdgeDistanceToOutOfScreenEdge(smi.master.gameObject.GetComponent<RocketModuleCluster>().CraftInterface.CurrentPad.gameObject), smi, false);
				smi.GoTo(this.not_grounded.launch_loop);
			});
			this.not_grounded.launch_loop.EventTransition(GameHashes.DoReturnRocket, this.not_grounded.landing_setup, null).Enter(delegate(LaunchableRocketCluster.StatesInstance smi)
			{
				smi.UpdatePartsAnimPositionsAndDamage(false);
			}).Update(delegate(LaunchableRocketCluster.StatesInstance smi, float dt)
			{
				smi.LaunchLoop(dt);
			}, UpdateRate.SIM_EVERY_TICK, false).ParamTransition<float>(this.distanceAboveGround, this.not_grounded.launch_pst, (LaunchableRocketCluster.StatesInstance smi, float p) => p >= this.distanceToSpace.Get(smi)).TriggerOnEnter(GameHashes.StartRocketLaunch, null).Exit(delegate(LaunchableRocketCluster.StatesInstance smi)
			{
				WorldContainer myWorld = smi.gameObject.GetMyWorld();
				if (myWorld != null)
				{
					myWorld.RevealSurface();
				}
			});
			this.not_grounded.launch_pst.ScheduleGoTo(0f, this.not_grounded.space);
			this.not_grounded.space.EnterTransition(this.not_grounded.landing_setup, (LaunchableRocketCluster.StatesInstance smi) => smi.IsNotSpaceBound()).EventTransition(GameHashes.DoReturnRocket, this.not_grounded.landing_setup, null).Enter(delegate(LaunchableRocketCluster.StatesInstance smi)
			{
				smi.FinalizeLaunch();
			});
			this.not_grounded.landing_setup.Enter(delegate(LaunchableRocketCluster.StatesInstance smi)
			{
				smi.SetupLanding();
				smi.GoTo(this.not_grounded.landing_loop);
			});
			this.not_grounded.landing_loop.Enter(delegate(LaunchableRocketCluster.StatesInstance smi)
			{
				smi.UpdatePartsAnimPositionsAndDamage(false);
			}).Update(delegate(LaunchableRocketCluster.StatesInstance smi, float dt)
			{
				smi.LandingLoop(dt);
			}, UpdateRate.SIM_EVERY_TICK, false).ParamTransition<float>(this.distanceAboveGround, this.not_grounded.land, new StateMachine<LaunchableRocketCluster.States, LaunchableRocketCluster.StatesInstance, LaunchableRocketCluster, object>.Parameter<float>.Callback(this.IsFullyLanded<float>)).ParamTransition<float>(this.warmupTimeRemaining, this.not_grounded.land, new StateMachine<LaunchableRocketCluster.States, LaunchableRocketCluster.StatesInstance, LaunchableRocketCluster, object>.Parameter<float>.Callback(this.IsFullyLanded<float>));
			this.not_grounded.land.TriggerOnEnter(GameHashes.RocketTouchDown, null).Enter(delegate(LaunchableRocketCluster.StatesInstance smi)
			{
				foreach (Ref<RocketModuleCluster> @ref in smi.master.parts)
				{
					if (@ref != null && !(@ref.Get() == null))
					{
						@ref.Get().Trigger(-887025858, smi.gameObject);
					}
				}
				CraftModuleInterface craftInterface = smi.master.GetComponent<RocketModuleCluster>().CraftInterface;
				if (craftInterface != null)
				{
					craftInterface.Trigger(-887025858, smi.gameObject);
					WorldContainer component = craftInterface.GetComponent<WorldContainer>();
					if (component != null)
					{
						List<MinionIdentity> worldItems = Components.MinionIdentities.GetWorldItems(component.id, false);
						MinionMigrationEventArgs minionMigrationEventArgs = new MinionMigrationEventArgs
						{
							prevWorldId = component.id,
							targetWorldId = component.id
						};
						foreach (MinionIdentity minionId in worldItems)
						{
							minionMigrationEventArgs.minionId = minionId;
							Game.Instance.Trigger(586301400, minionMigrationEventArgs);
						}
					}
				}
				Game.Instance.Trigger(-887025858, smi.gameObject);
				if (craftInterface != null)
				{
					PassengerRocketModule passengerModule = craftInterface.GetPassengerModule();
					if (passengerModule != null)
					{
						passengerModule.RemovePassengersOnOtherWorlds();
					}
				}
				smi.GoTo(this.grounded);
			});
		}

		// Token: 0x0600873F RID: 34623 RVA: 0x000FD290 File Offset: 0x000FB490
		public bool IsFullyLanded<T>(LaunchableRocketCluster.StatesInstance smi, T p)
		{
			return this.distanceAboveGround.Get(smi) <= 0.0025f && this.warmupTimeRemaining.Get(smi) <= 0f;
		}

		// Token: 0x06008740 RID: 34624 RVA: 0x0035DF60 File Offset: 0x0035C160
		public static void DoWorldDamage(GameObject part, Vector3 apparentPosition, int actualWorld)
		{
			OccupyArea component = part.GetComponent<OccupyArea>();
			component.UpdateOccupiedArea();
			foreach (CellOffset offset in component.OccupiedCellsOffsets)
			{
				int num = Grid.OffsetCell(Grid.PosToCell(apparentPosition), offset);
				if (Grid.IsValidCell(num) && Grid.WorldIdx[num] == Grid.WorldIdx[actualWorld])
				{
					if (Grid.Solid[num])
					{
						WorldDamage.Instance.ApplyDamage(num, 10000f, num, BUILDINGS.DAMAGESOURCES.ROCKET, UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.ROCKET);
					}
					else if (Grid.FakeFloor[num])
					{
						GameObject gameObject = Grid.Objects[num, 39];
						if (gameObject != null && gameObject.HasTag(GameTags.GantryExtended))
						{
							BuildingHP component2 = gameObject.GetComponent<BuildingHP>();
							if (component2 != null)
							{
								gameObject.Trigger(-794517298, new BuildingHP.DamageSourceInfo
								{
									damage = component2.MaxHitPoints,
									source = BUILDINGS.DAMAGESOURCES.ROCKET,
									popString = UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.ROCKET
								});
							}
						}
					}
				}
			}
		}

		// Token: 0x0400667B RID: 26235
		public StateMachine<LaunchableRocketCluster.States, LaunchableRocketCluster.StatesInstance, LaunchableRocketCluster, object>.FloatParameter warmupTimeRemaining;

		// Token: 0x0400667C RID: 26236
		public StateMachine<LaunchableRocketCluster.States, LaunchableRocketCluster.StatesInstance, LaunchableRocketCluster, object>.FloatParameter distanceAboveGround;

		// Token: 0x0400667D RID: 26237
		public StateMachine<LaunchableRocketCluster.States, LaunchableRocketCluster.StatesInstance, LaunchableRocketCluster, object>.FloatParameter distanceToSpace;

		// Token: 0x0400667E RID: 26238
		public GameStateMachine<LaunchableRocketCluster.States, LaunchableRocketCluster.StatesInstance, LaunchableRocketCluster, object>.State grounded;

		// Token: 0x0400667F RID: 26239
		public LaunchableRocketCluster.States.NotGroundedStates not_grounded;

		// Token: 0x02001963 RID: 6499
		public class NotGroundedStates : GameStateMachine<LaunchableRocketCluster.States, LaunchableRocketCluster.StatesInstance, LaunchableRocketCluster, object>.State
		{
			// Token: 0x04006680 RID: 26240
			public GameStateMachine<LaunchableRocketCluster.States, LaunchableRocketCluster.StatesInstance, LaunchableRocketCluster, object>.State launch_setup;

			// Token: 0x04006681 RID: 26241
			public GameStateMachine<LaunchableRocketCluster.States, LaunchableRocketCluster.StatesInstance, LaunchableRocketCluster, object>.State launch_loop;

			// Token: 0x04006682 RID: 26242
			public GameStateMachine<LaunchableRocketCluster.States, LaunchableRocketCluster.StatesInstance, LaunchableRocketCluster, object>.State launch_pst;

			// Token: 0x04006683 RID: 26243
			public GameStateMachine<LaunchableRocketCluster.States, LaunchableRocketCluster.StatesInstance, LaunchableRocketCluster, object>.State space;

			// Token: 0x04006684 RID: 26244
			public GameStateMachine<LaunchableRocketCluster.States, LaunchableRocketCluster.StatesInstance, LaunchableRocketCluster, object>.State landing_setup;

			// Token: 0x04006685 RID: 26245
			public GameStateMachine<LaunchableRocketCluster.States, LaunchableRocketCluster.StatesInstance, LaunchableRocketCluster, object>.State landing_loop;

			// Token: 0x04006686 RID: 26246
			public GameStateMachine<LaunchableRocketCluster.States, LaunchableRocketCluster.StatesInstance, LaunchableRocketCluster, object>.State land;
		}
	}
}
