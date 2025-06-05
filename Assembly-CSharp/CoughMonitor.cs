using System;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x02001589 RID: 5513
public class CoughMonitor : GameStateMachine<CoughMonitor, CoughMonitor.Instance, IStateMachineTarget, CoughMonitor.Def>
{
	// Token: 0x060072C9 RID: 29385 RVA: 0x0030DA0C File Offset: 0x0030BC0C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.idle;
		this.idle.EventHandler(GameHashes.PoorAirQuality, new GameStateMachine<CoughMonitor, CoughMonitor.Instance, IStateMachineTarget, CoughMonitor.Def>.GameEvent.Callback(this.OnBreatheDirtyAir)).ParamTransition<bool>(this.shouldCough, this.coughing, (CoughMonitor.Instance smi, bool bShouldCough) => bShouldCough);
		this.coughing.ToggleStatusItem(Db.Get().DuplicantStatusItems.Coughing, null).ToggleReactable((CoughMonitor.Instance smi) => smi.GetReactable()).ParamTransition<bool>(this.shouldCough, this.idle, (CoughMonitor.Instance smi, bool bShouldCough) => !bShouldCough);
	}

	// Token: 0x060072CA RID: 29386 RVA: 0x0030DAE8 File Offset: 0x0030BCE8
	private void OnBreatheDirtyAir(CoughMonitor.Instance smi, object data)
	{
		float timeInCycles = GameClock.Instance.GetTimeInCycles();
		if (timeInCycles > 0.1f && timeInCycles - smi.lastCoughTime <= 0.1f)
		{
			return;
		}
		float num = (float)data;
		float num2 = (smi.lastConsumeTime <= 0f) ? 0f : (timeInCycles - smi.lastConsumeTime);
		smi.lastConsumeTime = timeInCycles;
		smi.amountConsumed -= 0.05f * num2;
		smi.amountConsumed = Mathf.Max(smi.amountConsumed, 0f);
		smi.amountConsumed += num;
		if (smi.amountConsumed >= 1f)
		{
			this.shouldCough.Set(true, smi, false);
			smi.lastConsumeTime = 0f;
			smi.amountConsumed = 0f;
		}
	}

	// Token: 0x04005612 RID: 22034
	private const float amountToCough = 1f;

	// Token: 0x04005613 RID: 22035
	private const float decayRate = 0.05f;

	// Token: 0x04005614 RID: 22036
	private const float coughInterval = 0.1f;

	// Token: 0x04005615 RID: 22037
	public GameStateMachine<CoughMonitor, CoughMonitor.Instance, IStateMachineTarget, CoughMonitor.Def>.State idle;

	// Token: 0x04005616 RID: 22038
	public GameStateMachine<CoughMonitor, CoughMonitor.Instance, IStateMachineTarget, CoughMonitor.Def>.State coughing;

	// Token: 0x04005617 RID: 22039
	public StateMachine<CoughMonitor, CoughMonitor.Instance, IStateMachineTarget, CoughMonitor.Def>.BoolParameter shouldCough = new StateMachine<CoughMonitor, CoughMonitor.Instance, IStateMachineTarget, CoughMonitor.Def>.BoolParameter(false);

	// Token: 0x0200158A RID: 5514
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200158B RID: 5515
	public new class Instance : GameStateMachine<CoughMonitor, CoughMonitor.Instance, IStateMachineTarget, CoughMonitor.Def>.GameInstance
	{
		// Token: 0x060072CD RID: 29389 RVA: 0x000EFADF File Offset: 0x000EDCDF
		public Instance(IStateMachineTarget master, CoughMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x060072CE RID: 29390 RVA: 0x0030DBB0 File Offset: 0x0030BDB0
		public Reactable GetReactable()
		{
			Emote cough_Small = Db.Get().Emotes.Minion.Cough_Small;
			SelfEmoteReactable selfEmoteReactable = new SelfEmoteReactable(base.master.gameObject, "BadAirCough", Db.Get().ChoreTypes.Cough, 0f, 0f, float.PositiveInfinity, 0f);
			selfEmoteReactable.SetEmote(cough_Small);
			selfEmoteReactable.preventChoreInterruption = true;
			return selfEmoteReactable.RegisterEmoteStepCallbacks("react_small", null, new Action<GameObject>(this.FinishedCoughing));
		}

		// Token: 0x060072CF RID: 29391 RVA: 0x0030DC3C File Offset: 0x0030BE3C
		private void FinishedCoughing(GameObject cougher)
		{
			cougher.GetComponent<Effects>().Add("ContaminatedLungs", true);
			base.sm.shouldCough.Set(false, base.smi, false);
			base.smi.lastCoughTime = GameClock.Instance.GetTimeInCycles();
		}

		// Token: 0x04005618 RID: 22040
		[Serialize]
		public float lastCoughTime;

		// Token: 0x04005619 RID: 22041
		[Serialize]
		public float lastConsumeTime;

		// Token: 0x0400561A RID: 22042
		[Serialize]
		public float amountConsumed;
	}
}
