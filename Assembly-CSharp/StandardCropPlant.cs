using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02001751 RID: 5969
public class StandardCropPlant : StateMachineComponent<StandardCropPlant.StatesInstance>
{
	// Token: 0x06007AB0 RID: 31408 RVA: 0x000F5404 File Offset: 0x000F3604
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x06007AB1 RID: 31409 RVA: 0x000F42AD File Offset: 0x000F24AD
	protected void DestroySelf(object callbackParam)
	{
		CreatureHelpers.DeselectCreature(base.gameObject);
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x06007AB2 RID: 31410 RVA: 0x00326F30 File Offset: 0x00325130
	public Notification CreateDeathNotification()
	{
		return new Notification(CREATURES.STATUSITEMS.PLANTDEATH.NOTIFICATION, NotificationType.Bad, (List<Notification> notificationList, object data) => CREATURES.STATUSITEMS.PLANTDEATH.NOTIFICATION_TOOLTIP + notificationList.ReduceMessages(false), "/t• " + base.gameObject.GetProperName(), true, 0f, null, null, null, true, false, false);
	}

	// Token: 0x06007AB3 RID: 31411 RVA: 0x000F5417 File Offset: 0x000F3617
	public void RefreshPositionPercent()
	{
		this.animController.SetPositionPercent(this.growing.PercentOfCurrentHarvest());
	}

	// Token: 0x06007AB4 RID: 31412 RVA: 0x00326F90 File Offset: 0x00325190
	private static string ToolTipResolver(List<Notification> notificationList, object data)
	{
		string text = "";
		for (int i = 0; i < notificationList.Count; i++)
		{
			Notification notification = notificationList[i];
			text += (string)notification.tooltipData;
			if (i < notificationList.Count - 1)
			{
				text += "\n";
			}
		}
		return string.Format(CREATURES.STATUSITEMS.PLANTDEATH.NOTIFICATION_TOOLTIP, text);
	}

	// Token: 0x04005C55 RID: 23637
	private const int WILT_LEVELS = 3;

	// Token: 0x04005C56 RID: 23638
	[MyCmpReq]
	private Crop crop;

	// Token: 0x04005C57 RID: 23639
	[MyCmpReq]
	private WiltCondition wiltCondition;

	// Token: 0x04005C58 RID: 23640
	[MyCmpReq]
	private ReceptacleMonitor rm;

	// Token: 0x04005C59 RID: 23641
	[MyCmpReq]
	private Growing growing;

	// Token: 0x04005C5A RID: 23642
	[MyCmpReq]
	private KAnimControllerBase animController;

	// Token: 0x04005C5B RID: 23643
	[MyCmpGet]
	private Harvestable harvestable;

	// Token: 0x04005C5C RID: 23644
	public bool wiltsOnReadyToHarvest;

	// Token: 0x04005C5D RID: 23645
	public static StandardCropPlant.AnimSet defaultAnimSet = new StandardCropPlant.AnimSet
	{
		grow = "grow",
		grow_pst = "grow_pst",
		idle_full = "idle_full",
		wilt_base = "wilt",
		harvest = "harvest",
		waning = "waning"
	};

	// Token: 0x04005C5E RID: 23646
	public StandardCropPlant.AnimSet anims = StandardCropPlant.defaultAnimSet;

	// Token: 0x02001752 RID: 5970
	public class AnimSet
	{
		// Token: 0x06007AB7 RID: 31415 RVA: 0x00327054 File Offset: 0x00325254
		public string GetWiltLevel(int level)
		{
			if (this.m_wilt == null)
			{
				this.m_wilt = new string[3];
				for (int i = 0; i < 3; i++)
				{
					this.m_wilt[i] = this.wilt_base + (i + 1).ToString();
				}
			}
			return this.m_wilt[level - 1];
		}

		// Token: 0x04005C5F RID: 23647
		public string grow;

		// Token: 0x04005C60 RID: 23648
		public string grow_pst;

		// Token: 0x04005C61 RID: 23649
		public string idle_full;

		// Token: 0x04005C62 RID: 23650
		public string wilt_base;

		// Token: 0x04005C63 RID: 23651
		public string harvest;

		// Token: 0x04005C64 RID: 23652
		public string waning;

		// Token: 0x04005C65 RID: 23653
		private string[] m_wilt;
	}

	// Token: 0x02001753 RID: 5971
	public class States : GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant>
	{
		// Token: 0x06007AB9 RID: 31417 RVA: 0x003270AC File Offset: 0x003252AC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			default_state = this.alive;
			this.dead.ToggleMainStatusItem(Db.Get().CreatureStatusItems.Dead, null).Enter(delegate(StandardCropPlant.StatesInstance smi)
			{
				if (smi.master.rm.Replanted && !smi.master.GetComponent<KPrefabID>().HasTag(GameTags.Uprooted))
				{
					Notifier notifier = smi.master.gameObject.AddOrGet<Notifier>();
					Notification notification = smi.master.CreateDeathNotification();
					notifier.Add(notification, "");
				}
				GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.PlantDeathId), smi.master.transform.GetPosition(), Grid.SceneLayer.FXFront, null, 0).SetActive(true);
				Harvestable component = smi.master.GetComponent<Harvestable>();
				if (component != null && component.CanBeHarvested && GameScheduler.Instance != null)
				{
					GameScheduler.Instance.Schedule("SpawnFruit", 0.2f, new Action<object>(smi.master.crop.SpawnConfiguredFruit), null, null);
				}
				smi.master.Trigger(1623392196, null);
				smi.master.GetComponent<KBatchedAnimController>().StopAndClear();
				UnityEngine.Object.Destroy(smi.master.GetComponent<KBatchedAnimController>());
				smi.Schedule(0.5f, new Action<object>(smi.master.DestroySelf), null);
			});
			this.blighted.InitializeStates(this.masterTarget, this.dead).PlayAnim((StandardCropPlant.StatesInstance smi) => smi.master.anims.waning, KAnim.PlayMode.Once).ToggleMainStatusItem(Db.Get().CreatureStatusItems.Crop_Blighted, null).TagTransition(GameTags.Blighted, this.alive, true);
			this.alive.InitializeStates(this.masterTarget, this.dead).DefaultState(this.alive.idle).ToggleComponent<Growing>(false).TagTransition(GameTags.Blighted, this.blighted, false);
			this.alive.idle.EventTransition(GameHashes.Wilt, this.alive.wilting, (StandardCropPlant.StatesInstance smi) => smi.master.wiltCondition.IsWilting()).EventTransition(GameHashes.Grow, this.alive.pre_fruiting, (StandardCropPlant.StatesInstance smi) => smi.master.growing.ReachedNextHarvest()).EventTransition(GameHashes.CropSleep, this.alive.sleeping, new StateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.Transition.ConditionCallback(this.IsSleeping)).PlayAnim((StandardCropPlant.StatesInstance smi) => smi.master.anims.grow, KAnim.PlayMode.Paused).Enter(new StateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State.Callback(StandardCropPlant.States.RefreshPositionPercent)).Update(new Action<StandardCropPlant.StatesInstance, float>(StandardCropPlant.States.RefreshPositionPercent), UpdateRate.SIM_4000ms, false).EventHandler(GameHashes.ConsumePlant, new StateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State.Callback(StandardCropPlant.States.RefreshPositionPercent));
			this.alive.pre_fruiting.PlayAnim((StandardCropPlant.StatesInstance smi) => smi.master.anims.grow_pst, KAnim.PlayMode.Once).TriggerOnEnter(GameHashes.BurstEmitDisease, null).EventTransition(GameHashes.AnimQueueComplete, this.alive.fruiting, null).EventTransition(GameHashes.Wilt, this.alive.wilting, null).ScheduleGoTo(2f, this.alive.fruiting);
			this.alive.fruiting_lost.Enter(delegate(StandardCropPlant.StatesInstance smi)
			{
				if (smi.master.harvestable != null)
				{
					smi.master.harvestable.SetCanBeHarvested(false);
				}
			}).GoTo(this.alive.idle);
			this.alive.wilting.PlayAnim(new Func<StandardCropPlant.StatesInstance, string>(StandardCropPlant.States.GetWiltAnim), KAnim.PlayMode.Loop).EventTransition(GameHashes.WiltRecover, this.alive.idle, (StandardCropPlant.StatesInstance smi) => !smi.master.wiltCondition.IsWilting()).EventTransition(GameHashes.Harvest, this.alive.harvest, null);
			this.alive.sleeping.PlayAnim((StandardCropPlant.StatesInstance smi) => smi.master.anims.grow, KAnim.PlayMode.Once).EventTransition(GameHashes.CropWakeUp, this.alive.idle, GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.Not(new StateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.Transition.ConditionCallback(this.IsSleeping))).EventTransition(GameHashes.Harvest, this.alive.harvest, null).EventTransition(GameHashes.Wilt, this.alive.wilting, null);
			this.alive.fruiting.PlayAnim((StandardCropPlant.StatesInstance smi) => smi.master.anims.idle_full, KAnim.PlayMode.Loop).Enter(delegate(StandardCropPlant.StatesInstance smi)
			{
				if (smi.master.harvestable != null)
				{
					smi.master.harvestable.SetCanBeHarvested(true);
				}
			}).EventHandlerTransition(GameHashes.Wilt, this.alive.wilting, (StandardCropPlant.StatesInstance smi, object obj) => smi.master.wiltsOnReadyToHarvest).EventTransition(GameHashes.Harvest, this.alive.harvest, null).EventTransition(GameHashes.Grow, this.alive.fruiting_lost, (StandardCropPlant.StatesInstance smi) => !smi.master.growing.ReachedNextHarvest());
			this.alive.harvest.PlayAnim((StandardCropPlant.StatesInstance smi) => smi.master.anims.harvest, KAnim.PlayMode.Once).Enter(delegate(StandardCropPlant.StatesInstance smi)
			{
				if (smi.master != null)
				{
					smi.master.crop.SpawnConfiguredFruit(null);
				}
				if (smi.master.harvestable != null)
				{
					smi.master.harvestable.SetCanBeHarvested(false);
				}
			}).Exit(delegate(StandardCropPlant.StatesInstance smi)
			{
				smi.Trigger(113170146, null);
			}).OnAnimQueueComplete(this.alive.idle);
		}

		// Token: 0x06007ABA RID: 31418 RVA: 0x003275A4 File Offset: 0x003257A4
		private static string GetWiltAnim(StandardCropPlant.StatesInstance smi)
		{
			float num = smi.master.growing.PercentOfCurrentHarvest();
			int level;
			if (num < 0.75f)
			{
				level = 1;
			}
			else if (num < 1f)
			{
				level = 2;
			}
			else
			{
				level = 3;
			}
			return smi.master.anims.GetWiltLevel(level);
		}

		// Token: 0x06007ABB RID: 31419 RVA: 0x000F5442 File Offset: 0x000F3642
		private static void RefreshPositionPercent(StandardCropPlant.StatesInstance smi, float dt)
		{
			smi.master.RefreshPositionPercent();
		}

		// Token: 0x06007ABC RID: 31420 RVA: 0x000F5442 File Offset: 0x000F3642
		private static void RefreshPositionPercent(StandardCropPlant.StatesInstance smi)
		{
			smi.master.RefreshPositionPercent();
		}

		// Token: 0x06007ABD RID: 31421 RVA: 0x003275F0 File Offset: 0x003257F0
		public bool IsSleeping(StandardCropPlant.StatesInstance smi)
		{
			CropSleepingMonitor.Instance smi2 = smi.master.GetSMI<CropSleepingMonitor.Instance>();
			return smi2 != null && smi2.IsSleeping();
		}

		// Token: 0x04005C66 RID: 23654
		public StandardCropPlant.States.AliveStates alive;

		// Token: 0x04005C67 RID: 23655
		public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State dead;

		// Token: 0x04005C68 RID: 23656
		public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.PlantAliveSubState blighted;

		// Token: 0x02001754 RID: 5972
		public class AliveStates : GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.PlantAliveSubState
		{
			// Token: 0x04005C69 RID: 23657
			public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State idle;

			// Token: 0x04005C6A RID: 23658
			public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State pre_fruiting;

			// Token: 0x04005C6B RID: 23659
			public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State fruiting_lost;

			// Token: 0x04005C6C RID: 23660
			public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State barren;

			// Token: 0x04005C6D RID: 23661
			public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State fruiting;

			// Token: 0x04005C6E RID: 23662
			public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State wilting;

			// Token: 0x04005C6F RID: 23663
			public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State destroy;

			// Token: 0x04005C70 RID: 23664
			public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State harvest;

			// Token: 0x04005C71 RID: 23665
			public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State sleeping;
		}
	}

	// Token: 0x02001756 RID: 5974
	public class StatesInstance : GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.GameInstance
	{
		// Token: 0x06007AD2 RID: 31442 RVA: 0x000F556C File Offset: 0x000F376C
		public StatesInstance(StandardCropPlant master) : base(master)
		{
		}
	}
}
