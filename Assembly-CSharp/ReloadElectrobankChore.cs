using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200072D RID: 1837
public class ReloadElectrobankChore : Chore<ReloadElectrobankChore.Instance>
{
	// Token: 0x0600205C RID: 8284 RVA: 0x001C7DCC File Offset: 0x001C5FCC
	public ReloadElectrobankChore(IStateMachineTarget target) : base(Db.Get().ChoreTypes.ReloadElectrobank, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.personalNeeds, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new ReloadElectrobankChore.Instance(this, target.gameObject);
		this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
		this.AddPrecondition(ReloadElectrobankChore.ElectrobankIsNotNull, null);
	}

	// Token: 0x0600205D RID: 8285 RVA: 0x001C7E30 File Offset: 0x001C6030
	public override void Begin(Chore.Precondition.Context context)
	{
		if (context.consumerState.consumer == null)
		{
			global::Debug.LogError("ReloadElectrobankChore null context.consumer");
			return;
		}
		BionicBatteryMonitor.Instance smi = context.consumerState.consumer.GetSMI<BionicBatteryMonitor.Instance>();
		if (smi == null)
		{
			global::Debug.LogError("ReloadElectrobankChore null RationMonitor.Instance");
			return;
		}
		Electrobank closestElectrobank = smi.GetClosestElectrobank();
		if (closestElectrobank == null)
		{
			global::Debug.LogError("ReloadElectrobankChore null electrobank.gameObject");
			return;
		}
		base.smi.cachedElectrobankSourcePrefabRef = Assets.GetPrefab(closestElectrobank.PrefabID());
		base.smi.sm.electrobankSource.Set(closestElectrobank.gameObject, base.smi, false);
		base.smi.sm.dupe.Set(context.consumerState.consumer, base.smi);
		base.Begin(context);
	}

	// Token: 0x0600205E RID: 8286 RVA: 0x000B9B43 File Offset: 0x000B7D43
	public static bool HasAnyDepletedBattery(ReloadElectrobankChore.Instance smi)
	{
		return smi.batteryMonitor.DepletedElectrobankCount > 0;
	}

	// Token: 0x0600205F RID: 8287 RVA: 0x000B9B53 File Offset: 0x000B7D53
	public static GameObject GetAnyEmptyBattery(ReloadElectrobankChore.Instance smi)
	{
		return smi.batteryMonitor.storage.FindFirst(GameTags.EmptyPortableBattery);
	}

	// Token: 0x06002060 RID: 8288 RVA: 0x001C7EFC File Offset: 0x001C60FC
	public static void RemoveDepletedElectrobank(ReloadElectrobankChore.Instance smi)
	{
		GameObject anyEmptyBattery = ReloadElectrobankChore.GetAnyEmptyBattery(smi);
		if (anyEmptyBattery != null)
		{
			smi.batteryMonitor.storage.Drop(anyEmptyBattery, true);
		}
	}

