using System;
using UnityEngine;

// Token: 0x020016FD RID: 5885
public class BasicForagePlantPlanted : StateMachineComponent<BasicForagePlantPlanted.StatesInstance>
{
	// Token: 0x06007946 RID: 31046 RVA: 0x000F429A File Offset: 0x000F249A
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x06007947 RID: 31047 RVA: 0x000F42AD File Offset: 0x000F24AD
	protected void DestroySelf(object callbackParam)
	{
		CreatureHelpers.DeselectCreature(base.gameObject);
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x04005B15 RID: 23317
	[MyCmpReq]
	private Harvestable harvestable;

	// Token: 0x04005B16 RID: 23318
	[MyCmpReq]
	private SeedProducer seedProducer;

	// Token: 0x04005B17 RID: 23319
	[MyCmpReq]
	private KBatchedAnimController animController;

	// Token: 0x020016FE RID: 5886
	public class StatesInstance : GameStateMachine<BasicForagePlantPlanted.States, BasicForagePlantPlanted.StatesInstance, BasicForagePlantPlanted, object>.GameInstance
	{
		// Token: 0x06007949 RID: 31049 RVA: 0x000F42CD File Offset: 0x000F24CD
		public StatesInstance(BasicForagePlantPlanted smi) : base(smi)
		{
		}
	}

	// Token: 0x020016FF RID: 5887
	public class States : GameStateMachine<BasicForagePlantPlanted.States, BasicForagePlantPlanted.StatesInstance, BasicForagePlantPlanted>
	{
		// Token: 0x0600794A RID: 31050 RVA: 0x00322B08 File Offset: 0x00320D08
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.seed_grow;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.seed_grow.PlayAnim("idle", KAnim.PlayMode.Once).EventTransition(GameHashes.AnimQueueComplete, this.alive.idle, null);
			this.alive.InitializeStates(this.masterTarget, this.dead);
			this.alive.idle.PlayAnim("idle").EventTransition(GameHashes.Harvest, this.alive.harvest, null).Enter(delegate(BasicForagePlantPlanted.StatesInstance smi)
			{
				smi.master.harvestable.SetCanBeHarvested(true);
			});
			this.alive.harvest.Enter(delegate(BasicForagePlantPlanted.StatesInstance smi)
			{
				smi.master.seedProducer.DropSeed(null);
			}).GoTo(this.dead);
			this.dead.Enter(delegate(BasicForagePlantPlanted.StatesInstance smi)
			{
				GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.PlantDeathId), smi.master.transform.GetPosition(), Grid.SceneLayer.FXFront, null, 0).SetActive(true);
				smi.master.Trigger(1623392196, null);
				smi.master.animController.StopAndClear();
				UnityEngine.Object.Destroy(smi.master.animController);
				smi.master.DestroySelf(null);
			});
		}

		// Token: 0x04005B18 RID: 23320
		public GameStateMachine<BasicForagePlantPlanted.States, BasicForagePlantPlanted.StatesInstance, BasicForagePlantPlanted, object>.State seed_grow;

		// Token: 0x04005B19 RID: 23321
		public BasicForagePlantPlanted.States.AliveStates alive;

		// Token: 0x04005B1A RID: 23322
		public GameStateMachine<BasicForagePlantPlanted.States, BasicForagePlantPlanted.StatesInstance, BasicForagePlantPlanted, object>.State dead;

		// Token: 0x02001700 RID: 5888
		public class AliveStates : GameStateMachine<BasicForagePlantPlanted.States, BasicForagePlantPlanted.StatesInstance, BasicForagePlantPlanted, object>.PlantAliveSubState
		{
			// Token: 0x04005B1B RID: 23323
			public GameStateMachine<BasicForagePlantPlanted.States, BasicForagePlantPlanted.StatesInstance, BasicForagePlantPlanted, object>.State idle;

			// Token: 0x04005B1C RID: 23324
			public GameStateMachine<BasicForagePlantPlanted.States, BasicForagePlantPlanted.StatesInstance, BasicForagePlantPlanted, object>.State harvest;
		}
	}
}
