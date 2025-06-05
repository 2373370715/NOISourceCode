using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02001709 RID: 5897
[SkipSaveFileSerialization]
public class ColdBreather : StateMachineComponent<ColdBreather.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x06007972 RID: 31090 RVA: 0x000F4466 File Offset: 0x000F2666
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.simEmitCBHandle = Game.Instance.massEmitCallbackManager.Add(new Action<Sim.MassEmittedCallback, object>(ColdBreather.OnSimEmittedCallback), this, "ColdBreather");
		base.smi.StartSM();
	}

	// Token: 0x06007973 RID: 31091 RVA: 0x000F44A0 File Offset: 0x000F26A0
	protected override void OnPrefabInit()
	{
		this.elementConsumer.EnableConsumption(false);
		base.Subscribe<ColdBreather>(1309017699, ColdBreather.OnReplantedDelegate);
		base.OnPrefabInit();
	}

	// Token: 0x06007974 RID: 31092 RVA: 0x003230CC File Offset: 0x003212CC
	private void OnReplanted(object data = null)
	{
		ReceptacleMonitor component = base.GetComponent<ReceptacleMonitor>();
		if (component == null)
		{
			return;
		}
		ElementConsumer component2 = base.GetComponent<ElementConsumer>();
		if (component.Replanted)
		{
			component2.consumptionRate = this.consumptionRate;
		}
		else
		{
			component2.consumptionRate = this.consumptionRate * 0.25f;
		}
		if (this.radiationEmitter != null)
		{
			this.radiationEmitter.emitRads = 480f;
			this.radiationEmitter.Refresh();
		}
	}

	// Token: 0x06007975 RID: 31093 RVA: 0x00323144 File Offset: 0x00321344
	protected override void OnCleanUp()
	{
		Game.Instance.massEmitCallbackManager.Release(this.simEmitCBHandle, "coldbreather");
		this.simEmitCBHandle.Clear();
		if (this.storage)
		{
			this.storage.DropAll(true, false, default(Vector3), true, null);
		}
		base.OnCleanUp();
	}

	// Token: 0x06007976 RID: 31094 RVA: 0x000F42AD File Offset: 0x000F24AD
	protected void DestroySelf(object callbackParam)
	{
		CreatureHelpers.DeselectCreature(base.gameObject);
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x06007977 RID: 31095 RVA: 0x000F44C5 File Offset: 0x000F26C5
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return new List<Descriptor>
		{
			new Descriptor(UI.GAMEOBJECTEFFECTS.COLDBREATHER, UI.GAMEOBJECTEFFECTS.TOOLTIPS.COLDBREATHER, Descriptor.DescriptorType.Effect, false)
		};
	}

	// Token: 0x06007978 RID: 31096 RVA: 0x000F44ED File Offset: 0x000F26ED
	private void SetEmitting(bool emitting)
	{
		if (this.radiationEmitter != null)
		{
			this.radiationEmitter.SetEmitting(emitting);
		}
	}

	// Token: 0x06007979 RID: 31097 RVA: 0x003231A4 File Offset: 0x003213A4
	private void Exhale()
	{
		if (this.lastEmitTag != Tag.Invalid)
		{
			return;
		}
		this.gases.Clear();
		this.storage.Find(GameTags.Gas, this.gases);
		if (this.nextGasEmitIndex >= this.gases.Count)
		{
			this.nextGasEmitIndex = 0;
		}
		while (this.nextGasEmitIndex < this.gases.Count)
		{
			int num = this.nextGasEmitIndex;
			this.nextGasEmitIndex = num + 1;
			int index = num;
			PrimaryElement component = this.gases[index].GetComponent<PrimaryElement>();
			if (component != null && component.Mass > 0f && this.simEmitCBHandle.IsValid())
			{
				float temperature = Mathf.Max(component.Element.lowTemp + 5f, component.Temperature + this.deltaEmitTemperature);
				int gameCell = Grid.PosToCell(base.transform.GetPosition() + this.emitOffsetCell);
				ushort idx = component.Element.idx;
				Game.Instance.massEmitCallbackManager.GetItem(this.simEmitCBHandle);
				SimMessages.EmitMass(gameCell, idx, component.Mass, temperature, component.DiseaseIdx, component.DiseaseCount, this.simEmitCBHandle.index);
				this.lastEmitTag = component.Element.tag;
				return;
			}
		}
	}

	// Token: 0x0600797A RID: 31098 RVA: 0x000F4509 File Offset: 0x000F2709
	private static void OnSimEmittedCallback(Sim.MassEmittedCallback info, object data)
	{
		((ColdBreather)data).OnSimEmitted(info);
	}

	// Token: 0x0600797B RID: 31099 RVA: 0x00323308 File Offset: 0x00321508
	private void OnSimEmitted(Sim.MassEmittedCallback info)
	{
		if (info.suceeded == 1 && this.storage && this.lastEmitTag.IsValid)
		{
			this.storage.ConsumeIgnoringDisease(this.lastEmitTag, info.mass);
		}
		this.lastEmitTag = Tag.Invalid;
	}

	// Token: 0x04005B38 RID: 23352
	[MyCmpReq]
	private WiltCondition wiltCondition;

	// Token: 0x04005B39 RID: 23353
	[MyCmpReq]
	private KAnimControllerBase animController;

	// Token: 0x04005B3A RID: 23354
	[MyCmpReq]
	private Storage storage;

	// Token: 0x04005B3B RID: 23355
	[MyCmpReq]
	private ElementConsumer elementConsumer;

	// Token: 0x04005B3C RID: 23356
	[MyCmpGet]
	private RadiationEmitter radiationEmitter;

	// Token: 0x04005B3D RID: 23357
	[MyCmpReq]
	private ReceptacleMonitor receptacleMonitor;

	// Token: 0x04005B3E RID: 23358
	private const float EXHALE_PERIOD = 1f;

	// Token: 0x04005B3F RID: 23359
	public float consumptionRate;

	// Token: 0x04005B40 RID: 23360
	public float deltaEmitTemperature = -5f;

	// Token: 0x04005B41 RID: 23361
	public Vector3 emitOffsetCell = new Vector3(0f, 0f);

	// Token: 0x04005B42 RID: 23362
	private List<GameObject> gases = new List<GameObject>();

	// Token: 0x04005B43 RID: 23363
	private Tag lastEmitTag;

	// Token: 0x04005B44 RID: 23364
	private int nextGasEmitIndex;

	// Token: 0x04005B45 RID: 23365
	private HandleVector<Game.ComplexCallbackInfo<Sim.MassEmittedCallback>>.Handle simEmitCBHandle = HandleVector<Game.ComplexCallbackInfo<Sim.MassEmittedCallback>>.InvalidHandle;

	// Token: 0x04005B46 RID: 23366
	private static readonly EventSystem.IntraObjectHandler<ColdBreather> OnReplantedDelegate = new EventSystem.IntraObjectHandler<ColdBreather>(delegate(ColdBreather component, object data)
	{
		component.OnReplanted(data);
	});

	// Token: 0x0200170A RID: 5898
	public class StatesInstance : GameStateMachine<ColdBreather.States, ColdBreather.StatesInstance, ColdBreather, object>.GameInstance
	{
		// Token: 0x0600797E RID: 31102 RVA: 0x000F4571 File Offset: 0x000F2771
		public StatesInstance(ColdBreather master) : base(master)
		{
		}
	}

	// Token: 0x0200170B RID: 5899
	public class States : GameStateMachine<ColdBreather.States, ColdBreather.StatesInstance, ColdBreather>
	{
		// Token: 0x0600797F RID: 31103 RVA: 0x0032335C File Offset: 0x0032155C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			default_state = this.grow;
			this.statusItemCooling = new StatusItem("cooling", CREATURES.STATUSITEMS.COOLING.NAME, CREATURES.STATUSITEMS.COOLING.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 129022, true, null);
			GameStateMachine<ColdBreather.States, ColdBreather.StatesInstance, ColdBreather, object>.State state = this.dead;
			string name = CREATURES.STATUSITEMS.DEAD.NAME;
			string tooltip = CREATURES.STATUSITEMS.DEAD.TOOLTIP;
			string icon = "";
			StatusItem.IconType icon_type = StatusItem.IconType.Info;
			NotificationType notification_type = NotificationType.Neutral;
			bool allow_multiples = false;
			StatusItemCategory main = Db.Get().StatusItemCategories.Main;
			state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main).Enter(delegate(ColdBreather.StatesInstance smi)
			{
				GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.PlantDeathId), smi.master.transform.GetPosition(), Grid.SceneLayer.FXFront, null, 0).SetActive(true);
				smi.master.Trigger(1623392196, null);
				smi.master.GetComponent<KBatchedAnimController>().StopAndClear();
				UnityEngine.Object.Destroy(smi.master.GetComponent<KBatchedAnimController>());
				smi.Schedule(0.5f, new Action<object>(smi.master.DestroySelf), null);
			});
			this.blocked_from_growing.ToggleStatusItem(Db.Get().MiscStatusItems.RegionIsBlocked, null).EventTransition(GameHashes.EntombedChanged, this.alive, (ColdBreather.StatesInstance smi) => this.alive.ForceUpdateStatus(smi.master.gameObject)).EventTransition(GameHashes.TooColdWarning, this.alive, (ColdBreather.StatesInstance smi) => this.alive.ForceUpdateStatus(smi.master.gameObject)).EventTransition(GameHashes.TooHotWarning, this.alive, (ColdBreather.StatesInstance smi) => this.alive.ForceUpdateStatus(smi.master.gameObject)).TagTransition(GameTags.Uprooted, this.dead, false);
			this.grow.Enter(delegate(ColdBreather.StatesInstance smi)
			{
				if (smi.master.receptacleMonitor.HasReceptacle() && !this.alive.ForceUpdateStatus(smi.master.gameObject))
				{
					smi.GoTo(this.blocked_from_growing);
				}
			}).PlayAnim("grow_seed", KAnim.PlayMode.Once).EventTransition(GameHashes.AnimQueueComplete, this.alive, null);
			this.alive.InitializeStates(this.masterTarget, this.dead).DefaultState(this.alive.mature).Update(delegate(ColdBreather.StatesInstance smi, float dt)
			{
				smi.master.Exhale();
			}, UpdateRate.SIM_200ms, false);
			this.alive.mature.EventTransition(GameHashes.Wilt, this.alive.wilting, (ColdBreather.StatesInstance smi) => smi.master.wiltCondition.IsWilting()).PlayAnim("idle", KAnim.PlayMode.Loop).ToggleMainStatusItem(this.statusItemCooling, null).Enter(delegate(ColdBreather.StatesInstance smi)
			{
				smi.master.elementConsumer.EnableConsumption(true);
				smi.master.SetEmitting(true);
			}).Exit(delegate(ColdBreather.StatesInstance smi)
			{
				smi.master.elementConsumer.EnableConsumption(false);
				smi.master.SetEmitting(false);
			});
			this.alive.wilting.PlayAnim("wilt1").EventTransition(GameHashes.WiltRecover, this.alive.mature, (ColdBreather.StatesInstance smi) => !smi.master.wiltCondition.IsWilting()).Enter(delegate(ColdBreather.StatesInstance smi)
			{
				smi.master.SetEmitting(false);
			});
		}

		// Token: 0x04005B47 RID: 23367
		public GameStateMachine<ColdBreather.States, ColdBreather.StatesInstance, ColdBreather, object>.State grow;

		// Token: 0x04005B48 RID: 23368
		public GameStateMachine<ColdBreather.States, ColdBreather.StatesInstance, ColdBreather, object>.State blocked_from_growing;

		// Token: 0x04005B49 RID: 23369
		public ColdBreather.States.AliveStates alive;

		// Token: 0x04005B4A RID: 23370
		public GameStateMachine<ColdBreather.States, ColdBreather.StatesInstance, ColdBreather, object>.State dead;

		// Token: 0x04005B4B RID: 23371
		private StatusItem statusItemCooling;

		// Token: 0x0200170C RID: 5900
		public class AliveStates : GameStateMachine<ColdBreather.States, ColdBreather.StatesInstance, ColdBreather, object>.PlantAliveSubState
		{
			// Token: 0x04005B4C RID: 23372
			public GameStateMachine<ColdBreather.States, ColdBreather.StatesInstance, ColdBreather, object>.State mature;

			// Token: 0x04005B4D RID: 23373
			public GameStateMachine<ColdBreather.States, ColdBreather.StatesInstance, ColdBreather, object>.State wilting;
		}
	}
}
