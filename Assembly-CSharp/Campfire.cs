using System;

// Token: 0x02000CF4 RID: 3316
public class Campfire : GameStateMachine<Campfire, Campfire.Instance, IStateMachineTarget, Campfire.Def>
{
	// Token: 0x06003FA9 RID: 16297 RVA: 0x00246638 File Offset: 0x00244838
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.noOperational;
		this.noOperational.Enter(new StateMachine<Campfire, Campfire.Instance, IStateMachineTarget, Campfire.Def>.State.Callback(Campfire.DisableHeatEmission)).TagTransition(GameTags.Operational, this.operational, false).PlayAnim("off", KAnim.PlayMode.Once);
		this.operational.TagTransition(GameTags.Operational, this.noOperational, true).DefaultState(this.operational.needsFuel);
		this.operational.needsFuel.Enter(new StateMachine<Campfire, Campfire.Instance, IStateMachineTarget, Campfire.Def>.State.Callback(Campfire.DisableHeatEmission)).EventTransition(GameHashes.OnStorageChange, this.operational.working, new StateMachine<Campfire, Campfire.Instance, IStateMachineTarget, Campfire.Def>.Transition.ConditionCallback(Campfire.HasFuel)).PlayAnim("off", KAnim.PlayMode.Once);
		this.operational.working.Enter(new StateMachine<Campfire, Campfire.Instance, IStateMachineTarget, Campfire.Def>.State.Callback(Campfire.EnableHeatEmission)).EventTransition(GameHashes.OnStorageChange, this.operational.needsFuel, GameStateMachine<Campfire, Campfire.Instance, IStateMachineTarget, Campfire.Def>.Not(new StateMachine<Campfire, Campfire.Instance, IStateMachineTarget, Campfire.Def>.Transition.ConditionCallback(Campfire.HasFuel))).PlayAnim("on", KAnim.PlayMode.Loop).Exit(new StateMachine<Campfire, Campfire.Instance, IStateMachineTarget, Campfire.Def>.State.Callback(Campfire.DisableHeatEmission));
	}

	// Token: 0x06003FAA RID: 16298 RVA: 0x000CDCEC File Offset: 0x000CBEEC
	public static bool HasFuel(Campfire.Instance smi)
	{
		return smi.HasFuel;
	}

	// Token: 0x06003FAB RID: 16299 RVA: 0x000CDCF4 File Offset: 0x000CBEF4
	public static void EnableHeatEmission(Campfire.Instance smi)
	{
		smi.EnableHeatEmission();
	}

	// Token: 0x06003FAC RID: 16300 RVA: 0x000CDCFC File Offset: 0x000CBEFC
	public static void DisableHeatEmission(Campfire.Instance smi)
	{
		smi.DisableHeatEmission();
	}

	// Token: 0x04002BF5 RID: 11253
	public const string LIT_ANIM_NAME = "on";

	// Token: 0x04002BF6 RID: 11254
	public const string UNLIT_ANIM_NAME = "off";

	// Token: 0x04002BF7 RID: 11255
	public GameStateMachine<Campfire, Campfire.Instance, IStateMachineTarget, Campfire.Def>.State noOperational;

	// Token: 0x04002BF8 RID: 11256
	public Campfire.OperationalStates operational;

	// Token: 0x04002BF9 RID: 11257
	public StateMachine<Campfire, Campfire.Instance, IStateMachineTarget, Campfire.Def>.BoolParameter WarmAuraEnabled;

	// Token: 0x02000CF5 RID: 3317
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04002BFA RID: 11258
		public Tag fuelTag;

		// Token: 0x04002BFB RID: 11259
		public float initialFuelMass;
	}

	// Token: 0x02000CF6 RID: 3318
	public class OperationalStates : GameStateMachine<Campfire, Campfire.Instance, IStateMachineTarget, Campfire.Def>.State
	{
		// Token: 0x04002BFC RID: 11260
		public GameStateMachine<Campfire, Campfire.Instance, IStateMachineTarget, Campfire.Def>.State needsFuel;

		// Token: 0x04002BFD RID: 11261
		public GameStateMachine<Campfire, Campfire.Instance, IStateMachineTarget, Campfire.Def>.State working;
	}

	// Token: 0x02000CF7 RID: 3319
	public new class Instance : GameStateMachine<Campfire, Campfire.Instance, IStateMachineTarget, Campfire.Def>.GameInstance
	{
		// Token: 0x17000307 RID: 775
		// (get) Token: 0x06003FB0 RID: 16304 RVA: 0x000CDD14 File Offset: 0x000CBF14
		public bool HasFuel
		{
			get
			{
				return this.storage.MassStored() > 0f;
			}
		}

		// Token: 0x17000308 RID: 776
		// (get) Token: 0x06003FB1 RID: 16305 RVA: 0x000CDD28 File Offset: 0x000CBF28
		public bool IsAuraEnabled
		{
			get
			{
				return base.sm.WarmAuraEnabled.Get(this);
			}
		}

		// Token: 0x06003FB2 RID: 16306 RVA: 0x000CDD3B File Offset: 0x000CBF3B
		public Instance(IStateMachineTarget master, Campfire.Def def) : base(master, def)
		{
		}

		// Token: 0x06003FB3 RID: 16307 RVA: 0x00246758 File Offset: 0x00244958
		public void EnableHeatEmission()
		{
			this.operational.SetActive(true, false);
			this.light.enabled = true;
			this.heater.EnableEmission = true;
			this.decorProvider.SetValues(CampfireConfig.DECOR_ON);
			this.decorProvider.Refresh();
		}

		// Token: 0x06003FB4 RID: 16308 RVA: 0x002467A8 File Offset: 0x002449A8
		public void DisableHeatEmission()
		{
			this.operational.SetActive(false, false);
			this.light.enabled = false;
			this.heater.EnableEmission = false;
			this.decorProvider.SetValues(CampfireConfig.DECOR_OFF);
			this.decorProvider.Refresh();
		}

		// Token: 0x04002BFE RID: 11262
		[MyCmpGet]
		public Operational operational;

		// Token: 0x04002BFF RID: 11263
		[MyCmpGet]
		public Storage storage;

		// Token: 0x04002C00 RID: 11264
		[MyCmpGet]
		public RangeVisualizer rangeVisualizer;

		// Token: 0x04002C01 RID: 11265
		[MyCmpGet]
		public Light2D light;

		// Token: 0x04002C02 RID: 11266
		[MyCmpGet]
		public DirectVolumeHeater heater;

		// Token: 0x04002C03 RID: 11267
		[MyCmpGet]
		public DecorProvider decorProvider;
	}
}
