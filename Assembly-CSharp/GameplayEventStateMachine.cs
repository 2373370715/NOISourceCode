using System;
using UnityEngine;

// Token: 0x020008E3 RID: 2275
public abstract class GameplayEventStateMachine<StateMachineType, StateMachineInstanceType, MasterType, SecondMasterType> : GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType> where StateMachineType : GameplayEventStateMachine<StateMachineType, StateMachineInstanceType, MasterType, SecondMasterType> where StateMachineInstanceType : GameplayEventStateMachine<StateMachineType, StateMachineInstanceType, MasterType, SecondMasterType>.GameplayEventStateMachineInstance where MasterType : IStateMachineTarget where SecondMasterType : GameplayEvent<StateMachineInstanceType>
{
	// Token: 0x060027DB RID: 10203 RVA: 0x001DF614 File Offset: 0x001DD814
	public void MonitorStart(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, object>.TargetParameter target, StateMachineInstanceType smi)
	{
		GameObject gameObject = target.Get(smi);
		if (gameObject != null)
		{
			gameObject.Trigger(-1660384580, smi.eventInstance);
		}
	}

	// Token: 0x060027DC RID: 10204 RVA: 0x001DF648 File Offset: 0x001DD848
	public void MonitorChanged(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, object>.TargetParameter target, StateMachineInstanceType smi)
	{
		GameObject gameObject = target.Get(smi);
		if (gameObject != null)
		{
			gameObject.Trigger(-1122598290, smi.eventInstance);
		}
	}

	// Token: 0x060027DD RID: 10205 RVA: 0x001DF67C File Offset: 0x001DD87C
	public void MonitorStop(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, object>.TargetParameter target, StateMachineInstanceType smi)
	{
		GameObject gameObject = target.Get(smi);
		if (gameObject != null)
		{
			gameObject.Trigger(-828272459, smi.eventInstance);
		}
	}

	// Token: 0x060027DE RID: 10206 RVA: 0x000AA765 File Offset: 0x000A8965
	public virtual EventInfoData GenerateEventPopupData(StateMachineInstanceType smi)
	{
		return null;
	}

	// Token: 0x020008E4 RID: 2276
	public class GameplayEventStateMachineInstance : GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, object>.GameInstance
	{
		// Token: 0x060027E0 RID: 10208 RVA: 0x000BE602 File Offset: 0x000BC802
		public GameplayEventStateMachineInstance(MasterType master, GameplayEventInstance eventInstance, SecondMasterType gameplayEvent) : base(master)
		{
			this.gameplayEvent = gameplayEvent;
			this.eventInstance = eventInstance;
			eventInstance.GetEventPopupData = (() => base.smi.sm.GenerateEventPopupData(base.smi));
			this.serializationSuffix = gameplayEvent.Id;
		}

		// Token: 0x04001B7E RID: 7038
		public GameplayEventInstance eventInstance;

		// Token: 0x04001B7F RID: 7039
		public SecondMasterType gameplayEvent;
	}
}
