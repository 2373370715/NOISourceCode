using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000EEF RID: 3823
public class MilkSeparator : GameStateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>
{
	// Token: 0x06004C92 RID: 19602 RVA: 0x00270780 File Offset: 0x0026E980
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.noOperational;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.root.EventHandler(GameHashes.OnStorageChange, new StateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State.Callback(MilkSeparator.RefreshMeters));
		this.noOperational.TagTransition(GameTags.Operational, this.operational, false).PlayAnim("off");
		this.operational.TagTransition(GameTags.Operational, this.noOperational, true).PlayAnim("on").DefaultState(this.operational.idle);
		this.operational.idle.EventTransition(GameHashes.OnStorageChange, this.operational.working.pre, new StateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.Transition.ConditionCallback(MilkSeparator.CanBeginSeparate)).EnterTransition(this.operational.full, new StateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.Transition.ConditionCallback(MilkSeparator.RequiresEmptying));
		this.operational.working.pre.QueueAnim("separating_pre", false, null).OnAnimQueueComplete(this.operational.working.work);
		this.operational.working.work.Enter(new StateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State.Callback(MilkSeparator.BeginSeparation)).PlayAnim("separating_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.OnStorageChange, this.operational.working.post, new StateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.Transition.ConditionCallback(MilkSeparator.CanNOTKeepSeparating)).Exit(new StateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State.Callback(MilkSeparator.EndSeparation));
		this.operational.working.post.QueueAnim("separating_pst", false, null).OnAnimQueueComplete(this.operational.idle);
		this.operational.full.PlayAnim("ready").ToggleRecurringChore(new Func<MilkSeparator.Instance, Chore>(MilkSeparator.CreateEmptyChore), null).WorkableCompleteTransition((MilkSeparator.Instance smi) => smi.workable, this.operational.emptyComplete).ToggleStatusItem(Db.Get().BuildingStatusItems.MilkSeparatorNeedsEmptying, null);
		this.operational.emptyComplete.Enter(new StateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State.Callback(MilkSeparator.DropMilkFat)).ScheduleActionNextFrame("AfterMilkFatDrop", delegate(MilkSeparator.Instance smi)
		{
			smi.GoTo(this.operational.idle);
		});
	}

	// Token: 0x06004C93 RID: 19603 RVA: 0x000D5EC9 File Offset: 0x000D40C9
	public static void BeginSeparation(MilkSeparator.Instance smi)
	{
		smi.operational.SetActive(true, false);
	}

	// Token: 0x06004C94 RID: 19604 RVA: 0x000D5ED8 File Offset: 0x000D40D8
	public static void EndSeparation(MilkSeparator.Instance smi)
	{
		smi.operational.SetActive(false, false);
	}

	// Token: 0x06004C95 RID: 19605 RVA: 0x000D5EE7 File Offset: 0x000D40E7
	public static bool CanBeginSeparate(MilkSeparator.Instance smi)
	{
		return !smi.MilkFatLimitReached && smi.elementConverter.HasEnoughMassToStartConverting(false);
	}

	// Token: 0x06004C96 RID: 19606 RVA: 0x000D5EFF File Offset: 0x000D40FF
	public static bool CanKeepSeparating(MilkSeparator.Instance smi)
	{
		return !smi.MilkFatLimitReached && smi.elementConverter.CanConvertAtAll();
	}

	// Token: 0x06004C97 RID: 19607 RVA: 0x000D5F16 File Offset: 0x000D4116
	public static bool CanNOTKeepSeparating(MilkSeparator.Instance smi)
	{
		return !MilkSeparator.CanKeepSeparating(smi);
	}

	// Token: 0x06004C98 RID: 19608 RVA: 0x000D5F21 File Offset: 0x000D4121
	public static bool RequiresEmptying(MilkSeparator.Instance smi)
	{
		return smi.MilkFatLimitReached;
	}

	// Token: 0x06004C99 RID: 19609 RVA: 0x000D5F29 File Offset: 0x000D4129
	public static bool ThereIsCapacityForMilkFat(MilkSeparator.Instance smi)
	{
		return !smi.MilkFatLimitReached;
	}

	// Token: 0x06004C9A RID: 19610 RVA: 0x000D5F34 File Offset: 0x000D4134
	public static void DropMilkFat(MilkSeparator.Instance smi)
	{
		smi.DropMilkFat();
	}

	// Token: 0x06004C9B RID: 19611 RVA: 0x000D5F3C File Offset: 0x000D413C
	public static void RefreshMeters(MilkSeparator.Instance smi)
	{
		smi.RefreshMeters();
	}

	// Token: 0x06004C9C RID: 19612 RVA: 0x002709C0 File Offset: 0x0026EBC0
	private static Chore CreateEmptyChore(MilkSeparator.Instance smi)
	{
		WorkChore<EmptyMilkSeparatorWorkable> workChore = new WorkChore<EmptyMilkSeparatorWorkable>(Db.Get().ChoreTypes.EmptyStorage, smi.workable, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		workChore.AddPrecondition(ChorePreconditions.instance.IsNotARobot, null);
		return workChore;
	}

	// Token: 0x04003598 RID: 13720
	public const string WORK_PRE_ANIM_NAME = "separating_pre";

	// Token: 0x04003599 RID: 13721
	public const string WORK_ANIM_NAME = "separating_loop";

	// Token: 0x0400359A RID: 13722
	public const string WORK_POST_ANIM_NAME = "separating_pst";

	// Token: 0x0400359B RID: 13723
	public GameStateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State noOperational;

	// Token: 0x0400359C RID: 13724
	public MilkSeparator.OperationalStates operational;

	// Token: 0x02000EF0 RID: 3824
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x06004C9F RID: 19615 RVA: 0x000D5F5F File Offset: 0x000D415F
		public Def()
		{
			this.MILK_FAT_TAG = ElementLoader.FindElementByHash(SimHashes.MilkFat).tag;
			this.MILK_TAG = ElementLoader.FindElementByHash(SimHashes.Milk).tag;
		}

		// Token: 0x0400359D RID: 13725
		public float MILK_FAT_CAPACITY = 100f;

		// Token: 0x0400359E RID: 13726
		public Tag MILK_TAG;

		// Token: 0x0400359F RID: 13727
		public Tag MILK_FAT_TAG;
	}

