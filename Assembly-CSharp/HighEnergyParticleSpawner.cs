using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000E2E RID: 3630
[SerializationConfig(MemberSerialization.OptIn)]
public class HighEnergyParticleSpawner : StateMachineComponent<HighEnergyParticleSpawner.StatesInstance>, IHighEnergyParticleDirection, IProgressBarSideScreen, ISingleSliderControl, ISliderControl
{
	// Token: 0x1700036B RID: 875
	// (get) Token: 0x060046EA RID: 18154 RVA: 0x000D258C File Offset: 0x000D078C
	public float PredictedPerCycleConsumptionRate
	{
		get
		{
			return (float)Mathf.FloorToInt(this.recentPerSecondConsumptionRate * 0.1f * 600f);
		}
	}

	// Token: 0x1700036C RID: 876
	// (get) Token: 0x060046EB RID: 18155 RVA: 0x000D25A6 File Offset: 0x000D07A6
	// (set) Token: 0x060046EC RID: 18156 RVA: 0x0025ED98 File Offset: 0x0025CF98
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

	// Token: 0x060046ED RID: 18157 RVA: 0x0025EDF0 File Offset: 0x0025CFF0
	private void OnCopySettings(object data)
	{
		HighEnergyParticleSpawner component = ((GameObject)data).GetComponent<HighEnergyParticleSpawner>();
		if (component != null)
		{
			this.Direction = component.Direction;
			this.particleThreshold = component.particleThreshold;
		}
	}

