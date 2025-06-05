using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000D67 RID: 3431
[SerializationConfig(MemberSerialization.OptIn)]
public class DevHEPSpawner : StateMachineComponent<DevHEPSpawner.StatesInstance>, IHighEnergyParticleDirection, ISingleSliderControl, ISliderControl
{
	// Token: 0x17000341 RID: 833
	// (get) Token: 0x0600427F RID: 17023 RVA: 0x000CF6DE File Offset: 0x000CD8DE
	// (set) Token: 0x06004280 RID: 17024 RVA: 0x0024FC98 File Offset: 0x0024DE98
	public EightDirection Direction
	{
		get
		{
			return this._direction;
		}
		set
		{
			this._direction = value;
			if (this.directionController != null)
			{
				this.directionController.SetRotation((float)(45 * EightDirectionUtil.GetDirectionIndex(this._direction)));
				this.directionController.controller.enabled = false;
				this.directionController.controller.enabled = true;
			}
		}
	}

	// Token: 0x06004281 RID: 17025 RVA: 0x0024FCF0 File Offset: 0x0024DEF0
	private void OnCopySettings(object data)
	{
		DevHEPSpawner component = ((GameObject)data).GetComponent<DevHEPSpawner>();
		if (component != null)
		{
			this.Direction = component.Direction;
			this.boltAmount = component.boltAmount;
		}
	}

