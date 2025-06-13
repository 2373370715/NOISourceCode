using System;
using STRINGS;
using UnityEngine;

{
	{
		base.smi = new UseSolidLubricantChore.Instance(this, target.gameObject);
		this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
		this.AddPrecondition(UseSolidLubricantChore.SolidLubricantIsNotNull, null);
	}

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

	public static void SetOverrideAnimSymbol(UseSolidLubricantChore.Instance smi, bool overriding)
	{
		string text = "lubricant";
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

	public const float LOOP_LENGTH = 6.666f;

	{
		id = "SolidLubricantIsNotNull ",
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return null != context.consumerState.consumer.GetSMI<BionicOilMonitor.Instance>().GetClosestSolidLubricant();
		}
	};

	public class States : GameStateMachine<UseSolidLubricantChore.States, UseSolidLubricantChore.Instance, UseSolidLubricantChore>
	{
		{
			default_state = this.fetch;
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

		public GameStateMachine<UseSolidLubricantChore.States, UseSolidLubricantChore.Instance, UseSolidLubricantChore, object>.FetchSubState fetch;










		{


		}
	}
	public class Instance : GameStateMachine<UseSolidLubricantChore.States, UseSolidLubricantChore.Instance, UseSolidLubricantChore, object>.GameInstance
	{
		public BionicOilMonitor.Instance oilMonitor
		{
			{
				return base.sm.dupe.Get(this).GetSMI<BionicOilMonitor.Instance>();
			}
		}

		public Instance(UseSolidLubricantChore master, GameObject duplicant) : base(master)
		{
		}
}