	// Token: 0x060046EE RID: 18158 RVA: 0x000D25AE File Offset: 0x000D07AE
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<HighEnergyParticleSpawner>(-905833192, HighEnergyParticleSpawner.OnCopySettingsDelegate);
	}

	// Token: 0x060046EF RID: 18159 RVA: 0x0025EE2C File Offset: 0x0025D02C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		this.directionController = new EightDirectionController(base.GetComponent<KBatchedAnimController>(), "redirector_target", "redirect", EightDirectionController.Offset.Infront);
		this.Direction = this.Direction;
		this.particleController = new MeterController(base.GetComponent<KBatchedAnimController>(), "orb_target", "orb_off", Meter.Offset.NoChange, Grid.SceneLayer.NoLayer, Array.Empty<string>());
		this.particleController.gameObject.AddOrGet<LoopingSounds>();
		this.progressMeterController = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
		Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Radiation, true);
	}

	// Token: 0x060046F0 RID: 18160 RVA: 0x000D25C7 File Offset: 0x000D07C7
	public float GetProgressBarMaxValue()
	{
		return this.particleThreshold;
	}

	// Token: 0x060046F1 RID: 18161 RVA: 0x000D25CF File Offset: 0x000D07CF
	public float GetProgressBarFillPercentage()
	{
		return this.particleStorage.Particles / this.particleThreshold;
	}

	// Token: 0x060046F2 RID: 18162 RVA: 0x000D25E3 File Offset: 0x000D07E3
	public string GetProgressBarTitleLabel()
	{
		return UI.UISIDESCREENS.RADBOLTTHRESHOLDSIDESCREEN.PROGRESS_BAR_LABEL;
	}

	// Token: 0x060046F3 RID: 18163 RVA: 0x0025EED8 File Offset: 0x0025D0D8
	public string GetProgressBarLabel()
	{
		return Mathf.FloorToInt(this.particleStorage.Particles).ToString() + "/" + Mathf.FloorToInt(this.particleThreshold).ToString();
	}

	// Token: 0x060046F4 RID: 18164 RVA: 0x000D25EF File Offset: 0x000D07EF
	public string GetProgressBarTooltip()
	{
		return UI.UISIDESCREENS.RADBOLTTHRESHOLDSIDESCREEN.PROGRESS_BAR_TOOLTIP;
	}

	// Token: 0x060046F5 RID: 18165 RVA: 0x000D25FB File Offset: 0x000D07FB
	public void DoConsumeParticlesWhileDisabled(float dt)
	{
		this.particleStorage.ConsumeAndGet(dt * 1f);
		this.progressMeterController.SetPositionPercent(this.GetProgressBarFillPercentage());
	}

	// Token: 0x060046F6 RID: 18166 RVA: 0x0025EF1C File Offset: 0x0025D11C
	public void LauncherUpdate(float dt)
	{
		this.radiationSampleTimer += dt;
		if (this.radiationSampleTimer >= this.radiationSampleRate)
		{
			this.radiationSampleTimer -= this.radiationSampleRate;
			int i = Grid.PosToCell(this);
			float num = Grid.Radiation[i];
			if (num != 0f && this.particleStorage.RemainingCapacity() > 0f)
			{
				base.smi.sm.isAbsorbingRadiation.Set(true, base.smi, false);
				this.recentPerSecondConsumptionRate = num / 600f;
				this.particleStorage.Store(this.recentPerSecondConsumptionRate * this.radiationSampleRate * 0.1f);
			}
			else
			{
				this.recentPerSecondConsumptionRate = 0f;
				base.smi.sm.isAbsorbingRadiation.Set(false, base.smi, false);
			}
		}
		this.progressMeterController.SetPositionPercent(this.GetProgressBarFillPercentage());
		if (!this.particleVisualPlaying && this.particleStorage.Particles > this.particleThreshold / 2f)
		{
			this.particleController.meterController.Play("orb_pre", KAnim.PlayMode.Once, 1f, 0f);
			this.particleController.meterController.Queue("orb_idle", KAnim.PlayMode.Loop, 1f, 0f);
			this.particleVisualPlaying = true;
		}
		this.launcherTimer += dt;
		if (this.launcherTimer < this.minLaunchInterval)
		{
			return;
		}
		if (this.particleStorage.Particles >= this.particleThreshold)
		{
			this.launcherTimer = 0f;
			int highEnergyParticleOutputCell = base.GetComponent<Building>().GetHighEnergyParticleOutputCell();
			GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab("HighEnergyParticle"), Grid.CellToPosCCC(highEnergyParticleOutputCell, Grid.SceneLayer.FXFront2), Grid.SceneLayer.FXFront2, null, 0);
			gameObject.SetActive(true);
			if (gameObject != null)
			{
				HighEnergyParticle component = gameObject.GetComponent<HighEnergyParticle>();
				component.payload = this.particleStorage.ConsumeAndGet(this.particleThreshold);
				component.SetDirection(this.Direction);
				this.directionController.PlayAnim("redirect_send", KAnim.PlayMode.Once);
				this.directionController.controller.Queue("redirect", KAnim.PlayMode.Once, 1f, 0f);
				this.particleController.meterController.Play("orb_send", KAnim.PlayMode.Once, 1f, 0f);
				this.particleController.meterController.Queue("orb_off", KAnim.PlayMode.Once, 1f, 0f);
				this.particleVisualPlaying = false;
			}
		}
	}

	// Token: 0x1700036D RID: 877
	// (get) Token: 0x060046F7 RID: 18167 RVA: 0x000D2621 File Offset: 0x000D0821
	public string SliderTitleKey
	{
		get
		{
			return "STRINGS.UI.UISIDESCREENS.RADBOLTTHRESHOLDSIDESCREEN.TITLE";
		}
	}

	// Token: 0x1700036E RID: 878
	// (get) Token: 0x060046F8 RID: 18168 RVA: 0x000CF6FF File Offset: 0x000CD8FF
	public string SliderUnits
	{
		get
		{
			return UI.UNITSUFFIXES.HIGHENERGYPARTICLES.PARTRICLES;
		}
	}

	// Token: 0x060046F9 RID: 18169 RVA: 0x000B1628 File Offset: 0x000AF828
	public int SliderDecimalPlaces(int index)
	{
		return 0;
	}

	// Token: 0x060046FA RID: 18170 RVA: 0x000D2628 File Offset: 0x000D0828
	public float GetSliderMin(int index)
	{
		return (float)this.minSlider;
	}

	// Token: 0x060046FB RID: 18171 RVA: 0x000D2631 File Offset: 0x000D0831
	public float GetSliderMax(int index)
	{
		return (float)this.maxSlider;
	}

	// Token: 0x060046FC RID: 18172 RVA: 0x000D25C7 File Offset: 0x000D07C7
	public float GetSliderValue(int index)
	{
		return this.particleThreshold;
	}

	// Token: 0x060046FD RID: 18173 RVA: 0x000D263A File Offset: 0x000D083A
	public void SetSliderValue(float value, int index)
	{
		this.particleThreshold = value;
	}

	// Token: 0x060046FE RID: 18174 RVA: 0x000D2643 File Offset: 0x000D0843
	public string GetSliderTooltipKey(int index)
	{
		return "STRINGS.UI.UISIDESCREENS.RADBOLTTHRESHOLDSIDESCREEN.TOOLTIP";
	}

	// Token: 0x060046FF RID: 18175 RVA: 0x000D264A File Offset: 0x000D084A
	string ISliderControl.GetSliderTooltip(int index)
	{
		return string.Format(Strings.Get("STRINGS.UI.UISIDESCREENS.RADBOLTTHRESHOLDSIDESCREEN.TOOLTIP"), this.particleThreshold);
	}

	// Token: 0x0400318C RID: 12684
	[MyCmpReq]
	private HighEnergyParticleStorage particleStorage;

	// Token: 0x0400318D RID: 12685
	[MyCmpGet]
	private Operational operational;

	// Token: 0x0400318E RID: 12686
	private float recentPerSecondConsumptionRate;

	// Token: 0x0400318F RID: 12687
	public int minSlider;

	// Token: 0x04003190 RID: 12688
	public int maxSlider;

	// Token: 0x04003191 RID: 12689
	[Serialize]
	private EightDirection _direction;

	// Token: 0x04003192 RID: 12690
	public float minLaunchInterval;

	// Token: 0x04003193 RID: 12691
	public float radiationSampleRate;

	// Token: 0x04003194 RID: 12692
	[Serialize]
	public float particleThreshold = 50f;

	// Token: 0x04003195 RID: 12693
	private EightDirectionController directionController;

	// Token: 0x04003196 RID: 12694
	private float launcherTimer;

	// Token: 0x04003197 RID: 12695
	private float radiationSampleTimer;

	// Token: 0x04003198 RID: 12696
	private MeterController particleController;

	// Token: 0x04003199 RID: 12697
	private bool particleVisualPlaying;

	// Token: 0x0400319A RID: 12698
	private MeterController progressMeterController;

	// Token: 0x0400319B RID: 12699
	[Serialize]
	public Ref<HighEnergyParticlePort> capturedByRef = new Ref<HighEnergyParticlePort>();

	// Token: 0x0400319C RID: 12700
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x0400319D RID: 12701
	private static readonly EventSystem.IntraObjectHandler<HighEnergyParticleSpawner> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<HighEnergyParticleSpawner>(delegate(HighEnergyParticleSpawner component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x02000E2F RID: 3631
	public class StatesInstance : GameStateMachine<HighEnergyParticleSpawner.States, HighEnergyParticleSpawner.StatesInstance, HighEnergyParticleSpawner, object>.GameInstance
	{
		// Token: 0x06004702 RID: 18178 RVA: 0x000D26A5 File Offset: 0x000D08A5
		public StatesInstance(HighEnergyParticleSpawner smi) : base(smi)
		{
		}
	}

	// Token: 0x02000E30 RID: 3632
	public class States : GameStateMachine<HighEnergyParticleSpawner.States, HighEnergyParticleSpawner.StatesInstance, HighEnergyParticleSpawner>
	{
		// Token: 0x06004703 RID: 18179 RVA: 0x0025F1B0 File Offset: 0x0025D3B0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.inoperational;
			this.inoperational.PlayAnim("off").TagTransition(GameTags.Operational, this.ready, false).DefaultState(this.inoperational.empty);
			this.inoperational.empty.EventTransition(GameHashes.OnParticleStorageChanged, this.inoperational.losing, (HighEnergyParticleSpawner.StatesInstance smi) => !smi.GetComponent<HighEnergyParticleStorage>().IsEmpty());
			this.inoperational.losing.ToggleStatusItem(Db.Get().BuildingStatusItems.LosingRadbolts, null).Update(delegate(HighEnergyParticleSpawner.StatesInstance smi, float dt)
			{
				smi.master.DoConsumeParticlesWhileDisabled(dt);
			}, UpdateRate.SIM_1000ms, false).EventTransition(GameHashes.OnParticleStorageChanged, this.inoperational.empty, (HighEnergyParticleSpawner.StatesInstance smi) => smi.GetComponent<HighEnergyParticleStorage>().IsEmpty());
			this.ready.TagTransition(GameTags.Operational, this.inoperational, true).DefaultState(this.ready.idle).Update(delegate(HighEnergyParticleSpawner.StatesInstance smi, float dt)
			{
				smi.master.LauncherUpdate(dt);
			}, UpdateRate.SIM_EVERY_TICK, false);
			this.ready.idle.ParamTransition<bool>(this.isAbsorbingRadiation, this.ready.absorbing, GameStateMachine<HighEnergyParticleSpawner.States, HighEnergyParticleSpawner.StatesInstance, HighEnergyParticleSpawner, object>.IsTrue).PlayAnim("on");
			this.ready.absorbing.Enter("SetActive(true)", delegate(HighEnergyParticleSpawner.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).Exit("SetActive(false)", delegate(HighEnergyParticleSpawner.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			}).ParamTransition<bool>(this.isAbsorbingRadiation, this.ready.idle, GameStateMachine<HighEnergyParticleSpawner.States, HighEnergyParticleSpawner.StatesInstance, HighEnergyParticleSpawner, object>.IsFalse).ToggleStatusItem(Db.Get().BuildingStatusItems.CollectingHEP, (HighEnergyParticleSpawner.StatesInstance smi) => smi.master).PlayAnim("working_loop", KAnim.PlayMode.Loop);
		}

		// Token: 0x0400319E RID: 12702
		public StateMachine<HighEnergyParticleSpawner.States, HighEnergyParticleSpawner.StatesInstance, HighEnergyParticleSpawner, object>.BoolParameter isAbsorbingRadiation;

		// Token: 0x0400319F RID: 12703
		public HighEnergyParticleSpawner.States.ReadyStates ready;

		// Token: 0x040031A0 RID: 12704
		public HighEnergyParticleSpawner.States.InoperationalStates inoperational;

		// Token: 0x02000E31 RID: 3633
		public class InoperationalStates : GameStateMachine<HighEnergyParticleSpawner.States, HighEnergyParticleSpawner.StatesInstance, HighEnergyParticleSpawner, object>.State
		{
			// Token: 0x040031A1 RID: 12705
			public GameStateMachine<HighEnergyParticleSpawner.States, HighEnergyParticleSpawner.StatesInstance, HighEnergyParticleSpawner, object>.State empty;

			// Token: 0x040031A2 RID: 12706
			public GameStateMachine<HighEnergyParticleSpawner.States, HighEnergyParticleSpawner.StatesInstance, HighEnergyParticleSpawner, object>.State losing;
		}

		// Token: 0x02000E32 RID: 3634
		public class ReadyStates : GameStateMachine<HighEnergyParticleSpawner.States, HighEnergyParticleSpawner.StatesInstance, HighEnergyParticleSpawner, object>.State
		{
			// Token: 0x040031A3 RID: 12707
			public GameStateMachine<HighEnergyParticleSpawner.States, HighEnergyParticleSpawner.StatesInstance, HighEnergyParticleSpawner, object>.State idle;

			// Token: 0x040031A4 RID: 12708
			public GameStateMachine<HighEnergyParticleSpawner.States, HighEnergyParticleSpawner.StatesInstance, HighEnergyParticleSpawner, object>.State absorbing;
		}
	}
}
