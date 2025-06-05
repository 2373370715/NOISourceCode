using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000E29 RID: 3625
[SerializationConfig(MemberSerialization.OptIn)]
public class HighEnergyParticleRedirector : StateMachineComponent<HighEnergyParticleRedirector.StatesInstance>, IHighEnergyParticleDirection
{
	// Token: 0x17000367 RID: 871
	// (get) Token: 0x060046CE RID: 18126 RVA: 0x000D24C2 File Offset: 0x000D06C2
	// (set) Token: 0x060046CF RID: 18127 RVA: 0x0025E8A4 File Offset: 0x0025CAA4
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

	// Token: 0x060046D0 RID: 18128 RVA: 0x0025E8FC File Offset: 0x0025CAFC
	private void OnCopySettings(object data)
	{
		HighEnergyParticleRedirector component = ((GameObject)data).GetComponent<HighEnergyParticleRedirector>();
		if (component != null)
		{
			this.Direction = component.Direction;
		}
	}

	// Token: 0x060046D1 RID: 18129 RVA: 0x000D24CA File Offset: 0x000D06CA
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<HighEnergyParticleRedirector>(-905833192, HighEnergyParticleRedirector.OnCopySettingsDelegate);
		base.Subscribe<HighEnergyParticleRedirector>(-801688580, HighEnergyParticleRedirector.OnLogicValueChangedDelegate);
	}

	// Token: 0x060046D2 RID: 18130 RVA: 0x0025E92C File Offset: 0x0025CB2C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		HighEnergyParticlePort component = base.GetComponent<HighEnergyParticlePort>();
		if (component)
		{
			HighEnergyParticlePort highEnergyParticlePort = component;
			highEnergyParticlePort.onParticleCaptureAllowed = (HighEnergyParticlePort.OnParticleCaptureAllowed)Delegate.Combine(highEnergyParticlePort.onParticleCaptureAllowed, new HighEnergyParticlePort.OnParticleCaptureAllowed(this.OnParticleCaptureAllowed));
		}
		if (HighEnergyParticleRedirector.infoStatusItem_Logic == null)
		{
			HighEnergyParticleRedirector.infoStatusItem_Logic = new StatusItem("HEPRedirectorLogic", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			HighEnergyParticleRedirector.infoStatusItem_Logic.resolveStringCallback = new Func<string, object, string>(HighEnergyParticleRedirector.ResolveInfoStatusItem);
			HighEnergyParticleRedirector.infoStatusItem_Logic.resolveTooltipCallback = new Func<string, object, string>(HighEnergyParticleRedirector.ResolveInfoStatusItemTooltip);
		}
		this.selectable.AddStatusItem(HighEnergyParticleRedirector.infoStatusItem_Logic, this);
		this.directionController = new EightDirectionController(base.GetComponent<KBatchedAnimController>(), "redirector_target", "redirector", EightDirectionController.Offset.Infront);
		this.Direction = this.Direction;
		base.smi.StartSM();
		Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Radiation, true);
	}

	// Token: 0x060046D3 RID: 18131 RVA: 0x000D24F4 File Offset: 0x000D06F4
	private bool OnParticleCaptureAllowed(HighEnergyParticle particle)
	{
		return this.AllowIncomingParticles;
	}

	// Token: 0x060046D4 RID: 18132 RVA: 0x0025EA20 File Offset: 0x0025CC20
	private void LaunchParticle()
	{
		if (base.smi.master.storage.Particles < 0.1f)
		{
			base.smi.master.storage.ConsumeAll();
			return;
		}
		int highEnergyParticleOutputCell = base.GetComponent<Building>().GetHighEnergyParticleOutputCell();
		GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab("HighEnergyParticle"), Grid.CellToPosCCC(highEnergyParticleOutputCell, Grid.SceneLayer.FXFront2), Grid.SceneLayer.FXFront2, null, 0);
		gameObject.SetActive(true);
		if (gameObject != null)
		{
			HighEnergyParticle component = gameObject.GetComponent<HighEnergyParticle>();
			component.payload = base.smi.master.storage.ConsumeAll();
			component.payload -= 0.1f;
			component.capturedBy = this.port;
			component.SetDirection(this.Direction);
			this.directionController.PlayAnim("redirector_send", KAnim.PlayMode.Once);
			this.directionController.controller.Queue("redirector", KAnim.PlayMode.Once, 1f, 0f);
		}
	}

	// Token: 0x060046D5 RID: 18133 RVA: 0x0025EB20 File Offset: 0x0025CD20
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		HighEnergyParticlePort component = base.GetComponent<HighEnergyParticlePort>();
		if (component != null)
		{
			HighEnergyParticlePort highEnergyParticlePort = component;
			highEnergyParticlePort.onParticleCaptureAllowed = (HighEnergyParticlePort.OnParticleCaptureAllowed)Delegate.Remove(highEnergyParticlePort.onParticleCaptureAllowed, new HighEnergyParticlePort.OnParticleCaptureAllowed(this.OnParticleCaptureAllowed));
		}
	}

	// Token: 0x17000368 RID: 872
	// (get) Token: 0x060046D6 RID: 18134 RVA: 0x000D24FC File Offset: 0x000D06FC
	public bool AllowIncomingParticles
	{
		get
		{
			return !this.hasLogicWire || (this.hasLogicWire && this.isLogicActive);
		}
	}

	// Token: 0x17000369 RID: 873
	// (get) Token: 0x060046D7 RID: 18135 RVA: 0x000D2518 File Offset: 0x000D0718
	public bool HasLogicWire
	{
		get
		{
			return this.hasLogicWire;
		}
	}

	// Token: 0x1700036A RID: 874
	// (get) Token: 0x060046D8 RID: 18136 RVA: 0x000D2520 File Offset: 0x000D0720
	public bool IsLogicActive
	{
		get
		{
			return this.isLogicActive;
		}
	}

	// Token: 0x060046D9 RID: 18137 RVA: 0x0025EB68 File Offset: 0x0025CD68
	private LogicCircuitNetwork GetNetwork()
	{
		int portCell = base.GetComponent<LogicPorts>().GetPortCell(HighEnergyParticleRedirector.PORT_ID);
		return Game.Instance.logicCircuitManager.GetNetworkForCell(portCell);
	}

	// Token: 0x060046DA RID: 18138 RVA: 0x0025EB98 File Offset: 0x0025CD98
	private void OnLogicValueChanged(object data)
	{
		LogicValueChanged logicValueChanged = (LogicValueChanged)data;
		if (logicValueChanged.portID == HighEnergyParticleRedirector.PORT_ID)
		{
			this.isLogicActive = (logicValueChanged.newValue > 0);
			this.hasLogicWire = (this.GetNetwork() != null);
		}
	}

	// Token: 0x060046DB RID: 18139 RVA: 0x0025EBDC File Offset: 0x0025CDDC
	private static string ResolveInfoStatusItem(string format_str, object data)
	{
		HighEnergyParticleRedirector highEnergyParticleRedirector = (HighEnergyParticleRedirector)data;
		if (!highEnergyParticleRedirector.HasLogicWire)
		{
			return BUILDING.STATUSITEMS.HIGHENERGYPARTICLEREDIRECTOR.NORMAL;
		}
		if (highEnergyParticleRedirector.IsLogicActive)
		{
			return BUILDING.STATUSITEMS.HIGHENERGYPARTICLEREDIRECTOR.LOGIC_CONTROLLED_ACTIVE;
		}
		return BUILDING.STATUSITEMS.HIGHENERGYPARTICLEREDIRECTOR.LOGIC_CONTROLLED_STANDBY;
	}

	// Token: 0x060046DC RID: 18140 RVA: 0x0025EC20 File Offset: 0x0025CE20
	private static string ResolveInfoStatusItemTooltip(string format_str, object data)
	{
		HighEnergyParticleRedirector highEnergyParticleRedirector = (HighEnergyParticleRedirector)data;
		if (!highEnergyParticleRedirector.HasLogicWire)
		{
			return BUILDING.STATUSITEMS.HIGHENERGYPARTICLEREDIRECTOR.TOOLTIPS.NORMAL;
		}
		if (highEnergyParticleRedirector.IsLogicActive)
		{
			return BUILDING.STATUSITEMS.HIGHENERGYPARTICLEREDIRECTOR.TOOLTIPS.LOGIC_CONTROLLED_ACTIVE;
		}
		return BUILDING.STATUSITEMS.HIGHENERGYPARTICLEREDIRECTOR.TOOLTIPS.LOGIC_CONTROLLED_STANDBY;
	}

	// Token: 0x04003176 RID: 12662
	public static readonly HashedString PORT_ID = "HEPRedirector";

	// Token: 0x04003177 RID: 12663
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04003178 RID: 12664
	[MyCmpReq]
	private HighEnergyParticleStorage storage;

	// Token: 0x04003179 RID: 12665
	[MyCmpGet]
	private HighEnergyParticlePort port;

	// Token: 0x0400317A RID: 12666
	public float directorDelay;

	// Token: 0x0400317B RID: 12667
	public bool directionControllable = true;

	// Token: 0x0400317C RID: 12668
	[Serialize]
	private EightDirection _direction;

	// Token: 0x0400317D RID: 12669
	private EightDirectionController directionController;

	// Token: 0x0400317E RID: 12670
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x0400317F RID: 12671
	private static readonly EventSystem.IntraObjectHandler<HighEnergyParticleRedirector> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<HighEnergyParticleRedirector>(delegate(HighEnergyParticleRedirector component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x04003180 RID: 12672
	private static readonly EventSystem.IntraObjectHandler<HighEnergyParticleRedirector> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<HighEnergyParticleRedirector>(delegate(HighEnergyParticleRedirector component, object data)
	{
		component.OnLogicValueChanged(data);
	});

	// Token: 0x04003181 RID: 12673
	private bool hasLogicWire;

	// Token: 0x04003182 RID: 12674
	private bool isLogicActive;

	// Token: 0x04003183 RID: 12675
	private static StatusItem infoStatusItem_Logic;

	// Token: 0x02000E2A RID: 3626
	public class StatesInstance : GameStateMachine<HighEnergyParticleRedirector.States, HighEnergyParticleRedirector.StatesInstance, HighEnergyParticleRedirector, object>.GameInstance
	{
		// Token: 0x060046DF RID: 18143 RVA: 0x000D2537 File Offset: 0x000D0737
		public StatesInstance(HighEnergyParticleRedirector smi) : base(smi)
		{
		}
	}

	// Token: 0x02000E2B RID: 3627
	public class States : GameStateMachine<HighEnergyParticleRedirector.States, HighEnergyParticleRedirector.StatesInstance, HighEnergyParticleRedirector>
	{
		// Token: 0x060046E0 RID: 18144 RVA: 0x0025ECB4 File Offset: 0x0025CEB4
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.inoperational;
			this.inoperational.PlayAnim("off").TagTransition(GameTags.Operational, this.ready, false);
			this.ready.PlayAnim("on").TagTransition(GameTags.Operational, this.inoperational, true).EventTransition(GameHashes.OnParticleStorageChanged, this.redirect, null);
			this.redirect.PlayAnim("working_pre").QueueAnim("working_loop", false, null).QueueAnim("working_pst", false, null).ScheduleGoTo((HighEnergyParticleRedirector.StatesInstance smi) => smi.master.directorDelay, this.ready).Exit(delegate(HighEnergyParticleRedirector.StatesInstance smi)
			{
				smi.master.LaunchParticle();
			});
		}

		// Token: 0x04003184 RID: 12676
		public GameStateMachine<HighEnergyParticleRedirector.States, HighEnergyParticleRedirector.StatesInstance, HighEnergyParticleRedirector, object>.State inoperational;

		// Token: 0x04003185 RID: 12677
		public GameStateMachine<HighEnergyParticleRedirector.States, HighEnergyParticleRedirector.StatesInstance, HighEnergyParticleRedirector, object>.State ready;

		// Token: 0x04003186 RID: 12678
		public GameStateMachine<HighEnergyParticleRedirector.States, HighEnergyParticleRedirector.StatesInstance, HighEnergyParticleRedirector, object>.State redirect;

		// Token: 0x04003187 RID: 12679
		public GameStateMachine<HighEnergyParticleRedirector.States, HighEnergyParticleRedirector.StatesInstance, HighEnergyParticleRedirector, object>.State launch;
	}
}
