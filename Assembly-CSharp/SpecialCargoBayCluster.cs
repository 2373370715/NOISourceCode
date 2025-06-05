using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020005B5 RID: 1461
public class SpecialCargoBayCluster : GameStateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>
{
	// Token: 0x06001953 RID: 6483 RVA: 0x001AE458 File Offset: 0x001AC658
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.close;
		this.close.DefaultState(this.close.idle);
		this.close.closing.Target(this.Door).PlayAnim("close").OnAnimQueueComplete(this.close.idle).Target(this.masterTarget);
		this.close.idle.Target(this.Door).PlayAnim("close_idle").ParamTransition<bool>(this.IsDoorOpen, this.open.opening, GameStateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.IsTrue).Target(this.masterTarget);
		this.close.cloud.Target(this.Door).PlayAnim("play_cloud").OnAnimQueueComplete(this.close.idle).Target(this.masterTarget);
		this.open.DefaultState(this.close.idle);
		this.open.opening.Target(this.Door).PlayAnim("open").OnAnimQueueComplete(this.open.idle).Target(this.masterTarget);
		this.open.idle.Target(this.Door).PlayAnim("open_idle").Enter(new StateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.State.Callback(SpecialCargoBayCluster.DropInventory)).Enter(new StateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.State.Callback(SpecialCargoBayCluster.CloseDoorAutomatically)).ParamTransition<bool>(this.IsDoorOpen, this.close.closing, GameStateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.IsFalse).Target(this.masterTarget);
	}

	// Token: 0x06001954 RID: 6484 RVA: 0x000B5138 File Offset: 0x000B3338
	public static void CloseDoorAutomatically(SpecialCargoBayCluster.Instance smi)
	{
		smi.CloseDoorAutomatically();
	}

	// Token: 0x06001955 RID: 6485 RVA: 0x000B5140 File Offset: 0x000B3340
	public static void DropInventory(SpecialCargoBayCluster.Instance smi)
	{
		smi.DropInventory();
	}

	// Token: 0x0400106A RID: 4202
	public const string DOOR_METER_TARGET_NAME = "fg_meter_target";

	// Token: 0x0400106B RID: 4203
	public const string TRAPPED_CRITTER_PIVOT_SYMBOL_NAME = "critter";

	// Token: 0x0400106C RID: 4204
	public const string LOOT_SYMBOL_NAME = "loot";

	// Token: 0x0400106D RID: 4205
	public const string DEATH_CLOUD_ANIM_NAME = "play_cloud";

	// Token: 0x0400106E RID: 4206
	private const string OPEN_DOOR_ANIM_NAME = "open";

	// Token: 0x0400106F RID: 4207
	private const string CLOSE_DOOR_ANIM_NAME = "close";

	// Token: 0x04001070 RID: 4208
	private const string OPEN_DOOR_IDLE_ANIM_NAME = "open_idle";

	// Token: 0x04001071 RID: 4209
	private const string CLOSE_DOOR_IDLE_ANIM_NAME = "close_idle";

	// Token: 0x04001072 RID: 4210
	public SpecialCargoBayCluster.OpenStates open;

	// Token: 0x04001073 RID: 4211
	public SpecialCargoBayCluster.CloseStates close;

	// Token: 0x04001074 RID: 4212
	public StateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.BoolParameter IsDoorOpen;

	// Token: 0x04001075 RID: 4213
	public StateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.TargetParameter Door;

	// Token: 0x020005B6 RID: 1462
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04001076 RID: 4214
		public Vector2 trappedOffset = new Vector2(0f, -0.3f);
	}

	// Token: 0x020005B7 RID: 1463
	public class OpenStates : GameStateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.State
	{
		// Token: 0x04001077 RID: 4215
		public GameStateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.State opening;

		// Token: 0x04001078 RID: 4216
		public GameStateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.State idle;
	}

	// Token: 0x020005B8 RID: 1464
	public class CloseStates : GameStateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.State
	{
		// Token: 0x04001079 RID: 4217
		public GameStateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.State closing;

		// Token: 0x0400107A RID: 4218
		public GameStateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.State idle;

		// Token: 0x0400107B RID: 4219
		public GameStateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.State cloud;
	}

	// Token: 0x020005B9 RID: 1465
	public new class Instance : GameStateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.GameInstance
	{
		// Token: 0x0600195A RID: 6490 RVA: 0x000B5175 File Offset: 0x000B3375
		public void PlayDeathCloud()
		{
			if (base.IsInsideState(base.sm.close.idle))
			{
				this.GoTo(base.sm.close.cloud);
			}
		}

		// Token: 0x0600195B RID: 6491 RVA: 0x000B51A5 File Offset: 0x000B33A5
		public void CloseDoor()
		{
			base.sm.IsDoorOpen.Set(false, base.smi, false);
		}

		// Token: 0x0600195C RID: 6492 RVA: 0x000B51C0 File Offset: 0x000B33C0
		public void OpenDoor()
		{
			base.sm.IsDoorOpen.Set(true, base.smi, false);
		}

		// Token: 0x0600195D RID: 6493 RVA: 0x001AE608 File Offset: 0x001AC808
		public Instance(IStateMachineTarget master, SpecialCargoBayCluster.Def def) : base(master, def)
		{
			this.buildingAnimController = base.GetComponent<KBatchedAnimController>();
			this.doorMeter = new MeterController(this.buildingAnimController, "fg_meter_target", "close_idle", Meter.Offset.UserSpecified, Grid.SceneLayer.BuildingFront, Array.Empty<string>());
			this.doorAnimController = this.doorMeter.meterController;
			KBatchedAnimTracker componentInChildren = this.doorAnimController.GetComponentInChildren<KBatchedAnimTracker>();
			componentInChildren.forceAlwaysAlive = true;
			componentInChildren.matchParentOffset = true;
			base.sm.Door.Set(this.doorAnimController.gameObject, base.smi, false);
			Storage[] components = base.gameObject.GetComponents<Storage>();
			this.critterStorage = components[0];
			this.sideProductStorage = components[1];
			base.Subscribe(1655598572, new Action<object>(this.OnLaunchConditionChanged));
		}

		// Token: 0x0600195E RID: 6494 RVA: 0x000B51DB File Offset: 0x000B33DB
		public void CloseDoorAutomatically()
		{
			this.CloseDoor();
		}

		// Token: 0x0600195F RID: 6495 RVA: 0x000B51E3 File Offset: 0x000B33E3
		public override void StartSM()
		{
			base.StartSM();
		}

		// Token: 0x06001960 RID: 6496 RVA: 0x001AE6D0 File Offset: 0x001AC8D0
		private void OnLaunchConditionChanged(object obj)
		{
			if (this.rocketModuleCluster.CraftInterface != null)
			{
				Clustercraft component = this.rocketModuleCluster.CraftInterface.GetComponent<Clustercraft>();
				if (component != null && component.Status == Clustercraft.CraftStatus.Launching)
				{
					this.CloseDoor();
				}
			}
		}

		// Token: 0x06001961 RID: 6497 RVA: 0x001AE71C File Offset: 0x001AC91C
		public void DropInventory()
		{
			List<GameObject> list = new List<GameObject>();
			List<GameObject> list2 = new List<GameObject>();
			foreach (GameObject gameObject in this.critterStorage.items)
			{
				if (gameObject != null)
				{
					Baggable component = gameObject.GetComponent<Baggable>();
					if (component != null)
					{
						component.keepWrangledNextTimeRemovedFromStorage = true;
					}
				}
			}
			Storage storage = this.critterStorage;
			bool vent_gas = false;
			bool dump_liquid = false;
			List<GameObject> collect_dropped_items = list;
			storage.DropAll(vent_gas, dump_liquid, default(Vector3), true, collect_dropped_items);
			Storage storage2 = this.sideProductStorage;
			bool vent_gas2 = false;
			bool dump_liquid2 = false;
			collect_dropped_items = list2;
			storage2.DropAll(vent_gas2, dump_liquid2, default(Vector3), true, collect_dropped_items);
			foreach (GameObject gameObject2 in list)
			{
				KBatchedAnimController component2 = gameObject2.GetComponent<KBatchedAnimController>();
				Vector3 storePositionForCritter = this.GetStorePositionForCritter(gameObject2);
				gameObject2.transform.SetPosition(storePositionForCritter);
				component2.SetSceneLayer(Grid.SceneLayer.Creatures);
				component2.Play("trussed", KAnim.PlayMode.Loop, 1f, 0f);
			}
			foreach (GameObject gameObject3 in list2)
			{
				KBatchedAnimController component3 = gameObject3.GetComponent<KBatchedAnimController>();
				Vector3 storePositionForDrops = this.GetStorePositionForDrops();
				gameObject3.transform.SetPosition(storePositionForDrops);
				component3.SetSceneLayer(Grid.SceneLayer.Ore);
			}
		}

		// Token: 0x06001962 RID: 6498 RVA: 0x001AE8AC File Offset: 0x001ACAAC
		public Vector3 GetCritterPositionOffet(GameObject critter)
		{
			KBatchedAnimController component = critter.GetComponent<KBatchedAnimController>();
			Vector3 zero = Vector3.zero;
			zero.x = base.def.trappedOffset.x - component.Offset.x;
			zero.y = base.def.trappedOffset.y - component.Offset.y;
			return zero;
		}

		// Token: 0x06001963 RID: 6499 RVA: 0x001AE910 File Offset: 0x001ACB10
		public Vector3 GetStorePositionForCritter(GameObject critter)
		{
			Vector3 critterPositionOffet = this.GetCritterPositionOffet(critter);
			bool flag;
			return this.buildingAnimController.GetSymbolTransform("critter", out flag).GetColumn(3) + critterPositionOffet;
		}

		// Token: 0x06001964 RID: 6500 RVA: 0x001AE950 File Offset: 0x001ACB50
		public Vector3 GetStorePositionForDrops()
		{
			bool flag;
			return this.buildingAnimController.GetSymbolTransform("loot", out flag).GetColumn(3);
		}

		// Token: 0x0400107C RID: 4220
		public MeterController doorMeter;

		// Token: 0x0400107D RID: 4221
		private Storage critterStorage;

		// Token: 0x0400107E RID: 4222
		private Storage sideProductStorage;

		// Token: 0x0400107F RID: 4223
		private KBatchedAnimController buildingAnimController;

		// Token: 0x04001080 RID: 4224
		private KBatchedAnimController doorAnimController;

		// Token: 0x04001081 RID: 4225
		[MyCmpGet]
		private RocketModuleCluster rocketModuleCluster;
	}
}
