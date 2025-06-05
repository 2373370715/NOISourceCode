using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200172B RID: 5931
public class Oxyfern : StateMachineComponent<Oxyfern.StatesInstance>
{
	// Token: 0x060079F9 RID: 31225 RVA: 0x000F42AD File Offset: 0x000F24AD
	protected void DestroySelf(object callbackParam)
	{
		CreatureHelpers.DeselectCreature(base.gameObject);
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x060079FA RID: 31226 RVA: 0x000F4B45 File Offset: 0x000F2D45
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x060079FB RID: 31227 RVA: 0x000F4B58 File Offset: 0x000F2D58
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (Tutorial.Instance.oxygenGenerators.Contains(base.gameObject))
		{
			Tutorial.Instance.oxygenGenerators.Remove(base.gameObject);
		}
	}

	// Token: 0x060079FC RID: 31228 RVA: 0x000F4B8D File Offset: 0x000F2D8D
	protected override void OnPrefabInit()
	{
		base.Subscribe<Oxyfern>(1309017699, Oxyfern.OnReplantedDelegate);
		base.OnPrefabInit();
	}

	// Token: 0x060079FD RID: 31229 RVA: 0x000F4BA6 File Offset: 0x000F2DA6
	private void OnReplanted(object data = null)
	{
		this.SetConsumptionRate();
		if (this.receptacleMonitor.Replanted)
		{
			Tutorial.Instance.oxygenGenerators.Add(base.gameObject);
		}
	}

	// Token: 0x060079FE RID: 31230 RVA: 0x000F4BD0 File Offset: 0x000F2DD0
	public void SetConsumptionRate()
	{
		if (this.receptacleMonitor.Replanted)
		{
			this.elementConsumer.consumptionRate = 0.00062500004f;
			return;
		}
		this.elementConsumer.consumptionRate = 0.00015625001f;
	}

	// Token: 0x04005BD3 RID: 23507
	[MyCmpReq]
	private WiltCondition wiltCondition;

	// Token: 0x04005BD4 RID: 23508
	[MyCmpReq]
	private ElementConsumer elementConsumer;

	// Token: 0x04005BD5 RID: 23509
	[MyCmpReq]
	private ElementConverter elementConverter;

	// Token: 0x04005BD6 RID: 23510
	[MyCmpReq]
	private ReceptacleMonitor receptacleMonitor;

	// Token: 0x04005BD7 RID: 23511
	private static readonly EventSystem.IntraObjectHandler<Oxyfern> OnReplantedDelegate = new EventSystem.IntraObjectHandler<Oxyfern>(delegate(Oxyfern component, object data)
	{
		component.OnReplanted(data);
	});

	// Token: 0x0200172C RID: 5932
	public class StatesInstance : GameStateMachine<Oxyfern.States, Oxyfern.StatesInstance, Oxyfern, object>.GameInstance
	{
		// Token: 0x06007A01 RID: 31233 RVA: 0x000F4C24 File Offset: 0x000F2E24
		public StatesInstance(Oxyfern master) : base(master)
		{
		}
	}

	// Token: 0x0200172D RID: 5933
	public class States : GameStateMachine<Oxyfern.States, Oxyfern.StatesInstance, Oxyfern>
	{
		// Token: 0x06007A02 RID: 31234 RVA: 0x00324D38 File Offset: 0x00322F38
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			default_state = this.grow;
			GameStateMachine<Oxyfern.States, Oxyfern.StatesInstance, Oxyfern, object>.State state = this.dead;
			string name = CREATURES.STATUSITEMS.DEAD.NAME;
			string tooltip = CREATURES.STATUSITEMS.DEAD.TOOLTIP;
			string icon = "";
			StatusItem.IconType icon_type = StatusItem.IconType.Info;
			NotificationType notification_type = NotificationType.Neutral;
			bool allow_multiples = false;
			StatusItemCategory main = Db.Get().StatusItemCategories.Main;
			state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main).Enter(delegate(Oxyfern.StatesInstance smi)
			{
				GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.PlantDeathId), smi.master.transform.GetPosition(), Grid.SceneLayer.FXFront, null, 0).SetActive(true);
				smi.master.Trigger(1623392196, null);
				smi.master.GetComponent<KBatchedAnimController>().StopAndClear();
				UnityEngine.Object.Destroy(smi.master.GetComponent<KBatchedAnimController>());
				smi.Schedule(0.5f, new Action<object>(smi.master.DestroySelf), null);
			});
			this.blocked_from_growing.ToggleStatusItem(Db.Get().MiscStatusItems.RegionIsBlocked, null).EventTransition(GameHashes.EntombedChanged, this.alive, (Oxyfern.StatesInstance smi) => this.alive.ForceUpdateStatus(smi.master.gameObject)).EventTransition(GameHashes.TooColdWarning, this.alive, (Oxyfern.StatesInstance smi) => this.alive.ForceUpdateStatus(smi.master.gameObject)).EventTransition(GameHashes.TooHotWarning, this.alive, (Oxyfern.StatesInstance smi) => this.alive.ForceUpdateStatus(smi.master.gameObject)).TagTransition(GameTags.Uprooted, this.dead, false);
			this.grow.Enter(delegate(Oxyfern.StatesInstance smi)
			{
				if (smi.master.receptacleMonitor.HasReceptacle() && !this.alive.ForceUpdateStatus(smi.master.gameObject))
				{
					smi.GoTo(this.blocked_from_growing);
				}
			}).PlayAnim("grow_pst", KAnim.PlayMode.Once).EventTransition(GameHashes.AnimQueueComplete, this.alive, null);
			this.alive.InitializeStates(this.masterTarget, this.dead).DefaultState(this.alive.mature);
			this.alive.mature.EventTransition(GameHashes.Wilt, this.alive.wilting, (Oxyfern.StatesInstance smi) => smi.master.wiltCondition.IsWilting()).PlayAnim("idle_full", KAnim.PlayMode.Loop).Enter(delegate(Oxyfern.StatesInstance smi)
			{
				smi.master.elementConsumer.EnableConsumption(true);
			}).Exit(delegate(Oxyfern.StatesInstance smi)
			{
				smi.master.elementConsumer.EnableConsumption(false);
			});
			this.alive.wilting.PlayAnim("wilt3").EventTransition(GameHashes.WiltRecover, this.alive.mature, (Oxyfern.StatesInstance smi) => !smi.master.wiltCondition.IsWilting());
		}

		// Token: 0x04005BD8 RID: 23512
		public GameStateMachine<Oxyfern.States, Oxyfern.StatesInstance, Oxyfern, object>.State grow;

		// Token: 0x04005BD9 RID: 23513
		public GameStateMachine<Oxyfern.States, Oxyfern.StatesInstance, Oxyfern, object>.State blocked_from_growing;

		// Token: 0x04005BDA RID: 23514
		public Oxyfern.States.AliveStates alive;

		// Token: 0x04005BDB RID: 23515
		public GameStateMachine<Oxyfern.States, Oxyfern.StatesInstance, Oxyfern, object>.State dead;

		// Token: 0x0200172E RID: 5934
		public class AliveStates : GameStateMachine<Oxyfern.States, Oxyfern.StatesInstance, Oxyfern, object>.PlantAliveSubState
		{
			// Token: 0x04005BDC RID: 23516
			public GameStateMachine<Oxyfern.States, Oxyfern.StatesInstance, Oxyfern, object>.State mature;

			// Token: 0x04005BDD RID: 23517
			public GameStateMachine<Oxyfern.States, Oxyfern.StatesInstance, Oxyfern, object>.State wilting;
		}
	}
}
