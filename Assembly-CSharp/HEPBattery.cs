using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02001415 RID: 5141
public class HEPBattery : GameStateMachine<HEPBattery, HEPBattery.Instance, IStateMachineTarget, HEPBattery.Def>
{
	// Token: 0x06006924 RID: 26916 RVA: 0x002E88D4 File Offset: 0x002E6AD4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.inoperational;
		this.inoperational.PlayAnim("off").TagTransition(GameTags.Operational, this.operational, false).Update(delegate(HEPBattery.Instance smi, float dt)
		{
			smi.DoConsumeParticlesWhileDisabled(dt);
			smi.UpdateDecayStatusItem(false);
		}, UpdateRate.SIM_200ms, false);
		this.operational.Enter("SetActive(true)", delegate(HEPBattery.Instance smi)
		{
			smi.operational.SetActive(true, false);
		}).Exit("SetActive(false)", delegate(HEPBattery.Instance smi)
		{
			smi.operational.SetActive(false, false);
		}).PlayAnim("on", KAnim.PlayMode.Loop).TagTransition(GameTags.Operational, this.inoperational, true).Update(new Action<HEPBattery.Instance, float>(this.LauncherUpdate), UpdateRate.SIM_200ms, false);
	}

	// Token: 0x06006925 RID: 26917 RVA: 0x002E89BC File Offset: 0x002E6BBC
	public void LauncherUpdate(HEPBattery.Instance smi, float dt)
	{
		smi.UpdateDecayStatusItem(true);
		smi.UpdateMeter(null);
		smi.operational.SetActive(smi.particleStorage.Particles > 0f, false);
		smi.launcherTimer += dt;
		if (smi.launcherTimer < smi.def.minLaunchInterval || !smi.AllowSpawnParticles)
		{
			return;
		}
		if (smi.particleStorage.Particles >= smi.particleThreshold)
		{
			smi.launcherTimer = 0f;
			this.Fire(smi);
		}
	}

	// Token: 0x06006926 RID: 26918 RVA: 0x002E8A44 File Offset: 0x002E6C44
	public void Fire(HEPBattery.Instance smi)
	{
		int highEnergyParticleOutputCell = smi.GetComponent<Building>().GetHighEnergyParticleOutputCell();
		GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab("HighEnergyParticle"), Grid.CellToPosCCC(highEnergyParticleOutputCell, Grid.SceneLayer.FXFront2), Grid.SceneLayer.FXFront2, null, 0);
		gameObject.SetActive(true);
		if (gameObject != null)
		{
			HighEnergyParticle component = gameObject.GetComponent<HighEnergyParticle>();
			component.payload = smi.particleStorage.ConsumeAndGet(smi.particleThreshold);
			component.SetDirection(smi.def.direction);
		}
	}

	// Token: 0x04004FC6 RID: 20422
	public static readonly HashedString FIRE_PORT_ID = "HEPBatteryFire";

	// Token: 0x04004FC7 RID: 20423
	public GameStateMachine<HEPBattery, HEPBattery.Instance, IStateMachineTarget, HEPBattery.Def>.State inoperational;

	// Token: 0x04004FC8 RID: 20424
	public GameStateMachine<HEPBattery, HEPBattery.Instance, IStateMachineTarget, HEPBattery.Def>.State operational;

	// Token: 0x02001416 RID: 5142
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04004FC9 RID: 20425
		public float particleDecayRate;

		// Token: 0x04004FCA RID: 20426
		public float minLaunchInterval;

		// Token: 0x04004FCB RID: 20427
		public float minSlider;

		// Token: 0x04004FCC RID: 20428
		public float maxSlider;

		// Token: 0x04004FCD RID: 20429
		public EightDirection direction;
	}

	// Token: 0x02001417 RID: 5143
	public new class Instance : GameStateMachine<HEPBattery, HEPBattery.Instance, IStateMachineTarget, HEPBattery.Def>.GameInstance, ISingleSliderControl, ISliderControl
	{
		// Token: 0x0600692A RID: 26922 RVA: 0x002E8ABC File Offset: 0x002E6CBC
		public Instance(IStateMachineTarget master, HEPBattery.Def def) : base(master, def)
		{
			base.Subscribe(-801688580, new Action<object>(this.OnLogicValueChanged));
			base.Subscribe(-905833192, new Action<object>(this.OnCopySettings));
			this.meterController = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
			this.UpdateMeter(null);
		}

		// Token: 0x0600692B RID: 26923 RVA: 0x000E94F9 File Offset: 0x000E76F9
		public void DoConsumeParticlesWhileDisabled(float dt)
		{
			if (this.m_skipFirstUpdate)
			{
				this.m_skipFirstUpdate = false;
				return;
			}
			this.particleStorage.ConsumeAndGet(dt * base.def.particleDecayRate);
			this.UpdateMeter(null);
		}

		// Token: 0x0600692C RID: 26924 RVA: 0x000E952B File Offset: 0x000E772B
		public void UpdateMeter(object data = null)
		{
			this.meterController.SetPositionPercent(this.particleStorage.Particles / this.particleStorage.Capacity());
		}

		// Token: 0x0600692D RID: 26925 RVA: 0x002E8B48 File Offset: 0x002E6D48
		public void UpdateDecayStatusItem(bool hasPower)
		{
			if (!hasPower)
			{
				if (this.particleStorage.Particles > 0f)
				{
					if (this.statusHandle == Guid.Empty)
					{
						this.statusHandle = base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.LosingRadbolts, null);
						return;
					}
				}
				else if (this.statusHandle != Guid.Empty)
				{
					base.GetComponent<KSelectable>().RemoveStatusItem(this.statusHandle, false);
					this.statusHandle = Guid.Empty;
					return;
				}
			}
			else if (this.statusHandle != Guid.Empty)
			{
				base.GetComponent<KSelectable>().RemoveStatusItem(this.statusHandle, false);
				this.statusHandle = Guid.Empty;
			}
		}

		// Token: 0x170006AA RID: 1706
		// (get) Token: 0x0600692E RID: 26926 RVA: 0x000E954F File Offset: 0x000E774F
		public bool AllowSpawnParticles
		{
			get
			{
				return this.hasLogicWire && this.isLogicActive;
			}
		}

		// Token: 0x170006AB RID: 1707
		// (get) Token: 0x0600692F RID: 26927 RVA: 0x000E9561 File Offset: 0x000E7761
		public bool HasLogicWire
		{
			get
			{
				return this.hasLogicWire;
			}
		}

		// Token: 0x170006AC RID: 1708
		// (get) Token: 0x06006930 RID: 26928 RVA: 0x000E9569 File Offset: 0x000E7769
		public bool IsLogicActive
		{
			get
			{
				return this.isLogicActive;
			}
		}

		// Token: 0x06006931 RID: 26929 RVA: 0x002E8C04 File Offset: 0x002E6E04
		private LogicCircuitNetwork GetNetwork()
		{
			int portCell = base.GetComponent<LogicPorts>().GetPortCell(HEPBattery.FIRE_PORT_ID);
			return Game.Instance.logicCircuitManager.GetNetworkForCell(portCell);
		}

		// Token: 0x06006932 RID: 26930 RVA: 0x002E8C34 File Offset: 0x002E6E34
		private void OnLogicValueChanged(object data)
		{
			LogicValueChanged logicValueChanged = (LogicValueChanged)data;
			if (logicValueChanged.portID == HEPBattery.FIRE_PORT_ID)
			{
				this.isLogicActive = (logicValueChanged.newValue > 0);
				this.hasLogicWire = (this.GetNetwork() != null);
			}
		}

		// Token: 0x06006933 RID: 26931 RVA: 0x002E8C78 File Offset: 0x002E6E78
		private void OnCopySettings(object data)
		{
			GameObject gameObject = data as GameObject;
			if (gameObject != null)
			{
				HEPBattery.Instance smi = gameObject.GetSMI<HEPBattery.Instance>();
				if (smi != null)
				{
					this.particleThreshold = smi.particleThreshold;
				}
			}
		}

		// Token: 0x170006AD RID: 1709
		// (get) Token: 0x06006934 RID: 26932 RVA: 0x000D2621 File Offset: 0x000D0821
		public string SliderTitleKey
		{
			get
			{
				return "STRINGS.UI.UISIDESCREENS.RADBOLTTHRESHOLDSIDESCREEN.TITLE";
			}
		}

		// Token: 0x170006AE RID: 1710
		// (get) Token: 0x06006935 RID: 26933 RVA: 0x000CF6FF File Offset: 0x000CD8FF
		public string SliderUnits
		{
			get
			{
				return UI.UNITSUFFIXES.HIGHENERGYPARTICLES.PARTRICLES;
			}
		}

		// Token: 0x06006936 RID: 26934 RVA: 0x000B1628 File Offset: 0x000AF828
		public int SliderDecimalPlaces(int index)
		{
			return 0;
		}

		// Token: 0x06006937 RID: 26935 RVA: 0x000E9571 File Offset: 0x000E7771
		public float GetSliderMin(int index)
		{
			return base.def.minSlider;
		}

		// Token: 0x06006938 RID: 26936 RVA: 0x000E957E File Offset: 0x000E777E
		public float GetSliderMax(int index)
		{
			return base.def.maxSlider;
		}

		// Token: 0x06006939 RID: 26937 RVA: 0x000E958B File Offset: 0x000E778B
		public float GetSliderValue(int index)
		{
			return this.particleThreshold;
		}

		// Token: 0x0600693A RID: 26938 RVA: 0x000E9593 File Offset: 0x000E7793
		public void SetSliderValue(float value, int index)
		{
			this.particleThreshold = value;
		}

		// Token: 0x0600693B RID: 26939 RVA: 0x000D2643 File Offset: 0x000D0843
		public string GetSliderTooltipKey(int index)
		{
			return "STRINGS.UI.UISIDESCREENS.RADBOLTTHRESHOLDSIDESCREEN.TOOLTIP";
		}

		// Token: 0x0600693C RID: 26940 RVA: 0x000E959C File Offset: 0x000E779C
		string ISliderControl.GetSliderTooltip(int index)
		{
			return string.Format(Strings.Get("STRINGS.UI.UISIDESCREENS.RADBOLTTHRESHOLDSIDESCREEN.TOOLTIP"), this.particleThreshold);
		}

		// Token: 0x04004FCE RID: 20430
		[MyCmpReq]
		public HighEnergyParticleStorage particleStorage;

		// Token: 0x04004FCF RID: 20431
		[MyCmpGet]
		public Operational operational;

		// Token: 0x04004FD0 RID: 20432
		[MyCmpAdd]
		public CopyBuildingSettings copyBuildingSettings;

		// Token: 0x04004FD1 RID: 20433
		[Serialize]
		public float launcherTimer;

		// Token: 0x04004FD2 RID: 20434
		[Serialize]
		public float particleThreshold = 50f;

		// Token: 0x04004FD3 RID: 20435
		public bool ShowWorkingStatus;

		// Token: 0x04004FD4 RID: 20436
		private bool m_skipFirstUpdate = true;

		// Token: 0x04004FD5 RID: 20437
		private MeterController meterController;

		// Token: 0x04004FD6 RID: 20438
		private Guid statusHandle = Guid.Empty;

		// Token: 0x04004FD7 RID: 20439
		private bool hasLogicWire;

		// Token: 0x04004FD8 RID: 20440
		private bool isLogicActive;
	}
}
