using System;
using UnityEngine;

// Token: 0x020014AD RID: 5293
public class JetSuitMonitor : GameStateMachine<JetSuitMonitor, JetSuitMonitor.Instance>
{
	// Token: 0x06006D97 RID: 28055 RVA: 0x002FA4DC File Offset: 0x002F86DC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		base.Target(this.owner);
		this.off.EventTransition(GameHashes.PathAdvanced, this.flying, new StateMachine<JetSuitMonitor, JetSuitMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(JetSuitMonitor.ShouldStartFlying));
		this.flying.Enter(new StateMachine<JetSuitMonitor, JetSuitMonitor.Instance, IStateMachineTarget, object>.State.Callback(JetSuitMonitor.StartFlying)).Exit(new StateMachine<JetSuitMonitor, JetSuitMonitor.Instance, IStateMachineTarget, object>.State.Callback(JetSuitMonitor.StopFlying)).EventTransition(GameHashes.PathAdvanced, this.off, new StateMachine<JetSuitMonitor, JetSuitMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(JetSuitMonitor.ShouldStopFlying)).Update(new Action<JetSuitMonitor.Instance, float>(JetSuitMonitor.Emit), UpdateRate.SIM_200ms, false);
	}

	// Token: 0x06006D98 RID: 28056 RVA: 0x000EC6D9 File Offset: 0x000EA8D9
	public static bool ShouldStartFlying(JetSuitMonitor.Instance smi)
	{
		return smi.navigator && smi.navigator.CurrentNavType == NavType.Hover;
	}

	// Token: 0x06006D99 RID: 28057 RVA: 0x000EC6F8 File Offset: 0x000EA8F8
	public static bool ShouldStopFlying(JetSuitMonitor.Instance smi)
	{
		return !smi.navigator || smi.navigator.CurrentNavType != NavType.Hover;
	}

	// Token: 0x06006D9A RID: 28058 RVA: 0x000AA038 File Offset: 0x000A8238
	public static void StartFlying(JetSuitMonitor.Instance smi)
	{
	}

	// Token: 0x06006D9B RID: 28059 RVA: 0x000AA038 File Offset: 0x000A8238
	public static void StopFlying(JetSuitMonitor.Instance smi)
	{
	}

	// Token: 0x06006D9C RID: 28060 RVA: 0x002FA578 File Offset: 0x002F8778
	public static void Emit(JetSuitMonitor.Instance smi, float dt)
	{
		if (!smi.navigator)
		{
			return;
		}
		GameObject gameObject = smi.sm.owner.Get(smi);
		if (!gameObject)
		{
			return;
		}
		int gameCell = Grid.PosToCell(gameObject.transform.GetPosition());
		float num = 0.1f * dt;
		num = Mathf.Min(num, smi.jet_suit_tank.amount);
		smi.jet_suit_tank.amount -= num;
		float num2 = num * 3f;
		if (num2 > 1E-45f)
		{
			SimMessages.AddRemoveSubstance(gameCell, SimHashes.CarbonDioxide, CellEventLogger.Instance.ElementConsumerSimUpdate, num2, 473.15f, byte.MaxValue, 0, true, -1);
		}
		if (smi.jet_suit_tank.amount == 0f)
		{
			smi.navigator.AddTag(GameTags.JetSuitOutOfFuel);
			smi.navigator.SetCurrentNavType(NavType.Floor);
		}
	}

	// Token: 0x04005298 RID: 21144
	public GameStateMachine<JetSuitMonitor, JetSuitMonitor.Instance, IStateMachineTarget, object>.State off;

	// Token: 0x04005299 RID: 21145
	public GameStateMachine<JetSuitMonitor, JetSuitMonitor.Instance, IStateMachineTarget, object>.State flying;

	// Token: 0x0400529A RID: 21146
	public StateMachine<JetSuitMonitor, JetSuitMonitor.Instance, IStateMachineTarget, object>.TargetParameter owner;

	// Token: 0x020014AE RID: 5294
	public new class Instance : GameStateMachine<JetSuitMonitor, JetSuitMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06006D9E RID: 28062 RVA: 0x000EC722 File Offset: 0x000EA922
		public Instance(IStateMachineTarget master, GameObject owner) : base(master)
		{
			base.sm.owner.Set(owner, base.smi, false);
			this.navigator = owner.GetComponent<Navigator>();
			this.jet_suit_tank = master.GetComponent<JetSuitTank>();
		}

		// Token: 0x0400529B RID: 21147
		public Navigator navigator;

		// Token: 0x0400529C RID: 21148
		public JetSuitTank jet_suit_tank;
	}
}