	// Token: 0x06004282 RID: 17026 RVA: 0x000CF6E6 File Offset: 0x000CD8E6
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<DevHEPSpawner>(-905833192, DevHEPSpawner.OnCopySettingsDelegate);
	}

	// Token: 0x06004283 RID: 17027 RVA: 0x0024FD2C File Offset: 0x0024DF2C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		this.directionController = new EightDirectionController(base.GetComponent<KBatchedAnimController>(), "redirector_target", "redirect", EightDirectionController.Offset.Infront);
		this.Direction = this.Direction;
		this.particleController = new MeterController(base.GetComponent<KBatchedAnimController>(), "orb_target", "orb_off", Meter.Offset.NoChange, Grid.SceneLayer.NoLayer, Array.Empty<string>());
		this.particleController.gameObject.AddOrGet<LoopingSounds>();
		this.progressMeterController = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
	}

	// Token: 0x06004284 RID: 17028 RVA: 0x0024FDCC File Offset: 0x0024DFCC
	public void LauncherUpdate(float dt)
	{
		if (this.boltAmount <= 0f)
		{
			return;
		}
		this.launcherTimer += dt;
		this.progressMeterController.SetPositionPercent(this.launcherTimer / 5f);
		if (this.launcherTimer > 5f)
		{
			this.launcherTimer -= 5f;
			int highEnergyParticleOutputCell = base.GetComponent<Building>().GetHighEnergyParticleOutputCell();
			GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab("HighEnergyParticle"), Grid.CellToPosCCC(highEnergyParticleOutputCell, Grid.SceneLayer.FXFront2), Grid.SceneLayer.FXFront2, null, 0);
			gameObject.SetActive(true);
			if (gameObject != null)
			{
				HighEnergyParticle component = gameObject.GetComponent<HighEnergyParticle>();
				component.payload = this.boltAmount;
				component.SetDirection(this.Direction);
				this.directionController.PlayAnim("redirect_send", KAnim.PlayMode.Once);
				this.directionController.controller.Queue("redirect", KAnim.PlayMode.Once, 1f, 0f);
				this.particleController.meterController.Play("orb_send", KAnim.PlayMode.Once, 1f, 0f);
				this.particleController.meterController.Queue("orb_off", KAnim.PlayMode.Once, 1f, 0f);
			}
		}
	}

	// Token: 0x17000342 RID: 834
	// (get) Token: 0x06004285 RID: 17029 RVA: 0x000CBEB9 File Offset: 0x000CA0B9
	public string SliderTitleKey
	{
		get
		{
			return "";
		}
	}

	// Token: 0x17000343 RID: 835
	// (get) Token: 0x06004286 RID: 17030 RVA: 0x000CF6FF File Offset: 0x000CD8FF
	public string SliderUnits
	{
		get
		{
			return UI.UNITSUFFIXES.HIGHENERGYPARTICLES.PARTRICLES;
		}
	}

	// Token: 0x06004287 RID: 17031 RVA: 0x000B1628 File Offset: 0x000AF828
	public int SliderDecimalPlaces(int index)
	{
		return 0;
	}

	// Token: 0x06004288 RID: 17032 RVA: 0x000C18F8 File Offset: 0x000BFAF8
	public float GetSliderMin(int index)
	{
		return 0f;
	}

	// Token: 0x06004289 RID: 17033 RVA: 0x000CF70B File Offset: 0x000CD90B
	public float GetSliderMax(int index)
	{
		return 500f;
	}

	// Token: 0x0600428A RID: 17034 RVA: 0x000CF712 File Offset: 0x000CD912
	public float GetSliderValue(int index)
	{
		return this.boltAmount;
	}

	// Token: 0x0600428B RID: 17035 RVA: 0x000CF71A File Offset: 0x000CD91A
	public void SetSliderValue(float value, int index)
	{
		this.boltAmount = value;
	}

	// Token: 0x0600428C RID: 17036 RVA: 0x000CBEB9 File Offset: 0x000CA0B9
	public string GetSliderTooltipKey(int index)
	{
		return "";
	}

	// Token: 0x0600428D RID: 17037 RVA: 0x000CBEB9 File Offset: 0x000CA0B9
	string ISliderControl.GetSliderTooltip(int index)
	{
		return "";
	}

	// Token: 0x04002DE9 RID: 11753
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04002DEA RID: 11754
	[Serialize]
	private EightDirection _direction;

	// Token: 0x04002DEB RID: 11755
	public float boltAmount;

	// Token: 0x04002DEC RID: 11756
	private EightDirectionController directionController;

	// Token: 0x04002DED RID: 11757
	private float launcherTimer;

	// Token: 0x04002DEE RID: 11758
	private MeterController particleController;

	// Token: 0x04002DEF RID: 11759
	private MeterController progressMeterController;

	// Token: 0x04002DF0 RID: 11760
	[Serialize]
	public Ref<HighEnergyParticlePort> capturedByRef = new Ref<HighEnergyParticlePort>();

	// Token: 0x04002DF1 RID: 11761
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04002DF2 RID: 11762
	private static readonly EventSystem.IntraObjectHandler<DevHEPSpawner> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<DevHEPSpawner>(delegate(DevHEPSpawner component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x02000D68 RID: 3432
	public class StatesInstance : GameStateMachine<DevHEPSpawner.States, DevHEPSpawner.StatesInstance, DevHEPSpawner, object>.GameInstance
	{
		// Token: 0x06004290 RID: 17040 RVA: 0x000CF752 File Offset: 0x000CD952
		public StatesInstance(DevHEPSpawner smi) : base(smi)
		{
		}
	}

	// Token: 0x02000D69 RID: 3433
	public class States : GameStateMachine<DevHEPSpawner.States, DevHEPSpawner.StatesInstance, DevHEPSpawner>
	{
		// Token: 0x06004291 RID: 17041 RVA: 0x0024FF0C File Offset: 0x0024E10C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.inoperational;
			this.inoperational.PlayAnim("off").TagTransition(GameTags.Operational, this.ready, false);
			this.ready.PlayAnim("on").TagTransition(GameTags.Operational, this.inoperational, true).Update(delegate(DevHEPSpawner.StatesInstance smi, float dt)
			{
				smi.master.LauncherUpdate(dt);
			}, UpdateRate.SIM_EVERY_TICK, false);
		}

		// Token: 0x04002DF3 RID: 11763
		public StateMachine<DevHEPSpawner.States, DevHEPSpawner.StatesInstance, DevHEPSpawner, object>.BoolParameter isAbsorbingRadiation;

		// Token: 0x04002DF4 RID: 11764
		public GameStateMachine<DevHEPSpawner.States, DevHEPSpawner.StatesInstance, DevHEPSpawner, object>.State ready;

		// Token: 0x04002DF5 RID: 11765
		public GameStateMachine<DevHEPSpawner.States, DevHEPSpawner.StatesInstance, DevHEPSpawner, object>.State inoperational;
	}
}
