using System;
using STRINGS;

namespace Klei.AI
{
	// Token: 0x02003CA5 RID: 15525
	public class EclipseEvent : GameplayEvent<EclipseEvent.StatesInstance>
	{
		// Token: 0x0600EE4E RID: 61006 RVA: 0x00144580 File Offset: 0x00142780
		public EclipseEvent() : base("EclipseEvent", 0, 0)
		{
			this.title = GAMEPLAY_EVENTS.EVENT_TYPES.ECLIPSE.NAME;
			this.description = GAMEPLAY_EVENTS.EVENT_TYPES.ECLIPSE.DESCRIPTION;
		}

		// Token: 0x0600EE4F RID: 61007 RVA: 0x001445AF File Offset: 0x001427AF
		public override StateMachine.Instance GetSMI(GameplayEventManager manager, GameplayEventInstance eventInstance)
		{
			return new EclipseEvent.StatesInstance(manager, eventInstance, this);
		}

		// Token: 0x0400EA29 RID: 59945
		public const string ID = "EclipseEvent";

		// Token: 0x0400EA2A RID: 59946
		public const float duration = 30f;

		// Token: 0x02003CA6 RID: 15526
		public class StatesInstance : GameplayEventStateMachine<EclipseEvent.States, EclipseEvent.StatesInstance, GameplayEventManager, EclipseEvent>.GameplayEventStateMachineInstance
		{
			// Token: 0x0600EE50 RID: 61008 RVA: 0x001445B9 File Offset: 0x001427B9
			public StatesInstance(GameplayEventManager master, GameplayEventInstance eventInstance, EclipseEvent eclipseEvent) : base(master, eventInstance, eclipseEvent)
			{
			}
		}

		// Token: 0x02003CA7 RID: 15527
		public class States : GameplayEventStateMachine<EclipseEvent.States, EclipseEvent.StatesInstance, GameplayEventManager, EclipseEvent>
		{
			// Token: 0x0600EE51 RID: 61009 RVA: 0x004E65D4 File Offset: 0x004E47D4
			public override void InitializeStates(out StateMachine.BaseState default_state)
			{
				default_state = this.planning;
				base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
				this.planning.GoTo(this.eclipse);
				this.eclipse.ToggleNotification((EclipseEvent.StatesInstance smi) => EventInfoScreen.CreateNotification(this.GenerateEventPopupData(smi), null)).Enter(delegate(EclipseEvent.StatesInstance smi)
				{
					TimeOfDay.Instance.SetEclipse(true);
				}).Exit(delegate(EclipseEvent.StatesInstance smi)
				{
					TimeOfDay.Instance.SetEclipse(false);
				}).ScheduleGoTo(30f, this.finished);
				this.finished.ReturnSuccess();
			}

			// Token: 0x0600EE52 RID: 61010 RVA: 0x004E6680 File Offset: 0x004E4880
			public override EventInfoData GenerateEventPopupData(EclipseEvent.StatesInstance smi)
			{
				return new EventInfoData(smi.gameplayEvent.title, smi.gameplayEvent.description, smi.gameplayEvent.animFileName)
				{
					location = GAMEPLAY_EVENTS.LOCATIONS.SUN,
					whenDescription = GAMEPLAY_EVENTS.TIMES.NOW
				};
			}

			// Token: 0x0400EA2B RID: 59947
			public GameStateMachine<EclipseEvent.States, EclipseEvent.StatesInstance, GameplayEventManager, object>.State planning;

			// Token: 0x0400EA2C RID: 59948
			public GameStateMachine<EclipseEvent.States, EclipseEvent.StatesInstance, GameplayEventManager, object>.State eclipse;

			// Token: 0x0400EA2D RID: 59949
			public GameStateMachine<EclipseEvent.States, EclipseEvent.StatesInstance, GameplayEventManager, object>.State finished;
		}
	}
}
