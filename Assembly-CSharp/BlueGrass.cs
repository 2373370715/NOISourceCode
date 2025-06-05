using System;
using STRINGS;
using UnityEngine;

// Token: 0x02001702 RID: 5890
public class BlueGrass : StateMachineComponent<BlueGrass.StatesInstance>
{
	// Token: 0x06007952 RID: 31058 RVA: 0x000F42AD File Offset: 0x000F24AD
	protected void DestroySelf(object callbackParam)
	{
		CreatureHelpers.DeselectCreature(base.gameObject);
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x06007953 RID: 31059 RVA: 0x000F4318 File Offset: 0x000F2518
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x06007954 RID: 31060 RVA: 0x000F432B File Offset: 0x000F252B
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x06007955 RID: 31061 RVA: 0x000F4333 File Offset: 0x000F2533
	protected override void OnPrefabInit()
	{
		base.Subscribe<BlueGrass>(1309017699, BlueGrass.OnReplantedDelegate);
		base.OnPrefabInit();
	}

	// Token: 0x06007956 RID: 31062 RVA: 0x000F434C File Offset: 0x000F254C
	private void OnReplanted(object data = null)
	{
		this.SetConsumptionRate();
	}

	// Token: 0x06007957 RID: 31063 RVA: 0x000F4354 File Offset: 0x000F2554
	public void SetConsumptionRate()
	{
		if (this.receptacleMonitor.Replanted)
		{
			this.elementConsumer.consumptionRate = 0.002f;
			return;
		}
		this.elementConsumer.consumptionRate = 0.0005f;
	}

	// Token: 0x04005B21 RID: 23329
	[MyCmpReq]
	private WiltCondition wiltCondition;

	// Token: 0x04005B22 RID: 23330
	[MyCmpReq]
	private ElementConsumer elementConsumer;

	// Token: 0x04005B23 RID: 23331
	[MyCmpReq]
	private ReceptacleMonitor receptacleMonitor;

	// Token: 0x04005B24 RID: 23332
	[MyCmpReq]
	private Growing growing;

	// Token: 0x04005B25 RID: 23333
	private static readonly EventSystem.IntraObjectHandler<BlueGrass> OnReplantedDelegate = new EventSystem.IntraObjectHandler<BlueGrass>(delegate(BlueGrass component, object data)
	{
		component.OnReplanted(data);
	});

	// Token: 0x02001703 RID: 5891
	public class StatesInstance : GameStateMachine<BlueGrass.States, BlueGrass.StatesInstance, BlueGrass, object>.GameInstance
	{
		// Token: 0x0600795A RID: 31066 RVA: 0x000F43A8 File Offset: 0x000F25A8
		public StatesInstance(BlueGrass master) : base(master)
		{
		}
	}

	// Token: 0x02001704 RID: 5892
	public class States : GameStateMachine<BlueGrass.States, BlueGrass.StatesInstance, BlueGrass>
	{
		// Token: 0x0600795B RID: 31067 RVA: 0x00322C94 File Offset: 0x00320E94
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.grow;
			GameStateMachine<BlueGrass.States, BlueGrass.StatesInstance, BlueGrass, object>.State state = this.dead;
			string name = CREATURES.STATUSITEMS.DEAD.NAME;
			string tooltip = CREATURES.STATUSITEMS.DEAD.TOOLTIP;
			string icon = "";
			StatusItem.IconType icon_type = StatusItem.IconType.Info;
			NotificationType notification_type = NotificationType.Neutral;
			bool allow_multiples = false;
			StatusItemCategory main = Db.Get().StatusItemCategories.Main;
			state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main).Enter(delegate(BlueGrass.StatesInstance smi)
			{
				GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.PlantDeathId), smi.master.transform.GetPosition(), Grid.SceneLayer.FXFront, null, 0).SetActive(true);
				smi.master.Trigger(1623392196, null);
				smi.master.GetComponent<KBatchedAnimController>().StopAndClear();
				UnityEngine.Object.Destroy(smi.master.GetComponent<KBatchedAnimController>());
				smi.Schedule(0.5f, new Action<object>(smi.master.DestroySelf), null);
			});
			this.blocked_from_growing.ToggleStatusItem(Db.Get().MiscStatusItems.RegionIsBlocked, null).EventTransition(GameHashes.EntombedChanged, this.alive, (BlueGrass.StatesInstance smi) => this.alive.ForceUpdateStatus(smi.master.gameObject)).EventTransition(GameHashes.TooColdWarning, this.alive, (BlueGrass.StatesInstance smi) => this.alive.ForceUpdateStatus(smi.master.gameObject)).EventTransition(GameHashes.TooHotWarning, this.alive, (BlueGrass.StatesInstance smi) => this.alive.ForceUpdateStatus(smi.master.gameObject)).TagTransition(GameTags.Uprooted, this.dead, false);
			this.grow.Enter(delegate(BlueGrass.StatesInstance smi)
			{
				if (smi.master.receptacleMonitor.HasReceptacle() && !this.alive.ForceUpdateStatus(smi.master.gameObject))
				{
					smi.GoTo(this.blocked_from_growing);
					return;
				}
				smi.GoTo(this.alive);
			});
			this.alive.InitializeStates(this.masterTarget, this.dead).DefaultState(this.alive.growing).Enter(delegate(BlueGrass.StatesInstance smi)
			{
				smi.master.SetConsumptionRate();
			});
			this.alive.growing.EventTransition(GameHashes.Wilt, this.alive.wilting, (BlueGrass.StatesInstance smi) => smi.master.wiltCondition.IsWilting()).Enter(delegate(BlueGrass.StatesInstance smi)
			{
				smi.master.elementConsumer.EnableConsumption(true);
			}).Exit(delegate(BlueGrass.StatesInstance smi)
			{
				smi.master.elementConsumer.EnableConsumption(false);
			}).EventTransition(GameHashes.Grow, this.alive.fullygrown, (BlueGrass.StatesInstance smi) => smi.master.growing.IsGrown());
			this.alive.fullygrown.EventTransition(GameHashes.Wilt, this.alive.wilting, (BlueGrass.StatesInstance smi) => smi.master.wiltCondition.IsWilting()).EventTransition(GameHashes.HarvestComplete, this.alive.growing, null);
			this.alive.wilting.EventTransition(GameHashes.WiltRecover, this.alive.growing, (BlueGrass.StatesInstance smi) => !smi.master.wiltCondition.IsWilting());
		}

		// Token: 0x04005B26 RID: 23334
		public GameStateMachine<BlueGrass.States, BlueGrass.StatesInstance, BlueGrass, object>.State grow;

		// Token: 0x04005B27 RID: 23335
		public GameStateMachine<BlueGrass.States, BlueGrass.StatesInstance, BlueGrass, object>.State blocked_from_growing;

		// Token: 0x04005B28 RID: 23336
		public BlueGrass.States.AliveStates alive;

		// Token: 0x04005B29 RID: 23337
		public GameStateMachine<BlueGrass.States, BlueGrass.StatesInstance, BlueGrass, object>.State dead;

		// Token: 0x02001705 RID: 5893
		public class AliveStates : GameStateMachine<BlueGrass.States, BlueGrass.StatesInstance, BlueGrass, object>.PlantAliveSubState
		{
			// Token: 0x04005B2A RID: 23338
			public GameStateMachine<BlueGrass.States, BlueGrass.StatesInstance, BlueGrass, object>.State growing;

			// Token: 0x04005B2B RID: 23339
			public GameStateMachine<BlueGrass.States, BlueGrass.StatesInstance, BlueGrass, object>.State fullygrown;

			// Token: 0x04005B2C RID: 23340
			public GameStateMachine<BlueGrass.States, BlueGrass.StatesInstance, BlueGrass, object>.State wilting;
		}
	}
}
