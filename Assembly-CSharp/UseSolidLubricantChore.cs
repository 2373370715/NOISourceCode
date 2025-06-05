using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000771 RID: 1905
public class UseSolidLubricantChore : Chore<UseSolidLubricantChore.Instance>
{
	// Token: 0x06002152 RID: 8530 RVA: 0x001CC640 File Offset: 0x001CA840
	public UseSolidLubricantChore(IStateMachineTarget target) : base(Db.Get().ChoreTypes.SolidOilChange, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.personalNeeds, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new UseSolidLubricantChore.Instance(this, target.gameObject);
		this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
		this.AddPrecondition(UseSolidLubricantChore.SolidLubricantIsNotNull, null);
	}

	// Token: 0x06002153 RID: 8531 RVA: 0x001CC6A4 File Offset: 0x001CA8A4
	public override void Begin(Chore.Precondition.Context context)
	{
		if (context.consumerState.consumer == null)
		{
			global::Debug.LogError("ReloadElectrobankChore null context.consumer");
			return;
		}
		BionicOilMonitor.Instance smi = context.consumerState.consumer.GetSMI<BionicOilMonitor.Instance>();
		if (smi == null)
		{
			global::Debug.LogError("ReloadElectrobankChore null RationMonitor.Instance");
			return;
		}
		Pickupable closestSolidLubricant = smi.GetClosestSolidLubricant();
		if (closestSolidLubricant == null)
		{
			global::Debug.LogError("ReloadElectrobankChore null electrobank.gameObject");
			return;
		}
		base.smi.sm.solidLubricantSource.Set(closestSolidLubricant.gameObject, base.smi, false);
		base.smi.sm.dupe.Set(context.consumerState.consumer, base.smi);
		base.Begin(context);
	}

	// Token: 0x06002154 RID: 8532 RVA: 0x001CC75C File Offset: 0x001CA95C
	public static void ConsumeLubricant(UseSolidLubricantChore.Instance smi)
	{
		PrimaryElement component = smi.sm.pickedUpSolidLubricant.Get(smi).GetComponent<PrimaryElement>();
		float num = Mathf.Min(component.Mass, 200f - smi.oilMonitor.oilAmount.value);
		smi.oilMonitor.RefillOil(num);
		if (num >= component.Mass)
		{
			Util.KDestroyGameObject(component.gameObject);
			smi.sm.pickedUpSolidLubricant.Set(null, smi);
			return;
		}
		component.Mass -= num;
	}

	// Token: 0x06002155 RID: 8533 RVA: 0x001CC7E4 File Offset: 0x001CA9E4
	public static void SetOverrideAnimSymbol(UseSolidLubricantChore.Instance smi, bool overriding)
	{
		string text = "lubricant";
		KBatchedAnimController component = smi.GetComponent<KBatchedAnimController>();
		SymbolOverrideController component2 = smi.gameObject.GetComponent<SymbolOverrideController>();
		GameObject gameObject = smi.sm.pickedUpSolidLubricant.Get(smi);
		if (gameObject != null)
		{
			KBatchedAnimTracker component3 = gameObject.GetComponent<KBatchedAnimTracker>();
			if (component3 != null)
			{
				component3.enabled = !overriding;
			}
			Storage.MakeItemInvisible(gameObject, overriding, false);
		}
		if (!overriding)
		{
			component2.RemoveSymbolOverride(text, 0);
			component.SetSymbolVisiblity(text, false);
			return;
		}
		KAnim.Build.Symbol symbolByIndex = gameObject.GetComponent<KBatchedAnimController>().AnimFiles[0].GetData().build.GetSymbolByIndex(0U);
		component2.AddSymbolOverride(text, symbolByIndex, 0);
		component.SetSymbolVisiblity(text, true);
	}

	// Token: 0x0400165C RID: 5724
	public const float LOOP_LENGTH = 6.666f;

