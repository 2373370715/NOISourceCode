using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x0200170F RID: 5903
public class CritterTrapPlant : StateMachineComponent<CritterTrapPlant.StatesInstance>
{
	// Token: 0x06007992 RID: 31122 RVA: 0x000F467B File Offset: 0x000F287B
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.master.growing.enabled = false;
		base.Subscribe<CritterTrapPlant>(-216549700, CritterTrapPlant.OnUprootedDelegate);
		base.smi.StartSM();
	}

	// Token: 0x06007993 RID: 31123 RVA: 0x000F46B5 File Offset: 0x000F28B5
	public void RefreshPositionPercent()
	{
		this.animController.SetPositionPercent(this.growing.PercentOfCurrentHarvest());
	}

	// Token: 0x06007994 RID: 31124 RVA: 0x003236BC File Offset: 0x003218BC
	private void OnUprooted(object data = null)
	{
		GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.PlantDeathId), base.gameObject.transform.GetPosition(), Grid.SceneLayer.FXFront, null, 0).SetActive(true);
		base.gameObject.Trigger(1623392196, null);
		base.gameObject.GetComponent<KBatchedAnimController>().StopAndClear();
		UnityEngine.Object.Destroy(base.gameObject.GetComponent<KBatchedAnimController>());
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x06007995 RID: 31125 RVA: 0x000F42AD File Offset: 0x000F24AD
	protected void DestroySelf(object callbackParam)
	{
		CreatureHelpers.DeselectCreature(base.gameObject);
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x06007996 RID: 31126 RVA: 0x00323734 File Offset: 0x00321934
	public Notification CreateDeathNotification()
	{
		return new Notification(CREATURES.STATUSITEMS.PLANTDEATH.NOTIFICATION, NotificationType.Bad, (List<Notification> notificationList, object data) => CREATURES.STATUSITEMS.PLANTDEATH.NOTIFICATION_TOOLTIP + notificationList.ReduceMessages(false), "/t• " + base.gameObject.GetProperName(), true, 0f, null, null, null, true, false, false);
	}

	// Token: 0x04005B57 RID: 23383
	[MyCmpReq]
	private Crop crop;

	// Token: 0x04005B58 RID: 23384
	[MyCmpReq]
	private WiltCondition wiltCondition;

	// Token: 0x04005B59 RID: 23385
	[MyCmpReq]
	private ReceptacleMonitor rm;

	// Token: 0x04005B5A RID: 23386
	[MyCmpReq]
	private Growing growing;

	// Token: 0x04005B5B RID: 23387
	[MyCmpReq]
	private KAnimControllerBase animController;

	// Token: 0x04005B5C RID: 23388
	[MyCmpReq]
	private Harvestable harvestable;

	// Token: 0x04005B5D RID: 23389
	[MyCmpReq]
	private Storage storage;

	// Token: 0x04005B5E RID: 23390
	public float gasOutputRate;

	// Token: 0x04005B5F RID: 23391
	public float gasVentThreshold;

	// Token: 0x04005B60 RID: 23392
	public SimHashes outputElement;

	// Token: 0x04005B61 RID: 23393
	private float GAS_TEMPERATURE_DELTA = 10f;

	// Token: 0x04005B62 RID: 23394
	private static readonly EventSystem.IntraObjectHandler<CritterTrapPlant> OnUprootedDelegate = new EventSystem.IntraObjectHandler<CritterTrapPlant>(delegate(CritterTrapPlant component, object data)
	{
		component.OnUprooted(data);
	});

	// Token: 0x02001710 RID: 5904
	public class StatesInstance : GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.GameInstance
	{
		// Token: 0x06007999 RID: 31129 RVA: 0x000F46FC File Offset: 0x000F28FC
		public StatesInstance(CritterTrapPlant master) : base(master)
		{
		}

		// Token: 0x0600799A RID: 31130 RVA: 0x000F4705 File Offset: 0x000F2905
		public void OnTrapTriggered(object data)
		{
			base.smi.sm.trapTriggered.Trigger(base.smi);
		}

		// Token: 0x0600799B RID: 31131 RVA: 0x00323794 File Offset: 0x00321994
		public void AddGas(float dt)
		{
			float temperature = base.smi.GetComponent<PrimaryElement>().Temperature + base.smi.master.GAS_TEMPERATURE_DELTA;
			base.smi.master.storage.AddGasChunk(base.smi.master.outputElement, base.smi.master.gasOutputRate * dt, temperature, byte.MaxValue, 0, false, true);
			if (this.ShouldVentGas())
			{
				base.smi.sm.ventGas.Trigger(base.smi);
			}
		}

		// Token: 0x0600799C RID: 31132 RVA: 0x00323828 File Offset: 0x00321A28
		public void VentGas()
		{
			PrimaryElement primaryElement = base.smi.master.storage.FindPrimaryElement(base.smi.master.outputElement);
			if (primaryElement != null)
			{
				SimMessages.AddRemoveSubstance(Grid.PosToCell(base.smi.transform.GetPosition()), primaryElement.ElementID, CellEventLogger.Instance.Dumpable, primaryElement.Mass, primaryElement.Temperature, primaryElement.DiseaseIdx, primaryElement.DiseaseCount, true, -1);
				base.smi.master.storage.ConsumeIgnoringDisease(primaryElement.gameObject);
			}
		}

		// Token: 0x0600799D RID: 31133 RVA: 0x003238C4 File Offset: 0x00321AC4
		public bool ShouldVentGas()
		{
			PrimaryElement primaryElement = base.smi.master.storage.FindPrimaryElement(base.smi.master.outputElement);
			return !(primaryElement == null) && primaryElement.Mass >= base.smi.master.gasVentThreshold;
		}
	}

	// Token: 0x02001711 RID: 5905
	public class States : GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant>
	{
		// Token: 0x0600799E RID: 31134 RVA: 0x00323920 File Offset: 0x00321B20
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			default_state = this.trap;
			this.trap.DefaultState(this.trap.open);
			this.trap.open.ToggleComponent<TrapTrigger>(false).Enter(delegate(CritterTrapPlant.StatesInstance smi)
			{
				smi.VentGas();
				smi.master.storage.ConsumeAllIgnoringDisease();
			}).EventHandler(GameHashes.TrapTriggered, delegate(CritterTrapPlant.StatesInstance smi, object data)
			{
				smi.OnTrapTriggered(data);
			}).EventTransition(GameHashes.Wilt, this.trap.wilting, null).OnSignal(this.trapTriggered, this.trap.trigger).ParamTransition<bool>(this.hasEatenCreature, this.trap.digesting, GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.IsTrue).PlayAnim("idle_open", KAnim.PlayMode.Loop);
			this.trap.trigger.PlayAnim("trap", KAnim.PlayMode.Once).Enter(delegate(CritterTrapPlant.StatesInstance smi)
			{
				smi.master.storage.ConsumeAllIgnoringDisease();
				smi.sm.hasEatenCreature.Set(true, smi, false);
			}).OnAnimQueueComplete(this.trap.digesting);
			this.trap.digesting.PlayAnim("digesting_loop", KAnim.PlayMode.Loop).ToggleComponent<Growing>(false).EventTransition(GameHashes.Grow, this.fruiting.enter, (CritterTrapPlant.StatesInstance smi) => smi.master.growing.ReachedNextHarvest()).EventTransition(GameHashes.Wilt, this.trap.wilting, null).DefaultState(this.trap.digesting.idle);
			this.trap.digesting.idle.PlayAnim("digesting_loop", KAnim.PlayMode.Loop).Update(delegate(CritterTrapPlant.StatesInstance smi, float dt)
			{
				smi.AddGas(dt);
			}, UpdateRate.SIM_4000ms, false).OnSignal(this.ventGas, this.trap.digesting.vent_pre);
			this.trap.digesting.vent_pre.PlayAnim("vent_pre").Exit(delegate(CritterTrapPlant.StatesInstance smi)
			{
				smi.VentGas();
			}).OnAnimQueueComplete(this.trap.digesting.vent);
			this.trap.digesting.vent.PlayAnim("vent_loop", KAnim.PlayMode.Once).QueueAnim("vent_pst", false, null).OnAnimQueueComplete(this.trap.digesting.idle);
			this.trap.wilting.PlayAnim("wilt1", KAnim.PlayMode.Loop).EventTransition(GameHashes.WiltRecover, this.trap, (CritterTrapPlant.StatesInstance smi) => !smi.master.wiltCondition.IsWilting());
			this.fruiting.EventTransition(GameHashes.Wilt, this.fruiting.wilting, null).EventTransition(GameHashes.Harvest, this.harvest, null).DefaultState(this.fruiting.idle);
			this.fruiting.enter.PlayAnim("open_harvest", KAnim.PlayMode.Once).Exit(delegate(CritterTrapPlant.StatesInstance smi)
			{
				smi.VentGas();
				smi.master.storage.ConsumeAllIgnoringDisease();
			}).OnAnimQueueComplete(this.fruiting.idle);
			this.fruiting.idle.PlayAnim("harvestable_loop", KAnim.PlayMode.Once).Enter(delegate(CritterTrapPlant.StatesInstance smi)
			{
				if (smi.master.harvestable != null)
				{
					smi.master.harvestable.SetCanBeHarvested(true);
				}
			}).Transition(this.fruiting.old, new StateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.Transition.ConditionCallback(this.IsOld), UpdateRate.SIM_4000ms);
			this.fruiting.old.PlayAnim("wilt1", KAnim.PlayMode.Once).Enter(delegate(CritterTrapPlant.StatesInstance smi)
			{
				if (smi.master.harvestable != null)
				{
					smi.master.harvestable.SetCanBeHarvested(true);
				}
			}).Transition(this.fruiting.idle, GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.Not(new StateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.Transition.ConditionCallback(this.IsOld)), UpdateRate.SIM_4000ms);
			this.fruiting.wilting.PlayAnim("wilt1", KAnim.PlayMode.Once).EventTransition(GameHashes.WiltRecover, this.fruiting, (CritterTrapPlant.StatesInstance smi) => !smi.master.wiltCondition.IsWilting());
			this.harvest.PlayAnim("harvest", KAnim.PlayMode.Once).Enter(delegate(CritterTrapPlant.StatesInstance smi)
			{
				if (GameScheduler.Instance != null && smi.master != null)
				{
					GameScheduler.Instance.Schedule("SpawnFruit", 0.2f, new Action<object>(smi.master.crop.SpawnConfiguredFruit), null, null);
				}
				smi.master.harvestable.SetCanBeHarvested(false);
			}).Exit(delegate(CritterTrapPlant.StatesInstance smi)
			{
				smi.sm.hasEatenCreature.Set(false, smi, false);
			}).OnAnimQueueComplete(this.trap.open);
			this.dead.ToggleMainStatusItem(Db.Get().CreatureStatusItems.Dead, null).Enter(delegate(CritterTrapPlant.StatesInstance smi)
			{
				if (smi.master.rm.Replanted && !smi.master.GetComponent<KPrefabID>().HasTag(GameTags.Uprooted))
				{
					Notifier notifier = smi.master.gameObject.AddOrGet<Notifier>();
					Notification notification = smi.master.CreateDeathNotification();
					notifier.Add(notification, "");
				}
				GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.PlantDeathId), smi.master.transform.GetPosition(), Grid.SceneLayer.FXFront, null, 0).SetActive(true);
				Harvestable harvestable = smi.master.harvestable;
				if (harvestable != null && harvestable.CanBeHarvested && GameScheduler.Instance != null)
				{
					GameScheduler.Instance.Schedule("SpawnFruit", 0.2f, new Action<object>(smi.master.crop.SpawnConfiguredFruit), null, null);
				}
				smi.master.Trigger(1623392196, null);
				smi.master.GetComponent<KBatchedAnimController>().StopAndClear();
				UnityEngine.Object.Destroy(smi.master.GetComponent<KBatchedAnimController>());
				smi.Schedule(0.5f, new Action<object>(smi.master.DestroySelf), null);
			});
		}

		// Token: 0x0600799F RID: 31135 RVA: 0x000F4722 File Offset: 0x000F2922
		public bool IsOld(CritterTrapPlant.StatesInstance smi)
		{
			return smi.master.growing.PercentOldAge() > 0.5f;
		}

		// Token: 0x04005B63 RID: 23395
		public StateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.Signal trapTriggered;

		// Token: 0x04005B64 RID: 23396
		public StateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.Signal ventGas;

		// Token: 0x04005B65 RID: 23397
		public StateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.BoolParameter hasEatenCreature;

		// Token: 0x04005B66 RID: 23398
		public GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State dead;

		// Token: 0x04005B67 RID: 23399
		public CritterTrapPlant.States.FruitingStates fruiting;

		// Token: 0x04005B68 RID: 23400
		public GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State harvest;

		// Token: 0x04005B69 RID: 23401
		public CritterTrapPlant.States.TrapStates trap;

		// Token: 0x02001712 RID: 5906
		public class DigestingStates : GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State
		{
			// Token: 0x04005B6A RID: 23402
			public GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State idle;

			// Token: 0x04005B6B RID: 23403
			public GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State vent_pre;

			// Token: 0x04005B6C RID: 23404
			public GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State vent;
		}

		// Token: 0x02001713 RID: 5907
		public class TrapStates : GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State
		{
			// Token: 0x04005B6D RID: 23405
			public GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State open;

			// Token: 0x04005B6E RID: 23406
			public GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State trigger;

			// Token: 0x04005B6F RID: 23407
			public CritterTrapPlant.States.DigestingStates digesting;

			// Token: 0x04005B70 RID: 23408
			public GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State wilting;
		}

		// Token: 0x02001714 RID: 5908
		public class FruitingStates : GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State
		{
			// Token: 0x04005B71 RID: 23409
			public GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State enter;

			// Token: 0x04005B72 RID: 23410
			public GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State idle;

			// Token: 0x04005B73 RID: 23411
			public GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State old;

			// Token: 0x04005B74 RID: 23412
			public GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State wilting;
		}
	}
}