	// Token: 0x02000EF1 RID: 3825
	public class WorkingStates : GameStateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State
	{
		// Token: 0x040035A0 RID: 13728
		public GameStateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State pre;

		// Token: 0x040035A1 RID: 13729
		public GameStateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State work;

		// Token: 0x040035A2 RID: 13730
		public GameStateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State post;
	}

	// Token: 0x02000EF2 RID: 3826
	public class OperationalStates : GameStateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State
	{
		// Token: 0x040035A3 RID: 13731
		public GameStateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State idle;

		// Token: 0x040035A4 RID: 13732
		public MilkSeparator.WorkingStates working;

		// Token: 0x040035A5 RID: 13733
		public GameStateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State full;

		// Token: 0x040035A6 RID: 13734
		public GameStateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State emptyComplete;
	}

	// Token: 0x02000EF3 RID: 3827
	public new class Instance : GameStateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.GameInstance
	{
		// Token: 0x17000439 RID: 1081
		// (get) Token: 0x06004CA2 RID: 19618 RVA: 0x000D5FA4 File Offset: 0x000D41A4
		public float MilkFatStored
		{
			get
			{
				return this.storage.GetAmountAvailable(base.def.MILK_FAT_TAG);
			}
		}

		// Token: 0x1700043A RID: 1082
		// (get) Token: 0x06004CA3 RID: 19619 RVA: 0x000D5FBC File Offset: 0x000D41BC
		public float MilkFatStoragePercentage
		{
			get
			{
				return Mathf.Clamp(this.MilkFatStored / base.def.MILK_FAT_CAPACITY, 0f, 1f);
			}
		}

		// Token: 0x1700043B RID: 1083
		// (get) Token: 0x06004CA4 RID: 19620 RVA: 0x000D5FDF File Offset: 0x000D41DF
		public bool MilkFatLimitReached
		{
			get
			{
				return this.MilkFatStored >= base.def.MILK_FAT_CAPACITY;
			}
		}

		// Token: 0x06004CA5 RID: 19621 RVA: 0x00270A0C File Offset: 0x0026EC0C
		public Instance(IStateMachineTarget master, MilkSeparator.Def def) : base(master, def)
		{
			KAnimControllerBase component = base.GetComponent<KBatchedAnimController>();
			this.fatMeter = new MeterController(component, "meter_target_1", "meter_fat", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
			{
				"meter_target_1"
			});
		}

		// Token: 0x06004CA6 RID: 19622 RVA: 0x000D5FF7 File Offset: 0x000D41F7
		public override void StartSM()
		{
			base.StartSM();
			this.workable.OnWork_PST_Begins = new System.Action(this.Play_Empty_MeterAnimation);
			this.RefreshMeters();
		}

		// Token: 0x06004CA7 RID: 19623 RVA: 0x000D601C File Offset: 0x000D421C
		private void Play_Empty_MeterAnimation()
		{
			this.fatMeter.SetPositionPercent(0f);
			this.fatMeter.meterController.Play("meter_fat_empty", KAnim.PlayMode.Once, 1f, 0f);
		}

		// Token: 0x06004CA8 RID: 19624 RVA: 0x00270A50 File Offset: 0x0026EC50
		public void DropMilkFat()
		{
			List<GameObject> list = new List<GameObject>();
			this.storage.Drop(base.def.MILK_FAT_TAG, list);
			Vector3 dropSpawnLocation = this.GetDropSpawnLocation();
			foreach (GameObject gameObject in list)
			{
				gameObject.transform.position = dropSpawnLocation;
			}
		}

		// Token: 0x06004CA9 RID: 19625 RVA: 0x00270AC8 File Offset: 0x0026ECC8
		private Vector3 GetDropSpawnLocation()
		{
			bool flag;
			Vector3 vector = base.GetComponent<KBatchedAnimController>().GetSymbolTransform(new HashedString("milkfat"), out flag).GetColumn(3);
			vector.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
			int num = Grid.PosToCell(vector);
			if (Grid.IsValidCell(num) && !Grid.Solid[num])
			{
				return vector;
			}
			return base.transform.GetPosition();
		}

		// Token: 0x06004CAA RID: 19626 RVA: 0x00270B34 File Offset: 0x0026ED34
		public void RefreshMeters()
		{
			if (this.fatMeter.meterController.currentAnim != "meter_fat")
			{
				this.fatMeter.meterController.Play("meter_fat", KAnim.PlayMode.Paused, 1f, 0f);
			}
			this.fatMeter.SetPositionPercent(this.MilkFatStoragePercentage);
		}

		// Token: 0x040035A7 RID: 13735
		[MyCmpGet]
		public EmptyMilkSeparatorWorkable workable;

		// Token: 0x040035A8 RID: 13736
		[MyCmpGet]
		public Operational operational;

		// Token: 0x040035A9 RID: 13737
		[MyCmpGet]
		public ElementConverter elementConverter;

		// Token: 0x040035AA RID: 13738
		[MyCmpGet]
		private Storage storage;

		// Token: 0x040035AB RID: 13739
		private MeterController fatMeter;
	}
}
