using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x0200174A RID: 5962
public class SapTree : GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>
{
	// Token: 0x06007A97 RID: 31383 RVA: 0x0032664C File Offset: 0x0032484C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.alive;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State state = this.dead;
		string name = CREATURES.STATUSITEMS.DEAD.NAME;
		string tooltip = CREATURES.STATUSITEMS.DEAD.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main).ToggleTag(GameTags.PreventEmittingDisease).Enter(delegate(SapTree.StatesInstance smi)
		{
			GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.PlantDeathId), smi.master.transform.GetPosition(), Grid.SceneLayer.FXFront, null, 0).SetActive(true);
			smi.master.Trigger(1623392196, null);
			smi.master.GetComponent<KBatchedAnimController>().StopAndClear();
		});
		this.alive.InitializeStates(this.masterTarget, this.dead).DefaultState(this.alive.normal);
		this.alive.normal.DefaultState(this.alive.normal.idle).EventTransition(GameHashes.Wilt, this.alive.wilting, (SapTree.StatesInstance smi) => smi.wiltCondition.IsWilting()).Update(delegate(SapTree.StatesInstance smi, float dt)
		{
			smi.CheckForFood();
		}, UpdateRate.SIM_1000ms, false);
		GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State state2 = this.alive.normal.idle.PlayAnim("idle", KAnim.PlayMode.Loop);
		string name2 = CREATURES.STATUSITEMS.IDLE.NAME;
		string tooltip2 = CREATURES.STATUSITEMS.IDLE.TOOLTIP;
		string icon2 = "";
		StatusItem.IconType icon_type2 = StatusItem.IconType.Info;
		NotificationType notification_type2 = NotificationType.Neutral;
		bool allow_multiples2 = false;
		main = Db.Get().StatusItemCategories.Main;
		state2.ToggleStatusItem(name2, tooltip2, icon2, icon_type2, notification_type2, allow_multiples2, default(HashedString), 129022, null, null, main).ParamTransition<bool>(this.hasNearbyEnemy, this.alive.normal.attacking_pre, GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.IsTrue).ParamTransition<float>(this.storedSap, this.alive.normal.oozing, (SapTree.StatesInstance smi, float p) => p >= smi.def.stomachSize).ParamTransition<GameObject>(this.foodItem, this.alive.normal.eating, GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.IsNotNull);
		this.alive.normal.eating.PlayAnim("eat_pre", KAnim.PlayMode.Once).QueueAnim("eat_loop", true, null).Update(delegate(SapTree.StatesInstance smi, float dt)
		{
			smi.EatFoodItem(dt);
		}, UpdateRate.SIM_1000ms, false).ParamTransition<GameObject>(this.foodItem, this.alive.normal.eating_pst, GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.IsNull).ParamTransition<float>(this.storedSap, this.alive.normal.eating_pst, (SapTree.StatesInstance smi, float p) => p >= smi.def.stomachSize);
		this.alive.normal.eating_pst.PlayAnim("eat_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.alive.normal.idle);
		this.alive.normal.oozing.PlayAnim("ooze_pre", KAnim.PlayMode.Once).QueueAnim("ooze_loop", true, null).Update(delegate(SapTree.StatesInstance smi, float dt)
		{
			smi.Ooze(dt);
		}, UpdateRate.SIM_200ms, false).ParamTransition<float>(this.storedSap, this.alive.normal.oozing_pst, (SapTree.StatesInstance smi, float p) => p <= 0f).ParamTransition<bool>(this.hasNearbyEnemy, this.alive.normal.oozing_pst, GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.IsTrue);
		this.alive.normal.oozing_pst.PlayAnim("ooze_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.alive.normal.idle);
		this.alive.normal.attacking_pre.PlayAnim("attacking_pre", KAnim.PlayMode.Once).OnAnimQueueComplete(this.alive.normal.attacking);
		this.alive.normal.attacking.PlayAnim("attacking_loop", KAnim.PlayMode.Once).Enter(delegate(SapTree.StatesInstance smi)
		{
			smi.DoAttack();
		}).OnAnimQueueComplete(this.alive.normal.attacking_cooldown);
		this.alive.normal.attacking_cooldown.PlayAnim("attacking_pst", KAnim.PlayMode.Once).QueueAnim("attack_cooldown", true, null).ParamTransition<bool>(this.hasNearbyEnemy, this.alive.normal.attacking_done, GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.IsFalse).ScheduleGoTo((SapTree.StatesInstance smi) => smi.def.attackCooldown, this.alive.normal.attacking);
		this.alive.normal.attacking_done.PlayAnim("attack_to_idle", KAnim.PlayMode.Once).OnAnimQueueComplete(this.alive.normal.idle);
		this.alive.wilting.PlayAnim("withered", KAnim.PlayMode.Loop).EventTransition(GameHashes.WiltRecover, this.alive.normal, null).ToggleTag(GameTags.PreventEmittingDisease);
	}

	// Token: 0x04005C28 RID: 23592
	public SapTree.AliveStates alive;

	// Token: 0x04005C29 RID: 23593
	public GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State dead;

	// Token: 0x04005C2A RID: 23594
	private StateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.TargetParameter foodItem;

	// Token: 0x04005C2B RID: 23595
	private StateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.BoolParameter hasNearbyEnemy;

	// Token: 0x04005C2C RID: 23596
	private StateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.FloatParameter storedSap;

	// Token: 0x0200174B RID: 5963
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04005C2D RID: 23597
		public Vector2I foodSenseArea;

		// Token: 0x04005C2E RID: 23598
		public float massEatRate;

		// Token: 0x04005C2F RID: 23599
		public float kcalorieToKGConversionRatio;

		// Token: 0x04005C30 RID: 23600
		public float stomachSize;

		// Token: 0x04005C31 RID: 23601
		public float oozeRate;

		// Token: 0x04005C32 RID: 23602
		public List<Vector3> oozeOffsets;

		// Token: 0x04005C33 RID: 23603
		public Vector2I attackSenseArea;

		// Token: 0x04005C34 RID: 23604
		public float attackCooldown;
	}

	// Token: 0x0200174C RID: 5964
	public class AliveStates : GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.PlantAliveSubState
	{
		// Token: 0x04005C35 RID: 23605
		public SapTree.NormalStates normal;

		// Token: 0x04005C36 RID: 23606
		public SapTree.WiltingState wilting;
	}

	// Token: 0x0200174D RID: 5965
	public class NormalStates : GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State
	{
		// Token: 0x04005C37 RID: 23607
		public GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State idle;

		// Token: 0x04005C38 RID: 23608
		public GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State eating;

		// Token: 0x04005C39 RID: 23609
		public GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State eating_pst;

		// Token: 0x04005C3A RID: 23610
		public GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State oozing;

		// Token: 0x04005C3B RID: 23611
		public GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State oozing_pst;

		// Token: 0x04005C3C RID: 23612
		public GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State attacking_pre;

		// Token: 0x04005C3D RID: 23613
		public GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State attacking;

		// Token: 0x04005C3E RID: 23614
		public GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State attacking_cooldown;

		// Token: 0x04005C3F RID: 23615
		public GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State attacking_done;
	}

	// Token: 0x0200174E RID: 5966
	public class WiltingState : GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State
	{
		// Token: 0x04005C40 RID: 23616
		public GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State wilting_pre;

		// Token: 0x04005C41 RID: 23617
		public GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State wilting;

		// Token: 0x04005C42 RID: 23618
		public GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State wilting_pst;
	}

	// Token: 0x0200174F RID: 5967
	public class StatesInstance : GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.GameInstance
	{
		// Token: 0x06007A9D RID: 31389 RVA: 0x00326B78 File Offset: 0x00324D78
		public StatesInstance(IStateMachineTarget master, SapTree.Def def) : base(master, def)
		{
			Vector2I vector2I = Grid.PosToXY(base.gameObject.transform.GetPosition());
			Vector2I vector2I2 = new Vector2I(vector2I.x - def.attackSenseArea.x / 2, vector2I.y);
			this.attackExtents = new Extents(vector2I2.x, vector2I2.y, def.attackSenseArea.x, def.attackSenseArea.y);
			this.partitionerEntry = GameScenePartitioner.Instance.Add("SapTreeAttacker", this, this.attackExtents, GameScenePartitioner.Instance.objectLayers[0], new Action<object>(this.OnMinionChanged));
			Vector2I vector2I3 = new Vector2I(vector2I.x - def.foodSenseArea.x / 2, vector2I.y);
			this.feedExtents = new Extents(vector2I3.x, vector2I3.y, def.foodSenseArea.x, def.foodSenseArea.y);
		}

		// Token: 0x06007A9E RID: 31390 RVA: 0x000F5372 File Offset: 0x000F3572
		protected override void OnCleanUp()
		{
			GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		}

		// Token: 0x06007A9F RID: 31391 RVA: 0x00326C74 File Offset: 0x00324E74
		public void EatFoodItem(float dt)
		{
			Pickupable pickupable = base.sm.foodItem.Get(this).GetComponent<Pickupable>().Take(base.def.massEatRate * dt);
			if (pickupable != null)
			{
				float mass = pickupable.GetComponent<Edible>().Calories * 0.001f * base.def.kcalorieToKGConversionRatio;
				Util.KDestroyGameObject(pickupable.gameObject);
				PrimaryElement component = base.GetComponent<PrimaryElement>();
				this.storage.AddLiquid(SimHashes.Resin, mass, component.Temperature, byte.MaxValue, 0, true, false);
				base.sm.storedSap.Set(this.storage.GetMassAvailable(SimHashes.Resin.CreateTag()), this, false);
			}
		}

		// Token: 0x06007AA0 RID: 31392 RVA: 0x00326D2C File Offset: 0x00324F2C
		public void Ooze(float dt)
		{
			float num = Mathf.Min(base.sm.storedSap.Get(this), dt * base.def.oozeRate);
			if (num <= 0f)
			{
				return;
			}
			int index = Mathf.FloorToInt(GameClock.Instance.GetTime() % (float)base.def.oozeOffsets.Count);
			this.storage.DropSome(SimHashes.Resin.CreateTag(), num, false, true, base.def.oozeOffsets[index], true, false);
			base.sm.storedSap.Set(this.storage.GetMassAvailable(SimHashes.Resin.CreateTag()), this, false);
		}

		// Token: 0x06007AA1 RID: 31393 RVA: 0x00326DDC File Offset: 0x00324FDC
		public void CheckForFood()
		{
			ListPool<ScenePartitionerEntry, SapTree>.PooledList pooledList = ListPool<ScenePartitionerEntry, SapTree>.Allocate();
			GameScenePartitioner.Instance.GatherEntries(this.feedExtents, GameScenePartitioner.Instance.pickupablesLayer, pooledList);
			foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList)
			{
				Pickupable pickupable = scenePartitionerEntry.obj as Pickupable;
				if (pickupable.GetComponent<Edible>() != null)
				{
					base.sm.foodItem.Set(pickupable.gameObject, this, false);
					pooledList.Recycle();
					return;
				}
			}
			base.sm.foodItem.Set(null, this);
			pooledList.Recycle();
		}

		// Token: 0x06007AA2 RID: 31394 RVA: 0x00326E98 File Offset: 0x00325098
		public bool DoAttack()
		{
			int num = this.weapon.AttackArea(base.transform.GetPosition());
			base.sm.hasNearbyEnemy.Set(num > 0, this, false);
			return true;
		}

		// Token: 0x06007AA3 RID: 31395 RVA: 0x000F5384 File Offset: 0x000F3584
		private void OnMinionChanged(object obj)
		{
			if (obj as GameObject != null)
			{
				base.sm.hasNearbyEnemy.Set(true, this, false);
			}
		}

		// Token: 0x04005C43 RID: 23619
		[MyCmpReq]
		public WiltCondition wiltCondition;

		// Token: 0x04005C44 RID: 23620
		[MyCmpReq]
		public EntombVulnerable entombVulnerable;

		// Token: 0x04005C45 RID: 23621
		[MyCmpReq]
		private Storage storage;

		// Token: 0x04005C46 RID: 23622
		[MyCmpReq]
		private Weapon weapon;

		// Token: 0x04005C47 RID: 23623
		private HandleVector<int>.Handle partitionerEntry;

		// Token: 0x04005C48 RID: 23624
		private Extents feedExtents;

		// Token: 0x04005C49 RID: 23625
		private Extents attackExtents;
	}
}
