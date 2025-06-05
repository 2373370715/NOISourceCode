using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000EC1 RID: 3777
[SerializationConfig(MemberSerialization.OptIn)]
public class ManualHighEnergyParticleSpawner : StateMachineComponent<ManualHighEnergyParticleSpawner.StatesInstance>, IHighEnergyParticleDirection
{
	// Token: 0x17000419 RID: 1049
	// (get) Token: 0x06004B80 RID: 19328 RVA: 0x000D53F4 File Offset: 0x000D35F4
	// (set) Token: 0x06004B81 RID: 19329 RVA: 0x0026CF88 File Offset: 0x0026B188
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

	// Token: 0x06004B82 RID: 19330 RVA: 0x000D53FC File Offset: 0x000D35FC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<ManualHighEnergyParticleSpawner>(-905833192, ManualHighEnergyParticleSpawner.OnCopySettingsDelegate);
	}

	// Token: 0x06004B83 RID: 19331 RVA: 0x0026CFE0 File Offset: 0x0026B1E0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		this.radiationEmitter.SetEmitting(false);
		this.directionController = new EightDirectionController(base.GetComponent<KBatchedAnimController>(), "redirector_target", "redirect", EightDirectionController.Offset.Infront);
		this.Direction = this.Direction;
		Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Radiation, true);
	}

	// Token: 0x06004B84 RID: 19332 RVA: 0x0026D040 File Offset: 0x0026B240
	private void OnCopySettings(object data)
	{
		ManualHighEnergyParticleSpawner component = ((GameObject)data).GetComponent<ManualHighEnergyParticleSpawner>();
		if (component != null)
		{
			this.Direction = component.Direction;
		}
	}

	// Token: 0x06004B85 RID: 19333 RVA: 0x0026D070 File Offset: 0x0026B270
	public void LauncherUpdate()
	{
		if (this.particleStorage.Particles > 0f)
		{
			int highEnergyParticleOutputCell = base.GetComponent<Building>().GetHighEnergyParticleOutputCell();
			GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab("HighEnergyParticle"), Grid.CellToPosCCC(highEnergyParticleOutputCell, Grid.SceneLayer.FXFront2), Grid.SceneLayer.FXFront2, null, 0);
			gameObject.SetActive(true);
			if (gameObject != null)
			{
				HighEnergyParticle component = gameObject.GetComponent<HighEnergyParticle>();
				component.payload = this.particleStorage.ConsumeAndGet(this.particleStorage.Particles);
				component.SetDirection(this.Direction);
				this.directionController.PlayAnim("redirect_send", KAnim.PlayMode.Once);
				this.directionController.controller.Queue("redirect", KAnim.PlayMode.Once, 1f, 0f);
			}
		}
	}

	// Token: 0x040034CF RID: 13519
	[MyCmpReq]
	private HighEnergyParticleStorage particleStorage;

	// Token: 0x040034D0 RID: 13520
	[MyCmpGet]
	private RadiationEmitter radiationEmitter;

	// Token: 0x040034D1 RID: 13521
	[Serialize]
	private EightDirection _direction;

	// Token: 0x040034D2 RID: 13522
	private EightDirectionController directionController;

	// Token: 0x040034D3 RID: 13523
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x040034D4 RID: 13524
	private static readonly EventSystem.IntraObjectHandler<ManualHighEnergyParticleSpawner> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<ManualHighEnergyParticleSpawner>(delegate(ManualHighEnergyParticleSpawner component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x02000EC2 RID: 3778
	public class StatesInstance : GameStateMachine<ManualHighEnergyParticleSpawner.States, ManualHighEnergyParticleSpawner.StatesInstance, ManualHighEnergyParticleSpawner, object>.GameInstance
	{
		// Token: 0x06004B88 RID: 19336 RVA: 0x000D5439 File Offset: 0x000D3639
		public StatesInstance(ManualHighEnergyParticleSpawner smi) : base(smi)
		{
		}

		// Token: 0x06004B89 RID: 19337 RVA: 0x000D5442 File Offset: 0x000D3642
		public bool IsComplexFabricatorWorkable(object data)
		{
			return data as ComplexFabricatorWorkable != null;
		}
	}

	// Token: 0x02000EC3 RID: 3779
	public class States : GameStateMachine<ManualHighEnergyParticleSpawner.States, ManualHighEnergyParticleSpawner.StatesInstance, ManualHighEnergyParticleSpawner>
	{
		// Token: 0x06004B8A RID: 19338 RVA: 0x0026D134 File Offset: 0x0026B334
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.inoperational;
			this.inoperational.Enter(delegate(ManualHighEnergyParticleSpawner.StatesInstance smi)
			{
				smi.master.radiationEmitter.SetEmitting(false);
			}).TagTransition(GameTags.Operational, this.ready, false);
			this.ready.DefaultState(this.ready.idle).TagTransition(GameTags.Operational, this.inoperational, true).Update(delegate(ManualHighEnergyParticleSpawner.StatesInstance smi, float dt)
			{
				smi.master.LauncherUpdate();
			}, UpdateRate.SIM_200ms, false);
			this.ready.idle.EventHandlerTransition(GameHashes.WorkableStartWork, this.ready.working, (ManualHighEnergyParticleSpawner.StatesInstance smi, object data) => smi.IsComplexFabricatorWorkable(data));
			this.ready.working.Enter(delegate(ManualHighEnergyParticleSpawner.StatesInstance smi)
			{
				smi.master.radiationEmitter.SetEmitting(true);
			}).EventHandlerTransition(GameHashes.WorkableCompleteWork, this.ready.idle, (ManualHighEnergyParticleSpawner.StatesInstance smi, object data) => smi.IsComplexFabricatorWorkable(data)).EventHandlerTransition(GameHashes.WorkableStopWork, this.ready.idle, (ManualHighEnergyParticleSpawner.StatesInstance smi, object data) => smi.IsComplexFabricatorWorkable(data)).Exit(delegate(ManualHighEnergyParticleSpawner.StatesInstance smi)
			{
				smi.master.radiationEmitter.SetEmitting(false);
			});
		}

		// Token: 0x040034D5 RID: 13525
		public GameStateMachine<ManualHighEnergyParticleSpawner.States, ManualHighEnergyParticleSpawner.StatesInstance, ManualHighEnergyParticleSpawner, object>.State inoperational;

		// Token: 0x040034D6 RID: 13526
		public ManualHighEnergyParticleSpawner.States.ReadyStates ready;

		// Token: 0x02000EC4 RID: 3780
		public class ReadyStates : GameStateMachine<ManualHighEnergyParticleSpawner.States, ManualHighEnergyParticleSpawner.StatesInstance, ManualHighEnergyParticleSpawner, object>.State
		{
			// Token: 0x040034D7 RID: 13527
			public GameStateMachine<ManualHighEnergyParticleSpawner.States, ManualHighEnergyParticleSpawner.StatesInstance, ManualHighEnergyParticleSpawner, object>.State idle;

			// Token: 0x040034D8 RID: 13528
			public GameStateMachine<ManualHighEnergyParticleSpawner.States, ManualHighEnergyParticleSpawner.StatesInstance, ManualHighEnergyParticleSpawner, object>.State working;
		}
	}
}
