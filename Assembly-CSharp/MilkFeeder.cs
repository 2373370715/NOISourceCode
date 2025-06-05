using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000EE9 RID: 3817
public class MilkFeeder : GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>
{
	// Token: 0x06004C71 RID: 19569 RVA: 0x00270314 File Offset: 0x0026E514
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		this.root.Enter(delegate(MilkFeeder.Instance smi)
		{
			smi.UpdateStorageMeter();
		}).EventHandler(GameHashes.OnStorageChange, delegate(MilkFeeder.Instance smi)
		{
			smi.UpdateStorageMeter();
		});
		this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.on, (MilkFeeder.Instance smi) => smi.GetComponent<Operational>().IsOperational);
		this.on.DefaultState(this.on.pre).EventTransition(GameHashes.OperationalChanged, this.on.pst, (MilkFeeder.Instance smi) => !smi.GetComponent<Operational>().IsOperational && smi.GetCurrentState() != this.on.pre).EventTransition(GameHashes.OperationalChanged, this.off, (MilkFeeder.Instance smi) => !smi.GetComponent<Operational>().IsOperational && smi.GetCurrentState() == this.on.pre);
		this.on.pre.PlayAnim("working_pre").OnAnimQueueComplete(this.on.working);
		this.on.working.PlayAnim("on").DefaultState(this.on.working.empty);
		this.on.working.empty.PlayAnim("empty").EnterTransition(this.on.working.refilling, (MilkFeeder.Instance smi) => smi.HasEnoughMilkForOneFeeding()).EventHandler(GameHashes.OnStorageChange, delegate(MilkFeeder.Instance smi)
		{
			if (smi.HasEnoughMilkForOneFeeding())
			{
				smi.GoTo(this.on.working.refilling);
			}
		});
		this.on.working.refilling.PlayAnim("fill").OnAnimQueueComplete(this.on.working.full);
		this.on.working.full.PlayAnim("full").Enter(delegate(MilkFeeder.Instance smi)
		{
			this.isReadyToStartFeeding.Set(true, smi, false);
		}).Exit(delegate(MilkFeeder.Instance smi)
		{
			this.isReadyToStartFeeding.Set(false, smi, false);
		}).ParamTransition<DrinkMilkStates.Instance>(this.currentFeedingCritter, this.on.working.emptying, (MilkFeeder.Instance smi, DrinkMilkStates.Instance val) => val != null);
		this.on.working.emptying.EnterTransition(this.on.working.full, delegate(MilkFeeder.Instance smi)
		{
			DrinkMilkMonitor.Instance smi2 = this.currentFeedingCritter.Get(smi).GetSMI<DrinkMilkMonitor.Instance>();
			return smi2 != null && !smi2.def.consumesMilk;
		}).PlayAnim("emptying").OnAnimQueueComplete(this.on.working.empty).Exit(delegate(MilkFeeder.Instance smi)
		{
			smi.StopFeeding();
		});
		this.on.pst.PlayAnim("working_pst").OnAnimQueueComplete(this.off);
	}

	// Token: 0x04003584 RID: 13700
	private GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State off;

	// Token: 0x04003585 RID: 13701
	private MilkFeeder.OnState on;

	// Token: 0x04003586 RID: 13702
	public StateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.BoolParameter isReadyToStartFeeding;

	// Token: 0x04003587 RID: 13703
	public StateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.ObjectParameter<DrinkMilkStates.Instance> currentFeedingCritter;

	// Token: 0x02000EEA RID: 3818
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x06004C79 RID: 19577 RVA: 0x00270638 File Offset: 0x0026E838
		public List<Descriptor> GetDescriptors(GameObject go)
		{
			List<Descriptor> list = new List<Descriptor>();
			go.GetSMI<MilkFeeder.Instance>();
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(CREATURES.MODIFIERS.GOTMILK.NAME, "", Descriptor.DescriptorType.Effect);
			list.Add(item);
			Effect.AddModifierDescriptions(list, "HadMilk", true, "STRINGS.CREATURES.STATS.");
			return list;
		}
	}

	// Token: 0x02000EEB RID: 3819
	public class OnState : GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State
	{
		// Token: 0x04003588 RID: 13704
		public GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State pre;

		// Token: 0x04003589 RID: 13705
		public MilkFeeder.OnState.WorkingState working;

		// Token: 0x0400358A RID: 13706
		public GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State pst;

		// Token: 0x02000EEC RID: 3820
		public class WorkingState : GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State
		{
			// Token: 0x0400358B RID: 13707
			public GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State empty;

			// Token: 0x0400358C RID: 13708
			public GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State refilling;

			// Token: 0x0400358D RID: 13709
			public GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State full;

			// Token: 0x0400358E RID: 13710
			public GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State emptying;
		}
	}

	// Token: 0x02000EED RID: 3821
	public new class Instance : GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.GameInstance
	{
		// Token: 0x06004C7D RID: 19581 RVA: 0x000D5D7F File Offset: 0x000D3F7F
		public Instance(IStateMachineTarget master, MilkFeeder.Def def) : base(master, def)
		{
			this.milkStorage = base.GetComponent<Storage>();
			this.storageMeter = new MeterController(base.smi.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
		}

		// Token: 0x06004C7E RID: 19582 RVA: 0x000D5DBD File Offset: 0x000D3FBD
		public override void StartSM()
		{
			base.StartSM();
			Components.MilkFeeders.Add(base.smi.GetMyWorldId(), this);
		}

		// Token: 0x06004C7F RID: 19583 RVA: 0x000D5DDB File Offset: 0x000D3FDB
		protected override void OnCleanUp()
		{
			base.OnCleanUp();
			Components.MilkFeeders.Remove(base.smi.GetMyWorldId(), this);
		}

		// Token: 0x06004C80 RID: 19584 RVA: 0x000D5DF9 File Offset: 0x000D3FF9
		public void UpdateStorageMeter()
		{
			this.storageMeter.SetPositionPercent(1f - Mathf.Clamp01(this.milkStorage.RemainingCapacity() / this.milkStorage.capacityKg));
		}

		// Token: 0x06004C81 RID: 19585 RVA: 0x000D5E28 File Offset: 0x000D4028
		public bool IsOperational()
		{
			return base.GetComponent<Operational>().IsOperational;
		}

		// Token: 0x06004C82 RID: 19586 RVA: 0x000CF301 File Offset: 0x000CD501
		public bool IsReserved()
		{
			return base.HasTag(GameTags.Creatures.ReservedByCreature);
		}

		// Token: 0x06004C83 RID: 19587 RVA: 0x00270688 File Offset: 0x0026E888
		public void SetReserved(bool isReserved)
		{
			if (isReserved)
			{
				global::Debug.Assert(!base.HasTag(GameTags.Creatures.ReservedByCreature));
				base.GetComponent<KPrefabID>().SetTag(GameTags.Creatures.ReservedByCreature, true);
				return;
			}
			if (base.HasTag(GameTags.Creatures.ReservedByCreature))
			{
				base.GetComponent<KPrefabID>().RemoveTag(GameTags.Creatures.ReservedByCreature);
				return;
			}
			global::Debug.LogWarningFormat(base.smi.gameObject, "Tried to unreserve a MilkFeeder that wasn't reserved", Array.Empty<object>());
		}

		// Token: 0x06004C84 RID: 19588 RVA: 0x000D5E35 File Offset: 0x000D4035
		public bool IsReadyToStartFeeding()
		{
			return this.IsOperational() && base.sm.isReadyToStartFeeding.Get(base.smi);
		}

		// Token: 0x06004C85 RID: 19589 RVA: 0x000D5E57 File Offset: 0x000D4057
		public void RequestToStartFeeding(DrinkMilkStates.Instance feedingCritter)
		{
			base.sm.currentFeedingCritter.Set(feedingCritter, base.smi, false);
		}

		// Token: 0x06004C86 RID: 19590 RVA: 0x002706F8 File Offset: 0x0026E8F8
		public void StopFeeding()
		{
			DrinkMilkStates.Instance instance = base.sm.currentFeedingCritter.Get(base.smi);
			if (instance != null)
			{
				instance.RequestToStopFeeding();
			}
			base.sm.currentFeedingCritter.Set(null, base.smi, false);
		}

		// Token: 0x06004C87 RID: 19591 RVA: 0x000D5E72 File Offset: 0x000D4072
		public bool HasEnoughMilkForOneFeeding()
		{
			return this.milkStorage.GetAmountAvailable(MilkFeederConfig.MILK_TAG) >= 5f;
		}

		// Token: 0x06004C88 RID: 19592 RVA: 0x000D5E8E File Offset: 0x000D408E
		public void ConsumeMilkForOneFeeding()
		{
			this.milkStorage.ConsumeIgnoringDisease(MilkFeederConfig.MILK_TAG, 5f);
		}

		// Token: 0x06004C89 RID: 19593 RVA: 0x00270740 File Offset: 0x0026E940
		public bool IsInCreaturePenRoom()
		{
			Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(base.gameObject);
			return roomOfGameObject != null && roomOfGameObject.roomType == Db.Get().RoomTypes.CreaturePen;
		}

		// Token: 0x0400358F RID: 13711
		public Storage milkStorage;

		// Token: 0x04003590 RID: 13712
		public MeterController storageMeter;
	}
}
