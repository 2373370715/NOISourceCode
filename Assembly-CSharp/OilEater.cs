using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02001726 RID: 5926
public class OilEater : StateMachineComponent<OilEater.StatesInstance>
{
	// Token: 0x060079E7 RID: 31207 RVA: 0x000F4A57 File Offset: 0x000F2C57
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x060079E8 RID: 31208 RVA: 0x003249D4 File Offset: 0x00322BD4
	public void Exhaust(float dt)
	{
		if (base.smi.master.wiltCondition.IsWilting())
		{
			return;
		}
		this.emittedMass += dt * this.emitRate;
		if (this.emittedMass >= this.minEmitMass)
		{
			int gameCell = Grid.PosToCell(base.transform.GetPosition() + this.emitOffset);
			PrimaryElement component = base.GetComponent<PrimaryElement>();
			SimMessages.AddRemoveSubstance(gameCell, SimHashes.CarbonDioxide, CellEventLogger.Instance.ElementEmitted, this.emittedMass, component.Temperature, byte.MaxValue, 0, true, -1);
			this.emittedMass = 0f;
		}
	}

	// Token: 0x04005BBE RID: 23486
	private const SimHashes srcElement = SimHashes.CrudeOil;

	// Token: 0x04005BBF RID: 23487
	private const SimHashes emitElement = SimHashes.CarbonDioxide;

	// Token: 0x04005BC0 RID: 23488
	public float emitRate = 1f;

	// Token: 0x04005BC1 RID: 23489
	public float minEmitMass;

	// Token: 0x04005BC2 RID: 23490
	public Vector3 emitOffset = Vector3.zero;

	// Token: 0x04005BC3 RID: 23491
	[Serialize]
	private float emittedMass;

	// Token: 0x04005BC4 RID: 23492
	[MyCmpReq]
	private WiltCondition wiltCondition;

	// Token: 0x04005BC5 RID: 23493
	[MyCmpReq]
	private Storage storage;

	// Token: 0x04005BC6 RID: 23494
	[MyCmpReq]
	private ReceptacleMonitor receptacleMonitor;

	// Token: 0x02001727 RID: 5927
	public class StatesInstance : GameStateMachine<OilEater.States, OilEater.StatesInstance, OilEater, object>.GameInstance
	{
		// Token: 0x060079EA RID: 31210 RVA: 0x000F4A88 File Offset: 0x000F2C88
		public StatesInstance(OilEater master) : base(master)
		{
		}
	}

	// Token: 0x02001728 RID: 5928
	public class States : GameStateMachine<OilEater.States, OilEater.StatesInstance, OilEater>
	{
		// Token: 0x060079EB RID: 31211 RVA: 0x00324A74 File Offset: 0x00322C74
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.grow;
			GameStateMachine<OilEater.States, OilEater.StatesInstance, OilEater, object>.State state = this.dead;
			string name = CREATURES.STATUSITEMS.DEAD.NAME;
			string tooltip = CREATURES.STATUSITEMS.DEAD.TOOLTIP;
			string icon = "";
			StatusItem.IconType icon_type = StatusItem.IconType.Info;
			NotificationType notification_type = NotificationType.Neutral;
			bool allow_multiples = false;
			StatusItemCategory main = Db.Get().StatusItemCategories.Main;
			state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main).Enter(delegate(OilEater.StatesInstance smi)
			{
				GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.PlantDeathId), smi.master.transform.GetPosition(), Grid.SceneLayer.FXFront, null, 0).SetActive(true);
				smi.master.Trigger(1623392196, null);
				smi.master.GetComponent<KBatchedAnimController>().StopAndClear();
				UnityEngine.Object.Destroy(smi.master.GetComponent<KBatchedAnimController>());
				smi.Schedule(0.5f, delegate(object data)
				{
					GameObject gameObject = (GameObject)data;
					CreatureHelpers.DeselectCreature(gameObject);
					Util.KDestroyGameObject(gameObject);
				}, smi.master.gameObject);
			});
			this.blocked_from_growing.ToggleStatusItem(Db.Get().MiscStatusItems.RegionIsBlocked, null).EventTransition(GameHashes.EntombedChanged, this.alive, (OilEater.StatesInstance smi) => this.alive.ForceUpdateStatus(smi.master.gameObject)).EventTransition(GameHashes.TooColdWarning, this.alive, (OilEater.StatesInstance smi) => this.alive.ForceUpdateStatus(smi.master.gameObject)).EventTransition(GameHashes.TooHotWarning, this.alive, (OilEater.StatesInstance smi) => this.alive.ForceUpdateStatus(smi.master.gameObject)).TagTransition(GameTags.Uprooted, this.dead, false);
			this.grow.Enter(delegate(OilEater.StatesInstance smi)
			{
				if (smi.master.receptacleMonitor.HasReceptacle() && !this.alive.ForceUpdateStatus(smi.master.gameObject))
				{
					smi.GoTo(this.blocked_from_growing);
				}
			}).PlayAnim("grow_seed", KAnim.PlayMode.Once).EventTransition(GameHashes.AnimQueueComplete, this.alive, null);
			this.alive.InitializeStates(this.masterTarget, this.dead).DefaultState(this.alive.mature).Update("Alive", delegate(OilEater.StatesInstance smi, float dt)
			{
				smi.master.Exhaust(dt);
			}, UpdateRate.SIM_200ms, false);
			this.alive.mature.EventTransition(GameHashes.Wilt, this.alive.wilting, (OilEater.StatesInstance smi) => smi.master.wiltCondition.IsWilting()).PlayAnim("idle", KAnim.PlayMode.Loop);
			this.alive.wilting.PlayAnim("wilt1").EventTransition(GameHashes.WiltRecover, this.alive.mature, (OilEater.StatesInstance smi) => !smi.master.wiltCondition.IsWilting());
		}

		// Token: 0x04005BC7 RID: 23495
		public GameStateMachine<OilEater.States, OilEater.StatesInstance, OilEater, object>.State grow;

		// Token: 0x04005BC8 RID: 23496
		public GameStateMachine<OilEater.States, OilEater.StatesInstance, OilEater, object>.State blocked_from_growing;

		// Token: 0x04005BC9 RID: 23497
		public OilEater.States.AliveStates alive;

		// Token: 0x04005BCA RID: 23498
		public GameStateMachine<OilEater.States, OilEater.StatesInstance, OilEater, object>.State dead;

		// Token: 0x02001729 RID: 5929
		public class AliveStates : GameStateMachine<OilEater.States, OilEater.StatesInstance, OilEater, object>.PlantAliveSubState
		{
			// Token: 0x04005BCB RID: 23499
			public GameStateMachine<OilEater.States, OilEater.StatesInstance, OilEater, object>.State mature;

			// Token: 0x04005BCC RID: 23500
			public GameStateMachine<OilEater.States, OilEater.StatesInstance, OilEater, object>.State wilting;
		}
	}
}