	// Token: 0x06002061 RID: 8289 RVA: 0x001C7F2C File Offset: 0x001C612C
	public static void InstallElectrobank(ReloadElectrobankChore.Instance smi)
	{
		Storage[] components = smi.gameObject.GetComponents<Storage>();
		for (int i = 0; i < components.Length; i++)
		{
			if (components[i] != smi.batteryMonitor.storage && components[i].FindFirst(GameTags.ChargedPortableBattery) != null)
			{
				components[i].Transfer(smi.batteryMonitor.storage, false, false);
				break;
			}
		}
		Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_BionicBattery, true);
	}

	// Token: 0x06002062 RID: 8290 RVA: 0x001C7FA4 File Offset: 0x001C61A4
	private static void SetStoredItemVisibility(GameObject item, bool visible)
	{
		KBatchedAnimTracker component = item.GetComponent<KBatchedAnimTracker>();
		if (component != null)
		{
			component.enabled = visible;
		}
		Storage.MakeItemInvisible(item, !visible, false);
	}

	// Token: 0x06002063 RID: 8291 RVA: 0x001C7FD4 File Offset: 0x001C61D4
	public static void SetOverrideAnimSymbol(ReloadElectrobankChore.Instance smi, bool overriding, bool hideFromBack, GameObject electrobank)
	{
		string text = "object";
		KBatchedAnimController component = smi.GetComponent<KBatchedAnimController>();
		SymbolOverrideController component2 = smi.gameObject.GetComponent<SymbolOverrideController>();
		if (electrobank != null && hideFromBack)
		{
			ReloadElectrobankChore.SetStoredItemVisibility(electrobank, !overriding);
		}
		if (!overriding)
		{
			component2.RemoveSymbolOverride(text, 0);
			component.SetSymbolVisiblity(text, false);
			return;
		}
		KAnim.Build.Symbol symbolByIndex = ((electrobank != null) ? electrobank.GetComponent<KBatchedAnimController>() : smi.cachedElectrobankSourcePrefabRef.GetComponent<KBatchedAnimController>()).AnimFiles[0].GetData().build.GetSymbolByIndex(0U);
		component2.AddSymbolOverride(text, symbolByIndex, 0);
		component.SetSymbolVisiblity(text, true);
	}

	// Token: 0x04001579 RID: 5497
	public const float LOOP_LENGTH = 4.333f;

	// Token: 0x0400157A RID: 5498
	public static readonly Chore.Precondition ElectrobankIsNotNull = new Chore.Precondition
	{
		id = "ElectrobankIsNotNull",
		description = DUPLICANTS.CHORES.PRECONDITIONS.EDIBLE_IS_NOT_NULL,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return null != context.consumerState.consumer.GetSMI<BionicBatteryMonitor.Instance>().GetClosestElectrobank();
		}
	};

	// Token: 0x0200072E RID: 1838
	public class States : GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore>
	{
		// Token: 0x06002065 RID: 8293 RVA: 0x001C80D0 File Offset: 0x001C62D0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.fetch;
			base.Target(this.dupe);
			this.fetch.InitializeStates(this.dupe, this.electrobankSource, this.pickedUpElectrobank, this.amountRequested, this.actualunits, this.fetchCompleted, null).OnTargetLost(this.electrobankSource, this.electrobankLost);
			this.fetchCompleted.EnterTransition(this.emptyDepleatedBatteries, new StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.Transition.ConditionCallback(ReloadElectrobankChore.HasAnyDepletedBattery)).GoTo(this.install);
			this.emptyDepleatedBatteries.DefaultState(this.emptyDepleatedBatteries.animate);
			this.emptyDepleatedBatteries.animate.ToggleAnims("anim_bionic_kanim", 0f).PlayAnim("discharge", KAnim.PlayMode.Once).Enter("Add Symbol Override", delegate(ReloadElectrobankChore.Instance smi)
			{
				ReloadElectrobankChore.SetOverrideAnimSymbol(smi, true, false, ReloadElectrobankChore.GetAnyEmptyBattery(smi));
			}).Exit("Revert Symbol Override", delegate(ReloadElectrobankChore.Instance smi)
			{
				ReloadElectrobankChore.SetOverrideAnimSymbol(smi, false, false, ReloadElectrobankChore.GetAnyEmptyBattery(smi));
			}).OnAnimQueueComplete(this.emptyDepleatedBatteries.end);
			this.emptyDepleatedBatteries.end.Enter(new StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State.Callback(ReloadElectrobankChore.RemoveDepletedElectrobank)).EnterTransition(this.emptyDepleatedBatteries.animate, new StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.Transition.ConditionCallback(ReloadElectrobankChore.HasAnyDepletedBattery)).GoTo(this.install);
			this.install.DefaultState(this.install.pre).ToggleAnims("anim_bionic_kanim", 0f).Enter("Add Symbol Override", delegate(ReloadElectrobankChore.Instance smi)
			{
				ReloadElectrobankChore.SetOverrideAnimSymbol(smi, true, true, this.pickedUpElectrobank.Get(smi));
			}).Exit("Revert Symbol Override", delegate(ReloadElectrobankChore.Instance smi)
			{
				ReloadElectrobankChore.SetOverrideAnimSymbol(smi, false, true, this.pickedUpElectrobank.Get(smi));
			});
			this.install.pre.PlayAnim("consume_pre", KAnim.PlayMode.Once).OnAnimQueueComplete(this.install.loop).ScheduleGoTo(3f, this.install.loop);
			this.install.loop.PlayAnim("consume_loop", KAnim.PlayMode.Loop).ScheduleGoTo(4.333f, this.install.pst);
			this.install.pst.PlayAnim("consume_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.complete).ScheduleGoTo(3f, this.complete);
			this.complete.Enter(new StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State.Callback(ReloadElectrobankChore.InstallElectrobank)).ReturnSuccess();
			this.electrobankLost.Target(this.dupe).TriggerOnEnter(GameHashes.TargetElectrobankLost, null).ReturnFailure();
		}

		// Token: 0x0400157B RID: 5499
		public GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.FetchSubState fetch;

		// Token: 0x0400157C RID: 5500
		public ReloadElectrobankChore.States.EmptyDepleatedStates emptyDepleatedBatteries;

		// Token: 0x0400157D RID: 5501
		public ReloadElectrobankChore.States.InstallState install;

		// Token: 0x0400157E RID: 5502
		public GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State fetchCompleted;

		// Token: 0x0400157F RID: 5503
		public GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State complete;

		// Token: 0x04001580 RID: 5504
		public GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State electrobankLost;

		// Token: 0x04001581 RID: 5505
		public StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.TargetParameter dupe;

		// Token: 0x04001582 RID: 5506
		public StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.TargetParameter electrobankSource;

		// Token: 0x04001583 RID: 5507
		public StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.TargetParameter lastDepleatedElectrobankFound;

		// Token: 0x04001584 RID: 5508
		public StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.TargetParameter pickedUpElectrobank;

		// Token: 0x04001585 RID: 5509
		public StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.TargetParameter messstation;

		// Token: 0x04001586 RID: 5510
		public StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.FloatParameter actualunits;

		// Token: 0x04001587 RID: 5511
		public StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.FloatParameter amountRequested = new StateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.FloatParameter(1f);

		// Token: 0x0200072F RID: 1839
		public class EmptyDepleatedStates : GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State
		{
			// Token: 0x04001588 RID: 5512
			public GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State animate;

			// Token: 0x04001589 RID: 5513
			public GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State end;
		}

		// Token: 0x02000730 RID: 1840
		public class InstallState : GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State
		{
			// Token: 0x0400158A RID: 5514
			public GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State pre;

			// Token: 0x0400158B RID: 5515
			public GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State loop;

			// Token: 0x0400158C RID: 5516
			public GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.State pst;
		}
	}

	// Token: 0x02000732 RID: 1842
	public class Instance : GameStateMachine<ReloadElectrobankChore.States, ReloadElectrobankChore.Instance, ReloadElectrobankChore, object>.GameInstance
	{
		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x0600206F RID: 8303 RVA: 0x000B9BE2 File Offset: 0x000B7DE2
		public BionicBatteryMonitor.Instance batteryMonitor
		{
			get
			{
				return base.sm.dupe.Get(this).GetSMI<BionicBatteryMonitor.Instance>();
			}
		}

		// Token: 0x06002070 RID: 8304 RVA: 0x000B9BFA File Offset: 0x000B7DFA
		public Instance(ReloadElectrobankChore master, GameObject duplicant) : base(master)
		{
		}

		// Token: 0x04001590 RID: 5520
		public GameObject cachedElectrobankSourcePrefabRef;
	}
}