	// Token: 0x0400165D RID: 5725
	public static readonly Chore.Precondition SolidLubricantIsNotNull = new Chore.Precondition
	{
		id = "SolidLubricantIsNotNull ",
		description = DUPLICANTS.CHORES.PRECONDITIONS.EDIBLE_IS_NOT_NULL,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return null != context.consumerState.consumer.GetSMI<BionicOilMonitor.Instance>().GetClosestSolidLubricant();
		}
	};

	// Token: 0x02000772 RID: 1906
	public class States : GameStateMachine<UseSolidLubricantChore.States, UseSolidLubricantChore.Instance, UseSolidLubricantChore>
	{
		// Token: 0x06002157 RID: 8535 RVA: 0x001CC8F8 File Offset: 0x001CAAF8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.fetch;
			base.Target(this.dupe);
			this.fetch.InitializeStates(this.dupe, this.solidLubricantSource, this.pickedUpSolidLubricant, this.amountRequested, this.actualunits, this.consume, null).OnTargetLost(this.solidLubricantSource, this.lubricantLost);
			this.consume.DefaultState(this.consume.pre).ToggleAnims("anim_bionic_kanim", 0f).Enter("Add Symbol Override", delegate(UseSolidLubricantChore.Instance smi)
			{
				UseSolidLubricantChore.SetOverrideAnimSymbol(smi, true);
			}).Exit("Revert Symbol Override", delegate(UseSolidLubricantChore.Instance smi)
			{
				UseSolidLubricantChore.SetOverrideAnimSymbol(smi, false);
			});
			this.consume.pre.PlayAnim("lubricate_pre", KAnim.PlayMode.Once).OnAnimQueueComplete(this.consume.loop).ScheduleGoTo(4.7f, this.consume.loop);
			this.consume.loop.PlayAnim("lubricate_loop", KAnim.PlayMode.Loop).ScheduleGoTo(6.666f, this.consume.pst);
			this.consume.pst.PlayAnim("lubricate_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.complete).ScheduleGoTo(3.5f, this.complete);
			this.complete.Enter(new StateMachine<UseSolidLubricantChore.States, UseSolidLubricantChore.Instance, UseSolidLubricantChore, object>.State.Callback(UseSolidLubricantChore.ConsumeLubricant)).ReturnSuccess();
			this.lubricantLost.Target(this.dupe).ReturnFailure();
		}

		// Token: 0x0400165E RID: 5726
		public GameStateMachine<UseSolidLubricantChore.States, UseSolidLubricantChore.Instance, UseSolidLubricantChore, object>.FetchSubState fetch;

		// Token: 0x0400165F RID: 5727
		public UseSolidLubricantChore.States.InstallState consume;

		// Token: 0x04001660 RID: 5728
		public GameStateMachine<UseSolidLubricantChore.States, UseSolidLubricantChore.Instance, UseSolidLubricantChore, object>.State complete;

		// Token: 0x04001661 RID: 5729
		public GameStateMachine<UseSolidLubricantChore.States, UseSolidLubricantChore.Instance, UseSolidLubricantChore, object>.State lubricantLost;

		// Token: 0x04001662 RID: 5730
		public StateMachine<UseSolidLubricantChore.States, UseSolidLubricantChore.Instance, UseSolidLubricantChore, object>.TargetParameter dupe;

		// Token: 0x04001663 RID: 5731
		public StateMachine<UseSolidLubricantChore.States, UseSolidLubricantChore.Instance, UseSolidLubricantChore, object>.TargetParameter solidLubricantSource;

		// Token: 0x04001664 RID: 5732
		public StateMachine<UseSolidLubricantChore.States, UseSolidLubricantChore.Instance, UseSolidLubricantChore, object>.TargetParameter pickedUpSolidLubricant;

		// Token: 0x04001665 RID: 5733
		public StateMachine<UseSolidLubricantChore.States, UseSolidLubricantChore.Instance, UseSolidLubricantChore, object>.TargetParameter messstation;

		// Token: 0x04001666 RID: 5734
		public StateMachine<UseSolidLubricantChore.States, UseSolidLubricantChore.Instance, UseSolidLubricantChore, object>.FloatParameter actualunits;

		// Token: 0x04001667 RID: 5735
		public StateMachine<UseSolidLubricantChore.States, UseSolidLubricantChore.Instance, UseSolidLubricantChore, object>.FloatParameter amountRequested = new StateMachine<UseSolidLubricantChore.States, UseSolidLubricantChore.Instance, UseSolidLubricantChore, object>.FloatParameter(1f);

		// Token: 0x02000773 RID: 1907
		public class InstallState : GameStateMachine<UseSolidLubricantChore.States, UseSolidLubricantChore.Instance, UseSolidLubricantChore, object>.State
		{
			// Token: 0x04001668 RID: 5736
			public GameStateMachine<UseSolidLubricantChore.States, UseSolidLubricantChore.Instance, UseSolidLubricantChore, object>.State pre;

			// Token: 0x04001669 RID: 5737
			public GameStateMachine<UseSolidLubricantChore.States, UseSolidLubricantChore.Instance, UseSolidLubricantChore, object>.State loop;

			// Token: 0x0400166A RID: 5738
			public GameStateMachine<UseSolidLubricantChore.States, UseSolidLubricantChore.Instance, UseSolidLubricantChore, object>.State pst;
		}
	}

	// Token: 0x02000775 RID: 1909
	public class Instance : GameStateMachine<UseSolidLubricantChore.States, UseSolidLubricantChore.Instance, UseSolidLubricantChore, object>.GameInstance
	{
		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x0600215E RID: 8542 RVA: 0x000BA438 File Offset: 0x000B8638
		public BionicOilMonitor.Instance oilMonitor
		{
			get
			{
				return base.sm.dupe.Get(this).GetSMI<BionicOilMonitor.Instance>();
			}
		}

		// Token: 0x0600215F RID: 8543 RVA: 0x000BA450 File Offset: 0x000B8650
		public Instance(UseSolidLubricantChore master, GameObject duplicant) : base(master)
		{
		}
	}
}
