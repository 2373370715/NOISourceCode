using System;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02001984 RID: 6532
[SerializationConfig(MemberSerialization.OptIn)]
public class RocketEngineCluster : StateMachineComponent<RocketEngineCluster.StatesInstance>
{
	// Token: 0x06008802 RID: 34818 RVA: 0x003610D4 File Offset: 0x0035F2D4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		if (this.mainEngine)
		{
			base.GetComponent<RocketModule>().AddModuleCondition(ProcessCondition.ProcessConditionType.RocketPrep, new RequireAttachedComponent(base.gameObject.GetComponent<AttachableBuilding>(), typeof(IFuelTank), UI.STARMAP.COMPONENT.FUEL_TANK));
			if (this.requireOxidizer)
			{
				base.GetComponent<RocketModule>().AddModuleCondition(ProcessCondition.ProcessConditionType.RocketPrep, new RequireAttachedComponent(base.gameObject.GetComponent<AttachableBuilding>(), typeof(OxidizerTank), UI.STARMAP.COMPONENT.OXIDIZER_TANK));
			}
			base.GetComponent<RocketModule>().AddModuleCondition(ProcessCondition.ProcessConditionType.RocketPrep, new ConditionRocketHeight(this));
		}
	}

	// Token: 0x06008803 RID: 34819 RVA: 0x00361178 File Offset: 0x0035F378
	private void ConfigureFlameLight()
	{
		this.flameLight = base.gameObject.AddOrGet<Light2D>();
		this.flameLight.Color = Color.white;
		this.flameLight.overlayColour = LIGHT2D.LIGHTBUG_OVERLAYCOLOR;
		this.flameLight.Range = 10f;
		this.flameLight.Angle = 0f;
		this.flameLight.Direction = LIGHT2D.LIGHTBUG_DIRECTION;
		this.flameLight.Offset = LIGHT2D.LIGHTBUG_OFFSET;
		this.flameLight.shape = global::LightShape.Circle;
		this.flameLight.drawOverlay = true;
		this.flameLight.Lux = 80000;
		this.flameLight.emitter.RemoveFromGrid();
		base.gameObject.AddOrGet<LightSymbolTracker>().targetSymbol = base.GetComponent<KBatchedAnimController>().CurrentAnim.rootSymbol;
		this.flameLight.enabled = false;
	}

	// Token: 0x06008804 RID: 34820 RVA: 0x0036125C File Offset: 0x0035F45C
	private void UpdateFlameLight(int cell)
	{
		base.smi.master.flameLight.RefreshShapeAndPosition();
		if (Grid.IsValidCell(cell))
		{
			if (!base.smi.master.flameLight.enabled && base.smi.timeinstate > 3f)
			{
				base.smi.master.flameLight.enabled = true;
				return;
			}
		}
		else
		{
			base.smi.master.flameLight.enabled = false;
		}
	}

	// Token: 0x06008805 RID: 34821 RVA: 0x000FD8D5 File Offset: 0x000FBAD5
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x0400670A RID: 26378
	public float exhaustEmitRate = 50f;

	// Token: 0x0400670B RID: 26379
	public float exhaustTemperature = 1500f;

	// Token: 0x0400670C RID: 26380
	public SpawnFXHashes explosionEffectHash;

	// Token: 0x0400670D RID: 26381
	public SimHashes exhaustElement = SimHashes.CarbonDioxide;

	// Token: 0x0400670E RID: 26382
	public Tag fuelTag;

	// Token: 0x0400670F RID: 26383
	public float efficiency = 1f;

	// Token: 0x04006710 RID: 26384
	public bool requireOxidizer = true;

	// Token: 0x04006711 RID: 26385
	public int maxModules = 32;

	// Token: 0x04006712 RID: 26386
	public int maxHeight;

	// Token: 0x04006713 RID: 26387
	public bool mainEngine = true;

	// Token: 0x04006714 RID: 26388
	public byte exhaustDiseaseIdx = byte.MaxValue;

	// Token: 0x04006715 RID: 26389
	public int exhaustDiseaseCount;

	// Token: 0x04006716 RID: 26390
	public bool emitRadiation;

	// Token: 0x04006717 RID: 26391
	[MyCmpGet]
	private RadiationEmitter radiationEmitter;

	// Token: 0x04006718 RID: 26392
	[MyCmpGet]
	private Generator powerGenerator;

	// Token: 0x04006719 RID: 26393
	[MyCmpReq]
	private KBatchedAnimController animController;

	// Token: 0x0400671A RID: 26394
	public Light2D flameLight;

	// Token: 0x02001985 RID: 6533
	public class StatesInstance : GameStateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.GameInstance
	{
		// Token: 0x06008807 RID: 34823 RVA: 0x000FD8DD File Offset: 0x000FBADD
		public StatesInstance(RocketEngineCluster smi) : base(smi)
		{
			if (smi.emitRadiation)
			{
				DebugUtil.Assert(smi.radiationEmitter != null, "emitRadiation enabled but no RadiationEmitter component");
				this.radiationEmissionBaseOffset = smi.radiationEmitter.emissionOffset;
			}
		}

		// Token: 0x06008808 RID: 34824 RVA: 0x00361340 File Offset: 0x0035F540
		public void BeginBurn()
		{
			if (base.smi.master.emitRadiation)
			{
				base.smi.master.radiationEmitter.SetEmitting(true);
			}
			LaunchPad currentPad = base.smi.master.GetComponent<RocketModuleCluster>().CraftInterface.CurrentPad;
			if (currentPad != null)
			{
				this.pad_cell = Grid.PosToCell(currentPad.gameObject.transform.GetPosition());
				if (base.smi.master.exhaustDiseaseIdx != 255)
				{
					currentPad.GetComponent<PrimaryElement>().AddDisease(base.smi.master.exhaustDiseaseIdx, base.smi.master.exhaustDiseaseCount, "rocket exhaust");
					return;
				}
			}
			else
			{
				global::Debug.LogWarning("RocketEngineCluster missing LaunchPad for burn.");
				this.pad_cell = Grid.InvalidCell;
			}
		}

		// Token: 0x06008809 RID: 34825 RVA: 0x00361414 File Offset: 0x0035F614
		public void DoBurn(float dt)
		{
			int num = Grid.PosToCell(base.smi.master.gameObject.transform.GetPosition() + base.smi.master.animController.Offset);
			if (Grid.AreCellsInSameWorld(num, this.pad_cell))
			{
				SimMessages.EmitMass(num, ElementLoader.GetElementIndex(base.smi.master.exhaustElement), dt * base.smi.master.exhaustEmitRate, base.smi.master.exhaustTemperature, base.smi.master.exhaustDiseaseIdx, base.smi.master.exhaustDiseaseCount, -1);
			}
			if (base.smi.master.emitRadiation)
			{
				Vector3 emissionOffset = base.smi.master.radiationEmitter.emissionOffset;
				base.smi.master.radiationEmitter.emissionOffset = base.smi.radiationEmissionBaseOffset + base.smi.master.animController.Offset;
				if (Grid.AreCellsInSameWorld(base.smi.master.radiationEmitter.GetEmissionCell(), this.pad_cell))
				{
					base.smi.master.radiationEmitter.Refresh();
				}
				else
				{
					base.smi.master.radiationEmitter.emissionOffset = emissionOffset;
					base.smi.master.radiationEmitter.SetEmitting(false);
				}
			}
			int num2 = 10;
			for (int i = 1; i < num2; i++)
			{
				int num3 = Grid.OffsetCell(num, -1, -i);
				int num4 = Grid.OffsetCell(num, 0, -i);
				int num5 = Grid.OffsetCell(num, 1, -i);
				if (Grid.AreCellsInSameWorld(num3, this.pad_cell))
				{
					if (base.smi.master.exhaustDiseaseIdx != 255)
					{
						SimMessages.ModifyDiseaseOnCell(num3, base.smi.master.exhaustDiseaseIdx, (int)((float)base.smi.master.exhaustDiseaseCount / ((float)i + 1f)));
					}
					SimMessages.ModifyEnergy(num3, base.smi.master.exhaustTemperature / (float)(i + 1), 3200f, SimMessages.EnergySourceID.Burner);
				}
				if (Grid.AreCellsInSameWorld(num4, this.pad_cell))
				{
					if (base.smi.master.exhaustDiseaseIdx != 255)
					{
						SimMessages.ModifyDiseaseOnCell(num4, base.smi.master.exhaustDiseaseIdx, (int)((float)base.smi.master.exhaustDiseaseCount / (float)i));
					}
					SimMessages.ModifyEnergy(num4, base.smi.master.exhaustTemperature / (float)i, 3200f, SimMessages.EnergySourceID.Burner);
				}
				if (Grid.AreCellsInSameWorld(num5, this.pad_cell))
				{
					if (base.smi.master.exhaustDiseaseIdx != 255)
					{
						SimMessages.ModifyDiseaseOnCell(num5, base.smi.master.exhaustDiseaseIdx, (int)((float)base.smi.master.exhaustDiseaseCount / ((float)i + 1f)));
					}
					SimMessages.ModifyEnergy(num5, base.smi.master.exhaustTemperature / (float)(i + 1), 3200f, SimMessages.EnergySourceID.Burner);
				}
			}
		}

		// Token: 0x0600880A RID: 34826 RVA: 0x0036173C File Offset: 0x0035F93C
		public void EndBurn()
		{
			if (base.smi.master.emitRadiation)
			{
				base.smi.master.radiationEmitter.emissionOffset = base.smi.radiationEmissionBaseOffset;
				base.smi.master.radiationEmitter.SetEmitting(false);
			}
			this.pad_cell = Grid.InvalidCell;
		}

		// Token: 0x0400671B RID: 26395
		public Vector3 radiationEmissionBaseOffset;

		// Token: 0x0400671C RID: 26396
		private int pad_cell;
	}

	// Token: 0x02001986 RID: 6534
	public class States : GameStateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster>
	{
		// Token: 0x0600880B RID: 34827 RVA: 0x0036179C File Offset: 0x0035F99C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.initializing.load;
			this.initializing.load.ScheduleGoTo(0f, this.initializing.decide);
			this.initializing.decide.Transition(this.space, new StateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.Transition.ConditionCallback(this.IsRocketInSpace), UpdateRate.SIM_200ms).Transition(this.burning, new StateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.Transition.ConditionCallback(this.IsRocketAirborne), UpdateRate.SIM_200ms).Transition(this.idle, new StateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.Transition.ConditionCallback(this.IsRocketGrounded), UpdateRate.SIM_200ms);
			this.idle.DefaultState(this.idle.grounded).EventTransition(GameHashes.RocketLaunched, this.burning_pre, null);
			this.idle.grounded.EventTransition(GameHashes.LaunchConditionChanged, this.idle.ready, new StateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.Transition.ConditionCallback(this.IsReadyToLaunch)).QueueAnim("grounded", true, null);
			this.idle.ready.EventTransition(GameHashes.LaunchConditionChanged, this.idle.grounded, GameStateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.Not(new StateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.Transition.ConditionCallback(this.IsReadyToLaunch))).PlayAnim("pre_ready_to_launch", KAnim.PlayMode.Once).QueueAnim("ready_to_launch", true, null).Exit(delegate(RocketEngineCluster.StatesInstance smi)
			{
				KAnimControllerBase component = smi.GetComponent<KAnimControllerBase>();
				if (component != null)
				{
					component.Play("pst_ready_to_launch", KAnim.PlayMode.Once, 1f, 0f);
				}
			});
			this.burning_pre.PlayAnim("launch_pre").OnAnimQueueComplete(this.burning);
			this.burning.EventTransition(GameHashes.RocketLanded, this.burnComplete, null).PlayAnim("launch_loop", KAnim.PlayMode.Loop).Enter(delegate(RocketEngineCluster.StatesInstance smi)
			{
				smi.BeginBurn();
			}).Update(delegate(RocketEngineCluster.StatesInstance smi, float dt)
			{
				smi.DoBurn(dt);
			}, UpdateRate.SIM_200ms, false).Exit(delegate(RocketEngineCluster.StatesInstance smi)
			{
				smi.EndBurn();
			}).TagTransition(GameTags.RocketInSpace, this.space, false);
			this.space.EventTransition(GameHashes.DoReturnRocket, this.burning, null);
			this.burnComplete.PlayAnim("launch_pst", KAnim.PlayMode.Loop).GoTo(this.idle);
		}

		// Token: 0x0600880C RID: 34828 RVA: 0x000FD915 File Offset: 0x000FBB15
		private bool IsReadyToLaunch(RocketEngineCluster.StatesInstance smi)
		{
			return smi.GetComponent<RocketModuleCluster>().CraftInterface.CheckPreppedForLaunch();
		}

		// Token: 0x0600880D RID: 34829 RVA: 0x000FD927 File Offset: 0x000FBB27
		public bool IsRocketAirborne(RocketEngineCluster.StatesInstance smi)
		{
			return smi.master.HasTag(GameTags.RocketNotOnGround) && !smi.master.HasTag(GameTags.RocketInSpace);
		}

		// Token: 0x0600880E RID: 34830 RVA: 0x000FD950 File Offset: 0x000FBB50
		public bool IsRocketGrounded(RocketEngineCluster.StatesInstance smi)
		{
			return smi.master.HasTag(GameTags.RocketOnGround);
		}

		// Token: 0x0600880F RID: 34831 RVA: 0x000FD962 File Offset: 0x000FBB62
		public bool IsRocketInSpace(RocketEngineCluster.StatesInstance smi)
		{
			return smi.master.HasTag(GameTags.RocketInSpace);
		}

		// Token: 0x0400671D RID: 26397
		public RocketEngineCluster.States.InitializingStates initializing;

		// Token: 0x0400671E RID: 26398
		public RocketEngineCluster.States.IdleStates idle;

		// Token: 0x0400671F RID: 26399
		public GameStateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.State burning_pre;

		// Token: 0x04006720 RID: 26400
		public GameStateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.State burning;

		// Token: 0x04006721 RID: 26401
		public GameStateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.State burnComplete;

		// Token: 0x04006722 RID: 26402
		public GameStateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.State space;

		// Token: 0x02001987 RID: 6535
		public class InitializingStates : GameStateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.State
		{
			// Token: 0x04006723 RID: 26403
			public GameStateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.State load;

			// Token: 0x04006724 RID: 26404
			public GameStateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.State decide;
		}

		// Token: 0x02001988 RID: 6536
		public class IdleStates : GameStateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.State
		{
			// Token: 0x04006725 RID: 26405
			public GameStateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.State grounded;

			// Token: 0x04006726 RID: 26406
			public GameStateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.State ready;
		}
	}
}
